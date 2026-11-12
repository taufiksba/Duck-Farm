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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace DuckFarm.Classes.Controls
{
    public class AppComboBox : ComboBox
    {
        // =========================================================
        // PRIVATE STATE
        // =========================================================

        private bool isFocused;
        private bool isMouseOver;


        // =========================================================
        // BORDER RADIUS
        // =========================================================

        private int borderRadius = 6;

        [Category("Appearance")]
        [Description("Radius sudut ComboBox.")]
        [DefaultValue(6)]
        public int BorderRadius
        {
            get => borderRadius;

            set
            {
                borderRadius = Math.Max(0, value);
                Invalidate();
            }
        }


        // =========================================================
        // BORDER COLOR
        // =========================================================

        private Color borderColor =
            Color.FromArgb(203, 213, 225);

        [Category("Appearance")]
        [Description("Warna border ComboBox.")]
        public Color BorderColor
        {
            get => borderColor;

            set
            {
                borderColor = value;
                Invalidate();
            }
        }


        // =========================================================
        // FOCUS BORDER COLOR
        // =========================================================

        private Color focusBorderColor =
            Color.FromArgb(37, 99, 235);

        [Category("Appearance")]
        [Description("Warna border ketika ComboBox mendapatkan fokus.")]
        public Color FocusBorderColor
        {
            get => focusBorderColor;

            set
            {
                focusBorderColor = value;
                Invalidate();
            }
        }


        // =========================================================
        // BORDER SIZE
        // =========================================================

        private int borderSize = 1;

        [Category("Appearance")]
        [Description("Ketebalan border ComboBox.")]
        [DefaultValue(1)]
        public int BorderSize
        {
            get => borderSize;

            set
            {
                borderSize = Math.Max(0, value);
                Invalidate();
            }
        }


        // =========================================================
        // FOCUS BORDER SIZE
        // =========================================================

        private int focusBorderSize = 1;

        [Category("Appearance")]
        [Description("Ketebalan border ketika fokus.")]
        [DefaultValue(1)]
        public int FocusBorderSize
        {
            get => focusBorderSize;

            set
            {
                focusBorderSize = Math.Max(0, value);
                Invalidate();
            }
        }


        // =========================================================
        // HORIZONTAL PADDING
        // =========================================================

        private int horizontalPadding = 10;

        [Category("Appearance")]
        [Description("Jarak teks dari sisi kiri ComboBox.")]
        [DefaultValue(10)]
        public int HorizontalPadding
        {
            get => horizontalPadding;

            set
            {
                horizontalPadding = Math.Max(0, value);

                Invalidate();
            }
        }


        // =========================================================
        // ARROW COLOR
        // =========================================================

        private Color arrowColor =
            Color.FromArgb(71, 85, 105);

        [Category("Appearance")]
        [Description("Warna panah dropdown.")]
        public Color ArrowColor
        {
            get => arrowColor;

            set
            {
                arrowColor = value;
                Invalidate();
            }
        }


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public AppComboBox()
        {
            // -----------------------------------------------------
            // CUSTOM PAINT
            // -----------------------------------------------------

            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw,
                true);

            DoubleBuffered = true;


            // -----------------------------------------------------
            // FONT
            // -----------------------------------------------------

            //Font =
            //    new Font(
            //        "Segoe UI",
            //        9F,
            //        FontStyle.Regular);

            Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Bold);


            // -----------------------------------------------------
            // COLORS
            // -----------------------------------------------------

            BackColor =
                Color.White;

            ForeColor =
                Color.FromArgb(
                    30,
                    41,
                    59);


            // -----------------------------------------------------
            // BORDER
            // -----------------------------------------------------

            BorderColor =
                Color.FromArgb(
                    203,
                    213,
                    225);

            FocusBorderColor =
                Color.FromArgb(
                    37,
                    99,
                    235);


            // -----------------------------------------------------
            // STYLE
            // -----------------------------------------------------

            FlatStyle =
                FlatStyle.Flat;

            DropDownStyle =
                ComboBoxStyle.DropDownList;


            // -----------------------------------------------------
            // SIZE
            // -----------------------------------------------------

            Height = 34;

            MinimumSize =
                new Size(
                    80,
                    32);


            // -----------------------------------------------------
            // RADIUS
            // -----------------------------------------------------

            BorderRadius = 6;


            // -----------------------------------------------------
            // DRAW MODE
            // -----------------------------------------------------

            DrawMode =
                DrawMode.OwnerDrawFixed;

            ItemHeight = 28;
        }


        // =========================================================
        // RESIZE
        // =========================================================

        protected override void OnResize(
            EventArgs e)
        {
            base.OnResize(e);

            Invalidate();
        }


        // =========================================================
        // ENTER
        // =========================================================

        protected override void OnEnter(
            EventArgs e)
        {
            base.OnEnter(e);

            isFocused = true;

            Invalidate();
        }


        // =========================================================
        // LEAVE
        // =========================================================

        protected override void OnLeave(
            EventArgs e)
        {
            base.OnLeave(e);

            isFocused = false;

            Invalidate();
        }


        // =========================================================
        // MOUSE ENTER
        // =========================================================

        protected override void OnMouseEnter(
            EventArgs e)
        {
            base.OnMouseEnter(e);

            isMouseOver = true;

            Invalidate();
        }


        // =========================================================
        // MOUSE LEAVE
        // =========================================================

        protected override void OnMouseLeave(
            EventArgs e)
        {
            base.OnMouseLeave(e);

            isMouseOver = false;

            Invalidate();
        }


        // =========================================================
        // DRAW ITEM
        // =========================================================

        protected override void OnDrawItem(
            DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            Graphics g =
                e.Graphics;

            g.SmoothingMode =
                SmoothingMode.AntiAlias;

            g.PixelOffsetMode =
                PixelOffsetMode.HighQuality;


            Color backgroundColor =
                BackColor;

            Color textColor =
                ForeColor;


            if ((e.State &
                 DrawItemState.Selected)
                 == DrawItemState.Selected)
            {
                backgroundColor =
                    Color.FromArgb(
                        239,
                        246,
                        255);
            }


            using (SolidBrush brush =
                new SolidBrush(
                    backgroundColor))
            {
                g.FillRectangle(
                    brush,
                    e.Bounds);
            }


            string text =
                GetItemText(
                    Items[e.Index]);


            Rectangle textRect =
                new Rectangle(
                    e.Bounds.X + 10,
                    e.Bounds.Y,
                    e.Bounds.Width - 20,
                    e.Bounds.Height);


            TextRenderer.DrawText(
                g,
                text,
                Font,
                textRect,
                textColor,
                TextFormatFlags.Left |
                TextFormatFlags.VerticalCenter |
                TextFormatFlags.EndEllipsis |
                TextFormatFlags.NoPrefix);
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
            // GRAPHICS QUALITY
            // -----------------------------------------------------

            g.SmoothingMode =
                SmoothingMode.AntiAlias;

            g.PixelOffsetMode =
                PixelOffsetMode.HighQuality;

            g.CompositingQuality =
                CompositingQuality.HighQuality;


            // -----------------------------------------------------
            // CLEAR
            // -----------------------------------------------------

            g.Clear(
                Parent?.BackColor ??
                SystemColors.Control);


            // -----------------------------------------------------
            // RECTANGLE
            // -----------------------------------------------------

            Rectangle rect =
                new Rectangle(
                    1,
                    1,
                    Width - 2,
                    Height - 2);


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
            // PATH
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
                    new SolidBrush(
                        BackColor))
                {
                    g.FillPath(
                        brush,
                        path);
                }


                // -------------------------------------------------
                // BORDER
                // -------------------------------------------------

                Color currentBorderColor =
                    isFocused
                        ? FocusBorderColor
                        : BorderColor;


                int currentBorderSize =
                    isFocused
                        ? FocusBorderSize
                        : BorderSize;


                if (currentBorderSize > 0)
                {
                    using (Pen pen =
                        new Pen(
                            currentBorderColor,
                            currentBorderSize))
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
            // SELECTED TEXT
            // -----------------------------------------------------

            string selectedText =
                Text;


            Rectangle textRect =
                new Rectangle(
                    HorizontalPadding,
                    0,
                    Width -
                    HorizontalPadding -
                    40,
                    Height);


            TextRenderer.DrawText(
                g,
                selectedText,
                Font,
                textRect,
                ForeColor,
                TextFormatFlags.Left |
                TextFormatFlags.VerticalCenter |
                TextFormatFlags.EndEllipsis |
                TextFormatFlags.NoPrefix);


            // -----------------------------------------------------
            // DROPDOWN ARROW
            // -----------------------------------------------------

            DrawArrow(g);
        }


        // =========================================================
        // DRAW ARROW
        // =========================================================

        private void DrawArrow(
            Graphics g)
        {
            int arrowWidth = 14;

            int centerX =
                Width - 20;

            int centerY =
                Height / 2;


            Point[] points =
            {
                new Point(
                    centerX - 4,
                    centerY - 2),

                new Point(
                    centerX + 4,
                    centerY - 2),

                new Point(
                    centerX,
                    centerY + 3)
            };


            using (SolidBrush brush =
                new SolidBrush(
                    ArrowColor))
            {
                g.FillPolygon(
                    brush,
                    points);
            }
        }


        // =========================================================
        // ROUNDED PATH
        // =========================================================

        private GraphicsPath CreateRoundedPath(
            Rectangle rect,
            int radius)
        {
            GraphicsPath path =
                new GraphicsPath();


            if (radius <= 0)
            {
                path.AddRectangle(rect);

                return path;
            }


            int diameter =
                radius * 2;


            if (diameter > rect.Width)
            {
                diameter =
                    rect.Width;
            }


            if (diameter > rect.Height)
            {
                diameter =
                    rect.Height;
            }


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
    }
}
