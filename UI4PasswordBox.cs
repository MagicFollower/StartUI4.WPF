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
using System.Windows.Threading;

namespace StartUI4Controls
{
    public class UI4PasswordBox : TextBox
    {
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(UI4PasswordBox),
                new PropertyMetadata(new CornerRadius(6), OnStyleRefresh));
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty BorderNormalColorProperty =
            DependencyProperty.Register(nameof(BorderNormalColor), typeof(Color), typeof(UI4PasswordBox),
                new PropertyMetadata(Color.FromRgb(200, 200, 220), OnStyleRefresh));
        public Color BorderNormalColor
        {
            get => (Color)GetValue(BorderNormalColorProperty);
            set => SetValue(BorderNormalColorProperty, value);
        }

        public static readonly DependencyProperty FocusGradientStartProperty =
            DependencyProperty.Register(nameof(FocusGradientStart), typeof(Color), typeof(UI4PasswordBox),
                new PropertyMetadata(Color.FromRgb(0, 120, 212), OnStyleRefresh));
        public Color FocusGradientStart
        {
            get => (Color)GetValue(FocusGradientStartProperty);
            set => SetValue(FocusGradientStartProperty, value);
        }

        public static readonly DependencyProperty FocusGradientEndProperty =
            DependencyProperty.Register(nameof(FocusGradientEnd), typeof(Color), typeof(UI4PasswordBox),
                new PropertyMetadata(Color.FromRgb(147, 51, 234), OnStyleRefresh));
        public Color FocusGradientEnd
        {
            get => (Color)GetValue(FocusGradientEndProperty);
            set => SetValue(FocusGradientEndProperty, value);
        }

