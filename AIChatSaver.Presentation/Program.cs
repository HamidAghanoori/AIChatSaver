using AIChatSaver.Infrastructure.Helpers;
using AIChatSaver.Presentation.Forms;
using CefSharp;
using CefSharp.WinForms;
namespace AIChatSaver.Presentation
{
    /// <summary>
    /// نقطه‌ی ورود برنامه.
    /// قبل از باز شدن فرم‌ها، CefSharp را مقداردهی اولیه می‌کند.
    /// </summary>
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // آماده‌سازی پوشه‌ی SRCFolder
            AppPaths.EnsureSrcFolderExists();

            // تنظیمات CefSharp
            var settings = new CefSettings
            {
                // غیرفعال کردن GPU برای جلوگیری از مشکل روی برخی سیستم‌ها
                // (اگر سیستم قوی داری می‌تونی true کنی)
                // WindowlessRenderingEnabled = true,
            };

            // مسیر کش CefSharp (کنار EXE)
            settings.CachePath = System.IO.Path.Combine(
                AppPaths.BaseDirectory, "CefCache");

            // غیرفعال کردن Log برای سرعت بیشتر
            settings.LogSeverity = LogSeverity.Disable;

            // مقداردهی اولیه CefSharp
            Cef.Initialize(settings);

            try
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new MainForm());
            }
            finally
            {
                // خاموش کردن تمیز CefSharp هنگام بستن برنامه
                Cef.Shutdown();
            }
        }
    }
}