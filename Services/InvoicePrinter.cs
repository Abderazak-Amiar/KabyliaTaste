using System.Drawing;
using System.Drawing.Printing;
using System.Collections.Generic;
using System.Linq;
using KabyliaTaste.Models;

namespace KabyliaTaste.Services
{
    public class InvoicePrinter
    {
        private readonly IReadOnlyList<Sale> _sales;
        private readonly string _buyerName;
        private readonly string _storeName;
        private readonly byte[]? _logoData;
        private readonly int? _invoiceId;
        private readonly DateTime? _invoiceDate;
        private readonly decimal _invoiceTotal;
        private readonly decimal _amountPaid;
        private readonly InvoicePaymentStatus _paymentStatus;
        private readonly string? _currencyCode;

        // A4 at 100 dpi: 827 x 1169 pts (printer units are 1/100 inch)
        private const float PageWidthPt  = 827f;
        private const float PageHeightPt = 1169f;

        public InvoicePrinter(IReadOnlyList<Sale> sales, string buyerName, string storeName = "KabyliaTaste", byte[]? logoData = null, int? invoiceId = null, DateTime? invoiceDate = null, decimal invoiceTotal = 0m, decimal amountPaid = 0m, InvoicePaymentStatus paymentStatus = InvoicePaymentStatus.No, string? currencyCode = null)
        {
            _sales = sales;
            _buyerName = buyerName;
            _storeName = storeName;
            _logoData = logoData;
            _invoiceId = invoiceId;
            _invoiceDate = invoiceDate;
            _invoiceTotal = invoiceTotal;
            _amountPaid = amountPaid;
            _paymentStatus = paymentStatus;
            _currencyCode = currencyCode;
        }

