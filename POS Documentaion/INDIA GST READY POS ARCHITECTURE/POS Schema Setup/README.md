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
