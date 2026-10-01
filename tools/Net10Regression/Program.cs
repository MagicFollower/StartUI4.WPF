using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Microsoft.Win32;

namespace Net10Regression
{
    internal static class Program
    {
        private static string _exeDir;
        private static int _launchWaitMs = 15000;

        [STAThread]
        private static int Main(string[] args)
        {
            Console.OutputEncoding = new UTF8Encoding(false);

            bool doTabs = false, doInProc = false, doSystem = false, doAll = false;
            string baselineDir = null;
            string outPath = Path.Combine(AppContext.BaseDirectory, "results.md");
            int onlyTab = -1;
            int doDump = -1;
            bool probeStatus = false;

            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                if (arg.StartsWith("--tab=", StringComparison.OrdinalIgnoreCase))
                {
                    onlyTab = int.Parse(arg.Substring("--tab=".Length), CultureInfo.InvariantCulture);
                    doTabs = true;
                    continue;
                }
                if (arg.StartsWith("--dump=", StringComparison.OrdinalIgnoreCase))
                {
                    doDump = int.Parse(arg.Substring("--dump=".Length), CultureInfo.InvariantCulture);
                    continue;
                }

                switch (arg)
                {
                    case "--tabs": doTabs = true; break;
                    case "--inproc": doInProc = true; break;
                    case "--probe-status": probeStatus = true; break;
                    case "--system": doSystem = true; break;
                    case "--all": doAll = true; break;
                    case "--exe": _exeDir = NormalizeDir(Value(args, ref i)); doTabs = true; break;
                    case "--baseline": baselineDir = NormalizeDir(Value(args, ref i)); break;
                    case "--out": outPath = Path.GetFullPath(Value(args, ref i)); break;
                    case "--wait": _launchWaitMs = int.Parse(Value(args, ref i), CultureInfo.InvariantCulture); break;
                    case "--help":
                        Console.WriteLine("用法：Net10Regression [--all] [--tabs [--tab=N]] [--inproc] [--system]");
                        Console.WriteLine("      [--exe <Demo bin 目录>] [--baseline <对照 bin 目录>] [--out <results.md>] [--wait <ms>]");
                        return 0;
                    default:
                        Console.WriteLine("未知参数：" + arg);
                        break;
                }
            }

            if (doAll || (!doTabs && !doInProc && !doSystem))
            {
                doTabs = true; doInProc = true; doSystem = true;
            }
            if (onlyTab >= 0) doTabs = true;

            if (string.IsNullOrEmpty(_exeDir)) _exeDir = DefaultExeDir();

            if (probeStatus)
            {
                ProbeStatusStaleness();
                return 0;
            }

            if (doDump >= 0)
            {
                DumpPage(onlyTab >= 0 ? onlyTab : doDump);
                return 0;
            }

            Report.Info("=== StartUI4 .NET 10 迁移回归 harness ===");
            Report.Info("harness 自身运行时：" + RuntimeInformation.FrameworkDescription +
                        "（Environment.Version=" + Environment.Version + "，x64=" + Environment.Is64BitProcess + "）");
            Report.Info("被测 Demo 目录：" + _exeDir);
            if (baselineDir != null) Report.Info("对照基线目录：" + baselineDir);
            Report.Info(string.Empty);

            Check("S00", "运行真实性", Environment.Version.Major == 10,
                  "harness 跑在 .NET 10", "Environment.Version=" + Environment.Version);

            if (doSystem) RunSystemSuite();
            if (doTabs) RunTabs(_exeDir, "net10", onlyTab);
            if (baselineDir != null) RunTabs(baselineDir, "baseline", onlyTab);
            if (doInProc) InProcSuite.Run();

            Report.Info(string.Empty);
            Report.Info("=== 汇总：" + Report.Pass + " PASS / " + Report.Fail + " FAIL / " + Report.Skip + " SKIP ===");
            Report.Write(outPath, "# StartUI4 .NET 10 迁移回归结果\n\n生成时间：" +
                       DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
            return Report.Fail;
        }

