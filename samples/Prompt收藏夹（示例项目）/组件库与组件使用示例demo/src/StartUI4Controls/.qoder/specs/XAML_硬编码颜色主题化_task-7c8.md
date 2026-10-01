# XAML 硬编码颜色主题化

## 颜色分类原则

| 类别 | 数量 | 处理方式 |
|------|------|---------|
| **结构性颜色**（窗口框架、表面、边框、文本） | ~18 处 | 必须响应主题 |
| **品牌/强调色**（"StartUI4.WPF" 标题蓝） | 1 处 | 保持固定 — 品牌标识 |
| **演示装饰色**（渐变、自定义配色等） | ~24 处 | 保持固定 — 展示控件能力 |
| **App.xaml 全局样式色** | 2 处 | DynamicResource 响应主题 |

---

## 步骤 1：App.xaml — 全局样式 DynamicResource 化

**文件**：`samples/StartUI4Demo/App.xaml`

添加 2 个 SolidColorBrush 资源，将 SectionTitle/SectionHint 的 Foreground 改为 DynamicResource 引用：

```xml
<Application.Resources>
    <SolidColorBrush x:Key="SectionTitleBrush" Color="#FF2861EB"/>
    <SolidColorBrush x:Key="SectionHintBrush" Color="#FF888888"/>
    <Style x:Key="SectionTitle" TargetType="TextBlock">
        <Setter Property="FontSize" Value="18"/>
        <Setter Property="FontWeight" Value="SemiBold"/>
        <Setter Property="Foreground" Value="{DynamicResource SectionTitleBrush}"/>
        <Setter Property="Margin" Value="0,18,0,8"/>
    </Style>
    <Style x:Key="SectionHint" TargetType="TextBlock">
        <Setter Property="FontSize" Value="12"/>
        <Setter Property="Foreground" Value="{DynamicResource SectionHintBrush}"/>
        <Setter Property="Margin" Value="0,0,0,6"/>
        <Setter Property="TextWrapping" Value="Wrap"/>
    </Style>
</Application.Resources>
```

**效果**：所有使用 `SectionTitle`/`SectionHint` 样式的 TextBlock（约 10 个，分布在各个 TabItem 中）自动响应主题切换，无需逐个赋值。

---

## 步骤 2：MainWindow.xaml — 添加 x:Name

**文件**：`samples/StartUI4Demo/MainWindow.xaml`

为以下 6 个元素添加 x:Name：

| 行号 | 元素描述 | 添加 x:Name | 用途 |
|------|---------|-------------|------|
| 22-23 | 副标题 UI4TextBlock | `x:Name="SubtitleText"` | 更新前景色 |
| 303 | TabItem "主页" 内容 Grid | `x:Name="TabContentHome"` | 更新背景色 |
| 306 | TabItem "文档" 内容 Grid | `x:Name="TabContentDocs"` | 更新背景色 |
| 309 | TabItem "设置" 内容 Grid | `x:Name="TabContentSettings"` | 更新背景色 |
| 329 | UI4ScrollViewer | `x:Name="DemoScrollViewer"` | 更新背景色 |
| 421 | 右键提示 TextBlock | `x:Name="CtxHostText"` | 更新前景色 |

已有 x:Name 无需修改：`RootWindow`、`HeaderBorder`、`RuntimeText`、`FooterBorder`、`StatusText`、`CtxHost`、`NavView`、`ColorSwatch`。

---

## 步骤 3：MainWindow.xaml.cs — 扩展 ApplyTheme

**文件**：`samples/StartUI4Demo/MainWindow.xaml.cs`

### 3a. 重写 ApplyTheme 方法

