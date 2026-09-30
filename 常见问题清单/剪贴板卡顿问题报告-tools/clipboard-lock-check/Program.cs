using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using StartUI4Controls;

/// <summary>
/// 剪贴板持锁回归验证：用独立进程占住剪贴板模拟监听类程序，再发真实 Ctrl+C/X/V，
/// 度量 UI 线程阻塞时长、选区是否立即处理、锁释放后剪贴板内容是否正确。
/// </summary>
static class Program
{
    const uint CF_UNICODETEXT = 13;
    const uint GMEM_MOVEABLE = 0x0002;
    const uint GMEM_ZEROINIT = 0x0040;
    const byte VK_CONTROL = 0x11;
    const uint KEYEVENTF_KEYUP = 0x0002;

    const int HoldMs = 1200;
    const int BlockLimitMs = 100;

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool OpenClipboard(IntPtr h);
    [DllImport("user32.dll")]
    static extern bool CloseClipboard();
    [DllImport("user32.dll")]
    static extern bool EmptyClipboard();
    [DllImport("user32.dll")]
    static extern IntPtr GetClipboardData(uint format);
    [DllImport("user32.dll")]
    static extern IntPtr SetClipboardData(uint format, IntPtr hMem);
    [DllImport("kernel32.dll")]
    static extern IntPtr GlobalAlloc(uint flags, UIntPtr size);
    [DllImport("kernel32.dll")]
    static extern IntPtr GlobalLock(IntPtr h);
    [DllImport("kernel32.dll")]
    static extern bool GlobalUnlock(IntPtr h);
    [DllImport("user32.dll")]
    static extern void keybd_event(byte vk, byte scan, uint flags, UIntPtr extra);

    static string ResultsPath;

    [STAThread]
    static int Main(string[] args)
    {
        ResultsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "results.txt");

