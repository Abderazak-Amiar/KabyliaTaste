namespace KabyliaTaste
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Label lblTrialStatus;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblError = new System.Windows.Forms.Label();
            this.lblTrialStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.Text = "Amiar Store Manager";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitle.Location = new System.Drawing.Point(60, 20);
            this.lblTitle.Size = new System.Drawing.Size(280, 45);

            // lblUsername
            this.lblUsername.Text = "Username";
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(80, 85);

            // txtUsername
            this.txtUsername.Location = new System.Drawing.Point(80, 105);
            this.txtUsername.Size = new System.Drawing.Size(240, 23);
            this.txtUsername.Name = "txtUsername";

            // lblPassword
            this.lblPassword.Text = "Password";
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(80, 140);

            // txtPassword
            this.txtPassword.Location = new System.Drawing.Point(80, 160);
            this.txtPassword.Size = new System.Drawing.Size(240, 23);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.UseSystemPasswordChar = true;

            // lblError
            this.lblError.Text = "";
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.AutoSize = true;
            this.lblError.Location = new System.Drawing.Point(80, 195);
            this.lblError.Size = new System.Drawing.Size(240, 15);

            // lblTrialStatus
            this.lblTrialStatus.Text = "";
            this.lblTrialStatus.AutoSize = true;
            this.lblTrialStatus.Location = new System.Drawing.Point(80, 214);
            this.lblTrialStatus.Size = new System.Drawing.Size(240, 15);
            this.lblTrialStatus.ForeColor = System.Drawing.Color.DarkGreen;

            // btnLogin
            this.btnLogin.Text = "Login";
            this.btnLogin.Location = new System.Drawing.Point(160, 242);
            this.btnLogin.Size = new System.Drawing.Size(80, 30);
            this.btnLogin.UseVisualStyleBackColor = true;

            // LoginForm
            this.ClientSize = new System.Drawing.Size(400, 305);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login – KabyliaTaste";
            this.Name = "LoginForm";
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.lblTrialStatus);
            this.Controls.Add(this.btnLogin);
            this.ResumeLayout(false);
        }
    }
}
