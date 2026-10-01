using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Win32;

namespace StartUI4Controls
{
    /// <summary>
    /// 主题模式枚举。
    /// </summary>
    public enum UI4ThemeMode
    {
        /// <summary>亮色主题。</summary>
        Light,
        /// <summary>暗色主题。</summary>
        Dark,
        /// <summary>跟随系统（读 <c>AppsUseLightTheme</c> 并监听 <see cref="SystemEvents.UserPreferenceChanged"/>）。</summary>
        System,
        /// <summary>高对比度主题，等价于 <see cref="UI4Theme.Apply"/>(“highcontrast”)。</summary>
        HighContrast
    }

    /// <summary>
    /// 统一主题系统：由 <see cref="UI4ThemeDefinition"/> 数据驱动，提供语义化颜色令牌、
    /// 弱引用控件追踪、<see cref="ThemeChanged"/> 通知，以及把令牌桥接进
    /// <see cref="Application.Resources"/>（<c>UI4.Brush.X</c> / <c>UI4.Color.X</c>）供宿主 DynamicResource 消费。
    /// </summary>
    public class UI4Theme
    {
        // ────────────────────────────────────────────────────────
        //  静态 Current + 模式 + 通知
        // ────────────────────────────────────────────────────────

        private static UI4Theme _current;
        private static UI4ThemeMode _requestedMode = UI4ThemeMode.Light;
        private static UI4ThemeMode _resolvedMode = UI4ThemeMode.Light;
        private static string _resolvedKey = "light";
        private static int _themeVersion;

        private static readonly Dictionary<string, UI4ThemeDefinition> _definitions =
            new Dictionary<string, UI4ThemeDefinition>(StringComparer.OrdinalIgnoreCase)
            {
                { "light", UI4ThemeDefinition.Light() },
                { "dark", UI4ThemeDefinition.Dark() },
                { "highcontrast", UI4ThemeDefinition.HighContrast() },
            };

        /// <summary>每个主题键对应一个可共享的 <see cref="UI4Theme"/> 实例（全局与 <see cref="UI4ThemeScope"/> 复用，避免重复构建）。</summary>
        private static readonly Dictionary<string, UI4Theme> _instances =
            new Dictionary<string, UI4Theme>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// <see cref="Current"/> 的换入栈。作用域内刷新命令式控件时临时把 <see cref="Current"/> 换成作用域主题，
        /// 出栈即恢复；<b>只允许在 UI 线程同步使用</b>（刷新若跨 Dispatcher 派活，被派出去的代码会读到全局主题）。
        /// </summary>
        private static readonly Stack<UI4Theme> _themeStack = new Stack<UI4Theme>();

        /// <summary>主题代号，每次全局主题重写（切换 / 设置强调色）自增。供标题栏等外部染色器判断是否需要重染。</summary>
        internal static int ThemeVersion
        {
            get { return _themeVersion; }
        }

        static UI4Theme()
        {
            // 标题栏不属于 WPF 客户区，只能由 DWM 染色；主题引擎一被触碰就挂上窗口钩子
            UI4WindowTitleBar.Install();
        }

        /// <summary>获取当前主题实例。首次访问时自动初始化为 Light 主题。</summary>
        public static UI4Theme Current
        {
            get
            {
                if (_current == null)
                    _current = InstanceOf("light");
                return _current;
            }
            private set => _current = value;
        }

        /// <summary>获取最近一次 <see cref="SetTheme"/> 请求的模式（可能是 <see cref="UI4ThemeMode.System"/>）。</summary>
        public static UI4ThemeMode CurrentMode => _requestedMode;

        /// <summary>把 <see cref="UI4ThemeMode.System"/> 解析为实际的 Light / Dark / HighContrast；其余模式与 <see cref="CurrentMode"/> 相同。</summary>
        public static UI4ThemeMode ResolvedMode => _resolvedMode;

