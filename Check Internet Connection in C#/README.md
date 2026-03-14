```
https://www.youtube.com/watch?v=xB9tWX4oKsw
```

You can **create a button programmatically** in WinForms and call `CheckInternetConnection()` when it is clicked.

Below is the **updated complete example**.

### ✔ WinForms Code (Programmatically Create Button)

```csharp
using System;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace WinFormsApp1
{
    public partial class Form2 : Form
    {
        Button btnCheckInternet;

        public Form2()
        {
            InitializeComponent();

            CreateButton();   // create button dynamically
        }

        private void CreateButton()
        {
            btnCheckInternet = new Button();
            btnCheckInternet.Text = "Check Internet";
            btnCheckInternet.Width = 150;
            btnCheckInternet.Height = 40;
            btnCheckInternet.Top = 50;
            btnCheckInternet.Left = 50;

            // attach click event
            btnCheckInternet.Click += BtnCheckInternet_Click;

            // add button to form
            this.Controls.Add(btnCheckInternet);
        }

        private void BtnCheckInternet_Click(object sender, EventArgs e)
        {
            CheckInternetConnection();
        }

        public void CheckInternetConnection()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                MessageBox.Show("Internet connection is available.");
            }
            else
            {
                MessageBox.Show("No internet connection.");
            }
        }
    }
}
```

### 🔹 What this code does

* Creates a **Button dynamically**
* Adds it to the **Form**
* Calls `CheckInternetConnection()` when clicked

### 💡 Optional (Better Internet Check)

`GetIsNetworkAvailable()` only checks **network adapter**, not real internet.
If you want **real internet check**, use **Ping Google DNS**.

```csharp
using System.Net.NetworkInformation;

public bool HasInternet()
{
    try
    {
        Ping ping = new Ping();
        PingReply reply = ping.Send("8.8.8.8", 1000);
        return reply.Status == IPStatus.Success;
    }
    catch
    {
        return false;
    }
}
```

---

If you want, I can also show a **🔥 Professional reusable WinForms component for Network Status (Online/Offline indicator like POS systems)**.
