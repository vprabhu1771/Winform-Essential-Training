Perfect! Let’s build the **POS.Modules.Supermarket** module for your modular POS. This module will handle **barcode scanning, bulk products, promotions, GST per product, and fast checkout**, integrating with your `POS.Core` and `POS.Data` repositories.

---

# 📦 POS.Modules.Supermarket Project Structure

```
POS.Modules.Supermarket
│
├── Interfaces
│   └── ISupermarketService.cs
│
├── Services
│   ├── SupermarketService.cs
│   └── PromotionService.cs
└── Entities
    └── Promotion.cs
```

---

# 1️⃣ Promotion.cs

Represents discounts or offers:

```csharp
using System;

namespace POS.Modules.Supermarket.Entities
{
    public class Promotion
    {
        public int Id { get; set; }
        public string Code { get; set; }         // Promo code
        public string Description { get; set; }
        public decimal DiscountPercent { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsActive { get; set; }
    }
}
```

---

# 2️⃣ ISupermarketService.cs

Interface defining supermarket operations:

```csharp
using POS.Core.Entities;
using POS.Modules.Supermarket.Entities;
using System.Collections.Generic;

namespace POS.Modules.Supermarket.Interfaces
{
    public interface ISupermarketService
    {
        Sale CreateSupermarketSale(Sale sale, string promoCode = null);
        void AddPromotion(Promotion promo);
        IEnumerable<Promotion> GetActivePromotions();
        decimal ApplyPromotion(decimal subtotal, string promoCode);
        Sale ScanProductByBarcode(string barcode, int quantity);
    }
}
```

---

# 3️⃣ PromotionService.cs

Handles active promotions and discounts:

```csharp
using POS.Modules.Supermarket.Entities;
using POS.Data.Context;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace POS.Modules.Supermarket.Services
{
    public class PromotionService
    {
        public void AddPromotion(Promotion promo)
        {
            using var conn = DbConnectionFactory.GetConnection();
            var cmd = new MySqlCommand(
                @"INSERT INTO supermarket_promotions 
                  (code, description, discountPercent, validFrom, validTo, isActive)
                  VALUES (@code, @desc, @disc, @from, @to, @active)", conn);

            cmd.Parameters.AddWithValue("@code", promo.Code);
            cmd.Parameters.AddWithValue("@desc", promo.Description);
            cmd.Parameters.AddWithValue("@disc", promo.DiscountPercent);
            cmd.Parameters.AddWithValue("@from", promo.ValidFrom);
            cmd.Parameters.AddWithValue("@to", promo.ValidTo);
            cmd.Parameters.AddWithValue("@active", promo.IsActive);
            cmd.ExecuteNonQuery();
        }

        public IEnumerable<Promotion> GetActivePromotions()
        {
            var list = new List<Promotion>();
            using var conn = DbConnectionFactory.GetConnection();
            var cmd = new MySqlCommand(
                "SELECT * FROM supermarket_promotions WHERE isActive=1 AND validFrom <= CURDATE() AND validTo >= CURDATE()", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Promotion
                {
                    Id = reader.GetInt32("id"),
                    Code = reader.GetString("code"),
                    Description = reader.GetString("description"),
                    DiscountPercent = reader.GetDecimal("discountPercent"),
                    ValidFrom = reader.GetDateTime("validFrom"),
                    ValidTo = reader.GetDateTime("validTo"),
                    IsActive = reader.GetBoolean("isActive")
                });
            }
            return list;
        }

        public decimal ApplyPromotion(decimal subtotal, Promotion promo)
        {
            if (promo == null || !promo.IsActive) return 0;
            return subtotal * promo.DiscountPercent / 100m;
        }

        public Promotion GetPromotionByCode(string code)
        {
            using var conn = DbConnectionFactory.GetConnection();
            var cmd = new MySqlCommand(
                "SELECT * FROM supermarket_promotions WHERE code=@code AND isActive=1", conn);
            cmd.Parameters.AddWithValue("@code", code);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Promotion
                {
                    Id = reader.GetInt32("id"),
                    Code = reader.GetString("code"),
                    Description = reader.GetString("description"),
                    DiscountPercent = reader.GetDecimal("discountPercent"),
                    ValidFrom = reader.GetDateTime("validFrom"),
                    ValidTo = reader.GetDateTime("validTo"),
                    IsActive = reader.GetBoolean("isActive")
                };
            }
            return null;
        }
    }
}
```

---

# 4️⃣ SupermarketService.cs

Handles **barcode scanning, bulk products, promotions, and checkout**:

