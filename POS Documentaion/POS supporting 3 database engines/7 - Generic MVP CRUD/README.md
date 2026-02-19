Excellent 👌 Now we build **Generic MVP CRUD** so you can reuse for:

* ✅ Category
* ✅ Brand
* ✅ Unit
* ✅ Customer
* ✅ Supplier
* ✅ Any future master

This will make your POS architecture **clean + scalable + enterprise-ready**.

---

# 🎯 FINAL STRUCTURE (Reusable)

```
Core
 ├ Models
 │   └ BaseEntity.cs

Modules
 └ Masters
     ├ Interfaces
     │   └ IMasterView.cs
     ├ Presenters
     │   └ MasterPresenter.cs
     └ Views
         └ MasterForm.cs
```

Now we remove duplication completely.

---

# ✅ 1️⃣ Create Base Entity

📁 `Core/Models/BaseEntity.cs`

```csharp
using System;

namespace winform_bughunt_pos.Core.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
```

All masters use this.

---

# ✅ 2️⃣ Generic View Interface

📁 `Modules/Masters/Interfaces/IMasterView.cs`

```csharp
using System;
using System.Data;

namespace winform_bughunt_pos.Modules.Masters.Interfaces
{
    public interface IMasterView
    {
        string EntityName { get; set; }
        int EntityId { get; set; }

        void SetData(DataTable table);
        void ClearForm();
        void ShowMessage(string message);

        event EventHandler AddClicked;
        event EventHandler UpdateClicked;
        event EventHandler DeleteClicked;
        event EventHandler LoadClicked;
    }
}
```

Now this works for any master table.

---

# ✅ 3️⃣ Generic Master Presenter

📁 `Modules/Masters/Presenters/MasterPresenter.cs`

```csharp
using System;
using System.Data;
using MySql.Data.MySqlClient;
using winform_bughunt_pos.Modules.Masters.Interfaces;

namespace winform_bughunt_pos.Modules.Masters.Presenters
{
    public class MasterPresenter
    {
        private readonly IMasterView _view;
        private readonly string _connectionString;
        private readonly string _tableName;

        public MasterPresenter(IMasterView view, string connectionString, string tableName)
        {
            _view = view;
            _connectionString = connectionString;
            _tableName = tableName;

            _view.AddClicked += OnAdd;
            _view.UpdateClicked += OnUpdate;
            _view.DeleteClicked += OnDelete;
            _view.LoadClicked += OnLoad;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                string query = $"SELECT * FROM {_tableName} ORDER BY id DESC";
                MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                _view.SetData(dt);
            }
        }

        private void OnAdd(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_view.EntityName))
            {
                _view.ShowMessage("Name required");
                return;
            }

            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                string query = $"INSERT INTO {_tableName}(name) VALUES(@name)";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@name", _view.EntityName);
                cmd.ExecuteNonQuery();
            }

            _view.ShowMessage("Saved Successfully");
            _view.ClearForm();
            OnLoad(null, null);
        }

        private void OnUpdate(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                string query = $"UPDATE {_tableName} SET name=@name WHERE id=@id";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@name", _view.EntityName);
                cmd.Parameters.AddWithValue("@id", _view.EntityId);
                cmd.ExecuteNonQuery();
            }

            _view.ShowMessage("Updated Successfully");
            _view.ClearForm();
            OnLoad(null, null);
        }

        private void OnDelete(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                string query = $"DELETE FROM {_tableName} WHERE id=@id";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", _view.EntityId);
                cmd.ExecuteNonQuery();
            }

            _view.ShowMessage("Deleted Successfully");
            _view.ClearForm();
            OnLoad(null, null);
        }
    }
}
```

---

# ✅ 4️⃣ Generic Master Form

📁 `Modules/Masters/Views/MasterForm.cs`

```csharp
using System;
using System.Data;
using System.Windows.Forms;
using winform_bughunt_pos.Modules.Masters.Interfaces;
using winform_bughunt_pos.Modules.Masters.Presenters;

namespace winform_bughunt_pos.Modules.Masters.Views
{
    public partial class MasterForm : Form, IMasterView
    {
        private MasterPresenter _presenter;

        public MasterForm(string tableName, string title)
        {
            InitializeComponent();

            this.Text = title;

            _presenter = new MasterPresenter(
                this,
                "server=localhost;user id=root;password=1234;database=pos;",
                tableName
            );

            LoadClicked?.Invoke(this, EventArgs.Empty);
        }

        public string EntityName
        {
            get => txtName.Text;
            set => txtName.Text = value;
        }

        public int EntityId { get; set; }

        public event EventHandler AddClicked;
        public event EventHandler UpdateClicked;
        public event EventHandler DeleteClicked;
        public event EventHandler LoadClicked;

        public void SetData(DataTable table)
        {
            dgvMaster.DataSource = table;
        }

        public void ClearForm()
        {
            txtName.Clear();
            EntityId = 0;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteClicked?.Invoke(this, EventArgs.Empty);
        }

        private void dgvMaster_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvMaster.CurrentRow == null) return;

            EntityId = Convert.ToInt32(dgvMaster.CurrentRow.Cells["id"].Value);
            EntityName = dgvMaster.CurrentRow.Cells["name"].Value.ToString();
        }
    }
}
```

---

# ✅ 5️⃣ Now Use It Anywhere

### Category

```csharp
new MasterForm("categories", "Category Master").ShowDialog();
```

### Brand

```csharp
new MasterForm("brands", "Brand Master").ShowDialog();
```

### Unit

```csharp
new MasterForm("units", "Unit Master").ShowDialog();
```

### Customer

```csharp
new MasterForm("customers", "Customer Master").ShowDialog();
```

🔥 ZERO duplicate code.

---

# 🚀 What You Achieved

✔ One Presenter for all masters
✔ One Form for all masters
✔ Clean MVP
✔ Scalable
✔ Professional POS architecture

---

# 🏆 Next Level Upgrade (Real Enterprise)

If you want true enterprise:

* 🔐 Add Duplicate Validation
* 🗑 Soft Delete (is_active column)
* 🔍 Add Search
* 📄 Add Paging
* 🔄 Convert to use your IDatabaseService (multi-db ready)
* 🧱 Create BaseCrudPresenter<T>

---

If you say:

> “Make this multi-database ready using IDatabaseService”

I’ll upgrade this to fully enterprise version 👌
