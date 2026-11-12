using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace DuckFarm.Classes.Controls
{
    public class AppButton : Button
    {
        // =========================================================
        // PRIVATE STATE
        // =========================================================

        private bool isMouseOver = false;
        private bool isMouseDown = false;


        // =========================================================
        // BORDER RADIUS
        // =========================================================

        private int borderRadius = 8;

        [Category("Appearance")]
        [Description("Radius sudut button.")]
        [DefaultValue(8)]
        public int BorderRadius
        {
            get
            {
                return borderRadius;
            }

            set
            {
                borderRadius = Math.Max(0, value);

                UpdateRegion();

                Invalidate();
            }
        }


        // =========================================================
        // BORDER COLOR
        // =========================================================

        private Color borderColor = Color.Transparent;

        [Category("Appearance")]
        [Description("Warna border button.")]
        public Color BorderColor
        {
            get
            {
                return borderColor;
            }

            set
            {
                borderColor = value;

                Invalidate();
            }
        }


        // =========================================================
        // BORDER SIZE
        // =========================================================

        private int borderSize = 0;

        [Category("Appearance")]
        [Description("Ukuran border button.")]
        [DefaultValue(0)]
        public int BorderSize
        {
            get
            {
                return borderSize;
            }

            set
            {
                borderSize = Math.Max(0, value);

                Invalidate();
            }
        }


        // =========================================================
        // HOVER COLOR
        // =========================================================

        private Color hoverColor =
            Color.FromArgb(29, 78, 216);

        [Category("Appearance")]
        [Description("Warna button ketika mouse berada di atas button.")]
        public Color HoverColor
        {
            get
            {
                return hoverColor;
            }

            set
            {
                hoverColor = value;

                Invalidate();
            }
        }


        // =========================================================
        // PRESSED COLOR
        // =========================================================

        private Color pressedColor =
            Color.FromArgb(30, 64, 175);

        [Category("Appearance")]
        [Description("Warna button ketika ditekan.")]
        public Color PressedColor
        {
            get
            {
                return pressedColor;
            }

            set
            {
                pressedColor = value;

                Invalidate();
            }
        }


        // =========================================================
        // DISABLED COLOR
        // =========================================================

        private Color disabledColor =
            Color.FromArgb(229, 231, 235);

        [Category("Appearance")]
        [Description("Warna background button ketika disabled.")]
        public Color DisabledColor
        {
            get
            {
                return disabledColor;
            }

            set
            {
                disabledColor = value;

                Invalidate();
            }
        }


        // =========================================================
        // DISABLED TEXT COLOR
        // =========================================================

        private Color disabledTextColor =
            Color.FromArgb(156, 163, 175);

        [Category("Appearance")]
        [Description("Warna text button ketika disabled.")]
        public Color DisabledTextColor
        {
            get
            {
                return disabledTextColor;
            }

            set
            {
                disabledTextColor = value;

                Invalidate();
            }
        }


        // =========================================================
        // TEXT PADDING
        // =========================================================

        private int textPadding = 10;

        [Category("Appearance")]
        [Description("Jarak horizontal text dari sisi button.")]
        [DefaultValue(10)]
        public int TextPadding
        {
            get
            {
                return textPadding;
            }

            set
            {
                textPadding = Math.Max(0, value);

                Invalidate();
            }
        }


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public AppButton()
        {
            // -----------------------------------------------------
            // CUSTOM PAINT
            // -----------------------------------------------------

            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor,
                true);

            DoubleBuffered = true;


            // -----------------------------------------------------
            // BUTTON STYLE
            // -----------------------------------------------------

            FlatStyle =
                FlatStyle.Flat;

            FlatAppearance.BorderSize =
                0;

            FlatAppearance.MouseOverBackColor =
                Color.Transparent;

            FlatAppearance.MouseDownBackColor =
                Color.Transparent;

            UseVisualStyleBackColor =
                false;

            TabStop =
                true;

            Cursor =
                Cursors.Hand;


            // -----------------------------------------------------
            // FONT
            // -----------------------------------------------------

            //Font =
            //    new Font(
            //        "Segoe UI",
            //        9F,
            //        FontStyle.Bold);

            Font =
                new System.Drawing.Font(
                "Segoe UI",
                9F,
                FontStyle.Bold);


            // -----------------------------------------------------
            // DEFAULT COLOR
            // -----------------------------------------------------

            BackColor =
                Color.FromArgb(
                    37,
                    99,
                    235);

            ForeColor =
                Color.White;


            // -----------------------------------------------------
            // DEFAULT SIZE
            // -----------------------------------------------------

            Width =
                100;

            Height =
                34;


            // -----------------------------------------------------
            // DEFAULT RADIUS
            // -----------------------------------------------------

            BorderRadius =
                8;


            // -----------------------------------------------------
            // ACCESSIBILITY
            // -----------------------------------------------------

            AccessibleRole =
                AccessibleRole.PushButton;
        }


        // =========================================================
        // ENABLED CHANGED
        // =========================================================

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);

            if (!Enabled)
            {
                isMouseOver = false;
                isMouseDown = false;

                Cursor =
                    Cursors.Default;
            }
            else
            {
                Cursor =
                    Cursors.Hand;
            }

            Invalidate();
        }


        // =========================================================
        // RESIZE
        // =========================================================

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            UpdateRegion();

            Invalidate();
        }


        // =========================================================
        // HANDLE CREATED
        // =========================================================

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            UpdateRegion();
        }


        // =========================================================
        // MOUSE ENTER
        // =========================================================

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            if (!Enabled)
                return;

            isMouseOver =
                true;

            Invalidate();
        }


        // =========================================================
        // MOUSE LEAVE
        // =========================================================

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            isMouseOver =
                false;

            isMouseDown =
                false;

            Invalidate();
        }


        // =========================================================
        // MOUSE DOWN
        // =========================================================

        protected override void OnMouseDown(
            MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (!Enabled)
                return;

            if (e.Button ==
                MouseButtons.Left)
            {
                isMouseDown =
                    true;

                Invalidate();
            }
        }


        // =========================================================
        // MOUSE UP
        // =========================================================

        protected override void OnMouseUp(
            MouseEventArgs e)
        {
            base.OnMouseUp(e);

            isMouseDown =
                false;

            Invalidate();
        }


        // =========================================================
        // KEY DOWN
        // =========================================================

        protected override void OnKeyDown(
            KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (!Enabled)
                return;

            if (e.KeyCode ==
                Keys.Space ||
                e.KeyCode ==
                Keys.Enter)
            {
                isMouseDown =
                    true;

                Invalidate();
            }
        }


        // =========================================================
        // KEY UP
        // =========================================================

        protected override void OnKeyUp(
            KeyEventArgs e)
        {
            base.OnKeyUp(e);

            isMouseDown =
                false;

            Invalidate();
        }


        // =========================================================
        // PAINT
        // =========================================================

        protected override void OnPaint(
            PaintEventArgs e)
        {
            Graphics g =
                e.Graphics;


            // -----------------------------------------------------
            // QUALITY
            // -----------------------------------------------------

            g.SmoothingMode =
                SmoothingMode.AntiAlias;

            g.PixelOffsetMode =
                PixelOffsetMode.HighQuality;

            g.CompositingQuality =
                CompositingQuality.HighQuality;


            // -----------------------------------------------------
            // BACKGROUND
            // -----------------------------------------------------

            Color parentColor =
                Parent != null
                    ? Parent.BackColor
                    : SystemColors.Control;

            g.Clear(parentColor);


            // -----------------------------------------------------
            // RECTANGLE
            // -----------------------------------------------------

            Rectangle rect =
                new Rectangle(
                    1,
                    1,
                    Math.Max(1, Width - 2),
                    Math.Max(1, Height - 2));


            // -----------------------------------------------------
            // RADIUS
            // -----------------------------------------------------

            int radius =
                Math.Min(
                    BorderRadius,
                    Math.Min(
                        rect.Width / 2,
                        rect.Height / 2));


            // -----------------------------------------------------
            // COLOR
            // -----------------------------------------------------

            Color fillColor;
            Color textColor;


            if (!Enabled)
            {
                fillColor =
                    DisabledColor;

                textColor =
                    DisabledTextColor;
            }
            else
            {
                fillColor =
                    BackColor;

                textColor =
                    ForeColor;

                if (isMouseDown)
                {
                    fillColor =
                        PressedColor;
                }
                else if (isMouseOver)
                {
                    fillColor =
                        HoverColor;
                }
            }


            // -----------------------------------------------------
            // ROUNDED PATH
            // -----------------------------------------------------

            using (GraphicsPath path =
                CreateRoundedPath(
                    rect,
                    radius))
            {
                // -------------------------------------------------
                // BACKGROUND
                // -------------------------------------------------

                using (SolidBrush brush =
                    new SolidBrush(fillColor))
                {
                    g.FillPath(
                        brush,
                        path);
                }


                // -------------------------------------------------
                // BORDER
                // -------------------------------------------------

                if (BorderSize > 0 &&
                    BorderColor != Color.Transparent)
                {
                    using (Pen pen =
                        new Pen(
                            BorderColor,
                            BorderSize))
                    {
                        pen.Alignment =
                            PenAlignment.Inset;

                        g.DrawPath(
                            pen,
                            path);
                    }
                }
            }


            // -----------------------------------------------------
            // TEXT RECTANGLE
            // -----------------------------------------------------

            Rectangle textRect =
                new Rectangle(
                    TextPadding,
                    0,
                    Math.Max(
                        1,
                        Width -
                        (TextPadding * 2)),
                    Height);


            // -----------------------------------------------------
            // TEXT FLAGS
            // -----------------------------------------------------

            TextFormatFlags flags =
                TextFormatFlags.HorizontalCenter |
                TextFormatFlags.VerticalCenter |
                TextFormatFlags.EndEllipsis |
                TextFormatFlags.NoPrefix |
                TextFormatFlags.SingleLine;


            // -----------------------------------------------------
            // DRAW TEXT
            // -----------------------------------------------------

            TextRenderer.DrawText(
                g,
                Text,
                Font,
                textRect,
                textColor,
                flags);
        }


        // =========================================================
        // CREATE ROUNDED PATH
        // =========================================================

        private GraphicsPath CreateRoundedPath(
            Rectangle rect,
            int radius)
        {
            GraphicsPath path =
                new GraphicsPath();


            if (rect.Width <= 0 ||
                rect.Height <= 0)
            {
                return path;
            }


            if (radius <= 0)
            {
                path.AddRectangle(rect);

                return path;
            }


            int diameter =
                radius * 2;


            // -----------------------------------------------------
            // TOP LEFT
            // -----------------------------------------------------

            path.AddArc(
                rect.X,
                rect.Y,
                diameter,
                diameter,
                180,
                90);


            // -----------------------------------------------------
            // TOP RIGHT
            // -----------------------------------------------------

            path.AddArc(
                rect.Right - diameter,
                rect.Y,
                diameter,
                diameter,
                270,
                90);


            // -----------------------------------------------------
            // BOTTOM RIGHT
            // -----------------------------------------------------

            path.AddArc(
                rect.Right - diameter,
                rect.Bottom - diameter,
                diameter,
                diameter,
                0,
                90);


            // -----------------------------------------------------
            // BOTTOM LEFT
            // -----------------------------------------------------

            path.AddArc(
                rect.X,
                rect.Bottom - diameter,
                diameter,
                diameter,
                90,
                90);


            path.CloseFigure();

            return path;
        }


        // =========================================================
        // UPDATE REGION
        // =========================================================

        private void UpdateRegion()
        {
            if (Width <= 0 ||
                Height <= 0)
            {
                return;
            }


            int radius =
                Math.Min(
                    BorderRadius,
                    Math.Min(
                        Width / 2,
                        Height / 2));


            Rectangle rect =
                new Rectangle(
                    0,
                    0,
                    Width,
                    Height);


            using (GraphicsPath path =
                CreateRoundedPath(
                    rect,
                    radius))
            {
                Region oldRegion =
                    Region;

                Region =
                    new Region(path);

                oldRegion?.Dispose();
            }
        }


        // =========================================================
        // RESET MOUSE STATE
        // =========================================================

        public void ResetMouseState()
        {
            isMouseOver =
                false;

            isMouseDown =
                false;

            Invalidate();
        }
    }
}
