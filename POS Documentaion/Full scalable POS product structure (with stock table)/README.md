Excellent 🔥
Now I’ll give you a **Full Scalable Retail POS Product Architecture**
(Production Ready – Supports Billing + Stock + Reports + Profit + Multi-Branch Ready)

This is suitable for your:

👉 Laravel + Filament Admin
👉 WinForms Billing App
👉 Future mobile/web expansion

---

# 🧠 CORE POS ARCHITECTURE DESIGN

We will structure like this:

```
Category
Brand
Product
ProductStock
StockMovement
Sale
SaleItem
Customer (optional)
Supplier (optional)
Purchase (optional future)
```

---

# ✅ 1️⃣ Categories Table

```sql
CREATE TABLE categories (
    id BIGINT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(150) NOT NULL,
    created_at TIMESTAMP NULL,
    updated_at TIMESTAMP NULL
);
```

---

# ✅ 2️⃣ Brands Table

```sql
CREATE TABLE brands (
    id BIGINT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(150) NOT NULL,
    created_at TIMESTAMP NULL,
    updated_at TIMESTAMP NULL
);
```

---

# ✅ 3️⃣ Products Table (Master Product Data)

```sql
CREATE TABLE products (
    id BIGINT PRIMARY KEY AUTO_INCREMENT,

    category_id BIGINT,
    brand_id BIGINT,

    name VARCHAR(200) NOT NULL,
    barcode VARCHAR(100) UNIQUE,
    hsn VARCHAR(20),

    cost_price DECIMAL(12,2) DEFAULT 0,
    wholesale_price DECIMAL(12,2) DEFAULT 0,
    retail_price DECIMAL(12,2) DEFAULT 0,

    tax_percent DECIMAL(5,2) DEFAULT 0,

    is_active BOOLEAN DEFAULT 1,

    created_at TIMESTAMP NULL,
    updated_at TIMESTAMP NULL,

    FOREIGN KEY (category_id) REFERENCES categories(id) ON DELETE SET NULL,
    FOREIGN KEY (brand_id) REFERENCES brands(id) ON DELETE SET NULL
);
```

### 🔥 Why Separate Product from Stock?

Because:

* Product = Definition
* Stock = Quantity (changes frequently)

---

# ✅ 4️⃣ Product Stock Table (Current Stock)

```sql
CREATE TABLE product_stocks (
    id BIGINT PRIMARY KEY AUTO_INCREMENT,
    product_id BIGINT NOT NULL,
    current_stock DECIMAL(12,2) DEFAULT 0,
    minimum_stock DECIMAL(12,2) DEFAULT 0,
    created_at TIMESTAMP NULL,
    updated_at TIMESTAMP NULL,

    FOREIGN KEY (product_id) REFERENCES products(id) ON DELETE CASCADE
);
```

### Why separate stock table?

* Fast updates
* Easy multi-warehouse future
* Better reporting
* No locking issues on products table

---

# ✅ 5️⃣ Stock Movements Table (VERY IMPORTANT)

Never directly edit stock. Always track movement.

```sql
CREATE TABLE stock_movements (
    id BIGINT PRIMARY KEY AUTO_INCREMENT,
    product_id BIGINT NOT NULL,
    type ENUM('opening','purchase','sale','adjustment','return'),
    quantity DECIMAL(12,2),
    reference_id BIGINT NULL,
    notes TEXT NULL,
    created_at TIMESTAMP NULL,

    FOREIGN KEY (product_id) REFERENCES products(id) ON DELETE CASCADE
);
```

### 🔥 This gives you:

* Full audit history
* Stock correction tracking
* Safe accounting
* Professional level POS

---

# ✅ 6️⃣ Sales Table

```sql
CREATE TABLE sales (
    id BIGINT PRIMARY KEY AUTO_INCREMENT,

    invoice_no VARCHAR(50),
    customer_id BIGINT NULL,

    sub_total DECIMAL(12,2),
    tax_total DECIMAL(12,2),
    discount_total DECIMAL(12,2),
    grand_total DECIMAL(12,2),

    payment_mode ENUM('cash','upi','card','mixed'),
    created_at TIMESTAMP NULL,
    updated_at TIMESTAMP NULL
);
```

---

# ✅ 7️⃣ Sale Items Table

```sql
CREATE TABLE sale_items (
    id BIGINT PRIMARY KEY AUTO_INCREMENT,

    sale_id BIGINT NOT NULL,
    product_id BIGINT NOT NULL,

    quantity DECIMAL(12,2),
    unit_price DECIMAL(12,2),
    tax_percent DECIMAL(5,2),
    tax_amount DECIMAL(12,2),
    total DECIMAL(12,2),

    created_at TIMESTAMP NULL,

    FOREIGN KEY (sale_id) REFERENCES sales(id) ON DELETE CASCADE,
    FOREIGN KEY (product_id) REFERENCES products(id)
);
```

---

# 🧩 Laravel Model Relationships

## Product.php

```php
public function category()
{
    return $this->belongsTo(Category::class);
}

public function brand()
{
    return $this->belongsTo(Brand::class);
}

public function stock()
{
    return $this->hasOne(ProductStock::class);
}

public function saleItems()
{
    return $this->hasMany(SaleItem::class);
}
```

---

## ProductStock.php

```php
public function product()
{
    return $this->belongsTo(Product::class);
}
```

---

## Sale.php

```php
public function items()
{
    return $this->hasMany(SaleItem::class);
}
```

---

## SaleItem.php

```php
public function sale()
{
    return $this->belongsTo(Sale::class);
}

public function product()
{
    return $this->belongsTo(Product::class);
}
```

---

# 🔥 STOCK UPDATE LOGIC (VERY IMPORTANT)

When Sale Happens:

```
1. Insert sale
2. Insert sale items
3. Reduce product_stocks.current_stock
4. Insert stock_movements record
```

Example:

```php
$productStock->decrement('current_stock', $qty);

StockMovement::create([
    'product_id' => $productId,
    'type' => 'sale',
    'quantity' => -$qty,
    'reference_id' => $saleId
]);
```

---

# 🚀 Why This Structure Is Professional

✔ No direct stock editing
✔ Full audit trail
✔ Easy profit calculation
✔ Easy GST reporting
✔ Easy brand/category reports
✔ Multi-branch ready
✔ Easy future mobile app
✔ Easy accounting integration

---

# 📊 What Reports You Can Now Build

* Daily Sales
* Brand Wise Sales
* Category Wise Sales
* Top Selling Products
* Slow Moving Products
* Stock Ledger
* Profit Report (cost vs selling)
* Low Stock Alert

---

# 🔥 Next Level Upgrade (Optional)

If you want enterprise-ready POS:

* Add `warehouses` table
* Add `purchase` + `purchase_items`
* Add `customers`
* Add `suppliers`
* Add `expense` tracking
* Add `cash_register_sessions` (Z Report style)

---

Since you're building serious POS system,
Next best step is:

👉 Add Purchase Module (to increase stock properly)

or

👉 Build Daily Z Report (Billing Close Report)

Tell me which direction you want 🔥
