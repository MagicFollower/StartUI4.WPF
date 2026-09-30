# T1 — 统一主题系统实施方案

## 架构设计

### 核心思路
创建 `UI4Theme` 静态类作为唯一的颜色令牌源。控件通过 `UI4Theme.Current.XXX` 获取当前主题颜色，不再硬编码。主题切换时触发 `ThemeChanged` 事件，所有活动控件重建样式。

### 关键设计决策

1. **静态类 + 实例属性**：`UI4Theme` 是非静态实例类，通过 `static UI4Theme Current` 访问。便于未来支持自定义主题。
2. **Color 类型为主**：令牌使用 `Color` 类型（匹配现有 DP），同时提供 `SolidColorBrush` 便捷属性（冻结）。
3. **保持 OnStyleRefresh 模式**：不改变控件的样式重建机制，仅在 BuildXxxStyle 中读取主题颜色。
4. **弱事件防止泄漏**：控件实例通过弱引用追踪，Unloaded 时自动清理。

---

## 步骤 1：创建 UI4Theme 基础设施

**新建文件**：`src/StartUI4Controls/UI4Theme.cs`

```csharp
public enum UI4ThemeMode { Light, Dark }

public class UI4Theme
{
    // 静态 Current + ThemeChanged 事件
    public static UI4Theme Current { get; private set; }
    public static event EventHandler ThemeChanged;
    
    public static void SetTheme(UI4ThemeMode mode);
    
    // 语义化颜色令牌 (~20 个)
    public Color AccentColor { get; }            // #0078D4 / Dark: #0099FF
    public Color AccentDarkColor { get; }        // #0066B5 / Dark: #0078D4
    public Color AccentEndColor { get; }         // #9333EA / Dark: #6428C8
    public Color TextForegroundColor { get; }    // #1E1E1E / Dark: #E6E6E6
    public Color TextSecondaryColor { get; }     // #000000 / Dark: #CCCCCC
    public Color BackgroundColor { get; }        // #FFFFFF / Dark: #202026
    public Color SurfaceColor { get; }           // #FFFFFF / Dark: #282830
    public Color BorderNormalColor { get; }      // #C8C8DC / Dark: #3C3C4B
    public Color BorderHoverColor { get; }       // #0078D4 / Dark: #0099FF
    public Color BorderFocusColor { get; }       // #0066B5 / Dark: #0078D4
    public Color PlaceholderColor { get; }       // LightGray / Dark: #808080
    public Color HoverOverlayColor { get; }      // Argb(20,0,0,0) / Dark: Argb(20,255,255,255)
    public Color SelectedOverlayColor { get; }   // Argb(10,0,0,0) / Dark: Argb(10,255,255,255)
    public Color TrackBackgroundColor { get; }   // Argb(10,0,0,0) / Dark: Argb(20,255,255,255)
    public Color CheckBackgroundColor { get; }   // #0066B5 / Dark: #008CD2
    public Color IconColor { get; }              // #78788C / Dark: #A0A0B4
    public Color IconHoverColor { get; }         // #3C3C50 / Dark: #C8C8DC
    public Color PanelBorderColor { get; }       // Argb(60,120,140,200) / Dark: Argb(60,100,120,180)
    public Color OffBackgroundColor { get; }     // #C8C8D2 / Dark: #3C3C46
    public Color MenuBackgroundColor { get; }    // #F8F8F8 / Dark: #2D2D32
    
    // 冻结 Brush 便捷属性
    public SolidColorBrush AccentBrush { get; }
    public SolidColorBrush BackgroundBrush { get; }
    public SolidColorBrush TextForegroundBrush { get; }
    // ... 其他常用 Brush
}
```

