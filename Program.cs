using Microsoft.Win32;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Reflection;

/// <summary>
/// Windows 主题切换器 - 系统托盘应用程序
/// 支持在浅色和深色主题之间切换
/// </summary>
public static class Program
{
    // 注册表路径和值名称
    private const string RegistryKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize";
    private const string RegistryValueName = "AppsUseLightTheme";

    // 自启动相关常量
    private const string StartupRegistryKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
    private const string StartupValueName = "WindowsThemeSwitcher";

    // 系统托盘图标和应用程序图标
    private static NotifyIcon trayIcon;
    private static Icon sunIcon;
    private static Icon moonIcon;

    /// <summary>
    /// 应用程序主入口点
    /// </summary>
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        try
        {
            // 使用 IconFactory 创建图标
            sunIcon = IconFactory.CreateSunIcon();
            moonIcon = IconFactory.CreateMoonIcon();

            // 初始化系统托盘图标
            InitializeTrayIcon();

            // 更新图标以反映当前主题状态
            UpdateIcon();

            // 显示托盘图标
            trayIcon.Visible = true;

            // 运行应用程序消息循环
            Application.Run();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"应用程序启动失败: {ex.Message}", "错误", 
                          MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// 初始化系统托盘图标
    /// </summary>
    private static void InitializeTrayIcon()
    {
        trayIcon = new NotifyIcon();
        trayIcon.Click += OnTrayIconClick;

        // 创建右键菜单
        ContextMenuStrip contextMenu = new ContextMenuStrip();

        // 添加切换主题菜单项
        ToolStripMenuItem toggleItem = new ToolStripMenuItem("切换主题", null, OnToggleTheme);
        contextMenu.Items.Add(toggleItem);

        // 添加分隔线
        contextMenu.Items.Add(new ToolStripSeparator());

        // 添加自启动菜单项
        ToolStripMenuItem startupItem = new ToolStripMenuItem();
        UpdateStartupMenuItem(startupItem);
        startupItem.Click += OnToggleStartup;
        contextMenu.Items.Add(startupItem);

        // 添加分隔线
        contextMenu.Items.Add(new ToolStripSeparator());

        // 添加退出菜单项
        ToolStripMenuItem exitItem = new ToolStripMenuItem("退出", null, OnExit);
        contextMenu.Items.Add(exitItem);
        
        trayIcon.ContextMenuStrip = contextMenu;
    }

    /// <summary>
    /// 处理托盘图标点击事件
    /// </summary>
    private static void OnTrayIconClick(object sender, EventArgs e)
    {
        if (e is MouseEventArgs mouseEvent && mouseEvent.Button == MouseButtons.Left)
        {
            ToggleTheme();
        }
    }

    /// <summary>
    /// 处理菜单中的切换主题事件
    /// </summary>
    private static void OnToggleTheme(object sender, EventArgs e)
    {
        ToggleTheme();
    }

    /// <summary>
    /// 切换 Windows 主题（浅色/深色）
    /// </summary>
    private static void ToggleTheme()
    {
        try
        {
            int currentValue = GetCurrentThemeValue();
            int newValue = (currentValue == 1) ? 0 : 1;

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true))
            {
                if (key != null)
                {
                    // 设置应用程序主题
                    key.SetValue("AppsUseLightTheme", newValue, RegistryValueKind.DWord);
                    // 设置系统主题
                    key.SetValue("SystemUsesLightTheme", newValue, RegistryValueKind.DWord);
                }
                else
                {
                    MessageBox.Show("无法访问注册表项，请确保以管理员权限运行。", "警告", 
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            // 更新托盘图标
            UpdateIcon();

            // 显示切换成功的通知
            string themeName = (newValue == 1) ? "浅色" : "深色";
            trayIcon.ShowBalloonTip(2000, "主题已切换", $"已切换到{themeName}主题", ToolTipIcon.Info);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"切换主题时发生错误: {ex.Message}", "错误", 
                          MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// 更新托盘图标以反映当前主题
    /// </summary>
    private static void UpdateIcon()
    {
        int currentValue = GetCurrentThemeValue();
        
        if (currentValue == 1)
        {
            // 浅色主题 - 显示太阳图标
            trayIcon.Icon = sunIcon;
            trayIcon.Text = "当前主题: 浅色 (点击切换到深色)";
        }
        else
        {
            // 深色主题 - 显示月亮图标
            trayIcon.Icon = moonIcon;
            trayIcon.Text = "当前主题: 深色 (点击切换到浅色)";
        }
    }

    /// <summary>
    /// 获取当前主题设置值
    /// </summary>
    /// <returns>1表示浅色主题，0表示深色主题</returns>
    private static int GetCurrentThemeValue()
    {
        try
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, false))
            {
                if (key != null)
                {
                    object value = key.GetValue(RegistryValueName, 1);
                    return Convert.ToInt32(value);
                }
                return 1; // 默认为浅色主题
            }
        }
        catch
        {
            return 1; // 出错时默认为浅色主题
        }
    }

    /// <summary>
    /// 处理自启动切换事件
    /// </summary>
    private static void OnToggleStartup(object sender, EventArgs e)
    {
        try
        {
            bool isCurrentlyEnabled = IsStartupEnabled();

            if (isCurrentlyEnabled)
            {
                // 禁用自启动
                DisableStartup();
                trayIcon.ShowBalloonTip(2000, "自启动已禁用", "程序将不会在系统启动时自动运行", ToolTipIcon.Info);
            }
            else
            {
                // 启用自启动
                EnableStartup();
                trayIcon.ShowBalloonTip(2000, "自启动已启用", "程序将在系统启动时自动运行", ToolTipIcon.Info);
            }

            // 更新菜单项显示
            if (sender is ToolStripMenuItem menuItem)
            {
                UpdateStartupMenuItem(menuItem);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"设置自启动时发生错误: {ex.Message}", "错误",
                          MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// 更新自启动菜单项的显示文本和状态
    /// </summary>
    private static void UpdateStartupMenuItem(ToolStripMenuItem menuItem)
    {
        bool isEnabled = IsStartupEnabled();
        menuItem.Text = isEnabled ? "✓ 开机自启动" : "开机自启动";
        menuItem.Checked = isEnabled;
    }

    /// <summary>
    /// 检查是否已启用自启动
    /// </summary>
    /// <returns>如果已启用自启动则返回 true</returns>
    private static bool IsStartupEnabled()
    {
        try
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(StartupRegistryKeyPath, false))
            {
                if (key != null)
                {
                    object value = key.GetValue(StartupValueName);
                    return value != null;
                }
            }
        }
        catch
        {
            // 如果读取失败，假设未启用
        }

        return false;
    }

    /// <summary>
    /// 启用开机自启动
    /// </summary>
    private static void EnableStartup()
    {
        try
        {
            string exePath = Assembly.GetExecutingAssembly().Location;

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(StartupRegistryKeyPath, true))
            {
                if (key != null)
                {
                    key.SetValue(StartupValueName, exePath, RegistryValueKind.String);
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"无法启用自启动: {ex.Message}");
        }
    }

    /// <summary>
    /// 禁用开机自启动
    /// </summary>
    private static void DisableStartup()
    {
        try
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(StartupRegistryKeyPath, true))
            {
                if (key != null)
                {
                    key.DeleteValue(StartupValueName, false);
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"无法禁用自启动: {ex.Message}");
        }
    }

    /// <summary>
    /// 处理退出事件
    /// </summary>
    private static void OnExit(object sender, EventArgs e)
    {
        // 隐藏托盘图标
        trayIcon.Visible = false;
        
        // 释放资源
        sunIcon?.Dispose();
        moonIcon?.Dispose();
        trayIcon?.Dispose();
        
        // 退出应用程序
        Application.Exit();
    }
}
