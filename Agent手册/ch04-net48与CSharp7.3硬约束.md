# 第 04 章 net48 与 C# 7.3 硬约束

> **本章描述的是 net48 基线**（`E:\Qoder灵感项目\StartUI4.WPF_net48`）。本仓库已迁到 .NET 10，差异如下，写作时以本节为准：
> - 工程三开关变成 `net10.0-windows` / `LangVersion=latest` / `Nullable=disable` / `ImplicitUsings=disable`
>   ——**语言上限解除了，但源码没有回写现代语法**，因此本章的禁用清单（`record`、switch 表达式、`using var`、
>   target-typed `new()`、`is not`、引用类型 `?`）对「读代码」仍成立，对「能不能写」已不再成立。
> - net48 BCL 缺失项里，`Math.Clamp`、`Enum.GetValues<T>`、`string.Contains(char)`、`Dictionary.TryAdd`、`??=`、
>   `Random.Shared`、`Index/Range`、`Span`、`System.Text.Json`、`DateOnly/TimeOnly`、`EffectiveViewportChanged`
>   在 net10 **都已可用**；库里那 17 处 `Math.Max(v, Math.Min(...))` 因此是可选清理项，不是约束。
>   仍不存在的是 .NET Framework 专属物：`AppDomain` 证据/私有目录、`BinaryFormatter`、CAS/XBAP、`System.Drawing` 隐式引用。
> - `ConditionalWeakTable` 的三条限制（值须引用类型、无 `AddOrUpdate`、重复 `Add` 抛）在 net10 已放宽，
>   `AddOrUpdate`/`TryGetValue` 都在。
> - 产物与宿主机制：`exe` = 原生 apphost + 同名托管 `dll`；`.deps.json`/`.runtimeconfig.json` 出现，`exe.config` 消失；
>   `TargetFrameworkAttribute` 变成 `.NETCoreApp,Version=v10.0`。
> - 验收多了一条硬要求：跑 `tools\Net10Regression\Net10Regression.exe --all`（旧 `.ps1` 脚本已不存在）。
> 详见仓库根 `PORTING-NET10.md`。

> 本章解决：把「能跑在 net6 上的现代 C# 写法」变成「能在 net48 + C# 7.3 编译通过的写法」，并给出降级检查清单。
> 读者：AI Agent（要在本项目之外新建/续写 WPF 业务代码）
> 前置章节：第 02 章、第 03 章

## 4.1 目标与验收

1. 我能说出三个工程开关（`LangVersion=7.3` / `Nullable=disable` / `ImplicitUsings=disable`）各自的落地后果，并指出它们在两个 csproj 的具体行。
2. 我写的每一段 C# 都能在 C# 7.3 下编译：`dotnet build` 后**不出现** CS8xxx（新语法）与 CS0117/CS1061（BCL 缺失成员）。
3. 我能对 §4.3.2 对照表任一条目给出仓库内等价写法出处（文件:行），并指出 §4.3.7 里对应的片段方法名。
4. 我能用 §4.3.6 清单把一段 net6 代码逐条降下来，且降级后行为不变（特别是 `using` 生命周期与 `Math.Clamp` 的边界）。
5. 我知道 `ConditionalWeakTable` 在 net48 上的三条限制，并能写出「可变盒子」正确形态。

## 4.2 源码依据

- `src/StartUI4Controls/StartUI4Controls.csproj:6-8`（`LangVersion` / `Nullable` / `ImplicitUsings`）、`:32`（`<Reference Include="System.Drawing" />`）
- `samples/StartUI4Demo/StartUI4Demo.csproj:7-9`（同样三条，Demo 也锁 7.3）
- `src/StartUI4Controls/UI4WindowTitleBar.cs:2`（`using System.Runtime.CompilerServices;`）、`:54-61`（CWT + 可变盒子）、`:84`（`TryGetValue`）、`:109`（`GetValue(key, factory)`）
- `src/StartUI4Controls/UI4ThemePersistence.cs:70`（手写一行 JSON，不引序列化库）、`:48,72,85`（`catch { }` 静默降级）
- `src/StartUI4Controls/UI4Theme.cs:80-82`（`if (_current == null) _current = …`，即 `??=` 的 7.3 写法）、`:299`（`(UI4ThemeToken[])Enum.GetValues(typeof(...))`）、`:633`（索引器赋值代替 `Add`）
- `src/StartUI4Controls/UI4GridView.cs:244`（`Math.Max(1, (int)Math.Floor(...))`，钳位的 7.3 写法）
- `src/StartUI4Controls/UI4Menu.cs:286-288`（库内弹窗兜底，反面案例）
- `samples/StartUI4Demo/MainWindow.xaml.cs:14`（`private static readonly Random Rng = new Random();`，`Random.Shared` 的替代）
- `PORTING.md:13-27`（不兼容点矩阵 + 「扫描确认未使用」清单）、`:42-50`（移植时的 11 项语法降级处数）

