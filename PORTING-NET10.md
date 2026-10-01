# StartUI4.WPF — .NET 10 (LTS) 迁移说明

基线仓库：`E:\Qoder灵感项目\StartUI4.WPF_net48\`（net48 / C# 7.3，git HEAD `af30948`，**本轮零改动**，作对照产物与差异表基准）。
本目录 = `E:\Qoder灵感项目\StartUI4.WPF_net10\`，由基线的 229 个跟踪文件**逐字节复制**（`git ls-files` + `tar`，SHA-1 集合与基线完全一致）后 retarget 而来。
上游原始 `net6.0-windows7.0` 源码仍在 `upstream/` 供对照；`PORTING.md` 记录的是 net6→net48 的**降级**，本文档就是它大部分条目的反向勾除。

---

## 1. 为什么是 .NET 10，以及它的代价

| 事实 | 出处 |
|---|---|
| .NET 10 于 **2025-11** 发布，**LTS** | Microsoft Learn「What's new in WPF for .NET 10」 |
| 桌面运行时 = `Microsoft.WindowsDesktop.App`（WPF + WinForms + System.Drawing.Common + Win32 相关） | 「Install .NET on Windows」 |
| **不支持 Windows 7 / 8.1**；客户端只有 Windows 11（23H2+）与 Windows 10（1607/1809/21H2 的 **LTSC / 企业版**） | 同上，支持矩阵 |
| 最后支持 Win7/8.1 的是 .NET 6，已于 **2024-11-12** 终止支持 | 同上 |
| WPF 在 10 里的变化：Fluent 样式补齐（TextBox/RichTextBox/DatePicker/Label/GroupBox/GridView/GridSplitter/Hyperlink 等）、**剪贴板与 WinForms 统一为一套 API 并 obsolete 掉依赖 BinaryFormatter 的方法**、`MessageBox` 增补按钮与结果、`Grid` 支持 `RowDefinitions/ColumnDefinitions` 简写、字体/动态资源/XAML 解析性能优化、清掉 net48 遗留的 CAS/XBAP 代码 | 同上 |

> **底线变化必须显式承认**：net48 版 README 写的「支持 Windows 7 / 8.1 / 10 / 11」在 net10 版**不再成立**，本目录 README 已就地改成 Win10（LTSC/企业版）+ Win11。

## 2. 三个方案的对比与选择

| | **A′ 单目标 net10 + 只删死依赖（本目录采用）** | B 净室现代化 | C 多目标 `net48;net10.0-windows` |
|---|---|---|---|
| csproj | TFM→`net10.0-windows`，`LangVersion=latest`（**不回写新语法**），`Nullable/ImplicitUsings` 保持 disable，`GeneratePackageOnBuild=false` | 同 A′，外加 `Nullable enable` 或 18 处可空注解回灌 | 双 TFM + `#if NET48` 隔离差异点 + 两套 obj/bin |
| 源码改动 | 库内 **1 处**（`UI4Menu` 的 catch）+ 删 2 个死文件 | 20+ 文件（反向恢复 `Math.Clamp`/`is not`/`using var`/target-typed new 等） | A′ 量 + 条件编译位点 |
| 行为平价 | 最好：可用 `git diff --name-only` 断言「只动了白名单文件」 | 变量最多，回归 FAIL 难归因 | 平价最好 |
| OS 底线 | Win10+ | Win10+ | 保住 Win7/8.1 |
| 回归成本 | 单矩阵 | 单矩阵 + 色值需重测 | **矩阵 ×2** |
| 工作量 | 小 | 中 | 大 |

选 **A′** 的理由：组件库是别的应用在用的**基础层**，这一轮的目标是「跑到 .NET 10 上并且行为和原来一致」，不是顺手现代化。改动面越小，回归里出现的差异就越能归因给运行时本身。C 的兼容性收益用户当前不需要（已确认接受 Win10+）；B 引入的语法/可空性变量会污染归因。
**后续增量留口**：A′ 没引入任何新语法，将来真要保 Win7/8.1，只需在 csproj 加 `net48;` 并把本文档 §5 的几处做成条件编译，不必重做源码。

