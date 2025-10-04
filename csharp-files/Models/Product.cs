namespace YameApi.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal? SalePrice { get; set; }
        public List<string> Images { get; set; } = new();
        public List<ProductVariant> Variants { get; set; } = new();
        public bool Availability { get; set; }
        public List<string> Tags { get; set; } = new();
        public string PrimaryCategory { get; set; } = string.Empty;
        public ProductSpecs? Specs { get; set; }
        public ProductReviews? Reviews { get; set; }
    }

    public class ProductVariant
    {
        public int VariantId { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int Stock { get; set; }
        public decimal Price { get; set; }
    }

    public class ProductSpecs
    {
        public string? Material { get; set; }
        public string? MadeIn { get; set; }
    }

    public class ProductReviews
    {
        public double Average { get; set; }
        public int Count { get; set; }
    }
}
