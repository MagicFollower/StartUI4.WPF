using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Input;

namespace StartUI4Controls
{
    /// <summary>
    /// 上下文菜单项类型枚举。
    /// </summary>
    public enum UI4MenuItemType
    {
        Undo,
        Redo,
        Cut,
        Copy,
        Paste,
        Delete,
        SelectAll
    }

    /// <summary>
    /// 上下文菜单项数据类。
    /// </summary>
    public class UI4MenuItem
    {
        public UI4MenuItemType Type { get; set; }
        public string Text { get; set; }
        public ImageSource Icon { get; set; }
        public Action Command { get; set; }
        public Func<bool> CanExecute { get; set; }

        public UI4MenuItem(UI4MenuItemType type, string text, ImageSource icon, Action command, Func<bool> canExecute = null)
        {
            Type = type;
            Text = text;
            Icon = icon;
            Command = command;
            CanExecute = canExecute;
        }
    }

    /// <summary>
    /// 提供几何图形辅助方法的静态类。
    /// </summary>
    public static class GeometryHelper
    {
        public static Geometry GetOutlinedGeometry(this RectangleGeometry rectGeo)
        {
            Rect rect = rectGeo.Rect;
            PathGeometry path = new PathGeometry();
            PathFigure figure = new PathFigure
            {
                StartPoint = new Point(rect.Left, rect.Top),
                IsClosed = true,
                IsFilled = false
            };
            figure.Segments.Add(new LineSegment(new Point(rect.Right, rect.Top), true));
            figure.Segments.Add(new LineSegment(new Point(rect.Right, rect.Bottom), true));
            figure.Segments.Add(new LineSegment(new Point(rect.Left, rect.Bottom), true));
            path.Figures.Add(figure);
            path.Freeze();
            return path;
        }
    }

    /// <summary>
    /// 提供菜单项图标的静态类。
    /// </summary>
    public static class UI4MenuIcons
    {
        private static ImageSource _undoIcon;
        private static ImageSource _redoIcon;
        private static ImageSource _cutIcon;
        private static ImageSource _copyIcon;
        private static ImageSource _pasteIcon;
        private static ImageSource _deleteIcon;
        private static ImageSource _selectAllIcon;

        public static ImageSource Undo => _undoIcon ?? (_undoIcon = CreateUndoIcon());
        public static ImageSource Redo => _redoIcon ?? (_redoIcon = CreateRedoIcon());
        public static ImageSource Cut => _cutIcon ?? (_cutIcon = CreateCutIcon());
        public static ImageSource Copy => _copyIcon ?? (_copyIcon = CreateCopyIcon());
        public static ImageSource Paste => _pasteIcon ?? (_pasteIcon = CreatePasteIcon());
        public static ImageSource Delete => _deleteIcon ?? (_deleteIcon = CreateDeleteIcon());
        public static ImageSource SelectAll => _selectAllIcon ?? (_selectAllIcon = CreateSelectAllIcon());

        public static ImageSource GetIcon(UI4MenuItemType type)
        {
            switch (type)
            {
                case UI4MenuItemType.Undo: return Undo;
                case UI4MenuItemType.Redo: return Redo;
                case UI4MenuItemType.Cut: return Cut;
                case UI4MenuItemType.Copy: return Copy;
                case UI4MenuItemType.Paste: return Paste;
                case UI4MenuItemType.Delete: return Delete;
                case UI4MenuItemType.SelectAll: return SelectAll;
                default: return null;
            }
        }

        private static ImageSource CreateIconFromGeometry(Geometry geometry, Brush strokeBrush)
        {
            Pen pen = new Pen(strokeBrush, 1.2)
            {
                LineJoin = PenLineJoin.Round,
                StartLineCap = PenLineCap.Round,
                EndLineCap = PenLineCap.Round
            };
            GeometryDrawing drawing = new GeometryDrawing(null, pen, geometry);
            DrawingGroup drawingGroup = new DrawingGroup();
            drawingGroup.Children.Add(drawing);
            DrawingImage image = new DrawingImage(drawingGroup);
            image.Freeze();
            pen.Freeze();
            return image;
        }

        private static ImageSource CreateUndoIcon()
        {
            StreamGeometry geo = new StreamGeometry();
            using (var ctx = geo.Open())
            {
                ctx.BeginFigure(new Point(4, 10), true, true);
                ctx.LineTo(new Point(9, 10), true, false);
                ctx.ArcTo(new Point(14, 15), new Size(5, 5), 0, false, SweepDirection.Clockwise, true, false);
                ctx.LineTo(new Point(16, 15), true, false);
                ctx.LineTo(new Point(13, 18), true, false);
                ctx.LineTo(new Point(10, 15), true, false);
                ctx.LineTo(new Point(11, 15), true, false);
                ctx.ArcTo(new Point(7, 11), new Size(4, 4), 0, false, SweepDirection.Counterclockwise, true, false);
                ctx.LineTo(new Point(7, 10), true, false);
            }
            geo.Freeze();
            return CreateIconFromGeometry(geo, Brushes.Black);
        }

