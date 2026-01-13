using System.Drawing.Drawing2D;
using System.Text.Json;

namespace Rufty
{
    public class ListItem
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsCompleted { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }

    public class ModernButton : Button
    {
        public Color HoverColor { get; set; }
        public Color NormalColor { get; set; }
        private bool isHovered = false;

        public ModernButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Cursor = Cursors.Hand;
            Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            BackColor = NormalColor;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            isHovered = true;
            BackColor = HoverColor;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            isHovered = false;
            BackColor = NormalColor;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = GetRoundedRectangle(ClientRectangle, 10))
            {
                this.Region = new Region(path);
                using (SolidBrush brush = new SolidBrush(BackColor))
                {
                    e.Graphics.FillPath(brush, path);
                }
            }

            TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private GraphicsPath GetRoundedRectangle(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, radius, radius, 180, 90);
            path.AddArc(bounds.Right - radius, bounds.Y, radius, radius, 270, 90);
            path.AddArc(bounds.Right - radius, bounds.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    public class RoundedPanel : Panel
    {
        public int BorderRadius { get; set; } = 15;
        public Color BorderColor { get; set; } = Color.FromArgb(220, 220, 225);

        public RoundedPanel()
        {
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = GetRoundedRectangle(ClientRectangle, BorderRadius))
            {
                this.Region = new Region(path);

                // Glass effect - semi-transparent white overlay
                using (LinearGradientBrush glassBrush = new LinearGradientBrush(
                    ClientRectangle,
                    Color.FromArgb(40, 255, 255, 255),
                    Color.FromArgb(10, 255, 255, 255),
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillPath(glassBrush, path);
                }

                using (Pen pen = new Pen(BorderColor, 1))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        private GraphicsPath GetRoundedRectangle(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            if (radius <= 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            bounds.Width -= 1;
            bounds.Height -= 1;

            path.AddArc(bounds.X, bounds.Y, radius, radius, 180, 90);
            path.AddArc(bounds.Right - radius, bounds.Y, radius, radius, 270, 90);
            path.AddArc(bounds.Right - radius, bounds.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    public class GlassTextBox : Panel
    {
        private TextBox textBox;
        public int BorderRadius { get; set; } = 10;

        public GlassTextBox()
        {
            DoubleBuffered = true;
            textBox = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(250, 250, 252),
                Location = new Point(12, 8)
            };

            this.Controls.Add(textBox);
            this.Height = 40;
            this.BackColor = Color.FromArgb(250, 250, 252);
        }

        public override string Text
        {
            get { return textBox.Text; }
            set { textBox.Text = value; }
        }

        public new Font Font
        {
            get { return textBox.Font; }
            set { textBox.Font = value; }
        }

        public bool Multiline
        {
            get { return textBox.Multiline; }
            set
            {
                textBox.Multiline = value;
                if (value)
                {
                    textBox.ScrollBars = ScrollBars.Vertical;
                }
            }
        }

        public void Clear()
        {
            textBox.Clear();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            textBox.Size = new Size(this.Width - 24, this.Height - 16);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = GetRoundedRectangle(ClientRectangle, BorderRadius))
            {
                this.Region = new Region(path);

                using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(250, 250, 252)))
                {
                    e.Graphics.FillPath(bgBrush, path);
                }

                // Glass effect
                using (LinearGradientBrush glassBrush = new LinearGradientBrush(
                    ClientRectangle,
                    Color.FromArgb(60, 255, 255, 255),
                    Color.FromArgb(20, 255, 255, 255),
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillPath(glassBrush, path);
                }

                using (Pen pen = new Pen(Color.FromArgb(220, 220, 225), 1))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        private GraphicsPath GetRoundedRectangle(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            bounds.Width -= 1;
            bounds.Height -= 1;

            path.AddArc(bounds.X, bounds.Y, radius, radius, 180, 90);
            path.AddArc(bounds.Right - radius, bounds.Y, radius, radius, 270, 90);
            path.AddArc(bounds.Right - radius, bounds.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    public class ModernListBox : ListBox
    {
        public event EventHandler<int> CheckboxClicked;

        public ModernListBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            ItemHeight = 60;
            BorderStyle = BorderStyle.None;
            Font = new Font("Segoe UI", 10F);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            int index = IndexFromPoint(e.Location);
            if (index != -1)
            {
                Rectangle itemBounds = GetItemRectangle(index);
                Rectangle checkBoxBounds = new Rectangle(itemBounds.X + 15, itemBounds.Y + 20, 20, 20);

                if (checkBoxBounds.Contains(e.Location))
                {
                    CheckboxClicked?.Invoke(this, index);
                }
            }
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            Rectangle itemBounds = new Rectangle(e.Bounds.X + 5, e.Bounds.Y + 3, e.Bounds.Width - 10, e.Bounds.Height - 6);

            using (GraphicsPath path = GetRoundedRectangle(itemBounds, 12))
            {
                if (isSelected)
                {
                    using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(240, 240, 248)))
                    {
                        e.Graphics.FillPath(bgBrush, path);
                    }

                    // Glass effect for selected item
                    using (LinearGradientBrush glassBrush = new LinearGradientBrush(
                        itemBounds,
                        Color.FromArgb(50, 255, 255, 255),
                        Color.FromArgb(10, 255, 255, 255),
                        LinearGradientMode.Vertical))
                    {
                        e.Graphics.FillPath(glassBrush, path);
                    }

                    using (Pen borderPen = new Pen(Color.FromArgb(0, 122, 255), 2))
                    {
                        e.Graphics.DrawPath(borderPen, path);
                    }
                }
                else
                {
                    using (SolidBrush bgBrush = new SolidBrush(Color.White))
                    {
                        e.Graphics.FillPath(bgBrush, path);
                    }
                }
            }

            ListItem item = Items[e.Index] as ListItem;
            if (item != null)
            {
                // Draw checkbox
                Rectangle checkBoxBounds = new Rectangle(itemBounds.X + 15, itemBounds.Y + 20, 20, 20);
                using (Pen checkPen = new Pen(Color.FromArgb(0, 122, 255), 2))
                {
                    e.Graphics.DrawRectangle(checkPen, checkBoxBounds);
                }

                if (item.IsCompleted)
                {
                    // Draw checkmark
                    using (Pen checkMarkPen = new Pen(Color.FromArgb(52, 199, 89), 3))
                    {
                        checkMarkPen.StartCap = LineCap.Round;
                        checkMarkPen.EndCap = LineCap.Round;

                        Point[] checkPoints = new Point[]
                        {
                            new Point(checkBoxBounds.X + 4, checkBoxBounds.Y + 10),
                            new Point(checkBoxBounds.X + 8, checkBoxBounds.Y + 14),
                            new Point(checkBoxBounds.X + 16, checkBoxBounds.Y + 6)
                        };
                        e.Graphics.DrawLines(checkMarkPen, checkPoints);
                    }
                }

                Color textColor = item.IsCompleted ? Color.FromArgb(150, 150, 150) : Color.FromArgb(30, 30, 30);
                FontStyle titleStyle = item.IsCompleted ? FontStyle.Bold | FontStyle.Strikeout : FontStyle.Bold;

                using (SolidBrush textBrush = new SolidBrush(textColor))
                {
                    Font titleFont = new Font("Segoe UI", 10F, titleStyle);
                    Font bodyFont = new Font("Segoe UI", 8.5F, item.IsCompleted ? FontStyle.Strikeout : FontStyle.Regular);

                    e.Graphics.DrawString(item.Title, titleFont, textBrush,
                        new RectangleF(itemBounds.X + 45, itemBounds.Y + 10, itemBounds.Width - 60, 20));

                    string preview = item.Body.Length > 50 ? item.Body.Substring(0, 50) + "..." : item.Body;
                    using (SolidBrush grayBrush = new SolidBrush(item.IsCompleted ? Color.FromArgb(170, 170, 170) : Color.FromArgb(120, 120, 120)))
                    {
                        e.Graphics.DrawString(preview, bodyFont, grayBrush,
                            new RectangleF(itemBounds.X + 45, itemBounds.Y + 32, itemBounds.Width - 60, 18));
                    }
                }
            }
        }

        private GraphicsPath GetRoundedRectangle(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, radius, radius, 180, 90);
            path.AddArc(bounds.Right - radius, bounds.Y, radius, radius, 270, 90);
            path.AddArc(bounds.Right - radius, bounds.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    public class MainForm : Form
    {
        private ModernListBox listBox;
        private GlassTextBox txtTitle;
        private GlassTextBox txtBody;
        private GlassTextBox txtSearch;
        private ModernButton btnAdd;
        private ModernButton btnEdit;
        private ModernButton btnDelete;
        private ModernButton btnExport;
        private ModernButton btnImport;
        private ModernButton btnSearch;
        private Label lblTitle;
        private Label lblBody;
        private Label lblHeader;
        private Label lblCounter;
        private RoundedPanel sidePanel;
        private Panel mainPanel;
        private List<ListItem> items;

        public MainForm()
        {
            items = new List<ListItem>();
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "VPLAssist+";
            this.Size = new Size(950, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.Font = new Font("Segoe UI", 9F);

            // Side Panel
            sidePanel = new RoundedPanel();
            sidePanel.BackColor = Color.FromArgb(252, 252, 254);
            sidePanel.BorderRadius = 20;
            sidePanel.Location = new Point(15, 15);
            sidePanel.Size = new Size(340, this.ClientSize.Height - 30);
            sidePanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;

            // Header
            lblHeader = new Label();
            lblHeader.Text = "Task Manager";
            lblHeader.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblHeader.ForeColor = Color.FromArgb(30, 30, 30);
            lblHeader.Location = new Point(25, 25);
            lblHeader.Size = new Size(180, 40);
            lblHeader.AutoSize = false;

            // Counter Label
            lblCounter = new Label();
            lblCounter.Text = "(0)";
            lblCounter.Font = new Font("Segoe UI", 14F, FontStyle.Regular);
            lblCounter.ForeColor = Color.FromArgb(100, 100, 100);
            lblCounter.Location = new Point(215, 28);
            lblCounter.Size = new Size(100, 35);
            lblCounter.AutoSize = true;

            // Search TextBox
            txtSearch = new GlassTextBox();
            txtSearch.Location = new Point(15, 75);
            txtSearch.Size = new Size(230, 40);
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // Search Button
            btnSearch = new ModernButton();
            btnSearch.Text = "Search";
            btnSearch.Location = new Point(255, 75);
            btnSearch.Size = new Size(70, 40);
            btnSearch.BackColor = Color.FromArgb(0, 122, 255);
            btnSearch.NormalColor = Color.FromArgb(0, 122, 255);
            btnSearch.HoverColor = Color.FromArgb(0, 108, 230);
            btnSearch.ForeColor = Color.White;
            btnSearch.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSearch.Click += BtnSearch_Click;
            btnSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // ListBox
            listBox = new ModernListBox();
            listBox.Location = new Point(15, 125);
            listBox.Size = new Size(310, sidePanel.Height - 195);
            listBox.BackColor = Color.FromArgb(252, 252, 254);
            listBox.SelectedIndexChanged += ListBox_SelectedIndexChanged;
            listBox.CheckboxClicked += ListBox_CheckboxClicked;
            listBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            sidePanel.Controls.Add(lblHeader);
            sidePanel.Controls.Add(lblCounter);
            sidePanel.Controls.Add(txtSearch);
            sidePanel.Controls.Add(btnSearch);
            sidePanel.Controls.Add(listBox);

            // Export Button
            btnExport = new ModernButton();
            btnExport.Text = "Export";
            btnExport.Location = new Point(15, sidePanel.Height - 50);
            btnExport.Size = new Size(145, 35);
            btnExport.BackColor = Color.FromArgb(140, 140, 150);
            btnExport.NormalColor = Color.FromArgb(140, 140, 150);
            btnExport.HoverColor = Color.FromArgb(120, 120, 130);
            btnExport.ForeColor = Color.White;
            btnExport.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnExport.Click += BtnExport_Click;
            btnExport.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            // Import Button
            btnImport = new ModernButton();
            btnImport.Text = "Import";
            btnImport.Location = new Point(170, sidePanel.Height - 50);
            btnImport.Size = new Size(145, 35);
            btnImport.BackColor = Color.FromArgb(140, 140, 150);
            btnImport.NormalColor = Color.FromArgb(140, 140, 150);
            btnImport.HoverColor = Color.FromArgb(120, 120, 130);
            btnImport.ForeColor = Color.White;
            btnImport.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnImport.Click += BtnImport_Click;
            btnImport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            sidePanel.Controls.Add(btnExport);
            sidePanel.Controls.Add(btnImport);

            // Main Panel
            mainPanel = new Panel();
            mainPanel.BackColor = Color.Transparent;
            mainPanel.Location = new Point(365, 15);
            mainPanel.Size = new Size(this.ClientSize.Width - 380, this.ClientSize.Height - 30);
            mainPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // Title Label
            lblTitle = new Label();
            lblTitle.Text = "Title";
            lblTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(60, 60, 60);
            lblTitle.Location = new Point(5, 15);
            lblTitle.Size = new Size(500, 30);

            // Title TextBox
            txtTitle = new GlassTextBox();
            txtTitle.Location = new Point(5, 45);
            txtTitle.Size = new Size(mainPanel.Width - 10, 45);
            txtTitle.Font = new Font("Segoe UI", 11F);
            txtTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // Body Label
            lblBody = new Label();
            lblBody.Text = "Body";
            lblBody.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblBody.ForeColor = Color.FromArgb(60, 60, 60);
            lblBody.Location = new Point(5, 110);
            lblBody.Size = new Size(500, 30);

            // Body TextBox
            txtBody = new GlassTextBox();
            txtBody.Location = new Point(5, 140);
            txtBody.Size = new Size(mainPanel.Width - 10, mainPanel.Height - 230);
            txtBody.Multiline = true;
            txtBody.Font = new Font("Segoe UI", 10F);
            txtBody.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // Add Button
            btnAdd = new ModernButton();
            btnAdd.Text = "Add Note";
            btnAdd.Location = new Point(5, mainPanel.Height - 65);
            btnAdd.Size = new Size(165, 45);
            btnAdd.BackColor = Color.FromArgb(0, 122, 255);
            btnAdd.NormalColor = Color.FromArgb(0, 122, 255);
            btnAdd.HoverColor = Color.FromArgb(0, 108, 230);
            btnAdd.ForeColor = Color.White;
            btnAdd.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAdd.Click += BtnAdd_Click;
            btnAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            // Edit Button
            btnEdit = new ModernButton();
            btnEdit.Text = "Save Changes";
            btnEdit.Location = new Point(180, mainPanel.Height - 65);
            btnEdit.Size = new Size(165, 45);
            btnEdit.BackColor = Color.FromArgb(52, 199, 89);
            btnEdit.NormalColor = Color.FromArgb(52, 199, 89);
            btnEdit.HoverColor = Color.FromArgb(45, 180, 78);
            btnEdit.ForeColor = Color.White;
            btnEdit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnEdit.Click += BtnEdit_Click;
            btnEdit.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            // Delete Button
            btnDelete = new ModernButton();
            btnDelete.Text = "Delete";
            btnDelete.Location = new Point(355, mainPanel.Height - 65);
            btnDelete.Size = new Size(165, 45);
            btnDelete.BackColor = Color.FromArgb(255, 59, 48);
            btnDelete.NormalColor = Color.FromArgb(255, 59, 48);
            btnDelete.HoverColor = Color.FromArgb(230, 50, 40);
            btnDelete.ForeColor = Color.White;
            btnDelete.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDelete.Click += BtnDelete_Click;
            btnDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            mainPanel.Controls.Add(lblTitle);
            mainPanel.Controls.Add(txtTitle);
            mainPanel.Controls.Add(lblBody);
            mainPanel.Controls.Add(txtBody);
            mainPanel.Controls.Add(btnAdd);
            mainPanel.Controls.Add(btnEdit);
            mainPanel.Controls.Add(btnDelete);

            this.Controls.Add(mainPanel);
            this.Controls.Add(sidePanel);
        }

        private void UpdateCounter()
        {
            lblCounter.Text = $"({items.Count})";
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                MessageBox.Show("Please enter a search term.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Search for item by title (case-insensitive)
            int foundIndex = -1;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Title.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    foundIndex = i;
                    break;
                }
            }

            if (foundIndex >= 0)
            {
                listBox.SelectedIndex = foundIndex;
                listBox.TopIndex = foundIndex; // Scroll to the item
            }
            else
            {
                MessageBox.Show($"No task found with title containing '{searchTerm}'.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ListBox_CheckboxClicked(object sender, int index)
        {
            if (index >= 0 && index < items.Count)
            {
                items[index].IsCompleted = !items[index].IsCompleted;
                listBox.Invalidate();
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Please enter a title.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ListItem newItem = new ListItem
            {
                Title = txtTitle.Text,
                Body = txtBody.Text,
                IsCompleted = false
            };

            items.Add(newItem);
            listBox.Items.Add(newItem);
            UpdateCounter();
            ClearFields();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a note to edit.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Please enter a title.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int index = listBox.SelectedIndex;
            items[index].Title = txtTitle.Text;
            items[index].Body = txtBody.Text;

            listBox.Items[index] = items[index];
            listBox.Refresh();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a note to delete.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int index = listBox.SelectedIndex;
            items.RemoveAt(index);
            listBox.Items.RemoveAt(index);
            UpdateCounter();
            ClearFields();
        }

        private void ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex != -1)
            {
                ListItem selectedItem = items[listBox.SelectedIndex];
                txtTitle.Text = selectedItem.Title;
                txtBody.Text = selectedItem.Body;
                listBox.Invalidate();
            }
        }

        private void ClearFields()
        {
            txtTitle.Clear();
            txtBody.Clear();
            listBox.ClearSelected();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (items.Count == 0)
            {
                MessageBox.Show("No notes to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                saveDialog.DefaultExt = "json";
                saveDialog.FileName = "notes_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".json";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveDialog.FileName;
                    btnExport.Enabled = false;
                    btnExport.Text = "Exporting...";

                    System.Threading.Thread exportThread = new System.Threading.Thread(() =>
                    {
                        try
                        {
                            string json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
                            File.WriteAllText(filePath, json);

                            this.Invoke((MethodInvoker)delegate
                            {
                                btnExport.Enabled = true;
                                btnExport.Text = "Export";
                                MessageBox.Show("Notes exported successfully!", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            });
                        }
                        catch (Exception ex)
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                btnExport.Enabled = true;
                                btnExport.Text = "Export";
                                MessageBox.Show($"Error exporting notes: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            });
                        }
                    });
                    exportThread.IsBackground = true;
                    exportThread.Start();
                }
            }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openDialog = new OpenFileDialog())
            {
                openDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                openDialog.DefaultExt = "json";

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openDialog.FileName;
                    btnImport.Enabled = false;
                    btnImport.Text = "Importing...";

                    // Create a new thread for import operation
                    System.Threading.Thread importThread = new System.Threading.Thread(() =>
                    {
                        try
                        {
                            string json = File.ReadAllText(filePath);
                            List<ListItem> importedItems = JsonSerializer.Deserialize<List<ListItem>>(json);

                            if (importedItems != null && importedItems.Count > 0)
                            {
                                this.Invoke((MethodInvoker)delegate
                                {
                                    DialogResult result = MessageBox.Show(
                                        $"Found {importedItems.Count} note(s). Replace existing notes or add to them?",
                                        "Import Notes",
                                        MessageBoxButtons.YesNoCancel,
                                        MessageBoxIcon.Question);

                                    if (result == DialogResult.Yes) // Replace
                                    {
                                        items.Clear();
                                        listBox.Items.Clear();
                                        items.AddRange(importedItems);
                                        foreach (var item in importedItems)
                                        {
                                            listBox.Items.Add(item);
                                        }
                                        UpdateCounter();
                                        ClearFields();
                                        MessageBox.Show("Notes replaced successfully!", "Import Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else if (result == DialogResult.No) // Add
                                    {
                                        items.AddRange(importedItems);
                                        foreach (var item in importedItems)
                                        {
                                            listBox.Items.Add(item);
                                        }
                                        UpdateCounter();
                                        MessageBox.Show("Notes added successfully!", "Import Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }

                                    btnImport.Enabled = true;
                                    btnImport.Text = "Import";
                                });
                            }
                            else
                            {
                                this.Invoke((MethodInvoker)delegate
                                {
                                    btnImport.Enabled = true;
                                    btnImport.Text = "Import";
                                    MessageBox.Show("No valid notes found in file.", "Import", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                btnImport.Enabled = true;
                                btnImport.Text = "Import";
                                MessageBox.Show($"Error importing notes: {ex.Message}", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            });
                        }
                    });
                    importThread.IsBackground = true;
                    importThread.Start();
                }
            }
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}