## 3. 迁移前的实测（结论直接决定了改动清单）

| 检查 | 方法 | 结果 | 结论 |
|---|---|---|---|
| SDK / 运行时 / 引用包 | `dotnet --list-sdks`、`--list-runtimes`、列 `C:\Program Files\dotnet\packs` | SDK `10.0.401`；`Microsoft.WindowsDesktop.App 10.0.12`；引用包 `...Ref/10.0.12/ref/net10.0` | 本机离线即可构建与运行 |
| 桌面引用包内容 | 列引用目录 | 含 `System.Drawing.Common.dll`、`Microsoft.Win32.SystemEvents.dll`、`UIAutomationClient/Types.dll`；基础包另有 `Microsoft.Win32.Registry.dll` | 托盘用的 `System.Drawing.Icon`、主题跟随用的 `Registry`/`SystemEvents` **都不必再加 NuGet** |
| `DependencyPropertyDescriptor` | 二进制里搜符号 | 在 net10 `WindowsBase.dll` 中存在；再用探针真实回调 2 次 | Demo 三处 `AddValueChanged` 不用改写 |
| `System.Data.SQLite 2.0.3` 的 netstandard 资产 | 列包目录 + nuspec | 只有 `lib/net471\|netstandard2.0\|netstandard2.1`，**nuspec 不声明依赖、包内无 `runtimes/`**；本机唯一的原生 `SQLite.Interop.dll` 来自 `stub...netframework/1.0.118/build/net20~net45` | 现代 .NET 下缺原生载体；而它唯一使用者 `UI4DataGrid` 是 `internal` 死代码（`PORTING.md:65`，且 README:1616 曾实测「把 dll 删掉各分页照常」）→ **删依赖 + 删死代码**，不换 Microsoft.Data.Sqlite |
| `AvalonEdit 6.3.1.120` | 列 `lib` | 含 `net6.0-windows7.0`、`net8.0-windows7.0` 资产 | net10.0-windows 直接可用，零改动 |
| nuget.org 可达 | `curl` | HTTP 302 / 0.37s | 万一需要补包不会卡住 |

### 探针实验（`_probe`，跑完已删除，数值留档）

| 编号 | 断言 | 结果 |
|---|---|---|
| P00/P0W | 探针自身跑在 .NET 10、WPF 窗口拿到 HWND | `Environment.Version=10.0.12`、HWND 有效 |
| P01/P02/P03/P03b | `SystemIcons.Application`、`Icon.FromHandle`、`new Icon(真实 ICO 流)`、`new Icon(文件路径)` | 全部可用 |
| P04 | `Marshal.SizeOf(NOTIFYICONDATAW)` | **976**；同一份结构在 net48（`_probe48`，x64）也算 **976** → 两框架 cbSize 一致 |
| P05 | `Shell_NotifyIcon(NIM_ADD)` 真实注册 | **True**（随后 NIM_DELETE） |
| P06/P07/P08 | 复刻 `UI4Menu.cs:276-280` 的 `ParserContext` + `XamlReader.Parse`（空前缀 / `assembly=` 短名 / `x:Type` / `TemplateBinding`），解析出的 Style 再 `ApplyTemplate()` | 全部可用；同程序集短名解析自定义类型也可用 |
| P09 | `DependencyPropertyDescriptor.FromProperty(...).AddValueChanged` 回调次数 | 2/2 |
| P10/P11 | 读 `AppsUseLightTheme`；在一次性键上写→读→删 | 值可读；往返成功；`HKCU\Software\StartUI4` **未被创建** |
| P12 | `SystemEvents.UserPreferenceChanged` 订阅/退订 | 可用（事件本身要人工改系统主题才发得出，见 §8 人工清单） |
| P13/P14/P15 | WPF 框架剪贴板往返；`IsClipboardFormatAvailable`；`GlobalAlloc/GlobalLock/SetClipboardData/GetClipboardData` 全链路 | 可用（首轮曾报 2 个 FAIL：P04 是我按「Win32 应为 952」的**预期写错**，P03 是我**自造的最小 ICO 字节不合法**；改用真实图标往返后即 PASS——都是探针自己的问题，不是平台差异） |

