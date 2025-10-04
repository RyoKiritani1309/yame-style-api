namespace YameApi.Models
{
    public class Cart
    {
        public string CartId { get; set; } = string.Empty;
        public List<CartItem> Items { get; set; } = new();
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Shipping { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
    }

    public class CartItem
    {
        public string ItemId { get; set; } = string.Empty;
        public int VariantId { get; set; }
        public Product Product { get; set; } = null!;
        public ProductVariant Variant { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }
}
