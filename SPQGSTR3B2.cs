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
using SPEQTAGST.BAL.M796r3b;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace SPEQTAGST.rintlcs3b
{
    public partial class SPQGSTR3B2 : Form
    {
        r3bPublicclass objGSTR13 = new r3bPublicclass();
        r1Publicclass objGSTR5 = new r1Publicclass();

        public SPQGSTR3B2()
        {
            InitializeComponent();
            GetData();

            // total calculation
            string[] colNo = { "colTaxableValue", "colIGST" };
            GetTotal(colNo);

            SetGridViewColor();
            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName(CommonHelper.ReturnName);

            pbGSTR1.Visible = false;

            dgvGSTR3B2.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dgvGSTR3B2.EnableHeadersVisualStyles = false;
            dgvGSTR3B2.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);
            dgvGSTR3B2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvGSTR3B2.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            TotaldgvGSTR13.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void GetData()
        {
            try
            {
                // create datatable to store database data
                DataTable dt = new DataTable();
                string Query = "Select * from SPQR3BInterStateSupplies where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
                Application.DoEvents();

                // get data from database
                dt = objGSTR13.GetDataGSTR3B(Query);

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
                    foreach (DataGridViewColumn col in dgvGSTR3B2.Columns)
                    {
                        dt.Columns[col.Index].ColumnName = col.Name.ToString();
                        col.DataPropertyName = col.Name;
                    }
                    dt.AcceptChanges();

                    dgvGSTR3B2.DataSource = dt;
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

        public void GetTotal(string[] colNo)
        {
            try
            {
                if (dgvGSTR3B2.Rows.Count > 1)
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
                        dr["colTTaxable"] = dgvGSTR3B2.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colTaxableValue"].Value != null).Sum(x => x.Cells["colTaxableValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colTaxableValue"].Value)).ToString();
                        dr["colTIGST"] = dgvGSTR3B2.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colIGST"].Value != null).Sum(x => x.Cells["colIGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIGST"].Value)).ToString();

                        // add datarow to datatable
                        dtTotal.Rows.Add(dr);

                        for (int i = 0; i < dtTotal.Rows.Count; i++)
                        {
                            for (int j = 0; j < dtTotal.Columns.Count; j++)
                            {
                                string ColName = dtTotal.Columns[j].ColumnName;
                                if (ColName == "colTTaxable" || ColName == "colTIGST")
                                    dtTotal.Rows[i][j] = Utility.DisplayIndianCurrency(Convert.ToString(dtTotal.Rows[i][j]));
                            }
                        }

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
                            if (item == "colTaxableValue")
                                TotaldgvGSTR13.Rows[0].Cells["colTTaxable"].Value = Utility.DisplayIndianCurrency(dgvGSTR3B2.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colTaxableValue"].Value != null).Sum(x => x.Cells["colTaxableValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colTaxableValue"].Value)).ToString());
                            else if (item == "colIGST")
                                TotaldgvGSTR13.Rows[0].Cells["colTIGST"].Value = Utility.DisplayIndianCurrency(dgvGSTR3B2.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colIGST"].Value != null).Sum(x => x.Cells["colIGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIGST"].Value)).ToString());
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
                        if (dgvGSTR3B2.Rows.Count > 0)
                        {
                            // delete selected cell in grid
                            foreach (DataGridViewCell oneCell in dgvGSTR3B2.SelectedCells)
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
                    string[] colNo = { "colTaxableValue", "colIGST" };
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
                    if (dgvGSTR3B2.RowCount > 0) // IF GRID IS NOT NULL AND PAST ONLY ON SELECTED CELLS
                    {
                        foreach (DataGridViewCell oneCell in dgvGSTR3B2.SelectedCells)
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
                            DisableControls(dgvGSTR3B2);

                            gRowNo = dgvGSTR3B2.Rows.Count - 1;
                            int no = tmp;

                            if (iRow > gRowNo - 1)
                            {
                                DataTable dtDGV = new DataTable();
                                //dtDGV = dgvGSTR13.DataSource as DataTable;

                                #region ADD DATATABLE COLUMN

                                // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                                foreach (DataGridViewColumn col in dgvGSTR3B2.Columns)
                                {
                                    dtDGV.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }
                                #endregion

                                #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                                // CREATE OBJECT ARRAY TO STORE ONE ROW DATA OF MAIN GRID
                                object[] rowValue = new object[dtDGV.Columns.Count];

                                foreach (DataGridViewRow dr in dgvGSTR3B2.Rows)
                                {
                                    if (dr.Index != dgvGSTR3B2.Rows.Count - 1) // DON'T ADD LAST ROW
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
                                        if (iCol + i < this.dgvGSTR3B2.ColumnCount && i < 7)
                                        {
                                            // skip check box column and sequance column to paste data
                                            if (iCol == 0)
                                                oCell = dgvGSTR3B2[iCol + i + 2, iRow];
                                            else if (iCol == 1)
                                                oCell = dgvGSTR3B2[iCol + i + 1, iRow];
                                            else
                                                oCell = dgvGSTR3B2[iCol + i, iRow];

                                            sCells[i] = sCells[i].Trim().Replace(",", "");
                                            if (oCell.ColumnIndex != 0)
                                            {
                                                if (dgvGSTR3B2.Columns[oCell.ColumnIndex].Name != "colChk" && dgvGSTR3B2.Columns[oCell.ColumnIndex].Name != "colSrNo")
                                                {
                                                    #region VALIDATION
                                                    if (sCells[i].ToString().Trim() == "" && dgvGSTR3B2.Columns[oCell.ColumnIndex].Name != "colTax")
                                                    {
                                                        dgvGSTR3B2.Rows[iRow].Cells[oCell.ColumnIndex].Value = DBNull.Value;
                                                    }
                                                    else
                                                    {
                                                        if (oCell.ColumnIndex >= 2 && oCell.ColumnIndex <= 8)
                                                        {
                                                            if (chkCellValue(sCells[i].Trim(), dgvGSTR3B2.Columns[oCell.ColumnIndex].Name))
                                                                dgvGSTR3B2.Rows[iRow].Cells[oCell.ColumnIndex].Value = sCells[i].Trim();
                                                            else
                                                                dgvGSTR3B2.Rows[iRow].Cells[oCell.ColumnIndex].Value = DBNull.Value;
                                                        }
                                                        else { dgvGSTR3B2.Rows[iRow].Cells[oCell.ColumnIndex].Value = sCells[i].Trim(); }
                                                    }
                                                    #endregion
                                                }
                                            }
                                            else
                                            {
                                                if (iCol > i)
                                                {
                                                    for (int j = oCell.ColumnIndex; j < dgvGSTR3B2.Columns.Count; j++)
                                                    {
                                                        #region VALIDATION
                                                        if (sCells[i].ToString().Trim() == "") { dgvGSTR3B2.Rows[iRow].Cells[j].Value = DBNull.Value; }
                                                        else
                                                        {
                                                            if (j >= 2 && j <= 8)
                                                            {
                                                                if (chkCellValue(sCells[i].Trim(), dgvGSTR3B2.Columns[j].Name))
                                                                    dgvGSTR3B2.Rows[iRow].Cells[j].Value = sCells[i].Trim();
                                                                else
                                                                    dgvGSTR3B2.Rows[iRow].Cells[j].Value = DBNull.Value;
                                                            }
                                                            else { dgvGSTR3B2.Rows[iRow].Cells[j].Value = sCells[i].Trim(); }
                                                        }
                                                        #endregion

                                                        //dgvGSTR13.Rows[iRow].Cells[j].Value = sCells[i].Trim();
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
                                                    for (int j = oCell.ColumnIndex; j < dgvGSTR3B2.Columns.Count; j++)
                                                    {
                                                        #region VALIDATION
                                                        if (sCells[i].ToString().Trim() == "") { dgvGSTR3B2.Rows[iRow].Cells[j].Value = DBNull.Value; }
                                                        else
                                                        {
                                                            if (j >= 2 && j <= 8)
                                                            {
                                                                if (chkCellValue(sCells[i].Trim(), dgvGSTR3B2.Columns[j].Name))
                                                                    dgvGSTR3B2.Rows[iRow].Cells[j].Value = sCells[i].Trim();
                                                                else
                                                                    dgvGSTR3B2.Rows[iRow].Cells[j].Value = DBNull.Value;
                                                            }
                                                            else { dgvGSTR3B2.Rows[iRow].Cells[j].Value = sCells[i].Trim(); }
                                                        }
                                                        #endregion

                                                        //dgvGSTR13.Rows[iRow].Cells[j].Value = sCells[i].Trim();
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
                    EnableControls(dgvGSTR3B2);
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
                EnableControls(dgvGSTR3B2);
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
                DisableControls(dgvGSTR3B2);

                #region Set datatable
                int cnt = 0, colNo = 0;

                // assign grid data to datatable
                DataTable dt = dtDGV;

                if (dt == null)
                {
                    // if no record in grid then create new daatable
                    dt = new DataTable();

                    // add column as par main grid and set data access property
                    foreach (DataGridViewColumn col in dgvGSTR3B2.Columns)
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
                                if (iCol + i < this.dgvGSTR3B2.ColumnCount && colNo < 8)
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
                                            if (sCells[i].ToString().Trim() == "" && dgvGSTR3B2.Columns[colNo].Name != "colTax") { dt.Rows[dt.Rows.Count - 1][colNo] = DBNull.Value; }
                                            else
                                            {
                                                if (colNo >= 2 && colNo <= 8)
                                                {
                                                    if (chkCellValue(sCells[i].Trim(), dgvGSTR3B2.Columns[colNo].Name))
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
                                        if (iCol > i)
                                        {
                                            for (int j = colNo; j < dgvGSTR3B2.Columns.Count; j++)
                                            {
                                                #region VALIDATION
                                                if (sCells[i].ToString().Trim() == "") { dt.Rows[dt.Rows.Count - 1][j] = DBNull.Value; }
                                                else
                                                {
                                                    if (j >= 2 && j <= 8)
                                                    {
                                                        if (chkCellValue(sCells[i].Trim(), dgvGSTR3B2.Columns[j].Name))
                                                            dt.Rows[dt.Rows.Count - 1][j] = sCells[i].Trim();
                                                        else
                                                            dt.Rows[dt.Rows.Count - 1][j] = DBNull.Value;
                                                    }
                                                    else { dt.Rows[dt.Rows.Count - 1][j] = sCells[i].Trim(); }
                                                }
                                                #endregion

                                                //dt.Rows[dt.Rows.Count - 1][j + i] = sCells[i].Trim();
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
                                            for (int j = colNo; j < dgvGSTR3B2.Columns.Count; j++)
                                            {
                                                #region VALIDATION
                                                if (sCells[i].ToString().Trim() == "") { dt.Rows[dt.Rows.Count - 1][j] = DBNull.Value; }
                                                else
                                                {
                                                    if (j >= 2 && j <= 8)
                                                    {
                                                        if (chkCellValue(sCells[i].Trim(), dgvGSTR3B2.Columns[j].Name))
                                                            dt.Rows[dt.Rows.Count - 1][j] = sCells[i].Trim();
                                                        else
                                                            dt.Rows[dt.Rows.Count - 1][j] = DBNull.Value;
                                                    }
                                                    else { dt.Rows[dt.Rows.Count - 1][j] = sCells[i].Trim(); }
                                                }
                                                #endregion

                                                //dt.Rows[dt.Rows.Count - 1][j + i] = sCells[i].Trim();
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
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string ColName = dt.Columns[j].ColumnName;
                            if (ColName == "colTaxableValue" || ColName == "colIGST")
                                dt.Rows[i][j] = Utility.DisplayIndianCurrency(Convert.ToString(dt.Rows[i][j]));
                        }
                    }
                    dgvGSTR3B2.DataSource = dt;
                }

                // total calculation 
                string[] colGroup = { "colTaxableValue", "colIGST" };
                GetTotal(colGroup);

                pbGSTR1.Visible = false;

                EnableControls(dgvGSTR3B2);

                #endregion
            }
            catch (Exception ex)
            {
                pbGSTR1.Visible = false;
                EnableControls(dgvGSTR3B2);
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }

        private void dgvGSTR13_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string cNo = dgvGSTR3B2.Columns[e.ColumnIndex].Name;

                if (e.RowIndex >= 0)
                {
                    
                    if (cNo == "colTaxableValue" || cNo == "colIGST")
                    {
                        if (!chkCellValue(Convert.ToString(dgvGSTR3B2.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                            dgvGSTR3B2.Rows[e.RowIndex].Cells[cNo].Value = "";

                        if (chkCellValue(Convert.ToString(dgvGSTR3B2.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                        {
                            dgvGSTR3B2.CellValueChanged -= dgvGSTR13_CellValueChanged;
                            dgvGSTR3B2.Rows[e.RowIndex].Cells[cNo].Value = Utility.DisplayIndianCurrency(Convert.ToString(Math.Round(Convert.ToDecimal(dgvGSTR3B2.Rows[e.RowIndex].Cells[cNo].Value), 2, MidpointRounding.AwayFromZero)));
                            dgvGSTR3B2.CellValueChanged += dgvGSTR13_CellValueChanged;

                            string[] colNo = { dgvGSTR3B2.Columns[e.ColumnIndex].Name };
                            GetTotal(colNo);
                        }
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

        private Boolean chkCellValue(string cellValue, string cNo)
        {
            try
            {
                if (cellValue.Trim() != "")// NOT EQUEL BLANK
                {
                    if (cNo == "colTaxableValue" || cNo == "colIGST")
                    {
                        if (Utility.IsDecimalOrNumber(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colDetails")
                    {
                        if(cellValue=="Supplies made to Unregistered Persons"||cellValue=="Supplies made to Composition Taxable Persons"||cellValue=="Supplies made to UIN holders")
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colPOS")
                    {
                        if (Utility.IsValidStateName(cellValue))
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
                foreach (DataGridViewColumn col in dgvGSTR3B2.Columns)
                {
                    dt.Columns.Add(col.Name.ToString());
                }

                // add datatable column to store file status
                dt.Columns.Add("colFileStatus");

                #endregion

                #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                // create object array to store one row data of main grid
                object[] rowValue = new object[dt.Columns.Count];

                foreach (DataGridViewRow dr in dgvGSTR3B2.Rows)
                {
                    if (dr.Index != dgvGSTR3B2.Rows.Count - 1) // DON'T ADD LAST ROW
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
                    Query = "Delete from SPQR3BInterStateSupplies where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
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
                    _Result = objGSTR13.GSTR3B2BulkEntry(dt, Convert.ToString(CommonHelper.StatusText));

                    if (_Result == 1)
                    {

                        string[] colNo = { "colTaxableValue", "colIGST" };
                        GetTotal(colNo);

                        #region ADD DATATABLE COLUMN

                        // CREATE DATATABLE TO STORE MAIN GRID DATA
                        dt = new DataTable();

                        // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                        foreach (DataGridViewColumn col in dgvGSTR3B2.Columns)
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

                        _Result = objGSTR13.GSTR3B2BulkEntry(dt, "Total");

                        if (_Result == 1)
                        {
                            //DONE
                            MessageBox.Show("Data Saved Successfully …!!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // BIND DATA
                            GetData();
                            // BindData();
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
                    Query = "Delete from SPQR3BInterStateSupplies where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";

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
                if (dgvGSTR3B2.Rows.Count == 1)
                {
                    ckboxHeader.Checked = false;
                    return;
                }
                if (dgvGSTR3B2.CurrentCell.RowIndex == 0 && dgvGSTR3B2.CurrentCell.ColumnIndex == 0)
                {
                    dgvGSTR3B2.CurrentCell = dgvGSTR3B2.Rows[0].Cells["colSrNo"];
                }
                else { dgvGSTR3B2.CurrentCell = dgvGSTR3B2.Rows[0].Cells["colChk"]; }

                // CREATE FLAG FRO DELETE ROWS
                Boolean flgChk = false; Boolean flgSelect = false;

                // CREATE OBJECT OF SELECTED ROW TO DELETE
                List<DataGridViewRow> toDelete = new List<DataGridViewRow>();

                // CHECK THERE ARE RECORD PRESENT IN GRID
                if (dgvGSTR3B2.Rows.Count > 1)
                {
                    // FLAG TRUE IF CHECK ALL SELECTED
                    if (ckboxHeader.Checked)
                        flgChk = true;

                    #region ADD SELECTED ROW TO OBJECT FOR DELETE
                    for (int i = 0; i < dgvGSTR3B2.Rows.Count - 1; i++)
                    {
                        if (dgvGSTR3B2[0, i].Value != null && dgvGSTR3B2[0, i].Value.ToString() != "")
                        {
                            if (Convert.ToBoolean(dgvGSTR3B2[0, i].Value) == true)
                            {
                                // ADD ROW TO OBJECT IF ROW IS SELECTED
                                flgSelect = true;
                                toDelete.Add(dgvGSTR3B2.Rows[i]);
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
                                foreach (DataGridViewColumn col in dgvGSTR3B2.Columns)
                                {
                                    dt.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }

                                // ASSIGN BLANK DATATABLE TO GRID
                                dgvGSTR3B2.DataSource = dt;
                            }
                            else if (flgSelect == true)
                            {
                                // DELETE SELECTED ROW
                                foreach (DataGridViewRow row in toDelete)
                                {
                                    dgvGSTR3B2.Rows.RemoveAt(row.Index);
                                    Application.DoEvents();
                                }
                            }

                            // SEQUANCING MAIN GRID RECORDS
                            for (int i = 0; i < dgvGSTR3B2.Rows.Count - 1; i++)
                            {
                                dgvGSTR3B2.Rows[i].Cells["colSrNo"].Value = Convert.ToString(i + 1);
                            }

                            if (dgvGSTR3B2.Rows.Count == 1)
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
                            dgvGSTR3B2.Columns[0].HeaderText = "Check All";
                            #endregion
                        }
                    }
                    // TOTAL CALCULATION
                    string[] colNo = { "colTaxableValue", "colIGST" };
                    GetTotal(colNo);
                    pbGSTR1.Visible = false;
                }
                else
                {
                    // IF THERE ARE NO RECORD TO DELETE
                    ckboxHeader.Checked = false;
                    dgvGSTR3B2.Columns[0].HeaderText = "Check All";
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
                        dt = (DataTable)dgvGSTR3B2.DataSource;

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
                                DisableControls(dgvGSTR3B2);

                                #region IMPORT EXCEL DATATABLE TO GRID DATATABLE
                                if (dtExcel != null && dtExcel.Rows.Count > 0)
                                {
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
                                foreach (DataGridViewColumn col in dgvGSTR3B2.Columns)
                                {
                                    dt.Columns[col.Index].ColumnName = col.Name.ToString();
                                    col.DataPropertyName = col.Name;
                                }
                                #endregion

                                //ASSIGN DATATABLE TO DATAGRID
                                dgvGSTR3B2.DataSource = dt;

                                // ENABLE MAIN GRID
                                EnableControls(dgvGSTR3B2);
                            }
                            else
                            {
                                // IF THERE ARE NO RECORDS IN MAIN GRID

                                if (dtExcel != null && dtExcel.Rows.Count > 0)
                                {
                                    // IF THERE ARE DATA IN IMPORTED EXCEL FILE

                                    // DISABLE MAIN GRID
                                    DisableControls(dgvGSTR3B2);

                                    #region RENAME DATATABLE COLUMN NAME AS PAR MAIN GRID
                                    foreach (DataGridViewColumn col in dgvGSTR3B2.Columns)
                                    {
                                        dtExcel.Columns[col.Index].ColumnName = col.Name.ToString();
                                        col.DataPropertyName = col.Name;
                                    }
                                    dtExcel.AcceptChanges();

                                    // ASSIGN DATATALE TO GRID
                                    dgvGSTR3B2.DataSource = dtExcel;

                                    // ENABLE MAIN GRID
                                    EnableControls(dgvGSTR3B2);
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
                            string[] colNo = { "colTaxableValue", "colIGST" };
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
                EnableControls(dgvGSTR3B2);
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
                        OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [B2B_DOC$]", con);
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
                        for (int i = 1; i < dgvGSTR3B2.Columns.Count - 1; i++)
                        {
                            flg = false;
                            for (int j = 0; j < dtexcel.Columns.Count; j++)
                            {
                                // CHECK GRID COLUMN IS PRESENT OR NOT IN IMPORTED EXCEL
                                if (dgvGSTR3B2.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim() == dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim())
                                {
                                    // IF GRID COLUMN PRESENT IN EXCEL THEN ITS INDEX AS PAR GRID COLUMN INDEX
                                    flg = true;
                                    dtexcel.Columns[j].SetOrdinal(dgvGSTR3B2.Columns[i].Index - 1);
                                    break;
                                }
                                else if (dgvGSTR3B2.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim() == "" && dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim() == "f2")
                                {
                                    // IF GRID COLUMN PRESENT IN EXCEL THEN ITS INDEX AS PAR GRID COLUMN INDEX
                                    flg = true;
                                    dtexcel.Columns[j].SetOrdinal(dgvGSTR3B2.Columns[i].Index - 1);
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
                        if (dtexcel.Columns.Count >= dgvGSTR3B2.Columns.Count - 2)
                        {
                            for (int i = dtexcel.Columns.Count - 1; i > (dgvGSTR3B2.Columns.Count - 2); i--)
                            {
                                dtexcel.Columns.Remove(dtexcel.Columns[i]);
                            }
                        }
                        dtexcel.AcceptChanges();
                        #endregion

                        #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME
                        foreach (DataGridViewColumn col in dgvGSTR3B2.Columns)
                        {
                            if (col.Index != 0)
                                dtexcel.Columns[col.Index - 1].ColumnName = col.Name.ToString();
                        }
                        #endregion

                        // ADD CHECK BOX COLUMN TO DATATBLE AND MAKE IT FIRST TABLE COLUMN
                        dtexcel.Columns.Add(new DataColumn("colChk"));
                        dtexcel.Columns["colChk"].SetOrdinal(0);
                        dtexcel.AcceptChanges();

                        #region SET COLTAX VALUE AS TRUE/FALSE
                        for (int i = 0; i < dtexcel.Rows.Count; i++)
                        {
                            dtexcel.Rows[i]["colChk"] = "False";
                            dtexcel.Rows[i]["colSrNo"] = i + 1;

                            if (dtexcel.Rows[i]["colDetails"].ToString().Trim().ToLower() == "supplies made to unregistered persons" ||
dtexcel.Rows[i]["colDetails"].ToString().Trim().ToLower() == "supplies made to composition taxable persons" ||
dtexcel.Rows[i]["colDetails"].ToString().Trim().ToLower() == "supplies made to uin holders")
                            { }
                            else
                                dtexcel.Rows[i]["colDetails"] = "";
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
                if (dgvGSTR3B2.Rows.Count > 1)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID

                    pbGSTR1.Visible = true;

                    #region CREATE WORKBOOK AND ASSIGN COLUMNNAME
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook WB = excelApp.Workbooks.Add(Missing.Value);

                    Excel.Worksheet newWS = (Excel.Worksheet)excelApp.Worksheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    newWS.Name = "B2B_DOC";

                    // DELETE UNUSED WORKSHEETS FROM WORKBOOK
                    foreach (Excel.Worksheet ws in WB.Worksheets)
                    {
                        if (ws.Name != "B2B_DOC")
                            ((Excel.Worksheet)WB.Worksheets[ws.Name]).Delete();
                    }

                    // ASSIGN COLUMN HEADER AS PAR THE GRID HEADER
                    for (int i = 1; i < dgvGSTR3B2.Columns.Count; i++)
                    {
                        newWS.Cells[1, i] = dgvGSTR3B2.Columns[i].HeaderText.ToString();

                        // SET COLUMN WIDTH
                        if (i == 1)
                            ((Excel.Range)newWS.Cells[1, i]).ColumnWidth = 7;
                        else if (i >= 2 && i <= 12)
                            ((Excel.Range)newWS.Cells[1, i]).ColumnWidth = 10;
                        else
                            ((Excel.Range)newWS.Cells[1, i]).ColumnWidth = 15;
                    }

                    // GET RANGE AND SET DIFFRENT PROPERTIES
                    Excel.Range headerRange = (Excel.Range)newWS.get_Range((Excel.Range)newWS.Cells[1, 1], (Excel.Range)newWS.Cells[1, dgvGSTR3B2.Columns.Count - 1]);
                    headerRange.WrapText = true;
                    headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Calibri";
                    #endregion

                    #region COPY DATA FROM DATATABLE TO ARRAY

                    // CREATE ARRAY TO HOLD THE DATA OF DATATABLE
                    object[,] arr = new object[dgvGSTR3B2.Rows.Count - 1, dgvGSTR3B2.Columns.Count];

                    // ASSIGN DATA TO ARRAY FROM DATATABLE
                    if (CommonHelper.IsLicence)
                    {
                        // FOR LICENECE ALLOWS TO EXPORT ALL RECORDS
                        for (int i = 0; i < dgvGSTR3B2.Rows.Count - 1; i++)
                        {
                            for (int j = 1; j < dgvGSTR3B2.Columns.Count; j++)
                            {
                                arr[i, j - 1] = Convert.ToString(dgvGSTR3B2.Rows[i].Cells[j].Value);
                            }
                        }
                    }
                    else
                    {
                        // FOR DEMO ALLOW ONLY 100 RECORDS TO EXPORT
                        for (int i = 0; i < dgvGSTR3B2.Rows.Count - 1; i++)
                        {
                            if (i < 100)
                            {
                                for (int j = 1; j < dgvGSTR3B2.Columns.Count; j++)
                                {
                                    arr[i, j - 1] = Convert.ToString(dgvGSTR3B2.Rows[i].Cells[j].Value);
                                }
                            }
                        }
                    }

                    //SET EXCEL RANGE TO PASTE THE DATA
                    Excel.Range top = (Excel.Range)newWS.Cells[2, 1];
                    Excel.Range bottom = (Excel.Range)newWS.Cells[dgvGSTR3B2.Rows.Count, dgvGSTR3B2.Columns.Count];
                    Excel.Range sheetRange = newWS.Range[top, bottom];

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
                        dt = (DataTable)dgvGSTR3B2.DataSource;

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
                                DisableControls(dgvGSTR3B2);

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
                                foreach (DataGridViewColumn col in dgvGSTR3B2.Columns)
                                {
                                    dtCsv.Columns[col.Index].ColumnName = col.Name.ToString();
                                    col.DataPropertyName = col.Name;
                                }
                                dt.AcceptChanges();
                                #endregion

                                // ASSIGN DATATABLE TO GRID
                                dgvGSTR3B2.DataSource = dt;

                                // ENABLE MAIN GRID
                                EnableControls(dgvGSTR3B2);
                            }
                            else
                            {
                                // IF THERE ARE NO RECORDS IN MAIN GRID

                                if (dtCsv != null && dtCsv.Rows.Count > 0)
                                {
                                    // IF THERE ARE RECORD PRESENT IN IMPORT FILE

                                    // DISABLE MAIN GRID
                                    DisableControls(dgvGSTR3B2);

                                    #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME AND ASSIGN TO MAIN GRID
                                    foreach (DataGridViewColumn col in dgvGSTR3B2.Columns)
                                    {
                                        dtCsv.Columns[col.Index].ColumnName = col.Name.ToString();
                                        col.DataPropertyName = col.Name;
                                    }
                                    dtCsv.AcceptChanges();

                                    // ASSIGN DATATABLE TO GRID
                                    dgvGSTR3B2.DataSource = dtCsv;

                                    // ENABLE CONTROL
                                    EnableControls(dgvGSTR3B2);
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
                EnableControls(dgvGSTR3B2);
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
                    for (int i = 1; i < dgvGSTR3B2.Columns.Count; i++)
                    {
                        Boolean flg = false;
                        for (int j = 0; j < csvData.Columns.Count; j++)
                        {
                            // CHECK GRID COLUMN IS PRESENT OR NOT IN IMPORTED EXCEL
                            if (dgvGSTR3B2.Columns[i].HeaderText.Replace(" ", "") == csvData.Columns[j].ColumnName.Replace(" ", "").Trim())
                            {
                                // IF GRID COLUMN PRESENT IN EXCEL THEN ITS INDEX AS PAR GRID COLUMN INDEX
                                flg = true;
                                csvData.Columns[j].SetOrdinal(dgvGSTR3B2.Columns[i].Index - 1);
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
                    if (csvData.Columns.Count >= dgvGSTR3B2.Columns.Count - 2)
                    {
                        for (int i = csvData.Columns.Count - 1; i > (dgvGSTR3B2.Columns.Count - 2); i--)
                        {
                            csvData.Columns.Remove(csvData.Columns[i]);
                        }
                    }
                    #endregion

                    #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME
                    foreach (DataGridViewColumn col in dgvGSTR3B2.Columns)
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

                        if (csvData.Rows[i]["colNatureOfDocument"].ToString().Trim().ToLower() == "invoice for outward supply" ||
csvData.Rows[i]["colNatureOfDocument"].ToString().Trim().ToLower() == "invoice for inward supply from unregistered person" ||
csvData.Rows[i]["colNatureOfDocument"].ToString().Trim().ToLower() == "revised invoice" ||
csvData.Rows[i]["colNatureOfDocument"].ToString().Trim().ToLower() == "debit note" ||
csvData.Rows[i]["colNatureOfDocument"].ToString().Trim().ToLower() == "credit note" ||
csvData.Rows[i]["colNatureOfDocument"].ToString().Trim().ToLower() == "receipt voucher" ||
csvData.Rows[i]["colNatureOfDocument"].ToString().Trim().ToLower() == "payment voucher" ||
csvData.Rows[i]["colNatureOfDocument"].ToString().Trim().ToLower() == "refund voucher" ||
csvData.Rows[i]["colNatureOfDocument"].ToString().Trim().ToLower() == "delivery challan for job work" ||
csvData.Rows[i]["colNatureOfDocument"].ToString().Trim().ToLower() == "delivery challan for supply on approval" ||
csvData.Rows[i]["colNatureOfDocument"].ToString().Trim().ToLower() == "delivery challan in case of liquid gas" ||
csvData.Rows[i]["colNatureOfDocument"].ToString().Trim().ToLower() == "delivery challan in cases other than by way of supply (excluding at S no. 9 to 11)")
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
                if (dgvGSTR3B2.Rows.Count > 1)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID

                    pbGSTR1.Visible = true;

                    string csv = string.Empty;
                    // CREATE DATATABLE AND GET GRID DATA
                    DataTable dt = new DataTable();
                    dt = (DataTable)dgvGSTR3B2.DataSource;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        #region ASSIGN COLUMN NAME TO CSV STRING
                        for (int i = 1; i < dgvGSTR3B2.Columns.Count; i++)
                        {
                            csv += dgvGSTR3B2.Columns[i].HeaderText + ',';
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

        #region PDF TRANSACTIONS UPDATED BY Vipul

        public void ExportPDF()
        {
            try
            {
                pbGSTR1.Visible = true;

                #region CREATING ITEXTSHARP TABLE FROM THE DATATABLE DATA AND ASSIGNING TABLE HEADER
                PdfPTable pdfTable = new PdfPTable(dgvGSTR3B2.ColumnCount - 2);
                pdfTable.DefaultCell.Padding = 3;
                pdfTable.WidthPercentage = 100;
                pdfTable.DefaultCell.BorderWidth = 0;
                iTextSharp.text.Font fontHeader = iTextSharp.text.FontFactory.GetFont("Calibri", 6);

                // ADD HEADER TO PDF TABLE
                pdfTable = AssignHeader(pdfTable, "3.2 Of supplies shown in 3.1 (a) above,details of inter -State supplies made to unregistered persons, Composition taxable persons and UIN holders");
                #endregion

                #region ADDING HEADER ROW
                int i = 0;

                #region HEADER1
                PdfPCell celHeader1 = new PdfPCell();

                celHeader1 = new PdfPCell(new Phrase("", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Place of Supply (State/UT)", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Total Taxable Value", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Amount of Integerated Tax", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                pdfTable.CompleteRow();
                #endregion

                Application.DoEvents();
                #endregion

                #region ADDING COLUMN NUMBER
                i = 0;
                foreach (DataGridViewColumn column in dgvGSTR3B2.Columns)
                {
                    if (i != 0 && i != 5)
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
                    foreach (DataGridViewRow row in dgvGSTR3B2.Rows)
                    {
                        i = 0;

                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            if (cell.Value != null && i != 0 && i != 1)
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
                    foreach (DataGridViewRow row in dgvGSTR3B2.Rows)
                    {
                        if (sj < 100)
                        {
                            i = 0;
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                if (cell.Value != null && i != 0 && i != 1)
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
                ce1.Colspan = dgvGSTR3B2.Columns.Count - 1;
                ce1.VerticalAlignment = Element.ALIGN_CENTER;
                ce1.HorizontalAlignment = Element.ALIGN_LEFT;
                ce1.BackgroundColor = new iTextSharp.text.BaseColor(197, 223, 197);
                ce1.BorderWidth = 0;
                pdfTable.AddCell(ce1);

                iTextSharp.text.Font FigToRs = FontFactory.GetFont("Calibri", 10, iTextSharp.text.Font.BOLD);
                PdfPCell ceHeader2 = new PdfPCell(new Phrase("(figures in Rs)", FigToRs));
                ceHeader2.Colspan = dgvGSTR3B2.Columns.Count - 1;
                ceHeader2.VerticalAlignment = Element.ALIGN_CENTER;
                ceHeader2.HorizontalAlignment = Element.ALIGN_RIGHT;
                ceHeader2.BorderWidth = 0;
                pdfTable.AddCell(ceHeader2);

                PdfPCell ce2 = new PdfPCell(new Phrase(" "));
                ce2.Colspan = dgvGSTR3B2.Columns.Count - 1;
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

        #region JSON TRANSACTIONS

        public class UnregDetail
        {
            public string pos { get; set; }
            public double txval { get; set; }
            public double iamt { get; set; }
        }

        public class CompDetail
        {
            public string pos { get; set; }
            public double txval { get; set; }
            public double iamt { get; set; }
        }

        public class UinDetail
        {
            public string pos { get; set; }
            public double txval { get; set; }
            public double iamt { get; set; }
        }

        public class InterSup
        {
            public List<UnregDetail> unreg_details { get; set; }
            public List<CompDetail> comp_details { get; set; }
            public List<UinDetail> uin_details { get; set; }
        }

        public class RootObject
        {
            public InterSup inter_sup { get; set; }
        }

        public void JSONCreator()
        {
            #region Second With Object GSTR3B1
            try
            {
                RootObject ObjJson = new RootObject();

                List<DataGridViewRow> supInterInvoiceList = dgvGSTR3B2.Rows
                          .OfType<DataGridViewRow>()
                          .ToList();

                InterSup objInterSup = new InterSup();
                List<UnregDetail> objuregDet = new List<UnregDetail>();
                List<CompDetail> objcompDet = new List<CompDetail>();
                List<UinDetail> objuinDet = new List<UinDetail>();

                for (int i = 0; i < supInterInvoiceList.Count - 1; i++)
                {


                    if (supInterInvoiceList[i].Cells["colDetails"].Value.ToString() == "Supplies Made to Unregistered Persons")
                    {
                        UnregDetail uregDet = new UnregDetail();
                        uregDet.pos = Convert.ToString(supInterInvoiceList[i].Cells["colPOS"].Value.ToString());
                        uregDet.txval = Convert.ToDouble(supInterInvoiceList[i].Cells["colTaxableValue"].Value.ToString());
                        uregDet.iamt = Convert.ToDouble(supInterInvoiceList[i].Cells["colIGST"].Value.ToString());
                        objuregDet.Add(uregDet);
                        objInterSup.unreg_details = objuregDet;

                    }
                    if (supInterInvoiceList[i].Cells["colDetails"].Value.ToString() == "Supplies Made to Composition Taxable Persons")
                    {
                        CompDetail compDet = new CompDetail();

                        compDet.pos = Convert.ToString(supInterInvoiceList[i].Cells["colPOS"].Value.ToString());
                        compDet.txval = Convert.ToDouble(supInterInvoiceList[i].Cells["colTaxableValue"].Value.ToString());
                        compDet.iamt = Convert.ToDouble(supInterInvoiceList[i].Cells["colIGST"].Value.ToString());
                        objcompDet.Add(compDet);
                        objInterSup.comp_details = objcompDet;
                    }
                    if (supInterInvoiceList[i].Cells["colDetails"].Value.ToString() == "Supplies Made to UIN holders")
                    {
                        UinDetail uinDet = new UinDetail();
                        uinDet.txval = Convert.ToDouble(supInterInvoiceList[i].Cells["colTaxableValue"].Value.ToString());
                        objuinDet.Add(uinDet);
                        objInterSup.uin_details = objuinDet;
                    }

                }
                ObjJson.inter_sup = objInterSup;

                #region File Save
                JavaScriptSerializer objScript = new JavaScriptSerializer();

                var settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.DefaultValueHandling = DefaultValueHandling.Ignore;

                objScript.MaxJsonLength = 2147483647;

                string FinalJson = JsonConvert.SerializeObject(ObjJson, settings);

                SaveFileDialog save = new SaveFileDialog();
                save.FileName = "3B2.json";
                save.Filter = "Json File | *.json";
                if (save.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter writer = new StreamWriter(save.OpenFile());
                    writer.WriteLine(FinalJson);
                    writer.Dispose();
                    writer.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
                #endregion
        }

            #endregion
        #endregion

        public void SetGridViewColor()
        {
            try
            {
                // DO NOT ALLOW TO AUTO GENERATE COLUMNS
                dgvGSTR3B2.AutoGenerateColumns = false;

                // SET HEIGHT WIDTH OF FORM
                this.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.611));
                this.Height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.68));

                // SET WIDTH OF HEADER, MAIN AND TOTAL GRID
                this.panel1.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.594));
                this.dgvGSTR3B2.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.594));
                this.TotaldgvGSTR13.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.594));

                // SET HEIGHT OF MAIN GRID
                this.dgvGSTR3B2.Height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.56));
                this.TotaldgvGSTR13.Height = 29;

                // SET LOCATION OF HEADER,LOADING PIC, CHECKBOX AND MAIN AND TOTAL GRID
                //this.panel1.Location = new System.Drawing.Point(12, 5);
                //this.dgvGSTR3B2.Location = new System.Drawing.Point(12, Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.063)));
                //this.TotaldgvGSTR13.Location = new System.Drawing.Point(12, Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.63)));
                //this.ckboxHeader.Location = new System.Drawing.Point(29, Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.137)));
                //this.pbGSTR1.Location = new System.Drawing.Point(Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.45)), Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.30)));

                // SET MAIN GRID PROPERTY
                dgvGSTR3B2.EnableHeadersVisualStyles = false;
                dgvGSTR3B2.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(23, 196, 187);
                dgvGSTR3B2.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

                this.dgvGSTR3B2.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                this.dgvGSTR3B2.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                this.dgvGSTR3B2.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

                foreach (DataGridViewColumn column in dgvGSTR3B2.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.Automatic;
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

        private void dgvGSTR13_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                // SET INDEX OF USER ADDED ROW IN MAIN GRID
                dgvGSTR3B2.Rows[e.Row.Index - 1].Cells["colSrNo"].Value = e.Row.Index;
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
                if (dgvGSTR3B2.Rows.Count > 1)
                {
                    pbGSTR1.Visible = true;

                    if (ckboxHeader.Checked)
                    {
                        // IF CHECK BOX IS CHECKED

                        // SET CHECK BOX COLUMN VALUE AS TRUE
                        for (int i = 0; i < dgvGSTR3B2.Rows.Count - 1; i++)
                        {
                            dgvGSTR3B2.Rows[i].Cells[0].Value = "True";
                            Application.DoEvents();
                        }

                        // CHANGE HEADER TEXT AND WIDTH OF COLUMN AND POSITION OF CHECK BOX OF CHECK ALL COLUMN
                        dgvGSTR3B2.Columns[0].HeaderText = "Uncheck All";
                    }
                    else if (ckboxHeader.Checked == false)
                    {
                        // IF CHECK BOX IS UNCHECKED

                        // SET CHECK BOX COLUMN VALUE AS FALSE
                        for (int i = 0; i < dgvGSTR3B2.Rows.Count - 1; i++)
                        {
                            dgvGSTR3B2.Rows[i].Cells[0].Value = "False";
                            Application.DoEvents();
                        }

                        // CHANGE HEADER TEXT AND WIDTH OF COLUMN AND POSITION OF CHECK BOX OF CHECK ALL COLUMN
                        dgvGSTR3B2.Columns[0].HeaderText = "Check All";
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
                if (e.ColumnIndex == 0 && dgvGSTR3B2.Rows.Count > 1)
                {
                    // CHECK AND UNCHECK CHECK BOX OF HEADER FOR SELECTING AND UNSELECTING ALL RECORDS
                    if (dgvGSTR3B2.Columns[e.ColumnIndex].HeaderText == "Check All")
                        ckboxHeader.Checked = true;
                    else if (dgvGSTR3B2.Columns[e.ColumnIndex].HeaderText == "Uncheck All")
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
                this.TotaldgvGSTR13.HorizontalScrollingOffset = this.dgvGSTR3B2.HorizontalScrollingOffset;
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
                this.dgvGSTR3B2.HorizontalScrollingOffset = this.TotaldgvGSTR13.HorizontalScrollingOffset;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void TotaldgvGSTR13_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.dgvGSTR3B2.ClearSelection();
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


        private void btnGSTR1_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("colChk");
                dt.Columns.Add("colSrNo");
                dt.Columns.Add("colDetails");
                dt.Columns.Add("colPOS");
                dt.Columns.Add("colTaxableValue");
                dt.Columns.Add("colIGST");
                dt.AcceptChanges();

                #region B2CS
                string Query = "Select Fld_POS,SUM(Replace(Fld_TaxableValue,',','')) AS colTaxableValue, SUM(Replace(Fld_IGST,',','')) AS colIGST from SPQR1B2CS where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear='" + CommonHelper.ReturnYear + "' AND Fld_FileStatus != 'Total' AND Fld_IGST > '0.00' Group By Fld_POS;";
                DataTable dtB2CS = new DataTable();
                dtB2CS = objGSTR5.GetDataGSTR1(Query);
                if (dtB2CS != null && dtB2CS.Rows.Count > 0)
                {
                    int srNo = 0;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        srNo = dt.Rows.Count;
                    }

                    for (int i = 0; i < dtB2CS.Rows.Count; i++)
                    {
                        srNo = srNo + 1;
                        string colTaxableValue = "";
                        if (dtB2CS.Rows[i]["colTaxableValue"] != null && Convert.ToString(dtB2CS.Rows[i]["colTaxableValue"]) != "")
                        {
                            colTaxableValue = Utility.Round(Convert.ToString(dtB2CS.Rows[i]["colTaxableValue"]));
                        }
                        string colIGST = "";
                        if (dtB2CS.Rows[i]["colIGST"] != null && Convert.ToString(dtB2CS.Rows[i]["colIGST"]) != "")
                        {
                            colIGST = Utility.Round(Convert.ToString(dtB2CS.Rows[i]["colIGST"]));
                        }
                        dt.Rows.Add(false, srNo, "Supplies made to Unregistered Persons", Convert.ToString(dtB2CS.Rows[i]["Fld_POS"]), colTaxableValue, colIGST);
                    }
                }
                #endregion

                #region B2CL
                Query = "Select Fld_POS,SUM(Replace(Fld_TaxableValue,',','')) AS colTaxableValue, SUM(Replace(Fld_IGST,',','')) AS colIGST from SPQR1B2CL where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear='" + CommonHelper.ReturnYear + "' AND Fld_FileStatus != 'Total' AND Fld_IGST > '0.00' Group By Fld_POS;";
                DataTable dtB2CL = new DataTable();
                dtB2CL = objGSTR5.GetDataGSTR1(Query);
                if (dtB2CL != null && dtB2CL.Rows.Count > 0)
                {
                    int srNo = 0;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        srNo = dt.Rows.Count;
                    }

                    for (int i = 0; i < dtB2CL.Rows.Count; i++)
                    {
                        string colTaxableValue = "";
                        if (dtB2CL.Rows[i]["colTaxableValue"] != null && Convert.ToString(dtB2CL.Rows[i]["colTaxableValue"]) != "")
                        {
                            colTaxableValue = Utility.Round(Convert.ToString(dtB2CL.Rows[i]["colTaxableValue"]));
                        }
                        string colIGST = "";
                        if (dtB2CL.Rows[i]["colIGST"] != null && Convert.ToString(dtB2CL.Rows[i]["colIGST"]) != "")
                        {
                            colIGST = Utility.Round(Convert.ToString(dtB2CL.Rows[i]["colIGST"]));
                        }

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            bool isSamePOS = false;
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                if (Convert.ToString(dt.Rows[j]["colPOS"]) == Convert.ToString(dtB2CL.Rows[i]["Fld_POS"]))
                                {
                                    isSamePOS = true;

                                    if (Convert.ToString(dt.Rows[i]["colTaxableValue"]) != "" && colTaxableValue != "")
                                    {
                                        dt.Rows[j]["colTaxableValue"] = Utility.Round(Convert.ToString(Convert.ToDecimal(dt.Rows[j]["colTaxableValue"]) + Convert.ToDecimal(colTaxableValue)));
                                    }
                                    else
                                    {
                                        dt.Rows[j]["colTaxableValue"] = colTaxableValue;
                                    }

                                    if (Convert.ToString(dt.Rows[i]["colIGST"]) != "" && colIGST != "")
                                    {
                                        dt.Rows[j]["colIGST"] = Utility.Round(Convert.ToString(Convert.ToDecimal(dt.Rows[j]["colIGST"]) + Convert.ToDecimal(colIGST)));
                                    }
                                    else
                                    {
                                        dt.Rows[j]["colIGST"] = colIGST;
                                    }
                                }
                            }

                            if (isSamePOS == false)
                            {
                                srNo = srNo + 1;
                                dt.Rows.Add(false, srNo, "Supplies made to Unregistered Persons", Convert.ToString(dtB2CL.Rows[i]["Fld_POS"]), colTaxableValue, colIGST);
                            }
                        }
                        else
                        {
                            srNo = srNo + 1;
                            dt.Rows.Add(false, srNo, "Supplies made to Unregistered Persons", Convert.ToString(dtB2CL.Rows[i]["Fld_POS"]), colTaxableValue, colIGST);
                        }
                    }
                }
                #endregion

                if (dt != null && dt.Rows.Count > 0)
                {
                    // assign datatable to main grid
                    this.dgvGSTR3B2.ClearSelection();

                    //RENAME DATATABLE COLUMN NAME TO DATAGRIDVIEW COLUMN NAME
                    foreach (DataGridViewColumn col in dgvGSTR3B2.Columns)
                    {
                        dt.Columns[col.Index].ColumnName = col.Name.ToString();
                        col.DataPropertyName = col.Name;
                    }
                    dt.AcceptChanges();

                    dgvGSTR3B2.DataSource = dt;

                    string[] colNo = { "colTaxableValue", "colIGST" };
                    GetTotal(colNo);

                    MessageBox.Show("Get Data From GSTR1 Succesfully!");
                }
                else
                {
                    MessageBox.Show("No Data Availabe in GSTR1!");
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
    }
}
