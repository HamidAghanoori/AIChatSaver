using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIChatSaver.Domain.Entities;
using AIChatSaver.Domain.Enums;
using AIChatSaver.Domain.Interfaces;
using AIChatSaver.UseCases.Interfaces;

namespace AIChatSaver.UseCases.Services
{
    /// <summary>
    /// سرویس ذخیره‌ی یک گفتگوی جدید.
    /// این سرویس مسئول مراحل زیر است:
    ///   1) اعتبارسنجی عنوان و محتوا
    ///   2) بررسی تکراری نبودن عنوان
    ///   3) گرفتن شناسه‌ی جدید
    ///   4) نوشتن فایل HTML در پوشه‌ی مربوطه
    ///   5) ثبت متادیتا در فایل JSON
    /// خروجی: نتیجه‌ی عملیات + اطلاعات گفتگوی ذخیره‌شده (در صورت موفقیت).
    /// </summary>
    public class SaveChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IHtmlFileStore _htmlFileStore;
        private readonly ICategoryProvider _categoryProvider;

        public SaveChatService(
            IChatRepository chatRepository,
            IHtmlFileStore htmlFileStore,
            ICategoryProvider categoryProvider)
        {
            _chatRepository = chatRepository;
            _htmlFileStore = htmlFileStore;
            _categoryProvider = categoryProvider;
        }

        /// <summary>
        /// یک گفتگوی جدید را ذخیره می‌کند.
        /// </summary>
        /// <param name="title">عنوان گفتگو (اجباری، نباید خالی باشد، نباید تکراری باشد)</param>
        /// <param name="categoryEnglishName">نام انگلیسی دسته‌بندی (اجباری)</param>
        /// <param name="htmlContent">محتوای HTML خام (اجباری)</param>
        /// <returns>نتیجه‌ی عملیات و در صورت موفقیت، اطلاعات گفتگوی ذخیره‌شده</returns>
        public (OperationResult Result, SavedChatInfo? SavedChat) Save(
            string title,
            string categoryEnglishName,
            string htmlContent)
        {
            // اعتبارسنجی عنوان
            if (string.IsNullOrWhiteSpace(title))
                return (OperationResult.InvalidTitle, null);

            // اعتبارسنجی محتوا
            if (string.IsNullOrWhiteSpace(htmlContent))
                return (OperationResult.EmptyHtmlContent, null);

            // اعتبارسنجی دسته‌بندی
            if (!_categoryProvider.Exists(categoryEnglishName))
                return (OperationResult.InvalidCategory, null);

            // بررسی تکراری نبودن عنوان
            if (_chatRepository.TitleExists(title.Trim()))
                return (OperationResult.DuplicateTitle, null);

            // گرفتن شناسه‌ی جدید
            int newId = _chatRepository.GetNextId();
            string fileName = $"{newId:D4}.html";

            // نوشتن فایل HTML
            try
            {
                _htmlFileStore.WriteHtml(categoryEnglishName, fileName, htmlContent);
            }
            catch
            {
                return (OperationResult.UnknownError, null);
            }

            // ساخت رکورد متادیتا
            var now = DateTime.Now;
            var newChat = new SavedChatInfo
            {
                Id = newId,
                Title = title.Trim(),
                CategoryName = categoryEnglishName,
                FileName = fileName,
                CreatedAt = now,
                UpdatedAt = now
            };

            // ثبت در مخزن
            _chatRepository.Add(newChat);

            return (OperationResult.Success, newChat);
        }
    }
}
