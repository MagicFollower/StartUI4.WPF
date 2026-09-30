# 第 10 章 XAML 编写规范与命名约定

> 本章解决：把宿主 XAML 的书写纪律固定下来 —— 前缀、主题色消费方式、命名、图标、`new`-隐藏属性的规避写法，以及常见报错到真因的对照。
> 读者：AI Agent（要在本项目之外新建/续写 WPF 业务代码）
> 前置章节：第 9 章（骨架与页文件）、第 12 章（为什么必须 `DynamicResource` 的机制解释）

## 10.1 目标与验收

1. 全工程 XAML 里主题相关颜色**只出现** `DynamicResource` + `UI4.Color.*` / `UI4.Brush.*` 两类键；`grep` 搜不到 `StaticResource UI4.`，也搜不到硬编码 `#RRGGBB` 的主题色。
2. 所有需要被脚本定位的元素都有 `x:Name`，且事件处理命名统一为 `<Element>_<Event>`。
3. 只用仓库里存在的成员名：写每个属性前先在 `src/StartUI4Controls/` 里 `grep` 一次（本库有四条「文档说谎」，见 10.3.8）。
4. 一页 XAML 能独立编译：自带 `xmlns:ui`，不依赖父文件的命名空间。

## 10.2 源码依据

- `samples/StartUI4Demo/MainWindow.xaml:4`、`ScopeWindow.xaml:4` —— 全工程统一前缀写法 `xmlns:ui="clr-namespace:StartUI4Controls;assembly=StartUI4Controls"`。
- `src/StartUI4Controls/AssemblyInfo.cs` —— 只有 `[assembly: ThemeInfo(...)]`，**没有 `XmlnsDefinition`** ⇒ `assembly=` 段不可省略。
- `src/StartUI4Controls/UI4Theme.cs:297-309` —— 资源键生成规则：30 个 `UI4.Color.<Token>` + 30 个 `UI4.Brush.<Token>` + 三个别名 `UI4.Brush.Text` / `UI4.Brush.Border` / `UI4.Brush.Accent`。
- `src/StartUI4Controls/UI4ThemeToken.cs:7-39` —— 令牌枚举原文（键名逐字取自枚举成员名，大小写敏感）。
- `src/StartUI4Controls/UI4TextBlock.cs:50-61`（`Foreground`）、`:116-122`（`Padding`）、`:129-135`（`FontSize`）、`:142-148`（`FontWeight`）四处 `new` 注册的 DP；`:349-350`、`:354-357` 模板内部对这几个新 DP 的绑定。
- `src/StartUI4Controls/UI4Panel.cs:46-56`（`BorderColor` 是 `Color`）、`:172-173`（`BorderThickness` 重写默认值为 1 并触发重建）、`:220-224`（内层 `Border.BorderBrush` 只由 `BorderColor` 生成）。
- `src/StartUI4Controls/UI4CheckBox.cs:43-56` —— `BoxCornerRadiusProperty = CornerRadiusProperty` 的 `[Obsolete]` 别名。
- `src/StartUI4Controls/UI4TabControl.cs:19/:32-34`、`UI4NavigationView.cs:87` —— `TextIcon` / `TextIconFontFamily`（`FontFamily` 类型）；`UI4Menu.cs:296/:307/:318` —— 菜单侧叫 `TextIcon` / `IconFontFamily` / `IconFontSize`（**不同名**）。
- `samples/StartUI4Demo/App.xaml:7-18` —— `SectionTitle` / `SectionHint` 两个样板样式（`Setter` 值用 `DynamicResource`）。
- `MainWindow.xaml:21-23`、`:302`、`:316`、`:476`、`ScopeWindow.xaml:10` —— 图标码位与容器级文字色的实例写法。
- `MainWindow.xaml.cs:39`、`:107`、`:157/:172`、`:218`、`:262`、`:278` —— `<Element>_<Event>` 命名实例。

## 10.3 正文

### 10.3.1 xmlns 前缀约定

只认一种写法，前缀名统一为 `ui`：

```xml
xmlns:ui="clr-namespace:StartUI4Controls;assembly=StartUI4Controls"
```

- 库没有 `XmlnsDefinition` 特性，所以**去掉 `;assembly=StartUI4Controls` 一定失败**（同程序集内才可省）。
- 每个用到 UI4 类型的 XAML 文件都要重复这一行；命名空间不继承父文件之外的范围，UserControl 也必须自带。
- 你的业务前缀另起（如 `xmlns:local="clr-namespace:MyApp.Pages"`），不要复用 `ui`。
- 宿主程序集名与库不同，所以 `x:Class` 前缀是你的命名空间，而 `xmlns:ui` 的 `clr-namespace` 永远是 `StartUI4Controls`。

