using DuckFarm.Classes;
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
    public partial class frmTrnAbsensidanHasilCWR : Form
    {
        public frmTrnAbsensidanHasilCWR()
        {
            InitializeComponent();
        }

        private void frmTrnAbsensidanHasilCWR_Load(object sender, EventArgs e)
        {
            SetRowTableLayoutPanel();
            ApplicationStyle.SetPrimaryButton(btnFind);
            ApplicationStyle.SetPrimaryButton(btnSave);
            ApplicationStyle.SetSuccessButton(btnAdd);
            ApplicationStyle.SetDangerButton(btnDelete);
            ApplicationStyle.SetSecondaryButton(btnCancel);
            ApplicationStyle.SetWarningButton(btnEdit);
            ApplicationStyle.SetDangerButton(btnExit);
        }
        // =========================================================
        // TABLE LAYOUT
        // =========================================================
        private void SetRowTableLayoutPanel()
        {
            tableLayoutPanel1.SuspendLayout();

            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Clear();

            // Row 0: Header (Fixed 195px)
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 170f));

            // Row 1: Body (Expands to fill remaining space)
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            // Row 2: Footer (Fixed 60px)
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60f));

            tableLayoutPanel1.ResumeLayout();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            // Buat form Show.
            var form = new frmTrnAbsensidanHasilCWRShow();

            // Terapkan tema aktif sebelum form ditampilkan.
            ThemeManager.ApplyTheme(form);

            // Ambil DuckMENU sebagai MDI Parent.
            DUCKMENU menu = this.MdiParent as DUCKMENU;

            if (menu == null || menu.MdiManager == null)
            {
                form.Dispose();

                MessageBox.Show(
                    "Tidak dapat membuka form pencarian CWR.",
                    "DuckFarm",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            // Tampilkan sebagai MDI child khusus untuk proses Find.
            menu.MdiManager.ShowModalChild(form);
        }

    }
}
