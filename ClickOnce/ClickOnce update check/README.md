Here is **how to check for updates in ClickOnce** — both **automatically** and **manually** — for a Windows Forms or WPF application.

---

# ✅ **1. Automatic Check for Update (Recommended)**

In your **Project Properties → Publish → Updates**, enable:

* ✔ *The application should check for updates*
* Choose:

  * *Before application starts* **(best)**
  * OR *After application starts*

This will make ClickOnce automatically detect new updates.

---

# ✅ **2. Manual Check for Update in Code**

```
https://www.nuget.org/packages/FrameworkExtract.System.Deployment
```

ClickOnce provides the `ApplicationDeployment` class.

### **Add this namespace**

```csharp
using System.Deployment.Application;
```

---

# 🔹 **Check Automatically on Startup**

Place this in `Form_Load()` or at app startup:

```csharp
private void CheckForUpdates()
{
    if (ApplicationDeployment.IsNetworkDeployed)
    {
        ApplicationDeployment deploy = ApplicationDeployment.CurrentDeployment;

        try
        {
            UpdateCheckInfo info = deploy.CheckForDetailedUpdate();

            if (info.UpdateAvailable)
            {
                DialogResult result = MessageBox.Show(
                    "Update available. Do you want to update now?",
                    "Update",
                    MessageBoxButtons.YesNo
                );

                if (result == DialogResult.Yes)
                {
                    deploy.Update();
                    MessageBox.Show("Application will restart.");

                    Application.Restart();
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error checking for update: " + ex.Message);
        }
    }
}
```

Call it:

```csharp
private void Form1_Load(object sender, EventArgs e)
{
    CheckForUpdates();
}
```

---

# 🔹 **Manual Check for Update (Button Click)**

If you want a **Check for Update** button:

```csharp
private void btnCheckUpdate_Click(object sender, EventArgs e)
{
    CheckForUpdates();
}
```

---

# 🔹 **Check for Update Periodically**

Example: check every 7 days

```csharp
if (ApplicationDeployment.IsNetworkDeployed)
{
    var deploy = ApplicationDeployment.CurrentDeployment;

    if (deploy.IsFirstRun || DateTime.Now.Subtract(Properties.Settings.Default.LastUpdateCheck).Days >= 7)
    {
        CheckForUpdates();
        Properties.Settings.Default.LastUpdateCheck = DateTime.Now;
        Properties.Settings.Default.Save();
    }
}
```

---

# ⚠ IMPORTANT NOTES

### ✔ Your updates must be placed in the **publish folder**

(ClickOnce deployment location)

### ✔ Version number must increase

`Publish → Options → Automatically increment revision number`

### ✔ User must run the application from the ClickOnce installer (.application file)

---

# If you need a **full working Windows Forms example**, just tell me — I can give you a ready-made sample code.
