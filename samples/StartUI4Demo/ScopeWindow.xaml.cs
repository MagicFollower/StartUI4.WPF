using System;
using System.Windows;
using StartUI4Controls;

namespace StartUI4Demo
{
    /// <summary>
    /// 整窗主题作用域演示：本窗口自身设置 <see cref="UI4ThemeScope.ThemeProperty"/>，
    /// 因此窗内所有控件改用该主题，与主窗口的全局主题互不影响。
    /// </summary>
    public partial class ScopeWindow : Window
    {
        private string _scopeKey;

        public ScopeWindow()
        {
            InitializeComponent();

            _scopeKey = OppositeOfGlobal();
            UI4ThemeScope.SetTheme(this, _scopeKey);
            UpdateInfo();

            // 主窗口切换全局主题时，本窗口的作用域保持不变，只需刷新说明文字。
            UI4Theme.ThemeChanged += delegate { UpdateInfo(); };
        }

        private static string OppositeOfGlobal()
        {
            string key = UI4Theme.ResolvedKey;
            if (string.Equals(key, "dark", StringComparison.OrdinalIgnoreCase)) return "light";
            return "dark";
        }

        private void ApplyScope(string key)
        {
            _scopeKey = key;
            UI4ThemeScope.SetTheme(this, key);
            UpdateInfo();
        }

        private void FlipScope_Click(object sender, RoutedEventArgs e)
        {
            ApplyScope(OppositeOfGlobal());
        }

        private void ClearScope_Click(object sender, RoutedEventArgs e)
        {
            ApplyScope(string.Empty);
        }

        private void HighContrastScope_Click(object sender, RoutedEventArgs e)
        {
            ApplyScope("highcontrast");
        }

        private void UpdateInfo()
        {
            ScopeInfo.Text = "本窗口作用域 Theme=" +
                             (string.IsNullOrEmpty(_scopeKey) ? "(none，跟随全局)" : _scopeKey);
            GlobalInfo.Text = "全局主题 ResolvedMode=" + UI4Theme.ResolvedMode + " Key=" + UI4Theme.ResolvedKey;
        }
    }
}
