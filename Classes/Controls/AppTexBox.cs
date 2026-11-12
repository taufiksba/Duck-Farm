using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DuckFarm.Classes.Controls
{
    public class AppTextBox : TextBox
    {
        // =========================================================
        // BORDER RADIUS
        // =========================================================

        private int borderRadius = 6;

        [Category("Appearance")]
        [Description("Radius sudut TextBox.")]
        [DefaultValue(6)]
        public int BorderRadius
        {
            get => borderRadius;

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
            Color.FromArgb(148, 163, 184);

        [Category("Appearance")]
        [Description("Warna border TextBox.")]
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
        [Description("Warna border ketika TextBox mendapatkan fokus.")]
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
        [Description("Ketebalan border TextBox.")]
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
        [Description("Ketebalan border ketika TextBox mendapatkan fokus.")]
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

        private int horizontalPadding = 14;

        [Category("Appearance")]
        [Description("Jarak teks dari sisi kiri dan kanan.")]
        [DefaultValue(14)]
        public int HorizontalPadding
        {
            get => horizontalPadding;

            set
            {
                horizontalPadding = Math.Max(0, value);

                UpdatePadding();
                Invalidate();
            }
        }


        // =========================================================
        // VERTICAL PADDING
        // =========================================================

        private int verticalPadding = 5;

        [Category("Appearance")]
        [Description("Jarak teks dari sisi atas dan bawah.")]
        [DefaultValue(5)]
        public int VerticalPadding
        {
            get => verticalPadding;

            set
            {
                verticalPadding = Math.Max(0, value);

                UpdatePadding();
                Invalidate();
            }
        }


        // =========================================================
        // LOCKED
        // =========================================================
        //
        // Locked = true
        //
        // - User tidak dapat mengetik
        // - Text tetap dapat dibaca
        // - Text tetap dapat di-copy
        // - Warna berubah menjadi abu-abu halus
        //
        // Cocok untuk:
        // - ID otomatis
        // - Nomor dokumen
        // - Kode otomatis
        // - Field hasil sistem
        //
        // =========================================================

        private bool locked = false;

        [Category("Behavior")]
        [Description("Mengunci TextBox agar user tidak dapat mengubah isi.")]
        [DefaultValue(false)]
        public bool Locked
        {
            get => locked;

            set
            {
                locked = value;

                ApplyLockedStyle();

                Invalidate();
            }
        }


        // =========================================================
        // LOCK BACK COLOR
        // =========================================================

        private Color lockedBackColor =
            Color.FromArgb(241, 245, 249);

        [Category("Appearance")]
        [Description("Warna background ketika TextBox dikunci.")]
        public Color LockedBackColor
        {
            get => lockedBackColor;

            set
            {
                lockedBackColor = value;

                ApplyLockedStyle();
                Invalidate();
            }
        }


        // =========================================================
        // LOCK FORE COLOR
        // =========================================================

        private Color lockedForeColor =
            Color.FromArgb(100, 116, 139);

        [Category("Appearance")]
        [Description("Warna teks ketika TextBox dikunci.")]
        public Color LockedForeColor
        {
            get => lockedForeColor;

            set
            {
                lockedForeColor = value;

                ApplyLockedStyle();
                Invalidate();
            }
        }


        // =========================================================
        // LOCK BORDER COLOR
        // =========================================================

        private Color lockedBorderColor =
            Color.FromArgb(203, 213, 225);

        [Category("Appearance")]
        [Description("Warna border ketika TextBox dikunci.")]
        public Color LockedBorderColor
        {
            get => lockedBorderColor;

            set
            {
                lockedBorderColor = value;

                Invalidate();
            }
        }


        // =========================================================
        // PRIVATE STATE
        // =========================================================

        private bool isFocused;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public AppTextBox()
        {
            // =====================================================
            // NATIVE WINDOWS PAINT
            // =====================================================

            SetStyle(
                ControlStyles.UserPaint,
                false);

            SetStyle(
                ControlStyles.AllPaintingInWmPaint,
                false);

            SetStyle(
                ControlStyles.OptimizedDoubleBuffer,
                false);

            SetStyle(
                ControlStyles.ResizeRedraw,
                true);


            // =====================================================
            // FONT
            // =====================================================

            Font =
                new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Regular);


            // =====================================================
            // BACKGROUND
            // =====================================================

            base.BackColor =
                Color.White;


            // =====================================================
            // TEXT COLOR
            // =====================================================

            base.ForeColor =
                Color.FromArgb(
                    30,
                    41,
                    59);


            // =====================================================
            // BORDER
            // =====================================================

            borderColor =
                Color.FromArgb(
                    148,
                    163,
                    184);


            focusBorderColor =
                Color.FromArgb(
                    37,
                    99,
                    235);


            lockedBorderColor =
                Color.FromArgb(
                    203,
                    213,
                    225);


            // =====================================================
            // SIZE
            // =====================================================

            Height = 34;

            MinimumSize =
                new Size(
                    50,
                    32);


            // =====================================================
            // RADIUS
            // =====================================================

            borderRadius = 6;


            // =====================================================
            // PADDING
            // =====================================================

            horizontalPadding = 14;
            verticalPadding = 5;

            UpdatePadding();


            // =====================================================
            // REMOVE NATIVE BORDER
            // =====================================================

            BorderStyle =
                BorderStyle.None;


            // =====================================================
            // REGION
            // =====================================================

            UpdateRegion();


            // =====================================================
            // INITIAL LOCK STYLE
            // =====================================================

            ApplyLockedStyle();
        }


        // =========================================================
        // CREATE PARAMS
        // =========================================================

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp =
                    base.CreateParams;

                // Remove WS_BORDER
                cp.Style &= ~0x00800000;

                return cp;
            }
        }


        // =========================================================
        // RESIZE
        // =========================================================

        protected override void OnResize(
            EventArgs e)
        {
            base.OnResize(e);

            UpdateRegion();

            UpdatePadding();

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
        // ENABLED CHANGED
        // =========================================================

        protected override void OnEnabledChanged(
            EventArgs e)
        {
            base.OnEnabledChanged(e);

            Invalidate();
        }


        // =========================================================
        // BACK COLOR CHANGED
        // =========================================================

        protected override void OnBackColorChanged(
            EventArgs e)
        {
            base.OnBackColorChanged(e);

            Invalidate();
        }


        // =========================================================
        // FORE COLOR CHANGED
        // =========================================================

        protected override void OnForeColorChanged(
            EventArgs e)
        {
            base.OnForeColorChanged(e);

            Invalidate();
        }


        // =========================================================
        // FONT CHANGED
        // =========================================================

        protected override void OnFontChanged(
            EventArgs e)
        {
            base.OnFontChanged(e);

            UpdatePadding();

            Invalidate();
        }


        // =========================================================
        // PADDING
        // =========================================================

        private void UpdatePadding()
        {
            int horizontal =
                Math.Max(
                    0,
                    HorizontalPadding);

            int vertical =
                Math.Max(
                    0,
                    VerticalPadding);

            Padding =
                new Padding(
                    horizontal,
                    vertical,
                    horizontal,
                    vertical);
        }


        // =========================================================
        // LOCK STYLE
        // =========================================================

        private void ApplyLockedStyle()
        {
            if (locked)
            {
                base.ReadOnly = true;

                base.BackColor =
                    LockedBackColor;

                base.ForeColor =
                    LockedForeColor;
            }
            else
            {
                base.ReadOnly = false;

                base.BackColor =
                    Color.White;

                base.ForeColor =
                    Color.FromArgb(
                        30,
                        41,
                        59);
            }

            Invalidate();
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
                Region newRegion =
                    new Region(path);


                Region oldRegion =
                    Region;


                Region =
                    newRegion;


                oldRegion?.Dispose();
            }
        }


        // =========================================================
        // WND PROC
        // =========================================================

        protected override void WndProc(
            ref Message m)
        {
            const int WM_PAINT = 0x000F;

            base.WndProc(ref m);


            if (m.Msg == WM_PAINT)
            {
                DrawBorder();
            }
        }


        // =========================================================
        // DRAW BORDER
        // =========================================================

        private void DrawBorder()
        {
            if (Width <= 0 ||
                Height <= 0)
            {
                return;
            }


            using (Graphics g =
                   Graphics.FromHwnd(Handle))
            {
                // =================================================
                // GRAPHICS QUALITY
                // =================================================

                g.SmoothingMode =
                    SmoothingMode.AntiAlias;

                g.PixelOffsetMode =
                    PixelOffsetMode.HighQuality;

                g.CompositingQuality =
                    CompositingQuality.HighQuality;


                // =================================================
                // RECTANGLE
                // =================================================

                Rectangle rect =
                    new Rectangle(
                        0,
                        0,
                        Width - 1,
                        Height - 1);


                if (rect.Width <= 0 ||
                    rect.Height <= 0)
                {
                    return;
                }


                // =================================================
                // RADIUS
                // =================================================

                int radius =
                    Math.Min(
                        BorderRadius,
                        Math.Min(
                            rect.Width / 2,
                            rect.Height / 2));


                // =================================================
                // BORDER COLOR
                // =================================================

                Color currentBorderColor;


                if (Locked)
                {
                    currentBorderColor =
                        LockedBorderColor;
                }
                else
                {
                    currentBorderColor =
                        isFocused
                            ? FocusBorderColor
                            : BorderColor;
                }


                // =================================================
                // BORDER SIZE
                // =================================================

                int currentBorderSize =
                    isFocused && !Locked
                        ? FocusBorderSize
                        : BorderSize;


                // =================================================
                // DRAW
                // =================================================

                if (currentBorderSize > 0 &&
                    currentBorderColor != Color.Transparent)
                {
                    using (GraphicsPath path =
                           CreateRoundedPath(
                               rect,
                               radius))
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


            // =====================================================
            // NORMAL RECTANGLE
            // =====================================================

            if (radius <= 0)
            {
                path.AddRectangle(rect);

                return path;
            }


            // =====================================================
            // DIAMETER
            // =====================================================

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


            // =====================================================
            // TOP LEFT
            // =====================================================

            path.AddArc(
                rect.X,
                rect.Y,
                diameter,
                diameter,
                180,
                90);


            // =====================================================
            // TOP RIGHT
            // =====================================================

            path.AddArc(
                rect.Right - diameter,
                rect.Y,
                diameter,
                diameter,
                270,
                90);


            // =====================================================
            // BOTTOM RIGHT
            // =====================================================

            path.AddArc(
                rect.Right - diameter,
                rect.Bottom - diameter,
                diameter,
                diameter,
                0,
                90);


            // =====================================================
            // BOTTOM LEFT
            // =====================================================

            path.AddArc(
                rect.X,
                rect.Bottom - diameter,
                diameter,
                diameter,
                90,
                90);


            // =====================================================
            // CLOSE
            // =====================================================

            path.CloseFigure();


            return path;
        }


        // =========================================================
        // CLEAR
        // =========================================================
        //
        // Kompatibel dengan kode lama:
        //
        // txtUsername.Clear();
        // txtPassword.Clear();
        //
        // =========================================================

        public new void Clear()
        {
            base.Clear();
        }


        // =========================================================
        // SELECT ALL
        // =========================================================

        public new void SelectAll()
        {
            base.SelectAll();
        }


        // =========================================================
        // APPEND TEXT
        // =========================================================

        public new void AppendText(
            string text)
        {
            base.AppendText(text);
        }
    }
}