# 第 08 章 App 启动序列与生命周期收尾

> 本章解决：把「进程起来 → 主题播种 → 首帧颜色正确 → 退出干净无泄漏」这条时序固定成一份可抄的 `App.xaml.cs`。
> 读者：AI Agent（要在本项目之外新建/续写 WPF 业务代码）
> 前置章节：第 7 章（工程配置）、第 12 章（主题引擎生效时序，本章只给结论与操作顺序）

## 8.1 目标与验收

1. 应用第一帧就是正确主题色，不出现「先白一下再变深」。
2. 任何后台线程/Dispatcher 里的未处理异常都落进 `<app>-errors.log`，运行期**不弹窗自证**。
3. 退出后无残留：无托盘幽灵图标、无 `SystemEvents` 订阅泄漏、无挂在 `UI4Theme.ThemeChanged` 上的死窗口。
4. `UI4Theme.Persistence` 可选接入：重启后主题与上次一致。

判据：连续开关应用 5 次，`<app>-errors.log` 不存在且托盘无残留图标；`ThemeChanged` 订阅数在关闭主窗后归零（用 8.3.5 的计数法验证）。

## 8.2 源码依据

- `samples/StartUI4Demo/App.xaml:4-5` —— `StartupUri="MainWindow.xaml"` + `ShutdownMode="OnMainWindowClose"`；`:6-19` 两个样板样式（见第 10 章）。
- `samples/StartUI4Demo/App.xaml.cs:12-21` —— `OnStartup` 里先 `base.OnStartup(e)` 再 `UI4Theme.ApplyToApplication()`，随后注册两个异常钩子；`:35-51` 异常落盘写法。
- `src/StartUI4Controls/UI4Theme.cs:69-73` —— 静态构造顺带 `UI4WindowTitleBar.Install()`；`:283` `ApplyToApplication`；`:285-291` 写入 `Application.Resources`（`Application.Current == null` 时静默跳过）；`:297-309` 键集合与三个别名。
- `src/StartUI4Controls/UI4Theme.cs:236-251` —— `EnableSystemFollow`/`DisableSystemFollow`/公开 `ReleaseSystemFollow()`。
- `src/StartUI4Controls/UI4Theme.cs:315-339` —— `Persistence`、`Save()`、`ApplyPersisted()`、`ThemeSaved`/`ThemeLoading`。
- `src/StartUI4Controls/UI4ContextMenu.cs:277` 构造订阅 `ThemeChanged`；`:364-373` `Detach()` 内退订并摘右键钩子。
- `samples/StartUI4Demo/MainWindow.xaml.cs:296-301` —— `MainWindow_Closed` 收尾三件事；`:43` 与 `ScopeWindow.xaml.cs:24` 是**用 lambda 订阅 `ThemeChanged` 且不退订**的反例。
- `src/StartUI4Controls/UI4NotifyIcon.cs:85` `VisibilityProperty.OverrideMetadata`；`:104` 订阅 `Application.Current.Exit`；`:627-648` `Dispose()` 逐项清理。

## 8.3 正文

### 8.3.1 推荐启动序列（顺序不可换）

```
1. App.xaml 上设 ShutdownMode（普通业务应用用 OnMainWindowClose；托盘常驻用 OnExplicitShutdown）
2. OnStartup 内：base.OnStartup(e)
3. （可选）UI4Theme.Persistence = new RegistryThemePersistence();   ← 必须先赋值
4. UI4Theme.ApplyToApplication();                                    ← 播种资源键
5. （可选）UI4Theme.ApplyPersisted();                                ← 依赖第 3 步，否则恒返回 false
6. 注册 DispatcherUnhandledException + AppDomain.CurrentDomain.UnhandledException
7. 创建主窗（StartupUri 或自己 new + Show()）
```

`StartupUri` 的窗口在 `OnStartup` 返回之后才被创建，因此第 2–6 步天然早于第一次 XAML 解析 —— 这就是「`ApplyToApplication()` 必须在第一次解析主题色之前」这条约束能靠 `OnStartup` 满足的原因。若你改成在 `App` 构造函数里调 `ApplyToApplication()`，`Application.Current` 可能还不是你的实例，`UI4Theme.cs:287-288` 的 `app != null` 判断会让这次播种**静默失效**（不报错、键缺失）。