## 4. 改动清单（相对基线，`git diff --name-only` 可核）

工程文件（3 个）：

1. `src/StartUI4Controls/StartUI4Controls.csproj`
   - `net48` → `net10.0-windows`；`LangVersion 7.3` → `latest`；`Nullable/ImplicitUsings` 仍 disable
   - 删 `<Reference Include="System.Drawing" />`（WindowsDesktop 框架引用已提供）
   - 删 `PackageReference System.Data.SQLite 2.0.3`
   - `GeneratePackageOnBuild` → **false**（改显式 `dotnet pack`；true 时 `dotnet build` 会走 pack 目标，许可证文件解析失败会连带 NU5019 把构建拖红）
   - 版本 1.0.20 → **2.0.0**，Description 改 .NET 10
2. `samples/StartUI4Demo/StartUI4Demo.csproj`：TFM + `LangVersion latest`，`app.manifest` 保留（.NET 的 WPF 仍按清单取 DPI 级别，PerMonitorV2 照旧生效）
3. `tools/clipboard-lock-check/clipboard-lock-check.csproj`：TFM + `LangVersion latest`；工程从 `常见问题清单/…-tools/` 移到正式的 `tools/`（原来那条 `../../src/...` 相对引用在新位置才真正成立）；`StartUI4Controls.sln` 从 2 个项目扩到 4 个

库源码：

- **删除** `UI4DataGrid.cs`（internal，SQLite 唯一使用者）、`UI43DSphere.cs`（internal，Media3D，无引用）——两者在 `PORTING.md §3.3` 就已被记为「上游即 internal 且全仓无引用」的死代码
- `UI4Menu.cs`：`InitStyles()` 的 `catch { MessageBox.Show(...) }` 改为「落盘 `ui4menu-style-error.log` + 重新抛出」。原写法把「菜单整块没样式」这种致命状态压成一次点击，还会让自动化回归被模态框卡死
- `Internal/ScrollBarResources.cs`：两处提到 `UI4DataGrid` 的注释同步删除
- 源码到此为止：改动 **2 个** `.cs`（`UI4Menu.cs` + 上面这条注释），其余 **41 个** `.cs` 逐字节未动
  （`git diff --name-status f266aba -- src` 可复核，f266aba = 本目录那份基线副本的提交）

Demo：

- 标题/副标题/托盘 ToolTip 的 `.NET Framework 4.8 / C# 7.3` 字样改成 `.NET 10 (LTS)`
- 「库内部 Ctrl+C/X/V 接管」段落里删掉已不存在的 `UI4DataGrid 编辑单元`
- 按钮对比度文案改成本轮真实复现值（见 §5）

新增：`tools/Net10Regression/`（回归 harness，见 §7）。

## 5. 迁移过程中发现的差异（含被证伪的旧文档）

1. **对比度那组数不能全部复现**。Demo 第 0 页原文写「实测对比度：默认渐变启用 3.05、禁用 10.12、浅灰底 10.04」。进程内重测（离屏渲染 + `VisualTreeHelper` 取解析后的实际画刷）得到：
   - 禁用态 = **10.12**（底色 `#C8C8DC` = BorderNormal 令牌，前景 `#1E1E1E` = 正文色）✔ 与原文一致
   - 浅灰底启用 = **10.04**（`#C8C8D2` + `#1E1E1E`）✔ 与原文一致
   - 深渐变 `#0024FF→#B400FF` 启用 = **7.08**（白字）；库默认渐变 `#0078D4→#9333EA` 启用 = **5.76**（白字）✘ 都凑不出 3.05
   处理方式：**不猜**。把断言从「复述一个记忆中的比值」改成「按规则断言」——深底自动白字、浅底自动正文色、禁用整块换模板；比值只作为证据打印。文案里的 3.05 已换成上面三个可复现的数。
