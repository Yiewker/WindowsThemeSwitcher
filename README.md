# Windows 主题切换器

<div align="center">

![Windows Theme Switcher](https://img.shields.io/badge/Windows-10%2F11-blue?style=flat-square&logo=windows)
![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.8-purple?style=flat-square&logo=.net)
![License](https://img.shields.io/badge/License-MIT-green?style=flat-square)
![Release](https://img.shields.io/github/v/release/Yiewker/WindowsThemeSwitcher?style=flat-square)

一个简单易用的 Windows 10/11 主题切换器，运行在系统托盘中，支持在浅色和深色主题之间快速切换。

[下载最新版本](https://github.com/Yiewker/WindowsThemeSwitcher/releases/latest) | [功能演示](#演示) | [使用说明](#使用方法)

</div>

## ✨ 功能特点

- 🌞 **智能图标显示**: 浅色主题显示太阳图标，深色主题显示月亮图标
- 🖱️ **一键切换**: 左键点击托盘图标即可快速切换主题
- 🎨 **自定义图标**: 支持外部图标文件，内置精美的扁平风格图标
- 💡 **实时反馈**: 切换后显示气泡通知提示
- 🚀 **开机自启动**: 支持开机自动启动，状态持久化记忆
- 🔧 **右键菜单**: 提供切换主题、自启动管理和退出选项
- 📦 **单文件运行**: 完全独立的 EXE 文件，无需安装和外部依赖
- 🎯 **轻量高效**: 占用内存极小，运行流畅

## 📥 下载安装

### 方式一：直接下载（推荐）
1. 访问 [Releases 页面](https://github.com/Yiewker/WindowsThemeSwitcher/releases)
2. 下载 `WindowsThemeSwitcher.exe` 文件
3. 双击运行即可，无需安装

### 方式二：从源码编译
```bash
# 克隆仓库
git clone https://github.com/Yiewker/WindowsThemeSwitcher.git
cd WindowsThemeSwitcher

# 编译项目（需要 Visual Studio 或 MSBuild）
msbuild WindowsThemeSwitcher.csproj /p:Configuration=Release

# 运行程序
.\bin\Release\WindowsThemeSwitcher.exe
```

## 🎮 使用方法

### 基本操作
- **左键点击托盘图标**: 在浅色和深色主题之间切换
- **右键点击托盘图标**: 显示上下文菜单

### 右键菜单功能
```
┌─────────────────┐
│ 切换主题        │
├─────────────────┤
│ ✓ 开机自启动    │  ← 点击切换自启动状态
├─────────────────┤
│ 退出            │
└─────────────────┘
```

### 图标说明
- 🌞 **太阳图标**: 当前为浅色主题
- 🌙 **月亮图标**: 当前为深色主题

## 🎬 演示

### 主题切换效果
<!-- TODO: 插入主题切换效果的 GIF 图片 -->

### 系统托盘界面
<!-- TODO: 插入系统托盘界面的 GIF 图片 -->

## ⚙️ 高级功能

### 自定义图标
1. 将自定义的 `sun.ico` 和 `moon.ico` 文件放入 `Icons` 文件夹
2. 重新编译项目，图标将自动嵌入到 EXE 文件中
3. 支持的格式：ICO（推荐 32x32 像素）

### 开机自启动
- 右键菜单中点击"开机自启动"即可启用/禁用
- 设置会自动保存，重启后保持状态
- 通过注册表 `HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run` 实现

## 🔧 技术实现

### 架构设计
```
WindowsThemeSwitcher/
├── Program.cs              # 主程序逻辑和系统托盘管理
├── IconFactory.cs          # 图标加载和生成工厂
├── Icons/                  # 图标资源文件夹
│   ├── sun.ico            # 太阳图标（浅色主题）
│   └── moon.ico           # 月亮图标（深色主题）
├── app-icon.ico           # 应用程序图标
└── WindowsThemeSwitcher.csproj  # 项目配置文件
```

### 核心技术
- **注册表操作**: 通过修改 Windows 个性化设置实现主题切换
- **系统托盘集成**: 使用 NotifyIcon 实现托盘功能
- **资源嵌入**: 图标文件嵌入到 EXE 中，实现单文件部署
- **自启动管理**: 通过注册表管理开机自启动

### 注册表路径
- **主题设置**: `HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize`
  - `AppsUseLightTheme`: 控制应用程序主题
  - `SystemUsesLightTheme`: 控制系统主题
- **自启动设置**: `HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run`

## 💻 系统要求

- **操作系统**: Windows 10 版本 1903+ 或 Windows 11
- **运行时**: .NET Framework 4.8（通常已预装）
- **权限**: 当前用户权限（无需管理员权限）
- **内存**: < 10MB
- **存储**: < 1MB

## ❓ 故障排除

<details>
<summary><strong>常见问题解答</strong></summary>

### Q: 无法切换主题怎么办？
**A**:
1. 确保 Windows 版本支持主题切换（Windows 10 1903+）
2. 检查是否启用了"个性化"设置
3. 尝试重启应用程序

### Q: 系统托盘中看不到图标？
**A**:
1. 检查系统托盘设置，确保允许显示所有图标
2. 右键任务栏 → 任务栏设置 → 选择哪些图标显示在任务栏上
3. 重启应用程序

### Q: 开机自启动不生效？
**A**:
1. 确保已在右键菜单中启用自启动
2. 检查杀毒软件是否阻止了注册表修改
3. 手动检查注册表项是否存在

### Q: 编译时出现错误？
**A**:
1. 确保安装了 .NET Framework 4.8 SDK
2. 使用 Visual Studio 2019+ 或对应的 MSBuild 版本
3. 检查所有文件是否完整

</details>

## 🤝 贡献指南

欢迎提交 Issue 和 Pull Request！

### 开发环境设置
1. 安装 Visual Studio 2019+ 或 Visual Studio Code
2. 安装 .NET Framework 4.8 SDK
3. 克隆仓库并打开项目

### 提交规范
- 使用清晰的提交信息
- 遵循现有的代码风格
- 添加必要的注释和文档

## 🗺️ 路线图

- [ ] 支持自定义快捷键
- [ ] 添加主题切换动画效果
- [ ] 支持更多个性化选项
- [ ] 多语言支持
- [ ] 主题定时切换功能

## 📄 许可证

本项目采用 [MIT 许可证](LICENSE)。

## ⭐ 支持项目

如果这个项目对你有帮助，请给个 Star ⭐ 支持一下！

---

<div align="center">

**[🏠 返回顶部](#windows-主题切换器)**

Made with ❤️ by [Yiewker](https://github.com/Yiewker)

</div>
