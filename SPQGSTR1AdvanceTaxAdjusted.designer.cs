namespace SPEQTAGST.xasjbr1
{
    partial class SPQGSTR1AdvanceTaxAdjusted
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPQGSTR1AdvanceTaxAdjusted));
            this.label1 = new System.Windows.Forms.Label();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.cmbFilter = new System.Windows.Forms.ComboBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.pbClose = new System.Windows.Forms.LinkLabel();
            this.dgvGSTR1A5 = new System.Windows.Forms.DataGridView();
            this.colChk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colSequence = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIGSTRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPOS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGrossAdvRcv = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIGST = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCGST = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSGST = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCess = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvGSTR1A5Total = new System.Windows.Forms.DataGridView();
            this.colTChk = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTSequence = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTPOS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTGrossAdvRcv = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTIGST = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTCGST = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTSGST = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTCess = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.pbGSTR1 = new System.Windows.Forms.PictureBox();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGSTR1A5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGSTR1A5Total)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGSTR1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(429, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(219, 26);
            this.label1.TabIndex = 10;
            this.label1.Text = "Advance Tax Adj";
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHeader.Controls.Add(this.cmbFilter);
            this.pnlHeader.Controls.Add(this.txtSearch);
            this.pnlHeader.Controls.Add(this.pbClose);
            this.pnlHeader.Controls.Add(this.label1);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(858, 30);
            this.pnlHeader.TabIndex = 16;
            // 
            // cmbFilter
            // 
            this.cmbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilter.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbFilter.FormattingEnabled = true;
            this.cmbFilter.Location = new System.Drawing.Point(223, 3);
            this.cmbFilter.Name = "cmbFilter";
            this.cmbFilter.Size = new System.Drawing.Size(122, 26);
            this.cmbFilter.TabIndex = 25;
            this.cmbFilter.Visible = false;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(69, 2);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(148, 27);
            this.txtSearch.TabIndex = 24;
            this.txtSearch.Visible = false;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // pbClose
            // 
            this.pbClose.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.pbClose.Location = new System.Drawing.Point(6, 2);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new System.Drawing.Size(55, 26);
            this.pbClose.TabIndex = 55;
            this.pbClose.TabStop = true;
            this.pbClose.Text = "<< Back";
            this.pbClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.pbClose.Click += new System.EventHandler(this.pbClose_Click);
            // 
            // dgvGSTR1A5
            // 
            this.dgvGSTR1A5.AllowUserToAddRows = false;
            this.dgvGSTR1A5.AllowUserToDeleteRows = false;
            this.dgvGSTR1A5.AllowUserToResizeRows = false;
            this.dgvGSTR1A5.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Calibri", 9.75F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGSTR1A5.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvGSTR1A5.ColumnHeadersHeight = 83;
            this.dgvGSTR1A5.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvGSTR1A5.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colChk,
            this.colSequence,
            this.colIGSTRate,
            this.colPOS,
            this.colGrossAdvRcv,
            this.colIGST,
            this.colCGST,
            this.colSGST,
            this.colCess});
            this.dgvGSTR1A5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGSTR1A5.Location = new System.Drawing.Point(0, 0);
            this.dgvGSTR1A5.Name = "dgvGSTR1A5";
            this.dgvGSTR1A5.ReadOnly = true;
            this.dgvGSTR1A5.RowHeadersVisible = false;
            this.dgvGSTR1A5.Size = new System.Drawing.Size(858, 478);
            this.dgvGSTR1A5.TabIndex = 15;
            this.dgvGSTR1A5.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvGSTR1A5_Scroll);
            // 
            // colChk
            // 
            this.colChk.Frozen = true;
            this.colChk.HeaderText = "Check All";
            this.colChk.Name = "colChk";
            this.colChk.ReadOnly = true;
            this.colChk.Visible = false;
            this.colChk.Width = 50;
            // 
            // colSequence
            // 
            this.colSequence.DataPropertyName = "colSequence";
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.LightGray;
            this.colSequence.DefaultCellStyle = dataGridViewCellStyle5;
            this.colSequence.Frozen = true;
            this.colSequence.HeaderText = "Sr. #";
            this.colSequence.Name = "colSequence";
            this.colSequence.ReadOnly = true;
            this.colSequence.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colSequence.Width = 50;
            // 
            // colIGSTRate
            // 
            this.colIGSTRate.DataPropertyName = "colRate";
            this.colIGSTRate.Frozen = true;
            this.colIGSTRate.HeaderText = "Rate";
            this.colIGSTRate.Name = "colIGSTRate";
            this.colIGSTRate.ReadOnly = true;
            this.colIGSTRate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colPOS
            // 
            this.colPOS.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPOS.DataPropertyName = "colPOS";
            this.colPOS.HeaderText = "Place of Supply (Name of State)";
            this.colPOS.Name = "colPOS";
            this.colPOS.ReadOnly = true;
            this.colPOS.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colGrossAdvRcv
            // 
            this.colGrossAdvRcv.HeaderText = "Gross Advance Recieved";
            this.colGrossAdvRcv.Name = "colGrossAdvRcv";
            this.colGrossAdvRcv.ReadOnly = true;
            this.colGrossAdvRcv.Width = 140;
            // 
            // colIGST
            // 
            this.colIGST.DataPropertyName = "colIGSTAmnt";
            this.colIGST.HeaderText = "IGST Amount";
            this.colIGST.Name = "colIGST";
            this.colIGST.ReadOnly = true;
            this.colIGST.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colIGST.Width = 140;
            // 
            // colCGST
            // 
            this.colCGST.DataPropertyName = "colCGSTAmnt";
            this.colCGST.HeaderText = "CGST Amount";
            this.colCGST.Name = "colCGST";
            this.colCGST.ReadOnly = true;
            this.colCGST.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colCGST.Width = 140;
            // 
            // colSGST
            // 
            this.colSGST.DataPropertyName = "colSGSTAmnt";
            this.colSGST.HeaderText = "SGST Amount";
            this.colSGST.Name = "colSGST";
            this.colSGST.ReadOnly = true;
            this.colSGST.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colSGST.Width = 140;
            // 
            // colCess
            // 
            this.colCess.HeaderText = "Cess Amount";
            this.colCess.Name = "colCess";
            this.colCess.ReadOnly = true;
            this.colCess.Width = 140;
            // 
            // dgvGSTR1A5Total
            // 
            this.dgvGSTR1A5Total.AllowUserToAddRows = false;
            this.dgvGSTR1A5Total.AllowUserToDeleteRows = false;
            this.dgvGSTR1A5Total.AllowUserToResizeColumns = false;
            this.dgvGSTR1A5Total.AllowUserToResizeRows = false;
            this.dgvGSTR1A5Total.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Calibri", 9.75F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGSTR1A5Total.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvGSTR1A5Total.ColumnHeadersVisible = false;
            this.dgvGSTR1A5Total.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTChk,
            this.colTSequence,
            this.colTRate,
            this.colTPOS,
            this.colTGrossAdvRcv,
            this.colTIGST,
            this.colTCGST,
            this.colTSGST,
            this.colTCess});
            this.dgvGSTR1A5Total.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGSTR1A5Total.Location = new System.Drawing.Point(0, 0);
            this.dgvGSTR1A5Total.Name = "dgvGSTR1A5Total";
            this.dgvGSTR1A5Total.ReadOnly = true;
            this.dgvGSTR1A5Total.RowHeadersVisible = false;
            this.dgvGSTR1A5Total.Size = new System.Drawing.Size(858, 37);
            this.dgvGSTR1A5Total.TabIndex = 17;
            this.dgvGSTR1A5Total.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvGSTR1A5Total_DataBindingComplete);
            this.dgvGSTR1A5Total.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvGSTR1A5Total_Scroll);
            // 
            // colTChk
            // 
            this.colTChk.Frozen = true;
            this.colTChk.HeaderText = "Total";
            this.colTChk.Name = "colTChk";
            this.colTChk.ReadOnly = true;
            this.colTChk.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colTChk.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colTChk.Visible = false;
            this.colTChk.Width = 50;
            // 
            // colTSequence
            // 
            this.colTSequence.DataPropertyName = "colSequence";
            this.colTSequence.Frozen = true;
            this.colTSequence.HeaderText = "Sr. #";
            this.colTSequence.Name = "colTSequence";
            this.colTSequence.ReadOnly = true;
            this.colTSequence.Width = 50;
            // 
            // colTRate
            // 
            this.colTRate.DataPropertyName = "colIGSTRate";
            this.colTRate.HeaderText = "Rate";
            this.colTRate.Name = "colTRate";
            this.colTRate.ReadOnly = true;
            // 
            // colTPOS
            // 
            this.colTPOS.DataPropertyName = "colPOS";
            this.colTPOS.HeaderText = "POS(only if different from the location of recipient)";
            this.colTPOS.Name = "colTPOS";
            this.colTPOS.ReadOnly = true;
            this.colTPOS.Width = 150;
            // 
            // colTGrossAdvRcv
            // 
            this.colTGrossAdvRcv.HeaderText = "GrossAdvRcv";
            this.colTGrossAdvRcv.Name = "colTGrossAdvRcv";
            this.colTGrossAdvRcv.ReadOnly = true;
            this.colTGrossAdvRcv.Width = 130;
            // 
            // colTIGST
            // 
            this.colTIGST.DataPropertyName = "colIGSTAmnt";
            this.colTIGST.HeaderText = "IGST Amount";
            this.colTIGST.Name = "colTIGST";
            this.colTIGST.ReadOnly = true;
            // 
            // colTCGST
            // 
            this.colTCGST.DataPropertyName = "colCGSTAmnt";
            this.colTCGST.HeaderText = "CGST Amount";
            this.colTCGST.Name = "colTCGST";
            this.colTCGST.ReadOnly = true;
            // 
            // colTSGST
            // 
            this.colTSGST.DataPropertyName = "colSGSTAmnt";
            this.colTSGST.HeaderText = "SGST Amount";
            this.colTSGST.Name = "colTSGST";
            this.colTSGST.ReadOnly = true;
            // 
            // colTCess
            // 
            this.colTCess.HeaderText = "CessAmount";
            this.colTCess.Name = "colTCess";
            this.colTCess.ReadOnly = true;
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.pbGSTR1);
            this.pnlMain.Controls.Add(this.panel1);
            this.pnlMain.Controls.Add(this.pnlGrid);
            this.pnlMain.Controls.Add(this.pnlHeader);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(858, 577);
            this.pnlMain.TabIndex = 26;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvGSTR1A5Total);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 508);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(858, 37);
            this.panel1.TabIndex = 17;
            // 
            // pnlGrid
            // 
            this.pnlGrid.Controls.Add(this.dgvGSTR1A5);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlGrid.Location = new System.Drawing.Point(0, 30);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new System.Drawing.Size(858, 478);
            this.pnlGrid.TabIndex = 0;
            // 
            // pbGSTR1
            // 
            this.pbGSTR1.BackColor = System.Drawing.Color.Transparent;
            this.pbGSTR1.Image = ((System.Drawing.Image)(resources.GetObject("pbGSTR1.Image")));
            this.pbGSTR1.Location = new System.Drawing.Point(387, 246);
            this.pbGSTR1.Name = "pbGSTR1";
            this.pbGSTR1.Size = new System.Drawing.Size(85, 84);
            this.pbGSTR1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbGSTR1.TabIndex = 220;
            this.pbGSTR1.TabStop = false;
            this.pbGSTR1.Visible = false;
            // 
            // SPQGSTR1AdvanceTaxAdjusted
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.ClientSize = new System.Drawing.Size(858, 577);
            this.Controls.Add(this.pnlMain);
            this.Font = new System.Drawing.Font("Calibri", 9.75F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SPQGSTR1AdvanceTaxAdjusted";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "5. Taxable outward supplies to a registered person";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmGSTR1A5_FormClosed);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGSTR1A5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGSTR1A5Total)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbGSTR1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.DataGridView dgvGSTR1A5;
        private System.Windows.Forms.DataGridView dgvGSTR1A5Total;
        private System.Windows.Forms.ComboBox cmbFilter;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.LinkLabel pbClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTChk;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTSequence;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTPOS;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTGrossAdvRcv;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTIGST;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTCGST;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTSGST;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTCess;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colChk;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSequence;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIGSTRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPOS;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGrossAdvRcv;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIGST;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCGST;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSGST;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCess;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pbGSTR1;
    }
}