        /// <summary>当前生效的主题键（<c>light</c> / <c>dark</c> / <c>highcontrast</c> / 自定义）。</summary>
        public static string ResolvedKey => _resolvedKey;

        /// <summary>静态属性变化通知，供 XAML 绑定 <c>UI4Theme.CurrentMode</c> / <c>ResolvedMode</c> 等使用。</summary>
        public static event PropertyChangedEventHandler StaticPropertyChanged;

        /// <summary>主题切换事件。在全部已追踪控件刷新完成后触发。</summary>
        public static event EventHandler ThemeChanged;

        /// <summary>
        /// 设置当前主题模式。调用后所有已追踪控件自动刷新、资源字典同步更新；
        /// 解析后的实际主题与当前相同时不重复重建。
        /// </summary>
        public static void SetTheme(UI4ThemeMode mode)
        {
            bool requestedChanged = mode != _requestedMode;
            _requestedMode = mode;

            string key;
            if (mode == UI4ThemeMode.System)
            {
                EnableSystemFollow();
                key = ResolveSystemKey();
            }
            else
            {
                DisableSystemFollow();
                key = KeyForMode(mode);
            }

            if (requestedChanged)
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(CurrentMode)));

            UI4ThemeMode resolved = ModeForKey(key);
            ApplyResolved(key, resolved);
        }

        private static string KeyForMode(UI4ThemeMode mode)
        {
            switch (mode)
            {
                case UI4ThemeMode.Dark: return "dark";
                case UI4ThemeMode.HighContrast: return "highcontrast";
                default: return "light";
            }
        }

        /// <summary>主题键回报给 <see cref="UI4ThemeMode"/> 的形式；自定义键统一报 <see cref="UI4ThemeMode.Light"/>。</summary>
        private static UI4ThemeMode ModeForKey(string key)
        {
            if (string.Equals(key, "dark", StringComparison.OrdinalIgnoreCase)) return UI4ThemeMode.Dark;
            if (string.Equals(key, "highcontrast", StringComparison.OrdinalIgnoreCase)) return UI4ThemeMode.HighContrast;
            return UI4ThemeMode.Light;
        }

        /// <summary>按已注册主题键应用主题（如 "light" / "dark" / "highcontrast" / 自定义）。未知键返回 false。</summary>
        public static bool Apply(string definitionKey)
        {
            if (!_definitions.ContainsKey(definitionKey)) return false;
            UI4ThemeMode mode = ModeForKey(definitionKey);
            _requestedMode = mode;
            DisableSystemFollow();
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(CurrentMode)));
            ApplyResolved(definitionKey, mode);
            return true;
        }

        /// <summary>注册一份自定义主题定义（同名覆盖）。</summary>
        public static void Register(UI4ThemeDefinition definition)
        {
            if (definition == null) throw new ArgumentNullException(nameof(definition));
            _definitions[definition.Key] = definition;
            _instances.Remove(definition.Key);
        }

        /// <summary>主题键是否已注册。供 <see cref="UI4ThemeScope"/> 校验作用域键。</summary>
        internal static bool IsRegistered(string key) => key != null && _definitions.ContainsKey(key);

        /// <summary>已注册的主题键集合。</summary>
        public static IEnumerable<string> ThemeKeys => _definitions.Keys;

        private static void ApplyResolved(string key, UI4ThemeMode resolved)
        {
            bool resolvedChanged = !string.Equals(key, _resolvedKey, StringComparison.OrdinalIgnoreCase) || _current == null;
            _resolvedKey = key;
            _resolvedMode = resolved;
            if (resolvedChanged)
            {
                Current = InstanceOf(key);
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(ResolvedMode)));
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(ResolvedKey)));
                NotifyThemeChanged();
            }
            WriteToApplicationResources();
        }

        // ────────────────────────────────────────────────────────
        //  系统主题跟随
        // ────────────────────────────────────────────────────────

        private static bool _followingSystem;
        private static bool _followSystemHighContrast;

        /// <summary>
        /// <see cref="UI4ThemeMode.System"/> 下是否优先跟随系统高对比度（默认 false，保持既有亮/暗行为）。
        /// 设为 true 且当前处于 System 模式时立即重新解析主题。
        /// </summary>
        public static bool FollowSystemHighContrast
        {
            get { return _followSystemHighContrast; }
            set
            {
                if (_followSystemHighContrast == value) return;
                _followSystemHighContrast = value;
                if (_requestedMode == UI4ThemeMode.System) ReResolveSystemTheme();
            }
        }

        private static bool QuerySystemIsDark()
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
                {
                    if (key != null)
                    {
                        object v = key.GetValue("AppsUseLightTheme");
                        if (v is int && (int)v == 0) return true;
                    }
                }
            }
            catch { }
            return false;
        }

        /// <summary>解析「跟随系统」应使用的主题键：高对比度优先，其次按系统亮/暗设置。</summary>
        private static string ResolveSystemKey()
        {
            if (_followSystemHighContrast && SystemParameters.HighContrast) return "highcontrast";
            return QuerySystemIsDark() ? "dark" : "light";
        }

        private static void EnableSystemFollow()
        {
            if (_followingSystem) return;
            _followingSystem = true;
            SystemEvents.UserPreferenceChanged += OnUserPreferenceChanged;
        }

        private static void DisableSystemFollow()
        {
            if (!_followingSystem) return;
            _followingSystem = false;
            SystemEvents.UserPreferenceChanged -= OnUserPreferenceChanged;
        }

        /// <summary>显式停止系统跟随（如应用退出时）。</summary>
        public static void ReleaseSystemFollow() => DisableSystemFollow();

        private static void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category != UserPreferenceCategory.General &&
                e.Category != UserPreferenceCategory.Color &&
                e.Category != UserPreferenceCategory.Accessibility) return;
            if (_requestedMode != UI4ThemeMode.System) return;

            var dispatcher = Application.Current != null ? Application.Current.Dispatcher : null;
            if (dispatcher != null && !dispatcher.CheckAccess())
            {
                dispatcher.BeginInvoke(new Action(() => ReResolveSystemTheme()));
            }
            else
            {
                ReResolveSystemTheme();
            }
        }

        private static void ReResolveSystemTheme()
        {
            if (_requestedMode != UI4ThemeMode.System) return;
            string key = ResolveSystemKey();
            ApplyResolved(key, ModeForKey(key));
        }

        // ────────────────────────────────────────────────────────
        //  资源桥：令牌 → Application.Resources
        //  ────────────────────────────────────────────────────────

        /// <summary>把当前主题令牌写入 <see cref="Application.Resources"/>（幂等）。宿主可用 <c>{DynamicResource UI4.Brush.Surface}</c> 消费。</summary>
        public static void ApplyToApplication() => WriteToApplicationResources();

        private static void WriteToApplicationResources()
        {
            var app = Application.Current;
            if (app != null) WriteTokens(app.Resources, Current);
            // 全局主题变化后，各作用域字典按其自身键同步
            UI4ThemeScope.RefreshDefinitions();
        }

        /// <summary>
        /// 把一个主题实例的全部令牌写入资源表（<c>UI4.Color.X</c> / <c>UI4.Brush.X</c> + 三个别名）。
        /// <see cref="Application.Resources"/> 与 <see cref="UI4ThemeScope"/> 的作用域字典共用这份键集合，避免两处漂移。
        /// </summary>
        internal static void WriteTokens(IDictionary res, UI4Theme theme)
        {
            foreach (UI4ThemeToken token in Enum.GetValues(typeof(UI4ThemeToken)))
            {
                Color c = theme.ColorOf(token);
                res["UI4.Color." + token] = c;
                res["UI4.Brush." + token] = CreateFrozen(c);
            }
            // 常用别名，方便宿主书写
            res["UI4.Brush.Text"] = theme.TextForegroundBrush;
            res["UI4.Brush.Border"] = theme.BorderNormalBrush;
            res["UI4.Brush.Accent"] = theme.AccentBrush;
        }

        // ────────────────────────────────────────────────────────
        //  持久化（默认关闭）
        // ────────────────────────────────────────────────────────

        /// <summary>主题持久化实现；为 null 时不持久化（默认）。</summary>
        public static IThemePersistence Persistence { get; set; }

        /// <summary><see cref="Save"/> 后触发。</summary>
        public static event EventHandler<UI4ThemeMode> ThemeSaved;
        /// <summary><see cref="ApplyPersisted"/> 读取到已保存模式、应用前触发。</summary>
        public static event EventHandler<UI4ThemeMode> ThemeLoading;

        /// <summary>把当前请求模式写入 <see cref="Persistence"/>（未配置持久化时仅触发事件）。</summary>
        public static void Save()
        {
            if (Persistence != null) Persistence.Save(_requestedMode);
            ThemeSaved?.Invoke(null, _requestedMode);
        }

        /// <summary>从 <see cref="Persistence"/> 读取并应用；无配置或无已存值时返回 false。</summary>
        public static bool ApplyPersisted()
        {
            if (Persistence == null) return false;
            UI4ThemeMode? mode = Persistence.Load();
            if (!mode.HasValue) return false;
            ThemeLoading?.Invoke(null, mode.Value);
            SetTheme(mode.Value);
            return true;
        }

        // ────────────────────────────────────────────────────────
        //  弱引用追踪机制
        // ────────────────────────────────────────────────────────

        private sealed class TrackedEntry
        {
            public readonly WeakReference<FrameworkElement> Reference;
            public int SyncedVersion;
            /// <summary>该控件最近一次刷新时所「看到」的主题实例；处于 <see cref="UI4ThemeScope"/> 内时与全局实例不同。null 表示登记时全局主题尚未初始化。</summary>
            public UI4Theme AppliedTheme;
            public TrackedEntry(FrameworkElement control, int syncedVersion)
            {
                Reference = new WeakReference<FrameworkElement>(control);
                SyncedVersion = syncedVersion;
                AppliedTheme = _current;
            }
        }

        private static readonly List<TrackedEntry> _trackedControls = new List<TrackedEntry>();
        private static readonly object _trackLock = new object();

        /// <summary>注册控件实例以便主题切换时自动刷新（幂等；重复调用无副作用）。</summary>
        internal static void TrackControl(FrameworkElement control)
        {
            lock (_trackLock)
            {
                for (int i = 0; i < _trackedControls.Count; i++)
                {
                    FrameworkElement existing;
                    if (_trackedControls[i].Reference.TryGetTarget(out existing) && existing == control)
                        return;
                }
                var entry = new TrackedEntry(control, _themeVersion);
                _trackedControls.Add(entry);
                control.Loaded += (s, e) => OnTrackedControlLoaded(entry);
            }
        }

        /// <summary>取消追踪。控件保持追踪时不会泄漏（弱引用），卸载后仍会在重新加载时补齐主题。</summary>
        internal static void UntrackControl(FrameworkElement control)
        {
            lock (_trackLock)
            {
                _trackedControls.RemoveAll(w =>
                {
                    FrameworkElement t;
                    return !w.Reference.TryGetTarget(out t) || t == control;
                });
            }
        }

        private static void OnTrackedControlLoaded(TrackedEntry entry)
        {
            FrameworkElement control;
            if (!entry.Reference.TryGetTarget(out control))
                return;
            // 标题栏在窗口内容加载时按有效主题补染（同一主题代号内只做一次）
            UI4WindowTitleBar.NotifyContentLoaded(control);
            IThemeAware aware = control as IThemeAware;
            if (aware == null) return;

            UI4Theme effective;
            lock (_trackLock)
            {
                // 失联期间主题若已切换（或控件落在了某个 UI4ThemeScope 内），重新挂载时补齐到「有效主题」
                bool versionStale = entry.SyncedVersion != _themeVersion;
                effective = EffectiveThemeFor(control);
                bool themeStale = entry.AppliedTheme != null && !ReferenceEquals(effective, entry.AppliedTheme);
                if (!versionStale && !themeStale)
                    return;
                entry.SyncedVersion = _themeVersion;
                entry.AppliedTheme = effective;
            }
            RefreshAware(aware, effective);
        }

        /// <summary>按「有效主题」刷新一个命令式控件：作用域主题通过 <see cref="UseTheme"/> 同步换入后刷新。</summary>
        private static void RefreshAware(IThemeAware aware, UI4Theme effective)
        {
            if (effective == null || ReferenceEquals(effective, _current))
            {
                aware.OnThemeChanged();
                return;
            }
            using (UseTheme(effective)) aware.OnThemeChanged();
        }

        /// <summary>
        /// 由 <see cref="UI4ThemeScope"/> 调用：按指定有效主题（null 表示回到全局）刷新单个控件，
        /// 并把追踪表的「上次应用主题」同步为该主题，避免下次 Loaded 重复重建。
        /// </summary>
        internal static void RefreshControl(FrameworkElement control, UI4Theme effective)
        {
            var aware = control as IThemeAware;
            if (aware == null) return;
            UI4Theme applied = effective ?? _current;
            RefreshAware(aware, applied);
            NoteApplied(control, applied);
        }

        private static void NoteApplied(FrameworkElement control, UI4Theme theme)
        {
            lock (_trackLock)
            {
                foreach (var entry in _trackedControls)
                {
                    FrameworkElement target;
                    if (entry.Reference.TryGetTarget(out target) && target == control)
                    {
                        entry.SyncedVersion = _themeVersion;
                        entry.AppliedTheme = theme;
                        return;
                    }
                }
            }
        }

        private static void NotifyThemeChanged()
        {
            List<TrackedEntry> entries;
            lock (_trackLock)
            {
                _themeVersion++;
                _trackedControls.RemoveAll(w =>
                {
                    FrameworkElement t;
                    return !w.Reference.TryGetTarget(out t) || !t.IsLoaded;
                });
                entries = new List<TrackedEntry>(_trackedControls.Count);
                foreach (var wr in _trackedControls)
                {
                    FrameworkElement t;
                    if (wr.Reference.TryGetTarget(out t) && t.IsLoaded)
                        entries.Add(wr);
                }
            }

            // 使用 Dispatcher 批量调度，合并同一帧内的刷新；ThemeChanged 在全部控件刷新完成后触发
            if (Application.Current != null)
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
                {
                    RefreshEntries(entries);
                    ThemeChanged?.Invoke(null, EventArgs.Empty);
                }));
            }
            else
            {
                RefreshEntries(entries);
                ThemeChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        private static void RefreshEntries(List<TrackedEntry> entries)
        {
            foreach (var entry in entries)
            {
                FrameworkElement ctrl;
                if (!entry.Reference.TryGetTarget(out ctrl) || !ctrl.IsLoaded)
                    continue;
                entry.SyncedVersion = _themeVersion;
                var aware = ctrl as IThemeAware;
                if (aware == null) continue;
                var effective = EffectiveThemeFor(ctrl);
                entry.AppliedTheme = effective;
                RefreshAware(aware, effective);
            }
        }

        // ────────────────────────────────────────────────────────
        //  实例：令牌数据 + 冻结画刷
        // ────────────────────────────────────────────────────────

        private readonly Dictionary<UI4ThemeToken, Color> _colors = new Dictionary<UI4ThemeToken, Color>();
        private UI4ThemeDefinition _def;

        /// <summary>取令牌颜色。</summary>
        public Color ColorOf(UI4ThemeToken token) => _colors[token];

        /// <summary>取令牌对应的冻结画刷。</summary>
        public SolidColorBrush BrushOf(UI4ThemeToken token) => CreateFrozen(_colors[token]);

        // 颜色令牌（Color 类型）——全部由 _colors 驱动，保持既有公开属性名不变。

        /// <summary>主题强调色。Light: #0078D4, Dark: #0099FF</summary>
        public Color AccentColor => _colors[UI4ThemeToken.Accent];
        /// <summary>深色强调色（悬停/焦点）。Light: #0066B5, Dark: #0078D4</summary>
        public Color AccentDarkColor => _colors[UI4ThemeToken.AccentDark];
        /// <summary>渐变结束色。Light: #9333EA, Dark: #6428C8</summary>
        public Color AccentEndColor => _colors[UI4ThemeToken.AccentEnd];
        /// <summary>主文本前景色。Light: #1E1E1E, Dark: #E6E6E6</summary>
        public Color TextForegroundColor => _colors[UI4ThemeToken.TextForeground];
        /// <summary>次要文本色。Light: #000000, Dark: #CCCCCC</summary>
        public Color TextSecondaryColor => _colors[UI4ThemeToken.TextSecondary];
        /// <summary>窗口/面板背景色。Light: #FFFFFF, Dark: #202026</summary>
        public Color BackgroundColor => _colors[UI4ThemeToken.Background];
        /// <summary>控件表面背景色（编辑区、下拉弹出等）。Light: #FFFFFF, Dark: #282830</summary>
        public Color SurfaceColor => _colors[UI4ThemeToken.Surface];
        /// <summary>默认边框色。Light: #C8C8DC, Dark: #3C3C4B</summary>
        public Color BorderNormalColor => _colors[UI4ThemeToken.BorderNormal];
        /// <summary>次要边框色（CheckBox/Radio）。Light: #B4B4C8, Dark: #36364A</summary>
        public Color BorderSecondaryColor => _colors[UI4ThemeToken.BorderSecondary];
        /// <summary>悬停边框色。Light: #0078D4, Dark: #0099FF</summary>
        public Color BorderHoverColor => _colors[UI4ThemeToken.BorderHover];
        /// <summary>焦点边框色。Light: #0066B5, Dark: #0078D4</summary>
        public Color BorderFocusColor => _colors[UI4ThemeToken.BorderFocus];
        /// <summary>占位符前景色。Light: LightGray, Dark: #808080</summary>
        public Color PlaceholderColor => _colors[UI4ThemeToken.Placeholder];
        /// <summary>悬停叠加色。Light: Argb(20,0,0,0), Dark: Argb(20,255,255,255)</summary>
        public Color HoverOverlayColor => _colors[UI4ThemeToken.HoverOverlay];
        /// <summary>选中叠加色。Light: Argb(10,0,0,0), Dark: Argb(10,255,255,255)</summary>
        public Color SelectedOverlayColor => _colors[UI4ThemeToken.SelectedOverlay];
        /// <summary>轨道背景色。Light: Argb(10,0,0,0), Dark: Argb(20,255,255,255)</summary>
        public Color TrackBackgroundColor => _colors[UI4ThemeToken.TrackBackground];
        /// <summary>勾选/选中背景色。Light: #0066B5, Dark: #008CD2</summary>
        public Color CheckBackgroundColor => _colors[UI4ThemeToken.CheckBackground];
        /// <summary>图标色。Light: #78788C, Dark: #A0A0B4</summary>
        public Color IconColor => _colors[UI4ThemeToken.Icon];
        /// <summary>图标悬停色。Light: #3C3C50, Dark: #C8C8DC</summary>
        public Color IconHoverColor => _colors[UI4ThemeToken.IconHover];
        /// <summary>面板边框色。Light: Argb(60,120,140,200), Dark: Argb(60,100,120,180)</summary>
        public Color PanelBorderColor => _colors[UI4ThemeToken.PanelBorder];
        /// <summary>关闭状态背景色（Switch）。Light: #C8C8D2, Dark: #3C3C46</summary>
        public Color OffBackgroundColor => _colors[UI4ThemeToken.OffBackground];
        /// <summary>菜单栏背景色。Light: #F8F8F8, Dark: #2D2D32</summary>
        public Color MenuBackgroundColor => _colors[UI4ThemeToken.MenuBackground];
        /// <summary>列表选中/按下色。Light: #2563EB, Dark: #3B7BFF</summary>
        public Color ListSelectedColor => _colors[UI4ThemeToken.ListSelected];
        /// <summary>表头背景色。Light: #F5F5F5, Dark: #2A2A30</summary>
        public Color HeaderBackgroundColor => _colors[UI4ThemeToken.HeaderBackground];
        /// <summary>表头前景色。Light: #1E1E1E, Dark: #E6E6E6</summary>
        public Color HeaderForegroundColor => _colors[UI4ThemeToken.HeaderForeground];
        /// <summary>行悬停背景色。Light: #F0F0F5, Dark: #2E2E38</summary>
        public Color RowHoverBackgroundColor => _colors[UI4ThemeToken.RowHoverBackground];
        /// <summary>行选中背景色。Light: #D3D3D3, Dark: #3A3A48</summary>
        public Color RowSelectedBackgroundColor => _colors[UI4ThemeToken.RowSelectedBackground];
        /// <summary>网格线色。Light: #E6E6EB, Dark: #3A3A45</summary>
        public Color GridLineColor => _colors[UI4ThemeToken.GridLine];
        /// <summary>进度条渐变起始色。Light: #0096E6, Dark: #00AAFF</summary>
        public Color ProgressStartColor => _colors[UI4ThemeToken.ProgressStart];
        /// <summary>未勾选复选框背景。Light: LightGray, Dark: #505058</summary>
        public Color CheckBoxUncheckedBackground => _colors[UI4ThemeToken.CheckBoxUnchecked];
        /// <summary>悬停边框微调色。Light: #8C8CAA, Dark: #606078</summary>
        public Color HoverBorderColorLight => _colors[UI4ThemeToken.HoverBorderColorLight];

        // 冻结 Brush 便捷属性

        /// <summary>强调色冻结画刷。</summary>
        public SolidColorBrush AccentBrush { get; private set; }
        /// <summary>深色强调色冻结画刷。</summary>
        public SolidColorBrush AccentDarkBrush { get; private set; }
        /// <summary>背景色冻结画刷。</summary>
        public SolidColorBrush BackgroundBrush { get; private set; }
        /// <summary>表面色冻结画刷。</summary>
        public SolidColorBrush SurfaceBrush { get; private set; }
        /// <summary>主文本冻结画刷。</summary>
        public SolidColorBrush TextForegroundBrush { get; private set; }
        /// <summary>白色冻结画刷（按钮前景等）。</summary>
        public SolidColorBrush OnWhiteBrush { get; private set; }
        /// <summary>占位符冻结画刷。</summary>
        public SolidColorBrush PlaceholderBrush { get; private set; }
        /// <summary>默认边框冻结画刷。</summary>
        public SolidColorBrush BorderNormalBrush { get; private set; }
        /// <summary>菜单背景冻结画刷。</summary>
        public SolidColorBrush MenuBackgroundBrush { get; private set; }
        /// <summary>悬停叠加冻结画刷。</summary>
        public SolidColorBrush HoverOverlayBrush { get; private set; }
        /// <summary>图标色冻结画刷。</summary>
        public SolidColorBrush IconBrush { get; private set; }

        internal static UI4Theme FromDefinition(UI4ThemeDefinition def)
        {
            var t = new UI4Theme();
            t._def = def;
            foreach (UI4ThemeToken token in Enum.GetValues(typeof(UI4ThemeToken)))
                t._colors[token] = def.GetColor(token);
            t.BuildFrozenBrushes();
            return t;
        }

        /// <summary>
        /// 取（并缓存）主题键对应的实例；未注册键返回 null。
        /// 全局主题与所有同键作用域共用同一实例，因此 <see cref="SetAccent"/> 之类的改动天然对两侧同时生效。
        /// </summary>
        internal static UI4Theme InstanceOf(string key)
        {
            if (key == null) return null;
            UI4Theme theme;
            if (_instances.TryGetValue(key, out theme)) return theme;
            UI4ThemeDefinition def;
            if (!_definitions.TryGetValue(key, out def)) return null;
            theme = FromDefinition(def);
            _instances[key] = theme;
            return theme;
        }

        /// <summary>当前全局主题实例（不含作用域解析）。</summary>
        internal static UI4Theme GlobalTheme => Current;

        /// <summary>
        /// 同步把 <see cref="Current"/> 换成指定主题，返回的 <see cref="IDisposable"/> 释放时恢复。
        /// 只能用于包裹**同步**的命令式刷新：若被包裹的代码把活儿派给 Dispatcher，派出去的部分会读到全局主题。
        /// </summary>
        internal static IDisposable UseTheme(UI4Theme theme)
        {
            _themeStack.Push(_current);
            _current = theme;
            return new ThemeRestorer();
        }

        private sealed class ThemeRestorer : IDisposable
        {
            private bool _done;
            public void Dispose()
            {
                if (_done) return;
                _done = true;
                if (_themeStack.Count > 0) _current = _themeStack.Pop();
            }
        }

        /// <summary>
        /// 解析某个元素在 <see cref="UI4ThemeScope"/> 下应当使用的主题实例；
        /// 祖先上没有作用域时返回 null（表示「跟随全局」）。
        /// </summary>
        internal static UI4Theme EffectiveThemeFor(DependencyObject element)
        {
            string key = UI4ThemeScope.ResolveKey(element);
            if (key == null) return null;
            return InstanceOf(key);
        }

        /// <summary>
        /// 覆盖强调色：自动派生 <see cref="AccentDarkColor"/>（约 85% 亮度）。
        /// 触发一次主题刷新与资源字典更新；同时写回当前主题的 <see cref="UI4ThemeDefinition"/>，
        /// 使显式声明该键的 <see cref="UI4ThemeScope"/> 也继承覆盖。
        /// </summary>
        public static void SetAccent(Color accent)
        {
            var t = Current;
            Color accentDark = Darken(accent, 0.85f);
            t._colors[UI4ThemeToken.Accent] = accent;
            t._colors[UI4ThemeToken.AccentDark] = accentDark;
            t.BuildFrozenBrushes();

            UI4ThemeDefinition def;
            if (_definitions.TryGetValue(_resolvedKey, out def))
                def.With(UI4ThemeToken.Accent, accent).With(UI4ThemeToken.AccentDark, accentDark);

            NotifyThemeChanged();
            WriteToApplicationResources();
        }

        private void BuildFrozenBrushes()
        {
            AccentBrush = CreateFrozen(AccentColor);
            AccentDarkBrush = CreateFrozen(AccentDarkColor);
            BackgroundBrush = CreateFrozen(BackgroundColor);
            SurfaceBrush = CreateFrozen(SurfaceColor);
            TextForegroundBrush = CreateFrozen(TextForegroundColor);
            OnWhiteBrush = CreateFrozen(Colors.White);
            PlaceholderBrush = CreateFrozen(PlaceholderColor);
            BorderNormalBrush = CreateFrozen(BorderNormalColor);
            MenuBackgroundBrush = CreateFrozen(MenuBackgroundColor);
            HoverOverlayBrush = CreateFrozen(HoverOverlayColor);
            IconBrush = CreateFrozen(IconColor);
        }

        private static SolidColorBrush CreateFrozen(Color c)
        {
            var b = new SolidColorBrush(c);
            b.Freeze();
            return b;
        }

        private static Color Darken(Color c, float factor)
        {
            return Color.FromArgb(c.A,
                (byte)(c.R * factor),
                (byte)(c.G * factor),
                (byte)(c.B * factor));
        }
    }

    /// <summary>
    /// 实现此接口的控件将在主题切换时自动收到通知并刷新。
    /// </summary>
    internal interface IThemeAware
    {
        void OnThemeChanged();
    }
}
