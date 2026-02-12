Your code has two main problems:

1. `pictureBoxQR` is declared but never initialized.
2. `upiUrl` is created before form initialization and uses fields directly (better to build it dynamically).

Here is the **fixed and clean version** of your QRForm:

```csharp
using QRCoder;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace pos
{
    public partial class QRForm : Form
    {
        private PictureBox pictureBoxQR;

        public QRForm(string upiId, string name, string amount)
        {
            InitializeComponent();

            // Create PictureBox properly
            pictureBoxQR = new PictureBox();
            pictureBoxQR.Dock = DockStyle.Fill;
            pictureBoxQR.SizeMode = PictureBoxSizeMode.Zoom;
            this.Controls.Add(pictureBoxQR);

            // Build UPI URL dynamically
            string upiUrl = $"upi://pay?pa={upiId}&pn={name}&am={amount}&cu=INR";

            GenerateQRCode(upiUrl);
        }

        private void GenerateQRCode(string qrText)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q))
            using (QRCode qrCode = new QRCode(qrCodeData))
            {
                Bitmap qrImage = qrCode.GetGraphic(20);
                pictureBoxQR.Image = qrImage;
            }
        }
    }
}
```

---

### ✅ How to Call This Form from POSForm

```csharp
QRForm qr = new QRForm("merchant@upi", "My Shop", "250.00");
qr.ShowDialog();
```

---

### 🔥 What Was Fixed

* ✅ PictureBox initialized correctly
* ✅ UPI URL built inside constructor
* ✅ Used `using` blocks to prevent memory leaks
* ✅ Made QR dynamic (amount can change)

---

If you want, I can also give you:

* Auto amount from POS total
* Auto close after payment
* Payment success detection method
* Print QR option

Tell me what you need next 🚀


You want **Press F7 → Open QRForm** in your POSForm.

Here is the correct way to do it in WinForms 👇

---

## ✅ Step 1: Enable KeyPreview in POSForm

In your **POSForm constructor**:

```csharp
public POSForm()
{
    InitializeComponent();
    this.KeyPreview = true;   // VERY IMPORTANT
}
```

---

## ✅ Step 2: Add KeyDown Event

Add this event to your POSForm:

```csharp
protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
{
    if (keyData == Keys.F7)
    {
        OpenQRForm();
        return true; // Prevent further processing
    }

    return base.ProcessCmdKey(ref msg, keyData);
}
```

---

## ✅ Step 3: Create OpenQRForm Method

```csharp
private void OpenQRForm()
{
    string totalAmount = lblTotal.Text;   // Example: get amount from label
    // OR calculate from your cart

    QRForm qr = new QRForm("merchant@upi", "My Shop", totalAmount);
    qr.ShowDialog();
}
```

---

## 🔥 Alternative Method (Using KeyDown Event)

If you prefer normal event:

```csharp
private void POSForm_KeyDown(object sender, KeyEventArgs e)
{
    if (e.KeyCode == Keys.F7)
    {
        OpenQRForm();
        e.Handled = true;
    }
}
```

And in constructor:

```csharp
this.KeyDown += POSForm_KeyDown;
this.KeyPreview = true;
```

---

# 🎯 Result

Now:

👉 Press **F7**
👉 QRForm opens
👉 Amount auto loads

---

If you want next:

* Auto close QR after payment
* Show QR in small popup
* Direct print QR on bill
* F8 = Cash, F9 = Card shortcut

Tell me your POS flow, I’ll design it properly for you 🚀
