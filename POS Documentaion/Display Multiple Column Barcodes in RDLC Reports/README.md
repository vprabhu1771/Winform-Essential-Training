# Display Multiple Column Barcodes in RDLC 
```
https://www.youtube.com/watch?v=znAA0FDI1Ds
```

Perfect 👍 now we’ll implement:

✅ RDLC Print Integration
✅ Multiple barcode copies using Number field
✅ Clean professional POS-ready solution

We will NOT print from PictureBox.
We will generate barcode images → send to RDLC → print properly.

---

# 🏗 STEP 1 — Create RDLC Report

### Add New Report

```
Add → New Item → Report → BarcodeReport.rdlc
```

### Add Dataset

Dataset Name: `BarcodeDataSet`

Add one field:

```
BarcodeImage (System.Byte[])
```

---

# 🖼 STEP 2 — Design RDLC Layout

1. Insert → Image
2. Set properties:

```
Source: Database
Value: =Fields!BarcodeImage.Value
MIMEType: image/png
Sizing: FitProportional
```

Resize image area to label size (example 2x1 inch).

---

# 🧠 STEP 3 — Generate Multiple Barcode Images (as byte[])

Now modify your button to open RDLC preview.

---

# ✅ FINAL PROFESSIONAL CODE

Add:

```csharp
using Microsoft.Reporting.WinForms;
using System.IO;
```

---

## 🔹 Replace BtnGenerate_Click With:

```csharp
private void BtnGenerate_Click(object sender, EventArgs e)
{
    try
    {
        if (string.IsNullOrWhiteSpace(txtBarcode.Text))
        {
            MessageBox.Show("Enter barcode value");
            return;
        }

        int count = (int)numNumber.Value;
        if (count <= 0) count = 1;

        DataTable dt = new DataTable();
        dt.Columns.Add("BarcodeImage", typeof(byte[]));

        var barcode = new Barcode();
        barcode.IncludeLabel = true;

        for (int i = 0; i < count; i++)
        {
            using (var skImage = barcode.Encode(
                BarcodeStandard.Type.Code128,
                txtBarcode.Text.Trim(),
                SKColors.Black,
                SKColors.White,
                400,
                150))
            {
                using (var data = skImage.Encode(SKEncodedImageFormat.Png, 100))
                {
                    dt.Rows.Add(data.ToArray());
                }
            }
        }

        // Open RDLC Preview
        PrintBarcode(dt);
    }
    catch (Exception ex)
    {
        MessageBox.Show("Barcode Error:\n" + ex.Message);
    }
}
```

---

# 🖨 STEP 4 — Create Print Method

Add this method in your form:

```csharp
private void PrintBarcode(DataTable dt)
{
    Form reportForm = new Form();
    reportForm.WindowState = FormWindowState.Maximized;

    ReportViewer viewer = new ReportViewer();
    viewer.Dock = DockStyle.Fill;
    viewer.ProcessingMode = ProcessingMode.Local;

    viewer.LocalReport.ReportPath = "BarcodeReport.rdlc";

    viewer.LocalReport.DataSources.Clear();
    viewer.LocalReport.DataSources.Add(
        new ReportDataSource("BarcodeDataSet", dt));

    viewer.RefreshReport();

    reportForm.Controls.Add(viewer);
    reportForm.ShowDialog();
}
```

---

# 🎯 What Happens Now

If Number = 5
→ RDLC will print 5 barcode rows
→ Each row = 1 barcode
→ Ready for A4 or thermal label

---

# 🏷 For Thermal Label (2x1 inch)

In RDLC:

Report → Page Setup

```
Width: 2in
Height: 1in
Margins: 0
```

Then image size:

```
Width: 1.9in
Height: 0.9in
```

Perfect for 203 DPI thermal printers.

---

# 🏆 Why This Is Best Practice

✔ Clean RDLC print layout
✔ Supports multiple copies
✔ No PictureBox printing
✔ Works with thermal printers
✔ Production ready for POS

---

If you want next:

* 🔥 A4 sticker sheet (3x8 grid)
* 🖨 Direct silent printing (no preview)
* 📦 Print barcode from product list
* 🏷 Print different barcodes in one batch

Tell me your POS workflow — I’ll build the correct architecture for you.

That’s why it says:

> The report definition for 'BarcodeReport' has not been specified

---

# ✅ ✅ CORRECT FIX (Physical File Method – Simple & Safe)

### 🔹 Step 1 — Click `BarcodeReport.rdlc` in Solution Explorer

Set properties:

```
Build Action = Content
Copy to Output Directory = Copy Always
```
