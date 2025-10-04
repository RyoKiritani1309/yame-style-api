namespace YameApi.Models.DTOs
{
    public class ProductQuery
    {
        public string? Q { get; set; }
        public int? CategoryId { get; set; }
        public int? CollectionId { get; set; }
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }
        public List<string>? Sizes { get; set; }
        public List<string>? Colors { get; set; }
        public string Sort { get; set; } = "relevance";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
        public bool? OnSale { get; set; }
        public bool? AvailableOnly { get; set; }
    }
}
