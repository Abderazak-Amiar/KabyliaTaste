using System.Drawing;
using System.Drawing;
using System.Windows.Forms;

namespace KabyliaTaste
{
    public sealed class BackupToastForm : Form
    {
        private readonly Label _statusLabel;
        private readonly ProgressBar _progressBar;

        public BackupToastForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            TopMost = true;
            BackColor = Color.White;
            Opacity = 0.97;
            ClientSize = new Size(320, 92);

            _statusLabel = new Label
            {
                Dock = DockStyle.Top,
                Height = 58,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font(Font.FontFamily, 9.5f, FontStyle.Regular),
                Text = "Preparing database backup...",
                Padding = new Padding(10, 8, 10, 8)
            };

            _progressBar = new ProgressBar
            {
                Dock = DockStyle.Bottom,
                Height = 18,
                Minimum = 0,
                Maximum = 100,
                Style = ProgressBarStyle.Continuous,
                Value = 0
            };

            Controls.Add(_progressBar);
            Controls.Add(_statusLabel);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            var area = Screen.FromControl(this).WorkingArea;
            Location = new Point(area.Right - Width - 16, area.Bottom - Height - 16);
        }

        public void ShowToast(string statusText)
        {
            _statusLabel.Text = statusText;
            Show();
            BringToFront();
            Refresh();
        }

        public void SetProgress(int percent, string? statusText = null)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => SetProgress(percent, statusText)));
                return;
            }

            var value = Math.Clamp(percent, 0, 100);
            _progressBar.Value = value;
            _statusLabel.Text = statusText ?? $"Database backup {value}%";
            Refresh();
        }
    }
}
