Perfect 😄 let’s build a **REAL, working WinForms POS system with MySQL + bill printing** — the kind you can actually deploy in a shop.

I’ll give you a **clean architecture**, **database**, **full flow**, and **ready-to-run code snippets**. You can expand later without rewriting everything.

---

# 🧾 WinForms POS System (MySQL + Bill Print)

## 🔥 Features

✅ Login (Admin / Cashier)
✅ Product master
✅ Billing screen (cart)
✅ Auto total calculation
✅ Save sales to MySQL
✅ Print bill (RDLC – A4 / Thermal ready)

---

## 🧱 1. Database Design (MySQL)

Run this first 👇

```sql
CREATE DATABASE pos_db;
USE pos_db;

-- Users
CREATE TABLE users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50),
    password VARCHAR(255),
    role VARCHAR(20)
);

INSERT INTO users (username, password, role)
VALUES ('admin', '1234', 'Admin');

-- Products
CREATE TABLE products (
    id INT AUTO_INCREMENT PRIMARY KEY,
    barcode VARCHAR(50),
    name VARCHAR(100),
    price DECIMAL(10,2),
    stock INT
);

-- Sales master
CREATE TABLE sales (
    id INT AUTO_INCREMENT PRIMARY KEY,
    bill_no VARCHAR(20),
    sale_date DATETIME,
    total DECIMAL(10,2)
);

-- Sales items
CREATE TABLE sale_items (
    id INT AUTO_INCREMENT PRIMARY KEY,
    sale_id INT,
    product_name VARCHAR(100),
    qty INT,
    price DECIMAL(10,2),
    total DECIMAL(10,2)
);
```

---

## 🔌 2. NuGet Packages (IMPORTANT)

Install these in your WinForms project:

```
MySql.Data
Microsoft.ReportingServices.ReportViewerControl.WinForms
```

---

## 🔑 3. MySQL Connection Class

`DB.cs`

```csharp
using MySql.Data.MySqlClient;

namespace POS
{
    class DB
    {
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(
                "server=localhost;database=pos_db;uid=root;pwd=;"
            );
        }
    }
}
```

---

## 🔐 4. Login Form

### Login Button Code

```csharp
private void btnLogin_Click(object sender, EventArgs e)
{
    using (var con = DB.GetConnection())
    {
        con.Open();
        var cmd = new MySqlCommand(
            "SELECT * FROM users WHERE username=@u AND password=@p",
            con
        );
        cmd.Parameters.AddWithValue("@u", txtUser.Text);
        cmd.Parameters.AddWithValue("@p", txtPass.Text);

        var dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            new POSForm().Show();
            this.Hide();
        }
        else
        {
            MessageBox.Show("Invalid Login");
        }
    }
}
```

✔ Enter = Login
✔ Esc = Exit (optional shortcut)

---

## 🧾 5. POS Billing Form UI

### Controls Needed

* `TextBox txtBarcode`
* `DataGridView dgv`
* `Label lblTotal`
* `Button btnSavePrint`

### DataGridView Columns

```
Item | Qty | Price | Total
```

---

## 📦 6. Add Product by Barcode

```csharp
private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
{
    if (e.KeyCode == Keys.Enter)
    {
        using (var con = DB.GetConnection())
        {
            con.Open();
            var cmd = new MySqlCommand(
                "SELECT name, price FROM products WHERE barcode=@b",
                con
            );
            cmd.Parameters.AddWithValue("@b", txtBarcode.Text);

            var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                string name = dr["name"].ToString();
                decimal price = Convert.ToDecimal(dr["price"]);

                dgv.Rows.Add(name, 1, price, price);
                CalculateTotal();
            }
        }
        txtBarcode.Clear();
    }
}
```

---

## ➕ 7. Calculate Grand Total

```csharp
void CalculateTotal()
{
    decimal total = 0;
    foreach (DataGridViewRow row in dgv.Rows)
    {
        total += Convert.ToDecimal(row.Cells["Total"].Value);
    }
    lblTotal.Text = total.ToString("0.00");
}
```

---

## 💾 8. Save Sale to MySQL

```csharp
private void SaveSale()
{
    using (var con = DB.GetConnection())
    {
        con.Open();
        var tran = con.BeginTransaction();

        string billNo = DateTime.Now.ToString("yyyyMMddHHmmss");

        var cmd = new MySqlCommand(
            "INSERT INTO sales(bill_no, sale_date, total) VALUES(@b, NOW(), @t)",
            con, tran
        );
        cmd.Parameters.AddWithValue("@b", billNo);
        cmd.Parameters.AddWithValue("@t", lblTotal.Text);
        cmd.ExecuteNonQuery();

        int saleId = (int)cmd.LastInsertedId;

        foreach (DataGridViewRow row in dgv.Rows)
        {
            var itemCmd = new MySqlCommand(
                "INSERT INTO sale_items(sale_id, product_name, qty, price, total) " +
                "VALUES(@sid,@n,@q,@p,@t)",
                con, tran
            );

            itemCmd.Parameters.AddWithValue("@sid", saleId);
            itemCmd.Parameters.AddWithValue("@n", row.Cells[0].Value);
            itemCmd.Parameters.AddWithValue("@q", row.Cells[1].Value);
            itemCmd.Parameters.AddWithValue("@p", row.Cells[2].Value);
            itemCmd.Parameters.AddWithValue("@t", row.Cells[3].Value);

            itemCmd.ExecuteNonQuery();
        }

        tran.Commit();
        PrintBill(saleId);
    }
}
```

---

## 🖨️ 9. Bill Printing (RDLC)

### RDLC Fields

```
Product | Qty | Price | Total
```

### Print Code

```csharp
void PrintBill(int saleId)
{
    ReportForm frm = new ReportForm(saleId);
    frm.ShowDialog();
}
```

`ReportForm.cs`

```csharp
reportViewer1.LocalReport.ReportPath = "Bill.rdlc";
reportViewer1.LocalReport.DataSources.Clear();
reportViewer1.LocalReport.DataSources.Add(
    new ReportDataSource("BillDataSet", GetBillData())
);
reportViewer1.RefreshReport();
```

✔ Works with **A4**
✔ Can resize to **80mm thermal**

---

## 🚀 Next Upgrades (Tell me what you want)

🔹 Barcode scanner support
🔹 Stock auto-deduction
🔹 GST / Tax split
🔹 Daily sales report
🔹 Thermal printer layout
🔹 Keyboard-only POS (fast billing)

Just say:

> “Next, add thermal printer bill”
> or
> “Give full project zip structure”

I’ve got you 🤝💻