        public void PrintPreview()
        {
            var doc = BuildDocument();
            // Let the user pick a printer before previewing
            using var printDialog = new PrintDialog
            {
                Document = doc,
                AllowSomePages = false,
                UseEXDialog  = true
            };
            if (printDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            using var preview = new PrintPreviewDialog
            {
                Document = doc,
                Width    = 650,
                Height   = 900
            };
            preview.ShowDialog();
        }

        public void Print()
        {
            var doc = BuildDocument();
            using var printDialog = new PrintDialog
            {
                Document    = doc,
                UseEXDialog = true
            };
            if (printDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                doc.Print();
        }

        private PrintDocument BuildDocument()
        {
            var doc = new PrintDocument();

            // Force A4 paper size
            doc.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169);
            doc.DefaultPageSettings.Landscape  = false;

            doc.PrintPage += OnPrintPage;
            return doc;
        }

        private void OnPrintPage(object sender, PrintPageEventArgs e)
        {
            if (e.Graphics is null) return;

            var g     = e.Graphics;
            float x   = 60f;
            float y   = 60f;
            float lineH = 22f;

            // column X positions
            float colProduct   = x;
            float colQty       = x + 260;
            float colUnitPrice = x + 330;
            float colTotal     = x + 430;
            float tableRight   = x + 520;

            using var titleFont  = new Font("Arial", 16, FontStyle.Bold);
            using var headerFont = new Font("Arial", 10, FontStyle.Bold);
            using var bodyFont   = new Font("Arial", 10, FontStyle.Regular);

            // ?? Header ??????????????????????????????????????????????????????
            if (_logoData != null && _logoData.Length > 0)
            {
                using var ms = new System.IO.MemoryStream(_logoData);
                using var logo = System.Drawing.Image.FromStream(ms);
                float logoH = lineH * 3f;
                float logoW = logo.Width * logoH / logo.Height;
                g.DrawImage(logo, x, y, logoW, logoH);
                y += logoH + 2;
                g.DrawString(_storeName, titleFont, Brushes.Black, x, y);
                y += lineH * 1.5f;
            }
            else
            {
                g.DrawString(_storeName, titleFont, Brushes.Black, x, y);
                y += lineH * 2;
            }

            var invoiceNumber = _invoiceId ?? _sales[0].Id;
            g.DrawString($"{AppLocalization.T("Invoice #")}INV-{invoiceNumber:D5}", headerFont, Brushes.Black, x, y);
            y += lineH;
            g.DrawString($"{AppLocalization.T("Date")}: {(_invoiceDate ?? DateTime.Now):dd-MM-yyyy HH:mm}", bodyFont, Brushes.DarkRed, x, y);
            y += lineH;
            g.DrawString($"{AppLocalization.T("Client")}: {_buyerName}", bodyFont, Brushes.DarkRed, x, y);
            y += lineH * 1.5f;

            var grandTotal = _invoiceTotal > 0 ? _invoiceTotal : _sales.Sum(s => s.TotalPrice);
            var dueAmount = grandTotal - _amountPaid;

            g.DrawString(AppLocalization.T("Invoice Details"), headerFont, Brushes.Black, x, y);
            y += lineH;
            g.DrawString($"{AppLocalization.T("Total")}: {CurrencyFormatting.FormatAmount(grandTotal, _currencyCode)}", bodyFont, Brushes.Black, x, y);
            g.DrawString($"{AppLocalization.T("Paid")}: {CurrencyFormatting.FormatAmount(_amountPaid, _currencyCode)}", bodyFont, Brushes.Black, x + 220, y);
            y += lineH;
            g.DrawString($"{AppLocalization.T("Due")}: {CurrencyFormatting.FormatAmount(dueAmount, _currencyCode)}", bodyFont, Brushes.Black, x, y);
            g.DrawString($"{AppLocalization.T("Status")}: {AppLocalization.GetInvoiceStatusDisplayText(_paymentStatus)}", bodyFont, Brushes.Black, x + 220, y);
            y += lineH * 1.5f;

            // ?? Table header ????????????????????????????????????????????????
            g.DrawString(AppLocalization.T("Product"),    headerFont, Brushes.Black, colProduct,   y);
            g.DrawString(AppLocalization.T("Qty"),        headerFont, Brushes.Black, colQty,       y);
            g.DrawString(AppLocalization.T("Unit Price"), headerFont, Brushes.Black, colUnitPrice, y);
            g.DrawString(AppLocalization.T("Total"),      headerFont, Brushes.Black, colTotal,     y);
            y += lineH;
            g.DrawLine(Pens.Black, x, y, tableRight, y);
            y += 6;

            // ?? Rows ????????????????????????????????????????????????????????
            decimal rowTotal = 0m;
            foreach (var sale in _sales)
            {
                var rowBrush = sale.Quantity == 1m ? Brushes.Teal : Brushes.Black;

                g.DrawString(sale.Product?.Name ?? "-",               bodyFont, rowBrush, colProduct,   y);
                g.DrawString(CurrencyFormatting.FormatQuantity(sale.Quantity), bodyFont, rowBrush, colQty, y);
                g.DrawString(CurrencyFormatting.FormatAmount(sale.UnitPrice, _currencyCode), bodyFont, rowBrush, colUnitPrice, y);
                g.DrawString(CurrencyFormatting.FormatAmount(sale.TotalPrice, _currencyCode), bodyFont, rowBrush, colTotal,     y);
                y += lineH;
                rowTotal += sale.TotalPrice;
            }

            g.DrawLine(Pens.Black, x, y, tableRight, y);
            y += lineH;

            // ?? Grand total ?????????????????????????????????????????????????
            g.DrawString($"{AppLocalization.T("Grand Total")}: {CurrencyFormatting.FormatAmount(grandTotal, _currencyCode)}", headerFont, Brushes.Black, colUnitPrice, y);
            y += lineH * 2;

            // ?? Footer ??????????????????????????????????????????????????????
            g.DrawString(AppLocalization.T("Thank you for your purchase!"), bodyFont, Brushes.DarkGreen, x, y);

            e.HasMorePages = false;
        }
    }
}