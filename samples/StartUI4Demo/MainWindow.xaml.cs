using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using StartUI4Controls;

namespace StartUI4Demo
{
    public partial class MainWindow : Window
    {
        private static readonly Random Rng = new Random();
        private UI4ContextMenu _hostMenu;
        private int _tabCounter;
        private ListBoxItem _hoveredListItem;
        private DispatcherTimer _listEchoTimer;

        public List<CardItem> Cards { get; private set; }

        public MainWindow()
        {
            Cards = BuildCards();
            DataContext = this;

            InitializeComponent();

            CodeEditor.Text =
                "// UI4CodeEditor 基于 AvalonEdit，内置 C# 语法高亮与 UI4ContextMenu\r\n" +
                "public class Sample\r\n" +
                "{\r\n" +
                "    public int Add(int a, int b)\r\n" +
                "    {\r\n" +
                "        return a + b;\r\n" +
                "    }\r\n" +
                "}\r\n";

            BuildHostContextMenu();

            PwdBox.TextChanged += delegate { PwdEcho.Text = "Password = " + Describe(PwdBox.Password); };
            UpdateLangSample();
            ApplyStartupTab();
            RuntimeText.Text = "实际运行时：" + System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
            UI4Theme.ThemeChanged += delegate { UpdateScopeStatus(); UpdateThemeFooter(); };
            ApplyTheme(false);
            SetStatus("就绪");
            UpdateScopeStatus();
            UpdateThemeFooter();
            AttachValueWatchers();
            RefreshThemeKeyCombo();
        }

        /// <summary>
        /// 页脚主题显示走事件驱动，而不是 {Binding Path=(ui:UI4Theme.CurrentMode)}：
        /// CurrentMode 只是普通静态 CLR 属性，库发的是 StaticPropertyChanged，
        /// WPF 普通绑定需要同名静态 CurrentModeChanged 事件才更新，绑定会停在初值上不再刷新。
        /// </summary>
        private void UpdateThemeFooter()
        {
            if (ThemeFooter == null) return;
            ThemeFooter.Text = "主题: " + UI4Theme.ResolvedKey
                + "　CurrentMode=" + UI4Theme.CurrentMode
                + "　ResolvedMode=" + UI4Theme.ResolvedMode;
        }

        // 支持 "--tab=N" 直接打开指定分页，便于自动化逐页验证。
        private void ApplyStartupTab()
        {
            foreach (string arg in Environment.GetCommandLineArgs())
            {
                if (!arg.StartsWith("--tab=", StringComparison.OrdinalIgnoreCase)) continue;
                int index;
                if (int.TryParse(arg.Substring(6), NumberStyles.Integer, CultureInfo.InvariantCulture, out index)
                    && index >= 0 && index < DemoTabs.Items.Count)
                {
                    DemoTabs.SelectedIndex = index;
                }
            }
        }

        private static List<CardItem> BuildCards()
        {
            return new List<CardItem>
            {
                new CardItem("数据看板", "实时指标与趋势", "#FFFFF6E0", Brushes.Goldenrod),
                new CardItem("订单管理", "创建、跟踪与归档", "#FFE7F1FF", Brushes.SteelBlue),
                new CardItem("用户中心", "账户与权限设置", "#FFEAF8EE", Brushes.MediumSeaGreen),
                new CardItem("系统日志", "运行状态与告警", "#FFFDEAEA", Brushes.IndianRed),
            };
        }

