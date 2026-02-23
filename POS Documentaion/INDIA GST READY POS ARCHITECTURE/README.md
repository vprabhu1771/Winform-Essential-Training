Excellent 🔥
Now I’ll give you a **Full Production-Level POS Product Structure**
✔ With Stock Management
✔ With Opening Stock
✔ With GST (India compliant)
✔ Retail + Wholesale Ready
✔ Future multi-branch scalable

This design works perfectly for your **Laravel + Filament + WinForms POS** system.

---

# 🇮🇳 INDIA GST READY POS ARCHITECTURE

We must support:

* CGST / SGST (intra-state)
* IGST (inter-state)
* HSN Code
* Tax Slabs (0%, 5%, 12%, 18%, 28%)
* GST Reports
* Tax breakup in invoice
* B2C + B2B ready

---

# 🏗 DATABASE STRUCTURE

---

# 1️⃣ Categories Table

```sql
CREATE TABLE categories (
    id BIGINT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(150) NOT NULL,
    created_at TIMESTAMP NULL,
    updated_at TIMESTAMP NULL
);
```

---

# 2️⃣ Brands Table

```sql
CREATE TABLE brands (
    id BIGINT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(150) NOT NULL,
    created_at TIMESTAMP NULL,
    updated_at TIMESTAMP NULL
);
```

---

# 3️⃣ GST Tax Slabs Table (Important 🔥)

```sql
CREATE TABLE tax_slabs (
    id BIGINT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(50),                -- GST 5%, GST 12% etc
    percentage DECIMAL(5,2),         -- 5.00, 12.00, 18.00
    created_at TIMESTAMP NULL
);
```

Example records:

| id | name    | percentage |
| -- | ------- | ---------- |
| 1  | GST 5%  | 5.00       |
| 2  | GST 12% | 12.00      |
| 3  | GST 18% | 18.00      |

---

# 4️⃣ Products Table (GST Ready)

```sql
CREATE TABLE products (
    id BIGINT PRIMARY KEY AUTO_INCREMENT,

    category_id BIGINT NULL,
    brand_id BIGINT NULL,
    tax_slab_id BIGINT NULL,

    name VARCHAR(200) NOT NULL,
    barcode VARCHAR(100) UNIQUE,
    hsn_code VARCHAR(20),

    cost_price DECIMAL(12,2) DEFAULT 0,
    retail_price DECIMAL(12,2) DEFAULT 0,
    wholesale_price DECIMAL(12,2) DEFAULT 0,

    is_tax_inclusive BOOLEAN DEFAULT 1,

    is_active BOOLEAN DEFAULT 1,

    created_at TIMESTAMP NULL,
    updated_at TIMESTAMP NULL,

    FOREIGN KEY (category_id) REFERENCES categories(id) ON DELETE SET NULL,
    FOREIGN KEY (brand_id) REFERENCES brands(id) ON DELETE SET NULL,
    FOREIGN KEY (tax_slab_id) REFERENCES tax_slabs(id) ON DELETE SET NULL
);
```

---

# 5️⃣ Product Stock Table

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

---

# 6️⃣ Stock Movements (Audit Trail)

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

Opening stock = type = 'opening'

---

# 7️⃣ Sales Table (GST Compliant)

```sql
CREATE TABLE sales (
    id BIGINT PRIMARY KEY AUTO_INCREMENT,

    invoice_no VARCHAR(50),
    customer_name VARCHAR(200) NULL,
    customer_gstin VARCHAR(20) NULL,

    sub_total DECIMAL(12,2),
    cgst_total DECIMAL(12,2),
    sgst_total DECIMAL(12,2),
    igst_total DECIMAL(12,2),

    discount_total DECIMAL(12,2),
    grand_total DECIMAL(12,2),

    payment_mode ENUM('cash','upi','card','mixed'),

    created_at TIMESTAMP NULL,
    updated_at TIMESTAMP NULL
);
```

---

# 8️⃣ Sale Items Table (Tax Split Stored 🔥)

