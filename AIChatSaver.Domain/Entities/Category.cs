using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIChatSaver.Domain.Entities
{
    /// <summary>
    /// یک دسته‌بندی (Category) را نگه می‌دارد.
    /// هر دسته شامل نام انگلیسی (برای نام پوشه) و توضیح فارسی (برای نمایش به کاربر) است.
    /// این کلاس هنگام خواندن فایل Categories.json ساخته می‌شود.
    /// </summary>
    public class Category
    {
        /// <summary> نام انگلیسی دسته. همین نام، اسم پوشه داخل SRCFolder </summary>
        public string EnglishName { get; set; } = string.Empty;

        /// <summary> توضیح فارسی دسته. برای نمایش به کاربر </summary>
        public string PersianDescription { get; set; } = string.Empty;

        /// <summary> متنی که در ComboBox نمایش داده می‌شود. </summary>
        public string DisplayText => $"{EnglishName} : {PersianDescription}";
    }
}
