using Microsoft.Data.SqlClient;
using YameApi.Models;

namespace YameApi.Services
{
    public class OrderServiceDatabase : IOrderService
    {
        private readonly string _connectionString;
        private readonly ICartService _cartService;

        public OrderServiceDatabase(IConfiguration configuration, ICartService cartService)
        {
            _connectionString = configuration.GetConnectionString("YameDB") 
                ?? throw new InvalidOperationException("Database connection string 'YameDB' not found");
            _cartService = cartService;
        }

        public async Task<Order> CreateOrderAsync(CheckoutRequest request, int? userId)
        {
            // Get cart
            var cart = await _cartService.GetCartAsync(request.CartId);
            if (cart == null || cart.Items.Count == 0)
                throw new InvalidOperationException("Cart is empty or not found");

            // Validate inventory
            foreach (var item in cart.Items)
            {
                if (item.Variant.Stock < item.Quantity)
                    throw new InvalidOperationException($"Insufficient stock for {item.Product.Title}");
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int customerId = 0;

                        // Get or create customer
                        if (userId.HasValue)
                        {
                            var customerQuery = "SELECT CustomerId FROM Customers WHERE UserId = @UserId";
                            using (var cmd = new SqlCommand(customerQuery, connection, transaction))
                            {
                                cmd.Parameters.Add(new SqlParameter("@UserId", userId.Value));
                                var result = await cmd.ExecuteScalarAsync();
                                if (result != null)
                                {
                                    customerId = (int)result;
                                }
                            }
                        }

                        // If no customer found, create guest customer
                        if (customerId == 0)
                        {
                            var createCustomerQuery = @"
                                INSERT INTO Customers (UserId, FullName, Email, Phone, Address)
                                OUTPUT INSERTED.CustomerId
                                VALUES (@UserId, @FullName, @Email, @Phone, @Address)";

                            using (var cmd = new SqlCommand(createCustomerQuery, connection, transaction))
                            {
                                cmd.Parameters.Add(new SqlParameter("@UserId", (object?)userId ?? DBNull.Value));
                                cmd.Parameters.Add(new SqlParameter("@FullName", request.FullName));
                                cmd.Parameters.Add(new SqlParameter("@Email", request.Email));
                                cmd.Parameters.Add(new SqlParameter("@Phone", request.Phone));
                                cmd.Parameters.Add(new SqlParameter("@Address", request.ShippingAddress));
                                
                                customerId = (int)await cmd.ExecuteScalarAsync();
                            }
                        }

                        // Generate order number
                        var orderNumber = $"YM{DateTime.Now:yyyyMMddHHmmss}";

                        // Create shipping address string
                        var shippingAddress = $"{request.ShippingAddress}, {request.Ward}, {request.District}, {request.City}";
                        var billingAddress = request.BillingAddress ?? shippingAddress;

                        // Create order
                        var createOrderQuery = @"
                            INSERT INTO Orders (CustomerId, OrderNumber, OrderDate, SubTotal, Discount, Shipping, Tax, Total, Status, ShippingAddress, BillingAddress, PaymentMethod, ShippingMethod, Notes)
                            OUTPUT INSERTED.OrderId
                            VALUES (@CustomerId, @OrderNumber, GETDATE(), @SubTotal, @Discount, @Shipping, @Tax, @Total, @Status, @ShippingAddress, @BillingAddress, @PaymentMethod, @ShippingMethod, @Notes)";

                        int orderId;
                        using (var cmd = new SqlCommand(createOrderQuery, connection, transaction))
                        {
                            cmd.Parameters.Add(new SqlParameter("@CustomerId", customerId));
                            cmd.Parameters.Add(new SqlParameter("@OrderNumber", orderNumber));
                            cmd.Parameters.Add(new SqlParameter("@SubTotal", cart.SubTotal));
                            cmd.Parameters.Add(new SqlParameter("@Discount", cart.Discount));
                            cmd.Parameters.Add(new SqlParameter("@Shipping", cart.Shipping));
                            cmd.Parameters.Add(new SqlParameter("@Tax", cart.Tax));
                            cmd.Parameters.Add(new SqlParameter("@Total", cart.Total));
                            cmd.Parameters.Add(new SqlParameter("@Status", "Pending"));
                            cmd.Parameters.Add(new SqlParameter("@ShippingAddress", shippingAddress));
                            cmd.Parameters.Add(new SqlParameter("@BillingAddress", billingAddress));
                            cmd.Parameters.Add(new SqlParameter("@PaymentMethod", request.PaymentMethod));
                            cmd.Parameters.Add(new SqlParameter("@ShippingMethod", request.ShippingMethod));
                            cmd.Parameters.Add(new SqlParameter("@Notes", (object?)request.Notes ?? DBNull.Value));

                            orderId = (int)await cmd.ExecuteScalarAsync();
                        }

                        // Create order items and update stock
                        foreach (var item in cart.Items)
                        {
                            // Insert order item
                            var createOrderItemQuery = @"
                                INSERT INTO OrderItems (OrderId, VariantId, Quantity, UnitPrice, LineTotal)
                                VALUES (@OrderId, @VariantId, @Quantity, @UnitPrice, @LineTotal)";

                            using (var cmd = new SqlCommand(createOrderItemQuery, connection, transaction))
                            {
                                cmd.Parameters.Add(new SqlParameter("@OrderId", orderId));
                                cmd.Parameters.Add(new SqlParameter("@VariantId", item.VariantId));
                                cmd.Parameters.Add(new SqlParameter("@Quantity", item.Quantity));
                                cmd.Parameters.Add(new SqlParameter("@UnitPrice", item.UnitPrice));
                                cmd.Parameters.Add(new SqlParameter("@LineTotal", item.LineTotal));
                                await cmd.ExecuteNonQueryAsync();
                            }

                            // Update stock
                            var updateStockQuery = @"
                                UPDATE ProductVariants 
                                SET Stock = Stock - @Quantity 
                                WHERE VariantId = @VariantId";

                            using (var cmd = new SqlCommand(updateStockQuery, connection, transaction))
                            {
                                cmd.Parameters.Add(new SqlParameter("@Quantity", item.Quantity));
                                cmd.Parameters.Add(new SqlParameter("@VariantId", item.VariantId));
                                await cmd.ExecuteNonQueryAsync();
                            }
                        }

                        // Clear cart
                        var clearCartQuery = "DELETE FROM CartItems WHERE CartId = @CartId";
                        using (var cmd = new SqlCommand(clearCartQuery, connection, transaction))
                        {
                            cmd.Parameters.Add(new SqlParameter("@CartId", request.CartId));
                            await cmd.ExecuteNonQueryAsync();
                        }

                        transaction.Commit();

                        // Return created order
                        return await GetOrderByIdAsync(orderId) ?? throw new InvalidOperationException("Order created but not found");
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
                    SELECT OrderId, CustomerId, OrderNumber, OrderDate, SubTotal, Discount, Shipping, Tax, Total, Status, ShippingAddress, BillingAddress, PaymentMethod, ShippingMethod
                    FROM Orders
                    WHERE OrderId = @OrderId";

                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@OrderId", orderId));

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var order = new Order
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
                            };

                            await reader.CloseAsync();
                            await LoadOrderItemsAsync(connection, order);
                            return order;
                        }
                    }
                }
            }

            return null;
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = new List<Order>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
                    SELECT o.OrderId, o.CustomerId, o.OrderNumber, o.OrderDate, o.SubTotal, o.Discount, o.Shipping, o.Tax, o.Total, o.Status, o.ShippingAddress, o.BillingAddress, o.PaymentMethod, o.ShippingMethod
                    FROM Orders o
                    INNER JOIN Customers c ON o.CustomerId = c.CustomerId
                    WHERE c.UserId = @UserId
                    ORDER BY o.OrderDate DESC";

                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@UserId", userId));

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

                foreach (var order in orders)
                {
                    await LoadOrderItemsAsync(connection, order);
                }
            }

            return orders;
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "UPDATE Orders SET Status = @Status WHERE OrderId = @OrderId";

                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@Status", status));
                    cmd.Parameters.Add(new SqlParameter("@OrderId", orderId));

                    var rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        private async Task LoadOrderItemsAsync(SqlConnection connection, Order order)
        {
            var query = @"
                SELECT oi.OrderItemId, oi.VariantId, oi.Quantity, oi.UnitPrice, oi.LineTotal,
                       p.ProductId, p.Title, p.Slug, p.Price,
                       pv.Size, pv.Color,
                       pi.ImageUrl
                FROM OrderItems oi
                INNER JOIN ProductVariants pv ON oi.VariantId = pv.VariantId
                INNER JOIN Products p ON pv.ProductId = p.ProductId
                LEFT JOIN (
                    SELECT ProductId, ImageUrl, 
                           ROW_NUMBER() OVER (PARTITION BY ProductId ORDER BY SortOrder, ImageId) AS RowNum
                    FROM ProductImages
                ) pi ON p.ProductId = pi.ProductId AND pi.RowNum = 1
                WHERE oi.OrderId = @OrderId";

            using (var cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.Add(new SqlParameter("@OrderId", order.OrderId));

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        order.Items.Add(new OrderItem
                        {
                            OrderItemId = reader.GetInt32(0),
                            VariantId = reader.GetInt32(1),
                            Quantity = reader.GetInt32(2),
                            UnitPrice = reader.GetDecimal(3),
                            LineTotal = reader.GetDecimal(4),
                            Product = new Product
                            {
                                Id = reader.GetInt32(5),
                                Title = reader.GetString(6),
                                Slug = reader.GetString(7),
                                Price = reader.GetDecimal(8),
                                Images = new List<string> { reader.IsDBNull(11) ? "" : reader.GetString(11) }
                            },
                            Variant = new ProductVariant
                            {
                                VariantId = reader.GetInt32(1),
                                Size = reader.IsDBNull(9) ? "" : reader.GetString(9),
                                Color = reader.IsDBNull(10) ? "" : reader.GetString(10)
                            }
                        });
                    }
                }
            }
        }
    }
}
