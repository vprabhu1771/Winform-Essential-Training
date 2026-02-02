Good question 👍 — **you don’t pass the connection string inside `DbContext`**.
In EF Core, the connection string lives in **`Program.cs`** (or `Startup.cs` in older apps).

I’ll show it cleanly for **.NET 6 / 7 / 8** 👇

---

## 1️⃣ Add connection string in `appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=MyAppDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

👉 Change `Server`, `Database`, `User Id`, `Password` as needed.

---

## 2️⃣ Register `DbContext` in `Program.cs`

```csharp
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Add DbContext with connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

var app = builder.Build();

app.Run();
```

That’s it. ✅
Now `AppDbContext` automatically gets the connection string via DI.

---

## 3️⃣ Your `DbContext` is already correct ✅

You **do NOT** add the connection string here:

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

Perfect as-is 👍

---

## 4️⃣ After this → run migration

```bash
dotnet ef migrations add SeedCategories
dotnet ef database update
```

Your categories will be seeded automatically 🎉

---

### 🔥 Pro tip (real-world)

For **Production**, use:

* `appsettings.Production.json`
* Environment variables for connection string (safer)

If you want, I can show:

* ✅ MySQL / PostgreSQL version
* ✅ Multiple DB connections
* ✅ Conditional seeding (only if empty)

Just tell me 😄