        /// <summary>把某一页的整棵 UIA 控制视图打出来（含每个节点的矩形），用来判断「页面没文本」到底是没渲染还是没暴露。</summary>
        private static void DumpPage(int tab)
        {
            string exe = Path.Combine(_exeDir, "StartUI4Demo.exe");
            Console.WriteLine("=== UIA 快照 dump：页 " + tab + "（" + Cases.TabTitles[tab] + "）===");
            using (DemoSession session = DemoSession.Launch(exe, tab, _launchWaitMs))
            {
                List<Node> nodes = session.Snapshot();
                Console.WriteLine("节点数=" + nodes.Count + " 窗口=" + session.Title);
                int index = 0;
                foreach (Node node in nodes)
                {
                    index++;
                    string rect;
                    try
                    {
                        System.Windows.Rect bounds = node.Element.Current.BoundingRectangle;
                        rect = string.Format(CultureInfo.InvariantCulture, "{0:0}x{1:0}@{2:0},{3:0}",
                                             bounds.Width, bounds.Height, bounds.Left, bounds.Top);
                    }
                    catch (Exception)
                    {
                        rect = "?";
                    }
                    string text = string.IsNullOrEmpty(node.Name) ? node.Value : node.Name;
                    Console.WriteLine(string.Format(CultureInfo.InvariantCulture,
                        "{0,4}  {1,-26} {2,-14} {3}", index, node.Type, rect, Truncate(text, 70)));
                }

                // 原始视图对照：只统计带文字的节点，用来判断内容到底有没有渲染
                List<Node> raw = session.SnapshotRaw();
                int withText = 0;
                foreach (Node node in raw)
                {
                    if (!string.IsNullOrEmpty(node.Name)) withText++;
                }
                Console.WriteLine("原始视图：节点数=" + raw.Count + "，其中有 Name 的=" + withText);
                foreach (Node node in raw)
                {
                    if (!string.IsNullOrEmpty(node.Name) && node.Name.Contains("内容"))
                    {
                        Console.WriteLine("   RAW 命中「" + Truncate(node.Name, 40) + "」类型=" + node.Type);
                    }
                }
                session.Close();
            }
        }

        /// <summary>
        /// 判别实验：连续点两个按钮，每次都读底部状态栏。
        /// 用来分清「点击没生效」与「TextBlock 改了文本但 UIA 还在报旧值（Name 陈旧）」。
        /// </summary>
        private static void ProbeStatusStaleness()
        {
            string exe = Path.Combine(_exeDir, "StartUI4Demo.exe");
            using (DemoSession session = DemoSession.Launch(exe, 0, _launchWaitMs))
            {
                string ReadFooter()
                {
                    foreach (Node node in session.Snapshot())
                    {
                        if (!string.IsNullOrEmpty(node.Name) &&
                            (node.Name.StartsWith("就绪", StringComparison.Ordinal) ||
                             node.Name.StartsWith("UI4Button", StringComparison.Ordinal)))
                        {
                            return node.Name;
                        }
                    }
                    return "(没找到状态栏节点)";
                }

                Console.WriteLine("初始状态栏 = " + ReadFooter());
                string how;
                Console.WriteLine("点「默认按钮」= " + session.Click("默认按钮", out how) + " (" + how + ")");
                Thread.Sleep(400);
                Console.WriteLine("第一次读 = " + ReadFooter());
                Console.WriteLine("点「绿色按钮」= " + session.Click("绿色按钮", out how) + " (" + how + ")");
                Thread.Sleep(400);
                Console.WriteLine("第二次读 = " + ReadFooter());
                Console.WriteLine("点「启用（浅灰底）」= " + session.Click("启用（浅灰底）", out how) + " (" + how + ")");
                Thread.Sleep(400);
                Console.WriteLine("第三次读 = " + ReadFooter());
                Console.WriteLine("点「启用（默认渐变）」= " + session.Click("启用（默认渐变）", out how) + " (" + how + ")");
                Thread.Sleep(600);
                Console.WriteLine("全量含「被点击」的节点：");
                foreach (Node node in session.Snapshot())
                {
                    if ((node.Name ?? string.Empty).Contains("被点击") ||
                        (node.Value ?? string.Empty).Contains("被点击"))
                    {
                        Console.WriteLine("   [" + node.Type + "] Name=" + node.Name + " Value=" + node.Value);
                    }
                }
                session.Close();
            }
        }