## 4.3 正文

### 4.3.1 三个开关的落地后果

```xml
<PropertyGroup>
  <TargetFramework>net48</TargetFramework>
  <UseWPF>true</UseWPF>
  <LangVersion>7.3</LangVersion>          <!-- 编译器拒绝 C# 8+ 语法 -->
  <Nullable>disable</Nullable>            <!-- 引用类型上的 ? 报 CS8370 一类错 -->
  <ImplicitUsings>disable</ImplicitUsings><!-- 没有任何隐式 using，全部手写 -->
</PropertyGroup>
```

- `LangVersion=7.3`：语法上界 = VS2019 16.3 一代。`record`、`switch` 表达式、`using var`、目标类型 `new()`、可空注解、默认接口成员、`and/or/not` 模式一律报错。7.0–7.3 允许的都可用（`out var`、模式匹配、元组、局部函数、表达式体成员、`nameof`、字符串内插）。
- `Nullable=disable`：不要把 `string?` 这类**引用类型**可空注解写进注解位。**值类型的 `Nullable<T>`/`T?` 仍然正常**，库内大量在用：`UI4ColorPicker.ShowDialog(string title, Color? defaultColor, Window owner)`（`UI4ColorPicker.cs:782`）、`IThemePersistence.Load()` 返回 `UI4ThemeMode?`。
- `ImplicitUsings=disable`：每个文件自己写全 using（`UI4TextBlock.cs:1-10` 就是 10 行）。抄 net6 代码时**新类型的 using 必须补**——`PORTING.md:185` 记录的 CS0246（`DropShadowEffect` 缺 `using System.Windows.Media.Effects;`）即这条的真实事故。

### 4.3.2 禁用特性 → 等价写法对照表

