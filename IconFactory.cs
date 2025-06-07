using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

/// <summary>
/// 用于加载和生成应用程序图标的工厂类
/// 支持从外部文件加载图标，如果文件不存在则使用程序化生成的图标
/// </summary>
public static class IconFactory
{
    /// <summary>
    /// 创建太阳图标（用于浅色主题）
    /// 优先从嵌入资源加载，然后尝试外部文件，最后使用程序化生成
    /// </summary>
    /// <returns>太阳形状的图标</returns>
    public static Icon CreateSunIcon()
    {
        // 尝试从嵌入资源加载图标
        Icon embeddedIcon = LoadIconFromEmbeddedResource("Icons.sun.ico");
        if (embeddedIcon != null)
        {
            return embeddedIcon;
        }

        // 尝试从外部 ICO 文件加载图标
        Icon externalIcon = LoadIconFromFile("Icons/sun.ico");
        if (externalIcon != null)
        {
            return externalIcon;
        }

        // 尝试从 SVG 文件加载图标
        Icon svgIcon = LoadIconFromSvg("Icons/sun.svg");
        if (svgIcon != null)
        {
            return svgIcon;
        }

        // 如果都不存在，使用程序化生成的图标
        return CreateSunIconProgrammatically();
    }

    /// <summary>
    /// 创建月亮图标（用于深色主题）
    /// 优先从嵌入资源加载，然后尝试外部文件，最后使用程序化生成
    /// </summary>
    /// <returns>月牙形状的图标</returns>
    public static Icon CreateMoonIcon()
    {
        // 尝试从嵌入资源加载图标
        Icon embeddedIcon = LoadIconFromEmbeddedResource("Icons.moon.ico");
        if (embeddedIcon != null)
        {
            return embeddedIcon;
        }

        // 尝试从外部 ICO 文件加载图标
        Icon externalIcon = LoadIconFromFile("Icons/moon.ico");
        if (externalIcon != null)
        {
            return externalIcon;
        }

        // 尝试从 SVG 文件加载图标
        Icon svgIcon = LoadIconFromSvg("Icons/moon.svg");
        if (svgIcon != null)
        {
            return svgIcon;
        }

        // 如果都不存在，使用程序化生成的图标
        return CreateMoonIconProgrammatically();
    }

    /// <summary>
    /// 从文件加载图标
    /// </summary>
    /// <param name="relativePath">相对于程序目录的图标文件路径</param>
    /// <returns>加载的图标，如果失败则返回 null</returns>
    private static Icon LoadIconFromFile(string relativePath)
    {
        try
        {
            // 获取程序所在目录
            string exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string iconPath = Path.Combine(exeDirectory, relativePath);

            // 检查文件是否存在
            if (File.Exists(iconPath))
            {
                return new Icon(iconPath);
            }
        }
        catch (Exception)
        {
            // 如果加载失败，返回 null，将使用程序化生成的图标
        }

        return null;
    }

    /// <summary>
    /// 从嵌入资源加载图标
    /// </summary>
    /// <param name="resourceName">嵌入资源名称，例如 "Icons.sun.ico"</param>
    /// <returns>加载的图标，如果失败则返回 null</returns>
    private static Icon LoadIconFromEmbeddedResource(string resourceName)
    {
        try
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string fullResourceName = $"{assembly.GetName().Name}.{resourceName}";

            using (Stream stream = assembly.GetManifestResourceStream(fullResourceName))
            {
                if (stream != null)
                {
                    return new Icon(stream);
                }
            }
        }
        catch (Exception)
        {
            // 如果加载失败，返回 null
        }

