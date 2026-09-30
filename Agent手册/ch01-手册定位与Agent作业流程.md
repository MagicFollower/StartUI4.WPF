# 第 01 章 手册定位与 Agent 作业流程

> 本章解决：让 Agent 明确「本手册给谁读、按什么顺序读、动手前读哪三处真相源、改完跑哪些验证、什么情况下必须停下来问人」。
> 读者：AI Agent（要在本项目之外新建/续写 WPF 业务代码）
> 前置章节：无

## 1.1 目标与验收

读完本章后，Agent 应当能独立完成下面四件事，且每件都有可自检的判据：

1. 依据用户的任务类型，从 §1.3.3 的八条路线中选出一条，并列出该路线要读的章号序列（不多不少）。
2. 说出本仓库的三处真相源路径与 §1.3.4 的四级约束优先级，并解释为什么 README 与 `.qoder/specs` 不在真相源里。
3. 给出「一次改动完成后必须跑的最小验证序列」，并说明每条命令的通过判据。
4. 用 §1.3.4-D 的模板写出本次作业的可执行自查清单（每条含判据，不接受「确认正常」这类不可执行表述）。
5. 背出 §1.3.6 的「文档与实现不一致」清单，写任何属性名前先对一遍。

硬性验收：能默写出术语对照表中的 6 组词（客户区/非客户区、令牌、资源桥、作用域、命令式/引用式、DWM），
并说清「为什么主题色必须用 `DynamicResource`」——答「因为资源桥的键是原地覆盖的」即正确。

## 1.2 源码依据（必读文件清单）

本章不引入新 API，只引用仓库既有事实。以下文件用于核对「三处真相源」与验证口径：

- `src/StartUI4Controls/UI4Theme.cs:283`（`ApplyToApplication()`，资源桥播种入口）、`:297-308`（`WriteTokens`，键集合唯一来源）
- `src/StartUI4Controls/UI4ThemeScope.cs:13-19`（两条生效通道的类注释，命令式/引用式的权威定义）
- `src/StartUI4Controls/UI4WindowTitleBar.cs:17-23`（四条通路 + 「能力探测而非版本号判断」的注释）
- `samples/StartUI4Demo/App.xaml.cs:16-21`（`ApplyToApplication()` 调用点 + 两个异常钩子）、`:35-51`（`demo-errors.log` 写入）
- `samples/StartUI4Demo/MainWindow.xaml.cs:49-62`（`--tab=N` 参数）、`:98-103`（XAML 解析期事件空判注释）
- `titlebar.ps1:9-20`（库内 `DwmSetWindowAttribute` 是 private，验证器自带回读 import）、`:25-34`（泵序 `ContextIdle`→`Render`）
- `theme.ps1:8-10`（`demo-errors.log` 先删后测的约定）、`:36-44`（DWM 深/浅标志回读）
- `PORTING.md` 第 5、7、8 节（构建口径、sln 与 4.8 的关系、MC3074 级联的真实案例）
- §1.3.6 清单的取证位置：`UI4MultiLanguage.cs:37-57`（公开面只有 `Current`/`Get`/`Refresh`/`GetStrings`）、
  `UI4ComboBox.cs:55-64`（`FocusGradientStart/End`）与 `UI4TextBox.cs:49-64`（`HoverBorderColor`/`FocusBorderColor` 只在 TextBox 上）、
  `UI4Panel.cs:21`（注释 `cref="Title"`）、`README.md:1317`（`UI4ContextMenuLanguage`，源码全仓库无此类型）

## 1.3 正文

### 1.3.1 手册面向的对象与前置假设

本手册的读者是**执行型 Agent**，不是人类初学者。写作与执行都遵循三条约定：

- 手册只回答「怎么做、按什么顺序、怎么验证」；「是什么、为什么这样设计」由 `README.md`、`PORTING.md`、
  `主题方案分析与改进.md` 承担。同一事实不在两处重复描述，只在此处给指针。
- 假设读者可以读写文件、执行 shell、逐条比对构建输出。**不假设**读者记得任何 API 名——所有 API 名都要现查现用。
- 前置环境假设（不满足时先执行第 3 章）：Windows 10 1903+ 或 Windows 11（标题栏染色需 Win11 才有三色自定义，
  Win10 早期仅有深/浅标志）；已装 VS2022 或 .NET SDK + MSBuild；已装 .NET Framework 4.8 Developer Pack。

产品假设：目标是**新写一个业务 WPF 应用并引用本库**，而不是改造本库。凡是需要改 `src/StartUI4Controls/` 的想法，
都按 §1.3.4 的规则停下来问人。

