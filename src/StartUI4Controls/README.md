# StartUI4Controls 组件库架构审计报告

> **审计日期**：2026年9月29日  
> **审计范围**：`src/StartUI4Controls` 全部 27 个源文件（约 12,000+ 行代码）  
> **审计视角**：架构完整性、设计准确性、代码一致性、主题覆盖度

---

## 一、主题系统架构（UI4Theme）

### 1.1 设计评价：良好

[UI4Theme](UI4Theme.cs) 采用 **单例 + 弱引用追踪** 模式，核心设计合理：

| 设计点 | 评价 | 说明 |
|--------|------|------|
| 弱引用追踪 | ✅ 正确 | 避免控件被主题系统持有导致内存泄漏 |
| `Freeze()` 冻结 Brush | ✅ 正确 | 9 个冻结 Brush 零分配跨线程安全 |
| Dispatcher 合并刷新 | ✅ 正确 | `DispatcherPriority.Input` 合并同一帧内的多次切换 |
| 线程安全 | ✅ 正确 | `lock (_trackLock)` 保护追踪列表 |

### 1.2 问题：颜色令牌与冻结 Brush 不对称

| 冻结 Brush（9 个） | 缺失的冻结 Brush |
|---------------------|-----------------|
| AccentBrush, AccentDarkBrush, BackgroundBrush, SurfaceBrush, TextForegroundBrush, OnWhiteBrush, PlaceholderBrush, BorderNormalBrush, MenuBackgroundBrush | **缺少**：`BorderSecondaryBrush`, `BorderHoverBrush`, `BorderFocusBrush`, `IconBrush`, `IconHoverBrush`, `HoverOverlayBrush`, `SelectedOverlayBrush`, `TrackBackgroundBrush`, `CheckBackgroundBrush`, `OffBackgroundBrush`, `ListSelectedBrush`, `HeaderBackgroundBrush`, `HeaderForegroundBrush`, `RowHoverBrush`, `RowSelectedBrush`, `GridLineBrush`, `ProgressStartBrush` |

**影响**：控件在 `BuildXxxStyle()` 中被迫用 `new SolidColorBrush(color)` 创建临时画刷，每次属性变更都产生分配。例如 `UI4CheckBox.BuildCheckBoxStyle()` 创建 `new SolidColorBrush(UI4Theme.Current.CheckBoxUncheckedBackground)`。

**严重度**：中 — 仅在主题切换和属性变更时发生，非高频路径，但与冻结 Brush 的设计哲学不一致。

### 1.3 问题：`IThemeAware` 为 `internal` 接口

`IThemeAware`（UI4Theme.cs 第 303-306 行）声明为 `internal`，这意味着：
- 外部消费者无法实现自定义主题感知控件
- 主题系统对扩展不友好

**建议**：如果这是有意限制，应在文档中说明。如果未来需要支持第三方控件主题化，应改为 `public`。

---

## 二、样式构建模式分析

### 2.1 统一模式：`OnStyleRefresh` → `BuildXxxStyle()`

所有控件遵循相同模式：
```
DP 变更 → OnStyleRefresh → Style = BuildXxxStyle() → 完整重建
```

| 控件 | 回调名 | 模式一致性 |
|------|--------|-----------|
| UI4Button | `OnStyleRefresh` | ✅ |
| UI4TextBox | `OnStyleRefresh` | ✅ |
| UI4CheckBox | `OnStyleRefresh` | ✅ |
| UI4Radio | `OnStyleRefresh` | ✅ |
| UI4Slider | `OnStyleRefresh` | ✅ |
| UI4ComboBox | `OnStyleRefresh` | ✅ |
| UI4ListBox | `OnStyleRefresh` | ✅ |
| UI4ListView | `OnStyleUpdate` | ⚠️ 命名不一致 |
| UI4DataGrid | `OnStyleChanged` | ⚠️ 命名不一致 |
| UI4TabControl | `OnStyleChanged` | ⚠️ 命名不一致 |
| UI4TextBlock | `OnStyleRefresh` | ✅ |
| UI4PasswordBox | `OnStyleRefresh` | ✅ |

