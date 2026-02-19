Perfect 👌
I’ll give you **clean production-ready starter code** for your `POS.Core` project.

This will be:

* ✅ Clean
* ✅ Industry scalable
* ✅ Modular ready
* ✅ Easy to extend

Namespace used:

```csharp
namespace POS.Core
```

---

# 📦 1️⃣ ENTITIES

---

## 🛍️ Product.cs

```csharp
using System;

namespace POS.Core.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string HSN { get; set; }

        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }

        public decimal GSTPercent { get; set; }

        public bool IsBatchEnabled { get; set; }
        public bool IsExpiryEnabled { get; set; }
        public bool IsWeighingScaleItem { get; set; }

        public int Stock { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
```

---

## 👤 Customer.cs

```csharp
using System;

namespace POS.Core.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string GSTNumber { get; set; }

        public decimal LoyaltyPoints { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
```

---

## 🚚 Supplier.cs

```csharp
using System;

namespace POS.Core.Entities
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string GSTNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
```

---

## 🧾 Sale.cs

```csharp
using System;
using System.Collections.Generic;

namespace POS.Core.Entities
{
    public class Sale
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }

        public int? CustomerId { get; set; }

        public DateTime SaleDate { get; set; } = DateTime.Now;

        public decimal SubTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal GrandTotal { get; set; }

        public Enums.PaymentMode PaymentMode { get; set; }

        public List<SaleItem> Items { get; set; } = new List<SaleItem>();
    }
}
```

---

## 🧾 SaleItem.cs

```csharp
using System;

namespace POS.Core.Entities
{
    public class SaleItem
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public decimal Discount { get; set; }
        public decimal TaxPercent { get; set; }

        public decimal Total => (Quantity * UnitPrice) - Discount;
    }
}
```

---

## 📦 Purchase.cs

```csharp
using System;

namespace POS.Core.Entities
{
    public class Purchase
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }

        public int SupplierId { get; set; }

        public DateTime PurchaseDate { get; set; } = DateTime.Now;

        public decimal TotalAmount { get; set; }
    }
}
```

---

## 👨‍💼 User.cs

```csharp
namespace POS.Core.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public string Role { get; set; } // Admin, Cashier, Manager

        public bool IsActive { get; set; } = true;
    }
}
```

---

# 📦 2️⃣ ENUMS

---

## 🏢 BusinessType.cs

```csharp
namespace POS.Core.Enums
{
    public enum BusinessType
    {
        Retail = 1,
        Supermarket = 2,
        Pharmacy = 3,
        Restaurant = 4
    }
}
```

---

## 💳 PaymentMode.cs

```csharp
namespace POS.Core.Enums
{
    public enum PaymentMode
    {
        Cash = 1,
        Card = 2,
        UPI = 3,
        Credit = 4
    }
}
```

---

## 🧾 TaxType.cs

```csharp
namespace POS.Core.Enums
{
    public enum TaxType
    {
        Inclusive = 1,
        Exclusive = 2
    }
}
```

---

# 📦 3️⃣ INTERFACES

---

## 📦 IRepository.cs (Generic Repository)

```csharp
using System.Collections.Generic;

namespace POS.Core.Interfaces
{
    public interface IRepository<T>
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
```

---

## 🛍️ IProductService.cs

```csharp
using System.Collections.Generic;
using POS.Core.Entities;

namespace POS.Core.Interfaces
{
    public interface IProductService
    {
        Product GetById(int id);
        Product GetByBarcode(string barcode);
        IEnumerable<Product> GetAll();

        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);

        bool IsStockAvailable(int productId, decimal quantity);
    }
}
```

---

## 🧾 ISaleService.cs

```csharp
using POS.Core.Entities;

namespace POS.Core.Interfaces
{
    public interface ISaleService
    {
        Sale CreateSale(Sale sale);
        Sale GetByInvoice(string invoiceNo);
        void CancelSale(int saleId);
    }
}
```

---

## 📦 IStockService.cs

```csharp
namespace POS.Core.Interfaces
{
    public interface IStockService
    {
        void IncreaseStock(int productId, decimal quantity);
        void DecreaseStock(int productId, decimal quantity);
        decimal GetCurrentStock(int productId);
    }
}
```

---

# 📦 4️⃣ COMMON

---

## 📌 Constants.cs

```csharp
namespace POS.Core.Common
{
    public static class Constants
    {
        public const string DefaultCurrency = "₹";
        public const decimal DefaultGST = 18m;

        public const string AdminRole = "Admin";
        public const string CashierRole = "Cashier";
    }
}
```

---

## 🧠 Helpers.cs

```csharp
using System;

namespace POS.Core.Common
{
    public static class Helpers
    {
        public static string GenerateInvoiceNo()
        {
            return $"INV-{DateTime.Now:yyyyMMddHHmmss}";
        }

        public static decimal CalculateGST(decimal amount, decimal gstPercent)
        {
            return amount * gstPercent / 100m;
        }

        public static decimal CalculateGrandTotal(decimal subtotal, decimal discount, decimal tax)
        {
            return subtotal - discount + tax;
        }
    }
}
```

---

# 🎯 Now You Have:

✔ Clean Core Layer
✔ Entities
✔ Enums
✔ Interfaces
✔ Helpers
✔ Industry-ready foundation

---

# 🚀 Next Step For You

Since you're serious about building this properly, next I recommend:

👉 I’ll give you `POS.Data` implementation with MySQL
👉 Or `SaleService` full business logic implementation
👉 Or Dependency Injection setup in WinForms

Tell me which one you want next 👌
