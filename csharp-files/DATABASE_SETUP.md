# YAME Database Setup Guide

This guide explains how to set up the SQL Server database for the YAME E-Commerce application.

## Prerequisites

- SQL Server 2019 or later (or SQL Server Express)
- SQL Server Management Studio (SSMS) or Azure Data Studio
- .NET 8.0 SDK

## Step 1: Install SQL Server

If you don't have SQL Server installed:

### Option A: SQL Server Express (Free, Local Development)
1. Download SQL Server Express from: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
2. Run the installer and choose "Basic" installation
3. Note the connection string provided at the end of installation

### Option B: SQL Server Developer Edition (Free, Full Features)
1. Download from: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
2. Choose "Developer" edition
3. Follow the installation wizard

## Step 2: Run the Database Creation Script

1. Open SQL Server Management Studio (SSMS) or Azure Data Studio
2. Connect to your SQL Server instance
3. Open the file `Database/CreateDatabase.sql`
4. Click "Execute" or press F5 to run the script

The script will:
- Create the `YameDB` database
- Create all necessary tables (Products, Categories, Variants, Users, Orders, etc.)
- Insert sample data for 15 products with images, variants, and reviews
- Create views for easier querying

## Step 3: Configure Connection String

Update your `appsettings.json` file with the database connection string:

```json
{
  "ConnectionStrings": {
    "YameDB": "Server=localhost;Database=YameDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### Connection String Formats:

**For Windows Authentication (Recommended for local development):**
```
Server=localhost;Database=YameDB;Trusted_Connection=True;TrustServerCertificate=True;
```

**For SQL Server Authentication:**
```
Server=localhost;Database=YameDB;User Id=your_username;Password=your_password;TrustServerCertificate=True;
```

**For SQL Server Express:**
```
Server=localhost\\SQLEXPRESS;Database=YameDB;Trusted_Connection=True;TrustServerCertificate=True;
```

## Step 4: Install Required NuGet Package

The application needs the Microsoft SQL Server client library:

```bash
dotnet add package Microsoft.Data.SqlClient
```

## Step 5: Update Program.cs to Use Database

Replace the mock `ProductService` with `ProductServiceDatabase` in your `Program.cs`:

```csharp
// Replace this line:
// builder.Services.AddScoped<IProductService, ProductService>();

// With this line:
builder.Services.AddScoped<IProductService, ProductServiceDatabase>();
```

## Step 6: Verify Database Setup

Run the application and navigate to:
- `/Products` - Should show all 15 products from the database
- `/Products/ao-thun-thermo-mesh` - Should show product detail page
- `/api/v1/products` - Should return JSON list of products

## Database Schema Overview

### Main Tables:
- **Products**: Core product information
- **Categories**: Product categories (Áo, Quần, Giày, Phụ kiện)
- **ProductImages**: Product image URLs
- **ProductVariants**: Size, color, stock, and pricing variants
- **Tags**: Product tags for filtering
- **ProductTags**: Many-to-many relationship between products and tags
- **Reviews**: Customer reviews and ratings
- **Users**: User authentication data
- **Customers**: Customer profile information
- **Carts**: Shopping cart data
- **Orders**: Order information

### Sample Data Included:
- 15 diverse products across 4 categories
- Multiple variants per product (different sizes, colors)
- 15 product images (product-1.jpg through product-15.jpg)
- 28 tags for categorization
- 18 customer reviews

## Troubleshooting

### Error: "Cannot open database 'YameDB'"
- Make sure you ran the CreateDatabase.sql script
- Verify your connection string is correct

### Error: "Login failed for user"
- Check your SQL Server authentication settings
- Ensure Windows Authentication is enabled or use SQL Server Authentication

### Error: "A network-related or instance-specific error"
- Verify SQL Server service is running
- Check if TCP/IP is enabled in SQL Server Configuration Manager
- Confirm the server name in your connection string

### Images Not Displaying
- Ensure all product images (product-1.jpg through product-15.jpg) are in the `wwwroot/images/` folder
- Verify the application is serving static files from wwwroot

## Next Steps

After successful setup:
1. The Products page will display all 15 products from the database
2. Filtering and search will work with real database queries
3. You can add more products by inserting into the database
4. User authentication is ready to use with the Users table

## Production Considerations

For production deployment:
1. Use Azure SQL Database or a dedicated SQL Server instance
2. Implement proper backup strategies
3. Use environment variables for connection strings
4. Enable SSL/TLS for database connections
5. Implement proper error logging
6. Add database migration scripts for schema updates
