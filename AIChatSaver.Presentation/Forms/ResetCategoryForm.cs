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

namespace AIChatSaver.Presentation.Forms
{
    /// <summary>
    /// فرم انتخاب دسته‌بندی جدید برای یک گفتگو.
    /// فقط دسته‌های موجود قابل انتخاب هستند.
    /// </summary>
    public partial class ResetCategoryForm : Form
    {
        private ComboBox _cmbCategory = null!;

        /// <summary>نام انگلیسی دسته‌بندی انتخاب‌شده.</summary>
        public string SelectedCategoryEnglishName { get; private set; } = string.Empty;

        public ResetCategoryForm(string currentCategoryEnglishName)
        {
            InitializeUI();
            LoadCategories(currentCategoryEnglishName);
        }

        private void InitializeUI()
        {
            Text = "تغییر دسته‌بندی";
            Width = 500;
            Height = 200;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            Font = new Font("Segoe UI", 10F);

            var lblPrompt = new Label
            {
                Text = "دسته‌بندی جدید را انتخاب کنید:",
                Dock = DockStyle.Top,
                Height = 40,
                Padding = new Padding(15, 15, 15, 0),
                TextAlign = ContentAlignment.MiddleRight
            };

            _cmbCategory = new ComboBox
            {
                Dock = DockStyle.Top,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };

            var cmbPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                Padding = new Padding(15, 5, 15, 5)
            };
            _cmbCategory.Dock = DockStyle.Fill;
            cmbPanel.Controls.Add(_cmbCategory);

            var buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 55,
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
            btnOk.Click += (s, e) =>
            {
                if (_cmbCategory.SelectedItem is Category cat)
                    SelectedCategoryEnglishName = cat.EnglishName;
                DialogResult = DialogResult.OK;
                Close();
            };

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
            Controls.Add(cmbPanel);
            Controls.Add(lblPrompt);

            AcceptButton = btnOk;
            CancelButton = btnCancel;
        }

        private void LoadCategories(string currentCategory)
        {
            var categories = AppServices.CategoryProvider.GetAll().ToList();
            _cmbCategory.Items.Clear();
            foreach (var cat in categories)
                _cmbCategory.Items.Add(cat);

            _cmbCategory.DisplayMember = nameof(Category.DisplayText);

            var current = categories.FirstOrDefault(c =>
                string.Equals(c.EnglishName, currentCategory,
                    StringComparison.OrdinalIgnoreCase));

            if (current != null)
                _cmbCategory.SelectedItem = current;
            else if (_cmbCategory.Items.Count > 0)
                _cmbCategory.SelectedIndex = 0;
        }
    }
}
