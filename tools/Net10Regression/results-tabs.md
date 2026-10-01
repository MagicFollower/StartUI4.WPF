# StartUI4 .NET 10 迁移回归结果

生成时间：2026-10-01 15:34:32

| 结果 | 用例 | 作用域 | 期望 | 实际 |
|---|---|---|---|---|
| PASS | S00 | 运行真实性 | harness 跑在 .NET 10 | Environment.Version=10.0.12 |
| PASS | T0-live | 页0 按钮与开关（net10） | 进程存活且主窗口在场 | pid=9364 |
| PASS | T0-title | 页0 按钮与开关（net10） | 窗口标题到位 | 标题=StartUI4.WPF for .NET 10 (LTS) — 控件演示 |
| PASS | T0-hdr | 页0 按钮与开关（net10） | 禁用态小节存在 | 命中「UI4Button 启动态 / 禁用态」 |
| PASS | T0-contrast-note | 页0 按钮与开关（net10） | 亮度自适应前景说明 + 复现对比度值在场 | 命中「前景色自动跟随背景亮度」 |
| PASS | T0-disabled | 页0 按钮与开关（net10） | 禁用态样本在场 | 命中「禁用（浅灰底）」 |
| PASS | T0-plain | 页0 按钮与开关（net10） | 状态栏显示被点击 | InvokePattern → 「UI4Button 被点击」 |
| FAIL | T0-gradient | 页0 按钮与开关（net10） | 状态栏带上按钮名 | 点击后未出现回显（触发方式 InvokePattern）；现场状态栏/回显=UI4Button \| UI4Button 启动态 / 禁用态 \| UI4CheckBox \| UI4Radio \| UI4Switch |
| PASS | T0-binding | 页0 按钮与开关（net10） | IsEnabled 绑定随勾选翻转 | 点击前 IsEnabled=True，点击后=False（鼠标点击=True） |
| PASS | T0-runtime | 页0 按钮与开关（net10） | 标题栏自检显示真运行时 | 回显=实际运行时：.NET 10.0.12 |
| PASS | T0-noerror | 页0 按钮与开关（net10） | 本页未写 demo-errors.log | StartDirectory=E:\Qoder灵感项目\StartUI4.WPF_net10\samples\StartUI4Demo\bin\Debug\net10.0-windows |
| PASS | T1-live | 页1 文本输入（net10） | 进程存活且主窗口在场 | pid=13260 |
| PASS | T1-title | 页1 文本输入（net10） | 窗口标题到位 | 标题=StartUI4.WPF for .NET 10 (LTS) — 控件演示 |
| PASS | T1-takeover-note | 页1 文本输入（net10） | 接管说明存在 | 命中「库在按键隧道阶段改走原生 Win32 剪贴板」 |
| PASS | T1-pwd-note | 页1 文本输入（net10） | 密码模式限制说明存在 | 命中「密码模式下复制与剪切被拒绝」 |
| PASS | T1-codeeditor | 页1 文本输入（net10） | AvalonEdit 说明 + 内置 C# 样例在场 | 命中「基于 AvalonEdit 的封装，构造里默认开启：C# 语法高亮、行号、自动换…」 |
| PASS | T1-echo-init | 页1 文本输入（net10） | 字符数回显初值 | 命中「字符数 = 5」 |
| PASS | T1-pwd-echo | 页1 文本输入（net10） | 密码回显初值 | 命中「Password = (空)」 |
| PASS | T1-snippet | 页1 文本输入（net10） | 代码编辑器里有 C# 片段 | 命中「// UI4CodeEditor 基于 AvalonEdit」 |
| PASS | T1-typing | 页1 文本输入（net10） | 输入框值被替换并回显字符数 | 回显=字符数 = 7 |
| PASS | T1-ctrl-c | 页1 文本输入（net10） | Ctrl+C 走库内原生通道，剪贴板里就是选中文本 | 剪贴板读回=[回归写入的文本] |
| PASS | T1-ctrl-x | 页1 文本输入（net10） | Ctrl+X 后原框被清空且内容进剪贴板 | 框内值="" 剪贴板=[回归写入的文本] |
| PASS | T1-ctrl-v | 页1 文本输入（net10） | Ctrl+V 把内容贴回 | 框内值="回归写入的文本" |
| PASS | T1-noerror | 页1 文本输入（net10） | 本页未写 demo-errors.log | StartDirectory=E:\Qoder灵感项目\StartUI4.WPF_net10\samples\StartUI4Demo\bin\Debug\net10.0-windows |
| PASS | T2-live | 页2 文本显示（net10） | 进程存活且主窗口在场 | pid=4432 |
| PASS | T2-title | 页2 文本显示（net10） | 窗口标题到位 | 标题=StartUI4.WPF for .NET 10 (LTS) — 控件演示 |
| PASS | T2-shadow | 页2 文本显示（net10） | 带阴影样本在场 | 命中「带阴影的文本」 |
| PASS | T2-gradient | 页2 文本显示（net10） | 渐变样本在场 | 命中「渐变文本」 |
| PASS | T2-init | 页2 文本显示（net10） | FlipText 初值 42 | 命中「42」 |
| PASS | T2-flip | 页2 文本显示（net10） | FlipText 换成 0..99 的随机数 | InvokePattern → 「随机翻转」 |
| PASS | T2-value | 页2 文本显示（net10） | 翻转后数字改变（0..99 且不为 42） | 初值节点=42 现值=7 |
| PASS | T2-noerror | 页2 文本显示（net10） | 本页未写 demo-errors.log | StartDirectory=E:\Qoder灵感项目\StartUI4.WPF_net10\samples\StartUI4Demo\bin\Debug\net10.0-windows |
| PASS | T3-live | 页3 选择器（net10） | 进程存活且主窗口在场 | pid=6304 |
| PASS | T3-title | 页3 选择器（net10） | 窗口标题到位 | 标题=StartUI4.WPF for .NET 10 (LTS) — 控件演示 |
| PASS | T3-combo | 页3 选择器（net10） | 下拉初值回显 | 命中「选中：选项 1」 |
| PASS | T3-slider | 页3 选择器（net10） | 滑杆初值回显 | 命中「UI4Slider Value = 50」 |
| PASS | T3-circle-note | 页3 选择器（net10） | CircleSlider 的 AddValueChanged 说明在场 | 命中「DependencyPropertyDescriptor.AddValueCha…」 |
| PASS | T3-range | 页3 选择器（net10） | UI4Slider ValueChanged 回显 30 | 回显=UI4Slider Value = 30 |
| PASS | T3-circle-watch | 页3 选择器（net10） | AddValueChanged 驱动的 CircleEcho 跟上动画终值 60 | 回显=UI4CircleSlider A Value = 60 |
| PASS | T3-noerror | 页3 选择器（net10） | 本页未写 demo-errors.log | StartDirectory=E:\Qoder灵感项目\StartUI4.WPF_net10\samples\StartUI4Demo\bin\Debug\net10.0-windows |
| PASS | T4-live | 页4 进度指示（net10） | 进程存活且主窗口在场 | pid=3948 |
| PASS | T4-title | 页4 进度指示（net10） | 窗口标题到位 | 标题=StartUI4.WPF for .NET 10 (LTS) — 控件演示 |
| PASS | T4-bar | 页4 进度指示（net10） | 进度条样本在场 | 命中「推进 Bar1」 |
| PASS | T4-ring | 页4 进度指示（net10） | 环形进度样本在场 | 命中「启停 Ring1」 |
| PASS | T4-advance | 页4 进度指示（net10） | 状态栏 Value = 60 | InvokePattern → 「UI4ProgressBar Value = 60」 |
| PASS | T4-indeterminate | 页4 进度指示（net10） | BarEcho 显示 True | InvokePattern → 「Bar1 IsIndeterminate = True」 |
| PASS | T4-ring-toggle | 页4 进度指示（net10） | RingEcho 显示 IsActive = False | InvokePattern → 「Ring1 IsActive = False」 |
| PASS | T4-ring-advance | 页4 进度指示（net10） | RingEcho 显示 Value = 10 | InvokePattern → 「Ring1 Value = 10」 |
| PASS | T4-noerror | 页4 进度指示（net10） | 本页未写 demo-errors.log | StartDirectory=E:\Qoder灵感项目\StartUI4.WPF_net10\samples\StartUI4Demo\bin\Debug\net10.0-windows |
| PASS | T5-live | 页5 列表与网格（net10） | 进程存活且主窗口在场 | pid=8960 |
| PASS | T5-title | 页5 列表与网格（net10） | 窗口标题到位 | 标题=StartUI4.WPF for .NET 10 (LTS) — 控件演示 |
| PASS | T5-adaptive-note | 页5 列表与网格（net10） | 自适应说明在场 | 命中「列数 = 可用宽度 ÷ 单元宽度 自动算出来」 |
| PASS | T5-cards | 页5 列表与网格（net10） | 绑定数据源渲染出卡片 | 命中「数据看板」 |
| PASS | T5-echo-init | 页5 列表与网格（net10） | GridEcho 有宽度与列数 | 回显=UI4GridView 实际宽度 = 1094 px，基准单元 230 px → 当前约 4 列 |
| PASS | T5-resize | 页5 列表与网格（net10） | 窗口变宽后 GridView 实际宽度与列数跟着变 | 窄时=1094 宽时=1314（回显=UI4GridView 实际宽度 = 1314 px，基准单元 230 px → 当前约 5 列） |
| PASS | T5-resize-back | 页5 列表与网格（net10） | 窗口变窄后列数回落 | 宽时=1314 窄时=760 |
| PASS | T5-noerror | 页5 列表与网格（net10） | 本页未写 demo-errors.log | StartDirectory=E:\Qoder灵感项目\StartUI4.WPF_net10\samples\StartUI4Demo\bin\Debug\net10.0-windows |
| PASS | T6-live | 页6 导航容器（net10） | 进程存活且主窗口在场 | pid=6080 |
| PASS | T6-title | 页6 导航容器（net10） | 窗口标题到位 | 标题=StartUI4.WPF for .NET 10 (LTS) — 控件演示 |
| PASS | T6-pivot | 页6 导航容器（net10） | UI4Pivot 表头在场 | 命中「首页」 |
| PASS | T6-tab | 页6 导航容器（net10） | UI4Tab 表头在场 | 命中「主页」 |
| FAIL | T6-nav | 页6 导航容器（net10） | UI4NavigationView 项在场 | 快照内未出现任何关键词（共 90 节点） |
| PASS | T6-scroll-note | 页6 导航容器（net10） | ScrollViewer 说明在场 | 命中「UI4ScrollViewer 美化了系统 ScrollViewer」 |
| FAIL | T6-tab-select | 页6 导航容器（net10） | 切到「文档」标签后该项进入选中态（内容渲染由 I13~I16 证明） | 切换动作=True IsSelected=False |
| PASS | T6-scroll-bottom | 页6 导航容器（net10） | ScrollEcho 底部文案 | InvokePattern → 「已平滑滚动到底部」 |
| PASS | T6-scroll-top | 页6 导航容器（net10） | ScrollEcho 顶部文案 | InvokePattern → 「已平滑滚动到顶部」 |
| PASS | T6-noerror | 页6 导航容器（net10） | 本页未写 demo-errors.log | StartDirectory=E:\Qoder灵感项目\StartUI4.WPF_net10\samples\StartUI4Demo\bin\Debug\net10.0-windows |
| PASS | T7-live | 页7 布局面板（net10） | 进程存活且主窗口在场 | pid=14676 |
| PASS | T7-title | 页7 布局面板（net10） | 窗口标题到位 | 标题=StartUI4.WPF for .NET 10 (LTS) — 控件演示 |
| PASS | T7-panel | 页7 布局面板（net10） | UI4Panel 样本在场 | 命中「悬停我会放大，点我上报事件」 |
| PASS | T7-panel2 | 页7 布局面板（net10） | 圆角 + 悬停描边样本在场 | 命中「圆角 + 悬停描边」 |
| PASS | T7-grid-button | 页7 布局面板（net10） | 状态栏带按钮名 | InvokePattern → 「UI4Button 被点击：按钮 1」 |
| PASS | T7-noerror | 页7 布局面板（net10） | 本页未写 demo-errors.log | StartDirectory=E:\Qoder灵感项目\StartUI4.WPF_net10\samples\StartUI4Demo\bin\Debug\net10.0-windows |
| PASS | T8-live | 页8 对话框（net10） | 进程存活且主窗口在场 | pid=6700 |
| PASS | T8-title | 页8 对话框（net10） | 窗口标题到位 | 标题=StartUI4.WPF for .NET 10 (LTS) — 控件演示 |
| PASS | T8-init | 页8 对话框（net10） | 返回值初值 | 命中「返回值：（尚未点击）」 |
| FAIL | T8-msgbox-ok | 页8 对话框（net10） | UI4MessageBox(OK) 弹出→点确定→返回 true | 模态命中=True 点击=True 回显=返回值：（尚未点击） |
| PASS | T8-msgbox-cancel | 页8 对话框（net10） | UI4MessageBox(OKCancel) 点取消→返回 false | 点击=True 回显=返回值：false |
| PASS | T8-colorpicker | 页8 对话框（net10） | UI4ColorPicker 以独立窗口弹出 | 窗口名=选择颜色 |
| PASS | T8-noerror | 页8 对话框（net10） | 本页未写 demo-errors.log | StartDirectory=E:\Qoder灵感项目\StartUI4.WPF_net10\samples\StartUI4Demo\bin\Debug\net10.0-windows |
| PASS | T9-live | 页9 菜单与托盘（net10） | 进程存活且主窗口在场 | pid=15804 |
| PASS | T9-title | 页9 菜单与托盘（net10） | 窗口标题到位 | 标题=StartUI4.WPF for .NET 10 (LTS) — 控件演示 |
| PASS | T9-menu | 页9 菜单与托盘（net10） | UI4Menu 菜单栏顶层项在场 | 命中「文件」 |
| PASS | T9-menu-expand | 页9 菜单与托盘（net10） | 展开后弹出层里出现「新建」 | mouse_event → 「新建」 |
| PASS | T9-menu-item | 页9 菜单与托盘（net10） | UI4Menu 弹出层含子项并可回到主窗 | 弹出层命中「新建」 |
| PASS | T9-ctx-note | 页9 菜单与托盘（net10） | 右键菜单说明在场 | 命中「复制/粘贴/删除/全选都是真操作」 |
| PASS | T9-tray-note | 页9 菜单与托盘（net10） | 托盘说明在场 | 命中「启用托盘图标：右键弹出真菜单」 |
| PASS | T9-lang | 页9 菜单与托盘（net10） | 多语言初值（zh-CN） | 命中「OK="确定"」 |
| PASS | T9-ctx-open | 页9 菜单与托盘（net10） | 右键弹出 UI4ContextMenu | 右键=True 菜单项命中=复制 |
| PASS | T9-ctx-copy | 页9 菜单与托盘（net10） | 菜单「复制」把选区写进剪贴板（UI4Clipboard 通道） | 剪贴板=[右键菜单回归文本] |
| FAIL | T9-ctx-status | 页9 菜单与托盘（net10） | 状态栏上报菜单动作 | 回显= |
| PASS | T9-lang-switch | 页9 菜单与托盘（net10） | 切到 en-US 后 UI4MultiLanguage 词条变化 | 点开下拉=True 点中 en-US=True 回显=OK="OK"  Cancel="Cancel"  Notice="Notice" |
| PASS | T9-lang-keys | 页9 菜单与托盘（net10） | 10 个语言键全量列出 | LangKeysLine=OK="OK"  Cancel="Cancel"  Notice="Notice" |
| PASS | T9-tray-on | 页9 菜单与托盘（net10） | 点开 UI4Switch 后状态栏显示托盘已启用 | 点击=True 回显=托盘图标已启用：右键弹菜单、双击有事件 |
| PASS | T9-tray-menu | 页9 菜单与托盘（net10） | 托盘菜单已建（状态栏文案点名右键弹菜单） | 回显=托盘图标已启用：右键弹菜单、双击有事件 |
| PASS | T9-tray-off | 页9 菜单与托盘（net10） | 再点一次托盘停用 | 点击=True 回显=托盘图标已停用 |
| PASS | T9-noerror | 页9 菜单与托盘（net10） | 本页未写 demo-errors.log | StartDirectory=E:\Qoder灵感项目\StartUI4.WPF_net10\samples\StartUI4Demo\bin\Debug\net10.0-windows |
| PASS | T10-live | 页10 局部主题（net10） | 进程存活且主窗口在场 | pid=4320 |
| PASS | T10-title | 页10 局部主题（net10） | 窗口标题到位 | 标题=StartUI4.WPF for .NET 10 (LTS) — 控件演示 |
| PASS | T10-scope-card | 页10 局部主题（net10） | 作用域卡片在场 | 命中「作用域卡片（Theme 见左上方下拉框）」 |
| PASS | T10-global-card | 页10 局部主题（net10） | 无作用域卡片在场 | 命中「无作用域（跟随全局主题）」 |
| PASS | T10-nested | 页10 局部主题（net10） | 嵌套作用域在场 | 命中「外层 = highcontrast」 |
| PASS | T10-status | 页10 局部主题（net10） | ScopeStatus 报出全局与作用域键 | 回显=全局主题 ResolvedMode=Light Key=light　　作用域卡片 Theme=dark |
| PASS | T10-footer | 页10 局部主题（net10） | 页脚主题行是事件驱动的（有值且含三个字段） | 页脚=主题: light　CurrentMode=Light　ResolvedMode=Light |
| PASS | T10-hc | 页10 局部主题（net10） | 状态栏 highcontrast + 页脚 Key=highcontrast | InvokePattern → 「全局主题 → highcontrast」 |
| PASS | T10-footer-hc | 页10 局部主题（net10） | 页脚跟着换成 highcontrast | 页脚=主题: highcontrast　CurrentMode=HighContrast　ResolvedMode=HighContrast |
| PASS | T10-dwm-hc | 页10 局部主题（net10） | 标题栏 DWM 深色标志=1（跨进程回读） | DWM flag=1（深色） |
| PASS | T10-scope-key | 页10 局部主题（net10） | 作用域键切到 highcontrast 后 ScopeStatus 跟着变 | 点开=True 点中=True 状态=全局主题 ResolvedMode=HighContrast Key=highcontrast　　作用域卡片 Theme=highcontrast |
| PASS | T10-scope-window | 页10 局部主题（net10） | ScopeWindow 以独立窗口出现 | InvokePattern → 「已打开异主题窗口」 |
| PASS | T10-scope-window | 页10 局部主题（net10） | 异主题窗口存在且标题栏独立染色 | 窗口=UI4ThemeScope 异主题窗口 |
| PASS | T10-system | 页10 局部主题（net10） | 状态栏显示跟随系统 | InvokePattern → 「全局主题 → 跟随系统」 |
| PASS | T10-noerror | 页10 局部主题（net10） | 本页未写 demo-errors.log | StartDirectory=E:\Qoder灵感项目\StartUI4.WPF_net10\samples\StartUI4Demo\bin\Debug\net10.0-windows |
| PASS | T11-live | 页11 剪贴板与原生交互（net10） | 进程存活且主窗口在场 | pid=16868 |
| PASS | T11-title | 页11 剪贴板与原生交互（net10） | 窗口标题到位 | 标题=StartUI4.WPF for .NET 10 (LTS) — 控件演示 |
| PASS | T11-probe-note | 页11 剪贴板与原生交互（net10） | ContainsText 的「不抢锁」说明在场 | 命中「只查格式是否存在，不 OpenClipboard」 |
| PASS | T11-write-note | 页11 剪贴板与原生交互（net10） | 写入的后台重试说明在场 | 命中「写入固定在后台线程重试」 |
| PASS | T11-internal-note | 页11 剪贴板与原生交互（net10） | 内部接管清单在场 | 命中「都在按键隧道阶段把复制/剪切/粘贴改走 UI4Clipboard」 |
| PASS | T11-probe | 页11 剪贴板与原生交互（net10） | ClipProbeResult 有结论 | InvokePattern → 「剪贴板里有 Unicode 文本」 |
| PASS | T11-copy-fixed | 页11 剪贴板与原生交互（net10） | ClipWriteResult 显示复制成功 | InvokePattern → 「复制成功：」 |
| PASS | T11-copy-verify | 页11 剪贴板与原生交互（net10） | 独立进程式读回：剪贴板内容 = 页面声称的 payload | 页面=StartUI4Demo 复制于 15:34:25 剪贴板=StartUI4Demo 复制于 15:34:25 |
| PASS | T11-read | 页11 剪贴板与原生交互（net10） | ClipReadResult 报出内容或字符数 | InvokePattern → 「字符」 |
| PASS | T11-ctx-open | 页11 剪贴板与原生交互（net10） | CtxOpenState = True | InvokePattern → 「IsOpen = True」 |
| PASS | T11-ctx-close | 页11 剪贴板与原生交互（net10） | CtxOpenState = False | InvokePattern → 「IsOpen = False」 |
| PASS | T11-noerror | 页11 剪贴板与原生交互（net10） | 本页未写 demo-errors.log | StartDirectory=E:\Qoder灵感项目\StartUI4.WPF_net10\samples\StartUI4Demo\bin\Debug\net10.0-windows |
| PASS | T12-live | 页12 主题与强调色（net10） | 进程存活且主窗口在场 | pid=996 |
| PASS | T12-title | 页12 主题与强调色（net10） | 窗口标题到位 | 标题=StartUI4.WPF for .NET 10 (LTS) — 控件演示 |
| PASS | T12-accent-note | 页12 主题与强调色（net10） | SetAccent 说明在场 | 命中「只改强调色：会写回当前主题定义并自动派生 AccentDark」 |
| PASS | T12-register-note | 页12 主题与强调色（net10） | Register 自定义主题说明在场 | 命中「克隆内置 light 后改 Key 与若干令牌」 |
| PASS | T12-persist-note | 页12 主题与强调色（net10） | 持久化刻意不碰注册表 | 命中「不往 HKCU」 |
| PASS | T12-titlebar-note | 页12 主题与强调色（net10） | 标题栏染色说明在场 | 命中「标题栏属于非客户区，只能经 DWM 染色」 |
| PASS | T12-accent-blue | 页12 主题与强调色（net10） | AccentEcho 报 #0078D4 | InvokePattern → 「强调色 → #0078D4」 |
| PASS | T12-accent-orange | 页12 主题与强调色（net10） | AccentEcho 报 #E67814 | InvokePattern → 「强调色 → #E67814」 |
| PASS | T12-accent-reset | 页12 主题与强调色（net10） | AccentEcho 报已恢复 | InvokePattern → 「已恢复内置主题定义」 |
| PASS | T12-register-ocean | 页12 主题与强调色（net10） | ThemeEcho 报已注册 ocean | InvokePattern → 「ocean」 |
| PASS | T12-theme-keys | 页12 主题与强调色（net10） | ThemeKeys 下拉列出新注册的键 ocean | 点开下拉=True 弹出层命中=ocean |
| PASS | T12-save | 页12 主题与强调色（net10） | PersistEcho 报已写入 + 文件落盘 | InvokePattern → 「已把」 |
| PASS | T12-persist-file | 页12 主题与强调色（net10） | demo-theme.json 落在程序目录（不写注册表） | 程序目录=E:\Qoder灵感项目\StartUI4.WPF_net10\samples\StartUI4Demo\bin\Debug\net10.0-windows |
| PASS | T12-apply-persisted | 页12 主题与强调色（net10） | PersistEcho 报读取结果 | InvokePattern → 「读取」 |
| PASS | T12-clear | 页12 主题与强调色（net10） | PersistEcho 报已删除 + 文件消失 | InvokePattern → 「已删除」 |
| PASS | T12-persist-clean | 页12 主题与强调色（net10） | 删除后程序目录不再残留 demo-theme.json | - |
| PASS | T12-titlebar-apply | 页12 主题与强调色（net10） | TitleBarEcho 报 Apply=True | InvokePattern → 「Apply(本窗口) = True」 |
| PASS | T12-dwm-apply | 页12 主题与强调色（net10） | Apply 后 DWM 深色标志可读 | DWM flag=0（亮色） |
| PASS | T12-titlebar-exempt | 页12 主题与强调色（net10） | TitleBarEcho 报已豁免 | InvokePattern → 「本窗口已豁免标题栏染色」 |
| PASS | T12-titlebar-restore | 页12 主题与强调色（net10） | TitleBarEcho 报恢复 | InvokePattern → 「本窗口恢复跟随主题染色」 |
| PASS | T12-noerror | 页12 主题与强调色（net10） | 本页未写 demo-errors.log | StartDirectory=E:\Qoder灵感项目\StartUI4.WPF_net10\samples\StartUI4Demo\bin\Debug\net10.0-windows |