2. **`exe` 不再等于托管程序集**。net48 的 `StartUI4Demo.exe` 里能直接扫到 `.NETFramework,Version=v4.8`；net10 的 exe 是原生 apphost，托管主体在同名 `dll` 里，TFM 串是 `.NETCoreApp,Version=v10.0`（注意不是 `.NETCore,Version=`）。同时 `StartUI4Demo.exe.config` 消失，换成 `StartUI4Demo.runtimeconfig.json`——`PORTING.md §7.1` 那套「三层实证」的第三层要按新机制改写。
3. **控制台进程里的探针窗口**：`Application.Current` 默认 `ShutdownMode=OnLastWindowClose`，第一个探针窗口 `Close()` 后应用进入关闭流程，之后 `Show()` 出来的窗口**拿不到 HWND**，表现为 `UI4WindowTitleBar.Apply` 返回 false、`DwmGetWindowAttribute` 返回 0x80000000。改成 `OnExplicitShutdown` 后同一份代码 Apply=True、DWM 标志可读回（深色=1 / 豁免后=0）。这是**测试宿主**的坑，不是库的缺陷，但足以在回归里伪装成「net10 的 DWM 染色坏了」。
4. **`ComboBoxItem` 在折叠态不在 UIA 树里**：对未展开的下拉直接 `SelectionItemPattern.Select()` 会命中「选中框里那份文本」，`SelectedIndex` 不变——UI 上看着像成功了。回归必须「点开 → 在弹出层里点项」。
5. **UI4Pivot / UI4Tab / UI4NavigationView 的内容文本在控制视图里取不到**（net10 与 net48 对照见 §7 的差异表；若两边一致则属库既有特征，不是迁移引入）。
6. **`ThemeChanged` 是在 `DispatcherPriority.Input` 上派发的**（`UI4Theme.cs:481`），同步取值断言必须先跑一次小消息循环，否则读数恒为 0——这也是测试宿主的坑。
7. **Demo 底部状态栏原本被 `UI4CircleSlider` 的入场动画逐帧覆写**（net48/net10 同现）：`AddValueChanged` 每帧写一次状态栏，
   导致「推进进度条」「点按钮」「右键菜单」的反馈几乎看不见。已加「只在对应页在前台时才写」的闸门 + 行内 `CircleEcho`。
8. **Demo「启动态/禁用态」那组按钮原本没有 `Click` 处理器**，点了没有任何反馈，与本页「所有可点项都有回显」的约定矛盾（net48 版就有）。三个可用样本已补 `AnyButton_Click`。
9. **UIA（无障碍/自动化）在 net10 上退化，渲染不受影响**——本轮唯一的 net10-only 行为差异，已用同源码 A/B 判别：
   - `UI4Pivot` / `UI4Tab` / `UI4NavigationView` 的内容区文本不再进 UIA 树：net48 的原始视图里 6 处命中，net10 为 0；
   - 内容其实渲染了——I13~I16 按 Demo 的嵌套方式（Show 前选页 + ScrollViewer）在进程内翻视觉树，三个容器的选中内容都在；
   - `UI4ProgressBar` / `UI4CircleSlider` 等控件本体也不进 UIA 树，但这一条 **net48 同样读不到**，属两边共有的库缺口，不是迁移引入。
   详见 §7.3。库侧待办：给上述容器补 `AutomationPeer`（`GetChildrenCore` / `ContentElement`）。
   另外，中途曾据 `--probe-status` 得出「net10 的 `TextBlock` Name 不再刷新」，**该结论已被复验推翻**：
   真因是本节第 7 条的状态栏刷屏竞态，修掉后 net10 与 net48 都实时报最后一次点击的文本（记录见 §7.3 末）。

## 6. 与消费方的关系