| 不能写（C# 8+/net6 BCL） | 7.3 / net48 写法 | 仓库内可抄的出处 | §4.3.7 片段 |
|---|---|---|---|
| `using var fs = File.Open(...)` | `using (var fs = File.Open(...)) { ... }`，按原生命周期重排块 | `PORTING.md:49`（`UI4DataGrid.cs` 5 处即这样改） | `ReadFirstLine` |
| `if (x is not T y)` | `if (!(x is T y))` + 显式转型 | `PORTING.md:45`（6 处） | `Summarize` |
| `var list = new();` | `var list = new List<string>();` | `PORTING.md:46`（1 处） | `Put` |
| `Handle((_, _) => ...)` | `Handle((sender, e) => ...)` | `PORTING.md:47` | `BuildTickHandler` |
| `string? name`、`ScrollBar? bar` | 去掉 `?`，用 `null` 判断 | `PORTING.md:48`（18 处） | `SafeLength` |
| `Math.Clamp(v, lo, hi)` | `Math.Max(lo, Math.Min(hi, v))`（按 int/double 选字面量） | `UI4GridView.cs:244`；`PORTING.md:44`（17 处） | `ClampPercent` / `ClampUnit` |
| `array[1..^1]`、`Index`/`Range` | `Substring`/`Array.Copy`/显式循环 | `PORTING.md:24`（扫描确认未用） | `Head3` |
| `Span<T>`/`Memory<T>` | 数组 + 索引；引 `System.Memory` 包属新依赖，先问人 | 同上 | `Sum` |
| `System.Text.Json` | 手写单行 JSON 或 `DataContractJsonSerializer` | `UI4ThemePersistence.cs:70` | `SaveDay` |
| `Random.Shared` | 自建 `static readonly Random`（跨线程共享时加锁） | `MainWindow.xaml.cs:14` | `NextSample` |
| `dict.TryAdd(k, v)` | `dict[k] = v` 或 `if (!dict.ContainsKey(k)) dict.Add(k, v)` | `UI4Theme.cs:633` | `Put` |
| `x ??= new T()` | `if (x == null) x = new T();` | `UI4Theme.cs:80-82` | `Put` |
| `record` / 记录结构 | 普通类 + 显式构造函数 | `MainWindow.xaml.cs:304-318`（`CardItem`） | `OrderRow` |
| 默认接口成员 / 静态虚拟接口成员 | 抽象类或扩展方法 | 库内 `IThemePersistence`（仅 2 个纯抽象方法） | `NotifierBase` |
| `switch` 表达式、`and/or/not` 模式 | `switch` 语句、嵌套 `if` + `is` | `UI4Theme.cs:132-137`（`KeyForMode`） | `Summarize` |
| `Enum.GetValues<Probe>()` | `(T[])Enum.GetValues(typeof(T))` | `UI4Theme.cs:299` | `AllProbes` |
| `str.Contains('c')` | `str.IndexOf('c') >= 0` | `PORTING.md:25` | `HasDash` |
| `DateOnly` / `TimeOnly` | `DateTime` + 固定格式串 | 库内无日期持久化，按 `UI4ThemePersistence.cs` 的字符串风格自拼 | `DayKey` |
| `await foreach`（`IAsyncEnumerable`） | `foreach` + `await`，或先物化成 `List<T>` | 库内 `async` 见 `UI4NotifyIcon.cs` 周边调用 | `SumAsync` |

### 4.3.3 net48 与 net6+ 的 BCL / 平台差异

| 主题 | net48 现实 | 操作建议 |
|---|---|---|
| `HttpClient` | 类型可用，但需框架引用 `System.Net.Http`；**没有** `IHttpClientFactory`、`SocketsHttpHandler`、HTTP/2 默认栈 | 自建 `static readonly HttpClient`（进程级复用），超时用 `client.Timeout`；别照抄 net6 的工厂写法 |
| JSON | 无 `System.Text.Json` | 引第三方包要问人；本库对策是自己拼/自己解析（`UI4ThemePersistence.cs:70,75-87`） |
| 运行时信息 | `RuntimeInformation.FrameworkDescription` 自 **4.7.1** 起可用 | 第 03 章 §3.3.5 的窗口内自检即依赖它 |
| 部署产物 | **没有** `.deps.json` / `.runtimeconfig.json`；靠 Fusion/GAC + `app.config` | 别在发布包里找这两个文件 |
| 程序集元数据 | `TargetFrameworkAttribute` = `.NETFramework,Version=v4.8` | 反射读它做编译期实证（`PORTING.md:125-126`） |
| GDI+/图标 | `System.Drawing` 不是默认引用 | 库已显式引用（csproj`:32`）；宿主用 `UI4NotifyIcon.IconSource` 回退链时同样要自己加 `<Reference Include="System.Drawing" />` |
| WPF 新 API | `EffectiveViewportChanged`、`TextBox.SpellCheck` 等为 Core 3+ 才有 | 用 `LayoutUpdated`/`ScrollChanged` 等 net48 老 API 替代 |
| 线程/异步 | `async/await`、`Task` 完整可用；无 `IAsyncEnumerable`、无 `[SupportedOSPlatform]` | 别写 `await foreach`；平台判断走「先试、按返回码定论」（`UI4WindowTitleBar.cs:143-161`） |
| 高 DPI | 库不处理 DPI，声明在宿主的 `app.manifest` | 新应用必须自带 `PerMonitorV2` manifest（抄 `samples/StartUI4Demo/app.manifest`） |

### 4.3.4 `ConditionalWeakTable` 的三条约束与可变盒子

