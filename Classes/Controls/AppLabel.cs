using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;


namespace DuckFarm.Classes.Controls
{
    public class AppLabel : Label
    {
        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public AppLabel()
        {
            // -----------------------------------------------------
            // FONT
            // -----------------------------------------------------

            Font =
                new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Regular);


            // -----------------------------------------------------
            // WARNA
            // -----------------------------------------------------

            ForeColor =
                Color.FromArgb(
                    51,
                    65,
                    85);


            // -----------------------------------------------------
            // BACKGROUND
            // -----------------------------------------------------

            BackColor =
                Color.Transparent;


            // -----------------------------------------------------
            // UKURAN
            // -----------------------------------------------------

            AutoSize = true;


            // -----------------------------------------------------
            // TEXT RENDERING
            // -----------------------------------------------------

            UseCompatibleTextRendering = false;


            // -----------------------------------------------------
            // ALIGNMENT
            // -----------------------------------------------------

            TextAlign =
                ContentAlignment.MiddleLeft;
        }


        // =========================================================
        // BOLD
        // =========================================================

        [Category("Appearance")]
        [Description("Menjadikan teks label lebih tebal.")]
        [DefaultValue(false)]
        public bool IsBold
        {
            get
            {
                return Font.Bold;
            }

            set
            {
                Font =
                    new Font(
                        Font.FontFamily,
                        Font.Size,
                        value
                            ? FontStyle.Bold
                            : FontStyle.Regular);

                Invalidate();
            }
        }


        // =========================================================
        // FONT SIZE
        // =========================================================

        [Category("Appearance")]
        [Description("Ukuran font label.")]
        [DefaultValue(9F)]
        public float FontSize
        {
            get
            {
                return Font.Size;
            }

            set
            {
                if (value <= 0)
                    return;

                Font =
                    new Font(
                        Font.FontFamily,
                        value,
                        Font.Style);

                Invalidate();
            }
        }


        // =========================================================
        // PADDING
        // =========================================================

        private int horizontalPadding = 0;

        [Category("Layout")]
        [Description("Jarak horizontal teks label.")]
        [DefaultValue(0)]
        public int HorizontalPadding
        {
            get
            {
                return horizontalPadding;
            }

            set
            {
                horizontalPadding =
                    Math.Max(0, value);

                Padding =
                    new Padding(
                        horizontalPadding,
                        0,
                        horizontalPadding,
                        0);
            }
        }


        // =========================================================
        // DISPOSE FONT
        // =========================================================

        protected override void Dispose(
            bool disposing)
        {
            if (disposing)
            {
                Font?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
