using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIChatSaver.Domain.Enums
{
    /// <summary>
    /// نتیجه‌ی یک عملیات (ذخیره، حذف، تغییر نام و ...) را مشخص می‌کند.
    /// این Enum به جای برگرداندن bool ساده استفاده می‌شود تا بتوانیم دلیل خطا را هم بفهمیم.
    /// </summary>
    public enum OperationResult
    {

        /// <summary>عملیات با موفقیت انجام شد.</summary>
        Success = 0,

        /// <summary>عنوان وارد شده تکراری است.</summary>
        DuplicateTitle = 1,

        /// <summary>عنوان خالی یا نامعتبر است.</summary>
        InvalidTitle = 2,

        /// <summary>دسته‌بندی نامعتبر یا پیدا نشد.</summary>
        InvalidCategory = 3,

        /// <summary>متن HTML خالی است.</summary>
        EmptyHtmlContent = 4,

        /// <summary>فایل مورد نظر پیدا نشد.</summary>
        FileNotFound = 5,

        /// <summary>خطای ناشناخته در حین عملیات.</summary>
        UnknownError = 99
    }
}
