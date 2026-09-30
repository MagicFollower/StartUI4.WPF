# Demo P1/P2/P3 修复

## 修改范围

仅修改 2 个文件，不修改库代码：
- `samples/StartUI4Demo/MainWindow.xaml.cs`
- `samples/StartUI4Demo/MainWindow.xaml`

---

## P1：UI4MessageBox.Show() 补充 `owner: this`

**文件**：`MainWindow.xaml.cs`

**第 175 行**（MsgOk_Click）：
```csharp
// 前：
bool? r = UI4MessageBox.Show("这是 UI4MessageBox 的内容。", "提示", UI4MessageBoxButtons.OK);
// 后：
bool? r = UI4MessageBox.Show("这是 UI4MessageBox 的内容。", "提示", UI4MessageBoxButtons.OK, owner: this);
```

**第 181 行**（MsgOkCancel_Click）：
```csharp
// 前：
bool? r = UI4MessageBox.Show("确认执行该操作吗？", "请确认", UI4MessageBoxButtons.OKCancel);
// 后：
bool? r = UI4MessageBox.Show("确认执行该操作吗？", "请确认", UI4MessageBoxButtons.OKCancel, owner: this);
```

使用命名参数 `owner:` 跳过 `width` 默认参数，与 ColorPicker 调用（第 193 行传 `this`）保持一致。

---

## P2：复用已有"深色模式"开关接入主题切换

### 发现

XAML 第 65-68 行已有一个标签为"深色模式"的 `UI4Switch`：
```xml
<ui:UI4Switch IsOn="True" Toggled="Switch_Toggled"/>
<TextBlock Text="深色模式" .../>
```
但 `Switch_Toggled`（cs 第 115-119 行）仅输出状态文本，未调用 `UI4Theme.SetTheme()`。

### 步骤 2a：XAML 变更

**1. 修改开关初始值**（第 66 行）— `IsOn="True"` → `IsOn="False"`：
```xml
<ui:UI4Switch IsOn="False" Toggled="Switch_Toggled"/>
```
启动时默认 Light 主题，开关 Off 状态与主题一致。

**2. 给 Window 添加 `x:Name`**（第 1 行）：
```xml
<Window x:Name="RootWindow" x:Class="StartUI4Demo.MainWindow" ...>
```

**3. 给顶部 Header Border 添加 `x:Name`**（第 18 行）：
```xml
<Border x:Name="HeaderBorder" Grid.Row="0" Background="White" ...>
```

**4. 给底部状态栏 Border 添加 `x:Name`**（第 445 行）：
```xml
<Border x:Name="FooterBorder" Grid.Row="2" Background="White" ...>
```

### 步骤 2b：代码变更

**修改 `Switch_Toggled`**（cs 第 115-119 行），追加主题切换调用：
```csharp
private void Switch_Toggled(object sender, RoutedEventArgs e)
{
    UI4Switch sw = (UI4Switch)sender;
    SetStatus("UI4Switch IsOn = " + sw.IsOn);
    ApplyTheme(sw.IsOn);
}
```

**新增 `ApplyTheme` 方法**：
```csharp
private void ApplyTheme(bool isDark)
{
    UI4Theme.SetTheme(isDark ? UI4ThemeMode.Dark : UI4ThemeMode.Light);
    var theme = UI4Theme.Current;
    Background = theme.BackgroundBrush;
    if (HeaderBorder != null) HeaderBorder.Background = theme.SurfaceBrush;
    if (FooterBorder != null) FooterBorder.Background = theme.SurfaceBrush;
}
```

**依赖的 UI4Theme API**（已存在于库中，无需修改）：
- `UI4Theme.SetTheme(UI4ThemeMode)` — 切换主题，自动刷新所有 IThemeAware 控件
- `UI4Theme.Current.BackgroundBrush` — 冻结的窗口背景画刷（Light: #F4F6FB, Dark: #202026）
- `UI4Theme.Current.SurfaceBrush` — 冻结的表面色画刷（Light: #FFFFFF, Dark: #282830）

**行为说明**：
- UI4* 控件（UI4Button、UI4TextBlock 等）已通过 `TrackControl` + `IThemeAware` 自动响应主题切换
- 标准 WPF 控件（Border、Window）需手动更新 — `ApplyTheme` 处理窗口级元素
- 内部演示区的硬编码色（如 TabItem 内容区的 `Background="White"`）保持不变，作为演示限制可接受

---

## P3：SolidColorBrush 安全转换

**文件**：`MainWindow.xaml.cs`，`PickColor_Click` 方法（第 191-208 行）

**第 193-196 行**：
```csharp
// 前：
Color? result = UI4ColorPicker.ShowDialog(
    "选择颜色",
    ((SolidColorBrush)ColorSwatch.Background).Color,
    this);

// 后：
var currentBrush = ColorSwatch.Background as SolidColorBrush;
Color initialColor = currentBrush != null ? currentBrush.Color : Colors.Blue;
Color? result = UI4ColorPicker.ShowDialog("选择颜色", initialColor, this);
```

用 `as` 软转换替代硬转，避免 `InvalidCastException`。回退色 `Colors.Blue` 与原色 `#FF2861EB`（蓝色系）视觉接近。

---

## 被否决的方案

| 方案 | 否决原因 |
|------|---------|
| 在标题栏新增独立主题切换开关 | 已有"深色模式"开关可复用，新增会造成两个开关控制同一功能 |
| P3 使用缓存字段 `_currentSwatchColor` | 增加维护负担（需与 XAML 初始值同步），`as` 转换已足够安全 |
| 更新所有 XAML 硬编码颜色响应主题 | 改动量大（13+ 处），内部演示区保持固定色是可接受的演示限制 |
| P3 中 `Freeze()` 新建的 SolidColorBrush | 演示代码，非性能关键路径，保持简洁优先 |

---

## 关键文件

1. `samples/StartUI4Demo/MainWindow.xaml.cs` — P1（2 行）、P2（修改 Switch_Toggled + 新增 ApplyTheme）、P3（3 行）
2. `samples/StartUI4Demo/MainWindow.xaml` — P2（3 个 x:Name + 1 个 IsOn 修改）
3. `src/StartUI4Controls/UI4Theme.cs` — 只读参考（API：SetTheme、Current、BackgroundBrush、SurfaceBrush）
