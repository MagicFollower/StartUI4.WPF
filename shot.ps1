param(
    [Parameter(Mandatory=$true)][int]$Tab,
    [int]$WaitMs = 4500
)

$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot
$exe = Join-Path $root 'samples\StartUI4Demo\bin\Debug\net48\StartUI4Demo.exe'
$shotDir = Join-Path $root 'shots'
if (-not (Test-Path $shotDir)) { New-Item -ItemType Directory -Path $shotDir | Out-Null }
$out = Join-Path $shotDir ("tab{0}.png" -f $Tab)

Add-Type @"
using System;
using System.Runtime.InteropServices;
public static class Win {
    [DllImport("user32.dll")] public static extern bool SetForegroundWindow(IntPtr h);
    [DllImport("user32.dll")] public static extern bool ShowWindow(IntPtr h, int cmd);
    [DllImport("user32.dll")] public static extern bool GetWindowRect(IntPtr h, out RECT r);
    [DllImport("user32.dll")] public static extern bool PrintWindow(IntPtr h, IntPtr hdc, uint flags);
    [StructLayout(LayoutKind.Sequential)] public struct RECT { public int L, T, R, B; }
}
"@

Add-Type -AssemblyName System.Drawing

$p = Start-Process -FilePath $exe -ArgumentList "--tab=$Tab" -PassThru
Start-Sleep -Milliseconds $WaitMs

if ($p.HasExited) {
    Write-Output "EXITED code=$($p.ExitCode)"
    exit 1
}

$p.Refresh()
$h = $p.MainWindowHandle
if ($h -eq [IntPtr]::Zero) {
    Start-Sleep -Milliseconds 1500
    $p.Refresh()
    $h = $p.MainWindowHandle
}

[Win]::ShowWindow($h, 9) | Out-Null
[Win]::SetForegroundWindow($h) | Out-Null
Start-Sleep -Milliseconds 1000

$r = New-Object Win+RECT
[Win]::GetWindowRect($h, [ref]$r) | Out-Null
$w = $r.R - $r.L
$ht = $r.B - $r.T
Write-Output "window=$($r.L),$($r.T) ${w}x${ht}"

$bmp = New-Object System.Drawing.Bitmap $w, $ht
$g = [System.Drawing.Graphics]::FromImage($bmp)
$hdc = $g.GetHdc()
$ok = [Win]::PrintWindow($h, $hdc, 2)
$g.ReleaseHdc($hdc)
$g.Dispose()
Write-Output "printwindow=$ok"
$bmp.Save($out, [System.Drawing.Imaging.ImageFormat]::Png)
$bmp.Dispose()

Stop-Process -Id $p.Id -Force
Write-Output "saved $out"
