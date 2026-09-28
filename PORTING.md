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
├── shot.ps1 / interact.ps1       运行时验证脚本（截图 / UI 自动化交互）
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
