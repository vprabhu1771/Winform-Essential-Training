Here is the **updated full code** including:

✅ **MenuStrip programmatically**
✅ **Keyboard shortcuts (Ctrl+N, Ctrl+I, Ctrl+Q, etc.)**
✅ **Event handlers in separate functions (clean structure)**
✅ **New File → Exit (Ctrl+Q)**

---

# ✅ **Complete C# WinForms Code**

Put this inside your **MainForm.cs**:

```csharp
public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
        CreateMenu();
    }

    private void CreateMenu()
    {
        MenuStrip menuStrip = new MenuStrip();

        //
        // FILE MENU
        //
        ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");

        ToolStripMenuItem exitItem = new ToolStripMenuItem("Exit");
        exitItem.ShortcutKeys = Keys.Control | Keys.Q;
        exitItem.Click += Exit_Click;

        fileMenu.DropDownItems.Add(exitItem);


        //
        // BOOK MENU
        //
        ToolStripMenuItem bookMenu = new ToolStripMenuItem("Book");

        ToolStripMenuItem addNewBook = new ToolStripMenuItem("Add New Book");
        addNewBook.ShortcutKeys = Keys.Control | Keys.N;
        addNewBook.Click += AddNewBook_Click;

        ToolStripMenuItem addNewCategory = new ToolStripMenuItem("Add New Category");
        addNewCategory.Click += AddNewCategory_Click;

        ToolStripMenuItem removeBook = new ToolStripMenuItem("Remove Book");
        removeBook.Click += RemoveBook_Click;

        ToolStripMenuItem removeCategory = new ToolStripMenuItem("Remove Category");
        removeCategory.Click += RemoveCategory_Click;

        bookMenu.DropDownItems.Add(addNewBook);
        bookMenu.DropDownItems.Add(addNewCategory);
        bookMenu.DropDownItems.Add(new ToolStripSeparator());
        bookMenu.DropDownItems.Add(removeBook);
        bookMenu.DropDownItems.Add(removeCategory);


        //
        // ISSUE MENU
        //
        ToolStripMenuItem issueMenu = new ToolStripMenuItem("Issue Details");

        ToolStripMenuItem issueBook = new ToolStripMenuItem("Issue Book");
        issueBook.ShortcutKeys = Keys.Control | Keys.I;
        issueBook.Click += IssueBook_Click;

        ToolStripMenuItem returnBook = new ToolStripMenuItem("Return Book");
        returnBook.ShortcutKeys = Keys.Control | Keys.R;
        returnBook.Click += ReturnBook_Click;

        issueMenu.DropDownItems.Add(issueBook);
        issueMenu.DropDownItems.Add(returnBook);


        //
        // STUDENT DETAILS MENU
        //
        ToolStripMenuItem studentMenu = new ToolStripMenuItem("Student Details");

        ToolStripMenuItem studentForIssue = new ToolStripMenuItem("Student Details for Issue");
        studentForIssue.Click += StudentIssue_Click;

        studentMenu.DropDownItems.Add(studentForIssue);


        //
        // HELP MENU
        //
        ToolStripMenuItem helpMenu = new ToolStripMenuItem("Help");

        ToolStripMenuItem checkUpdate = new ToolStripMenuItem("Check for update");
        checkUpdate.Click += CheckUpdate_Click;

        ToolStripMenuItem about = new ToolStripMenuItem("About");
        about.Click += About_Click;

        helpMenu.DropDownItems.Add(checkUpdate);
        helpMenu.DropDownItems.Add(about);


        //
        // Add all menus to MenuStrip
        //
        menuStrip.Items.Add(fileMenu);
        menuStrip.Items.Add(bookMenu);
        menuStrip.Items.Add(issueMenu);
        menuStrip.Items.Add(studentMenu);
        menuStrip.Items.Add(helpMenu);

        this.MainMenuStrip = menuStrip;
        this.Controls.Add(menuStrip);
    }

    // =============================
    // EVENT HANDLERS (SEPARATE)
    // =============================

    private void Exit_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }

    private void AddNewBook_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Add New Book clicked");
    }

    private void AddNewCategory_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Add New Category clicked");
    }

    private void RemoveBook_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Remove Book clicked");
    }

   private void RemoveCategory_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Remove Category clicked");
    }

    private void IssueBook_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Issue Book clicked");
    }

    private void ReturnBook_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Return Book clicked");
    }

    private void StudentIssue_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Student Details for Issue clicked");
    }

    private void CheckUpdate_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Checking for updates...");
    }

    private void About_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Library Management System v1.0");
    }
}
```

---

# 🎉 **Keyboard Shortcuts Added**

| Action       | Shortcut     |
| ------------ | ------------ |
| Add New Book | **Ctrl + N** |
| Issue Book   | **Ctrl + I** |
| Return Book  | **Ctrl + R** |
| Exit         | **Ctrl + Q** |

---

If you want, I can also add:

✔ Icons for each menu item
✔ Open child forms (MDI) instead of MessageBox
✔ Add separators and styling
✔ Create a reusable MenuStrip builder class

Just tell me!
