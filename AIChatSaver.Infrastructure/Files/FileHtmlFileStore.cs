using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIChatSaver.UseCases.Interfaces;
using AIChatSaver.Infrastructure.Helpers;

namespace AIChatSaver.Infrastructure.Files
{
    /// <summary>
    /// پیاده‌سازی IHtmlFileStore روی فایل‌سیستم ویندوز.
    /// فایل‌های HTML داخل SRCFolder/<CategoryName>/ ذخیره می‌شوند.
    /// </summary>
    public class FileHtmlFileStore : IHtmlFileStore
    {
        public string WriteHtml(string categoryName, string fileName, string htmlContent)
        {
            AppPaths.EnsureCategoryFolderExists(categoryName);
            string fullPath = Path.Combine(
                AppPaths.GetCategoryFolder(categoryName), fileName);

            File.WriteAllText(fullPath, htmlContent, System.Text.Encoding.UTF8);
            return fullPath;
        }

        public string? ReadHtml(string categoryName, string fileName)
        {
            string fullPath = Path.Combine(
                AppPaths.GetCategoryFolder(categoryName), fileName);

            if (!File.Exists(fullPath))
                return null;

            try
            {
                return File.ReadAllText(fullPath, System.Text.Encoding.UTF8);
            }
            catch
            {
                return null;
            }
        }

        public bool Exists(string categoryName, string fileName)
        {
            string fullPath = Path.Combine(
                AppPaths.GetCategoryFolder(categoryName), fileName);
            return File.Exists(fullPath);
        }

        public bool DeleteHtml(string categoryName, string fileName)
        {
            string fullPath = Path.Combine(
                AppPaths.GetCategoryFolder(categoryName), fileName);

            if (!File.Exists(fullPath))
                return false;

            try
            {
                File.Delete(fullPath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetFullPath(string categoryName, string fileName)
        {
            return Path.Combine(AppPaths.GetCategoryFolder(categoryName), fileName);
        }
    }
}
