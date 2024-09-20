using System;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

namespace LCDControlApp
{
    public partial class MainForm : Form
    {
        private SerialPort serialPort;

        public MainForm()
        {
            InitializeComponent();
            InitializeSerialPort();

            // Set Window Title
            this.Text = "LCD Control App";
        }

        private void InitializeSerialPort()
        {
            try
            {
                serialPort = new SerialPort("COM6", 9600);
                serialPort.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error opening serial port: {e.Message}");
            }
        }

        private void SendToLCD()
        {
            string text = entryTextBox.Text;
            Console.WriteLine(text);
            serialPort.Write(text);
            Console.WriteLine(Encoding.ASCII.GetBytes(text));
            // Send a newline character to indicate the end of the message
            // serialPort.Write("\n");
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            SendToLCD();
        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
            serialPort.Close();
            Close();
        }
    }
}
