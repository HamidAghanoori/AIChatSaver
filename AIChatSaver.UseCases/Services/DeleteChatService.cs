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
    /// سرویس حذف یک گفتگوی ذخیره‌شده.
    /// هم فایل HTML و هم رکورد متادیتا را حذف می‌کند.
    /// </summary>
    public class DeleteChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IHtmlFileStore _htmlFileStore;

        public DeleteChatService(
            IChatRepository chatRepository,
            IHtmlFileStore htmlFileStore)
        {
            _chatRepository = chatRepository;
            _htmlFileStore = htmlFileStore;
        }

        /// <summary>
        /// یک گفتگو را بر اساس شناسه حذف می‌کند.
        /// </summary>
        public OperationResult Delete(int id)
        {
            var chat = _chatRepository.GetById(id);
            if (chat == null)
                return OperationResult.FileNotFound;

            // حذف فایل HTML (اگر وجود داشت)
            _htmlFileStore.DeleteHtml(chat.CategoryName, chat.FileName);

            // حذف رکورد متادیتا
            bool removed = _chatRepository.Delete(id);

            return removed ? OperationResult.Success : OperationResult.UnknownError;
        }
    }
}
