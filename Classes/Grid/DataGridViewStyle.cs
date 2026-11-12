using System.Drawing;
using System.Windows.Forms;

namespace DuckFarm.Classes.Grid
{
    public static class DataGridViewStyle
    {
        // =========================================================
        // STANDARD COLOR
        // =========================================================

        private static readonly Color HeaderColor =
            Color.FromArgb(43, 84, 130);

        private static readonly Color HeaderTextColor =
            Color.White;

        private static readonly Color NormalRowColor =
            Color.White;

        private static readonly Color AlternateRowColor =
            Color.FromArgb(245, 246, 248);

        private static readonly Color RowTextColor =
            Color.FromArgb(45, 45, 45);

        private static readonly Color SelectionColor =
            Color.FromArgb(91, 143, 190);

        private static readonly Color GridLineColor =
            Color.FromArgb(225, 225, 225);


        // =========================================================
        // APPLY STANDARD STYLE
        // =========================================================

        public static void Apply(
            DataGridView grid, bool autoGenerateColumns = false)
        {
            if (grid == null)
                return;


            // =====================================================
            // DATA
            // =====================================================

            // Gunakan hanya kolom yang dibuat di Designer.
            //grid.AutoGenerateColumns = false;
            grid.AutoGenerateColumns = autoGenerateColumns;

            // Data tidak dapat diedit langsung.
            grid.ReadOnly = true;

            // User memilih satu baris penuh.
            grid.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            grid.MultiSelect = false;


            // =====================================================
            // ROW HEADER
            // =====================================================

            grid.RowHeadersVisible = true;

            grid.RowHeadersWidth = 50;


            // =====================================================
            // HEADER VISUAL
            // =====================================================

            grid.EnableHeadersVisualStyles = false;


            // =====================================================
            // COLUMN HEADER
            // =====================================================

            grid.ColumnHeadersDefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor = HeaderColor,
                    ForeColor = HeaderTextColor,

                    Font = new Font(
                        grid.Font,
                        FontStyle.Bold),

                    Alignment =
                        DataGridViewContentAlignment.MiddleCenter,

                    SelectionBackColor = HeaderColor,
                    SelectionForeColor = HeaderTextColor
                };


            // =====================================================
            // ROW HEADER
            // =====================================================

            grid.RowHeadersDefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor = HeaderColor,
                    ForeColor = HeaderTextColor,

                    Font = new Font(
                        grid.Font,
                        FontStyle.Bold),

                    Alignment =
                        DataGridViewContentAlignment.MiddleCenter,

                    SelectionBackColor = HeaderColor,
                    SelectionForeColor = HeaderTextColor
                };


            // =====================================================
            // TOP LEFT HEADER
            // =====================================================

            grid.TopLeftHeaderCell.Value = "No";

            grid.TopLeftHeaderCell.Style =
                new DataGridViewCellStyle
                {
                    BackColor = HeaderColor,
                    ForeColor = HeaderTextColor,

                    Font = new Font(
                        grid.Font,
                        FontStyle.Bold),

                    Alignment =
                        DataGridViewContentAlignment.MiddleCenter
                };


            // =====================================================
            // NORMAL ROW
            // =====================================================

            grid.RowsDefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor = NormalRowColor,
                    ForeColor = RowTextColor,

                    SelectionBackColor = SelectionColor,
                    SelectionForeColor = HeaderTextColor,

                    Alignment =
                        DataGridViewContentAlignment.MiddleLeft
                };


            // =====================================================
            // ALTERNATE / ZEBRA ROW
            // =====================================================

            grid.AlternatingRowsDefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor = AlternateRowColor,
                    ForeColor = RowTextColor,

                    SelectionBackColor = SelectionColor,
                    SelectionForeColor = HeaderTextColor,

                    Alignment =
                        DataGridViewContentAlignment.MiddleLeft
                };


            // =====================================================
            // GRID BORDER
            // =====================================================

            grid.CellBorderStyle =
                DataGridViewCellBorderStyle.SingleHorizontal;

            grid.GridColor =
                GridLineColor;


            // =====================================================
            // HEADER HEIGHT
            // =====================================================

            grid.ColumnHeadersHeight = 50;

            grid.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode
                    .DisableResizing;


            // =====================================================
            // ROW HEIGHT
            // =====================================================

            grid.RowTemplate.Height = 28;


            // =====================================================
            // USER ACTION
            // =====================================================

            // Tidak boleh menambah baris dari Grid.
            grid.AllowUserToAddRows = false;

            // Tidak boleh menghapus baris dari Grid.
            grid.AllowUserToDeleteRows = false;

            // Tidak boleh mengubah tinggi baris.
            grid.AllowUserToResizeRows = false;
        }
    }
}