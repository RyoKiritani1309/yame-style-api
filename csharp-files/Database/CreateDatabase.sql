-- YAME E-Commerce Database Schema for SQL Server
-- Run this script to create the database and tables

USE master;
GO

-- Create database if not exists
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'YameDB')
BEGIN
    CREATE DATABASE YameDB;
END
GO

USE YameDB;
GO

-- Categories Table
CREATE TABLE Categories (
    CategoryId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Slug NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(500),
    ImageUrl NVARCHAR(500),
    ParentCategoryId INT NULL,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (ParentCategoryId) REFERENCES Categories(CategoryId)
);
GO

-- Products Table
CREATE TABLE Products (
    ProductId INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(200) NOT NULL,
    Slug NVARCHAR(200) NOT NULL UNIQUE,
    ShortDescription NVARCHAR(500),
    Description NVARCHAR(MAX),
    Price DECIMAL(18,2) NOT NULL,
    SalePrice DECIMAL(18,2) NULL,
    PrimaryCategoryId INT NOT NULL,
    Availability BIT DEFAULT 1,
    Material NVARCHAR(200),
    MadeIn NVARCHAR(100),
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (PrimaryCategoryId) REFERENCES Categories(CategoryId)
);
GO

-- Product Images Table
CREATE TABLE ProductImages (
    ImageId INT PRIMARY KEY IDENTITY(1,1),
    ProductId INT NOT NULL,
    ImageUrl NVARCHAR(500) NOT NULL,
    IsPrimary BIT DEFAULT 0,
    SortOrder INT DEFAULT 0,
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId) ON DELETE CASCADE
);
GO

-- Product Variants Table
CREATE TABLE ProductVariants (
    VariantId INT PRIMARY KEY IDENTITY(1,1),
    ProductId INT NOT NULL,
    Sku NVARCHAR(50) NOT NULL UNIQUE,
    Size NVARCHAR(20),
    Color NVARCHAR(50),
    Stock INT DEFAULT 0,
    Price DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId) ON DELETE CASCADE
);
GO

-- Product Tags Table
CREATE TABLE Tags (
    TagId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) NOT NULL UNIQUE,
    Slug NVARCHAR(50) NOT NULL UNIQUE
);
GO

-- Product-Tag Junction Table
CREATE TABLE ProductTags (
    ProductId INT NOT NULL,
    TagId INT NOT NULL,
    PRIMARY KEY (ProductId, TagId),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId) ON DELETE CASCADE,
    FOREIGN KEY (TagId) REFERENCES Tags(TagId) ON DELETE CASCADE
);
GO

-- Reviews Table
CREATE TABLE Reviews (
    ReviewId INT PRIMARY KEY IDENTITY(1,1),
    ProductId INT NOT NULL,
    CustomerName NVARCHAR(100),
    Rating INT CHECK (Rating >= 1 AND Rating <= 5),
    Comment NVARCHAR(1000),
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId) ON DELETE CASCADE
);
GO

-- Customers Table
CREATE TABLE Customers (
    CustomerId INT PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(200) NOT NULL UNIQUE,
    FullName NVARCHAR(200),
    Phone NVARCHAR(20),
    CreatedAt DATETIME2 DEFAULT GETDATE()
);
GO

-- Shopping Carts Table
CREATE TABLE Carts (
    CartId NVARCHAR(50) PRIMARY KEY,
    CustomerId INT NULL,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
);
GO

-- Cart Items Table
CREATE TABLE CartItems (
    ItemId INT PRIMARY KEY IDENTITY(1,1),
    CartId NVARCHAR(50) NOT NULL,
    VariantId INT NOT NULL,
    Quantity INT NOT NULL DEFAULT 1,
    UnitPrice DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (CartId) REFERENCES Carts(CartId) ON DELETE CASCADE,
    FOREIGN KEY (VariantId) REFERENCES ProductVariants(VariantId)
);
GO

-- Orders Table
CREATE TABLE Orders (
    OrderId INT PRIMARY KEY IDENTITY(1,1),
    OrderNumber NVARCHAR(50) NOT NULL UNIQUE,
    CustomerId INT NOT NULL,
    SubTotal DECIMAL(18,2) NOT NULL,
    Discount DECIMAL(18,2) DEFAULT 0,
    Shipping DECIMAL(18,2) DEFAULT 0,
    Tax DECIMAL(18,2) DEFAULT 0,
    Total DECIMAL(18,2) NOT NULL,
    Status NVARCHAR(50) DEFAULT 'Pending',
    ShippingAddress NVARCHAR(500),
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
);
GO

-- Order Items Table
CREATE TABLE OrderItems (
    OrderItemId INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT NOT NULL,
    VariantId INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    LineTotal DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId) ON DELETE CASCADE,
    FOREIGN KEY (VariantId) REFERENCES ProductVariants(VariantId)
);
GO

-- Insert Sample Categories
INSERT INTO Categories (Name, Slug, Description, ImageUrl) VALUES
('Áo', 'ao', 'Bộ sưu tập áo thời trang', '/images/category-shirts.jpg'),
('Quần', 'quan', 'Bộ sưu tập quần thời trang', '/images/category-pants.jpg'),
('Giày', 'giay', 'Bộ sưu tập giày dép', '/images/category-shoes.jpg'),
('Phụ kiện', 'phu-kien', 'Phụ kiện thời trang', '/images/category-accessories.jpg');
GO

