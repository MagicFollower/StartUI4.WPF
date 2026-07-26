using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
namespace StartUI4Controls
{
    public class UI4TrayMenuItem
    {
        public UI4MenuItemType Type { get; set; }
        public string Text { get; set; }
        public ImageSource Icon { get; set; }
        public string IconText { get; set; }
        public Action Command { get; set; }
        public Func<bool> CanExecute { get; set; }
        public UI4TrayMenuItem(UI4MenuItemType type, string text, ImageSource icon, Action command, Func<bool> canExecute = null)
        {
            Type = type;
            Text = text;
            Icon = icon;
            Command = command;
            CanExecute = canExecute;
        }
        public UI4TrayMenuItem(UI4MenuItemType type, string text, string iconText, Action command, Func<bool> canExecute = null)
        {
            Type = type;
            Text = text;
            IconText = iconText;
            Command = command;
            CanExecute = canExecute;
        }
    }
    public class UI4NotifyIcon : TaskbarIcon
    {
        public double MenuWidth
        {
            get => (double)GetValue(MenuWidthProperty);
            set => SetValue(MenuWidthProperty, value);
        }
        public static readonly DependencyProperty MenuWidthProperty =
            DependencyProperty.Register(
                nameof(MenuWidth),
                typeof(double),
                typeof(UI4NotifyIcon),
                new FrameworkPropertyMetadata(160d));
        public Thickness MenuItemPadding
        {
            get => (Thickness)GetValue(MenuItemPaddingProperty);
            set => SetValue(MenuItemPaddingProperty, value);
        }
        public static readonly DependencyProperty MenuItemPaddingProperty =
            DependencyProperty.Register(
                nameof(MenuItemPadding),
                typeof(Thickness),
                typeof(UI4NotifyIcon),
                new FrameworkPropertyMetadata(new Thickness(12, 8, 12, 8)));
        public Color MenuBorderColor
        {
            get => (Color)GetValue(MenuBorderColorProperty);
            set => SetValue(MenuBorderColorProperty, value);
        }
        public static readonly DependencyProperty MenuBorderColorProperty =
            DependencyProperty.Register(
                nameof(MenuBorderColor),
                typeof(Color),
                typeof(UI4NotifyIcon),
                new FrameworkPropertyMetadata(Color.FromArgb(255, 200, 200, 220)));
        public Brush MenuBackground
        {
            get => (Brush)GetValue(MenuBackgroundProperty);
            set => SetValue(MenuBackgroundProperty, value);
        }
        public static readonly DependencyProperty MenuBackgroundProperty =
            DependencyProperty.Register(
                nameof(MenuBackground),
                typeof(Brush),
                typeof(UI4NotifyIcon),
                new FrameworkPropertyMetadata(Brushes.White));
        public Color MenuHoverBg
        {
            get => (Color)GetValue(MenuHoverBgProperty);
            set => SetValue(MenuHoverBgProperty, value);
        }
        public static readonly DependencyProperty MenuHoverBgProperty =
            DependencyProperty.Register(
                nameof(MenuHoverBg),
                typeof(Color),
                typeof(UI4NotifyIcon),
                new FrameworkPropertyMetadata(Color.FromArgb(10, 0, 0, 0)));
        public CornerRadius MenuCornerRadius
        {
            get => (CornerRadius)GetValue(MenuCornerRadiusProperty);
            set => SetValue(MenuCornerRadiusProperty, value);
        }
        public static readonly DependencyProperty MenuCornerRadiusProperty =
            DependencyProperty.Register(
                nameof(MenuCornerRadius),
                typeof(CornerRadius),
                typeof(UI4NotifyIcon),
                new FrameworkPropertyMetadata(new CornerRadius(8)));
        private Popup _trayPopup;
        private UI4ListBox _listBox;
        private readonly List<UI4TrayMenuItem> _menuItems = new List<UI4TrayMenuItem>();
        private Window _hookedWindow;
        private DispatcherTimer _closeCheckTimer;
        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;
        }
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        [DllImport("user32.dll")]
        private static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);
        public UI4NotifyIcon()
        {
            MenuActivation = PopupActivationMode.None;
            TrayRightMouseDown += OnTrayRightClick;
            BuildTrayPopup();
            _closeCheckTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            _closeCheckTimer.Tick += CheckMouseOutsidePopup;
        }
        public void AddItem(UI4TrayMenuItem item)
        {
            _menuItems.Add(item);
        }
        public void AddItem(UI4MenuItemType type, Action command, Func<bool> canExecute = null)
        {
            var langDict = UI4ContextMenuLanguage.Current;
            if (!langDict.ContainsKey(type)) return;
            var item = new UI4TrayMenuItem(
                type,
                langDict[type],
                UI4MenuIcons.GetIcon(type),
                command,
                canExecute);
            _menuItems.Add(item);
        }
        public void ClearMenuItems()
        {
            _menuItems.Clear();
            _listBox?.Items.Clear();
        }
        public void OpenMenu()
        {
            if (_menuItems.Count == 0) return;
            var mainWindow = Application.Current?.MainWindow;
            if (mainWindow != null)
            {
                IntPtr hwnd = new WindowInteropHelper(mainWindow).Handle;
                uint currentThread = (uint)Thread.CurrentThread.ManagedThreadId;
                uint foreThread = GetWindowThreadProcessId(GetForegroundWindow(), out _);
                if (currentThread != foreThread)
                {
                    AttachThreadInput(currentThread, foreThread, true);
                }
                SetForegroundWindow(hwnd);
                if (currentThread != foreThread)
                {
                    AttachThreadInput(currentThread, foreThread, false);
                }
            }
            RebuildAllRows();
            _trayPopup.PlacementTarget = mainWindow;
            _trayPopup.Placement = PlacementMode.MousePoint;
            _listBox.Opacity = 0;
            _trayPopup.IsOpen = true;
            var fadeAnim = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(150));
            _listBox.BeginAnimation(UIElement.OpacityProperty, fadeAnim);
            HookCloseEvents();
            _closeCheckTimer.Start();
        }
        public void CloseMenu()
        {
            _closeCheckTimer.Stop();
            if (_trayPopup != null)
                _trayPopup.IsOpen = false;
            if (_listBox != null)
                _listBox.SelectedIndex = -1;
            UnhookCloseEvents();
        }
        private void BuildTrayPopup()
        {
            _listBox = new UI4ListBox
            {
                Width = MenuWidth,
                ItemPadding = MenuItemPadding,
                BorderNormalColor = MenuBorderColor,
                Background = MenuBackground,
                HoverBackground = MenuHoverBg,
                CornerRadius = MenuCornerRadius,
            };
            _listBox.PreviewMouseLeftButtonUp += OnListBoxClick;
            _trayPopup = new Popup
            {
                Child = _listBox,
                Placement = PlacementMode.MousePoint,
                StaysOpen = false,
                AllowsTransparency = true,
                PopupAnimation = PopupAnimation.Slide
            };
            _trayPopup.Closed += OnTrayPopupClosed;
        }
        private void RebuildAllRows()
        {
            _listBox.Items.Clear();
            foreach (var item in _menuItems)
            {
                var container = new Grid();
                container.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(20) });
                container.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                bool enable = item.CanExecute?.Invoke() ?? true;
                double opacityDisable = enable ? 1.0 : 0.3;
                double textOpacityDisable = enable ? 1.0 : 0.4;
                if (item.Icon != null)
                {
                    var icon = new Image
                    {
                        Source = item.Icon,
                        Width = 16,
                        Height = 16,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Opacity = opacityDisable
                    };
                    Grid.SetColumn(icon, 0);
                    container.Children.Add(icon);
                }
                else if (!string.IsNullOrEmpty(item.IconText))
                {
                    var iconText = new TextBlock
                    {
                        Text = item.IconText,
                        FontSize = 12,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Opacity = opacityDisable
                    };
                    Grid.SetColumn(iconText, 0);
                    container.Children.Add(iconText);
                }
                var textBlock = new TextBlock
                {
                    Text = item.Text,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(6, 0, 0, 0),
                    Opacity = textOpacityDisable
                };
                Grid.SetColumn(textBlock, 1);
                container.Children.Add(textBlock);
                container.Tag = item;
                _listBox.Items.Add(container);
            }
            _listBox.Width = MenuWidth;
        }
        private void OnTrayRightClick(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (_trayPopup.IsOpen)
            {
                CloseMenu();
                return;
            }
            OpenMenu();
        }
        private void OnListBoxClick(object sender, MouseButtonEventArgs e)
        {
            var hit = _listBox.InputHitTest(e.GetPosition(_listBox)) as DependencyObject;
            while (hit != null && hit != _listBox)
            {
                if (hit is ListBoxItem lbi)
                {
                    if (lbi.Content is FrameworkElement fe && fe.Tag is UI4TrayMenuItem item)
                    {
                        bool canExec = item.CanExecute?.Invoke() ?? true;
                        if (canExec)
                        {
                            item.Command?.Invoke();
                            CloseMenu();
                        }
                        e.Handled = true;
                    }
                    break;
                }
                hit = VisualTreeHelper.GetParent(hit);
            }
        }
        private void CheckMouseOutsidePopup(object sender, EventArgs e)
        {
            if (!_trayPopup.IsOpen) return;
            Point mousePos = Mouse.GetPosition(_listBox);
            Rect menuBounds = VisualTreeHelper.GetDescendantBounds(_listBox);
            if (!menuBounds.Contains(mousePos))
            {
                CloseMenu();
            }
        }
        private void OnTrayPopupClosed(object sender, EventArgs e)
        {
            _listBox.SelectedIndex = -1;
            UnhookCloseEvents();
            _closeCheckTimer.Stop();
        }
        private void HookCloseEvents()
        {
            UnhookCloseEvents();
            _hookedWindow = Application.Current?.MainWindow;
            if (_hookedWindow != null)
            {
                _hookedWindow.Deactivated += OnWindowDeactivated;
                _hookedWindow.LocationChanged += OnWindowLocationChanged;
                _hookedWindow.StateChanged += OnWindowStateChanged;
            }
        }
        private void UnhookCloseEvents()
        {
            if (_hookedWindow != null)
            {
                _hookedWindow.Deactivated -= OnWindowDeactivated;
                _hookedWindow.LocationChanged -= OnWindowLocationChanged;
                _hookedWindow.StateChanged -= OnWindowStateChanged;
                _hookedWindow = null;
            }
        }
        private void OnWindowDeactivated(object sender, EventArgs e) => CloseMenu();
        private void OnWindowLocationChanged(object sender, EventArgs e) => CloseMenu();
        private void OnWindowStateChanged(object sender, EventArgs e) => CloseMenu();
        private Point GetMouseScreenPositionDip()
        {
            GetCursorPos(out POINT pt);
            var transform = PresentationSource.FromVisual(this)?.CompositionTarget?.TransformFromDevice
                            ?? Matrix.Identity;
            return transform.Transform(new Point(pt.X, pt.Y));
        }
        public new void Dispose()
        {
            TrayRightMouseDown -= OnTrayRightClick;
            CloseMenu();
            if (_trayPopup != null)
                _trayPopup.Closed -= OnTrayPopupClosed;
            if (_closeCheckTimer != null)
                _closeCheckTimer.Tick -= CheckMouseOutsidePopup;
            ClearMenuItems();
            Visibility = Visibility.Collapsed;
            base.Dispose();
        }
    }
}