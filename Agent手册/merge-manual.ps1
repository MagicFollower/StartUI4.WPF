# 合并 Agent 手册各章为单一大文档。
# 章节按文件名顺序收集（ch01 … ch24），本目录下的 编写规范.md / 事实速查.md / _弃稿/ 不参与合并。
# 用法：powershell -ExecutionPolicy Bypass -File .\merge-manual.ps1 [-Out <路径>]
param(
    [string]$Out = ''
)

$ErrorActionPreference = 'Stop'

$root = $PSScriptRoot
if ([string]::IsNullOrEmpty($Out)) {
    $Out = Join-Path $root 'AI-Agent-WPF-Handbook.md'
}

$files = Get-ChildItem -LiteralPath $root -Filter 'ch*.md' -File | Sort-Object Name
if ($files.Count -ne 24) {
    Write-Host ("预期 24 章，实际找到 {0} 章：{1}" -f $files.Count, (($files | ForEach-Object { $_.Name }) -join ', '))
}

# 一级标题（章标题）与二级标题（小节）都进目录，便于长文档跳转
$sb = New-Object System.Text.StringBuilder
$null = $sb.AppendLine('# 面向 AI Agent 的 WPF 业务应用接入操作手册')
$null = $sb.AppendLine()
$null = $sb.AppendLine('> 本手册面向要在本组件库之上新建 WPF 业务应用、并继续做业务开发的 AI Agent。')
$null = $sb.AppendLine('> 全文 24 章分五个区（分区口径同第 01 章 §1.3.2）：第 01–05 章认知与准备，第 06–11 章应用骨架，')
$null = $sb.AppendLine('> 第 12–17 章主题与品牌，第 18–22 章控件实操，第 23–24 章业务接入与交付。')
$null = $sb.AppendLine('> 按任务查章请先读第 01 章 §1.3.3 的任务路由表。')
$null = $sb.AppendLine()
$null = $sb.AppendLine('本文件由 `Agent手册/merge-manual.ps1` 从 `Agent手册/chNN-*.md` 生成，请勿直接编辑合并产物；')
$null = $sb.AppendLine('要改内容请改对应章节文件后重新执行脚本。')
$null = $sb.AppendLine()
$null = $sb.AppendLine('## 目录')
$null = $sb.AppendLine()

foreach ($f in $files) {
    $text = [System.IO.File]::ReadAllText($f.FullName)
    $lines = $text -split "`r?`n"
    $h1 = ''
    foreach ($l in $lines) { if ($l.StartsWith('# ')) { $h1 = $l.Substring(2).Trim(); break } }
    if ([string]::IsNullOrEmpty($h1)) { $h1 = $f.BaseName }
    $null = $sb.AppendLine(('- ' + $h1))
    foreach ($l in $lines) {
        if ($l.StartsWith('## ')) {
            $h2 = $l.Substring(3).Trim()
            $null = $sb.AppendLine('  - ' + $h2)
        }
    }
}

$null = $sb.AppendLine()
$null = $sb.AppendLine('---')

foreach ($f in $files) {
    $text = [System.IO.File]::ReadAllText($f.FullName)
    $null = $sb.AppendLine()
    $null = $sb.AppendLine($text.Trim())
    $null = $sb.AppendLine()
    $null = $sb.AppendLine('---')
}

$body = $sb.ToString()
$utf8WithBom = New-Object System.Text.UTF8Encoding($true)
[System.IO.File]::WriteAllText($Out, $body, $utf8WithBom)

$chapters = ([regex]::Matches($body, "(?m)^# 第 \d+ 章")).Count
Write-Host ("已生成 {0}" -f $Out)
Write-Host ("章标题数 = {0}，行数 = {1}" -f $chapters, (($body -split "`r?`n").Count))
