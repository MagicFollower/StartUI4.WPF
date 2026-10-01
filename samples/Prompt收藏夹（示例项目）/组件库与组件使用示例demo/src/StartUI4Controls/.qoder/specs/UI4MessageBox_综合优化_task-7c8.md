# UI4MessageBox 综合优化方案

## 问题全景

从三个视角（简洁性/可维护性、性能/可扩展性、最小变更/风险控制）审视 `UI4MessageBox.cs`（379 行），识别出以下问题：

| # | 问题 | 严重度 | 视角 |
|---|------|--------|------|
| 1 | UI4Panel 无意义包装 — 所有功能禁用但仍触发 5 次 `BuildPanelStyle()` 重建（125+ 临时对象） | 高 | 性能 |
| 2 | 未实现 `IThemeAware` — 主题切换时背景/前景不更新 | 高 | 风险 |
| 3 | 与 UI4ColorPicker 存在 ~120 行完全重复代码（resize + 动画） | 中 | 维护性 |
| 4 | 构造函数 137 行，混合 4 种职责 | 中 | 维护性 |
| 5 | 魔法数字泛滥（resize 方向 1-8、动画参数 90/0.88/14/200） | 中 | 维护性 |
| 6 | `Dispatcher.Invoke` 冗余（已在 UI 线程） | 低 | 性能 |
| 7 | 变量命名不规范（`winUI4Style_Panel`） | 低 | 维护性 |
| 8 | FontFamily/DropShadowEffect 每次实例化重复创建 | 低 | 性能 |
| 9 | 缺少私有方法 XML 文档注释 | 低 | 维护性 |

---

## 步骤 1：移除 UI4Panel 包装层（性能收益最大）

**文件**：`src/StartUI4Controls/UI4MessageBox.cs`

**问题**：第 165-177 行创建 UI4Panel 但禁用其全部功能（`HoverScale=1, ShadowBlurRadius=0, ShadowOpacity=0, BorderThickness=0, Background=Transparent`）。UI4Panel 构造时先调用 `BuildPanelStyle()`（第 179 行），然后 5 个属性赋值各触发一次 `OnStyleUpdate → BuildPanelStyle()`（UI4Panel.cs 第 162-166 行），每次重建创建 25+ 个对象。总计 **125+ 个临时对象在首次渲染前即被丢弃**。

**方案**：用简单 `Border` 替代 UI4Panel：

```csharp
// 替换第 165-177 行
Border rootBorder = new Border
{
    Margin = new Thickness(20),
    Background = Brushes.Transparent
};
rootBorder.Child = resizeGrid;
this.Content = rootBorder;
```

**关联修改**：
- 第 213 行：`this.Content is UI4Panel rootPanel` → `this.Content is Border rootPanel`
- 第 255 行：同上

**收益**：消除 125+ 临时对象、1 层布局 pass、1 次 Theme.TrackControl 注册。

---

## 步骤 2：添加 IThemeAware 主题响应

**文件**：`src/StartUI4Controls/UI4MessageBox.cs`

**问题**：UI4MessageBox 在构造时一次性读取 `UI4Theme.Current.SurfaceBrush`（第 62 行），主题切换后不更新。`_headingText`、`_messageText`、`_iconText` 未设置 Foreground，依赖 WPF 继承。

**方案**：

```csharp
// 2a. 类声明（第 27 行）添加 IThemeAware
public class UI4MessageBox : Window, IThemeAware

// 2b. 构造函数末尾（第 178 行后）添加追踪
UI4Theme.TrackControl(this);
this.Unloaded += (s, e) => UI4Theme.UntrackControl(this);

// 2c. 构造时为文本元素设置初始前景色
_iconText.Foreground = new SolidColorBrush(UI4Theme.Current.IconColor);
_headingText.Foreground = UI4Theme.Current.TextForegroundBrush;
_messageText.Foreground = UI4Theme.Current.TextForegroundBrush;

// 2d. 实现 OnThemeChanged
void IThemeAware.OnThemeChanged()
{
    _mainContainer.Background = UI4Theme.Current.SurfaceBrush;
    _headingText.Foreground = UI4Theme.Current.TextForegroundBrush;
    _messageText.Foreground = UI4Theme.Current.TextForegroundBrush;
    _iconText.Foreground = new SolidColorBrush(UI4Theme.Current.IconColor);
}
```

> 注：UI4Theme 当前有 `IconColor`（Color 类型）但无 `IconBrush`。可考虑在 UI4Theme 中添加 `IconBrush` 冻结画刷属性，或直接在 MessageBox 中使用 `new SolidColorBrush(UI4Theme.Current.IconColor)`。

---

## 步骤 3：提取共享辅助类（消除 ~120 行重复）

**新建文件**：
- `src/StartUI4Controls/Internal/WindowResizeBehavior.cs` — 封装 8 方向 resize 逻辑
- `src/StartUI4Controls/Internal/WindowAnimationHelper.cs` — 提取开/关动画

**问题**：UI4MessageBox（第 137-163, 340-377, 211-318 行）与 UI4ColorPicker（第 401-429, 868-905, 785-866 行）存在逐行相同的代码块，共 ~120 行。

**方案 3a — WindowResizeBehavior**：