### 10.3.2 主题色一律 DynamicResource

规则：**凡颜色/画刷，出现在元素属性或 `Style` 的 `Setter.Value` 上，一律 `DynamicResource`。**

```xml
<!-- 正确 -->
<Border Background="{DynamicResource UI4.Brush.Surface}"
        BorderBrush="{DynamicResource UI4.Brush.BorderNormal}" BorderThickness="0,0,0,1"/>

<!-- 错误：解析期快照，切主题不更新 -->
<Border Background="{StaticResource UI4.Brush.Surface}" .../>

<!-- 错误：硬编码，等于永久脱离主题 -->
<Border Background="#FFFFFF" .../>
```

原因是资源桥**原地覆盖同一批键**（`UI4Theme.WriteTokens` 对同一个 `IDictionary` 反复赋同名键），`StaticResource` 在解析期把当时的值取走后就与字典脱钩。机制细节见第 12 章。

`Style` 内可以且应该用 `DynamicResource`（`App.xaml:10`、`:15` 就是这么写的），`Style` 本身用 `StaticResource` 引用没问题：

```xml
<TextBlock Text="订单列表" Style="{StaticResource SectionTitle}"/>
```

反过来，主题令牌键**绝不能**用 `StaticResource`，除非你确定该元素所在整棵子树永不切主题。

### 10.3.3 可用资源键清单

30 个令牌 → 每个有两个键；别名只有 Brush 形式：

| 类别 | 键 |
|---|---|
| 颜色 | `UI4.Color.Accent` `UI4.Color.AccentDark` `UI4.Color.AccentEnd` `UI4.Color.TextForeground` `UI4.Color.TextSecondary` `UI4.Color.Background` `UI4.Color.Surface` `UI4.Color.BorderNormal` `UI4.Color.BorderSecondary` `UI4.Color.BorderHover` `UI4.Color.BorderFocus` `UI4.Color.Placeholder` `UI4.Color.HoverOverlay` `UI4.Color.SelectedOverlay` `UI4.Color.TrackBackground` `UI4.Color.CheckBackground` `UI4.Color.Icon` `UI4.Color.IconHover` `UI4.Color.PanelBorder` `UI4.Color.OffBackground` `UI4.Color.MenuBackground` `UI4.Color.ListSelected` `UI4.Color.HeaderBackground` `UI4.Color.HeaderForeground` `UI4.Color.RowHoverBackground` `UI4.Color.RowSelectedBackground` `UI4.Color.GridLine` `UI4.Color.ProgressStart` `UI4.Color.CheckBoxUnchecked` `UI4.Color.HoverBorderColorLight` |
| 画刷 | 上述每一项把 `Color` 换成 `Brush` 即存在（`UI4.Brush.TextForeground` …） |
| 别名 | **仅** `UI4.Brush.Text`（= `TextForeground`）、`UI4.Brush.Border`（= `BorderNormal`）、`UI4.Brush.Accent`（= `Accent`） |

两条硬性推论：

- **别名没有 Color 形式**：`UI4.Color.Text`、`UI4.Color.Border` 不存在。给 `Color` 类型 DP（`UI4Pivot.ItemForeground`、`UI4TextBox.FocusBorderColor`、`UI4Panel.BorderColor`）赋值时必须写全名 `UI4.Color.TextForeground` / `UI4.Color.BorderNormal`。
- 键名大小写敏感、逐字取自枚举成员名。`UI4.Brush.TextForeground` 正确，`UI4.Brush.TextForegroundBrush`、`UI4.Brush.Foreground` 都不存在。

`Color` 还是 `Brush` 由**目标 DP 的类型**决定，不由你的偏好决定：`Brush` DP 用 `UI4.Brush.*`，`Color` DP 用 `UI4.Color.*`，写反了是「类型转换失败」而不是「取不到值」。

### 10.3.4 容器级文字色：TextElement.Foreground

与其给每个 `TextBlock` 写色，不如在容器上写一次，让子树继承（Demo `MainWindow.xaml:476`、`:505`、`:536`、`ScopeWindow.xaml:10`）：

```xml
<StackPanel TextElement.Foreground="{DynamicResource UI4.Brush.Text}">
  <TextBlock Text="作用域卡片标题" FontWeight="SemiBold"/>
  <TextBlock Text="副标题"/>
</StackPanel>
```

