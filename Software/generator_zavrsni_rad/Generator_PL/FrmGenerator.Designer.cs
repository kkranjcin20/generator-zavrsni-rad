namespace generator_zavrsni_rad
{
    partial class FrmGenerator
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
            this.btnUpdateData = new System.Windows.Forms.Button();
            this.txtSearchDB = new System.Windows.Forms.TextBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.dgvTableColumns = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTableColumns)).BeginInit();
            this.SuspendLayout();
            // 
            // btnUpdateData
            // 
            this.btnUpdateData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateData.Location = new System.Drawing.Point(998, 545);
            this.btnUpdateData.Name = "btnUpdateData";
            this.btnUpdateData.Size = new System.Drawing.Size(158, 39);
            this.btnUpdateData.TabIndex = 1;
            this.btnUpdateData.Text = "Fetch data from DB";
            this.btnUpdateData.UseVisualStyleBackColor = true;
            this.btnUpdateData.Click += new System.EventHandler(this.btnFetchData_Click);
            // 
            // txtSearchDB
            // 
            this.txtSearchDB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchDB.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.txtSearchDB.Location = new System.Drawing.Point(12, 12);
            this.txtSearchDB.Name = "txtSearchDB";
            this.txtSearchDB.Size = new System.Drawing.Size(967, 22);
            this.txtSearchDB.TabIndex = 2;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerate.Location = new System.Drawing.Point(998, 590);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(158, 39);
            this.btnGenerate.TabIndex = 4;
            this.btnGenerate.Text = "Generate class(es)";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // dgvTableColumns
            // 
            this.dgvTableColumns.AllowUserToAddRows = false;
            this.dgvTableColumns.AllowUserToDeleteRows = false;
            this.dgvTableColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTableColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTableColumns.Location = new System.Drawing.Point(12, 54);
            this.dgvTableColumns.Name = "dgvTableColumns";
            this.dgvTableColumns.ReadOnly = true;
            this.dgvTableColumns.RowHeadersWidth = 51;
            this.dgvTableColumns.RowTemplate.Height = 24;
            this.dgvTableColumns.Size = new System.Drawing.Size(967, 575);
            this.dgvTableColumns.TabIndex = 5;
            // 
            // FrmGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1168, 641);
            this.Controls.Add(this.dgvTableColumns);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.txtSearchDB);
            this.Controls.Add(this.btnUpdateData);
            this.Name = "FrmGenerator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generator";
            this.Load += new System.EventHandler(this.FrmGenerator_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTableColumns)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUpdateData;
        private System.Windows.Forms.TextBox txtSearchDB;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.DataGridView dgvTableColumns;
    }
}