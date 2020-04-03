using Newtonsoft.Json;
using SPEQTAGST.BAL;
using SPEQTAGST.BAL.M264r1;
using SPEQTAGST.Usermain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace SPEQTAGST.xasjbr1
{
    public partial class SPQGSTR1NilRated : Form
    {
        r1Publicclass objGSTR13 = new r1Publicclass();

        public SPQGSTR1NilRated()
        {
            InitializeComponent();

            GetData();
            BindData();

            // total calculation
            string[] colNo = { "colTotal", "colCancelled", "colIssued" };
            //GetTotal(colNo);

            SetGridViewColor();
            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName(CommonHelper.ReturnName);

            dgvmain.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dgvmain.EnableHeadersVisualStyles = false;
            dgvmain.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);
            dgvmain.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvmain.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            dgvGSTR1_NilRated.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dgvGSTR1_NilRated.EnableHeadersVisualStyles = false;
            dgvGSTR1_NilRated.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);
            dgvGSTR1_NilRated.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvGSTR1_NilRated.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void GetData()
        {
            try
            {
                // create datatable to store database data
                DataTable dtMulti = new DataTable();
                string Query = "Select * from SPQR1NilRatedMulti where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
                Application.DoEvents();

                // get data from database
                dtMulti = objGSTR13.GetDataGSTR1(Query);

                if (dtMulti != null && dtMulti.Rows.Count > 0)
                {
                    //// assign file status filed value
                    //if (Convert.ToString(dtMulti.Rows[0]["Fld_FileStatus"]).ToLower() == "draft")
                    //    ((MDI)Application.OpenForms["SPQMDI"]).SetFileStatus(1);
                    //else if (Convert.ToString(dtMulti.Rows[0]["Fld_FileStatus"]).ToLower() == "completed")
                    //    ((MDI)Application.OpenForms["SPQMDI"]).SetFileStatus(2);
                    //else if (Convert.ToString(dtMulti.Rows[0]["Fld_FileStatus"]).ToLower() == "not-completed")
                    //    ((MDI)Application.OpenForms["SPQMDI"]).SetFileStatus(3);

                    // remove last column (month)
                    dtMulti.Columns.Remove(dtMulti.Columns[dtMulti.Columns.Count - 1]);
                    // remove last column (file status)
                    dtMulti.Columns.Remove(dtMulti.Columns[dtMulti.Columns.Count - 1]);
                    dtMulti.Columns.Remove(dtMulti.Columns[dtMulti.Columns.Count - 1]);
                    // remove first column (field id)
                    dtMulti.Columns.Remove(dtMulti.Columns[0]);
                    dtMulti.Columns.Add(new DataColumn("colChk"));
                    dtMulti.Columns["colChk"].SetOrdinal(0);
                    #region GOODS GRID
                    //RENAME DATATABLE COLUMN NAME TO DATAGRIDVIEW COLUMN NAME
                    foreach (DataGridViewColumn col in dgvmain.Columns)
                    {
                        dtMulti.Columns[col.Index].ColumnName = col.Name.ToString();
                        col.DataPropertyName = col.Name;
                    }

                    for (int i = 0; i < dtMulti.Rows.Count; i++)
                    {
                        for (int j = 0; j < dtMulti.Columns.Count; j++)
                        {
                            string ColName = dtMulti.Columns[j].ColumnName;
                            if (ColName == "colInvoiceValue")
                                dtMulti.Rows[i][j] = Utility.DisplayIndianCurrency(Convert.ToString(dtMulti.Rows[i][j]));
                        }
                    }

                    dtMulti.AcceptChanges();

                    dgvmain.DataSource = dtMulti;
                    #endregion
                }


                // create datatable to store database data
                DataTable dt = new DataTable();
                Query = "Select * from SPQR1NilRated where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
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
                    dt.Columns.Remove(dt.Columns[dt.Columns.Count - 1]);
                    // remove first column (field id)
                    dt.Columns.Remove(dt.Columns[0]);


                    #region GOODS GRID
                    //RENAME DATATABLE COLUMN NAME TO DATAGRIDVIEW COLUMN NAME
                    foreach (DataGridViewColumn col in dgvGSTR1_NilRated.Columns)
                    {
                        dt.Columns[col.Index].ColumnName = col.Name.ToString();
                        col.DataPropertyName = col.Name;
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string ColName = dt.Columns[j].ColumnName;
                            if (ColName == "colNilRatedSupply" || ColName == "colExempted" || ColName == "colNonGSTSupplies")
                                dt.Rows[i][j] = Utility.DisplayIndianCurrency(Convert.ToString(dt.Rows[i][j]));
                        }
                    }

                    dt.AcceptChanges();

                    dgvGSTR1_NilRated.DataSource = dt;
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
            try
            {
                if (dgvGSTR1_NilRated.Rows.Count <= 1)
                {
                    DataTable dt = new DataTable();

                    // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                    foreach (DataGridViewColumn col in dgvGSTR1_NilRated.Columns)
                    {
                        dt.Columns.Add(col.Name.ToString());
                        col.DataPropertyName = col.Name;
                    }
                    dt.AcceptChanges();

                    DataRow dr = dt.NewRow();
                    dr["colSrNo"] = "1";
                    dr["colNatureOfDocument"] = "Inter-State supplies to registered persons";
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr["colSrNo"] = "2";
                    dr["colNatureOfDocument"] = "Intra-State supplies to registered persons";
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr["colSrNo"] = "3";
                    dr["colNatureOfDocument"] = "Inter-State supplies to unregistered persons";
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr["colSrNo"] = "4";
                    dr["colNatureOfDocument"] = "Intra-State supplies to unregistered persons";
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();

                    // assign datatable to main grid
                    dgvGSTR1_NilRated.DataSource = dt;

                    DataGridViewRow row = this.dgvGSTR1_NilRated.RowTemplate;
                    row.MinimumHeight = 30;
                }
                else
                {
                    DataGridViewRow row = this.dgvGSTR1_NilRated.RowTemplate;
                    row.MinimumHeight = 30;
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

        private void dgvmain_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                pbGSTR1.Visible = true;

                // IF USER WANTS TO DELETE DATA
                if (e.KeyCode == Keys.Delete)
                {
                    #region DELETE SELECTED CELLS
                    try
                    {
                        // CHECK PRESENT RECORDS IN MAIN GRID
                        if (dgvmain.Rows.Count > 0)
                        {
                            // DELETE SELECTED CELL IN GRID
                            foreach (DataGridViewCell oneCell in dgvmain.SelectedCells)
                            {
                                //oneCell.ValueType = typeof(string);
                                // CHECK BOX COLUMN (0,17) DATA DO NOT DELETE
                                if (oneCell.Selected)
                                {
                                    // dgvmain.Columns[oneCell.ColumnIndex].ValueType = typeof(string);
                                    oneCell.ValueType.Name.ToString();
                                    oneCell.ValueType.FullName.ToString();
                                    if (oneCell.ValueType.Name.ToString() == "Double")
                                    {
                                        oneCell.Value = Convert.ToDecimal(DBNull.Value);
                                    }
                                    else
                                        oneCell.Value = "";
                                }
                                Application.DoEvents();
                            }

                            DataTable dt = new DataTable();

                            #region ADD DATATABLE COLUMN

                            foreach (DataGridViewColumn col in dgvmain.Columns)
                            {
                                dt.Columns.Add(col.Name.ToString());
                                col.DataPropertyName = col.Name;
                            }
                            #endregion

                            #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                            // CREATE OBJECT ARRAY TO STORE ONE ROW DATA OF MAIN GRID
                            object[] rowValue = new object[dt.Columns.Count];

                            foreach (DataGridViewRow dr in dgvmain.Rows)
                            {
                                if (dr.Index != dgvmain.Rows.Count - 1) // DON'T ADD LAST ROW
                                {
                                    // SET CHECK BOX VALUE
                                    rowValue[0] = "False";
                                    for (int i = 1; i < dr.Cells.Count; i++)
                                    {
                                        rowValue[i] = Convert.ToString(dr.Cells[i].Value);
                                    }
                                    rowValue[dr.Cells.Count - 1] = Convert.ToString(dr.Cells[dr.Cells.Count - 1].Value);

                                    // ADD ARRAY OF GRID ROW VALUE TO DATATABLE AS ROW
                                    dt.Rows.Add(rowValue);
                                }

                            }
                            dt.AcceptChanges();
                            #endregion

                            BindGridDataToMainGrid(dt);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    #endregion
                }

                // IF USER WANTS TO PASTE DATA
                if (e.KeyCode == Keys.V)
                {
                    #region PAST FROM EXCELL SHEET

                    // GET COPIED DATA TO STRING
                    string s = Clipboard.GetText();
                    string[] lines = s.Split('\n');
                    int iRow = 0, iCol = 0;

                    #region PAST ON SELECTED CELLS ONLY
                    if (dgvmain.RowCount > 0) // IF GRID IS NOT NULL AND PAST ONLY ON SELECTED CELLS
                    {
                        foreach (DataGridViewCell oneCell in dgvmain.SelectedCells)
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
                    gRowNo = dgvmain.Rows.Count - 1;

                    foreach (string line in lines)
                    {
                        pbGSTR1.Visible = true;

                        if (line != "")
                        {
                            DisableControls(dgvmain);
                            int no = tmp;

                            DataTable dtDGV = new DataTable();

                            #region ADD DATATABLE COLUMN

                            foreach (DataGridViewColumn col in dgvmain.Columns)
                            {
                                dtDGV.Columns.Add(col.Name.ToString());
                                col.DataPropertyName = col.Name;
                            }
                            #endregion

                            #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                            // CREATE OBJECT ARRAY TO STORE ONE ROW DATA OF MAIN GRID
                            object[] rowValue = new object[dtDGV.Columns.Count];

                            foreach (DataGridViewRow dr in dgvmain.Rows)
                            {
                                if (dr.Index != dgvmain.Rows.Count - 1) // DON'T ADD LAST ROW
                                {
                                    // SET CHECK BOX VALUE
                                    rowValue[0] = "False";
                                    for (int i = 1; i < dr.Cells.Count; i++)
                                    {
                                        rowValue[i] = Convert.ToString(dr.Cells[i].Value);
                                    }
                                    rowValue[dr.Cells.Count - 1] = Convert.ToString(dr.Cells[dr.Cells.Count - 1].Value);

                                    // ADD ARRAY OF GRID ROW VALUE TO DATATABLE AS ROW
                                    dtDGV.Rows.Add(rowValue);
                                }

                            }
                            dtDGV.AcceptChanges();
                            #endregion

                            //GridRowPaste(dtDGV, tmp, iCol, lines);
                            GridRowPaste(dtDGV, iRow, iCol, lines);
                            return;
                        }

                        tmp++;
                    }
                    #endregion

                    // enable control
                    EnableControls(dgvmain);
                }

                // DISABLE CNTR + A FOR SELECT WHOLE GRID ROW OR CNTR + MINUS FOR DELETE WHOLE ROW OR SHIFT + SPACE FOR SELECT WHOLE ROW OR CNTR + F4 FOR CLOSE APPLICATION
                if ((e.Control && (e.KeyCode == Keys.A || e.KeyCode == Keys.Subtract)) || (e.KeyCode == Keys.Space && Control.ModifierKeys == Keys.Shift) || (e.Alt && e.KeyCode == Keys.F4))
                {
                    e.Handled = true;
                }

                pbGSTR1.Visible = false;
            }
            catch (Exception ex)
            {
                EnableControls(dgvmain);
                pbGSTR1.Visible = false;
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                DisableControls(dgvmain);

                #region SET DATATABLE
                int cnt = lineNo, colNo = 0, rowNo = 0;

                // ASSIGN GRID DATA TO DATATABLE
                DataTable dt = dtDGV;

                if (dt == null || dt.Rows.Count == 0)
                {
                    // IF NO RECORD IN GRID THEN CREATE NEW DATATABLE
                    dt = new DataTable();

                    // ADD COLUMN AS PAR MAIN GRID AND SET DATA ACCESS PROPERTY
                    foreach (DataGridViewColumn col in dgvmain.Columns)
                    {
                        dt.Columns.Add(col.Name.ToString());
                        col.DataPropertyName = col.Name;
                    }
                }
                #endregion

                foreach (string line in lines)
                {
                    colNo = 0;
                    if (line != "" && line.Length > 0)
                    {
                        if (cnt > dt.Rows.Count - 1)
                        {
                            DataRow dtRow = dt.NewRow();
                            dt.Rows.Add(dtRow);
                            rowNo = dt.Rows.Count - 1;
                        }
                        else
                        { rowNo = cnt; }

                        #region ROW PASTE
                        string[] sCells = line.Split('\t');

                        for (int i = 0; i < sCells.GetLength(0); ++i)
                        {
                            if (iCol + i < dt.Columns.Count && colNo < 7)
                            {
                                if (iCol == 0)
                                    colNo = iCol + i + 1;
                                else
                                    colNo = iCol + i;

                                sCells[i] = sCells[i].Trim().Replace(",", "");
                                if (colNo != 0)
                                {
                                    if (dt.Columns[colNo].ColumnName != "colChk" && dt.Columns[colNo].ColumnName != "colStateList")
                                    {
                                        #region VALIDATION
                                        if (sCells[i].ToString().Trim() == "") { dt.Rows[rowNo][colNo] = DBNull.Value; }
                                        else
                                        {
                                            if (colNo >= 1 && colNo <= 7)
                                            {
                                                if (chkCellValue(sCells[i].Trim(), dgvmain.Columns[colNo].Name))
                                                {
                                                    if (dgvmain.Columns[colNo].Name == "colType")
                                                        dt.Rows[dt.Rows.Count - 1][colNo] = Utility.StrNilRatedType(sCells[i]);
                                                    else
                                                        dt.Rows[dt.Rows.Count - 1][colNo] = sCells[i].Trim();
                                                }
                                                else
                                                {
                                                    if (dgvmain.Columns[colNo].Name == "colPlaceSupply")
                                                        dt.Rows[rowNo][colNo] = "";
                                                    else
                                                        dt.Rows[rowNo][colNo] = DBNull.Value;
                                                }
                                            }
                                            else { dt.Rows[rowNo][colNo] = sCells[i].Trim(); }
                                        }
                                        #endregion
                                    }
                                }
                                else
                                {
                                    #region REST PART
                                    if (iCol > i)
                                    {
                                        for (int j = colNo; j < dgvmain.Columns.Count; j++)
                                        {
                                            #region VALIDATION
                                            if (sCells[i].ToString().Trim() == "") { dt.Rows[rowNo][j] = DBNull.Value; }
                                            else
                                            {
                                                if (j >= 1 && j <= 7)
                                                {
                                                    if (chkCellValue(sCells[i].Trim(), dgvmain.Columns[j].Name))
                                                    {
                                                        if (dgvmain.Columns[j].Name == "colType")
                                                            dt.Rows[dt.Rows.Count - 1][j] = Utility.StrNilRatedType(sCells[i]);
                                                        else
                                                            dt.Rows[dt.Rows.Count - 1][j] = sCells[i].Trim();
                                                    }
                                                    else
                                                    {
                                                        if (dgvmain.Columns[colNo].Name == "colPlaceSupply")
                                                            dt.Rows[rowNo][j] = "";
                                                        else
                                                            dt.Rows[rowNo][j] = DBNull.Value;
                                                    }
                                                }
                                                else { dt.Rows[rowNo][j] = sCells[i].Trim(); }
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
                                        for (int j = colNo; j < dgvmain.Columns.Count; j++)
                                        {
                                            #region VALIDATION
                                            if (sCells[i].ToString().Trim() == "") { dt.Rows[rowNo][j] = DBNull.Value; }
                                            else
                                            {
                                                if (j >= 1 && j <= 7)
                                                {
                                                    if (chkCellValue(sCells[i].Trim(), dgvmain.Columns[j].Name))
                                                    {
                                                        if (dgvmain.Columns[j].Name == "colType")
                                                            dt.Rows[dt.Rows.Count - 1][j] = Utility.StrNilRatedType(sCells[i]);
                                                        else
                                                            dt.Rows[dt.Rows.Count - 1][j] = sCells[i].Trim();
                                                    }
                                                    else
                                                    {
                                                        if (dgvmain.Columns[colNo].Name == "colPlaceSupply")
                                                            dt.Rows[rowNo][j] = "";
                                                        else
                                                            dt.Rows[rowNo][j] = DBNull.Value;
                                                    }
                                                }
                                                else { dt.Rows[rowNo][j] = sCells[i].Trim(); }
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

                        dt.Rows[rowNo]["colChk"] = "False";
                    }
                    cnt++;
                }

                #region EXPORT DATATABLE TO GRID

                // IF THERE ARE RECORDS IN DATA TABLE THEN ASSIGN IT TO GRID
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string ColName = dt.Columns[j].ColumnName;
                            if (ColName == "colInvoiceValue")
                                dt.Rows[i][j] = Utility.DisplayIndianCurrency(Convert.ToString(dt.Rows[i][j]));
                        }
                    }
                    dgvmain.DataSource = dt;
                }

                BindGridDataToMainGrid(dt);

                pbGSTR1.Visible = false;
                EnableControls(dgvmain);

                #endregion
            }
            catch (Exception ex)
            {
                EnableControls(dgvmain);
                pbGSTR1.Visible = false;
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }

        private void dgvmain_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string cNo = dgvmain.Columns[e.ColumnIndex].Name;

                if (e.RowIndex >= 0)
                {
                    if (cNo == "colType" || cNo == "colGSTIN" || cNo == "colInvoiceDate" || cNo == "colInvoiceValue" || cNo == "colPlaceSupply")
                    {
                        if (chkCellValue(Convert.ToString(dgvmain.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                        {
                            if (cNo == "colInvoiceValue")
                            {
                                if (Convert.ToString(dgvmain.Rows[e.RowIndex].Cells[cNo].Value).Trim() != "")
                                {
                                    dgvmain.CellValueChanged -= dgvmain_CellValueChanged;
                                    dgvmain.Rows[e.RowIndex].Cells[cNo].Value = Utility.DisplayIndianCurrency(Convert.ToString(Math.Round(Convert.ToDecimal(dgvmain.Rows[e.RowIndex].Cells[cNo].Value), 2, MidpointRounding.AwayFromZero)));
                                    dgvmain.CellValueChanged += dgvmain_CellValueChanged;

                                }
                            }

                            #region Bind Data
                            DataTable dt = new DataTable();

                            foreach (DataGridViewColumn col in dgvmain.Columns)
                            {
                                dt.Columns.Add(col.Name.ToString());
                                col.DataPropertyName = col.Name;
                            }

                            object[] rowValue = new object[dt.Columns.Count];

                            foreach (DataGridViewRow dr in dgvmain.Rows)
                            {
                                if (dr.Index != dgvmain.Rows.Count - 1) // DON'T ADD LAST ROW
                                {
                                    // SET CHECK BOX VALUE
                                    rowValue[0] = "False";
                                    for (int i = 1; i < dr.Cells.Count; i++)
                                    {
                                        rowValue[i] = Convert.ToString(dr.Cells[i].Value);
                                    }
                                    dt.Rows.Add(rowValue);
                                }
                                
                            }
                            dt.AcceptChanges();
                            #endregion
                            //dgvmain.Refresh();
                            //DataTable dt = new DataTable();
                            //foreach (DataGridViewColumn col in dgvmain.Columns)
                            //{
                            //    dt.Columns.Add(col.Name.ToString());
                            //}
                            //for (int i = 0; i < dgvmain.Rows.Count; i++)
                            //{
                            //    dt.Rows.Add("False", Convert.ToString(dgvmain.Rows[i].Cells["colType"].Value), Convert.ToString(dgvmain.Rows[i].Cells["colGSTIN"].Value), Convert.ToString(dgvmain.Rows[i].Cells["colParty"].Value), Convert.ToString(dgvmain.Rows[i].Cells["colInvoice"].Value), Convert.ToString(dgvmain.Rows[i].Cells["colInvoiceDate"].Value), Convert.ToString(dgvmain.Rows[i].Cells["colInvoiceValue"].Value), Convert.ToString(dgvmain.Rows[i].Cells["colPlaceSupply"].Value));
                            //}

                            //dt = (DataTable)dgvmain.DataSource;
                            //if (dt != null && dt.Rows.Count > 0)
                            BindGridDataToMainGrid(dt);
                        }
                        else
                        {
                            dgvmain.Rows[e.RowIndex].Cells[cNo].Value = "";
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

        public void BindGridDataToMainGrid(DataTable dt)
        {
            try
            {
                DataTable dtfilter = new DataTable();
                dtfilter = dt.Copy();

                // CHECK IMPORTED TEMPLATE
                if (dtfilter.Columns.Count != 1)
                {
                    if (dtfilter != null && dtfilter.Rows.Count > 0)
                    {
                        dtfilter.AcceptChanges();
                        dgvmain.AutoGenerateColumns = false;
                        dgvmain.DataSource = dtfilter;
                    }
                    else
                    {
                        MessageBox.Show("There are no records found ...!!!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Please fill data...!!!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                string gstin = CommonHelper.StateName;

                DataTable dtfn = new DataTable();

                //var dtfn = new System.Data.DataTable("tableName");

                // create fields
                dtfn.Columns.Add("colSrNo", typeof(string));
                dtfn.Columns.Add("colNatureOfDocument", typeof(string));
                dtfn.Columns.Add("colNilRatedSupply", typeof(string));
                dtfn.Columns.Add("colExempted", typeof(string));
                dtfn.Columns.Add("colNonGSTSupplies", typeof(string));

                #region Regsame
                DataTable dt_RegSame = Utility.Filter(dt, "colGSTIN <> '' and colPlaceSupply <> '" + gstin + "'");
                var result1 = (from r in dt_RegSame.AsEnumerable()
                               group r by new
                               {
                                   colType = r["colType"],
                               } into g

                               select new
                               {
                                   colType = g.Key.colType,
                                   colAdvadj = g.Sum(x => Convert.ToString(x["colInvoiceValue"]).Trim() == "" ? 0 : Convert.ToDecimal(x["colInvoiceValue"])),
                               });

                string ni = "";
                string ex = "";
                string non = "";
                foreach (var item in result1)
                {
                    if (item.colType.ToString().Trim() == "Nil rated")
                        ni = Convert.ToString(item.colAdvadj);
                    else if (item.colType.ToString().Trim() == "Exempted")
                        ex = Convert.ToString(item.colAdvadj);
                    else if (item.colType.ToString().Trim() == "Non-GST Supply")
                        non = Convert.ToString(item.colAdvadj);
                }
                dtfn.Rows.Add(new object[] { 1, "Inter-State supplies to registered persons", ni, ex, non });
                #endregion

                #region regDiff

                ni = ""; ex = ""; non = "";

                DataTable dt_RegDiff = Utility.Filter(dt, "colGSTIN <> '' and colPlaceSupply = '" + gstin + "'");

                var result11 = (from r in dt_RegDiff.AsEnumerable()
                                group r by new
                                {
                                    colType = r["colType"],
                                } into g

                                select new
                                {
                                    colType = g.Key.colType,
                                    colAdvadj = g.Sum(x => Convert.ToString(x["colInvoiceValue"]).Trim() == "" ? 0 : Convert.ToDecimal(x["colInvoiceValue"])),
                                });


                foreach (var item in result11)
                {
                    if (item.colType.ToString().Trim() == "Nil rated")
                        ni = Convert.ToString(item.colAdvadj);
                    else if (item.colType.ToString().Trim() == "Exempted")
                        ex = Convert.ToString(item.colAdvadj);
                    else if (item.colType.ToString().Trim() == "Non-GST Supply")
                        non = Convert.ToString(item.colAdvadj);
                }

                dtfn.Rows.Add(new object[] { 2, "Intra-State supplies to registered persons", ni, ex, non });

                #endregion

                #region unregsame
                ni = ""; ex = ""; non = "";
                DataTable dt_unRegSame = Utility.Filter(dt, "(colGSTIN is null or colGSTIN = '') and colPlaceSupply <> '" + gstin + "'");

                var result111 = (from r in dt_unRegSame.AsEnumerable()
                                 group r by new
                                 {
                                     colType = r["colType"],
                                 } into g

                                 select new
                                 {
                                     colType = g.Key.colType,
                                     colAdvadj = g.Sum(x => Convert.ToString(x["colInvoiceValue"]).Trim() == "" ? 0 : Convert.ToDecimal(x["colInvoiceValue"])),
                                 });

                foreach (var item in result111)
                {
                    if (item.colType.ToString().Trim() == "Nil rated")
                        ni = Convert.ToString(item.colAdvadj);
                    else if (item.colType.ToString().Trim() == "Exempted")
                        ex = Convert.ToString(item.colAdvadj);
                    else if (item.colType.ToString().Trim() == "Non-GST Supply")
                        non = Convert.ToString(item.colAdvadj);
                }

                dtfn.Rows.Add(new object[] { 3, "Inter-State supplies to unregistered persons", ni, ex, non });

                #endregion

                #region unregDiff
                ni = ""; ex = ""; non = "";
                DataTable dt_unRegDiff = Utility.Filter(dt, "(colGSTIN is null or colGSTIN = '') and colPlaceSupply = '" + gstin + "'");

                var result1111 = (from r in dt_unRegDiff.AsEnumerable()
                                  group r by new
                                  {
                                      colType = r["colType"],
                                  } into g

                                  select new
                                  {
                                      colType = g.Key.colType,
                                      colAdvadj = g.Sum(x => Convert.ToString(x["colInvoiceValue"]).Trim() == "" ? 0 : Convert.ToDecimal(x["colInvoiceValue"])),
                                  });

                foreach (var item in result1111)
                {
                    if (item.colType.ToString().Trim() == "Nil rated")
                        ni = Convert.ToString(item.colAdvadj);
                    else if (item.colType.ToString().Trim() == "Exempted")
                        ex = Convert.ToString(item.colAdvadj);
                    else if (item.colType.ToString().Trim() == "Non-GST Supply")
                        non = Convert.ToString(item.colAdvadj);
                }

                dtfn.Rows.Add(new object[] { 4, "Intra-State supplies to unregistered persons", ni, ex, non });

                #endregion

                dtfn.AcceptChanges();

                dgvGSTR1_NilRated.AutoGenerateColumns = false;
                dgvGSTR1_NilRated.DataSource = dtfn;
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

        private void dgvGSTR1_NilRated_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete)
                {
                    #region DELETE SELECTED CELLS
                    try
                    {
                        if (dgvGSTR1_NilRated.Rows.Count > 0)
                        {
                            foreach (DataGridViewCell oneCell in dgvGSTR1_NilRated.SelectedCells)
                            {
                                if (oneCell.Selected && oneCell.ColumnIndex != 0 && oneCell.Selected && oneCell.ColumnIndex != 1)
                                {
                                    oneCell.ValueType.Name.ToString();
                                    oneCell.ValueType.FullName.ToString();
                                    oneCell.Value = "";
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                        StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                        errorWriter.Write(errorMessage);
                        errorWriter.Close();
                        return;
                    }
                    #endregion

                    // TOTAL CALCULATION

                    string[] colNo = { "colTotal", "colCancelled", "colIssued" };
                    //GetTotal(colNo);
                }
                if (e.KeyCode == Keys.V)
                {
                    #region PAST FROM EXCELL SHEET

                    string s = Clipboard.GetText();
                    string[] lines = s.Split('\n');
                    int iRow = 0, iCol = 0;

                    #region PAST ON SELECTED CELLS ONLY
                    if (dgvGSTR1_NilRated.RowCount > 0) // IF GRID IS NOT NULL AND PAST ONLY ON SELECTED CELLS
                    {
                        foreach (DataGridViewCell oneCell in dgvGSTR1_NilRated.SelectedCells)
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
                    foreach (string line in lines)
                    {
                        if (line != "")
                        {
                            if (iRow < dgvGSTR1_NilRated.RowCount && line.Length > 0 && iRow < 4)
                            {
                                string[] sCells = line.Split('\t');

                                for (int i = 0; i < sCells.GetLength(0); ++i)
                                {
                                    if (iCol + i < this.dgvGSTR1_NilRated.ColumnCount && i < 3)
                                    {
                                        if (iCol == 0)
                                            oCell = dgvGSTR1_NilRated[iCol + i + 2, iRow];
                                        else if (iCol == 1)
                                            oCell = dgvGSTR1_NilRated[iCol + i + 1, iRow];
                                        else
                                            oCell = dgvGSTR1_NilRated[iCol + i, iRow];

                                        sCells[i] = sCells[i].Trim().Replace(",", "");
                                        if (oCell.ColumnIndex != 0)
                                        {
                                            if (dgvGSTR1_NilRated.Columns[oCell.ColumnIndex].Name != "colChk" && dgvGSTR1_NilRated.Columns[oCell.ColumnIndex].Name != "colSequence")
                                            {
                                                #region VALIDATION
                                                if (sCells[i].ToString().Trim() == "") { dgvGSTR1_NilRated.Rows[iRow].Cells[oCell.ColumnIndex].Value = DBNull.Value; }
                                                else
                                                {
                                                    if (oCell.ColumnIndex >= 1 && oCell.ColumnIndex <= 8)
                                                    {
                                                        //if (chkCellValue(sCells[i].Trim(), oCell.ColumnIndex))
                                                        dgvGSTR1_NilRated.Rows[iRow].Cells[oCell.ColumnIndex].Value = sCells[i].Trim();
                                                        //else
                                                        //    dgvGSTR13.Rows[iRow].Cells[oCell.ColumnIndex].Value = DBNull.Value;
                                                    }
                                                    else { dgvGSTR1_NilRated.Rows[iRow].Cells[oCell.ColumnIndex].Value = sCells[i].Trim(); }
                                                }
                                                #endregion
                                            }
                                        }
                                        else
                                        {
                                            if (iCol > i)
                                            {
                                                for (int j = oCell.ColumnIndex; j < dgvGSTR1_NilRated.Columns.Count; j++)
                                                {
                                                    #region VALIDATION
                                                    if (sCells[i].ToString().Trim() == "") { dgvGSTR1_NilRated.Rows[iRow].Cells[j].Value = DBNull.Value; }
                                                    else
                                                    {
                                                        if (j >= 1 && j <= 8)
                                                        {
                                                            //if (chkCellValue(sCells[i].Trim(), j))
                                                            dgvGSTR1_NilRated.Rows[iRow].Cells[j].Value = sCells[i].Trim();
                                                            // else
                                                            //   dgvGSTR13.Rows[iRow].Cells[j].Value = DBNull.Value;
                                                        }
                                                        else { dgvGSTR1_NilRated.Rows[iRow].Cells[j].Value = sCells[i].Trim(); }
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
                                                for (int j = oCell.ColumnIndex; j < dgvGSTR1_NilRated.Columns.Count; j++)
                                                {
                                                    #region VALIDATION
                                                    if (sCells[i].ToString().Trim() == "") { dgvGSTR1_NilRated.Rows[iRow].Cells[j].Value = DBNull.Value; }
                                                    else
                                                    {
                                                        if (j >= 1 && j <= 8)
                                                        {
                                                            //if (chkCellValue(sCells[i].Trim(), j))
                                                            dgvGSTR1_NilRated.Rows[iRow].Cells[j].Value = sCells[i].Trim();
                                                            //    else
                                                            //       dgvGSTR13.Rows[iRow].Cells[j].Value = DBNull.Value;
                                                        }
                                                        else { dgvGSTR1_NilRated.Rows[iRow].Cells[j].Value = sCells[i].Trim(); }
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
                            }
                        }
                    }

                    #endregion
                }
                if (e.KeyCode == Keys.A)
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }

        private void dgvGSTR1_NilRated_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string cNo = dgvGSTR1_NilRated.Columns[e.ColumnIndex].Name;

                if (e.RowIndex >= 0)
                {
                    if (cNo == "colNilRatedSupply" || cNo == "colExempted" || cNo == "colNonGSTSupplies")
                    {
                        if (chkCellValue(Convert.ToString(dgvGSTR1_NilRated.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                        {
                            if (Convert.ToString(dgvGSTR1_NilRated.Rows[e.RowIndex].Cells[cNo].Value).Trim() != "")
                            {
                                dgvGSTR1_NilRated.CellValueChanged -= dgvGSTR1_NilRated_CellValueChanged;
                                dgvGSTR1_NilRated.Rows[e.RowIndex].Cells[cNo].Value = Utility.DisplayIndianCurrency(Convert.ToString(Math.Round(Convert.ToDecimal(dgvGSTR1_NilRated.Rows[e.RowIndex].Cells[cNo].Value), 2, MidpointRounding.AwayFromZero)));
                                dgvGSTR1_NilRated.CellValueChanged += dgvGSTR1_NilRated_CellValueChanged;
                            }
                            string[] colNo = { dgvGSTR1_NilRated.Columns[e.ColumnIndex].Name };

                            //GetTotal(colNo);
                        }
                        else
                        {
                            dgvGSTR1_NilRated.Rows[e.RowIndex].Cells[cNo].Value = "";
                            string[] colNo = { dgvGSTR1_NilRated.Columns[e.ColumnIndex].Name };
                            //GetTotal(colNo);
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
                    if (cNo == "colNilRatedSupply" || cNo == "colExempted" || cNo == "colNonGSTSupplies")
                    {
                        if (Utility.IsDecimalOrNumber(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colType")
                    {
                        if (Utility.NilRatedType(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colGSTIN")
                    {
                        if (Utility.IsValidGSTN(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colInvoiceDate")
                    {
                        if (Utility.IsInvoiceDate(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colInvoice")
                    {
                        if (Utility.IsBlankInvoiceNumber(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colInvoiceValue")
                    {
                        if (Utility.IsInvoiceValue(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colPlaceSupply")
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

        public bool chkNilType(string val)
        {
            bool flg = false;
            val = val.Trim();
            try
            {
                if (val == "Nil rated" || val == "Exempted" || val == "Non-GST Supply")
                    flg = true;
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

            return flg;
        }

        public void Save()
        {
            try
            {
                #region ADD DATATABLE COLUMN Multi

                // create datatable to store main grid data
                DataTable dtMain = new DataTable();

                // add datatble collumn as par main  grid column
                foreach (DataGridViewColumn col in dgvmain.Columns)
                {
                    dtMain.Columns.Add(col.Name.ToString());
                }

                // add datatable column to store file status
                dtMain.Columns.Add("colFileStatus");

                #endregion

                #region ASSIGN GRIDVIEW ROWS IN DATATABLE Multi

                bool flg = false;

                for (int i = 0; i < dgvmain.Rows.Count-1; i++)
                {
                    for (int j = 1; j < dgvmain.Columns.Count; j++)
                    {
                        if (Convert.ToString(dgvmain.Rows[i].Cells[j].Value).Trim() != "")
                            flg = true;
                    }
                }

                if (flg)
                {
                    //if (CommonHelper.StatusIndex == 0)
                    //{
                    //    MessageBox.Show("Please Select File Status!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    return;
                    //}

                    // create object array to store one row data of main grid
                    object[] rowValue = new object[dtMain.Columns.Count];

                    foreach (DataGridViewRow dr in dgvmain.Rows)
                    {
                        if (dr.Index != dgvmain.Rows.Count -1) // DON'T ADD LAST ROW
                        {
                            for (int i = 0; i < dr.Cells.Count; i++)
                            {
                                rowValue[i] = Convert.ToString(dr.Cells[i].Value);
                            }

                            // assign file status value with each grid row
                            rowValue[dr.Cells.Count] = Convert.ToString(CommonHelper.StatusText);

                            // add array of grid row value to datatable as row
                            dtMain.Rows.Add(rowValue);
                        }
                    }
                    dtMain.AcceptChanges();
                }
                #endregion

                #region RECORD SAVE Multi
                string Query = "";
                int _Result = 0;

                // check there are records in grid
                if (dtMain != null && dtMain.Rows.Count > 0)
                {
                    #region first delete old data from database
                    Query = "Delete from SPQR1NilRatedMulti where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                    _Result = objGSTR13.IUDData(Query);
                    if (_Result != 1)
                    {
                        // error occurs while deleting data
                        MessageBox.Show("System error.\nPlease try after sometime!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    #endregion

                    // query fire to save records to database
                    _Result = objGSTR13.SPQR1NilRatedMulti(dtMain, Convert.ToString(CommonHelper.StatusText));

                    if (_Result == 1)
                    {
                        #region For Total Save
                        // CREATE DATATABLE TO STORE MAIN GRID DATA
                        DataTable dtTotal = new DataTable();

                        // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                        foreach (DataGridViewColumn col in dgvmain.Columns)
                        {
                            dtTotal.Columns.Add(col.Name.ToString());
                        }

                        // ADD DATATABLE COLUMN TO STORE FILE STATUS
                        dtTotal.Columns.Remove("colChk");
                        dtTotal.Columns.Add("colFileStatus");

                        string InvoiceNo = "0";
                        string InvoiceValue = "0";
                        InvoiceNo = dgvmain.Rows.Cast<DataGridViewRow>().Where(x => Convert.ToString(x.Cells["colInvoice"].Value).Trim() != "").GroupBy(x => x.Cells["colInvoice"].Value).Select(x => x.First()).Distinct().Count().ToString();
                        InvoiceValue = dgvmain.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colInvoiceValue"].Value != null).Sum(x => x.Cells["colInvoiceValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colInvoiceValue"].Value)).ToString();
                        
                        dtTotal.Rows.Add("", "", "", InvoiceNo, "", InvoiceValue, "", "");

                        _Result = objGSTR13.SPQR1NilRatedMulti(dtTotal, "Total");
                        #endregion
                    }
                    else
                    {
                        // if errors ocurs while saving record from the database
                        MessageBox.Show("System error.\nPlease try after sometime!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    #region delete all old record if there are no records present in grid
                    Query = "Delete from SPQR1NilRatedMulti where Fld_Month='" + CommonHelper.SelectedMonth + "'  AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";

                    // fire queary to delete records
                    _Result = objGSTR13.IUDData(Query);

                    //if (_Result == 1)
                    //{
                    //    // if records deleted from database
                    //    MessageBox.Show("Record Successfully Deleted!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //    // make file status blank
                    //    ((MDI)Application.OpenForms["SPQMDI"]).SetFileStatus(0);
                    //}
                    //else
                    //{
                    //    // if errors ocurs while deleting record from the database
                    //    MessageBox.Show("System error.\nPlease try after sometime!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    return;
                    //}
                    #endregion
                }
                #endregion

                #region ADD DATATABLE COLUMN

                // create datatable to store main grid data
                DataTable dt = new DataTable();

                // add datatble collumn as par main  grid column
                foreach (DataGridViewColumn col in dgvGSTR1_NilRated.Columns)
                {
                    dt.Columns.Add(col.Name.ToString());
                }

                // add datatable column to store file status
                dt.Columns.Add("colFileStatus");

                #endregion

                #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                flg = false;

                for (int i = 0; i < dgvGSTR1_NilRated.Rows.Count; i++)
                {
                    for (int j = 2; j < dgvGSTR1_NilRated.Columns.Count; j++)
                    {
                        if (Convert.ToString(dgvGSTR1_NilRated.Rows[i].Cells[j].Value).Trim() != "")
                            flg = true;
                    }
                }

                if (flg)
                {

                    // create object array to store one row data of main grid
                    object[] rowValue = new object[dt.Columns.Count];

                    foreach (DataGridViewRow dr in dgvGSTR1_NilRated.Rows)
                    {
                        if (dr.Index != dgvGSTR1_NilRated.Rows.Count) // DON'T ADD LAST ROW
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
                }
                #endregion

                #region RECORD SAVE
                Query = "";
                _Result = 0;

                // check there are records in grid
                if (dt != null && dt.Rows.Count > 0)
                {
                    #region first delete old data from database
                    Query = "Delete from SPQR1NilRated where Fld_Month='" + CommonHelper.SelectedMonth + "'  AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                    _Result = objGSTR13.IUDData(Query);
                    if (_Result != 1)
                    {
                        // error occurs while deleting data
                        MessageBox.Show("System error.\nPlease try after sometime!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    #endregion

                    CommonHelper.StatusText = "Completed";
                    // query fire to save records to database
                    _Result = objGSTR13.SPQR1NilRated(dt, Convert.ToString(CommonHelper.StatusText));

                    if (_Result == 1)
                    {

                        string[] colNo = { "colTotal", "colCancelled", "colIssued" };
                        //GetTotal(colNo);

                        #region ADD DATATABLE COLUMN

                        // CREATE DATATABLE TO STORE MAIN GRID DATA
                        dt = new DataTable();

                        // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                        foreach (DataGridViewColumn col in dgvGSTR1_NilRated.Columns)
                        {
                            dt.Columns.Add(col.Name.ToString());
                        }

                        // ADD DATATABLE COLUMN TO STORE FILE STATUS
                        dt.Columns.Add("colFileStatus");

                        #endregion

                        #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                        // CREATE OBJECT ARRAY TO STORE ONE ROW DATA OF MAIN GRID
                        object[] rowVal = new object[dt.Columns.Count];

                        //aks commented
                        //if (TotaldgvAakash.Rows.Count == 1)
                        //{
                        //    foreach (DataGridViewRow dr in TotaldgvAakash.Rows)
                        //    {
                        //        for (int i = 0; i < dr.Cells.Count; i++)
                        //        {
                        //            rowVal[i] = Convert.ToString(dr.Cells[i].Value);
                        //        }

                        //        // ASSIGN FILE STATUS VALUE WITH EACH GRID ROW
                        //        rowVal[dr.Cells.Count] = "Total";

                        //        // ADD ARRAY OF GRID ROW VALUE TO DATATABLE AS ROW
                        //        dt.Rows.Add(rowVal);
                        //    }
                        //}
                        dt.AcceptChanges();
                        #endregion

                        //_Result = objGSTR13.GSTR13BulkEntry(dt, "Total");

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
                            MessageBox.Show("System error.\nPlease try after sometime!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                    }
                    else
                    {
                        // if errors ocurs while saving record from the database
                        MessageBox.Show("System error.\nPlease try after sometime!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    #region delete all old record if there are no records present in grid
                    Query = "Delete from SPQR1NilRated where Fld_Month='" + CommonHelper.SelectedMonth + "'  AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";

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
                        MessageBox.Show("System error.\nPlease try after sometime!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    #endregion
                }
                #endregion
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

        public void Delete()
        {
            try
            {
                if (dgvmain.Rows.Count == 1)
                {
                    ckboxHeader.Checked = false;
                    return;
                }
                if (dgvmain.CurrentCell.RowIndex == 0 && dgvmain.CurrentCell.ColumnIndex == 0)
                {
                    dgvmain.CurrentCell = dgvmain.Rows[0].Cells["colType"];
                }
                else { dgvmain.CurrentCell = dgvmain.Rows[0].Cells["colChk"]; }

                // CREATE FLAG FRO DELETE ROWS
                Boolean flgChk = false; Boolean flgSelect = false;

                // CREATE OBJECT OF SELECTED ROW TO DELETE
                List<DataGridViewRow> toDelete = new List<DataGridViewRow>();

                // CHECK THERE ARE RECORD PRESENT IN GRID
                if (dgvmain.Rows.Count > 1)
                {
                    // FLAG TRUE IF CHECK ALL SELECTED
                    if (ckboxHeader.Checked)
                        flgChk = true;

                    #region ADD SELECTED ROW TO OBJECT FOR DELETE
                    for (int i = 0; i < dgvmain.Rows.Count - 1; i++)
                    {
                        if (dgvmain[0, i].Value != null && dgvmain[0, i].Value.ToString() != "")
                        {
                            if (Convert.ToBoolean(dgvmain[0, i].Value) == true)
                            {
                                // ADD ROW TO OBJECT IF ROW IS SELECTED
                                flgSelect = true;
                                toDelete.Add(dgvmain.Rows[i]);
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
                            #region DELETE RECORDS

                            pbGSTR1.Visible = true;

                            if (flgChk)
                            {
                                // IF CHECK BOX OF CHECK ALL IS SELECTED
                                flgChk = false;

                                // CREATE DATATABLE AND ADD COLUMN AS PAR MAIN GRID
                                DataTable dt = new DataTable();
                                foreach (DataGridViewColumn col in dgvmain.Columns)
                                {
                                    dt.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }

                                // ASSIGN BLANK DATATABLE TO GRID
                                dgvmain.DataSource = dt;

                                dgvGSTR1_NilRated.DataSource = null;
                                BindData();
                            }
                            else if (flgSelect == true)
                            {
                                // DELETE SELECTED ROW
                                foreach (DataGridViewRow row in toDelete)
                                {
                                    dgvmain.Rows.RemoveAt(row.Index);
                                    Application.DoEvents();
                                }

                                DataTable dt = new DataTable();
                                dt = (DataTable)dgvmain.DataSource;
                                BindGridDataToMainGrid(dt);

                                //DataTable dt = new DataTable();

                                //#region ADD DATATABLE COLUMN

                                //foreach (DataGridViewColumn col in dgvmain.Columns)
                                //{
                                //    dt.Columns.Add(col.Name.ToString());
                                //    col.DataPropertyName = col.Name;
                                //}
                                //#endregion

                                //#region ASSIGN GRIDVIEW ROWS IN DATATABLE

                                //object[] rowValue = new object[dt.Columns.Count];

                                //foreach (DataGridViewRow dr in dgvmain.Rows)
                                //{
                                //    if (dr.Index != dgvmain.Rows.Count - 1) // DON'T ADD LAST ROW
                                //    {
                                //        // SET CHECK BOX VALUE
                                //        for (int i = 0; i < dr.Cells.Count; i++)
                                //        {
                                //            rowValue[i] = Convert.ToString(dr.Cells[i].Value);
                                //        }
                                //        rowValue[dr.Cells.Count - 1] = Convert.ToString(dr.Cells[dr.Cells.Count - 1].Value);

                                //        // ADD ARRAY OF GRID ROW VALUE TO DATATABLE AS ROW
                                //        dt.Rows.Add(rowValue);
                                //    }

                                //}
                                //dt.AcceptChanges();
                                //#endregion

                                //BindGridDataToMainGrid(dt);

                            }

                            // SET CONTROL PROPERTY AFTER ROW DELETION
                            ckboxHeader.Checked = false;
                            dgvmain.Columns[0].HeaderText = "Check All";
                            #endregion
                        }
                    }

                    pbGSTR1.Visible = false;
                }
                else
                {
                    // IF THERE ARE NO RECORD TO DELETE
                    ckboxHeader.Checked = false;
                    dgvmain.Columns[0].HeaderText = "Check All";
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
                        DataTable dtExcel = new DataTable();
                        dtExcel = ReadExcel(filePath, fileExt, dt);

                        #region RENAME DATATABLE COLUMN NAME AS PAR MAIN GRID
                        foreach (DataGridViewColumn col in dgvmain.Columns)
                        {
                            dtExcel.Columns[col.Index].ColumnName = col.Name.ToString();
                            col.DataPropertyName = col.Name;
                        }

                        DataTable dtfilter = new DataTable();
                        dtfilter = dtExcel.Copy();

                        // CHECK IMPORTED TEMPLATE
                        if (dtfilter.Columns.Count != 1)
                        {
                            if (dtfilter != null && dtfilter.Rows.Count > 0)
                            {
                                dtfilter.AcceptChanges();
                                dgvmain.AutoGenerateColumns = false;
                                dgvmain.DataSource = dtfilter;
                            }
                            else
                            {
                                MessageBox.Show("There are no records found in imported excel ...!!!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please import valid excel template...!!!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        #endregion

                        string gstin = CommonHelper.StateName;

                        DataTable dtfn = new DataTable();

                        //var dtfn = new System.Data.DataTable("tableName");

                        // create fields
                        dtfn.Columns.Add("colSrNo", typeof(string));
                        dtfn.Columns.Add("colNatureOfDocument", typeof(string));
                        dtfn.Columns.Add("colNilRatedSupply", typeof(string));
                        dtfn.Columns.Add("colExempted", typeof(string));
                        dtfn.Columns.Add("colNonGSTSupplies", typeof(string));

                        #region Regsame
                        DataTable dt_RegSame = Utility.Filter(dtExcel, "colGSTIN <> '' and colPlaceSupply <> '" + gstin + "'");
                        var result1 = (from r in dt_RegSame.AsEnumerable()
                                       group r by new
                                       {
                                           colType = r["colType"],
                                       } into g

                                       select new
                                       {
                                           colType = g.Key.colType,
                                           colAdvadj = g.Sum(x => Convert.ToString(x["colInvoiceValue"]).Trim() == "" ? 0 : Convert.ToDecimal(x["colInvoiceValue"])),
                                       });


                        string ni = "";
                        string ex = "";
                        string non = "";
                        foreach (var item in result1)
                        {
                            if (item.colType.ToString().Trim() == "Nil rated")
                                ni = Convert.ToString(item.colAdvadj);
                            else if (item.colType.ToString().Trim() == "Exempted")
                                ex = Convert.ToString(item.colAdvadj);
                            else if (item.colType.ToString().Trim() == "Non-GST Supply")
                                non = Convert.ToString(item.colAdvadj);
                        }
                        dtfn.Rows.Add(new object[] { 1, "Inter-State supplies to registered persons", ni, ex, non });
                        #endregion

                        #region regDiff

                        ni = ""; ex = ""; non = "";

                        DataTable dt_RegDiff = Utility.Filter(dtExcel, "colGSTIN <> '' and colPlaceSupply = '" + gstin + "'");

                        var result11 = (from r in dt_RegDiff.AsEnumerable()
                                        group r by new
                                        {
                                            colType = r["colType"],
                                        } into g

                                        select new
                                        {
                                            colType = g.Key.colType,
                                            colAdvadj = g.Sum(x => Convert.ToString(x["colInvoiceValue"]).Trim() == "" ? 0 : Convert.ToDecimal(x["colInvoiceValue"])),
                                        });


                        foreach (var item in result11)
                        {
                            if (item.colType.ToString().Trim() == "Nil rated")
                                ni = Convert.ToString(item.colAdvadj);
                            else if (item.colType.ToString().Trim() == "Exempted")
                                ex = Convert.ToString(item.colAdvadj);
                            else if (item.colType.ToString().Trim() == "Non-GST Supply")
                                non = Convert.ToString(item.colAdvadj);
                        }

                        dtfn.Rows.Add(new object[] { 2, "Intra-State supplies to registered persons", ni, ex, non });

                        #endregion

                        #region unregsame
                        ni = ""; ex = ""; non = "";
                        DataTable dt_unRegSame = Utility.Filter(dtExcel, "(colGSTIN is null or colGSTIN = '') and colPlaceSupply <> '" + gstin + "'");

                        var result111 = (from r in dt_unRegSame.AsEnumerable()
                                         group r by new
                                         {
                                             colType = r["colType"],
                                         } into g

                                         select new
                                         {
                                             colType = g.Key.colType,
                                             colAdvadj = g.Sum(x => Convert.ToString(x["colInvoiceValue"]).Trim() == "" ? 0 : Convert.ToDecimal(x["colInvoiceValue"])),
                                         });


                        foreach (var item in result111)
                        {
                            if (item.colType.ToString().Trim() == "Nil rated")
                                ni = Convert.ToString(item.colAdvadj);
                            else if (item.colType.ToString().Trim() == "Exempted")
                                ex = Convert.ToString(item.colAdvadj);
                            else if (item.colType.ToString().Trim() == "Non-GST Supply")
                                non = Convert.ToString(item.colAdvadj);
                        }

                        dtfn.Rows.Add(new object[] { 3, "Inter-State supplies to unregistered persons", ni, ex, non });

                        #endregion

                        #region unregDiff
                        ni = ""; ex = ""; non = "";
                        DataTable dt_unRegDiff = Utility.Filter(dtExcel, "(colGSTIN is null or colGSTIN = '') and colPlaceSupply = '" + gstin + "'");

                        var result1111 = (from r in dt_unRegDiff.AsEnumerable()
                                          group r by new
                                          {
                                              colType = r["colType"],
                                          } into g

                                          select new
                                          {
                                              colType = g.Key.colType,
                                              colAdvadj = g.Sum(x => Convert.ToString(x["colInvoiceValue"]).Trim() == "" ? 0 : Convert.ToDecimal(x["colInvoiceValue"])),
                                          });


                        foreach (var item in result1111)
                        {
                            if (item.colType.ToString().Trim() == "Nil rated")
                                ni = Convert.ToString(item.colAdvadj);
                            else if (item.colType.ToString().Trim() == "Exempted")
                                ex = Convert.ToString(item.colAdvadj);
                            else if (item.colType.ToString().Trim() == "Non-GST Supply")
                                non = Convert.ToString(item.colAdvadj);
                        }

                        dtfn.Rows.Add(new object[] { 4, "Intra-State supplies to unregistered persons", ni, ex, non });

                        #endregion

                        dtfn.AcceptChanges();
                        dgvGSTR1_NilRated.AutoGenerateColumns = false;
                        dgvGSTR1_NilRated.DataSource = dtfn;

                    }
                    else
                    {
                        MessageBox.Show("Please choose .xls or .xlsx file only.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //CUSTOM MESSAGEBOX TO SHOW ERROR 
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
                        OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [nilrated-exempted-nongst$]", con);
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
                        #region REMOVE UNUSED COLUMN FROM EXCEL
                        if (dtexcel.Columns.Count >= dgvmain.Columns.Count - 2)
                        {
                            for (int i = dtexcel.Columns.Count - 1; i > (8 - 2); i--)
                            {
                                dtexcel.Columns.Remove(dtexcel.Columns[i]);
                            }
                        }
                        dtexcel.AcceptChanges();
                        #endregion


                        #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME
                        foreach (DataGridViewColumn col in dgvmain.Columns)
                        {
                            if (col.Index != 0)
                                dtexcel.Columns[col.Index - 1].ColumnName = col.Name.ToString();
                        }
                        #endregion


                        dtexcel.Columns.Add(new DataColumn("colChk"));
                        dtexcel.Columns["colChk"].SetOrdinal(0);
                        dtexcel.AcceptChanges();

                        #region SET COLTAX VALUE AS TRUE/FALSE
                        for (int i = 0; i < dtexcel.Rows.Count; i++)
                        {
                            dtexcel.Rows[i]["colChk"] = "False";

                            //if (chkNilType(Convert.ToString(dtexcel.Rows[i]["colType"])))
                            //    dtexcel.Rows[i]["colType"] = Convert.ToString(dtexcel.Rows[i]["colType"]).Trim();
                            //else
                            //    dtexcel.Rows[i]["colType"] = "";

                            if (Utility.NilRatedType(Convert.ToString(dtexcel.Rows[i]["colType"]).Trim()))
                                dtexcel.Rows[i]["colType"] = Utility.StrNilRatedType(Convert.ToString(dtexcel.Rows[i]["colType"]).Trim());
                            else
                                dtexcel.Rows[i]["colType"] = "";

                            if (Utility.IsValidStateName(Convert.ToString(dtexcel.Rows[i]["colPlaceSupply"]).Trim()))
                                dtexcel.Rows[i]["colPlaceSupply"] = Convert.ToString(dtexcel.Rows[i]["colPlaceSupply"]).Trim();
                            else
                                dtexcel.Rows[i]["colPlaceSupply"] = "";

                            int sj = dtexcel.Columns["colInvoiceDate"].Ordinal;
                            dtexcel = Utility.ChangeColumnDataType(dtexcel, dtexcel.Columns["colInvoiceDate"].ColumnName, typeof(string));
                            dtexcel.Columns["colInvoiceDate"].SetOrdinal(sj);

                            try
                            {
                                DateTime ss = Convert.ToDateTime(dtexcel.Rows[i]["colInvoiceDate"]);
                                dtexcel.Rows[i]["colInvoiceDate"] = Convert.ToString(ss.ToString("dd-MM-yyyy").Replace('/', '-'));
                            }
                            catch (Exception)
                            {
                                dtexcel.Rows[i]["colInvoiceDate"] = "";
                            }
                        }
                        #endregion
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

                // return datatable
                return dtexcel;
            }
        }

        #endregion

        #region JSON TRANSACTION

        #region JSON CLASS
        public class InvNil
        {
            public string sply_ty { get; set; }
            public double expt_amt { get; set; }
            public double nil_amt { get; set; }
            public double ngsup_amt { get; set; }
        }
        public class Nil
        {
            public List<InvNil> inv { get; set; }
        }
        public class RootObject
        {
            public string gstin { get; set; }
            public string fp { get; set; }
            public double gt { get; set; }
            public double cur_gt { get; set; }
            public Nil nil { get; set; }
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
                    DataTable dt = new DataTable();

                    #region Bind Grid Data

                    #region ADD DATATABLE COLUMN

                    foreach (DataGridViewColumn col in dgvGSTR1_NilRated.Columns)
                    {
                        dt.Columns.Add(col.Name.ToString());
                        col.DataPropertyName = col.Name;
                    }
                    #endregion

                    #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                    object[] rowValue = new object[dt.Columns.Count];

                    foreach (DataGridViewRow dr in dgvGSTR1_NilRated.Rows)
                    {
                        if (dr.Index != dgvGSTR1_NilRated.Rows.Count)
                        {
                            for (int i = 0; i < dr.Cells.Count; i++)
                            {
                                rowValue[i] = Convert.ToString(dr.Cells[i].Value);
                            }

                            dt.Rows.Add(rowValue);
                        }
                    }
                    dt.Columns.Remove("colSrNo");
                    dt.AcceptChanges();
                    #endregion

                    #endregion

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ObjJson.gstin = CommonHelper.CompanyGSTN; // tax person GSTIN
                        ObjJson.fp = CommonHelper.GetReturnPeriod(); // current return period
                        ObjJson.gt = CommonHelper.TurnOver; // previous financial year turnover
                        ObjJson.cur_gt = Convert.ToDouble(CommonHelper.CurrentTurnOver); // current Finacial year turnover

                        Nil objNil = new Nil();

                        List<InvNil> invNil = new List<InvNil>();

                        foreach (DataRow dr in dt.Rows)
                        {
                            InvNil InvDet = new InvNil();
                            InvDet.sply_ty = GetNilType(Convert.ToString(dr["colNatureOfDocument"]).Trim());

                            if (!string.IsNullOrEmpty(Convert.ToString(dr["colNilRatedSupply"]).Trim()))
                                InvDet.expt_amt = Convert.ToDouble(dr["colNilRatedSupply"]); // nil rated supply 

                            if (!string.IsNullOrEmpty(Convert.ToString(dr["colExempted"]).Trim()))
                                InvDet.nil_amt = Convert.ToDouble(dr["colExempted"]); // axempted

                            if (!string.IsNullOrEmpty(Convert.ToString(dr["colNonGSTSupplies"]).Trim()))
                                InvDet.ngsup_amt = Convert.ToDouble(dr["colNonGSTSupplies"]); // from

                            invNil.Add(InvDet);
                            objNil.inv = invNil;
                            ObjJson.nil = objNil;
                        }

                        #region File Save
                        JavaScriptSerializer objScript = new JavaScriptSerializer();

                        var settings = new JsonSerializerSettings();
                        settings.NullValueHandling = NullValueHandling.Ignore;
                        settings.DefaultValueHandling = DefaultValueHandling.Ignore;

                        objScript.MaxJsonLength = 2147483647;

                        string FinalJson = JsonConvert.SerializeObject(ObjJson, settings);

                        SaveFileDialog save = new SaveFileDialog();
                        save.FileName = "NIL.json";
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
                    //}
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

        #endregion

        public void SetGridViewColor()
        {
            try
            {
                // set main grid property
                this.dgvGSTR1_NilRated.AllowUserToAddRows = false;
                this.dgvGSTR1_NilRated.AutoGenerateColumns = false;

                dgvGSTR1_NilRated.EnableHeadersVisualStyles = false;
                dgvGSTR1_NilRated.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(23, 196, 187);
                dgvGSTR1_NilRated.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

                this.dgvGSTR1_NilRated.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                this.dgvGSTR1_NilRated.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                this.dgvGSTR1_NilRated.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

                this.dgvmain.AutoGenerateColumns = false;

                dgvmain.EnableHeadersVisualStyles = false;
                dgvmain.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(23, 196, 187);
                dgvmain.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

                this.dgvmain.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                this.dgvmain.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                this.dgvmain.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

                dgvGSTR1_NilRated.Columns[0].ReadOnly = true;
                dgvGSTR1_NilRated.Columns[0].DefaultCellStyle.BackColor = Color.FromArgb(23, 196, 187);

                foreach (DataGridViewColumn column in dgvGSTR1_NilRated.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
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

        public string GetNilType(string val)
        {
            string typeNil = "";
            val = val.Trim();
            try
            {
                if (val == "Inter-State supplies to registered persons")
                    typeNil = "INTRB2B";
                else if (val == "Intra-State supplies to registered persons")
                    typeNil = "INTRAB2B";
                else if (val == "Inter-State supplies to unregistered persons")
                    typeNil = "INTRB2C";
                else if (val == "Intra-State supplies to unregistered persons")
                    typeNil = "INTRAB2C";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
            return typeNil;
        }

        #region DISABLE/ENABLE CONTROLS

        private void DisableControls(Control con)
        {
            foreach (Control c in con.Controls)
            {
                if (c.Name != "SPQGSTR1B2B" && c.Name != "dgvmainTotal")
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

        #region Check/Uncheck All

        private void dgvmain_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                // CHECK FIRST COLUMN HEADER PRESSED AND MAIN GRID HAVING RECORDS
                if (e.ColumnIndex == 0 && dgvmain.Rows.Count > 1)
                {
                    // CHECK AND UNCHECK CHECK BOX OF HEADER FOR SELECTING AND UNSELECTING ALL RECORDS
                    if (dgvmain.Columns[e.ColumnIndex].HeaderText == "Check All")
                        ckboxHeader.Checked = true;
                    else if (dgvmain.Columns[e.ColumnIndex].HeaderText == "Uncheck All")
                        ckboxHeader.Checked = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ckboxHeader_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvmain.Rows.Count > 1)
                {
                    pbGSTR1.Visible = true;
                    if (ckboxHeader.Checked)
                    {
                        for (int i = 0; i < dgvmain.Rows.Count - 1; i++)
                        {
                            dgvmain.Rows[i].Cells[0].Value = "True";
                            Application.DoEvents();
                        }

                        dgvmain.Columns[0].HeaderText = "Uncheck All";
                    }
                    else if (ckboxHeader.Checked == false)
                    {
                        for (int i = 0; i < dgvmain.Rows.Count - 1; i++)
                        {
                            dgvmain.Rows[i].Cells[0].Value = "False";
                            Application.DoEvents();
                        }

                        dgvmain.Columns[0].HeaderText = "Check All";
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

        #endregion

        private void frmGSTR1_NilRated_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName("");
        }

        private void dgvmain_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                // SET INDEX OF USER ADDED ROW IN MAIN GRID
                dgvmain.Rows[e.Row.Index - 1].Cells["colChk"].Value = "False";
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

        public void ValidataAndGetGSTIN()
        {
            try
            {
                if (dgvmain.Rows.Count > 1)
                {
                    pbGSTR1.Visible = true;
                    new PrefillHelper().GetNameByGSTIN(dgvmain, "colGSTIN", "colParty");

                    if (CommonHelper.IsGetGSINError != null)
                    {
                        if ((bool)CommonHelper.IsGetGSINError)
                            MessageBox.Show("There may be some wrong GSTIN number or Something went wrong...!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            Save();
                    }

                    pbGSTR1.Visible = false;
                }
                else
                    MessageBox.Show("There is No record", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnClose_Click(object sender, EventArgs e)
        {

            //(new SPQMDI()).Save_Close();
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
            //ExportExcel();
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            //Validate();
           // IsValidateData();
        }

        private void btnVerifyGSTIN_Click(object sender, EventArgs e)
        {
            ValidataAndGetGSTIN();
        }

    }
}
