using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Win32;
using StartUI4Controls;

namespace Net10Regression
{
    /// <summary>
    /// 进程内 STA 断言：色值 / 对比度 / 作用域 / 主题引擎 / 剪贴板通道 / 多语言 / 持久化。
    /// 本机 WPF 抓屏恒返回全白（PORTING.md 第 11、13 节都记录过），所以颜色正确性只能这样在进程内取值证明。
    /// </summary>
    internal static class InProcSuite
    {
        public static void Run()
        {
            Report.Info(string.Empty);
            Report.Info("=== 进程内 STA 断言（直接构造库控件、读解析后的实际颜色）===");

            // 控制台进程默认没有 Application 实例，而库的资源桥是往 Application.Resources 写的
            if (Application.Current == null)
            {
                new Application();
            }
            // 关键：默认 OnLastWindowClose 会在第一个探针窗口 Close 后启动应用关闭流程，
            // 之后 Show() 出来的窗口拿不到 HWND（首轮 I09 的 Apply=False 就是这么来的）
            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            UI4Theme.ApplyToApplication();

            ThemeBridge();
            ThemeSwitching();
            ButtonContrastParity();
            ScopeTheme();
            AccentAndRegister();
            PersistenceParity();
            ClipboardChannel();
            MultiLanguageAll();
            WindowTitleBar();
            NavigationContentRealization();
        }

        // ---------- I10/I11 导航类容器的内容是否真的被实例化 ----------
        private static void NavigationContentRealization()
        {
            UI4Pivot pivot = new UI4Pivot { Width = 420, Height = 160 };
            pivot.Items.Add(new UI4PivotItem { Header = "首页", Content = new TextBlock { Text = "甲内容-pivot" } });
            pivot.Items.Add(new UI4PivotItem { Header = "设置", Content = new TextBlock { Text = "乙内容-pivot" } });

            Window window = OffscreenHost(pivot, 460, 200);
            string selected;
            try
            {
                selected = FindTextInTree(pivot, "甲内容-pivot");
                Report.Check("I10", "导航容器内容",
                             pivot.SelectedIndex == 0 && selected != null,
                             "UI4Pivot 载入后自动选中第 0 项，且其内容被放进 PART_ContentPresenter（视觉树里能找到）",
                             "SelectedIndex=" + pivot.SelectedIndex + " 视觉树命中=" + (selected ?? "无") +
                             "　（ApplyTemplate 再试一次的结果见下）");

                if (selected == null)
                {
                    // 判别：内容缺失是「Loaded 早于模板应用」还是「压根没选中」
                    pivot.ApplyTemplate();
                    Pump(120);
                    string afterApply = FindTextInTree(pivot, "甲内容-pivot");
                    Report.Check("I10b", "导航容器内容", afterApply != null,
                                 "补一次 ApplyTemplate 后内容出现 → 根因是 Loaded 时模板还没应用",
                                 "补模板后命中=" + (afterApply ?? "仍无") + " SelectedIndex=" + pivot.SelectedIndex);
                }
            }
            finally
            {
                window.Close();
            }

            UI4Tab tab = new UI4Tab { Width = 420, Height = 180 };
            tab.Items.Add(new UI4TabItem { Header = "主页", Content = new TextBlock { Text = "甲内容-tab" } });
            tab.Items.Add(new UI4TabItem { Header = "文档", Content = new TextBlock { Text = "乙内容-tab" } });
            Window tabWindow = OffscreenHost(tab, 460, 220);
            try
            {
                string found = FindTextInTree(tab, "甲内容-tab");
                Report.Check("I11", "导航容器内容", tab.SelectedIndex == 0 && found != null,
                             "UI4Tab 同样应选中第 0 页并渲染其内容",
                             "SelectedIndex=" + tab.SelectedIndex + " 视觉树命中=" + (found ?? "无"));
            }
            finally
            {
                tabWindow.Close();
            }

            UI4NavigationView nav = new UI4NavigationView { Width = 420, Height = 260, Header = "导航" };
            UI4NavigationViewItem navItem = new UI4NavigationViewItem { Header = "代码", Content = new TextBlock { Text = "甲内容-nav" } };
            nav.Items.Add(navItem);
            nav.Items.Add(new UI4NavigationViewItem { Header = "属性", Content = new TextBlock { Text = "乙内容-nav" } });
            Window navWindow = OffscreenHost(nav, 460, 300);
            try
            {
                string found = FindTextInTree(nav, "甲内容-nav");
                Report.Check("I12", "导航容器内容",
                             found != null || nav.SelectedItem != null,
                             "UI4NavigationView 选中项内容可见（或至少有选中项）",
                             "SelectedItem=" + (nav.SelectedItem == null ? "null" : nav.SelectedItem.Header) +
                             " 视觉树命中=" + (found ?? "无"));
            }
            finally
            {
                navWindow.Close();
            }

            HostedInsideTabControl();
        }