### 1.3.2 24 章分区导览

全书 24 章，按任务阶段分五个区。分区与各章标题均已固定，与合并产物 `AI-Agent-WPF-Handbook.md` 的目录逐条对应。

| 区 | 章 | 章节标题 | 该区的产出 |
|---|---|---|---|
| 一 认知与准备 | 01–05 | 手册定位与 Agent 作业流程 / 仓库全景与真相源地图 / 开发环境与首次构建 / net48 与 C# 7.3 硬约束 / 组件库能力总览与选型决策 | 能构建、能跑起 Demo、能选对控件 |
| 二 应用骨架 | 06–11 | 新建应用的接入路径与最小可运行工程 / 工程配置与依赖治理 / App 启动序列与生命周期收尾 / 主窗口骨架与页组织 / XAML 编写规范与命名约定 / 导航容器选型与落地 | 一个可运行、可切主题、可排错的空壳应用 |
| 三 主题与品牌 | 12–17 | 主题引擎的工作方式与生效时序 / 设计令牌与资源桥 / 局部作用域主题 / 自定义主题、强调色与品牌化 / 系统跟随、持久化与高对比度 / 窗口标题栏与 DWM 染色 | 客户的品牌配色落到全应用 |
| 四 控件实操 | 18–22 | 基础交互控件实操 / 文本与输入控件实操 / 数据展示：列表、卡片视图与下拉 / 容器、滚动与「主题盲区」清单 / 菜单、对话框与系统托盘 | 每个业务页面都能用库内控件搭出来 |
| 五 业务与交付 | 23–24 | 本地化、图标资源与业务数据接入 / 自动化验证、故障定位与交付 | 可交付的 exe + dll 包 |

### 1.3.3 按任务类型的阅读路线

先判类型，再按序列读；序列外的章节只作查表用。章号写作两位（`01`…`24`）。

| 任务类型 | 必读序列（按序） | 可略 | 起手第一条动作 |
|---|---|---|---|
| **新建工程**（从零起一个业务应用） | 01 → 02 → 03 → 04 → 05 → 06 → 07 → 08 → 09 | 10–22 到具体页面时再查 | 第 06 章路径 A，把库工程加进你的解决方案 |
| **加一页界面**（应用骨架已存在） | 02 → 05 → 09 → 11 → 该页控件所在章（18–22）→ 23 | 03、06–08 | 在 `MainWindow.xaml` grep `TabItem Header` 找最接近的样板页 |
| **换肤 / 品牌化**（只改颜色不改布局） | 03 → 05 → 13 → 14 → 15 → 17 → 24 | 06–11、18–22 | `UI4ThemeDefinition.Clone()` + `With()` + `UI4Theme.Register/Apply`（第 15 章），不要逐个控件赋色 |
| **加一个列表页** | 05 → 20（列表/卡片/下拉）→ 21（容器与主题盲区）→ 23 | 06–11 | 先按第 05 章判据选 `UI4ListBox` / `UI4ListView` / `UI4GridView` / 原生 `DataGrid`，判据是「是否要求跟随主题」 |
| **加对话框 / 托盘** | 05 → 22（对话框/菜单/托盘）→ 08（生命周期收尾）→ 24 | 09–17 | 对话框务必传 `owner`；托盘先确认「必须在视觉树里 + 退出 `Collapsed`→`Dispose()`」两条硬规矩 |
| **修主题相关缺陷** | 02 → 03 → 05（通道矩阵）→ 12 → 13 → 14 → 17，逐章读 `NN.5 坑与排错` | 06–11、18–22 | 先判定该控件的主题通道，通道决定修法；再跑 `p3verify.ps1` 看是否已有断言 |
| **交付发布** | 03 → 06（依赖义务）→ 07 → 24 | 08–22 | 按第 24 章 §24.9 产物清单核一遍最小包，别把 `System.Data.SQLite.dll` 当必需 |
| **做自动化验证 / 接 CI** | 01 → 02 → 03 → 06 → 24，另读 `p2verify.ps1`、`p3verify.ps1`、`titlebar.ps1` 脚本本体 | 18–22 | 先跑 `theme.ps1` 确认基线绿，再写自己的断言 |

### 1.3.4 Agent 作业协议

**A. 动手前必读的三处真相源**（优先级从高到低，冲突时以序号小者为准）：

1. `src/StartUI4Controls/*.cs` —— 任何 API 名（类型、DP、事件、枚举值）必须在此 grep 到才允许写进代码或文档。
2. `samples/StartUI4Demo/MainWindow.xaml` 与 `MainWindow.xaml.cs` —— 唯一可编译运行的用法样板；
   写 XAML 前先在这里找同名控件的现成写法照抄。
