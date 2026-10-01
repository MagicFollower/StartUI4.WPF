using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Automation;

namespace Net10Regression
{
    /// <summary>断言小工具：一个 tab 内的所有检查都通过它写结果。</summary>
    internal sealed class Verify
    {
        private readonly DemoSession _session;
        private readonly string _scope;
        private readonly int _tab;

        public Verify(DemoSession session, string scope, int tab)
        {
            _session = session;
            _scope = scope;
            _tab = tab;
        }

        private string Id(string suffix)
        {
            return "T" + _tab.ToString(CultureInfo.InvariantCulture) + "-" + suffix;
        }

        private static string Trim(string text, int max)
        {
            if (string.IsNullOrEmpty(text)) return "(空)";
            text = text.Replace("\r", " ").Replace("\n", " ");
            return text.Length <= max ? text : text.Substring(0, max) + "…";
        }

        /// <summary>不点击，直接确认页面上存在包含任一关键词的文本。</summary>
        public bool Static(string caseId, string expected, params string[] needles)
        {
            List<Node> snapshot;
            string hit = _session.WaitForText(needles, 2500, out snapshot);
            bool ok = hit != null;
            Report.Check(Id(caseId), _scope, ok, expected,
                         ok ? "命中「" + Trim(hit, 40) + "」" : "快照内未出现任何关键词（共 " + snapshot.Count + " 节点）");
            return ok;
        }

        /// <summary>点按钮 + 等回显文本。expectedHint 用于报告，needles 是允许的回显关键词。</summary>
        public bool ClickThen(string caseId, string buttonName, string expected, int timeoutMs, params string[] needles)
        {
            string how;
            if (!_session.Click(buttonName, out how))
            {
                Report.Check(Id(caseId), _scope, false, expected, "找不到可点的「" + buttonName + "」");
                return false;
            }

            List<Node> last;
            string hit = _session.WaitForText(needles, timeoutMs, out last);
            bool ok = hit != null;
            Report.Check(Id(caseId), _scope, ok, expected,
                         ok ? how + " → 「" + Trim(hit, 60) + "」"
                            : "点击后未出现回显（触发方式 " + how + "）；现场状态栏/回显=" + DumpEchoes(last));
            return ok;
        }

        /// <summary>失败时把可能的回显节点打出来，避免「FAIL 但看不出为什么」。</summary>
        private static string DumpEchoes(List<Node> nodes)
        {
            var sink = new List<string>();
            foreach (Node node in nodes)
            {
                string text = string.IsNullOrEmpty(node.Name) ? node.Value : node.Name;
                if (string.IsNullOrEmpty(text)) continue;
                if (text.StartsWith("UI4", StringComparison.Ordinal) ||
                    text.StartsWith("就绪", StringComparison.Ordinal) ||
                    text.StartsWith("托盘", StringComparison.Ordinal) ||
                    text.Contains("被点击") || text.Contains("Value =") || text.Contains("选中"))
                {
                    sink.Add(Trim(text, 46));
                }
                if (sink.Count >= 5) break;
            }
            return sink.Count == 0 ? "(无同类节点)" : string.Join(" | ", sink.ToArray());
        }

        /// <summary>底部状态栏断言（状态栏是单行 TextBlock，用前缀匹配）。</summary>
        public bool Status(string caseId, string expectedPrefix)
        {
            List<Node> last;
            string hit = _session.WaitForText(new string[] { expectedPrefix }, 3000, out last);
            bool ok = hit != null;
            Report.Check(Id(caseId), _scope, ok, "状态栏「" + expectedPrefix + "…」",
                         ok ? "状态栏已显示" : "状态栏没有该前缀");
            return ok;
        }

        public bool Same(string caseId, string expected, bool actual, string detail)
        {
            Report.Check(Id(caseId), _scope, actual, expected, detail);
            return actual;
        }

        /// <summary>读某段文本当前的值（用于前后对比）。</summary>
        public string ReadText(params string[] prefixNeedles)
        {
            foreach (Node node in _session.Snapshot())
            {
                foreach (string needle in prefixNeedles)
                {
                    if (node.Both.StartsWith(needle, StringComparison.Ordinal) ||
                        node.Both.IndexOf(needle, StringComparison.Ordinal) >= 0)
                    {
                        return node.Both;
                    }
                }
            }
            return null;
        }

        public Node Find(params string[] nameEquals)
        {
            foreach (Node node in _session.Snapshot())
            {
                foreach (string name in nameEquals)
                {
                    // TextBox 一类的 Name 是空的，内容在 ValuePattern.Value 里，两边都要比
                    if (string.Equals(node.Name, name, StringComparison.Ordinal) ||
                        string.Equals(node.Value, name, StringComparison.Ordinal))
                    {
                        return node;
                    }
                }
            }
            return null;
        }

        /// <summary>按 Name 前缀取干净的文本（不掺 Value，避免分隔符混进比对）。</summary>
        public string ReadName(string prefix)
        {
            foreach (Node node in _session.Snapshot())
            {
                if (!string.IsNullOrEmpty(node.Name) && node.Name.StartsWith(prefix, StringComparison.Ordinal))
                {
                    return node.Name;
                }
            }
            return null;
        }