**实例追踪机制**（在 UI4Theme 中）：
```csharp
// 弱引用列表追踪活动控件实例
private static readonly List<WeakReference<FrameworkElement>> _trackedControls = new List<WeakReference<FrameworkElement>>();
private static readonly object _trackLock = new object();

internal static void TrackControl(FrameworkElement control)
{
    lock (_trackLock) { _trackedControls.Add(new WeakReference<FrameworkElement>(control)); }
}

internal static void UntrackControl(FrameworkElement control)
{
    lock (_trackLock) { _trackedControls.RemoveAll(w => !w.TryGetTarget(out var t) || t == control); }
}

// SetTheme 内部：遍历追踪列表触发刷新
private static void NotifyThemeChanged()
{
    lock (_trackLock)
    {
        _trackedControls.RemoveAll(w => !w.TryGetTarget(out _));
        foreach (var wr in _trackedControls.ToList())
        {
            if (wr.TryGetTarget(out var ctrl))
                Dispatcher.BeginInvoke(..., () => RefreshControl(ctrl));
        }
    }
    ThemeChanged?.Invoke(null, EventArgs.Empty);
}
```

---

## 步骤 2：修改 UI4Button（试点控件）

**文件**：`src/StartUI4Controls/UI4Button.cs`

变更：
- DP 默认值：`Color.FromRgb(0, 120, 212)` → `UI4Theme.Current.AccentColor`
- DP 默认值：`Color.FromRgb(147, 51, 234)` → `UI4Theme.Current.AccentEndColor`
- DP 默认值：`new SolidColorBrush(Color.FromRgb(0, 102, 181))` → `UI4Theme.Current.AccentDarkBrush`
- DP 默认值：`new SolidColorBrush(Colors.White)` → `UI4Theme.Current.OnWhiteBrush`
- BuildPrimaryStyle 中：`Brushes.White` → `UI4Theme.Current.OnWhiteBrush`
- 构造函数中添加：`UI4Theme.TrackControl(this); Unloaded += (s,e) => UI4Theme.UntrackControl(this);`
- 订阅主题变化：在构造函数中 `UI4Theme.ThemeChanged += (s,e) => Style = BuildPrimaryStyle();`

---

## 步骤 3：批量修改 Style 重建类控件

按 UI4Button 的模式修改以下控件（替换硬编码颜色 + 添加主题订阅）：

| 文件 | 关键替换 | 刷新方法 |
|------|---------|---------|
| `UI4TextBox.cs` | TextColor, BorderNormalColor, HoverBorderColor, FocusBorderColor, EditBackground, PlaceholderForeground, 清除按钮颜色 | `Style = BuildEditStyle()` |
| `UI4PasswordBox.cs` | 同 UI4TextBox 模式 | `Style = BuildEditStyle()` |
| `UI4CheckBox.cs` | CheckBackground, BorderNormalColor, TextColor, `Brushes.LightGray`, `Brushes.White` | `Style = BuildCheckBoxStyle()` |
| `UI4Radio.cs` | CheckBackground, BorderNormalColor, DotColor, TextColor | `Style = BuildRadioStyle()` |
| `UI4ComboBox.cs` | BorderNormalColor, FocusGradientStart/End, EditBackground, TextColor, 下拉弹出背景, 箭头颜色, item hover/selected | `Style = BuildComboStyle()` |
| `UI4Slider.cs` | GradientStart/End, TrackBackground | `Style = BuildSliderStyle()` |
| `UI4ListBox.cs` | BorderNormalColor, PanelBackground, TextColor, HoverBackground/Foreground, PressedBackground/Foreground | `Style = BuildListStyle()` |
| `UI4Panel.cs` | BorderColor, HoverBorderBrush, Background | `Style = BuildPanelStyle()` |
| `UI4TextBlock.cs` | GradientStart/End, ShadowColor | `Style = BuildTextStyle()` |
| `UI4FlipTextBlock.cs` | CardBackground, CardForeground, CardBorderBrush | 直接更新视觉元素 |
| `UI4DataGrid.cs` | HeaderBackground, HeaderForeground, RowHoverBackground, RowSelectedBackground, GridLineColor | `ApplyCustomStyle()` |
| `UI4Pivot.cs` | SelectedItemForeground, ItemForeground, ItemHoverForeground | `BuildPivotStyle()` |
| `UI4ContextMenu.cs` | BorderColor, Background, HoverBackground | 创建 Popup 时读取主题 |
| `UI4CodeEditor.cs` | ContextMenu 颜色 | 创建时读取主题 |
| `UI4MessageBox.cs` | 窗口背景, 文字颜色 | 创建时读取主题 |

