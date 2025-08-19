Param(
    [switch]$CleanDest
)
$ErrorActionPreference = 'Stop'

# Determine repo root (scripts directory's parent)
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$RepoRoot  = Split-Path -Parent $ScriptDir

$Src = Join-Path $RepoRoot 'legacy\Admin\images'
$Dst = Join-Path $RepoRoot 'modern\frontend\public\legacy\admin\images'

Write-Host "[INFO] Repo Root: $RepoRoot"
Write-Host "[INFO] Source    : $Src"
Write-Host "[INFO] Dest      : $Dst"

if (-not (Test-Path $Src)) {
  Write-Error "Source not found: $Src"
  exit 1
}

# Ensure destination directory
New-Item -ItemType Directory -Force -Path $Dst | Out-Null

if ($CleanDest) {
  Write-Host "[CLEAN] Removing existing destination contents..."
  Get-ChildItem -Force -Recurse -Path $Dst | Remove-Item -Force -Recurse -ErrorAction SilentlyContinue
}

Write-Host "[COPY] Copying assets..."
Copy-Item -Recurse -Force -Path (Join-Path $Src '*') -Destination $Dst

# Summaries
$srcFiles = @(Get-ChildItem -Recurse -File $Src)
$dstFiles = @(Get-ChildItem -Recurse -File $Dst)
Write-Host "[OK] Copied $($srcFiles.Count) files to '$Dst'"

# Spot-check a few known files
$CheckList = @(
  'repeat.jpg',
  'pro_line_0.gif',
  'pro_line_1.gif',
  'login_bg.jpg',
  'loginbox_bg.png',
  'form_inp.gif',
  'inp_login.gif',
  'submit_login.gif',
  'table_header_repeat.jpg'
)
foreach ($name in $CheckList) {
  $found = Get-ChildItem -Recurse -File -Filter $name -Path $Dst | Select-Object -First 1
  if ($null -ne $found) {
    Write-Host "  - [OK] Found '$name' at $($found.FullName)"
  } else {
    Write-Warning "  - [WARN] Missing expected asset '$name'"
  }
}

exit 0