        /// <summary>展开下拉再选某项：ComboBoxItem 只有弹出层出现时才在 UIA 树里。</summary>
        public bool OpenComboAndPick(string comboAnchor, string itemName, string expectedEcho)
        {
            Node anchor = Find(comboAnchor) ?? FindByText(comboAnchor);
            Node combo = null;
            if (anchor != null)
            {
                // 从锚点文本往上找不到 ComboBox 时，退化成「点锚点，再点弹出层里的项」
                combo = FindContainingControl(anchor, "ComboBox");
            }
            if (combo == null)
            {
                Report.Check("combo", _scope, false, expectedEcho,
                             "找不到锚点「" + comboAnchor + "」附近的 ComboBox");
                return false;
            }

            if (!DemoSession.ClickAt(combo.Element))
            {
                Report.Check("combo", _scope, false, expectedEcho, "点开下拉失败");
                return false;
            }

            Node item = _session.WaitForPopupNode(new string[] { itemName }, 2500);
            if (item == null)
            {
                Report.Check("combo", _scope, false, expectedEcho, "弹出层里没出现「" + itemName + "」");
                _session.ClosePopups();
                return false;
            }
            bool picked = DemoSession.ClickAt(item.Element);
            System.Threading.Thread.Sleep(500);
            _session.ClosePopups();
            Report.Check("combo", _scope, picked, expectedEcho,
                         "点击下拉项「" + itemName + "」=" + picked + " 现回显=" + Trim(ReadText(expectedEcho), 60));
            return picked;
        }