        /// <summary>
        /// 复刻 Demo 第 6 页的真实嵌套：容器控件放在宿主 TabControl 的某个 TabItem 里再选中该页。
        /// Demo 里 UI4Pivot / UI4Tab / UI4NavigationView 的内容在 net10 上不显示（UIA 原始视图里
        /// 连文本节点都没有），而直接承载时是好的——差别就在这层嵌套引起的 Loaded / OnApplyTemplate 顺序。
        /// </summary>
        private static void HostedInsideTabControl()
        {
            UI4Pivot pivot = new UI4Pivot { Width = 420, Height = 160 };
            pivot.Items.Add(new UI4PivotItem { Header = "首页", Content = new TextBlock { Text = "甲内容-pivot-嵌套" } });
            pivot.Items.Add(new UI4PivotItem { Header = "设置", Content = new TextBlock { Text = "乙内容-pivot-嵌套" } });

            UI4Tab tab = new UI4Tab { Width = 420, Height = 160 };
            tab.Items.Add(new UI4TabItem { Header = "主页", Content = new TextBlock { Text = "甲内容-tab-嵌套" } });

            UI4NavigationView nav = new UI4NavigationView { Width = 420, Height = 220, Header = "导航" };
            nav.Items.Add(new UI4NavigationViewItem { Header = "代码", Content = new TextBlock { Text = "甲内容-nav-嵌套" } });

            StackPanel page = new StackPanel();
            page.Children.Add(pivot);
            page.Children.Add(tab);
            page.Children.Add(nav);

            TabControl host = new TabControl();
            host.Items.Add(new TabItem { Header = "第一页", Content = new TextBlock { Text = "占位" } });
            host.Items.Add(new TabItem { Header = "导航容器", Content = page });

            Window window = OffscreenHost(host, 620, 700);
            try
            {
                host.SelectedIndex = 1;                    // 相当于 Demo 的 --tab=6
                Pump(200);
                window.UpdateLayout();
                Pump(200);

                string pivotText = FindTextInTree(pivot, "甲内容-pivot-嵌套");
                string tabText = FindTextInTree(tab, "甲内容-tab-嵌套");
                string navText = FindTextInTree(nav, "甲内容-nav-嵌套");

                Report.Check("I13", "嵌套后的内容 realization",
                             pivot.SelectedIndex == 0 && pivotText != null,
                             "宿主 TabItem 里显示的 UI4Pivot 应选中第 0 页并渲染内容",
                             "SelectedIndex=" + pivot.SelectedIndex + " 视觉树命中=" + (pivotText ?? "无"));
                Report.Check("I14", "嵌套后的内容 realization",
                             tab.SelectedIndex == 0 && tabText != null,
                             "同样嵌套下的 UI4Tab 应渲染选中页内容",
                             "SelectedIndex=" + tab.SelectedIndex + " 视觉树命中=" + (tabText ?? "无"));
                Report.Check("I15", "嵌套后的内容 realization",
                             nav.SelectedItem != null && navText != null,
                             "同样嵌套下的 UI4NavigationView 应渲染选中项内容",
                             "SelectedItem=" + (nav.SelectedItem == null ? "null" : nav.SelectedItem.Header) +
                             " 视觉树命中=" + (navText ?? "无"));
            }
            finally
            {
                window.Close();
            }

            SelectedBeforeShowInsideScrollViewer();
        }

        /// <summary>
        /// 第二组复刻：照上 Demo 的两个细节——① Show 之前就设好 SelectedIndex
        /// （Demo 在构造函数里用 --tab=N 设），② 页面内容包在 ScrollViewer 里。
        /// 用来解释「Live Demo 第 6 页没内容，而 I13~I15 有」。
        /// </summary>
        private static void SelectedBeforeShowInsideScrollViewer()
        {
            UI4Pivot pivot = new UI4Pivot { Width = 420, Height = 160 };
            pivot.Items.Add(new UI4PivotItem { Header = "首页", Content = new TextBlock { Text = "甲内容-pivot-复刻" } });
            UI4Tab tab = new UI4Tab { Width = 420, Height = 160 };
            tab.Items.Add(new UI4TabItem { Header = "主页", Content = new TextBlock { Text = "甲内容-tab-复刻" } });

            StackPanel page = new StackPanel { Margin = new Thickness(16) };
            page.Children.Add(pivot);
            page.Children.Add(tab);

            TabControl host = new TabControl();
            host.Items.Add(new TabItem { Header = "第一页", Content = new TextBlock { Text = "占位" } });
            host.Items.Add(new TabItem
            {
                Header = "导航容器",
                Content = new ScrollViewer
                {
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                    Content = page
                }
            });
            host.SelectedIndex = 1;          // 关键差异：Show 之前就选好页

            Window window = new Window
            {
                Content = host,
                Width = 900,
                Height = 700,
                Left = -32000,
                Top = -32000,
                ShowInTaskbar = false,
                WindowStyle = WindowStyle.None
            };
            window.Show();
            Pump(300);
            window.UpdateLayout();
            Pump(300);
            try
            {
                string pivotText = FindTextInTree(pivot, "甲内容-pivot-复刻");
                string tabText = FindTextInTree(tab, "甲内容-tab-复刻");
                Report.Check("I16", "Show 前选页 + ScrollViewer",
                             pivotText != null && tabText != null,
                             "照上 Demo 两个细节后，Pivot / Tab 的内容仍应被渲染",
                             "pivot SelectedIndex=" + pivot.SelectedIndex + " 命中=" + (pivotText ?? "无") +
                             "；tab SelectedIndex=" + tab.SelectedIndex + " 命中=" + (tabText ?? "无"));
            }
            finally
            {
                window.Close();
            }
        }

