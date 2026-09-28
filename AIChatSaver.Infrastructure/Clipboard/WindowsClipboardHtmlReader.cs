using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIChatSaver.UseCases.Interfaces;

namespace AIChatSaver.Infrastructure
{
    /// <summary>
    /// پیاده‌سازی IClipboardHtmlReader برای ویندوز.
    /// از Clipboard ویندوز، محتوای HTML خام را استخراج می‌کند.
    /// Clipboard ویندوز HTML را با هدرهای مخصوص ذخیره می‌کند (Version, StartHTML, EndHTML, ...).
    /// این کلاس آن هدرها را حذف کرده و HTML خالص را برمی‌گرداند.
    /// ⚠️ این کلاس باید روی ترد UI فراخوانی شود (چون Clipboard به WinForms نیاز دارد).
    /// </summary>
    public class WindowsClipboardHtmlReader : IClipboardHtmlReader
    {
        /// <summary>
        /// بررسی می‌کند که آیا Clipboard شامل فرمت HTML هست یا نه.
        /// </summary>
        public bool ContainsHtml()
        {
            try
            {
                return Clipboard.ContainsData(DataFormats.Html);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// محتوای HTML خام را از Clipboard می‌خواند و هدرهای اضافی را حذف می‌کند.
        /// خروجی: رشته‌ی HTML خالص، یا null در صورت نبود HTML.
        /// </summary>
        public string? ReadHtmlFromClipboard()
        {
            try
            {
                if (!Clipboard.ContainsData(DataFormats.Html))
                    return null;

                string? raw = Clipboard.GetData(DataFormats.Html) as string;
                if (string.IsNullOrWhiteSpace(raw))
                    return null;

                return ExtractHtmlBody(raw);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Clipboard ویندوز HTML را با هدرهای مخصوص ذخیره می‌کند:
        /// Version:0.9
        /// StartHTML:0000000105
        /// EndHTML:0000000200
        /// StartFragment:0000000140
        /// EndFragment:0000000180
        /// <html>...محتوا...</html>
        /// این متد با استفاده از StartHTML بخش اصلی HTML را استخراج می‌کند.
        /// </summary>
        private static string ExtractHtmlBody(string raw)
        {
            // پیدا کردن موقعیت شروع HTML واقعی
            int startMarker = raw.IndexOf("StartHTML:");
            if (startMarker < 0)
            {
                // اگر هدرها نبودند، کل متن را برمی‌گردانیم
                return raw;
            }

            try
            {
                // استخراج مقدار StartHTML
                int valueStart = startMarker + "StartHTML:".Length;
                int valueEnd = raw.IndexOfAny(new[] { '\r', '\n' }, valueStart);
                if (valueEnd < 0) return raw;

                string startHtmlStr = raw.Substring(valueStart, valueEnd - valueStart).Trim();
                if (!int.TryParse(startHtmlStr, out int htmlStart))
                    return raw;

                if (htmlStart >= 0 && htmlStart < raw.Length)
                    return raw.Substring(htmlStart);

                return raw;
            }
            catch
            {
                return raw;
            }
        }
    }
}
