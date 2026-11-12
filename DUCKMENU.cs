
using FontAwesome.Sharp;
using DuckFarm.Classes;
using DuckFarm.Classes.Factories;
using DuckFarm.Classes.MDI;
using DuckFarm.Forms.Dashboard;
using DuckFarm.Forms.Transaksi;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DuckFarm
{

    public partial class DUCKMENU : Form
    {
        // =========================================================
        // MDI MANAGER DAN SIDEBAR
        // =========================================================

        private MdiManager mdiManager;
        private DuckSidebar sidebar;
        private readonly IFormFactory _formFactory;

        public MdiManager MdiManager
        {
            get { return mdiManager; }
        }

        // =========================================================
        // WINDOWS MENU
        // =========================================================

        private ThemeToggleSwitch themeToggleSwitch;
        private IconButton logoutButton;

        // Header utama aplikasi
        private Panel mainHeaderPanel;
        private Label lblHeaderTitle;

        // Tinggi header utama
        private const int MAIN_HEADER_HEIGHT = 64;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================


        public DUCKMENU(IFormFactory formFactory)
        {
            _formFactory = formFactory
                ?? throw new ArgumentNullException(nameof(formFactory));

            InitializeComponent();

            // Seluruh kode constructor yang sudah ada
            // tetap dipertahankan tanpa perubahan.
        

            this.FormBorderStyle = FormBorderStyle.None;
            this.IsMdiContainer = true;
            this.StartPosition = FormStartPosition.Manual;
            this.WindowState = FormWindowState.Normal;
            this.Shown += DuckMENU_Shown;
            this.HandleCreated += DuckMENU_HandleCreated;

            // =====================================================    
            // SIDEBAR HYBRID ERP
            // =====================================================


            sidebar = new DuckSidebar();

            // Daftarkan grup sesuai urutan yang diinginkan.
            // AddGroup menempatkan grup terbaru di posisi paling atas,
            // sehingga urutan pendaftaran dilakukan dari bawah ke atas.

            sidebar.AddGroup("Dashboard", "", "dashboard");
            sidebar.AddGroup("Master", "", "master");
            sidebar.AddGroup("Transaksi", "", "transaksi");
            sidebar.AddGroup("Windows", "", "windows");
            sidebar.AddGroup("Application", "", "application");

            // Dashboard dibuka otomatis saat aplikasi dimulai.
            // =====================================================
            // MASTER
            // =====================================================

            sidebar.AddMenuItem(
                "Master",
                "Master Item",
                "",
                "master.item",
                null);

            // =====================================================
            // TRANSAKSI
            // =====================================================

            sidebar.AddMenuItem(
                "Transaksi",
                "Absensi dan Hasil CWR",
                "",
                "transaksi.cwr",
                IssueAbsensiHasilCWR_Click);

            // =====================================================
            // WINDOWS
            // =====================================================

            sidebar.AddMenuItem(
                "Windows",
                "Close All",
                "",
                "windows.close-all",
                closeAllToolStripMenuItem_Click);

            sidebar.AddMenuItem(
                "Windows",
                "Tile Horizontally",
                "",
                "windows.tile-horizontal",
                tileHorizontalToolStripMenuItem_Click);

            sidebar.AddMenuItem(
                "Windows",
                "Tile Vertically",
                "",
                "windows.tile-vertical",
                tileVerticalToolStripMenuItem_Click);

            sidebar.AddMenuItem(
                "Windows",
                "Cascade",
                "",
                "windows.cascade",
                cascadeToolStripMenuItem_Click);

            // =====================================================
            // APPLICATION
            // =====================================================

            sidebar.AddNestedMenuItem(
                "Application",
                "About",
                "Informasi Aplikasi",
                "",
                "application.about",
                "application.about.info",
                InformasiAplikasi_Click);

            sidebar.AddMenuItem(
                "Application",
                "Exit",
                "",
                "application.exit",
                exitProgramToolStripMenuItem_Click);

                sidebar.DebugMenuTree();
                sidebar.DebugMenuViewNodes();
                sidebar.DebugMenuViewHierarchy();

          
                // =====================================================
                // SIDEBAR
                // =====================================================

                this.Controls.Add(sidebar);
                sidebar.BringToFront();

                // =====================================================
                // HEADER UTAMA APLIKASI
                // =====================================================

                mainHeaderPanel = new Panel
                {
                    Name = "mainHeaderPanel",
                    Dock = DockStyle.Top,
                    Height = MAIN_HEADER_HEIGHT,
                    BackColor = Color.FromArgb(37, 99, 166),
                    Padding = new Padding(8, 4, 12, 4)
                };

                lblHeaderTitle = new Label
                {
                    Name = "lblHeaderTitle",
                    Text = "Dashboard",
                    Dock = DockStyle.Left,
                    Width = 200,
                    Height = 64,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font(
                    "Segoe UI Semibold",
                    12F,
                    FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.Transparent,
                    Padding = new Padding(20, 0, 0, 0)
                };

                mainHeaderPanel.Controls.Add(lblHeaderTitle);

            //// Theme Toggle berada di sisi kanan header.

            themeToggleSwitch = new ThemeToggleSwitch
            {
                IsDarkMode = false,
                Dock = DockStyle.None,
                Size = new Size(112, 34),
                Location = new Point(
                mainHeaderPanel.ClientSize.Width - 112 - 64 - 12,
                (MAIN_HEADER_HEIGHT - 34) / 2),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Margin = Padding.Empty,
                BackColor = Color.FromArgb(37, 99, 166)
            };

            themeToggleSwitch.ThemeChanged +=
                    ThemeToggleSwitch_ThemeChanged;

            //mainHeaderPanel.Controls.Add(themeToggleSwitch);

            //this.Controls.Add(mainHeaderPanel);
            //mainHeaderPanel.BringToFront();
            mainHeaderPanel.Controls.Add(themeToggleSwitch);

            // =====================================================
            // LOGOUT BUTTON
            // =====================================================

            logoutButton = new IconButton
            {
                Name = "logoutButton",
                Dock = DockStyle.Right,
                Width = MAIN_HEADER_HEIGHT,
                IconChar = FontAwesome.Sharp.IconChar.SignOutAlt,
                IconColor = Color.White,
                IconSize = 22,
                Text = string.Empty,
                BackColor = Color.FromArgb(37, 99, 166),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = Padding.Empty
            };

            logoutButton.FlatAppearance.BorderSize = 0;
            logoutButton.Click += exitProgramToolStripMenuItem_Click;

            mainHeaderPanel.Controls.Add(logoutButton);

            this.Controls.Add(mainHeaderPanel);
            mainHeaderPanel.BringToFront();

            // =====================================================
            // STATUS STRIP
            // =====================================================

            if (this.StatusStrip != null)
                {
                    this.StatusStrip.BringToFront();
                }

                // =====================================================
                // MDI MANAGER
                // =====================================================


                mdiManager = new MdiManager(
                    this,
                    this.StatusStrip,
                    this.sidebar);


                // =====================================================
                // SIDEBAR TOGGLE EVENT
                // =====================================================

                sidebar.ToggleChanged += (sender, e) =>
                {
                    if (mdiManager == null)
                        return;

                    mdiManager.OnParentLayout();
                };
                sidebar.MenuSelected += menuText =>
                {
                    if (lblHeaderTitle != null)
                    {
                        lblHeaderTitle.Text = menuText;
                    }
                };

            sidebar.WindowsFormSelected += form =>
                {
                    if (mdiManager != null)
                    {
                        mdiManager.ActivateChild(form);
                    }
                };
                sidebar.MenuSelected += menuText =>
                {
                    if (lblHeaderTitle != null)
                    {
                        lblHeaderTitle.Text = menuText;
                    }
                };

        }
        private void DuckMENU_HandleCreated(object sender, EventArgs e)
        {
            //AcrylicBackdrop.Apply( this,ThemeManager.IsDarkMode);
        }
        private void DuckMENU_Shown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;

            // Ambil monitor tempat form berada.
            Screen screen = Screen.FromControl(this);
            Rectangle workingArea = screen.WorkingArea;

            // Batasi form ke area kerja monitor.
            this.Bounds = workingArea;

            this.PerformLayout();

            if (mdiManager != null)
            {
                mdiManager.OnParentLayout();
            }

            // Tampilkan ukuran aktual untuk pemeriksaan.
            //MessageBox.Show(
            //    "Screen Bounds : " + screen.Bounds.ToString()
            //    + Environment.NewLine
            //    + "Working Area  : " + workingArea.ToString()
            //    + Environment.NewLine
            //    + "Form Bounds   : " + this.Bounds.ToString()
            //    + Environment.NewLine
            //    + "Window State  : " + this.WindowState.ToString()
            //    + Environment.NewLine
            //    + "TopMost       : " + this.TopMost.ToString(),
            //    "Diagnostik Ukuran DuckMENU",
            //    MessageBoxButtons.OK,
            //    MessageBoxIcon.Information);
        }


        //private void InformasiAplikasi_Click(object sender, EventArgs e)
        //{
        //    // Tulis kode fungsi di sini
        //    MessageBox.Show("DuckFarm - Aplikasi Transaksi CWR", "Informasi Aplikasi");
        //}

        private void InformasiAplikasi_Click(object sender, EventArgs e)
        {
            using (var demo =
                new DuckFarm.Forms.Development.frmKryptonThemeDemo())
            {
                demo.ShowDialog(this);
            }
        }

        private void ThemeToggleSwitch_ThemeChanged(
            object sender,
            EventArgs e)
        {
            ThemeManager.SetTheme(
                themeToggleSwitch.IsDarkMode
                    ? ThemeManager.ThemeMode.Dark
                    : ThemeManager.ThemeMode.Light);

            ApplyCurrentTheme();

            string mode = themeToggleSwitch.IsDarkMode
                ? "Dark"
                : "Light";

            if (this.StatusStrip != null &&
                this.StatusStrip.Items.Count > 0)
            {
                this.StatusStrip.Items[0].Text =
                    "DuckFarm | Mode: " + mode;
            }
        }


        private void ApplyCurrentTheme()
        {
            // 1. Sinkronkan palette Krypton dengan sumber tema aplikasi.
            KryptonThemeManager.ApplyTheme(ThemeManager.CurrentTheme);

            // 2. Terapkan tema ke kontrol WinForms lama.
            ThemeManager.ApplyTheme(this);


            if (mainHeaderPanel != null)
            {
                mainHeaderPanel.BackColor =
                    ThemeManager.IsDarkMode
                        ? Color.FromArgb(30, 41, 59)
                        : Color.FromArgb(37, 99, 166);

                mainHeaderPanel.Invalidate();
            }


            // 3. STATUS STRIP.
            if (this.StatusStrip != null)
            {
                this.StatusStrip.BackColor =
                    ThemeManager.IsDarkMode
                        ? Color.FromArgb(30, 41, 59)
                        : Color.FromArgb(226, 232, 240);

                this.StatusStrip.ForeColor =
                    ThemeManager.IsDarkMode
                        ? Color.White
                        : Color.FromArgb(30, 41, 59);

                this.StatusStrip.Invalidate();
            }
            // Background tombol Light/Dark Mode dan Logout.
            Color headerColor = ThemeManager.IsDarkMode
                ? Color.FromArgb(30, 41, 59)
                : Color.FromArgb(37, 99, 166);

            if (themeToggleSwitch != null)
            {
                themeToggleSwitch.BackColor = headerColor;
            }
            if (logoutButton != null)
            {
                logoutButton.BackColor = ThemeManager.IsDarkMode
                    ? Color.FromArgb(30, 41, 59)
                    : Color.FromArgb(37, 99, 166);

                logoutButton.IconColor = Color.White;
            }


            // 4. Terapkan tema ke seluruh MDI Child Forms.
            foreach (Form child in this.MdiChildren)
            {
                ThemeManager.ApplyTheme(child);

                if (child is frmDashboard dashboard)
                {
                    dashboard.ApplyDashboardTheme();
                }
            }


            // 5. Sinkronkan Sidebar paling akhir.
            if (sidebar != null)
            {
                sidebar.SetDarkTheme(ThemeManager.IsDarkMode);
            }

            //this.PerformLayout();
            //this.Invalidate(true);
            // Terapkan Acrylic sesuai tema aktif.
            //AcrylicBackdrop.Apply( this,ThemeManager.IsDarkMode);

            this.PerformLayout();
            this.Invalidate(true);
        }

        private void DuckMENU_Load(
            object sender,
            EventArgs e)
        {
            // 1. Inisialisasi area MDI.
            if (mdiManager != null)
            {
                mdiManager.Initialize();
            }

            // 2. Perbarui daftar form pada sidebar.
            RefreshWindowsForms();

            // 3. Terapkan tema awal aplikasi.
            ApplyCurrentTheme();

            // 4. Daftarkan event Resize.
            this.Resize -= DuckMENU_Resize;
            this.Resize += DuckMENU_Resize;

            // 5. Daftarkan event Layout.
            this.Layout -= DuckMENU_Layout;
            this.Layout += DuckMENU_Layout;

            // 6. Daftarkan event pergantian form MDI.
            this.MdiChildActivate -= DuckMENU_MdiChildActivate;
            this.MdiChildActivate += DuckMENU_MdiChildActivate;

            // 7. Hitung ulang area MDI.
            if (mdiManager != null)
            {
                mdiManager.OnParentLayout();
            }

            // 8. Buka Dashboard menggunakan Konsep B.
            try
            {
                Form dashboard = _formFactory.Create("dashboard.home");

                OpenMdiChild(
                    dashboard,
                    "Gagal membuka Dashboard.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Gagal membuka Dashboard."
                    + Environment.NewLine
                    + Environment.NewLine
                    + ex.Message,
                    "DuckFarm",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        // =========================================================
        // RESIZE
        // =========================================================

        private void DuckMENU_Resize(
            object sender,
            EventArgs e)
        {
            if (mdiManager == null)
                return;

            mdiManager.OnParentResize();
        }

        // =========================================================
        // LAYOUT
        // =========================================================

        private void DuckMENU_Layout(
            object sender,
            LayoutEventArgs e)
        {
            if (mdiManager == null)
                return;

            mdiManager.OnParentLayout();
        }

        // =========================================================
        // MDI CHILD ACTIVATE
        // =========================================================
        private void DuckMENU_MdiChildActivate(object sender, EventArgs e)
        {
            if (mdiManager != null)
            {
                mdiManager.OnMdiChildActivate();
            }

            // Pastikan setiap form anak memiliki event penutupan.
            foreach (Form child in this.MdiChildren)
            {
                child.FormClosed -= MdiChild_FormClosed;
                child.FormClosed += MdiChild_FormClosed;
            }

            RefreshWindowsForms();
        }

         private void MdiChild_FormClosed(
            object sender,
            FormClosedEventArgs e)
        {
            if (this.IsDisposed ||
                this.Disposing ||
                !this.IsHandleCreated)
            {
                return;
            }

            this.BeginInvoke(new MethodInvoker(() =>
            {
                if (this.IsDisposed || this.Disposing)
                    return;

                RefreshWindowsForms();
            }));
        }

        private void RefreshWindowsForms()
        {
            if (sidebar == null || this.IsDisposed)
                return;

            Form[] openForms = this.MdiChildren
                .Where(form =>
                    form != null &&
                    !form.IsDisposed &&
                    !form.Disposing &&
                    !(form is frmDashboard))
                .ToArray();

            sidebar.UpdateWindowsForms(
                openForms,
                this.ActiveMdiChild is frmDashboard
                    ? null
                    : this.ActiveMdiChild);
        }


        // =========================================================
        // CHECK MODAL
        // =========================================================

        private bool IsModalActive()
        {
            if (mdiManager == null)
                return false;

            return mdiManager.IsModalActive;
        }

        // =========================================================
        // ACTIVATE MODAL
        // =========================================================

        private void ActivateModal()
        {
            if (mdiManager == null)
                return;

            mdiManager.ActivateModalChild();
        }

        // =========================================================
            private void IssueAbsensiHasilCWR_Click(
            object sender,
            EventArgs e)
        {
            Form form = _formFactory.Create(
                "transaksi.cwr");

            OpenMdiChild(
                form,
                "Gagal membuka form Absensi dan Hasil CWR.");
        }

        //==================================================================================================================
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>MENU UTILITY<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        //==================================================================================================================

        private void StatusBarMenu_Click(object sender, EventArgs e)
        {
            this.StatusStrip.Visible = !this.StatusStrip.Visible;

            if (mdiManager != null)
            {
                mdiManager.OnParentLayout();
            }
        }


        private void OpenMdiChild(Form form, string errorMessage)
        {
            if (form == null)
            {
                return;
            }

            if (IsModalActive())
            {
                form.Dispose();
                ActivateModal();
                return;
            }

            if (mdiManager == null)
            {
                form.Dispose();

                MessageBox.Show(
                    "MdiManager belum diinisialisasi.",
                    "DuckFarm",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                
                bool opened = mdiManager.ShowChild(form);

                if (opened)
                {
                    // Dashboard harus memenuhi seluruh area MDI.
                    if (form is frmDashboard)
                    {
                        form.WindowState = FormWindowState.Maximized;
                    }

                    RefreshWindowsForms();
                }
            }
            catch (Exception ex)
            {
                if (!form.IsDisposed)
                {
                    form.Dispose();
                }

                MessageBox.Show(
                    errorMessage + "\n\n" + ex.Message,
                    "DuckFarm",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        // =========================================================
        // CASCADE
        // =========================================================

        private void cascadeToolStripMenuItem_Click(
            object sender,
            EventArgs e)
        {
            if (mdiManager != null)
            {
                mdiManager.LayoutChildren(MdiLayout.Cascade);
            }
        }

        // =========================================================
        // TILE VERTICAL
        // =========================================================

        private void tileVerticalToolStripMenuItem_Click(
            object sender,
            EventArgs e)
        {
            if (mdiManager != null)
            {
                mdiManager.LayoutChildren(MdiLayout.TileVertical);
            }
        }

        // =========================================================
        // TILE HORIZONTAL
        // =========================================================

        private void tileHorizontalToolStripMenuItem_Click(
            object sender,
            EventArgs e)
        {
            if (mdiManager != null)
            {
                mdiManager.LayoutChildren(MdiLayout.TileHorizontal);
            }
        }


        // =========================================================
        // CLOSE ALL MDI CHILDREN
        // =========================================================

        private void closeAllToolStripMenuItem_Click(
            object sender,
            EventArgs e)
        {
            // Jangan menutup form lain ketika modal aktif.
            if (IsModalActive())
            {
                ActivateModal();
                return;
            }

            // Ambil seluruh form MDI yang sedang terbuka.
            // Gunakan snapshot karena koleksi MdiChildren
            // berubah ketika form ditutup.
            Form[] children = this.MdiChildren;

            foreach (Form child in children)
            {
                if (child == null || child.IsDisposed)
                    continue;

                // Dashboard adalah halaman utama aplikasi.
                // Jangan tutup ketika Close All dijalankan.
                if (child is frmDashboard)
                    continue;

                child.Close();
            }


            // Bersihkan status internal MDI hanya jika
            // seluruh form benar-benar sudah tertutup.
            if (this.MdiChildren.Length == 0 &&
                mdiManager != null)
            {   
                mdiManager.Clear();
            }

            // Sinkronkan kedua daftar Windows.
            RefreshWindowsForms();
        }

        // =========================================================
        // EXIT PROGRAM
        // =========================================================

        private void exitProgramToolStripMenuItem_Click(
            object sender,
            EventArgs e)
        {
            ExitApplication();
        }

        private void ExitApplication()
        {
            DialogResult result = MessageBox.Show(
                "Apakah Anda yakin ingin keluar dari aplikasi?",
                "DuckFarm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            // Tutup aplikasi beserta seluruh form yang terbuka.
            // Event FormClosing tetap diberi kesempatan berjalan.
            System.Windows.Forms.Application.Exit();
        }

    }

}
