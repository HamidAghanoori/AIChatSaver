using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIChatSaver.Domain.Entities
{
    /// <summary>
    /// تنظیمات کلی برنامه را نگه می‌دارد.
    /// این تنظیمات در فایل settings.json داخل SRCFolder ذخیره می‌شود.
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Method for displaying a HTML file in the ChatViewerForm window.
        /// If true: LoadHtml is used (content is loaded into memory).
        /// If false: file:/// is used (the file is loaded directly from disk).
        /// </summary>
        public bool UseLoadHtmlInsteadOfFileUrl { get; set; } = false;

        /// <summary>
        /// The default category selected in the ComboBox of the Add form.
        /// Default value: "programing Learning"
        /// </summary>
        public string DefaultCategoryName { get; set; } = "programing Learning";
    }
}
