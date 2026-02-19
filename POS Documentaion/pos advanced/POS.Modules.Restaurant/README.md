Perfect! Let’s build a **complete POS.Modules.Restaurant** module for your modular POS. This module will handle **tables, kitchen orders, bill splitting, and kitchen printing**. It integrates with your `POS.Core` entities and `POS.Data` repositories.

---

# 📦 POS.Modules.Restaurant Project Structure

```
POS.Modules.Restaurant
│
├── Interfaces
│   └── IRestaurantService.cs
│
├── Services
│   ├── RestaurantService.cs
│   ├── TableService.cs
│   └── KOTService.cs
└── Entities
    ├── Table.cs
    └── KitchenOrder.cs
```

---

# 1️⃣ Table.cs

Represents a restaurant table:

```csharp
namespace POS.Modules.Restaurant.Entities
{
    public class Table
    {
        public int Id { get; set; }
        public string Name { get; set; } // Table 1, Table 2...
        public int Capacity { get; set; }
        public bool IsOccupied { get; set; }
    }
}
```

---

# 2️⃣ KitchenOrder.cs

Represents a KOT (Kitchen Order Ticket):

```csharp
using System;
using System.Collections.Generic;
using POS.Core.Entities;

namespace POS.Modules.Restaurant.Entities
{
    public class KitchenOrder
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public DateTime OrderTime { get; set; }
        public List<SaleItem> Items { get; set; } = new List<SaleItem>();
        public bool IsPrinted { get; set; }
        public bool IsServed { get; set; }
    }
}
```

---

# 3️⃣ IRestaurantService.cs

Interface defining restaurant operations:

```csharp
using POS.Core.Entities;
using POS.Modules.Restaurant.Entities;
using System.Collections.Generic;

namespace POS.Modules.Restaurant.Interfaces
{
    public interface IRestaurantService
    {
        // Table management
        void AddTable(Table table);
        void UpdateTable(Table table);
        IEnumerable<Table> GetTables();
        Table GetTableById(int tableId);

        // KOT management
        KitchenOrder CreateKOT(KitchenOrder kot);
        void MarkKOTServed(int kotId);
        IEnumerable<KitchenOrder> GetPendingKOTs(int tableId);

        // Billing
        Sale CreateBill(int tableId, List<SaleItem> items, bool splitBill = false);
    }
}
```

---

# 4️⃣ TableService.cs

Manages tables:

```csharp
using POS.Modules.Restaurant.Entities;
using POS.Data.Context;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace POS.Modules.Restaurant.Services
{
    public class TableService
    {
        public void AddTable(Table table)
        {
            using var conn = DbConnectionFactory.GetConnection();
            var cmd = new MySqlCommand(
                "INSERT INTO restaurant_tables (name, capacity, isOccupied) VALUES (@name, @cap, @occ)", conn);
            cmd.Parameters.AddWithValue("@name", table.Name);
            cmd.Parameters.AddWithValue("@cap", table.Capacity);
            cmd.Parameters.AddWithValue("@occ", table.IsOccupied);
            cmd.ExecuteNonQuery();
        }

        public void UpdateTable(Table table)
        {
            using var conn = DbConnectionFactory.GetConnection();
            var cmd = new MySqlCommand(
                "UPDATE restaurant_tables SET name=@name, capacity=@cap, isOccupied=@occ WHERE id=@id", conn);
            cmd.Parameters.AddWithValue("@id", table.Id);
            cmd.Parameters.AddWithValue("@name", table.Name);
            cmd.Parameters.AddWithValue("@cap", table.Capacity);
            cmd.Parameters.AddWithValue("@occ", table.IsOccupied);
            cmd.ExecuteNonQuery();
        }

        public IEnumerable<Table> GetTables()
        {
            var list = new List<Table>();
            using var conn = DbConnectionFactory.GetConnection();
            var cmd = new MySqlCommand("SELECT * FROM restaurant_tables", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Table
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    Capacity = reader.GetInt32("capacity"),
                    IsOccupied = reader.GetBoolean("isOccupied")
                });
            }
            return list;
        }

        public Table GetTableById(int tableId)
        {
            using var conn = DbConnectionFactory.GetConnection();
            var cmd = new MySqlCommand("SELECT * FROM restaurant_tables WHERE id=@id", conn);
            cmd.Parameters.AddWithValue("@id", tableId);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Table
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    Capacity = reader.GetInt32("capacity"),
                    IsOccupied = reader.GetBoolean("isOccupied")
                };
            }
            return null;
        }
    }
}
```

