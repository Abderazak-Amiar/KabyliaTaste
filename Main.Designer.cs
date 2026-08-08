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
    private System.Windows.Forms.TabPage tabExpenses;
    private System.Windows.Forms.TabPage tabInvoices;
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
    private System.Windows.Forms.Button btnUpdateSale;
    private System.Windows.Forms.Button btnClearSale;
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
    private System.Windows.Forms.Label lblStatsFilterBuyer;
    private System.Windows.Forms.ComboBox cmbStatsBuyer;
    private System.Windows.Forms.ComboBox cmbStatsProduct;
    private System.Windows.Forms.Label lblStatsFilterPeriod;
    private System.Windows.Forms.ComboBox cmbStatsPeriod;
    private System.Windows.Forms.DateTimePicker dtpStatsDate;
    private System.Windows.Forms.Button btnClearStatsFilter;
    private System.Windows.Forms.Button btnPrintReport;
    // Expenses tab
    private System.Windows.Forms.Panel panelExpenseLeft;
    private System.Windows.Forms.Label lblExpenseDescription;
    private System.Windows.Forms.TextBox txtExpenseDescription;
    private System.Windows.Forms.Label lblExpenseAmount;
    private System.Windows.Forms.NumericUpDown numExpenseAmount;
    private System.Windows.Forms.Label lblExpenseCategory;
    private System.Windows.Forms.TextBox txtExpenseCategory;
    private System.Windows.Forms.Label lblExpenseDate;
    private System.Windows.Forms.DateTimePicker dtpExpenseDate;
    private System.Windows.Forms.Button btnAddExpense;
    private System.Windows.Forms.Button btnUpdateExpense;
    private System.Windows.Forms.Button btnDeleteExpense;
    private System.Windows.Forms.Button btnClearExpense;
    private System.Windows.Forms.DataGridView dgvExpenses;
    private System.Windows.Forms.Panel panelExpensePagination;
    private System.Windows.Forms.Button btnExpensePrev;
    private System.Windows.Forms.Button btnExpenseNext;
    private System.Windows.Forms.Label lblExpensePage;
    private System.Windows.Forms.Panel panelExpenseFilter;
    private System.Windows.Forms.Label lblExpenseFilterCategory;
    private System.Windows.Forms.ComboBox cmbExpenseFilterCategory;
    private System.Windows.Forms.Button btnClearExpenseFilter;
    // Invoices tab
    private System.Windows.Forms.DataGridView dgvInvoices;
    private System.Windows.Forms.Panel panelInvoiceFilter;
    private System.Windows.Forms.Label lblInvoiceFilterBuyer;
    private System.Windows.Forms.ComboBox cmbInvoiceFilterBuyer;
    private System.Windows.Forms.Label lblInvoiceFilterStatus;
    private System.Windows.Forms.ComboBox cmbInvoiceFilterStatus;
    private System.Windows.Forms.Button btnClearInvoiceFilter;
    private System.Windows.Forms.Panel panelInvoicePagination;
    private System.Windows.Forms.Button btnInvoicePrev;
    private System.Windows.Forms.Button btnInvoiceNext;
     private System.Windows.Forms.Button btnInvoicePreview;
      private System.Windows.Forms.Button btnDeleteInvoice;
    private System.Windows.Forms.Label lblInvoicePage;
    // Settings tab inner controls
    private System.Windows.Forms.TabControl tabControlSettings;
    private System.Windows.Forms.TabPage tabProfile;
    private System.Windows.Forms.TabPage tabStore;
    private System.Windows.Forms.TabPage tabGoogleDrive;
    private System.Windows.Forms.TabPage tabBackup;
    private System.Windows.Forms.Label lblCurrentPassword;
    private System.Windows.Forms.TextBox txtCurrentPassword;
    private System.Windows.Forms.Label lblNewPassword;
    private System.Windows.Forms.TextBox txtNewPassword;
    private System.Windows.Forms.Label lblConfirmPassword;
    private System.Windows.Forms.TextBox txtConfirmPassword;
    private System.Windows.Forms.Button btnChangePassword;
    private System.Windows.Forms.Label lblStoreName;
    private System.Windows.Forms.TextBox txtStoreName;
    private System.Windows.Forms.Button btnSaveStoreName;
    private System.Windows.Forms.PictureBox picStoreLogo;
    private System.Windows.Forms.Button btnChangeLogo;
    private System.Windows.Forms.Label lblGoogleDriveClientId;
    private System.Windows.Forms.TextBox txtGoogleDriveClientId;
    private System.Windows.Forms.Label lblGoogleDriveClientSecret;
    private System.Windows.Forms.TextBox txtGoogleDriveClientSecret;
    private System.Windows.Forms.Label lblGoogleDriveFolderId;
    private System.Windows.Forms.TextBox txtGoogleDriveFolderId;
    private System.Windows.Forms.Label lblGoogleDriveRefreshToken;
    private System.Windows.Forms.TextBox txtGoogleDriveRefreshToken;
    private System.Windows.Forms.Button btnSaveGoogleDriveConfig;
     private System.Windows.Forms.Button btnGenerateGoogleDriveRefreshToken;
     private System.Windows.Forms.Button btnGoogleDriveHelp;
     private System.Windows.Forms.Button btnOpenGoogleCloudConsole;
    private System.Windows.Forms.Label lblBackupInfo;
    private System.Windows.Forms.Button btnDownloadDatabaseBackup;
    private System.Windows.Forms.Button btnRestoreDatabaseBackup;
     private System.Windows.Forms.Button btnUploadGoogleDriveBackup;
     private System.Windows.Forms.Button btnDownloadGoogleDriveBackup;
    private System.Windows.Forms.Button btnLogout;
    // Profile extra controls
    private System.Windows.Forms.Label lblUsernameProfile;
    private System.Windows.Forms.TextBox txtUsernameProfile;
    private System.Windows.Forms.Button btnChangeUsername;
    // Top-bar info controls
    private KabyliaTaste.Controls.MixedFontLabel lblGreeting;
    private System.Windows.Forms.Label lblDateTime;
    private System.Windows.Forms.Timer timerClock;

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
            this.tabExpenses = new System.Windows.Forms.TabPage();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.tabGoogleDrive = new System.Windows.Forms.TabPage();
            this.tabBackup = new System.Windows.Forms.TabPage();
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
            this.btnUpdateSale = new System.Windows.Forms.Button();
            this.btnClearSale = new System.Windows.Forms.Button();
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
            this.lblStatsFilterBuyer = new System.Windows.Forms.Label();
            this.cmbStatsBuyer = new System.Windows.Forms.ComboBox();
            this.cmbStatsPeriod = new System.Windows.Forms.ComboBox();
            this.dtpStatsDate = new System.Windows.Forms.DateTimePicker();
            this.btnClearStatsFilter = new System.Windows.Forms.Button();
            this.btnPrintReport = new System.Windows.Forms.Button();
            this.panelStatsFilter = new System.Windows.Forms.Panel();
            this.tabControlSettings = new System.Windows.Forms.TabControl();
            this.tabProfile = new System.Windows.Forms.TabPage();
            this.tabStore = new System.Windows.Forms.TabPage();
            this.lblCurrentPassword = new System.Windows.Forms.Label();
            this.txtCurrentPassword = new System.Windows.Forms.TextBox();
            this.lblNewPassword = new System.Windows.Forms.Label();
            this.txtNewPassword = new System.Windows.Forms.TextBox();
            this.lblConfirmPassword = new System.Windows.Forms.Label();
            this.txtConfirmPassword = new System.Windows.Forms.TextBox();
            this.btnChangePassword = new System.Windows.Forms.Button();
            this.lblStoreName = new System.Windows.Forms.Label();
            this.txtStoreName = new System.Windows.Forms.TextBox();
            this.btnSaveStoreName = new System.Windows.Forms.Button();
            this.picStoreLogo = new System.Windows.Forms.PictureBox();
            this.btnChangeLogo = new System.Windows.Forms.Button();
            this.lblGoogleDriveClientId = new System.Windows.Forms.Label();
            this.txtGoogleDriveClientId = new System.Windows.Forms.TextBox();
            this.lblGoogleDriveClientSecret = new System.Windows.Forms.Label();
            this.txtGoogleDriveClientSecret = new System.Windows.Forms.TextBox();
            this.lblGoogleDriveFolderId = new System.Windows.Forms.Label();
            this.txtGoogleDriveFolderId = new System.Windows.Forms.TextBox();
            this.lblGoogleDriveRefreshToken = new System.Windows.Forms.Label();
            this.txtGoogleDriveRefreshToken = new System.Windows.Forms.TextBox();
            this.btnSaveGoogleDriveConfig = new System.Windows.Forms.Button();
            this.btnGenerateGoogleDriveRefreshToken = new System.Windows.Forms.Button();
            this.btnGoogleDriveHelp = new System.Windows.Forms.Button();
            this.btnOpenGoogleCloudConsole = new System.Windows.Forms.Button();
            this.lblBackupInfo = new System.Windows.Forms.Label();
            this.btnDownloadDatabaseBackup = new System.Windows.Forms.Button();
            this.btnRestoreDatabaseBackup = new System.Windows.Forms.Button();
            this.btnUploadGoogleDriveBackup = new System.Windows.Forms.Button();
            this.btnDownloadGoogleDriveBackup = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.lblUsernameProfile = new System.Windows.Forms.Label();
            this.txtUsernameProfile = new System.Windows.Forms.TextBox();
            this.btnChangeUsername = new System.Windows.Forms.Button();
            this.lblGreeting = new KabyliaTaste.Controls.MixedFontLabel();
            this.lblDateTime = new System.Windows.Forms.Label();
            this.timerClock = new System.Windows.Forms.Timer();
            this.panelExpenseLeft = new System.Windows.Forms.Panel();
            this.lblExpenseDescription = new System.Windows.Forms.Label();
            this.txtExpenseDescription = new System.Windows.Forms.TextBox();
            this.lblExpenseAmount = new System.Windows.Forms.Label();
            this.numExpenseAmount = new System.Windows.Forms.NumericUpDown();
            this.lblExpenseCategory = new System.Windows.Forms.Label();
            this.txtExpenseCategory = new System.Windows.Forms.TextBox();
            this.lblExpenseDate = new System.Windows.Forms.Label();
            this.dtpExpenseDate = new System.Windows.Forms.DateTimePicker();
            this.btnAddExpense = new System.Windows.Forms.Button();
            this.btnUpdateExpense = new System.Windows.Forms.Button();
            this.btnDeleteExpense = new System.Windows.Forms.Button();
            this.btnClearExpense = new System.Windows.Forms.Button();
            this.dgvExpenses = new System.Windows.Forms.DataGridView();
            this.panelExpensePagination = new System.Windows.Forms.Panel();
            this.btnExpensePrev = new System.Windows.Forms.Button();
            this.btnExpenseNext = new System.Windows.Forms.Button();
            this.lblExpensePage = new System.Windows.Forms.Label();
            this.panelExpenseFilter = new System.Windows.Forms.Panel();
            this.lblExpenseFilterCategory = new System.Windows.Forms.Label();
            this.cmbExpenseFilterCategory = new System.Windows.Forms.ComboBox();
            this.btnClearExpenseFilter = new System.Windows.Forms.Button();
            this.tabInvoices = new System.Windows.Forms.TabPage();
            this.dgvInvoices = new System.Windows.Forms.DataGridView();
            this.panelInvoiceFilter = new System.Windows.Forms.Panel();
            this.lblInvoiceFilterBuyer = new System.Windows.Forms.Label();
            this.cmbInvoiceFilterBuyer = new System.Windows.Forms.ComboBox();
            this.lblInvoiceFilterStatus = new System.Windows.Forms.Label();
            this.cmbInvoiceFilterStatus = new System.Windows.Forms.ComboBox();
            this.btnClearInvoiceFilter = new System.Windows.Forms.Button();
            this.panelInvoicePagination = new System.Windows.Forms.Panel();
            this.btnInvoicePrev = new System.Windows.Forms.Button();
            this.btnInvoiceNext = new System.Windows.Forms.Button();
            this.btnInvoicePreview = new System.Windows.Forms.Button();
            this.btnDeleteInvoice = new System.Windows.Forms.Button();
            this.lblInvoicePage = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.None;
            this.tabControlMain.Location = new System.Drawing.Point(0, 40);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(1000, 560);
            this.tabControlMain.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.tabControlMain.TabIndex = 0;
            this.tabControlMain.TabPages.AddRange(new System.Windows.Forms.TabPage[] {
            this.tabProducts,
            this.tabSales,
            this.tabStats,
            this.tabExpenses,
            this.tabInvoices,
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
            this.lblSearch.Location = new System.Drawing.Point(12, 10);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Text = "Search:";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(110, 7);
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
            this.btnProductPrev.Size = new System.Drawing.Size(95, 25);
            this.btnProductPrev.Location = new System.Drawing.Point(5, 5);
            // 
            // lblProductPage
            // 
            this.lblProductPage.Name = "lblProductPage";
            this.lblProductPage.Text = "Page 1";
            this.lblProductPage.AutoSize = true;
            this.lblProductPage.Location = new System.Drawing.Point(110, 9);
            this.lblProductPage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnProductNext
            // 
            this.btnProductNext.Text = "Next >";
            this.btnProductNext.Name = "btnProductNext";
            this.btnProductNext.Size = new System.Drawing.Size(70, 25);
            this.btnProductNext.Location = new System.Drawing.Point(180, 5);
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
            this.panelSaleLeft.Controls.Add(this.btnUpdateSale);
            this.panelSaleLeft.Controls.Add(this.btnClearSale);
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
            // btnUpdateSale
            // 
            this.btnUpdateSale.Location = new System.Drawing.Point(98, 325);
            this.btnUpdateSale.Name = "btnUpdateSale";
            this.btnUpdateSale.Size = new System.Drawing.Size(100, 25);
            this.btnUpdateSale.Text = "Update Sale";
            this.btnUpdateSale.UseVisualStyleBackColor = true;
            // 
            // btnPrintInvoice
            // 
            this.btnPrintInvoice.Location = new System.Drawing.Point(13, 325);
            this.btnPrintInvoice.Name = "btnPrintInvoice";
            this.btnPrintInvoice.Size = new System.Drawing.Size(75, 25);
            this.btnPrintInvoice.Text = "🖨 Print Invoice";
            this.btnPrintInvoice.UseVisualStyleBackColor = true;
            // 
            // btnClearSale
            // 
            this.btnClearSale.Location = new System.Drawing.Point(13, 360);
            this.btnClearSale.Name = "btnClearSale";
            this.btnClearSale.Size = new System.Drawing.Size(75, 25);
            this.btnClearSale.Text = "Clear";
            this.btnClearSale.UseVisualStyleBackColor = true;
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
            this.btnSalePrev.Size = new System.Drawing.Size(95, 25);
            this.btnSalePrev.Location = new System.Drawing.Point(5, 5);
            // 
            // lblSalePage
            // 
            this.lblSalePage.Name = "lblSalePage";
            this.lblSalePage.Text = "Page 1";
            this.lblSalePage.AutoSize = true;
            this.lblSalePage.Location = new System.Drawing.Point(110, 9);
            this.lblSalePage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSaleNext
            // 
            this.btnSaleNext.Text = "Next >";
            this.btnSaleNext.Name = "btnSaleNext";
            this.btnSaleNext.Size = new System.Drawing.Size(70, 25);
            this.btnSaleNext.Location = new System.Drawing.Point(180, 5);
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

            this.dtpStatsDate.Location = new System.Drawing.Point(368, 7);
            this.dtpStatsDate.Width = 130;
            this.dtpStatsDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStatsDate.ValueChanged += new System.EventHandler(this.StatsFilter_Changed);

            this.lblStatsFilterBuyer.Text = "Client:";
            this.lblStatsFilterBuyer.AutoSize = true;
            this.lblStatsFilterBuyer.Location = new System.Drawing.Point(508, 10);

            this.cmbStatsBuyer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatsBuyer.Location = new System.Drawing.Point(553, 7);
            this.cmbStatsBuyer.Size = new System.Drawing.Size(130, 25);
            this.cmbStatsBuyer.SelectedIndexChanged += new System.EventHandler(this.StatsFilter_Changed);

            this.btnClearStatsFilter.Text = "Clear";
            this.btnClearStatsFilter.Location = new System.Drawing.Point(693, 6);
            this.btnClearStatsFilter.Size = new System.Drawing.Size(130, 25);
            this.btnClearStatsFilter.Click += new System.EventHandler(this.BtnClearStatsFilter_Click);

            this.btnPrintReport.Text = "🖨 Print Report";
            this.btnPrintReport.Location = new System.Drawing.Point(833, 6);
            this.btnPrintReport.Size = new System.Drawing.Size(120, 25);
            this.btnPrintReport.UseVisualStyleBackColor = true;

            this.panelStatsFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelStatsFilter.Height = 35;
            this.panelStatsFilter.Controls.Add(this.lblStatsFilterProduct);
            this.panelStatsFilter.Controls.Add(this.cmbStatsProduct);
            this.panelStatsFilter.Controls.Add(this.lblStatsFilterPeriod);
            this.panelStatsFilter.Controls.Add(this.cmbStatsPeriod);
            this.panelStatsFilter.Controls.Add(this.dtpStatsDate);
            this.panelStatsFilter.Controls.Add(this.lblStatsFilterBuyer);
            this.panelStatsFilter.Controls.Add(this.cmbStatsBuyer);
            this.panelStatsFilter.Controls.Add(this.btnClearStatsFilter);
            this.panelStatsFilter.Controls.Add(this.btnPrintReport);

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
            // ── tabExpenses ───────────────────────────────────────────────
            // panelExpenseLeft
            this.panelExpenseLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelExpenseLeft.Width = 300;
            this.panelExpenseLeft.Padding = new System.Windows.Forms.Padding(10);
            // lblExpenseDescription
            this.lblExpenseDescription.AutoSize = true;
            this.lblExpenseDescription.Location = new System.Drawing.Point(13, 15);
            this.lblExpenseDescription.Name = "lblExpenseDescription";
            this.lblExpenseDescription.Text = "Description";
            // txtExpenseDescription
            this.txtExpenseDescription.Location = new System.Drawing.Point(13, 35);
            this.txtExpenseDescription.Name = "txtExpenseDescription";
            this.txtExpenseDescription.Width = 260;
            // lblExpenseAmount
            this.lblExpenseAmount.AutoSize = true;
            this.lblExpenseAmount.Location = new System.Drawing.Point(13, 75);
            this.lblExpenseAmount.Name = "lblExpenseAmount";
            this.lblExpenseAmount.Text = "Amount";
            // numExpenseAmount
            this.numExpenseAmount.DecimalPlaces = 2;
            this.numExpenseAmount.Location = new System.Drawing.Point(13, 95);
            this.numExpenseAmount.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            this.numExpenseAmount.Name = "numExpenseAmount";
            this.numExpenseAmount.Width = 120;
            // lblExpenseCategory
            this.lblExpenseCategory.AutoSize = true;
            this.lblExpenseCategory.Location = new System.Drawing.Point(13, 135);
            this.lblExpenseCategory.Name = "lblExpenseCategory";
            this.lblExpenseCategory.Text = "Category";
            // txtExpenseCategory
            this.txtExpenseCategory.Location = new System.Drawing.Point(13, 155);
            this.txtExpenseCategory.Name = "txtExpenseCategory";
            this.txtExpenseCategory.Width = 260;
            // lblExpenseDate
            this.lblExpenseDate.AutoSize = true;
            this.lblExpenseDate.Location = new System.Drawing.Point(13, 195);
            this.lblExpenseDate.Name = "lblExpenseDate";
            this.lblExpenseDate.Text = "Date";
            // dtpExpenseDate
            this.dtpExpenseDate.Location = new System.Drawing.Point(13, 215);
            this.dtpExpenseDate.Name = "dtpExpenseDate";
            this.dtpExpenseDate.Width = 200;
            this.dtpExpenseDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            // btnAddExpense
            this.btnAddExpense.Location = new System.Drawing.Point(13, 255);
            this.btnAddExpense.Name = "btnAddExpense";
            this.btnAddExpense.Size = new System.Drawing.Size(75, 25);
            this.btnAddExpense.Text = "Add";
            this.btnAddExpense.UseVisualStyleBackColor = true;
            // btnUpdateExpense
            this.btnUpdateExpense.Location = new System.Drawing.Point(98, 255);
            this.btnUpdateExpense.Name = "btnUpdateExpense";
            this.btnUpdateExpense.Size = new System.Drawing.Size(75, 25);
            this.btnUpdateExpense.Text = "Update";
            this.btnUpdateExpense.UseVisualStyleBackColor = true;
            // btnDeleteExpense
            this.btnDeleteExpense.Location = new System.Drawing.Point(183, 255);
            this.btnDeleteExpense.Name = "btnDeleteExpense";
            this.btnDeleteExpense.Size = new System.Drawing.Size(75, 25);
            this.btnDeleteExpense.Text = "Delete";
            this.btnDeleteExpense.UseVisualStyleBackColor = true;
            // btnClearExpense
            this.btnClearExpense.Location = new System.Drawing.Point(13, 290);
            this.btnClearExpense.Name = "btnClearExpense";
            this.btnClearExpense.Size = new System.Drawing.Size(75, 25);
            this.btnClearExpense.Text = "Clear";
            this.btnClearExpense.UseVisualStyleBackColor = true;
            this.panelExpenseLeft.Controls.Add(this.lblExpenseDescription);
            this.panelExpenseLeft.Controls.Add(this.txtExpenseDescription);
            this.panelExpenseLeft.Controls.Add(this.lblExpenseAmount);
            this.panelExpenseLeft.Controls.Add(this.numExpenseAmount);
            this.panelExpenseLeft.Controls.Add(this.lblExpenseCategory);
            this.panelExpenseLeft.Controls.Add(this.txtExpenseCategory);
            this.panelExpenseLeft.Controls.Add(this.lblExpenseDate);
            this.panelExpenseLeft.Controls.Add(this.dtpExpenseDate);
            this.panelExpenseLeft.Controls.Add(this.btnAddExpense);
            this.panelExpenseLeft.Controls.Add(this.btnUpdateExpense);
            this.panelExpenseLeft.Controls.Add(this.btnDeleteExpense);
            this.panelExpenseLeft.Controls.Add(this.btnClearExpense);
            // dgvExpenses
            this.dgvExpenses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvExpenses.Name = "dgvExpenses";
            this.dgvExpenses.ReadOnly = true;
            this.dgvExpenses.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvExpenses.MultiSelect = false;
            this.dgvExpenses.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvExpenses.AllowUserToAddRows = false;
            this.dgvExpenses.AllowUserToDeleteRows = false;
            this.dgvExpenses.RowHeadersVisible = false;
            // btnExpensePrev
            this.btnExpensePrev.Text = "< Prev";
            this.btnExpensePrev.Name = "btnExpensePrev";
            this.btnExpensePrev.Size = new System.Drawing.Size(95, 25);
            this.btnExpensePrev.Location = new System.Drawing.Point(5, 5);
            // lblExpensePage
            this.lblExpensePage.Name = "lblExpensePage";
            this.lblExpensePage.Text = "Page 1";
            this.lblExpensePage.AutoSize = true;
            this.lblExpensePage.Location = new System.Drawing.Point(110, 9);
            this.lblExpensePage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // btnExpenseNext
            this.btnExpenseNext.Text = "Next >";
            this.btnExpenseNext.Name = "btnExpenseNext";
            this.btnExpenseNext.Size = new System.Drawing.Size(70, 25);
            this.btnExpenseNext.Location = new System.Drawing.Point(180, 5);
            // panelExpensePagination
            this.panelExpensePagination.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelExpensePagination.Height = 35;
            this.panelExpensePagination.Name = "panelExpensePagination";
            this.panelExpensePagination.Controls.Add(this.btnExpensePrev);
            this.panelExpensePagination.Controls.Add(this.lblExpensePage);
            this.panelExpensePagination.Controls.Add(this.btnExpenseNext);
            // panelExpenseFilter
            this.lblExpenseFilterCategory.AutoSize = true;
            this.lblExpenseFilterCategory.Location = new System.Drawing.Point(8, 13);
            this.lblExpenseFilterCategory.Name = "lblExpenseFilterCategory";
            this.lblExpenseFilterCategory.Text = "Category:";
            this.cmbExpenseFilterCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExpenseFilterCategory.Location = new System.Drawing.Point(70, 10);
            this.cmbExpenseFilterCategory.Name = "cmbExpenseFilterCategory";
            this.cmbExpenseFilterCategory.Width = 160;
            this.btnClearExpenseFilter.Text = "Clear";
            this.btnClearExpenseFilter.Name = "btnClearExpenseFilter";
            this.btnClearExpenseFilter.Location = new System.Drawing.Point(240, 9);
            this.btnClearExpenseFilter.Size = new System.Drawing.Size(130, 25);
            this.btnClearExpenseFilter.UseVisualStyleBackColor = true;
            this.panelExpenseFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelExpenseFilter.Height = 35;
            this.panelExpenseFilter.Name = "panelExpenseFilter";
            this.panelExpenseFilter.Controls.Add(this.lblExpenseFilterCategory);
            this.panelExpenseFilter.Controls.Add(this.cmbExpenseFilterCategory);
            this.panelExpenseFilter.Controls.Add(this.btnClearExpenseFilter);
            // tabExpenses
            this.tabExpenses.Controls.Add(this.dgvExpenses);
            this.tabExpenses.Controls.Add(this.panelExpensePagination);
            this.tabExpenses.Controls.Add(this.panelExpenseFilter);
            this.tabExpenses.Controls.Add(this.panelExpenseLeft);
            this.tabExpenses.Location = new System.Drawing.Point(4, 24);
            this.tabExpenses.Name = "tabExpenses";
            this.tabExpenses.Size = new System.Drawing.Size(792, 422);
            this.tabExpenses.TabIndex = 4;
            this.tabExpenses.Text = "Expenses";
            this.tabExpenses.UseVisualStyleBackColor = true;
            // ── tabInvoices ───────────────────────────────────────────────
            // dgvInvoices
            this.dgvInvoices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvInvoices.Name = "dgvInvoices";
            this.dgvInvoices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvInvoices.MultiSelect = false;
            this.dgvInvoices.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvInvoices.AllowUserToAddRows = false;
            this.dgvInvoices.AllowUserToDeleteRows = false;
            this.dgvInvoices.RowHeadersVisible = false;
            // lblInvoiceFilterBuyer
            this.lblInvoiceFilterBuyer.AutoSize = true;
            this.lblInvoiceFilterBuyer.Location = new System.Drawing.Point(8, 13);
            this.lblInvoiceFilterBuyer.Name = "lblInvoiceFilterBuyer";
            this.lblInvoiceFilterBuyer.Text = "Buyer:";
            // cmbInvoiceFilterBuyer
            this.cmbInvoiceFilterBuyer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInvoiceFilterBuyer.Location = new System.Drawing.Point(58, 10);
            this.cmbInvoiceFilterBuyer.Name = "cmbInvoiceFilterBuyer";
            this.cmbInvoiceFilterBuyer.Width = 160;
            // lblInvoiceFilterStatus
            this.lblInvoiceFilterStatus.AutoSize = true;
            this.lblInvoiceFilterStatus.Location = new System.Drawing.Point(228, 13);
            this.lblInvoiceFilterStatus.Name = "lblInvoiceFilterStatus";
            this.lblInvoiceFilterStatus.Text = "Status:";
            // cmbInvoiceFilterStatus
            this.cmbInvoiceFilterStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInvoiceFilterStatus.Location = new System.Drawing.Point(280, 10);
            this.cmbInvoiceFilterStatus.Name = "cmbInvoiceFilterStatus";
            this.cmbInvoiceFilterStatus.Width = 130;
            // btnClearInvoiceFilter
            this.btnClearInvoiceFilter.Text = "Clear";
            this.btnClearInvoiceFilter.Name = "btnClearInvoiceFilter";
            this.btnClearInvoiceFilter.Location = new System.Drawing.Point(420, 9);
            this.btnClearInvoiceFilter.Size = new System.Drawing.Size(130, 25);
            this.btnClearInvoiceFilter.UseVisualStyleBackColor = true;
            // panelInvoiceFilter
            this.panelInvoiceFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInvoiceFilter.Height = 35;
            this.panelInvoiceFilter.Name = "panelInvoiceFilter";
            this.panelInvoiceFilter.Controls.Add(this.lblInvoiceFilterBuyer);
            this.panelInvoiceFilter.Controls.Add(this.cmbInvoiceFilterBuyer);
            this.panelInvoiceFilter.Controls.Add(this.lblInvoiceFilterStatus);
            this.panelInvoiceFilter.Controls.Add(this.cmbInvoiceFilterStatus);
            this.panelInvoiceFilter.Controls.Add(this.btnClearInvoiceFilter);
            // btnInvoicePrev
            this.btnInvoicePrev.Text = "< Prev";
            this.btnInvoicePrev.Name = "btnInvoicePrev";
            this.btnInvoicePrev.Size = new System.Drawing.Size(95, 25);
            this.btnInvoicePrev.Location = new System.Drawing.Point(5, 5);
            // lblInvoicePage
            this.lblInvoicePage.Name = "lblInvoicePage";
            this.lblInvoicePage.Text = "Page 1";
            this.lblInvoicePage.AutoSize = true;
            this.lblInvoicePage.Location = new System.Drawing.Point(110, 9);
            this.lblInvoicePage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // btnInvoiceNext
            this.btnInvoiceNext.Text = "Next >";
            this.btnInvoiceNext.Name = "btnInvoiceNext";
            this.btnInvoiceNext.Size = new System.Drawing.Size(70, 25);
            this.btnInvoiceNext.Location = new System.Drawing.Point(180, 5);
            // btnInvoicePreview
            this.btnInvoicePreview.Text = "🖨 Preview Invoice";
            this.btnInvoicePreview.Name = "btnInvoicePreview";
            this.btnInvoicePreview.Size = new System.Drawing.Size(130, 25);
            this.btnInvoicePreview.Location = new System.Drawing.Point(230, 5);
            // btnDeleteInvoice
            this.btnDeleteInvoice.Text = "Delete Invoice";
            this.btnDeleteInvoice.Name = "btnDeleteInvoice";
            this.btnDeleteInvoice.Size = new System.Drawing.Size(110, 25);
            this.btnDeleteInvoice.Location = new System.Drawing.Point(370, 5);
            // panelInvoicePagination
            this.panelInvoicePagination.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelInvoicePagination.Height = 35;
            this.panelInvoicePagination.Name = "panelInvoicePagination";
            this.panelInvoicePagination.Controls.Add(this.btnInvoicePrev);
            this.panelInvoicePagination.Controls.Add(this.lblInvoicePage);
            this.panelInvoicePagination.Controls.Add(this.btnInvoiceNext);
            this.panelInvoicePagination.Controls.Add(this.btnInvoicePreview);
            this.panelInvoicePagination.Controls.Add(this.btnDeleteInvoice);
            // tabInvoices
            this.tabInvoices.Controls.Add(this.dgvInvoices);
            this.tabInvoices.Controls.Add(this.panelInvoicePagination);
            this.tabInvoices.Controls.Add(this.panelInvoiceFilter);
            this.tabInvoices.Location = new System.Drawing.Point(4, 24);
            this.tabInvoices.Name = "tabInvoices";
            this.tabInvoices.Size = new System.Drawing.Size(792, 422);
            this.tabInvoices.TabIndex = 5;
            this.tabInvoices.Text = "Invoices";
            this.tabInvoices.UseVisualStyleBackColor = true;
            // tabSettings
            this.tabSettings.Location = new System.Drawing.Point(4, 24);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Size = new System.Drawing.Size(792, 422);
            this.tabSettings.TabIndex = 3;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;

            // tabControlSettings (inner)
            this.tabControlSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlSettings.Name = "tabControlSettings";
            this.tabControlSettings.TabPages.AddRange(new System.Windows.Forms.TabPage[] {
                this.tabProfile,
                this.tabStore,
                this.tabGoogleDrive,
                this.tabBackup });

            // ── tabProfile ───────────────────────────────────────────────
            this.tabProfile.Name = "tabProfile";
            this.tabProfile.Text = "Profile";
            this.tabProfile.UseVisualStyleBackColor = true;
            this.tabProfile.Padding = new System.Windows.Forms.Padding(20);

            this.lblCurrentPassword.Text = "Current Password";
            this.lblCurrentPassword.AutoSize = true;
            this.lblCurrentPassword.Location = new System.Drawing.Point(30, 30);

            this.txtCurrentPassword.Location = new System.Drawing.Point(30, 50);
            this.txtCurrentPassword.Size = new System.Drawing.Size(260, 23);
            this.txtCurrentPassword.UseSystemPasswordChar = true;
            this.txtCurrentPassword.Name = "txtCurrentPassword";

            this.lblNewPassword.Text = "New Password";
            this.lblNewPassword.AutoSize = true;
            this.lblNewPassword.Location = new System.Drawing.Point(30, 85);

            this.txtNewPassword.Location = new System.Drawing.Point(30, 105);
            this.txtNewPassword.Size = new System.Drawing.Size(260, 23);
            this.txtNewPassword.UseSystemPasswordChar = true;
            this.txtNewPassword.Name = "txtNewPassword";

            this.lblConfirmPassword.Text = "Confirm New Password";
            this.lblConfirmPassword.AutoSize = true;
            this.lblConfirmPassword.Location = new System.Drawing.Point(30, 140);

            this.txtConfirmPassword.Location = new System.Drawing.Point(30, 160);
            this.txtConfirmPassword.Size = new System.Drawing.Size(260, 23);
            this.txtConfirmPassword.UseSystemPasswordChar = true;
            this.txtConfirmPassword.Name = "txtConfirmPassword";

            this.btnChangePassword.Text = "Change Password";
            this.btnChangePassword.Name = "btnChangePassword";
            this.btnChangePassword.Location = new System.Drawing.Point(30, 200);
            this.btnChangePassword.Size = new System.Drawing.Size(140, 30);
            this.btnChangePassword.UseVisualStyleBackColor = true;

            this.tabProfile.Controls.Add(this.lblCurrentPassword);
            this.tabProfile.Controls.Add(this.txtCurrentPassword);
            this.tabProfile.Controls.Add(this.lblNewPassword);
            this.tabProfile.Controls.Add(this.txtNewPassword);
            this.tabProfile.Controls.Add(this.lblConfirmPassword);
            this.tabProfile.Controls.Add(this.txtConfirmPassword);
            this.tabProfile.Controls.Add(this.btnChangePassword);

            // Username section in Profile tab
            this.lblUsernameProfile.Text = "Username";
            this.lblUsernameProfile.AutoSize = true;
            this.lblUsernameProfile.Location = new System.Drawing.Point(30, 250);

            this.txtUsernameProfile.Location = new System.Drawing.Point(30, 270);
            this.txtUsernameProfile.Size = new System.Drawing.Size(260, 23);
            this.txtUsernameProfile.Name = "txtUsernameProfile";

            this.btnChangeUsername.Text = "Change Username";
            this.btnChangeUsername.Name = "btnChangeUsername";
            this.btnChangeUsername.Location = new System.Drawing.Point(30, 305);
            this.btnChangeUsername.Size = new System.Drawing.Size(150, 30);
            this.btnChangeUsername.UseVisualStyleBackColor = true;

            this.tabProfile.Controls.Add(this.lblUsernameProfile);
            this.tabProfile.Controls.Add(this.txtUsernameProfile);
            this.tabProfile.Controls.Add(this.btnChangeUsername);

            // ── tabStore ─────────────────────────────────────────────────
            this.tabStore.Name = "tabStore";
            this.tabStore.Text = "Store";
            this.tabStore.UseVisualStyleBackColor = true;
            this.tabStore.Padding = new System.Windows.Forms.Padding(20);

            this.lblStoreName.Text = "Store Name";
            this.lblStoreName.AutoSize = true;
            this.lblStoreName.Location = new System.Drawing.Point(30, 30);

            this.txtStoreName.Location = new System.Drawing.Point(30, 50);
            this.txtStoreName.Size = new System.Drawing.Size(300, 23);
            this.txtStoreName.Name = "txtStoreName";

            this.btnSaveStoreName.Text = "Save Name";
            this.btnSaveStoreName.Name = "btnSaveStoreName";
            this.btnSaveStoreName.Location = new System.Drawing.Point(340, 48);
            this.btnSaveStoreName.Size = new System.Drawing.Size(90, 27);
            this.btnSaveStoreName.UseVisualStyleBackColor = true;

            this.picStoreLogo.Location = new System.Drawing.Point(30, 95);
            this.picStoreLogo.Size = new System.Drawing.Size(128, 128);
            this.picStoreLogo.Name = "picStoreLogo";
            this.picStoreLogo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picStoreLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;

            this.btnChangeLogo.Text = "Change Logo";
            this.btnChangeLogo.Name = "btnChangeLogo";
            this.btnChangeLogo.Location = new System.Drawing.Point(30, 235);
            this.btnChangeLogo.Size = new System.Drawing.Size(120, 30);
            this.btnChangeLogo.UseVisualStyleBackColor = true;

            this.tabStore.Controls.Add(this.lblStoreName);
            this.tabStore.Controls.Add(this.txtStoreName);
            this.tabStore.Controls.Add(this.btnSaveStoreName);
            this.tabStore.Controls.Add(this.picStoreLogo);
            this.tabStore.Controls.Add(this.btnChangeLogo);

            // ── tabGoogleDrive ─────────────────────────────────────────
            this.tabGoogleDrive.Name = "tabGoogleDrive";
            this.tabGoogleDrive.Text = "Google Drive";
            this.tabGoogleDrive.UseVisualStyleBackColor = true;
            this.tabGoogleDrive.Padding = new System.Windows.Forms.Padding(20);

            this.lblGoogleDriveClientId.Text = "Client ID";
            this.lblGoogleDriveClientId.AutoSize = true;
            this.lblGoogleDriveClientId.Location = new System.Drawing.Point(30, 30);

            this.txtGoogleDriveClientId.Location = new System.Drawing.Point(30, 50);
            this.txtGoogleDriveClientId.Size = new System.Drawing.Size(360, 23);
            this.txtGoogleDriveClientId.Name = "txtGoogleDriveClientId";

            this.lblGoogleDriveClientSecret.Text = "Client Secret";
            this.lblGoogleDriveClientSecret.AutoSize = true;
            this.lblGoogleDriveClientSecret.Location = new System.Drawing.Point(30, 85);

            this.txtGoogleDriveClientSecret.Location = new System.Drawing.Point(30, 105);
            this.txtGoogleDriveClientSecret.Size = new System.Drawing.Size(360, 23);
            this.txtGoogleDriveClientSecret.Name = "txtGoogleDriveClientSecret";
            this.txtGoogleDriveClientSecret.UseSystemPasswordChar = true;

            this.lblGoogleDriveFolderId.Text = "Folder ID or Name";
            this.lblGoogleDriveFolderId.AutoSize = true;
            this.lblGoogleDriveFolderId.Location = new System.Drawing.Point(30, 140);

            this.txtGoogleDriveFolderId.Location = new System.Drawing.Point(30, 160);
            this.txtGoogleDriveFolderId.Size = new System.Drawing.Size(360, 23);
            this.txtGoogleDriveFolderId.Name = "txtGoogleDriveFolderId";

            this.lblGoogleDriveRefreshToken.Text = "Refresh Token";
            this.lblGoogleDriveRefreshToken.AutoSize = true;
            this.lblGoogleDriveRefreshToken.Location = new System.Drawing.Point(30, 195);

            this.txtGoogleDriveRefreshToken.Location = new System.Drawing.Point(30, 215);
            this.txtGoogleDriveRefreshToken.Size = new System.Drawing.Size(360, 23);
            this.txtGoogleDriveRefreshToken.Name = "txtGoogleDriveRefreshToken";
            this.txtGoogleDriveRefreshToken.UseSystemPasswordChar = true;

            this.btnSaveGoogleDriveConfig.Text = "Save Google Drive Config";
            this.btnSaveGoogleDriveConfig.Name = "btnSaveGoogleDriveConfig";
            this.btnSaveGoogleDriveConfig.Location = new System.Drawing.Point(30, 255);
            this.btnSaveGoogleDriveConfig.Size = new System.Drawing.Size(190, 30);
            this.btnSaveGoogleDriveConfig.UseVisualStyleBackColor = true;

            this.btnGenerateGoogleDriveRefreshToken.Text = "Generate Refresh Token";
            this.btnGenerateGoogleDriveRefreshToken.Name = "btnGenerateGoogleDriveRefreshToken";
            this.btnGenerateGoogleDriveRefreshToken.Location = new System.Drawing.Point(230, 255);
            this.btnGenerateGoogleDriveRefreshToken.Size = new System.Drawing.Size(160, 30);
            this.btnGenerateGoogleDriveRefreshToken.UseVisualStyleBackColor = true;

            this.btnGoogleDriveHelp.Text = "Setup Help";
            this.btnGoogleDriveHelp.Name = "btnGoogleDriveHelp";
            this.btnGoogleDriveHelp.Location = new System.Drawing.Point(400, 255);
            this.btnGoogleDriveHelp.Size = new System.Drawing.Size(150, 30);
            this.btnGoogleDriveHelp.UseVisualStyleBackColor = true;

            this.btnOpenGoogleCloudConsole.Text = "Open Console";
            this.btnOpenGoogleCloudConsole.Name = "btnOpenGoogleCloudConsole";
            this.btnOpenGoogleCloudConsole.Location = new System.Drawing.Point(560, 255);
            this.btnOpenGoogleCloudConsole.Size = new System.Drawing.Size(140, 30);
            this.btnOpenGoogleCloudConsole.UseVisualStyleBackColor = true;

            this.tabGoogleDrive.Controls.Add(this.lblGoogleDriveClientId);
            this.tabGoogleDrive.Controls.Add(this.txtGoogleDriveClientId);
            this.tabGoogleDrive.Controls.Add(this.lblGoogleDriveClientSecret);
            this.tabGoogleDrive.Controls.Add(this.txtGoogleDriveClientSecret);
            this.tabGoogleDrive.Controls.Add(this.lblGoogleDriveFolderId);
            this.tabGoogleDrive.Controls.Add(this.txtGoogleDriveFolderId);
            this.tabGoogleDrive.Controls.Add(this.lblGoogleDriveRefreshToken);
            this.tabGoogleDrive.Controls.Add(this.txtGoogleDriveRefreshToken);
            this.tabGoogleDrive.Controls.Add(this.btnSaveGoogleDriveConfig);
            this.tabGoogleDrive.Controls.Add(this.btnGenerateGoogleDriveRefreshToken);
            this.tabGoogleDrive.Controls.Add(this.btnGoogleDriveHelp);
            this.tabGoogleDrive.Controls.Add(this.btnOpenGoogleCloudConsole);

            // ── tabBackup ───────────────────────────────────────────────
            this.tabBackup.Name = "tabBackup";
            this.tabBackup.Text = "Backup";
            this.tabBackup.UseVisualStyleBackColor = true;
            this.tabBackup.Padding = new System.Windows.Forms.Padding(20);

            this.lblBackupInfo.Text = "Download creates a local copy of the SQLite database. Restore replaces the current local database file.";
            this.lblBackupInfo.AutoSize = true;
            this.lblBackupInfo.Location = new System.Drawing.Point(30, 30);
            this.lblBackupInfo.MaximumSize = new System.Drawing.Size(650, 0);

            this.btnDownloadDatabaseBackup.Text = "Download DB Backup";
            this.btnDownloadDatabaseBackup.Name = "btnDownloadDatabaseBackup";
            this.btnDownloadDatabaseBackup.Location = new System.Drawing.Point(30, 80);
            this.btnDownloadDatabaseBackup.Size = new System.Drawing.Size(180, 32);
            this.btnDownloadDatabaseBackup.UseVisualStyleBackColor = true;

            this.btnRestoreDatabaseBackup.Text = "Upload / Restore DB Backup";
            this.btnRestoreDatabaseBackup.Name = "btnRestoreDatabaseBackup";
            this.btnRestoreDatabaseBackup.Location = new System.Drawing.Point(220, 80);
            this.btnRestoreDatabaseBackup.Size = new System.Drawing.Size(210, 32);
            this.btnRestoreDatabaseBackup.UseVisualStyleBackColor = true;

            this.btnUploadGoogleDriveBackup.Text = "Upload to Google Drive";
            this.btnUploadGoogleDriveBackup.Name = "btnUploadGoogleDriveBackup";
            this.btnUploadGoogleDriveBackup.Location = new System.Drawing.Point(30, 130);
            this.btnUploadGoogleDriveBackup.Size = new System.Drawing.Size(180, 32);
            this.btnUploadGoogleDriveBackup.UseVisualStyleBackColor = true;

            this.btnDownloadGoogleDriveBackup.Text = "Download from Google Drive";
            this.btnDownloadGoogleDriveBackup.Name = "btnDownloadGoogleDriveBackup";
            this.btnDownloadGoogleDriveBackup.Location = new System.Drawing.Point(220, 130);
            this.btnDownloadGoogleDriveBackup.Size = new System.Drawing.Size(210, 32);
            this.btnDownloadGoogleDriveBackup.UseVisualStyleBackColor = true;

            this.tabBackup.Controls.Add(this.lblBackupInfo);
            this.tabBackup.Controls.Add(this.btnDownloadDatabaseBackup);
            this.tabBackup.Controls.Add(this.btnRestoreDatabaseBackup);
            this.tabBackup.Controls.Add(this.btnUploadGoogleDriveBackup);
            this.tabBackup.Controls.Add(this.btnDownloadGoogleDriveBackup);

            this.tabSettings.Controls.Add(this.tabControlSettings);
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
            // btnLogout – always visible at the top-right of the main window
            // ── Top bar: greeting | date-time | logout ────────────────────
            this.lblGreeting.Name = "lblGreeting";
            this.lblGreeting.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblGreeting.BackColor = System.Drawing.SystemColors.Control;
            this.lblGreeting.Location = new System.Drawing.Point(10, 11);
            this.lblGreeting.Size = new System.Drawing.Size(10, 22);
            this.lblGreeting.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;

            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Text = "";
            this.lblDateTime.AutoSize = true;
            this.lblDateTime.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblDateTime.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblDateTime.Location = new System.Drawing.Point(400, 11);

            this.btnLogout.Text = "🔓 Logout";
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(100, 30);
            this.btnLogout.Location = new System.Drawing.Point(888, 5);
            this.btnLogout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnLogout.UseVisualStyleBackColor = true;

            this.timerClock.Interval = 1000;
            this.timerClock.Enabled = true;

            // 
            // Main
            // 
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.lblGreeting);
            this.Controls.Add(this.lblDateTime);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.tabControlMain);
            this.Name = "Main";
            this.Text = "Amiar Store Manager";
            this.ResumeLayout(false);

        }

        #endregion
    }
}