        private void BuildHostContextMenu()
        {
            _hostMenu = new UI4ContextMenu
            {
                Width = 200,
                ItemPadding = new Thickness(12, 8, 12, 8),
                BorderColor = UI4Theme.Current.BorderNormalColor,
                HoverBackground = UI4Theme.Current.HoverOverlayColor
            };

            // 用 AddItem(type, Action, Func<bool>) 重载：文案由 UI4MultiLanguage 本地化，
            // 与"复制/粘贴/删除/全选"的实际行为天然一致，避免自造文案名不副实。
            _hostMenu.AddItem(UI4MenuItemType.Copy,
                () => UI4Clipboard.TrySetTextAsync(CtxSource.SelectedText, OnHostCopyDone),
                () => CtxSource.SelectionLength > 0);

            _hostMenu.AddItem(UI4MenuItemType.Paste,
                () => UI4Clipboard.TryGetTextAsync(OnHostPasteGot),
                () => UI4Clipboard.ContainsText());

            _hostMenu.AddItem(UI4MenuItemType.Delete,
                () =>
                {
                    CtxSource.SelectedText = string.Empty;
                    SetStatus("UI4ContextMenu → 已删除选中文本");
                },
                () => CtxSource.SelectionLength > 0);

            _hostMenu.AddItem(UI4MenuItemType.SelectAll,
                () =>
                {
                    CtxSource.SelectAll();
                    SetStatus("UI4ContextMenu → 已全选");
                });

            _hostMenu.Attach(CtxHost);
        }

        // 回调由 UI4Clipboard 投递回 UI 线程执行。
        private void OnHostCopyDone(bool ok)
        {
            SetStatus(ok
                ? "UI4ContextMenu → 已复制（切到记事本 Ctrl+V 可验证）"
                : "UI4ContextMenu → 复制失败：剪贴板被占用");
        }

        private void OnHostPasteGot(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                SetStatus("UI4ContextMenu → 剪贴板里没有文本");
                return;
            }

            CtxSource.SelectedText = text;
            SetStatus("UI4ContextMenu → 已粘贴 " + text.Length + " 字符");
        }

        private static string Describe(string value)
        {
            return string.IsNullOrEmpty(value) ? "(空)" : "\"" + value + "\" (" + value.Length + " 字符)";
        }

        private void SetStatus(string text)
        {
            // XAML 加载期间控件就可能触发事件（例如 UI4Switch 的 IsOn="True" 会立刻 Toggled），
            // 此时命名元素尚未赋值。
            if (StatusText != null) StatusText.Text = text;
        }

        // ---------- 主题 ----------

        private void Switch_Toggled(object sender, RoutedEventArgs e)
        {
            UI4Switch sw = (UI4Switch)sender;
            SetStatus("UI4Switch IsOn = " + sw.IsOn);
            ApplyTheme(sw.IsOn);
        }

        private void ApplyTheme(bool isDark)
        {
            // 资源桥：SetTheme 内部把令牌写入 Application.Resources，
            // 宿主 XAML 的 {DynamicResource UI4.Brush.X} 自动跟随，无需逐元素手工同步。
            UI4Theme.SetTheme(isDark ? UI4ThemeMode.Dark : UI4ThemeMode.Light);
        }

        // ---------- 按钮与开关 ----------

        private void PlainButton_Click(object sender, RoutedEventArgs e)
        {
            SetStatus("UI4Button 被点击");
        }

