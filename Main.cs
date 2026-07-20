namespace KabyliaTaste
{
    using System;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using KabyliaTaste.Data;
    using KabyliaTaste.Models;
    using Microsoft.EntityFrameworkCore;

    public partial class Main : Form
    {
        private int? selectedProductId = null;
        private int? selectedSaleId = null;
        private int _currentSalePage = 1;
        private const int SalePageSize = 20;
        private int _currentProductPage = 1;
        private const int ProductPageSize = 20;
        private string _productFilter = "";

        public Main()
        {
            InitializeComponent();

            // wire events
            Load += Main_Load;
            btnAdd.Click += BtnAdd_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnDelete.Click += BtnDelete_Click;
            btnClear.Click += BtnClear_Click;
            dgvProducts.SelectionChanged += DgvProducts_SelectionChanged;
            dgvProducts.CellClick += DgvProducts_CellClick;
            dgvProducts.CellFormatting += DgvProducts_CellFormatting;
            chkShowBuyPrice.CheckedChanged += ChkShowBuyPrice_CheckedChanged;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            tabControlMain.SelectedIndexChanged += TabControlMain_SelectedIndexChanged;
            btnProductPrev.Click += BtnProductPrev_Click;
            btnProductNext.Click += BtnProductNext_Click;
            cmbSaleProduct.SelectedIndexChanged += CmbSaleProduct_SelectedIndexChanged;
            numSaleQuantity.ValueChanged += SaleInputChanged;
            numSaleQuantity.TextChanged += SaleInputChanged;
            numSaleUnitPrice.ValueChanged += SaleInputChanged;
            btnSell.Click += BtnSell_Click;
            btnDeleteSale.Click += BtnDeleteSale_Click;
            btnPrintInvoice.Click += BtnPrintInvoice_Click;
            txtBuyerName.TextChanged += TxtBuyerName_TextChanged;
            dgvSales.SelectionChanged += DgvSales_SelectionChanged;
            btnSalePrev.Click += BtnSalePrev_Click;
            btnSaleNext.Click += BtnSaleNext_Click;
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            _currentProductPage = 1;
            LoadProducts(txtSearch.Text.Trim());
        }

        private void Main_Load(object? sender, EventArgs e)
        {
            LoadProducts();
        }

        private void LoadProducts(string filter = "")
        {
            _productFilter = filter;
            using var db = new AppDbContext();
            var query = db.Products.OrderBy(p => p.Id).AsQueryable();
            if (!string.IsNullOrEmpty(filter))
                query = query.Where(p => p.Name.ToLower().Contains(filter.ToLower()));

            var totalCount = query.Count();
            var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)ProductPageSize));
            _currentProductPage = Math.Clamp(_currentProductPage, 1, totalPages);

            var list = query
                .Skip((_currentProductPage - 1) * ProductPageSize)
                .Take(ProductPageSize)
                .ToList();
            dgvProducts.DataSource = list;

            lblProductPage.Text = $"Page {_currentProductPage} / {totalPages}";
            btnProductPrev.Enabled = _currentProductPage > 1;
            btnProductNext.Enabled = _currentProductPage < totalPages;

        if (dgvProducts.Columns.Contains("BuyPrice"))
            dgvProducts.Columns["BuyPrice"].Visible = chkShowBuyPrice.Checked;
        ClearForm(false);
        // auto-select first row if available
        if (dgvProducts.Rows.Count > 0)
        {
            dgvProducts.ClearSelection();
            dgvProducts.Rows[0].Selected = true;
            DgvProducts_SelectionChanged(null, EventArgs.Empty);
        }
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            var name = txtName.Text?.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Name is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var db = new AppDbContext();
            if (db.Products.Any(p => p.Name.ToLower() == name.ToLower()))
            {
                MessageBox.Show("A product with this name already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var product = new Product
            {
                Name = name,
                BuyPrice = numBuyPrice.Value,
                SellPrice = numSellPrice.Value,
                Quantity = (int)numQuantity.Value,
                Unit = (ProductUnit)cmbUnit.SelectedIndex
            };
            db.Products.Add(product);
            db.SaveChanges();
            _currentProductPage = 1;
            LoadProducts();
        }

        private void BtnUpdate_Click(object? sender, EventArgs e)
        {
            if (!selectedProductId.HasValue)
            {
                MessageBox.Show("Select a product to update.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var db = new AppDbContext();
            var product = db.Products.Find(selectedProductId.Value);
            if (product == null) return;
            var updatedName = txtName.Text?.Trim() ?? string.Empty;
            if (db.Products.Any(p => p.Name.ToLower() == updatedName.ToLower() && p.Id != selectedProductId.Value))
            {
                MessageBox.Show("A product with this name already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            product.Name = updatedName;
            product.BuyPrice = numBuyPrice.Value;
            product.SellPrice = numSellPrice.Value;
            product.Quantity = (int)numQuantity.Value;
            product.Unit = (ProductUnit)cmbUnit.SelectedIndex;
            db.SaveChanges();
            _currentProductPage = 1;
            LoadProducts();
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (!selectedProductId.HasValue)
            {
                MessageBox.Show("Select a product to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var ok = MessageBox.Show("Are you sure you want to delete the selected product?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ok != DialogResult.Yes) return;

            using var db = new AppDbContext();
            var product = db.Products.Find(selectedProductId.Value);
            if (product == null) return;
            var deletedProductId = product.Id;
            db.Products.Remove(product);
            db.SaveChanges();
            ResequenceProducts(db, deletedProductId);
            _currentProductPage = 1;
            LoadProducts();
        }

        private void ChkShowBuyPrice_CheckedChanged(object? sender, EventArgs e)
        {
            bool show = chkShowBuyPrice.Checked;
            lblBuyPrice.Visible = show;
            numBuyPrice.Visible = show;
            if (dgvProducts.Columns.Contains("BuyPrice"))
                dgvProducts.Columns["BuyPrice"].Visible = show;
        }

        private void BtnClear_Click(object? sender, EventArgs e)
        {
            ClearForm(true);
        }

        private void ClearForm(bool clearSelection)
        {
            txtName.Text = string.Empty;
            numBuyPrice.Value = 0;
            numSellPrice.Value = 0;
            numQuantity.Value = 0;
            cmbUnit.SelectedIndex = 2; // default: Piece
            selectedProductId = null;
            if (clearSelection && dgvProducts.CurrentRow != null)
            {
                dgvProducts.ClearSelection();
            }
        }

        private void DgvProducts_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null || dgvProducts.CurrentRow.Cells.Count == 0)
            {
                ClearForm(false);
                return;
            }

            object? idValue = null;
            if (dgvProducts.Columns.Contains("Id"))
            {
                idValue = dgvProducts.CurrentRow.Cells["Id"].Value;
            }
            else
            {
                idValue = dgvProducts.CurrentRow.Cells[0].Value;
            }

            if (idValue == null) { ClearForm(false); return; }

            if (!int.TryParse(idValue.ToString(), out var id)) { ClearForm(false); return; }

            using var db = new AppDbContext();
            var product = db.Products.Find(id);
            if (product == null)
            {
                ClearForm(false);
                return;
            }

            selectedProductId = product.Id;
            txtName.Text = product.Name;
            numBuyPrice.Value = product.BuyPrice;
            numSellPrice.Value = product.SellPrice;
            numQuantity.Value = product.Quantity;
            cmbUnit.SelectedIndex = (int)product.Unit;
        }

    private void DgvProducts_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (dgvProducts?.Columns == null || dgvProducts.Columns.Count == 0 || e.RowIndex < 0) return;
        if (!dgvProducts.Columns.Contains("Quantity")) return;
        var quantityColumn = dgvProducts.Columns["Quantity"];
        if (quantityColumn == null || quantityColumn.Index != e.ColumnIndex) return;
        if (e.Value is int qty && e.CellStyle != null)
        {
            e.CellStyle.BackColor = qty < 5
                ? Color.LightCoral
                : qty <= 10
                    ? Color.Orange
                    : Color.LightGreen;
        }
    }

    private void DgvProducts_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        // Populate the form directly from the selected row's cells (fallback if SelectionChanged isn't enough)
        if (e.RowIndex < 0) return;
        var row = dgvProducts.Rows[e.RowIndex];
        if (row == null) return;

        object? idVal = null;
        object? nameVal = null;
        object? buyPriceVal = null;
        object? sellPriceVal = null;
        object? qtyVal = null;

        if (dgvProducts.Columns.Contains("Id")) idVal = row.Cells["Id"].Value;
        if (dgvProducts.Columns.Contains("Name")) nameVal = row.Cells["Name"].Value;
        if (dgvProducts.Columns.Contains("BuyPrice")) buyPriceVal = row.Cells["BuyPrice"].Value;
        if (dgvProducts.Columns.Contains("SellPrice")) sellPriceVal = row.Cells["SellPrice"].Value;
        if (dgvProducts.Columns.Contains("Quantity")) qtyVal = row.Cells["Quantity"].Value;

        object? unitVal = null;
        if (dgvProducts.Columns.Contains("Unit")) unitVal = row.Cells["Unit"].Value;

        // fallback by index
        if (idVal == null && row.Cells.Count > 0) idVal = row.Cells[0].Value;
        if (nameVal == null && row.Cells.Count > 1) nameVal = row.Cells[1].Value;
        if (buyPriceVal == null && row.Cells.Count > 2) buyPriceVal = row.Cells[2].Value;
        if (sellPriceVal == null && row.Cells.Count > 3) sellPriceVal = row.Cells[3].Value;
        if (qtyVal == null && row.Cells.Count > 4) qtyVal = row.Cells[4].Value;

        if (idVal != null && int.TryParse(idVal.ToString(), out var id)) selectedProductId = id;
        else selectedProductId = null;

        txtName.Text = nameVal?.ToString() ?? string.Empty;

        if (decimal.TryParse(buyPriceVal?.ToString(), out var buyPrice)) numBuyPrice.Value = buyPrice;
        else numBuyPrice.Value = 0;

        if (decimal.TryParse(sellPriceVal?.ToString(), out var sellPrice)) numSellPrice.Value = sellPrice;
        else numSellPrice.Value = 0;

        if (int.TryParse(qtyVal?.ToString(), out var qty)) numQuantity.Value = qty;
        else numQuantity.Value = 0;

        if (unitVal is ProductUnit unit) cmbUnit.SelectedIndex = (int)unit;
        else if (Enum.TryParse<ProductUnit>(unitVal?.ToString(), out var unitParsed)) cmbUnit.SelectedIndex = (int)unitParsed;
        else cmbUnit.SelectedIndex = 2;
    }

    // ── Sales Tab ────────────────────────────────────────────────────────────

    private void TabControlMain_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (tabControlMain.SelectedTab == tabSales)
            LoadSalesTab();
        else if (tabControlMain.SelectedTab == tabStats)
            LoadStats();
    }

    private void LoadSalesTab()
    {
        // populate product combo
        using var db = new AppDbContext();
        var products = db.Products.OrderBy(p => p.Name).ToList();
        cmbSaleProduct.DataSource = products;
        cmbSaleProduct.DisplayMember = "Name";
        cmbSaleProduct.ValueMember = "Id";

        LoadSales();
    }

    private void LoadSales()
    {
        using var db = new AppDbContext();
        var totalCount = db.Sales.Count();
        var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)SalePageSize));
        _currentSalePage = Math.Clamp(_currentSalePage, 1, totalPages);

        var sales = db.Sales
            .Include(s => s.Product)
            .OrderBy(s => s.Id)
            .Skip((_currentSalePage - 1) * SalePageSize)
            .Take(SalePageSize)
            .Select(s => new
            {
                s.Id,
                Product = s.Product.Name,
                s.Quantity,
                UnitPrice = s.UnitPrice,
                Total = s.TotalPrice,
                Date = s.SaleDate
            })
            .ToList();
        dgvSales.DataSource = sales;

        if (dgvSales.Columns.Contains("Date"))
            dgvSales.Columns["Date"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";

        RefreshInvoiceCheckboxColumn();

        lblSalePage.Text = $"Page {_currentSalePage} / {totalPages}";
        btnSalePrev.Enabled = _currentSalePage > 1;
        btnSaleNext.Enabled = _currentSalePage < totalPages;
    }

    private void TxtBuyerName_TextChanged(object? sender, EventArgs e)
    {
        RefreshInvoiceCheckboxColumn();
    }

    private void RefreshInvoiceCheckboxColumn()
    {
        bool show = !string.IsNullOrWhiteSpace(txtBuyerName.Text);

        if (!dgvSales.Columns.Contains("Select"))
        {
            var chk = new DataGridViewCheckBoxColumn
            {
                Name = "Select",
                HeaderText = "✔",
                Width = 40,
                DisplayIndex = 0,
                FlatStyle = FlatStyle.Standard,
                ReadOnly = false
            };
            dgvSales.Columns.Add(chk);
        }

        dgvSales.Columns["Select"].Visible = show;

        // Make all data columns read-only; only the checkbox column stays editable
        dgvSales.ReadOnly = false;
        foreach (DataGridViewColumn col in dgvSales.Columns)
        {
            if (col.Name != "Select")
                col.ReadOnly = true;
        }
    }

    private void BtnPrintInvoice_Click(object? sender, EventArgs e)
    {
        var buyerName = txtBuyerName.Text.Trim();
        if (string.IsNullOrWhiteSpace(buyerName))
        {
            MessageBox.Show("Enter a buyer name to enable invoice printing.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var selectedIds = new System.Collections.Generic.List<int>();
        foreach (DataGridViewRow row in dgvSales.Rows)
        {
            if (dgvSales.Columns.Contains("Select") &&
                row.Cells["Select"] is DataGridViewCheckBoxCell chkCell &&
                chkCell.Value is true)
            {
                if (row.Cells["Id"]?.Value != null &&
                    int.TryParse(row.Cells["Id"].Value.ToString(), out var rowId))
                    selectedIds.Add(rowId);
            }
        }

        if (selectedIds.Count == 0)
        {
            MessageBox.Show("Select at least one sale to include in the invoice.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var db = new AppDbContext();
        var sales = db.Sales
            .Include(s => s.Product)
            .Where(s => selectedIds.Contains(s.Id))
            .ToList();

        foreach (var s in sales)
            s.BuyerName = buyerName;

        new KabyliaTaste.Services.InvoicePrinter(sales, buyerName).PrintPreview();
    }

    private void CmbSaleProduct_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (cmbSaleProduct.SelectedValue is int productId)
        {
            using var db = new AppDbContext();
            var product = db.Products.Find(productId);
            if (product != null)
                numSaleUnitPrice.Value = product.SellPrice;
        }
        UpdateSaleTotal();
    }

    private void SaleInputChanged(object? sender, EventArgs e) => UpdateSaleTotal();

    private void UpdateSaleTotal()
    {
        var qty = decimal.TryParse(numSaleQuantity.Text, out var parsed) ? parsed : numSaleQuantity.Value;
        lblSaleTotalValue.Text = (qty * numSaleUnitPrice.Value).ToString("F2");
    }

    private void BtnSell_Click(object? sender, EventArgs e)
    {
        if (cmbSaleProduct.SelectedValue is not int productId)
        {
            MessageBox.Show("Select a product.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var qty = (int)numSaleQuantity.Value;
        var unitPrice = numSaleUnitPrice.Value;

        using var db = new AppDbContext();
        var product = db.Products.Find(productId);
        if (product == null) return;

        if (qty > product.Quantity)
        {
            MessageBox.Show($"Not enough stock. Available: {product.Quantity}.", "Stock Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        product.Quantity -= qty;

        var sale = new Sale
        {
            ProductId = productId,
            Quantity = qty,
            UnitPrice = unitPrice,
            TotalPrice = qty * unitPrice,
            SaleDate = DateTime.Now
        };
        db.Sales.Add(sale);
        db.SaveChanges();

        _currentSalePage = 1;
        LoadSales();
        LoadProducts();
        MessageBox.Show("Sale recorded successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void DgvSales_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvSales.CurrentRow == null) { selectedSaleId = null; return; }
        var idCell = dgvSales.CurrentRow.Cells["Id"];
        if (idCell?.Value != null && int.TryParse(idCell.Value.ToString(), out var id))
            selectedSaleId = id;
        else
            selectedSaleId = null;
    }

    private void BtnDeleteSale_Click(object? sender, EventArgs e)
    {
        if (!selectedSaleId.HasValue)
        {
            MessageBox.Show("Select a sale to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var ok = MessageBox.Show("Delete this sale? The stock will be restored.", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (ok != DialogResult.Yes) return;

        using var db = new AppDbContext();
        var sale = db.Sales.Include(s => s.Product).FirstOrDefault(s => s.Id == selectedSaleId.Value);
        if (sale == null) return;

        sale.Product.Quantity += sale.Quantity;
        var deletedSaleId = sale.Id;
        db.Sales.Remove(sale);
        db.SaveChanges();
        ResequenceSales(db, deletedSaleId);

        selectedSaleId = null;
        _currentSalePage = 1;
        LoadSales();
        LoadProducts();
    }

    private void LoadStats()
    {
        using var db = new AppDbContext();
        var stats = db.Sales
            .Include(s => s.Product)
            .GroupBy(s => s.Product.Name)
            .Select(g => new
            {
                Product = g.Key,
                UnitsSold = g.Sum(s => s.Quantity),
                Revenue = g.Sum(s => s.TotalPrice),
                Cost = g.Sum(s => s.Quantity * s.Product.BuyPrice),
                Profit = g.Sum(s => s.TotalPrice - s.Quantity * s.Product.BuyPrice)
            })
            .OrderByDescending(x => x.Profit)
            .ToList();

        dgvStats.DataSource = stats;

        var totalProfit = stats.Sum(x => x.Profit);
        lblTotalProfitValue.Text = totalProfit.ToString("F2");
        lblTotalProfitValue.ForeColor = totalProfit >= 0 ? System.Drawing.Color.Green : System.Drawing.Color.Red;
    }

    private void BtnSalePrev_Click(object? sender, EventArgs e)
    {
        _currentSalePage--;
        LoadSales();
    }

    private void BtnSaleNext_Click(object? sender, EventArgs e)
    {
        _currentSalePage++;
        LoadSales();
    }

    private void BtnProductPrev_Click(object? sender, EventArgs e)
    {
        _currentProductPage--;
        LoadProducts(_productFilter);
    }

    private void BtnProductNext_Click(object? sender, EventArgs e)
    {
        _currentProductPage++;
        LoadProducts(_productFilter);
    }

    private static void ResequenceProducts(AppDbContext db, int deletedId)
    {
        db.Database.ExecuteSqlRaw("PRAGMA foreign_keys = OFF");
        db.Database.ExecuteSqlRaw("UPDATE Sales SET ProductId = ProductId - 1 WHERE ProductId > {0}", deletedId);
        db.Database.ExecuteSqlRaw("UPDATE Products SET Id = Id - 1 WHERE Id > {0}", deletedId);
        db.Database.ExecuteSqlRaw("PRAGMA foreign_keys = ON");
        db.Database.ExecuteSqlRaw("UPDATE sqlite_sequence SET seq = (SELECT IFNULL(MAX(Id), 0) FROM Products) WHERE name = 'Products'");
        db.Database.ExecuteSqlRaw("UPDATE sqlite_sequence SET seq = (SELECT IFNULL(MAX(Id), 0) FROM Sales) WHERE name = 'Sales'");
    }

    private static void ResequenceSales(AppDbContext db, int deletedId)
    {
        db.Database.ExecuteSqlRaw("UPDATE Sales SET Id = Id - 1 WHERE Id > {0}", deletedId);
        db.Database.ExecuteSqlRaw("UPDATE sqlite_sequence SET seq = (SELECT IFNULL(MAX(Id), 0) FROM Sales) WHERE name = 'Sales'");
    }
    }
}