若你自己 `new MainWindow().Show()`，就不要保留 `StartupUri`，否则会起两个主窗；此时 `ShutdownMode="OnMainWindowClose"` 认的是 `Application.MainWindow`（第一个显示的窗口）。

### 8.3.2 为什么键要先播种

`ApplyToApplication()` 做的事只有一件：把 30 个令牌写成 `UI4.Color.<Token>` / `UI4.Brush.<Token>`，外加 `UI4.Brush.Text` / `UI4.Brush.Border` / `UI4.Brush.Accent` 三个别名，全部塞进 `Application.Resources`（`UI4Theme.cs:297-309`）。不播种会怎样：

- `{DynamicResource UI4.Brush.Background}` 取不到值时**不抛异常**，解析结果为空 —— 窗口背景落回默认（透明/系统色），首帧观感错误，且没有任何日志提示。这是最难查的一类 bug。
- `{StaticResource UI4.Brush.Background}` 取不到值时**在解析期直接抛**（「找不到名为 X 的资源」），整窗 XAML 加载失败。所以「漏播种 + 误用 StaticResource」的组合会让你看到一堵红，而不是颜色不对。
- 只有第一次**真的发生主题变更**（`ApplyResolved` 里 `resolvedChanged` 为 true）才会补写资源 —— 而 `ApplyToApplication()` 与每次 `ApplyResolved` 末尾的 `WriteToApplicationResources()`（`:186`）都是同步写入，不等批处理。

补播种不解决问题：`DynamicResource` 会在键出现后自动更新，但 `StaticResource` 已在解析期快照，永远停在旧值（或抛错）。所以第 4 步之后，全工程的主题色一律 `DynamicResource`。

### 8.3.3 知道 `UI4Theme` 静态构造有副作用

任何时候第一次触碰 `UI4Theme` 的任意静态成员（包括只读 `UI4Theme.Current`），静态构造就会执行 `UI4WindowTitleBar.Install()`（`UI4Theme.cs:69-73`），后者订阅 `ThemeChanged → ApplyOpenWindows()`。含义：

- 你不需要为标题栏染色写任何代码，但也**无法关掉这条通路**；想让某个窗口豁免，用 `ui:UI4WindowTitleBar.Enabled="False"` 附加属性（见第 17 章）。
- 因此「先读一个主题属性、之后再决定要不要装钩子」这种延迟初始化想法不成立。要初始化就按 8.3.1 的顺序显式初始化。
- 在 App 之外的静态初始化器里读 `UI4Theme.*` 是安全的（`Install` 幂等，`_installed` 保护）。

### 8.3.4 异常落盘 sink

`App.xaml.cs` 里两个钩子都要注册：`DispatcherUnhandledException` 覆盖 UI 线程（包括 XAML 模板内抛出的异常），`AppDomain.CurrentDomain.UnhandledException` 覆盖后台线程。写日志照 Demo 的 `File.AppendAllText` + `Encoding.UTF8`，但**去掉弹窗**：Demo 弹 `MessageBox` 是控件排查场景的取舍，业务应用会在无人值守时把进程卡死，也会污染自动化验证。运行期自证改用窗内文本（第 9 章的 `StatusText`）与日志。

### 8.3.5 退出序列清单

主窗 `Closed`（或 `OnExit`）里按序做：

1. `_hostMenu.Detach()` —— 这是 `UI4ContextMenu` 唯一的退订入口：构造订阅 `UI4Theme.ThemeChanged`（`UI4ContextMenu.cs:277`），只有 `Detach()` 会退订（`:366`）。只用过 `Attach()` 却从不 `Detach()` 的实例会永久挂在静态事件上。
2. `TrayIcon.Visibility = Visibility.Collapsed;` 然后 `TrayIcon.Dispose();` —— 顺序不能颠倒也不能省第一步：`Visibility` 是它的开关（`OverrideMetadata` 里 Visible→Create、非 Visible→Remove），`Dispose()` 负责删图标、停 100ms 定时器、退订 `Application.Exit`、释放内部隐藏消息 `HwndSource`（`UI4NotifyIcon.cs:627-648`）。
3. 退订你自己订阅的 `UI4Theme.ThemeChanged`。**必须用字段保存委托**，lambda 无法退订（Demo 的 `MainWindow.xaml.cs:43`、`ScopeWindow.xaml.cs:24` 正是反例：每开一个 `ScopeWindow` 就多一条永久订阅，窗口 GC 掉但委托链仍在）。
4. `UI4Theme.ReleaseSystemFollow()` —— 只要用过 `UI4ThemeMode.System` 就必须调；`SystemEvents.UserPreferenceChanged` 是进程级静态事件，不退订在长会话宿主里会泄漏并被反复回调（`UI4Theme.cs:240-251`）。没用过 System 模式时调用也无害。
5. 需要保存主题偏好则 `UI4Theme.Save()`（未配置 `Persistence` 时只触发 `ThemeSaved`，不写任何地方）。
6. 关掉你自己创建的模态/子窗引用、释放 `IDisposable` 业务对象。

