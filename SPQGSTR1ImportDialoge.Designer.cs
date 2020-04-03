namespace SPEQTAGST.xasjbr1
{
    partial class SPQGSTR1ImportDialoge
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPQGSTR1ImportDialoge));
            this.pnlContent = new System.Windows.Forms.Panel();
            this.btnImportJson = new System.Windows.Forms.Button();
            this.btnImportGstin = new System.Windows.Forms.Button();
            this.btnImportTallyExcel = new System.Windows.Forms.Button();
            this.rdbJson = new System.Windows.Forms.RadioButton();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.rdbGstin = new System.Windows.Forms.RadioButton();
            this.rdbTallyExcel = new System.Windows.Forms.RadioButton();
            this.rdbSoftExcel = new System.Windows.Forms.RadioButton();
            this.lblYear = new System.Windows.Forms.Label();
            this.lblMainHeader = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnImportSoftExcel = new System.Windows.Forms.Button();
            this.pnlContent.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(248)))), ((int)(((byte)(250)))));
            this.pnlContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlContent.Controls.Add(this.btnImportJson);
            this.pnlContent.Controls.Add(this.btnImportGstin);
            this.pnlContent.Controls.Add(this.btnImportTallyExcel);
            this.pnlContent.Controls.Add(this.rdbJson);
            this.pnlContent.Controls.Add(this.pnlHeader);
            this.pnlContent.Controls.Add(this.rdbGstin);
            this.pnlContent.Controls.Add(this.rdbTallyExcel);
            this.pnlContent.Controls.Add(this.rdbSoftExcel);
            this.pnlContent.Controls.Add(this.lblYear);
            this.pnlContent.Controls.Add(this.lblMainHeader);
            this.pnlContent.Controls.Add(this.btnCancel);
            this.pnlContent.Controls.Add(this.btnImportSoftExcel);
            this.pnlContent.Location = new System.Drawing.Point(7, 6);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(464, 396);
            this.pnlContent.TabIndex = 49;
            this.pnlContent.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlContent_Paint);
            // 
            // btnImportJson
            // 
            this.btnImportJson.BackColor = System.Drawing.Color.White;
            this.btnImportJson.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportJson.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportJson.ForeColor = System.Drawing.Color.Navy;
            this.btnImportJson.Image = global::SPEQTAGST.Properties.Resources.json_24;
            this.btnImportJson.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImportJson.Location = new System.Drawing.Point(252, 234);
            this.btnImportJson.Name = "btnImportJson";
            this.btnImportJson.Size = new System.Drawing.Size(164, 38);
            this.btnImportJson.TabIndex = 42;
            this.btnImportJson.Text = " Offline Tool / Tally Json";
            this.btnImportJson.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnImportJson.UseVisualStyleBackColor = false;
            this.btnImportJson.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnImportGstin
            // 
            this.btnImportGstin.BackColor = System.Drawing.Color.White;
            this.btnImportGstin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportGstin.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportGstin.ForeColor = System.Drawing.Color.Navy;
            this.btnImportGstin.Image = ((System.Drawing.Image)(resources.GetObject("btnImportGstin.Image")));
            this.btnImportGstin.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImportGstin.Location = new System.Drawing.Point(44, 234);
            this.btnImportGstin.Name = "btnImportGstin";
            this.btnImportGstin.Size = new System.Drawing.Size(164, 38);
            this.btnImportGstin.TabIndex = 41;
            this.btnImportGstin.Text = "  Offline Tool Excel";
            this.btnImportGstin.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnImportGstin.UseVisualStyleBackColor = false;
            this.btnImportGstin.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnImportTallyExcel
            // 
            this.btnImportTallyExcel.BackColor = System.Drawing.Color.White;
            this.btnImportTallyExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportTallyExcel.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportTallyExcel.ForeColor = System.Drawing.Color.Navy;
            this.btnImportTallyExcel.Image = global::SPEQTAGST.Properties.Resources.Excel22;
            this.btnImportTallyExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImportTallyExcel.Location = new System.Drawing.Point(252, 179);
            this.btnImportTallyExcel.Name = "btnImportTallyExcel";
            this.btnImportTallyExcel.Size = new System.Drawing.Size(164, 38);
            this.btnImportTallyExcel.TabIndex = 40;
            this.btnImportTallyExcel.Text = "     Tally Excel File";
            this.btnImportTallyExcel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnImportTallyExcel.UseVisualStyleBackColor = false;
            this.btnImportTallyExcel.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // rdbJson
            // 
            this.rdbJson.AutoSize = true;
            this.rdbJson.Font = new System.Drawing.Font("Verdana", 9.25F, System.Drawing.FontStyle.Bold);
            this.rdbJson.ForeColor = System.Drawing.Color.Navy;
            this.rdbJson.Location = new System.Drawing.Point(286, 124);
            this.rdbJson.Name = "rdbJson";
            this.rdbJson.Size = new System.Drawing.Size(117, 20);
            this.rdbJson.TabIndex = 39;
            this.rdbJson.Text = "JSON Import";
            this.rdbJson.UseVisualStyleBackColor = true;
            this.rdbJson.Visible = false;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnlHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHeader.Controls.Add(this.btnClose);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(462, 31);
            this.pnlHeader.TabIndex = 38;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.White;
            this.btnClose.Font = new System.Drawing.Font("Webdings", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.Maroon;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnClose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnClose.Location = new System.Drawing.Point(437, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(25, 24);
            this.btnClose.TabIndex = 26;
            this.btnClose.Text = "r";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.btnClose.UseCompatibleTextRendering = true;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click_1);
            // 
            // rdbGstin
            // 
            this.rdbGstin.AutoSize = true;
            this.rdbGstin.Font = new System.Drawing.Font("Verdana", 9.25F, System.Drawing.FontStyle.Bold);
            this.rdbGstin.ForeColor = System.Drawing.Color.Navy;
            this.rdbGstin.Location = new System.Drawing.Point(286, 98);
            this.rdbGstin.Name = "rdbGstin";
            this.rdbGstin.Size = new System.Drawing.Size(170, 20);
            this.rdbGstin.TabIndex = 18;
            this.rdbGstin.Text = "GSTIN Utility Import";
            this.rdbGstin.UseVisualStyleBackColor = true;
            this.rdbGstin.Visible = false;
            // 
            // rdbTallyExcel
            // 
            this.rdbTallyExcel.AutoSize = true;
            this.rdbTallyExcel.Font = new System.Drawing.Font("Verdana", 9.25F, System.Drawing.FontStyle.Bold);
            this.rdbTallyExcel.ForeColor = System.Drawing.Color.Navy;
            this.rdbTallyExcel.Location = new System.Drawing.Point(95, 124);
            this.rdbTallyExcel.Name = "rdbTallyExcel";
            this.rdbTallyExcel.Size = new System.Drawing.Size(157, 20);
            this.rdbTallyExcel.TabIndex = 17;
            this.rdbTallyExcel.Text = "Tally Excel Import";
            this.rdbTallyExcel.UseVisualStyleBackColor = true;
            this.rdbTallyExcel.Visible = false;
            // 
            // rdbSoftExcel
            // 
            this.rdbSoftExcel.AutoSize = true;
            this.rdbSoftExcel.Checked = true;
            this.rdbSoftExcel.Font = new System.Drawing.Font("Verdana", 9.25F, System.Drawing.FontStyle.Bold);
            this.rdbSoftExcel.ForeColor = System.Drawing.Color.Navy;
            this.rdbSoftExcel.Location = new System.Drawing.Point(92, 98);
            this.rdbSoftExcel.Name = "rdbSoftExcel";
            this.rdbSoftExcel.Size = new System.Drawing.Size(188, 20);
            this.rdbSoftExcel.TabIndex = 16;
            this.rdbSoftExcel.TabStop = true;
            this.rdbSoftExcel.Text = "Software Excel Import";
            this.rdbSoftExcel.UseVisualStyleBackColor = true;
            this.rdbSoftExcel.Visible = false;
            // 
            // lblYear
            // 
            this.lblYear.Font = new System.Drawing.Font("Verdana", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYear.ForeColor = System.Drawing.Color.Navy;
            this.lblYear.Location = new System.Drawing.Point(4, 67);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(454, 20);
            this.lblYear.TabIndex = 13;
            this.lblYear.Text = "F.Y. 2018-2019";
            this.lblYear.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMainHeader
            // 
            this.lblMainHeader.Font = new System.Drawing.Font("Verdana", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainHeader.ForeColor = System.Drawing.Color.Navy;
            this.lblMainHeader.Location = new System.Drawing.Point(4, 40);
            this.lblMainHeader.Name = "lblMainHeader";
            this.lblMainHeader.Size = new System.Drawing.Size(454, 20);
            this.lblMainHeader.TabIndex = 12;
            this.lblMainHeader.Text = "Import Data in GSTR-1";
            this.lblMainHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Crimson;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 8.75F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Image = global::SPEQTAGST.Properties.Resources.Remove;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(340, 341);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 33);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel   ";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnImportSoftExcel
            // 
            this.btnImportSoftExcel.BackColor = System.Drawing.Color.White;
            this.btnImportSoftExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportSoftExcel.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportSoftExcel.ForeColor = System.Drawing.Color.Navy;
            this.btnImportSoftExcel.Image = global::SPEQTAGST.Properties.Resources.Excel22;
            this.btnImportSoftExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImportSoftExcel.Location = new System.Drawing.Point(44, 179);
            this.btnImportSoftExcel.Name = "btnImportSoftExcel";
            this.btnImportSoftExcel.Size = new System.Drawing.Size(164, 38);
            this.btnImportSoftExcel.TabIndex = 4;
            this.btnImportSoftExcel.Text = "     SPEQTA Template";
            this.btnImportSoftExcel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnImportSoftExcel.UseVisualStyleBackColor = false;
            this.btnImportSoftExcel.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // SPQGSTR1ImportDialoge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkSlateGray;
            this.ClientSize = new System.Drawing.Size(479, 408);
            this.Controls.Add(this.pnlContent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SPQGSTR1ImportDialoge";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SPQGSTR1ImportDialoge";
            this.Load += new System.EventHandler(this.SPQGSTR1ImportDialoge_Load);
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.Label lblMainHeader;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnImportSoftExcel;
        private System.Windows.Forms.RadioButton rdbGstin;
        private System.Windows.Forms.RadioButton rdbTallyExcel;
        private System.Windows.Forms.RadioButton rdbSoftExcel;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Button btnImportJson;
        private System.Windows.Forms.Button btnImportGstin;
        private System.Windows.Forms.Button btnImportTallyExcel;
        private System.Windows.Forms.RadioButton rdbJson;
    }
}