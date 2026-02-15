Perfect 👌 Let’s build a **Professional Database Selection Screen UI** for your POS.

This screen will:

* ✅ Let user choose SQLite / MySQL / MSSQL
* ✅ Enter connection details dynamically
* ✅ Test Connection
* ✅ Save to `config.ini`
* ✅ Start POS after success

Clean + production ready 🔥

---

# 🏗 Final Flow

```
App Start
   ↓
Check config.ini
   ↓
If DB not configured → Open DatabaseSetupForm
   ↓
User selects DB → Test → Save → Continue
```

---

# 1️⃣ DatabaseSetupForm UI Design

Create new form:

```
DatabaseSetupForm.cs
```

---

## 🎨 Add Controls

### 🔹 Top Section

* Label: **Select Database Type**
* ComboBox → `cmbDatabaseType`

```csharp
cmbDatabaseType.Items.AddRange(new object[]
{
    "SQLite",
    "MySQL",
    "MSSQL"
});
```

---

### 🔹 SQLite Panel

Panel: `panelSQLite`

Controls:

* TextBox → `txtSQLitePath`
* Button → `btnBrowseSQLite`

---

### 🔹 MySQL Panel

Panel: `panelMySQL`

Controls:

* txtServer
* txtDatabase
* txtUser
* txtPassword

---

### 🔹 MSSQL Panel

Panel: `panelMSSQL`

Controls:

* txtServer
* txtDatabase
* txtUser
* txtPassword
* Checkbox → Windows Authentication

---

### 🔹 Bottom Buttons

* `btnTestConnection`
* `btnSave`
* `btnCancel`

---

# 2️⃣ Show/Hide Panels Automatically

```csharp
private void cmbDatabaseType_SelectedIndexChanged(object sender, EventArgs e)
{
    panelSQLite.Visible = false;
    panelMySQL.Visible = false;
    panelMSSQL.Visible = false;

    switch (cmbDatabaseType.SelectedItem.ToString())
    {
        case "SQLite":
            panelSQLite.Visible = true;
            break;

        case "MySQL":
            panelMySQL.Visible = true;
            break;

        case "MSSQL":
            panelMSSQL.Visible = true;
            break;
    }
}
```

---

# 3️⃣ Build Connection Strings

```csharp
private string BuildConnectionString()
{
    string type = cmbDatabaseType.SelectedItem.ToString();

    if (type == "SQLite")
    {
        return $"Data Source={txtSQLitePath.Text};Version=3;";
    }
    else if (type == "MySQL")
    {
        return $"server={txtServer.Text};database={txtDatabase.Text};user id={txtUser.Text};password={txtPassword.Text};";
    }
    else // MSSQL
    {
        if (chkWindowsAuth.Checked)
            return $"Server={txtServer.Text};Database={txtDatabase.Text};Trusted_Connection=True;";
        else
            return $"Server={txtServer.Text};Database={txtDatabase.Text};User Id={txtUser.Text};Password={txtPassword.Text};";
    }
}
```

---

# 4️⃣ Test Connection Button

```csharp
private void btnTestConnection_Click(object sender, EventArgs e)
{
    try
    {
        string connectionString = BuildConnectionString();
        string type = cmbDatabaseType.SelectedItem.ToString();

        if (type == "SQLite")
        {
            using (var conn = new System.Data.SQLite.SQLiteConnection(connectionString))
            {
                conn.Open();
            }
        }
        else if (type == "MySQL")
        {
            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
            {
                conn.Open();
            }
        }
        else
        {
            using (var conn = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                conn.Open();
            }
        }

        MessageBox.Show("Connection Successful!", "Success");
    }
    catch (Exception ex)
    {
        MessageBox.Show("Connection Failed: " + ex.Message);
    }
}
```

---

# 5️⃣ Save to config.ini

Using your existing `AppConfig`.

```csharp
private void btnSave_Click(object sender, EventArgs e)
{
    string type = cmbDatabaseType.SelectedItem.ToString();
    string connectionString = BuildConnectionString();

    AppConfig.SaveDatabaseSettings(type, connectionString);

    MessageBox.Show("Database configuration saved!");

    this.DialogResult = DialogResult.OK;
    this.Close();
}
```

---

# 6️⃣ AppConfig Update

Add:

```csharp
public static void SaveDatabaseSettings(string type, string connection)
{
    var parser = new FileIniDataParser();
    IniData data = new IniData();

    data["Database"]["Type"] = type;
    data["Database"]["ConnectionString"] = connection;

    parser.WriteFile(ConfigPath, data);
}
```

---

# 7️⃣ Show Setup On First Run

In `Program.cs`:

```csharp
if (!AppConfig.DatabaseConfigured())
{
    using (DatabaseSetupForm setup = new DatabaseSetupForm())
    {
        if (setup.ShowDialog() != DialogResult.OK)
            return;
    }
}
```

---

# 🎨 Professional UI Tips

Make it modern:

* Use GroupBoxes
* Set fixed dialog
* Center screen
* Disable maximize
* Add logo + POS title
* Add “Test” icon

---

# 🏆 Final Result

When POS starts:

```
---------------------------------
        POS Database Setup
---------------------------------
[ Select Database Type ▼ ]

SQLite
MySQL
MSSQL

[ Test Connection ]
[ Save & Continue ]
---------------------------------
```

Fully dynamic. Clean. Professional.

---

# 🚀 Want Next Level?

I can now build:

* 🔐 Encrypted connection string
* 🌐 Auto-detect SQL Server instances
* 📦 One-click SQLite create DB
* 🏢 Multi-branch DB selector
* ☁ Cloud-hosted database wizard

Tell me your next upgrade 😎