        return null;
    }

    /// <summary>
    /// 从 SVG 文件加载图标（简化版本，仅支持基本的 SVG）
    /// </summary>
    /// <param name="relativePath">相对于程序目录的 SVG 文件路径</param>
    /// <returns>转换后的图标，如果失败则返回 null</returns>
    private static Icon LoadIconFromSvg(string relativePath)
    {
        try
        {
            // 获取程序所在目录
            string exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string svgPath = Path.Combine(exeDirectory, relativePath);

            // 检查文件是否存在
            if (File.Exists(svgPath))
            {
                // 注意：这是一个简化的实现
                // 对于复杂的 SVG，建议转换为 ICO 文件
                string svgContent = File.ReadAllText(svgPath);

                // 这里我们返回 null，让程序使用程序化生成的图标
                // 因为完整的 SVG 解析需要额外的库
                return null;
            }
        }
        catch (Exception)
        {
            // 如果加载失败，返回 null
        }

        return null;
    }

    /// <summary>
    /// 程序化生成太阳图标（用于浅色主题）- Windows 10/11 扁平风格
    /// </summary>
    /// <returns>太阳形状的图标</returns>
    private static Icon CreateSunIconProgrammatically()
    {
        using (Bitmap bitmap = new Bitmap(32, 32, PixelFormat.Format32bppArgb))
        {
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                // 设置高质量渲染
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

                // 清除背景为透明
                graphics.Clear(Color.Transparent);

                // Windows 10/11 风格的颜色 - 使用系统主题色调
                Color iconColor = Color.FromArgb(255, 96, 96, 96); // 深灰色，类似系统图标

                using (Pen iconPen = new Pen(iconColor, 4.5f)) // 线条粗度增加到原来的3倍
                using (SolidBrush iconBrush = new SolidBrush(iconColor))
                {
                    int centerX = 16, centerY = 16;

                    // 绘制太阳光线 - 8条简洁的直线
                    int rayStartDistance = 8;  // 光线起始距离
                    int rayEndDistance = 13;   // 光线结束距离

                    for (int i = 0; i < 8; i++)
                    {
                        double angle = i * Math.PI / 4; // 每45度一条射线

                        // 计算射线起点和终点
                        int startX = centerX + (int)(Math.Cos(angle) * rayStartDistance);
                        int startY = centerY + (int)(Math.Sin(angle) * rayStartDistance);
                        int endX = centerX + (int)(Math.Cos(angle) * rayEndDistance);
                        int endY = centerY + (int)(Math.Sin(angle) * rayEndDistance);

                        // 绘制简洁的直线光线
                        graphics.DrawLine(iconPen, startX, startY, endX, endY);
                    }

                    // 绘制太阳主体 - 简洁的空心圆
                    int sunRadius = 6;
                    Rectangle sunRect = new Rectangle(centerX - sunRadius, centerY - sunRadius,
                                                    sunRadius * 2, sunRadius * 2);

                    // 只绘制圆形轮廓，不填充，保持扁平风格
                    graphics.DrawEllipse(iconPen, sunRect);
                }
            }

            // 转换为图标
            IntPtr hIcon = bitmap.GetHicon();
            return Icon.FromHandle(hIcon);
        }
    }
    
    /// <summary>
    /// 程序化生成月亮图标（用于深色主题）- Windows 10/11 扁平风格
    /// </summary>
    /// <returns>月牙形状的图标</returns>
    private static Icon CreateMoonIconProgrammatically()
    {
        using (Bitmap bitmap = new Bitmap(32, 32, PixelFormat.Format32bppArgb))
        {
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                // 设置高质量渲染
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

                // 清除背景为透明
                graphics.Clear(Color.Transparent);

                // Windows 10/11 风格的颜色 - 深色模式下使用白色以确保可见性
                Color iconColor = Color.FromArgb(255, 255, 255, 255); // 纯白色，在深色模式下清晰可见

                using (SolidBrush iconBrush = new SolidBrush(iconColor))
                {
                    int centerX = 16, centerY = 16;

                    // 创建月牙形状的路径 - 使用 GraphicsPath 绘制精确的月牙
                    using (GraphicsPath moonPath = new GraphicsPath())
                    {
                        // 月牙的外弧 - 左侧的大圆弧
                        int moonRadius = 9;
                        Rectangle outerCircle = new Rectangle(centerX - moonRadius, centerY - moonRadius,
                                                            moonRadius * 2, moonRadius * 2);

                        // 添加外圆弧（从上到下的左半圆）
                        moonPath.AddArc(outerCircle, -90, 180);

                        // 月牙的内弧 - 右侧的小圆弧（创建凹陷效果）
                        int innerRadius = 7;
                        int offsetX = 3; // 向右偏移
                        Rectangle innerCircle = new Rectangle(centerX - innerRadius + offsetX, centerY - innerRadius,
                                                            innerRadius * 2, innerRadius * 2);

                        // 添加内圆弧（从下到上的右半圆，逆向）
                        moonPath.AddArc(innerCircle, 90, -180);

                        // 闭合路径
                        moonPath.CloseFigure();

                        // 填充月牙形状
                        graphics.FillPath(iconBrush, moonPath);
                    }
                }
            }

            // 转换为图标
            IntPtr hIcon = bitmap.GetHicon();
            return Icon.FromHandle(hIcon);
        }
    }
}
