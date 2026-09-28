using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AIChatSaver.Domain.Entities;
using AIChatSaver.Domain.Interfaces;
using AIChatSaver.Infrastructure.Helpers;

namespace AIChatSaver.Infrastructure.Files
{
    /// <summary>
    /// پیاده‌سازی ICategoryProvider با استفاده از فایل JSON.
    /// لیست دسته‌بندی‌ها در فایل Categories.json داخل SRCFolder ذخیره می‌شود.
    /// ساختار فایل:
    /// {
    ///   "Fantasy": "جادو، اژدها، دنیای خیالی",
    ///   "Science": "علم و دانش"
    /// }
    /// اگر فایل وجود نداشته باشد، یک لیست پیش‌فرض ساخته می‌شود.
    /// </summary>
    public class JsonCategoryProvider : ICategoryProvider
    {
        private readonly Dictionary<string, string> _categories;

        public JsonCategoryProvider()
        {
            _categories = LoadFromDisk();
        }

        /// <summary>
        /// فایل Categories.json را از دیسک می‌خواند.
        /// اگر فایل نبود، لیست پیش‌فرض را می‌سازد و ذخیره می‌کند.
        /// </summary>
        private static Dictionary<string, string> LoadFromDisk()
        {
            AppPaths.EnsureSrcFolderExists();

            if (!File.Exists(AppPaths.CategoriesJsonPath))
            {
                var defaults = GetDefaultCategories();
                SaveToDisk(defaults);
                return defaults;
            }

            try
            {
                string json = File.ReadAllText(AppPaths.CategoriesJsonPath);
                var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json, JsonHelper.Options);
                return dict ?? GetDefaultCategories();
            }
            catch
            {
                return GetDefaultCategories();
            }
        }

        /// <summary>
        /// لیست پیش‌فرض دسته‌بندی‌ها (همان لیستی که در صورت نبود فایل ساخته می‌شود).
        /// </summary>
        public static Dictionary<string, string> GetDefaultCategories()
        {
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Fantasy", "جادو، اژدها، دنیای خیالی" },
                { "Science Fiction (Sci-Fi)", "فضا، ربات‌ها، آینده" },
                { "Mystery", "معما و کارآگاهی" },
                { "Adventure", "ماجراجویی و سفر" },
                { "Horror", "ترسناک" },
                { "Romance", "عاشقانه" },
                { "Historical Fiction", "داستان تخیلی ولی توی گذشته واقعی" },
                { "Comics / Graphic Novels", "کمیک و رمان" },
                { "Science", "علم و دانش" },
                { "History", "تاریخ" },
                { "Biography", "زندگی‌نامه واقعی آدم‌ها" },
                { "Self-Help", "کتاب‌هایی که بهت یاد میدن چطور بهتر زندگی کنی" },
                { "Sports", "ورزش" },
                { "Cooking", "آشپزی" },
                { "Travel", "سفر و گردشگری" },
                { "Dictionary", "فرهنگ لغت" },
                { "Encyclopedia", "دایرةالمعارف" },
                { "Medicine", "پزشکی" },
                { "Atlas / Geography", "اطلس و نقشه و جغرافیا" },
                { "Language Learning", "آموزش زبان" },
                { "programing Learning", "آموزش برنامه نویسی" },
                { "Music Learning", "آموزش موسیقی" },
                { "Religious", "مذهبی" },
                { "Fun", "سرگرمی" },
                { "other Fiction", "دیگر مباحث خیالی" },
                { "other Non-Fiction", "دیگر مباحث غیر خیالی" },
                { "other Reference", "دیگر مباحث مرجع" },
                { "other Educational", "دیگر مباحث آموزشی" },
                { "other", "دیگر موارد ..." }
            };
        }

        private static void SaveToDisk(Dictionary<string, string> categories)
        {
            AppPaths.EnsureSrcFolderExists();
            string json = JsonSerializer.Serialize(categories, JsonHelper.Options);
            File.WriteAllText(AppPaths.CategoriesJsonPath, json);
        }

        public IReadOnlyList<Category> GetAll()
        {
            return _categories
                .Select(kv => new Category
                {
                    EnglishName = kv.Key,
                    PersianDescription = kv.Value
                })
                .ToList();
        }

        public Category? GetByEnglishName(string englishName)
        {
            if (!_categories.TryGetValue(englishName, out var persian))
                return null;

            return new Category
            {
                EnglishName = englishName,
                PersianDescription = persian
            };
        }

        public bool Exists(string englishName)
        {
            return !string.IsNullOrWhiteSpace(englishName) &&
                   _categories.ContainsKey(englishName);
        }
    }
}
