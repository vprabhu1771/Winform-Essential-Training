You want a **simple test print like the image** (TEST PRINT SUCCESSFUL) from your **WinForms thermal printer class**.
So we will create a **TestPrint() function** inside your `ThermalPrinter` class.

This prints exactly like your sample receipt.

---

## ✅ Add TestPrint Method

Add this method inside your `ThermalPrinter` class.

```csharp
public void TestPrint()
{
    PrintDocument pd = new PrintDocument();

    pd.DefaultPageSettings.PaperSize =
        new PaperSize("Receipt", 280, 200);

    pd.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);

    pd.PrintPage += (sender, e) =>
    {
        Graphics g = e.Graphics;

        Font titleFont = new Font("Consolas", 14, FontStyle.Bold);
        Font normalFont = new Font("Consolas", 10);

        float y = 20;

        g.DrawString(
            "TEST PRINT SUCCESSFUL",
            titleFont,
            Brushes.Black,
            10,
            y
        );

        y += 40;

        g.DrawString(
            "Thank you for using our app!",
            normalFont,
            Brushes.Black,
            10,
            y
        );

        y += 40;

        g.DrawString(
            "Date: " + DateTime.Now.ToString("dd/MM/yyyy | hh:mm tt"),
            normalFont,
            Brushes.Black,
            10,
            y
        );

        y += 30;

        g.DrawString(
            "------------------------------------------",
            normalFont,
            Brushes.Black,
            0,
            y
        );
    };

    pd.Print();
}
```

---

## ✅ How to Call Test Print

Example **button click**:

```csharp
private void btnTestPrint_Click(object sender, EventArgs e)
{
    ThermalPrinter printer = new ThermalPrinter(
        dataGridView1,
        0,
        0,
        "",
        0,
        0,
        0
    );

    printer.TestPrint();
}
```

---

## ✅ Output

The printer will print:

```
TEST PRINT SUCCESSFUL

Thank you for using our app!

Date: 28/12/2025 | 12:14 PM
------------------------------------------
```

Exactly like your receipt photo.

---

## ⭐ Pro Tip (POS Software)

In professional POS systems we usually add:

```
ESC/POS Cut Command
```

So paper auto-cuts after printing.

I can also give you:

✔ **Professional POS Thermal Printer Class (ESC/POS Fast Printing)**
✔ **Logo Printing**
✔ **QR Code / UPI QR on receipt**
✔ **Ultra-fast printing (10x faster than PrintDocument)**

If you want, I can give the **best WinForms POS thermal printer class used in production systems.** 🚀
