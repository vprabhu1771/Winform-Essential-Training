```sql
CREATE DATABASE laravel_pos;
USE laravel_pos;


CREATE TABLE categories (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

CREATE TABLE customers (
    id INT AUTO_INCREMENT PRIMARY KEY,
    
    customer_code VARCHAR(20) UNIQUE,   -- Optional: CUST001
    
    name VARCHAR(150) NOT NULL,
    mobile VARCHAR(15) NULL,
    email VARCHAR(100) NULL,
    
    gst_number VARCHAR(20) NULL,            -- For business customers
    address TEXT NULL,
    city VARCHAR(100) NULL,
    state VARCHAR(100) NULL,
    pincode VARCHAR(10) NULL,
    
    opening_balance DECIMAL(12,2) DEFAULT 0.00 NULL,
    
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

INSERT INTO customers (name, mobile)
VALUES ('Walkin', '1234567890');

CREATE TABLE products (
    id INT AUTO_INCREMENT PRIMARY KEY,
    hsn VARCHAR(20),
    barcode VARCHAR(50) UNIQUE,
    product_name VARCHAR(150),
    retail_rate DECIMAL(10,2),
    wholesale_rate DECIMAL(5,2)
);

INSERT INTO products (hsn, barcode, product_name, retail_rate, wholesale_rate)
VALUES 
('1001', '8901765119865', 'Flair Yolo Pen', 10.00, 7.00),
('1002', '8901324580143', 'Natraj 30cm Scale', 10.00, 7.00),
('1003', '8901425022504', 'Camlin 30cm Scale', 10.00, 7.00),
('1004', '8901765094209', 'Hauser XO Kit', 100.00, 50.00);

CREATE TABLE sales (
    id INT AUTO_INCREMENT PRIMARY KEY,
    bill_no VARCHAR(50),
    sale_date DATETIME,
    customer_id INT,
    total_items INT,
    total_amount DECIMAL(10,2),
    discount DECIMAL(10,2),
    grand_total DECIMAL(10,2),
    payment_mode VARCHAR(20),
    cash_amount DECIMAL(10,2) DEFAULT 0,
    card_amount DECIMAL(10,2) DEFAULT 0,
    upi_amount DECIMAL(10,2) DEFAULT 0,
    status VARCHAR(20) DEFAULT 'Completed',

    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

CREATE TABLE sale_items (
    id INT AUTO_INCREMENT PRIMARY KEY,
    sale_id INT,
    product_name VARCHAR(150),
    barcode VARCHAR(50),
    hsn VARCHAR(20),
    qty DECIMAL(10,2),
    rate DECIMAL(10,2),
    discount DECIMAL(10,2),
    total DECIMAL(10,2)
    -- FOREIGN KEY (sale_id) REFERENCES sales(id)
);

ALTER TABLE sale_items
ADD CONSTRAINT fk_sale
FOREIGN KEY (sale_id) REFERENCES sales(id)
ON DELETE CASCADE;


CREATE TABLE activation (
    id INT AUTO_INCREMENT PRIMARY KEY,
    serial_key VARCHAR(20),
    is_activated INT DEFAULT(0)
);

```

🔥 Proper Payment Mode popup (Cash / UPI / Card / Credit)

🔥 Thermal receipt design (80mm)

🔥 F6 = Hold bill system

🔥 Complete sale save to MySQL

🔥 F3 = Change Discount popup