net48 的 CWT 与 net6 有三点不同，库内标题栏去重表就是按这三点写的（`UI4WindowTitleBar.cs:2`、`:54-61`、`:84`、`:109`）：
**① `TValue` 必须是引用类型**（不能 `ConditionalWeakTable<Window, int>`）⇒ 包一层可变盒子；**② 没有 `AddOrUpdate`** ⇒ 「有则改、无则建」只能 `GetValue(key, factory)` 拿盒子再改字段；**③ 同键重复 `Add` 抛 `ArgumentException`** ⇒ 只读探测用 `TryGetValue`。

```csharp
private sealed class PaintedVersion { public int Value; }   // 盒子：引用类型 + 可变字段

private static readonly ConditionalWeakTable<Window, PaintedVersion> _paintedVersions =
    new ConditionalWeakTable<Window, PaintedVersion>();

public static void Remember(Window window, int version)
{
    PaintedVersion painted;                                       // ③：探测用 TryGetValue，不要靠 Add 抛异常
    if (_paintedVersions.TryGetValue(window, out painted) && painted.Value == version) return;
    _paintedVersions.GetValue(window, k => new PaintedVersion()).Value = version;   // ①②
}
```

配套纪律：键必须是**长生命周期的引用对象**（这里是 `Window`）；别把「表项消失」当窗口关闭信号。库内另一形态是 `UI4Theme.cs:347-350` 的 `WeakReference<T>` 追踪项，两种都要认得。

### 4.3.5 异常处理惯例（照抄库内风格，别自创）

- **探测/IO/系统调用失败 = 静默降级 + 返回保守值**：`catch { }` 后 `return null/false`。出处：`UI4Theme.cs:225`（探测系统主题）、`UI4ThemePersistence.cs:48,72,85`、`UI4NotifyIcon.cs:463,473,493`（图标三种解析路径逐个失败再退）、`UI4ListView.cs:588-591`。
- **参数校验 = 立即抛**：`throw new ArgumentNullException(nameof(element))`（`UI4ThemeScope.cs:31,38`、`UI4Theme.cs:163`）。
- **接口里「不该被调用的方向」= `NotImplementedException`**：所有 `IValueConverter.ConvertBack`（如 `UI4TextBlock.cs:28-31`）。
- **不要在库/控件代码里弹窗**：`UI4Menu.cs:286-288` 的 `MessageBox.Show(ex.Message)` 是既有反面案例——打断 UIA 走查、把异常伪装成用户操作。宿主收口在 `App.xaml.cs` 的两级钩子 + 日志（第 08 章）。
- net48 的 `DispatcherUnhandledException` 里 `e.Handled = true` 可继续运行（`App.xaml.cs:26`），但 `AppDomain.CurrentDomain.UnhandledException` **不能**阻止进程终止（`:29-32` 只做记录）。

### 4.3.6 把 net6 示例降到 7.3 的检查清单（按序执行）

1. 顶部补齐 using：逐个确认代码里**每个新类型**的命名空间已 using（`System.Windows.Media.Effects`、`System.Runtime.CompilerServices`、`System.Linq`、`System.Collections.Generic` 最容易漏）。
2. 全量替换语法糖：`using var`→块、`is not`→`!(is)`、`new()`→显式类型、`(_, _)`→具名参数、删掉引用类型的 `?`。
3. 扫 BCL：`Math.Clamp`、`Enum.GetValues<T>()`、`string.Contains(char)`、`dict.TryAdd`、`??=`、`Random.Shared`、`Index/Range`、`Span`、`System.Text.Json`、`DateOnly/TimeOnly`、`IAsyncEnumerable`、`EffectiveViewportChanged`。
4. 检查集合与锁：net48 没有新的并发糖，`lock` 仍是最稳妥形态（锁对象 `UI4Theme.cs:360`，使用 `:365,382,403,443,461`）。
5. 编译一次并**只读 C# 错误**：`dotnet build`，按 §4.3.2 逐条修；忽略 MC3074 级联（第 03 章）。
6. 行为回归：`Math.Clamp`→`Max/Min` 要确认 `lo<=hi` 且原调用点类型（int/double）与字面量一致；`using` 改块要确认释放时机没被延后或提前。
7. 运行时冒烟：`--tab=N` + `demo-errors.log` 不存在。语法降级不该引入任何运行期异常。

