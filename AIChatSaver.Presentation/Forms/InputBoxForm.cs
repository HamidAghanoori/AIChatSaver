using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AIChatSaver.Presentation.Forms
{
    /// <summary>
    /// یک فرم ساده‌ی ورودی که نقش InputBox را بازی می‌کند.
    /// برای گرفتن یک رشته‌ی کوتاه از کاربر (مثلاً عنوان جدید در Rename) استفاده می‌شود.
    /// </summary>
    public partial class InputBoxForm : Form
    {
        private TextBox _txtInput = null!;

        /// <summary>متن وارد شده توسط کاربر.</summary>
        public string InputText => _txtInput.Text;

        public InputBoxForm(string title, string prompt, string initialValue = "")
        {
            Text = title;
            Width = 450;
            Height = 180;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            Font = new Font("Segoe UI", 10F);

            var lblPrompt = new Label
            {
                Text = prompt,
                Dock = DockStyle.Top,
                Height = 40,
                Padding = new Padding(15, 15, 15, 0),
                TextAlign = ContentAlignment.MiddleRight
            };

            _txtInput = new TextBox
            {
                Dock = DockStyle.Top,
                Text = initialValue,
                Font = new Font("Segoe UI", 10F),
                Margin = new Padding(15)
            };

            var txtPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 45,
                Padding = new Padding(15, 0, 15, 0)
            };
            _txtInput.Dock = DockStyle.Fill;
            txtPanel.Controls.Add(_txtInput);

            var buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(15)
            };

            var btnOk = new Button
            {
                Text = "تأیید",
                Width = 90,
                Height = 32,
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.OK
            };
            btnOk.Click += (s, e) => { DialogResult = DialogResult.OK; Close(); };

            var btnCancel = new Button
            {
                Text = "انصراف",
                Width = 90,
                Height = 32,
                BackColor = Color.FromArgb(158, 158, 158),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };

            buttonPanel.Controls.Add(btnOk);
            buttonPanel.Controls.Add(btnCancel);

            Controls.Add(buttonPanel);
            Controls.Add(txtPanel);
            Controls.Add(lblPrompt);

            AcceptButton = btnOk;
            CancelButton = btnCancel;

            // انتخاب کامل متن اولیه
            Load += (s, e) => { _txtInput.SelectAll(); _txtInput.Focus(); };
        }
    }
}
