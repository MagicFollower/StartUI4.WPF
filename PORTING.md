# StartUI4.WPF — .NET Framework 4.8 移植说明

上游仓库：<https://github.com/KSSTU/StartUI4.WPF>（master，v1.0.20，MIT）
本目录是其 .NET Framework 4.8 / C# 7.3 的完整重写版本。上游原始源码保留在 `upstream/` 供对照。

---

## 1. 兼容性结论：不兼容，需要重写

上游 `StartUI4Controls.csproj` 目标为 `net6.0-windows7.0`，无法在 .NET Framework 4.8 下编译或运行。
不兼容点分四类：

| 类别 | 上游用法 | net48 下的问题 |
|---|---|---|
| 目标框架 | `net6.0-windows7.0` | 需改为 `net48` |
| 语言级别 | C# 10（net6 默认） | net48 默认 C# 7.3，现代语法全部报错 |
| 可空引用类型 | `<Nullable>enable</Nullable>` + 18 处 `T?` 注解 | C# 7.3 不支持该特性（CS8370） |
| 隐式 using | `<ImplicitUsings>enable</ImplicitUsings>` | 需显式补全（实测源码已带全部所需 using，无需补） |
| BCL API | `Math.Clamp` ×17 | .NET Framework 4.8 无此方法 |
| NuGet 依赖 | `Microsoft.Data.Sqlite 10.0.10` | 源码中**从未使用**，且该版本不支持 net48 → 移除 |
| NuGet 依赖 | `AvalonEdit 6.3.1.120` | 提供 net462 目标 → 保留 |
| NuGet 依赖 | `System.Data.SQLite 2.0.3` | 提供 net471 目标 → 保留 |

扫描确认**未使用**以下 net6 专属 API，故无需替换：`Index/Range`、`Span/Memory`、`System.Text.Json`、
`DateOnly/TimeOnly`、`Random.Shared`、`string.Contains(char)`、`Enum.GetValues<T>()`、`Dictionary.TryAdd`、
async streams、`EffectiveViewportChanged` 等。WPF 侧用到的 API（`CompositionTarget`、`HwndSource`、
`Media3D`、`AutomationPeer` 未用）均在 net48 可用范围内。

---

## 2. 移植改动清单

工程配置（`src/StartUI4Controls/StartUI4Controls.csproj`）：

- `TargetFramework` → `net48`；`LangVersion` → `7.3`；`Nullable`/`ImplicitUsings` → `disable`
- 移除未使用的 `Microsoft.Data.Sqlite`
- 显式 `<Reference Include="System.Drawing" />`（`UI4NotifyIcon` 使用 `System.Drawing.Icon`）
- 保留 NuGet 打包元数据，构建时产出 `StartUI4.WPF.1.0.20.nupkg`

源码改动（共 11 个文件，其余 21 个文件逐字节未动）：

| 改动 | 处数 | 说明 |
|---|---|---|
| `Math.Clamp(v, lo, hi)` → `Math.Max(lo, Math.Min(hi, v))` | 17 | 按调用点实际类型（int/double）选择字面量 |
| `x is not T y` → `!(x is T y)` | 6 | C# 9 → C# 7.3 |
| target-typed `new()` → 显式 `new List<string>()` | 1 | C# 9 → C# 7.3 |
| lambda 丢弃参数 `(_, _)` → `(sender, e)` | 1 | C# 9 → C# 7.3 |
| 移除可空引用注解 `ScrollBar?`/`string?` 等 | 18 | 引用类型上的 `?` |
| `using var x = ...` → `using (var x = ...) { }` | 5 | 全部在 `UI4DataGrid.cs`，按原生命周期重排代码块 |
| 隐藏继承成员补 `new`（消 CS0108） | 7 | `UI4Panel`/`UI4TextBlock` 重定义的 DP 字段与属性 |
| `UI4Switch.ArrangeOverride` 修复 | 1 | 见下「上游问题 1」 |

---

## 3. 移植过程中发现的上游问题

以下均为上游既有缺陷（与 net6/net48 无关），已用截图证实：

1. **`UI4Switch` 被水平拉伸时轨道与滑块分离**（已在库内修复）。
   控件自建视觉树，`ArrangeOverride` 把内部 Grid 铺满 `arrangeBounds`；当控件在
   `StackPanel`/`Grid` 中被拉伸时，居中的轨道与靠左的滑块会相距数百像素。
   修复：按 `SwitchWidth × SwitchHeight` 期望尺寸左上对齐摆放内部 Grid（`UI4Switch.cs`）。