```csharp
using POS.Core.Entities;
using POS.Data.Repositories;
using POS.Modules.Supermarket.Entities;
using POS.Modules.Supermarket.Interfaces;
using System;
using System.Collections.Generic;

namespace POS.Modules.Supermarket.Services
{
    public class SupermarketService : ISupermarketService
    {
        private readonly ProductRepository _productRepo;
        private readonly StockRepository _stockRepo;
        private readonly SaleRepository _saleRepo;
        private readonly PromotionService _promoService;

        public SupermarketService()
        {
            _productRepo = new ProductRepository();
            _stockRepo = new StockRepository();
            _saleRepo = new SaleRepository();
            _promoService = new PromotionService();
        }

        public void AddPromotion(Promotion promo) => _promoService.AddPromotion(promo);
        public IEnumerable<Promotion> GetActivePromotions() => _promoService.GetActivePromotions();

        public decimal ApplyPromotion(decimal subtotal, string promoCode)
        {
            var promo = _promoService.GetPromotionByCode(promoCode);
            return _promoService.ApplyPromotion(subtotal, promo);
        }

        public Sale ScanProductByBarcode(string barcode, int quantity)
        {
            var product = _productRepo.GetByBarcode(barcode);
            if (product == null) throw new Exception($"Product with barcode {barcode} not found.");
            if (!_productRepo.IsStockAvailable(product.Id, quantity))
                throw new Exception($"Insufficient stock for {product.ProductName}");

            var saleItem = new SaleItem
            {
                ProductId = product.Id,
                ProductName = product.ProductName,
                Quantity = quantity,
                UnitPrice = product.Rate,
                TaxPercent = product.GstPercent
            };

            return new Sale
            {
                Items = new List<SaleItem> { saleItem },
                SubTotal = saleItem.Quantity * saleItem.UnitPrice,
                TaxAmount = saleItem.Quantity * saleItem.UnitPrice * saleItem.TaxPercent / 100m,
                GrandTotal = saleItem.Quantity * saleItem.UnitPrice * (1 + saleItem.TaxPercent / 100m)
            };
        }

        public Sale CreateSupermarketSale(Sale sale, string promoCode = null)
        {
            // Validate stock
            foreach (var item in sale.Items)
            {
                if (!_productRepo.IsStockAvailable(item.ProductId, item.Quantity))
                    throw new Exception($"Insufficient stock for {item.ProductName}");
            }

            // Calculate subtotal
            sale.SubTotal = 0;
            foreach (var item in sale.Items)
            {
                item.Total = item.Quantity * item.UnitPrice;
                sale.SubTotal += item.Total;
            }

            // Apply promotion
            if (!string.IsNullOrEmpty(promoCode))
            {
                sale.DiscountAmount = ApplyPromotion(sale.SubTotal, promoCode);
            }

            // Calculate tax
            sale.TaxAmount = 0;
            foreach (var item in sale.Items)
            {
                sale.TaxAmount += item.Total * item.TaxPercent / 100m;
            }

            // Grand total
            sale.GrandTotal = sale.SubTotal - sale.DiscountAmount + sale.TaxAmount;

            // Generate invoice
            sale.InvoiceNo = POS.Core.Common.Helpers.GenerateInvoiceNo();

            // Deduct stock
            foreach (var item in sale.Items)
            {
                _stockRepo.DecreaseStock(item.ProductId, item.Quantity);
            }

            // Save sale
            return _saleRepo.CreateSale(sale);
        }
    }
}
```

---

# 5️⃣ Scripts Update (`posdb.sql`)

Add **promotions table**:

```sql
CREATE TABLE IF NOT EXISTS supermarket_promotions (
    id INT AUTO_INCREMENT PRIMARY KEY,
    code VARCHAR(50) UNIQUE,
    description VARCHAR(255),
    discountPercent DECIMAL(5,2),
    validFrom DATE,
    validTo DATE,
    isActive BOOLEAN DEFAULT TRUE
);
```

---

# ✅ Features Implemented

* Barcode scanning for fast checkout
* Stock validation per product
* Bulk product sale
* Promotions/discounts per code
* GST calculation per item
* Invoice generation
* Fully integrated with **POS.Core** and **POS.Data**

---

# 🔹 Example Usage in WinForms

```csharp
var supermarket = new SupermarketService();

// Scan product by barcode
var sale = supermarket.ScanProductByBarcode("1234567890123", 3);

// Apply promotion
var finalSale = supermarket.CreateSupermarketSale(sale, "SUMMER10");
MessageBox.Show($"Invoice {finalSale.InvoiceNo} created! Grand Total: ₹{finalSale.GrandTotal}");
```

---

✅ Now you have **all four modules implemented**:

1. Retail ✅
2. Pharmacy ✅
3. Restaurant ✅
4. Supermarket ✅

Next step, I can create a **full All-in-One WinForms POS template**:

* Module switching at runtime
* Dynamic repository injection
* POS form with main menu for Retail / Pharmacy / Restaurant / Supermarket

Do you want me to do that next?
