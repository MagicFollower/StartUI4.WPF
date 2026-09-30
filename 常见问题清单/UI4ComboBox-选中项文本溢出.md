# UI4ComboBox 选中项文本溢出问题

## 问题现象

在 MainWindow.xaml 中使用 UI4ComboBox 时，当选中的选项文本长度超过控件可用宽度（内容列宽 = 控件总宽度 - 36px 箭头列），文本会溢出到右侧箭头区域甚至突破控件圆角边框，造成视觉上的"文字超出显示区域"。

典型触发场景：
- 控件 Width 较小（如 180px、220px），但选中项文本很长
- 下拉列表中存在长文本选项，选中后回显到标题区

演示用例（MainWindow.xaml 第 155-158 行）：

```xml
<ui:UI4ComboBox Width="220" Height="36" SelectedIndex="1">
    <ComboBoxItem>短选项</ComboBoxItem>
    <ComboBoxItem>这是一个非常非常长的选项文本，用于验证选中项不会溢出控件边界</ComboBoxItem>
</ui:UI4ComboBox>
```

## 根因分析

问题由三个缺陷共同导致：

### 1. 内容宿主 Grid 未启用裁剪（ClipToBounds）

在 BuildComboStyle() 构建的 ControlTemplate 中，contentHostGrid（承载选中项的 Grid，位于第 0 列）没有设置 ClipToBounds = true。WPF 中子元素的渲染默认可以超出父容器边界，因此当 ContentPresenter 中的文本宽度超过 contentHostGrid 的列宽时，文本会直接溢出到箭头列（第 1 列）和控件边框之外。

### 2. 选中项缺少文本截断模板

上游版本的 PART_ContentPresenter 直接绑定 SelectionBoxItemTemplate，当该属性为 null（用户未显式指定模板）时，ContentPresenter 会以默认方式呈现字符串——不换行、不截断、不限宽。文本按自然宽度渲染，超出控件可视区域后仍然完整显示。

### 3. 下拉弹出层使用固定 Width 绑定

上游版本中 PART_Popup 绑定的是 Width = ActualWidth，导致下拉列表宽度被锁死为控件宽度。当选项文本比控件更宽时，下拉列表中的长文本也会被裁切显示不全。

## 完整修复方案（4 处改动）

### 修复 1：contentHostGrid 启用 ClipToBounds

```csharp
// contentHostGrid 构建后追加：
contentHostGrid.SetValue(UIElement.ClipToBoundsProperty, true);
```

设置后 WPF 布局系统会在渲染时裁剪超出 contentHostGrid 边界的所有内容，文本不会再溢出到箭头列和边框外。

### 修复 2：创建默认选中项模板（省略号截断 + ToolTip 完整提示）

新增 CreateDefaultSelectionTemplate() 方法。当用户未指定 SelectionBoxItemTemplate 时，自动使用带 TextTrimming="CharacterEllipsis" 的 DataTemplate：

```csharp
private static DataTemplate CreateDefaultSelectionTemplate()
{
    DataTemplate dataTemplate = new DataTemplate();
    FrameworkElementFactory grid = new FrameworkElementFactory(typeof(Grid));

    FrameworkElementFactory text = new FrameworkElementFactory(typeof(TextBlock));
    text.SetValue(TextBlock.TextWrappingProperty, TextWrapping.NoWrap);
    text.SetValue(TextBlock.TextTrimmingProperty, TextTrimming.CharacterEllipsis);
    text.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
    text.SetBinding(TextBlock.TextProperty,
        new Binding { Converter = StringSelectionConverter.TextOnly });
    text.SetBinding(FrameworkElement.ToolTipProperty,
        new Binding { Converter = StringSelectionConverter.TextOnly });
    text.SetBinding(UIElement.VisibilityProperty,
        new Binding { Converter = StringSelectionConverter.WhenString });

    grid.AppendChild(text);
    dataTemplate.VisualTree = grid;
    return dataTemplate;
}
```

