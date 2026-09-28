param(
    [Parameter(Mandatory=$true)][ValidateSet('msgbox','color','menu','ctx','tabadd','tray','hover','combo')][string]$Scenario
)

$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot
$exe = Join-Path $root 'samples\StartUI4Demo\bin\Debug\net48\StartUI4Demo.exe'
$log = Join-Path (Split-Path $exe) 'demo-errors.log'
$shotDir = Join-Path $root 'shots'
if (Test-Path $log) { Remove-Item $log -Force }

Add-Type @"
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
public static class W {
    [DllImport("user32.dll")] public static extern bool SetForegroundWindow(IntPtr h);
    [DllImport("user32.dll")] public static extern bool ShowWindow(IntPtr h, int cmd);
    [DllImport("user32.dll")] public static extern bool GetWindowRect(IntPtr h, out RECT r);
    [DllImport("user32.dll")] public static extern bool PrintWindow(IntPtr h, IntPtr hdc, uint flags);
    [DllImport("user32.dll")] public static extern bool IsWindowVisible(IntPtr h);
    [DllImport("user32.dll")] public static extern bool SetCursorPos(int x, int y);
    [DllImport("user32.dll")] public static extern void mouse_event(uint f, uint dx, uint dy, uint d, UIntPtr e);
    [DllImport("user32.dll")] public static extern int GetWindowText(IntPtr h, StringBuilder s, int n);
    [DllImport("user32.dll")] public static extern IntPtr GetForegroundWindow();
    [DllImport("user32.dll")] public static extern bool AttachThreadInput(uint a, uint b, bool attach);
    [DllImport("user32.dll")] public static extern bool BringWindowToTop(IntPtr h);
    [DllImport("kernel32.dll")] public static extern uint GetCurrentThreadId();
    public static void ForceForeground(IntPtr h) {
        IntPtr fg = GetForegroundWindow();
        uint pid; uint fgThread = GetWindowThreadProcessId(fg, out pid);
        uint myThread = GetCurrentThreadId();
        if (fgThread != myThread) AttachThreadInput(fgThread, myThread, true);
        ShowWindow(h, 9);
        BringWindowToTop(h);
        SetForegroundWindow(h);
        if (fgThread != myThread) AttachThreadInput(fgThread, myThread, false);
    }
    public delegate bool EnumProc(IntPtr h, IntPtr l);
    [DllImport("user32.dll")] public static extern bool EnumWindows(EnumProc p, IntPtr l);
    [DllImport("user32.dll")] public static extern uint GetWindowThreadProcessId(IntPtr h, out uint pid);
    [StructLayout(LayoutKind.Sequential)] public struct RECT { public int L, T, R, B; }
    public static List<IntPtr> WindowsOf(uint pid) {
        var list = new List<IntPtr>();
        EnumWindows(delegate(IntPtr h, IntPtr l) {
            uint p; GetWindowThreadProcessId(h, out p);
            if (p == pid && IsWindowVisible(h)) list.Add(h);
            return true;
        }, IntPtr.Zero);
        return list;
    }
    public static string Title(IntPtr h) {
        var sb = new StringBuilder(256); GetWindowText(h, sb, 256); return sb.ToString();
    }
}
"@

Add-Type -AssemblyName System.Drawing
Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName UIAutomationClient
Add-Type -AssemblyName UIAutomationTypes

function Shot-Window([IntPtr]$h, [string]$path) {
    $r = New-Object W+RECT
    [W]::GetWindowRect($h, [ref]$r) | Out-Null
    $w = $r.R - $r.L; $ht = $r.B - $r.T
    if ($w -le 0 -or $ht -le 0) { Write-Output "skip empty window $path"; return }
    $bmp = New-Object System.Drawing.Bitmap $w, $ht
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $hdc = $g.GetHdc()
    [W]::PrintWindow($h, $hdc, 2) | Out-Null
    $g.ReleaseHdc($hdc); $g.Dispose()
    $bmp.Save($path, [System.Drawing.Imaging.ImageFormat]::Png)
    $bmp.Dispose()
    Write-Output "saved $path (${w}x${ht})"
}

