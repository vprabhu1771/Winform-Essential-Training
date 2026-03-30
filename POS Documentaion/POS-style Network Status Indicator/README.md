A **POS-style Network Status Indicator** in WinForms usually shows a **green/red icon + text (Online / Offline)** and automatically updates every few seconds.
We can build it as a **reusable UserControl** so you can drop it into **any form in your POS project**.

---

# 1️⃣ Create Reusable Control

Create a new class:

```
NetworkStatusControl.cs
```

Make it inherit from `UserControl`.

---

# 2️⃣ Full Professional Network Status Control

```csharp
using System;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace WinFormsApp1.Controls
{
    public class NetworkStatusControl : UserControl
    {
        private Label lblStatus;
        private Panel indicator;
        private Timer timer;

        public int CheckInterval { get; set; } = 5000; // 5 seconds

        public NetworkStatusControl()
        {
            InitializeControl();
            StartMonitoring();
        }

        private void InitializeControl()
        {
            this.Width = 140;
            this.Height = 30;

            indicator = new Panel();
            indicator.Width = 12;
            indicator.Height = 12;
            indicator.Left = 5;
            indicator.Top = 8;
            indicator.BackColor = Color.Gray;

            lblStatus = new Label();
            lblStatus.Left = 25;
            lblStatus.Top = 6;
            lblStatus.AutoSize = true;
            lblStatus.Text = "Checking...";

            this.Controls.Add(indicator);
            this.Controls.Add(lblStatus);
        }

        private void StartMonitoring()
        {
            timer = new Timer();
            timer.Interval = CheckInterval;
            timer.Tick += Timer_Tick;
            timer.Start();

            CheckConnection();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            CheckConnection();
        }

        private void CheckConnection()
        {
            bool online = HasInternet();

            if (online)
            {
                indicator.BackColor = Color.LimeGreen;
                lblStatus.Text = "Online";
            }
            else
            {
                indicator.BackColor = Color.Red;
                lblStatus.Text = "Offline";
            }
        }

        private bool HasInternet()
        {
            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send("8.8.8.8", 1000);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
```

---

# 3️⃣ Use in Your Form

```csharp
using WinFormsApp1.Controls;

public partial class Form2 : Form
{
    public Form2()
    {
        InitializeComponent();

        NetworkStatusControl net = new NetworkStatusControl();
        net.Top = 20;
        net.Left = 20;

        this.Controls.Add(net);
    }
}
```

---

# 4️⃣ POS Style Output

When running your app:

🟢 **Online**
🔴 **Offline**

Example:

```
● Online
```

or

```
● Offline
```

Color changes automatically every **5 seconds**.

---

# 5️⃣ Why This Is Professional

✔ Reusable Control
✔ Automatic background checking
✔ Lightweight (Timer based)
✔ Works in **any form**
✔ POS friendly UI indicator
✔ Internet verified using **Ping**

---

# 6️⃣ Advanced POS Version (Optional Features)

You can also add:

✔ WiFi / LAN icon
✔ Tooltip with IP address
✔ Offline popup warning
✔ Event when connection lost
✔ Animated indicator
✔ Signal strength

Example event:

```csharp
public event EventHandler InternetLost;
```

---

💡 If you want, I can also give you a **🔥 POS-level Network Monitor Component used in billing software**

Features:

* Internet + Server + Printer status
* Animated indicator
* Toast notification when connection lost
* Auto reconnect detection

It looks like **supermarket POS systems**.