        /// <summary>在视觉树里找某个文本（既可能是 TextBlock.Text，也可能是 ContentControl 的 Content）。</summary>
        private static string FindTextInTree(DependencyObject root, string needle)
        {
            if (root == null) return null;
            int count = VisualTreeHelper.GetChildrenCount(root);
            for (int i = 0; i < count; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(root, i);
                if (child is System.Windows.Controls.TextBlock text && text.Text == needle) return needle;
                if (child is ContentControl content && content.Content is string s && s == needle) return needle;
                string deeper = FindTextInTree(child, needle);
                if (deeper != null) return deeper;
            }
            return null;
        }

        // ---------- I01 令牌资源桥 ----------
        private static void ThemeBridge()
        {
            object surface = Application.Current.Resources["UI4.Brush.Surface"];
            object background = Application.Current.Resources["UI4.Color.Background"];
            object alias = Application.Current.Resources["UI4.Brush.Text"];
            Program.Check("I01", "资源桥",
                          surface is SolidColorBrush && background is Color && alias != null,
                          "ApplyToApplication 把令牌写进 Application.Resources（Brush/Color + 别名）",
                          "UI4.Brush.Surface=" + (surface == null ? "(null)" : surface.GetType().Name) +
                          " UI4.Color.Background=" + (background == null ? "(null)" : background.ToString()) +
                          " UI4.Brush.Text 存在=" + (alias != null));

            int keys = 0;
            foreach (object key in Application.Current.Resources.Keys)
            {
                if (key is string text && text.StartsWith("UI4.", StringComparison.Ordinal)) keys++;
            }
            Program.Check("I01b", "资源桥", keys >= 60,
                          "至少 30 个 Color + 30 个 Brush 键（另含别名）", "实测 UI4.* 键数=" + keys);
        }

        // ---------- I02 主题切换 ----------
        private static void ThemeSwitching()
        {
            UI4Theme.SetTheme(UI4ThemeMode.Light);
            Color lightAccent = UI4Theme.Current.AccentColor;
            Color lightSurface = UI4Theme.Current.SurfaceColor;

            UI4Theme.SetTheme(UI4ThemeMode.Dark);
            Color darkAccent = UI4Theme.Current.AccentColor;
            Color darkSurface = UI4Theme.Current.SurfaceColor;

            Program.Check("I02", "主题切换",
                          UI4Theme.CurrentMode == UI4ThemeMode.Dark &&
                          UI4Theme.ResolvedMode == UI4ThemeMode.Dark &&
                          darkSurface != lightSurface,
                          "SetTheme(Dark) 后 CurrentMode/ResolvedMode=Dark 且面板底色真的变了",
                          "亮 Surface=" + Hex(lightSurface) + " 暗 Surface=" + Hex(darkSurface) +
                          " 强调 " + Hex(lightAccent) + "→" + Hex(darkAccent));

            bool appliedHighContrast = UI4Theme.Apply("highcontrast");
            Program.Check("I02b", "主题切换",
                          appliedHighContrast && UI4Theme.ResolvedKey == "highcontrast" &&
                          UI4Theme.Current.TextForegroundColor != darkSurface,
                          "Apply(\"highcontrast\") 生效且正文色不是面板色",
                          "返回=" + appliedHighContrast + " ResolvedKey=" + UI4Theme.ResolvedKey +
                          " 正文=" + Hex(UI4Theme.Current.TextForegroundColor) +
                          " 面板=" + Hex(UI4Theme.Current.SurfaceColor));

            int fired = 0;
            UI4Theme.ThemeChanged += delegate { fired++; };
            UI4Theme.SetTheme(UI4ThemeMode.Light);
            PumpOnce();                       // 库把 ThemeChanged 放在同一次 Dispatcher 回调尾部
            UI4Theme.SetTheme(UI4ThemeMode.Dark);
            PumpOnce();
            UI4Theme.SetTheme(UI4ThemeMode.Light);
            PumpOnce();
            Program.Check("I02c", "主题切换", fired >= 2, "ThemeChanged 事件在每次切换后发出", "收到次数=" + fired);
        }

