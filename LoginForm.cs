namespace KabyliaTaste
{
    using System;
    using System.Windows.Forms;
    using KabyliaTaste.Data;

    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            AcceptButton = btnLogin;
            Load += LoginForm_Load;
            btnLogin.Click += BtnLogin_Click;
        }

        private void LoginForm_Load(object? sender, EventArgs e)
        {
            txtUsername.Focus();
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