        private static string Value(string[] args, ref int i)
        {
            if (i + 1 >= args.Length) throw new ArgumentException(args[i] + " 缺参数");
            i++;
            return args[i];
        }

        private static string NormalizeDir(string dir)
        {
            if (string.IsNullOrEmpty(dir)) return null;
            return Path.GetFullPath(dir.TrimEnd('"').TrimEnd('\\'));
        }

        private static string DefaultExeDir()
        {
            // base = tools/Net10Regression/bin/Debug/net10.0-windows/
            string candidate = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory,
                "..", "..", "..", "..", "..", "samples", "StartUI4Demo", "bin", "Debug", "net10.0-windows"));
            if (File.Exists(Path.Combine(candidate, "StartUI4Demo.exe"))) return candidate;

            candidate = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory,
                "..", "..", "..", "..", "..", "samples", "StartUI4Demo", "bin", "Release", "net10.0-windows"));
            if (File.Exists(Path.Combine(candidate, "StartUI4Demo.exe"))) return candidate;

            throw new FileNotFoundException("没找到 StartUI4Demo.exe，请先构建 Demo 或用 --exe 指定目录");
        }

        public static void Check(string id, string scope, bool ok, string expected, string actual)
        {
            Report.Check(id, scope, ok, expected, actual);
        }

        // ---------- 供 Cases.cs 复用 ----------

        public static bool DemoErrorLogEmpty(string dir)
        {
            if (string.IsNullOrEmpty(dir)) return true;
            bool clean = !File.Exists(Path.Combine(dir, "demo-errors.log")) &&
                         !File.Exists(Path.Combine(dir, "ui4menu-style-error.log"));
            if (!clean)
            {
                foreach (string name in new string[] { "demo-errors.log", "ui4menu-style-error.log" })
                {
                    string path = Path.Combine(dir, name);
                    if (File.Exists(path)) Report.Info("!! " + path + " 出现内容：\n" + ReadAll(path));
                }
            }
            return clean;
        }

        /// <summary>原样读回（保留换行）。早先这里顺手 Truncate，把 \r\n 换成空格，
        /// 导致 S08 统计 results.txt 里的 PASS 行数恒为 0——解析用的文本绝不能先被格式化。</summary>
        private static string ReadAll(string path)
        {
            try { return File.ReadAllText(path); }
            catch (Exception ex) { return "(读取失败 " + ex.GetType().Name + ")"; }
        }

        public static bool PersistedFileExists(string dir)
        {
            return !string.IsNullOrEmpty(dir) && File.Exists(Path.Combine(dir, "demo-theme.json"));
        }

        public static bool RegistryStartUI4Exists()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\StartUI4"))
            {
                return key != null;
            }
        }

        public static int ExtractInt(string text, string after)
        {
            if (string.IsNullOrEmpty(text)) return -1;
            int index = text.IndexOf(after, StringComparison.Ordinal);
            if (index < 0) return -1;
            string rest = text.Substring(index + after.Length);
            var builder = new StringBuilder();
            foreach (char c in rest)
            {
                if (c >= '0' && c <= '9') builder.Append(c);
                else if (builder.Length > 0) break;
            }
            int value;
            return int.TryParse(builder.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out value) ? value : -1;
        }

        public static string FlagText(bool? value)
        {
            return value == null ? "(不可读/系统拒绝)" : (value.Value ? "1（深色）" : "0（亮色）");
        }

        public static string Truncate(string text, int max)
        {
            if (string.IsNullOrEmpty(text)) return "(空)";
            text = text.Replace("\r", " ").Replace("\n", " ");
            return text.Length <= max ? text : text.Substring(0, max) + "…";
        }

        // ---------- 系统级套件 ----------

        private static void RunSystemSuite()
        {
            string apphost = Path.Combine(_exeDir, "StartUI4Demo.exe");
            string managed = Path.Combine(_exeDir, "StartUI4Demo.dll");
            string lib = Path.Combine(_exeDir, "StartUI4Controls.dll");

            Check("S01", "运行真实性", File.Exists(apphost) && File.Exists(managed) && File.Exists(lib),
                  "被测产物存在（.NET 的 exe 是原生 apphost，托管代码在同名 dll 里）",
                  "exe=" + File.Exists(apphost) + " dll=" + File.Exists(managed) + " 库=" + File.Exists(lib));

            string appTfm = ReadTargetFrameworkString(managed);
            string libTfm = ReadTargetFrameworkString(lib);
            Check("S02", "运行真实性", (appTfm ?? string.Empty).Contains(".NETCoreApp,Version=v10.0"),
                  "StartUI4Demo.dll 的 TargetFrameworkAttribute = .NETCoreApp,Version=v10.0",
                  "实测=" + (appTfm ?? "(未找到)"));
            Check("S03", "运行真实性", (libTfm ?? string.Empty).Contains(".NETCoreApp,Version=v10.0"),
                  "StartUI4Controls.dll 的 TargetFrameworkAttribute = .NETCoreApp,Version=v10.0",
                  "实测=" + (libTfm ?? "(未找到)"));

            // apphost 里没有 TFM 字符串本身就是一条结构差异证据：net48 时代 exe 就是托管程序集
            Check("S03b", "运行真实性", ReadTargetFrameworkString(apphost) == null,
                  "exe 是原生 apphost（不含 TFM 特性串），托管主体在 dll —— 与 net48 的单文件托管 exe 不同",
                  "exe 大小=" + new FileInfo(apphost).Length + " 找到的 TFM 串=" + (ReadTargetFrameworkString(apphost) ?? "(无)"));

            string runtimeConfigPath = Path.Combine(_exeDir, "StartUI4Demo.runtimeconfig.json");
            string runtimeConfig = File.Exists(runtimeConfigPath) ? ReadAll(runtimeConfigPath) : "(不存在)";
            Check("S04", "运行真实性",
                  runtimeConfig.Contains("Microsoft.WindowsDesktop.App") && runtimeConfig.Contains("10."),
                  "runtimeconfig.json 声明 WindowsDesktop 10.x（net48 时代由 exe.config 的 sku 承担）",
                  Truncate(runtimeConfig, 200));

            string config = Path.Combine(_exeDir, "StartUI4Demo.exe.config");
            Check("S05", "运行真实性", !File.Exists(config),
                  "net10 产物不再产出 .exe.config（net48 基线里有，是两套宿主机制的分水岭）",
                  "存在=" + File.Exists(config));

            Check("S06", "注册表零污染", !RegistryStartUI4Exists(),
                  "HKCU\\Software\\StartUI4 不存在（Demo 只用 JsonThemePersistence）",
                  "存在=" + RegistryStartUI4Exists());

            Check("S07", "异常日志", DemoErrorLogEmpty(_exeDir),
                  "demo-errors.log / ui4menu-style-error.log 均不存在", "目录=" + _exeDir);

            RunClipboardLockCheck();
        }

        /// <summary>
        /// 从产物里读 TargetFrameworkAttribute 的值：该字符串以 UTF8 存在元数据堆里，
        /// 直接扫字节最省事，也避免在 net10 进程里加载 net48 程序集（会抛）。
        /// </summary>
        private static string ReadTargetFrameworkString(string path)
        {
            try
            {
                byte[] bytes = File.ReadAllBytes(path);
                foreach (string marker in new string[] { ".NETCoreApp,Version=v", ".NETCore,Version=v", ".NETFramework,Version=v" })
                {
                    byte[] needle = Encoding.ASCII.GetBytes(marker);
                    int index = IndexOf(bytes, needle);
                    if (index < 0) continue;
                    var builder = new StringBuilder(marker);
                    for (int i = index + needle.Length; i < bytes.Length; i++)
                    {
                        byte b = bytes[i];
                        if (b == 0 || b < 0x20 || b > 0x7E) break;
                        builder.Append((char)b);
                    }
                    return builder.ToString();
                }
                return null;
            }
            catch (Exception ex)
            {
                return "(读取失败 " + ex.GetType().Name + ")";
            }
        }

        private static int IndexOf(byte[] haystack, byte[] needle)
        {
            for (int i = 0; i <= haystack.Length - needle.Length; i++)
            {
                bool hit = true;
                for (int j = 0; j < needle.Length; j++)
                {
                    if (haystack[i + j] != needle[j]) { hit = false; break; }
                }
                if (hit) return i;
            }
            return -1;
        }

        private static void RunClipboardLockCheck()
        {
            string toolDir = FindToolDir();
            if (toolDir == null)
            {
                Report.Skipped("S08", "剪贴板持锁矩阵",
                               "没找到 clipboard-lock-check.exe，请先构建 tools/clipboard-lock-check");
                return;
            }

            string exe = Path.Combine(toolDir, "clipboard-lock-check.exe");
            string results = Path.Combine(toolDir, "results.txt");
            if (File.Exists(results)) File.Delete(results);

            Report.Info("→ 运行剪贴板持锁矩阵：" + exe);
            try
            {
                Process process = Process.Start(new ProcessStartInfo(exe)
                {
                    WorkingDirectory = toolDir,
                    UseShellExecute = false
                });
                if (!process.WaitForExit(240000))
                {
                    try { process.Kill(true); } catch (Exception) { }
                    Report.Check("S08", "剪贴板持锁矩阵", false, "240s 内跑完", "超时被终止");
                    return;
                }
            }
            catch (Exception ex)
            {
                Report.Check("S08", "剪贴板持锁矩阵", false, "进程正常退出", "启动失败：" + ex.GetType().Name);
                return;
            }

            if (!File.Exists(results))
            {
                Report.Check("S08", "剪贴板持锁矩阵", false, "生成 results.txt", "文件不存在");
                return;
            }

            string text = ReadAll(results);
            int pass = CountLines(text, "PASS");
            int fail = CountLines(text, "FAIL");
            int skip = CountLines(text, "SKIP");
            Check("S08", "剪贴板持锁矩阵", fail == 0 && pass >= 8,
                  "0 FAIL 且 ≥8 PASS（历史基线 10 用例；本机若有剪贴板监听器可能 SKIP）",
                  "PASS=" + pass + " FAIL=" + fail + " SKIP=" + skip +
                  " 文件字节=" + new FileInfo(results).Length +
                  " 写于=" + File.GetLastWriteTime(results).ToString("HH:mm:ss", CultureInfo.InvariantCulture) +
                  " 读回字符=" + text.Length + " 头部=" + Truncate(text, 90));
            if (fail > 0) Report.Info("--- clipboard-lock-check results.txt ---\n" + text);
        }

        private static int CountLines(string text, string token)
        {
            int count = 0;
            foreach (string line in text.Split(new char[] { '\r', '\n' }, StringSplitOptions.None))
            {
                if (line.Trim().StartsWith(token, StringComparison.Ordinal)) count++;
            }
            return count;
        }

        private static string FindToolDir()
        {
            foreach (string config in new string[] { "Debug", "Release" })
            {
                string candidate = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory,
                    "..", "..", "..", "..", "..", "tools", "clipboard-lock-check", "bin", config, "net10.0-windows"));
                if (File.Exists(Path.Combine(candidate, "clipboard-lock-check.exe"))) return candidate;
            }
            return null;
        }

        // ---------- 逐页 UIA ----------

        private static void RunTabs(string dir, string label, int onlyTab)
        {
            Report.Info(string.Empty);
            Report.Info("=== 逐页 UIA 走查（" + label + "：" + dir + "）===");
            string exe = Path.Combine(dir, "StartUI4Demo.exe");
            int from = onlyTab >= 0 ? onlyTab : 0;
            int to = onlyTab >= 0 ? onlyTab : Cases.TabTitles.Length - 1;

            for (int tab = from; tab <= to; tab++)
            {
                Report.Info(string.Empty);
                Report.Info("--- 页 " + tab + "：" + Cases.TabTitles[tab] + " ---");
                try
                {
                    Cases.RunTabAt(tab, exe, _launchWaitMs, label);
                }
                catch (Exception ex)
                {
                    Check("T" + tab + "-launch", "页" + tab + " " + Cases.TabTitles[tab], false,
                          "该页跑完且无异常", ex.GetType().Name + ": " + ex.Message);
                }
            }
        }
    }
}
