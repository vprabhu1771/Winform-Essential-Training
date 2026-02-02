
## 1️⃣ Create Web API Project

```bash
dotnet new webapi -n EfCoreWebApi
cd EfCoreWebApi
```

### 2️⃣ Add a test root route (optional)

If you want `/` to show “Server is running”, add this to `Program.cs` (for .NET 6/7 minimal API):

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Map controllers
app.MapControllers();

// Add a test route at /
app.MapGet("/", () => "Server is running!");

// Run the app
app.Run();
```

## 3️⃣ Test with Swagger

Run:

```bash
dotnet run
```

Open:

```
https://localhost:xxxx/swagger
```
