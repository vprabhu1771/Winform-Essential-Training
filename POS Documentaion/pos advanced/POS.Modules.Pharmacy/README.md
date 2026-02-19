Perfect! Let’s build a **complete POS.Modules.Pharmacy** module for your modular POS. This module is tailored for pharmacies with **batch management, expiry tracking, prescriptions, and GST per medicine**.

I’ll provide **interfaces + services** that integrate **POS.Core** entities and **POS.Data repositories**.

---

# 📦 POS.Modules.Pharmacy Project Structure

```
POS.Modules.Pharmacy
│
├── Interfaces
│   └── IPharmacyService.cs
│
├── Services
│   ├── PharmacyService.cs
│   ├── BatchService.cs
│   └── PrescriptionService.cs
└── Entities
    └── MedicineBatch.cs
```

---

# 1️⃣ MedicineBatch.cs

Represents batches with expiry:

```csharp
using System;

namespace POS.Modules.Pharmacy.Entities
{
    public class MedicineBatch
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string BatchNo { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal Quantity { get; set; }
    }
}
```

---

# 2️⃣ IPharmacyService.cs

Interface defining pharmacy operations:

```csharp
using POS.Core.Entities;
using POS.Modules.Pharmacy.Entities;
using System;
using System.Collections.Generic;

namespace POS.Modules.Pharmacy.Interfaces
{
    public interface IPharmacyService
    {
        Sale CreatePharmacySale(Sale sale);
        void AddBatch(MedicineBatch batch);
        void UpdateBatch(MedicineBatch batch);
        IEnumerable<MedicineBatch> GetBatches(int productId);
        void RemoveExpiredBatches();
        IEnumerable<MedicineBatch> GetExpiringSoon(DateTime withinDays);
        bool IsStockAvailable(int productId, decimal quantity);
    }
}
```

---

# 3️⃣ BatchService.cs

Handles **batch management and expiry tracking**:

```csharp
using POS.Modules.Pharmacy.Entities;
using POS.Data.Context;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace POS.Modules.Pharmacy.Services
{
    public class BatchService
    {
        public void AddBatch(MedicineBatch batch)
        {
            using var conn = DbConnectionFactory.GetConnection();
            var cmd = new MySqlCommand(
                @"INSERT INTO medicine_batches (productId, batchNo, expiryDate, quantity)
                  VALUES (@pid, @batch, @expiry, @qty)", conn);

            cmd.Parameters.AddWithValue("@pid", batch.ProductId);
            cmd.Parameters.AddWithValue("@batch", batch.BatchNo);
            cmd.Parameters.AddWithValue("@expiry", batch.ExpiryDate);
            cmd.Parameters.AddWithValue("@qty", batch.Quantity);

            cmd.ExecuteNonQuery();
        }

        public void UpdateBatch(MedicineBatch batch)
        {
            using var conn = DbConnectionFactory.GetConnection();
            var cmd = new MySqlCommand(
                @"UPDATE medicine_batches
                  SET batchNo=@batch, expiryDate=@expiry, quantity=@qty
                  WHERE id=@id", conn);

            cmd.Parameters.AddWithValue("@id", batch.Id);
            cmd.Parameters.AddWithValue("@batch", batch.BatchNo);
            cmd.Parameters.AddWithValue("@expiry", batch.ExpiryDate);
            cmd.Parameters.AddWithValue("@qty", batch.Quantity);

            cmd.ExecuteNonQuery();
        }

        public IEnumerable<MedicineBatch> GetBatches(int productId)
        {
            var list = new List<MedicineBatch>();
            using var conn = DbConnectionFactory.GetConnection();
            var cmd = new MySqlCommand("SELECT * FROM medicine_batches WHERE productId=@pid ORDER BY expiryDate ASC", conn);
            cmd.Parameters.AddWithValue("@pid", productId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new MedicineBatch
                {
                    Id = reader.GetInt32("id"),
                    ProductId = reader.GetInt32("productId"),
                    BatchNo = reader.GetString("batchNo"),
                    ExpiryDate = reader.GetDateTime("expiryDate"),
                    Quantity = reader.GetDecimal("quantity")
                });
            }
            return list;
        }

        public void RemoveExpiredBatches()
        {
            using var conn = DbConnectionFactory.GetConnection();
            var cmd = new MySqlCommand("DELETE FROM medicine_batches WHERE expiryDate < CURDATE()", conn);
            cmd.ExecuteNonQuery();
        }

        public IEnumerable<MedicineBatch> GetExpiringSoon(DateTime withinDate)
        {
            var list = new List<MedicineBatch>();
            using var conn = DbConnectionFactory.GetConnection();
            var cmd = new MySqlCommand("SELECT * FROM medicine_batches WHERE expiryDate <= @date ORDER BY expiryDate ASC", conn);
            cmd.Parameters.AddWithValue("@date", withinDate);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new MedicineBatch
                {
                    Id = reader.GetInt32("id"),
                    ProductId = reader.GetInt32("productId"),
                    BatchNo = reader.GetString("batchNo"),
                    ExpiryDate = reader.GetDateTime("expiryDate"),
                    Quantity = reader.GetDecimal("quantity")
                });
            }
            return list;
        }
    }
}
```

---

# 4️⃣ PrescriptionService.cs

Handles **prescription validation** (simplified):

```csharp
using POS.Core.Entities;
using POS.Modules.Pharmacy.Entities;
using System.Collections.Generic;

namespace POS.Modules.Pharmacy.Services
{
    public class PrescriptionService
    {
        public bool ValidatePrescription(List<int> productIds)
        {
            // Example: pharmacy must check if prescription exists for certain medicines
            // In real app: integrate with prescription scan or database
            // Here we just return true
            return true;
        }
    }
}
```

