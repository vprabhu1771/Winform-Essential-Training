Perfect Prabhu 👌
Since you're building a **full POS system**, I’ll give you a **Modern WinForms POS UI Kit Structure** — clean, scalable, production-ready (.NET 6/7/8).

This is not just a textbox — this is a **complete reusable POS design system**.

---

# 🖥 MODERN POS UI KIT (WinForms)

## 🎨 Design Style

✔ Flat
✔ Soft border
✔ Focus highlight
✔ Keyboard shortcut friendly
✔ Dark mode ready
✔ 4K monitor safe
✔ Fast rendering

---

# 🧱 UI KIT STRUCTURE

Create a new folder:

```
UI
 ├── Controls
 ├── Theme
 ├── Layout
```

---

# 1️⃣ THEME ENGINE (Global Styling)

## 🔹 POSTheme.cs

```csharp
using System.Drawing;

namespace winform_bughunt_pos.UI.Theme
{
    public static class POSTheme
    {
        public static Color Primary = Color.FromArgb(0, 120, 215);
        public static Color Border = Color.FromArgb(220, 220, 220);
        public static Color Background = Color.White;
        public static Color Hover = Color.AliceBlue;
        public static Color Danger = Color.FromArgb(220, 53, 69);
        public static Color Success = Color.FromArgb(40, 167, 69);
        public static Color TextPrimary = Color.Black;
        public static Color ShortcutBg = Color.Gainsboro;
    }
}
```

Now your entire POS can change color by editing one file.

---

# 2️⃣ MODERN POS TEXTBOX (Upgraded Version)

Enhance your control:

### Add BorderColor + FocusBorderColor

Inside `ModernTextBox`:

```csharp
private Color borderColor = UI.Theme.POSTheme.Border;
private Color focusBorderColor = UI.Theme.POSTheme.Primary;
private bool isFocused = false;
```

Override paint:

```csharp
protected override void OnPaint(PaintEventArgs e)
{
    base.OnPaint(e);

    Color drawColor = isFocused ? focusBorderColor : borderColor;

    using (Pen pen = new Pen(drawColor, 2))
    {
        e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
    }
}
```

Modify focus events:

```csharp
txtInput.Enter += (s, e) =>
{
    isFocused = true;
    this.Invalidate();
};

txtInput.Leave += (s, e) =>
{
    isFocused = false;
    this.Invalidate();
};
```

Now it looks like modern billing software.

---

# 3️⃣ MODERN POS BUTTON

Create `POSButton.cs`

```csharp
using System;
using System.Drawing;
using System.Windows.Forms;
using winform_bughunt_pos.UI.Theme;

public class POSButton : Button
{
    public POSButton()
    {
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        BackColor = POSTheme.Primary;
        ForeColor = Color.White;
        Font = new Font("Segoe UI", 11, FontStyle.Bold);
        Height = 45;
        Cursor = Cursors.Hand;
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        BackColor = ControlPaint.Light(POSTheme.Primary);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        BackColor = POSTheme.Primary;
    }
}
```

---

# 4️⃣ POS SHORTCUT BUTTON (F1 / F2 / F4)

Create:

```csharp
public class ShortcutButton : POSButton
{
    public Keys ShortcutKey { get; set; }

    public ShortcutButton()
    {
        BackColor = POSTheme.ShortcutBg;
        ForeColor = Color.Black;
    }
}
```

---

# 5️⃣ POS HEADER BAR

Create a top panel:

```csharp
Panel header = new Panel()
{
    Dock = DockStyle.Top,
    Height = 60,
    BackColor = POSTheme.Primary
};
```

Add:

* App name
* Logged user
* DateTime
* Counter name

---

# 6️⃣ MAIN POS LAYOUT STRUCTURE

Recommended Layout:

```
--------------------------------------
| Header                             |
--------------------------------------
| Search | Customer | Payment Shortcuts |
--------------------------------------
|        Sale Grid (DataGridView)        |
--------------------------------------
| Total | Tax | Grand Total | Pay Btn    |
--------------------------------------
```

Use:

* `TableLayoutPanel`
* `Dock = Fill`
* `Anchor` properly

---

# 7️⃣ MODERN SALE GRID STYLE

```csharp
dataGridView1.EnableHeadersVisualStyles = false;
dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = POSTheme.Primary;
dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
dataGridView1.RowTemplate.Height = 35;
dataGridView1.Font = new Font("Segoe UI", 11);
```

---

# 8️⃣ DARK MODE SUPPORT (Optional)

In `POSTheme.cs`:

```csharp
public static void EnableDarkMode()
{
    Background = Color.FromArgb(30,30,30);
    Border = Color.FromArgb(60,60,60);
    TextPrimary = Color.White;
}
```

---

# 🔥 ADVANCED FEATURES FOR YOUR POS

Since you're building:

* Split payment
* QR payment
* Thermal print
* License API
* Modular architecture

You should add:

✔ KeyboardManager (Global F1/F2/F4 handler)
✔ Notification toast system
✔ Loading overlay control
✔ Reusable numeric keypad control
✔ Custom MessageBox

---

# 🎯 PROFESSIONAL POS CONTROL SET

Your final UI kit should contain:

| Control         | Purpose             |
| --------------- | ------------------- |
| ModernTextBox   | Search, amount, qty |
| POSButton       | Main action buttons |
| ShortcutButton  | F1/F2/F4            |
| POSGrid         | Sales listing       |
| POSCard         | Summary boxes       |
| POSHeader       | Top bar             |
| POSNotification | Toast alert         |
| POSLoader       | Processing overlay  |

---

# 🚀 If You Want Next Level

I can now build for you:

* 💎 Complete Billing Screen Template
* 🧾 Thermal Print Preview UI
* 💰 Payment Popup UI (Cash / UPI / Split)
* 🧮 On-screen numeric keypad
* 🖥 4K responsive layout
* 🏪 Supermarket / Pharmacy / Restaurant specific UI

Tell me which module you want first:

Retail
Pharmacy
Restaurant
Supermarket

I’ll build a production-ready screen layout for your POS.