        public static readonly DependencyProperty EditBackgroundProperty =
            DependencyProperty.Register(nameof(EditBackground), typeof(Brush), typeof(UI4PasswordBox),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 255, 255)), OnStyleRefresh));
        public Brush EditBackground
        {
            get => (Brush)GetValue(EditBackgroundProperty);
            set => SetValue(EditBackgroundProperty, value);
        }

        public static readonly DependencyProperty TextColorProperty =
            DependencyProperty.Register(nameof(TextColor), typeof(Color), typeof(UI4PasswordBox),
                new PropertyMetadata(Color.FromRgb(30, 30, 30), OnStyleRefresh));
        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public static readonly DependencyProperty InnerPaddingProperty =
            DependencyProperty.Register(nameof(InnerPadding), typeof(Thickness), typeof(UI4PasswordBox),
                new PropertyMetadata(new Thickness(12, 5, 32, 5), OnStyleRefresh));
        public Thickness InnerPadding
        {
            get => (Thickness)GetValue(InnerPaddingProperty);
            set => SetValue(InnerPaddingProperty, value);
        }

        public static readonly DependencyProperty ShowClearButtonProperty =
            DependencyProperty.Register(nameof(ShowClearButton), typeof(bool), typeof(UI4PasswordBox),
                new PropertyMetadata(false, OnStyleRefresh));
        public bool ShowClearButton
        {
            get => (bool)GetValue(ShowClearButtonProperty);
            set => SetValue(ShowClearButtonProperty, value);
        }

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register(nameof(PlaceholderText), typeof(string), typeof(UI4PasswordBox),
                new PropertyMetadata(string.Empty, OnStyleRefresh));
        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }

        public static readonly DependencyProperty PlaceholderForegroundProperty =
            DependencyProperty.Register(nameof(PlaceholderForeground), typeof(Brush), typeof(UI4PasswordBox),
                new PropertyMetadata(new SolidColorBrush(Colors.LightGray), OnStyleRefresh));
        public Brush PlaceholderForeground
        {
            get => (Brush)GetValue(PlaceholderForegroundProperty);
            set => SetValue(PlaceholderForegroundProperty, value);
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(nameof(Password), typeof(string), typeof(UI4PasswordBox),
                new PropertyMetadata(string.Empty, OnPasswordChanged));
        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        public static readonly DependencyProperty PasswordCharProperty =
            DependencyProperty.Register(nameof(PasswordChar), typeof(char), typeof(UI4PasswordBox),
                new PropertyMetadata('●', OnStyleRefresh));
        public char PasswordChar
        {
            get => (char)GetValue(PasswordCharProperty);
            set => SetValue(PasswordCharProperty, value);
        }

        public static readonly DependencyProperty IsPasswordModeProperty =
            DependencyProperty.Register(nameof(IsPasswordMode), typeof(bool), typeof(UI4PasswordBox),
                new PropertyMetadata(true, OnPasswordModeChanged));
        public bool IsPasswordMode
        {
            get => (bool)GetValue(IsPasswordModeProperty);
            set => SetValue(IsPasswordModeProperty, value);
        }

        private static void OnPasswordModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = d as UI4PasswordBox;
            if (box != null)
            {
                box.UpdateDisplay();
                box.RebuildContextMenu();
            }
        }

        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = d as UI4PasswordBox;
            if (box != null)
            {
                box._password = e.NewValue as string ?? string.Empty;
                if (box.IsPasswordMode)
                    box.UpdateDisplay();
            }
        }

        private static void OnStyleRefresh(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UI4PasswordBox box) box.Style = box.BuildEditStyle();
        }

        private static string _scrollBarResourcesXaml;
        private ScrollViewer _scrollViewer;
        private ScrollBar _verticalScrollBar;
        private DispatcherTimer _fadeTimer;
        private EventHandler _fadeTimerTickHandler;
        private UI4ContextMenu _contextMenu;
        private string _password = string.Empty;
        private int? _pendingCaretIndex;
        private bool _showPlainText = false;
        private bool _suppressTextChanged = false;

        static UI4PasswordBox()
        {
            _scrollBarResourcesXaml = GetScrollBarResourcesXaml();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UI4PasswordBox),
                new FrameworkPropertyMetadata(typeof(UI4PasswordBox)));
        }

        private static string GetScrollBarResourcesXaml()
        {
            return @"
<ResourceDictionary xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'
                    xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>
    <Style x:Key='ScrollBarThumb' TargetType='{x:Type Thumb}'>
        <Setter Property='OverridesDefaultStyle' Value='true'/>
        <Setter Property='IsTabStop' Value='false'/>
        <Setter Property='Template'>
            <Setter.Value>
                <ControlTemplate TargetType='{x:Type Thumb}'>
                    <Rectangle Fill='#90000000' RadiusX='3' RadiusY='3'/>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
    <Style x:Key='HorizontalScrollBarPageButton' TargetType='{x:Type RepeatButton}'>
        <Setter Property='OverridesDefaultStyle' Value='true'/>
        <Setter Property='Background' Value='Transparent'/>
        <Setter Property='Focusable' Value='false'/>
        <Setter Property='IsTabStop' Value='false'/>
        <Setter Property='Opacity' Value='0'/>
        <Setter Property='Template'>
            <Setter.Value>
                <ControlTemplate TargetType='{x:Type RepeatButton}'>
                    <Rectangle Fill='{TemplateBinding Background}'
                               Width='{TemplateBinding Width}'
                               Height='{TemplateBinding Height}'/>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
    <Style x:Key='VerticalScrollBarPageButton' TargetType='{x:Type RepeatButton}'>
        <Setter Property='OverridesDefaultStyle' Value='true'/>
        <Setter Property='Background' Value='Transparent'/>
        <Setter Property='Focusable' Value='false'/>
        <Setter Property='IsTabStop' Value='false'/>
        <Setter Property='Opacity' Value='0'/>
        <Setter Property='Template'>
            <Setter.Value>
                <ControlTemplate TargetType='{x:Type RepeatButton}'>
                    <Rectangle Fill='{TemplateBinding Background}'
                               Width='{TemplateBinding Width}'
                               Height='{TemplateBinding Height}'/>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
    <Style TargetType='{x:Type ScrollBar}'>
        <Setter Property='Stylus.IsPressAndHoldEnabled' Value='false'/>
        <Setter Property='Stylus.IsFlicksEnabled' Value='false'/>
        <Setter Property='Background' Value='Transparent'/>
        <Setter Property='Margin' Value='0,1,2,6'/>
        <Setter Property='Width' Value='6'/>
        <Setter Property='MinWidth' Value='6'/>
        <Setter Property='Opacity' Value='0'/>
        <Setter Property='Template'>
            <Setter.Value>
                <ControlTemplate TargetType='{x:Type ScrollBar}'>
                    <Grid x:Name='Bg' SnapsToDevicePixels='true'>
                        <Track x:Name='PART_Track' IsDirectionReversed='true'>
                            <Track.DecreaseRepeatButton>
                                <RepeatButton Style='{StaticResource VerticalScrollBarPageButton}'
                                              Command='{x:Static ScrollBar.PageUpCommand}'/>
                            </Track.DecreaseRepeatButton>
                            <Track.IncreaseRepeatButton>
                                <RepeatButton Style='{StaticResource VerticalScrollBarPageButton}'
                                              Command='{x:Static ScrollBar.PageDownCommand}'/>
                            </Track.IncreaseRepeatButton>
                            <Track.Thumb>
                                <Thumb Style='{StaticResource ScrollBarThumb}'/>
                            </Track.Thumb>
                        </Track>
                    </Grid>
                    <ControlTemplate.Triggers>
                        <Trigger Property='IsMouseOver' Value='True'>
                            <Trigger.EnterActions>
                                <BeginStoryboard>
                                    <Storyboard>
                                        <DoubleAnimation Storyboard.TargetProperty='Opacity' To='1' Duration='0:0:0.2'/>
                                    </Storyboard>
                                </BeginStoryboard>
                            </Trigger.EnterActions>
                            <Trigger.ExitActions>
                                <BeginStoryboard>
                                    <Storyboard>
                                        <DoubleAnimation Storyboard.TargetProperty='Opacity' To='0' Duration='0:0:0.5'/>
                                    </Storyboard>
                                </BeginStoryboard>
                            </Trigger.ExitActions>
                        </Trigger>
                    </ControlTemplate.Triggers>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
        <Style.Triggers>
            <Trigger Property='Orientation' Value='Horizontal'>
                <Setter Property='Background' Value='Transparent'/>
                <Setter Property='Margin' Value='2,0,6,2'/>
                <Setter Property='Height' Value='6'/>
                <Setter Property='MinHeight' Value='6'/>
                <Setter Property='Width' Value='Auto'/>
                <Setter Property='Opacity' Value='0'/>
                <Setter Property='Template'>
                    <Setter.Value>
                        <ControlTemplate TargetType='{x:Type ScrollBar}'>
                            <Grid x:Name='Bg' SnapsToDevicePixels='true'>
                                <Track x:Name='PART_Track'>
                                    <Track.DecreaseRepeatButton>
                                        <RepeatButton Style='{StaticResource HorizontalScrollBarPageButton}'
                                                      Command='{x:Static ScrollBar.PageLeftCommand}'/>
                                    </Track.DecreaseRepeatButton>
                                    <Track.IncreaseRepeatButton>
                                        <RepeatButton Style='{StaticResource HorizontalScrollBarPageButton}'
                                                      Command='{x:Static ScrollBar.PageRightCommand}'/>
                                    </Track.IncreaseRepeatButton>
                                    <Track.Thumb>
                                        <Thumb Style='{StaticResource ScrollBarThumb}'/>
                                    </Track.Thumb>
                                </Track>
                            </Grid>
                            <ControlTemplate.Triggers>
                                <Trigger Property='IsMouseOver' Value='True'>
                                    <Trigger.EnterActions>
                                        <BeginStoryboard>
                                            <Storyboard>
                                                <DoubleAnimation Storyboard.TargetProperty='Opacity' To='1' Duration='0:0:0.2'/>
                                            </Storyboard>
                                        </BeginStoryboard>
                                    </Trigger.EnterActions>
                                    <Trigger.ExitActions>
                                        <BeginStoryboard>
                                            <Storyboard>
                                                <DoubleAnimation Storyboard.TargetProperty='Opacity' To='0' Duration='0:0:0.5'/>
                                            </Storyboard>
                                        </BeginStoryboard>
                                    </Trigger.ExitActions>
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

        private static ResourceDictionary CreateScrollBarResources()
            => (ResourceDictionary)XamlReader.Parse(_scrollBarResourcesXaml);

        public UI4PasswordBox()
        {
            FontSize = 15d;
            Cursor = Cursors.IBeam;
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            var resDict = CreateScrollBarResources();
            foreach (DictionaryEntry entry in resDict)
                if (!Resources.Contains(entry.Key))
                    Resources.Add(entry.Key, entry.Value);

            Style = BuildEditStyle();
            Loaded += UI4PasswordBox_Loaded;
            Unloaded += UI4PasswordBox_Unloaded;
            LostFocus += (s, e) => HidePlainText();

            PreviewTextInput += OnPreviewTextInput;
            PreviewKeyDown += OnPreviewKeyDown;
            TextChanged += OnTextChanged;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, OnPasteExecuted, OnPasteCanExecute));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, OnCopyExecuted, OnCopyCanExecute));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, OnCutExecuted, OnCutCanExecute));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _scrollViewer = GetTemplateChild("PART_ContentHost") as ScrollViewer;
            if (_scrollViewer != null)
            {
                _scrollViewer.ApplyTemplate();
                _verticalScrollBar = _scrollViewer.Template.FindName("PART_VerticalScrollBar", _scrollViewer) as ScrollBar;
                if (_verticalScrollBar != null)
                {
                    _verticalScrollBar.MouseEnter += OnScrollBarMouseEnter;
                    _verticalScrollBar.MouseLeave += OnScrollBarMouseLeave;
                }
                _scrollViewer.ScrollChanged += OnScrollViewerScrollChanged;
            }
        }

        private void UI4PasswordBox_Loaded(object sender, RoutedEventArgs e)
        {
            RebuildContextMenu();
            if (_scrollViewer == null)
            {
                _scrollViewer = GetTemplateChild("PART_ContentHost") as ScrollViewer;
                if (_scrollViewer != null)
                {
                    _scrollViewer.ApplyTemplate();
                    _verticalScrollBar = _scrollViewer.Template.FindName("PART_VerticalScrollBar", _scrollViewer) as ScrollBar;
                    if (_verticalScrollBar != null)
                    {
                        _verticalScrollBar.MouseEnter += OnScrollBarMouseEnter;
                        _verticalScrollBar.MouseLeave += OnScrollBarMouseLeave;
                    }
                    _scrollViewer.ScrollChanged += OnScrollViewerScrollChanged;
                }
            }
            if (IsPasswordMode)
                UpdateDisplay();
        }

        private void UI4PasswordBox_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_scrollViewer != null)
                _scrollViewer.ScrollChanged -= OnScrollViewerScrollChanged;
            if (_verticalScrollBar != null)
            {
                _verticalScrollBar.MouseEnter -= OnScrollBarMouseEnter;
                _verticalScrollBar.MouseLeave -= OnScrollBarMouseLeave;
            }
            _fadeTimer?.Stop();
            _fadeTimer = null;
            _fadeTimerTickHandler = null;
        }

        private void OnScrollBarMouseEnter(object sender, MouseEventArgs e)
        {
            _fadeTimer?.Stop();
            _verticalScrollBar?.BeginAnimation(UIElement.OpacityProperty, null);
        }

        private void OnScrollBarMouseLeave(object sender, MouseEventArgs e)
        {
            if (_verticalScrollBar != null && _verticalScrollBar.Opacity > 0.9)
            {
                _verticalScrollBar.BeginAnimation(UIElement.OpacityProperty, null);
                _verticalScrollBar.Opacity = 0.4;
                StartFadeTimer();
            }
        }

        private void OnScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (_verticalScrollBar == null)
            {
                if (_scrollViewer != null)
                {
                    _scrollViewer.ApplyTemplate();
                    _verticalScrollBar = _scrollViewer.Template.FindName("PART_VerticalScrollBar", _scrollViewer) as ScrollBar;
                    if (_verticalScrollBar != null)
                    {
                        _verticalScrollBar.MouseEnter += OnScrollBarMouseEnter;
                        _verticalScrollBar.MouseLeave += OnScrollBarMouseLeave;
                    }
                }
                if (_verticalScrollBar == null) return;
            }
            _verticalScrollBar.BeginAnimation(UIElement.OpacityProperty,
                new DoubleAnimation(0.4, TimeSpan.FromSeconds(0.25)) { FillBehavior = FillBehavior.HoldEnd });
            StartFadeTimer();
        }

        private void StartFadeTimer()
        {
            _fadeTimer?.Stop();
            if (_fadeTimerTickHandler != null && _fadeTimer != null)
                _fadeTimer.Tick -= _fadeTimerTickHandler;

            _fadeTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1.5) };
            _fadeTimerTickHandler = (s, args) =>
            {
                _fadeTimer.Stop();
                if (_verticalScrollBar != null && !_verticalScrollBar.IsMouseOver)
                {
                    _verticalScrollBar.BeginAnimation(UIElement.OpacityProperty,
                        new DoubleAnimation(0, TimeSpan.FromSeconds(0.5)) { FillBehavior = FillBehavior.HoldEnd });
                }
                else if (_verticalScrollBar != null && _verticalScrollBar.IsMouseOver)
                {
                    StartFadeTimer();
                }
            };
            _fadeTimer.Tick += _fadeTimerTickHandler;
            _fadeTimer.Start();
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsPasswordMode) return;
            e.Handled = true;
            string input = e.Text;
            if (string.IsNullOrEmpty(input)) return;
            ProcessTextInput(input);
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!IsPasswordMode) return;

            if (e.Key == Key.Back)
            {
                e.Handled = true;
                ProcessBackspace();
            }
            else if (e.Key == Key.Delete)
            {
                e.Handled = true;
                ProcessDelete();
            }
            else if (e.Key == Key.Escape)
            {
                e.Handled = true;
                SelectAll();
            }
            else if ((e.Key == Key.V && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control))
            {
                e.Handled = true;
                PasteClipboard();
            }
            else if ((e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control) ||
                     (e.Key == Key.X && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control))
            {
                e.Handled = true;
            }
        }

        private void ProcessTextInput(string input)
        {
            int start = Math.Clamp(SelectionStart, 0, _password.Length);
            int length = Math.Clamp(SelectionLength, 0, _password.Length - start);
            string current = _password;
            if (length > 0)
                current = current.Remove(start, length);
            current = current.Insert(start, input);
            int newPos = start + input.Length;
            _password = current;
            Password = current;
            _pendingCaretIndex = newPos;
            UpdateDisplay();
        }

        private void ProcessBackspace()
        {
            int start = Math.Clamp(SelectionStart, 0, _password.Length);
            int length = Math.Clamp(SelectionLength, 0, _password.Length - start);
            string current = _password;
            if (length > 0)
            {
                current = current.Remove(start, length);
                _pendingCaretIndex = start;
            }
            else if (start > 0)
            {
                current = current.Remove(start - 1, 1);
                _pendingCaretIndex = start - 1;
            }
            else
                return;
            _password = current;
            Password = current;
            UpdateDisplay();
        }

        private void ProcessDelete()
        {
            int start = Math.Clamp(SelectionStart, 0, _password.Length);
            int length = Math.Clamp(SelectionLength, 0, _password.Length - start);
            string current = _password;
            if (length > 0)
            {
                current = current.Remove(start, length);
                _pendingCaretIndex = start;
            }
            else if (start < current.Length)
            {
                current = current.Remove(start, 1);
                _pendingCaretIndex = start;
            }
            else
                return;
            _password = current;
            Password = current;
            UpdateDisplay();
        }

        private void PasteClipboard()
        {
            if (Clipboard.ContainsText())
            {
                string text = Clipboard.GetText();
                if (!string.IsNullOrEmpty(text))
                {
                    ProcessTextInput(text);
                }
            }
        }

        private void UpdateDisplay()
        {
            if (IsPasswordMode && !_showPlainText)
            {
                string mask = new string(PasswordChar, _password.Length);
                _suppressTextChanged = true;
                Text = mask;
                _suppressTextChanged = false;
                if (_pendingCaretIndex.HasValue)
                {
                    int idx = Math.Clamp(_pendingCaretIndex.Value, 0, _password.Length);
                    SelectionStart = idx;
                    _pendingCaretIndex = null;
                }
            }
            else
            {
                _suppressTextChanged = true;
                Text = _password;
                _suppressTextChanged = false;
                if (_pendingCaretIndex.HasValue)
                {
                    int idx = Math.Clamp(_pendingCaretIndex.Value, 0, _password.Length);
                    SelectionStart = idx;
                    _pendingCaretIndex = null;
                }
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_suppressTextChanged) return;
            if (!IsPasswordMode)
            {
                _password = Text;
                Password = Text;
            }
        }

        private void ShowPlainText()
        {
            if (!IsPasswordMode) return;
            _showPlainText = true;
            IsPasswordMode = false;
            UpdateDisplay();
        }

        private void HidePlainText()
        {
            if (_showPlainText == false && IsPasswordMode) return;
            _showPlainText = false;
            IsPasswordMode = true;
            UpdateDisplay();
        }

        private void OnRevealButtonMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ShowPlainText();
                (sender as Button)?.CaptureMouse();
                e.Handled = true;
            }
        }

        private void OnRevealButtonMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                if ((sender as Button)?.IsMouseCaptured == true)
                {
                    (sender as Button)?.ReleaseMouseCapture();
                }
                HidePlainText();
                e.Handled = true;
            }
        }

        private void OnRevealButtonMouseLeave(object sender, MouseEventArgs e)
        {
            if ((sender as Button)?.IsMouseCaptured == true)
            {
                (sender as Button)?.ReleaseMouseCapture();
                HidePlainText();
            }
        }

        private void OnPasteExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsPasswordMode)
            {
                PasteClipboard();
                e.Handled = true;
            }
        }

        private void OnPasteCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        private void OnCopyExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsPasswordMode)
                e.Handled = true;
        }

        private void OnCopyCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !IsPasswordMode;
            e.Handled = true;
        }

        private void OnCutExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsPasswordMode)
                e.Handled = true;
        }

        private void OnCutCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !IsPasswordMode;
            e.Handled = true;
        }

        private void RebuildContextMenu()
        {
            _contextMenu = null;
            ContextMenu = null;
            InitCustomMenu();
        }

        private void InitCustomMenu()
        {
            if (_contextMenu != null) return;

            _contextMenu = new UI4ContextMenu
            {
                Width = 170
            };

            _contextMenu.AddItem(UI4MenuItemType.Undo,
                () => { if (CanUndo) Undo(); },
                () => CanUndo);

            if (IsPasswordMode)
            {
                _contextMenu.AddItem(UI4MenuItemType.Paste,
                    () => PasteClipboard(),
                    () => Clipboard.ContainsText());
                _contextMenu.AddItem(UI4MenuItemType.SelectAll,
                    () => SelectAll());
            }
            else
            {
                _contextMenu.AddItem(UI4MenuItemType.Cut,
                    () => { if (!string.IsNullOrEmpty(SelectedText)) Cut(); },
                    () => !string.IsNullOrEmpty(SelectedText));
                _contextMenu.AddItem(UI4MenuItemType.Copy,
                    () => { if (!string.IsNullOrEmpty(SelectedText)) Copy(); },
                    () => !string.IsNullOrEmpty(SelectedText));
                _contextMenu.AddItem(UI4MenuItemType.Paste,
                    () => { if (Clipboard.ContainsText()) Paste(); },
                    () => Clipboard.ContainsText());
                _contextMenu.AddItem(UI4MenuItemType.Delete,
                    () => { if (!string.IsNullOrEmpty(SelectedText)) SelectedText = string.Empty; },
                    () => !string.IsNullOrEmpty(SelectedText));
                _contextMenu.AddItem(UI4MenuItemType.SelectAll,
                    () => SelectAll());
            }

            ContextMenu = null;
            _contextMenu.Attach(this);
        }

        private Style BuildEditStyle()
        {
            Style style = new Style(typeof(TextBox));
            style.Setters.Add(new Setter(ForegroundProperty, new SolidColorBrush(TextColor)));
            style.Setters.Add(new Setter(PaddingProperty, InnerPadding));
            style.Setters.Add(new Setter(BackgroundProperty, EditBackground));
            style.Setters.Add(new Setter(BorderThicknessProperty, new Thickness(1)));
            style.Setters.Add(new Setter(BorderBrushProperty, new SolidColorBrush(BorderNormalColor)));
            style.Setters.Add(new Setter(CursorProperty, Cursors.IBeam));

            ControlTemplate template = new ControlTemplate(typeof(TextBox));
            FrameworkElementFactory borderRoot = new FrameworkElementFactory(typeof(Border));
            borderRoot.SetBinding(Border.CornerRadiusProperty, new Binding(nameof(CornerRadius)) { RelativeSource = RelativeSource.TemplatedParent });
            borderRoot.SetBinding(Border.BackgroundProperty, new Binding(nameof(Background)) { RelativeSource = RelativeSource.TemplatedParent });
            borderRoot.SetBinding(Border.BorderBrushProperty, new Binding(nameof(BorderBrush)) { RelativeSource = RelativeSource.TemplatedParent });
            borderRoot.SetBinding(Border.BorderThicknessProperty, new Binding(nameof(BorderThickness)) { RelativeSource = RelativeSource.TemplatedParent });

            FrameworkElementFactory grid = new FrameworkElementFactory(typeof(Grid));

            FrameworkElementFactory placeholderText = new FrameworkElementFactory(typeof(TextBlock));
            placeholderText.SetBinding(TextBlock.TextProperty, new Binding(nameof(PlaceholderText)) { RelativeSource = RelativeSource.TemplatedParent });
            placeholderText.SetBinding(TextBlock.ForegroundProperty, new Binding(nameof(PlaceholderForeground)) { RelativeSource = RelativeSource.TemplatedParent });
            placeholderText.SetBinding(TextBlock.FontSizeProperty, new Binding(nameof(FontSize)) { RelativeSource = RelativeSource.TemplatedParent });
            placeholderText.SetBinding(TextBlock.FontFamilyProperty, new Binding(nameof(FontFamily)) { RelativeSource = RelativeSource.TemplatedParent });
            placeholderText.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            placeholderText.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            placeholderText.SetValue(FrameworkElement.MarginProperty, new Thickness(14, 0, 0, 0));
            placeholderText.SetValue(Panel.ZIndexProperty, 0);
            placeholderText.SetBinding(UIElement.VisibilityProperty, new Binding(nameof(Text))
            {
                RelativeSource = RelativeSource.TemplatedParent,
                Converter = new PlaceholderVisibilityConverter()
            });
            grid.AppendChild(placeholderText);

            FrameworkElementFactory scrollViewer = new FrameworkElementFactory(typeof(ScrollViewer));
            scrollViewer.Name = "PART_ContentHost";
            scrollViewer.SetValue(Control.BorderThicknessProperty, new Thickness(0));
            scrollViewer.SetBinding(ScrollViewer.HorizontalScrollBarVisibilityProperty,
                new Binding(nameof(HorizontalScrollBarVisibility)) { RelativeSource = RelativeSource.TemplatedParent });
            scrollViewer.SetBinding(ScrollViewer.VerticalScrollBarVisibilityProperty,
                new Binding(nameof(VerticalScrollBarVisibility)) { RelativeSource = RelativeSource.TemplatedParent });
            scrollViewer.SetBinding(ScrollViewer.PaddingProperty, new Binding(nameof(Padding)) { RelativeSource = RelativeSource.TemplatedParent });
            grid.AppendChild(scrollViewer);

            Style revealBtnStyle = new Style(typeof(Button));
            revealBtnStyle.Setters.Add(new Setter(Button.BorderThicknessProperty, new Thickness(0)));
            revealBtnStyle.Setters.Add(new Setter(Button.PaddingProperty, new Thickness(0)));
            revealBtnStyle.Setters.Add(new Setter(Button.CursorProperty, Cursors.Hand));
            revealBtnStyle.Setters.Add(new Setter(Button.ForegroundProperty, new SolidColorBrush(Color.FromArgb(150, 80, 80, 100))));
            revealBtnStyle.Setters.Add(new Setter(Button.MarginProperty, new Thickness(0, 0, 10, 0)));

            ControlTemplate revealBtnTemplate = new ControlTemplate(typeof(Button));
            FrameworkElementFactory revealPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
            revealPresenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            revealPresenter.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
            FrameworkElementFactory revealBg = new FrameworkElementFactory(typeof(Border));
            revealBg.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Button.BackgroundProperty));
            revealBg.AppendChild(revealPresenter);
            revealBtnTemplate.VisualTree = revealBg;
            revealBtnStyle.Setters.Add(new Setter(Button.TemplateProperty, revealBtnTemplate));

            Trigger revealHoverTrigger = new Trigger { Property = Button.IsMouseOverProperty, Value = true };
            revealHoverTrigger.Setters.Add(new Setter(Button.ForegroundProperty, new SolidColorBrush(Color.FromArgb(255, 80, 80, 100))));
            revealBtnStyle.Triggers.Add(revealHoverTrigger);

            FrameworkElementFactory revealBtn = new FrameworkElementFactory(typeof(Button));
            revealBtn.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Right);
            revealBtn.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Stretch);
            revealBtn.SetValue(Button.WidthProperty, 26d);
            revealBtn.SetValue(Button.HeightProperty, double.NaN);
            revealBtn.SetValue(Button.StyleProperty, revealBtnStyle);
            revealBtn.SetBinding(Button.BackgroundProperty, new Binding(nameof(Background)) { RelativeSource = RelativeSource.TemplatedParent });
            revealBtn.SetBinding(UIElement.VisibilityProperty, new Binding(nameof(ShowClearButton))
            {
                RelativeSource = RelativeSource.TemplatedParent,
                Converter = new BoolToVisibilityConverter()
            });
            revealBtn.SetValue(Panel.ZIndexProperty, 1);

            FrameworkElementFactory eyeIcon = new FrameworkElementFactory(typeof(TextBlock));
            eyeIcon.SetValue(TextBlock.TextProperty, "\xE052");
            eyeIcon.SetValue(TextBlock.FontSizeProperty, 18d);
            eyeIcon.SetValue(TextBlock.FontFamilyProperty, new FontFamily("Segoe MDL2 Assets"));
            eyeIcon.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            eyeIcon.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            revealBtn.AppendChild(eyeIcon);

            revealBtn.AddHandler(UIElement.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(OnRevealButtonMouseDown));
            revealBtn.AddHandler(UIElement.PreviewMouseLeftButtonUpEvent, new MouseButtonEventHandler(OnRevealButtonMouseUp));
            revealBtn.AddHandler(UIElement.MouseLeaveEvent, new MouseEventHandler(OnRevealButtonMouseLeave));

            grid.AppendChild(revealBtn);

            borderRoot.AppendChild(grid);
            template.VisualTree = borderRoot;
            style.Setters.Add(new Setter(TemplateProperty, template));

            Trigger focusTrigger = new Trigger { Property = IsFocusedProperty, Value = true };
            LinearGradientBrush focusBorderGrad = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 1),
                GradientStops = { new GradientStop(FocusGradientStart, 0), new GradientStop(FocusGradientEnd, 1) }
            };
            focusTrigger.Setters.Add(new Setter(BorderBrushProperty, focusBorderGrad));
            focusTrigger.Setters.Add(new Setter(BorderThicknessProperty, new Thickness(1.2)));
            style.Triggers.Add(focusTrigger);

            Trigger hoverTrigger = new Trigger { Property = IsMouseOverProperty, Value = true };
            hoverTrigger.Setters.Add(new Setter(BorderBrushProperty, new SolidColorBrush(Color.FromRgb(160, 160, 190))));
            style.Triggers.Add(hoverTrigger);

            return style;
        }
    }
}