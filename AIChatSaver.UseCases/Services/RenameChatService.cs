using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIChatSaver.Domain.Enums;
using AIChatSaver.Domain.Interfaces;

namespace AIChatSaver.UseCases.Services
{
    /// <summary>
    /// سرویس تغییر عنوان یک گفتگوی ذخیره‌شده.
    /// فقط متادیتا را تغییر می‌دهد؛ نام فایل HTML دست‌نخورده می‌ماند.
    /// </summary>
    public class RenameChatService
    {
        private readonly IChatRepository _chatRepository;

        public RenameChatService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        /// <summary>
        /// عنوان یک گفتگو را تغییر می‌دهد.
        /// </summary>
        public OperationResult Rename(int id, string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
                return OperationResult.InvalidTitle;

            var chat = _chatRepository.GetById(id);
            if (chat == null)
                return OperationResult.FileNotFound;

            // بررسی تکراری نبودن عنوان جدید (به‌جز خودِ همین رکورد)
            if (_chatRepository.TitleExists(newTitle.Trim()) &&
                !string.Equals(chat.Title, newTitle.Trim(), System.StringComparison.OrdinalIgnoreCase))
            {
                return OperationResult.DuplicateTitle;
            }

            chat.Title = newTitle.Trim();
            chat.UpdatedAt = System.DateTime.Now;

            _chatRepository.Update(chat);
            return OperationResult.Success;
        }
    }
}
