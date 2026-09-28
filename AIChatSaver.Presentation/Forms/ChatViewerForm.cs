using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AIChatSaver.Domain.Entities;
using AIChatSaver.Presentation.Services;
using CefSharp.WinForms;
using CefSharp;

namespace AIChatSaver.Presentation.Forms
{
    /// <summary>
    /// پنجره‌ی نمایش یک گفتگوی ذخیره‌شده.
    /// کلاینت CefSharp کل فضای فرم را پر می‌کند و محتوای HTML را نمایش می‌دهد.
    /// روش نمایش (file:/// یا LoadHtml) از تنظیمات خوانده می‌شود.
    /// </summary>
    public partial class ChatViewerForm : Form
    {
        private ChromiumWebBrowser _browser = null!;
        private readonly SavedChatInfo _chat;

        public ChatViewerForm(SavedChatInfo chat)
        {
            _chat = chat;
            InitializeUI();
            LoadHtmlContent();
        }

        private void InitializeUI()
        {
            Text = $"نمایش گفتگو: {_chat.Title}";
            Width = 1100;
            Height = 750;
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 10F);

            _browser = new ChromiumWebBrowser
            {
                Dock = DockStyle.Fill
            };

            Controls.Add(_browser);
        }

        /// <summary>
        /// محتوای HTML را بر اساس تنظیمات کاربر لود می‌کند.
        /// - اگر UseLoadHtmlInsteadOfFileUrl = true → محتوا از فایل خوانده و با LoadHtml نمایش داده می‌شود.
        /// - در غیر این صورت → فایل با file:/// لود می‌شود.
        /// </summary>
        private void LoadHtmlContent()
        {
            try
            {
                var settings = AppServices.SettingsProvider.GetSettings();
                string fullPath = AppServices.HtmlFileStore.GetFullPath(
                    _chat.CategoryName, _chat.FileName);

                if (settings.UseLoadHtmlInsteadOfFileUrl)
                {
                    string? html = AppServices.HtmlFileStore.ReadHtml(
                        _chat.CategoryName, _chat.FileName);

                    if (html == null)
                    {
                        ShowErrorPage("فایل HTML پیدا نشد.");
                        return;
                    }

                    _browser.LoadHtml(html, "https://aichatsaver.local/");
                }
                else
                {
                    if (!File.Exists(fullPath))
                    {
                        ShowErrorPage("فایل HTML روی دیسک پیدا نشد.");
                        return;
                    }

                    // file:/// نیاز به فرمت خاص دارد
                    string fileUrl = new Uri(fullPath).AbsoluteUri;
                    _browser.Load(fileUrl);
                }
            }
            catch (Exception ex)
            {
                ShowErrorPage("خطا در بارگذاری محتوا: " + ex.Message);
            }
        }

        /// <summary>
        /// در صورت بروز خطا، یک صفحه‌ی HTML ساده با پیام خطا نمایش می‌دهد.
        /// </summary>
        private void ShowErrorPage(string message)
        {
            string safeMessage = System.Net.WebUtility.HtmlEncode(message);
            string html = $@"
<!DOCTYPE html>
<html lang='fa' dir='rtl'>
<head>
    <meta charset='utf-8'>
    <title>خطا</title>
    <style>
        body {{
            font-family: Tahoma, Segoe UI, sans-serif;
            background: #fafafa;
            color: #c62828;
            display: flex;
            align-items: center;
            justify-content: center;
            height: 100vh;
            margin: 0;
        }}
        .box {{
            background: #fff;
            padding: 30px 40px;
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            text-align: center;
        }}
        h2 {{ margin: 0 0 10px 0; }}
        p {{ color: #555; margin: 0; }}
    </style>
</head>
<body>
    <div class='box'>
        <h2>⚠ خطا</h2>
        <p>{safeMessage}</p>
    </div>
</body>
</html>";
            _browser.LoadHtml(html, "https://aichatsaver.local/error");
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            // آزادسازی منابع CefSharp
            _browser?.Dispose();
        }
    }
}
