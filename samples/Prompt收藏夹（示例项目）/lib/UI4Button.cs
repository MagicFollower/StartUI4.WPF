using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace StartUI4Controls
{
    /// <summary>
    /// 现代风格的按钮控件，支持圆角和渐变背景。
    /// </summary>
    /// <remarks>
    /// <para>继承自 <see cref="System.Windows.Controls.Button"/>，提供以下自定义属性：</para>
    /// <list type="bullet">
    ///   <item><see cref="CornerRadius"/> — 圆角半径</item>
    ///   <item><see cref="GradientStart"/> / <see cref="GradientEnd"/> — 水平渐变背景色</item>
    ///   <item><see cref="HoverBackground"/> — 鼠标悬停背景</item>
    /// </list>
    /// </remarks>
    public class UI4Button : Button, IThemeAware
    {
        /// <summary>获取或设置按钮的圆角半径。默认值为 6。</summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(UI4Button),
                new PropertyMetadata(new CornerRadius(6), OnStyleRefresh));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>获取或设置渐变背景的起始颜色。默认值为 #0078D4。</summary>
        public static readonly DependencyProperty GradientStartProperty =
            DependencyProperty.Register(
                nameof(GradientStart),
                typeof(Color),
                typeof(UI4Button),
                new PropertyMetadata(Color.FromRgb(0, 120, 212), OnStyleRefresh));

        public Color GradientStart
        {
            get => (Color)GetValue(GradientStartProperty);
            set => SetValue(GradientStartProperty, value);
        }

        /// <summary>获取或设置渐变背景的结束颜色。默认值为 #9333EA。</summary>
        public static readonly DependencyProperty GradientEndProperty =
            DependencyProperty.Register(
                nameof(GradientEnd),
                typeof(Color),
                typeof(UI4Button),
                new PropertyMetadata(Color.FromRgb(147, 51, 234), OnStyleRefresh));

        public Color GradientEnd
        {
            get => (Color)GetValue(GradientEndProperty);
            set => SetValue(GradientEndProperty, value);
        }

        /// <summary>获取或设置鼠标悬停时的背景画刷。</summary>
        public static readonly DependencyProperty HoverBackgroundProperty =
            DependencyProperty.Register(
                nameof(HoverBackground),
                typeof(Brush),
                typeof(UI4Button),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 102, 181)), OnStyleRefresh));

        public Brush HoverBackground
        {
            get => (Brush)GetValue(HoverBackgroundProperty);
            set => SetValue(HoverBackgroundProperty, value);
        }

        public static readonly DependencyProperty HoverBorderBrushProperty =
            DependencyProperty.Register(
                nameof(HoverBorderBrush),
                typeof(Brush),
                typeof(UI4Button),
                new PropertyMetadata(null, OnStyleRefresh));

        public Brush HoverBorderBrush
        {
            get => (Brush)GetValue(HoverBorderBrushProperty);
            set => SetValue(HoverBorderBrushProperty, value);
        }

        public static readonly DependencyProperty HoverForegroundProperty =
            DependencyProperty.Register(
                nameof(HoverForeground),
                typeof(Brush),
                typeof(UI4Button),
                new PropertyMetadata(new SolidColorBrush(Colors.White), OnStyleRefresh));

        public Brush HoverForeground
        {
            get => (Brush)GetValue(HoverForegroundProperty);
            set => SetValue(HoverForegroundProperty, value);
        }

        private static void OnStyleRefresh(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UI4Button btn)
            {
                btn.Style = btn.BuildPrimaryStyle();
            }
        }

        static UI4Button()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UI4Button),
                new FrameworkPropertyMetadata(typeof(UI4Button)));
        }

        public UI4Button()
        {
            Style = BuildPrimaryStyle();
            UI4Theme.TrackControl(this);
        }

        void IThemeAware.OnThemeChanged()
        {
            Style = BuildPrimaryStyle();
        }

        private Style BuildPrimaryStyle()
        {
            Style style = new Style(typeof(Button));
            style.Setters.Add(new Setter(ForegroundProperty, ForegroundFor(Blend(GradientStart, GradientEnd))));
            style.Setters.Add(new Setter(PaddingProperty, new Thickness(10, 0, 10, 0)));
            style.Setters.Add(new Setter(BorderThicknessProperty, new Thickness(0)));
            style.Setters.Add(new Setter(FontSizeProperty, 15d));
            style.Setters.Add(new Setter(FontWeightProperty, FontWeights.SemiBold));
            style.Setters.Add(new Setter(CursorProperty, Cursors.Hand));
            var gradient = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0.5),
                EndPoint = new Point(1, 0.5)
            };
            gradient.GradientStops.Add(new GradientStop(GradientStart, 0));
            gradient.GradientStops.Add(new GradientStop(GradientEnd, 1));
            style.Setters.Add(new Setter(BackgroundProperty, gradient));
            style.Setters.Add(new Setter(MinHeightProperty, 30d));

            ControlTemplate normalTemplate = new ControlTemplate(typeof(Button));
            FrameworkElementFactory borderNormal = new FrameworkElementFactory(typeof(Border));

            borderNormal.SetBinding(Border.CornerRadiusProperty, new Binding(nameof(CornerRadius)) { RelativeSource = RelativeSource.TemplatedParent });
            borderNormal.SetBinding(Border.BackgroundProperty, new Binding(nameof(Background)) { RelativeSource = RelativeSource.TemplatedParent });
            borderNormal.SetBinding(Border.PaddingProperty, new Binding(nameof(Padding)) { RelativeSource = RelativeSource.TemplatedParent });
            borderNormal.SetBinding(Border.BorderBrushProperty, new Binding(nameof(BorderBrush)) { RelativeSource = RelativeSource.TemplatedParent });
            borderNormal.SetBinding(Border.BorderThicknessProperty, new Binding(nameof(BorderThickness)) { RelativeSource = RelativeSource.TemplatedParent });

            FrameworkElementFactory cpNormal = new FrameworkElementFactory(typeof(ContentPresenter));
            cpNormal.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            cpNormal.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
            borderNormal.AppendChild(cpNormal);
            normalTemplate.VisualTree = borderNormal;
            style.Setters.Add(new Setter(TemplateProperty, normalTemplate));

            Trigger hoverTrigger = new Trigger
            {
                Property = IsMouseOverProperty,
                Value = true
            };
            ControlTemplate hoverTemplate = new ControlTemplate(typeof(Button));
            FrameworkElementFactory borderHover = new FrameworkElementFactory(typeof(Border));
            borderHover.SetBinding(Border.CornerRadiusProperty, new Binding(nameof(CornerRadius)) { RelativeSource = RelativeSource.TemplatedParent });
            borderHover.SetBinding(Border.BackgroundProperty, new Binding(nameof(HoverBackground)) { RelativeSource = RelativeSource.TemplatedParent });
            borderHover.SetBinding(Border.PaddingProperty, new Binding(nameof(Padding)) { RelativeSource = RelativeSource.TemplatedParent });
            borderHover.SetBinding(Border.BorderBrushProperty, new Binding(nameof(HoverBorderBrush)) { RelativeSource = RelativeSource.TemplatedParent });
            borderHover.SetBinding(Border.BorderThicknessProperty, new Binding(nameof(BorderThickness)) { RelativeSource = RelativeSource.TemplatedParent });

            FrameworkElementFactory cpHover = new FrameworkElementFactory(typeof(ContentPresenter));
            cpHover.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            cpHover.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
            cpHover.SetBinding(TextElement.ForegroundProperty, new Binding(nameof(HoverForeground)) { RelativeSource = RelativeSource.TemplatedParent });
            borderHover.AppendChild(cpHover);
            hoverTemplate.VisualTree = borderHover;

            hoverTrigger.Setters.Add(new Setter(TemplateProperty, hoverTemplate));
            style.Triggers.Add(hoverTrigger);

            // 禁用态：换掉整个模板并固定灰色背景。必须走 Template 而不是 Background/Gradient，
            // 因为使用者在 XAML 里本地设置 GradientStart/End 后，样式 Setter 无法覆盖本地值。
            ControlTemplate disabledTemplate = new ControlTemplate(typeof(Button));
            FrameworkElementFactory borderDisabled = new FrameworkElementFactory(typeof(Border));
            borderDisabled.SetBinding(Border.CornerRadiusProperty, new Binding(nameof(CornerRadius)) { RelativeSource = RelativeSource.TemplatedParent });
            borderDisabled.SetBinding(Border.PaddingProperty, new Binding(nameof(Padding)) { RelativeSource = RelativeSource.TemplatedParent });
            borderDisabled.SetValue(Border.BackgroundProperty, UI4Theme.Current.BorderNormalBrush);
            borderDisabled.SetValue(Border.BorderThicknessProperty, new Thickness(0));

            FrameworkElementFactory cpDisabled = new FrameworkElementFactory(typeof(ContentPresenter));
            cpDisabled.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            cpDisabled.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
            cpDisabled.SetValue(TextElement.ForegroundProperty, ForegroundFor(UI4Theme.Current.BorderNormalColor));
            borderDisabled.AppendChild(cpDisabled);
            disabledTemplate.VisualTree = borderDisabled;

            Trigger disabledTrigger = new Trigger
            {
                Property = IsEnabledProperty,
                Value = false
            };
            disabledTrigger.Setters.Add(new Setter(TemplateProperty, disabledTemplate));
            style.Triggers.Add(disabledTrigger);

            return style;
        }

        /// <summary>
        /// 前景色跟随背景亮度：深色底用白字，浅色底（禁用态、中性灰按钮等）用主题正文色。
        /// 只是样式 Setter 的默认值，使用方本地显式设置的 Foreground 仍然优先。
        /// </summary>
        private static SolidColorBrush ForegroundFor(Color background)
        {
            return Luminance(background) < 0.45
                ? UI4Theme.Current.OnWhiteBrush
                : UI4Theme.Current.TextForegroundBrush;
        }

        private static Color Blend(Color a, Color b)
        {
            return Color.FromRgb(
                (byte)((a.R + b.R) / 2),
                (byte)((a.G + b.G) / 2),
                (byte)((a.B + b.B) / 2));
        }

        private static double Luminance(Color c)
        {
            return 0.2126 * Channel(c.R) + 0.7152 * Channel(c.G) + 0.0722 * Channel(c.B);
        }

        private static double Channel(byte value)
        {
            double s = value / 255.0;
            return s <= 0.03928 ? s / 12.92 : System.Math.Pow((s + 0.055) / 1.055, 2.4);
        }
    }
}