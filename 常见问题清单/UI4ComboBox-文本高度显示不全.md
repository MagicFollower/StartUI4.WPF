# UI4ComboBox 选中项文本高度显示不全

## 问题现象

在 MainWindow.xaml 中使用 UI4ComboBox 时，选中项的文本在垂直方向上显示不完整，
表现为文字底部或顶部被截断，看起来像是被"削掉"了一截。

典型触发场景：
- ComboBox 设置了固定 Height（如 36 DIP）
- InnerPadding 的垂直分量（Top + Bottom）占用了较多空间
- 字体大小（默认 15 DIP）的行高接近或超过剩余可用高度

## 单位说明

本文涉及的所有数值单位均为 DIP（Device Independent Pixel，设备无关像素），
包括 Height、InnerPadding、FontSize 等。

WPF 中 FontSize 的单位是 DIP，不是 pt（磅）。
1 DIP = 1/96 英寸，在 Windows 默认 96 DPI（100% 缩放）下，1 DIP = 1 物理像素。

## 根因分析

问题由两个因素叠加导致：

### 1. 缺少默认选中项模板，文本无法垂直居中

上游版本（upstream/UI4ComboBox.cs 第 413 行）中 PART_ContentPresenter 直接绑定
SelectionBoxItemTemplate：

    contentPresenter1.SetBinding(ContentPresenter.ContentTemplateProperty,
        new Binding(nameof(SelectionBoxItemTemplate))
        {
            RelativeSource = RelativeSource.TemplatedParent
        });

当用户未显式指定 SelectionBoxItemTemplate 时，该绑定值为 null。ContentPresenter
直接渲染字符串内容，生成的 TextBlock 没有显式设置 VerticalAlignment。在 Grid 容器
（contentHostGrid）默认 Stretch 布局下，TextBlock 会填满整个可用高度；当可用高度
小于文本自然行高时，文本底部被截断。

### 2. contentHostGrid 未启用 ClipToBounds

上游版本中 contentHostGrid 没有设置 ClipToBounds = true。WPF 默认允许子元素
渲染超出父容器边界。当文本高度超出可用区域时，超出部分仍然会渲染，但会溢出到
边框圆角的裁剪区域之外，造成视觉上的"文字被切掉一部分"或"文字边缘模糊/残缺"。

## 垂直空间计算

### 默认配置参数

| 参数 | 值 | 来源 |
|------|---|------|
| Height | 36 DIP | 用户在 XAML 中设置 |
| 边框厚度 | 上下各 1 DIP | BorderThickness = (1,1,1,1)，第 316 行 |
| InnerPadding.Top | 10 DIP | 默认值 Thickness(12,10,30,10)，第 89 行 |
| InnerPadding.Bottom | 10 DIP | 默认值 Thickness(12,10,30,10)，第 89 行 |
| FontSize | 15 DIP | 构造函数硬编码，第 307 行 |

### 可用高度计算

    文本可用高度 = Height - 边框占用 - InnerPadding.Top - InnerPadding.Bottom
                 = 36 - 2 - 10 - 10
                 = 14 DIP

### 行高计算

FontSize 的单位是 DIP（设备无关像素），不是 pt（磅）。

WPF 中字体行高 = 字号 + leading（行间距）。leading 的比例取决于具体字体文件，
WPF 默认字体（Segoe UI）的 leading 比例约为字号的 1.2 倍：

    行高 = FontSize × 1.2
         = 15 × 1.2
         = 18 DIP

行高结构分解：

    ┌─ 1.5 DIP leading（上方行间距）─┐
    │                                │
    │  字形主体 15 DIP（实际文字）     │
    │                                │
    └─ 1.5 DIP leading（下方行间距）─┘
    ────────────────────────────────
    行高 = 18 DIP

### 空间差额

    可用高度:  14 DIP
    行高:      18 DIP
    差额:      14 - 18 = -4 DIP（不够）

可用高度 14 DIP 小于行高 18 DIP，文本在垂直方向上必然被裁剪 4 DIP。

### 不同对齐方式下的裁剪分布

可用高度 14 DIP，行高 18 DIP，需要裁剪 4 DIP。
行高结构：上方 leading 1.5 DIP + 字形 15 DIP + 下方 leading 1.5 DIP。

#### 顶部对齐（VerticalAlignment.Top）

