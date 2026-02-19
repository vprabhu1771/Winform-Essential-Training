Nice, this is where your POS starts getting muscles 💪
Below is **clean, production-ready starter code** for your **`POS.Data`** layer, exactly matching your folder structure.

I’ll keep it **simple, readable, and expandable** (no magic, no over-engineering).

---

# 📁 POS.Data → Context → `DbConnectionFactory.cs`

👉 Single place to manage MySQL connection
👉 Uses `config.ini` / connection string later easily

### ✅ DbConnectionFactory.cs

```csharp
using MySql.Data.MySqlClient;
using System.Configuration;

namespace winform_bughunt_pos.Data.Context
{
    public static class DbConnectionFactory
    {
        private static readonly string _connectionString =
            "Server=localhost;Database=posdb;Uid=root;Pwd=;SslMode=none;";

        public static MySqlConnection CreateConnection()
        {
            var connection = new MySqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
```

👉 Later you can replace `_connectionString` with:

* `config.ini`
* `app.config`
* encrypted license-based config

---

# 📁 POS.Data → Repositories → `ProductRepository.cs`

### ✅ ProductRepository.cs

```csharp
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using winform_bughunt_pos.Core.Entities;
using winform_bughunt_pos.Data.Context;

namespace winform_bughunt_pos.Data.Repositories
{
    public class ProductRepository
    {
        public List<Product> GetAll()
        {
            var products = new List<Product>();

            using (var con = DbConnectionFactory.CreateConnection())
            using (var cmd = new MySqlCommand("SELECT * FROM products", con))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        Id = reader.GetInt32("id"),
                        ProductName = reader.GetString("product_name"),
                        Barcode = reader.GetString("barcode"),
                        Rate = reader.GetDecimal("rate"),
                        GstPercent = reader.GetDecimal("gst_percent"),
                        Stock = reader.GetInt32("stock")
                    });
                }
            }

            return products;
        }

        public void UpdateStock(int productId, int qty)
        {
            using var con = DbConnectionFactory.CreateConnection();
            using var cmd = new MySqlCommand(
                "UPDATE products SET stock = stock - @qty WHERE id = @id", con);

            cmd.Parameters.AddWithValue("@qty", qty);
            cmd.Parameters.AddWithValue("@id", productId);
            cmd.ExecuteNonQuery();
        }
    }
}
```

---

# 📁 POS.Data → Repositories → `CustomerRepository.cs`

### ✅ CustomerRepository.cs

```csharp
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using winform_bughunt_pos.Core.Entities;
using winform_bughunt_pos.Data.Context;

namespace winform_bughunt_pos.Data.Repositories
{
    public class CustomerRepository
    {
        public List<Customer> GetAll()
        {
            var customers = new List<Customer>();

            using var con = DbConnectionFactory.CreateConnection();
            using var cmd = new MySqlCommand("SELECT * FROM customers", con);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                customers.Add(new Customer
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    Mobile = reader.GetString("mobile")
                });
            }

            return customers;
        }
    }
}
```

---

# 📁 POS.Data → Repositories → `SaleRepository.cs`

### ✅ SaleRepository.cs

```csharp
using MySql.Data.MySqlClient;
using winform_bughunt_pos.Core.Entities;
using winform_bughunt_pos.Data.Context;

namespace winform_bughunt_pos.Data.Repositories
{
    public class SaleRepository
    {
        public int CreateSale(Sale sale)
        {
            using var con = DbConnectionFactory.CreateConnection();
            using var tran = con.BeginTransaction();

            try
            {
                var saleCmd = new MySqlCommand(
                    @"INSERT INTO sales (bill_no, sale_date, total_amount)
                      VALUES (@bill, @date, @total);
                      SELECT LAST_INSERT_ID();", con, tran);

                saleCmd.Parameters.AddWithValue("@bill", sale.BillNo);
                saleCmd.Parameters.AddWithValue("@date", sale.SaleDate);
                saleCmd.Parameters.AddWithValue("@total", sale.TotalAmount);

                int saleId = System.Convert.ToInt32(saleCmd.ExecuteScalar());

                foreach (var item in sale.Items)
                {
                    var itemCmd = new MySqlCommand(
                        @"INSERT INTO sale_items (sale_id, product_id, qty, rate)
                          VALUES (@saleId, @productId, @qty, @rate)", con, tran);

                    itemCmd.Parameters.AddWithValue("@saleId", saleId);
                    itemCmd.Parameters.AddWithValue("@productId", item.ProductId);
                    itemCmd.Parameters.AddWithValue("@qty", item.Qty);
                    itemCmd.Parameters.AddWithValue("@rate", item.Rate);
                    itemCmd.ExecuteNonQuery();
                }

                tran.Commit();
                return saleId;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }
    }
}
```

---

# 📁 POS.Data → Repositories → `StockRepository.cs`

### ✅ StockRepository.cs

```csharp
using MySql.Data.MySqlClient;
using winform_bughunt_pos.Data.Context;

namespace winform_bughunt_pos.Data.Repositories
{
    public class StockRepository
    {
        public int GetStock(int productId)
        {
            using var con = DbConnectionFactory.CreateConnection();
            using var cmd = new MySqlCommand(
                "SELECT stock FROM products WHERE id = @id", con);

            cmd.Parameters.AddWithValue("@id", productId);
            return System.Convert.ToInt32(cmd.ExecuteScalar());
        }
    }
}
```

---

# 📁 POS.Data → Scripts → `posdb.sql`

### ✅ posdb.sql (Starter Database)

```sql
CREATE DATABASE IF NOT EXISTS posdb;
USE posdb;

-- PRODUCTS
CREATE TABLE products (
    id INT AUTO_INCREMENT PRIMARY KEY,
    product_name VARCHAR(150),
    barcode VARCHAR(50),
    rate DECIMAL(10,2),
    gst_percent DECIMAL(5,2),
    stock INT DEFAULT 0
);

-- CUSTOMERS
CREATE TABLE customers (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(150),
    mobile VARCHAR(20)
);

-- SALES
CREATE TABLE sales (
    id INT AUTO_INCREMENT PRIMARY KEY,
    bill_no VARCHAR(50),
    sale_date DATETIME,
    total_amount DECIMAL(10,2)
);

-- SALE ITEMS
CREATE TABLE sale_items (
    id INT AUTO_INCREMENT PRIMARY KEY,
    sale_id INT,
    product_id INT,
    qty INT,
    rate DECIMAL(10,2)
);
```

---

# 🧠 Important Rules (Remember This Always)

✔ Repositories = only DB logic
✔ No WinForms code here
✔ No MessageBox here
✔ All validation → Services layer

You’re building this **correctly** 👌
Most POS projects fail because they mix UI + DB.

---

## 👉 Next I strongly recommend:

* `POS.Core.Entities` code
* `POS.Services` (SaleService with GST logic)
* High-speed Billing Form architecture
* Pharmacy Batch + Expiry DB extension

Say the word and we’ll build it step by step 🚀
