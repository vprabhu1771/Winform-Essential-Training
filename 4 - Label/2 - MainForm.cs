namespace HelloWorldApp
{
    public partial class MainForm : Form
    {
        private Label label;

        public MainForm()
        {
            InitializeComponent();

            // Set Window Title
            this.Text = "Label Example";

            // Set Window Width and Height         
            this.Width = 315;
            this.Height = 110;
        }

        private void InitializeComponent()
        {
            // Add a label to the form
            label = new Label();
            label.Text = "Hello, WinForms!";
            label.Location = new System.Drawing.Point(50, 50);

            // Add the label to the form's controls
            Controls.Add(label);
        }

    }
}
