# UI4MessageBox 弹出定位修复

## 问题根因

`UI4MessageBox.cs` 第 89 行设置 `WindowStartupLocation = CenterOwner`，但第 267-275 行的 `Show()` 方法从未设置 `box.Owner`。当 `CenterOwner` + `Owner == null` 时，WPF 回退到 Windows 的 `CW_USEDEFAULT`，触发对话框级联偏移，导致每次弹出位置不一致（向右下偏移）。

## 修复方案

### 步骤 1：修改 `Show()` 方法签名与实现

**文件**：`src/StartUI4Controls/UI4MessageBox.cs`（第 259-275 行）

```csharp
/// <summary>
/// 显示消息框并返回用户选择结果。
/// </summary>
/// <param name="content">消息内容文本。</param>
/// <param name="title">窗口标题（为 null 时使用默认"注意"）。</param>
/// <param name="buttons">按钮模式。</param>
/// <param name="width">窗口宽度。</param>
/// <param name="owner">父窗口（消息框居中于其上方；为 null 时居中于屏幕）。</param>
/// <returns>DialogResult：OK 为 true，Cancel 为 false，关闭为 null。</returns>
public static bool? Show(string content,
    string title = null,
    UI4MessageBoxButtons buttons = UI4MessageBoxButtons.OK,
    double width = DefaultWidth,
    Window owner = null)
{
    var box = new UI4MessageBox(title ?? UI4MultiLanguage.Get(UI4LanguageKey.Notice), content, buttons);
    box.Width = width;
    if (owner != null)
    {
        box.Owner = owner;
        // CenterOwner 已在 ConfigureWindowProperties 中设置
    }
    else
    {
        box.WindowStartupLocation = WindowStartupLocation.CenterScreen;
    }
    return box.ShowDialog();
}
```

**关键设计决策**：

| 决策 | 选择 | 理由 |
|------|------|------|
| 参数位置 | 最后一个（`width` 之后） | 现有调用方使用位置参数，不影响已有调用 |
| 默认值 | `null` | 不强制要求调用方提供 Owner |
| owner 为 null 时 | 显式切换到 `CenterScreen` | 消除 `CenterOwner` + null Owner 的 WPF 不确定行为 |
| owner 非 null 时 | 保持 `CenterOwner` | 构造函数已设置，无需额外操作 |

### 步骤 2（可选）：更新 Demo 调用方展示新参数用法

**文件**：`samples/StartUI4Demo/MainWindow.xaml.cs`（第 175、181 行）

现有调用无需修改即可正常工作。可选择性更新以展示 `owner` 用法。

---

## 多角度修复完整性分析

### 1. 定位正确性

| 场景 | 修复前 | 修复后 |
|------|--------|--------|
| 不传 owner | CW_USEDEFAULT → 位置不确定 | `CenterScreen` → 始终居中于屏幕 |
| 传入 owner | 同上（owner 被忽略） | `CenterOwner` → 居中于 owner 窗口 |
| 连续弹出两次 | 第二次向右下偏移 | 每次位置完全一致 |

### 2. API 兼容性

| 维度 | 影响 |
|------|------|
| 源码兼容 | ✅ 完全兼容 — 可选参数默认 null，现有调用无需修改 |
| 二进制兼容 | ⚠️ 需重编译 — 可选参数改变 IL 元数据（.NET 标准行为） |
| 命名参数兼容 | ✅ `width: xxx` 调用不受影响 |

### 3. 与项目既有模式的一致性

`UI4ColorPicker.ShowDialog()` 已采用完全相同的模式（第 770-776 行）：
```csharp
public static Color? ShowDialog(string title = null, Color? defaultColor = null, Window owner = null)
{
    var picker = new UI4ColorPicker(title, defaultColor);
    if (owner != null) picker.Owner = owner;
    ...
}
```
本次修复与项目既有 API 风格一致。

### 4. 性能影响

**零感知**。`box.Owner = owner` 是 O(1) 引用赋值。`CenterOwner` 居中计算仅读取 Owner 的 Left/Top/Width/Height 四个属性做简单算术，耗时纳秒级。

### 5. 边界场景

| 场景 | 行为 | 风险 |
|------|------|------|
| `owner == null` | 回退到 `CenterScreen`，确定性居中 | 无 |
| `owner` 已关闭 | WPF 抛出 `InvalidOperationException` | 调用方 bug，非本组件责任 |
| `owner` 最小化 | 居中于 Owner 的还原位置 | 低 — WPF 标准行为 |
| `owner` 跨线程 | WPF 抛出 `InvalidOperationException` | 调用方 bug |

### 6. 与现有功能的交互

| 功能 | 是否受影响 | 说明 |
|------|-----------|------|
| 打开/关闭动画 | 否 | 动画操作 RenderTransform/Opacity，与窗口位置无关 |
| Resize 行为 | 否 | WindowResizeBehavior 操作 Width/Height，不涉及位置 |
| 主题切换 | 否 | IThemeAware 更新颜色，不涉及位置 |
| 拖拽移动 | 否 | DragMove() 与初始位置无关 |

---

## 被否决的方案

| 方案 | 否决原因 |
|------|---------|
| 仅添加 owner 参数但不处理 null 情况 | `CenterOwner` + null Owner 仍是不确定行为，修复不完整 |
| 改用 `CenterScreen` 作为唯一策略 | 丢失了居中于父窗口的能力，多屏场景不佳 |
| 手动计算居中位置 | 重复 WPF 内置逻辑，增加维护负担 |

---

## 关键文件

1. `src/StartUI4Controls/UI4MessageBox.cs` — 修改 `Show()` 方法（第 259-275 行）
2. `src/StartUI4Controls/UI4ColorPicker.cs` — 参考实现（第 770 行）
3. `samples/StartUI4Demo/MainWindow.xaml.cs` — 调用方（第 175、181 行，可选更新）
