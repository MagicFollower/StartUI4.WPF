using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Automation;

namespace Net10Regression
{
    /// <summary>UIA 快照里的一条记录：控件类型 + Name + 可读值。</summary>
    internal sealed class Node
    {
        public string Name;
        public string Type;
        public string Value;
        public AutomationElement Element;

        public string Both
        {
            get { return (Name ?? string.Empty) + "\u0001" + (Value ?? string.Empty); }
        }
    }

    /// <summary>被测 Demo 进程 + UIA 驱动。</summary>
    internal sealed class DemoSession : IDisposable
    {
        private readonly Process _process;
        private AutomationElement _root;

        private DemoSession(Process process)
        {
            _process = process;
        }

        public Process Process { get { return _process; } }
        public IntPtr Hwnd { get { return _process.MainWindowHandle; } }

        /// <summary>被测 exe 所在目录（demo-errors.log / demo-theme.json 都落在这里）。</summary>
        public string ExeDirectory { get; private set; }

        /// <summary>主窗口 HWND，启动时就缓存：Process.MainWindowHandle 会被模态框刷新，不能拿来排除主窗口。</summary>
        private IntPtr _mainHwnd;

        public static DemoSession Launch(string exePath, int tab, int waitMs)
        {
            if (!System.IO.File.Exists(exePath))
            {
                throw new FileNotFoundException("找不到被测 exe：" + exePath);
            }

            ProcessStartInfo startInfo = new ProcessStartInfo(exePath, "--tab=" + tab);
            startInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(exePath);
            startInfo.UseShellExecute = false;

            Process process = Process.Start(startInfo);
            DemoSession session = new DemoSession(process);
            session.ExeDirectory = startInfo.WorkingDirectory;

            DateTime deadline = DateTime.UtcNow.AddMilliseconds(waitMs);
            while (DateTime.UtcNow < deadline)
            {
                process.Refresh();
                if (process.HasExited)
                {
                    throw new InvalidOperationException("被测进程在窗口出现前就退出了，退出码 " +
                                                        SafeExitCode(process));
                }
                if (process.MainWindowHandle != IntPtr.Zero)
                {
                    try
                    {
                        session._root = AutomationElement.FromHandle(process.MainWindowHandle);
                    }
                    catch (Exception)
                    {
                        session._root = null;
                    }
                    if (session._root != null)
                    {
                        session._mainHwnd = session._root.Current.NativeWindowHandle;
                        return session;
                    }
                }
                Thread.Sleep(120);
            }

            throw new InvalidOperationException("等待 " + waitMs + "ms 仍未拿到主窗口");
        }

        private static string SafeExitCode(Process process)
        {
            try { return process.ExitCode.ToString(); }
            catch (Exception) { return "?"; }
        }

        public string Title
        {
            get
            {
                _process.Refresh();
                return _process.MainWindowTitle;
            }
        }

        public bool Alive
        {
            get
            {
                _process.Refresh();
                return !_process.HasExited;
            }
        }

        /// <summary>控件视图全量快照（Depth 足够覆盖 13 页内容）。</summary>
        public List<Node> Snapshot()
        {
            List<Node> nodes = new List<Node>();
            if (_root == null) return nodes;
            Walk(_root, nodes, 0);
            return nodes;
        }

        /// <summary>
        /// 原始视图快照：用来区分「内容根本没渲染」与「渲染了但没进 UIA 控制视图」。
        /// </summary>
        public List<Node> SnapshotRaw()
        {
            List<Node> nodes = new List<Node>();
            if (_root == null) return nodes;
            WalkRaw(_root, nodes, 0);
            return nodes;
        }

        private static void WalkRaw(AutomationElement element, List<Node> sink, int depth)
        {
            if (depth > 60 || sink.Count > 6000) return;
            Node node = new Node();
            try { node.Name = element.Current.Name ?? string.Empty; } catch (Exception) { node.Name = string.Empty; }
            try { node.Type = element.Current.ControlType.ProgrammaticName; } catch (Exception) { node.Type = "?"; }
            node.Element = element;
            sink.Add(node);

            AutomationElement child = TreeWalker.RawViewWalker.GetFirstChild(element);
            while (child != null)
            {
                WalkRaw(child, sink, depth + 1);
                child = TreeWalker.RawViewWalker.GetNextSibling(child);
            }
        }