汇总：140 PASS / 5 FAIL / 0 SKIP

```
=== StartUI4 .NET 10 迁移回归 harness ===
harness 自身运行时：.NET 10.0.12（Environment.Version=10.0.12，x64=True）
被测 Demo 目录：E:\Qoder灵感项目\StartUI4.WPF_net10\samples\StartUI4Demo\bin\Debug\net10.0-windows


=== 逐页 UIA 走查（net10：E:\Qoder灵感项目\StartUI4.WPF_net10\samples\StartUI4Demo\bin\Debug\net10.0-windows）===

--- 页 0：按钮与开关 ---

--- 页 1：文本输入 ---

--- 页 2：文本显示 ---

--- 页 3：选择器 ---

--- 页 4：进度指示 ---
T4 RangeValue 观察：UI4ProgressBar 可读值 NaN→NaN（读不到=控件未进 UIA，属无障碍待办，见 PORTING-NET10.md §5.9）

--- 页 5：列表与网格 ---

--- 页 6：导航容器 ---

--- 页 7：布局面板 ---

--- 页 8：对话框 ---

--- 页 9：菜单与托盘 ---

--- 页 10：局部主题 ---

--- 页 11：剪贴板与原生交互 ---

--- 页 12：主题与强调色 ---

=== 汇总：140 PASS / 5 FAIL / 0 SKIP ===

```
