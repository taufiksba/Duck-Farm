using System.Drawing;
using System.Windows.Forms;
using DuckFarm.Classes;

namespace DuckFarm.Forms.Dashboard
{
    public partial class frmDashboard : Form
    {
        public frmDashboard()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.None;
            ControlBox = false;
            Text = string.Empty;
            BackColor = Color.FromArgb(245, 247, 250);

            Label welcomeLabel = new Label
            {
                Name = "lblWelcome",
                Text = "Selamat Datang IMAM TAUFIK",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 55, 72),
                BackColor = Color.Transparent
            };

            Controls.Add(welcomeLabel);
        }

        /// <summary>
        /// Menerapkan warna khusus Dashboard mengikuti tema aplikasi.
        /// Dipanggil oleh DUCKMENU setelah ThemeManager menerapkan tema umum.
        /// </summary>
        public void ApplyDashboardTheme()
        {
            bool isDark = ThemeManager.IsDarkMode;

            BackColor = isDark
                ? Color.FromArgb(15, 23, 42)
                : Color.FromArgb(245, 247, 250);

            Label welcomeLabel = Controls["lblWelcome"] as Label;
            if (welcomeLabel != null)
            {
                welcomeLabel.ForeColor = isDark
                    ? Color.FromArgb(241, 245, 249)
                    : Color.FromArgb(45, 55, 72);
                welcomeLabel.BackColor = Color.Transparent;
            }
        }
    }
}
