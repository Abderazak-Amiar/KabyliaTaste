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
            txtSearch.TextChanged += TxtSearch_TextChanged;
            // Sales tab events
            tabControlMain.SelectedIndexChanged += TabControlMain_SelectedIndexChanged;
            cmbSaleProduct.SelectedIndexChanged += CmbSaleProduct_SelectedIndexChanged;
            numSaleQuantity.ValueChanged += SaleInputChanged;
            numSaleQuantity.TextChanged += SaleInputChanged;
            numSaleUnitPrice.ValueChanged += SaleInputChanged;
            btnSell.Click += BtnSell_Click;
            btnDeleteSale.Click += BtnDeleteSale_Click;
            dgvSales.SelectionChanged += DgvSales_SelectionChanged;
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            LoadProducts(txtSearch.Text.Trim());
        }

        private void Main_Load(object? sender, EventArgs e)
        {
            LoadProducts();
        }

        private void LoadProducts(string filter = "")
        {
            using var db = new AppDbContext();
            var query = db.Products.OrderBy(p => p.Id).AsQueryable();
            if (!string.IsNullOrEmpty(filter))
                query = query.Where(p => p.Name.ToLower().Contains(filter.ToLower()));
            var list = query.ToList();
            dgvProducts.DataSource = list;
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
                Price = numPrice.Value,
                Quantity = (int)numQuantity.Value,
                Unit = (ProductUnit)cmbUnit.SelectedIndex
            };
            db.Products.Add(product);
            db.SaveChanges();
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
            product.Price = numPrice.Value;
            product.Quantity = (int)numQuantity.Value;
            product.Unit = (ProductUnit)cmbUnit.SelectedIndex;
            db.SaveChanges();
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
            db.Products.Remove(product);
            db.SaveChanges();
            LoadProducts();
        }

        private void BtnClear_Click(object? sender, EventArgs e)
        {
            ClearForm(true);
        }

        private void ClearForm(bool clearSelection)
        {
            txtName.Text = string.Empty;
            numPrice.Value = 0;
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
            numPrice.Value = product.Price;
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
        object? priceVal = null;
        object? qtyVal = null;

        if (dgvProducts.Columns.Contains("Id")) idVal = row.Cells["Id"].Value;
        if (dgvProducts.Columns.Contains("Name")) nameVal = row.Cells["Name"].Value;
        if (dgvProducts.Columns.Contains("Price")) priceVal = row.Cells["Price"].Value;
        if (dgvProducts.Columns.Contains("Quantity")) qtyVal = row.Cells["Quantity"].Value;

        object? unitVal = null;
        if (dgvProducts.Columns.Contains("Unit")) unitVal = row.Cells["Unit"].Value;

        // fallback by index
        if (idVal == null && row.Cells.Count > 0) idVal = row.Cells[0].Value;
        if (nameVal == null && row.Cells.Count > 1) nameVal = row.Cells[1].Value;
        if (priceVal == null && row.Cells.Count > 2) priceVal = row.Cells[2].Value;
        if (qtyVal == null && row.Cells.Count > 3) qtyVal = row.Cells[3].Value;

        if (idVal != null && int.TryParse(idVal.ToString(), out var id)) selectedProductId = id;
        else selectedProductId = null;

        txtName.Text = nameVal?.ToString() ?? string.Empty;

        if (decimal.TryParse(priceVal?.ToString(), out var price)) numPrice.Value = price;
        else numPrice.Value = 0;

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
        var sales = db.Sales
            .Include(s => s.Product)
            .OrderByDescending(s => s.SaleDate)
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
    }

    private void CmbSaleProduct_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (cmbSaleProduct.SelectedValue is int productId)
        {
            using var db = new AppDbContext();
            var product = db.Products.Find(productId);
            if (product != null)
                numSaleUnitPrice.Value = product.Price;
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
        db.Sales.Remove(sale);
        db.SaveChanges();

        selectedSaleId = null;
        LoadSales();
        LoadProducts();
    }
    }
}
