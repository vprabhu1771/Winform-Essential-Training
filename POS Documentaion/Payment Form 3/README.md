To build your **POS Payment UI like the screenshot**, the best design is a **modular reusable control architecture**.
This makes it easy to reuse in multiple forms and maintain.

We will create these **5 reusable controls**:

```
Controls
 ├ PaymentModeSelector   (Cash / Card / GPay / Paytm / Credit / Split)
 ├ AmountBox             (Reusable amount textbox)
 ├ LoyaltyPointsControl  (Points Earned + Redeem)
 ├ BankPaymentControl    (Bank Accounts + Card Amount)
 └ PaymentControl        (Main container with auto calculations)
```

And include:

⚡ **Auto change calculation**
🎯 **Keyboard shortcuts F1–F6**

---

# 1️⃣ AmountBox (Reusable POS Textbox)

```csharp
public class AmountBox : TextBox
{
    public AmountBox()
    {
        this.Font = new Font("Segoe UI", 16, FontStyle.Bold);
        this.BackColor = Color.Beige;
        this.BorderStyle = BorderStyle.FixedSingle;
        this.Width = 150;

        this.KeyPress += AmountBox_KeyPress;
    }

    void AmountBox_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (!char.IsControl(e.KeyChar) &&
            !char.IsDigit(e.KeyChar) &&
            e.KeyChar != '.')
        {
            e.Handled = true;
        }

        if (e.KeyChar == '.' && this.Text.Contains("."))
            e.Handled = true;
    }

    public decimal Value
    {
        get
        {
            decimal.TryParse(this.Text, out decimal v);
            return v;
        }
        set
        {
            this.Text = value.ToString("0.00");
        }
    }
}
```

---

# 2️⃣ PaymentModeSelector

Supports **F1–F6 shortcuts**

```csharp
public class PaymentModeSelector : UserControl
{
    public RadioButton rCash;
    public RadioButton rCard;
    public RadioButton rGpay;
    public RadioButton rPaytm;
    public RadioButton rCredit;
    public RadioButton rSplit;

    public PaymentModeSelector()
    {
        rCash = new RadioButton() { Text="Cash", Left=10 };
        rCard = new RadioButton() { Text="Card", Left=80 };
        rGpay = new RadioButton() { Text="GPay", Left=150 };
        rPaytm = new RadioButton() { Text="Paytm", Left=230 };
        rCredit = new RadioButton() { Text="Credit", Left=320 };
        rSplit = new RadioButton() { Text="Split", Left=410 };

        Controls.AddRange(new Control[] {
            rCash,rCard,rGpay,rPaytm,rCredit,rSplit
        });

        rCash.Checked = true;
    }

    public void HandleShortcut(Keys key)
    {
        if (key == Keys.F1) rCash.Checked = true;
        if (key == Keys.F2) rCard.Checked = true;
        if (key == Keys.F3) rGpay.Checked = true;
        if (key == Keys.F4) rPaytm.Checked = true;
        if (key == Keys.F5) rCredit.Checked = true;
        if (key == Keys.F6) rSplit.Checked = true;
    }
}
```

---

# 3️⃣ LoyaltyPointsControl

```csharp
public class LoyaltyPointsControl : UserControl
{
    public AmountBox txtPoints;
    public AmountBox txtRedeem;

    public LoyaltyPointsControl()
    {
        Label l1 = new Label(){Text="Points Earned:",Top=0};
        txtPoints = new AmountBox(){Top=20};

        Label l2 = new Label(){Text="Redeem:",Left=180};
        txtRedeem = new AmountBox(){Left=180,Top=20};

        Controls.AddRange(new Control[]{l1,txtPoints,l2,txtRedeem});
    }
}
```

---

# 4️⃣ BankPaymentControl

```csharp
public class BankPaymentControl : UserControl
{
    public ComboBox cmbBank;
    public AmountBox txtAmount;

    public BankPaymentControl()
    {
        Label l1 = new Label(){Text="Bank Accounts:"};
        cmbBank = new ComboBox(){Top=20,Width=200};

        Label l2 = new Label(){Text="Bank/Card:",Left=230};
        txtAmount = new AmountBox(){Left=230,Top=20};

        Controls.AddRange(new Control[]{l1,cmbBank,l2,txtAmount});
    }
}
```

---

# 5️⃣ Main PaymentControl (Auto Change Calculation)

```csharp
public class PaymentControl : UserControl
{
    public AmountBox txtReceived;
    public AmountBox txtReturned;
    public AmountBox txtTotal;

    PaymentModeSelector modeSelector;

    decimal totalAmount = 0;

    public PaymentControl()
    {
        modeSelector = new PaymentModeSelector();
        modeSelector.Top = 10;

        Label l1 = new Label(){Text="Amt Received:",Top=50};
        txtReceived = new AmountBox(){Top=70};

        Label l2 = new Label(){Text="Amt Returned:",Left=180,Top=50};
        txtReturned = new AmountBox(){Left=180,Top=70};

        Label l3 = new Label(){Text="Amt",Left=360,Top=50};
        txtTotal = new AmountBox(){Left=360,Top=70};

        txtReceived.TextChanged += Calc;

        Controls.AddRange(new Control[]{
            modeSelector,l1,txtReceived,l2,txtReturned,l3,txtTotal
        });
    }

    public decimal TotalAmount
    {
        get { return totalAmount; }
        set
        {
            totalAmount = value;
            txtTotal.Value = value;
        }
    }

    void Calc(object sender,EventArgs e)
    {
        decimal change = txtReceived.Value - totalAmount;
        txtReturned.Value = change;
    }

    public void HandleShortcut(Keys key)
    {
        modeSelector.HandleShortcut(key);
    }
}
```

---

# 6️⃣ Using In POS Form

```csharp
public partial class POS : Form
{
    PaymentControl payment;

    public POS()
    {
        InitializeComponent();

        payment = new PaymentControl();
        payment.TotalAmount = 7065;
        payment.Dock = DockStyle.Fill;

        this.Controls.Add(payment);

        this.KeyPreview = true;
        this.KeyDown += POS_KeyDown;
    }

    void POS_KeyDown(object sender, KeyEventArgs e)
    {
        payment.HandleShortcut(e.KeyCode);
    }
}
```

---

# 🎯 Features You Now Have

✔ Modular reusable POS components
✔ Auto **change calculation**
✔ **Keyboard shortcuts F1–F6**
✔ Bank / card support
✔ Loyalty points
✔ Reusable **AmountBox**

---

💡 If you want, I can also show a **professional POS upgrade** used in real retail systems:

* 🔢 **On-screen numeric keypad**
* 💰 **Multi payment (Cash + UPI + Card split)**
* ⚡ **Realtime payment validation**
* 🎨 **Modern POS UI like Reliance / Walmart**

I can build the **full production-ready POS payment UI for WinForms**.