2. **`UI4TextBlock.GradientStart` / `GradientEnd` 是死属性**：声明了 DP 但全文件从未使用，
   渐变文本实际要走 `Foreground` 传入 `LinearGradientBrush`（README 示例即如此）。未改库，Demo 已按正确用法演示。
3. **`UI4DataGrid` 与 `UI43DSphere` 是 `internal` 且全仓库无任何引用**（死代码）。
   连带问题：`System.Data.SQLite` 的原生 `SQLite.Interop.dll` 不会随类库输出到消费方，
   若将来把 `UI4DataGrid` 公开，需要一并处理原生依赖部署。
4. **5 个只写不读的私有字段**（CS0414）：`UI4ProgressRing._isLoaded / _isStartupAnimationRunning /
   _isAnimating`、`UI4ScrollViewer._isAnimatingVertical / _isAnimatingHorizontal`。
   属上游遗留状态位，删除可消除警告但会改动上游代码结构，故**保留**，构建保留这 5 条警告。
5. **`UI4ListView` / `UI4GridView` 悬浮时文字发虚**（已修复）。项模板把 `DropShadowEffect` 与
   `RenderTransform(ScaleTransform)` 挂在同一个 Border 上；WPF 中带 Effect 的元素会先光栅化成位图，
   再被 RenderTransform 缩放，文字因此被拉伸发虚。已按 `UI4Panel` 的正确结构重构：缩放放在无 Effect 的
   根 Grid（`PART_ItemRoot`），阴影独立一层，文字层不受光栅化影响。验证见 `shots/hover-list.png`。
6. **`UI4ComboBox` 长文本溢出 / 下拉显示不全**（已修复）。选中项 `ContentPresenter` 无裁剪，长文本会溢出到
   箭头列与边框外；弹出层宽度又被绑死为控件宽度，展开时长选项同样被裁。已改为：选中区 `ClipToBounds` +
   默认 `DataTemplate` 单行省略号截断（附完整文本 ToolTip，非字符串内容仍原样呈现）；弹出层改为
   `MinWidth=控件宽度`、宽度随最宽选项自适应。验证见 `shots/combo-closed.png` / `combo-open.png`。

---

## 4. 目录结构

```
StartUI4.WPF_net48/
├── StartUI4Controls.sln          解决方案（库 + Demo）
├── PORTING.md                    本文件
├── 主题方案分析与改进.md          主题切换方案的不足分析与 P0–P3 改进方案
├── shot.ps1 / interact.ps1       运行时验证脚本（截图 / UI 自动化交互）
├── theme.ps1 / scopewalk.ps1     UIA 走查（主题切换 / 局部作用域页）
├── p2verify.ps1 / p3verify.ps1   进程内色值断言（P2 引用式模板 / P3 作用域+高对比度+H 组）
├── titlebar.ps1 / titlebar-live.ps1  标题栏染色断言（进程内回读 / 跨进程读真实 Demo）
├── shots/                        验证截图（10 个分页 + 对话框/菜单/托盘）
├── upstream/                     上游原始源码（对照用）
├── src/StartUI4Controls/         net48 控件库（32 个 .cs，产出 nupkg）
└── samples/StartUI4Demo/         net48 WPF 演示程序（覆盖 29 个公开类型）
```

## 5. 构建与验证

```bash
dotnet build StartUI4Controls.sln        # 0 error；5 条为上游遗留 CS0414
```

运行时验证（已执行，全部通过，无未处理异常）：

```bash
powershell -File shot.ps1 -Tab 0..9              # 逐分页截图
powershell -File interact.ps1 -Scenario msgbox   # UI4MessageBox 打开/关闭并取返回值
powershell -File interact.ps1 -Scenario color    # UI4ColorPicker 完整 HSV 界面
powershell -File interact.ps1 -Scenario menu     # UI4Menu 下拉展开
powershell -File interact.ps1 -Scenario ctx      # UI4ContextMenu 右键弹出
powershell -File interact.ps1 -Scenario tabadd   # UI4Tab 动态新增标签
powershell -File interact.ps1 -Scenario tray     # UI4NotifyIcon 注册系统托盘
```

Demo 支持 `--tab=N` 直接打开第 N 个分页，便于自动化逐页检查。

---

## 6. 运行时版本验证（确认真跑在 .NET Framework 4.8 上）

仅看编译产物不足以证明运行时，因此做了两层实证：

1. **编译期**：反射读取产物程序集的 `TargetFrameworkAttribute`，
   `StartUI4Controls.dll` 与 `StartUI4Demo.exe` 均为 `.NETFramework,Version=v4.8`。