三个适用边界：

- 只对继承 `TextElement.Foreground` 的元素生效（`TextBlock`、`ContentControl` 内部文本等）。
- 一旦某个子元素自己写了 `Foreground="..."`（本地值），继承链就在该元素断开，切主题后它停在旧色 —— 这是「局部不留神就漏跟主题」的头号来源。
- 同理可用 `TextElement.FontSize` / `TextElement.FontWeight` 统一起手，但**对 `UI4TextBlock` 无效**（见 10.3.6）。

### 10.3.5 x:Name 与事件命名约定

- 凡需要被代码或脚本定位的元素都要 `x:Name`。`x:Name` 同时成为 UIA 的 `AutomationId`，是本仓库所有验证脚本的定位依赖（库内没有任何 `OnCreateAutomationPeer` 实现）。
- 命名用 PascalCase 且语义化：`HeaderBorder`、`DemoTabs`、`ScopeKeyCombo`、`StatusText`、`CtxHost`（Demo 全项目一致）。
- 事件处理命名 `<Element>_<Event>`：`Switch_Toggled`、`ScopeKeyCombo_SelectionChanged`、`BrowserTab_AddTab`、`TrayIcon_TrayRightMouseDown`、`MainWindow_Closed`。不要写 `Handler1`、`OnClick`。
- `x:Name` 与 `Name` 不要同时给；`x:Name` 生成的字段是 `internal`，页内直接用名字访问即可。
- **处理解析期早触发的事件必须空判**：`SelectedIndex="0"`、`IsOn="True"` 这类初值会在 `InitializeComponent()` 期间就触发事件，此时其它命名字段还是 null（`MainWindow.xaml.cs:98-103`、`:218-227` 两处注释都强调）。约定写法是处理函数第一行 `if (所需元素 == null) return;`。

### 10.3.6 `new`-隐藏 DP 造成的歧义与规避

库里有两类「与基类同名但**不是同一个属性**」的情况，必须区别对待。

**① `UI4TextBlock`（`ContentControl` 的子树渲染）用 `new` 重新注册了四个 DP**：`Foreground`（`:50-61`，默认 null）、`Padding`（`:116-122`）、`FontSize`（`:129-135`，默认 15）、`FontWeight`（`:142-148`）。它们与 `Control`/`Control.FontSize` 等基类属性是**两条独立路径**：

```xml
<!-- 生效：直接写在元素上，命中的是 UI4TextBlock 自己的 DP -->
<ui:UI4TextBlock Text="标题" FontSize="24" Foreground="{DynamicResource UI4.Brush.Text}"/>

<!-- 不生效：容器级 TextElement.FontSize / TextBlock.FontSize 附加属性进不了模板，
     模板已把内部 TextBlock 的 FontSize 绑到 UI4TextBlock.FontSize（:349），默认 15 -->
<StackPanel TextElement.FontSize="24">
  <ui:UI4TextBlock Text="标题"/>   <!-- 仍是 15 -->
</StackPanel>

<!-- 不生效：代码里SetValue(Control.ForegroundProperty, ...) 动的是基类 DP -->
```

规避：`UI4TextBlock` 的字号/粗细/内边距/前景一律写在元素自身；文字色想跟随容器就**完全不要写 `Foreground`**（默认 null 时模板不覆盖内部文本，继承值才能落地，见 `:354-357`）。另注意它不是 `System.Windows.Controls.TextBlock`，把两者混进同一个 `as`/参数类型会 CS1503。

**② `UI4Panel` 的描边**：内层 `Border` 的 `BorderBrush` 由 `BorderColor`（`Color` 类型，`:46-56`、`:224`）生成，`Control.BorderBrush` **对渲染无影响**；`BorderThickness` 有效（`:172-173` 重写默认值为 1，`:220-221` 绑到内层）。

```xml
<!-- 正确 -->
<ui:UI4Panel BorderColor="{DynamicResource UI4.Color.BorderNormal}" BorderThickness="2" CornerRadius="16"/>
<!-- 错误：写了不会显示，且会让人误以为「主题没生效」 -->
<ui:UI4Panel BorderBrush="{DynamicResource UI4.Brush.BorderNormal}" .../>
```

同族提醒：`HoverBorderBrush` 是 `SolidColorBrush` 类型（`:59-69`），与 `BorderColor`（`Color`）不是同一类型 —— 同一概念在不同控件上类型不同（`UI4ListView.ItemBackground` 是 `Brush`）。