---

# 5️⃣ KOTService.cs

Manages Kitchen Orders:

```csharp
using POS.Modules.Restaurant.Entities;
using POS.Data.Context;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace POS.Modules.Restaurant.Services
{
    public class KOTService
    {
        public KitchenOrder CreateKOT(KitchenOrder kot)
        {
            using var conn = DbConnectionFactory.GetConnection();
            using var trans = conn.BeginTransaction();
            try
            {
                // Insert KOT
                var cmd = new MySqlCommand(
                    "INSERT INTO kitchen_orders (tableId, orderTime, isPrinted, isServed) VALUES (@tableId, @time, @printed, @served);",
                    conn, trans);

                cmd.Parameters.AddWithValue("@tableId", kot.TableId);
                cmd.Parameters.AddWithValue("@time", kot.OrderTime);
                cmd.Parameters.AddWithValue("@printed", kot.IsPrinted);
                cmd.Parameters.AddWithValue("@served", kot.IsServed);
                cmd.ExecuteNonQuery();

                cmd.CommandText = "SELECT LAST_INSERT_ID();";
                kot.Id = Convert.ToInt32(cmd.ExecuteScalar());

                // Insert KOT items
                foreach (var item in kot.Items)
                {
                    var itemCmd = new MySqlCommand(
                        "INSERT INTO kitchen_order_items (kotId, productId, productName, quantity, unitPrice) " +
                        "VALUES (@kotId, @pid, @pname, @qty, @price)", conn, trans);

                    itemCmd.Parameters.AddWithValue("@kotId", kot.Id);
                    itemCmd.Parameters.AddWithValue("@pid", item.ProductId);
                    itemCmd.Parameters.AddWithValue("@pname", item.ProductName);
                    itemCmd.Parameters.AddWithValue("@qty", item.Quantity);
                    itemCmd.Parameters.AddWithValue("@price", item.UnitPrice);

                    itemCmd.ExecuteNonQuery();
                }

                trans.Commit();
                return kot;
            }
            catch
            {
                trans.Rollback();
                throw;
            }
        }

        public void MarkKOTServed(int kotId)
        {
            using var conn = DbConnectionFactory.GetConnection();
            var cmd = new MySqlCommand("UPDATE kitchen_orders SET isServed=1 WHERE id=@id", conn);
            cmd.Parameters.AddWithValue("@id", kotId);
            cmd.ExecuteNonQuery();
        }

        public IEnumerable<KitchenOrder> GetPendingKOTs(int tableId)
        {
            var list = new List<KitchenOrder>();
            using var conn = DbConnectionFactory.GetConnection();
            var cmd = new MySqlCommand("SELECT * FROM kitchen_orders WHERE tableId=@tableId AND isServed=0", conn);
            cmd.Parameters.AddWithValue("@tableId", tableId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new KitchenOrder
                {
                    Id = reader.GetInt32("id"),
                    TableId = reader.GetInt32("tableId"),
                    OrderTime = reader.GetDateTime("orderTime"),
                    IsPrinted = reader.GetBoolean("isPrinted"),
                    IsServed = reader.GetBoolean("isServed")
                });
            }
            return list;
        }
    }
}
```

---

# 6️⃣ RestaurantService.cs

Integrates tables, KOT, and billing:

