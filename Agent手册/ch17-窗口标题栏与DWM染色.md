# 第 17 章 窗口标题栏与 DWM 染色

> 本章解决：让系统绘制的标题栏/边框跟随主题，覆盖含「纯 WPF 窗口」在内的全部场景，并给出可执行的验证手段。
> 读者：AI Agent（要在本项目之外新建/续写 WPF 业务代码）
> 前置章节：第 12 章（ThemeChanged 时序）、第 14 章（作用域根为 Window）。选型论证与改动史见 `主题方案分析与改进.md` 十一/十二节，勿重复阅读成本。

## 17.1 目标与验收

- 说清 DWM、非客户区/客户区分界，以及为什么 WPF 代码够不着标题栏。
- 掌握 `UI4WindowTitleBar` 全部公开面与所用 DWM 属性、COLORREF 编码。
- 能判断当前窗口吃的是四条通路里的哪一条，纯窗口知道宿主义务在哪。
- 会用「能力探测 + 回读不对称」的正确验证手段，避开已否掉的方案。
- 验收判据：切主题后 `DwmGetWindowAttribute` 回读深浅标志翻转；`demo-errors.log` 不存在。

## 17.2 源码依据

- `src/StartUI4Controls/UI4WindowTitleBar.cs` 全文（201 行）。关键点位：`:27-32` 属性常量与撤销哨兵；`:39-48` `EnabledProperty`/`Get/SetEnabled`；`:64` `SupportsCaptionColors`；`:67-72` `Install`（internal）；`:77-86` `NotifyContentLoaded`（internal，`ThemeVersion` 去重）；`:102-111` `Apply(Window)→bool`；`:114-119` `ApplyOpenWindows()`；`:129-141` `Revert`；`:143-161` `SetDarkMode`；`:163-178` `SetCaptionColors`；`:181-185` 深浅亮度判定；`:191-194` `ToColorRef`。
- `src/StartUI4Controls/UI4Theme.cs:69-73` — 静态构造首次触碰主题即 `Install()`。
- `src/StartUI4Controls/UI4ThemeScope.cs:149-151` — 作用域根为 Window 时补染。
- 仓库根 `titlebar.ps1`（进程内 31 项断言）、`titlebar-live.ps1`（跨进程 9 项）、`theme.ps1`（以 DWM 标志为状态判据）。

## 17.3 正文

### 17.3.1 概念：非客户区归 DWM

窗口分两块：WPF 只绘制**客户区**；标题栏、边框、圆角、投影属于**非客户区**，由 DWM（Desktop Window Manager，Vista 起的桌面合成组件）在合成阶段绘制。`WM_NCPAINT` 自绘在合成后失效，程序唯一的官方通道是 `dwmapi.dll!DwmSetWindowAttribute`（`:35-36` P/Invoke 声明）。因此「标题栏跟主题」不是样式问题，而是给 DWM 设置窗口属性。

### 17.3.2 公开 API 面

- `bool Apply(Window window)`：按该窗**有效主题**（`EffectiveThemeFor(window) ?? Current`，含作用域解析）立即染色。幂等，可随时手调。返回 true 的条件：HWND 已创建、未被 `Enabled` 豁免、深浅标志设置成功（`:102-111`）。
- `ApplyOpenWindows()`：遍历 `Application.Current.Windows` 逐窗 `Apply`（`:114-119`）。
- `bool SupportsCaptionColors`：本机是否支持标题栏任意配色（Win11 起；首试后按 HRESULT 缓存，`:53,64`）。
- `int ToColorRef(Color)`：`0x00BBGGRR` 转换，alpha 丢弃（`:191-194`）。宿主自己调 dwmapi 时复用它保证一致。
- 附加属性 `Enabled`（bool，默认 true）+ `Get/SetEnabled`：`ui:UI4WindowTitleBar.Enabled="False"` 即时撤销该窗染色（`:39-48, 88-96, 129-141`）。

### 17.3.3 DWM 属性表

| 属性 | 编号 | 值来源 | 说明 |
|---|---|---|---|
| `DWMWA_USE_IMMERSIVE_DARK_MODE` | 20（20H1 前为 19） | 底色亮度判定 | 深/浅标题栏标志；19/20 互为回退（`:143-161`） |
| `DWMWA_CAPTION_COLOR` | 35 | `Background` 令牌 | 标题栏底色（`:166-168`） |
| `DWMWA_TEXT_COLOR` | 36 | `TextForeground` 令牌 | 标题文字色（`:172-173`） |
| `DWMWA_BORDER_COLOR` | 34 | `BorderNormal` 令牌 | 窗口边框色（`:174-175`） |
| 撤销 | 写 `DWMWA_COLOR_DEFAULT = 0x01000000` 哨兵 | — | 把 34/35/36 交还系统默认（`:32, 129-141`） |

