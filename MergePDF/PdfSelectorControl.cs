using System.Drawing;
using System.Windows.Forms;

namespace MergePDF
{
    public partial class PdfSelectorControl : UserControl
    {
        private OpenFileDialog openFileDialog;
        public string FileName
        {
            get => textBox.Text;
            set => textBox.Text = value;
        }

        public int Index { get; }

        TextBox textBox;

        public PdfSelectorControl(int index)
        {
            InitializeComponent();

            openFileDialog = new OpenFileDialog
            {
                InitialDirectory = "c:\\",
                Title = "Open PDF file",
                Filter = "PDF files (*.pdf)|*.pdf",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            Index = index;

            CreateTextbox();

            Controls.Add(textBox);
            Controls.Add(CreateOpenButton());
            Controls.Add(CreateMoveUpButton());
            Controls.Add(CreateMoveDownButton());
        }

        private Button CreateMoveUpButton()
        {
            Button button = new Button
            {
                Location = new Point(490, 0),
                Size = new Size(20, 20),
                Text = "▲",
                Visible = true
            };

            button.Click += (sender, e) =>
            {
                if (this.Index == 1)
                {
                    MessageBox.Show("This is already the first file.", "Unable to move", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var prev = (PdfSelectorControl)this.Parent.Controls[this.Index - 2];
                var prevFileName = prev.FileName;
                prev.FileName = FileName;
                FileName = prevFileName;

            };

            return button;
        }

        private Button CreateMoveDownButton()
        {
            Button button = new Button
            {
                Location = new Point(510, 0),
                Size = new Size(20, 20),
                Text = "▼",
                Visible = true
            };

            button.Click += (sender, e) =>
            {
                var next = this.Parent.Controls[this.Index] as PdfSelectorControl;
                if (next == null)
                {
                    MessageBox.Show("This is already the last file.", "Unable to move", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var nextFilename = next.FileName;
                next.FileName = FileName;
                FileName = nextFilename;

            };

            return button;
        }

        private void CreateTextbox()
        {
            textBox = new TextBox
            {
                Location = new Point(126, 0),
                Size = new Size(350, 20),
                Visible = true,
                ReadOnly = true
            };
        }

        private Button CreateOpenButton()
        {
            Button button = new Button
            {
                Location = new Point(10, 0),
                Text = $"Browse file {Index}",
                Visible = true
            };

            button.Click += (sender, e) =>
            {
                var dialog = openFileDialog;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        FileName = dialog.FileName;
                    }
                    catch (System.IO.IOException)
                    {
                        MessageBox.Show("Uh-oh, something went wrong. Please try closing all the PDF files you want to import", "Import error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };

            return button;
        }
    }
}
