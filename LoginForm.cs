namespace KabyliaTaste
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using KabyliaTaste.Data;
    using KabyliaTaste.Services;

    public partial class LoginForm : Form
    {
        private readonly bool _syncDatabaseOnLoad;

        private static string GetDatabaseFilePath()
        {
            var appDataFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "KabyliaTaste");

            Directory.CreateDirectory(appDataFolder);
            return Path.Combine(appDataFolder, "app.db");
        }

        public LoginForm(bool syncDatabaseOnLoad = true)
        {
            _syncDatabaseOnLoad = syncDatabaseOnLoad;
            InitializeComponent();
            AcceptButton = btnLogin;
            Load += LoginForm_Load;
            btnLogin.Click += BtnLogin_Click;
        }

        private async void LoginForm_Load(object? sender, EventArgs e)
        {
            await UpdateLicenseStatusAsync();
            if (_syncDatabaseOnLoad)
            {
                await RefreshDatabaseFromGoogleDriveAsync();
            }
            ApplyStoreName();
            txtUsername.Focus();
        }

        private void ApplyStoreName()
        {
            using var db = new AppDbContext();
            var storeName = db.StoreSettings.FirstOrDefault()?.StoreName;

            if (string.IsNullOrWhiteSpace(storeName))
                storeName = "Amiar Store Manager";

            lblTitle.Text = storeName;
            Text = $"Login - {storeName}";
        }

        private async Task UpdateLicenseStatusAsync()
        {
            try
            {
                var license = await new StoreLicenseService().CheckLicenseAsync();

                if (!license.IsPackagedApp)
                {
                    lblTrialStatus.Text = string.Empty;
                    return;
                }

                if (!license.IsLicenseValid)
                {
                    lblTrialStatus.ForeColor = System.Drawing.Color.DarkRed;
                    lblTrialStatus.Text = license.ErrorMessage ?? "Microsoft Store license is not valid.";
                    return;
                }

                if (license.IsTrial && license.ExpirationDate.HasValue)
                {
                    var daysRemaining = Math.Max(0, (int)Math.Ceiling((license.ExpirationDate.Value - DateTimeOffset.Now).TotalDays));
                    lblTrialStatus.ForeColor = System.Drawing.Color.DarkGreen;
                    lblTrialStatus.Text = $"Trial remaining: {daysRemaining} day{(daysRemaining == 1 ? string.Empty : "s")} (expires {license.ExpirationDate.Value:dd/MM/yyyy})";
                }
                else
                {
                    lblTrialStatus.ForeColor = System.Drawing.Color.DarkGreen;
                    lblTrialStatus.Text = "Microsoft Store license active.";
                }
            }
            catch
            {
                lblTrialStatus.Text = string.Empty;
            }
        }

        private async Task RefreshDatabaseFromGoogleDriveAsync()
        {
            using var db = new AppDbContext();
            var store = db.StoreSettings.FirstOrDefault();

            if (store == null ||
                string.IsNullOrWhiteSpace(store.GoogleDriveClientId) ||
                string.IsNullOrWhiteSpace(store.GoogleDriveClientSecret) ||
                string.IsNullOrWhiteSpace(store.GoogleDriveRefreshToken))
            {
                return;
            }

            using var progressToast = new BackupToastForm();
            progressToast.ShowToast("Downloading database backup from Google Drive...");

            var progress = new Progress<int>(percent =>
            {
                progressToast.SetProgress(percent, $"Downloading database backup from Google Drive... {percent}%");
            });

            try
            {
                await Task.Run(() =>
                {
                    var service = new GoogleDriveBackupService();
                    service.DownloadDatabaseBackup(store, GetDatabaseFilePath(), progress);
                });

                progressToast.SetProgress(100, "Database is up to date.");
            }
            catch
            {
                // Keep the local database available if the remote download fails.
            }
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            var username = txtUsername.Text.Trim();
            var password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblError.Text = "Please enter username and password.";
                return;
            }

            using var db = new AppDbContext();
            var user = db.Users.FirstOrDefault(u => u.Username.ToLower() == username.ToLower() && u.Password == password);
            if (user == null)
            {
                lblError.Text = "Invalid username or password.";
                txtPassword.Clear();
                txtPassword.Focus();
                return;
            }

            Session.CurrentUser = user;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
