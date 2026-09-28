using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AIChatSaver.Presentation.Services;

namespace AIChatSaver.Presentation.Forms
{
    /// <summary>
    /// فرم تنظیمات برنامه.
    /// در حال حاضر فقط روش نمایش HTML را می‌توان تغییر داد.
    /// </summary>
    public partial class SettingsForm : Form
    {
        private RadioButton _rbFileUrl = null!;
        private RadioButton _rbLoadHtml = null!;
        private Button _btnSave = null!;
        private Button _btnCancel = null!;

        public SettingsForm()
        {
            InitializeUI();
            LoadSettings();
        }

        private void InitializeUI()
        {
            Text = "تنظیمات";
            Width = 550;
            Height = 280;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            Font = new Font("Segoe UI", 10F);

            var lblHeader = new Label
            {
                Text = "روش نمایش فایل‌های HTML در پنجره‌ی نمایش:",
                Dock = DockStyle.Top,
                Height = 40,
                Padding = new Padding(15, 15, 15, 0),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleRight
            };

            _rbFileUrl = new RadioButton
            {
                Text = "بارگذاری مستقیم فایل (file:///) — سبک‌تر و سریع‌تر",
                Dock = DockStyle.Top,
                Height = 35,
                Padding = new Padding(30, 0, 15, 0),
                TextAlign = ContentAlignment.MiddleRight,
                Checked = true
            };

            _rbLoadHtml = new RadioButton
            {
                Text = "بارگذاری محتوا در حافظه (LoadHtml) — مناسب وقتی فایل جابه‌جا شود",
                Dock = DockStyle.Top,
                Height = 35,
                Padding = new Padding(30, 0, 15, 0),
                TextAlign = ContentAlignment.MiddleRight
            };

            var btnPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(15)
            };

            _btnSave = new Button
            {
                Text = "💾 ذخیره",
                Width = 110,
                Height = 34,
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _btnSave.Click += (s, e) => SaveSettings();

            _btnCancel = new Button
            {
                Text = "انصراف",
                Width = 90,
                Height = 34,
                BackColor = Color.FromArgb(158, 158, 158),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };

            btnPanel.Controls.Add(_btnSave);
            btnPanel.Controls.Add(_btnCancel);

            Controls.Add(btnPanel);
            Controls.Add(_rbLoadHtml);
            Controls.Add(_rbFileUrl);
            Controls.Add(lblHeader);
        }

        private void LoadSettings()
        {
            var s = AppServices.SettingsProvider.GetSettings();
            _rbLoadHtml.Checked = s.UseLoadHtmlInsteadOfFileUrl;
            _rbFileUrl.Checked = !s.UseLoadHtmlInsteadOfFileUrl;
        }

        private void SaveSettings()
        {
            var s = AppServices.SettingsProvider.GetSettings();
            s.UseLoadHtmlInsteadOfFileUrl = _rbLoadHtml.Checked;
            AppServices.SettingsProvider.SaveSettings(s);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
