# StartUI4.WPF 控件库文档（.NET 10 LTS 版）

> 一套现代风格（Modern Design）的 WPF UI 控件库，本仓库为 **.NET 10 (LTS) / net10.0-windows** 版本。

> **上游仓库**：<https://github.com/KSSTU/StartUI4.WPF>（net6.0-windows7.0）
> **本版本的来历**：由 `.NET Framework 4.8` 移植版（`E:\Qoder灵感项目\StartUI4.WPF_net48`）retarget 而来；
> net6→net48 的降级记录见 [`PORTING.md`](PORTING.md)，net48→net10 的改动清单、探针实测值与回归结果见
> [`PORTING-NET10.md`](PORTING-NET10.md)。
>
> **底线变化（必读）**：本版本要求 **.NET 10 桌面运行时**，因此**不再支持 Windows 7 / 8.1**；
> 客户端支持范围是 Windows 11 与 Windows 10（1607/1809/21H2 的 LTSC / 企业版）。
> 需要保 Win7/8.1 的场景请留在 net48 版，或另做多目标（见 PORTING-NET10.md §2 方案 C）。

> **找组件文档？** 本仓库根 README 只讲工程与仓库级的事（构建、打包、Demo、迁移、发布产物）。
> **每个组件的属性表、事件、示例，以及主题系统的完整说明，都在
> [`src/StartUI4Controls/README.md`](src/StartUI4Controls/README.md)（组件库手册）**——那份是组件库的真相源，
> 与源码逐条核对过；本文与它冲突时以它为准（差异登记见它的 [§8.4 校对记录](src/StartUI4Controls/README.md#84-校对记录与根-readme-的口径差异)）。

---

## 目录

- [一、简介](#一简介)
- [二、特性](#二特性)
- [三、快速开始](#三快速开始)
- [四、控件一览](#四控件一览)
- [五、控件参考（已迁至组件库手册）](#五控件参考)
- [六、完整使用教程：从零搭建一个应用](#六完整使用教程从零搭建一个应用)
- [七、Demo 工程指南](#七demo-工程指南)
- [八、与上游（net6 版）的差异与已知问题](#八与上游net6-版的差异与已知问题)
- [九、附录](#九附录)
- [十、主题系统（已迁至组件库手册 §四）](#十主题系统)
- [十一、.NET 10 迁移与回归](#十一net-10-迁移与回归)
- [许可证](#许可证)

---

## 一、简介

**StartUI4.WPF** 是一套对齐现代设计语言的 WPF 控件库。本版本在原 .NET Framework 4.8 移植版的基础上
retarget 到 **.NET 10 (LTS)**，可在 Visual Studio 2026（18.x+）或安装 .NET 10 SDK 的环境里编译与运行。

- **版本**：3.0.0（csproj 的 `Version` / `AssemblyVersion` / `FileVersion` 三处一致；主题机制重写与
  `UI4Theme.*Color` 实例属性删除都是 breaking change）
- **原作者**：KS.STUDIO
- **目标框架**：.NET 10（`net10.0-windows`，`UseWPF=true`）
- **语言级别**：`LangVersion=latest`，但源码保持与 net48 版逐行可比（**没有**回写现代语法），
  `Nullable` / `ImplicitUsings` 仍为 disable
- **NuGet 包名**：StartUI4.WPF
- **支持系统**：Windows 11（23H2+）/ Windows 10（1607、1809、21H2 的 LTSC 与企业版）；**不含 Win7 / 8.1**
- **运行时自检**：Demo 主窗口标题栏右侧实时显示 `RuntimeInformation.FrameworkDescription`，
  本机实测 `.NET 10.0.12`

---

## 二、特性

- **现代设计风格** —— 圆角、渐变、阴影、悬浮动效，对齐现代设计语言
- **渐变支持** —— 按钮、进度条、滑块、开关等均支持起止渐变色配置
- **丰富动画** —— 悬浮缩放、开关滑动、加载旋转、数字翻转等平滑动画
- **高度可定制** —— 234 个依赖属性（含 2 个附加属性）+ 1 个别名对外开放，几乎每个视觉细节都可调
- **开箱即用** —— 引用程序集或 NuGet 包后直接在 XAML 中使用，无需额外资源字典
- **主题系统** —— 亮/暗/跟随系统/高对比度一键切换，38 个颜色令牌经一份共享资源字典 + `DynamicResource` 桥接到宿主；`UI4ThemeScope` 可对单张卡片或整个窗口局部换肤；`UI4WindowTitleBar` 经 DWM 让**系统标题栏**同步跟随（详见[组件库手册 §四](src/StartUI4Controls/README.md#四主题系统)）
- **纯代码模板** —— 所有控件模板由代码构建，不依赖 Themes/generic.xaml，单 dll 即可分发
- **.NET 10 原生** —— 单文件类库 + `.deps.json`/`.runtimeconfig.json` 由 SDK 生成；不需要 `IsExternalInit` 之类的 polyfill

---

## 三、快速开始

### 1. 环境要求

| 项目 | 要求 |
|---|---|
| 操作系统 | Windows 11（23H2+）或 Windows 10（1607 / 1809 / 21H2 的 LTSC 与企业版）；**不支持 Win7 / 8.1** |
| 运行时 | .NET 10 桌面运行时（`Microsoft.WindowsDesktop.App` 10.0.x） |
| 编译工具 | .NET SDK 10.0.1xx 及以上（本机实测 10.0.401）；Visual Studio 2026 18.x+ |
| 依赖包 | 仅 AvalonEdit 6.3.1.120（`UI4CodeEditor` 需要）。`System.Drawing.Common`、`Microsoft.Win32.SystemEvents`、`Microsoft.Win32.Registry` 均由 `Microsoft.WindowsDesktop.App` 框架引用提供，**不必再显式引用**；net48 版的 `System.Data.SQLite 2.0.3` 已随 `UI4DataGrid` 死代码一并移除（详见 PORTING-NET10.md §4） |

### 2. 方式一：源码构建

```bash
git clone <本仓库>
dotnet build StartUI4Controls.sln
```

产物：

- `src/StartUI4Controls/bin/Debug/net10.0-windows/StartUI4Controls.dll`（+ 同名 `.xml` 文档、`.pdb` 符号）
- 打包**不再随构建自动进行**（`GeneratePackageOnBuild=false`）：需要包时显式执行
  `dotnet pack src/StartUI4Controls/StartUI4Controls.csproj -c Release`，产出 `StartUI4.WPF.3.0.0.nupkg`。
  net48 版把 `GeneratePackageOnBuild` 设成了 true，`dotnet build` 会顺带跑 pack，一旦许可证文件解析失败
  就报 NU5019 把整条构建链拖红——这是本次迁移顺手修掉的一个工程坑。

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
实际运行时： .NET 10.0.12
```

若误跑在 .NET Core / .NET 5+ 上，此处前缀会变为 `.NET Core` / `.NET`，可立即识别。

---

## 四、控件一览

> 速览表，只用于首页导航。**属性 / 事件 / 用法请看
> [`src/StartUI4Controls/README.md`](src/StartUI4Controls/README.md)**——按分类列在它的 §二（清单）与 §三（详解 3.1~3.11）。

| 控件 | 基类 | 说明 |
|---|---|---|
| `UI4Button` | `Button` | 渐变 / 圆角 / 悬浮色按钮；禁用态自动换灰色模板，前景在 `OnAccent` 与正文色之间取对比度更高者（见[组件库手册 §3.1](src/StartUI4Controls/README.md#31-基础交互)） |
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
| `UI4Clipboard` | —（静态服务） | 原生 Win32 剪贴板读写，库内文本控件的复制/剪切/粘贴均走此通道（见第九节附录 E 与[组件库手册 §5.1](src/StartUI4Controls/README.md#51-ui4clipboard)） |
| `UI4Grid` | `Grid` | 默认渐变背景的 Grid |
| `UI4MultiLanguage` | —（静态服务） | 静态文案：zh / en / ja / ko / de / fr / es / ru 八套，切语言靠 `CultureInfo.CurrentUICulture` + `Refresh()` |
| `UI4Theme` | —（静态服务） | 全局主题：亮/暗/跟随系统/高对比度、令牌资源桥、`SetAccent`、自定义主题注册、持久化 |
| `UI4ThemeScope` | —（附加属性） | 局部/每窗口主题：`ui:UI4ThemeScope.Theme="dark"`，子树独立换肤（见[组件库手册 §4.6](src/StartUI4Controls/README.md#46-局部作用域-ui4themescope)） |
| `UI4WindowTitleBar` | —（附加属性 + 静态方法） | 系统标题栏跟随主题：DWM 深/浅 + 标题栏底色/文字/边框染色，默认全自动，`ui:UI4WindowTitleBar.Enabled="False"` 可豁免（见[组件库手册 §4.10](src/StartUI4Controls/README.md#410-窗口标题栏跟随主题)） |
| `UI4ThemeMode` / `UI4ThemeToken` | —（枚举） | 主题模式（Light/Dark/System/HighContrast）与 38 个颜色令牌键 |
| `UI4ThemeDefinition` | —（sealed 类） | 一套主题的令牌取值：内置 `Light()` / `Dark()` / `HighContrast()`，可 `Clone()` + `With(token, color)` 定制后 `UI4Theme.Register` |
| `RegistryThemePersistence` / `JsonThemePersistence` | `IThemePersistence` 实现 | 主题模式持久化后端：HKCU 注册表 / JSON 文件（宿主自选，见[组件库手册 §4.9](src/StartUI4Controls/README.md#49-主题持久化)） |
| `UI4MenuItem` / `UI4MenuItemType` / `UI4MenuIcons` | —（配套类型） | 右键菜单条目数据、七种标准条目类型（Undo/Redo/Cut/Copy/Paste/Delete/SelectAll）与内置图标 |
| `UI4TrayMenuItem` / `PopupActivationMode` | —（配套类型） | 托盘菜单条目与"哪种鼠标键弹菜单"的枚举 |
| `UI4LanguageKey` | —（枚举） | 静态文案键（OK/Cancel/Notice/ColorPicker/Undo/…/SelectAll） |
| `TabCloseRoutedEventArgs` | `RoutedEventArgs` | `UI4Tab.CloseTab` 事件参数（携带被关的 `UI4TabItem`） |

> `UI4DataGrid`、`UI43DSphere` 在上游即为 `internal` 且无引用；**net10 版已把这两个死代码文件连同 `System.Data.SQLite` 依赖一起删除**（见 `PORTING-NET10.md` §4）。

---

## 五、控件参考

控件的属性表、事件、行为契约与逐个控件的 XAML/C# 示例**已迁出**，见
[`src/StartUI4Controls/README.md`](src/StartUI4Controls/README.md)（组件库手册）：

| 手册小节 | 内容 |
|---|---|
| §一 / §二 | 程序集事实（公开 API 统计、文件地图、三分钟接入）与 30 个控件的分类总表（含每个控件的公开属性数） |
| §三 | 组件详解 3.1 基础交互 … 3.11 系统集成：逐控件的依赖属性表（类型 / 字面默认值 / 跟随令牌 / 说明）+ 事件 + 方法 + 示例 + 行为契约 |
| §四 | 主题系统全部内容（本文第十节只留索引） |
| §五 | 静态服务：`UI4Clipboard`、`UI4MultiLanguage` |
| §六 | 用法配方：App 启动序列、主窗口骨架、数据绑定列表、对话框与取色、托盘与退出清理、局部换肤 |
| §七 / §八 | 扩展约定（新增控件 / 新增令牌的清单）、枚举与事件全清单、校对记录 |

**为什么迁走**：同一套 API 事实在两份文档里各写一遍必然漂移。本次迁出时按源码逐条核对，
已发现并修正 20 处口径差异（例如「250+ 依赖属性」实测为 234、`UI4NotifyIcon` 三个已删除的菜单配色 DP、
`UI4Button` 前景与禁用态的现行规则），每条依据见
[组件库手册 §8.4 校对记录](src/StartUI4Controls/README.md#84-校对记录与根-readme-的口径差异)。

第四节保留一张速览表，仅用于仓库首页导航。

---

## 六、完整使用教程：从零搭建一个应用

以下以 Visual Studio 2026 + .NET 10 SDK 为例，从零搭一个使用本库的应用。

### 步骤 1：新建项目

新建「WPF 应用」项目（.NET 10），或在 SDK 风格 csproj 中写：

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net10.0-windows</TargetFramework>
    <UseWPF>true</UseWPF>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\StartUI4Controls\StartUI4Controls.csproj" />
    <!-- 或 <PackageReference Include="StartUI4.WPF" Version="3.0.0" /> -->
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
确认输出为 `.NET 10.x`。

---

## 七、Demo 工程指南

仓库自带完整演示工程 `samples/StartUI4Demo`（net10.0-windows），覆盖全部公开控件，共 13 页（`--tab=0..12`）。

### 构建与运行

```bash
# 本仓库有 StartUI4Controls.sln，含 4 个工程：库 / Demo / clipboard-lock-check / Net10Regression
dotnet build StartUI4Controls.sln
samples\StartUI4Demo\bin\Debug\net10.0-windows\StartUI4Demo.exe            # 直接运行
samples\StartUI4Demo\bin\Debug\net10.0-windows\StartUI4Demo.exe --tab=11   # 直接打开指定分页（0 起）
```

net48 时代要求带 `-p:GeneratePackageOnBuild=false`（否则 pack 阶段 NU5019 拖垮构建），
本仓库已把 `GeneratePackageOnBuild` 直接设为 false，**不再需要这个参数**；需要包时显式 `dotnet pack`。

回归（自动化）：

```bash
dotnet build StartUI4Controls.sln
tools\Net10Regression\bin\Debug\net10.0-windows\Net10Regression.exe --all
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

> 本节第 1–4 条记录 **net6 → net48** 那段历史（`PORTING.md`）；本仓库（net10）在其之上又走了一步，见第 5 条与 `PORTING-NET10.md`。

完整清单见 [`PORTING.md`](PORTING.md)，摘要：

1. **目标框架**：`net6.0-windows7.0` → `net48`；语言级别锁定 C# 7.3。
2. **依赖**：移除源码中从未使用的 `Microsoft.Data.Sqlite`；保留 `AvalonEdit`、`System.Data.SQLite`（后者已随 net10 版的死代码清理一并移除，见第 5 条）。
3. **本移植版修复的上游缺陷**：
   - `UI4Switch` 被拉伸时轨道与滑块分离；
   - `UI4ListView` / `UI4GridView` 悬浮缩放导致文字模糊（Effect 与 ScaleTransform 同层）；
   - `UI4ComboBox` 选中项长文本溢出、下拉弹出层裁切长选项；
   - 重定义依赖属性缺少 `new` 关键字产生的 CS0108 警告。
4. **上游遗留、未改动**：
   - `UI4TextBlock.GradientStart/End` 上游是未实现的死属性——**net10 版已连同声明一起删除**（渐变请用 `Foreground`）；
   - 5 个只写不读的私有字段（CS0414 警告保留）；
   - `UI4DataGrid` / `UI43DSphere` 为 internal 死代码，且 SQLite 原生 `SQLite.Interop.dll` 不随类库部署。
5. **net48 → net10 的差异**（详见 `PORTING-NET10.md`）：
   - TFM 改 `net10.0-windows`，**OS 底线升到 Win10（LTSC/企业版）+ Win11**，不再支持 Win7/8.1；
   - 删除 `UI4DataGrid` / `UI43DSphere` 两个 internal 死代码与 `System.Data.SQLite` 依赖；
     `System.Drawing` 的显式框架引用也不再需要（`System.Drawing.Common`、`Microsoft.Win32.SystemEvents`
     都由 `Microsoft.WindowsDesktop.App` 框架引用提供，探针 P01/P12 实测）；
   - `UI4Menu` 样式初始化失败不再弹模态框，改为写 `ui4menu-style-error.log` 并抛出；
   - 产物形态：`StartUI4Demo.exe` 变成原生 apphost + 同名托管 `dll`，`exe.config` 让位于
     `runtimeconfig.json` / `deps.json`；TFM 特性串也从 `.NETFramework,Version=v4.8` 变成 `.NETCoreApp,Version=v10.0`；
   - **UIA 暴露差异（新发现，属无障碍待办）**：`UI4Pivot` / `UI4Tab` / `UI4NavigationView` 的内容区文本，
     在 net10 下不再进入 UIA 树（同源码重编的 net48 原始视图里 6 处命中，net10 为 0）。
     渲染本身没坏——harness 的进程内断言 I13~I16 按 Demo 的嵌套方式（Show 前选页 + ScrollViewer）直接翻视觉树，
     三个容器的选中内容都在；差异只在自动化与辅助技术可读性。
     另有一条**两个运行时共有**的缺口：`UI4ProgressBar` / `UI4CircleSlider` 等控件本体本来就不进 UIA。
     要让屏幕阅读器可用，需要给这些容器补 `AutomationPeer`（`GetChildrenCore` / `ContentElement`），
     进度类控件再补 `RangeValue` 型 peer。判别过程见 `PORTING-NET10.md` §7.3。

6. **本仓库内新发现并修复（2026-10）：悬浮放大越出父容器 + 网格不随宽度调整卡片**

   - **成因四条，缺一不成立**：① `ItemWidth` 为 `NaN` 时不给容器设 `Width`，靠 `HorizontalContentAlignment=Stretch` 铺满整行；
     ② 缩放是 `RenderTransform`，纯视觉，必然画到布局槽外面；③ 全库唯一的 `ClipToBounds=true` 在被缩放节点的**子级**上，裁不到父节点的放大输出；
     ④ Demo「列表与网格」页是 `ScrollViewer + StackPanel(MinWidth=760)`，横向默认可滚 → 子级拿到无限宽 → 整页被撑到 2398 px，静止态就已出窗口右缘。
   - **修法**：生效倍率按像素预算反算取小（完整契约见[组件库手册 §3.7](src/StartUI4Controls/README.md#37-列表与卡片)），并把缩放节点从模板子节点提到容器本体（每容器一份 `ScaleTransform`，
     顺带规避 `FrameworkElementFactory.SetValue` 的对象被所有容器共享）。**不采用 `ClipToBounds` 兜底**：
     `ShadowDepth=15 + BlurRadius=12` 的投影需要约 27 px 外溢空间，裁剪会把四边投影切平。
   - **实测**（UIA 量 `ListItem.BoundingRectangle`；负数＝在控件边界内）：

     | ListView 宽 | 行宽 | 生效 ScaleX | 距控件左/右内边 |
     |---|---|---|---|
     | 1094 | 1066 | 1.0100（未钳） | 9 / 9 px |
     | 642 | 614 | 1.0100（未钳） | 11 / 11 px |
     | 1742 | 1714 | 1.0093（已钳） | 6 / 6 px |
     | 2398（全屏） | 2370 | 1.0068（已钳） | 6 / 6 px |

     对照实验：临时把 `HoverScale` 设成 `1.5`，生效值被钳到 1.0130（= 1 + 2×7/1076，与公式一致），
     且只有被悬浮的那一项变化、其余三项与静止态逐像素相同。
     `UI4GridView` 卡片随列数铺满所在列（642 px→2 列各 297、1094→4 列各 252、1742→4 列各 414、2398→4 列各 578）；
     运行期把 `ItemWidth` 从 230 改到 400，列数在**不缩窗口**的情况下当场从 4 变 3，`ComputedColumns` 与实测排布一致。
     探针脚本是仓库外的临时件，未入库；复现方法是"UIA 取 `ListItem` 矩形 + `SetCursorPos` 触发悬浮 + 等动画跑完再量"。
   - **同批移除（破坏性 API 变更）**：`UI4ListView` / `UI4GridView` 的 `ItemHoverBorderBrush` 属性与"悬浮把边框换成蓝色"的动画一起去掉，
     悬浮反馈只剩放大 + 投影，边框始终保持 `ItemBorderBrush`。宿主若设过该属性会编译失败；要恢复就重新加回指向
     `PART_ItemBorder` 的 `ColorAnimation` + `EventTrigger(MouseEnter/MouseLeave)`。`UI4Panel` 的悬浮观感本次未动。
   - **仍未处理**：靠边界那侧的投影被视口裁（静止态就存在，与悬浮无关）；`UI4Panel` 里第三份复制的 hover 代码本次未动；
     项数少时卡片会被撑得很宽（见 `UI4GridView` 一节的代价说明）。

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

### B. 解决方案配置与目标框架的关系（net48 历史口径）

> 本节描述 net48 基线。net10 下：`.sln` 含 4 个工程（库/Demo/两个工具），
> 运行期框架改由 `runtimeconfig.json` 声明 `Microsoft.WindowsDesktop.App 10.0.x`，不再有 `exe.config`。

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

> 下表是 **net48 基线**的产物口径，保留作历史对照。net10 的产物形态不同：
> `StartUI4Demo.exe` 只有原生 apphost（约 150 KB），托管主体在同名 `StartUI4Demo.dll`；
> 不再有 `StartUI4Demo.exe.config`，改为 `StartUI4Demo.runtimeconfig.json` + `.deps.json`（这两个是 .NET 宿主定位框架与依赖的必需文件）；
> `System.Data.SQLite.dll` 与其原生 `SQLite.Interop.dll` 随死代码清理彻底消失。

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
  `UI4TextBox` / `UI4PasswordBox`（明文模式）/ `UI4CodeEditor` / `UI4TextBlock`
  构造或准备编辑时自动安装；右键菜单项与快捷键共用同一实现。（net48 版这里还列有 `UI4DataGrid` 编辑单元，该控件已随死代码清理删除。）
- 需要自行写剪贴板时（例如"一键复制"按钮）直接调用：

```csharp
UI4Clipboard.TrySetTextAsync(text, ok => { if (!ok) { /* 提示 */ } });  // 后台线程重试写入，回投调用线程
UI4Clipboard.TryGetTextAsync(text => { /* text 为 null 表示未读到 */ });  // 后台线程读取
if (UI4Clipboard.ContainsText()) { /* 不打开剪贴板，不参与抢锁 */ }
```

两条要点（踩过的坑，细节与实测数据见
[`常见问题清单/剪贴板卡顿问题报告.md`](常见问题清单/剪贴板卡顿问题报告.md)）：

- 只在 `CommandBinding.PreviewExecuted` 上接管**拦不住真实按键**（实测仍阻塞约 2 秒），必须在 `PreviewKeyDown`。
- Notepad 式"所有权 + 延迟渲染"（`SetClipboardData(CF_UNICODETEXT, NULL)` + `WM_RENDERFORMAT`）在装有剪贴板历史工具的
  机器上不可靠：实测取锁与获得所有权均成功，但延迟渲染声明固定失败且从不收到渲染消息；同路径 eager 写入则正常。
- 回归验证：`dotnet build tools/clipboard-lock-check/clipboard-lock-check.csproj`（net10 版已把 `GeneratePackageOnBuild`
  设为 false，不再需要 `-p:GeneratePackageOnBuild=false`；该工具也已并入 `StartUI4Controls.sln`）
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

## 十、主题系统

主题机制、令牌取值与标题栏染色的完整说明**已迁出**到
[`src/StartUI4Controls/README.md` §四](src/StartUI4Controls/README.md#四主题系统)：

| 小节 | 内容 |
|---|---|
| 4.1 | 38 个颜色令牌与数据模型（`UI4ThemeToken` / `UI4ThemeMode` / `UI4ThemeDefinition`） |
| 4.2 | 生效通路：每个主题键一份共享资源字典 + `SetResourceReference` / `DynamicResource`（实测 25 个文件 / 95 处调用）；「本地赋值即退订主题」的语义；换字典不留空窗 |
| 4.3 | `UI4Theme` API 一览 |
| 4.4 | `light` / `dark` / `highcontrast` 三套的 38 令牌完整取值 |
| 4.5 | `UI4ThemePacks` 8 套预置业务主题（键是稳定契约，逐套完整取值表） |
| 4.6 | `UI4ThemeScope` 局部 / 每窗口作用域（嵌套、撤销语义、与标题栏的联动） |
| 4.7 | `SetAccent`、自定义主题注册与 `Register` 的三条分支 |
| 4.8 | 跟随系统（注册表 `AppsUseLightTheme` + `SystemEvents`）与高对比度 |
| 4.9 | 持久化：`RegistryThemePersistence`（HKCU）/ `JsonThemePersistence` |
| 4.10 | `UI4WindowTitleBar`：DWM 属性表、判深浅公式、四条生效通路、能力探测与降级路径 |
| 4.11 | 主题盲区与已知限制：**8 个令牌在库内无消费者**、**20 个颜色类依赖属性未挂令牌**（含宿主自救写法）、性能与观感限制 |

速查：

```csharp
UI4Theme.ApplyToApplication();             // 令牌进 Application.Resources，宿主写 {DynamicResource UI4.Brush.X}
UI4Theme.SetTheme(UI4ThemeMode.System);    // Light / Dark / System / HighContrast
UI4ThemePacks.RegisterAll();               // 注册 8 套业务主题（亮 5 + 暗 3）
UI4Theme.Apply(UI4ThemePacks.DataConsole); // 按英文键应用；也可写进 ui:UI4ThemeScope.Theme="data-console"
UI4Theme.SetAccent(Color.FromRgb(0xE6, 0x78, 0x14));   // 只换强调色，AccentDark 自动派生
```

> 本节原有的两处外部引用已失效并就地更正：
> `主题方案分析与改进.md`（第十一、十二节）随文档整理从仓库删除，其标题栏机制与通路结论由组件库手册 §4.10 承载；
> 指向 `src/StartUI4Controls/README.md` 「§九 / 9.2 / 9.3 / 9.7 / 9.9~9.11 / A1-4」的内容，
> 那份 3.0.0 主题机制审计报告已另存为
> [`src/StartUI4Controls/架构审计报告-3.0.0主题机制评审.md`](src/StartUI4Controls/架构审计报告-3.0.0主题机制评审.md)，
> 实测数字（令牌覆盖率 17 → 89、单按钮一次 `SetTheme` 重建 6~8 份 Style、300 按钮 78 ms、`SetAccent` 277 ms、
> 黄底黑字 19.56:1 等）仍在该文档内；其中 §5、§1.2、§1.3、§8 围绕 2.0.0 的 `IThemeAware` 机制展开，仅作历史记录。

---

## 十一、.NET 10 迁移与回归

本节只讲「从 net48 迁到 net10 之后有什么不一样、怎么验」，细节在 [`PORTING-NET10.md`](PORTING-NET10.md)。

### 1. 工程侧

| 项 | net48 版 | 本版 |
|---|---|---|
| TFM | `net48` | `net10.0-windows` |
| 语言 | C# 7.3 锁定 | `latest`（源码未回写新语法，同一份代码仍能以 net48/C#7.3 编过，已在 `_ab` 试验证） |
| 依赖 | AvalonEdit + System.Data.SQLite + 显式 `System.Drawing` 引用 | **仅 AvalonEdit**；Drawing/SystemEvents/Registry 由 WindowsDesktop 框架引用提供 |
| 打包 | `GeneratePackageOnBuild=true`（`dotnet build` 会顺带 pack，NU5019 会拖红整条链） | `false`，显式 `dotnet pack` |
| 产物 | 托管 `exe` + `exe.config`（`supportedRuntime sku=.NETFramework,Version=v4.8`） | 原生 apphost `exe` + 托管 `dll` + `runtimeconfig.json`/`deps.json`，TFM 特性串 `.NETCoreApp,Version=v10.0` |
| OS | Win7/8.1/10/11 | Win10（LTSC/企业版）+ Win11 |

### 2. 一键回归

```bash
dotnet build StartUI4Controls.sln
tools\Net10Regression\bin\Debug\net10.0-windows\Net10Regression.exe --all
```

覆盖：13 页 UIA 逐页走查（真实按键 Ctrl+C/X/V + 原生读回剪贴板、模态框、右键菜单、托盘开关、下拉弹出层、
DWM 标题栏跨进程回读、`demo-theme.json` 往返）、系统级（TFM 三层实证、注册表零污染、异常日志、
`clipboard-lock-check` 持锁矩阵）、进程内 STA（令牌桥、三主题、按钮对比度、局部作用域、SetAccent/Register/持久化、
剪贴板通道、8 套语言、导航容器内容）。`--help` 见 `tools/Net10Regression/USAGE.md`。

本轮结果：**326 PASS / 0 FAIL，退出码 0**
（net10 逐页 145/0、同源码重编的 net48 A/B 逐页 145/0、系统级 10/0、进程内 26/0）。
两侧跑同一套用例、同一份 Demo 源码，逐条同结果；`T0-runtime` 一条在两侧分别报
`.NET 10.0.12` 与 `.NET Framework 4.8.9345.0`，证明对照确实换了运行时。
剪贴板持锁矩阵 10/10：接管后 UI 阻塞 11~30ms，未接管的框架通道在同一把锁下阻塞 1048ms。
唯一真实的 net10 独有差异是 UIA 暴露（不在用例失败里体现，用 `--dump=6` 两侧对比可见），见 §8.5 与 `PORTING-NET10.md` §7.3。

### 3. 已知限制

- `UI4Pivot` / `UI4Tab` / `UI4NavigationView` 的内容区文本在 net10 不进 UIA 树（同源码的 net48 会进），
  屏幕阅读器与 UIA 自动化因此拿不到页面主体；渲染不受影响（进程内 I13~I16 已证）。需要库侧补 `AutomationPeer`。
- 需要 Windows 7 / 8.1 的场景请继续用 net48 版仓库，或按 `PORTING-NET10.md` §2 的方案 C 改多目标。
- 附录 B / D / E / F 中的产物清单与宿主机制描述仍以 net48 基线为口径（历史留档），
  net10 的对应内容看本节与 `PORTING-NET10.md`。

---

## 许可证

MIT License —— 见 [`src/StartUI4Controls/LICENSE.txt`](src/StartUI4Controls/LICENSE.txt)
（仓库根没有许可证文件，许可证只随库工程与 nupkg 分发）。上游作者 KS.STUDIO，本移植版改动见 `PORTING.md`。
