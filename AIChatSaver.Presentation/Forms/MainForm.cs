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
    /// صفحه‌ی اصلی برنامه.
    /// شامل یک DataGridView برای نمایش لیست گفتگوها و یک دکمه‌ی «+» برای افزودن.
    /// دکمه‌های عملیات (حذف / تغییر نام / تغییر دسته) بالای گرید قرار دارند
    /// و فقط وقتی ردیفی انتخاب شده باشد فعال می‌شوند.
    /// </summary>
    public partial class MainForm : Form
    {
        private DataGridView _grid = null!;
        private Button _btnAdd = null!;
        private Button _btnDelete = null!;
        private Button _btnRename = null!;
        private Button _btnChangeCategory = null!;
        private Button _btnSettings = null!;
        private Label _lblStatus = null!;

        public MainForm()
        {
            InitializeUI();
            LoadChats();
        }

        /// <summary>
        /// ساخت دستی کنترل‌های UI (بدون Designer).
        /// </summary>
        private void InitializeUI()
        {
            // تنظیمات فرم
            Text = "AIChatSaver - ذخیره‌ساز گفتگوها";
            Width = 1000;
            Height = 600;
            StartPosition = FormStartPosition.CenterScreen;
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            Font = new Font("Segoe UI", 10F);

            // نوار بالا: دکمه‌ها
            var topPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 50,
                Padding = new Padding(10),
                FlowDirection = FlowDirection.RightToLeft,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            _btnAdd = CreateToolbarButton("➕ افزودن", Color.FromArgb(76, 175, 80));
            _btnAdd.Click += (s, e) => OpenAddForm();

            _btnDelete = CreateToolbarButton("🗑 حذف", Color.FromArgb(244, 67, 54));
            _btnDelete.Click += (s, e) => DeleteSelected();
            _btnDelete.Enabled = false;

            _btnRename = CreateToolbarButton("✏ تغییر نام", Color.FromArgb(255, 152, 0));
            _btnRename.Click += (s, e) => RenameSelected();
            _btnRename.Enabled = false;

            _btnChangeCategory = CreateToolbarButton("📁 تغییر دسته", Color.FromArgb(33, 150, 243));
            _btnChangeCategory.Click += (s, e) => ChangeCategorySelected();
            _btnChangeCategory.Enabled = false;

            _btnSettings = CreateToolbarButton("⚙ تنظیمات", Color.FromArgb(96, 125, 139));
            _btnSettings.Click += (s, e) => OpenSettings();

            topPanel.Controls.Add(_btnAdd);
            topPanel.Controls.Add(_btnDelete);
            topPanel.Controls.Add(_btnRename);
            topPanel.Controls.Add(_btnChangeCategory);
            topPanel.Controls.Add(_btnSettings);

            // نوار پایین: وضعیت
            var bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 30,
                BackColor = Color.FromArgb(240, 240, 240)
            };
            _lblStatus = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight,
                Padding = new Padding(10, 0, 10, 0),
                Text = "آماده"
            };
            bottomPanel.Controls.Add(_lblStatus);

            // گرید اصلی
            _grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RightToLeft = RightToLeft.Yes,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(250, 250, 250)
                }
            };

            // ستون‌ها
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colId",
                HeaderText = "شناسه",
                FillWeight = 8
            });
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTitle",
                HeaderText = "عنوان",
                FillWeight = 40
            });
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCategory",
                HeaderText = "دسته‌بندی",
                FillWeight = 22
            });
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCreatedAt",
                HeaderText = "تاریخ ایجاد",
                FillWeight = 18
            });
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colFilePath",
                HeaderText = "مسیر فایل",
                FillWeight = 12
            });

            _grid.SelectionChanged += (s, e) => UpdateButtonsState();
            _grid.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0) OpenViewerForSelected();
            };

            // چیدمان
            Controls.Add(_grid);
            Controls.Add(topPanel);
            Controls.Add(bottomPanel);
        }

        private static Button CreateToolbarButton(string text, Color backColor)
        {
            return new Button
            {
                Text = text,
                Width = 130,
                Height = 34,
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Margin = new Padding(5, 0, 5, 0),
                Cursor = Cursors.Hand
            };
        }

        /// <summary>
        /// لیست گفتگوها را از سرویس می‌گیرد و در گرید نمایش می‌دهد.
        /// </summary>
        private void LoadChats()
        {
            try
            {
                _grid.Rows.Clear();
                var chats = AppServices.ListChatService.GetAll();
                var categories = AppServices.CategoryProvider.GetAll()
                    .ToDictionary(c => c.EnglishName, c => c.DisplayText,
                        StringComparer.OrdinalIgnoreCase);

                foreach (var chat in chats)
                {
                    string displayCategory = categories.TryGetValue(chat.CategoryName, out var disp)
                        ? disp
                        : chat.CategoryName;

                    string filePath = AppServices.HtmlFileStore.GetFullPath(
                        chat.CategoryName, chat.FileName);

                    int rowIndex = _grid.Rows.Add(
                        chat.Id,
                        chat.Title,
                        displayCategory,
                        chat.CreatedAt.ToString("yyyy/MM/dd HH:mm"),
                        filePath);

                    _grid.Rows[rowIndex].Tag = chat;
                }

                _lblStatus.Text = $"تعداد گفتگوها: {chats.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در بارگذاری لیست: " + ex.Message,
                    "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateButtonsState()
        {
            bool hasSelection = _grid.SelectedRows.Count > 0 &&
                                _grid.SelectedRows[0].Tag is SavedChatInfo;
            _btnDelete.Enabled = hasSelection;
            _btnRename.Enabled = hasSelection;
            _btnChangeCategory.Enabled = hasSelection;
        }

        private SavedChatInfo? GetSelectedChat()
        {
            if (_grid.SelectedRows.Count == 0) return null;
            return _grid.SelectedRows[0].Tag as SavedChatInfo;
        }

        private void OpenAddForm()
        {
            using var form = new AddChatForm();
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                LoadChats();
                _lblStatus.Text = "گفتگوی جدید ذخیره شد.";
            }
        }

        private void OpenViewerForSelected()
        {
            var chat = GetSelectedChat();
            if (chat == null) return;

            if (!AppServices.HtmlFileStore.Exists(chat.CategoryName, chat.FileName))
            {
                MessageBox.Show("فایل HTML این گفتگو پیدا نشد.",
                    "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var viewer = new ChatViewerForm(chat);
            viewer.ShowDialog(this);
        }

        private void DeleteSelected()
        {
            var chat = GetSelectedChat();
            if (chat == null) return;

            var result = MessageBox.Show(
                $"آیا از حذف «{chat.Title}» مطمئن هستید؟",
                "تأیید حذف",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result != DialogResult.Yes) return;

            var opResult = AppServices.DeleteChatService.Delete(chat.Id);
            if (opResult == OperationResult.Success)
            {
                LoadChats();
                _lblStatus.Text = "گفتگو حذف شد.";
            }
            else
            {
                MessageBox.Show("خطا در حذف گفتگو: " + opResult,
                    "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RenameSelected()
        {
            var chat = GetSelectedChat();
            if (chat == null) return;

            using var input = new InputBoxForm("تغییر عنوان", "عنوان جدید:", chat.Title);
            if (input.ShowDialog(this) != DialogResult.OK) return;

            var opResult = AppServices.RenameChatService.Rename(chat.Id, input.InputText);
            switch (opResult)
            {
                case OperationResult.Success:
                    LoadChats();
                    _lblStatus.Text = "عنوان تغییر کرد.";
                    break;
                case OperationResult.InvalidTitle:
                    MessageBox.Show("عنوان نمی‌تواند خالی باشد.",
                        "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case OperationResult.DuplicateTitle:
                    MessageBox.Show("این عنوان قبلاً استفاده شده است.",
                        "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                default:
                    MessageBox.Show("خطای ناشناخته: " + opResult,
                        "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }

        private void ChangeCategorySelected()
        {
            var chat = GetSelectedChat();
            if (chat == null) return;

            using var form = new ResetCategoryForm(chat.CategoryName);
            if (form.ShowDialog(this) != DialogResult.OK) return;

            var opResult = AppServices.ChangeCategoryService.ChangeCategory(
                chat.Id, form.SelectedCategoryEnglishName);

            if (opResult == OperationResult.Success)
            {
                LoadChats();
                _lblStatus.Text = "دسته‌بندی تغییر کرد.";
            }
            else
            {
                MessageBox.Show("خطا در تغییر دسته‌بندی: " + opResult,
                    "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenSettings()
        {
            using var form = new SettingsForm();
            form.ShowDialog(this);
        }
    }
}
