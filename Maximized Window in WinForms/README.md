To open a maximized window in a C# WinForms application, you can set the `WindowState` property of the form to `FormWindowState.Maximized`. This will ensure that the form opens in a maximized state when the application starts.

Here’s how you can do it:

1. In the constructor of your form (`Form1`), set the `WindowState` property to `FormWindowState.Maximized`.

```csharp
public Form1()
{
    InitializeComponent();
    
    // Set the window to open maximized
    this.WindowState = FormWindowState.Maximized;
}
```

This code will maximize the form when it is first shown.

Alternatively, if you want to maximize the form at runtime, you can trigger this action through an event, like clicking a button:

```csharp
private void buttonMaximize_Click(object sender, EventArgs e)
{
    this.WindowState = FormWindowState.Maximized;
}
```

In this case, when the button is clicked, the window will be maximized.