工作区里的应用 `Prompt收藏夹（示例项目）\src\PromptFavorites\` 仍是 **net48**，且以 ProjectReference 指向它自己那份库源码副本（`组件库与组件使用示例demo\src\StartUI4Controls`）。本目录的 net10 库**不能直接给它用**；要让它跑在 net10 上，应用与它那份副本得同步升级（另起一轮）。`samples/Prompt收藏夹（示例项目）/`（含 `lib/StartUI4Controls.csproj`，net48）作为示例快照原样保留，本轮不迁移，也不进 sln。

## 7. 回归（harness + 结果）

见 `tools/Net10Regression/USAGE.md`。三层：系统级（运行真实性、注册表零污染、异常日志、剪贴板持锁矩阵）、13 页 UIA 逐页走查（含真实按键 Ctrl+C/X/V 与原生读回、模态对话框、托盘开关、下拉弹出层、DWM 标题栏回读）、进程内 STA 色值断言。

### 7.1 A/B 对照的做法

为了做到「唯一变量是运行时」，没有直接用 net48 仓库里那份旧产物，而是把**本目录当前源码原样**复制一份到 `_ab/`，
只把 TFM 改回 `net48` / `LangVersion=7.3` 再编一遍（`cp -r src samples → sed TFM → dotnet build`）。
同一套 harness 用例分别打两个产物，差异就能归因到运行时；顺带也证明了本目录的库源码在 C# 7.3 / net48 下依然编得过
（即「没有回写现代语法」这条承诺是可验证的）。

两点说明：`_ab/` 是一次性试验台，跑完即可删（重建三条命令见 §7.6 备注）；
它沿用了本目录 Demo 的文案，所以窗口标题会写「.NET 10 (LTS)」——这不影响结论，
真实框架以标题栏右侧的 `RuntimeText`（`FrameworkDescription`）为准，那边会显示 `.NET Framework 4.8.x`。

### 7.2 最终结果（2026-10-01 16:2x，同一台机器、同一份 Demo 源码，退出码 0）

| 层 | 结果 |
|---|---|
| 系统级 S01–S08 | **10 PASS / 0 FAIL**，含剪贴板持锁矩阵 **10/10**（接管后 UI 阻塞 11~30ms；作为对照的未接管框架通道在同一把锁下阻塞 1048ms） |
| 逐页 UIA（net10，13 页） | **145 PASS / 0 FAIL** |
| 逐页 UIA（同源码重编的 net48 A/B） | **145 PASS / 0 FAIL** |
| 进程内 STA（I01–I16） | **26 PASS / 0 FAIL** |
| 合计 | **326 PASS / 0 FAIL / 0 SKIP**，进程退出码 **0** |

两侧跑的是同一套用例、同一份源码，逐条同结果；而运行时确实不是同一个——日志里两次 `T0-runtime`
就是铁证：net10 侧 `实际运行时：.NET 10.0.12`，A/B net48 侧 `实际运行时： .NET Framework 4.8.9345.0`。

`results-final.md`（本目录 `tools/Net10Regression/`）是原始逐条输出，含每条用例的期望与实际值。

> 中途两轮也留在话里，免得读者以为一次就绿：第一轮逐页 117 PASS / 16 FAIL，第二轮 322 PASS / 2 FAIL。
> 失败项经 A/B 判别后全部是**取证通道用错**：模态框与下拉项要用 `EnumWindows` 找同进程的弹出窗口、
> `ComboBoxItem` 折叠态不在 UIA 树里、状态栏被 CircleSlider 动画逐帧覆写、探针宿主 `ShutdownMode` 让后续窗口拿不到 HWND。
> 修完这些取证方式才收敛到 0 FAIL。唯一**真实**的 net10 独有差异是 §7.3 那条 UIA 暴露回归，
> 它不表现为用例失败（用例已改成只断言 UIA 真能看到的东西），要用 `--dump=6` 两侧对比才看得见。

### 7.3 net10 相对 net48 的**唯一**行为差异：三个导航容器的内容不进 UIA

判别实验（`Net10Regression --dump=6`，同一份 harness 分别打两个产物）：

| | net48（同源码 A/B） | net10 |
|---|---|---|
| 页 6 原始视图节点数 | 128（有 Name 的 82） | 109 |
| 「首页内容 / 主页内容 / 代码页内容 / 属性页内容 / 设置页内容」 | **6 处命中** | **0 命中** |
| 内容是否真的渲染 | 是 | **是**（I13~I16 在进程内按 Demo 的嵌套方式翻视觉树，三个容器的选中内容都在） |

所以这是 **.NET 10 下的 UIA/无障碍暴露回归**，不是渲染回归：`UI4Pivot` / `UI4Tab` / `UI4NavigationView` 的内容区
不再交给 UIA 客户端，屏幕阅读器与 UIA 自动化因此读不到页面主体。库侧待办见 §9 第 0 条。

> 一条**已被自己复验推翻的中间结论**，留在这里当作方法说明：中途我曾据 `--probe-status` 判定
> 「net10 下 `TextBlock.Text` 改到第二次就不再推给 UIA（状态栏冻结在第 2 次点击的值）」。
> 那其实是 §5.7 的状态栏刷屏竞态——`UI4CircleSlider` 的入场动画每帧重写状态栏，UIA 事件队列被冲乱，
> 表现出来像「Name 不刷新」。修掉刷屏（§5.7 的闸门 + 行内 `CircleEcho`）之后，
> `--probe-status` 在 net10 与 net48 上都报出**最后一次**点击的文本，两侧一致。
> 教训：把「自动化读不到」直接归因到平台前，先排除被测程序自己的输出通道被高频覆写。

### 7.4 剪贴板持锁矩阵（`tools/clipboard-lock-check`，锁 1200ms / 接管阈值 100ms）

| 用例 | UI 阻塞 | 结论 |
|---|---|---|
| 基线 plain TextBox（框架 OLE 通道）Ctrl+X | **1048ms** | PASS（作为对照，证明锁真的被占住、框架通道真的被拖） |
| UI4TextBox Ctrl+X / C / V | 18 / 19 / 12 ms | PASS |
| UI4CodeEditor Ctrl+X / C / V | 28 / 11 / 13 ms | PASS |
| UI4PasswordBox 明文 Ctrl+X / C / V | 15 / 17 / 30 ms | PASS |

最终这轮 **10 PASS / 0 FAIL**，「基线 plain TextBox（框架 OLE 通道）」那条对照同样成立（1048ms 阻塞）。
中间两轮曾出现过 9/10：失败的都是这条对照，它的判据是「阻塞 >400ms」，
实测值随本机剪贴板监听器（PixPin）当时的忙闲浮动；原始数值留在 `tools/clipboard-lock-check/bin/.../results.txt`。

### 7.5 迁移过程中顺手修掉的真实缺陷（都在 Demo/工程层，不在库行为层）

1. **状态栏被刷屏**：`UI4CircleSlider` 载入后逐帧跑入场动画，`AddValueChanged` 每帧写一次底部状态栏，
   于是「推进 Bar1」「点按钮」「右键菜单动作」的反馈全被冲掉（net48 同样存在，只是它的 UIA 刷新正常所以以前没暴露）。
   修法：写状态栏前判断「选择器」页是否在前台，并在第 3 页加一个行内 `CircleEcho` 专门显示动画终值。
2. **「启动态/禁用态」那组按钮没有 Click 处理器**：点了完全没反馈，与本页「每个可点项都有回显」的约定不一致（net48 版就有）。
   修法：三个可用样本补 `Click="AnyButton_Click"`。
3. **对比度文案里有一个数复现不了**：原文写「默认渐变启用 3.05、禁用 10.12、浅灰底 10.04」。
   进程内实测（离屏渲染后取模板里的实际画刷）：禁用 **10.12** ✔、浅灰底 **10.04** ✔、
   显式深渐变 `#0024FF→#B400FF` **7.08**、库默认渐变 `#0078D4→#9333EA` **5.76**——3.05 无对应口径，
   文案已改为写可复现的四个数，断言也从「复述比值」改成「按规则断言」（深底白字 / 浅底正文色 / 禁用换模板）。
