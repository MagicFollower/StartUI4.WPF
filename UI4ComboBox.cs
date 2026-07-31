using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
namespace StartUI4Controls
{
    public class UI4ComboBox : ComboBox
    {
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(UI4ComboBox),
                new PropertyMetadata(new CornerRadius(8), OnStyleRefresh));
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly DependencyProperty BorderNormalColorProperty =
            DependencyProperty.Register(
                nameof(BorderNormalColor),
                typeof(Color),
                typeof(UI4ComboBox),
                new PropertyMetadata(Color.FromRgb(200, 200, 220), OnStyleRefresh));
        public Color BorderNormalColor
        {
            get => (Color)GetValue(BorderNormalColorProperty);
            set => SetValue(BorderNormalColorProperty, value);
        }
        public static readonly DependencyProperty FocusGradientStartProperty =
            DependencyProperty.Register(
                nameof(FocusGradientStart),
                typeof(Color),
                typeof(UI4ComboBox),
                new PropertyMetadata(Color.FromRgb(37, 99, 235), OnStyleRefresh));
        public Color FocusGradientStart
        {
            get => (Color)GetValue(FocusGradientStartProperty);
            set => SetValue(FocusGradientStartProperty, value);
        }
        public static readonly DependencyProperty FocusGradientEndProperty =
            DependencyProperty.Register(
                nameof(FocusGradientEnd),
                typeof(Color),
                typeof(UI4ComboBox),
                new PropertyMetadata(Color.FromRgb(147, 51, 234), OnStyleRefresh));
        public Color FocusGradientEnd
        {
            get => (Color)GetValue(FocusGradientEndProperty);
            set => SetValue(FocusGradientEndProperty, value);
        }
        public static readonly DependencyProperty EditBackgroundProperty =
            DependencyProperty.Register(
                nameof(EditBackground),
                typeof(Brush),
                typeof(UI4ComboBox),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 255, 255)), OnStyleRefresh));
        public Brush EditBackground
        {
            get => (Brush)GetValue(EditBackgroundProperty);
            set => SetValue(EditBackgroundProperty, value);
        }
        public static readonly DependencyProperty TextColorProperty =
            DependencyProperty.Register(
                nameof(TextColor),
                typeof(Color),
                typeof(UI4ComboBox),
                new PropertyMetadata(Color.FromRgb(30, 30, 30), OnStyleRefresh));
        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }
        public static readonly DependencyProperty InnerPaddingProperty =
            DependencyProperty.Register(
                nameof(InnerPadding),
                typeof(Thickness),
                typeof(UI4ComboBox),
                new PropertyMetadata(new Thickness(12, 10, 30, 10), OnStyleRefresh));
        public Thickness InnerPadding
        {
            get => (Thickness)GetValue(InnerPaddingProperty);
            set => SetValue(InnerPaddingProperty, value);
        }
        public static readonly DependencyProperty DropCornerRadiusProperty =
            DependencyProperty.Register(
                nameof(DropCornerRadius),
                typeof(CornerRadius),
                typeof(UI4ComboBox),
                new PropertyMetadata(new CornerRadius(6), OnStyleRefresh));
        public CornerRadius DropCornerRadius
        {
            get => (CornerRadius)GetValue(DropCornerRadiusProperty);
            set => SetValue(DropCornerRadiusProperty, value);
        }
        private static string _scrollBarResourcesXaml;
        private static bool _globalScrollResLoaded = false;
        private static void OnStyleRefresh(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UI4ComboBox cbx)
                cbx.Style = cbx.BuildComboStyle();
        }
        static UI4ComboBox()
        {
            _scrollBarResourcesXaml = GetScrollBarResourcesXaml();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UI4ComboBox),
                new FrameworkPropertyMetadata(typeof(UI4ComboBox)));
            LoadGlobalScrollResource();
        }
        private static void LoadGlobalScrollResource()
        {
            if (_globalScrollResLoaded) return;
            var appRes = Application.Current.Resources;
            var dict = (ResourceDictionary)XamlReader.Parse(_scrollBarResourcesXaml);
            foreach (DictionaryEntry entry in dict)
            {
                if (!appRes.Contains(entry.Key))
                    appRes.Add(entry.Key, entry.Value);
            }
            _globalScrollResLoaded = true;
        }
        private static void OnDropDownPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is ScrollViewer scrollViewer)
                ShowDropDownScrollBars(scrollViewer);
        }
        private static void OnDropDownScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (sender is not ScrollViewer scrollViewer || (e.VerticalChange == 0 && e.HorizontalChange == 0))
                return;

            ShowDropDownScrollBars(scrollViewer);
        }
        private static void ShowDropDownScrollBars(ScrollViewer scrollViewer)
        {
            foreach (ScrollBar scrollBar in FindVisualChildren<ScrollBar>(scrollViewer))
            {
                scrollBar.BeginAnimation(OpacityProperty, null);
                scrollBar.Opacity = 0.6;
            }

            if (scrollViewer.Tag is not DispatcherTimer timer)
            {
                timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(600) };
                timer.Tick += (_, _) =>
                {
                    timer.Stop();
                    foreach (ScrollBar scrollBar in FindVisualChildren<ScrollBar>(scrollViewer))
                    {
                        scrollBar.BeginAnimation(OpacityProperty, new DoubleAnimation(0, TimeSpan.FromMilliseconds(300)));
                    }
                };
                scrollViewer.Tag = timer;
            }

            timer.Stop();
            timer.Start();
        }
        private static IEnumerable FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null)
                yield break;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T matchedChild)
                    yield return matchedChild;

                foreach (T descendant in FindVisualChildren<T>(child))
                    yield return descendant;
            }
        }
        private static string GetScrollBarResourcesXaml()
        {
            return @"
<ResourceDictionary xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'
                    xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>
    <Style x:Key='ScrollBarThumb' TargetType='{x:Type Thumb}'>
        <Setter Property='OverridesDefaultStyle' Value='true' />
        <Setter Property='IsTabStop' Value='false' />
        <Setter Property='Template'>
            <Setter.Value>
                <ControlTemplate TargetType='{x:Type Thumb}'>
                    <Rectangle Fill='#90000000' RadiusX='3' RadiusY='3' />
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
    <Style x:Key='HorizontalScrollBarPageButton' TargetType='{x:Type RepeatButton}'>
        <Setter Property='OverridesDefaultStyle' Value='true' />
        <Setter Property='Background' Value='Transparent' />
        <Setter Property='Focusable' Value='false' />
        <Setter Property='IsTabStop' Value='false' />
        <Setter Property='Opacity' Value='0' />
        <Setter Property='Template'>
            <Setter.Value>
                <ControlTemplate TargetType='{x:Type RepeatButton}'>
                    <Rectangle Fill='{TemplateBinding Background}'
                               Width='{TemplateBinding Width}'
                               Height='{TemplateBinding Height}' />
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
    <Style x:Key='VerticalScrollBarPageButton' TargetType='{x:Type RepeatButton}'>
        <Setter Property='OverridesDefaultStyle' Value='true' />
        <Setter Property='Background' Value='Transparent' />
        <Setter Property='Focusable' Value='false' />
        <Setter Property='IsTabStop' Value='false' />
        <Setter Property='Opacity' Value='0' />
        <Setter Property='Template'>
            <Setter.Value>
                <ControlTemplate TargetType='{x:Type RepeatButton}'>
                    <Rectangle Fill='{TemplateBinding Background}'
                               Width='{TemplateBinding Width}'
                               Height='{TemplateBinding Height}' />
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
    <Style TargetType='{x:Type ScrollBar}'>
        <Setter Property='Stylus.IsPressAndHoldEnabled' Value='false' />
        <Setter Property='Stylus.IsFlicksEnabled' Value='false' />
        <Setter Property='Background' Value='Transparent' />
        <Setter Property='Margin' Value='0,1,2,6' />
        <Setter Property='Width' Value='6' />
        <Setter Property='MinWidth' Value='6' />
        <Setter Property='Opacity' Value='0' />
        <Setter Property='Template'>
            <Setter.Value>
                <ControlTemplate TargetType='{x:Type ScrollBar}'>
                    <Grid x:Name='Bg' SnapsToDevicePixels='true'>
                        <Track x:Name='PART_Track' IsDirectionReversed='true'>
                            <Track.DecreaseRepeatButton>
                                <RepeatButton Style='{StaticResource VerticalScrollBarPageButton}'
                                              Command='{x:Static ScrollBar.PageUpCommand}' />
                            </Track.DecreaseRepeatButton>
                            <Track.IncreaseRepeatButton>
                                <RepeatButton Style='{StaticResource VerticalScrollBarPageButton}'
                                              Command='{x:Static ScrollBar.PageDownCommand}' />
                            </Track.IncreaseRepeatButton>
                            <Track.Thumb>
                                <Thumb Style='{StaticResource ScrollBarThumb}' />
                            </Track.Thumb>
                        </Track>
                    </Grid>
                    <ControlTemplate.Triggers>
                        <Trigger Property='IsMouseOver' Value='True'>
                            <Setter Property='Opacity' Value='0.6' />
                        </Trigger>
                    </ControlTemplate.Triggers>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
        <Style.Triggers>
            <Trigger Property='Orientation' Value='Horizontal'>
                <Setter Property='Background' Value='Transparent' />
                <Setter Property='Margin' Value='2,0,6,2' />
                <Setter Property='Height' Value='6' />
                <Setter Property='MinHeight' Value='6' />
                <Setter Property='Width' Value='Auto' />
                <Setter Property='Opacity' Value='0' />
                <Setter Property='Template'>
                    <Setter.Value>
                        <ControlTemplate TargetType='{x:Type ScrollBar}'>
                            <Grid x:Name='Bg' SnapsToDevicePixels='true'>
                                <Track x:Name='PART_Track'>
                                    <Track.DecreaseRepeatButton>
                                        <RepeatButton Style='{StaticResource HorizontalScrollBarPageButton}'
                                                      Command='{x:Static ScrollBar.PageLeftCommand}' />
                                    </Track.DecreaseRepeatButton>
                                    <Track.IncreaseRepeatButton>
                                        <RepeatButton Style='{StaticResource HorizontalScrollBarPageButton}'
                                                      Command='{x:Static ScrollBar.PageRightCommand}' />
                                    </Track.IncreaseRepeatButton>
                                    <Track.Thumb>
                                        <Thumb Style='{StaticResource ScrollBarThumb}' />
                                    </Track.Thumb>
                                </Track>
                            </Grid>
                            <ControlTemplate.Triggers>
                                <Trigger Property='IsMouseOver' Value='True'>
                                    <Setter Property='Opacity' Value='0.6' />
                                </Trigger>
                            </ControlTemplate.Triggers>
                        </ControlTemplate>
                    </Setter.Value>
                </Setter>
            </Trigger>
        </Style.Triggers>
    </Style>
</ResourceDictionary>";
        }
        public UI4ComboBox()
        {
            FontSize = 15d;
            Style = BuildComboStyle();
        }
        private Style BuildComboStyle()
        {
            Style style = new Style(typeof(ComboBox));
            style.Setters.Add(new Setter(ForegroundProperty, new SolidColorBrush(TextColor)));
            style.Setters.Add(new Setter(PaddingProperty, InnerPadding));
            style.Setters.Add(new Setter(BackgroundProperty, EditBackground));
            style.Setters.Add(new Setter(BorderThicknessProperty, new Thickness(1, 1, 1, 1)));
            style.Setters.Add(new Setter(BorderBrushProperty, new SolidColorBrush(BorderNormalColor)));
            style.Setters.Add(new Setter(ComboBox.MinWidthProperty, new Binding(nameof(ActualWidth)) { RelativeSource = RelativeSource.Self }));
            style.Setters.Add(new Setter(CursorProperty, Cursors.Hand));
            Style itemStyle = new Style(typeof(ComboBoxItem));
            ControlTemplate itemTemplate = new ControlTemplate(typeof(ComboBoxItem));
            FrameworkElementFactory itemBorder = new FrameworkElementFactory(typeof(Border));
            itemBorder.SetBinding(Border.BackgroundProperty, new Binding(nameof(Background)) { RelativeSource = RelativeSource.TemplatedParent });
            itemBorder.SetBinding(Border.PaddingProperty, new Binding(nameof(Padding)) { RelativeSource = RelativeSource.TemplatedParent });
            FrameworkElementFactory contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenter.SetBinding(ContentPresenter.VerticalAlignmentProperty, new Binding(nameof(VerticalContentAlignment)) { RelativeSource = RelativeSource.TemplatedParent });
            contentPresenter.SetBinding(ContentPresenter.HorizontalAlignmentProperty, new Binding(nameof(HorizontalContentAlignment)) { RelativeSource = RelativeSource.TemplatedParent });
            itemBorder.AppendChild(contentPresenter);
            itemTemplate.VisualTree = itemBorder;
            itemStyle.Setters.Add(new Setter(Control.TemplateProperty, itemTemplate));
            Trigger itemHoverTrigger = new Trigger { Property = UIElement.IsMouseOverProperty, Value = true };
            itemHoverTrigger.Setters.Add(new Setter(Control.BackgroundProperty, new SolidColorBrush(Color.FromArgb(20, 0, 0, 0))));
            itemHoverTrigger.Setters.Add(new Setter(CursorProperty, Cursors.Hand));
            itemStyle.Triggers.Add(itemHoverTrigger);
            Trigger itemSelectedTrigger = new Trigger { Property = ListBoxItem.IsSelectedProperty, Value = true };
            itemSelectedTrigger.Setters.Add(new Setter(Control.BackgroundProperty, new SolidColorBrush(Color.FromArgb(10, 0, 0, 0))));
            itemSelectedTrigger.Setters.Add(new Setter(CursorProperty, Cursors.Hand));
            itemStyle.Triggers.Add(itemSelectedTrigger);
            style.Setters.Add(new Setter(ComboBox.ItemContainerStyleProperty, itemStyle));
            itemStyle.Setters.Add(new Setter(Control.HeightProperty, 36d));
            itemStyle.Setters.Add(new Setter(Control.PaddingProperty, new Thickness(10, 0, 0, 0)));
            itemStyle.Setters.Add(new Setter(Control.VerticalContentAlignmentProperty, VerticalAlignment.Center));
            itemStyle.Setters.Add(new Setter(Control.HorizontalContentAlignmentProperty, HorizontalAlignment.Left));
            itemStyle.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.Transparent));
            Trigger hoverTrigger = new Trigger { Property = UIElement.IsMouseOverProperty, Value = true };
            hoverTrigger.Setters.Add(new Setter(Control.BackgroundProperty, new SolidColorBrush(Color.FromArgb(20, 0, 0, 0))));
            hoverTrigger.Setters.Add(new Setter(CursorProperty, Cursors.Hand));
            itemStyle.Triggers.Add(hoverTrigger);
            style.Setters.Add(new Setter(ComboBox.ItemContainerStyleProperty, itemStyle));
            ControlTemplate template = new ControlTemplate(typeof(ComboBox));
            FrameworkElementFactory rootGrid = new FrameworkElementFactory(typeof(Grid));
            FrameworkElementFactory borderRoot = new FrameworkElementFactory(typeof(Border));
            borderRoot.Name = "PART_Border";
            borderRoot.SetBinding(Border.CornerRadiusProperty, new Binding(nameof(CornerRadius)) { RelativeSource = RelativeSource.TemplatedParent });
            borderRoot.SetBinding(Border.BackgroundProperty, new Binding(nameof(Background)) { RelativeSource = RelativeSource.TemplatedParent });
            borderRoot.SetBinding(Border.BorderBrushProperty, new Binding(nameof(BorderBrush)) { RelativeSource = RelativeSource.TemplatedParent });
            borderRoot.SetBinding(Border.BorderThicknessProperty, new Binding(nameof(BorderThickness)) { RelativeSource = RelativeSource.TemplatedParent });
            FrameworkElementFactory mainGrid = new FrameworkElementFactory(typeof(Grid));
            FrameworkElementFactory colDef1 = new FrameworkElementFactory(typeof(ColumnDefinition));
            FrameworkElementFactory colDef2 = new FrameworkElementFactory(typeof(ColumnDefinition));
            colDef2.SetValue(ColumnDefinition.WidthProperty, new GridLength(36d));
            mainGrid.AppendChild(colDef1);
            mainGrid.AppendChild(colDef2);
            Style toggleBtnStyle = new Style(typeof(ToggleButton));
            toggleBtnStyle.Setters.Add(new Setter(ToggleButton.BackgroundProperty, Brushes.Transparent));
            toggleBtnStyle.Setters.Add(new Setter(ToggleButton.BorderThicknessProperty, new Thickness(0)));
            toggleBtnStyle.Setters.Add(new Setter(ToggleButton.CursorProperty, Cursors.Hand));
            toggleBtnStyle.Setters.Add(new Setter(ToggleButton.ForegroundProperty, new SolidColorBrush(Color.FromRgb(120, 120, 140))));
            ControlTemplate toggleBtnTemplate = new ControlTemplate(typeof(ToggleButton));
            FrameworkElementFactory toggleBtnBorder = new FrameworkElementFactory(typeof(Border));
            toggleBtnBorder.SetValue(Border.BackgroundProperty, Brushes.Transparent);
            toggleBtnBorder.SetBinding(Border.BackgroundProperty, new Binding(nameof(Background)) { RelativeSource = RelativeSource.TemplatedParent });
            FrameworkElementFactory arrowPath = new FrameworkElementFactory(typeof(Path));
            arrowPath.SetValue(Path.DataProperty, Geometry.Parse("M 0 0 L 6 6 L 12 0"));
            arrowPath.SetValue(Path.StrokeProperty, new SolidColorBrush(Color.FromRgb(120, 120, 140)));
            arrowPath.SetValue(Path.StrokeThicknessProperty, 1.5);
            arrowPath.SetValue(Path.FillProperty, Brushes.Transparent);
            arrowPath.SetValue(Path.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            arrowPath.SetValue(Path.VerticalAlignmentProperty, VerticalAlignment.Center);
            arrowPath.SetValue(Path.RenderTransformProperty, new RotateTransform(0, 6, 3));
            arrowPath.Name = "ArrowPath";
            toggleBtnBorder.AppendChild(arrowPath);
            toggleBtnTemplate.VisualTree = toggleBtnBorder;
            Trigger toggleBtnHoverTrigger = new Trigger
            {
                Property = UIElement.IsMouseOverProperty,
                Value = true
            };
            toggleBtnHoverTrigger.Setters.Add(new Setter(Path.StrokeProperty, new SolidColorBrush(Color.FromRgb(60, 60, 80))) { TargetName = "ArrowPath" });
            toggleBtnTemplate.Triggers.Add(toggleBtnHoverTrigger);
            Trigger toggleBtnCheckedTrigger = new Trigger
            {
                Property = ToggleButton.IsCheckedProperty,
                Value = true
            };
            toggleBtnCheckedTrigger.Setters.Add(new Setter(Path.RenderTransformProperty, new RotateTransform(180, 6, 3)) { TargetName = "ArrowPath" });
            toggleBtnTemplate.Triggers.Add(toggleBtnCheckedTrigger);
            toggleBtnStyle.Setters.Add(new Setter(ToggleButton.TemplateProperty, toggleBtnTemplate));
            FrameworkElementFactory toggleBtn = new FrameworkElementFactory(typeof(ToggleButton));
            toggleBtn.SetValue(Grid.ColumnProperty, 1);
            toggleBtn.SetValue(ToggleButton.StyleProperty, toggleBtnStyle);
            toggleBtn.SetBinding(ToggleButton.IsCheckedProperty, new Binding(nameof(IsDropDownOpen)) { RelativeSource = RelativeSource.TemplatedParent, Mode = BindingMode.TwoWay });
            mainGrid.AppendChild(toggleBtn);
            FrameworkElementFactory contentHostGrid = new FrameworkElementFactory(typeof(Grid));
            contentHostGrid.SetValue(Grid.ColumnProperty, 0);
            contentHostGrid.SetValue(Panel.BackgroundProperty, Brushes.Transparent);
            FrameworkElementFactory contentPresenter1 = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenter1.Name = "PART_ContentPresenter";
            contentPresenter1.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            contentPresenter1.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
            contentPresenter1.SetBinding(ContentPresenter.MarginProperty, new Binding(nameof(Padding)) { RelativeSource = RelativeSource.TemplatedParent });
            contentPresenter1.SetBinding(ContentPresenter.ContentProperty, new Binding(nameof(SelectionBoxItem)) { RelativeSource = RelativeSource.TemplatedParent });
            contentPresenter1.SetBinding(ContentPresenter.ContentTemplateProperty, new Binding(nameof(SelectionBoxItemTemplate)) { RelativeSource = RelativeSource.TemplatedParent });
            contentHostGrid.AppendChild(contentPresenter1);
            Style editableTextBoxStyle = new Style(typeof(TextBox));
            editableTextBoxStyle.Setters.Add(new Setter(TextBox.BorderThicknessProperty, new Thickness(0)));
            editableTextBoxStyle.Setters.Add(new Setter(TextBox.BackgroundProperty, Brushes.Transparent));
            editableTextBoxStyle.Setters.Add(new Setter(TextBox.PaddingProperty, new Thickness(0)));
            editableTextBoxStyle.Setters.Add(new Setter(TextBox.FocusVisualStyleProperty, null));
            ControlTemplate editBoxTemplate = new ControlTemplate(typeof(TextBox));
            FrameworkElementFactory editBoxBorder = new FrameworkElementFactory(typeof(Border));
            editBoxBorder.SetValue(Border.BorderThicknessProperty, new Thickness(0));
            editBoxBorder.SetValue(Border.BackgroundProperty, Brushes.Transparent);
            FrameworkElementFactory editBoxScrollViewer = new FrameworkElementFactory(typeof(ScrollViewer));
            editBoxScrollViewer.Name = "PART_ContentHost";
            editBoxScrollViewer.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Hidden);
            editBoxScrollViewer.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Hidden);
            editBoxBorder.AppendChild(editBoxScrollViewer);
            editBoxTemplate.VisualTree = editBoxBorder;
            editableTextBoxStyle.Setters.Add(new Setter(TextBox.TemplateProperty, editBoxTemplate));
            FrameworkElementFactory editableTextBox = new FrameworkElementFactory(typeof(TextBox));
            editableTextBox.Name = "PART_EditableTextBox";
            editableTextBox.SetValue(TextBox.VisibilityProperty, Visibility.Collapsed);
            editableTextBox.SetValue(TextBox.StyleProperty, editableTextBoxStyle);
            editableTextBox.SetValue(TextBox.MarginProperty, new Thickness(6, 0, 6, 0));
            editableTextBox.SetBinding(TextBox.TextProperty, new Binding(nameof(Text)) { RelativeSource = RelativeSource.TemplatedParent, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            editableTextBox.SetBinding(TextBox.ForegroundProperty, new Binding(nameof(Foreground)) { RelativeSource = RelativeSource.TemplatedParent });
            editableTextBox.SetValue(TextBox.CursorProperty, Cursors.IBeam);
            editableTextBox.SetValue(TextBox.VerticalContentAlignmentProperty, VerticalAlignment.Center);
            editableTextBox.SetValue(TextBox.VerticalAlignmentProperty, VerticalAlignment.Center);
            contentHostGrid.AppendChild(editableTextBox);
            contentHostGrid.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler((s, e) =>
            {
                if (this is UI4ComboBox cbx && !cbx.IsDropDownOpen)
                {
                    cbx.IsDropDownOpen = true;
                    e.Handled = true;
                }
            }), handledEventsToo: true);
            mainGrid.AppendChild(contentHostGrid);
            borderRoot.AppendChild(mainGrid);
            rootGrid.AppendChild(borderRoot);
            FrameworkElementFactory dropPopup = new FrameworkElementFactory(typeof(Popup));
            dropPopup.Name = "PART_Popup";
            dropPopup.SetValue(Popup.PlacementProperty, PlacementMode.Bottom);
            dropPopup.SetValue(Popup.AllowsTransparencyProperty, true);
            dropPopup.SetValue(Popup.PopupAnimationProperty, PopupAnimation.Fade);
            dropPopup.SetBinding(Popup.IsOpenProperty, new Binding(nameof(IsDropDownOpen)) { RelativeSource = RelativeSource.TemplatedParent });
            dropPopup.SetBinding(FrameworkElement.WidthProperty, new Binding(nameof(ActualWidth)) { RelativeSource = RelativeSource.TemplatedParent });
            FrameworkElementFactory dropBorder = new FrameworkElementFactory(typeof(Border));
            dropBorder.SetBinding(Border.CornerRadiusProperty, new Binding(nameof(DropCornerRadius)) { RelativeSource = RelativeSource.TemplatedParent });
            dropBorder.SetValue(Border.BackgroundProperty, Brushes.White);
            dropBorder.SetValue(Border.BorderThicknessProperty, new Thickness(1, 1, 1, 1));
            dropBorder.SetValue(Border.BorderBrushProperty, new SolidColorBrush(Color.FromRgb(200, 200, 220)));
            FrameworkElementFactory dropScroll = new FrameworkElementFactory(typeof(ScrollViewer));
            dropScroll.SetBinding(FrameworkElement.MaxHeightProperty, new Binding(nameof(MaxDropDownHeight)) { RelativeSource = RelativeSource.TemplatedParent });
            dropScroll.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Auto);
            dropScroll.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Hidden);
            dropScroll.AddHandler(UIElement.PreviewMouseWheelEvent, new MouseWheelEventHandler(OnDropDownPreviewMouseWheel), true);
            dropScroll.AddHandler(ScrollViewer.ScrollChangedEvent, new ScrollChangedEventHandler(OnDropDownScrollChanged));
            FrameworkElementFactory itemsPresenter = new FrameworkElementFactory(typeof(ItemsPresenter));
            dropScroll.AppendChild(itemsPresenter);
            dropBorder.AppendChild(dropScroll);
            dropPopup.AppendChild(dropBorder);
            rootGrid.AppendChild(dropPopup);
            template.VisualTree = rootGrid;
            Trigger editableTrigger = new Trigger
            {
                Property = ComboBox.IsEditableProperty,
                Value = true
            };
            editableTrigger.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Collapsed) { TargetName = "PART_ContentPresenter" });
            editableTrigger.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Visible) { TargetName = "PART_EditableTextBox" });
            template.Triggers.Add(editableTrigger);
            Trigger focusTrigger = new Trigger
            {
                Property = UIElement.IsKeyboardFocusWithinProperty,
                Value = true
            };
            LinearGradientBrush focusGrad = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 1),
                GradientStops =
                {
                    new GradientStop(FocusGradientStart, 0),
                    new GradientStop(FocusGradientEnd, 1)
                }
            };
            focusTrigger.Setters.Add(new Setter(Border.BorderBrushProperty, focusGrad) { TargetName = "PART_Border" });
            focusTrigger.Setters.Add(new Setter(Border.BorderThicknessProperty, new Thickness(1.2, 1.2, 1.2, 1.2)) { TargetName = "PART_Border" });
            template.Triggers.Add(focusTrigger);
            Trigger hoverTrigger1 = new Trigger
            {
                Property = UIElement.IsMouseOverProperty,
                Value = true
            };
            hoverTrigger1.Setters.Add(new Setter(Border.BorderBrushProperty, new SolidColorBrush(Color.FromRgb(160, 160, 190))) { TargetName = "PART_Border" });
            template.Triggers.Add(hoverTrigger1);
            style.Setters.Add(new Setter(Control.TemplateProperty, template));
            return style;
        }
    }
}