颜色一律 `COLORREF`（`0x00BBGGRR`），不是 WPF 的 `0xAARRGGBB`。深浅判定不枚举主题键：底色 `0.299R+0.587G+0.114B < 128` 即暗（`:181-185`），自定义主题自动适用。

### 17.3.4 四条生效通道与时序

- 通道 A（全局清扫）：`UI4Theme.ThemeChanged` 触发 `ApplyOpenWindows()`（`Install` 订阅，`:71`；时序在控件刷新批处理之后，见 `UI4Theme.cs:458-492`）。覆盖：一切已打开窗口在切主题/`SetAccent` 后重染。
- 通道 B（控件 Loaded 补染）：任一 UI4 控件 `Loaded` 时经 `NotifyContentLoaded` 染所属窗口；同一 `ThemeVersion` 内每窗口只调一次 dwmapi，去重表 `ConditionalWeakTable<Window, PaintedVersion>`（`:54-61, 77-86`）。覆盖：主题先行、窗口后开（新窗内容 Loaded 即补）。
- 通道 C（作用域根）：`UI4ThemeScope` 根为 `Window` 时按其有效主题单窗重染（`UI4ThemeScope.cs:149-151`）。覆盖：第 14 章异主题窗口。
- 通道 D（手动兜底）：宿主直接 `Apply(window)` / `ApplyOpenWindows()`。

### 17.3.5 宿主义务：纯窗口必须自己调一次 Apply

不含任何 UI4 控件的窗口（如纯 WPF 的登录窗、报告窗）触发不了通道 B；若它打开时全局主题早已是深色，也只是等下一次通道 A。**宿主义务：为这类窗口显式调 `Apply`，最佳调用点是 `SourceInitialized`**——此时 HWND 已创建；`Show()` 之前 `WindowInteropHelper.Handle` 为 `IntPtr.Zero`，`Apply` 直接返回 false（`:105-106, 196-199`）：

```csharp
// C# 7.3
public PlainReportWindow()
{
    SourceInitialized += (s, e) => UI4WindowTitleBar.Apply(this);
    UI4Theme.ThemeChanged += (s, e) => UI4WindowTitleBar.Apply(this); // 此后主题变化也不漏
}
```

### 17.3.6 豁免与能力探测

截图/投屏/录制窗口的标题栏颜色属于画面内容，要钉死为系统默认：XAML 写 `ui:UI4WindowTitleBar.Enabled="False"`（即时 `Revert`，恢复深浅标志 off + 三属性回哨兵）。能力探测一律「先试、按 HRESULT 定论」并缓存：`SetDarkMode` 先试 20 失败退 19（`:143-161`），`SetCaptionColors` 首试 35 失败即整级判 2（不支持）永不再试（`:163-178`）。**不能用版本号**：未带 app.manifest 的进程 `Environment.OSVersion` 会虚报 6.3（类注释 `:22-23`；Demo 工程带 manifest 的动机之一见 `事实速查.md` §0/§7-14）。

### 17.3.7 回读不对称与验证手段

实测（`事实速查.md` §5）：`DwmGetWindowAttribute` 从外部进程**能**回读深浅标志（19/20），**不能**回读 34/35/36（报 `0x80070057` E_INVALIDARG）。因此：
- 硬证据 = 深浅标志。`theme.ps1` 就以 DWM 标志为状态判据。
- 底色/文字/边框色没有外部回读通道，只能进程内断言（比对写入前 `ToColorRef` 期望值）或跨进程截图辅助（本机 WPF 抓屏常全白，通用坑 §2，不能当唯一判据）。
- 新应用最小读回代码（宿主进程内跑即可）：

```csharp
// C# 7.3：验证深浅标志（属性 20 失败则按 19 再读一次）
[DllImport("dwmapi.dll")]
static extern int DwmGetWindowAttribute(IntPtr hwnd, int attr, out int val, int size);
bool IsTitlebarDark(Window w)
{
    IntPtr h = new System.Windows.Interop.WindowInteropHelper(w).Handle;
    int v;
    if (DwmGetWindowAttribute(h, 20, out v, 4) == 0) return v == 1;
    if (DwmGetWindowAttribute(h, 19, out v, 4) == 0) return v == 1;
    throw new InvalidOperationException("系统无法回读深色标志");
}
```

