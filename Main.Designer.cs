namespace KabyliaTaste
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.TabControl tabControlMain;
    private System.Windows.Forms.TabPage tabProducts;
    private System.Windows.Forms.TabPage tabSales;
    private System.Windows.Forms.TabPage tabStats;
    private System.Windows.Forms.TabPage tabSettings;
    private System.Windows.Forms.Panel panelLeft;
    private System.Windows.Forms.Label lblName;
    private System.Windows.Forms.TextBox txtName;
    private System.Windows.Forms.CheckBox chkShowBuyPrice;
    private System.Windows.Forms.Label lblBuyPrice;
    private System.Windows.Forms.NumericUpDown numBuyPrice;
    private System.Windows.Forms.Label lblSellPrice;
    private System.Windows.Forms.NumericUpDown numSellPrice;
    // Stats tab
    private System.Windows.Forms.DataGridView dgvStats;
    private System.Windows.Forms.Label lblTotalProfit;
    private System.Windows.Forms.Label lblTotalProfitValue;
    private System.Windows.Forms.Label lblQuantity;
    private System.Windows.Forms.NumericUpDown numQuantity;
    private System.Windows.Forms.Label lblUnit;
    private System.Windows.Forms.ComboBox cmbUnit;
    private System.Windows.Forms.Button btnAdd;
    private System.Windows.Forms.Button btnUpdate;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnClear;
    private System.Windows.Forms.DataGridView dgvProducts;
    private System.Windows.Forms.Panel panelSearch;
    private System.Windows.Forms.Label lblSearch;
    private System.Windows.Forms.TextBox txtSearch;
    private System.Windows.Forms.Panel panelProductPagination;
    private System.Windows.Forms.Button btnProductPrev;
    private System.Windows.Forms.Button btnProductNext;
    private System.Windows.Forms.Label lblProductPage;
    // Sales tab
    private System.Windows.Forms.Panel panelSaleLeft;
    private System.Windows.Forms.Label lblSaleProduct;
    private System.Windows.Forms.ComboBox cmbSaleProduct;
    private System.Windows.Forms.Label lblSaleQuantity;
    private System.Windows.Forms.NumericUpDown numSaleQuantity;
    private System.Windows.Forms.Label lblSaleUnitPrice;
    private System.Windows.Forms.NumericUpDown numSaleUnitPrice;
    private System.Windows.Forms.Label lblSaleTotal;
    private System.Windows.Forms.Label lblSaleTotalValue;
    private System.Windows.Forms.Button btnSell;
    private System.Windows.Forms.Button btnDeleteSale;
    private System.Windows.Forms.Label lblBuyerName;
    private System.Windows.Forms.TextBox txtBuyerName;
    private System.Windows.Forms.Button btnPrintInvoice;
    private System.Windows.Forms.DataGridView dgvSales;
    private System.Windows.Forms.Panel panelSalePagination;
    private System.Windows.Forms.Button btnSalePrev;
    private System.Windows.Forms.Button btnSaleNext;
    private System.Windows.Forms.Label lblSalePage;
    // Sales filter panel
    private System.Windows.Forms.Panel panelSaleFilter;
    private System.Windows.Forms.Label lblFilterBuyer;
    private System.Windows.Forms.ComboBox cmbFilterBuyer;
    private System.Windows.Forms.Label lblFilterProduct;
    private System.Windows.Forms.ComboBox cmbFilterProduct;
    private System.Windows.Forms.CheckBox chkFilterDate;
    private System.Windows.Forms.DateTimePicker dtpFilterDate;
    private System.Windows.Forms.Button btnClearSaleFilter;
    // Stats filter panel
    private System.Windows.Forms.Panel panelStatsFilter;
    private System.Windows.Forms.Label lblStatsFilterProduct;
    private System.Windows.Forms.ComboBox cmbStatsProduct;
    private System.Windows.Forms.Label lblStatsFilterPeriod;
    private System.Windows.Forms.ComboBox cmbStatsPeriod;
    private System.Windows.Forms.DateTimePicker dtpStatsDate;
    private System.Windows.Forms.Button btnClearStatsFilter;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabProducts = new System.Windows.Forms.TabPage();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.chkShowBuyPrice = new System.Windows.Forms.CheckBox();
            this.lblBuyPrice = new System.Windows.Forms.Label();
            this.numBuyPrice = new System.Windows.Forms.NumericUpDown();
            this.lblSellPrice = new System.Windows.Forms.Label();
            this.numSellPrice = new System.Windows.Forms.NumericUpDown();
            this.dgvStats = new System.Windows.Forms.DataGridView();
            this.lblTotalProfit = new System.Windows.Forms.Label();
            this.lblTotalProfitValue = new System.Windows.Forms.Label();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.numQuantity = new System.Windows.Forms.NumericUpDown();
            this.lblUnit = new System.Windows.Forms.Label();
            this.cmbUnit = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.dgvProducts = new System.Windows.Forms.DataGridView();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.panelProductPagination = new System.Windows.Forms.Panel();
            this.btnProductPrev = new System.Windows.Forms.Button();
            this.btnProductNext = new System.Windows.Forms.Button();
            this.lblProductPage = new System.Windows.Forms.Label();
            this.tabSales = new System.Windows.Forms.TabPage();
            this.tabStats = new System.Windows.Forms.TabPage();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.panelSaleLeft = new System.Windows.Forms.Panel();
            this.lblSaleProduct = new System.Windows.Forms.Label();
            this.cmbSaleProduct = new System.Windows.Forms.ComboBox();
            this.lblSaleQuantity = new System.Windows.Forms.Label();
            this.numSaleQuantity = new System.Windows.Forms.NumericUpDown();
            this.lblSaleUnitPrice = new System.Windows.Forms.Label();
            this.numSaleUnitPrice = new System.Windows.Forms.NumericUpDown();
            this.lblSaleTotal = new System.Windows.Forms.Label();
            this.lblSaleTotalValue = new System.Windows.Forms.Label();
            this.lblBuyerName = new System.Windows.Forms.Label();
            this.txtBuyerName = new System.Windows.Forms.TextBox();
            this.btnSell = new System.Windows.Forms.Button();
            this.btnDeleteSale = new System.Windows.Forms.Button();
            this.btnPrintInvoice = new System.Windows.Forms.Button();
            this.dgvSales = new System.Windows.Forms.DataGridView();
            this.panelSalePagination = new System.Windows.Forms.Panel();
            this.btnSalePrev = new System.Windows.Forms.Button();
            this.btnSaleNext = new System.Windows.Forms.Button();
            this.lblSalePage = new System.Windows.Forms.Label();
            this.panelSaleFilter = new System.Windows.Forms.Panel();
            this.lblFilterBuyer = new System.Windows.Forms.Label();
            this.cmbFilterBuyer = new System.Windows.Forms.ComboBox();
            this.lblFilterProduct = new System.Windows.Forms.Label();
            this.cmbFilterProduct = new System.Windows.Forms.ComboBox();
            this.chkFilterDate = new System.Windows.Forms.CheckBox();
            this.dtpFilterDate = new System.Windows.Forms.DateTimePicker();
            this.btnClearSaleFilter = new System.Windows.Forms.Button();
            this.lblStatsFilterProduct = new System.Windows.Forms.Label();
            this.cmbStatsProduct = new System.Windows.Forms.ComboBox();
            this.lblStatsFilterPeriod = new System.Windows.Forms.Label();
            this.cmbStatsPeriod = new System.Windows.Forms.ComboBox();
            this.dtpStatsDate = new System.Windows.Forms.DateTimePicker();
            this.btnClearStatsFilter = new System.Windows.Forms.Button();
            this.panelStatsFilter = new System.Windows.Forms.Panel();
            SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(800, 450);
            this.tabControlMain.TabIndex = 0;
            this.tabControlMain.TabPages.AddRange(new System.Windows.Forms.TabPage[] {
            this.tabProducts,
            this.tabSales,
            this.tabStats,
            this.tabSettings});
            // 
            // tabProducts
            // 
            this.tabProducts.Location = new System.Drawing.Point(4, 24);
            this.tabProducts.Name = "tabProducts";
            this.tabProducts.Padding = new System.Windows.Forms.Padding(3);
            this.tabProducts.Size = new System.Drawing.Size(792, 422);
            this.tabProducts.TabIndex = 0;
            this.tabProducts.Text = "Products";
            this.tabProducts.UseVisualStyleBackColor = true;
            // 
            // panelLeft
            // 
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Width = 300;
            this.panelLeft.Padding = new System.Windows.Forms.Padding(10);
            // add controls to panel
            this.panelLeft.Controls.Add(this.lblName);
            this.panelLeft.Controls.Add(this.txtName);
            this.panelLeft.Controls.Add(this.chkShowBuyPrice);
            this.panelLeft.Controls.Add(this.lblBuyPrice);
            this.panelLeft.Controls.Add(this.numBuyPrice);
            this.panelLeft.Controls.Add(this.lblSellPrice);
            this.panelLeft.Controls.Add(this.numSellPrice);
            this.panelLeft.Controls.Add(this.lblQuantity);
            this.panelLeft.Controls.Add(this.numQuantity);
            this.panelLeft.Controls.Add(this.lblUnit);
            this.panelLeft.Controls.Add(this.cmbUnit);
            this.panelLeft.Controls.Add(this.btnAdd);
            this.panelLeft.Controls.Add(this.btnUpdate);
            this.panelLeft.Controls.Add(this.btnDelete);
            this.panelLeft.Controls.Add(this.btnClear);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(13, 15);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(39, 15);
            this.lblName.Text = "Name";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(13, 35);
            this.txtName.Name = "txtName";
            this.txtName.Width = 260;
            // 
            // chkShowBuyPrice
            // 
            this.chkShowBuyPrice.AutoSize = true;
            this.chkShowBuyPrice.Location = new System.Drawing.Point(13, 70);
            this.chkShowBuyPrice.Name = "chkShowBuyPrice";
            this.chkShowBuyPrice.Text = "Show Buy Price";
            this.chkShowBuyPrice.Checked = false;
            // 
            // lblBuyPrice
            // 
            this.lblBuyPrice.AutoSize = true;
            this.lblBuyPrice.Location = new System.Drawing.Point(145, 100);
            this.lblBuyPrice.Visible = false;
            this.lblBuyPrice.Name = "lblBuyPrice";
            this.lblBuyPrice.Text = "Buy Price";
            // 
            // numBuyPrice
            // 
            this.numBuyPrice.DecimalPlaces = 2;
            this.numBuyPrice.Location = new System.Drawing.Point(145, 118);
            this.numBuyPrice.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.numBuyPrice.Name = "numBuyPrice";
            this.numBuyPrice.Width = 120;
            this.numBuyPrice.Visible = false;
            // 
            // lblSellPrice
            // 
            this.lblSellPrice.AutoSize = true;
            this.lblSellPrice.Location = new System.Drawing.Point(13, 100);
            this.lblSellPrice.Name = "lblSellPrice";
            this.lblSellPrice.Text = "Sell Price";
            // 
            // numSellPrice
            // 
            this.numSellPrice.DecimalPlaces = 2;
            this.numSellPrice.Location = new System.Drawing.Point(13, 118);
            this.numSellPrice.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.numSellPrice.Name = "numSellPrice";
            this.numSellPrice.Width = 120;
            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new System.Drawing.Point(13, 158);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(54, 15);
            this.lblQuantity.Text = "Quantity";
            // 
            // numQuantity
            // 
            this.numQuantity.Location = new System.Drawing.Point(13, 176);
            this.numQuantity.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.numQuantity.Name = "numQuantity";
            this.numQuantity.Width = 120;
            // 
            // lblUnit
            // 
            this.lblUnit.AutoSize = true;
            this.lblUnit.Location = new System.Drawing.Point(13, 216);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(27, 15);
            this.lblUnit.Text = "Unit";
            // 
            // cmbUnit
            // 
            this.cmbUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUnit.Location = new System.Drawing.Point(13, 234);
            this.cmbUnit.Name = "cmbUnit";
            this.cmbUnit.Width = 120;
            this.cmbUnit.Items.AddRange(new object[] { "Bottle", "Kg", "Piece" });
            this.cmbUnit.SelectedIndex = 2;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(13, 275);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 25);
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(98, 275);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 25);
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(183, 275);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 25);
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(13, 310);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 25);
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // dgvProducts
            // 
            this.dgvProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProducts.Location = new System.Drawing.Point(303, 3);
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.ReadOnly = true;
            this.dgvProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProducts.MultiSelect = false;
            this.dgvProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.AllowUserToDeleteRows = false;
            this.dgvProducts.RowHeadersVisible = false;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(6, 10);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Text = "Search:";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(60, 7);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Width = 250;
            this.txtSearch.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Top;
            // 
            // panelSearch
            // 
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Height = 35;
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Controls.Add(this.lblSearch);
            this.panelSearch.Controls.Add(this.txtSearch);
            // 
            // btnProductPrev
            // 
            this.btnProductPrev.Text = "< Prev";
            this.btnProductPrev.Name = "btnProductPrev";
            this.btnProductPrev.Size = new System.Drawing.Size(70, 25);
            this.btnProductPrev.Location = new System.Drawing.Point(5, 5);
            // 
            // lblProductPage
            // 
            this.lblProductPage.Name = "lblProductPage";
            this.lblProductPage.Text = "Page 1";
            this.lblProductPage.AutoSize = true;
            this.lblProductPage.Location = new System.Drawing.Point(85, 9);
            this.lblProductPage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnProductNext
            // 
            this.btnProductNext.Text = "Next >";
            this.btnProductNext.Name = "btnProductNext";
            this.btnProductNext.Size = new System.Drawing.Size(70, 25);
            this.btnProductNext.Location = new System.Drawing.Point(155, 5);
            // 
            // panelProductPagination
            // 
            this.panelProductPagination.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelProductPagination.Height = 35;
            this.panelProductPagination.Name = "panelProductPagination";
            this.panelProductPagination.Controls.Add(this.btnProductPrev);
            this.panelProductPagination.Controls.Add(this.lblProductPage);
            this.panelProductPagination.Controls.Add(this.btnProductNext);
            // add controls to tabProducts
            this.tabProducts.Controls.Add(this.dgvProducts);
            this.tabProducts.Controls.Add(this.panelProductPagination);
            this.tabProducts.Controls.Add(this.panelSearch);
            this.tabProducts.Controls.Add(this.panelLeft);
            // 
            // panelSaleLeft
            // 
            this.panelSaleLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSaleLeft.Width = 300;
            this.panelSaleLeft.Padding = new System.Windows.Forms.Padding(10);
            this.panelSaleLeft.Controls.Add(this.lblSaleProduct);
            this.panelSaleLeft.Controls.Add(this.cmbSaleProduct);
            this.panelSaleLeft.Controls.Add(this.lblSaleQuantity);
            this.panelSaleLeft.Controls.Add(this.numSaleQuantity);
            this.panelSaleLeft.Controls.Add(this.lblSaleUnitPrice);
            this.panelSaleLeft.Controls.Add(this.numSaleUnitPrice);
            this.panelSaleLeft.Controls.Add(this.lblSaleTotal);
            this.panelSaleLeft.Controls.Add(this.lblSaleTotalValue);
            this.panelSaleLeft.Controls.Add(this.lblBuyerName);
            this.panelSaleLeft.Controls.Add(this.txtBuyerName);
            this.panelSaleLeft.Controls.Add(this.btnSell);
            this.panelSaleLeft.Controls.Add(this.btnDeleteSale);
            this.panelSaleLeft.Controls.Add(this.btnPrintInvoice);
            // 
            // lblBuyerName
            // 
            this.lblBuyerName.AutoSize = true;
            this.lblBuyerName.Location = new System.Drawing.Point(13, 235);
            this.lblBuyerName.Name = "lblBuyerName";
            this.lblBuyerName.Text = "Buyer Name (optional)";
            // 
            // txtBuyerName
            // 
            this.txtBuyerName.Location = new System.Drawing.Point(13, 255);
            this.txtBuyerName.Name = "txtBuyerName";
            this.txtBuyerName.Width = 260;
            // 
            // btnSell
            // 
            this.btnSell.Location = new System.Drawing.Point(13, 290);
            this.btnSell.Name = "btnSell";
            this.btnSell.Size = new System.Drawing.Size(75, 25);
            this.btnSell.Text = "Sell";
            this.btnSell.UseVisualStyleBackColor = true;
            // 
            // btnDeleteSale
            // 
            this.btnDeleteSale.Location = new System.Drawing.Point(98, 290);
            this.btnDeleteSale.Name = "btnDeleteSale";
            this.btnDeleteSale.Size = new System.Drawing.Size(90, 25);
            this.btnDeleteSale.Text = "Delete Sale";
            this.btnDeleteSale.UseVisualStyleBackColor = true;
            // 
            // btnPrintInvoice
            // 
            this.btnPrintInvoice.Location = new System.Drawing.Point(13, 325);
            this.btnPrintInvoice.Name = "btnPrintInvoice";
            this.btnPrintInvoice.Size = new System.Drawing.Size(120, 25);
            this.btnPrintInvoice.Text = "🖨 Print Invoice";
            this.btnPrintInvoice.UseVisualStyleBackColor = true;
            // 
            // dgvSales
            // 
            this.dgvSales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSales.Location = new System.Drawing.Point(303, 3);
            this.dgvSales.Name = "dgvSales";
            this.dgvSales.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSales.MultiSelect = false;
            this.dgvSales.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSales.AllowUserToAddRows = false;
            this.dgvSales.AllowUserToDeleteRows = false;
            this.dgvSales.RowHeadersVisible = false;
            // 
            // btnSalePrev
            // 
            this.btnSalePrev.Text = "< Prev";
            this.btnSalePrev.Name = "btnSalePrev";
            this.btnSalePrev.Size = new System.Drawing.Size(70, 25);
            this.btnSalePrev.Location = new System.Drawing.Point(5, 5);
            // 
            // lblSalePage
            // 
            this.lblSalePage.Name = "lblSalePage";
            this.lblSalePage.Text = "Page 1";
            this.lblSalePage.AutoSize = true;
            this.lblSalePage.Location = new System.Drawing.Point(85, 9);
            this.lblSalePage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSaleNext
            // 
            this.btnSaleNext.Text = "Next >";
            this.btnSaleNext.Name = "btnSaleNext";
            this.btnSaleNext.Size = new System.Drawing.Size(70, 25);
            this.btnSaleNext.Location = new System.Drawing.Point(155, 5);
            // 
            // panelSalePagination
            // 
            this.panelSalePagination.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelSalePagination.Height = 35;
            this.panelSalePagination.Name = "panelSalePagination";
            this.panelSalePagination.Controls.Add(this.btnSalePrev);
            this.panelSalePagination.Controls.Add(this.lblSalePage);
            this.panelSalePagination.Controls.Add(this.btnSaleNext);
            // 
            // panelSaleFilter
            // 
            this.panelSaleFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSaleFilter.Height = 45;
            this.panelSaleFilter.Name = "panelSaleFilter";
            this.panelSaleFilter.Padding = new System.Windows.Forms.Padding(5, 5, 5, 5);
            // lblFilterBuyer
            this.lblFilterBuyer.AutoSize = true;
            this.lblFilterBuyer.Location = new System.Drawing.Point(8, 14);
            this.lblFilterBuyer.Name = "lblFilterBuyer";
            this.lblFilterBuyer.Text = "Buyer:";
            // cmbFilterBuyer
            this.cmbFilterBuyer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cmbFilterBuyer.Location = new System.Drawing.Point(58, 10);
            this.cmbFilterBuyer.Name = "cmbFilterBuyer";
            this.cmbFilterBuyer.Width = 150;
            this.cmbFilterBuyer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbFilterBuyer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            // lblFilterProduct
            this.lblFilterProduct.AutoSize = true;
            this.lblFilterProduct.Location = new System.Drawing.Point(218, 14);
            this.lblFilterProduct.Name = "lblFilterProduct";
            this.lblFilterProduct.Text = "Product:";
            // cmbFilterProduct
            this.cmbFilterProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterProduct.Location = new System.Drawing.Point(278, 10);
            this.cmbFilterProduct.Name = "cmbFilterProduct";
            this.cmbFilterProduct.Width = 160;
            // chkFilterDate
            this.chkFilterDate.AutoSize = true;
            this.chkFilterDate.Location = new System.Drawing.Point(448, 12);
            this.chkFilterDate.Name = "chkFilterDate";
            this.chkFilterDate.Text = "Date:";
            // dtpFilterDate
            this.dtpFilterDate.Location = new System.Drawing.Point(503, 10);
            this.dtpFilterDate.Name = "dtpFilterDate";
            this.dtpFilterDate.Width = 130;
            this.dtpFilterDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFilterDate.Enabled = false;
            // btnClearSaleFilter
            this.btnClearSaleFilter.Location = new System.Drawing.Point(643, 10);
            this.btnClearSaleFilter.Name = "btnClearSaleFilter";
            this.btnClearSaleFilter.Size = new System.Drawing.Size(100, 25);
            this.btnClearSaleFilter.Text = "Clear Filters";
            this.btnClearSaleFilter.UseVisualStyleBackColor = true;
            this.panelSaleFilter.Controls.Add(this.lblFilterBuyer);
            this.panelSaleFilter.Controls.Add(this.cmbFilterBuyer);
            this.panelSaleFilter.Controls.Add(this.lblFilterProduct);
            this.panelSaleFilter.Controls.Add(this.cmbFilterProduct);
            this.panelSaleFilter.Controls.Add(this.chkFilterDate);
            this.panelSaleFilter.Controls.Add(this.dtpFilterDate);
            this.panelSaleFilter.Controls.Add(this.btnClearSaleFilter);
            // add controls to tabSales
            this.tabSales.Controls.Add(this.dgvSales);
            this.tabSales.Controls.Add(this.panelSalePagination);
            this.tabSales.Controls.Add(this.panelSaleLeft);
            this.tabSales.Controls.Add(this.panelSaleFilter);
            this.tabSales.Location = new System.Drawing.Point(4, 24);
            this.tabSales.Name = "tabSales";
            this.tabSales.Padding = new System.Windows.Forms.Padding(3);
            this.tabSales.Size = new System.Drawing.Size(792, 422);
            this.tabSales.TabIndex = 1;
            this.tabSales.Text = "Sales";
            this.tabSales.UseVisualStyleBackColor = true;
            // dgvStats
            this.dgvStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStats.Name = "dgvStats";
            this.dgvStats.ReadOnly = true;
            this.dgvStats.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStats.MultiSelect = false;
            this.dgvStats.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvStats.AllowUserToAddRows = false;
            this.dgvStats.AllowUserToDeleteRows = false;
            this.dgvStats.RowHeadersVisible = false;
            // lblTotalProfit
            this.lblTotalProfit.AutoSize = true;
            this.lblTotalProfit.Name = "lblTotalProfit";
            this.lblTotalProfit.Text = "Total Profit:";
            this.lblTotalProfit.Font = new System.Drawing.Font(this.lblTotalProfit.Font, System.Drawing.FontStyle.Bold);
            this.lblTotalProfit.Dock = System.Windows.Forms.DockStyle.Left;
            // lblTotalProfitValue
            this.lblTotalProfitValue.AutoSize = true;
            this.lblTotalProfitValue.Name = "lblTotalProfitValue";
            this.lblTotalProfitValue.Text = "0.00";
            this.lblTotalProfitValue.Font = new System.Drawing.Font(this.lblTotalProfitValue.Font, System.Drawing.FontStyle.Bold);
            this.lblTotalProfitValue.Dock = System.Windows.Forms.DockStyle.Fill;
            var panelStatsBottom = new System.Windows.Forms.Panel();
            panelStatsBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            panelStatsBottom.Height = 35;
            panelStatsBottom.Padding = new System.Windows.Forms.Padding(10, 8, 0, 0);
            panelStatsBottom.Controls.Add(this.lblTotalProfitValue);
            panelStatsBottom.Controls.Add(this.lblTotalProfit);
            // Stats filter controls
            this.lblStatsFilterProduct.Text = "Product:";
            this.lblStatsFilterProduct.AutoSize = true;
            this.lblStatsFilterProduct.Location = new System.Drawing.Point(5, 10);

            this.cmbStatsProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatsProduct.Location = new System.Drawing.Point(60, 7);
            this.cmbStatsProduct.Width = 150;
            this.cmbStatsProduct.SelectedIndexChanged += new System.EventHandler(this.StatsFilter_Changed);

            this.lblStatsFilterPeriod.Text = "Period:";
            this.lblStatsFilterPeriod.AutoSize = true;
            this.lblStatsFilterPeriod.Location = new System.Drawing.Point(220, 10);

            this.cmbStatsPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatsPeriod.Location = new System.Drawing.Point(268, 7);
            this.cmbStatsPeriod.Width = 90;
            this.cmbStatsPeriod.Items.AddRange(new object[] { "", "Day", "Week", "Month", "Year" });
            this.cmbStatsPeriod.SelectedIndex = 0;
            this.cmbStatsPeriod.SelectedIndexChanged += new System.EventHandler(this.StatsFilter_Changed);

            this.dtpStatsDate.Location = new System.Drawing.Point(368, 6);
            this.dtpStatsDate.Width = 130;
            this.dtpStatsDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStatsDate.ValueChanged += new System.EventHandler(this.StatsFilter_Changed);

            this.btnClearStatsFilter.Text = "Clear";
            this.btnClearStatsFilter.Location = new System.Drawing.Point(508, 5);
            this.btnClearStatsFilter.Width = 60;
            this.btnClearStatsFilter.Click += new System.EventHandler(this.BtnClearStatsFilter_Click);

            this.panelStatsFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelStatsFilter.Height = 35;
            this.panelStatsFilter.Controls.Add(this.lblStatsFilterProduct);
            this.panelStatsFilter.Controls.Add(this.cmbStatsProduct);
            this.panelStatsFilter.Controls.Add(this.lblStatsFilterPeriod);
            this.panelStatsFilter.Controls.Add(this.cmbStatsPeriod);
            this.panelStatsFilter.Controls.Add(this.dtpStatsDate);
            this.panelStatsFilter.Controls.Add(this.btnClearStatsFilter);

            // tabStats
            this.tabStats.Controls.Add(this.dgvStats);
            this.tabStats.Controls.Add(panelStatsBottom);
            this.tabStats.Controls.Add(this.panelStatsFilter);
            this.tabStats.Location = new System.Drawing.Point(4, 24);
            this.tabStats.Name = "tabStats";
            this.tabStats.Size = new System.Drawing.Size(792, 422);
            this.tabStats.TabIndex = 2;
            this.tabStats.Text = "Stats";
            this.tabStats.UseVisualStyleBackColor = true;
            // tabSettings
            this.tabSettings.Location = new System.Drawing.Point(4, 24);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Size = new System.Drawing.Size(792, 422);
            this.tabSettings.TabIndex = 3;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // lblSaleProduct
            this.lblSaleProduct.AutoSize = true;
            this.lblSaleProduct.Location = new System.Drawing.Point(13, 15);
            this.lblSaleProduct.Name = "lblSaleProduct";
            this.lblSaleProduct.Text = "Product";
            // cmbSaleProduct
            this.cmbSaleProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSaleProduct.Location = new System.Drawing.Point(13, 35);
            this.cmbSaleProduct.Name = "cmbSaleProduct";
            this.cmbSaleProduct.Width = 260;
            // lblSaleQuantity
            this.lblSaleQuantity.AutoSize = true;
            this.lblSaleQuantity.Location = new System.Drawing.Point(13, 75);
            this.lblSaleQuantity.Name = "lblSaleQuantity";
            this.lblSaleQuantity.Text = "Quantity";
            // numSaleQuantity
            this.numSaleQuantity.Location = new System.Drawing.Point(13, 95);
            this.numSaleQuantity.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numSaleQuantity.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.numSaleQuantity.Name = "numSaleQuantity";
            this.numSaleQuantity.Width = 120;
            // lblSaleUnitPrice
            this.lblSaleUnitPrice.AutoSize = true;
            this.lblSaleUnitPrice.Location = new System.Drawing.Point(13, 135);
            this.lblSaleUnitPrice.Name = "lblSaleUnitPrice";
            this.lblSaleUnitPrice.Text = "Unit Price";
            // numSaleUnitPrice
            this.numSaleUnitPrice.DecimalPlaces = 2;
            this.numSaleUnitPrice.Location = new System.Drawing.Point(13, 155);
            this.numSaleUnitPrice.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.numSaleUnitPrice.Name = "numSaleUnitPrice";
            this.numSaleUnitPrice.Width = 120;
            // lblSaleTotal
            this.lblSaleTotal.AutoSize = true;
            this.lblSaleTotal.Location = new System.Drawing.Point(13, 195);
            this.lblSaleTotal.Name = "lblSaleTotal";
            this.lblSaleTotal.Text = "Total:";
            // lblSaleTotalValue
            this.lblSaleTotalValue.AutoSize = true;
            this.lblSaleTotalValue.Location = new System.Drawing.Point(60, 195);
            this.lblSaleTotalValue.Name = "lblSaleTotalValue";
            this.lblSaleTotalValue.Text = "0.00";
            // 
            // Main
            // 
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.tabControlMain);
            this.Name = "Main";
            this.Text = "KabyliaTaste";
            this.ResumeLayout(false);

        }

        #endregion
    }
}