2. **运行期**：Demo 主窗口标题栏右侧显示
   `System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription`
   （该 API 自 .NET Framework 4.7.1 起可用；若误跑在 .NET Core/5+ 上，此处会显示
   `.NET Core ...` / `.NET 5+` 而非 `.NET Framework ...`，可立即识破）。
   本机实测输出：`实际运行时： .NET Framework 4.8.9345.0`，见 `shots/tab0.png` 右上角。

复验方式：启动 Demo 看标题栏右侧，或自行调用
`MessageBox.Show(System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);`。

---

## 7. 归档：解决方案配置（.sln）与「4.8」的关系

**结论：编译和运行所用的 4.8 都不来自 `StartUI4Controls.sln`——该文件里没有任何 4.8 / net48 字样。**
sln 只负责「用哪个 Debug/Release、哪个平台、构建哪些项目、启动哪个项目」，与目标框架无关。

### 7.1 三个「4.8」各自的真正来源

| 阶段 | 4.8 的来源 | 说明 |
|---|---|---|
| 编译期 | 各 `.csproj` 的 `<TargetFramework>net48</TargetFramework>` | MSBuild 据此解析出 `.NETFramework,Version=v4.8`，并到本机引用程序集目录 `C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.8` 取参照程序集给编译器 |
| 产物元数据 | 编译时写入程序集的 `TargetFrameworkAttribute` | 实测 `StartUI4Controls.dll` / `StartUI4Demo.exe` 均为 `.NETFramework,Version=v4.8` |
| 运行期 | `StartUI4Demo.exe.config` 的 `<supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.8"/>` + 上述程序集元数据 | 该 exe.config 由 SDK 依据 `TargetFramework` **自动生成**（仓库中并无手写 app.config）；CLR 加载器据此选择已安装的 .NET Framework 4.8。实测 `FrameworkDescription` = `.NET Framework 4.8.9345.0` |

因此：把 sln 里的配置改成任意组合（Debug/Release、Any CPU/x64/x86）都**不会**改变目标框架；
反之只改 csproj 的 `TargetFramework`，exe.config 的 sku 会在下次构建时自动跟着变。

### 7.2 sln 中各配置段的实际作用

- `GlobalSection(SolutionConfigurationPlatforms)`：定义解决方案级的 6 个「配置|平台」组合
  （Debug/Release × Any CPU/x64/x86），即 VS 工具栏下拉里可选项。
- `GlobalSection(ProjectConfigurationPlatforms)`：把每个解决方案组合映射到具体项目配置。
  每行形如 `{项目GUID}.Debug|x64.ActiveCfg = Debug|Any CPU` 与 `...Build.0 = Debug|Any CPU`：
  - `ActiveCfg`：选中该解决方案组合时，项目实际激活的配置；
  - `Build.0`：该组合下项目是否参与构建（缺省即不构建）。
  本解决方案中 x64/x86 全部映射到项目的 `Any CPU`（csproj 为单 TFM、仅定义 Any CPU），
  所以在 VS 里选 x64 **不会**产出 64 位专用程序集，产物仍是 AnyCPU；位宽由 csproj 侧（如 `Prefer32Bit`）决定，与 sln 无关。
- `GlobalSection(NestedProjects)`：解决方案文件夹（src / samples）的归属关系，纯组织结构。
- 项目条目顺序：第一个项目条目即 VS 默认启动项目（本文件已把 `StartUI4Demo` 置于首位，见第 5 节启动问题说明）。
- 不在 sln 中的内容：每个用户的启动项目选择、断点、窗口布局等存于 per-user 的 `.suo`，不随 sln 分发。

### 7.3 复验命令

```bash
grep -c "4\.8" StartUI4Controls.sln                      # 0：sln 不含 4.8
grep -H "TargetFramework>" src/StartUI4Controls/*.csproj samples/StartUI4Demo/*.csproj   # 编译期 4.8 来源
cat samples/StartUI4Demo/bin/Debug/net48/StartUI4Demo.exe.config   # 运行期 4.8 来源
```

---

## 8. 修复记录：移植后改动引入的编译回归（2026-09-29）

移植完成后，库内又做过两轮重构（`.qoder/specs/` 下的「UI4MessageBox 综合优化」与「组件库改进分析」），
引入了 3 处编译错误，IDE 表现为 Demo 启动失败：

