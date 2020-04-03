namespace SPEQTAGST.rintlclass4a
{
    partial class SPQGSTR4A6
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPQGSTR4A6));
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvGSTR4A6 = new System.Windows.Forms.DataGridView();
            this.colChk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colSequence = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGSTINofSup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTypeOfNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDbtCrdtNoteNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDbtCrdtNoteDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrgInvcNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrgInvsDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiffVal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiffIGSTRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiffIGSTAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiffCGSTRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiffCGSTAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiffSGSTRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiffSGSTAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ckboxHeader = new System.Windows.Forms.CheckBox();
            this.ProgressBar = new System.Windows.Forms.Panel();
            this.pBar = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvGSTR4A6Total = new System.Windows.Forms.DataGridView();
            this.colTChk = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTSequence = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTGSTINofSup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTTypeOfNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTDbtCrdtNoteNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTDbtCrdtNoteDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTOrgInvcNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTOrgInvsDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTDiffVal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTDiffIGSTRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTDiffIGSTAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTDiffCGSTRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTDiffCGSTAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTDiffSGSTRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTDiffSGSTAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pbGSTR1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGSTR4A6)).BeginInit();
            this.ProgressBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGSTR4A6Total)).BeginInit();
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
            this.panel1.TabIndex = 16;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(3, 3);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(200, 23);
            this.txtSearch.TabIndex = 48;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(497, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(335, 23);
            this.label1.TabIndex = 10;
            this.label1.Text = "6. Details of Credit/Debit Notes received";
            // 
            // dgvGSTR4A6
            // 
            this.dgvGSTR4A6.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGSTR4A6.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvGSTR4A6.ColumnHeadersHeight = 83;
            this.dgvGSTR4A6.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colChk,
            this.colSequence,
            this.colGSTINofSup,
            this.colTypeOfNote,
            this.colDbtCrdtNoteNo,
            this.colDbtCrdtNoteDate,
            this.colOrgInvcNo,
            this.colOrgInvsDate,
            this.colDiffVal,
            this.colDiffIGSTRate,
            this.colDiffIGSTAmt,
            this.colDiffCGSTRate,
            this.colDiffCGSTAmt,
            this.colDiffSGSTRate,
            this.colDiffSGSTAmt});
            this.dgvGSTR4A6.Location = new System.Drawing.Point(12, 59);
            this.dgvGSTR4A6.Name = "dgvGSTR4A6";
            this.dgvGSTR4A6.RowHeadersVisible = false;
            this.dgvGSTR4A6.Size = new System.Drawing.Size(1330, 535);
            this.dgvGSTR4A6.TabIndex = 46;
            this.dgvGSTR4A6.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGSTR4A6_CellValueChanged);
            this.dgvGSTR4A6.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvGSTR4A6_ColumnHeaderMouseClick);
            this.dgvGSTR4A6.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvGSTR4A6_Scroll);
            this.dgvGSTR4A6.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvGSTR4A6_UserAddedRow);
            this.dgvGSTR4A6.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvGSTR4A6_UserDeletingRow);
            this.dgvGSTR4A6.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvGSTR4A6_KeyDown);
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
            // colGSTINofSup
            // 
            this.colGSTINofSup.HeaderText = "GSTIN of supplier";
            this.colGSTINofSup.Name = "colGSTINofSup";
            // 
            // colTypeOfNote
            // 
            this.colTypeOfNote.HeaderText = "Type of note (Debit/ Credit)";
            this.colTypeOfNote.Name = "colTypeOfNote";
            // 
            // colDbtCrdtNoteNo
            // 
            this.colDbtCrdtNoteNo.HeaderText = "Debit note/ Credit note No";
            this.colDbtCrdtNoteNo.Name = "colDbtCrdtNoteNo";
            // 
            // colDbtCrdtNoteDate
            // 
            this.colDbtCrdtNoteDate.HeaderText = "Debit Note/ credit note Date";
            this.colDbtCrdtNoteDate.Name = "colDbtCrdtNoteDate";
            // 
            // colOrgInvcNo
            // 
            this.colOrgInvcNo.HeaderText = "Original Invoice No";
            this.colOrgInvcNo.Name = "colOrgInvcNo";
            // 
            // colOrgInvsDate
            // 
            this.colOrgInvsDate.HeaderText = "Original Invoice Date";
            this.colOrgInvsDate.Name = "colOrgInvsDate";
            // 
            // colDiffVal
            // 
            this.colDiffVal.HeaderText = "Differential Value (Plus or Minus)";
            this.colDiffVal.Name = "colDiffVal";
            // 
            // colDiffIGSTRate
            // 
            this.colDiffIGSTRate.HeaderText = "Differential Tax IGST Rate";
            this.colDiffIGSTRate.Name = "colDiffIGSTRate";
            // 
            // colDiffIGSTAmt
            // 
            this.colDiffIGSTAmt.HeaderText = "Differential Tax IGST Amount";
            this.colDiffIGSTAmt.Name = "colDiffIGSTAmt";
            // 
            // colDiffCGSTRate
            // 
            this.colDiffCGSTRate.HeaderText = "Differential Tax CGST Rate";
            this.colDiffCGSTRate.Name = "colDiffCGSTRate";
            // 
            // colDiffCGSTAmt
            // 
            this.colDiffCGSTAmt.HeaderText = "Differential Tax CGST Amount";
            this.colDiffCGSTAmt.Name = "colDiffCGSTAmt";
            // 
            // colDiffSGSTRate
            // 
            this.colDiffSGSTRate.HeaderText = "Differential Tax SGST Rate";
            this.colDiffSGSTRate.Name = "colDiffSGSTRate";
            // 
            // colDiffSGSTAmt
            // 
            this.colDiffSGSTAmt.HeaderText = "Differential Tax SGST Amount";
            this.colDiffSGSTAmt.Name = "colDiffSGSTAmt";
            // 
            // ckboxHeader
            // 
            this.ckboxHeader.Location = new System.Drawing.Point(32, 121);
            this.ckboxHeader.Name = "ckboxHeader";
            this.ckboxHeader.Size = new System.Drawing.Size(13, 13);
            this.ckboxHeader.TabIndex = 49;
            this.ckboxHeader.UseVisualStyleBackColor = true;
            this.ckboxHeader.CheckedChanged += new System.EventHandler(this.ckboxHeader_CheckedChanged);
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
            // dgvGSTR4A6Total
            // 
            this.dgvGSTR4A6Total.AllowUserToAddRows = false;
            this.dgvGSTR4A6Total.AllowUserToDeleteRows = false;
            this.dgvGSTR4A6Total.AllowUserToResizeColumns = false;
            this.dgvGSTR4A6Total.AllowUserToResizeRows = false;
            this.dgvGSTR4A6Total.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGSTR4A6Total.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvGSTR4A6Total.ColumnHeadersHeight = 83;
            this.dgvGSTR4A6Total.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvGSTR4A6Total.ColumnHeadersVisible = false;
            this.dgvGSTR4A6Total.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTChk,
            this.colTSequence,
            this.colTGSTINofSup,
            this.colTTypeOfNote,
            this.colTDbtCrdtNoteNo,
            this.colTDbtCrdtNoteDate,
            this.colTOrgInvcNo,
            this.colTOrgInvsDate,
            this.colTDiffVal,
            this.colTDiffIGSTRate,
            this.colTDiffIGSTAmt,
            this.colTDiffCGSTRate,
            this.colTDiffCGSTAmt,
            this.colTDiffSGSTRate,
            this.colTDiffSGSTAmt});
            this.dgvGSTR4A6Total.Location = new System.Drawing.Point(12, 600);
            this.dgvGSTR4A6Total.Name = "dgvGSTR4A6Total";
            this.dgvGSTR4A6Total.ReadOnly = true;
            this.dgvGSTR4A6Total.RowHeadersVisible = false;
            this.dgvGSTR4A6Total.Size = new System.Drawing.Size(1330, 44);
            this.dgvGSTR4A6Total.TabIndex = 51;
            this.dgvGSTR4A6Total.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvGSTR4A6Total_Scroll);
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
            // colTGSTINofSup
            // 
            this.colTGSTINofSup.HeaderText = "GSTIN of supplier";
            this.colTGSTINofSup.Name = "colTGSTINofSup";
            this.colTGSTINofSup.ReadOnly = true;
            // 
            // colTTypeOfNote
            // 
            this.colTTypeOfNote.HeaderText = "Type of note (Debit/ Credit)";
            this.colTTypeOfNote.Name = "colTTypeOfNote";
            this.colTTypeOfNote.ReadOnly = true;
            // 
            // colTDbtCrdtNoteNo
            // 
            this.colTDbtCrdtNoteNo.HeaderText = "Debit note/ Credit note No";
            this.colTDbtCrdtNoteNo.Name = "colTDbtCrdtNoteNo";
            this.colTDbtCrdtNoteNo.ReadOnly = true;
            // 
            // colTDbtCrdtNoteDate
            // 
            this.colTDbtCrdtNoteDate.HeaderText = "Debit Note/ credit note Date";
            this.colTDbtCrdtNoteDate.Name = "colTDbtCrdtNoteDate";
            this.colTDbtCrdtNoteDate.ReadOnly = true;
            // 
            // colTOrgInvcNo
            // 
            this.colTOrgInvcNo.HeaderText = "Original Invoice No";
            this.colTOrgInvcNo.Name = "colTOrgInvcNo";
            this.colTOrgInvcNo.ReadOnly = true;
            // 
            // colTOrgInvsDate
            // 
            this.colTOrgInvsDate.HeaderText = "Original Invoice Date";
            this.colTOrgInvsDate.Name = "colTOrgInvsDate";
            this.colTOrgInvsDate.ReadOnly = true;
            // 
            // colTDiffVal
            // 
            this.colTDiffVal.HeaderText = "Differential Value (Plus or Minus)";
            this.colTDiffVal.Name = "colTDiffVal";
            this.colTDiffVal.ReadOnly = true;
            // 
            // colTDiffIGSTRate
            // 
            this.colTDiffIGSTRate.HeaderText = "Differential Tax IGST Rate";
            this.colTDiffIGSTRate.Name = "colTDiffIGSTRate";
            this.colTDiffIGSTRate.ReadOnly = true;
            // 
            // colTDiffIGSTAmt
            // 
            this.colTDiffIGSTAmt.HeaderText = "Differential Tax IGST Amount";
            this.colTDiffIGSTAmt.Name = "colTDiffIGSTAmt";
            this.colTDiffIGSTAmt.ReadOnly = true;
            // 
            // colTDiffCGSTRate
            // 
            this.colTDiffCGSTRate.HeaderText = "Differential Tax CGST Rate";
            this.colTDiffCGSTRate.Name = "colTDiffCGSTRate";
            this.colTDiffCGSTRate.ReadOnly = true;
            // 
            // colTDiffCGSTAmt
            // 
            this.colTDiffCGSTAmt.HeaderText = "Differential Tax CGST Amount";
            this.colTDiffCGSTAmt.Name = "colTDiffCGSTAmt";
            this.colTDiffCGSTAmt.ReadOnly = true;
            // 
            // colTDiffSGSTRate
            // 
            this.colTDiffSGSTRate.HeaderText = "Differential Tax SGST Rate";
            this.colTDiffSGSTRate.Name = "colTDiffSGSTRate";
            this.colTDiffSGSTRate.ReadOnly = true;
            // 
            // colTDiffSGSTAmt
            // 
            this.colTDiffSGSTAmt.HeaderText = "Differential Tax SGST Amount";
            this.colTDiffSGSTAmt.Name = "colTDiffSGSTAmt";
            this.colTDiffSGSTAmt.ReadOnly = true;
            // 
            // pbGSTR1
            // 
            this.pbGSTR1.BackColor = System.Drawing.Color.Transparent;
            this.pbGSTR1.Image = ((System.Drawing.Image)(resources.GetObject("pbGSTR1.Image")));
            this.pbGSTR1.Location = new System.Drawing.Point(635, 283);
            this.pbGSTR1.Name = "pbGSTR1";
            this.pbGSTR1.Size = new System.Drawing.Size(85, 84);
            this.pbGSTR1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbGSTR1.TabIndex = 52;
            this.pbGSTR1.TabStop = false;
            // 
            // frmGSTR4A6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1354, 650);
            this.Controls.Add(this.pbGSTR1);
            this.Controls.Add(this.dgvGSTR4A6Total);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.ckboxHeader);
            this.Controls.Add(this.dgvGSTR4A6);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGSTR4A6";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmGSTR4A6";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmGSTR4A6_FormClosed);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGSTR4A6)).EndInit();
            this.ProgressBar.ResumeLayout(false);
            this.ProgressBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGSTR4A6Total)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGSTR1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvGSTR4A6;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.CheckBox ckboxHeader;
        private System.Windows.Forms.Panel ProgressBar;
        private System.Windows.Forms.ProgressBar pBar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colChk;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSequence;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGSTINofSup;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTypeOfNote;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDbtCrdtNoteNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDbtCrdtNoteDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrgInvcNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrgInvsDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiffVal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiffIGSTRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiffIGSTAmt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiffCGSTRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiffCGSTAmt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiffSGSTRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiffSGSTAmt;
        private System.Windows.Forms.DataGridView dgvGSTR4A6Total;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTChk;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTSequence;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTGSTINofSup;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTTypeOfNote;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTDbtCrdtNoteNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTDbtCrdtNoteDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTOrgInvcNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTOrgInvsDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTDiffVal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTDiffIGSTRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTDiffIGSTAmt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTDiffCGSTRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTDiffCGSTAmt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTDiffSGSTRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTDiffSGSTAmt;
        private System.Windows.Forms.PictureBox pbGSTR1;
    }
}