### 10.3.7 图标约定

仓库内**没有任何图标字体或图片资源**。图标一律用系统字体 `Segoe MDL2 Assets` 的码位：

```xml
<ui:UI4TabItem Header="设置" TextIcon="&#xE713;" TextIconFontFamily="Segoe MDL2 Assets" IsClosable="False"/>
<ui:UI4NavigationViewItem TextIcon="&#xE104;" TextIconFontFamily="Segoe MDL2 Assets" Header="代码"/>
<ui:UI4MenuElementItem Header="新建" TextIcon="&#xE710;" IconFontFamily="Segoe MDL2 Assets" IconFontSize="14"/>
```

- XAML 里写实体 `&#xE713;`；C# 里写转义 `"\uE713"`（`MainWindow.xaml.cs:163`）。
- 属性名**不统一**：`UI4TabItem` / `UI4NavigationViewItem` 是 `TextIcon` + `TextIconFontFamily`（`FontFamily` 类型），`UI4MenuElementItem` 是 `TextIcon` + `IconFontFamily` + `IconFontSize`。写之前按控件查名。
- 码位不要凭记忆猜：先在 Demo 里找已用过的（`&#xE713;` 设置、`&#xE710;` 新增、`&#xE8E5;` 打开、`&#xE723;` 文档、`&#xE80F;` 主页、`&#xE7A6;`/`&#xE7A7;` 重做/撤销、`&#xE104;` 播放/代码、`&#xE8F1;` 属性）。

### 10.3.8 四条「文档说谎」与 Obsolete 别名

写 XAML/代码前，以下成员名以源码为准，不要照 README 或类注释抄：

1. `UI4ComboBox` **没有** `HoverBorderColor` / `FocusBorderColor`（焦点边框是渐变，实为 `FocusGradientStart` / `FocusGradientEnd`）。`FocusBorderColor` / `HoverBorderColor` 只存在于 **`UI4TextBox`**（`UI4TextBox.cs:50/:59`）。
2. `UI4Panel` **没有** `Title` 属性（类注释残留）。
3. `UI4MultiLanguage` **没有** `SetLanguage(string)`：切语言要改 `CultureInfo.CurrentUICulture` 再调 `UI4MultiLanguage.Refresh()`（`MainWindow.xaml.cs:278-286`）。
4. `UI4ContextMenuLanguage` 已被删除，不要在 XAML 里引它。
5. `UI4CheckBox.BoxCornerRadius` 是 `[Obsolete]` 别名 DP，与 `CornerRadius` 是**同一个** DP（`UI4CheckBox.cs:47`）。Demo 里仍写 `BoxCornerRadius="6"`（`MainWindow.xaml:51`）能工作但会吃 CS0618 警告；新代码写 `CornerRadius`。

### 10.3.9 样板样式放 App.xaml

两个跨页复用的文本样式照抄 `App.xaml:7-18`，只放 `Application.Resources`（页文件里不重复定义）：

```xml
<Style x:Key="SectionTitle" TargetType="TextBlock">
  <Setter Property="FontSize" Value="18"/>
  <Setter Property="FontWeight" Value="SemiBold"/>
  <Setter Property="Foreground" Value="{DynamicResource UI4.Brush.Accent}"/>
  <Setter Property="Margin" Value="0,18,0,8"/>
</Style>
<Style x:Key="SectionHint" TargetType="TextBlock">
  <Setter Property="FontSize" Value="12"/>
  <Setter Property="Foreground" Value="{DynamicResource UI4.Brush.Icon}"/>
  <Setter Property="Margin" Value="0,0,0,6"/>
  <Setter Property="TextWrapping" Value="Wrap"/>
</Style>
```

约定：`SectionTitle` 用于分区标题（强调色），`SectionHint` 用于说明文字（次要 `Icon` 色）。样式键是你自己定的普通键，所以引用它们用 `StaticResource` 合理；样式**内部**的颜色必须 `DynamicResource`。

## 10.4 推荐做法（Do / Don't）

