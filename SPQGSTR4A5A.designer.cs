namespace SPEQTAGST.rintlclass4a
{
    partial class SPQGSTR4A5A
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPQGSTR4A5A));
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvGSTR4A5A = new System.Windows.Forms.DataGridView();
            this.colChk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colSequence = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrgInvoiceGSTIN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrgInvoiceNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrgInvoiceDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRvsGSTIN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRvsNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRvsDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRvsValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRvsGudSvs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRvsHSNSAC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRvsTaxVal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col5AIGSTRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col5AIGSTAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col5ACGSTRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col5ACGSTAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col5ASGSTRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col5ASGSTAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ckboxHeader = new System.Windows.Forms.CheckBox();
            this.dgvGSTR4A5ATotal = new System.Windows.Forms.DataGridView();
            this.colTChk = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTSequence = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTOrgInvoiceGSTIN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTOrgInvoiceNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTOrgInvoiceDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTRvsGSTIN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTRvsNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTRvsDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTRvsValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTRvsGudSvs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTRvsHSNSAC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTRvsTaxVal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTIGSTRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTIGSTAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTCGSTRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTCGSTAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTSGSTRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTSGSTAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProgressBar = new System.Windows.Forms.Panel();
            this.pBar = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.pbGSTR1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGSTR4A5A)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGSTR4A5ATotal)).BeginInit();
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
            this.panel1.Location = new System.Drawing.Point(12, 22);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1330, 31);
            this.panel1.TabIndex = 15;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(3, 3);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(200, 23);
            this.txtSearch.TabIndex = 47;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(226, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(877, 23);
            this.label1.TabIndex = 10;
            this.label1.Text = "5A. Amendments to details of inward supplies received from registered taxable per" +
    "sons in earlier tax periods";
            // 
            // dgvGSTR4A5A
            // 
            this.dgvGSTR4A5A.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGSTR4A5A.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvGSTR4A5A.ColumnHeadersHeight = 83;
            this.dgvGSTR4A5A.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colChk,
            this.colSequence,
            this.colOrgInvoiceGSTIN,
            this.colOrgInvoiceNo,
            this.colOrgInvoiceDate,
            this.colRvsGSTIN,
            this.colRvsNo,
            this.colRvsDate,
            this.colRvsValue,
            this.colRvsGudSvs,
            this.colRvsHSNSAC,
            this.colRvsTaxVal,
            this.col5AIGSTRate,
            this.col5AIGSTAmt,
            this.col5ACGSTRate,
            this.col5ACGSTAmt,
            this.col5ASGSTRate,
            this.col5ASGSTAmt});
            this.dgvGSTR4A5A.Location = new System.Drawing.Point(12, 59);
            this.dgvGSTR4A5A.Name = "dgvGSTR4A5A";
            this.dgvGSTR4A5A.RowHeadersVisible = false;
            this.dgvGSTR4A5A.Size = new System.Drawing.Size(1330, 535);
            this.dgvGSTR4A5A.TabIndex = 44;
            this.dgvGSTR4A5A.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGSTR4A5A_CellValueChanged);
            this.dgvGSTR4A5A.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvGSTR4A5A_ColumnHeaderMouseClick);
            this.dgvGSTR4A5A.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvGSTR4A5A_Scroll);
            this.dgvGSTR4A5A.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvGSTR4A5A_UserAddedRow);
            this.dgvGSTR4A5A.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvGSTR4A5A_UserDeletingRow);
            this.dgvGSTR4A5A.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvGSTR4A5A_KeyDown);
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
            // colOrgInvoiceGSTIN
            // 
            this.colOrgInvoiceGSTIN.HeaderText = "Original Invoice GSTIN of supplier";
            this.colOrgInvoiceGSTIN.Name = "colOrgInvoiceGSTIN";
            // 
            // colOrgInvoiceNo
            // 
            this.colOrgInvoiceNo.HeaderText = "Original Invoice No.";
            this.colOrgInvoiceNo.Name = "colOrgInvoiceNo";
            // 
            // colOrgInvoiceDate
            // 
            this.colOrgInvoiceDate.HeaderText = "Original Invoice Date";
            this.colOrgInvoiceDate.Name = "colOrgInvoiceDate";
            // 
            // colRvsGSTIN
            // 
            this.colRvsGSTIN.HeaderText = "Revised Details GSTIN of supplier";
            this.colRvsGSTIN.Name = "colRvsGSTIN";
            // 
            // colRvsNo
            // 
            this.colRvsNo.HeaderText = "Revised Details No";
            this.colRvsNo.Name = "colRvsNo";
            // 
            // colRvsDate
            // 
            this.colRvsDate.HeaderText = "Revised Details Date";
            this.colRvsDate.Name = "colRvsDate";
            // 
            // colRvsValue
            // 
            this.colRvsValue.HeaderText = "Revised Details Value";
            this.colRvsValue.Name = "colRvsValue";
            // 
            // colRvsGudSvs
            // 
            this.colRvsGudSvs.HeaderText = "Revised Details Goods/ Services";
            this.colRvsGudSvs.Name = "colRvsGudSvs";
            // 
            // colRvsHSNSAC
            // 
            this.colRvsHSNSAC.HeaderText = "Revised Details HSN/ SAC";
            this.colRvsHSNSAC.Name = "colRvsHSNSAC";
            // 
            // colRvsTaxVal
            // 
            this.colRvsTaxVal.HeaderText = "Revised Details Taxable value";
            this.colRvsTaxVal.Name = "colRvsTaxVal";
            // 
            // col5AIGSTRate
            // 
            this.col5AIGSTRate.HeaderText = "IGST Rate";
            this.col5AIGSTRate.Name = "col5AIGSTRate";
            // 
            // col5AIGSTAmt
            // 
            this.col5AIGSTAmt.HeaderText = "IGST Amount";
            this.col5AIGSTAmt.Name = "col5AIGSTAmt";
            // 
            // col5ACGSTRate
            // 
            this.col5ACGSTRate.HeaderText = "CGST Rate";
            this.col5ACGSTRate.Name = "col5ACGSTRate";
            // 
            // col5ACGSTAmt
            // 
            this.col5ACGSTAmt.HeaderText = "CGST Amount";
            this.col5ACGSTAmt.Name = "col5ACGSTAmt";
            // 
            // col5ASGSTRate
            // 
            this.col5ASGSTRate.HeaderText = "SGST Rate";
            this.col5ASGSTRate.Name = "col5ASGSTRate";
            // 
            // col5ASGSTAmt
            // 
            this.col5ASGSTAmt.HeaderText = "SGST Amount";
            this.col5ASGSTAmt.Name = "col5ASGSTAmt";
            // 
            // ckboxHeader
            // 
            this.ckboxHeader.Location = new System.Drawing.Point(32, 106);
            this.ckboxHeader.Name = "ckboxHeader";
            this.ckboxHeader.Size = new System.Drawing.Size(13, 13);
            this.ckboxHeader.TabIndex = 47;
            this.ckboxHeader.UseVisualStyleBackColor = true;
            this.ckboxHeader.CheckedChanged += new System.EventHandler(this.ckboxHeader_CheckedChanged);
            // 
            // dgvGSTR4A5ATotal
            // 
            this.dgvGSTR4A5ATotal.AllowUserToAddRows = false;
            this.dgvGSTR4A5ATotal.AllowUserToDeleteRows = false;
            this.dgvGSTR4A5ATotal.AllowUserToResizeColumns = false;
            this.dgvGSTR4A5ATotal.AllowUserToResizeRows = false;
            this.dgvGSTR4A5ATotal.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGSTR4A5ATotal.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvGSTR4A5ATotal.ColumnHeadersHeight = 83;
            this.dgvGSTR4A5ATotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvGSTR4A5ATotal.ColumnHeadersVisible = false;
            this.dgvGSTR4A5ATotal.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTChk,
            this.colTSequence,
            this.colTOrgInvoiceGSTIN,
            this.colTOrgInvoiceNo,
            this.colTOrgInvoiceDate,
            this.colTRvsGSTIN,
            this.colTRvsNo,
            this.colTRvsDate,
            this.colTRvsValue,
            this.colTRvsGudSvs,
            this.colTRvsHSNSAC,
            this.colTRvsTaxVal,
            this.colTIGSTRate,
            this.colTIGSTAmt,
            this.colTCGSTRate,
            this.colTCGSTAmt,
            this.colTSGSTRate,
            this.colTSGSTAmt});
            this.dgvGSTR4A5ATotal.Location = new System.Drawing.Point(12, 600);
            this.dgvGSTR4A5ATotal.Name = "dgvGSTR4A5ATotal";
            this.dgvGSTR4A5ATotal.ReadOnly = true;
            this.dgvGSTR4A5ATotal.RowHeadersVisible = false;
            this.dgvGSTR4A5ATotal.Size = new System.Drawing.Size(1330, 44);
            this.dgvGSTR4A5ATotal.TabIndex = 48;
            this.dgvGSTR4A5ATotal.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvGSTR4A5ATotal_Scroll);
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
            // colTOrgInvoiceGSTIN
            // 
            this.colTOrgInvoiceGSTIN.HeaderText = "Original Invoice GSTIN of supplier";
            this.colTOrgInvoiceGSTIN.Name = "colTOrgInvoiceGSTIN";
            this.colTOrgInvoiceGSTIN.ReadOnly = true;
            // 
            // colTOrgInvoiceNo
            // 
            this.colTOrgInvoiceNo.HeaderText = "Original Invoice No.";
            this.colTOrgInvoiceNo.Name = "colTOrgInvoiceNo";
            this.colTOrgInvoiceNo.ReadOnly = true;
            // 
            // colTOrgInvoiceDate
            // 
            this.colTOrgInvoiceDate.HeaderText = "Original Invoice Date";
            this.colTOrgInvoiceDate.Name = "colTOrgInvoiceDate";
            this.colTOrgInvoiceDate.ReadOnly = true;
            // 
            // colTRvsGSTIN
            // 
            this.colTRvsGSTIN.HeaderText = "Revised Details GSTIN of supplier";
            this.colTRvsGSTIN.Name = "colTRvsGSTIN";
            this.colTRvsGSTIN.ReadOnly = true;
            // 
            // colTRvsNo
            // 
            this.colTRvsNo.HeaderText = "Revised Details No";
            this.colTRvsNo.Name = "colTRvsNo";
            this.colTRvsNo.ReadOnly = true;
            // 
            // colTRvsDate
            // 
            this.colTRvsDate.HeaderText = "Revised Details Date";
            this.colTRvsDate.Name = "colTRvsDate";
            this.colTRvsDate.ReadOnly = true;
            // 
            // colTRvsValue
            // 
            this.colTRvsValue.HeaderText = "Revised Details Value";
            this.colTRvsValue.Name = "colTRvsValue";
            this.colTRvsValue.ReadOnly = true;
            // 
            // colTRvsGudSvs
            // 
            this.colTRvsGudSvs.HeaderText = "Revised Details Goods/ Services";
            this.colTRvsGudSvs.Name = "colTRvsGudSvs";
            this.colTRvsGudSvs.ReadOnly = true;
            // 
            // colTRvsHSNSAC
            // 
            this.colTRvsHSNSAC.HeaderText = "Revised Details HSN/SAC";
            this.colTRvsHSNSAC.Name = "colTRvsHSNSAC";
            this.colTRvsHSNSAC.ReadOnly = true;
            // 
            // colTRvsTaxVal
            // 
            this.colTRvsTaxVal.HeaderText = "Revised Details Taxable value";
            this.colTRvsTaxVal.Name = "colTRvsTaxVal";
            this.colTRvsTaxVal.ReadOnly = true;
            // 
            // colTIGSTRate
            // 
            this.colTIGSTRate.HeaderText = "IGST Rate";
            this.colTIGSTRate.Name = "colTIGSTRate";
            this.colTIGSTRate.ReadOnly = true;
            // 
            // colTIGSTAmt
            // 
            this.colTIGSTAmt.HeaderText = "IGST Amount";
            this.colTIGSTAmt.Name = "colTIGSTAmt";
            this.colTIGSTAmt.ReadOnly = true;
            // 
            // colTCGSTRate
            // 
            this.colTCGSTRate.HeaderText = "CGST Rate";
            this.colTCGSTRate.Name = "colTCGSTRate";
            this.colTCGSTRate.ReadOnly = true;
            // 
            // colTCGSTAmt
            // 
            this.colTCGSTAmt.HeaderText = "CGST Amount";
            this.colTCGSTAmt.Name = "colTCGSTAmt";
            this.colTCGSTAmt.ReadOnly = true;
            // 
            // colTSGSTRate
            // 
            this.colTSGSTRate.HeaderText = "SGST Rate";
            this.colTSGSTRate.Name = "colTSGSTRate";
            this.colTSGSTRate.ReadOnly = true;
            // 
            // colTSGSTAmt
            // 
            this.colTSGSTAmt.HeaderText = "SGST Amount";
            this.colTSGSTAmt.Name = "colTSGSTAmt";
            this.colTSGSTAmt.ReadOnly = true;
            // 
            // ProgressBar
            // 
            this.ProgressBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(217)))), ((int)(((byte)(255)))));
            this.ProgressBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ProgressBar.Controls.Add(this.pBar);
            this.ProgressBar.Controls.Add(this.label2);
            this.ProgressBar.Location = new System.Drawing.Point(504, 300);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(347, 51);
            this.ProgressBar.TabIndex = 50;
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
            this.pbGSTR1.Location = new System.Drawing.Point(635, 283);
            this.pbGSTR1.Name = "pbGSTR1";
            this.pbGSTR1.Size = new System.Drawing.Size(85, 84);
            this.pbGSTR1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbGSTR1.TabIndex = 51;
            this.pbGSTR1.TabStop = false;
            // 
            // frmGSTR4A5A
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1354, 650);
            this.Controls.Add(this.pbGSTR1);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.dgvGSTR4A5ATotal);
            this.Controls.Add(this.ckboxHeader);
            this.Controls.Add(this.dgvGSTR4A5A);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGSTR4A5A";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "1354, 650";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmGSTR4A5A_FormClosed);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGSTR4A5A)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGSTR4A5ATotal)).EndInit();
            this.ProgressBar.ResumeLayout(false);
            this.ProgressBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGSTR1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvGSTR4A5A;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.CheckBox ckboxHeader;
        private System.Windows.Forms.DataGridView dgvGSTR4A5ATotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTChk;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTSequence;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTOrgInvoiceGSTIN;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTOrgInvoiceNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTOrgInvoiceDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTRvsGSTIN;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTRvsNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTRvsDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTRvsValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTRvsGudSvs;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTRvsHSNSAC;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTRvsTaxVal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTIGSTRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTIGSTAmt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTCGSTRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTCGSTAmt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTSGSTRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTSGSTAmt;
        private System.Windows.Forms.Panel ProgressBar;
        private System.Windows.Forms.ProgressBar pBar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colChk;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSequence;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrgInvoiceGSTIN;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrgInvoiceNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrgInvoiceDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRvsGSTIN;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRvsNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRvsDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRvsValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRvsGudSvs;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRvsHSNSAC;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRvsTaxVal;
        private System.Windows.Forms.DataGridViewTextBoxColumn col5AIGSTRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn col5AIGSTAmt;
        private System.Windows.Forms.DataGridViewTextBoxColumn col5ACGSTRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn col5ACGSTAmt;
        private System.Windows.Forms.DataGridViewTextBoxColumn col5ASGSTRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn col5ASGSTAmt;
        private System.Windows.Forms.PictureBox pbGSTR1;
    }
}