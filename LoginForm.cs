namespace KabyliaTaste
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using KabyliaTaste.Data;
    using KabyliaTaste.Models;
    using KabyliaTaste.Services;
    using Microsoft.EntityFrameworkCore;

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
            ApplyLocalization();
            ApplyStoreName();
            txtUsername.Focus();
        }

        private void ApplyLocalization()
        {
            lblUsername.Text = AppLocalization.T("Username");
            lblPassword.Text = AppLocalization.T("Password");
            btnLogin.Text = AppLocalization.T("Login");

            if (!string.IsNullOrWhiteSpace(lblError.Text))
                lblError.Text = AppLocalization.T(lblError.Text);

            if (!string.IsNullOrWhiteSpace(lblTrialStatus.Text))
                lblTrialStatus.Text = LocalizeTrialStatus(lblTrialStatus.Text);
        }

        private static string LocalizeTrialStatus(string text)
        {
            if (text.StartsWith("Trial remaining:", StringComparison.OrdinalIgnoreCase))
                return text.Replace("Trial remaining:", AppLocalization.T("Trial remaining") + ":", StringComparison.OrdinalIgnoreCase)
                    .Replace("day", AppLocalization.T("day"), StringComparison.OrdinalIgnoreCase)
                    .Replace("days", AppLocalization.T("days"), StringComparison.OrdinalIgnoreCase);

            return text switch
            {
                "Microsoft Store license active." => AppLocalization.T("Microsoft Store license active."),
                "Microsoft Store license is not valid." => AppLocalization.T("Microsoft Store license is not valid."),
                _ => text
            };
        }

        private void ApplyStoreName()
        {
            using var db = new AppDbContext();
            var storeName = db.StoreSettings.FirstOrDefault()?.StoreName;

            if (string.IsNullOrWhiteSpace(storeName))
                storeName = "Amiar Store Manager";

            lblTitle.Text = storeName;
            Text = $"{AppLocalization.T("Login - ")}{storeName}";
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
                    lblTrialStatus.Text = AppLocalization.T(license.ErrorMessage ?? "Microsoft Store license is not valid.");
                    return;
                }

                if (license.IsTrial && license.ExpirationDate.HasValue)
                {
                    var daysRemaining = Math.Max(0, (int)Math.Ceiling((license.ExpirationDate.Value - DateTimeOffset.Now).TotalDays));
                    lblTrialStatus.ForeColor = System.Drawing.Color.DarkGreen;
                    lblTrialStatus.Text = $"{AppLocalization.T("Trial remaining")}: {daysRemaining} {AppLocalization.T(daysRemaining == 1 ? "day" : "days")} ({license.ExpirationDate.Value:dd/MM/yyyy})";
                }
                else
                {
                    lblTrialStatus.ForeColor = System.Drawing.Color.DarkGreen;
                    lblTrialStatus.Text = AppLocalization.T("Microsoft Store license active.");
                }
            }
            catch
            {
                lblTrialStatus.Text = string.Empty;
            }
        }

        private async Task RefreshDatabaseFromGoogleDriveAsync()
        {
            StoreSettings? store;

            using (var db = new AppDbContext())
            {
                store = db.StoreSettings.FirstOrDefault();
            }

            if (store == null ||
                string.IsNullOrWhiteSpace(store.GoogleDriveClientId) ||
                string.IsNullOrWhiteSpace(store.GoogleDriveClientSecret) ||
                string.IsNullOrWhiteSpace(store.GoogleDriveRefreshToken))
            {
                return;
            }

            using var progressToast = new BackupToastForm();
            progressToast.ShowToast("Téléchargement de la sauvegarde de la base de données depuis Google Drive...");

            var progress = new Progress<int>(percent =>
            {
                progressToast.SetProgress(percent, $"Téléchargement de la sauvegarde de la base de données depuis Google Drive... {percent}%");
            });

            try
            {
                await Task.Run(() =>
                {
                    var service = new GoogleDriveBackupService();
                    service.DownloadDatabaseBackup(store, GetDatabaseFilePath(), progress);
                });

                using (var db = new AppDbContext())
                {
                    db.Database.Migrate();
                }

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
                lblError.Text = AppLocalization.T("Please enter username and password.");
                return;
            }

            using var db = new AppDbContext();
            var user = db.Users.FirstOrDefault(u => u.Username.ToLower() == username.ToLower() && u.Password == password);
            if (user == null)
            {
                lblError.Text = AppLocalization.T("Invalid username or password.");
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
