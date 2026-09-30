using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using StartUI4Controls;

namespace StartUI4Demo
{
    public partial class MainWindow : Window
    {
        private static readonly Random Rng = new Random();
        private UI4ContextMenu _hostMenu;
        private int _tabCounter;

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
            UI4Theme.ThemeChanged += delegate { UpdateScopeStatus(); };
            ApplyTheme(false);
            SetStatus("就绪");
            UpdateScopeStatus();
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
            _hostMenu = new UI4ContextMenu { Width = 190 };

            _hostMenu.AddItem(new UI4MenuItem(
                UI4MenuItemType.Copy, "复制文本", UI4MenuIcons.Copy,
                () => SetStatus("UI4ContextMenu → 复制文本")));
            _hostMenu.AddItem(new UI4MenuItem(
                UI4MenuItemType.Paste, "粘贴", UI4MenuIcons.Paste,
                () => SetStatus("UI4ContextMenu → 粘贴")));
            _hostMenu.AddItem(new UI4MenuItem(
                UI4MenuItemType.Delete, "删除", UI4MenuIcons.Delete,
                () => SetStatus("UI4ContextMenu → 删除")));
            _hostMenu.AddItem(UI4MenuItemType.SelectAll, () => SetStatus("UI4ContextMenu → 全选"));

            _hostMenu.Attach(CtxHost);
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
            SetStatus("托盘图标已" + (TraySwitch.IsOn ? "启用" : "停用"));
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

            string lang = LangCombo.SelectedIndex == 0 ? "zh-CN" : "en-US";
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