```sql
CREATE TABLE sale_items (
    id BIGINT PRIMARY KEY AUTO_INCREMENT,

    sale_id BIGINT NOT NULL,
    product_id BIGINT NOT NULL,

    quantity DECIMAL(12,2),
    unit_price DECIMAL(12,2),

    tax_percent DECIMAL(5,2),
    cgst DECIMAL(12,2),
    sgst DECIMAL(12,2),
    igst DECIMAL(12,2),

    total DECIMAL(12,2),

    created_at TIMESTAMP NULL,

    FOREIGN KEY (sale_id) REFERENCES sales(id) ON DELETE CASCADE,
    FOREIGN KEY (product_id) REFERENCES products(id)
);
```

---

# 🧠 GST Calculation Logic (India)

### If intra-state:

Split tax:

```
CGST = tax% / 2
SGST = tax% / 2
```

Example:
GST 18% → 9% CGST + 9% SGST

### If inter-state:

```
IGST = full tax%
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

public function taxSlab()
{
    return $this->belongsTo(TaxSlab::class);
}

public function stock()
{
    return $this->hasOne(ProductStock::class);
}

public function stockMovements()
{
    return $this->hasMany(StockMovement::class);
}

public function openingStock()
{
    return $this->hasOne(StockMovement::class)
        ->where('type', 'opening');
}
```

---

# 🧮 GST Calculation Example (Laravel)

```php
$taxPercent = $product->taxSlab->percentage;
$amount = $qty * $unitPrice;

if ($intraState) {
    $cgst = ($amount * ($taxPercent / 2)) / 100;
    $sgst = $cgst;
    $igst = 0;
} else {
    $cgst = 0;
    $sgst = 0;
    $igst = ($amount * $taxPercent) / 100;
}

$total = $amount + $cgst + $sgst + $igst;
```

---

# 📊 What Reports You Can Now Build

✔ GST Summary Report
✔ HSN Wise Report
✔ GSTR-1 Ready Report
✔ Tax Liability Report
✔ Brand Wise GST
✔ Category Wise GST
✔ Profit Report
✔ Stock Ledger
✔ Low Stock Alert

---

# 🏆 Why This Structure Is Professional

✔ GST compliant (India)
✔ Audit friendly
✔ Stock history maintained
✔ Fast POS billing
✔ Multi-branch expandable
✔ B2B ready
✔ Accountant friendly

---

# 🔥 Next Level Upgrade (Enterprise Mode)

If you want full accounting-ready POS:

* Add `companies` table (for GSTIN of your shop)
* Add `states` table (to detect intra/inter automatically)
* Add `customers` with state + GSTIN
* Add `purchase` + `purchase_items`
* Add `expense` tracking
* Add `cash_register_sessions` (Daily Z Report)

---

Since you are building serious POS system,
Next best step:

👉 Add Purchase Module (with GST input credit)
or
👉 Build GST Summary Report Page

Tell me which one you want next 🚀



###

Good 👌 Your schema is already **production-ready POS structure**.
Now I’ll give you:

1. ✅ Sample INSERT data for all master tables
2. ✅ How to insert product with stock
3. ✅ How to save a sale (header + items + stock movement)
4. ✅ Important report queries (Daily, GST, Stock, Profit etc.)

---

# ✅ 1️⃣ Insert Master Data

## Insert Categories

```sql
INSERT INTO categories (name, created_at)
VALUES 
('Groceries', NOW()),
('Beverages', NOW()),
('Snacks', NOW());
```

---

## Insert Brands

```sql
INSERT INTO brands (name, created_at)
VALUES 
('ITC', NOW()),
('HUL', NOW()),
('Coca Cola', NOW());
```

---

## Insert Tax Slabs (GST India)

```sql
INSERT INTO tax_slabs (name, percentage, created_at)
VALUES
('GST 5%', 5.00, NOW()),
('GST 12%', 12.00, NOW()),
('GST 18%', 18.00, NOW());
```

---

# ✅ 2️⃣ Insert Product + Opening Stock

## Insert Product

```sql
INSERT INTO products
(category_id, brand_id, tax_slab_id, name, barcode, hsn_code,
 cost_price, retail_price, wholesale_price,
 is_tax_inclusive, created_at)
VALUES
(1, 1, 1,
 'Aashirvaad Atta 1kg',
 '8901234567890',
 '1101',
 40.00,
 50.00,
 45.00,
 1,
 NOW());
```

---

## Add Opening Stock

```sql
-- Insert into product_stocks
INSERT INTO product_stocks
(product_id, current_stock, minimum_stock, created_at)
VALUES
(1, 100, 10, NOW());

-- Insert stock movement
INSERT INTO stock_movements
(product_id, type, quantity, notes, created_at)
VALUES
(1, 'opening', 100, 'Opening Stock', NOW());
```

