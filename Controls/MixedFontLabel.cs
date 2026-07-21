namespace KabyliaTaste.Controls
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// A read-only label that can render text segments with different fonts
    /// (e.g. normal and bold) side-by-side, with no cursor flickering.
    /// </summary>
    public class MixedFontLabel : Control
    {
        public readonly record struct Segment(string Text, bool Bold);

        private readonly List<Segment> _segments = new();

        public MixedFontLabel()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint  |
                     ControlStyles.UserPaint             |
                     ControlStyles.ResizeRedraw, true);

            Cursor = Cursors.Default;
            TabStop = false;
        }

        public void SetSegments(IEnumerable<Segment> segments)
        {
            _segments.Clear();
            _segments.AddRange(segments);
            // Auto-size width to content
            using var g = CreateGraphics();
            float totalWidth = 0;
            float maxHeight  = 0;
            foreach (var seg in _segments)
            {
                using var f = MakeFont(seg.Bold);
                var sz = g.MeasureString(seg.Text, f);
                totalWidth += sz.Width;
                if (sz.Height > maxHeight) maxHeight = sz.Height;
            }
            Width  = (int)Math.Ceiling(totalWidth) + 4;
            Height = (int)Math.Ceiling(maxHeight);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            float x = 0;
            foreach (var seg in _segments)
            {
                using var f = MakeFont(seg.Bold);
                e.Graphics.DrawString(seg.Text, f, new SolidBrush(ForeColor), x, 0);
                x += e.Graphics.MeasureString(seg.Text, f).Width;
            }
        }

        // Block the IBeam cursor that Windows tries to set over text controls
        protected override void WndProc(ref Message m)
        {
            const int WM_SETCURSOR = 0x0020;
            if (m.Msg == WM_SETCURSOR)
            {
                Cursor.Current = Cursors.Default;
                m.Result = (IntPtr)1;
                return;
            }
            base.WndProc(ref m);
        }

        private Font MakeFont(bool bold) =>
            new Font(Font.FontFamily, Font.Size,
                     bold ? FontStyle.Bold : FontStyle.Regular,
                     Font.Unit);
    }
}
