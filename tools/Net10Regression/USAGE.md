# Net10Regression — 迁移回归 harness 用法

```
dotnet build tools/Net10Regression/Net10Regression.csproj
dotnet run --project tools/Net10Regression -- --all
```

| 参数 | 作用 |
|---|---|
| `--all` | 系统级 + 13 页 UIA + 进程内断言（缺省行为） |
| `--tabs` | 只跑逐页 UIA 走查（起 `StartUI4Demo.exe --tab=N`，N=0..12） |
| `--tab=N` | 只跑第 N 页，便于定位 |
| `--inproc` | 只跑进程内 STA 断言（色值、对比度、作用域、剪贴板通道、多语言、持久化、标题栏） |
| `--system` | 只跑系统级：TFM 元数据、runtimeconfig、无 exe.config、注册表零污染、异常日志、剪贴板持锁矩阵 |
| `--exe <目录>` | 被测 Demo 的产物目录（含 `StartUI4Demo.exe`），缺省取 `samples/StartUI4Demo/bin/Debug/net10.0-windows` |
| `--baseline <目录>` | 再跑一遍同一套 UIA 用例作对照（如 net48 的 `bin/Release/net48`），用于出「行为差异表」 |
| `--out <文件>` | `results.md` 落盘位置，缺省写在本程序产物目录 |
| `--wait <ms>` | 等主窗口的超时，缺省 15000 |

退出码 = FAIL 用例数，可直接进 CI。

## 为什么是这个形态

- **本机 WPF 抓屏恒返回全白**（`PORTING.md` 第 11、13 节记录过，基线同样如此），截图法只能证明「没崩」证明不了「颜色对」。
  所以颜色类断言一律走 `--inproc`：离屏承载控件、`ApplyTemplate` 后用 `VisualTreeHelper` 读解析后的实际画刷色，再算 WCAG 对比度。
- **UIA 侧不改动 Demo**：整棵控件视图抓成 `Name/Type/Value` 快照，按页面上真实存在的中文文案断言。
  好处是同一套用例可以原样跑 net48 基线（`--baseline`），差异只可能来自运行时，不可能来自测试脚本。
- **Ctrl+C/X/V 用真按键**：`keybd_event` 发组合键，再用 harness 自己的原生 `GetClipboardData` 读回——
  「页面说复制成功了」和「系统剪贴板里真有什么」是两条独立证据，正是这轮迁移最需要守住的通道。
- **剪贴板持锁矩阵**复用 `tools/clipboard-lock-check`：独立进程 `OpenClipboard` 持锁 + `--seed` 播种 + `--dump` 原生读回。

## 用例编号约定

| 前缀 | 作用域 |
|---|---|
| `S01…S08` | 系统级（运行真实性三层、注册表、异常日志、剪贴板矩阵） |
| `T<页>-<名>` | 逐页 UIA，如 `T4-ring-advance`、`T1-ctrl-x`、`T12-persist-file` |
| `I01…I09` | 进程内 STA 断言 |
