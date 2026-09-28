using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AIChatSaver.UseCases.Interfaces;
using AIChatSaver.Domain.Entities;
using AIChatSaver.Infrastructure.Helpers;

namespace AIChatSaver.Infrastructure.Files
{
    /// <summary>
    /// پیاده‌سازی ISettingsProvider با استفاده از فایل JSON.
    /// تنظیمات در فایل settings.json داخل SRCFolder ذخیره می‌شود.
    /// اگر فایل نبود، مقادیر پیش‌فرض استفاده می‌شود.
    /// </summary>
    public class JsonSettingsProvider : ISettingsProvider
    {
        private AppSettings _cache;

        public JsonSettingsProvider()
        {
            _cache = LoadFromDisk();
        }

        private static AppSettings LoadFromDisk()
        {
            AppPaths.EnsureSrcFolderExists();

            if (!File.Exists(AppPaths.SettingsJsonPath))
                return new AppSettings();

            try
            {
                string json = File.ReadAllText(AppPaths.SettingsJsonPath);
                var settings = JsonSerializer.Deserialize<AppSettings>(json, JsonHelper.Options);
                return settings ?? new AppSettings();
            }
            catch
            {
                return new AppSettings();
            }
        }

        public AppSettings GetSettings()
        {
            return _cache;
        }

        public void SaveSettings(AppSettings settings)
        {
            _cache = settings;
            AppPaths.EnsureSrcFolderExists();
            string json = JsonSerializer.Serialize(settings, JsonHelper.Options);
            File.WriteAllText(AppPaths.SettingsJsonPath, json);
        }
    }
}
