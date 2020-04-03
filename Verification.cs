using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Data.OleDb;
using System.Threading;
using System.Diagnostics;
using ClosedXML.Excel;

namespace SPEQTAGST_DESIGN
{
    public partial class Verification : Form
    {
        public DataTable dtGStinInfo = new DataTable();
        DataSet ds = new DataSet();
        public Verification()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void DataBind_verification(DataGridView Grd_verify)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Description");
            dt.Columns.Add("Details");



            //GSTIN
            //Trade Name of Business
            //Legal Name of Business
            //Constitution of Business
            //Taxpayer Type
            //Eligible to Collect Tax {If Taxpayer Type = Regular then Yes else No}
            //Date of Registration
            //GSTIN / UIN Status
            //Date of Cancellation
            //Centre Jurisdiction
            //State Jurisdiction


            DataRow dr = dt.NewRow();

            dr[0] = "GSTIN";
            dr[1] = "";

            dt.Rows.Add(dr);

            //DataRow dr = dt.NewRow();
            dr = dt.NewRow();
            dr[0] = "Trade Name of Business";
            dr[1] = "";

            dt.Rows.Add(dr);

            dr = dt.NewRow();

            dr[0] = "Legal Name of Business";
            dr[1] = "";

            dt.Rows.Add(dr);

            dr = dt.NewRow();

            dr[0] = "Constitution of Business";
            dr[1] = "";


            dt.Rows.Add(dr);

            dr = dt.NewRow();

            dr[0] = "Taxpayer Type";
            dr[1] = "";

            dt.Rows.Add(dr);

            dr = dt.NewRow();

            dr[0] = "Eligible to Collect Tax";
            dr[1] = "";

            dt.Rows.Add(dr);

            dr = dt.NewRow();

            dr[0] = "Date of Registration";
            dr[1] = "";

            dt.Rows.Add(dr);

            dr = dt.NewRow();

            dr[0] = "GSTIN / UIN Status";
            dr[1] = "";
            dt.Rows.Add(dr);

            dr = dt.NewRow();

            dr[0] = "Date of Cancellation ";
            dr[1] = "";
            dt.Rows.Add(dr);

            dr = dt.NewRow();

            dr[0] = "Centre Jurisdiction";
            dr[1] = "";
            dt.Rows.Add(dr);

            dr = dt.NewRow();