        // ---------- I03 按钮前景/禁用态对比度（net48 实测 3.05 / 10.12 / 10.04）----------
        private static void ButtonContrastParity()
        {
            UI4Theme.SetTheme(UI4ThemeMode.Light);

            // 与 Demo 第 0 页逐字相同的四个样本，对比度目标值取自 net48 时代的手工实测记录
            Color[] gradient = new Color[] { Color.FromRgb(0x00, 0x24, 0xFF), Color.FromRgb(0xB4, 0x00, 0xFF) };
            Color neutral = Color.FromRgb(0xC8, 0xC8, 0xD2);

            UI4Button enabledGradient = new UI4Button
            {
                Content = "启用（默认渐变）",
                Width = 150,
                Height = 36,
                GradientStart = gradient[0],
                GradientEnd = gradient[1]
            };
            UI4Button neutralEnabled = new UI4Button
            {
                Content = "启用（浅灰底）",
                Width = 150,
                Height = 36,
                GradientStart = neutral,
                GradientEnd = neutral
            };

            double cEnabledGradient = ContrastOf(enabledGradient, out string fgA, out string bgA);
            double cNeutral = ContrastOf(neutralEnabled, out string fgC, out string bgC);

            // 禁用态：另建一个实例，先入树渲染，再把 IsEnabled 置 false
            // （模板由 Style 的 IsEnabled 触发器整块替换，必须走一次真实的属性变化）
            UI4Button forDisable = new UI4Button
            {
                Content = "禁用（默认渐变）",
                Width = 150,
                Height = 36,
                GradientStart = gradient[0],
                GradientEnd = gradient[1]
            };
            double cDisabled = ContrastAfterDisable(forDisable, out bool swapped, out string tplBefore, out string tplAfter,
                                                   out string fgB, out string bgB, out string treeB);

            // 断言按「规则」而不是记忆中的比值：
            //   启用深底 → 模板底色就是那两个渐变端点的混合，前景自动取白色；
            //   禁用      → 模板整块换成禁用模板，底色=BorderNormal 令牌，前景按亮度换成正文色；
            //   浅灰底    → 前景自动换成正文色，且对比度过 WCAG AA(4.5)。
            // 比值照实打印，与 README/Demo 里那组数（3.05 / 10.12 / 10.04）是否一致由数据说话。
            Color borderNormal = UI4Theme.Current.BorderNormalColor;
            Color textOn = UI4Theme.Current.TextForegroundColor;
            Color white = Colors.White;

            Report.Check("I03a", "按钮对比度",
                         bgA == Hex(Blend(gradient[0], gradient[1])) && fgA == Hex(white),
                         "启用态：模板底色=渐变两端混合、前景=白（亮度 < 0.45 走白字分支）",
                         "前景=" + fgA + " 底色=" + bgA + " 对比度=" + Num(cEnabledGradient));
            Report.Check("I03b", "按钮对比度",
                         swapped && bgB == Hex(borderNormal) && fgB == Hex(textOn) && cDisabled >= 7.0,
                         "禁用后：模板实例被换、底色=BorderNormal 令牌、前景按亮度换成正文色",
                         "前景=" + fgB + " 底色=" + bgB + " 对比度=" + Num(cDisabled) +
                         "（期望 BorderNormal=" + Hex(borderNormal) + " 正文=" + Hex(textOn) + "）" +
                         " 禁用态视觉树=" + treeB);
            Report.Check("I03c", "按钮对比度",
                         fgC == Hex(textOn) && cNeutral >= 4.5,
                         "浅灰底启用态：前景自动换成正文色且对比度过 WCAG AA",
                         "前景=" + fgC + " 底色=" + bgC + " 对比度=" + Num(cNeutral));
            Report.Info("I03 比值小结：启用深渐变=" + Num(cEnabledGradient) +
                        " 禁用=" + Num(cDisabled) + " 浅灰底=" + Num(cNeutral) +
                        "（Demo 第 0 页文案写的是 3.05 / 10.12 / 10.04）");

            // 把「库默认渐变」这一组也量出来：Demo 文案里的 3.05 说不清是按哪个底算的，
            // 这里给出可复现的两个口径（显式 #0024FF→#B400FF，与库默认 #0078D4→#9333EA）。
            UI4Button libraryDefault = new UI4Button { Content = "库默认渐变", Width = 150, Height = 36 };
            double cLibraryDefault = ContrastOf(libraryDefault, out string fgD, out string bgD);
            Report.Info("I03d 库默认渐变启用态：前景=" + fgD + " 底色=" + bgD + " 对比度=" + Num(cLibraryDefault));
        }

