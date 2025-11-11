using Microsoft.Data.SqlClient;
using YameApi.Models;

namespace YameApi.Services
{
    public class CartServiceDatabase : ICartService
    {
        private readonly string _connectionString;

        public CartServiceDatabase(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("YameDB") 
                ?? throw new InvalidOperationException("Database connection string 'YameDB' not found");
        }

        public async Task<Cart> CreateCartAsync()
        {
            var cartId = Guid.NewGuid().ToString();
            
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
                    INSERT INTO Carts (CartId, CreatedAt, UpdatedAt)
                    VALUES (@CartId, GETDATE(), GETDATE());
                    
                    SELECT CartId, CreatedAt, UpdatedAt FROM Carts WHERE CartId = @CartId";

                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@CartId", cartId));

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Cart
                            {
                                CartId = reader.GetString(0),
                                Items = new List<CartItem>(),
                                SubTotal = 0,
                                Discount = 0,
                                Shipping = 0,
                                Tax = 0,
                                Total = 0
                            };
                        }
                    }
                }
            }

            throw new InvalidOperationException("Failed to create cart");
        }

        public async Task<Cart?> GetCartAsync(string cartId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Get cart
                var cartQuery = "SELECT CartId FROM Carts WHERE CartId = @CartId";
                
                using (var cmd = new SqlCommand(cartQuery, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@CartId", cartId));

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (!await reader.ReadAsync())
                            return null;
                    }
                }

                var cart = new Cart
                {
                    CartId = cartId,
                    Items = new List<CartItem>()
                };

                // Get cart items with product and variant details
                var itemsQuery = @"
                    SELECT 
                        ci.ItemId, ci.Quantity, ci.UnitPrice,
                        p.ProductId, p.Title, p.Slug, p.Price, p.SalePrice,
                        pv.VariantId, pv.Size, pv.Color, pv.Stock,
                        pi.ImageUrl
                    FROM CartItems ci
                    INNER JOIN ProductVariants pv ON ci.VariantId = pv.VariantId
                    INNER JOIN Products p ON pv.ProductId = p.ProductId
                    LEFT JOIN (
                        SELECT ProductId, ImageUrl, 
                               ROW_NUMBER() OVER (PARTITION BY ProductId ORDER BY SortOrder, ImageId) AS RowNum
                        FROM ProductImages
                    ) pi ON p.ProductId = pi.ProductId AND pi.RowNum = 1
                    WHERE ci.CartId = @CartId";

                using (var cmd = new SqlCommand(itemsQuery, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@CartId", cartId));

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var item = new CartItem
                            {
                                ItemId = reader.GetString(0),
                                Quantity = reader.GetInt32(1),
                                UnitPrice = reader.GetDecimal(2),
                                Product = new Product
                                {
                                    Id = reader.GetInt32(3),
                                    Title = reader.GetString(4),
                                    Slug = reader.GetString(5),
                                    Price = reader.GetDecimal(6),
                                    SalePrice = reader.IsDBNull(7) ? null : reader.GetDecimal(7),
                                    Images = new List<string> { reader.IsDBNull(12) ? "" : reader.GetString(12) }
                                },
                                Variant = new ProductVariant
                                {
                                    VariantId = reader.GetInt32(8),
                                    Size = reader.IsDBNull(9) ? "" : reader.GetString(9),
                                    Color = reader.IsDBNull(10) ? "" : reader.GetString(10),
                                    Stock = reader.GetInt32(11)
                                },
                                VariantId = reader.GetInt32(8)
                            };
                            item.LineTotal = item.UnitPrice * item.Quantity;
                            cart.Items.Add(item);
                        }
                    }
                }

                RecalculateCart(cart);
                return cart;
            }
        }

        public async Task<Cart?> AddItemAsync(string cartId, int variantId, int quantity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Verify cart exists
                var cartCheck = "SELECT CartId FROM Carts WHERE CartId = @CartId";
                using (var cmd = new SqlCommand(cartCheck, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@CartId", cartId));
                    var result = await cmd.ExecuteScalarAsync();
                    if (result == null)
                        return null;
                }

                // Get variant price
                var priceQuery = "SELECT Price FROM ProductVariants WHERE VariantId = @VariantId";
                decimal unitPrice = 0;
                
                using (var cmd = new SqlCommand(priceQuery, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@VariantId", variantId));
                    var result = await cmd.ExecuteScalarAsync();
                    if (result == null)
                        throw new InvalidOperationException("Variant not found");
                    unitPrice = (decimal)result;
                }

                // Check if item already exists in cart
                var existingItemQuery = @"
                    SELECT ItemId, Quantity 
                    FROM CartItems 
                    WHERE CartId = @CartId AND VariantId = @VariantId";

                string? existingItemId = null;
                int existingQuantity = 0;

                using (var cmd = new SqlCommand(existingItemQuery, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@CartId", cartId));
                    cmd.Parameters.Add(new SqlParameter("@VariantId", variantId));

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            existingItemId = reader.GetString(0);
                            existingQuantity = reader.GetInt32(1);
                        }
                    }
                }

                if (existingItemId != null)
                {
                    // Update existing item
                    var updateQuery = @"
                        UPDATE CartItems 
                        SET Quantity = @Quantity, UnitPrice = @UnitPrice
                        WHERE ItemId = @ItemId";

                    using (var cmd = new SqlCommand(updateQuery, connection))
                    {
                        cmd.Parameters.Add(new SqlParameter("@Quantity", existingQuantity + quantity));
                        cmd.Parameters.Add(new SqlParameter("@UnitPrice", unitPrice));
                        cmd.Parameters.Add(new SqlParameter("@ItemId", existingItemId));
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                else
                {
                    // Add new item
                    var insertQuery = @"
                        INSERT INTO CartItems (ItemId, CartId, VariantId, Quantity, UnitPrice)
                        VALUES (@ItemId, @CartId, @VariantId, @Quantity, @UnitPrice)";

                    using (var cmd = new SqlCommand(insertQuery, connection))
                    {
                        cmd.Parameters.Add(new SqlParameter("@ItemId", Guid.NewGuid().ToString()));
                        cmd.Parameters.Add(new SqlParameter("@CartId", cartId));
                        cmd.Parameters.Add(new SqlParameter("@VariantId", variantId));
                        cmd.Parameters.Add(new SqlParameter("@Quantity", quantity));
                        cmd.Parameters.Add(new SqlParameter("@UnitPrice", unitPrice));
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                // Update cart timestamp
                var updateCartQuery = "UPDATE Carts SET UpdatedAt = GETDATE() WHERE CartId = @CartId";
                using (var cmd = new SqlCommand(updateCartQuery, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@CartId", cartId));
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            return await GetCartAsync(cartId);
        }

        public async Task<Cart?> UpdateItemAsync(string cartId, string itemId, int quantity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                if (quantity <= 0)
                {
                    return await RemoveItemAsync(cartId, itemId);
                }

                var updateQuery = @"
                    UPDATE CartItems 
                    SET Quantity = @Quantity
                    WHERE ItemId = @ItemId AND CartId = @CartId";

                using (var cmd = new SqlCommand(updateQuery, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@Quantity", quantity));
                    cmd.Parameters.Add(new SqlParameter("@ItemId", itemId));
                    cmd.Parameters.Add(new SqlParameter("@CartId", cartId));
                    
                    var rowsAffected = await cmd.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                        return null;
                }

                // Update cart timestamp
                var updateCartQuery = "UPDATE Carts SET UpdatedAt = GETDATE() WHERE CartId = @CartId";
                using (var cmd = new SqlCommand(updateCartQuery, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@CartId", cartId));
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            return await GetCartAsync(cartId);
        }

        public async Task<Cart?> RemoveItemAsync(string cartId, string itemId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var deleteQuery = "DELETE FROM CartItems WHERE ItemId = @ItemId AND CartId = @CartId";

                using (var cmd = new SqlCommand(deleteQuery, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@ItemId", itemId));
                    cmd.Parameters.Add(new SqlParameter("@CartId", cartId));
                    
                    var rowsAffected = await cmd.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                        return null;
                }

                // Update cart timestamp
                var updateCartQuery = "UPDATE Carts SET UpdatedAt = GETDATE() WHERE CartId = @CartId";
                using (var cmd = new SqlCommand(updateCartQuery, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@CartId", cartId));
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            return await GetCartAsync(cartId);
        }

        private void RecalculateCart(Cart cart)
        {
            cart.SubTotal = cart.Items.Sum(i => i.LineTotal);
            cart.Total = cart.SubTotal - cart.Discount + cart.Shipping + cart.Tax;
        }
    }
}