        private static void Walk(AutomationElement element, List<Node> sink, int depth)
        {
            if (depth > 40 || sink.Count > 4000) return;

            Node node = new Node();
            try { node.Name = element.Current.Name ?? string.Empty; }
            catch (Exception) { node.Name = string.Empty; }
            try { node.Type = element.Current.ControlType.ProgrammaticName; }
            catch (Exception) { node.Type = "?"; }
            try { node.Value = ReadValue(element); }
            catch (Exception) { node.Value = string.Empty; }
            node.Element = element;
            sink.Add(node);

            AutomationElement child = TreeWalker.ControlViewWalker.GetFirstChild(element);
            while (child != null)
            {
                Walk(child, sink, depth + 1);
                child = TreeWalker.ControlViewWalker.GetNextSibling(child);
            }
        }

        /// <summary>UIA 的 TryGetCurrentPattern 只有 out object 形态，统一收口在这里。</summary>
        public static T PatternOf<T>(AutomationElement element, AutomationPattern pattern) where T : class
        {
            object raw;
            try
            {
                if (!element.TryGetCurrentPattern(pattern, out raw)) return null;
            }
            catch (Exception)
            {
                return null;
            }
            return raw as T;
        }

        public static string ReadValue(AutomationElement element)
        {
            ValuePattern value = PatternOf<ValuePattern>(element, ValuePattern.Pattern);
            if (value != null)
            {
                try { return value.Current.Value ?? string.Empty; } catch (Exception) { }
            }
            RangeValuePattern range = PatternOf<RangeValuePattern>(element, RangeValuePattern.Pattern);
            if (range != null)
            {
                try
                {
                    return range.Current.Value.ToString(CultureInfo.InvariantCulture);
                }
                catch (Exception) { }
            }
            TogglePattern toggle = PatternOf<TogglePattern>(element, TogglePattern.Pattern);
            if (toggle != null)
            {
                try { return toggle.Current.ToggleState.ToString(); } catch (Exception) { }
            }
            return string.Empty;
        }

        /// <summary>
        /// 轮询等待快照里出现包含任意一个关键词的节点；返回命中的关键词，超时返回 null。
        /// 轮询间隔压到 25ms：Demo 底部状态栏会被 UI4CircleSlider 的动画回调持续覆写
        /// （net48 与 net10 同现，见 PORTING-NET10.md §5.7），慢轮询会把真实发生过的回显漏掉。
        /// </summary>
        public string WaitForText(string[] needles, int timeoutMs, out List<Node> last)
        {
            DateTime deadline = DateTime.UtcNow.AddMilliseconds(timeoutMs);
            last = new List<Node>();
            while (DateTime.UtcNow < deadline)
            {
                last = Snapshot();
                foreach (Node node in last)
                {
                    foreach (string needle in needles)
                    {
                        if (node.Both.IndexOf(needle, StringComparison.Ordinal) >= 0)
                        {
                            return needle;
                        }
                    }
                }
                Thread.Sleep(25);
            }
            return null;
        }

        public Node FindButton(string name)
        {
            foreach (Node node in Snapshot())
            {
                // UI4MenuElementItem 派生自 MenuItem，控制类型是 MenuItem 而不是 Button
                if ((node.Type.IndexOf("Button", StringComparison.Ordinal) >= 0 ||
                     node.Type.IndexOf("MenuItem", StringComparison.Ordinal) >= 0) &&
                    string.Equals(node.Name, name, StringComparison.Ordinal))
                {
                    return node;
                }
            }
            return null;
        }

