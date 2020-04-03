using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SPEQTAGST.BAL;
using SPEQTAGST.BAL.M796r3b;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Data.OleDb;
using System.Reflection;
using Newtonsoft.Json;

namespace SPEQTAGST.rintlcs3b
{
    public partial class SPQGSTR3B5 : Form
    {
        r3bPublicclass objGSTR3B = new r3bPublicclass();

        public SPQGSTR3B5()
        {
            InitializeComponent();
            GetData();
            BindData();
            SetGridViewColor();
            ((SPQMDI)Application.OpenForms["SPQMDI"]).ShowExtraToolsMenu();
            ((SPQMDI)Application.OpenForms["SPQMDI"]).ShowCompanyDetailMenu();
            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName(CommonHelper.ReturnName);

            dgvGSTR3B5.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dgvGSTR3B5.EnableHeadersVisualStyles = false;
            dgvGSTR3B5.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);
            dgvGSTR3B5.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvGSTR3B5.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            
        }

        private void GetData()
        {
            try
            {
                // create datatable to store database data
                DataTable dt = new DataTable();
                string Query = "Select * from SPQR3BExemptSupply where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
                Application.DoEvents();

                // get data from database
                dt = objGSTR3B.GetDataGSTR3B(Query);

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
                    foreach (DataGridViewColumn col in dgvGSTR3B5.Columns)
                    {
                        dt.Columns[col.Index].ColumnName = col.Name.ToString();
                        col.DataPropertyName = col.Name;
                    }
                    dt.AcceptChanges();
                    dgvGSTR3B5.DataSource = dt;

                    #endregion
                }
                else
                {
                    dgvGSTR3B5.DataSource = null;
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
                if (dgvGSTR3B5.Rows.Count <= 1)
                {
                    DataTable dt = new DataTable();

                    // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                    foreach (DataGridViewColumn col in dgvGSTR3B5.Columns)
                    {
                        dt.Columns.Add(col.Name.ToString());
                        col.DataPropertyName = col.Name;
                    }
                    dt.AcceptChanges();

                    DataRow dr = dt.NewRow();
                    dr["colSrNo"] = "1";
                    dr["colNatureofSupply"] = "From a supplier under composition scheme, Exempt and Nil rated Supply";
                    dr["colInterStateSupplies"] = "";
                    dr["colIntraStateSupplies"] = "";
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr["colSrNo"] = "2";
                    dr["colNatureofSupply"] = "Non GST Supply";
                    dr["colInterStateSupplies"] = "";
                    dr["colIntraStateSupplies"] = "";
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dgvGSTR3B5.DataSource = dt;

                    dgvGSTR3B5.Columns["colNatureofSupply"].ReadOnly = true;
                    DataGridViewRow row = this.dgvGSTR3B5.RowTemplate;
                    row.MinimumHeight = 30;
                    for (int i = 0; i < dgvGSTR3B5.Rows.Count; i++)
                    {
                        for (int j = 3; j < dgvGSTR3B5.ColumnCount; j++)
                        {
                            if (dgvGSTR3B5.Rows[i].Cells[j].Value.ToString() == "-" || dgvGSTR3B5.Rows[i].Cells[j].Value.ToString() == "" || dgvGSTR3B5.Rows[i].Cells[j].Value == null)
                            {
                                //dgvGSTR3B5.Rows[i].Cells[j].Value = "0";
                            }
                        }
                    }
                }
                else
                {
                    DataGridViewRow row = this.dgvGSTR3B5.RowTemplate;
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

        public void SetGridViewColor()
        {
            try
            {
                // set main grid property
                this.dgvGSTR3B5.AllowUserToAddRows = false;
                this.dgvGSTR3B5.AutoGenerateColumns = false;

                dgvGSTR3B5.EnableHeadersVisualStyles = false;
                dgvGSTR3B5.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(23, 196, 187);
                dgvGSTR3B5.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

                this.dgvGSTR3B5.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                this.dgvGSTR3B5.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                this.dgvGSTR3B5.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

                dgvGSTR3B5.Columns[0].ReadOnly = true;
                dgvGSTR3B5.Columns[0].DefaultCellStyle.BackColor = Color.FromArgb(23, 196, 187);

                foreach (DataGridViewColumn column in dgvGSTR3B5.Columns)
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

        public void Save()
        {
            try
            {
                //if (CommonHelper.StatusIndex == 0)
                //{
                //    MessageBox.Show("Please Select File Status!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}

                #region ADD DATATABLE COLUMN

                // create datatable to store main grid data
                DataTable dt = new DataTable();

                // add datatble collumn as par main  grid column
                foreach (DataGridViewColumn col in dgvGSTR3B5.Columns)
                {
                    dt.Columns.Add(col.Name.ToString());
                }

                // add datatable column to store file status
                dt.Columns.Add("colFileStatus");

                #endregion

                #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                // create object array to store one row data of main grid
                object[] rowValue = new object[dt.Columns.Count];

                foreach (DataGridViewRow dr in dgvGSTR3B5.Rows)
                {
                    if (dr.Index != dgvGSTR3B5.Rows.Count) // DON'T ADD LAST ROW
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
                    Query = "Delete from SPQR3BExemptSupply where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                    _Result = objGSTR3B.IUDData(Query);
                    if (_Result != 1)
                    {
                        // error occurs while deleting data
                        MessageBox.Show("System error.\nPlease try after sometime!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    #endregion

                    // query fire to save records to database
                    _Result = objGSTR3B.GSTR3B5BulkEntry(dt, Convert.ToString(CommonHelper.StatusText));

                    if (_Result == 1)
                    {
                        #region ADD DATATABLE COLUMN

                        // CREATE DATATABLE TO STORE MAIN GRID DATA
                        dt = new DataTable();

                        // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                        foreach (DataGridViewColumn col in dgvGSTR3B5.Columns)
                        {
                            dt.Columns.Add(col.Name.ToString());
                        }

                        // ADD DATATABLE COLUMN TO STORE FILE STATUS
                        dt.Columns.Add("colFileStatus");

                        #endregion

                        MessageBox.Show("Data Saved Successfully …!!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // BIND DATA
                        GetData();
                        BindData();

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
                    Query = "Delete from SPQR3BExemptSupply where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";

                    // fire queary to delete records
                    _Result = objGSTR3B.IUDData(Query);

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
                DialogResult result = MessageBox.Show("Do you want to delete selected data?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // IF USER CONFIRM FOR DELETING RECORDS
                if (result == DialogResult.Yes)
                {
                    #region first delete old data from database
                    string Query = "Delete from SPQR3BExemptSupply where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                    int _Result = objGSTR3B.IUDData(Query);
                    if (_Result != 1)
                    {
                        // error occurs while deleting data
                        MessageBox.Show("System error.\nPlease try after sometime!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        GetData();
                        BindData();
                    }
                    #endregion
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
                if (e.KeyCode == Keys.Delete)
                {
                    #region DELETE SELECTED CELLS
                    try
                    {
                        if (dgvGSTR3B5.Rows.Count > 0)
                        {
                            foreach (DataGridViewCell oneCell in dgvGSTR3B5.SelectedCells)
                            {
                                if (oneCell.ColumnIndex != 1)
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

                }
                if (e.KeyCode == Keys.V)
                {
                    #region PAST FROM EXCELL SHEET

                    string s = Clipboard.GetText();
                    string[] lines = s.Split('\n');
                    int iRow = 0, iCol = 0;

                    #region PAST ON SELECTED CELLS ONLY
                    if (dgvGSTR3B5.RowCount > 0) // IF GRID IS NOT NULL AND PAST ONLY ON SELECTED CELLS
                    {
                        foreach (DataGridViewCell oneCell in dgvGSTR3B5.SelectedCells)
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
                            if (iRow < dgvGSTR3B5.RowCount && line.Length > 0 && iRow < 12)
                            {
                                string[] sCells = line.Split('\t');

                                for (int i = 0; i < sCells.GetLength(0); ++i)
                                {
                                    if (iCol + i < this.dgvGSTR3B5.ColumnCount && i < 7)
                                    {
                                        if (iCol == 0)
                                            oCell = dgvGSTR3B5[iCol + i + 2, iRow];
                                        else if (iCol == 1)
                                            oCell = dgvGSTR3B5[iCol + i + 1, iRow];
                                        else
                                            oCell = dgvGSTR3B5[iCol + i, iRow];

                                        sCells[i] = sCells[i].Trim().Replace(",", "");
                                        if (oCell.ColumnIndex != 0)
                                        {
                                            if (dgvGSTR3B5.Columns[oCell.ColumnIndex].Name != "colChk" && dgvGSTR3B5.Columns[oCell.ColumnIndex].Name != "colSequence")
                                            {
                                                #region VALIDATION
                                                if (sCells[i].ToString().Trim() == "") { dgvGSTR3B5.Rows[iRow].Cells[oCell.ColumnIndex].Value = DBNull.Value; }
                                                else
                                                {
                                                    if (oCell.ColumnIndex >= 1 && oCell.ColumnIndex <= 8)
                                                        dgvGSTR3B5.Rows[iRow].Cells[oCell.ColumnIndex].Value = sCells[i].Trim();
                                                    else { dgvGSTR3B5.Rows[iRow].Cells[oCell.ColumnIndex].Value = sCells[i].Trim(); }
                                                }
                                                #endregion
                                            }
                                        }
                                        else
                                        {
                                            if (iCol > i)
                                            {
                                                for (int j = oCell.ColumnIndex; j < dgvGSTR3B5.Columns.Count; j++)
                                                {
                                                    #region VALIDATION
                                                    if (sCells[i].ToString().Trim() == "") { dgvGSTR3B5.Rows[iRow].Cells[j].Value = DBNull.Value; }
                                                    else
                                                    {
                                                        if (j >= 1 && j <= 8) { dgvGSTR3B5.Rows[iRow].Cells[j].Value = sCells[i].Trim(); }
                                                        else { dgvGSTR3B5.Rows[iRow].Cells[j].Value = sCells[i].Trim(); }
                                                    }
                                                    #endregion

                                                    i++;
                                                    if (i >= sCells.Length)
                                                        break;
                                                }
                                                break;
                                            }
                                            else
                                            {
                                                for (int j = oCell.ColumnIndex; j < dgvGSTR3B5.Columns.Count; j++)
                                                {
                                                    #region VALIDATION
                                                    if (sCells[i].ToString().Trim() == "") { dgvGSTR3B5.Rows[iRow].Cells[j].Value = DBNull.Value; }
                                                    else
                                                    {
                                                        if (j >= 1 && j <= 8)
                                                            dgvGSTR3B5.Rows[iRow].Cells[j].Value = sCells[i].Trim();
                                                        else { dgvGSTR3B5.Rows[iRow].Cells[j].Value = sCells[i].Trim(); }
                                                    }
                                                    #endregion

                                                    i = i + 1;
                                                    if (i >= sCells.Length)
                                                        break;
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

        private void dgvGSTR13_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string cNo = dgvGSTR3B5.Columns[e.ColumnIndex].Name;

                if (e.RowIndex >= 0)
                {
                    if (cNo == "colInterStateSupplies" || cNo == "colIntraStateSupplies")
                    {
                        if (chkCellValue(Convert.ToString(dgvGSTR3B5.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                        {
                            dgvGSTR3B5.CellValueChanged -= dgvGSTR13_CellValueChanged;
                            dgvGSTR3B5.Rows[e.RowIndex].Cells[cNo].Value = Utility.DisplayIndianCurrency(Convert.ToString(Math.Round(Convert.ToDecimal(dgvGSTR3B5.Rows[e.RowIndex].Cells[cNo].Value), 2, MidpointRounding.AwayFromZero)));
                            dgvGSTR3B5.CellValueChanged += dgvGSTR13_CellValueChanged;
                        }
                        else
                            dgvGSTR3B5.Rows[e.RowIndex].Cells[cNo].Value = "";
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
                    if (cNo == "colInterStateSupplies" || cNo == "colIntraStateSupplies")
                    {
                        if (Utility.IsDecimalOrNumber(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else
                        return true;
                }
                else
                    return false;
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
                        #region IF IMPOTED FILE IS OPEN THEN CLOSE OPEN FILE
                        foreach (Process proc in System.Diagnostics.Process.GetProcessesByName("EXCEL"))
                        {
                            if (proc.MainWindowTitle == "Microsoft Excel - " + file.SafeFileName)
                                proc.Kill();
                        }
                        #endregion

                        // CREATE DATATABLE TO STORE MAIN GRID DATA
                        DataTable dt = new DataTable();
                        dt = (DataTable)dgvGSTR3B5.DataSource;

                        // CREATE DATATABLE TO STORE IMPOTED FILE DATA
                        DataTable dtExcel = new DataTable();
                        dtExcel = ReadExcel(filePath, fileExt, dt);

                        // CHECK IMPORTED TEMPLATE
                        if (dtExcel.Columns.Count != 1)
                        {
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                // COMBINE IMPORTED EXCEL DATA AND GRID DATA

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
                                foreach (DataGridViewColumn col in dgvGSTR3B5.Columns)
                                {
                                    dt.Columns[col.Index].ColumnName = col.Name.ToString();
                                    col.DataPropertyName = col.Name;
                                }
                                #endregion

                                //ASSIGN DATATABLE TO DATAGRID
                                dgvGSTR3B5.DataSource = dt;
                            }
                            else
                            {
                                // IF THERE ARE NO RECORDS IN MAIN GRID

                                if (dtExcel != null && dtExcel.Rows.Count > 0)
                                {
                                    // IF THERE ARE DATA IN IMPORTED EXCEL FILE

                                    #region RENAME DATATABLE COLUMN NAME AS PAR MAIN GRID
                                    foreach (DataGridViewColumn col in dgvGSTR3B5.Columns)
                                    {
                                        dtExcel.Columns[col.Index].ColumnName = col.Name.ToString();
                                        col.DataPropertyName = col.Name;
                                    }
                                    dtExcel.AcceptChanges();

                                    // ASSIGN DATATALE TO GRID
                                    dgvGSTR3B5.DataSource = dtExcel;
                                    #endregion
                                }
                                else
                                {
                                    // IF THERE ARE NO RECORDS IN IMPORTED EXCEL FILE
                                    MessageBox.Show("There are no records found in imported excel ...!!!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }

                            // TOTAL CALCULATION
                            //string[] colNo = { "colTaxableValue", "colIGST" };
                            //GetTotal(colNo);
                        }
                        else
                        {
                            MessageBox.Show("Please import valid excel template...!!!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
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
                        OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [3B_Nil$]", con);
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
                        for (int i = 1; i < dgvGSTR3B5.Columns.Count - 1; i++)
                        {
                            flg = false;
                            for (int j = 0; j < dtexcel.Columns.Count; j++)
                            {
                                // CHECK GRID COLUMN IS PRESENT OR NOT IN IMPORTED EXCEL
                                if (dgvGSTR3B5.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim() == dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim())
                                {
                                    // IF GRID COLUMN PRESENT IN EXCEL THEN ITS INDEX AS PAR GRID COLUMN INDEX
                                    flg = true;
                                    dtexcel.Columns[j].SetOrdinal(dgvGSTR3B5.Columns[i].Index - 1);
                                    break;
                                }
                                else if (dgvGSTR3B5.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim() == "" && dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim() == "f2")
                                {
                                    // IF GRID COLUMN PRESENT IN EXCEL THEN ITS INDEX AS PAR GRID COLUMN INDEX
                                    flg = true;
                                    dtexcel.Columns[j].SetOrdinal(dgvGSTR3B5.Columns[i].Index - 1);
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
                        if (dtexcel.Columns.Count >= dgvGSTR3B5.Columns.Count - 2)
                        {
                            for (int i = dtexcel.Columns.Count - 1; i > (dgvGSTR3B5.Columns.Count - 2); i--)
                            {
                                dtexcel.Columns.Remove(dtexcel.Columns[i]);
                            }
                        }
                        dtexcel.AcceptChanges();
                        #endregion

                        #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME
                        foreach (DataGridViewColumn col in dgvGSTR3B5.Columns)
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
                if (dgvGSTR3B5.Rows.Count > 1)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID

                    #region CREATE WORKBOOK AND ASSIGN COLUMNNAME
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook WB = excelApp.Workbooks.Add(Missing.Value);

                    Excel.Worksheet newWS = (Excel.Worksheet)excelApp.Worksheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    newWS.Name = "3B_Nil";

                    // DELETE UNUSED WORKSHEETS FROM WORKBOOK
                    foreach (Excel.Worksheet ws in WB.Worksheets)
                    {
                        if (ws.Name != "3B_Nil")
                            ((Excel.Worksheet)WB.Worksheets[ws.Name]).Delete();
                    }

                    // ASSIGN COLUMN HEADER AS PAR THE GRID HEADER
                    for (int i = 1; i < dgvGSTR3B5.Columns.Count; i++)
                    {
                        newWS.Cells[1, i] = dgvGSTR3B5.Columns[i].HeaderText.ToString();

                        // SET COLUMN WIDTH
                        if (i == 1)
                            ((Excel.Range)newWS.Cells[1, i]).ColumnWidth = 40;
                        else
                            ((Excel.Range)newWS.Cells[1, i]).ColumnWidth = 20;
                    }

                    // GET RANGE AND SET DIFFRENT PROPERTIES
                    Excel.Range headerRange = (Excel.Range)newWS.get_Range((Excel.Range)newWS.Cells[1, 1], (Excel.Range)newWS.Cells[1, dgvGSTR3B5.Columns.Count - 1]);
                    headerRange.WrapText = true;
                    headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Calibri";
                    #endregion

                    #region COPY DATA FROM DATATABLE TO ARRAY

                    // CREATE ARRAY TO HOLD THE DATA OF DATATABLE
                    object[,] arr = new object[dgvGSTR3B5.Rows.Count, dgvGSTR3B5.Columns.Count];

                    // ASSIGN DATA TO ARRAY FROM DATATABLE
                    if (CommonHelper.IsLicence)
                    {
                        // FOR LICENECE ALLOWS TO EXPORT ALL RECORDS
                        for (int i = 0; i < dgvGSTR3B5.Rows.Count; i++)
                        {
                            for (int j = 1; j < dgvGSTR3B5.Columns.Count; j++)
                            {
                                arr[i, j - 1] = Convert.ToString(dgvGSTR3B5.Rows[i].Cells[j].Value);
                            }
                        }
                    }
                    else
                    {
                        // FOR DEMO ALLOW ONLY 100 RECORDS TO EXPORT
                        for (int i = 0; i < dgvGSTR3B5.Rows.Count; i++)
                        {
                            if (i < 100)
                            {
                                for (int j = 1; j < dgvGSTR3B5.Columns.Count; j++)
                                {
                                    arr[i, j - 1] = Convert.ToString(dgvGSTR3B5.Rows[i].Cells[j].Value);
                                }
                            }
                        }
                    }

                    //SET EXCEL RANGE TO PASTE THE DATA
                    Excel.Range top = (Excel.Range)newWS.Cells[3, 1];
                    Excel.Range bottom = (Excel.Range)newWS.Cells[dgvGSTR3B5.Rows.Count, dgvGSTR3B5.Columns.Count];
                    Excel.Range sheetRange = newWS.Range[top, bottom];

                    //FILL ARRAY IN EXCEL
                    sheetRange.Value2 = arr;

                    #endregion

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
                #region CREATING ITEXTSHARP TABLE FROM THE DATATABLE DATA AND ASSIGNING TABLE HEADER
                PdfPTable pdfTable = new PdfPTable(dgvGSTR3B5.ColumnCount - 1);
                pdfTable.DefaultCell.Padding = 3;
                pdfTable.WidthPercentage = 100;
                pdfTable.DefaultCell.BorderWidth = 0;
                iTextSharp.text.Font fontHeader = iTextSharp.text.FontFactory.GetFont("Calibri", 6);

                // ADD HEADER TO PDF TABLE
                pdfTable = AssignHeader(pdfTable, "4. Values of exempt, nil rated and non-GST inward supplies");
                #endregion

                #region ADDING HEADER ROW
                int i = 0;

                #region HEADER1
                PdfPCell celHeader1 = new PdfPCell();

                celHeader1 = new PdfPCell(new Phrase("Nature of Supply", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Inter-State Supplies", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Intra-State Supplies", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                pdfTable.CompleteRow();
                #endregion

                Application.DoEvents();
                #endregion

                #region ADDING COLUMN NUMBER
                i = 0;
                foreach (DataGridViewColumn column in dgvGSTR3B5.Columns)
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
                    foreach (DataGridViewRow row in dgvGSTR3B5.Rows)
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
                    foreach (DataGridViewRow row in dgvGSTR3B5.Rows)
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
                ce1.Colspan = dgvGSTR3B5.Columns.Count - 1;
                ce1.VerticalAlignment = Element.ALIGN_CENTER;
                ce1.HorizontalAlignment = Element.ALIGN_LEFT;
                ce1.BackgroundColor = new iTextSharp.text.BaseColor(197, 223, 197);
                ce1.BorderWidth = 0;
                pdfTable.AddCell(ce1);

                iTextSharp.text.Font FigToRs = FontFactory.GetFont("Calibri", 10, iTextSharp.text.Font.BOLD);
                PdfPCell ceHeader2 = new PdfPCell(new Phrase("(figures in Rs)", FigToRs));
                ceHeader2.Colspan = dgvGSTR3B5.Columns.Count - 1;
                ceHeader2.VerticalAlignment = Element.ALIGN_CENTER;
                ceHeader2.HorizontalAlignment = Element.ALIGN_RIGHT;
                ceHeader2.BorderWidth = 0;
                pdfTable.AddCell(ceHeader2);

                PdfPCell ce2 = new PdfPCell(new Phrase(" "));
                ce2.Colspan = dgvGSTR3B5.Columns.Count - 1;
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

        #region JSON TRABSACTIIO
        #region Json
        public class IsupDetail
        {
            public string ty { get; set; }
            public int inter { get; set; }
            public int intra { get; set; }
        }

        public class InwardSup
        {
            public List<IsupDetail> isup_details { get; set; }
        }

        public class IntrDetails
        {
            public int iamt { get; set; }
            public int camt { get; set; }
            public int samt { get; set; }
            public int csamt { get; set; }
        }

        public class IntrLtfee
        {
            public IntrDetails intr_details { get; set; }
        }

        public class RootObject
        {
            public InwardSup inward_sup { get; set; }
            public IntrLtfee intr_ltfee { get; set; }
        }


        #endregion

        public void JSONCreator()
        {
            RootObject ObjJson = new RootObject();

            List<DataGridViewRow> Invoicelist = dgvGSTR3B5.Rows
                      .OfType<DataGridViewRow>()
                      .ToList();

            InwardSup objInwardSup = new InwardSup();
            IntrLtfee objintr_ltfee = new IntrLtfee();
            List<IsupDetail> Objisup_details = new List<IsupDetail>();
            for (int i = 0; i < Invoicelist.Count; i++)
            {
                if (i == 0 || i == 1)
                {
                    IsupDetail iIsupDetail = new IsupDetail();
                    if (i == 0)
                        iIsupDetail.ty = "GST";
                    else
                        iIsupDetail.ty = "NONGST";
                    iIsupDetail.inter = Convert.ToInt32(Invoicelist[i].Cells["colInterStateSupplies"].Value.ToString());
                    iIsupDetail.intra = Convert.ToInt32(Invoicelist[i].Cells["colIntraStateSupplies"].Value.ToString());
                    Objisup_details.Add(iIsupDetail);
                    objInwardSup.isup_details = Objisup_details;
                }
            }
            ObjJson.inward_sup = objInwardSup;

            #region File Save
            JavaScriptSerializer objScript = new JavaScriptSerializer();

            var settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.DefaultValueHandling = DefaultValueHandling.Ignore;

            objScript.MaxJsonLength = 2147483647;

            string FinalJson = JsonConvert.SerializeObject(ObjJson, settings);

            SaveFileDialog save = new SaveFileDialog();
            save.FileName = "3B5.json";
            save.Filter = "Json File | *.json";
            if (save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(save.OpenFile());
                writer.WriteLine(FinalJson);
                writer.Dispose();
                writer.Close();
            }
            #endregion
        }
        #endregion        

        private void dgvGSTR3B5_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.dgvGSTR3B5.ClearSelection();
        }

        private void frmGSTR3B5_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName("");
        }
    }
}
