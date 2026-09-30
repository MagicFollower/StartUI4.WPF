using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace StartUI4Controls
{
    /// <summary>
    /// 局部/每窗口主题作用域。在任意 <see cref="FrameworkElement"/>（通常是 Window、UserControl 或一张卡片）上
    /// 设置 <c>ui:UI4ThemeScope.Theme="dark"</c>，其整棵子树改用该主题，与全局 <see cref="UI4Theme"/> 互不干扰：
    /// <code>
    /// &lt;Border ui:UI4ThemeScope.Theme="dark"&gt; ... &lt;/Border&gt;
    /// </code>
    /// 两条生效通道（控件无需任何改动）：
    /// ① 向元素自身 <see cref="FrameworkElement.Resources"/> 的 MergedDictionaries 末位插入一份该主题的令牌字典
    ///   （<c>UI4.Color.X</c> / <c>UI4.Brush.X</c>），因此宿主 <c>{DynamicResource}</c> 与构造函数里
    ///   <c>SetResourceReference</c> 的控件（UI4CheckBox/UI4Radio/UI4TextBox/UI4PasswordBox 等）自动跟随；
    /// ② 仍走命令式刷新的控件（UI4Button/UI4ComboBox/UI4Menu…）在子树刷新时被同步换入该主题实例。
    /// 置空或未知键 = 撤销作用域，子树回到全局主题。
    /// </summary>
    public static class UI4ThemeScope
    {
        /// <summary>作用域主题键（<c>light</c> / <c>dark</c> / <c>highcontrast</c> / 自定义注册键），大小写不敏感。</summary>
        public static readonly DependencyProperty ThemeProperty = DependencyProperty.RegisterAttached(
            "Theme", typeof(string), typeof(UI4ThemeScope),
            new FrameworkPropertyMetadata(null, OnThemePropertyChanged));

        /// <summary>设置元素的主题作用域；<see cref="string.Empty"/> 或 null 撤销作用域。</summary>
        public static void SetTheme(DependencyObject element, string value)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            element.SetValue(ThemeProperty, value);
        }

        /// <summary>读取元素自身声明的主题作用域键（不含祖先的作用域）。</summary>
        public static string GetTheme(DependencyObject element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            return (string)element.GetValue(ThemeProperty);
        }

        private sealed class ScopeEntry
        {
            public readonly WeakReference<FrameworkElement> Element;
            public readonly string Key;
            public readonly ResourceDictionary Dictionary;

            public ScopeEntry(FrameworkElement element, string key, ResourceDictionary dictionary)
            {
                Element = new WeakReference<FrameworkElement>(element);
                Key = key;
                Dictionary = dictionary;
            }
        }

        private static readonly List<ScopeEntry> _scopes = new List<ScopeEntry>();

        private static void OnThemePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = d as FrameworkElement;
            if (element == null) return;

            // 空串 = 撤销作用域；未知键同样按撤销处理：XAML 里写错键名不应让宿主崩溃。
            string key = (e.NewValue as string ?? string.Empty).Trim();
            if (key.Length == 0 || !UI4Theme.IsRegistered(key)) key = null;

            ScopeEntry existing = Find(element);
            if (existing != null)
            {
                element.Resources.MergedDictionaries.Remove(existing.Dictionary);
                _scopes.Remove(existing);
            }

            if (key == null)
            {
                if (existing != null) RefreshSubtree(element, null);
                return;
            }

            UI4Theme theme = UI4Theme.InstanceOf(key);
            var dictionary = new ResourceDictionary();
            UI4Theme.WriteTokens(dictionary, theme);
            element.Resources.MergedDictionaries.Add(dictionary);
            _scopes.Add(new ScopeEntry(element, key, dictionary));
            RefreshSubtree(element, theme);
        }

        private static ScopeEntry Find(FrameworkElement element)
        {
            for (int i = 0; i < _scopes.Count; i++)
            {
                FrameworkElement target;
                if (_scopes[i].Element.TryGetTarget(out target) && ReferenceEquals(target, element))
                    return _scopes[i];
            }
            return null;
        }

        /// <summary>
        /// 从元素向上找最近的作用域键（<see cref="UI4Theme"/> 解析控件有效主题时调用）。
        /// 先走逻辑/模板父链（可穿过 Popup 与控件模板），再退回视觉父链。
        /// </summary>
        internal static string ResolveKey(DependencyObject element)
        {
            for (DependencyObject current = element; current != null; current = NextAncestor(current))
            {
                string key = current.GetValue(ThemeProperty) as string;
                if (!string.IsNullOrEmpty(key) && UI4Theme.IsRegistered(key)) return key;
            }
            return null;
        }

        private static DependencyObject NextAncestor(DependencyObject d)
        {
            FrameworkElement fe = d as FrameworkElement;
            if (fe != null && fe.Parent != null) return fe.Parent;
            FrameworkContentElement fce = d as FrameworkContentElement;
            if (fce != null && fce.Parent != null) return fce.Parent;
            return VisualTreeHelper.GetParent(d);
        }

        /// <summary>
        /// 全局主题刚被重写（含 <see cref="UI4Theme.SetAccent"/>）后同步各作用域字典的令牌值。
        /// 只改资源，不刷新命令式控件——那由 <see cref="UI4Theme"/> 的批量刷新按每个控件的有效主题处理。
        /// </summary>
        internal static void RefreshDefinitions()
        {
            if (_scopes.Count == 0) return;
            for (int i = _scopes.Count - 1; i >= 0; i--)
            {
                ScopeEntry entry = _scopes[i];
                FrameworkElement element;
                if (!entry.Element.TryGetTarget(out element))
                {
                    _scopes.RemoveAt(i);
                    continue;
                }
                UI4Theme.WriteTokens(entry.Dictionary, UI4Theme.InstanceOf(entry.Key));
            }
        }

        /// <summary>
        /// 作用域变更时刷新其子树内的命令式控件。<paramref name="theme"/> 为 null 表示回到全局主题。
        /// </summary>
        private static void RefreshSubtree(DependencyObject scopeRoot, UI4Theme theme)
        {
            // 撤销时 scopeKey 为 null：只刷新「向上已找不到任何作用域」的控件，外层/嵌套作用域各自保持不变
            string scopeKey = theme == null ? null : GetTheme(scopeRoot);
            RefreshDescendants(scopeRoot, theme, scopeKey);
        }

        private static void RefreshDescendants(DependencyObject node, UI4Theme theme, string scopeKey)
        {
            foreach (object child in LogicalTreeHelper.GetChildren(node))
            {
                DependencyObject dep = child as DependencyObject;
                if (dep != null) RefreshDescendants(dep, theme, scopeKey);
            }

            if (node is Visual)
            {
                int count = VisualTreeHelper.GetChildrenCount(node);
                for (int i = 0; i < count; i++)
                    RefreshDescendants(VisualTreeHelper.GetChild(node, i), theme, scopeKey);
            }

            FrameworkElement control = node as FrameworkElement;
            if (control == null) return;
            // 嵌套作用域由它自己负责：有效键与本作用域键不一致即跳过
            if (!KeyMatches(ResolveKey(control), scopeKey)) return;
            UI4Theme.RefreshControl(control, theme);
        }

        private static bool KeyMatches(string actual, string expected)
        {
            if (actual == null || expected == null) return actual == null && expected == null;
            return string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase);
        }
    }
}