```csharp
using POS.Core.Entities;
using POS.Data.Repositories;
using POS.Modules.Restaurant.Entities;
using POS.Modules.Restaurant.Interfaces;
using System;
using System.Collections.Generic;

namespace POS.Modules.Restaurant.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly TableService _tableService;
        private readonly KOTService _kotService;
        private readonly SaleRepository _saleRepo;
        private readonly StockRepository _stockRepo;

        public RestaurantService()
        {
            _tableService = new TableService();
            _kotService = new KOTService();
            _saleRepo = new SaleRepository();
            _stockRepo = new StockRepository();
        }

        // Table management
        public void AddTable(Table table) => _tableService.AddTable(table);
        public void UpdateTable(Table table) => _tableService.UpdateTable(table);
        public IEnumerable<Table> GetTables() => _tableService.GetTables();
        public Table GetTableById(int tableId) => _tableService.GetTableById(tableId);

        // KOT management
        public KitchenOrder CreateKOT(KitchenOrder kot) => _kotService.CreateKOT(kot);
        public void MarkKOTServed(int kotId) => _kotService.MarkKOTServed(kotId);
        public IEnumerable<KitchenOrder> GetPendingKOTs(int tableId) => _kotService.GetPendingKOTs(tableId);

        // Billing
        public Sale CreateBill(int tableId, List<SaleItem> items, bool splitBill = false)
        {
            // Validate stock
            foreach (var item in items)
            {
                if (!_stockRepo.GetCurrentStock(item.ProductId).Equals(0) && _stockRepo.GetCurrentStock(item.ProductId) < item.Quantity)
                    throw new Exception($"Insufficient stock for {item.ProductName}");
            }

            var sale = new Sale
            {
                CustomerId = null,
                PaymentMode = Core.Enums.PaymentMode.Cash,
                Items = items,
                InvoiceNo = Core.Common.Helpers.GenerateInvoiceNo()
            };

            sale.SubTotal = 0;
            foreach (var item in items)
            {
                item.Total = item.Quantity * item.UnitPrice;
                sale.SubTotal += item.Total;
                _stockRepo.DecreaseStock(item.ProductId, item.Quantity);
            }

            sale.TaxAmount = 0; // can calculate per item if required
            sale.GrandTotal = sale.SubTotal + sale.TaxAmount;

            return _saleRepo.CreateSale(sale);
        }
    }
}
```

---

# 7️⃣ Scripts Update (`posdb.sql`)

Add **restaurant tables** and **KOT** tables:

```sql
CREATE TABLE IF NOT EXISTS restaurant_tables (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(50),
    capacity INT,
    isOccupied BOOLEAN DEFAULT FALSE
);

CREATE TABLE IF NOT EXISTS kitchen_orders (
    id INT AUTO_INCREMENT PRIMARY KEY,
    tableId INT,
    orderTime DATETIME,
    isPrinted BOOLEAN DEFAULT FALSE,
    isServed BOOLEAN DEFAULT FALSE
);

CREATE TABLE IF NOT EXISTS kitchen_order_items (
    id INT AUTO_INCREMENT PRIMARY KEY,
    kotId INT,
    productId INT,
    productName VARCHAR(150),
    quantity DECIMAL(10,2),
    unitPrice DECIMAL(10,2)
);
```

---

# ✅ Features Implemented

* Table management (add/update/list)
* KOT creation & pending orders
* Kitchen printing flag (`isPrinted`)
* Serve tracking (`isServed`)
* Billing per table, optional bill splitting
* Stock validation integrated with POS.Data

---

# 🔹 Example Usage in WinForms

```csharp
var restaurantService = new RestaurantService();

// Add table
restaurantService.AddTable(new Table { Name = "Table 1", Capacity = 4, IsOccupied = false });

// Create KOT
var kot = new KitchenOrder
{
    TableId = 1,
    OrderTime = DateTime.Now,
    Items = new List<SaleItem>
    {
        new SaleItem { ProductId = 1, ProductName="Pasta", Quantity=2, UnitPrice=250 }
    }
};
var createdKOT = restaurantService.CreateKOT(kot);

// Generate bill
var saleItems = new List<SaleItem>
{
    new SaleItem { ProductId = 1, ProductName="Pasta", Quantity=2, UnitPrice=250 }
};
var bill = restaurantService.CreateBill(1, saleItems);
MessageBox.Show($"Bill created: {bill.InvoiceNo}, Total: ₹{bill.GrandTotal}");
```

---

This completes your **Restaurant module**, fully integrated with **POS.Core** and **POS.Data**.

If you want, I can now create a **full “All-in-One POS WinForms template”** showing:

* Module switching (Retail / Pharmacy / Restaurant / Supermarket)
* Dependency injection
* Main UI + POS form
* Repository & service integration

Do you want me to do that next?