        private void AnyButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as ContentControl;
            SetStatus("UI4Button 被点击：" + (btn == null ? "(未知)" : btn.Content));
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var item = sender as UI4MenuElementItem;
            SetStatus("UI4Menu 菜单项：" + (item == null ? "(未知)" : item.Header));
        }

        // ---------- 就近回显 ----------

        private void TxtBasic_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtEcho == null || TxtBasic == null) return;
            TxtEcho.Text = "字符数 = " + TxtBasic.Text.Length.ToString(CultureInfo.InvariantCulture);
        }

        private void DemoCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboEcho == null || DemoCombo == null) return;
            var item = DemoCombo.SelectedItem as ComboBoxItem;
            string text = item == null ? "(空)" : Convert.ToString(item.Content, CultureInfo.InvariantCulture);
            if (text.Length > 24) text = text.Substring(0, 24) + "…";
            ComboEcho.Text = "选中：" + text;
        }

        private void DemoSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (SliderEcho == null) return;
            SliderEcho.Text = "UI4Slider Value = " + e.NewValue.ToString("0.#", CultureInfo.InvariantCulture);
        }

        private void DemoPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DemoPivot == null) return;
            var item = DemoPivot.SelectedItem as UI4PivotItem;
            SetStatus("UI4Pivot 当前页：" + (item == null ? "(空)" : item.Header));
        }

        private void BarToggleIndeterminate_Click(object sender, RoutedEventArgs e)
        {
            Bar1.IsIndeterminate = !Bar1.IsIndeterminate;
            BarEcho.Text = "Bar1 IsIndeterminate = " + Bar1.IsIndeterminate;
        }

        private void RingToggle_Click(object sender, RoutedEventArgs e)
        {
            Ring1.IsActive = !Ring1.IsActive;
            RingEcho.Text = "Ring1 IsActive = " + Ring1.IsActive;
        }

        private void RingAdvance_Click(object sender, RoutedEventArgs e)
        {
            Ring1.IsIndeterminate = false;
            double next = Ring1.Value + 10;
            Ring1.Value = next > Ring1.Maximum ? 0 : next;
            RingEcho.Text = "Ring1 Value = " + Ring1.Value.ToString("0", CultureInfo.InvariantCulture)
                + " / " + Ring1.Maximum.ToString("0", CultureInfo.InvariantCulture);
        }

        private void PanelUp_Click(object sender, RoutedEventArgs e)
        {
            SetStatus("UI4Panel 被点击（悬停缩放 + 阴影是它的两个卖点）");
        }

        private void ScrollTop_Click(object sender, RoutedEventArgs e)
        {
            DemoScrollViewer.SmoothScrollToVerticalOffset(0);
            ScrollEcho.Text = "已平滑滚动到顶部";
        }

        private void ScrollBottom_Click(object sender, RoutedEventArgs e)
        {
            DemoScrollViewer.SmoothScrollToVerticalOffset(double.MaxValue);
            ScrollEcho.Text = "已平滑滚动到底部";
        }

        // UI4CircleSlider / UI4NavigationView / UI4ProgressRing 只有依赖属性、没有路由事件，
        // 宿主用 DependencyPropertyDescriptor.AddValueChanged 订阅（进程内订阅一次即可）。
        private bool _watchersAttached;

        private void AttachValueWatchers()
        {
            if (_watchersAttached) return;
            _watchersAttached = true;

            DependencyPropertyDescriptor.FromProperty(UI4CircleSlider.ValueProperty, typeof(UI4CircleSlider))
                .AddValueChanged(CircleA, delegate
                {
                    SetStatus("UI4CircleSlider A Value = " + CircleA.Value.ToString("0", CultureInfo.InvariantCulture));
                });

            DependencyPropertyDescriptor.FromProperty(UI4NavigationView.SelectedItemProperty, typeof(UI4NavigationView))
                .AddValueChanged(NavView, delegate
                {
                    var item = NavView.SelectedItem as UI4NavigationViewItem;
                    SetStatus("UI4NavigationView → " + (item == null ? "(空)" : item.Header));
                });

            DependencyPropertyDescriptor.FromProperty(UI4ProgressRing.IsActiveProperty, typeof(UI4ProgressRing))
                .AddValueChanged(Ring1, delegate
                {
                    SetStatus("UI4ProgressRing IsActive = " + Ring1.IsActive);
                });
        }

        // ---------- 文本显示 ----------

        private void FlipButton_Click(object sender, RoutedEventArgs e)
        {
            FlipText.Text = Rng.Next(0, 100).ToString(CultureInfo.InvariantCulture);
        }

        // ---------- 进度 ----------

        private void AdvanceBar_Click(object sender, RoutedEventArgs e)
        {
            double next = Bar1.Value + 10;
            Bar1.Value = next > Bar1.Maximum ? 0 : next;
            SetStatus("UI4ProgressBar Value = " + Bar1.Value.ToString("0", CultureInfo.InvariantCulture));
        }

        // ---------- 列表与网格 ----------

        private void Cards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CardItem item = ((Selector)sender).SelectedItem as CardItem;
            if (item != null)
            {
                SetStatus(((Control)sender).GetType().Name + " 选中：" + item.Title);
            }
        }

        // 列数直接读控件算好的 ComputedColumns，不在 Demo 里再抄一遍算法（抄一遍就会错一次）。
        private void GridView1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateGridEcho();
        }

        // 不缩窗口也能验到"列数随可用宽度当场重算"：换基准单元会触发样式重建，
        // 而样式重建连带重建 UniformGrid，列数若不在重建时跟上就会滞留到下次缩放窗口。
        private void GridUnit_Click(object sender, RoutedEventArgs e)
        {
            GridView1.ItemWidth = GridView1.ItemWidth > 300 ? 230 : 400;
            UpdateGridEcho();
        }

        private void UpdateGridEcho()
        {
            if (GridEcho == null || GridView1 == null) return;

            GridEcho.Text = "UI4GridView 实际宽度 = " + Px(GridView1.ActualWidth)
                + " px，基准单元 " + Px(GridView1.ItemWidth)
                + " px → 当前 " + GridView1.ComputedColumns.ToString(CultureInfo.InvariantCulture)
                + " 列，卡片铺满所在列";
        }

        // 悬浮放大的自证：动画跑完（200ms）后量一次真实几何，
        // 报放大后的行左右两端距 UI4ListView 边界的距离；负数就是越出父容器。
        private void ListView1_MouseMove(object sender, MouseEventArgs e)
        {
            if (ListEcho == null || ListView1 == null) return;

            ListBoxItem container = ListContainerUnderMouse();
            if (container == null) return;

            _hoveredListItem = container;

            if (_listEchoTimer == null)
            {
                _listEchoTimer = new DispatcherTimer();
                _listEchoTimer.Interval = TimeSpan.FromMilliseconds(260);
                _listEchoTimer.Tick += ListEchoTimer_Tick;
            }
            _listEchoTimer.Stop();
            _listEchoTimer.Start();
        }

        private void ListEchoTimer_Tick(object sender, EventArgs e)
        {
            _listEchoTimer.Stop();

            ListBoxItem container = _hoveredListItem;
            if (container == null || ListView1 == null || ListEcho == null) return;

            ScaleTransform scale = container.RenderTransform as ScaleTransform;
            double sx = scale == null ? 1.0 : scale.ScaleX;
            double sy = scale == null ? 1.0 : scale.ScaleY;

            Point origin = container.TranslatePoint(new Point(0, 0), ListView1);
            double leftGap = origin.X;
            double rightGap = ListView1.ActualWidth - (origin.X + container.ActualWidth * sx);
            double topGap = origin.Y;
            double bottomGap = ListView1.ActualHeight - (origin.Y + container.ActualHeight * sy);

            ListEcho.Text = "行宽 " + Px(container.ActualWidth) + " px，生效 ScaleX "
                + sx.ToString("0.0000", CultureInfo.InvariantCulture) + " / ScaleY "
                + sy.ToString("0.0000", CultureInfo.InvariantCulture)
                + "；放大后距控件内边 左 " + Px(leftGap) + "、右 " + Px(rightGap)
                + "、上 " + Px(topGap) + "、下 " + Px(bottomGap)
                + " px（负数＝越出父容器）";
        }

        private ListBoxItem ListContainerUnderMouse()
        {
            Point p = Mouse.GetPosition(ListView1);
            foreach (object data in ListView1.Items)
            {
                ListBoxItem container = ListView1.ItemContainerGenerator.ContainerFromItem(data) as ListBoxItem;
                if (container == null) continue;

                Point origin = container.TranslatePoint(new Point(0, 0), ListView1);
                if (p.X >= origin.X && p.X <= origin.X + container.ActualWidth &&
                    p.Y >= origin.Y && p.Y <= origin.Y + container.ActualHeight)
                    return container;
            }
            return null;
        }

        private static string Px(double value)
        {
            return ((int)Math.Round(value)).ToString(CultureInfo.InvariantCulture);
        }

        // ---------- UI4Tab ----------

        private void BrowserTab_AddTab(object sender, RoutedEventArgs e)
        {
            _tabCounter++;
            UI4TabItem item = new UI4TabItem
            {
                Header = "新标签 " + _tabCounter,
                TextIcon = "\uE723",
                TextIconFontFamily = new FontFamily("Segoe MDL2 Assets"),
                Content = new TextBlock { Text = "动态新增的标签内容", Margin = new Thickness(12) }
            };
            BrowserTab.Items.Add(item);
            BrowserTab.SelectedItem = item;
            SetStatus("已新增标签：" + item.Header);
        }

        private void BrowserTab_CloseTab(object sender, TabCloseRoutedEventArgs e)
        {
            // 不设置 e.Handled，UI4Tab 会自行移除该标签。
            SetStatus("关闭标签：" + (e.TabItem == null ? "(null)" : e.TabItem.Header));
        }

        // ---------- 对话框 ----------

        private void MsgOk_Click(object sender, RoutedEventArgs e)
        {
            bool? r = UI4MessageBox.Show("这是 UI4MessageBox 的内容。", "提示", UI4MessageBoxButtons.OK, owner: this);
            MsgResult.Text = "返回值：" + Format(r);
        }

        private void MsgOkCancel_Click(object sender, RoutedEventArgs e)
        {
            bool? r = UI4MessageBox.Show("确认执行该操作吗？", "请确认", UI4MessageBoxButtons.OKCancel, owner: this);
            MsgResult.Text = "返回值：" + Format(r);
        }

        private static string Format(bool? value)
        {
            if (!value.HasValue) return "null（被关闭）";
            return value.Value ? "true" : "false";
        }

        private void PickColor_Click(object sender, RoutedEventArgs e)
        {
            var currentBrush = ColorSwatch.Background as SolidColorBrush;
            Color initialColor = currentBrush != null ? currentBrush.Color : Colors.Blue;
            Color? result = UI4ColorPicker.ShowDialog("选择颜色", initialColor, this);

            if (result.HasValue)
            {
                ColorSwatch.Background = new SolidColorBrush(result.Value);
                ColorText.Text = result.Value.ToString();
                SetStatus("UI4ColorPicker 返回：" + result.Value);
            }
            else
            {
                SetStatus("UI4ColorPicker 已取消");
            }
        }

        // ---------- 局部主题（UI4ThemeScope） ----------

        private void ScopeKeyCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // XAML 解析阶段 SelectedIndex="0" 会先触发一次，此时 ScopeCard 字段尚未赋值。
            if (ScopeCard == null || ScopeKeyCombo == null) return;

            ComboBoxItem item = ScopeKeyCombo.SelectedItem as ComboBoxItem;
            string key = item == null ? null : item.Tag as string;
            UI4ThemeScope.SetTheme(ScopeCard, key);
            UpdateScopeStatus();
        }

        private void OpenScopeWindow_Click(object sender, RoutedEventArgs e)
        {
            ScopeWindow window = new ScopeWindow { Owner = this };
            window.Show();
            SetStatus("已打开异主题窗口（与全局主题相反）");
        }

        private void HighContrast_Click(object sender, RoutedEventArgs e)
        {
            UI4Theme.Apply("highcontrast");
            SetStatus("全局主题 → highcontrast");
        }

        private void FollowSystem_Click(object sender, RoutedEventArgs e)
        {
            UI4Theme.SetTheme(UI4ThemeMode.System);
            SetStatus("全局主题 → 跟随系统（ResolvedMode = " + UI4Theme.ResolvedMode + "）");
        }

        private void UpdateScopeStatus()
        {
            if (ScopeStatus == null) return;
            string scopeKey = UI4ThemeScope.GetTheme(ScopeCard);
            ScopeStatus.Text = "全局主题 ResolvedMode=" + UI4Theme.ResolvedMode + " Key=" + UI4Theme.ResolvedKey +
                               "　　作用域卡片 Theme=" + (string.IsNullOrEmpty(scopeKey) ? "(none)" : scopeKey);
        }

        // ---------- 菜单 / 托盘 / 多语言 ----------
        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TraySwitch_Toggled(object sender, RoutedEventArgs e)
        {
            TrayIcon.Visibility = TraySwitch.IsOn ? Visibility.Visible : Visibility.Collapsed;

            if (TraySwitch.IsOn)
            {
                BuildTrayMenu();
                SetStatus("托盘图标已启用：右键弹菜单、双击有事件");
            }
            else
            {
                // OpenMenu() 在没有菜单项时直接返回，所以停用时清空掉，重新启用时再建
                TrayIcon.ClearMenuItems();
                _trayMenuBuilt = false;
                SetStatus("托盘图标已停用");
            }
        }

        private bool _trayMenuBuilt;

        private void BuildTrayMenu()
        {
            if (_trayMenuBuilt) return;
            _trayMenuBuilt = true;

            TrayIcon.MenuActivation = PopupActivationMode.RightClick;
            TrayIcon.MenuWidth = 180;

            TrayIcon.AddItem(UI4MenuItemType.Copy,
                () => UI4Clipboard.TrySetTextAsync("来自 StartUI4Demo 托盘菜单的文本",
                    ok => SetStatus(ok ? "托盘菜单：已复制到剪贴板" : "托盘菜单：复制失败")),
                () => true);
            TrayIcon.AddItem(UI4MenuItemType.SelectAll,
                () => SetStatus("托盘菜单：全选（示例动作）"));
        }

        private void TrayIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            TrayLog.Text = "托盘事件：双击 @ " + DateTime.Now.ToString("HH:mm:ss", CultureInfo.CurrentCulture);
        }

        private void TrayIcon_TrayLeftMouseUp(object sender, RoutedEventArgs e)
        {
            TrayLog.Text = "托盘事件：左键单击 @ " + DateTime.Now.ToString("HH:mm:ss", CultureInfo.CurrentCulture);
        }

        private void TrayIcon_TrayRightMouseDown(object sender, RoutedEventArgs e)
        {
            TrayLog.Text = "托盘事件：右键按下 @ " + DateTime.Now.ToString("HH:mm:ss", CultureInfo.CurrentCulture);
        }

        private void LangCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LangSample == null) return;

            ComboBoxItem item = LangCombo.SelectedItem as ComboBoxItem;
            string lang = item == null || item.Tag == null ? "zh-CN" : item.Tag.ToString();
            CultureInfo.CurrentUICulture = new CultureInfo(lang);
            UI4MultiLanguage.Refresh();
            UpdateLangSample();
        }

        private void UpdateLangSample()
        {
            LangSample.Text = string.Format("OK=\"{0}\"  Cancel=\"{1}\"  Notice=\"{2}\"",
                UI4MultiLanguage.Get(UI4LanguageKey.OK),
                UI4MultiLanguage.Get(UI4LanguageKey.Cancel),
                UI4MultiLanguage.Get(UI4LanguageKey.Notice));

            if (LangKeysLine != null)
            {
                var sb = new System.Text.StringBuilder("全部 UI4LanguageKey：");
                foreach (UI4LanguageKey key in Enum.GetValues(typeof(UI4LanguageKey)))
                    sb.Append("  ").Append(key).Append('=').Append(UI4MultiLanguage.Get(key));
                LangKeysLine.Text = sb.ToString();
            }
        }

        // ---------- 剪贴板与原生交互 ----------

        private void ClipProbe_Click(object sender, RoutedEventArgs e)
        {
            bool has = UI4Clipboard.ContainsText();
            ClipProbeResult.Text = has ? "剪贴板里有 Unicode 文本" : "剪贴板没有文本格式";
            SetStatus("UI4Clipboard.ContainsText() = " + has + "（不打开剪贴板，不抢锁）");
        }

        private void ClipCopyFixed_Click(object sender, RoutedEventArgs e)
        {
            string payload = "StartUI4Demo 复制于 " + DateTime.Now.ToString("HH:mm:ss", CultureInfo.CurrentCulture);
            ClipWriteResult.Text = "已发起复制（后台重试中）：" + payload;
            UI4Clipboard.TrySetTextAsync(payload, ok =>
            {
                if (ClipWriteResult != null)
                    ClipWriteResult.Text = (ok ? "复制成功：" : "复制失败（剪贴板被占用）：") + payload;
            });
        }

        private void ClipCopySelection_Click(object sender, RoutedEventArgs e)
        {
            if (ClipSource.SelectionLength <= 0)
            {
                ClipWriteResult.Text = "请先在上方输入框里选中一部分文字";
                return;
            }

            int length = ClipSource.SelectionLength;
            UI4Clipboard.TrySetTextAsync(ClipSource.SelectedText, ok =>
            {
                if (ClipWriteResult != null)
                    ClipWriteResult.Text = ok
                        ? "已复制选区 " + length.ToString(CultureInfo.InvariantCulture) + " 字符，可去记事本 Ctrl+V 验证"
                        : "复制失败：剪贴板被其它程序长期占用";
            });
        }

        private void ClipRead_Click(object sender, RoutedEventArgs e)
        {
            if (!UI4Clipboard.ContainsText())
            {
                ClipReadResult.Text = "剪贴板里没有文本格式";
                return;
            }

            UI4Clipboard.TryGetTextAsync(text =>
            {
                if (ClipReadResult == null) return;
                ClipReadResult.Text = text == null
                    ? "读取失败：剪贴板被占用超过重试预算"
                    : "读回 " + text.Length.ToString(CultureInfo.InvariantCulture) + " 字符：" + text;
            });
        }

        private void CtxOpen_Click(object sender, RoutedEventArgs e)
        {
            if (_hostMenu == null) return;
            _hostMenu.Open();
            CtxOpenState.Text = "IsOpen = " + _hostMenu.IsOpen;
        }

        private void CtxClose_Click(object sender, RoutedEventArgs e)
        {
            if (_hostMenu == null) return;
            _hostMenu.Close();
            CtxOpenState.Text = "IsOpen = " + _hostMenu.IsOpen;
        }

        // ---------- 主题 · 强调色 · 持久化 · 标题栏 ----------

        private const string OceanKey = "ocean";
        private bool _suppressThemeCombo;

        private string PersistedPath
        {
            get { return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "demo-theme.json"); }
        }

        private void AccentOcean_Click(object sender, RoutedEventArgs e)
        {
            UI4Theme.SetAccent(Color.FromRgb(0x00, 0x78, 0xD4));
            AccentEcho.Text = "强调色 → #0078D4（AccentDark 由库自动派生）";
        }

        private void AccentOrange_Click(object sender, RoutedEventArgs e)
        {
            UI4Theme.SetAccent(Color.FromRgb(0xE6, 0x78, 0x14));
            AccentEcho.Text = "强调色 → #E67814";
        }

        private void AccentPicker_Click(object sender, RoutedEventArgs e)
        {
            Color? picked = UI4ColorPicker.ShowDialog("选择强调色", UI4Theme.Current.AccentColor, this);
            if (!picked.HasValue) return;

            UI4Theme.SetAccent(picked.Value);
            AccentEcho.Text = "强调色 → " + picked.Value;
        }

        private void AccentReset_Click(object sender, RoutedEventArgs e)
        {
            // SetAccent 改的是当前主题定义，重新注册内置定义即可恢复
            UI4Theme.Register(UI4ThemeDefinition.Light());
            UI4Theme.Register(UI4ThemeDefinition.Dark());
            UI4Theme.Register(UI4ThemeDefinition.HighContrast());
            UI4Theme.SetTheme(UI4Theme.CurrentMode);
            AccentEcho.Text = "已恢复内置主题定义";
        }

        private void RegisterOcean_Click(object sender, RoutedEventArgs e)
        {
            UI4ThemeDefinition def = UI4ThemeDefinition.Light().Clone();
            def.Key = OceanKey;
            def.With(UI4ThemeToken.Accent, Color.FromRgb(0x00, 0x96, 0xAA))
               .With(UI4ThemeToken.AccentEnd, Color.FromRgb(0x00, 0x5A, 0x82))
               .With(UI4ThemeToken.Background, Color.FromRgb(0xEC, 0xF8, 0xFA));
            UI4Theme.Register(def);
            UI4Theme.Apply(OceanKey);
            RefreshThemeKeyCombo();
            ThemeEcho.Text = "已注册并应用自定义主题 \"" + OceanKey + "\"（克隆自 light，改了 3 个令牌）";
        }

        private void RefreshThemeKeyCombo()
        {
            if (ThemeKeyCombo == null) return;

            _suppressThemeCombo = true;
            string current = UI4Theme.ResolvedKey;
            ThemeKeyCombo.Items.Clear();
            foreach (string key in UI4Theme.ThemeKeys)
                ThemeKeyCombo.Items.Add(new ComboBoxItem { Content = key, Tag = key });

            int index = 0;
            foreach (string key in UI4Theme.ThemeKeys)
            {
                if (string.Equals(key, current, StringComparison.OrdinalIgnoreCase)) break;
                index++;
            }
            ThemeKeyCombo.SelectedIndex = index >= 0 ? index : 0;
            _suppressThemeCombo = false;
        }

        private void ThemeKeyCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_suppressThemeCombo || ThemeKeyCombo == null) return;

            var item = ThemeKeyCombo.SelectedItem as ComboBoxItem;
            string key = item == null ? null : item.Tag as string;
            if (string.IsNullOrEmpty(key)) return;

            bool applied = UI4Theme.Apply(key);
            ThemeEcho.Text = (applied ? "已切换全局主题 → " : "切换失败（未注册的键）：") + key;
        }

        private void ThemeSave_Click(object sender, RoutedEventArgs e)
        {
            UI4Theme.Persistence = new JsonThemePersistence(PersistedPath);
            UI4Theme.Save();
            PersistEcho.Text = "已把 " + UI4Theme.CurrentMode + " 写入 " + PersistedPath;
        }

        private void ThemeApplyPersisted_Click(object sender, RoutedEventArgs e)
        {
            UI4Theme.Persistence = new JsonThemePersistence(PersistedPath);
            bool loaded = UI4Theme.ApplyPersisted();
            PersistEcho.Text = loaded
                ? "已从文件恢复主题模式 → " + UI4Theme.CurrentMode
                : "没有可恢复的保存（文件不存在或内容无效）";
        }

        private void ThemeClearPersisted_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (System.IO.File.Exists(PersistedPath)) System.IO.File.Delete(PersistedPath);
                PersistEcho.Text = "已删除 " + PersistedPath;
            }
            catch (System.IO.IOException ex)
            {
                PersistEcho.Text = "删除失败：" + ex.Message;
            }
        }

        private void TitleBarApply_Click(object sender, RoutedEventArgs e)
        {
            bool applied = UI4WindowTitleBar.Apply(this);
            TitleBarEcho.Text = "Apply(本窗口) = " + applied
                + "　SupportsCaptionColors = " + UI4WindowTitleBar.SupportsCaptionColors
                + "　Enabled = " + UI4WindowTitleBar.GetEnabled(this);
        }

        private void TitleBarDisable_Click(object sender, RoutedEventArgs e)
        {
            UI4WindowTitleBar.SetEnabled(this, false);
            TitleBarEcho.Text = "本窗口已豁免标题栏染色（交还系统默认）";
        }

        private void TitleBarEnable_Click(object sender, RoutedEventArgs e)
        {
            UI4WindowTitleBar.SetEnabled(this, true);
            TitleBarEcho.Text = "本窗口恢复跟随主题染色";
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            if (_hostMenu != null) _hostMenu.Detach();
            TrayIcon.Visibility = Visibility.Collapsed;
            TrayIcon.Dispose();
        }
    }

    public class CardItem
    {
        public CardItem(string title, string description, string background, Brush iconColor)
        {
            Title = title;
            Description = description;
            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(background));
            IconColor = iconColor;
        }

        public string Title { get; private set; }
        public string Description { get; private set; }
        public Brush Background { get; private set; }
        public Brush IconColor { get; private set; }
    }
}