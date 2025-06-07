# Windows 主题切换器 - 发布构建脚本
# 用于构建 Release 版本并准备发布文件

param(
    [string]$Version = "1.0.0"
)

Write-Host "🚀 开始构建 Windows 主题切换器 v$Version" -ForegroundColor Green

# 清理之前的构建
Write-Host "🧹 清理之前的构建文件..." -ForegroundColor Yellow
if (Test-Path "bin") { Remove-Item "bin" -Recurse -Force }
if (Test-Path "obj") { Remove-Item "obj" -Recurse -Force }
if (Test-Path "release") { Remove-Item "release" -Recurse -Force }

# 查找 MSBuild
$msbuildPath = ""
$possiblePaths = @(
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"
)

foreach ($path in $possiblePaths) {
    if (Test-Path $path) {
        $msbuildPath = $path
        break
    }
}

if (-not $msbuildPath) {
    Write-Host "❌ 未找到 MSBuild，请确保安装了 Visual Studio" -ForegroundColor Red
    exit 1
}

Write-Host "🔨 使用 MSBuild: $msbuildPath" -ForegroundColor Cyan

# 构建项目
Write-Host "🔨 构建 Release 版本..." -ForegroundColor Yellow
& "$msbuildPath" WindowsThemeSwitcher.csproj /p:Configuration=Release /p:Platform="Any CPU" /verbosity:minimal

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ 构建失败" -ForegroundColor Red
    exit 1
}

# 创建发布目录
Write-Host "📦 准备发布文件..." -ForegroundColor Yellow
New-Item -ItemType Directory -Path "release" -Force | Out-Null

# 复制主要文件
Copy-Item "bin\Release\WindowsThemeSwitcher.exe" "release\"
Copy-Item "README.md" "release\"
Copy-Item "LICENSE" "release\"
Copy-Item "CHANGELOG.md" "release\"

# 获取文件信息
$exeFile = Get-Item "release\WindowsThemeSwitcher.exe"
$fileSize = [math]::Round($exeFile.Length / 1KB, 2)

Write-Host "✅ 构建完成！" -ForegroundColor Green
Write-Host ""
Write-Host "📋 发布信息:" -ForegroundColor Cyan
Write-Host "  版本: $Version"
Write-Host "  文件大小: $fileSize KB"
Write-Host "  发布目录: $(Resolve-Path 'release')"
Write-Host ""
Write-Host "📁 发布文件列表:" -ForegroundColor Cyan
Get-ChildItem "release" | ForEach-Object {
    Write-Host "  - $($_.Name)" -ForegroundColor White
}

Write-Host ""
Write-Host "🎉 现在可以将 release 文件夹中的内容上传到 GitHub Release！" -ForegroundColor Green