- Do：写任何 UI4 成员名前 `grep` 一次源码，确认存在与类型（`Color` 还是 `Brush`）。
- Do：颜色走 `DynamicResource` + 令牌键；字号/边距/圆角这类尺寸可以直接写数字。
- Do：容器级 `TextElement.Foreground` 打底，元素级只在真正要突出时覆盖。
- Do：给脚本要定位的元素一律加 `x:Name`，命名即 `AutomationId`。
- Do：事件处理第一行做空判，把「解析期就触发」当默认预期。
- Don't：不要写 `{StaticResource UI4.Brush.*}`，也不要写 `{DynamicResource UI4.Color.Text}` 这种不存在的别名 Color 形式。
- Don't：不要用 `TextElement.FontSize` 去统一 `UI4TextBlock` 的字号（无效）。
- Don't：不要在新代码里用 `BoxCornerRadius`。
- Don't：不要引入自定义图标字体或 `.ico` —— 仓库无字体/图片资源，托盘用系统回退即可。

## 10.5 常见 XAML 报错 → 真因对照

| 报错形态 | 真因 | 处置 |
|---|---|---|
| 「类型 `ui:XXX` 不存在 / 无效标签名」（常成片出现，MC3074 一类） | ① 类名拼错；② `xmlns:ui` 少了 `;assembly=`；③ **库内有 C# 编译错误造成级联**，dll 没产出，所有 XAML 一起报 | 先全量构建库工程看真实错误列表（`事实速查.md` 第 7 节第 1 条），确认 dll 存在后再查 XAML |
| 「属性 `X` 在类型 `Y` 上未找到」 | 该属性只存在于过时文档（10.3.8 四条），或你把 A 控件的属性抄到 B 控件上（`IconFontFamily` vs `TextIconFontFamily`） | 用源码 `grep` 确认；改名或删掉该属性 |
| 「找不到名为 `UI4.Brush.X` 的资源」 | ① 用了 `StaticResource` 而键尚未播种（第 8 章）；② 键名拼错（多/少后缀、把别名写成 Color） | 改 `DynamicResource`，核对 10.3.3 清单；确认 `UI4Theme.ApplyToApplication()` 已在主窗创建前调用 |
| 「`Cannot use DynamicResource`」/ 资源引用无效 | 把 `{DynamicResource}` 用在了非依赖属性上（POCO、普通 CLR 属性），或用在 `Trigger` 的条件值上 —— 触发条件需要可比对的常量 | 把颜色移到 `Setter.Value`；触发条件改成常量或换用 `DataTrigger` + 绑定 |
| 类型转换失败（Color 给 Brush DP 或反之） | 键的类型与 DP 类型不匹配（`UI4.Color.*` 是 `Color`，`UI4.Brush.*` 是 `SolidColorBrush`） | 按 10.3.3 的推论选形式；`Color` 类型 DP 一律 `UI4.Color.*` |
| 元素在深色主题下文字仍是黑色 | 元素上有硬编码 `Foreground` 本地值，切断了继承；或对 `UI4TextBlock` 误以为继承生效 | 删本地值或改 `DynamicResource`；`UI4TextBlock` 要显式写 `Foreground` |
| 切主题后某控件不变色，重启后却是对的 | 你在被主题管理的 DP 上赋了本地值（`ReadLocalValue` 有值即跟随失效） | 删掉赋值，或改用 `UI4ContextMenu` 之外不受此约束的属性；参见第 12 章 |
| 运行时抛「找不到 `ICSharpCode.AvalonEdit`」 | XAML 里放了 `UI4CodeEditor` 但部署包缺 dll（第 7 章） | 补 dll 或去掉该控件 |

## 10.6 完成判据

1. `grep -r "StaticResource UI4\."` 命中 0 次；`grep -r "clr-namespace:StartUI4Controls"` 的结果数 = 用到 UI4 的 XAML 文件数（每文件都自带）。
2. 逐条核对：工程里没有 `UI4ComboBox.HoverBorderColor`/`FocusBorderColor`、`UI4Panel.Title`、`UI4MultiLanguage.SetLanguage`、`UI4ContextMenuLanguage`、`BoxCornerRadius` 这五个不存在/废弃名。
3. 所有 `{DynamicResource UI4.*}` 的键都能在 10.3.3 清单里找到，且形式（Color/Brush）与目标 DP 类型匹配。
4. 需要脚本定位的元素都有 `x:Name`；随机抽三个事件处理，命名符合 `<Element>_<Event>` 且首行有空判。
5. 图标全部是 `Segoe MDL2 Assets` 码位，XAML 用 `&#x…;`、C# 用 `\u…`，属性名与该控件一致。
6. 切一次全局主题，页面上**没有**任何元素颜色停在旧值（用截图基线或 UIA 读色值对比）。
7. `App.xaml` 里 `SectionTitle` / `SectionHint` 存在且内部颜色为 `DynamicResource`；页文件中无重复定义。
