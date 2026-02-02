
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

# Type 1

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Server is running!");

// Sample categories
var categories = new[]
{
    "Monitor", "CPU", "Keyboard", "Mouse", "UPS", "Ink Tank Printer"
};

// Fixed categories endpoint
app.MapGet("/categories", () => categories)
   .WithName("GetCategories"); // renamed for clarity

app.Run();
```

```json
[
  "Monitor",
  "CPU",
  "Keyboard",
  "Mouse",
  "UPS",
  "Ink Tank Printer"
]
```

# Type 2

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Server is running!");

// Sample categories
var categories = new[]
{
    "Monitor", "CPU", "Keyboard", "Mouse", "UPS", "Ink Tank Printer"
};

// Return categories in "data" field
app.MapGet("/categories", () => new { data = categories })
   .WithName("GetCategories");

app.Run();
```

```json
{
  "data": [
    "Monitor",
    "CPU",
    "Keyboard",
    "Mouse",
    "UPS",
    "Ink Tank Printer"
  ]
}
```
