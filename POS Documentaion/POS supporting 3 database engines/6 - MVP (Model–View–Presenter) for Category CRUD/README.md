Perfect 👌 Let’s build **MVP (Model–View–Presenter)** for **Category CRUD** properly for your WinForms POS.

Your table:

```sql
CREATE TABLE categories (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);
```

We’ll structure like this:

```
Modules
 └ Categories
     ├ Model
     │   └ Category.cs
     ├ View
     │   ├ ICategoryView.cs
     │   └ CategoryForm.cs
     └ Presenter
         └ CategoryPresenter.cs
```

This is clean MVP.

---

# ✅ 1️⃣ MODEL

📁 `Modules/Categories/Model/Category.cs`

```csharp
namespace winform_bughunt_pos.Modules.Categories.Model
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
```

---

# ✅ 2️⃣ VIEW INTERFACE

📁 `Modules/Categories/View/ICategoryView.cs`

```csharp
using System;
using System.Data;

namespace winform_bughunt_pos.Modules.Categories.View
{
    public interface ICategoryView
    {
        string CategoryName { get; set; }
        int CategoryId { get; set; }

        void SetCategoryList(DataTable table);
        void ClearForm();
        void ShowMessage(string message);

        event EventHandler AddCategory;
        event EventHandler UpdateCategory;
        event EventHandler DeleteCategory;
        event EventHandler LoadCategories;
        event EventHandler SelectCategory;
    }
}
```

---

# ✅ 3️⃣ PRESENTER

📁 `Modules/Categories/Presenter/CategoryPresenter.cs`

```csharp
using System;
using System.Data;
using MySql.Data.MySqlClient;
using winform_bughunt_pos.Modules.Categories.View;

namespace winform_bughunt_pos.Modules.Categories.Presenter
{
    public class CategoryPresenter
    {
        private readonly ICategoryView _view;
        private readonly string _connectionString;

        public CategoryPresenter(ICategoryView view, string connectionString)
        {
            _view = view;
            _connectionString = connectionString;

            _view.AddCategory += OnAddCategory;
            _view.UpdateCategory += OnUpdateCategory;
            _view.DeleteCategory += OnDeleteCategory;
            _view.LoadCategories += OnLoadCategories;
        }

        private void OnLoadCategories(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                string query = "SELECT * FROM categories ORDER BY id DESC";
                MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                _view.SetCategoryList(dt);
            }
        }

        private void OnAddCategory(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_view.CategoryName))
            {
                _view.ShowMessage("Category name required");
                return;
            }

            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                string query = "INSERT INTO categories(name) VALUES(@name)";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@name", _view.CategoryName);
                cmd.ExecuteNonQuery();
            }

            _view.ShowMessage("Category Added");
            _view.ClearForm();
            OnLoadCategories(null, null);
        }

        private void OnUpdateCategory(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                string query = "UPDATE categories SET name=@name WHERE id=@id";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@name", _view.CategoryName);
                cmd.Parameters.AddWithValue("@id", _view.CategoryId);
                cmd.ExecuteNonQuery();
            }

            _view.ShowMessage("Category Updated");
            _view.ClearForm();
            OnLoadCategories(null, null);
        }

        private void OnDeleteCategory(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                string query = "DELETE FROM categories WHERE id=@id";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", _view.CategoryId);
                cmd.ExecuteNonQuery();
            }

            _view.ShowMessage("Category Deleted");
            _view.ClearForm();
            OnLoadCategories(null, null);
        }
    }
}
```

---

# ✅ 4️⃣ FORM (VIEW IMPLEMENTATION)

📁 `Modules/Categories/View/CategoryForm.cs`

```csharp
using System;
using System.Data;
using System.Windows.Forms;
using winform_bughunt_pos.Modules.Categories.Presenter;

namespace winform_bughunt_pos.Modules.Categories.View
{
    public partial class CategoryForm : Form, ICategoryView
    {
        private CategoryPresenter _presenter;

        public CategoryForm()
        {
            InitializeComponent();

            _presenter = new CategoryPresenter(
                this,
                "server=localhost;user id=root;password=1234;database=pos;"
            );

            LoadCategories?.Invoke(this, EventArgs.Empty);
        }

        public string CategoryName
        {
            get => txtName.Text;
            set => txtName.Text = value;
        }

        public int CategoryId { get; set; }

        public event EventHandler AddCategory;
        public event EventHandler UpdateCategory;
        public event EventHandler DeleteCategory;
        public event EventHandler LoadCategories;
        public event EventHandler SelectCategory;

        public void SetCategoryList(DataTable table)
        {
            dgvCategories.DataSource = table;
        }

        public void ClearForm()
        {
            txtName.Clear();
            CategoryId = 0;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddCategory?.Invoke(this, EventArgs.Empty);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateCategory?.Invoke(this, EventArgs.Empty);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteCategory?.Invoke(this, EventArgs.Empty);
        }

        private void dgvCategories_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCategories.CurrentRow == null) return;

            CategoryId = Convert.ToInt32(dgvCategories.CurrentRow.Cells["id"].Value);
            CategoryName = dgvCategories.CurrentRow.Cells["name"].Value.ToString();
        }
    }
}
```

---

# 🎯 Result

You now have:

✔ Clean MVP
✔ No business logic in Form
✔ Proper separation
✔ Scalable structure

---

# 🚀 Next Level Upgrade (Optional)

If you want professional-level POS:

* 🔄 Add search box
* 📄 Add paging
* 🔐 Add duplicate name validation
* 🗑 Soft delete instead of hard delete
* 🧱 Convert to generic BaseCrudPresenter

---

If you want next:

👉 “Make this generic for all masters (Brand, Unit, Customer)”
or
👉 “Upgrade this to use your IDatabaseService (multi-db ready)”

Tell me 👌
