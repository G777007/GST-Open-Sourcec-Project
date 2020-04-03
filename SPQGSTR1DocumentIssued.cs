using SPEQTAGST.BAL;
using SPEQTAGST.BAL.M264r1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data.OleDb;
using Microsoft.VisualBasic.FileIO;
using System.Reflection;
using System.Diagnostics;
using SPEQTAGST.Usermain;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace SPEQTAGST.xasjbr1
{
    public partial class SPQGSTR1DocumentIssued : Form
    {
        r1Publicclass objGSTR13 = new r1Publicclass();

        public SPQGSTR1DocumentIssued()
        {
            InitializeComponent();
            GetData();
            //BindData();

            // total calculation
            string[] colNo = { "colTotal", "colCancelled", "colIssued" };
            GetTotal(colNo);

            SetGridViewColor();
            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName(CommonHelper.ReturnName);

            pbGSTR1.Visible = false;

            dgvGSTR13.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dgvGSTR13.EnableHeadersVisualStyles = false;
            dgvGSTR13.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);
            dgvGSTR13.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvGSTR13.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            TotaldgvGSTR13.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void GetData()
        {
            try
            {
                // create datatable to store database data
                DataTable dt = new DataTable();
                string Query = "Select * from SPQR1Document where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
                Application.DoEvents();

                // get data from database
                dt = objGSTR13.GetDataGSTR1(Query);

                if (dt != null && dt.Rows.Count > 0)
                {
                    // assign file status filed value
                    if (Convert.ToString(dt.Rows[0]["Fld_FileStatus"]).ToLower() == "draft")
                        ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(1);
                    else if (Convert.ToString(dt.Rows[0]["Fld_FileStatus"]).ToLower() == "completed")
                        ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(2);
                    else if (Convert.ToString(dt.Rows[0]["Fld_FileStatus"]).ToLower() == "not-completed")
                        ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(3);

                    // remove last column (month)
                    dt.Columns.Remove(dt.Columns[dt.Columns.Count - 1]);
                    // remove last column (file status)
                    dt.Columns.Remove(dt.Columns[dt.Columns.Count - 1]);
                    // remove first column (field id)
                    dt.Columns.Remove(dt.Columns[0]);

                    // ADD COLUMN (CHEK BOX)
                    dt.Columns.Add(new DataColumn("colChk"));
                    dt.Columns["colChk"].SetOrdinal(0);
                    #region GOODS GRID
                    //RENAME DATATABLE COLUMN NAME TO DATAGRIDVIEW COLUMN NAME
                    foreach (DataGridViewColumn col in dgvGSTR13.Columns)
                    {
                        dt.Columns[col.Index].ColumnName = col.Name.ToString();
                        col.DataPropertyName = col.Name;
                    }
                    dt.AcceptChanges();

                    dgvGSTR13.DataSource = dt;
                    #endregion
                }
                else
                {
                    ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Powre GST", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }

        private void BindData()
        {
            //try
            //{
            //    if (dgvGSTR13.Rows.Count <= 1)
            //    {
            //        DataTable dt = new DataTable();

            //        // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
            //        foreach (DataGridViewColumn col in dgvGSTR13.Columns)
            //        {
            //            dt.Columns.Add(col.Name.ToString());
            //            col.DataPropertyName = col.Name;
            //        }
            //        dt.AcceptChanges();

            //        DataRow dr = dt.NewRow();
            //        dr["colSrNo"] = "1";
            //        dr["colNatureOfDocument"] = "Invoice for outward supply";
            //        dt.Rows.Add(dr);
            //        dr = dt.NewRow();
            //        dr["colSrNo"] = "2";
            //        dr["colNatureOfDocument"] = "Invoice for inward supply from unregistered person";
            //        dt.Rows.Add(dr);
            //        dr = dt.NewRow();
            //        dr["colSrNo"] = "3";
            //        dr["colNatureOfDocument"] = "Revised Invoice";
            //        dt.Rows.Add(dr);
            //        dr = dt.NewRow();
            //        dr["colSrNo"] = "4";
            //        dr["colNatureOfDocument"] = "Debit Note";
            //        dt.Rows.Add(dr);
            //        dr = dt.NewRow();
            //        dr["colSrNo"] = "5";
            //        dr["colNatureOfDocument"] = "Credit Note";
            //        dt.Rows.Add(dr);
            //        dr = dt.NewRow();
            //        dr["colSrNo"] = "6";
            //        dr["colNatureOfDocument"] = "Receipt voucher";
            //        dt.Rows.Add(dr);
            //        dr = dt.NewRow();
            //        dr["colSrNo"] = "7";
            //        dr["colNatureOfDocument"] = "Payment Voucher";
            //        dt.Rows.Add(dr);
            //        dr = dt.NewRow();
            //        dr["colSrNo"] = "8";
            //        dr["colNatureOfDocument"] = "Refund voucher";
            //        dt.Rows.Add(dr);
            //        dr = dt.NewRow();
            //        dr["colSrNo"] = "9";
            //        dr["colNatureOfDocument"] = "Delivery Challan for job work";
            //        dt.Rows.Add(dr);
            //        dr = dt.NewRow();
            //        dr["colSrNo"] = "10";
            //        dr["colNatureOfDocument"] = "Delivery Challan for supply on approval";
            //        dt.Rows.Add(dr);
            //        dr = dt.NewRow();
            //        dr["colSrNo"] = "11";
            //        dr["colNatureOfDocument"] = "Delivery Challan in case of liquid gas";
            //        dt.Rows.Add(dr);
            //        dr = dt.NewRow();
            //        dr["colSrNo"] = "12";
            //        dr["colNatureOfDocument"] = "Delivery Challan in cases other than by way of supply (excluding at S no. 9 to 11)";
            //        dt.Rows.Add(dr);

            //        // assign datatable to main grid
            //        dgvGSTR13.DataSource = dt;

            //        DataGridViewRow row = this.dgvGSTR13.RowTemplate;
            //        row.MinimumHeight = 30;
            //    }
            //    else
            //    {
            //        DataGridViewRow row = this.dgvGSTR13.RowTemplate;
            //        row.MinimumHeight = 30;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error : " + ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
            //    StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
            //    errorWriter.Write(errorMessage);
            //    errorWriter.Close();
            //}
        }

        public void GetTotal(string[] colNo)
        {
            try
            {
                if (dgvGSTR13.Rows.Count > 1)
                {
                    // if main grid having records

                    if (TotaldgvGSTR13.Rows.Count == 0)
                    {
                        #region if total grid having no record
                        // create temprory datatable to store column calculation
                        DataTable dtTotal = new DataTable();

                        // add column as par datagridview column
                        foreach (DataGridViewColumn col in TotaldgvGSTR13.Columns)
                        {
                            dtTotal.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;
                        }

                        // create datarow to store grid column calculation
                        DataRow dr = dtTotal.NewRow();
                        dr["colTNumber"] = dgvGSTR13.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colTotal"].Value != null).Sum(x => x.Cells["colTotal"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colTotal"].Value)).ToString();
                        dr["colTCancelled"] = dgvGSTR13.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCancelled"].Value != null).Sum(x => x.Cells["colCancelled"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCancelled"].Value)).ToString();
                        dr["colTIssued"] = dgvGSTR13.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colIssued"].Value != null).Sum(x => x.Cells["colIssued"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIssued"].Value)).ToString();

                        // add datarow to datatable
                        dtTotal.Rows.Add(dr);
                        dtTotal.AcceptChanges();

                        // assign datatable to grid
                        TotaldgvGSTR13.DataSource = dtTotal;
                        #endregion
                    }
                    else if (TotaldgvGSTR13.Rows.Count == 1)
                    {
                        #region if total grid having only one records

                        // calculate total only specific column
                        foreach (var item in colNo)
                        {
                            if (item == "colTotal")
                                TotaldgvGSTR13.Rows[0].Cells["colTNumber"].Value = dgvGSTR13.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colTotal"].Value != null).Sum(x => x.Cells["colTotal"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colTotal"].Value)).ToString();
                            else if (item == "colCancelled")
                                TotaldgvGSTR13.Rows[0].Cells["colTCancelled"].Value = dgvGSTR13.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCancelled"].Value != null).Sum(x => x.Cells["colCancelled"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCancelled"].Value)).ToString();
                            else if (item == "colIssued")
                                TotaldgvGSTR13.Rows[0].Cells["colTIssued"].Value = dgvGSTR13.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colIssued"].Value != null).Sum(x => x.Cells["colIssued"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIssued"].Value)).ToString();
                        }
                        #endregion
                    }

                    TotaldgvGSTR13.Rows[0].Cells[0].Value = "TOTAL";
                    TotaldgvGSTR13.Rows[0].Height = 30;
                }
                else
                {
                    // check if total grid having record

                    if (TotaldgvGSTR13.Rows.Count >= 0)
                    {
                        #region if there are no records in main grid then assign blank datatable to total grid
                        DataTable dtTotal = new DataTable();
                        foreach (DataGridViewColumn col in TotaldgvGSTR13.Columns)
                        {
                            dtTotal.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;
                        }
                        TotaldgvGSTR13.DataSource = dtTotal;
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }

        private void dgvGSTR13_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                pbGSTR1.Visible = true;

                // if user wants to delete data
                if (e.KeyCode == Keys.Delete)
                {
                    #region DELETE SELECTED CELLS
                    try
                    {
                        // check main grid having records
                        if (dgvGSTR13.Rows.Count > 0)
                        {
                            // delete selected cell in grid
                            foreach (DataGridViewCell oneCell in dgvGSTR13.SelectedCells)
                            {
                                // check box column (0,12), sequance column (1) data do not delete
                                if (oneCell.Selected && oneCell.ColumnIndex != 0 && oneCell.ColumnIndex != 1)
                                {
                                    oneCell.ValueType.Name.ToString();
                                    oneCell.ValueType.FullName.ToString();
                                    if (oneCell.ValueType.Name.ToString() == "Double")
                                        oneCell.Value = DBNull.Value;
                                    else
                                        oneCell.Value = "";
                                }
                                Application.DoEvents();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    #endregion

                    // total calculation
                    string[] colNo = { "colTotal", "colCancelled", "colIssued" };
                    GetTotal(colNo);
                }

                // if user wants to paste data
                if (e.KeyCode == Keys.V)
                {
                    #region PAST FROM EXCELL SHEET

                    // get copied data to string
                    string s = Clipboard.GetText();
                    string[] lines = s.Split('\n');
                    int iRow = 0, iCol = 0;

                    #region PAST ON SELECTED CELLS ONLY
                    if (dgvGSTR13.RowCount > 0) // IF GRID IS NOT NULL AND PAST ONLY ON SELECTED CELLS
                    {
                        foreach (DataGridViewCell oneCell in dgvGSTR13.SelectedCells)
                        {
                            if (oneCell.Selected)
                            {
                                iCol = oneCell.ColumnIndex;
                                iRow = oneCell.RowIndex;
                            }
                        }
                    }
                    #endregion

                    DataGridViewCell oCell;
                    int gRowNo = 0, tmp = 0;

                    foreach (string line in lines)
                    {
                        if (line != "")
                        {
                            // disable main grid
                            DisableControls(dgvGSTR13);

                            gRowNo = dgvGSTR13.Rows.Count - 1;
                            int no = tmp;

                            if (iRow > gRowNo - 1)
                            {
                                DataTable dtDGV = new DataTable();
                                //dtDGV = dgvGSTR13.DataSource as DataTable;

                                #region ADD DATATABLE COLUMN

                                // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                                foreach (DataGridViewColumn col in dgvGSTR13.Columns)
                                {
                                    dtDGV.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }
                                #endregion

                                #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                                // CREATE OBJECT ARRAY TO STORE ONE ROW DATA OF MAIN GRID
                                object[] rowValue = new object[dtDGV.Columns.Count];

                                foreach (DataGridViewRow dr in dgvGSTR13.Rows)
                                {
                                    if (dr.Index != dgvGSTR13.Rows.Count - 1) // DON'T ADD LAST ROW
                                    {
                                        // SET CHECK BOX VALUE
                                        rowValue[0] = "False";
                                        for (int i = 1; i < dr.Cells.Count; i++)
                                        {
                                            rowValue[i] = Convert.ToString(dr.Cells[i].Value);
                                        }

                                        // ADD ARRAY OF GRID ROW VALUE TO DATATABLE AS ROW
                                        dtDGV.Rows.Add(rowValue);
                                    }
                                }
                                dtDGV.AcceptChanges();
                                #endregion

                                // PASTE DATA AT ADDITIONAL GRID ROW
                                GridRowPaste(dtDGV, tmp, iCol, lines);
                                return;
                            }
                            else
                            {
                                // paste data to existing row in grid
                                if (line.Length > 0)
                                {
                                    #region Row Paste
                                    // split one copied row data to array
                                    string[] sCells = line.Split('\t');

                                    for (int i = 0; i < sCells.GetLength(0); ++i)
                                    {
                                        // check grid column count
                                        if (iCol + i < this.dgvGSTR13.ColumnCount && i < 7)
                                        {
                                            // skip check box column and sequance column to paste data
                                            if (iCol == 0)
                                                oCell = dgvGSTR13[iCol + i + 2, iRow];
                                            else if (iCol == 1)
                                                oCell = dgvGSTR13[iCol + i + 1, iRow];
                                            else
                                                oCell = dgvGSTR13[iCol + i, iRow];

                                            sCells[i] = sCells[i].Trim().Replace(",", "");
                                            if (oCell.ColumnIndex != 0)
                                            {
                                                if (dgvGSTR13.Columns[oCell.ColumnIndex].Name != "colChk" && dgvGSTR13.Columns[oCell.ColumnIndex].Name != "colSrNo")
                                                {
                                                    #region VALIDATION
                                                    if (sCells[i].ToString().Trim() == "") { dgvGSTR13.Rows[iRow].Cells[oCell.ColumnIndex].Value = "";}
                                                    else
                                                    {
                                                        if (oCell.ColumnIndex >= 2 && oCell.ColumnIndex <= 8)
                                                        {
                                                            if (chkCellValue(sCells[i].Trim(), dgvGSTR13.Columns[oCell.ColumnIndex].Name))
                                                            {
                                                                if (dgvGSTR13.Columns[oCell.ColumnIndex].Name == "colNatureOfDocument")
                                                                    dgvGSTR13.Rows[iRow].Cells[oCell.ColumnIndex].Value = Utility.StrNatureOfDocument(sCells[i]);
                                                                else
                                                                    dgvGSTR13.Rows[iRow].Cells[oCell.ColumnIndex].Value = sCells[i].Trim();
                                                            }
                                                            else
                                                                dgvGSTR13.Rows[iRow].Cells[oCell.ColumnIndex].Value = "";
                                                        }
                                                        else { dgvGSTR13.Rows[iRow].Cells[oCell.ColumnIndex].Value = sCells[i].Trim(); }
                                                    }
                                                    #endregion
                                                }
                                            }
                                            else
                                            {
                                                if (iCol > i)
                                                {
                                                    for (int j = oCell.ColumnIndex; j < dgvGSTR13.Columns.Count; j++)
                                                    {
                                                        #region VALIDATION
                                                        if (sCells[i].ToString().Trim() == "") { dgvGSTR13.Rows[iRow].Cells[j].Value = DBNull.Value; }
                                                        else
                                                        {
                                                            if (j >= 2 && j <= 8)
                                                            {
                                                                if (chkCellValue(sCells[i].Trim(), dgvGSTR13.Columns[j].Name))
                                                                {
                                                                    if (dgvGSTR13.Columns[j].Name == "colNatureOfDocument")
                                                                        dgvGSTR13.Rows[iRow].Cells[j].Value = Utility.StrNatureOfDocument(sCells[i]);
                                                                    else
                                                                        dgvGSTR13.Rows[iRow].Cells[j].Value = sCells[i].Trim();
                                                                }
                                                                else
                                                                    dgvGSTR13.Rows[iRow].Cells[j].Value = "";
                                                            }
                                                            else { dgvGSTR13.Rows[iRow].Cells[j].Value = sCells[i].Trim(); }
                                                        }
                                                        #endregion

                                                        i++;
                                                        if (i >= sCells.Length)
                                                        {
                                                            break;
                                                        }
                                                    }
                                                    break;
                                                }
                                                else
                                                {
                                                    for (int j = oCell.ColumnIndex; j < dgvGSTR13.Columns.Count; j++)
                                                    {
                                                        #region VALIDATION
                                                        if (sCells[i].ToString().Trim() == "") { dgvGSTR13.Rows[iRow].Cells[j].Value = DBNull.Value; }
                                                        else
                                                        {
                                                            if (j >= 2 && j <= 8)
                                                            {
                                                                if (chkCellValue(sCells[i].Trim(), dgvGSTR13.Columns[j].Name))
                                                                {
                                                                    if (dgvGSTR13.Columns[j].Name == "colNatureOfDocument")
                                                                        dgvGSTR13.Rows[iRow].Cells[j].Value = Utility.StrNatureOfDocument(sCells[i]);
                                                                    else
                                                                        dgvGSTR13.Rows[iRow].Cells[j].Value = sCells[i].Trim();
                                                                }
                                                                else
                                                                    dgvGSTR13.Rows[iRow].Cells[j].Value = "";
                                                            }
                                                            else { dgvGSTR13.Rows[iRow].Cells[j].Value = sCells[i].Trim(); }
                                                        }
                                                        #endregion

                                                        i = i + 1;
                                                        if (i >= sCells.Length)
                                                        {
                                                            break;
                                                        }
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    iRow++;
                                    #endregion
                                }
                            }
                        }
                        tmp++;
                    }
                    #endregion

                    // enabal main grid
                    EnableControls(dgvGSTR13);
                }

                // disable cntr + A for select whole grid row or cntr + minus for delete whole row or shift + space for select whole row or cntr + F4 for close application
                if ((e.Control && (e.KeyCode == Keys.A || e.KeyCode == Keys.Subtract)) || (e.KeyCode == Keys.Space && Control.ModifierKeys == Keys.Shift) || (e.Alt && e.KeyCode == Keys.F4))
                {
                    e.Handled = true;
                }

                pbGSTR1.Visible = false;
            }
            catch (Exception ex)
            {
                pbGSTR1.Visible = false;
                EnableControls(dgvGSTR13);
                MessageBox.Show(ex.Message);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }

        private void GridRowPaste(DataTable dtDGV, int lineNo, int iCol, string[] lines)
        {
            try
            {
                DisableControls(dgvGSTR13);

                #region Set datatable
                int cnt = 0, colNo = 0;

                // assign grid data to datatable
                DataTable dt = dtDGV;

                if (dt == null)
                {
                    // if no record in grid then create new daatable
                    dt = new DataTable();

                    // add column as par main grid and set data access property
                    foreach (DataGridViewColumn col in dgvGSTR13.Columns)
                    {
                        dt.Columns.Add(col.Name.ToString());
                        col.DataPropertyName = col.Name;
                    }
                }
                #endregion

                foreach (string line in lines)
                {
                    colNo = 0;
                    if (cnt >= lineNo)
                    {
                        if (line != "" && line.Length > 0)
                        {
                            // add data row to datatable
                            DataRow dtRow = dt.NewRow();
                            dt.Rows.Add(dtRow);

                            #region Row Paste
                            string[] sCells = line.Split('\t');

                            for (int i = 0; i < sCells.GetLength(0); ++i)
                            {
                                // check grid column count
                                if (iCol + i < this.dgvGSTR13.ColumnCount && colNo < 8)
                                {
                                    // skip check box column and sequance column to paste data
                                    if (iCol == 0)
                                        colNo = iCol + i + 2;
                                    else if (iCol == 1)
                                        colNo = iCol + i + 1;

                                    else
                                        colNo = iCol + i;

                                    sCells[i] = sCells[i].Trim().Replace(",", "");
                                    if (colNo != 0 || colNo != 2)
                                    {
                                        if (dt.Columns[colNo].ColumnName != "colChk")
                                        {
                                            #region VALIDATION
                                            if (sCells[i].ToString().Trim() == "") { dt.Rows[dt.Rows.Count - 1][colNo] = DBNull.Value; }
                                            else
                                            {
                                                if (colNo >= 2 && colNo <= 8)
                                                {
                                                    if (chkCellValue(sCells[i].Trim(), dgvGSTR13.Columns[colNo].Name))
                                                    {
                                                        if (dgvGSTR13.Columns[colNo].Name == "colNatureOfDocument")
                                                            dt.Rows[dt.Rows.Count - 1][colNo] = Utility.StrNatureOfDocument(sCells[i]);
                                                        else
                                                            dt.Rows[dt.Rows.Count - 1][colNo] = sCells[i].Trim();
                                                    }
                                                    else
                                                        dt.Rows[dt.Rows.Count - 1][colNo] = DBNull.Value;
                                                }
                                                else { dt.Rows[dt.Rows.Count - 1][colNo] = sCells[i].Trim(); }
                                            }
                                            #endregion
                                        }
                                    }
                                    else
                                    {
                                        if (iCol > i)
                                        {
                                            for (int j = colNo; j < dgvGSTR13.Columns.Count; j++)
                                            {
                                                #region VALIDATION
                                                if (sCells[i].ToString().Trim() == "") { dt.Rows[dt.Rows.Count - 1][j] = DBNull.Value; }
                                                else
                                                {
                                                    if (j >= 2 && j <= 8)
                                                    {
                                                        if (chkCellValue(sCells[i].Trim(), dgvGSTR13.Columns[j].Name))
                                                        {
                                                            if (dgvGSTR13.Columns[j].Name == "colNatureOfDocument")
                                                                dt.Rows[dt.Rows.Count - 1][j] = Utility.StrNatureOfDocument(sCells[i]);
                                                            else
                                                                dt.Rows[dt.Rows.Count - 1][j] = sCells[i].Trim();
                                                        }
                                                        else
                                                            dt.Rows[dt.Rows.Count - 1][j] = DBNull.Value;
                                                    }
                                                    else { dt.Rows[dt.Rows.Count - 1][j] = sCells[i].Trim(); }
                                                }
                                                #endregion

                                                i++;
                                                if (i >= sCells.Length)
                                                {
                                                    break;
                                                }
                                            }
                                            break;
                                        }
                                        else
                                        {
                                            for (int j = colNo; j < dgvGSTR13.Columns.Count; j++)
                                            {
                                                #region VALIDATION
                                                if (sCells[i].ToString().Trim() == "") { dt.Rows[dt.Rows.Count - 1][j] = DBNull.Value; }
                                                else
                                                {
                                                    if (j >= 2 && j <= 8)
                                                    {
                                                        if (chkCellValue(sCells[i].Trim(), dgvGSTR13.Columns[j].Name))
                                                        {
                                                            if (dgvGSTR13.Columns[j].Name == "colNatureOfDocument")
                                                                dt.Rows[dt.Rows.Count - 1][j] = Utility.StrNatureOfDocument(sCells[i]);
                                                            else
                                                                dt.Rows[dt.Rows.Count - 1][j] = sCells[i].Trim();
                                                        }
                                                        else
                                                            dt.Rows[dt.Rows.Count - 1][j] = DBNull.Value;
                                                    }
                                                    else { dt.Rows[dt.Rows.Count - 1][j] = sCells[i].Trim(); }
                                                }
                                                #endregion

                                                i = i + 1;
                                                if (i >= sCells.Length)
                                                {
                                                    break;
                                                }
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                            #endregion

                            Application.DoEvents();
                            dt.Rows[dt.Rows.Count - 1]["colChk"] = "False";
                            dt.Rows[dt.Rows.Count - 1]["colSrNo"] = dt.Rows.Count;
                        }
                    }
                    cnt++;
                }

                #region Export datatable to grid

                // if there are records in data table then assign it to grid
                if (dt != null && dt.Rows.Count > 0)
                    dgvGSTR13.DataSource = dt;

                // total calculation

                string[] colGroup = { "colTotal", "colCancelled", "colIssued" };
                GetTotal(colGroup);

                pbGSTR1.Visible = false;

                EnableControls(dgvGSTR13);

                #endregion
            }
            catch (Exception ex)
            {
                pbGSTR1.Visible = false;
                EnableControls(dgvGSTR13);
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }

        public bool IsValidateData()
        {
            try
            {
                int _cnt = 0;
                string _str = "",colName= "";

                pbGSTR1.Visible = true;
                dgvGSTR13.CurrentCell = dgvGSTR13.Rows[0].Cells[0];
                dgvGSTR13.AllowUserToAddRows = false;


                #region Nature of Document
                colName = "colNatureOfDocument";
                List<DataGridViewRow> list = dgvGSTR13.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != chkNatureOfDoc(Convert.ToString(x.Cells[colName].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR13.Rows[list[i].Cells[colName].RowIndex].Cells[colName].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please select proper Nature of document.\n";
                }
                list = dgvGSTR13.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == chkNatureOfDoc(Convert.ToString(x.Cells[colName].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR13.Rows[list[i].Cells[colName].RowIndex].Cells[colName].Style.BackColor = Color.White;
                }
                #endregion

                #region Sr. No. From
                colName = "colFrom";
                list = null;
                list = dgvGSTR13.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsInvoiceNumber(Convert.ToString(x.Cells[colName].Value)) || "" == Convert.ToString(x.Cells[colName].Value))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR13.Rows[list[i].Cells[colName].RowIndex].Cells[colName].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper Sr. No. From and can consist only(/) and (-).\n";
                }
                list = dgvGSTR13.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsInvoiceNumber(Convert.ToString(x.Cells[colName].Value)) && "" != Convert.ToString(x.Cells[colName].Value))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR13.Rows[list[i].Cells[colName].RowIndex].Cells[colName].Style.BackColor = Color.White;
                }
                #endregion

                #region Sr. No. To
                colName = "colTo";
                list = null;
                list = dgvGSTR13.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsInvoiceNumber(Convert.ToString(x.Cells[colName].Value)) || "" == Convert.ToString(x.Cells[colName].Value))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR13.Rows[list[i].Cells[colName].RowIndex].Cells[colName].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper Sr. No. To and can consist only(/) and (-).\n";
                }
                list = dgvGSTR13.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsInvoiceNumber(Convert.ToString(x.Cells[colName].Value)) && "" != Convert.ToString(x.Cells[colName].Value))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR13.Rows[list[i].Cells[colName].RowIndex].Cells[colName].Style.BackColor = Color.White;
                }
                #endregion

                #region Total Number
                colName = "colTotal";
                list = null;
                list = dgvGSTR13.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsNumber(Convert.ToString(x.Cells[colName].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR13.Rows[list[i].Cells[colName].RowIndex].Cells[colName].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper Total Number\n";
                }
                list = dgvGSTR13.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsNumber(Convert.ToString(x.Cells[colName].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR13.Rows[list[i].Cells[colName].RowIndex].Cells[colName].Style.BackColor = Color.White;
                }
                #endregion

                #region Cancelled
                colName = "colCancelled";
                list = null;
                list = dgvGSTR13.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsNumber(Convert.ToString(x.Cells[colName].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR13.Rows[list[i].Cells[colName].RowIndex].Cells[colName].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper Cancelled\n";
                }
                list = dgvGSTR13.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsNumber(Convert.ToString(x.Cells[colName].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR13.Rows[list[i].Cells[colName].RowIndex].Cells[colName].Style.BackColor = Color.White;
                }
                #endregion

                #region Net Issued
                colName = "colIssued";
                list = null;
                list = dgvGSTR13.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsNumber(Convert.ToString(x.Cells[colName].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR13.Rows[list[i].Cells[colName].RowIndex].Cells[colName].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper Net Issued\n";
                }
                list = dgvGSTR13.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsNumber(Convert.ToString(x.Cells[colName].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR13.Rows[list[i].Cells[colName].RowIndex].Cells[colName].Style.BackColor = Color.White;
                }
                #endregion

                dgvGSTR13.AllowUserToAddRows = true;
                pbGSTR1.Visible = false;

                if (_str != "")
                {
                    int _Result = objGSTR13.InsertValidationFlg("GSTR1", "DOC-ISSUE", "false", CommonHelper.SelectedMonth);
                    if (_Result != 1)
                        MessageBox.Show("System error.\nPlease try after sometime! SPQValidation Error", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    CommonHelper.ErrorList = Convert.ToString(_str);
                    SPQErrorList obj = new SPQErrorList();
                    obj.ShowDialog();


                    return false;
                }
                else
                {
                    int _Result = objGSTR13.InsertValidationFlg("GSTR1", "DOC-ISSUE", "true", CommonHelper.SelectedMonth);
                    if (_Result != 1)
                        MessageBox.Show("System error.\nPlease try after sometime! SPQValidation Error", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    MessageBox.Show("Data Validation Successful.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    CommonHelper.StatusText = "Completed";
                    return true;
                }
            }
            catch (Exception ex)
            {
                pbGSTR1.Visible = false;
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
                return false;
            }
        }

        private Boolean chkCellValue(string cellValue, string cNo)
        {
            try
            {
                if (cellValue.Trim() != "")// NOT EQUEL BLANK
                {
                    if (cNo == "colTotal" || cNo == "colCancelled" || cNo == "colIssued")
                    {
                        if (Utility.IsNumber(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colFrom" || cNo == "colTo")
                    {
                        if (Utility.IsInvoiceNumber(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colNatureOfDocument")
                    {
                        if (Utility.NatureOfDocument(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else
                        return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
                return false;
            }
        }

        private void dgvGSTR13_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string cNo = dgvGSTR13.Columns[e.ColumnIndex].Name;

                if (e.RowIndex >= 0)
                {
                    if (cNo == "colTotal" || cNo == "colCancelled" || cNo == "colIssued")
                    {
                        if (chkCellValue(Convert.ToString(dgvGSTR13.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                        {
                            string[] colNo = { dgvGSTR13.Columns[e.ColumnIndex].Name };
                            GetTotal(colNo);
                        }
                    }
                    else if (cNo == "colFrom" || cNo == "colTo")
                    {
                        if (!chkCellValue(Convert.ToString(dgvGSTR13.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                            dgvGSTR13.Rows[e.RowIndex].Cells[cNo].Value = "";
                    }
                    else if (cNo == "colNatureOfDocument")
                    {
                        if (!chkCellValue(Convert.ToString(dgvGSTR13.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                            dgvGSTR13.Rows[e.RowIndex].Cells[cNo].Value = Utility.StrNatureOfDocument(Convert.ToString(dgvGSTR13.Rows[e.RowIndex].Cells[cNo].Value));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }
        
        public void Save()
        {
            try
            {
                //if (CommonHelper.StatusIndex == 0)
                //{
                //    MessageBox.Show("Please Select File Status!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}

                pbGSTR1.Visible = true;

                #region ADD DATATABLE COLUMN

                // create datatable to store main grid data
                DataTable dt = new DataTable();

                // add datatble collumn as par main  grid column
                foreach (DataGridViewColumn col in dgvGSTR13.Columns)
                {
                    dt.Columns.Add(col.Name.ToString());
                }

                // add datatable column to store file status
                dt.Columns.Add("colFileStatus");

                #endregion

                #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                // create object array to store one row data of main grid
                object[] rowValue = new object[dt.Columns.Count];

                foreach (DataGridViewRow dr in dgvGSTR13.Rows)
                {
                    if (dr.Index != dgvGSTR13.Rows.Count - 1) // DON'T ADD LAST ROW
                    {
                        for (int i = 0; i < dr.Cells.Count; i++)
                        {
                            rowValue[i] = Convert.ToString(dr.Cells[i].Value);
                        }

                        // assign file status value with each grid row
                        rowValue[dr.Cells.Count] = Convert.ToString(CommonHelper.StatusText);

                        // add array of grid row value to datatable as row
                        dt.Rows.Add(rowValue);
                    }
                }

                dt.AcceptChanges();
                #endregion

                #region RECORD SAVE
                string Query = "";
                int _Result = 0;

                // check there are records in grid
                if (dt != null && dt.Rows.Count > 0)
                {
                    #region first delete old data from database
                    Query = "Delete from SPQR1Document where Fld_Month='" + CommonHelper.SelectedMonth + "'";
                    _Result = objGSTR13.IUDData(Query);
                    if (_Result != 1)
                    {
                        // error occurs while deleting data
                        pbGSTR1.Visible = false;
                        MessageBox.Show("System error.\nPlease try after sometime!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    #endregion

                    // query fire to save records to database
                    _Result = objGSTR13.GSTR13BulkEntry(dt, Convert.ToString(CommonHelper.StatusText));

                    if (_Result == 1)
                    {

                        string[] colNo = { "colTotal", "colCancelled", "colIssued" };
                        GetTotal(colNo);

                        #region ADD DATATABLE COLUMN

                        // CREATE DATATABLE TO STORE MAIN GRID DATA
                        dt = new DataTable();

                        // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                        foreach (DataGridViewColumn col in dgvGSTR13.Columns)
                        {
                            dt.Columns.Add(col.Name.ToString());
                        }

                        // ADD DATATABLE COLUMN TO STORE FILE STATUS
                        dt.Columns.Add("colFileStatus");

                        #endregion

                        #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                        // CREATE OBJECT ARRAY TO STORE ONE ROW DATA OF MAIN GRID
                        object[] rowVal = new object[dt.Columns.Count];

                        if (TotaldgvGSTR13.Rows.Count == 1)
                        {
                            foreach (DataGridViewRow dr in TotaldgvGSTR13.Rows)
                            {
                                for (int i = 0; i < dr.Cells.Count; i++)
                                {
                                    rowVal[i] = Convert.ToString(dr.Cells[i].Value);
                                }

                                // ASSIGN FILE STATUS VALUE WITH EACH GRID ROW
                                rowVal[dr.Cells.Count] = "Total";

                                // ADD ARRAY OF GRID ROW VALUE TO DATATABLE AS ROW
                                dt.Rows.Add(rowVal);
                            }
                        }
                        dt.AcceptChanges();
                        #endregion

                        _Result = objGSTR13.GSTR13BulkEntry(dt, "Total");

                        if (_Result == 1)
                        {
                            //DONE
                            MessageBox.Show("Data Saved Successfully …!!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // BIND DATA
                            GetData();
                            BindData();
                        }
                        else
                        {
                            // IF ERRORS OCCURS WHILE INSERTING DATA TO DATABASE
                            pbGSTR1.Visible = false;
                            MessageBox.Show("System error.\nPlease try after sometime!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        // if errors ocurs while saving record from the database
                        pbGSTR1.Visible = false;
                        MessageBox.Show("System error.\nPlease try after sometime!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    #region delete all old record if there are no records present in grid
                    Query = "Delete from SPQR1Document where Fld_Month='" + CommonHelper.SelectedMonth + "'";

                    // fire queary to delete records
                    _Result = objGSTR13.IUDData(Query);

                    if (_Result == 1)
                    {
                        // if records deleted from database
                        MessageBox.Show("Record Successfully Deleted!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // make file status blank
                        ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(0);
                    }
                    else
                    {
                        // if errors ocurs while deleting record from the database
                        pbGSTR1.Visible = false;
                        MessageBox.Show("System error.\nPlease try after sometime!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    #endregion
                }
                #endregion

                pbGSTR1.Visible = false;
            }
            catch (Exception ex)
            {
                pbGSTR1.Visible = false;
                MessageBox.Show("Error : " + ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }

        public void Delete()
        {
            try
            {
                if (dgvGSTR13.Rows.Count == 1)
                {
                    ckboxHeader.Checked = false;
                    return;
                }
                if (dgvGSTR13.CurrentCell.RowIndex == 0 && dgvGSTR13.CurrentCell.ColumnIndex == 0)
                {
                    dgvGSTR13.CurrentCell = dgvGSTR13.Rows[0].Cells["colSrNo"];
                }
                else { dgvGSTR13.CurrentCell = dgvGSTR13.Rows[0].Cells["colChk"]; }

                // CREATE FLAG FRO DELETE ROWS
                Boolean flgChk = false; Boolean flgSelect = false;

                // CREATE OBJECT OF SELECTED ROW TO DELETE
                List<DataGridViewRow> toDelete = new List<DataGridViewRow>();

                // CHECK THERE ARE RECORD PRESENT IN GRID
                if (dgvGSTR13.Rows.Count > 1)
                {
                    // FLAG TRUE IF CHECK ALL SELECTED
                    if (ckboxHeader.Checked)
                        flgChk = true;

                    #region ADD SELECTED ROW TO OBJECT FOR DELETE
                    for (int i = 0; i < dgvGSTR13.Rows.Count - 1; i++)
                    {
                        if (dgvGSTR13[0, i].Value != null && dgvGSTR13[0, i].Value.ToString() != "")
                        {
                            if (Convert.ToBoolean(dgvGSTR13[0, i].Value) == true)
                            {
                                // ADD ROW TO OBJECT IF ROW IS SELECTED
                                flgSelect = true;
                                toDelete.Add(dgvGSTR13.Rows[i]);
                            }
                        }
                    }
                    #endregion

                    // CHECK ROW IS SELECTED TO DELETE
                    if (flgChk || flgSelect)
                    {
                        // OPEN DIALOG FOR THE CONFIRMATION
                        DialogResult result = MessageBox.Show("Do you want to delete this data?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        // IF USER CONFIRM FOR DELETING RECORDS
                        if (result == DialogResult.Yes)
                        {
                            pbGSTR1.Visible = false;

                            #region DELETE RECORDS


                            if (flgChk)
                            {
                                // IF CHECK BOX OF CHECK ALL IS SELECTED
                                flgChk = false;

                                // CREATE DATATABLE AND ADD COLUMN AS PAR MAIN GRID
                                DataTable dt = new DataTable();
                                foreach (DataGridViewColumn col in dgvGSTR13.Columns)
                                {
                                    dt.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }

                                // ASSIGN BLANK DATATABLE TO GRID
                                dgvGSTR13.DataSource = dt;
                            }
                            else if (flgSelect == true)
                            {
                                // DELETE SELECTED ROW
                                foreach (DataGridViewRow row in toDelete)
                                {
                                    dgvGSTR13.Rows.RemoveAt(row.Index);
                                    Application.DoEvents();
                                }
                            }

                            // SEQUANCING MAIN GRID RECORDS
                            for (int i = 0; i < dgvGSTR13.Rows.Count - 1; i++)
                            {
                                dgvGSTR13.Rows[i].Cells["colSrNo"].Value = Convert.ToString(i + 1);
                            }

                            if (dgvGSTR13.Rows.Count == 1)
                            {
                                // IF THERE ARE NO RECORDS IN MAIN GRID THENE ASSIGN BLANK DATATABLE TO TOTAL GRID
                                DataTable dtTotal = new DataTable();
                                foreach (DataGridViewColumn col in TotaldgvGSTR13.Columns)
                                {
                                    dtTotal.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }
                                TotaldgvGSTR13.DataSource = dtTotal;
                            }

                            // SET CONTROL PROPERTY AFTER ROW DELETION
                            ckboxHeader.Checked = false;
                            dgvGSTR13.Columns[0].HeaderText = "Check All";
                            #endregion
                        }
                    }
                    // TOTAL CALCULATION
                    string[] colNo = { "colTotal", "colCancelled", "colIssued" };

                    GetTotal(colNo);
                    pbGSTR1.Visible = false;
                }
                else
                {
                    // IF THERE ARE NO RECORD TO DELETE
                    ckboxHeader.Checked = false;
                    dgvGSTR13.Columns[0].HeaderText = "Check All";
                    MessageBox.Show("There are no records to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                pbGSTR1.Visible = false;
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }

        #region EXCEL TRANSACTIONS

        public void ImportExcel()
        {
            try
            {
                string filePath = string.Empty; string fileExt = string.Empty;

                //OPEN DIALOG TO CHOOSE FILE
                OpenFileDialog file = new OpenFileDialog();
                if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // GET FILE NAME AND EXTENTION OF SELECTED FILE
                    filePath = file.FileName;
                    fileExt = Path.GetExtension(filePath);

                    // CHECK SELECTED FILE EXTENTION
                    if (fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0 || fileExt.CompareTo(".xlsm") == 0)
                    {
                        pbGSTR1.Visible = true;

                        #region IF IMPOTED FILE IS OPEN THEN CLOSE OPEN FILE
                        foreach (Process proc in System.Diagnostics.Process.GetProcessesByName("EXCEL"))
                        {
                            if (proc.MainWindowTitle == "Microsoft Excel - " + file.SafeFileName)
                                proc.Kill();
                        }
                        #endregion

                        // CREATE DATATABLE TO STORE MAIN GRID DATA
                        DataTable dt = new DataTable();
                        #region ADD DATATABLE COLUMN

                        // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                        foreach (DataGridViewColumn col in dgvGSTR13.Columns)
                        {
                            dt.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;
                        }
                        #endregion

                        #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                        // CREATE OBJECT ARRAY TO STORE ONE ROW DATA OF MAIN GRID
                        object[] rowValue = new object[dt.Columns.Count];

                        foreach (DataGridViewRow dr in dgvGSTR13.Rows)
                        {
                            if (dr.Index != dgvGSTR13.Rows.Count - 1) // DON'T ADD LAST ROW
                            {
                                // SET CHECK BOX VALUE
                                rowValue[0] = "False";
                                for (int i = 1; i < dr.Cells.Count; i++)
                                {
                                    rowValue[i] = Convert.ToString(dr.Cells[i].Value);
                                }

                                // ADD ARRAY OF GRID ROW VALUE TO DATATABLE AS ROW
                                dt.Rows.Add(rowValue);
                            }
                        }
                        dt.AcceptChanges();
                        #endregion

                        // CREATE DATATABLE TO STORE IMPOTED FILE DATA
                        DataTable dtExcel = new DataTable();
                        dtExcel = ReadExcel(filePath, fileExt, dt);

                        // CHECK IMPORTED TEMPLATE
                        if (dtExcel.Columns.Count != 1)
                        {
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                // COMBINE IMPORTED EXCEL DATA AND GRID DATA

                                // DISABLE MAIN GRID
                                DisableControls(dgvGSTR13);

                                #region IMPORT EXCEL DATATABLE TO GRID DATATABLE
                                if (dtExcel != null && dtExcel.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dt.Columns.Count; i++)
                                    {
                                        dt = Utility.ChangeColumnDataType(dt, dt.Columns[i].ColumnName, typeof(string));
                                        dt.Columns[i].SetOrdinal(i);
                                    }

                                    for (int i = 0; i < dtExcel.Columns.Count; i++)
                                    {
                                        dtExcel = Utility.ChangeColumnDataType(dtExcel, dtExcel.Columns[i].ColumnName, typeof(string));
                                        dtExcel.Columns[i].SetOrdinal(i);
                                    }

                                    foreach (DataRow row in dtExcel.Rows)
                                    {
                                        // COPY EACH ROW OF IMPORTED DATATABLE ROW TO GRID DATATALE
                                        DataRow newRow = dt.NewRow();
                                        newRow.ItemArray = row.ItemArray;
                                        dt.Rows.Add(newRow);
                                        dt.Rows[dt.Rows.Count - 1]["colSrNo"] = dt.Rows.Count;
                                        Application.DoEvents();
                                    }
                                }
                                dt.AcceptChanges();
                                #endregion

                                #region RENAME DATATABLE COLUMN NAME AS PAR MAIN GRID
                                foreach (DataGridViewColumn col in dgvGSTR13.Columns)
                                {
                                    dt.Columns[col.Index].ColumnName = col.Name.ToString();
                                    col.DataPropertyName = col.Name;
                                }
                                #endregion

                                //ASSIGN DATATABLE TO DATAGRID
                                dgvGSTR13.DataSource = dt;

                                // ENABLE MAIN GRID
                                EnableControls(dgvGSTR13);
                            }
                            else
                            {
                                // IF THERE ARE NO RECORDS IN MAIN GRID

                                if (dtExcel != null && dtExcel.Rows.Count > 0)
                                {
                                    // IF THERE ARE DATA IN IMPORTED EXCEL FILE

                                    // DISABLE MAIN GRID
                                    DisableControls(dgvGSTR13);

                                    #region RENAME DATATABLE COLUMN NAME AS PAR MAIN GRID
                                    foreach (DataGridViewColumn col in dgvGSTR13.Columns)
                                    {
                                        dtExcel.Columns[col.Index].ColumnName = col.Name.ToString();
                                        col.DataPropertyName = col.Name;
                                    }
                                    dtExcel.AcceptChanges();

                                    // ASSIGN DATATALE TO GRID
                                    dgvGSTR13.DataSource = dtExcel;

                                    // ENABLE MAIN GRID
                                    EnableControls(dgvGSTR13);
                                    #endregion
                                }
                                else
                                {
                                    // IF THERE ARE NO RECORDS IN IMPORTED EXCEL FILE
                                    pbGSTR1.Visible = false;
                                    MessageBox.Show("There are no records found in imported excel ...!!!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }

                            // TOTAL CALCULATION
                            string[] colNo = { "colTotal", "colCancelled", "colIssued" };
                            GetTotal(colNo);

                            pbGSTR1.Visible = false;
                        }
                        else
                        {
                            pbGSTR1.Visible = false;
                            MessageBox.Show("Please import valid excel template...!!!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please choose .xls or .xlsx file only.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //CUSTOM MESSAGEBOX TO SHOW ERROR  
                    }
                }
                pbGSTR1.Visible = false;
            }
            catch (Exception ex)
            {
                EnableControls(dgvGSTR13);
                pbGSTR1.Visible = false;
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }

        public DataTable ReadExcel(string fileName, string fileExt, DataTable grdData)
        {
            string conn = string.Empty;
            bool flg;
            DataTable dtexcel = new DataTable();

            #region CONNECTION STRING
            if (fileExt.CompareTo(".xls") == 0)
                conn = @"provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //FOR BELOW EXCEL 2007  
            else
                conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 12.0 Xml;HDR=Yes;IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text'"; //FOR ABOVE EXCEL 2007  
            #endregion

            using (OleDbConnection con = new OleDbConnection(conn))
            {
                try
                {
                    try
                    {
                        // READ DATA FROM SHEET1 AND SAVE INTO DTATABLE
                        OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [documents_issued$]", con);
                        oleAdpt.Fill(dtexcel); //FILL EXCEL DATA INTO DATATABLE
                    }
                    catch
                    {
                        // CALL WHEN IMPORTED TEMPLATE SHEET NAME IS DIFFER FROM PREDEFINE TEMPLATE
                        DataTable dt = new DataTable();
                        dt.Columns.Add("colError");
                        return dt;
                    }

                    dtexcel = Utility.RemoveEmptyRowsFromDataTable(dtexcel);

                    if (dtexcel != null && dtexcel.Rows.Count > 0)
                    {
                        #region VALIDATE TEMPLATE
                        for (int i = 2; i < dgvGSTR13.Columns.Count; i++)
                        {
                            flg = false;
                            for (int j = 0; j < dtexcel.Columns.Count; j++)
                            {
                                // CHECK GRID COLUMN IS PRESENT OR NOT IN IMPORTED EXCEL
                                if (dgvGSTR13.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim() == dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim())
                                {
                                    // IF GRID COLUMN PRESENT IN EXCEL THEN ITS INDEX AS PAR GRID COLUMN INDEX
                                    flg = true;
                                    dtexcel.Columns[j].SetOrdinal(dgvGSTR13.Columns[i].Index - 2);
                                    break;
                                }
                            }
                            if (flg == false)
                            {
                                // IF GRID COLUMN NOT PRESENT IN EXCEL THEN RETURN DATATABLE WITH ERROR
                                DataTable dt = new DataTable();
                                dt.Columns.Add("colError");
                                return dt;
                            }
                            dtexcel.AcceptChanges();
                        }
                        #endregion

                        #region REMOVE UNUSED COLUMN FROM EXCEL
                        if (dtexcel.Columns.Count >= dgvGSTR13.Columns.Count - 2)
                        {
                            for (int i = dtexcel.Columns.Count; i > (dgvGSTR13.Columns.Count - 2); i--)
                            {
                                dtexcel.Columns.Remove(dtexcel.Columns[i - 1]);
                            }
                        }
                        dtexcel.AcceptChanges();
                        #endregion

                        #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME
                        foreach (DataGridViewColumn col in dgvGSTR13.Columns)
                        {
                            if (col.Index != 0 && col.Index != 1)
                                dtexcel.Columns[col.Index - 2].ColumnName = col.Name.ToString();
                        }
                        #endregion

                        // ADD CHECK BOX COLUMN TO DATATBLE AND MAKE IT FIRST TABLE COLUMN
                        dtexcel.Columns.Add(new DataColumn("colSrNo"));
                        dtexcel.Columns["colSrNo"].SetOrdinal(0);
                        dtexcel.Columns.Add(new DataColumn("colChk"));
                        dtexcel.Columns["colChk"].SetOrdinal(0);
                        dtexcel.AcceptChanges();

                        #region SET COLTAX VALUE AS TRUE/FALSE
                        for (int i = 0; i < dtexcel.Rows.Count; i++)
                        {
                            dtexcel.Rows[i]["colChk"] = "False";
                            dtexcel.Rows[i]["colSrNo"] = i + 1;

                            if (Utility.NatureOfDocument(Convert.ToString(dtexcel.Rows[i]["colNatureOfDocument"]).Trim()))
                                dtexcel.Rows[i]["colNatureOfDocument"] = Utility.StrNatureOfDocument(Convert.ToString(dtexcel.Rows[i]["colNatureOfDocument"]).Trim());
                            else
                                dtexcel.Rows[i]["colNatureOfDocument"] = "";

                            //                            if (dtexcel.Rows[i]["colNatureOfDocument"].ToString().Trim() == "Invoice for outward supply" ||
                            //dtexcel.Rows[i]["colNatureOfDocument"].ToString().Trim() == "Invoice for inward supply from unregistered person" ||
                            //dtexcel.Rows[i]["colNatureOfDocument"].ToString().Trim() == "Revised Invoice" ||
                            //dtexcel.Rows[i]["colNatureOfDocument"].ToString().Trim() == "Debit Note" ||
                            //dtexcel.Rows[i]["colNatureOfDocument"].ToString().Trim() == "Credit Note" ||
                            //dtexcel.Rows[i]["colNatureOfDocument"].ToString().Trim() == "Receipt voucher" ||
                            //dtexcel.Rows[i]["colNatureOfDocument"].ToString().Trim() == "Payment Voucher" ||
                            //dtexcel.Rows[i]["colNatureOfDocument"].ToString().Trim() == "Refund voucher" ||
                            //dtexcel.Rows[i]["colNatureOfDocument"].ToString().Trim() == "Delivery Challan for job work" ||
                            //dtexcel.Rows[i]["colNatureOfDocument"].ToString().Trim() == "Delivery Challan for supply on approval" ||
                            //dtexcel.Rows[i]["colNatureOfDocument"].ToString().Trim() == "Delivery Challan in case of liquid gas" ||
                            //dtexcel.Rows[i]["colNatureOfDocument"].ToString().Trim() == "Delivery Challan in cases other than by way of supply (excluding at S no. 9 to 11)")
                            //                            { }
                            //                            else
                            //                                dtexcel.Rows[i]["colNatureOfDocument"] = "";

                        }
                        dtexcel.AcceptChanges();
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    pbGSTR1.Visible = false;
                    MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                    StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                    errorWriter.Write(errorMessage);
                    errorWriter.Close();
                }

                // return datatable
                return dtexcel;
            }
        }

        public void ExportExcel()
        {
            try
            {
                if (dgvGSTR13.Rows.Count > 1)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID

                    pbGSTR1.Visible = true;

                    #region CREATE WORKBOOK AND ASSIGN COLUMNNAME
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook WB = excelApp.Workbooks.Add(Missing.Value);

                    Excel.Worksheet newWS = (Excel.Worksheet)excelApp.Worksheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    newWS.Name = "documents_issued";

                    // DELETE UNUSED WORKSHEETS FROM WORKBOOK
                    foreach (Excel.Worksheet ws in WB.Worksheets)
                    {
                        if (ws.Name != "documents_issued")
                            ((Excel.Worksheet)WB.Worksheets[ws.Name]).Delete();
                    }

                    // ASSIGN COLUMN HEADER AS PAR THE GRID HEADER
                    for (int i = 2; i < dgvGSTR13.Columns.Count; i++)
                    {
                        newWS.Cells[1, i - 1] = dgvGSTR13.Columns[i].HeaderText.ToString();

                        ((Excel.Range)newWS.Cells[1, i - 1]).ColumnWidth = 17;
                    }

                    // GET RANGE AND SET DIFFRENT PROPERTIES
                    Excel.Range headerRange = (Excel.Range)newWS.get_Range((Excel.Range)newWS.Cells[1, 1], (Excel.Range)newWS.Cells[1, dgvGSTR13.Columns.Count - 2]);
                    headerRange.WrapText = true;
                    headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Calibri";
                    #endregion

                    #region COPY DATA FROM DATATABLE TO ARRAY

                    // CREATE ARRAY TO HOLD THE DATA OF DATATABLE
                    object[,] arr = new object[dgvGSTR13.Rows.Count - 1, dgvGSTR13.Columns.Count];

                    // ASSIGN DATA TO ARRAY FROM DATATABLE
                    if (CommonHelper.IsLicence)
                    {
                        // FOR LICENECE ALLOWS TO EXPORT ALL RECORDS
                        for (int i = 0; i < dgvGSTR13.Rows.Count - 1; i++)
                        {
                            for (int j = 2; j < dgvGSTR13.Columns.Count; j++)
                            {
                                arr[i, j - 2] = Convert.ToString(dgvGSTR13.Rows[i].Cells[j].Value);
                            }
                        }
                    }
                    else
                    {
                        // FOR DEMO ALLOW ONLY 100 RECORDS TO EXPORT
                        for (int i = 0; i < dgvGSTR13.Rows.Count - 1; i++)
                        {
                            if (i < 100)
                            {
                                for (int j = 2; j < dgvGSTR13.Columns.Count; j++)
                                {
                                    arr[i, j - 2] = Convert.ToString(dgvGSTR13.Rows[i].Cells[j].Value);
                                }
                            }
                        }
                    }

                    //SET EXCEL RANGE TO PASTE THE DATA
                    Excel.Range top = (Excel.Range)newWS.Cells[2, 1];
                    Excel.Range bottom = (Excel.Range)newWS.Cells[dgvGSTR13.Rows.Count, dgvGSTR13.Columns.Count];
                    Excel.Range sheetRange = newWS.Range[top, bottom];
                    sheetRange.WrapText = true;
                    sheetRange.Columns.AutoFit();
                    sheetRange.Rows.AutoFit();
                    //sheetRange.NumberFormat = "@";


                    //FILL ARRAY IN EXCEL
                    sheetRange.Value2 = arr;

                    #endregion

                    pbGSTR1.Visible = false;

                    #region EXPORTING TO EXCEL

                    // SAVE DIALOG BOX TO SAVE EXCEL WORKBOOK
                    SaveFileDialog saveExcel = new SaveFileDialog();
                    saveExcel.Filter = "Execl files (*.xlsx)|*.xlsx";
                    saveExcel.Title = "Save excel File";
                    saveExcel.ShowDialog();

                    if (saveExcel.FileName != "")
                    {
                        #region CLOSE OPENED EXCEL IF SAME NAME USER SAVED FILE
                        foreach (Process proc in System.Diagnostics.Process.GetProcessesByName("EXCEL"))
                        {
                            string fName = System.IO.Path.GetFileName(saveExcel.FileName);
                            if (proc.MainWindowTitle == "Microsoft Excel - " + fName)
                                proc.Kill();
                        }
                        #endregion

                        // DELETE OLD FILE
                        //try
                        //{
                        if (File.Exists(saveExcel.FileName))
                            File.Delete(saveExcel.FileName);
                        //}
                        //catch
                        //{
                        //    MessageBox.Show("Please close opened related excel file.");
                        //    return;
                        //}

                        // SAVE EXCEL FILE AND CLOSE CREATED APPLICATION
                        newWS.SaveAs(saveExcel.FileName);
                        excelApp.Quit();
                        MessageBox.Show("Excel file saved!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    #endregion
                }
                else
                {
                    // IF THERE ARE NO RECORD IN MAIN  GRID
                    MessageBox.Show("ExportToExcel: There are no records to export...!!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                pbGSTR1.Visible = false;
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }

        #endregion

        #region CSV TRANSACTIONS

        public void ImportCSV()
        {
            try
            {
                string filePath = string.Empty, fileExt = string.Empty;

                //OPEN DIALOG TO CHOOSE FILE
                OpenFileDialog file = new OpenFileDialog();

                if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // GET FILE NAME AND EXTENTION OF SELECTED FILE
                    filePath = file.FileName;
                    fileExt = Path.GetExtension(filePath);

                    // CHCK EXTENTION OF SELECTED FILE
                    if (fileExt.CompareTo(".csv") == 0 || fileExt.CompareTo(".~csv") == 0)
                    {
                        pbGSTR1.Visible = true;

                        // CREATE DATATABLE AND SAVE GRID DATA
                        DataTable dt = new DataTable();
                        dt = (DataTable)dgvGSTR13.DataSource;

                        // CREATE DATATBLE AND GET IMPOTED CSV FILE DATA
                        DataTable dtCsv = new DataTable();
                        dtCsv = GetDataTabletFromCSVFile(filePath, dt);

                        // CHECK IMPORTED TEMPLATE
                        if (dtCsv.Columns.Count != 1)
                        {
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                // COMBINE IMPORTED CSV DATA AND GRID DATA

                                // DISABLE MAIN GRID
                                DisableControls(dgvGSTR13);

                                #region COPY IMPORTED CSV DATATABLE DATA INTO GRID DATATABLE
                                if (dtCsv != null && dtCsv.Rows.Count > 0)
                                {
                                    int tmp = 1;
                                    foreach (DataRow row in dtCsv.Rows)
                                    {
                                        // PROGRESS BAR                                        
                                        tmp++;

                                        // COPY EACH ROW OF IMPORTED DATATABLE ROW TO GRID DATATABLE
                                        DataRow newRow = dt.NewRow();
                                        newRow.ItemArray = row.ItemArray;
                                        dt.Rows.Add(newRow);
                                        dt.Rows[dt.Rows.Count - 1]["colSrNo"] = dt.Rows.Count;
                                        Application.DoEvents();
                                    }
                                }
                                dt.AcceptChanges();
                                #endregion

                                #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME
                                foreach (DataGridViewColumn col in dgvGSTR13.Columns)
                                {
                                    dtCsv.Columns[col.Index].ColumnName = col.Name.ToString();
                                    col.DataPropertyName = col.Name;
                                }
                                dt.AcceptChanges();
                                #endregion

                                // ASSIGN DATATABLE TO GRID
                                dgvGSTR13.DataSource = dt;

                                // ENABLE MAIN GRID
                                EnableControls(dgvGSTR13);
                            }
                            else
                            {
                                // IF THERE ARE NO RECORDS IN MAIN GRID

                                if (dtCsv != null && dtCsv.Rows.Count > 0)
                                {
                                    // IF THERE ARE RECORD PRESENT IN IMPORT FILE

                                    // DISABLE MAIN GRID
                                    DisableControls(dgvGSTR13);

                                    #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME AND ASSIGN TO MAIN GRID
                                    foreach (DataGridViewColumn col in dgvGSTR13.Columns)
                                    {
                                        dtCsv.Columns[col.Index].ColumnName = col.Name.ToString();
                                        col.DataPropertyName = col.Name;
                                    }
                                    dtCsv.AcceptChanges();

                                    // ASSIGN DATATABLE TO GRID
                                    dgvGSTR13.DataSource = dtCsv;

                                    // ENABLE CONTROL
                                    EnableControls(dgvGSTR13);
                                    #endregion
                                }
                                else
                                {
                                    // IF THERE ARE NO RECORDS IN IMPORT FILE                                    
                                    pbGSTR1.Visible = false;
                                    MessageBox.Show("There are no records in CSV file...!!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }

                            // TOTAL CALCULATION
                            string[] colNo = { "colTotal", "colCancelled", "colIssued" };
                            GetTotal(colNo);

                            pbGSTR1.Visible = false;
                        }
                        else
                        {

                            pbGSTR1.Visible = false;
                            MessageBox.Show("Please import valid csv template...!!!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please choose .csv or .~csv file only.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //CUSTOM MESSAGEBOX TO SHOW ERROR  
                    }
                }
            }
            catch (Exception ex)
            {
                EnableControls(dgvGSTR13);
                pbGSTR1.Visible = false;
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }

        private DataTable GetDataTabletFromCSVFile(string csv_file_path, DataTable grdData)
        {
            //CREATE DATATABLE TO STORE CSV DATA
            DataTable csvData = new DataTable();

            // READ DATA FROM IMPOTED CSV FILE
            using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
            {
                try
                {
                    // SPECIFI SEPRATER FOR CSV FILE
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();

                    #region ADD DATATABLE COLUMN
                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }
                    #endregion

                    #region VALIDATE TEMPLATE
                    for (int i = 1; i < dgvGSTR13.Columns.Count; i++)
                    {
                        Boolean flg = false;
                        for (int j = 0; j < csvData.Columns.Count; j++)
                        {
                            // CHECK GRID COLUMN IS PRESENT OR NOT IN IMPORTED EXCEL
                            if (dgvGSTR13.Columns[i].HeaderText.Replace(" ", "") == csvData.Columns[j].ColumnName.Replace(" ", "").Trim())
                            {
                                // IF GRID COLUMN PRESENT IN EXCEL THEN ITS INDEX AS PAR GRID COLUMN INDEX
                                flg = true;
                                csvData.Columns[j].SetOrdinal(dgvGSTR13.Columns[i].Index - 1);
                                break;
                            }
                        }
                        if (flg == false)
                        {
                            // IF GRID COLUMN NOT PRESENT IN EXCEL THEN RETURN DATATABLE WITH ERROR
                            DataTable dt = new DataTable();
                            dt.Columns.Add("colError");
                            return dt;
                        }
                        csvData.AcceptChanges();
                    }
                    #endregion

                    #region ADD ROW DATA
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();

                        //MAKING EMPTY VALUE AS NULL
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                                fieldData[i] = null;
                        }
                        csvData.Rows.Add(fieldData);
                    }
                    #endregion

                    #region REMOVE UNUSED COLUMN FROM CSV DATATABLE
                    if (csvData.Columns.Count >= dgvGSTR13.Columns.Count - 2)
                    {
                        for (int i = csvData.Columns.Count - 1; i > (dgvGSTR13.Columns.Count - 2); i--)
                        {
                            csvData.Columns.Remove(csvData.Columns[i]);
                        }
                    }
                    #endregion

                    #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME
                    foreach (DataGridViewColumn col in dgvGSTR13.Columns)
                    {
                        if (col.Index != 0)
                            csvData.Columns[col.Index - 1].ColumnName = col.Name.ToString();
                    }
                    #endregion

                    // ADD CHECK BOX COLUMN TO DATATABLE AND SET TO FIRST COLUMN
                    csvData.Columns.Add(new DataColumn("colChk"));
                    csvData.Columns["colChk"].SetOrdinal(0);
                    csvData.AcceptChanges();

                    #region SET COLTAX VALUE AS TRUE/FALSE
                    for (int i = 0; i < csvData.Rows.Count; i++)
                    {
                        csvData.Rows[i]["colChk"] = "False";
                        csvData.Rows[i]["colSrNo"] = i + 1;

                        if (csvData.Rows[i]["colNatureOfDocument"].ToString().Trim() == "Invoice for outward supply" ||
csvData.Rows[i]["colNatureOfDocument"].ToString().Trim() == "Invoice for inward supply from unregistered person" ||
csvData.Rows[i]["colNatureOfDocument"].ToString().Trim() == "Revised Invoice" ||
csvData.Rows[i]["colNatureOfDocument"].ToString().Trim() == "Debit Note" ||
csvData.Rows[i]["colNatureOfDocument"].ToString().Trim() == "Credit Note" ||
csvData.Rows[i]["colNatureOfDocument"].ToString().Trim() == "Receipt voucher" ||
csvData.Rows[i]["colNatureOfDocument"].ToString().Trim() == "Payment Voucher" ||
csvData.Rows[i]["colNatureOfDocument"].ToString().Trim() == "Refund voucher" ||
csvData.Rows[i]["colNatureOfDocument"].ToString().Trim() == "Delivery Challan for job work" ||
csvData.Rows[i]["colNatureOfDocument"].ToString().Trim() == "Delivery Challan for supply on approval" ||
csvData.Rows[i]["colNatureOfDocument"].ToString().Trim() == "Delivery Challan in case of liquid gas" ||
csvData.Rows[i]["colNatureOfDocument"].ToString().Trim() == "Delivery Challan in cases other than by way of supply (excluding at S no. 9 to 11)")
                        { }
                        else
                            csvData.Rows[i]["colNatureOfDocument"] = "";
                    }
                    csvData.AcceptChanges();
                    #endregion
                }
                catch (Exception ex)
                {
                    pbGSTR1.Visible = false;
                    MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                    StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                    errorWriter.Write(errorMessage);
                    errorWriter.Close();
                }
                return csvData;
            }
        }

        public void ExportCSV()
        {
            try
            {
                if (dgvGSTR13.Rows.Count > 1)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID

                    pbGSTR1.Visible = true;

                    string csv = string.Empty;
                    // CREATE DATATABLE AND GET GRID DATA
                    DataTable dt = new DataTable();
                    dt = (DataTable)dgvGSTR13.DataSource;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        #region ASSIGN COLUMN NAME TO CSV STRING
                        for (int i = 1; i < dgvGSTR13.Columns.Count; i++)
                        {
                            csv += dgvGSTR13.Columns[i].HeaderText + ',';
                        }

                        //ADD NEW LINE.
                        csv += "\r\n";
                        #endregion

                        #region ASSIGN DREID ROW TO CSV STRING
                        StringBuilder sb = new StringBuilder();
                        sb.Append(csv);

                        // SEPRATE EACH RECORD AND APPEND AS SEPRATED STRING
                        int sj = 0;
                        if (CommonHelper.IsLicence)
                        {
                            // FOR LICENCE ALLOWS TO EXPORT ALL RECORDS
                            foreach (DataRow row in dt.Rows)
                            {
                                var fields = row.ItemArray.Select(field => "\"" + field.ToString().Replace("\"", "\"\"") + "\"").Skip(1).ToArray();
                                sb.AppendLine(string.Join(",", fields));
                                sj++;
                            }
                        }
                        else
                        {
                            // FOR DEMO ALLOW ONLY 100 RECORDS TO EXPORT
                            foreach (DataRow row in dt.Rows)
                            {
                                if (sj < 100)
                                {
                                    var fields = row.ItemArray.Select(field => "\"" + field.ToString().Replace("\"", "\"\"") + "\"").Skip(1).ToArray();
                                    sb.AppendLine(string.Join(",", fields));
                                    sj++;
                                }
                            }
                        }
                        csv = sb.ToString();
                        #endregion

                        pbGSTR1.Visible = false;

                        #region EXPORTING TO CSV

                        // SAVE DIALOG BOX FOR SAVE FILE
                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Filter = "CSV files (*.csv)|*.csv";

                        if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            try
                            {
                                using (StreamWriter sw = new StreamWriter(sfd.FileName, false))
                                {
                                    // WRITE CSV STRING INTO SAVED FILE
                                    sw.WriteLine(csv.ToString());
                                    MessageBox.Show("CSV file saved.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            catch
                            {
                                MessageBox.Show("Please close opened related csv file.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        pbGSTR1.Visible = false;
                        MessageBox.Show("Please save data after export csv file!");
                    }
                }
                else
                {
                    // IF THERE ARE NO RECORD IN MAIN  GRID
                    MessageBox.Show("ExportToCSV: There are no records to export...!!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                pbGSTR1.Visible = false;
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }

        #endregion

        #region PDF TRANSACTIONS UPDATED BY MEGHA - 11/05/2017

        public void ExportPDF()
        {
            try
            {
                pbGSTR1.Visible = true;

                #region CREATING ITEXTSHARP TABLE FROM THE DATATABLE DATA AND ASSIGNING TABLE HEADER
                PdfPTable pdfTable = new PdfPTable(dgvGSTR13.ColumnCount - 1);
                pdfTable.DefaultCell.Padding = 3;
                pdfTable.WidthPercentage = 100;
                pdfTable.DefaultCell.BorderWidth = 0;
                iTextSharp.text.Font fontHeader = iTextSharp.text.FontFactory.GetFont("Calibri", 6);

                // ADD HEADER TO PDF TABLE
                pdfTable = AssignHeader(pdfTable, "13. Documents issued during the tax period");
                #endregion

                #region ADDING HEADER ROW
                int i = 0;

                #region HEADER1
                PdfPCell celHeader1 = new PdfPCell();

                celHeader1 = new PdfPCell(new Phrase("Sr. No.", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Nature of Document", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Sr. No.", fontHeader));
                celHeader1.Colspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Total Number", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Cancelled", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Net Issued", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                pdfTable.CompleteRow();
                #endregion

                #region HEADER2
                PdfPCell celHeader2 = new PdfPCell();

                celHeader2 = new PdfPCell(new Phrase("From", fontHeader));
                celHeader2 = SetAllignMent(celHeader2, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(255, 255, 204));
                pdfTable.AddCell(celHeader2);

                celHeader2 = new PdfPCell(new Phrase("To", fontHeader));
                celHeader2 = SetAllignMent(celHeader2, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(255, 255, 204));
                pdfTable.AddCell(celHeader2);

                pdfTable.CompleteRow();
                #endregion

                Application.DoEvents();
                #endregion

                #region ADDING COLUMN NUMBER
                i = 0;
                foreach (DataGridViewColumn column in dgvGSTR13.Columns)
                {
                    if (i != 0)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase("(" + (i).ToString() + ")", fontHeader));
                        cell.VerticalAlignment = Element.ALIGN_CENTER;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        pdfTable.AddCell(cell);
                    }
                    i++;
                }
                pdfTable.CompleteRow();
                Application.DoEvents();

                #endregion

                #region ADDING DATAROW TO PDF TABLE

                int sj = 0;
                if (CommonHelper.IsLicence)
                {
                    // FOR LICENCE ALLOWS TO EXPORT ALL RECORDS
                    foreach (DataGridViewRow row in dgvGSTR13.Rows)
                    {
                        i = 0;

                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            if (cell.Value != null && i != 0)
                            {
                                //CREATE PDF CELL TO GRID RECORDS
                                PdfPCell cell1 = new PdfPCell(new Phrase(cell.Value.ToString(), fontHeader));
                                cell1.VerticalAlignment = Element.ALIGN_CENTER;
                                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                                pdfTable.AddCell(cell1);
                            }
                            i++;
                        }
                        sj++;

                        // COMPLETE PDF-TABLE ROW
                        pdfTable.CompleteRow();
                    }
                }
                else
                {
                    // FOR DEMO ALLOW ONLY 100 RECORDS TO EXPORT
                    foreach (DataGridViewRow row in dgvGSTR13.Rows)
                    {
                        if (sj < 100)
                        {
                            i = 0;
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                if (cell.Value != null && i != 0)
                                {
                                    //CREATE PDF CELL TO GRID RECORDS
                                    PdfPCell cell1 = new PdfPCell(new Phrase(cell.Value.ToString(), fontHeader));
                                    cell1.VerticalAlignment = Element.ALIGN_CENTER;
                                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                                    pdfTable.AddCell(cell1);
                                }
                                i++;
                            }
                            sj++;

                            // COMPLETE PDF-TABLE ROW
                            pdfTable.CompleteRow();
                        }
                    }
                }
                Application.DoEvents();
                #endregion

                pbGSTR1.Visible = false;

                #region EXPORTING TO PDF

                // SAVE DIALOG BOX FOR SAVE PDF FILE
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "PDF document (*.pdf)|*.pdf";
                saveFileDialog1.Title = "Save pdf File";
                saveFileDialog1.ShowDialog();

                // IF THE FILE NAME IS NOT AN EMPTY STRING OPEN IT FOR SAVING.
                if (saveFileDialog1.FileName != "")
                {
                    try
                    {
                        // WRITE PDF TABLE INTO SAVED FILE
                        FileStream stream = (FileStream)saveFileDialog1.OpenFile();
                        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                        PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();
                        pdfDoc.Add(pdfTable);
                        pdfDoc.Close();
                        stream.Close();
                        Application.DoEvents();
                        MessageBox.Show("PDF file saved.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch
                    {
                        MessageBox.Show("Please close opened related pdf file.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                pbGSTR1.Visible = false;
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }

        public PdfPTable AssignHeader(PdfPTable pdfTable, string HeaderName)
        {
            try
            {
                // ADD HEADER TO PDF TABLE
                iTextSharp.text.Font fontHeader = iTextSharp.text.FontFactory.GetFont("Calibri", 10);
                PdfPCell ce1 = new PdfPCell(new Phrase(HeaderName, fontHeader));
                ce1.Colspan = dgvGSTR13.Columns.Count - 1;
                ce1.VerticalAlignment = Element.ALIGN_CENTER;
                ce1.HorizontalAlignment = Element.ALIGN_LEFT;
                ce1.BackgroundColor = new iTextSharp.text.BaseColor(197, 223, 197);
                ce1.BorderWidth = 0;
                pdfTable.AddCell(ce1);

                iTextSharp.text.Font FigToRs = FontFactory.GetFont("Calibri", 10, iTextSharp.text.Font.BOLD);
                PdfPCell ceHeader2 = new PdfPCell(new Phrase("(figures in Rs)", FigToRs));
                ceHeader2.Colspan = dgvGSTR13.Columns.Count - 1;
                ceHeader2.VerticalAlignment = Element.ALIGN_CENTER;
                ceHeader2.HorizontalAlignment = Element.ALIGN_RIGHT;
                ceHeader2.BorderWidth = 0;
                pdfTable.AddCell(ceHeader2);

                PdfPCell ce2 = new PdfPCell(new Phrase(" "));
                ce2.Colspan = dgvGSTR13.Columns.Count - 1;
                ce2.BorderWidth = 0;
                pdfTable.AddCell(ce2);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }

            return pdfTable;
        }

        public PdfPCell SetAllignMent(PdfPCell pdfPCell, int VertCalAlignMent, int HoriZontalAlignMent, BaseColor BackColor)
        {
            try
            {
                // ADD ALLIGENMENT AND BACKGROUND COLOR TO PDF CELL

                pdfPCell.VerticalAlignment = VertCalAlignMent;
                pdfPCell.HorizontalAlignment = HoriZontalAlignMent;
                pdfPCell.BackgroundColor = BackColor;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }

            return pdfPCell;
        }

        #endregion

        #region JSON TRANSACTION

        #region JSON CLASS
        public class Doc
        {
            public int num { get; set; }
            public string from { get; set; }
            public string to { get; set; }
            public int totnum { get; set; }
            public int cancel { get; set; }
            public int net_issue { get; set; }
        }

        public class DocDet
        {
            public int doc_num { get; set; }
            public List<Doc> docs { get; set; }
        }

        public class DocIssue
        {
            public List<DocDet> doc_det { get; set; }
        }

        public class RootObject
        {
            public string gstin { get; set; }
            public string fp { get; set; }
            public double gt { get; set; }
            public double cur_gt { get; set; }
            public DocIssue doc_issue { get; set; }
        }
        #endregion

        public void JSONCreator()
        {
            try
            {
                RootObject ObjJson = new RootObject();

                //frmSelectTurnOver objYear = new frmSelectTurnOver();
                //var result = objYear.ShowDialog();
                //if (result != DialogResult.OK)
                //{
                //    MessageBox.Show("Plese select current year gross turnover..", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);                    
                //}

                //if (CommonHelper.CurrentTurnOver != null && Convert.ToString(CommonHelper.CurrentTurnOver).Trim() != "")
                //{
                    pbGSTR1.Visible = true;
                    DataTable dt = new DataTable();

                    #region Bind Grid Data

                    #region ADD DATATABLE COLUMN

                    foreach (DataGridViewColumn col in dgvGSTR13.Columns)
                    {
                        dt.Columns.Add(col.Name.ToString());
                        col.DataPropertyName = col.Name;
                    }
                    #endregion

                    #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                    object[] rowValue = new object[dt.Columns.Count];

                    foreach (DataGridViewRow dr in dgvGSTR13.Rows)
                    {
                        if (dr.Index != dgvGSTR13.Rows.Count - 1)
                        {
                            rowValue[0] = "False";
                            for (int i = 1; i < dr.Cells.Count; i++)
                            {
                                rowValue[i] = Convert.ToString(dr.Cells[i].Value);
                            }

                            dt.Rows.Add(rowValue);
                        }
                    }
                    dt.Columns.Remove("colChk");
                    dt.Columns.Remove("colSrNo");
                    dt.AcceptChanges();
                    #endregion

                    #endregion

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        List<string> lstRow = dt.Rows
                        .OfType<DataRow>()
                        .Select(x => Convert.ToString(x["colNatureOfDocument"]).Trim())
                        .Distinct()
                        .ToList();

                        if (lstRow != null && lstRow.Count > 0)
                        {
                            ObjJson.gstin = CommonHelper.CompanyGSTN; // tax person GSTIN
                            ObjJson.fp = CommonHelper.GetReturnPeriod(); // current return period
                            ObjJson.gt = CommonHelper.TurnOver; // previous financial year turnover
                            ObjJson.cur_gt = Convert.ToDouble(CommonHelper.CurrentTurnOver); // current Finacial year turnover   

                            DocIssue objDocIssue = new DocIssue();
                            List<DocDet> lstObjDocDet = new List<DocDet>();

                            for (int j = 0; j < lstRow.Count; j++)
                            {
                                if (Convert.ToString(lstRow[j]).Trim() != "")
                                {
                                    DocDet objDocDet = new DocDet();
                                    objDocDet.doc_num = GetDocNo(Convert.ToString(lstRow[j]));

                                    List<DataRow> lstDt = dt.Rows
                                       .OfType<DataRow>()
                                       .Where(x => Convert.ToString(lstRow[j]).Trim() == Convert.ToString(x["colNatureOfDocument"]).Trim())
                                       .Select(x => x)
                                       .ToList();

                                    if (lstDt != null && lstDt.Count > 0)
                                    {
                                        List<Doc> lstObjDoc = new List<Doc>();

                                        for (int i = 0; i < lstDt.Count; i++)
                                        {
                                            Doc clsDoc = new Doc();
                                            clsDoc.num = i + 1;

                                            if (!string.IsNullOrEmpty(Convert.ToString(lstDt[i]["colFrom"]).Trim()))
                                                clsDoc.from = Convert.ToString(lstDt[i]["colFrom"]).Trim(); // from

                                            if (!string.IsNullOrEmpty(Convert.ToString(lstDt[i]["colTo"]).Trim()))
                                                clsDoc.to = Convert.ToString(lstDt[i]["colTo"]).Trim(); // to

                                            if (!string.IsNullOrEmpty(Convert.ToString(lstDt[i]["colTotal"]).Trim()))
                                                clsDoc.totnum = Convert.ToInt32(Convert.ToString(lstDt[i]["colTotal"]).Trim()); // total no

                                            if (!string.IsNullOrEmpty(Convert.ToString(lstDt[i]["colCancelled"]).Trim()))
                                                clsDoc.cancel = Convert.ToInt32(Convert.ToString(lstDt[i]["colCancelled"]).Trim()); // cancel

                                            if (!string.IsNullOrEmpty(Convert.ToString(lstDt[i]["colIssued"]).Trim()))
                                                clsDoc.net_issue = Convert.ToInt32(Convert.ToString(lstDt[i]["colIssued"]).Trim()); // net issue

                                            lstObjDoc.Add(clsDoc);
                                            objDocDet.docs = lstObjDoc;

                                            if (i == lstDt.Count - 1)
                                                lstObjDocDet.Add(objDocDet);

                                            objDocIssue.doc_det = lstObjDocDet;
                                            ObjJson.doc_issue = objDocIssue;
                                        }
                                    }
                                //}
                            }
                        }
                    }

                    #region File Save
                    JavaScriptSerializer objScript = new JavaScriptSerializer();

                    var settings = new JsonSerializerSettings();
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    settings.DefaultValueHandling = DefaultValueHandling.Ignore;

                    objScript.MaxJsonLength = 2147483647;

                    string FinalJson = JsonConvert.SerializeObject(ObjJson, settings);

                    SaveFileDialog save = new SaveFileDialog();
                    save.FileName = "DOC.json";
                    save.Filter = "Json File | *.json";
                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        StreamWriter writer = new StreamWriter(save.OpenFile());
                        writer.WriteLine(FinalJson);
                        writer.Dispose();
                        writer.Close();
                        MessageBox.Show("JSON file saved!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    #endregion

                    pbGSTR1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                pbGSTR1.Visible = false;
                MessageBox.Show("Error : " + ex.Message);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }

        public int GetDocNo(string docNature)
        {
            int docNo = 0;
            docNature = docNature.Trim().ToLower();

            try
            {
                if (docNature == "invoice for outward supply")
                    docNo = 1;
                else if (docNature == "invoice for inward supply from unregistered person")
                    docNo = 2;
                else if (docNature == "revised invoice")
                    docNo = 3;
                else if (docNature == "debit note")
                    docNo = 4;
                else if (docNature == "credit note")
                    docNo = 5;
                else if (docNature == "receipt voucher")
                    docNo = 6;
                else if (docNature == "payment voucher")
                    docNo = 7;
                else if (docNature == "refund voucher")
                    docNo = 8;
                else if (docNature == "delivery challan for job work")
                    docNo = 9;
                else if (docNature == "delivery challan for supply on approval")
                    docNo = 10;
                else if (docNature == "delivery challan in case of liquid gas")
                    docNo = 11;
                else if (docNature == "delivery challan in cases other than by way of supply (excluding at s no. 9 to 11)")
                    docNo = 12;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }

            return docNo;
        }

        #endregion

        public void SetGridViewColor()
        {
            try
            {
                // DO NOT ALLOW TO AUTO GENERATE COLUMNS
                dgvGSTR13.AutoGenerateColumns = false;

                // SET HEIGHT WIDTH OF FORM
                this.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.76));
                this.Height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.66));

                // SET WIDTH OF HEADER, MAIN AND TOTAL GRID
                this.panel1.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.745));
                this.dgvGSTR13.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.745));
                this.TotaldgvGSTR13.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.745));

                // SET HEIGHT OF MAIN GRID
                this.dgvGSTR13.Height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.54));
                this.TotaldgvGSTR13.Height = 29;

                // SET LOCATION OF HEADER,LOADING PIC, CHECKBOX AND MAIN AND TOTAL GRID
                //this.panel1.Location = new System.Drawing.Point(12, 5);
                //this.dgvGSTR13.Location = new System.Drawing.Point(12, Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.055)));
                //this.TotaldgvGSTR13.Location = new System.Drawing.Point(12, Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.61)));
                //this.ckboxHeader.Location = new System.Drawing.Point(29, Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.128)));
                //this.pbGSTR1.Location = new System.Drawing.Point(Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.45)), Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.30)));

                // SET MAIN GRID PROPERTY
                dgvGSTR13.EnableHeadersVisualStyles = false;
                dgvGSTR13.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(23, 196, 187);
                dgvGSTR13.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

                this.dgvGSTR13.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                this.dgvGSTR13.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                this.dgvGSTR13.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

                foreach (DataGridViewColumn column in dgvGSTR13.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }

        public bool chkNatureOfDoc(string val)
        {
            bool flg = false;
            val = val.ToString().Trim();
            try
            {
                if (val == "Invoice for outward supply" || val == "Invoice for inward supply from unregistered person" || val == "Revised Invoice" || val == "Debit Note" || val == "Credit Note" || val == "Receipt voucher" || val == "Payment Voucher" || val == "Refund voucher" || val == "Delivery Challan for job work" || val == "Delivery Challan for supply on approval" || val == "Delivery Challan in case of liquid gas" || val == "Delivery Challan in cases other than by way of supply (excluding at S no. 9 to 11)")
                    return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }

            return flg;
        }

        private void dgvGSTR13_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                // SET INDEX OF USER ADDED ROW IN MAIN GRID
                dgvGSTR13.Rows[e.Row.Index - 1].Cells["colSrNo"].Value = e.Row.Index;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }

        #region disable/enable controls

        private void DisableControls(Control con)
        {
            foreach (Control c in con.Controls)
            {
                if (c.Name != "dgvGSTR13" && c.Name != "TotaldgvGSTR13")
                    DisableControls(c);
            }
            con.Enabled = false;
            //EnableControls(pbGSTR1);
        }

        private void EnableControls(Control con)
        {
            if (con != null)
            {
                con.Enabled = true;
                EnableControls(con.Parent);
            }
        }

        #endregion

        #region Check All / Uncheck All
        private void ckboxHeader_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                // IF THERE ARE RECORDS IN MAIN GRID
                if (dgvGSTR13.Rows.Count > 1)
                {
                    pbGSTR1.Visible = true;

                    if (ckboxHeader.Checked)
                    {
                        // IF CHECK BOX IS CHECKED

                        // SET CHECK BOX COLUMN VALUE AS TRUE
                        for (int i = 0; i < dgvGSTR13.Rows.Count - 1; i++)
                        {
                            dgvGSTR13.Rows[i].Cells[0].Value = "True";
                            Application.DoEvents();
                        }

                        // CHANGE HEADER TEXT AND WIDTH OF COLUMN AND POSITION OF CHECK BOX OF CHECK ALL COLUMN
                        dgvGSTR13.Columns[0].HeaderText = "Uncheck All";
                    }
                    else if (ckboxHeader.Checked == false)
                    {
                        // IF CHECK BOX IS UNCHECKED

                        // SET CHECK BOX COLUMN VALUE AS FALSE
                        for (int i = 0; i < dgvGSTR13.Rows.Count - 1; i++)
                        {
                            dgvGSTR13.Rows[i].Cells[0].Value = "False";
                            Application.DoEvents();
                        }

                        // CHANGE HEADER TEXT AND WIDTH OF COLUMN AND POSITION OF CHECK BOX OF CHECK ALL COLUMN
                        dgvGSTR13.Columns[0].HeaderText = "Check All";
                    }

                    pbGSTR1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                pbGSTR1.Visible = false;
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvGSTR13_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                // CHECK FIRST COLUMN HEADER PRESSED AND MAIN GRID HAVING RECORDS
                if (e.ColumnIndex == 0 && dgvGSTR13.Rows.Count > 1)
                {
                    // CHECK AND UNCHECK CHECK BOX OF HEADER FOR SELECTING AND UNSELECTING ALL RECORDS
                    if (dgvGSTR13.Columns[e.ColumnIndex].HeaderText == "Check All")
                        ckboxHeader.Checked = true;
                    else if (dgvGSTR13.Columns[e.ColumnIndex].HeaderText == "Uncheck All")
                        ckboxHeader.Checked = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Scroll

        private void dgvGSTR13_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                // set total grid offset as par main grid scrol
                this.TotaldgvGSTR13.HorizontalScrollingOffset = this.dgvGSTR13.HorizontalScrollingOffset;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TotaldgvGSTR13_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                // set main grid offset as par total grid scrol
                this.dgvGSTR13.HorizontalScrollingOffset = this.TotaldgvGSTR13.HorizontalScrollingOffset;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void TotaldgvGSTR13_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.TotaldgvGSTR13.ClearSelection();

            if (TotaldgvGSTR13.Rows.Count > 0)
            {
                DataGridViewRow row = this.TotaldgvGSTR13.RowTemplate;
                row.MinimumHeight = 30;
            }
        }

        private void frmGSTR113_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName("");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {


            // (new SPQMDI()).Save_Close();
            ((SPQMDI)Application.OpenForms["SPQMDI"]).Save_Close();

            //if (Convert.ToString(CommonHelper.IsMainFormType) != "1Account")
            //{
            //    DialogResult dialogResult = MessageBox.Show("Do you want to save it?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //    if (dialogResult == DialogResult.Yes)
            //    {
            //        (new SPQMDI()).SaveAndClose();
            //        //SaveAndClose();
            //       // Save();
            //    }
            //}

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            ImportExcel();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportExcel();
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            //Validate();
            IsValidateData();
        }

        private void btnVerifyGSTIN_Click(object sender, EventArgs e)
        {
            //ValidataAndGetGSTIN();
        }
    }
}
