In **Windows Forms (C#)** you can list all installed printers in a **ComboBox**, allow the user to select one, and set it as the **default printer**.

Below is a simple working example.

---

## 1️⃣ Add Namespaces

```csharp
using System.Drawing.Printing;
using System.Management;
```

---

# 2️⃣ Load All Printers into ComboBox

Call this in **Form_Load**.

```csharp
private void LoadPrinters()
{
    comboBoxPrinters.Items.Clear();

    foreach (string printer in PrinterSettings.InstalledPrinters)
    {
        comboBoxPrinters.Items.Add(printer);
    }

    // Select current default printer
    comboBoxPrinters.SelectedItem = new PrinterSettings().PrinterName;
}
```

### Form Load

```csharp
private void Form1_Load(object sender, EventArgs e)
{
    LoadPrinters();
}
```

---

# 3️⃣ Set Selected Printer as Default

Add a **Button → "Set Default Printer"**

```csharp
private void btnSetDefault_Click(object sender, EventArgs e)
{
    if (comboBoxPrinters.SelectedItem == null)
    {
        MessageBox.Show("Please select a printer.");
        return;
    }

    string printerName = comboBoxPrinters.SelectedItem.ToString();

    SetDefaultPrinter(printerName);

    MessageBox.Show("Default printer set successfully.");
}
```

---

# 4️⃣ Function to Set Default Printer

```csharp
public static void SetDefaultPrinter(string printerName)
{
    using (ManagementObjectSearcher searcher =
        new ManagementObjectSearcher("SELECT * FROM Win32_Printer"))
    {
        foreach (ManagementObject printer in searcher.Get())
        {
            if (printer["Name"].ToString() == printerName)
            {
                printer.InvokeMethod("SetDefaultPrinter", null);
                break;
            }
        }
    }
}
```

---

# 5️⃣ WinForm UI Example

```
[ComboBox]  comboBoxPrinters
[Button]    btnSetDefault
```

---

# ✅ Result

* Load **all installed printers**
* Show **current default printer**
* Select printer from ComboBox
* Click button → **Set as default printer**

---

💡 **Better POS/Invoice Practice**

Instead of changing **Windows default printer**, many POS systems store printer in **database/config** and print using that printer only.

Example:

```csharp
printDocument1.PrinterSettings.PrinterName = comboBoxPrinters.Text;
```

---

✅ If you want, I can also show:

* **How to Save Printer in App.config / Database**
* **How to Print Directly to Selected Printer**
* **POS Thermal Printer (80mm / 58mm) Setup for WinForms**

Just tell me 👍