function Shot-Screen([string]$path) {
    $b = [System.Windows.Forms.Screen]::PrimaryScreen.Bounds
    $bmp = New-Object System.Drawing.Bitmap $b.Width, $b.Height
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $g.CopyFromScreen(0, 0, 0, 0, $bmp.Size)
    $g.Dispose()
    $bmp.Save($path, [System.Drawing.Imaging.ImageFormat]::Png)
    $bmp.Dispose()
    Write-Output "saved $path"
}

function Shot-Region([int]$x, [int]$y, [int]$w, [int]$h, [string]$path) {
    $bmp = New-Object System.Drawing.Bitmap $w, $h
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $g.CopyFromScreen($x, $y, 0, 0, $bmp.Size)
    $g.Dispose()
    $bmp.Save($path, [System.Drawing.Imaging.ImageFormat]::Png)
    $bmp.Dispose()
    Write-Output "saved $path"
}

function Click-At([int]$x, [int]$y, [bool]$right) {
    [W]::SetCursorPos($x, $y) | Out-Null
    Start-Sleep -Milliseconds 150
    if ($right) {
        [W]::mouse_event(0x0008, 0, 0, 0, [UIntPtr]::Zero)
        [W]::mouse_event(0x0010, 0, 0, 0, [UIntPtr]::Zero)
    } else {
        [W]::mouse_event(0x0002, 0, 0, 0, [UIntPtr]::Zero)
        [W]::mouse_event(0x0004, 0, 0, 0, [UIntPtr]::Zero)
    }
}

function Find-ByName($scope, [string]$name) {
    $c = New-Object System.Windows.Automation.PropertyCondition([System.Windows.Automation.AutomationElement]::NameProperty, $name)
    return $scope.FindFirst([System.Windows.Automation.TreeScope]::Descendants, $c)
}

function Invoke-El($el) {
    $p = $el.GetCurrentPattern([System.Windows.Automation.InvokePattern]::Pattern)
    $p.Invoke()
}

$tab = @{ msgbox = 8; color = 8; menu = 9; ctx = 9; tabadd = 6; tray = 9; hover = 5; combo = 3 }[$Scenario]

$p = Start-Process -FilePath $exe -ArgumentList "--tab=$tab" -PassThru
Start-Sleep -Milliseconds 4500
if ($p.HasExited) { Write-Output "EXITED code=$($p.ExitCode)"; exit 1 }
$p.Refresh()
$main = $p.MainWindowHandle
[W]::ShowWindow($main, 9) | Out-Null
[W]::SetForegroundWindow($main) | Out-Null
Start-Sleep -Milliseconds 800

$autoRoot = [System.Windows.Automation.AutomationElement]::RootElement
$pc = New-Object System.Windows.Automation.PropertyCondition([System.Windows.Automation.AutomationElement]::ProcessIdProperty, [int]$p.Id)
$appRoot = $autoRoot.FindFirst([System.Windows.Automation.TreeScope]::Children, $pc)