3. 仓库根的 8 个 `*.ps1` 验证脚本 —— 它们定义了「成功」的口径（读哪个 DWM 属性、断言哪条令牌、泵几次 Dispatcher）。

次级资料只作背景：`README.md`（是什么）、`PORTING.md`（移植结论与修复记录）、`主题方案分析与改进.md`（方案与选型）、
`常见问题清单/`（两个 ComboBox 尺寸专题）、`src/StartUI4Controls/README.md`（架构审计，行号已漂移）。
`src/StartUI4Controls/.qoder/specs/*.md` 是历史任务规格，**不得作为 API 依据**（其中引用过绝对路径 `d:/TTTTT/StartUI4.WPF_net48/...`，
与当前仓库目录名不同，且描述的是当时的代码形状）。

**A′. 约束优先级（冲突时自上而下裁决）**：

```text
1. 源码本身（src/StartUI4Controls/*.cs、samples/StartUI4Demo/*）   ← 唯一裁决者
2. Agent手册/事实速查.md                                          ← 四路撰写者共用的事实基线
3. Agent手册/chNN-*.md（本手册）                                   ← 操作步骤与判据
4. README.md / PORTING.md / 主题方案分析与改进.md 的说明性文字       ← 只解释「为什么」，其属性表与行号会过期
```

执行规则：第 4 级与本手册冲突时按本手册做，并把冲突写进作业记录；本手册与 `事实速查.md` 冲突时按 `事实速查.md` 做并上报；
任何一级与源码冲突时一律按源码做。**Agent 不得为了让文档自洽而修改源码。**

**B. 改动后必跑的验证**（顺序即依赖，前者不过不要跑后者）：

```powershell
taskkill //IM StartUI4Demo.exe //F          # 先释放文件锁，否则 MSB3027
dotnet build StartUI4Controls.sln           # 判据：0 错误；警告数与既有基线一致（第 3 章）
powershell -STA -ExecutionPolicy Bypass -File theme.ps1              # UIA 走查 11 页 + 深浅往返
powershell -STA -ExecutionPolicy Bypass -File p3verify.ps1         # 进程内令牌/作用域/持久化断言
powershell -STA -ExecutionPolicy Bypass -File titlebar.ps1          # 标题栏染色回读断言
```

改完还要检查一个文件的存在性：`samples/StartUI4Demo/bin/Debug/net48/demo-errors.log`。**存在即有运行期异常**，
即使构建 0 错误也不算通过（`App.xaml.cs:35-51` 是它的唯一写入点）。

**C. 必须停下来问人的六种情况**：

1. 需要修改 `src/StartUI4Controls/` 里的库源码（含为了让自己方便而给 internal 成员开洞）。
2. 需要新增 NuGet 依赖、访问在线 feed，或本机缺 .NET Framework 4.8 Developer Pack 需要装东西。
3. 需要 `WindowChrome` / `WindowStyle=None` 自绘标题栏（该路线已被明确否掉，见 `主题方案分析与改进.md` 第十一节）。
4. 需要引入 MVVM/DI/导航框架（本仓库的真实模式是 code-behind + `DataContext = this`）。
5. 用户要求支持 .NET 6/8 或跨平台，或要求把 `UI4DataGrid` 之类的 internal 能力开放出来。
6. 目标机器可能是 Windows 7/8.1（标题栏三色与部分 DWM 行为不可用，观感需客户确认）。

**D. 自查清单怎么写**：一条一行，格式为「动作 → 判据 → 证据位置」。反例：「确认主题正常」。正例：

```text
[x] 切 Dark 后主窗标题栏变深 → DwmGetWindowAttribute(20) 回读为 1 → titlebar.ps1 输出 PASS
[x] 全量构建 0 错误 → dotnet build 末行 "0 Error(s)" → 构建日志尾部
[ ] 新增页面无运行期异常 → demo-errors.log 不存在 → 该路径 ls 结果为空
```

### 1.3.5 标准作业流程（六步，按序执行，不得跳过第 1 步与第 6 步）

