using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AIChatSaver.Domain.Entities;
using AIChatSaver.Domain.Interfaces;
using AIChatSaver.Infrastructure.Helpers;

namespace AIChatSaver.Infrastructure.Files
{
    /// <summary>
    /// پیاده‌سازی IChatRepository با استفاده از فایل JSON.
    /// همه‌ی گفتگوها در یک فایل meta.json داخل SRCFolder ذخیره می‌شوند.
    /// ساختار فایل:
    /// {
    ///   "nextId": 5,
    ///   "items": [ {...}, {...} ]
    /// }
    /// این کلاس به‌عنوان «دیتابیس سبک» برنامه عمل می‌کند.
    /// </summary>
    public class JsonChatRepository : IChatRepository
    {
        /// <summary>
        /// ساختار داخلی فایل meta.json.
        /// </summary>
        private class MetaFileFormat
        {
            public int NextId { get; set; } = 1;
            public List<SavedChatInfo> Items { get; set; } = new();
        }

        private MetaFileFormat _cache;

        public JsonChatRepository()
        {
            _cache = LoadFromDisk();
        }

        /// <summary>
        /// فایل meta.json را از دیسک می‌خواند.
        /// اگر فایل وجود نداشته باشد، یک ساختار خالی برمی‌گرداند.
        /// </summary>
        private MetaFileFormat LoadFromDisk()
        {
            AppPaths.EnsureSrcFolderExists();

            if (!File.Exists(AppPaths.MetaJsonPath))
                return new MetaFileFormat();

            try
            {
                string json = File.ReadAllText(AppPaths.MetaJsonPath);
                var data = JsonSerializer.Deserialize<MetaFileFormat>(json, JsonHelper.Options);
                return data ?? new MetaFileFormat();
            }
            catch
            {
                // در صورت خرابی فایل، یک ساختار خالی برمی‌گردانیم تا برنامه کرش نکند.
                return new MetaFileFormat();
            }
        }

        /// <summary>
        /// تغییرات فعلی را روی فایل meta.json ذخیره می‌کند.
        /// </summary>
        private void SaveToDisk()
        {
            AppPaths.EnsureSrcFolderExists();
            string json = JsonSerializer.Serialize(_cache, JsonHelper.Options);
            File.WriteAllText(AppPaths.MetaJsonPath, json);
        }

        public IReadOnlyList<SavedChatInfo> GetAll()
        {
            return _cache.Items
                .OrderByDescending(x => x.CreatedAt)
                .ToList();
        }

        public SavedChatInfo? GetById(int id)
        {
            return _cache.Items.FirstOrDefault(x => x.Id == id);
        }

        public SavedChatInfo Add(SavedChatInfo newChat)
        {
            _cache.Items.Add(newChat);
            if (newChat.Id >= _cache.NextId)
                _cache.NextId = newChat.Id + 1;
            SaveToDisk();
            return newChat;
        }

        public void Update(SavedChatInfo chat)
        {
            var existing = _cache.Items.FirstOrDefault(x => x.Id == chat.Id);
            if (existing == null) return;

            existing.Title = chat.Title;
            existing.CategoryName = chat.CategoryName;
            existing.FileName = chat.FileName;
            existing.UpdatedAt = chat.UpdatedAt;

            SaveToDisk();
        }

        public bool Delete(int id)
        {
            var existing = _cache.Items.FirstOrDefault(x => x.Id == id);
            if (existing == null) return false;

            _cache.Items.Remove(existing);
            SaveToDisk();
            return true;
        }

        public bool TitleExists(string title)
        {
            return _cache.Items.Any(x =>
                string.Equals(x.Title, title, StringComparison.OrdinalIgnoreCase));
        }

        public int GetNextId()
        {
            return _cache.NextId;
        }
    }
}