### 4.3.7 §4.3.2 每条对照的最小可编译片段

整文件可直接投进 net48 + 7.3 业务工程，`dotnet build` 应当 0 错误、0 新增警告；把任一段改回现代写法都会立刻报错——这就是它的用法。

```csharp
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Fabrix.App
{
    // [record / 记录结构] → 普通 sealed 类 + 显式构造函数（库内样板：MainWindow.xaml.cs 的 CardItem）
    public sealed class OrderRow
    {
        public OrderRow(string id, int quantity) { Id = id; Quantity = quantity; }
        public string Id { get; private set; }
        public int Quantity { get; private set; }
    }

    public enum ProbeMode { Off, Light, Dark }

    // [默认接口成员 / 静态虚拟接口成员] → 抽象基类给 virtual 默认实现
    public abstract class NotifierBase
    {
        public virtual void Notify(string text) { Console.WriteLine(text); }
    }

    public static class Samples
    {
        private static readonly Random Rng = new Random();   // [Random.Shared] → 进程级 static readonly；跨线程共享自行加锁
        private static Dictionary<string, OrderRow> _rows;

        // [x ??= new T()] + [目标类型 new()] + [Dictionary.TryAdd]
        public static void Put(string id, int quantity)
        {
            if (_rows == null) _rows = new Dictionary<string, OrderRow>();
            OrderRow row = new OrderRow(id, quantity);
            if (_rows.ContainsKey(id)) _rows[id] = row; else _rows.Add(id, row);
        }

        // [is not T] → !(x is T) + 显式转型；[switch 表达式 / and-or-not 模式] → switch 语句
        public static string Summarize(object candidate)
        {
            if (!(candidate is OrderRow)) return "(不是订单)";
            OrderRow row = (OrderRow)candidate;
            switch (row.Quantity)
            {
                case 0: return row.Id + " 空单";
                default: return row.Id + " x" + row.Quantity;
            }
        }

        // [可空引用注解 string? value] → 去掉 ?，显式判 null；[值类型 T?] → net48 + 7.3 合法，别一起删
        public static int SafeLength(string value) { return value == null ? 0 : value.Length; }
        public static ProbeMode? Probe() { return null; }

        // [lambda 丢弃参数 (_, _) =>] → 具名参数
        public static EventHandler BuildTickHandler() { return (sender, e) => { } }

        // [Math.Clamp] → Math.Max(lo, Math.Min(hi, v))；确认 lo<=hi，int/double 字面量与形参同型
        public static int ClampPercent(int v) { return Math.Max(0, Math.Min(100, v)); }
        public static double ClampUnit(double v) { return Math.Max(0d, Math.Min(1d, v)); }

        // [using var x = ...] → using (var x = ...) { }，块边界即原释放时机
        public static string ReadFirstLine(string path)
        {
            using (var reader = new StreamReader(path, Encoding.UTF8)) { return reader.ReadLine(); }
        }

        // [^ 与 ..（Index/Range）] → Substring；[Span<T>/Memory<T>] → net48 无，用数组 + 下标
        public static string Head3(string s) { return (s == null || s.Length <= 3) ? s : s.Substring(0, 3) + "..."; }
        public static int Sum(int[] values, int from, int count)
        {
            int total = 0;
            for (int i = from; i < from + count; i++) total += values[i];
            return total;
        }

        // [string.Contains(char)（Core 2.1+ 重载）] → IndexOf；[Enum.GetValues<T>()] → typeof 形态
        public static bool HasDash(string s) { return s.IndexOf('-') >= 0; }
        public static ProbeMode[] AllProbes() { return (ProbeMode[])Enum.GetValues(typeof(ProbeMode)); }
        public static int NextSample() { return Rng.Next(1, 100); }

        // [DateOnly/TimeOnly] → DateTime + 固定格式串；[System.Text.Json] → 手写一行 JSON（UI4ThemePersistence.cs:70）
        public static string DayKey(DateTime day) { return day.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture); }
        public static void SaveDay(string path, DateTime day) { File.WriteAllText(path, "{\"day\":\"" + DayKey(day) + "\"}"); }

        // [async]：Task/await 完整可用；没有 await foreach（IAsyncEnumerable 不是 net48 BCL）
        public static async Task<int> SumAsync(IEnumerable<int> items)
        {
            int total = 0;
            foreach (int i in items) { total += i; await Task.Yield(); }
            return total;
        }
    }
}
```

