# StartUI4.WPF 控件库文档（.NET Framework 4.8 移植版）

> 一套现代风格（Modern Design）的 WPF UI 控件库，本仓库为 **.NET Framework 4.8 / C# 7.3** 的完整移植版本。

> **上游仓库**：<https://github.com/KSSTU/StartUI4.WPF>（net6.0-windows7.0）
> **移植与差异说明**：见本仓库 [`PORTING.md`](PORTING.md)

---

## 目录

- [一、简介](#一简介)
- [二、特性](#二特性)
- [三、快速开始](#三快速开始)
- [四、控件一览](#四控件一览)
- [五、控件详解](#五控件详解)
- [六、完整使用教程：从零搭建一个应用](#六完整使用教程从零搭建一个应用)
- [七、Demo 工程指南](#七demo-工程指南)
- [八、与上游（net6 版）的差异与已知问题](#八与上游net6-版的差异与已知问题)
- [九、附录](#九附录)
- [十、主题系统（全局 / 局部作用域 / 高对比度）](#十主题系统全局--局部作用域--高对比度)
- [许可证](#许可证)

---

## 一、简介

**StartUI4.WPF** 是一套对齐现代设计语言的 WPF 控件库。本移植版将上游的 .NET 6 代码完整重写为
**.NET Framework 4.8**，可在 VS2019 及更高版本、仅安装 .NET Framework 4.8  targeting pack 的环境中编译与运行。

- **版本**：1.0.20
- **原作者**：KS.STUDIO
- **目标框架**：.NET Framework 4.8（`net48`）
- **语言级别**：C# 7.3（net48 默认，不依赖任何高版本语法开关）
- **NuGet 包名**：StartUI4.WPF
- **支持系统**：Windows 7 / 8.1 / 10 / 11（.NET Framework 4.8 所支持的范围）
- **运行时自检**：Demo 主窗口标题栏右侧实时显示 `RuntimeInformation.FrameworkDescription`，
  用于确认程序确实运行在 .NET Framework 4.8 上

---

## 二、特性

- **现代设计风格** —— 圆角、渐变、阴影、悬浮动效，对齐现代设计语言
- **渐变支持** —— 按钮、进度条、滑块、开关等均支持起止渐变色配置
- **丰富动画** —— 悬浮缩放、开关滑动、加载旋转、数字翻转等平滑动画
- **高度可定制** —— 250+ 个依赖属性对外开放，几乎每个视觉细节都可调
- **开箱即用** —— 引用程序集或 NuGet 包后直接在 XAML 中使用，无需额外资源字典
- **主题系统** —— 亮/暗/跟随系统/高对比度一键切换，30 个颜色令牌经 `DynamicResource` 桥接到宿主；`UI4ThemeScope` 可对单张卡片或整个窗口局部换肤；`UI4WindowTitleBar` 经 DWM 让**系统标题栏**同步跟随（详见第十节）
- **纯代码模板** —— 所有控件模板由代码构建，不依赖 Themes/generic.xaml，单 dll 即可分发
- **.NET Framework 4.8 原生** —— 无 `IsExternalInit` 等 polyfill、无 LangVersion 开关，老工具链亦可编译

---

## 三、快速开始

### 1. 环境要求

| 项目 | 要求 |
|---|---|
| 操作系统 | Windows 7 SP1 及以上 |
| 运行时 | .NET Framework 4.8 |
| 编译工具 | Visual Studio 2019 16.8+ / VS2022 / .NET SDK（MSBuild）均可 |
| 依赖包 | AvalonEdit 6.3.1.120、System.Data.SQLite 2.0.3（仅 `UI4CodeEditor` / 内部 `UI4DataGrid` 需要） |

### 2. 方式一：源码构建

```bash
git clone <本仓库>
dotnet build StartUI4Controls.sln
```

产物：

- `src/StartUI4Controls/bin/Debug/net48/StartUI4Controls.dll`
- `src/StartUI4Controls/bin/Debug/StartUI4.WPF.1.0.20.nupkg`（构建时自动打包）

### 3. 方式二：NuGet 引用

```powershell
Install-Package StartUI4.WPF
```

或使用本地构建出的 nupkg：

```powershell
dotnet add package StartUI4.WPF --source ./src/StartUI4Controls/bin/Debug
```

### 4. 引入命名空间

在任意 XAML 文件根节点添加：

```xml
xmlns:ui="clr-namespace:StartUI4Controls;assembly=StartUI4Controls"
```

### 5. 第一个控件

```xml
<ui:UI4Button Content="你好，StartUI4" Width="200" Height="40" />
```

### 6. 验证运行环境

启动 `samples/StartUI4Demo`，主窗口标题栏右侧会显示实际运行时，例如：

```
实际运行时： .NET Framework 4.8.9345.0
```

若误跑在 .NET Core / .NET 5+ 上，此处前缀会变为 `.NET Core` / `.NET`，可立即识别。

---

## 四、控件一览

| 控件 | 基类 | 说明 |
|---|---|---|
| `UI4Button` | `Button` | 渐变 / 圆角 / 悬浮色按钮；禁用态自动换灰色模板，前景按背景亮度自适应（见第五节 UI4Button） |
| `UI4CheckBox` | `CheckBox` | 自定义勾选框 |
| `UI4Radio` | `RadioButton` | 自定义单选按钮 |
| `UI4Switch` | `Control` | 现代滑动开关 |
| `UI4TextBox` | `TextBox` | 聚焦描边、占位符、清除按钮输入框 |
| `UI4PasswordBox` | `TextBox` | 密码框，支持明文切换与自定义掩码 |
| `UI4TextBlock` | `ContentControl` | 带阴影 / 渐变 / 圆角面板的文本显示 |
| `UI4FlipTextBlock` | `ContentControl` | 数字翻牌动画文本 |
| `UI4ComboBox` | `ComboBox` | 自定义下拉框，弹出层宽度自适应 |
| `UI4ProgressBar` | `Control` | 渐变进度条，支持不确定模式 |
| `UI4ProgressRing` | `ContentControl` | 环形进度（确定 / 不确定） |
| `UI4Slider` | `Slider` | 渐变滑块，带数值显示 |
| `UI4CircleSlider` | `ContentControl` | 环形滑块 |
| `UI4ColorPicker` | `Window` | HSV 取色对话框 |
| `UI4Panel` | `ContentControl` | 阴影 + 悬浮缩放容器 |
| `UI4Pivot` / `UI4PivotItem` | `Selector` / `HeaderedContentControl` | 滑动切换页签 |
| `UI4Tab` / `UI4TabItem` | `Selector` / `HeaderedContentControl` | 浏览器风格标签页 |
| `UI4NavigationView` 及 Item | `ItemsControl` / `ContentControl` | 侧边导航 |
| `UI4ListBox` | `ListBox` | 支持普通 / 圆点 / 编号三种列表样式 |
| `UI4ListView` | `ListBox` | 卡片式列表 |
| `UI4GridView` | `ListBox` | 自适应列数网格卡片 |
| `UI4ScrollViewer` | `ScrollViewer` | 美化滚动条 + 平滑滚动 |
| `UI4MessageBox` | `Window` | 自定义消息对话框 |
| `UI4NotifyIcon` | `FrameworkElement` | 系统托盘图标 + 自定义右键菜单 |
| `UI4Menu` 及 Item | `Menu` / `MenuItem` | 支持文字图标与 KeyTip 的菜单栏 |
| `UI4ContextMenu` | —（代码组件） | 自定义右键菜单 |
| `UI4CodeEditor` | AvalonEdit `TextEditor` | 代码编辑器，内置 C# 高亮与右键菜单 |
| `UI4Clipboard` | —（静态服务） | 原生 Win32 剪贴板读写，库内文本控件的复制/剪切/粘贴均走此通道（见第八节 E） |
| `UI4Grid` | `Grid` | 默认渐变背景的 Grid |
| `UI4MultiLanguage` | —（静态服务） | 静态文案：zh / en / ja / ko / de / fr / es / ru 八套，切语言靠 `CultureInfo.CurrentUICulture` + `Refresh()` |
| `UI4Theme` | —（静态服务） | 全局主题：亮/暗/跟随系统/高对比度、令牌资源桥、`SetAccent`、自定义主题注册、持久化 |
| `UI4ThemeScope` | —（附加属性） | 局部/每窗口主题：`ui:UI4ThemeScope.Theme="dark"`，子树独立换肤（见第十节） |
| `UI4WindowTitleBar` | —（附加属性 + 静态方法） | 系统标题栏跟随主题：DWM 深/浅 + 标题栏底色/文字/边框染色，默认全自动，`ui:UI4WindowTitleBar.Enabled="False"` 可豁免（见第十节） |
| `UI4ThemeMode` / `UI4ThemeToken` | —（枚举） | 主题模式（Light/Dark/System/HighContrast）与 30 个颜色令牌键 |
| `UI4ThemeDefinition` | —（sealed 类） | 一套主题的令牌取值：内置 `Light()` / `Dark()` / `HighContrast()`，可 `Clone()` + `With(token, color)` 定制后 `UI4Theme.Register` |
| `RegistryThemePersistence` / `JsonThemePersistence` | `IThemePersistence` 实现 | 主题模式持久化后端：HKCU 注册表 / JSON 文件（宿主自选，见第十节） |
| `UI4MenuItem` / `UI4MenuItemType` / `UI4MenuIcons` | —（配套类型） | 右键菜单条目数据、七种标准条目类型（Undo/Redo/Cut/Copy/Paste/Delete/SelectAll）与内置图标 |
| `UI4TrayMenuItem` / `PopupActivationMode` | —（配套类型） | 托盘菜单条目与"哪种鼠标键弹菜单"的枚举 |
| `UI4LanguageKey` | —（枚举） | 静态文案键（OK/Cancel/Notice/ColorPicker/Undo/…/SelectAll） |
| `TabCloseRoutedEventArgs` | `RoutedEventArgs` | `UI4Tab.CloseTab` 事件参数（携带被关的 `UI4TabItem`） |

> `UI4DataGrid`、`UI43DSphere` 在上游即为 `internal` 且无引用，本移植版保持 internal，不对外公开（见附录 C）。

---

## 五、控件详解

> 约定：颜色默认值以 `#AARRGGBB` 表示；"继承属性"指基类自带、可直接使用的属性。
> "默认值"列给出的是依赖属性的字面值；标注「跟随主题 X」的属性，在宿主未显式赋值时会被当前主题的对应令牌覆盖，
> 一旦在 XAML/代码里显式赋值即停止跟随（宿主优先）。

---

### UI4Button

现代按钮，支持圆角、渐变、悬浮背景与悬浮前景色。

**继承自**：`Button`

#### 可设置属性

| 属性 | 类型 | 默认值 | 说明 |
|---|---|---|---|
| `CornerRadius` | `CornerRadius` | `6` | 圆角半径 |
| `GradientStart` | `Color` | `#FF0078D4` | 渐变起始色 |
| `GradientEnd` | `Color` | `#FF9333EA` | 渐变结束色 |
| `HoverBackground` | `Brush` | `#FF0066B5` | 悬浮背景 |
| `HoverBorderBrush` | `Brush` | `null` | 悬浮描边（null 表示不改变） |
| `HoverForeground` | `Brush` | `White` | 悬浮前景色 |

#### 继承属性

`Content`、`Background`、`Foreground`、`FontSize`、`FontWeight`、`Width`、`Height`、`Margin`、`Padding`、`Cursor` 等。

#### 示例

```xml
<ui:UI4Button Content="确定" Width="100" Height="30" />

<ui:UI4Button Content="绿色按钮" Background="Green" HoverBackground="DarkGreen"
              Width="200" Height="40" />

<ui:UI4Button Content="渐变按钮" GradientStart="#FF0024FF" GradientEnd="#FFB400FF"
              Width="150" Height="40" />

<ui:UI4Button Content="圆角按钮" CornerRadius="20" Width="150" Height="40" />
```

#### 事件

继承 `Button` 的全部事件：`Click`、`MouseEnter`、`MouseLeave` 等。

#### 前景色与禁用态

前景色**跟随背景亮度**：样式构建时按 `GradientStart`/`GradientEnd` 混合色的 WCAG 相对亮度选色——深色底（亮度 < 0.45）用白字，浅色底用主题正文色 `TextForeground`。这只是样式默认值，使用方本地显式设置的 `Foreground` 仍然优先。

`IsEnabled="False"`（或绑定的 `Command.CanExecute` 返回 false）时整块替换 `Template`：背景取主题令牌 `BorderNormal`，前景按同一亮度规则自动变深，跟随主题切换，使用方无需自己刷颜色。

实现上禁用态必须**换 `Template`** 而不是设 `Background`/`GradientStart`——使用方在 XAML 里本地设置过渐变值后，样式 Setter 无法覆盖本地值（这正是"复制按钮禁用却仍是蓝色"的成因）。

实测对比度（背景 vs 实际渲染的文字色）：

| 状态 | 背景 | 文字 | 对比度 |
|---|---|---|---|
| 强调色按钮（`#3D9BD9`） | `#3D9BD9` | `#FFFFFF` | 3.05 |
| 禁用态 | `#C8C8DC` | `#1E1E1E` | 10.12 |
| 中性灰按钮（`#C8C8D2`） | `#C8C8D2` | `#1E1E1E` | 10.04 |

修复前禁用态与中性灰按钮都是"浅灰底 + 硬编码白字"，对比度只有 1.10 / 1.66，文字几乎不可读。

---

### UI4CheckBox

自定义勾选框，勾选块颜色、尺寸、圆角、文字颜色均可调。

**继承自**：`CheckBox`

#### 可设置属性

| 属性 | 类型 | 默认值 | 说明 |
|---|---|---|---|
| `BoxCornerRadius` | `CornerRadius` | `6` | 勾选块圆角 |
| `CheckBackground` | `Color` | `#FF0066B5` | 勾选态填充色 |
| `BorderNormalColor` | `Color` | `#FFB4B4C8` | 未勾选边框色 |
| `BoxSize` | `double` | `18` | 勾选块边长 |
| `TextColor` | `Color` | `LightGray` | 文字颜色 |
| `TextMargin` | `Thickness` | `8,0,0,0` | 文字与勾选块间距 |

#### 示例

```xml
<ui:UI4CheckBox Content="同意服务条款" IsChecked="True" Margin="10" />

<ui:UI4CheckBox Content="绿色主题" CheckBackground="Green" BorderNormalColor="DarkGreen"
                BoxSize="24" BoxCornerRadius="6" />

<ui:UI4CheckBox Content="自定义文字" TextColor="Black" TextMargin="12,0,0,0" FontSize="16" />
```

#### 说明

`IsChecked`、`IsThreeState`、`Checked` / `Unchecked` / `Indeterminate` 事件均继承自 `CheckBox`。

---

### UI4Radio

自定义单选按钮，样式属性与 `UI4CheckBox` 对应。

**继承自**：`RadioButton`

#### 可设置属性

| 属性 | 类型 | 默认值 | 说明 |
|---|---|---|---|
| `CheckBackground` | `Color` | `#FF0066B5` | 选中态圆环颜色 |
| `BorderNormalColor` | `Color` | `#FFB4B4C8` | 未选中边框色 |
| `DotColor` | `Color` | `White` | 选中圆点颜色 |
| `BoxSize` | `double` | `18` | 圆圈直径 |
| `TextColor` | `Color` | `Black` | 文字颜色 |
| `TextMargin` | `Thickness` | `8,0,0,0` | 文字间距 |

#### 示例

```xml
<ui:UI4Radio Content="选项 1" IsChecked="True" GroupName="G1" Margin="5" />
<ui:UI4Radio Content="选项 2" GroupName="G1" Margin="5" />
<ui:UI4Radio Content="绿色主题" CheckBackground="Green" BorderNormalColor="DarkGreen"
             DotColor="White" BoxSize="20" GroupName="G1" />
```

---

### UI4Switch

现代滑动开关。**支持双向绑定**（`IsOn` 为 `BindsTwoWayByDefault`）。

**继承自**：`Control`

#### 可设置属性

| 属性 | 类型 | 默认值 | 说明 |
|---|---|---|---|
| `IsOn` | `bool` | `false` | 开关状态（双向绑定） |
| `GradientStart` | `Color` | `#FF0078D4` | 开启态渐变起始色 |
| `GradientEnd` | `Color` | `#FF0078D4` | 开启态渐变结束色 |
| `OffBackground` | `Color` | `#FFC8C8D2` | 关闭态底色 |
| `ThumbColor` | `Color` | `White` | 滑块颜色 |
| `SwitchWidth` | `double` | `50` | 开关宽度 |
| `SwitchHeight` | `double` | `28` | 开关高度 |

#### 事件

| 事件 | 签名 | 说明 |
|---|---|---|
| `Toggled` | `RoutedEventHandler` | 状态切换时触发（冒泡） |

#### 示例

```xml
<ui:UI4Switch IsOn="True" Toggled="Switch_Toggled" />

<ui:UI4Switch IsOn="True" SwitchWidth="60" SwitchHeight="32"
              GradientStart="Green" GradientEnd="DarkGreen" OffBackground="LightGray" />
```

```csharp
private void Switch_Toggled(object sender, RoutedEventArgs e)
{
    var sw = (UI4Switch)sender;
    Debug.WriteLine("IsOn = " + sw.IsOn);
}
```

#### 说明

- 控件在 `StackPanel` / `Grid` 中被拉伸时，开关本体会靠左摆放而不会被拉变形（移植版修复项）。
- XAML 中写 `IsOn="True"` 会在加载期立即触发一次 `Toggled`，事件处理器需对未初始化的成员做判空。

---

### UI4TextBox

带占位符、悬浮/聚焦描边、清除按钮的输入框，支持多行。

**继承自**：`TextBox`

#### 可设置属性

| 属性 | 类型 | 默认值 | 说明 |
|---|---|---|---|
| `CornerRadius` | `CornerRadius` | `6` | 圆角 |
| `BorderNormalColor` | `Color` | `#FFC8C8DC` | 常态边框色 |
| `HoverBorderColor` | `Color` | `#FF0078D4` | 悬浮边框色 |
| `FocusBorderColor` | `Color` | `#FF0066B5` | 聚焦边框色 |
| `EditBackground` | `Brush` | `White` | 编辑区背景 |
| `TextColor` | `Color` | `#FF1E1E1E` | 文字颜色 |
| `InnerPadding` | `Thickness` | `12,5,32,5` | 内边距（右侧预留按钮位） |
| `ShowClearButton` | `bool` | `false` | 是否显示清除按钮 |
| `PlaceholderText` | `string` | 空 | 占位符文本 |
| `PlaceholderForeground` | `Brush` | `LightGray` | 占位符颜色 |

#### 继承属性

`Text`、`AcceptsReturn`、`TextWrapping`、`VerticalScrollBarVisibility`、`MaxLength`、`IsReadOnly` 等。

#### 示例

```xml
<ui:UI4TextBox Width="260" Text="可编辑文本" />

<ui:UI4TextBox Width="260" Text="带清除按钮" ShowClearButton="True" />

<ui:UI4TextBox Width="300" PlaceholderText="请输入用户名..." />

<ui:UI4TextBox Width="300" PlaceholderText="请输入密码..." PlaceholderForeground="Gray" />

<ui:UI4TextBox Width="450" Height="100" AcceptsReturn="True" TextWrapping="Wrap"
               VerticalScrollBarVisibility="Auto" ShowClearButton="True"
               Text="多行文本" />
```

#### 说明

占位符在有文本时自动隐藏；清除按钮仅在 `ShowClearButton=True` 且有文本时出现。

---

### UI4PasswordBox

密码输入框：自定义掩码字符、明文/密文切换按钮、占位符、双向绑定 `Password`。

**继承自**：`TextBox`

#### 可设置属性

| 属性 | 类型 | 默认值 | 说明 |
|---|---|---|---|
| `CornerRadius` | `CornerRadius` | `6` | 圆角 |
| `BorderNormalColor` | `Color` | `#FFC8C8DC` | 常态边框色 |
| `HoverBorderColor` | `Color` | `#FF0078D4` | 悬浮边框色 |
| `FocusBorderColor` | `Color` | `#FF0066B5` | 聚焦边框色 |
| `EditBackground` | `Brush` | `White` | 编辑区背景 |
| `TextColor` | `Color` | `#FF1E1E1E` | 文字颜色 |
| `InnerPadding` | `Thickness` | `12,5,32,5` | 内边距 |
| `ShowPasswordButton` | `bool` | `true` | 是否显示明文切换按钮 |
| `PlaceholderText` | `string` | 空 | 占位符 |
| `PlaceholderForeground` | `Brush` | `LightGray` | 占位符颜色 |
| `Password` | `string` | 空 | 密码明文（可双向绑定） |
| `PasswordChar` | `char` | `●` | 掩码字符 |
| `IsPasswordMode` | `bool` | `true` | 当前是否密文显示 |

#### 示例

```xml
<ui:UI4PasswordBox PlaceholderText="请输入密码" Width="260" Height="36" />

<ui:UI4PasswordBox PasswordChar="*" ShowPasswordButton="True" Width="260" />

<ui:UI4PasswordBox Password="{Binding UserPassword, Mode=TwoWay}" ShowClearButton="True" />
```

```csharp
PwdBox.TextChanged += delegate { Trace.WriteLine("Password = " + PwdBox.Password); };
```

---

### UI4TextBlock

文本显示控件：阴影、圆角背景面板、对齐方式、换行控制。

**继承自**：`ContentControl`

#### 可设置属性

| 属性 | 类型 | 默认值 | 说明 |
|---|---|---|---|
| `Text` | `string` | 空 | 文本（与 `Content` 二选一） |
| `Foreground` | `Brush` | `null` | 前景；传 `LinearGradientBrush` 即渐变字 |
| `CornerRadius` | `CornerRadius` | `6` | 背景面板圆角 |
| `PanelBackground` | `Brush` | `Transparent` | 背景面板颜色 |
| `Padding` | `Thickness` | `8,6,8,6` | 内边距 |
| `FontSize` | `double` | `15` | 字号 |
| `FontWeight` | `FontWeight` | `Normal` | 字重 |
| `HorizontalContentAlign` | `HorizontalAlignment` | `Left` | 水平对齐 |
| `VerticalContentAlign` | `VerticalAlignment` | `Center` | 垂直对齐 |
| `TextWrapping` | `TextWrapping` | `NoWrap` | 换行方式 |
| `ShadowDepth` | `double` | `8` | 阴影深度 |
| `ShadowBlurRadius` | `double` | `5` | 阴影模糊半径 |
| `ShadowOpacity` | `double` | `0` | 阴影不透明度（0 = 无阴影） |
| `ShadowColor` | `Color` | `Black` | 阴影颜色 |

#### 示例

```xml
<ui:UI4TextBlock Text="普通文本" FontSize="24" />

<ui:UI4TextBlock Content="带阴影的文本" FontSize="26"
                 ShadowDepth="8" ShadowOpacity="0.6" ShadowBlurRadius="10" ShadowColor="Black" />

<!-- 渐变文本：通过 Foreground 传入渐变画笔 -->
<ui:UI4TextBlock Text="渐变文本" FontSize="32" FontWeight="SemiBold">
    <ui:UI4TextBlock.Foreground>
        <LinearGradientBrush EndPoint="1,0.5" StartPoint="0,0.5">
            <GradientStop Color="#FF2762EB"/>
            <GradientStop Color="#FFAD00FF" Offset="1"/>
        </LinearGradientBrush>
    </ui:UI4TextBlock.Foreground>
</ui:UI4TextBlock>
```

#### 说明

`GradientStart` / `GradientEnd` 为上游遗留的未实现属性（声明但从未生效），渐变请一律使用 `Foreground`。

---

### UI4FlipTextBlock

数字翻牌控件：文本变化时以卡片翻转动画过渡，适合计数器、时钟、指标看板。

**继承自**：`ContentControl`

#### 可设置属性

| 属性 | 类型 | 默认值 | 说明 |
|---|---|---|---|
| `Text` | `string` | `"0"` | 显示文本 |
| `FlipRate` | `double` | `0.3` | 翻转动画速率（秒/字符段） |
| `CardBackground` | `Color` | `White` | 卡片背景 |
| `CardForeground` | `Color` | `Black` | 卡片文字色 |
| `CardBorderBrush` | `Color` | `Gray` | 卡片边框色 |
| `CardCornerRadius` | `CornerRadius` | `12` | 卡片圆角 |
| `CardBorderThickness` | `Thickness` | `1` | 卡片边框厚度 |
| `ShadowColor` | `Color` | `Black` | 阴影颜色 |
| `CardShadowDepth` | `double` | `10` | 阴影深度 |
| `CardShadowBlurRadius` | `double` | `15` | 阴影模糊 |
| `CardShadowOpacity` | `double` | `0.1` | 阴影不透明度 |

#### 示例

```xml
<ui:UI4FlipTextBlock x:Name="FlipText" Text="42" FontSize="64" />

<ui:UI4FlipTextBlock Text="7" FontSize="48" CardBackground="DarkGreen"
                     CardForeground="Gold" FlipRate="0.5" />
```

```csharp
FlipText.Text = new Random().Next(0, 100).ToString();   // 赋值即触发翻转动画
```

---

### UI4ComboBox

自定义下拉框：聚焦渐变描边、圆角、下拉面板圆角；**弹出层宽度随最宽选项自适应**，
选中项超长时单行省略号截断并附完整文本 ToolTip。

**继承自**：`ComboBox`

#### 可设置属性

| 属性 | 类型 | 默认值 | 说明 |
|---|---|---|---|
| `CornerRadius` | `CornerRadius` | `6` | 控件圆角 |
| `BorderNormalColor` | `Color` | `#FFC8C8DC` | 常态边框色（跟随主题 `BorderNormal`） |
| `FocusGradientStart` | `Color` | `#FF0078D4` | 聚焦渐变起始色（跟随主题 `Accent`） |
| `FocusGradientEnd` | `Color` | `#FF9333EA` | 聚焦渐变结束色（跟随主题 `AccentEnd`） |
| `EditBackground` | `Brush` | `White` | 编辑区背景（跟随主题 `Surface`） |
| `TextColor` | `Color` | `#FF1E1E1E` | 文字颜色（跟随主题 `TextForeground`） |
| `InnerPadding` | `Thickness` | `12,10,30,10` | 内边距 |
| `DropCornerRadius` | `CornerRadius` | `6` | 下拉面板圆角 |

#### 示例

```xml
<ui:UI4ComboBox Width="300" Height="36" SelectedIndex="0">
    <ComboBoxItem>选项 1</ComboBoxItem>
    <ComboBoxItem>选项 2</ComboBoxItem>
</ui:UI4ComboBox>

<ui:UI4ComboBox Width="300" CornerRadius="16" DropCornerRadius="10"
                FocusGradientStart="Green" FocusGradientEnd="LimeGreen">
    <ComboBoxItem>圆角下拉框</ComboBoxItem>
</ui:UI4ComboBox>

<!-- 长选项：闭合态省略号 + ToolTip，展开态弹出层自动加宽完整显示 -->
<ui:UI4ComboBox Width="220" SelectedIndex="1">
    <ComboBoxItem>短选项</ComboBoxItem>
    <ComboBoxItem>这是一个非常非常长的选项文本，用于验证选中项不会溢出控件边界</ComboBoxItem>
</ui:UI4ComboBox>
```

#### 说明

- 下拉滚动时滚动条淡入、停止后淡出（内置行为）。
- `IsEditable=True` 时切换为内嵌编辑框，样式同步应用。

---

### UI4ProgressBar

渐变进度条，支持不确定模式（往返动画）。

**继承自**：`Control`

#### 可设置属性

| 属性 | 类型 | 默认值 | 说明 |
|---|---|---|---|
| `CornerRadius` | `CornerRadius` | `5` | 圆角 |
| `GradientStart` | `Color` | `#FF0096E6` | 渐变起始色 |
| `GradientEnd` | `Color` | `#FF0078D4` | 渐变结束色 |
| `TrackBackground` | `Color` | `#0A000000` | 轨道底色 |
| `IsIndeterminate` | `bool` | `false` | 不确定模式 |
| `Minimum` / `Maximum` | `double` | `0` / `100` | 取值范围 |
| `Value` | `double` | `0` | 当前值 |

#### 示例

```xml
<ui:UI4ProgressBar Value="50" Maximum="100" Width="200" Height="6" />

<ui:UI4ProgressBar Value="50" Width="200" Height="6" Background="Red" />

<ui:UI4ProgressBar Value="50" Width="200" Height="6"
                   GradientStart="#FF55FF00" GradientEnd="#FF0016FF" />

<ui:UI4ProgressBar IsIndeterminate="True" Width="200" Height="6" />
```

#### 说明

设置 `Background` 会覆盖渐变，呈现纯色进度条。

---

### UI4ProgressRing

环形进度指示器：不确定模式为旋转弧；确定模式显示进度弧与中心数值，支持启动动画。

**继承自**：`ContentControl`

#### 可设置属性

| 属性 | 类型 | 默认值 | 说明 |
|---|---|---|---|
| `IsActive` | `bool` | `true` | 是否激活动画 |
| `IsIndeterminate` | `bool` | `true` | 不确定（旋转）模式 |
| `Minimum` / `Maximum` | `double` | `0` / `100` | 取值范围 |
| `Value` | `double` | `0` | 当前值 |
| `AnimatedValue` | `double` | `0` | 动画驱动值（内部使用） |
| `RingBackground` | `Brush` | `#0A000000` 画笔 | 环底色 |
| `RingForeground` | `Brush` | `#FF0078D4` 画笔 | 进度环颜色（可传渐变画笔） |
| `RingThickness` | `double` | `6` | 环宽 |
| `ShowValueText` | `bool` | `true` | 是否显示中心数值 |
| `ValueFontSize` | `double` | `30` | 数值字号 |
| `EnableStartupAnimation` | `bool` | `true` | 加载时从 0 转到当前值 |
| `StartupAnimationDuration` | `double` | `0.5` | 启动动画秒数 |

#### 示例

```xml
<ui:UI4ProgressRing IsActive="True" IsIndeterminate="True" Width="80" Height="80" />

<ui:UI4ProgressRing IsIndeterminate="False" Value="50" Maximum="150"
                    Foreground="Red" ValueFontSize="20" ShowValueText="True" />

<ui:UI4ProgressRing Width="150" Height="150" IsIndeterminate="False"
                    RingThickness="12" Maximum="60" Value="36"
                    ValueFontSize="34" ShowValueText="True">
    <ui:UI4ProgressRing.RingForeground>
        <LinearGradientBrush EndPoint="0.5,1" StartPoint="0.5,0">
            <GradientStop Color="#FF2861EB"/>
            <GradientStop Color="#FFAC01FF" Offset="1"/>
        </LinearGradientBrush>
    </ui:UI4ProgressRing.RingForeground>
</ui:UI4ProgressRing>
```

---

### UI4Slider

渐变滑块，带最小/当前/最大数值显示。

**继承自**：`Slider`

#### 可设置属性

| 属性 | 类型 | 默认值 | 说明 |
|---|---|---|---|
| `CornerRadius` | `CornerRadius` | `6` | 圆角 |
| `GradientStart` / `GradientEnd` | `Color` | `#FF0078D4` | 已选区渐变色 |
| `TrackBackground` | `Color` | `White` | 未选轨道色 |
| `ThumbSize` | `double` | `16` | 滑块直径 |
| `IsValueVisible` | `bool` | `true` | 是否显示数值行 |

#### 示例

```xml
<ui:UI4Slider Value="50" Maximum="100" Minimum="0" Width="200" Height="20" />

<ui:UI4Slider Value="50" Maximum="200" Width="200" IsValueVisible="False"
              GradientStart="#FFFFF900" GradientEnd="Red" />

<ui:UI4Slider Value="30" ThumbSize="20" Width="300" />
```

---

### UI4CircleSlider

环形滑块：拖拽圆环改变数值，中心显示数值。

**继承自**：`ContentControl`

#### 可设置属性

| 属性 | 类型 | 默认值 | 说明 |
|---|---|---|---|
| `Value` | `double` | `0` | 当前值 |
| `Minimum` / `Maximum` | `double` | `0` / `100` | 取值范围 |
| `SmallChange` | `double` | `1` | 步进 |
| `RingThickness` | `double` | `8` | 环宽 |
| `RingForeground` | `Brush` | `#FF0078D4` 画笔 | 进度环颜色（可渐变） |
| `RingBackground` | `Brush` | `#0A000000` 画笔 | 环底色 |
| `ShowValueText` | `bool` | `false` | 是否显示中心数值 |
| `ValueFontSize` | `double` | `30` | 数值字号 |
| `AnimationDuration` | `double` | `0.5` | 动画秒数 |

#### 示例

```xml
<ui:UI4CircleSlider Value="60" ShowValueText="True" />

<ui:UI4CircleSlider Width="200" Height="200" RingThickness="12" Maximum="360"
                    ValueFontSize="60" Value="60" ShowValueText="True" Foreground="Green" />
```

---

### UI4ColorPicker

HSV 取色对话框：二维色图 + 色相条 + HEX/ARGB 输入 + RGB/HSV 模式切换。

**继承自**：`Window`

#### 构造与静态方法

| 成员 | 签名 | 说明 |
|---|---|---|
| 构造函数 | `UI4ColorPicker(string title = null, Color? defaultColor = null)` | 创建实例 |
| `ShowDialog` | `static Color? ShowDialog(string title = null, Color? defaultColor = null, Window owner = null)` | 模态取色，取消返回 `null` |
| `Show` | `bool? Show(Window owner = null)` | 实例方式显示 |
| `SelectedColor` | `Color`（只读） | 确认后的颜色 |

#### 示例

```csharp
// 静态方式
Color? result = UI4ColorPicker.ShowDialog();
if (result.HasValue)
    myPanel.Background = new SolidColorBrush(result.Value);

// 带标题与默认色
Color? c = UI4ColorPicker.ShowDialog("选择背景色", Colors.CornflowerBlue);

// 实例方式
var picker = new UI4ColorPicker("选择颜色", Colors.Green);
if (picker.Show(this) == true)
    Apply(picker.SelectedColor);
```

#### 说明

对话框可拖拽边框八向调整大小；标题、按钮文字跟随 `UI4MultiLanguage` 当前语言。

---

### UI4Panel

容器面板：阴影、圆角、描边、悬浮缩放动画。

**继承自**：`ContentControl`

#### 可设置属性

| 属性 | 类型 | 默认值 | 说明 |
|---|---|---|---|
| `CornerRadius` | `CornerRadius` | `12` | 圆角 |
| `BorderBrush` | `Color` | `#3C788CC8` | 常态描边色 |
| `HoverBorderBrush` | `SolidColorBrush` | `#46788CC8` 画笔 | 悬浮描边 |
| `BorderThickness` | `Thickness` | `1` | 描边厚度 |
| `ShadowDepth` | `double` | `0` | 阴影深度 |
| `ShadowBlurRadius` | `double` | `15` | 阴影模糊 |
| `ShadowOpacity` | `double` | `0.1` | 阴影不透明度 |
| `ShadowColor` | `Color` | `Black` | 阴影颜色 |
| `ContentPadding` | `Thickness` | `0` | 内容内边距 |
| `HoverAnimationDuration` | `Duration` | `200ms` | 悬浮动画时长 |
| `HoverScale` | `double` | `1.005` | 悬浮缩放倍率 |

#### 示例

```xml
<ui:UI4Panel Width="300" Height="200">
    <TextBlock Text="面板内容" HorizontalAlignment="Center" VerticalAlignment="Center"/>
</ui:UI4Panel>

<ui:UI4Panel Width="800" Height="600" ShadowDepth="15" ShadowOpacity="0.6" HoverScale="1.1">
    <Grid Margin="20"><TextBlock Text="阴影 + 悬浮缩放"/></Grid>
</ui:UI4Panel>
```

#### 说明

阴影与缩放分层实现（阴影层不含文字），因此悬浮缩放时文字保持清晰。

---

### UI4Pivot / UI4PivotItem

滑动切换页签，切换时内容平移过渡。

**继承自**：`Selector` / `HeaderedContentControl`

#### UI4Pivot 可设置属性

| 属性 | 类型 | 默认值 | 说明 |
|---|---|---|---|
| `IsBrand` | `bool` | `false` | 是否显示品牌文字 |
| `SelectedItemForeground` | `Color` | `#FF0078D4` | 选中项颜色 |
| `ItemFontSize` | `double` | `20` | 项字号 |
| `SelectedFontSize` | `double` | `25` | 选中项字号 |
| `BrandFontSize` | `double` | `22` | 品牌文字字号 |
| `ItemFontWeight` | `FontWeight` | `Normal` | 项字重 |
| `BrandFontWeight` | `FontWeight` | `SemiBold` | 品牌文字字重 |
| `ItemForeground` | `Color` | `#DC000000` | 项颜色 |
| `ItemHoverForeground` | `Color` | `#DC000000` | 项悬浮颜色 |
| `ItemPadding` | `Thickness` | `10,8,10,8` | 项内边距 |
| `ItemMargin` | `Thickness` | `5,0,5,0` | 项外边距 |

#### 示例

```xml
<ui:UI4Pivot ItemFontSize="18" SelectionChanged="Pivot_SelectionChanged">
    <ui:UI4PivotItem Header="首页"><TextBlock Text="首页内容"/></ui:UI4PivotItem>
    <ui:UI4PivotItem Header="设置"><TextBlock Text="设置内容"/></ui:UI4PivotItem>
    <ui:UI4PivotItem Header="关于"><TextBlock Text="关于内容"/></ui:UI4PivotItem>
</ui:UI4Pivot>
```

---

### UI4Tab / UI4TabItem

浏览器风格标签页：图标（文字图标或图片）、关闭按钮、新增按钮。

**继承自**：`Selector` / `HeaderedContentControl`

#### UI4Tab 可设置属性

| 属性 | 类型 | 默认值 | 说明 |
|---|---|---|---|
| `HeaderBackground` | `Color` | `#0A000000` | 标签栏背景 |
| `TabBackground` | `Color` | `Transparent` | 标签背景 |
| `TabSelectedBackground` | `Color` | `White` | 选中标签背景 |
| `TabHoverBackground` | `Color` | `#1E000000` | 标签悬浮背景 |
| `TabForeground` | `Color` | `#C8000000` | 标签文字色 |
| `TabSelectedForeground` | `Color` | `#FF000000` | 选中标签文字色 |
| `CloseButtonColor` | `Color` | `#96000000` | 关闭按钮颜色 |
| `TabFontSize` | `double` | `13` | 标签字号 |
| `TabPadding` | `Thickness` | `12,8,8,8` | 标签内边距 |
| `ShowAddButton` | `bool` | `true` | 是否显示新增按钮 |
| `AddButtonColor` | `Color` | `#96000000` | 新增按钮颜色 |
| `IsBrand` | `bool` | `false` | 是否显示品牌区 |

#### UI4TabItem 可设置属性

| 属性 | 类型 | 默认值 | 说明 |
|---|---|---|---|
| `TextIcon` | `string` | `null` | 文字图标（如 Segoe MDL2 Assets 码位） |
| `TextIconFontFamily` | `FontFamily` | `Segoe MDL2 Assets` | 文字图标字体 |
| `ImageSource` | `ImageSource` | `null` | 图片图标 |
| `IconSize` | `double` | `16` | 图标尺寸 |
| `IsClosable` | `bool` | `true` | 是否可关闭 |

#### 事件

| 事件 | 签名 | 说明 |
|---|---|---|
| `AddTab` | `RoutedEventHandler` | 点击新增按钮 |
| `CloseTab` | `TabCloseRoutedEventHandler` | 请求关闭标签；`e.TabItem` 为被关标签。不置 `e.Handled` 时控件自行移除 |

#### 示例

```xml
<ui:UI4Tab x:Name="MyTab" AddTab="MyTab_AddTab" CloseTab="MyTab_CloseTab">
    <ui:UI4TabItem Header="主页" TextIcon="&#xE80F;">
        <Grid Background="White"><TextBlock Text="主页内容"/></Grid>
    </ui:UI4TabItem>
    <ui:UI4TabItem Header="设置" TextIcon="&#xE713;" IsClosable="False">
        <Grid Background="White"><TextBlock Text="设置内容"/></Grid>
    </ui:UI4TabItem>
</ui:UI4Tab>
```

```csharp
private void MyTab_AddTab(object sender, RoutedEventArgs e)
{
    var item = new UI4TabItem
    {
        Header = "新标签",
        TextIcon = "\uE723",
        Content = new TextBlock { Text = "动态标签内容", Margin = new Thickness(12) }
    };
    MyTab.Items.Add(item);
    MyTab.SelectedItem = item;
}

private void MyTab_CloseTab(object sender, TabCloseRoutedEventArgs e)
{
    // 不设置 e.Handled，UI4Tab 会自行移除该标签
    StatusText.Text = "关闭：" + e.TabItem.Header;
}
```

---

### UI4NavigationView

侧边导航视图：可滚动项 + 固定底部项，选中项内容显示在右侧。

**继承自**：`ItemsControl`

#### 可设置属性

| 属性 | 类型 | 默认值 | 说明 |
|---|---|---|---|
| `Header` | `string` | 空 | 顶部标题 |
| `LeftPanelBackground` | `Brush` | `#0A000000` 画笔 | 左栏背景 |
| `LeftPanelWidth` | `double` | `NaN`（自动） | 左栏宽度 |
| `ItemFontSize` | `double` | `10` | 项字号 |
| `ItemBackground` | `Brush` | `Transparent` | 项背景 |
| `ItemHoverColor` | `Color` | `#0A000000` | 项悬浮色 |
| `ItemPressedBackground` | `Color` | `White` | 项按下背景 |
| `ItemPressedForeground` | `Color` | `Black` | 项按下文字色 |
| `ItemHoverForeground` | `Color` | `Black` | 项悬浮文字色 |
| `ItemForeground` | `Brush` | `Black` | 项文字色 |
| `SelectedItemBackground` | `Brush` | `White` | 选中项背景 |
| `SelectedItem` | `UI4NavigationViewItem` | `null` | 当前选中项 |

#### Item 可设置属性（`UI4NavigationViewItem` / `UI4NavigationViewBottomItem`）

| 属性 | 类型 | 说明 |
|---|---|---|
| `Header` | `string` | 项标题 |
| `ImageSource` | `ImageSource` | 图片图标 |
| `TextIcon` | `string` | 文字图标 |
| `TextIconFontFamily` | `FontFamily` | 文字图标字体 |

#### 示例

```xml
<ui:UI4NavigationView Header="导航视图" LeftPanelBackground="#FFEDF1F8"
                      ItemBackground="Transparent" SelectedItemBackground="White">
    <ui:UI4NavigationViewItem TextIcon="&#xE104;" Header="代码">
        <TextBlock Text="代码页内容" Margin="16"/>
    </ui:UI4NavigationViewItem>
    <ui:UI4NavigationViewBottomItem TextIcon="&#xE713;" Header="设置">
        <TextBlock Text="设置页内容" Margin="16"/>
    </ui:UI4NavigationViewBottomItem>
</ui:UI4NavigationView>
```

---

### UI4ListBox

列表框，支持普通 / 圆点 / 编号三种列表样式。

**继承自**：`ListBox`

#### 可设置属性

| 属性 | 类型 | 默认值 | 说明 |
|---|---|---|---|
| `CornerRadius` | `CornerRadius` | `6` | 圆角 |
| `BorderNormalColor` | `Color` | `#FF2563EB` | 边框色（跟随主题 `BorderNormal`） |
| `PanelBackground` | `Brush` | `White` | 面板背景（跟随主题 `Surface`） |
| `TextColor` | `Color` | `Black` | 文字颜色（跟随主题 `TextForeground`） |
| `ItemPadding` | `Thickness` | `12,8,12,8` | 项内边距 |
| `ItemCornerRadius` | `CornerRadius` | `6` | 项圆角 |
| `HoverBackground` | `Color` | `#0AF5FFFF` | 项悬浮背景（跟随主题 `HoverOverlay`，浅色下为 `#14000000`） |
| `HoverForeground` | `Color` | `#DC000000` | 项悬浮文字色 |
| `PressedBackground` | `Color` | `#FF2563EB` | 项按下背景 |
| `PressedForeground` | `Color` | `White` | 项按下文字色 |
| `ListStyleType` | `ListStyleType` | `None` | 列表样式 |
| `NumberCircleBackground` | `Brush` | 蓝色渐变 | 编号圆底（Number 样式） |

#### 枚举 `ListStyleType`

| 值 | 说明 |
|---|---|
| `None` | 普通列表 |
| `Disc` | 圆点列表 |
| `Number` | 编号列表 |

#### 示例

```xml
<ui:UI4ListBox Width="200" Height="220">
    <ListBoxItem>Item 1</ListBoxItem>
    <ListBoxItem>Item 2</ListBoxItem>
</ui:UI4ListBox>

<ui:UI4ListBox ListStyleType="Disc" HoverForeground="Black" HoverBackground="#0C000000"
               Width="200" Height="220">
    <ListBoxItem>Item 1</ListBoxItem>
</ui:UI4ListBox>

<ui:UI4ListBox ListStyleType="Number" NumberCircleBackground="Green" Width="200" Height="220">
    <ListBoxItem>Item 1</ListBoxItem>
</ui:UI4ListBox>
```

---

### UI4ListView

卡片式列表：每项为带阴影的圆角卡片，悬浮描边 + 轻微缩放（文字保持清晰）。

**继承自**：`ListBox`

#### 可设置属性

| 属性 | 类型 | 默认值 | 说明 |
|---|---|---|---|
| `ItemWidth` / `ItemHeight` | `double` | `NaN` | 项尺寸（NaN = 自适应） |
| `ItemCornerRadius` | `CornerRadius` | `12` | 卡片圆角 |
| `ItemBackground` | `Brush` | `White` | 卡片背景 |
| `ItemBorderBrush` | `Color` | `#3C788CC8` | 卡片边框色 |
| `ItemHoverBorderBrush` | `Color` | `#FF0078D4` | 悬浮边框色 |
| `ItemBorderThickness` | `Thickness` | `1` | 边框厚度 |
| `ItemPadding` | `Thickness` | `0` | 卡片内边距 |
| `ItemMargin` | `Thickness` | `5` | 卡片外边距 |
| `HoverScale` | `double` | `1.01` | 悬浮缩放倍率 |
| `HoverAnimationDuration` | `Duration` | `200ms` | 悬浮动画时长 |
| `ShadowColor` | `Color` | `#23000000` | 阴影颜色 |
| `ShadowBlurRadius` | `double` | `12` | 阴影模糊 |
| `ShadowDepth` | `double` | `0` | 阴影深度 |
| `ShadowOpacity` | `double` | `0` | 阴影不透明度 |

#### 示例

```xml
<ui:UI4ListView ItemsSource="{Binding Cards}" ItemBackground="White"
                ShadowDepth="15" ShadowOpacity="0.2" HoverScale="1.01"
                SelectionChanged="Cards_SelectionChanged">
    <ui:UI4ListView.ItemTemplate>
        <DataTemplate>
            <Border Background="LightYellow" CornerRadius="10">
                <StackPanel Margin="20">
                    <ui:UI4TextBlock Content="{Binding Title}" Margin="0,10,0,4"/>
                    <ui:UI4TextBlock Content="{Binding Description}" TextWrapping="Wrap"/>
                </StackPanel>
            </Border>
        </DataTemplate>
    </ui:UI4ListView.ItemTemplate>
</ui:UI4ListView>
```

---

### UI4GridView

网格卡片视图：按可用宽度自适应列数，卡片悬浮缩放。

**继承自**：`ListBox`

#### 可设置属性

与 `UI4ListView` 相同，另含：

| 属性 | 类型 | 默认值 | 说明 |
|---|---|---|---|
| `ItemWidth` | `double` | `300` | 卡片宽度（决定列数） |
| `ItemHeight` | `double` | `220` | 卡片高度 |

#### 示例

```xml
<ui:UI4GridView ItemsSource="{Binding Cards}" ItemHeight="200" ItemWidth="250"
                ShadowDepth="15" ShadowOpacity="0.3" HoverScale="1.1">
    <ui:UI4GridView.ItemTemplate>
        <DataTemplate>
            <Border Background="{Binding Background}" CornerRadius="10">
                <StackPanel Margin="20">
                    <Ellipse Width="36" Height="36" Fill="{Binding IconColor}"/>
                    <ui:UI4TextBlock Content="{Binding Title}" Margin="0,8,0,3"/>
                    <ui:UI4TextBlock Content="{Binding Description}" TextWrapping="Wrap"/>
                </StackPanel>
            </Border>
        </DataTemplate>
    </ui:UI4GridView.ItemTemplate>
</ui:UI4GridView>
```

---

### UI4ScrollViewer

美化滚动条的 ScrollViewer，可选平滑滚动动画。

**继承自**：`ScrollViewer`

#### 可设置属性

| 属性 | 类型 | 默认值 | 说明 |
|---|---|---|---|
| `IsSmoothScrollEnabled` | `bool` | `true` | 滚轮平滑滚动动画 |

#### 示例

```xml
<ui:UI4ScrollViewer Width="400" Height="200" IsSmoothScrollEnabled="True">
    <StackPanel Margin="20" Height="600">
        <TextBlock Text="滚动内容..."/>
    </StackPanel>
</ui:UI4ScrollViewer>
```

#### 说明

滚动时滚动条淡入、静止后淡出；`ScrollToTop` / `ScrollToEnd` 等基类方法均可用。

---

### UI4NotifyIcon

系统托盘图标：P/Invoke 实现（非 WinForms），自定义右键菜单、悬浮提示、三种鼠标事件。

**继承自**：`FrameworkElement`，实现 `IDisposable`

#### 可设置属性

| 属性 | 类型 | 默认值 | 说明 |
|---|---|---|---|
| `MenuActivation` | `PopupActivationMode` | `RightClick` | 菜单触发方式 |
| `IconSource` | `ImageSource` | `null`（回退系统默认图标） | 托盘图标 |
| `ToolTipText` | `string` | 空 | 悬浮提示 |
| `MenuWidth` | `double` | `160` | 右键菜单宽度 |
| `MenuItemPadding` | `Thickness` | `12,8,12,8` | 菜单项内边距 |
| `MenuBorderColor` | `Color` | 浅灰 | 菜单边框色 |
| `MenuBackground` | `Brush` | `White` | 菜单背景 |
| `MenuHoverBg` | `Color` | `#0A000000` | 菜单悬浮色 |
| `MenuCornerRadius` | `CornerRadius` | `6` | 菜单圆角 |

#### 事件

`TrayLeftMouseUp`、`TrayRightMouseDown`、`TrayMouseDoubleClick`（均为 `RoutedEventHandler`）

#### 示例

```xml
<ui:UI4NotifyIcon x:Name="TrayIcon" Visibility="Collapsed"
                  ToolTipText="My App"
                  TrayLeftMouseUp="TrayIcon_TrayLeftMouseUp"
                  TrayRightMouseDown="TrayIcon_TrayRightMouseDown"/>
```

```csharp
TrayIcon.Visibility = Visibility.Visible;   // 显示即注册托盘图标
// 关闭窗口时：
TrayIcon.Visibility = Visibility.Collapsed;
TrayIcon.Dispose();
```

#### 说明

- `IconSource` 支持 pack / 文件路径；解析失败自动回退 `SystemIcons.Application`。
- Windows 10/11 默认把新托盘图标收进溢出区，需在任务栏设置中拖出。

---

### UI4Menu / UI4MenuElementItem / UI4MenuSeparatorElement

菜单栏：支持文字图标、图标字体、KeyTip 提示与分隔符。

**继承自**：`Menu` / `MenuItem` / `Separator`

#### UI4Menu 可设置属性

| 属性 | 类型 | 默认值 | 说明 |
|---|---|---|---|
| `BarBackground` | `Brush` | 浅灰 | 菜单栏背景 |
| `ItemHoverBrush` | `Brush` | `#14000000` 画笔 | 项悬浮背景 |
| `PopupCornerRadius` | `CornerRadius` | `6` | 下拉面板圆角 |
| `TextForeground` | `Brush` | 深灰 | 文字颜色 |
| `SeparatorColor` | `Brush` | 浅灰 | 分隔符颜色 |

#### UI4MenuElementItem 可设置属性

| 属性 | 类型 | 默认值 | 说明 |
|---|---|---|---|
| `TextIcon` | `string` | 空 | 文字图标 |
| `IconFontFamily` | `FontFamily` | `Segoe UI Symbol` | 图标字体 |
| `IconFontSize` | `double` | `14` | 图标字号 |
| `IconForeground` | `Brush` | `null` | 图标颜色 |
| `KeyTip` | `string` | 空 | 键提示（如 `(F)`） |

#### 示例

```xml
<ui:UI4Menu>
    <ui:UI4MenuElementItem Header="文件" KeyTip="(F)">
        <ui:UI4MenuElementItem Header="新建" TextIcon="&#xE710;"
                               IconFontFamily="Segoe MDL2 Assets" IconFontSize="14"
                               Click="New_Click"/>
        <ui:UI4MenuSeparatorElement/>
        <ui:UI4MenuElementItem Header="退出" TextIcon="&#xE7E8;" Click="Exit_Click"/>
    </ui:UI4MenuElementItem>
    <ui:UI4MenuElementItem Header="编辑" KeyTip="(E)">
        <ui:UI4MenuElementItem Header="撤销" TextIcon="&#xE7A7;"/>
        <ui:UI4MenuElementItem Header="重做" TextIcon="&#xE7A6;"/>
    </ui:UI4MenuElementItem>
</ui:UI4Menu>
```

---

### UI4Grid

默认带渐变背景的 `Grid`，其余与 `Grid` 完全一致。

**继承自**：`Grid`

#### 示例

```xml
<ui:UI4Grid Margin="20">
    <Grid.RowDefinitions>
        <RowDefinition Height="Auto"/>
        <RowDefinition Height="*"/>
    </Grid.RowDefinitions>
    <ui:UI4TextBlock Text="标题" Grid.Row="0" FontSize="24"/>
    <ui:UI4ListBox Grid.Row="1" Width="300" Height="200">
        <ListBoxItem>Item 1</ListBoxItem>
    </ui:UI4ListBox>
</ui:UI4Grid>

<!-- 覆盖默认背景 -->
<ui:UI4Grid Background="White"/>
```

---

### UI4MessageBox

自定义消息对话框：圆角卡片、图标、可拖拽边框八向缩放。

**继承自**：`Window`

#### 枚举 `UI4MessageBoxButtons`

| 值 | 说明 |
|---|---|
| `OK` | 仅确定 |
| `OKCancel` | 确定 / 取消 |

#### 静态方法

```csharp
public static bool? Show(string content,
                         string title = null,
                         UI4MessageBoxButtons buttons = UI4MessageBoxButtons.OK,
                         double width = 460)
```

返回 `true`（确定）/ `false`（取消）/ `null`（关闭）。

#### 构造函数

```csharp
public UI4MessageBox(string title, string content,
                     UI4MessageBoxButtons buttonMode = UI4MessageBoxButtons.OK)
```

#### 示例

```csharp
bool? r = UI4MessageBox.Show("这是内容。", "提示", UI4MessageBoxButtons.OK);

bool? r2 = UI4MessageBox.Show("确认执行该操作吗？", "请确认", UI4MessageBoxButtons.OKCancel);
if (r2 == true) { /* 执行 */ }
```

#### 说明

标题缺省取 `UI4MultiLanguage.Get(UI4LanguageKey.Notice)`；按钮文字跟随当前语言。

---

### UI4ContextMenu

代码组件形式的右键菜单：图标 + 文字 + 命令 + 可用性判断。

#### 成员

| 成员 | 说明 |
|---|---|
| `Width` / `ItemPadding` / `BorderColor` / `Background` / `HoverBackground` | 外观属性 |
| `IsOpen` | 是否打开（只读） |
| `AddItem(UI4MenuItem item)` | 添加自定义项 |
| `AddItem(UI4MenuItemType type, Action command, Func<bool> canExecute = null)` | 按内置类型添加（自动取图标与多语言文本） |
| `Attach(UIElement target)` / `Detach()` | 绑定 / 解绑右键目标 |
| `Open()` / `Close()` | 手动开合 |

#### 示例

```csharp
var menu = new UI4ContextMenu
{
    Width = 190,
    Background = Brushes.White,
    BorderColor = Color.FromRgb(200, 200, 210),
    HoverBackground = Color.FromArgb(14, 0, 0, 0)
};

menu.AddItem(new UI4MenuItem(UI4MenuItemType.Copy, "复制文本", UI4MenuIcons.Copy,
                             () => DoCopy()));
menu.AddItem(UI4MenuItemType.SelectAll, () => DoSelectAll());
menu.Attach(myControl);          // 在 myControl 上右键弹出
```

> `UI4CodeEditor` 内置了该菜单（撤销/重做/剪切/复制/粘贴/删除/全选），右键即可体验。

---

### UI4CodeEditor

基于 AvalonEdit 的代码编辑器：默认 C# 语法高亮、行号、自动换行、Consolas 字体，
并内置 `UI4ContextMenu` 右键菜单与美化滚动条。

**继承自**：`ICSharpCode.AvalonEdit.TextEditor`

#### 示例

```xml
<ui:UI4CodeEditor x:Name="CodeEditor" Height="220"/>
```

```csharp
CodeEditor.Text = "public class Sample { }";
CodeEditor.SyntaxHighlighting =
    ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinition("XML");
```

---

### UI4MultiLanguage

zh / en 双语字符串服务，供 `UI4MessageBox`、`UI4ColorPicker`、`UI4ContextMenu` 等内部使用，也可在业务代码中调用。

#### 成员

| 成员 | 说明 |
|---|---|
| `Current` | 当前语言字典（按 `CurrentUICulture` 解析） |
| `Get(UI4LanguageKey key)` | 取字符串 |
| `Refresh()` | 切换语言后刷新缓存 |
| `GetStrings(string lang)` | 取指定语言字典（`"zh"` / 其余为 en） |

`UI4LanguageKey`：`OK`、`Cancel`、`Notice`、`ColorPicker`、`Undo`、`Redo`、`Cut`、`Copy`、`Paste`、`Delete`、`SelectAll`

#### 示例

```csharp
CultureInfo.CurrentUICulture = new CultureInfo("en-US");
UI4MultiLanguage.Refresh();
UI4ContextMenuLanguage.Refresh();
string ok = UI4MultiLanguage.Get(UI4LanguageKey.OK);   // "OK"
```

---

## 六、完整使用教程：从零搭建一个应用

以下以 Visual Studio 2022 + .NET Framework 4.8 为例，从零搭一个使用本库的应用。

### 步骤 1：新建项目

新建「WPF 应用（.NET Framework）」项目，或在 SDK 风格 csproj 中写：

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net48</TargetFramework>
    <UseWPF>true</UseWPF>
    <LangVersion>7.3</LangVersion>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\StartUI4Controls\StartUI4Controls.csproj" />
    <!-- 或 <PackageReference Include="StartUI4.WPF" Version="1.0.20" /> -->
  </ItemGroup>
</Project>
```

### 步骤 2：MainWindow.xaml 骨架

```xml
<Window x:Class="MyApp.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:ui="clr-namespace:StartUI4Controls;assembly=StartUI4Controls"
        Title="MyApp" Width="900" Height="600" Background="#FFF4F6FB">
    <Grid Margin="24">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>

        <ui:UI4TextBlock Text="我的应用" FontSize="30" FontWeight="Bold"/>

        <ui:UI4Panel Grid.Row="1" Margin="0,16,0,0" ShadowDepth="12" ShadowOpacity="0.35"
                     HoverScale="1.0" CornerRadius="14">
            <StackPanel Margin="24">
                <ui:UI4TextBox x:Name="NameBox" Width="320" PlaceholderText="请输入名称"
                               ShowClearButton="True" HorizontalAlignment="Left"/>
                <ui:UI4PasswordBox Width="320" Margin="0,10,0,0" PlaceholderText="请输入密码"
                                   HorizontalAlignment="Left"/>
                <StackPanel Orientation="Horizontal" Margin="0,16,0,0">
                    <ui:UI4Switch x:Name="RememberSwitch"/>
                    <TextBlock Text="记住我" VerticalAlignment="Center" Margin="10,0,24,0"/>
                    <ui:UI4Button Content="登录" Width="120" Height="36" Click="Login_Click"/>
                </StackPanel>
            </StackPanel>
        </ui:UI4Panel>
    </Grid>
</Window>
```

### 步骤 3：代码behind

```csharp
private void Login_Click(object sender, RoutedEventArgs e)
{
    if (string.IsNullOrEmpty(NameBox.Text))
    {
        UI4MessageBox.Show("名称不能为空", "提示", UI4MessageBoxButtons.OK);
        return;
    }

    bool? ok = UI4MessageBox.Show(
        string.Format("以 {0} 登录？", NameBox.Text),
        "确认", UI4MessageBoxButtons.OKCancel);

    if (ok == true)
        Title = "已登录：" + NameBox.Text + (RememberSwitch.IsOn ? "（记住我）" : "");
}
```

### 步骤 4：数据绑定列表

```csharp
public class CardItem
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Brush Background { get; set; }
    public Brush IconColor { get; set; }
}

DataContext = new { Cards = new List<CardItem> { /* ... */ } };
```

```xml
<ui:UI4GridView ItemsSource="{Binding Cards}" ItemWidth="240" ItemHeight="180"
                HoverScale="1.05" ShadowOpacity="0.25">
    <ui:UI4GridView.ItemTemplate>
        <DataTemplate>
            <Border Background="{Binding Background}" CornerRadius="10">
                <StackPanel Margin="16">
                    <Ellipse Width="32" Height="32" Fill="{Binding IconColor}" HorizontalAlignment="Left"/>
                    <ui:UI4TextBlock Content="{Binding Title}" Margin="0,8,0,3"/>
                    <ui:UI4TextBlock Content="{Binding Description}" TextWrapping="Wrap"/>
                </StackPanel>
            </Border>
        </DataTemplate>
    </ui:UI4GridView.ItemTemplate>
</ui:UI4GridView>
```

### 步骤 5：托盘图标与退出清理

```csharp
protected override void OnClosed(EventArgs e)
{
    TrayIcon.Visibility = Visibility.Collapsed;
    TrayIcon.Dispose();
    base.OnClosed(e);
}
```

### 步骤 6：运行并确认运行时

启动后查看窗口中自行显示的 `RuntimeInformation.FrameworkDescription`，
或临时加一行 `MessageBox.Show(System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);`
确认输出为 `.NET Framework 4.8.x`。

---

## 七、Demo 工程指南

仓库自带完整演示工程 `samples/StartUI4Demo`（net48），覆盖全部公开控件。

### 构建与运行

```bash
# 本仓库没有 .sln，按 csproj 构建；组件库开了 GeneratePackageOnBuild 且缺 LICENSE 打包元数据，
# 必须带 -p:GeneratePackageOnBuild=false，否则 pack 阶段报 NU5019 拖垮整个构建。
dotnet build samples/StartUI4Demo/StartUI4Demo.csproj -p:GeneratePackageOnBuild=false
samples\StartUI4Demo\bin\Debug\net48\StartUI4Demo.exe            # 直接运行
samples\StartUI4Demo\bin\Debug\net48\StartUI4Demo.exe --tab=11   # 直接打开指定分页（0 起）
```

或在 Visual Studio 中：**右键 `StartUI4Demo` → 设为启动项目** 后 F5。
注意：把组件库工程加进 .sln 会让 VS 构建它（命令行 MSBuild 本来就会构建），而库目录名含中文时
VS 会因 URI 转义超过 260 字符报"路径太长"，所以 Demo 与库都建议用 csproj 方式打开或构建。

每个组件页按同一条动线组织：**外观属性 → 交互事件 → 状态反馈 → 与主题的关系**，
所有可点项都有就近回显或底部状态栏反馈，不存在"点了没反应"的示例。

### 分页结构

| 页 | 内容 |
|---|---|
| 按钮与开关 | UI4Button（含**启动态/禁用态**并排对比与可用性切换示例）/ UI4CheckBox / UI4Radio / UI4Switch（切全局主题） |
| 文本输入 | UI4TextBox / UI4PasswordBox / UI4CodeEditor（含 Ctrl+C·X·V 原生接管与密码模式差异说明） |
| 文本显示 | UI4TextBlock / UI4FlipTextBlock |
| 选择器 | UI4ComboBox（含长文本用例与选中回显）/ UI4Slider（ValueChanged 就近回显）/ UI4CircleSlider（AddValueChanged 回显） |
| 进度指示 | UI4ProgressBar（推进 / 切换不确定模式）/ UI4ProgressRing（启停、进度推进） |
| 列表与网格 | UI4ListBox（None/Disc/Number 三种样式）/ UI4ListView / UI4GridView（**自适应列数**：拖动窗口宽度看实时列数） |
| 导航容器 | UI4Pivot / UI4Tab / UI4NavigationView / UI4ScrollViewer（平滑滚到顶部/底部） |
| 布局面板 | UI4Grid / UI4Panel |
| 对话框 | UI4MessageBox / UI4ColorPicker |
| 菜单与托盘 | UI4Menu / UI4ContextMenu（**真操作剪贴板**，无选区时自动置灰）/ UI4NotifyIcon（右键弹真菜单、左键与双击上报）/ UI4MultiLanguage（8 套语言） |
| 局部主题 | UI4ThemeScope（左卡片深色 / 右卡片跟随全局的对照、两卡各含 `UI4ComboBox`+`UI4ListBox` 底色对照、作用域键下拉、嵌套作用域、`ScopeWindow` 异主题窗口、高对比度入口） |
| 剪贴板与原生交互 | `UI4Clipboard` 三个方法逐个动手验证（`ContainsText` 不抢锁 / 写入后可去记事本 Ctrl+V 验证 / 读回）+ `UI4ContextMenu.Open/Close/IsOpen` + 库内部按键接管说明 |
| 主题与强调色 | `UI4Theme.SetAccent`（含取色器选色、恢复内置定义）、`Register(UI4ThemeDefinition)` 自定义 `ocean` 主题 + `ThemeKeys` 下拉切换、`Save/ApplyPersisted`（用 `JsonThemePersistence` 写本程序目录，**不碰注册表**）、`UI4WindowTitleBar.Apply/SupportsCaptionColors/SetEnabled` |

### 命令行参数

```bash
StartUI4Demo.exe --tab=5     # 直接打开第 5 页（0 起），便于自动化逐页验证
```

### 自动化验证脚本

| 脚本 | 用途 |
|---|---|
| `shot.ps1 -Tab N` | 启动 Demo 打开第 N 页并用 `PrintWindow` 截图到 `shots/tabN.png` |
| `interact.ps1 -Scenario msgbox` | 打开/关闭 UI4MessageBox 并校验返回值 |
| `interact.ps1 -Scenario color` | 打开 UI4ColorPicker 截图 |
| `interact.ps1 -Scenario menu` | 展开 UI4Menu 下拉 |
| `interact.ps1 -Scenario ctx` | 右键弹出 UI4ContextMenu |
| `interact.ps1 -Scenario tabadd` | 点击加号动态新增标签 |
| `interact.ps1 -Scenario tray` | 启用托盘图标并截取任务栏溢出区 |
| `interact.ps1 -Scenario hover` | 悬浮 UI4ListView 卡片，验证文字不模糊 |
| `interact.ps1 -Scenario combo` | 长文本下拉框闭合/展开态截图 |
| `theme.ps1` | UIA 走查：切深色 → Tab 往返 → 切回亮色，检查 `demo-errors.log` |
| `p2verify.ps1` | **进程内值断言**：STA 加载已构建 dll，离屏窗口承载 CheckBox/Radio/TextBox/PasswordBox，`Template.FindName` 读解析后的画刷色，深/浅各 16 项 + 显式覆盖 + `Style` 不重建 |
| `p3verify.ps1` | **进程内值断言**：局部作用域（G1 资源 / G2 换入 / G3 令牌化三组控件）、全局切换不串味、作用域可撤销、高对比度 30 令牌齐备、System 解析、持久化往返、`SetAccent` 传播、H 组：ComboBox/ListBox 选中框与面板底色及焦点渐变在三主题下跟随令牌（共 83 项） |
| `scopewalk.ps1` | UIA 走查 Demo 第 11 页：作用域键切换、全局高对比度/跟随系统不污染作用域、撤销作用域、打开 `ScopeWindow` 并读其自证文本 |
| `titlebar.ps1` | **进程内值断言**：用自备 `DwmGetWindowAttribute` 回读标题栏深色标志，覆盖全局切换、`Enabled=false` 豁免、窗口级作用域、三主题 COLORREF 计算、能力探测一致性、三条染色通路（手动 `Apply` / 控件加载补染 / `ThemeChanged` 清扫），共 31 项 |
| `titlebar-live.ps1` | **跨进程证据**：启动真实 Demo，由本脚本独立读 DWM 属性 —— 亮色启动标志 0 → 打开 `ScopeWindow`（异主题）其标题栏 1 而主窗仍 0 → 点「全局高对比度」主窗变 1，共 9 项 |

> 本机对 WPF 窗口的屏幕抓取（`PrintWindow` 与 `CopyFromScreen`）返回全白，且未修改的基线同样全白，属环境限制；
> 因此主题相关验证一律用 `p2verify.ps1` / `p3verify.ps1` 的进程内取值断言 + `theme.ps1` / `scopewalk.ps1` 的 UIA 走查，
> 截图脚本仅在环境可用时补充。所有脚本用 `powershell -STA -ExecutionPolicy Bypass -File <脚本>` 运行。

所有截图存于 `shots/`；运行期异常会写入 Demo 输出目录的 `demo-errors.log` 并弹窗。

---

## 八、与上游（net6 版）的差异与已知问题

完整清单见 [`PORTING.md`](PORTING.md)，摘要：

1. **目标框架**：`net6.0-windows7.0` → `net48`；语言级别锁定 C# 7.3。
2. **依赖**：移除源码中从未使用的 `Microsoft.Data.Sqlite`；保留 `AvalonEdit`、`System.Data.SQLite`。
3. **本移植版修复的上游缺陷**：
   - `UI4Switch` 被拉伸时轨道与滑块分离；
   - `UI4ListView` / `UI4GridView` 悬浮缩放导致文字模糊（Effect 与 ScaleTransform 同层）；
   - `UI4ComboBox` 选中项长文本溢出、下拉弹出层裁切长选项；
   - 重定义依赖属性缺少 `new` 关键字产生的 CS0108 警告。
4. **上游遗留、未改动**：
   - `UI4TextBlock.GradientStart/End` 为未实现的死属性（渐变请用 `Foreground`）；
   - 5 个只写不读的私有字段（CS0414 警告保留）；
   - `UI4DataGrid` / `UI43DSphere` 为 internal 死代码，且 SQLite 原生 `SQLite.Interop.dll` 不随类库部署。

---

## 九、附录

### A. 命名空间引用

```xml
<Window xmlns:ui="clr-namespace:StartUI4Controls;assembly=StartUI4Controls" ...>
```

完整示例：

```xml
<Window x:Class="YourApp.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:ui="clr-namespace:StartUI4Controls;assembly=StartUI4Controls"
        Title="MainWindow" Height="800" Width="1200">
    <Grid>
        <ui:UI4Button Content="Hello StartUI4" Width="200" Height="40"/>
    </Grid>
</Window>
```

### B. 解决方案配置与 4.8 的关系

`.sln` 中**不含**任何 4.8 字样：编译期 4.8 来自各 csproj 的 `<TargetFramework>net48</TargetFramework>`，
运行期 4.8 来自自动生成的 `exe.config` 中 `<supportedRuntime sku=".NETFramework,Version=v4.8"/>`。
sln 的 `Debug/Release × Any CPU/x64/x86` 只决定构建配置与平台映射（x64/x86 均映射到 Any CPU），
不影响目标框架。详见 `PORTING.md` 第 7 节。

### C. 未公开类型

| 类型 | 状态 |
|---|---|
| `UI4DataGrid` | `internal`，SQLite 分页加载表格，上游即无引用 |
| `UI43DSphere` | `internal`，Media3D 纹理球体，上游即无引用 |
| `Internal/*`（`ClipboardCommandTakeover`、`ScrollBarResources`、`ThemeSync`、`WindowAnimationHelper`、`WindowResizeBehavior`、`ColorToBrushConverter`） | `internal`，库内部实现 |
| 部分 `*Converter`（`NavigationColorToBrushConverter`、`Tab*Converter`、`TextOrContentConverter`） | `internal`，服务于各自控件模板 |
| 其余 `*Converter`（`IndexPlusOneConverter`、`ObjectIsStringConverter`、`BoolToVisibilityConverter`、`PlaceholderVisibilityConverter`、`InnerPaddingConverter`） | **`public`**，随库导出，宿主 XAML 可直接复用 |

### D. 发布产物清单（哪些文件是运行必需的）

以 `samples/StartUI4Demo/bin/Release/net48` 构建出的 7 个文件为例（合计约 1472 KB）：

| 文件 | 大小 | 运行必需 | 说明 |
|---|---|---|---|
| `StartUI4Demo.exe` | 35 KB | **必需** | 入口程序集 |
| `StartUI4Controls.dll` | 360 KB | **必需** | 控件库实现 |
| `ICSharpCode.AvalonEdit.dll` | 621 KB | **必需**（当前 Demo） | `UI4CodeEditor` 继承自 AvalonEdit 的 `TextEditor`，主窗口 XAML 在构造期即解析该类型层级；实测移除后启动即抛 `FileNotFoundException` |
| `StartUI4Demo.exe.config` | 174 B | 建议保留 | 仅声明 `<supportedRuntime sku=".NETFramework,Version=v4.8"/>`；实测删除后仍可正常运行（.NET Framework 4.x 就地升级），保留它可在只装了更低版本的环境上给出明确报错 |
| `System.Data.SQLite.dll` | 398 KB | **不必需** | 库中唯一使用者 `UI4DataGrid` 为 `internal` 且无引用；程序集引用是惰性解析，实测删除后各分页均正常 |
| `StartUI4Demo.pdb` / `StartUI4Controls.pdb` | 92 KB | **不必需** | 仅调试符号（异常堆栈行号），发布时应排除 |

**最小可分发包** = `StartUI4Demo.exe` + `StartUI4Controls.dll` + `ICSharpCode.AvalonEdit.dll`（约 993 KB，比整目录小 33%）；
推荐再加 `StartUI4Demo.exe.config`。

补充：

- 若你的应用不使用 `UI4CodeEditor`，`ICSharpCode.AvalonEdit.dll` 也可不部署。
- 若将来把 `UI4DataGrid` 公开，除 `System.Data.SQLite.dll` 外还需随包部署原生 `x64/SQLite.Interop.dll`
  与 `x86/SQLite.Interop.dll`（当前类库构建并不会自动复制它们，见 `PORTING.md` 第 3 节第 3 条）。
- 精简发布建议用 `dotnet publish` 或只拷贝上表中的必需文件，而非直接打包整个 `bin` 目录。

这 7 个文件由构建流水线的不同环节各自产出（可用
`obj/Release/net48/StartUI4Demo.csproj.FileListAbsolute.txt` 核对完整复制清单）：

| 产物 | 生成环节 |
|---|---|
| `StartUI4Demo.exe` | csc 编译本项目；`App.xaml`/`MainWindow.xaml` 先被标记编译为 `.baml` 并嵌入 `.g.resources`，`app.manifest`（DPI 声明）与自动生成的 `AssemblyInfo.cs` 一并编入 |
| `StartUI4Demo.pdb` | 同上，`DebugType` 默认为 `portable`，随编译一并产出 |
| `StartUI4Demo.exe.config` | SDK 的 `GenerateSupportedRuntime` 目标按 `TargetFramework` 注入 `supportedRuntime sku`，先落为 `obj/.../StartUI4Demo.exe.withSupportedRuntime.config`，再由 `CopyAppConfig` 复制为 `exe.config` |
| `StartUI4Controls.dll` + `.pdb` | `ProjectReference` 的 CopyLocal（`Private` 默认 true），连同其符号一起复制到使用者输出目录 |
| `ICSharpCode.AvalonEdit.dll`、`System.Data.SQLite.dll` | 库的 `PackageReference` 经 NuGet 传递解析后进入 CopyLocal 闭包，按 TFM 就近取 `lib/net462`、`lib/net471` 资产复制 |

**为什么没有别的文件**：`*.deps.json` / `*.runtimeconfig.json` 是 .NET Core/5+ 的宿主探测机制，
.NET Framework 改用 Fusion/GAC + `app.config`；无本地化资源故无卫星程序集；原生 `SQLite.Interop.dll` 也未被复制（见上表说明）。
更正一处旧说法：`StartUI4Controls.xml`（IntelliSense 文档）**会**被复制进输出目录——库的 csproj 里
`GenerateDocumentationFile=true`；它与 `.pdb` 一样运行时不需要，详见 F 节。
其余中间产物（`.g.cs`、`.baml`、`.Up2Date`、各类 `.cache`）全部留在 `obj/`，不进入 `bin/`。

### E. 剪贴板通道：库内改用原生 Win32（`UI4Clipboard`）

WPF 的 `System.Windows.Clipboard` 走 OLE 通道（写入前 `OleFlushClipboard`，抢锁失败在调用线程重试）。
剪贴板是全局单锁资源，当截图 / 剪贴板历史类程序反复打开剪贴板时，可编辑控件的 Ctrl+C / Ctrl+X / Ctrl+V
会在 UI 线程上卡顿秒级，甚至抛 `CLIPBRD_E_CANT_OPEN`。库内现已统一改用 Win32 原生通道，使用者无需配合：

- 控件侧：`Internal/ClipboardCommandTakeover` 在**按键隧道（`PreviewKeyDown`）**阶段接管 Ctrl+C/X/V，
  `UI4TextBox` / `UI4PasswordBox`（明文模式）/ `UI4CodeEditor` / `UI4TextBlock` / `UI4DataGrid` 编辑单元
  构造或准备编辑时自动安装；右键菜单项与快捷键共用同一实现。
- 需要自行写剪贴板时（例如"一键复制"按钮）直接调用：

```csharp
UI4Clipboard.TrySetTextAsync(text, ok => { if (!ok) { /* 提示 */ } });  // 后台线程重试写入，回投调用线程
UI4Clipboard.TryGetTextAsync(text => { /* text 为 null 表示未读到 */ });  // 后台线程读取
if (UI4Clipboard.ContainsText()) { /* 不打开剪贴板，不参与抢锁 */ }
```

两条要点（踩过的坑，细节与实测数据见 [`../剪贴板卡顿问题报告.md`](../剪贴板卡顿问题报告.md)）：

- 只在 `CommandBinding.PreviewExecuted` 上接管**拦不住真实按键**（实测仍阻塞约 2 秒），必须在 `PreviewKeyDown`。
- Notepad 式"所有权 + 延迟渲染"（`SetClipboardData(CF_UNICODETEXT, NULL)` + `WM_RENDERFORMAT`）在装有剪贴板历史工具的
  机器上不可靠：实测取锁与获得所有权均成功，但延迟渲染声明固定失败且从不收到渲染消息；同路径 eager 写入则正常。
- 回归验证：`dotnet build tools/clipboard-lock-check/clipboard-lock-check.csproj -p:GeneratePackageOnBuild=false`
  后直接运行 exe，结果写入同目录 `results.txt`（10 条用例，含框架 `TextBox` 基线）。

### F. 输出目录里的 `.pdb` / `.xml` 是什么，运行时到底需要哪些文件（问答）

**问：exe 目录下还有一些 `.pdb` 和 `.xml` 文件，这些是什么？运行时不需要吗？**

以 Prompt 收藏夹（`src/PromptFavorites`）一次 Debug 构建的真实输出为例：

| 文件 | 大小 | 运行必需 | 是什么 |
|---|---|---|---|
| `PromptFavorites.exe` | 96 KB | **必需** | 入口程序集（WPF 的 `.baml` 已嵌进它的资源） |
| `StartUI4Controls.dll` | 429 KB | **必需** | 本控件库 |
| `ICSharpCode.AvalonEdit.dll` | 607 KB | **必需** | `UI4CodeEditor` 继承它的 `TextEditor`，XAML 解析期就要这个类型；实测删掉后启动即 `FileNotFoundException` |
| `PromptFavorites.exe.config` | 174 B | 建议保留 | 只有 `<supportedRuntime sku=".NETFramework,Version=v4.8"/>`；删了也能跑（4.x 就地升级），留着可在只装了更低版本的机器上给出明确报错 |
| `PromptFavorites.pdb` / `StartUI4Controls.pdb` | 36 KB / 106 KB | **不必需** | 调试符号 |
| `StartUI4Controls.xml` | 63 KB | **不必需** | IntelliSense 文档 |
| `System.Data.SQLite.dll` | 389 KB | **不必需** | 唯一使用者 `UI4DataGrid` 是 `internal` 且无人引用；程序集引用是惰性解析，实测删除后功能正常 |

**`.pdb` = 调试符号数据库（Program Database）**：存的是源文件名与行号、局部变量名、私有成员签名。
CLR 运行时**不加载**它，删掉程序照常运行。唯一影响是未处理异常的堆栈里能不能给出行号——
没有 pdb 时堆栈会退化成"在 PromptFavorites.ViewModels.MainViewModel.Copy 位置 行号 0"，
而排查用户报障（本项目排查"复制闪退"就是实例）往往就靠那几行行号。
工程里没写 `DebugType`，.NET SDK 对 net48 默认 `portable`，所以每次构建都会产出 pdb。

**`.xml` = XML 文档文件**：由 `StartUI4Controls.csproj` 的 `<GenerateDocumentationFile>true</GenerateDocumentationFile>`
产出，内容全是源码里的 `<summary>` / `<param>` / `<remarks>` 注释，只供 Visual Studio / Rider 的
IntelliSense 悬浮提示与对象浏览器使用，**运行时完全不读**，也不影响任何行为。

**发布时的三种取舍**：

```bash
# 1) 保留行号但不想多带文件：把符号嵌进 exe/dll 内部（不再有 .pdb 文件）
dotnet build src/PromptFavorites/PromptFavorites.csproj -p:GeneratePackageOnBuild=false -p:DebugType=embedded

# 2) 彻底不带符号：包最小，代价是崩溃堆栈没有行号
dotnet build src/PromptFavorites/PromptFavorites.csproj -p:GeneratePackageOnBuild=false -p:DebugType=none

# 3) 常规做法：发布包只带上面标"必需"的 3~4 个文件，pdb 单独归档一份，
#    用户报障时用它对照堆栈定位行号
```

补充两点：`*.deps.json` / `*.runtimeconfig.json` 是 .NET Core/5+ 的宿主探测机制，.NET Framework 不用；
没有本地化资源所以没有卫星目录；原生 `SQLite.Interop.dll` 也不会被自动复制（真要公开 `UI4DataGrid` 才需要，见 D 节）。

---

## 十、主题系统（全局 / 局部作用域 / 高对比度）

### 1. 全局主题与资源桥

```csharp
UI4Theme.SetTheme(UI4ThemeMode.Dark);     // Light / Dark / System / HighContrast
UI4Theme.SetTheme(UI4ThemeMode.System);   // 跟随系统（注册表 AppsUseLightTheme，实时响应切换）
UI4ThemeMode resolved = UI4Theme.ResolvedMode;  // System 时报告真正解析出的亮/暗
```

`SetTheme` 会把 30 个颜色令牌写进 `Application.Resources`，宿主 XAML 用 `{DynamicResource}` 即可跟随，无需逐元素手工同步：

```xml
<Window Background="{DynamicResource UI4.Brush.Background}">
    <TextBlock Foreground="{DynamicResource UI4.Brush.Text}"/>   <!-- 别名：Text / Border / Accent -->
    <Border BorderBrush="{DynamicResource UI4.Brush.BorderNormal}"/> <!-- 或全名 UI4.Brush.<令牌名> -->
</Window>
```

另有 `UI4.Color.<令牌>`（`Color` 值）与 `UI4Theme.SetAccent(Color)`（自动派生 AccentDark，全部控件跟随）。

### 2. 局部 / 每窗口主题（`UI4ThemeScope`）

在任意元素（卡片、`UserControl`、整个 `Window`）上声明主题键，其**整棵子树**改用该主题，与全局互不干扰：

```xml
xmlns:ui="clr-namespace:StartUI4Controls;assembly=StartUI4Controls"

<Border ui:UI4ThemeScope.Theme="dark"> <!-- 子树全部深色，即使全局是亮色 -->
    <StackPanel TextElement.Foreground="{DynamicResource UI4.Brush.Text}">
        <ui:UI4Button Content="深色按钮"/>
        <ui:UI4TextBox Text="深色输入框"/>
    </StackPanel>
</Border>
```

```csharp
UI4ThemeScope.SetTheme(myWindow, "highcontrast"); // 整窗另一套主题
UI4ThemeScope.SetTheme(card, "");                 // 撤销：子树回到全局主题
string key = UI4ThemeScope.GetTheme(card);        // 只读该元素自身声明的键
```

要点：

- 键大小写不敏感，取值 `light` / `dark` / `highcontrast` / 通过 `UI4Theme.Register(...)` 注册的自定义键；
  **空串与未注册键都表示撤销作用域**（XAML 写错键名不会导致崩溃）。
- 两条生效通道：① 向该元素 `Resources` 注入令牌字典，故 `{DynamicResource}` 宿主画刷与库内引用式控件
  （`UI4CheckBox`/`UI4Radio`/`UI4TextBox`/`UI4PasswordBox`/`UI4Switch`/`UI4ProgressBar`/`UI4Slider`/`UI4Pivot` 等）自动跟随；
  ② 仍走命令式刷新的控件（`UI4Button`/`UI4ComboBox`/`UI4Menu`…）在子树刷新时被同步换入该主题，因此**无需改任何控件代码**。
- 支持嵌套：子树内再声明一个键即为内层作用域，内外层各自正确；向上查找可穿过 Popup 与控件模板。
- 元素自身 `Resources` 里的同名直接键优先于作用域字典（标准 WPF 资源语义）。
- Demo 第 11 页「局部主题」提供左右对照卡片、作用域键下拉与嵌套示例；「打开异主题窗口」演示整窗作用域。

### 3. 高对比度主题

```csharp
UI4Theme.Apply("highcontrast");              // 直接切
UI4Theme.FollowSystemHighContrast = true;    // 可选：开启后 SetTheme(System) 在系统高对比度下优先用 highcontrast
```

定义为黑底 / 白字白框 / 黄强调，选中态用深蓝承托白色前景，覆盖全部 30 个令牌。
`FollowSystemHighContrast` 默认为 `false`（避免未经宿主同意就改变观感），开启后会订阅系统
`UserPreferenceChanged` 并在 Dispatcher 上编组刷新。

### 4. 自定义主题与持久化

```csharp
// 自定义主题：克隆内置定义（30 个令牌齐全），再改想改的令牌
var ocean = UI4ThemeDefinition.Dark().Clone();
ocean.With(UI4ThemeToken.Accent, Color.FromRgb(0, 150, 136));
ocean.With(UI4ThemeToken.Background, Color.FromRgb(0, 20, 26));
UI4Theme.Register(ocean);            // 键 = ocean.Key（Clone 生成，如 "dark.clone"）
UI4Theme.Apply(ocean.Key);           // 也可用作 UI4ThemeScope.Theme 的取值
foreach (string k in UI4Theme.ThemeKeys) { /* light / dark / highcontrast / 自定义 */ }

UI4Theme.Persistence = new RegistryThemePersistence();   // 或 JsonThemePersistence(path)；默认 null = 不持久化
UI4Theme.Save();
UI4Theme.ApplyPersisted();
```

### 5. 窗口标题栏跟随主题（`UI4WindowTitleBar`）

**需求**：切到深色/高对比度后，客户区已整片变暗，但窗口顶部那条系统标题栏仍是亮色白条——
标题栏属于**非客户区**，由 DWM 绘制，WPF 的属性、`DynamicResource`、控件模板全都够不着它。

**概念补充：DWM 是什么**。DWM = Desktop Window Manager（桌面窗口管理器），Windows Vista 起引入的桌面合成组件：
它把每个窗口的内容当作一张纹理取到 GPU 上合成后再输出，因此才有透明/毛玻璃、动画与贴边分屏。
关键在于**一个窗口由两部分组成**——

- **客户区（client area）**：程序自己画的区域，WPF 的内容、`Window.Background` 都落在这里；
- **非客户区（non-client area）**：标题栏、边框、圆角、投影，**由 DWM 画，不由程序画**。

普通窗口通过处理 `WM_NCPAINT` 自绘非客户区，而 DWM 合成后这条通路对标准窗口基本失效，程序只能改用
DWM 开放的窗口属性接口 `DwmSetWindowAttribute`（`dwmapi.dll`）来表达意图：

| 属性 | 编号 | 含义 | 可用性 |
|---|---|---|---|
| `DWMWA_USE_IMMERSIVE_DARK_MODE` | 20（20H1 前为 19） | 深/浅标题栏开关 | Win10 起 |
| `DWMWA_CAPTION_COLOR` | 35 | 标题栏底色 | Win11 起 |
| `DWMWA_TEXT_COLOR` | 36 | 标题文字色 | Win11 起 |
| `DWMWA_BORDER_COLOR` | 34 | 边框色 | Win11 起 |

值按 COLORREF `0x00BBGGRR` 传入，`0x01000000`（`DWMWA_COLOR_DEFAULT`）表示交还系统默认。
其中深/浅标志（19/20）**可被 `DwmGetWindowAttribute` 从外部进程回读**，是本特性唯一的硬证据；
配色三色（34/35/36）能写但**拒绝回读**（`0x80070057`）。这也解释了为什么只能拿到
「深/浅 + 底色/文字/边框」这四个维度，做不到像素级自定义——要突破就得自绘标题栏（下表方案 ②）。

**三条候选方案**：

| 方案 | 做法 | 代价 | 结论 |
|---|---|---|---|
| ① 宿主每窗口手写 P/Invoke | 宿主在各窗口 `SourceInitialized` 里自己调 `DwmSetWindowAttribute` | 每个窗口都要写代码；主题切换后不会自动重染；作用域/自定义主题得宿主自己算有效主题；库升级后宿主还得再改一遍 | 与「宿主零改动」的库定位相悖，放弃 |
| ② `WindowChrome` 自绘标题栏 | 把标题栏搬进客户区，自己画颜色、按钮 | 拖拽、双击、最大化还原、Aero Snap、贴边分屏、系统菜单、高 DPI、无障碍与键盘焦点全部要重写并保持与原生一致；`WindowStyle=None` 还会破坏辅助功能与第三方窗口管理 | 观感可控但行为风险远大于收益，放弃 |
| ③ 库内集中式 DWM 染色器 | 新增静态类 `UI4WindowTitleBar`，用 DWM 属性染色原生标题栏，并挂上自动通路 | 只能拿到 DWM 开放的颜色维度（深/浅标志 + 底色/文字/边框三色），做不到像素级自定义 | **采用**：零宿主改动、原生窗口行为一分不失、能读回证据 |

**用法**（默认全自动，宿主无需调用任何 API）：

```csharp
UI4Theme.SetTheme(UI4ThemeMode.Dark);   // 标题栏随之变深，无需其他代码

UI4WindowTitleBar.Apply(myWindow);      // 立即按该窗口的「有效主题」染色（窗口内没有任何 UI4 控件时用它兜底）
UI4WindowTitleBar.ApplyOpenWindows();   // 重染本进程全部已打开窗口
bool canTint = UI4WindowTitleBar.SupportsCaptionColors;   // 系统是否允许自定义标题栏配色
int  cref    = UI4WindowTitleBar.ToColorRef(color);       // Color -> DWM COLORREF(0x00BBGGRR)，宿主自调 dwmapi 时口径一致
```

```xml
<!-- 个别窗口豁免（例如截图/投屏窗口要保持系统原样） -->
<Window ui:UI4WindowTitleBar.Enabled="False" .../>
```

**染色内容**：先按底色亮度（0.299R+0.587G+0.114B < 128）判定深/浅，写 `DWMWA_USE_IMMERSIVE_DARK_MODE`；
再尝试把底色染成 `Background` 令牌、标题文字染成 `TextForeground`、边框染成 `BorderNormal`。
因此 `dark` 是深底浅字、`highcontrast` 是纯黑底白字白框，自定义主题同样按其令牌取值，无需枚举主题键。

**四条生效通路**（前三条自动，第四条兜底）：

- ① **清扫**：`UI4Theme.ThemeChanged` 触发后遍历 `Application.Windows` 重染全部已打开窗口——覆盖运行中的主题切换、`SetAccent`、跟随系统；
- ② **补染**：任一 UI4 控件 `Loaded` 时染它所属的窗口——覆盖「主题已是深色、窗口之后才打开」，同一 `ThemeVersion` 内每窗口只调一次 dwmapi；
- ③ **作用域**：`UI4ThemeScope` 的根若是整个 `Window`，其变更与撤销都直接按该作用域染色，异主题窗口的标题栏与内容一致；
- ④ **手动**：不含任何 UI4 控件的纯窗口自行调用一次 `Apply`（否则要等下一次主题切换被 ① 扫到）。

**兼容与探测**：不做版本号判断（未 manifest 声明的进程里 `Environment.OSVersion` 会虚报 6.3），
一律「先试 `DwmSetWindowAttribute`，失败即认定不支持」并缓存结论：深/浅标志先试属性 20，失败退旧编号 19；
配色属性（34/35/36）仅 Windows 11 起可用，在早期 Windows 10 上自动退化为「只有深/浅标题栏」。

> 代码改动清单（库内 4 处挂钩 + 1 个新文件）、`Apply` 的执行序列、四条通路的时序表、失败降级矩阵与断言↔实现对应关系，
> 见 `主题方案分析与改进.md` 第十二节；选型过程见同文档第十一节。

### 6. 已知限制

- 标题栏染色维度由系统给出：Windows 10 1903~2004 只认深/浅标志（标题栏变深但底色仍是系统深色，非主题 `Background`）；
  配色属性在更早系统与 Windows Server 上会静默失败，此时只保留深/浅标志。圆角、阴影与动画由 DWM 掌控，库不改。
- **不含任何 UI4 控件**的窗口没有加载钩子可挂，需自行调用一次 `UI4WindowTitleBar.Apply(this)`（否则要等到下一次主题切换才被清扫）。
- 命令式控件（`UI4Button`/`UI4ComboBox`/`UI4Menu`/`UI4ListBox`/`UI4NavigationView`/`UI4DataGrid` 等）在
  **作用域子树内**切换时仍会重建 `Style`；待 P2 把这些模板逐批改用令牌引用后，该开销归零（见 `PORTING.md` 第 11、12、13、14 节）。
- `UI4ComboBox`（含闭合选中框背景、焦点渐变）、`UI4ListBox`（面板背景、悬浮色）的背景**已跟随主题**，
  深色与高对比度下文字与底面对比度成立（`p3verify.ps1` H 组逐主题断言）。浅色主题下有一处**有意的观感变化**：
  `UI4ListBox` 项悬浮色由上游遗留的青色 `#0AF5FFFF` 改为主题令牌 `HoverOverlay`（浅色即 `#14000000` 半透黑）。
- 仍**未接入主题**的控件：`UI4Button` 恒为「蓝→紫渐变 + 白字」的强调按钮（三主题取值相同，对比度成立，但在高对比度黑底上
  不与黄/白体系呼应）；`UI4ListView`/`UI4GridView`/`UI4TabControl` 无 `IThemeAware`，卡片与文字恒为浅色（可读，观感不统一）。
  二者均归入 P2 批次 ②。
- `UI4ListBox` 编号样式的角标**数字颜色**在 `Dispatcher.BeginInvoke` 中重绘，作用域下该项可能取到全局色（同一处遗留，P2 批次 ② 消除）。
- 主题切换为瞬时生效，无交叉淡入动画。

---

## 许可证

MIT License —— 见 [`LICENSE.txt`](LICENSE.txt)。上游作者 KS.STUDIO，本移植版改动见 `PORTING.md`。
