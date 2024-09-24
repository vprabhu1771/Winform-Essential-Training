namespace HelloWorldApp
{
    public partial class MainForm : Form
    {
        private ComboBox comboBox;
        private Button showButton;

        public MainForm()
        {            
            InitializeUI();

            // Set Window Title
            this.Text = "Combo Box Example";

            // Set Window Width and Height
            this.Width = 300;
            this.Height = 150;
        }

        private void InitializeUI()
        {
            // Create ComboBox
            comboBox = new ComboBox();

            comboBox.Items.AddRange(new string[] { 
                "C", 
                "C++", 
                "Java", 
                "Python" 
            });

            comboBox.Location = new System.Drawing.Point(20, 20);

            comboBox.Size = new System.Drawing.Size(150, 21);

            Controls.Add(comboBox);

            // Create Button
            showButton = new Button();
            showButton.Text = "Show Selection";
            showButton.Location = new System.Drawing.Point(20, 50);
            showButton.Click += ShowButton_Click;
            Controls.Add(showButton);
        }

        private void ShowButton_Click(object sender, EventArgs e)
        {
            string selectedItem = comboBox.SelectedItem as string;
            if (selectedItem != null)
            {
                MessageBox.Show("Selected item: " + selectedItem);
            }
            else
            {
                MessageBox.Show("No item selected.");
            }
        }

    }
}