订阅计数自查：把 `UI4Theme` 的静态事件用反射数一次订阅者不体面；改用「每次 `ThemeChanged` 回调里打一行带窗口 `IsLoaded` 的日志」，退出主窗后再切一次全局主题（若进程仍在），日志不再增长即说明没有僵尸 handler。

### 8.3.6 完整 App.xaml.cs 模板（C# 7.3）

```csharp
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using StartUI4Controls;

namespace MyApp
{
    public partial class App : Application
    {
        public static readonly string ErrorLogPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "myapp-errors.log");

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 播种资源键：必须早于第一次解析主题色的 XAML
            UI4Theme.Persistence = new RegistryThemePersistence();
            UI4Theme.ApplyToApplication();
            if (!UI4Theme.ApplyPersisted()) UI4Theme.SetTheme(UI4ThemeMode.Light);

            DispatcherUnhandledException += OnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += OnDomainUnhandledException;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            UI4Theme.Save();
            UI4Theme.ReleaseSystemFollow();
            base.OnExit(e);
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Report("UI thread", e.Exception);
            e.Handled = true;   // 保持进程存活；若要崩溃即退，改为不置 Handled
        }

        private void OnDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Report("AppDomain", e.ExceptionObject as Exception);
        }

        public static void Report(string source, Exception ex)
        {
            string text = ex == null ? "(null)" : ex.ToString();
            try
            {
                File.AppendAllText(ErrorLogPath,
                    string.Format("[{0:yyyy-MM-dd HH:mm:ss}] {1}{2}{3}{4}",
                        DateTime.Now, source, Environment.NewLine, text, Environment.NewLine),
                    Encoding.UTF8);
            }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
        }
    }
}
```

主窗收尾模板（代码后置，`Closed` 事件在 XAML 里挂 `Closed="MainWindow_Closed"`）：

```csharp
private void MainWindow_Closed(object sender, EventArgs e)
{
    if (_hostMenu != null) { _hostMenu.Detach(); _hostMenu = null; }
    UI4Theme.ThemeChanged -= OnGlobalThemeChanged;   // 字段委托，不是 lambda
    TrayIcon.Visibility = Visibility.Collapsed;
    TrayIcon.Dispose();
    UI4Theme.ReleaseSystemFollow();
}

private void OnGlobalThemeChanged(object sender, EventArgs args) { /* 刷新业务侧回显 */ }
```

`XAML 解析期早触发的事件必须空判`这条纪律同样适用于这里：`Closed` 里访问的字段可能因构造中途抛异常而未赋值，全部判空后再用（`MainWindow.xaml.cs:98-103` 同理）。

### 8.3.7 多窗口与单实例

本库**不提供**单实例能力：没有 `Mutex` 封装、没有 `WM_COPYDATA` 转发、没有第二实例检测。要单实例就自己在 `OnStartup` 最前面做：命名 `Mutex` 探测 → 已存在时通过命名管道或 `FindWindow` + 自定义消息让老实例 `Activate()` + 把命令行参数交给它 → `Shutdown()`。要点：

- 单实例检查要放在第 3–4 步**之前**，否则第二个实例已经把主题写进注册表才退出。
- `--tab=N` 这类「第二个实例带参数唤起」的转发，正好复用第 9 章的启动参数解析函数。
- 多窗口：`ShutdownMode` 决定行为。`OnMainWindowClose` 下关主窗即退出（副窗随之销毁）；托盘常驻应用用 `OnExplicitShutdown` + 显式 `Shutdown()`，并在 `MainWindow_Closed` 里先处理托盘再 `Shutdown()`。
- 不含任何 UI4 控件的窗口不会自动染标题栏，需要 `UI4WindowTitleBar.Apply(this)`（`README.md 第十章 §6`）。