| 报错 | 位置 | 根因 | 修复 |
|---|---|---|---|
| CS0246 `DropShadowEffect` | `src/StartUI4Controls/UI4MessageBox.cs` 36/47 行 | 「缓存 DropShadowEffect 为 static readonly」优化新增类型引用但未补 using | 补 `using System.Windows.Media.Effects;` |
| MC3074 `UI4TextBlock` 不存在 | `samples/StartUI4Demo/MainWindow.xaml` 21 行 | 级联错误：库因上一条编译失败，XAML 标记编译拿不到 `StartUI4Controls.dll` 类型；xmlns 本身无误 | 修上一条后自动消失 |
| CS1503 参数类型不匹配 | `samples/StartUI4Demo/MainWindow.xaml.cs` `SetForeground(SubtitleText, …)` | `SubtitleText` 在 XAML 中已改为 `UI4TextBlock`（`ContentControl` 派生，自带 `new Foreground` DP），不再是 WPF 原生 `TextBlock` | 直接 `SubtitleText.Foreground = CreateFrozenBrush(theme.IconColor)` |
| CS0103 `UI4ContextMenuLanguage` 不存在 | `samples/StartUI4Demo/MainWindow.xaml.cs` 316 行 | 「改进分析」重构已把 `UI4ContextMenuLanguage` 删除并入 `UI4MultiLanguage`（`UI4ContextMenu` 现统一走 `UI4MultiLanguage.Get`），Demo 调用未同步 | 删除该调用；`UI4MultiLanguage.Refresh()` 已覆盖右键菜单取词 |

注：第 3、4 条在 IDE 中被 MC3074 挡住未显示，命令行 `dotnet build` 修复第 1 条后才暴露。
验证：`dotnet build StartUI4Controls.sln` → 0 错误；Demo 可正常启动运行。

经验：第 2 节「隐式 using」结论（源码已带全部所需 using）只对**移植时点**的源码成立；
后续任何新增类型引用的重构都必须重新过 using，且以 `dotnet build` 全解决方案为准，
不能只看 IDE 当前错误列表（XAML 级联错误会掩盖 C# 侧的真实报错）。

---

## 9. 主题切换方案 P0 正确性修复（2026-09-29）

针对 `src/StartUI4Controls` 命令式主题系统的确定性 bug，实施 P0 修复（详见 `主题方案分析与改进.md` 第三节 P0 与第六节实施记录）。要点：

- **追踪生命周期**：`UI4Theme.TrackControl` 幂等挂 `Loaded`、按 `_themeVersion` 在重挂载时补齐主题；删除各控件 `Unloaded` 内的 `UntrackControl`（弱引用表本就容忍失联）。
- **通知顺序 / 短路**：`ThemeChanged` 移到控件批量刷新之后（同一 Dispatcher 回调尾部）；`SetTheme` 同模式直接 return；新增 `CurrentMode` + `StaticPropertyChanged` 供 XAML 绑定。
- **补齐失感控件**：`UI4ContextMenu`、`UI4ColorPicker` 实现 `IThemeAware`；`UI4Menu` 去掉静态 `_stylesInitialized` 翻转，颜色改实例级刷新。
- **热点 DP 跟随主题（关键新发现）**：`SyncThemeColors` 原先用 `ReadLocalValue == UnsetValue` 判断「用户是否覆盖」，但主题同步自身会写入本地值，导致下次切换被误判为已覆盖而跳过，控件**永久冻结在构造时的亮色**。新增 `Internal/ThemeSync.cs`（记录上次主题写入值、仅当前值仍等于该记录时才跟随），`UI4Menu/UI4ComboBox/UI4ListBox/UI4NavigationView` 统一改用 `ThemeSync.Apply`。

验证：`dotnet build` 0 错误；Demo 深/浅往返 + Tab 切走再回，UI4Menu 菜单栏与 UI4ContextMenu 弹出菜单深色下均为浅字深底，无残影。

---

## 10. 主题切换方案 P1：令牌化 + 资源桥 + 系统跟随 + 持久化（2026-09-29）

在 P0 基础上实施 P1（详见 `主题方案分析与改进.md` 第三节 P1 与第七节实施记录）。原则：**只增不改**——
`UI4Theme` 的 30 个公开 Color 属性与 11 个冻结 Brush 属性名/类型/取值全部保持不变，控件的 P0 命令式路径继续可用。

