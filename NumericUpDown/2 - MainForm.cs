namespace HelloWorldApp
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();

            // Set Window Title
            this.Text = "Numeric Up Down Example";

            // Set Window Width and Height
            this.Width = 350;
            this.Height = 100;
        }

        private void InitializeComponent()
        {
            // Create a NumericUpDown control
            NumericUpDown numericUpDown = new NumericUpDown();

            // Set properties for the NumericUpDown control
            numericUpDown.Location = new System.Drawing.Point(10, 10);

            numericUpDown.Width = 120;

            // Minimum Value
            numericUpDown.Minimum = 0;

            // Maximum Value
            numericUpDown.Maximum = 100;

            // Initial Value
            numericUpDown.Value = 50;

            // Add the NumericUpDown control to the form's controls collection
            this.Controls.Add(numericUpDown);
        }
    }
}
