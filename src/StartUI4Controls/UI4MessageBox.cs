using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace StartUI4Controls
{
    public enum UI4MessageBoxButtons
    {
        OK,
        OKCancel
    }

    public class UI4MessageBox : Window
    {
        private bool _isClosingAnimating;
        private const double ResizeThumbSize = 8;
        private Point _resizeStartPoint;
        private double _resizeStartWidth, _resizeStartHeight;
        private int _resizeDirection = 0;
        private readonly UI4MessageBoxButtons _buttonMode;
        private TextBlock _iconText;
        private TextBlock _headingText;
        private TextBlock _messageText;
        private UI4Button _okButton;
        private UI4Button _cancelButton;
        private Border _mainContainer;

        public UI4MessageBox(string title, string content, UI4MessageBoxButtons buttonMode = UI4MessageBoxButtons.OK)
        {
            _buttonMode = buttonMode;
            Title = title ?? UI4MultiLanguage.Get(UI4LanguageKey.Notice);
            Width = 460;
            MinHeight = 160;
            MaxHeight = 400;
            SizeToContent = SizeToContent.Height;
            ResizeMode = ResizeMode.CanResize;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ShowInTaskbar = false;
            FontFamily = new FontFamily("Segoe UI Variable Display, Segoe UI, sans-serif");
            Background = Brushes.Transparent;
            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;
            TextOptions.SetTextFormattingMode(this, TextFormattingMode.Display);

            _mainContainer = new Border
            {
                Margin = new Thickness(28),
                Background = new SolidColorBrush(Colors.White),
                CornerRadius = new CornerRadius(10),
                Effect = new DropShadowEffect
                {
                    Color = Colors.Black,
                    BlurRadius = 18,
                    ShadowDepth = 6,
                    Opacity = 0.3
                }
            };

            Grid rootGrid = new Grid { Margin = new Thickness(20) };
            rootGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            rootGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            rootGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            Grid headerGrid = new Grid();
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            _iconText = new TextBlock
            {
                FontFamily = new FontFamily("Segoe MDL2 Assets"),
                FontSize = 28,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 12, 0),
                Text = "\uE134"
            };
            Grid.SetColumn(_iconText, 0);
            headerGrid.Children.Add(_iconText);

            _headingText = new TextBlock
            {
                FontSize = 16,
                FontWeight = FontWeights.SemiBold,
                VerticalAlignment = VerticalAlignment.Center,
                TextTrimming = TextTrimming.CharacterEllipsis,
                Text = title
            };
            Grid.SetColumn(_headingText, 1);
            headerGrid.Children.Add(_headingText);
            Grid.SetRow(headerGrid, 0);
            rootGrid.Children.Add(headerGrid);

            _messageText = new TextBlock
            {
                FontSize = 14,
                TextWrapping = TextWrapping.Wrap,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 12, 0, 0),
                Opacity = 0.85,
                Text = content
            };
            Grid.SetRow(_messageText, 1);
            rootGrid.Children.Add(_messageText);

            Grid buttonWrapper = new Grid();
            buttonWrapper.Margin = new Thickness(0, 12, 0, 0);
            Grid.SetRow(buttonWrapper, 2);
            rootGrid.Children.Add(buttonWrapper);

            StackPanel buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
            };
            buttonWrapper.Children.Add(buttonPanel);
            BuildButtons(buttonPanel);

            _mainContainer.Child = rootGrid;
            _mainContainer.MouseLeftButtonDown += (ss, ee) =>
            {
                if (ee.ClickCount == 1) DragMove();
            };

            Grid resizeGrid = new Grid();
            Border left = new Border { Width = ResizeThumbSize, HorizontalAlignment = HorizontalAlignment.Left, Cursor = Cursors.SizeWE, Background = Brushes.Transparent };
            Border right = new Border { Width = ResizeThumbSize, HorizontalAlignment = HorizontalAlignment.Right, Cursor = Cursors.SizeWE, Background = Brushes.Transparent };
            Border top = new Border { Height = ResizeThumbSize, VerticalAlignment = VerticalAlignment.Top, Cursor = Cursors.SizeNS, Background = Brushes.Transparent };
            Border bottom = new Border { Height = ResizeThumbSize, VerticalAlignment = VerticalAlignment.Bottom, Cursor = Cursors.SizeNS, Background = Brushes.Transparent };
            Border topLeft = new Border { Width = ResizeThumbSize, Height = ResizeThumbSize, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, Cursor = Cursors.SizeNWSE, Background = Brushes.Transparent };
            Border topRight = new Border { Width = ResizeThumbSize, Height = ResizeThumbSize, HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Top, Cursor = Cursors.SizeNESW, Background = Brushes.Transparent };
            Border bottomLeft = new Border { Width = ResizeThumbSize, Height = ResizeThumbSize, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Bottom, Cursor = Cursors.SizeNESW, Background = Brushes.Transparent };
            Border bottomRight = new Border { Width = ResizeThumbSize, Height = ResizeThumbSize, HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom, Cursor = Cursors.SizeNWSE, Background = Brushes.Transparent };
            resizeGrid.Children.Add(_mainContainer);
            resizeGrid.Children.Add(left);
            resizeGrid.Children.Add(right);
            resizeGrid.Children.Add(top);
            resizeGrid.Children.Add(bottom);
            resizeGrid.Children.Add(topLeft);
            resizeGrid.Children.Add(topRight);
            resizeGrid.Children.Add(bottomLeft);
            resizeGrid.Children.Add(bottomRight);

            left.MouseLeftButtonDown += (s, e) => StartResize(e, 1);
            right.MouseLeftButtonDown += (s, e) => StartResize(e, 2);
            top.MouseLeftButtonDown += (s, e) => StartResize(e, 3);
            bottom.MouseLeftButtonDown += (s, e) => StartResize(e, 4);
            topLeft.MouseLeftButtonDown += (s, e) => StartResize(e, 5);
            topRight.MouseLeftButtonDown += (s, e) => StartResize(e, 6);
            bottomLeft.MouseLeftButtonDown += (s, e) => StartResize(e, 7);
            bottomRight.MouseLeftButtonDown += (s, e) => StartResize(e, 8);

            UI4Panel winUI4Style_Panel = new UI4Panel
            {
                Margin = new Thickness(20),
                HoverScale = 1,
                HoverBorderBrush = new SolidColorBrush(Colors.Transparent),
                BorderThickness = new Thickness(0),
                Background = Brushes.Transparent,
                ShadowBlurRadius = 0,
                ShadowOpacity = 0,
                ShadowDepth = 0
            };
            winUI4Style_Panel.Content = resizeGrid;
            this.Content = winUI4Style_Panel;
            this.Loaded += Window_LoadedAnim;
        }

        private void BuildButtons(StackPanel buttonPanel)
        {
            _okButton = new UI4Button
            {
                Content = UI4MultiLanguage.Get(UI4LanguageKey.OK),
                Width = 80,
                Height = 32,
                FontSize = 13,
                Cursor = Cursors.Hand,
                IsDefault = true
            };
            _okButton.Click += OkButton_Click;
            buttonPanel.Children.Add(_okButton);

            if (_buttonMode == UI4MessageBoxButtons.OKCancel)
            {
                _cancelButton = new UI4Button
                {
                    Content = UI4MultiLanguage.Get(UI4LanguageKey.Cancel),
                    Width = 80,
                    Height = 32,
                    FontSize = 13,
                    Cursor = Cursors.Hand,
                    Margin = new Thickness(12, 0, 0, 0)
                };
                _cancelButton.Click += CloseButton_Click;
                buttonPanel.Children.Add(_cancelButton);
            }
        }

        private void Window_LoadedAnim(object s, RoutedEventArgs e)
        {
            if (!(this.Content is UI4Panel rootPanel))
                return;
            rootPanel.Opacity = 0;
            rootPanel.RenderTransform = new TransformGroup
            {
                Children = new TransformCollection
                {
                    new TranslateTransform(0, 90),
                    new ScaleTransform(0.88, 0.88)
                }
            };
            rootPanel.RenderTransformOrigin = new Point(0.5, 0.5);
            rootPanel.Effect = new BlurEffect { Radius = 14 };
            DoubleAnimation fadeAnim = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(200))
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            rootPanel.BeginAnimation(UIElement.OpacityProperty, fadeAnim);
            TransformGroup transformGroup = rootPanel.RenderTransform as TransformGroup;
            TranslateTransform translate = transformGroup.Children[0] as TranslateTransform;
            ScaleTransform scale = transformGroup.Children[1] as ScaleTransform;
            DoubleAnimation slideAnim = new DoubleAnimation(90, 0, TimeSpan.FromMilliseconds(200))
            {
                EasingFunction = new BackEase { EasingMode = EasingMode.EaseOut, Amplitude = 0.35 }
            };
            translate.BeginAnimation(TranslateTransform.YProperty, slideAnim);
            DoubleAnimation scaleAnim = new DoubleAnimation(0.88, 1, TimeSpan.FromMilliseconds(200))
            {
                EasingFunction = new BackEase { EasingMode = EasingMode.EaseOut, Amplitude = 0.5 }
            };
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnim);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnim);
            DoubleAnimation blurAnim = new DoubleAnimation(14, 0, TimeSpan.FromMilliseconds(200))
            {
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            rootPanel.Effect.BeginAnimation(BlurEffect.RadiusProperty, blurAnim);
        }

        private void CloseAnimation(bool dialogResult)
        {
            if (_isClosingAnimating) return;
            if (!(this.Content is UI4Panel rootPanel))
                return;
            if (!(rootPanel.RenderTransform is TransformGroup tg) || tg.Children.Count < 2)
                return;
            if (!(tg.Children[0] is TranslateTransform trans) || !(tg.Children[1] is ScaleTransform scale))
                return;
            BlurEffect blurEffect = rootPanel.Effect as BlurEffect;
            if (blurEffect == null)
            {
                blurEffect = new BlurEffect { Radius = 0 };
                rootPanel.Effect = blurEffect;
            }
            _isClosingAnimating = true;
            TimeSpan duration = TimeSpan.FromMilliseconds(200);
            DoubleAnimation fadeOut = new DoubleAnimation(1, 0, duration)
            {
                EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseIn },
                FillBehavior = FillBehavior.HoldEnd
            };
            DoubleAnimation slideDown = new DoubleAnimation(0, 90, duration)
            {
                EasingFunction = new BackEase() { EasingMode = EasingMode.EaseIn, Amplitude = 0.35 },
                FillBehavior = FillBehavior.HoldEnd
            };
            DoubleAnimation shrinkX = new DoubleAnimation(1, 0.88, duration)
            {
                EasingFunction = new BackEase() { EasingMode = EasingMode.EaseIn, Amplitude = 0.5 },
                FillBehavior = FillBehavior.HoldEnd
            };
            DoubleAnimation shrinkY = new DoubleAnimation(1, 0.88, duration)
            {
                EasingFunction = new BackEase() { EasingMode = EasingMode.EaseIn, Amplitude = 0.5 },
                FillBehavior = FillBehavior.HoldEnd
            };
            DoubleAnimation blurOut = new DoubleAnimation(0, 14, duration)
            {
                EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseIn },
                FillBehavior = FillBehavior.HoldEnd
            };
            int completeCount = 0;
            int totalAnim = 5;
            void OnAnyAnimationCompleted(object s, EventArgs e)
            {
                completeCount++;
                if (completeCount >= totalAnim)
                {
                    Dispatcher.Invoke(() =>
                    {
                        DialogResult = dialogResult;
                        this.Close();
                    });
                }
            }
            fadeOut.Completed += OnAnyAnimationCompleted;
            slideDown.Completed += OnAnyAnimationCompleted;
            shrinkX.Completed += OnAnyAnimationCompleted;
            shrinkY.Completed += OnAnyAnimationCompleted;
            blurOut.Completed += OnAnyAnimationCompleted;
            rootPanel.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            trans.BeginAnimation(TranslateTransform.YProperty, slideDown);
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, shrinkX);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, shrinkY);
            blurEffect.BeginAnimation(BlurEffect.RadiusProperty, blurOut);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            CloseAnimation(true);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CloseAnimation(false);
        }

        public static bool? Show(string content,
            string title = null,
            UI4MessageBoxButtons buttons = UI4MessageBoxButtons.OK,
            double width = 460)
        {
            UI4MessageBox box = new UI4MessageBox(title ?? UI4MultiLanguage.Get(UI4LanguageKey.Notice), content, buttons);
            box.Width = width;
            return box.ShowDialog();
        }

        private void StartResize(MouseButtonEventArgs e, int direction)
        {
            _resizeDirection = direction;
            _resizeStartPoint = PointToScreen(e.GetPosition(this));
            _resizeStartWidth = Width;
            _resizeStartHeight = Height;
            Mouse.Capture(this);
            MouseMove += DoResize;
            MouseLeftButtonUp += EndResize;
        }

        private void DoResize(object sender, MouseEventArgs e)
        {
            if (_resizeDirection == 0 || e.LeftButton != MouseButtonState.Pressed) return;
            Point current = PointToScreen(e.GetPosition(this));
            double dx = current.X - _resizeStartPoint.X;
            double dy = current.Y - _resizeStartPoint.Y;
            double minW = 200, minH = 150;
            switch (_resizeDirection)
            {
                case 2: Width = Math.Max(minW, _resizeStartWidth + dx); break;
                case 1: Width = Math.Max(minW, _resizeStartWidth - dx); break;
                case 4: Height = Math.Max(minH, _resizeStartHeight + dy); break;
                case 3: Height = Math.Max(minH, _resizeStartHeight - dy); break;
                case 6: Width = Math.Max(minW, _resizeStartWidth + dx); Height = Math.Max(minH, _resizeStartHeight - dy); break;
                case 5: Width = Math.Max(minW, _resizeStartWidth - dx); Height = Math.Max(minH, _resizeStartHeight - dy); break;
                case 8: Width = Math.Max(minW, _resizeStartWidth + dx); Height = Math.Max(minH, _resizeStartHeight + dy); break;
                case 7: Width = Math.Max(minW, _resizeStartWidth - dx); Height = Math.Max(minH, _resizeStartHeight + dy); break;
            }
        }

        private void EndResize(object sender, MouseButtonEventArgs e)
        {
            _resizeDirection = 0;
            Mouse.Capture(null);
            MouseMove -= DoResize;
            MouseLeftButtonUp -= EndResize;
        }
    }
}