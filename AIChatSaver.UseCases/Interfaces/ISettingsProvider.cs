using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIChatSaver.Domain.Entities;

namespace AIChatSaver.UseCases.Interfaces
{
    /// <summary>
    /// قرارداد برای خواندن و ذخیره‌ی تنظیمات برنامه (AppSettings).
    /// تنظیمات شامل روش نمایش HTML و دسته‌بندی پیش‌فرض است.
    /// پیاده‌سازی واقعی در لایه‌ی Infrastructure انجام می‌شود.
    /// </summary>
    public interface ISettingsProvider
    {
        /// <summary>
        /// تنظیمات فعلی برنامه را برمی‌گرداند.
        /// اگر فایل تنظیمات وجود نداشته باشد، مقادیر پیش‌فرض برگردانده می‌شود.
        /// </summary>
        AppSettings GetSettings();

        /// <summary>
        /// تنظیمات را در فایل ذخیره می‌کند.
        /// </summary>
        void SaveSettings(AppSettings settings);
    }
}
