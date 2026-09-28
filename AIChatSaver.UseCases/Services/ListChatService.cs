using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIChatSaver.Domain.Entities;
using AIChatSaver.Domain.Interfaces;

namespace AIChatSaver.UseCases.Services
{
    /// <summary>
    /// سرویس گرفتن لیست گفتگوهای ذخیره‌شده.
    /// این سرویس برای پر کردن DataGridView در صفحه‌ی اصلی استفاده می‌شود.
    /// </summary>
    public class ListChatService
    {
        private readonly IChatRepository _chatRepository;

        public ListChatService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        /// <summary>
        /// تمام گفتگوهای ذخیره‌شده را برمی‌گرداند.
        /// </summary>
        public IReadOnlyList<SavedChatInfo> GetAll()
        {
            return _chatRepository.GetAll();
        }

        /// <summary>
        /// یک گفتگو را بر اساس شناسه برمی‌گرداند.
        /// </summary>
        public SavedChatInfo? GetById(int id)
        {
            return _chatRepository.GetById(id);
        }
    }
}
