using System.Collections.Generic;

namespace KabyliaTaste.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public string BuyerName { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public InvoicePaymentStatus PaymentStatus { get; set; } = InvoicePaymentStatus.No;
        public decimal AmountPaid { get; set; }

        public List<Sale> Sales { get; set; } = new();
    }
}