        private static ImageSource CreateRedoIcon()
        {
            StreamGeometry geo = new StreamGeometry();
            using (var ctx = geo.Open())
            {
                ctx.BeginFigure(new Point(16, 10), true, true);
                ctx.LineTo(new Point(11, 10), true, false);
                ctx.ArcTo(new Point(6, 15), new Size(5, 5), 0, false, SweepDirection.Counterclockwise, true, false);
                ctx.LineTo(new Point(4, 15), true, false);
                ctx.LineTo(new Point(7, 18), true, false);
                ctx.LineTo(new Point(10, 15), true, false);
                ctx.LineTo(new Point(9, 15), true, false);
                ctx.ArcTo(new Point(13, 11), new Size(4, 4), 0, false, SweepDirection.Clockwise, true, false);
                ctx.LineTo(new Point(13, 10), true, false);
            }
            geo.Freeze();
            return CreateIconFromGeometry(geo, Brushes.Black);
        }

        private static ImageSource CreateCutIcon()
        {
            StreamGeometry geo = new StreamGeometry();
            using (var ctx = geo.Open())
            {
                ctx.BeginFigure(new Point(3, 3), false, false);
                ctx.LineTo(new Point(15, 15), true, false);
                ctx.BeginFigure(new Point(15, 3), false, false);
                ctx.LineTo(new Point(3, 15), true, false);
                ctx.BeginFigure(new Point(5, 2), true, true);
                ctx.ArcTo(new Point(7, 4), new Size(2, 2), 0, false, SweepDirection.Clockwise, true, false);
                ctx.ArcTo(new Point(5, 6), new Size(2, 2), 0, false, SweepDirection.Clockwise, true, false);
                ctx.ArcTo(new Point(3, 4), new Size(2, 2), 0, false, SweepDirection.Clockwise, true, false);
                ctx.ArcTo(new Point(5, 2), new Size(2, 2), 0, false, SweepDirection.Clockwise, true, false);
                ctx.BeginFigure(new Point(15, 2), true, true);
                ctx.ArcTo(new Point(17, 4), new Size(2, 2), 0, false, SweepDirection.Clockwise, true, false);
                ctx.ArcTo(new Point(15, 6), new Size(2, 2), 0, false, SweepDirection.Clockwise, true, false);
                ctx.ArcTo(new Point(13, 4), new Size(2, 2), 0, false, SweepDirection.Clockwise, true, false);
                ctx.ArcTo(new Point(15, 2), new Size(2, 2), 0, false, SweepDirection.Clockwise, true, false);
            }
            geo.Freeze();
            return CreateIconFromGeometry(geo, Brushes.Black);
        }

        private static ImageSource CreateCopyIcon()
        {
            GeometryGroup group = new GeometryGroup();
            group.Children.Add(new RectangleGeometry(new Rect(6, 2, 10, 12), 1, 1).GetOutlinedGeometry());
            group.Children.Add(new RectangleGeometry(new Rect(2, 6, 10, 12), 1, 1).GetOutlinedGeometry());
            group.Freeze();
            return CreateIconFromGeometry(group, Brushes.Black);
        }

        private static ImageSource CreatePasteIcon()
        {
            GeometryGroup group = new GeometryGroup();
            group.Children.Add(new RectangleGeometry(new Rect(7, 1, 6, 3), 1, 1).GetOutlinedGeometry());
            group.Children.Add(new RectangleGeometry(new Rect(4, 4, 12, 14), 1, 1).GetOutlinedGeometry());
            group.Freeze();
            return CreateIconFromGeometry(group, Brushes.Black);
        }

        private static ImageSource CreateDeleteIcon()
        {
            GeometryGroup group = new GeometryGroup();
            group.Children.Add(new RectangleGeometry(new Rect(3, 4, 14, 2), 0.5, 0.5).GetOutlinedGeometry());
            group.Children.Add(new RectangleGeometry(new Rect(5, 6, 10, 12), 0, 0).GetOutlinedGeometry());
            group.Children.Add(new LineGeometry(new Point(8, 9), new Point(8, 15)));
            group.Children.Add(new LineGeometry(new Point(12, 9), new Point(12, 15)));
            group.Freeze();
            return CreateIconFromGeometry(group, Brushes.Black);
        }

