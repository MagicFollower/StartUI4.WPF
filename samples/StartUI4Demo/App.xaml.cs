using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using StartUI4Controls;

namespace StartUI4Demo
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 资源桥：把主题令牌写入 Application.Resources，供宿主 XAML 的 {DynamicResource UI4.Brush.X} 消费。
            UI4Theme.ApplyToApplication();

            DispatcherUnhandledException += OnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += OnDomainUnhandledException;
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Report("UI thread", e.Exception);
            e.Handled = true;
        }

        private void OnDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Report("AppDomain", e.ExceptionObject as Exception);
        }

        // 演示程序把异常写到日志并弹窗，便于逐个控件排查运行时问题。
        private static void Report(string source, Exception ex)
        {
            string text = ex == null ? "(null)" : ex.ToString();
            try
            {
                File.AppendAllText(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "demo-errors.log"),
                    string.Format("[{0:yyyy-MM-dd HH:mm:ss}] {1}{2}{3}{4}",
                        DateTime.Now, source, Environment.NewLine, text, Environment.NewLine),
                    Encoding.UTF8);
            }
            catch (IOException)
            {
            }

            MessageBox.Show(text, "StartUI4Demo 异常 (" + source + ")", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
