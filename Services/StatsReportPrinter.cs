using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace KabyliaTaste.Services
{
    public class StatsReportRow
    {
        public string Product   { get; set; } = "";
        public DateTime Date    { get; set; }
        public string Hour      { get; set; } = "";
        public int    UnitsSold { get; set; }
        public decimal Revenue  { get; set; }
        public decimal Cost     { get; set; }
        public decimal Profit   { get; set; }
    }

    public class StatsReportPrinter
    {
        private readonly IReadOnlyList<StatsReportRow> _rows;
        private readonly string _productFilter;
        private readonly string _clientFilter;
        private readonly string _period;
        private readonly DateTime _refDate;
        private readonly string _storeName;
        private readonly byte[]? _logoData;
        private readonly decimal _collectedAmount;
        private readonly decimal _debtAmount;
        private readonly decimal _expensesAmount;

        public StatsReportPrinter(
            IReadOnlyList<StatsReportRow> rows,
            string productFilter,
            string clientFilter,
            string period,
            DateTime refDate,
            string storeName = "KabyliaTaste",
            byte[]? logoData = null,
            decimal collectedAmount = 0m,
            decimal debtAmount = 0m,
            decimal expensesAmount = 0m)
        {
            _rows          = rows;
            _productFilter = productFilter;
            _clientFilter  = clientFilter;
            _period        = period;
            _refDate       = refDate;
            _storeName     = storeName;
            _logoData      = logoData;
            _collectedAmount = collectedAmount;
            _debtAmount      = debtAmount;
            _expensesAmount  = expensesAmount;
        }

        public void PrintPreview()
        {
            var doc = BuildDocument();
            using var printDialog = new PrintDialog
            {
                Document    = doc,
                UseEXDialog = true
            };
            if (printDialog.ShowDialog() != DialogResult.OK)
                return;

            using var preview = new PrintPreviewDialog
            {
                Document = doc,
                Width    = 680,
                Height   = 950
            };
            preview.ShowDialog();
        }

        private PrintDocument BuildDocument()
        {
            var doc = new PrintDocument();
            doc.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169);
            doc.DefaultPageSettings.Landscape = false;
            doc.PrintPage += OnPrintPage;
            return doc;
        }

        private void OnPrintPage(object sender, PrintPageEventArgs e)
        {
            if (e.Graphics is null) return;

            var g      = e.Graphics;
            float x    = 60f;
            float y    = 60f;
            float lineH = 22f;

            // column X positions
            float colDate      = x;
            float colHour      = x + 90;
            float colProduct   = x + 155;
            float colUnits     = x + 340;
            float colRevenue   = x + 420;
            float colCost      = x + 510;
            float colProfit    = x + 600;
            float tableRight   = x + 690;

            using var titleFont  = new Font("Arial", 16, FontStyle.Bold);
            using var headerFont = new Font("Arial", 10, FontStyle.Bold);
            using var bodyFont   = new Font("Arial", 10, FontStyle.Regular);
            using var smallFont  = new Font("Arial", 9,  FontStyle.Italic);

            // ?? Title ??????????????????????????????????????????????????????????
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

            g.DrawString("Sales Statistics Report", headerFont, Brushes.Black, x, y);
            y += lineH;
            g.DrawString($"Generated: {DateTime.Now:dd-MM-yyyy HH:mm}", smallFont, Brushes.DarkGray, x, y);
            y += lineH;

            // ?? Filters ????????????????????????????????????????????????????????
            string filterDesc = BuildFilterDescription();
            g.DrawString($"Filters: {filterDesc}", smallFont, Brushes.DarkRed, x, y);
            y += lineH * 1.5f;

            // ?? Table header ???????????????????????????????????????????????????
            g.FillRectangle(Brushes.DarkSlateGray, x, y, tableRight - x, lineH);
            g.DrawString("Date",      headerFont, Brushes.White, colDate,      y + 3);
            g.DrawString("Hour",      headerFont, Brushes.White, colHour,      y + 3);
            g.DrawString("Product",   headerFont, Brushes.White, colProduct,   y + 3);
            g.DrawString("Units",     headerFont, Brushes.White, colUnits,     y + 3);
            g.DrawString("Revenue",   headerFont, Brushes.White, colRevenue,   y + 3);
            g.DrawString("Cost",      headerFont, Brushes.White, colCost,      y + 3);
            g.DrawString("Profit",    headerFont, Brushes.White, colProfit,    y + 3);
            y += lineH;

            // ?? Table rows ?????????????????????????????????????????????????????
            bool shade = false;
            foreach (var row in _rows)
            {
                if (shade)
                    g.FillRectangle(Brushes.AliceBlue, x, y, tableRight - x, lineH);

                var profitBrush = row.Profit >= 0 ? Brushes.DarkGreen : Brushes.Red;

                g.DrawString(row.Date.ToString("yyyy-MM-dd"), bodyFont, Brushes.Black, colDate, y + 2);
                g.DrawString(row.Hour,                       bodyFont, Brushes.Black, colHour, y + 2);
                g.DrawString(row.Product,                       bodyFont, Brushes.Black,   colProduct,  y + 2);
                g.DrawString(row.UnitsSold.ToString(),          bodyFont, Brushes.Black,   colUnits,    y + 2);
                g.DrawString(row.Revenue.ToString("F2"),        bodyFont, Brushes.Black,   colRevenue,  y + 2);
                g.DrawString(row.Cost.ToString("F2"),           bodyFont, Brushes.Black,   colCost,     y + 2);
                g.DrawString(row.Profit.ToString("F2"),         bodyFont, profitBrush,     colProfit,   y + 2);

                y += lineH;
                shade = !shade;
            }

            // ?? Separator ??????????????????????????????????????????????????????
            y += 4;
            g.DrawLine(Pens.Black, x, y, tableRight, y);
            y += 6;

            // ?? Totals ?????????????????????????????????????????????????????????
            var totalRevenue = _rows.Sum(r => r.Revenue);
            var totalCost    = _rows.Sum(r => r.Cost);
            var totalProfit  = _rows.Sum(r => r.Profit);
            var totalUnits   = _rows.Sum(r => r.UnitsSold);

            var totalProfitBrush = totalProfit >= 0 ? Brushes.DarkGreen : Brushes.Red;

            g.DrawString("TOTAL",                       headerFont, Brushes.Black,       colProduct,  y);
            g.DrawString(totalUnits.ToString(),         headerFont, Brushes.Black,       colUnits,    y);
            g.DrawString(totalRevenue.ToString("F2"),   headerFont, Brushes.Black,       colRevenue,  y);
            g.DrawString(totalCost.ToString("F2"),      headerFont, Brushes.Black,       colCost,     y);
            g.DrawString(totalProfit.ToString("F2"),    headerFont, totalProfitBrush,    colProfit,   y);

            y += lineH + 8;
            var netProfit = _collectedAmount - _expensesAmount;
            var netProfitBrush = netProfit >= 0 ? Brushes.DarkGreen : Brushes.Red;
            g.DrawString($"Collected: {_collectedAmount:F2}", headerFont, Brushes.Black, x, y);
            y += lineH;
            g.DrawString($"Debt: {_debtAmount:F2}", headerFont, Brushes.Black, x, y);
            y += lineH;
            g.DrawString($"Expenses: {_expensesAmount:F2}", headerFont, Brushes.Black, x, y);
            y += lineH;
            g.DrawString($"Net Profit: {netProfit:F2}", headerFont, netProfitBrush, x, y);

            e.HasMorePages = false;
        }

        private string BuildFilterDescription()
        {
            var parts = new System.Collections.Generic.List<string>();
            if (!string.IsNullOrEmpty(_productFilter))
                parts.Add($"Product: {_productFilter}");
            if (!string.IsNullOrEmpty(_clientFilter))
                parts.Add($"Client: {_clientFilter}");
            if (!string.IsNullOrEmpty(_period))
                parts.Add($"Period: {_period} ({_refDate:dd-MM-yyyy})");
            return parts.Count > 0 ? string.Join(", ", parts) : "None";
        }
    }
}
