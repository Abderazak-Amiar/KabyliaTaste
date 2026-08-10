namespace KabyliaTaste
{
    using System;
    using System.Globalization;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.Json;
    using System.Windows.Forms;
    using KabyliaTaste.Data;
    using KabyliaTaste.Models;
    using KabyliaTaste.Services;
    using Microsoft.EntityFrameworkCore;

    public partial class Main : Form
    {
        private static string GetAppDataFolder()
        {
            var folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "KabyliaTaste");

            Directory.CreateDirectory(folder);
            return folder;
        }

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

        private sealed class LanguageOption
        {
            public string Code { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public override string ToString() => Name;
        }

        private sealed class CurrencyOption
        {
            public string Code { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public override string ToString() => $"{Code} - {Name}";
        }

        private sealed class ProductUnitRow
        {
            public string Name { get; set; } = string.Empty;
        }

        private sealed class StatsGridRow
        {
            public string Product { get; set; } = string.Empty;
            public DateTime Date { get; set; }
            public string Hour { get; set; } = string.Empty;
            public decimal UnitsSold { get; set; }
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
        private bool _loadingSettingsUi = false;
        private bool _clearingProductForm = false;
        private List<string> _productUnits = new();
        private TabPage? tabLanguages;
        private ComboBox? cmbLanguage;
        private ComboBox? cmbCurrency;
        private Label? lblLanguage;
        private Label? lblCurrency;
        private Label? lblUnits;
        private Button? btnSaveLanguagePreference;
        private DataGridView? dgvProductUnits;
        private TextBox? txtProductUnitName;
        private Button? btnAddUnit;
        private Button? btnUpdateUnit;
        private Button? btnDeleteUnit;
        private Button? btnClearUnit;

        public bool LogoutRequested { get; private set; } = false;

        public Main()
        {
            InitializeComponent();
            ConfigureQuantityInputs();

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
            dgvSales.CellFormatting += DgvSales_CellFormatting;
            dgvStats.CellFormatting += DgvStats_CellFormatting;
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
            btnDeleteInvoice.Click += BtnDeleteInvoice_Click;
            dgvInvoices.CellValueChanged += DgvInvoices_CellValueChanged;
            dgvInvoices.CurrentCellDirtyStateChanged += DgvInvoices_CurrentCellDirtyStateChanged;
            dgvInvoices.CellParsing += DgvInvoices_CellParsing;
            dgvInvoices.CellValidating += DgvInvoices_CellValidating;
            dgvInvoices.DataError += DgvInvoices_DataError;

            InitializeSettingsUi();
            InitializeHelpUi();
        }

        private void ConfigureQuantityInputs()
        {
            ConfigureQuantityInput(numQuantity);
            ConfigureQuantityInput(numSaleQuantity);
        }

        private static void ConfigureQuantityInput(KabyliaTaste.Controls.QuantityNumericUpDown input)
        {
            input.DecimalPlaces = 1;
            input.Increment = 0.1M;
            input.Minimum = 0;
            input.Maximum = 1000000;
            input.ThousandsSeparator = false;
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

                using var progressToast = new BackupToastForm();
                progressToast.ShowToast(AppLocalization.T("Preparing database backup..."));

                var progress = new Progress<int>(percent =>
                {
                    progressToast.SetProgress(percent, $"{AppLocalization.T("Uploading database backup...")} {percent}%");
                });

                await Task.Run(() =>
                {
                    var service = new GoogleDriveBackupService();
                    service.UploadDatabaseBackup(store, GetDatabaseFilePath(), progress);
                });

                progressToast.SetProgress(100, AppLocalization.T("Upload complete."));
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
                    $"{AppLocalization.T("Failed to upload database to Google Drive before closing:")} {ex.Message}",
                    AppLocalization.T("Google Drive"),
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
            ApplyRoleRestrictions();
            UpdateGreeting();
            UpdateDateTime();

            LoadStorePreferences();
            ApplyLocalizedSettingsTexts();

            using var db = new AppDbContext();
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
            var timeOfDay = AppLocalization.GetGreeting(DateTime.Now);
            var role      = AppLocalization.GetRoleLabel(Session.CurrentUser?.IsAdmin == true);
            var username = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Session.CurrentUser?.Username ?? "");

            lblGreeting.SetSegments(new[]
            {
                new KabyliaTaste.Controls.MixedFontLabel.Segment($"{timeOfDay}, ",          Bold: false),
                new KabyliaTaste.Controls.MixedFontLabel.Segment(username,                  Bold: true),
                new KabyliaTaste.Controls.MixedFontLabel.Segment($"  |  {AppLocalization.T("You're logged in as:")}", Bold: false),
                new KabyliaTaste.Controls.MixedFontLabel.Segment($" {role}",                  Bold: true),
            });

            UpdateDateTime();
        }

        private void UpdateDateTime()
        {
            lblDateTime.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(
                DateTime.Now.ToString("dddd, dd MMMM yyyy   HH:mm:ss"));
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
            RefreshLocalizedGridHeaders();

            lblProductPage.Text = $"{AppLocalization.T("Page")} {_currentProductPage} / {totalPages}";
            btnProductPrev.Enabled = _currentProductPage > 1;
            btnProductNext.Enabled = _currentProductPage < totalPages;

        if (dgvProducts.Columns.Contains("BuyPrice"))
            dgvProducts.Columns["BuyPrice"].Visible = Session.IsAdmin && chkShowBuyPrice.Checked;
        if (dgvProducts.Columns.Contains("Unit"))
            dgvProducts.Columns["Unit"].Visible = false;
        if (dgvProducts.Columns.Contains("UnitName"))
        {
            dgvProducts.Columns["UnitName"].HeaderText = "Unit";
            dgvProducts.Columns["UnitName"].ReadOnly = true;
        }
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
            SelectFirstVisibleCell(dgvProducts);
            dgvProducts.Rows[0].Selected = true;
            DgvProducts_SelectionChanged(null, EventArgs.Empty);
        }
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            var name = txtName.Text?.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show(AppLocalization.T("Name is required."), AppLocalization.T("Validation"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var db = new AppDbContext();
            if (db.Products.Any(p => p.Name.ToLower() == name.ToLower()))
            {
                MessageBox.Show(AppLocalization.T("A product with this name already exists."), AppLocalization.T("Duplicate"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var product = new Product
            {
                Id = (db.Products.Any() ? db.Products.Max(p => p.Id) : 0) + 1,
                Name = name,
                BuyPrice = numBuyPrice.Value,
                SellPrice = numSellPrice.Value,
                Quantity = numQuantity.Value,
                Unit = ProductUnit.Piece,
                UnitName = GetSelectedProductUnit()
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
                MessageBox.Show(AppLocalization.T("Select a product to update."), AppLocalization.T("Information"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var db = new AppDbContext();
            var product = db.Products.Find(selectedProductId.Value);
            if (product == null) return;
            var updatedName = txtName.Text?.Trim() ?? string.Empty;
            if (db.Products.Any(p => p.Name.ToLower() == updatedName.ToLower() && p.Id != selectedProductId.Value))
            {
                MessageBox.Show(AppLocalization.T("A product with this name already exists."), AppLocalization.T("Duplicate"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            product.Name = updatedName;
            product.BuyPrice = numBuyPrice.Value;
            product.SellPrice = numSellPrice.Value;
            product.Quantity = numQuantity.Value;
            product.UnitName = GetSelectedProductUnit();
            product.Unit = ProductUnit.Piece;
            product.Date = DateTime.Now;
            db.SaveChanges();
            _currentProductPage = 1;
            LoadProducts();
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (!selectedProductId.HasValue)
            {
                MessageBox.Show(AppLocalization.T("Select a product to delete."), AppLocalization.T("Information"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var ok = ShowEnglishYesNoConfirmation(AppLocalization.T("Are you sure you want to delete the selected product?"), AppLocalization.T("Confirm"));
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
            selectedProductId = null;
            _clearingProductForm = true;
            try
            {
                if (clearSelection)
                {
                    dgvProducts.ClearSelection();
                    dgvProducts.CurrentCell = null;
                }

                txtName.Text = string.Empty;
                numBuyPrice.Value = 0;
                numSellPrice.Value = 0;
                numQuantity.Value = 0;
                SelectDefaultProductUnit();
            }
            finally
            {
                _clearingProductForm = false;
            }
        }

        private string GetSelectedProductUnit()
        {
            var selected = cmbUnit.SelectedItem?.ToString()?.Trim();
            if (!string.IsNullOrWhiteSpace(selected))
                return selected;

            return _productUnits.FirstOrDefault() ?? "Piece";
        }

        private void SelectDefaultProductUnit()
        {
            if (cmbUnit.Items.Count == 0)
                return;

            var defaultUnit = _productUnits.FirstOrDefault() ?? cmbUnit.Items[0]?.ToString() ?? "Piece";
            SetSelectedProductUnit(defaultUnit);
        }

        private void SetSelectedProductUnit(string? unitName)
        {
            if (string.IsNullOrWhiteSpace(unitName) || cmbUnit.Items.Count == 0)
                return;

            var index = cmbUnit.Items.Cast<object>().ToList().FindIndex(item => string.Equals(item?.ToString(), unitName, StringComparison.OrdinalIgnoreCase));
            if (index >= 0)
                cmbUnit.SelectedIndex = index;
            else
                cmbUnit.SelectedIndex = 0;
        }

        private static string GetProductUnitName(Product product)
        {
            if (!string.IsNullOrWhiteSpace(product.UnitName))
                return product.UnitName.Trim();

            return product.Unit.ToString();
        }

        private void DgvProducts_SelectionChanged(object? sender, EventArgs e)
        {
            if (_clearingProductForm)
                return;

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
            SetSelectedProductUnit(GetProductUnitName(product));
        }

    private void DgvProducts_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (dgvProducts?.Columns == null || dgvProducts.Columns.Count == 0 || e.RowIndex < 0) return;
        if (!dgvProducts.Columns.Contains("Quantity")) return;

        var quantityColumn = dgvProducts.Columns["Quantity"];
        if (quantityColumn == null || quantityColumn.Index != e.ColumnIndex) return;

        if (e.Value is decimal qty && e.CellStyle != null)
        {
            e.CellStyle.BackColor = qty < 5
                ? Color.LightCoral
                : qty <= 10
                    ? Color.Orange
                    : Color.LightGreen;

            if (qty == 0m)
            {
                e.Value = "0  ⚠ Out of stock";
                e.FormattingApplied = true;
            }
            else
            {
                e.Value = CurrencyFormatting.FormatQuantity(qty);
                e.FormattingApplied = true;
            }
        }
    }

    private void DgvSales_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (dgvSales?.Columns == null || dgvSales.Columns.Count == 0 || e.RowIndex < 0) return;
        if (!dgvSales.Columns.Contains("Quantity")) return;

        var quantityColumn = dgvSales.Columns["Quantity"];
        if (quantityColumn == null || quantityColumn.Index != e.ColumnIndex) return;

        if (e.Value is decimal qty && e.CellStyle != null)
        {
            e.Value = CurrencyFormatting.FormatQuantity(qty);
            e.FormattingApplied = true;
        }
    }

    private void DgvStats_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (dgvStats?.Columns == null || dgvStats.Columns.Count == 0 || e.RowIndex < 0) return;
        if (!dgvStats.Columns.Contains("UnitsSold")) return;

        var unitsColumn = dgvStats.Columns["UnitsSold"];
        if (unitsColumn == null || unitsColumn.Index != e.ColumnIndex) return;

        if (e.Value is decimal qty && e.CellStyle != null)
        {
            e.Value = CurrencyFormatting.FormatQuantity(qty);
            e.FormattingApplied = true;
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

        if (decimal.TryParse(qtyVal?.ToString(), out var qty)) numQuantity.Value = qty;
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

        cmbStatsPeriod.SelectedIndexChanged -= StatsFilter_Changed;
        cmbStatsPeriod.Items.Clear();
        cmbStatsPeriod.Items.AddRange(new object[]
        {
            "",
            AppLocalization.T("Day"),
            AppLocalization.T("Week"),
            AppLocalization.T("Month"),
            AppLocalization.T("Year")
        });
        cmbStatsPeriod.SelectedIndex = 0;
        cmbStatsPeriod.SelectedIndexChanged += StatsFilter_Changed;

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
        RefreshLocalizedGridHeaders();

        if (dgvSales.Columns.Contains("Date"))
            dgvSales.Columns["Date"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";

        RefreshInvoiceCheckboxColumn();

        if (dgvSales.Rows.Count > 0)
        {
            dgvSales.ClearSelection();
            SelectFirstVisibleCell(dgvSales);
            dgvSales.Rows[0].Selected = true;
            DgvSales_SelectionChanged(null, EventArgs.Empty);
        }
        else
        {
            selectedSaleId = null;
            ClearSaleForm();
        }

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

    private static void SelectFirstVisibleCell(DataGridView grid)
    {
        if (grid.Rows.Count == 0)
            return;

        var firstVisibleCell = grid.Rows[0].Cells.Cast<DataGridViewCell>().FirstOrDefault(c => c.Visible);
        if (firstVisibleCell != null)
            grid.CurrentCell = firstVisibleCell;
    }

    private static DialogResult ShowEnglishYesNoConfirmation(string message, string title)
    {
        const int contentWidth = 340;
        const int margin = 12;
        const int iconSize = 32;
        const int buttonWidth = 80;
        const int buttonHeight = 28;
        const int buttonGap = 8;

        var textSize = TextRenderer.MeasureText(
            message,
            SystemFonts.MessageBoxFont,
            new Size(contentWidth, 0),
            TextFormatFlags.WordBreak | TextFormatFlags.NoPadding);

        using var form = new Form
        {
            Text = title,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            StartPosition = FormStartPosition.CenterParent,
            MinimizeBox = false,
            MaximizeBox = false,
            ShowInTaskbar = false,
            ClientSize = new Size(margin * 2 + iconSize + 10 + contentWidth, margin * 2 + Math.Max(iconSize, textSize.Height) + 20 + buttonHeight),
            TopMost = true,
            Font = SystemFonts.MessageBoxFont
        };

        var iconBox = new PictureBox
        {
            Image = SystemIcons.Question.ToBitmap(),
            SizeMode = PictureBoxSizeMode.AutoSize,
            Location = new Point(margin, margin)
        };

        var label = new Label
        {
            AutoSize = false,
            Location = new Point(margin + iconSize + 10, margin),
            Size = new Size(contentWidth, Math.Max(iconSize, textSize.Height)),
            Text = message
        };

        var btnNo = new Button
        {
            Text = AppLocalization.T("No"),
            DialogResult = DialogResult.No,
            Size = new Size(buttonWidth, buttonHeight)
        };

        var btnYes = new Button
        {
            Text = AppLocalization.T("Yes"),
            DialogResult = DialogResult.Yes,
            Size = new Size(buttonWidth, buttonHeight)
        };

        var buttonTop = form.ClientSize.Height - margin - buttonHeight;
        btnNo.Location = new Point(form.ClientSize.Width - margin - buttonWidth, buttonTop);
        btnYes.Location = new Point(btnNo.Left - buttonGap - buttonWidth, buttonTop);

        form.Controls.Add(iconBox);
        form.Controls.Add(label);
        form.Controls.Add(btnYes);
        form.Controls.Add(btnNo);
        form.AcceptButton = btnYes;
        form.CancelButton = btnNo;

        return form.ShowDialog();
    }

    private void BtnPrintInvoice_Click(object? sender, EventArgs e)
    {
        var buyerName = txtBuyerName.Text.Trim();
        if (string.IsNullOrWhiteSpace(buyerName))
        {
            MessageBox.Show(AppLocalization.T("Enter a buyer name to enable invoice printing."), AppLocalization.T("Validation"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            MessageBox.Show(AppLocalization.T("Select at least one sale to include in the invoice."), AppLocalization.T("Validation"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            invoice.PaymentStatus,
            storeForInvoice?.CurrencyCode).PrintPreview();

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
            MessageBox.Show(AppLocalization.T("Select a product."), AppLocalization.T("Validation"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var qty = numSaleQuantity.Value;
        var unitPrice = numSaleUnitPrice.Value;

        using var db = new AppDbContext();
        var product = db.Products.Find(productId);
        if (product == null) return;

        if (qty > product.Quantity)
        {
            MessageBox.Show($"{AppLocalization.T("Not enough stock. Available:")} {product.Quantity}.", AppLocalization.T("Stock Error"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        MessageBox.Show(AppLocalization.T("Sale recorded successfully."), AppLocalization.T("Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show(AppLocalization.T("Select a sale to update."), AppLocalization.T("Information"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

            if (cmbSaleProduct.SelectedValue is not int productId)
            {
                MessageBox.Show(AppLocalization.T("Select a product."), AppLocalization.T("Validation"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var newQty = numSaleQuantity.Value;
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
                MessageBox.Show($"{AppLocalization.T("Not enough stock. Available:")} {newProduct.Quantity}.", AppLocalization.T("Stock Error"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show($"{AppLocalization.T("Not enough stock. Available:")} {sale.Product.Quantity}.", AppLocalization.T("Stock Error"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        MessageBox.Show(AppLocalization.T("Sale updated successfully."), AppLocalization.T("Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void BtnDeleteSale_Click(object? sender, EventArgs e)
    {
            if (!selectedSaleId.HasValue)
            {
                MessageBox.Show(AppLocalization.T("Select a sale to delete."), AppLocalization.T("Information"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

            var ok = ShowEnglishYesNoConfirmation(AppLocalization.T("Delete this sale? The stock will be restored."), AppLocalization.T("Confirm"));
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
                    var p when string.Equals(p, "Day", StringComparison.OrdinalIgnoreCase) || string.Equals(p, AppLocalization.T("Day"), StringComparison.OrdinalIgnoreCase)
                        => query.Where(s => s.SaleDate >= dayStart && s.SaleDate < dayEnd),
                    var p when string.Equals(p, "Week", StringComparison.OrdinalIgnoreCase) || string.Equals(p, AppLocalization.T("Week"), StringComparison.OrdinalIgnoreCase)
                        => query.Where(s => s.SaleDate >= weekStart && s.SaleDate < weekEnd),
                    var p when string.Equals(p, "Month", StringComparison.OrdinalIgnoreCase) || string.Equals(p, AppLocalization.T("Month"), StringComparison.OrdinalIgnoreCase)
                        => query.Where(s => s.SaleDate >= monthStart && s.SaleDate < monthEnd),
                    var p when string.Equals(p, "Year", StringComparison.OrdinalIgnoreCase) || string.Equals(p, AppLocalization.T("Year"), StringComparison.OrdinalIgnoreCase)
                        => query.Where(s => s.SaleDate >= yearStart && s.SaleDate < yearEnd),
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

            if (dgvStats.Columns.Contains("Product")) dgvStats.Columns["Product"].HeaderText = AppLocalization.T("Product");
            if (dgvStats.Columns.Contains("Date")) dgvStats.Columns["Date"].HeaderText = AppLocalization.T("Date");
            if (dgvStats.Columns.Contains("Hour")) dgvStats.Columns["Hour"].HeaderText = AppLocalization.T("Hour");
            if (dgvStats.Columns.Contains("UnitsSold")) dgvStats.Columns["UnitsSold"].HeaderText = AppLocalization.T("Units Sold");
            if (dgvStats.Columns.Contains("Revenue")) dgvStats.Columns["Revenue"].HeaderText = AppLocalization.T("Revenue");
            if (dgvStats.Columns.Contains("Cost")) dgvStats.Columns["Cost"].HeaderText = AppLocalization.T("Cost");
            if (dgvStats.Columns.Contains("Profit")) dgvStats.Columns["Profit"].HeaderText = AppLocalization.T("Profit");

            var grossProfit = stats.Sum(x => x.Profit);
            var invoiceTotals = GetInvoiceTotals(db, filteredSales);
            var totalExpenses = db.Expenses.AsEnumerable().Sum(e => e.Amount);
            var netProfit = grossProfit - totalExpenses;
            lblTotalProfitValue.Text = $"{AppLocalization.T("Net Profit")}: {netProfit:F2}  ({AppLocalization.T("Gross")}: {grossProfit:F2}  {AppLocalization.T("Debt")}: {invoiceTotals.Debt:F2}  {AppLocalization.T("Expenses")}: {totalExpenses:F2})";
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
            totalExpenses,
            storeForReport?.CurrencyCode).PrintPreview();
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
        RefreshLocalizedGridHeaders();

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
            MessageBox.Show(AppLocalization.T("Description is required."), AppLocalization.T("Validation"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            MessageBox.Show(AppLocalization.T("Select an expense to update."), AppLocalization.T("Information"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        var description = txtExpenseDescription.Text.Trim();
        if (string.IsNullOrEmpty(description))
        {
            MessageBox.Show(AppLocalization.T("Description is required."), AppLocalization.T("Validation"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            MessageBox.Show(AppLocalization.T("Select an expense to delete."), AppLocalization.T("Information"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        var ok = ShowEnglishYesNoConfirmation(AppLocalization.T("Are you sure you want to delete the selected expense?"), AppLocalization.T("Confirm"));
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
        cmbInvoiceFilterStatus.Items.AddRange(new object[] { "", AppLocalization.T("No"), AppLocalization.T("Yes"), AppLocalization.T("Partially Paid") });
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
                if (!AppLocalization.TryParseInvoiceStatus(statusFilter, out var status))
                    status = InvoicePaymentStatus.No;
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
                    Status = AppLocalization.GetInvoiceStatusText(i.PaymentStatus)
            })
            .ToList();

            if (dgvInvoices.IsCurrentCellDirty || dgvInvoices.CurrentCell?.IsInEditMode == true)
                dgvInvoices.EndEdit();

            _loadingInvoices = true;
            try
            {
                dgvInvoices.DataSource = invoices;
                RefreshLocalizedGridHeaders();

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
                        HeaderText = AppLocalization.T("Status"),
                        DataPropertyName = "Status",
                        Items = { AppLocalization.T("No"), AppLocalization.T("Yes"), AppLocalization.T("Partially Paid") }
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
            MessageBox.Show(AppLocalization.T("Select an invoice to preview."), AppLocalization.T("Information"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var idCell = dgvInvoices.CurrentRow.Cells["Id"];
        if (idCell?.Value == null || !int.TryParse(idCell.Value.ToString(), out var invoiceId))
        {
            MessageBox.Show(AppLocalization.T("Select a valid invoice to preview."), AppLocalization.T("Information"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var db = new AppDbContext();
        var invoice = db.Invoices.FirstOrDefault(i => i.Id == invoiceId);
        if (invoice == null)
        {
            MessageBox.Show(AppLocalization.T("The selected invoice could not be found."), AppLocalization.T("Information"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var sales = db.Sales
            .Include(s => s.Product)
            .Where(s => s.InvoiceId == invoice.Id)
            .ToList();

        if (sales.Count == 0)
        {
            MessageBox.Show(AppLocalization.T("No sales were found for the selected invoice."), AppLocalization.T("Information"), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            invoice.PaymentStatus,
            store?.CurrencyCode).PrintPreview();
    }

    private void BtnDeleteInvoice_Click(object? sender, EventArgs e)
    {
        if (dgvInvoices.CurrentRow == null)
        {
            MessageBox.Show(AppLocalization.T("Select an invoice to delete."), AppLocalization.T("Information"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var idCell = dgvInvoices.CurrentRow.Cells["Id"];
        if (idCell?.Value == null || !int.TryParse(idCell.Value.ToString(), out var invoiceId))
        {
            MessageBox.Show(AppLocalization.T("Select a valid invoice to delete."), AppLocalization.T("Information"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirm = ShowEnglishYesNoConfirmation(
            AppLocalization.T("Are you sure you want to delete the selected invoice?"),
            AppLocalization.T("Confirm"));

        if (confirm != DialogResult.Yes)
            return;

        using var db = new AppDbContext();
        var invoice = db.Invoices.FirstOrDefault(i => i.Id == invoiceId);
        if (invoice == null)
        {
            MessageBox.Show(AppLocalization.T("The selected invoice could not be found."), AppLocalization.T("Information"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var sales = db.Sales.Where(s => s.InvoiceId == invoice.Id).ToList();
        foreach (var sale in sales)
            sale.InvoiceId = null;

        db.Invoices.Remove(invoice);
        db.SaveChanges();

        LoadInvoicesTab();
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
            MessageBox.Show(AppLocalization.T("Amount paid must be a valid number."), AppLocalization.T("Validation"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            e.Cancel = true;
            return;
        }

        if (dgvInvoices.Rows[e.RowIndex].Cells["Total"].Value == null ||
            !decimal.TryParse(dgvInvoices.Rows[e.RowIndex].Cells["Total"].Value.ToString(), out var total))
            return;

        if (paid < 0 || paid > total)
        {
            MessageBox.Show(AppLocalization.T("Amount paid must not exceed the invoice total."), AppLocalization.T("Validation"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            if (!AppLocalization.TryParseInvoiceStatus(statusText, out var paymentStatus))
                paymentStatus = InvoicePaymentStatus.No;

            invoice.PaymentStatus = paymentStatus;

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
                MessageBox.Show(AppLocalization.T("Amount paid must be a valid number."), AppLocalization.T("Validation"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ReloadInvoices();
                return;
            }

            if (paid < 0 || paid > invoice.TotalAmount)
            {
                MessageBox.Show(AppLocalization.T("Amount paid must not exceed the invoice total."), AppLocalization.T("Validation"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            LoadStorePreferences();
            ApplyLocalizedSettingsTexts();
    }

        private void InitializeSettingsUi()
        {
            tabLanguages = new TabPage
            {
                Name = "tabLanguages",
                Text = AppLocalization.T("Languages"),
                UseVisualStyleBackColor = true,
                Padding = new Padding(20)
            };

            tabControlSettings.TabPages.Insert(2, tabLanguages);

            lblLanguage = new Label
            {
                AutoSize = true,
                Location = new Point(30, 30),
                Text = AppLocalization.T("Language")
            };

            cmbLanguage = new ComboBox
            {
                Location = new Point(30, 52),
                Width = 240,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbLanguage.SelectedIndexChanged += CmbLanguage_SelectedIndexChanged;

            btnSaveLanguagePreference = new Button
            {
                Location = new Point(285, 50),
                Size = new Size(160, 30),
                Text = AppLocalization.T("Save Preference")
            };
            btnSaveLanguagePreference.Click += BtnSaveLanguagePreference_Click;

            tabLanguages.Controls.Add(lblLanguage);
            tabLanguages.Controls.Add(cmbLanguage);
            tabLanguages.Controls.Add(btnSaveLanguagePreference);

            lblCurrency = new Label
            {
                AutoSize = true,
                Location = new Point(30, 285),
                Text = AppLocalization.T("Currency")
            };

            cmbCurrency = new ComboBox
            {
                Location = new Point(30, 307),
                Width = 260,
                DropDownStyle = ComboBoxStyle.DropDown,
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems
            };
            cmbCurrency.SelectedIndexChanged += CmbCurrency_SelectedIndexChanged;

            lblUnits = new Label
            {
                AutoSize = true,
                Location = new Point(450, 30),
                Text = AppLocalization.T("Product Units")
            };

            txtProductUnitName = new TextBox
            {
                Location = new Point(450, 50),
                Width = 240
            };

            btnAddUnit = new Button
            {
                Location = new Point(450, 85),
                Size = new Size(60, 28),
                Text = "Add"
            };
            btnAddUnit.Click += BtnAddUnit_Click;

            btnUpdateUnit = new Button
            {
                Location = new Point(518, 85),
                Size = new Size(70, 28),
                Text = "Update"
            };
            btnUpdateUnit.Click += BtnUpdateUnit_Click;

            btnDeleteUnit = new Button
            {
                Location = new Point(596, 85),
                Size = new Size(70, 28),
                Text = "Delete"
            };
            btnDeleteUnit.Click += BtnDeleteUnit_Click;

            btnClearUnit = new Button
            {
                Location = new Point(674, 85),
                Size = new Size(80, 28),
                Text = "Clear"
            };
            btnClearUnit.Click += BtnClearUnit_Click;

            dgvProductUnits = new DataGridView
            {
                Location = new Point(450, 125),
                Size = new Size(330, 220),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowHeadersVisible = false
            };
            dgvProductUnits.SelectionChanged += DgvProductUnits_SelectionChanged;

            tabStore.Controls.Add(lblCurrency);
            tabStore.Controls.Add(cmbCurrency);
            tabStore.Controls.Add(lblUnits);
            tabStore.Controls.Add(txtProductUnitName);
            tabStore.Controls.Add(btnAddUnit);
            tabStore.Controls.Add(btnUpdateUnit);
            tabStore.Controls.Add(btnDeleteUnit);
            tabStore.Controls.Add(btnClearUnit);
            tabStore.Controls.Add(dgvProductUnits);
        }

        private void InitializeHelpUi()
        {
            var helpTabs = new TabControl
            {
                Dock = DockStyle.Fill,
                Name = "tabControlHelp"
            };

            var tabDocumentation = new TabPage
            {
                Name = "tabDocumentation",
                Text = AppLocalization.T("Documentation"),
                UseVisualStyleBackColor = true,
                Padding = new Padding(20)
            };

            var tabAboutUs = new TabPage
            {
                Name = "tabAboutUs",
                Text = AppLocalization.T("About Us"),
                UseVisualStyleBackColor = true,
                Padding = new Padding(20)
            };
            tabAboutUs.AutoScroll = true;

            var tabContact = new TabPage
            {
                Name = "tabContact",
                Text = AppLocalization.T("Contact"),
                UseVisualStyleBackColor = true,
                Padding = new Padding(20)
            };

            var tabBugReporting = new TabPage
            {
                Name = "tabBugReporting",
                Text = AppLocalization.T("Bug Reporting"),
                UseVisualStyleBackColor = true,
                Padding = new Padding(20)
            };

            var tabSoftwareVersion = new TabPage
            {
                Name = "tabSoftwareVersion",
                Text = AppLocalization.T("Software Version"),
                UseVisualStyleBackColor = true,
                Padding = new Padding(20)
            };

            var openRepositoryButton = new Button
            {
                Name = "btnOpenDocumentation",
                Text = AppLocalization.T("Open Documentation"),
                Location = new Point(20, 95),
                Size = new Size(180, 30)
            };
            openRepositoryButton.Click += (_, _) => OpenUrl(GetDocumentationUrl());

            var contactRepositoryButton = new Button
            {
                Text = AppLocalization.T("Email Us"),
                Location = new Point(20, 125),
                Size = new Size(150, 30)
            };
            contactRepositoryButton.Click += (_, _) => OpenUrl("mailto:amiar.software@gmail.com");

            var contactEmailLabel = new LinkLabel
            {
                AutoSize = true,
                Location = new Point(20, 55),
                Text = "amiar.software@gmail.com",
                LinkBehavior = LinkBehavior.HoverUnderline
            };
            contactEmailLabel.Click += (_, _) => CopyToClipboard("amiar.software@gmail.com", "Adresse e-mail copiée dans le presse-papiers.");

            var reportIssueButton = new Button
            {
                Text = AppLocalization.T("Report an issue"),
                Location = new Point(20, 95),
                Size = new Size(150, 30)
            };
            reportIssueButton.Click += (_, _) => OpenUrl("https://github.com/Abderazak-Amiar/amiar-store-manager-bug-report/issues");

            var documentationLabel = new Label
            {
                AutoSize = false,
                Location = new Point(20, 20),
                Size = new Size(700, 60),
                Text = "Use the main tabs to manage products, sales, stats, expenses, invoices, and settings. " +
                       "This Help area provides quick reference information for the application.",
                MaximumSize = new Size(700, 0)
            };

            var aboutLabel = new Label
            {
                Name = "lblAboutUs",
                AutoSize = true,
                Location = new Point(20, 20),
                Text = AppLocalization.T("Amiar Software builds practical business software focused on store operations, sales tracking, backups, and day-to-day management. Amiar Store Manager is designed to keep core workflows organized and easy to use."),
                MaximumSize = new Size(700, 0)
            };

            var contactLabel = new Label
            {
                AutoSize = false,
                Location = new Point(20, 20),
                Size = new Size(700, 30),
                Text = "Email us for product questions and support requests. Click the email below to copy it.",
                MaximumSize = new Size(700, 0)
            };

            var bugLabel = new Label
            {
                AutoSize = false,
                Location = new Point(20, 20),
                Size = new Size(700, 60),
                Text = "If you find a bug, open the issue tracker and include the steps to reproduce it, " +
                       "expected behavior, and screenshots if available.",
                MaximumSize = new Size(700, 0)
            };

            var versionLabel = new Label
            {
                AutoSize = true,
                Location = new Point(20, 20),
                Text = AppLocalization.T("App Version:")
            };

            var versionValueLabel = new Label
            {
                AutoSize = true,
                Location = new Point(20, 50),
                Text = GetAppVersionDisplay()
            };

            var runtimeLabel = new Label
            {
                AutoSize = true,
                Location = new Point(20, 85),
                Text = AppLocalization.T("Runtime Version:") + $" {Environment.Version}"
            };

            tabDocumentation.Controls.Add(documentationLabel);
            tabDocumentation.Controls.Add(openRepositoryButton);
            tabAboutUs.Controls.Add(aboutLabel);
            tabContact.Controls.Add(contactLabel);
            tabContact.Controls.Add(contactEmailLabel);
            tabContact.Controls.Add(contactRepositoryButton);
            tabBugReporting.Controls.Add(bugLabel);
            tabBugReporting.Controls.Add(reportIssueButton);
            tabSoftwareVersion.Controls.Add(versionLabel);
            tabSoftwareVersion.Controls.Add(versionValueLabel);
            tabSoftwareVersion.Controls.Add(runtimeLabel);

            helpTabs.TabPages.AddRange(new[]
            {
                tabDocumentation,
                tabAboutUs,
                tabContact,
                tabBugReporting,
                tabSoftwareVersion
            });

            tabHelp.Controls.Add(helpTabs);
        }

        private static void OpenUrl(string url)
        {
            try
            {
                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };

                System.Diagnostics.Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Échec de l'ouverture du lien : {ex.Message}", "Aide", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string GetDocumentationUrl()
        {
            return AppLocalization.CurrentLanguageCode == "fr"
                ? "https://github.com/Abderazak-Amiar/amiar-store-manager-doc/blob/master/README.fr.md"
                : "https://github.com/Abderazak-Amiar/amiar-store-manager-doc";
        }

        private static string GetAppVersionDisplay()
        {
            var version = Assembly.GetEntryAssembly()
                ?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion;

            if (string.IsNullOrWhiteSpace(version))
                version = Application.ProductVersion;

            return version.Split('+', 2)[0];
        }

        private static void CopyToClipboard(string text, string message)
        {
            try
            {
                Clipboard.SetText(text);
                MessageBox.Show(message, "Aide", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Échec de la copie du texte : {ex.Message}", "Aide", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadStorePreferences()
        {
            _loadingSettingsUi = true;
            try
            {
                using var db = new AppDbContext();
                var store = db.StoreSettings.FirstOrDefault();

                AppLocalization.SetLanguage(store?.LanguageCode);

                if (store != null)
                {
                    this.Text = store.StoreName;

                    SetStoreLogoPreview(store.LogoData);

                    txtStoreName.Text = store.StoreName;
                    txtGoogleDriveClientId.Text = store.GoogleDriveClientId ?? string.Empty;
                    txtGoogleDriveClientSecret.Text = store.GoogleDriveClientSecret ?? string.Empty;
                    txtGoogleDriveFolderId.Text = store.GoogleDriveFolderId ?? string.Empty;
                    txtGoogleDriveRefreshToken.Text = store.GoogleDriveRefreshToken ?? string.Empty;
                }
                else
                {
                    txtStoreName.Clear();
                    txtGoogleDriveClientId.Clear();
                    txtGoogleDriveClientSecret.Clear();
                    txtGoogleDriveFolderId.Clear();
                    txtGoogleDriveRefreshToken.Clear();
                }

                LoadLanguageOptions();
                LoadCurrencyOptions();
                LoadProductUnits(db, store);

                SelectLanguage(store?.LanguageCode);
                SelectCurrency(store?.CurrencyCode);

                txtCurrentPassword.Clear();
                txtNewPassword.Clear();
                txtConfirmPassword.Clear();
                txtUsernameProfile.Text = Session.CurrentUser?.Username ?? string.Empty;
            }
            finally
            {
                _loadingSettingsUi = false;
            }
        }

        private void SetStoreLogoPreview(byte[]? logoData)
        {
            if (picStoreLogo == null)
                return;

            picStoreLogo.Image?.Dispose();
            picStoreLogo.Image = null;

            if (logoData == null || logoData.Length == 0)
                return;

            using var ms = new MemoryStream(logoData);
            using var img = Image.FromStream(ms);
            picStoreLogo.Image = new Bitmap(img);

            using var iconBitmap = new Bitmap(img, 32, 32);
            this.Icon = Icon.FromHandle(iconBitmap.GetHicon());
        }

        private void ApplyLocalizedSettingsTexts()
        {
            tabControlMain.TabPages[0].Text = AppLocalization.T("Products");
            tabControlMain.TabPages[1].Text = AppLocalization.T("Sales");
            if (tabControlMain.TabPages.Contains(tabStats)) tabStats.Text = AppLocalization.T("Stats");
            if (tabControlMain.TabPages.Contains(tabExpenses)) tabExpenses.Text = AppLocalization.T("Expenses");
            if (tabControlMain.TabPages.Contains(tabInvoices)) tabInvoices.Text = AppLocalization.T("Invoices");
            if (tabControlMain.TabPages.Contains(tabHelp)) tabHelp.Text = AppLocalization.T("Help");
            if (tabControlMain.TabPages.Contains(tabSettings)) tabSettings.Text = AppLocalization.T("Settings");

            if (tabControlSettings.TabPages.Contains(tabProfile)) tabProfile.Text = AppLocalization.T("Profile");
            if (tabControlSettings.TabPages.Contains(tabStore)) tabStore.Text = AppLocalization.T("Store");
            if (tabControlSettings.TabPages.Contains(tabGoogleDrive)) tabGoogleDrive.Text = AppLocalization.T("Google Drive");
            if (tabControlSettings.TabPages.Contains(tabBackup)) tabBackup.Text = AppLocalization.T("Backup");
            if (tabLanguages != null) tabLanguages.Text = AppLocalization.T("Languages");

            if (lblCurrentPassword != null) lblCurrentPassword.Text = AppLocalization.T("Current Password");
            if (lblNewPassword != null) lblNewPassword.Text = AppLocalization.T("New Password");
            if (lblConfirmPassword != null) lblConfirmPassword.Text = AppLocalization.T("Confirm New Password");
            if (btnChangePassword != null) btnChangePassword.Text = AppLocalization.T("Change Password");
            if (lblUsernameProfile != null) lblUsernameProfile.Text = AppLocalization.T("Username");
            if (btnChangeUsername != null) btnChangeUsername.Text = AppLocalization.T("Change Username");

            if (lblStoreName != null) lblStoreName.Text = AppLocalization.T("Store Name");
            if (btnSaveStoreName != null) btnSaveStoreName.Text = AppLocalization.T("Save Name");
            if (btnChangeLogo != null) btnChangeLogo.Text = AppLocalization.T("Change Logo");

            if (lblGoogleDriveClientId != null) lblGoogleDriveClientId.Text = AppLocalization.T("Client ID");
            if (lblGoogleDriveClientSecret != null) lblGoogleDriveClientSecret.Text = AppLocalization.T("Client Secret");
            if (lblGoogleDriveFolderId != null) lblGoogleDriveFolderId.Text = AppLocalization.T("Folder ID or Name");
            if (lblGoogleDriveRefreshToken != null) lblGoogleDriveRefreshToken.Text = AppLocalization.T("Refresh Token");
            if (btnSaveGoogleDriveConfig != null) btnSaveGoogleDriveConfig.Text = AppLocalization.T("Save Google Drive Config");
            if (btnGenerateGoogleDriveRefreshToken != null) btnGenerateGoogleDriveRefreshToken.Text = AppLocalization.T("Generate Refresh Token");
            if (btnGoogleDriveHelp != null) btnGoogleDriveHelp.Text = AppLocalization.T("Setup Help");
            if (btnOpenGoogleCloudConsole != null) btnOpenGoogleCloudConsole.Text = AppLocalization.T("Open Console");
            AdjustGoogleDriveButtonLayout();

            if (lblBackupInfo != null)
                lblBackupInfo.Text = AppLocalization.T("Download creates a local copy of the SQLite database. Restore replaces the current local database file.");
            if (btnDownloadDatabaseBackup != null) btnDownloadDatabaseBackup.Text = AppLocalization.T("Download DB Backup");
            if (btnRestoreDatabaseBackup != null) btnRestoreDatabaseBackup.Text = AppLocalization.T("Upload / Restore DB Backup");
            if (btnUploadGoogleDriveBackup != null) btnUploadGoogleDriveBackup.Text = AppLocalization.T("Upload to Google Drive");
            if (btnDownloadGoogleDriveBackup != null) btnDownloadGoogleDriveBackup.Text = AppLocalization.T("Download from Google Drive");

            if (lblLanguage != null) lblLanguage.Text = AppLocalization.T("Language");
            if (btnSaveLanguagePreference != null) btnSaveLanguagePreference.Text = AppLocalization.T("Save Preference");
            if (lblCurrency != null) lblCurrency.Text = AppLocalization.T("Currency");
            if (lblUnits != null) lblUnits.Text = AppLocalization.T("Product Units");
            if (btnAddUnit != null) btnAddUnit.Text = AppLocalization.T("Add");
            if (btnUpdateUnit != null) btnUpdateUnit.Text = AppLocalization.T("Update");
            if (btnDeleteUnit != null) btnDeleteUnit.Text = AppLocalization.T("Delete");
            if (btnClearUnit != null) btnClearUnit.Text = AppLocalization.T("Clear");

            ApplyLocalizedHelpTexts();

            lblName.Text = AppLocalization.T("Name");
            lblBuyPrice.Text = AppLocalization.T("Buy Price");
            lblSellPrice.Text = AppLocalization.T("Sell Price");
            lblQuantity.Text = AppLocalization.T("Quantity");
            lblUnit.Text = AppLocalization.T("Unit");
            lblSearch.Text = AppLocalization.T("Search");
            btnAdd.Text = AppLocalization.T("Add");
            btnUpdate.Text = AppLocalization.T("Update");
            btnDelete.Text = AppLocalization.T("Delete");
            btnClear.Text = AppLocalization.T("Clear");
            chkShowBuyPrice.Text = AppLocalization.T("Show Buy Price");

            btnProductPrev.Text = $"< {AppLocalization.T("Previous")}";
            btnProductNext.Text = $"{AppLocalization.T("Next")} >";

            lblSaleProduct.Text = AppLocalization.T("Product");
            lblSaleQuantity.Text = AppLocalization.T("Quantity");
            lblSaleUnitPrice.Text = AppLocalization.T("Unit Price");
            lblSaleTotal.Text = AppLocalization.T("Total");
            lblBuyerName.Text = AppLocalization.T("Buyer");
            lblFilterBuyer.Text = AppLocalization.T("Buyer");
            lblFilterProduct.Text = AppLocalization.T("Product");
            chkFilterDate.Text = AppLocalization.T("Date");
            btnClearSaleFilter.Text = AppLocalization.T("Clear Filters");
            btnSell.Text = AppLocalization.T("Add Sale");
            btnDeleteSale.Text = AppLocalization.T("Delete Sale");
            btnUpdateSale.Text = AppLocalization.T("Update Sale");
            btnClearSale.Text = AppLocalization.T("Clear");
            btnPrintInvoice.Text = AppLocalization.T("Invoice");

            btnSalePrev.Text = $"< {AppLocalization.T("Previous")}";
            btnSaleNext.Text = $"{AppLocalization.T("Next")} >";

            lblStatsFilterProduct.Text = AppLocalization.T("Product");
            lblStatsFilterBuyer.Text = AppLocalization.T("Buyer");
            lblStatsFilterPeriod.Text = AppLocalization.T("Period");
            btnClearStatsFilter.Text = AppLocalization.T("Clear Filters");
            btnPrintReport.Text = AppLocalization.T("Print Report");

            lblExpenseDescription.Text = AppLocalization.T("Description");
            lblExpenseAmount.Text = AppLocalization.T("Total");
            lblExpenseCategory.Text = AppLocalization.T("Category");
            lblExpenseFilterCategory.Text = AppLocalization.T("Category");
            btnAddExpense.Text = AppLocalization.T("Add Expense");
            btnUpdateExpense.Text = AppLocalization.T("Update Expense");
            btnDeleteExpense.Text = AppLocalization.T("Delete Expense");
            btnClearExpense.Text = AppLocalization.T("Clear");
            btnClearExpenseFilter.Text = AppLocalization.T("Clear Filters");

            btnExpensePrev.Text = $"< {AppLocalization.T("Previous")}";
            btnExpenseNext.Text = $"{AppLocalization.T("Next")} >";

            lblInvoiceFilterBuyer.Text = AppLocalization.T("Buyer");
            lblInvoiceFilterStatus.Text = AppLocalization.T("Status");
            btnClearInvoiceFilter.Text = AppLocalization.T("Clear Filters");
            btnInvoicePreview.Text = AppLocalization.T("Invoice");
            btnDeleteInvoice.Text = AppLocalization.T("Delete");

            btnInvoicePrev.Text = $"< {AppLocalization.T("Previous")}";
            btnInvoiceNext.Text = $"{AppLocalization.T("Next")} >";

            lblSaleProduct.Text = AppLocalization.T("Product");
            if(lblStoreName != null) lblStoreName.Text = AppLocalization.T("Store");
            if (btnSaveStoreName != null) btnSaveStoreName.Text = AppLocalization.T("Save Preference");
            if (btnChangePassword != null) btnChangePassword.Text = AppLocalization.T("Update");
            if (btnChangeUsername != null) btnChangeUsername.Text = AppLocalization.T("Update");
            if (btnLogout != null) btnLogout.Text = AppLocalization.T("Logout");

            RefreshLocalizedGridHeaders();
        }

        private void AdjustGoogleDriveButtonLayout()
        {
            if (btnSaveGoogleDriveConfig == null || btnGenerateGoogleDriveRefreshToken == null ||
                btnGoogleDriveHelp == null || btnOpenGoogleCloudConsole == null)
            {
                return;
            }

            var buttons = new[]
            {
                btnSaveGoogleDriveConfig,
                btnGenerateGoogleDriveRefreshToken,
                btnGoogleDriveHelp,
                btnOpenGoogleCloudConsole
            };

            var left = 30;
            const int top = 255;
            const int gap = 10;

            foreach (var button in buttons)
            {
                var textSize = TextRenderer.MeasureText(
                    button.Text ?? string.Empty,
                    button.Font,
                    new Size(int.MaxValue, int.MaxValue),
                    TextFormatFlags.SingleLine | TextFormatFlags.NoPadding);

                var width = Math.Max(button.Width, textSize.Width + 24);
                button.Location = new Point(left, top);
                button.Size = new Size(width, button.Height);
                button.TextAlign = ContentAlignment.MiddleCenter;

                left += width + gap;
            }
        }

        private void ApplyLocalizedHelpTexts()
        {
            if (tabHelp == null)
                return;

            var helpTabs = tabHelp.Controls.Find("tabControlHelp", true).OfType<TabControl>().FirstOrDefault();
            if (helpTabs == null)
                return;

            tabHelp.Text = AppLocalization.T("Help");

            var tabDocumentation = helpTabs.TabPages["tabDocumentation"];
            var tabAboutUs = helpTabs.TabPages["tabAboutUs"];
            var tabContact = helpTabs.TabPages["tabContact"];
            var tabBugReporting = helpTabs.TabPages["tabBugReporting"];
            var tabSoftwareVersion = helpTabs.TabPages["tabSoftwareVersion"];

            if (tabDocumentation != null) tabDocumentation.Text = AppLocalization.T("Documentation");
            if (tabAboutUs != null) tabAboutUs.Text = AppLocalization.T("About Us");
            if (tabContact != null) tabContact.Text = AppLocalization.T("Contact");
            if (tabBugReporting != null) tabBugReporting.Text = AppLocalization.T("Bug Reporting");
            if (tabSoftwareVersion != null) tabSoftwareVersion.Text = AppLocalization.T("Software Version");

            var openDocumentationButton = tabDocumentation?.Controls.Find("btnOpenDocumentation", true).OfType<Button>().FirstOrDefault();
            if (openDocumentationButton != null) openDocumentationButton.Text = AppLocalization.T("Open Documentation");

            var emailButton = tabContact?.Controls.Find("btnEmailUs", true).OfType<Button>().FirstOrDefault();
            if (emailButton != null) emailButton.Text = AppLocalization.T("Email Us");

            var emailLabel = tabContact?.Controls.Find("lblSupportEmail", true).OfType<LinkLabel>().FirstOrDefault();
            if (emailLabel != null) emailLabel.Text = "amiar.software@gmail.com";

            var documentationLabel = tabDocumentation?.Controls.Find("lblHelpDocumentation", true).OfType<Label>().FirstOrDefault();
            if (documentationLabel != null)
                documentationLabel.Text = AppLocalization.T("Use the main tabs to manage products, sales, stats, expenses, invoices, and settings. This Help area provides quick reference information for the application.");

            var aboutLabel = tabAboutUs?.Controls.Find("lblAboutUs", true).OfType<Label>().FirstOrDefault();
            if (aboutLabel != null)
            {
                aboutLabel.AutoSize = true;
                aboutLabel.MaximumSize = new Size(700, 0);
                aboutLabel.Text = AppLocalization.T("Amiar Software builds practical business software focused on store operations, sales tracking, backups, and day-to-day management. Amiar Store Manager is designed to keep core workflows organized and easy to use.");
            }

            var contactLabel = tabContact?.Controls.Find("lblHelpContact", true).OfType<Label>().FirstOrDefault();
            if (contactLabel != null)
                contactLabel.Text = AppLocalization.T("Email us for product questions and support requests. Click the email below to copy it.");

            var bugLabel = tabBugReporting?.Controls.Find("lblBugReporting", true).OfType<Label>().FirstOrDefault();
            if (bugLabel != null)
                bugLabel.Text = AppLocalization.T("If you find a bug, open the issue tracker and include the steps to reproduce it, expected behavior, and screenshots if available.");

            var versionLabel = tabSoftwareVersion?.Controls.Find("lblApplicationVersion", true).OfType<Label>().FirstOrDefault();
            if (versionLabel != null)
                versionLabel.Text = AppLocalization.T("App Version:");

            var runtimeLabel = tabSoftwareVersion?.Controls.Find("lblRuntimeVersion", true).OfType<Label>().FirstOrDefault();
            if (runtimeLabel != null)
                runtimeLabel.Text = AppLocalization.T("Runtime Version:") + $" {Environment.Version}";
        }

        private void RefreshLocalizedGridHeaders()
        {
            if (dgvProducts.Columns.Contains("UnitName")) dgvProducts.Columns["UnitName"].HeaderText = AppLocalization.T("Unit");
            if (dgvProducts.Columns.Contains("Date")) dgvProducts.Columns["Date"].HeaderText = AppLocalization.T("Date");
            if (dgvProducts.Columns.Contains("Name")) dgvProducts.Columns["Name"].HeaderText = AppLocalization.T("Name");
            if (dgvProducts.Columns.Contains("SellPrice")) dgvProducts.Columns["SellPrice"].HeaderText = AppLocalization.T("Sell Price");
            if (dgvProducts.Columns.Contains("BuyPrice")) dgvProducts.Columns["BuyPrice"].HeaderText = AppLocalization.T("Buy Price");
            if (dgvProducts.Columns.Contains("Quantity"))
            {
                dgvProducts.Columns["Quantity"].HeaderText = AppLocalization.T("Quantity");
                dgvProducts.Columns["Quantity"].DefaultCellStyle.Format = "0.#";
                dgvProducts.Columns["Quantity"].DefaultCellStyle.FormatProvider = CultureInfo.InvariantCulture;
            }

            lblTotalProfit.Text = AppLocalization.T("Total Profit") + ":";

            if (dgvInvoices.Columns.Contains("Status") && dgvInvoices.Columns["Status"] is DataGridViewComboBoxColumn statusCol)
            {
                statusCol.HeaderText = AppLocalization.T("Status");
                statusCol.Items.Clear();
                statusCol.Items.AddRange(new object[]
                {
                    AppLocalization.T("No"),
                    AppLocalization.T("Yes"),
                    AppLocalization.T("Partially Paid")
                });
            }

            if (dgvInvoices.Columns.Contains("Buyer")) dgvInvoices.Columns["Buyer"].HeaderText = AppLocalization.T("Client");
            if (dgvInvoices.Columns.Contains("Date")) dgvInvoices.Columns["Date"].HeaderText = AppLocalization.T("Date");
            if (dgvInvoices.Columns.Contains("Total")) dgvInvoices.Columns["Total"].HeaderText = AppLocalization.T("Total");
            if (dgvInvoices.Columns.Contains("DueAmount")) dgvInvoices.Columns["DueAmount"].HeaderText = AppLocalization.T("Due");
            if (dgvInvoices.Columns.Contains("Paid")) dgvInvoices.Columns["Paid"].HeaderText = AppLocalization.T("Paid");
            if (dgvInvoices.Columns.Contains("Hour")) dgvInvoices.Columns["Hour"].HeaderText = AppLocalization.T("Hour");

            if (dgvSales.Columns.Contains("Product")) dgvSales.Columns["Product"].HeaderText = AppLocalization.T("Product");
            if (dgvSales.Columns.Contains("Buyer")) dgvSales.Columns["Buyer"].HeaderText = AppLocalization.T("Client");
            if (dgvSales.Columns.Contains("Date")) dgvSales.Columns["Date"].HeaderText = AppLocalization.T("Date");
            if (dgvSales.Columns.Contains("Quantity"))
            {
                dgvSales.Columns["Quantity"].HeaderText = AppLocalization.T("Quantity");
                dgvSales.Columns["Quantity"].DefaultCellStyle.Format = "0.#";
                dgvSales.Columns["Quantity"].DefaultCellStyle.FormatProvider = CultureInfo.InvariantCulture;
            }
            if (dgvSales.Columns.Contains("UnitPrice")) dgvSales.Columns["UnitPrice"].HeaderText = AppLocalization.T("Unit Price");
            if (dgvSales.Columns.Contains("Total")) dgvSales.Columns["Total"].HeaderText = AppLocalization.T("Total");

            if (dgvStats.Columns.Contains("Product")) dgvStats.Columns["Product"].HeaderText = AppLocalization.T("Product");
            if (dgvStats.Columns.Contains("Date")) dgvStats.Columns["Date"].HeaderText = AppLocalization.T("Date");
            if (dgvStats.Columns.Contains("Hour")) dgvStats.Columns["Hour"].HeaderText = AppLocalization.T("Hour");
            if (dgvStats.Columns.Contains("UnitsSold"))
            {
                dgvStats.Columns["UnitsSold"].HeaderText = AppLocalization.T("Units Sold");
                dgvStats.Columns["UnitsSold"].DefaultCellStyle.Format = "0.#";
                dgvStats.Columns["UnitsSold"].DefaultCellStyle.FormatProvider = CultureInfo.InvariantCulture;
            }
            if (dgvStats.Columns.Contains("Revenue")) dgvStats.Columns["Revenue"].HeaderText = AppLocalization.T("Revenue");
            if (dgvStats.Columns.Contains("Cost")) dgvStats.Columns["Cost"].HeaderText = AppLocalization.T("Cost");
            if (dgvStats.Columns.Contains("Profit")) dgvStats.Columns["Profit"].HeaderText = AppLocalization.T("Profit");

            if (dgvExpenses.Columns.Contains("Description")) dgvExpenses.Columns["Description"].HeaderText = AppLocalization.T("Description");
            if (dgvExpenses.Columns.Contains("Amount")) dgvExpenses.Columns["Amount"].HeaderText = AppLocalization.T("Total");
            if (dgvExpenses.Columns.Contains("Category")) dgvExpenses.Columns["Category"].HeaderText = AppLocalization.T("Category");
            if (dgvExpenses.Columns.Contains("Date")) dgvExpenses.Columns["Date"].HeaderText = AppLocalization.T("Date");
        }

        private void LoadLanguageOptions()
        {
            if (cmbLanguage == null)
                return;

            var current = (cmbLanguage.SelectedItem as LanguageOption)?.Code;
            cmbLanguage.Items.Clear();
            cmbLanguage.Items.AddRange(new object[]
            {
                new LanguageOption { Code = "en", Name = AppLocalization.T("English") },
                new LanguageOption { Code = "fr", Name = AppLocalization.T("French") }
            });
            SelectLanguage(current);
        }

        private void LoadCurrencyOptions()
        {
            if (cmbCurrency == null)
                return;

            var current = (cmbCurrency.SelectedItem as CurrencyOption)?.Code;
            var currencies = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Select(c => new RegionInfo(c.Name))
                .GroupBy(r => r.ISOCurrencySymbol)
                .Select(g => g.First())
                .OrderBy(r => r.ISOCurrencySymbol)
                .Select(r => new CurrencyOption
                {
                    Code = r.ISOCurrencySymbol,
                    Name = r.CurrencyEnglishName
                })
                .ToList();

            foreach (var fallbackCurrency in GetFallbackCurrencyOptions())
                AddCurrencyOptionIfMissing(currencies, fallbackCurrency.Code, fallbackCurrency.Name);

            currencies = currencies
                .OrderBy(c => c.Code)
                .ToList();

            cmbCurrency.Items.Clear();
            cmbCurrency.Items.AddRange(currencies.Cast<object>().ToArray());
            SelectCurrency(current);
        }

        private static void AddCurrencyOptionIfMissing(List<CurrencyOption> currencies, string code, string name)
        {
            if (currencies.Any(c => string.Equals(c.Code, code, StringComparison.OrdinalIgnoreCase)))
                return;

            currencies.Add(new CurrencyOption
            {
                Code = code,
                Name = name
            });
        }

        private static IEnumerable<CurrencyOption> GetFallbackCurrencyOptions()
        {
            return new[]
            {
                new CurrencyOption { Code = "AED", Name = "UAE Dirham" },
                new CurrencyOption { Code = "AFN", Name = "Afghan Afghani" },
                new CurrencyOption { Code = "ALL", Name = "Albanian Lek" },
                new CurrencyOption { Code = "AMD", Name = "Armenian Dram" },
                new CurrencyOption { Code = "ARS", Name = "Argentine Peso" },
                new CurrencyOption { Code = "AUD", Name = "Australian Dollar" },
                new CurrencyOption { Code = "BHD", Name = "Bahraini Dinar" },
                new CurrencyOption { Code = "BDT", Name = "Bangladeshi Taka" },
                new CurrencyOption { Code = "BHD", Name = "Bahraini Dinar" },
                new CurrencyOption { Code = "BND", Name = "Brunei Dollar" },
                new CurrencyOption { Code = "BRL", Name = "Brazilian Real" },
                new CurrencyOption { Code = "CAD", Name = "Canadian Dollar" },
                new CurrencyOption { Code = "CHF", Name = "Swiss Franc" },
                new CurrencyOption { Code = "CNY", Name = "Chinese Yuan" },
                new CurrencyOption { Code = "COP", Name = "Colombian Peso" },
                new CurrencyOption { Code = "CZK", Name = "Czech Koruna" },
                new CurrencyOption { Code = "DKK", Name = "Danish Krone" },
                new CurrencyOption { Code = "EGP", Name = "Egyptian Pound" },
                new CurrencyOption { Code = "EUR", Name = "Euro" },
                new CurrencyOption { Code = "GBP", Name = "British Pound Sterling" },
                new CurrencyOption { Code = "HKD", Name = "Hong Kong Dollar" },
                new CurrencyOption { Code = "HUF", Name = "Hungarian Forint" },
                new CurrencyOption { Code = "IDR", Name = "Indonesian Rupiah" },
                new CurrencyOption { Code = "INR", Name = "Indian Rupee" },
                new CurrencyOption { Code = "IQD", Name = "Iraqi Dinar" },
                new CurrencyOption { Code = "IRR", Name = "Iranian Rial" },
                new CurrencyOption { Code = "JPY", Name = "Japanese Yen" },
                new CurrencyOption { Code = "KRW", Name = "South Korean Won" },
                new CurrencyOption { Code = "KWD", Name = "Kuwaiti Dinar" },
                new CurrencyOption { Code = "LKR", Name = "Sri Lankan Rupee" },
                new CurrencyOption { Code = "MAD", Name = "Moroccan Dirham" },
                new CurrencyOption { Code = "MDL", Name = "Moldovan Leu" },
                new CurrencyOption { Code = "MGA", Name = "Malagasy Ariary" },
                new CurrencyOption { Code = "MKD", Name = "Macedonian Denar" },
                new CurrencyOption { Code = "MMK", Name = "Myanmar Kyat" },
                new CurrencyOption { Code = "MNT", Name = "Mongolian Tugrik" },
                new CurrencyOption { Code = "MOP", Name = "Macanese Pataca" },
                new CurrencyOption { Code = "MUR", Name = "Mauritian Rupee" },
                new CurrencyOption { Code = "MXN", Name = "Mexican Peso" },
                new CurrencyOption { Code = "MYR", Name = "Malaysian Ringgit" },
                new CurrencyOption { Code = "NAD", Name = "Namibian Dollar" },
                new CurrencyOption { Code = "NGN", Name = "Nigerian Naira" },
                new CurrencyOption { Code = "NOK", Name = "Norwegian Krone" },
                new CurrencyOption { Code = "NZD", Name = "New Zealand Dollar" },
                new CurrencyOption { Code = "OMR", Name = "Omani Rial" },
                new CurrencyOption { Code = "PHP", Name = "Philippine Peso" },
                new CurrencyOption { Code = "PKR", Name = "Pakistani Rupee" },
                new CurrencyOption { Code = "PLN", Name = "Polish Zloty" },
                new CurrencyOption { Code = "QAR", Name = "Qatari Riyal" },
                new CurrencyOption { Code = "RON", Name = "Romanian Leu" },
                new CurrencyOption { Code = "RSD", Name = "Serbian Dinar" },
                new CurrencyOption { Code = "RUB", Name = "Russian Ruble" },
                new CurrencyOption { Code = "SAR", Name = "Saudi Riyal" },
                new CurrencyOption { Code = "SEK", Name = "Swedish Krona" },
                new CurrencyOption { Code = "SGD", Name = "Singapore Dollar" },
                new CurrencyOption { Code = "THB", Name = "Thai Baht" },
                new CurrencyOption { Code = "TND", Name = "Tunisian Dinar" },
                new CurrencyOption { Code = "TRY", Name = "Turkish Lira" },
                new CurrencyOption { Code = "TWD", Name = "New Taiwan Dollar" },
                new CurrencyOption { Code = "USD", Name = "US Dollar" },
                new CurrencyOption { Code = "VND", Name = "Vietnamese Dong" },
                new CurrencyOption { Code = "ZAR", Name = "South African Rand" }
            };
        }

        private void LoadProductUnits(AppDbContext db, StoreSettings? store)
        {
            var units = ParseProductUnits(store?.ProductUnitsJson);
            var usedUnits = db.Products
                .Select(p => p.UnitName)
                .Where(u => !string.IsNullOrWhiteSpace(u))
                .Select(u => u.Trim())
                .AsEnumerable()
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            foreach (var usedUnit in usedUnits)
            {
                if (!units.Any(u => string.Equals(u, usedUnit, StringComparison.OrdinalIgnoreCase)))
                    units.Add(usedUnit);
            }

            _productUnits = units;
            RefreshProductUnitControls();
        }

        private static List<string> ParseProductUnits(string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new List<string> { "Bottle", "Kg", "Piece" };

            try
            {
                var units = JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
                return units
                    .Select(u => u?.Trim())
                    .Where(u => !string.IsNullOrWhiteSpace(u))
                    .Select(u => u!)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();
            }
            catch
            {
                return new List<string> { "Bottle", "Kg", "Piece" };
            }
        }

        private void RefreshProductUnitControls(string? selectedUnit = null)
        {
            if (cmbUnit != null)
            {
                cmbUnit.BeginUpdate();
                cmbUnit.Items.Clear();
                cmbUnit.Items.AddRange(_productUnits.Cast<object>().ToArray());
                cmbUnit.EndUpdate();
            }

            if (dgvProductUnits != null)
            {
                var rows = _productUnits.Select(u => new ProductUnitRow { Name = u }).ToList();
                dgvProductUnits.DataSource = rows;
                if (dgvProductUnits.Columns.Contains("Name"))
                    dgvProductUnits.Columns["Name"].HeaderText = "Unit";
            }

            if (!string.IsNullOrWhiteSpace(selectedUnit))
                SetSelectedProductUnit(selectedUnit);
            else if (cmbUnit is not null && cmbUnit.Items.Count > 0 && cmbUnit.SelectedIndex < 0)
                cmbUnit.SelectedIndex = 0;

            if (dgvProductUnits?.Rows.Count > 0)
            {
                dgvProductUnits.ClearSelection();
                dgvProductUnits.Rows[0].Selected = true;
            }
        }

        private void SelectLanguage(string? languageCode)
        {
            if (cmbLanguage == null || cmbLanguage.Items.Count == 0)
                return;

            var match = cmbLanguage.Items.Cast<LanguageOption>()
                .FirstOrDefault(x => string.Equals(x.Code, languageCode, StringComparison.OrdinalIgnoreCase));
            cmbLanguage.SelectedItem = match ?? cmbLanguage.Items[0];
        }

        private void SelectCurrency(string? currencyCode)
        {
            if (cmbCurrency == null || cmbCurrency.Items.Count == 0)
                return;

            var match = cmbCurrency.Items.Cast<CurrencyOption>()
                .FirstOrDefault(x => string.Equals(x.Code, currencyCode, StringComparison.OrdinalIgnoreCase));
            cmbCurrency.SelectedItem = match ?? cmbCurrency.Items[0];
        }

        private void UpdateStoreSettings(Action<StoreSettings> updater)
        {
            using var db = new AppDbContext();
            var store = db.StoreSettings.FirstOrDefault();
            if (store == null)
            {
                store = new StoreSettings();
                db.StoreSettings.Add(store);
            }

            updater(store);
            db.SaveChanges();
        }

        private void PersistProductUnits()
        {
            UpdateStoreSettings(store => store.ProductUnitsJson = JsonSerializer.Serialize(_productUnits));
        }

        private void CmbLanguage_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_loadingSettingsUi || cmbLanguage?.SelectedItem is not LanguageOption language)
                return;

            AppLocalization.SetLanguage(language.Code);
            ApplyLocalizedSettingsTexts();
            UpdateGreeting();
        }

        private void BtnSaveLanguagePreference_Click(object? sender, EventArgs e)
        {
            if (cmbLanguage?.SelectedItem is not LanguageOption language)
                return;

            UpdateStoreSettings(store => store.LanguageCode = language.Code);
            AppLocalization.SetLanguage(language.Code);
            ApplyLocalizedSettingsTexts();
            UpdateGreeting();
            LoadInvoicesTab();
            LoadProducts(_productFilter);
            LoadSalesTab();
            LoadStatsTab();
            LoadExpensesTab();
            MessageBox.Show(AppLocalization.T("Language changed successfully."), AppLocalization.T("Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CmbCurrency_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_loadingSettingsUi || cmbCurrency?.SelectedItem is not CurrencyOption currency)
                return;

            UpdateStoreSettings(store => store.CurrencyCode = currency.Code);
        }

        private void DgvProductUnits_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvProductUnits?.CurrentRow == null || dgvProductUnits.CurrentRow.Cells.Count == 0)
                return;

            var name = dgvProductUnits.CurrentRow.Cells["Name"]?.Value?.ToString()?.Trim();
            if (string.IsNullOrWhiteSpace(name))
                return;

            txtProductUnitName!.Text = name;
        }

        private void BtnAddUnit_Click(object? sender, EventArgs e)
        {
            var unitName = txtProductUnitName?.Text.Trim();
            if (string.IsNullOrWhiteSpace(unitName))
            {
                MessageBox.Show(AppLocalization.T("Unit name is required."), AppLocalization.T("Validation"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_productUnits.Any(u => string.Equals(u, unitName, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show(AppLocalization.T("This unit already exists."), AppLocalization.T("Duplicate"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _productUnits.Add(unitName);
            PersistProductUnits();
            RefreshProductUnitControls(unitName);
        }

        private void BtnUpdateUnit_Click(object? sender, EventArgs e)
        {
            if (dgvProductUnits?.CurrentRow == null)
            {
                MessageBox.Show(AppLocalization.T("Select a unit to update."), AppLocalization.T("Information"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var oldName = dgvProductUnits.CurrentRow.Cells["Name"]?.Value?.ToString()?.Trim();
            var newName = txtProductUnitName?.Text.Trim();

            if (string.IsNullOrWhiteSpace(oldName) || string.IsNullOrWhiteSpace(newName))
            {
                MessageBox.Show(AppLocalization.T("Unit name is required."), AppLocalization.T("Validation"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!string.Equals(oldName, newName, StringComparison.OrdinalIgnoreCase) &&
                _productUnits.Any(u => string.Equals(u, newName, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show(AppLocalization.T("This unit already exists."), AppLocalization.T("Duplicate"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var index = _productUnits.FindIndex(u => string.Equals(u, oldName, StringComparison.OrdinalIgnoreCase));
            if (index < 0)
                return;

            using (var db = new AppDbContext())
            {
                var productsToUpdate = db.Products.Where(p => p.UnitName == oldName).ToList();
                foreach (var product in productsToUpdate)
                    product.UnitName = newName;

                db.SaveChanges();
            }

            _productUnits[index] = newName;
            PersistProductUnits();
            RefreshProductUnitControls(newName);
            LoadProducts();
        }

        private void BtnDeleteUnit_Click(object? sender, EventArgs e)
        {
            if (dgvProductUnits?.CurrentRow == null)
            {
                MessageBox.Show(AppLocalization.T("Select a unit to delete."), AppLocalization.T("Information"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var unitName = dgvProductUnits.CurrentRow.Cells["Name"]?.Value?.ToString()?.Trim();
            if (string.IsNullOrWhiteSpace(unitName))
                return;

            using var db = new AppDbContext();
            var inUse = db.Products.Any(p => p.UnitName == unitName);
            if (inUse)
            {
                MessageBox.Show(AppLocalization.T("This unit is used by one or more products and cannot be deleted."), AppLocalization.T("Validation"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var ok = ShowEnglishYesNoConfirmation(AppLocalization.T("Delete this unit?"), AppLocalization.T("Confirm"));
            if (ok != DialogResult.Yes) return;

            _productUnits.RemoveAll(u => string.Equals(u, unitName, StringComparison.OrdinalIgnoreCase));
            PersistProductUnits();
            RefreshProductUnitControls();
            ClearUnitEditor();
        }

        private void BtnClearUnit_Click(object? sender, EventArgs e) => ClearUnitEditor();

        private void ClearUnitEditor()
        {
            if (txtProductUnitName != null)
                txtProductUnitName.Clear();

            if (dgvProductUnits?.Rows.Count > 0)
            {
                dgvProductUnits.ClearSelection();
                dgvProductUnits.CurrentCell = null;
            }
        }

    private void BtnChangeUsername_Click(object? sender, EventArgs e)
    {
        if (Session.CurrentUser == null) return;

        var newUsername = txtUsernameProfile.Text.Trim();
            if (string.IsNullOrEmpty(newUsername))
            {
                MessageBox.Show(AppLocalization.T("Username cannot be empty."), AppLocalization.T("Validation"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var db = new Data.AppDbContext();
        if (db.Users.Any(u => u.Username == newUsername && u.Id != Session.CurrentUser.Id))
        {
            MessageBox.Show(AppLocalization.T("That username is already taken."), AppLocalization.T("Validation"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var user = db.Users.Find(Session.CurrentUser.Id);
        if (user == null) return;

        user.Username = newUsername;
        db.SaveChanges();
        Session.CurrentUser.Username = newUsername;

        UpdateGreeting();
        MessageBox.Show(AppLocalization.T("Username updated successfully."), AppLocalization.T("Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            MessageBox.Show(AppLocalization.T("Please fill in all password fields."), AppLocalization.T("Validation"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (newPass != confirm)
        {
            MessageBox.Show(AppLocalization.T("New password and confirmation do not match."), AppLocalization.T("Validation"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var db = new Data.AppDbContext();
        var user = db.Users.Find(Session.CurrentUser.Id);
        if (user == null) return;

        if (user.Password != current)
        {
            MessageBox.Show(AppLocalization.T("Current password is incorrect."), AppLocalization.T("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        user.Password = newPass;
        db.SaveChanges();
        Session.CurrentUser.Password = newPass;

        txtCurrentPassword.Clear();
        txtNewPassword.Clear();
        txtConfirmPassword.Clear();
        MessageBox.Show(AppLocalization.T("Password changed successfully."), AppLocalization.T("Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void BtnSaveStoreName_Click(object? sender, EventArgs e)
    {
        var name = txtStoreName.Text.Trim();
        if (string.IsNullOrEmpty(name))
        {
            MessageBox.Show(AppLocalization.T("Store name cannot be empty."), AppLocalization.T("Validation"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        MessageBox.Show(AppLocalization.T("Store name saved."), AppLocalization.T("Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void BtnChangeLogo_Click(object? sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Title = "Sélectionner une image de logo",
                Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.gif",
                RestoreDirectory = true
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
        MessageBox.Show(AppLocalization.T("Logo updated."), AppLocalization.T("Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            MessageBox.Show(AppLocalization.T("Google Drive settings saved."), AppLocalization.T("Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                MessageBox.Show(AppLocalization.T("Refresh token generated and saved."), AppLocalization.T("Google Drive"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Échec de génération du jeton d'actualisation : {ex.Message}", "Google Drive", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGoogleDriveHelp_Click(object? sender, EventArgs e)
        {
            MessageBox.Show(
                "Configuration Google Drive :\n\n" +
                "1. Ouvrez Google Cloud Console et créez un projet.\n" +
                "2. Activez l'API Google Drive.\n" +
                "3. Créez un ID client OAuth pour une application de bureau.\n" +
                "4. Copiez l'ID client et le secret client dans cet onglet.\n" +
                "5. Cliquez sur 'Générer le jeton d'actualisation' puis connectez-vous.\n" +
                "6. Collez l'ID ou le nom du dossier Google Drive si vous souhaitez enregistrer les sauvegardes dans un dossier spécifique.\n" +
                "7. Enregistrez le jeton d'actualisation, puis utilisez Téléverser / Télécharger depuis Google Drive.",
                "Aide Google Drive",
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
                MessageBox.Show($"Échec de l'ouverture de Google Cloud Console : {ex.Message}", "Google Drive", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDownloadDatabaseBackup_Click(object? sender, EventArgs e)
        {
            var databasePath = GetDatabaseFilePath();
            if (!File.Exists(databasePath))
            {
                MessageBox.Show(AppLocalization.T("The database file was not found."), AppLocalization.T("Backup"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var dlg = new SaveFileDialog
            {
                Title = AppLocalization.T("Save Database Backup"),
                Filter = "SQLite Database|*.db",
                FileName = $"KabyliaTaste-{DateTime.Now:yyyyMMdd-HHmmss}.db",
                InitialDirectory = GetAppDataFolder(),
                RestoreDirectory = true
            };

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                File.Copy(databasePath, dlg.FileName, true);
                CopyRelatedSqliteFiles(databasePath, dlg.FileName);
                MessageBox.Show(AppLocalization.T("Database backup created successfully."), AppLocalization.T("Backup"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{AppLocalization.T("Failed to create backup:")} {ex.Message}", AppLocalization.T("Backup"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRestoreDatabaseBackup_Click(object? sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog
            {
                Title = AppLocalization.T("Select Database Backup"),
                Filter = "SQLite Database|*.db",
                InitialDirectory = GetAppDataFolder(),
                RestoreDirectory = true,
                CheckFileExists = true
            };

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            var confirm = MessageBox.Show(
                AppLocalization.T("This will replace the current local database file. Continue?"),
                AppLocalization.T("Restore Backup"),
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

                MessageBox.Show(AppLocalization.T("Database restored. The application will restart."), AppLocalization.T("Backup"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Restart();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{AppLocalization.T("Failed to restore backup:")} {ex.Message}", AppLocalization.T("Backup"), MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                MessageBox.Show(AppLocalization.T("Database uploaded to Google Drive successfully."), AppLocalization.T("Google Drive"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{AppLocalization.T("Failed to upload database to Google Drive:")} {ex.Message}", AppLocalization.T("Google Drive"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnDownloadGoogleDriveBackup_Click(object? sender, EventArgs e)
        {
            try
            {
                using var progressToast = new BackupToastForm();
                progressToast.ShowToast(AppLocalization.T("Downloading database backup from Google Drive..."));

                var progress = new Progress<int>(percent =>
                {
                    progressToast.SetProgress(percent, $"{AppLocalization.T("Downloading database backup from Google Drive...")} {percent}%");
                });

                await Task.Run(() =>
                {
                    var store = GetGoogleDriveConfiguredStore();
                    var service = new GoogleDriveBackupService();
                    service.DownloadDatabaseBackup(store, GetDatabaseFilePath(), progress);
                });

                progressToast.SetProgress(100, AppLocalization.T("Download complete."));
                MessageBox.Show(AppLocalization.T("Database downloaded from Google Drive successfully. The application will restart."), AppLocalization.T("Google Drive"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Restart();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{AppLocalization.T("Failed to download database from Google Drive:")} {ex.Message}", AppLocalization.T("Google Drive"), MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private static string GetDatabaseFilePath() => Path.Combine(GetAppDataFolder(), "app.db");

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