- **P1-1 令牌数据化**：新增 `UI4ThemeToken` 枚举与 `UI4ThemeDefinition`（`Dictionary<token,Color>`，内置 `Light()/Dark()`）；`UI4Theme` 内部改由 `_colors` 驱动，公开 Color 属性转为表达式属性；新增 `SetAccent(Color)`、`Register/Apply(key)/ThemeKeys`。
- **P1-2 资源桥**：`SetTheme` 末尾把每个令牌以 `UI4.Color.X` / `UI4.Brush.X` 写入 `Application.Resources`（附 `UI4.Brush.Text/Border/Accent` 别名），公开 `ApplyToApplication()`。Demo 宿主 XAML 改用 `{DynamicResource}`，`MainWindow.xaml.cs` 的逐元素手工同步块整体删除。
- **P1-3 系统跟随**：新增 `UI4ThemeMode.System`，读注册表 `AppsUseLightTheme` 解析、订阅 `SystemEvents.UserPreferenceChanged` 实时切换；新增 `ResolvedMode`。
- **P1-4 持久化**：`IThemePersistence` + `RegistryThemePersistence`/`JsonThemePersistence`（无第三方依赖），`UI4Theme.Persistence` 默认关闭。

验证：`dotnet build` 0 错误；STA 探针确认资源键写入、`System→ResolvedMode=Dark`、持久化 `Save/Load=Dark` 往返、`SetAccent` 派生 AccentDark；Demo 深色截图确认宿主纯靠 DynamicResource 跟随。

遗留（属 P2）：库内控件模板仍命令式重建 Style，其弹出层底色（如 UI4ComboBox 下拉）未令牌化；P2 将模板逐批改用 `SetResourceReference`，届时 `ThemeSync`/`TrackControl` 命令式通道可废弃。

---

## 11. 主题切换方案 P2 批次 1：编辑/选择类控件改引用式模板（2026-09-30）

把 `UI4CheckBox` / `UI4Radio` / `UI4TextBox` / `UI4PasswordBox` 从「切主题时整份重建 `Style`」迁移为
「`Style` 只建一次 + 令牌引用」（详见 `主题方案分析与改进.md` 第八节）。这四类控件此前**未接入主题通知**，
深色模式下输入框白底深字，是可见缺陷。构建 `dotnet build` 0 错误。

三条规则（后续批次沿用）：

- 控件自身主题色 DP：构造函数 `SetResourceReference(dp, "UI4.Color.<Token>")` / `"UI4.Brush.<Token>"`。引用式自动重解析，
  且宿主显式赋值会覆盖引用 → 天然得到「跟随主题，除非用户覆盖」。
- `FrameworkElementFactory` 的取值：改 `SetBinding` + `RelativeSource.TemplatedParent` + `Internal/ColorToBrushConverter.Instance`。
- `Style`/`Trigger` 的 `Setter.Value`：改 `new Binding(path){ Source = this, Converter = ... }`（`Setter.Value` 放不了 DynamicResource；
  触发器里 `TemplatedParent` 解析目标不确定，绑到控件实例最稳）。

配套：去 `IThemeAware` / `TrackControl` / `OnThemeChanged`；**色类** DP 去掉 `OnStyleRefresh`，
**结构类** DP（`CornerRadius`/`BoxSize`/`InnerPadding`/`TextMargin` 等）保留（它们确实需要重建模板）。
按钮半透明由 `Color.FromArgb(150, …)` 改为「不透明色绑定 + `Opacity=0.588`」，且 **Opacity 必须写在 Style 的 Setter**
（写成工厂本地值会让悬停触发器永远无法覆盖）。

验证：本机 WPF 窗口屏幕抓取（`PrintWindow` / `CopyFromScreen`）返回全白，**回退到未修改基线同样全白**，属环境限制。
改用 `p2verify.ps1`（STA 进程内、离屏 Window 承载控件、`Template.FindName` 读解析后的画刷色）做值断言，
加 `theme.ps1`（UIA 深→Tab 往返→浅）走查无异常。结果：深浅 16 项颜色与令牌一致；
`CheckBackground=Green`、`TextColor=Red` 两类显式覆盖在两主题下均保持；`Style` 实例切换前后引用相同（不再重建）。

P2 剩余批次（按同一改法逐批推进，每批构建 + 进程内断言）：
① `UI4ComboBox` / `UI4Button`；② 大模板控件 `ListBox`、`ListView`、`GridView`、`Pivot`、`NavigationView`、`TabControl`、`Menu`、`Slider`。
已知遗留：`UI4ComboBox` 下拉弹层底色在深色下仍为浅色（弹层未令牌化，属批次 ①）。

---

## 12. 主题切换方案 P3：局部主题作用域 + 高对比度（2026-09-30）

