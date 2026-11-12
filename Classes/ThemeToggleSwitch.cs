
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DuckFarm.Classes
{
    public sealed class ThemeToggleSwitch : Control
    {
        private bool isDarkMode;

        public event EventHandler ThemeChanged;

        public bool IsDarkMode
        {
            get { return isDarkMode; }
            set
            {
                if (isDarkMode == value)
                    return;

                isDarkMode = value;

                Invalidate();
                ThemeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public ThemeToggleSwitch()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw,
                true);

            Size = new Size(112, 34);
            MinimumSize = new Size(112, 34);
            MaximumSize = new Size(112, 34);

            Cursor = Cursors.Hand;
            TabStop = true;
            AccessibleName = "Dark / Light Mode";
            AccessibleDescription = "Mengubah mode tampilan aplikasi";
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Rectangle track = new Rectangle(1, 2, 110, 30);

            Color trackColor = isDarkMode
                ? Color.FromArgb(45, 55, 72)
                : Color.FromArgb(226, 232, 240);

            Color textColor = isDarkMode
                ? Color.White
                : Color.FromArgb(51, 65, 85);

            using (GraphicsPath path = CreateRoundedPath(track, 15))
            using (SolidBrush trackBrush = new SolidBrush(trackColor))
            {
                g.FillPath(trackBrush, path);
            }

            int knobX = isDarkMode ? 80 : 4;

            Rectangle knob = new Rectangle(knobX, 5, 24, 24);

            using (SolidBrush knobBrush = new SolidBrush(Color.White))
            {
                g.FillEllipse(knobBrush, knob);
            }

            string text = isDarkMode ? "Dark" : "Light";

            using (Font font = new Font(
                "Segoe UI",
                9F,
                FontStyle.Bold))
            using (SolidBrush textBrush = new SolidBrush(textColor))
            {
                int textX = isDarkMode ? 17 : 36;

                g.DrawString(
                    text,
                    font,
                    textBrush,
                    textX,
                    9);
            }

            if (Focused)
            {
                Rectangle focusRect = new Rectangle(0, 0, Width - 1, Height - 1);
                ControlPaint.DrawFocusRectangle(g, focusRect);
            }
        }

        private static GraphicsPath CreateRoundedPath(
            Rectangle rectangle,
            int radius)
        {
            GraphicsPath path = new GraphicsPath();

            int diameter = radius * 2;

            path.AddArc(
                rectangle.X,
                rectangle.Y,
                diameter,
                diameter,
                180,
                90);

            path.AddArc(
                rectangle.Right - diameter,
                rectangle.Y,
                diameter,
                diameter,
                270,
                90);

            path.AddArc(
                rectangle.Right - diameter,
                rectangle.Bottom - diameter,
                diameter,
                diameter,
                0,
                90);

            path.AddArc(
                rectangle.X,
                rectangle.Bottom - diameter,
                diameter,
                diameter,
                90,
                90);

            path.CloseFigure();

            return path;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            IsDarkMode = !IsDarkMode;
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Space ||
                keyData == Keys.Enter)
            {
                return true;
            }

            return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.Space ||
                e.KeyCode == Keys.Enter)
            {
                IsDarkMode = !IsDarkMode;
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}