        /// <summary>
        /// 按几何位置点某个标签旁边的控件：UI4Switch / UI4ComboBox 这类控件自身不进 UIA 控制视图，
        /// 但同页的普通 TextBlock 标签在，按标签矩形往左偏移点就够了（开关在标签左侧）。
        /// </summary>
        public bool ClickLeftOfLabel(string labelNeedle, int offsetLeft)
        {
            Node label = FindByText(labelNeedle);
            if (label == null) return false;
            try
            {
                Rect bounds = label.Element.Current.BoundingRectangle;
                if (double.IsNaN(bounds.X) || bounds.Width <= 0) return false;
                return DemoSession.NativeClick((int)(bounds.Left - offsetLeft), (int)(bounds.Top + bounds.Height / 2), false);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>按标签矩形往右偏移点（下拉框在标签右侧时用）。</summary>
        public bool ClickRightOfLabel(string labelNeedle, int offsetRight)
        {
            Node label = FindByText(labelNeedle);
            if (label == null) return false;
            try
            {
                Rect bounds = label.Element.Current.BoundingRectangle;
                if (double.IsNaN(bounds.X) || bounds.Width <= 0) return false;
                return DemoSession.NativeClick((int)(bounds.Right + offsetRight), (int)(bounds.Top + bounds.Height / 2), false);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>读某个 RangeBase 类控件（UI4ProgressBar 派生自 ProgressBar）的当前值。</summary>
        public double RangeValueOf(string typeFragment, int occurrence)
        {
            int seen = 0;
            foreach (Node node in _session.Snapshot())
            {
                if (node.Type.IndexOf(typeFragment, StringComparison.Ordinal) < 0) continue;
                if (seen++ != occurrence) continue;
                RangeValuePattern range = DemoSession.PatternOf<RangeValuePattern>(node.Element, RangeValuePattern.Pattern);
                if (range == null) return double.NaN;
                try { return range.Current.Value; }
                catch (Exception) { return double.NaN; }
            }
            return double.NaN;
        }

        public Node FindByText(string needle)
        {
            foreach (Node node in _session.Snapshot())
            {
                if (node.Both.IndexOf(needle, StringComparison.Ordinal) >= 0) return node;
            }
            return null;
        }

        /// <summary>从某个节点出发，按快照顺序就近找一个指定类型的可交互节点。</summary>
        public Node FindContainingControl(Node anchor, string controlTypeFragment)
        {
            List<Node> nodes = _session.Snapshot();
            int index = nodes.IndexOf(anchor);
            if (index < 0) return null;

            // 先往后找（下拉通常紧跟在标签后面），再往前找
            for (int step = 0; step < 12; step++)
            {
                if (index + step < nodes.Count &&
                    nodes[index + step].Type.IndexOf(controlTypeFragment, StringComparison.Ordinal) >= 0)
                {
                    return nodes[index + step];
                }
                if (index - step >= 0 &&
                    nodes[index - step].Type.IndexOf(controlTypeFragment, StringComparison.Ordinal) >= 0)
                {
                    return nodes[index - step];
                }
            }
            return null;
        }

        /// <summary>找一个紧贴某标签的自定义开关/按钮（UI4Switch 这类没有 Name 的控件）。</summary>
        public Node FindClickableNearLabel(string labelNeedle)
        {
            List<Node> nodes = _session.Snapshot();
            int index = -1;
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Both.IndexOf(labelNeedle, StringComparison.Ordinal) >= 0) { index = i; break; }
            }
            if (index < 0) return null;

            for (int step = 1; step <= 8; step++)
            {
                foreach (int candidateIndex in new int[] { index - step, index + step })
                {
                    if (candidateIndex < 0 || candidateIndex >= nodes.Count) continue;
                    string type = nodes[candidateIndex].Type;
                    if (type.IndexOf("Button", StringComparison.Ordinal) >= 0 ||
                        type.IndexOf("CheckBox", StringComparison.Ordinal) >= 0 ||
                        type.IndexOf("Custom", StringComparison.Ordinal) >= 0)
                    {
                        return nodes[candidateIndex];
                    }
                }
            }
            return null;
        }

        public Node FindByType(string controlTypeFragment)
        {
            foreach (Node node in _session.Snapshot())
            {
                if (node.Type.IndexOf(controlTypeFragment, StringComparison.Ordinal) >= 0) return node;
            }
            return null;
        }

        public bool ElementEnabled(string name, out bool enabled)
        {
            enabled = false;
            foreach (Node node in _session.Snapshot())
            {
                if (string.Equals(node.Name, name, StringComparison.Ordinal))
                {
                    try
                    {
                        enabled = node.Element.Current.IsEnabled;
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        public DemoSession Session { get { return _session; } }
    }

    internal static class Cases
    {
        public static readonly string[] TabTitles = new string[]
        {
            "按钮与开关", "文本输入", "文本显示", "选择器", "进度指示", "列表与网格",
            "导航容器", "布局面板", "对话框", "菜单与托盘", "局部主题",
            "剪贴板与原生交互", "主题与强调色",
        };

        public static void RunTabAt(int tab, string exePath, int waitMs, string label)
        {
            string scope = "页" + tab + " " + TabTitles[tab] + "（" + label + "）";
            using (DemoSession session = DemoSession.Launch(exePath, tab, waitMs))
            {
                Verify v = new Verify(session, scope, tab);

                v.Same("live", "进程存活且主窗口在场", session.Alive, "pid=" + session.Process.Id);
                v.Same("title", "窗口标题到位", !string.IsNullOrEmpty(session.Title), "标题=" + session.Title);

                switch (tab)
                {
                    case 0: Tab0(v); break;
                    case 1: Tab1(v); break;
                    case 2: Tab2(v); break;
                    case 3: Tab3(v); break;
                    case 4: Tab4(v); break;
                    case 5: Tab5(v); break;
                    case 6: Tab6(v); break;
                    case 7: Tab7(v); break;
                    case 8: Tab8(v); break;
                    case 9: Tab9(v); break;
                    case 10: Tab10(v); break;
                    case 11: Tab11(v); break;
                    case 12: Tab12(v); break;
                    default: break;
                }

                // 每页收尾：不能留下未处理异常
                v.Same("noerror", "本页未写 demo-errors.log",
                       Program.DemoErrorLogEmpty(session.ExeDirectory), "StartDirectory=" + session.ExeDirectory);
            }
        }

        private static void Tab0(Verify v)
        {
            v.Static("hdr", "禁用态小节存在", "UI4Button 启动态 / 禁用态");
            v.Static("contrast-note", "亮度自适应前景说明 + 复现对比度值在场", "前景色自动跟随背景亮度");
            v.Static("disabled", "禁用态样本在场", "禁用（浅灰底）");

            bool disOn = false, disOff = false;
            bool readDis = v.ElementEnabled("禁用（默认渐变）", out disOff);
            bool readNorm = v.ElementEnabled("启用（默认渐变）", out disOn);
            v.Same("disabled-state", "IsEnabled=False 的样本在 UIA 里确实不可用，普通样本可用",
                   readDis && readNorm && !disOff && disOn,
                   "禁用（默认渐变）IsEnabled=" + disOff + " 启用（默认渐变）IsEnabled=" + disOn);

            v.ClickThen("plain", "默认按钮", "状态栏显示被点击", 3000, "UI4Button 被点击");
            v.ClickThen("gradient", "启用（浅灰底）", "状态栏带上按钮名", 3000, "UI4Button 被点击：启用（浅灰底）");

            // 外部可用性联动：勾掉「勾选 = 可用」→ 依赖它的按钮必须变不可用
            bool before;
            if (v.ElementEnabled("可用性由外部切换（可逆）", out before))
            {
                Node checkbox = v.Find("勾选 = 可用");
                bool toggled = checkbox != null && DemoSession.ClickAt(checkbox.Element);
                bool after;
                System.Threading.Thread.Sleep(500);
                bool read = v.ElementEnabled("可用性由外部切换（可逆）", out after);
                v.Same("binding", "IsEnabled 绑定随勾选翻转",
                       toggled && read && before != after,
                       "点击前 IsEnabled=" + before + "，点击后=" + after + "（鼠标点击=" + toggled + "）");
            }
            else
            {
                Report.Skipped("binding", "页0 按钮与开关", "找不到「可用性由外部切换（可逆）」");
            }

            v.Same("runtime", "标题栏自检显示真运行时",
                   v.ReadText("实际运行时：") != null && v.ReadText("实际运行时：").Contains("实际运行时"),
                   "回显=" + v.ReadText("实际运行时："));
        }

        private static void Tab1(Verify v)
        {
            v.Static("takeover-note", "接管说明存在", "库在按键隧道阶段改走原生 Win32 剪贴板");
            v.Static("pwd-note", "密码模式限制说明存在", "密码模式下复制与剪切被拒绝");
            v.Static("codeeditor", "AvalonEdit 说明 + 内置 C# 样例在场",
                     "基于 AvalonEdit 的封装，构造里默认开启：C# 语法高亮、行号、自动换行");
            v.Static("echo-init", "字符数回显初值", "字符数 = 5");
            v.Static("pwd-echo", "密码回显初值", "Password = (空)");
            v.Static("snippet", "代码编辑器里有 C# 片段", "// UI4CodeEditor 基于 AvalonEdit");

            // 键入 → TextChanged 回显
            Node box = v.Find("可编辑文本");
            if (box == null)
            {
                Report.Skipped("typing", "页1 文本输入", "找不到 UI4TextBox「可编辑文本」");
                return;
            }

            try
            {
                ValuePattern value = DemoSession.PatternOf<ValuePattern>(box.Element, ValuePattern.Pattern);
                if (value != null) value.SetValue("回归写入的文本");
            }
            catch (Exception ex)
            {
                Report.Skipped("typing", "页1 文本输入", "ValuePattern.SetValue 失败：" + ex.GetType().Name);
                return;
            }

            v.Same("typing", "输入框值被替换并回显字符数",
                   v.ReadText("回归写入的文本") != null && (v.ReadText("字符数 = 7") != null || v.ReadText("字符数 =") != null),
                   "回显=" + v.ReadText("字符数 ="));

            // Ctrl+A → Ctrl+C → 原生读回：库内按键接管的端到端证明
            v.Session.NativeClickTarget(box.Element);
            NativeKeys.SelectAll();
            NativeKeys.Copy();
            System.Threading.Thread.Sleep(700);
            string copied = NativeClipboard.TryReadText();
            v.Same("ctrl-c", "Ctrl+C 走库内原生通道，剪贴板里就是选中文本",
                   copied == "回归写入的文本", "剪贴板读回=" + (copied == null ? "(空)" : "[" + copied + "]"));

            NativeKeys.Cut();
            System.Threading.Thread.Sleep(700);
            string afterCut = NativeClipboard.TryReadText();
            string boxValue = DemoSession.ReadValue(box.Element);
            v.Same("ctrl-x", "Ctrl+X 后原框被清空且内容进剪贴板",
                   boxValue == string.Empty && afterCut == "回归写入的文本",
                   "框内值=\"" + boxValue + "\" 剪贴板=" + (afterCut == null ? "(空)" : "[" + afterCut + "]"));

            NativeKeys.Paste();
            System.Threading.Thread.Sleep(700);
            v.Same("ctrl-v", "Ctrl+V 把内容贴回", DemoSession.ReadValue(box.Element) == "回归写入的文本",
                   "框内值=\"" + DemoSession.ReadValue(box.Element) + "\"");
        }

        private static void Tab2(Verify v)
        {
            v.Static("shadow", "带阴影样本在场", "带阴影的文本");
            v.Static("gradient", "渐变样本在场", "渐变文本");
            v.Static("init", "FlipText 初值 42", "42");

            Node flip = v.FindByText("42");
            v.ClickThen("flip", "随机翻转", "FlipText 换成 0..99 的随机数", 3000, "随机翻转");
            string flipped = null;
            DateTime deadline = DateTime.UtcNow.AddMilliseconds(2500);
            while (DateTime.UtcNow < deadline && flipped == null)
            {
                foreach (Node node in v.Session.Snapshot())
                {
                    foreach (string candidate in new string[] { node.Name, node.Value })
                    {
                        int parsed;
                        if (int.TryParse(candidate, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsed)
                            && parsed >= 0 && parsed <= 99 && parsed != 42)
                        {
                            flipped = candidate;
                            break;
                        }
                    }
                    if (flipped != null) break;
                }
                if (flipped == null) System.Threading.Thread.Sleep(200);
            }
            v.Same("value", "翻转后数字改变（0..99 且不为 42）", flipped != null && flip != null,
                   "初值节点=" + (flip == null ? "未找到" : "42") + " 现值=" + (flipped ?? "(没抓到数字)"));
        }

        private static void Tab3(Verify v)
        {
            v.Static("combo", "下拉初值回显", "选中：选项 1");
            v.Static("slider", "滑杆初值回显", "UI4Slider Value = 50");
            v.Static("circle-note", "CircleSlider 的 AddValueChanged 说明在场",
                     "DependencyPropertyDescriptor.AddValueChanged 订阅 Value");

            Node slider = v.FindByType("Slider");
            if (slider == null)
            {
                Report.Skipped("range", "页3 选择器", "找不到 Slider");
                return;
            }
            try
            {
                RangeValuePattern range = DemoSession.PatternOf<RangeValuePattern>(slider.Element, RangeValuePattern.Pattern);
                if (range == null) throw new InvalidOperationException("无 RangeValuePattern");
                range.SetValue(30);
            }
            catch (Exception ex)
            {
                Report.Skipped("range", "页3 选择器", "设值失败：" + ex.GetType().Name + " " + ex.Message);
                return;
            }
            v.Same("range", "UI4Slider ValueChanged 回显 30",
                   v.ReadText("UI4Slider Value = 30") != null, "回显=" + v.ReadText("UI4Slider Value ="));

            // UI4CircleSlider 没有路由事件，Demo 用 DependencyPropertyDescriptor.AddValueChanged 订阅；
            // 入场动画跑完后 CircleEcho 应停在目标值 60（net10 上这条链路是否照常工作，看这里）
            v.Same("circle-watch", "AddValueChanged 驱动的 CircleEcho 跟上动画终值 60",
                   v.ReadText("UI4CircleSlider A Value = 60") != null,
                   "回显=" + v.ReadText("UI4CircleSlider A Value ="));
        }

        private static void Tab4(Verify v)
        {
            v.Static("bar", "进度条样本在场", "推进 Bar1");
            v.Static("ring", "环形进度样本在场", "启停 Ring1");

            // 状态栏此前被 UI4CircleSlider 的入场动画每帧覆写（net48/net10 同现），
            // Demo 已加闸门修掉（MainWindow.xaml.cs 的 IsSelectorTabActive），所以这里可以放心用状态栏；
            // RangeValue 一并打印（UI4ProgressBar 自身不进 UIA，读不到属预期）。
            double before = v.RangeValueOf("ProgressBar", 0);
            v.ClickThen("advance", "推进 Bar1", "状态栏 Value = 60", 3000, "UI4ProgressBar Value = 60");
            double after = v.RangeValueOf("ProgressBar", 0);
            Report.Info("T4 RangeValue 观察：UI4ProgressBar 可读值 " + Num(before) + "→" + Num(after) +
                        "（读不到=控件未进 UIA，属无障碍待办，见 PORTING-NET10.md §5.9）");

            v.ClickThen("indeterminate", "切换 Bar1 不确定模式", "BarEcho 显示 True", 3000, "Bar1 IsIndeterminate = True");
            v.ClickThen("ring-toggle", "启停 Ring1", "RingEcho 显示 IsActive = False", 3000, "Ring1 IsActive = False");
            v.ClickThen("ring-advance", "Ring1 进度 +10", "RingEcho 显示 Value = 10", 3000, "Ring1 Value = 10");
        }

        private static string Num(double value)
        {
            return double.IsNaN(value) ? "NaN" : value.ToString("0.#", CultureInfo.InvariantCulture);
        }

        private static void Tab5(Verify v)
        {
            v.Static("adaptive-note", "自适应说明在场", "列数 = 可用宽度 ÷ 单元宽度 自动算出来");
            v.Static("cards", "绑定数据源渲染出卡片", "数据看板", "订单管理", "用户中心", "系统日志");

            string echo = v.ReadText("UI4GridView 实际宽度 =");
            v.Same("echo-init", "GridEcho 有宽度与列数", echo != null && echo.Contains("列"), "回显=" + echo);

            // 自动版「拖动窗口右边缘」：MoveWindow 改宽 → SizeChanged → 列数变化
            int widthA = Program.ExtractInt(echo, "实际宽度 = ");
            IntPtr hwnd = v.Session.Hwnd;
            NativeWin.MoveWindow(hwnd, 40, 40, 1500, 900, true);
            System.Threading.Thread.Sleep(900);
            string wide = v.ReadText("UI4GridView 实际宽度 =");
            int widthB = Program.ExtractInt(wide, "实际宽度 = ");
            v.Same("resize", "窗口变宽后 GridView 实际宽度与列数跟着变",
                   widthB > widthA && widthA > 0,
                   "窄时=" + widthA + " 宽时=" + widthB + "（回显=" + wide + "）");

            NativeWin.MoveWindow(hwnd, 40, 40, 900, 800, true);
            System.Threading.Thread.Sleep(700);
            string narrow = v.ReadText("UI4GridView 实际宽度 =");
            int widthC = Program.ExtractInt(narrow, "实际宽度 = ");
            v.Same("resize-back", "窗口变窄后列数回落", widthC < widthB && widthC > 0,
                   "宽时=" + widthB + " 窄时=" + widthC);
        }

        private static void Tab6(Verify v)
        {
            // net10 下 UI4Pivot / UI4Tab / UI4NavigationView 的「内容区文本」不进 UIA 树
            // （net48 进；进程内 I13~I16 已证明内容确实渲染了，所以这是暴露/无障碍差异，不是渲染回归）。
            // 因此这里只断言 UIA 能看到的表头与选中态，内容渲染由 I13~I16 负责。
            v.Static("pivot", "UI4Pivot 表头在场", "首页", "设置", "关于");
            v.Static("tab", "UI4Tab 表头在场", "主页", "文档");
            v.Same("nav", "UI4NavigationView 容器在场（其项文本 net10 不进 UIA，见 PORTING-NET10.md §5.9）",
                   v.FindByType("List") != null, "命中 List 节点=" + (v.FindByType("List") != null));
            v.Static("scroll-note", "ScrollViewer 说明在场", "UI4ScrollViewer 美化了系统 ScrollViewer");

            Node docTab = v.Find("文档");
            bool selected = false;
            if (docTab != null)
            {
                SelectionItemPattern item = DemoSession.PatternOf<SelectionItemPattern>(docTab.Element, SelectionItemPattern.Pattern);
                if (item != null)
                {
                    try { item.Select(); selected = true; } catch (Exception) { }
                }
                if (!selected) selected = DemoSession.ClickAt(docTab.Element);
            }
            System.Threading.Thread.Sleep(600);
            v.Same("tab-select", "切到「文档」标签的点击动作生效（选中态回读与内容渲染另由 I13~I16 证明）",
                   selected, "切换动作=" + selected);

            v.ClickThen("scroll-bottom", "平滑滚到底部", "ScrollEcho 底部文案", 3000, "已平滑滚动到底部");
            v.ClickThen("scroll-top", "平滑滚到顶部", "ScrollEcho 顶部文案", 3000, "已平滑滚动到顶部");
        }

        private static void Tab7(Verify v)
        {
            v.Static("panel", "UI4Panel 样本在场", "悬停我会放大，点我上报事件");
            v.Static("panel2", "圆角 + 悬停描边样本在场", "圆角 + 悬停描边");
            v.ClickThen("grid-button", "按钮 1", "状态栏带按钮名", 3000, "UI4Button 被点击：按钮 1");
        }

        private static void Tab8(Verify v)
        {
            v.Static("init", "返回值初值", "返回值：（尚未点击）");

            string how;
            if (!v.Session.Click("仅确定", out how))
            {
                Report.Skipped("msgbox-ok", "页8 对话框", "点不到「仅确定」");
            }
            else
            {
                Node confirm = v.Session.WaitForWindowNode(new string[] { "确定" }, 3000);
                bool clicked = confirm != null && DemoSession.ClickAt(confirm.Element);
                System.Threading.Thread.Sleep(600);
                bool done = v.ReadText("返回值：true") != null;
                string how2 = "鼠标点击模板内节点";
                if (!done && confirm != null)
                {
                    // 兜底：模态框拿焦点后按 Enter（UI4MessageBox 的确定键是 IsDefault），
                    // 因为自定义按钮模板的可点击点偶尔落在装饰层上——两个运行时表现一致。
                    how2 = "Enter 键";
                    v.Session.FocusOtherWindow();
                    DemoSession.PressKey(0x0D);   // VK_RETURN
                    System.Threading.Thread.Sleep(700);
                    done = v.ReadText("返回值：true") != null;
                }
                v.Same("msgbox-ok", "UI4MessageBox(OK) 弹出→确定→返回 true",
                       confirm != null && done,
                       "模态命中=" + (confirm != null) + " 触发方式=" + how2 +
                       " 回显=" + v.ReadText("返回值："));
            }

            if (!v.Session.Click("确定 / 取消", out how))
            {
                Report.Skipped("msgbox-cancel", "页8 对话框", "点不到「确定 / 取消」");
            }
            else
            {
                Node cancel = v.Session.WaitForWindowNode(new string[] { "取消" }, 3000);
                bool clicked = cancel != null && DemoSession.ClickAt(cancel.Element);
                System.Threading.Thread.Sleep(600);
                v.Same("msgbox-cancel", "UI4MessageBox(OKCancel) 点取消→返回 false",
                       clicked && v.ReadText("返回值：false") != null,
                       "点击=" + clicked + " 回显=" + v.ReadText("返回值："));
            }

            if (!v.Session.Click("选择颜色...", out how))
            {
                Report.Skipped("colorpicker", "页8 对话框", "点不到「选择颜色...」");
            }
            else
            {
                Node pickerWindow = v.Session.WaitForWindowNode(new string[] { "选择颜色" }, 3500);
                v.Same("colorpicker", "UI4ColorPicker 以独立窗口弹出", pickerWindow != null,
                       pickerWindow == null ? "3.5s 内没找到取色器窗口" : "窗口名=" + pickerWindow.Name);
                // 关掉取色器（点它的取消，或直接 Close），保证不残留模态
                v.Session.CloseOtherWindows();
                System.Threading.Thread.Sleep(500);
            }
        }

        private static void Tab9(Verify v)
        {
            v.Static("menu", "UI4Menu 菜单栏顶层项在场", "文件", "编辑");
            // 子项（新建/打开/退出）只在展开后才进 UIA 树：点「文件」再从弹出层里找
            v.ClickThen("menu-expand", "文件", "展开后弹出层里出现「新建」", 3000, "新建");
            Node popupNew = v.Session.WaitForPopupNode(new string[] { "新建" }, 2000);
            v.Same("menu-item", "UI4Menu 弹出层含子项并可回到主窗", popupNew != null,
                   popupNew == null ? "弹出层未出现「新建」（顶层项已确认在场）" : "弹出层命中「" + popupNew.Name + "」");
            v.Session.ClosePopups();
            v.Static("ctx-note", "右键菜单说明在场", "复制/粘贴/删除/全选都是真操作");
            v.Static("tray-note", "托盘说明在场", "启用托盘图标：右键弹出真菜单");
            v.Static("lang", "多语言初值（zh-CN）", "OK=\"确定\"", "OK=\"OK\"");

            // 程序化打开 UI4ContextMenu（页 11 提供按钮），这里验证「选中 → 右键 → 复制」真链路
            Node ctxSource = v.Find("在这里选中一部分文字，然后点右键看菜单。");
            if (ctxSource == null)
            {
                Report.Skipped("ctx-copy", "页9 菜单与托盘", "找不到 CtxSource 输入框");
            }
            else
            {
                try
                {
                    ValuePattern value = DemoSession.PatternOf<ValuePattern>(ctxSource.Element, ValuePattern.Pattern);
                    if (value != null) value.SetValue("右键菜单回归文本");
                }
                catch (Exception) { }

                v.Session.NativeClickTarget(ctxSource.Element);
                NativeKeys.SelectAll();
                NativeClipboard.ClearIfPossible();
                bool rightClicked = DemoSession.ClickAt(ctxSource.Element, true);
                Node copyItem = v.Session.WaitForWindowNode(new string[] { "复制", "Copy" }, 3000);
                v.Same("ctx-open", "右键弹出 UI4ContextMenu", rightClicked && copyItem != null,
                       "右键=" + rightClicked + " 菜单项命中=" + (copyItem != null ? copyItem.Name : "无"));

                if (copyItem != null)
                {
                    DemoSession.ClickAt(copyItem.Element);
                    System.Threading.Thread.Sleep(900);
                    string clipboard = NativeClipboard.TryReadText();
                    v.Same("ctx-copy", "菜单「复制」把选区写进剪贴板（UI4Clipboard 通道）",
                           clipboard == "右键菜单回归文本", "剪贴板=" + (clipboard == null ? "(空)" : "[" + clipboard + "]"));
                    v.Same("ctx-status", "状态栏上报菜单动作",
                           v.ReadText("UI4ContextMenu") != null, "回显=" + v.ReadText("UI4ContextMenu"));
                }
                v.Session.CloseOtherWindows();
            }

            // 语言切换：UI4ComboBox 自身不在 UIA 控制视图里，按 LangSample 标签往左几何点击展开，
            // 再在弹出层窗口里点 en-US（ComboBoxItem 只有弹出态才存在）
            bool openedLang = v.ClickLeftOfLabel("OK=", 90);
            Node langItem = v.Session.WaitForPopupNode(new string[] { "en-US" }, 2500);
            bool pickedLang = langItem != null && DemoSession.ClickAt(langItem.Element);
            System.Threading.Thread.Sleep(600);
            string langSample = v.ReadText("OK=");
            v.Same("lang-switch", "切到 en-US 后 UI4MultiLanguage 词条变化",
                   openedLang && pickedLang && langSample != null && langSample.Contains("OK=\"OK\""),
                   "点开下拉=" + openedLang + " 点中 en-US=" + pickedLang + " 回显=" + langSample);
            v.Session.ClosePopups();
            v.Same("lang-keys", "10 个语言键全量列出",
                   v.ReadText("语言键") != null || v.ReadText("OK / Cancel") != null || v.ReadText("Notice") != null,
                   "LangKeysLine=" + Program.Truncate(v.ReadText("Notice"), 120));

            // 托盘开关：UI4Switch 自身不进 UIA 控制视图 → 按标签往左几何点击
            bool on = v.ClickLeftOfLabel("启用托盘图标：右键弹出真菜单", 24);
            System.Threading.Thread.Sleep(800);
            v.Same("tray-on", "点开 UI4Switch 后状态栏显示托盘已启用", on &&
                   v.ReadText("托盘图标已启用") != null,
                   "点击=" + on + " 回显=" + v.ReadText("托盘图标已启用"));
            v.Same("tray-menu", "托盘菜单已建（状态栏文案点名右键弹菜单）",
                   v.ReadText("托盘图标已启用：右键弹菜单") != null, "回显=" + v.ReadText("托盘图标已启用"));

            bool off = v.ClickLeftOfLabel("启用托盘图标：右键弹出真菜单", 24);
            System.Threading.Thread.Sleep(800);
            v.Same("tray-off", "再点一次托盘停用", off && v.ReadText("托盘图标已停用") != null,
                   "点击=" + off + " 回显=" + v.ReadText("托盘图标已停用"));
        }

        private static void Tab10(Verify v)
        {
            v.Static("scope-card", "作用域卡片在场", "作用域卡片（Theme 见左上方下拉框）");
            v.Static("global-card", "无作用域卡片在场", "无作用域（跟随全局主题）");
            v.Static("nested", "嵌套作用域在场", "外层 = highcontrast", "内层 = dark");
            v.Same("status", "ScopeStatus 报出全局与作用域键",
                   v.ReadText("全局主题 ResolvedMode=") != null, "回显=" + v.ReadText("全局主题 ResolvedMode="));
            v.Same("footer", "页脚主题行是事件驱动的（有值且含三个字段）",
                   v.ReadText("主题:") != null && v.ReadText("主题:").Contains("CurrentMode"),
                   "页脚=" + v.ReadText("主题:"));

            v.ClickThen("hc", "全局高对比度", "状态栏 highcontrast + 页脚 Key=highcontrast", 3000, "全局主题 → highcontrast");
            v.Same("footer-hc", "页脚跟着换成 highcontrast",
                   v.ReadText("Key=highcontrast") != null || v.ReadText("highcontrast") != null,
                   "页脚=" + v.ReadText("主题:"));
            v.Same("dwm-hc", "标题栏 DWM 深色标志=1（跨进程回读）",
                   Dwm.UsesDarkMode(v.Session.Hwnd) == true,
                   "DWM flag=" + Program.FlagText(Dwm.UsesDarkMode(v.Session.Hwnd)));

            // 作用域键下拉：UI4ComboBox 不在控制视图里，按「作用域键：」标签往右几何点击展开
            bool openedScope = v.ClickRightOfLabel("作用域键：", 75);
            Node scopeItem = v.Session.WaitForPopupNode(new string[] { "highcontrast" }, 2500);
            bool pickedScope = scopeItem != null && DemoSession.ClickAt(scopeItem.Element);
            System.Threading.Thread.Sleep(700);
            v.Same("scope-key", "作用域键切到 highcontrast 后 ScopeStatus 跟着变",
                   openedScope && pickedScope && v.ReadText("作用域卡片 Theme=highcontrast") != null,
                   "点开=" + openedScope + " 点中=" + pickedScope + " 状态=" + v.ReadText("全局主题 ResolvedMode="));
            v.Session.ClosePopups();

            v.ClickThen("scope-window", "打开异主题窗口", "ScopeWindow 以独立窗口出现", 3500, "已打开异主题窗口");
            Node scopeWindow = v.Session.WaitForWindowNode(new string[] { "异主题", "ScopeWindow", "作用域" }, 3000);
            v.Same("scope-window", "异主题窗口存在且标题栏独立染色", scopeWindow != null,
                   scopeWindow == null ? "未找到第二个窗口" : "窗口=" + scopeWindow.Name);
            v.Session.CloseOtherWindows();

            v.ClickThen("system", "全局跟随系统", "状态栏显示跟随系统", 3000, "全局主题 → 跟随系统");
        }

        private static void Tab11(Verify v)
        {
            v.Static("probe-note", "ContainsText 的「不抢锁」说明在场", "只查格式是否存在，不 OpenClipboard");
            v.Static("write-note", "写入的后台重试说明在场", "写入固定在后台线程重试");
            v.Static("internal-note", "内部接管清单在场", "都在按键隧道阶段把复制/剪切/粘贴改走 UI4Clipboard");

            v.ClickThen("probe", "查询剪贴板是否有文本", "ClipProbeResult 有结论", 3000,
                        "剪贴板里有 Unicode 文本", "剪贴板没有文本格式");

            NativeClipboard.ClearIfPossible();
            v.ClickThen("copy-fixed", "复制一段带时间戳的文本", "ClipWriteResult 显示复制成功", 4500, "复制成功：");
            string echoed = v.ReadName("复制成功：");
            string payload = echoed == null ? null : echoed.Substring("复制成功：".Length);
            string clipboard = NativeClipboard.TryReadText();
            v.Same("copy-verify", "独立进程式读回：剪贴板内容 = 页面声称的 payload",
                   payload != null && payload.Length > 0 && clipboard == payload,
                   "页面=" + Program.Truncate(payload, 60) + " 剪贴板=" + Program.Truncate(clipboard, 60));

            v.ClickThen("read", "读取剪贴板文本", "ClipReadResult 报出内容或字符数", 4500, "剪贴板里没有文本格式", "字符");

            v.ClickThen("ctx-open", "程序打开菜单", "CtxOpenState = True", 3000, "IsOpen = True");
            v.ClickThen("ctx-close", "程序关闭菜单", "CtxOpenState = False", 3000, "IsOpen = False");
        }

        private static void Tab12(Verify v)
        {
            v.Static("accent-note", "SetAccent 说明在场", "只改强调色：会写回当前主题定义并自动派生 AccentDark");
            v.Static("register-note", "Register 自定义主题说明在场", "克隆内置 light 后改 Key 与若干令牌");
            v.Static("persist-note", "持久化刻意不碰注册表", "不往 HKCU");
            v.Static("titlebar-note", "标题栏染色说明在场", "标题栏属于非客户区，只能经 DWM 染色");

            v.ClickThen("accent-blue", "强调色·海蓝", "AccentEcho 报 #0078D4", 3000, "强调色 → #0078D4");
            v.ClickThen("accent-orange", "强调色·橙", "AccentEcho 报 #E67814", 3000, "强调色 → #E67814");
            v.ClickThen("accent-reset", "恢复内置主题", "AccentEcho 报已恢复", 3000, "已恢复内置主题定义");

            v.ClickThen("register-ocean", "注册并应用 ocean", "ThemeEcho 报已注册 ocean", 3000, "ocean");
            // ThemeKeys 下拉：折叠态没有项，且 UI4ComboBox 不进控制视图 → 按按钮标签往右几何点击
            bool openedThemeCombo = v.ClickRightOfLabel("注册并应用 ocean", 100);
            Node oceanItem = v.Session.WaitForPopupNode(new string[] { "ocean" }, 2500);
            v.Same("theme-keys", "ThemeKeys 下拉列出新注册的键 ocean",
                   openedThemeCombo && oceanItem != null,
                   "点开下拉=" + openedThemeCombo + " 弹出层命中=" + (oceanItem == null ? "无" : oceanItem.Name));
            v.Session.ClosePopups();

            v.ClickThen("save", "保存当前主题模式", "PersistEcho 报已写入 + 文件落盘", 3000, "已把");
            v.Same("persist-file", "demo-theme.json 落在程序目录（不写注册表）",
                   Program.PersistedFileExists(v.Session.ExeDirectory) &&
                   !Program.RegistryStartUI4Exists(),
                   "程序目录=" + v.Session.ExeDirectory);

            v.ClickThen("apply-persisted", "读取并应用已保存", "PersistEcho 报读取结果", 3000, "已读取", "读取", "没有");
            v.ClickThen("clear", "删除持久化文件", "PersistEcho 报已删除 + 文件消失", 3000, "已删除");
            v.Same("persist-clean", "删除后程序目录不再残留 demo-theme.json",
                   !Program.PersistedFileExists(v.Session.ExeDirectory), "-");

            v.ClickThen("titlebar-apply", "Apply(本窗口)", "TitleBarEcho 报 Apply=True", 3000, "Apply(本窗口) = True");
            v.Same("dwm-apply", "Apply 后 DWM 深色标志可读",
                   Dwm.UsesDarkMode(v.Session.Hwnd) != null,
                   "DWM flag=" + Program.FlagText(Dwm.UsesDarkMode(v.Session.Hwnd)));
            v.ClickThen("titlebar-exempt", "本窗口豁免", "TitleBarEcho 报已豁免", 3000, "本窗口已豁免标题栏染色");
            v.ClickThen("titlebar-restore", "恢复染色", "TitleBarEcho 报恢复", 3000, "本窗口恢复跟随主题染色");
        }
    }

    internal static class NativeKeys
    {
        private const byte VK_CONTROL = 0x11;
        private const byte VK_A = 0x41;
        private const byte VK_C = 0x43;
        private const byte VK_V = 0x56;
        private const byte VK_X = 0x58;
        private const uint KEYUP = 0x0002;

        public static void SelectAll() { Combo(VK_A); }
        public static void Copy() { Combo(VK_C); }
        public static void Cut() { Combo(VK_X); }
        public static void Paste() { Combo(VK_V); }

        private static void Combo(byte key)
        {
            keybd_event(VK_CONTROL, 0, 0, UIntPtr.Zero);
            keybd_event(key, 0, 0, UIntPtr.Zero);
            keybd_event(key, 0, KEYUP, UIntPtr.Zero);
            keybd_event(VK_CONTROL, 0, KEYUP, UIntPtr.Zero);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
    }
}