| 步 | 动作 | 完成判据（可验证） |
|---|---|---|
| 1 读事实源 | 按 §1.3.3 选定路线并读完；把本次要用到的每个 API 名在 `src/StartUI4Controls/` grep 一遍 | 每个名字都有「文件:行」命中记录；§1.3.6 清单逐条比对无命中 |
| 2 建骨架 | 新建/修改工程与窗口外壳（csproj、`App.xaml`、三段式主窗）；先让空壳跑起来再加内容 | `dotnet build` 0 错误；窗口出现；`demo-errors.log` 式异常日志不存在 |
| 3 接主题 | `UI4Theme.ApplyToApplication()` 落进启动路径；宿主画刷/颜色一律 `DynamicResource` | 切 `Light`/`Dark` 后窗口背景与文字色变；标题栏深浅标志随主题变 |
| 4 填控件 | 按第 05 章选型表放控件，抄 Demo 同名用法；不跟随类控件按 §5.3.4 接线 | 每个新控件的主题通道已标注；深色页无「白纸块」；事件处理器已做 XAML 解析期空判 |
| 5 验证 | 跑 §1.3.4-B 序列 + 本次改动专属断言；泵序遵守 `ContextIdle`→`Render` | 各脚本全绿；异常日志不存在；构建警告不新增 |
| 6 归档 | 写作业记录：改了哪些文件、用了哪些 API（带行号）、验证输出摘要、遗留风险 | 记录可让另一次作业在只读它 + 本手册的前提下复现本次结果 |

第 2 步与第 4 步之间不要合并提交：骨架跑不通时，控件层的问题会被启动期异常掩盖。

### 1.3.6 已知「文档/注释与实现不一致」清单（写名字前对一遍）

| # | 文档或注释怎么说 | 源码实际 | 后果与正确写法 |
|---|---|---|---|
| 1 | `UI4MultiLanguage` 注释提供 `SetLanguage(string)` | 该成员不存在，公开面只有 `Current`/`Get`/`Refresh`/`GetStrings` | 引用它 → CS0117；正确：改 `CultureInfo.CurrentUICulture` 后调 `UI4MultiLanguage.Refresh()` |
| 2 | `UI4ComboBox` 注释列 `HoverBorderColor` / `FocusBorderColor` | 两 DP 在 `UI4ComboBox` 上不存在（只在 `UI4TextBox` 上存在），ComboBox 用 `FocusGradientStart` / `FocusGradientEnd` | 焦点边框是**渐变**，配 `FocusGradient*` |
| 3 | `UI4Panel` 注释提到 `Title` 属性 | 类内无该 DP（注释 `cref` 无效，即构建里的 CS1574 之一） | 卡片标题自己放子元素 |
| 4 | README 出现 `UI4ContextMenuLanguage.Refresh()` | 全仓库 `.cs` 无 `UI4ContextMenuLanguage` 类型 | 宿主照抄 → CS0103；右键菜单取词走 `UI4MultiLanguage.Get` |
| 5 | README / `PORTING.md` 第 6 节建议「可用 `MessageBox.Show` 临时确认运行时」 | 可行但会污染验证 | 本手册一律要求写在窗口文本里（第 03 章 §3.3.5），因为脚本依赖 UIA 与异常日志判定 |

## 1.4 推荐做法（Do / Don't）

- Do：每次写 API 名之前先 `grep` 库源码；Don't：相信记忆或文档里的属性名（`UI4ComboBox.HoverBorderColor` 就是文档有、源码无）。
- Do：把验证脚本当「成功定义」；Don't：把「窗口看起来对了」当成功定义（本机 WPF 抓屏全白，肉眼与截图都不可靠）。
- Do：一次改一个可验证单元（构建 + 一条断言）；Don't：批量改完再一起验证（错误会被级联掩盖，见 `PORTING.md` 第 8 节）。
- Do：按 §1.3.3 的路线读章；Don't：从第 18 章开始逐章通读（会在新项目上浪费两轮上下文）。
- Do：把「停下来问人」当成正常输出；Don't：为了「任务完成」自行扩大改动面到库源码或工程结构。

## 1.5 坑与排错

| 症状 | 根因 | 处置与判据 |
|---|---|---|
| Demo 的 XAML 报 MC3074「类型 UI4TextBlock 不存在」 | 库内 C# 编译失败导致 `StartUI4Controls.dll` 没产出，XAML 标记编译拿不到类型；xmlns 本身没错 | 跑 `dotnet build StartUI4Controls.sln` 看真实 C# 错误（IDE 错误列表会被级联掩盖）。判据：库错误修完后 MC3074 自动消失（`PORTING.md` 第 8 节实测） |
| 构建报 MSB3027 文件被占用 | 上一次跑起来的 `StartUI4Demo.exe` 还活着 | `taskkill //IM StartUI4Demo.exe //F` 后重建；脚本收尾统一 `Stop-Process` |
| 脚本刚切完主题就断言，色值还是旧主题 | UI4Theme 的刷新批在 `DispatcherPriority.Input`，脚本泵得不够深 | 先 `Dispatcher.Invoke(noop, ContextIdle)` 再 `Render`（`titlebar.ps1:25-34` 的 `Pump`） |
| 找不到要用的属性，怀疑手册写错 | 读的是过时文档（第 2 章 §文档陷阱清单） | 以源码为准；把不一致记进作业记录，不要顺手「修正」文档以外的实现 |
| UIA 定位不到 `UI4Switch` | 库内无一处 `OnCreateAutomationPeer`（全仓库 grep 只在 specs 与第三方 dll 里出现），没有 Toggle pattern | 给控件设 `x:Name`（→ `AutomationId`）或按 Name/坐标点击（`theme.ps1` 即按 Name 找「深色模式」） |

