namespace KabyliaTaste.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
        public int Quantity { get; set; }
        public ProductUnit Unit { get; set; } = ProductUnit.Piece;
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