实施 P3（详见 `主题方案分析与改进.md` 第九节）。能力：**一屏/一窗一套主题**（`ui:UI4ThemeScope.Theme`）与
**高对比度主题**（`highcontrast`）。构建 `dotnet build` 0 错误；`p3verify.ps1` 44 项断言全绿；
`scopewalk.ps1`（真实 Demo 进程 UIA 走查第 11 页）19 项全绿；`p2verify.ps1` / `theme.ps1` /
`interact.ps1 -Scenario msgbox|menu|ctx` 回归均无异常。

- **新文件 `UI4ThemeScope.cs`**：附加属性 `Theme`（大小写不敏感的定义键）。置为某键时向该元素自身
  `Resources.MergedDictionaries` **末位**插入一份该主题的令牌字典，并同步刷新子树内命令式控件；
  置空 / null / **未注册的键**一律视为撤销作用域（写错键名不崩，回到全局主题）。
  作用域解析沿 `FrameworkElement.Parent` → `FrameworkContentElement.Parent` → `VisualTreeHelper.GetParent` 向上找最近声明者，
  因此能穿过 Popup 与控件模板；嵌套作用域按「控件解析到的键 == 当前作用域键」过滤，内外层互不污染。
- **`UI4Theme` 新增主题换入栈**：`UseTheme`（internal，压栈/出栈 `_current`）、`InstanceOf(key)`（按键缓存实例）、
  `EffectiveThemeFor(element)`；`RefreshEntries` / `OnTrackedControlLoaded` 改为**按每个控件的有效主题**刷新。
  这样 11 个仍走命令式刷新的控件（Button/ComboBox/Menu/ListBox/NavigationView/DataGrid/Panel/FlipTextBlock/
  ContextMenu/ColorPicker/MessageBox）**零改动**即作用域化。
  使用约束：换入只在 **UI 线程 + 同步** 区间有效，被刷新控件不得在刷新路径里 `Dispatcher.BeginInvoke` 二段跳。
- **G3 五控件顺手修复**（`UI4Switch`/`UI4ProgressBar`/`UI4Slider`/`UI4Pivot`/`UI4TextBlock`）：
  这些控件此前把浅色硬编码在 Color DP 默认值里，**连全局深色都不跟**（A8 残留）。
  现改为构造函数 `SetResourceReference`，并摘除 `IThemeAware`/`TrackControl`/`OnThemeChanged`；
  `UI4TextBlock` 因 `Foreground` 默认 null 走继承，无需引用。
- **高对比度**：`UI4ThemeDefinition.HighContrast()` 注册为键 `highcontrast`（覆盖全部 30 令牌：黑底、白字白框、
  黄强调，选中态深蓝承托白色前景），新增 `UI4ThemeMode.HighContrast`；`UI4Theme.FollowSystemHighContrast`
  默认 **false**（opt-in），开启后 `SetTheme(System)` 在系统高对比度下优先解析为 `highcontrast`，
  并订阅 `SystemEvents.UserPreferenceChanged`（含 `Accessibility` 类别）。持久化按枚举整数/名称存储，旧值兼容。
- **Demo**：新增第 11 页「局部主题」（`--tab=10`，左右同内容双卡片对照、作用域键下拉、全局高对比度/跟随系统、
  嵌套作用域示例）与 `ScopeWindow`（整窗作用域，初始取与全局相反的键）；状态一律用**窗口内文本**自证，不弹窗。

**与 P2 的关系**：作用域内命令式控件仍会重建 Style——这是过渡代价。P2 批次 ①②（上节的 11 个控件）迁移完成后，
`UseTheme`/`EffectiveThemeFor`/`RefreshControl` 整块可删除，届时作用域退化为「纯资源字典覆盖」，切换开销为 0。

已知小瑕疵：`UI4ListBox` 角标数字色在 `Dispatcher.BeginInvoke`（`UI4ListBox.cs:340`）里重绘，
作用域下该项可能取到全局色；将在 P2 批次 ② 迁移 `UI4ListBox` 时消除。
本轮**未做**切换动画（交叉淡入）：需整窗位图缓存，而本机 WPF 抓屏返回全白、无法回归验证。

## 13. 修复：深色 / 高对比度下拉框文本不清晰（2026-09-30）

用户反馈「深色和高对比度两种主题下，下拉框内文本颜色显示不清晰」。进程内探针取证（STA + 离屏 Window，
读解析后的依赖属性值）确认根因**不是**文字色错，而是**底不跟随主题**：

