namespace DuckFarm
{
    partial class DUCKMENU
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            StatusStrip = new System.Windows.Forms.StatusStrip();
            ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();

            StatusStrip.Items.AddRange(
                new System.Windows.Forms.ToolStripItem[]
                {
                    ToolStripStatusLabel
                });

            StatusStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            StatusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            StatusStrip.Name = "StatusStrip";
            StatusStrip.Size = new System.Drawing.Size(1000, 26);
            StatusStrip.TabIndex = 0;

            ToolStripStatusLabel.Name = "ToolStripStatusLabel";
            ToolStripStatusLabel.Spring = true;
            ToolStripStatusLabel.Text = "DuckFarm";
            ToolStripStatusLabel.TextAlign =
                System.Drawing.ContentAlignment.MiddleLeft;

            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1000, 600);
            Controls.Add(StatusStrip);
            IsMdiContainer = true;
            Name = "DuckMENU";
            StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            Text = "DuckFarm";
            Load += DuckMENU_Load;
        }
    }
}