```csharp
private void ApplyTheme(bool isDark)
{
    UI4Theme.SetTheme(isDark ? UI4ThemeMode.Dark : UI4ThemeMode.Light);
    var theme = UI4Theme.Current;

    // ── 窗口背景 ──
    Background = theme.BackgroundBrush;

    // ── Header / Footer 表面 + 边框 ──
    if (HeaderBorder != null)
    {
        HeaderBorder.Background = theme.SurfaceBrush;
        HeaderBorder.BorderBrush = theme.BorderNormalBrush;
    }
    if (FooterBorder != null)
    {
        FooterBorder.Background = theme.SurfaceBrush;
        FooterBorder.BorderBrush = theme.BorderNormalBrush;
    }

    // ── 次要文本前景（IconColor 对应 #FF8A93A6 / #FF6B7488 语义）──
    SetForeground(SubtitleText, theme.IconColor);
    SetForeground(RuntimeText, theme.IconColor);
    SetForeground(StatusText, theme.IconColor);
    SetForeground(CtxHostText, theme.IconColor);

    // ── 内容区域表面背景 ──
    SetBackground(DemoScrollViewer, theme.SurfaceBrush);
    SetBackground(CtxHost, theme.SurfaceBrush);
    SetBackground(TabContentHome, theme.SurfaceBrush);
    SetBackground(TabContentDocs, theme.SurfaceBrush);
    SetBackground(TabContentSettings, theme.SurfaceBrush);

    // ── NavigationView 面板 + 选中背景 ──
    if (NavView != null)
    {
        NavView.LeftPanelBackground = theme.SurfaceBrush;
        NavView.SelectedItemBackground = theme.SurfaceBrush;
    }

    // ── App.xaml DynamicResource 更新（自动传播到 ~10 个 SectionTitle/SectionHint）──
    var res = Application.Current.Resources;
    res["SectionTitleBrush"] = theme.AccentBrush;
    res["SectionHintBrush"] = CreateFrozenBrush(theme.IconColor);

    // ── 右键菜单 ──
    if (_hostMenu != null)
    {
        _hostMenu.Background = theme.SurfaceBrush;
        _hostMenu.BorderColor = theme.BorderNormalColor;
        _hostMenu.HoverBackground = theme.HoverOverlayColor;
    }
}
```

### 3b. 新增辅助方法

```csharp
private static void SetForeground(TextBlock tb, Color color)
{
    if (tb != null) tb.Foreground = new SolidColorBrush(color);
}

private static void SetBackground(FrameworkElement el, Brush brush)
{
    if (el is Border b) b.Background = brush;
    else if (el is Grid g) g.Background = brush;
    else if (el is Panel p) p.Background = brush;
}

private static SolidColorBrush CreateFrozenBrush(Color color)
{
    var brush = new SolidColorBrush(color);
    brush.Freeze();
    return brush;
}
```

### 3c. 构造函数中初始化主题

在 `InitializeComponent()` 之后、`SetStatus("就绪")` 之前添加：

```csharp
ApplyTheme(false); // 初始化 Light 主题，确保所有元素颜色正确
```

---

## 颜色映射速查表

| 硬编码值 | 语义 | 主题令牌 | Light 值 | Dark 值 |
|---------|------|---------|---------|---------|
| `#FFF4F6FB` | 窗口背景 | `BackgroundBrush` | #FFFFFF | #202026 |
| `White` (表面) | 面板/控件表面 | `SurfaceBrush` | #FFFFFF | #282830 |
| `#FFE2E6F0` / `#FFDDE3EE` | 分隔线/边框 | `BorderNormalBrush` | #C8C8DC | #3C3C4B |
| `#FF8A93A6` / `#FF6B7488` / `#FF888888` | 次要文本 | `IconColor` | #78788C | #A0A0B4 |
| `#FF2861EB` (SectionTitle) | 章节标题 | `AccentBrush` | #0078D4 | #0099FF |
| `Brushes.White` (菜单) | 右键菜单背景 | `SurfaceBrush` | #FFFFFF | #282830 |
| `Rgb(200,200,210)` | 菜单边框 | `BorderNormalColor` | #C8C8DC | #3C3C4B |
| `#FFEDF1F8` | 导航面板 | `SurfaceBrush` | #FFFFFF | #282830 |

---

## 不修改的内容

| 类别 | 示例 | 原因 |
|------|------|------|
| 品牌色 | `"StartUI4.WPF"` 标题 `#FF2861EB` | 品牌标识，不随主题变化 |
| 演示装饰色 | 绿色按钮、渐变滑块、翻牌器配色等 24 处 | 展示控件自定义能力 |
| 数据绑定色 | CardItem 背景、LightYellow 卡片 | 数据驱动，与主题无关 |
| UI4Theme.cs | 不修改 | 所有需要的令牌已存在 |

---

## 关键文件

1. `samples/StartUI4Demo/App.xaml` — 添加 2 个资源，修改 2 个 Setter
2. `samples/StartUI4Demo/MainWindow.xaml` — 添加 6 个 x:Name
3. `samples/StartUI4Demo/MainWindow.xaml.cs` — 重写 ApplyTheme + 3 个辅助方法 + 构造函数初始化

---

## 被否决的方案

| 方案 | 否决原因 |
|------|---------|
| 每个元素都用 DynamicResource | 需要定义大量资源键，XAML 冗长，调试困难 |
| VisualTreeHelper 遍历更新 SectionTitle/SectionHint | 每次主题切换遍历视觉树，脆弱且不可预测 |
| 修改 Style.Setters 的 Value | Style 可能被 WPF Seal，运行时修改抛异常 |
| 在 UI4Theme 中新增 SubtleTextColor 令牌 | 现有 IconColor 已足够覆盖次要文本语义，无需扩展库 |
