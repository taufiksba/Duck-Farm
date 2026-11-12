using System;
using System.Drawing;
using System.Windows.Forms;
using Krypton.Toolkit;
using DuckFarm.Classes;

namespace DuckFarm.Forms.Development
{
    /// <summary>
    /// Form uji terpisah untuk memastikan kontrol Krypton mengikuti
    /// Light/Dark palette tanpa mengganti kontrol lama DuckFarm.
    /// </summary>
    public class frmKryptonThemeDemo : Form
    {
        private readonly KryptonLabel lblTitle;
        private readonly KryptonLabel lblDescription;
        private readonly KryptonTextBox txtExample;
        private readonly KryptonComboBox cboExample;
        private readonly KryptonButton btnTest;
        private readonly KryptonButton btnClose;

        public frmKryptonThemeDemo()
        {
            Text = "DuckFarm - Krypton Theme Demo";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(720, 440);
            MinimumSize = new Size(640, 380);
            Font = new Font("Segoe UI", 9F);

            KryptonPanel contentPanel = new KryptonPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(28)
            };

            lblTitle = new KryptonLabel
            {
                AutoSize = true,
                Location = new Point(28, 28),
                Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold),
                Text = "Krypton Toolkit"
            };

            lblDescription = new KryptonLabel
            {
                AutoSize = true,
                Location = new Point(30, 78),
                Text = "Kontrol pada form ini seharusnya mengikuti Light/Dark Mode aplikasi."
            };

            KryptonLabel lblText = new KryptonLabel
            {
                AutoSize = true,
                Location = new Point(30, 142),
                Text = "Contoh TextBox"
            };

            txtExample = new KryptonTextBox
            {
                Location = new Point(30, 168),
                Size = new Size(300, 32),
                Text = "DuckFarm ERP"
            };

            KryptonLabel lblCombo = new KryptonLabel
            {
                AutoSize = true,
                Location = new Point(30, 222),
                Text = "Contoh ComboBox"
            };

            cboExample = new KryptonComboBox
            {
                Location = new Point(30, 248),
                Size = new Size(300, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboExample.Items.AddRange(new object[] { "Inventory", "Livestock", "Production" });
            cboExample.SelectedIndex = 0;

            btnTest = new KryptonButton
            {
                Location = new Point(30, 310),
                Size = new Size(150, 38),
                Text = "Test Button"
            };
            btnTest.Click += BtnTest_Click;

            btnClose = new KryptonButton
            {
                Location = new Point(194, 310),
                Size = new Size(136, 38),
                Text = "Tutup"
            };
            btnClose.Click += (sender, e) => Close();

            contentPanel.Controls.Add(lblTitle);
            contentPanel.Controls.Add(lblDescription);
            contentPanel.Controls.Add(lblText);
            contentPanel.Controls.Add(txtExample);
            contentPanel.Controls.Add(lblCombo);
            contentPanel.Controls.Add(cboExample);
            contentPanel.Controls.Add(btnTest);
            contentPanel.Controls.Add(btnClose);

            Controls.Add(contentPanel);
        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Kontrol Krypton merespons dengan normal.\nTema aplikasi: " + DuckFarm.Classes.ThemeManager.CurrentTheme,
                "Krypton Theme Demo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}