**问题**：回调命名不统一（`OnStyleRefresh` / `OnStyleUpdate` / `OnStyleChanged`），建议统一为 `OnStyleRefresh`。

### 2.2 核心问题：每次属性变更完整重建 Style

**所有控件** 在任意 DP 变更时都完整重建整个 `Style` 对象（含 `ControlTemplate`、`Trigger`、`Binding` 等），这导致：

| 问题 | 影响 | 示例 |
|------|------|------|
| 临时对象风暴 | 每次属性变更创建 20-50 个临时对象 | `UI4ComboBox.BuildComboStyle()` 约 225 行，每次创建 ~40 个对象 |
| Binding 重新建立 | 所有 TemplateBinding/Binding 重新创建 | UI4ComboBox 有 ~20 个 Binding |
| 触发器重新创建 | Trigger/EventSetter 全部重建 | UI4ListBox 有 3 个 EventSetter |

**根因**：使用 `FrameworkElementFactory` 构建模板，无法像 XAML 模板那样被 WPF 引擎缓存和共享。

**建议**：对于不频繁变更的属性（如 `CornerRadius`），可以考虑使用 `TemplateBinding` 或 `Binding` 到 DP，而非在 `BuildXxxStyle()` 中硬编码值。这样属性变更时 WPF 引擎自动更新，无需重建整个 Style。

---

## 三、DependencyProperty 设计审计

### 3.1 默认值硬编码与主题脱节

| 控件 | 属性 | 硬编码默认值 | 主题对应值 | 问题 |
|------|------|-------------|-----------|------|
| UI4TextBox | `BorderNormalColor` | `Color.FromRgb(200, 200, 220)` | `BorderNormalColor` Light: 同值 ✅ | 初始一致，但主题切换后用户设置会覆盖 |
| UI4TextBox | `EditBackground` | `Color.FromRgb(255, 255, 255)` | `SurfaceColor` Light: White ✅ | Dark 模式下默认值错误 |
| UI4TextBox | `TextColor` | `Color.FromRgb(30, 30, 30)` | `TextForegroundColor` Light: 同值 ✅ | Dark 模式下默认值错误 |
| UI4CheckBox | `CheckBackground` | `Color.FromRgb(0, 102, 181)` | `CheckBackgroundColor` Light: 同值 ✅ | — |
| UI4Radio | `TextColor` | `Colors.Black` | `TextForegroundColor` Light: `#1E1E1E` | ⚠️ 不一致 |
| UI4ListBox | `BorderNormalColor` | `Color.FromRgb(37, 99, 235)` | 无对应主题色 | ⚠️ 蓝色边框？ |
| UI4Slider | `TrackBackground` | `Color.FromArgb(255, 255, 255, 255)` | `TrackBackgroundColor` Light: `Argb(10,0,0,0)` | ⚠️ 不一致 |
| UI4ProgressBar | `TrackBackground` | `Color.FromArgb(10, 0, 0, 0)` | `TrackBackgroundColor` 同值 ✅ | — |

**关键问题**：DP 默认值在编译时确定，无法感知主题。当用户不显式设置这些属性时，Dark 模式下控件颜色将是错误的（白色背景、黑色文字等）。

**建议**：
1. 在构造函数中将 DP 默认值设置为主题令牌值
2. 或在 `BuildXxxStyle()` 中使用 `ReadLocalValue()` 检测用户是否显式设置，未设置时使用主题值

### 3.2 `new` 关键字隐藏基类属性

| 控件 | 隐藏的属性 | 风险 |
|------|-----------|------|
| UI4TextBlock | `ForegroundProperty`, `PaddingProperty`, `FontSizeProperty`, `FontWeightProperty` | ⚠️ 4 个 `new` 隐藏，可能导致 XAML 绑定歧义 |

`UI4TextBlock` 用 `new` 重新注册了 4 个继承自 `ContentControl`/`Control` 的属性。这会导致：
- 通过基类引用设置属性时行为不一致
- XAML 中可能解析到错误的 DP

---

## 四、代码重复分析

