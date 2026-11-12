namespace DuckFarm.Forms.Transaksi
{
    partial class frmTrnAbsensidanHasilCWRShow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            pnlheader = new System.Windows.Forms.Panel();
            appPanel1 = new DuckFarm.Classes.Controls.AppPanel();
            dtperiode2 = new System.Windows.Forms.DateTimePicker();
            label7 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            cbobagian = new System.Windows.Forms.ComboBox();
            cbopekerjaan = new System.Windows.Forms.ComboBox();
            cbodepartemen = new System.Windows.Forms.ComboBox();
            dtperiode1 = new System.Windows.Forms.DateTimePicker();
            pnlbody = new System.Windows.Forms.Panel();
            Dg = new System.Windows.Forms.DataGridView();
            pnlfooter = new System.Windows.Forms.Panel();
            btnExit = new DuckFarm.Classes.Controls.AppButton();
            btnChoose = new DuckFarm.Classes.Controls.AppButton();
            btnRefresh = new DuckFarm.Classes.Controls.AppButton();
            tableLayoutPanel1.SuspendLayout();
            pnlheader.SuspendLayout();
            appPanel1.SuspendLayout();
            pnlbody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Dg).BeginInit();
            pnlfooter.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(pnlheader, 0, 0);
            tableLayoutPanel1.Controls.Add(pnlbody, 0, 1);
            tableLayoutPanel1.Controls.Add(pnlfooter, 0, 2);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 28.0392151F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 71.9607849F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 67F));
            tableLayoutPanel1.Size = new System.Drawing.Size(1090, 578);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // pnlheader
            // 
            pnlheader.Controls.Add(appPanel1);
            pnlheader.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlheader.Location = new System.Drawing.Point(3, 3);
            pnlheader.Name = "pnlheader";
            pnlheader.Size = new System.Drawing.Size(1084, 137);
            pnlheader.TabIndex = 0;
            // 
            // appPanel1
            // 
            appPanel1.AutoScroll = true;
            appPanel1.BackColor = System.Drawing.Color.Transparent;
            appPanel1.BorderColor = System.Drawing.Color.FromArgb(148, 163, 184);
            appPanel1.Controls.Add(dtperiode2);
            appPanel1.Controls.Add(label7);
            appPanel1.Controls.Add(label6);
            appPanel1.Controls.Add(label5);
            appPanel1.Controls.Add(label1);
            appPanel1.Controls.Add(cbobagian);
            appPanel1.Controls.Add(cbopekerjaan);
            appPanel1.Controls.Add(cbodepartemen);
            appPanel1.Controls.Add(dtperiode1);
            appPanel1.Location = new System.Drawing.Point(0, 0);
            appPanel1.Margin = new System.Windows.Forms.Padding(0);
            appPanel1.Name = "appPanel1";
            appPanel1.Padding = new System.Windows.Forms.Padding(16);
            appPanel1.PanelBackColor = System.Drawing.Color.White;
            appPanel1.ShadowColor = System.Drawing.Color.FromArgb(40, 0, 0, 0);
            appPanel1.Size = new System.Drawing.Size(756, 130);
            appPanel1.TabIndex = 0;
            // 
            // dtperiode2
            // 
            dtperiode2.Location = new System.Drawing.Point(672, 9);
            dtperiode2.Name = "dtperiode2";
            dtperiode2.Size = new System.Drawing.Size(51, 27);
            dtperiode2.TabIndex = 23;
            dtperiode2.Visible = false;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(15, 11);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(92, 20);
            label7.TabIndex = 22;
            label7.Text = "Departemen";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(15, 45);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(55, 20);
            label6.TabIndex = 21;
            label6.Text = "Bagian";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(15, 87);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(72, 20);
            label5.TabIndex = 20;
            label5.Text = "Pekerjaan";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(362, 14);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(59, 20);
            label1.TabIndex = 19;
            label1.Text = "Periode";
            // 
            // cbobagian
            // 
            cbobagian.FormattingEnabled = true;
            cbobagian.Location = new System.Drawing.Point(113, 45);
            cbobagian.Name = "cbobagian";
            cbobagian.Size = new System.Drawing.Size(229, 28);
            cbobagian.TabIndex = 18;
            // 
            // cbopekerjaan
            // 
            cbopekerjaan.FormattingEnabled = true;
            cbopekerjaan.Location = new System.Drawing.Point(113, 79);
            cbopekerjaan.Name = "cbopekerjaan";
            cbopekerjaan.Size = new System.Drawing.Size(229, 28);
            cbopekerjaan.TabIndex = 17;
            // 
            // cbodepartemen
            // 
            cbodepartemen.FormattingEnabled = true;
            cbodepartemen.Location = new System.Drawing.Point(113, 11);
            cbodepartemen.Name = "cbodepartemen";
            cbodepartemen.Size = new System.Drawing.Size(229, 28);
            cbodepartemen.TabIndex = 16;
            // 
            // dtperiode1
            // 
            dtperiode1.Location = new System.Drawing.Point(446, 9);
            dtperiode1.Name = "dtperiode1";
            dtperiode1.Size = new System.Drawing.Size(220, 27);
            dtperiode1.TabIndex = 15;
            // 
            // pnlbody
            // 
            pnlbody.Controls.Add(Dg);
            pnlbody.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlbody.Location = new System.Drawing.Point(3, 146);
            pnlbody.Name = "pnlbody";
            pnlbody.Size = new System.Drawing.Size(1084, 361);
            pnlbody.TabIndex = 1;
            // 
            // Dg
            // 
            Dg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            Dg.Dock = System.Windows.Forms.DockStyle.Fill;
            Dg.Location = new System.Drawing.Point(0, 0);
            Dg.Name = "Dg";
            Dg.ReadOnly = true;
            Dg.RowHeadersWidth = 51;
            Dg.Size = new System.Drawing.Size(1084, 361);
            Dg.TabIndex = 0;
            // 
            // pnlfooter
            // 
            pnlfooter.Controls.Add(btnExit);
            pnlfooter.Controls.Add(btnChoose);
            pnlfooter.Controls.Add(btnRefresh);
            pnlfooter.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlfooter.Location = new System.Drawing.Point(3, 513);
            pnlfooter.Name = "pnlfooter";
            pnlfooter.Size = new System.Drawing.Size(1084, 62);
            pnlfooter.TabIndex = 2;
            // 
            // btnExit
            // 
            btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            btnExit.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnExit.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            btnExit.BorderColor = System.Drawing.Color.Transparent;
            btnExit.DisabledColor = System.Drawing.Color.FromArgb(229, 231, 235);
            btnExit.DisabledTextColor = System.Drawing.Color.FromArgb(156, 163, 175);
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            btnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnExit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnExit.ForeColor = System.Drawing.Color.White;
            btnExit.HoverColor = System.Drawing.Color.FromArgb(29, 78, 216);
            btnExit.Location = new System.Drawing.Point(967, 10);
            btnExit.Name = "btnExit";
            btnExit.PressedColor = System.Drawing.Color.FromArgb(30, 64, 175);
            btnExit.Size = new System.Drawing.Size(108, 43);
            btnExit.TabIndex = 5;
            btnExit.Text = "Exit";
            btnExit.UseVisualStyleBackColor = false;
            // 
            // btnChoose
            // 
            btnChoose.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            btnChoose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnChoose.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            btnChoose.BorderColor = System.Drawing.Color.Transparent;
            btnChoose.DisabledColor = System.Drawing.Color.FromArgb(229, 231, 235);
            btnChoose.DisabledTextColor = System.Drawing.Color.FromArgb(156, 163, 175);
            btnChoose.FlatAppearance.BorderSize = 0;
            btnChoose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            btnChoose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            btnChoose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnChoose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnChoose.ForeColor = System.Drawing.Color.White;
            btnChoose.HoverColor = System.Drawing.Color.FromArgb(29, 78, 216);
            btnChoose.Location = new System.Drawing.Point(853, 10);
            btnChoose.Name = "btnChoose";
            btnChoose.PressedColor = System.Drawing.Color.FromArgb(30, 64, 175);
            btnChoose.Size = new System.Drawing.Size(108, 43);
            btnChoose.TabIndex = 4;
            btnChoose.Text = "Choose";
            btnChoose.UseVisualStyleBackColor = false;
            // 
            // btnRefresh
            // 
            btnRefresh.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnRefresh.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            btnRefresh.BorderColor = System.Drawing.Color.Transparent;
            btnRefresh.DisabledColor = System.Drawing.Color.FromArgb(229, 231, 235);
            btnRefresh.DisabledTextColor = System.Drawing.Color.FromArgb(156, 163, 175);
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            btnRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            btnRefresh.ForeColor = System.Drawing.Color.White;
            btnRefresh.HoverColor = System.Drawing.Color.FromArgb(29, 78, 216);
            btnRefresh.Location = new System.Drawing.Point(739, 10);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.PressedColor = System.Drawing.Color.FromArgb(30, 64, 175);
            btnRefresh.Size = new System.Drawing.Size(108, 43);
            btnRefresh.TabIndex = 3;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = false;
            // 
            // frmTrnAbsensidanHasilCWRShow
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1090, 578);
            Controls.Add(tableLayoutPanel1);
            Name = "frmTrnAbsensidanHasilCWRShow";
            Text = "List Data Absensi dan Hasil CWR";
            Load += frmTrnAbsensidanHasilCWRShow_Load;
            tableLayoutPanel1.ResumeLayout(false);
            pnlheader.ResumeLayout(false);
            appPanel1.ResumeLayout(false);
            appPanel1.PerformLayout();
            pnlbody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Dg).EndInit();
            pnlfooter.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pnlheader;
        private System.Windows.Forms.Panel pnlbody;
        private System.Windows.Forms.Panel pnlfooter;
        private Classes.Controls.AppPanel appPanel1;
        private System.Windows.Forms.DataGridView Dg;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbobagian;
        private System.Windows.Forms.ComboBox cbopekerjaan;
        private System.Windows.Forms.ComboBox cbodepartemen;
        private System.Windows.Forms.DateTimePicker dtperiode1;
        private System.Windows.Forms.DateTimePicker dtperiode2;
        private Classes.Controls.AppButton btnExit;
        private Classes.Controls.AppButton btnChoose;
        private Classes.Controls.AppButton btnRefresh;
    }
}