using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIChatSaver.Domain.Interfaces;
using AIChatSaver.Infrastructure;
using AIChatSaver.Infrastructure.Files;
using AIChatSaver.UseCases.Interfaces;
using AIChatSaver.UseCases.Services;

namespace AIChatSaver.Presentation.Services
{
    /// <summary>
    /// نقطه‌ی مرکزی برای ساخت و دسترسی به همه‌ی سرویس‌ها.
    /// این کلاس نقش «Dependency Injection دستی» را بازی می‌کند.
    /// هر فرم می‌تواند از AppServices برای گرفتن سرویس مورد نیازش استفاده کند.
    /// همه‌ی سرویس‌ها به‌صورت Lazy و Singleton ساخته می‌شوند.
    /// </summary>
    public static class AppServices
    {
        // پیاده‌سازی‌های Infrastructure
        private static IChatRepository? _chatRepository;
        private static ICategoryProvider? _categoryProvider;
        private static IHtmlFileStore? _htmlFileStore;
        private static IClipboardHtmlReader? _clipboardHtmlReader;
        private static ISettingsProvider? _settingsProvider;

        // سرویس‌های Application
        private static SaveChatService? _saveChatService;
        private static ListChatService? _listChatService;
        private static DeleteChatService? _deleteChatService;
        private static RenameChatService? _renameChatService;
        private static ChangeCategoryService? _changeCategoryService;

        /// <summary>مخزن گفتگوها (meta.json).</summary>
        public static IChatRepository ChatRepository =>
            _chatRepository ??= new JsonChatRepository();

        /// <summary>تأمین‌کننده‌ی دسته‌بندی‌ها (Categories.json).</summary>
        public static ICategoryProvider CategoryProvider =>
            _categoryProvider ??= new JsonCategoryProvider();

        /// <summary>مدیریت فایل‌های HTML.</summary>
        public static IHtmlFileStore HtmlFileStore =>
            _htmlFileStore ??= new FileHtmlFileStore();

        /// <summary>خواننده‌ی HTML از Clipboard ویندوز.</summary>
        public static IClipboardHtmlReader ClipboardHtmlReader =>
            _clipboardHtmlReader ??= new WindowsClipboardHtmlReader();

        /// <summary>تأمین‌کننده‌ی تنظیمات (settings.json).</summary>
        public static ISettingsProvider SettingsProvider =>
            _settingsProvider ??= new JsonSettingsProvider();

        /// <summary>سرویس ذخیره‌ی گفتگوی جدید.</summary>
        public static SaveChatService SaveChatService =>
            _saveChatService ??= new SaveChatService(
                ChatRepository, HtmlFileStore, CategoryProvider);

        /// <summary>سرویس گرفتن لیست گفتگوها.</summary>
        public static ListChatService ListChatService =>
            _listChatService ??= new ListChatService(ChatRepository);

        /// <summary>سرویس حذف گفتگو.</summary>
        public static DeleteChatService DeleteChatService =>
            _deleteChatService ??= new DeleteChatService(ChatRepository, HtmlFileStore);

        /// <summary>سرویس تغییر عنوان.</summary>
        public static RenameChatService RenameChatService =>
            _renameChatService ??= new RenameChatService(ChatRepository);

        /// <summary>سرویس تغییر دسته‌بندی.</summary>
        public static ChangeCategoryService ChangeCategoryService =>
            _changeCategoryService ??= new ChangeCategoryService(
                ChatRepository, HtmlFileStore, CategoryProvider);
    }
}