## 8.4 推荐做法（Do / Don't）

- Do：`ApplyToApplication()` 放在 `OnStartup` 内 `base.OnStartup(e)` 之后，作为访问 `UI4Theme` 的第一个动作。
- Do：`Persistence` 在 `ApplyPersisted()` 之前赋值；两者成对出现在同一个方法里，一眼能看出依赖。
- Do：日志文件名跟 `AssemblyName`（`myapp-errors.log`），并在自动化脚本里以「文件是否存在」判定运行期错误。
- Do：`ThemeChanged` 一律用方法 + `-=` 退订；一次性探测才用 lambda。
- Don't：不要在 `App` 构造函数里播种资源（`Application.Current` 未就绪，静默失效）。
- Don't：不要用 `MessageBox` 做运行期自证或异常提示 —— 弹窗会阻塞 Dispatcher，让后续验证脚本超时。
- Don't：不要先 `Dispose()` 再改 `Visibility`：那时图标已删、隐藏消息窗口已释放，`Visibility` 变更反而可能再走一次 Create。
- Don't：不要指望 `UI4ContextMenu` 被 GC 就自动退订 —— 静态事件持有它，只有 `Detach()` 解套。

## 8.5 坑与排错

| 症状 | 真因 | 处置 |
|---|---|---|
| 首帧颜色不对（背景/文字发暗或透明），切一次主题后正常 | 没播种：`Application.Resources` 里没有 `UI4.Brush.*` 键 | 在 `OnStartup` 里补 `UI4Theme.ApplyToApplication()`，确认它早于主窗创建 |
| 启动即抛「找不到名为 `UI4.Brush.Background` 的资源」 | 播种缺失 + 用了 `StaticResource`（解析期快照） | 改 `DynamicResource`，并补播种 |
| 在 App 构造函数调了 `ApplyToApplication()` 却仍然没键 | `Application.Current` 尚未指向你的实例，`UI4Theme.cs:287-288` 静默跳过 | 挪到 `OnStartup` |
| 退出后托盘留幽灵图标 | 没先 `Visibility=Collapsed`，或根本没 `Dispose()` | 按 8.3.5 第 2 步补两行 |
| 改系统亮/暗设置后应用抛异常或行为异常，进程退出后其他程序收到回调 | 用过 `System` 模式却没 `ReleaseSystemFollow()` | 在 `Closed`/`OnExit` 里补调 |
| 打开异主题窗口 N 次后，切主题明显变慢 | 每次 new 窗口都用 lambda 订阅了 `ThemeChanged` 且不退订 | 改字段委托 + `Closed` 退订，或在 `Window.Closed` 里 `this` 相关订阅全清 |
| `ApplyPersisted()` 恒返回 false | `Persistence` 还是默认 null | 先赋 `RegistryThemePersistence()`/`JsonThemePersistence(path)` |
| 自定义主题键下窗底显示 `CurrentMode` 显示 `Light` | 自定义键一律被映射为 `Light`（`UI4Theme.cs:141-146`） | 状态回显改比 `ResolvedKey`（见第 12 章） |

## 8.6 完成判据

1. `OnStartup` 内顺序为：`base.OnStartup` →（`Persistence`）→ `ApplyToApplication()` →（`ApplyPersisted()`）→ 异常钩子；对照 8.3.1 逐行核。
2. 首次启动不做任何主题操作，主窗背景/标题栏即为期望主题色（截图或读 `Window.Background` 解析值）。
3. 全局搜工程源码：`StaticResource UI4.` 出现 0 次；`ApplyToApplication` 出现 1 次且在 `App` 里。
4. 异常日志路径由 `AppDomain.CurrentDomain.BaseDirectory` 拼出，文件名含你的 exe 名；钩子注册两处，`DispatcherUnhandledException` 里显式置 `e.Handled`。
5. `MainWindow_Closed`（或 `OnClosed`）里能数出四件事：`_hostMenu.Detach()`、`TrayIcon` 先 Collapsed 再 Dispose、`ThemeChanged -= 具名方法`、`ReleaseSystemFollow()`。
6. 连开连关 5 次：托盘无残留图标、`<app>-errors.log` 不存在、任务管理器无遗留进程。
7. 全工程搜 `new Mutex`/单实例相关代码：若产品要求单实例，实现落在 `App.OnStartup` 且在主题播种之前；若不需要，明确记录「本库不提供单实例」。
