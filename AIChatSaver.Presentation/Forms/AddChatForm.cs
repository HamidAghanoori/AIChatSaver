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
using AIChatSaver.Domain.Enums;
using AIChatSaver.Presentation.Services;

namespace AIChatSaver.Presentation.Forms
{
    /// <summary>
    /// فرم افزودن گفتگوی جدید.
    /// شامل:
    ///   - TextBox عنوان
    ///   - ComboBox دسته‌بندی (فقط از دسته‌های موجود)
    ///   - RichTextBox برای پیست کردن محتوای HTML
    ///   - دکمه‌ی «Paste from Clipboard» برای خواندن سریع HTML خام
    /// اعتبارسنجی: عنوان و محتوا و دسته اجباری هستند.
    /// </summary>
    public partial class AddChatForm : Form
    {
        private TextBox _txtTitle = null!;
        private ComboBox _cmbCategory = null!;
        private RichTextBox _rtbContent = null!;
        private Button _btnPaste = null!;
        private Button _btnSave = null!;
        private Button _btnCancel = null!;
        private Label _lblStatus = null!;

        public AddChatForm()
        {
            InitializeUI();
            LoadCategories();
        }

        private void InitializeUI()
        {
            Text = "افزودن گفتگوی جدید";
            Width = 850;
            Height = 650;
            StartPosition = FormStartPosition.CenterParent;
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            Font = new Font("Segoe UI", 10F);

            // پنل بالا: عنوان و دسته
            var topPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 100,
                ColumnCount = 2,
                RowCount = 2,
                Padding = new Padding(15),
                RightToLeft = RightToLeft.Yes
            };
            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            topPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            topPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));

            var lblTitle = new Label
            {
                Text = "عنوان:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight
            };
            _txtTitle = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F)
            };

            var lblCategory = new Label
            {
                Text = "دسته‌بندی:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight
            };
            _cmbCategory = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };

            topPanel.Controls.Add(lblTitle, 0, 0);
            topPanel.Controls.Add(_txtTitle, 1, 0);
            topPanel.Controls.Add(lblCategory, 0, 1);
            topPanel.Controls.Add(_cmbCategory, 1, 1);

            // پنل میانی: محتوا
            var midPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(15, 0, 15, 0)
            };

            var lblContent = new Label
            {
                Text = "محتوای HTML (از Clipboard پیست کنید):",
                Dock = DockStyle.Top,
                Height = 25,
                TextAlign = ContentAlignment.MiddleRight
            };

            _rtbContent = new RichTextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 9.5F),
                WordWrap = false,
                ScrollBars = RichTextBoxScrollBars.Both,
                AcceptsTab = true,
                RightToLeft = RightToLeft.No // محتوای HTML چپ‌به‌راست
            };

            // پشتیبانی از Ctrl+V استاندارد + امکان پیست HTML خام
            _rtbContent.KeyDown += (s, e) =>
            {
                if (e.Control && e.KeyCode == Keys.V)
                {
                    // اجازه بده پیست معمولی انجام شود (متن ساده)
                    // برای پیست HTML خام، کاربر باید از دکمه استفاده کند.
                }
            };

            midPanel.Controls.Add(_rtbContent);
            midPanel.Controls.Add(lblContent);

            // پنل پایین: دکمه‌ها + وضعیت
            var bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 90,
                Padding = new Padding(15)
            };

            var buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 40,
                FlowDirection = FlowDirection.RightToLeft
            };

            _btnPaste = new Button
            {
                Text = "📋 Paste HTML از Clipboard",
                Width = 220,
                Height = 32,
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };
            _btnPaste.Click += (s, e) => PasteHtmlFromClipboard();

            _btnSave = new Button
            {
                Text = "💾 ذخیره",
                Width = 110,
                Height = 32,
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };
            _btnSave.Click += (s, e) => SaveChat();

            _btnCancel = new Button
            {
                Text = "انصراف",
                Width = 90,
                Height = 32,
                BackColor = Color.FromArgb(158, 158, 158),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };
            _btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            buttonPanel.Controls.Add(_btnPaste);
            buttonPanel.Controls.Add(_btnSave);
            buttonPanel.Controls.Add(_btnCancel);

            _lblStatus = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight,
                ForeColor = Color.FromArgb(120, 120, 120)
            };

            bottomPanel.Controls.Add(_lblStatus);
            bottomPanel.Controls.Add(buttonPanel);

            // چیدمان کلی
            Controls.Add(midPanel);
            Controls.Add(bottomPanel);
            Controls.Add(topPanel);
        }

        /// <summary>
        /// دسته‌بندی‌های موجود را در ComboBox بارگذاری می‌کند
        /// و مقدار پیش‌فرض را انتخاب می‌کند.
        /// </summary>
        private void LoadCategories()
        {
            var categories = AppServices.CategoryProvider.GetAll().ToList();
            _cmbCategory.Items.Clear();
            foreach (var cat in categories)
                _cmbCategory.Items.Add(cat);

            _cmbCategory.DisplayMember = nameof(Category.DisplayText);

            // انتخاب دسته‌ی پیش‌فرض از تنظیمات
            var settings = AppServices.SettingsProvider.GetSettings();
            var defaultCat = categories.FirstOrDefault(c =>
                string.Equals(c.EnglishName, settings.DefaultCategoryName,
                    StringComparison.OrdinalIgnoreCase));

            if (defaultCat != null)
                _cmbCategory.SelectedItem = defaultCat;
            else if (_cmbCategory.Items.Count > 0)
                _cmbCategory.SelectedIndex = 0;
        }

        /// <summary>
        /// محتوای HTML خام را از Clipboard می‌خواند و در RichTextBox می‌گذارد.
        /// </summary>
        private void PasteHtmlFromClipboard()
        {
            try
            {
                if (!AppServices.ClipboardHtmlReader.ContainsHtml())
                {
                    _lblStatus.Text = "Clipboard شامل HTML نیست.";
                    _lblStatus.ForeColor = Color.OrangeRed;
                    return;
                }

                string? html = AppServices.ClipboardHtmlReader.ReadHtmlFromClipboard();
                if (string.IsNullOrWhiteSpace(html))
                {
                    _lblStatus.Text = "محتوای HTML از Clipboard خالی بود.";
                    _lblStatus.ForeColor = Color.OrangeRed;
                    return;
                }

                _rtbContent.Text = html;
                _lblStatus.Text = $"HTML از Clipboard خوانده شد ({html.Length} کاراکتر).";
                _lblStatus.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                _lblStatus.Text = "خطا در خواندن Clipboard: " + ex.Message;
                _lblStatus.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// اعتبارسنجی و ذخیره‌ی گفتگو.
        /// </summary>
        private void SaveChat()
        {
            // اعتبارسنجی عنوان
            if (string.IsNullOrWhiteSpace(_txtTitle.Text))
            {
                MessageBox.Show("عنوان نمی‌تواند خالی باشد.",
                    "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtTitle.Focus();
                return;
            }

            // اعتبارسنجی محتوا
            if (string.IsNullOrWhiteSpace(_rtbContent.Text))
            {
                MessageBox.Show("محتوای HTML نمی‌تواند خالی باشد.",
                    "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _rtbContent.Focus();
                return;
            }

            // اعتبارسنجی دسته
            if (_cmbCategory.SelectedItem is not Category selectedCat)
            {
                MessageBox.Show("لطفاً یک دسته‌بندی انتخاب کنید.",
                    "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // فراخوانی سرویس
            var (result, _) = AppServices.SaveChatService.Save(
                _txtTitle.Text,
                selectedCat.EnglishName,
                _rtbContent.Text);

            switch (result)
            {
                case OperationResult.Success:
                    DialogResult = DialogResult.OK;
                    Close();
                    break;
                case OperationResult.DuplicateTitle:
                    MessageBox.Show("این عنوان قبلاً استفاده شده است. لطفاً عنوان دیگری انتخاب کنید.",
                        "عنوان تکراری", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _txtTitle.Focus();
                    _txtTitle.SelectAll();
                    break;
                case OperationResult.InvalidCategory:
                    MessageBox.Show("دسته‌بندی انتخابی معتبر نیست.",
                        "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case OperationResult.EmptyHtmlContent:
                    MessageBox.Show("محتوای HTML نمی‌تواند خالی باشد.",
                        "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                default:
                    MessageBox.Show("خطای ناشناخته در ذخیره‌سازی: " + result,
                        "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }
    }
}