switch ($Scenario) {
    'msgbox' {
        Invoke-El (Find-ByName $appRoot '仅确定')
        Start-Sleep -Milliseconds 1200
        $wins = [W]::WindowsOf([uint32]$p.Id)
        foreach ($h in $wins) {
            $t = [W]::Title($h)
            if ($t -match '提示') { Shot-Window $h (Join-Path $shotDir 'dlg-msgbox.png') }
        }
        $dlg = $null
        foreach ($h in $wins) { if (([W]::Title($h)) -match '提示') { $dlg = $h } }
        if ($dlg) {
            $dc = New-Object System.Windows.Automation.PropertyCondition([System.Windows.Automation.AutomationElement]::ProcessIdProperty, [int]$p.Id)
            $dw = $autoRoot.FindFirst([System.Windows.Automation.TreeScope]::Children, $dc)
            # 对话框是同一进程的另一个顶层窗口，按标题定位
            $tc = New-Object System.Windows.Automation.PropertyCondition([System.Windows.Automation.AutomationElement]::NameProperty, '提示')
            $dlgEl = $autoRoot.FindFirst([System.Windows.Automation.TreeScope]::Children, $tc)
            if ($dlgEl) { Invoke-El (Find-ByName $dlgEl '确定') }
            Start-Sleep -Milliseconds 800
            Shot-Window $main (Join-Path $shotDir 'dlg-msgbox-after.png')
        }
    }
    'color' {
        Invoke-El (Find-ByName $appRoot '选择颜色...')
        Start-Sleep -Milliseconds 1500
        foreach ($h in [W]::WindowsOf([uint32]$p.Id)) {
            $t = [W]::Title($h)
            if ($t -match '选择颜色') { Shot-Window $h (Join-Path $shotDir 'dlg-color.png') }
        }
        [System.Windows.Forms.SendKeys]::SendWait('{ESC}')
        Start-Sleep -Milliseconds 800
    }
    'menu' {
        $el = Find-ByName $appRoot '文件 (F)'
        if (-not $el) { $el = Find-ByName $appRoot '文件' }
        $r = $el.Current.BoundingRectangle
        Click-At ([int]($r.X + $r.Width/2)) ([int]($r.Y + $r.Height/2)) $false
        Start-Sleep -Milliseconds 900
        Shot-Screen (Join-Path $shotDir 'dlg-menu.png')
        [System.Windows.Forms.SendKeys]::SendWait('{ESC}')
    }
    'ctx' {
        $el = Find-ByName $appRoot '右键点击这里'
        $r = $el.Current.BoundingRectangle
        Click-At ([int]($r.X + $r.Width/2)) ([int]($r.Y + $r.Height/2)) $true
        Start-Sleep -Milliseconds 900
        Shot-Screen (Join-Path $shotDir 'dlg-ctx.png')
        [System.Windows.Forms.SendKeys]::SendWait('{ESC}')
    }
    'tabadd' {
        $wr = New-Object W+RECT
        [W]::GetWindowRect($main, [ref]$wr) | Out-Null
        $btns = $appRoot.FindAll([System.Windows.Automation.TreeScope]::Descendants,
            (New-Object System.Windows.Automation.PropertyCondition([System.Windows.Automation.AutomationElement]::ControlTypeProperty, [System.Windows.Automation.ControlType]::Button)))
        $best = $null
        foreach ($b in $btns) {
            $r = $b.Current.BoundingRectangle
            if ($r.Y -gt ($wr.T + 330) -and $r.Y -lt ($wr.T + 410) -and $r.X -gt ($wr.L + 400)) {
                if (-not $best -or $r.X -gt $best.Current.BoundingRectangle.X) { $best = $b }
            }
        }
        if ($best) {
            $br = $best.Current.BoundingRectangle
            Write-Output ("addbutton at {0},{1}" -f [int]$br.X, [int]$br.Y)
            Invoke-El $best
            Start-Sleep -Milliseconds 900
        } else { Write-Output 'add button not found' }
        Shot-Window $main (Join-Path $shotDir 'dlg-tabadd.png')
    }
    'tray' {
        $label = Find-ByName $appRoot '启用托盘图标（使用系统默认图标，左键/右键均有事件）'
        if (-not $label) { Write-Output 'tray label not found' }
        $r = $label.Current.BoundingRectangle
        # 开关在说明文字左侧
        Click-At ([int]($r.X - 25)) ([int]($r.Y + $r.Height/2)) $false
        Start-Sleep -Milliseconds 1500
        $b = [System.Windows.Forms.Screen]::PrimaryScreen.Bounds
        $bmp = New-Object System.Drawing.Bitmap 500, 60
        $g = [System.Drawing.Graphics]::FromImage($bmp)
        $g.CopyFromScreen($b.Width - 500, $b.Height - 60, 0, 0, $bmp.Size)
        $g.Dispose()
        $bmp.Save((Join-Path $shotDir 'tray.png'), [System.Drawing.Imaging.ImageFormat]::Png)
        $bmp.Dispose()
        Write-Output 'saved tray.png'
        # 点开托盘溢出区，确认图标已注册
        Click-At ($b.Width - 500 + 100) ($b.Height - 60 + 45) $false
        Start-Sleep -Milliseconds 1200
        $bmp2 = New-Object System.Drawing.Bitmap 700, 400
        $g2 = [System.Drawing.Graphics]::FromImage($bmp2)
        $g2.CopyFromScreen($b.Width - 700, $b.Height - 460, 0, 0, $bmp2.Size)
        $g2.Dispose()
        $bmp2.Save((Join-Path $shotDir 'tray-overflow.png'), [System.Drawing.Imaging.ImageFormat]::Png)
        $bmp2.Dispose()
        Write-Output 'saved tray-overflow.png'
        Shot-Window $main (Join-Path $shotDir 'tray-main.png')
    }
    'hover' {
        $items = $appRoot.FindAll([System.Windows.Automation.TreeScope]::Descendants,
            (New-Object System.Windows.Automation.PropertyCondition([System.Windows.Automation.AutomationElement]::ControlTypeProperty, [System.Windows.Automation.ControlType]::ListItem)))
        $target = $null
        foreach ($it in $items) {
            $r = $it.Current.BoundingRectangle
            if ($r.Width -gt 400 -and $r.Height -gt 40) { $target = $it; break }
        }
        if (-not $target) { Write-Output 'list card not found' }
        $r = $target.Current.BoundingRectangle
        Write-Output ("before {0:N1}x{1:N1}" -f $r.Width, $r.Height)
        [W]::ShowWindow($main, 9) | Out-Null
        [W]::ForceForeground($main)
        Start-Sleep -Milliseconds 400
        [W]::SetCursorPos([int]($r.X + $r.Width/2) - 4, [int]($r.Y + $r.Height/2)) | Out-Null
        [W]::mouse_event(0x0001, 4, 0, 0, [UIntPtr]::Zero)
        Start-Sleep -Milliseconds 1200
        $r2 = $target.Current.BoundingRectangle
        Write-Output ("after  {0:N1}x{1:N1}" -f $r2.Width, $r2.Height)
        $cx = [int]$r.X - 30; $cy = [int]$r.Y - 40
        $cw = [int]$r.Width + 60; $ch = [int]$r.Height + 120
        $bmp = New-Object System.Drawing.Bitmap $cw, $ch
        $g = [System.Drawing.Graphics]::FromImage($bmp)
        $g.CopyFromScreen($cx, $cy, 0, 0, $bmp.Size)
        $g.Dispose()
        $bmp.Save((Join-Path $shotDir 'hover-list.png'), [System.Drawing.Imaging.ImageFormat]::Png)
        $bmp.Dispose()
        Write-Output 'saved hover-list.png'
    }
    'combo' {
        $combos = $appRoot.FindAll([System.Windows.Automation.TreeScope]::Descendants,
            (New-Object System.Windows.Automation.PropertyCondition([System.Windows.Automation.AutomationElement]::ControlTypeProperty, [System.Windows.Automation.ControlType]::ComboBox)))
        $cb = $combos[$combos.Count - 1]
        $r = $cb.Current.BoundingRectangle
        Shot-Region ([int]$r.X - 15) ([int]$r.Y - 15) ([int]$r.Width + 30) ([int]$r.Height + 30) (Join-Path $shotDir 'combo-closed.png')
        [W]::ShowWindow($main, 9) | Out-Null
        [W]::ForceForeground($main)
        Start-Sleep -Milliseconds 300
        $ep = $cb.GetCurrentPattern([System.Windows.Automation.ExpandCollapsePattern]::Pattern)
        $ep.Expand()
        Start-Sleep -Milliseconds 900
        Shot-Region ([int]$r.X - 15) ([int]$r.Y - 15) ([int]$r.Width + 620) ([int]$r.Height + 130) (Join-Path $shotDir 'combo-open.png')
    }
}

Start-Sleep -Milliseconds 400
if (Test-Path $log) {
    Write-Output '--- RUNTIME ERRORS ---'
    Get-Content $log -Encoding UTF8 | Select-Object -First 15
} else {
    Write-Output 'no runtime errors'
}
Stop-Process -Id $p.Id -Force
