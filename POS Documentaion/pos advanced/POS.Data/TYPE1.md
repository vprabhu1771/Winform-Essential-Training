Great 👍 Below is **clean, ready-to-use code** for your **`POS.Data`** project, matching exactly this structure:

```
POS.Data
│
├── Context
│   └── DbConnectionFactory.cs
│
├── Repositories
│   ├── ProductRepository.cs
│   ├── SaleRepository.cs
│   ├── CustomerRepository.cs
│   └── StockRepository.cs
│
└── Scripts
    └── posdb.sql
```

Assumptions (based on your earlier POS work):

* ✅ **WinForms**
* ✅ **MySQL**
* ✅ **MySql.Data** NuGet package
* ✅ India GST ready

---

## 📌 1️⃣ Context → `DbConnectionFactory.cs`

**Purpose:** Centralized DB connection (never write connection string in Forms)

```csharp
using MySql.Data.MySqlClient;
using System.Configuration;

namespace POS.Data.Context
{
    public static class DbConnectionFactory
    {
        private static readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["POSDB"].ConnectionString;

        public static MySqlConnection CreateConnection()
        {
            var conn = new MySqlConnection(_connectionString);
            conn.Open();
            return conn;
        }
    }
}
```

### 🔹 App.config (WinForms project)

```xml
<connectionStrings>
  <add name="POSDB"
       connectionString="Server=localhost;Database=posdb;Uid=root;Pwd=;"
       providerName="MySql.Data.MySqlClient"/>
</connectionStrings>
```

---

## 📌 2️⃣ Repositories

---

## 🛒 ProductRepository.cs

```csharp
using MySql.Data.MySqlClient;
using POS.Data.Context;
using System.Collections.Generic;

namespace POS.Data.Repositories
{
    public class ProductRepository
    {
        public List<dynamic> GetAll()
        {
            var list = new List<dynamic>();

            using var con = DbConnectionFactory.CreateConnection();
            string sql = "SELECT * FROM products";

            using var cmd = new MySqlCommand(sql, con);
            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                list.Add(new
                {
                    Id = dr.GetInt32("id"),
                    Name = dr.GetString("product_name"),
                    Rate = dr.GetDecimal("rate"),
                    Gst = dr.GetDecimal("gst_percent"),
                    Stock = dr.GetInt32("stock")
                });
            }
            return list;
        }

        public void Insert(string name, decimal rate, decimal gst)
        {
            using var con = DbConnectionFactory.CreateConnection();
            string sql = @"INSERT INTO products 
                           (product_name, rate, gst_percent) 
                           VALUES (@name,@rate,@gst)";

            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@rate", rate);
            cmd.Parameters.AddWithValue("@gst", gst);
            cmd.ExecuteNonQuery();
        }
    }
}
```

---

## 🧾 SaleRepository.cs

```csharp
using MySql.Data.MySqlClient;
using POS.Data.Context;

namespace POS.Data.Repositories
{
    public class SaleRepository
    {
        public int CreateSale(decimal total, decimal gst, decimal netTotal)
        {
            using var con = DbConnectionFactory.CreateConnection();
            string sql = @"INSERT INTO sales 
                           (total_amount, gst_amount, net_amount)
                           VALUES (@total,@gst,@net);
                           SELECT LAST_INSERT_ID();";

            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@total", total);
            cmd.Parameters.AddWithValue("@gst", gst);
            cmd.Parameters.AddWithValue("@net", netTotal);

            return int.Parse(cmd.ExecuteScalar().ToString());
        }

        public void AddSaleItem(int saleId, int productId, int qty, decimal price)
        {
            using var con = DbConnectionFactory.CreateConnection();
            string sql = @"INSERT INTO sale_items
                           (sale_id, product_id, qty, price)
                           VALUES (@sale,@product,@qty,@price)";

            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@sale", saleId);
            cmd.Parameters.AddWithValue("@product", productId);
            cmd.Parameters.AddWithValue("@qty", qty);
            cmd.Parameters.AddWithValue("@price", price);
            cmd.ExecuteNonQuery();
        }
    }
}
```

---

## 👤 CustomerRepository.cs

```csharp
using MySql.Data.MySqlClient;
using POS.Data.Context;

namespace POS.Data.Repositories
{
    public class CustomerRepository
    {
        public void Insert(string name, string mobile)
        {
            using var con = DbConnectionFactory.CreateConnection();
            string sql = @"INSERT INTO customers (name, mobile)
                           VALUES (@name,@mobile)";

            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@mobile", mobile);
            cmd.ExecuteNonQuery();
        }
    }
}
```

---

## 📦 StockRepository.cs

```csharp
using MySql.Data.MySqlClient;
using POS.Data.Context;

namespace POS.Data.Repositories
{
    public class StockRepository
    {
        public void ReduceStock(int productId, int qty)
        {
            using var con = DbConnectionFactory.CreateConnection();
            string sql = @"UPDATE products 
                           SET stock = stock - @qty 
                           WHERE id = @id";

            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@qty", qty);
            cmd.Parameters.AddWithValue("@id", productId);
            cmd.ExecuteNonQuery();
        }
    }
}
```

---

## 📌 3️⃣ Scripts → `posdb.sql`

```sql
CREATE DATABASE IF NOT EXISTS posdb;
USE posdb;

-- PRODUCTS
CREATE TABLE products (
    id INT AUTO_INCREMENT PRIMARY KEY,
    product_name VARCHAR(150) NOT NULL,
    rate DECIMAL(10,2),
    gst_percent DECIMAL(5,2),
    stock INT DEFAULT 0,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- CUSTOMERS
CREATE TABLE customers (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(150),
    mobile VARCHAR(15),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- SALES
CREATE TABLE sales (
    id INT AUTO_INCREMENT PRIMARY KEY,
    total_amount DECIMAL(10,2),
    gst_amount DECIMAL(10,2),
    net_amount DECIMAL(10,2),
    sale_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- SALE ITEMS
CREATE TABLE sale_items (
    id INT AUTO_INCREMENT PRIMARY KEY,
    sale_id INT,
    product_id INT,
    qty INT,
    price DECIMAL(10,2),
    FOREIGN KEY (sale_id) REFERENCES sales(id),
    FOREIGN KEY (product_id) REFERENCES products(id)
);
```

---

## ✅ What You Have Now

✔ Clean Data Layer
✔ Repositories pattern
✔ MySQL ready
✔ GST friendly
✔ Industry-agnostic (Retail/Pharmacy/Restaurant ready)

This is **real POS-grade structure**, not demo code.

---

## 🔥 Next Logical Steps (Recommended)

1️⃣ `POS.Services` → Billing & GST logic
2️⃣ Transaction handling (Commit / Rollback)
3️⃣ Pharmacy batch tables
4️⃣ Restaurant KOT tables
5️⃣ High-speed billing optimization

Tell me what you want next 🚀