| 主题 | ComboBox 选中框底（修复前） | 文字色 | 结果 |
|---|---|---|---|
| Light | `#FFFFFF` | `#1E1E1E` | 正常 |
| Dark | `#FFFFFF` | `#E6E6E6` | 浅灰压白底，发虚 |
| HighContrast | `#FFFFFF` | `#FFFFFF` | **白字白底，完全不可见** |

- **主因**：`UI4ComboBox.SyncThemeColors()` 只同步 `TextColor` 与 `BorderNormalColor`，
  `EditBackground`（默认硬编码白）从未跟随主题；`FocusGradientStart/End` 亦是硬编码蓝→紫（A8 残留）。
- **同类洞**：`UI4ListBox` 的 `PanelBackground`（默认 `Brushes.White`）与 `HoverBackground`（上游遗留青色
  `#0AF5FFFF`）只在 `RefreshTheme()`（供 `UI4ContextMenu` 弹层预构建调用）里同步，`SyncThemeColors()` 里没有——
  两条刷新路径同步的属性集合不一致，属 P0-4「热点 DP 跟随主题」的漏网。实测深色下面板仍 `#FFFFFF` 而文字已 `#E6E6E6`。

改动（命令式同步通道，共 5 行 + 一处收敛）：

- `UI4ComboBox.cs` `SyncThemeColors()` 补 `EditBackground → Surface`、`FocusGradientStart → Accent`、
  `FocusGradientEnd → AccentEnd`。
- `UI4ListBox.cs` 把 `PanelBackground → Surface`、`HoverBackground → HoverOverlay` 从 `RefreshTheme()` 下沉到
  `SyncThemeColors()`，`RefreshTheme()` 收敛为 `SyncThemeColors(); Style = BuildListStyle();`，两条路径共用同一份集合。
- 全部通过 `ThemeSync.Apply`：记录「上一次由主题写入的值」，只有当前值仍等于它时才覆盖 → **宿主显式赋值永远优先**
  （`p3verify.ps1` H 组用 `EditBackground=Red` / `PanelBackground=Green` 锁定该行为）。
- 选这条通道而非 P2 批次 ② 的 `SetResourceReference`：作用域正确性**天然成立**（`OnThemeChanged` 由 P3 换入通道在
  控件的有效主题下调用），且 `UI4ContextMenu` 弹层预构建无资源继承链也能拿到色；引用式改造留给独立批次。

浅色主题外观影响：ComboBox 逐项核对为**逐字节不变**（Light `Surface=#FFFFFF`、`Accent=#0078D4`、`AccentEnd=#9333EA`
与旧硬编码值相同）；ListBox 面板底色仍白，唯一变化是项悬浮色由 `#0AF5FFFF` 变为 `#14000000`（半透黑，即主题令牌本意）。

验证：`p3verify.ps1` 新增 **H 组** 39 项断言（每主题 13 项 × light/dark/highcontrast）——三主题下闭合态对比对（选中框底/文字/焦点渐变两端）、
下拉展开态（弹层底 == `Surface`、项文字 == `TextForeground`）、`UI4ListBox` 三项、全局 light + 作用域 dark 时
作用域内控件保持 dark、宿主显式赋值不被吃掉。**全量回归**：`dotnet build` 0 错误 0 警告；
`p3verify.ps1` 83 项 PASS / 0 FAIL（原 44 项无回退）；`p2verify.ps1` 32 对色值全匹配且 Style 实例未重建；
`theme.ps1` 11 页走查无运行时错误；`interact.ps1 -Scenario combo|msgbox|menu|ctx` 均「no runtime errors」；
`scopewalk.ps1` 19 项 PASS / 0 FAIL。本机 WPF 抓屏仍全白（基线亦然），故**颜色正确性仅由进程内断言证明**。

**仍未接入主题**（记为已知限制，归 P2 批次 ②）：`UI4Button` 恒为「蓝→紫渐变 + 白字」的强调按钮，三主题下取值相同
（对比度成立，但在高对比度黑底上不与黄/白体系呼应）；`UI4ListView` / `UI4GridView` / `UI4TabControl` 无 `IThemeAware`，
卡片与文字恒浅色（可读）。



## 14. 修复：深色模式下窗口标题栏不跟随主题（2026-09-30）

现象：`SetTheme(Dark)` / `Apply("highcontrast")` 后客户区整片变暗，窗口顶部标题栏仍是亮色白条。
根因不是漏了某个 setter，而是**够不着**：标题栏属非客户区，由 DWM 绘制，WPF 的 DP、`DynamicResource`、
`FrameworkElementFactory` 模板全部无效——库内前三批主题改造从未覆盖过这条边界。