        /// <summary>InvokePattern 优先，自定义控件回落到真实鼠标点击。</summary>
        public bool Click(string name, out string how)
        {
            Node node = FindButton(name);
            how = null;
            if (node == null) return false;

            InvokePattern invoke = PatternOf<InvokePattern>(node.Element, InvokePattern.Pattern);
            if (invoke != null)
            {
                try
                {
                    invoke.Invoke();
                    how = "InvokePattern";
                    return true;
                }
                catch (Exception ex)
                {
                    how = "InvokePattern 失败转鼠标：" + ex.GetType().Name;
                }
            }

            if (ClickAt(node.Element))
            {
                how = (how == null ? "" : how + " → ") + "mouse_event";
                return true;
            }
            how = (how == null ? "" : how + " → ") + "鼠标点击也失败";
            return false;
        }

        public static bool ClickAt(AutomationElement element, bool rightButton = false)
        {
            try
            {
                System.Windows.Point point = element.GetClickablePoint();
                if (double.IsNaN(point.X) || double.IsNaN(point.Y)) return false;
                return NativeClick((int)point.X, (int)point.Y, rightButton);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool NativeClick(int x, int y, bool rightButton)
        {
            if (!SetCursorPos(x, y)) return false;
            uint down = rightButton ? MOUSEEVENTF_RIGHTDOWN : MOUSEEVENTF_LEFTDOWN;
            uint up = rightButton ? MOUSEEVENTF_RIGHTUP : MOUSEEVENTF_LEFTUP;
            mouse_event((int)down, 0, 0, 0, UIntPtr.Zero);
            Thread.Sleep(40);
            mouse_event((int)up, 0, 0, 0, UIntPtr.Zero);
            return true;
        }

        /// <summary>按键事件只发给「有焦点的窗口」，所以先激活再点元素中心。</summary>
        public bool NativeClickTarget(AutomationElement element, bool rightButton = false)
        {
            _process.Refresh();
            BringToForeground(_process.MainWindowHandle);
            return ClickAt(element, rightButton);
        }

        /// <summary>在同进程的其它窗口（Popup / 模态）里找包含任一关键词的节点。</summary>
        public Node WaitForWindowNode(string[] needles, int timeoutMs)
        {
            DateTime deadline = DateTime.UtcNow.AddMilliseconds(timeoutMs);
            while (DateTime.UtcNow < deadline)
            {
                foreach (Node node in SnapshotOtherWindows())
                {
                    foreach (string needle in needles)
                    {
                        if (node.Both.IndexOf(needle, StringComparison.Ordinal) >= 0) return node;
                    }
                }
                Thread.Sleep(150);
            }
            return null;
        }

        /// <summary>关掉本进程的其它可见顶层窗口（模态对话框 / 取色器），避免残留阻塞下一次交互。</summary>
        public void CloseOtherWindows()
        {
            foreach (IntPtr hwnd in TopLevelWindowsOf(_process.Id))
            {
                if (hwnd == _mainHwnd) continue;
                try
                {
                    AutomationElement element = AutomationElement.FromHandle(hwnd);
                    WindowPattern window = element == null ? null : PatternOf<WindowPattern>(element, WindowPattern.Pattern);
                    if (window != null)
                    {
                        try { window.Close(); } catch (Exception) { }
                    }
                    else
                    {
                        PostClose(hwnd);
                    }
                }
                catch (Exception) { }
            }
            Thread.Sleep(300);
        }

        private static void PostClose(IntPtr hwnd)
        {
            try { SendMessage(hwnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero); }
            catch (Exception) { }
        }

        private const int WM_CLOSE = 0x0010;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        /// <summary>把焦点切到本进程除主窗之外的第一个可见窗口（模态对话框），供按键兜底用。</summary>
        public bool FocusOtherWindow()
        {
            foreach (IntPtr hwnd in TopLevelWindowsOf(_process.Id))
            {
                if (hwnd == _mainHwnd) continue;
                BringToForeground(hwnd);
                return true;
            }
            return false;
        }

        public static void PressKey(byte virtualKey)
        {
            keybd_event(virtualKey, 0, 0, UIntPtr.Zero);
            Thread.Sleep(30);
            keybd_event(virtualKey, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
        }

        private const uint KEYEVENTF_KEYUP = 0x0002;

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        private static void BringToForeground(IntPtr hwnd)
        {
            if (hwnd == IntPtr.Zero) return;
            uint own = GetCurrentThreadId();
            uint target = GetWindowThreadProcessId(hwnd, IntPtr.Zero);
            if (target != own) AttachThreadInput(own, target, true);
            SetForegroundWindow(hwnd);
            BringWindowToTop(hwnd);
            if (target != own) AttachThreadInput(own, target, false);
            Thread.Sleep(120);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr processId);

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        [DllImport("user32.dll")]
        private static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        /// <summary>在同进程的其它窗口（Popup / 模态对话框）里找文本。</summary>
        public List<Node> SnapshotOtherWindows()
        {
            List<Node> sink = new List<Node>();
            foreach (IntPtr hwnd in TopLevelWindowsOf(_process.Id))
            {
                if (hwnd == _mainHwnd) continue;
                try
                {
                    AutomationElement element = AutomationElement.FromHandle(hwnd);
                    if (element != null) Walk(element, sink, 0);
                }
                catch (Exception) { }
            }
            return sink;
        }

        /// <summary>
        /// 用 EnumWindows 直接枚举本进程的可见顶层窗口（含 WPF 的 Popup HWND 与模态对话框）。
        /// 之前遍历桌面控件视图的做法在两个运行时上都取不到对话框，属于取证通道选错。
        /// </summary>
        private static List<IntPtr> TopLevelWindowsOf(int processId)
        {
            List<IntPtr> handles = new List<IntPtr>();
            EnumWindows(delegate(IntPtr hwnd, IntPtr param)
            {
                uint windowPid;
                GetWindowThreadProcessId(hwnd, out windowPid);
                if ((int)windowPid == processId && IsWindowVisible(hwnd))
                {
                    handles.Add(hwnd);
                }
                return true;
            }, IntPtr.Zero);
            return handles;
        }

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumProc callback, IntPtr param);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        private delegate bool EnumProc(IntPtr hWnd, IntPtr param);

        /// <summary>只在同进程的其它窗口（弹出层 / 模态框）里等一段文本出现。</summary>
        public Node WaitForPopupNode(string[] needles, int timeoutMs)
        {
            DateTime deadline = DateTime.UtcNow.AddMilliseconds(timeoutMs);
            while (DateTime.UtcNow < deadline)
            {
                foreach (Node node in SnapshotOtherWindows())
                {
                    foreach (string needle in needles)
                    {
                        if (node.Both.IndexOf(needle, StringComparison.Ordinal) >= 0) return node;
                    }
                }
                Thread.Sleep(120);
            }
            return null;
        }

        /// <summary>收起弹出层：点主窗口顶部空白处（比 Esc 通用，Esc 常被控件自己吃掉）。</summary>
        public void ClosePopups()
        {
            try
            {
                System.Windows.Rect bounds = _root.Current.BoundingRectangle;
                if (bounds.Width > 40 && bounds.Height > 40)
                {
                    NativeClick((int)(bounds.Left + bounds.Width / 2), (int)(bounds.Top + 10), false);
                }
                Thread.Sleep(250);
            }
            catch (Exception) { }
        }

        public void Close()
        {
            try
            {
                _process.Refresh();
                if (!_process.HasExited)
                {
                    WindowPattern window = PatternOf<WindowPattern>(_root, WindowPattern.Pattern);
                    if (window != null)
                    {
                        try { window.Close(); } catch (Exception) { }
                        Thread.Sleep(400);
                    }
                    _process.Refresh();
                    if (!_process.HasExited) _process.Kill(true);
                }
            }
            catch (Exception) { }
        }

        public void Dispose()
        {
            Close();
        }

        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const uint MOUSEEVENTF_RIGHTUP = 0x0010;

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, UIntPtr dwExtraInfo);
    }

