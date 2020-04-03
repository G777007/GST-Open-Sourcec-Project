namespace SPEQTAGST.rintlclass4a
{
    partial class SPQGSTR4A5
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPQGSTR4A5));
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvGSTR4A5 = new System.Windows.Forms.DataGridView();
            this.colChk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colSequence = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGSTINofSuplier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInvoiceNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInvoiceDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInvoiceValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInvoiceGudServi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInvoiceHSNSAC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInvoiceTaxableVal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIGSTRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIGSTAmnt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCGSTRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCGSTAmnt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSGSTRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSGSTAmnt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ckboxHeader = new System.Windows.Forms.CheckBox();
            this.dgvGSTR4A5Total = new System.Windows.Forms.DataGridView();
            this.colTChk = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTSequence = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTGSTINofSuplier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTInvoiceNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTInvoiceDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTInvoiceValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTInvoiceGudServi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTInvoiceHSNSAC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTInvoiceTaxableVal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTIGSTRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTIGSTAmnt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTCGSTRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTCGSTAmnt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTSGSTRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTSGSTAmnt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProgressBar = new System.Windows.Forms.Panel();
            this.pBar = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.pbGSTR1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGSTR4A5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGSTR4A5Total)).BeginInit();
            this.ProgressBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGSTR1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(217)))), ((int)(((byte)(255)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.txtSearch);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1330, 31);
            this.panel1.TabIndex = 15;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(3, 3);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(200, 23);
            this.txtSearch.TabIndex = 45;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(432, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(478, 23);
            this.label1.TabIndex = 10;
            this.label1.Text = "5. Inward supplies received from registered taxable person";
            // 
            // dgvGSTR4A5
            // 
            this.dgvGSTR4A5.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGSTR4A5.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvGSTR4A5.ColumnHeadersHeight = 83;
            this.dgvGSTR4A5.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvGSTR4A5.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colChk,
            this.colSequence,
            this.colGSTINofSuplier,
            this.colInvoiceNo,
            this.colInvoiceDate,
            this.colInvoiceValue,
            this.colInvoiceGudServi,
            this.colInvoiceHSNSAC,
            this.colInvoiceTaxableVal,
            this.colIGSTRate,
            this.colIGSTAmnt,
            this.colCGSTRate,
            this.colCGSTAmnt,
            this.colSGSTRate,
            this.colSGSTAmnt});
            this.dgvGSTR4A5.Location = new System.Drawing.Point(12, 45);
            this.dgvGSTR4A5.Name = "dgvGSTR4A5";
            this.dgvGSTR4A5.RowHeadersVisible = false;
            this.dgvGSTR4A5.Size = new System.Drawing.Size(1330, 535);
            this.dgvGSTR4A5.TabIndex = 43;
            this.dgvGSTR4A5.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGSTR4A5_CellValueChanged);
            this.dgvGSTR4A5.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvGSTR4A5_ColumnHeaderMouseClick);
            this.dgvGSTR4A5.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvGSTR4A5_Scroll);
            this.dgvGSTR4A5.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvGSTR4A5_UserAddedRow);
            this.dgvGSTR4A5.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvGSTR4A5_UserDeletingRow);
            this.dgvGSTR4A5.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvGSTR4A5_KeyDown);
            // 
            // colChk
            // 
            this.colChk.Frozen = true;
            this.colChk.HeaderText = "Check All";
            this.colChk.Name = "colChk";
            this.colChk.Width = 50;
            // 
            // colSequence
            // 
            this.colSequence.Frozen = true;
            this.colSequence.HeaderText = "Sr. #";
            this.colSequence.Name = "colSequence";
            this.colSequence.ReadOnly = true;
            this.colSequence.Width = 50;
            // 
            // colGSTINofSuplier
            // 
            this.colGSTINofSuplier.HeaderText = "GSTIN of supplier";
            this.colGSTINofSuplier.Name = "colGSTINofSuplier";
            this.colGSTINofSuplier.Width = 110;
            // 
            // colInvoiceNo
            // 
            this.colInvoiceNo.HeaderText = "Invoice No.";
            this.colInvoiceNo.Name = "colInvoiceNo";
            // 
            // colInvoiceDate
            // 
            this.colInvoiceDate.HeaderText = "Invoice Date";
            this.colInvoiceDate.Name = "colInvoiceDate";
            // 
            // colInvoiceValue
            // 
            this.colInvoiceValue.HeaderText = "Invoice Value";
            this.colInvoiceValue.Name = "colInvoiceValue";
            this.colInvoiceValue.Width = 110;
            // 
            // colInvoiceGudServi
            // 
            this.colInvoiceGudServi.HeaderText = "Invoice Goods/ Services";
            this.colInvoiceGudServi.Name = "colInvoiceGudServi";
            // 
            // colInvoiceHSNSAC
            // 
            this.colInvoiceHSNSAC.HeaderText = "Invoice HSN/ SAC";
            this.colInvoiceHSNSAC.Name = "colInvoiceHSNSAC";
            // 
            // colInvoiceTaxableVal
            // 
            this.colInvoiceTaxableVal.HeaderText = "Invoice Taxable Value";
            this.colInvoiceTaxableVal.Name = "colInvoiceTaxableVal";
            this.colInvoiceTaxableVal.Width = 120;
            // 
            // colIGSTRate
            // 
            this.colIGSTRate.HeaderText = "IGST Rate";
            this.colIGSTRate.Name = "colIGSTRate";
            // 
            // colIGSTAmnt
            // 
            this.colIGSTAmnt.HeaderText = "IGST Amount";
            this.colIGSTAmnt.Name = "colIGSTAmnt";
            // 
            // colCGSTRate
            // 
            this.colCGSTRate.HeaderText = "CGST Rate";
            this.colCGSTRate.Name = "colCGSTRate";
            // 
            // colCGSTAmnt
            // 
            this.colCGSTAmnt.HeaderText = "CGST Amount";
            this.colCGSTAmnt.Name = "colCGSTAmnt";
            // 
            // colSGSTRate
            // 
            this.colSGSTRate.HeaderText = "SGST Rate";
            this.colSGSTRate.Name = "colSGSTRate";
            // 
            // colSGSTAmnt
            // 
            this.colSGSTAmnt.HeaderText = "SGST Amount";
            this.colSGSTAmnt.Name = "colSGSTAmnt";
            // 
            // ckboxHeader
            // 
            this.ckboxHeader.Location = new System.Drawing.Point(32, 106);
            this.ckboxHeader.Name = "ckboxHeader";
            this.ckboxHeader.Size = new System.Drawing.Size(13, 13);
            this.ckboxHeader.TabIndex = 45;
            this.ckboxHeader.UseVisualStyleBackColor = true;
            this.ckboxHeader.CheckedChanged += new System.EventHandler(this.ckboxHeader_CheckedChanged);
            // 
            // dgvGSTR4A5Total
            // 
            this.dgvGSTR4A5Total.AllowUserToAddRows = false;
            this.dgvGSTR4A5Total.AllowUserToDeleteRows = false;
            this.dgvGSTR4A5Total.AllowUserToResizeColumns = false;
            this.dgvGSTR4A5Total.AllowUserToResizeRows = false;
            this.dgvGSTR4A5Total.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGSTR4A5Total.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvGSTR4A5Total.ColumnHeadersHeight = 83;
            this.dgvGSTR4A5Total.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvGSTR4A5Total.ColumnHeadersVisible = false;
            this.dgvGSTR4A5Total.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTChk,
            this.colTSequence,
            this.colTGSTINofSuplier,
            this.colTInvoiceNo,
            this.colTInvoiceDate,
            this.colTInvoiceValue,
            this.colTInvoiceGudServi,
            this.colTInvoiceHSNSAC,
            this.colTInvoiceTaxableVal,
            this.colTIGSTRate,
            this.colTIGSTAmnt,
            this.colTCGSTRate,
            this.colTCGSTAmnt,
            this.colTSGSTRate,
            this.colTSGSTAmnt});
            this.dgvGSTR4A5Total.Location = new System.Drawing.Point(12, 586);
            this.dgvGSTR4A5Total.Name = "dgvGSTR4A5Total";
            this.dgvGSTR4A5Total.ReadOnly = true;
            this.dgvGSTR4A5Total.RowHeadersVisible = false;
            this.dgvGSTR4A5Total.Size = new System.Drawing.Size(1330, 40);
            this.dgvGSTR4A5Total.TabIndex = 47;
            this.dgvGSTR4A5Total.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvGSTR4A5Total_Scroll);
            // 
            // colTChk
            // 
            this.colTChk.Frozen = true;
            this.colTChk.HeaderText = "Total";
            this.colTChk.Name = "colTChk";
            this.colTChk.ReadOnly = true;
            this.colTChk.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colTChk.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colTChk.Width = 50;
            // 
            // colTSequence
            // 
            this.colTSequence.Frozen = true;
            this.colTSequence.HeaderText = "Sr. #";
            this.colTSequence.Name = "colTSequence";
            this.colTSequence.ReadOnly = true;
            this.colTSequence.Width = 50;
            // 
            // colTGSTINofSuplier
            // 
            this.colTGSTINofSuplier.HeaderText = "GSTIN of supplier";
            this.colTGSTINofSuplier.Name = "colTGSTINofSuplier";
            this.colTGSTINofSuplier.ReadOnly = true;
            this.colTGSTINofSuplier.Width = 110;
            // 
            // colTInvoiceNo
            // 
            this.colTInvoiceNo.HeaderText = "Invoice No.";
            this.colTInvoiceNo.Name = "colTInvoiceNo";
            this.colTInvoiceNo.ReadOnly = true;
            // 
            // colTInvoiceDate
            // 
            this.colTInvoiceDate.HeaderText = "Invoice Date";
            this.colTInvoiceDate.Name = "colTInvoiceDate";
            this.colTInvoiceDate.ReadOnly = true;
            // 
            // colTInvoiceValue
            // 
            this.colTInvoiceValue.HeaderText = "Invoice Value";
            this.colTInvoiceValue.Name = "colTInvoiceValue";
            this.colTInvoiceValue.ReadOnly = true;
            this.colTInvoiceValue.Width = 110;
            // 
            // colTInvoiceGudServi
            // 
            this.colTInvoiceGudServi.HeaderText = "Invoice Goods/ Services";
            this.colTInvoiceGudServi.Name = "colTInvoiceGudServi";
            this.colTInvoiceGudServi.ReadOnly = true;
            // 
            // colTInvoiceHSNSAC
            // 
            this.colTInvoiceHSNSAC.HeaderText = "Invoice HSN/ SAC";
            this.colTInvoiceHSNSAC.Name = "colTInvoiceHSNSAC";
            this.colTInvoiceHSNSAC.ReadOnly = true;
            // 
            // colTInvoiceTaxableVal
            // 
            this.colTInvoiceTaxableVal.HeaderText = "Invoice Taxable Value";
            this.colTInvoiceTaxableVal.Name = "colTInvoiceTaxableVal";
            this.colTInvoiceTaxableVal.ReadOnly = true;
            this.colTInvoiceTaxableVal.Width = 120;
            // 
            // colTIGSTRate
            // 
            this.colTIGSTRate.HeaderText = "IGST Rate";
            this.colTIGSTRate.Name = "colTIGSTRate";
            this.colTIGSTRate.ReadOnly = true;
            // 
            // colTIGSTAmnt
            // 
            this.colTIGSTAmnt.HeaderText = "IGST Amount";
            this.colTIGSTAmnt.Name = "colTIGSTAmnt";
            this.colTIGSTAmnt.ReadOnly = true;
            // 
            // colTCGSTRate
            // 
            this.colTCGSTRate.HeaderText = "CGST Rate";
            this.colTCGSTRate.Name = "colTCGSTRate";
            this.colTCGSTRate.ReadOnly = true;
            // 
            // colTCGSTAmnt
            // 
            this.colTCGSTAmnt.HeaderText = "CGST Amount";
            this.colTCGSTAmnt.Name = "colTCGSTAmnt";
            this.colTCGSTAmnt.ReadOnly = true;
            // 
            // colTSGSTRate
            // 
            this.colTSGSTRate.HeaderText = "SGST Rate";
            this.colTSGSTRate.Name = "colTSGSTRate";
            this.colTSGSTRate.ReadOnly = true;
            // 
            // colTSGSTAmnt
            // 
            this.colTSGSTAmnt.HeaderText = "SGST Amount";
            this.colTSGSTAmnt.Name = "colTSGSTAmnt";
            this.colTSGSTAmnt.ReadOnly = true;
            // 
            // ProgressBar
            // 
            this.ProgressBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(217)))), ((int)(((byte)(255)))));
            this.ProgressBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ProgressBar.Controls.Add(this.pBar);
            this.ProgressBar.Controls.Add(this.label2);
            this.ProgressBar.Location = new System.Drawing.Point(504, 294);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(347, 51);
            this.ProgressBar.TabIndex = 48;
            this.ProgressBar.Visible = false;
            // 
            // pBar
            // 
            this.pBar.Location = new System.Drawing.Point(6, 13);
            this.pBar.Name = "pBar";
            this.pBar.Size = new System.Drawing.Size(283, 23);
            this.pBar.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(295, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 15);
            this.label2.TabIndex = 21;
            this.label2.Text = ".......";
            // 
            // pbGSTR1
            // 
            this.pbGSTR1.BackColor = System.Drawing.Color.Transparent;
            this.pbGSTR1.Image = ((System.Drawing.Image)(resources.GetObject("pbGSTR1.Image")));
            this.pbGSTR1.Location = new System.Drawing.Point(635, 277);
            this.pbGSTR1.Name = "pbGSTR1";
            this.pbGSTR1.Size = new System.Drawing.Size(85, 84);
            this.pbGSTR1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbGSTR1.TabIndex = 49;
            this.pbGSTR1.TabStop = false;
            // 
            // frmGSTR4A5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1354, 638);
            this.Controls.Add(this.pbGSTR1);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.dgvGSTR4A5Total);
            this.Controls.Add(this.ckboxHeader);
            this.Controls.Add(this.dgvGSTR4A5);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGSTR4A5";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmGSTR4A5";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmGSTR4A5_FormClosed);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGSTR4A5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGSTR4A5Total)).EndInit();
            this.ProgressBar.ResumeLayout(false);
            this.ProgressBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGSTR1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvGSTR4A5;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.CheckBox ckboxHeader;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colChk;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSequence;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGSTINofSuplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvoiceNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvoiceDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvoiceValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvoiceGudServi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvoiceHSNSAC;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvoiceTaxableVal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIGSTRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIGSTAmnt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCGSTRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCGSTAmnt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSGSTRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSGSTAmnt;
        private System.Windows.Forms.DataGridView dgvGSTR4A5Total;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTChk;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTSequence;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTGSTINofSuplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTInvoiceNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTInvoiceDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTInvoiceValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTInvoiceGudServi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTInvoiceHSNSAC;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTInvoiceTaxableVal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTIGSTRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTIGSTAmnt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTCGSTRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTCGSTAmnt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTSGSTRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTSGSTAmnt;
        private System.Windows.Forms.Panel ProgressBar;
        private System.Windows.Forms.ProgressBar pBar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pbGSTR1;
    }
}