namespace HelloWorldApp
{
    public partial class MainForm : Form
    {
        private TextBox textBox;
        private Button saveButton;

        public MainForm()
        {
            InitializeTextBox();
            InitializeSaveButton();

            // Set Window Title
            this.Text = "Text Box Example";

            // Set Window Width and Height
            this.Width = 400;
            this.Height = 400;
        }

        private void InitializeTextBox()
        {
            textBox = new TextBox();
            textBox.Multiline = true;
            textBox.Dock = DockStyle.Fill;

            Controls.Add(textBox);
        }

        private void InitializeSaveButton()
        {
            saveButton = new Button();
            saveButton.Text = "Save";
            saveButton.Dock = DockStyle.Bottom;

            saveButton.Click += SaveButton_Click;

            Controls.Add(saveButton);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string filePath = saveDialog.FileName;
                        string content = textBox.Text;
                        System.IO.File.WriteAllText(filePath, content);
                        MessageBox.Show("File saved successfully!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

    }
}
