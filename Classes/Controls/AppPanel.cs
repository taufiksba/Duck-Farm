using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuckFarm.Classes.Controls
{
    public class AppPanel : Panel
    {
        // =========================================================
        // PRIVATE FIELD
        // =========================================================

        private int borderRadius = 10;
        private int borderSize = 2; // Diubah ke 2 agar border bawaan lebih tegas/jelas

        private Color borderColor = Color.FromArgb(148, 163, 184); // Warna Slate-400 (Sudah cukup kontras)
        private Color panelBackColor = Color.White;

        // ---------------------------------------------------------
        // SHADOW
        // ---------------------------------------------------------
        private bool shadowEnabled = false;
        private int shadowSize = 4;
        private Color shadowColor = Color.FromArgb(40, 0, 0, 0);


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public AppPanel()
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
            // DEFAULT
            // -----------------------------------------------------
            base.BackColor = Color.Transparent; // Menggunakan transparan agar tidak double border di ujung luar

            AutoScroll = true;
            Padding = new Padding(16);
            Margin = new Padding(0);
        }


        // =========================================================
        // PROPERTY MANAGEMENT
        // =========================================================

        [Category("AppPanel")]
        [Description("Radius sudut panel.")]
        [DefaultValue(10)]
        public int BorderRadius
        {
            get => borderRadius;
            set { borderRadius = Math.Max(0, value); Invalidate(); }
        }

        [Category("AppPanel")]
        [Description("Ukuran border panel.")]
        [DefaultValue(2)]
        public int BorderSize
        {
            get => borderSize;
            set { borderSize = Math.Max(0, value); Invalidate(); }
        }

        [Category("AppPanel")]
        [Description("Warna border panel.")]
        public Color BorderColor
        {
            get => borderColor;
            set { borderColor = value; Invalidate(); }
        }

        [Category("AppPanel")]
        [Description("Warna background panel.")]
        public Color PanelBackColor
        {
            get => panelBackColor;
            set { panelBackColor = value; Invalidate(); }
        }

        [Category("AppPanel")]
        [Description("Mengaktifkan shadow halus pada panel.")]
        [DefaultValue(false)]
        public bool ShadowEnabled
        {
            get => shadowEnabled;
            set { shadowEnabled = value; Invalidate(); }
        }

        [Category("AppPanel")]
        [Description("Ukuran shadow panel.")]
        [DefaultValue(4)]
        public int ShadowSize
        {
            get => shadowSize;
            set { shadowSize = Math.Max(0, value); Invalidate(); }
        }

        [Category("AppPanel")]
        [Description("Warna shadow panel.")]
        public Color ShadowColor
        {
            get => shadowColor;
            set { shadowColor = value; Invalidate(); }
        }


        // =========================================================
        // OVERRIDES
        // =========================================================

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            Invalidate();
        }


        // =========================================================
        // PAINT
        // =========================================================

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // -----------------------------------------------------
            // QUALITY SETTINGS
            // -----------------------------------------------------
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;

            // Bersihkan background total menggunakan warna container induk (Parent)
            g.Clear(Parent?.BackColor ?? SystemColors.Control);

            if (Width <= 0 || Height <= 0) return;

            // -----------------------------------------------------
            // PERBAIKAN UTAMA: ATASI PERGESERAN KORDINAT SCROLL
            // -----------------------------------------------------
            // Simpan status grafik sebelum ditranslasi
            GraphicsState state = g.Save();

            // Geser titik koordinat (0,0) mengikuti posisi ScrollBar saat ini
            g.TranslateTransform(AutoScrollPosition.X, AutoScrollPosition.Y);

            // Gunakan ukuran fisik komponen dasar agar background & border 
            // tidak ikut terpotong atau bergeser saat di-scroll
            int totalWidth = ClientRectangle.Width;
            int totalHeight = ClientRectangle.Height;

            // Jika komponen di dalamnya memanjang ke bawah melebihi batas, 
            // sesuaikan tinggi gambar agar background mengikuti tinggi konten asli (Scroll)
            if (AutoScroll && DisplayRectangle.Height > totalHeight)
            {
                totalHeight = DisplayRectangle.Height;
            }
            if (AutoScroll && DisplayRectangle.Width > totalWidth)
            {
                totalWidth = DisplayRectangle.Width;
            }

            // -----------------------------------------------------
            // SHADOW DRAWING
            // -----------------------------------------------------
            if (ShadowEnabled && shadowSize > 0)
            {
                // Panggil metode DrawShadow yang disesuaikan
                DrawShadowInternal(g, totalWidth, totalHeight);
            }

            // -----------------------------------------------------
            // PANEL RECTANGLE CALCULATION
            // -----------------------------------------------------
            int offset = ShadowEnabled ? shadowSize : 0;
            int marginCorrection = (int)Math.Ceiling((double)BorderSize / 2);

            Rectangle rect = new Rectangle(
                marginCorrection,
                marginCorrection,
                totalWidth - offset - (marginCorrection * 2),
                totalHeight - offset - (marginCorrection * 2)
            );

            if (rect.Width <= 0 || rect.Height <= 0)
            {
                g.Restore(state);
                return;
            }

            int radius = Math.Min(BorderRadius, Math.Min(rect.Width / 2, rect.Height / 2));

            // -----------------------------------------------------
            // DRAW BODY & BORDER
            // -----------------------------------------------------
            using (GraphicsPath path = CreateRoundedPath(rect, radius))
            {
                // Isi warna Background Panel
                using (SolidBrush brush = new SolidBrush(PanelBackColor))
                {
                    g.FillPath(brush, path);
                }

                // Gambar Border Tegas
                if (BorderSize > 0 && BorderColor != Color.Transparent)
                {
                    using (Pen pen = new Pen(BorderColor, BorderSize))
                    {
                        pen.Alignment = PenAlignment.Center;
                        g.DrawPath(pen, path);
                    }
                }
            }

            // Kembalikan status grafik semula
            g.Restore(state);
        }

        // Ubah parameter DrawShadow agar menerima parameter dinamis lebar & tinggi
        private void DrawShadowInternal(Graphics g, int targetWidth, int targetHeight)
        {
            int size = Math.Min(shadowSize, Math.Min(targetWidth / 4, targetHeight / 4));
            if (size <= 0) return;

            int radius = Math.Min(BorderRadius, Math.Min(targetWidth / 2, targetHeight / 2));

            for (int i = size; i >= 1; i--)
            {
                float ratio = (float)i / size;
                int alpha = (int)(shadowColor.A * ratio * 0.25f);

                if (alpha <= 0) continue;

                Color color = Color.FromArgb(alpha, shadowColor.R, shadowColor.G, shadowColor.B);

                Rectangle shadowRect = new Rectangle(
                    (i / 2) + 1,
                    (i / 2) + 2,
                    Math.Max(1, targetWidth - shadowSize),
                    Math.Max(1, targetHeight - shadowSize)
                );

                int shadowRadius = Math.Min(radius, Math.Min(shadowRect.Width / 2, shadowRect.Height / 2));

                using (GraphicsPath shadowPath = CreateRoundedPath(shadowRect, shadowRadius))
                {
                    using (SolidBrush brush = new SolidBrush(color))
                    {
                        g.FillPath(brush, shadowPath);
                    }
                }
            }
        }


        // =========================================================
        // CREATE ROUNDED PATH
        // =========================================================

        private GraphicsPath CreateRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            if (rect.Width <= 0 || rect.Height <= 0) return path;

            if (radius <= 0)
            {
                path.AddRectangle(rect);
                return path;
            }

            int diameter = radius * 2;

            // Top Left
            path.AddArc(new Rectangle(rect.X, rect.Y, diameter, diameter), 180, 90);

            // Top Right
            path.AddArc(new Rectangle(rect.Right - diameter, rect.Y, diameter, diameter), 270, 90);

            // Bottom Right
            path.AddArc(new Rectangle(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter), 0, 90);

            // Bottom Left
            path.AddArc(new Rectangle(rect.X, rect.Bottom - diameter, diameter, diameter), 90, 90); path.CloseFigure(); return path;
        }
    }
}