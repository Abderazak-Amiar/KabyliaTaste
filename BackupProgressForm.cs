using System.Drawing;
using System.Windows.Forms;

namespace KabyliaTaste
{
    public sealed class BackupProgressForm : Form
    {
        private readonly Label _statusLabel;
        private readonly ProgressBar _progressBar;

        public BackupProgressForm()
        {
            Text = "Google Drive Backup";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MinimizeBox = false;
            MaximizeBox = false;
            ShowInTaskbar = false;
            ControlBox = false;
            ClientSize = new Size(420, 110);

            _statusLabel = new Label
            {
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font(Font.FontFamily, 10f, FontStyle.Regular),
                Text = "Preparing database backup..."
            };

            _progressBar = new ProgressBar
            {
                Dock = DockStyle.Bottom,
                Height = 24,
                Minimum = 0,
                Maximum = 100,
                Style = ProgressBarStyle.Continuous,
                Value = 0
            };

            Controls.Add(_progressBar);
            Controls.Add(_statusLabel);
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
            _statusLabel.Text = statusText ?? $"Uploading database backup... {value}%";
            Refresh();
        }
    }
}
