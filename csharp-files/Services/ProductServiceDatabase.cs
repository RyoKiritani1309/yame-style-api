using Microsoft.Data.SqlClient;
using YameApi.Models;
using YameApi.Models.DTOs;
using System.Data;

namespace YameApi.Services
{
    /// <summary>
    /// Product service implementation using SQL Server database
    /// Replace ProductService with this implementation once database is set up
    /// </summary>
    public class ProductServiceDatabase : IProductService
    {
        private readonly string _connectionString;

        public ProductServiceDatabase(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("YameDB") 
                ?? throw new InvalidOperationException("Database connection string 'YameDB' not found");
        }

        public async Task<ProductListResponse> GetProductsAsync(ProductQuery query)
        {
            var products = new List<Product>();
            int totalCount = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Build dynamic WHERE clause based on filters
                var whereConditions = new List<string>();
                var parameters = new List<SqlParameter>();

                if (!string.IsNullOrEmpty(query.Q))
                {
                    whereConditions.Add("(p.Title LIKE @SearchTerm OR p.Description LIKE @SearchTerm)");
                    parameters.Add(new SqlParameter("@SearchTerm", $"%{query.Q}%"));
                }

                if (query.PriceMin.HasValue)
                {
                    whereConditions.Add("ISNULL(p.SalePrice, p.Price) >= @PriceMin");
                    parameters.Add(new SqlParameter("@PriceMin", query.PriceMin.Value));
                }

                if (query.PriceMax.HasValue)
                {
                    whereConditions.Add("ISNULL(p.SalePrice, p.Price) <= @PriceMax");
                    parameters.Add(new SqlParameter("@PriceMax", query.PriceMax.Value));
                }

                if (query.AvailableOnly == true)
                {
                    whereConditions.Add("p.Availability = 1");
                }

                if (query.OnSale == true)
                {
                    whereConditions.Add("p.SalePrice IS NOT NULL AND p.SalePrice < p.Price");
                }

                if (query.CategoryId.HasValue)
                {
                    whereConditions.Add("p.PrimaryCategoryId = @CategoryId");
                    parameters.Add(new SqlParameter("@CategoryId", query.CategoryId.Value));
                }

                string whereClause = whereConditions.Any() 
                    ? "WHERE " + string.Join(" AND ", whereConditions)
                    : "";

                // Get total count
                var countQuery = $@"
                    SELECT COUNT(*) 
                    FROM Products p
                    {whereClause}";

                using (var countCmd = new SqlCommand(countQuery, connection))
                {
                    countCmd.Parameters.AddRange(parameters.ToArray());
                    totalCount = (int)await countCmd.ExecuteScalarAsync();
                }

                // Get paginated products with all details
                var offset = (query.Page - 1) * query.PageSize;
                var productsQuery = $@"
                    SELECT 
                        p.ProductId, p.Title, p.Slug, p.ShortDescription, p.Description,
                        p.Price, p.SalePrice, p.Availability, p.Material, p.MadeIn,
                        c.Name AS CategoryName
                    FROM Products p
                    INNER JOIN Categories c ON p.PrimaryCategoryId = c.CategoryId
                    {whereClause}
                    ORDER BY p.ProductId
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                using (var cmd = new SqlCommand(productsQuery, connection))
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                    cmd.Parameters.Add(new SqlParameter("@Offset", offset));
                    cmd.Parameters.Add(new SqlParameter("@PageSize", query.PageSize));

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var product = new Product
                            {
                                Id = reader.GetInt32("ProductId"),
                                Title = reader.GetString("Title"),
                                Slug = reader.GetString("Slug"),
                                ShortDescription = reader.IsDBNull("ShortDescription") ? null : reader.GetString("ShortDescription"),
                                Description = reader.IsDBNull("Description") ? null : reader.GetString("Description"),
                                Price = reader.GetDecimal("Price"),
                                SalePrice = reader.IsDBNull("SalePrice") ? null : reader.GetDecimal("SalePrice"),
                                Availability = reader.GetBoolean("Availability"),
                                PrimaryCategory = reader.GetString("CategoryName"),
                                Specs = new ProductSpecs
                                {
                                    Material = reader.IsDBNull("Material") ? null : reader.GetString("Material"),
                                    MadeIn = reader.IsDBNull("MadeIn") ? null : reader.GetString("MadeIn")
                                }
                            };
                            products.Add(product);
                        }
                    }
                }

                // Load images, variants, tags, and reviews for each product
                foreach (var product in products)
                {
                    await LoadProductDetailsAsync(connection, product);
                }
            }

            // Get facets
            var facets = await GetFacetsAsync();

            return new ProductListResponse
            {
                Items = products,
                Meta = new ResponseMeta
                {
                    TotalCount = totalCount,
                    Page = query.Page,
                    PageSize = query.PageSize,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize)
                },
                Facets = facets
            };
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
                    SELECT 
                        p.ProductId, p.Title, p.Slug, p.ShortDescription, p.Description,
                        p.Price, p.SalePrice, p.Availability, p.Material, p.MadeIn,
                        c.Name AS CategoryName
                    FROM Products p
                    INNER JOIN Categories c ON p.PrimaryCategoryId = c.CategoryId
                    WHERE p.ProductId = @ProductId";

                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@ProductId", id));

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var product = new Product
                            {
                                Id = reader.GetInt32("ProductId"),
                                Title = reader.GetString("Title"),
                                Slug = reader.GetString("Slug"),
                                ShortDescription = reader.IsDBNull("ShortDescription") ? null : reader.GetString("ShortDescription"),
                                Description = reader.IsDBNull("Description") ? null : reader.GetString("Description"),
                                Price = reader.GetDecimal("Price"),
                                SalePrice = reader.IsDBNull("SalePrice") ? null : reader.GetDecimal("SalePrice"),
                                Availability = reader.GetBoolean("Availability"),
                                PrimaryCategory = reader.GetString("CategoryName"),
                                Specs = new ProductSpecs
                                {
                                    Material = reader.IsDBNull("Material") ? null : reader.GetString("Material"),
                                    MadeIn = reader.IsDBNull("MadeIn") ? null : reader.GetString("MadeIn")
                                }
                            };

                            await reader.CloseAsync();
                            await LoadProductDetailsAsync(connection, product);
                            return product;
                        }
                    }
                }
            }

            return null;
        }

        public async Task<Product?> GetBySlugAsync(string slug)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
                    SELECT 
                        p.ProductId, p.Title, p.Slug, p.ShortDescription, p.Description,
                        p.Price, p.SalePrice, p.Availability, p.Material, p.MadeIn,
                        c.Name AS CategoryName
                    FROM Products p
                    INNER JOIN Categories c ON p.PrimaryCategoryId = c.CategoryId
                    WHERE p.Slug = @Slug";

                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@Slug", slug));

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var product = new Product
                            {
                                Id = reader.GetInt32("ProductId"),
                                Title = reader.GetString("Title"),
                                Slug = reader.GetString("Slug"),
                                ShortDescription = reader.IsDBNull("ShortDescription") ? null : reader.GetString("ShortDescription"),
                                Description = reader.IsDBNull("Description") ? null : reader.GetString("Description"),
                                Price = reader.GetDecimal("Price"),
                                SalePrice = reader.IsDBNull("SalePrice") ? null : reader.GetDecimal("SalePrice"),
                                Availability = reader.GetBoolean("Availability"),
                                PrimaryCategory = reader.GetString("CategoryName"),
                                Specs = new ProductSpecs
                                {
                                    Material = reader.IsDBNull("Material") ? null : reader.GetString("Material"),
                                    MadeIn = reader.IsDBNull("MadeIn") ? null : reader.GetString("MadeIn")
                                }
                            };

                            await reader.CloseAsync();
                            await LoadProductDetailsAsync(connection, product);
                            return product;
                        }
                    }
                }
            }

            return null;
        }

        private async Task LoadProductDetailsAsync(SqlConnection connection, Product product)
        {
            // Load images
            var imagesQuery = @"
                SELECT ImageUrl 
                FROM ProductImages 
                WHERE ProductId = @ProductId 
                ORDER BY SortOrder, ImageId";

            using (var cmd = new SqlCommand(imagesQuery, connection))
            {
                cmd.Parameters.Add(new SqlParameter("@ProductId", product.Id));
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        product.Images.Add(reader.GetString("ImageUrl"));
                    }
                }
            }

            // Load variants
            var variantsQuery = @"
                SELECT VariantId, Sku, Size, Color, Stock, Price 
                FROM ProductVariants 
                WHERE ProductId = @ProductId";

            using (var cmd = new SqlCommand(variantsQuery, connection))
            {
                cmd.Parameters.Add(new SqlParameter("@ProductId", product.Id));
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        product.Variants.Add(new ProductVariant
                        {
                            VariantId = reader.GetInt32("VariantId"),
                            Sku = reader.GetString("Sku"),
                            Size = reader.IsDBNull("Size") ? "" : reader.GetString("Size"),
                            Color = reader.IsDBNull("Color") ? "" : reader.GetString("Color"),
                            Stock = reader.GetInt32("Stock"),
                            Price = reader.GetDecimal("Price")
                        });
                    }
                }
            }

            // Load tags
            var tagsQuery = @"
                SELECT t.Name 
                FROM Tags t
                INNER JOIN ProductTags pt ON t.TagId = pt.TagId
                WHERE pt.ProductId = @ProductId";

            using (var cmd = new SqlCommand(tagsQuery, connection))
            {
                cmd.Parameters.Add(new SqlParameter("@ProductId", product.Id));
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        product.Tags.Add(reader.GetString("Name"));
                    }
                }
            }

            // Load reviews stats
            var reviewsQuery = @"
                SELECT COUNT(*) AS ReviewCount, AVG(CAST(Rating AS FLOAT)) AS AvgRating
                FROM Reviews 
                WHERE ProductId = @ProductId";

            using (var cmd = new SqlCommand(reviewsQuery, connection))
            {
                cmd.Parameters.Add(new SqlParameter("@ProductId", product.Id));
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        product.Reviews = new ProductReviews
                        {
                            Count = reader.GetInt32("ReviewCount"),
                            Average = reader.IsDBNull("AvgRating") ? 0 : reader.GetDouble("AvgRating")
                        };
                    }
                }
            }
        }

        private async Task<ProductFacets> GetFacetsAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var facets = new ProductFacets();

                // Get distinct sizes
                var sizesQuery = "SELECT DISTINCT Size FROM ProductVariants WHERE Size IS NOT NULL ORDER BY Size";
                using (var cmd = new SqlCommand(sizesQuery, connection))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            facets.Sizes.Add(reader.GetString("Size"));
                        }
                    }
                }

                // Get distinct colors
                var colorsQuery = "SELECT DISTINCT Color FROM ProductVariants WHERE Color IS NOT NULL ORDER BY Color";
                using (var cmd = new SqlCommand(colorsQuery, connection))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            facets.Colors.Add(reader.GetString("Color"));
                        }
                    }
                }

                // Get categories
                var categoriesQuery = "SELECT Name FROM Categories ORDER BY Name";
                using (var cmd = new SqlCommand(categoriesQuery, connection))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            facets.Categories.Add(reader.GetString("Name"));
                        }
                    }
                }

                return facets;
            }
        }
    }
}