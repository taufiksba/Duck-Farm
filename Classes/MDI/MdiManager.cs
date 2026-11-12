using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DuckFarm.Classes.MDI
{
    public class MdiManager
    {
        // =========================================================
        // REFERENCES
        // =========================================================

        private readonly Form parentForm;
        private readonly StatusStrip statusStrip;

        // =========================================================
        // MDI CLIENT
        // =========================================================

        private MdiClient mdiClient;
        private DuckSidebar sidebar;

        // =========================================================
        // NORMAL BOUNDS CHILD
        // =========================================================

        private readonly Dictionary<Form, Rectangle> normalBounds =
            new Dictionary<Form, Rectangle>();

        // =========================================================
        // CUSTOM MAXIMIZED
        // =========================================================

        private readonly HashSet<Form> customMaximized =
            new HashSet<Form>();

        // =========================================================
        // MODAL CHILD
        // =========================================================
        //
        // Jika ada Form Show / Lookup aktif:
        //
        // - Form Utama lain tidak boleh diaktifkan.
        // - Windows Menu tidak boleh memindahkan fokus.
        // - Cascade / Tile tidak boleh memindahkan fokus.
        //
        // Semua aturan dikendalikan dari MdiManager.
        //

        private Form modalChild;

        // =========================================================
        // FLAG
        // =========================================================

        private bool adjustingChild = false;

        // =========================================================
        // TOP OFFSET
        // =========================================================
        //
        // MDI dimulai tepat di bawah MenuStrip.
        //

        private const int TOP_OFFSET = 0;

        // =========================================================
        // CONSTRUCTOR
        // =========================================================


        public MdiManager(
            Form parentForm,
            StatusStrip statusStrip,
            DuckSidebar sidebar = null)
        {
            this.parentForm = parentForm;
            this.statusStrip = statusStrip;
            this.sidebar = sidebar;
        }


        // =========================================================
        // PUBLIC PROPERTY
        // =========================================================
        //
        // Digunakan oleh DuckMENU untuk mengetahui apakah sedang
        // ada Form Show / Modal yang aktif.
        //

        public bool IsModalActive
        {
            get
            {
                return modalChild != null &&
                       !modalChild.IsDisposed &&
                       modalChild.Visible;
            }
        }

        // =========================================================
        // INITIALIZE
        // =========================================================

        public void Initialize()
        {
            FindMdiClient();

            UpdateMdiArea();
        }

        // =========================================================
        // FIND MDI CLIENT
        // =========================================================

        private void FindMdiClient()
        {
            mdiClient = null;

            foreach (Control control in parentForm.Controls)
            {
                if (control is MdiClient client)
                {
                    mdiClient = client;
                    break;
                }
            }
        }

        // =========================================================
        // PARENT RESIZE
        // =========================================================

        public void OnParentResize()
        {
            UpdateMdiArea();

            // -----------------------------------------------------
            // MODAL CHILD
            // -----------------------------------------------------

            if (IsModalActive)
            {
                ActivateModalChild();

                return;
            }

            Form child =
                parentForm.ActiveMdiChild;

            if (child == null ||
                child.IsDisposed)
            {
                return;
            }

            // =====================================================
            // MINIMIZED
            // =====================================================
            //
            // Jangan mengubah posisi Form yang sedang minimized.
            //
            // Posisi minimized dikelola oleh MDI Windows Forms.
            //

            if (child.WindowState ==
                FormWindowState.Minimized)
            {
                return;
            }

            // =====================================================
            // CUSTOM MAXIMIZED
            // =====================================================

            if (customMaximized.Contains(child))
            {
                MaximizeChild(child);

                return;
            }

            // =====================================================
            // NORMAL
            // =====================================================

            if (child.WindowState ==
                FormWindowState.Normal)
            {
                CenterChild(child);
            }
        }

        // =========================================================
        // PARENT LAYOUT
        // =========================================================


        public void OnParentLayout()
        {
            UpdateMdiArea();

            // Jika modal aktif, modal tetap diprioritaskan.
            if (IsModalActive)
            {
                ActivateModalChild();
                return;
            }

            // Ambil form MDI yang sedang aktif.
            Form child = parentForm.ActiveMdiChild;

            if (child == null || child.IsDisposed)
                return;

            // Jangan mengubah form yang sedang diminimalkan.
            if (child.WindowState == FormWindowState.Minimized)
                return;

            // Jika menggunakan custom maximize,
            // sesuaikan ukuran dengan area MDI yang baru.
            if (customMaximized.Contains(child))
            {
                MaximizeChild(child);
            }
        }


        // =========================================================
        // MDI CHILD ACTIVATE
        // =========================================================

        public void OnMdiChildActivate()
        {
            Form child =
                parentForm.ActiveMdiChild;

            // =====================================================
            // MODAL CHILD
            // =====================================================

            if (IsModalActive)
            {
                if (child != modalChild)
                {
                    ActivateModalChild();

                    return;
                }

                return;
            }

            // =====================================================
            // NORMAL
            // =====================================================

            if (child == null ||
                child.IsDisposed)
            {
                return;
            }

            // =====================================================
            // EVENT RESIZE
            // =====================================================

            child.Resize -= Child_Resize;
            child.Resize += Child_Resize;

            // =====================================================
            // MINIMIZED
            // =====================================================
            //
            // Jangan restore.
            //
            // MDI Windows Forms yang mengatur posisi minimized.
            //

            if (child.WindowState ==
                FormWindowState.Minimized)
            {
                return;
            }

            // =====================================================
            // CUSTOM MAXIMIZED
            // =====================================================

            if (customMaximized.Contains(child))
            {
                UpdateMdiArea();

                MaximizeChild(child);

                return;
            }

            // =====================================================
            // NORMAL
            // =====================================================

            if (child.WindowState ==
                FormWindowState.Normal)
            {
                if (!normalBounds.ContainsKey(child))
                {
                    SetDefaultSize(child);

                    CenterChild(child);

                    SaveNormalBounds(child);
                }
                else
                {
                    RestoreNormalBounds(child);
                }
            }
        }

        // =========================================================
        // UPDATE MDI AREA
        // =========================================================



        public void UpdateMdiArea()
        {
            if (mdiClient == null ||
                mdiClient.IsDisposed)
            {
                FindMdiClient();

                if (mdiClient == null)
                    return;
            }

            // =====================================================
            // TOP: DI BAWAH HEADER DAN MENUBAR
            // =====================================================

            int top = 0;

            Control mainHeader =
                parentForm.Controls["mainHeaderPanel"];

            if (mainHeader != null &&
                mainHeader.Visible)
            {
                top = mainHeader.Bottom;
            }


            // =====================================================
            // BOTTOM: DI ATAS STATUSSTRIP
            // =====================================================

            int bottom = parentForm.ClientSize.Height;

            if (statusStrip != null &&
                statusStrip.Visible)
            {
                bottom = statusStrip.Top;
            }

            // =====================================================
            // LEFT: DI SEBELAH KANAN SIDEBAR
            // =====================================================

            int left = 0;

            if (sidebar != null &&
                sidebar.Visible)
            {
                left = sidebar.Right;
            }

            // =====================================================
            // BATASI KOORDINAT
            // =====================================================

            left = Math.Max(
                0,
                Math.Min(left, parentForm.ClientSize.Width));

            top = Math.Max(
                0,
                Math.Min(top, parentForm.ClientSize.Height));

            bottom = Math.Max(
                top,
                Math.Min(bottom, parentForm.ClientSize.Height));

            // =====================================================
            // HITUNG AREA MDI
            // =====================================================

            int width =
                parentForm.ClientSize.Width - left;

            int height =
                bottom - top;

            Rectangle bounds = new Rectangle(
                left,
                top,
                Math.Max(0, width),
                Math.Max(0, height));

            // =====================================================
            // TERAPKAN AREA MDI
            // =====================================================

            if (mdiClient.Bounds != bounds)
            {
                mdiClient.Bounds = bounds;
            }
            ResizeCustomMaximizedChildren();
        }



        // =========================================================
        // CHILD RESIZE
        // =========================================================

        private void Child_Resize(
            object sender,
            EventArgs e)
        {
            if (adjustingChild)
                return;

            if (!(sender is Form child))
                return;

            if (child.IsDisposed)
                return;


            if (child.WindowState ==
                FormWindowState.Minimized)
            {
                return;
            }

            // =====================================================
            // MAXIMIZED
            // =====================================================

            if (child.WindowState ==
                FormWindowState.Maximized)
            {
                // -------------------------------------------------
                // SUDAH CUSTOM MAXIMIZED
                // = RESTORE
                // -------------------------------------------------

                //if (customMaximized.Contains(child))
                //{
                //    customMaximized.Remove(child);

                //    UpdateMdiArea();

                //    RestoreNormalBounds(child);

                //    return;
                //}


                if (customMaximized.Contains(child))
                {
                    customMaximized.Remove(child);

                    UpdateMdiArea();

                    adjustingChild = true;

                    try
                    {
                        child.WindowState = FormWindowState.Normal;
                    }
                    finally
                    {
                        adjustingChild = false;
                    }

                    RestoreNormalBounds(child);

                    child.BringToFront();

                    Control mainHeader =
                        parentForm.Controls["mainHeaderPanel"];

                    if (mainHeader != null && mainHeader.Visible)
                    {
                        mainHeader.BringToFront();
                    }

                    if (sidebar != null && sidebar.Visible)
                    {
                        sidebar.BringToFront();
                    }

                    if (statusStrip != null && statusStrip.Visible)
                    {
                        statusStrip.BringToFront();
                    }

                    return;
                }


                // -------------------------------------------------
                // SIMPAN NORMAL BOUNDS
                // -------------------------------------------------

                if (!normalBounds.ContainsKey(child))
                {
                    SaveNormalBounds(child);
                }

                // -------------------------------------------------
                // CUSTOM MAXIMIZE
                // -------------------------------------------------

                customMaximized.Add(child);

                adjustingChild = true;

                try
                {
                    child.WindowState =
                        FormWindowState.Normal;
                }
                finally
                {
                    adjustingChild = false;
                }

                UpdateMdiArea();

                MaximizeChild(child);

                return;
            }

            // =====================================================
            // NORMAL
            // =====================================================

            if (child.WindowState ==
                FormWindowState.Normal)
            {
                if (customMaximized.Contains(child))
                    return;

                SaveNormalBounds(child);
            }
        }

        // =========================================================
        // DEFAULT SIZE
        // =========================================================

        private void SetDefaultSize(Form child)
        {
            if (mdiClient == null ||
                mdiClient.IsDisposed)
            {
                return;
            }

            if (child == null ||
                child.IsDisposed)
            {
                return;
            }

            int maxWidth =
                (int)(
                    mdiClient.ClientSize.Width *
                    0.80);

            int maxHeight =
                (int)(
                    mdiClient.ClientSize.Height *
                    0.80);

            if (maxWidth <= 0 ||
                maxHeight <= 0)
            {
                return;
            }

            int width =
                Math.Min(
                    child.Width,
                    maxWidth);

            int height =
                Math.Min(
                    child.Height,
                    maxHeight);

            // =====================================================
            // MINIMUM SIZE
            // =====================================================

            if (width <
                child.MinimumSize.Width)
            {
                width =
                    child.MinimumSize.Width;
            }

            if (height <
                child.MinimumSize.Height)
            {
                height =
                    child.MinimumSize.Height;
            }

            // =====================================================
            // JANGAN MELEBIHI MDI
            // =====================================================

            width =
                Math.Min(
                    width,
                    mdiClient.ClientSize.Width);

            height =
                Math.Min(
                    height,
                    mdiClient.ClientSize.Height);

            child.Size =
                new Size(
                    width,
                    height);
        }

        // =========================================================
        // CENTER CHILD
        // =========================================================

        private void CenterChild(Form child)
        {
            if (mdiClient == null ||
                mdiClient.IsDisposed)
            {
                return;
            }

            if (child == null ||
                child.IsDisposed)
            {
                return;
            }

            if (child.WindowState !=
                FormWindowState.Normal)
            {
                return;
            }

            int x =
                (
                    mdiClient.ClientSize.Width -
                    child.Width
                ) / 2;

            int y =
                (
                    mdiClient.ClientSize.Height -
                    child.Height
                ) / 2;

            if (x < 0)
                x = 0;

            if (y < 0)
                y = 0;

            child.Location =
                new Point(
                    x,
                    y);
        }

        // =========================================================
        // CUSTOM MAXIMIZE
        // =========================================================

        private void MaximizeChild(Form child)
        {
            if (mdiClient == null ||
                mdiClient.IsDisposed)
            {
                return;
            }

            if (child == null ||
                child.IsDisposed)
            {
                return;
            }

            int width =
                mdiClient.ClientSize.Width;

            int height =
                mdiClient.ClientSize.Height;

            if (width <= 0 ||
                height <= 0)
            {
                return;
            }

            adjustingChild = true;

            try
            {
                child.WindowState =
                    FormWindowState.Normal;

                child.Location =
                    new Point(
                        0,
                        TOP_OFFSET);

                child.Size =
                    new Size(
                        width,
                        height);
            }
            finally
            {
                adjustingChild = false;
            }

            // =====================================================
            // Z-ORDER
            // =====================================================

            child.BringToFront();

            Control mainHeader = parentForm.Controls["mainHeaderPanel"];

            if (mainHeader != null && mainHeader.Visible)
            {
                mainHeader.BringToFront();
            }


            if (statusStrip != null && statusStrip.Visible)
            {
                statusStrip.BringToFront();
            }

        }
        private void ResizeCustomMaximizedChildren()
        {
            if (mdiClient == null || mdiClient.IsDisposed)
                return;

            foreach (Form child in customMaximized)
            {
                if (child == null || child.IsDisposed)
                    continue;

                if (child.WindowState == FormWindowState.Minimized)
                    continue;

                adjustingChild = true;

                try
                {
                    child.WindowState = FormWindowState.Normal;
                    child.Location = new Point(0, TOP_OFFSET);
                    child.Size = mdiClient.ClientSize;
                }
                finally
                {
                    adjustingChild = false;
                }
            }
        }
        // =========================================================
        // SAVE NORMAL BOUNDS
        // =========================================================

        private void SaveNormalBounds(Form child)
        {
            if (child == null ||
                child.IsDisposed)
            {
                return;
            }

            if (child.WindowState !=
                FormWindowState.Normal)
            {
                return;
            }

            if (customMaximized.Contains(child))
                return;

            normalBounds[child] =
                new Rectangle(
                    child.Location,
                    child.Size);
        }

        // =========================================================
        // RESTORE NORMAL BOUNDS
        // =========================================================

        private void RestoreNormalBounds(Form child)
        {
            if (child == null ||
                child.IsDisposed)
            {
                return;
            }

            if (mdiClient == null ||
                mdiClient.IsDisposed)
            {
                return;
            }

            // =====================================================
            // BELUM ADA BOUNDS
            // =====================================================

            if (!normalBounds.TryGetValue(
                    child,
                    out Rectangle bounds))
            {
                SetDefaultSize(child);

                CenterChild(child);

                SaveNormalBounds(child);

                return;
            }

            // =====================================================
            // SIZE
            // =====================================================

            int width =
                Math.Min(
                    bounds.Width,
                    mdiClient.ClientSize.Width);

            int height =
                Math.Min(
                    bounds.Height,
                    mdiClient.ClientSize.Height);

            // =====================================================
            // MINIMUM SIZE
            // =====================================================

            if (width <
                child.MinimumSize.Width)
            {
                width =
                    child.MinimumSize.Width;
            }

            if (height <
                child.MinimumSize.Height)
            {
                height =
                    child.MinimumSize.Height;
            }

            // =====================================================
            // POSITION
            // =====================================================

            int x = bounds.X;
            int y = bounds.Y;

            // =====================================================
            // JANGAN KELUAR AREA MDI
            // =====================================================

            if (x + width >
                mdiClient.ClientSize.Width)
            {
                x =
                    mdiClient.ClientSize.Width -
                    width;
            }

            if (y + height >
                mdiClient.ClientSize.Height)
            {
                y =
                    mdiClient.ClientSize.Height -
                    height;
            }

            if (x < 0)
                x = 0;

            if (y < 0)
                y = 0;

            // =====================================================
            // APPLY
            // =====================================================

            adjustingChild = true;

            try
            {
                child.WindowState =
                    FormWindowState.Normal;

                child.Size =
                    new Size(
                        width,
                        height);

                child.Location =
                    new Point(
                        x,
                        y);
            }
            finally
            {
                adjustingChild = false;
            }
        }

        // =========================================================
        // SHOW MODAL CHILD
        // =========================================================
        //
        // Contoh:
        //
        // var form =
        //     new frmMstPekerjaanShow(_service);
        //
        // menu.MdiManager.ShowModalChild(form);
        //

        public string GetMdiLayoutInfo()
        {
            if (mdiClient == null || mdiClient.IsDisposed)
            {
                FindMdiClient();
            }

            if (mdiClient == null)
            {
                return "MdiClient tidak ditemukan.";
            }

            Control header =
                parentForm.Controls["mainHeaderPanel"];

            string headerInfo = header == null
                ? "Header tidak ditemukan"
                : $"Header Bounds: {header.Bounds}";

            string sidebarInfo = sidebar == null
                ? "Sidebar tidak ditemukan"
                : $"Sidebar Bounds: {sidebar.Bounds}";

            return
                $"Parent ClientSize: {parentForm.ClientSize}\r\n" +
                $"{headerInfo}\r\n" +
                $"{sidebarInfo}\r\n" +
                $"MdiClient Bounds: {mdiClient.Bounds}\r\n" +
                $"MdiClient Parent: {mdiClient.Parent?.Name}";
        }
        public void ShowModalChild(Form child)
        {
            if (child == null ||
                child.IsDisposed)
            {
                return;
            }

            if (mdiClient == null ||
                mdiClient.IsDisposed)
            {
                FindMdiClient();

                if (mdiClient == null)
                    return;
            }

            // =====================================================
            // SUDAH ADA MODAL CHILD
            // =====================================================

            //if (IsModalActive)
            //{
            //    ActivateModalChild();

            //    return;
            //}

            if (IsModalActive)
            {
                // Buang form baru yang belum digunakan.
                if (!ReferenceEquals(child, modalChild))
                {
                    child.Dispose();
                }

                // Aktifkan kembali form Show yang sudah terbuka.
                ActivateModalChild();

                return;
            }

            // =====================================================
            // SIMPAN MODAL CHILD
            // =====================================================

            modalChild = child;

            // =====================================================
            // FORM CLOSED
            // =====================================================

            child.FormClosed -=
                ModalChild_FormClosed;

            child.FormClosed +=
                ModalChild_FormClosed;


            //// =====================================================
            //// MDI PARENT
            //// =====================================================

            //child.MdiParent = parentForm;

            //// =====================================================
            //// EVENT RESIZE
            //// =====================================================
            //// Diperlukan karena OnMdiChildActivate()
            //// tidak memasang event untuk modalChild.

            //child.Resize -= Child_Resize;
            //child.Resize += Child_Resize;

            //// =====================================================
            //// SHOW
            //// =====================================================

            //child.Show();
            // Terapkan tema aktif sebelum Form ditampilkan.
            DuckFarm.Classes.ThemeManager.ApplyTheme(child);

            // Tetapkan parent MDI.
            child.MdiParent = parentForm;

            // Pasang event Resize untuk custom maximize.
            child.Resize -= Child_Resize;
            child.Resize += Child_Resize;

            // SHOW
            child.Show();

            // =====================================================
            // ACTIVATE
            // =====================================================

            ActivateModalChild();
        }

        // =========================================================
        // ACTIVATE MODAL CHILD
        // =========================================================

        public void ActivateModalChild()
        {
            if (!IsModalActive)
                return;

            modalChild.Activate();

            modalChild.BringToFront();
        }

        // =========================================================
        // MODAL CHILD CLOSED
        // =========================================================

       
        private void ModalChild_FormClosed(
            object sender,
            FormClosedEventArgs e)
        {
            if (!(sender is Form child))
                return;

            // Lepaskan event yang dipasang oleh MdiManager.
            child.Resize -= Child_Resize;

            // Bersihkan referensi modal.
            if (modalChild == child)
                modalChild = null;

            // Bersihkan data milik form yang sudah ditutup.
            customMaximized.Remove(child);
            normalBounds.Remove(child);

            // Perbarui area MDI setelah proses penutupan selesai.
            if (parentForm != null &&
                !parentForm.IsDisposed &&
                !parentForm.Disposing)
            {
                parentForm.BeginInvoke((MethodInvoker)delegate
                {
                    if (parentForm.IsDisposed ||
                        parentForm.Disposing)
                    {
                        return;
                    }

                    UpdateMdiArea();

                    if (mdiClient != null &&
                        !mdiClient.IsDisposed)
                    {
                        mdiClient.Invalidate(true);
                    }

                    parentForm.Invalidate(true);
                });
            }
        }



        // =========================================================
        // SHOW CHILD
        // =========================================================
        // Membuka Form MDI biasa tanpa mengunci perpindahan
        // ke Form MDI lainnya.
        // =========================================================

        public bool ShowChild(Form child)
        {
            if (child == null || child.IsDisposed)
            {
                return false;
            }

            // Jika form yang sama sudah terbuka, aktifkan kembali.
            foreach (Form openForm in parentForm.MdiChildren)
            {
                if (openForm == child)
                {
                    return ActivateChild(openForm);
                }

                if (openForm.GetType() == child.GetType())
                {
                    child.Dispose();

                    return ActivateChild(openForm);
                }
            }

            // Jangan membuka form utama lain jika modal aktif.
            if (IsModalActive)
            {
                child.Dispose();
                ActivateModalChild();

                return false;
            }

            // Pastikan MDI Client tersedia.
            if (mdiClient == null || mdiClient.IsDisposed)
            {
                FindMdiClient();

                if (mdiClient == null)
                {
                    child.Dispose();

                    return false;
                }
            }

            //// Tetapkan parent MDI.
            //child.MdiParent = parentForm;

            //// Pasang event Resize untuk custom maximize.
            //child.Resize -= Child_Resize;
            //child.Resize += Child_Resize;

            //// SHOW
            //child.Show();
            // Tetapkan parent MDI.
            child.MdiParent = parentForm;

            // Pasang event Resize untuk custom maximize.
            child.Resize -= Child_Resize;
            child.Resize += Child_Resize;

            // Terapkan tema sebelum form ditampilkan.
            DuckFarm.Classes.ThemeManager.ApplyTheme(child);

            // SHOW
            child.Show();

            // Aktifkan melalui MdiManager.
            return ActivateChild(child);
        }
    
        // =========================================================
        // ACTIVATE CHILD
        // =========================================================
        //
        // Semua perpindahan antar Form Utama nantinya diarahkan
        // melalui method ini.
        //
        // Jika Modal / Show aktif:
        //
        //     Form Utama tidak boleh berpindah.
        //
        // Jika tidak ada Modal:
        //
        //     Form Utama boleh diaktifkan.
        //


        public bool ActivateChild(Form child)
        {
            if (child == null ||
                child.IsDisposed)
            {
                return false;
            }

            // =====================================================
            // MODAL ACTIVE
            // =====================================================

            if (IsModalActive)
            {
                ActivateModalChild();

                return false;
            }

            // =====================================================
            // MINIMIZED
            // =====================================================
            //
            // Jika user memilih Form dari Windows Menu:
            // restore terlebih dahulu.
            //

            if (child.WindowState ==
                FormWindowState.Minimized)
            {
                child.WindowState =
                    FormWindowState.Normal;
            }

            // =====================================================
            // ACTIVATE
            // =====================================================

            child.Activate();

            child.BringToFront();

            return true;
        }

        // =========================================================
        // LAYOUT CHILDREN
        // =========================================================
        //
        // Cascade / Tile harus melewati MdiManager.
        //
        // Jika Modal aktif:
        //     jangan layout Form Utama.
        //
        // Jika tidak:
        //     jalankan layout MDI normal.
        //

        public bool LayoutChildren(
            MdiLayout layout)
        {
            // =====================================================
            // MODAL ACTIVE
            // =====================================================

            if (IsModalActive)
            {
                ActivateModalChild();

                return false;
            }

            // =====================================================
            // LAYOUT
            // =====================================================

            parentForm.LayoutMdi(layout);

            return true;
        }

        // =========================================================
        // RESTORE ACTIVE CHILD
        // =========================================================

        public void RestoreActiveChild()
        {
            // -----------------------------------------------------
            // Jika modal masih aktif, jangan restore child lain.
            // -----------------------------------------------------

            if (IsModalActive)
            {
                ActivateModalChild();

                return;
            }

            Form child =
                parentForm.ActiveMdiChild;

            if (child == null ||
                child.IsDisposed)
            {
                return;
            }

            if (child.WindowState ==
                FormWindowState.Minimized)
            {
                return;
            }

            if (customMaximized.Contains(child))
            {
                customMaximized.Remove(child);

                UpdateMdiArea();

                RestoreNormalBounds(child);
            }
        }

        // =========================================================
        // CLEAR
        // =========================================================

        public void Clear()
        {
            normalBounds.Clear();

            customMaximized.Clear();

            modalChild = null;

            UpdateMdiArea();
        }
    }
}