```csharp
// Internal/WindowResizeBehavior.cs
internal class WindowResizeBehavior
{
    private int _direction;
    private Point _startPoint;
    private double _startWidth, _startHeight;
    private readonly Window _window;
    private const double MinWidth = 200;
    private const double MinHeight = 150;
    private const double ThumbSize = 8;

    public WindowResizeBehavior(Window window) { _window = window; }

    public void Attach(Grid contentGrid)
    {
        // 创建 8 个透明 Border，使用 Tag 存储方向，共享单一事件处理方法
        // 替代 UI4MessageBox.cs 第 138-163 行的重复代码
    }

    private void OnResizeBorderMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is Border b) StartResize(e, (int)b.Tag);
    }
    // StartResize / DoResize / EndResize 从 UI4MessageBox 第 340-377 行提取
}
```

**方案 3b — WindowAnimationHelper**：

```csharp
// Internal/WindowAnimationHelper.cs
internal static class WindowAnimationHelper
{
    private const double SlideOffset = 90;
    private const double ScaleFrom = 0.88;
    private const double BlurRadius = 14;
    private static readonly TimeSpan Duration = TimeSpan.FromMilliseconds(200);

    /// <summary>窗口打开动画：淡入 + 上滑 + 放大 + 去模糊。</summary>
    internal static void PlayOpenAnimation(FrameworkElement root) { ... }

    /// <summary>窗口关闭动画。完成后调用 onComplete 回调。</summary>
    internal static void PlayCloseAnimation(FrameworkElement root, Action onComplete) { ... }
}
```

**同步修改 UI4ColorPicker.cs**：删除重复的 resize/动画方法，改为调用辅助类。

---

## 步骤 4：拆分构造函数

**文件**：`src/StartUI4Controls/UI4MessageBox.cs`

**问题**：构造函数（第 42-179 行）137 行，混合窗口配置、容器创建、内容构建、resize 绑定 4 种职责。

**方案**：拆分为职责单一的私有方法：

```csharp
public UI4MessageBox(string title, string content, UI4MessageBoxButtons buttonMode = ...)
{
    _buttonMode = buttonMode;
    ConfigureWindowProperties();
    BuildMainContainer();
    BuildContentLayout(title, content);
    AttachDragAndResize();
    this.Loaded += Window_LoadedAnim;
    UI4Theme.TrackControl(this);
    this.Unloaded += (s, e) => UI4Theme.UntrackControl(this);
}

private void ConfigureWindowProperties() { /* 第 46-57 行 */ }
private void BuildMainContainer() { /* 第 59-71 行 */ }
private void BuildContentLayout(string title, string content) { /* 第 73-129 行 */ }
private void AttachDragAndResize() { /* 第 132-178 行，使用 WindowResizeBehavior */ }
```

---

## 步骤 5：常量化魔法数字 + 枚举替代方向码

**文件**：`src/StartUI4Controls/UI4MessageBox.cs` + `Internal/WindowResizeBehavior.cs`

```csharp
// 窗口尺寸常量（替代第 46-48, 357 行的魔法数字）
private const double DefaultWidth = 460;
private const double DefaultMinHeight = 160;
private const double DefaultMaxHeight = 400;

// Resize 方向枚举（替代方向码 1-8）
internal enum ResizeDirection
{
    Left = 1, Right = 2, Top = 3, Bottom = 4,
    TopLeft = 5, TopRight = 6, BottomLeft = 7, BottomRight = 8
}
```

---

## 步骤 6：小型优化（低风险独立改动）

| 改动 | 位置 | 说明 |
|------|------|------|
| 缓存 FontFamily | 第 53, 84 行 | 提取为 `static readonly FontFamily` |
| 缓存 DropShadowEffect | 第 64-70 行 | 提取为 `static readonly`，调用 `Freeze()` |
| 移除冗余 Dispatcher.Invoke | 第 301-305 行 | 已在 UI 线程，直接执行 |
| 修正变量命名 | 第 165 行 | `winUI4Style_Panel` → `rootBorder`（步骤 1 已解决） |
| 补充 XML 注释 | 第 211, 252, 340, 351, 371 行 | 为私有方法添加 summary |

---

## 依赖关系

```
步骤 1 (移除UI4Panel) ─┐
步骤 2 (IThemeAware)   ├──→ 步骤 4 (拆分构造函数)
步骤 3a (ResizeHelper) ─┤
步骤 3b (AnimHelper)  ──┤
步骤 5 (常量化)        ──┘──→ 步骤 3 同步修改 UI4ColorPicker
步骤 6 (小型优化) ──────── 独立，可随时执行
```

建议执行顺序：1 → 2 → 6 → 3 → 5 → 4

---

## 被否决的方案

| 方案 | 否决原因 |
|------|---------|
| **移除 BlurEffect 动画** | BlurEffect 仅在 200ms 开/关动画期间运行，窗口面积小（460×160+），实际影响极有限。移除会降低视觉过渡质量，收益不成比例 |
| **8 个 resize Border 合并为单个自定义 Panel** | 视觉树元素减少 7 个，但引入自定义 Panel 增加复杂度。MessageBox 是低频控件，8 个 Border 的开销可忽略 |
| **添加 Owner 参数到 Show()** | 属于 API 扩展，不是优化，可能影响现有调用方 |

---

## 关键文件

1. `src/StartUI4Controls/UI4MessageBox.cs` — 主要重构目标
2. `src/StartUI4Controls/UI4ColorPicker.cs` — 重复代码来源，步骤 3 需同步修改
3. `src/StartUI4Controls/Internal/WindowResizeBehavior.cs` — 新建
4. `src/StartUI4Controls/Internal/WindowAnimationHelper.cs` — 新建
5. `src/StartUI4Controls/UI4Panel.cs` — 被移除的包装层（参考其 BuildPanelStyle 开销）
6. `src/StartUI4Controls/UI4Theme.cs` — 主题系统参考
