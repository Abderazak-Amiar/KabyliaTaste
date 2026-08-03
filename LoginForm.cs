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
        public LoginForm()
        {
            InitializeComponent();
            AcceptButton = btnLogin;
            Load += LoginForm_Load;
            btnLogin.Click += BtnLogin_Click;
        }

        private async void LoginForm_Load(object? sender, EventArgs e)
        {
            await RefreshDatabaseFromGoogleDriveAsync();
            txtUsername.Focus();
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

            using var progressForm = new BackupProgressForm();
            progressForm.Show(this);
            progressForm.SetProgress(0, "Downloading database backup from Google Drive...");

            var progress = new Progress<int>(percent =>
            {
                progressForm.SetProgress(percent, $"Downloading database backup from Google Drive... {percent}%");
            });

            try
            {
                await Task.Run(() =>
                {
                    var service = new GoogleDriveBackupService();
                    service.DownloadDatabaseBackup(store, Path.GetFullPath("app.db"), progress);
                });

                progressForm.SetProgress(100, "Database is up to date.");
            }
            catch
            {
                // Keep the local database available if the remote download fails.
            }
            finally
            {
                progressForm.Close();
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
