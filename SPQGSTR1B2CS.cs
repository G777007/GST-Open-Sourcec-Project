using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SPEQTAGST.BAL;
using SPEQTAGST.BAL.M264r1;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Data.OleDb;

namespace SPEQTAGST.xasjbr1
{
    public partial class SPQGSTR1B2CS : Form
    {

        r1Publicclass objGSTR7 = new r1Publicclass();

        public SPQGSTR1B2CS()
        {
            InitializeComponent();

            // set grid property
            SetGridViewColor();

            // Bind data
            GetData();
            // total calculation
            string[] colNo = { "colTaxableValue", "colIGST", "colCGST", "colSGST", "colCess" };
            GetTotal(colNo);
            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName(CommonHelper.ReturnName);

            pbGSTR1.Visible = false;
            BindFilter();

            dgvGSTR171B.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dgvGSTR171B.EnableHeadersVisualStyles = false;
            dgvGSTR171B.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);
            dgvGSTR171B.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvGSTR171B.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dgvGSTR172ATotal.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void GetData()
        {
            try
            {
                // create datatable to store database data
                DataTable dt = new DataTable();
                string Query = "Select  "+
                    "Fld_Id,Fld_Sequence,Fld_SupplyType,Fld_POS,Fld_TaxableValue,Fld_Rate,Fld_IGST,Fld_CGST,Fld_SGST,Fld_Cess,Fld_GSTINofEcom, " +
                     " Fld_NameofEcom,Fld_FileStatus,Fld_Month,Fld_FinancialYear "+
                    " from SPQR1B2CS where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
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
                    // add column (chek box)

                    dt.Columns.Add(new DataColumn("colChk"));
                    dt.Columns["colChk"].SetOrdinal(0);
                    //dt.Columns.Add(new DataColumn("colError"));

                    // rename datatable column name to datagridview column name
                    foreach (DataGridViewColumn col in dgvGSTR171B.Columns)
                    {
                        dt.Columns[col.Index].ColumnName = col.Name.ToString();
                        col.DataPropertyName = col.Name;
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string ColName = dt.Columns[j].ColumnName;
                            if (ColName == "colTaxableValue" || ColName == "colIGST" || ColName == "colCGST" || ColName == "colSGST" || ColName == "colCess")
                                dt.Rows[i][j] = Utility.DisplayIndianCurrency(Convert.ToString(dt.Rows[i][j]));
                        }
                    }

                    dt.AcceptChanges();

                    // assign datatable to data grid view
                    dgvGSTR171B.DataSource = dt;
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
                if (dgvGSTR171B.Rows.Count >= 1)
                {
                    // if main grid having records

                    if (dgvGSTR172ATotal.Rows.Count == 0)
                    {
                        #region if total grid having no record
                        // create temprory datatable to store column calculation
                        DataTable dtTotal = new DataTable();

                        // add column as par datagridview column
                        foreach (DataGridViewColumn col in dgvGSTR172ATotal.Columns)
                        {
                            dtTotal.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;
                        }

                        #region ADD DATATABLE COLUMN
                        DataTable dt = new DataTable();
                        // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                        foreach (DataGridViewColumn col in dgvGSTR171B.Columns)
                        {
                            dt.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;

                            if (col.Name == "colNameofEcom")
                                dt.Columns["colNameofEcom"].DataType = typeof(System.String);

                            if (col.Name == "colRateTax")
                                dt.Columns["colRateTax"].DataType = typeof(System.String);
                        }
                        #endregion

                        #region ASSIGN GRIDVIEW ROWS IN DATATABLE
                        object[] rowValue = new object[dt.Columns.Count];

                        foreach (DataGridViewRow drn in dgvGSTR171B.Rows)
                        {
                            if (drn.Index != dgvGSTR171B.Rows.Count - 1)
                            {
                                rowValue[0] = "False";
                                for (int i = 1; i < drn.Cells.Count; i++)
                                {
                                    if (i != 4)
                                    {
                                        rowValue[i] = Convert.ToString(drn.Cells[i].Value);
                                    }
                                    else
                                    {
                                        if (drn.Cells[i].Value.ToString().Length > 0)
                                        {
                                            rowValue[i] = Convert.ToDouble(drn.Cells[i].Value);
                                        }
                                        else
                                        {
                                            rowValue[i] = 0;
                                        }
                                    }
                                }
                                dt.Rows.Add(rowValue);
                            }
                        }
                        dt.AcceptChanges();
                        #endregion

                        // create datarow to store grid column calculation
                        DataRow dr = dtTotal.NewRow();

                        #region Rate Count
                        var result2 = (from row in dt.AsEnumerable()
                                       where row.Field<string>("colNameofEcom") != "" && row.Field<string>("colRateTax") != ""
                                       group row by new { colNameofEcom = row.Field<string>("colNameofEcom"), colRateTax = row.Field<string>("colRateTax") } into grp
                                       select new
                                       {
                                           colNameofEcom = grp.Key.colNameofEcom,
                                           colRateTax = grp.Key.colRateTax
                                       }).ToList();

                        if (result2.Count != null && result2.Count > 0)
                            dr["colTRateofTax"] = result2.Count;
                        else
                            dr["colTRateofTax"] = 0;
                        #endregion

                        dr["colTTaxableValue"] = dgvGSTR171B.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colTaxableValue"].Value != null).Sum(x => x.Cells["colTaxableValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colTaxableValue"].Value)).ToString();
                        dr["colTIGST"] = dgvGSTR171B.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colIGST"].Value != null).Sum(x => x.Cells["colIGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIGST"].Value)).ToString();
                        dr["colTCGST"] = dgvGSTR171B.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCGST"].Value != null).Sum(x => x.Cells["colCGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCGST"].Value)).ToString();
                        dr["colTSGST"] = dgvGSTR171B.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colSGST"].Value != null).Sum(x => x.Cells["colSGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colSGST"].Value)).ToString();
                        dr["colTCess"] = dgvGSTR171B.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCess"].Value != null).Sum(x => x.Cells["colCess"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCess"].Value)).ToString();

                        // add datarow to datatable
                        dtTotal.Rows.Add(dr);

                        for (int i = 0; i < dtTotal.Rows.Count; i++)
                        {
                            for (int j = 0; j < dtTotal.Columns.Count; j++)
                            {
                                string ColName = dtTotal.Columns[j].ColumnName;
                                if (ColName == "colTTaxableValue" || ColName == "colTIGST" || ColName == "colTCGST" || ColName == "colTSGST" || ColName == "colTCess")
                                    dtTotal.Rows[i][j] = Utility.DisplayIndianCurrency(Convert.ToString(dtTotal.Rows[i][j]));
                            }
                        }

                        dtTotal.AcceptChanges();

                        // assign datatable to grid
                        dgvGSTR172ATotal.DataSource = dtTotal;