4. **harness 自己的两个假失败**（值得记下来，因为都会伪装成产品缺陷）：
   - 解析 `results.txt` 的辅助函数顺手做了 `Truncate`，把 `\r\n` 换成空格 → 统计 PASS 行数恒为 0，看起来像「矩阵没跑」；
   - 探针窗口在 `Application.Current` 默认 `ShutdownMode=OnLastWindowClose` 下，第一个窗口 Close 后进程进入关闭流程，
     之后 `Show()` 的窗口拿不到 HWND → `UI4WindowTitleBar.Apply` 返回 false、DWM 回读 0x80000000，
     看起来像「net10 的标题栏染色坏了」。改成 `OnExplicitShutdown` 后同一份代码 Apply=True、深色标志回读=1、豁免后=0。

### 7.6 复跑命令

```bash
dotnet build StartUI4Controls.sln
tools\Net10Regression\bin\Debug\net10.0-windows\Net10Regression.exe --all          # 全量
tools\...\Net10Regression.exe --tabs --tab=9        # 单页
tools\...\Net10Regression.exe --inproc              # 进程内色值/对比度/通道
tools\...\Net10Regression.exe --dump=6              # 打印某页 UIA 控制视图 + 原始视图
tools\...\Net10Regression.exe --probe-status        # §7.3 那个 A/B 判别实验
reg query "HKCU\Software\StartUI4"                  # 期望：找不到该键
```

