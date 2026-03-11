To make this **Payment Form UI reusable in WinForms**, the best approach is to create a **single reusable `UserControl` called `PaymentControl`** that contains all payment fields (Amt Received, Returned, Points, Bank/Card, etc.).
Then you can drop it into **any POS form**.

---

# 🧩 1️⃣ Create Reusable Component

Create a **UserControl**

```
Add → User Control
Name: PaymentControl.cs
```

---

# 🧩 2️⃣ Payment UI Layout

Structure inside the control:

```
PaymentControl
 ├── Payment Mode Panel
 │     ├ Cash
 │     ├ Card
 │     ├ GPay
 │     ├ Paytm
 │     ├ Credit
 │     └ Split
 │
 ├── Amount Section
 │     ├ Amt Received
 │     ├ Amt Returned
 │     └ Wallet Button
 │
 ├── Points Section
 │     ├ Points Earned
 │     ├ Redeem
 │     └ Amt
 │
 └── Bank Section
       ├ Bank Accounts (ComboBox)
       └ Bank/Card Amount
```

---

# 🧩 3️⃣ Full Reusable Control Code

### PaymentControl.cs

```csharp
using System;
using System.Drawing;
using System.Windows.Forms;

public class PaymentControl : UserControl
{
    public TextBox txtReceived;
    public TextBox txtReturned;
    public TextBox txtPoints;
    public TextBox txtRedeem;
    public TextBox txtTotal;
    public ComboBox cmbBank;
    public TextBox txtCardAmount;

    public PaymentControl()
    {
        BuildUI();
    }

    private void BuildUI()
    {
        this.Width = 650;
        this.Height = 260;
        this.BackColor = Color.White;

        Font labelFont = new Font("Segoe UI", 10, FontStyle.Bold);

        Label lblReceived = new Label()
        {
            Text = "Amt Received:",
            Location = new Point(20, 20),
            Font = labelFont,
            AutoSize = true
        };

        txtReceived = new TextBox()
        {
            Location = new Point(20, 45),
            Width = 150,
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            BackColor = Color.Beige
        };

        Label lblReturn = new Label()
        {
            Text = "Amt Returned:",
            Location = new Point(200, 20),
            Font = labelFont,
            AutoSize = true
        };

        txtReturned = new TextBox()
        {
            Location = new Point(200, 45),
            Width = 150,
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            BackColor = Color.Beige
        };

        Button btnWallet = new Button()
        {
            Text = "+Wallet",
            Location = new Point(380, 45),
            Width = 120,
            Height = 40
        };

        Label lblPoints = new Label()
        {
            Text = "Points Earned:",
            Location = new Point(20, 100),
            Font = labelFont
        };

        txtPoints = new TextBox()
        {
            Location = new Point(20, 125),
            Width = 150,
            BackColor = Color.MediumOrchid,
            ForeColor = Color.White
        };

        Label lblRedeem = new Label()
        {
            Text = "Redeem:",
            Location = new Point(200, 100),
            Font = labelFont
        };

        txtRedeem = new TextBox()
        {
            Location = new Point(200, 125),
            Width = 150
        };

        Label lblAmt = new Label()
        {
            Text = "Amt",
            Location = new Point(380, 100),
            Font = labelFont
        };

        txtTotal = new TextBox()
        {
            Location = new Point(380, 125),
            Width = 150,
            Font = new Font("Segoe UI", 14, FontStyle.Bold)
        };

        Label lblBank = new Label()
        {
            Text = "Bank Accounts:",
            Location = new Point(20, 170),
            Font = labelFont
        };

        cmbBank = new ComboBox()
        {
            Location = new Point(20, 195),
            Width = 200
        };

        Label lblCard = new Label()
        {
            Text = "Bank/Card:",
            Location = new Point(250, 170),
            Font = labelFont
        };

        txtCardAmount = new TextBox()
        {
            Location = new Point(250, 195),
            Width = 150
        };

        this.Controls.Add(lblReceived);
        this.Controls.Add(txtReceived);

        this.Controls.Add(lblReturn);
        this.Controls.Add(txtReturned);

        this.Controls.Add(btnWallet);

        this.Controls.Add(lblPoints);
        this.Controls.Add(txtPoints);

        this.Controls.Add(lblRedeem);
        this.Controls.Add(txtRedeem);

        this.Controls.Add(lblAmt);
        this.Controls.Add(txtTotal);

        this.Controls.Add(lblBank);
        this.Controls.Add(cmbBank);

        this.Controls.Add(lblCard);
        this.Controls.Add(txtCardAmount);
    }
}
```

---

# 🧩 4️⃣ Use in POS Form

```csharp
public partial class POSForm : Form
{
    public POSForm()
    {
        InitializeComponent();

        PaymentControl payment = new PaymentControl();
        payment.Location = new Point(10,50);

        this.Controls.Add(payment);
    }
}
```

---

# 🧩 5️⃣ Result

You now have a **reusable payment UI component**:

✔ POS Payment layout
✔ Reusable across forms
✔ All fields accessible
✔ Easy to extend

---

# 🚀 Advanced POS Version (Recommended)

For a **professional POS architecture**, create components:

```
Controls
 ├ PaymentModeSelector
 ├ AmountBox
 ├ LoyaltyPointsControl
 ├ BankPaymentControl
 └ PaymentControl (Main)
```

Benefits:

✔ Fully modular
✔ Easy maintenance
✔ Used across POS screens
✔ Clean architecture

---

💡 If you want, I can also give you a **complete POS payment component system** including:

* 🔢 **Numeric keypad**
* 💳 **Multi payment (Cash + Card + UPI Split)**
* ⚡ **Auto change calculation**
* 🎯 **Keyboard shortcuts (F1–F6)**

Just say **“Full POS payment component architecture”**.