### 4.1 滚动条淡入淡出逻辑 — 3 处重复

| 位置 | 行数 | 逻辑 |
|------|------|------|
| UI4TextBox（第 190-251 行） | ~60 行 | ScrollBar 淡入淡出 + DispatcherTimer |
| UI4PasswordBox（第 292-351 行） | ~60 行 | **完全相同**的 ScrollBar 淡入淡出逻辑 |
| UI4DataGrid（第 358-386 行） | ~30 行 | 类似逻辑（async Task 版本） |

**建议**：提取为 `ScrollBarFadeHelper` 共享类。

### 4.2 右键菜单初始化 — 4 处重复

| 位置 | 菜单项 |
|------|--------|
| UI4TextBox.InitCustomMenu() | Undo/Cut/Copy/Paste/Delete/SelectAll |
| UI4PasswordBox.InitCustomMenu() | 密码/明文模式分别配置 |
| UI4TextBlock.InitCustomMenu() | Copy/SelectAll |
| UI4DataGrid.InitContextMenu() | Copy/SelectAll |

### 4.3 `ColorToBrushConverter` — 3 处独立定义

| 位置 | 类名 |
|------|------|
| UI4Radio（第 195-208 行） | `ColorToBrushConverter`（private nested） |
| UI4NavigationView（第 41-52 行） | `NavigationColorToBrushConverter`（internal） |
| UI4TabControl（第 722-735 行） | `TabColorToBrushConverter`（internal） |

**建议**：统一为一个 `internal` 共享的 `ColorToBrushConverter`。

### 4.4 `BoolToVisibilityConverter` — 2 处独立定义

| 位置 | 可见性 |
|------|--------|
| UI4TextBox（第 425-431 行） | `public` |
| UI4TabControl（第 705-720 行） | `internal` |

---

## 五、主题响应覆盖度审计

### 5.1 IThemeAware 实现覆盖

| 控件 | 实现 IThemeAware | TrackControl/UntrackControl | OnThemeChanged |
|------|:---:|:---:|:---:|
| UI4Button | ✅ | ✅ | ✅ |
| UI4TextBox | ✅ | ✅ | ✅ |
| UI4CheckBox | ✅ | ✅ | ✅ |
| UI4Radio | ✅ | ✅ | ✅ |
| UI4Switch | ✅ | ✅ | ✅ |
| UI4Slider | ✅ | ✅ | ✅ |
| UI4ProgressBar | ✅ | ✅ | ✅ |
| UI4ComboBox | ✅ | ✅ | ✅ |
| UI4PasswordBox | ✅ | ✅ | ✅ |
| UI4ListBox | ✅ | ✅ | ✅ |
| UI4TextBlock | ✅ | ✅ | ✅ |
| UI4NavigationView | ✅ | ✅ | ✅ |
| UI4DataGrid | ✅ | ✅ | ✅ |
| UI4MessageBox | ✅ | ✅ | ✅ |
| **UI4ListView** | ❌ | ❌ | ❌ |
| **UI4Tab** | ❌ | ❌ | ❌ |
| **UI4ScrollViewer** | ❌ | ❌ | ❌ |
| **UI4ContextMenu** | ❌（非控件） | — | — |

### 5.2 未实现主题响应的控件分析

**UI4ListView**：
- 不实现 `IThemeAware`，不调用 `TrackControl`
- `ItemBackground` 默认 `White`，`ItemBorderBrush` 默认 `Argb(60,120,140,200)` — Dark 模式下不协调
- 使用 XAML 字符串解析的 ScrollViewer 样式（第 380-593 行），内含硬编码颜色 `#50000000`

**UI4Tab**：
- 不实现 `IThemeAware`
- `HeaderBackground` 默认 `Argb(10,0,0,0)` — Dark 模式下太浅
- `TabSelectedBackground` 默认 `Colors.White` — Dark 模式下刺眼
- `TabForeground` 默认 `Argb(200,0,0,0)` — Dark 模式下不可见
- 所有颜色都是面向 Light 模式的硬编码值

---

## 六、命名规范审计

### 6.1 属性命名不一致

