namespace generator_zavrsni_rad.Generator_PL
{
    partial class FrmDatabase
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
            this.dgvTables = new System.Windows.Forms.DataGridView();
            this.lblChooseTables = new System.Windows.Forms.Label();
            this.btnFetchTableData = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTables)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvTables
            // 
            this.dgvTables.AllowUserToAddRows = false;
            this.dgvTables.AllowUserToDeleteRows = false;
            this.dgvTables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTables.Location = new System.Drawing.Point(13, 53);
            this.dgvTables.Name = "dgvTables";
            this.dgvTables.ReadOnly = true;
            this.dgvTables.RowHeadersWidth = 51;
            this.dgvTables.RowTemplate.Height = 24;
            this.dgvTables.Size = new System.Drawing.Size(402, 244);
            this.dgvTables.TabIndex = 0;
            // 
            // lblChooseTables
            // 
            this.lblChooseTables.AutoSize = true;
            this.lblChooseTables.Location = new System.Drawing.Point(12, 22);
            this.lblChooseTables.Name = "lblChooseTables";
            this.lblChooseTables.Size = new System.Drawing.Size(290, 16);
            this.lblChooseTables.TabIndex = 1;
            this.lblChooseTables.Text = "Choose the table you want to generate class for:";
            // 
            // btnFetchTableData
            // 
            this.btnFetchTableData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFetchTableData.Location = new System.Drawing.Point(432, 259);
            this.btnFetchTableData.Name = "btnFetchTableData";
            this.btnFetchTableData.Size = new System.Drawing.Size(122, 38);
            this.btnFetchTableData.TabIndex = 2;
            this.btnFetchTableData.Text = "Fetch table data";
            this.btnFetchTableData.UseVisualStyleBackColor = true;
            this.btnFetchTableData.Click += new System.EventHandler(this.btnFetchTableData_Click);
            // 
            // FrmDatabase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 309);
            this.Controls.Add(this.btnFetchTableData);
            this.Controls.Add(this.lblChooseTables);
            this.Controls.Add(this.dgvTables);
            this.Name = "FrmDatabase";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Database tables";
            this.Load += new System.EventHandler(this.FrmDatabase_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTables)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTables;
        private System.Windows.Forms.Label lblChooseTables;
        private System.Windows.Forms.Button btnFetchTableData;
    }
}