                        #endregion
                    }
                    else if (dgvGSTR172ATotal.Rows.Count == 1)
                    {
                        #region if total grid having only one records

                        // calculate total only specific column
                        foreach (var item in colNo)
                        {
                            if (item == "colRateTax" || item == "colNameofEcom")
                            {

                                #region ADD DATATABLE COLUMN
                                DataTable dt = new DataTable();
                                // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                                foreach (DataGridViewColumn col in dgvGSTR171B.Columns)
                                {
                                    dt.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;

                                    if (col.Name == "colNameofEcom")
                                        dt.Columns["colNameofEcom"].DataType = typeof(System.String);

                                    if (col.Name == "colRateTax")
                                        dt.Columns["colRateTax"].DataType = typeof(System.String);
                                }
                                #endregion

                                #region ASSIGN GRIDVIEW ROWS IN DATATABLE
                                object[] rowValue = new object[dt.Columns.Count];

                                foreach (DataGridViewRow drn in dgvGSTR171B.Rows)
                                {
                                    if (drn.Index != dgvGSTR171B.Rows.Count - 1)
                                    {
                                        rowValue[0] = "False";
                                        for (int i = 1; i < drn.Cells.Count; i++)
                                        {
                                            if (i != 4)
                                            {
                                                rowValue[i] = Convert.ToString(drn.Cells[i].Value);
                                            }
                                            else
                                            {
                                                if (Convert.ToString(drn.Cells[i].Value).Length > 0)
                                                {
                                                    rowValue[i] = Convert.ToDouble(drn.Cells[i].Value);
                                                }
                                                else
                                                {
                                                    rowValue[i] = 0;
                                                }
                                            }
                                        }
                                        dt.Rows.Add(rowValue);
                                    }
                                }
                                dt.AcceptChanges();
                                #endregion

                                #region Rate Count
                                var result = (from row in dt.AsEnumerable()
                                               where row.Field<string>("colNameofEcom") != "" && row.Field<string>("colRateTax") != ""
                                               group row by new { colNameofEcom = row.Field<string>("colNameofEcom"), colRateTax = row.Field<string>("colRateTax") } into grp
                                               select new
                                               {
                                                   colNameofEcom = grp.Key.colNameofEcom,
                                                   colRateTax = grp.Key.colRateTax
                                               }).ToList();

                                if (result.Count != null && result.Count > 0)
                                    dgvGSTR172ATotal.Rows[0].Cells["colTRateofTax"].Value = result.Count;
                                else
                                    dgvGSTR172ATotal.Rows[0].Cells["colTRateofTax"].Value = 0;
                                #endregion

                            }

                            if (item == "colTaxableValue")
                                dgvGSTR172ATotal.Rows[0].Cells["colTTaxableValue"].Value = Utility.DisplayIndianCurrency(dgvGSTR171B.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colTaxableValue"].Value != null).Sum(x => x.Cells["colTaxableValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colTaxableValue"].Value)).ToString());
                            else if (item == "colIGST")
                                dgvGSTR172ATotal.Rows[0].Cells["colTIGST"].Value = Utility.DisplayIndianCurrency(dgvGSTR171B.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colIGST"].Value != null).Sum(x => x.Cells["colIGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIGST"].Value)).ToString());
                            else if (item == "colCGST")
                                dgvGSTR172ATotal.Rows[0].Cells["colTCGST"].Value = Utility.DisplayIndianCurrency(dgvGSTR171B.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCGST"].Value != null).Sum(x => x.Cells["colCGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCGST"].Value)).ToString());
                            else if (item == "colSGST")
                                dgvGSTR172ATotal.Rows[0].Cells["colTSGST"].Value = Utility.DisplayIndianCurrency(dgvGSTR171B.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colSGST"].Value != null).Sum(x => x.Cells["colSGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colSGST"].Value)).ToString());
                            else if (item == "colCess")
                                dgvGSTR172ATotal.Rows[0].Cells["colTCess"].Value = Utility.DisplayIndianCurrency(dgvGSTR171B.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCess"].Value != null).Sum(x => x.Cells["colCess"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCess"].Value)).ToString());
                        }
                        #endregion
                    }

                    // set grid row height and assign total header
                    dgvGSTR172ATotal.Rows[0].Height = 30;
                    dgvGSTR172ATotal.Rows[0].Cells[0].Value = "TOTAL";
                }
                else
                {
                    // check if total grid having record

                    if (dgvGSTR172ATotal.Rows.Count >= 0)
                    {
                        #region if there are no records in main grid then assign blank datatable to total grid
                        DataTable dtTotal = new DataTable();
                        foreach (DataGridViewColumn col in dgvGSTR172ATotal.Columns)
                        {
                            dtTotal.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;
                        }
                        dgvGSTR172ATotal.DataSource = dtTotal;
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

        private void dgvGSTR171B_KeyDown(object sender, KeyEventArgs e)
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
                        if (dgvGSTR171B.Rows.Count > 0)
                        {
                            // delete selected cell in grid
                            foreach (DataGridViewCell oneCell in dgvGSTR171B.SelectedCells)
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
                    string[] colNo = { "colTaxableValue", "colIGST", "colCGST", "colSGST", "colCess" };
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
                    if (dgvGSTR171B.RowCount > 0) // IF GRID IS NOT NULL AND PAST ONLY ON SELECTED CELLS
                    {
                        foreach (DataGridViewCell oneCell in dgvGSTR171B.SelectedCells)
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
                            DisableControls(dgvGSTR171B);

                            gRowNo = dgvGSTR171B.Rows.Count - 1;
                            int no = tmp;

                            if (iRow > gRowNo - 1)
                            {
                                DataTable dtDGV = new DataTable();
                                //dtDGV = dgvGSTR172A.DataSource as DataTable;

                                #region ADD DATATABLE COLUMN

                                // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                                foreach (DataGridViewColumn col in dgvGSTR171B.Columns)
                                {
                                    dtDGV.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }
                                #endregion

                                #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                                // CREATE OBJECT ARRAY TO STORE ONE ROW DATA OF MAIN GRID
                                object[] rowValue = new object[dtDGV.Columns.Count];

                                foreach (DataGridViewRow dr in dgvGSTR171B.Rows)
                                {
                                    if (dr.Index != dgvGSTR171B.Rows.Count - 1) // DON'T ADD LAST ROW
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
                                        if (iCol + i < this.dgvGSTR171B.ColumnCount && i < this.dgvGSTR171B.ColumnCount - 1)
                                        {
                                            // skip check box column and sequance column to paste data
                                            if (iCol == 0)
                                                oCell = dgvGSTR171B[iCol + i + 2, iRow];
                                            else if (iCol == 1)
                                                oCell = dgvGSTR171B[iCol + i + 1, iRow];
                                            else
                                                oCell = dgvGSTR171B[iCol + i, iRow];

                                            sCells[i] = sCells[i].Trim().Replace(",", "");
                                            if (oCell.ColumnIndex != 0)
                                            {
                                                if (dgvGSTR171B.Columns[oCell.ColumnIndex].Name != "colChk" && dgvGSTR171B.Columns[oCell.ColumnIndex].Name != "colSequence")
                                                {
                                                    #region VALIDATION
                                                    if (sCells[i].ToString().Trim() == "") { dgvGSTR171B.Rows[iRow].Cells[oCell.ColumnIndex].Value = ""; }
                                                    else
                                                    {
                                                        if (oCell.ColumnIndex >= 2 && oCell.ColumnIndex <= dgvGSTR171B.ColumnCount)
                                                            dgvGSTR171B.Rows[iRow].Cells[oCell.ColumnIndex].Value = sCells[i].Replace("₹", "").Trim();
                                                    }
                                                    #endregion
                                                }
                                            }
                                            else
                                            {
                                                if (iCol > i)
                                                {
                                                    for (int j = oCell.ColumnIndex; j < dgvGSTR171B.Columns.Count; j++)
                                                    {
                                                        #region VALIDATION
                                                        if (sCells[i].ToString().Trim() == "") { dgvGSTR171B.Rows[iRow].Cells[j].Value = ""; }
                                                        else
                                                        {
                                                            if (j >= 2 && j <= dgvGSTR171B.ColumnCount)
                                                                dgvGSTR171B.Rows[iRow].Cells[j].Value = sCells[i].Replace("₹", "").Trim();
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
                                                    for (int j = oCell.ColumnIndex; j < dgvGSTR171B.Columns.Count; j++)
                                                    {
                                                        #region VALIDATION
                                                        if (sCells[i].ToString().Trim() == "") { dgvGSTR171B.Rows[iRow].Cells[j].Value = ""; }
                                                        else
                                                        {
                                                            if (j >= 2 && j <= dgvGSTR171B.ColumnCount)
                                                                dgvGSTR171B.Rows[iRow].Cells[j].Value = sCells[i].Replace("₹", "").Trim();
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
                    #endregion

                    // enabal main grid
                    EnableControls(dgvGSTR171B);
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
                EnableControls(dgvGSTR171B);
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
                DisableControls(dgvGSTR171B);

                #region Set datatable
                int cnt = 0, colNo = 0;

                // assign grid data to datatable
                DataTable dt = dtDGV;

                if (dt == null)
                {
                    // if no record in grid then create new daatable
                    dt = new DataTable();

                    // add column as par main grid and set data access property
                    foreach (DataGridViewColumn col in dgvGSTR171B.Columns)
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
                                if (iCol + i < this.dgvGSTR171B.ColumnCount && colNo < dgvGSTR171B.ColumnCount - 1)
                                {
                                    // skip check box column and sequance column to paste data
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
                                                if (colNo >= 2 && colNo <= dgvGSTR171B.Columns.Count)
                                                {
                                                    if (chkCellValue(sCells[i].Trim(), dgvGSTR171B.Columns[colNo].Name))
                                                    {
                                                        if (dgvGSTR171B.Columns[colNo].Name == "colSupplyType")
                                                            dt.Rows[dt.Rows.Count - 1][colNo] = Utility.Strb2csSupType(sCells[i]);
                                                        else if (dgvGSTR171B.Columns[colNo].Name == "colNameofEcom")
                                                            dt.Rows[dt.Rows.Count - 1][colNo] = Utility.strValidStateName(sCells[i]);
                                                        else
                                                            dt.Rows[dt.Rows.Count - 1][colNo] = sCells[i].Trim();
                                                    }
                                                    else
                                                        dt.Rows[dt.Rows.Count - 1][colNo] = "";
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                    else
                                    {
                                        if (iCol > i)
                                        {
                                            for (int j = colNo; j < dgvGSTR171B.Columns.Count; j++)
                                            {
                                                #region VALIDATION
                                                if (sCells[i].ToString().Trim() == "") { dt.Rows[dt.Rows.Count - 1][j] = DBNull.Value; }
                                                else
                                                {
                                                    if (j >= 2 && j <= dgvGSTR171B.Columns.Count)
                                                    {
                                                        if (chkCellValue(sCells[i].Trim(), dgvGSTR171B.Columns[j].Name))
                                                        {
                                                            if (dgvGSTR171B.Columns[j].Name == "colSupplyType")
                                                                dt.Rows[dt.Rows.Count - 1][j] = Utility.Strb2csSupType(sCells[i]);
                                                            else if (dgvGSTR171B.Columns[j].Name == "colNameofEcom")
                                                                dt.Rows[dt.Rows.Count - 1][j] = Utility.strValidStateName(sCells[i]);
                                                            else
                                                                dt.Rows[dt.Rows.Count - 1][j] = sCells[i].Trim();
                                                        }
                                                        else
                                                            dt.Rows[dt.Rows.Count - 1][j] = "";
                                                    }
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
                                            for (int j = colNo; j < dgvGSTR171B.Columns.Count; j++)
                                            {
                                                #region VALIDATION
                                                if (sCells[i].ToString().Trim() == "") { dt.Rows[dt.Rows.Count - 1][j] = DBNull.Value; }
                                                else
                                                {
                                                    if (j >= 2 && j <= dgvGSTR171B.Columns.Count)
                                                    {
                                                        if (chkCellValue(sCells[i].Trim(), dgvGSTR171B.Columns[j].Name))
                                                        {
                                                            if (dgvGSTR171B.Columns[j].Name == "colSupplyType")
                                                                dt.Rows[dt.Rows.Count - 1][j] = Utility.Strb2csSupType(sCells[i]);
                                                            else if (dgvGSTR171B.Columns[j].Name == "colNameofEcom")
                                                                dt.Rows[dt.Rows.Count - 1][j] = Utility.strValidStateName(sCells[i]);
                                                            else
                                                                dt.Rows[dt.Rows.Count - 1][j] = sCells[i].Trim();
                                                        }
                                                        else
                                                            dt.Rows[dt.Rows.Count - 1][j] = "";
                                                    }
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
                    dgvGSTR171B.DataSource = dt;

                // total calculation

                string[] colGroup = { "colTaxableValue", "colIGST", "colCGST", "colSGST", "colCess" };
                GetTotal(colGroup);

                pbGSTR1.Visible = false;

                EnableControls(dgvGSTR171B);

                #endregion
            }
            catch (Exception ex)
            {
                pbGSTR1.Visible = false;
                EnableControls(dgvGSTR171B);
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }
        
        public bool IsValidateData()
        {
            int i;
            int j;
            string SupplyType;
            bool flag;
            try
            {
                int _cnt = 0;
                string _str = "";
                this.pbGSTR1.Visible = true;
                this.dgvGSTR171B.CurrentCell = this.dgvGSTR171B.Rows[0].Cells["colChk"];
                this.dgvGSTR171B.AllowUserToAddRows = false;
                List<DataGridViewRow> list = (
                    from x in this.dgvGSTR171B.Rows.OfType<DataGridViewRow>()
                    where !Utility.b2burSupplyType(Convert.ToString(x.Cells["colSupplyType"].Value))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR171B.Rows[list[i].Cells["colSupplyType"].RowIndex].Cells["colSupplyType"].Style.BackColor = Color.LightPink;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Please select Supply Tyepe.\n");
                }
                list = (
                    from x in this.dgvGSTR171B.Rows.OfType<DataGridViewRow>()
                    where Utility.b2burSupplyType(Convert.ToString(x.Cells["colSupplyType"].Value))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    this.dgvGSTR171B.Rows[list[i].Cells["colSupplyType"].RowIndex].Cells["colSupplyType"].Style.BackColor = Color.LightGreen;
                }
                list = null;
                list = (
                    from x in this.dgvGSTR171B.Rows.OfType<DataGridViewRow>()
                    where !Utility.IsValidStateName(Convert.ToString(x.Cells["colNameofEcom"].Value))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR171B.Rows[list[i].Cells["colNameofEcom"].RowIndex].Cells["colNameofEcom"].Style.BackColor = Color.LightPink;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Please enter proper place of supply.\n");
                }
                list = (
                    from x in this.dgvGSTR171B.Rows.OfType<DataGridViewRow>()
                    where Utility.IsNumber(Convert.ToString(x.Cells["colNameofEcom"].Value))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    this.dgvGSTR171B.Rows[list[i].Cells["colNameofEcom"].RowIndex].Cells["colNameofEcom"].Style.BackColor = Color.LightGreen;
                }
                list = null;
                list = (
                    from x in this.dgvGSTR171B.Rows.OfType<DataGridViewRow>()
                    where !Utility.IsRate(Convert.ToString(x.Cells["colRateTax"].Value))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR171B.Rows[list[i].Cells["colRateTax"].RowIndex].Cells["colRateTax"].Style.BackColor = Color.LightPink;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Please enter proper Rate.\n");
                }
                list = (
                    from x in this.dgvGSTR171B.Rows.OfType<DataGridViewRow>()
                    where Utility.IsRate(Convert.ToString(x.Cells["colRateTax"].Value))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    this.dgvGSTR171B.Rows[list[i].Cells["colRateTax"].RowIndex].Cells["colRateTax"].Style.BackColor = Color.LightGreen;
                }
                list = null;
                list = (
                    from x in this.dgvGSTR171B.Rows.OfType<DataGridViewRow>()
                    where !Utility.IsPMDecimalOrNumber(Convert.ToString(x.Cells["colTaxableValue"].Value))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR171B.Rows[list[i].Cells["colTaxableValue"].RowIndex].Cells["colTaxableValue"].Style.BackColor = Color.LightPink;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Please enter proper taxable value.\n");
                }
                list = (
                    from x in this.dgvGSTR171B.Rows.OfType<DataGridViewRow>()
                    where Utility.IsPMDecimalOrNumber(Convert.ToString(x.Cells["colTaxableValue"].Value))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    this.dgvGSTR171B.Rows[list[i].Cells["colTaxableValue"].RowIndex].Cells["colTaxableValue"].Style.BackColor = Color.LightGreen;
                }
                Convert.ToString(CommonHelper.StateName);
                list = this.dgvGSTR171B.Rows.OfType<DataGridViewRow>().ToList<DataGridViewRow>();
                for (j = 0; j < list.Count; j++)
                {
                    SupplyType = Convert.ToString(this.dgvGSTR171B.Rows[list[j].Cells["colSupplyType"].RowIndex].Cells["colSupplyType"].Value);
                    if (!(SupplyType != "Intra"))
                    {
                        if (Convert.ToString(this.dgvGSTR171B.Rows[list[j].Cells["colIGST"].RowIndex].Cells["colIGST"].Value) != "")
                        {
                            if (!(Convert.ToDecimal(this.dgvGSTR171B.Rows[list[j].Cells["colIGST"].RowIndex].Cells["colIGST"].Value) > new decimal(0)))
                            {
                                this.dgvGSTR171B.Rows[list[j].Cells["colIGST"].RowIndex].Cells["colIGST"].Style.BackColor = Color.LightGreen;
                            }
                            else
                            {
                                _cnt++;
                                _str = string.Concat(_str, _cnt, ") Please enter proper Integrated tax Amount.\n");
                                this.dgvGSTR171B.Rows[list[j].Cells["colIGST"].RowIndex].Cells["colIGST"].Style.BackColor = Color.LightPink;
                            }
                        }
                        else if (Convert.ToString(this.dgvGSTR171B.Rows[list[j].Cells["colIGST"].RowIndex].Cells["colIGST"].Value) == "")
                        {
                            this.dgvGSTR171B.Rows[list[j].Cells["colIGST"].RowIndex].Cells["colIGST"].Style.BackColor = Color.LightGreen;
                        }
                    }
                    else if (Convert.ToString(this.dgvGSTR171B.Rows[list[j].Cells["colIGST"].RowIndex].Cells["colIGST"].Value) == "")
                    {
                        _cnt++;
                        _str = string.Concat(_str, _cnt, ") Please enter proper IGST Amount.\n");
                        this.dgvGSTR171B.Rows[list[j].Cells["colIGST"].RowIndex].Cells["colIGST"].Style.BackColor = Color.LightPink;
                    }
                    else if (Utility.IsICSC(Convert.ToString(this.dgvGSTR171B.Rows[list[j].Cells["colIGST"].RowIndex].Cells["colIGST"].Value)))
                    {
                        this.dgvGSTR171B.Rows[list[j].Cells["colIGST"].RowIndex].Cells["colIGST"].Style.BackColor = Color.LightGreen;
                    }
                    else if (!(Convert.ToDecimal(this.dgvGSTR171B.Rows[list[j].Cells["colIGST"].RowIndex].Cells["colIGST"].Value) > new decimal(0)))
                    {
                        this.dgvGSTR171B.Rows[list[j].Cells["colIGST"].RowIndex].Cells["colIGST"].Style.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        _cnt++;
                        _str = string.Concat(_str, _cnt, ") Please enter proper IGST Amount.\n");
                        this.dgvGSTR171B.Rows[list[j].Cells["colIGST"].RowIndex].Cells["colIGST"].Style.BackColor = Color.LightPink;
                    }
                }
                for (j = 0; j < list.Count; j++)
                {
                    SupplyType = Convert.ToString(this.dgvGSTR171B.Rows[list[j].Cells["colSupplyType"].RowIndex].Cells["colSupplyType"].Value);
                    if (!(SupplyType == "Intra"))
                    {
                        if (!(Convert.ToString(this.dgvGSTR171B.Rows[list[j].Cells["colCGST"].RowIndex].Cells["colCGST"].Value) == "" ? true : !(SupplyType != "")))
                        {
                            if (!(Convert.ToDecimal(this.dgvGSTR171B.Rows[list[j].Cells["colCGST"].RowIndex].Cells["colCGST"].Value) > new decimal(0)))
                            {
                                this.dgvGSTR171B.Rows[list[j].Cells["colCGST"].RowIndex].Cells["colCGST"].Style.BackColor = Color.LightGreen;
                            }
                            else
                            {
                                _cnt++;
                                _str = string.Concat(_str, _cnt, ") Please enter proper Central tax Amount.\n");
                                this.dgvGSTR171B.Rows[list[j].Cells["colCGST"].RowIndex].Cells["colCGST"].Style.BackColor = Color.LightPink;
                            }
                        }
                        else if (Convert.ToString(this.dgvGSTR171B.Rows[list[j].Cells["colCGST"].RowIndex].Cells["colCGST"].Value) == "")
                        {
                            this.dgvGSTR171B.Rows[list[j].Cells["colCGST"].RowIndex].Cells["colCGST"].Style.BackColor = Color.LightGreen;
                        }
                    }
                    else if (Convert.ToString(this.dgvGSTR171B.Rows[list[j].Cells["colCGST"].RowIndex].Cells["colCGST"].Value) == "")
                    {
                        _cnt++;
                        _str = string.Concat(_str, _cnt, ") Please enter proper CGST Amount.\n");
                        this.dgvGSTR171B.Rows[list[j].Cells["colCGST"].RowIndex].Cells["colCGST"].Style.BackColor = Color.LightPink;
                    }
                    else if (Utility.IsICSC(Convert.ToString(this.dgvGSTR171B.Rows[list[j].Cells["colCGST"].RowIndex].Cells["colCGST"].Value)))
                    {
                        this.dgvGSTR171B.Rows[list[j].Cells["colCGST"].RowIndex].Cells["colCGST"].Style.BackColor = Color.LightGreen;
                    }
                    else if (!(Convert.ToDecimal(this.dgvGSTR171B.Rows[list[j].Cells["colCGST"].RowIndex].Cells["colCGST"].Value) > new decimal(0)))
                    {
                        this.dgvGSTR171B.Rows[list[j].Cells["colCGST"].RowIndex].Cells["colCGST"].Style.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        _cnt++;
                        _str = string.Concat(_str, _cnt, ") Please enter proper CGST Amount.\n");
                        this.dgvGSTR171B.Rows[list[j].Cells["colCGST"].RowIndex].Cells["colCGST"].Style.BackColor = Color.LightPink;
                    }
                }
                for (j = 0; j < list.Count; j++)
                {
                    SupplyType = Convert.ToString(this.dgvGSTR171B.Rows[list[j].Cells["colSupplyType"].RowIndex].Cells["colSupplyType"].Value);
                    if (!(SupplyType == "Intra"))
                    {
                        if (!(Convert.ToString(this.dgvGSTR171B.Rows[list[j].Cells["colSGST"].RowIndex].Cells["colSGST"].Value) == "" ? true : !(SupplyType != "")))
                        {
                            if (!(Convert.ToDecimal(this.dgvGSTR171B.Rows[list[j].Cells["colSGST"].RowIndex].Cells["colSGST"].Value) > new decimal(0)))
                            {
                                this.dgvGSTR171B.Rows[list[j].Cells["colSGST"].RowIndex].Cells["colSGST"].Style.BackColor = Color.LightGreen;
                            }
                            else
                            {
                                _cnt++;
                                _str = string.Concat(_str, _cnt, ") Please enter proper State/UT tax Amount.\n");
                                this.dgvGSTR171B.Rows[list[j].Cells["colSGST"].RowIndex].Cells["colSGST"].Style.BackColor = Color.LightPink;
                            }
                        }
                        else if (Convert.ToString(this.dgvGSTR171B.Rows[list[j].Cells["colSGST"].RowIndex].Cells["colSGST"].Value) == "")
                        {
                            this.dgvGSTR171B.Rows[list[j].Cells["colSGST"].RowIndex].Cells["colSGST"].Style.BackColor = Color.LightGreen;
                        }
                    }
                    else if (Convert.ToString(this.dgvGSTR171B.Rows[list[j].Cells["colSGST"].RowIndex].Cells["colSGST"].Value) == "")
                    {
                        _cnt++;
                        _str = string.Concat(_str, _cnt, ") Please enter proper SGST Amount.\n");
                        this.dgvGSTR171B.Rows[list[j].Cells["colSGST"].RowIndex].Cells["colSGST"].Style.BackColor = Color.LightPink;
                    }
                    else if (Utility.IsICSC(Convert.ToString(this.dgvGSTR171B.Rows[list[j].Cells["colSGST"].RowIndex].Cells["colSGST"].Value)))
                    {
                        this.dgvGSTR171B.Rows[list[j].Cells["colSGST"].RowIndex].Cells["colSGST"].Style.BackColor = Color.LightGreen;
                    }
                    else if (!(Convert.ToDecimal(this.dgvGSTR171B.Rows[list[j].Cells["colSGST"].RowIndex].Cells["colSGST"].Value) > new decimal(0)))
                    {
                        this.dgvGSTR171B.Rows[list[j].Cells["colSGST"].RowIndex].Cells["colSGST"].Style.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        _cnt++;
                        _str = string.Concat(_str, _cnt, ") Please enter proper SGST Amount.\n");
                        this.dgvGSTR171B.Rows[list[j].Cells["colSGST"].RowIndex].Cells["colSGST"].Style.BackColor = Color.LightPink;
                    }
                }
                for (j = 0; j < list.Count; j++)
                {
                    if (!(Convert.ToString(this.dgvGSTR171B.Rows[list[j].Cells["colCess"].RowIndex].Cells["colCess"].Value) != ""))
                    {
                        this.dgvGSTR171B.Rows[list[j].Cells["colCess"].RowIndex].Cells["colCess"].Style.BackColor = Color.LightGreen;
                    }
                    else if (Utility.IsICSC(Convert.ToString(this.dgvGSTR171B.Rows[list[j].Cells["colCess"].RowIndex].Cells["colCess"].Value)))
                    {
                        this.dgvGSTR171B.Rows[list[j].Cells["colCess"].RowIndex].Cells["colCess"].Style.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        _cnt++;
                        _str = string.Concat(_str, _cnt, ") Please enter proper CESS Amount.\n");
                        this.dgvGSTR171B.Rows[list[j].Cells["colCess"].RowIndex].Cells["colCess"].Style.BackColor = Color.LightPink;
                    }
                }
                list = null;
                list = (
                    from x in this.dgvGSTR171B.Rows.OfType<DataGridViewRow>()
                    where Convert.ToString(x.Cells["colGSTINofECommerceOperator"].Value).Trim() != ""
                    select x).ToList<DataGridViewRow>();
                if ((list == null ? false : list.Count > 0))
                {
                    List<DataGridViewRow> list1 = null;
                    list1 = (
                        from x in list.OfType<DataGridViewRow>()
                        where !Utility.IsBlankGSTN(Convert.ToString(x.Cells["colGSTINofECommerceOperator"].Value).Trim())
                        select x).ToList<DataGridViewRow>();
                    if ((list1 == null ? true : list1.Count <= 0))
                    {
                        for (i = 0; i < list1.Count; i++)
                        {
                            this.dgvGSTR171B.Rows[list[i].Cells["colGSTINofECommerceOperator"].RowIndex].Cells["colGSTINofECommerceOperator"].Style.BackColor = Color.LightGreen;
                        }
                    }
                    else
                    {
                        for (i = 0; i < list1.Count; i++)
                        {
                            this.dgvGSTR171B.Rows[list[i].Cells["colGSTINofECommerceOperator"].RowIndex].Cells["colGSTINofECommerceOperator"].Style.BackColor = Color.LightPink;
                        }
                        _cnt++;
                        _str = string.Concat(_str, _cnt, ") Please enter proper GSTIN of E-Commerce.\n");
                    }
                }
                list = null;
                list = (
                    from x in this.dgvGSTR171B.Rows.OfType<DataGridViewRow>()
                    where Utility.IsBlankGSTN(Convert.ToString(x.Cells["colGSTINofECommerceOperator"].Value).Trim())
                    select x).ToList<DataGridViewRow>();
                if ((list == null ? false : list.Count > 0))
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR171B.Rows[list[i].Cells["colGSTINofECommerceOperator"].RowIndex].Cells["colGSTINofECommerceOperator"].Style.BackColor = Color.LightGreen;
                    }
                }
                this.dgvGSTR171B.AllowUserToAddRows = true;
                this.pbGSTR1.Visible = false;
                if (!(_str != ""))
                {
                    if (this.objGSTR7.InsertValidationFlg("GSTR1", "B2CS", "true", CommonHelper.SelectedMonth) != 1)
                    {
                        MessageBox.Show("System error.\nPlease try after sometime! SPQValidation Error", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    MessageBox.Show("Data Validation Successful.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    CommonHelper.StatusText = "Completed";
                    flag = true;
                }
                else
                {
                    CommonHelper.StatusText = "Draft";
                    if (this.objGSTR7.InsertValidationFlg("GSTR1", "B2CS", "false", CommonHelper.SelectedMonth) != 1)
                    {
                        MessageBox.Show("System error.\nPlease try after sometime! SPQValidation Error", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    if (MessageBox.Show("File Not Validated. Do you want error description in excel?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.Yes)
                    {
                        this.ExportExcelForValidatation();
                    }
                    flag = false;
                }
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                this.pbGSTR1.Visible = false;
                this.dgvGSTR171B.AllowUserToAddRows = true;
                MessageBox.Show(string.Concat("Error : ", ex.Message), "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                object[] message = new object[] { ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine };
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", message);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
                flag = false;
            }
            return flag;
        }

        

        public bool IsValidateData_old()
        {
            try
            {
                int _cnt = 0;
                string _str = "";

                pbGSTR1.Visible = true;
                dgvGSTR171B.CurrentCell = dgvGSTR171B.Rows[0].Cells["colChk"];
                dgvGSTR171B.AllowUserToAddRows = false;

                #region Supply Tyepe
                List<DataGridViewRow> list = dgvGSTR171B.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.b2burSupplyType(Convert.ToString(x.Cells["colSupplyType"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR171B.Rows[list[i].Cells["colSupplyType"].RowIndex].Cells["colSupplyType"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please select Supply Tyepe.\n";
                }
                list = dgvGSTR171B.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.b2burSupplyType(Convert.ToString(x.Cells["colSupplyType"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR171B.Rows[list[i].Cells["colSupplyType"].RowIndex].Cells["colSupplyType"].Style.BackColor = Color.White;
                }
                #endregion

                #region POS
                list = null;
                list = dgvGSTR171B.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsValidStateName(Convert.ToString(x.Cells["colNameofEcom"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR171B.Rows[list[i].Cells["colNameofEcom"].RowIndex].Cells["colNameofEcom"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper place of supply.\n";
                }
                list = dgvGSTR171B.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsNumber(Convert.ToString(x.Cells["colNameofEcom"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR171B.Rows[list[i].Cells["colNameofEcom"].RowIndex].Cells["colNameofEcom"].Style.BackColor = Color.White;
                }
                #endregion

                #region Rate
                list = null;
                list = dgvGSTR171B.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsRate(Convert.ToString(x.Cells["colRateTax"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR171B.Rows[list[i].Cells["colRateTax"].RowIndex].Cells["colRateTax"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper Rate.\n";
                }
                list = dgvGSTR171B.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsRate(Convert.ToString(x.Cells["colRateTax"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR171B.Rows[list[i].Cells["colRateTax"].RowIndex].Cells["colRateTax"].Style.BackColor = Color.White;
                }
                #endregion

                #region Taxable Value
                list = null;
                list = dgvGSTR171B.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsPMDecimalOrNumber(Convert.ToString(x.Cells["colTaxableValue"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR171B.Rows[list[i].Cells["colTaxableValue"].RowIndex].Cells["colTaxableValue"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper taxable value.\n";
                }
                list = dgvGSTR171B.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsPMDecimalOrNumber(Convert.ToString(x.Cells["colTaxableValue"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR171B.Rows[list[i].Cells["colTaxableValue"].RowIndex].Cells["colTaxableValue"].Style.BackColor = Color.White;
                }
                #endregion

                #region IGST Amount

                string gstin = Convert.ToString(CommonHelper.StateName);
                string result = gstin;

                list = dgvGSTR171B.Rows
                        .OfType<DataGridViewRow>()
                        .ToList();

                for (int j = 0; j < list.Count; j++)
                {
                    string SupplyType = Convert.ToString(dgvGSTR171B.Rows[list[j].Cells["colSupplyType"].RowIndex].Cells["colSupplyType"].Value);


                    if (SupplyType != "Intra")
                    {
                        if (Convert.ToString(dgvGSTR171B.Rows[list[j].Cells["colIGST"].RowIndex].Cells["colIGST"].Value) == "")
                        {
                            _cnt += 1;
                            _str += _cnt + ") Please enter proper IGST Amount.\n";
                            dgvGSTR171B.Rows[list[j].Cells["colIGST"].RowIndex].Cells["colIGST"].Style.BackColor = Color.Red;
                        }
                        else
                        {
                            if (!Utility.IsICSC(Convert.ToString(dgvGSTR171B.Rows[list[j].Cells["colIGST"].RowIndex].Cells["colIGST"].Value)))
                            {
                                _cnt += 1;
                                _str += _cnt + ") Please enter proper IGST Amount.\n";
                                dgvGSTR171B.Rows[list[j].Cells["colIGST"].RowIndex].Cells["colIGST"].Style.BackColor = Color.Red;
                            }
                            else
                                dgvGSTR171B.Rows[list[j].Cells["colIGST"].RowIndex].Cells["colIGST"].Style.BackColor = Color.White;
                        }
                    }
                    else if (Convert.ToString(dgvGSTR171B.Rows[list[j].Cells["colIGST"].RowIndex].Cells["colIGST"].Value) != "")
                    {
                        _cnt += 1;
                        _str += _cnt + ") Please enter proper Integrated tax Amount.\n";
                        dgvGSTR171B.Rows[list[j].Cells["colIGST"].RowIndex].Cells["colIGST"].Style.BackColor = Color.Red;
                    }
                    else if (Convert.ToString(dgvGSTR171B.Rows[list[j].Cells["colIGST"].RowIndex].Cells["colIGST"].Value) == "")
                    {
                        dgvGSTR171B.Rows[list[j].Cells["colIGST"].RowIndex].Cells["colIGST"].Style.BackColor = Color.White;
                    }
                }
                #endregion

                #region CGST Amount

                for (int j = 0; j < list.Count; j++)
                {
                    string SupplyType = Convert.ToString(dgvGSTR171B.Rows[list[j].Cells["colSupplyType"].RowIndex].Cells["colSupplyType"].Value);

                    if (SupplyType == "Intra")
                    {
                        if (Convert.ToString(dgvGSTR171B.Rows[list[j].Cells["colCGST"].RowIndex].Cells["colCGST"].Value) == "")
                        {
                            _cnt += 1;
                            _str += _cnt + ") Please enter proper CGST Amount.\n";
                            dgvGSTR171B.Rows[list[j].Cells["colCGST"].RowIndex].Cells["colCGST"].Style.BackColor = Color.Red;
                        }
                        else
                        {
                            if (!Utility.IsICSC(Convert.ToString(dgvGSTR171B.Rows[list[j].Cells["colCGST"].RowIndex].Cells["colCGST"].Value)))
                            {
                                _cnt += 1;
                                _str += _cnt + ") Please enter proper CGST Amount.\n";
                                dgvGSTR171B.Rows[list[j].Cells["colCGST"].RowIndex].Cells["colCGST"].Style.BackColor = Color.Red;
                            }
                            else
                                dgvGSTR171B.Rows[list[j].Cells["colCGST"].RowIndex].Cells["colCGST"].Style.BackColor = Color.White;
                        }
                    }
                    else if (Convert.ToString(dgvGSTR171B.Rows[list[j].Cells["colCGST"].RowIndex].Cells["colCGST"].Value) != "" && SupplyType != "")
                    {
                        _cnt += 1;
                        _str += _cnt + ") Please enter proper Central tax Amount.\n";
                        dgvGSTR171B.Rows[list[j].Cells["colCGST"].RowIndex].Cells["colCGST"].Style.BackColor = Color.Red;
                    }
                    else if (Convert.ToString(dgvGSTR171B.Rows[list[j].Cells["colCGST"].RowIndex].Cells["colCGST"].Value) == "")
                    {
                        dgvGSTR171B.Rows[list[j].Cells["colCGST"].RowIndex].Cells["colCGST"].Style.BackColor = Color.White;
                    }
                }
                #endregion

                #region SGST Amount

                for (int j = 0; j < list.Count; j++)
                {
                    string SupplyType = Convert.ToString(dgvGSTR171B.Rows[list[j].Cells["colSupplyType"].RowIndex].Cells["colSupplyType"].Value);

                    if (SupplyType == "Intra")
                    {
                        if (Convert.ToString(dgvGSTR171B.Rows[list[j].Cells["colSGST"].RowIndex].Cells["colSGST"].Value) == "")
                        {
                            _cnt += 1;
                            _str += _cnt + ") Please enter proper SGST Amount.\n";
                            dgvGSTR171B.Rows[list[j].Cells["colSGST"].RowIndex].Cells["colSGST"].Style.BackColor = Color.Red;
                        }
                        else
                        {
                            if (!Utility.IsICSC(Convert.ToString(dgvGSTR171B.Rows[list[j].Cells["colSGST"].RowIndex].Cells["colSGST"].Value)))
                            {
                                _cnt += 1;
                                _str += _cnt + ") Please enter proper SGST Amount.\n";
                                dgvGSTR171B.Rows[list[j].Cells["colSGST"].RowIndex].Cells["colSGST"].Style.BackColor = Color.Red;
                            }
                            else
                                dgvGSTR171B.Rows[list[j].Cells["colSGST"].RowIndex].Cells["colSGST"].Style.BackColor = Color.White;
                        }
                    }
                    else if (Convert.ToString(dgvGSTR171B.Rows[list[j].Cells["colSGST"].RowIndex].Cells["colSGST"].Value) != "" && SupplyType != "")
                    {
                        _cnt += 1;
                        _str += _cnt + ") Please enter proper State/UT tax Amount.\n";
                        dgvGSTR171B.Rows[list[j].Cells["colSGST"].RowIndex].Cells["colSGST"].Style.BackColor = Color.Red;
                    }
                    else if (Convert.ToString(dgvGSTR171B.Rows[list[j].Cells["colSGST"].RowIndex].Cells["colSGST"].Value) == "")
                    {
                        dgvGSTR171B.Rows[list[j].Cells["colSGST"].RowIndex].Cells["colSGST"].Style.BackColor = Color.White;
                    }
                }
                #endregion

                #region CESS Amount
                for (int j = 0; j < list.Count; j++)
                {
                    if (Convert.ToString(dgvGSTR171B.Rows[list[j].Cells["colCess"].RowIndex].Cells["colCess"].Value) != "")
                    {
                        if (!Utility.IsICSC(Convert.ToString(dgvGSTR171B.Rows[list[j].Cells["colCess"].RowIndex].Cells["colCess"].Value)))
                        {
                            _cnt += 1;
                            _str += _cnt + ") Please enter proper CESS Amount.\n";
                            dgvGSTR171B.Rows[list[j].Cells["colCess"].RowIndex].Cells["colCess"].Style.BackColor = Color.Red;
                        }
                        else
                        { dgvGSTR171B.Rows[list[j].Cells["colCess"].RowIndex].Cells["colCess"].Style.BackColor = Color.White; }
                    }
                    else
                    { dgvGSTR171B.Rows[list[j].Cells["colCess"].RowIndex].Cells["colCess"].Style.BackColor = Color.White; }
                }
                #endregion

                #region Actual value and Cumputer value different validation
                /*
                list = null;
                list = dgvGSTR171B.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Convert.ToString(x.Cells["colCGST"].Value) != "" && Convert.ToString(x.Cells["colSGST"].Value) != "" && Convert.ToString(x.Cells["colRateTax"].Value) != "" && Convert.ToString(x.Cells["colTaxableValue"].Value) != "")
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        decimal CGST = Convert.ToDecimal(dgvGSTR171B.Rows[list[i].Cells["colCGST"].RowIndex].Cells["colCGST"].Value);
                        decimal SGST = Convert.ToDecimal(dgvGSTR171B.Rows[list[i].Cells["colSGST"].RowIndex].Cells["colSGST"].Value);
                        decimal Rate = Convert.ToDecimal(dgvGSTR171B.Rows[list[i].Cells["colRateTax"].RowIndex].Cells["colRateTax"].Value);
                        decimal Tax = Convert.ToDecimal(dgvGSTR171B.Rows[list[i].Cells["colTaxableValue"].RowIndex].Cells["colTaxableValue"].Value);

                        decimal ComValue = Tax * Rate / 200;
                        ComValue = Math.Round(ComValue, 2, MidpointRounding.AwayFromZero);
                        decimal ResultCGST = ComValue - CGST;
                        decimal ResultSGST = ComValue - SGST;

                        //if (ResultCGST >= -1 && ResultCGST < 1 && ResultSGST >= -1 && ResultSGST < 1)
                        if (Convert.ToDecimal(dgvGSTR171B.Rows[list[i].Cells["colCGST"].RowIndex].Cells["colCGST"].Value) == ComValue && Convert.ToDecimal(dgvGSTR171B.Rows[list[i].Cells["colSGST"].RowIndex].Cells["colSGST"].Value) == ComValue)
                        {
                            if (dgvGSTR171B.Rows[list[i].Cells["colCGST"].RowIndex].Cells["colCGST"].Style.BackColor == Color.Red)
                                dgvGSTR171B.Rows[list[i].Cells["colCGST"].RowIndex].Cells["colCGST"].Style.BackColor = Color.White;
                            if (dgvGSTR171B.Rows[list[i].Cells["colSGST"].RowIndex].Cells["colSGST"].Style.BackColor == Color.White)
                                dgvGSTR171B.Rows[list[i].Cells["colSGST"].RowIndex].Cells["colSGST"].Style.BackColor = Color.White;
                        }
                        else
                        {
                            dgvGSTR171B.Rows[list[i].Cells["colCGST"].RowIndex].Cells["colCGST"].Style.BackColor = Color.Red;
                            dgvGSTR171B.Rows[list[i].Cells["colSGST"].RowIndex].Cells["colSGST"].Style.BackColor = Color.Red;
                            _cnt += 1;
                            _str += _cnt + ") Please enter proper CGST Amount and SGST Amount it can be no different value.\n";
                        }
                    }
                }

                list = null;
                list = dgvGSTR171B.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Convert.ToString(x.Cells["colIGST"].Value) != "" && Convert.ToString(x.Cells["colRateTax"].Value) != "" && Convert.ToString(x.Cells["colTaxableValue"].Value) != "")
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        decimal IGST = Convert.ToDecimal(dgvGSTR171B.Rows[list[i].Cells["colIGST"].RowIndex].Cells["colIGST"].Value);
                        decimal Rate = Convert.ToDecimal(dgvGSTR171B.Rows[list[i].Cells["colRateTax"].RowIndex].Cells["colRateTax"].Value);
                        decimal Tax = Convert.ToDecimal(dgvGSTR171B.Rows[list[i].Cells["colTaxableValue"].RowIndex].Cells["colTaxableValue"].Value);

                        decimal ComValue = Tax * Rate / 100;
                        ComValue = Math.Round(ComValue, 2, MidpointRounding.AwayFromZero);
                        decimal ResultIGST = ComValue - IGST;

                        //if (ResultIGST >= -1 && ResultIGST < 1)
                        if (Convert.ToDecimal(dgvGSTR171B.Rows[list[i].Cells["colIGST"].RowIndex].Cells["colIGST"].Value) == ComValue)
                        {
                            dgvGSTR171B.Rows[list[i].Cells["colIGST"].RowIndex].Cells["colIGST"].Style.BackColor = Color.White;
                        }
                        else
                        {
                            dgvGSTR171B.Rows[list[i].Cells["colIGST"].RowIndex].Cells["colIGST"].Style.BackColor = Color.Red;
                            _cnt += 1;
                            _str += _cnt + ") Please enter proper IGST Amount it can be no different value.\n";
                        }
                    }
                }
                */
                #endregion

                #region E-Com GSTIN
                list = null;
                list = dgvGSTR171B.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Convert.ToString(x.Cells["colGSTINofECommerceOperator"].Value).Trim() != "")
                       .Select(x => x)
                       .ToList();
                if (list != null && list.Count > 0)
                {
                    List<DataGridViewRow> list1 = null;
                    list1 = list
                           .OfType<DataGridViewRow>()
                           .Where(x => true != Utility.IsBlankGSTN(Convert.ToString(x.Cells["colGSTINofECommerceOperator"].Value).Trim()))
                           .Select(x => x)
                           .ToList();
                    if (list1 != null && list1.Count > 0)
                    {
                        for (int i = 0; i < list1.Count; i++)
                        {
                            dgvGSTR171B.Rows[list[i].Cells["colGSTINofECommerceOperator"].RowIndex].Cells["colGSTINofECommerceOperator"].Style.BackColor = Color.Red;
                        }
                        _cnt += 1;
                        _str += _cnt + ") Please enter proper GSTIN of E-Commerce.\n";
                    }
                    else
                    {
                        for (int i = 0; i < list1.Count; i++)
                        {
                            dgvGSTR171B.Rows[list[i].Cells["colGSTINofECommerceOperator"].RowIndex].Cells["colGSTINofECommerceOperator"].Style.BackColor = Color.White;
                        }
                    }
                }
                list = null;
                list = dgvGSTR171B.Rows
                   .OfType<DataGridViewRow>()
                   .Where(x => true == Utility.IsBlankGSTN(Convert.ToString(x.Cells["colGSTINofECommerceOperator"].Value).Trim()))
                   .Select(x => x)
                   .ToList();
                if (list != null && list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR171B.Rows[list[i].Cells["colGSTINofECommerceOperator"].RowIndex].Cells["colGSTINofECommerceOperator"].Style.BackColor = Color.White;
                    }
                }
                #endregion

                dgvGSTR171B.AllowUserToAddRows = true;
                pbGSTR1.Visible = false;

                if (_str != "")
                {
                    CommonHelper.StatusText = "Draft";
                    int _Result = objGSTR7.InsertValidationFlg("GSTR1", "B2CS", "false", CommonHelper.SelectedMonth);
                    if (_Result != 1)
                        MessageBox.Show("System error.\nPlease try after sometime! SPQValidation Error", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    DialogResult dialogResult = MessageBox.Show("File Not Validated. Do you want error description in excel?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    if (dialogResult == DialogResult.Yes)
                        ExportExcelForValidatation();

                    return false;
                }
                else
                {
                    int _Result = objGSTR7.InsertValidationFlg("GSTR1", "B2CS", "true", CommonHelper.SelectedMonth);
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
                dgvGSTR171B.AllowUserToAddRows = true;
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
                return false;
            }
        }

        private void dgvGSTR171B_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string cNo = dgvGSTR171B.Columns[e.ColumnIndex].Name;

                if (e.RowIndex >= 0)
                {
                    if (cNo == "colSupplyType") // value
                    {
                        if (chkCellValue(Convert.ToString(dgvGSTR171B.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                            dgvGSTR171B.Rows[e.RowIndex].Cells[cNo].Value = Utility.Strb2csSupType(Convert.ToString(dgvGSTR171B.Rows[e.RowIndex].Cells[cNo].Value));
                        else
                            dgvGSTR171B.Rows[e.RowIndex].Cells[cNo].Value = "";
                    }
                    else if (cNo == "colGSTINofECommerceOperator") // value
                    {
                        if (!chkCellValue(Convert.ToString(dgvGSTR171B.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                            dgvGSTR171B.Rows[e.RowIndex].Cells[cNo].Value = "";
                    }
                    else if (cNo == "colNameofEcom")
                    {
                        if (chkCellValue(Convert.ToString(dgvGSTR171B.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                            dgvGSTR171B.Rows[e.RowIndex].Cells[cNo].Value = Utility.strValidStateName(Convert.ToString(dgvGSTR171B.Rows[e.RowIndex].Cells[cNo].Value));

                        string[] colNo = { (dgvGSTR171B.Columns[e.ColumnIndex].Name) };
                        GetTotal(colNo);
                    }
                    else if (cNo == "colTaxableValue" || cNo == "colIGST" || cNo == "colCGST" || cNo == "colSGST" || cNo == "colCess") // value
                    {
                        if (chkCellValue(Convert.ToString(dgvGSTR171B.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                        {
                            if (cNo == "colTaxableValue" || cNo == "colIGST" || cNo == "colCGST" || cNo == "colSGST" || cNo == "colCess")
                            {
                                if (Convert.ToString(dgvGSTR171B.Rows[e.RowIndex].Cells[cNo].Value).Trim() != "")
                                {
                                    dgvGSTR171B.CellValueChanged -= dgvGSTR171B_CellValueChanged;
                                    dgvGSTR171B.Rows[e.RowIndex].Cells[cNo].Value = Utility.DisplayIndianCurrency(Convert.ToString(Math.Round(Convert.ToDecimal(dgvGSTR171B.Rows[e.RowIndex].Cells[cNo].Value), 2, MidpointRounding.AwayFromZero)));
                                    dgvGSTR171B.CellValueChanged += dgvGSTR171B_CellValueChanged;
                                }
                            }

                            string[] colNo = { (dgvGSTR171B.Columns[e.ColumnIndex].Name) };
                            GetTotal(colNo);
                        }
                        else { dgvGSTR171B.Rows[e.RowIndex].Cells[cNo].Value = ""; }
                    }
                    else if (cNo == "colRateTax")// Rate
                    {
                        if (!chkCellValue(Convert.ToString(dgvGSTR171B.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                            dgvGSTR171B.Rows[e.RowIndex].Cells[cNo].Value = "";

                        if (cNo == "colRateTax")
                        {
                            if (Convert.ToString(dgvGSTR171B.Rows[e.RowIndex].Cells[cNo].Value).Trim() != "")
                            {
                                dgvGSTR171B.CellValueChanged -= dgvGSTR171B_CellValueChanged;
                                dgvGSTR171B.Rows[e.RowIndex].Cells[cNo].Value = Math.Round(Convert.ToDecimal(dgvGSTR171B.Rows[e.RowIndex].Cells[cNo].Value), 2, MidpointRounding.AwayFromZero);
                                dgvGSTR171B.CellValueChanged += dgvGSTR171B_CellValueChanged;
                            }

                            string[] colNo = { (dgvGSTR171B.Columns[e.ColumnIndex].Name) };
                            GetTotal(colNo);
                        }
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

        private Boolean chkCellValue(string cellValue, string cNo)
        {
            try
            {
                if (cellValue.Trim() != "")// NOT EQUEL BLANK
                {
                    if (cNo == "colSupplyType") // Supply Type
                    {
                        if (Utility.b2csSupType(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colNameofEcom") // Place of supply
                    {
                        //if (Utility.b2csSupType(cellValue))
                        return true;
                        //else
                        //    return false;
                    }
                    else if (cNo == "colTaxableValue") // value
                    {
                        if (Utility.IsPMTaxableValue(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colIGST" || cNo == "colCGST" || cNo == "colSGST" || cNo == "colCess") // ICSC
                    {
                        if (Utility.IsPMICSC(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colRateTax") // Rate
                    {
                        if (Utility.IsRate(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colGSTINofECommerceOperator") // E-com GSTIN
                    {
                        if (Utility.IsValidGSTN(cellValue))
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

        public void Save()
        {
            try
            {
                pbGSTR1.Visible = true;

                //For text clear before save
                cmbFilter.SelectedIndex = 0;
                textBox1.Text = "";

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
                foreach (DataGridViewColumn col in dgvGSTR171B.Columns)
                {
                    dt.Columns.Add(col.Name.ToString());
                }

                // add datatable column to store file status
                dt.Columns.Add("colFileStatus");

                #endregion

                #region ASSIGN GRIDVIEW ROWS IN DATATABLE
                // create object array to store one row data of main grid
                object[] rowValue = new object[dt.Columns.Count];

                foreach (DataGridViewRow dr in dgvGSTR171B.Rows)
                {
                    if (dr.Index != dgvGSTR171B.Rows.Count - 1) // DON'T ADD LAST ROW
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
                    Query = "Delete from SPQR1B2CS where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
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
                    _Result = objGSTR7.GSTR171BBulkEntry(dt, Convert.ToString(CommonHelper.StatusText));

                    if (_Result == 1)
                    {
                        // TOTAL CALCULATION
                        string[] colNo = { "colTaxableValue", "colRateTax", "colIGST", "colCGST", "colSGST", "colCess" };
                        GetTotal(colNo);

                        #region ADD DATATABLE COLUMN
                        dt = new DataTable();

                        foreach (DataGridViewColumn col in dgvGSTR171B.Columns)
                        {
                            dt.Columns.Add(col.Name.ToString());
                        }
                        dt.Columns.Add("colFileStatus");
                        #endregion

                        #region ASSIGN GRIDVIEW ROWS IN DATATABLE
                        object[] rowVal = new object[dt.Columns.Count];

                        if (dgvGSTR172ATotal.Rows.Count == 1)
                        {
                            foreach (DataGridViewRow dr in dgvGSTR172ATotal.Rows)
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

                        _Result = objGSTR7.GSTR171BBulkEntry(dt, "Total");
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
                    Query = "Delete from SPQR1B2CS where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";

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
                        string[] colNo = { "colTaxableValue", "colIGST", "colCGST", "colSGST", "colCess" };
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
                if (dgvGSTR171B.CurrentCell.RowIndex == 0 && dgvGSTR171B.CurrentCell.ColumnIndex == 0)
                {
                    dgvGSTR171B.CurrentCell = dgvGSTR171B.Rows[0].Cells[1];
                }
                else { dgvGSTR171B.CurrentCell = dgvGSTR171B.Rows[0].Cells[0]; }


                // CREATE FLAG FRO DELETE ROWS
                Boolean flgChk = false; Boolean flgSelect = false;

                // CREATE OBJECT OF SELECTED ROW TO DELETE
                List<DataGridViewRow> toDelete = new List<DataGridViewRow>();

                // CHECK THERE ARE RECORD PRESENT IN GRID
                if (dgvGSTR171B.Rows.Count > 1)
                {
                    // FLAG TRUE IF CHECK ALL SELECTED
                    if (ckboxHeader.Checked)
                        flgChk = true;

                    #region ADD SELECTED ROW TO OBJECT FOR DELETE
                    for (int i = 0; i < dgvGSTR171B.Rows.Count - 1; i++)
                    {
                        if (dgvGSTR171B[0, i].Value != null && dgvGSTR171B[0, i].Value.ToString() != "")
                        {
                            if (Convert.ToBoolean(dgvGSTR171B[0, i].Value) == true)
                            {
                                // ADD ROW TO OBJECT IF ROW IS SELECTED
                                flgSelect = true;
                                toDelete.Add(dgvGSTR171B.Rows[i]);
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
                            pbGSTR1.Visible = true;

                            #region DELETE RECORDS

                            if (flgChk)
                            {
                                // IF CHECK BOX OF CHECK ALL IS SELECTED
                                flgChk = false;

                                // CREATE DATATABLE AND ADD COLUMN AS PAR MAIN GRID
                                DataTable dt = new DataTable();
                                foreach (DataGridViewColumn col in dgvGSTR171B.Columns)
                                {
                                    dt.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }

                                // ASSIGN BLANK DATATABLE TO GRID
                                dgvGSTR171B.DataSource = dt;
                            }
                            else if (flgSelect == true)
                            {
                                // DELETE SELECTED ROW
                                foreach (DataGridViewRow row in toDelete)
                                {
                                    dgvGSTR171B.Rows.RemoveAt(row.Index);
                                    Application.DoEvents();
                                }
                            }

                            // SEQUANCING MAIN GRID RECORDS
                            for (int i = 0; i < dgvGSTR171B.Rows.Count - 1; i++)
                            {
                                dgvGSTR171B.Rows[i].Cells["colSequence"].Value = Convert.ToString(i + 1);
                            }

                            if (dgvGSTR171B.Rows.Count == 1)
                            {
                                // IF THERE ARE NO RECORDS IN MAIN GRID THENE ASSIGN BLANK DATATABLE TO TOTAL GRID
                                DataTable dtTotal = new DataTable();
                                foreach (DataGridViewColumn col in dgvGSTR172ATotal.Columns)
                                {
                                    dtTotal.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }
                                dgvGSTR172ATotal.DataSource = dtTotal;
                            }

                            // SET CONTROL PROPERTY AFTER ROW DELETION
                            ckboxHeader.Checked = false;
                            dgvGSTR171B.Columns[0].HeaderText = "Check All";
                            #endregion
                        }
                    }

                    pbGSTR1.Visible = false;

                    // TOTAL CALCULATION
                    string[] colNo = { "colTaxableValue", "colIGST", "colCGST", "colSGST", "colCess" };
                    GetTotal(colNo);
                }
                else
                {
                    // IF THERE ARE NO RECORD TO DELETE
                    ckboxHeader.Checked = false;
                    dgvGSTR171B.Columns[0].HeaderText = "Check All";
                    MessageBox.Show("There are no records to delete..", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                pbGSTR1.Visible = false;
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

        #region EXCEL TRANSACTION

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
                        foreach (DataGridViewColumn col in dgvGSTR171B.Columns)
                        {
                            dt.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;
                        }
                        #endregion

                        #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                        // CREATE OBJECT ARRAY TO STORE ONE ROW DATA OF MAIN GRID
                        object[] rowValue = new object[dt.Columns.Count];

                        foreach (DataGridViewRow dr in dgvGSTR171B.Rows)
                        {
                            if (dr.Index != dgvGSTR171B.Rows.Count - 1) // DON'T ADD LAST ROW
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
                                DisableControls(dgvGSTR171B);

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
                                        dt.Rows[dt.Rows.Count - 1]["colSequence"] = dt.Rows.Count;
                                        Application.DoEvents();
                                    }
                                }
                                dt.AcceptChanges();
                                #endregion

                                #region RENAME DATATABLE COLUMN NAME AS PAR MAIN GRID
                                foreach (DataGridViewColumn col in dgvGSTR171B.Columns)
                                {
                                    dt.Columns[col.Index].ColumnName = col.Name.ToString();
                                    col.DataPropertyName = col.Name;
                                }
                                #endregion

                                //ASSIGN DATATABLE TO DATAGRID
                                dgvGSTR171B.DataSource = dt;

                                // ENABLE MAIN GRID
                                EnableControls(dgvGSTR171B);
                            }
                            else
                            {
                                // IF THERE ARE NO RECORDS IN MAIN GRID

                                if (dtExcel != null && dtExcel.Rows.Count > 0)
                                {
                                    // IF THERE ARE DATA IN IMPORTED EXCEL FILE

                                    // DISABLE MAIN GRID
                                    DisableControls(dgvGSTR171B);

                                    #region RENAME DATATABLE COLUMN NAME AS PAR MAIN GRID
                                    foreach (DataGridViewColumn col in dgvGSTR171B.Columns)
                                    {
                                        if (col.Index != 11 && col.Index != 12)
                                        {
                                            dtExcel.Columns[col.Index].ColumnName = col.Name.ToString();
                                            col.DataPropertyName = col.Name;
                                        }
                                        else
                                        {
                                            if (col.Index != 12)
                                            {
                                                dtExcel.Columns[col.Index].ColumnName = "colError";
                                                col.DataPropertyName = "colError";
                                            }
                                        }
                                    }
                                    dtExcel.AcceptChanges();

                                    // ASSIGN DATATALE TO GRID
                                    dgvGSTR171B.DataSource = dtExcel;

                                    // ENABLE MAIN GRID
                                    EnableControls(dgvGSTR171B);
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
                            string[] colNo = { "colTaxableValue", "colIGST", "colCGST", "colSGST", "colCess" };
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
                EnableControls(dgvGSTR171B);
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
                        OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [b2cs$]", con);
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
                        if (dtexcel.Columns.Count >= dgvGSTR171B.Columns.Count - 1)
                        {
                            for (int k = dtexcel.Columns.Count - 1; k > (dgvGSTR171B.Columns.Count - 6); k--)
                            {
                                dtexcel.Columns.Remove(dtexcel.Columns[k]);
                            }
                        }
                        dtexcel.AcceptChanges();
                        #endregion

                        flg = false;
                        #region VALIDATE TEMPLATE
                        for (int i = 2; i < dgvGSTR171B.Columns.Count; i++)
                        {
                            for (int j = 0; j < dtexcel.Columns.Count; j++)
                            {
                                // CHECK GRID COLUMN IS PRESENT OR NOT IN IMPORTED EXCEL
                                if (dgvGSTR171B.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim() == dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim())
                                {
                                    // IF GRID COLUMN PRESENT IN EXCEL THEN ITS INDEX AS PAR GRID COLUMN INDEX
                                    flg = true;
                                    //dtexcel.Columns[j].SetOrdinal(dgvGSTR171B.Columns[i].Index - 2);
                                    dtexcel.Columns[j].SetOrdinal(dgvGSTR171B.Columns[i - 2].Index);
                                    break;
                                }
                            }
                            if (flg == false)
                            {
                                // IF GRID COLUMN NOT PRESENT IN EXCEL THEN RETURN DATATABLE WITH ERROR
                                DataTable dt = new DataTable();
                                dt.Columns.Add("colError");
                                //return dt;
                            }
                            dtexcel.AcceptChanges();
                        }
                        #endregion


                        dtexcel.Columns.Add("colSequence");
                        dtexcel.Columns[dtexcel.Columns.Count - 1].SetOrdinal(0);
                        dtexcel.Columns.Add("colSupplyType");
                        dtexcel.Columns["colSupplyType"].SetOrdinal(1);
                        dtexcel.Columns["Place Of Supply"].SetOrdinal(2);
                        dtexcel.Columns["Rate"].SetOrdinal(3);
                        dtexcel.Columns["Taxable Value"].SetOrdinal(4);
                        dtexcel.Columns["IGST Amount"].SetOrdinal(5);
                        dtexcel.Columns["CGST Amount"].SetOrdinal(6);
                        dtexcel.Columns["SGST Amount"].SetOrdinal(7);
                        dtexcel.Columns["CESS Amount"].SetOrdinal(8);
                        dtexcel.Columns["E-Commerce GSTIN"].SetOrdinal(9);

                        #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME
                        foreach (DataGridViewColumn col in dgvGSTR171B.Columns)
                        {
                            if (col.Index != 0 && col.Index != 11 && col.Index != 12)
                                dtexcel.Columns[col.Index - 1].ColumnName = col.Name.ToString();
                        }
                        #endregion

                        // ADD CHECK BOX COLUMN TO DATATBLE AND MAKE IT FIRST TABLE COLUMN
                        //dtexcel.Columns.Add(new DataColumn("colSequence"));
                        //dtexcel.Columns["colSequence"].SetOrdinal(0);
                        dtexcel.Columns.Add(new DataColumn("colChk"));
                        dtexcel.Columns["colChk"].SetOrdinal(0);
                        dtexcel.Columns.Add("colError");
                        dtexcel.AcceptChanges();

                        #region SET COLTAX VALUE AS TRUE/FALSE
                        for (int i = 0; i < dtexcel.Rows.Count; i++)
                        {
                            dtexcel.Rows[i]["colChk"] = "False";
                            dtexcel.Rows[i]["colSequence"] = i + 1;

                            if (!Utility.IsValidStateName(Convert.ToString(dtexcel.Rows[i]["colNameofEcom"]).Trim()))
                                dtexcel.Rows[i]["colNameofEcom"] = "";

                            int StateId = 0;
                            int CompanyGSTNState = Convert.ToInt16(CommonHelper.CompanyGSTN.Substring(0, 2));
                            string Pos = CommonHelper.GetStateCode(Convert.ToString(dtexcel.Rows[i]["colNameofEcom"]));
                            if (Pos.Length > 2)
                                StateId = Convert.ToInt16(Pos.Split('-')[0]);
                            else
                                StateId = Convert.ToInt16(Pos);

                            if (StateId == CompanyGSTNState)
                            {
                                dtexcel.Rows[i]["colSupplyType"] = "Intra";
                            }
                            else
                            {
                                dtexcel.Rows[i]["colSupplyType"] = "Inter";
                            }
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

                // return datatable+
                return dtexcel;
            }
        }

        public void ExportExcelForValidatation()
        {
            List<int> listValid = new List<int>();
            try
            {
                if (dgvGSTR171B.Rows.Count > 1)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID

                    pbGSTR1.Visible = true;

                    #region CREATE WORKBOOK AND ASSIGN COLUMNNAME
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook WB = excelApp.Workbooks.Add(Missing.Value);

                    Excel.Worksheet newWS = (Excel.Worksheet)excelApp.Worksheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    newWS.Name = "export";

                    // DELETE UNUSED WORKSHEETS FROM WORKBOOK
                    foreach (Excel.Worksheet ws in WB.Worksheets)
                    {
                        if (ws.Name != "export")
                            ((Excel.Worksheet)WB.Worksheets[ws.Name]).Delete();
                    }

                    // ASSIGN COLUMN HEADER AS PAR THE GRID HEADER
                    int yy = 1;
                    for (int i = 2; i < dgvGSTR171B.Columns.Count + 1; i++)
                    {
                        newWS.Cells[1, i - 1] = dgvGSTR171B.Columns[yy].HeaderText.ToString();

                        ((Excel.Range)newWS.Cells[1, i - 1]).ColumnWidth = 17;
                        yy++;
                    }

                    ((Excel.Range)newWS.Cells[1, 13]).ColumnWidth = 45;
                    // GET RANGE AND SET DIFFRENT PROPERTIES
                    Excel.Range headerRange = (Excel.Range)newWS.get_Range((Excel.Range)newWS.Cells[1, 1], (Excel.Range)newWS.Cells[1, dgvGSTR171B.Columns.Count]);
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
                    foreach (DataGridViewColumn column in dgvGSTR171B.Columns)
                        dt.Columns.Add(column.Name, typeof(string));

                    for (int k = 0; k < dgvGSTR171B.Rows.Count; k++)
                    {
                        for (int j = 0; j < dgvGSTR171B.ColumnCount; j++)
                        {
                            if (dgvGSTR171B.Rows[k].Cells[j].Style.BackColor == Color.Red)
                            {
                                ExcelValidFlag = true;
                                //sheetRange.Cells[k + 1, j - 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                            }
                        }
                        if (ExcelValidFlag == true)
                        {
                            dt.Rows.Add();
                            int count = dt.Rows.Count - 1;
                            for (int b = 0; b < dgvGSTR171B.Columns.Count; b++)
                            {
                                dt.Rows[count][b] = dgvGSTR171B.Rows[k].Cells[b].Value;
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
                    sheetRange.NumberFormat = "@";

                    //FILL ARRAY IN EXCEL
                    //bool ExcelValidFlag = false;
                    object[,] Excelarr = new object[0, 0];
                    //DataTable dt = new DataTable();

                    listValid = listValid.Distinct().ToList();
                    int[] array = listValid.ToArray();
                    int Ab = 0;
                    DataTable dt_new = new DataTable();
                    dt_new = dt.Clone();

                    for (int k = 0; k < dgvGSTR171B.Rows.Count; k++)
                    {
                        string str_error = "";
                        int cnt = 1;
                        for (int j = 0; j < dgvGSTR171B.ColumnCount; j++)
                        {
                            if (dgvGSTR171B.Rows[k].Cells[j].Style.BackColor == Color.Red)
                            {
                                ExcelValidFlag = true;
                                sheetRange.Cells[Ab + 1, j].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                if (dgvGSTR171B.Columns[j].Name == "colSupplyType")
                                {
                                    if (Convert.ToString(dgvGSTR171B.Rows[k].Cells[j].Value).Trim() == "")
                                        str_error += cnt + ") " + " Please select " + dgvGSTR171B.Columns[j].HeaderText + ".\n";
                                }
                                else if (dgvGSTR171B.Columns[j].Name == "colNameofEcom")
                                {
                                    if (Convert.ToString(dgvGSTR171B.Rows[k].Cells[j].Value).Trim() == "")
                                        str_error += cnt + ") " + " Please select " + dgvGSTR171B.Columns[j].HeaderText + ".\n";
                                }
                                else if (dgvGSTR171B.Columns[j].Name == "colRateTax")
                                {
                                    if (Convert.ToString(dgvGSTR171B.Rows[k].Cells[j].Value).Trim() == "")
                                        str_error += cnt + ") " + " Please enter " + dgvGSTR171B.Columns[j].HeaderText + "(Ex : (0),(2.5),(3),(5),(12),(18),(28)). \n";
                                    else
                                        str_error += cnt + ") " + " Please enter proper " + dgvGSTR171B.Columns[j].HeaderText + "(Ex : (0),(2.5),(3),(5),(12),(18),(28)).\n";
                                }
                                else if (dgvGSTR171B.Columns[j].Name == "colTaxableValue")
                                {
                                    if (Convert.ToString(dgvGSTR171B.Rows[k].Cells[j].Value).Trim() == "")
                                        str_error += cnt + ") " + " Please enter " + dgvGSTR171B.Columns[j].HeaderText + ".\n";
                                }

                                else if (dgvGSTR171B.Columns[j].Name == "colIGST")
                                {
                                    if (Convert.ToString(dgvGSTR171B.Rows[k].Cells[j].Value).Trim() == "")
                                        str_error += cnt + ") " + " Please enter Must be " + dgvGSTR171B.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") " + dgvGSTR171B.Columns[j].HeaderText + " is not applicable for Intra State. Please enter exact match " + dgvGSTR171B.Columns[j].HeaderText + " base on `Total Taxable Value` and `Rate` calculation. \n";
                                }
                                else if (dgvGSTR171B.Columns[j].Name == "colCGST")
                                {
                                    if (Convert.ToString(dgvGSTR171B.Rows[k].Cells[j].Value).Trim() == "")
                                        str_error += cnt + ") " + " Please enter Must be " + dgvGSTR171B.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") " + dgvGSTR171B.Columns[j].HeaderText + " is not applicable for Inter State. Please enter exact match " + dgvGSTR171B.Columns[j].HeaderText + " base on `Total Taxable Value` and `Rate` calculation or CGST & SGST values must be same. \n";
                                }
                                else if (dgvGSTR171B.Columns[j].Name == "colSGST")
                                {
                                    if (Convert.ToString(dgvGSTR171B.Rows[k].Cells[j].Value).Trim() == "")
                                        str_error += cnt + ") " + " Please enter Must be " + dgvGSTR171B.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") " + dgvGSTR171B.Columns[j].HeaderText + " is not applicable for Inter State. Please enter exact match " + dgvGSTR171B.Columns[j].HeaderText + " base on `Total Taxable Value` and `Rate` calculation or CGST & SGST values must be same. \n";
                                }
                                else if (dgvGSTR171B.Columns[j].Name == "colCess")
                                {
                                    if (Convert.ToString(dgvGSTR171B.Rows[k].Cells[j].Value).Trim() == "")
                                        str_error += cnt + ") " + " Please enter Must be " + dgvGSTR171B.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") " + " Please enter proper " + dgvGSTR171B.Columns[j].HeaderText + ".\n";
                                }
                                else
                                {
                                    str_error += cnt + ") " + " Please enter proper " + dgvGSTR171B.Columns[j].HeaderText + ".\n";
                                }
                                cnt++;
                            }
                        }
                        if (ExcelValidFlag == true)
                        {
                            Ab++;
                            dt_new.Rows.Add();
                            int c = dt_new.Rows.Count;
                            for (int b = 0; b < dgvGSTR171B.Columns.Count; b++)
                            {
                                if (dt_new.Columns.Count - 1 == b)
                                {
                                    dt_new.Rows[c - 1][b] = str_error;
                                }
                                else
                                {
                                    dt_new.Rows[c - 1][b] = Convert.ToString(dgvGSTR171B.Rows[k].Cells[b].Value);
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

        public void ExportExcel()
        {
            try
            {
                if (dgvGSTR171B.Rows.Count > 1)
                {
                    // if records are present in main grid

                    pbGSTR1.Visible = true;

                    #region Create Workbook and assign columnName
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook WB = excelApp.Workbooks.Add(Missing.Value);

                    Excel.Worksheet newWS = (Excel.Worksheet)excelApp.Worksheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    newWS.Name = "B2CS";

                    // Delete unused worksheets from workbook
                    foreach (Excel.Worksheet ws in WB.Worksheets)
                    {
                        if (ws.Name != "B2CS")
                            ((Excel.Worksheet)WB.Worksheets[ws.Name]).Delete();
                    }

                    // assign column header as par the grid header
                    for (int i = 1; i < dgvGSTR171B.Columns.Count; i++)
                    {
                        newWS.Cells[1, i] = dgvGSTR171B.Columns[i].HeaderText.ToString();

                        // set column width
                        if (i == 1)
                            ((Excel.Range)newWS.Cells[1, i]).ColumnWidth = 7;
                        else if (i >= 2 && i <= 11)
                            ((Excel.Range)newWS.Cells[1, i]).ColumnWidth = 10;
                        else
                            ((Excel.Range)newWS.Cells[1, i]).ColumnWidth = 15;
                    }

                    // get range and set range properties
                    Excel.Range headerRange = (Excel.Range)newWS.get_Range((Excel.Range)newWS.Cells[1, 1], (Excel.Range)newWS.Cells[1, dgvGSTR171B.Columns.Count - 1]);
                    headerRange.WrapText = true;
                    headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Calibri";

                    #endregion

                    #region Copy Data from DataTable to Array

                    if (dgvGSTR171B.Rows.Count <= 0)
                        throw new Exception("ExportToExcel: There are no records in grid...!!!\n");

                    // Create Array to hold the data of DataTable                
                    object[,] arr = new object[dgvGSTR171B.Rows.Count - 1, dgvGSTR171B.Columns.Count];

                    // Assign data to Array from DataTable
                    if (CommonHelper.IsLicence)
                    {
                        // for licenece allows to export all records
                        for (int i = 0; i < dgvGSTR171B.Rows.Count - 1; i++)
                        {
                            for (int j = 1; j < dgvGSTR171B.Columns.Count; j++)
                            {
                                arr[i, j - 1] = Convert.ToString(dgvGSTR171B.Rows[i].Cells[j].Value);
                            }
                        }
                    }
                    else
                    {
                        // for demo allow only 100 records to export
                        for (int i = 0; i < dgvGSTR171B.Rows.Count - 1; i++)
                        {
                            if (i < 100)
                            {
                                for (int j = 1; j < dgvGSTR171B.Columns.Count; j++)
                                {
                                    arr[i, j - 1] = Convert.ToString(dgvGSTR171B.Rows[i].Cells[j].Value);
                                }
                            }
                        }
                    }

                    //Set Excel Range to Paste the Data
                    Excel.Range top = (Excel.Range)newWS.Cells[2, 1];
                    Excel.Range bottom = (Excel.Range)newWS.Cells[dgvGSTR171B.Rows.Count, dgvGSTR171B.Columns.Count];
                    Excel.Range sheetRange = newWS.Range[top, bottom];

                    //Fill Array in Excel
                    sheetRange.Value2 = arr;

                    #endregion

                    pbGSTR1.Visible = false;

                    #region Exporting to Excel
                    SaveFileDialog saveExcel = new SaveFileDialog();
                    saveExcel.Filter = "Execl files (*.xlsx)|*.xlsx";
                    saveExcel.Title = "Save excel File";
                    saveExcel.ShowDialog();

                    if (saveExcel.FileName != "")
                    {
                        #region close imported file
                        foreach (Process proc in System.Diagnostics.Process.GetProcessesByName("EXCEL"))
                        {
                            string fName = System.IO.Path.GetFileName(saveExcel.FileName);
                            if (proc.MainWindowTitle == "Microsoft Excel - " + fName)
                                proc.Kill();
                        }
                        #endregion

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

                        newWS.SaveAs(saveExcel.FileName);
                        excelApp.Quit();
                        MessageBox.Show("Excel file saved!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    #endregion
                }
                else
                {
                    // if there are no record in main  grid
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

        #region Filter
        private void BindFilter()
        {
            try
            {
                List<colList> lstColumns = new List<colList>();
                for (int i = 0; i < dgvGSTR171B.Columns.Count; i++)
                {
                    if (i > 1)
                    {
                        string HeaderText = dgvGSTR171B.Columns[i].HeaderText;
                        string Name = dgvGSTR171B.Columns[i].Name;
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

        public void SetGridViewColor()
        {
            try
            {
                // DO NOT ALLOW TO AUTO GENERATE COLUMNS
                dgvGSTR171B.AutoGenerateColumns = false;

                // SET HEIGHT WIDTH OF FORM
                this.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.68));
                this.Height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.68));

                // SET WIDTH OF HEADER, MAIN AND TOTAL GRID
                this.pnlHeader.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.67));
                this.dgvGSTR171B.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.67));
                this.dgvGSTR172ATotal.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.67));

                // SET HEIGHT OF MAIN GRID
                this.dgvGSTR171B.Height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.52));
                this.dgvGSTR172ATotal.Height = 42;

                // SET LOCATION OF HEADER,LOADING PIC, CHECKBOX AND MAIN AND TOTAL GRID
                this.pnlHeader.Location = new System.Drawing.Point(10, 5);
                this.lnkClose.Location = new System.Drawing.Point(lnkClose.Location.X - 80, lnkClose.Location.Y);
                this.dgvGSTR171B.Location = new System.Drawing.Point(10, Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.085)));
                this.dgvGSTR172ATotal.Location = new System.Drawing.Point(10, Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.616)));
                //this.pbGSTR1.Location = new System.Drawing.Point(Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.45)), Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.30)));

                // SET MAIN GRID PROPERTY
                dgvGSTR171B.EnableHeadersVisualStyles = false;
                dgvGSTR171B.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(23, 196, 187);
                dgvGSTR171B.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

                this.dgvGSTR171B.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                this.dgvGSTR171B.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                this.dgvGSTR171B.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

                foreach (DataGridViewColumn column in dgvGSTR171B.Columns)
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
                DataTable dt = (DataTable)dgvGSTR171B.DataSource;
                if (dt == null)
                {
                    MessageBox.Show("Kindly save record(s) before search!!!", "Alert", MessageBoxButtons.OK);
                    return;
                }
                if (cmbFilter.SelectedValue.ToString() == "")
                {
                    ((DataTable)dgvGSTR171B.DataSource).DefaultView.RowFilter = string.Format("colSupplyType like '%{0}%' or colNameofEcom like '%{0}%' or colRateTax like '%{0}%' or colTaxableValue like '%{0}%' or colIGST like '%{0}%' or colCGST like '%{0}%' or colSGST like '%{0}%' or colCess like '%{0}%' or colGSTINofECommerceOperator like '%{0}%' or colNameofECommerceOperator like '%{0}%'", textBox1.Text.Trim().Replace("'", "''"));
                }
                else
                {
                    ((DataTable)dgvGSTR171B.DataSource).DefaultView.RowFilter = string.Format("" + cmbFilter.SelectedValue + " like '%{0}%'", textBox1.Text.Trim().Replace("'", "''"));
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

        private void ckboxHeader_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                // IF THERE ARE RECORDS IN MAIN GRID
                if (dgvGSTR171B.Rows.Count > 1)
                {
                    pbGSTR1.Visible = true;
                    if (ckboxHeader.Checked)
                    {
                        // IF CHECK BOX IS CHECKED

                        // SET CHECK BOX COLUMN VALUE AS TRUE
                        for (int i = 0; i < dgvGSTR171B.Rows.Count - 1; i++)
                        {
                            dgvGSTR171B.Rows[i].Cells[0].Value = "True";
                            Application.DoEvents();
                        }

                        // CHANGE HEADER TEXT AND WIDTH OF COLUMN AND POSITION OF CHECK BOX OF CHECK ALL COLUMN
                        //dgvGSTR171B.Columns[0].DefaultCellStyle.NullValue = true;
                        dgvGSTR171B.Columns[0].HeaderText = "Uncheck All";
                    }
                    else if (ckboxHeader.Checked == false)
                    {
                        // IF CHECK BOX IS UNCHECKED

                        // SET CHECK BOX COLUMN VALUE AS FALSE
                        for (int i = 0; i < dgvGSTR171B.Rows.Count - 1; i++)
                        {
                            dgvGSTR171B.Rows[i].Cells[0].Value = "False";
                            Application.DoEvents();
                        }

                        // CHANGE HEADER TEXT AND WIDTH OF COLUMN AND POSITION OF CHECK BOX OF CHECK ALL COLUMN
                        //dgvGSTR171B.Columns[0].DefaultCellStyle.NullValue = false;
                        dgvGSTR171B.Columns[0].HeaderText = "Check All";
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

        public DataTable LINQResultToDataTable<T>(IEnumerable<T> Linqlist)
        {
            DataTable dt = new DataTable();


            PropertyInfo[] columns = null;

            if (Linqlist == null) return dt;

            foreach (T Record in Linqlist)
            {

                if (columns == null)
                {
                    columns = ((Type)Record.GetType()).GetProperties();
                    foreach (PropertyInfo GetProperty in columns)
                    {
                        Type colType = GetProperty.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dt.Columns.Add(new DataColumn(GetProperty.Name, colType));
                    }
                }

                DataRow dr = dt.NewRow();

                foreach (PropertyInfo pinfo in columns)
                {
                    dr[pinfo.Name] = pinfo.GetValue(Record, null) == null ? DBNull.Value : pinfo.GetValue
                    (Record, null);
                }

                dt.Rows.Add(dr);
            }
            return dt;
        }

        private void dgvGSTR171B_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                // CHECK FIRST COLUMN HEADER PRESSED AND MAIN GRID HAVING RECORDS
                if (e.ColumnIndex == 0 && dgvGSTR171B.Rows.Count > 1)
                {
                    // CHECK AND UNCHECK CHECK BOX OF HEADER FOR SELECTING AND UNSELECTING ALL RECORDS
                    if (dgvGSTR171B.Columns[e.ColumnIndex].HeaderText == "Check All")
                        ckboxHeader.Checked = true;
                    else if (dgvGSTR171B.Columns[e.ColumnIndex].HeaderText == "Uncheck All")
                        ckboxHeader.Checked = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvGSTR171B_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                dgvGSTR171B.Rows[e.Row.Index - 1].Cells["colSequence"].Value = e.Row.Index;
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
                if (c.Name != "dgvGSTR172A" && c.Name != "dgvGSTR172ATotal")
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
        private void dgvGSTR171B_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                // set total grid offset as par main grid scrol
                this.dgvGSTR172ATotal.HorizontalScrollingOffset = this.dgvGSTR171B.HorizontalScrollingOffset;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvGSTR171BTotal_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                // set main grid offset as par total grid scrol
                this.dgvGSTR171B.HorizontalScrollingOffset = this.dgvGSTR172ATotal.HorizontalScrollingOffset;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        private void dgvGSTR172ATotal_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.dgvGSTR172ATotal.ClearSelection();

            if (dgvGSTR172ATotal.Rows.Count == 1)
            {
                dgvGSTR172ATotal.Rows[0].Height = 30;
            }
        }

        private void lnkClose_Click(object sender, EventArgs e)
        {
            //frmGSTR1B2CSummary obj = new frmGSTR1B2CSummary();
            //obj.MdiParent = this.MdiParent;
            //Utility.CloseAllOpenForm();
            //obj.Dock = DockStyle.Fill;             
            //obj.Show();


            ((SPQMDI)Application.OpenForms["SPQMDI"]).HideExtraToolsMenu();
            ((SPQMDI)Application.OpenForms["SPQMDI"]).HideCompanyDetailMenu();
        }

        private void frmGSTR171B_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName("");
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

    }
}
