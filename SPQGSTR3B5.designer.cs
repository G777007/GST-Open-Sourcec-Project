namespace SPEQTAGST.rintlcs3b
{
    partial class SPQGSTR3B5
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvGSTR3B5 = new System.Windows.Forms.DataGridView();
            this.colSrNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNatureofSupply = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInterStateSupplies = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIntraStateSupplies = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlGSTR19 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGSTR3B5)).BeginInit();
            this.pnlGSTR19.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvGSTR3B5
            // 
            this.dgvGSTR3B5.AllowUserToAddRows = false;
            this.dgvGSTR3B5.AllowUserToDeleteRows = false;
            this.dgvGSTR3B5.AllowUserToResizeColumns = false;
            this.dgvGSTR3B5.AllowUserToResizeRows = false;
            this.dgvGSTR3B5.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGSTR3B5.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvGSTR3B5.ColumnHeadersHeight = 50;
            this.dgvGSTR3B5.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvGSTR3B5.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSrNo,
            this.colNatureofSupply,
            this.colInterStateSupplies,
            this.colIntraStateSupplies});
            this.dgvGSTR3B5.Location = new System.Drawing.Point(0, 52);
            this.dgvGSTR3B5.Name = "dgvGSTR3B5";
            this.dgvGSTR3B5.RowHeadersVisible = false;
            this.dgvGSTR3B5.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            this.dgvGSTR3B5.Size = new System.Drawing.Size(678, 97);
            this.dgvGSTR3B5.TabIndex = 15;
            this.dgvGSTR3B5.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGSTR13_CellValueChanged);
            this.dgvGSTR3B5.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvGSTR3B5_DataBindingComplete);
            this.dgvGSTR3B5.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvGSTR13_KeyDown);
            // 
            // colSrNo
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomLeft;
            this.colSrNo.DefaultCellStyle = dataGridViewCellStyle2;
            this.colSrNo.HeaderText = "Sr. No.";
            this.colSrNo.Name = "colSrNo";
            this.colSrNo.ReadOnly = true;
            this.colSrNo.Visible = false;
            this.colSrNo.Width = 50;
            // 
            // colNatureofSupply
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomLeft;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.colNatureofSupply.DefaultCellStyle = dataGridViewCellStyle3;
            this.colNatureofSupply.HeaderText = "Nature of Supply";
            this.colNatureofSupply.Name = "colNatureofSupply";
            this.colNatureofSupply.ReadOnly = true;
            this.colNatureofSupply.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colNatureofSupply.Width = 410;
            // 
            // colInterStateSupplies
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomRight;
            this.colInterStateSupplies.DefaultCellStyle = dataGridViewCellStyle4;
            this.colInterStateSupplies.HeaderText = "Inter State Supplies";
            this.colInterStateSupplies.Name = "colInterStateSupplies";
            this.colInterStateSupplies.Width = 165;
            // 
            // colIntraStateSupplies
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomRight;
            this.colIntraStateSupplies.DefaultCellStyle = dataGridViewCellStyle5;
            this.colIntraStateSupplies.HeaderText = "Intra State Supplies";
            this.colIntraStateSupplies.Name = "colIntraStateSupplies";
            this.colIntraStateSupplies.Width = 170;
            // 
            // pnlGSTR19
            // 
            this.pnlGSTR19.BackColor = System.Drawing.Color.White;
            this.pnlGSTR19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGSTR19.Controls.Add(this.label1);
            this.pnlGSTR19.Location = new System.Drawing.Point(0, 0);
            this.pnlGSTR19.Name = "pnlGSTR19";
            this.pnlGSTR19.Size = new System.Drawing.Size(678, 52);
            this.pnlGSTR19.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(97, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(552, 20);
            this.label1.TabIndex = 10;
            this.label1.Text = "Values of exempt, nil rated and non-GST inward supplies";
            // 
            // frmGSTR3B5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.ClientSize = new System.Drawing.Size(700, 210);
            this.Controls.Add(this.dgvGSTR3B5);
            this.Controls.Add(this.pnlGSTR19);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmGSTR3B5";
            this.Text = "frmGSTR3B5";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmGSTR3B5_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGSTR3B5)).EndInit();
            this.pnlGSTR19.ResumeLayout(false);
            this.pnlGSTR19.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvGSTR3B5;
        private System.Windows.Forms.Panel pnlGSTR19;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSrNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNatureofSupply;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInterStateSupplies;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIntraStateSupplies;
    }
}