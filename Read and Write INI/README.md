ini-parser 
```
https://www.nuget.org/packages/ini-parser
```

Gotcha 👍 You want a **helper class** that:

* Loads `config.ini` **once**
* Reads **App Name / other settings**
* Lets **every Form** use it easily
* Automatically sets the **Form title (Text)** from `config.ini`

This is exactly the right time for a **static helper / config service** 💡
Let’s do it clean and reusable.

---

## 1️⃣ Create Helper Class: `AppConfig.cs`

Create a new class file in your project.

```csharp
using IniParser;
using IniParser.Model;
using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace pos
{
    public static class AppConfig
    {
        private static readonly string ConfigPath =
            Path.Combine(Application.StartupPath, "config.ini");

        public static string AppName { get; private set; }
        public static string DefaultPrinter { get; private set; }

        public static void Load()
        {
            var parser = new FileIniDataParser();

            // Create config.ini if not exists
            if (!File.Exists(ConfigPath))
            {
                IniData newData = new IniData();
                newData["APP"]["Name"] = Application.ProductName;
                newData["PRINTER"]["Default"] =
                    new PrinterSettings().PrinterName;

                parser.WriteFile(ConfigPath, newData);
            }

            // Read config.ini
            IniData data = parser.ReadFile(ConfigPath);

            AppName = data["APP"]["Name"];
            DefaultPrinter = data["PRINTER"]["Default"];
        }

        // Helper to apply title to any form
        public static void ApplyFormTitle(Form form)
        {
            form.Text = AppName;
        }
    }
}
```

---

## 2️⃣ Load Config ONCE (Program.cs)

Open **Program.cs** and load config before showing any form.

```csharp
static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        AppConfig.Load();   // 🔥 Load config.ini once

        Application.Run(new MainForm());
    }
}
```

---

## 3️⃣ Use in MainForm (or ANY Form)

### MainForm.cs

```csharp
public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
        AppConfig.ApplyFormTitle(this);
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        Application.Exit();
        base.OnFormClosing(e);
    }
}
```

---

## 4️⃣ Use in Other Forms (Login, Settings, POS, etc.)

```csharp
public partial class LoginForm : Form
{
    public LoginForm()
    {
        InitializeComponent();
        AppConfig.ApplyFormTitle(this);
    }
}
```

💡 If you want **different titles per form**:

```csharp
form.Text = $"{AppConfig.AppName} - Login";
```

---

## 5️⃣ Access Settings Anywhere

```csharp
string appName = AppConfig.AppName;
string printer = AppConfig.DefaultPrinter;
```

Perfect for:

* 🧾 Bill printing
* ⚙ Settings form
* 🖨 Printer selection
* 📊 Reports

---

## 6️⃣ Sample `config.ini`

```ini
[APP]
Name=My POS System

[PRINTER]
Default=HP LaserJet P1108
```

---

### ✅ Result

* config.ini loaded **once**
* No duplicate code
* All forms get the **same app title**
* Clean, professional POS architecture 💼

If you want next:
👉 auto-append branch name
👉 change title at runtime
👉 save settings back to ini
👉 multi-branch config

Just tell me 😄
