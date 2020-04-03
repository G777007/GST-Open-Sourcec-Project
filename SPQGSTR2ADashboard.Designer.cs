namespace SPEQTAGST.cachsR2a
{
    partial class SPQGSTR2ADashboard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPQGSTR2ADashboard));
            this.DgvMain = new System.Windows.Forms.DataGridView();
            this.dgvdiff = new System.Windows.Forms.DataGridView();
            this.dgvaccount = new System.Windows.Forms.DataGridView();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnRequest = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.msList = new System.Windows.Forms.MenuStrip();
            this.msClose = new System.Windows.Forms.ToolStripMenuItem();
            this.msExp = new System.Windows.Forms.ToolStripMenuItem();
            this.msExpExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.msExpExcelAll = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.msImpExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.msImpFromGSP = new System.Windows.Forms.ToolStripMenuItem();
            this.msImpJson = new System.Windows.Forms.ToolStripMenuItem();
            this.msDownload = new System.Windows.Forms.ToolStripMenuItem();
            this.msRequest = new System.Windows.Forms.ToolStripMenuItem();
            this.msClearAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pbGSTR1 = new System.Windows.Forms.PictureBox();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlGrid = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.DgvMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvdiff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvaccount)).BeginInit();
            this.pnlHeader.SuspendLayout();
            this.msList.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGSTR1)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            this.SuspendLayout();
            // 
            // DgvMain
            // 
            this.DgvMain.AllowUserToAddRows = false;
            this.DgvMain.AllowUserToDeleteRows = false;
            this.DgvMain.AllowUserToResizeColumns = false;
            this.DgvMain.AllowUserToResizeRows = false;
            this.DgvMain.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.DgvMain.BackgroundColor = System.Drawing.Color.White;
            this.DgvMain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DgvMain.ColumnHeadersHeight = 50;
            this.DgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgvMain.DefaultCellStyle = dataGridViewCellStyle1;
            this.DgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgvMain.Location = new System.Drawing.Point(0, 0);
            this.DgvMain.Name = "DgvMain";
            this.DgvMain.ReadOnly = true;
            this.DgvMain.RowHeadersVisible = false;
            this.DgvMain.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DgvMain.RowTemplate.Height = 28;
            this.DgvMain.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DgvMain.Size = new System.Drawing.Size(957, 158);
            this.DgvMain.TabIndex = 171;
            this.DgvMain.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvMain_CellClick);
            this.DgvMain.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DgvMain_CellFormatting);
            this.DgvMain.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvaccount_DataBindingComplete);
            this.DgvMain.Scroll += new System.Windows.Forms.ScrollEventHandler(this.DgvMain_Scroll);
            // 
            // dgvdiff
            // 
            this.dgvdiff.AllowUserToAddRows = false;
            this.dgvdiff.AllowUserToDeleteRows = false;
            this.dgvdiff.AllowUserToResizeColumns = false;
            this.dgvdiff.AllowUserToResizeRows = false;
            this.dgvdiff.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(241)))), ((int)(((byte)(252)))));
            this.dgvdiff.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvdiff.CausesValidation = false;
            this.dgvdiff.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvdiff.ColumnHeadersVisible = false;
            this.dgvdiff.Location = new System.Drawing.Point(1, 313);
            this.dgvdiff.Name = "dgvdiff";
            this.dgvdiff.RowHeadersVisible = false;
            this.dgvdiff.Size = new System.Drawing.Size(827, 20);
            this.dgvdiff.TabIndex = 173;
            this.dgvdiff.Visible = false;
            this.dgvdiff.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvdiff_CellFormatting);
            this.dgvdiff.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvaccount_DataBindingComplete);
            this.dgvdiff.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvdiff_Scroll);
            // 
            // dgvaccount
            // 
            this.dgvaccount.AllowUserToAddRows = false;
            this.dgvaccount.AllowUserToDeleteRows = false;
            this.dgvaccount.AllowUserToOrderColumns = true;
            this.dgvaccount.AllowUserToResizeColumns = false;
            this.dgvaccount.AllowUserToResizeRows = false;
            this.dgvaccount.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(241)))), ((int)(((byte)(252)))));
            this.dgvaccount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvaccount.CausesValidation = false;
            this.dgvaccount.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvaccount.ColumnHeadersVisible = false;
            this.dgvaccount.Location = new System.Drawing.Point(1, 287);
            this.dgvaccount.Name = "dgvaccount";
            this.dgvaccount.RowHeadersVisible = false;
            this.dgvaccount.Size = new System.Drawing.Size(827, 20);
            this.dgvaccount.TabIndex = 172;
            this.dgvaccount.Visible = false;
            this.dgvaccount.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvaccount_CellFormatting);
            this.dgvaccount.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvaccount_CellValueChanged);
            this.dgvaccount.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvaccount_DataBindingComplete);
            this.dgvaccount.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvaccount_Scroll);
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHeader.Controls.Add(this.btnDownload);
            this.pnlHeader.Controls.Add(this.btnRequest);
            this.pnlHeader.Controls.Add(this.label1);
            this.pnlHeader.Controls.Add(this.msList);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(957, 52);
            this.pnlHeader.TabIndex = 181;
            // 
            // btnDownload
            // 
            this.btnDownload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(217)))), ((int)(((byte)(255)))));
            this.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDownload.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnDownload.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this.btnDownload.Location = new System.Drawing.Point(265, 3);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(92, 23);
            this.btnDownload.TabIndex = 184;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = false;
            this.btnDownload.Visible = false;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnRequest
            // 
            this.btnRequest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(217)))), ((int)(((byte)(255)))));
            this.btnRequest.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRequest.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnRequest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this.btnRequest.Location = new System.Drawing.Point(172, 3);
            this.btnRequest.Name = "btnRequest";
            this.btnRequest.Size = new System.Drawing.Size(87, 23);
            this.btnRequest.TabIndex = 183;
            this.btnRequest.Text = "Request";
            this.btnRequest.UseVisualStyleBackColor = false;
            this.btnRequest.Visible = false;
            this.btnRequest.Click += new System.EventHandler(this.btnRequest_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Verdana", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(379, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(187, 20);
            this.label1.TabIndex = 11;
            this.label1.Text = "GSTR 2A Summary";
            // 
            // msList
            // 
            this.msList.AutoSize = false;
            this.msList.BackColor = System.Drawing.Color.Transparent;
            this.msList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.msList.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.msList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msClose,
            this.msExp,
            this.importToolStripMenuItem,
            this.msDownload,
            this.msRequest,
            this.msClearAll,
            this.toolStripMenuItem1});
            this.msList.Location = new System.Drawing.Point(0, 0);
            this.msList.Name = "msList";
            this.msList.Size = new System.Drawing.Size(955, 50);
            this.msList.Stretch = false;
            this.msList.TabIndex = 182;
            this.msList.Text = "menuStrip1";
            // 
            // msClose
            // 
            this.msClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.msClose.AutoSize = false;
            this.msClose.BackColor = System.Drawing.Color.Navy;
            this.msClose.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msClose.ForeColor = System.Drawing.Color.White;
            this.msClose.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.msClose.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.msClose.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.msClose.Name = "msClose";
            this.msClose.Padding = new System.Windows.Forms.Padding(4, 0, 10, 0);
            this.msClose.Size = new System.Drawing.Size(50, 30);
            this.msClose.Text = "Close";
            this.msClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.msClose.Click += new System.EventHandler(this.msClose_Click);
            // 
            // msExp
            // 
            this.msExp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.msExp.AutoSize = false;
            this.msExp.BackColor = System.Drawing.Color.Navy;
            this.msExp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msExpExcel,
            this.msExpExcelAll});
            this.msExp.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msExp.ForeColor = System.Drawing.Color.White;
            this.msExp.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.msExp.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.msExp.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.msExp.Name = "msExp";
            this.msExp.Padding = new System.Windows.Forms.Padding(4, 0, 10, 0);
            this.msExp.Size = new System.Drawing.Size(50, 30);
            this.msExp.Text = "Export";
            this.msExp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // msExpExcel
            // 
            this.msExpExcel.AutoSize = false;
            this.msExpExcel.BackColor = System.Drawing.Color.Navy;
            this.msExpExcel.ForeColor = System.Drawing.Color.White;
            this.msExpExcel.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.msExpExcel.Name = "msExpExcel";
            this.msExpExcel.Size = new System.Drawing.Size(160, 30);
            this.msExpExcel.Text = "Excel";
            this.msExpExcel.Click += new System.EventHandler(this.msExpExcel_Click);
            // 
            // msExpExcelAll
            // 
            this.msExpExcelAll.AutoSize = false;
            this.msExpExcelAll.BackColor = System.Drawing.Color.Navy;
            this.msExpExcelAll.ForeColor = System.Drawing.Color.White;
            this.msExpExcelAll.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.msExpExcelAll.Name = "msExpExcelAll";
            this.msExpExcelAll.Size = new System.Drawing.Size(160, 30);
            this.msExpExcelAll.Text = "Export All";
            this.msExpExcelAll.Visible = false;
            this.msExpExcelAll.Click += new System.EventHandler(this.msExpExcelAll_Click);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.importToolStripMenuItem.AutoSize = false;
            this.importToolStripMenuItem.BackColor = System.Drawing.Color.Navy;
            this.importToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msImpExcel,
            this.msImpFromGSP,
            this.msImpJson});
            this.importToolStripMenuItem.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.importToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.importToolStripMenuItem.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.importToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.importToolStripMenuItem.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Padding = new System.Windows.Forms.Padding(4, 0, 10, 0);
            this.importToolStripMenuItem.Size = new System.Drawing.Size(50, 30);
            this.importToolStripMenuItem.Text = "Import";
            this.importToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // msImpExcel
            // 
            this.msImpExcel.AutoSize = false;
            this.msImpExcel.BackColor = System.Drawing.Color.Navy;
            this.msImpExcel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msImpExcel.ForeColor = System.Drawing.Color.White;
            this.msImpExcel.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.msImpExcel.Name = "msImpExcel";
            this.msImpExcel.Size = new System.Drawing.Size(160, 30);
            this.msImpExcel.Text = "Soft. Excel";
            this.msImpExcel.Visible = false;
            this.msImpExcel.Click += new System.EventHandler(this.msImpExcel_Click);
            // 
            // msImpFromGSP
            // 
            this.msImpFromGSP.AutoSize = false;
            this.msImpFromGSP.BackColor = System.Drawing.Color.Navy;
            this.msImpFromGSP.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msImpFromGSP.ForeColor = System.Drawing.Color.White;
            this.msImpFromGSP.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.msImpFromGSP.Name = "msImpFromGSP";
            this.msImpFromGSP.Size = new System.Drawing.Size(160, 30);
            this.msImpFromGSP.Text = "From GSTN";
            this.msImpFromGSP.Click += new System.EventHandler(this.msImpFromGSP_Click);
            // 
            // msImpJson
            // 
            this.msImpJson.BackColor = System.Drawing.Color.Navy;
            this.msImpJson.ForeColor = System.Drawing.Color.White;
            this.msImpJson.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.msImpJson.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.msImpJson.Name = "msImpJson";
            this.msImpJson.Size = new System.Drawing.Size(152, 22);
            this.msImpJson.Text = "JSON";
            this.msImpJson.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.msImpJson.Click += new System.EventHandler(this.msImpJson_Click);
            // 
            // msDownload
            // 
            this.msDownload.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.msDownload.AutoSize = false;
            this.msDownload.BackColor = System.Drawing.Color.Navy;
            this.msDownload.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msDownload.ForeColor = System.Drawing.Color.White;
            this.msDownload.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.msDownload.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.msDownload.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.msDownload.Name = "msDownload";
            this.msDownload.Padding = new System.Windows.Forms.Padding(4, 0, 10, 0);
            this.msDownload.Size = new System.Drawing.Size(74, 30);
            this.msDownload.Text = "Download";
            this.msDownload.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.msDownload.ToolTipText = "Download Json";
            this.msDownload.Click += new System.EventHandler(this.msDownload_Click);
            // 
            // msRequest
            // 
            this.msRequest.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.msRequest.AutoSize = false;
            this.msRequest.BackColor = System.Drawing.Color.Navy;
            this.msRequest.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msRequest.ForeColor = System.Drawing.Color.White;
            this.msRequest.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.msRequest.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.msRequest.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.msRequest.Name = "msRequest";
            this.msRequest.Padding = new System.Windows.Forms.Padding(4, 0, 10, 0);
            this.msRequest.Size = new System.Drawing.Size(60, 30);
            this.msRequest.Text = "Request";
            this.msRequest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.msRequest.ToolTipText = "Request Json";
            this.msRequest.Click += new System.EventHandler(this.msRequest_Click);
            // 
            // msClearAll
            // 
            this.msClearAll.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.msClearAll.AutoSize = false;
            this.msClearAll.BackColor = System.Drawing.Color.Navy;
            this.msClearAll.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msClearAll.ForeColor = System.Drawing.Color.White;
            this.msClearAll.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.msClearAll.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.msClearAll.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.msClearAll.Name = "msClearAll";
            this.msClearAll.Padding = new System.Windows.Forms.Padding(4, 0, 10, 0);
            this.msClearAll.Size = new System.Drawing.Size(70, 30);
            this.msClearAll.Text = "Clear All";
            this.msClearAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.msClearAll.Click += new System.EventHandler(this.msClearAll_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(23, 46);
            this.toolStripMenuItem1.Text = "``";
            this.toolStripMenuItem1.Visible = false;
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.label3);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFooter.Location = new System.Drawing.Point(0, 210);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(957, 45);
            this.pnlFooter.TabIndex = 193;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(957, 45);
            this.label3.TabIndex = 184;
            this.label3.Text = "Note : In case of more than 500 records click on the \'Request Button\' to generate" +
    " json file from GSTR &&  than click on the \'Download Button\' after 20 Minutes.";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(420, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(397, 20);
            this.label2.TabIndex = 183;
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pbGSTR1
            // 
            this.pbGSTR1.BackColor = System.Drawing.Color.Transparent;
            this.pbGSTR1.Image = ((System.Drawing.Image)(resources.GetObject("pbGSTR1.Image")));
            this.pbGSTR1.Location = new System.Drawing.Point(372, 70);
            this.pbGSTR1.Name = "pbGSTR1";
            this.pbGSTR1.Size = new System.Drawing.Size(85, 84);
            this.pbGSTR1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbGSTR1.TabIndex = 191;
            this.pbGSTR1.TabStop = false;
            this.pbGSTR1.Visible = false;
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.White;
            this.pnlMain.Controls.Add(this.pnlFooter);
            this.pnlMain.Controls.Add(this.pnlGrid);
            this.pnlMain.Controls.Add(this.pnlHeader);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(957, 545);
            this.pnlMain.TabIndex = 192;
            // 
            // pnlGrid
            // 
            this.pnlGrid.Controls.Add(this.DgvMain);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlGrid.Location = new System.Drawing.Point(0, 52);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new System.Drawing.Size(957, 158);
            this.pnlGrid.TabIndex = 193;
            // 
            // GSTR2ADashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.ClientSize = new System.Drawing.Size(957, 545);
            this.Controls.Add(this.pbGSTR1);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgvdiff);
            this.Controls.Add(this.dgvaccount);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "GSTR2ADashboard";
            this.Text = "GSTR2ADashboard";
            ((System.ComponentModel.ISupportInitialize)(this.DgvMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvdiff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvaccount)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.msList.ResumeLayout(false);
            this.msList.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbGSTR1)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.pnlGrid.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DgvMain;
        private System.Windows.Forms.DataGridView dgvdiff;
        private System.Windows.Forms.DataGridView dgvaccount;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip msList;
        private System.Windows.Forms.ToolStripMenuItem msExp;
        private System.Windows.Forms.ToolStripMenuItem msExpExcel;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem msImpExcel;
        private System.Windows.Forms.ToolStripMenuItem msImpFromGSP;
        private System.Windows.Forms.ToolStripMenuItem msClearAll;
        private System.Windows.Forms.ToolStripMenuItem msImpJson;
        private System.Windows.Forms.Button btnRequest;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.ToolStripMenuItem msRequest;
        private System.Windows.Forms.ToolStripMenuItem msDownload;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem msClose;
        private System.Windows.Forms.ToolStripMenuItem msExpExcelAll;
        private System.Windows.Forms.PictureBox pbGSTR1;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}