---

# ✅ 3️⃣ Save Sale (Billing Logic)

⚠️ IMPORTANT: Always use TRANSACTION in your POS.

---

## Step 1 – Insert Sale Header

```sql
START TRANSACTION;

INSERT INTO sales
(invoice_no, customer_name, sub_total,
 cgst_total, sgst_total, igst_total,
 discount_total, grand_total,
 payment_mode, created_at)
VALUES
('INV-001',
 'Walk-in Customer',
 100.00,
 2.50,
 2.50,
 0.00,
 0.00,
 105.00,
 'cash',
 NOW());

SET @sale_id = LAST_INSERT_ID();
```

---

## Step 2 – Insert Sale Item

```sql
INSERT INTO sale_items
(sale_id, product_id, quantity,
 unit_price, tax_percent,
 cgst, sgst, igst,
 total, created_at)
VALUES
(@sale_id, 1, 2,
 50.00, 5.00,
 2.50, 2.50, 0.00,
 105.00, NOW());
```

---

## Step 3 – Reduce Stock

```sql
UPDATE product_stocks
SET current_stock = current_stock - 2
WHERE product_id = 1;

INSERT INTO stock_movements
(product_id, type, quantity, reference_id, created_at)
VALUES
(1, 'sale', -2, @sale_id, NOW());

COMMIT;
```

---

# ✅ 4️⃣ Important Report Queries

These are production-ready report queries 👇

---

# 📊 1. Daily Sales Report

```sql
SELECT 
    DATE(created_at) AS sale_date,
    COUNT(*) AS total_bills,
    SUM(grand_total) AS total_sales
FROM sales
GROUP BY DATE(created_at)
ORDER BY sale_date DESC;
```

---

# 📊 2. Product Wise Sales Report

```sql
SELECT 
    p.name,
    SUM(si.quantity) AS total_qty,
    SUM(si.total) AS total_amount
FROM sale_items si
JOIN products p ON si.product_id = p.id
GROUP BY p.id
ORDER BY total_qty DESC;
```

---

# 📊 3. GST Report (India GST Split)

```sql
SELECT 
    DATE(created_at) AS date,
    SUM(cgst_total) AS total_cgst,
    SUM(sgst_total) AS total_sgst,
    SUM(igst_total) AS total_igst
FROM sales
GROUP BY DATE(created_at)
ORDER BY date DESC;
```

---

# 📊 4. Current Stock Report

```sql
SELECT 
    p.name,
    ps.current_stock,
    ps.minimum_stock,
    CASE 
        WHEN ps.current_stock <= ps.minimum_stock 
        THEN 'LOW STOCK'
        ELSE 'OK'
    END AS stock_status
FROM product_stocks ps
JOIN products p ON ps.product_id = p.id
ORDER BY ps.current_stock ASC;
```

---

# 📊 5. Profit Report (Very Important)

```sql
SELECT 
    p.name,
    SUM(si.quantity) AS total_qty,
    SUM(si.total) AS total_sales,
    SUM(p.cost_price * si.quantity) AS total_cost,
    (SUM(si.total) - SUM(p.cost_price * si.quantity)) AS profit
FROM sale_items si
JOIN products p ON si.product_id = p.id
GROUP BY p.id
ORDER BY profit DESC;
```

---

# 📊 6. Top Selling Products

```sql
SELECT 
    p.name,
    SUM(si.quantity) AS qty_sold
FROM sale_items si
JOIN products p ON si.product_id = p.id
GROUP BY p.id
ORDER BY qty_sold DESC
LIMIT 10;
```

---

# 🧠 For Your POS (Since you're building WinForms + Laravel)

For clean architecture:

* 👉 Sales table = Header
* 👉 Sale_items = Detail
* 👉 Stock always updated through stock_movements
* 👉 Reports always read from sale_items + sales

Never calculate report from UI — always SQL aggregation.

---

If you want next:

* ✅ Purchase table structure
* ✅ Return handling structure
* ✅ Credit sales & customer ledger
* ✅ Advanced analytics queries
* ✅ Full GST GSTR-1 report query
* ✅ Optimized indexes for performance
* ✅ Stored procedure for saving sale

Tell me which one you need next 🚀
