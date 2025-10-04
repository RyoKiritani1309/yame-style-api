namespace YameApi.Models.DTOs
{
    public class ProductListResponse
    {
        public List<Product> Items { get; set; } = new();
        public ResponseMeta Meta { get; set; } = new();
        public ProductFacets? Facets { get; set; }
    }

    public class ResponseMeta
    {
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }

    public class ProductFacets
    {
        public List<string> Sizes { get; set; } = new();
        public List<string> Colors { get; set; } = new();
        public List<PriceRange> PriceRanges { get; set; } = new();
        public List<string> Categories { get; set; } = new();
    }

    public class PriceRange
    {
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public int Count { get; set; }
    }
}
