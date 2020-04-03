namespace SPEQTAGST.rintlclass4a
{
    partial class SPQGSTR4A6A
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPQGSTR4A6A));
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ckboxHeader = new System.Windows.Forms.CheckBox();
            this.dgvGSTR4A6A = new System.Windows.Forms.DataGridView();
            this.colChk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colSequence = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col6AGSTIN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTypeOfNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrgDbtCrdtNoteNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrgDbtCrdtNoteDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrgRvsNoteNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrgRvsNoteDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col4ADiffValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col6AIGSTRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col6AIGSTAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col6ACGSTRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col6ACGSTAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col6ASGSTRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col6ASGSTAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvGSTR4A6ATotal = new System.Windows.Forms.DataGridView();
            this.colTChk = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTSequence = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTGSTIN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTTypeOfNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTOrgDbtCrdtNoteNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTOrgDbtCrdtNoteDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTOrgRvsNoteNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTOrgRvsNoteDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTDiffValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            ((System.ComponentModel.ISupportInitialize)(this.dgvGSTR4A6A)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGSTR4A6ATotal)).BeginInit();
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
            this.panel1.TabIndex = 16;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(3, 3);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(200, 23);
            this.txtSearch.TabIndex = 51;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(285, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(733, 23);
            this.label1.TabIndex = 10;
            this.label1.Text = "6A. Amendment to Details of Credit/Debit Notes Issued and Received of earlier tax" +
    " periods";
            // 
            // ckboxHeader
            // 
            this.ckboxHeader.Location = new System.Drawing.Point(32, 121);
            this.ckboxHeader.Name = "ckboxHeader";
            this.ckboxHeader.Size = new System.Drawing.Size(13, 13);
            this.ckboxHeader.TabIndex = 48;
            this.ckboxHeader.UseVisualStyleBackColor = true;
            this.ckboxHeader.CheckedChanged += new System.EventHandler(this.ckboxHeader_CheckedChanged);
            // 
            // dgvGSTR4A6A
            // 
            this.dgvGSTR4A6A.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGSTR4A6A.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvGSTR4A6A.ColumnHeadersHeight = 83;
            this.dgvGSTR4A6A.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvGSTR4A6A.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colChk,
            this.colSequence,
            this.col6AGSTIN,
            this.colTypeOfNote,
            this.colOrgDbtCrdtNoteNo,
            this.colOrgDbtCrdtNoteDate,
            this.colOrgRvsNoteNo,
            this.colOrgRvsNoteDate,
            this.col4ADiffValue,
            this.col6AIGSTRate,
            this.col6AIGSTAmt,
            this.col6ACGSTRate,
            this.col6ACGSTAmt,
            this.col6ASGSTRate,
            this.col6ASGSTAmt});
            this.dgvGSTR4A6A.Location = new System.Drawing.Point(12, 59);
            this.dgvGSTR4A6A.Name = "dgvGSTR4A6A";
            this.dgvGSTR4A6A.RowHeadersVisible = false;
            this.dgvGSTR4A6A.Size = new System.Drawing.Size(1330, 535);
            this.dgvGSTR4A6A.TabIndex = 49;
            this.dgvGSTR4A6A.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGSTR4A6A_CellValueChanged);
            this.dgvGSTR4A6A.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvGSTR4A6A_ColumnHeaderMouseClick);
            this.dgvGSTR4A6A.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvGSTR4A6A_Scroll);
            this.dgvGSTR4A6A.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvGSTR4A6A_UserAddedRow);
            this.dgvGSTR4A6A.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvGSTR4A6A_UserDeletingRow);
            this.dgvGSTR4A6A.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvGSTR4A6A_KeyDown);
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
            // col6AGSTIN
            // 
            this.col6AGSTIN.HeaderText = "GSTIN of supplier";
            this.col6AGSTIN.Name = "col6AGSTIN";
            // 
            // colTypeOfNote
            // 
            this.colTypeOfNote.HeaderText = "Type of note (Debit/ Credit)";
            this.colTypeOfNote.Name = "colTypeOfNote";
            // 
            // colOrgDbtCrdtNoteNo
            // 
            this.colOrgDbtCrdtNoteNo.HeaderText = "Original Debit note/ Credit note No";
            this.colOrgDbtCrdtNoteNo.Name = "colOrgDbtCrdtNoteNo";
            // 
            // colOrgDbtCrdtNoteDate
            // 
            this.colOrgDbtCrdtNoteDate.HeaderText = "Original Debit Note/ credit note Date";
            this.colOrgDbtCrdtNoteDate.Name = "colOrgDbtCrdtNoteDate";
            // 
            // colOrgRvsNoteNo
            // 
            this.colOrgRvsNoteNo.HeaderText = "Original/ Revised Debit Note/ credit note No";
            this.colOrgRvsNoteNo.Name = "colOrgRvsNoteNo";
            this.colOrgRvsNoteNo.Width = 120;
            // 
            // colOrgRvsNoteDate
            // 
            this.colOrgRvsNoteDate.HeaderText = "Original/ Revised Debit Note/ credit note Date";
            this.colOrgRvsNoteDate.Name = "colOrgRvsNoteDate";
            this.colOrgRvsNoteDate.Width = 120;
            // 
            // col4ADiffValue
            // 
            this.col4ADiffValue.HeaderText = "Differential Value (Plus or Minus)";
            this.col4ADiffValue.Name = "col4ADiffValue";
            // 
            // col6AIGSTRate
            // 
            this.col6AIGSTRate.HeaderText = "Differential Tax IGST Rate";
            this.col6AIGSTRate.Name = "col6AIGSTRate";
            // 
            // col6AIGSTAmt
            // 
            this.col6AIGSTAmt.HeaderText = "Differential Tax IGST Amount";
            this.col6AIGSTAmt.Name = "col6AIGSTAmt";
            // 
            // col6ACGSTRate
            // 
            this.col6ACGSTRate.HeaderText = "Differential Tax CGST Rate";
            this.col6ACGSTRate.Name = "col6ACGSTRate";
            // 
            // col6ACGSTAmt
            // 
            this.col6ACGSTAmt.HeaderText = "Differential Tax CGST Amount";
            this.col6ACGSTAmt.Name = "col6ACGSTAmt";
            // 
            // col6ASGSTRate
            // 
            this.col6ASGSTRate.HeaderText = "Differential Tax SGST Rate";
            this.col6ASGSTRate.Name = "col6ASGSTRate";
            // 
            // col6ASGSTAmt
            // 
            this.col6ASGSTAmt.HeaderText = "Differential Tax SGST Amount";
            this.col6ASGSTAmt.Name = "col6ASGSTAmt";
            // 
            // dgvGSTR4A6ATotal
            // 
            this.dgvGSTR4A6ATotal.AllowUserToAddRows = false;
            this.dgvGSTR4A6ATotal.AllowUserToDeleteRows = false;
            this.dgvGSTR4A6ATotal.AllowUserToResizeColumns = false;
            this.dgvGSTR4A6ATotal.AllowUserToResizeRows = false;
            this.dgvGSTR4A6ATotal.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGSTR4A6ATotal.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvGSTR4A6ATotal.ColumnHeadersHeight = 83;
            this.dgvGSTR4A6ATotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvGSTR4A6ATotal.ColumnHeadersVisible = false;
            this.dgvGSTR4A6ATotal.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTChk,
            this.colTSequence,
            this.colTGSTIN,
            this.colTTypeOfNote,
            this.colTOrgDbtCrdtNoteNo,
            this.colTOrgDbtCrdtNoteDate,
            this.colTOrgRvsNoteNo,
            this.colTOrgRvsNoteDate,
            this.colTDiffValue,
            this.colTIGSTRate,
            this.colTIGSTAmt,
            this.colTCGSTRate,
            this.colTCGSTAmt,
            this.colTSGSTRate,
            this.colTSGSTAmt});
            this.dgvGSTR4A6ATotal.Location = new System.Drawing.Point(12, 600);
            this.dgvGSTR4A6ATotal.Name = "dgvGSTR4A6ATotal";
            this.dgvGSTR4A6ATotal.ReadOnly = true;
            this.dgvGSTR4A6ATotal.RowHeadersVisible = false;
            this.dgvGSTR4A6ATotal.Size = new System.Drawing.Size(1330, 44);
            this.dgvGSTR4A6ATotal.TabIndex = 51;
            this.dgvGSTR4A6ATotal.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvGSTR4A6ATotal_Scroll);
            // 
            // colTChk
            // 
            this.colTChk.Frozen = true;
            this.colTChk.HeaderText = "Check All";
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
            // colTGSTIN
            // 
            this.colTGSTIN.HeaderText = "GSTIN of supplier";
            this.colTGSTIN.Name = "colTGSTIN";
            this.colTGSTIN.ReadOnly = true;
            // 
            // colTTypeOfNote
            // 
            this.colTTypeOfNote.HeaderText = "Type of note (Debit/ Credit)";
            this.colTTypeOfNote.Name = "colTTypeOfNote";
            this.colTTypeOfNote.ReadOnly = true;
            // 
            // colTOrgDbtCrdtNoteNo
            // 
            this.colTOrgDbtCrdtNoteNo.HeaderText = "Original Debit note/ Credit note No";
            this.colTOrgDbtCrdtNoteNo.Name = "colTOrgDbtCrdtNoteNo";
            this.colTOrgDbtCrdtNoteNo.ReadOnly = true;
            // 
            // colTOrgDbtCrdtNoteDate
            // 
            this.colTOrgDbtCrdtNoteDate.HeaderText = "Original Debit Note/ credit note Date";
            this.colTOrgDbtCrdtNoteDate.Name = "colTOrgDbtCrdtNoteDate";
            this.colTOrgDbtCrdtNoteDate.ReadOnly = true;
            // 
            // colTOrgRvsNoteNo
            // 
            this.colTOrgRvsNoteNo.HeaderText = "Original/ Revised Debit Note/ credit note No";
            this.colTOrgRvsNoteNo.Name = "colTOrgRvsNoteNo";
            this.colTOrgRvsNoteNo.ReadOnly = true;
            this.colTOrgRvsNoteNo.Width = 120;
            // 
            // colTOrgRvsNoteDate
            // 
            this.colTOrgRvsNoteDate.HeaderText = "Original/ Revised Debit Note/ credit note Date";
            this.colTOrgRvsNoteDate.Name = "colTOrgRvsNoteDate";
            this.colTOrgRvsNoteDate.ReadOnly = true;
            this.colTOrgRvsNoteDate.Width = 120;
            // 
            // colTDiffValue
            // 
            this.colTDiffValue.HeaderText = "Differential Value (Plus or Minus)";
            this.colTDiffValue.Name = "colTDiffValue";
            this.colTDiffValue.ReadOnly = true;
            // 
            // colTIGSTRate
            // 
            this.colTIGSTRate.HeaderText = "Differential Tax IGST Rate";
            this.colTIGSTRate.Name = "colTIGSTRate";
            this.colTIGSTRate.ReadOnly = true;
            // 
            // colTIGSTAmt
            // 
            this.colTIGSTAmt.HeaderText = "Differential Tax IGST Amount";
            this.colTIGSTAmt.Name = "colTIGSTAmt";
            this.colTIGSTAmt.ReadOnly = true;
            // 
            // colTCGSTRate
            // 
            this.colTCGSTRate.HeaderText = "Differential Tax CGST Rate";
            this.colTCGSTRate.Name = "colTCGSTRate";
            this.colTCGSTRate.ReadOnly = true;
            // 
            // colTCGSTAmt
            // 
            this.colTCGSTAmt.HeaderText = "Differential Tax CGST Amount";
            this.colTCGSTAmt.Name = "colTCGSTAmt";
            this.colTCGSTAmt.ReadOnly = true;
            // 
            // colTSGSTRate
            // 
            this.colTSGSTRate.HeaderText = "Differential Tax SGST Rate";
            this.colTSGSTRate.Name = "colTSGSTRate";
            this.colTSGSTRate.ReadOnly = true;
            // 
            // colTSGSTAmt
            // 
            this.colTSGSTAmt.HeaderText = "Differential Tax SGST Amount";
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
            this.ProgressBar.TabIndex = 52;
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
            this.pbGSTR1.TabIndex = 53;
            this.pbGSTR1.TabStop = false;
            // 
            // frmGSTR4A6A
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1354, 650);
            this.Controls.Add(this.pbGSTR1);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.dgvGSTR4A6ATotal);
            this.Controls.Add(this.ckboxHeader);
            this.Controls.Add(this.dgvGSTR4A6A);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(12, 22);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGSTR4A6A";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmGSTR4A6A";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmGSTR4A6A_FormClosed);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGSTR4A6A)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGSTR4A6ATotal)).EndInit();
            this.ProgressBar.ResumeLayout(false);
            this.ProgressBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGSTR1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.CheckBox ckboxHeader;
        private System.Windows.Forms.DataGridView dgvGSTR4A6A;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colChk;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSequence;
        private System.Windows.Forms.DataGridViewTextBoxColumn col6AGSTIN;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTypeOfNote;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrgDbtCrdtNoteNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrgDbtCrdtNoteDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrgRvsNoteNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrgRvsNoteDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn col4ADiffValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn col6AIGSTRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn col6AIGSTAmt;
        private System.Windows.Forms.DataGridViewTextBoxColumn col6ACGSTRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn col6ACGSTAmt;
        private System.Windows.Forms.DataGridViewTextBoxColumn col6ASGSTRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn col6ASGSTAmt;
        private System.Windows.Forms.DataGridView dgvGSTR4A6ATotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTChk;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTSequence;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTGSTIN;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTTypeOfNote;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTOrgDbtCrdtNoteNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTOrgDbtCrdtNoteDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTOrgRvsNoteNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTOrgRvsNoteDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTDiffValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTIGSTRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTIGSTAmt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTCGSTRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTCGSTAmt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTSGSTRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTSGSTAmt;
        private System.Windows.Forms.Panel ProgressBar;
        private System.Windows.Forms.ProgressBar pBar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pbGSTR1;
    }
}