        private static string Num(double value)
        {
            return value.ToString("0.00", CultureInfo.InvariantCulture);
        }

        private static string TemplateKey(Control control)
        {
            if (control.Template == null) return "null";
            return System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(control.Template).ToString();
        }

        /// <summary>入树渲染并读实际前景/底色，算 WCAG 对比度（不改变任何状态）。</summary>
        private static double ContrastOf(UI4Button button, out string foregroundHex, out string backgroundHex)
        {
            Window window = OffscreenHost(button, 200, 60);
            try
            {
                Color foreground = EffectiveForeground(button);
                Color background = TemplateBackground(button);
                foregroundHex = Hex(foreground);
                backgroundHex = Hex(background);
                return Contrast(foreground, background);
            }
            finally
            {
                window.Close();
            }
        }

        private static double ContrastAfterDisable(UI4Button button, out bool swapped, out string before,
                                                  out string after, out string foregroundHex, out string backgroundHex,
                                                  out string tree)
        {
            Window window = OffscreenHost(button, 200, 60);
            try
            {
                before = TemplateKey(button);
                button.IsEnabled = false;
                ForceLayout(button, 200, 60);
                after = TemplateKey(button);
                swapped = before != after;

                Color foreground = EffectiveForeground(button);
                Color background = TemplateBackground(button);
                foregroundHex = Hex(foreground);
                backgroundHex = Hex(background);
                tree = DescribeTree(button, 400);
                return Contrast(foreground, background);
            }
            finally
            {
                window.Close();
            }
        }

        private static Color EffectiveForeground(UI4Button button)
        {
            // 禁用态换的是整块 Template：真正的文字色写在模板内 ContentPresenter 的
            // TextElement.Foreground 上，button.Foreground 仍是主样式的值，所以先读模板里的。
            ContentPresenter presenter = FindVisual<ContentPresenter>(button);
            if (presenter != null)
            {
                Brush fromTemplate = TextElement.GetForeground(presenter);
                if (fromTemplate is SolidColorBrush templateBrush) return templateBrush.Color;
            }

            TextBlock text = FindVisual<TextBlock>(button);
            if (text != null && text.Foreground is SolidColorBrush textBrush) return textBrush.Color;

            if (button.Foreground is SolidColorBrush own) return own.Color;
            return UI4Theme.Current.TextForegroundColor;
        }

        /// <summary>把当前模板的视觉树摘要打出来，供失败时定位（哪个 Border、什么底色）。</summary>
        private static string DescribeTree(DependencyObject root, int budget)
        {
            var builder = new StringBuilder();
            Describe(root, builder, 0, budget);
            return builder.ToString();
        }

        private static void Describe(DependencyObject node, StringBuilder sink, int depth, int budget)
        {
            if (node == null || depth > 8 || sink.Length > budget) return;
            string description = node.GetType().Name;
            if (node is Border border)
            {
                description += "(bg=" + DescribeBrush(border.Background) + ")";
            }
            if (node is ContentPresenter presenter)
            {
                Brush foreground = TextElement.GetForeground(presenter);
                description += "(TextElement.Foreground=" + DescribeBrush(foreground) + ")";
            }
            sink.Append(depth > 0 ? "  " + description : description);

            int count = VisualTreeHelper.GetChildrenCount(node);
            for (int i = 0; i < count; i++)
            {
                sink.Append(" > ");
                Describe(VisualTreeHelper.GetChild(node, i), sink, depth + 1, budget);
            }
        }

        private static string DescribeBrush(Brush brush)
        {
            if (brush == null) return "null";
            SolidColorBrush solid = brush as SolidColorBrush;
            if (solid != null) return Hex(solid.Color);
            LinearGradientBrush gradient = brush as LinearGradientBrush;
            if (gradient != null && gradient.GradientStops.Count > 0)
            {
                return "渐变" + Hex(gradient.GradientStops[0].Color) + "→" +
                       Hex(gradient.GradientStops[gradient.GradientStops.Count - 1].Color);
            }
            return brush.GetType().Name;
        }

        private static Color TemplateBackground(UI4Button button)
        {
            Border border = FindVisual<Border>(button);
            if (border != null)
            {
                SolidColorBrush solid = border.Background as SolidColorBrush;
                if (solid != null) return solid.Color;

                LinearGradientBrush gradient = border.Background as LinearGradientBrush;
                if (gradient != null && gradient.GradientStops.Count > 0)
                {
                    return Blend(gradient.GradientStops[0].Color,
                                 gradient.GradientStops[gradient.GradientStops.Count - 1].Color);
                }
            }

            LinearGradientBrush own = button.Background as LinearGradientBrush;
            if (own != null && own.GradientStops.Count > 0)
            {
                return Blend(own.GradientStops[0].Color, own.GradientStops[own.GradientStops.Count - 1].Color);
            }
            SolidColorBrush ownSolid = button.Background as SolidColorBrush;
            if (ownSolid != null) return ownSolid.Color;
            return UI4Theme.Current.BackgroundColor;
        }