文本从容器顶部开始渲染，底部 4 DIP 被裁掉：

    容器顶部 ──────────────────
    │ 1.5 DIP leading（可见）    │
    │                           │
    │ 字形顶部 12.5 DIP（可见）   │
    ├─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─┤
    │ 字形底部 2.5 DIP（被裁掉）  │  ← 字形损失 2.5 DIP
    │ 1.5 DIP leading（被裁掉）  │
    容器底部 ──────────────────

    字形被裁：2.5 DIP（全部在底部）
    视觉效果：文字"缺脚"，g/p/y 等下伸字母严重受损

#### 底部对齐（VerticalAlignment.Bottom）

文本从容器底部开始渲染，顶部 4 DIP 被裁掉：

    容器顶部 ──────────────────
    │ 1.5 DIP leading（被裁掉）  │
    │ 字形顶部 2.5 DIP（被裁掉）  │  ← 字形损失 2.5 DIP
    ├─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─┤
    │ 字形底部 12.5 DIP（可见）   │
    │                           │
    │ 1.5 DIP leading（可见）    │
    容器底部 ──────────────────

    字形被裁：2.5 DIP（全部在顶部）
    视觉效果：文字"缺头"，汉字上半部分受损

#### 居中对齐（VerticalAlignment.Center）

文本在容器内垂直居中，上下各裁 2 DIP：

    容器顶部 ──────────────────
    │ 1.5 DIP leading（被裁掉）  │
    │ 字形顶部 0.5 DIP（被裁掉）  │  ← 字形损失 0.5 DIP
    │                           │
    │ 字形中间 14 DIP（可见）     │
    │                           │
    │ 字形底部 0.5 DIP（被裁掉）  │  ← 字形损失 0.5 DIP
    │ 1.5 DIP leading（被裁掉）  │
    容器底部 ──────────────────

    字形被裁：0.5 + 0.5 = 1 DIP（上下各 0.5）
    视觉效果：几乎不可察觉，0.5 DIP 约半个物理像素

### 汇总对比

| 对齐方式 | leading 被裁 | 字形被裁 | 视觉影响 |
|---------|-------------|---------|---------|
| 顶部对齐 | 0（全保留） | 2.5 DIP（底部） | 文字"缺脚" |
| 底部对齐 | 0（全保留） | 2.5 DIP（顶部） | 文字"缺头" |
| 居中对齐 | 3 DIP（全裁完） | 1 DIP（上下各 0.5） | 几乎无感 |

结论：垂直居中并没有增加可用空间，而是将不可避免的裁剪从字形区域转移到
leading 区域，并将剩余的字形裁剪均匀分布在上下两侧，使视觉影响最小化。

### 当高度严重不足时

如果 Height 进一步缩小（如 Height=28），可用高度 = 28-2-20 = 6 DIP，
行高 18 DIP，差额 -12 DIP，上下各裁 6 DIP。此时 leading 只有 1.5 DIP，
裁剪会大幅侵入字形主体（每侧 4.5 DIP），文字上下被明显"削掉"。

此时垂直居中已无法掩盖问题，只能靠增大 Height 或减小 InnerPadding 垂直分量
让可用高度接近或超过行高。

## 完整修复方案

### 修复 1：新增默认选中项模板，TextBlock 显式垂直居中

