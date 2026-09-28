using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIChatSaver.UseCases.Interfaces
{
    /// <summary>
    /// قرارداد برای خواندن و نوشتن فایل‌های HTML روی دیسک.
    /// چون Domain نباید چیزی درباره‌ی «فایل» بداند، این Interface در لایه‌ی Application تعریف می‌شود.
    /// پیاده‌سازی واقعی در لایه‌ی Infrastructure انجام می‌شود.
    /// </summary>
    public interface IHtmlFileStore
    {
        /// <summary>
        /// یک فایل HTML جدید را با محتوای داده‌شده در مسیر مشخص می‌نویسد.
        /// اگر پوشه وجود نداشته باشد، آن را می‌سازد.
        /// خروجی: مسیر کامل فایل نوشته‌شده.
        /// </summary>
        string WriteHtml(string categoryName, string fileName, string htmlContent);

        /// <summary>
        /// محتوای یک فایل HTML را از مسیر نسبی داده‌شده می‌خواند.
        /// اگر فایل وجود نداشته باشد، مقدار null برمی‌گرداند.
        /// </summary>
        string? ReadHtml(string categoryName, string fileName);

        /// <summary>
        /// بررسی می‌کند که آیا فایل HTML مورد نظر وجود دارد یا نه.
        /// </summary>
        bool Exists(string categoryName, string fileName);

        /// <summary>
        /// یک فایل HTML را حذف می‌کند.
        /// خروجی: true اگر حذف موفق بود، در غیر این صورت false.
        /// </summary>
        bool DeleteHtml(string categoryName, string fileName);

        /// <summary>
        /// مسیر کامل فایل HTML را برمی‌گرداند (حتی اگر فایل وجود نداشته باشد).
        /// برای ساخت URL از نوع file:/// استفاده می‌شود.
        /// </summary>
        string GetFullPath(string categoryName, string fileName);
    }
}