概念：DWM（Desktop Window Manager）是 Vista 起的桌面合成组件，把各窗口内容作为纹理在 GPU 合成后输出。
一个窗口因此分两块——**客户区**由程序自己画（WPF 视觉树、`Window.Background`），**非客户区**（标题栏、边框、圆角、
投影）由 DWM 画。DWM 合成后传统的 `WM_NCPAINT` 自绘通路对标准窗口失效，程序只能用 `DwmSetWindowAttribute`
（`dwmapi.dll`）设置它开放的属性：深浅标志 20（旧系统 19）、底色 35 / 文字 36 / 边框 34（Win11 起），
撤销写哨兵 `DWMWA_COLOR_DEFAULT=0x01000000`，颜色按 COLORREF `0x00BBGGRR`（非 WPF 的 `0xAARRGGBB`）。
能力面就是这四项，且**只有深浅标志可被 `DwmGetWindowAttribute` 回读**——这既限定了观感上限，也决定了验证手段。

三个方案对比（详见 `主题方案分析与改进.md` 第十一节）：① 宿主每窗口手写 `DwmSetWindowAttribute` —— 零库改动但
每窗要写码、切主题不重染、作用域/自定义主题要宿主自己算，违背「宿主零改动」定位；② `WindowChrome` 自绘标题栏 ——
观感可控到像素，但拖拽/双击/最大化/Aero Snap/系统菜单/高 DPI/无障碍全要自己保住，风险远大于收益；
③ 库内集中式 DWM 染色器 —— 只拿系统开放的颜色维度，换来零宿主改动 + 原生行为不损 + 可回读证据。**选 ③**。

落地 `src/StartUI4Controls/UI4WindowTitleBar.cs`（新增，静态类 + `Enabled` 附加属性）：
- 深浅判定按底色亮度 `0.299R+0.587G+0.114B < 128`，不枚举主题键，自定义主题天然适用；
- 先写 `DWMWA_USE_IMMERSIVE_DARK_MODE`（属性 20，失败退 19），再尝试 `CAPTION_COLOR=Background` /
  `TEXT_COLOR=TextForeground` / `BORDER_COLOR=BorderNormal`（34/35/36，Win11 起）；
- **能力探测不用版本号**：未 manifest 声明的进程里 `Environment.OSVersion` 会虚报 6.3，一律「先试、按 HRESULT 定论」并缓存；
- 四条生效通路（A/B/C 自动 + D 兜底）：**A** `UI4Theme.ThemeChanged` 后清扫 `Application.Windows`；
  **B** UI4 控件 `Loaded` 时补染所属窗口（`UI4Theme.OnTrackedControlLoaded → NotifyContentLoaded`，
  以新增 `internal UI4Theme.ThemeVersion` 去重，每窗口每代号一次）；**C** `UI4ThemeScope` 的作用域根若是 `Window`，
  `RefreshSubtree` 直接按该作用域染色；**D** 宿主手动 `Apply`（给不含 UI4 控件的纯窗口）。

改动清单（1 个新文件 + 库内 4 处挂钩）、`Apply` 的执行序列、四条通路的时序表与失败降级矩阵见
`主题方案分析与改进.md` 第十二节。

一条被否掉的通路：`EventManager.RegisterClassHandler(typeof(Window), LoadedEvent, …)` 在本机 STA 承载下实测**从不触发**
（隔离探针：安装后计数恒为 0，而实例 `Add_Loaded` 正常），故未依赖它；改由控件加载钩子覆盖「深色状态下新开的窗口」。

验证（`titlebar.ps1` 31 项 PASS / 0 FAIL，`titlebar-live.ps1` 9 项 PASS / 0 FAIL）：DWM 的 19/20 标志**可被外部回读**，
这是本主题系列里唯一能拿到硬证据的观感项——`titlebar-live.ps1` 起真实 Demo，由另一进程独立读属性：亮色启动 flag=0 →
打开 `ScopeWindow`（异主题）其标题栏 flag=1 而主窗仍 0 → 点「全局高对比度」主窗 flag=1。
配色属性 34/35/36 系统**拒绝回读**（`0x80070057`），只能断言送进 DWM 的 COLORREF 计算与探测结论一致，本机抓屏仍全白。
回归：`dotnet build` 0 错误；`p3verify.ps1` 83 项、`p2verify.ps1` 色值对全匹配、`theme.ps1`（改用标题栏 flag 作为状态探针）、
`scopewalk.ps1` 19 项、`interact.ps1 -Scenario combo|msgbox|menu|ctx` 均无运行时错误。
