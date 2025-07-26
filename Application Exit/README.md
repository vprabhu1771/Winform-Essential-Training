In a **WinForms application**, to exit the application when a **Button** is clicked, you can use:

---

### ✅ Option 1: `Application.Exit();`

This is the **recommended way** to gracefully shut down the app:

```csharp
private void btnExit_Click(object sender, EventArgs e)
{
    Application.Exit();
}
```

* It **closes all forms** and ends the application.
* Runs any `FormClosing` or `Application.ApplicationExit` logic.

---

### ✅ Option 2: `this.Close();`

This only closes the **current form**:

```csharp
private void btnExit_Click(object sender, EventArgs e)
{
    this.Close();
}
```

* If it's the **main form**, the application will also exit.
* Otherwise, other open forms may remain running.

---

### ✅ Option 3: `Environment.Exit(0);`

This **forcefully exits** the app:

```csharp
private void btnExit_Click(object sender, EventArgs e)
{
    Environment.Exit(0);
}
```

* Use this only if you want to **immediately terminate** the app.
* Skips `FormClosing`, might not dispose resources properly.

---

### 🔧 How to Wire the Event

1. Drag a **Button** to your form (e.g., `btnExit`).
2. Double-click it to auto-generate the `Click` event handler.
3. Add one of the exit codes above inside the method.

---

Let me know if you want to confirm exit with a dialog (`Yes/No`) before exiting.


Here’s how you can show a **confirmation dialog** before exiting the application using a button in **WinForms**:

---

### ✅ Exit with Confirmation (Yes/No Dialog)

```csharp
private void btnExit_Click(object sender, EventArgs e)
{
    DialogResult result = MessageBox.Show(
        "Are you sure you want to exit?", 
        "Confirm Exit", 
        MessageBoxButtons.YesNo, 
        MessageBoxIcon.Question
    );

    if (result == DialogResult.Yes)
    {
        Application.Exit();
    }
}
```

---

### 🔍 Explanation:

* `MessageBox.Show(...)`: Displays a confirmation dialog.
* `MessageBoxButtons.YesNo`: Shows Yes and No buttons.
* `MessageBoxIcon.Question`: Adds a question mark icon.
* `Application.Exit()`: Exits the app only if **Yes** is clicked.

---

Let me know if you'd like to **customize the message box text or icon** or **localize it to another language** like Tamil.