---

## 步骤 4：修改直接视觉树类控件

| 文件 | 刷新方法 | 特殊处理 |
|------|---------|---------|
| `UI4Switch.cs` | `UpdateBrushes()` | 主题变化时调用 `UpdateBrushes()` |
| `UI4ProgressBar.cs` | `UpdateVisual()` | 主题变化时调用 `UpdateVisual()` |

---

## 步骤 5：处理特殊控件

### UI4NavigationView（Sealed 模板）
- 静态构造函数中 `template.Seal()` + `OverrideMetadata` — 模板一旦创建不可修改
- **方案**：将 `InitNavigationViewTemplate()` 改为非密封，主题变化时重新创建模板并调用 `OverrideMetadata`
- 或：在主题变化时重新创建整个控件样式（调用已有的样式构建方法）

### UI4Menu（XAML 资源字典）
- 使用 `InitStyles()` 创建 ResourceDictionary 并注册到 `Application.Current.Resources`
- 样式中通过 `TemplateBinding`/`Binding` 引用 DP 值（如 `BarBackground`）
- **方案**：主题变化时更新 DP 默认值 → 绑定自动传播 → 无需重建样式
- 需要重置 `_stylesInitialized = false` 并在主题变化后重新调用 `InitStyles()`

---

## 步骤 6：更新 XML 文档注释

- `UI4Theme.cs`：为类、SetTheme 方法、所有令牌属性添加 XML 注释

---

## 依赖关系

```
步骤 1 (UI4Theme.cs) ──→ 步骤 2 (UI4Button 试点)
                       ──→ 步骤 3 (批量修改) ──→ 步骤 5 (特殊控件)
                       ──→ 步骤 4 (视觉树控件)
步骤 2-5 全部完成 ──→ 步骤 6 (XML 注释)
```

步骤 2 建议先完成作为验证，然后步骤 3+4 并行推进。

---

## 风险与缓解

| 风险 | 严重度 | 缓解 |
|------|--------|------|
| **内存泄漏**：静态 ThemeChanged 事件持有控件强引用 | 高 | 使用 WeakReference 追踪列表 + Unloaded 时取消追踪 |
| **DP 默认值只求值一次**：主题切换后未显式设值的 DP 仍为旧值 | 高 | BuildXxxStyle 中读取 `UI4Theme.Current.XXX` 而非依赖 DP 默认值；DP 默认值仅影响首次创建 |
| **UI4NavigationView Sealed 模板**：无法修改已密封的 ControlTemplate | 中 | 主题变化时重新创建模板（移除 Seal 或在重建前重新实例化） |
| **N×M 重建风暴**：主题切换触发所有控件重建 | 低 | 使用 Dispatcher.BeginInvoke 批量调度，合并同一帧内的多次刷新 |
| **Light 主题回归**：修改后 Light 模式外观与当前不一致 | 低 | Light 主题颜色值必须与当前硬编码值完全一致 |
| **C# 7.3 限制**：无 switch 表达式、无 default interface methods | 低 | 使用简单属性 + switch 语句，无语言障碍 |

---

## 被否决的替代方案

### 方案 B：ResourceDictionary + DynamicResource
- **否决原因**：需要将所有 BuildXxxStyle 改为 DynamicResource 绑定，与现有 FrameworkElementFactory 模式冲突。改动量 ~500+ 行，风险过高。

### 方案 C：OverrideMetadata 修改 DP 默认值
- **否决原因**：只能覆盖 DP 默认值，无法覆盖 BuildXxxStyle 方法体内的硬编码颜色（如 `Brushes.White`、下拉弹出背景等）。不完整。

---

## 关键文件

1. `src/StartUI4Controls/UI4Theme.cs` — 新建，主题核心基础设施
2. `src/StartUI4Controls/UI4Button.cs` — 试点改造，验证方案可行性
3. `src/StartUI4Controls/UI4ComboBox.cs` — 最复杂控件（480 行），最多硬编码颜色
4. `src/StartUI4Controls/UI4NavigationView.cs` — Sealed 模板特殊处理
5. `src/StartUI4Controls/UI4Switch.cs` — 直接视觉树模式代表