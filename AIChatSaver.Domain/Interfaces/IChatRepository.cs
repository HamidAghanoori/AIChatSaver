using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIChatSaver.Domain.Entities;

namespace AIChatSaver.Domain.Interfaces
{
    /// <summary>
    /// قرارداد ذخیره و بازیابی اطلاعات گفتگوها.
    /// این اینترفیس در لایه‌ی دامین تعریف می‌شود تا دامین بداند «به یک منبع داده نیاز دارد»
    /// ولی نداند که این منبع، جیسون است یا اس کیو ال، یا هر چیز دیگر و
    /// پیاده‌سازی واقعی این اینترفیس در لایه‌ی اینفراستراکچر انجام می‌شود
    /// </summary>
    public interface IChatRepository
    {
        /// <summary> تمام گفتگوهای ذخیره‌شده را برمی‌گرداند </summary>
        IReadOnlyList<SavedChatInfo> GetAll();

        /// <summary>
        /// یک گفتگو را بر اساس شناسه‌ی عددی پیدا می‌کند.
        /// اگر پیدا نشود، مقدار null برگردانده می‌شود.
        /// </summary>
        SavedChatInfo? GetById(int id);

        /// <summary>
        /// یک گفتگوی جدید را اضافه می‌کند و شناسه‌ی اختصاص‌یافته را برمی‌گرداند.
        /// </summary>
        SavedChatInfo Add(SavedChatInfo newChat);

        /// <summary>
        /// اطلاعات یک گفتگوی موجود را به‌روزرسانی می‌کند (عنوان، دسته‌بندی و ...).
        /// </summary>
        void Update(SavedChatInfo chat);

        /// <summary>
        /// یک گفتگو را بر اساس شناسه حذف می‌کند.
        /// خروجی: true اگر حذف موفق بود، در غیر این صورت false.
        /// </summary>
        bool Delete(int id);

        /// <summary>
        /// بررسی می‌کند که آیا عنوانی تکراری وجود دارد یا نه.
        /// از این متد برای جلوگیری از ثبت عنوان تکراری استفاده می‌شود.
        /// </summary>
        bool TitleExists(string title);

        /// <summary>
        /// شناسه‌ی بعدی که به گفتگوی جدید اختصاص داده می‌شود را برمی‌گرداند.
        /// </summary>
        int GetNextId();
    }
}
