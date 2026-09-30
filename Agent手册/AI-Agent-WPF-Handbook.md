# 面向 AI Agent 的 WPF 业务应用接入操作手册

> 本手册面向要在本组件库之上新建 WPF 业务应用、并继续做业务开发的 AI Agent。
> 全文 24 章分五个区（分区口径同第 01 章 §1.3.2）：第 01–05 章认知与准备，第 06–11 章应用骨架，
> 第 12–17 章主题与品牌，第 18–22 章控件实操，第 23–24 章业务接入与交付。
> 按任务查章请先读第 01 章 §1.3.3 的任务路由表。

本文件由 `Agent手册/merge-manual.ps1` 从 `Agent手册/chNN-*.md` 生成，请勿直接编辑合并产物；
要改内容请改对应章节文件后重新执行脚本。

## 目录

- 第 01 章 手册定位与 Agent 作业流程
  - 1.1 目标与验收
  - 1.2 源码依据（必读文件清单）
  - 1.3 正文
  - 1.4 推荐做法（Do / Don't）
  - 1.5 坑与排错
  - 1.6 完成判据
- 第 02 章 仓库全景与真相源地图
  - 2.1 目标与验收
  - 2.2 源码依据
  - 2.3 正文
  - 2.4 推荐做法（Do / Don't）
  - 2.5 坑与排错
  - 2.6 完成判据
- 第 03 章 开发环境与首次构建
  - 3.1 目标与验收
  - 3.2 源码依据
  - 3.3 正文
  - 3.4 推荐做法（Do / Don't）
  - 3.5 坑与排错
  - 3.6 完成判据
- 第 04 章 net48 与 C# 7.3 硬约束
  - 4.1 目标与验收
  - 4.2 源码依据
  - 4.3 正文
  - 4.4 推荐做法（Do / Don't）
  - 4.5 坑与排错
  - 4.6 完成判据
- 第 05 章 组件库能力总览与选型决策
  - 5.1 目标与验收
  - 5.2 源码依据
  - 5.3 正文
  - 5.4 推荐做法（Do / Don't）
  - 5.5 坑与排错
  - 5.6 完成判据
- 第 06 章 新建应用的接入路径与最小可运行工程
  - 6.1 目标与验收
  - 6.2 源码依据
  - 6.3 正文
  - 6.4 推荐做法（Do / Don't）
  - 6.5 坑与排错
  - 6.6 完成判据
- 第 07 章 工程配置与依赖治理
  - 7.1 目标与验收
  - 7.2 源码依据
  - 7.3 正文
  - 7.4 推荐做法（Do / Don't）
  - 7.5 坑与排错
  - 7.6 完成判据
- 第 08 章 App 启动序列与生命周期收尾
  - 8.1 目标与验收
  - 8.2 源码依据
  - 8.3 正文
  - 8.4 推荐做法（Do / Don't）
  - 8.5 坑与排错
  - 8.6 完成判据
- 第 09 章 主窗口骨架与页组织
  - 9.1 目标与验收
  - 9.2 源码依据
  - 9.3 正文
  - 9.4 推荐做法（Do / Don't）
  - 9.5 坑与排错
  - 9.6 完成判据
- 第 10 章 XAML 编写规范与命名约定
  - 10.1 目标与验收
  - 10.2 源码依据
  - 10.3 正文
  - 10.4 推荐做法（Do / Don't）
  - 10.5 常见 XAML 报错 → 真因对照
  - 10.6 完成判据
- 第 11 章 导航容器选型与落地
  - 11.1 目标与验收
  - 11.2 源码依据
  - 11.3 正文
  - 11.4 推荐做法（Do / Don't）
  - 11.5 坑与排错
  - 11.6 完成判据
- 第 12 章 主题引擎的工作方式与生效时序
  - 12.1 目标与验收
  - 12.2 源码依据
  - 12.3 正文
  - 12.4 推荐做法（Do / Don't）
  - 12.5 坑与排错
  - 12.6 完成判据
- 第 13 章 设计令牌与资源桥
  - 13.1 目标与验收
  - 13.2 源码依据
  - 13.3 正文
  - 13.4 推荐做法（Do / Don't）
  - 13.5 坑与排错
  - 13.6 完成判据
- 第 14 章 局部作用域主题（UI4ThemeScope）
  - 14.1 目标与验收
  - 14.2 源码依据
  - 14.3 正文
  - 14.4 推荐做法（Do / Don't）
  - 14.5 坑与排错
  - 14.6 完成判据
- 第 15 章 自定义主题、强调色与品牌化
  - 15.1 目标与验收
  - 15.2 源码依据
  - 15.3 正文
  - 15.4 推荐做法（Do / Don't）
  - 15.5 坑与排错
  - 15.6 完成判据
- 第 16 章 系统跟随、持久化与高对比度
  - 16.1 目标与验收
  - 16.2 源码依据
  - 16.3 正文
  - 16.4 推荐做法（Do / Don't）
  - 16.5 坑与排错
  - 16.6 完成判据
- 第 17 章 窗口标题栏与 DWM 染色
  - 17.1 目标与验收
  - 17.2 源码依据
  - 17.3 正文
  - 17.4 推荐做法（Do / Don't）
  - 17.5 坑与排错
  - 17.6 完成判据
- 第 18 章 基础交互控件实操
  - 18.1 目标与验收
  - 18.2 源码依据
  - 18.3 正文
  - 18.4 推荐做法（Do / Don't）
  - 18.5 坑与排错
  - 18.6 完成判据
- 第 19 章 文本与输入控件实操
  - 19.1 目标与验收
  - 19.2 源码依据
  - 19.3 选型表
  - 19.4 UI4TextBlock：它是 ContentControl
  - 19.5 UI4FlipTextBlock：翻牌数字
  - 19.6 UI4TextBox：输入框
  - 19.7 UI4PasswordBox：它继承 TextBox
  - 19.8 UI4CodeEditor：AvalonEdit 包装
  - 19.9 一屏输入表单（可抄）
  - 19.10 尺寸与可读性
  - 19.11 推荐做法（Do / Don't）
  - 19.12 坑与排错
  - 19.13 完成判据
- 第 20 章 数据展示：列表、卡片视图与下拉
  - 20.1 目标与验收
  - 20.2 源码依据（必读）
  - 20.3 UI4ListBox：带样式的列表
  - 20.4 UI4ListView 与 UI4GridView：卡片视图（都不跟随主题）
  - 20.5 UI4ComboBox：焦点渐变与尺寸公式
  - 20.6 绑定数据源与“数据表”选型
  - 20.7 Do / Don't
  - 20.8 坑与排错
  - 20.9 完成判据（自查）
- 第 21 章 容器、滚动与「主题盲区」清单
  - 21.1 目标与验收
  - 21.2 源码依据（必读）
  - 21.3 UI4Panel：卡片容器
  - 21.4 UI4Grid 与 UI4ScrollViewer
  - 21.5 主题盲区总表
  - 21.6 布局经验
  - 21.7 页面模板库（可直接粘贴，均用 DynamicResource 令牌）
  - 21.8 Do / Don't
  - 21.9 坑与排错
  - 21.10 完成判据（自查）
- 第 22 章 菜单、对话框与系统托盘
  - 22.1 目标与验收
  - 22.2 源码依据（必读）
  - 22.3 UI4Menu：顶部菜单条
  - 22.4 UI4ContextMenu：纯代码式右键菜单
  - 22.5 UI4MessageBox 与 UI4ColorPicker
  - 22.6 UI4NotifyIcon：全生命周期
  - 22.7 Do / Don't
  - 22.8 坑与排错
  - 22.9 完成判据（自查）
- 第 23 章 本地化、图标资源与业务数据接入
  - 23.1 目标与验收
  - 23.2 源码依据（必读）
  - 23.3 UI4MultiLanguage 真实用法
  - 23.4 业务应用自己的资源策略
  - 23.5 图标与静态资源
  - 23.6 数据接入现实与最小 MVVM
  - 23.7 异常、日志与配置持久化
  - 23.8 Do / Don't
  - 23.9 坑与排错
  - 23.10 完成判据（自查）
- 第 24 章 自动化验证、故障定位与交付
  - 24.1 目标与验收
  - 24.2 源码依据（必读）
  - 24.3 验证哲学
  - 24.4 工具链表：证明什么、怎么读输出
  - 24.5 脚本编写约定（照抄）
  - 24.6 为新业务应用搭最小自检（ASCII-only 模板）
  - 24.7 症状 → 定位路径 决策表
  - 24.8 回归执行顺序与预期
  - 24.9 发布产物清单与 4.8 依赖检查
  - 24.10 交付前 Agent 自查表
  - 24.11 坑与排错
  - 24.12 完成判据

---

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

---

# 第 02 章 仓库全景与真相源地图

> 本章解决：在 30 秒内定位「某个 API 在哪个文件、某类页面在哪个样板、某条事实该信哪份文档」。
> 读者：AI Agent（要在本项目之外新建/续写 WPF 业务代码）
> 前置章节：第 1 章

## 2.1 目标与验收

1. 能在不看文档的情况下说出：解决方案里有几个可构建工程、它们的 TFM/语言级别/输出类型。
2. 能给出任一公开控件对应的源文件路径（含两个易混文件名：`UI4TabControl.cs`、`UI4PasswordBox.cs`）。
3. 能背出真相源优先级序列，并解释每一级的用途与失效方式。
4. 能在动手前判断「我准备写的这个 API 名是文档陷阱」——本章 §2.3.5 列出 9 条已核实陷阱。
5. 能为「新页面要找样板」给出具体动作：`--tab=N` 直达 + 在 `MainWindow.xaml` 里 grep `TabItem Header`。
6. 能说出根目录每个目录/脚本的职责，以及「哪些路径永远不该当事实来源」（§2.3.7）。
7. 给定一类需求（令牌色值、作用域、标题栏、持久化…），能立刻说出权威出处的文件与行号段（§2.3.8）。

## 2.2 源码依据

- `StartUI4Controls.sln:6-13`（两个解决方案文件夹 + 两个 C# 工程条目；`StartUI4Demo` 列在首位）、`:24-47`（六组配置映射，x64/x86 全部落到 `Debug|Any CPU` / `Release|Any CPU`）
- `src/StartUI4Controls/StartUI4Controls.csproj:1-40`（全部属性与两个 `PackageReference`、`<Reference Include="System.Drawing" />`）
- `samples/StartUI4Demo/StartUI4Demo.csproj:1-20`（`WinExe`、`ApplicationManifest`、唯一 `ProjectReference`）
- `src/StartUI4Controls/AssemblyInfo.cs:1-8`（只有 `ThemeInfo`，**没有** `XmlnsDefinition`）
- `src/StartUI4Controls/UI4ComboBox.cs:26`、`UI4Panel.cs:21`、`UI4MultiLanguage.cs:29-31`（三处注释与实现不符，见 §2.3.5）
- `src/StartUI4Controls/UI4ColorPicker.cs:790-794`（实例 `Show` 实际调用 `base.ShowDialog()`）
- `src/StartUI4Controls/UI4TextBlock.cs:254-258`、`:354-357`（无 `TrackControl`、无 `SetResourceReference`，靠继承）
- `samples/StartUI4Demo/MainWindow.xaml:33,77,107,140,187,223,283,344,374,397,444`（11 个 `TabItem Header`，即页索引）
- `samples/StartUI4Demo/MainWindow.xaml.cs:49-62`（`--tab=N` 解析，0 基，越界静默忽略）
- §2.3.2 的 43 行清单来自 `Glob src/StartUI4Controls/**/*.cs` 实测 + 每个文件首个类型声明行的 grep（`wc -l` 给行数）；
  §2.3.7 的目录清单来自仓库根 `ls`，实测 `_recovered/` 为空目录、`shots/` 有 18 张 png、`.qoder/specs/` 有 7 份规格、`常见问题清单/` 有 2 份
- §2.3.8 索引表里每个行号都已在源码核实：`UI4Theme.cs:16,106,149,161,283,297-308,678`、`UI4ThemeDefinition.cs:94,130,170`、
  `UI4ThemeToken.cs:7`、`UI4ThemeScope.cs:21-38`、`UI4ThemePersistence.cs:11,19,54`、`UI4TabControl.cs:15,82,136,694`、
  `UI4Pivot.cs:13,15-22`、`UI4NavigationView.cs:17,30,682`、`UI4Menu.cs:13,292,354`、`UI4ContextMenu.cs:16,30,51,75`、
  `UI4ComboBox.cs:55-64`、`UI4TextBox.cs:49,58,459,470,484`、`UI4ColorPicker.cs:790-794`、`UI4MultiLanguage.cs:10,37,47,52,57`

## 2.3 正文

### 2.3.1 解决方案与工程结构

```text
StartUI4Controls.sln                 56 行；无 net48 字样（TFM 只来自 csproj）
├── 解决方案文件夹 src                 {2150E333-…} 仅组织结构
│   └── StartUI4Controls.csproj        类库（SDK 风格），net48，AssemblyName=StartUI4Controls
└── 解决方案文件夹 samples
    └── StartUI4Demo.csproj            WinExe，net48，只引用上面那个工程
```

- 两个工程都是 **SDK 风格**（`<Project Sdk="Microsoft.NET.Sdk">` + `UseWPF=true`），没有 `packages/` 目录、没有 `.csproj.user` 依赖。
- sln 声明 6 个「配置|平台」组合，但 x64/x86 都 `ActiveCfg = Debug|Any CPU`（`sln:26-29`、`:38-41`）：
  **在 VS 里选 x64 不会产出 64 位专用程序集**。位宽由 csproj 侧（`Prefer32Bit` 等）决定，与 sln 无关。
- sln 里没有任何「4.8」字样（`grep -c "4\.8" StartUI4Controls.sln` = 0；三个 4.8 的真正来源见第 03 章 §3.3.5）。
  仓库根各目录/文件该谁动、不该谁动，见 §2.3.7。
- 项目条目顺序：`StartUI4Demo` 在前（`sln:10`），因此它是 VS 默认启动项目；但**用户级 `.suo` 会覆盖这个选择**（`.vs/StartUI4Controls/`），
  这就是「按 F5 提示无法启动类库项目」的根因（修法见第 3 章 §3.4）。

### 2.3.2 库内 43 个源文件逐行速查

`src/StartUI4Controls/` 共 43 个 `.cs`（顶层 38 + `Internal/` 5，清单由 `Glob **/*.cs` 实测得到），约 1.59 万行。
下表一行一个文件，给出真实行数、职责与必须知道的坑；标 **internal** 的宿主不可使用。注意文件与类型并非一一对应
（`UI4Tab`/`UI4TabItem` 都在 `UI4TabControl.cs`，`UI4ContextMenu.cs` 里没有 `Control`）。

| 分组 | 文件（行数） | 一句话职责 |
|---|---|---|
| 主题引擎 | `UI4Theme.cs`(732) | 模式枚举 `UI4ThemeMode`(:16)、`SetTheme`(:106)/`Apply`(:149)/`Register`(:161)/`ApplyToApplication`(:283)/`SetAccent`(:678)、资源键唯一来源 `WriteTokens`(:297-308)；`IThemeAware`(:728)、`TrackControl`、`EffectiveThemeFor` 都在本文件且为 internal |
| 主题引擎 | `UI4ThemeDefinition.cs`(205) | 主题「配方」数据类：`Key`/`GetColor`/`Has`/`With`/`Clone`，内置 `Light()`(:94)、`Dark()`(:130)、`HighContrast()`(:170) |
| 主题引擎 | `UI4ThemeToken.cs`(40) | 30 个颜色令牌枚举(:7)，资源键名 `UI4.Color.<Token>` / `UI4.Brush.<Token>` 的来源 |
| 主题引擎 | `UI4ThemeScope.cs`(183) | 局部作用域：`ThemeProperty`/`SetTheme`/`GetTheme`(:21-38)；`null`/空白/未注册键 = 撤销 |
| 主题引擎 | `UI4ThemePersistence.cs`(89) | `IThemePersistence`(:11) + `RegistryThemePersistence`(:19) + `JsonThemePersistence`(:54，手写一行 JSON) |
| 元数据 | `AssemblyInfo.cs`(8) | 只有 `[assembly: ThemeInfo(...)]`，**没有 `XmlnsDefinition`** ⇒ 宿主必须写 `assembly=StartUI4Controls` |
| 标题栏 | `UI4WindowTitleBar.cs`(201) | DWM 非客户区染色静态类：四条通路、`Apply(Window)`、`ToColorRef`，CWT 去重表 |
| 输入与选择 | `UI4TextBox.cs`(499) | 引用式文本框（占位符/清除按钮/内置右键菜单）；同文件带 `BoolToVisibilityConverter`(:459)、`PlaceholderVisibilityConverter`(:470)、`InnerPaddingConverter`(:484)；`HoverBorderColor`(:49)/`FocusBorderColor`(:58) **只在本类型上存在** |
| 输入与选择 | `UI4PasswordBox.cs`(810) | **继承 `TextBox`** 的密码框：`Password` 是明文 `string` DP，`ClearPassword()`(:549)，`Unloaded` 清密码；高安全场景用原生 `PasswordBox` |
| 输入与选择 | `UI4ComboBox.cs`(500) | 命令式下拉；焦点边框是渐变 `FocusGradientStart/End`(:55-64)；注释里的 `HoverBorderColor/FocusBorderColor` 不存在 |
| 输入与选择 | `UI4Slider.cs`(286) | 引用式滑块（`TrackBackground` 默认色与令牌不一致） |
| 输入与选择 | `UI4CircleSlider.cs`(420) | 环形取值 `ContentControl`；不跟随主题 |
| 开关与勾选 | `UI4Button.cs`(190) | 命令式按钮；三种主题下都是同一套蓝→紫渐变（渐变未令牌化） |
| 开关与勾选 | `UI4CheckBox.cs`(259) | 引用式勾选框；`BoxCornerRadius` 是 `[Obsolete]` 别名 DP，与 `CornerRadius` 同一份 |
| 开关与勾选 | `UI4Radio.cs`(210) | 引用式单选；`TextColor` 默认 `Colors.Black`（≠ 令牌 `#1E1E1E`） |
| 开关与勾选 | `UI4Switch.cs`(337) | 引用式开关 `: Control`，`IsOn` 默认双向 + `Toggled` 路由事件；`ArrangeOverride` 固定尺寸；**无 AutomationPeer** |
| 文本与展示 | `UI4TextBlock.cs`(367) | **`ContentControl`**（不是 WPF `TextBlock`）；`TextOrContentConverter` 为 internal(:15)；`GradientStart/End` 是死属性；4 个 `new` DP 有基类歧义 |
| 文本与展示 | `UI4FlipTextBlock.cs`(578) | 命令式数字翻转块，`FontSize` 默认 60，有 `ReadLocalValue` 保护 |
| 文本与展示 | `UI4ProgressBar.cs`(358) | 引用式进度条，渐变起止已挂令牌 |
| 文本与展示 | `UI4ProgressRing.cs`(458) | 环形进度 `ContentControl`；不跟随主题；3 个 CS0414 只写不读字段 |
| 列表与卡片 | `UI4ListBox.cs`(441) | 命令式列表（`ListStyleType` = None/Disc/Number）；`IndexPlusOneConverter`(:32)、公开 `RefreshTheme()`(:264) 与泄漏的公开静态字段 `_scrollViewerStyle`(:212) |
| 列表与卡片 | `UI4ListView.cs`(595) | 单行平铺卡片带，`: ListBox`；**不跟随主题**；`XamlReader` 内嵌滚动条样式 |
| 列表与卡片 | `UI4GridView.cs`(650) | 自动列数网格，`: ListBox`；`UpdateColumns`(:235)，`ItemWidth` 为 `NaN` 时直接返回；不跟随主题 |
| 导航与页签 | `UI4TabControl.cs`(762) | 装着 `UI4TabItem`(:15，`IsBrand`:82) 与 `UI4Tab`(:136) 与 `TabCloseRoutedEventArgs`(:694)；**没有 `UI4Tab.cs`**；不跟随主题 |
| 导航与页签 | `UI4Pivot.cs`(381) | 引用式 `UI4Pivot` + `UI4PivotItem`(:13，`IsBrand`:15-22 钉最前) |
| 导航与页签 | `UI4NavigationView.cs`(906) | 命令式侧边导航 `: ItemsControl`；`RegularItems`/`BottomItems`(:682)；`NullToVisibilityConverter`(:17)/`InverseNullToVisibilityConverter`(:30) |
| 容器 | `UI4Panel.cs`(309) | 命令式卡片：`BorderColor` 是 `Color`；用 `new` 重定义 `BorderBrush/BorderThickness`；注释里的 `Title` 不存在 |
| 容器 | `UI4Grid.cs`(22) | `: Grid`，构造写死 `#E1ECF5→#FFFFFF` 渐变，主题中性（任何主题都浅色） |
| 容器 | `UI4ScrollViewer.cs`(163) | `: ScrollViewer` + `IsSmoothScrollEnabled`(:36) 与 `SmoothScrollTo…Offset`(:117,:124)；吃掉滚轮、不跟随主题；2 个 CS0414 字段 |
| 弹出与系统 | `UI4MessageBox.cs`(287) | 命令式对话框 `: Window`；`UI4MessageBoxButtons` 枚举在同文件；静态 `Show(...)` → `bool?` |
| 弹出与系统 | `UI4ColorPicker.cs`(796) | 命令式取色 `: Window`；实例 `Show(Window)`(:790-794) 内部是 `base.ShowDialog()`，**实为模态** |
| 弹出与系统 | `UI4ContextMenu.cs`(497) | 代码式右键菜单（**普通对象**）+ `UI4MenuItemType`(:16) + `UI4MenuItem`(:30) + `UI4MenuIcons`(:75) + 公开扩展方法 `GeometryHelper`(:51) |
| 弹出与系统 | `UI4Menu.cs`(371) | 命令式菜单栏 `: Menu`；`ObjectIsStringConverter`(:13)、`UI4MenuElementItem`(:292)、`UI4MenuSeparatorElement`(:354)；`XamlReader` 内嵌样式 |
| 弹出与系统 | `UI4NotifyIcon.cs`(717) | P/Invoke 托盘 `: FrameworkElement, IDisposable`；`PopupActivationMode` 枚举与 `UI4TrayMenuItem` 同文件；需宿主引用 `System.Drawing` |
| 编辑器 | `UI4CodeEditor.cs`(91) | `: ICSharpCode.AvalonEdit.TextEditor`；XAML 一旦触及它，`ICSharpCode.AvalonEdit.dll` 即运行必需 |
| 死码 | `UI4DataGrid.cs`(549) | **internal**，SQLite 临时库表格，全仓库无引用 ⇒ 表格用原生 `DataGrid` |
| 死码 | `UI43DSphere.cs`(553) | **internal**，Media3D 纹理球，全仓库无引用 |
| 本地化 | `UI4MultiLanguage.cs`(223) | 静态取词：`UI4LanguageKey`(:10)、`Current`(:37)、`Get`(:47)、`Refresh`(:52)、`GetStrings`(:57)；**没有 `SetLanguage`**（注释 `:29` 的 `cref` 即 CS1574 之一）；构造期拉取 |
| 内部设施 | `Internal/ThemeSync.cs`(30) | **internal**：「跟随主题但尊重用户显式改动」的 DP 助手 |
| 内部设施 | `Internal/ScrollBarResources.cs`(568) | **internal**：库内滚动条样式（`XamlReader` 内嵌 XAML 字符串） |
| 内部设施 | `Internal/WindowResizeBehavior.cs`(134) | **internal**：对话框/取色器窗口的缩放行为 |
| 内部设施 | `Internal/WindowAnimationHelper.cs`(111) | **internal**：窗口淡入/位移动画助手 |
| 内部设施 | `Internal/ColorToBrushConverter.cs`(29) | **internal**：`Color`→`Brush` 转换器 |

三条读法纪律：

1. **同文件里还藏着公开转换器**：`BoolToVisibilityConverter`(`UI4TextBox.cs:459`)、`PlaceholderVisibilityConverter`(`:470`)、
   `InnerPaddingConverter`(`:484`)、`IndexPlusOneConverter`(`UI4ListBox.cs:32`)、`ObjectIsStringConverter`(`UI4Menu.cs:13`)、
   `NullToVisibilityConverter` / `InverseNullToVisibilityConverter`(`UI4NavigationView.cs:17,30`)。grep 属性名时顺带命中它们属正常。
2. **DP 声明集中在文件上半部**，模板构建在下半部（`BuildXxxStyle()`）。要看「有没有这个属性」只看上半部即可。
3. 库内**无 `.xaml` 文件**：模板用 `FrameworkElementFactory` 建；少数 Style 用 `XamlReader` 解析内嵌字符串
   （`UI4ListView`/`UI4GridView.CreateScrollViewerStyleFromXaml`、`UI4Menu`、`Internal/ScrollBarResources`）。
   因此不要去找 `Themes/generic.xaml`，也不要指望 `StaticResource` 拿到库内私有资源。

### 2.3.3 Demo 工程文件与行数（唯一可运行样板）

| 文件 | 行数 | 作用 |
|---|---|---|
| `App.xaml` | 19 | 只定义 `SectionTitle` / `SectionHint` 两个样式；`StartupUri`、`ShutdownMode="OnMainWindowClose"` |
| `App.xaml.cs` | 53 | `UI4Theme.ApplyToApplication()`（`:17`）+ 两级异常钩子写 `demo-errors.log`（`:35-51`） |
| `MainWindow.xaml` | 565 | 三段式外壳 + 一个 `TabControl`（`TabStripPlacement="Left"`）内联 11 页全部控件用法 |
| `MainWindow.xaml.cs` | 318 | `DataContext = this`；事件命名 `<Element>_<Event>`；`--tab=N`；托盘/菜单清理 |
| `ScopeWindow.xaml` | 47 | 整窗作用域示例（异主题窗口） |
| `ScopeWindow.xaml.cs` | 63 | 作用域翻转/撤销/高对比度三个按钮 |
| `app.manifest` | 22 | `supportedOS` 三项 + `dpiAwareness PerMonitorV2, PerMonitor` |
| `StartUI4Demo.csproj` | 20 | 见 §2.3.1 |

Demo 里没有 `Pages/` 目录、没有 `UserControl`、没有 MVVM/DI 容器。这是**有意保留的现实**（第 6 章 §6.3.6 说明为什么手册尊重它）。

### 2.3.4 真相源优先级与各自用途

| 级 | 载体 | 用途 | 失效方式（怎么识别它过期了） |
|---|---|---|---|
| 1 | `src/StartUI4Controls/*.cs` | API 名、默认值、主题通道、行为的最终裁决 | 不会失效；只会被你读错（注意注释与实现分叉，见 §2.3.5） |
| 2 | `samples/StartUI4Demo/*.xaml(.cs)` | 可编译的用法样板、控件放哪、事件怎么接 | 该页被删或控件未被覆盖时找不到样板 → 回落到第 1 级 |
| 3 | 仓库根 `*.ps1` | 「成功」的定义：读哪个属性、断言哪个色值、泵几轮 | 脚本内硬编码 `bin\Debug\net48` 路径，改输出目录后需同步 |
| 4 | `README.md` | 是什么：控件一览、属性表、主题系统十章综述 | 属性表偶有把 item 挂错类型、残留已删类型（下表） |
| 5 | `PORTING.md` | 移植结论、改动清单、每次修复的根因与验证 | 第 4 节的目录树含已不存在的 `upstream/` |
| 6 | `主题方案分析与改进.md` | 架构、不足清单、P0–P3 记录、标题栏选型与改动清单 | 行号随重构漂移（用它讲原理，不要用它抄行号） |
| 7 | `常见问题清单/*.md` | ComboBox 文本溢出与高度两个专题的取证与公式 | 只覆盖两个问题，别当通用尺寸规范 |
| 8 | `src/StartUI4Controls/.qoder/specs/*.md` | 历史任务背景（当时的改动计划） | 描述的是历史代码形状，含旧绝对路径；**禁止作为 API 依据** |
| 9 | `src/StartUI4Controls/README.md` | 架构审计报告（好/坏评价） | 写于 2026-09-29，行号与文件数已明显漂移 |

规则：**下一级与上一级冲突时，以上一级为准，并在作业记录里写下冲突**，不要顺手改上级。

### 2.3.5 已核实的过时文档陷阱清单

| # | 陷阱 | 证据（源码位置） | 正确写法 |
|---|---|---|---|
| 1 | `UI4MultiLanguage.SetLanguage(string)` 不存在 | 注释 `UI4MultiLanguage.cs:29` 用它做 `cref`；类内公开成员只有 `Current`/`Get`/`Refresh`/`GetStrings`（`:37-78`） | 改 `CultureInfo.CurrentUICulture` 后调 `UI4MultiLanguage.Refresh()` |
| 2 | 注释称支持「中文繁体」 | `UI4MultiLanguage.cs:31`；`GetStrings` 分支只有 `zh ja ko de fr es ru` + 默认 en（`:57-77`） | 无繁中分支；`zh-TW` 会落到 `zh` 简体检 |
| 3 | `UI4ComboBox.HoverBorderColor` / `FocusBorderColor` 不存在 | 注释 `UI4ComboBox.cs:26`；全文件只有 `BorderNormalColor` / `FocusGradientStart` / `FocusGradientEnd` | 焦点边框是渐变，配 `FocusGradient*` |
| 4 | `UI4Panel.Title` 不存在 | 注释 `UI4Panel.cs:21` 的 `cref="Title"`；类内无该 DP | 标题自己放子元素 |
| 5 | README 残留已删除的 `UI4ContextMenuLanguage` | `README.md:1317`；源码全仓库无该类型（只存在于 specs） | 右键菜单取词统一走 `UI4MultiLanguage.Get`，宿主写错会 CS0103 |
| 6 | `UI4ColorPicker.Show(...)` 名字像非模态 | `UI4ColorPicker.cs:790-794` 内 `return base.ShowDialog();` | 它**是模态**；需要非模态请自建窗口或用 `Window` 派生类 |
| 7 | README 把 `IsBrand` 挂在 `UI4Tab` / `UI4Pivot` 的属性表里 | 实际 DP 声明在 `UI4TabItem`（`UI4TabControl.cs:82-88`）与 `UI4PivotItem`（`UI4Pivot.cs:15-22`）；`UI4Tab`/`UI4Pivot` 类体内无 `IsBrand` | 品牌项写在**项**上，不写在容器上 |
| 8 | `src/StartUI4Controls/README.md` 的行号/文件数过期 | 该文件称 `IThemeAware` 在 `UI4Theme.cs` 303-306、库共 27 个源文件；实际 `IThemeAware` 在 `UI4Theme.cs:728`，库为 43 个 `.cs` | 引用审计报告只取结论，行号一律重查 |
| 9 | `PORTING.md:94` 目录树列有 `upstream/` | 当前仓库副本根目录实测无 `upstream`（`ls` 不存在） | 不要按它做路径规划 |

另有一条属于「共用基线自身需修正」：`Agent手册/事实速查.md` §6 把 `UI4TextBlock` 标为命令式；
实测 `UI4TextBlock.cs` 既无 `UI4Theme.TrackControl` 也无 `SetResourceReference`（构造仅 `:254-258` 设 `Style`），
其前景靠 **继承**——`BuildTextStyle()` 只在 `Foreground != null` 时才写内部 `TextBox` 的前景色（`:354-357`），
未赋值时沿用宿主 `TextElement.Foreground`。`主题方案分析与改进.md:248` 同结论。第 5 章矩阵按「继承跟随」处理。

### 2.3.6 为新页面快速找到可抄的样板

1. 先定页：`--tab=N`（0 基）直达，`shot.ps1 -Tab N` 可顺手存档。索引：
   0 按钮与开关 / 1 文本输入 / 2 文本显示 / 3 选择器 / 4 进度指示 / 5 列表与网格 / 6 导航容器 / 7 布局面板 / 8 对话框 / 9 菜单与托盘 / 10 局部主题。
2. 再定位行：在 `samples/StartUI4Demo/MainWindow.xaml` 里 grep `TabItem Header`（11 个命中，行号见 §2.2）。
3. 抄 XAML 后必抄配套 code-behind：同名处理器在 `MainWindow.xaml.cs`（例：`BrowserTab_AddTab` `:157-170`、
   `ScopeKeyCombo_SelectionChanged` `:218-227`），**空判注释就写在 `:98-103` 与 `:220`**。
4. 若 Demo 未覆盖该控件（`UI4CircleSlider` 有、`GeometryHelper` 无示例）：回到源码读 DP 区，再用一行最小 XAML 自测。

### 2.3.7 仓库根目录职责一览（谁该动、谁不该动）

| 路径 | 类型 | 职责 | Agent 处置 |
|---|---|---|---|
| `StartUI4Controls.sln` | 根文件 | 两个工程条目 + `src`/`samples` 两个解决方案文件夹；6 组「配置\|平台」映射 | 仅「同解决方案接入」（第 06 章路径 A）时新增宿主工程条目；其余不动 |
| `src/StartUI4Controls/` | 库工程 | 43 个 `.cs`（顶层 38 + `Internal/` 5），产出 `StartUI4Controls.dll` 与 nupkg | **只读**；改库源码 = 第 01 章 §1.3.4-C 第 1 条 |
| `src/StartUI4Controls/.qoder/specs/` | 历史规格 | 7 份当时的任务规格文档 | 只作背景，**禁止**作为 API 依据 |
| `src/StartUI4Controls/README.md` | 审计报告 | 库内好/坏评价 | 只取结论；行号与文件数以源码为准 |
| `samples/StartUI4Demo/` | 示例工程 | 7 个源文件，唯一可运行样板 | 只读；业务页写进你自己的工程目录，不要往 Demo 里塞 |
| `shots/` | 脚本产物 | 18 张 png（`shot.ps1` / `interact.ps1` 落图） | 只读参考；本机抓 WPF 常全白，别拿它当颜色判据 |
| `常见问题清单/` | 专题文档 | 2 份 `UI4ComboBox` 取证（选中项文本溢出、文本高度显示不全） | 只读；动 `UI4ComboBox` 尺寸/模板前先读 |
| `Agent手册/` | 本手册 | `编写规范.md` + `事实速查.md` + `ch01`…`ch24` + `merge-manual.ps1` + 生成物 `AI-Agent-WPF-Handbook.md`（`_弃稿/` 仅留档，不合并） | 按章号读写 `chNN-*.md`；改完重跑 `merge-manual.ps1` 再生成大文档；规范与基线不改，合并产物不手改 |
| 根 `*.ps1`（8 个） | 验证工具链 | `titlebar` / `titlebar-live` / `p3verify` / `p2verify` / `scopewalk` / `theme` / `interact` / `shot` | 只读，它们是「成功」的定义；自定义脚本放你自己的仓库 |
| `PORTING.md` / `主题方案分析与改进.md` / `README.md` | 说明性文档 | 移植记录 / 方案论证 / 用户文档 | 只读；与本手册冲突时按第 01 章 §1.3.4-A′ 裁决 |
| `GIT-PROXY.txt` | 排障笔记 | GitHub HTTPS/代理连通排障说明 | 与构建无关，忽略 |
| `LICENSE.txt` / `.gitignore` | 仓库杂项 | MIT 许可 / VS 系忽略模板（`.gitignore` 是上游模板，非工程配置） | 不动 |
| `_recovered/` / `.vs/` / `bin/` / `obj/` / `.git/` | 本机或产物 | `_recovered/` 实测为空目录；`.vs/` 存 per-user 启动项与断点；`bin`/`obj` 是构建产物 | **一律不作为事实来源**；`bin/*/net48/StartUI4Controls.xml` 会淹没 grep 结果 |

### 2.3.8 「要找某能力的权威出处」索引表

| 我要找 | 权威出处（已核实行号） | 备注 |
|---|---|---|
| 主题模式枚举 | `UI4Theme.cs:16`（`UI4ThemeMode`） | `Light/Dark/System/HighContrast` |
| 切主题 / 按键应用 / 注册自定义 | `UI4Theme.cs:106`（`SetTheme`）、`:149`（`Apply`→`bool`）、`:161`（`Register`） | `Apply` 未知键返回 `false` 不抛 |
| 资源桥播种 | `UI4Theme.cs:283`（`ApplyToApplication`） | 宿主启动路径必调 |
| 资源键集合（唯一来源） | `UI4Theme.cs:297-308`（`WriteTokens`，internal） | 30 令牌 × 2 键 + 3 个别名 |
| 30 个令牌名 | `UI4ThemeToken.cs:7`（`enum UI4ThemeToken`） | 键名 = `UI4.Color.<Token>` / `UI4.Brush.<Token>` |
| 三主题实际色值 | `UI4ThemeDefinition.cs:94`（`Light`）、`:130`（`Dark`）、`:170`（`HighContrast`） | 断言的期望值从这里取，不从控件取 |
| 强调色覆盖 | `UI4Theme.cs:678`（`SetAccent(Color)`） | 会写回已注册 definition，测试要隔离 |
| 局部作用域 | `UI4ThemeScope.cs:21-38`（`ThemeProperty` / `SetTheme` / `GetTheme`） | 挂任意 `FrameworkElement` |
| 主题持久化 | `UI4ThemePersistence.cs:11`（`IThemePersistence`）、`:19`（`RegistryThemePersistence`）、`:54`（`JsonThemePersistence`） | 默认关闭，需显式赋 `UI4Theme.Persistence` |
| 标题栏 DWM 染色 | `UI4WindowTitleBar.cs`（201 行静态类；公开面 `Get/SetEnabled`、`Apply(Window)`、`ApplyOpenWindows()`、`SupportsCaptionColors`、`ToColorRef`） | 属性号与 COLORREF 见第 17 章 |
| 某控件的外观 DP | 该控件同名 `.cs` 的**上半部**；模板构建在下半部 | 例外：`UI4Tab`/`UI4TabItem` 在 `UI4TabControl.cs` |
| 库内滚动条样式 | `Internal/ScrollBarResources.cs` | internal，宿主不可使用 |
| 移植语法降级的逐条依据 | `PORTING.md` §1（不兼容矩阵）、§2（11 项改动处数） | 第 04 章据此写规则 |
| 上游遗留缺陷清单 | `PORTING.md` §3（6 条，含 5 处 CS0414 与 `UI4TextBlock` 死属性） | |
| 构建与运行时验证口径 | `PORTING.md` §5、§6；根 `*.ps1` | §6 建议的 `MessageBox` 自检法本手册不采纳（第 03 章 §3.3.5） |
| `.sln` 与「4.8」的关系 | `PORTING.md` §7（含 `grep -c "4\.8"` 复验命令） | 结论：sln 不含任何 4.8 字样 |
| MC3074 级联掩盖真实错误 | `PORTING.md` §8 | 判据与修法见第 03 章 §3.3.6 |
| 主题体系「为什么这么设计」 | `主题方案分析与改进.md` §一–§五（现状架构、不足清单 A1–A11、P0–P3 方案） | 只作背景；操作按第 12–16 章 |
| 标题栏三方案对比 / 改动清单 | `主题方案分析与改进.md` §十一、§十二 | `WindowStyle=None` 自绘路线已被否 |
| ComboBox 尺寸经验公式 | `常见问题清单/UI4ComboBox-文本高度显示不全.md`、`…选中项文本溢出.md` | DIP 不是 pt |

## 2.4 推荐做法（Do / Don't）

- Do：用「文件名 → 分组表」定位源码；Don't：用全文 `grep -r` 扫 `bin/`（`bin/*/net48/StartUI4Controls.xml` 是构建产物，会淹没真实声明）。
- Do：把 §2.3.5 当成检查表，写每个属性名前对一遍；Don't：把注释当规格——注释与实现分叉正是这 9 条的来源。
- Do：以 Demo 的 11 页为样板库；Don't：在 Demo 里找 `UserControl`/ViewModel 范例（不存在）。
- Do：发现新的文档/实现分叉就登记进本章；Don't：直接改 README 或 PORTING 来「对齐」（本手册不改既有文档）。
- Do：把 internal 清单（§2.3.2 末行 + `UI4DataGrid`/`UI43DSphere`/`IThemeAware`）当成硬墙；Don't：为绕过它去改库可见性。

## 2.5 坑与排错

| 症状 | 根因 | 处置与判据 |
|---|---|---|
| 写 `xmlns="clr-namespace:StartUI4Controls;assembly=StartUI4.WPF"` 报 MC3074 | 包名 `StartUI4.WPF` ≠ 程序集名 `StartUI4Controls`（`AssemblyName`，且无 `XmlnsDefinition`） | 固定用 `assembly=StartUI4Controls`；判据：Demo 全部 XAML 第 4 行的前缀串完全一致 |
| `UI4ComboBox` 上写 `HoverBorderColor`  IntelliSense 不认、构建 CS0117/解析异常 | 该 DP 不存在（陷阱 #3） | 改用 `FocusGradientStart/End`；先在 `UI4ComboBox.cs` 上半部 grep 确认 |
| 按 `PORTING.md` 第 4 节去找 `upstream/` 目录 | 该目录不在本副本（陷阱 #9） | 忽略；上游对照需求出现时问人 |
| 想用 `Themes/generic.xaml` 覆盖库内模板 | 库无 XAML 文件，模板在 C# 里构建 | 走「宿主侧 Style + DP 赋值」路线；要改库模板即触发第 1 章 §1.3.4-C 第 1 条 |
| 找不到 `UI4Tab` 的源文件以为缺失 | 它叫 `UI4TabControl.cs`（上游遗留文件名） | 按 §2.3.2 分组表定位；`UI4Tab` 类在 `:136`，`TabCloseRoutedEventArgs` 在 `:694` |

## 2.6 完成判据

1. 我能在 1 分钟内说出我要用的每个控件的源文件绝对路径，并 `Read` 到其 DP 声明行。
2. 我能复述真相源 9 级序列，并说出「为什么 specs 不能当 API 依据」。
3. 我把准备写进代码的属性名逐个对照了 §2.3.5 的 9 条陷阱，无一命中；每条都给了源码命中行号。
4. 我知道本次任务要抄的 Demo 页号，并已用 `--tab=N` 或 grep `TabItem Header` 找到对应 XAML 片段与处理器。
5. 我没有把 `bin/`、`obj/`、`shots/`、`.vs/` 里的内容当事实来源。
6. 若发现新的分叉，我已记录「文档说什么 / 源码是什么 / 我采用哪个」，未修改任何既有文档。

---

# 第 03 章 开发环境与首次构建

> 本章解决：把「拿到仓库」变成「跑起来的 Demo + 一份可信的构建基线」，并给出首次构建的正确判据。
> 读者：AI Agent（要在本项目之外新建/续写 WPF 业务代码）
> 前置章节：第 1 章、第 2 章

## 3.1 目标与验收

1. 产出一次**已知良好**的构建：0 错误，警告数与基线一致且全部落在库内既有文件。
2. 产出可运行的 Demo，并用窗口内文本（不是弹窗）确认运行时是 .NET Framework 4.8。
3. 确认 `demo-errors.log` 不存在，作为「无运行期异常」的基线。
4. 能在失败时区分三类噪声：MC3074 级联、文件占用 MSB3027、依赖缺失（AvalonEdit / NuGet 还原）。
5. 能复述三个「4.8」的来源，不再从 `.sln` 里找目标框架。

## 3.2 源码依据

- `src/StartUI4Controls/StartUI4Controls.csproj:4-15`（`net48`/`UseWPF`/`LangVersion`/`GenerateDocumentationFile`/`GeneratePackageOnBuild`）、`:36-37`（两个 `PackageReference`）
- `samples/StartUI4Demo/StartUI4Demo.csproj:4-13`（`WinExe`、`ApplicationManifest=app.manifest`）
- `samples/StartUI4Demo/App.xaml.cs:17`（资源桥播种）、`:35-51`（`demo-errors.log`）
- `samples/StartUI4Demo/MainWindow.xaml.cs:42`（`RuntimeInformation.FrameworkDescription` 写入 `RuntimeText`）
- `samples/StartUI4Demo/app.manifest:16-20`（`dpiAwareness PerMonitorV2, PerMonitor`）
- `samples/StartUI4Demo/bin/Debug/net48/StartUI4Demo.exe.config`（自动生成，含 `supportedRuntime sku=".NETFramework,Version=v4.8"`）
- `src/StartUI4Controls/UI4ProgressRing.cs:14,15,219`、`UI4ScrollViewer.cs:25,26`（CS0414 五处字段）
- `src/StartUI4Controls/UI4Panel.cs:21`、`UI4ComboBox.cs:26`、`UI4MultiLanguage.cs:29`（CS1574 四处 `cref`）
- `PORTING.md:143-152`（三个 4.8 的来源）、`:178-195`（MC3074 级联的真实案例）、`:99-117`（构建与运行时验证脚本清单）

## 3.3 正文

### 3.3.1 所需工具（按检查顺序）

| 工具 | 要求 | 自检命令 | 缺失的后果 |
|---|---|---|---|
| Visual Studio 2022（16.8+ 的 VS2019 亦可） | 勾选「.NET 桌面开发」+「.NET Framework 4.8 SDK/目标包」 | 见下行 | 构建报 `MSB3644 …v4.8 reference assemblies not found` |
| .NET Framework 4.8 Developer Pack | 引用程序集位于 `C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.8` | `ls "/c/Program Files (x86)/Reference Assemblies/Microsoft/Framework/.NETFramework/v4.8"` | 同上；这是**编译期** 4.8 的唯一来源 |
| .NET Framework 4.8 运行时 | 装机即有（Win10 1903+/Win11 自带） | 见 §3.3.4 | 运行期报需要更高版本框架，exe.config 的 `supportedRuntime` 给出明确报错 |
| .NET SDK（`dotnet` CLI）或 MSBuild | 任一可运行 MSBuild 的版本即可 | `dotnet --version`（本机实测输出 `10.0.401`） | 无法用 `dotnet build`；退回 VS 内「生成解决方案」 |
| Windows PowerShell 5.1 | 必须是 5.1（`powershell.exe`），不是 `pwsh` | `$PSVersionTable.PSVersion` | 验证脚本按 5.1 + `-STA` 编写，7.x 下部分 `Add-Type`/STA 行为不同 |
| NuGet 包缓存 | `avalonedit 6.3.1.120`、`system.data.sqlite 2.0.3`（`~/.nuget/packages`） | `ls ~/.nuget/packages/avalonedit` | 离线机器还原失败 `NU1101/NU1301`，构建停在库工程 |

约束：本章命令都在**Git Bash / PowerShell** 下可直接执行；`taskkill` 的双斜杠参数是 Git Bash 的路径转义规避写法，
在 PowerShell 里请写 `taskkill /IM StartUI4Demo.exe /F`。

### 3.3.2 三条构建/运行路径与产物

**路径 1（推荐给 Agent）：命令行全量构建**

```powershell
taskkill /IM StartUI4Demo.exe /F 2>$null
dotnet build StartUI4Controls.sln
```

产物（已实测存在）：

```text
src/StartUI4Controls/bin/Debug/StartUI4.WPF.1.0.20.nupkg     ← GeneratePackageOnBuild=true，顺带产出
src/StartUI4Controls/bin/Debug/net48/StartUI4Controls.dll
src/StartUI4Controls/bin/Debug/net48/StartUI4Controls.xml    ← GenerateDocumentationFile=true
src/StartUI4Controls/bin/Debug/net48/ICSharpCode.AvalonEdit.dll
src/StartUI4Controls/bin/Debug/net48/System.Data.SQLite.dll
samples/StartUI4Demo/bin/Debug/net48/StartUI4Demo.exe
samples/StartUI4Demo/bin/Debug/net48/StartUI4Demo.exe.config ← 自动生成，见 §3.3.5
```

Release 走 `dotnet build StartUI4Controls.sln -c Release`，同样在 `bin/Release/` 出一份 nupkg。
`obj/Debug/net48/*.FileListAbsolute.txt` 是「本次到底拷了哪些文件进 bin」的权威清单，产物存疑时读它。

**路径 2：跑起来**

```powershell
dotnet run --project samples/StartUI4Demo
dotnet run --project samples/StartUI4Demo -- --tab=10      # 直达「局部主题」页，便于逐页验证
```

`dotnet run` 隐含一次构建，因此也要求先 `taskkill`。它启动的就是 `bin/Debug/net48/StartUI4Demo.exe`。

**路径 3：VS 内 F5**

sln 把 `StartUI4Demo` 列在第一个项目条目（`StartUI4Controls.sln:10`），默认即启动项目。若 F5 提示
「无法直接启动带有 `OutputType=Library` 的项目」，说明激活的是库工程 → 见 §3.4 第 3 条。

### 3.3.3 构建前必做：释放文件锁

```powershell
taskkill /IM StartUI4Demo.exe /F
```

上一次 `dotnet run` / 脚本 / F5 残留的进程会锁住 `StartUI4Controls.dll`，构建以 MSB3027/MSB3021
（「文件被另一进程使用」）失败。判据：错误文本里出现被占用文件的路径与持有进程名 `StartUI4Demo (pid)`。
所有启动型脚本收尾都有 `Stop-Process`，Agent 手写脚本时照此办理。

### 3.3.4 首次构建的正确判据

```powershell
dotnet build StartUI4Controls.sln 2>&1 | Select-String -Pattern "error|warning" | Tee-Object build-baseline.txt
```

- **错误必须为 0**。任何非 0 都要先修，不要进入运行期调试（XAML 侧的 MC3074 常常只是 C# 错误的影子）。
- **警告基线 = 9 条**，全部落在 `src/StartUI4Controls/` 既有文件：
  - `CS0414` × 5：只写不读的私有字段 `UI4ProgressRing._isLoaded`(`:14`)、`_isStartupAnimationRunning`(`:15`)、
    `_isAnimating`(`:219`)、`UI4ScrollViewer._isAnimatingVertical`(`:25`)、`_isAnimatingHorizontal`(`:26`)。上游遗留，刻意保留。
  - `CS1574` × 4：XML 注释里的 `cref` 指向不存在成员——`UI4Panel.cs:21`（`Title`）、`UI4ComboBox.cs:26`（`HoverBorderColor` 与 `FocusBorderColor` 各一条）、
    `UI4MultiLanguage.cs:29`（`SetLanguage(string)`）。因为 `GenerateDocumentationFile=true` 才会报；它们只说明注释过期（第 2 章 §2.3.5）。
- 判据的正确用法：**「警告总数与所在文件」对上基线**，而不是「凑够 9」。你新增/修改的文件里出现任何警告都要处理；
  既有文件里的这 9 条属噪声，不要去「顺手清理」（清理要改库源码，触发第 1 章停止条件）。
- 顺带确认打包步骤没失败：`src/StartUI4Controls/bin/Debug/StartUI4.WPF.1.0.20.nupkg` 的时间戳应更新到本次构建。

### 3.3.5 运行期自检（三件事）

**① 确认运行时真的是 4.8**：用窗口内文本，不要用弹窗。

```csharp
// 正确（Demo 的做法，MainWindow.xaml.cs:42）：把描述写进已存在的 TextBlock
RuntimeText.Text = "实际运行时：" + System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;

// 不要这样：MessageBox.Show(...) 会阻塞 Dispatcher，
// 使 UIA 走查脚本在「窗口未就绪」上超时，并把每个页面的验证串成一次人工点击
```

期望输出以 `.NET Framework 4.8.` 开头（本机历史实测 `.NET Framework 4.8.9345.0`）。
若显示 `.NET Core …` / `.NET 6…`，说明产物被别的运行时宿主拉起，属环境异常，先查 `exe.config`。
`RuntimeInformation` 需要 .NET Framework **4.7.1+**，本仓库是 4.8，可直接使用。
注意：`README.md` 第六章步骤 6 与 `PORTING.md` 第 6 节都写了「也可临时用 `MessageBox.Show` 看一眼」——
**本手册不采纳**（第 01 章 §1.3.6 第 5 条）：弹窗会阻塞 `Dispatcher`、让 UIA 走查在「窗口未就绪」上超时，
并且它自身就是一条会被记进 `demo-errors.log` 判定链路的人工干预。新应用一律用窗口内文本。

**② 确认 `demo-errors.log` 不存在**：

```powershell
$log = 'samples/StartUI4Demo/bin/Debug/net48/demo-errors.log'
if (Test-Path $log) { Get-Content $log -Tail 40 } else { 'no runtime error' }
```

语义：唯一写入点是 `App.xaml.cs:35-51`，即 `DispatcherUnhandledException` 与
`AppDomain.CurrentDomain.UnhandledException` 捕获到的异常，追加 UTF-8 文本，**同时弹一次 MessageBox**。
所以「跑起来后弹出异常窗」与「该文件存在」是同一事件的两个可见面。验证脚本一律先删后测（`theme.ps1:10`）。
注意：UI 线程钩子里 `e.Handled = true`（`:26`）会让程序带病继续跑，**不要因为「没崩」就判定无异常**。

**③ 确认编译期/运行期两个 4.8 都在**：

```powershell
Get-Content samples/StartUI4Demo/bin/Debug/net48/StartUI4Demo.exe.config
```

内容应含 `<supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.8" />`。
三个「4.8」的来源（`PORTING.md:143-152`）：编译期 = csproj `TargetFramework`；产物元数据 = `TargetFrameworkAttribute`；
运行期 = 自动生成的 `exe.config` + 该元数据。`.sln` 里没有任何 4.8 字样，且 net48 **不产出** `.deps.json` / `.runtimeconfig.json`。

### 3.3.6 常见失败与定位

| 失败表现 | 根因 | 定位与处置 |
|---|---|---|
| `MC3074: 类型 UI4TextBlock 在…不存在`（Demo XAML） | 库 C# 编译失败 → dll 未产出 → XAML 标记编译拿不到类型；xmlns 本身无误 | 以 `dotnet build` 全量输出找**真实 C# 错误**（IDE 列表会掩盖，`PORTING.md:190`）；修完 MC3074 自动消失 |
| `MSB3027 / MSB3021 文件被占用` | Demo 进程存活 | `taskkill` 后重建；若反复出现，检查你自己写的脚本有没有 `Stop-Process` |
| `FileNotFoundException: ICSharpCode.AvalonEdit` | `UI4CodeEditor` 继承 AvalonEdit 的 `TextEditor`，XAML 在构造期即解析该类型层级 | 部署时带上 `ICSharpCode.AvalonEdit.dll`；开发期报此错说明 NuGet 还原没落地（`~/.nuget/packages/avalonedit/6.3.1.120`） |
| `NU1101/NU1301 找不到包`（离线机器） | 无法从在线源取 AvalonEdit / System.Data.SQLite | 让维护者把两个 nupkg 放进本地 feed 再构建；**不要**擅自删除 `PackageReference` 来「让它编译过」（SQLite 可讨论，AvalonEdit 会让 `UI4CodeEditor` 崩） |
| `MSB3644 找不到 .NETFramework,Version=v4.8 引用程序集` | 缺 Developer Pack | 装 4.8 目标包；这是「停下来问人」场景（涉及安装机器级组件） |
| F5 启动的是类库 / 断点不生效 | VS 的启动项目存在**用户级** `.vs/StartUI4Controls/…`（`.suo`），不随 sln 分发 | 右键 `StartUI4Demo` → 设为启动项目（一次即可）；或改用 `dotnet run --project samples/StartUI4Demo` 绕开 VS 状态 |
| 切平台为 x64 后产物仍是 AnyCPU | sln 把 x64/x86 全映射到 `Debug\|Any CPU` | 需要真 64 位时在 csproj 侧设 `PlatformTarget`；属工程结构变更，先问人 |
| 抓屏/截图全白 | 本机对 WPF 的 `PrintWindow`/`CopyFromScreen` 均返回全白（基线亦然），是环境限制 | 颜色正确性改用「进程内断言 + UIA + DWM 回读」；截图仅作辅助（第 24 章） |

## 3.4 推荐做法（Do / Don't）

- Do：每条构建命令前都无条件 `taskkill`；Don't：把 MSB3027 当「偶发」重试三次。
- Do：把首次干净构建的 `warning` 清单存档为基线（`build-baseline.txt`）；Don't：只记「9 条」这个数字——数字随注释修复而变。
- Do：用 `--tab=N` 做逐页冒烟；Don't：每次只测第一页就宣称「Demo 正常」。
- Do：运行环境检查写进窗口文本；Don't：用弹窗验证任何东西（它同时污染 UIA 与 `demo-errors.log` 判定）。
- Do：先修 C# 错误再看 XAML 错误；Don't：为了消 MC3074 去改 xmlns（那是正确写法）。
- Do：产物存疑时读 `obj/**/ *.FileListAbsolute.txt`；Don't：用「目录里看起来该有的文件」推断构建成功。

## 3.5 坑与排错

1. **症状**：构建 0 错误但 Demo 一启动就弹异常框。**根因**：运行期异常与编译无关，多为 XAML 解析期事件处理器访问未初始化成员。
   **处置**：看 `demo-errors.log` 首行栈；对照 `MainWindow.xaml.cs:98-103, 218-227` 的空判写法——你抄样板时把空判抄丢了。
2. **症状**：`dotnet run` 报 `net48` 不支持 `dotnet run`。**根因**：使用了只支持 .NET Core 的启动方式或 SDK 过旧。
   **处置**：直接执行产物 `samples/StartUI4Demo/bin/Debug/net48/StartUI4Demo.exe --tab=0`。
3. **症状**：删掉 `.vs/` 后启动项目回到库工程。**根因**：`.vs` 存的是用户级选择，删除即回落 sln 顺序。
   **处置**：无需重建 `.vs`；用 F5 前设一次启动项目，或直接走 `dotnet run`。
4. **症状**：切深色后标题栏仍亮。**根因**：不是构建问题，是该窗口内没有任何 UI4 控件（缺通路②）。
   **处置**：转第 17 章；判据是 `titlebar.ps1` 的对应断言。
5. **症状**：`p3verify.ps1` 里断言的色值与 `UI4ThemeDefinition` 不符。**根因**：上一次跑过 `UI4Theme.SetAccent`，
   它写回已注册的 definition，污染后续 `Apply("light")`。**处置**：每个进程内脚本用新 `powershell -STA` 进程，或显式恢复强调色。

## 3.6 完成判据

1. `dotnet build StartUI4Controls.sln` 输出「0 错误」，警告逐条对上了我存档的基线清单。
2. `src/StartUI4Controls/bin/Debug/StartUI4.WPF.1.0.20.nupkg` 时间戳 = 本次构建时间。
3. Demo 启动后窗口右上显示 `实际运行时： .NET Framework 4.8.x`，且全程未弹异常框。
4. `samples/StartUI4Demo/bin/Debug/net48/demo-errors.log` 不存在。
5. 我能说出三个 4.8 的来源，并能在 `StartUI4Demo.exe.config` 里指出运行期那条。
6. 我对 11 个页号各跑过一次 `--tab=N`（或 `shot.ps1 -Tab N`）冒烟，未观察到未处理异常。
7. 我记录了本机 PowerShell 版本与 `dotnet --version`，供第 24 章 CI 段落复用。

---

# 第 04 章 net48 与 C# 7.3 硬约束

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

---

# 第 05 章 组件库能力总览与选型决策

> 本章解决：面对一个业务需求，「库里有没有现成类型、该用哪一个、切主题时它会不会跟着变」。
> 读者：AI Agent（要在本项目之外新建/续写 WPF 业务代码）
> 前置章节：第 02 章、第 03 章、第 04 章

## 5.1 目标与验收

1. 给定一个界面需求，我能在本章总表或 §5.3.6 快速表里找到候选类型，并说出它的**主题通道**（命令式 / 引用式 / 继承 / 不跟随）。
2. 面对 §5.3.2 的六组重叠能力，我能按判据选出唯一实现，并说明放弃其他候选的原因。
3. 我能列出宿主**不可使用**的类型清单，并解释「不能自写主题感知控件」这一后果如何决定所有扩展都走 `DynamicResource`。
4. 我能说出「深色下仍是浅色观感」的控件清单，并给出三类对策（接线令牌 / 换控件 / `UI4ThemeScope` 局部反衬）。
5. 我写进代码的每个类型名都能在 `src/StartUI4Controls/` grep 命中；表中标注 internal 的一律不使用。

## 5.2 源码依据

- 类型清单来源：对 `src/StartUI4Controls/*.cs` 的声明行 grep（`public class|enum|static class|interface`），逐条命中；internal 命中位置：`UI4Theme.cs:728`（`IThemeAware`）、`UI4DataGrid.cs:16`、`UI43DSphere.cs:11`、`Internal/*.cs` 全部。
- 通道判定来源：`SetResourceReference` 命中 `UI4CheckBox.cs:168-172`、`UI4Radio.cs:120-122`、`UI4TextBox.cs:171-178`、`UI4PasswordBox.cs:241-248`、`UI4Switch.cs:146-148`、`UI4ProgressBar.cs:160-162`、`UI4Slider.cs:118-121`、`UI4Pivot.cs:176-178`；`UI4Theme.TrackControl` 命中 `UI4Button.cs:122`、`UI4ComboBox.cs:196`、`UI4ColorPicker.cs:66`、`UI4FlipTextBlock.cs:183`、`UI4ListBox.cs:242`、`UI4Menu.cs:103`、`UI4MessageBox.cs:69`、`UI4NavigationView.cs:661`、`UI4Panel.cs:181`（`UI4ContextMenu` 走 `ThemeChanged` 订阅）。
- 零主题引用（不跟随）来源：`UI4ListView.cs`、`UI4GridView.cs`、`UI4TabControl.cs`、`UI4Grid.cs`、`UI4ScrollViewer.cs`、`UI4CircleSlider.cs`、`UI4ProgressRing.cs`、`UI4NotifyIcon.cs`、`UI4CodeEditor.cs` 内 `UI4Theme.` 引用数为 0。
- 特殊通道：`UI4TextBlock.cs:254-258`（构造只设 `Style`）、`:354-357`（仅 `Foreground != null` 才写内部 `TextBox` 前景）；`UI4ScrollViewer.cs:29-40`（`IsSmoothScrollEnabled` 默认 `true`）、`:62-70`（`e.Handled = true`）、`:108,158`（`CompositionTarget.Rendering` 缓动）。
- 选型判据的样板：`samples/StartUI4Demo/MainWindow.xaml:30`（原生 `TabControl` 做外壳）、`:283-343`（导航容器页）、`:223-282`（列表与网格页）、`:444-552`（局部主题页）。
- 令牌与色值：`UI4ThemeToken.cs:7-39`、`UI4Theme.cs:297-309`、`UI4ThemeDefinition.cs:94,130,170`；作用域：`UI4ThemeScope.cs:24-38`（附加属性 `Theme`，`string`，默认 `null`）。

## 5.3 正文

### 5.3.1 公开类型总表（按用途分组）

主题通道列的含义：**命令式** = 切主题时被回调重刷（作用域下会重建 `Style`）；**引用式** = 构造时对 DP 挂资源引用，随资源继承自动跟随；**继承** = 自身不接主题，未显式赋值时沿用宿主 `TextElement.Foreground` 等继承值；**不跟随** = 默认值硬编码浅色，主题切换不动它。

**内容与基础交互**

| 类型 | 基类 | 通道 | 选用要点 / 已知坑 |
|---|---|---|---|
| `UI4Button` | `Button` | 命令式 | 三主题下都是蓝→紫渐变 + 白字（未令牌化）；要中性按钮请显式给 `Background`（一旦赋本地值即停跟随） |
| `UI4CheckBox` | `CheckBox` | 引用式 | `BoxCornerRadius` 是 `[Obsolete]` 别名 DP（`UI4CheckBox.cs:46,52`），与 `CornerRadius` 同一 DP；新代码用 `CornerRadius` |
| `UI4Radio` | `RadioButton` | 引用式 | `TextColor` 默认 `Colors.Black`，与令牌 `TextForeground`（`#1E1E1E`）不同值 |
| `UI4Switch` | `Control` | 引用式 | `IsOn` 默认双向绑定；`Toggled` 是路由事件；**无 AutomationPeer/Toggle pattern**，UIA 只能按 Name/坐标 |
| `UI4TextBox` | `TextBox` | 引用式 | 内置 `UI4ContextMenu`；外观 DP 全套（`HoverBorderColor`/`FocusBorderColor` 在此存在） |
| `UI4PasswordBox` | **`TextBox`** | 引用式 | `Password` 是明文 `string`（可绑定）；`Unloaded` 会清密码；高安全场景改用原生 `PasswordBox` |
| `UI4ComboBox` | `ComboBox` | 命令式 | 焦点边框是**渐变**（`FocusGradientStart/End`）；尺寸经验式 `Height ≥ FontSize×1.2 + 22` |
| `UI4Slider` | `Slider` | 引用式 | `TrackBackground` 默认白色与令牌不一致（`UI4Slider.cs:118-121` 已挂引用，显式赋值才脱钩） |
| `UI4CircleSlider` | `ContentControl` | 不跟随 | 环形取值；`RingForeground/RingBackground` 为 `Brush`，可宿主接线 |
| `UI4ProgressBar` | `Control` | 引用式 | 渐变起止已挂令牌（`ProgressStart`/`Accent`） |
| `UI4ProgressRing` | `ContentControl` | 不跟随 | `AnimatedValue` 有公开 setter（`UI4ProgressRing.cs:182-186`），但会被内部动画覆盖，别把它当业务状态源；含 3 个 CS0414 遗留字段 |
| `UI4TextBlock` | `ContentControl` | 继承 | **不是** `System.Windows.Controls.TextBlock`，混用报 CS1503；`GradientStart/End` 是死属性，渐变走 `Foreground`；4 个 `new` DP 有基类引用歧义 |
| `UI4FlipTextBlock` | `ContentControl` | 命令式 | `FontSize` 默认 60；有 `ReadLocalValue` 保护 |

**容器与列表**

| 类型 | 基类 | 通道 | 选用要点 |
|---|---|---|---|
| `UI4Panel` | `ContentControl` | 命令式 | `BorderColor` 是 `Color`；用 `new` 把基类 `BorderBrush/BorderThickness` 换成 Color 语义；注释里的 `Title` 不存在 |
| `UI4Grid` | `Grid` | 不跟随（主题中性） | 22 行文件，构造写死 `#E1ECF5→#FFFFFF` 渐变；任何主题下都浅色 |
| `UI4ScrollViewer` | `ScrollViewer` | 不跟随 | 滚轮 `e.Handled=true` + `CompositionTarget.Rendering` 约 100ms 缓动；`IsSmoothScrollEnabled="False"` 即退回原生行为（默认 `true`） |
| `UI4ListBox` | `ListBox` | 命令式 | `ListStyleType` = `None/Disc/Number`；公开 `RefreshTheme()`（`:264`）；公开静态字段 `_scrollViewerStyle`(`:212`) 属泄漏细节，宿主勿动 |
| `UI4ListView` | `ListBox` | 不跟随 | 单行平铺、禁横向滚动 |
| `UI4GridView` | `ListBox` | 不跟随 | `SizeChanged` 自动重算列数（`UpdateColumns` `:235`；`ItemWidth` 为 `NaN` 直接返回） |
| `UI4CodeEditor` | AvalonEdit `TextEditor` | 不跟随（0 主题引用） | 构造期设 C# 高亮/行号/折行/Consolas 14/Tab→4 空格并预挂 `UI4ContextMenu`；`Loaded` 只套静态滚动条样式（`UI4CodeEditor.cs:24-28,64-70`）。XAML 一旦触及该类型，`ICSharpCode.AvalonEdit.dll` 即运行必需 |

**导航与页签**

| 类型 | 基类 | 通道 | 选用要点 |
|---|---|---|---|
| `UI4NavigationView` | `ItemsControl` | 命令式 | 集合 `RegularItems` / `BottomItems`（`UI4NavigationView.cs:681-682`，`ObservableCollection` 只读 getter）；`SelectedItem` 类型是 `UI4NavigationViewItem` 且默认双向 |
| `UI4NavigationViewItem` / `UI4NavigationViewBottomItem` | `ContentControl` | 随父 | `Header`(string)/`TextIcon`/`TextIconFontFamily`/`ItemFontSize`；Bottom 变体钉在底部 |
| `UI4Tab` | `Selector` | 不跟随 | `AddTab` / `CloseTab`（委托 `TabCloseRoutedEventHandler`，参数 `TabCloseRoutedEventArgs.TabItem`，不置 `e.Handled` 时控件自行移除）；**`IsBrand` 不在本类型上** |
| `UI4TabItem` | `HeaderedContentControl` | 不跟随（随父外观） | `TextIcon`/`ImageSource`/`IconSize`/`IsClosable`/`IsBrand`(`UI4TabControl.cs:82`)、事件 `CloseTab` |
| `UI4Pivot` | `Selector` | 引用式 | 三项挂了 `TextForeground`/`Accent` 资源引用（`UI4Pivot.cs:176-178`） |
| `UI4PivotItem` | `HeaderedContentControl` | 引用式（随父） | `IsBrand` 钉在最前（`UI4Pivot.cs:15-22`） |

**弹出、菜单与系统**

| 类型 | 基类 | 通道 | 选用要点 |
|---|---|---|---|
| `UI4MessageBox` | `Window` | 命令式 | `Show(content, title=null, buttons=OK, width=460, owner=null)` → `bool?`（true=OK / false=Cancel / null=关闭）；**务必传 `owner`** |
| `UI4ColorPicker` | `Window` | 命令式 | 静态 `ShowDialog(...) → Color?`；实例 `Show(Window owner=null)` 名字骗人，内部是 `base.ShowDialog()`（`:790-794`） |
| `UI4Menu` | `Menu` | 命令式 | `BarBackground`/`ItemHoverBrush`/`PopupCornerRadius`/`TextForeground`/`KeyTipForeground`；子项 `UI4MenuElementItem`（文字图标 `TextIcon` + `IconFontFamily`，`KeyTip` 形如 `(F)`）、分隔符 `UI4MenuSeparatorElement` |
| `UI4ContextMenu` | —（普通对象，非 Control） | 命令式（订阅 `ThemeChanged` 且不退订） | 代码式：`AddItem(UI4MenuItem)` / `AddItem(UI4MenuItemType, Action, Func<bool>)`、`Attach/Detach/Open/Close`；外观属性有「显式赋值后主题切换不再覆盖」语义 |
| `UI4MenuItem` / `UI4MenuItemType` / `UI4MenuIcons` | POCO / 枚举 / 静态类 | 不适用 | `Type/Text/Icon/IconText/Command/CanExecute`；枚举含 `Undo Redo Cut Copy Paste Delete SelectAll` |
| `UI4NotifyIcon` | `FrameworkElement` + `IDisposable` | 不跟随 | 必须在视觉树里；`Visibility` 即开关；退出必须 `Collapsed` 再 `Dispose()`；`IconSource` 解析失败回退 `SystemIcons.Application` |

**主题与本地化服务（非控件）**

| 类型 | 形态 | 用途一句话 |
|---|---|---|
| `UI4Theme` | 静态服务 + 实例主题 | 模式切换、30 令牌、资源桥播种、`SetAccent`、注册/应用自定义键、持久化、`ThemeChanged` |
| `UI4ThemeDefinition` | sealed 类 | 主题的**配方**：`Key`/`GetColor`/`Has`/`With`（流式）/`Clone()`（键变 `<key>.clone`）+ 静态 `Light/Dark/HighContrast` |
| `UI4ThemeToken` | 枚举（30 值） | 令牌名 = 资源键后缀 |
| `UI4ThemeScope` | 附加属性（string） | 子树换肤；`null`/空白/未注册键都 = 撤销 |
| `UI4WindowTitleBar` | 静态类 + 附加属性 | DWM 染非客户区；默认全自动，`Enabled="False"` 豁免 |
| `IThemePersistence` / `RegistryThemePersistence` / `JsonThemePersistence` | 接口 + 两实现 | `Save(UI4ThemeMode)` / `UI4ThemeMode? Load()`；HKCU 与手写一行 JSON |
| `UI4MultiLanguage` + `UI4LanguageKey` | 静态服务 + 枚举 | 构造期拉取；**无 `SetLanguage`** |
| 转换器与辅助 | 公开类 | `BoolToVisibilityConverter`、`PlaceholderVisibilityConverter`、`InnerPaddingConverter`、`IndexPlusOneConverter`、`ObjectIsStringConverter`、`NullToVisibilityConverter`、`InverseNullToVisibilityConverter`、`GeometryHelper.GetOutlinedGeometry`、`TabCloseRoutedEventArgs`、`UI4TrayMenuItem`、`PopupActivationMode`、`ListStyleType`、`UI4MessageBoxButtons`、`UI4ThemeMode`、`UI4LanguageKey` |

### 5.3.2 重叠能力选型判据

**① 导航与页切换**

| 候选 | 何时用 | 何时**不**要用 |
|---|---|---|
| 原生 `TabControl`（`TabStripPlacement="Left"`） | **应用主骨架**：页签只是索引，内容要虚拟化、要最稳定的选中语义、要跟随主题的宿主画刷 | 需要关闭/新增按钮、图标页签、浏览器式体验 |
| `UI4NavigationView` | 侧边栏式一级导航（顶部标题 + 底部固定项），且选中内容显示在右侧 | 页签横排、需要关闭按钮；命令式重建 `Style` 的开销对高频切换敏感时 |
| `UI4Pivot` | 少量平级视图、要平移过渡与「品牌项钉最前」 | 层级导航、需要底部固定项 |
| `UI4Tab` | 浏览器式多文档（可关闭、可新增、图标页签） | 作应用主骨架——**它不跟随主题**，深色下页签区仍是浅色 |

判据口诀：**外壳用原生 `TabControl`（Demo 就是这么做的，`MainWindow.xaml:30`），文档区用 `UI4Tab`，一级导航用 `UI4NavigationView`，同层视图切换用 `UI4Pivot`**。同一个窗口内不要让两个「选中即换内容」的容器互相驱动。

**② 卡片列表**

| 候选 | 判据 |
|---|---|
| `UI4ListView` | 一行平铺的卡片带（横向不滚动）；卡片样式属性最全 |
| `UI4GridView` | 需要「按可用宽度自动列数」的网格；必须给**有限** `ItemWidth`（`NaN` 时 `UpdateColumns` 直接返回，列数不更新） |
| `UI4ListBox` | 需要普通/圆点/编号三种列表样式，且**要求跟随主题**（它是命令式，切主题会重刷） |
| 原生 `DataGrid` | 需要表格（列排序、单元格编辑、虚拟化大表）。`UI4DataGrid` 是 internal，宿主拿不到 |

深色一致性判据：`UI4ListView` / `UI4GridView` **不跟随**主题（卡片与文字恒浅色）⇒ 深色业务列表页优先 `UI4ListBox` + 自定义 `ItemTemplate`，或按 §5.3.4 接线 / 反衬。

**③ 文本**

- 要阴影 / 圆角面板 / 渐变文字 / 与库内其他控件一致的「控件化」外观 → `UI4TextBlock`（记得它是 `ContentControl`）。
- 纯文本、需要 `Run`/`Hyperlink` 行内混排、需要最稳定的换行与测量、要最省心的主题继承 → 原生 `TextBlock`。
- 二者不可互相赋值传参（`CS1503`）；`UI4TextBlock.Text` 与 `Content` 二选一（内部 `MultiBinding` + `TextOrContentConverter`，`Text` 非空优先）。Demo 的 `SectionTitle`/`SectionHint` 两个样式打在原生 `TextBlock` 上（`App.xaml:7-18`），这就是「标题正文用原生、装饰块用 UI4」的既有分工。

**④ 密码输入**

要 `Password` 直接双向绑定 / 明文切换按钮 / 自定义掩码 → `UI4PasswordBox`（`: TextBox`，外观 DP 与 `UI4TextBox` 同名一套，主题通道同为引用式）。安全要求高（不让明文进可绑定的 string、要防内存转储）→ 原生 `PasswordBox`（有 `PasswordChanged`、`SecurePassword`；库内注释即明示这一退路）。
注意：`UI4PasswordBox` 在 `Unloaded` 时会清密码（`:238,297`），做页签切换保持输入的场景要预判这条行为。

**⑤ 菜单**

- 窗口顶部的菜单栏 → `UI4Menu`（命令式，随主题重刷；子项用 `UI4MenuElementItem`，分隔符 `UI4MenuSeparatorElement`）。
- 某个元素上的右键菜单 → `UI4ContextMenu`（**代码式**，`Attach(UIElement)`；`UI4MenuItem` 是 POCO 不是控件；
  它在构造时订阅 `ThemeChanged` 且不退订 ⇒ 循环里创建长生命周期实例要留意）。
- 托盘上的菜单 → `UI4NotifyIcon.AddItem(...)`（内部也是代码式弹出，观感独立于主题）。
- 不要用 `UI4ContextMenu` 去做菜单栏，也不要把 `UI4Menu` 挂成某控件的 `ContextMenu`（弹出层样式来源不同，主题刷新路径也不同）。

**⑥ 容器与滚动**

| 需求 | 选择 | 理由 |
|---|---|---|
| 一张带阴影 + 悬浮缩放的卡片 | `UI4Panel` | 命令式，作用域下正确重染；阴影与缩放已分层，文字不糊 |
| 需要默认渐变底的 Grid，且不介意永远浅色 | `UI4Grid` | 构造即写死浅色渐变（主题中性），深色页慎用 |
| 常规布局 + 主题一致的底 | 原生 `Grid`/`Border` + `Background="{DynamicResource UI4.Brush.Surface}"` | 走资源桥，零重建开销，嵌套作用域天然生效 |
| 需要平滑滚动的滚动区 | `UI4ScrollViewer` | 代价见下表 |

`UI4ScrollViewer` vs 原生 `ScrollViewer`（代码级差异，别只按「好不好看」选）：

| 维度 | 原生 `ScrollViewer` | `UI4ScrollViewer` |
|---|---|---|
| 滚轮事件 | 冒泡，内层到底后交给外层 | `OnMouseWheel` 里 `e.Handled = true`（`UI4ScrollViewer.cs:70`）⇒ **嵌套滚动会抢事件**，内层即使已到底也不外层 |
| 滚动动画 | 瞬移（net48 无内置惯性） | `CompositionTarget.Rendering` 逐帧逼近目标偏移（`:108,158`），约 100ms |
| 关闭动画 | 不适用 | `IsSmoothScrollEnabled="False"` 后行为与原生一致（`:29-40`，默认 `true`） |
| 主题 | 滚动条样式随宿主资源继承 | 0 主题引用，滚动条恒浅色观感（自绘样式才能改） |
| 选择判据 | 布局默认；多滚动区嵌套；长列表性能敏感 | 只在「单个主滚动区 + 要平滑手感」时用，或显式关掉平滑 |

### 5.3.3 宿主不可使用清单与后果

| internal 类型 | 位置 | 影响 |
|---|---|---|
| `IThemeAware` | `UI4Theme.cs:728` | **宿主无法为自己的控件挂命令式刷新通道**（`TrackControl` 也是 internal，`:363`）⇒ 自定义控件的主题跟随只有 `DynamicResource` / `SetResourceReference` 一条路 |
| `UI4Theme.EffectiveThemeFor` / `ThemeVersion` / `UseTheme` / `WriteTokens` / `RefreshControl` | `UI4Theme.cs:666,64,644,297,432` | 宿主无法参与作用域换入栈的调度；不要试图在宿主里「手工同步有效主题」 |
| `UI4DataGrid` | `UI4DataGrid.cs:16` | 无内置 SQLite 表格；表格用原生 `DataGrid`。连带：`System.Data.SQLite.dll` 对宿主并非必需（唯一消费者是它） |
| `UI43DSphere` | `UI43DSphere.cs:11` | 无 3D 展示能力 |
| `Internal/ThemeSync`、`Internal/ScrollBarResources`、`Internal/WindowResizeBehavior`、`Internal/WindowAnimationHelper`、`Internal/ColorToBrushConverter` | `src/StartUI4Controls/Internal/` | 拿不到「尊重用户显式改动的 DP 同步助手」，也拿不到库内滚动条样式 ⇒ 自写滚动条样式与对话框缩放/动画需自行实现 |

后果一句话：**扩展点 = 资源桥键（`UI4.Color.*` / `UI4.Brush.*`）+ 公开外观 DP + `UI4ThemeScope` + `UI4Theme.Register/Apply`**。
凡是想「像库内控件那样被批量重刷」的念头，都要改写成「让宿主元素消费令牌资源」。

### 5.3.4 主题跟随性矩阵（深色下仍保持 Light 观感的部分）

| 控件 | 深色下的实际观感 | 根因 | 对策（三选一） |
|---|---|---|---|
| `UI4Button` | 仍是蓝→紫渐变 + 白字 | 渐变未令牌化（三主题取值相同） | ① 接受（对比度成立）；② 赋 `GradientStart/End` 或 `Background`（此后不再跟随，属预期） |
| `UI4ListView` / `UI4GridView` | 卡片底 `White`、文字深色 ⇒ **深色页上像一块白纸** | 0 主题引用，默认值硬编码 | ① `ItemBackground="{DynamicResource UI4.Brush.Surface}"`、`ItemBorderBrush="{DynamicResource UI4.Brush.BorderNormal}"` 接线；② 改用 `UI4ListBox` + 模板；③ 见下方「局部反衬」 |
| `UI4Tab`（及页签内容底色） | 页签栏/选中标签仍浅色（`TabSelectedBackground=White` 等） | 0 主题引用 | ① 给 `HeaderBackground`/`TabBackground`/`TabSelectedBackground` 挂 `DynamicResource`；② 外壳改用原生 `TabControl`；③ 局部反衬 |
| `UI4Grid` | 永远浅蓝→白渐变 | 构造里写死 | 只当「装饰性浅色区」用；需要主题的布局层用原生 `Grid` |
| `UI4ScrollViewer` | 滚动条与内容底不随主题 | 0 主题引用 | 外层容器给 `{DynamicResource UI4.Brush.Background}`，滚动条样式需自绘 |
| `UI4CircleSlider` / `UI4ProgressRing` / `UI4NotifyIcon` 菜单 | 环底色、托盘菜单底色保持默认浅色 | 无主题通道 | `RingBackground`/`MenuBackground` 等 `Brush` DP 用 `DynamicResource` 接线 |
| `UI4Radio.TextColor` / `UI4Slider.TrackBackground` 等默认值 | 与令牌不同值（`Black` / `White`） | DP 默认值即字面色 | 需要严格一致时显式绑令牌（引用式控件本身已挂引用的属性不必再动） |

**局部反衬（第 14 章展开机制）**：接不了线、又不值得换控件时，把这块区域**声明成它自己那套浅色的作用域**，让周围文字/边框也转成浅色版取值，深色页上就变成「有意为之的浅色岛」而不是 bug：

```xml
<!-- 整块内容显式钉在 light 配方上；UI4ThemeScope.Theme 是 string 附加属性，大小写不敏感 -->
<Border ui:UI4ThemeScope.Theme="Light" Background="{DynamicResource UI4.Brush.Background}">
  <ui:UI4GridView ItemWidth="180" />
</Border>
```

适用判据：区域边界清楚（一张卡片、一个侧栏）⇒ 用反衬；区域铺满整页 ⇒ 换控件或重写宿主容器，别在根上钉作用域（会让标题栏 DWM 染色与页面取值不一致）。

判定「不跟随」的可复现方法（不改库）：切主题前后各读一次同一 DP 的解析值。批处理在 `DispatcherPriority.Input`，**必须泵到 `ContextIdle` 之后**才看得到效果：

```csharp
UI4Theme.SetTheme(UI4ThemeMode.Light);
Brush before = card.ItemBackground;            // UI4ListView 例
UI4Theme.SetTheme(UI4ThemeMode.Dark);
app.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.ContextIdle, new System.Action(() => { }));
Brush after = card.ItemBackground;             // changed = False ⇒ 该控件不跟随
```

### 5.3.5 自己重算通道（表可能过期，方法不会）

```powershell
Select-String -Path src/StartUI4Controls/*.cs -Pattern "TrackControl|SetResourceReference|IThemeAware|UI4Theme\." |
  Group-Object Filename | Select-Object Name, Count
```

读法：命中 `IThemeAware`+`TrackControl` = 命令式；只命中 `SetResourceReference` = 引用式；`UI4Theme.` 计数为 0 = 不跟随；三者皆无但仍显色（`UI4TextBlock`）= 继承。

### 5.3.6 新功能 → 用哪个控件 + 注意什么

| 我要做的东西 | 用什么 | 必须注意 |
|---|---|---|
| 应用外壳：左侧一级导航 + 右侧内容区 | 原生 `TabControl`（`TabStripPlacement="Left"`）+ `UI4NavigationView` | 外壳底色自己接 `UI4.Brush.*`；`NavigationView` 项要 `Header`(string)，不是任意内容 |
| 页签式多文档（可关闭） | `UI4Tab` + `UI4TabItem` | 不跟随主题，深色页需接线或反衬；`IsBrand` 在 `UI4TabItem` 上，不在 `UI4Tab` 上 |
| 一段说明文字 / 标题 | 原生 `TextBlock` + Demo 的 `SectionTitle`/`SectionHint` 样式 | 需要阴影或渐变装饰块才用 `UI4TextBlock`（它是 `ContentControl`，与原生不可互传） |
| 表单：标签 + 输入 | `UI4TextBox` / `UI4ComboBox` / `UI4CheckBox` / `UI4Radio` / `UI4Switch` | 全是引用式或命令式，别再赋死色；`UI4Switch` 要给 `x:Name` 才好用 UIA 定位 |
| 密码框 | 一般页 `UI4PasswordBox`；登录/支付页原生 `PasswordBox` | `UI4PasswordBox.Password` 是可绑定明文 string，且 `Unloaded` 会清空 |
| 数据列表（可能上千行） | 原生 `DataGrid` | `UI4DataGrid` 是 internal，宿主拿不到；也别为此引 `System.Data.SQLite` |
| 卡片流 / 磁贴墙 | `UI4ListView`（单行）或 `UI4GridView`（自动列数） | `UI4GridView.ItemWidth` 必须是有限值，否则列数不重算；两者都不跟随主题 |
| 普通条目列表（要随主题） | `UI4ListBox` + `ItemTemplate` | `ListStyleType` = `None/Disc/Number`；别碰公开的 `_scrollViewerStyle` 字段 |
| 右键菜单 / 顶部菜单栏 | `UI4ContextMenu` + `UI4MenuItem`（POCO，代码式 `Attach`）/ `UI4Menu` + `UI4MenuElementItem` | 前者订阅 `ThemeChanged` 不退订 ⇒ 单例复用；后者不要当 `ContextMenu` 挂到控件上 |
| 提示/确认对话框、取色 | `UI4MessageBox.Show(...)`、`UI4ColorPicker.ShowDialog(...)` → `Color?` | **一定传 `owner`**；`MessageBox` 返回 `bool?`（null = 用户关掉）；`ColorPicker` 的实例 `Show()` 名字骗人，内部仍是模态 |
| 进度反馈 | `UI4ProgressBar`（确定）/ `UI4ProgressRing`（不确定） | Ring 不跟随主题，`AnimatedValue` 会被内部动画覆盖 |
| 托盘图标 + 菜单 | `UI4NotifyIcon` | 必须挂在视觉树里；`Visibility` 即开关；退出前 `Collapsed` → `Dispose()` |
| 代码编辑区 | `UI4CodeEditor` | 交付必须带 `ICSharpCode.AvalonEdit.dll`；它完全不跟随主题（第 06 章 §6.3.7） |
| 局部换肤 / 品牌区 | `ui:UI4ThemeScope.Theme="..."` | 值是**主题键字符串**（`light`/`dark`/`highcontrast`/自定义键），`null`/空白/未注册键 = 撤销 |
| 窗口标题栏配色 | 默认自动（`UI4WindowTitleBar`） | 只在需要豁免时写 `Enabled="False"`；别自绘标题栏 |

## 5.4 推荐做法（Do / Don't）

- Do：先定「主题通道」再定控件——深色页不用不跟随类，除非准备为它接线；Don't：等等深色截图出来才发现卡片是白的。
- Do：应用外壳用原生 `TabControl` / `Grid` + `{DynamicResource}`；Don't：为了「全 UI4」把骨架换成 `UI4Tab`。
- Do：不跟随类的外观 DP 用令牌接线；Don't：给命令式控件的 DP 赋死色（既停跟随，又在作用域下被重刷覆盖回来）。
- Do：同一需求只保留一个「选中即换内容」的容器；Don't：`UI4NavigationView` 里再塞 `UI4Pivot` 并双向同步选中项。
- Do：滚动区默认用原生 `ScrollViewer`，只在单个主滚动区要平滑手感时才用 `UI4ScrollViewer`；Don't：在嵌套滚动里用 `UI4ScrollViewer` 又不开 `IsSmoothScrollEnabled="False"`（外层收不到滚轮）。
- Do：把 §5.3.1 的表当索引用，每次写代码前 grep 复核；Don't：把本章表当 API 手册抄（默认值与 DP 名会随库演进）。
- Do：需要表格直接用原生 `DataGrid`；Don't：试图把 `UI4DataGrid` 改公开（改库 = 停下来问人）。

## 5.5 坑与排错

| 症状 | 根因 | 处置与判据 |
|---|---|---|
| 深色下某块区域整片白 | 用了不跟随类（`UI4ListView`/`UI4GridView`/`UI4Grid`/`UI4Tab`） | 按 §5.3.4 接线或换控件；判据：§5.3.4 的进程内比较脚本输出 `changed = False` |
| 渐变文字没效果 | `UI4TextBlock.GradientStart/End` 是死属性 | 给 `Foreground` 传 `LinearGradientBrush`（`README.md` 五节示例即此写法） |
| `UI4Switch` 在 UIA 里只能坐标点 | 库无 `OnCreateAutomationPeer`，无 Toggle pattern | 给控件设 `x:Name`（→ `AutomationId`）；或按 Name 找文本兄弟节点 |
| 打开右键菜单后切主题，菜单颜色不刷 / 越用越慢 | `UI4ContextMenu` 构造订阅 `ThemeChanged` 且不退订 | 单例复用并 `Detach()`；不要在循环里造长生命周期实例 |
| 切深色后下拉框文字看不清 | 曾经真实存在的缺陷（选中框底与焦点渐变未令牌化） | 已修，判据跑 `p3verify.ps1` H 组；若又复现，说明有代码路径在被主题管理的 DP 上写了本地值 |
| 对话框出现在屏幕中央而不是主窗中央 | 没传 `owner` | `UI4MessageBox.Show(..., owner: this)`；判据：`UI4MessageBox.cs:276-283` 只有 owner 为空时才 `CenterScreen` |

## 5.6 完成判据

1. 我列出了本次界面将使用的全部类型，逐个标注主题通道，且每个通道都能用 §5.3.5 的 grep 复核。
2. 我对六组重叠能力各给出一次选择结论（哪个 / 为什么不用别的），并在 §5.3.6 里能指到对应行。
3. 我的深色页里没有出现「不跟随 + 未接线 / 未反衬」的控件。
4. 我没有引用任何 internal 类型（`UI4DataGrid`、`UI43DSphere`、`IThemeAware`、`Internal/*`、`UI4Theme` 的 internal 成员）。
5. 若用了 `UI4GridView`，`ItemWidth` 是有限值；若用了 `UI4ScrollViewer`，已决定是否关 `IsSmoothScrollEnabled`；若用了 `UI4NotifyIcon`，退出路径写了 `Collapsed` → `Dispose()`。

---

# 第 06 章 新建应用的接入路径与最小可运行工程

> 本章解决：拿到 `StartUI4Controls` 之后，让一个**全新**的 net48 WPF 业务工程第一次就编过、跑起来、主题生效。
> 读者：AI Agent。前置章节：第 03 章（环境与构建）、第 04 章（硬约束）、第 05 章（选型）。

## 6.1 目标与验收

1. 我能按 §6.3.1 判据在两条接入路径（ProjectReference / NuGet 包）里选一条，并说出哪条组合是不可接受的。
2. 我不看仓库也能写出 §6.3.4 的六个文件，且 `dotnet build` 0 错误、首屏主题生效。
3. 我能逐条跑完 §6.3.5 的首屏验收清单，并说出每条失败时的根因位置。
4. 我知道复制 Demo 当脚手架时**必须改**的 6 处和**不该改**的 1 件事（代码后置风格），以及两个隐式依赖（`ICSharpCode.AvalonEdit.dll`、`System.Drawing`）在交付包里的义务。

## 6.2 源码依据

- 宿主工程样板：`samples/StartUI4Demo/StartUI4Demo.csproj:1-20`（六行开关 + `ProjectReference`）、`App.xaml:1-20`、`App.xaml.cs:12-51`、`app.manifest:1-22`（`assemblyIdentity` 在第 3 行，DPI 声明在 `:18-19`）
- 窗口样板：`samples/StartUI4Demo/MainWindow.xaml:1-28`（`xmlns:ui` 在第 4 行，`Background="{DynamicResource UI4.Brush.Background}"` 在第 8 行）、`MainWindow.xaml.cs:42`（`RuntimeInformation.FrameworkDescription` 写进窗口内文本）
- 打包事实：`src/StartUI4Controls/StartUI4Controls.csproj:15-24`（`GeneratePackageOnBuild` / `PackageId StartUI4.WPF` / `Version 1.0.20`）、`:32`（`System.Drawing`）、`:36-37`（`AvalonEdit 6.3.1.120` / `System.Data.SQLite 2.0.3`）；包内容与依赖组见 `src/StartUI4Controls/obj/Debug/StartUI4.WPF.1.0.20.nuspec`（`lib/net48/StartUI4Controls.dll|.xml`）
- 产物位置：`src/StartUI4Controls/bin/Debug/StartUI4.WPF.1.0.20.nupkg`、`src/StartUI4Controls/bin/Debug/net48/`（`StartUI4Controls.dll`、`.xml`、`ICSharpCode.AvalonEdit.dll`、`System.Data.SQLite.dll`）
- 主题入口：`UI4Theme.cs:283`（`ApplyToApplication()`，幂等）、`:106`（`SetTheme(UI4ThemeMode)`）、`:16`（`UI4ThemeMode { Light, Dark, System, HighContrast }`）

## 6.3 正文

### 6.3.1 先选接入路径

| 情况 | 选 | 理由 |
|---|---|---|
| 业务工程与本仓库**同一个解决方案**，可能要顺手改库 | **A：`ProjectReference`** | 改库立刻生效；能用到未发版的行为（示例、修复） |
| 业务工程独立仓库 / 多个应用共用同一份库 / 交付走包管理 | **B：NuGet 包 `StartUI4.WPF`** | 版本可锁、可回滚，构建不依赖本仓库存在 |
| 需要库里 internal 能力（`UI4DataGrid`、`IThemeAware`） | 都不选 ⇒ **停下来问人** | 见第 05 章 §5.3.3，宿主拿不到，且改可见性 = 改库 |

不可接受的组合：**B 路径 + `<HintPath>` 指向某台机器的 `bin\Debug`**。它看起来像引用包，实际是把编译产物当依赖，换台机器就 CS0246；要么走本地 feed 的包，要么走 `ProjectReference`，二选一。

### 6.3.2 路径 A：同解决方案 ProjectReference

1. 把宿主 `.csproj` 放进解决方案目录树里（任何位置都行，SDK 工程按相对路径找依赖），加一行 `<ProjectReference Include="../src/StartUI4Controls/StartUI4Controls.csproj" />`。
2. `dotnet build StartUI4Controls.sln` 会连带编库；把宿主工程 `Add-Project` 进 `.sln` 不是必须（`.sln` 只影响 VS 打开视图，第 02 章）。
3. 代价：库的 NuGet 依赖会**传染**到宿主输出目录——`ICSharpCode.AvalonEdit.dll` 与 `System.Data.SQLite.dll` 都会出现，即使你没用代码编辑器也没用 SQLite（见 §6.3.7）。

### 6.3.3 路径 B：本地 feed + NuGet 包 `StartUI4.WPF`

包由库工程在构建时自动产出（`GeneratePackageOnBuild=true`），无需单独打包命令；产物在 `src/StartUI4Controls/bin/Debug/StartUI4.WPF.1.0.20.nupkg`。版本号写在 `StartUI4Controls.csproj:20-22`，**包可能落后于源码**：feed 里的 `1.0.20` 只反映它被产出那一刻的源码，源码后续改动不会自动进包（要重跑构建才覆盖）。需求依赖未发版行为时用路径 A。

宿主工程目录下放一份 `nuget.config`（全文可粘）：

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />   <!-- 清掉继承来的源，避免解析到公网上的同名包 -->
    <add key="startui4-local" value="../../src/StartUI4Controls/bin/Debug" />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
  </packageSources>
</configuration>
```

`nuget.org` 必须保留——`AvalonEdit` 与 `System.Data.SQLite` 是包的依赖，要从公网源解析。安装：

```powershell
dotnet add package StartUI4.WPF --version 1.0.20 --source ../../src/StartUI4Controls/bin/Debug
```

判据：`dotnet list package` 列出 `StartUI4.WPF [1.0.20]`，且 `obj/project.assets.json` 里出现 `lib/net48/StartUI4Controls.dll` 的编译与运行资产。显式 `--source` 是好习惯：本地 feed 排第一时同名包优先命中它，但显式指定才能防止在别的机器上静默装到别的东西。

### 6.3.4 最小文件集（六个文件，可整段粘贴）

以下用 `Fabrix.Shell` 作宿主命名空间/程序集名占位，替换成你的产品名。`Fabrix.Shell.csproj`：

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net48</TargetFramework>
    <UseWPF>true</UseWPF>
    <LangVersion>7.3</LangVersion>
    <Nullable>disable</Nullable>
    <ImplicitUsings>disable</ImplicitUsings>
    <RootNamespace>Fabrix.Shell</RootNamespace>
    <AssemblyName>Fabrix.Shell</AssemblyName>
    <GenerateAssemblyInfo>true</GenerateAssemblyInfo>
    <ApplicationManifest>app.manifest</ApplicationManifest>
  </PropertyGroup>

  <ItemGroup>
    <!-- 路径 A 用这一行；走路径 B 则删掉本行，改用 nuget.config + PackageReference StartUI4.WPF -->
    <ProjectReference Include="../src/StartUI4Controls/StartUI4Controls.csproj" />
    <!-- 宿主代码若直接用 System.Drawing（图标互转），再补： <Reference Include="System.Drawing" /> -->
  </ItemGroup>

</Project>
```

`app.manifest`（DPI 与 OS 兼容声明，抄 Demo 后**只改第 3 行的 name**）：

```xml
<?xml version="1.0" encoding="utf-8"?>
<assembly manifestVersion="1.0" xmlns="urn:schemas-microsoft-com:asm.v1">
  <assemblyIdentity version="1.0.0.0" name="Fabrix.Shell.app"/>
  <compatibility xmlns="urn:schemas-microsoft-com:compatibility.v1">
    <application>
      <supportedOS Id="{8e0f7a12-bfb3-4fe8-b9a5-48fd50a15a9a}" />
      <supportedOS Id="{1f676c76-80e1-4239-95bb-83d0f6d0da78}" />
      <supportedOS Id="{35138b9a-5d96-4fbd-8e2d-a2440225f93a}" />
    </application>
  </compatibility>
  <application xmlns="urn:schemas-microsoft-com:asm.v3">
    <windowsSettings>
      <dpiAware xmlns="http://schemas.microsoft.com/SMI/2005/WindowsSettings">true/pm</dpiAware>
      <dpiAwareness xmlns="http://schemas.microsoft.com/SMI/2016/WindowsSettings">PerMonitorV2, PerMonitor</dpiAwareness>
    </windowsSettings>
  </application>
</assembly>
```

`App.xaml`——**注意：库不需要合并任何 ResourceDictionary**（零 XAML 库），资源桥由代码播种：

```xml
<Application x:Class="Fabrix.Shell.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             StartupUri="MainWindow.xaml"
             ShutdownMode="OnMainWindowClose">
    <!-- Application.Resources 留空：库是零 XAML，令牌由 App.xaml.cs 的 ApplyToApplication() 播种 -->
</Application>
```

`App.xaml.cs`——启动序列的第一件事就是播种资源桥；异常落盘，**不弹窗**（弹窗会阻塞 Dispatcher、打断 UIA 走查；理由见第 01 章 §1.3.6 第 5 行、第 03 章 §3.3.5）：

```csharp
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using StartUI4Controls;

namespace Fabrix.Shell
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            UI4Theme.ApplyToApplication();            // 幂等；必须早于任何 {DynamicResource UI4.*} 被解析
            UI4Theme.SetTheme(UI4ThemeMode.System);   // 需要品牌锁定时改 Light/Dark，或按持久化结果传值
            DispatcherUnhandledException += (s, args) => { Report("UI", args.Exception); args.Handled = true; };
            AppDomain.CurrentDomain.UnhandledException += (s, args) => Report("Domain", args.ExceptionObject as Exception);
        }

        private static void Report(string source, Exception ex)
        {
            string line = string.Format("[{0:yyyy-MM-dd HH:mm:ss}] {1}{2}{3}", DateTime.Now, source, Environment.NewLine, ex == null ? "(null)" : ex.ToString());
            try { File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app-errors.log"), line, Encoding.UTF8); }
            catch (IOException) { }                   // 日志写不下去就算了，别在异常处理器里再抛
        }
    }
}
```

两级钩子分工不同：`DispatcherUnhandledException` 可以 `Handled = true` 让进程继续跑，`AppDomain.UnhandledException` **只能记录**（第 04 章 §4.3.5）。Demo 里的具名方法处理器与上面的 lambda 等价，选一种即可。

`MainWindow.xaml`：

```xml
<Window x:Class="Fabrix.Shell.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:ui="clr-namespace:StartUI4Controls;assembly=StartUI4Controls"
        Title="Fabrix" Width="960" Height="600"
        WindowStartupLocation="CenterScreen"
        Background="{DynamicResource UI4.Brush.Background}">
    <StackPanel Margin="24">
        <ui:UI4Button Content="确定" HorizontalAlignment="Left" Padding="20,8" Margin="0,0,0,12"/>
        <TextBlock x:Name="RuntimeText" FontSize="12"
                   Foreground="{DynamicResource UI4.Brush.Icon}"/>
    </StackPanel>
</Window>
```

`MainWindow.xaml.cs`：

```csharp
using System.Windows;
using System.Runtime.InteropServices;

namespace Fabrix.Shell
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RuntimeText.Text = "运行时：" + RuntimeInformation.FrameworkDescription;
        }
    }
}
```

### 6.3.5 「首屏跑起来」验收清单

| # | 判据 | 不满足时的根因 |
|---|---|---|
| 1 | `dotnet build` 0 错误，输出目录有 `Fabrix.Shell.exe` + `StartUI4Controls.dll` | 只有 `StartUI4Controls.dll` 缺失才是引用问题；exe 缺失 ⇒ `OutputType` 不是 `WinExe` |
| 2 | 双击 exe 出现窗口，**没有**控制台、没有 `app-errors.log` | 有 `app-errors.log` ⇒ 读它的第一条堆栈（第 03 章 §3.3.6） |
| 3 | 窗口内文本以 `.NET Framework 4.8` 开头 | 显示成别的版本 ⇒ 你测的不是这个 exe；判据只看窗口内文本，不看弹窗 |
| 4 | `Background="{DynamicResource UI4.Brush.Background}"` 解析出**非透明**底色 | `ApplyToApplication()` 没调、调在 `InitializeComponent` 之后、或写成了 `StaticResource` |
| 5 | 代码里 `UI4Theme.SetTheme(UI4ThemeMode.Dark)` 后窗口底色立刻变深 | 用了 `StaticResource`；或该区域由不跟随类控件构成（第 05 章 §5.3.4） |
| 6 | 深色主题下窗口标题栏同步变深，且 `System` 模式时跟随系统深浅色 | 宿主自绘/改了 `WindowStyle`，或显式关了 `UI4WindowTitleBar`；`System` 模式没走 `UI4ThemeMode.System`（第 16、17 章） |

### 6.3.6 拿 Demo 当脚手架（复制法）

适合「要快速做一个能演示全部控件的壳」。复制 `samples/StartUI4Demo/` 整个目录后，**必须改这 6 处**：

| 处 | 原值 | 改成 |
|---|---|---|
| 目录与 `.csproj` 文件名 | `StartUI4Demo.csproj` | `Fabrix.Shell.csproj` |
| `RootNamespace` / `AssemblyName`（`:10-11`） | `StartUI4Demo` | 宿主名 |
| `App.xaml:1` 的 `x:Class` | `StartUI4Demo.App` | `<宿主命名空间>.App` |
| `MainWindow.xaml:1` 的 `x:Class` | `StartUI4Demo.MainWindow` | `<宿主命名空间>.MainWindow` |
| `MainWindow.xaml:5` 的 `Title` | 演示标题 | 产品窗口标题 |
| `app.manifest:3` 的 `assemblyIdentity name` | `StartUI4Demo.app` | `<宿主名>.app` |

还要顺手**删掉**：`RuntimeText` 之外的演示页签、`--tab=N` 解析（`:49-59`，你自己的自动化想留就留）、`App.xaml.cs:50` 的 `MessageBox.Show`（改成只落盘，理由同 §6.3.4）。
**不要改**的一件事：别把 Demo 的代码后置风格（`x:Name` + 事件处理 + 构造里 `SetResourceReference`）整体重写成 MVVM 再动手。Demo 是本仓库**唯一**逐控件的真实用法样本，有些能力只能代码式用（`UI4ContextMenu.AddItem`、`UI4NotifyIcon.AddItem`、`UI4Theme.Register`/`SetAccent`），重构会把这些用法连带丢掉，而它们在文档里没有第二处来源。

### 6.3.7 依赖交付义务

| 依赖 | 从哪来 | 宿主义务 |
|---|---|---|
| `ICSharpCode.AvalonEdit.dll` | 库的 `PackageReference AvalonEdit 6.3.1.120`（csproj`:36`），随 ProjectReference/包传染到输出目录 | **交付必须带上它**。`UI4CodeEditor` 继承自 AvalonEdit 的 `TextEditor`：XAML 里只要出现过该类型，加载即需要这个程序集，运行期抛 `FileNotFoundException`/`TypeInitializationException`。没用也别从输出目录删 |
| `System.Drawing` | 库的 `<Reference Include="System.Drawing" />`（csproj`:32`），在 nuspec 里是 `frameworkAssembly` | 框架自带、不进交付包；但宿主代码若直接用 `SystemIcons`/`Icon` 转换，要在自己 csproj 补 `<Reference Include="System.Drawing" />`（net48 不默认引用） |
| `System.Data.SQLite.dll` | 库的 `PackageReference`（csproj`:37`），唯一消费者是 internal 的 `UI4DataGrid` | 路径 A/B 都会把它拷进宿主输出目录。**不要为了减包去改库的依赖**（改库 = 问人）；确认不需要时，问人后再决定是否在交付清单里剔除该文件 |
| `StartUI4Controls.xml` | 与 DLL 同目录（`GenerateDocumentationFile=true`） | 保留：IDE 智能提示的文档来源；删了不影响运行 |

## 6.4 推荐做法（Do / Don't）

- Do：新工程第一行代码就是 `UI4Theme.ApplyToApplication()`；Don't：先画界面再补主题播种（首屏会闪一帧未着色底）。
- Do：宿主 `xmlns:ui` 写全 `assembly=StartUI4Controls`；Don't：以为 CLR 命名空间等于程序集名（包 id 是 `StartUI4.WPF`，程序集是 `StartUI4Controls`）。
- Do：颜色一律 `{DynamicResource UI4.Brush.<令牌>}`；Don't：`StaticResource`（换肤后不重解析）。
- Do：宿主 `<LangVersion>7.3</LangVersion>` 与库保持一致；Don't：`<LangVersion>latest</LangVersion>`（第 04 章 §4.3.7）。
- Do：宿主异常落盘、路径 B 锁死版本号 `1.0.20`；Don't：照抄 Demo 的 `MessageBox.Show`（演示程序的取舍），Don't：写浮动版本（feed 一旦被覆盖，行为漂移查不动）。

## 6.5 坑与排错

| 症状 | 根因 | 处置与判据 |
|---|---|---|
| XAML 报 `MC3074`「找不到命名空间 StartUI4Controls」 | `xmlns:ui` 拼错 / 工程尚未编出库 DLL | 核对 `clr-namespace:StartUI4Controls;assembly=StartUI4Controls`；判据：输出目录里 `StartUI4Controls.dll` 存在 |
| C# 报 CS0246 找不到 `UI4Theme` | 缺 `using StartUI4Controls;`（`ImplicitUsings=disable`） | 补 using，而不是加全局 using（第 04 章 §4.3.1） |
| 只有 MC3074 一堆、看不到真正的 C# 错误 | XAML 编译失败级联掩盖 | 先修 MC3074，再读 C# 错误（第 03 章构建排错） |
| 装包时 `Unable to find package 'StartUI4.WPF'` | `nuget.config` 未生效 / 源路径不对 / 包还没产出 | 确认 `bin\Debug\*.nupkg` 存在（不存在就先构建库）；`dotnet nuget locals http-cache --clear` 后重试 |
| 换肤后标题栏不跟随，或交付后目标机报缺 `ICSharpCode.AvalonEdit.dll` | 宿主自绘/改了 `WindowStyle` 打断了 DWM 路径；打包脚本按「我用到的」挑 DLL | 用默认 `WindowStyle`，判据跑第 17 章 `titlebar.ps1`；按 §6.3.7 的输出目录清单整体打包，别手挑 |
| 新建工程时 VS 报「Duplicate 'ApplicationDefinition' items」 | 手工加了 `App.xaml` 的编译项，`UseWPF` 已隐式包含 | 删掉手写的 `<ApplicationDefinition Include="App.xaml" />` |

## 6.6 完成判据

1. 我的宿主工程只用六个文件起步（csproj / app.manifest / App.xaml(.cs) / MainWindow.xaml(.cs)），`dotnet build` 0 错误 0 新警告。
2. §6.3.5 的六条判据我逐条验过，并把结果写成「判据 → 观察到的现象」的对照，不写「应该没问题」。
3. 我的 `App.xaml.cs` 里 `UI4Theme.ApplyToApplication()` 在 `base.OnStartup` 之后、任何窗口创建之前。
4. 我的宿主 csproj 里 `LangVersion=7.3` / `Nullable=disable` / `ImplicitUsings=disable` 三项齐备，`<ApplicationManifest>app.manifest</ApplicationManifest>` 也在位。
5. 我说清了自己的接入路径（A 或 B）及选择理由（若选 B，`nuget.config` 与锁定版本号 `1.0.20` 都在版本库里），且交付清单包含 `StartUI4Controls.dll` 与 `ICSharpCode.AvalonEdit.dll`、没有改动库工程的任何依赖声明。

---

# 第 07 章 工程配置与依赖治理

> 本章解决：把「能还原、能编译、能启动、输出目录干净」的宿主 WPF 工程一次性配好，并弄清库带来的三个外部依赖各自真实影响什么。
> 读者：AI Agent（要在本项目之外新建/续写 WPF 业务代码）
> 前置章节：第 4 章（net48 与 C# 7.3 硬约束）、第 5 章（取库方式选型）

## 7.1 目标与验收

按本章做完，你的宿主工程必须同时满足：

1. `TargetFramework=net48` 且 `UseWPF=true`，编译期与运行期都锁定 4.8（判据：输出目录存在 `<app>.exe.config` 且内含 `sku=".NETFramework,Version=v4.8"`）。
2. `<Reference Include="System.Drawing" />` 已在宿主 csproj 里显式写出（只要你用到 `UI4NotifyIcon`）。
3. `app.manifest` 存在并已通过 `ApplicationManifest` 挂上，DPI 声明为 `PerMonitorV2`。
4. 输出目录里**只有**运行必需文件：`<app>.exe`、`StartUI4Controls.dll`、`ICSharpCode.AvalonEdit.dll`（用了 `UI4CodeEditor` 才需要）、`<app>.exe.config`。
5. 不存在 `*.deps.json` / `*.runtimeconfig.json` —— 出现它们说明工程被当成了 .NET Core 风格。

未完成第 2、3 条的典型后果分别是：`UI4NotifyIcon` 编译期找不到类型、高 DPI 屏上文字发虚且 `Environment.OSVersion` 虚报。

## 7.2 源码依据

- `samples/StartUI4Demo/StartUI4Demo.csproj:1-20` —— 宿主工程完整样板（13 行 `ApplicationManifest`、17 行唯一 `ProjectReference`）。
- `src/StartUI4Controls/StartUI4Controls.csproj:4-38` —— 库工程配置：`Version=1.0.20`（22 行）、`GeneratePackageOnBuild=true`（15 行）、`Reference Include="System.Drawing"`（32 行）、`PackageReference` 两项（36-37 行）。
- `src/StartUI4Controls/UI4NotifyIcon.cs:3-4` —— `using DrawingIcon = System.Drawing.Icon;` / `using SystemIcons = System.Drawing.SystemIcons;`；`:429-441` 图标解析与 `SystemIcons.Application` 回退。
- `src/StartUI4Controls/UI4CodeEditor.cs:22` —— `public class UI4CodeEditor : TextEditor`（AvalonEdit 基类）。
- `src/StartUI4Controls/UI4DataGrid.cs:16` —— `internal class UI4DataGrid : DataGrid, IThemeAware`，SQLite 的唯一消费者。
- `samples/StartUI4Demo/app.manifest:5-21` —— `supportedOS` 三条 GUID + `dpiAware`/`dpiAwareness`。
- `src/StartUI4Controls/UI4WindowTitleBar.cs:22-23` —— 明示「未经 manifest 声明的进程 `Environment.OSVersion` 可能虚报为 6.3」，故库改用能力探测。
- `samples/StartUI4Demo/bin/Debug/net48/` —— 实测产物清单；`README.md 第九章附录 B/D` 给出逐文件必需性表。

## 7.3 正文

### 7.3.1 csproj 每一项该填什么

直接照 `samples/StartUI4Demo/StartUI4Demo.csproj` 抄，只改 `RootNamespace`/`AssemblyName`/`ApplicationManifest`：

| 项 | 宿主取值 | 作用与不填的后果 |
|---|---|---|
| `OutputType` | `WinExe` | 不填则产出 dll，VS 按 F5 报「无法启动」（见 7.5.4） |
| `TargetFramework` | `net48` | 编译期引用面与 `supportedRuntime` 的唯一来源；`.sln` 里没有任何 4.8 字样 |
| `UseWPF` | `true` | 决定自动包含 `PresentationFramework` 等引用与 `.xaml` 编译目标；漏掉则 XAML 全报 MC |
| `LangVersion` | `7.3` | 与库一致。写更高版本会引入库无法编译的语法，跨工程重构时踩雷 |
| `Nullable` | `disable` | 库是 `disable`；宿主开可空注解会让两侧签名对接产生噪声 |
| `ImplicitUsings` | `disable` | 库无隐式 using，宿主依赖它会造成「拷贝一段代码就少 using」 |
| `AssemblyName` | 你的 exe 名 | 同时决定 `<app>.exe.config` 与异常日志文件名（见第 8 章） |
| `RootNamespace` | 与 `x:Class` 前缀一致 | 不一致时 `App.xaml`/`MainWindow.xaml` 的 `x:Class` 找不到类型 |
| `ApplicationManifest` | `app.manifest` | 不填即无 PerMonitorV2，且 OS 版本虚报（见 7.3.3） |
| `GenerateAssemblyInfo` | `true`（Demo 12 行） | 与库一致；若你手写 `AssemblyInfo.cs` 需改为 `false`，否则特性重复定义 CS0579 |

库工程侧另有三项你不需要复制到宿主：`GenerateDocumentationFile`（宿主若需要 XML 智能提示可自行打开）、`GeneratePackageOnBuild`、`NoWarn CS1591`。

### 7.3.2 传递依赖的三个真相

`ProjectReference` 会把库的 `PackageReference` 传递进你的输出闭包，但三者必需性完全不同：

- **AvalonEdit（6.3.1.120）—— 条件必需**。`UI4CodeEditor` 直接继承 `ICSharpCode.AvalonEdit.TextEditor`（`UI4CodeEditor.cs:22`）。只要 XAML 里出现 `<ui:UI4CodeEditor>`，类型层级解析就要求该 dll 在场；实测移除后启动即 `FileNotFoundException`。不用这个控件就可以不部署。
- **System.Data.SQLite（2.0.3）—— 事实上的死重**。全库唯一引用者是 `internal` 的 `UI4DataGrid`（`UI4DataGrid.cs:16`，宿主不可使用）。程序集引用惰性解析，删掉 `System.Data.SQLite.dll` 各页功能正常；它的原生 `SQLite.Interop.dll` 本来就不会被拷进输出目录。发布前直接从产物里剔除。
- **System.Drawing —— 不传递，必须宿主自己引**。库在 csproj 里写了 `<Reference Include="System.Drawing"/>`，但框架引用不随 `ProjectReference` 流动。你要用 `UI4NotifyIcon`，就得在宿主 csproj 补同一行，否则 `System.Drawing.Icon` / `SystemIcons` 在你的代码里不可见（CS0234）。若只是 XAML 里放 `<ui:UI4NotifyIcon>` 而代码不碰这些类型，编译能过，但运行期仍需 4.8 自带的 `System.Drawing.dll` 在 GAC 中（正常安装都有）。

结论：`UI4NotifyIcon` 的 `IconSource` 解析走 `Application.GetResourceStream`/`GetContentStream`，失败回退 `SystemIcons.Application`（`UI4NotifyIcon.cs:444-470`）。仓库内没有任何 ico/图片资源，所以不指定 `IconSource` 也能出图标 —— 宿主用系统回退即可。

### 7.3.3 app.manifest 为什么是宿主责任

库是 dll，manifest 只对 exe 生效，所以 DPI 与 OS 版本真实性必须由宿主声明。照抄 `samples/StartUI4Demo/app.manifest`：

```xml
<application xmlns="urn:schemas-microsoft-com:asm.v3">
  <windowsSettings>
    <dpiAware xmlns="http://schemas.microsoft.com/SMI/2005/WindowsSettings">true/pm</dpiAware>
    <dpiAwareness xmlns="http://schemas.microsoft.com/SMI/2016/WindowsSettings">PerMonitorV2, PerMonitor</dpiAwareness>
  </windowsSettings>
</application>
```

`supportedOS` 三条（Win10/11、8.1、7 的 GUID，见 `app.manifest:8/:10/:12`）不只是兼容性徽章：Windows 依据它决定行为兼容层，`PerMonitorV2` 只在声明了 Win10 GUID 时才生效。

同一份 manifest 还决定 `Environment.OSVersion` 是否如实报告。不写 manifest 的进程在 Win10/11 上会虚报 `6.3`（Win8.1 的兼容值）。本库不受影响 —— `UI4WindowTitleBar.cs:22-23` 明确「不看版本号，一律先试 `DwmSetWindowAttribute` 按 HRESULT 定论并缓存」。但**你自己的**按版本分支代码会因此走错分支：判据是窗内文本打印 `Environment.OSVersion.Version`，加了 manifest 前后对比。

### 7.3.4 exe.config 与 supportedRuntime

SDK 风格工程的 `GenerateSupportedRuntime` 目标按 `TargetFramework` 注入，产出与 exe 同名的配置文件：

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <startup>
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.8" />
  </startup>
</configuration>
```

保留它：删除后在装有更高 4.x 的机器上仍能跑（就地升级），但在只有更低版本的机器上会给出含糊错误；有它则报「需要 .NET Framework 4.8」。你若需要别的运行期配置（`assemblyBinding`、`AppContext switches`），在项目里放 `App.config`，构建会把它与上面这段合并成 `<app>.exe.config`。

### 7.3.5 net48 的交付产物形态

net48 **没有** `.deps.json`，也**没有** `.runtimeconfig.json` —— 那是 .NET Core/5+ 的宿主探测机制；4.x 走 Fusion/GAC + `app.config`。装配解析依赖同目录同名 dll，所以部署包就是文件清单（见 `README.md 第九章附录 D`）。同理没有卫星程序集（库无本地化资源文件，`UI4MultiLanguage` 的文案在 C# 里）。

### 7.3.6 改成私有目录引用或包引用

三种取库方式按优先级：`ProjectReference`（源码级、可调试）> `PackageReference` 到本地源 > 直接 `<Reference>` 拷 dll。

改用包引用时注意：库的 `GeneratePackageOnBuild=true`（csproj:15）+ `Version=1.0.20`（csproj:22）会在 `src/StartUI4Controls/bin/Debug/` 顺带产出 `StartUI4.WPF.1.0.20.nupkg`。**版本号永远停在 1.0.20，源码却可能已经改过** —— 这个包不能当「当前源码」的信任源。若你引用它，先确认包内 dll 的时间戳不早于你依赖的修复；否则改回 `ProjectReference`，或每次构建后从本地目录强制重装（`--source ./src/StartUI4Controls/bin/Debug`）。

纯 dll 引用（拷 `StartUI4Controls.dll` 到 `lib/`）会丢掉 NuGet 传递：AvalonEdit 不再自动进来，用了 `UI4CodeEditor` 就得手工把 `ICSharpCode.AvalonEdit.dll` 一起部署，且目标机要能找到它。

## 7.4 推荐做法（Do / Don't）

- Do：宿主 csproj 从 `samples/StartUI4Demo/StartUI4Demo.csproj` 整段复制后改三处（`OutputType` 保持 `WinExe`、`AssemblyName`、`RootNamespace`）。
- Do：用到 `UI4NotifyIcon` 就立刻写 `<Reference Include="System.Drawing" />`，别等编译报错再补。
- Do：发布脚本按必需清单拷贝（exe + `StartUI4Controls.dll` + 可选 AvalonEdit + exe.config），显式排除 `System.Data.SQLite.dll` 与全部 `.pdb`。
- Do：把 manifest 当运行前提待：新窗口工程第一步就建 `app.manifest` 并在 csproj 挂 `ApplicationManifest`。
- Don't：不要在宿主 csproj 里加 `System.Data.SQLite` 的 `PackageReference`「以防万一」—— 它只增加 398 KB 死重，且唯一消费者宿主用不到。
- Don't：不要把整包 nupkg 当源码等价物引用（见 7.3.6）。
- Don't：不要为了「让 x64 生效」去改 sln 的平台映射 —— x64/x86 都映射到 Any CPU，改它没有任何效果。
- Don't：不要手写 `runtimeconfig.json` 或 `deps.json`，net48 不认。

## 7.5 坑与排错

| 症状 | 真因 | 处置 |
|---|---|---|
| F5 弹出「无法启动，XXX 不是可启动项目」或没有可启动项 | 启动项目被设成了类库工程（库无 `OutputType`，产出 dll） | 解决方案右键宿主 exe 工程 → 设为启动项目；判据：`bin` 下产出的是 `.exe` 而非只有 `.dll` |
| CS0234「命名空间 System.Drawing 中不存在类型 Icon」或 XAML 里 `UI4NotifyIcon` 运行期异常 | 宿主没写 `<Reference Include="System.Drawing"/>`，框架引用不传递 | 补该行后重新还原 |
| 输出目录有 `System.Data.SQLite.dll`，体积翻倍 | 库的 `PackageReference` 传递闭包 | 发布时剔除；不影响运行（惰性解析） |
| 4K/混合 DPI 屏上字变小或发虚；自研代码按 `Environment.OSVersion` 分支走了 Win8.1 路径 | 缺 manifest（`PerMonitorV2` 未生效，OS 版本虚报 6.3） | 加 `app.manifest` 并挂 `ApplicationManifest`，重启进程验证 |
| 启动即 `FileNotFoundException: ICSharpCode.AvalonEdit` | XAML 构造期解析 `UI4CodeEditor` 的类型层级，dll 不在同目录 | 部署 AvalonEdit，或移除该控件 |
| 只有 `App.xaml` 报 MC 系列、其他 XAML 全红 | 库侧 C# 编译错误造成级联（见 `事实速查.md` 第 7 节第 1 条） | 先全量构建库工程看真实错误列表，别信 IDE 的 XAML 报错 |
| 生成后 exe 被占用、MSB3027 重试失败 | 上一次启动的进程还在 | 构建前先 `taskkill //IM <你的exe名> //F` |
| csproj 里加了 `net48` 但仍报缺 `WindowsBase` | `UseWPF` 没开 | 补 `<UseWPF>true</UseWPF>` |

## 7.6 完成判据

逐条自查，全部为「是」才算完成：

1. `grep` 你的 csproj：`OutputType`、`TargetFramework`、`UseWPF`、`LangVersion`、`Nullable`、`ImplicitUsings`、`AssemblyName`、`RootNamespace`、`ApplicationManifest` 九项都在，取值与 7.3.1 表一致。
2. 构建后 `bin/Debug/net48/` 内存在 `<app>.exe`、`StartUI4Controls.dll`、`<app>.exe.config`；`<app>.exe.config` 里有 `sku=".NETFramework,Version=v4.8"`。
3. 同目录内**不存在** `*.deps.json`、`*.runtimeconfig.json`。
4. 若代码引用了 `UI4NotifyIcon`：csproj 内有 `<Reference Include="System.Drawing" />`，且托盘图标能显示（可用 `UI4Switch` 控制 `Visibility` 验证出现/消失）。
5. 若 XAML 未出现 `UI4CodeEditor`：把 `ICSharpCode.AvalonEdit.dll` 移出目录后应用仍能启动 —— 证明依赖判断正确；若出现，则该 dll 已列入部署脚本。
6. 发布脚本显式排除了 `System.Data.SQLite.dll` 与 `.pdb`，并写明「最小包 = exe + `StartUI4Controls.dll`（+ 用到的 AvalonEdit）+ exe.config」。
7. 窗内文本能打印 `Environment.OSVersion.Version` 与 `RuntimeInformation.FrameworkDescription`，两者分别与本机真实系统、4.8 相符（不符即回 7.3.3）。

---

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

---

# 第 09 章 主窗口骨架与页组织

> 本章解决：把 Demo 的三段式主窗改造成业务应用主窗，并选定「一页一文件」的内容组织方式，让后续增量写入不必堆进单个 500 行 XAML。
> 读者：AI Agent（要在本项目之外新建/续写 WPF 业务代码）
> 前置章节：第 8 章（启动序列，本章骨架假定资源键已播种）、第 10 章（XAML 书写纪律）、第 11 章（导航容器）

## 9.1 目标与验收

1. 主窗是「Header / 内容区 / Footer」三段式，三段都用主题令牌着色，切全局主题无需改一行 XAML。
2. 每个业务页是独立 `.xaml(.cs)` 文件；主窗 XAML 总行数控制在 150 行以内。
3. Footer 能显示两件事：运行期状态文本（`StatusText`）与主题回显（`CurrentMode`/`ResolvedKey`），并且**运行版本自证也走窗内文本**而不是弹窗。
4. 你的应用支持 `--page=N`（或 `--tab=N`）启动参数直接跳到指定页，供自动化脚本逐页验证。

判据：用 UIA 按 `AutomationId`（即 `x:Name`）能找到 `HeaderText`、`MainContent`、`StatusText` 三个元素；带 `--page=2` 启动时选中第三页。

## 9.2 源码依据

- `samples/StartUI4Demo/MainWindow.xaml:4` —— `xmlns:ui="clr-namespace:StartUI4Controls;assembly=StartUI4Controls"` 前缀约定（库无 `XmlnsDefinition`，`assembly=` 段不可省）。
- `MainWindow.xaml:11-16` —— 三行 `Grid`（`Auto / * / Auto`）骨架；`:8` `Background="{DynamicResource UI4.Brush.Background}"`。
- `MainWindow.xaml:18` —— `HeaderBorder`（`UI4.Brush.Surface` + `BorderNormal` 下边框）；`:53` —— `FooterBorder`（同上但 `0,1,0,0` 上边框）。
- `MainWindow.xaml:30` —— Demo 的内容区是一个 `TabControl x:Name="DemoTabs" TabStripPlacement="Left"`，11 页全部内联 `TabItem`。
- `MainWindow.xaml:56` —— 主题回显 `Text="{Binding Path=(ui:UI4Theme.CurrentMode), StringFormat=主题: {0}}"`。
- `MainWindow.xaml:472-476` —— 作用域卡片：`ui:UI4ThemeScope.Theme="dark"` 挂 `Border`，其内 `StackPanel` 用 `TextElement.Foreground="{DynamicResource UI4.Brush.Text}"` 给整棵子树定文字色；`:533-547` 嵌套作用域。
- `MainWindow.xaml:561-564` —— `UI4NotifyIcon` 放在 `Grid.Row="0"` 且 `Visibility="Collapsed"`（它必须在视觉树里，但不占版面）。
- `MainWindow.xaml.cs:20-47` —— 构造序列：先备数据 → `DataContext = this` → `InitializeComponent()` → 命名元素操作 → `ApplyStartupTab()`；`:50-62` `--tab=N` 解析；`:98-103`/`:218-227` XAML 解析期早触发事件的空判；`:157-176` Tab 增删；`:238-254` 主题回显刷新；`:278-301` 托盘/语言/收尾。
- `App.xaml:7-18` —— `SectionTitle` / `SectionHint` 两个样板样式（见第 10 章）。

## 9.3 正文

### 9.3.1 三段式骨架照抄并改名

把 Demo 的三段改成业务命名，结构保持不变：

```xml
<Window x:Name="RootWindow" x:Class="MyApp.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:ui="clr-namespace:StartUI4Controls;assembly=StartUI4Controls"
        Title="我的业务应用" Width="1280" Height="820"
        WindowStartupLocation="CenterScreen"
        Background="{DynamicResource UI4.Brush.Background}"
        Closed="MainWindow_Closed">
  <Grid>
    <Grid.RowDefinitions>
      <RowDefinition Height="Auto"/>
      <RowDefinition Height="*"/>
      <RowDefinition Height="Auto"/>
    </Grid.RowDefinitions>

    <Border x:Name="HeaderBorder" Grid.Row="0" Padding="16,10"
            Background="{DynamicResource UI4.Brush.Surface}"
            BorderBrush="{DynamicResource UI4.Brush.BorderNormal}" BorderThickness="0,0,0,1">
      <Grid>
        <TextBlock x:Name="HeaderText" Text="我的业务应用" FontSize="22" FontWeight="Bold"
                   Foreground="{DynamicResource UI4.Brush.Accent}"/>
        <TextBlock x:Name="RuntimeText" HorizontalAlignment="Right" VerticalAlignment="Center"
                   FontSize="12" Foreground="{DynamicResource UI4.Brush.Icon}"/>
      </Grid>
    </Border>

    <ContentControl x:Name="MainContent" Grid.Row="1" Margin="8"/>

    <Border x:Name="FooterBorder" Grid.Row="2" Padding="16,8"
            Background="{DynamicResource UI4.Brush.Surface}"
            BorderBrush="{DynamicResource UI4.Brush.BorderNormal}" BorderThickness="0,1,0,0">
      <Grid TextElement.Foreground="{DynamicResource UI4.Brush.Icon}">
        <TextBlock x:Name="StatusText" Text="就绪" FontSize="12" HorizontalAlignment="Left"/>
        <TextBlock x:Name="ThemeEcho" FontSize="12" HorizontalAlignment="Right"
                   Text="{Binding Path=(ui:UI4Theme.CurrentMode), StringFormat=主题: {0}}"/>
      </Grid>
    </Border>

    <!-- 托盘元素必须在视觉树内，但不占版面 -->
    <ui:UI4NotifyIcon x:Name="TrayIcon" Grid.Row="0" Visibility="Collapsed"
                      ToolTipText="我的业务应用"/>
  </Grid>
</Window>
```

改造要点（对照 Demo 行号）：`HeaderBorder`(:18) 与 `FooterBorder`(:53) 的画刷键原样保留；`UI4TextBlock`(:21-23) 换成标准 `TextBlock`（业务标题不需要它的阴影/渐变能力，少一个 `new`-隐藏 DP 的歧义源，见第 10 章 10.3.6）；`RuntimeText`(:25-26) 保留，用代码写运行时自证（9.3.5）；`DemoTabs`(:30) 换成 `ContentControl`（9.3.3）。

### 9.3.2 主题回显为什么能绑定静态属性

`{Binding Path=(ui:UI4Theme.CurrentMode)}` 绑的是**静态 CLR 属性**（`UI4Theme.cs:88`，不是 `DependencyProperty`）。WPF 4.5+ 支持 `(类型.属性)` 形式的路径绑静态属性，前提是声明类提供 `public static event PropertyChangedEventHandler StaticPropertyChanged` —— `UI4Theme.cs:97` 正是为此存在，`SetTheme`/`Apply` 在模式变化时 raise 它（`:124`、`:155`、`:182-183`）。

由此三条结论：

- 没有 `StaticPropertyChanged`，这条绑定只会取一次初值然后永远不动 —— 回显「看起来是死的」。
- 路径要写成附加属性形式 `(ui:UI4Theme.CurrentMode)`，写成 `UI4Theme.CurrentMode` 会被解析成实例属性路径而绑不上。
- 绑定沿用当前 `DataContext` 作为源对象；静态属性路径与 DataContext 类型无关，所以 Demo 里 `DataContext = this`（窗口自身）也能正常回显。

回显哪个量取决于你要证明什么：`CurrentMode` 是「最近一次请求的模式」（可能是 `System`）；`ResolvedMode` 是解析后的实际亮/暗；`ResolvedKey` 是主题键。自定义主题键会被 `CurrentMode`/`ResolvedMode` 报成 `Light`（`UI4Theme.cs:141-146`），要区分自定义主题必须比 `ResolvedKey` —— Demo 的 `UpdateScopeStatus()`（`MainWindow.xaml.cs:248-254`）就是同时打印 `ResolvedMode` 与 `ResolvedKey`，业务应用建议照做。

### 9.3.3 页组织三种做法对比

| 做法 | 结构 | 优点 | 代价 | 适用 |
|---|---|---|---|---|
| A 全内联 `TabItem`（Demo 用法，`MainWindow.xaml:30-551`） | 一个 `TabControl`，每页一个 `TabItem`，内容直接写在里面 | 零跳转、页签天然存在、改造成本 0 | 主窗 XAML 随页数线性膨胀（Demo 已 565 行）；Agent 增量写一个大文件容易整体重写、diff 巨大；跨页引用命名元素成常态 | 演示工程、单页小工具 |
| B 每页 `UserControl` + `ContentControl` | 主窗放 `ContentControl x:Name="MainContent"`，代码/导航把页实例塞进 `Content` | 一页一文件、可单独编译、Agent 每次只写一个文件；切换逻辑自己可控（可缓存实例、可延迟构造） | 要自己写「导航 + 选中态 + 页缓存」约 30 行 | 业务应用的默认选择 |
| C 每页 `UserControl` + 导航容器 | 用 `UI4NavigationView`（左栏）或 `UI4Pivot`（页内）承载页 | 导航外观、选中指示动画、左栏宽度这些都是现成的 | 容器各有主题跟随差异与模板期约束（见第 11 章）；`UI4NavigationView.Content` 走 `SelectedItem.Content` 绑定，页与导航项耦合 | 应用级左侧主导航 |

**推荐**：业务应用先落 B，若产品形态是「左侧一级导航 + 右侧内容」再升级为 C（把 B 的 `ContentControl` 换成 `UI4NavigationView`，页文件不动）。理由不是风格，是两条工程约束：① Agent 增量写入应当「新增一个文件 + 改一处注册表」，而不是在同一个巨型 XAML 里插入 200 行；② 单文件超过 ~400 行时 XAML 解析错误定位成本急剧上升（一个标签不匹配会让整窗加载失败，见第 10 章 10.5）。

做法 B 的注册表写法（一份 `List` 描述所有页，主窗只读它）：

```csharp
private sealed class PageEntry
{
    public string Title;
    public string IconGlyph;      // Segoe MDL2 码位
    public Func<UserControl> Create;
    public UserControl Cached;    // 惰性创建 + 缓存
}

private readonly List<PageEntry> _pages = new List<PageEntry>();

private void RegisterPages()
{
    _pages.Add(new PageEntry { Title = "工作台", IconGlyph = "\uE80F", Create = () => new WorkbenchPage() });
    _pages.Add(new PageEntry { Title = "订单",   IconGlyph = "\uE710", Create = () => new OrdersPage() });
}

private void ShowPage(int index)
{
    if (index < 0 || index >= _pages.Count) return;
    PageEntry p = _pages[index];
    if (p.Cached == null) p.Cached = p.Create();
    MainContent.Content = p.Cached;
    SetStatus(p.Title);
}
```

`IconGlyph` 用 `"\uE80F"` 形式的码位字符串而不是 XAML 的 `&#xE80F;` —— 代码里只能这么写（第 10 章图标约定）。

### 9.3.4 页文件（UserControl）的正确写法

```xml
<UserControl x:Class="MyApp.Pages.WorkbenchPage"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:ui="clr-namespace:StartUI4Controls;assembly=StartUI4Controls">
    <ScrollViewer VerticalScrollBarVisibility="Auto">
        <StackPanel Margin="16" MaxWidth="900" HorizontalAlignment="Left"
                    TextElement.Foreground="{DynamicResource UI4.Brush.Text}">
            <TextBlock Text="工作台" Style="{StaticResource SectionTitle}"/>
            <ui:UI4TextBox Width="320" PlaceholderText="搜索订单号" ShowClearButton="True"
                           HorizontalAlignment="Left" Margin="0,0,0,10"/>
        </StackPanel>
    </ScrollViewer>
</UserControl>
```

三条纪律：

- 每个用到 UI4 类型的 XAML 文件都要**自己**写 `xmlns:ui`（含 `assembly=StartUI4Controls`），不要指望父文件继承。
- 主题色一律 `DynamicResource`；页文件里可以没有 `Background`（让主窗的 `Background` 透出），若需要卡片底面就写 `{DynamicResource UI4.Brush.Surface}`。
- 文字色在**容器级**用 `TextElement.Foreground="{DynamicResource UI4.Brush.Text}"` 设一次，内部 `TextBlock` 继承，省掉逐元素赋值（Demo `:476`、`:505`、`:536`、`ScopeWindow.xaml:10` 都是这个写法）。注意这只对继承 `TextElement.Foreground` 的元素生效；`UI4TextBlock` 有自己的 `new Foreground`，`Foreground` 为 null 时才落到继承值（`UI4TextBlock.cs:354-357`）。
- 每页自带 `ScrollViewer`：主窗不给内容区加滚动，避免页高变化时整体抖动。

`StaticResource SectionTitle` 在这里是合法的 —— 那两个样式定义在 `App.xaml` 且不是主题令牌，不涉及切换刷新；样式**内部**的颜色仍是 `DynamicResource`（见第 10 章 10.3.2）。

### 9.3.5 状态栏设计：运行期自证放窗内

Footer 三个固定槽位：

```csharp
private void SetStatus(string text)
{
    // XAML 解析期控件就可能触发事件（如 UI4Switch 的 IsOn="True" 立刻 Toggled），
    // 此时命名字段尚未赋值 —— 必须空判（MainWindow.xaml.cs:98-103）
    if (StatusText != null) StatusText.Text = text;
}

private void EchoRuntime()
{
    RuntimeText.Text = "实际运行时：" +
        System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
    ThemeEcho.Text = "主题: " + UI4Theme.ResolvedMode + " / " + UI4Theme.ResolvedKey;
}
```

- 左槽 `StatusText`：交互反馈（哪个控件被点了、哪个操作完成），UIA 可直接读，脚本据此判定。
- 右槽 `ThemeEcho`：主题回显，绑定或代码赋值二选一；订阅 `UI4Theme.ThemeChanged` 后用代码刷新能顺带打印 `ResolvedKey`（照 `MainWindow.xaml.cs:43` + `:248-254`）。
- Header 右槽 `RuntimeText`：运行版本自证（.NET Framework 4.8.x）。**禁止**用 `MessageBox` 做这类自证 —— 弹窗阻塞 Dispatcher，自动化脚本会卡在第一个断言前（第 8 章 8.4）。

### 9.3.6 复制 `--tab=N` 给你的应用

照 `MainWindow.xaml.cs:50-62` 的写法改成你的页表，保持「0 基索引 + 越界静默忽略 + `InvariantCulture` 解析」三条：

```csharp
// 支持 "--page=N" 直接打开指定页，便于自动化逐页验证。
private int ResolveStartupPage(int pageCount)
{
    int index = 0;
    foreach (string arg in Environment.GetCommandLineArgs())
    {
        if (!arg.StartsWith("--page=", StringComparison.OrdinalIgnoreCase)) continue;
        int parsed;
        if (int.TryParse(arg.Substring(7), NumberStyles.Integer,
                CultureInfo.InvariantCulture, out parsed)
            && parsed >= 0 && parsed < pageCount)
        {
            index = parsed;
        }
    }
    return index;
}
```

在构造序列里 `InitializeComponent()` **之后**调用（`MainWindow.xaml.cs:41` 的位置就是这条约束），并在 `Loaded` 之后 `Dispatcher.BeginInvoke` 一次 `ShowPage` —— 若页容器是 `ContentControl` 就没这个必要，若是 `UI4Pivot`/`UI4NavigationView`，模板未应用前设置选中不会呈现内容（第 11 章）。参数名固定成 `--page=N` 并写进 README，让脚本作者一眼知道。

### 9.3.7 `DataContext = this` 的现实与升级路径

Demo 用 `DataContext = this`（`MainWindow.xaml.cs:23`）+ 普通 `List<CardItem>` 无 INPC（`:64-73`）+ `ItemTemplate` 而不是 `DisplayMemberPath`（`:248-261`）。这是**故意保留的最小心智模型**：页文件里的 `{Binding}` 直接指向主窗公共属性，不需要 VM、DI、框架。

沿用它的条件：页是只读展示、数据一次算好、不需要运行期刷新。需要刷新时**不要**把 `DataContext` 换成 `this` 的兄弟对象，而是给数据类实现 `INotifyPropertyChanged`，或让页文件自己 `DataContext = pageInstance` 后用 `{Binding}` 绑页的公共属性。

本库不提供 MVVM 支撑（无 `ObservableObject`、无命令包装、无 DI 容器），升级路径与命名约定、页内 `Command` 的写法统一放在第 23 章，本章不重复。

## 9.4 推荐做法（Do / Don't）

- Do：主窗只保留三段骨架 + 页注册表，业务内容一律进 `Pages/*.xaml`。
- Do：三段的画刷键固定成 `Surface`（底）/ `BorderNormal`（分隔线）/ `Background`（窗），与 Demo 一致，便于对照截图基线。
- Do：每个页文件自己写全 `xmlns:ui`（含 `assembly=`）与容器级 `TextElement.Foreground`。
- Do：`SetStatus` 一律空判；把「解析期早触发」当作常态而不是异常。
- Do：给应用加 `--page=N`，并在文档里承诺它是自动化入口。
- Don't：不要把 `UI4NotifyIcon` 移出视觉树（比如从 XAML 删掉再代码里挂），它必须在树里，用 `Grid.Row` + `Visibility="Collapsed"` 藏（`MainWindow.xaml:561`）。
- Don't：不要在主窗 XAML 里用 `StaticResource UI4.Brush.*`（键是解析期快照，切主题不更新）。
- Don't：不要为了少几个文件而把页做成主窗里的 `DataTemplate` 大杂烩 —— 页有代码后置需求时会立刻后悔。
- Don't：不要用弹窗证明运行版本（用 `RuntimeText`）。

## 9.5 坑与排错

| 症状 | 真因 | 处置 |
|---|---|---|
| 主题回显文本永远不变 | 忘了 `StaticPropertyChanged` 前提：路径写成 `UI4Theme.CurrentMode` 而非 `(ui:UI4Theme.CurrentMode)` | 加括号形式；或改为订阅 `ThemeChanged` 代码赋值 |
| 回显显示 `Light`，但你明明应用了自定义品牌主题 | 自定义键被映射为 `Light`（`UI4Theme.cs:141-146`） | 比 `ResolvedKey`，别比 `CurrentMode` |
| 页在 `ContentControl` 里显示，但切走后回来丢了滚动位置/输入内容 | 每次 `Content` 赋新实例 | 用 9.3.3 的 `Cached` 字段缓存页实例 |
| 首次启动 `--page=2` 无效，页仍是第一页 | 在 `InitializeComponent()` 前设选中，命名元素还是 null；或容器模板未应用 | 挪到构造末尾或 `Loaded` 之后 |
| 页内控件文字在深色主题下仍是黑色 | 页自己或祖先写了硬编码 `Foreground="Black"`，切断了继承 | 删除硬编码，改为容器级 `TextElement.Foreground="{DynamicResource UI4.Brush.Text}"` |
| Footer 高度在长状态文本下变化，内容区抖动 | `RowDefinition Height="Auto"` + 文本换行 | Footer 设 `MinHeight`，状态文本用 `TextTrimming="CharacterEllipsis"` |
| 局部主题页里卡片边框不变色 | 卡片 `BorderBrush` 写死常量而非 `{DynamicResource UI4.Brush.BorderNormal}`（Demo `:475` 的写法） | 换回令牌键；显式本地值会让主题跟随失效（第 12 章） |

## 9.6 完成判据

1. 主窗 XAML 行数 ≤ 150；`grep -c "<TabItem"` 为 0；页文件数量 = 业务模块数。
2. `HeaderBorder`/`FooterBorder` 与 `MainContent` 三个 `x:Name` 存在，UIA 可按 `AutomationId` 命中。
3. 切一次全局主题（`UI4Theme.SetTheme(Dark)`），不改任何 XAML：三段底色、分隔线、状态文本色全部同步变化；`ThemeEcho` 文本变化。
4. 打印 `ResolvedMode` 与 `ResolvedKey` 两者，且能解释「自定义主题下 `CurrentMode` 报 `Light`」。
5. `--page=N` 对每个合法 N 生效、越界不崩；启动参数名已写进你的 README。
6. 每个页文件都自包含：`xmlns:ui` 完整、容器级文字色、自带 `ScrollViewer`、没有 `StaticResource UI4.*`。
7. `SetStatus` 与所有解析期可能触发的事件处理都带空判；构造序列顺序为「备数据 → `DataContext` → `InitializeComponent()` → 命名元素操作 → 启动参数」。

---

# 第 10 章 XAML 编写规范与命名约定

> 本章解决：把宿主 XAML 的书写纪律固定下来 —— 前缀、主题色消费方式、命名、图标、`new`-隐藏属性的规避写法，以及常见报错到真因的对照。
> 读者：AI Agent（要在本项目之外新建/续写 WPF 业务代码）
> 前置章节：第 9 章（骨架与页文件）、第 12 章（为什么必须 `DynamicResource` 的机制解释）

## 10.1 目标与验收

1. 全工程 XAML 里主题相关颜色**只出现** `DynamicResource` + `UI4.Color.*` / `UI4.Brush.*` 两类键；`grep` 搜不到 `StaticResource UI4.`，也搜不到硬编码 `#RRGGBB` 的主题色。
2. 所有需要被脚本定位的元素都有 `x:Name`，且事件处理命名统一为 `<Element>_<Event>`。
3. 只用仓库里存在的成员名：写每个属性前先在 `src/StartUI4Controls/` 里 `grep` 一次（本库有四条「文档说谎」，见 10.3.8）。
4. 一页 XAML 能独立编译：自带 `xmlns:ui`，不依赖父文件的命名空间。

## 10.2 源码依据

- `samples/StartUI4Demo/MainWindow.xaml:4`、`ScopeWindow.xaml:4` —— 全工程统一前缀写法 `xmlns:ui="clr-namespace:StartUI4Controls;assembly=StartUI4Controls"`。
- `src/StartUI4Controls/AssemblyInfo.cs` —— 只有 `[assembly: ThemeInfo(...)]`，**没有 `XmlnsDefinition`** ⇒ `assembly=` 段不可省略。
- `src/StartUI4Controls/UI4Theme.cs:297-309` —— 资源键生成规则：30 个 `UI4.Color.<Token>` + 30 个 `UI4.Brush.<Token>` + 三个别名 `UI4.Brush.Text` / `UI4.Brush.Border` / `UI4.Brush.Accent`。
- `src/StartUI4Controls/UI4ThemeToken.cs:7-39` —— 令牌枚举原文（键名逐字取自枚举成员名，大小写敏感）。
- `src/StartUI4Controls/UI4TextBlock.cs:50-61`（`Foreground`）、`:116-122`（`Padding`）、`:129-135`（`FontSize`）、`:142-148`（`FontWeight`）四处 `new` 注册的 DP；`:349-350`、`:354-357` 模板内部对这几个新 DP 的绑定。
- `src/StartUI4Controls/UI4Panel.cs:46-56`（`BorderColor` 是 `Color`）、`:172-173`（`BorderThickness` 重写默认值为 1 并触发重建）、`:220-224`（内层 `Border.BorderBrush` 只由 `BorderColor` 生成）。
- `src/StartUI4Controls/UI4CheckBox.cs:43-56` —— `BoxCornerRadiusProperty = CornerRadiusProperty` 的 `[Obsolete]` 别名。
- `src/StartUI4Controls/UI4TabControl.cs:19/:32-34`、`UI4NavigationView.cs:87` —— `TextIcon` / `TextIconFontFamily`（`FontFamily` 类型）；`UI4Menu.cs:296/:307/:318` —— 菜单侧叫 `TextIcon` / `IconFontFamily` / `IconFontSize`（**不同名**）。
- `samples/StartUI4Demo/App.xaml:7-18` —— `SectionTitle` / `SectionHint` 两个样板样式（`Setter` 值用 `DynamicResource`）。
- `MainWindow.xaml:21-23`、`:302`、`:316`、`:476`、`ScopeWindow.xaml:10` —— 图标码位与容器级文字色的实例写法。
- `MainWindow.xaml.cs:39`、`:107`、`:157/:172`、`:218`、`:262`、`:278` —— `<Element>_<Event>` 命名实例。

## 10.3 正文

### 10.3.1 xmlns 前缀约定

只认一种写法，前缀名统一为 `ui`：

```xml
xmlns:ui="clr-namespace:StartUI4Controls;assembly=StartUI4Controls"
```

- 库没有 `XmlnsDefinition` 特性，所以**去掉 `;assembly=StartUI4Controls` 一定失败**（同程序集内才可省）。
- 每个用到 UI4 类型的 XAML 文件都要重复这一行；命名空间不继承父文件之外的范围，UserControl 也必须自带。
- 你的业务前缀另起（如 `xmlns:local="clr-namespace:MyApp.Pages"`），不要复用 `ui`。
- 宿主程序集名与库不同，所以 `x:Class` 前缀是你的命名空间，而 `xmlns:ui` 的 `clr-namespace` 永远是 `StartUI4Controls`。

### 10.3.2 主题色一律 DynamicResource

规则：**凡颜色/画刷，出现在元素属性或 `Style` 的 `Setter.Value` 上，一律 `DynamicResource`。**

```xml
<!-- 正确 -->
<Border Background="{DynamicResource UI4.Brush.Surface}"
        BorderBrush="{DynamicResource UI4.Brush.BorderNormal}" BorderThickness="0,0,0,1"/>

<!-- 错误：解析期快照，切主题不更新 -->
<Border Background="{StaticResource UI4.Brush.Surface}" .../>

<!-- 错误：硬编码，等于永久脱离主题 -->
<Border Background="#FFFFFF" .../>
```

原因是资源桥**原地覆盖同一批键**（`UI4Theme.WriteTokens` 对同一个 `IDictionary` 反复赋同名键），`StaticResource` 在解析期把当时的值取走后就与字典脱钩。机制细节见第 12 章。

`Style` 内可以且应该用 `DynamicResource`（`App.xaml:10`、`:15` 就是这么写的），`Style` 本身用 `StaticResource` 引用没问题：

```xml
<TextBlock Text="订单列表" Style="{StaticResource SectionTitle}"/>
```

反过来，主题令牌键**绝不能**用 `StaticResource`，除非你确定该元素所在整棵子树永不切主题。

### 10.3.3 可用资源键清单

30 个令牌 → 每个有两个键；别名只有 Brush 形式：

| 类别 | 键 |
|---|---|
| 颜色 | `UI4.Color.Accent` `UI4.Color.AccentDark` `UI4.Color.AccentEnd` `UI4.Color.TextForeground` `UI4.Color.TextSecondary` `UI4.Color.Background` `UI4.Color.Surface` `UI4.Color.BorderNormal` `UI4.Color.BorderSecondary` `UI4.Color.BorderHover` `UI4.Color.BorderFocus` `UI4.Color.Placeholder` `UI4.Color.HoverOverlay` `UI4.Color.SelectedOverlay` `UI4.Color.TrackBackground` `UI4.Color.CheckBackground` `UI4.Color.Icon` `UI4.Color.IconHover` `UI4.Color.PanelBorder` `UI4.Color.OffBackground` `UI4.Color.MenuBackground` `UI4.Color.ListSelected` `UI4.Color.HeaderBackground` `UI4.Color.HeaderForeground` `UI4.Color.RowHoverBackground` `UI4.Color.RowSelectedBackground` `UI4.Color.GridLine` `UI4.Color.ProgressStart` `UI4.Color.CheckBoxUnchecked` `UI4.Color.HoverBorderColorLight` |
| 画刷 | 上述每一项把 `Color` 换成 `Brush` 即存在（`UI4.Brush.TextForeground` …） |
| 别名 | **仅** `UI4.Brush.Text`（= `TextForeground`）、`UI4.Brush.Border`（= `BorderNormal`）、`UI4.Brush.Accent`（= `Accent`） |

两条硬性推论：

- **别名没有 Color 形式**：`UI4.Color.Text`、`UI4.Color.Border` 不存在。给 `Color` 类型 DP（`UI4Pivot.ItemForeground`、`UI4TextBox.FocusBorderColor`、`UI4Panel.BorderColor`）赋值时必须写全名 `UI4.Color.TextForeground` / `UI4.Color.BorderNormal`。
- 键名大小写敏感、逐字取自枚举成员名。`UI4.Brush.TextForeground` 正确，`UI4.Brush.TextForegroundBrush`、`UI4.Brush.Foreground` 都不存在。

`Color` 还是 `Brush` 由**目标 DP 的类型**决定，不由你的偏好决定：`Brush` DP 用 `UI4.Brush.*`，`Color` DP 用 `UI4.Color.*`，写反了是「类型转换失败」而不是「取不到值」。

### 10.3.4 容器级文字色：TextElement.Foreground

与其给每个 `TextBlock` 写色，不如在容器上写一次，让子树继承（Demo `MainWindow.xaml:476`、`:505`、`:536`、`ScopeWindow.xaml:10`）：

```xml
<StackPanel TextElement.Foreground="{DynamicResource UI4.Brush.Text}">
  <TextBlock Text="作用域卡片标题" FontWeight="SemiBold"/>
  <TextBlock Text="副标题"/>
</StackPanel>
```

三个适用边界：

- 只对继承 `TextElement.Foreground` 的元素生效（`TextBlock`、`ContentControl` 内部文本等）。
- 一旦某个子元素自己写了 `Foreground="..."`（本地值），继承链就在该元素断开，切主题后它停在旧色 —— 这是「局部不留神就漏跟主题」的头号来源。
- 同理可用 `TextElement.FontSize` / `TextElement.FontWeight` 统一起手，但**对 `UI4TextBlock` 无效**（见 10.3.6）。

### 10.3.5 x:Name 与事件命名约定

- 凡需要被代码或脚本定位的元素都要 `x:Name`。`x:Name` 同时成为 UIA 的 `AutomationId`，是本仓库所有验证脚本的定位依赖（库内没有任何 `OnCreateAutomationPeer` 实现）。
- 命名用 PascalCase 且语义化：`HeaderBorder`、`DemoTabs`、`ScopeKeyCombo`、`StatusText`、`CtxHost`（Demo 全项目一致）。
- 事件处理命名 `<Element>_<Event>`：`Switch_Toggled`、`ScopeKeyCombo_SelectionChanged`、`BrowserTab_AddTab`、`TrayIcon_TrayRightMouseDown`、`MainWindow_Closed`。不要写 `Handler1`、`OnClick`。
- `x:Name` 与 `Name` 不要同时给；`x:Name` 生成的字段是 `internal`，页内直接用名字访问即可。
- **处理解析期早触发的事件必须空判**：`SelectedIndex="0"`、`IsOn="True"` 这类初值会在 `InitializeComponent()` 期间就触发事件，此时其它命名字段还是 null（`MainWindow.xaml.cs:98-103`、`:218-227` 两处注释都强调）。约定写法是处理函数第一行 `if (所需元素 == null) return;`。

### 10.3.6 `new`-隐藏 DP 造成的歧义与规避

库里有两类「与基类同名但**不是同一个属性**」的情况，必须区别对待。

**① `UI4TextBlock`（`ContentControl` 的子树渲染）用 `new` 重新注册了四个 DP**：`Foreground`（`:50-61`，默认 null）、`Padding`（`:116-122`）、`FontSize`（`:129-135`，默认 15）、`FontWeight`（`:142-148`）。它们与 `Control`/`Control.FontSize` 等基类属性是**两条独立路径**：

```xml
<!-- 生效：直接写在元素上，命中的是 UI4TextBlock 自己的 DP -->
<ui:UI4TextBlock Text="标题" FontSize="24" Foreground="{DynamicResource UI4.Brush.Text}"/>

<!-- 不生效：容器级 TextElement.FontSize / TextBlock.FontSize 附加属性进不了模板，
     模板已把内部 TextBlock 的 FontSize 绑到 UI4TextBlock.FontSize（:349），默认 15 -->
<StackPanel TextElement.FontSize="24">
  <ui:UI4TextBlock Text="标题"/>   <!-- 仍是 15 -->
</StackPanel>

<!-- 不生效：代码里SetValue(Control.ForegroundProperty, ...) 动的是基类 DP -->
```

规避：`UI4TextBlock` 的字号/粗细/内边距/前景一律写在元素自身；文字色想跟随容器就**完全不要写 `Foreground`**（默认 null 时模板不覆盖内部文本，继承值才能落地，见 `:354-357`）。另注意它不是 `System.Windows.Controls.TextBlock`，把两者混进同一个 `as`/参数类型会 CS1503。

**② `UI4Panel` 的描边**：内层 `Border` 的 `BorderBrush` 由 `BorderColor`（`Color` 类型，`:46-56`、`:224`）生成，`Control.BorderBrush` **对渲染无影响**；`BorderThickness` 有效（`:172-173` 重写默认值为 1，`:220-221` 绑到内层）。

```xml
<!-- 正确 -->
<ui:UI4Panel BorderColor="{DynamicResource UI4.Color.BorderNormal}" BorderThickness="2" CornerRadius="16"/>
<!-- 错误：写了不会显示，且会让人误以为「主题没生效」 -->
<ui:UI4Panel BorderBrush="{DynamicResource UI4.Brush.BorderNormal}" .../>
```

同族提醒：`HoverBorderBrush` 是 `SolidColorBrush` 类型（`:59-69`），与 `BorderColor`（`Color`）不是同一类型 —— 同一概念在不同控件上类型不同（`UI4ListView.ItemBackground` 是 `Brush`）。

### 10.3.7 图标约定

仓库内**没有任何图标字体或图片资源**。图标一律用系统字体 `Segoe MDL2 Assets` 的码位：

```xml
<ui:UI4TabItem Header="设置" TextIcon="&#xE713;" TextIconFontFamily="Segoe MDL2 Assets" IsClosable="False"/>
<ui:UI4NavigationViewItem TextIcon="&#xE104;" TextIconFontFamily="Segoe MDL2 Assets" Header="代码"/>
<ui:UI4MenuElementItem Header="新建" TextIcon="&#xE710;" IconFontFamily="Segoe MDL2 Assets" IconFontSize="14"/>
```

- XAML 里写实体 `&#xE713;`；C# 里写转义 `"\uE713"`（`MainWindow.xaml.cs:163`）。
- 属性名**不统一**：`UI4TabItem` / `UI4NavigationViewItem` 是 `TextIcon` + `TextIconFontFamily`（`FontFamily` 类型），`UI4MenuElementItem` 是 `TextIcon` + `IconFontFamily` + `IconFontSize`。写之前按控件查名。
- 码位不要凭记忆猜：先在 Demo 里找已用过的（`&#xE713;` 设置、`&#xE710;` 新增、`&#xE8E5;` 打开、`&#xE723;` 文档、`&#xE80F;` 主页、`&#xE7A6;`/`&#xE7A7;` 重做/撤销、`&#xE104;` 播放/代码、`&#xE8F1;` 属性）。

### 10.3.8 四条「文档说谎」与 Obsolete 别名

写 XAML/代码前，以下成员名以源码为准，不要照 README 或类注释抄：

1. `UI4ComboBox` **没有** `HoverBorderColor` / `FocusBorderColor`（焦点边框是渐变，实为 `FocusGradientStart` / `FocusGradientEnd`）。`FocusBorderColor` / `HoverBorderColor` 只存在于 **`UI4TextBox`**（`UI4TextBox.cs:50/:59`）。
2. `UI4Panel` **没有** `Title` 属性（类注释残留）。
3. `UI4MultiLanguage` **没有** `SetLanguage(string)`：切语言要改 `CultureInfo.CurrentUICulture` 再调 `UI4MultiLanguage.Refresh()`（`MainWindow.xaml.cs:278-286`）。
4. `UI4ContextMenuLanguage` 已被删除，不要在 XAML 里引它。
5. `UI4CheckBox.BoxCornerRadius` 是 `[Obsolete]` 别名 DP，与 `CornerRadius` 是**同一个** DP（`UI4CheckBox.cs:47`）。Demo 里仍写 `BoxCornerRadius="6"`（`MainWindow.xaml:51`）能工作但会吃 CS0618 警告；新代码写 `CornerRadius`。

### 10.3.9 样板样式放 App.xaml

两个跨页复用的文本样式照抄 `App.xaml:7-18`，只放 `Application.Resources`（页文件里不重复定义）：

```xml
<Style x:Key="SectionTitle" TargetType="TextBlock">
  <Setter Property="FontSize" Value="18"/>
  <Setter Property="FontWeight" Value="SemiBold"/>
  <Setter Property="Foreground" Value="{DynamicResource UI4.Brush.Accent}"/>
  <Setter Property="Margin" Value="0,18,0,8"/>
</Style>
<Style x:Key="SectionHint" TargetType="TextBlock">
  <Setter Property="FontSize" Value="12"/>
  <Setter Property="Foreground" Value="{DynamicResource UI4.Brush.Icon}"/>
  <Setter Property="Margin" Value="0,0,0,6"/>
  <Setter Property="TextWrapping" Value="Wrap"/>
</Style>
```

约定：`SectionTitle` 用于分区标题（强调色），`SectionHint` 用于说明文字（次要 `Icon` 色）。样式键是你自己定的普通键，所以引用它们用 `StaticResource` 合理；样式**内部**的颜色必须 `DynamicResource`。

## 10.4 推荐做法（Do / Don't）

- Do：写任何 UI4 成员名前 `grep` 一次源码，确认存在与类型（`Color` 还是 `Brush`）。
- Do：颜色走 `DynamicResource` + 令牌键；字号/边距/圆角这类尺寸可以直接写数字。
- Do：容器级 `TextElement.Foreground` 打底，元素级只在真正要突出时覆盖。
- Do：给脚本要定位的元素一律加 `x:Name`，命名即 `AutomationId`。
- Do：事件处理第一行做空判，把「解析期就触发」当默认预期。
- Don't：不要写 `{StaticResource UI4.Brush.*}`，也不要写 `{DynamicResource UI4.Color.Text}` 这种不存在的别名 Color 形式。
- Don't：不要用 `TextElement.FontSize` 去统一 `UI4TextBlock` 的字号（无效）。
- Don't：不要在新代码里用 `BoxCornerRadius`。
- Don't：不要引入自定义图标字体或 `.ico` —— 仓库无字体/图片资源，托盘用系统回退即可。

## 10.5 常见 XAML 报错 → 真因对照

| 报错形态 | 真因 | 处置 |
|---|---|---|
| 「类型 `ui:XXX` 不存在 / 无效标签名」（常成片出现，MC3074 一类） | ① 类名拼错；② `xmlns:ui` 少了 `;assembly=`；③ **库内有 C# 编译错误造成级联**，dll 没产出，所有 XAML 一起报 | 先全量构建库工程看真实错误列表（`事实速查.md` 第 7 节第 1 条），确认 dll 存在后再查 XAML |
| 「属性 `X` 在类型 `Y` 上未找到」 | 该属性只存在于过时文档（10.3.8 四条），或你把 A 控件的属性抄到 B 控件上（`IconFontFamily` vs `TextIconFontFamily`） | 用源码 `grep` 确认；改名或删掉该属性 |
| 「找不到名为 `UI4.Brush.X` 的资源」 | ① 用了 `StaticResource` 而键尚未播种（第 8 章）；② 键名拼错（多/少后缀、把别名写成 Color） | 改 `DynamicResource`，核对 10.3.3 清单；确认 `UI4Theme.ApplyToApplication()` 已在主窗创建前调用 |
| 「`Cannot use DynamicResource`」/ 资源引用无效 | 把 `{DynamicResource}` 用在了非依赖属性上（POCO、普通 CLR 属性），或用在 `Trigger` 的条件值上 —— 触发条件需要可比对的常量 | 把颜色移到 `Setter.Value`；触发条件改成常量或换用 `DataTrigger` + 绑定 |
| 类型转换失败（Color 给 Brush DP 或反之） | 键的类型与 DP 类型不匹配（`UI4.Color.*` 是 `Color`，`UI4.Brush.*` 是 `SolidColorBrush`） | 按 10.3.3 的推论选形式；`Color` 类型 DP 一律 `UI4.Color.*` |
| 元素在深色主题下文字仍是黑色 | 元素上有硬编码 `Foreground` 本地值，切断了继承；或对 `UI4TextBlock` 误以为继承生效 | 删本地值或改 `DynamicResource`；`UI4TextBlock` 要显式写 `Foreground` |
| 切主题后某控件不变色，重启后却是对的 | 你在被主题管理的 DP 上赋了本地值（`ReadLocalValue` 有值即跟随失效） | 删掉赋值，或改用 `UI4ContextMenu` 之外不受此约束的属性；参见第 12 章 |
| 运行时抛「找不到 `ICSharpCode.AvalonEdit`」 | XAML 里放了 `UI4CodeEditor` 但部署包缺 dll（第 7 章） | 补 dll 或去掉该控件 |

## 10.6 完成判据

1. `grep -r "StaticResource UI4\."` 命中 0 次；`grep -r "clr-namespace:StartUI4Controls"` 的结果数 = 用到 UI4 的 XAML 文件数（每文件都自带）。
2. 逐条核对：工程里没有 `UI4ComboBox.HoverBorderColor`/`FocusBorderColor`、`UI4Panel.Title`、`UI4MultiLanguage.SetLanguage`、`UI4ContextMenuLanguage`、`BoxCornerRadius` 这五个不存在/废弃名。
3. 所有 `{DynamicResource UI4.*}` 的键都能在 10.3.3 清单里找到，且形式（Color/Brush）与目标 DP 类型匹配。
4. 需要脚本定位的元素都有 `x:Name`；随机抽三个事件处理，命名符合 `<Element>_<Event>` 且首行有空判。
5. 图标全部是 `Segoe MDL2 Assets` 码位，XAML 用 `&#x…;`、C# 用 `\u…`，属性名与该控件一致。
6. 切一次全局主题，页面上**没有**任何元素颜色停在旧值（用截图基线或 UIA 读色值对比）。
7. `App.xaml` 里 `SectionTitle` / `SectionHint` 存在且内部颜色为 `DynamicResource`；页文件中无重复定义。

---

# 第 11 章 导航容器选型与落地

> 本章解决：在「应用级主导航 / 页内切换 / 可关闭文档页」三种形态里选对一个容器，并给出四种容器的可抄骨架与「加一页」的完整改动清单。
> 读者：AI Agent（要在本项目之外新建/续写 WPF 业务代码）
> 前置章节：第 9 章（主窗骨架与页组织）、第 12 章（主题生效时序，解释 11.3.7 的深色观感差异）

## 11.1 目标与验收

1. 从四个容器（原生 `TabControl` / `UI4Pivot` / `UI4Tab` / `UI4NavigationView`）中按决策表选定一个，另一页内容能切换出来。
2. 每页都是独立 `UserControl`（第 9 章做法 B/C），容器只负责「选哪一页」。
3. 深色主题下，导航条与内容区**没有**残留浅色块；说不清观感来源就退回 11.3.7 的结论表。
4. 导航项文本在切换语言后会刷新（本库是构造期拉取，需宿主重建，见 11.3.8）。

## 11.2 源码依据

- `samples/StartUI4Demo/MainWindow.xaml:30`（`TabControl x:Name="DemoTabs" TabStripPlacement="Left"`）、`:33/:77/…/:444`（11 个内联 `TabItem`）、`:283-341`（导航容器页，四个容器同屏对照）。
- `MainWindow.xaml.cs:157-176` —— `BrowserTab_AddTab` / `BrowserTab_CloseTab` 实例：不置 `e.Handled` 时由控件自行移除标签。
- `src/StartUI4Controls/UI4Pivot.cs:13-48`（`UI4PivotItem`，`IsBrand` `:15-22`）、`:51-179`（`UI4Pivot` 外观 DP 与 `SetResourceReference`）、`:199-205`（`OnApplyTemplate` 取 `PART_ContentPresenter`）、`:288-298`（`IsBrand` 触发器）、`:317-331`（`ApplySelection` 的模板依赖）。
- `src/StartUI4Controls/UI4TabControl.cs:15-110`（`UI4TabItem`：`TextIcon/:19`、`TextIconFontFamily/:32`、`IsClosable/:69-79`、`IsBrand/:82-89`、`CloseTab` 路由事件 `:92-102`）、`:136-306`（`UI4Tab`：外观 DP 全是 `Color`、`ShowAddButton/:255-265` 默认 `true`、`AddTab/:281-291`、`CloseTab/:296-306`）、`:597-615`（关闭按钮回调与自行移除）、`:694-703`（`TabCloseRoutedEventArgs.TabItem`）。
- `src/StartUI4Controls/UI4NavigationView.cs:17-41`（`NullToVisibilityConverter` / `InverseNullToVisibilityConverter`）、`:56-95`（`UI4NavigationViewItem`：`Header`/`ImageSource`/`TextIcon`/`TextIconFontFamily`）、`:107`（`UI4NavigationViewBottomItem`）、`:111`（`UI4NavigationView : ItemsControl, IThemeAware`）、`:161-170`（`LeftPanelWidth`：`double.NaN` + `BindsTwoWayByDefault | AffectsMeasure`）、`:327-334`（`Header`）、`:359-366`（`SelectedItem`，类型 `UI4NavigationViewItem`，默认双向）、`:493/:511`（Header 可见性用空判转换器）、`:584/:618`（两个 `ListBox` 绑 `RegularItems`/`BottomItems`）、`:643`（内容区绑 `SelectedItem.Content`）、`:681-696`（`OnItemsChanged` 由 `Items` 重建两个集合）、`:698-746`（`OnApplyTemplate` 与初值选中）。

## 11.3 正文

### 11.3.1 方案 A：原生 `TabControl`（Demo 用法）

迁移成本 0，观感不可控（库不重绘原生 `TabControl`）。

```xml
<TabControl x:Name="MainTabs" Grid.Row="1" TabStripPlacement="Left"
            Margin="8" Background="Transparent" BorderThickness="0">
  <TabItem Header="工作台"><pages:WorkbenchPage/></TabItem>
  <TabItem Header="订单"><pages:OrdersPage/></TabItem>
</TabControl>
```

要点：`Background="Transparent"` + `BorderThickness="0"` 去掉原生边框底色（Demo `:30` 就是这么写的）；页做成 `UserControl` 直接放 `TabItem` 内容，比 Demo 的内联更省行数。选中联动：`MainTabs.SelectedIndex` 与 `--page=N` 对齐即可，脚本按 `TabItem` 的 `Name`（Header 文本）也能定位。

### 11.3.2 方案 B：`UI4Pivot` + `UI4PivotItem`（页内大标题切换）

```xml
<ui:UI4Pivot x:Name="MainPivot" Height="600" ItemFontSize="16" SelectedFontSize="20">
  <ui:UI4PivotItem IsBrand="True" Header="我的应用">
    <TextBlock Text="品牌位，钉在最前" Margin="10"/>
  </ui:UI4PivotItem>
  <ui:UI4PivotItem Header="工作台"><pages:WorkbenchPage/></ui:UI4PivotItem>
  <ui:UI4PivotItem Header="订单"><pages:OrdersPage/></ui:UI4PivotItem>
</ui:UI4Pivot>
```

- 外观 DP 全是 **`Color` 类型**：`ItemForeground`、`ItemHoverForeground`、`SelectedItemForeground`、`ItemFontSize`、`SelectedFontSize`、`BrandFontSize`、`ItemFontWeight`、`BrandFontWeight`、`ItemPadding`、`ItemMargin`。要给它们跟主题，必须写 `{DynamicResource UI4.Color.…}` 形式，写 `UI4.Brush.*` 会类型不匹配。
- 主题通道：**引用式**。构造里 `SetResourceReference(ItemForegroundProperty, "UI4.Color.TextForeground")` 等（`UI4Pivot.cs:176-178`）⇒ 作用域内自动跟随。反过来说：你在 XAML 里显式写 `SelectedItemForeground="Black"`（Demo `:288` 正是如此）就**覆盖掉了这条引用**，该色从此不跟主题。
- `IsBrand="True"` 只影响字号/字重触发器（`:288-298`）；品牌项习惯放最前，靠声明顺序保证。
- **内容呈现依赖模板已应用**：`_contentPresenter` 只在 `OnApplyTemplate` 里取得（`:199-205`），`ApplySelection` 在为 null 时直接返回（`:317-322`）。⇒ 把 Pivot 放在 `Visibility="Collapsed"` 容器里时，首次可见前不会有内容；启动参数直达某页要在 `Loaded` 之后再设 `SelectedIndex`。

### 11.3.3 方案 C：`UI4Tab` + `UI4TabItem`（可关闭文档页）

```xml
<ui:UI4Tab x:Name="DocsTab" Height="520" ShowAddButton="True"
           HeaderBackground="{DynamicResource UI4.Color.Surface}"
           TabSelectedBackground="{DynamicResource UI4.Color.Surface}"
           TabForeground="{DynamicResource UI4.Color.TextForeground}"
           TabSelectedForeground="{DynamicResource UI4.Color.TextForeground}"
           CloseButtonColor="{DynamicResource UI4.Color.Icon}"
           AddButtonColor="{DynamicResource UI4.Color.Icon}"
           AddTab="DocsTab_AddTab" CloseTab="DocsTab_CloseTab">
  <ui:UI4TabItem Header="主页" TextIcon="&#xE80F;" TextIconFontFamily="Segoe MDL2 Assets">
    <pages:HomePage/>
  </ui:UI4TabItem>
  <ui:UI4TabItem Header="设置" TextIcon="&#xE713;" TextIconFontFamily="Segoe MDL2 Assets" IsClosable="False">
    <pages:SettingsPage/>
  </ui:UI4TabItem>
</ui:UI4Tab>
```

```csharp
private int _tabCounter;

private void DocsTab_AddTab(object sender, RoutedEventArgs e)
{
    _tabCounter++;
    UI4TabItem item = new UI4TabItem
    {
        Header = "文档 " + _tabCounter,
        TextIcon = "\uE723",
        TextIconFontFamily = new FontFamily("Segoe MDL2 Assets"),
        Content = new OrdersPage()
    };
    DocsTab.Items.Add(item);
    DocsTab.SelectedItem = item;
}

private void DocsTab_CloseTab(object sender, TabCloseRoutedEventArgs e)
{
    // 不置 e.Handled ⇒ UI4Tab 自行从 Items 移除（UI4TabControl.cs:603-612）
    if (e.TabItem != null) SetStatus("关闭标签：" + e.TabItem.Header);
}
```

关键事实：

- 属性名是 **`ShowAddButton`**（`:255-265`，默认 `true`），不存在 `ShowAddTab`。
- 事件：`AddTab` 是普通 `RoutedEventHandler`；`CloseTab` 是 `TabCloseRoutedEventHandler`，参数 `TabCloseRoutedEventArgs.TabItem`（`:694-703`）。**不置 `e.Handled` 时控件自己 `Items.RemoveAt`**；要接管删除（比如先确认再删）才置 `e.Handled = true` 并自行移除。
- `IsClosable="False"` 隐藏该项关闭按钮（`:69-79`，模板用可见性转换器绑 `:526`）；`UI4TabItem` 也有 `IsBrand`（`:82-89`）。
- **`UI4Tab` 不跟随主题**：类声明是 `public class UI4Tab : Selector`（`:136`），没有 `IThemeAware`，也没有 `SetResourceReference`。默认值是硬编码的浅色观感：`TabSelectedBackground = Colors.White`（`:169`）、`TabForeground = #C8000000`（`:195`）、`TabSelectedForeground = 不透黑`（`:208`）。⇒ 深色主题下必须像上面的骨架那样逐项绑 `UI4.Color.*`，否则标签条是白底黑字。
- 这些外观 DP 的变更回调是 `OnStyleChanged` ⇒ **每改一次就整体重建 Style**（第 12 章 11.3.7 的代价说明）。别在动画/高频路径上改它们。

### 11.3.4 方案 D：`UI4NavigationView`（应用级左侧主导航）

```xml
<ui:UI4NavigationView x:Name="Nav" Grid.Row="1" Header="我的应用"
                      LeftPanelWidth="220" ItemBackground="Transparent">
  <ui:UI4NavigationViewItem Header="工作台" TextIcon="&#xE80F;" TextIconFontFamily="Segoe MDL2 Assets">
    <pages:WorkbenchPage/>
  </ui:UI4NavigationViewItem>
  <ui:UI4NavigationViewItem Header="订单" TextIcon="&#xE710;" TextIconFontFamily="Segoe MDL2 Assets">
    <pages:OrdersPage/>
  </ui:UI4NavigationViewItem>
  <ui:UI4NavigationViewBottomItem Header="设置" TextIcon="&#xE713;" TextIconFontFamily="Segoe MDL2 Assets">
    <pages:SettingsPage/>
  </ui:UI4NavigationViewBottomItem>
</ui:UI4NavigationView>
```

- **项类型必须匹配集合**：`RegularItems` 是 `ObservableCollection<UI4NavigationViewItem>`，`BottomItems` 是 `ObservableCollection<UI4NavigationViewBottomItem>`（`:681-682`）。往 `BottomItems` 里加普通 `UI4NavigationViewItem` 编译期就不过。
- 两个集合是**派生视图**：`OnItemsChanged` 每次按 `Items` 重建（`:684-696`）。⇒ 加页一律 `nav.Items.Add(...)`，不要直接改 `RegularItems`/`BottomItems`（后者的改动会被下一次 `Items` 变更清空）。
- `SelectedItem` 类型是 `UI4NavigationViewItem`（不是 `object`），默认 `BindsTwoWayByDefault`（`:359-366`）⇒ 可以直接 `{Binding SelectedItem, ElementName=Nav}` 给外部状态栏/命令用。
- `LeftPanelWidth` 默认 `NaN`，同时带 `BindsTwoWayByDefault | AffectsMeasure`（`:161-165`）⇒ 可双向绑到你的展开/折叠状态；改变它会触发布局重算，折叠动画里逐帧改值 = 逐帧重排。
- 内容区绑的是 `SelectedItem.Content`（`:643`）⇒ 页实例就挂在导航项的 `Content` 上，导航项与页一一对应；不要让多个导航项共享同一 `Content` 实例（同一可视元素不能有两个逻辑父）。
- `Header` 可见性走 `NullToVisibilityConverter` / `InverseNullToVisibilityConverter`（`:493/:511`，两个类都是 `public`，宿主可在自己的资源里实例化复用）；`Header` 为空时标题位自动收起。
- 模板必需部件缺失会抛 `InvalidOperationException("Missing template parts.")`（`:719-720`）⇒ 不要给 `UI4NavigationView` 自定义 `Template`。

### 11.3.5 选型决策表

| 需求 | 选 | 理由 |
|---|---|---|
| 应用级左侧主导航（一级页面、可折叠、底部固定「设置」） | `UI4NavigationView` | 唯一自带左栏 + 选中指示 + 底部项的容器；`IThemeAware` 会跟随主题（`:111`、`:664-679`） |
| 页内切换（同屏几组平行视图，大标题风格） | `UI4Pivot` | 标题式页签、引用式跟随主题、内容切换带动画 |
| 可关闭的文档页/多标签工作区 | `UI4Tab` | 唯一有 `AddTab`/`CloseTab`/`IsClosable` 的容器；但要自己接管外观 DP 的主题绑定 |
| 只要「能切页」、零风险 | 原生 `TabControl` | 观感由系统给，不受库的模板重建影响；迁移成本 0 |

经验结论：主导航用 `UI4NavigationView`，业务页内部的分段用 `UI4Pivot`，两者可以叠；`UI4Tab` 只在真需要「关闭文档」语义时引入，并显式绑一整套 `UI4.Color.*`。

### 11.3.6 「加一页」的完整改动清单

以 `UI4NavigationView` 为例，其余容器同构（把第 2 步换成对应容器语法）：

1. 新增 `Pages/ReportsPage.xaml(.cs)`（页文件写法见第 9 章 9.3.4）。
2. 主窗 XAML 里加导航项：`<ui:UI4NavigationViewItem Header="报表" TextIcon="&#xE8F1;" TextIconFontFamily="Segoe MDL2 Assets"><pages:ReportsPage/></ui:UI4NavigationViewItem>`。注意顺序即索引顺序，`--page=N` 的 N 会因此整体后移 —— 若你承诺过参数语义，把新页追加在末尾。
3. 页索引表（第 9 章 9.3.3 的 `PageEntry` 列表）同步追加一行，保持「导航索引 == 页表索引」。
4. 选中联动：`Nav.SelectedItem` 变化时写 `StatusText`；若业务需要「进入页才加载数据」，订阅 `SelectionChanged`（或在 `UI4NavigationViewItem.Content` 里放惰性 `ContentControl`）。
5. 语言联动：导航项文本改为构造后可重建（11.3.8），把新页文本纳入同一份 `RebuildNavigationText()`。
6. 验证：`--page=<新索引>` 能直达；UIA 按新页里某个 `x:Name` 能命中；切深色主题后左栏与内容区一致（11.3.7）。

### 11.3.7 深色主题下的观感结论

| 容器 | 通道 | 深色下的实测观感 | 宿主动作 |
|---|---|---|---|
| 原生 `TabControl` | 不跟随 | 页签与内容底板是系统浅色，窗体深色时形成明显亮块 | 用 `Background="Transparent"` + 自己给 `TabItem` 套样式，或换容器 |
| `UI4Pivot` | 引用式 | 标题文字自动用 `TextForeground`/`Accent`；容器本身背景透明，无亮块 | 不要在 XAML 上覆盖 `ItemForeground` 等（一覆盖就脱离主题） |
| `UI4Tab` | **完全不跟随** | 默认白底选中 + 半透黑文字，深色窗下是刺眼白条 | 逐个绑 `UI4.Color.*`（11.3.3 骨架），并知悉每次改色都会重建 Style |
| `UI4NavigationView` | 命令式（`IThemeAware`） | `Background`/`Foreground` 每次切换重设，左栏/选中底/指示条走 `ThemeSync`（尊重你的显式赋值） | 显式赋值过 `LeftPanelBackground`/`SelectedItemBackground`/`SelectionIndicatorBrush` 的项从此不再跟随 |

### 11.3.8 与 `UI4MultiLanguage` 的关系

本库本地化是**构造期拉取**：文本在对象构造时从 `UI4MultiLanguage.Get(...)` 取一次（内置 zh/ja/ko/de/fr/es/ru + 默认英文，无繁中）。切语言只有 `CultureInfo.CurrentUICulture = new CultureInfo(lang); UI4MultiLanguage.Refresh();` 两步（`MainWindow.xaml.cs:278-286`）——**没有** `SetLanguage(string)`。

对导航的直接后果：导航项的 `Header` 字符串是你自己写死的，`Refresh()` 不会回写它；即使你绑了 `UI4MultiLanguage.Get(...)`，那也是取一次的值。所以：

- 导航文本要在宿主侧集中一份「索引 → 语言键」表，切语言后调 `RebuildNavigationText()` 重新给每个 `UI4NavigationViewItem.Header` / `UI4PivotItem.Header` 赋值（赋的是 CLR 属性，不新建控件就不会丢页实例与滚动位置）。
- `UI4MessageBox`/`UI4ContextMenu` 已弹出的实例不会重译，需要关掉重建。
- 细节与模板见第 23 章，本章不重复。

## 11.4 推荐做法（Do / Don't）

- Do：主导航选 `UI4NavigationView`，页内分段选 `UI4Pivot`；`UI4Tab` 仅用于「可关闭文档」。
- Do：给 `UI4Tab` 一整套 `{DynamicResource UI4.Color.*}` 覆盖默认浅色，或干脆接受它不跟主题并在文档里写明。
- Do：加页一律走 `Items`（或 XAML 子元素），保持导航索引与页表索引一致。
- Do：`SelectedItem` / `LeftPanelWidth` 用双向绑定同步外部状态，它们本来就是 `BindsTwoWayByDefault`。
- Don't：不要直接改 `RegularItems`/`BottomItems`，也不要往 `BottomItems` 塞非 `UI4NavigationViewBottomItem`。
- Don't：不要在容器上还叠一层自己的 `TabControl`（两套选中状态必然漂移）。
- Don't：不要在 `Pivot`/`Tab` 上给外观 DP 写硬编码颜色后又称「它跟主题」；显式本地值即断开跟随。
- Don't：不要给 `UI4NavigationView` 换 `Template`（必需部件缺失直接抛异常）。

## 11.5 坑与排错

| 症状 | 真因 | 处置 |
|---|---|---|
| 深色主题下标签条仍是白底黑字 | `UI4Tab` 不实现 `IThemeAware`，默认值硬编码浅色 | 按 11.3.3 绑 `UI4.Color.*`；或改用 `UI4Pivot` |
| 切主题后某个导航项文字颜色不变 | 你在 XAML 里显式写了 `ItemForeground`/`SelectedItemForeground` 本地值，覆盖了 `SetResourceReference` | 删掉本地值，或改写成 `{DynamicResource UI4.Color.*}` |
| Pivot 放在初始隐藏的容器里，首次显示是空白 | `ApplySelection` 在 `_contentPresenter == null` 时直接返回，模板未应用 | 首次可见后再设 `SelectedIndex`，或在 `Loaded`/`IsVisibleChanged` 里补一次赋值 |
| `CloseTab` 处理完标签还在（或莫名被删两次） | 置了 `e.Handled = true`（控件就不删了），或没置又自己 `Items.Remove` | 二选一：不置 `Handled` 让控件删；置 `Handled` 后自己删（先弹确认再删属于这类） |
| 想加「+」按钮却找不到属性 | 名字是 `ShowAddButton`，且默认已是 `true`；对应事件是 `AddTab` | 用 `ShowAddButton="False"` 关掉，`AddTab` 里自己建项 |
| 左栏宽度改了没反应 / 布局反复抖动 | `LeftPanelWidth` 带 `AffectsMeasure`，且被模板里的 `_leftPanel.Width` 直接消费 | 一次性赋值；折叠动画改 `Visibility`/外层列宽而非逐帧改该 DP |
| 直接给 `BottomItems.Add(new UI4NavigationViewItem(...))` 编译报错 | 集合元素类型是 `UI4NavigationViewBottomItem` | 改用 Bottom 类型，或走 `Items.Add` |
| 加了一个导航项后原 `--page=N` 全错位 | 索引表与导航顺序耦合 | 新页追加到末尾；或把索引改由 `Header`/键字符串解析 |
| 切语言后导航文本没变 | 本地化是构造期拉取，`Refresh()` 不回写宿主文本 | 实现 `RebuildNavigationText()`（11.3.8） |

## 11.6 完成判据

1. 主窗只存在**一个**应用级导航容器；页文件数 = 导航项数（`grep -c "<ui:UI4NavigationViewItem` 与 `Pages/*.xaml` 数一致）。
2. 依次切 Light / Dark / HighContrast，导航条与内容区颜色一致，无残留亮块；若所选容器不跟随主题，已在 XAML 里显式绑 `UI4.Color.*` 并在文档说明。
3. 「加一页」六步清单（11.3.6）能在一次改动内完成：新增页文件 + 一个导航项 + 页表一行，不改导航索引语义。
4. `--page=N` 对每个页索引生效；越界静默落回第一页而不抛异常。
5. 用 UIA 能按 `AutomationId` 命中容器本身与每页至少一个元素。
6. 若用到 `UI4Tab`：`AddTab` 能加、`CloseTab` 能删、`IsClosable="False"` 的项确实没有关闭按钮。
7. 切语言后导航文本按 `RebuildNavigationText()` 刷新，且页实例未被重建（滚动位置仍在）。

---

# 第 12 章 主题引擎的工作方式与生效时序

> 本章解决：把 `UI4Theme` 从「调用」到「屏幕上变色」的完整时序摊开，明确宿主 handler 能读到什么、什么时候读到，以及如何测量一次切换的真实代价。
> 读者：AI Agent（要在本项目之外新建/续写 WPF 业务代码）
> 前置章节：第 13 章（令牌与资源桥的键清单）、第 8 章（启动序列里的播种时机）

## 12.1 目标与验收

1. 能画出并解释这条链：入口调用 → `NotifyThemeChanged` → 三条消费通道 → 标题栏。
2. 写 `ThemeChanged` handler 时知道：**控件已刷完**，但整个批处理是异步的，同步读渲染结果会拿到旧值。
3. 知道 `CurrentMode` / `ResolvedMode` / `ResolvedKey` 三者的差异与自定义键的坑。
4. 有一段可用的 pump 代码与一个可执行的「重建范围」判据。

## 12.2 源码依据

- `src/StartUI4Controls/UI4Theme.cs:69-73` —— 静态构造安装标题栏钩子；`:39-61` —— `_current`/`_requestedMode`/`_resolvedMode`/`_resolvedKey`/`_themeVersion`/`_definitions`/`_themeStack` 六个状态字段。
- `UI4Theme.cs:76-94` —— `Current`、`CurrentMode`（请求模式）、`ResolvedMode`、`ResolvedKey`；`:97` `StaticPropertyChanged`；`:100` `ThemeChanged`（注释即「在全部已追踪控件刷新完成后触发」）。
- `UI4Theme.cs:106-128`（`SetTheme`）、`:141-146`（`ModeForKey`，自定义键 → `Light`）、`:149-158`（`Apply`）、`:161-166`（`Register`）、`:174-187`（`ApplyResolved`：换 `Current` → raise `StaticPropertyChanged` → `NotifyThemeChanged` → 末尾**同步** `WriteToApplicationResources()`）。
- `UI4Theme.cs:200-209` —— `FollowSystemHighContrast`（默认 `false`）；`:230-234` `ResolveSystemKey`；`:236-251` 系统跟随的启用/停用与公开 `ReleaseSystemFollow()`。
- `UI4Theme.cs:283-309` —— `ApplyToApplication` / `WriteToApplicationResources`（`Application.Current == null` 时跳过）/ `WriteTokens`（含 `UI4ThemeScope.RefreshDefinitions()`）。
- `UI4Theme.cs:363-377`（`TrackControl`：幂等 + 挂 `Loaded`）、`:392-415`（`OnTrackedControlLoaded`：补染标题栏 + 按有效主题补齐）、`:418-426`（`RefreshAware`：作用域主题经 `UseTheme` 同步换入后刷新）、`:458-492`（`NotifyThemeChanged`：`_themeVersion++` → 清死引用 → `BeginInvoke(DispatcherPriority.Input)` 内**先 `RefreshEntries` 后 `ThemeChanged?.Invoke`**；`Application.Current == null` 时同步执行但顺序不变）、`:494-508`（`RefreshEntries` 只刷 `IsLoaded` 的控件）。
- `UI4Theme.cs:611-635`（`FromDefinition`/`InstanceOf` 实例缓存）、`:644-660`（internal `UseTheme` + `ThemeRestorer`）、`:666-671`（internal `EffectiveThemeFor`）、`:678-692`（`SetAccent`：派生 `AccentDark`、写回 definition、`NotifyThemeChanged` + 同步写资源）。
- `UI4Theme.cs:728-731` —— `internal interface IThemeAware { void OnThemeChanged(); }`。
- `UI4ThemeScope.cs:103-111`（`ResolveKey` 沿父链找最近已注册键）、`:126-140`（`RefreshDefinitions`）、`:145-181`（`RefreshSubtree`/`RefreshDescendants`：嵌套以最内层为准；根是 `Window` 时补染标题栏）。
- `UI4WindowTitleBar.cs:67-72`（`Install` 订阅 `ThemeChanged → ApplyOpenWindows`）、`:77-86`（控件 `Loaded` 补染，同 `ThemeVersion` 内去重）、`:102-119`（`Apply`/`ApplyOpenWindows`）、`:121-141`（属性 20/19、34/35/36 与哨兵回退）。
- 通道差异实例：`UI4Panel.cs:184-188`、`UI4Button.cs:125-128`、`UI4ComboBox.cs:199-203`、`UI4ListBox.cs:245-249`（重建 `Style`）；`UI4Menu.cs:106-109`、`UI4NavigationView.cs:664-667` + `:671-679`（只重设色，不重建）；`UI4Pivot.cs:176-178`（引用式 `SetResourceReference`）。
- `README.md 第十章 §6` —— 「命令式控件在作用域子树内切换时仍会重建 `Style`」的官方结论与 P2 计划。

## 12.3 正文

### 12.3.1 全景数据流

```
入口（任一）
  SetTheme(mode) :106 ── mode==System ? EnableSystemFollow+ResolveSystemKey : DisableSystemFollow+KeyForMode
  Apply(key)     :149 ── 未注册键直接 return false
  Register(def)  :161 ── 只进 _definitions，并作废该键的实例缓存
  SetAccent(c)   :678 ── 改当前实例令牌 + 写回 definition（派生 AccentDark=0.85×）
  系统事件 UserPreferenceChanged :253 ── 仅 _requestedMode==System 时 ReResolveSystemTheme
        │
        ▼
  ApplyResolved(key, resolved) :174
        ├─ resolvedChanged ? Current = InstanceOf(key) ; raise StaticPropertyChanged(ResolvedMode/ResolvedKey)
        │                   ; NotifyThemeChanged() :458
        └─ 无论如何：WriteToApplicationResources() :186  ← 同步、无批处理
              ├─ WriteTokens(Application.Resources, Current) :297   [通道①]
              └─ UI4ThemeScope.RefreshDefinitions() :126            [通道①′ 作用域字典]
        │
        ▼
  NotifyThemeChanged :458
        ├─ _themeVersion++ ; 清掉 dead / 未 IsLoaded 的弱引用条目
        ├─ Application.Current != null → Dispatcher.BeginInvoke(DispatcherPriority.Input, () => {
        │        RefreshEntries(entries) :494        [通道② 命令式控件，逐个按 EffectiveThemeFor 刷]
        │        ThemeChanged?.Invoke(...) :484      [通道③ 宿主与库内消费者]
        │   })
        └─ 否则同步执行，顺序不变 :489-490
        │
        ├── 通道② 细节：RefreshAware :418 ── 有效主题 == 全局 → aware.OnThemeChanged()
        │                                    ── 作用域主题 → using(UseTheme(effective)) { OnThemeChanged() }
        │        命令式控件：UI4Panel/UI4Button/UI4ComboBox/UI4ListBox ⇒ Style = BuildXxxStyle()（整体重建）
        │                  UI4Menu/UI4NavigationView ⇒ 只重设色（ThemeSync 尊重显式本地值）
        │
        ├── 引用式控件（SetResourceReference）与宿主 DynamicResource：不走通道②，由通道① 的字典覆盖自动跟随
        │
        └── 通道③ 之后：UI4WindowTitleBar 三条自动通路
                 A ThemeChanged → ApplyOpenWindows()（遍历 Application.Windows）
                 B 任一 UI4 控件 Loaded → NotifyContentLoaded → Apply(window)（同 ThemeVersion 去重）
                 C UI4ThemeScope 根为 Window → RefreshSubtree → Apply(scopeWindow) :149-151
                 D 宿主手动 UI4WindowTitleBar.Apply(this)（不含任何 UI4 控件的窗口只能靠它或等下一次 A）
```

### 12.3.2 顺序契约：控件先刷完，事件后到

`RefreshEntries` 与 `ThemeChanged?.Invoke` 在**同一个 `Input` 优先级回调里、按此顺序**执行（`:481-485`）。对你的 handler 意味着：

- 可以：在 handler 里读 `panel.Background`、`combo.EditBackground` 等已被重刷的属性，读到的是新主题值；库自身的「主题后收尾」逻辑（Demo 的 `UpdateScopeStatus`，`MainWindow.xaml.cs:43`）都建立在这一点上。
- 不要：在 handler 里假设「DynamicResource 消费者也刷完了」。通道① 早已在 `SetTheme` 调用栈内同步完成，两条通道之间**没有**先后保证；只有「命令式刷完 → 事件」有保证。
- 事件是**全局单播**：`ThemeChanged` 没有主题参数，handler 必须自己读 `ResolvedKey`/`ResolvedMode` 判断是什么变化。

### 12.3.3 批处理是异步的：必须泵 ContextIdle

`SetTheme` 返回时，`_themeVersion` 已自增、`Application.Resources` 已更新、`Current` 已换 —— 但**控件刷新和事件回调还排在 Dispatcher 队列里**。所以：

- 同步读 `UI4Theme.ResolvedKey`、`UI4Theme.Current.BackgroundColor`、`Application.Current.TryFindResource("UI4.Brush.Background")` ⇒ **新值**。
- 同步读某个已加载 UI4 控件的外观 DP（如 `comboBox.EditBackground`、`listBox.PanelBackground`）或它当前 `Style` 里的画刷 ⇒ **旧值**。

因此任何验证脚本 / 断言代码在切主题后必须先泵一轮，泵优先级必须**低于** `Input`（`ContextIdle` 满足）：

```csharp
UI4Theme.SetTheme(UI4ThemeMode.Dark);

// 把 Dispatcher 排到 ContextIdle：低于 NotifyThemeChanged 用的 Input 优先级，
// 保证批处理（RefreshEntries + ThemeChanged）已经跑完。
// 注意 net48 没有 Dispatcher.Nop 这类静态空委托，必须自己给出 Action。
Application.Current.Dispatcher.Invoke(new Action(delegate { }),
    DispatcherPriority.ContextIdle, null);

// 此时才能安全断言控件的外观 DP 与标题栏状态
```

本仓库的 PowerShell 验证脚本遵循同一约定：先泵 `ContextIdle` 再抓渲染（`事实速查.md` 第 7 节第 4 条）。

### 12.3.4 三个「当前主题」读数怎么选

| 读数 | 语义 | 何时用 |
|---|---|---|
| `CurrentMode`（`:88`） | 最近一次 `SetTheme` **请求**的模式，可能是 `System` | UI 上显示「用户选了跟随系统」；持久化存的就是它（`Save()` 存 `_requestedMode`，`:326`） |
| `ResolvedMode`（`:91`） | 解析后的实际 `Light`/`Dark`/`HighContrast` | 判断深浅、决定图标码位、亮/暗分支 |
| `ResolvedKey`（`:94`） | 生效的主题键字符串（`light`/`dark`/`highcontrast`/自定义键） | **唯一能区分自定义主题的读数** |

坑：`ModeForKey`（`:141-146`）只认 `dark`/`highcontrast`，其余一律 `Light` ⇒ 注册品牌主题 `mybrand` 后 `Apply("mybrand")`，`CurrentMode` 与 `ResolvedMode` 都显示 `Light`。判据：窗内同时打印三者（照 `MainWindow.xaml.cs:248-254`），若 `ResolvedKey=mybrand` 而另两者是 `Light`，你的分支逻辑必须改用 `ResolvedKey`。注意标题栏不受此影响 —— 它按底色亮度判定深浅（`UI4WindowTitleBar.cs:181-185`），不枚举主题键。

### 12.3.5 系统跟随与高对比度

- `FollowSystemHighContrast` 默认 **`false`**（`:200-209`）：处于 `System` 模式时只看注册表 `AppsUseLightTheme` 的亮/暗，**不**跟随系统高对比度。要跟随必须显式置 `true`，且它会立即重解析（仍在 `System` 模式下）。
- `ResolveSystemKey`（`:230-234`）优先高对比度、其次亮暗；`SystemParameters.HighContrast` 是判定源。
- 系统侧变更经 `SystemEvents.UserPreferenceChanged` 回来，只接受 `General`/`Color`/`Accessibility` 三类（`:253-257`），且非 UI 线程回调时 `BeginInvoke` 回 UI 线程（`:260-264`）。
- 退出必须 `ReleaseSystemFollow()`（`:251`，见第 8 章 8.3.5）。

### 12.3.6 `UseTheme` 换入栈的同步限制

作用域内刷新命令式控件时，库通过 internal 的 `UseTheme(effective)`（`:644-660`）把 `Current` 暂时压栈换成作用域主题，`OnThemeChanged()` 返回后即恢复。约束写在注释里（`:57-61`、`:642`）：**只能包裹同步刷新**。

- 后果：若被包裹的代码把活儿 `BeginInvoke` 派出去，派出去那部分执行时栈已恢复，读到的是**全局** `Current`。已知犯例：`UI4ListBox` 序号徽章的数字色（`README.md 第十章 §6`）。
- 宿主侧同样成立：你在 `ThemeChanged` handler 里 `BeginInvoke` 延后做的事，读到的是全局主题；在作用域子树里想做作用域正确的事，只能在 handler 内同步完成，或直接读元素自身的解析值。
- `IThemeAware`（`:728-731`）与 `UseTheme`/`EffectiveThemeFor`/`ThemeVersion`/`TrackControl` 都是 `internal` ⇒ **宿主自定义控件无法挂上命令式通道**。替代方案是引用式：构造里对自己要跟随的主题 DP 调 `SetResourceReference`，只消费 `UI4.Color.*` / `UI4.Brush.*` 键。

宿主自定义控件模板（只走 DynamicResource/引用通道，C# 7.3）：

```csharp
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MyApp.Controls
{
    public class MyCard : ContentControl
    {
        public static readonly DependencyProperty SurfaceBrushProperty =
            DependencyProperty.Register("SurfaceBrush", typeof(Brush), typeof(MyCard));

        public Brush SurfaceBrush
        {
            get { return (Brush)GetValue(SurfaceBrushProperty); }
            set { SetValue(SurfaceBrushProperty, value); }
        }

        public MyCard()
        {
            // 引用通道：不写本地值，作用域/全局切换都会自动跟随
            SetResourceReference(SurfaceBrushProperty, "UI4.Brush.Surface");
            SetResourceReference(TextBlock.ForegroundProperty, "UI4.Brush.Text");
        }
    }
}
```

对应 XAML 侧用 `{DynamicResource UI4.Brush.Surface}` 效果等价；区别是 `SetResourceReference` 写在构造函数里，任何使用该控件的地方都自动获得跟随，而 `DynamicResource` 要每处都写。

### 12.3.7 命令式控件的代价：Style 整体重建

`OnThemeChanged` 的两种写法决定代价量级：

| 写法 | 代表 | 每次切换的代价 |
|---|---|---|
| `Style = BuildXxxStyle()` | `UI4Panel.cs:184-188`、`UI4Button.cs:125-128`、`UI4ComboBox.cs:199-203`、`UI4ListBox.cs:245-249` | 模板重建：视觉树重解析、动画/选中态/滚动位置等运行期状态被重置 |
| 只重设色 | `UI4Menu.cs:106-109`、`UI4NavigationView.cs:664-679` | 无模板重建，仅若干 DP 赋值 |

叠加两个放大因素：① 这些外观 DP 的变更回调本身也走重建（`OnStyleChanged`/`OnStyleUpdate`）⇒ 一次改多个 DP = 多次重建；② **在 `UI4ThemeScope` 子树内**切全局主题时，作用域内每个命令式控件都要按作用域主题重刷一遍（`README.md 第十章 §6`）。

由此两条纪律：不要逐帧改命令式控件的外观 DP（动画频率级别改 DP 必掉帧）；把长列表、大表单放进引用式控件或宿主自己的 `DynamicResource` 路径，避开重建。

### 12.3.8 持久化事件

`Persistence`（`:316`，默认 null）配好后：`Save()` 写存储并 raise `ThemeSaved`（`:324-328`）；`ApplyPersisted()` 读到值才 raise `ThemeLoading` 然后 `SetTheme`，无配置或无已存值返回 `false`（`:331-339`）。把 `ThemeSaved` 当作「已落盘」的确认信号，把 `ThemeLoading` 当作「首帧前的最后一次改主题机会」，避免自己的 handler 在 `ThemeLoading` 里又触发一次 `Save()`（会递归回写，虽不死循环但会重复 I/O）。

### 12.3.9 测量一次切换：耗时与重建范围

耗时（同步段 vs 批处理段要分开看）：

```csharp
var sw = System.Diagnostics.Stopwatch.StartNew();
UI4Theme.SetTheme(UI4ThemeMode.Dark);
long syncMs = sw.ElapsedMilliseconds;                      // 只含状态改写 + 资源字典覆盖
Application.Current.Dispatcher.Invoke(new Action(delegate { }),
    DispatcherPriority.ContextIdle, null);
sw.Stop();
StatusText.Text = string.Format("主题：同步 {0}ms / 批处理+同步 共 {1}ms", syncMs, sw.ElapsedMilliseconds);
```

判据：`syncMs` 接近 0 是正常的（`WriteTokens` 只写 63 个键）；大头必然在第二段（`RefreshEntries` + 事件 handler）。若第二段随控件数量线性增长，说明重建在起主导作用。

重建范围（可执行、不需要 internal 成员）—— 比较切换前后各控件 `Style` 的**引用同一性**：

```csharp
private static IEnumerable<FrameworkElement> Walk(DependencyObject root)
{
    FrameworkElement self = root as FrameworkElement;
    if (self != null) yield return self;
    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
        foreach (FrameworkElement child in Walk(VisualTreeHelper.GetChild(root, i)))
            yield return child;
}

private void MeasureRebuild()
{
    var captured = new List<KeyValuePair<FrameworkElement, Style>>();
    foreach (FrameworkElement fe in Walk(this))
        if (fe.Style != null) captured.Add(new KeyValuePair<FrameworkElement, Style>(fe, fe.Style));

    UI4Theme.SetTheme(UI4ThemeMode.Dark);
    Application.Current.Dispatcher.Invoke(new Action(delegate { }), DispatcherPriority.ContextIdle, null);

    int rebuilt = 0;
    foreach (KeyValuePair<FrameworkElement, Style> kv in captured)
        if (!ReferenceEquals(kv.Value, kv.Key.Style)) rebuilt++;
    StatusText.Text = string.Format("Style 重建 {0} / {1}", rebuilt, captured.Count);
}
```

读数解释：`rebuilt` 就是命令式通道这次真正重建了模板的元素数；`captured.Count - rebuilt` 是靠引用通道（或本就不跟随主题）的元素。想确认「某控件是否走了重建路径」，看它 `Style` 的引用是否变了即可，不必读任何 internal 成员。回归基线由 `p2verify.ps1` 承担 —— 它离屏实例化控件、按主题断言解析后的画刷色值，并**确认 Style 未被重建**（`事实速查.md` 第 8 节）。

## 12.4 推荐做法（Do / Don't）

- Do：`ThemeChanged` handler 里假定「命令式控件已刷完」，只做读值与业务回显，不再 `BeginInvoke` 延后。
- Do：任何断言/截图前泵一次 `ContextIdle`；把 pump 写成公共辅助函数，别在每处复制。
- Do：状态回显同时打印 `ResolvedMode` 与 `ResolvedKey`，让自定义主题一眼可辨。
- Do：宿主自定义控件一律走 `SetResourceReference` / `DynamicResource`，并接受「拿不到命令式通道」这一事实。
- Do：给长列表、大表单选引用式控件；把命令式控件数量控制在导航条、按钮这类小面积元素上。
- Don't：不要在同步代码里读「控件解析后的色值」来验证切换是否生效。
- Don't：不要逐帧改命令式控件外观 DP 做动画；不要同时改多个（每次都重建）。
- Don't：不要在被主题管理的 DP 上赋本地值（`ReadLocalValue` 有值即失去跟随）。
- Don't：不要靠 `Environment.OSVersion` 判断能不能染标题栏（无 manifest 的进程会虚报 6.3，见第 7 章）。

## 12.5 坑与排错

| 症状 | 真因 | 处置 |
|---|---|---|
| 切主题后立刻读控件颜色还是旧值 | 批处理在 `Input` 优先级队列里，尚未执行（`:481`） | 泵 `ContextIdle`（12.3.3）后再读 |
| `ThemeChanged` 里 `BeginInvoke` 出去的代码在作用域窗口读到全局色 | `UseTheme` 栈在同步刷新结束时已恢复（`:57-61`、`:642`） | 改同步执行，或改读元素自身解析值 |
| 自定义主题下深浅判断全错 | `CurrentMode`/`ResolvedMode` 对自定义键恒报 `Light`（`:141-146`） | 比 `ResolvedKey`；深浅按底色亮度自算（照 `UI4WindowTitleBar.cs:181-185`） |
| `Apply("mytheme")` 毫无效果、返回 false | 键未注册（`:151`） | 先 `UI4Theme.Register(new UI4ThemeDefinition("mytheme")…)`；大小写不敏感但仍要逐字一致 |
| 系统开了高对比度，应用仍走亮/暗 | `FollowSystemHighContrast` 默认 false（`:200`） | 置 `true`（且需处于 `System` 模式）；或直接用 `Apply("highcontrast")` |
| 大量控件在切主题后闪烁/丢选中/丢滚动位置 | 命令式控件 `OnThemeChanged` 重建 `Style`（12.3.7） | 用 `MeasureRebuild` 定位后换成引用式；或减少同屏命令式控件 |
| `SetTheme` 连续调用两次没反应 | `ApplyResolved` 短路：键未变则不通知（`:176`） | 若确实要重刷（例如刚改过令牌），改用 `SetAccent` 或 `Register`+`Apply` |
| 换了 `SetAccent` 之后，`Apply("light")` 结果也变了 | `SetAccent` 写回已注册 definition（`:686-688`） | 测试里恢复或隔离；跨用例不要共享默认 definition |
| 没引用任何 UI4 控件的窗口标题栏不变深 | 只有通路 A/D 覆盖它（`UI4WindowTitleBar.cs:18-21`） | 在 `SourceInitialized`/`Shown` 里 `UI4WindowTitleBar.Apply(this)` |

## 12.6 完成判据

1. 能在纸面/注释里复现 12.3.1 的图，并说清「哪条通道是同步的、哪条是异步批处理」。
2. 全工程搜 `Dispatcher.Invoke`：出现在断言前的那一处用的是 `ContextIdle`，不是 `Background`/`Normal`。
3. 状态回显同时含 `ResolvedMode` 与 `ResolvedKey`，自定义主题下能看出 `CurrentMode` 报 `Light`。
4. `MeasureRebuild()` 在你的应用上跑通并输出一行数字；已解释「为什么某些控件不计入重建」。
5. 宿主自定义控件（若有）里能看到 `SetResourceReference` 或 `DynamicResource`，且没有任何对 `IThemeAware`/`TrackControl`/`UseTheme` 的引用（它们是 internal，编译不过）。
6. 同屏命令式控件（重建型）数量已清点，且已知它们落在哪些页；动画/高频路径上没有外观 DP 的连续赋值。
7. 用过 `System` 模式时 `FollowSystemHighContrast` 的取值有明确决策记录；`ReleaseSystemFollow()` 在退出路径上。

---

# 第 13 章 设计令牌与资源桥

> 本章解决：把 `UI4Theme` 的 30 个语义令牌与 `UI4.Color.*` / `UI4.Brush.*` 资源桥正确接进新应用，并确定令牌写到哪一层、怎么验收完整度。
> 读者：AI Agent（要在本项目之外新建/续写 WPF 业务代码）
> 前置章节：第 12 章（主题引擎总览与切换时序）；概览背景见 `README.md 第十章`，设计取舍与实施记录见 `主题方案分析与改进.md` 第七节（P1 资源桥）/第九节（P3 作用域与高对比度）。

## 13.1 目标与验收

- 说清 30 个 `UI4ThemeToken` 的语义与三主题（Light / Dark / HighContrast）取值，据此挑对令牌。
- 掌握资源键规则（`UI4.Color.<Token>` 是 `Color`、`UI4.Brush.<Token>` 是 `SolidColorBrush`、三个别名）与实例 API（`ColorOf` / `BrushOf`、`*Color`、11 个冻结画刷）。
- 能解释令牌写进哪一层资源表、查找优先级如何覆盖，并坚持 `DynamicResource` 纪律。
- 给自定义主题跑「令牌完整度」检查（highcontrast 是既有可验收项，30 项齐全）。
- 验收判据：改一次全局主题，界面所有语义色同帧翻转；`p3verify.ps1` 的 `F token count` 断言思路（枚举计数 == 30）在宿主自查中复现通过。

## 13.2 源码依据

- `src/StartUI4Controls/UI4ThemeToken.cs:7-39` — 30 个枚举成员，顺序即令牌全集。
- `src/StartUI4Controls/UI4ThemeDefinition.cs:94-127`（`Light()`）、`:130-163`（`Dark()`）、`:170-203`（`HighContrast()`）— 逐令牌真值。
- `src/StartUI4Controls/UI4Theme.cs:294-309` — `WriteTokens`：资源键集合与三个别名。
- `src/StartUI4Controls/UI4Theme.cs:283` — `ApplyToApplication()`；`:285-291` — 写 `Application.Resources` 并同步各作用域字典。
- `src/StartUI4Controls/UI4Theme.cs:518-521` — `ColorOf(token)` / `BrushOf(token)`。
- `src/StartUI4Controls/UI4Theme.cs:526-584` — 实例 30 个 `*Color` 属性；`:589-609` — 11 个冻结画刷；`:694-707` — `BuildFrozenBrushes`。
- `src/StartUI4Controls/UI4Theme.cs:611-619` — `FromDefinition` 逐令牌 `GetColor`（缺项即抛）。
- `src/StartUI4Controls/UI4ThemeScope.cs:80-84` — 作用域字典写入位置（合并字典末位）。
- `p3verify.ps1:143`（`D highcontrast covers all tokens`）、`:170-171`（`F token count` = 30）。

## 13.3 正文

### 13.3.1 30 令牌全表（真值取自 `UI4ThemeDefinition`）

六位为 `#RRGGBB`，八位为带 alpha 的 `#AARRGGBB`。HighContrast 列同时是「是否定义」列：30 项在 `HighContrast()`（`UI4ThemeDefinition.cs:172-202`）中全部定义，无缺项。

| 枚举名 | 语义用途 | Light | Dark | HighContrast |
|---|---|---|---|---|
| Accent | 主强调色（焦点/选中/开关开） | #0078D4 | #0099FF | #FFFF00 |
| AccentDark | 深强调（悬停/按下派生） | #0066B5 | #0078D4 | #DDDD00 |
| AccentEnd | 渐变尾色 | #9333EA | #6428C8 | #FFFF00 |
| TextForeground | 主文本 | #1E1E1E | #E6E6E6 | #FFFFFF |
| TextSecondary | 次要文本 | #000000 | #CCCCCC | #FFFFFF |
| Background | 窗口/面板底 | #FFFFFF | #202026 | #000000 |
| Surface | 控件表面（卡片/编辑区） | #FFFFFF | #282830 | #000000 |
| BorderNormal | 默认边框 | #C8C8DC | #3C3C4B | #FFFFFF |
| BorderSecondary | 次要边框（Radio 框） | #B4B4C8 | #36364A | #FFFFFF |
| BorderHover | 悬停边框 | #0078D4 | #0099FF | #FFFF00 |
| BorderFocus | 焦点边框 | #0066B5 | #0078D4 | #FFFF00 |
| Placeholder | 占位符 | #D3D3D3 | #808080 | #CCCCCC |
| HoverOverlay | 悬停叠加（半透明） | #14000000 | #14FFFFFF | #33FFFF00 |
| SelectedOverlay | 选中叠加（半透明） | #0A000000 | #0AFFFFFF | #4DFFFF00 |
| TrackBackground | 轨道底（滑条/进度条） | #0A000000 | #14FFFFFF | #33FFFFFF |
| CheckBackground | 勾选/选中填充 | #0066B5 | #008CD2 | #00008B |
| Icon | 图标 | #78788C | #A0A0B4 | #FFFFFF |
| IconHover | 图标悬停 | #3C3C50 | #C8C8DC | #FFFF00 |
| PanelBorder | 面板边框（半透明） | #3C788CC8 | #3C6478B4 | #FFFFFF |
| OffBackground | 开关关闭态底 | #C8C8D2 | #3C3C46 | #3A3A3A |
| MenuBackground | 菜单栏底 | #F8F8F8 | #2D2D32 | #000000 |
| ListSelected | 列表选中/按下 | #2563EB | #3B7BFF | #00008B |
| HeaderBackground | 表头底 | #F5F5F5 | #2A2A30 | #101010 |
| HeaderForeground | 表头前景 | #1E1E1E | #E6E6E6 | #FFFFFF |
| RowHoverBackground | 行悬停底 | #F0F0F5 | #2E2E38 | #33FFFFFF |
| RowSelectedBackground | 行选中底 | #D3D3D3 | #3A3A48 | #00008B |
| GridLine | 网格线 | #E6E6EB | #3A3A45 | #FFFFFF |
| ProgressStart | 进度渐变起色 | #0096E6 | #00AAFF | #FFFF00 |
| CheckBoxUnchecked | 复选框未勾选底 | #D3D3D3 | #505058 | #000000 |
| HoverBorderColorLight | 悬停边框微调色 | #8C8CAA | #606078 | #FFFF00 |

读表纪律：高对比度用黑底白字白框黄强调；`CheckBackground` / `ListSelected` / `RowSelectedBackground` 刻意取深蓝 `#00008B`，保证压在其上的白色对勾/文字仍可读——不要为「好看」改动这三项。

### 13.3.2 资源键规则

`UI4Theme.WriteTokens`（`UI4Theme.cs:297-309`）对每个令牌写两个键，外加三个别名：

- `UI4.Color.<Token>` → `Color` 值类型，给 `Color` 型 DP 用（如控件的 `TextColor`、`GradientStart`）。
- `UI4.Brush.<Token>` → 冻结 `SolidColorBrush`，给 `Brush` 型属性用（`Background`、`Foreground`、`BorderBrush`）。
- 别名：`UI4.Brush.Text`（=`TextForegroundBrush`）、`UI4.Brush.Border`（=`BorderNormalBrush`）、`UI4.Brush.Accent`（=`AccentBrush`）。

`<Token>` 与枚举成员名逐字符一致（`BorderNormal` 不是 `Bordernormal`）；切换主题时同名键被**原地覆盖**，键不变、值换。

### 13.3.3 实例 API 与何时用

一个 `UI4Theme` 实例（`UI4Theme.Current` 或作用域解析出的实例）暴露三套读取口：

- `ColorOf(UI4ThemeToken)`（`:518`）/ `BrushOf(UI4ThemeToken)`（`:521`）。注意 `BrushOf` 内部每次 `CreateFrozen` 新建画刷，与缓存的 `*Brush` 属性不满足引用相等，属预期。
- 30 个 `*Color` 只读属性（`:526-584`），命名不规则处要背下来：`CheckBoxUncheckedBackground`（不是 `...Color`）、`HoverBorderColorLight`（无后缀）。
- 11 个冻结画刷属性（`:589-609`）：`AccentBrush`、`AccentDarkBrush`、`BackgroundBrush`、`SurfaceBrush`、`TextForegroundBrush`、`OnWhiteBrush`、`PlaceholderBrush`、`BorderNormalBrush`、`MenuBackgroundBrush`、`HoverOverlayBrush`、`IconBrush`。`OnWhiteBrush` 恒为白色，不是令牌派生。

何时用实例 API：只在**代码里取色**（算亮度、写日志、合成非 XAML 资源如画 `DrawingBrush` / `GeometryDrawing` / 位图）时使用。它们是快照，不跟随后续主题变更；界面外观一律回到 `DynamicResource` 通道。

```csharp
// C# 7.3：代码里按令牌构造一个非 XAML 的渐变画刷
Color c1 = UI4Theme.Current.ColorOf(UI4ThemeToken.Accent);
Color c2 = UI4Theme.Current.ColorOf(UI4ThemeToken.AccentEnd);
var grad = new LinearGradientBrush(c1, c2, 90d);
```

### 13.3.4 令牌写到哪一层与查找优先级

- 全局主题变更：`WriteToApplicationResources`（`:285-291`）把令牌写进 `Application.Resources`，随后 `UI4ThemeScope.RefreshDefinitions` 按各自键同步所有已登记作用域的字典。
- 作用域变更：字典追加到该元素 `Resources.MergedDictionaries` **末位**（`UI4ThemeScope.cs:81-83`），末位优先于靠前的合并字典。
- 查找优先级（标准 WPF 规则）：元素自身 `Resources` 的显式项 > 自身合并字典（越靠末位越优先）> 祖先元素 > `Application.Resources`。因此 `element.Resources["UI4.Color.Accent"] = 某色` 可以局部覆盖作用域/全局令牌，宿主覆盖保护测试（`p3verify.ps1:83`）就是利用这一层。
- 新应用启动必须调一次 `UI4Theme.ApplyToApplication()`（`samples/StartUI4Demo/App.xaml.cs:17` 即此约定），否则解析期没有 `UI4.*` 键，`DynamicResource` 首帧为空。

### 13.3.5 DynamicResource 纪律

XAML 里主题相关颜色/画刷必须写 `DynamicResource`：

```xml
<!-- 前缀约定：xmlns:ui="clr-namespace:StartUI4Controls;assembly=StartUI4Controls" -->
<Border Background="{DynamicResource UI4.Brush.Surface}"
        BorderBrush="{DynamicResource UI4.Brush.BorderNormal}">
    <TextBlock Text="状态栏" Foreground="{DynamicResource UI4.Brush.Text}" />
</Border>
```

反例（禁止）：`{StaticResource UI4.Brush.Text}`。`StaticResource` 在 XAML 解析期快照当值，之后不再查表，切主题不更新。判据：把一处引用改成 `StaticResource` 后切 `dark`，那一处不变色即复现。代码等价物是构造函数里 `SetResourceReference(DP, "UI4.Brush.X")`——它和 `DynamicResource` 同属延迟解析，允许使用。

### 13.3.6 令牌完整度是可验收项

`FromDefinition`（`:611-619`）对全部枚举逐个 `def.GetColor(token)`，definition 少任何一项都会在 `Apply` / 作用域解析时抛 `KeyNotFoundException`。内置 `highcontrast` 是已验收项：`p3verify.ps1:143` 断言缺失数为 0，`:170-171` 断言枚举总数为 30。宿主注册自定义主题前跑同样守卫：

```csharp
// C# 7.3
static bool TokensComplete(UI4ThemeDefinition def)
{
    foreach (UI4ThemeToken t in Enum.GetValues(typeof(UI4ThemeToken)))
        if (!def.Has(t)) return false;   // Has 不抛；GetColor 缺项抛 KeyNotFoundException
    return true;
}
```

### 13.3.7 新增业务语义色的命名规则

不要自造第四十一个 `UI4.*` 键——资源桥键集合由 30 个枚举驱动，你写的任何 `UI4.` 前缀自定义键都会在下次主题写入时成为无人维护的孤儿。正确做法：把品牌语义映射到既有令牌，在宿主 `App.xaml` 建自己的语义别名层（前缀用 `App.`）：

```xml
<SolidColorBrush x:Key="App.Brush.PrimaryText" Color="{DynamicResource UI4.Color.TextForeground}" />
<SolidColorBrush x:Key="App.Brush.Link"        Color="{DynamicResource UI4.Color.Accent}" />
```

页面只引用 `App.Brush.*`；将来换品牌来源只改这一层。若某语义确实无对应令牌（如「危险红」），把它定义为宿主自己的固定画刷并显式写进两套深浅（用第 16 章的亮度公式校验），不要塞进 `UI4.*` 命名空间。

## 13.4 推荐做法（Do / Don't）

- Do：界面外观一律 `DynamicResource UI4.Brush.*` / `UI4.Color.*`；`Color` 型 DP 用 `UI4.Color.*`，`Brush` 型属性用 `UI4.Brush.*`。
- Do：应用启动即 `UI4Theme.ApplyToApplication();`，再建第一个窗口。
- Do：给页面引用语义别名层（`App.Brush.*`），令牌来源集中一处。
- Don't：对 `UI4.*` 键使用 `StaticResource`（解析期快照，切主题不更新）。
- Don't：修改 `UI4Theme.Current.AccentBrush.Color`——冻结画刷不可变，改了也不驱动资源桥；改强调色用 `UI4Theme.SetAccent`（副作用见第 15 章）。
- Don't：在业务代码硬编码 `#0078D4` 等具体值；Don't：新增 `UI4.` 前缀的自定义资源键。

## 13.5 坑与排错

- 症状：切主题后某处颜色不变。根因：该处用了 `StaticResource`，或代码取的是快照。处置：换成 `DynamicResource`；快照仅允许出现在非外观路径（判据：13.3.5）。
- 症状：`Color="{DynamicResource UI4.Brush.Text}"` 报类型错误或无色。根因：`Color` 属性只能吃 `Color` 型资源。处置：改用 `UI4.Color.TextForeground`。
- 症状：`Apply("自定义键")` 或设作用域时抛 `KeyNotFoundException`。根因：definition 令牌没给全。处置：用 `Clone()` 起手 + `TokensComplete` 守卫（13.3.6）。
- 症状：首帧控件无色/模板里 `DynamicResource` 解析为空。根因：忘了调 `ApplyToApplication()`，`Application.Resources` 未播种。处置：在 App 启动序列最前补上（对照 `samples/StartUI4Demo/App.xaml.cs:17`）。
- 症状：以为 `BrushOf` 与资源桥共享画刷实例，做引用相等判断失败。根因：`BrushOf` 每次新建冻结画刷（`:521`）；11 个 `*Brush` 属性才是缓存单例。属预期，勿依赖 `ReferenceEquals`。
- 症状：元素自身覆盖了一个令牌色后切主题「不生效」。根因：元素 `Resources` 显式项优先级高于全局与作用域字典（13.3.4），这是设计行为。处置：删掉覆盖项或改为覆盖 `Brush` 属性本身。

## 13.6 完成判据

1. 全局搜索宿主 XAML：`UI4.` 键无一处 `StaticResource` 前缀。
2. App 启动序列存在 `UI4Theme.ApplyToApplication();` 且先于首个窗口创建。
3. `App.xaml` 存在语义别名层，抽查 3 个页面均引用 `App.Brush.*` 或直接令牌，无硬编码色值。
4. 调 `UI4Theme.SetTheme(UI4ThemeMode.Dark)` 后，别名层与所有令牌消费者同帧翻转（进程内断言：`((SolidColorBrush)app.Resources["UI4.Brush.Background"]).Color.ToString()` 等于 `#202026`）。
5. 注册任何自定义 definition 前 `TokensComplete(def) == true`；对内置 `HighContrast()` 复跑同守卫应为 30 项齐全（等价 `p3verify.ps1` 的 D 组与 `F token count` 断言）。
6. 任取一处深底配文，用 `0.299R+0.587G+0.114B` 校验前景/背景亮度差 ≥ 60（与 `UI4WindowTitleBar.cs:181-185` 的深浅判定同式）。

---

# 第 14 章 局部作用域主题（UI4ThemeScope）

> 本章解决：在新应用里给一张卡片、一个窗口或一个预览面板挂上独立主题，且知道代价在哪、怎么验收。
> 读者：AI Agent（要在本项目之外新建/续写 WPF 业务代码）
> 前置章节：第 13 章（令牌与资源桥）、第 12 章（主题引擎时序）；背景见 `README.md 第十章` 与 `主题方案分析与改进.md` 第九节（P3 记录）。

## 14.1 目标与验收

- 会用附加属性 `ui:UI4ThemeScope.Theme` 在任意 `FrameworkElement` 上开/撤一个主题作用域。
- 能解释两条生效通道、撤销语义、嵌套解析链与「最内层优先」。
- 知道命令式控件在作用域下的重建代价与 `UseTheme` 同步路径限制（BeginInvoke 逃逸）。
- 能复刻 Demo 的 `ScopeWindow`，并给出不启脚本、进程内读回解析色的最小验收代码。
- 验收判据：作用域卡片内所有控件（含 `UI4ComboBox`、`UI4ListBox`）按作用域主题解析；撤销后整卡回到全局主题且样式不被无谓重建（对应 `p3verify.ps1` 的 C/E 组）。

## 14.2 源码依据

- `src/StartUI4Controls/UI4ThemeScope.cs:24-26` — `ThemeProperty` 附加属性（`string`，默认 null）；`:29-40` — `SetTheme` / `GetTheme`。
- `src/StartUI4Controls/UI4ThemeScope.cs:58-86` — `OnThemePropertyChanged`：撤销语义（`:63-65`）、字典写入（`:81-83`）。
- `src/StartUI4Controls/UI4ThemeScope.cs:103-120` — `ResolveKey` 与父链上溯（`NextAncestor`）。
- `src/StartUI4Controls/UI4ThemeScope.cs:145-181` — `RefreshSubtree` / `RefreshDescendants`：作用域根为 `Window` 时重染标题栏（`:149-151`）、嵌套以最内层为准（`:172-174`）。
- `src/StartUI4Controls/UI4Theme.cs:57-61,644-660` — `UseTheme` 换入栈（仅同步路径）；`:417-439` — `RefreshAware` / `RefreshControl`。
- `src/StartUI4Controls/UI4ListBox.cs:338-342` — 控件内 `Dispatcher.BeginInvoke` 派活实例；`:403` — 模板构建期读 `UI4Theme.Current` 快照。
- `samples/StartUI4Demo/ScopeWindow.xaml`（全文 47 行）/ `ScopeWindow.xaml.cs:15-32,34-61`。
- `samples/StartUI4Demo/MainWindow.xaml:472-547`、`MainWindow.xaml.cs:218-227,248-254`。

## 14.3 正文

### 14.3.1 用法与可挂宿主

XAML（前缀约定 `xmlns:ui="clr-namespace:StartUI4Controls;assembly=StartUI4Controls"`，键大小写不敏感）：

```xml
<Border ui:UI4ThemeScope.Theme="dark"
        Background="{DynamicResource UI4.Brush.Surface}"
        BorderBrush="{DynamicResource UI4.Brush.BorderNormal}">
    <StackPanel TextElement.Foreground="{DynamicResource UI4.Brush.Text}">
        <!-- 这棵子树全部改用 dark -->
    </StackPanel>
</Border>
```

C#：`UI4ThemeScope.SetTheme(element, "dark");` 读取用 `UI4ThemeScope.GetTheme(element)`（返回元素**自身**声明的键，不含祖先作用域）。可挂任何 `FrameworkElement`——Window、卡片 Border、UserControl 都行；变更回调先把宿主强转 `FrameworkElement`，非 FE 宿主（纯 `FrameworkContentElement`）设了也不会建字典。

三种典型用途：
1. 整窗异主题（`ScopeWindow`：主窗 dark 时此窗 light，互不干扰）。
2. 卡片反衬（设置页里一张始终深色的「效果预览」卡）。
3. 预览面板（品牌主题上线前在局部先看效果，不碰全局）。

### 14.3.2 两条生效通道

控件无需任何改动，靠两条通道跟随（`UI4ThemeScope.cs:13-19` 注释即此契约）：

- 通道①（引用式）：向元素 `Resources.MergedDictionaries` **末位**插入该主题的令牌字典（`UI4.Color.*` / `UI4.Brush.*`），所有 `DynamicResource` 与构造期 `SetResourceReference` 的消费者（UI4CheckBox/UI4Radio/UI4TextBox/UI4Switch/UI4Slider/UI4ProgressBar 等）自动重解析。
- 通道②（命令式）：`RefreshSubtree` 遍历子树，对 `IThemeAware` 控件（UI4Button/UI4ComboBox/UI4Menu/UI4ListBox…）经 `UseTheme` 把作用域主题**同步换入 `UI4Theme.Current`** 后调 `OnThemeChanged()` 重建样式（`UI4Theme.cs:417-426`）。

### 14.3.3 撤销语义

写 `null`、空白串（会被 `Trim()`）、或**未注册的键**，一律按撤销处理：移除该元素此前登记的字典，子树回全局主题，**不抛异常**（`UI4ThemeScope.cs:63-77`；XAML 里打错键名不会让宿主崩，代价只是「没有作用域」）。撤销刷新只针对「向上已找不到任何作用域」的控件，外层/嵌套作用域各自保持不变。判据：`p3verify.ps1:154` 的 `E unknown key falls back to global`。

### 14.3.4 嵌套解析链与最内层优先

解析某元素的有效键从**元素自身**起逐级上溯（`ResolveKey` :103-111）：先 `FrameworkElement.Parent`，再 `FrameworkContentElement.Parent`，最后 `VisualTreeHelper.GetParent` 兜底——这条链能穿过 Popup 与控件模板。第一个「已注册且非空」的键即生效，所以**最内层作用域优先**。外层 `highcontrast` 套内层 `dark` 时，内层子树按 dark 解析（Demo `MainWindow.xaml:533-546` 就是这个嵌套样本）；外层撤销不会波及内层。

### 14.3.5 作用域根为 Window 时标题栏跟随

`RefreshSubtree` 若发现作用域根是 `Window`，直接调 `UI4WindowTitleBar.Apply(scopeWindow)`（`UI4ThemeScope.cs:149-151`）——非客户区（标题栏）不在 WPF 树里，只能走这条专门通道（细节见第 17 章）。

### 14.3.6 已知代价与限制

- 命令式控件在作用域变更时被**重建 Style**（模板全量重走 `FrameworkElementFactory`）：切换开销大，别把 `SetTheme` 放在逐帧动画里。引用式控件只重解析资源，样式不动（`p3verify.ps1:160` 的 `E G1 style not rebuilt` 断言的就是这点）。
- `UseTheme` 换入栈**只在 UI 线程同步路径内有效**（`UI4Theme.cs:57-61` 注释、`:644-660`）。命令式刷新代码若把活儿用 `BeginInvoke` 派给 Dispatcher，被派出去的回调运行时 `Current` 已恢复为全局主题，会读到错的色。真实犯例：`UI4ListBox` 序号徽章——模板构建期直接取 `UI4Theme.Current.OnWhiteBrush` 快照（`UI4ListBox.cs:403`），`NumberCircleBackground` 的 DP 默认值也是静态构建（`:205-210`），叠加该控件内部存在 `Dispatcher.BeginInvoke` 的延迟处理路径（`:338-342`），作用域下徽章颜色可能落在换入窗口之外拿到全局主题。规避：作用域内列表需要异色徽章时，显式给 `NumberCircleBackground` 赋 `DynamicResource UI4.Brush.Accent`（宿主显式赋值优先于主题，见通用坑 §5）。
- 作用域不改变 `UI4Theme.ResolvedKey` 等全局静态读数；想知道某元素生效主题，用 `UI4ThemeScope.GetTheme` 只拿自身声明，拿「有效值」要在宿主侧沿树自行上溯或进程内读回解析色（14.3.8）。

### 14.3.7 Demo 完整做法：ScopeWindow

`ScopeWindow.xaml.cs:15-32`：构造函数里 `InitializeComponent()` 后取 `_scopeKey = OppositeOfGlobal();` 再 `UI4ThemeScope.SetTheme(this, _scopeKey);`。`OppositeOfGlobal()` 按全局键给反色：

```csharp
// C# 7.3（与 ScopeWindow.xaml.cs:27-32 一致）
private static string OppositeOfGlobal()
{
    string key = UI4Theme.ResolvedKey;
    if (string.Equals(key, "dark", StringComparison.OrdinalIgnoreCase)) return "light";
    return "dark";
}
```

回显：构造里订阅 `UI4Theme.ThemeChanged += delegate { UpdateInfo(); };`——主窗切全局主题时本窗作用域**保持不变**，只刷新说明文字（`ScopeInfo` 显示自身作用域键，`GlobalInfo` 显示全局 `ResolvedMode/ResolvedKey`，`:56-61`）。三个按钮分别演示：改用相反主题（重设键）、撤销（`SetTheme(this, string.Empty)`）、高对比度（`SetTheme(this, "highcontrast")`）。主窗侧样本：`MainWindow.xaml:472` 卡片默认 `Theme="dark"`，下拉框经 `MainWindow.xaml.cs:218-227` 调 `UI4ThemeScope.SetTheme(ScopeCard, key)`（注意该处对 XAML 解析期早触发的事件做了空判）。

### 14.3.8 Agent 操作步骤与验收

加一张异主题卡片：
1. 新建 `Border`，挂 `ui:UI4ThemeScope.Theme="dark"`；
2. 卡片自身底色/边框用 `DynamicResource UI4.Brush.Surface` / `UI4.Brush.BorderNormal`（它们会被重解析为作用域值，Demo 即此写法）；
3. 卡片内根 `StackPanel` 设 `TextElement.Foreground="{DynamicResource UI4.Brush.Text}"` 兜住标准 WPF `TextBlock`；
4. 卡内放 `UI4ComboBox`：直接用默认外观，**不要**给它的 `EditBackground`/`TextColor` 等赋本地值——它是命令式控件，作用域切换时靠重建样式取作用域色，本地值会让跟随失效（`p3verify.ps1:258-259` 验证的正是「显式赋值主题不吃掉」这条反向行为）。同页放 `UI4ListBox` 时按 14.3.6 处理徽章色。

验收（两条腿）：
- 脚本腿：`p3verify.ps1` 的成对断言——G1（引用式：卡内/卡外 CheckBox 边框分别等于内层键/全局键的 `BorderNormal` 定义值）、G2（命令式：ComboBox/Menu）、G3（P3 令牌化控件）、C（撤销回全局）、E（未知键撤销且不重建样式）、H（ComboBox/ListBox 含 Popup 项的可读性）。`scopewalk.ps1` 走 UIA 查 Demo 第 10 页：拨作用域键下拉、撤销、打开 ScopeWindow 读自述文本。
- 进程内腿（最小读回代码，宿主调试菜单里即可挂）：

```csharp
// C# 7.3：从作用域子树内任取一个控件，读它解析到的令牌画刷
var brush = (System.Windows.Media.SolidColorBrush)
    anyChildInScope.TryFindResource("UI4.Brush.Background");
string hex = brush.Color.ToString();   // 作用域 dark 时应为 #FF202026
```

## 14.4 推荐做法（Do / Don't）

- Do：作用域键用已注册键或小写常量；宿主自己注册过自定义键后可照样挂作用域（大小写不敏感）。
- Do：卡片底色走 `DynamicResource`，让通道①覆盖到宿主自定义画刷属性。
- Do：撤销传 `string.Empty` 或 `null`，语义完全等价；未注册键当作撤销来兜底容错。
- Don't：在作用域子树的命令式控件外观 DP 上随手赋本地值（主题跟随即失效）。
- Don't：把 `SetTheme`/作用域键切换放进逐帧循环或动画回调——命令式重建样式会掉帧（通用坑 §6）。
- Don't：指望 `UI4Theme.ResolvedMode` 反映作用域——它只报全局；作用域判定用 `GetTheme` + 资源读回。

## 14.5 坑与排错

- 症状：作用域里个别控件颜色没变。根因一：键名拼错（被静默撤销）。判据：`UI4ThemeScope.GetTheme(card)` 读回你写入的值，再用 `UI4Theme.IsRegistered` 语义自查（等价测试：`UI4Theme.ThemeKeys` 是否包含该键）。根因二：该控件被宿主赋了本地值。判据：`control.ReadLocalValue(UI4ComboBox.EditBackgroundProperty) != DependencyProperty.UnsetValue`。
- 症状：`UI4ListBox` 序号徽章在作用域卡片里仍是全局蓝。根因：14.3.6 的同步窗口逃逸 + 静态默认画刷。处置：显式 `NumberCircleBackground="{DynamicResource UI4.Brush.Accent}"`。
- 症状：撤销后部分控件仍留着旧主题色。根因：撤销只刷新「解析链已无任何作用域」的控件；若外层还有别的 `Theme`，内层保持外层色是正确行为。判据：沿祖先链找是否残留 `GetTheme != null` 的元素。
- 症状：整窗作用域标题栏不变。根因：`Window` 根路径只在作用域**变更**时补染（`UI4ThemeScope.cs:150-151`），纯窗口无 UI4 控件且从未变更过作用域时没有触发点。处置：按第 17 章通路 D，在 `SourceInitialized` 手动 `UI4WindowTitleBar.Apply(this)`。
- 症状：Popup 里的内容不跟随作用域。根因：逻辑树上 Popup 子元素可能尚未实例化。处置：给该区域控件调用公开的 `RefreshTheme()`（`UI4ListBox.cs:264`）类接口，或先弹出一次。

## 14.6 完成判据

1. 目标卡片 `GetTheme` 读回与写入一致；卡内 `UI4CheckBox` 的 `BorderNormalColor` == 作用域键的 `BorderNormal` 定义值（对照：`UI4Radio` 引用的是 `BorderSecondary` 令牌，见第 18 章）。
2. 卡内 `UI4ComboBox` 的 `EditBackground` 解析到作用域 `Surface`（进程内断言或 `p3verify.ps1` H 组同款比较）。
3. 传 `string.Empty` 撤销后，卡内解析色回到全局主题值，且 CheckBox 的 `Style` 引用未变（不重建）。
4. 写未注册键（如 `"nonsense"`）不抛异常，行为等同撤销。
5. 作用域根为 `Window` 时标题栏随动（读回 DWM 深浅标志，判据见第 17 章）。
6. 嵌套样本（外 highcontrast / 内 dark）内层子树按内层键解析（对照 `MainWindow.xaml:533-546`）。

---

# 第 15 章 自定义主题、强调色与品牌化

> 本章解决：在内置 light/dark/highcontrast 之外注册品牌主题、安全使用 `SetAccent`，并盘点哪些控件不会随品牌化改变。
> 读者：AI Agent（要在本项目之外新建/续写 WPF 业务代码）
> 前置章节：第 13 章（令牌全集）、第 14 章（作用域）。

## 15.1 目标与验收

- 掌握 `UI4ThemeDefinition` 的完整 API（含 `Clone()` 改键、`GetColor` 抛错两个反直觉点）。
- 能写出「Clone → With → Register → Apply」四步配方并跑通验收。
- 理解 `SetAccent` 写回已注册 definition 的副作用，掌握「改前先 Clone 备份」的恢复模板。
- 能列出品牌化必改的令牌与仍会露出写死观感的控件清单。
- 验收判据：`Apply(自定义键)` 返回 true 且 `UI4Theme.ResolvedKey` 等于该键；未注册键返回 false；强调色改动被全局与作用域消费者同时看到（`p3verify.ps1` G 组同款断言）。

## 15.2 源码依据

- `src/StartUI4Controls/UI4ThemeDefinition.cs:11-49` — `Key`（private set）、`GetColor`（`:25-28`，未设抛 `KeyNotFoundException`）、`Has`（`:31-34`）、`With` 流式（`:37-41`）、`Clone`（`:44-49`，键变 `<key>.clone`）。
- `src/StartUI4Controls/UI4ThemeDefinition.cs:94-203` — 静态 `Light()` / `Dark()` / `HighContrast()` 三份内置数据。
- `src/StartUI4Controls/UI4Theme.cs:161-166` — `Register`（同键覆盖 definition 并移除缓存实例）；`:149-158` — `Apply(key)→bool`；`:174-187` — `ApplyResolved` 的同键短路。
- `src/StartUI4Controls/UI4Theme.cs:678-692` — `SetAccent`（派生 0.85×、写回 definition）；`:716-722` — `Darken`。
- `src/StartUI4Controls/UI4Theme.cs:141-146` — `ModeForKey`：自定义键一律被报成 `Light`。
- `src/StartUI4Controls/UI4Button.cs:38-63,139-146` — 按钮渐变 DP 默认值硬编码、不走令牌。
- `src/StartUI4Controls/UI4Grid.cs:13-20` — 写死的浅蓝渐变背景。
- `p3verify.ps1:186-196` — `G SetAccent global switch` / `G SetAccent scoped switch` 与「测后必须恢复强调色」的注释。

## 15.3 正文

### 15.3.1 UI4ThemeDefinition API 要点

- `Key` 只读（private set），造实例只能走 `new UI4ThemeDefinition("brand")` 或 `Clone()`。
- `GetColor(token)` 对未设令牌抛 `KeyNotFoundException`；判断用 `Has(token)`。资源桥构建主题实例时会逐枚 `GetColor`（`UI4Theme.cs:615-616`），所以**30 个令牌必须给全**（守卫写法见 13.3.6）。
- `With(token, color)` 返回 `this`，可链式连写。
- `Clone()` 深拷贝颜色表，但**新键 = 原键 + ".clone"**（`UI4ThemeDefinition.cs:46`）——这是配方第一步的既定行为，不是 bug；键名在下一步统一改掉（改键只能再 Clone 一次或新建实例手工搬色，最实用的做法是 Clone 后马上把期望键注册出去，见下）。

### 15.3.2 自定义主题四步配方

```csharp
// C# 7.3：在 dark 基础上做品牌主题
// 1) Clone：拿到 30 项齐全的数据（键暂为 "dark.clone"）
var brand = UI4ThemeDefinition.Dark().Clone();
// 2) With：改品牌相关令牌（至少 Accent 系 + 强调边框）
brand.With(UI4ThemeToken.Accent,     Color.FromRgb(255, 92, 0))
     .With(UI4ThemeToken.AccentDark,  Color.FromRgb(217, 78, 0))
     .With(UI4ThemeToken.AccentEnd,   Color.FromRgb(255, 140, 60))
     .With(UI4ThemeToken.BorderHover, Color.FromRgb(255, 92, 0))
     .With(UI4ThemeToken.BorderFocus, Color.FromRgb(217, 78, 0));
// 3) Register：注册到品牌键。Clone 出来的键带 ".clone" 后缀，
//    若想要干净键名，用 new UI4ThemeDefinition("brand") 逐项 With 复制，
//    或接受 "dark.clone" 作为键（功能完全一致，只是名字丑）。
UI4Theme.Register(brand);
// 4) Apply：按键应用，返回 false = 键未注册
bool ok = UI4Theme.Apply("dark.clone");
```

注意同键短路的坑：`Register` 同键覆盖 definition 并**失效缓存实例**（`UI4Theme.cs:164-165`），但若该键此刻正是生效主题，`ApplyResolved`（`:174-187`）因键未变而短路，`Current` 仍是旧实例——重注册当前生效键不会立刻改外观。规避：先 `Apply` 别的键再 `Apply` 回来，或换一个新键注册。

### 15.3.3 SetAccent 的派生与副作用

`UI4Theme.SetAccent(Color)`（`:678-692`）做四件事：改当前实例的 `Accent`；派生 `AccentDark = 每通道 ×0.85`（`Darken`，`:716-722`）；**写回当前已注册 definition**（`:686-688`）；触发刷新与资源写回。第 3 步意味着它是有全局副作用的操作：之后再 `Apply("light")` 得到的 `Accent` 是你上次设置的值，而不是出厂的 `#0078D4`。测试里必须恢复或隔离，否则出现假失败（`p3verify.ps1:194-196` 的注释记录了同一教训）。

可运行的「改前先 Clone 备份」模板：

```csharp
// C# 7.3：安全改强调色 + 恢复
UI4ThemeDefinition backup = UI4ThemeDefinition.Light().Clone(); // 出厂数据重建，无需快照活的 definition
UI4Theme.SetAccent(Color.FromRgb(160, 32, 240));
// ……断言/演示……
// 恢复方案 A（简单）：再 SetAccent 回出厂值，同时把实例与 definition 都改回去
UI4Theme.SetAccent(Color.FromRgb(0, 120, 212));
// 恢复方案 B（彻底，light 正生效时）：重注册备份后借道切换打破同键短路
UI4Theme.Register(backup);                 // 键为 "light.clone"，不动 "light" 本身
UI4Theme.Apply("dark");
UI4Theme.Register(UI4ThemeDefinition.Light()); // 用全新出厂数据覆盖 "light"
UI4Theme.Apply("light");
```

另注意：全局与同键作用域**共享缓存主题实例**（`InstanceOf`，`:625-635`），`SetAccent` 天然同时打到两侧——这是特性不是缺陷（`p3verify.ps1:192-193` 的两条 G 组断言分别验证卡外/卡内开关的 `GradientStart` 都变新色）。

### 15.3.4 品牌化落地清单

构成品牌观感、值得在自定义 definition 里逐枚改的令牌：`Accent`、`AccentDark`、`AccentEnd`、`BorderHover`、`BorderFocus`、`CheckBackground`、`ListSelected`、`ProgressStart`、`Icon`。其余（背景/表面/文字/网格线）决定深浅可读性，一般保持与基座主题一致。

改完令牌后仍会露出写死观感的控件（宿主需有预期，详表见第 18/19 章）：
- `UI4Button`：`GradientStart/End` DP 默认 `#0078D4→#9333EA`（蓝→紫），任何主题下不变（`UI4Button.cs:38-63`，无资源引用）。品牌化办法：样式里统一设 `GradientStart="{DynamicResource UI4.Color.Accent}" GradientEnd="{DynamicResource UI4.Color.AccentEnd}"`。
- `UI4Grid`：构造里写死 `#E1ECF5→#FFFFFF` 浅蓝渐变，主题中性（`UI4Grid.cs:17`）。
- `UI4ListView` / `UI4GridView` / `UI4Tab` / `UI4ScrollViewer`：不跟随主题（事实速查 §6 清单），深浅背景下都保持出厂观感；放进异主题区域前先目检或显式覆盖外观 DP。

### 15.3.5 配色推导建议

- 对比度：主文本压底色用亮度式 `L = 0.299R+0.587G+0.114B`（与 `UI4WindowTitleBar.cs:183-184` 同式）。深底（L<128）配 `#E6E6E6` 级浅字；浅底配 `#1E1E1E` 级深字；强调色做按钮底时前景走 `OnWhiteBrush`（白），要校验白字压品牌色的 L 差 ≥ 60。
- highcontrast 兜底：品牌主题若供无障碍场景派生，保留「选中底深于选中字」关系（内置做法是深蓝 `#00008B` 托白字，`UI4ThemeDefinition.cs:188,194,198`）；不要直接拿品牌亮色当选中底。
- 派生色偷懒法：`AccentDark` 用与 `SetAccent` 相同的 0.85 系数逐通道相乘，保持与运行时派生一致。

### 15.3.6 键名常量化与大小写

主题键查找用 `StringComparer.OrdinalIgnoreCase`（`UI4Theme.cs:46,55`），大小写随便写；但业务代码仍应把键收敛为常量，防止 `"Brand"` / `"brand"` 混用导致注册了两个主题：

```csharp
// C# 7.3
public static class AppThemeKeys
{
    public const string Light = "light";
    public const string Dark = "dark";
    public const string HighContrast = "highcontrast";
    public const string Brand = "brand"; // 与 UI4Theme.Register 用的是同一个字符串
}
```

### 15.3.7 验收方式

- `UI4Theme.Apply(key)` 返回 `false` = 键未注册（`UI4Theme.cs:151`）；先断言 `UI4Theme.ThemeKeys` 包含该键。
- 判定生效主题比 `ResolvedKey`，**不要比 `CurrentMode`/`ResolvedMode`**——自定义键会被 `ModeForKey` 报成 `Light`（`:141-146`）。
- 进程内色值断言：读 `Application.Current.Resources["UI4.Brush.Accent"]` 的 `Color` 与你注册值比较；作用域消费者读回法见 14.3.8。
- `SetAccent` 传播：复刻 `p3verify.ps1` G 组——卡外与卡内（`Theme="dark"` 作用域）各放一个 `UI4Switch`，`SetAccent` 后两者的 `GradientStart` 应同时等于新品牌色。

## 15.4 推荐做法（Do / Don't）

- Do：自定义主题一律 `Clone()` 起手，保证 30 令牌齐全；注册前跑 `TokensComplete` 守卫。
- Do：测试/演示里用 `SetAccent` 前后配对恢复（15.3.3 方案 A）。
- Do：品牌键名收敛到常量类；`Apply` 返回值当断言用。
- Don't：对活的 definition 直接 `With` 改内置键而不备份——`Register` 覆盖后出厂值只能靠重新调用 `Light()/Dark()/HighContrast()` 找回。
- Don't：把 `SetAccent` 当「局部换色」用——它写回注册表、影响后续所有 `Apply`。
- Don't：假设改完令牌全控件跟随；按 15.3.4 清单逐个目检未令牌化控件。

## 15.5 坑与排错

- 症状：注册了自定义主题但 `CurrentMode` 显示 `Light`。根因：`ModeForKey` 对未知键兜底 `Light`（`:141-146`）。处置：改比 `ResolvedKey`。
- 症状：`Clone()` 后 `Apply("brand")` 返回 false。根因：`Clone` 不改键，键还是 `"dark.clone"`。处置：注册用的键与你 `Apply` 的字符串必须一致（或按 15.3.2 重建干净键）。
- 症状：重注册当前生效键后界面不变。根因：`ApplyResolved` 同键短路 + `Current` 实例未重建（15.3.2 注意事项）。处置：借道另一键再切回。
- 症状：后续测试里 `Apply("light")` 的强调色不对。根因：先前 `SetAccent` 写回了 definition（`:686-688`）。处置：恢复方案 A/B；或整场测试结束后重建进程（Demo 的验证脚本每次都是新实例）。
- 症状：自定义主题某控件报 `KeyNotFoundException`。根因：手工 `new` definition 漏了令牌，`FromDefinition` 逐枚取值即抛（`:615-616`）。处置：`Has` 全查或改 `Clone` 起手。

## 15.6 完成判据

1. 品牌主题经 `Register` 后 `UI4Theme.ThemeKeys` 包含键名常量；`Apply` 返回 true 且 `ResolvedKey == 键名`。
2. 注册前 `TokensComplete(def) == true`（30 项）。
3. 截屏/读回：`UI4.Brush.Accent`、`UI4.Brush.Border` 等资源值等于品牌定义；作用域卡片内同步变色。
4. `SetAccent` 演示后已执行恢复，重跑 13.6 第 4 条断言仍过（出厂 accent 链路无污染）。
5. 15.3.4 清单中的未跟随控件逐一过目：已显式覆盖外观 DP 或已接受出厂观感并在 UI 规范里注明。
6. 进程内断言 G 组同款：卡外/卡内开关 `GradientStart` 同帧等于新强调色。

---

# 第 16 章 系统跟随、持久化与高对比度

> 本章解决：让新应用跟随 Windows 亮/暗设置、把用户选择存下来下次启动恢复、并正确进入/交付高对比度主题。
> 读者：AI Agent（要在本项目之外新建/续写 WPF 业务代码）
> 前置章节：第 13 章（令牌）、第 15 章（自定义键与 `ResolvedKey` 判定）。

## 16.1 目标与验收

- 说清 `UI4ThemeMode.System` 的实现事实（读注册表 + `SystemEvents`）、`ResolvedMode` 与 `CurrentMode` 的分工。
- 会选并接入两种 `IThemePersistence` 实现，掌握 `Persistence` / `Save()` / `ApplyPersisted()` 的调用时序结论。
- 能落地「显式覆盖 > 持久化 > 跟随系统」三段式策略。
- 理解高对比度主题的配色语义与进入方式；给出不依赖库内不存在的无障碍设施的正确做法。
- 验收判据：`p3verify.ps1` F 组同款断言全过（System 解析出真实模式、`FollowSystemHighContrast` 默认 false、持久化往返恢复 Dark）；退出后无 `SystemEvents` 泄漏。

## 16.2 源码依据

- `src/StartUI4Controls/UI4Theme.cs:16-26` — `UI4ThemeMode` 四值；`:211-227` — `QuerySystemIsDark`（注册表 `AppsUseLightTheme`）；`:230-234` — `ResolveSystemKey`。
- `src/StartUI4Controls/UI4Theme.cs:236-248` — `Enable/DisableSystemFollow`（`SystemEvents.UserPreferenceChanged` 订阅/退订）；`:250-251` — `ReleaseSystemFollow()`；`:253-269` — 变更回调（类别过滤 + UI 线程编组）；`:196-209` — `FollowSystemHighContrast`（默认 **false**）。
- `src/StartUI4Controls/UI4Theme.cs:88-94` — `CurrentMode` / `ResolvedMode` / `ResolvedKey`。
- `src/StartUI4Controls/UI4Theme.cs:311-339` — `Persistence` / `ThemeSaved` / `ThemeLoading` / `Save()` / `ApplyPersisted()`。
- `src/StartUI4Controls/UI4ThemePersistence.cs:11-16` — `IThemePersistence` 契约；`:19-51` — `RegistryThemePersistence`；`:54-88` — `JsonThemePersistence`。
- `src/StartUI4Controls/UI4ThemeDefinition.cs:170-203` — `HighContrast()` 数据。
- `p3verify.ps1:162-183` — F 组断言（System/默认关/令牌数/持久化往返）。

## 16.3 正文

### 16.3.1 System 模式的实现事实

`SetTheme(UI4ThemeMode.System)`（`UI4Theme.cs:106-128`）做两件事：订阅 `SystemEvents.UserPreferenceChanged`；立即按 `ResolveSystemKey()` 解析——优先看高对比度（仅当 `FollowSystemHighContrast==true` 且 `SystemParameters.HighContrast`），否则读 `HKCU\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize` 的 `AppsUseLightTheme`，值为 0 判暗（`:211-234`；读失败静默按亮）。此后系统设置变化时回调过滤 `General/Color/Accessibility` 三类并编组回 UI 线程重解析（`:253-269`）。

读数分工：`CurrentMode` 报告**请求**（可能一直是 `System`），`ResolvedMode` 才是解析后的 Light/Dark/HighContrast 真值（`:88-91`）；自定义键场景两者都不可靠，比 `ResolvedKey`（第 15 章）。XAML 回显可绑 `UI4Theme.CurrentMode` 静态属性（Demo 页脚即 `{Binding Path=(ui:UI4Theme.CurrentMode), StringFormat=主题: {0}}`，`MainWindow.xaml:56`）。

### 16.3.2 ReleaseSystemFollow 为何必须

`SystemEvents` 是进程级静态事件，用过 System 模式就存在订阅。切回 `SetTheme(Light/Dark)` 或 `Apply(key)` 会自动退订（`:119,154`），但**全程停留在 System 模式直到退出**时没人替你收尾——不调 `UI4Theme.ReleaseSystemFollow()`（`:251`）就是订阅泄漏（验证脚本宿主与常驻服务尤其要在 `Exit` 事件里调一次；通用坑 §13）。

### 16.3.3 持久化：两种实现与接线时序

`IThemePersistence` 只有两个方法：`Save(UI4ThemeMode)`、`UI4ThemeMode? Load()`（无记录返回 null）。库内两个实现：

- `RegistryThemePersistence`：写 `HKCU\Software\StartUI4` 下名为 `ThemeMode` 的 DWORD（枚举整数值）；有参构造可换子键（多产品共存时用 `Software\MyCorp\MyApp`）。`Load` 校验 `Enum.IsDefined` 并吞异常；**`Save` 没有 try/catch**，注册表被组策略限制时会抛，宿主需自兜。
- `JsonThemePersistence(path)`：手写一行 JSON `{"mode":"Dark"}`，不引任何第三方库；`Load` 在文件文本里按带引号的枚举名匹配。路径通常放 `%APPDATA%`。

时序契约（结论先行）：**`Persistence` 赋值与 `ApplyPersisted()` 必须在第一个窗口创建之前**（App 构造函数内，或 `OnStartup` 里先于 `base.OnStartup(e)`——`StartupUri` 的主窗是在 base.OnStartup 才实例化的）。理由：`ApplyPersisted → SetTheme → NotifyThemeChanged` 走 `Dispatcher.BeginInvoke(Input)` 批处理（`:458-492`），首窗已加载后再恢复主题要多付一轮全量样式重建，且用户会看到浅色闪一下。

```csharp
// C# 7.3 —— App.xaml.cs（去掉 StartupUri，手动起窗）
protected override void OnStartup(StartupEventArgs e)
{
    UI4Theme.ApplyToApplication();                       // 播种资源桥（对齐 Demo App.xaml.cs:17）
    UI4Theme.Persistence = new RegistryThemePersistence();
    UI4Theme.ApplyPersisted();                           // 有存档则应用
    base.OnStartup(e);
    var main = new MainWindow();
    main.Show();
}
protected override void OnExit(ExitEventArgs e)
{
    UI4Theme.ReleaseSystemFollow();                      // System 模式收尾，防泄漏
    base.OnExit(e);
}
```

配套事件：`ThemeSaved(UI4ThemeMode)` 在 `Save()` 后触发；`ThemeLoading(UI4ThemeMode)` 在 `ApplyPersisted()` 读到值、应用前触发（`:319-337`）——挂日志或做「存档模式非法则拦截」用它。

### 16.3.4 三段式：显式覆盖 > 持久化 > 跟随系统

组合策略：用户应用内手动选过主题 → 存盘并锁住选择；没选过但本机有存档 → 用存档；都没有 → 跟随系统。

```csharp
// C# 7.3 —— 启动决策（放 OnStartup，ApplyToApplication 之后）
UI4Theme.Persistence = new JsonThemePersistence(
    System.IO.Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "MyApp\\theme.json"));
if (!UI4Theme.ApplyPersisted())          // 无 Persistence 或无存档 → false
    UI4Theme.SetTheme(UI4ThemeMode.System);

// C# 7.3 —— 用户在设置页里点了具体主题
void OnUserPickedTheme(UI4ThemeMode mode)
{
    UI4Theme.SetTheme(mode);             // 显式覆盖：SetTheme 会自动 DisableSystemFollow
    UI4Theme.Save();                     // 落盘，下次启动 ApplyPersisted 直接命中
}
// 用户点「跟随系统」时同样 SetTheme(System) + Save()，把 System 本身作为选择持久化。
```

### 16.3.5 高对比度主题语义

进入方式三条，全部到达同一份数据（`UI4ThemeDefinition.HighContrast()`）：`UI4Theme.Apply("highcontrast")`、`UI4Theme.SetTheme(UI4ThemeMode.HighContrast)`、System 模式开启 `FollowSystemHighContrast` 后跟随 `SystemParameters.HighContrast`。语义：纯黑底（Background/Surface/MenuBackground=`#000000`）、白字白框（TextForeground/Border 系/Icon/GridLine=`#FFFFFF`）、黄强调（Accent/AccentEnd/ProgressStart/BorderHover/BorderFocus=`#FFFF00`）；选中/勾选底用深蓝 `#00008B`（CheckBackground/ListSelected/RowSelectedBackground，`UI4ThemeDefinition.cs:188,194,198`）托白色对勾与文字。悬停/选中叠加改用半透明黄（`#33FFFF00`/`#4DFFFF00`），比默认的 8% 黑/白叠加显眼得多。纪律：高对比度下不要局部撤销这三个深蓝底（第 13 章表注），也不要给控件设固定彩色本地值——会把 WCAG 级对比度打回私有审美。

### 16.3.6 无障碍现实：宿主自己补 AutomationProperties

库内没有任何控件覆写 `OnCreateAutomationPeer`（全 src 检索仅命中设计文档一条），也没有 Toggle/Range 等 UIA pattern 实现。后果与对策：
- UIA 客户端只能按 `AutomationId`/Name/坐标定位（通用坑 §11）。宿主给每个可交互控件设 `x:Name`（自动进 `AutomationId`）与 `AutomationProperties.Name`：

```xml
<ui:UI4Switch x:Name="AutoStartSwitch" IsOn="{Binding AutoStart}"
              AutomationProperties.Name="开机自动启动" />
<ui:UI4Slider x:Name="VolumeSlider" Minimum="0" Maximum="100"
              Value="{Binding Volume}"
              AutomationProperties.Name="音量" />
```

- `UI4Switch` 无 Toggle pattern 且只响应鼠标左键抬起（`UI4Switch.cs:241-249`），键盘不可达：宿主要么包一层 `Button`/`CheckBox` 语义（用 `InputBindings` 绑 Space/Enter 翻 `IsOn`），要么在表单里改用 `UI4CheckBox`。

### 16.3.7 可访问性检查清单

逐项在交付前过一遍：
1. 对比度：正文/图标对所在底 ≥ 4.5:1 粗校验，或亮度差 ≥ 60（15.3.5 公式）；高对比度主题下跑通全页。
2. 焦点可见：Tab 走查每个可聚焦控件都有肉眼可见的焦点态（文本框类走 `BorderFocus` 令牌；`UI4Button` 的模板**没有**焦点态——宿主用 `FocusVisualStyle` 补）。
3. 键盘可达：CheckBox/Radio 原生可用；Switch/CircleSlider 依赖鼠标，按 16.3.6 补桥；避免把功能只放在 hover 上（UI4ListView/GridView 的悬停放大对键盘用户不可见）。
4. 读屏标签：每个控件 `AutomationProperties.Name` 非空且说的是用途不是装饰。
5. 状态回显不依赖颜色单一通道（高对比度下黄/蓝语义会变）。

## 16.4 推荐做法（Do / Don't）

- Do：启动顺序固定为 `ApplyToApplication → 配 Persistence → ApplyPersisted（失败则 SetTheme(System)）→ 建窗`。
- Do：`OnExit` 里 `ReleaseSystemFollow()`；每次用户改主题 `Save()`。
- Do：判断当前生效主题用 `ResolvedMode`（内置三模式）或 `ResolvedKey`（含自定义）。
- Don't：把 `CurrentMode` 当真值展示给用户——它可能一直是 `System`。
- Don't：给 `FollowSystemHighContrast` 默认置 true 而不告知用户——这会改变 System 模式语义（默认 false 是既有行为，`:196-199`）。
- Don't：假设控件自带 UIA 语义；所有无障碍标签由宿主显式给。

## 16.5 坑与排错

- 症状：进程驻留后 `SystemEvents` 回调还在跑、报「在无 UI 线程时访问」。根因：没调 `ReleaseSystemFollow`，静态事件跨窗口生命周期存活。处置：`OnExit` 收尾；判据：全程用过 System 且退出前从未 SetTheme 到非 System。
- 症状：启动恢复主题闪一下浅色。根因：`ApplyPersisted()` 在首窗 Loaded 之后才调。处置：按 16.3.3 时序前移。
- 症状：`ApplyPersisted()` 永远 false。根因：忘了赋 `Persistence`（默认 null，`:316`）或存档值不在 `Enum.IsDefined` 范围。判据：直接 `new RegistryThemePersistence().Load()` 看返回；注册表看 `HKCU\Software\StartUI4` 的 `ThemeMode`。
- 症状：系统改暗色应用不动。根因一：`FollowSystemHighContrast`/模式没处在 `System`（`OnUserPreferenceChanged` 里 `_requestedMode != System` 直接 return）。根因二：改的是「应用」暗色而注册表路径读的是 `AppsUseLightTheme`——确认改的是系统→个性化→颜色→应用模式。判据：读回 `UI4Theme.ResolvedMode`。
- 症状：高对比度下某些宿主自绘区域整块黑底丢边框。根因：宿主画刷用硬编码色没走令牌。处置：全部改 `DynamicResource`（第 13 章）。

## 16.6 完成判据

1. 新装机器（无存档）启动后 `ResolvedMode ∈ {Light, Dark}` 且与系统「应用颜色模式」一致；切系统设置后不重启即翻转。
2. 用户选 Dark → 重启 → `ApplyPersisted()` 返回 true、`ResolvedMode == Dark`（等价 `p3verify.ps1:175-182` F persistence roundtrip）。
3. `FollowSystemHighContrast` 初值为 false（`:168` 同款断言）；置 true 且系统开 HC 时 `ResolvedKey == "highcontrast"`。
4. `Apply("highcontrast")` 后 `Application.Resources["UI4.Brush.CheckBackground"]` == `#00008B`（D 组断言思路）。
5. 退出路径存在 `ReleaseSystemFollow()`；AutomationProperties 抽查 5 个控件均有 Name。
6. 可访问性清单 5 项逐条签字。

---

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

---

# 第 18 章 基础交互控件实操

> 本章解决：在新应用里正确使用 8 个基础交互控件（按钮/开关/复选/单选/滑条/圆滑条/进度条/进度环），并避开类型与主题跟随上的既有坑。
> 读者：AI Agent（要在本项目之外新建/续写 WPF 业务代码）
> 前置章节：第 13 章（令牌与资源桥）、第 14 章（作用域）、第 16 章（无障碍标签）。控件一览背景见 `README.md 第四/五章`。

## 18.1 目标与验收

- 每个控件能写出：关键 DP 与类型、事件、绑定方式、主题通道（引用式/命令式/不跟随）。
- 记住「属性名相同但类型不同（Color vs Brush）」对照，不再写错赋值类型。
- 明白结构类 DP 触发样式重建的代价，不在动画循环里改 DP。
- 能给表单页/设置页组合这些控件并通过深色主题可读性验收。
- 验收判据：所有可交互控件有 `x:Name` + `AutomationProperties.Name`；作用域卡片内切主题，这些控件解析色全部跟对（`p3verify.ps1` G1/G3、H 组同款）。

## 18.2 源码依据

- `src/StartUI4Controls/UI4Button.cs:24-128`；`UI4Switch.cs:24-148,202-249`；`UI4CheckBox.cs:30-173`；`UI4Radio.cs:25-123`。
- `src/StartUI4Controls/UI4Slider.cs:24-122`；`UI4CircleSlider.cs:28-151,189-260`；`UI4ProgressBar.cs:29-163,268-356`；`UI4ProgressRing.cs:37-212`。
- 主题通道定义见 `事实速查.md` §6；`SetResourceReference`（引用式）与 `TrackControl`（命令式）机制见 `UI4Theme.cs:363-377`。
- Demo 用法样本：`samples/StartUI4Demo/MainWindow.xaml:478-492`（作用域卡内全套）、`ScopeWindow.xaml:22-38`。

## 18.3 正文

以下「通道」含义：**引用式** = 构造里 `SetResourceReference`，作用域/全局自动跟随；**命令式** = `IThemeAware`+`TrackControl`，切主题重建样式；**不跟随** = 默认值硬编码且不随主题刷新。前缀约定 `xmlns:ui="clr-namespace:StartUI4Controls;assembly=StartUI4Controls"`。

### 18.3.1 UI4Button（命令式）

用途：主操作按钮。DP：`CornerRadius`（CornerRadius，默认 6）、`GradientStart`/`GradientEnd`（**Color**，默认 `#0078D4`/`#9333EA`）、`HoverBackground`/`HoverBorderBrush`/`HoverForeground`（**Brush**，默认 `#0066B5` 画刷/null/白）。事件沿用 `Button.Click`；命令走原生 `Command`。每个外观 DP 的回调都全量重建样式（`OnStyleRefresh`）。坑：**所有主题下仍是蓝→紫渐变**——渐变 DP 默认值硬编码、不走令牌，深浅/高对比度都不变（`UI4Button.cs:38-63,139-146`）；模板无焦点态。品牌化片段：

```xml
<ui:UI4Button Content="保存" Command="{Binding SaveCmd}"
              AutomationProperties.Name="保存"
              GradientStart="{DynamicResource UI4.Color.Accent}"
              GradientEnd="{DynamicResource UI4.Color.AccentEnd}"
              HoverBackground="{DynamicResource UI4.Brush.AccentDark}" />
```

### 18.3.2 UI4Switch（引用式）

用途：即时生效的开/关（设置页）。继承 `Control`（无 Content），文字标签用旁边 `TextBlock`。DP：`IsOn`（bool，**默认双向绑定**，`BindsTwoWayByDefault`）、`GradientStart`/`GradientEnd`（Color）→ 令牌 `UI4.Color.Accent`、`OffBackground`（Color）→ `UI4.Color.OffBackground`、`ThumbColor`（Color，白色，**不引用令牌**）、`SwitchWidth`/`SwitchHeight`（double，50/28）。事件：`Toggled`（冒泡 RoutedEvent，`IsOn` 任何变化都发——包括代码赋值，处理里别再翻 `IsOn`）。绑定：`IsOn="{Binding AutoStart}"` 免写 Mode。主题观感：开=Accent 渐变，关=OffBackground（Light `#C8C8D2` / Dark `#3C3C46`）。坑：① 只响应鼠标左键抬起（`OnMouseLeftButtonUp` 且 `e.Handled=true`），**无键盘路径**；② **无 Toggle pattern/AutomationPeer**，UIA 只能按 Name 定位 + 坐标点击；③ 上游拉伸缺陷已用 `MeasureOverride`/`ArrangeOverride` 钉死为 `SwitchWidth×SwitchHeight`，宿主不要再给 `HorizontalAlignment="Stretch"` 期望铺满。片段：

```xml
<StackPanel Orientation="Horizontal">
    <ui:UI4Switch x:Name="AutoStartSwitch" IsOn="{Binding AutoStart}"
                  AutomationProperties.Name="开机自动启动" Toggled="AutoStartSwitch_Toggled"/>
    <TextBlock Text="开机自动启动" VerticalAlignment="Center" Margin="10,0,0,0"
               Foreground="{DynamicResource UI4.Brush.Text}"/>
</StackPanel>
```

### 18.3.3 UI4CheckBox（引用式）

用途：可多选的确认项。DP：`CornerRadius`（默认 6；**`BoxCornerRadius` 是 `[Obsolete]` 别名，指向同一个 DP**，新代码禁用）、`CheckBackground`（Color）→ 令牌 `CheckBackground`、`BorderNormalColor`（Color）→ 令牌 `BorderNormal`、`TextColor`（Color）→ 令牌 `TextForeground`、`BoxSize`（18）、`TextMargin`（Thickness 8,0,0,0）；另有私有引用项跟随 `CheckBoxUnchecked`/`HoverBorderColorLight` 令牌。事件/绑定：原生 `IsChecked`（三态可用）。对勾是白色硬编码 Path——深色/高对比度下由 `CheckBackground`（Dark `#008CD2` / HC 深蓝 `#00008B`）保证对比。坑：结构类 `CornerRadius/BoxSize/TextMargin` 改动重建模板；颜色类 DP 改动不重建（回调无 `OnStyleRefresh`），可放心动态换。

### 18.3.4 UI4Radio（引用式）

用途：互斥选项组。DP：`CheckBackground`（Color）→ 令牌 `CheckBackground`、`BorderNormalColor`（Color）→ 令牌 **`BorderSecondary`**（属性名与令牌名不同名，验收时别拿错基准）、`DotColor`（Color，白，选中内点）、`TextColor`（Color）→ 令牌 `TextForeground`、`BoxSize`/`TextMargin` 同上。绑定：原生 `IsChecked` + `GroupName`（Demo `MainWindow.xaml:486` 即 `GroupName="ScopeRadioGroup"`）。坑：`TextColor` 的 **DP 默认值是 `Colors.Black`**，与令牌 `#1E1E1E` 不一致——仅当资源解析失败（没调 `ApplyToApplication`）时露出；其余默认值（`CheckBackground #0066B5`、`BorderNormalColor #B4B4C8`）同理只是兜底。`DotColor/BoxSize/TextMargin` 改动重建样式。

### 18.3.5 UI4Slider（引用式）

用途：连续数值（音量/亮度）。继承原生 `Slider`。DP：`IsValueVisible`（bool，默认 true，轨道下方显示 Min/Value/Max 三个刻度文本）、`ThumbSize`（16）、`TrackBackground`（Color）→ 令牌 `Surface`（**DP 默认值却是纯白**，两者不一致，见坑）、`GradientStart/End`（Color）→ 令牌 `Accent`、`CornerRadius`（6）。绑定：原生 `Value`/`Minimum`/`Maximum`。主题观感：Dark 下轨道底 `Surface #282830`、渐变 `#0099FF`。坑：① **所有外观 DP（含颜色）都挂 `OnStyleRefresh` 重建样式**——在值动画循环里改 `GradientStart` 之类会掉帧，只改 `Value` 安全；② `TrackBackground` DP 默认白与令牌不一致，脱离资源环境（如离线单测 new 出来未挂树）会露白轨；③ 拖动数学按 `ActualWidth - ThumbSize` 线性换算，`Width` 未定（NaN）时不更新轨道布局，确保给宽或让它拉伸。

### 18.3.6 UI4CircleSlider（不跟随）

用途：旋钮式环形取值。继承 `ContentControl`，**`Content` 被构造函数占为内部绘制容器**（`this.Content = _container`，`UI4CircleSlider.cs:28-38`）——宿主设置 `Content` 会摧毁渲染，中心文字请用 `ShowValueText`（bool，默认 false）+ `ValueFontSize`（30），不要往 Content 里塞标签。DP：`Value/Minimum/Maximum/SmallChange`（double；`Value` 有 Coerce 夹在 [Min,Max]，抬手按 `SmallChange` 吸附）、`RingThickness`（8）、`RingForeground`/`RingBackground`（**Brush**，默认 `#0078D4`/`#0A000000`，**无资源引用 ⇒ 不跟主题**）、`AnimationDuration`（double 秒，默认 0.5，Loaded 时从 Minimum 起动画到当前 Value）。交互：按下即打断入场动画并捕获鼠标，角度从 12 点顺时针映射。深色下必改：

```xml
<ui:UI4CircleSlider x:Name="TimerDial" Minimum="0" Maximum="60"
                    AutomationProperties.Name="分钟选择盘"
                    RingForeground="{DynamicResource UI4.Brush.Accent}"
                    RingBackground="{DynamicResource UI4.Brush.TrackBackground}"
                    ShowValueText="True" Foreground="{DynamicResource UI4.Brush.Text}" />
```

坑：中心数值文字前景取 `Foreground`（继承属性，可用上层 `TextElement.Foreground` 驱动）；`Value` 被入场动画 `BeginAnimation` 驱动，动画期间读 `Value` 是中间值，绑定业务属性要等 `Completed` 或用户抬手。

### 18.3.7 UI4ProgressBar（引用式）

用途：线性进度/忙碌指示。DP：`Minimum/Maximum/Value`（0/100/0）、`IsIndeterminate`（bool，默认 false；true 时 40% 宽滑块 1 秒循环扫过）、`CornerRadius`（5）、`GradientStart`（Color）→ 令牌 `ProgressStart`、`GradientEnd` → `Accent`、`TrackBackground` → `TrackBackground` 令牌。构造默认 `Height=6`、`MinWidth=100`。绑定：`Value="{Binding Job.Progress}"` 直接可。坑：① 显式给 `Background` 赋画刷会**顶替渐变填充**（`GetIndicatorBrush` 优先本地值，`:268-274`）——这不是主题通道，别拿它改色；② `Unloaded` 停动画、`Loaded` 复原，放进 TabControl 切页不会泄漏；③ 扫块宽度按 `ActualWidth*0.4` 计算，0 宽容器里不可见属正常。

### 18.3.8 UI4ProgressRing（不跟随）

用途：环形进度/忙碌。同为 `ContentControl` 且 **`Content` 被内部占用**（构造 `this.Content = _container`），默认 `Width/Height=80`。DP：`IsActive`（bool，默认 true）、`IsIndeterminate`（默认 **true**，120° 弧 0.5 秒旋转）、`Minimum/Maximum/Value`、`RingBackground`/`RingForeground`（**Brush**，默认 `#0A000000`/`#0078D4`，无资源引用 ⇒ 不跟主题；但显式设 `Foreground` 且未设 `RingForeground` 时会自动同步一次，`:244-253`）、`RingThickness`（6）、`ShowValueText`（默认 **true**）、`ValueFontSize`（30）、`EnableStartupAnimation`（bool，默认 true）+ `StartupAnimationDuration`（0.5）。`AnimatedValue` 是公开的动画中间值 DP（带 setter，但由故事板驱动，宿主按只读对待，别去写它）。坑：① 大部分变更回调经 `Dispatcher.BeginInvoke` 延迟生效——同一同步事务里连设 `Value` 两次只渲染末次，作用域内主题快照逃逸风险同 14.3.6；② 深色下必须显式给 `RingForeground`/`RingBackground` 令牌画刷（片段同 18.3.6 风格）；③ `IsActive=false` 只清动画并置容器透明度 0，占位仍在，收起要连容器 Visibility 一起管。

### 18.3.9 属性名相同、类型不同对照表

| 概念 | Color 型（吃 `UI4.Color.*`） | Brush 型（吃 `UI4.Brush.*`） |
|---|---|---|
| 渐变起/止 | UI4Button/UI4Switch/UI4Slider/UI4ProgressBar 的 `GradientStart/End` | —（UI4ComboBox 的 `FocusGradientStart/End` 亦 Color） |
| 开关关闭底 | UI4Switch `OffBackground` | —（UI4ProgressBar 同名 `TrackBackground` 是 Color，Ring 的不是） |
| 轨道/环底 | UI4Slider、UI4ProgressBar `TrackBackground`（Color） | UI4ProgressRing `RingBackground`、UI4CircleSlider `RingBackground`（Brush） |
| 前景 | UI4CheckBox/UI4Radio `TextColor`（Color） | UI4ProgressRing/UI4CircleSlider `RingForeground`（Brush） |
| 悬停 | UI4CheckBox 私有 `HoverBoxColor`（Color，宿主不可设） | UI4Button `HoverBackground/HoverBorderBrush/HoverForeground`（Brush） |
| 其他库例 | UI4Panel `BorderColor`（Color） | UI4ListView `ItemBackground`（Brush）（通用坑 §7） |

规则：赋值/XAML 引用前先看 DP 注册类型——`Color` 属性配 `UI4.Color.*`，`Brush` 属性配 `UI4.Brush.*`，跨型赋值在 XAML 是运行期空值、在代码是编译错误。

### 18.3.10 结构类 DP 与样式重建代价

本章所有控件的 `CornerRadius/BoxSize/ThumbSize/TextMargin/SwitchWidth/SwitchHeight` 等结构类 DP，以及 UI4Slider 的全部外观 DP，回调都触发 `Style = BuildXxxStyle()` 全量重建（`FrameworkElementFactory` 重走一遍）。后果：在 `CompositionTarget.Rendering`/故事板回调里高频改这些 DP 会明显掉帧；p2verify 的「样式未被重建」断言思路可用 `ReferenceEquals(ctrl.Style, before)` 在宿主自查。安全的高频更新只走 `Value`/`IsChecked`/`IsOn` 这类状态 DP。

### 18.3.11 表单页与设置页组合示例

设置页（开关 + 滑条 + 进度 + 环形），深色可读性写法：

```xml
<StackPanel Margin="16" TextElement.Foreground="{DynamicResource UI4.Brush.Text}">
    <ui:UI4Switch x:Name="NotifSwitch" IsOn="{Binding Notify}"
                  AutomationProperties.Name="桌面通知" Margin="0,0,0,10"/>
    <ui:UI4CheckBox Content="开机自动启动" IsChecked="{Binding AutoStart}"
                    AutomationProperties.Name="开机自动启动项" Margin="0,0,0,10"/>
    <ui:UI4Slider x:Name="BrightnessSlider" Minimum="0" Maximum="100"
                  Value="{Binding Brightness}" Width="240" HorizontalAlignment="Left"
                  AutomationProperties.Name="屏幕亮度" Margin="0,0,0,10"/>
    <ui:UI4ProgressBar x:Name="SyncBar" Value="{Binding SyncProgress}"
                       IsIndeterminate="{Binding SyncBusy}" Height="6" Margin="0,0,0,10"/>
    <ui:UI4ProgressRing x:Name="SyncRing" Width="64" Height="64" Value="{Binding SyncProgress}"
                        RingForeground="{DynamicResource UI4.Brush.Accent}"
                        RingBackground="{DynamicResource UI4.Brush.TrackBackground}"
                        Foreground="{DynamicResource UI4.Brush.Text}"
                        ValueFontSize="16" AutomationProperties.Name="同步进度"/>
</StackPanel>
```

深色验收判据：切 `dark` 后——开关关态 `OffBackground` 应为 `#3C3C46`、滑条轨道 `Surface #282830`、Ring 数字用 `TextForeground #E6E6E6`；Ring/CircleSlider 若仍是出厂蓝即忘了显式令牌引用（18.3.6/18.3.8）。任何一处「看不见」，按 13.6 第 6 条亮度公式判。

## 18.4 推荐做法（Do / Don't）

- Do：`IsOn`/`Value` 直接 `{Binding}`，Switch 的双向是默认；其余原生语义（`IsChecked`、`Command`）照用。
- Do：Ring/CircleSlider/Button 这类「不跟随或半跟随」控件统一显式引用 `DynamicResource UI4.Brush/Color.*`。
- Do：标签列给足 `AutomationProperties.Name`（无 pattern，坐标与 Name 是 UIA 唯一抓手）。
- Don't：在动画/渲染回调里改结构类 DP（18.3.10）。
- Don't：给 `UI4CircleSlider`/`UI4ProgressRing` 设 `Content`（内部占用）。
- Don't：拿 `BoxCornerRadius` 写新代码（`[Obsolete]` 别名，同一 DP）；Don't：靠颜色单一通道表达开关状态（16.3.7）。

## 18.5 坑与排错

- 症状：StackPanel 里 Switch 的轨道和滑块分离。根因：宿主覆盖了拉伸或给了大于期望的尺寸历史行为。处置：保持默认对齐（库已用 `ArrangeOverride` 钉 `SwitchWidth×SwitchHeight`，`UI4Switch.cs:207-218`）；判据：控件 ActualWidth == `SwitchWidth`。
- 症状：UIA 脚本点不动开关。根因：无 Toggle pattern，`Toggle` 控件模式调用直接不可用。处置：按 `AutomationProperties.Name` 找元素后点击其中心坐标（Demo 的 `theme.ps1` 即此法）。
- 症状：深色下按钮还是亮蓝渐变。根因：UI4Button 渐变不走令牌（18.3.1）。处置：显式 `GradientStart/End` 引 `UI4.Color.*`。
- 症状：赋值 `RingForeground = (Color)…` 编译不过。根因：Ring/CircleSlider 是 Brush 型（对照 18.3.9）。处置：用 `BrushOf`/`UI4.Brush.*`。
- 症状：`SwitchWidth` 改了没效果。根因：先设尺寸后要等模板期元素就绪——`OnSizeChanged` 回调已在构造期防护（`_trackBorder` 判空），但 `Loaded` 前 ActualWidth 仍按旧值测量。判据：`Loaded` 后读 `_trackBorder` 所属控件 ActualSize == 新尺寸；否则检查是否宿主 Width 覆写。
- 症状：Toggled 处理器里读到的 `IsOn` 与 UI 不符。根因：事件在 DP 变更回调里同步发出，动画（200ms）尚在飞行。处置：只信 DP 值，别按视觉帧推断。
- 症状：CircleSlider 入场动画期间绑定业务值被打断写回 Minimum。根因：动画持有 `ValueProperty`（FillBehavior.HoldEnd + `BeginAnimation`），本地赋值被动画优先级压制；按下鼠标会解除（`OnMouseLeftButtonDown` 打断逻辑）。处置：代码设值前先 `ClearValue`/等待动画 Complete，或把 `AnimationDuration` 设 0。

## 18.6 完成判据

1. 页面上每个本章控件有 `x:Name` 与非空 `AutomationProperties.Name`；开关/旋钮有键盘替代路径（16.3.6）。
2. `dark` 与 `highcontrast` 下逐一目检/断言：Switch 关态底、Slider 轨道、ProgressBar 轨道分别等于 13.3.1 表中对应令牌值；Ring/CircleSlider 用了令牌画刷。
3. 全工程 XAML 检索：无 `BoxCornerRadius`、无给 `UI4CircleSlider`/`UI4ProgressRing` 设 `Content`。
4. `ReferenceEquals(ctrl.Style, beforeStyle)` 在仅改 `Value/IsChecked/IsOn` 前后为 true（样式未被重建）。
5. 把 18.3.11 片段放进带 `ui:UI4ThemeScope.Theme="dark"` 的卡片，全局为 light 时卡内按 18.3.11 判据全对（等价 `p3verify.ps1` G1/G3、H 组断言思路）。
6. 运行期无异常：宿主错误日志（对齐 Demo `demo-errors.log` 机制）为空。

---

# 第 19 章 文本与输入控件实操

> 本章解决：在新业务应用里正确落地文本显示与文本输入（`UI4TextBlock` / `UI4FlipTextBlock` / `UI4TextBox` / `UI4PasswordBox` / `UI4CodeEditor`），并搭出一屏带校验提示的输入表单。
> 读者：AI Agent（要在本项目之外新建/续写 WPF 业务代码）
> 前置章节：第 13 章（令牌与资源桥）、第 10 章（XAML 编写规范与命名约定）、第 07 章（依赖与工程配置）

## 19.1 目标与验收

- 能区分 `UI4TextBlock`（`ContentControl`）与原生 `TextBlock`，且不写出 CS1503。
- 五个控件的公开属性全部用对：不写不存在的属性、不给死属性赋值、不踩 `new` 隐藏 DPs 的引用歧义。
- 密码框知道真实基类是 `TextBox`、值要从 `Password` 读、以及明文内存暴露的处置边界。
- 交付一屏可编译的输入表单（标签 + 输入 + 校验提示），主题色全走 `DynamicResource`。
- 验收：切一次全局主题，四个输入控件（除 `UI4TextBlock` 外）同帧变色；`demo-errors.log` 类运行期日志为空。

## 19.2 源码依据

- `src/StartUI4Controls/UI4TextBlock.cs:34`（`: ContentControl`）、`:37/:50/:63/:76/:89/:102/:115/:128/:141/:154/:167/:180/:193-243`（DP 全集）、`:254`（ctor）、`:284-319`（模板回调 + 内置菜单）、`:321-364`（`BuildTextStyle`）。
- `src/StartUI4Controls/UI4FlipTextBlock.cs:12`、`:16-158`（DP）、`:173-177`（`FontSize` 默认 60）、`:186-194`（`ReadLocalValue` 保护）、`:302-318`（`UserSet`）、`:320-406`（翻牌动画与 `RenderTargetBitmap`）。
- `src/StartUI4Controls/UI4TextBox.cs:29`、`:31-133`（DP）、`:159-179`（ctor 与 8 个 `SetResourceReference`）、`:296-330`（内置菜单）、`:332-445`（模板）、`:459/:470/:484`（三个公开转换器）。
- `src/StartUI4Controls/UI4PasswordBox.cs:31`（`: TextBox`）、`:123-158`（安全注释 + `Password`/`PasswordChar`/`IsPasswordMode`）、`:228-255`（ctor）、`:297-312`（`Unloaded`）、`:486-556`（掩码显示与 `ClearPassword`）、`:558-598`（按住显形按钮）、`:639-687`（`RebuildContextMenu`）。
- `src/StartUI4Controls/UI4CodeEditor.cs:22-74`。
- `samples/StartUI4Demo/MainWindow.xaml:77-137`（输入页与文本显示页实例）。
- `常见问题清单/UI4ComboBox-文本高度显示不全.md:250-285`（高度公式的推导方法，本章套用到输入控件）。

## 19.3 选型表

| 控件 | 基类 | 主题通道 | 宿主主要属性 |
|---|---|---|---|
| `UI4TextBlock` | `ContentControl` | **不跟随**（无 `IThemeAware`、无 `TrackControl`） | `Text` `Content` `Foreground` `PanelBackground` `Padding` `CornerRadius` `TextWrapping` `Shadow*` |
| `UI4FlipTextBlock` | `ContentControl` | 命令式（`IThemeAware`，`TrackControl` 于 `:183`） | `Text` `FlipRate` `Card*` `ShadowColor` |
| `UI4TextBox` | `TextBox` | 引用式（8 个 `SetResourceReference`） | `PlaceholderText` `ShowClearButton` `InnerPadding` `EditBackground` `HoverBorderColor` `FocusBorderColor` |
| `UI4PasswordBox` | **`TextBox`** | 引用式（8 个 `SetResourceReference`，`:241-248`） | `Password` `PasswordChar` `IsPasswordMode` `ShowPasswordButton` `ClearPassword()` |
| `UI4CodeEditor` | `ICSharpCode.AvalonEdit.TextEditor` | 仅滚动条套库样式 | AvalonEdit 全集：`Text` `SyntaxHighlighting` `IsReadOnly` `ShowLineNumbers` `WordWrap` |

`UI4TextBlock` 与 `UI4PasswordBox` 的这两行是本表的价值所在：**一个不跟随主题，一个不是你以为的基类**。

## 19.4 UI4TextBlock：它是 ContentControl

模板内部是一个只读无边框 `TextBox`（`UI4TextBlock.cs:334-338`），文本经 `TextOrContentConverter` 从 `Text` 或 `Content` 二选一取出（`:340-346`）：`Text` 非空用 `Text`，否则用 `Content.ToString()`。

由此得三条硬规则：

1. **绑定用 `Content`，不要绑 `Text`。** `TextProperty` 的回调是 `OnStyleRefresh`（`:42`），每次变更全量重建 `Style`（`:260-264`）；`Content` 是 `ContentControl` 原生 DP，不触发重建。Demo 的卡片模板正是这么写的（`MainWindow.xaml:255`）。
2. **`Content` 放 UIElement 不会渲染。** 转换器只调 `ToString()`，模板里没有 `ContentPresenter`，界面上会出现 `System.Windows.Controls.TextBlock` 这种类型名。要放富内容就用原生 `TextBlock`/`ContentControl`。
3. **类型不是 `TextBlock`。** 把 `UI4TextBlock` 传给形参为 `System.Windows.Controls.TextBlock` 的方法会报 CS1503；反之原生 `TextBlock` 也没有 `PanelBackground`/`ShadowDepth`。

### 四个 `new` 隐藏 DP 的引用歧义

`Foreground`(`:50`)、`Padding`(`:115`)、`FontSize`(`:128`)、`FontWeight`(`:141`) 都用 `public static new readonly DependencyProperty` 重新注册。**代码里清除/读取值必须显式点名派生类的字段**：

```csharp
// 正确：清掉 UI4TextBlock 自己那份
tb.ClearValue(UI4TextBlock.FontSizeProperty);
// 错误：清的是 ContentControl 那份，UI4TextBlock 的值仍在，字号不会回默认
tb.ClearValue(Control.FontSizeProperty);
```

XAML 里直接写属性名不受影响（`FontSize="24"` 走 CLR 属性 → 派生 DP）。

### GradientStart/GradientEnd 是死属性

两者在 `:76/:89` 注册（默认 #0078D4 / #9333EA），但 `BuildTextStyle()`(`:321-364`) 从未读取。渐变文字只有一条路：给 `Foreground` 传 `LinearGradientBrush`（`MainWindow.xaml:116-123` 实例）。

### 主题色必须宿主自己给

`Foreground` 默认 `null`（`:55`），且只有非 null 时才写到内层 `TextBox`（`:354-357`），而类里没有任何主题订阅 ⇒ **深色主题下文字仍是系统默认黑，深底黑字不可读**。两种正确写法：

```xml
<!-- 方案 A（推荐）：靠继承，压过内层 TextBox 的默认样式值 -->
<StackPanel TextElement.Foreground="{DynamicResource UI4.Brush.Text}">
    <ui:UI4TextBlock Text="跟随所在作用域" FontSize="13"/>
</StackPanel>

<!-- 方案 B：显式绑定，注意 StaticResource 不会跟随主题 -->
<ui:UI4TextBlock Text="标题" Foreground="{DynamicResource UI4.Brush.Text}"/>
```

Demo 的两个作用域卡片都用方案 A（`MainWindow.xaml:476`、`:505`），这是它能在深色/高对比度下可读的原因。

### 阴影与内置菜单

`ShadowDepth`(8)/`ShadowBlurRadius`(5)/`ShadowOpacity`(0)/`ShadowColor`(Black) 的回调是 `OnShadowPropertyChanged` → `UpdateTextShadow()`（`:266-282`），**不重建 Style**，但每次赋值 `new` 一个 `DropShadowEffect`；逐帧改阴影参数会掉帧。`ShadowOpacity` 默认 0，即默认无阴影。

`OnApplyTemplate`(`:284-290`) 抓到 `PART_TextBox` 后挂一个 `UI4ContextMenu{Width=150}`，只含「复制」「全选」（`:297-319`），并把内层 `TextBox.ContextMenu` 置 null。判据：深色主题下右键该控件，菜单项文案随 `UI4MultiLanguage` 当前语言，且不是系统菜单。

## 19.5 UI4FlipTextBlock：翻牌数字

- `FontSize` 静态构造里被改为 **60**（`:173-177`），不是 `Control` 的 12。
- `CardBackground`/`CardForeground`/`CardBorderBrush`/`ShadowColor` 都是 **`Color`**（`UI4FlipTextBlock.cs:29-75`），不是 Brush。
- 主题刷新受 `ReadLocalValue` 保护（`:186-194`）：只要宿主显式赋值过 `CardBackground`，切主题就不再覆盖它。要「跟随主题」就**别在 XAML 里写这三个颜色**。
- 翻牌由内层文本变化触发（`:360-363`），动画时长 = `FlipRate` 秒（默认 0.3，`:16`）。
- 上下两半用 `RenderTargetBitmap` 以 **固定 96 DPI** 光栅化后 `CroppedBitmap` 裁切（`:379-403`）⇒ 125%/150% 缩放下翻牌过程可能与静态字形错位、发虚。这是显示型控件，**不要当实时计数器**：`GetRenderTop/Bottom` 内部 `Dispatcher.Invoke(..., Render)` 每次都重绘整棵 `_g_main`。
- 卡片本体是内部 `UI4Panel`（`:202`），因此 19.6 里 `UI4Panel` 的 `BorderBrush` 陷阱会间接影响它——但 `CardBorderBrush` 走的是 `BorderColor`，正常。

## 19.6 UI4TextBox：输入框

默认值（全部可从源码核对）：`CornerRadius` 6、`InnerPadding` **(12,5,32,5)**（`:85-87`）、`ShowClearButton` false、`FontSize` 15（`:161`）、`EditBackground` 白、`TextColor` #1E1E1E。构造函数把 7 个外观 DP 绑到资源桥（`:171-178`），所以**引用式跟随主题且支持作用域**。

三个实操要点：

1. **占位符位置写死。** 占位符 `TextBlock` 用固定 `Margin(14,0,0,0)`（`:364`），而文本宿主 `PART_ContentHost` 的 `Padding` 绑到 `InnerPadding`（经 `InnerPaddingConverter`，`:343-345`、`:484-496`）。改 `InnerPadding.Left` 后占位符不动 ⇒ 与光标错位 `InnerPadding.Left − 14`。要改内边距就同时换掉占位符方案（自己叠一层 `TextBlock`）。
2. **`ShowClearButton=false` 时右边距自动收回。** `InnerPaddingConverter` 把 Right 收成 Left 值（`:488-493`），避免右侧留一条空带。默认 Right=32 就是给「×」按钮（宽 26，`:415-416`）预留的。
3. **清除按钮写的是 `Text`。** `clearBtn` 的 Click 处理器为 `Text = string.Empty`（`:431`）⇒ 若 `Text` 是 TwoWay 绑定，点清除会回写数据源。只想清显示就别开 `ShowClearButton`。

内置右键菜单：`InitCustomMenu()`（`:296-330`）建 `UI4ContextMenu{Width=170}`，项为 Undo/Cut/Copy/Paste/Delete/SelectAll，并把原生 `ContextMenu` 置 null。菜单文案在 **构造期**从 `UI4MultiLanguage.Get` 取（见第 22 章），切语言后需重建控件。

同文件还公开了三个转换器，宿主 XAML 可直接复用，不必自己写：`BoolToVisibilityConverter`(`:459`)、`PlaceholderVisibilityConverter`(`:470`)、`InnerPaddingConverter`(`:484`)。

## 19.7 UI4PasswordBox：它继承 TextBox

类注释（`:17-30`）与声明（`:31`）都确认基类是 `System.Windows.Controls.TextBox`，不是 `PasswordBox`。掩码是**把 `Text` 写成等长的 `PasswordChar` 串**实现的（`UpdateDisplay()` `:486-512`），真实值在私有字段 `_password`（`:217`），每次编辑同步写回 `Password` DP（`:425/:448/:470`）。

后果与用法：

- **读值只读 `Password`。** `Text` 在密文态是 `●●●●`；`IsPasswordMode=false` 时就是明文本身。别把 `Text` 绑出去。
- **`Password` 可 TwoWay 绑定**，这是它相对原生 `PasswordBox`（`Password` 非 DP）的唯一实用优势。
- **明文 string 常驻内存。** 源码安全注释（`:123-132`）明说可能被内存转储/调试器读取，高安全场景应改回原生 `PasswordBox`。判定边界：登录/支付等凭据类界面若要求「不可从转储恢复口令」→ 用原生控件 + 自己写样式；只做观感一致的普通表单 → 可用本控件。
- **`Unloaded` 只清私有字段。** `:311` 把 `_password` 置空，但**没有清 `Password` DP**，也没清绑定源 ⇒ 切页/关窗后 DP 仍是明文；再 `Loaded` 时 `:293-294` 用空的 `_password` 重算显示 ⇒ 出现「框里是空的、`Password` 却有值」的不一致。要真清，显式调用 `ClearPassword()`（`:549-556`，同时清 `_password`/`Password`/`Text`）。
- **按住显形。** `ShowPasswordButton`（默认 true，`:96`）的按钮是 `MouseDown` 显示 / `MouseUp`·`MouseLeave` 隐藏（`:558-598`，靠 `CaptureMouse`）。显示时会把公开 DP `IsPasswordMode` 翻成 `false`（`:525-531`），松开翻回（`:533-539`）⇒ 别把 `IsPasswordMode` 当「用户设置：显示明文」的持久状态；`LostFocus` 也会触发 `HidePlainText`（`:239`），所以宿主显式设 `IsPasswordMode="False"` 后一旦失焦就被改回 `True`。
- 键鼠被接管：`Ctrl+C`/`Ctrl+X` 在密文态被拦（`:384-413`），`Esc` 映射为全选；右键菜单在模式切换时重建，密文态只给「粘贴/全选」（`:639-687`）。外观 DP 与 `UI4TextBox` 同名同默认（含 `InnerPadding (12,5,32,5)`），但 `Padding` 直接绑 `InnerPadding`（`:693`），无清除按钮收回逻辑。

## 19.8 UI4CodeEditor：AvalonEdit 包装

构造函数（`UI4CodeEditor.cs:31-65`）已替你设好：C# 高亮、`ShowLineNumbers`、`WordWrap`、Consolas 14、`ConvertTabsToSpaces=true`、`IndentationSize=4`、`EnableRectangularSelection=false`（矩形选择关），并预挂 `UI4ContextMenu{Width=200}`（Undo/Redo/Cut/Copy/Paste/Delete/SelectAll，`:48-61`）→ **不要重复 `Attach` 第二个菜单**（右键会弹两层，`UI4ContextMenu.Attach` 只是加 `MouseRightButtonUp` 处理器）。

宿主只需三件事：

```csharp
UI4CodeEditor editor = new UI4CodeEditor();
editor.IsReadOnly = true;                                  // TextEditor 原生属性
editor.SyntaxHighlighting =
    ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinition("XML");
editor.Text = File.ReadAllText(path);                      // Text 是普通 string，可读可写
```

XAML 里只要出现 `<ui:UI4CodeEditor/>`，`ICSharpCode.AvalonEdit.dll` 就是**运行必需**（缺失表现为首次触碰该类型的 `FileNotFoundException`，见第 24 章）。高亮定义名要与 `HighlightingManager` 注册名逐字一致（`"C#"`、`"XML"`、`"JSON"`），拼错只得到无高亮且不报错。

## 19.9 一屏输入表单（可抄）

前缀约定：`xmlns:ui="clr-namespace:StartUI4Controls;assembly=StartUI4Controls"`。标签用原生 `TextBlock`（`UI4TextBlock` 不跟随主题，见 19.4）。

```xml
<Border Background="{DynamicResource UI4.Brush.Surface}"
        BorderBrush="{DynamicResource UI4.Brush.Border}" BorderThickness="1"
        CornerRadius="10" Padding="20" MaxWidth="460" HorizontalAlignment="Left">
    <StackPanel>
        <TextBlock Text="账号" Margin="0,0,0,4"
                   Foreground="{DynamicResource UI4.Brush.Text}"/>
        <ui:UI4TextBox x:Name="AccountBox" Height="36" ShowClearButton="True"
                       PlaceholderText="字母、数字、下划线，4-20 位" Margin="0,0,0,4"/>
        <TextBlock x:Name="AccountHint" Text="必填" FontSize="12" Margin="0,0,0,12"
                   Foreground="{DynamicResource UI4.Brush.Icon}"/>

        <TextBlock Text="口令" Margin="0,0,0,4"
                   Foreground="{DynamicResource UI4.Brush.Text}"/>
        <ui:UI4PasswordBox x:Name="PwdBox" Height="36"
                           PlaceholderText="至少 8 位" Margin="0,0,0,4"/>
        <TextBlock x:Name="PwdHint" Text="必填" FontSize="12" Margin="0,0,0,12"
                   Foreground="{DynamicResource UI4.Brush.Icon}"/>

        <StackPanel Orientation="Horizontal" HorizontalAlignment="Right">
            <ui:UI4Button x:Name="ResetButton" Content="重置" Width="88" Height="32"
                          Margin="0,0,10,0" Click="ResetButton_Click"/>
            <ui:UI4Button x:Name="SaveButton" Content="保存" Width="88" Height="32"
                          Click="SaveButton_Click"/>
        </StackPanel>
    </StackPanel>
</Border>
```

```csharp
private void SaveButton_Click(object sender, RoutedEventArgs e)
{
    string account = AccountBox.Text == null ? string.Empty : AccountBox.Text.Trim();
    bool accountOk = account.Length >= 4 && account.Length <= 20;
    AccountHint.Text = accountOk ? string.Empty : "账号需 4-20 位";
    AccountHint.Foreground = (Brush)FindResource(
        accountOk ? "UI4.Brush.Icon" : "UI4.Color.TextForeground");

    string pwd = PwdBox.Password == null ? string.Empty : PwdBox.Password;
    bool pwdOk = pwd.Length >= 8;
    PwdHint.Text = pwdOk ? string.Empty : "口令至少 8 位";

    if (!accountOk || !pwdOk)
    {
        PwdBox.ClearPassword();          // 失败也要清，别让明文留在内存
        return;
    }
    // 业务写入见第 23 章；这里绝不把 pwd 写日志、写绑定源
}

private void ResetButton_Click(object sender, RoutedEventArgs e)
{
    AccountBox.Clear();
    PwdBox.ClearPassword();
    AccountHint.Text = "必填";
    PwdHint.Text = "必填";
}
```

上面刻意演示两条：口令只从 `Password` 读；离手前 `ClearPassword()`。

## 19.10 尺寸与可读性

输入类控件的文本可用高度按同一种几何算法算（方法源自 `常见问题清单/UI4ComboBox-文本高度显示不全.md:250-285`）：

```
可用高度 = Height − 上下边框(2) − InnerPadding.Top − InnerPadding.Bottom
行高 ≈ FontSize × 1.2          // WPF 的 FontSize 单位是 DIP，不是 pt
舒适条件：可用高度 ≥ 行高   ⇒  Height ≥ FontSize × 1.2 + Top + Bottom + 2
下限条件：可用高度 ≥ FontSize ⇒  Height ≥ FontSize + Top + Bottom + 2
```

按各控件现行默认内边距代入（右列是 `FontSize=15` 的推荐 `Height`）：

| 控件 | 默认 InnerPadding | 常数和 | 舒适 Height(FS=15) | 下限 Height |
|---|---|---|---|---|
| `UI4TextBox` / `UI4PasswordBox` | 12,5,32,5 | 12 | ≥ 30 | ≥ 27 |
| `UI4ComboBox` | 12,4,30,4 | 10 | ≥ 28 | ≥ 25 |

Demo 用 `Height="36"`（`MainWindow.xaml:94/:146`）留了余量，宿主照 36 起步即可。注意 `常见问题清单` 里 `Height ≥ FontSize×1.2 + 22` 的常数 22 对应**旧版** ComboBox 内边距 (12,10,30,10)；按现在的 (12,4,30,4) 应为 +10，别照抄。

## 19.11 推荐做法（Do / Don't）

| Do | Don't |
|---|---|
| 绑定 `UI4TextBlock.Content`（不触发重建） | 绑定 `UI4TextBlock.Text`（每次变更重建 `Style`） |
| 在容器上写 `TextElement.Foreground="{DynamicResource UI4.Brush.Text}"` 喂 `UI4TextBlock` | 以为 `UI4TextBlock` 会自动跟随主题 |
| 渐变文字用 `Foreground` + `LinearGradientBrush` | 给 `GradientStart`/`GradientEnd` 赋值（死属性） |
| `ClearValue(UI4TextBlock.FontSizeProperty)` | `ClearValue(Control.FontSizeProperty)`（清错一份） |
| 只读 `PwdBox.Password`，离手 `ClearPassword()` | 读 `PwdBox.Text`、依赖 `Unloaded` 清密码（DP 不会被清） |
| 让 `IsPasswordMode` 归控件自己管 | 把它当「显示明文」开关来双向绑定 |
| 代码语言/只读用 AvalonEdit 原生属性 | 再 `Attach` 一个 `UI4ContextMenu` 到 `UI4CodeEditor` |
| `FontSize` 定了先按 19.10 公式算 `Height` | 直接抄 `Height="24"` 然后抱怨字被切 |

## 19.12 坑与排错

| 症状 | 根因 | 处置（含判据） |
|---|---|---|
| CS1503 无法从 `UI4TextBlock` 转 `TextBlock` | `UI4TextBlock : ContentControl`（`:34`），不是 `TextBlock` | 形参改 `FrameworkElement`/`ContentControl`；判据：`typeof(System.Windows.Controls.TextBlock).IsAssignableFrom(typeof(UI4TextBlock))` 为 false |
| 深色主题下 `UI4TextBlock` 黑字看不见 | 无主题订阅，`Foreground` 默认 null 且不写内层 | 用 19.4 方案 A/B；判据：切 Dark 后读内层 `PART_TextBox` 的 `Foreground`，应为浅色 |
| `UI4TextBlock` 显示 `System.Windows.Controls.TextBlock…` | `Content` 被 `ToString()`（转换器 `:15-32`） | 换成 `Text` 绑字符串，或改用原生 `ContentControl` |
| 设了 `GradientStart/End` 没变化 | 两属性未被 `BuildTextStyle` 读取 | 改传 `Foreground` 渐变 |
| 改 `FontSize` 后部分代码不生效 | 读写用了基类 DP（`new` 隐藏歧义） | 显式 `UI4TextBlock.FontSizeProperty` |
| 占位符与光标不在同一处起笔 | 占位符固定 `Margin(14,0,0,0)`（`:364`），文本用 `InnerPadding` | 恢复默认 `InnerPadding.Left`，或自绘占位符 |
| `PlaceholderText` 不显示 | `Text` 非空（转换器只看 `Text`，`:470-476`） | 检查绑定初始值；判据：断点看 `Text.Length == 0` |
| 点「×」后绑定源被清空 | Click 写的是 `Text`（`:431`） | 关 `ShowClearButton` 或改为单向绑定 |
| 密码框切页后 `Password` 仍有值但显示空 | `Unloaded` 只清 `_password`（`:311`） | 导航离开前显式 `ClearPassword()` |
| 按住「眼睛」时自己的逻辑读到 `IsPasswordMode=false` | 显形会翻公开 DP（`:525-531`） | 别绑该 DP；判据：按住后读回 `true` 才说明你绑错方向 |
| 翻牌动画在高 DPI 下发虚/错位 | `RenderTargetBitmap` 固定 96 DPI（`:382-385`） | 只用于低频展示；高频数字改原生 `TextBlock` |
| 启动即 `FileNotFoundException: ICSharpCode.AvalonEdit` | XAML 触及 `UI4CodeEditor` 即需该 DLL | 按第 24 章最小产物集补 DLL；判据：输出目录存在该 DLL |

## 19.13 完成判据

1. 全项目搜索 `GradientStart`（仅允许出现在 `UI4Button`/`UI4Slider`/`UI4ProgressBar` 等真用它作渐变的控件上）、`UI4TextBlock.FontSizeProperty` 之外的基类引用：0 命中。
2. `UI4TextBlock` 所在容器有 `TextElement.Foreground` 或自身 `Foreground` 的 `DynamicResource`：逐个卡片核对，Light/Dark/HighContrast 三主题下文字与底色对比不倒置。
3. 输入表单在 `FontSize=15`、`Height=36` 下渲染：上下沿无截断（把 `Height` 改 26 复现截断，以确认自己会算）。
4. 口令流程：输入 → 保存失败 → `ClearPassword()` → `UI4PasswordBox.Password.Length == 0`（不是靠 `Unloaded`）。
5. `UI4CodeEditor` 页能改 `SyntaxHighlighting` 且只有一层右键菜单。
6. 运行期日志（宿主自己的 `*-errors.log`）为空；`UI4FlipTextBlock` 连续点击不产生 `demo-errors.log` 类异常。

---

# 第 20 章 数据展示：列表、卡片视图与下拉

> 本章解决：在新应用里正确选型并摆放列表/卡片视图/下拉，保证文字不裁切、不溢出，并在深色页里处理“不跟随主题”的控件。
> 读者：AI Agent（要在本项目之外新建/续写 WPF 业务代码）
> 前置章节：第 13 章（令牌与资源桥）、第 12 章（主题生效时序）、第 19 章（输入控件与尺寸纪律）

## 20.1 目标与验收

- 列表用 `UI4ListBox`（可带序号/圆点）；卡片流用 `UI4ListView`（单行）或 `UI4GridView`（自适应多列）。
- 下拉 `UI4ComboBox` 的选中项文字垂直不被削、横向不溢出箭头列。
- 深色页里所有展示控件的文字都可读；不跟随主题的控件由宿主显式接管颜色。
- 数据表选型给出结论与代价（见 20.6）。

## 20.2 源码依据（必读）

- `src\StartUI4Controls\UI4ListBox.cs:22`（`enum ListStyleType{None,Disc,Number}`）、`:32`（`IndexPlusOneConverter`）、`:68-73`（`BorderNormalColor` 默认 `#2563EB`）、`:107/120`（`ItemPadding(12,8,12,8)/ItemCornerRadius`）、`:264-268`（公开 `RefreshTheme()`）、`:338-342`（`BeginInvoke` 逃逸作用域）。
- `src\StartUI4Controls\UI4ListView.cs:14`（`: ListBox`）、`:16-40`（`ItemWidth/ItemHeight` 默认 NaN）、`:55-127`（`Item*` 外观）、`:194-203`（`HoverScale`）、`:239`（禁横向滚动）、`:378`（`CreateScrollViewerStyleFromXaml`）。
- `src\StartUI4Controls\UI4GridView.cs:15`（`: ListBox`）、`:220-254`（`SizeChanged`→`UpdateColumns`，`ItemWidth` 为 NaN/≤0 直接 return）。
- `src\StartUI4Controls\UI4ComboBox.cs:55-120`（`FocusGradientStart/End`、`DropCornerRadius`、`InnerPadding(12,4,30,4)`）、`:26/165`（注释里的 `Hover/FocusBorderColor` 不存在）、`:314`（`ClipToBounds=true`）、`:386`（popup `MinWidth←ActualWidth`）、`:406`（`IsEditable`）。
- `常见问题清单\UI4ComboBox-文本高度显示不全.md`、`常见问题清单\UI4ComboBox-选中项文本溢出.md`。
- `samples\StartUI4Demo\MainWindow.xaml:229-277`（三种 `ListStyleType` + `ListView/GridView` 绑定）、`MainWindow.xaml.cs:64-73`（`List<CardItem>`，无 INPC）。

## 20.3 UI4ListBox：带样式的列表

`UI4ListBox : ListBox`（命令式，跟随主题）。

- 列表样式：`ListStyleType` = `None`（无装饰）/`Disc`（圆点）/`Number`（序号徽章）。序号由 `AlternationIndex` + `IndexPlusOneConverter`（`:32`）从 1 开始显示；徽章底色用 `NumberCircleBackground`。
- 外观：`ItemPadding`（默认 `12,8,12,8`）、`ItemCornerRadius`（6）、`CornerRadius`（6）、`PanelBackground`、`TextColor`、`HoverBackground/HoverForeground/PressedBackground/PressedForeground`。
- `BorderNormalColor` DP 默认是 `#2563EB`（`:68-73`，一个强调蓝），但构造函数里 `SyncThemeColors()` 会用主题 `BorderNormal` 覆盖它，除非你显式赋过值——所以别指望默认就是那个蓝。
- `RefreshTheme()`（`:264-268`）：供“未挂到视觉树、收不到 Loaded”的场景手动刷新，典型是 `Popup` 里预构建的内容（`UI4ContextMenu` 弹出前就调它）。业务里若把 `UI4ListBox` 放进未显示的 Popup，弹出前调一次 `RefreshTheme()`。
- 作用域陷阱：`Number` 徽章的按下态清除走 `Dispatcher.BeginInvoke(..., Input)`（`:338-342`）。该回调在 `UseTheme` 作用域栈弹出后才执行，若你正处于局部主题作用域，它会读到**全局主题**色。判据：在局部作用域页里发现序号/按下色与卡片色不一致，即此路径；处置：作用域下优先用引用式控件，或避免依赖该回调色。

```xml
<ui:UI4ListBox Width="200" Height="150" ListStyleType="Number"
               NumberCircleBackground="{DynamicResource UI4.Brush.Accent}">
    <ListBoxItem>第一项</ListBoxItem>
    <ListBoxItem>第二项</ListBoxItem>
</ui:UI4ListBox>
```

## 20.4 UI4ListView 与 UI4GridView：卡片视图（都不跟随主题）

两者都 `: ListBox`，都是“卡片化展示器”，**默认都不跟随主题**（无 `TrackControl`），构造函数里把 `ItemBackground` 写死白色系。

- 共同 DP：`ItemWidth/ItemHeight`（默认 NaN）、`ItemCornerRadius`(12)、`ItemBackground`(**Brush**)、`ItemBorderBrush`/`ItemHoverBorderBrush`(**Color**)、`ItemBorderThickness`、`ItemPadding`、`ItemMargin`、`HoverScale`、`ShadowColor/BlurRadius/Depth/Opacity`、`HoverAnimationDuration`。
- `UI4ListView`：单行平铺，内部 `ScrollViewer` 禁横向滚动（`:239`），靠 `HoverScale` 做悬停微放大。适合“一排卡片 + 溢出即截断”。
- `UI4GridView`：`SizeChanged` 时 `UpdateColumns()` 自动算列数，公式 `columns = max(1, floor((ActualWidth - 竖滚动条宽) / (ItemWidth + ItemMargin.Left + ItemMargin.Right)))`（`:240-244`）；**`ItemWidth` 为 NaN 或 ≤0 时直接 return，不排版**——所以用 GridView 必须给正数 `ItemWidth`。
- 深色页处置方案：因为两者不跟随主题，切 Dark 后卡片仍是白底。三种做法，按代价从低到高：
  1. 宿主在 `ThemeChanged` 里遍历这些控件，按 `UI4Theme.CurrentMode` 手动设 `ItemBackground`/`TextColor`（Demo 的做法是干脆常亮浅色卡片，见 `MainWindow.xaml:248 ItemBackground="White"`）。
  2. 把它们放进一块带反衬底的 `UI4ThemeScope.Theme` 区域，卡片继续浅色、周围深色，形成“深色页里的浅色卡片岛”。
  3. 换用跟随主题的 `UI4ListBox`（命令式）承载同样数据。

```xml
<ui:UI4GridView ItemsSource="{Binding Cards}" ItemWidth="230" ItemHeight="170"
                HoverScale="1.06" ShadowDepth="15" ShadowOpacity="0.3">
    <ui:UI4GridView.ItemTemplate>
        <DataTemplate>
            <Border Background="{Binding Background}" CornerRadius="10">
                <StackPanel Margin="16">
                    <ui:UI4TextBlock Content="{Binding Title}"/>
                    <ui:UI4TextBlock Content="{Binding Description}" TextWrapping="Wrap"/>
                </StackPanel>
            </Border>
        </DataTemplate>
    </ui:UI4GridView.ItemTemplate>
</ui:UI4GridView>
```

## 20.5 UI4ComboBox：焦点渐变与尺寸公式

`UI4ComboBox : ComboBox`（命令式，跟随主题）。外观 DP：`CornerRadius`、`BorderNormalColor`、**焦点边框是渐变** `FocusGradientStart/FocusGradientEnd`（`ThemeSync` 跟随 `Accent`/`AccentEnd`）、`EditBackground`(→`Surface`)、`TextColor`(→`TextForeground`)、`InnerPadding`、`DropCornerRadius`（下拉面板圆角）、支持 `IsEditable`。

> 文档谎言提醒：类注释里的 `HoverBorderColor/FocusBorderColor` 在本控件**不存在**（那是 `UI4TextBox` 的 DP）。别照注释写 XAML，会报未知属性。

**尺寸公式**（下拉文字被切的根治）。可用高：

    文本可用高度 = Height − 2×边框(1) − InnerPadding.Top − InnerPadding.Bottom
    行高 ≈ FontSize × 1.2   （FontSize 单位是 DIP，不是 pt）

- 舒适：`可用高度 ≥ 行高` ⇒ `Height ≥ FontSize×1.2 + InnerPadding.Top + InnerPadding.Bottom + 2`。
- 下限：`Height ≥ FontSize + 2×边框 + InnerPadding.Top + InnerPadding.Bottom`。
- 注意默认值：`UI4ComboBox.InnerPadding` 源码默认是 `12,4,30,4`（`:104`，上下各 4）；两份 FAQ 文档按**旧默认 `12,10,30,10`** 推导，其“`舒适 Height ≥ FontSize×1.2 + 22` / `下限 ≥ FontSize + 22`”里的常数 22 = 10+10+2。用当前默认（上下各 4）时常数应取 `4+4+2=10`，留白更充裕。下表按 FAQ 旧默认（垂直各 10）给出，若你保持源码默认，可各再降约 12。

| FontSize (DIP) | 行高≈1.2× | 建议 Height（旧默认，舒适） | 最小 Height（旧默认） |
|---|---|---|---|
| 12 | 14.4 | 38 | 36 |
| 15（默认） | 18 | 42 | 39 |
| 16 | 19.2 | 44 | 40 |
| 18 | 21.6 | 46 | 44 |
| 20 | 24 | 48 | 46 |

**文本溢出三处修复（库已内置，宿主要知道对应自己该设什么）**：

1. `contentHostGrid` 已设 `ClipToBounds=true`（`:314`）——超宽内容被裁进边界，不再溢出箭头列。
2. 选中项默认模板已带 `TextTrimming="CharacterEllipsis"` + `ToolTip`——长文本省略号截断、悬停看全文；宿主给自定义 `ItemTemplate` 时，自己也要在 `TextBlock` 上写 `TextTrimming` 和 `ToolTip`。
3. 下拉 `Popup` 宽度绑定改为 `MinWidth ← ActualWidth`（`:386`）——面板至少与控件等宽，允许内容撑得更宽。宿主若想让面板更宽，设 `MinWidth`/`MaxDropDownHeight`，别去锁死 `Width`。

## 20.6 绑定数据源与“数据表”选型

- `ItemTemplate` vs `DisplayMemberPath`：字符串列表用 `DisplayMemberPath` 最省；对象列表用 `ItemTemplate`（Demo 用 `ItemTemplate`，见 `MainWindow.xaml:251`）。两者别同时指望——`ItemTemplate` 优先。
- `List<T>` 无 INPC 的刷新陷阱：Demo 的 `Cards` 是普通 `List<CardItem>`（`MainWindow.xaml.cs:64-73`），增删改不会自动刷新界面。要么整体重设 `ItemsSource`（触发一次重绘），要么换 `ObservableCollection<T>` 并让项实现 `INotifyPropertyChanged`（第 23 章）。
- 虚拟化：`UI4ListBox` 大列表默认继承 `ListBox` 的 `VirtualizingStackPanel`；但 `UI4ListView` 用单行布局、`UI4GridView` 用 `UniformGrid`，**UniformGrid 不支持行/列虚拟化**——上千条卡片会全量实例化。大数据优先 `UI4ListBox` 或分页。
- 数据表（grid/table）选型结论：`UI4DataGrid` 是 **internal，宿主不可使用**（其 SQLite 后端也是内部死码）。要做“表格”只有两条路：
  1. 用原生 `DataGrid` + 自染样式（`RowBackground`/`BorderBrush` 用 `DynamicResource` 令牌）——列多、排序/编辑需求强的场景，代价是主题跟随要自己维护。
  2. 用 `UI4ListView`/`UI4GridView` 做“卡片式”伪表格——视觉更现代，代价是不跟随主题（见 20.4）且非真表格（无单元格编辑/排序）。
  - 简单键值行列表：直接用 `UI4ListBox`（`ListStyleType=None`）。

## 20.7 Do / Don't

- Do：`UI4GridView` 一定给正数 `ItemWidth`，否则不排版；`UI4ListView` 用于单行卡片流。
- Do：`ComboBox` 的 `Height` 按 20.5 公式给，别用默认 36 配大字号。
- Do：自定义 `ItemTemplate` 时自带 `TextTrimming` + `ToolTip`，配合 `ClipToBounds`。
- Don't：别照 `UI4ComboBox` 注释写 `Hover/FocusBorderColor`（不存在）。
- Don't：别把不跟随主题的 `ListView/GridView` 直接丢进深色页就以为会变暗。
- Don't：别指望 `List<T>` 自动刷新；别在 `UI4GridView` 上放几千张卡片。

## 20.8 坑与排错

- 症状：下拉选中文字上/下被削一截。根因：`Height − 2 − Top − Bottom < FontSize×1.2`。处置：按 20.5 抬高 `Height` 或减小 `InnerPadding` 上下分量；`p3verify [H]` 证明未显式赋值时 `TextColor` 已跟随主题，若仍异常多半是尺寸不是颜色。
- 症状：长文本溢出到箭头列外/突破圆角。根因：给了不带截断的自定义模板。处置：模板 `TextBlock` 加 `TextTrimming` + `ToolTip`；确认没关掉 `ClipToBounds`。
- 症状：`UI4GridView` 空白不显示卡片。根因：`ItemWidth` 未设（NaN）→`UpdateColumns` 直接 return。处置：设正数 `ItemWidth`。
- 症状：切 Dark 后卡片仍白底。根因：`ListView/GridView` 不跟随主题。处置：见 20.4 三种方案之一。
- 症状：序号徽章按下后颜色与局部作用域卡片不符。根因：`BeginInvoke` 回调读到全局主题（`:338`）。处置：作用域页改用引用式控件。
- 症状：数据改了列表不动。根因：`List<T>` 无 INPC。处置：重设 `ItemsSource` 或换 `ObservableCollection<T>`。

## 20.9 完成判据（自查）

- [ ] 三种 `ListStyleType` 按需求选定；作用域页未依赖 Number 徽章回调色。
- [ ] `UI4GridView` 设了正数 `ItemWidth`；`UI4ListView` 确认单行且横向不滚。
- [ ] `UI4ComboBox` 的 `Height` 满足可用高公式，长文本项有省略号 + ToolTip。
- [ ] 深色页里的 `ListView/GridView` 已用（反衬岛 / 手动染色 / 换 ListBox）之一处理。
- [ ] 数据源刷新方式（重设 ItemsSource 或 ObservableCollection）明确；未使用 internal 的 `UI4DataGrid`。
- [ ] `demo-errors.log` 无新增异常。

---

# 第 21 章 容器、滚动与「主题盲区」清单

> 本章解决：给新应用挑对容器与滚动件，并逐项识别“切主题后不会变”的控件盲区，给出可复制的四套页面骨架。
> 读者：AI Agent（要在本项目之外新建/续写 WPF 业务代码）
> 前置章节：第 13 章（令牌与资源桥）、第 14 章（作用域 UI4ThemeScope）、第 20 章（列表/卡片/下拉）

## 21.1 目标与验收

- 需要卡片外观才用 `UI4Panel`；纯布局用原生 `Grid/Border`，不误用写死浅底渐变的 `UI4Grid`。
- 需要平滑滚动才用 `UI4ScrollViewer`，并接受其滚动条样式写死浅色。
- 手里有一张“主题盲区总表”，任何控件切主题后不变都能对上号并给出处置。
- 四套页面骨架（列表/表单/详情/设置）可直接粘贴，全部用 `DynamicResource` 令牌。

## 21.2 源码依据（必读）

- `src\StartUI4Controls\UI4Panel.cs:24`（`: ContentControl, IThemeAware`）、`:46-57`（`BorderColor` 是 `Color`）、`:59-70`（`HoverBorderBrush` 是 `SolidColorBrush`）、`:72-131`（`Shadow*/ContentPadding`）、`:137-157`（`HoverAnimationDuration/HoverScale`）、`:21`（注释提的 `Title` 属性不存在）、`:170-173`（`BorderThicknessProperty.OverrideMetadata`，非 `new` 隐藏）。
- `src\StartUI4Controls\UI4Grid.cs:9-21`（构造函数写死 `#E1ECF5→#FFFFFF` 渐变，主题中性）。
- `src\StartUI4Controls\UI4ScrollViewer.cs:21`（`: ScrollViewer`）、`:29-40`（`IsSmoothScrollEnabled`）、`:62-110`（`e.Handled=true` + `CompositionTarget.Rendering` 约 100ms 缓动）、`:117-129`（`SmoothScrollTo*`）、`:46-54`（样式来自 `Internal/ScrollBarResources`）。
- `src\StartUI4Controls\Internal\ScrollBarResources.cs:13`（internal）、`:362`（`BorderBrush=LightGray`，写死浅色）。

## 21.3 UI4Panel：卡片容器

`UI4Panel : ContentControl`（引用式/命令式混合，跟随主题）。它是“带圆角+边框+阴影+悬停放大的卡片外壳”。

- `CornerRadius` 默认 12；`ContentPadding` 默认 `0`（内容内边距）。
- 边框用两个不同型：`BorderColor` 是 **`Color`**（驱动悬停 `ColorAnimation`），`HoverBorderBrush` 是 **`SolidColorBrush`**（`:46-70`）。想静态改边框色就设 `BorderColor`；想要悬停变色就设 `HoverBorderBrush`。
- `ShadowDepth/ShadowBlurRadius/ShadowOpacity/ShadowColor`、`HoverScale`、`HoverAnimationDuration` 组合出“抬起 + 悬停微放大”的卡片观感。
- 澄清：类注释提到的 `Title` 属性**不存在**；`事实速查` 曾说它用 `new` 隐藏 `BorderBrush/BorderThickness`——源码里 `UI4Panel` **并未** `new` 这两个属性，只对继承自 `Control` 的 `BorderThicknessProperty` 做了 `OverrideMetadata`（`:172`）。`Control.BorderBrush`（Brush）仍在，模板内部另用 `BorderColor`（Color）画边框。别去 XAML 里写 `ui:UI4Panel.BorderBrush` 期望它等价于 `BorderColor`。

```xml
<ui:UI4Panel CornerRadius="12" ContentPadding="16"
             HoverScale="1.02" ShadowDepth="10" ShadowBlurRadius="15" ShadowOpacity="0.12"
             BorderColor="{DynamicResource UI4.Color.BorderNormal}">
    <ui:UI4TextBlock Text="卡片内容" FontSize="16"/>
</ui:UI4Panel>
```

## 21.4 UI4Grid 与 UI4ScrollViewer

**UI4Grid : Grid**：构造函数把 `Background` 写死为 `#E1ECF5→#FFFFFF` 竖向浅渐变（`UI4Grid.cs:9-21`），**主题中性**——任何主题下都保持浅色，且没有任何外观 DP 可关。结论：几乎不该用它做布局容器。要做布局用原生 `Grid`；要浅底卡片用 `UI4Panel`；要跟随主题的底用原生 `Border Background="{DynamicResource UI4.Brush.Surface}"`。

**UI4ScrollViewer : ScrollViewer**：唯一差异是平滑滚动。

- `IsSmoothScrollEnabled`（默认 true）。滚轮处理里 `e.Handled=true`，把目标偏移缓动到 `CompositionTarget.Rendering` 上跑约 100ms（`AnimationDuration=100`，EaseOutCubic）。
- 代码驱动：`SmoothScrollToVerticalOffset(double)` / `SmoothScrollToHorizontalOffset(double)`（`:117-129`），内部对 `ScrollableHeight/Width` 做夹取。
- **不跟随主题**：它自己不设外观色；滚动条样式来自 `Internal/ScrollBarResources`，其 `BorderBrush=LightGray` 等写死浅色（`:362`）。深色页里滚动条会显得偏亮——处置见 21.5。
- 只在“确实需要平滑手感”时才用它；普通容器用原生 `ScrollViewer`（可被主题滚动条资源覆盖）。

## 21.5 主题盲区总表

> 下表列“切主题后默认不会自动变暗/变亮”的控件与已知盲区。处置优先级：改 `UI4ThemeScope` 反衬 < 换原生控件自染 < 宿主 `ThemeChanged` 手动刷 < 等 P2 批次② 令牌化。

| 控件 | 表现 | 影响 | 处置 |
|---|---|---|---|
| `UI4Grid` | 恒为浅渐变背景，无外观 DP | 深色页里是块亮斑 | 换原生 `Grid`/`Border` + 令牌背景 |
| `UI4ListView` / `UI4GridView` | `ItemBackground` 等硬编码浅色，不跟随 | 深色页卡片仍白底 | 反衬岛 / `ThemeChanged` 手动刷 / 改 `UI4ListBox` |
| `UI4Tab`（`UI4TabControl`） | 不跟随主题 | 页签栏色不变 | 反衬岛或自染 Header 区 |
| `UI4ScrollViewer` 滚动条 | 样式来自 internal `ScrollBarResources`，写死浅色 | 深色页滚动条偏亮 | 需要暗滚动条时用原生 `ScrollViewer` 并覆盖样式 |
| `UI4Button` | 所有主题恒为蓝→紫渐变（未令牌化） | 主题里按钮色不成对 | 需要语义色时改原生 `Button` 自染 |
| `UI4Radio.TextColor` / `UI4Slider.TrackBackground` | 默认 `Colors.Black` / 白色，与令牌不一致 | 深色下对比不足 | 显式绑令牌或避开默认值 |
| `UI4ComboBox` 焦点边框 | 依赖 `FocusGradientStart/End` | 已被显式赋值则脱离主题 | 别赋本地值 |

## 21.6 布局经验

- **StackPanel 拉伸 UI4Switch 类问题**：竖向 `StackPanel` 会给子元素无限高度并横向 Stretch，导致按宽度拉伸的控件（如 `UI4Switch` 轨道）出现轨道与滑块分离。上游已用 `ArrangeOverride` 把 `UI4Switch` 固定为 `SwitchWidth×SwitchHeight`，但放进 `StackPanel` 仍建议显式设 `HorizontalAlignment="Left"`，避免依赖被拉伸后的尺寸。
- **模板重建型 DP 别在动画频率下改**：命令式控件（`UI4ListBox`/`UI4ComboBox` 等）的外观 DP 一旦变化就走 `BuildXxxStyle()` 全量重建样式。`CornerRadius/BoxSize/InnerPadding` 这类“结构类”DP 重建模板；若在 `CompositionTarget.Rendering`/定时器里逐帧改它们会明显掉帧。动画期间只改颜色类（已引用式）或 `Value` 类 DP，结构类改一次到位。
- **`ContentControl` 系容器**（`UI4Panel`/`UI4TextBlock`/`UI4FlipTextBlock`）放文本时，优先用它们自己的 `Text`/`Content`，别在其内部再塞一个 `UI4TextBlock` 造成双重模板重建。

## 21.7 页面模板库（可直接粘贴，均用 DynamicResource 令牌）

XAML 前缀约定：`xmlns:ui="clr-namespace:StartUI4Controls;assembly=StartUI4Controls"`。

**列表页骨架**

```xml
<Grid Background="{DynamicResource UI4.Brush.Background}">
    <Grid.RowDefinitions><RowDefinition Height="Auto"/><RowDefinition Height="*"/></Grid.RowDefinitions>
    <ui:UI4TextBlock Grid.Row="0" Text="记录列表" FontSize="20" FontWeight="Bold" Padding="16"/>
    <ui:UI4ListBox Grid.Row="1" x:Name="Records"
                   PanelBackground="{DynamicResource UI4.Brush.Surface}"
                   TextColor="{DynamicResource UI4.Color.TextForeground}"
                   BorderNormalColor="{DynamicResource UI4.Color.BorderNormal}" Margin="16"/>
</Grid>
```

**表单页骨架**

```xml
<Border Background="{DynamicResource UI4.Brush.Background}" Padding="20">
    <StackPanel>
        <ui:UI4TextBlock Text="新建条目" FontSize="18" FontWeight="Bold" Margin="0,0,0,12"/>
        <ui:UI4TextBox x:Name="FTitle" Height="36" PlaceholderText="标题" Margin="0,0,0,8"/>
        <ui:UI4ComboBox x:Name="FType" Height="42" SelectedIndex="0" Margin="0,0,0,8">
            <ComboBoxItem>类型 A</ComboBoxItem><ComboBoxItem>类型 B</ComboBoxItem>
        </ui:UI4ComboBox>
        <StackPanel Orientation="Horizontal" HorizontalAlignment="Right" Margin="0,12,0,0">
            <ui:UI4Button Content="保存" IsDefault="True" Margin="0,0,8,0"/>
            <ui:UI4Button Content="取消" IsCancel="True"/>
        </StackPanel>
    </StackPanel>
</Border>
```

**详情页骨架**

```xml
<ScrollViewer Background="{DynamicResource UI4.Brush.Background}" Padding="20"
              VerticalScrollBarVisibility="Auto">
    <StackPanel>
        <ui:UI4TextBlock Text="详情标题" FontSize="22" FontWeight="Bold"/>
        <ui:UI4TextBlock Text="副标题" Foreground="{DynamicResource UI4.Brush.Text}"
                         HorizontalContentAlign="Left" Margin="0,4,0,16"/>
        <ui:UI4Panel ContentPadding="16" CornerRadius="12"
                     BorderColor="{DynamicResource UI4.Color.BorderNormal}">
            <ui:UI4TextBlock Text="正文内容……" TextWrapping="Wrap"/>
        </ui:UI4Panel>
    </StackPanel>
</ScrollViewer>
```

**设置页骨架**

```xml
<Border Background="{DynamicResource UI4.Brush.Background}" Padding="20">
    <StackPanel>
        <ui:UI4TextBlock Text="外观" FontSize="18" FontWeight="Bold" Margin="0,0,0,12"/>
        <Grid Margin="0,0,0,8">
            <ui:UI4TextBlock Text="深色模式" HorizontalContentAlign="Left"/>
            <ui:UI4Switch x:Name="DarkSwitch" IsOn="False" HorizontalAlignment="Right"/>
        </Grid>
        <Grid Margin="0,0,0,8">
            <ui:UI4TextBlock Text="跟随系统" HorizontalContentAlign="Left"/>
            <ui:UI4Switch x:Name="SysSwitch" HorizontalAlignment="Right"/>
        </Grid>
    </StackPanel>
</Border>
```

## 21.8 Do / Don't

- Do：卡片用 `UI4Panel`，布局用原生 `Grid/Border`，滚动默认用原生 `ScrollViewer`。
- Do：切主题后要检查 21.5 表里每一项，逐个确认是否落在你的页面上。
- Do：动画/高频回调里只改颜色类与 `Value` 类 DP，结构类 DP 一次设好。
- Don't：别用 `UI4Grid` 做需要变暗的容器；别对 `UI4Panel.BorderBrush`（Brush）与 `BorderColor`（Color）混用等价假设。
- Don't：别在局部主题作用域里放 `ListView/GridView/Tab` 后期待它们自动跟随。

## 21.9 坑与排错

- 症状：深色页出现一块浅蓝白渐变区。根因：用了 `UI4Grid`。处置：换原生 `Grid` + `Background="{DynamicResource UI4.Brush.Surface}"`。
- 症状：卡片在深色页仍白底。根因：`ListView/GridView` 不跟随主题。处置：见 21.5。
- 症状：深色页滚动条偏亮。根因：`UI4ScrollViewer`/内部 `ScrollBarResources` 写死浅色。处置：需要暗滚动条时换原生 `ScrollViewer`。
- 症状：改了 `InnerPadding`/`CornerRadius` 后卡顿。根因：结构类 DP 触发整样式重建。处置：一次设好，别放进逐帧动画。
- 症状：`UI4Switch` 轨道与滑块分离。根因：`StackPanel` 拉伸。处置：设 `HorizontalAlignment="Left"` 或放进有界的 `Grid` 单元格。

## 21.10 完成判据（自查）

- [ ] 页面里没有用于“需变暗容器”的 `UI4Grid`。
- [ ] `UI4Panel` 的边框只用了 `BorderColor`(Color)/`HoverBorderBrush`(Brush) 正确对应。
- [ ] 对照 21.5 总表逐项确认，命中项已选一种处置。
- [ ] 四套骨架的颜色全用 `DynamicResource` 令牌，无硬编码浅色。
- [ ] 高频动画路径下未修改结构类 DP。

---

# 第 22 章 菜单、对话框与系统托盘

> 本章解决：在新应用里搭出主菜单、右键菜单、消息/颜色对话框，并让托盘图标随应用正确创建与销毁。
> 读者：AI Agent（要在本项目之外新建/续写 WPF 业务代码）
> 前置章节：第 07 章（工程配置与 System.Drawing 引用）、第 13 章（令牌与资源桥）、第 23 章（本地化，菜单文案来源）

## 22.1 目标与验收

- 顶部菜单用 `UI4Menu` + `UI4MenuElementItem`；右键菜单用代码式 `UI4ContextMenu`。
- 消息框/颜色选择器都传 owner；关闭窗体时托盘图标先 `Collapsed` 再 `Dispose`，不留幽灵图标。
- `interact.ps1 -Scenario menu|ctx|tray` 能通过，`demo-errors.log` 无新增异常。

## 22.2 源码依据（必读）

- `src\StartUI4Controls\UI4Menu.cs:25`（`UI4Menu : Menu`）、`:27-88`（`BarBackground/ItemHoverBrush/PopupCornerRadius/TextForeground/PopupBackground/KeyTipForeground`）、`:292-347`（`UI4MenuElementItem : MenuItem` 的 `TextIcon/IconFontFamily/IconFontSize/IconForeground/KeyTip`）、`:354-362`（`UI4MenuSeparatorElement : Separator` 的 `SeparatorColor`）、`:13-15`（`ObjectIsStringConverter.Instance`）。
- `src\StartUI4Controls\UI4ContextMenu.cs:235`（`class UI4ContextMenu`，**非 Control**）、`:16-45`（`UI4MenuItemType` 七项 + `UI4MenuItem` POCO）、`:75-106`（`UI4MenuIcons`）、`:242-267`（`Width(160)/ItemPadding/BorderColor/Background/HoverBackground`，显式赋值后不再覆盖）、`:271-278`（构造订阅 `ThemeChanged`）、`:310-322`（`SyncBeforeOpen`）、`:324-373`（`AddItem/Attach/Detach`）、`:451-476`（`Open/Close`）。
- `src\StartUI4Controls\UI4MessageBox.cs:14-17`（`UI4MessageBoxButtons{OK,OKCancel}`）、`:268-285`（静态 `Show(...)`→`bool?`，owner null 时 CenterScreen）、`:33-37`（静态缓存字体/阴影）。
- `src\StartUI4Controls\UI4ColorPicker.cs:52`（`SelectedColor`）、`:790-794`（实例 `Show`→`ShowDialog`）、`:782-788`（静态 `ShowDialog`→`Color?`）。
- `src\StartUI4Controls\UI4NotifyIcon.cs:19-27`（`PopupActivationMode`）、`:57-105`（生命周期/100ms 定时器/`Application.Exit`）、`:134-162`（`MenuActivation/IconSource/ToolTipText`）、`:224-235`（`AddItem` 自动本地化）、`:429-441`（`SystemIcons.Application` 回退）、`:599-625`（`OnLoaded/OnUnloaded/OnVisibilityChanged`）、`:627-651`（`Dispose`）。
- `samples\StartUI4Demo\MainWindow.xaml.cs:75-91`（宿主 `UI4ContextMenu`）、`:180-214`（对话框）、`:262-301`（托盘开关/关闭清理）。

## 22.3 UI4Menu：顶部菜单条

`UI4Menu : Menu`（命令式，跟随主题）。外观 DP：`BarBackground`、`ItemHoverBrush`、`PopupBackground`、`PopupCornerRadius`、`TextForeground`、`KeyTipForeground`。子项类型：

- `UI4MenuElementItem : MenuItem`：`Text`（继承）+ `TextIcon`（码位字符串）+ `IconFontFamily`（默认 `Segoe UI Symbol`）+ `IconFontSize` + `IconForeground` + `KeyTip`（Alt 提示键）。
- `UI4MenuSeparatorElement : Separator`：`SeparatorColor`。

```xml
<ui:UI4Menu x:Name="MainMenu" BarBackground="{DynamicResource UI4.Brush.HeaderBackground}"
            TextForeground="{DynamicResource UI4.Brush.Text}">
    <ui:UI4MenuElementItem Header="文件" TextIcon="&#xE712;" IconFontFamily="Segoe MDL2 Assets" KeyTip="F">
        <ui:UI4MenuElementItem Header="新建" KeyTip="N"/>
        <ui:UI4MenuSeparatorElement/>
        <ui:UI4MenuElementItem Header="退出" Click="MenuExit_Click"/>
    </ui:UI4MenuElementItem>
</ui:UI4Menu>
```

图标不引第三方字体，直接用系统 `Segoe MDL2 Assets` 码位（Demo 约定）。

## 22.4 UI4ContextMenu：纯代码式右键菜单

`UI4ContextMenu` **不是 Control、不能写进 XAML**。用法固定为“new + AddItem + Attach”：

```csharp
_hostMenu = new UI4ContextMenu { Width = 190 };
_hostMenu.AddItem(new UI4MenuItem(
    UI4MenuItemType.Copy, "复制文本", UI4MenuIcons.Copy,
    () => CopySelected()));
_hostMenu.AddItem(UI4MenuItemType.Paste, () => PasteFromClipboard(),
    () => Clipboard.ContainsText());   // 第三参 Func<bool> 控制是否可点
_hostMenu.Attach(myTargetElement);      // 挂到目标，右键弹出
```

- `UI4MenuItemType` 七项：`Undo Redo Cut Copy Paste Delete SelectAll`；`AddItem(type, action, canExecute)` 会用 `UI4MultiLanguage.Get(key)` 自动取本地化文案，图标来自 `UI4MenuIcons`（几何绘制的静态缓存）。自定义文案用 `AddItem(new UI4MenuItem(type, text, icon, action, canExecute))`——注意 `UI4MenuItem` 字段是 `Type/Text/Icon(ImageSource)/Command/CanExecute`，**没有 `IconText`**。
- 生命周期：`Attach(UIElement)` 订阅目标右键并构建菜单；`Detach()` 退订 `ThemeChanged`、解除目标事件并 `Close()`。**构造里订阅了 `UI4Theme.ThemeChanged` 且不退订**——所以复用实例、别在循环里 `new`（否则订阅累积）。一个页面一个实例，窗体关闭时 `Detach()`（Demo `:298`）。
- 外观属性 `BorderColor/Background/HoverBackground` 各带“**显式赋值后主题切换不再覆盖**”语义（`:245-267`）：不赋值就跟随主题，一旦赋值就冻结。`Width` 默认 160、`ItemPadding` 默认 `12,8,12,8`。
- `SyncBeforeOpen()`（`:310-322`）：`Open()` 前把失联期间错过的主题补上——因为“未弹出过的 `Popup` 子元素不触发 `Loaded`”。它内部会调 `UI4ListBox.RefreshTheme()`。宿主一般不用手调，`Open()` 已含；但把 `UI4ListBox` 直接塞进自己写的 `Popup` 时，记得弹出前 `RefreshTheme()`。

## 22.5 UI4MessageBox 与 UI4ColorPicker

**UI4MessageBox**：静态 `Show(content, title=null, buttons=OK, width=460, owner=null)` 返回 `bool?`。三态语义：`true`=点了 OK、`false`=点了 Cancel、`null`=被关闭（右上角/Escape）。`title=null` 时用本地化 `Notice`。字体/图标/阴影是静态缓存（`:33-37`）。**务必传 `owner: this`**：不传时回退 `CenterScreen`（`:280-283`），曾有的 `CenterOwner`+null Owner 级联偏移缺陷就靠传 owner 规避。

```csharp
bool? r = UI4MessageBox.Show("确认删除该记录吗？", "请确认",
    UI4MessageBoxButtons.OKCancel, owner: this);
if (r == true) DeleteSelected();   // 只有 true 才继续
```

**UI4ColorPicker**：构造 `(title=null, defaultColor=null)`。
- 静态 `ShowDialog(title=null, defaultColor=null, owner=null)` 返回 `Color?`（取消→`null`）——首选这种。
- 实例方法 `Show(Window owner=null)` **名字骗人，实际内部是 `base.ShowDialog()` 模态**（`:790-794`），别当成非阻塞 `Show`。选完读实例的 `SelectedColor`。

```csharp
Color? picked = UI4ColorPicker.ShowDialog("选择颜色", Colors.Blue, this);
if (picked.HasValue) ApplyAccent(picked.Value);
```

## 22.6 UI4NotifyIcon：全生命周期

`UI4NotifyIcon : FrameworkElement, IDisposable`，用 P/Invoke `Shell_NotifyIcon`（非 WinForms）。

- **必须在视觉树里**：`Loaded→CreateTrayIcon`、`Unloaded→RemoveTrayIcon`。放在 `Window` 的内容里（Demo `MainWindow.xaml:561` 放在 Grid 内，`Visibility="Collapsed"`）。
- **`Visibility` 即开关**：`OverrideMetadata` 里 `Visible→CreateTrayIcon`、非 `Visible→RemoveTrayIcon`（`:85-86, 618-625`）。显隐托盘 = 切 `Visibility`（Demo `:264` `TrayIcon.Visibility = TraySwitch.IsOn ? Visible : Collapsed`）。
- `MenuActivation`（`PopupActivationMode`）：枚举值 `None / LeftClick / RightClick / DoubleClick / LeftOrRightClick / All`。构造函数显式设成 `None`（`:91`）——要右键弹菜单得自己设 `RightClick`，或自己订阅 `TrayRightMouseDown` 调 `OpenMenu()`。
- `ToolTipText`（超 127 字符自动截断）、`IconSource`（`ImageSource`；经 `GetResourceStream/GetContentStream`/URI 解析，失败回退 `System.Drawing.SystemIcons.Application`，`:429-441`）。
- `AddItem(UI4MenuItemType, Action, Func<bool>)` 自动本地化（`:229-235`），或 `AddItem(UI4TrayMenuItem)`；`ClearMenuItems/OpenMenu/CloseMenu`。
- 事件：`TrayLeftMouseUp` / `TrayRightMouseDown` / `TrayMouseDoubleClick`。
- 内部机制：隐藏消息 `HwndSource`、`DispatcherTimer`（100ms 检查鼠标是否在弹窗外以自动关菜单）、订阅 `Application.Exit`（`:599` 触发 `Dispose`）。
- **需要宿主引用 `System.Drawing`**（`SystemIcons`/`Icon`），否则编译/运行缺类型。
- 退出**必须**：先 `Visibility=Collapsed` 再 `Dispose()`，否则 `Shell_NotifyIcon` 记录不删除，留“幽灵图标”（鼠标划过才消失）。`Dispose()` 会 `RemoveTrayIcon`、退订 `Application.Exit`、销毁 `HwndSource`。
- Win10/11 新图标默认进**通知区域溢出区**（小号图标），验收时要点“^”展开才看得见，属系统行为不是 bug。

**完整“关闭时清理”代码**（照抄 Demo `:296-301` 的思路）：

```csharp
private void MainWindow_Closed(object sender, EventArgs e)
{
    if (_hostMenu != null) _hostMenu.Detach();   // 退订 ThemeChanged
    TrayIcon.Visibility = Visibility.Collapsed;   // 先摘图标
    TrayIcon.Dispose();                           // 再释放消息窗口/定时器
    UI4Theme.ReleaseSystemFollow();               // 用过 System 模式才需要（见第 21/24 章）
}
```

**“最小化到托盘”库不提供**，自己组合：在 `StateChanged` 里若 `WindowStyle==Minimized` 则 `Hide()`；托盘双击事件里 `Show()+WindowState=Normal+Activate()`；真正退出用一个 tray 菜单项调 `Application.Current.Shutdown()`（或在其中把 `Visibility` 保持 `Visible` 直到 `Closed`）。

## 22.7 Do / Don't

- Do：`UI4ContextMenu` 一实例一目标，窗体 `Closed` 里 `Detach()`；`MessageBox/ColorPicker` 一律传 owner。
- Do：托盘 `MenuActivation` 显式设成你要的枚举；关闭流程 `Collapsed→Dispose`。
- Don't：别把 `UI4ContextMenu`/`UI4MessageBox` 写进 XAML；别在循环里 `new UI4ContextMenu()`（订阅泄漏）。
- Don't：别把 `UI4ColorPicker.Show()` 当非阻塞用（它是模态）。
- Don't：别漏 `System.Drawing` 引用；别只 `Dispose()` 不先 `Collapsed`（幽灵图标）。

## 22.8 坑与排错

- 症状：右键菜单不出现。根因：`UI4ContextMenu` 不是控件、没 `Attach`，或目标元素已被自身 `ContextMenu` 抢占。处置：确认 `Attach(target)`，并清掉目标的原生 `ContextMenu`。
- 症状：切主题后托盘/右键菜单色还停在旧主题。根因：外观属性被显式赋值（“显式赋值后不再覆盖”）。处置：想跟随就别赋值。
- 症状：颜色选择器“点确定后界面没更新”。根因：误把实例 `Show` 当非阻塞，主线程其实被模态阻塞。处置：改判 `ShowDialog` 返回值或 `Show` 返回的 `bool?`。
- 症状：应用关闭后托盘残留图标。根因：未 `Collapsed`+`Dispose`。判据：`demo-errors.log` 无异常但图标还在=生命周期没收尾。处置：22.6 的 `MainWindow_Closed`。
- 症状：托盘图标是个通用程序图标。根因：`IconSource` URI 解析失败，回退 `SystemIcons.Application`。处置：给合法 pack uri 或 `BitmapImage`，或接受回退。
- 症状：CS0103 `UI4ContextMenuLanguage` 不存在。根因：README 残留的已删除类型。处置：该类型已移除，改用 `UI4MultiLanguage`（第 23 章）。

## 22.9 完成判据（自查）

- [ ] 主菜单/右键菜单文案随语言变化时能被重建（见第 23 章“构造期拉取”）。
- [ ] `UI4ContextMenu` 每实例 `Detach` 一次；未在循环里 `new`。
- [ ] 所有 `MessageBox.Show` / `ColorPicker.ShowDialog` 都带 owner。
- [ ] 托盘 `MenuActivation` 已显式设定；`System.Drawing` 已引用。
- [ ] `Closed` 事件里 `Collapsed`→`Dispose`；用过 System 模式已 `ReleaseSystemFollow()`。
- [ ] `interact.ps1 -Scenario menu|ctx|tray` 通过，`demo-errors.log` 无新增异常。

---

# 第 23 章 本地化、图标资源与业务数据接入

> 本章解决：在新应用里正确驱动库的本地化、用系统字体出图标、并在“库无 MVVM/无 DI”的现实下搭出可编译的最小业务分层。
> 读者：AI Agent（要在本项目之外新建/续写 WPF 业务代码）
> 前置章节：第 19/20 章（控件绑定）、第 22 章（菜单文案来源）

## 23.1 目标与验收

- 切语言：改 `CultureInfo.CurrentUICulture` 后 `UI4MultiLanguage.Refresh()`，再手动刷新依赖它的 UI。
- 图标零资源文件：用 `Segoe MDL2 Assets` 码位。
- 业务层有可编译的 `ObservableObject/RelayCommand`，集合更新回 UI 线程，后台取数不阻塞 UI。
- 异常落 `<app>-errors.log`，运行期不弹自证框。

## 23.2 源码依据（必读）

- `src\StartUI4Controls\UI4MultiLanguage.cs:10-23`（`enum UI4LanguageKey`，实为 **11 项**）、`:37-55`（`Current/Get/Refresh`）、`:57-78`（`GetStrings(lang)`：zh/ja/ko/de/fr/es/ru，default=英文）、`:28-32`（注释谎称有 `SetLanguage(string)` 且支持繁中）。
- `samples\StartUI4Demo\MainWindow.xaml.cs:18-24`（`DataContext=this` + `List<CardItem>`）、`:64-73`（`BuildCards`）、`:278-294`（语言切换 + `UpdateLangSample`）、`:304-318`（`CardItem` 类）。
- `src\StartUI4Controls\UI4NotifyIcon.cs:429-452`（`IconSource` URI 解析与 `SystemIcons.Application` 回退）。
- `src\StartUI4Controls\UI4Theme.cs`（`IThemePersistence`/`Persistence`/`Save`/`ApplyPersisted`，见第 24 章引用）。

## 23.3 UI4MultiLanguage 真实用法

`UI4MultiLanguage` 是静态类，只提供组件库内部（消息框、右键菜单）用的字符串。

- `UI4LanguageKey` 共 **11 项**：`OK Cancel Notice ColorPicker Undo Redo Cut Copy Paste Delete SelectAll`。（提示：不要按“十项”计数写映射表，源码是 11。）
- API：`Get(UI4LanguageKey)`（缺键回退返回枚举名）、`Refresh()`（把缓存 `_current` 置空，下次 `Get` 依 `CultureInfo.CurrentUICulture` 重新取）、`GetStrings(string lang)`（按两字母语言名给字典）、`Current`（当前字典）。
- **没有 `SetLanguage` 方法**（注释是过时文档）。内置语言 `zh/ja/ko/de/fr/es/ru` + 默认英文，**无繁体中文分支**：`zh-TW` 的两字母码也是 `zh`，拿到的是简体。
- 切语言 = 两步：改 `CultureInfo.CurrentUICulture` → `UI4MultiLanguage.Refresh()`。

```csharp
private void SwitchToEnglish()
{
    CultureInfo.CurrentUICulture = new CultureInfo("en-US");
    UI4MultiLanguage.Refresh();
    RebuildLocalizedUi();   // 见下：库不会自动重译已存在的 UI
}
```

- **构造期拉取**：库在控件/菜单“构造时”调一次 `Get()` 把文案烤进对象（`UI4ContextMenu.AddItem`、`UI4MessageBox` 按钮等）。`Refresh()` 只影响之后新构造的东西，**已弹出的菜单/已开着的对话框不会重译**。处置：切语言后重建这些 UI。
- 需要宿主手动刷新的清单：
  1. `UI4ContextMenu`（含各输入框内置的右键菜单）→ 重新 `Attach` 或重建实例。
  2. `UI4MessageBox` → 关掉重开。
  3. 托盘菜单项文本（`AddItem` 时烤入）→ `ClearMenuItems()` 后重加。
  4. 业务自己写死中文的 `Content/Header/Text` → 自行按资源刷新。
  - 不需要刷新的：纯 `DynamicResource` 令牌颜色（与语言无关）。

## 23.4 业务应用自己的资源策略

库的字典是“库内部件”用的，业务文案别塞进去。两条线并存：

- 业务 UI 文案：放你自己的 `.resx`（`Resources/Strings.resx`、`Strings.en.resx`…），`XAML` 里 `{x:Static loc:Strings.SomeKey}` 或 `Properties.Resources.SomeKey`。切换时同样依赖 `CurrentUICulture`。
- 库件文案：交给 `UI4MultiLanguage`，只在“切语言后重建”时生效（23.3）。
- 判据：若某个按钮同时受两套影响（如“确定”），以业务 `.resx` 为准；库只在它自带部件（消息框/右键菜单）里出词。别去改库的字典。

## 23.5 图标与静态资源

- 仓库零资源文件：`shots/` 是脚本产物，无字体/图片/ico。托盘图标用系统回退（`SystemIcons.Application`）。
- 系统图标：直接用 `Segoe MDL2 Assets` 码位，不引第三方图标字体。菜单/页签写 `TextIcon="&#xE712;" TextIconFontFamily="Segoe MDL2 Assets"`（Demo `:163-164` 用 `IconFontFamily`/`TextIconFontFamily` 分别对应 `UI4MenuElementItem`/`UI4TabItem`）。
- `ImageSource` 与 pack uri：`IconSource` 经 `GetResourceStream/GetContentStream`/URI 解析（`UI4NotifyIcon.cs:429-452`）；失败静默回退系统图标。自定义 `BitmapImage` 用 pack uri 形如 `pack://application:,,,/YourAssembly;component/Images/app.ico`；宿主须真的把该资源以 `Resource` 编入。

## 23.6 数据接入现实与最小 MVVM

现实：Demo 用 `DataContext = this` + `List<CardItem>` + `ItemTemplate`，**没有 MVVM/DI**（`MainWindow.xaml.cs:18-24`）。`List<T>` 无变更通知，改了不刷新。推荐分层（无第三方框架时的最小 MVVM，纯 C# 7.3）：

```csharp
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

public abstract class ObservableObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    protected void Raise([CallerMemberName] string name = null)
    {
        PropertyChangedEventHandler h = PropertyChanged;
        if (h != null) h(this, new PropertyChangedEventArgs(name));
    }
    protected bool Set<T>(ref T field, T value, [CallerMemberName] string name = null)
    {
        if (System.Collections.Generic.EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        Raise(name);
        return true;
    }
}

public sealed class RelayCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool> _canExecute;
    public RelayCommand(Action execute, Func<bool> canExecute = null)
    { _execute = execute; _canExecute = canExecute; }
    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
    public bool CanExecute(object parameter) { return _canExecute == null || _canExecute(); }
    public void Execute(object parameter) { _execute(); }
}
```

`ObservableObject`/`RelayCommand` 放宿主自己的 `Mvvm/` 目录，不进库；两文件都用标准 `System` 引用，无第三方依赖。

- 视图模型示例：`public ObservableCollection<CardItem> Cards { get; }`，配 `ICommand SaveCommand`，`ItemsSource="{Binding Cards}"`。
- 集合更新必须回 UI 线程：后台线程改 `ObservableCollection` 会抛跨线程异常。用 `Dispatcher.Invoke`/`BeginInvoke` 回到 UI 线程再增删（见下）。
- 后台取数模式：`await Task.Run(...)` 取数据，再在 UI 线程赋值；**别在 UI 线程同步做 IO/等待**。

```csharp
private async Task LoadAsync()
{
    var rows = await Task.Run(() => _repo.ReadAll());       // 后台线程读
    Cards.Clear();                                            // 回到 UI 线程再改集合
    foreach (var r in rows) Cards.Add(r);
}
```

- 分页/大列表：优先分页或虚拟化 `UI4ListBox`；`UI4GridView`（UniformGrid）不虚拟化，避免上千条（第 20 章）。
- SQLite 依赖真相：库里那套 SQLite 只被 **internal 的 `UI4DataGrid`** 使用，宿主拿不到、也用不上（它甚至会把 `SQLite.Interop.dll` 落到 `%TEMP%`）。要本地库请宿主自带 provider（自己引 `System.Data.SQLite` 或别的），别依赖库的传递引用。

## 23.7 异常、日志与配置持久化

- 异常与日志：沿用 Demo 的 `<app>-errors.log` 落盘。在 `App.xaml.cs` 注册 `DispatcherUnhandledException` + `AppDomain.CurrentDomain.UnhandledException`，异常**追加**写入 exe 同目录日志文件；**运行期禁止用弹窗自证**（弹窗会打断自动化并掩盖真实故障面）。运行期状态提示走窗内 `StatusText`。所有验证脚本以“`<app>-errors.log` 是否存在”判定运行期错误。
- 配置持久化：
  - 主题：用库的 `IThemePersistence`（`RegistryThemePersistence` 或 `JsonThemePersistence(path)`），赋给 `UI4Theme.Persistence`，切主题后 `UI4Theme.Save()`、启动时 `UI4Theme.ApplyPersisted()`。
  - 业务配置：自管（自己的文件/注册表），别混进主题持久化。

## 23.8 Do / Don't

- Do：切语言 = 改 `CurrentUICulture` + `Refresh()` + 重建依赖文案的 UI；业务文案进自己的 `.resx`。
- Do：`ObservableCollection` + `Raise` 更新 UI；后台 `Task.Run` 取数、回 UI 线程改集合。
- Don't：别调不存在的 `UI4LanguageKey.SetLanguage`；别以为 `Refresh()` 会重译已弹出的菜单/对话框。
- Don't：别在 UI 线程同步等 IO；别在后台线程直接改绑定集合。
- Don't：别指望库的 SQLite 给你用（那是 internal 死码）；别用运行期弹窗做自证。

## 23.9 坑与排错

- 症状：切语言后消息框/右键菜单仍是旧语言。根因：构造期拉取，已建对象不重译。处置：关掉重开 / 重新 `Attach` / `ClearMenuItems` 重加。
- 症状：想设繁体但全是简体。根因：无 `zh-TW` 分支，两字母码都归 `zh`。处置：接受或自己维护繁中 `.resx`。
- 症状：数据改了界面不刷新。根因：`List<T>` 无 INPC。处置：`ObservableCollection<T>` + `INotifyPropertyChanged`。
- 症状：后台线程更新集合崩溃（跨线程 `ObservableCollection`）。根因：非 UI 线程改集合。处置：`Dispatcher.Invoke/BeginInvoke` 回 UI 线程。
- 症状：找不到 `System.Data.SQLite` 类型。根因：库没暴露公共 SQLite API（仅 internal `UI4DataGrid` 用）。处置：宿主自带 provider。

## 23.10 完成判据（自查）

- [ ] 语言切换走 `CurrentUICulture` + `Refresh()`，并重建了菜单/对话框/托盘项文案。
- [ ] 图标全用 `Segoe MDL2` 码位；`pack uri` 资源确已编入宿主。
- [ ] 有可编译的 `ObservableObject`/`RelayCommand`；绑定集合为 `ObservableCollection<T>`。
- [ ] 后台取数用 `Task.Run`，集合更新在 UI 线程。
- [ ] 未使用 internal 的 `UI4DataGrid`/SQLite；异常落 `<app>-errors.log`，无运行期自证弹窗。
- [ ] 主题持久化走 `IThemePersistence`；业务配置自管。

---

# 第 24 章 自动化验证、故障定位与交付

> 本章解决：给新应用建立“可信”的验证习惯、按症状快速定位，并产出可交付的 net48 运行包与自查表。
> 读者：AI Agent（要在本项目之外新建/续写 WPF 业务代码）
> 前置章节：第 02–08 章（仓库/工程/启动）、第 12–17 章（主题机制）、第 18–23 章（控件与业务实操）

## 24.1 目标与验收

- 理解本库的验证哲学：截图全白不可信，只认“进程内断言 + UIA 走查 + DWM 属性回读”三类硬证据。
- 会用工具链，会写符合约定的新脚本，会给新应用搭最小自检。
- 按“症状→定位路径”决策表排障；知道回归执行顺序与各自预期输出。
- 产出正确交付物清单与 Agent 交付前自查表。

## 24.2 源码依据（必读）

- 验证脚本开头约定（读不跑）：`titlebar.ps1:1-40`、`titlebar-live.ps1`、`p3verify.ps1:1-40`（期望值取 `UI4ThemeDefinition`）、`p2verify.ps1:1-40`（离屏宿主控件按主题断言画刷色）、`scopewalk.ps1:1-40`（ASCII-only + `Str 0x...` 码点 + `Check` PASS/FAIL + UIA `FindFirst`）、`theme.ps1:1-40`（以 DWM dark flag 为状态判据）、`interact.ps1:1-40`（`-Scenario` 参数 + 合成点击）。
- `samples\StartUI4Demo\App.xaml.cs:17-51`（异常落 `demo-errors.log`）；`MainWindow.xaml.cs:49-62`（`--tab=N`）。
- 事实来源：`事实速查.md §7/§8/§9`、`PORTING.md §1–§9`、`README.md 第七/九章`。

## 24.3 验证哲学

本机 WPF 抓屏（`PrintWindow`/`CopyFromScreen`）**全白**，连不引库的基线窗体也如此（§7.2）。所以颜色/主题正确性不能靠看图。三类硬证据：

1. 进程内断言：`Add-Type` 载入 `StartUI4Controls.dll`，直接 new 控件、切主题、读 DP/画刷色比对（`p2verify`/`p3verify`）。
2. UIA 走查：跨进程用 `UIAutomationClient` 按 `AutomationId`(=x:Name)/`Name` 找元素、读文本/切换状态（`scopewalk`/`theme`/`interact`）。
3. DWM 属性回读：`DwmGetWindowAttribute` 读 immersive-dark flag（属性 20，pre-20H1 用 19）判断标题栏深浅，是唯一能从外部进程拿到的硬证据；caption/text/border（34/35/36）拒绝回读 `0x80070057`。截图只作辅助留档（`shots/*.png`）。

## 24.4 工具链表：证明什么、怎么读输出

| 脚本 | 证明什么 | 怎么读输出 |
|---|---|---|
| `titlebar.ps1` | 进程内 31 项：自带 `DwmGetWindowAttribute` 回读深浅/COLORREF，覆盖三条自动通路 + 手动兜底 | 每行 `PASS/FAIL 标签 实际 期望`；末尾汇总计数 |
| `titlebar-live.ps1` | 跨进程 9 项：起真 Demo，独立读 DWM 属性验异主题窗口与全局高对比 | 同上；须先看 `demo-errors.log` 不存在 |
| `p3verify.ps1` | 进程内 83 项：作用域主题、highcontrast 30 令牌完整、持久化往返、`SetAccent` 传播、ComboBox/ListBox 令牌色 | 分组标签（G2/H 等）；`[H]` 组证 `EditBackground/TextColor/FocusGradient*` 跟随主题且覆盖项保留自身色 |
| `p2verify.ps1` | 进程内：离屏宿主控件按主题断言解析后的画刷色（16×2），并确认 Style 未被重建 | 色值 `#RRGGBB` 对齐；“未重建”看断言行 |
| `scopewalk.ps1` | UIA 走查 `--tab=10`：作用域键下拉、全局高对比/跟随系统、撤销、开 `ScopeWindow` 读自述 | `Check` 逐条 PASS/FAIL |
| `theme.ps1` | 起 `--tab=0`，UIA 拨“深色模式”开关、Tab 往返再拨回；以 DWM flag 为状态判据 | 失败即 `Stop-Process; exit 1` |
| `interact.ps1 -Scenario {msgbox,color,menu,ctx,tabadd,tray,hover,combo}` | 按场景合成交互并落 `shots/*.png` | 看返回码与 `shots` 产物 + `demo-errors.log` |
| `shot.ps1 -Tab N` | 起 Demo 到第 N 页 `PrintWindow` 抓图后杀 | 仅产物路径，图可能全白，不作正确性判据 |

约定：全部 `powershell -STA -ExecutionPolicy Bypass -File <script>`；运行期错误判据 = `samples/StartUI4Demo/bin/Debug/net48/demo-errors.log` 是否存在。

## 24.5 脚本编写约定（照抄）

- **ASCII-only**：PS 5.1 会错读无 BOM 的 UTF-8。中文 UI 文案用码点拼：`function Str([int[]]$codes){ $s=''; foreach($c in $codes){$s+=[char]$c}; return $s }`，如 `Str 0x6253,0x5F00`。
- **泵序**：验证主题批处理必须先 `Dispatcher.Invoke(noop, ContextIdle)` 再 `Render`；`UI4Theme` 的刷新在 `Input` 优先级批处理，`ContextIdle` 低于 `Input`，只泵 `Render` 落不了地。
- **期望值取自 `UI4ThemeDefinition.Light()/Dark()/HighContrast()` 的 `GetColor(token)`**，绝不从被测控件反取（否则自证循环）。
- **构建前** `taskkill //IM StartUI4Demo.exe //F`，否则 MSB3027 文件占用（构建由人工执行，脚本内不 build）。
- **启动型脚本收尾 `Stop-Process -Id $p.Id -Force`**（`interact/shot/scopewalk/theme/titlebar-live` 都是）。
- **`--tab=N`**（0 基）逐页验证；启动时先删旧 `demo-errors.log`，结束判其存在与否。

## 24.6 为新业务应用搭最小自检（ASCII-only 模板）

放到宿主工程根，`powershell -STA -ExecutionPolicy Bypass -File selfcheck.ps1`。它起窗、遍历主题、落日志、退出：

```powershell
param()
$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName PresentationFramework
Add-Type -AssemblyName PresentationCore
Add-Type -AssemblyName WindowsBase
$dll = Join-Path $PSScriptRoot 'src\StartUI4Controls\bin\Debug\net48\StartUI4Controls.dll'
Add-Type -Path $dll

$app = [System.Windows.Application]::Current
if (-not $app) { $app = New-Object System.Windows.Application }

function Pump([int]$rounds){
  $d = [System.Windows.Threading.Dispatcher]::CurrentDispatcher
  for($i=0;$i -lt $rounds;$i++){
    $d.Invoke([Action]{}, [System.Windows.Threading.DispatcherPriority]::ContextIdle) | Out-Null
    Start-Sleep -Milliseconds 80
    $d.Invoke([Action]{}, [System.Windows.Threading.DispatcherPriority]::Render) | Out-Null
    Start-Sleep -Milliseconds 40
  }
}

$pass = 0; $fail = 0
function Check([string]$label, [bool]$ok, [string]$detail){
  if($ok){ $script:pass++; Write-Output ('  PASS  ' + $label) }
  else   { $script:fail++; Write-Output ('  FAIL  ' + $label + '  <' + $detail + '>') }
}

foreach($mode in @('Light','Dark')){
  [StartUI4Controls.UI4Theme]::SetTheme([Enum]::Parse([StartUI4Controls.UI4ThemeMode], $mode))
  Pump 3
  $bg = [StartUI4Controls.UI4Theme]::Current.BackgroundColor
  # Expected from the theme definition, not from the control under test.
  $def = switch($mode){ 'Light' { [StartUI4Controls.UI4ThemeDefinition]::Light() }
                        'Dark'  { [StartUI4Controls.UI4ThemeDefinition]::Dark() } }
  $tok = [Enum]::Parse([StartUI4Controls.UI4ThemeToken], 'Background')
  $exp = $def.GetColor($tok)
  $same = ($bg.R -eq $exp.R -and $bg.G -eq $exp.G -and $bg.B -eq $exp.B)
  Check ('theme.' + $mode + '.background') $same ('got ' + $bg.ToString() + ' want ' + $exp.ToString())
}

Write-Output ('pass=' + $pass + ' fail=' + $fail)
if ($fail -gt 0) { exit 1 }
```

- 若要验标题栏，另起真窗口 + `DwmGetWindowAttribute`（照 `titlebar.ps1`）。运行期错误判据：把 `$log` 指向宿主的 `<app>-errors.log`，先 `Remove-Item`、结束 `Test-Path`。

## 24.7 症状 → 定位路径 决策表

| 症状 | 定位路径 | 判据/处置 |
|---|---|---|
| XAML 报“类型不存在”一堆（MC3074 级联） | 先全量 `dotnet build` 看真实错误 | 别信 IDE 列表；根因常是库内 C# 编译错，修好即级联消失 |
| 切主题界面不更新 | 是否用了 `StaticResource` 快照令牌 | 全部改 `DynamicResource`（键原地覆盖）|
| 色值仍是浅色 | 对被主题管理的 DP 赋了本地值（`ReadLocalValue`）；或控件本就“不跟随主题”（见 21.5） | 删本地值；或用 `UI4ThemeScope` 反衬/宿主自染 |
| 标题栏不变 | 四条通路逐条排查 | A 无 `ThemeChanged`？B 窗内无 UI4 控件 Loaded？C 作用域根不是 Window？D 未手动 `UI4WindowTitleBar.Apply(win)`？纯窗口只能靠 D |
| 下拉文字被切 | `Height` 不满足公式（第 20 章） | `Height ≥ FontSize×1.2 + Top + Bottom + 2` |
| 文字溢出 | 自定义模板无 `TextTrimming`/`ClipToBounds`；popup 用 Width 而非 MinWidth | 模板加截断 + ToolTip；面板用 `MinWidth` |
| 托盘幽灵图标 | 退出未 `Collapsed`+`Dispose` | 见 22.6 `MainWindow_Closed` |
| `demo-errors.log` 出现异常 | 打开看首条堆栈定位控件 | 常见：XAML 解析期早触发事件未空判（`if (X != null)`）、跨线程改集合 |
| UIA 找不到控件 | 缺 `x:Name`/无 `AutomationId`；或该控件无 pattern | 给控件设 `x:Name`；`UI4Switch` 无 Toggle pattern，按 Name/坐标点击；属主窗挂 owner 下，用 `Descendants` |
| 系统跟随后泄漏 | 用过 `UI4ThemeMode.System` 未收尾 | 退出调 `UI4Theme.ReleaseSystemFollow()` |

## 24.8 回归执行顺序与预期

按序（每步全绿再下一步）：

1. `taskkill //IM StartUI4Demo.exe //F` → `dotnet build`（人工执行，预期 0 error；上游 CS0414 遗留噪声可忽略，§7.12）。
2. `titlebar.ps1` → 31 项 PASS（深浅标志/COLORREF 回读全中）。
3. `p3verify.ps1` → 83 项 PASS（含 highcontrast 30 令牌完整、持久化往返、`SetAccent` 传播、`[H]` 组）。
4. `p2verify.ps1` → 16×2 画刷色对齐、Style 未重建。
5. `theme.ps1` / `scopewalk.ps1` → UIA 交互与局部主题页全 PASS。
6. `interact.ps1 -Scenario menu|ctx|tray|...` → 各场景返回码 0，落 `shots/*.png`，无 `demo-errors.log`。

预期输出样式：逐行 `  PASS  <label>` / `  FAIL  <label>  <detail>`，末尾 `pass=<N> fail=0`；判定运行期错误看 `demo-errors.log` 是否被创建。

## 24.9 发布产物清单与 4.8 依赖检查

- 最小运行包：`<App>.exe` + `StartUI4Controls.dll` + `ICSharpCode.AvalonEdit.dll` + 自动生成的 `<App>.exe.config`。
- `System.Data.SQLite.dll` **可省**：唯一消费者 `UI4DataGrid` 是 internal 死码，且其 `SQLite.Interop.dll` 本就不拷到宿主输出。
- net48 **没有** `.deps.json` / `.runtimeconfig.json`；运行时由 csproj `TargetFramework=net48` 与 exe.config 的 `supportedRuntime sku=".NETFramework,Version=v4.8"` 决定，与 `.sln` 无关。
- 若 XAML 触及 `UI4CodeEditor`，`ICSharpCode.AvalonEdit.dll` 必须随包（否则运行期解析 XAML 崩溃）。
- 构建顺带产出 `src/StartUI4Controls/bin/Debug/StartUI4.WPF.1.0.20.nupkg`（`GeneratePackageOnBuild=true`）。
- `app.manifest`（PerMonitorV2）是宿主责任，随包；它同时保证 `Environment.OSVersion` 真实（库靠“先试 + HRESULT”探测，不看版本号）。

## 24.10 交付前 Agent 自查表

- [ ] 主题三态（Light/Dark/HighContrast）全部页面逐一肉眼 + `p3verify` 对齐；自定义主题比 `ResolvedKey` 不比 `CurrentMode`。
- [ ] 高对比：30 令牌完整，选中底 `#00008B` 上白字可读。
- [ ] DPI：100%/150%/200% 下下拉不裁字（按 `Height` 公式，DIP 非 pt）；manifest PerMonitorV2 在位。
- [ ] 语言：切 `CurrentUICulture`+`Refresh()` 后，菜单/对话框/托盘项已重建；无对不存在 `SetLanguage` 的调用。
- [ ] 托盘收尾：关闭流程 `Collapsed`→`Dispose`；用过 System 模式已 `ReleaseSystemFollow()`。
- [ ] 异常：`App.xaml.cs` 两级 handler 落 `<app>-errors.log`；运行期无自证弹窗；`demo-errors.log`/宿主日志为空。
- [ ] 交付物：exe + 两 dll + exe.config 齐全；无 SQLite 依赖缺失告警；无 `.deps/.runtimeconfig` 期望。
- [ ] 文档归档：新增控件/坑写进 `README.md`（第五/十章）与 `PORTING.md`（对应节），手册章节沿用 `chNN-短标题.md` 命名并入 `Agent手册/`。

## 24.11 坑与排错

- 症状：脚本里中文变量名/字符串导致解析错乱。根因：非 ASCII + 无 BOM UTF-8。处置：脚本保持 ASCII，中文用码点 `Str`。
- 症状：切主题后断言读到旧色。根因：只泵 `Render` 没泵 `ContextIdle`。处置：先 `ContextIdle` 再 `Render`。
- 症状：断言永远通过。根因：期望值取自被测控件。处置：改取 `UI4ThemeDefinition`。
- 症状：构建报 MSB3027 文件占用。根因：Demo 进程还活着。处置：先 `taskkill`。
- 症状：发布后 `ColorPicker/CodeEditor` 打不开。根因：漏带 `ICSharpCode.AvalonEdit.dll`。处置：随包。

## 24.12 完成判据

- [ ] 24.8 六步回归按序全绿，`fail=0`。
- [ ] 新应用有 `selfcheck.ps1`（ASCII-only，先泵 ContextIdle 再 Render）。
- [ ] 24.7 决策表覆盖到的症状都能对上处置。
- [ ] 24.9 产物清单核对通过；24.10 交付前自查逐条打勾。
- [ ] 运行期错误判据（`<app>-errors.log` 不存在）成立。

---
