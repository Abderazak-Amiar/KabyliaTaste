namespace KabyliaTaste.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime SaleDate { get; set; }
        public string? BuyerName { get; set; }
        public int? InvoiceId { get; set; }
        public Invoice? Invoice { get; set; }
    }
}
