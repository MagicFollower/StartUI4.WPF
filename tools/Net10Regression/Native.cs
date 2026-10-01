using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Net10Regression
{
    /// <summary>
    /// harness 自己的原生剪贴板读回：与被测进程完全无关，等价于 clipboard-lock-check 的 --dump，
    /// 这样「页面说复制成功了」和「系统剪贴板里真有什么」是两条独立证据。
    /// </summary>
    internal static class NativeClipboard
    {
        private const uint CF_UNICODETEXT = 13;
        private const uint GHND = 0x0042;

        public static string TryReadText()
        {
            for (int attempt = 0; attempt < 20; attempt++)
            {
                if (!IsClipboardFormatAvailable(CF_UNICODETEXT)) return null;
                string text = TryReadOnce();
                if (text != null) return text;
                Thread.Sleep(100);
            }
            return null;
        }

        private static string TryReadOnce()
        {
            if (!OpenClipboard(IntPtr.Zero)) return null;
            try
            {
                IntPtr data = GetClipboardData(CF_UNICODETEXT);
                if (data == IntPtr.Zero) return string.Empty;
                IntPtr locked = GlobalLock(data);
                if (locked == IntPtr.Zero) return string.Empty;
                try
                {
                    return Marshal.PtrToStringUni(locked);
                }
                finally
                {
                    GlobalUnlock(data);
                }
            }
            finally
            {
                CloseClipboard();
            }
        }

        /// <summary>写入空串占位，让「复制」类断言能区分新旧内容。</summary>
        public static void ClearIfPossible()
        {
            for (int attempt = 0; attempt < 20; attempt++)
            {
                if (OpenClipboard(IntPtr.Zero))
                {
                    try
                    {
                        EmptyClipboard();
                    }
                    finally
                    {
                        CloseClipboard();
                    }
                    return;
                }
                Thread.Sleep(100);
            }
        }

        public static bool WriteText(string text)
        {
            byte[] bytes = Encoding.Unicode.GetBytes((text ?? string.Empty) + "\0");
            for (int attempt = 0; attempt < 20; attempt++)
            {
                if (!OpenClipboard(IntPtr.Zero))
                {
                    Thread.Sleep(100);
                    continue;
                }
                IntPtr mem = GlobalAlloc(GHND, (UIntPtr)bytes.Length);
                if (mem == IntPtr.Zero)
                {
                    CloseClipboard();
                    return false;
                }
                try
                {
                    IntPtr locked = GlobalLock(mem);
                    Marshal.Copy(bytes, 0, locked, bytes.Length);
                    GlobalUnlock(mem);
                    EmptyClipboard();
                    return SetClipboardData(CF_UNICODETEXT, mem) != IntPtr.Zero;
                }
                finally
                {
                    CloseClipboard();
                }
            }
            return false;
        }

        [DllImport("user32.dll")]
        private static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll")]
        private static extern bool CloseClipboard();

        [DllImport("user32.dll")]
        private static extern bool EmptyClipboard();

        [DllImport("user32.dll")]
        private static extern IntPtr GetClipboardData(uint uFormat);

        [DllImport("user32.dll")]
        private static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);

        [DllImport("user32.dll")]
        private static extern bool IsClipboardFormatAvailable(uint uFormat);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr dwBytes);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("kernel32.dll")]
        private static extern bool GlobalUnlock(IntPtr hMem);
    }

    internal static class NativeWin
    {
        [DllImport("user32.dll")]
        public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int width, int height, bool repaint);
    }
}
