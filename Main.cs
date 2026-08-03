namespace KabyliaTaste
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using KabyliaTaste.Data;
    using KabyliaTaste.Models;
    using KabyliaTaste.Services;
    using Microsoft.EntityFrameworkCore;

    public partial class Main : Form
    {
        private sealed class InvoiceGridRow
        {
            public int Id { get; set; }
            public string Buyer { get; set; } = string.Empty;
            public DateTime Date { get; set; }
            public string Hour { get; set; } = string.Empty;
            public decimal Total { get; set; }
            public decimal? Paid { get; set; }
            public decimal DueAmount { get; set; }
            public string Status { get; set; } = "No";
        }

        private sealed class StatsGridRow
        {
            public string Product { get; set; } = string.Empty;
            public DateTime Date { get; set; }
            public string Hour { get; set; } = string.Empty;
            public int UnitsSold { get; set; }
            public decimal Revenue { get; set; }
            public decimal Cost { get; set; }
            public decimal Profit { get; set; }
        }

        private int? selectedProductId = null;
        private int? selectedSaleId = null;
        private int? selectedExpenseId = null;
        private int _currentSalePage = 1;
        private const int SalePageSize = 20;
        private int _currentProductPage = 1;
        private const int ProductPageSize = 20;
        private int _currentExpensePage = 1;
        private const int ExpensePageSize = 20;
        private int _currentInvoicePage = 1;
        private const int InvoicePageSize = 20;
        private string _productFilter = "";
        private string _expenseCategoryFilter = "";
        private bool _closingAfterBackup = false;

        public bool LogoutRequested { get; private set; } = false;

        public Main()
        {
            InitializeComponent();

            // wire events
            Load += Main_Load;
            FormClosing += Main_FormClosing;
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
            btnUpdateSale.Click += BtnUpdateSale_Click;
            btnClearSale.Click += BtnClearSale_Click;
            btnPrintInvoice.Click += BtnPrintInvoice_Click;
            txtBuyerName.TextChanged += TxtBuyerName_TextChanged;
            dgvSales.SelectionChanged += DgvSales_SelectionChanged;
            btnSalePrev.Click += BtnSalePrev_Click;
            btnSaleNext.Click += BtnSaleNext_Click;
            cmbFilterBuyer.TextChanged += SaleFilter_Changed;
            cmbFilterProduct.SelectedIndexChanged += SaleFilter_Changed;
            chkFilterDate.CheckedChanged += ChkFilterDate_CheckedChanged;
            dtpFilterDate.ValueChanged += SaleFilter_Changed;
            btnClearSaleFilter.Click += BtnClearSaleFilter_Click;
            btnPrintReport.Click += BtnPrintReport_Click;
            btnAddExpense.Click += BtnAddExpense_Click;
            btnUpdateExpense.Click += BtnUpdateExpense_Click;
            btnDeleteExpense.Click += BtnDeleteExpense_Click;
            btnClearExpense.Click += BtnClearExpense_Click;
            dgvExpenses.SelectionChanged += DgvExpenses_SelectionChanged;
            cmbExpenseFilterCategory.SelectedIndexChanged += ExpenseFilter_Changed;
            btnClearExpenseFilter.Click += BtnClearExpenseFilter_Click;
            btnExpensePrev.Click += BtnExpensePrev_Click;
            btnExpenseNext.Click += BtnExpenseNext_Click;
            btnLogout.Click += BtnLogout_Click;
            btnChangePassword.Click += BtnChangePassword_Click;
            btnSaveStoreName.Click += BtnSaveStoreName_Click;
            btnChangeLogo.Click += BtnChangeLogo_Click;
            btnSaveGoogleDriveConfig.Click += BtnSaveGoogleDriveConfig_Click;
            btnGenerateGoogleDriveRefreshToken.Click += BtnGenerateGoogleDriveRefreshToken_Click;
            btnGoogleDriveHelp.Click += BtnGoogleDriveHelp_Click;
            btnOpenGoogleCloudConsole.Click += BtnOpenGoogleCloudConsole_Click;
            btnDownloadDatabaseBackup.Click += BtnDownloadDatabaseBackup_Click;
            btnRestoreDatabaseBackup.Click += BtnRestoreDatabaseBackup_Click;
            btnUploadGoogleDriveBackup.Click += BtnUploadGoogleDriveBackup_Click;
            btnDownloadGoogleDriveBackup.Click += BtnDownloadGoogleDriveBackup_Click;
            btnChangeUsername.Click += BtnChangeUsername_Click;
            timerClock.Tick += TimerClock_Tick;
            Resize += (s, e) => UpdateDateTime();
            cmbInvoiceFilterBuyer.SelectedIndexChanged += InvoiceFilter_Changed;
            cmbInvoiceFilterStatus.SelectedIndexChanged += InvoiceFilter_Changed;
            btnClearInvoiceFilter.Click += BtnClearInvoiceFilter_Click;
            btnInvoicePrev.Click += BtnInvoicePrev_Click;
            btnInvoiceNext.Click += BtnInvoiceNext_Click;
            btnInvoicePreview.Click += BtnInvoicePreview_Click;
            dgvInvoices.CellValueChanged += DgvInvoices_CellValueChanged;
            dgvInvoices.CurrentCellDirtyStateChanged += DgvInvoices_CurrentCellDirtyStateChanged;
            dgvInvoices.CellParsing += DgvInvoices_CellParsing;
            dgvInvoices.CellValidating += DgvInvoices_CellValidating;
            dgvInvoices.DataError += DgvInvoices_DataError;
        }

        private async void Main_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (_closingAfterBackup || e.CloseReason != CloseReason.UserClosing)
                return;

            e.Cancel = true;

            try
            {
                var store = GetGoogleDriveConfiguredStore();
                if (string.IsNullOrWhiteSpace(store.GoogleDriveClientId) ||
                    string.IsNullOrWhiteSpace(store.GoogleDriveClientSecret) ||
                    string.IsNullOrWhiteSpace(store.GoogleDriveRefreshToken))
                {
                    _closingAfterBackup = true;
                    Close();
                    return;
                }

                using var progressForm = new BackupProgressForm();
                progressForm.Show(this);
                progressForm.SetProgress(0, "Preparing database backup...");

                var progress = new Progress<int>(percent =>
                {
                    progressForm.SetProgress(percent, $"Uploading database backup... {percent}%");
                });

                await Task.Run(() =>
                {
                    var service = new GoogleDriveBackupService();
                    service.UploadDatabaseBackup(store, GetDatabaseFilePath(), progress);
                });

                progressForm.SetProgress(100, "Upload complete.");
                _closingAfterBackup = true;
                Close();
            }
            catch (InvalidOperationException)
            {
                _closingAfterBackup = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to upload database to Google Drive before closing: {ex.Message}",
                    "Google Drive",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            _currentProductPage = 1;
            LoadProducts(txtSearch.Text.Trim());
        }

        private void Main_Load(object? sender, EventArgs e)
        {
            using var db = new AppDbContext();
            var store = db.StoreSettings.FirstOrDefault();
            if (store != null)
            {
                this.Text = store.StoreName;
                if (store.LogoData != null && store.LogoData.Length > 0)
                {
                    using var ms = new System.IO.MemoryStream(store.LogoData);
                    var img = System.Drawing.Image.FromStream(ms);
                    // Convert image to icon for the title bar / taskbar
                    var bmp = new System.Drawing.Bitmap(img, 32, 32);
                    this.Icon = System.Drawing.Icon.FromHandle(bmp.GetHicon());
                }
            }

            ApplyRoleRestrictions();
            UpdateGreeting();
            UpdateDateTime();
            CompactProductIds(db);
            LoadProducts();
        }

        private void ApplyRoleRestrictions()
        {
            bool isAdmin = Session.IsAdmin;

            // Products tab: hide the entire form panel for non-admins
            panelLeft.Visible = isAdmin;

            // Sales tab: hide delete sale and restrict unit price editing for non-admins
            btnDeleteSale.Visible = isAdmin;
            numSaleUnitPrice.Enabled = isAdmin;

            // Hide Stats and Settings tabs for non-admins
            if (!isAdmin)
            {
                tabControlMain.TabPages.Remove(tabStats);
                tabControlMain.TabPages.Remove(tabExpenses);
                tabControlMain.TabPages.Remove(tabSettings);
            }
        }

        private void UpdateGreeting()
        {
            var hour = DateTime.Now.Hour;
            var timeOfDay = hour < 12 ? "Good morning" : hour < 18 ? "Good afternoon" : "Good evening";
            var role     = Session.CurrentUser?.IsAdmin == true ? "admin" : "user";
            var username = Session.CurrentUser?.Username ?? "";

            lblGreeting.SetSegments(new[]
            {
                new KabyliaTaste.Controls.MixedFontLabel.Segment($"{timeOfDay}, ",          Bold: false),
                new KabyliaTaste.Controls.MixedFontLabel.Segment(username,                  Bold: true),
                new KabyliaTaste.Controls.MixedFontLabel.Segment("  |  You're logged in as: ", Bold: false),
                new KabyliaTaste.Controls.MixedFontLabel.Segment(role,                      Bold: true),
            });

            UpdateDateTime();
        }

        private void UpdateDateTime()
        {
            lblDateTime.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy   HH:mm:ss");
            // Centre in the gap between the greeting and the logout button
            int areaLeft  = lblGreeting.Right + 5;
            int areaRight = btnLogout.Left - 5;
            int center    = areaLeft + (areaRight - areaLeft - lblDateTime.Width) / 2;
            lblDateTime.Left = Math.Max(areaLeft, center);
        }

        private void TimerClock_Tick(object? sender, EventArgs e)
        {
            UpdateDateTime();
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
            dgvProducts.Columns["BuyPrice"].Visible = Session.IsAdmin && chkShowBuyPrice.Checked;
        if (dgvProducts.Columns.Contains("Date"))
        {
            dgvProducts.Columns["Date"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvProducts.Columns["Date"].HeaderText = "Date";
        }
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
                Id = (db.Products.Any() ? db.Products.Max(p => p.Id) : 0) + 1,
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
            product.Date = DateTime.Now;
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
            db.Products.Remove(product);
            db.SaveChanges();
            CompactProductIds(db);
            _currentProductPage = 1;
            LoadProducts();
        }

        private void ChkShowBuyPrice_CheckedChanged(object? sender, EventArgs e)
        {
            bool show = Session.IsAdmin && chkShowBuyPrice.Checked;
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

            if (qty == 0)
            {
                e.Value = "0  ⚠ Out of stock";
                e.FormattingApplied = true;
            }
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
            LoadStatsTab();
        else if (tabControlMain.SelectedTab == tabExpenses)
            LoadExpensesTab();
        else if (tabControlMain.SelectedTab == tabInvoices)
            LoadInvoicesTab();
        else if (tabControlMain.SelectedTab == tabSettings)
            LoadSettingsTab();
    }

    private void LoadStatsTab()
    {
        using var db = new AppDbContext();
        var products = db.Products.OrderBy(p => p.Name).Select(p => p.Name).ToList();
        products.Insert(0, "");
        cmbStatsProduct.SelectedIndexChanged -= StatsFilter_Changed;
        cmbStatsProduct.DataSource = products;
        cmbStatsProduct.SelectedIndex = 0;
        cmbStatsProduct.SelectedIndexChanged += StatsFilter_Changed;

        var buyers = db.Sales
            .Where(s => s.BuyerName != null && s.BuyerName != "")
            .Select(s => s.BuyerName!)
            .Distinct()
            .OrderBy(b => b)
            .ToList();
        buyers.Insert(0, "");
        cmbStatsBuyer.SelectedIndexChanged -= StatsFilter_Changed;
        cmbStatsBuyer.DataSource = buyers;
        cmbStatsBuyer.SelectedIndex = 0;
        cmbStatsBuyer.SelectedIndexChanged += StatsFilter_Changed;

        LoadStats();
    }

    private void LoadSalesTab()
    {
        using var db = new AppDbContext();
        var products = db.Products.OrderBy(p => p.Name).ToList();
        cmbSaleProduct.DataSource = products;
        cmbSaleProduct.DisplayMember = "Name";
        cmbSaleProduct.ValueMember = "Id";

        // populate filter buyer dropdown
        var buyers = db.Sales
            .Where(s => s.BuyerName != null && s.BuyerName != "")
            .Select(s => s.BuyerName!)
            .Distinct()
            .OrderBy(b => b)
            .ToList();
        buyers.Insert(0, "");
        cmbFilterBuyer.DataSource = buyers;
        cmbFilterBuyer.SelectedIndex = 0;

        // autocomplete on the buyer name input
        var buyerAc = new AutoCompleteStringCollection();
        buyerAc.AddRange(buyers.Where(b => b != "").ToArray());
        txtBuyerName.AutoCompleteCustomSource = buyerAc;
        txtBuyerName.AutoCompleteMode   = AutoCompleteMode.SuggestAppend;
        txtBuyerName.AutoCompleteSource = AutoCompleteSource.CustomSource;

        // populate filter product dropdown
        var filterProducts = db.Products.OrderBy(p => p.Name).Select(p => p.Name).ToList();
        filterProducts.Insert(0, "");
        cmbFilterProduct.DataSource = filterProducts;
        cmbFilterProduct.SelectedIndex = 0;

        LoadSales();
    }

    private void LoadSales()
    {
        var buyerFilter = cmbFilterBuyer?.Text?.Trim() ?? "";
        var productFilter = cmbFilterProduct?.SelectedItem as string ?? "";
        var filterDate = chkFilterDate?.Checked == true ? dtpFilterDate?.Value.Date : (DateTime?)null;

        using var db = new AppDbContext();
        var query = db.Sales.Include(s => s.Product).AsQueryable();

        if (!string.IsNullOrEmpty(buyerFilter))
            query = query.Where(s => s.BuyerName != null && s.BuyerName.ToLower().Contains(buyerFilter.ToLower()));

        if (!string.IsNullOrEmpty(productFilter))
            query = query.Where(s => s.Product.Name == productFilter);

        if (filterDate.HasValue)
            query = query.Where(s => s.SaleDate.Date == filterDate.Value);

        var totalCount = query.Count();
        var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)SalePageSize));
        _currentSalePage = Math.Clamp(_currentSalePage, 1, totalPages);

        var sales = query
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
                Date = s.SaleDate,
                Buyer = s.BuyerName
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

        var invoice = new KabyliaTaste.Models.Invoice
        {
            BuyerName = buyerName,
            Date = DateTime.Now,
            TotalAmount = sales.Sum(s => s.TotalPrice),
            PaymentStatus = KabyliaTaste.Models.InvoicePaymentStatus.No,
            AmountPaid = 0
        };
        db.Invoices.Add(invoice);
        db.SaveChanges();

        foreach (var s in sales)
            s.InvoiceId = invoice.Id;
        db.SaveChanges();

        using var dbStore1 = new AppDbContext();
        var storeForInvoice = dbStore1.StoreSettings.FirstOrDefault();
        new KabyliaTaste.Services.InvoicePrinter(
            sales,
            buyerName,
            storeForInvoice?.StoreName ?? "KabyliaTaste",
            storeForInvoice?.LogoData,
            invoice.Id,
            invoice.Date,
            invoice.TotalAmount,
            invoice.AmountPaid,
            invoice.PaymentStatus).PrintPreview();

        if (tabControlMain.SelectedTab == tabInvoices)
            LoadInvoices();
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
            Id = (db.Sales.Any() ? db.Sales.Max(s => s.Id) : 0) + 1,
            ProductId = productId,
            Quantity = qty,
            UnitPrice = unitPrice,
            TotalPrice = qty * unitPrice,
            SaleDate = DateTime.Now,
            BuyerName = string.IsNullOrWhiteSpace(txtBuyerName.Text) ? null : txtBuyerName.Text.Trim()
        };
        db.Sales.Add(sale);
        db.SaveChanges();

        _currentSalePage = 1;
        RefreshBuyerFilterDropdown();
        LoadSalesTab();
        LoadProducts();
        MessageBox.Show("Sale recorded successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void DgvSales_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvSales.CurrentRow == null) { selectedSaleId = null; return; }
        var idCell = dgvSales.CurrentRow.Cells["Id"];
        if (idCell?.Value != null && int.TryParse(idCell.Value.ToString(), out var id))
        {
            selectedSaleId = id;
            PopulateSaleForm(id);
        }
        else
        {
            selectedSaleId = null;
        }
    }

    private void PopulateSaleForm(int saleId)
    {
        using var db = new AppDbContext();
        var sale = db.Sales.Include(s => s.Product).FirstOrDefault(s => s.Id == saleId);
        if (sale == null) return;

        cmbSaleProduct.SelectedValue = sale.ProductId;
        numSaleQuantity.Value = sale.Quantity;
        numSaleUnitPrice.Value = sale.UnitPrice;
        txtBuyerName.Text = sale.BuyerName ?? string.Empty;
        UpdateSaleTotal();
    }

    private void BtnClearSale_Click(object? sender, EventArgs e) => ClearSaleForm();

    private void ClearSaleForm()
    {
        selectedSaleId = null;
        dgvSales.ClearSelection();
        if (cmbSaleProduct.Items.Count > 0)
            cmbSaleProduct.SelectedIndex = 0;
        numSaleQuantity.Value = 1;
        txtBuyerName.Clear();
        UpdateSaleTotal();
    }

    private void BtnUpdateSale_Click(object? sender, EventArgs e)
    {
        if (!selectedSaleId.HasValue)
        {
            MessageBox.Show("Select a sale to update.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (cmbSaleProduct.SelectedValue is not int productId)
        {
            MessageBox.Show("Select a product.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var newQty = (int)numSaleQuantity.Value;
        var newUnitPrice = numSaleUnitPrice.Value;

        using var db = new AppDbContext();
        var sale = db.Sales.Include(s => s.Product).FirstOrDefault(s => s.Id == selectedSaleId.Value);
        if (sale == null) return;

        // Restore old stock
        sale.Product.Quantity += sale.Quantity;

        // Apply new product if changed
        if (sale.ProductId != productId)
        {
            var newProduct = db.Products.Find(productId);
            if (newProduct == null) return;
            if (newQty > newProduct.Quantity)
            {
                MessageBox.Show($"Not enough stock. Available: {newProduct.Quantity}.", "Stock Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // Re-deduct the restored stock
                sale.Product.Quantity -= sale.Quantity;
                return;
            }
            newProduct.Quantity -= newQty;
            sale.ProductId = productId;
        }
        else
        {
            if (newQty > sale.Product.Quantity)
            {
                MessageBox.Show($"Not enough stock. Available: {sale.Product.Quantity}.", "Stock Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // Re-deduct the restored stock
                sale.Product.Quantity -= sale.Quantity;
                return;
            }
            sale.Product.Quantity -= newQty;
        }

        sale.Quantity = newQty;
        sale.UnitPrice = newUnitPrice;
        sale.TotalPrice = newQty * newUnitPrice;
        sale.BuyerName = string.IsNullOrWhiteSpace(txtBuyerName.Text) ? null : txtBuyerName.Text.Trim();

        db.SaveChanges();

        LoadSalesTab();
        LoadProducts();
        MessageBox.Show("Sale updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            var productFilter = cmbStatsProduct?.SelectedItem as string ?? "";
            var buyerFilter   = cmbStatsBuyer?.SelectedItem as string ?? "";
            var period = cmbStatsPeriod?.SelectedItem as string ?? "";
            var refDate = dtpStatsDate?.Value.Date ?? DateTime.Today;

            using var db = new AppDbContext();
            var query = db.Sales.Include(s => s.Product).AsQueryable();

            if (!string.IsNullOrEmpty(productFilter))
                query = query.Where(s => s.Product.Name == productFilter);

            if (!string.IsNullOrEmpty(buyerFilter))
                query = query.Where(s => s.BuyerName == buyerFilter);

            if (!string.IsNullOrEmpty(period))
            {
                var dayStart   = refDate;
                var dayEnd     = refDate.AddDays(1);
                var weekStart  = refDate.AddDays(-(int)refDate.DayOfWeek);
                var weekEnd    = weekStart.AddDays(7);
                var monthStart = new DateTime(refDate.Year, refDate.Month, 1);
                var monthEnd   = monthStart.AddMonths(1);
                var yearStart  = new DateTime(refDate.Year, 1, 1);
                var yearEnd    = yearStart.AddYears(1);

                query = period switch
                {
                    "Day"   => query.Where(s => s.SaleDate >= dayStart   && s.SaleDate < dayEnd),
                    "Week"  => query.Where(s => s.SaleDate >= weekStart  && s.SaleDate < weekEnd),
                    "Month" => query.Where(s => s.SaleDate >= monthStart && s.SaleDate < monthEnd),
                    "Year"  => query.Where(s => s.SaleDate >= yearStart  && s.SaleDate < yearEnd),
                    _ => query
                };
            }

            var filteredSales = query.ToList();

            var stats = filteredSales
                .Select(s => new StatsGridRow
                {
                    Product = s.Product.Name,
                    Date = s.SaleDate.Date,
                    Hour = s.SaleDate.ToString("HH:mm"),
                    UnitsSold = s.Quantity,
                    Revenue = s.TotalPrice,
                    Cost = s.Quantity * s.Product.BuyPrice,
                    Profit = s.TotalPrice - s.Quantity * s.Product.BuyPrice
                })
                .OrderByDescending(x => x.Date)
                .ThenByDescending(x => x.Hour)
                .ToList();

            dgvStats.DataSource = stats;

            var grossProfit = stats.Sum(x => x.Profit);
            var invoiceTotals = GetInvoiceTotals(db, filteredSales);
            var totalExpenses = db.Expenses.AsEnumerable().Sum(e => e.Amount);
            var netProfit = grossProfit - totalExpenses;
            lblTotalProfitValue.Text = $"{netProfit:F2}  (Gross: {grossProfit:F2}  Debt: {invoiceTotals.Debt:F2}  Expenses: {totalExpenses:F2})";
            lblTotalProfitValue.ForeColor = netProfit >= 0
                ? System.Drawing.Color.Green
                : System.Drawing.Color.Red;
        }

        private static (decimal Collected, decimal Debt) GetInvoiceTotals(AppDbContext db, IEnumerable<Sale> sales)
        {
            var invoiceIds = sales
                .Where(s => s.InvoiceId.HasValue)
                .Select(s => s.InvoiceId!.Value)
                .Distinct()
                .ToList();

            var invoices = db.Invoices
                .Where(i => invoiceIds.Contains(i.Id))
                .AsEnumerable();

            var collected = invoices.Sum(i => i.PaymentStatus switch
            {
                InvoicePaymentStatus.Yes => i.TotalAmount,
                InvoicePaymentStatus.PartiallyPaid => i.AmountPaid,
                _ => 0m
            });

            var debt = invoices.Sum(i => i.PaymentStatus switch
            {
                InvoicePaymentStatus.Yes => 0m,
                InvoicePaymentStatus.PartiallyPaid => i.TotalAmount - i.AmountPaid,
                _ => i.TotalAmount
            });

            return (collected, debt);
        }

    private void RefreshBuyerFilterDropdown()
    {
        var currentText = cmbFilterBuyer.Text;
        using var db = new AppDbContext();
        var buyers = db.Sales
            .Where(s => s.BuyerName != null && s.BuyerName != "")
            .Select(s => s.BuyerName!)
            .Distinct()
            .OrderBy(b => b)
            .ToList();
        buyers.Insert(0, "");
        cmbFilterBuyer.TextChanged -= SaleFilter_Changed;
        cmbFilterBuyer.DataSource = buyers;
        cmbFilterBuyer.Text = currentText;
        cmbFilterBuyer.TextChanged += SaleFilter_Changed;
    }

    private void SaleFilter_Changed(object? sender, EventArgs e)
    {
        _currentSalePage = 1;
        LoadSales();
    }

    private void StatsFilter_Changed(object? sender, EventArgs e)
    {
        LoadStats();
    }

    private void BtnClearStatsFilter_Click(object? sender, EventArgs e)
    {
        cmbStatsProduct.SelectedIndex = 0;
        cmbStatsBuyer.SelectedIndex = 0;
        cmbStatsPeriod.SelectedIndex = 0;
        dtpStatsDate.Value = DateTime.Today;
        LoadStats();
    }

    private void BtnPrintReport_Click(object? sender, EventArgs e)
    {
        var productFilter = cmbStatsProduct?.SelectedItem as string ?? "";
        var buyerFilter   = cmbStatsBuyer?.SelectedItem as string ?? "";
        var period = cmbStatsPeriod?.SelectedItem as string ?? "";
        var refDate = dtpStatsDate?.Value.Date ?? DateTime.Today;

        using var db = new AppDbContext();
        var query = db.Sales.Include(s => s.Product).AsQueryable();

        if (!string.IsNullOrEmpty(productFilter))
            query = query.Where(s => s.Product.Name == productFilter);

        if (!string.IsNullOrEmpty(buyerFilter))
            query = query.Where(s => s.BuyerName == buyerFilter);

        if (!string.IsNullOrEmpty(period))
        {
            var dayStart   = refDate;
            var dayEnd     = refDate.AddDays(1);
            var weekStart  = refDate.AddDays(-(int)refDate.DayOfWeek);
            var weekEnd    = weekStart.AddDays(7);
            var monthStart = new DateTime(refDate.Year, refDate.Month, 1);
            var monthEnd   = monthStart.AddMonths(1);
            var yearStart  = new DateTime(refDate.Year, 1, 1);
            var yearEnd    = yearStart.AddYears(1);

            query = period switch
            {
                "Day"   => query.Where(s => s.SaleDate >= dayStart   && s.SaleDate < dayEnd),
                "Week"  => query.Where(s => s.SaleDate >= weekStart  && s.SaleDate < weekEnd),
                "Month" => query.Where(s => s.SaleDate >= monthStart && s.SaleDate < monthEnd),
                "Year"  => query.Where(s => s.SaleDate >= yearStart  && s.SaleDate < yearEnd),
                _ => query
            };
        }

        var filteredSales = query.ToList();

        var rows = filteredSales
            .Select(s => new KabyliaTaste.Services.StatsReportRow
            {
                Product   = s.Product.Name,
                Date      = s.SaleDate.Date,
                Hour      = s.SaleDate.ToString("HH:mm"),
                UnitsSold = s.Quantity,
                Revenue   = s.TotalPrice,
                Cost      = s.Quantity * s.Product.BuyPrice,
                Profit    = s.TotalPrice - s.Quantity * s.Product.BuyPrice
            })
            .OrderByDescending(x => x.Date)
            .ThenByDescending(x => x.Hour)
            .ToList();

        using var dbStore2 = new AppDbContext();
        var storeForReport = dbStore2.StoreSettings.FirstOrDefault();
        var invoiceTotals = GetInvoiceTotals(db, filteredSales);
        var totalExpenses = db.Expenses.AsEnumerable().Sum(e => e.Amount);
        new KabyliaTaste.Services.StatsReportPrinter(
            rows,
            productFilter,
            buyerFilter,
            period,
            refDate,
            storeForReport?.StoreName ?? "KabyliaTaste",
            storeForReport?.LogoData,
            invoiceTotals.Collected,
            invoiceTotals.Debt,
            totalExpenses).PrintPreview();
    }

    private void ChkFilterDate_CheckedChanged(object? sender, EventArgs e)
    {
        dtpFilterDate.Enabled = chkFilterDate.Checked;
        _currentSalePage = 1;
        LoadSales();
    }

    private void BtnClearSaleFilter_Click(object? sender, EventArgs e)
    {
        cmbFilterBuyer.Text = "";
        cmbFilterProduct.SelectedIndex = 0;
        chkFilterDate.Checked = false;
        dtpFilterDate.Enabled = false;
        _currentSalePage = 1;
        LoadSales();
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

    private static void CompactProductIds(AppDbContext db)
    {
        // Reassign all product IDs to be sequential (1, 2, 3, ...) fixing any gaps
        db.Database.ExecuteSqlRaw("PRAGMA foreign_keys = OFF");
        // Use negative IDs as a staging step to avoid unique constraint conflicts
        var products = db.Products.OrderBy(p => p.Id).ToList();
        int seq = 1;
        foreach (var p in products)
        {
            if (p.Id != seq)
            {
                db.Database.ExecuteSqlRaw("UPDATE Sales SET ProductId = {0} WHERE ProductId = {1}", -seq, p.Id);
                db.Database.ExecuteSqlRaw("UPDATE Products SET Id = {0} WHERE Id = {1}", -seq, p.Id);
            }
            seq++;
        }
        // Flip negatives to positives
        db.Database.ExecuteSqlRaw("UPDATE Sales SET ProductId = -ProductId WHERE ProductId < 0");
        db.Database.ExecuteSqlRaw("UPDATE Products SET Id = -Id WHERE Id < 0");
        db.Database.ExecuteSqlRaw("PRAGMA foreign_keys = ON");
        db.Database.ExecuteSqlRaw(
            "INSERT OR REPLACE INTO sqlite_sequence(name, seq) VALUES ('Products', (SELECT IFNULL(MAX(Id), 0) FROM Products))");
    }

    private static void ResequenceProducts(AppDbContext db, int deletedId)
    {
        db.Database.ExecuteSqlRaw("PRAGMA foreign_keys = OFF");
        db.Database.ExecuteSqlRaw("UPDATE Sales SET ProductId = ProductId - 1 WHERE ProductId > {0}", deletedId);
        db.Database.ExecuteSqlRaw("UPDATE Products SET Id = Id - 1 WHERE Id > {0}", deletedId);
        db.Database.ExecuteSqlRaw("PRAGMA foreign_keys = ON");
        db.Database.ExecuteSqlRaw(
            "INSERT OR REPLACE INTO sqlite_sequence(name, seq) VALUES ('Products', (SELECT IFNULL(MAX(Id), 0) FROM Products))");
        db.Database.ExecuteSqlRaw(
            "INSERT OR REPLACE INTO sqlite_sequence(name, seq) VALUES ('Sales', (SELECT IFNULL(MAX(Id), 0) FROM Sales))");
    }

    private static void ResequenceSales(AppDbContext db, int deletedId)
    {
        db.Database.ExecuteSqlRaw("UPDATE Sales SET Id = Id - 1 WHERE Id > {0}", deletedId);
        db.Database.ExecuteSqlRaw("UPDATE sqlite_sequence SET seq = (SELECT IFNULL(MAX(Id), 0) FROM Sales) WHERE name = 'Sales'");
    }

    // ── Expenses Tab ──────────────────────────────────────────────────────────

    private void LoadExpensesTab()
    {
        using var db = new AppDbContext();
        var categories = db.Expenses
            .Where(e => e.Category != null && e.Category != "")
            .Select(e => e.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToList();
        categories.Insert(0, "");
        cmbExpenseFilterCategory.SelectedIndexChanged -= ExpenseFilter_Changed;
        cmbExpenseFilterCategory.DataSource = categories;
        cmbExpenseFilterCategory.SelectedIndex = 0;
        cmbExpenseFilterCategory.SelectedIndexChanged += ExpenseFilter_Changed;
        LoadExpenses();
    }

    private void LoadExpenses()
    {
        using var db = new AppDbContext();
        var query = db.Expenses.OrderByDescending(e => e.Date).AsQueryable();
        if (!string.IsNullOrEmpty(_expenseCategoryFilter))
            query = query.Where(e => e.Category == _expenseCategoryFilter);

        var totalCount = query.Count();
        var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)ExpensePageSize));
        _currentExpensePage = Math.Clamp(_currentExpensePage, 1, totalPages);

        var list = query
            .Skip((_currentExpensePage - 1) * ExpensePageSize)
            .Take(ExpensePageSize)
            .ToList();
        dgvExpenses.DataSource = list;

        if (dgvExpenses.Columns.Contains("Date"))
            dgvExpenses.Columns["Date"].DefaultCellStyle.Format = "dd/MM/yyyy";

        lblExpensePage.Text = $"Page {_currentExpensePage} / {totalPages}";
        btnExpensePrev.Enabled = _currentExpensePage > 1;
        btnExpenseNext.Enabled = _currentExpensePage < totalPages;

        ClearExpenseForm(false);
        if (dgvExpenses.Rows.Count > 0)
        {
            dgvExpenses.ClearSelection();
            dgvExpenses.Rows[0].Selected = true;
            DgvExpenses_SelectionChanged(null, EventArgs.Empty);
        }
    }

    private void ClearExpenseForm(bool clearSelection = true)
    {
        selectedExpenseId = null;
        txtExpenseDescription.Clear();
        numExpenseAmount.Value = 0;
        txtExpenseCategory.Clear();
        dtpExpenseDate.Value = DateTime.Today;
        if (clearSelection && dgvExpenses.CurrentRow != null)
            dgvExpenses.ClearSelection();
    }

    private void DgvExpenses_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvExpenses.CurrentRow == null || dgvExpenses.CurrentRow.Index < 0) return;
        if (dgvExpenses.CurrentRow.DataBoundItem is not KabyliaTaste.Models.Expense expense) return;
        selectedExpenseId = expense.Id;
        txtExpenseDescription.Text = expense.Description;
        numExpenseAmount.Value = expense.Amount;
        txtExpenseCategory.Text = expense.Category;
        dtpExpenseDate.Value = expense.Date;
    }

    private void BtnAddExpense_Click(object? sender, EventArgs e)
    {
        var description = txtExpenseDescription.Text.Trim();
        if (string.IsNullOrEmpty(description))
        {
            MessageBox.Show("Description is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        using var db = new AppDbContext();
        var expense = new KabyliaTaste.Models.Expense
        {
            Description = description,
            Amount = numExpenseAmount.Value,
            Category = txtExpenseCategory.Text.Trim(),
            Date = dtpExpenseDate.Value.Date
        };
        db.Expenses.Add(expense);
        db.SaveChanges();
        _currentExpensePage = 1;
        LoadExpensesTab();
    }

    private void BtnUpdateExpense_Click(object? sender, EventArgs e)
    {
        if (!selectedExpenseId.HasValue)
        {
            MessageBox.Show("Select an expense to update.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        var description = txtExpenseDescription.Text.Trim();
        if (string.IsNullOrEmpty(description))
        {
            MessageBox.Show("Description is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        using var db = new AppDbContext();
        var expense = db.Expenses.Find(selectedExpenseId.Value);
        if (expense == null) return;
        expense.Description = description;
        expense.Amount = numExpenseAmount.Value;
        expense.Category = txtExpenseCategory.Text.Trim();
        expense.Date = dtpExpenseDate.Value.Date;
        db.SaveChanges();
        _currentExpensePage = 1;
        LoadExpensesTab();
    }

    private void BtnDeleteExpense_Click(object? sender, EventArgs e)
    {
        if (!selectedExpenseId.HasValue)
        {
            MessageBox.Show("Select an expense to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        var ok = MessageBox.Show("Are you sure you want to delete the selected expense?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (ok != DialogResult.Yes) return;
        using var db = new AppDbContext();
        var expense = db.Expenses.Find(selectedExpenseId.Value);
        if (expense == null) return;
        db.Expenses.Remove(expense);
        db.SaveChanges();
        _currentExpensePage = 1;
        LoadExpensesTab();
    }

    private void BtnClearExpense_Click(object? sender, EventArgs e) => ClearExpenseForm();

    private void ExpenseFilter_Changed(object? sender, EventArgs e)
    {
        _expenseCategoryFilter = cmbExpenseFilterCategory.SelectedItem as string ?? "";
        _currentExpensePage = 1;
        LoadExpenses();
    }

    private void BtnClearExpenseFilter_Click(object? sender, EventArgs e)
    {
        cmbExpenseFilterCategory.SelectedIndex = 0;
        _currentExpensePage = 1;
        LoadExpenses();
    }

    private void BtnExpensePrev_Click(object? sender, EventArgs e)
    {
        _currentExpensePage--;
        LoadExpenses();
    }

    private void BtnExpenseNext_Click(object? sender, EventArgs e)
    {
        _currentExpensePage++;
        LoadExpenses();
    }

    // ── Invoices Tab ─────────────────────────────────────────────────────────

    private bool _loadingInvoices = false;

    private void LoadInvoicesTab()
    {
        using var db = new AppDbContext();
        var buyers = db.Invoices
            .Select(i => i.BuyerName)
            .Distinct()
            .OrderBy(b => b)
            .ToList();
        buyers.Insert(0, "");
        cmbInvoiceFilterBuyer.SelectedIndexChanged -= InvoiceFilter_Changed;
        cmbInvoiceFilterBuyer.DataSource = buyers;
        cmbInvoiceFilterBuyer.SelectedIndex = 0;
        cmbInvoiceFilterBuyer.SelectedIndexChanged += InvoiceFilter_Changed;

        cmbInvoiceFilterStatus.SelectedIndexChanged -= InvoiceFilter_Changed;
        cmbInvoiceFilterStatus.Items.Clear();
        cmbInvoiceFilterStatus.Items.AddRange(new object[] { "", "No", "Yes", "PP" });
        cmbInvoiceFilterStatus.SelectedIndex = 0;
        cmbInvoiceFilterStatus.SelectedIndexChanged += InvoiceFilter_Changed;

        _currentInvoicePage = 1;
        LoadInvoices();
    }

    private void LoadInvoices()
    {
        using var db = new AppDbContext();
        var buyerFilter = cmbInvoiceFilterBuyer?.SelectedItem as string ?? "";
        var statusFilter = cmbInvoiceFilterStatus?.SelectedItem as string ?? "";

        var query = db.Invoices.AsQueryable();

        if (!string.IsNullOrEmpty(buyerFilter))
            query = query.Where(i => i.BuyerName == buyerFilter);

        if (!string.IsNullOrEmpty(statusFilter))
        {
            var status = statusFilter switch
            {
                "Yes" => InvoicePaymentStatus.Yes,
                "PP" => InvoicePaymentStatus.PartiallyPaid,
                _ => InvoicePaymentStatus.No
            };
            query = query.Where(i => i.PaymentStatus == status);
        }

        var totalCount = query.Count();
        var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)InvoicePageSize));
        _currentInvoicePage = Math.Clamp(_currentInvoicePage, 1, totalPages);

        var invoices = query
            .OrderByDescending(i => i.Id)
            .Skip((_currentInvoicePage - 1) * InvoicePageSize)
            .Take(InvoicePageSize)
            .Select(i => new InvoiceGridRow
            {
                Id = i.Id,
                Buyer = i.BuyerName,
                Date = i.Date.Date,
                Hour = i.Date.ToString("HH:mm"),
                Total = i.TotalAmount,
                Paid = i.AmountPaid,
                DueAmount = i.TotalAmount - i.AmountPaid,
                Status = i.PaymentStatus == InvoicePaymentStatus.Yes ? "Yes" :
                         i.PaymentStatus == InvoicePaymentStatus.PartiallyPaid ? "PP" : "No"
            })
            .ToList();

            if (dgvInvoices.IsCurrentCellDirty || dgvInvoices.CurrentCell?.IsInEditMode == true)
                dgvInvoices.EndEdit();

            _loadingInvoices = true;
            try
            {
                dgvInvoices.DataSource = invoices;

                if (dgvInvoices.Columns.Contains("Date"))
                    dgvInvoices.Columns["Date"].DefaultCellStyle.Format = "yyyy-MM-dd";
                if (dgvInvoices.Columns.Contains("Hour"))
                    dgvInvoices.Columns["Hour"].DefaultCellStyle.Format = "HH:mm";
                if (dgvInvoices.Columns.Contains("Id"))
                    dgvInvoices.Columns["Id"].ReadOnly = true;
                if (dgvInvoices.Columns.Contains("Buyer"))
                    dgvInvoices.Columns["Buyer"].ReadOnly = true;
                if (dgvInvoices.Columns.Contains("Date"))
                    dgvInvoices.Columns["Date"].ReadOnly = true;
                if (dgvInvoices.Columns.Contains("Hour"))
                    dgvInvoices.Columns["Hour"].ReadOnly = true;
                if (dgvInvoices.Columns.Contains("Total"))
                    dgvInvoices.Columns["Total"].ReadOnly = true;
                if (dgvInvoices.Columns.Contains("DueAmount"))
                    dgvInvoices.Columns["DueAmount"].ReadOnly = true;

                // Replace Status column with a combo box column
                if (dgvInvoices.Columns.Contains("Status") && dgvInvoices.Columns["Status"] is not DataGridViewComboBoxColumn)
                {
                    var idx = dgvInvoices.Columns["Status"].Index;
                    dgvInvoices.Columns.Remove("Status");
                    var statusCol = new DataGridViewComboBoxColumn
                    {
                        Name = "Status",
                        HeaderText = "Status",
                        DataPropertyName = "Status",
                        Items = { "No", "Yes", "PP" }
                    };
                    dgvInvoices.Columns.Insert(idx, statusCol);
                }

                if (dgvInvoices.Columns.Contains("Paid"))
                {
                    dgvInvoices.Columns["Paid"].ReadOnly = false;
                    dgvInvoices.Columns["Paid"].DefaultCellStyle.NullValue = 0m;
                }
            }
            finally
            {
                _loadingInvoices = false;
            }

        lblInvoicePage.Text = $"Page {_currentInvoicePage} / {totalPages}";
        btnInvoicePrev.Enabled = _currentInvoicePage > 1;
        btnInvoiceNext.Enabled = _currentInvoicePage < totalPages;
    }

    private void ReloadInvoices()
    {
        if (IsDisposed)
            return;

        if (IsHandleCreated)
            BeginInvoke(new Action(LoadInvoices));
        else
            LoadInvoices();
    }

    private void InvoiceFilter_Changed(object? sender, EventArgs e)
    {
        _currentInvoicePage = 1;
        LoadInvoices();
    }

    private void BtnClearInvoiceFilter_Click(object? sender, EventArgs e)
    {
        cmbInvoiceFilterBuyer.SelectedIndex = 0;
        cmbInvoiceFilterStatus.SelectedIndex = 0;
        _currentInvoicePage = 1;
        LoadInvoices();
    }

    private void BtnInvoicePrev_Click(object? sender, EventArgs e)
    {
        _currentInvoicePage--;
        LoadInvoices();
    }

    private void BtnInvoiceNext_Click(object? sender, EventArgs e)
    {
        _currentInvoicePage++;
        LoadInvoices();
    }

    private void BtnInvoicePreview_Click(object? sender, EventArgs e)
    {
        if (dgvInvoices.CurrentRow == null)
        {
            MessageBox.Show("Select an invoice to preview.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var idCell = dgvInvoices.CurrentRow.Cells["Id"];
        if (idCell?.Value == null || !int.TryParse(idCell.Value.ToString(), out var invoiceId))
        {
            MessageBox.Show("Select a valid invoice to preview.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var db = new AppDbContext();
        var invoice = db.Invoices.FirstOrDefault(i => i.Id == invoiceId);
        if (invoice == null)
        {
            MessageBox.Show("The selected invoice could not be found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var sales = db.Sales
            .Include(s => s.Product)
            .Where(s => s.InvoiceId == invoice.Id)
            .ToList();

        if (sales.Count == 0)
        {
            MessageBox.Show("No sales were found for the selected invoice.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var store = db.StoreSettings.FirstOrDefault();
        new KabyliaTaste.Services.InvoicePrinter(
            sales,
            invoice.BuyerName,
            store?.StoreName ?? "KabyliaTaste",
            store?.LogoData,
            invoice.Id,
            invoice.Date,
            invoice.TotalAmount,
            invoice.AmountPaid,
            invoice.PaymentStatus).PrintPreview();
    }

    private void DgvInvoices_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
    {
        if (!dgvInvoices.IsCurrentCellDirty || dgvInvoices.CurrentCell == null)
            return;

        if (dgvInvoices.CurrentCell is DataGridViewCheckBoxCell ||
            dgvInvoices.CurrentCell is DataGridViewComboBoxCell)
        {
            dgvInvoices.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
    }

    private void DgvInvoices_CellParsing(object? sender, DataGridViewCellParsingEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
        if (dgvInvoices.Columns[e.ColumnIndex].Name != "Paid") return;

        if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
        {
            e.Value = 0m;
            e.ParsingApplied = true;
        }
    }

    private void DgvInvoices_CellValidating(object? sender, DataGridViewCellValidatingEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
        if (dgvInvoices.Columns[e.ColumnIndex].Name != "Paid") return;

        var text = e.FormattedValue?.ToString()?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(text))
            return;

        if (!decimal.TryParse(text, out var paid))
        {
            MessageBox.Show("Amount paid must be a valid number.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            e.Cancel = true;
            return;
        }

        if (dgvInvoices.Rows[e.RowIndex].Cells["Total"].Value == null ||
            !decimal.TryParse(dgvInvoices.Rows[e.RowIndex].Cells["Total"].Value.ToString(), out var total))
            return;

        if (paid < 0 || paid > total)
        {
            MessageBox.Show("Amount paid must not exceed the invoice total.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            e.Cancel = true;
        }
    }

    private void DgvInvoices_DataError(object? sender, DataGridViewDataErrorEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
        if (dgvInvoices.Columns[e.ColumnIndex].Name != "Paid") return;

        e.ThrowException = false;
        ReloadInvoices();
    }

    private void DgvInvoices_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (_loadingInvoices || e.RowIndex < 0) return;

        var row = dgvInvoices.Rows[e.RowIndex];
        if (row.Cells["Id"]?.Value == null ||
            !int.TryParse(row.Cells["Id"].Value.ToString(), out var invoiceId))
            return;

        var columnName = dgvInvoices.Columns[e.ColumnIndex].Name;
        if (columnName != "Status" && columnName != "Paid") return;

        using var db = new AppDbContext();
        var invoice = db.Invoices.Find(invoiceId);
        if (invoice == null) return;

        if (columnName == "Status")
        {
            var statusText = row.Cells["Status"].Value?.ToString() ?? "No";
            invoice.PaymentStatus = statusText switch
            {
                "Yes" => InvoicePaymentStatus.Yes,
                "PP" => InvoicePaymentStatus.PartiallyPaid,
                _ => InvoicePaymentStatus.No
            };

            if (invoice.PaymentStatus == InvoicePaymentStatus.Yes)
                invoice.AmountPaid = invoice.TotalAmount;
            else if (invoice.PaymentStatus == InvoicePaymentStatus.No)
                invoice.AmountPaid = 0;
        }
        else if (columnName == "Paid")
        {
            var paidText = row.Cells["Paid"].Value?.ToString();
            var paid = 0m;

            if (!string.IsNullOrWhiteSpace(paidText) && !decimal.TryParse(paidText, out paid))
            {
                MessageBox.Show("Amount paid must be a valid number.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ReloadInvoices();
                return;
            }

            if (paid < 0 || paid > invoice.TotalAmount)
            {
                MessageBox.Show("Amount paid must not exceed the invoice total.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ReloadInvoices();
                return;
            }

            invoice.AmountPaid = paid;
            invoice.PaymentStatus = paid >= invoice.TotalAmount
                ? InvoicePaymentStatus.Yes
                : paid <= 0
                    ? InvoicePaymentStatus.No
                    : InvoicePaymentStatus.PartiallyPaid;
        }

        db.SaveChanges();
        ReloadInvoices();
    }

    // ── Settings Tab ─────────────────────────────────────────────────────────

    private void LoadSettingsTab()
    {
        using var db = new Data.AppDbContext();
        var store = db.StoreSettings.FirstOrDefault();

        if (store != null)
        {
            txtStoreName.Text = store.StoreName;

            if (store.LogoData != null && store.LogoData.Length > 0)
            {
                using var ms = new System.IO.MemoryStream(store.LogoData);
                picStoreLogo.Image = System.Drawing.Image.FromStream(ms);
            }
            else
            {
                picStoreLogo.Image = null;
            }

            txtGoogleDriveClientId.Text = store.GoogleDriveClientId ?? string.Empty;
            txtGoogleDriveClientSecret.Text = store.GoogleDriveClientSecret ?? string.Empty;
            txtGoogleDriveFolderId.Text = store.GoogleDriveFolderId ?? string.Empty;
            txtGoogleDriveRefreshToken.Text = store.GoogleDriveRefreshToken ?? string.Empty;
        }
        else
        {
            txtStoreName.Clear();
            picStoreLogo.Image = null;
            txtGoogleDriveClientId.Clear();
            txtGoogleDriveClientSecret.Clear();
            txtGoogleDriveFolderId.Clear();
            txtGoogleDriveRefreshToken.Clear();
        }

        // Clear password fields
        txtCurrentPassword.Clear();
        txtNewPassword.Clear();
        txtConfirmPassword.Clear();

        // Populate username field
        txtUsernameProfile.Text = Session.CurrentUser?.Username ?? "";
    }

    private void BtnChangeUsername_Click(object? sender, EventArgs e)
    {
        if (Session.CurrentUser == null) return;

        var newUsername = txtUsernameProfile.Text.Trim();
        if (string.IsNullOrEmpty(newUsername))
        {
            MessageBox.Show("Username cannot be empty.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var db = new Data.AppDbContext();
        if (db.Users.Any(u => u.Username == newUsername && u.Id != Session.CurrentUser.Id))
        {
            MessageBox.Show("That username is already taken.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var user = db.Users.Find(Session.CurrentUser.Id);
        if (user == null) return;

        user.Username = newUsername;
        db.SaveChanges();
        Session.CurrentUser.Username = newUsername;

        UpdateGreeting();
        MessageBox.Show("Username updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void BtnLogout_Click(object? sender, EventArgs e)
    {
        LogoutRequested = true;
        Close();
    }

    private void BtnChangePassword_Click(object? sender, EventArgs e)
    {
        if (Session.CurrentUser == null) return;

        var current = txtCurrentPassword.Text;
        var newPass = txtNewPassword.Text;
        var confirm = txtConfirmPassword.Text;

        if (string.IsNullOrWhiteSpace(current) || string.IsNullOrWhiteSpace(newPass))
        {
            MessageBox.Show("Please fill in all password fields.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (newPass != confirm)
        {
            MessageBox.Show("New password and confirmation do not match.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var db = new Data.AppDbContext();
        var user = db.Users.Find(Session.CurrentUser.Id);
        if (user == null) return;

        if (user.Password != current)
        {
            MessageBox.Show("Current password is incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        user.Password = newPass;
        db.SaveChanges();
        Session.CurrentUser.Password = newPass;

        txtCurrentPassword.Clear();
        txtNewPassword.Clear();
        txtConfirmPassword.Clear();
        MessageBox.Show("Password changed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void BtnSaveStoreName_Click(object? sender, EventArgs e)
    {
        var name = txtStoreName.Text.Trim();
        if (string.IsNullOrEmpty(name))
        {
            MessageBox.Show("Store name cannot be empty.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var db = new Data.AppDbContext();
        var store = db.StoreSettings.FirstOrDefault();
        if (store == null)
        {
            store = new Models.StoreSettings { StoreName = name };
            db.StoreSettings.Add(store);
        }
        else
        {
            store.StoreName = name;
        }
        db.SaveChanges();
        this.Text = name;
        MessageBox.Show("Store name saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void BtnChangeLogo_Click(object? sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Title = "Select Logo Image",
            Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.gif"
        };
        if (dlg.ShowDialog() != DialogResult.OK) return;

        var imageData = System.IO.File.ReadAllBytes(dlg.FileName);

        using var db = new Data.AppDbContext();
        var store = db.StoreSettings.FirstOrDefault();
        if (store == null)
        {
            store = new Models.StoreSettings { StoreName = txtStoreName.Text.Trim(), LogoData = imageData };
            db.StoreSettings.Add(store);
        }
        else
        {
            store.LogoData = imageData;
        }
        db.SaveChanges();

        using var ms = new System.IO.MemoryStream(imageData);
        picStoreLogo.Image = System.Drawing.Image.FromStream(ms);
        MessageBox.Show("Logo updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

        private void BtnSaveGoogleDriveConfig_Click(object? sender, EventArgs e)
        {
            using var db = new Data.AppDbContext();
            var store = db.StoreSettings.FirstOrDefault();
            if (store == null)
            {
                store = new Models.StoreSettings();
                db.StoreSettings.Add(store);
            }

            store.GoogleDriveClientId = txtGoogleDriveClientId.Text.Trim();
            store.GoogleDriveClientSecret = txtGoogleDriveClientSecret.Text.Trim();
            store.GoogleDriveFolderId = txtGoogleDriveFolderId.Text.Trim();
            store.GoogleDriveRefreshToken = txtGoogleDriveRefreshToken.Text.Trim();

            db.SaveChanges();
            MessageBox.Show("Google Drive settings saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void BtnGenerateGoogleDriveRefreshToken_Click(object? sender, EventArgs e)
        {
            try
            {
                var store = new StoreSettings
                {
                    GoogleDriveClientId = txtGoogleDriveClientId.Text.Trim(),
                    GoogleDriveClientSecret = txtGoogleDriveClientSecret.Text.Trim()
                };

                var service = new GoogleDriveBackupService();
                var refreshToken = await service.GenerateRefreshTokenAsync(store);

                txtGoogleDriveRefreshToken.Text = refreshToken;

                using var db = new Data.AppDbContext();
                var settings = db.StoreSettings.FirstOrDefault();
                if (settings == null)
                {
                    settings = new StoreSettings
                    {
                        StoreName = txtStoreName.Text.Trim()
                    };
                    db.StoreSettings.Add(settings);
                }

                settings.GoogleDriveClientId = store.GoogleDriveClientId;
                settings.GoogleDriveClientSecret = store.GoogleDriveClientSecret;
                settings.GoogleDriveRefreshToken = refreshToken;
                db.SaveChanges();

                MessageBox.Show("Refresh token generated and saved.", "Google Drive", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to generate refresh token: {ex.Message}", "Google Drive", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGoogleDriveHelp_Click(object? sender, EventArgs e)
        {
            MessageBox.Show(
                "Google Drive setup:\n\n" +
                "1. Open Google Cloud Console and create a project.\n" +
                "2. Enable the Google Drive API.\n" +
                "3. Create OAuth Client ID for a Desktop app.\n" +
                "4. Copy Client ID and Client Secret into this tab.\n" +
                "5. Click 'Generate Refresh Token' and sign in.\n" +
                "6. Paste the Google Drive folder ID or folder name if you want backups in a specific folder.\n" +
                "7. Save the refresh token, then use Upload / Download from Google Drive.",
                "Google Drive Help",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void BtnOpenGoogleCloudConsole_Click(object? sender, EventArgs e)
        {
            try
            {
                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "https://console.cloud.google.com/",
                    UseShellExecute = true
                };

                System.Diagnostics.Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open Google Cloud Console: {ex.Message}", "Google Drive", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDownloadDatabaseBackup_Click(object? sender, EventArgs e)
        {
            var databasePath = GetDatabaseFilePath();
            if (!File.Exists(databasePath))
            {
                MessageBox.Show("The database file was not found.", "Backup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var dlg = new SaveFileDialog
            {
                Title = "Save Database Backup",
                Filter = "SQLite Database|*.db",
                FileName = $"KabyliaTaste-{DateTime.Now:yyyyMMdd-HHmmss}.db"
            };

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                File.Copy(databasePath, dlg.FileName, true);
                CopyRelatedSqliteFiles(databasePath, dlg.FileName);
                MessageBox.Show("Database backup created successfully.", "Backup", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create backup: {ex.Message}", "Backup", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRestoreDatabaseBackup_Click(object? sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog
            {
                Title = "Select Database Backup",
                Filter = "SQLite Database|*.db"
            };

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            var confirm = MessageBox.Show(
                "This will replace the current local database file. Continue?",
                "Restore Backup",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
                return;

            var databasePath = GetDatabaseFilePath();

            try
            {
                if (File.Exists(databasePath))
                {
                    var safetyCopy = $"{databasePath}.before-restore-{DateTime.Now:yyyyMMdd-HHmmss}";
                    File.Copy(databasePath, safetyCopy, true);
                }

                File.Copy(dlg.FileName, databasePath, true);
                CopyRelatedSqliteFiles(dlg.FileName, databasePath);

                MessageBox.Show("Database restored. The application will restart.", "Backup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Restart();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to restore backup: {ex.Message}", "Backup", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnUploadGoogleDriveBackup_Click(object? sender, EventArgs e)
        {
            try
            {
                await Task.Run(() =>
                {
                    var store = GetGoogleDriveConfiguredStore();
                    var service = new GoogleDriveBackupService();
                    service.UploadDatabaseBackup(store, GetDatabaseFilePath());
                });

                MessageBox.Show("Database uploaded to Google Drive successfully.", "Google Drive", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to upload database to Google Drive: {ex.Message}", "Google Drive", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnDownloadGoogleDriveBackup_Click(object? sender, EventArgs e)
        {
            try
            {
                using var progressForm = new BackupProgressForm();
                progressForm.Show(this);
                progressForm.SetProgress(0, "Downloading database backup from Google Drive...");

                var progress = new Progress<int>(percent =>
                {
                    progressForm.SetProgress(percent, $"Downloading database backup from Google Drive... {percent}%");
                });

                await Task.Run(() =>
                {
                    var store = GetGoogleDriveConfiguredStore();
                    var service = new GoogleDriveBackupService();
                    service.DownloadDatabaseBackup(store, GetDatabaseFilePath(), progress);
                });

                progressForm.SetProgress(100, "Database downloaded successfully.");
                MessageBox.Show("Database downloaded from Google Drive successfully. The application will restart.", "Google Drive", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Restart();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to download database from Google Drive: {ex.Message}", "Google Drive", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private StoreSettings GetGoogleDriveConfiguredStore()
        {
            using var db = new Data.AppDbContext();
            var store = db.StoreSettings.FirstOrDefault();
            if (store == null)
                throw new InvalidOperationException("Google Drive settings are not configured.");

            return store;
        }

        private static string GetDatabaseFilePath() => Path.GetFullPath("app.db");

        private static void CopyRelatedSqliteFiles(string sourceDatabasePath, string targetDatabasePath)
        {
            CopyOrDelete($"{sourceDatabasePath}-wal", $"{targetDatabasePath}-wal");
            CopyOrDelete($"{sourceDatabasePath}-shm", $"{targetDatabasePath}-shm");
        }

        private static void CopyOrDelete(string sourcePath, string targetPath)
        {
            if (File.Exists(targetPath))
            {
                File.Delete(targetPath);
            }

            if (File.Exists(sourcePath))
            {
                File.Copy(sourcePath, targetPath, true);
            }
        }
    }
}