重建 A/B 对照产物（`_ab/` 是一次性试验台，不入库）：

```bash
mkdir -p _ab/src _ab/samples
cp -r src/StartUI4Controls _ab/src/ && cp -r samples/StartUI4Demo _ab/samples/
rm -rf _ab/*/*/obj _ab/*/*/bin
sed -i 's|<TargetFramework>net10.0-windows</TargetFramework>|<TargetFramework>net48</TargetFramework>|; s|<LangVersion>latest</LangVersion>|<LangVersion>7.3</LangVersion>|' \
    _ab/src/StartUI4Controls/StartUI4Controls.csproj _ab/samples/StartUI4Demo/StartUI4Demo.csproj
(cd _ab/samples/StartUI4Demo && dotnet build)
```

## 8. 人工确认清单（脚本证明不了的，用手 + PixPin）

1. 系统「设置 → 个性化 → 颜色」深浅切换，Demo 的「全局跟随系统」页脚 `ResolvedMode` 是否实时变（`SystemEvents.UserPreferenceChanged` 只在真实系统事件下触发）。
2. 复制类操作切到记事本按 Ctrl+V 验证内容；剪贴板被占用时按复制按钮 UI 不卡。
3. 拖动窗口右边缘看 UI4GridView 列数与回显一起变。
4. 托盘图标的**右键菜单**真的弹出、双击有日志（脚本只能验到「开关拨动 + 状态栏文案」）。
5. Win11 上标题栏底色/文字色是否跟着主题（DWM 拒绝回读配色，只能看）。
6. 高对比度页可读性、`ScopeWindow` 与主窗异主题并存。

## 9. 遗留

0. **【本轮唯一 net10 独有问题，优先级最高】三个导航容器的内容不进 UIA**（判别见 §7.3）：
   `UI4Pivot` / `UI4Tab` / `UI4NavigationView` 的内容区文本在 net48 的 UIA 原始视图里 6 处命中，net10 为 0；
   渲染本身正常（I13~I16 已证）。影响面是屏幕阅读器与一切基于 UIA 的自动化。
   修法是库侧补自动化对等物：容器控件实现 `OnCreateAutomationPeer` + `GetChildrenCore`（把内容区的子元素交出去）。
   顺带一条两边共有的缺口：`UI4ProgressBar` / `UI4CircleSlider` 等控件本体不进 UIA，
   需要 `RangeValue` 类 peer 才能让辅助技术读到进度值。
   本轮按「迁移不改库的行为」原则没动，留作单独一轮。

- `UI4Theme` 的 `CurrentMode` 仍是只有 `StaticPropertyChanged` 的静态 CLR 属性，宿主 XAML 普通绑定不会刷新（Demo 已改为事件驱动页脚）；想让宿主可直接绑定，需要按 WPF 约定补一个同名静态 `CurrentModeChanged` 事件。
- `UI4TextBlock.GradientStart/GradientEnd` 死属性（`PORTING.md §3.2`）仍未处理。
- 文档里那 8 个 `.ps1` 验证脚本（`shot/interact/theme/scopewalk/p2verify/p3verify/titlebar*`）在基线仓库中**并不存在**，本目录用 harness 取代；README 引用处已加说明。
- README 其余既有失真项（PORTING.md 死链、`UI4ComboBox.InnerPadding` 默认值、`UI4MessageBox.Show` 缺 `owner` 参数等）另起一轮。
