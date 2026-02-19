Perfect 👌 Now we upgrade your **Generic Master MVP** to be:

> ✅ Multi-Database Ready
> ✅ SQLite / MySQL / SQL Server compatible
> ✅ Using your `IDatabaseService`
> ✅ Clean MVP
> ✅ Future-proof

This is now **real enterprise structure**.

---

# 🎯 Final Clean Structure

```
Core
 ├ Interfaces
 │   └ IDatabaseService.cs
 ├ Database
 │   ├ SQLiteService.cs
 │   ├ MySqlService.cs
 │   ├ SqlServerService.cs
 │   └ DatabaseFactory.cs

Modules
 └ Masters
     ├ Interfaces
     │   └ IMasterView.cs
     ├ Presenters
     │   └ MasterPresenter.cs
     └ Views
         └ MasterForm.cs
```

---

# ✅ 1️⃣ Updated IDatabaseService (Professional Version)

Make sure your interface looks like this:

```csharp
using System.Data;

namespace winform_bughunt_pos.Core.Interfaces
{
    public interface IDatabaseService
    {
        DataTable ExecuteQuery(string query, params IDataParameter[] parameters);
        int ExecuteNonQuery(string query, params IDataParameter[] parameters);

        IDataParameter CreateParameter(string name, object value);
    }
}
```

Why `CreateParameter()`?

Because:

* SQLite uses `SQLiteParameter`
* MySQL uses `MySqlParameter`
* MSSQL uses `SqlParameter`

We must let each service create its own parameter type.

---

# ✅ 2️⃣ Example: MySqlService Implementation

```csharp
public IDataParameter CreateParameter(string name, object value)
{
    return new MySql.Data.MySqlClient.MySqlParameter(name, value);
}
```

SQLiteService:

```csharp
public IDataParameter CreateParameter(string name, object value)
{
    return new System.Data.SQLite.SQLiteParameter(name, value);
}
```

SqlServerService:

```csharp
public IDataParameter CreateParameter(string name, object value)
{
    return new System.Data.SqlClient.SqlParameter(name, value);
}
```

Now fully database-agnostic.

---

# ✅ 3️⃣ Updated Generic MasterPresenter (Multi-DB Ready)

📁 `Modules/Masters/Presenters/MasterPresenter.cs`

```csharp
using System;
using System.Data;
using winform_bughunt_pos.Core.Interfaces;
using winform_bughunt_pos.Modules.Masters.Interfaces;

namespace winform_bughunt_pos.Modules.Masters.Presenters
{
    public class MasterPresenter
    {
        private readonly IMasterView _view;
        private readonly IDatabaseService _db;
        private readonly string _tableName;

        public MasterPresenter(
            IMasterView view,
            IDatabaseService db,
            string tableName)
        {
            _view = view;
            _db = db;
            _tableName = tableName;

            _view.AddClicked += OnAdd;
            _view.UpdateClicked += OnUpdate;
            _view.DeleteClicked += OnDelete;
            _view.LoadClicked += OnLoad;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            string query = $"SELECT * FROM {_tableName} ORDER BY id DESC";
            DataTable dt = _db.ExecuteQuery(query);
            _view.SetData(dt);
        }

        private void OnAdd(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_view.EntityName))
            {
                _view.ShowMessage("Name required");
                return;
            }

            string query = $"INSERT INTO {_tableName} (name) VALUES (@name)";

            _db.ExecuteNonQuery(
                query,
                _db.CreateParameter("@name", _view.EntityName)
            );

            _view.ShowMessage("Saved Successfully");
            _view.ClearForm();
            OnLoad(null, null);
        }

        private void OnUpdate(object sender, EventArgs e)
        {
            string query = $"UPDATE {_tableName} SET name=@name WHERE id=@id";

            _db.ExecuteNonQuery(
                query,
                _db.CreateParameter("@name", _view.EntityName),
                _db.CreateParameter("@id", _view.EntityId)
            );

            _view.ShowMessage("Updated Successfully");
            _view.ClearForm();
            OnLoad(null, null);
        }

        private void OnDelete(object sender, EventArgs e)
        {
            string query = $"DELETE FROM {_tableName} WHERE id=@id";

            _db.ExecuteNonQuery(
                query,
                _db.CreateParameter("@id", _view.EntityId)
            );

            _view.ShowMessage("Deleted Successfully");
            _view.ClearForm();
            OnLoad(null, null);
        }
    }
}
```

🔥 Notice:

* No MySql reference
* No SQLite reference
* No SQL Server reference
* 100% database independent

---

# ✅ 4️⃣ Updated MasterForm (Using DatabaseFactory)

```csharp
using winform_bughunt_pos.Core.Interfaces;
using winform_bughunt_pos.Core.Database;
using winform_bughunt_pos.Enums;

public partial class MasterForm : Form, IMasterView
{
    private MasterPresenter _presenter;

    public MasterForm(string tableName, string title)
    {
        InitializeComponent();
        this.Text = title;

        DatabaseType type = AppConfig.DatabaseType;
        string conn = AppConfig.ConnectionString;

        IDatabaseService db =
            DatabaseFactory.Create(type, conn);

        _presenter = new MasterPresenter(
            this,
            db,
            tableName
        );

        LoadClicked?.Invoke(this, EventArgs.Empty);
    }
}
```

Now switching database is just:

```
Type=SQLite
Type=MySQL
Type=MSSQL
```

No code change.

---

# 🏆 What You Now Have

✔ Generic CRUD
✔ Single Form for all masters
✔ Single Presenter
✔ Fully Multi-Database
✔ Clean MVP
✔ Enterprise-ready

---

# 🚀 What Makes This Enterprise Level

1. Database-agnostic
2. Parameterized queries
3. Clean separation
4. Easily testable
5. Future API compatible

---

# 🎯 Next Upgrade Options

Choose next:

1. 🔄 Add Transaction Support (Atomic Save)
2. 🔍 Add Search + Paging
3. 🗑 Soft Delete Architecture
4. 🧱 Convert to BaseCrudPresenter<T>
5. 🏢 Add Multi-Branch Support

Tell me which level you want next 👌
