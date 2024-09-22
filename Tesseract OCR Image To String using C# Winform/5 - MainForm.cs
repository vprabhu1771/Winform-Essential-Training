using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Tesseract;

namespace HelloWorldApp
{
    public partial class MainForm : Form
    {
        private Button selectImageButton;
        private PictureBox pictureBox;
        private TextBox resultTextBox;

        public MainForm()
        {
            InitializeUI();

            // Set Window Title
            this.Text = "OCR Example";

            // Set Window Width and Height
            this.Width = 800;
            this.Height = 600;
        }

        private void InitializeUI()
        {
            selectImageButton = new Button
            {
                Text = "Select Image",
                
                Location = new Point(10, 10)
            };

            selectImageButton.Click += SelectImageButton_Click;

            pictureBox = new PictureBox
            {
                Location = new Point(10, 50),

                // Resize the PictureBox
                Size = new Size(400, 400), 

                BorderStyle = BorderStyle.FixedSingle,

                SizeMode = PictureBoxSizeMode.Zoom
            };

            resultTextBox = new TextBox
            {
                Multiline = true,
                
                ScrollBars = ScrollBars.Vertical,
                
                Location = new Point(420, 50),

                // Adjust the size
                Size = new Size(360, 400), 
                
                ReadOnly = true,

                // Enable word wrapping
                WordWrap = true
            };

            Controls.Add(selectImageButton);
            Controls.Add(pictureBox);
            Controls.Add(resultTextBox);
        }

        private void SelectImageButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.jpg; *.png; *.bmp)|*.jpg;*.png;*.bmp|All Files (*.*)|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox.Image = Image.FromFile(openFileDialog.FileName);

                    InitializeOCR(openFileDialog.FileName);
                }
            }
        }

        private void InitializeOCR(string imagePathFromFile)
        {
            string imagePath = imagePathFromFile;
            string tessdataPath = "tessdata";
            string fullTessdataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tessdataPath);

            using (var engine = new TesseractEngine(fullTessdataPath, "eng", EngineMode.Default))
            {
                using (var img = Pix.LoadFromFile(imagePath))
                {
                    using (var page = engine.Process(img))
                    {
                        string text = page.GetText();

                        // Show text in the TextBox
                        resultTextBox.Text = text;

                    }
                }
            }
        }

    }
}
