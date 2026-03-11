A **Splash Screen in Windows Forms (WinForms)** is a small screen that appears **while the application is loading**. It usually shows the **logo, loading message, or progress bar** before the main form opens.

Below are the **common ways to create a WinForms Splash Screen**.

---

## 1️⃣ Simple Splash Screen (Recommended)

### Step 1: Create Splash Form

1. In **Visual Studio**
2. **Right Click Project → Add → Windows Form**
3. Name it **SplashScreen.cs**

Design it:

* Add **PictureBox (logo)**
* Add **Label (Loading...)**
* Optional **ProgressBar**

Example Design:

```
-----------------------------
|        [LOGO]             |
|                           |
|       Loading...          |
|    [=========     ]       |
-----------------------------
```

---

### Step 2: Add Timer in SplashScreen

Double click form → add code.

```csharp
public partial class SplashScreen : Form
{
    public SplashScreen()
    {
        InitializeComponent();
    }

    private void SplashScreen_Load(object sender, EventArgs e)
    {
        timer1.Start();
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
        progressBar1.Increment(5);

        if (progressBar1.Value >= 100)
        {
            timer1.Stop();
            this.Hide();

            Form1 main = new Form1();
            main.Show();
        }
    }
}
```

Set **timer interval = 100**

---

### Step 3: Start Application with Splash Screen

Open **Program.cs**

Change:

```csharp
Application.Run(new Form1());
```

to

```csharp
Application.Run(new SplashScreen());
```

---

✅ **Flow**

```
Program.cs
     ↓
SplashScreen
     ↓
Main Form (Form1)
```

---

## 2️⃣ Professional Splash Screen (Threaded)

This method shows splash **while app loads in background**.

Example:

```csharp
using System.Threading;

static class Program
{
    static SplashScreen splash;

    [STAThread]
    static void Main()
    {
        Thread t = new Thread(new ThreadStart(ShowSplash));
        t.Start();

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        Thread.Sleep(3000); // simulate loading

        splash.Invoke(new Action(() => splash.Close()));

        Application.Run(new Form1());
    }

    static void ShowSplash()
    {
        splash = new SplashScreen();
        Application.Run(splash);
    }
}
```

---

## 3️⃣ No Border Splash Style

Make splash look modern:

In **SplashScreen Properties**

```
FormBorderStyle = None
StartPosition = CenterScreen
TopMost = True
BackColor = White
```

---

## 4️⃣ Example Modern Layout

![Image](https://images.openai.com/static-rsc-4/W4E6hatSpsXERbx04oSW8LdVFoR5fT1T3ilXRnMqCiPwM1QutJHMMhfjeFGibyGyveSxNTdA4pDwOeP_OkJ_hghBDbFqBg9QtH_fyNzRmVwTPof0SeZ_Bb4nTCvys6oq5LBazoySqvAftBU5fXgD7f25yZWXMCbP1EqkO2OGeXcto5AfXOPy_U9-pbIcoaoS?purpose=fullsize)

![Image](https://images.openai.com/static-rsc-4/Zdhbn00Qr1JbLFICs4opEjDvDdTgtugwqwNxwgQH0REoQrP_uiKtCamr5QJ8qCLybCQLK3TjW6i7WWt8ywWgsU_Ouz_n5EIulPBmpDjjTjq3CoL170oQ6_BP2IAjBCyg73FECvnNnp3WQ8lyTMjE1EX-Hu6QHr4w09GKfKo6SxbGhywi8P-yZht6SAdqqIDu?purpose=fullsize)

![Image](https://images.openai.com/static-rsc-4/9k-lW__ClleSJKBIqCoKokQtSrvATpNB_Vpb7W6-ghG8TTEUEMYdohCn6qgXLdvdtLlKd1v2dz2D71J9p4LjkomeTAcIDzjAA8BnEdFb0CJUXziavDiuKuAcAtA2B7G0zA3wF7tc-DsDkQao9qk2_KYKT_v_-4Q8stW3Z3-mQC3YV4JHS1cKzfdMqW0LJ8Jl?purpose=fullsize)

![Image](https://images.openai.com/static-rsc-4/Ex9uFX2kTaLwuJyW0ZFAOwxwPpoCZ20Qnjg_EXcE0DHhAZdK2UfBKAuPB5lSlF1zAKVhI6YtYYT19xBDloSXZiFv6G5qANhWmvCymedHlZsZoQmQsekVRu3EeF-5esTxePedz8OI_LdvlZfjb1UawRFwQe4WDU7RQwXZYboiungFWeC4ijMAiqJ6Z7wVhvu8?purpose=fullsize)

![Image](https://images.openai.com/static-rsc-4/Bxe3rJ3Hak1Ko7bDPUFSllGbAN3ZuU0lFqDnh8coMkUwQdWI4cr1VIzC6fIYOd5tNLjQ6T1vHWFX9pMXhDr6sVTF50X6Emre7QMIeXU62qixEbs0TMtZJKIChGtdgscWndK-mo9c5gOYvcPc_RqMizR50mpXSfrRJmHMxKOvPX6RE1n17HvH6RF2VlwWXadC?purpose=fullsize)

---

## 5️⃣ Extra Features You Can Add

✔ Company Logo
✔ Version Number
✔ Progress Bar
✔ Loading Animation GIF
✔ Database Loading Status
✔ Fade In / Fade Out

Example loading message:

```csharp
label1.Text = "Connecting Database...";
```

---

## 6️⃣ If You Want I Can Also Give

* **Professional POS Splash Screen UI**
* **Animated Splash Screen**
* **Splash Screen with Loading Steps**
* **Splash Screen with Fade Effect**
* **Full WinForms POS Starter Template**

Just tell me 👍.
