# YAME Fashion E-commerce - C# ASP.NET Core

This folder contains all the C# code files for running the YAME fashion website in **Visual Studio 2022**.

## 📁 File Structure

```
csharp-files/
├── Models/
│   ├── Product.cs              # Product model
│   ├── Cart.cs                 # Cart and CartItem models
│   └── DTOs/
│       ├── ProductListResponse.cs
│       └── ProductQuery.cs
├── Controllers/
│   ├── ProductsController.cs   # API controller for products
│   ├── CartController.cs       # API controller for cart
│   ├── HomeController.cs       # MVC controller for homepage
│   └── ProductsViewController.cs # MVC controller for product pages
├── Services/
│   ├── IProductService.cs
│   ├── ProductService.cs       # Product business logic (with mock data)
│   ├── ICartService.cs
│   └── CartService.cs          # Cart business logic
├── Views/
│   ├── Home/
│   │   └── Index.cshtml        # Homepage view
│   └── Products/
│       ├── Index.cshtml        # Product listing page
│       └── Detail.cshtml       # Product detail page
├── Program.cs                  # Application entry point
└── README.md                   # This file
```

## 🚀 How to Use in Visual Studio 2022

### Step 1: Create New ASP.NET Core Project

1. Open **Visual Studio 2022**
2. Create new project → **ASP.NET Core Web API** (or **ASP.NET Core MVC**)
3. Name it: `YameApi`
4. Choose **.NET 8.0** or **.NET 7.0**
5. Enable **"Configure for HTTPS"**
6. Click **Create**

### Step 2: Copy Files to Your Project

Copy the files from this folder structure into your Visual Studio project:

- Copy `Models/` → to your project's `Models/` folder
- Copy `Controllers/` → to your project's `Controllers/` folder
- Copy `Services/` → create a `Services/` folder and copy there
- Copy `Views/` → to your project's `Views/` folder (if using MVC)
- **Replace** `Program.cs` with the provided `Program.cs`

### Step 3: Install Required NuGet Packages

Right-click on your project → **Manage NuGet Packages** → Install:

```
Microsoft.EntityFrameworkCore.SqlServer (if using database later)
Swashbuckle.AspNetCore (Swagger - usually pre-installed)
```

### Step 4: Update Program.cs

The provided `Program.cs` includes:
- ✅ Service registration
- ✅ CORS configuration for frontend
- ✅ Swagger setup
- ✅ Controller mapping

### Step 5: Add Static Files Support (for images)

Create a `wwwroot/images/` folder and add your product images there.

### Step 6: Run the Application

1. Press **F5** or click **▶️ Run**
2. Your API will start at: `https://localhost:7xxx`
3. Swagger UI: `https://localhost:7xxx/swagger`

## 📡 API Endpoints

### Product API (REST)
```
GET  /api/v1/products              # List products (with filters)
GET  /api/v1/products/{id}         # Get product by ID
GET  /api/v1/products/slug/{slug}  # Get product by slug
```

### Cart API (REST)
```
POST   /api/v1/cart                      # Create cart
GET    /api/v1/cart/{cartId}             # Get cart
POST   /api/v1/cart/{cartId}/items       # Add item
PUT    /api/v1/cart/{cartId}/items/{id}  # Update item
DELETE /api/v1/cart/{cartId}/items/{id}  # Remove item
```

### MVC Views (Server-rendered)
```
GET  /                    # Homepage
GET  /Products            # Product listing page
GET  /Products/{slug}     # Product detail page
```

## 🎨 Frontend Integration

If you want to connect the React frontend (from Lovable) to this C# backend:

1. Update the CORS origins in `Program.cs` to match your React dev server
2. In your React app, change API calls to point to: `https://localhost:7xxx/api/v1/`
3. The React app will call the C# API endpoints

## 🔧 Next Steps

### Replace Mock Data with Real Database

1. Install Entity Framework Core:
   ```
   Microsoft.EntityFrameworkCore.SqlServer
   Microsoft.EntityFrameworkCore.Tools
   ```

2. Create `DbContext`:
   ```csharp
   public class YameDbContext : DbContext
   {
       public DbSet<Product> Products { get; set; }
       public DbSet<ProductVariant> Variants { get; set; }
   }
   ```

3. Update `appsettings.json` with connection string
4. Run migrations: `Add-Migration Initial` → `Update-Database`
5. Update `ProductService` to use EF Core instead of mock data

### Add Authentication

1. Install: `Microsoft.AspNetCore.Authentication.JwtBearer`
2. Configure JWT in `Program.cs`
3. Add `[Authorize]` attributes to protected endpoints

### Deploy to Azure/IIS

1. Publish from Visual Studio: **Build** → **Publish**
2. Choose target (Azure App Service, IIS, Folder)
3. Configure environment variables and connection strings

## 📝 Notes

- **Mock Data**: Currently uses in-memory mock data. Replace with database in production.
- **CORS**: Configured for local React development. Update for production.
- **Images**: Place product images in `wwwroot/images/`
- **Session Management**: Cart uses in-memory storage. Use Redis or database in production.

## 🆘 Troubleshooting

**Error: "Cannot find namespace YameApi"**
→ Make sure all files have the correct namespace: `YameApi.Models`, `YameApi.Controllers`, etc.

**Swagger not showing**
→ Make sure you're running in Development mode and accessing `/swagger`

**CORS errors from React**
→ Update the CORS origins in `Program.cs` to match your React dev server port

## 📚 Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Swagger/OpenAPI](https://swagger.io/)

---

**Need help?** The React frontend is still running in Lovable. You can use both together or just use these C# files for a traditional server-rendered MVC app.
