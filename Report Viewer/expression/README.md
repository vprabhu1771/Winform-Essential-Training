# ✅ STEP 2 — Add Page Header

Right click outside body → **Add Page Header**

Add 3 TextBoxes:

### 🟢 Title (Center, Bold, 16pt)

```
BUGHUNT POS
Sales Summary Report
```

### 🟢 Printed Date (Right side)

Expression:

```
="Printed: " & Format(Now(), "dd-MM-yyyy hh:mm tt")
```

# 🔥 BONUS (Highly Recommended for POS)

Add summary text above table:

Textbox expression:

```
="Total Bills: " & Count(Fields!bill_no.Value)
```

And:

```
="Total Sales: ₹ " & Sum(Fields!grand_total.Value)
```

# ✅ BEST METHOD (Recommended – Using Designer)

Instead of editing XML manually:

### Step 1

Open `SalesReport.rdlc` in Designer.

### Step 2

Right click outside body → **Add Page Header**

### Step 3

Drag **Textbox** and add:

```
BUGHUNT POS
Sales Report
```

### Step 4

Add another TextBox for date:

Expression:

```
=Today()
```

Or:

```
="Report Date: " & Format(Today(), "dd-MM-yyyy")
```

# 🔥 If You Want Professional POS Header

Usually POS header includes:

* Shop Name
* Address
* GST Number
* Phone Number
* Date Range
* Logo

Example expression for shop name:

```
="Shop: " & Parameters!ShopName.Value
```