| 属性概念 | 不同命名 | 出现位置 |
|---------|---------|---------|
| 边框颜色 | `BorderNormalColor` / `ItemBorderBrush` | TextBox, CheckBox, ListBox vs ListView |
| 表面背景 | `EditBackground` / `SurfaceColor` / `PanelBackground` | TextBox/ComboBox vs Theme vs ListBox/TextBlock |
| 文字颜色 | `TextColor` / `TextForegroundColor` | TextBox/CheckBox/Radio vs Theme |
| 悬停背景 | `HoverBackground` / `HoverOverlayColor` / `RowHoverBackground` | Button/ListBox vs Theme vs DataGrid |
| 选中背景 | `PressedBackground` / `SelectedOverlayColor` / `RowSelectedBackground` | ListBox vs Theme vs DataGrid |

### 6.2 类型不一致

同一概念在不同控件中使用不同类型：

| 属性 | UI4Button | UI4TextBox | UI4CheckBox | UI4DataGrid |
|------|-----------|-----------|-------------|-------------|
| 背景 | `Brush` | `Brush` | `Color` | `Brush` |
| 边框 | `Brush`（HoverBorderBrush） | `Color`（BorderNormalColor） | `Color` | `Brush` |
| 文字 | — | `Color`（TextColor） | `Color`（TextColor） | `Brush`（HeaderForeground） |

**建议**：统一为 `Brush` 类型（与 WPF 原生控件一致），或统一为 `Color` 类型。

---

## 七、架构完整性评分

| 维度 | 评分 | 关键发现 |
|------|------|---------|
| **主题系统** | ★★★★☆ | 核心设计正确，冻结 Brush 覆盖不全 |
| **样式构建** | ★★★☆☆ | 全量重建模式导致对象风暴，Binding 无法利用 WPF 缓存 |
| **DP 设计** | ★★★☆☆ | 默认值硬编码与 Dark 模式脱节，`new` 隐藏基类属性 |
| **代码复用** | ★★☆☆☆ | 滚动条淡入淡出、右键菜单、ColorToBrushConverter 多处重复 |
| **命名一致性** | ★★★☆☆ | 回调名、属性名、类型在不同控件间不统一 |
| **主题覆盖** | ★★★★☆ | 14/17 控件实现 IThemeAware，UI4ListView/UI4Tab 缺失 |
| **内存管理** | ★★★★★ | 弱引用追踪 + Unloaded 清理，设计正确 |
| **安全性** | ★★★★☆ | PasswordBox 明文存储已标注风险，Unloaded 时清除密码 |
| **可扩展性** | ★★★☆☆ | IThemeAware 为 internal，外部无法扩展主题感知 |

---

## 八、优先改进建议

| 优先级 | 改进项 | 影响范围 | 工作量 |
|--------|--------|---------|--------|
| **P0** | UI4ListView/UI4Tab 实现 IThemeAware | 2 个控件 | 小 |
| **P0** | DP 默认值在构造函数中初始化为主题令牌 | 全部控件 | 中 |
| **P1** | 提取共享 ScrollBarFadeHelper | 3 个文件 | 小 |
| **P1** | 悬浮缩放 + 阴影在 `UI4ListView` / `UI4GridView` / `UI4Panel` 三处逐字复制；缩放余量契约（`FitHoverScale` / `HoverMaxGrow` / `EdgeReserve`）已在前两处同步，**改第三处时必须同步**，否则不越界保证会破 | 3 个文件 | 中 |
| **P1** | 提取共享 ColorToBrushConverter / BoolToVisibilityConverter | 5 个文件 | 小 |
| **P1** | 统一回调命名为 `OnStyleRefresh` | 3 个文件 | 小 |
| **P2** | 补全冻结 Brush（至少覆盖常用 10 个） | UI4Theme | 小 |
| **P2** | 统一属性类型（Color vs Brush） | 跨控件 | 中 |
| **P3** | 将 IThemeAware 改为 public | 1 个文件 | 极小 |
| **P3** | UI4TextBlock 消除 `new` 隐藏 | 1 个文件 | 小 |
