using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.VisualBasic.FileIO;
using SPEQTAGST.BAL;
using SPEQTAGST.BAL.R113r4a;
//using SPEQTAGST.closexcs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace SPEQTAGST.rintlclass4a
{
    public partial class SPQGSTR4A5 : Form
    {
        R4aPublicclass objGSTR4A = new R4aPublicclass();

        public SPQGSTR4A5()
        {
            InitializeComponent();
            SetGridViewColor();

            GetData();

            int[] colNo = { 3, 5, 8, 10, 12, 14 };
            GetTotal(colNo);

            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName(CommonHelper.ReturnName);

            pbGSTR1.Visible = false;
        }

        private void GetData()
        {
            try
            {
                DataTable dt = new DataTable();
                string Query = "Select * from GSTR4AFormPartA5 where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
                Application.DoEvents();
                dt = objGSTR4A.GetDataGSTR4A(Query);

                if (dt != null && dt.Rows.Count > 0)
                {
                    // ASSIGN FILE STATUS FILED VALUE
                    if (Convert.ToString(dt.Rows[0]["Fld_FileStatus"]).ToLower() == "draft")
                        ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(1);
                    else if (Convert.ToString(dt.Rows[0]["Fld_FileStatus"]).ToLower() == "completed")
                        ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(2);
                    else if (Convert.ToString(dt.Rows[0]["Fld_FileStatus"]).ToLower() == "not-completed")
                        ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(3);

                    // REMOVE LAST COLUMN (MONTH)
                    dt.Columns.Remove(dt.Columns[dt.Columns.Count - 1]);
                    // REMOVE LAST COLUMN (FILE STATUS)
                    dt.Columns.Remove(dt.Columns[dt.Columns.Count - 1]);
                    // REMOVE FIRST COLUMN (FIELD ID)
                    dt.Columns.Remove(dt.Columns[0]);
                    // ADD COLUMN (CHEK BOX)
                    dt.Columns.Add(new DataColumn("colChk"));
                    // SET CHECK BOX COLUMN AT FIRST INDEX OF DATATABLE
                    dt.Columns["colChk"].SetOrdinal(0);

                    // RENAME DATATABLE COLUMN NAME TO DATAGRIDVIEW COLUMN NAME
                    foreach (DataGridViewColumn col in dgvGSTR4A5.Columns)
                    {
                        dt.Columns[col.Index].ColumnName = col.Name.ToString();
                        col.DataPropertyName = col.Name;
                    }
                    dt.AcceptChanges();

                    // ASSIGN DATATABLE TO DATA GRID VIEW
                    dgvGSTR4A5.DataSource = dt;
                    Application.DoEvents();
                }
                else
                {
                    ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }

        public void GetTotal(int[] colNo)
        {
            try
            {
                if (dgvGSTR4A5.Rows.Count > 1)
                {
                    // IF MAIN GRID HAVING RECORDS

                    if (dgvGSTR4A5Total.Rows.Count == 0)
                    {
                        #region IF TOTAL GRID HAVING NO RECORD
                        // CREATE TEMPORARY DATATABLE TO STORE COLUMN CALCULATION
                        DataTable dtTotal = new DataTable();

                        // ADD COLUMN AS PAR DATAGRIDVIEW COLUMN
                        foreach (DataGridViewColumn col in dgvGSTR4A5Total.Columns)
                        {
                            dtTotal.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;
                        }

                        // CREATE DATAROW TO STORE GRID COLUMN CALCULATION
                        DataRow dr = dtTotal.NewRow();
                        dr["colTInvoiceNo"] = dgvGSTR4A5.Rows.Cast<DataGridViewRow>().Where(x => Convert.ToString(x.Cells[3].Value).Trim() != "").GroupBy(x => x.Cells[3].Value).Select(x => x.First()).Distinct().Count();
                        dr["colTInvoiceValue"] = dgvGSTR4A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[5].Value != null).Sum(x => x.Cells[5].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[5].Value)).ToString();
                        dr["colTInvoiceTaxableVal"] = dgvGSTR4A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[8].Value != null).Sum(x => x.Cells[8].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[8].Value)).ToString();
                        dr["colTIGSTAmnt"] = dgvGSTR4A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[10].Value != null).Sum(x => x.Cells[10].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[10].Value)).ToString();
                        dr["colTCGSTAmnt"] = dgvGSTR4A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[12].Value != null).Sum(x => x.Cells[12].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[12].Value)).ToString();
                        dr["colTSGSTAmnt"] = dgvGSTR4A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[14].Value != null).Sum(x => x.Cells[14].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[14].Value)).ToString();

                        // ADD DATAROW TO DATATABLE
                        dtTotal.Rows.Add(dr);
                        dtTotal.AcceptChanges();

                        // ASSIGN DATATABLE TO GRID
                        dgvGSTR4A5Total.DataSource = dtTotal;

                        #endregion
                    }
                    else if (dgvGSTR4A5Total.Rows.Count == 1)
                    {
                        #region IF TOTAL GRID HAVING ONLY ONE RECORDS

                        // CALCULATE TOTAL ONLY SPECIFIC COLUMN
                        foreach (var item in colNo)
                        {
                            if (item == 3)
                                dgvGSTR4A5Total.Rows[0].Cells[3].Value = dgvGSTR4A5.Rows.Cast<DataGridViewRow>().Where(x => Convert.ToString(x.Cells[3].Value).Trim() != "").GroupBy(x => x.Cells[3].Value).Select(x => x.First()).Distinct().Count();
                            else if (item == 5)
                                dgvGSTR4A5Total.Rows[0].Cells[5].Value = dgvGSTR4A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[5].Value != null).Sum(x => x.Cells[5].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[5].Value)).ToString();
                            else if (item == 8)
                                dgvGSTR4A5Total.Rows[0].Cells[8].Value = dgvGSTR4A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[8].Value != null).Sum(x => x.Cells[8].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[8].Value)).ToString();
                            else if (item == 10)
                                dgvGSTR4A5Total.Rows[0].Cells[10].Value = dgvGSTR4A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[10].Value != null).Sum(x => x.Cells[10].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[10].Value)).ToString();
                            else if (item == 12)
                                dgvGSTR4A5Total.Rows[0].Cells[12].Value = dgvGSTR4A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[12].Value != null).Sum(x => x.Cells[12].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[12].Value)).ToString();
                            else if (item == 14)
                                dgvGSTR4A5Total.Rows[0].Cells[14].Value = dgvGSTR4A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[14].Value != null).Sum(x => x.Cells[14].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[14].Value)).ToString();
                        }
                        #endregion
                    }

                    // SET GRID ROW HEIGHT AND ASSIGN TOTAL HEADER
                    dgvGSTR4A5Total.Rows[0].Cells[0].Value = "TOTAL";
                }
                else
                {
                    // CHECK IF TOTAL GRID HAVING RECORD

                    if (dgvGSTR4A5Total.Rows.Count >= 0)
                    {
                        #region IF THERE ARE NO RECORDS IN MAIN GRID THEN ASSIGN BLANK DATATABLE TO TOTAL GRID
                        DataTable dtTotal = new DataTable();
                        foreach (DataGridViewColumn col in dgvGSTR4A5Total.Columns)
                        {
                            dtTotal.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;
                        }
                        dgvGSTR4A5Total.DataSource = dtTotal;
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }

        private void dgvGSTR4A5_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete)
                {
                    #region DELETE SELECTED CELLS
                    try
                    {
                        if (dgvGSTR4A5.Rows.Count > 0)
                        {
                            foreach (DataGridViewCell oneCell in dgvGSTR4A5.SelectedCells)
                            {
                                if (oneCell.Selected && oneCell.ColumnIndex != 0)
                                {
                                    oneCell.ValueType.Name.ToString();
                                    oneCell.ValueType.FullName.ToString();
                                    oneCell.Value = "";// string.Empty;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                        StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                        errorWriter.Write(errorMessage);
                        errorWriter.Close();
                        return;
                    }
                    #endregion

                    int[] colNo = { 3, 5, 8, 10, 12, 14 };
                    GetTotal(colNo);
                }
                if (e.KeyCode == Keys.V)
                {
                    #region PAST FROM EXCELL SHEET

                    string s = Clipboard.GetText();
                    string[] lines = s.Split('\n');
                    int iRow = 0, iCol = 0;

                    #region PAST ON SELECTED CELLS ONLY
                    if (dgvGSTR4A5.RowCount > 0) // IF GRID IS NOT NULL AND PAST ONLY ON SELECTED CELLS
                    {
                        foreach (DataGridViewCell oneCell in dgvGSTR4A5.SelectedCells)
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
                        ProgressBar.Visible = true;
                        if (tmp == 0)
                            load(true, lines.Count(), tmp.ToString());
                        else
                            load(false, lines.Count(), tmp.ToString());
                        if (line != "")
                        {
                            // disable main grid
                            gRowNo = dgvGSTR4A5.Rows.Count - 1;
                            int no = tmp;

                            if (iRow > gRowNo - 1)
                            {
                                DataTable dtDGV = new DataTable();

                                #region ADD DATATABLE COLUMN

                                // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                                foreach (DataGridViewColumn col in dgvGSTR4A5.Columns)
                                {
                                    dtDGV.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }
                                #endregion

                                #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                                // CREATE OBJECT ARRAY TO STORE ONE ROW DATA OF MAIN GRID
                                object[] rowValue = new object[dtDGV.Columns.Count];

                                foreach (DataGridViewRow dr in dgvGSTR4A5.Rows)
                                {
                                    if (dr.Index != dgvGSTR4A5.Rows.Count - 1) // DON'T ADD LAST ROW
                                    {
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
                                // PASTE DATA TO EXISTING ROW IN GRID
                                if (line.Length > 0)
                                {
                                    #region ROW PASTE
                                    // SPLIT ONE COPIED ROW DATA TO ARRAY
                                    string[] sCells = line.Split('\t');

                                    for (int i = 0; i < sCells.GetLength(0); ++i)
                                    {
                                        // CHECK GRID COLUMN COUNT
                                        if (iCol + i < this.dgvGSTR4A5.ColumnCount && i < 13)
                                        {
                                            // SKIP CHECK BOX COLUMN AND SEQUANCE COLUMN TO PASTE DATA
                                            if (iCol == 0)
                                                oCell = dgvGSTR4A5[iCol + i + 2, iRow];
                                            else if (iCol == 1)
                                                oCell = dgvGSTR4A5[iCol + i + 1, iRow];
                                            else
                                                oCell = dgvGSTR4A5[iCol + i, iRow];

                                            sCells[i] = sCells[i].Trim().Replace(",", "");
                                            if (oCell.ColumnIndex != 0)
                                            {
                                                if (dgvGSTR4A5.Columns[oCell.ColumnIndex].Name != "colChk" && dgvGSTR4A5.Columns[oCell.ColumnIndex].Name != "colSequence")
                                                {
                                                    #region VALIDATION
                                                    if (sCells[i].ToString().Trim() == "") { dgvGSTR4A5.Rows[iRow].Cells[oCell.ColumnIndex].Value = DBNull.Value; }
                                                    else
                                                    {
                                                        if (oCell.ColumnIndex >= 2 && oCell.ColumnIndex <= 14)
                                                        {
                                                            if (chkCellValue(sCells[i].Trim(), oCell.ColumnIndex))
                                                                dgvGSTR4A5.Rows[iRow].Cells[oCell.ColumnIndex].Value = sCells[i].Trim();
                                                            else
                                                                dgvGSTR4A5.Rows[iRow].Cells[oCell.ColumnIndex].Value = DBNull.Value;
                                                        }
                                                        else { dgvGSTR4A5.Rows[iRow].Cells[oCell.ColumnIndex].Value = sCells[i].Trim(); }
                                                    }
                                                    #endregion
                                                }
                                            }
                                            else
                                            {
                                                if (iCol > i)
                                                {
                                                    for (int j = oCell.ColumnIndex; j < dgvGSTR4A5.Columns.Count; j++)
                                                    {
                                                        #region VALIDATION
                                                        if (sCells[i].ToString().Trim() == "") { dgvGSTR4A5.Rows[iRow].Cells[j].Value = DBNull.Value; }
                                                        else
                                                        {
                                                            if (j >= 2 && j <= 14)
                                                            {
                                                                if (chkCellValue(sCells[i].Trim(), j))
                                                                    dgvGSTR4A5.Rows[iRow].Cells[j].Value = sCells[i].Trim();
                                                                else
                                                                    dgvGSTR4A5.Rows[iRow].Cells[j].Value = DBNull.Value;
                                                            }
                                                            else { dgvGSTR4A5.Rows[iRow].Cells[j].Value = sCells[i].Trim(); }
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
                                                    for (int j = oCell.ColumnIndex; j < dgvGSTR4A5.Columns.Count; j++)
                                                    {
                                                        #region VALIDATION
                                                        if (sCells[i].ToString().Trim() == "") { dgvGSTR4A5.Rows[iRow].Cells[j].Value = DBNull.Value; }
                                                        else
                                                        {
                                                            if (j >= 2 && j <= 14)
                                                            {
                                                                if (chkCellValue(sCells[i].Trim(), j))
                                                                    dgvGSTR4A5.Rows[iRow].Cells[j].Value = sCells[i].Trim();
                                                                else
                                                                    dgvGSTR4A5.Rows[iRow].Cells[j].Value = DBNull.Value;
                                                            }
                                                            else { dgvGSTR4A5.Rows[iRow].Cells[j].Value = sCells[i].Trim(); }
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

                                    Application.DoEvents();
                                }
                            }
                        }
                        tmp++;
                    }

                    for (int i = 0; i < dgvGSTR4A5.Rows.Count - 1; i++)
                    {
                        dgvGSTR4A5.Rows[i].Cells["colSequence"].Value = i + 1;
                    }
                    #endregion

                    EnableControls(dgvGSTR4A5);
                }

                if ((e.Control && (e.KeyCode == Keys.A || e.KeyCode == Keys.Subtract)) || (e.KeyCode == Keys.Space && Control.ModifierKeys == Keys.Shift) || (e.Alt && e.KeyCode == Keys.F4))
                {
                    e.Handled = true;
                }

                load(false, 0, Convert.ToString(""));
                ProgressBar.Visible = false;
            }
            catch (Exception ex)
            {
                EnableControls(dgvGSTR4A5);
                load(false, 0, Convert.ToString(""));
                ProgressBar.Visible = false;
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
                #region SET DATATABLE
                int cnt = 0, colNo = 0;

                // ASSIGN GRID DATA TO DATATABLE
                DataTable dt = dtDGV;

                if (dt == null)
                {
                    // IF NO RECORD IN GRID THEN CREATE NEW DATATABLE
                    dt = new DataTable();

                    // ADD COLUMN AS PAR MAIN GRID AND SET DATA ACCESS PROPERTY
                    foreach (DataGridViewColumn col in dgvGSTR4A5.Columns)
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
                            // ADD DATA ROW TO DATATABLE
                            DataRow dtRow = dt.NewRow();
                            dt.Rows.Add(dtRow);

                            #region ROW PASTE
                            string[] sCells = line.Split('\t');

                            for (int i = 0; i < sCells.GetLength(0); ++i)
                            {
                                // CHECK GRID COLUMN COUNT
                                if (iCol + i < this.dgvGSTR4A5.ColumnCount && colNo < 14)
                                {
                                    // SKIP CHECK BOX COLUMN AND SEQUANCE COLUMN TO PASTE DATA
                                    if (iCol == 0)
                                        colNo = iCol + i + 2;
                                    else if (iCol == 1)
                                        colNo = iCol + i + 1;
                                    else
                                        colNo = iCol + i;

                                    sCells[i] = sCells[i].Trim().Replace(",", "");
                                    if (colNo != 0)
                                    {
                                        if (dt.Columns[colNo].ColumnName != "colChk")
                                        {
                                            #region VALIDATION
                                            if (sCells[i].ToString().Trim() == "") { dt.Rows[dt.Rows.Count - 1][colNo] = DBNull.Value; }
                                            else
                                            {
                                                if (colNo >= 2 && colNo <= 14)
                                                {
                                                    if (chkCellValue(sCells[i].Trim(), colNo))
                                                        dt.Rows[dt.Rows.Count - 1][colNo] = sCells[i].Trim();
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
                                        #region REST PART
                                        if (iCol > i)
                                        {
                                            for (int j = colNo; j < dgvGSTR4A5.Columns.Count; j++)
                                            {
                                                #region VALIDATION
                                                if (sCells[i].ToString().Trim() == "") { dt.Rows[dt.Rows.Count - 1][j] = DBNull.Value; }
                                                else
                                                {
                                                    if (j >= 2 && j <= 14)
                                                    {
                                                        if (chkCellValue(sCells[i].Trim(), j))
                                                            dt.Rows[dt.Rows.Count - 1][j] = sCells[i].Trim();
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
                                            for (int j = colNo; j < dgvGSTR4A5.Columns.Count; j++)
                                            {
                                                #region VALIDATION
                                                if (sCells[i].ToString().Trim() == "") { dt.Rows[dt.Rows.Count - 1][j] = DBNull.Value; }
                                                else
                                                {
                                                    if (j >= 2 && j <= 14)
                                                    {
                                                        if (chkCellValue(sCells[i].Trim(), j))
                                                            dt.Rows[dt.Rows.Count - 1][j] = sCells[i].Trim();
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
                                        #endregion
                                    }
                                }
                            }
                            #endregion

                            Application.DoEvents();
                            dt.Rows[dt.Rows.Count - 1]["colSequence"] = dt.Rows.Count;
                            dt.Rows[dt.Rows.Count - 1]["colChk"] = "False";
                        }

                        load(false, lines.Count(), (cnt + 1).ToString());
                    }
                    cnt++;
                }

                #region EXPORT DATATABLE TO GRID

                // IF THERE ARE RECORDS IN DATA TABLE THEN ASSIGN IT TO GRID
                if (dt != null && dt.Rows.Count > 0)
                    dgvGSTR4A5.DataSource = dt;

                // TOTAL CALCULATION
                int[] colGroup = { 3, 5, 8, 10, 12, 14 };
                GetTotal(colGroup);

                load(false, 0, Convert.ToString(""));
                ProgressBar.Visible = false;

                EnableControls(dgvGSTR4A5);

                #endregion
            }
            catch (Exception ex)
            {
                EnableControls(dgvGSTR4A5);
                load(false, 0, Convert.ToString(""));
                ProgressBar.Visible = false;
                MessageBox.Show("Error : " + ex.Message);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }

        private void dgvGSTR4A5_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int cNo = e.ColumnIndex;

                if (e.RowIndex >= 0)
                {
                    if (cNo == 2 || cNo == 4 || cNo == 6 || cNo == 7 || cNo == 9 || cNo == 11 || cNo == 13)
                    {
                        if (!chkCellValue(Convert.ToString(dgvGSTR4A5.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                            dgvGSTR4A5.Rows[e.RowIndex].Cells[cNo].Value = "";
                    }

                    else if (cNo == 3 || cNo == 5 || cNo == 8 || cNo == 10 || cNo == 12 || cNo == 14) // value
                    {
                        if (chkCellValue(Convert.ToString(dgvGSTR4A5.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                        {
                            int[] colNo = { e.ColumnIndex };
                            GetTotal(colNo);
                        }
                        else { dgvGSTR4A5.Rows[e.RowIndex].Cells[cNo].Value = ""; }
                    }
                    else
                    {
                        //if (!chkCellValue(Convert.ToString(dgvGSTR24.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                        //    dgvGSTR24.Rows[e.RowIndex].Cells[cNo].Value = "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }

        private Boolean chkCellValue(string cellValue, int cNo)
        {
            try
            {
                if (cellValue.Trim() != "")// NOT EQUEL BLANK
                {
                    if (cNo == 2) //GSTN
                    {
                        if (Utility.IsGSTN(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == 4) // Date
                    {
                        if (Utility.IsDate(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == 5 || cNo == 8 || cNo == 10 || cNo == 12 || cNo == 14) // value
                    {
                        if (Utility.IsNumber(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == 6) //Goods And Service
                    {
                        if (cellValue.ToLower() == "g" || cellValue.ToLower() == "goods" || cellValue.ToLower() == "s" || cellValue.ToLower() == "services")
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == 9 || cNo == 11 || cNo == 13) // Rate
                    {
                        if (Utility.IsNumber(cellValue))
                        {
                            if (Convert.ToDecimal(cellValue) > 100)
                                return false;
                            else
                                return true;
                        }
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
                MessageBox.Show("Error : " + ex.Message);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
                return false;
            }
        }

        public bool IsValidateData()
        {
            try
            {
                int _cnt = 0,sj = 0;
                string _str = "";
                
                dgvGSTR4A5.CurrentCell = dgvGSTR4A5.Rows[0].Cells[0];
                dgvGSTR4A5.AllowUserToAddRows = false;

                #region GSTN Number
                sj = 2;
                List<DataGridViewRow> list = dgvGSTR4A5.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsGSTN(Convert.ToString(x.Cells[sj].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR4A5.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper GSTN of supplier.\n";
                }
                list = dgvGSTR4A5.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsGSTN(Convert.ToString(x.Cells[sj].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR4A5.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.White;
                }
                #endregion

                #region Invoice Date
                sj = 4;
                list = null;//dd-MM-yyyy
                list = dgvGSTR4A5.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsDate(Convert.ToString(x.Cells[sj].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR4A5.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper invoice date.\n";
                }
                list = dgvGSTR4A5.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsDate(Convert.ToString(x.Cells[sj].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR4A5.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.White;
                }
                #endregion

                #region Invoice Value
                sj = 5;
                list = null;
                list = dgvGSTR4A5.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsNumber(Convert.ToString(x.Cells[sj].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR4A5.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper invoice value.\n";
                }
                list = dgvGSTR4A5.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsNumber(Convert.ToString(x.Cells[sj].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR4A5.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.White;
                }
                #endregion

                #region Goods And Services
                sj = 6;
                list = null;
                list = dgvGSTR4A5.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => "g" != Convert.ToString(x.Cells[sj].Value).ToLower() && "goods" != Convert.ToString(x.Cells[sj].Value).ToLower() && "s" != Convert.ToString(x.Cells[sj].Value).ToLower() && "services" != Convert.ToString(x.Cells[sj].Value).ToLower())
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR4A5.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper goods and services.\n";
                }
                list = dgvGSTR4A5.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => "g" == Convert.ToString(x.Cells[sj].Value).ToLower() || "goods" == Convert.ToString(x.Cells[sj].Value).ToLower() || "s" == Convert.ToString(x.Cells[sj].Value).ToLower() || "services" == Convert.ToString(x.Cells[sj].Value).ToLower())
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR4A5.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.White;
                }
                #endregion

                #region Invoice Taxable Value
                sj = 8;
                list = null;
                list = dgvGSTR4A5.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsNumber(Convert.ToString(x.Cells[sj].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR4A5.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper invoice taxable value.\n";
                }
                list = dgvGSTR4A5.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsNumber(Convert.ToString(x.Cells[sj].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR4A5.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.White;
                }
                #endregion

                #region IGST Rate
                sj = 9;
                list = null;
                list = dgvGSTR4A5.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsNumber(Convert.ToString(x.Cells[sj].Value)) || Convert.ToDouble(x.Cells[sj].Value) > 100)
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR4A5.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper IGST Rate.\n";
                }
                list = dgvGSTR4A5.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsNumber(Convert.ToString(x.Cells[sj].Value)) && Convert.ToDouble(x.Cells[sj].Value) <= 100)
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR4A5.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.White;
                }
                #endregion

                #region IGST Amount
                sj = 10;
                list = null;
                list = dgvGSTR4A5.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsNumber(Convert.ToString(x.Cells[sj].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR4A5.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper IGST Amount.\n";
                }
                list = dgvGSTR4A5.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsNumber(Convert.ToString(x.Cells[sj].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR4A5.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.White;
                }
                #endregion

                #region CGST Rate
                sj = 11;
                list = null;
                list = dgvGSTR4A5.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsNumber(Convert.ToString(x.Cells[sj].Value)) || Convert.ToDouble(x.Cells[sj].Value) > 100)
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR4A5.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper CGST Rate.\n";
                }
                list = dgvGSTR4A5.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsNumber(Convert.ToString(x.Cells[sj].Value)) && Convert.ToDouble(x.Cells[sj].Value) <= 100)
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR4A5.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.White;
                }
                #endregion

                #region CGST Amount
                sj = 12;
                list = null;
                list = dgvGSTR4A5.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsNumber(Convert.ToString(x.Cells[sj].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR4A5.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper CGST Amount.\n";
                }
                list = dgvGSTR4A5.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsNumber(Convert.ToString(x.Cells[sj].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR4A5.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.White;
                }
                #endregion

                #region SGST Rate
                sj = 13;
                list = null;
                list = dgvGSTR4A5.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsNumber(Convert.ToString(x.Cells[sj].Value)) || Convert.ToDouble(x.Cells[sj].Value) > 100)
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR4A5.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper SGST Rate.\n";
                }
                list = dgvGSTR4A5.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsNumber(Convert.ToString(x.Cells[sj].Value)) && Convert.ToDouble(x.Cells[sj].Value) <= 100)
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR4A5.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.White;
                }
                #endregion

                #region SGST Amount
                sj = 14;
                list = null;
                list = dgvGSTR4A5.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsNumber(Convert.ToString(x.Cells[sj].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR4A5.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper SGST Amount.\n";
                }
                list = dgvGSTR4A5.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsNumber(Convert.ToString(x.Cells[sj].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR4A5.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.White;
                }
                #endregion

                dgvGSTR4A5.AllowUserToAddRows = true;
                if (_str != "")
                {
                    CommonHelper.ErrorList = Convert.ToString(_str);
                    SPQErrorList obj = new SPQErrorList();
                    obj.ShowDialog();
                    return false;
                }
                else
                {
                    MessageBox.Show("Data Validation Successful.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
                return false;
            }
        }

        public void Save()
        {
            try
            {
                //if (CommonHelper.StatusIndex == 0)
                //{
                //    MessageBox.Show("Please Select File Status!");
                //    return;
                //}

                #region ADD DATATABLE COLUMN
                DataTable dt = new DataTable();
                foreach (DataGridViewColumn col in dgvGSTR4A5.Columns)
                {
                    dt.Columns.Add(col.Name.ToString());
                }
                dt.Columns.Add("colFileStatus");
                #endregion

                #region ASSIGN GRIDVIEW ROWS IN DATATABLE
                object[] rowValue = new object[dt.Columns.Count];

                foreach (DataGridViewRow dr in dgvGSTR4A5.Rows)
                {
                    if (dr.Index != dgvGSTR4A5.Rows.Count - 1)// DON'T ADD LAST ROW
                    {
                        for (int i = 0; i < dr.Cells.Count; i++)
                        {
                            rowValue[i] = Convert.ToString(dr.Cells[i].Value);
                        }

                        rowValue[dr.Cells.Count] = Convert.ToString(CommonHelper.StatusText);

                        dt.Rows.Add(rowValue);
                    }
                }
                // REMOVE FIRST COLUMM (FIELD ID)
                dt.Columns.Remove(dt.Columns[0]);
                dt.AcceptChanges();
                #endregion

                #region RECORD SAVE
                string Query = "";
                int _Result = 0;

                if (dt != null && dt.Rows.Count > 0)
                {
                    #region DELETE RECORD
                    Query = "Delete from GSTR4AFormPartA5 where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                    _Result = objGSTR4A.IUDData(Query);
                    if (_Result != 1)
                    {
                        //FAIL
                        MessageBox.Show("System error.\nPlease try after sometime!");
                        return;
                    }
                    #endregion

                    ProgressBar.Visible = true;

                    _Result = objGSTR4A.GSTR4A5BulkEntry(dt, Convert.ToString(CommonHelper.StatusText));

                    if (_Result == 1)
                    {
                        // TOTAL CALCULATION
                        int[] colNo = { 3, 5, 8, 10, 12, 14 };
                        GetTotal(colNo);

                        #region ADD DATATABLE COLUMN
                        dt = new DataTable();

                        foreach (DataGridViewColumn col in dgvGSTR4A5.Columns)
                        {
                            dt.Columns.Add(col.Name.ToString());
                        }
                        dt.Columns.Add("colFileStatus");
                        #endregion

                        #region ASSIGN GRIDVIEW ROWS IN DATATABLE
                        object[] rowVal = new object[dt.Columns.Count];

                        if (dgvGSTR4A5Total.Rows.Count == 1)
                        {
                            foreach (DataGridViewRow dr in dgvGSTR4A5Total.Rows)
                            {
                                for (int i = 0; i < dr.Cells.Count; i++)
                                {
                                    rowVal[i] = Convert.ToString(dr.Cells[i].Value);
                                }

                                rowVal[dr.Cells.Count] = "Total";

                                dt.Rows.Add(rowVal);
                            }
                        }
                        // REMOVE FIRST COLUMM (FIELD ID)
                        dt.Columns.Remove(dt.Columns[0]);
                        dt.AcceptChanges();
                        #endregion

                        _Result = objGSTR4A.GSTR4A5BulkEntry(dt, "Total");
                        if (_Result == 1)
                        {
                            //DONE
                            load(false, 0, Convert.ToString(""));
                            ProgressBar.Visible = false;
                            MessageBox.Show("Data Saved Successfully …!!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // BIND DATA
                            GetData();
                        }
                        else
                        {
                            // IF ERRORS OCCURS WHILE INSERTING DATA TO DATABASE
                            load(false, 0, Convert.ToString(""));
                            ProgressBar.Visible = false;
                            MessageBox.Show("System error.\nPlease try after sometime!");
                            return;
                        }
                    }
                    else
                    {
                        //FAIL
                        load(false, 0, Convert.ToString(""));
                        ProgressBar.Visible = false;
                        MessageBox.Show("System error.\nPlease try after sometime!");
                        return;
                    }
                }
                else
                {
                    #region DELETE RECORD
                    Query = "Delete from GSTR4AFormPartA5 where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                    _Result = objGSTR4A.IUDData(Query);
                    if (_Result == 1)
                    {
                        //DONE
                        MessageBox.Show("Record Successfully Deleted!");

                        // MAKE FILE STATUS BLANK
                        ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(0);

                        int[] colNo = { 3, 5, 8, 10, 12, 14 };
                        GetTotal(colNo);
                    }
                    else
                    {
                        //FAIL
                        MessageBox.Show("System error.\nPlease try after sometime!");
                        return;
                    }
                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                load(false, 0, Convert.ToString(""));
                ProgressBar.Visible = false;
                MessageBox.Show(ex.Message);
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
                if (dgvGSTR4A5.CurrentCell.RowIndex == 0 && dgvGSTR4A5.CurrentCell.ColumnIndex == 0)
                {
                    dgvGSTR4A5.CurrentCell = dgvGSTR4A5.Rows[0].Cells[1];
                }
                else { dgvGSTR4A5.CurrentCell = dgvGSTR4A5.Rows[0].Cells[0]; }

                // CREATE FLAG FRO DELETE ROWS
                Boolean flgChk = false; Boolean flgSelect = false;

                // CREATE OBJECT OF SELECTED ROW TO DELETE
                List<DataGridViewRow> toDelete = new List<DataGridViewRow>();

                // CHECK THERE ARE RECORD PRESENT IN GRID
                if (dgvGSTR4A5.Rows.Count > 1)
                {
                    // FLAG TRUE IF CHECK ALL SELECTED
                    if (ckboxHeader.Checked)
                        flgChk = true;

                    #region ADD SELECTED ROW TO OBJECT FOR DELETE
                    for (int i = 0; i < dgvGSTR4A5.Rows.Count - 1; i++)
                    {
                        if (dgvGSTR4A5[0, i].Value != null && dgvGSTR4A5[0, i].Value.ToString() != "")
                        {
                            if (Convert.ToBoolean(dgvGSTR4A5[0, i].Value) == true)
                            {
                                // ADD ROW TO OBJECT IF ROW IS SELECTED
                                flgSelect = true;
                                toDelete.Add(dgvGSTR4A5.Rows[i]);
                            }
                        }
                    }
                    #endregion

                    // CHECK ROW IS SELECTED TO DELETE
                    if (flgChk || flgSelect)
                    {
                        // OPEN DIALOG FOR THE CONFIRMATION
                        DialogResult result = MessageBox.Show("Do you want to delete this data?", "Confirmation", MessageBoxButtons.YesNo);

                        // IF USER CONFIRM FOR DELETING RECORDS
                        if (result == DialogResult.Yes)
                        {
                            #region DELETE RECORDS

                            if (flgChk)
                            {
                                // IF CHECK BOX OF CHECK ALL IS SELECTED
                                flgChk = false;

                                // CREATE DATATABLE AND ADD COLUMN AS PAR MAIN GRID
                                DataTable dt = new DataTable();
                                foreach (DataGridViewColumn col in dgvGSTR4A5.Columns)
                                {
                                    dt.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }

                                // ASSIGN BLANK DATATABLE TO GRID
                                dgvGSTR4A5.DataSource = dt;
                            }
                            else if (flgSelect == true)
                            {
                                int temp = 1;
                                // DELETE SELECTED ROW
                                foreach (DataGridViewRow row in toDelete)
                                {
                                    dgvGSTR4A5.Rows.RemoveAt(row.Index);
                                    Application.DoEvents();
                                    if (temp == 1)
                                    {
                                        ProgressBar.Visible = true;
                                        load(true, toDelete.Count, Convert.ToString(temp));
                                    }
                                    else { load(false, toDelete.Count, Convert.ToString(temp)); }
                                    temp++;
                                }
                            }

                            // SEQUANCING MAIN GRID RECORDS
                            for (int i = 0; i < dgvGSTR4A5.Rows.Count - 1; i++)
                            {
                                dgvGSTR4A5.Rows[i].Cells["colSequence"].Value = Convert.ToString(i + 1);
                            }

                            if (dgvGSTR4A5.Rows.Count == 1)
                            {
                                // IF THERE ARE NO RECORDS IN MAIN GRID THENE ASSIGN BLANK DATATABLE TO TOTAL GRID
                                DataTable dtTotal = new DataTable();
                                foreach (DataGridViewColumn col in dgvGSTR4A5Total.Columns)
                                {
                                    dtTotal.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }
                                dgvGSTR4A5Total.DataSource = dtTotal;
                            }

                            // SET CONTROL PROPERTY AFTER ROW DELETION
                            ckboxHeader.Checked = false;
                            dgvGSTR4A5.Columns[0].HeaderText = "Check All";
                            #endregion
                        }
                    }

                    // TOTAL CALCULATION
                    int[] colNo = { 3, 5, 8, 10, 12, 14 };
                    GetTotal(colNo);

                    load(false, 0, Convert.ToString(""));
                    ProgressBar.Visible = false;
                }
                else
                {
                    // IF THERE ARE NO RECORD TO DELETE
                    ckboxHeader.Checked = false;
                    dgvGSTR4A5.Columns[0].HeaderText = "Check All";
                    MessageBox.Show("There are no records to delete..");
                }
            }
            catch (Exception ex)
            {
                load(false, 0, Convert.ToString(""));
                ProgressBar.Visible = false;
                MessageBox.Show("Error : " + ex.Message);
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
                    if (fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0)
                    {
                        #region IF IMPOTED FILE IS OPEN THEN CLOSE OPEN FILE
                        foreach (Process proc in System.Diagnostics.Process.GetProcessesByName("EXCEL"))
                        {
                            if (proc.MainWindowTitle == "Microsoft Excel - " + file.SafeFileName)
                                proc.Kill();
                        }
                        #endregion

                        // CREATE DATATABLE TO STORE MAIN GRID DATA
                        DataTable dt = new DataTable();
                        dt = (DataTable)dgvGSTR4A5.DataSource;

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
                                DisableControls(dgvGSTR4A5);

                                #region IMPORT EXCEL DATATABLE TO GRID DATATABLE
                                if (dtExcel != null && dtExcel.Rows.Count > 0)
                                {
                                    int tmp = 1;
                                    foreach (DataRow row in dtExcel.Rows)
                                    {
                                        load(false, dtExcel.Rows.Count * 2, (dtExcel.Rows.Count + tmp).ToString());

                                        // COPY EACH ROW OF IMPORTED DATATABLE ROW TO GRID DATATALE
                                        DataRow newRow = dt.NewRow();
                                        newRow.ItemArray = row.ItemArray;
                                        dt.Rows.Add(newRow);
                                        dt.Rows[dt.Rows.Count - 1]["colSequence"] = dt.Rows.Count;
                                        Application.DoEvents();
                                        tmp++;
                                    }
                                }
                                dt.AcceptChanges();
                                #endregion

                                #region RENAME DATATABLE COLUMN NAME AS PAR MAIN GRID
                                foreach (DataGridViewColumn col in dgvGSTR4A5.Columns)
                                {
                                    dt.Columns[col.Index].ColumnName = col.Name.ToString();
                                    col.DataPropertyName = col.Name;
                                }
                                #endregion

                                //ASSIGN DATATABLE TO DATAGRID
                                dgvGSTR4A5.DataSource = dt;

                                // ENABLE MAIN GRID
                                EnableControls(dgvGSTR4A5);
                            }
                            else
                            {
                                // IF THERE ARE NO RECORDS IN MAIN GRID

                                if (dtExcel != null && dtExcel.Rows.Count > 0)
                                {
                                    // IF THERE ARE DATA IN IMPORTED EXCEL FILE

                                    // DISABLE MAIN GRID

                                    #region RENAME DATATABLE COLUMN NAME AS PAR MAIN GRID
                                    foreach (DataGridViewColumn col in dgvGSTR4A5.Columns)
                                    {
                                        dtExcel.Columns[col.Index].ColumnName = col.Name.ToString();
                                        col.DataPropertyName = col.Name;
                                    }
                                    dtExcel.AcceptChanges();

                                    // ASSIGN DATATALE TO GRID
                                    dgvGSTR4A5.DataSource = dt;

                                    // ENABLE MAIN GRID
                                    EnableControls(dgvGSTR4A5);
                                    #endregion
                                }
                                else
                                {
                                    // IF THERE ARE NO RECORDS IN IMPORTED EXCEL FILE
                                    load(false, 0, Convert.ToString(""));
                                    ProgressBar.Visible = false;
                                    MessageBox.Show("There are no records found in imported excel ...!!!!");
                                }
                            }

                            // TOTAL CALCULATION
                            int[] colNo = { 3, 5, 8, 10, 12, 14 };
                            GetTotal(colNo);

                            load(false, 0, Convert.ToString(""));
                            ProgressBar.Visible = false;
                        }
                        else
                        {
                            load(false, 0, Convert.ToString(""));
                            ProgressBar.Visible = false;
                            MessageBox.Show("Please import valid excel template...!!!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please choose .xls or .xlsx file only.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error); //CUSTOM MESSAGEBOX TO SHOW ERROR  
                    }
                }
            }
            catch (Exception ex)
            {
                EnableControls(dgvGSTR4A5);
                load(false, 0, Convert.ToString(""));
                ProgressBar.Visible = false;
                MessageBox.Show("Error : " + ex.Message);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }

        public DataTable ReadExcel(string fileName, string fileExt, DataTable grdData)
        {
            string conn = string.Empty;
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
                        OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [B2BA_4A5$]", con);
                        oleAdpt.Fill(dtexcel); //FILL EXCEL DATA INTO DATATABLE
                    }
                    catch
                    {
                        // CALL WHEN IMPORTED TEMPLATE SHEET NAME IS DIFFER FROM PREDEFINE TEMPLATE
                        DataTable dt = new DataTable();
                        dt.Columns.Add("colError");
                        return dt;
                    }
                    if (dtexcel != null && dtexcel.Rows.Count > 0)
                    {
                        #region VALIDATE TEMPLATE
                        for (int i = 1; i < dgvGSTR4A5.Columns.Count; i++)
                        {
                            Boolean flg = false;
                            for (int j = 0; j < dtexcel.Columns.Count; j++)
                            {
                                // CHECK GRID COLUMN IS PRESENT OR NOT IN IMPORTED EXCEL
                                if (dgvGSTR4A5.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim().Length >= 40)
                                {
                                    string piece = dgvGSTR4A5.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim().Substring(0, 40);
                                    string piece1 = string.Empty;

                                    if (dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim().Length >= 40)
                                        piece1 = dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim().Substring(0, 40);
                                    else
                                        piece1 = dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim();

                                    if (piece == piece1)
                                    {
                                        // if grid column present in excel then its index as par grid column index
                                        flg = true;
                                        dtexcel.Columns[j].SetOrdinal(dgvGSTR4A5.Columns[i].Index - 1);
                                        break;
                                    }
                                }
                                else if (dgvGSTR4A5.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim() == dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim())
                                {
                                    // IF GRID COLUMN PRESENT IN EXCEL THEN ITS INDEX AS PAR GRID COLUMN INDEX
                                    flg = true;
                                    dtexcel.Columns[j].SetOrdinal(dgvGSTR4A5.Columns[i].Index - 1);
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
                        if (dtexcel.Columns.Count >= dgvGSTR4A5.Columns.Count - 2)
                        {
                            for (int i = dtexcel.Columns.Count - 1; i > (dgvGSTR4A5.Columns.Count - 2); i--)
                            {
                                dtexcel.Columns.Remove(dtexcel.Columns[i]);
                            }
                        }
                        dtexcel.AcceptChanges();
                        #endregion

                        #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME
                        foreach (DataGridViewColumn col in dgvGSTR4A5.Columns)
                        {
                            if (col.Index != 0)
                                dtexcel.Columns[col.Index - 1].ColumnName = col.Name.ToString();
                        }
                        #endregion

                        // ADD CHECK BOX COLUMN TO DATATBLE AND MAKE IT FIRST TABLE COLUMN
                        dtexcel.Columns.Add(new DataColumn("colChk"));
                        dtexcel.Columns["colChk"].SetOrdinal(0);
                        dtexcel.AcceptChanges();

                        #region SET SEQUENCE NO
                        for (int i = 0; i < dtexcel.Rows.Count; i++)
                        {
                            ProgressBar.Visible = true;
                            if (i == 0)
                                if (grdData != null && grdData.Rows.Count > 0)
                                    load(true, dtexcel.Rows.Count * 2, i.ToString());
                                else
                                    load(true, dtexcel.Rows.Count, i.ToString());
                            else
                                load(false, dtexcel.Rows.Count, i.ToString());

                            dtexcel.Rows[i]["colChk"] = "False";
                            dtexcel.Rows[i]["colSequence"] = i + 1;
                        }
                        dtexcel.AcceptChanges();
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    load(false, 0, Convert.ToString(""));
                    ProgressBar.Visible = false;
                    MessageBox.Show("Error : " + ex.Message);
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
                if (dgvGSTR4A5.Rows.Count > 1)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID

                    #region CREATE WORKBOOK AND ASSIGN COLUMNNAME
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook WB = excelApp.Workbooks.Add(Missing.Value);

                    Excel.Worksheet newWS = (Excel.Worksheet)excelApp.Worksheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    newWS.Name = "B2BA_4A5";

                    // DELETE UNUSED WORKSHEETS FROM WORKBOOK
                    foreach (Excel.Worksheet ws in WB.Worksheets)
                    {
                        if (ws.Name != "B2BA_4A5")
                            ((Excel.Worksheet)WB.Worksheets[ws.Name]).Delete();
                    }

                    // ASSIGN COLUMN HEADER AS PAR THE GRID HEADER
                    for (int i = 1; i < dgvGSTR4A5.Columns.Count; i++)
                    {
                        newWS.Cells[1, i] = dgvGSTR4A5.Columns[i].HeaderText.ToString();

                        // SET COLUMN WIDTH
                        if (i == 1)
                            ((Excel.Range)newWS.Cells[1, i]).ColumnWidth = 7;
                        else if (i == 2)
                            ((Excel.Range)newWS.Cells[1, i]).ColumnWidth = 20;
                        else
                            ((Excel.Range)newWS.Cells[1, i]).ColumnWidth = 15;
                    }

                    // GET RANGE AND SET DIFFRENT PROPERTIES
                    Excel.Range headerRange = (Excel.Range)newWS.get_Range((Excel.Range)newWS.Cells[1, 1], (Excel.Range)newWS.Cells[1, dgvGSTR4A5.Columns.Count - 1]);
                    headerRange.WrapText = true;
                    headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Calibri";
                    #endregion

                    #region COPY DATA FROM DATATABLE TO ARRAY

                    // CREATE ARRAY TO HOLD THE DATA OF DATATABLE
                    object[,] arr = new object[dgvGSTR4A5.Rows.Count - 1, dgvGSTR4A5.Columns.Count];

                    // ASSIGN DATA TO ARRAY FROM DATATABLE
                    if (CommonHelper.IsLicence)
                    {
                        // FOR LICENECE ALLOWS TO EXPORT ALL RECORDS
                        for (int i = 0; i < dgvGSTR4A5.Rows.Count - 1; i++)
                        {
                            ProgressBar.Visible = true;
                            if (i == 0)
                                load(true, dgvGSTR4A5.Rows.Count - 1, i.ToString());
                            else
                                load(false, dgvGSTR4A5.Rows.Count - 1, i.ToString());

                            for (int j = 1; j < dgvGSTR4A5.Columns.Count; j++)
                            {
                                arr[i, j - 1] = dgvGSTR4A5.Rows[i].Cells[j].Value.ToString();
                            }
                        }
                    }
                    else
                    {
                        // FOR DEMO ALLOW ONLY 100 RECORDS TO EXPORT
                        for (int i = 0; i < dgvGSTR4A5.Rows.Count - 1; i++)
                        {
                            if (i < 100)
                            {
                                for (int j = 1; j < dgvGSTR4A5.Columns.Count; j++)
                                {
                                    arr[i, j - 1] = dgvGSTR4A5.Rows[i].Cells[j].Value.ToString();
                                }
                            }
                        }
                    }
                    //SET EXCEL RANGE TO PASTE THE DATA
                    Excel.Range top = (Excel.Range)newWS.Cells[2, 1];
                    Excel.Range bottom = (Excel.Range)newWS.Cells[dgvGSTR4A5.Rows.Count, dgvGSTR4A5.Columns.Count];
                    Excel.Range sheetRange = newWS.Range[top, bottom];
                    //FILL ARRAY IN EXCEL
                    sheetRange.Value2 = arr;

                    #endregion

                    load(false, 0, Convert.ToString(""));
                    ProgressBar.Visible = false;

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
                        //    MessageBox.Show("Please close opened related excel file..");
                        //    return;
                        //}

                        // SAVE EXCEL FILE AND CLOSE CREATED APPLICATION
                        newWS.SaveAs(saveExcel.FileName);
                        excelApp.Quit();
                        MessageBox.Show("Excel file saved!");
                    }
                    #endregion
                }
                else
                {
                    // IF THERE ARE NO RECORD IN MAIN  GRID
                    MessageBox.Show("ExportToExcel: There are no records to export...!!!");
                }
            }
            catch (Exception ex)
            {
                load(false, 0, Convert.ToString(""));
                ProgressBar.Visible = false;
                MessageBox.Show("Error : " + ex.Message);
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
                        // CREATE DATATABLE AND SAVE GRID DATA
                        DataTable dt = new DataTable();
                        dt = (DataTable)dgvGSTR4A5.DataSource;

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
                                DisableControls(dgvGSTR4A5);

                                #region COPY IMPORTED CSV DATATABLE DATA INTO GRID DATATABLE
                                if (dtCsv != null && dtCsv.Rows.Count > 0)
                                {
                                    int tmp = 1;
                                    foreach (DataRow row in dtCsv.Rows)
                                    {
                                        load(false, dtCsv.Rows.Count * 2, (dtCsv.Rows.Count + tmp).ToString());

                                        // COPY EACH ROW OF IMPORTED DATATABLE ROW TO GRID DATATABLE
                                        DataRow newRow = dt.NewRow();
                                        newRow.ItemArray = row.ItemArray;
                                        dt.Rows.Add(newRow);
                                        dt.Rows[dt.Rows.Count - 1]["colSequence"] = dt.Rows.Count;
                                        Application.DoEvents();
                                        tmp++;
                                    }
                                }
                                dt.AcceptChanges();
                                #endregion

                                #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME
                                foreach (DataGridViewColumn col in dgvGSTR4A5.Columns)
                                {
                                    dtCsv.Columns[col.Index].ColumnName = col.Name.ToString();
                                    col.DataPropertyName = col.Name;
                                }
                                dt.AcceptChanges();
                                #endregion

                                // ASSIGN DATATABLE TO GRID
                                dgvGSTR4A5.DataSource = dt;

                                // ENABLE MAIN GRID
                                EnableControls(dgvGSTR4A5);
                            }
                            else
                            {
                                // IF THERE ARE NO RECORDS IN MAIN GRID

                                if (dtCsv != null && dtCsv.Rows.Count > 0)
                                {
                                    // IF THERE ARE RECORD PRESENT IN IMPORT FILE

                                    // DISABLE MAIN GRID
                                    DisableControls(dgvGSTR4A5);

                                    #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME
                                    foreach (DataGridViewColumn col in dgvGSTR4A5.Columns)
                                    {
                                        dtCsv.Columns[col.Index].ColumnName = col.Name.ToString();
                                        col.DataPropertyName = col.Name;
                                    }
                                    dtCsv.AcceptChanges();

                                    // ASSIGN DATATABLE TO GRID
                                    dgvGSTR4A5.DataSource = dtCsv;

                                    // ENABLE MAIN GRID
                                    EnableControls(dgvGSTR4A5);
                                    #endregion
                                }
                                else
                                {
                                    // IF THERE ARE NO RECORDS IN IMPORT FILE
                                    load(false, 0, Convert.ToString(""));
                                    ProgressBar.Visible = false;
                                    MessageBox.Show("There are no records in CSV file...!!!");
                                    return;
                                }
                            }

                            // TOTAL CALCULATION
                            int[] colNo = { 3, 5, 8, 10, 12, 14 };
                            GetTotal(colNo);

                            load(false, 0, Convert.ToString(""));
                            ProgressBar.Visible = false;
                        }
                        else
                        {
                            load(false, 0, Convert.ToString(""));
                            ProgressBar.Visible = false;
                            MessageBox.Show("Please import valid csv template...!!!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please choose .csv or .~csv file only.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error); //CUSTOM MESSAGEBOX TO SHOW ERROR  
                    }
                }
            }
            catch (Exception ex)
            {
                EnableControls(dgvGSTR4A5);
                load(false, 0, Convert.ToString(""));
                ProgressBar.Visible = false;
                MessageBox.Show("Error : " + ex.Message);
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

                    #region VALIDATE TEMPLATE
                    for (int i = 1; i < dgvGSTR4A5.Columns.Count; i++)
                    {
                        Boolean flg = false;
                        for (int j = 0; j < csvData.Columns.Count; j++)
                        {
                            // CHECK GRID COLUMN IS PRESENT OR NOT IN IMPORTED EXCEL
                            if (dgvGSTR4A5.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim().Length >= 40)
                            {
                                string piece = dgvGSTR4A5.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim().Substring(0, 40);
                                string piece1 = string.Empty;

                                if (csvData.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim().Length >= 40)
                                    piece1 = csvData.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim().Substring(0, 40);
                                else
                                    piece1 = csvData.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim();

                                if (piece == piece1)
                                {
                                    // if grid column present in excel then its index as par grid column index
                                    flg = true;
                                    csvData.Columns[j].SetOrdinal(dgvGSTR4A5.Columns[i].Index - 1);
                                    break;
                                }
                            }
                            else if (dgvGSTR4A5.Columns[i].HeaderText.Replace(" ", "") == csvData.Columns[j].ColumnName.Replace(" ", "").Trim())
                            {
                                // IF GRID COLUMN PRESENT IN EXCEL THEN ITS INDEX AS PAR GRID COLUMN INDEX
                                flg = true;
                                csvData.Columns[j].SetOrdinal(dgvGSTR4A5.Columns[i].Index - 1);
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

                    #region REMOVE UNUSED COLUMN FROM CSV DATATABLE
                    if (csvData.Columns.Count >= dgvGSTR4A5.Columns.Count - 2)
                    {
                        for (int i = csvData.Columns.Count - 1; i > (dgvGSTR4A5.Columns.Count - 2); i--)
                        {
                            csvData.Columns.Remove(csvData.Columns[i]);
                        }
                    }
                    #endregion

                    #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME
                    foreach (DataGridViewColumn col in dgvGSTR4A5.Columns)
                    {
                        if (col.Index != 0)
                            csvData.Columns[col.Index - 1].ColumnName = col.Name.ToString();
                    }
                    #endregion

                    // ADD CHECK BOX COLUMN TO DATATABLE AND SET TO FIRST COLUMN
                    csvData.Columns.Add(new DataColumn("colChk"));
                    csvData.Columns["colChk"].SetOrdinal(0);
                    csvData.AcceptChanges();

                    #region SET CHECK BOX AND SEQUENCE NO
                    for (int i = 0; i < csvData.Rows.Count; i++)
                    {
                        ProgressBar.Visible = true;
                        if (i == 0)
                            if (grdData != null && grdData.Rows.Count > 0)
                                load(true, csvData.Rows.Count * 2, i.ToString());
                            else
                                load(true, csvData.Rows.Count, i.ToString());
                        else
                            load(false, csvData.Rows.Count, i.ToString());

                        csvData.Rows[i]["colChk"] = "False";
                        csvData.Rows[i]["colSequence"] = i + 1;
                    }
                    csvData.AcceptChanges();
                    #endregion
                }
                catch (Exception ex)
                {
                    load(false, 0, Convert.ToString(""));
                    ProgressBar.Visible = false;
                    MessageBox.Show("Error : " + ex.Message);
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
                if (dgvGSTR4A5.Rows.Count > 1)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID
                    string csv = string.Empty;

                    // CREATE DATATABLE AND GET GRID DATA
                    DataTable dt = new DataTable();
                    dt = (DataTable)dgvGSTR4A5.DataSource;
                    dt.AcceptChanges();

                    #region ASSIGN COLUMN NAME TO CSV STRING
                    for (int i = 1; i < dgvGSTR4A5.Columns.Count; i++)
                    {
                        csv += dgvGSTR4A5.Columns[i].HeaderText + ',';
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
                            ProgressBar.Visible = true;
                            if (sj == 0)
                                load(true, dt.Rows.Count, (sj + 1).ToString());
                            else
                                load(false, dt.Rows.Count, (sj + 1).ToString());

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
                                MessageBox.Show("CSV file saved.");
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Please close opened related csv file..");
                            return;
                        }
                    }
                    #endregion
                }
                else
                {
                    // IF THERE ARE NO RECORD IN MAIN  GRID
                    MessageBox.Show("ExportToCSV: There are no records to export...!!!");
                }
            }
            catch (Exception ex)
            {
                load(false, 0, Convert.ToString(""));
                ProgressBar.Visible = false;
                MessageBox.Show("Error : " + ex.Message);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }

        #endregion

        #region PDF TRANSACTIONS

        public void ExportPDF()
        {
            try
            {
                if (dgvGSTR4A5.Rows.Count > 1)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID

                    #region CREATING ITEXTSHARP TABLE FROM THE DATATABLE DATA AND ASSIGNING TABLE HEADER
                    PdfPTable pdfTable = new PdfPTable(dgvGSTR4A5.ColumnCount - 1);
                    pdfTable.DefaultCell.Padding = 3;
                    pdfTable.WidthPercentage = 100;
                    pdfTable.DefaultCell.BorderWidth = 0;
                    iTextSharp.text.Font fontHeader = iTextSharp.text.FontFactory.GetFont("Calibri", 6);

                    // ADD HEADER TO PDF TABLE
                    string headerName = "5. Inward supplies received from registered taxable person";
                    pdfTable = AssignHeader(pdfTable, headerName);
                    #endregion

                    #region ADDING HEADER ROW
                    int i = 0;
                    foreach (DataGridViewColumn column in dgvGSTR4A5.Columns)
                    {
                        if (i != 0)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, fontHeader));
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
                        foreach (DataGridViewRow row in dgvGSTR4A5.Rows)
                        {
                            i = 0;
                            ProgressBar.Visible = true;
                            if (sj == 0)
                                load(true, dgvGSTR4A5.Rows.Count - 1, sj.ToString());
                            else
                                load(false, dgvGSTR4A5.Rows.Count - 1, sj.ToString());
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                if (cell.Value != null && i != 0) // && i != 1)
                                {
                                    //CREATE PDF CELL TO GRID RECORDS
                                    PdfPCell cell1 = new PdfPCell(new Phrase(cell.Value.ToString(), fontHeader));
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
                        foreach (DataGridViewRow row in dgvGSTR4A5.Rows)
                        {
                            if (sj < 100)
                            {
                                i = 0;
                                foreach (DataGridViewCell cell in row.Cells)
                                {
                                    if (cell.Value != null && i != 0) // && i != 1)
                                    {
                                        //CREATE PDF CELL TO GRID RECORDS
                                        PdfPCell cell1 = new PdfPCell(new Phrase(cell.Value.ToString(), fontHeader));
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

                    load(false, 0, Convert.ToString(""));
                    ProgressBar.Visible = false;

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
                            MessageBox.Show("PDF file saved.");
                        }
                        catch
                        {
                            MessageBox.Show("Please close opened related pdf file..");
                            return;
                        }
                    }
                    #endregion
                }
                else
                {
                    // IF THERE ARE NO RECORD IN MAIN  GRID
                    MessageBox.Show("ExportToPDF: There are no records to export...!!!");
                }
            }
            catch (Exception ex)
            {
                load(false, 0, Convert.ToString(""));
                ProgressBar.Visible = false;
                MessageBox.Show("Error : " + ex.Message);
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
                ce1.Colspan = dgvGSTR4A5.Columns.Count;
                ce1.VerticalAlignment = Element.ALIGN_CENTER;
                ce1.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(ce1);

                PdfPCell ce2 = new PdfPCell(new Phrase(" "));
                ce2.Colspan = dgvGSTR4A5.Columns.Count;
                ce2.BorderWidth = 0;
                pdfTable.AddCell(ce2);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }

            return pdfTable;
        }

        #endregion

        public void SetGridViewColor()
        {
            try
            {
                // DO NOT ALLOW TO AUTO GENERATE COLUMNS
                dgvGSTR4A5.AutoGenerateColumns = false;

                // SET HEIGHT WIDTH OF FORM
                this.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.97));
                this.Height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.77));

                // SET WIDTH OF HEADER, MAIN AND TOTAL GRID
                this.panel1.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.96));
                this.dgvGSTR4A5.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.96));
                this.dgvGSTR4A5Total.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.96));

                // SET HEIGHT OF MAIN GRID
                this.dgvGSTR4A5.Height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.65));
                this.dgvGSTR4A5Total.Height = 40;

                // SET LOCATION OF HEADER,LOADING PIC, CHECKBOX AND MAIN AND TOTAL GRID
                this.panel1.Location = new System.Drawing.Point(12, 0);
                this.dgvGSTR4A5.Location = new System.Drawing.Point(12, Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.05)));
                this.dgvGSTR4A5Total.Location = new System.Drawing.Point(12, Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.71)));
                this.ckboxHeader.Location = new System.Drawing.Point(32, Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.135)));
                //this.pbGSTR1.Location = new System.Drawing.Point(Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.45)), Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.30)));

                // SET MAIN GRID PROPERTY
                dgvGSTR4A5.EnableHeadersVisualStyles = false;
                dgvGSTR4A5.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(23, 196, 187);
                dgvGSTR4A5.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

                this.dgvGSTR4A5.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                this.dgvGSTR4A5.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                this.dgvGSTR4A5.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

                foreach (DataGridViewColumn column in dgvGSTR4A5.Columns)
                {
                    //column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }

        public void load(bool flg, int maxnumber, string Msg)
        {
            if (flg)
            {
                pBar.Value = 0;
                label2.Text = ".......";
                pBar.Maximum = maxnumber;
            }
            if (maxnumber != 0)
            {
                Application.DoEvents();
                pBar.Value = Convert.ToInt32(Msg);

                label2.Text = Convert.ToString(Math.Round((Convert.ToDouble(pBar.Value) / Convert.ToDouble(pBar.Maximum)) * 100, 2)) + "%";

                if (Math.Round((Convert.ToDouble(pBar.Value) / Convert.ToDouble(pBar.Maximum)) * 100, 2) >= 95)
                {
                    pBar.PerformStep();
                }
            }
            else { pBar.Value = 0; pBar.Text = Convert.ToString(""); }

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ((DataTable)dgvGSTR4A5.DataSource).DefaultView.RowFilter = string.Format("colSequence like '%{0}%' or colGSTINofSuplier like '%{1}%' or colInvoiceNo like '%{2}%' or colInvoiceDate like '%{3}%' or colInvoiceValue like '%{4}%' or colInvoiceGudServi like '%{5}%' or colInvoiceHSNSAC like '%{6}%' or colInvoiceTaxableVal like '%{7}%' or colIGSTRate like '%{8}%' or colIGSTAmnt like '%{9}%' or colCGSTRate like '%{10}%' or colCGSTAmnt like '%{11}%' or colSGSTRate like '%{12}%' or colSGSTAmnt like '%{13}%'", txtSearch.Text.Trim().Replace("'", "''"), txtSearch.Text.Trim().Replace("'", "''"), txtSearch.Text.Trim().Replace("'", "''"), txtSearch.Text.Trim().Replace("'", "''"), txtSearch.Text.Trim().Replace("'", "''"), txtSearch.Text.Trim().Replace("'", "''"), txtSearch.Text.Trim().Replace("'", "''"), txtSearch.Text.Trim().Replace("'", "''"), txtSearch.Text.Trim().Replace("'", "''"), txtSearch.Text.Trim().Replace("'", "''"), txtSearch.Text.Trim().Replace("'", "''"), txtSearch.Text.Trim().Replace("'", "''"), txtSearch.Text.Trim().Replace("'", "''"), txtSearch.Text.Trim().Replace("'", "''"), txtSearch.Text.Trim().Replace("'", "''"));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }

        private void dgvGSTR4A5_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                // SET INDEX OF USER ADDED ROW IN MAIN GRID
                dgvGSTR4A5.Rows[e.Row.Index - 1].Cells[1].Value = e.Row.Index;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }

        private void dgvGSTR4A5_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                // SET SEQUNCING AFTER USER DELETING ROW IN GRID
                for (int i = e.Row.Index; i < dgvGSTR4A5.Rows.Count - 1; i++)
                {
                    dgvGSTR4A5.Rows[i].Cells["colSequence"].Value = i;
                }

                // TOTAL CALCULATION
                int[] colNo = { 3, 5, 8, 10, 12, 14 };
                GetTotal(colNo);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error : " + ex.Message);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }

        #region DISABLE/ENABLE CONTROLS

        private void DisableControls(Control con)
        {
            foreach (Control c in con.Controls)
            {
                if (c.Name != "frmGSTR4A5" && c.Name != "dgvGSTR4A5Total")
                    DisableControls(c);
            }
            con.Enabled = false;
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

        #region SCROLL GRID

        private void dgvGSTR4A5_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                // SET TOTAL GRID OFFSET AS PAR MAIN GRID SCROLL
                this.dgvGSTR4A5Total.HorizontalScrollingOffset = this.dgvGSTR4A5.HorizontalScrollingOffset;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
            }
        }

        private void dgvGSTR4A5Total_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                // SET TOTAL GRID OFFSET AS PAR MAIN GRID SCROLL
                this.dgvGSTR4A5.HorizontalScrollingOffset = this.dgvGSTR4A5Total.HorizontalScrollingOffset;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
            }
        }

        #endregion

        #region CHECK ALL AND UNCHECK ALL

        private void dgvGSTR4A5_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                // CHECK FIRST COLUMN HEADER PRESSED AND MAIN GRID HAVING RECORDS
                if (e.ColumnIndex == 0 && dgvGSTR4A5.Rows.Count > 1)
                {
                    // CHECK AND UNCHECK CHECK BOX OF HEADER FOR SELECTING AND UNSELECTING ALL RECORDS
                    if (dgvGSTR4A5.Columns[e.ColumnIndex].HeaderText == "Check All")
                        ckboxHeader.Checked = true;
                    else if (dgvGSTR4A5.Columns[e.ColumnIndex].HeaderText == "Uncheck All")
                        ckboxHeader.Checked = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
            }
        }

        private void ckboxHeader_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                // IF THERE ARE RECORDS IN MAIN GRID
                if (dgvGSTR4A5.Rows.Count > 1)
                {
                    pbGSTR1.Visible = true;
                    if (ckboxHeader.Checked)
                    {
                        // IF CHECK BOX IS CHECKED

                        // SET CHECK BOX COLUMN VALUE AS TRUE
                        for (int i = 0; i < dgvGSTR4A5.Rows.Count - 1; i++)
                        {
                            dgvGSTR4A5.Rows[i].Cells[0].Value = "True";
                            Application.DoEvents();
                        }

                        // CHANGE HEADER TEXT AND WIDTH OF COLUMN AND POSITION OF CHECK BOX OF CHECK ALL COLUMN
                        //dgvGSTR4A5.Columns[0].DefaultCellStyle.NullValue = true;
                        dgvGSTR4A5.Columns[0].HeaderText = "Uncheck All";
                    }
                    else if (ckboxHeader.Checked == false)
                    {
                        // IF CHECK BOX IS UNCHECKED

                        // SET CHECK BOX COLUMN VALUE AS FALSE
                        for (int i = 0; i < dgvGSTR4A5.Rows.Count - 1; i++)
                        {
                            dgvGSTR4A5.Rows[i].Cells[0].Value = "False";
                            Application.DoEvents();
                        }

                        // CHANGE HEADER TEXT AND WIDTH OF COLUMN AND POSITION OF CHECK BOX OF CHECK ALL COLUMN
                        //dgvGSTR4A5.Columns[0].DefaultCellStyle.NullValue = false;
                        dgvGSTR4A5.Columns[0].HeaderText = "Check All";
                    }
                    pbGSTR1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                load(false, 0, Convert.ToString(""));
                pbGSTR1.Visible = false;
                MessageBox.Show("Error : " + ex.Message);
            }
        }
        #endregion

        private void dgvGSTR4A5Total_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                if (dgvGSTR4A5Total.Rows.Count > 0)
                {
                    DataGridViewRow row = this.dgvGSTR4A5Total.RowTemplate;
                    row.MinimumHeight = 30;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }

        private void frmGSTR4A5_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName("");
        }
    }
}