    /// <summary>跨进程回读 DWM 标题栏深色标志（PORTING.md §14 里唯一能拿到硬证据的观感项）。</summary>
    internal static class Dwm
    {
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
        private const int DWMWA_CAPTION_COLOR = 35;

        public static int LastHresult;

        [DllImport("dwmapi.dll")]
        private static extern int DwmGetWindowAttribute(IntPtr hwnd, int attr, out int value, int size);

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int value, int size);

        public static bool? UsesDarkMode(IntPtr hwnd)
        {
            LastHresult = int.MinValue;
            if (hwnd == IntPtr.Zero) return null;
            int value;
            int hr = DwmGetWindowAttribute(hwnd, DWMWA_USE_IMMERSIVE_DARK_MODE, out value, sizeof(int));
            LastHresult = hr;
            if (hr != 0) return null;
            return value != 0;
        }

        /// <summary>直接用 harness 自己的 P/Invoke 试一次，用来区分「系统拒绝」与「库内的声明在 net10 下失效」。</summary>
        public static string RawProbe(IntPtr hwnd)
        {
            if (hwnd == IntPtr.Zero) return "无 HWND";
            int on = 1;
            int hr20 = DwmSetWindowAttribute(hwnd, DWMWA_USE_IMMERSIVE_DARK_MODE, ref on, sizeof(int));
            int hr19 = DwmSetWindowAttribute(hwnd, DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1, ref on, sizeof(int));
            int caption = 0x00282830;
            int hr35 = DwmSetWindowAttribute(hwnd, DWMWA_CAPTION_COLOR, ref caption, sizeof(int));
            int read;
            int hrGet = DwmGetWindowAttribute(hwnd, DWMWA_USE_IMMERSIVE_DARK_MODE, out read, sizeof(int));
            return "Set20=0x" + Hex(hr20) + " Set19=0x" + Hex(hr19) + " SetCaption35=0x" + Hex(hr35) +
                   " Get20=0x" + Hex(hrGet) + "(值=" + read + ")";
        }