        if (args.Length > 0) return Child(args);
        return Matrix();
    }

    // ---- 子命令：占锁 / 播种内容 / 原生读回 ----
    static int Child(string[] args)
    {
        if (args[0] == "--hold")
        {
            int ms = args.Length > 1 ? int.Parse(args[1]) : HoldMs;
            if (!OpenClipboard(IntPtr.Zero)) return 1;
            Thread.Sleep(ms);
            CloseClipboard();
            return 0;
        }

        if (args[0] == "--seed")
        {
            return TryWriteNative(args.Length > 1 ? args[1] : string.Empty) ? 0 : 1;
        }

        if (args[0] == "--dump")
        {
            string text = null;
            for (int i = 0; i < 25 && text == null; i++)
            {
                text = TryReadNative();
                if (text == null) Thread.Sleep(100);
            }
            File.WriteAllText(args.Length > 1 ? args[1] : "dump.txt", text ?? string.Empty);
            return text == null ? 1 : 0;
        }

        return 2;
    }

    static bool TryWriteNative(string value)
    {
        if (!OpenClipboard(IntPtr.Zero)) return false;
        try
        {
            EmptyClipboard();
            var bytes = new byte[(value.Length + 1) * 2];
            Encoding.Unicode.GetBytes(value, 0, value.Length, bytes, 0);
            IntPtr hMem = GlobalAlloc(GMEM_MOVEABLE | GMEM_ZEROINIT, (UIntPtr)bytes.Length);
            if (hMem == IntPtr.Zero) return false;
            IntPtr p = GlobalLock(hMem);
            if (p == IntPtr.Zero) return false;
            Marshal.Copy(bytes, 0, p, bytes.Length);
            GlobalUnlock(hMem);
            return SetClipboardData(CF_UNICODETEXT, hMem) != IntPtr.Zero;
        }
        catch
        {
            return false;
        }
        finally
        {
            CloseClipboard();
        }
    }

    static string TryReadNative()
    {
        if (!OpenClipboard(IntPtr.Zero)) return null;
        try
        {
            IntPtr hMem = GetClipboardData(CF_UNICODETEXT);
            if (hMem == IntPtr.Zero) return null;
            IntPtr p = GlobalLock(hMem);
            if (p == IntPtr.Zero) return null;
            string text = Marshal.PtrToStringUni(p);
            GlobalUnlock(hMem);
            return text;
        }
        catch
        {
            return null;
        }
        finally
        {
            CloseClipboard();
        }
    }

    // ---- 用例矩阵 ----
    static int Matrix()
    {
        File.WriteAllText(ResultsPath,
            "clipboard-lock-check  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
            " | 锁占用时长=" + HoldMs + "ms | 接管阈值=" + BlockLimitMs + "ms\r\n\r\n");

        var app = new Application();
        var panel = new StackPanel();

        var plain = new TextBox { Text = "alpha bravo", Width = 240 };
        var ui4Box = new UI4TextBox { Text = "alpha bravo", Width = 240 };
        var editor = new UI4CodeEditor { Width = 240, Height = 60 };
        editor.Text = "alpha bravo";
        var pwd = new UI4PasswordBox { Width = 240, IsPasswordMode = false };
        pwd.Text = "alpha bravo";

        panel.Children.Add(plain);
        panel.Children.Add(ui4Box);
        panel.Children.Add(editor);
        panel.Children.Add(pwd);

        var win = new Window
        {
            Content = panel,
            Width = 320,
            Height = 300,
            WindowStartupLocation = WindowStartupLocation.Manual,
            Left = -3000,
            Top = -3000,
            ShowInTaskbar = false
        };
        win.Show();
        app.Dispatcher.Invoke(new Action(delegate { }), DispatcherPriority.ContextIdle);

        // 基线：框架控件走 WPF/OLE 通道，用于证明实验确实处于抢锁状态
        Run(app, "基线 plain TextBox（框架通道）", plain, (byte)'X',
            delegate { plain.Text = "alpha bravo"; plain.Focus(); plain.Select(6, 5); },
            delegate { return plain.Text; }, null, null);

        Run(app, "UI4TextBox", ui4Box, (byte)'X',
            delegate { ui4Box.Text = "alpha bravo"; ui4Box.Focus(); ui4Box.Select(6, 5); },
            delegate { return ui4Box.Text; }, "alpha ", "bravo");

        Run(app, "UI4TextBox", ui4Box, (byte)'C',
            delegate { ui4Box.Text = "alpha bravo"; ui4Box.Focus(); ui4Box.Select(6, 5); },
            delegate { return ui4Box.Text; }, "alpha bravo", "bravo");

        Run(app, "UI4TextBox", ui4Box, (byte)'V',
            delegate
            {
                Seed("Z");
                ui4Box.Text = "alpha ";
                ui4Box.CaretIndex = 6;
                ui4Box.Focus();
            },
            delegate { return ui4Box.Text; }, "alpha Z", "Z");

        Run(app, "UI4CodeEditor", editor, (byte)'X',
            delegate
            {
                editor.Text = "alpha bravo";
                editor.Focus();
                editor.TextArea.Focus();
                editor.SelectionStart = 6;
                editor.SelectionLength = 5;
            },
            delegate { return editor.Text; }, "alpha ", "bravo");

        Run(app, "UI4CodeEditor", editor, (byte)'C',
            delegate
            {
                editor.Text = "alpha bravo";
                editor.Focus();
                editor.TextArea.Focus();
                editor.SelectionStart = 6;
                editor.SelectionLength = 5;
            },
            delegate { return editor.Text; }, "alpha bravo", "bravo");

        Run(app, "UI4CodeEditor", editor, (byte)'V',
            delegate
            {
                Seed("Z");
                editor.Text = "alpha ";
                editor.CaretOffset = 6;
                editor.Focus();
                editor.TextArea.Focus();
            },
            delegate { return editor.Text; }, "alpha Z", "Z");

        Run(app, "UI4PasswordBox 明文模式", pwd, (byte)'X',
            delegate { pwd.Text = "alpha bravo"; pwd.Focus(); pwd.Select(6, 5); },
            delegate { return pwd.Text; }, "alpha ", "bravo");

        Run(app, "UI4PasswordBox 明文模式", pwd, (byte)'C',
            delegate { pwd.Text = "alpha bravo"; pwd.Focus(); pwd.Select(6, 5); },
            delegate { return pwd.Text; }, "alpha bravo", "bravo");

        Run(app, "UI4PasswordBox 明文模式", pwd, (byte)'V',
            delegate
            {
                Seed("Z");
                pwd.Text = "alpha ";
                pwd.CaretIndex = 6;
                pwd.Focus();
            },
            delegate { return pwd.Text; }, "alpha Z", "Z");

        return 0;
    }

    static void Run(Application app, string name, Control view, byte key,
        Action reset, Func<string> readText, string expectRemaining, string expectClipboard)
    {
        reset();

        var holder = Start("--hold " + HoldMs);
        Thread.Sleep(300);

        var sw = Stopwatch.StartNew();
        keybd_event(VK_CONTROL, 0, 0, UIntPtr.Zero);
        keybd_event(key, 0, 0, UIntPtr.Zero);
        keybd_event(key, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
        keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
        app.Dispatcher.Invoke(new Action(delegate { }), DispatcherPriority.ContextIdle);
        sw.Stop();

        long blocked = sw.ElapsedMilliseconds;
        string immediate = readText();

        // 粘贴是后台读取后回填，轮询等待最终文本
        var deadline = DateTime.Now.AddSeconds(3);
        while (expectRemaining != null && immediate != expectRemaining && DateTime.Now < deadline)
        {
            app.Dispatcher.Invoke(new Action(delegate { }), DispatcherPriority.ContextIdle);
            Thread.Sleep(50);
            immediate = readText();
        }

        bool lockHeld = holder.WaitForExit(8000) && holder.ExitCode == 0;
        if (!lockHeld && !holder.HasExited) holder.Kill();

        string clipboard = null;
        if (expectClipboard != null)
        {
            string dumpFile = Path.Combine(Path.GetTempPath(), "clipdump.txt");
            if (File.Exists(dumpFile)) File.Delete(dumpFile);
            var dump = Start("--dump \"" + dumpFile + "\"");
            if (!dump.WaitForExit(8000)) dump.Kill();
            if (File.Exists(dumpFile)) clipboard = File.ReadAllText(dumpFile);
        }

        bool takenOver = expectRemaining != null;
        bool pass = takenOver
            ? blocked <= BlockLimitMs && immediate == expectRemaining && clipboard == expectClipboard
            : blocked > 400;

        Line((pass ? "PASS" : "FAIL") + "  " + name + "  Ctrl+" + (char)key
            + " | UI阻塞=" + blocked + "ms"
            + " | 即时文本=[" + immediate + "] 期望=[" + (expectRemaining ?? "—") + "]"
            + " | 剪贴板=[" + (clipboard ?? "—") + "] 期望=[" + (expectClipboard ?? "—") + "]"
            + " | 抢锁生效=" + lockHeld
            + (takenOver ? string.Empty : "（基线用例期望：阻塞 >400ms，证明通道确实被锁拖住）"));
    }

    static void Seed(string text)
    {
        var seed = Start("--seed \"" + text + "\"");
        if (!seed.WaitForExit(5000)) seed.Kill();
    }

    static Process Start(string arguments)
    {
        return Process.Start(new ProcessStartInfo(
            Process.GetCurrentProcess().MainModule.FileName, arguments)
        {
            UseShellExecute = false,
            CreateNoWindow = true
        });
    }

    static void Line(string text)
    {
        File.AppendAllText(ResultsPath, text + "\r\n");
    }
}