            dr[0] = "State Jurisdiction";
            dr[1] = "";
            dt.Rows.Add(dr);
            Grd_verify.DataSource = dt;
            Grd_verify.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

        }
        private void Verification_Load(object sender, EventArgs e)
        {
            DataBind_verification(GrdInfo);
            //DataBind_Grd_Bulk(Grd_Bulk);
            dtGStinInfo.Columns.Add("GSTIN");
            Grd_Bulk.Columns[0].Width = 70;
            Grd_Bulk.Columns[2].Width = 220;
            Grd_Bulk.Columns[3].Width = 220;
            Grd_Bulk.Columns[5].Width = 110;
            Grd_Bulk.Columns[6].Width = 100;
            Grd_Bulk.Columns[7].Width = 200;
            Grd_Bulk.DefaultCellStyle.Font = new Font("Verdana", 8, FontStyle.Bold);
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {

            TabControl tabcntrl = sender as TabControl;
            Graphics g = e.Graphics;
            Brush _textBrush;

            // Get the item from the collection.
            TabPage _tabPage = tabSoftware.TabPages[e.Index];

            // Get the real bounds for the tab rectangle.
            Rectangle _tabBounds = tabSoftware.GetTabRect(e.Index);

            if (e.State == DrawItemState.Selected)
            {
                // Draw a different background color, and don't paint a focus rectangle.


                //_textBrush = new SolidBrush(Color.Black);
                if (tabcntrl.Name == "tabGSTPortal") { _textBrush = new SolidBrush(Color.WhiteSmoke); g.FillRectangle(Brushes.Navy, e.Bounds); }
                else { _textBrush = new SolidBrush(Color.Black); g.FillRectangle(new SolidBrush(Color.FromArgb(23, 196, 187)), e.Bounds); }
                // Color.FromArgb(23,196,87)
            }
            else
            {
                //_textBrush = new SolidBrush(Color.WhiteSmoke); //e.ForeColor
                if (tabcntrl.Name == "tabGSTPortal") { _textBrush = new SolidBrush(Color.Black); g.FillRectangle(new SolidBrush(Color.FromArgb(23, 196, 187)), e.Bounds); }
                else { _textBrush = new SolidBrush(Color.WhiteSmoke); g.FillRectangle(Brushes.Navy, e.Bounds); }
                //e.DrawBackground();
            }

            // Use our own font.
            Font _tabFont = new Font("Verdana", (float)11.0, FontStyle.Bold, GraphicsUnit.Pixel);

            // Draw string. Center the text.
            StringFormat _stringFlags = new StringFormat();
            _stringFlags.Alignment = StringAlignment.Center;
            _stringFlags.LineAlignment = StringAlignment.Center;

            g.DrawString(_tabPage.Text, _tabFont, _textBrush, _tabBounds, new StringFormat(_stringFlags));
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //private void DataBind_Grd_Bulk(DataGridView Grd_Bulk)
        //{
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("Select All");
        //    dt.Columns.Add("S No.");
        //    dt.Columns.Add("GSTIN");
        //    dt.Columns.Add("Trade Name");
        //    dt.Columns.Add("Legal Name");
        //    dt.Columns.Add("Date of Registration");
        //    dt.Columns.Add("Taxpayer Type");
        //    dt.Columns.Add("GSTIN / UIN Status");
        //    dt.Columns.Add("Principal Address");
        //    dt.Columns.Add("City");
        //    dt.Columns.Add("State");

        //    DataRow dr = dt.NewRow();
        //    dr[0] = "";
        //    dr[1] = "";
        //    dr[2] = "";
        //    dr[3] = "";
        //    dr[4] = "";
        //    dr[5] = "";
        //    dr[6] = "";
        //    dr[7] = "";
        //    dr[8] = "";
        //    dr[9] = "";
        //    dr[10] = "";

        //    dt.Rows.Add(dr);

        //    Grd_Bulk.DataSource = dt;


        //}

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            if (TxtGSTIN_NO.Text.Trim() == "" || TxtGSTIN_NO.Text.Trim() == "Enter GSTIN No")
            {
                MessageBox.Show("Please Enter GSTIN NO.", "REMINDER", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SPQGstLogin frmGstLogin = new SPQGstLogin();
            frmGstLogin.lblGSTINNO.Visible = true;
            frmGstLogin.lblGSTHEADING.Visible = true;
            frmGstLogin.lblUserName.Visible = false;
            frmGstLogin.lblPassword.Visible = false;
            frmGstLogin.TxtUserName.Visible = false;
            frmGstLogin.TxtPassword.Visible = false;
            frmGstLogin.lblMendetory1.Visible = false;
            frmGstLogin.lblMendetory2.Visible = false;
            frmGstLogin.img.Location = new Point(100, 71);
            frmGstLogin.pbRefresh.Location = new Point(294, 79);
            frmGstLogin.lblCaptcha.Location = new Point(20, 142);
            frmGstLogin.lblmendetory3.Location = new Point(118, 142);
            frmGstLogin.txtCaptcha.Location = new Point(134, 141);
            frmGstLogin.btnContinue.Location = new Point(88, 191);
            frmGstLogin.btnCancel.Location = new Point(203, 191);


            frmGstLogin.lblGSTINNO.Text = TxtGSTIN_NO.Text;
            frmGstLogin.strSingle = btnVerify.AccessibleDescription;
            frmGstLogin.ShowDialog();
            //DataTable dt = frmGstLogin.dtGstinInfoss;
            DataSet dsjstinsingle = new DataSet();
            DataSet dsReturnFile = new DataSet();
            DataTable dtReturnFile = new DataTable();
            dsjstinsingle = frmGstLogin.dsPartyInfo;
            dsReturnFile = frmGstLogin.dsReturnInfo;
            dtReturnFile = frmGstLogin.dsReturnInfo.Tables["filingStatus"];
            GrdReturnfilingStatus.AutoGenerateColumns = false;



            if (dsjstinsingle.Tables.Count > 0 && dsjstinsingle.Tables[0].Rows.Count > 0)
            {

                if (dsjstinsingle.Tables["TDPARTY"].Rows[0][2].ToString() == "SWEB_9035")
                {
                    MessageBox.Show("INVALID JSTIN INSERTED...!!!", "REMINDER", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TxtGSTIN_NO.Focus();
                    dsjstinsingle.Tables.Clear();
                    return;
                }



                GrdInfo.Rows[0].Cells[1].Value = dsjstinsingle.Tables["TDPARTY"].Rows[0]["gstin"];
                GrdInfo[1, 1].Value = dsjstinsingle.Tables["TDPARTY]"].Rows[0]["tradeNam"];
                GrdInfo[1, 2].Value = dsjstinsingle.Tables["TDPARTY"].Rows[0]["lgnm"];
                GrdInfo[1, 3].Value = dsjstinsingle.Tables["TDPARTY"].Rows[0]["ctb"];
                GrdInfo[1, 4].Value = dsjstinsingle.Tables["TDPARTY"].Rows[0]["dty"];
                GrdInfo[1, 4].Value = dsjstinsingle.Tables["TDPARTY"].Rows[0]["dty"];

                if (dsjstinsingle.Tables["TDPARTY"].Rows[0]["dty"].ToString() == "Regular")
                {
                    GrdInfo[1, 5].Value = "YES";
                    GrdInfo.Rows[5].Cells[1].Style.BackColor = Color.Green;
                }
                else
                {
                    GrdInfo[1, 5].Value = "NO";
                    GrdInfo.Rows[5].Cells[1].Style.BackColor = Color.Red;
                }


                GrdInfo[1, 6].Value = dsjstinsingle.Tables["TDPARTY"].Rows[0]["rgdt"];
                GrdInfo[1, 7].Value = dsjstinsingle.Tables["TDPARTY"].Rows[0]["sts"];

                if (GrdInfo[1, 7].Value.ToString() == "Active")
                {
                    GrdInfo.Rows[7].Cells[1].Style.BackColor = Color.Green;
                }
                else
                {
                    GrdInfo.Rows[7].Cells[1].Style.BackColor = Color.Red;
                }

                GrdInfo[1, 8].Value = dsjstinsingle.Tables["TDPARTY"].Rows[0]["cxdt"];
                GrdInfo[1, 9].Value = dsjstinsingle.Tables["TDPARTY"].Rows[0]["ctj"];
                GrdInfo[1, 10].Value = dsjstinsingle.Tables["TDPARTY"].Rows[0]["stj"];
            }

            if (dsReturnFile.Tables.Count > 0 && dsReturnFile.Tables["filingStatus"].Rows.Count > 0)
            {



                //dsReturnFile.Tables["filingStatus"].Rows.RemoveAt(0);
                //dsReturnFile.Tables["filingStatus"].AcceptChanges();



                //GrdReturnfilingStatus.Columns[1].DataPropertyName = "rtntype";
                //GrdReturnfilingStatus.Columns[2].DataPropertyName = "fy";
                //GrdReturnfilingStatus.Columns[3].DataPropertyName = "taxp";
                //GrdReturnfilingStatus.Columns[4].DataPropertyName = "dof";
                //GrdReturnfilingStatus.Columns[5].DataPropertyName = "status";
                //GrdReturnfilingStatus.DataSource = dsReturnFile.Tables["filingStatus"];

                if (GrdReturnfilingStatus.Rows.Count > 0)
                {
                    GrdReturnfilingStatus.Rows.Clear();
                }


                GrdReturnfilingStatus.RowCount = dsReturnFile.Tables["filingStatus"].Rows.Count - 1;
                for (int i = 1; i < dsReturnFile.Tables["filingStatus"].Rows.Count; i++)
                {
                    GrdReturnfilingStatus[0, i - 1].Value = i;
                    GrdReturnfilingStatus[1, i - 1].Value = dsReturnFile.Tables["filingStatus"].Rows[i]["rtntype"];
                    GrdReturnfilingStatus[2, i - 1].Value = dsReturnFile.Tables["filingStatus"].Rows[i]["fy"];
                    GrdReturnfilingStatus[3, i - 1].Value = dsReturnFile.Tables["filingStatus"].Rows[i]["taxp"];
                    GrdReturnfilingStatus[4, i - 1].Value = dsReturnFile.Tables["filingStatus"].Rows[i]["dof"];
                    GrdReturnfilingStatus[5, i - 1].Value = dsReturnFile.Tables["filingStatus"].Rows[i]["status"];

                }

            }

            //06AADCS1804N1ZC
        }
        private void BtnBulkGSTINverification_Click(object sender, EventArgs e)
        {
            if (CheckForInternetConnection() == false)
            {
                MessageBox.Show("Internet is not available..!\n please check Internet connection then Veryfy ...! ", "Warning");

                return;

            }
            if (Grd_Bulk.Rows.Count <= 1)
            {
                MessageBox.Show("Please Enter GSTIN NO. Then Veryfy...!!!!", "REMINDER", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dtGStinInfo.Rows.Count > 0)
            {
                dtGStinInfo.Rows.Clear();
            }
            for (int i = 0; i < Grd_Bulk.Rows.Count - 1; i++)
            {
                DataRow dr = dtGStinInfo.NewRow();
                dr[0] = Grd_Bulk.Rows[i].Cells[1].Value.ToString();
                dtGStinInfo.Rows.Add(dr);
            }
            SPQGstLogin Gstvery = new SPQGstLogin();
            //Gstvery.lblGSTINNO.Text = TxtGSTIN_NO.Text;
            Gstvery.lblGSTINNO.Visible = false;
            Gstvery.lblGSTHEADING.Visible = false;
            Gstvery.lblUserName.Visible = true;
            Gstvery.lblPassword.Visible = true;
            Gstvery.TxtUserName.Visible = true;
            Gstvery.TxtPassword.Visible = true;
            Gstvery.lblMendetory1.Visible = true;
            Gstvery.lblMendetory2.Visible = true;
            Gstvery.dtgstIn = dtGStinInfo;
            Gstvery.strBulk = BtnBulkGSTINverification.AccessibleDescription;
            Gstvery.ShowDialog();
            //DataTable dt = Gstvery.dtGstinInfoss;
            //DataSet ds = new DataSet();
            ds = Gstvery.dsPartyInfo;

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    if (ds.Tables["TDPARTY"].Rows[j]["gstin"].ToString() == Grd_Bulk.Rows[j].Cells[1].Value.ToString())
                    {
                        Grd_Bulk.Rows[j].Cells[2].Value = ds.Tables["TDPARTY"].Rows[j]["tradeNam"].ToString();
                        Grd_Bulk.Rows[j].Cells[3].Value = ds.Tables["TDPARTY"].Rows[j]["lgnm"].ToString();
                        Grd_Bulk.Rows[j].Cells[4].Value = ds.Tables["TDPARTY"].Rows[j]["rgdt"].ToString();
                        Grd_Bulk.Rows[j].Cells[5].Value = ds.Tables["TDPARTY"].Rows[j]["dty"].ToString();

                        if (Grd_Bulk.Rows[j].Cells[5].Value.ToString() == "Regular")
                        {
                            Grd_Bulk.Rows[j].Cells[5].Style.BackColor = Color.Green;
                        }
                        else
                        {
                            Grd_Bulk.Rows[j].Cells[5].Style.BackColor = Color.Red;
                        }
                        Grd_Bulk.Rows[j].Cells[6].Value = ds.Tables["TDPARTY"].Rows[j]["sts"].ToString();


                        if (Grd_Bulk.Rows[j].Cells[6].Value.ToString() == "Active")
                        {
                            Grd_Bulk.Rows[j].Cells[6].Style.BackColor = Color.Green;
                        }
                        else
                        {
                            Grd_Bulk.Rows[j].Cells[6].Style.BackColor = Color.Red;
                        }
                        Grd_Bulk.Rows[j].Cells[7].Value = ds.Tables["TDPARTY"].Rows[j]["ctb"].ToString();


                    }
                }
            }


        }

        private void TxtGSTIN_NO_Enter(object sender, EventArgs e)
        {
            TxtGSTIN_NO.Text = "";
        }

        private void TxtGSTIN_NO_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnVerify_Click(null, null);
            }
        }

        private void Grd_Bulk_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.V)
            {
                PasteClipboard();
                //PasteFromClipboard();
            }