        private static string Hex(int value)
        {
            return value.ToString("X8", CultureInfo.InvariantCulture);
        }
    }

    internal static class Report
    {
        private static readonly List<string[]> Rows = new List<string[]>();
        private static readonly StringBuilder Log = new StringBuilder();
        public static int Pass;
        public static int Fail;
        public static int Skip;

        public static void Check(string id, string scope, bool ok, string expected, string actual)
        {
            if (ok) Pass++; else Fail++;
            Rows.Add(new string[] { ok ? "PASS" : "FAIL", id, scope, expected, actual });
            Console.WriteLine((ok ? "PASS " : "FAIL ") + id + " [" + scope + "] 期望=" + expected +
                              " 实际=" + actual);
        }

        public static void Skipped(string id, string scope, string why)
        {
            Skip++;
            Rows.Add(new string[] { "SKIP", id, scope, "-", why });
            Console.WriteLine("SKIP " + id + " [" + scope + "] " + why);
        }

        public static void Info(string line)
        {
            Log.AppendLine(line);
            Console.WriteLine(line);
        }

        public static void Write(string path, string header)
        {
            StringBuilder md = new StringBuilder();
            md.AppendLine(header);
            md.AppendLine();
            md.AppendLine("| 结果 | 用例 | 作用域 | 期望 | 实际 |");
            md.AppendLine("|---|---|---|---|---|");
            foreach (string[] row in Rows)
            {
                md.AppendLine("| " + row[0] + " | " + row[1] + " | " + row[2] + " | " +
                              Esc(row[3]) + " | " + Esc(row[4]) + " |");
            }
            md.AppendLine();
            md.AppendLine("汇总：" + Pass + " PASS / " + Fail + " FAIL / " + Skip + " SKIP");
            md.AppendLine();
            md.AppendLine("```");
            md.AppendLine(Log.ToString());
            md.AppendLine("```");
            System.IO.File.WriteAllText(path, md.ToString(), new UTF8Encoding(false));
            Console.WriteLine("结果已写入 " + path);
        }

        private static string Esc(string text)
        {
            if (string.IsNullOrEmpty(text)) return "-";
            return text.Replace("|", "\\|").Replace("\r", " ").Replace("\n", " ");
        }
    }
}
