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
    public partial class SPQGSTR1HSNOutwardSummary : Form
    {
        r1Publicclass objGSTR7 = new r1Publicclass();

        public SPQGSTR1HSNOutwardSummary()
        {
            InitializeComponent();

            // set grid property
            //SetGridViewColor();

            // Bind data
            GetData();
            // total calculation
            string[] colNo = { "colTotalQuantity", "colTotalValue", "colTotalTaxableValue", "colIGST", "colCGST", "colSGST", "colCess" };
            GetTotal(colNo);
            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName(CommonHelper.ReturnName);

            pbGSTR1.Visible = false;
            BindFilter();

            dgvGSTR1_HSNSummary.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dgvGSTR1_HSNSummary.EnableHeadersVisualStyles = false;
            dgvGSTR1_HSNSummary.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);
            dgvGSTR1_HSNSummary.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvGSTR1_HSNSummary.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dgvGSTR1_HSNSummaryTotal.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void GetData()
        {
            try
            {
                // create datatable to store database data
                DataTable dt = new DataTable();
                string Query = "Select * from SPQR1HSN where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
                Application.DoEvents();

                // get data from database
                dt = objGSTR7.GetDataGSTR1(Query);

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
                    dt.Columns.Remove("Fld_Month");
                    // remove last column (file status)
                    dt.Columns.Remove("Fld_FileStatus");
                    // remove first column (field id)
                    dt.Columns.Remove(dt.Columns[0]);
                    dt.Columns.RemoveAt(dt.Columns.Count - 1);
                    // add column (chek box)
                    dt.Columns.Add(new DataColumn("colChk"));
                    // set check box column at first index of datatable
                    dt.Columns["colChk"].SetOrdinal(0);

                    // rename datatable column name to datagridview column name
                    foreach (DataGridViewColumn col in dgvGSTR1_HSNSummary.Columns)
                    {
                        dt.Columns[col.Index].ColumnName = col.Name.ToString();
                        col.DataPropertyName = col.Name;
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string ColName = dt.Columns[j].ColumnName;
                            if (ColName == "colTotalValue" || ColName == "colTotalTaxableValue" || ColName == "colIGST" || ColName == "colCGST" || ColName == "colSGST" || ColName == "colCess")
                                dt.Rows[i][j] = Utility.DisplayIndianCurrency(Convert.ToString(dt.Rows[i][j]));
                        }
                    }

                    dt.AcceptChanges();

                    // assign datatable to data grid view
                    dgvGSTR1_HSNSummary.DataSource = dt;
                    Application.DoEvents();

                }
                else
                {
                    ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(0);
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

        public void GetTotal(string[] colNo)
        {
            try
            {
                if (dgvGSTR1_HSNSummary.Rows.Count > 1)
                {
                    // if main grid having records

                    if (dgvGSTR1_HSNSummaryTotal.Rows.Count == 0)
                    {
                        #region if total grid having no record
                        // create temprory datatable to store column calculation
                        DataTable dtTotal = new DataTable();

                        // add column as par datagridview column
                        foreach (DataGridViewColumn col in dgvGSTR1_HSNSummaryTotal.Columns)
                        {
                            dtTotal.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;
                        }

                        // create datarow to store grid column calculation
                        DataRow dr = dtTotal.NewRow();
                        dr["colTTotalQuantity"] = dgvGSTR1_HSNSummary.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colTotalQuantity"].Value != null).Sum(x => x.Cells["colTotalQuantity"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colTotalQuantity"].Value)).ToString();
                        dr["colTTotalValue"] = dgvGSTR1_HSNSummary.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colTotalValue"].Value != null).Sum(x => x.Cells["colTotalValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colTotalValue"].Value)).ToString();
                        dr["colTTotalTaxableValue"] = dgvGSTR1_HSNSummary.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colTotalTaxableValue"].Value != null).Sum(x => x.Cells["colTotalTaxableValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colTotalTaxableValue"].Value)).ToString();
                        dr["colTIGST"] = dgvGSTR1_HSNSummary.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colIGST"].Value != null).Sum(x => x.Cells["colIGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIGST"].Value)).ToString();
                        dr["colTCGST"] = dgvGSTR1_HSNSummary.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCGST"].Value != null).Sum(x => x.Cells["colCGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCGST"].Value)).ToString();
                        dr["colTSGST"] = dgvGSTR1_HSNSummary.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colSGST"].Value != null).Sum(x => x.Cells["colSGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colSGST"].Value)).ToString();
                        dr["colTCess"] = dgvGSTR1_HSNSummary.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCess"].Value != null).Sum(x => x.Cells["colCess"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCess"].Value)).ToString();

                        // add datarow to datatable
                        dtTotal.Rows.Add(dr);

                        for (int i = 0; i < dtTotal.Rows.Count; i++)
                        {
                            for (int j = 0; j < dtTotal.Columns.Count; j++)
                            {
                                string ColName = dtTotal.Columns[j].ColumnName;
                                if (ColName == "colTTotalValue" || ColName == "colTTotalTaxableValue" || ColName == "colTIGST" || ColName == "colTCGST" || ColName == "colTSGST" || ColName == "colTCess")
                                    dtTotal.Rows[i][j] = Utility.DisplayIndianCurrency(Convert.ToString(dtTotal.Rows[i][j]));
                            }
                        }

                        dtTotal.AcceptChanges();

                        // assign datatable to grid
                        dgvGSTR1_HSNSummaryTotal.DataSource = dtTotal;

                        #endregion
                    }
                    else if (dgvGSTR1_HSNSummaryTotal.Rows.Count == 1)
                    {
                        #region if total grid having only one records

                        // calculate total only specific column
                        foreach (var item in colNo)
                        {
                            if (item == "colTotalQuantity")
                                dgvGSTR1_HSNSummaryTotal.Rows[0].Cells["colTTotalQuantity"].Value = dgvGSTR1_HSNSummary.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colTotalQuantity"].Value != null).Sum(x => x.Cells["colTotalQuantity"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colTotalQuantity"].Value)).ToString();
                            else if (item == "colTotalValue")
                                dgvGSTR1_HSNSummaryTotal.Rows[0].Cells["colTTotalValue"].Value = Utility.DisplayIndianCurrency(dgvGSTR1_HSNSummary.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colTotalValue"].Value != null).Sum(x => x.Cells["colTotalValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colTotalValue"].Value)).ToString());
                            else if (item == "colTotalTaxableValue")
                                dgvGSTR1_HSNSummaryTotal.Rows[0].Cells["colTTotalTaxableValue"].Value = Utility.DisplayIndianCurrency(dgvGSTR1_HSNSummary.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colTotalTaxableValue"].Value != null).Sum(x => x.Cells["colTotalTaxableValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colTotalTaxableValue"].Value)).ToString());
                            else if (item == "colIGST")
                                dgvGSTR1_HSNSummaryTotal.Rows[0].Cells["colTIGST"].Value = Utility.DisplayIndianCurrency(dgvGSTR1_HSNSummary.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colIGST"].Value != null).Sum(x => x.Cells["colIGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIGST"].Value)).ToString());
                            else if (item == "colCGST")
                                dgvGSTR1_HSNSummaryTotal.Rows[0].Cells["colTCGST"].Value = Utility.DisplayIndianCurrency(dgvGSTR1_HSNSummary.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCGST"].Value != null).Sum(x => x.Cells["colCGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCGST"].Value)).ToString());
                            else if (item == "colSGST")
                                dgvGSTR1_HSNSummaryTotal.Rows[0].Cells["colTSGST"].Value = Utility.DisplayIndianCurrency(dgvGSTR1_HSNSummary.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colSGST"].Value != null).Sum(x => x.Cells["colSGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colSGST"].Value)).ToString());
                            else if (item == "colCess")
                                dgvGSTR1_HSNSummaryTotal.Rows[0].Cells["colTCess"].Value = Utility.DisplayIndianCurrency(dgvGSTR1_HSNSummary.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCess"].Value != null).Sum(x => x.Cells["colCess"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCess"].Value)).ToString());
                        }
                        #endregion
                    }

                    // set grid row height and assign total header
                    dgvGSTR1_HSNSummaryTotal.Rows[0].Height = 30;
                    dgvGSTR1_HSNSummaryTotal.Rows[0].Cells[0].Value = "TOTAL";
                }
                else
                {
                    // check if total grid having record

                    if (dgvGSTR1_HSNSummaryTotal.Rows.Count >= 0)
                    {
                        #region if there are no records in main grid then assign blank datatable to total grid
                        DataTable dtTotal = new DataTable();
                        foreach (DataGridViewColumn col in dgvGSTR1_HSNSummaryTotal.Columns)
                        {
                            dtTotal.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;
                        }
                        dgvGSTR1_HSNSummaryTotal.DataSource = dtTotal;
                        #endregion
                    }
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

        #region Filter
        private void BindFilter()
        {
            try
            {
                List<colList> lstColumns = new List<colList>();
                for (int i = 0; i < dgvGSTR1_HSNSummary.Columns.Count; i++)
                {
                    if (i > 1)
                    {
                        string HeaderText = dgvGSTR1_HSNSummary.Columns[i].HeaderText;
                        string Name = dgvGSTR1_HSNSummary.Columns[i].Name;
                        lstColumns.Add(new colList { colHeaderText = HeaderText, colName = Name });
                        //cmbFilter.Items.Add(HeaderText);
                    }
                    else if (i == 0)
                    {
                        lstColumns.Add(new colList { colHeaderText = "", colName = "" });
                    }
                }
                cmbFilter.DataSource = lstColumns;
                cmbFilter.DisplayMember = "colHeaderText";
                cmbFilter.ValueMember = "colName";
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

        class colList
        {
            public string colHeaderText { get; set; }

            public string colName { get; set; }
        }
        #endregion

        private void dgvGSTR1_HSNSummary_KeyDown(object sender, KeyEventArgs e)
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
                        if (dgvGSTR1_HSNSummary.Rows.Count > 0)
                        {
                            // delete selected cell in grid
                            foreach (DataGridViewCell oneCell in dgvGSTR1_HSNSummary.SelectedCells)
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
                    string[] colNo = { "colTotalQuantity", "colTotalValue", "colTotalTaxableValue", "colIGST", "colCGST", "colSGST", "colCess" };
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
                    if (dgvGSTR1_HSNSummary.RowCount > 0) // IF GRID IS NOT NULL AND PAST ONLY ON SELECTED CELLS
                    {
                        foreach (DataGridViewCell oneCell in dgvGSTR1_HSNSummary.SelectedCells)
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
                            DisableControls(dgvGSTR1_HSNSummary);

                            gRowNo = dgvGSTR1_HSNSummary.Rows.Count - 1;
                            int no = tmp;

                            if (iRow > gRowNo - 1)
                            {
                                DataTable dtDGV = new DataTable();
                                //dtDGV = dgvGSTR1_HSNSummary.DataSource as DataTable;

                                #region ADD DATATABLE COLUMN

                                // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                                foreach (DataGridViewColumn col in dgvGSTR1_HSNSummary.Columns)
                                {
                                    dtDGV.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }
                                #endregion

                                #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                                // CREATE OBJECT ARRAY TO STORE ONE ROW DATA OF MAIN GRID
                                object[] rowValue = new object[dtDGV.Columns.Count];

                                foreach (DataGridViewRow dr in dgvGSTR1_HSNSummary.Rows)
                                {
                                    if (dr.Index != dgvGSTR1_HSNSummary.Rows.Count - 1) // DON'T ADD LAST ROW
                                    {
                                        // SET CHECK BOX VALUE
                                        rowValue[0] = "False";
                                        for (int i = 1; i < dr.Cells.Count; i++)
                                        {
                                            if (dgvGSTR1_HSNSummary.Columns[dr.Index].Name == "colUQC")
                                            {
                                                if (chkQOC(Convert.ToString(dr.Cells[i].Value)))
                                                    rowValue[i] = Convert.ToString(dr.Cells[i].Value);
                                            }
                                            else
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
                                        if (iCol + i < this.dgvGSTR1_HSNSummary.ColumnCount && i < 10)
                                        {
                                            // skip check box column and sequance column to paste data
                                            if (iCol == 0)
                                                oCell = dgvGSTR1_HSNSummary[iCol + i + 2, iRow];
                                            else if (iCol == 1)
                                                oCell = dgvGSTR1_HSNSummary[iCol + i + 1, iRow];
                                            else
                                                oCell = dgvGSTR1_HSNSummary[iCol + i, iRow];

                                            sCells[i] = sCells[i].Trim().Replace(",", "");
                                            if (oCell.ColumnIndex != 0)
                                            {
                                                if (dgvGSTR1_HSNSummary.Columns[oCell.ColumnIndex].Name != "colChk" && dgvGSTR1_HSNSummary.Columns[oCell.ColumnIndex].Name != "colSequence")
                                                {
                                                    #region VALIDATION
                                                    if (sCells[i].ToString().Trim() == "") { dgvGSTR1_HSNSummary.Rows[iRow].Cells[oCell.ColumnIndex].Value = DBNull.Value; }
                                                    else
                                                    {
                                                        if (oCell.ColumnIndex >= 2 && oCell.ColumnIndex <= 11)
                                                        {
                                                            if (chkCellValue(sCells[i].Trim(), dgvGSTR1_HSNSummary.Columns[oCell.ColumnIndex].Name))
                                                            {
                                                                if (dgvGSTR1_HSNSummary.Columns[oCell.ColumnIndex].Name == "colUQC")
                                                                    dgvGSTR1_HSNSummary.Rows[iRow].Cells[oCell.ColumnIndex].Value = Utility.StrHNSUQC(sCells[i]);
                                                                else
                                                                    dgvGSTR1_HSNSummary.Rows[iRow].Cells[oCell.ColumnIndex].Value = sCells[i].Trim();
                                                            }
                                                            else
                                                            {
                                                                dgvGSTR1_HSNSummary.Rows[iRow].Cells[oCell.ColumnIndex].Value = "";
                                                            }
                                                        }
                                                        else { dgvGSTR1_HSNSummary.Rows[iRow].Cells[oCell.ColumnIndex].Value = sCells[i].Trim(); }
                                                    }
                                                    #endregion
                                                }
                                            }
                                            else
                                            {
                                                if (iCol > i)
                                                {
                                                    for (int j = oCell.ColumnIndex; j < dgvGSTR1_HSNSummary.Columns.Count; j++)
                                                    {
                                                        #region VALIDATION
                                                        if (sCells[i].ToString().Trim() == "") { dgvGSTR1_HSNSummary.Rows[iRow].Cells[j].Value = DBNull.Value; }
                                                        else
                                                        {
                                                            if (j >= 2 && j <= 11)
                                                            {
                                                                if (chkCellValue(sCells[i].Trim(), dgvGSTR1_HSNSummary.Columns[j].Name))
                                                                {
                                                                    if (dgvGSTR1_HSNSummary.Columns[j].Name == "colUQC")
                                                                        dgvGSTR1_HSNSummary.Rows[iRow].Cells[j].Value = Utility.StrHNSUQC(sCells[i]);
                                                                    else
                                                                        dgvGSTR1_HSNSummary.Rows[iRow].Cells[j].Value = sCells[i].Trim();
                                                                }
                                                                else
                                                                    dgvGSTR1_HSNSummary.Rows[iRow].Cells[j].Value = "";
                                                            }
                                                            else { dgvGSTR1_HSNSummary.Rows[iRow].Cells[j].Value = sCells[i].Trim(); }
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
                                                    for (int j = oCell.ColumnIndex; j < dgvGSTR1_HSNSummary.Columns.Count; j++)
                                                    {
                                                        #region VALIDATION
                                                        if (sCells[i].ToString().Trim() == "") { dgvGSTR1_HSNSummary.Rows[iRow].Cells[j].Value = DBNull.Value; }
                                                        else
                                                        {
                                                            if (j >= 2 && j <= 11)
                                                            {
                                                                if (chkCellValue(sCells[i].Trim(), dgvGSTR1_HSNSummary.Columns[j].Name))
                                                                {
                                                                    if (dgvGSTR1_HSNSummary.Columns[j].Name == "colUQC")
                                                                        dgvGSTR1_HSNSummary.Rows[iRow].Cells[j].Value = Utility.StrHNSUQC(sCells[i]);
                                                                    else
                                                                        dgvGSTR1_HSNSummary.Rows[iRow].Cells[j].Value = sCells[i].Trim();
                                                                }
                                                                else
                                                                    dgvGSTR1_HSNSummary.Rows[iRow].Cells[j].Value = "";
                                                            }
                                                            else { dgvGSTR1_HSNSummary.Rows[iRow].Cells[j].Value = sCells[i].Trim(); }
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

                                    //Application.DoEvents();
                                }
                            }
                        }
                        tmp++;
                    }
                    #endregion

                    // enabal main grid
                    EnableControls(dgvGSTR1_HSNSummary);
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
                EnableControls(dgvGSTR1_HSNSummary);
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
                DisableControls(dgvGSTR1_HSNSummary);

                #region Set datatable
                int cnt = 0, colNo = 0;

                // assign grid data to datatable
                DataTable dt = dtDGV;

                if (dt == null)
                {
                    // if no record in grid then create new daatable
                    dt = new DataTable();

                    // add column as par main grid and set data access property
                    foreach (DataGridViewColumn col in dgvGSTR1_HSNSummary.Columns)
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
                                if (iCol + i < this.dgvGSTR1_HSNSummary.ColumnCount && colNo < 11)
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
                                            if (sCells[i].ToString().Trim() == "" && dgvGSTR1_HSNSummary.Columns[colNo].Name != "colTax") { dt.Rows[dt.Rows.Count - 1][colNo] = DBNull.Value; }
                                            else
                                            {
                                                if (colNo >= 2 && colNo <= 11)
                                                {
                                                    if (chkCellValue(sCells[i].Trim(), dgvGSTR1_HSNSummary.Columns[colNo].Name))
                                                    {
                                                        if (dgvGSTR1_HSNSummary.Columns[colNo].Name == "colUQC")
                                                            dt.Rows[dt.Rows.Count - 1][colNo] = Utility.StrHNSUQC(sCells[i]);
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
                                            for (int j = colNo; j < dgvGSTR1_HSNSummary.Columns.Count; j++)
                                            {
                                                #region VALIDATION
                                                if (sCells[i].ToString().Trim() == "") { dt.Rows[dt.Rows.Count - 1][j] = DBNull.Value; }
                                                else
                                                {
                                                    if (j >= 2 && j <= 11)
                                                    {
                                                        if (chkCellValue(sCells[i].Trim(), dgvGSTR1_HSNSummary.Columns[j].Name))
                                                        {
                                                            if (dgvGSTR1_HSNSummary.Columns[j].Name == "colUQC")
                                                                dt.Rows[dt.Rows.Count - 1][j] = Utility.StrHNSUQC(sCells[i]);
                                                            else
                                                                dt.Rows[dt.Rows.Count - 1][j] = sCells[i].Trim();
                                                        }
                                                        else
                                                            dt.Rows[dt.Rows.Count - 1][j] = "";
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
                                            for (int j = colNo; j < dgvGSTR1_HSNSummary.Columns.Count; j++)
                                            {
                                                #region VALIDATION
                                                if (sCells[i].ToString().Trim() == "") { dt.Rows[dt.Rows.Count - 1][j] = DBNull.Value; }
                                                else
                                                {
                                                    if (j >= 2 && j <= 11)
                                                    {
                                                        if (chkCellValue(sCells[i].Trim(), dgvGSTR1_HSNSummary.Columns[j].Name))
                                                        {
                                                            if (dgvGSTR1_HSNSummary.Columns[j].Name == "colUQC")
                                                                dt.Rows[dt.Rows.Count - 1][j] = Utility.StrHNSUQC(sCells[i]);
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
                            dt.Rows[dt.Rows.Count - 1]["colSequence"] = dt.Rows.Count;
                        }
                    }
                    cnt++;
                }

                #region Export datatable to grid

                // if there are records in data table then assign it to grid
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string ColName = dt.Columns[j].ColumnName;
                            if (ColName == "colTotalValue" || ColName == "colTotalTaxableValue" || ColName == "colIGST" || ColName == "colCGST" || ColName == "colSGST" || ColName == "colCess")
                                dt.Rows[i][j] = Utility.DisplayIndianCurrency(Convert.ToString(dt.Rows[i][j]));
                        }
                    }

                    dgvGSTR1_HSNSummary.DataSource = dt;
                }
                // total calculation

                string[] colGroup = { "colTotalQuantity", "colTotalValue", "colTotalTaxableValue", "colIGST", "colCGST", "colSGST", "colCess" };
                GetTotal(colGroup);

                pbGSTR1.Visible = false;

                EnableControls(dgvGSTR1_HSNSummary);

                #endregion
            }
            catch (Exception ex)
            {
                pbGSTR1.Visible = false;
                EnableControls(dgvGSTR1_HSNSummary);
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
                string _str = "";

                pbGSTR1.Visible = true;
                dgvGSTR1_HSNSummary.CurrentCell = dgvGSTR1_HSNSummary.Rows[0].Cells[0];
                dgvGSTR1_HSNSummary.AllowUserToAddRows = false;

                List<DataGridViewRow> list = null;

                #region HSN
                list = null;
                list = dgvGSTR1_HSNSummary.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Utility.IsHSNCode(Convert.ToString(x.Cells["colHSN"].Value)) == false)
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR1_HSNSummary.Rows[list[i].Cells["colHSN"].RowIndex].Cells["colHSN"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") HSN must be max 8 digit and .(Dot) is not allowed.\n";
                }
                list = dgvGSTR1_HSNSummary.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Utility.IsHSNCode(Convert.ToString(x.Cells["colHSN"].Value)) == true)
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR1_HSNSummary.Rows[list[i].Cells["colHSN"].RowIndex].Cells["colHSN"].Style.BackColor = Color.White;
                }
                #endregion

                #region Description
                list = null;
                list = dgvGSTR1_HSNSummary.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Utility.IsHsnDescription(Convert.ToString(x.Cells["colDesciption"].Value)) == false)
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR1_HSNSummary.Rows[list[i].Cells["colDesciption"].RowIndex].Cells["colDesciption"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter description up to 30 character.\n";
                }
                list = dgvGSTR1_HSNSummary.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Utility.IsHsnDescription(Convert.ToString(x.Cells["colDesciption"].Value)) == true)
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR1_HSNSummary.Rows[list[i].Cells["colDesciption"].RowIndex].Cells["colDesciption"].Style.BackColor = Color.White;
                }
                #endregion

                #region UQC Validation
                list = null;
                list = dgvGSTR1_HSNSummary.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Utility.HNSUQC(Convert.ToString(x.Cells["colUQC"].Value)) == false)
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR1_HSNSummary.Rows[list[i].Cells["colUQC"].RowIndex].Cells["colUQC"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please select proper UQC value from the drop-down.\n";
                }
                list = dgvGSTR1_HSNSummary.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Convert.ToString(x.Cells["colUQC"].Value) != "" && Utility.HNSUQC(Convert.ToString(x.Cells["colUQC"].Value)) == true)
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR1_HSNSummary.Rows[list[i].Cells["colUQC"].RowIndex].Cells["colUQC"].Style.BackColor = Color.White;
                }
                #endregion

                #region  Same HSN Same Description Same UQC

                DataTable dt = (DataTable)dgvGSTR1_HSNSummary.DataSource;
                var result = dt.AsEnumerable()
                           .GroupBy(row => new
                           {
                               colHSN = row.Field<string>("colHSN"),
                               colDesciption = row.Field<string>("colDesciption"),
                               colUQC = row.Field<string>("colUQC")
                           })
                           .Where(gr => gr.Count() > 1)
                           .Select(g => g.CopyToDataTable()).ToList();


                foreach (var item in result)
                {
                    #region Same HSN, UQC Same Description 
                    if (item != null && item.Rows.Count > 0)
                    {
                        // list = dgvGSTR1_HSNSummary.Rows
                        //.OfType<DataGridViewRow>()
                        //.Where(x => Convert.ToString(x.Cells["colHSN"].Value) == Convert.ToString(item.Rows[0]["colHSN"]).Trim() 
                        //    && Convert.ToString(x.Cells["colDesciption"]).Trim() == Convert.ToString(item.Rows[0]["colDesciption"]).Trim() 
                        //    && Convert.ToString(x.Cells["colUQC"]).Trim() == Convert.ToString(item.Rows[0]["colUQC"]).Trim())
                        //.Select(x => x)
                        //.ToList();
                        for (int j = 0; j < item.Rows.Count; j++)
                        {
                            list = dgvGSTR1_HSNSummary.Rows
                           .OfType<DataGridViewRow>()
                           .Where(x => Convert.ToString(x.Cells["colSequence"].Value) == Convert.ToString(item.Rows[j]["colSequence"]).Trim())
                           .Select(x => x)
                           .ToList();
                            if (list.Count > 0)
                            {
                                for (int i = 0; i < list.Count; i++)
                                {
                                    dgvGSTR1_HSNSummary.Rows[list[i].Cells["colDesciption"].RowIndex].Cells["colDesciption"].Style.BackColor = Color.Red;
                                    _cnt += 1;
                                    _str += _cnt + ") Same description for same HSN and same UQC is not allowed.\n";
                                }
                            }
                            else
                            {
                                dgvGSTR1_HSNSummary.Rows[list[j].Cells["colDesciption"].RowIndex].Cells["colDesciption"].Style.BackColor = Color.White;
                            }
                        }
                    }
                    #endregion
                }

                #region  Same HSN & Description Only
                /*
                DataTable dt = (DataTable)dgvGSTR1_HSNSummary.DataSource;
                var result = dt.AsEnumerable()
                             .GroupBy(r => r["colHSN"])//Using Column Name
                             .Where(gr => gr.Count() > 1)
                             .Select(g => g.Key);


                foreach (var item in result)
                {
                    #region Same HSN Same Description
                    if (Convert.ToString(item).Trim() != "")
                    {
                        list = dgvGSTR1_HSNSummary.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Convert.ToString(x.Cells["colHSN"].Value) == Convert.ToString(item).Trim() && Convert.ToString(x.Cells["colDesciption"]).Trim() != "")
                       .Select(x => x)
                       .ToList();
                        if (list.Count > 0)
                        {
                            for (int i = 0; i < list.Count; i++)
                            {
                                if ( Convert.ToString(dgvGSTR1_HSNSummary.Rows[list[0].Cells["colDesciption"].RowIndex].Cells["colDesciption"].Value).Trim() == Convert.ToString(dgvGSTR1_HSNSummary.Rows[list[i].Cells["colDesciption"].RowIndex].Cells["colDesciption"].Value).Trim())
                                {
                                    if (i != 0)
                                    {
                                        dgvGSTR1_HSNSummary.Rows[list[i].Cells["colDesciption"].RowIndex].Cells["colDesciption"].Style.BackColor = Color.Red;
                                        _cnt += 1;
                                        _str += _cnt + ") Same description for same HSN is not allowed.\n";
                                    }
                                }
                                else
                                    dgvGSTR1_HSNSummary.Rows[list[i].Cells["colDesciption"].RowIndex].Cells["colDesciption"].Style.BackColor = Color.White;
                            }
                        }
                    }
                    #endregion
                }
                */
                #endregion

                #region Old
                //var result = (from row in dt.AsEnumerable()
                //              group row by new { colDesciption = row.Field<string>("colDesciption"), colHSN = row.Field<string>("colHSN"), colUQC = row.Field<string>("colUQC") } into grp
                //              select new
                //              {
                //                  colDesciption = grp.Key.colDesciption,
                //                  colHSN = grp.Key.colHSN,
                //                  colUQC = grp.Key.colUQC,
                //              }).ToList();

                //if (result != null && result.Count > 0)
                //{
                //    foreach (var item in result)
                //    {
                //        #region Same HSN Same Description Same UQC
                //        list = dgvGSTR1_HSNSummary.Rows
                //                .OfType<DataGridViewRow>()
                //                .Where(x => Convert.ToString(x.Cells["colDesciption"].Value) == Convert.ToString(item.colDesciption) && Convert.ToString(x.Cells["colHSN"].Value) == Convert.ToString(item.colHSN) && Convert.ToString(x.Cells["colUQC"].Value) == Convert.ToString(item.colUQC))
                //                .Select(p => p)
                //                .ToList();

                //        if (list != null && list.Count > 1)
                //        {
                //            for (int i = 0; i < list.Count; i++)
                //            {
                //                if (dgvGSTR1_HSNSummary.Rows[list[i].Cells["colDesciption"].RowIndex].Cells["colDesciption"].Value != "")
                //                    dgvGSTR1_HSNSummary.Rows[list[i].Cells["colDesciption"].RowIndex].Cells["colDesciption"].Style.BackColor = Color.Red;
                //                if (dgvGSTR1_HSNSummary.Rows[list[i].Cells["colUQC"].RowIndex].Cells["colUQC"].Value != "")
                //                    dgvGSTR1_HSNSummary.Rows[list[i].Cells["colUQC"].RowIndex].Cells["colUQC"].Style.BackColor = Color.Red;
                //            }
                //            _cnt += 1;
                //            _str += _cnt + ") Same description and same UQC for same HSN is not allowed.\n";
                //        }
                //        #endregion


                //        #region Same HSN Same Description
                //        //list = dgvGSTR1_HSNSummary.Rows
                //        //        .OfType<DataGridViewRow>()
                //        //        .Where(x => Convert.ToString(x.Cells["colDesciption"].Value) == Convert.ToString(item.colDesciption) && Convert.ToString(x.Cells["colHSN"].Value) == Convert.ToString(item.colHSN))
                //        //        .Select(p => p)
                //        //        .ToList();

                //        //if (list!=null && list.Count>1)
                //        //{
                //        //    for (int i = 0; i < list.Count; i++)
                //        //    {
                //        //        dgvGSTR1_HSNSummary.Rows[list[i].Cells["colDesciption"].RowIndex].Cells["colDesciption"].Style.BackColor = Color.Red;
                //        //    }
                //        //    _cnt += 1;
                //        //    _str += _cnt + ") Same description for same HSN is not allowed.\n";
                //        //}
                //        #endregion

                //        #region Same HSN Same UQC
                //        //list = dgvGSTR1_HSNSummary.Rows
                //        //        .OfType<DataGridViewRow>()
                //        //        .Where(x => Convert.ToString(x.Cells["colUQC"].Value) == Convert.ToString(item.colUQC) && Convert.ToString(x.Cells["colHSN"].Value) == Convert.ToString(item.colHSN))
                //        //        .Select(p => p)
                //        //        .ToList();

                //        //if (list != null && list.Count > 1)
                //        //{
                //        //    for (int i = 0; i < list.Count; i++)
                //        //    {
                //        //        dgvGSTR1_HSNSummary.Rows[list[i].Cells["colUQC"].RowIndex].Cells["colUQC"].Style.BackColor = Color.Red;
                //        //    }
                //        //    _cnt += 1;
                //        //    _str += _cnt + ") Same UQC for same HSN is not allowed.\n";
                //        //}
                //        #endregion
                //    }
                //}
                #endregion
                #endregion

                #region HSN = "" & Description = ""
                list = null;
                list = dgvGSTR1_HSNSummary.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Utility.IsHSNCode(Convert.ToString(x.Cells["colHSN"].Value)) == false && Convert.ToString(x.Cells["colDesciption"].Value) == "")
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (dgvGSTR1_HSNSummary.Rows[list[i].Cells["colHSN"].RowIndex].Cells["colHSN"].Style.BackColor != Color.Red)
                        dgvGSTR1_HSNSummary.Rows[list[i].Cells["colHSN"].RowIndex].Cells["colHSN"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please eneter either HSN or Description.\n";
                }
                list = dgvGSTR1_HSNSummary.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Utility.IsHSNCode(Convert.ToString(x.Cells["colHSN"].Value)) == true || Convert.ToString(x.Cells["colDesciption"].Value) != "")
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    if(dgvGSTR1_HSNSummary.Rows[list[i].Cells["colHSN"].RowIndex].Cells["colHSN"].Style.BackColor != Color.Red)
                    dgvGSTR1_HSNSummary.Rows[list[i].Cells["colHSN"].RowIndex].Cells["colHSN"].Style.BackColor = Color.White;
                }
                #endregion

                #region Total Quantity
                list = null;
                list = dgvGSTR1_HSNSummary.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsInvoiceValue(Convert.ToString(x.Cells["colTotalQuantity"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR1_HSNSummary.Rows[list[i].Cells["colTotalQuantity"].RowIndex].Cells["colTotalQuantity"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper Total Quantity value.\n";
                }
                list = dgvGSTR1_HSNSummary.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsInvoiceValue(Convert.ToString(x.Cells["colTotalQuantity"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR1_HSNSummary.Rows[list[i].Cells["colTotalQuantity"].RowIndex].Cells["colTotalQuantity"].Style.BackColor = Color.White;
                }
                #endregion

                #region Total Value
                list = null;
                list = dgvGSTR1_HSNSummary.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsPMInvoiceValue(Convert.ToString(x.Cells["colTotalValue"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR1_HSNSummary.Rows[list[i].Cells["colTotalValue"].RowIndex].Cells["colTotalValue"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper Total Value.\n";
                }
                list = dgvGSTR1_HSNSummary.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsPMInvoiceValue(Convert.ToString(x.Cells["colTotalValue"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR1_HSNSummary.Rows[list[i].Cells["colTotalValue"].RowIndex].Cells["colTotalValue"].Style.BackColor = Color.White;
                }
                #endregion

                #region Total Taxable Value
                list = null;
                list = dgvGSTR1_HSNSummary.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsPMTaxableValue(Convert.ToString(x.Cells["colTotalTaxableValue"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR1_HSNSummary.Rows[list[i].Cells["colTotalTaxableValue"].RowIndex].Cells["colTotalTaxableValue"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper Total Taxable Value.\n";
                }
                list = dgvGSTR1_HSNSummary.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsPMTaxableValue(Convert.ToString(x.Cells["colTotalTaxableValue"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR1_HSNSummary.Rows[list[i].Cells["colTotalTaxableValue"].RowIndex].Cells["colTotalTaxableValue"].Style.BackColor = Color.White;
                }
                #endregion

                #region IGST Amount
                list = null;
                list = dgvGSTR1_HSNSummary.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsPMBlankICSC(Convert.ToString(x.Cells["colIGST"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR1_HSNSummary.Rows[list[i].Cells["colIGST"].RowIndex].Cells["colIGST"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper IGST Amount.\n";
                }
                list = dgvGSTR1_HSNSummary.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsPMBlankICSC(Convert.ToString(x.Cells["colIGST"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR1_HSNSummary.Rows[list[i].Cells["colIGST"].RowIndex].Cells["colIGST"].Style.BackColor = Color.White;
                }
                #endregion

                #region CGST Amount
                list = null;
                list = dgvGSTR1_HSNSummary.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsPMBlankICSC(Convert.ToString(x.Cells["colCGST"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR1_HSNSummary.Rows[list[i].Cells["colCGST"].RowIndex].Cells["colCGST"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper CGST Amount.\n";
                }
                list = dgvGSTR1_HSNSummary.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsPMBlankICSC(Convert.ToString(x.Cells["colCGST"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR1_HSNSummary.Rows[list[i].Cells["colCGST"].RowIndex].Cells["colCGST"].Style.BackColor = Color.White;
                }
                #endregion

                #region SGST Amount
                list = null;
                list = dgvGSTR1_HSNSummary.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsPMBlankICSC(Convert.ToString(x.Cells["colSGST"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR1_HSNSummary.Rows[list[i].Cells["colSGST"].RowIndex].Cells["colSGST"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper SGST Amount.\n";
                }
                list = dgvGSTR1_HSNSummary.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsPMBlankICSC(Convert.ToString(x.Cells["colSGST"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR1_HSNSummary.Rows[list[i].Cells["colSGST"].RowIndex].Cells["colSGST"].Style.BackColor = Color.White;
                }
                #endregion

                #region Cess Amount
                list = null;
                list = dgvGSTR1_HSNSummary.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsPMBlankICSC(Convert.ToString(x.Cells["colCess"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR1_HSNSummary.Rows[list[i].Cells["colCess"].RowIndex].Cells["colCess"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper Cess Amount.\n";
                }
                list = dgvGSTR1_HSNSummary.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsPMBlankICSC(Convert.ToString(x.Cells["colCess"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR1_HSNSummary.Rows[list[i].Cells["colCess"].RowIndex].Cells["colCess"].Style.BackColor = Color.White;
                }
                #endregion

                pbGSTR1.Visible = false;
                dgvGSTR1_HSNSummary.AllowUserToAddRows = true;

                if (_str != "")
                {
                    int _Result = objGSTR7.InsertValidationFlg("GSTR1", "HSN", "false", CommonHelper.SelectedMonth);
                    if (_Result != 1)
                        MessageBox.Show("System error.\nPlease try after sometime! SPQValidation Error", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    DialogResult dialogResult = MessageBox.Show("File Not Validated. Do you want error description in excel?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    if (dialogResult == DialogResult.Yes)
                        ExportExcelForValidatation();

                    return false;
                }
                else
                {
                    int _Result = objGSTR7.InsertValidationFlg("GSTR1", "HSN", "true", CommonHelper.SelectedMonth);
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
                dgvGSTR1_HSNSummary.AllowUserToAddRows = true;
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
                    if (cNo == "colTotalQuantity" || cNo == "colTotalValue" || cNo == "colTotalTaxableValue" || cNo == "colIGST" || cNo == "colCGST" || cNo == "colSGST" || cNo == "colCess") // value
                    {
                        if (Utility.IsPMDecimalOrNumber(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colDesciption")
                    {
                        if (cellValue.Length < 31)
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colUQC") // QOC
                    {
                        if (Utility.HNSUQC(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colHSN") // HSN
                    {
                        if (Utility.IsHSNCode(cellValue))
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
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
                return false;
            }
        }

        private void dgvGSTR1_HSNSummary_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string cNo = dgvGSTR1_HSNSummary.Columns[e.ColumnIndex].Name;

                if (e.RowIndex >= 0)
                {
                    if (cNo == "colTotalQuantity" || cNo == "colTotalValue" || cNo == "colTotalTaxableValue" || cNo == "colIGST" || cNo == "colCGST" || cNo == "colSGST" || cNo == "colCess") // value
                    {
                        if (chkCellValue(Convert.ToString(dgvGSTR1_HSNSummary.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                        {
                            if (cNo == "colTotalQuantity" || cNo == "colTotalValue" || cNo == "colTotalTaxableValue" || cNo == "colIGST" || cNo == "colCGST" || cNo == "colSGST" || cNo == "colCess")
                            {
                                if (Convert.ToString(dgvGSTR1_HSNSummary.Rows[e.RowIndex].Cells[cNo].Value).Trim() != "")
                                {
                                    dgvGSTR1_HSNSummary.CellValueChanged -= dgvGSTR1_HSNSummary_CellValueChanged;
                                    dgvGSTR1_HSNSummary.Rows[e.RowIndex].Cells[cNo].Value = Utility.DisplayIndianCurrency(Convert.ToString(Math.Round(Convert.ToDecimal(dgvGSTR1_HSNSummary.Rows[e.RowIndex].Cells[cNo].Value), 2, MidpointRounding.AwayFromZero)));
                                    dgvGSTR1_HSNSummary.CellValueChanged += dgvGSTR1_HSNSummary_CellValueChanged;
                                }
                            }

                            string[] colNo = { (dgvGSTR1_HSNSummary.Columns[e.ColumnIndex].Name) };
                            GetTotal(colNo);
                        }
                        else { dgvGSTR1_HSNSummary.Rows[e.RowIndex].Cells[cNo].Value = ""; }
                    }
                    else if (cNo == "colUQC") // QOC
                    {
                        if (!chkCellValue(Convert.ToString(dgvGSTR1_HSNSummary.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                            dgvGSTR1_HSNSummary.Rows[e.RowIndex].Cells[cNo].Value = Utility.StrHNSUQC(Convert.ToString(dgvGSTR1_HSNSummary.Rows[e.RowIndex].Cells[cNo].Value));
                    }
                    else if (cNo == "colDesciption") // Description
                    {
                        if (!chkCellValue(Convert.ToString(dgvGSTR1_HSNSummary.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                            dgvGSTR1_HSNSummary.Rows[e.RowIndex].Cells[cNo].Value = "";
                    }
                    else if (cNo == "colHSN") // HSN
                    {
                        if (!chkCellValue(Convert.ToString(dgvGSTR1_HSNSummary.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                            dgvGSTR1_HSNSummary.Rows[e.RowIndex].Cells[cNo].Value = "";
                    }
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

        public void Save()
        {
            try
            {
                pbGSTR1.Visible = true;

                //if (CommonHelper.StatusIndex == 0)
                //{
                //    pbGSTR1.Visible = false;
                //    MessageBox.Show("Please Select File Status!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}

                #region ADD DATATABLE COLUMN

                // create datatable to store main grid data
                DataTable dt = new DataTable();

                // add datatble collumn as par main  grid column
                foreach (DataGridViewColumn col in dgvGSTR1_HSNSummary.Columns)
                {
                    dt.Columns.Add(col.Name.ToString());
                }

                // add datatable column to store file status
                dt.Columns.Add("colFileStatus");

                #endregion

                #region ASSIGN GRIDVIEW ROWS IN DATATABLE
                // create object array to store one row data of main grid
                object[] rowValue = new object[dt.Columns.Count];

                foreach (DataGridViewRow dr in dgvGSTR1_HSNSummary.Rows)
                {
                    if (dr.Index != dgvGSTR1_HSNSummary.Rows.Count - 1) // DON'T ADD LAST ROW
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

                // remove first columm (field id)
                dt.Columns.Remove(dt.Columns[0]);
                dt.AcceptChanges();
                #endregion

                #region RECORD SAVE
                string Query = "";
                int _Result = 0;

                // check there are records in grid
                if (dt != null && dt.Rows.Count > 0)
                {
                    #region first delete old data from database
                    Query = "Delete from SPQR1HSN where Fld_Month='" + CommonHelper.SelectedMonth + "'  AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                    _Result = objGSTR7.IUDData(Query);
                    if (_Result != 1)
                    {
                        // error occurs while deleting data
                        pbGSTR1.Visible = false;
                        MessageBox.Show("System error.\nPlease try after sometime!", "System error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    #endregion

                    // query fire to save records to database
                    _Result = objGSTR7.GSTR1_HSNSummaryBulkEntry(dt, Convert.ToString(CommonHelper.StatusText));

                    if (_Result == 1)
                    {
                        // TOTAL CALCULATION
                        string[] colNo = { "colTotalQuantity", "colTotalValue", "colTotalTaxableValue", "colIGST", "colCGST", "colSGST", "colCess" };
                        GetTotal(colNo);

                        #region ADD DATATABLE COLUMN
                        dt = new DataTable();

                        foreach (DataGridViewColumn col in dgvGSTR1_HSNSummary.Columns)
                        {
                            dt.Columns.Add(col.Name.ToString());
                        }
                        dt.Columns.Add("colFileStatus");
                        #endregion

                        #region ASSIGN GRIDVIEW ROWS IN DATATABLE
                        object[] rowVal = new object[dt.Columns.Count];

                        if (dgvGSTR1_HSNSummaryTotal.Rows.Count == 1)
                        {
                            foreach (DataGridViewRow dr in dgvGSTR1_HSNSummaryTotal.Rows)
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

                        _Result = objGSTR7.GSTR1_HSNSummaryBulkEntry(dt, "Total");
                        if (_Result == 1)
                        {
                            //DONE                            
                            pbGSTR1.Visible = false;
                            MessageBox.Show("Data Saved Successfully …!!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // BIND DATA
                            GetData();
                        }
                        else
                        {
                            // IF ERRORS OCCURS WHILE INSERTING DATA TO DATABASE
                            pbGSTR1.Visible = false;
                            MessageBox.Show("System error.\nPlease try after sometime!", "System error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        // if errors occurs while inserting data to database                        
                        pbGSTR1.Visible = false;
                        MessageBox.Show("System error.\nPlease try after sometime!", "System error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    #region delete all old record if there are no records present in grid
                    Query = "Delete from SPQR1HSN where Fld_Month='" + CommonHelper.SelectedMonth + "'  AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";

                    // fire queary to delete records
                    _Result = objGSTR7.IUDData(Query);

                    if (_Result == 1)
                    {
                        pbGSTR1.Visible = false;

                        // if records deleted from database
                        MessageBox.Show("Record Successfully Deleted!");

                        // make file status blank
                        ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(0);

                        // total calculation
                        string[] colNo = { "colTotalQuantity", "colTotalValue", "colTotalTaxableValue", "colIGST", "colCGST", "colSGST", "colCess" };
                        GetTotal(colNo);
                    }
                    else
                    {
                        // if errors ocurs while deleting record from the database
                        pbGSTR1.Visible = false;
                        MessageBox.Show("System error.\nPlease try after sometime!", "System error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    #endregion
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

        public void Delete()
        {
            try
            {
                if (dgvGSTR1_HSNSummary.Rows.Count == 1)
                {
                    ckboxHeader.Checked = false;
                    return;
                }
                if (dgvGSTR1_HSNSummary.CurrentCell.RowIndex == 0 && dgvGSTR1_HSNSummary.CurrentCell.ColumnIndex == 0)
                {
                    dgvGSTR1_HSNSummary.CurrentCell = dgvGSTR1_HSNSummary.Rows[0].Cells[1];
                }
                else { dgvGSTR1_HSNSummary.CurrentCell = dgvGSTR1_HSNSummary.Rows[0].Cells[0]; }

                // create falg fro delete rows
                Boolean flgChk = false; Boolean flgSelect = false;

                // create grid row object of selected row to delete
                List<DataGridViewRow> toDelete = new List<DataGridViewRow>();

                // check there are record present in grid
                if (dgvGSTR1_HSNSummary.Rows.Count > 1)
                {
                    // flg true if check all selected
                    if (ckboxHeader.Checked)
                        flgChk = true;

                    #region add selected row to object for delete
                    for (int i = 0; i < dgvGSTR1_HSNSummary.Rows.Count - 1; i++)
                    {
                        if (dgvGSTR1_HSNSummary[0, i].Value != null && dgvGSTR1_HSNSummary[0, i].Value.ToString() != "")
                        {
                            if (Convert.ToBoolean(dgvGSTR1_HSNSummary[0, i].Value) == true)
                            {
                                // add row to object if row is selected
                                flgSelect = true;
                                toDelete.Add(dgvGSTR1_HSNSummary.Rows[i]);
                            }
                        }
                    }
                    #endregion

                    // check row is selected to delete
                    if (flgChk || flgSelect)
                    {
                        // open dialog for the confirmation
                        DialogResult result = MessageBox.Show("Do you want to delete this data?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        // if user confirm for deleting records
                        if (result == DialogResult.Yes)
                        {
                            pbGSTR1.Visible = true;

                            if (flgChk)
                            {
                                // if check box of check all is selected
                                flgChk = false;

                                // create datatable and add column as par main grid
                                DataTable dt = new DataTable();
                                foreach (DataGridViewColumn col in dgvGSTR1_HSNSummary.Columns)
                                {
                                    dt.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }

                                // assign blank datatable to grid
                                dgvGSTR1_HSNSummary.DataSource = dt;
                            }
                            else if (flgSelect == true)
                            {
                                // delete selected row
                                foreach (DataGridViewRow row in toDelete)
                                {
                                    dgvGSTR1_HSNSummary.Rows.RemoveAt(row.Index);
                                    Application.DoEvents();
                                }
                            }

                            // sequancing main grid records
                            for (int i = 0; i < dgvGSTR1_HSNSummary.Rows.Count - 1; i++)
                            {
                                dgvGSTR1_HSNSummary.Rows[i].Cells["colSequence"].Value = Convert.ToString(i + 1);
                            }

                            if (dgvGSTR1_HSNSummary.Rows.Count == 1)
                            {
                                // if there are no records in main grid thene assign blank datatable to total grid
                                DataTable dtTotal = new DataTable();
                                foreach (DataGridViewColumn col in dgvGSTR1_HSNSummaryTotal.Columns)
                                {
                                    dtTotal.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }
                                dgvGSTR1_HSNSummaryTotal.DataSource = dtTotal;
                            }

                            // set control property after row deletion
                            ckboxHeader.Checked = false;
                            dgvGSTR1_HSNSummary.Columns[0].HeaderText = "Check All";
                        }
                    }

                    pbGSTR1.Visible = false;

                    // total calculation
                    string[] colNo = { "colTotalQuantity", "colTotalValue", "colTotalTaxableValue", "colIGST", "colCGST", "colSGST", "colCess" };
                    GetTotal(colNo);
                }
                else
                {
                    // if there are no record to delete
                    ckboxHeader.Checked = false;
                    dgvGSTR1_HSNSummary.Columns[0].HeaderText = "Check All";
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
                        foreach (DataGridViewColumn col in dgvGSTR1_HSNSummary.Columns)
                        {
                            dt.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;
                        }
                        #endregion

                        #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                        // CREATE OBJECT ARRAY TO STORE ONE ROW DATA OF MAIN GRID
                        object[] rowValue = new object[dt.Columns.Count];

                        foreach (DataGridViewRow dr in dgvGSTR1_HSNSummary.Rows)
                        {
                            if (dr.Index != dgvGSTR1_HSNSummary.Rows.Count - 1) // DON'T ADD LAST ROW
                            {
                                // SET CHECK BOX VALUE
                                rowValue[0] = "False";
                                for (int i = 1; i < dr.Cells.Count; i++)
                                {
                                    if (dgvGSTR1_HSNSummary.Columns[dr.Index].Name == "colUQC")
                                    {
                                        if (chkQOC(Convert.ToString(dr.Cells[i].Value)))
                                            rowValue[i] = Convert.ToString(dr.Cells[i].Value);
                                    }
                                    else
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
                                DisableControls(dgvGSTR1_HSNSummary);

                                #region IMPORT EXCEL DATATABLE TO GRID DATATABLE
                                if (dtExcel != null && dtExcel.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dt.Columns.Count; i++)
                                    {
                                        dt = Utility.ChangeColumnDataType(dt, dt.Columns[i].ColumnName, typeof(string));
                                        dt.Columns[i].SetOrdinal(i);
                                    }

                                    foreach (DataRow row in dtExcel.Rows)
                                    {
                                        // COPY EACH ROW OF IMPORTED DATATABLE ROW TO GRID DATATALE
                                        DataRow newRow = dt.NewRow();
                                        newRow.ItemArray = row.ItemArray;
                                        dt.Rows.Add(newRow);
                                        dt.Rows[dt.Rows.Count - 1]["colSequence"] = dt.Rows.Count;
                                        Application.DoEvents();
                                    }
                                }
                                dt.AcceptChanges();
                                #endregion

                                #region RENAME DATATABLE COLUMN NAME AS PAR MAIN GRID
                                foreach (DataGridViewColumn col in dgvGSTR1_HSNSummary.Columns)
                                {
                                    dt.Columns[col.Index].ColumnName = col.Name.ToString();
                                    col.DataPropertyName = col.Name;
                                }
                                #endregion

                                //ASSIGN DATATABLE TO DATAGRID
                                dgvGSTR1_HSNSummary.DataSource = dt;

                                // ENABLE MAIN GRID
                                EnableControls(dgvGSTR1_HSNSummary);
                            }
                            else
                            {
                                // IF THERE ARE NO RECORDS IN MAIN GRID

                                if (dtExcel != null && dtExcel.Rows.Count > 0)
                                {
                                    // IF THERE ARE DATA IN IMPORTED EXCEL FILE

                                    // DISABLE MAIN GRID
                                    DisableControls(dgvGSTR1_HSNSummary);

                                    #region RENAME DATATABLE COLUMN NAME AS PAR MAIN GRID
                                    foreach (DataGridViewColumn col in dgvGSTR1_HSNSummary.Columns)
                                    {
                                        dtExcel.Columns[col.Index].ColumnName = col.Name.ToString();
                                        col.DataPropertyName = col.Name;
                                    }
                                    dtExcel.AcceptChanges();

                                    // ASSIGN DATATALE TO GRID
                                    dgvGSTR1_HSNSummary.DataSource = dtExcel;

                                    // ENABLE MAIN GRID
                                    EnableControls(dgvGSTR1_HSNSummary);
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
                            string[] colNo = { "colTotalQuantity", "colTotalValue", "colTotalTaxableValue", "colIGST", "colCGST", "colSGST", "colCess" };
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
                EnableControls(dgvGSTR1_HSNSummary);
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
                        OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [hsn$]", con);
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
                        for (int i = 2; i < dgvGSTR1_HSNSummary.Columns.Count; i++)
                        {
                            flg = false;
                            for (int j = 0; j < dtexcel.Columns.Count; j++)
                            {
                                // CHECK GRID COLUMN IS PRESENT OR NOT IN IMPORTED EXCEL
                                if (dgvGSTR1_HSNSummary.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim() == dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim())
                                {
                                    // IF GRID COLUMN PRESENT IN EXCEL THEN ITS INDEX AS PAR GRID COLUMN INDEX
                                    flg = true;
                                    dtexcel.Columns[j].SetOrdinal(dgvGSTR1_HSNSummary.Columns[i].Index - 2);
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
                        if (dtexcel.Columns.Count >= dgvGSTR1_HSNSummary.Columns.Count - 2)
                        {
                            for (int i = dtexcel.Columns.Count; i > (dgvGSTR1_HSNSummary.Columns.Count - 2); i--)
                            {
                                dtexcel.Columns.Remove(dtexcel.Columns[i - 1]);
                            }
                        }
                        dtexcel.AcceptChanges();
                        #endregion

                        #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME
                        foreach (DataGridViewColumn col in dgvGSTR1_HSNSummary.Columns)
                        {
                            if (col.Index != 0 && col.Index != 1)
                                dtexcel.Columns[col.Index - 2].ColumnName = col.Name.ToString();
                        }
                        #endregion

                        // ADD CHECK BOX COLUMN TO DATATBLE AND MAKE IT FIRST TABLE COLUMN
                        dtexcel.Columns.Add(new DataColumn("colSequence"));
                        dtexcel.Columns["colSequence"].SetOrdinal(0);
                        dtexcel.Columns.Add(new DataColumn("colChk"));
                        dtexcel.Columns["colChk"].SetOrdinal(0);
                        dtexcel.AcceptChanges();

                        #region SET COLTAX VALUE AS TRUE/FALSE
                        for (int i = 0; i < dtexcel.Rows.Count; i++)
                        {
                            dtexcel.Rows[i]["colChk"] = "False";
                            dtexcel.Rows[i]["colSequence"] = i + 1;

                            //if (chkCellValue(Convert.ToString(dtexcel.Rows[i]["colUQC"]).Trim(), "colUQC"))
                            //    dtexcel.Rows[i]["colUQC"] = Convert.ToString(dtexcel.Rows[i]["colUQC"]).Trim().Replace('-', ' ');
                            //else
                            //    dtexcel.Rows[i]["colUQC"] = "";

                            if (Utility.HNSUQC(Convert.ToString(dtexcel.Rows[i]["colUQC"]).Trim()))
                                dtexcel.Rows[i]["colUQC"] = Utility.StrHNSUQC(Convert.ToString(dtexcel.Rows[i]["colUQC"]).Trim());
                            else
                                dtexcel.Rows[i]["colUQC"] = "";
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
                if (dgvGSTR1_HSNSummary.Rows.Count > 1)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID

                    pbGSTR1.Visible = true;

                    #region CREATE WORKBOOK AND ASSIGN COLUMNNAME
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook WB = excelApp.Workbooks.Add(Missing.Value);

                    Excel.Worksheet newWS = (Excel.Worksheet)excelApp.Worksheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    newWS.Name = "hsn";

                    // DELETE UNUSED WORKSHEETS FROM WORKBOOK
                    foreach (Excel.Worksheet ws in WB.Worksheets)
                    {
                        if (ws.Name != "hsn")
                            ((Excel.Worksheet)WB.Worksheets[ws.Name]).Delete();
                    }

                    // ASSIGN COLUMN HEADER AS PAR THE GRID HEADER
                    for (int i = 2; i < dgvGSTR1_HSNSummary.Columns.Count; i++)
                    {
                        newWS.Cells[1, i - 1] = dgvGSTR1_HSNSummary.Columns[i].HeaderText.ToString();

                        ((Excel.Range)newWS.Cells[1, i - 1]).ColumnWidth = 17;
                    }

                    // GET RANGE AND SET DIFFRENT PROPERTIES
                    Excel.Range headerRange = (Excel.Range)newWS.get_Range((Excel.Range)newWS.Cells[1, 1], (Excel.Range)newWS.Cells[1, dgvGSTR1_HSNSummary.Columns.Count - 2]);
                    headerRange.WrapText = true;
                    headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Calibri";
                    #endregion

                    #region COPY DATA FROM DATATABLE TO ARRAY

                    // CREATE ARRAY TO HOLD THE DATA OF DATATABLE
                    object[,] arr = new object[dgvGSTR1_HSNSummary.Rows.Count - 1, dgvGSTR1_HSNSummary.Columns.Count];

                    // ASSIGN DATA TO ARRAY FROM DATATABLE
                    if (CommonHelper.IsLicence)
                    {
                        // FOR LICENECE ALLOWS TO EXPORT ALL RECORDS
                        for (int i = 0; i < dgvGSTR1_HSNSummary.Rows.Count - 1; i++)
                        {
                            for (int j = 2; j < dgvGSTR1_HSNSummary.Columns.Count; j++)
                            {
                                arr[i, j - 2] = Convert.ToString(dgvGSTR1_HSNSummary.Rows[i].Cells[j].Value);
                            }
                        }
                    }
                    else
                    {
                        // FOR DEMO ALLOW ONLY 100 RECORDS TO EXPORT
                        for (int i = 0; i < dgvGSTR1_HSNSummary.Rows.Count - 1; i++)
                        {
                            if (i < 100)
                            {
                                for (int j = 2; j < dgvGSTR1_HSNSummary.Columns.Count; j++)
                                {
                                    arr[i, j - 2] = Convert.ToString(dgvGSTR1_HSNSummary.Rows[i].Cells[j].Value);
                                }
                            }
                        }
                    }

                    //SET EXCEL RANGE TO PASTE THE DATA
                    Excel.Range top = (Excel.Range)newWS.Cells[2, 1];
                    Excel.Range bottom = (Excel.Range)newWS.Cells[dgvGSTR1_HSNSummary.Rows.Count, dgvGSTR1_HSNSummary.Columns.Count];
                    Excel.Range sheetRange = newWS.Range[top, bottom];
                    sheetRange.WrapText = true;
                    sheetRange.Columns.AutoFit();
                    sheetRange.Rows.AutoFit();
                    //sheetRange.NumberFormat = "@";

                    Excel.Range rg = (Excel.Range)sheetRange.Cells[1, 1];
                    rg.EntireColumn.NumberFormat = "@";

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

        public void ExportExcelForValidatation()
        {
            List<int> listValid = new List<int>();
            try
            {
                if (dgvGSTR1_HSNSummary.Rows.Count > 1)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID

                    pbGSTR1.Visible = true;

                    #region CREATE WORKBOOK AND ASSIGN COLUMNNAME
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook WB = excelApp.Workbooks.Add(Missing.Value);

                    Excel.Worksheet newWS = (Excel.Worksheet)excelApp.Worksheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    newWS.Name = "HSN";

                    // DELETE UNUSED WORKSHEETS FROM WORKBOOK
                    foreach (Excel.Worksheet ws in WB.Worksheets)
                    {
                        if (ws.Name != "HSN")
                            ((Excel.Worksheet)WB.Worksheets[ws.Name]).Delete();
                    }

                    // ASSIGN COLUMN HEADER AS PAR THE GRID HEADER
                    int yy = 1;
                    for (int i = 2; i < dgvGSTR1_HSNSummary.Columns.Count + 1; i++)
                    {
                        newWS.Cells[1, i - 1] = dgvGSTR1_HSNSummary.Columns[yy].HeaderText.ToString();

                        ((Excel.Range)newWS.Cells[1, i - 1]).ColumnWidth = 17;
                        yy++;
                    }
                    ((Excel.Range)newWS.Cells[1, 17]).ColumnWidth = 45;
                    //Change as per Requirement


                    // GET RANGE AND SET DIFFRENT PROPERTIES
                    Excel.Range headerRange = (Excel.Range)newWS.get_Range((Excel.Range)newWS.Cells[1, 1], (Excel.Range)newWS.Cells[1, dgvGSTR1_HSNSummary.Columns.Count]);
                    headerRange.WrapText = true;
                    headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Calibri";
                    #endregion

                    #region COPY DATA FROM DATATABLE TO ARRAY

                    //SET EXCEL RANGE TO PASTE THE DATA
                    bool ExcelValidFlag = false;
                    DataTable dt = new DataTable();
                    foreach (DataGridViewColumn column in dgvGSTR1_HSNSummary.Columns)
                        dt.Columns.Add(column.Name, typeof(string));

                    for (int k = 0; k < dgvGSTR1_HSNSummary.Rows.Count; k++)
                    {
                        for (int j = 0; j < dgvGSTR1_HSNSummary.ColumnCount; j++)
                        {
                            if (dgvGSTR1_HSNSummary.Rows[k].Cells[j].Style.BackColor == Color.Red)
                            {
                                ExcelValidFlag = true;
                                //sheetRange.Cells[k + 1, j - 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                            }
                        }
                        if (ExcelValidFlag == true)
                        {
                            dt.Rows.Add();
                            int count = dt.Rows.Count - 1;
                            for (int b = 0; b < dgvGSTR1_HSNSummary.Columns.Count; b++)
                            {
                                dt.Rows[count][b] = dgvGSTR1_HSNSummary.Rows[k].Cells[b].Value;
                            }
                            ExcelValidFlag = false;
                        }
                    }

                    Excel.Range top = (Excel.Range)newWS.Cells[2, 1];
                    Excel.Range bottom = (Excel.Range)newWS.Cells[dt.Rows.Count + 1, dt.Columns.Count - 1];
                    Excel.Range sheetRange = newWS.Range[top, bottom];
                    //   sheetRange.WrapText = true;
                    sheetRange.Columns.AutoFit();
                    sheetRange.Rows.AutoFit();
                    //sheetRange.NumberFormat = "@";

                    //FILL ARRAY IN EXCEL
                    //bool ExcelValidFlag = false;
                    object[,] Excelarr = new object[0, 0];
                    //DataTable dt = new DataTable();

                    listValid = listValid.Distinct().ToList();
                    int[] array = listValid.ToArray();
                    int Ab = 0;
                    DataTable dt_new = new DataTable();
                    dt_new = dt.Clone();

                    for (int k = 0; k < dgvGSTR1_HSNSummary.Rows.Count; k++)
                    {
                        string str_error = "";
                        int cnt = 1;
                        for (int j = 0; j < dgvGSTR1_HSNSummary.ColumnCount; j++)
                        {
                            if (dgvGSTR1_HSNSummary.Rows[k].Cells[j].Style.BackColor == Color.Red)
                            {
                                ExcelValidFlag = true;
                                sheetRange.Cells[Ab + 1, j].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);


                                if (dgvGSTR1_HSNSummary.Columns[j].Name == "colHSN")
                                {
                                    if (Convert.ToString(dgvGSTR1_HSNSummary.Rows[k].Cells[j].Value).Trim() == "")
                                    {
                                        str_error += cnt + ") " + "Please Enter Valid HSN no.\n";
                                    }
                                    else
                                    {
                                        str_error += cnt + ") Please remove .(dot) from the HSN no.. \n";
                                    }
                                }
                                else if (dgvGSTR1_HSNSummary.Columns[j].Name == "colDesciption")
                                {
                                    if (Convert.ToString(dgvGSTR1_HSNSummary.Rows[k].Cells[j].Value).Trim() == "")
                                    {
                                        str_error += cnt + ") " + " Please enter description up to 30 character.\n";
                                    }
                                    else
                                    {
                                        str_error += cnt + ") Same description for same HSN and UQC is not allowed. It allows only space and alphanumeric value. description only 30 character. \n";
                                    }
                                }

                                else if (dgvGSTR1_HSNSummary.Columns[j].Name == "colUQC")
                                {
                                    if (Convert.ToString(dgvGSTR1_HSNSummary.Rows[k].Cells[j].Value).Trim() == "")
                                    {
                                        str_error += cnt + ") " + "Please select proper UQC value from the drop-down..\n";
                                    }
                                    else
                                    {
                                        str_error += cnt + ") Same UQC and same description for same HSN is not allowed..\n";
                                    }
                                }
                                else if (dgvGSTR1_HSNSummary.Columns[j].Name == "colTotalQuantity")
                                {
                                    if (Convert.ToString(dgvGSTR1_HSNSummary.Rows[k].Cells[j].Value).Trim() == "")
                                    {
                                        str_error += cnt + ") " + " Please enter valid " + dgvGSTR1_HSNSummary.Columns[j].HeaderText + ".\n";
                                    }
                                    else
                                    {
                                        str_error += cnt + ") " + " Please enter valid " + dgvGSTR1_HSNSummary.Columns[j].HeaderText + ".\n";
                                    }
                                }
                                else
                                {
                                    str_error += cnt + ") " + " Please enter proper " + dgvGSTR1_HSNSummary.Columns[j].HeaderText + ".\n";
                                }
                                cnt++;
                            }
                        }
                        if (ExcelValidFlag == true)
                        {
                            Ab++;
                            dt_new.Rows.Add();
                            int c = dt_new.Rows.Count;

                            for (int b = 0; b < dgvGSTR1_HSNSummary.Columns.Count; b++)
                            {
                                if (dt_new.Columns.Count - 1 == b)
                                {
                                    dt_new.Rows[c - 1][b] = str_error;
                                }
                                else
                                {
                                    dt_new.Rows[c - 1][b] = Convert.ToString(dgvGSTR1_HSNSummary.Rows[k].Cells[b].Value);
                                }
                            }
                            ExcelValidFlag = false;
                        }
                    }
                    object[,] MyExcelarr = new object[dt_new.Rows.Count + 1, dt_new.Columns.Count - 1];

                    for (int i = 0; i < dt_new.Rows.Count; i++)
                    {
                        for (int j = 1; j < dt_new.Columns.Count; j++)
                        {
                            MyExcelarr[i, j - 1] = Convert.ToString(dt_new.Rows[i][j]);
                        }
                    }

                    Excel.Range rg = (Excel.Range)sheetRange.Cells[1, 2];
                    rg.EntireColumn.NumberFormat = "@";

                    sheetRange.Value2 = MyExcelarr;

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
                        dt = (DataTable)dgvGSTR1_HSNSummary.DataSource;

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
                                DisableControls(dgvGSTR1_HSNSummary);

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
                                        dt.Rows[dt.Rows.Count - 1]["colSequence"] = dt.Rows.Count;
                                        Application.DoEvents();
                                    }
                                }
                                dt.AcceptChanges();
                                #endregion

                                #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME
                                foreach (DataGridViewColumn col in dgvGSTR1_HSNSummary.Columns)
                                {
                                    dtCsv.Columns[col.Index].ColumnName = col.Name.ToString();
                                    col.DataPropertyName = col.Name;
                                }
                                dt.AcceptChanges();
                                #endregion

                                // ASSIGN DATATABLE TO GRID
                                dgvGSTR1_HSNSummary.DataSource = dt;

                                // ENABLE MAIN GRID
                                EnableControls(dgvGSTR1_HSNSummary);
                            }
                            else
                            {
                                // IF THERE ARE NO RECORDS IN MAIN GRID

                                if (dtCsv != null && dtCsv.Rows.Count > 0)
                                {
                                    // IF THERE ARE RECORD PRESENT IN IMPORT FILE

                                    // DISABLE MAIN GRID
                                    DisableControls(dgvGSTR1_HSNSummary);

                                    #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME AND ASSIGN TO MAIN GRID
                                    foreach (DataGridViewColumn col in dgvGSTR1_HSNSummary.Columns)
                                    {
                                        dtCsv.Columns[col.Index].ColumnName = col.Name.ToString();
                                        col.DataPropertyName = col.Name;
                                    }
                                    dtCsv.AcceptChanges();

                                    // ASSIGN DATATABLE TO GRID
                                    dgvGSTR1_HSNSummary.DataSource = dtCsv;

                                    // ENABLE CONTROL
                                    EnableControls(dgvGSTR1_HSNSummary);
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
                            string[] colNo = { "colTotalQuantity", "colTotalValue", "colTotalTaxableValue", "colIGST", "colCGST", "colSGST", "colCess" };
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
                EnableControls(dgvGSTR1_HSNSummary);
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
                    for (int i = 1; i < dgvGSTR1_HSNSummary.Columns.Count; i++)
                    {
                        Boolean flg = false;
                        for (int j = 0; j < csvData.Columns.Count; j++)
                        {
                            // CHECK GRID COLUMN IS PRESENT OR NOT IN IMPORTED EXCEL
                            if (dgvGSTR1_HSNSummary.Columns[i].HeaderText.Replace(" ", "") == csvData.Columns[j].ColumnName.Replace(" ", "").Trim())
                            {
                                // IF GRID COLUMN PRESENT IN EXCEL THEN ITS INDEX AS PAR GRID COLUMN INDEX
                                flg = true;
                                csvData.Columns[j].SetOrdinal(dgvGSTR1_HSNSummary.Columns[i].Index - 1);
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
                    if (csvData.Columns.Count >= dgvGSTR1_HSNSummary.Columns.Count - 2)
                    {
                        for (int i = csvData.Columns.Count - 1; i > (dgvGSTR1_HSNSummary.Columns.Count - 2); i--)
                        {
                            csvData.Columns.Remove(csvData.Columns[i]);
                        }
                    }
                    #endregion

                    #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME
                    foreach (DataGridViewColumn col in dgvGSTR1_HSNSummary.Columns)
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
                        csvData.Rows[i]["colSequence"] = i + 1;
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
                if (dgvGSTR1_HSNSummary.Rows.Count > 1)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID

                    pbGSTR1.Visible = true;

                    string csv = string.Empty;
                    // CREATE DATATABLE AND GET GRID DATA
                    DataTable dt = new DataTable();
                    dt = (DataTable)dgvGSTR1_HSNSummary.DataSource;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        #region ASSIGN COLUMN NAME TO CSV STRING
                        for (int i = 1; i < dgvGSTR1_HSNSummary.Columns.Count; i++)
                        {
                            csv += dgvGSTR1_HSNSummary.Columns[i].HeaderText + ',';
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
                PdfPTable pdfTable = new PdfPTable(dgvGSTR1_HSNSummary.ColumnCount - 1);
                pdfTable.DefaultCell.Padding = 3;
                pdfTable.WidthPercentage = 100;
                pdfTable.DefaultCell.BorderWidth = 0;
                iTextSharp.text.Font fontHeader = iTextSharp.text.FontFactory.GetFont("Calibri", 6);

                // ADD HEADER TO PDF TABLE
                pdfTable = AssignHeader(pdfTable, "12. HSN-wise summary of outward supplies");
                #endregion

                #region ADDING HEADER ROW
                int i = 0;

                #region HEADER1
                PdfPCell celHeader1 = new PdfPCell();

                celHeader1 = new PdfPCell(new Phrase("Sr. No.", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("HSN", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Description", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("UQC", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Total Quantity", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Total Value", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Total Taxable Value", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Amount", fontHeader));
                celHeader1.Colspan = 4;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                pdfTable.CompleteRow();
                #endregion

                #region HEADER2
                PdfPCell celHeader2 = new PdfPCell();

                celHeader2 = new PdfPCell(new Phrase("Integrated Tax", fontHeader));
                celHeader2 = SetAllignMent(celHeader2, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(255, 255, 204));
                pdfTable.AddCell(celHeader2);

                celHeader2 = new PdfPCell(new Phrase("Central Tax", fontHeader));
                celHeader2 = SetAllignMent(celHeader2, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(255, 255, 204));
                pdfTable.AddCell(celHeader2);

                celHeader2 = new PdfPCell(new Phrase("State/UT Tax", fontHeader));
                celHeader2 = SetAllignMent(celHeader2, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(255, 255, 204));
                pdfTable.AddCell(celHeader2);

                celHeader2 = new PdfPCell(new Phrase("Cess", fontHeader));
                celHeader2 = SetAllignMent(celHeader2, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(255, 255, 204));
                pdfTable.AddCell(celHeader2);

                pdfTable.CompleteRow();
                #endregion

                Application.DoEvents();
                #endregion

                #region ADDING COLUMN NUMBER
                i = 0;
                foreach (DataGridViewColumn column in dgvGSTR1_HSNSummary.Columns)
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
                    foreach (DataGridViewRow row in dgvGSTR1_HSNSummary.Rows)
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
                    foreach (DataGridViewRow row in dgvGSTR1_HSNSummary.Rows)
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
                ce1.Colspan = dgvGSTR1_HSNSummary.Columns.Count - 1;
                ce1.VerticalAlignment = Element.ALIGN_CENTER;
                ce1.HorizontalAlignment = Element.ALIGN_LEFT;
                ce1.BackgroundColor = new iTextSharp.text.BaseColor(197, 223, 197);
                ce1.BorderWidth = 0;
                pdfTable.AddCell(ce1);

                iTextSharp.text.Font FigToRs = FontFactory.GetFont("Calibri", 10, iTextSharp.text.Font.BOLD);
                PdfPCell ceHeader2 = new PdfPCell(new Phrase("(figures in Rs)", FigToRs));
                ceHeader2.Colspan = dgvGSTR1_HSNSummary.Columns.Count - 1;
                ceHeader2.VerticalAlignment = Element.ALIGN_CENTER;
                ceHeader2.HorizontalAlignment = Element.ALIGN_RIGHT;
                ceHeader2.BorderWidth = 0;
                pdfTable.AddCell(ceHeader2);

                PdfPCell ce2 = new PdfPCell(new Phrase(" "));
                ce2.Colspan = dgvGSTR1_HSNSummary.Columns.Count - 1;
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

        #region JSON Class
        public class Datum
        {
            public int num { get; set; }
            public string hsn_sc { get; set; }
            public string desc { get; set; }
            public string uqc { get; set; }
            [DefaultValue("")]
            public double qty { get; set; }
            [DefaultValue("")]
            public double val { get; set; }
            [DefaultValue("")]
            public double txval { get; set; }
            public double iamt { get; set; }
            public double samt { get; set; }
            public double camt { get; set; }
            public double csamt { get; set; }
        }

        public class Hsn
        {
            public List<Datum> data { get; set; }
        }

        public class RootObject
        {
            public string gstin { get; set; }
            public string fp { get; set; }
            public double gt { get; set; }
            public double cur_gt { get; set; }
            public Hsn hsn { get; set; }
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
                    #region Json Generation

                    DataTable dt = new DataTable();

                    #region Bind Grid Data

                    #region ADD DATATABLE COLUMN

                    foreach (DataGridViewColumn col in dgvGSTR1_HSNSummary.Columns)
                    {
                        dt.Columns.Add(col.Name.ToString());
                        col.DataPropertyName = col.Name;
                    }
                    #endregion

                    #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                    object[] rowValue = new object[dt.Columns.Count];

                    foreach (DataGridViewRow dr in dgvGSTR1_HSNSummary.Rows)
                    {
                        if (dr.Index != dgvGSTR1_HSNSummary.Rows.Count - 1)
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
                    dt.Columns.Remove("colSequence");
                    dt.AcceptChanges();

                    #endregion

                    #endregion

                    List<string> list = dt.Rows
                           .OfType<DataRow>()
                           .Select(x => Convert.ToString(x["colHSN"]).Trim())
                           .Distinct().ToList();

                    if (list != null && list.Count > 0)
                    {
                        ObjJson.gstin = CommonHelper.CompanyGSTN; // tax person GSTIN
                        ObjJson.fp = CommonHelper.GetReturnPeriod(); // current return period
                        ObjJson.gt = CommonHelper.TurnOver; // previous financial year turnover
                        ObjJson.cur_gt = Convert.ToDouble(CommonHelper.CurrentTurnOver); // current Finacial year turnover

                        Hsn objHSN = new Hsn();
                        List<Datum> lstDataum = new List<Datum>();

                        for (int i = 0; i < list.Count; i++)
                        {
                            if (list[i] != "")
                            {
                                List<DataRow> lstDrRow = dt.Rows
                                        .OfType<DataRow>()
                                        .Where(x => list[i] == Convert.ToString(x["colHSN"]).Trim())
                                        .Select(x => x)
                                        .ToList();

                                if (lstDrRow != null && lstDrRow.Count > 0)
                                {
                                    Datum clsDatum = new Datum();

                                    #region HSN Details

                                    clsDatum.num = i + 1;

                                    if (!string.IsNullOrEmpty(Convert.ToString(lstDrRow[0]["colHSN"]).Trim()))
                                        clsDatum.hsn_sc = Convert.ToString(lstDrRow[0]["colHSN"]).Trim(); // HSN

                                    if (!string.IsNullOrEmpty(Convert.ToString(lstDrRow[0]["colDesciption"]).Trim()))
                                        clsDatum.desc = Convert.ToString(lstDrRow[0]["colDesciption"]).Trim(); // HSN Descroption

                                    if (!string.IsNullOrEmpty(Convert.ToString(lstDrRow[0]["colUQC"]).Trim()))
                                        clsDatum.uqc = Convert.ToString(lstDrRow[0]["colUQC"]).Trim(); // Unit of Measurement

                                    if (!string.IsNullOrEmpty(Convert.ToString(lstDrRow[0]["colTotalQuantity"]).Trim()))
                                        clsDatum.qty = Convert.ToDouble(lstDrRow[0]["colTotalQuantity"]); // Quentity

                                    if (!string.IsNullOrEmpty(Convert.ToString(lstDrRow[0]["colTotalValue"]).Trim()))
                                        clsDatum.val = Convert.ToDouble(lstDrRow[0]["colTotalValue"]); // Value

                                    if (lstDrRow.Count == 1)
                                    {
                                        if (!string.IsNullOrEmpty(Convert.ToString(lstDrRow[0]["colTotalTaxableValue"]).Trim()))
                                            clsDatum.txval = Convert.ToDouble(lstDrRow[0]["colTotalTaxableValue"]); // Taxable Value

                                        if (!string.IsNullOrEmpty(Convert.ToString(lstDrRow[0]["colIGST"]).Trim()))
                                            clsDatum.iamt = Convert.ToDouble(lstDrRow[0]["colIGST"]); // IGST

                                        if (!string.IsNullOrEmpty(Convert.ToString(lstDrRow[0]["colCGST"]).Trim()))
                                            clsDatum.camt = Convert.ToDouble(lstDrRow[0]["colCGST"]); // CGST

                                        if (!string.IsNullOrEmpty(Convert.ToString(lstDrRow[0]["colSGST"]).Trim()))
                                            clsDatum.samt = Convert.ToDouble(lstDrRow[0]["colSGST"]); // SGST

                                        if (!string.IsNullOrEmpty(Convert.ToString(lstDrRow[0]["colCess"]).Trim()))
                                            clsDatum.csamt = Convert.ToDouble(lstDrRow[0]["colCess"]); // CESS
                                    }
                                    else
                                    {
                                        double? igst = null, cgst = null, sgst = null, cess = null;
                                        for (int sr = 0; sr < lstDrRow.Count; sr++)
                                        {
                                            if (Utility.IsDecimalOrNumber(Convert.ToString(lstDrRow[sr]["colIGST"]).Trim()))
                                                igst = Convert.ToDouble(igst) + Convert.ToDouble(lstDrRow[sr]["colIGST"]);

                                            if (Utility.IsDecimalOrNumber(Convert.ToString(lstDrRow[sr]["colCGST"]).Trim()))
                                                cgst = Convert.ToDouble(cgst) + Convert.ToDouble(lstDrRow[sr]["colCGST"]);

                                            if (Utility.IsDecimalOrNumber(Convert.ToString(lstDrRow[sr]["colSGST"]).Trim()))
                                                sgst = Convert.ToDouble(sgst) + Convert.ToDouble(lstDrRow[sr]["colSGST"]);

                                            if (Utility.IsDecimalOrNumber(Convert.ToString(lstDrRow[sr]["colCess"]).Trim()))
                                                cess = Convert.ToDouble(cess) + Convert.ToDouble(lstDrRow[sr]["colCess"]);
                                        }

                                        clsDatum.txval = lstDrRow.Cast<DataRow>().Where(x => x["colTotalTaxableValue"] != null).Sum(x => Convert.ToString(x["colTotalTaxableValue"]).Trim() == "" ? 0 : Convert.ToDouble(x["colTotalTaxableValue"])); // Taxble value

                                        if (igst != null) { clsDatum.iamt = Convert.ToDouble(igst); } // IGST value 
                                        if (cgst != null) { clsDatum.camt = Convert.ToDouble(cgst); } // CGST value
                                        if (sgst != null) { clsDatum.samt = Convert.ToDouble(sgst); } // SGST value
                                        if (cess != null) { clsDatum.csamt = Convert.ToDouble(cess); } // CESS value
                                    }
                                    #endregion

                                    lstDataum.Add(clsDatum);
                                    objHSN.data = lstDataum;
                                    ObjJson.hsn = objHSN;
                                }
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
                    save.FileName = "HSN.json";
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

                    #endregion
                //}
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

        public void SetGridViewColor()
        {
            try
            {
                // DO NOT ALLOW TO AUTO GENERATE COLUMNS
                dgvGSTR1_HSNSummary.AutoGenerateColumns = false;

                // SET HEIGHT WIDTH OF FORM
                this.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.76));
                this.Height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.68));

                // SET WIDTH OF HEADER, MAIN AND TOTAL GRID
                this.panel1.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.745));
                this.dgvGSTR1_HSNSummary.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.745));
                this.dgvGSTR1_HSNSummaryTotal.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.745));

                // SET HEIGHT OF MAIN GRID
                this.dgvGSTR1_HSNSummary.Height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.54));
                this.dgvGSTR1_HSNSummaryTotal.Height = 36;

                // SET LOCATION OF HEADER,LOADING PIC, CHECKBOX AND MAIN AND TOTAL GRID
                //this.panel1.Location = new System.Drawing.Point(12, 5);
                //this.dgvGSTR1_HSNSummary.Location = new System.Drawing.Point(12, Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.066)));
                //this.dgvGSTR1_HSNSummaryTotal.Location = new System.Drawing.Point(12, Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.62)));
                //this.ckboxHeader.Location = new System.Drawing.Point(29, Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.128)));
                //this.pbGSTR1.Location = new System.Drawing.Point(Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.45)), Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.30)));

                // SET MAIN GRID PROPERTY
                dgvGSTR1_HSNSummary.EnableHeadersVisualStyles = false;
                dgvGSTR1_HSNSummary.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(23, 196, 187);
                dgvGSTR1_HSNSummary.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

                this.dgvGSTR1_HSNSummary.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                this.dgvGSTR1_HSNSummary.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                this.dgvGSTR1_HSNSummary.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

                foreach (DataGridViewColumn column in dgvGSTR1_HSNSummary.Columns)
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)dgvGSTR1_HSNSummary.DataSource;
                if (dt == null)
                {
                    MessageBox.Show("Kindly save record(s) before search!!!", "Alert", MessageBoxButtons.OK);
                    return;
                }
                if (cmbFilter.SelectedValue.ToString() == "")
                {
                    ((DataTable)dgvGSTR1_HSNSummary.DataSource).DefaultView.RowFilter = string.Format("colSequence like '%{0}%' or colHSN like '%{0}%' or colDesciption like '%{0}%' or colUQC like '%{0}%' or colTotalQuantity like '%{0}%' or colTotalValue like '%{0}%' or colTotalTaxableValue like '%{0}%' or colIGST like '%{0}%' or colCGST like '%{0}%' or colSGST like '%{0}%' or colCess like '%{0}%'", textBox1.Text.Trim().Replace("'", "''"));
                }
                else
                {
                    ((DataTable)dgvGSTR1_HSNSummary.DataSource).DefaultView.RowFilter = string.Format("" + cmbFilter.SelectedValue + " like '%{0}%'", textBox1.Text.Trim().Replace("'", "''"));
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

        private void dgvGSTR1_HSNSummary_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                // set index of user added row in main grid
                dgvGSTR1_HSNSummary.Rows[e.Row.Index - 1].Cells[1].Value = e.Row.Index;
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

        private void dgvGSTR1_HSNSummary_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                // set sequncing after user deleting row in grid
                for (int i = e.Row.Index; i < dgvGSTR1_HSNSummary.Rows.Count - 1; i++)
                {
                    dgvGSTR1_HSNSummary.Rows[i].Cells["colSequence"].Value = i;
                }

                // total calculation
                string[] colNo = { "colTotalQuantity", "colTotalValue", "colTotalTaxableValue", "colIGST", "colCGST", "colSGST", "colCess" };
                GetTotal(colNo);
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

        public bool chkQOC(string qoc)
        {
            bool flg = false;
            try
            {
                string[] arry = { "", "BAG-BAGS", "BAL-BALE", "BDL-BUNDLES", "BKL-BUCKLES", "BOU-BILLION OF UNITS", "BOX-BOX", "BTL-BOTTLES", "BUN-BUNCHES", "CAN-CANS", "CBM-CUBIC METERS", "CCM-CUBIC CENTIMETERS", "CMS-CENTIMETERS", "CTN-CARTONS", "DOZ-DOZENS", "DRM-DRUMS", "GGK-GREAT GROSS", "GMS-GRAMMES", "GRS-GROSS", "GYD-GROSS YARDS", "KGS-KILOGRAMS", "KLR-KILOLITRE", "KME-KILOMETRE", "MLT-MILILITRE", "MTR-METERS", "MTS-METRIC TON", "NOS-NUMBERS", "PAC-PACKS", "PCS-PIECES", "PRS-PAIRS", "QTL-QUINTAL", "ROL-ROLLS", "SET-SETS", "SQF-SQUARE FEET", "SQM-SQUARE METERS", "SQY-SQUARE YARDS", "TBS-TABLETS", "TGM-TEN GROSS", "THD-THOUSANDS", "TON-TONNES", "TUB-TUBES", "UGS-US GALLONS", "UNT-UNITS", "YDS-YARDS", "OTH-OTHERS" };

                for (int i = 0; i < arry.Length; i++)
                {
                    if (qoc == arry[i])
                    {
                        flg = true;
                        break;
                    }
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

            return flg;
        }

        #region disable/enable controls

        private void DisableControls(Control con)
        {
            foreach (Control c in con.Controls)
            {
                if (c.Name != "dgvGSTR1_HSNSummary" && c.Name != "dgvGSTR1_HSNSummaryTotal")
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

        #region scroll grid
        private void dgvGSTR1_HSNSummary_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                // set total grid offset as par main grid scrol
                this.dgvGSTR1_HSNSummaryTotal.HorizontalScrollingOffset = this.dgvGSTR1_HSNSummary.HorizontalScrollingOffset;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvGSTR1_HSNSummaryTotal_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                // set main grid offset as par total grid scrol
                this.dgvGSTR1_HSNSummary.HorizontalScrollingOffset = this.dgvGSTR1_HSNSummaryTotal.HorizontalScrollingOffset;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Check All/ Uncheck All

        private void ckboxHeader_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                // IF THERE ARE RECORDS IN MAIN GRID
                if (dgvGSTR1_HSNSummary.Rows.Count > 1)
                {
                    pbGSTR1.Visible = true;

                    if (ckboxHeader.Checked)
                    {
                        // IF CHECK BOX IS CHECKED

                        // SET CHECK BOX COLUMN VALUE AS TRUE
                        for (int i = 0; i < dgvGSTR1_HSNSummary.Rows.Count - 1; i++)
                        {
                            dgvGSTR1_HSNSummary.Rows[i].Cells[0].Value = "True";
                            Application.DoEvents();
                        }

                        // CHANGE HEADER TEXT AND WIDTH OF COLUMN AND POSITION OF CHECK BOX OF CHECK ALL COLUMN
                        //dgvGSTR1_HSNSummary.Columns[0].DefaultCellStyle.NullValue = true;
                        dgvGSTR1_HSNSummary.Columns[0].HeaderText = "Uncheck All";
                    }
                    else if (ckboxHeader.Checked == false)
                    {
                        // IF CHECK BOX IS UNCHECKED

                        // SET CHECK BOX COLUMN VALUE AS FALSE
                        for (int i = 0; i < dgvGSTR1_HSNSummary.Rows.Count - 1; i++)
                        {
                            dgvGSTR1_HSNSummary.Rows[i].Cells["colChk"].Value = "False";
                            Application.DoEvents();
                        }

                        // CHANGE HEADER TEXT AND WIDTH OF COLUMN AND POSITION OF CHECK BOX OF CHECK ALL COLUMN
                        //dgvGSTR1_HSNSummary.Columns[0].DefaultCellStyle.NullValue = false;
                        dgvGSTR1_HSNSummary.Columns["colChk"].HeaderText = "Check All";
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

        private void dgvGSTR1_HSNSummary_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                // check first column header pressed and main grid having records
                if (e.ColumnIndex == 0 && dgvGSTR1_HSNSummary.Rows.Count > 1)
                {
                    // check and uncheck check box of header for selecting and unselecting all records
                    if (dgvGSTR1_HSNSummary.Columns[e.ColumnIndex].HeaderText == "Check All")
                        ckboxHeader.Checked = true;
                    else if (dgvGSTR1_HSNSummary.Columns[e.ColumnIndex].HeaderText == "Uncheck All")
                        ckboxHeader.Checked = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void frmGSTR17A1_FormClosed(object sender, FormClosedEventArgs e)
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
           // ValidataAndGetGSTIN();
        }

    }
}