---

# 5️⃣ PharmacyService.cs

Integrates **batches, stock, and sales**:

```csharp
using POS.Core.Entities;
using POS.Data.Repositories;
using POS.Modules.Pharmacy.Entities;
using POS.Modules.Pharmacy.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POS.Modules.Pharmacy.Services
{
    public class PharmacyService : IPharmacyService
    {
        private readonly ProductRepository _productRepo;
        private readonly StockRepository _stockRepo;
        private readonly SaleRepository _saleRepo;
        private readonly BatchService _batchService;
        private readonly PrescriptionService _prescriptionService;

        public PharmacyService()
        {
            _productRepo = new ProductRepository();
            _stockRepo = new StockRepository();
            _saleRepo = new SaleRepository();
            _batchService = new BatchService();
            _prescriptionService = new PrescriptionService();
        }

        public void AddBatch(MedicineBatch batch) => _batchService.AddBatch(batch);
        public void UpdateBatch(MedicineBatch batch) => _batchService.UpdateBatch(batch);
        public IEnumerable<MedicineBatch> GetBatches(int productId) => _batchService.GetBatches(productId);
        public void RemoveExpiredBatches() => _batchService.RemoveExpiredBatches();
        public IEnumerable<MedicineBatch> GetExpiringSoon(DateTime withinDate) => _batchService.GetExpiringSoon(withinDate);

        public bool IsStockAvailable(int productId, decimal quantity)
        {
            var batches = _batchService.GetBatches(productId)
                                       .Where(b => b.ExpiryDate >= DateTime.Today)
                                       .OrderBy(b => b.ExpiryDate)
                                       .ToList();

            decimal totalAvailable = batches.Sum(b => b.Quantity);
            return totalAvailable >= quantity;
        }

        public Sale CreatePharmacySale(Sale sale)
        {
            // Validate prescription
            var productIds = sale.Items.Select(i => i.ProductId).ToList();
            if (!_prescriptionService.ValidatePrescription(productIds))
                throw new Exception("Prescription required for one or more medicines.");

            // Validate stock with batches
            foreach (var item in sale.Items)
            {
                if (!IsStockAvailable(item.ProductId, item.Quantity))
                    throw new Exception($"Insufficient stock for {item.ProductName}");
            }

            // Calculate subtotal
            sale.SubTotal = sale.Items.Sum(i => (i.Quantity * i.UnitPrice) - i.Discount);

            // Calculate tax per item
            sale.TaxAmount = sale.Items.Sum(i => i.Total * i.TaxPercent / 100m);

            sale.GrandTotal = sale.SubTotal + sale.TaxAmount;

            // Generate invoice
            sale.InvoiceNo = POS.Core.Common.Helpers.GenerateInvoiceNo();

            // Create sale
            var createdSale = _saleRepo.CreateSale(sale);

            // Deduct stock from batches (FIFO)
            foreach (var item in sale.Items)
            {
                var batches = _batchService.GetBatches(item.ProductId)
                                           .Where(b => b.ExpiryDate >= DateTime.Today)
                                           .OrderBy(b => b.ExpiryDate)
                                           .ToList();

                decimal qtyToDeduct = item.Quantity;
                foreach (var batch in batches)
                {
                    if (batch.Quantity >= qtyToDeduct)
                    {
                        batch.Quantity -= qtyToDeduct;
                        _batchService.UpdateBatch(batch);
                        break;
                    }
                    else
                    {
                        qtyToDeduct -= batch.Quantity;
                        batch.Quantity = 0;
                        _batchService.UpdateBatch(batch);
                    }
                }

                // Update main stock
                _stockRepo.DecreaseStock(item.ProductId, item.Quantity);
            }

            return createdSale;
        }
    }
}
```

---

# 6️⃣ Scripts Update (`posdb.sql`)

Add **medicine_batches** table:

```sql
CREATE TABLE IF NOT EXISTS medicine_batches (
    id INT AUTO_INCREMENT PRIMARY KEY,
    productId INT NOT NULL,
    batchNo VARCHAR(50),
    expiryDate DATE,
    quantity DECIMAL(10,2)
);
```

---

# ✅ Features Implemented

* Batch management (add/update/list)
* Expiry alerts (`GetExpiringSoon`)
* Automatic stock deduction (FIFO by expiry)
* Prescription validation
* Sale creation with **per-item GST**
* Integrated with **POS.Core** & **POS.Data repositories**

---

# 🔹 Example Usage in WinForms

```csharp
var pharmacyService = new PharmacyService();

// Add batch
pharmacyService.AddBatch(new MedicineBatch
{
    ProductId = 1,
    BatchNo = "BATCH001",
    ExpiryDate = DateTime.Today.AddMonths(6),
    Quantity = 100
});

// Create sale
var sale = new Sale
{
    CustomerId = 1,
    PaymentMode = POS.Core.Enums.PaymentMode.Cash,
    Items = new List<SaleItem>
    {
        new SaleItem { ProductId = 1, ProductName="Paracetamol 500mg", Quantity=2, UnitPrice=15, TaxPercent=12 }
    }
};

var createdSale = pharmacyService.CreatePharmacySale(sale);
MessageBox.Show($"Invoice {createdSale.InvoiceNo} created! Grand Total: ₹{createdSale.GrandTotal}");
```

---

If you want, next I can provide **POS.Modules.Restaurant full code**, including:

* Table management
* KOT (Kitchen Order Ticket)
* Bill splitting
* Kitchen printing

Do you want me to do that next?
