namespace XagSurveillanceGCS.GCSViews
{
    partial class PreFlightConfig
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Button btnPreFlight;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

        private void InitializeComponent()
        {
            this.btnPreFlight = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();

            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add((new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0F)));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.0F));
            this.tableLayoutPanel1.RowStyles.Add(
                new System.Windows.Forms.RowStyle(
                    System.Windows.Forms.SizeType.Percent, 100F));

            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1600, 900);
            this.tableLayoutPanel1.TabIndex = 0;

            // 
            // PreFlightConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "PreFlightConfig";
            this.Size = new System.Drawing.Size(1600, 900);

            this.ResumeLayout(false);
        }
    }
}