        private static Color Blend(Color a, Color b)
        {
            return Color.FromRgb(
                (byte)((a.R + b.R) / 2),
                (byte)((a.G + b.G) / 2),
                (byte)((a.B + b.B) / 2));
        }

        private static bool Near(double value, double expected, double tolerance)
        {
            return value > 0 && Math.Abs(value - expected) <= tolerance;
        }

        // ---------- I04 局部作用域 ----------
        private static void ScopeTheme()
        {
            UI4Theme.SetTheme(UI4ThemeMode.Light);

            Border scoped = new Border { Width = 220, Height = 80 };
            TextBlock inside = new TextBlock { Text = "inside" };
            inside.SetResourceReference(TextBlock.ForegroundProperty, "UI4.Brush.Text");
            TextBlock outside = new TextBlock { Text = "outside" };
            outside.SetResourceReference(TextBlock.ForegroundProperty, "UI4.Brush.Text");

            StackPanel panel = new StackPanel();
            panel.Children.Add(scoped);
            panel.Children.Add(outside);
            scoped.Child = inside;
            UI4ThemeScope.SetTheme(scoped, "dark");

            Window window = OffscreenHost(panel, 300, 200);
            try
            {
                Color insideColor = ((SolidColorBrush)inside.Foreground).Color;
                Color outsideColor = ((SolidColorBrush)outside.Foreground).Color;
                Program.Check("I04", "局部作用域",
                              insideColor != outsideColor &&
                              UI4ThemeScope.GetTheme(scoped) == "dark",
                              "同一屏内作用域子树用 dark 正文色、外部仍用 light",
                              "作用域键=" + UI4ThemeScope.GetTheme(scoped) +
                              " 内=" + Hex(insideColor) + " 外=" + Hex(outsideColor));
            }
            finally
            {
                window.Close();
            }
        }

        // ---------- I05 强调色与自定义主题 ----------
        private static void AccentAndRegister()
        {
            UI4Theme.SetTheme(UI4ThemeMode.Light);
            Color original = UI4Theme.Current.AccentColor;

            UI4Theme.SetAccent(Color.FromRgb(0xE6, 0x78, 0x14));
            Program.Check("I05", "强调色",
                          UI4Theme.Current.AccentColor == Color.FromRgb(0xE6, 0x78, 0x14),
                          "SetAccent 立刻改到 Current.AccentColor",
                          "原=" + Hex(original) + " 新=" + Hex(UI4Theme.Current.AccentColor) +
                          " 派生 AccentDark=" + Hex(UI4Theme.Current.AccentDarkColor));

            UI4Theme.SetAccent(original);
            Program.Check("I05b", "强调色", UI4Theme.Current.AccentColor == original,
                          "SetAccent 可逆（回到原强调色）", "回读=" + Hex(UI4Theme.Current.AccentColor));

            UI4ThemeDefinition ocean = UI4ThemeDefinition.Light().Clone();
            ocean.Key = "probe-ocean";
            ocean = ocean.With(UI4ThemeToken.Accent, Color.FromRgb(0x00, 0x69, 0x7B));
            UI4Theme.Register(ocean);

            bool listed = false;
            foreach (string key in UI4Theme.ThemeKeys)
            {
                if (key == "probe-ocean") listed = true;
            }
            bool applied = UI4Theme.Apply("probe-ocean");
            Program.Check("I05c", "自定义主题",
                          listed && applied && UI4Theme.ResolvedKey == "probe-ocean" &&
                          UI4Theme.Current.AccentColor == Color.FromRgb(0x00, 0x69, 0x7B),
                          "Register→ThemeKeys→Apply 全链路生效，令牌按定义取值",
                          "在列表=" + listed + " Apply=" + applied + " ResolvedKey=" + UI4Theme.ResolvedKey +
                          " Accent=" + Hex(UI4Theme.Current.AccentColor));

            UI4Theme.SetTheme(UI4ThemeMode.Light);
        }

