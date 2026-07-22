using System.Collections.Generic;
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

        // A4 at 100 dpi: 827 x 1169 pts (printer units are 1/100 inch)
        private const float PageWidthPt  = 827f;
        private const float PageHeightPt = 1169f;

        public InvoicePrinter(IReadOnlyList<Sale> sales, string buyerName, string storeName = "KabyliaTaste", byte[]? logoData = null)
        {
            _sales = sales;
            _buyerName = buyerName;
            _storeName = storeName;
            _logoData = logoData;
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

            g.DrawString($"Invoice #INV-{_sales[0].Id:D5}", headerFont, Brushes.Black, x, y);
            y += lineH;
            g.DrawString($"Date: {DateTime.Now:dd-MM-yyyy HH:mm}", bodyFont, Brushes.DarkRed, x, y);
            y += lineH;
            g.DrawString($"Client: {_buyerName}", bodyFont, Brushes.DarkRed, x, y);
            y += lineH * 1.5f;

            // ?? Table header ????????????????????????????????????????????????
            g.DrawString("Product",    headerFont, Brushes.Black, colProduct,   y);
            g.DrawString("Qty",        headerFont, Brushes.Black, colQty,       y);
            g.DrawString("Unit Price", headerFont, Brushes.Black, colUnitPrice, y);
            g.DrawString("Total",      headerFont, Brushes.Black, colTotal,     y);
            y += lineH;
            g.DrawLine(Pens.Black, x, y, tableRight, y);
            y += 6;

            // ?? Rows ????????????????????????????????????????????????????????
            decimal grandTotal = 0m;
            foreach (var sale in _sales)
            {
                var rowBrush = sale.Quantity == 1 ? Brushes.Teal : Brushes.Black;

                g.DrawString(sale.Product?.Name ?? "-",               bodyFont, rowBrush, colProduct,   y);
                g.DrawString(sale.Quantity.ToString(),                 bodyFont, rowBrush, colQty,       y);
                g.DrawString(sale.UnitPrice.ToString("F2") + " DA",   bodyFont, rowBrush, colUnitPrice, y);
                g.DrawString(sale.TotalPrice.ToString("F2") + " DA",  bodyFont, rowBrush, colTotal,     y);
                y += lineH;
                grandTotal += sale.TotalPrice;
            }

            g.DrawLine(Pens.Black, x, y, tableRight, y);
            y += lineH;

            // ?? Grand total ?????????????????????????????????????????????????
            g.DrawString($"Grand Total: {grandTotal:F2} DA", headerFont, Brushes.Black, colUnitPrice, y);
            y += lineH * 2;

            // ?? Footer ??????????????????????????????????????????????????????
            g.DrawString("Thank you for your purchase!", bodyFont, Brushes.DarkGreen, x, y);

            e.HasMorePages = false;
        }
    }
}