写作纪律：**新建业务工程时把 `LangVersion` 也钉在 `7.3`**（见第 06 章 §6.3.4 的 csproj 模板）。不钉的话 IDE 与 `dotnet build` 会默认放行 C# 12+ 语法，代码在你的机器上能编、在客户的 4.8 工具链上不能编；钉死之后所有不可用写法在**编写当场**报错，而不是留到交付。

## 4.4 推荐做法（Do / Don't）

- Do：以两个 csproj 的三个开关为不可协商前提；Don't：为写爽而加 `<LangVersion>latest`（库与 Demo 都锁 7.3，宿主跟着锁）。
- Do：把 §4.3.2 的「仓库内出处」直接抄进新代码；Don't：自创等价写法（同一件事库里有既定形态，混用会让评审误判）。
- Do：值类型可空照用 `Color?`/`UI4ThemeMode?`；Don't：把这套理解为「可以写 `string?`」。
- Do：CWT 值一律走盒子 + `GetValue(factory)`；Don't：`Add` 后再 `TryGetValue` 兜异常，或假设存在 `AddOrUpdate`。
- Do：探测失败即缓存结论并降级；Don't：读 `Environment.OSVersion` 判平台（无 manifest 的进程会虚报 6.3）。
- Do：宿主异常收口在 `App`；Don't：在控件/转换器里 `MessageBox.Show`。

## 4.5 坑与排错

| 症状 | 根因 | 处置与判据 |
|---|---|---|
| CS8370/CS8400 系列「feature not available in C# 7.3」 | 抄了 net6 代码的可空注解或新语法 | 按 §4.3.6 第 2 条逐条降级；判据：错误文本里的特性名能对上 §4.3.2 行 |
| CS1061「`ConditionalWeakTable<,>` 未包含 `AddOrUpdate`」/CS0452 | 把 net6 的 CWT 用法抄了进来 | 用 §4.3.4 的盒子写法；`TValue` 是值类型时编译期即报 CS0452 |
| CS0246 找不到类型（构建数分钟后才暴露） | `ImplicitUsings=disable` 下没补 using | `PORTING.md:193-195`：「源码已带全部 using」只对移植时点成立，任何新增引用都要重查 |
| 运行期 `FileNotFoundException: System.Net.Http` | net48 里 `System.Net.Http` 需显式框架引用 | 宿主 csproj 加 `<Reference Include="System.Net.Http" />` |
| `Math.Max(lo, Math.Min(hi, v))` 结果与预期反了 | lo/hi 传反，或 int/double 字面量混用触发重载歧义 | 明确写 `0d`/`100d` 或强转；判据：边界值单测（0、lo、hi、hi+1） |
| 切语言后已弹出的菜单/对话框仍是旧语言 | 本地化是**构造期拉取**，且 `UI4MultiLanguage` 无 `SetLanguage` | 改 `CultureInfo.CurrentUICulture` → `UI4MultiLanguage.Refresh()` → **重建**控件（第 23 章） |
| `Nullable` 注解删不干净，IDE 仍提示 | 只改了 csproj 没清声明处的 `T?` | 在新增文件的声明处 grep `\w+\?\s+\w+`；保留值类型形态 |

## 4.6 完成判据

1. §4.3.2 表的每一行我都能报出「片段方法名 + 库内出处」，无需翻文件。
2. 我的新工程 csproj 里 `<LangVersion>7.3</LangVersion>` 与 `<Nullable>disable</Nullable>` 都在位，且 `dotnet build` 无 CS8xxx。
3. 我把一段 net6 示例按 §4.3.6 降完，编译 0 错误、边界行为与 `Math.Clamp`/`using` 原语义一致。
4. 我写的 CWT 用法通过了 §4.3.4 三条约束自查（引用类型值、`GetValue` 工厂、`TryGetValue` 探测）。
