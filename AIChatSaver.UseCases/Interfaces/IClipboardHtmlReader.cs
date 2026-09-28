using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIChatSaver.UseCases.Interfaces
{
    /// <summary>
    /// قرارداد برای خواندن محتوای HTML از Clipboard ویندوز.
    /// چون Clipboard یک مفهوم سیستم‌عامل است و به Domain ربطی ندارد،
    /// این Interface در لایه‌ی Application تعریف می‌شود.
    /// پیاده‌سازی واقعی در لایه‌ی Infrastructure انجام می‌شود.
    /// </summary>
    public interface IClipboardHtmlReader
    {
        /// <summary>
        /// محتوای HTML خام را از Clipboard می‌خواند.
        /// اگر Clipboard شامل HTML نباشد، مقدار null برمی‌گرداند.
        /// </summary>
        string? ReadHtmlFromClipboard();

        /// <summary>
        /// بررسی می‌کند که آیا Clipboard در حال حاضر شامل محتوای HTML هست یا نه.
        /// </summary>
        bool ContainsHtml();
    }
}