        // ---------- I06 持久化（不碰注册表）----------
        private static void PersistenceParity()
        {
            string path = Path.Combine(Path.GetTempPath(), "startui4-probe-theme-" + Guid.NewGuid().ToString("N") + ".json");
            UI4Theme.Persistence = new JsonThemePersistence(path);
            try
            {
                UI4Theme.SetTheme(UI4ThemeMode.Dark);
                UI4Theme.Save();
                bool fileWritten = File.Exists(path);

                UI4Theme.SetTheme(UI4ThemeMode.Light);
                bool loaded = UI4Theme.ApplyPersisted();

                Program.Check("I06", "持久化",
                              fileWritten && loaded && UI4Theme.CurrentMode == UI4ThemeMode.Dark,
                              "JsonThemePersistence 写文件→ApplyPersisted 读回 Dark 往返成立",
                              "文件=" + fileWritten + " ApplyPersisted=" + loaded +
                              " CurrentMode=" + UI4Theme.CurrentMode);
                Program.Check("I06b", "持久化", !Program.RegistryStartUI4Exists(),
                              "整个持久化过程没往 HKCU\\Software\\StartUI4 写", "注册表键存在=" +
                              Program.RegistryStartUI4Exists());
            }
            finally
            {
                UI4Theme.Persistence = null;
                UI4Theme.SetTheme(UI4ThemeMode.Light);
                if (File.Exists(path)) File.Delete(path);
            }
        }

        // ---------- I07 库内原生剪贴板通道 ----------
        private static void ClipboardChannel()
        {
            string payload = "startui4-net10-" + Guid.NewGuid().ToString("N").Substring(0, 8);
            NativeClipboard.ClearIfPossible();

            bool? wrote = null;
            ManualResetEventSlim writeDone = new ManualResetEventSlim(false);
            UI4Clipboard.TrySetTextAsync(payload, ok =>
            {
                wrote = ok;
                writeDone.Set();
            });
            bool writeReturned = writeDone.Wait(5000);

            string read = null;
            ManualResetEventSlim readDone = new ManualResetEventSlim(false);
            UI4Clipboard.TryGetTextAsync(text =>
            {
                read = text;
                readDone.Set();
            });
            bool readReturned = readDone.Wait(5000);

            Program.Check("I07", "剪贴板通道",
                          writeReturned && wrote == true && readReturned && read == payload,
                          "TrySetTextAsync / TryGetTextAsync 在 net10 上往返一致（后台线程不等锁）",
                          "写回调=" + (wrote.HasValue ? wrote.Value.ToString() : "(无)") +
                          " 读回=" + (read ?? "(空)"));
            Program.Check("I07b", "剪贴板通道", UI4Clipboard.ContainsText(),
                          "ContainsText 探测到刚写入的 Unicode 文本（不 OpenClipboard）", "-");
        }

        // ---------- I08 八套语言词条 ----------
        private static void MultiLanguageAll()
        {
            string[] cultures = new string[] { "zh-CN", "en-US", "ja-JP", "ko-KR", "de-DE", "fr-FR", "es-ES", "ru-RU" };
            List<string> distinctPairs = new List<string>();
            var builder = new StringBuilder();
            int missing = 0;

            CultureInfo original = CultureInfo.CurrentUICulture;
            try
            {
                foreach (string culture in cultures)
                {
                    CultureInfo.CurrentUICulture = new CultureInfo(culture);
                    UI4MultiLanguage.Refresh();
                    string ok = UI4MultiLanguage.Get(UI4LanguageKey.OK);
                    string cancel = UI4MultiLanguage.Get(UI4LanguageKey.Cancel);
                    if (string.IsNullOrEmpty(ok) || string.IsNullOrEmpty(cancel)) missing++;
                    string pair = ok + "/" + cancel;
                    builder.Append(culture).Append('=').Append(pair).Append("  ");
                    if (!distinctPairs.Contains(pair)) distinctPairs.Add(pair);
                }
            }
            finally
            {
                CultureInfo.CurrentUICulture = original;
                UI4MultiLanguage.Refresh();
            }

            // 注意：OK 这个词在 ja/de/fr/ru 里都写作「OK」，所以按 OK 去重只有 4 种；
            // 真正能区分语言的是 OK+Cancel 这一对，故按组合去重。
            Program.Check("I08", "多语言", missing == 0 && distinctPairs.Count >= 7,
                          "8 套语言的 OK/Cancel 全部非空，且 OK+Cancel 组合至少 7 种互不相同",
                          "缺失=" + missing + " 去重组合=" + distinctPairs.Count + " 明细=" + builder);
        }