            if (e.KeyCode == Keys.Delete)
            {
                if (Grd_Bulk.CurrentRow.Index == 0)
                {
                    return;
                }
                if (Grd_Bulk.CurrentRow.IsNewRow == true)
                {
                    return;
                }

                Grd_Bulk.Rows.RemoveAt(Grd_Bulk.CurrentRow.Index);
                ResetSNO();
            }
        }


        private void btnImportExcell_Click(object sender, EventArgs e)
        {
            string filePath = ""; string fileExt = "";


            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {



                filePath = file.FileName;
                if (filePath == "")
                {
                    MessageBox.Show("Select Excell File To Import Data...");

                }
                else
                {
                    ImportFromExcell(filePath);
                }


                //fileExt = Path.GetExtension(filePath);

                //// CHECK SELECTED FILE EXTENTION
                //if (fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0 || fileExt.CompareTo(".xlsm") == 0)
                //{

                //    foreach (Process proc in System.Diagnostics.Process.GetProcessesByName("EXCEL"))
                //    {
                //        if (proc.MainWindowTitle == "Microsoft Excel - " + file.SafeFileName)
                //            proc.Kill();
                //    }

                //    // CREATE DATATABLE TO STORE IMPOTED FILE DATA
                //    DataTable dtExcel = new DataTable();
                //    dtExcel = ReadExcel(filePath, fileExt);

                //}

            }
        }

        private void btnExportToExcell_Click(object sender, EventArgs e)
        {
            if (Grd_Bulk.Rows.Count <= 1)
            {
                MessageBox.Show("No Records Found For Export To Excell...");
                return;
            }
            SaveFileDialog savfile = new SaveFileDialog();

            //  XLWorkbook workbook = new XLWorkbook();
            //  workbook.Worksheets.Add(ds.Tables["TDPARTY"]);

            //  string path = "";
            //  savfile.InitialDirectory = @"C:\";
            //  savfile.Filter = "Excel files (*.xlsx)|*.xlsx";
            //  savfile.FilterIndex = 0;
            //  savfile.RestoreDirectory = true;
            //  savfile.Title = "Export Excel File To";
            //  if (savfile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //  {
            //      path = savfile.FileName;
            //      workbook.SaveAs(path);
            //      System.Diagnostics.Process.Start(path);
            //  }
            //  //workbook.SaveAs("ACTIVATION.xlsx");


            //MessageBox.Show("Excel Exporting  Successfully....!!!");




            string path = "";
            savfile.InitialDirectory = @"C:\";
            savfile.Filter = "Excel files (*.xlsx)|*.xlsx";
            savfile.FilterIndex = 0;
            savfile.RestoreDirectory = true;
            savfile.Title = "Export Excel File To";
            if (savfile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = savfile.FileName;
                ExportToExcel(Grd_Bulk, path);
            }

        }

        private void GrdReturnfilingStatus_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex >= 0 || e.RowIndex == 0)
            {
                e.Value = e.RowIndex + 1;
            }
        }
        private void PasteClipboard()
        {

            string s = Clipboard.GetText();

            string[] lines = s.Replace("\n", "").Split('\r');


            //Grd_Bulk.Rows.Add(lines.Length - 1);



            string[] fields;
            //int row = 0;
            int col = 1;

            int iRow = Grd_Bulk.CurrentCell.RowIndex;
            int iCol = Grd_Bulk.CurrentCell.ColumnIndex;

            if (iRow + lines.Length > Grd_Bulk.Rows.Count - 1)
            {
                bool bFlag = false;
                foreach (string sEmpty in lines)
                {
                    if (sEmpty == "")
                    {
                        bFlag = true;
                    }
                }

                int iNewRows = iRow + lines.Length - Grd_Bulk.Rows.Count;
                if (iNewRows > 0)
                {
                    if (bFlag)
                        Grd_Bulk.Rows.Add(iNewRows);
                    else
                        Grd_Bulk.Rows.Add(iNewRows + 1);
                }
                else
                    Grd_Bulk.Rows.Add(iNewRows + 1);
            }


            foreach (string item in lines)
            {
                if (item != "")
                {
                    fields = item.Split('\t');
                    foreach (string f in fields)
                    {
                        //Console.WriteLine(f);
                        Grd_Bulk[col, iRow].Value = f;
                        Grd_Bulk[0, iRow].Value = iRow + 1; // FRO SNO
                        //col++;
                    }
                    iRow++;
                    col = 1;
                }

            }



        }

        private void PasteFromClipboard()
        {
            try
            {
                string s = Clipboard.GetText();
                string[] lines = s.Split('\n');

                int iRow = Grd_Bulk.CurrentCell.RowIndex;
                int iCol = Grd_Bulk.CurrentCell.ColumnIndex;
                DataGridViewCell oCell;
                if (iRow + lines.Length > Grd_Bulk.Rows.Count - 1)
                {
                    bool bFlag = false;
                    foreach (string sEmpty in lines)
                    {
                        if (sEmpty == "")
                        {
                            bFlag = true;
                        }
                    }

                    int iNewRows = iRow + lines.Length - Grd_Bulk.Rows.Count;
                    if (iNewRows > 0)
                    {
                        if (bFlag)
                            Grd_Bulk.Rows.Add(iNewRows);
                        else
                            Grd_Bulk.Rows.Add(iNewRows + 1);
                    }
                    else
                        Grd_Bulk.Rows.Add(iNewRows + 1);
                }
                foreach (string line in lines)
                {
                    //if (iRow < dataGridView1.RowCount && line.Length > 0)
                    if (line != "")
                    {
                        string[] sCells = line.Split('\t');
                        for (int i = 0; i < sCells.GetLength(0); ++i)
                        {
                            if (iCol + i < this.Grd_Bulk.ColumnCount)
                            {
                                oCell = Grd_Bulk[iCol + i, iRow];
                                oCell.Value = Convert.ChangeType(sCells[i].Replace("\r", ""), oCell.ValueType);
                            }
                            else
                            {
                                break;
                            }


                            //Grd_Bulk[0, iRow].Value = Convert.ChangeType(sCells[i].Replace("\r", ""), oCell.ValueType);
                        }
                        iRow++;
                    }
                    else
                    {
                        break;
                    }
                }
                //Clipboard.Clear();
            }
            catch (FormatException)
            {
                MessageBox.Show("The data you pasted is in the wrong format for the cell");
                return;
            }
        }

        private void ResetSNO()
        {
            for (int i = 0; i < Grd_Bulk.Rows.Count; i++)
            {
                Grd_Bulk[0, i].Value = i + 1;
            }
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead("http://www.google.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }






        public DataTable ReadExcel(string fileName, string fileExt)
        {
            string conn = string.Empty;
            bool flg;
            DataTable dtexcel = new DataTable();

            #region CONNECTION STRING
            if (fileExt.CompareTo(".xls") == 0)
                conn = @"provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //FOR BELOW EXCEL 2007
            //conn = @"Provider=Microsoft.Jet.OleDb.4.0; Data Source = " + fileName + "; Extended Properties = \"Excel 8.0;HDR=Yes;IMEX=1\";"; //FOR ABOVE EXCEL 2007   
            else
                //conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 12.0 Xml;HDR=Yes;IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text'"; //FOR ABOVE EXCEL 2007   

                conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties=\"Excel 12.0;HDR=YES;\"";
            #endregion

            using (OleDbConnection con = new OleDbConnection(conn))
            {
                try
                {


                    OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select GSTIN from [GSTIN$]", con);
                    oleAdpt.Fill(dtexcel); //FILL EXCEL DATA INTO DATATABLE


                }
                catch (Exception ex)
                {

                    MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                    StreamWriter errorWriter = new StreamWriter("SPEQTAGSTError.txt", true);
                    errorWriter.Write(errorMessage);
                    errorWriter.Close();
                }

                // return datatable+
                return dtexcel;
            }
        }


        public void ExportToExcel(DataGridView gridviewID, string FilePath)
        {

            Microsoft.Office.Interop.Excel.Application objexcelapp = new Microsoft.Office.Interop.Excel.Application();
            objexcelapp.Application.Workbooks.Add(Type.Missing);
            objexcelapp.Columns.ColumnWidth = 25;

            //Microsoft.Office.Interop.Excel.Range headerRange = (Microsoft.Office.Interop.Excel.Range)objexcelapp.get_Range((Microsoft.Office.Interop.Excel.Range)objexcelapp.Cells[1, 1], (Microsoft.Office.Interop.Excel.Range)objexcelapp.Cells[1, gridviewID.Columns.Count]);
            //headerRange.WrapText = true;
            //headerRange.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
            //headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            //headerRange.Font.Bold = true;
            //headerRange.Font.Name = "Calibri";

            for (int i = 1; i < gridviewID.Columns.Count + 1; i++)
            {
                objexcelapp.Cells[1, i] = gridviewID.Columns[i - 1].HeaderText;


            }
            /*For storing Each row and column value to excel sheet*/
            for (int i = 0; i < gridviewID.Rows.Count; i++)
            {
                for (int j = 0; j < gridviewID.Columns.Count; j++)
                {
                    if (gridviewID.Rows[i].Cells[j].Value != null)
                    {
                        objexcelapp.Cells[i + 2, j + 1] = gridviewID.Rows[i].Cells[j].Value.ToString();
                    }
                }
            }
            MessageBox.Show("Your excel file exported successfully " + FilePath);
            //MessageBox.Show("Your excel file exported successfully at D:\\" + excelFilename + ".xlsx");
            //objexcelapp.ActiveWorkbook.SaveCopyAs("D:\\" + excelFilename + ".xlsx");
            objexcelapp.ActiveWorkbook.SaveCopyAs(FilePath);
            objexcelapp.ActiveWorkbook.Saved = true;

        }

        public void ImportFromExcell(string filePath)
        {
            Grd_Bulk.AutoGenerateColumns = false;
            //string filePath = openFileDialog1.FileName;
            int cnt = 0;
            //Open the Excel file using ClosedXML.
            using (XLWorkbook workBook = new XLWorkbook(filePath))
            {
                //Read the first Sheet from Excel file.
                IXLWorksheet workSheet = workBook.Worksheet(1);

                //Create a new DataTable.
                DataTable dt = new DataTable();

                //Loop through the Worksheet rows.
                bool firstRow = true;
                foreach (IXLRow row in workSheet.Rows())
                {
                    //Use the first row to add columns to DataTable.
                    if (firstRow)
                    {
                        foreach (IXLCell cell in row.Cells())
                        {
                            dt.Columns.Add("SNO");
                            dt.Columns.Add(cell.Value.ToString());
                        }
                        firstRow = false;
                    }
                    else
                    {
                        //Add rows to DataTable.
                        dt.Rows.Add();
                        int i = 0;

                        foreach (IXLCell cell in row.Cells())
                        {
                            dt.Rows[dt.Rows.Count - 1][i + 1] = cell.Value.ToString();
                            dt.Rows[dt.Rows.Count - 1][i] = cnt + 1;
                            i++;
                            cnt++;
                        }
                    }
                    Grd_Bulk.Columns[0].DataPropertyName = dt.Columns[0].ColumnName;
                    Grd_Bulk.Columns[1].DataPropertyName = dt.Columns[1].ColumnName;
                    Grd_Bulk.DataSource = dt;

                    //dataGridView1.DataSource = dt;
                }
            }

        }

      

    }

   
}
    
  
    
