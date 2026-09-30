# T4 — API 命名和类型不一致修复

## 问题诊断

经全面审计全部 30 个控件的 DependencyProperty，发现 3 个剩余问题：

### 问题 1（严重）：UI4Panel 用 `new` 隐藏基类属性
- `BorderBrush`：类型从 `Brush` 改为 `Color`，创建了独立的 DP 标识符，破坏多态性
- `BorderThickness`：虽然类型相同（`Thickness`），但 `new` 重新注册导致与基类 DP 完全独立
- **影响**：外部通过样式/绑定设置 `BorderBrush` 时命中基类 DP 而非自定义 DP

### 问题 2（中等）：UI4CheckBox 命名不一致
- `BoxCornerRadius` vs 其他所有控件的 `CornerRadius`

### 问题 3（无需修改）：Color vs Brush 类型混用
- 属性名含 `Color`（如 `TextColor`、`BorderNormalColor`）→ 类型为 `Color` ✓
- 属性名含 `Brush/Background/Foreground`（如 `HoverBackground`、`EditBackground`）→ 类型为 `Brush` ✓
- **唯一例外**：`UI4Panel.BorderBrush`（类型为 `Color`）→ 修复问题 1 后消除
- **结论**：修复 UI4Panel 后，类型与命名约定自然一致，无需额外修改

---

## 步骤 1：修复 UI4Panel.BorderBrush `new` 隐藏

**文件**：`src/StartUI4Controls/UI4Panel.cs`

1. **删除** 第 38-49 行的 `new` `BorderBrushProperty` 和 CLR 包装器
2. **新增** `BorderColorProperty`（`Color` 类型），默认值 `Color.FromArgb(60, 120, 140, 200)`
3. **新增** `BorderColor` CLR 属性
4. **修改** 第 220 行：`new SolidColorBrush(BorderBrush)` → `new SolidColorBrush(BorderColor)`
5. **修改** 第 245 行：`To = BorderBrush` → `To = BorderColor`

## 步骤 2：修复 UI4Panel.BorderThickness `new` 隐藏

**文件**：`src/StartUI4Controls/UI4Panel.cs`

1. **删除** 第 64-75 行的 `new` `BorderThicknessProperty` 和 CLR 包装器
2. **新增** `BorderThicknessProperty.OverrideMetadata(typeof(UI4Panel), new FrameworkPropertyMetadata(new Thickness(1), OnStyleUpdate))` 在 `static UI4Panel()` 中
3. 第 216-217 行的 `Binding(nameof(BorderThickness))` 无需修改（字符串匹配仍有效）

## 步骤 3：更新 UI4FlipTextBlock 消费方

**文件**：`src/StartUI4Controls/UI4FlipTextBlock.cs`

1. **修改** 第 295 行：`_userset_ui4panel.BorderBrush = CardBorderBrush` → `_userset_ui4panel.BorderColor = CardBorderBrush`
2. **修改** 第 514 行：`_userset_ui4panel.BorderBrush = (Color)e.NewValue` → `_userset_ui4panel.BorderColor = (Color)e.NewValue`
3. 第 298 行 `_userset_ui4panel.BorderThickness = CardBorderThickness` 无需修改（使用基类 DP）

## 步骤 4：修复 UI4CheckBox.BoxCornerRadius 命名

**文件**：`src/StartUI4Controls/UI4CheckBox.cs`

1. **新增** `CornerRadiusProperty`（`DependencyProperty.Register("CornerRadius", ...)`），默认值 `new CornerRadius(6)`
2. **新增** `CornerRadius` CLR 属性
3. **保留** `BoxCornerRadiusProperty` 静态字段，指向 `CornerRadiusProperty`（`public static readonly DependencyProperty BoxCornerRadiusProperty = CornerRadiusProperty;`）
4. **保留** `BoxCornerRadius` CLR 属性，标记 `[Obsolete("Use CornerRadius instead.")]`，getter/setter 使用 `CornerRadiusProperty`
5. **修改** 第 135 行：`nameof(BoxCornerRadius)` → `nameof(CornerRadius)`
6. **更新** XML 文档注释（第 18 行）

## 步骤 5：更新 XML 文档注释

- `UI4Panel.cs`：更新类注释，添加 `BorderColor` 说明
- `UI4CheckBox.cs`：更新类注释中 `BoxCornerRadius` → `CornerRadius`

---

## 依赖关系

```
步骤 1 (BorderBrush) ──→ 步骤 3 (UI4FlipTextBlock 更新)
步骤 2 (BorderThickness) ──→ 无依赖
步骤 4 (BoxCornerRadius) ──→ 无依赖
步骤 5 (XML 注释) ──→ 步骤 1-4 全部完成
```

步骤 1+2 可合并（同一文件），步骤 3 依赖步骤 1，步骤 4 独立可并行。

---

## 风险与缓解

| 风险 | 严重度 | 缓解 |
|------|--------|------|
| UI4Panel.BorderBrush 重命名为 BorderColor — 破坏现有 C# 代码 | 高 | 这是必要的破坏性修复。旧代码 `panel.BorderBrush = someColor` 会静默设置基类 DP（不触发 OnStyleUpdate），属于隐藏 bug。新代码 `panel.BorderColor = someColor` 行为正确 |
| UI4Panel.BorderThickness `new` 移除 — 行为变化 | 低 | 类型相同（Thickness），OverrideMetadata 添加回调后行为等价 |
| BoxCornerRadius 废弃 — CS0618 警告 | 低 | 保留旧属性转发到新 DP，现有代码编译通过但有警告 |
| UI4FlipTextBlock 赋值目标变化 | 低 | 类型仍为 Color，仅属性名变化 |

---

## 被否决的替代方案

### 方案 A：将所有 Color 属性统一改为 Brush（Agent B 建议）
- **否决原因**：影响 ~40 个属性、10+ 个控件，是大规模破坏性变更。Color 类型作为"简单颜色"的便捷 API 是合理的，WPF 原生控件也同时提供 `Background`（Brush）和允许 XAML 中用颜色字符串赋值。性能收益（减少 SolidColorBrush 分配）不足以证明风险。

### 方案 B：UI4Panel.BorderBrush 改用 OverrideMetadata 保持 Brush 类型
- **否决原因**：UI4Panel 内部使用 `ColorAnimation` 驱动悬停动画（第 232-252 行），需要 `Color` 类型值。如果改为 Brush 类型，需要重构整个动画逻辑。新增 `BorderColor`（Color）属性更简洁。

### 方案 C：直接删除 BoxCornerRadius 不保留兼容
- **否决原因**：保留 `[Obsolete]` 过渡属性的成本极低（一个静态字段 + 一个 CLR 包装器），但可以避免现有消费者编译失败。

---

## 关键文件

1. `src/StartUI4Controls/UI4Panel.cs` — 核心修复：`new` 隐藏问题（步骤 1-2）
2. `src/StartUI4Controls/UI4CheckBox.cs` — 命名修复：BoxCornerRadius → CornerRadius（步骤 4）
3. `src/StartUI4Controls/UI4FlipTextBlock.cs` — 消费方更新：BorderBrush → BorderColor（步骤 3）