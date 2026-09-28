using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIChatSaver.Domain.Entities
{
    /// <summary>
    /// نگهدارنده‌ی لیست دسته‌بندی‌ها.
    /// این کلاس هنگام خواندن یا نوشتن فایل Categories.json استفاده می‌شود.
    /// </summary>
    public class CategoryList
    {
        /// <summary>
        /// دیکشنری از نام انگلیسی دسته به توضیح فارسی آن.
        /// </summary>
        public Dictionary<string, string> Categories { get; set; } = new();
    }
}