脚本腿：仓库根 `titlebar.ps1`（进程内 31 项，覆盖三通道 + 手动兜底）、`titlebar-live.ps1`（起真实 Demo 跨进程 9 项，验异主题窗口与全局高对比度）；均按 `powershell -STA -ExecutionPolicy Bypass -File <脚本>` 运行。

### 17.3.8 被否掉的 RegisterClassHandler 教训

曾尝试 `EventManager.RegisterClassHandler(typeof(Window), LoadedEvent, …)` 做零侵入全局补染：在 PowerShell STA 承载下实测**从不触发**，无法验证的通道不能作为产品路径（`事实速查.md` §5）。本章所有自动通道（A/B/C）都有可回读的判据（DWM 标志、p3verify/titlebar 断言）；新加的染色通路必须先回答「怎么证明它跑了」。

### 17.3.9 失败降级矩阵

| 环境 | 表现 | 判据 |
|---|---|---|
| Win11+ | 深浅标志 + 35/36/34 全生效 | `SupportsCaptionColors == true` |
| Win10 20H1~ | 仅深浅标志（属性 20）；35 首试失败→缓存 2，标题栏为系统深/浅默认色 | `SupportsCaptionColors == false`，回读标志仍翻转 |
| Win10 早期（<20H1） | 深浅标志走属性 19 | 首试 20 失败后 19 成功（`:147-159` 回退逻辑） |
| dwmapi 不可用/超旧系统 | `Apply` 返回 false，标题栏保持系统原样，无异常 | `Apply(w) == false` |
| 被豁免窗口 | 深浅标志 off + 哨兵复位 | `GetEnabled(w) == false` |

## 17.4 推荐做法（Do / Don't）

- Do：常规业务窗什么都不做（A/B/C 覆盖）；纯窗口按 17.3.5 在 `SourceInitialized` 调 `Apply`。
- Do：验证一律以 19/20 深浅标志为外部硬证据；配色正确性用进程内断言。
- Do：截图/录制窗用 `Enabled="False"` 豁免，而不是绕过 `Apply` 的时机。
- Don't：用 `Environment.OSVersion` 或版本号决定染色策略。
- Don't：把 34/35/36 的写入成功当「可回读」证据去写测试——它们拒绝回读。
- Don't：新建任何「不可验证」的全局事件通路替代 A/B/C（17.3.8）。

## 17.5 坑与排错

- 症状：`Apply` 返回 false。根因三选一：窗口尚未 `Show`（无 HWND）、`Enabled=false` 被豁免、深浅属性全被拒（超旧系统）。判据顺序：`new WindowInteropHelper(w).Handle != IntPtr.Zero` → `GetEnabled(w)` → 在 `SourceInitialized` 里重试一次。
- 症状：切主题后老窗口标题栏不变。根因：`ThemeChanged` 批处理未到（`Dispatcher.BeginInvoke(Input)`，须先泵到 `ContextIdle` 再观察，通用坑 §4）。判据：泵一轮后读 DWM 标志。
- 症状：标题栏深色了但底色仍是系统灰。属预期——Win10 上 35 被拒（降级矩阵第二行），不是 bug。
- 症状：异主题作用域窗口标题栏跟的是全局色。根因：窗内没有任何元素、从未触发作用域刷新。处置：`SetTheme` 一次即走通道 C，或手动 `Apply(this)`。
- 症状：PowerShell 承载的宿主里标题栏永不变。根因：命中 17.3.8 所述类处理器不触发的同类环境问题。处置：改为在脚本里对每个窗口显式调 `Apply`（通道 D）。

## 17.6 完成判据

1. 每个新建窗口归类到 A/B/C/D 之一；纯窗口都有 `SourceInitialized` 的 `Apply` 调用点。
2. 切 `dark` 后用 17.3.7 读回代码（或 `titlebar.ps1`）验证所有可见窗口深浅标志 == 1；切回 `light` == 0。
3. 打开异主题 `ScopeWindow`：该窗深浅标志与主窗相反（`titlebar-live.ps1` 同款断言）。
4. `UI4Theme.Apply("highcontrast")` 后标题栏为深色标志且文字可读（底色黑/字白来自 35/36，Win11 上截图辅助目检）。
5. 豁免窗口 `Enabled="False"` 后深浅标志回 0、哨兵写入无异常。
6. 验证跑完 `samples/StartUI4Demo/bin/Debug/net48/demo-errors.log` 不存在。
