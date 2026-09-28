using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIChatSaver.Domain.Enums;
using AIChatSaver.Domain.Interfaces;
using AIChatSaver.UseCases.Interfaces;

namespace AIChatSaver.UseCases.Services
{
    /// <summary>
    /// سرویس تغییر دسته‌بندی یک گفتگوی ذخیره‌شده.
    /// این عملیات شامل جابه‌جا کردن فایل HTML از پوشه‌ی قدیمی به پوشه‌ی جدید است.
    /// </summary>
    public class ChangeCategoryService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IHtmlFileStore _htmlFileStore;
        private readonly ICategoryProvider _categoryProvider;

        public ChangeCategoryService(
            IChatRepository chatRepository,
            IHtmlFileStore htmlFileStore,
            ICategoryProvider categoryProvider)
        {
            _chatRepository = chatRepository;
            _htmlFileStore = htmlFileStore;
            _categoryProvider = categoryProvider;
        }

        /// <summary>
        /// دسته‌بندی یک گفتگو را تغییر می‌دهد و فایل HTML را به پوشه‌ی جدید منتقل می‌کند.
        /// </summary>
        public OperationResult ChangeCategory(int id, string newCategoryEnglishName)
        {
            // اعتبارسنجی دسته‌بندی جدید
            if (!_categoryProvider.Exists(newCategoryEnglishName))
                return OperationResult.InvalidCategory;

            var chat = _chatRepository.GetById(id);
            if (chat == null)
                return OperationResult.FileNotFound;

            // اگر دسته‌بندی تغییر نکرده، کاری نکن
            if (string.Equals(chat.CategoryName, newCategoryEnglishName,
                System.StringComparison.OrdinalIgnoreCase))
            {
                return OperationResult.Success;
            }

            // خواندن محتوای فعلی از پوشه‌ی قدیمی
            string? htmlContent = _htmlFileStore.ReadHtml(chat.CategoryName, chat.FileName);
            if (htmlContent == null)
                return OperationResult.FileNotFound;

            // نوشتن در پوشه‌ی جدید
            _htmlFileStore.WriteHtml(newCategoryEnglishName, chat.FileName, htmlContent);

            // حذف از پوشه‌ی قدیمی
            _htmlFileStore.DeleteHtml(chat.CategoryName, chat.FileName);

            // به‌روزرسانی متادیتا
            chat.CategoryName = newCategoryEnglishName;
            chat.UpdatedAt = System.DateTime.Now;
            _chatRepository.Update(chat);

            return OperationResult.Success;
        }
    }
}
