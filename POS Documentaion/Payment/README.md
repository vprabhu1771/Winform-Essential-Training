```csharp
using System;
using System.Drawing;
using System.Windows.Forms;

namespace winform_bughunt_pos
{
    public partial class PaymentForm : Form
    {
        private ComboBox cmbMode;
        private TextBox txtCash;
        private TextBox txtUpi;
        private TextBox txtCard;
        private Label lblTotal;
        private Button btnOK;

        private decimal grandTotal;

        public string SelectedPaymentMode { get; private set; }
        public decimal CashAmount { get; private set; }
        public decimal UpiAmount { get; private set; }
        public decimal CardAmount { get; private set; }

        public PaymentForm(decimal total)
        {
            grandTotal = total;
            InitializeComponent();
            BuildUI();

            this.KeyPreview = true;
        }

        // =============================
        // GLOBAL SHORTCUT HANDLER
        // =============================
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Enter:
                    BtnOK_Click(btnOK, EventArgs.Empty); // Trigger Confirm button
                    return true;

                case Keys.Escape:
                    this.DialogResult = DialogResult.Cancel; // Close the form
                    this.Close();
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }


        private void BuildUI()
        {
            this.Text = "Payment";
            this.Size = new Size(400, 350);
            this.StartPosition = FormStartPosition.CenterParent;

            lblTotal = new Label()
            {
                Text = $"Grand Total : ₹ {grandTotal:0.00}",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true,
                Top = 20,
                Left = 20
            };

            cmbMode = new ComboBox()
            {
                Left = 20,
                Top = 60,
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cmbMode.Items.Add("Cash");
            cmbMode.Items.Add("UPI");
            cmbMode.Items.Add("Card");
            cmbMode.Items.Add("Split (Cash + UPI)");

            cmbMode.SelectedIndexChanged += CmbMode_SelectedIndexChanged;

            Label lblCash = new Label() { Text = "Cash", Left = 20, Top = 110 };
            txtCash = new TextBox() { Left = 120, Top = 110, Width = 150 };

            Label lblUpi = new Label() { Text = "UPI", Left = 20, Top = 150 };
            txtUpi = new TextBox() { Left = 120, Top = 150, Width = 150 };

            Label lblCard = new Label() { Text = "Card", Left = 20, Top = 190 };
            txtCard = new TextBox() { Left = 120, Top = 190, Width = 150 };

            btnOK = new Button()
            {
                Text = "Confirm",
                Height = 50,
                Width = 120,
                Left = 120,
                Top = 240,
                BackColor = Color.SeaGreen,
                ForeColor = Color.White
            };

            btnOK.Click += BtnOK_Click;

            this.Controls.Add(lblTotal);
            this.Controls.Add(cmbMode);
            this.Controls.Add(lblCash);
            this.Controls.Add(txtCash);
            this.Controls.Add(lblUpi);
            this.Controls.Add(txtUpi);
            this.Controls.Add(lblCard);
            this.Controls.Add(txtCard);
            this.Controls.Add(btnOK);

            cmbMode.SelectedIndex = 0;
        }

        private void CmbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCash.Text = "";
            txtUpi.Text = "";
            txtCard.Text = "";

            switch (cmbMode.SelectedItem.ToString())
            {
                case "Cash":
                    txtCash.Text = grandTotal.ToString("0.00");
                    break;

                case "UPI":
                    txtUpi.Text = grandTotal.ToString("0.00");
                    break;

                case "Card":
                    txtCard.Text = grandTotal.ToString("0.00");
                    break;

                case "Split (Cash + UPI)":
                    txtCash.Text = "0.00";
                    txtUpi.Text = "0.00";
                    break;
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            decimal cash = string.IsNullOrEmpty(txtCash.Text) ? 0 : Convert.ToDecimal(txtCash.Text);
            decimal upi = string.IsNullOrEmpty(txtUpi.Text) ? 0 : Convert.ToDecimal(txtUpi.Text);
            decimal card = string.IsNullOrEmpty(txtCard.Text) ? 0 : Convert.ToDecimal(txtCard.Text);

            if ((cash + upi + card) != grandTotal)
            {
                MessageBox.Show("Payment total must match Grand Total!");
                return;
            }

            CashAmount = cash;
            UpiAmount = upi;
            CardAmount = card;

            if (cmbMode.SelectedItem.ToString().StartsWith("Split"))
                SelectedPaymentMode = "Split";
            else
                SelectedPaymentMode = cmbMode.SelectedItem.ToString();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        //private void BtnOK_Click(object sender, EventArgs e)
        //{
        //    decimal cash = string.IsNullOrEmpty(txtCash.Text) ? 0 : Convert.ToDecimal(txtCash.Text);
        //    decimal upi = string.IsNullOrEmpty(txtUpi.Text) ? 0 : Convert.ToDecimal(txtUpi.Text);
        //    decimal card = string.IsNullOrEmpty(txtCard.Text) ? 0 : Convert.ToDecimal(txtCard.Text);

        //    if ((cash + upi + card) != grandTotal)
        //    {
        //        MessageBox.Show("Payment total must match Grand Total!");
        //        return;
        //    }

        //    CashAmount = cash;
        //    UpiAmount = upi;
        //    CardAmount = card;

        //    if (cmbMode.SelectedItem.ToString().StartsWith("Split"))
        //        SelectedPaymentMode = "Split";
        //    else
        //        SelectedPaymentMode = cmbMode.SelectedItem.ToString();

        //    this.DialogResult = DialogResult.OK;
        //    this.Close();
        //}
    }
}
```