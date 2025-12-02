Here is the **complete C# WinForms code** to **add a MenuStrip programmatically** with all the menu items you listed.

---

# ✅ **Add MenuStrip Programmatically (C# WinForms)**

Place this in your **Form_Load** or in the **form constructor after InitializeComponent()**.

```csharp
private void MainForm_Load(object sender, EventArgs e)
{
    // Create MenuStrip
    MenuStrip menuStrip = new MenuStrip();

    //
    // BOOK MENU
    //
    ToolStripMenuItem bookMenu = new ToolStripMenuItem("Book");

    ToolStripMenuItem addNewBook = new ToolStripMenuItem("Add New Book");
    ToolStripMenuItem addNewCategory = new ToolStripMenuItem("Add New Category");
    ToolStripMenuItem removeBook = new ToolStripMenuItem("Remove Book");
    ToolStripMenuItem removeCategory = new ToolStripMenuItem("Remove Category");

    bookMenu.DropDownItems.Add(addNewBook);
    bookMenu.DropDownItems.Add(addNewCategory);
    bookMenu.DropDownItems.Add(new ToolStripSeparator());
    bookMenu.DropDownItems.Add(removeBook);
    bookMenu.DropDownItems.Add(removeCategory);

    //
    // ISSUE DETAILS MENU
    //
    ToolStripMenuItem issueMenu = new ToolStripMenuItem("Issue Details");

    ToolStripMenuItem issueBook = new ToolStripMenuItem("Issue Book");
    ToolStripMenuItem returnBook = new ToolStripMenuItem("Return Book");

    issueMenu.DropDownItems.Add(issueBook);
    issueMenu.DropDownItems.Add(returnBook);

    //
    // STUDENT DETAILS MENU
    //
    ToolStripMenuItem studentMenu = new ToolStripMenuItem("Student Details");

    ToolStripMenuItem studentForIssue = new ToolStripMenuItem("Student Details for Issue");

    studentMenu.DropDownItems.Add(studentForIssue);

    //
    // HELP MENU
    //
    ToolStripMenuItem helpMenu = new ToolStripMenuItem("Help");

    ToolStripMenuItem checkUpdate = new ToolStripMenuItem("Check for update");
    ToolStripMenuItem about = new ToolStripMenuItem("About");

    helpMenu.DropDownItems.Add(checkUpdate);
    helpMenu.DropDownItems.Add(about);

    //
    // Add all menus to MenuStrip
    //
    menuStrip.Items.Add(bookMenu);
    menuStrip.Items.Add(issueMenu);
    menuStrip.Items.Add(studentMenu);
    menuStrip.Items.Add(helpMenu);

    // Add MenuStrip to Form
    this.MainMenuStrip = menuStrip;
    this.Controls.Add(menuStrip);

    //
    // OPTIONAL: Wire up click events
    //
    addNewBook.Click += (s, ev) => MessageBox.Show("Add New Book clicked");
    addNewCategory.Click += (s, ev) => MessageBox.Show("Add New Category clicked");
    removeBook.Click += (s, ev) => MessageBox.Show("Remove Book clicked");
    removeCategory.Click += (s, ev) => MessageBox.Show("Remove Category clicked");
    issueBook.Click += (s, ev) => MessageBox.Show("Issue Book clicked");
    returnBook.Click += (s, ev) => MessageBox.Show("Return Book clicked");
    studentForIssue.Click += (s, ev) => MessageBox.Show("Student Details for Issue clicked");
    checkUpdate.Click += (s, ev) => MessageBox.Show("Checking for updates...");
    about.Click += (s, ev) => MessageBox.Show("Library Management System v1.0");
}
```

---

# ✅ RESULTING MENU STRUCTURE

### **Book**

* Add New Book
* Add New Category
* ————
* Remove Book
* Remove Category

### **Issue Details**

* Issue Book
* Return Book

### **Student Details**

* Student Details for Issue

### **Help**

* Check for update
* About

---

If you want, I can also create:
✔ Event handlers in separate functions
✔ MDI-menu version
✔ Add icons
✔ Keyboard shortcuts (Ctrl+N, Ctrl+I etc.)

Just tell me!
