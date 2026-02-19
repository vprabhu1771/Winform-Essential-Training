Perfect! Let’s build the **POS.Modules.Retail** code for your modular POS. This module will handle **retail-specific features** like billing, discounts, and loyalty points, using the `POS.Core` and `POS.Data` repositories we already created.

I’ll provide a **clean service-layer implementation** that you can inject into WinForms.

---

# 📦 POS.Modules.Retail Project Structure

```
POS.Modules.Retail
│
├── Services
│   ├── RetailBillingService.cs
│   └── RetailDiscountService.cs
└── Interfaces
    └── IRetailService.cs
```

---

# 1️⃣ IRetailService.cs

Defines the **Retail module interface**:

```csharp
using POS.Core.Entities;
using System.Collections.Generic;

namespace POS.Modules.Retail.Interfaces
{
    public interface IRetailService
    {
        Sale CreateRetailSale(Sale sale);
        decimal ApplyDiscount(decimal subtotal, decimal discountPercent);
        decimal RedeemLoyaltyPoints(Customer customer, decimal amount);
        void UpdateLoyaltyPoints(Customer customer, decimal pointsEarned);
    }
}
```

---

# 2️⃣ RetailDiscountService.cs

Handles discounts for retail:

```csharp
using POS.Modules.Retail.Interfaces;

namespace POS.Modules.Retail.Services
{
    public class RetailDiscountService
    {
        /// <summary>
        /// Apply a percentage discount to subtotal
        /// </summary>
        /// <param name="subtotal"></param>
        /// <param name="discountPercent"></param>
        /// <returns></returns>
        public decimal ApplyDiscount(decimal subtotal, decimal discountPercent)
        {
            if (discountPercent < 0 || discountPercent > 100)
                discountPercent = 0;

            return subtotal * discountPercent / 100m;
        }
    }
}
```

---

# 3️⃣ RetailBillingService.cs

Handles **retail sales**, integrates **SaleRepository**, **ProductRepository**, and discounts.

```csharp
using POS.Core.Entities;
using POS.Core.Interfaces;
using POS.Data.Repositories;
using POS.Modules.Retail.Interfaces;
using System;
using System.Collections.Generic;

namespace POS.Modules.Retail.Services
{
    public class RetailBillingService : IRetailService
    {
        private readonly ProductRepository _productRepo;
        private readonly StockRepository _stockRepo;
        private readonly SaleRepository _saleRepo;
        private readonly RetailDiscountService _discountService;

        public RetailBillingService()
        {
            _productRepo = new ProductRepository();
            _stockRepo = new StockRepository();
            _saleRepo = new SaleRepository();
            _discountService = new RetailDiscountService();
        }

        /// <summary>
        /// Create a retail sale (billing)
        /// </summary>
        /// <param name="sale"></param>
        /// <returns></returns>
        public Sale CreateRetailSale(Sale sale)
        {
            // Validate stock for each item
            foreach (var item in sale.Items)
            {
                if (!_productRepo.IsStockAvailable(item.ProductId, item.Quantity))
                    throw new Exception($"Insufficient stock for {item.ProductName}");
            }

            // Calculate subtotal
            sale.SubTotal = 0;
            foreach (var item in sale.Items)
            {
                item.Total = (item.Quantity * item.UnitPrice) - item.Discount;
                sale.SubTotal += item.Total;
            }

            // Apply discount if provided
            if (sale.DiscountAmount > 0)
            {
                sale.DiscountAmount = _discountService.ApplyDiscount(sale.SubTotal, sale.DiscountAmount);
            }

            // Calculate GST
            sale.TaxAmount = 0;
            foreach (var item in sale.Items)
            {
                sale.TaxAmount += item.Total * item.TaxPercent / 100m;
            }

            // Calculate grand total
            sale.GrandTotal = sale.SubTotal - sale.DiscountAmount + sale.TaxAmount;

            // Generate invoice
            sale.InvoiceNo = POS.Core.Common.Helpers.GenerateInvoiceNo();

            // Create sale using SaleRepository
            var createdSale = _saleRepo.CreateSale(sale);

            // Update loyalty points if customer exists
            if (sale.CustomerId.HasValue)
            {
                // Example: 1 point per ₹100 spent
                var pointsEarned = Math.Floor(createdSale.GrandTotal / 100);
                UpdateLoyaltyPoints(new Customer { Id = sale.CustomerId.Value }, pointsEarned);
            }

            return createdSale;
        }

        /// <summary>
        /// Redeem loyalty points for discount
        /// </summary>
        public decimal RedeemLoyaltyPoints(Customer customer, decimal amount)
        {
            // Example: 1 point = 1 ₹
            if (customer.LoyaltyPoints >= amount)
            {
                customer.LoyaltyPoints -= amount;
                return amount;
            }
            else
            {
                var redeemed = customer.LoyaltyPoints;
                customer.LoyaltyPoints = 0;
                return redeemed;
            }
        }

        /// <summary>
        /// Add loyalty points to customer
        /// </summary>
        public void UpdateLoyaltyPoints(Customer customer, decimal pointsEarned)
        {
            var custRepo = new CustomerRepository();
            var existing = custRepo.GetById(customer.Id);
            if (existing != null)
            {
                existing.LoyaltyPoints += pointsEarned;
                custRepo.Update(existing);
            }
        }

        /// <summary>
        /// Apply percentage discount manually
        /// </summary>
        public decimal ApplyDiscount(decimal subtotal, decimal discountPercent)
        {
            return _discountService.ApplyDiscount(subtotal, discountPercent);
        }
    }
}
```

---

# 🔹 Features Implemented

* Create retail sale
* Validate stock before sale
* Apply manual discount
* Calculate subtotal, GST, grand total
* Generate invoice numbers
* Earn & redeem loyalty points
* Works with **POS.Core** entities and **POS.Data** repositories

---

# 🔹 How to Use in WinForms

```csharp
using POS.Modules.Retail.Services;

var retailService = new RetailBillingService();

var sale = new Sale
{
    CustomerId = 1, // optional
    PaymentMode = POS.Core.Enums.PaymentMode.Cash,
    Items = new List<SaleItem>
    {
        new SaleItem { ProductId = 1, ProductName="Soap", Quantity=2, UnitPrice=50, TaxPercent=18 },
        new SaleItem { ProductId = 2, ProductName="Shampoo", Quantity=1, UnitPrice=120, TaxPercent=18 }
    },
    DiscountAmount = 10 // percent
};

var createdSale = retailService.CreateRetailSale(sale);
MessageBox.Show($"Invoice {createdSale.InvoiceNo} created! Grand Total: ₹{createdSale.GrandTotal}");
```

---

✅ This module is **fully self-contained**.

* Later you can **inject it dynamically** in your POS based on **business type**
* Easy to extend for **supermarket**, **pharmacy**, or **restaurant** modules

---

If you want, I can next provide **POS.Modules.Pharmacy full code** including:

* Batch management
* Expiry alerts
* Prescription handling
* GST per medicine

Do you want me to do that next?
