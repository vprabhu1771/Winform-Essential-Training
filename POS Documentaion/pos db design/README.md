```sql
CREATE DATABASE posdb;
USE posdb;


CREATE TABLE customers (
    id INT AUTO_INCREMENT PRIMARY KEY,
    
    customer_code VARCHAR(20) UNIQUE,   -- Optional: CUST001
    
    name VARCHAR(150) NOT NULL,
    mobile VARCHAR(15),
    email VARCHAR(100),
    
    gst_number VARCHAR(20),             -- For business customers
    address TEXT,
    city VARCHAR(100),
    state VARCHAR(100),
    pincode VARCHAR(10),
    
    opening_balance DECIMAL(12,2) DEFAULT 0.00,
    
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

CREATE TABLE customers (
    id INT AUTO_INCREMENT PRIMARY KEY,
    customer_name VARCHAR(150),
    mobile VARCHAR(10)
);

INSERT INTO customers (customer_name, mobile)
VALUES ('Walkin', '1234567890');

CREATE TABLE products (
    id INT AUTO_INCREMENT PRIMARY KEY,
    hsn VARCHAR(20),
    barcode VARCHAR(50) UNIQUE,
    product_name VARCHAR(150),
    rate DECIMAL(10,2),
    gst_percent DECIMAL(5,2),
    stock INT DEFAULT 0
);

INSERT INTO products (hsn, barcode, product_name, rate, gst_percent, stock)
VALUES 
('1001', '8901765119865', 'Flair Yolo Pen', 10.00, 0.00, 1000),
('1002', '8901324580143', 'Natraj 30cm Scale', 10.00, 0.00, 50),
('1003', '8901425022504', 'Camlin 30cm Scale', 10.00, 0.00, 50),
('1004', '8901765094209', 'Hauser XO Kit', 100.00, 0.00, 50);

CREATE TABLE orders (
    id INT AUTO_INCREMENT PRIMARY KEY,
    invoice_no VARCHAR(50) UNIQUE,
    order_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    
    total_amount DECIMAL(12,2) DEFAULT 0.00,
    total_gst DECIMAL(12,2) DEFAULT 0.00,
    grand_total DECIMAL(12,2) DEFAULT 0.00,
    
    payment_mode VARCHAR(30),  -- Cash / Card / UPI
    paid_amount DECIMAL(12,2) DEFAULT 0.00,
    balance_amount DECIMAL(12,2) DEFAULT 0.00,
    
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

ALTER TABLE orders
ADD customer_id INT NULL,
ADD FOREIGN KEY (customer_id) REFERENCES customers(id);

CREATE TABLE order_items (
    id INT AUTO_INCREMENT PRIMARY KEY,
    
    order_id INT,
    product_id INT,
    
    hsn VARCHAR(20),
    barcode VARCHAR(50),
    product_name VARCHAR(150),
    
    qty INT DEFAULT 1,
    rate DECIMAL(10,2),
    gst_percent DECIMAL(5,2),
    
    gst_amount DECIMAL(10,2),
    total DECIMAL(12,2),
    
    FOREIGN KEY (order_id) REFERENCES orders(id) ON DELETE CASCADE,
    FOREIGN KEY (product_id) REFERENCES products(id)
);

CREATE TABLE sales (
    id INT AUTO_INCREMENT PRIMARY KEY,
    bill_no VARCHAR(20),
    sale_date DATETIME,
    customer_id INT,
    total_items INT,
    total_amount DECIMAL(10,2),
    discount DECIMAL(10,2),
    grand_total DECIMAL(10,2),
    payment_mode VARCHAR(20),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
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
    total DECIMAL(10,2),
    FOREIGN KEY (sale_id) REFERENCES sales(id)
);


```

🔥 Proper Payment Mode popup (Cash / UPI / Card / Credit)

🔥 Thermal receipt design (80mm)

🔥 F6 = Hold bill system

🔥 Complete sale save to MySQL

🔥 F3 = Change Discount popup