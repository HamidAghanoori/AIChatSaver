using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace AIChatSaver.Infrastructure.Helpers
{
    // <summary>
    /// تنظیمات مشترک سریالایز و دی‌سریالایز JSON را در یک جا نگه می‌دارد.
    /// از این کلاس برای یکدست بودن فرمت JSON در کل برنامه استفاده می‌شود.
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// تنظیمات پیش‌فرض System.Text.Json برای کل برنامه.
        /// - WriteIndented: خروجی خوانا با تودرتویی
        /// - Encoder: پشتیبانی از کاراکترهای فارسی بدون escape
        /// - PropertyNameCaseInsensitive: نام‌گذاری حساس به حروف بزرگ/کوچک نباشد
        /// </summary>
        public static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            DefaultIgnoreCondition = JsonIgnoreCondition.Never
        };
    }
}