-- Insert Sample Tags
INSERT INTO Tags (Name, Slug) VALUES
('Mới nhất', 'moi-nhat'),
('Bán chạy', 'ban-chay'),
('Giảm giá', 'giam-gia'),
('Nam', 'nam'),
('Nữ', 'nu'),
('Unisex', 'unisex');
GO

-- Insert Sample Products
INSERT INTO Products (Title, Slug, ShortDescription, Description, Price, SalePrice, PrimaryCategoryId, Material, MadeIn) VALUES
('Áo Polo Basic', 'ao-polo-basic', 'Áo polo nam/nữ phong cách tối giản', 'Áo polo basic với chất liệu cotton cao cấp, form dáng hiện đại, phù hợp cho mọi hoạt động hàng ngày.', 299000, 249000, 1, '100% Cotton', 'Việt Nam'),
('Quần Jean Slim Fit', 'quan-jean-slim-fit', 'Quần jean ôm vừa phải', 'Quần jean slim fit với chất liệu denim cao cấp, co giãn nhẹ, tạo form dáng thon gọn.', 499000, NULL, 2, 'Denim co giãn', 'Việt Nam'),
('Giày Sneaker Trắng', 'giay-sneaker-trang', 'Giày sneaker trắng tối giản', 'Giày sneaker với thiết kế tối giản, màu trắng tinh khôi, dễ dàng phối đồ với mọi trang phục.', 599000, 499000, 3, 'Da tổng hợp + Canvas', 'Việt Nam'),
('Áo Thun Oversized', 'ao-thun-oversized', 'Áo thun form rộng phong cách streetwear', 'Áo thun oversized chất liệu cotton 100%, form rộng thoải mái, phong cách streetwear hiện đại.', 199000, NULL, 1, '100% Cotton', 'Việt Nam');
GO

-- Insert Product Images
INSERT INTO ProductImages (ProductId, ImageUrl, IsPrimary, SortOrder) VALUES
(1, '/images/product-1.jpg', 1, 1),
(2, '/images/product-2.jpg', 1, 1),
(3, '/images/product-3.jpg', 1, 1),
(4, '/images/product-4.jpg', 1, 1);
GO

-- Insert Product Variants
INSERT INTO ProductVariants (ProductId, Sku, Size, Color, Stock, Price) VALUES
(1, 'POLO-BLK-S', 'S', 'Đen', 50, 249000),
(1, 'POLO-BLK-M', 'M', 'Đen', 100, 249000),
(1, 'POLO-BLK-L', 'L', 'Đen', 80, 249000),
(1, 'POLO-WHT-S', 'S', 'Trắng', 40, 249000),
(1, 'POLO-WHT-M', 'M', 'Trắng', 90, 249000),
(2, 'JEAN-BLU-30', '30', 'Xanh đậm', 60, 499000),
(2, 'JEAN-BLU-32', '32', 'Xanh đậm', 80, 499000),
(2, 'JEAN-BLU-34', '34', 'Xanh đậm', 50, 499000),
(3, 'SNK-WHT-39', '39', 'Trắng', 30, 499000),
(3, 'SNK-WHT-40', '40', 'Trắng', 40, 499000),
(3, 'SNK-WHT-41', '41', 'Trắng', 35, 499000),
(3, 'SNK-WHT-42', '42', 'Trắng', 45, 499000),
(4, 'OVER-BLK-M', 'M', 'Đen', 70, 199000),
(4, 'OVER-BLK-L', 'L', 'Đen', 90, 199000),
(4, 'OVER-BLK-XL', 'XL', 'Đen', 60, 199000);
GO

-- Insert Product Tags
INSERT INTO ProductTags (ProductId, TagId) VALUES
(1, 1), (1, 4), (1, 6),
(2, 2), (2, 4),
(3, 1), (3, 3), (3, 6),
(4, 1), (4, 2), (4, 6);
GO

-- Insert Sample Reviews
INSERT INTO Reviews (ProductId, CustomerName, Rating, Comment) VALUES
(1, 'Nguyễn Văn A', 5, 'Sản phẩm rất tốt, chất liệu mát mẻ!'),
(1, 'Trần Thị B', 4, 'Đẹp, form chuẩn'),
(2, 'Lê Văn C', 5, 'Quần jean rất đẹp và bền'),
(3, 'Phạm Thị D', 5, 'Giày đẹp, đi êm chân'),
(3, 'Hoàng Văn E', 4, 'Giá hợp lý, chất lượng tốt'),
(4, 'Đặng Thị F', 5, 'Form đẹp, mặc thoải mái');
GO

-- Create Views for easier querying
CREATE VIEW vw_ProductsWithDetails AS
SELECT 
    p.ProductId,
    p.Title,
    p.Slug,
    p.ShortDescription,
    p.Description,
    p.Price,
    p.SalePrice,
    p.Availability,
    p.Material,
    p.MadeIn,
    c.Name AS CategoryName,
    c.Slug AS CategorySlug,
    (SELECT COUNT(*) FROM Reviews r WHERE r.ProductId = p.ProductId) AS ReviewCount,
    (SELECT AVG(CAST(Rating AS FLOAT)) FROM Reviews r WHERE r.ProductId = p.ProductId) AS AvgRating,
    (SELECT TOP 1 ImageUrl FROM ProductImages pi WHERE pi.ProductId = p.ProductId AND pi.IsPrimary = 1) AS PrimaryImage
FROM Products p
INNER JOIN Categories c ON p.PrimaryCategoryId = c.CategoryId;
GO

PRINT 'Database setup completed successfully!';
