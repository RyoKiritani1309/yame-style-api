namespace YameApi.Models.DTOs
{
    public class ReviewRequest
    {
        public int ProductId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
