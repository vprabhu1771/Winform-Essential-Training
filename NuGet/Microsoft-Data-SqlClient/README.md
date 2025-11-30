```
https://www.nuget.org/packages/Microsoft.Data.SqlClient/7.0.0-preview2.25289.6
```

Here is the **quick and correct guide** for using **Microsoft.Data.SqlClient** in C#, including installation, basic connection examples, async queries, parameterized queries, and connection string tips.

---

# ✅ **How to Use Microsoft.Data.SqlClient (Complete Instructions)**

`Microsoft.Data.SqlClient` is the **new recommended SQL Server driver** for .NET (instead of System.Data.SqlClient).

---

# 📌 **1. Install the NuGet Package**

### **Visual Studio**

```
Install-Package Microsoft.Data.SqlClient
```

### **.NET CLI**

```
dotnet add package Microsoft.Data.SqlClient
```

---

# 📌 **2. Add Namespace**

```csharp
using Microsoft.Data.SqlClient;
```

---

# 📌 **3. Basic SQL Connection Example**

```csharp
string connStr = "Server=YOUR_SERVER;Database=YOUR_DB;User Id=sa;Password=yourPassword;TrustServerCertificate=True;";

using (SqlConnection conn = new SqlConnection(connStr))
{
    conn.Open();
    MessageBox.Show("Connected!");
}
```

---

# 📌 **4. SELECT Query (Reading Data)**

```csharp
using (SqlConnection conn = new SqlConnection(connStr))
{
    conn.Open();

    string query = "SELECT Id, Name FROM Employees";

    using (SqlCommand cmd = new SqlCommand(query, conn))
    using (SqlDataReader reader = cmd.ExecuteReader())
    {
        while (reader.Read())
        {
            Console.WriteLine($"{reader["Id"]} - {reader["Name"]}");
        }
    }
}
```

---

# 📌 **5. INSERT With Parameters (Recommended)**

```csharp
string sql = "INSERT INTO Employees (Name, Age) VALUES (@name, @age)";

using (SqlConnection conn = new SqlConnection(connStr))
using (SqlCommand cmd = new SqlCommand(sql, conn))
{
    cmd.Parameters.AddWithValue("@name", "John");
    cmd.Parameters.AddWithValue("@age", 30);

    conn.Open();
    cmd.ExecuteNonQuery();
}
```

---

# 📌 **6. UPDATE Example**

```csharp
string sql = "UPDATE Employees SET Age=@age WHERE Id=@id";

using (SqlConnection conn = new SqlConnection(connStr))
using (SqlCommand cmd = new SqlCommand(sql, conn))
{
    cmd.Parameters.AddWithValue("@age", 40);
    cmd.Parameters.AddWithValue("@id", 1);

    conn.Open();
    cmd.ExecuteNonQuery();
}
```

---

# 📌 **7. DELETE Example**

```csharp
string sql = "DELETE FROM Employees WHERE Id=@id";

using (SqlConnection conn = new SqlConnection(connStr))
using (SqlCommand cmd = new SqlCommand(sql, conn))
{
    cmd.Parameters.AddWithValue("@id", 1);

    conn.Open();
    cmd.ExecuteNonQuery();
}
```

---

# 📌 **8. Async Query (Modern Approach)**

```csharp
using (SqlConnection conn = new SqlConnection(connStr))
{
    await conn.OpenAsync();

    string sql = "SELECT COUNT(*) FROM Employees";

    using (SqlCommand cmd = new SqlCommand(sql, conn))
    {
        int count = (int)await cmd.ExecuteScalarAsync();
        Console.WriteLine($"Total Employees: {count}");
    }
}
```

---

# 📌 **9. ExecuteScalar Example (Return single value)**

```csharp
string sql = "SELECT COUNT(*) FROM Employees";

using (SqlConnection conn = new SqlConnection(connStr))
using (SqlCommand cmd = new SqlCommand(sql, conn))
{
    conn.Open();
    int total = (int)cmd.ExecuteScalar();
    MessageBox.Show($"Total: {total}");
}
```

---

# 📌 **10. Connection String Examples**

### **SQL Authentication**

```
Server=localhost;Database=MyDB;User Id=sa;Password=123;TrustServerCertificate=True;
```

### **Windows Authentication**

```
Server=localhost;Database=MyDB;Integrated Security=True;TrustServerCertificate=True;
```

### **LocalDB**

```
Server=(localdb)\MSSQLLocalDB;Database=MyDB;Trusted_Connection=True;
```

---

# 📌 **11. Handling Errors Safely**

```csharp
try
{
    using SqlConnection conn = new SqlConnection(connStr);
    conn.Open();
}
catch (SqlException ex)
{
    MessageBox.Show("Database error: " + ex.Message);
}
```

---

# 📌 If you want, I can generate:

✔️ CRUD functions
✔️ A full Database Helper class
✔️ A WinForms Login example
✔️ Async Repository pattern
✔️ Dapper version (faster)

Just tell me what you need!
