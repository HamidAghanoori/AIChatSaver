using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIChatSaver.Domain.Entities;

namespace AIChatSaver.Domain.Interfaces
{
    /// <summary>
    /// قرارداد برای گرفتن لیست دسته‌بندی‌ها (Categories).
    /// Domain به این Interface نیاز دارد تا بتواند لیست دسته‌بندی‌های مجاز را بشناسد،
    /// بدون آنکه بداند این لیست از JSON می‌آید یا از دیتابیس.
    /// پیاده‌سازی واقعی در لایه‌ی Infrastructure انجام می‌شود.
    /// </summary>
    public interface ICategoryProvider
    {
        /// <summary> تمام دسته‌بندی‌های موجود را برمی‌گرداند </summary>
        IReadOnlyList<Category> GetAll();

        /// <summary> یک دسته‌بندی را بر اساس نام انگلیسی پیدا می‌کند. اگر پیدا نشود، خالی (نال) برگردانده می‌شود </summary>
        Category? GetByEnglishName(string englishName);

        /// <summary>
        /// بررسی می‌کند که آیا یک دسته‌بندی با این نام انگلیسی وجود دارد یا نه.
        /// </summary>
        bool Exists(string englishName);
    }
}