        private static ImageSource CreateSelectAllIcon()
        {
            GeometryGroup group = new GeometryGroup();
            group.Children.Add(new RectangleGeometry(new Rect(2, 2, 7, 7), 1, 1).GetOutlinedGeometry());
            group.Children.Add(new RectangleGeometry(new Rect(11, 2, 7, 7), 1, 1).GetOutlinedGeometry());
            group.Children.Add(new RectangleGeometry(new Rect(2, 11, 7, 7), 1, 1).GetOutlinedGeometry());
            group.Children.Add(new RectangleGeometry(new Rect(11, 11, 7, 7), 1, 1).GetOutlinedGeometry());
            group.Freeze();
            return CreateIconFromGeometry(group, Brushes.Black);
        }
    }

    /// <summary>
    /// 现代风格的上下文菜单，支持多语言、图标和键盘快捷键。
    /// </summary>
    /// <remarks>
    /// <para>通过 <see cref="AddItem(UI4MenuItemType, Action, Func&lt;bool&gt;)"/> 添加菜单项，
    /// 使用 <see cref="Attach(UIElement)"/> 将菜单绑定到目标元素。</para>
    /// </remarks>
    public class UI4ContextMenu
    {
        private Popup _popup;
        private UI4ListBox _listBox;
        private List<UI4MenuItem> _menuItems;
        private UIElement _placementTarget;

        public double Width { get; set; } = 180;
        public Thickness ItemPadding { get; set; } = new Thickness(12, 8, 12, 8);

