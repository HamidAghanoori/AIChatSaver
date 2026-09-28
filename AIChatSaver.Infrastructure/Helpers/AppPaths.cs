using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIChatSaver.Infrastructure.Helpers
{
    /// <summary>
    /// مسیرهای اصلی برنامه را در یک جا نگه می‌دارد.
    /// همه‌ی فایل‌ها و پوشه‌های برنامه از این کلاس ساخته می‌شوند.
    /// پوشه‌ی اصلی: SRCFolder که کنار فایل EXE قرار دارد.
    /// </summary>
    public static class AppPaths
    {
        /// <summary>
        /// مسیر پوشه‌ی اجرایی برنامه (جایی که EXE قرار دارد).
        /// </summary>
        public static string BaseDirectory => AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// مسیر پوشه‌ی اصلی SRCFolder که کنار EXE است.
        /// </summary>
        public static string SrcFolder => Path.Combine(BaseDirectory, "SRCFolder");

        /// <summary>
        /// مسیر فایل متادیتای گفتگوها (meta.json).
        /// </summary>
        public static string MetaJsonPath => Path.Combine(SrcFolder, "meta.json");

        /// <summary>
        /// مسیر فایل دسته‌بندی‌ها (Categories.json).
        /// </summary>
        public static string CategoriesJsonPath => Path.Combine(SrcFolder, "Categories.json");

        /// <summary>
        /// مسیر فایل تنظیمات (settings.json).
        /// </summary>
        public static string SettingsJsonPath => Path.Combine(SrcFolder, "settings.json");

        /// <summary>
        /// مسیر یک پوشه‌ی دسته‌بندی خاص را برمی‌گرداند.
        /// مثال: SRCFolder/Fantasy
        /// </summary>
        public static string GetCategoryFolder(string categoryEnglishName)
        {
            return Path.Combine(SrcFolder, categoryEnglishName);
        }

        /// <summary>
        /// مطمئن می‌شود که پوشه‌ی SRCFolder و همه‌ی زیرپوشه‌های لازم وجود دارند.
        /// اگر وجود نداشته باشند، آن‌ها را می‌سازد.
        /// </summary>
        public static void EnsureSrcFolderExists()
        {
            if (!Directory.Exists(SrcFolder))
                Directory.CreateDirectory(SrcFolder);
        }

        /// <summary>
        /// مطمئن می‌شود که پوشه‌ی یک دسته‌بندی خاص وجود دارد.
        /// </summary>
        public static void EnsureCategoryFolderExists(string categoryEnglishName)
        {
            EnsureSrcFolderExists();
            string folder = GetCategoryFolder(categoryEnglishName);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
        }
    }
}