## 1.6 完成判据

逐条自查，全部为「是」才算读完本章：

1. 我能说出自己这次任务落在 §1.3.3 的哪一行，并给出章号序列。
2. 我列出本次要用到的每个 API 名，且每个都已在 `src/StartUI4Controls/` grep 命中（贴出命中文件与行号）。
3. 我知道本次改动的最小验证序列，并已把 §1.3.4-B 的命令抄进作业记录。
4. 我的自查清单每条都带判据与证据位置，没有「确认正常」式条目。
5. 我确认没有把库源码修改、新依赖、框架级改动列进本次计划；若有，已按 §1.3.4-C 停下来问人。
6. 我能解释「资源桥键是原地覆盖的，所以宿主必须用 `DynamicResource`」，并能在 `UI4Theme.cs:297-308` 指出这段代码。
7. 我能背出 §1.3.4-A′ 的四级约束优先级，并说出「本手册与源码冲突时怎么办」。
8. 我能按 §1.3.5 说出六步流程的次序，并说出第 2 步与第 4 步为什么不能合并。
9. 我把准备写进代码的每个属性名对过 §1.3.6 的 5 条不一致，无一命中。

### 术语中英对照（全书统一用词）

| 中文 | 英文 / 源码名 | 本手册中的含义 |
|---|---|---|
| 库 / 组件库 | the library | `src/StartUI4Controls/`，程序集名 `StartUI4Controls`，NuGet 包 id 为 `StartUI4.WPF`（两者不同名，是本手册最常见的混淆点） |
| Demo | 示例工程 | `samples/StartUI4Demo/`，唯一可编译运行的用法样板；不是产品代码规范，但它是「可抄的真相」 |
| 宿主 | host / 业务应用 | 你正在新建的应用：引用库、写自己的 `App`/`Window`；本手册全部面向宿主侧作业 |
| 客户区 | client area | WPF 自己绘制的区域，`Window.Content` 与 `Background` 都落在这里 |
| 非客户区 | non-client area | 标题栏、边框、圆角、投影，由系统绘制，WPF 属性够不着 |
| 令牌 | token / `UI4ThemeToken` | 30 个语义颜色名（`Accent`、`Background`、`BorderNormal`…），主题的唯一调色来源 |
| 资源桥 | resource bridge | 把令牌写成 `UI4.Color.X` / `UI4.Brush.X` 资源键供 XAML 消费的机制（`UI4Theme.cs:297-308`） |
| 作用域 | scope / `UI4ThemeScope` | 在子树上声明另一个主题键，使子树与全局互不干扰 |
| 命令式（刷新） | imperative | 控件在切主题时被回调、用 C# 重刷外观（`IThemeAware` + `TrackControl`），`IThemeAware` 为 internal |
| 引用式（刷新） | reference-based | 控件构造时对 DP 调 `SetResourceReference`，靠资源继承自动跟随 |
| 有效主题 | effective theme / `EffectiveThemeFor`（internal） | 元素在当前作用域链上实际应用的主题实例 |
| 主题代号 | `ThemeVersion`（internal） | 每次全局主题重写自增，用于去重「本版本已染过色的窗口」 |
| 换入栈 | theme stack | 刷新作用域子树时临时把全局主题换成作用域主题，仅 UI 线程同步可用 |
| DWM | Desktop Window Manager | Vista 起的桌面合成组件，库经 `DwmSetWindowAttribute` 表达标题栏意图 |
| UIA | UI Automation | 自动化走查所用树；本库控件默认无专属 `AutomationPeer` |
| 依赖属性 | DP / DependencyProperty | 外观几乎全部由 DP 暴露；赋本地值即停止主题跟随（`Internal/ThemeSync` 语义） |
| BAML | compiled XAML | XAML 编译后进 `.g.resources`，故库内无 `.xaml` 文件也能出模板 |
