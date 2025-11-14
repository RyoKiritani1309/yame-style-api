using Microsoft.Data.SqlClient;
using YameApi.Models;

namespace YameApi.Services
{
    public class AdminServiceDatabase : IAdminService
    {
        private readonly string _connectionString;

        public AdminServiceDatabase(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("YameDB")
                ?? throw new InvalidOperationException("Database connection string 'YameDB' not found");
        }

        public async Task<bool> IsUserAdminAsync(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT COUNT(*) FROM UserRoles WHERE UserId = @UserId AND Role = 'admin'";
                
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                    var count = (int)await cmd.ExecuteScalarAsync();
                    return count > 0;
                }
            }
        }

        public async Task<DashboardStats> GetDashboardStatsAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                var stats = new DashboardStats();
                
                // Get total products
                var productQuery = "SELECT COUNT(*) FROM Products";
                using (var cmd = new SqlCommand(productQuery, connection))
                {
                    stats.TotalProducts = (int)await cmd.ExecuteScalarAsync();
                }
                
                // Get total orders
                var orderQuery = "SELECT COUNT(*) FROM Orders";
                using (var cmd = new SqlCommand(orderQuery, connection))
                {
                    stats.TotalOrders = (int)await cmd.ExecuteScalarAsync();
                }
                
                // Get total users
                var userQuery = "SELECT COUNT(*) FROM Users";
                using (var cmd = new SqlCommand(userQuery, connection))
                {
                    stats.TotalUsers = (int)await cmd.ExecuteScalarAsync();
                }
                
                // Get total revenue
                var revenueQuery = "SELECT ISNULL(SUM(Total), 0) FROM Orders WHERE Status != 'Cancelled'";
                using (var cmd = new SqlCommand(revenueQuery, connection))
                {
                    stats.TotalRevenue = (decimal)await cmd.ExecuteScalarAsync();
                }
                
                // Get pending orders
                var pendingQuery = "SELECT COUNT(*) FROM Orders WHERE Status = 'Pending'";
                using (var cmd = new SqlCommand(pendingQuery, connection))
                {
                    stats.PendingOrders = (int)await cmd.ExecuteScalarAsync();
                }
                
                return stats;
            }
        }

        public async Task<bool> CreateProductAsync(ProductCreateRequest request)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insert product
                        var productQuery = @"
                            INSERT INTO Products (Title, Slug, ShortDescription, Description, Price, SalePrice, 
                                                PrimaryCategoryId, Availability, Material, MadeIn, CreatedAt, UpdatedAt)
                            OUTPUT INSERTED.ProductId
                            VALUES (@Title, @Slug, @ShortDesc, @Desc, @Price, @SalePrice, 
                                    @CategoryId, @Availability, @Material, @MadeIn, GETDATE(), GETDATE())";
                        
                        int productId;
                        using (var cmd = new SqlCommand(productQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Title", request.Title);
                            cmd.Parameters.AddWithValue("@Slug", request.Slug);
                            cmd.Parameters.AddWithValue("@ShortDesc", (object?)request.ShortDescription ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Desc", (object?)request.Description ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Price", request.Price);
                            cmd.Parameters.AddWithValue("@SalePrice", (object?)request.SalePrice ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@CategoryId", request.CategoryId);
                            cmd.Parameters.AddWithValue("@Availability", request.Availability);
                            cmd.Parameters.AddWithValue("@Material", (object?)request.Material ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@MadeIn", (object?)request.MadeIn ?? DBNull.Value);
                            
                            productId = (int)await cmd.ExecuteScalarAsync();
                        }
                        
                        // Insert images
                        if (request.Images?.Any() == true)
                        {
                            foreach (var (image, index) in request.Images.Select((img, idx) => (img, idx)))
                            {
                                var imageQuery = @"
                                    INSERT INTO ProductImages (ProductId, ImageUrl, IsPrimary, SortOrder)
                                    VALUES (@ProductId, @ImageUrl, @IsPrimary, @SortOrder)";
                                
                                using (var cmd = new SqlCommand(imageQuery, connection, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@ProductId", productId);
                                    cmd.Parameters.AddWithValue("@ImageUrl", image);
                                    cmd.Parameters.AddWithValue("@IsPrimary", index == 0);
                                    cmd.Parameters.AddWithValue("@SortOrder", index);
                                    await cmd.ExecuteNonQueryAsync();
                                }
                            }
                        }
                        
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public async Task<bool> UpdateProductAsync(int productId, ProductUpdateRequest request)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                var query = @"
                    UPDATE Products 
                    SET Title = @Title, 
                        Slug = @Slug, 
                        ShortDescription = @ShortDesc,
                        Description = @Desc,
                        Price = @Price,
                        SalePrice = @SalePrice,
                        PrimaryCategoryId = @CategoryId,
                        Availability = @Availability,
                        Material = @Material,
                        MadeIn = @MadeIn,
                        UpdatedAt = GETDATE()
                    WHERE ProductId = @ProductId";
                
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    cmd.Parameters.AddWithValue("@Title", request.Title);
                    cmd.Parameters.AddWithValue("@Slug", request.Slug);
                    cmd.Parameters.AddWithValue("@ShortDesc", (object?)request.ShortDescription ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Desc", (object?)request.Description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Price", request.Price);
                    cmd.Parameters.AddWithValue("@SalePrice", (object?)request.SalePrice ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CategoryId", request.CategoryId);
                    cmd.Parameters.AddWithValue("@Availability", request.Availability);
                    cmd.Parameters.AddWithValue("@Material", (object?)request.Material ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MadeIn", (object?)request.MadeIn ?? DBNull.Value);
                    
                    var rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "DELETE FROM Products WHERE ProductId = @ProductId";
                
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    var rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task<PaginatedList<Order>> GetAllOrdersAsync(int page, int pageSize, string? status)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                var whereClause = string.IsNullOrEmpty(status) ? "" : "WHERE Status = @Status";
                var countQuery = $"SELECT COUNT(*) FROM Orders {whereClause}";
                
                int totalCount;
                using (var cmd = new SqlCommand(countQuery, connection))
                {
                    if (!string.IsNullOrEmpty(status))
                        cmd.Parameters.AddWithValue("@Status", status);
                    totalCount = (int)await cmd.ExecuteScalarAsync();
                }
                
                var offset = (page - 1) * pageSize;
                var query = $@"
                    SELECT OrderId, CustomerId, OrderNumber, OrderDate, SubTotal, Discount, 
                           Shipping, Tax, Total, Status, ShippingAddress, BillingAddress, 
                           PaymentMethod, ShippingMethod
                    FROM Orders 
                    {whereClause}
                    ORDER BY OrderDate DESC
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
                
                var orders = new List<Order>();
                using (var cmd = new SqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(status))
                        cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@Offset", offset);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);
                    
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            orders.Add(new Order
                            {
                                OrderId = reader.GetInt32(0),
                                CustomerId = reader.GetInt32(1),
                                OrderNumber = reader.GetString(2),
                                OrderDate = reader.GetDateTime(3),
                                SubTotal = reader.GetDecimal(4),
                                Discount = reader.GetDecimal(5),
                                Shipping = reader.GetDecimal(6),
                                Tax = reader.GetDecimal(7),
                                Total = reader.GetDecimal(8),
                                Status = reader.GetString(9),
                                ShippingAddress = reader.IsDBNull(10) ? null : reader.GetString(10),
                                BillingAddress = reader.IsDBNull(11) ? null : reader.GetString(11),
                                PaymentMethod = reader.IsDBNull(12) ? null : reader.GetString(12),
                                ShippingMethod = reader.IsDBNull(13) ? null : reader.GetString(13)
                            });
                        }
                    }
                }
                
                return new PaginatedList<Order>
                {
                    Items = orders,
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize
                };
            }
        }

        public async Task<PaginatedList<UserAccount>> GetAllUsersAsync(int page, int pageSize)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                var countQuery = "SELECT COUNT(*) FROM Users";
                int totalCount;
                using (var cmd = new SqlCommand(countQuery, connection))
                {
                    totalCount = (int)await cmd.ExecuteScalarAsync();
                }
                
                var offset = (page - 1) * pageSize;
                var query = @"
                    SELECT u.UserId, u.Email, u.PasswordHash, u.CreatedAt,
                           c.FullName, c.Phone, c.Address,
                           STRING_AGG(ur.Role, ',') as Roles
                    FROM Users u
                    LEFT JOIN Customers c ON u.UserId = c.UserId
                    LEFT JOIN UserRoles ur ON u.UserId = ur.UserId
                    GROUP BY u.UserId, u.Email, u.PasswordHash, u.CreatedAt, c.FullName, c.Phone, c.Address
                    ORDER BY u.CreatedAt DESC
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
                
                var users = new List<UserAccount>();
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Offset", offset);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);
                    
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            users.Add(new UserAccount
                            {
                                UserId = reader.GetInt32(0),
                                Email = reader.GetString(1),
                                PasswordHash = reader.GetString(2),
                                CreatedAt = reader.GetDateTime(3),
                                FullName = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Phone = reader.IsDBNull(5) ? null : reader.GetString(5),
                                Address = reader.IsDBNull(6) ? null : reader.GetString(6),
                                Roles = reader.IsDBNull(7) ? new List<string>() : reader.GetString(7).Split(',').ToList()
                            });
                        }
                    }
                }
                
                return new PaginatedList<UserAccount>
                {
                    Items = users,
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize
                };
            }
        }

        public async Task<bool> ToggleUserRoleAsync(int userId, string role)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                // Check if role exists
                var checkQuery = "SELECT COUNT(*) FROM UserRoles WHERE UserId = @UserId AND Role = @Role";
                bool hasRole;
                using (var cmd = new SqlCommand(checkQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Role", role);
                    hasRole = (int)await cmd.ExecuteScalarAsync() > 0;
                }
                
                string query;
                if (hasRole)
                {
                    query = "DELETE FROM UserRoles WHERE UserId = @UserId AND Role = @Role";
                }
                else
                {
                    query = "INSERT INTO UserRoles (UserId, Role, CreatedAt) VALUES (@UserId, @Role, GETDATE())";
                }
                
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Role", role);
                    await cmd.ExecuteNonQueryAsync();
                    return true;
                }
            }
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT CategoryId, Name, Slug, Description FROM Categories ORDER BY Name";
                
                var categories = new List<Category>();
                using (var cmd = new SqlCommand(query, connection))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            categories.Add(new Category
                            {
                                CategoryId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Slug = reader.GetString(2),
                                Description = reader.IsDBNull(3) ? null : reader.GetString(3)
                            });
                        }
                    }
                }
                return categories;
            }
        }

        public async Task<bool> CreateCategoryAsync(string name, string slug, string? description)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"
                    INSERT INTO Categories (Name, Slug, Description, CreatedAt, UpdatedAt)
                    VALUES (@Name, @Slug, @Description, GETDATE(), GETDATE())";
                
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Slug", slug);
                    cmd.Parameters.AddWithValue("@Description", (object?)description ?? DBNull.Value);
                    
                    var rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "DELETE FROM Categories WHERE CategoryId = @CategoryId";
                
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                    var rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
    }
}