        /// <summary>边框色；显式赋值后主题切换不再覆盖。</summary>
        public Color BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; MarkCustomized(nameof(BorderColor)); ApplyColorsToListBox(); }
        }
        private Color _borderColor;

        /// <summary>菜单背景；显式赋值后主题切换不再覆盖。</summary>
        public Brush Background
        {
            get { return _background; }
            set { _background = value; MarkCustomized(nameof(Background)); ApplyColorsToListBox(); }
        }
        private Brush _background;

        /// <summary>悬停背景色；显式赋值后主题切换不再覆盖。</summary>
        public Color HoverBackground
        {
            get { return _hoverBackground; }
            set { _hoverBackground = value; MarkCustomized(nameof(HoverBackground)); ApplyColorsToListBox(); }
        }
        private Color _hoverBackground;

        public bool IsOpen => _popup?.IsOpen ?? false;

        public UI4ContextMenu()
        {
            _menuItems = new List<UI4MenuItem>();
            _borderColor = UI4Theme.Current.BorderNormalColor;
            _background = UI4Theme.Current.SurfaceBrush;
            _hoverBackground = UI4Theme.Current.HoverOverlayColor;
            UI4Theme.ThemeChanged += OnGlobalThemeChanged;
        }

        private void OnGlobalThemeChanged(object sender, EventArgs e)
        {
            var customized = _customizedColors;
            if (customized == null || !customized.Contains(nameof(BorderColor)))
                _borderColor = UI4Theme.Current.BorderNormalColor;
            if (customized == null || !customized.Contains(nameof(Background)))
                _background = UI4Theme.Current.SurfaceBrush;
            if (customized == null || !customized.Contains(nameof(HoverBackground)))
                _hoverBackground = UI4Theme.Current.HoverOverlayColor;
            ApplyColorsToListBox();
        }

        private HashSet<string> _customizedColors;

        private void MarkCustomized(string propertyName)
        {
            if (_customizedColors == null)
                _customizedColors = new HashSet<string>();
            _customizedColors.Add(propertyName);
        }

        private void ApplyColorsToListBox()
        {
            if (_listBox == null) return;
            _listBox.BorderNormalColor = BorderColor;
            // UI4ListBox 样式的表面背景取自 PanelBackground
            _listBox.PanelBackground = Background;
            _listBox.HoverBackground = HoverBackground;
        }

        /// <summary>弹出前补齐失联期间错过的主题（未弹出的 Popup 子元素不触发 Loaded）。</summary>
        private void SyncBeforeOpen()
        {
            if (_listBox == null) return;
            var customized = _customizedColors;
            if (customized == null || !customized.Contains(nameof(BorderColor)))
                _listBox.BorderNormalColor = _borderColor;
            if (customized == null || !customized.Contains(nameof(HoverBackground)))
                _listBox.HoverBackground = _hoverBackground;
            if (customized == null || !customized.Contains(nameof(Background)))
                _listBox.PanelBackground = _background;
            _listBox.RefreshTheme();
        }

        public void AddItem(UI4MenuItem item)
        {
            _menuItems.Add(item);
        }

        public void AddItem(UI4MenuItemType type, Action command, Func<bool> canExecute = null)
        {
            var key = TypeToKey(type);
            var label = UI4MultiLanguage.Get(key);
            var item = new UI4MenuItem(
                type,
                label,
                UI4MenuIcons.GetIcon(type),
                command,
                canExecute);
            _menuItems.Add(item);
        }

        private static UI4LanguageKey TypeToKey(UI4MenuItemType type)
        {
            switch (type)
            {
                case UI4MenuItemType.Undo: return UI4LanguageKey.Undo;
                case UI4MenuItemType.Redo: return UI4LanguageKey.Redo;
                case UI4MenuItemType.Cut: return UI4LanguageKey.Cut;
                case UI4MenuItemType.Copy: return UI4LanguageKey.Copy;
                case UI4MenuItemType.Paste: return UI4LanguageKey.Paste;
                case UI4MenuItemType.Delete: return UI4LanguageKey.Delete;
                case UI4MenuItemType.SelectAll: return UI4LanguageKey.SelectAll;
                default: return UI4LanguageKey.Copy;
            }
        }

        public void Attach(UIElement target)
        {
            _placementTarget = target;
            BuildMenu();
            target.MouseRightButtonUp += OnTargetRightButtonUp;
        }

        public void Detach()
        {
            UI4Theme.ThemeChanged -= OnGlobalThemeChanged;
            if (_placementTarget != null)
            {
                _placementTarget.MouseRightButtonUp -= OnTargetRightButtonUp;
                _placementTarget = null;
            }
            Close();
        }

        private void BuildMenu()
        {
            _listBox = new UI4ListBox
            {
                Width = this.Width,
                ItemPadding = this.ItemPadding,
                FontFamily = new FontFamily("Microsoft YaHei UI, Segoe UI, sans-serif"),
                BorderNormalColor = this.BorderColor,
                PanelBackground = this.Background,
                HoverBackground = this.HoverBackground,
            };

            foreach (var item in _menuItems)
            {
                var container = new Grid();
                container.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(20) });
                container.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                if (item.Icon != null)
                {
                    var icon = new System.Windows.Controls.Image
                    {
                        Source = item.Icon,
                        Width = 16,
                        Height = 16,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Opacity = item.CanExecute?.Invoke() ?? true ? 1.0 : 0.3
                    };
                    Grid.SetColumn(icon, 0);
                    container.Children.Add(icon);
                }

                var textBlock = new TextBlock
                {
                    Text = item.Text,
                    // Popup 逻辑树挂在 PlacementTarget 上会继承其 FontFamily（可能是图标字体），必须显式指定
                    FontFamily = new FontFamily("Microsoft YaHei UI, Segoe UI, sans-serif"),
                    FontSize = 14,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(6, 0, 0, 0),
                    Opacity = item.CanExecute?.Invoke() ?? true ? 1.0 : 0.4
                };
                Grid.SetColumn(textBlock, 1);
                container.Children.Add(textBlock);

                container.Tag = item;
                _listBox.Items.Add(container);
            }

            _listBox.SelectionChanged += OnSelectionChanged;

            _popup = new Popup
            {
                Child = _listBox,
                Placement = PlacementMode.MousePoint,
                StaysOpen = false,
                AllowsTransparency = true,
                PopupAnimation = PopupAnimation.Slide
            };
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_listBox.SelectedItem is FrameworkElement element && element.Tag is UI4MenuItem item)
            {
                if (item.CanExecute?.Invoke() ?? true)
                {
                    item.Command?.Invoke();
                }
                Close();
            }
        }

        private void OnTargetRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Open();
        }

        public void Open()
        {
            if (_popup == null || _listBox == null || _placementTarget == null) return;

            SyncBeforeOpen();
            Close();

            UpdateCanExecuteStates();

            _listBox.BeginAnimation(UIElement.OpacityProperty, null);
            _listBox.Opacity = 0;

            _popup.PlacementTarget = _placementTarget;
            _popup.IsOpen = true;

            var anim = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(150));
            _listBox.BeginAnimation(UIElement.OpacityProperty, anim);
        }

        public void Close()
        {
            if (_popup != null)
                _popup.IsOpen = false;
            if (_listBox != null)
                _listBox.SelectedIndex = -1;
        }

        private void UpdateCanExecuteStates()
        {
            if (_listBox == null) return;

            for (int i = 0; i < _listBox.Items.Count && i < _menuItems.Count; i++)
            {
                if (_listBox.Items[i] is FrameworkElement element && element.Tag is UI4MenuItem item)
                {
                    bool canExec = item.CanExecute?.Invoke() ?? true;
                    foreach (var child in (element as Grid).Children)
                    {
                        if (child is UIElement uiElement)
                        {
                            uiElement.Opacity = canExec ? 1.0 : 0.4;
                        }
                    }
                }
            }
        }
    }
}