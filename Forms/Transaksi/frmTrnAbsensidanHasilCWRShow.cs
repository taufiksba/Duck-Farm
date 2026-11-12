using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuckFarm.Forms.Transaksi
{
    public partial class frmTrnAbsensidanHasilCWRShow : Form
    {
        public frmTrnAbsensidanHasilCWRShow()
        {
            InitializeComponent();
        }

        private void frmTrnAbsensidanHasilCWRShow_Load(object sender, EventArgs e)
        {
            SetRowTableLayoutPanel();
        }
        //private void SetRowTableLayoutPanel()
        //{
        //    tableLayoutPanel1.RowStyles[0].SizeType =
        //        SizeType.Absolute;

        //    tableLayoutPanel1.RowStyles[0].Height = 145;

        //    tableLayoutPanel1.RowStyles[1].SizeType =
        //        SizeType.Percent;

        //    tableLayoutPanel1.RowStyles[1].Height = 100;

        //    tableLayoutPanel1.RowStyles[2].SizeType =
        //        SizeType.Absolute;

        //    tableLayoutPanel1.RowStyles[2].Height = 60;
        //}
        private void SetRowTableLayoutPanel()
        {
            tableLayoutPanel1.SuspendLayout();

            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Clear();

            // Row 0: Header (Fixed 195px)
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 145f));

            // Row 1: Body (Expands to fill remaining space)
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            // Row 2: Footer (Fixed 60px)
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60f));

            tableLayoutPanel1.ResumeLayout();
        }
    }
}