在 PART_ContentPresenter 的 ContentTemplate 绑定中使用 TargetNullValue：

```csharp
contentPresenter1.SetBinding(ContentPresenter.ContentTemplateProperty,
    new Binding(nameof(SelectionBoxItemTemplate))
    {
        RelativeSource = RelativeSource.TemplatedParent,
        TargetNullValue = CreateDefaultSelectionTemplate()
    });
```

效果：字符串选中项超长时自动以 ... 截断，鼠标悬停显示完整文本。

### 修复 3：新增 PART_RawContentPresenter 处理非字符串内容

对于非字符串类型的选中项（如自定义对象、DataTemplate），不应走截断模板，而是原样呈现。新增一个 PART_RawContentPresenter，通过 StringSelectionConverter.WhenNotString 控制可见性：

```csharp
FrameworkElementFactory rawPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
rawPresenter.Name = "PART_RawContentPresenter";
rawPresenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Left);
rawPresenter.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
rawPresenter.SetBinding(ContentPresenter.MarginProperty,
    new Binding(nameof(Padding)) { RelativeSource = RelativeSource.TemplatedParent });
rawPresenter.SetBinding(ContentPresenter.ContentProperty,
    new Binding(nameof(SelectionBoxItem)) { RelativeSource = RelativeSource.TemplatedParent });
rawPresenter.SetBinding(ContentPresenter.ContentTemplateProperty,
    new Binding(nameof(SelectionBoxItemTemplate)) { RelativeSource = RelativeSource.TemplatedParent });
rawPresenter.SetBinding(UIElement.VisibilityProperty,
    new Binding(nameof(SelectionBoxItem))
    {
        RelativeSource = RelativeSource.TemplatedParent,
        Converter = StringSelectionConverter.WhenNotString
    });
contentHostGrid.AppendChild(rawPresenter);
```

同时在 IsEditable Trigger 中同步折叠 PART_RawContentPresenter。

### 修复 4：下拉弹出层 Width -> MinWidth

将 PART_Popup 的宽度绑定从 Width 改为 MinWidth，使下拉列表至少与控件等宽，但允许内容撑开更宽：

```csharp
// 修复前（上游）：
dropPopup.SetBinding(FrameworkElement.WidthProperty,
    new Binding(nameof(ActualWidth)) { RelativeSource = RelativeSource.TemplatedParent });

// 修复后：
dropPopup.SetBinding(FrameworkElement.MinWidthProperty,
    new Binding(nameof(ActualWidth)) { RelativeSource = RelativeSource.TemplatedParent });
```

### 辅助：StringSelectionConverter

新增 IValueConverter 内部类，根据 SelectionBoxItem 是否为 string 分别控制两个 ContentPresenter 的可见性和文本提取：

| 实例 | 用途 | 字符串输入 | 非字符串输入 |
|------|------|-----------|-------------|
| TextOnly | 提取文本 | 返回原值 | 返回 null |
| WhenString | 截断 TextBlock 可见性 | Visible | Collapsed |
| WhenNotString | RawPresenter 可见性 | Collapsed | Visible |

## 涉及文件

| 文件 | 说明 |
|------|------|
| src/StartUI4Controls/UI4ComboBox.cs | 移植版（已修复） |
| upstream/UI4ComboBox.cs | 上游原版（存在此问题） |
| samples/StartUI4Demo/MainWindow.xaml | 演示页面，第 155-158 行有溢出测试用例 |

## 验证方法

使用上述 XML 测试用例，预期行为：
1. 选中长文本项后，标题区文本在到达箭头列之前以 ... 截断
2. 鼠标悬停在截断文本上时，ToolTip 显示完整文本
3. 下拉列表宽度能自适应最宽选项，不被锁死为控件宽度
4. 非字符串内容（如自定义 DataTemplate）仍正常渲染，不受截断模板影响