        // ---------- I09 标题栏染色 ----------
        private static void WindowTitleBar()
        {
            // 必须用带标题栏且真实上屏的窗口：WindowStyle=None 没有非客户区，
            // 而放在 -32000 的离屏窗口 DWM 不建帧，USE_IMMERSIVE_DARK_MODE 会被拒（hr 0x80000000）。
            // 判别实验：同一份代码，离屏 → Apply=False + 回读失败；上屏 → 见下面实际输出。
            Window window = new Window
            {
                Title = "StartUI4 标题栏探针",
                Width = 240,
                Height = 120,
                Left = 80,
                Top = 80,
                ShowInTaskbar = false,
                WindowStyle = WindowStyle.SingleBorderWindow
            };
            window.Show();
            PumpOnce();
            try
            {
                UI4Theme.SetTheme(UI4ThemeMode.Dark);
                PumpOnce();
                IntPtr hwnd = new WindowInteropHelper(window).Handle;
                bool applied = UI4WindowTitleBar.Apply(window);
                bool? dark = Dwm.UsesDarkMode(hwnd);

                UI4WindowTitleBar.SetEnabled(window, false);
                PumpOnce();
                bool? afterExempt = Dwm.UsesDarkMode(hwnd);

                UI4WindowTitleBar.SetEnabled(window, true);
                PumpOnce();

                Program.Check("I09", "标题栏", applied && hwnd != IntPtr.Zero,
                              "UI4WindowTitleBar.Apply 返回 true（DwmSetWindowAttribute 被系统接受）",
                              "Apply=" + applied + " HWND=" + hwnd +
                              " SupportsCaptionColors=" + UI4WindowTitleBar.SupportsCaptionColors +
                              " 深色标志=" + Program.FlagText(dark) + "（回读 hr=0x" +
                              Dwm.LastHresult.ToString("X8", CultureInfo.InvariantCulture) + "）" +
                              " 裸调 DWM 对照：" + Dwm.RawProbe(hwnd));
                Program.Check("I09b", "标题栏", dark == true,
                              "SetTheme(Dark)+Apply 后跨进程回读到深色标志=1",
                              "回读=" + Program.FlagText(dark) + " 豁免后=" + Program.FlagText(afterExempt));
            }
            finally
            {
                UI4Theme.SetTheme(UI4ThemeMode.Light);
                window.Close();
            }
        }

        // ---------- 通用小工具 ----------
        private static Window OffscreenHost(UIElement content, double width, double height)
        {
            Window window = new Window
            {
                Content = content,
                Width = width,
                Height = height,
                Left = -32000,
                Top = -32000,
                ShowInTaskbar = false,
                WindowStyle = WindowStyle.None
            };
            window.Show();
            ForceLayout(content, width, height);
            return window;
        }

        /// <summary>
        /// 强制把模板实例化出来：Show + UpdateLayout 在离屏窗口上并不会保证视觉树已建，
        /// 而读「模板里实际用的画刷」必须有那棵树（首轮 I03 就是因为树是空的，
        /// FindVisual 拿不到 Border，退回到了控件自身的 Background，测出来的数全是假象）。
        /// </summary>
        private static void ForceLayout(UIElement content, double width, double height)
        {
            Control control = content as Control;
            if (control != null)
            {
                control.ApplyTemplate();
                control.Measure(new Size(width, height));
                control.Arrange(new Rect(0, 0, width, height));
            }
            content.UpdateLayout();
            Pump(40);
        }

        private static void PumpOnce()
        {
            Pump(60);
        }

        /// <summary>跑一个 ms 毫秒的小消息循环：把所有优先级（含库用来发 ThemeChanged 的 Input）都排空。</summary>
        private static void Pump(int ms)
        {
            DispatcherFrame frame = new DispatcherFrame();
            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(ms)
            };
            timer.Tick += delegate
            {
                timer.Stop();
                frame.Continue = false;
            };
            timer.Start();
            Dispatcher.PushFrame(frame);
        }

        private static T FindVisual<T>(DependencyObject root) where T : DependencyObject
        {
            if (root == null) return null;
            int count = VisualTreeHelper.GetChildrenCount(root);
            for (int i = 0; i < count; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(root, i);
                if (child is T match) return match;
                T deeper = FindVisual<T>(child);
                if (deeper != null) return deeper;
            }
            return null;
        }

        private static double Contrast(Color foreground, Color background)
        {
            double l1 = Luminance(foreground);
            double l2 = Luminance(background);
            double lighter = Math.Max(l1, l2);
            double darker = Math.Min(l1, l2);
            return (lighter + 0.05) / (darker + 0.05);
        }

        private static double Luminance(Color color)
        {
            return 0.2126 * Channel(color.R) + 0.7152 * Channel(color.G) + 0.0722 * Channel(color.B);
        }

        private static double Channel(byte value)
        {
            double v = value / 255.0;
            return v <= 0.03928 ? v / 12.92 : Math.Pow((v + 0.055) / 1.055, 2.4);
        }

        private static string Hex(Color color)
        {
            return "#" + color.R.ToString("X2", CultureInfo.InvariantCulture) +
                   color.G.ToString("X2", CultureInfo.InvariantCulture) +
                   color.B.ToString("X2", CultureInfo.InvariantCulture);
        }
    }

    /// <summary>把 WPF Window 的 HWND 取出来（避免 InProcSuite 里到处写 WindowInteropHelper）。</summary>
    internal readonly struct WindowInteropHandle
    {
        public WindowInteropHandle(Window window)
        {
            Handle = new WindowInteropHelper(window).Handle;
        }

        public IntPtr Handle { get; }

        public static implicit operator IntPtr(WindowInteropHandle value)
        {
            return value.Handle;
        }
    }
}