新增 CreateDefaultSelectionTemplate() 方法（移植版第 536-553 行），当
SelectionBoxItemTemplate 为 null 时自动使用带垂直居中的 DataTemplate：

    private static DataTemplate CreateDefaultSelectionTemplate()
    {
        DataTemplate dataTemplate = new DataTemplate();
        FrameworkElementFactory grid = new FrameworkElementFactory(typeof(Grid));

        FrameworkElementFactory text = new FrameworkElementFactory(typeof(TextBlock));
        text.SetValue(TextBlock.TextWrappingProperty, TextWrapping.NoWrap);
        text.SetValue(TextBlock.TextTrimmingProperty, TextTrimming.CharacterEllipsis);
        // 关键：显式垂直居中，确保文本在可用空间内居中而非顶部对齐
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

在 PART_ContentPresenter 的 ContentTemplate 绑定中通过 TargetNullValue 使用
（移植版第 417-422 行）：

    contentPresenter1.SetBinding(ContentPresenter.ContentTemplateProperty,
        new Binding(nameof(SelectionBoxItemTemplate))
        {
            RelativeSource = RelativeSource.TemplatedParent,
            TargetNullValue = CreateDefaultSelectionTemplate()
        });

效果说明：

修复前（无模板，顶部对齐）：裁剪 4 DIP 全部压在底部，字形损失 2.5 DIP，
文字明显"缺脚"。

修复后（有模板，垂直居中）：裁剪 4 DIP 均匀分布在上下两侧，字形仅损失
1 DIP（上下各 0.5 DIP，约半个物理像素），肉眼几乎不可察觉。

注意：文本仍然被裁剪，垂直居中并没有增加可用空间。修复的本质是将裁剪
从"集中损伤字形"改为"优先消耗 leading + 均匀分摊"，使视觉影响最小化。

此方案的有效边界：

| 可用高度 | 对应 Height（默认配置） | 效果 |
|---------|----------------------|------|
| ≤ 0 DIP | ≤ 22 | 文本完全不可见，居中无意义 |
| 1~5 DIP | 23~27 | 文本勉强可见但严重压缩，居中效果差 |
| 6~14 DIP | 28~36 | 文本可见，居中明显减少字形损失 |
| ≥ 行高(18) | ≥ 40 | 文本完整显示，无裁剪 |

默认配置 Height=36（可用高度 14 DIP）落在第 3 档，居中方案效果良好。
若用户将 Height 设得更小（如 Height=10，可用高度 -12 DIP），文本完全不可见，
此时只能靠增大 Height 或减小 InnerPadding 垂直分量让可用高度 > 0。

### 修复 2：contentHostGrid 启用 ClipToBounds

    contentHostGrid.SetValue(UIElement.ClipToBoundsProperty, true);
    // 移植版第 408 行

启用后，超出 contentHostGrid 边界的内容被干净裁剪，不会出现文本溢出到圆角
边框外造成残缺的视觉效果。

### 修复 3（可选）：调整 InnerPadding 垂直分量

如果文本高度显示不全仍然存在，可以适当减小 InnerPadding 的 Top/Bottom 值，
为文本留出更多垂直空间。当前默认值为 Thickness(12, 10, 30, 10)：

    // 当前默认（垂直 10 DIP）：
    new Thickness(12, 10, 30, 10)

    // 更紧凑的选项（垂直 6 DIP，留更多空间给文本）：
    new Thickness(12, 6, 30, 6)

## Height 与 FontSize 的搭配建议

公式：

    文本可用高度 = Height - 2（边框）- InnerPadding.Top - InnerPadding.Bottom
    行高 ≈ FontSize × 1.2

    舒适显示条件：可用高度 ≥ 行高
    即 Height ≥ FontSize × 1.2 + InnerPadding.Top + InnerPadding.Bottom + 2

### 默认 InnerPadding（垂直各 10 DIP，合计 20 DIP）

| FontSize | 行高(≈1.2x) | 建议 Height | 可用高度 | 状态 |
|----------|------------|------------|---------|------|
| 12 | 14.4 DIP | 38 | 16 DIP | 充裕 |
| 15（默认） | 18 DIP | 42 | 20 DIP | 充裕 |
| 16 | 19.2 DIP | 44 | 22 DIP | 充裕 |
| 18 | 21.6 DIP | 46 | 24 DIP | 充裕 |
| 20 | 24 DIP | 48 | 26 DIP | 充裕 |

### 最小可用 Height（可用高度 = FontSize，勉强不裁字形）

| FontSize | 行高(≈1.2x) | 最小 Height | 可用高度 | 说明 |
|----------|------------|------------|---------|------|
| 12 | 14.4 | 36 | 14 DIP | 裁 leading，字形完整 |
| 14 | 16.8 | 38 | 16 DIP | 裁 leading，字形完整 |
| 15（默认） | 18 | 39 | 17 DIP | 裁少量字形，勉强可用 |
| 16 | 19.2 | 40 | 18 DIP | 裁少量字形，勉强可用 |
| 18 | 21.6 | 44 | 22 DIP | 刚好容纳 |
| 20 | 24 | 46 | 24 DIP | 刚好容纳 |

简记公式（默认 InnerPadding 下）：

    舒适 Height ≥ FontSize × 1.2 + 22
    最小 Height ≥ FontSize + 22

## 涉及文件

| 文件 | 说明 |
|------|------|
| src/StartUI4Controls/UI4ComboBox.cs | 移植版（已修复），第 89、307、408、417-422、536-553 行 |
| upstream/UI4ComboBox.cs | 上游原版（存在此问题），第 404-414 行 |

## 验证方法

    <ui:UI4ComboBox Width="220" Height="36" SelectedIndex="0">
        <ComboBoxItem>验证文本高度是否完整显示</ComboBoxItem>
    </ui:UI4ComboBox>

预期行为：
1. 文本在控件内垂直居中，上下不被截断
2. 文本不会溢出到边框圆角区域外
3. 不同 DPI 缩放下均正常显示
