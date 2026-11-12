using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using static System.Net.Mime.MediaTypeNames;

namespace DuckFarm.Classes.Controls
{
    public class AppDatePicker : DateTimePicker
    {
        // =========================================================
        // PROPERTY
        // =========================================================

        private int borderRadius = 8;

        [System.ComponentModel.Category("Appearance")]
        [System.ComponentModel.Description(
            "Radius sudut DatePicker.")]
        [System.ComponentModel.DefaultValue(8)]
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

        private Color borderColor =
            Color.FromArgb(209, 213, 219);

        [System.ComponentModel.Category("Appearance")]
        [System.ComponentModel.Description(
            "Warna border DatePicker.")]
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

        private int borderSize = 1;

        [System.ComponentModel.Category("Appearance")]
        [System.ComponentModel.Description(
            "Ukuran border DatePicker.")]
        [System.ComponentModel.DefaultValue(1)]
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
        // FOCUS BORDER COLOR
        // =========================================================

        private Color focusBorderColor =
            Color.FromArgb(37, 99, 235);

        [System.ComponentModel.Category("Appearance")]
        [System.ComponentModel.Description(
            "Warna border ketika DatePicker aktif.")]
        public Color FocusBorderColor
        {
            get
            {
                return focusBorderColor;
            }

            set
            {
                focusBorderColor = value;

                Invalidate();
            }
        }


        // =========================================================
        // PRIVATE STATE
        // =========================================================

        private bool isFocused = false;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public AppDatePicker()
        {
            // -----------------------------------------------------
            // DOUBLE BUFFER
            // -----------------------------------------------------

            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw,
                true);


            DoubleBuffered = true;


            // -----------------------------------------------------
            // FORMAT
            // -----------------------------------------------------

            Format =
                DateTimePickerFormat.Custom;

            CustomFormat =
                "dd/MM/yyyy";


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
            // WARNA
            // -----------------------------------------------------

            BackColor =
                Color.White;

            ForeColor =
                Color.FromArgb(
                    31,
                    41,
                    55);


            // -----------------------------------------------------
            // UKURAN
            // -----------------------------------------------------

            Height = 34;

            Width = 130;


            // -----------------------------------------------------
            // BORDER
            // -----------------------------------------------------

            BorderRadius = 8;

            BorderSize = 1;


            // -----------------------------------------------------
            // EVENTS
            // -----------------------------------------------------

            Enter += AppDatePicker_Enter;

            Leave += AppDatePicker_Leave;


            // -----------------------------------------------------
            // REGION
            // -----------------------------------------------------

            UpdateRegion();
        }


        // =========================================================
        // ENTER
        // =========================================================

        private void AppDatePicker_Enter(
            object sender,
            EventArgs e)
        {
            isFocused = true;

            Invalidate();
        }


        // =========================================================
        // LEAVE
        // =========================================================

        private void AppDatePicker_Leave(
            object sender,
            EventArgs e)
        {
            isFocused = false;

            Invalidate();
        }


        // =========================================================
        // RESIZE
        // =========================================================

        protected override void OnResize(
            EventArgs e)
        {
            base.OnResize(e);

            UpdateRegion();
        }


        // =========================================================
        // PAINT
        // =========================================================

        protected override void OnPaint(
            PaintEventArgs e)
        {
            Graphics graphics =
                e.Graphics;

            graphics.SmoothingMode =
                SmoothingMode.AntiAlias;

            graphics.PixelOffsetMode =
                PixelOffsetMode.HighQuality;


            // -----------------------------------------------------
            // RECTANGLE
            // -----------------------------------------------------

            Rectangle rect =
                new Rectangle(
                    0,
                    0,
                    Width - 1,
                    Height - 1);


            // -----------------------------------------------------
            // BACKGROUND
            // -----------------------------------------------------

            using (GraphicsPath path =
                GetRoundedPath(
                    rect,
                    BorderRadius))
            {
                using (SolidBrush brush =
                    new SolidBrush(
                        BackColor))
                {
                    graphics.FillPath(
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


                if (BorderSize > 0)
                {
                    using (Pen pen =
                        new Pen(
                            currentBorderColor,
                            BorderSize))
                    {
                        graphics.DrawPath(
                            pen,
                            path);
                    }
                }
            }


            // -----------------------------------------------------
            // TEXT
            // -----------------------------------------------------

            Rectangle textRect =
                new Rectangle(
                    10,
                    0,
                    Width - 35,
                    Height);


            TextRenderer.DrawText(
                graphics,
                Text,
                Font,
                textRect,
                ForeColor,
                TextFormatFlags.Left |
                TextFormatFlags.VerticalCenter |
                TextFormatFlags.EndEllipsis);


            // -----------------------------------------------------
            // ARROW
            // -----------------------------------------------------

            DrawArrow(graphics);
        }


        // =========================================================
        // DRAW ARROW
        // =========================================================

        private void DrawArrow(
            Graphics graphics)
        {
            int centerX =
                Width - 18;

            int centerY =
                Height / 2;


            Point[] arrow =
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
                    Color.FromArgb(
                        107,
                        114,
                        128)))
            {
                graphics.FillPolygon(
                    brush,
                    arrow);
            }
        }


        // =========================================================
        // ROUNDED PATH
        // =========================================================

        private GraphicsPath GetRoundedPath(
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

            Rectangle arc =
                new Rectangle(
                    rect.X,
                    rect.Y,
                    diameter,
                    diameter);

            path.AddArc(
                arc,
                180,
                90);


            // -----------------------------------------------------
            // TOP RIGHT
            // -----------------------------------------------------

            arc.X =
                rect.Right - diameter;

            path.AddArc(
                arc,
                270,
                90);


            // -----------------------------------------------------
            // BOTTOM RIGHT
            // -----------------------------------------------------

            arc.Y =
                rect.Bottom - diameter;

            path.AddArc(
                arc,
                0,
                90);


            // -----------------------------------------------------
            // BOTTOM LEFT
            // -----------------------------------------------------

            arc.X =
                rect.X;

            path.AddArc(
                arc,
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


            using (GraphicsPath path =
                GetRoundedPath(
                    new Rectangle(
                        0,
                        0,
                        Width,
                        Height),
                    BorderRadius))
            {
                Region =
                    new Region(path);
            }
        }
    }
}
