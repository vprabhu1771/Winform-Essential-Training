To load a **.rdlc report file** in `ReportViewer` (Microsoft ReportViewer) inside your WinForms POS project, follow this complete step-by-step guide 👇

---

# ✅ STEP 1: Add RDLC File to Project

1. Right click project → **Add → New Item**
2. Select **Report**
3. Name it:

   ```
   SalesReport.rdlc
   ```
4. Click **Add**

---

# ✅ STEP 2: Set RDLC Properties

Select `SalesReport.rdlc` → Properties:

```
Build Action = Embedded Resource
Copy to Output Directory = Do not copy
```

---

# ✅ STEP 3: Design Report

Inside RDLC designer:

1. Add **DataSet**
2. Name it: `SalesDataSet`
3. Add fields like:

   * InvoiceNo
   * CustomerName
   * TotalAmount
   * SaleDate

Then drag a **Table** and bind fields.

---

# ✅ STEP 4: Load RDLC in ReportViewer

Modify your `SaleReportForm` like this:

```csharp
using Microsoft.Reporting.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace winform_bughunt_pos
{
    public partial class SaleReportForm : Form
    {
        private readonly ReportViewer _reportViewer;
        public SaleReportForm()
        {
            InitializeComponent();
            _reportViewer = new ReportViewer();
            _reportViewer.Dock = DockStyle.Fill;
            this.Controls.Add(_reportViewer);

            LoadReport();
        }

        private void LoadReport()
        {
            _reportViewer.LocalReport.ReportEmbeddedResource =
                "winform_bughunt_pos.Reports.SalesReport.rdlc";

            DataTable dt = GetSalesData();

            ReportDataSource rds = new ReportDataSource(
                "SalesDataSet",  // must match RDLC dataset name
                dt
            );

            _reportViewer.LocalReport.DataSources.Clear();
            _reportViewer.LocalReport.DataSources.Add(rds);

            _reportViewer.RefreshReport();
        }

        private DataTable GetSalesData()
        {
            DBHelper db = new DBHelper();

            string query = @"
                SELECT 
                    s.id,
                    s.bill_no,
                    s.sale_date,
                    c.name AS customer_name,
                    s.total_items,
                    s.total_amount,
                    s.discount,
                    s.grand_total,
                    s.payment_mode
                FROM sales s
                LEFT JOIN customers c ON c.id = s.customer_id
                ORDER BY s.sale_date DESC";


            return db.ExecuteDataTable(query);
        }
    }
}
```

---

# 🚨 IMPORTANT

### 🔴 This must MATCH EXACTLY:

```csharp
"SalesDataSet"
```

Should be same as RDLC Dataset Name.

And:

```csharp
"winform_bughunt_pos.SalesReport.rdlc"
```

Should be:

```
Namespace + Folder + FileName
```

If RDLC is inside folder `Reports`, then:

```csharp
"winform_bughunt_pos.Reports.SalesReport.rdlc"
```

---

# ✅ Alternative (If NOT Embedded)

Instead of embedded resource:

```csharp
_reportViewer.LocalReport.ReportPath =
    Application.StartupPath + @"\SalesReport.rdlc";
```

But embedded is recommended.

---

# 🎯 Result

When you open:

```csharp
using (SaleReportForm f = new SaleReportForm())
{
    f.ShowDialog();
}
```

Your report loads automatically with database data.

---

If you want next, I can show:

* 🔎 Filter by From–To date
* 🖨 Direct print without preview
* 📊 Group by day/month
* 💰 GST report format
* 🧾 Thermal bill format 80mm

Just tell me what you need for your POS system 🚀

```csharp
posMenu.SalesReportClicked += (s, e) => SaleReportForm;
```

You are just referencing the class name. You must **create an object** and **show the form**.

---

## ✅ Correct Way to Call `SaleReportForm`

Use this:

```csharp
posMenu.SalesReportClicked += (s, e) =>
{
    SaleReportForm reportForm = new SaleReportForm();
    reportForm.Show();        // non-modal
};
```

OR if you want modal (recommended for reports):

```csharp
posMenu.SalesReportClicked += (s, e) =>
{
    using (SaleReportForm reportForm = new SaleReportForm())
    {
        reportForm.ShowDialog();
    }
};
```

---

## 💡 Where to Put This?

Usually inside your **MainForm** constructor after `InitializeComponent()`:

```csharp
public MainForm()
{
    InitializeComponent();

    posMenu.SalesReportClicked += (s, e) =>
    {
        using (SaleReportForm reportForm = new SaleReportForm())
        {
            reportForm.ShowDialog();
        }
    };
}
```

---

## 🔥 Better Professional Way (Clean Method)

Instead of lambda, create method:

```csharp
posMenu.SalesReportClicked += OpenSalesReport;

private void OpenSalesReport(object sender, EventArgs e)
{
    using (SaleReportForm reportForm = new SaleReportForm())
    {
        reportForm.ShowDialog();
    }
}
```

