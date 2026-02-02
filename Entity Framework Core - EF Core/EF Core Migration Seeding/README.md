Got it 👍 You want **EF Core migration + seeding a `Category` table** with **`Id, Name, Status`**. Here’s the clean, real-world way to do it.

---

## 1️⃣ Create Category Entity

```csharp
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Status { get; set; }   // true = active, false = inactive
}
```

---

## 2️⃣ Add DbSet in `DbContext`

```csharp
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Electronics", Status = true },
            new Category { Id = 2, Name = "Fashion", Status = true },
            new Category { Id = 3, Name = "Groceries", Status = true },
            new Category { Id = 4, Name = "Books", Status = false }
        );
    }
}
```

⚠️ **Important rules for `HasData`:**

* You **must provide the primary key (`Id`)**
* IDs must be **fixed values**
* No `DateTime.Now`, `Guid.NewGuid()` etc.

---

## 3️⃣ Create Migration

Run this in **Package Manager Console** or terminal:

```bash
dotnet ef migrations add CreateCategoryWithSeed
```

---

## 4️⃣ Apply Migration

```bash
dotnet ef database update
```

---

## 5️⃣ Generated Migration (for reference)

EF Core will generate something like this:

```csharp
migrationBuilder.InsertData(
    table: "Categories",
    columns: new[] { "Id", "Name", "Status" },
    values: new object[,]
    {
        { 1, "Electronics", true },
        { 2, "Fashion", true },
        { 3, "Groceries", true },
        { 4, "Books", false }
    });
```

---

## 6️⃣ Verify in DB

```sql
SELECT * FROM Categories;
```

✔ Data will be inserted automatically during migration.

---

## 💡 Alternative: Seed via Code (Runtime Seed)

If you want **seed only once at startup** (not migration-based), tell me — I’ll show you `DbContext.Database.Migrate()` + conditional seeding.

---

If you want:

* `status` as **enum**
* `IsActive` instead of `Status`
* SQL Server / MySQL / PostgreSQL specific config
  just say the word 👌
