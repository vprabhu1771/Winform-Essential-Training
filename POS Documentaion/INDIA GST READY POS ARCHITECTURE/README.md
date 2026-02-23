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
