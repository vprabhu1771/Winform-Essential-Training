namespace HelloWorldApp
{
    public partial class MainForm : Form
    {
        private Button fontChooserButton;
        public MainForm()
        {
            InitializeComponent();

            // Set Window Title
            this.Text = "Font Dialog Example";

            // Set Window Width and Height         
            this.Width = 400;
            this.Height = 200;
        }

        private void InitializeComponent()
        {
            fontChooserButton = new Button();
            fontChooserButton.Text = "Select Font";
            fontChooserButton.Location = new System.Drawing.Point(100, 100);
            fontChooserButton.Click += fontChooserButton_Click;

            Controls.Add(fontChooserButton);
        }

        private void fontChooserButton_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                // Handle selected font
                Font selectedFont = fontDialog.Font;
                MessageBox.Show($"Selected Font: {selectedFont.FontFamily.Name}, Size: {selectedFont.Size}");
            }
        }
    }
}