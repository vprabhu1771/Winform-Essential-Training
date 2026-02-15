using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace pos
{
    public partial class UIForm1 : Form
    {
        private DataGridView dgvPOS;
        private TextBox txtSearch;

        private Label lblQty;
        private Label lblTotal;
        private Label lblDiscount;
        private Label lblGrand;

        private Panel mainPanel;
        private Panel gridPanel;

        public string SelectedBarcode { get; private set; }

        private DBHelper db = new DBHelper();

        public UIForm1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.KeyPreview = true;

            CreateMainLayout();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Application.Exit();
            base.OnFormClosing(e);
        }

        // ==============================
        // MAIN LAYOUT
        // ==============================
        private void CreateMainLayout()
        {
            mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.BackColor = Color.FromArgb(245, 245, 245);
            this.Controls.Add(mainPanel);

            Panel topPanel = CreateTopPanel();
            Panel summaryPanel = CreateSummaryPanel();
            Panel buttonPanel = CreateBottomButtons();

            gridPanel = new Panel();
            gridPanel.Dock = DockStyle.Fill;
            gridPanel.Padding = new Padding(10);

            InitializeGrid();
            dgvPOS.Dock = DockStyle.Fill;
            gridPanel.Controls.Add(dgvPOS);

            mainPanel.Controls.Add(buttonPanel);
            mainPanel.Controls.Add(summaryPanel);
            mainPanel.Controls.Add(gridPanel);
            mainPanel.Controls.Add(topPanel);
        }

        // ==============================
        // TOP PANEL
        // ==============================
        private ComboBox cmbCustomer;
        private Button btnAddCustomer;
        private Panel CreateTopPanel()
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Top;
            panel.Height = 70;
            panel.Padding = new Padding(15);
            panel.BackColor = Color.White;

            // 🔹 Barcode Search
            txtSearch = new TextBox()
            {
                Left = 15,
                Top = 15,
                Width = 550,
                Height = 35,
                Font = new Font("Segoe UI", 14),
                PlaceholderText = "Item name / Barcode / Item code [Enter]"
            };
            txtSearch.KeyDown += TxtSearch_KeyDown;

            // 🔹 Customer Combo
            cmbCustomer = new ComboBox()
            {
                Left = txtSearch.Right + 20,
                Top = 18,
                Width = 250,
                Font = new Font("Segoe UI", 11),
                DropDownStyle = ComboBoxStyle.DropDown   // IMPORTANT (not DropDownList)
            };

            cmbCustomer.TextChanged += CmbCustomer_TextChanged;

            LoadCustomers(); // Load from DB

            // 🔹 Add Customer Button
            btnAddCustomer = new Button()
            {
                Text = "➕",
                Left = cmbCustomer.Right + 10,
                Top = 18,
                Width = 40,
                Height = 30,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.Green,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAddCustomer.FlatAppearance.BorderSize = 0;
            btnAddCustomer.Click += BtnAddCustomer_Click;

            panel.Controls.Add(txtSearch);
            panel.Controls.Add(cmbCustomer);
            panel.Controls.Add(btnAddCustomer);

            return panel;
        }

        private void LoadCustomers()
        {
            cmbCustomer.Items.Clear();

            string query = "SELECT id, name FROM customers ORDER BY name";

            DataTable dt = db.ExecuteDataTable(query);

            cmbCustomer.DisplayMember = "name";
            cmbCustomer.ValueMember = "id";
            cmbCustomer.DataSource = dt;

            if (dt.Rows.Count == 0)
            {
                cmbCustomer.Items.Add("Walk-in customer");
                cmbCustomer.SelectedIndex = 0;
            }
        }

        private void CmbCustomer_TextChanged(object sender, EventArgs e)
        {
            string mobile = cmbCustomer.Text.Trim();

            if (mobile.Length >= 3) // start searching after 3 digits
            {
                string query = "SELECT id, name, mobile FROM customers WHERE mobile LIKE @mobile LIMIT 10";

                MySqlParameter[] parameters =
                {
            new MySqlParameter("@mobile", mobile + "%")
        };

                DataTable dt = db.ExecuteDataTable(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    cmbCustomer.DataSource = dt;
                    cmbCustomer.DisplayMember = "name";
                    cmbCustomer.ValueMember = "id";
                    cmbCustomer.DroppedDown = true;
                }
            }
        }

        private void BtnAddCustomer_Click(object sender, EventArgs e)
        {
            CustomerSearchForm frm = new CustomerSearchForm();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadCustomers();
                cmbCustomer.SelectedValue = frm.SelectedCustomerId;
            }
        }


        // ==============================
        // GRID
        // ==============================
        private void InitializeGrid()
        {
            dgvPOS = new DataGridView();
            dgvPOS.AllowUserToAddRows = false;
            dgvPOS.RowHeadersVisible = false;
            dgvPOS.BackgroundColor = Color.White;
            dgvPOS.BorderStyle = BorderStyle.None;
            dgvPOS.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPOS.ColumnHeadersHeight = 40;

            dgvPOS.EnableHeadersVisualStyles = false;
            dgvPOS.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dgvPOS.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            dgvPOS.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPOS.MultiSelect = false;

            dgvPOS.DefaultCellStyle.Font = new Font("Segoe UI", 11);
            dgvPOS.RowTemplate.Height = 35;

            dgvPOS.Columns.Add("HSN", "HSN");
            dgvPOS.Columns.Add("Product", "Item Name");
            dgvPOS.Columns.Add("Barcode", "Barcode");
            dgvPOS.Columns.Add("Qty", "Qty");
            dgvPOS.Columns.Add("Rate", "Price");
            dgvPOS.Columns.Add("Disc", "Discount");
            dgvPOS.Columns.Add("Total", "Subtotal");
            dgvPOS.Columns.Add("GST", "GST%");
            dgvPOS.Columns["GST"].Visible = false;

            dgvPOS.KeyDown += DgvPOS_KeyDown;
        }

        // ==============================
        // SUMMARY PANEL
        // ==============================
        private Panel CreateSummaryPanel()
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Bottom;
            panel.Height = 70;
            panel.BackColor = Color.WhiteSmoke;

            lblQty = CreateLabel("Quantity: 0", 20);
            lblTotal = CreateLabel("Total Amount: ₹ 0.00", 250);
            lblDiscount = CreateLabel("Total Discount: ₹ 0.00", 550);

            lblGrand = new Label()
            {
                Text = "Grand Total: ₹ 0.00",
                Left = 900,
                Top = 25,
                AutoSize = true,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.DarkGreen
            };

            panel.Controls.Add(lblQty);
            panel.Controls.Add(lblTotal);
            panel.Controls.Add(lblDiscount);
            panel.Controls.Add(lblGrand);

            return panel;
        }

        private Label CreateLabel(string text, int left)
        {
            return new Label()
            {
                Text = text,
                Left = left,
                Top = 25,
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
        }

        // ==============================
        // BOTTOM BUTTONS
        // ==============================
        private Panel CreateBottomButtons()
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Bottom;
            panel.Height = 60;

            //panel.Controls.Add(CreateButton("Hold", Color.Red));
            //panel.Controls.Add(CreateButton("Multiple", Color.Blue));
            //panel.Controls.Add(CreateButton("Cash", Color.Green));
            //panel.Controls.Add(CreateButton("UPI", Color.Goldenrod));
            //panel.Controls.Add(CreateButton("Credit", Color.DarkRed));

            panel.Controls.Add(CreateButton("F7 - Record Sale and Print", Color.Gray));
            panel.Controls.Add(CreateButton("F6 - Record Sale", Color.Gray));
            panel.Controls.Add(CreateButton("F5 - Search Customer", Color.Gray));
            panel.Controls.Add(CreateButton("F4 - Payment Mode", Color.Gray));
            panel.Controls.Add(CreateButton("F3 - Change Quantity", Color.Gray));
            panel.Controls.Add(CreateButton("F2 - Search Product", Color.Gray));
            panel.Controls.Add(CreateButton("F1 - Scan Barcode", Color.Gray));

            return panel;
        }

        private Button CreateButton(string text, Color color)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Dock = DockStyle.Left;
            btn.Width = this.Width / 5;
            btn.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btn.BackColor = color;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        // ==============================
        // BARCODE SEARCH
        // ==============================
        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddOrIncreaseProduct(txtSearch.Text.Trim());
                txtSearch.Clear();
            }
        }

        private void AddOrIncreaseProduct(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode))
                return;

            foreach (DataGridViewRow row in dgvPOS.Rows)
            {
                if (row.Cells["Barcode"].Value?.ToString() == barcode)
                {
                    int qty = Convert.ToInt32(row.Cells["Qty"].Value);
                    row.Cells["Qty"].Value = qty + 1;
                    UpdateRow(row);
                    CalculateSummary();
                    return;
                }
            }

            string query = "SELECT hsn, product_name, rate, gst_percent FROM products WHERE barcode=@barcode";

            MySqlParameter[] parameters =
            {
                new MySqlParameter("@barcode", barcode)
            };

            DataTable dt = db.ExecuteDataTable(query, parameters);

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                dgvPOS.Rows.Add(
                    dr["hsn"],
                    dr["product_name"],
                    barcode,
                    1,
                    dr["rate"],
                    0,
                    dr["rate"],
                    dr["gst_percent"]
                );

                CalculateSummary();
            }
            else
            {
                MessageBox.Show("Product Not Found!");
            }
        }

        // ==============================
        // UPDATE ROW
        // ==============================
        private void UpdateRow(DataGridViewRow row)
        {
            decimal qty = Convert.ToDecimal(row.Cells["Qty"].Value);
            decimal rate = Convert.ToDecimal(row.Cells["Rate"].Value);
            decimal disc = Convert.ToDecimal(row.Cells["Disc"].Value);

            decimal total = (qty * rate) - disc;
            row.Cells["Total"].Value = total;
        }

        // ==============================
        // DELETE + EDIT
        // ==============================
        private void DgvPOS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && dgvPOS.SelectedRows.Count > 0)
            {
                dgvPOS.Rows.RemoveAt(dgvPOS.SelectedRows[0].Index);
                CalculateSummary();
            }

            //if (e.KeyCode == Keys.F2 && dgvPOS.SelectedRows.Count > 0)
            //{
            //    DataGridViewRow row = dgvPOS.SelectedRows[0];
            //    string input = Microsoft.VisualBasic.Interaction.InputBox("Enter Qty", "Edit Qty", row.Cells["Qty"].Value.ToString());

            //    if (int.TryParse(input, out int newQty))
            //    {
            //        row.Cells["Qty"].Value = newQty;
            //        UpdateRow(row);
            //        CalculateSummary();
            //    }
            //}
            

            if (e.KeyCode == Keys.F3 && dgvPOS.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvPOS.SelectedRows[0];

                int currentQty = Convert.ToInt32(row.Cells["Qty"].Value);

                // Open Quantity Form
                using (QuantityForm qtyForm = new QuantityForm(currentQty))
                {                    

                    if (qtyForm.ShowDialog() == DialogResult.OK)
                    {
                        // Get updated quantity
                        row.Cells["Qty"].Value = qtyForm.Quantity;

                        // Recalculate row
                        UpdateRow(row);

                        // Update totals
                        CalculateSummary();
                    }
                }

                e.Handled = true;
            }

            // F4 → UPI Payment
            if (e.KeyCode == Keys.F4)
            {
                if (dgvPOS.Rows.Count == 0)
                    return;

                decimal grandTotal = GetGrandTotal();

                // Your UPI details
                string upiId = "yourupi@bank";
                string merchantName = "My POS Store";

                using (QRForm qr = new QRForm(upiId, merchantName, grandTotal.ToString("0.00")))
                {
                    qr.ShowDialog();
                }

                e.Handled = true;
            }

        }

        private decimal GetGrandTotal()
        {
            decimal totalAmount = 0;
            decimal discount = 0;

            foreach (DataGridViewRow row in dgvPOS.Rows)
            {
                decimal qty = Convert.ToDecimal(row.Cells["Qty"].Value);
                decimal rate = Convert.ToDecimal(row.Cells["Rate"].Value);
                decimal disc = Convert.ToDecimal(row.Cells["Disc"].Value);

                totalAmount += qty * rate;
                discount += disc;
            }

            return totalAmount - discount;
        }


        // ==============================
        // CALCULATE SUMMARY
        // ==============================
        private void CalculateSummary()
        {
            int totalItems = dgvPOS.Rows.Count;
            decimal totalAmount = 0;
            decimal discount = 0;

            foreach (DataGridViewRow row in dgvPOS.Rows)
            {
                decimal qty = Convert.ToDecimal(row.Cells["Qty"].Value);
                decimal rate = Convert.ToDecimal(row.Cells["Rate"].Value);
                decimal disc = Convert.ToDecimal(row.Cells["Disc"].Value);

                totalAmount += qty * rate;
                discount += disc;
            }

            decimal grand = totalAmount - discount;

            lblQty.Text = $"Quantity: {totalItems}";
            lblTotal.Text = $"Total Amount: ₹ {totalAmount:0.00}";
            lblDiscount.Text = $"Total Discount: ₹ {discount:0.00}";
            lblGrand.Text = $"Grand Total: ₹ {grand:0.00}";
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.F1:
                    txtSearch.Focus();
                    return true;

                case Keys.F2:
                    OpenProductSearch();
                    return true;

                case Keys.F3:
                    ChangeSelectedQuantity();
                    return true;

                case Keys.F4:
                    OpenPaymentMode();
                    return true;

                case Keys.F5:
                    OpenCustomerSearch();
                    return true;

                case Keys.F6:
                    RecordSale(false);
                    return true;

                case Keys.F7:
                    RecordSale(true);
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void OpenProductSearch()
        {
            using (ProductSearchForm frm = new ProductSearchForm())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    AddOrIncreaseProduct(frm.SelectedBarcode);
                }
            }
        }

        private void ChangeSelectedQuantity()
        {
            if (dgvPOS.SelectedRows.Count == 0)
                return;

            DataGridViewRow row = dgvPOS.SelectedRows[0];
            int currentQty = Convert.ToInt32(row.Cells["Qty"].Value);

            using (QuantityForm qtyForm = new QuantityForm(currentQty))
            {
                if (qtyForm.ShowDialog() == DialogResult.OK)
                {
                    row.Cells["Qty"].Value = qtyForm.Quantity;
                    UpdateRow(row);
                    CalculateSummary();
                }
            }
        }

        private void OpenPaymentMode()
        {
            if (dgvPOS.Rows.Count == 0)
                return;

            decimal grandTotal = GetGrandTotal();

            string upiId = "yourupi@bank";
            string merchantName = "My POS Store";

            using (QRForm qr = new QRForm(upiId, merchantName, grandTotal.ToString("0.00")))
            {
                qr.ShowDialog();
            }
        }

        private void OpenCustomerSearch()
        {
            using (CustomerSearchForm frm = new CustomerSearchForm())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    cmbCustomer.SelectedValue = frm.SelectedCustomerId;
                }
            }
        }

        private void RecordSale(bool print)
        {
            if (dgvPOS.Rows.Count == 0)
            {
                MessageBox.Show("No items to save.");
                return;
            }

            try
            {
                int customerId = cmbCustomer.SelectedValue != null
                    ? Convert.ToInt32(cmbCustomer.SelectedValue)
                    : 0;

                decimal grandTotal = GetGrandTotal();

                string insertSale =
                    "INSERT INTO sales (customer_id, total_amount, sale_date) " +
                    "VALUES (@customer, @total, NOW()); SELECT LAST_INSERT_ID();";

                MySqlParameter[] saleParams =
                {
                    new MySqlParameter("@customer", customerId),
                    new MySqlParameter("@total", grandTotal)
                };

                int saleId = Convert.ToInt32(db.ExecuteScalar(insertSale, saleParams));

                foreach (DataGridViewRow row in dgvPOS.Rows)
                {
                    string insertItem =
                        "INSERT INTO sale_items (sale_id, product_name, qty, rate, discount, subtotal) " +
                        "VALUES (@saleId, @name, @qty, @rate, @disc, @total)";

                    MySqlParameter[] itemParams =
                    {
                        new MySqlParameter("@saleId", saleId),
                        new MySqlParameter("@name", row.Cells["Product"].Value),
                        new MySqlParameter("@qty", row.Cells["Qty"].Value),
                        new MySqlParameter("@rate", row.Cells["Rate"].Value),
                        new MySqlParameter("@disc", row.Cells["Disc"].Value),
                        new MySqlParameter("@total", row.Cells["Total"].Value)
                    };

                    db.ExecuteNonQuery(insertItem, itemParams);
                }

                MessageBox.Show("Sale Recorded Successfully!");

                if (print)
                {
                    PrintBill(saleId);
                }

                dgvPOS.Rows.Clear();
                CalculateSummary();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void PrintBill(int saleId)
        {
            MessageBox.Show($"Printing Bill for Sale ID: {saleId}");
            // Connect to RDLC or Thermal Printer here
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
