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

-- Users Table (Authentication)
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(200) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(500) NOT NULL,
    FullName NVARCHAR(200),
    Phone NVARCHAR(20),
    IsEmailConfirmed BIT DEFAULT 0,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    LastLoginAt DATETIME2 NULL
);
GO

-- User Roles Table
CREATE TABLE UserRoles (
    RoleId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    Role NVARCHAR(50) NOT NULL CHECK (Role IN ('admin', 'customer', 'moderator')),
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
    UNIQUE (UserId, Role)
);
GO

-- Customers Table (kept for backward compatibility, linked to Users)
CREATE TABLE Customers (
    CustomerId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL UNIQUE,
    Email NVARCHAR(200) NOT NULL UNIQUE,
    FullName NVARCHAR(200),
    Phone NVARCHAR(20),
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE
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
(N'Áo', 'ao', N'Bộ sưu tập áo thời trang', '/images/category-shirts.jpg'),
(N'Quần', 'quan', N'Bộ sưu tập quần thời trang', '/images/category-pants.jpg'),
(N'Giày', 'giay', N'Bộ sưu tập giày dép', '/images/category-shoes.jpg'),
(N'Phụ kiện', 'phu-kien', N'Phụ kiện thời trang', '/images/category-accessories.jpg');
GO

-- Insert Sample Tags (27 tags for diverse categorization)
INSERT INTO Tags (Name, Slug) VALUES
(N'casual', 'casual'),
(N'thermo-mesh', 'thermo-mesh'),
(N'hoodie', 'hoodie'),
(N'premium', 'premium'),
(N'sneaker', 'sneaker'),
(N'classic', 'classic'),
(N'jacket', 'jacket'),
(N'minimal', 'minimal'),
(N'jean', 'jean'),
(N'polo', 'polo'),
(N'short', 'short'),
(N'summer', 'summer'),
(N'tote', 'tote'),
(N'accessory', 'accessory'),
(N'oversized', 'oversized'),
(N'sport', 'sport'),
(N'shirt', 'shirt'),
(N'linen', 'linen'),
(N'jogger', 'jogger'),
(N'backpack', 'backpack'),
(N'travel', 'travel'),
(N'bomber', 'bomber'),
(N'vintage', 'vintage'),
(N'streetwear', 'streetwear'),
(N'active', 'active'),
(N'waterproof', 'waterproof'),
(N'formal', 'formal'),
(N'nam', 'nam');
GO

-- Insert all 15 Products
INSERT INTO Products (Title, Slug, ShortDescription, Description, Price, SalePrice, PrimaryCategoryId, Material, MadeIn) VALUES
(N'Áo Thun Thermo Mesh', 'ao-thun-thermo-mesh', N'Áo thun cao cấp với công nghệ Thermo Mesh thoáng mát', 
 N'Áo thun Thermo Mesh được thiết kế đặc biệt với chất liệu cao cấp, mang lại cảm giác thoáng mát suốt cả ngày. Phù hợp cho mọi hoạt động từ đi làm đến dạo phố.', 
 196200, 196200, 1, 'Thermo Mesh', N'Việt Nam'),
(N'Áo Hoodie Premium', 'ao-hoodie-premium', N'Áo hoodie thời trang, chất liệu cotton cao cấp', 
 N'Áo hoodie được thiết kế hiện đại với chất liệu cotton cao cấp, giữ ấm tốt và thoải mái. Form dáng chuẩn Hàn Quốc, phù hợp cho mọi phong cách.', 
 450000, 380000, 1, 'Cotton Premium', N'Việt Nam'),
(N'Giày Sneaker Classic', 'giay-sneaker-classic', N'Giày sneaker trắng phối màu, êm ái và bền bỉ', 
 N'Giày sneaker thiết kế classic với đế cao su êm ái, chống trơn trượt. Màu trắng dễ phối đồ, phù hợp cho mọi lứa tuổi.', 
 550000, 495000, 3, 'Canvas + Rubber', N'Việt Nam'),
(N'Áo Khoác Gió Minimal', 'ao-khoac-gio-minimal', N'Áo khoác gió nhẹ, chống nước hiệu quả', 
 N'Áo khoác gió với thiết kế tối giản, chất liệu chống nước tốt. Dễ dàng gấp gọn mang theo, là item không thể thiếu trong tủ đồ.', 
 680000, NULL, 1, N'Polyester chống nước', N'Việt Nam'),
(N'Quần Jean Slim Fit', 'quan-jean-slim-fit', N'Quần jean ôm vừa phải, co giãn thoải mái', 
 N'Quần jean slim fit với chất liệu denim cao cấp, co giãn nhẹ, tạo form dáng thon gọn. Thiết kế hiện đại, phù hợp cho nhiều dịp khác nhau.', 
 499000, NULL, 2, N'Denim co giãn', N'Việt Nam'),
(N'Áo Polo Basic', 'ao-polo-basic', N'Áo polo nam/nữ phong cách tối giản', 
 N'Áo polo basic với chất liệu cotton cao cấp, form dáng hiện đại, phù hợp cho mọi hoạt động hàng ngày.', 
 299000, 249000, 1, '100% Cotton', N'Việt Nam'),
(N'Quần Short Kaki', 'quan-short-kaki', N'Quần short kaki thoải mái cho mùa hè', 
 N'Quần short kaki với chất liệu thoáng mát, thiết kế đơn giản nhưng tinh tế. Lựa chọn hoàn hảo cho những ngày hè nóng bức.', 
 350000, 280000, 2, 'Kaki Premium', N'Việt Nam'),
(N'Túi Tote Canvas', 'tui-tote-canvas', N'Túi tote canvas đơn giản, tiện dụng', 
 N'Túi tote canvas thiết kế tối giản, chất liệu bền bỉ. Phù hợp cho đi làm, đi học hay đi chơi.', 
 150000, NULL, 4, 'Canvas', N'Việt Nam'),
(N'Áo Thun Oversized', 'ao-thun-oversized', N'Áo thun form rộng phong cách streetwear', 
 N'Áo thun oversized chất liệu cotton 100%, form rộng thoải mái, phong cách streetwear hiện đại.', 
 199000, NULL, 1, '100% Cotton', N'Việt Nam'),
(N'Giày Sneaker Sport', 'giay-sneaker-sport', N'Giày sneaker thể thao năng động', 
 N'Giày sneaker sport với thiết kế năng động, đế êm ái hỗ trợ tốt cho các hoạt động thể thao.', 
 750000, 650000, 3, 'Mesh + Rubber', N'Việt Nam'),
(N'Mũ Lưỡi Trai Classic', 'mu-luoi-trai-classic', N'Mũ lưỡi trai thiết kế cổ điển', 
 N'Mũ lưỡi trai với thiết kế cổ điển, chất liệu cotton thoáng mát. Phụ kiện hoàn hảo cho mọi outfit.', 
 120000, NULL, 4, 'Cotton', N'Việt Nam'),
(N'Áo Sơ Mi Linen', 'ao-so-mi-linen', N'Áo sơ mi linen mát mẻ, thanh lịch', 
 N'Áo sơ mi linen cao cấp, thoáng mát và thanh lịch. Phù hợp cho môi trường công sở và dạo phố.', 
 580000, 480000, 1, 'Linen Premium', N'Việt Nam'),
(N'Quần Jogger Thể Thao', 'quan-jogger-the-thao', N'Quần jogger thoải mái cho mọi hoạt động', 
 N'Quần jogger thể thao với chất liệu co giãn 4 chiều, thoải mái cho mọi vận động. Thiết kế hiện đại với túi khóa kéo.', 
 420000, NULL, 2, N'Polyester co giãn', N'Việt Nam'),
(N'Balo Du Lịch', 'balo-du-lich', N'Balo du lịch đa năng, chống nước', 
 N'Balo du lịch với nhiều ngăn tiện dụng, chất liệu chống nước. Thiết kế ergonomic êm ái cho lưng và vai.', 
 1200000, 980000, 4, N'Polyester chống nước', N'Việt Nam'),
(N'Áo Khoác Bomber', 'ao-khoac-bomber', N'Áo khoác bomber phong cách vintage', 
 N'Áo khoác bomber với thiết kế vintage, chất liệu vải dù cao cấp. Form dáng chuẩn, dễ phối đồ.', 
 850000, 720000, 1, N'Vải dù cao cấp', N'Việt Nam');
GO

-- Insert Product Images (15 products)
INSERT INTO ProductImages (ProductId, ImageUrl, IsPrimary, SortOrder) VALUES
(1, '/images/product-1.jpg', 1, 1),
(2, '/images/product-2.jpg', 1, 1),
(3, '/images/product-3.jpg', 1, 1),
(4, '/images/product-4.jpg', 1, 1),
(5, '/images/product-5.jpg', 1, 1),
(6, '/images/product-6.jpg', 1, 1),
(7, '/images/product-7.jpg', 1, 1),
(8, '/images/product-8.jpg', 1, 1),
(9, '/images/product-9.jpg', 1, 1),
(10, '/images/product-10.jpg', 1, 1),
(11, '/images/product-11.jpg', 1, 1),
(12, '/images/product-12.jpg', 1, 1),
(13, '/images/product-13.jpg', 1, 1),
(14, '/images/product-14.jpg', 1, 1),
(15, '/images/product-15.jpg', 1, 1);
GO

-- Insert Product Variants (all 15 products with various sizes and colors)
INSERT INTO ProductVariants (ProductId, Sku, Size, Color, Stock, Price) VALUES
-- Product 1: Áo Thun Thermo Mesh
(1, 'YM-AT-001-S', 'S', N'Vàng', 10, 196200),
(1, 'YM-AT-001-M', 'M', N'Vàng', 15, 196200),
(1, 'YM-AT-001-L', 'L', N'Vàng', 8, 196200),
(1, 'YM-AT-001-XL', 'XL', N'Vàng', 5, 196200),
-- Product 2: Áo Hoodie Premium
(2, 'YM-HD-002-M', 'M', N'Đen', 12, 380000),
(2, 'YM-HD-002-L', 'L', N'Đen', 20, 380000),
(2, 'YM-HD-002-XL', 'XL', N'Đen', 7, 380000),
-- Product 3: Giày Sneaker Classic
(3, 'YM-GY-003-39', '39', N'Trắng', 8, 495000),
(3, 'YM-GY-003-40', '40', N'Trắng', 15, 495000),
(3, 'YM-GY-003-41', '41', N'Trắng', 12, 495000),
(3, 'YM-GY-003-42', '42', N'Trắng', 10, 495000),
-- Product 4: Áo Khoác Gió Minimal
(4, 'YM-AK-004-M', 'M', 'Navy', 6, 680000),
(4, 'YM-AK-004-L', 'L', 'Navy', 10, 680000),
(4, 'YM-AK-004-XL', 'XL', 'Navy', 4, 680000),
-- Product 5: Quần Jean Slim Fit
(5, 'YM-QJ-005-30', '30', N'Xám', 15, 499000),
(5, 'YM-QJ-005-32', '32', N'Xám', 20, 499000),
(5, 'YM-QJ-005-34', '34', N'Xám', 12, 499000),
-- Product 6: Áo Polo Basic
(6, 'YM-PL-006-S', 'S', N'Trắng', 25, 249000),
(6, 'YM-PL-006-M', 'M', N'Trắng', 30, 249000),
(6, 'YM-PL-006-L', 'L', N'Trắng', 20, 249000),
(6, 'YM-PL-006-XL', 'XL', N'Trắng', 15, 249000),
-- Product 7: Quần Short Kaki
(7, 'YM-QS-007-M', 'M', N'Đen', 18, 280000),
(7, 'YM-QS-007-L', 'L', N'Đen', 22, 280000),
(7, 'YM-QS-007-XL', 'XL', N'Đen', 14, 280000),
-- Product 8: Túi Tote Canvas
(8, 'YM-TT-008-OS', 'OS', N'Trắng', 40, 150000),
(8, 'YM-TT-008-OS-B', 'OS', N'Đen', 35, 150000),
-- Product 9: Áo Thun Oversized
(9, 'YM-TO-009-M', 'M', N'Xám', 28, 199000),
(9, 'YM-TO-009-L', 'L', N'Xám', 32, 199000),
(9, 'YM-TO-009-XL', 'XL', N'Xám', 25, 199000),
-- Product 10: Giày Sneaker Sport
(10, 'YM-GS-010-39', '39', N'Đen', 10, 650000),
(10, 'YM-GS-010-40', '40', N'Đen', 15, 650000),
(10, 'YM-GS-010-41', '41', N'Đen', 12, 650000),
(10, 'YM-GS-010-42', '42', N'Đen', 8, 650000),
-- Product 11: Mũ Lưỡi Trai Classic
(11, 'YM-ML-011-OS', 'OS', N'Đen', 50, 120000),
(11, 'YM-ML-011-OS-N', 'OS', 'Navy', 45, 120000),
-- Product 12: Áo Sơ Mi Linen
(12, 'YM-SM-012-S', 'S', N'Trắng', 12, 480000),
(12, 'YM-SM-012-M', 'M', N'Trắng', 18, 480000),
(12, 'YM-SM-012-L', 'L', N'Trắng', 15, 480000),
(12, 'YM-SM-012-XL', 'XL', N'Trắng', 10, 480000),
-- Product 13: Quần Jogger Thể Thao
(13, 'YM-QJ-013-M', 'M', 'Navy', 20, 420000),
(13, 'YM-QJ-013-L', 'L', 'Navy', 25, 420000),
(13, 'YM-QJ-013-XL', 'XL', 'Navy', 18, 420000),
(13, 'YM-QJ-013-XXL', 'XXL', 'Navy', 12, 420000),
-- Product 14: Balo Du Lịch
(14, 'YM-BL-014-OS', 'OS', N'Đen', 15, 980000),
(14, 'YM-BL-014-OS-G', 'OS', N'Xám', 12, 980000),
-- Product 15: Áo Khoác Bomber
(15, 'YM-BM-015-M', 'M', N'Xám', 10, 720000),
(15, 'YM-BM-015-L', 'L', N'Xám', 14, 720000),
(15, 'YM-BM-015-XL', 'XL', N'Xám', 8, 720000);
GO

-- Insert Product Tags (all 15 products)
INSERT INTO ProductTags (ProductId, TagId) VALUES
(1, 1), (1, 2), (1, 28),   -- casual, thermo-mesh, nam
(2, 3), (2, 4), (2, 24),   -- hoodie, premium, streetwear
(3, 5), (3, 6),            -- sneaker, classic
(4, 7), (4, 8),            -- jacket, minimal
(5, 9), (5, 1),            -- jean, casual
(6, 10), (6, 1),           -- polo, casual
(7, 11), (7, 12),          -- short, summer
(8, 13), (8, 14),          -- tote, accessory
(9, 15), (9, 1), (9, 24),  -- oversized, casual, streetwear
(10, 5), (10, 16), (10, 25),-- sneaker, sport, active
(11, 14), (11, 6),         -- accessory, classic
(12, 17), (12, 18), (12, 27),-- shirt, linen, formal
(13, 19), (13, 16), (13, 25),-- jogger, sport, active
(14, 20), (14, 21), (14, 26),-- backpack, travel, waterproof
(15, 22), (15, 23);        -- bomber, vintage
GO

-- Insert Sample Reviews (multiple products)
INSERT INTO Reviews (ProductId, CustomerName, Rating, Comment) VALUES
(1, N'Nguyễn Văn A', 5, N'Sản phẩm rất tốt, chất lượng cao!'),
(1, N'Trần Thị B', 4, N'Đẹp, giá hợp lý'),
(2, N'Lê Văn C', 5, N'Áo hoodie rất ấm và thoải mái'),
(2, N'Phạm Thị D', 5, N'Chất liệu cotton mềm mại, form đẹp'),
(3, N'Hoàng Văn E', 4, N'Giày đẹp nhưng hơi chật'),
(3, N'Đặng Thị F', 5, N'Giày rất êm, phù hợp đi cả ngày'),
(4, N'Bùi Văn G', 5, N'Áo khoác gió chống nước tốt'),
(5, N'Vũ Thị H', 4, N'Quần jean đẹp, co giãn tốt'),
(6, N'Dương Văn I', 5, N'Áo polo basic rất đáng mua'),
(7, N'Mai Thị J', 4, N'Quần short thoáng mát cho mùa hè'),
(8, N'Trương Văn K', 5, N'Túi tote tiện dụng, chất lượng tốt'),
(9, N'Lý Thị L', 5, N'Áo oversized form đẹp, phong cách'),
(10, N'Phan Văn M', 5, N'Giày thể thao êm ái, đẹp'),
(11, N'Hồ Thị N', 4, N'Mũ đẹp, phù hợp nhiều phong cách'),
(12, N'Cao Văn O', 5, N'Áo sơ mi linen mát mẻ, thanh lịch'),
(13, N'Võ Thị P', 5, N'Quần jogger thoải mái cho tập luyện'),
(14, N'Tô Văn Q', 5, N'Balo chống nước tốt, nhiều ngăn'),
(15, N'Đỗ Thị R', 4, N'Áo bomber phong cách, chất liệu tốt');
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