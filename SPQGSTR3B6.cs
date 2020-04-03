using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using SPEQTAGST.BAL;
using SPEQTAGST.BAL.M796r3b;
//using SPEQTAGST.BAL.S458r3;
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
using System.Web.Script.Serialization;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace SPEQTAGST.rintlcs3b
{
    public partial class SPQGSTR3B6 : Form
    {
        r3bPublicclass objGSTR3 = new r3bPublicclass();

        public SPQGSTR3B6()
        {
            InitializeComponent();
            BindData();
            GetData();
            SetGridViewColor();

            // TOTAL CALCULATION
            int[] colNo = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
            GetTotal(colNo);

            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName(CommonHelper.ReturnName);

            dgvGSTR3B6.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dgvGSTR3B6.EnableHeadersVisualStyles = false;
            dgvGSTR3B6.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);
            dgvGSTR3B6.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvGSTR3B6.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dgvGSTR310Total.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void GetData()
        {
            try
            {
                // CREATE DATATABLE TO STORE DATABASE DATA
                DataTable dt = new DataTable();
                string Query = "Select * from SPQR3BTaxPayment where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
                Application.DoEvents();

                // GET DATA FROM DATABASE
                dt = objGSTR3.GetDataGSTR3B(Query);

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
                    dt.Columns.Remove("Fld_Sequence");
                    dt.Columns.Remove("Fld_FileStatus");
                    // RENAME DATATABLE COLUMN NAME TO DATAGRIDVIEW COLUMN NAME
                    foreach (DataGridViewColumn col in dgvGSTR3B6.Columns)
                    {
                        dt.Columns[col.Index].ColumnName = col.Name.ToString();
                        col.DataPropertyName = col.Name;
                    }
                    dt.AcceptChanges();

                    // ASSIGN DATATABLE TO DATA GRID VIEW
                    dgvGSTR3B6.DataSource = dt;
                    Application.DoEvents();
                }
                else
                {
                    BindClearData();
                    ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(0);
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

        public void GetTotal(int[] colNo)
        {
            try
            {
                if (dgvGSTR3B6.Rows.Count > 1)
                {
                    // IF MAIN GRID HAVING RECORDS

                    if (dgvGSTR310Total.Rows.Count == 0)
                    {
                        #region CHECK MAIN GRID DATA
                        bool fl = false;
                        if (dgvGSTR3B6.Rows.Count == 4)
                        {
                            for (int i = 0; i < dgvGSTR3B6.Rows.Count; i++)
                            {
                                for (int j = 1; j < dgvGSTR3B6.Columns.Count; j++)
                                {
                                    if (dgvGSTR3B6.Rows[i].Cells[j].Value != null)
                                    {
                                        if (dgvGSTR3B6.Rows[i].Cells[j].Value.ToString().Trim() != "")
                                        {
                                            fl = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        // check if main grid having proper value
                        if (fl)
                        {
                            #region IF TOTAL GRID HAVING NO RECORD
                            // CREATE TEMPORARY DATATABLE TO STORE COLUMN CALCULATION
                            DataTable dtTotal = new DataTable();

                            // ADD COLUMN AS PAR DATAGRIDVIEW COLUMN
                            foreach (DataGridViewColumn col in dgvGSTR310Total.Columns)
                            {
                                dtTotal.Columns.Add(col.Name.ToString());
                                col.DataPropertyName = col.Name;
                            }

                            // CREATE DATAROW TO STORE GRID COLUMN CALCULATION
                            DataRow dr = dtTotal.NewRow();
                            dr["Fld_TDescription"] = "Total";
                            dr["Fld_TOtRcTaxPay"] = dgvGSTR3B6.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[1].Value != null).Sum(x => x.Cells[1].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[1].Value)).ToString();
                            dr["Fld_TIGST"] = dgvGSTR3B6.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[2].Value != null).Sum(x => x.Cells[2].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[2].Value)).ToString();
                            dr["Fld_TCGST"] = dgvGSTR3B6.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[3].Value != null).Sum(x => x.Cells[3].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[3].Value)).ToString();
                            dr["Fld_TSGST"] = dgvGSTR3B6.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[4].Value != null).Sum(x => x.Cells[4].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[4].Value)).ToString();
                            dr["Fld_TCESS"] = dgvGSTR3B6.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[5].Value != null).Sum(x => x.Cells[5].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[5].Value)).ToString();
                            dr["Fld_TOtRcTaxPayCash"] = dgvGSTR3B6.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[6].Value != null).Sum(x => x.Cells[6].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[6].Value)).ToString();
                            dr["Fld_TRcTaxPay"] = dgvGSTR3B6.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[7].Value != null).Sum(x => x.Cells[7].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[7].Value)).ToString();
                            dr["Fld_TRcTaxPayCash"] = dgvGSTR3B6.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[8].Value != null).Sum(x => x.Cells[8].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[8].Value)).ToString();
                            dr["Fld_TInterestPay"] = dgvGSTR3B6.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[9].Value != null).Sum(x => x.Cells[9].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[9].Value)).ToString();
                            dr["Fld_TInterestPayCash"] = dgvGSTR3B6.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[10].Value != null).Sum(x => x.Cells[10].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[10].Value)).ToString();
                            dr["Fld_TLateFeePay"] = dgvGSTR3B6.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[11].Value != null).Sum(x => x.Cells[11].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[11].Value)).ToString();
                            dr["Fld_TLateFeePayCash"] = dgvGSTR3B6.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[12].Value != null).Sum(x => x.Cells[12].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[12].Value)).ToString();
                            dr["Fld_TUtilizableCash"] = dgvGSTR3B6.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[13].Value != null).Sum(x => x.Cells[13].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[13].Value)).ToString();
                            dr["Fld_TAdditionalCash"] = dgvGSTR3B6.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[14].Value != null).Sum(x => x.Cells[14].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[14].Value)).ToString();

                            // ADD DATAROW TO DATATABLE
                            dtTotal.Rows.Add(dr);

                            for (int i = 0; i < dtTotal.Rows.Count; i++)
                            {
                                for (int j = 0; j < dtTotal.Columns.Count; j++)
                                {
                                    string ColName = dtTotal.Columns[j].ColumnName;
                                    if (ColName == "Fld_TOtRcTaxPay" || ColName == "Fld_TIGST" || ColName == "Fld_TCGST" || ColName == "Fld_TSGST" || ColName == "Fld_TCESS" || ColName == "Fld_TOtRcTaxPayCash" || ColName == "Fld_TRcTaxPay" || ColName == "Fld_TRcTaxPayCash" || ColName == "Fld_TInterestPay" || ColName == "Fld_TInterestPayCash" || ColName == "Fld_TLateFeePay" || ColName == "Fld_TLateFeePayCash" || ColName == "Fld_TUtilizableCash" || ColName == "Fld_TAdditionalCash")
                                        dtTotal.Rows[i][j] = Utility.DisplayIndianCurrency(Convert.ToString(dtTotal.Rows[i][j]));
                                }
                            }

                            dtTotal.AcceptChanges();

                            // ASSIGN DATATABLE TO GRID
                            dgvGSTR310Total.DataSource = dtTotal;
                            #endregion
                        }
                        //else
                        //{
                        //    #region IF THERE ARE NO RECORDS IN MAIN GRID THEN ASSIGN BLANK DATATABLE TO TOTAL GRID
                        //    DataTable dtTotal = new DataTable();
                        //    foreach (DataGridViewColumn col in dgvGSTR310Total.Columns)
                        //    {
                        //        dtTotal.Columns.Add(col.Name.ToString());
                        //        col.DataPropertyName = col.Name;
                        //    }
                        //    DataRow dr = dtTotal.NewRow();
                        //    dr["colTotal"] = "Total";
                        //    dtTotal.Rows.Add(dr);
                        //    dgvGSTR310Total.DataSource = dtTotal;
                        //    #endregion
                        //}
                    }
                    else if (dgvGSTR310Total.Rows.Count == 1)
                    {
                        #region IF TOTAL GRID HAVING ONLY ONE RECORDS

                        // CALCULATE TOTAL ONLY SPECIFIC COLUMN
                        foreach (var item in colNo)
                        {
                            if (item == 1)
                                dgvGSTR310Total.Rows[0].Cells[1].Value = dgvGSTR3B6.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[1].Value != null).Sum(x => x.Cells[1].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[1].Value)).ToString();
                            if (item == 2)
                                dgvGSTR310Total.Rows[0].Cells[2].Value = Utility.DisplayIndianCurrency(dgvGSTR3B6.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[2].Value != null).Sum(x => x.Cells[2].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[2].Value)).ToString());
                            if (item == 3)
                                dgvGSTR310Total.Rows[0].Cells[3].Value = Utility.DisplayIndianCurrency(dgvGSTR3B6.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[3].Value != null).Sum(x => x.Cells[3].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[3].Value)).ToString());
                            else if (item == 4)
                                dgvGSTR310Total.Rows[0].Cells[4].Value = Utility.DisplayIndianCurrency(dgvGSTR3B6.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[4].Value != null).Sum(x => x.Cells[4].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[4].Value)).ToString());
                            else if (item == 5)
                                dgvGSTR310Total.Rows[0].Cells[5].Value = Utility.DisplayIndianCurrency(dgvGSTR3B6.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[5].Value != null).Sum(x => x.Cells[5].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[5].Value)).ToString());
                            else if (item == 6)
                                dgvGSTR310Total.Rows[0].Cells[6].Value = Utility.DisplayIndianCurrency(dgvGSTR3B6.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[6].Value != null).Sum(x => x.Cells[6].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[6].Value)).ToString());
                            else if (item == 7)
                                dgvGSTR310Total.Rows[0].Cells[7].Value = Utility.DisplayIndianCurrency(dgvGSTR3B6.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[7].Value != null).Sum(x => x.Cells[7].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[7].Value)).ToString());
                            else if (item == 8)
                                dgvGSTR310Total.Rows[0].Cells[8].Value = Utility.DisplayIndianCurrency(dgvGSTR3B6.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[8].Value != null).Sum(x => x.Cells[8].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[8].Value)).ToString());
                            else if (item == 9)
                                dgvGSTR310Total.Rows[0].Cells[9].Value = Utility.DisplayIndianCurrency(dgvGSTR3B6.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[9].Value != null).Sum(x => x.Cells[9].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[9].Value)).ToString());
                            else if (item == 10)
                                dgvGSTR310Total.Rows[0].Cells[10].Value = Utility.DisplayIndianCurrency(dgvGSTR3B6.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[10].Value != null).Sum(x => x.Cells[10].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[10].Value)).ToString());
                            else if (item == 11)
                                dgvGSTR310Total.Rows[0].Cells[11].Value = Utility.DisplayIndianCurrency(dgvGSTR3B6.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[11].Value != null).Sum(x => x.Cells[11].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[11].Value)).ToString());
                            else if (item == 12)
                                dgvGSTR310Total.Rows[0].Cells[12].Value = Utility.DisplayIndianCurrency(dgvGSTR3B6.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[12].Value != null).Sum(x => x.Cells[12].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[12].Value)).ToString());
                            else if (item == 13)
                                dgvGSTR310Total.Rows[0].Cells[13].Value = Utility.DisplayIndianCurrency(dgvGSTR3B6.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[13].Value != null).Sum(x => x.Cells[13].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[13].Value)).ToString());
                            else if (item == 14)
                                dgvGSTR310Total.Rows[0].Cells[14].Value = Utility.DisplayIndianCurrency(dgvGSTR3B6.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[14].Value != null).Sum(x => x.Cells[14].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells[14].Value)).ToString());
                        }
                        #endregion
                    }


                    // SET GRID ROW HEIGHT AND ASSIGN TOTAL HEADER
                    if (dgvGSTR310Total.Rows.Count > 0)
                    {
                        dgvGSTR310Total.Rows[0].Height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.045));
                        dgvGSTR310Total.Rows[0].Cells[0].Value = "TOTAL";
                    }
                }
                else
                {
                    // CHECK IF TOTAL GRID HAVING RECORD

                    if (dgvGSTR310Total.Rows.Count >= 0)
                    {
                        #region IF THERE ARE NO RECORDS IN MAIN GRID THEN ASSIGN BLANK DATATABLE TO TOTAL GRID
                        DataTable dtTotal = new DataTable();
                        foreach (DataGridViewColumn col in dgvGSTR310Total.Columns)
                        {
                            dtTotal.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;
                        }
                        dgvGSTR310Total.DataSource = dtTotal;
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

        private void BindData()
        {
            try
            {
                if (dgvGSTR3B6.Rows.Count <= 1)
                {
                    DataTable dt = new DataTable();

                    // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                    foreach (DataGridViewColumn col in dgvGSTR3B6.Columns)
                    {
                        dt.Columns.Add(col.Name.ToString());
                        col.DataPropertyName = col.Name;
                    }
                    dt.AcceptChanges();

                    DataRow dr = dt.NewRow();
                    dr[0] = "Integrated Tax";
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr[0] = "Central Tax";
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr[0] = "State/UT Tax";
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr[0] = "Cess";
                    dt.Rows.Add(dr);

                    // assign datatable to main grid
                    dgvGSTR3B6.DataSource = dt;

                    DataGridViewRow row = this.dgvGSTR3B6.RowTemplate;
                    row.MinimumHeight = 30;
                }
                else
                {
                    DataGridViewRow row = this.dgvGSTR3B6.RowTemplate;
                    row.MinimumHeight = 30;
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

        private void BindClearData()
        {
            try
            {
                DataTable dt = new DataTable();

                // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                foreach (DataGridViewColumn col in dgvGSTR3B6.Columns)
                {
                    dt.Columns.Add(col.Name.ToString());
                    col.DataPropertyName = col.Name;
                }
                dt.AcceptChanges();

                DataRow dr = dt.NewRow();
                dr[0] = "Integrated Tax";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = "Central Tax";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = "State/UT Tax";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = "Cess";
                dt.Rows.Add(dr);

                // assign datatable to main grid
                dgvGSTR3B6.DataSource = dt;

                DataGridViewRow row = this.dgvGSTR3B6.RowTemplate;
                row.MinimumHeight = 30;
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

        private void dgvGSTR39A_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete)
                {
                    #region DELETE SELECTED CELLS
                    try
                    {
                        if (dgvGSTR3B6.Rows.Count > 0)
                        {
                            foreach (DataGridViewCell oneCell in dgvGSTR3B6.SelectedCells)
                            {
                                if (oneCell.ColumnIndex != 0)
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

                    // TOTAL CALCULATION
                    int[] colNo = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
                    GetTotal(colNo);
                }
                if (e.KeyCode == Keys.V)
                {
                    #region PAST FROM EXCELL SHEET

                    string s = Clipboard.GetText();
                    string[] lines = s.Split('\n');
                    int iRow = 0, iCol = 0;

                    #region PAST ON SELECTED CELLS ONLY
                    if (dgvGSTR3B6.RowCount > 0) // IF GRID IS NOT NULL AND PAST ONLY ON SELECTED CELLS
                    {
                        foreach (DataGridViewCell oneCell in dgvGSTR3B6.SelectedCells)
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
                            if (iRow < dgvGSTR3B6.RowCount && line.Length > 0)
                            {
                                string[] sCells = line.Split('\t');

                                for (int i = 0; i < sCells.GetLength(0); ++i)
                                {
                                    if (iCol + i < this.dgvGSTR3B6.ColumnCount && i < 15)
                                    {
                                        if (iCol == 0)
                                            oCell = dgvGSTR3B6[iCol + i + 1, iRow];
                                        else
                                            oCell = dgvGSTR3B6[iCol + i, iRow];

                                        sCells[i] = sCells[i].Trim().Replace(",", "");
                                        if (oCell.ColumnIndex != 0)
                                        {
                                            if (!dgvGSTR3B6.Rows[iRow].Cells[oCell.ColumnIndex].ReadOnly)
                                            {
                                                #region VALIDATION
                                                if (sCells[i].ToString().Trim() == "") { dgvGSTR3B6.Rows[iRow].Cells[oCell.ColumnIndex].Value = DBNull.Value; }
                                                else
                                                {
                                                    if (oCell.ColumnIndex >= 1 && oCell.ColumnIndex <= 15)
                                                    {
                                                        if (chkCellValue(sCells[i].Trim(), oCell.ColumnIndex))
                                                            dgvGSTR3B6.Rows[iRow].Cells[oCell.ColumnIndex].Value = sCells[i].Trim();
                                                        else
                                                            dgvGSTR3B6.Rows[iRow].Cells[oCell.ColumnIndex].Value = DBNull.Value;
                                                    }
                                                    else { dgvGSTR3B6.Rows[iRow].Cells[oCell.ColumnIndex].Value = sCells[i].Trim(); }
                                                }
                                                #endregion
                                            }
                                        }
                                        else
                                        {
                                            if (iCol > i)
                                            {
                                                for (int j = oCell.ColumnIndex; j < dgvGSTR3B6.Columns.Count; j++)
                                                {
                                                    #region VALIDATION
                                                    if (sCells[i].ToString().Trim() == "") { dgvGSTR3B6.Rows[iRow].Cells[j].Value = DBNull.Value; }
                                                    else
                                                    {
                                                        if (j >= 1 && j <= 15)
                                                        {
                                                            if (chkCellValue(sCells[i].Trim(), j))
                                                                dgvGSTR3B6.Rows[iRow].Cells[j].Value = sCells[i].Trim();
                                                            else
                                                                dgvGSTR3B6.Rows[iRow].Cells[j].Value = DBNull.Value;
                                                        }
                                                        else { dgvGSTR3B6.Rows[iRow].Cells[j].Value = sCells[i].Trim(); }
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
                                                for (int j = oCell.ColumnIndex; j < dgvGSTR3B6.Columns.Count; j++)
                                                {
                                                    #region VALIDATION
                                                    if (sCells[i].ToString().Trim() == "") { dgvGSTR3B6.Rows[iRow].Cells[j].Value = DBNull.Value; }
                                                    else
                                                    {
                                                        if (j >= 1 && j <= 15)
                                                        {
                                                            if (chkCellValue(sCells[i].Trim(), j))
                                                                dgvGSTR3B6.Rows[iRow].Cells[j].Value = sCells[i].Trim();
                                                            else
                                                                dgvGSTR3B6.Rows[iRow].Cells[j].Value = DBNull.Value;
                                                        }
                                                        else { dgvGSTR3B6.Rows[iRow].Cells[j].Value = sCells[i].Trim(); }
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
                if ((e.Control && (e.KeyCode == Keys.A || e.KeyCode == Keys.Subtract)) || (e.KeyCode == Keys.Space && Control.ModifierKeys == Keys.Shift) || (e.Alt && e.KeyCode == Keys.F4))
                {
                    e.Handled = true;
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

        public bool IsValidateData()
        {
            try
            {
                int _cnt = 0;
                string _str = "";

                for (int i = 0; i < dgvGSTR3B6.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < dgvGSTR3B6.Columns.Count; j++)
                    {
                        if (Convert.ToString(dgvGSTR3B6.Rows[i].Cells[j].Value) != "")// NOT EQUEL BLANK
                        {
                            #region VALIDATION
                            if (j == 1)//IGST Rate
                            {
                                if (Utility.IsNumber(Convert.ToString(dgvGSTR3B6.Rows[i].Cells[j].Value)))
                                {
                                    if (Convert.ToInt32(Convert.ToString(dgvGSTR3B6.Rows[i].Cells[j].Value)) > 100)
                                    {
                                        dgvGSTR3B6.Rows[i].Cells[j].Style.BackColor = Color.Red;
                                        _cnt += 1;
                                        _str += _cnt + ") Sequence No. " + dgvGSTR3B6.Rows[i].Cells[1].Value + " -> Please enter proper IGST Rate (accept maximum value 100).\n";
                                    }
                                    else
                                    { dgvGSTR3B6.Rows[i].Cells[j].Style.BackColor = Color.White; }
                                }
                                else
                                {
                                    dgvGSTR3B6.Rows[i].Cells[j].Style.BackColor = Color.Red;
                                    _cnt += 1;
                                    _str += _cnt + ") Sequence No. " + dgvGSTR3B6.Rows[i].Cells[1].Value + " -> Please enter proper IGST Rate (accept maximum value 100).\n";
                                }
                            }
                            else if (j == 2)//IGST Amount
                            {
                                if (!Utility.IsNumber(Convert.ToString(dgvGSTR3B6.Rows[i].Cells[j].Value)))
                                {
                                    dgvGSTR3B6.Rows[i].Cells[j].Style.BackColor = Color.Red;
                                    _cnt += 1;
                                    _str += _cnt + ") Sequence No. " + dgvGSTR3B6.Rows[i].Cells[1].Value + " -> Please enter proper IGST Amount.\n";
                                }
                                else { dgvGSTR3B6.Rows[i].Cells[j].Style.BackColor = Color.White; }
                            }
                            else if (j == 3)//CGST Rate
                            {
                                if (Utility.IsNumber(Convert.ToString(dgvGSTR3B6.Rows[i].Cells[j].Value)))
                                {
                                    if (Convert.ToInt32(Convert.ToString(dgvGSTR3B6.Rows[i].Cells[j].Value)) > 100)
                                    {
                                        dgvGSTR3B6.Rows[i].Cells[j].Style.BackColor = Color.Red;
                                        _cnt += 1;
                                        _str += _cnt + ") Sequence No. " + dgvGSTR3B6.Rows[i].Cells[1].Value + " -> Please enter proper CGST Rate (accept maximum value 100).\n";
                                    }
                                    else
                                    { dgvGSTR3B6.Rows[i].Cells[j].Style.BackColor = Color.White; }
                                }
                                else
                                {
                                    dgvGSTR3B6.Rows[i].Cells[j].Style.BackColor = Color.Red;
                                    _cnt += 1;
                                    _str += _cnt + ") Sequence No. " + dgvGSTR3B6.Rows[i].Cells[1].Value + " -> Please enter proper CGST Rate (accept maximum value 100).\n";
                                }
                            }
                            else if (j == 4)//CGST Amount
                            {
                                if (!Utility.IsNumber(Convert.ToString(dgvGSTR3B6.Rows[i].Cells[j].Value)))
                                {
                                    dgvGSTR3B6.Rows[i].Cells[j].Style.BackColor = Color.Red;
                                    _cnt += 1;
                                    _str += _cnt + ") Sequence No. " + dgvGSTR3B6.Rows[i].Cells[1].Value + " -> Please enter proper CGST Amount.\n";
                                }
                                else { dgvGSTR3B6.Rows[i].Cells[j].Style.BackColor = Color.White; }
                            }
                            else if (j == 5)//SGST Rate
                            {
                                if (Utility.IsNumber(Convert.ToString(dgvGSTR3B6.Rows[i].Cells[j].Value)))
                                {
                                    if (Convert.ToInt32(Convert.ToString(dgvGSTR3B6.Rows[i].Cells[j].Value)) > 100)
                                    {
                                        dgvGSTR3B6.Rows[i].Cells[j].Style.BackColor = Color.Red;
                                        _cnt += 1;
                                        _str += _cnt + ") Sequence No. " + dgvGSTR3B6.Rows[i].Cells[1].Value + " -> Please enter proper SGST Rate (accept maximum value 100).\n";
                                    }
                                    else
                                    { dgvGSTR3B6.Rows[i].Cells[j].Style.BackColor = Color.White; }
                                }
                                else
                                {
                                    dgvGSTR3B6.Rows[i].Cells[j].Style.BackColor = Color.Red;
                                    _cnt += 1;
                                    _str += _cnt + ") Sequence No. " + dgvGSTR3B6.Rows[i].Cells[1].Value + " -> Please enter proper SGST Rate (accept maximum value 100).\n";
                                }
                            }
                            else if (j == 6)//SGST Amount
                            {
                                if (!Utility.IsNumber(Convert.ToString(dgvGSTR3B6.Rows[i].Cells[j].Value)))
                                {
                                    dgvGSTR3B6.Rows[i].Cells[j].Style.BackColor = Color.Red;
                                    _cnt += 1;
                                    _str += _cnt + ") Sequence No. " + dgvGSTR3B6.Rows[i].Cells[1].Value + " -> Please enter proper SGST Amount.\n";
                                }
                                else { dgvGSTR3B6.Rows[i].Cells[j].Style.BackColor = Color.White; }
                            }
                            else if (j == 7)//CESS Rate
                            {
                                if (Utility.IsNumber(Convert.ToString(dgvGSTR3B6.Rows[i].Cells[j].Value)))
                                {
                                    if (Convert.ToInt32(Convert.ToString(dgvGSTR3B6.Rows[i].Cells[j].Value)) > 100)
                                    {
                                        dgvGSTR3B6.Rows[i].Cells[j].Style.BackColor = Color.Red;
                                        _cnt += 1;
                                        _str += _cnt + ") Sequence No. " + dgvGSTR3B6.Rows[i].Cells[1].Value + " -> Please enter proper CESS Rate (accept maximum value 100).\n";
                                    }
                                    else
                                    { dgvGSTR3B6.Rows[i].Cells[j].Style.BackColor = Color.White; }
                                }
                                else
                                {
                                    dgvGSTR3B6.Rows[i].Cells[j].Style.BackColor = Color.Red;
                                    _cnt += 1;
                                    _str += _cnt + ") Sequence No. " + dgvGSTR3B6.Rows[i].Cells[1].Value + " -> Please enter proper CESS Rate (accept maximum value 100).\n";
                                }
                            }
                            else if (j == 8)//CESS Amount
                            {
                                if (!Utility.IsNumber(Convert.ToString(dgvGSTR3B6.Rows[i].Cells[j].Value)))
                                {
                                    dgvGSTR3B6.Rows[i].Cells[j].Style.BackColor = Color.Red;
                                    _cnt += 1;
                                    _str += _cnt + ") Sequence No. " + dgvGSTR3B6.Rows[i].Cells[1].Value + " -> Please enter proper SGST Amount.\n";
                                }
                                else { dgvGSTR3B6.Rows[i].Cells[j].Style.BackColor = Color.White; }
                            }
                            else { dgvGSTR3B6.Rows[i].Cells[j].Style.BackColor = Color.White; }
                            #endregion
                        }
                    }
                }
                if (_str != "")
                {
                    //MessageBox.Show(_str);
                    CommonHelper.ErrorList = Convert.ToString(_str);
                    SPQErrorList obj = new SPQErrorList();
                    obj.ShowDialog();
                    return false;
                }
                else { MessageBox.Show("Data Validation Successful.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk); }
                return true;
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

        private Boolean chkCellValue(string cellValue, int cNo)
        {
            try
            {
                int iRow = 0; string iCol = "";

                if (dgvGSTR3B6.RowCount > 0) // IF GRID IS NOT NULL AND PAST ONLY ON SELECTED CELLS
                {
                    foreach (DataGridViewCell oneCell in dgvGSTR3B6.SelectedCells)
                    {
                        if (oneCell.Selected)
                        {
                            iCol = dgvGSTR3B6.Columns[oneCell.ColumnIndex].Name;
                            iRow = oneCell.RowIndex;

                            if (iCol == "colStateUTTax" && iRow == 1)
                            {
                                dgvGSTR3B6.Rows[iRow].Cells[oneCell.ColumnIndex].Value = "";
                            }
                            if (iCol == "colCentarlTax" && iRow == 2)
                            {
                                dgvGSTR3B6.Rows[iRow].Cells[oneCell.ColumnIndex].Value = "";
                            }
                            if (iCol == "colIntegratedTax" && iRow == 3)
                            {
                                dgvGSTR3B6.Rows[iRow].Cells[oneCell.ColumnIndex].Value = "";
                            }
                            if (iCol == "colCentarlTax" && iRow == 3)
                            {
                                dgvGSTR3B6.Rows[iRow].Cells[oneCell.ColumnIndex].Value = "";
                            }
                            if (iCol == "colStateUTTax" && iRow == 3)
                            {
                                dgvGSTR3B6.Rows[iRow].Cells[oneCell.ColumnIndex].Value = "";
                            }
                        }
                    }

                    if (cNo == 1 || cNo == 3 || cNo == 5 || cNo == 7 || cNo == 2 || cNo == 4 || cNo == 6 || cNo == 8 || cNo == 9) // Rate
                    {
                        if (Utility.IsDecimalOrNumber(cellValue))
                        {
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

        private void dgvGSTR310_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int cNo = e.ColumnIndex;

                if (e.RowIndex >= 0)
                {

                    if (!chkCellValue(Convert.ToString(dgvGSTR3B6.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                        dgvGSTR3B6.Rows[e.RowIndex].Cells[cNo].Value = "";

                    if (cNo == 1 || cNo == 3 || cNo == 5 || cNo == 7 || cNo == 2 || cNo == 4 || cNo == 6 || cNo == 8 || cNo == 9 || cNo == 10 || cNo == 11 || cNo == 12 || cNo == 13 || cNo == 14) // value
                    {
                        if (chkCellValue(Convert.ToString(dgvGSTR3B6.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                        {
                            dgvGSTR3B6.CellValueChanged -= dgvGSTR310_CellValueChanged;
                            dgvGSTR3B6.Rows[e.RowIndex].Cells[cNo].Value = Utility.DisplayIndianCurrency(Convert.ToString(Math.Round(Convert.ToDecimal(dgvGSTR3B6.Rows[e.RowIndex].Cells[cNo].Value), 2, MidpointRounding.AwayFromZero)));
                            dgvGSTR3B6.CellValueChanged += dgvGSTR310_CellValueChanged;

                            int[] colNo = { e.ColumnIndex };
                            GetTotal(colNo);
                        }
                        else { dgvGSTR3B6.Rows[e.RowIndex].Cells[cNo].Value = ""; }

                        #region CHECK SGST & CGST VALIDATION
                        /*
                        decimal cGST = 0;
                        decimal sGST = 0;

                        if (dgvGSTR3B6.Rows[0].Cells["colCentarlTax"].Value != null && Convert.ToString(dgvGSTR3B6.Rows[0].Cells["colCentarlTax"].Value).Trim() != "")
                        {
                            cGST = Convert.ToDecimal(dgvGSTR3B6.Rows[0].Cells["colCentarlTax"].Value);
                        }
                        if (dgvGSTR3B6.Rows[0].Cells["colStateUTTax"].Value != null && Convert.ToString(dgvGSTR3B6.Rows[0].Cells["colStateUTTax"].Value).Trim() != "")
                        {
                            sGST = Convert.ToDecimal(dgvGSTR3B6.Rows[0].Cells["colStateUTTax"].Value);
                        }

                        if (cGST != sGST)
                        {
                            dgvGSTR3B6.Rows[0].Cells["colCentarlTax"].Style.BackColor = Color.Red;
                            dgvGSTR3B6.Rows[0].Cells["colStateUTTax"].Style.BackColor = Color.Red;
                        }
                        else
                        {
                            dgvGSTR3B6.Rows[0].Cells["colCentarlTax"].Style.BackColor = Color.White;
                            dgvGSTR3B6.Rows[0].Cells["colStateUTTax"].Style.BackColor = Color.White;
                        }
                        */
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

        public void Save()
        {
            try
            {
                #region CHECK SGST & CGST VALIDATION
                /*
                bool isValid = true;
                decimal cGST = 0;
                decimal sGST = 0;

                if (dgvGSTR3B6.Rows[0].Cells["colCentarlTax"].Value != null && Convert.ToString(dgvGSTR3B6.Rows[0].Cells["colCentarlTax"].Value).Trim() != "")
                {
                    cGST = Convert.ToDecimal(dgvGSTR3B6.Rows[0].Cells["colCentarlTax"].Value);
                }
                if (dgvGSTR3B6.Rows[0].Cells["colStateUTTax"].Value != null && Convert.ToString(dgvGSTR3B6.Rows[0].Cells["colStateUTTax"].Value).Trim() != "")
                {
                    sGST = Convert.ToDecimal(dgvGSTR3B6.Rows[0].Cells["colStateUTTax"].Value);
                }

                if (cGST != sGST)
                {
                    dgvGSTR3B6.Rows[0].Cells["colCentarlTax"].Style.BackColor = Color.Red;
                    dgvGSTR3B6.Rows[0].Cells["colStateUTTax"].Style.BackColor = Color.Red;

                    isValid = false;
                }
                else
                {
                    dgvGSTR3B6.Rows[0].Cells["colCentarlTax"].Style.BackColor = Color.White;
                    dgvGSTR3B6.Rows[0].Cells["colStateUTTax"].Style.BackColor = Color.White;
                }

                if (isValid == false)
                {
                    MessageBox.Show("CGST and SGST is mismatch!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                */
                #endregion

                #region ADD DATATABLE COLUMN

                // CREATE DATATABLE TO STORE MAIN GRID DATA
                DataTable dt = new DataTable();

                // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                foreach (DataGridViewColumn col in dgvGSTR3B6.Columns)
                {
                    dt.Columns.Add(col.Name.ToString());
                }

                // ADD DATATABLE COLUMN TO STORE FILE STATUS
                dt.Columns.Add("Fld_FileStatus");
                dt.Columns.Add("Fld_FinancialYear");
                dt.Columns.Add("Fld_Month");

                #endregion

                #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                // CREATE OBJECT ARRAY TO STORE ONE ROW DATA OF MAIN GRID
                object[] rowValue = new object[dt.Columns.Count];

                foreach (DataGridViewRow dr in dgvGSTR3B6.Rows)
                {
                    for (int i = 0; i < dr.Cells.Count; i++)
                    {
                        rowValue[i] = Convert.ToString(dr.Cells[i].Value);
                    }

                    // ASSIGN FILE STATUS VALUE WITH EACH GRID ROW
                    rowValue[dr.Cells.Count] = Convert.ToString(CommonHelper.StatusText);

                    // ADD ARRAY OF GRID ROW VALUE TO DATATABLE AS ROW
                    dt.Rows.Add(rowValue);
                }

                dt.AcceptChanges();
                #endregion

                #region CHECK MAIN GRID DATA
                bool fl = false;
                for (int i = 0; i < dgvGSTR3B6.Rows.Count; i++)
                {
                    for (int j = 2; j < dgvGSTR3B6.Columns.Count; j++)
                    {
                        if (dgvGSTR3B6.Rows[i].Cells[j].Value != null)
                        {
                            if (dgvGSTR3B6.Rows[i].Cells[j].Value.ToString().Trim() != "")
                            {
                                fl = true;
                                break;
                            }
                        }
                    }
                }
                #endregion

                #region RECORD SAVE
                string Query = "";
                int _Result = 0;

                // CHECK THERE ARE RECORDS IN GRID
                if (dt != null && dt.Rows.Count > 0)
                {
                    #region FIRST DELETE OLD DATA FROM DATABASE
                    Query = "Delete from SPQR3BTaxPayment where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                    _Result = objGSTR3.IUDData(Query);
                    if (_Result != 1)
                    {
                        // ERROR OCCURS WHILE DELETING DATA
                        MessageBox.Show("System error.\nPlease try after sometime!");
                        return;
                    }
                    #endregion

                    if (fl)
                    {
                        // QUERY FIRE TO SAVE RECORDS TO DATABASE
                        _Result = objGSTR3.GSTR3B6BulkEntry(dt, Convert.ToString(CommonHelper.StatusText));

                        if (_Result == 1)
                        {
                            // TOTAL CALCULATION
                            int[] colNo = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
                            GetTotal(colNo);

                            #region ADD DATATABLE COLUMN

                            // CREATE DATATABLE TO STORE MAIN GRID DATA
                            dt = new DataTable();

                            // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                            foreach (DataGridViewColumn col in dgvGSTR3B6.Columns)
                            {
                                dt.Columns.Add(col.Name.ToString());
                            }

                            // ADD DATATABLE COLUMN TO STORE FILE STATUS
                            dt.Columns.Add("Fld_FileStatus");
                            dt.Columns.Add("Fld_FinancialYear");
                            dt.Columns.Add("Fld_Month");

                            #endregion

                            #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                            // CREATE OBJECT ARRAY TO STORE ONE ROW DATA OF MAIN GRID
                            object[] rowVal = new object[dt.Columns.Count];

                            if (dgvGSTR310Total.Rows.Count == 1)
                            {
                                foreach (DataGridViewRow dr in dgvGSTR310Total.Rows)
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
                            #endregion

                            _Result = objGSTR3.GSTR3B6BulkEntry(dt, "Total");
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
                                MessageBox.Show("System error.\nPlease try after sometime!");
                                return;
                            }
                        }
                        else
                        {
                            // IF ERRORS OCCURS WHILE INSERTING DATA TO DATABASE
                            MessageBox.Show("System error.\nPlease try after sometime!");
                            return;
                        }
                    }
                    else
                    {
                        // IF RECORDS DELETED FROM DATABASE
                        MessageBox.Show("Record Successfully Deleted!");

                        // MAKE FILE STATUS BLANK
                        ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(0);
                    }
                }
                else
                {
                    #region DELETE ALL OLD RECORD IF THERE ARE NO RECORDS PRESENT IN GRID
                    Query = "Delete from SPQR3BTaxPayment where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";

                    // FIRE QUEARY TO DELETE RECORDS
                    _Result = objGSTR3.IUDData(Query);

                    if (_Result == 1)
                    {
                        // IF RECORDS DELETED FROM DATABASE
                        MessageBox.Show("Record Successfully Deleted!");

                        // MAKE FILE STATUS BLANK
                        ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(0);
                    }
                    else
                    {
                        // IF ERRORS OCCURS WHILE DELETING RECORD FROM THE DATABASE
                        MessageBox.Show("System error.\nPlease try after sometime!");
                        return;
                    }
                    #endregion
                }
                #endregion
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

        public void Delete()
        {
            try
            {
                DialogResult result = MessageBox.Show("Do you want to delete selected data?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // IF USER CONFIRM FOR DELETING RECORDS
                if (result == DialogResult.Yes)
                {
                    #region first delete old data from database
                    string Query = "Delete from SPQR3BTaxPayment where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                    int _Result = objGSTR3.IUDData(Query);
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

                        // TOTAL CALCULATION
                        int[] colNo = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
                        GetTotal(colNo);
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

        #region EXCEL TRANSACTIONS

        public void ImportExcel()
        {
            try
            {
                string filePath = string.Empty; string fileExt = string.Empty;

                //open dialog to choose file
                OpenFileDialog file = new OpenFileDialog();
                if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // get file name and extention of selected file
                    filePath = file.FileName;
                    fileExt = Path.GetExtension(filePath);

                    // check selected file extention
                    if (fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0)
                    {
                        #region if impoted file is open then close open file
                        foreach (Process proc in System.Diagnostics.Process.GetProcessesByName("EXCEL"))
                        {
                            if (proc.MainWindowTitle == "Microsoft Excel - " + file.SafeFileName)
                                proc.Kill();
                        }
                        #endregion

                        // create datatable to store impoted file data
                        DataTable dtExcel = new DataTable();
                        dtExcel = ReadExcel(filePath, fileExt);

                        // create datatable to store main grid data
                        DataTable dt = new DataTable();
                        dt = (DataTable)dgvGSTR3B6.DataSource;

                        // check imported template
                        if (dtExcel.Columns.Count != 1)
                        {
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                // open dialog for the confirmation
                                DialogResult result = MessageBox.Show("Do you want to replace existing data?", "Confirmation", MessageBoxButtons.YesNo);

                                // if user confirm for deleting records
                                if (result == DialogResult.Yes)
                                {
                                    if (dtExcel != null && dtExcel.Rows.Count > 0)
                                    {
                                        // if there are data in imported excel file
                                        #region rename datatanle column name as par main grid

                                        // set row size and row header visible property of main grid
                                        dgvGSTR3B6.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                                        dgvGSTR3B6.RowHeadersVisible = false;

                                        // assign datatale to grid
                                        dgvGSTR3B6.DataSource = dtExcel;
                                        #endregion
                                    }
                                    else
                                    {
                                        // if there are no records in imported excel file
                                        MessageBox.Show("There are no records found in imported excel ...!!!!");
                                    }
                                }
                            }
                            else
                            {
                                // if there are no records in main grid
                                if (dtExcel != null && dtExcel.Rows.Count > 0)
                                {
                                    // if there are data in imported excel file
                                    #region rename datatanle column name as par main grid

                                    // set row size and row header visible property of main grid
                                    dgvGSTR3B6.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                                    dgvGSTR3B6.RowHeadersVisible = false;

                                    // assign datatale to grid
                                    dgvGSTR3B6.DataSource = dtExcel;
                                    #endregion
                                }
                                else
                                {
                                    // if there are no records in imported excel file
                                    MessageBox.Show("There are no records found in imported excel ...!!!!");
                                }
                            }

                            // set description column in grid
                            BindData();

                            // TOTAL CALCULATION
                            int[] colNo = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
                            GetTotal(colNo);
                        }
                        else
                        {
                            MessageBox.Show("Please import valid excel template...!!!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please choose .xls or .xlsx file only.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error); //custom messageBox to show error  
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

        public DataTable ReadExcel(string fileName, string fileExt)
        {
            string conn = string.Empty;
            DataTable dtexcel = new DataTable();

            #region connection string
            if (fileExt.CompareTo(".xls") == 0)
                conn = @"provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //for below excel 2007  
            else
                conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 12.0 Xml;HDR=Yes;IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text'"; //for above excel 2007  
            #endregion

            using (OleDbConnection con = new OleDbConnection(conn))
            {
                try
                {
                    try
                    {
                        // read data from sheet1 and save into dtatable
                        OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [B2B_310$]", con);
                        oleAdpt.Fill(dtexcel); //fill excel data into dataTable
                    }
                    catch
                    {
                        // call when imported template sheet name is differ from predefine template
                        DataTable dt = new DataTable();
                        dt.Columns.Add("colError");
                        return dt;
                    }

                    if (dtexcel != null && dtexcel.Rows.Count > 0)
                    {
                        #region validate template
                        for (int i = 0; i < dgvGSTR3B6.Columns.Count; i++)
                        {
                            Boolean flg = false;
                            for (int j = 0; j < dtexcel.Columns.Count; j++)
                            {
                                // check grid column is present or not in imported excel
                                if (dgvGSTR3B6.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim().Length >= 40)
                                {
                                    string piece = dgvGSTR3B6.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim().Substring(0, 40);
                                    string piece1 = string.Empty;

                                    if (dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim().Length >= 40)
                                        piece1 = dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim().Substring(0, 40);
                                    else
                                        piece1 = dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim();

                                    if (piece == piece1)
                                    {
                                        // if grid column present in excel then its index as par grid column index
                                        flg = true;
                                        dtexcel.Columns[j].SetOrdinal(dgvGSTR3B6.Columns[i].Index);
                                        break;
                                    }
                                }
                                else if (dgvGSTR3B6.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim() == dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim())
                                {
                                    // if grid column present in excel then its index as par grid column index
                                    flg = true;
                                    dtexcel.Columns[j].SetOrdinal(dgvGSTR3B6.Columns[i].Index);
                                    break;
                                }
                            }
                            if (flg == false)
                            {
                                // if grid column not present in excel then return datatable with error
                                DataTable dt = new DataTable();
                                dt.Columns.Add("colError");
                                return dt;
                            }
                            dtexcel.AcceptChanges();
                        }
                        #endregion

                        #region Remove unused rows from excel
                        if (dtexcel.Rows.Count > 4)
                        {
                            for (int i = dtexcel.Rows.Count; i > 4; i--)
                            {
                                dtexcel.Rows[i - 1].Delete();
                            }
                        }
                        dtexcel.AcceptChanges();
                        #endregion

                        #region Remove unused column from excel
                        if (dtexcel.Columns.Count > dgvGSTR3B6.Columns.Count)
                        {
                            for (int i = dtexcel.Columns.Count; i > (dgvGSTR3B6.Columns.Count); i--)
                            {
                                dtexcel.Columns.Remove(dtexcel.Columns[i - 1]);
                            }
                        }
                        dtexcel.AcceptChanges();
                        #endregion

                        #region rename column name as par grid column name
                        foreach (DataGridViewColumn col in dgvGSTR3B6.Columns)
                        {
                            dtexcel.Columns[col.Index].ColumnName = col.Name.ToString();
                            col.DataPropertyName = col.Name;
                        }
                        #endregion
                    }
                    dtexcel.AcceptChanges();
                }
                catch (Exception ex)
                {
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
                #region CHECK MAIN GRID DATA
                bool fl = false;
                for (int i = 0; i < dgvGSTR3B6.Rows.Count; i++)
                {
                    for (int j = 2; j < dgvGSTR3B6.Columns.Count; j++)
                    {
                        if (dgvGSTR3B6.Rows[i].Cells[j].Value != null)
                        {
                            if (dgvGSTR3B6.Rows[i].Cells[j].Value.ToString().Trim() != "")
                            {
                                fl = true;
                                break;
                            }
                        }
                    }
                }
                #endregion

                if (fl)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID                    

                    #region CREATE WORKBOOK AND ASSIGN COLUMNNAME
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook WB = excelApp.Workbooks.Add(Missing.Value);

                    Excel.Worksheet newWS = (Excel.Worksheet)excelApp.Worksheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    newWS.Name = "B2B_310";

                    // DELETE UNUSED WORKSHEETS FROM WORKBOOK
                    foreach (Excel.Worksheet ws in WB.Worksheets)
                    {
                        if (ws.Name != "B2B_310")
                            ((Excel.Worksheet)WB.Worksheets[ws.Name]).Delete();
                    }

                    // ASSIGN COLUMN HEADER AS PAR THE GRID HEADER
                    for (int i = 0; i < dgvGSTR3B6.Columns.Count; i++)
                    {
                        newWS.Cells[1, i + 1] = dgvGSTR3B6.Columns[i].HeaderText.ToString();

                        //// SET COLUMN WIDTH
                        //if (i == 1)
                        //    ((Excel.Range)newWS.Cells[1, i]).ColumnWidth = 7;
                        //else if (i >= 2 && i <= 14)
                        //    ((Excel.Range)newWS.Cells[1, i]).ColumnWidth = 10;
                        //else
                        ((Excel.Range)newWS.Cells[1, i + 1]).ColumnWidth = 20;
                    }

                    // GET RANGE AND SET DIFFRENT PROPERTIES
                    Excel.Range headerRange = (Excel.Range)newWS.get_Range((Excel.Range)newWS.Cells[1, 1], (Excel.Range)newWS.Cells[1, dgvGSTR3B6.Columns.Count]);
                    headerRange.WrapText = true;
                    headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Calibri";
                    #endregion

                    #region COPY DATA FROM DATATABLE TO ARRAY

                    // CREATE ARRAY TO HOLD THE DATA OF DATATABLE
                    object[,] arr = new object[dgvGSTR3B6.Rows.Count, dgvGSTR3B6.Columns.Count];

                    // ASSIGN DATA TO ARRAY FROM DATATABLE
                    for (int i = 0; i < dgvGSTR3B6.Rows.Count; i++)
                    {
                        for (int j = 0; j < dgvGSTR3B6.Columns.Count; j++)
                        {
                            arr[i, j] = dgvGSTR3B6.Rows[i].Cells[j].Value.ToString();
                        }
                    }

                    //SET EXCEL RANGE TO PASTE THE DATA
                    Excel.Range top = (Excel.Range)newWS.Cells[2, 1];
                    Excel.Range bottom = (Excel.Range)newWS.Cells[dgvGSTR3B6.Rows.Count + 1, dgvGSTR3B6.Columns.Count];
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

                //open dialog to choose file
                OpenFileDialog file = new OpenFileDialog();

                if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // get file name and extention of selected file
                    filePath = file.FileName;
                    fileExt = Path.GetExtension(filePath);

                    // chck extention of selected file
                    if (fileExt.CompareTo(".csv") == 0 || fileExt.CompareTo(".~csv") == 0)
                    {
                        // create datatble and get impoted csv file data
                        DataTable dtCsv = new DataTable();
                        dtCsv = GetDataTabletFromCSVFile(filePath);

                        /// create datatable and save grid data
                        DataTable dt = new DataTable();
                        dt = (DataTable)dgvGSTR3B6.DataSource;

                        // check imported template
                        if (dtCsv.Columns.Count != 1)
                        {
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                // open dialog for the confirmation
                                DialogResult result = MessageBox.Show("Do you want to replace existing data?", "Confirmation", MessageBoxButtons.YesNo);

                                // if user confirm for deleting records
                                if (result == DialogResult.Yes)
                                {
                                    if (dtCsv != null && dtCsv.Rows.Count > 0)
                                    {
                                        // if there are record present in import file

                                        #region Assign datatable to datagrid

                                        // set row size and row header visible property of main grid
                                        dgvGSTR3B6.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                                        dgvGSTR3B6.RowHeadersVisible = false;

                                        // assign datatable to grid
                                        dgvGSTR3B6.DataSource = dtCsv;
                                        #endregion
                                    }
                                    else
                                    {
                                        // if there are no records in import file
                                        MessageBox.Show("There are no records in CSV file...!!!");
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                // if there are no records in main grid

                                if (dtCsv != null && dtCsv.Rows.Count > 0)
                                {
                                    // if there are record present in import file

                                    #region Assign datatable to datagrid
                                    // set row size and row header visible property of main grid
                                    dgvGSTR3B6.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                                    dgvGSTR3B6.RowHeadersVisible = false;

                                    // assign datatable to grid
                                    dgvGSTR3B6.DataSource = dtCsv;
                                    #endregion
                                }
                                else
                                {
                                    // if there are no records in import file
                                    MessageBox.Show("There are no records in CSV file...!!!");
                                    return;
                                }
                            }

                            // set description column value
                            BindData();

                            // TOTAL CALCULATION
                            int[] colNo = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
                            GetTotal(colNo);
                        }
                        else
                        {
                            MessageBox.Show("Please import valid csv template...!!!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please choose .csv or .~csv file only.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error); //custom messageBox to show error  
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

        private DataTable GetDataTabletFromCSVFile(string csv_file_path)
        {
            //create datatable to store csv data
            DataTable csvData = new DataTable();

            // read data from impoted csv file
            using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
            {
                try
                {
                    // specifi seprater for csv file
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();

                    #region Add Datatable column
                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }
                    #endregion

                    #region Add row data
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();

                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                                fieldData[i] = null;
                        }
                        csvData.Rows.Add(fieldData);
                    }
                    #endregion

                    #region validate template
                    for (int i = 0; i < dgvGSTR3B6.Columns.Count; i++)
                    {
                        Boolean flg = false;
                        for (int j = 0; j < csvData.Columns.Count; j++)
                        {
                            // check grid column is present or not in imported excel
                            if (dgvGSTR3B6.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim().Length >= 40)
                            {
                                string piece = dgvGSTR3B6.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim().Substring(0, 40);
                                string piece1 = string.Empty;

                                if (csvData.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim().Length >= 40)
                                    piece1 = csvData.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim().Substring(0, 40);
                                else
                                    piece1 = csvData.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim();

                                if (piece == piece1)
                                {
                                    // if grid column present in excel then its index as par grid column index
                                    flg = true;
                                    csvData.Columns[j].SetOrdinal(dgvGSTR3B6.Columns[i].Index);
                                    break;
                                }
                            }
                            else if (dgvGSTR3B6.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim() == csvData.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim())
                            {
                                // if grid column present in excel then its index as par grid column index
                                flg = true;
                                csvData.Columns[j].SetOrdinal(dgvGSTR3B6.Columns[i].Index);
                                break;
                            }
                        }
                        if (flg == false)
                        {
                            // if grid column not present in excel then return datatable with error
                            DataTable dt = new DataTable();
                            dt.Columns.Add("colError");
                            return dt;
                        }
                        csvData.AcceptChanges();
                    }
                    #endregion

                    #region Remove unused column from csv datatable
                    if (csvData.Columns.Count >= dgvGSTR3B6.Columns.Count)
                    {
                        for (int i = csvData.Columns.Count; i > (dgvGSTR3B6.Columns.Count); i--)
                        {
                            csvData.Columns.Remove(csvData.Columns[i - 1]);
                        }
                    }
                    csvData.AcceptChanges();
                    #endregion

                    #region Remove unused rows from csv datatable
                    if (csvData.Rows.Count > 4)
                    {
                        for (int i = csvData.Rows.Count; i > 4; i--)
                        {
                            csvData.Rows[i - 1].Delete();
                        }
                    }
                    csvData.AcceptChanges();
                    #endregion

                    #region rename column name as par grid column name
                    foreach (DataGridViewColumn col in dgvGSTR3B6.Columns)
                    {
                        csvData.Columns[col.Index].ColumnName = col.Name.ToString();
                        col.DataPropertyName = col.Name;
                    }
                    #endregion
                }
                catch (Exception ex)
                {
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
                #region CHECK MAIN GRID DATA
                bool fl = false;
                for (int i = 0; i < dgvGSTR3B6.Rows.Count; i++)
                {
                    for (int j = 2; j < dgvGSTR3B6.Columns.Count; j++)
                    {
                        if (dgvGSTR3B6.Rows[i].Cells[j].Value != null)
                        {
                            if (dgvGSTR3B6.Rows[i].Cells[j].Value.ToString().Trim() != "")
                            {
                                fl = true;
                                break;
                            }
                        }
                    }
                }
                #endregion

                if (fl)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID

                    string csv = string.Empty;
                    // CREATE DATATABLE AND GET GRID DATA
                    DataTable dt = new DataTable();
                    dt = (DataTable)dgvGSTR3B6.DataSource;

                    #region ASSIGN COLUMN NAME TO CSV STRING
                    for (int i = 0; i < dgvGSTR3B6.Columns.Count; i++)
                    {
                        csv += dgvGSTR3B6.Columns[i].HeaderText + ',';
                    }

                    //ADD NEW LINE.
                    csv += "\r\n";
                    #endregion

                    #region ASSIGN DREID ROW TO CSV STRING
                    StringBuilder sb = new StringBuilder();
                    sb.Append(csv);

                    // SEPRATE EACH RECORD AND APPEND AS SEPRATED STRING
                    int sj = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        var fields = row.ItemArray.Select(field => "\"" + field.ToString().Replace("\"", "\"\"") + "\"").ToArray();
                        sb.AppendLine(string.Join(",", fields));
                        sj++;
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
                #region CHECK MAIN GRID DATA
                bool fl = false;
                for (int i = 0; i < dgvGSTR3B6.Rows.Count; i++)
                {
                    for (int j = 2; j < dgvGSTR3B6.Columns.Count; j++)
                    {
                        if (dgvGSTR3B6.Rows[i].Cells[j].Value != null)
                        {
                            if (dgvGSTR3B6.Rows[i].Cells[j].Value.ToString().Trim() != "")
                            {
                                fl = true;
                                break;
                            }
                        }
                    }
                }
                #endregion

                if (fl)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID                    

                    #region CREATING ITEXTSHARP TABLE FROM THE DATATABLE DATA AND ASSIGNING TABLE HEADER
                    PdfPTable pdfTable = new PdfPTable(dgvGSTR3B6.ColumnCount);
                    pdfTable.DefaultCell.Padding = 3;
                    pdfTable.WidthPercentage = 100;
                    pdfTable.DefaultCell.BorderWidth = 0;
                    iTextSharp.text.Font fontHeader = iTextSharp.text.FontFactory.GetFont("Calibri", 6);

                    // ADD HEADER TO PDF TABLE
                    string headerName = "10. ITC received during the month";
                    pdfTable = AssignHeader(pdfTable, headerName);
                    #endregion

                    #region ADDING HEADER ROW
                    foreach (DataGridViewColumn column in dgvGSTR3B6.Columns)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, fontHeader));
                        cell.VerticalAlignment = Element.ALIGN_CENTER;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        pdfTable.AddCell(cell);
                    }
                    pdfTable.CompleteRow();
                    Application.DoEvents();
                    #endregion

                    #region ADDING DATAROW TO PDF TABLE

                    int sj = 0;
                    // FOR LICENCE ALLOWS TO EXPORT ALL RECORDS
                    foreach (DataGridViewRow row in dgvGSTR3B6.Rows)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            if (cell.Value != null) // && i != 1)
                            {
                                //CREATE PDF CELL TO GRID RECORDS
                                PdfPCell cell1 = new PdfPCell(new Phrase(cell.Value.ToString(), fontHeader));
                                pdfTable.AddCell(cell1);
                            }
                        }
                        sj++;

                        // COMPLETE PDF-TABLE ROW
                        pdfTable.CompleteRow();
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
                ce1.Colspan = dgvGSTR3B6.Columns.Count;
                ce1.VerticalAlignment = Element.ALIGN_CENTER;
                ce1.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(ce1);

                PdfPCell ce2 = new PdfPCell(new Phrase(" "));
                ce2.Colspan = dgvGSTR3B6.Columns.Count;
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

        #region Json Creator
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
            public IntrLtfee intr_ltfee { get; set; }
        }
        public void JSONCreator()
        {
            RootObject ObjJson = new RootObject();
            List<DataGridViewRow> supInterInvoiceList = dgvGSTR3B6.Rows
                      .OfType<DataGridViewRow>()
                      .ToList();
            IntrLtfee interLtFeeInvoiceList = new IntrLtfee();
            IntrDetails objoIntrDet = new IntrDetails();
            objoIntrDet.iamt = Convert.ToInt32(supInterInvoiceList[0].Cells["colIntegratedTax"].Value.ToString());
            objoIntrDet.camt = Convert.ToInt32(supInterInvoiceList[1].Cells["colCentarlTax"].Value.ToString());
            objoIntrDet.samt = Convert.ToInt32(supInterInvoiceList[2].Cells["colStateUTTax"].Value.ToString());
            objoIntrDet.csamt = Convert.ToInt32(supInterInvoiceList[3].Cells["colCess"].Value.ToString());
            interLtFeeInvoiceList.intr_details = objoIntrDet;
            ObjJson.intr_ltfee = interLtFeeInvoiceList;

            #region File Save
            JavaScriptSerializer objScript = new JavaScriptSerializer();

            var settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.DefaultValueHandling = DefaultValueHandling.Ignore;

            objScript.MaxJsonLength = 2147483647;

            string FinalJson = JsonConvert.SerializeObject(ObjJson, settings);

            SaveFileDialog save = new SaveFileDialog();
            save.FileName = "3B6.json";
            save.Filter = "Json File | *.json";
            if (save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(save.OpenFile());
                writer.WriteLine(FinalJson);
                writer.Dispose();
                writer.Close();
            }
        }
            #endregion

        #endregion

        public void SetGridViewColor()
        {
            try
            {
                this.dgvGSTR3B6.AllowUserToAddRows = false;
                this.dgvGSTR3B6.AllowUserToDeleteRows = false;

                dgvGSTR3B6.EnableHeadersVisualStyles = false;
                dgvGSTR3B6.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(23, 196, 187);
                dgvGSTR3B6.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

                this.dgvGSTR3B6.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                this.dgvGSTR3B6.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                this.dgvGSTR3B6.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

                dgvGSTR3B6.Columns[0].ReadOnly = true;
                dgvGSTR3B6.Columns[0].DefaultCellStyle.BackColor = Color.FromArgb(23, 196, 187);

                foreach (DataGridViewColumn column in dgvGSTR3B6.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
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

        private void dgvGSTR310Total_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                this.dgvGSTR3B6.ClearSelection();
                this.dgvGSTR310Total.ClearSelection();

                if (dgvGSTR310Total.Rows.Count > 0)
                {
                    DataGridViewRow row = this.dgvGSTR310Total.RowTemplate;
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

        private void dgvGSTR310_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvGSTR3B6.Rows[1].Cells["Fld_SGST"].ReadOnly = true;
            dgvGSTR3B6.Rows[1].Cells["Fld_SGST"].Style.BackColor = Color.Gray;

            dgvGSTR3B6.Rows[2].Cells["Fld_CGST"].ReadOnly = true;
            dgvGSTR3B6.Rows[2].Cells["Fld_CGST"].Style.BackColor = Color.Gray;

            dgvGSTR3B6.Rows[3].Cells["Fld_IGST"].ReadOnly = true;
            dgvGSTR3B6.Rows[3].Cells["Fld_IGST"].Style.BackColor = Color.Gray;

            dgvGSTR3B6.Rows[3].Cells["Fld_CGST"].ReadOnly = true;
            dgvGSTR3B6.Rows[3].Cells["Fld_CGST"].Style.BackColor = Color.Gray;

            dgvGSTR3B6.Rows[3].Cells["Fld_SGST"].ReadOnly = true;
            dgvGSTR3B6.Rows[3].Cells["Fld_SGST"].Style.BackColor = Color.Gray;

            dgvGSTR3B6.Rows[2].Cells["Fld_SGST"].ReadOnly = false;
            dgvGSTR3B6.Rows[2].Cells["Fld_SGST"].Style.BackColor = Color.White;

            dgvGSTR3B6.Rows[0].Cells["Fld_CESS"].ReadOnly = true;
            dgvGSTR3B6.Rows[0].Cells["Fld_CESS"].Style.BackColor = Color.Gray;
            dgvGSTR3B6.Rows[1].Cells["Fld_CESS"].ReadOnly = true;
            dgvGSTR3B6.Rows[1].Cells["Fld_CESS"].Style.BackColor = Color.Gray;
            dgvGSTR3B6.Rows[2].Cells["Fld_CESS"].ReadOnly = true;
            dgvGSTR3B6.Rows[2].Cells["Fld_CESS"].Style.BackColor = Color.Gray;


            dgvGSTR3B6.Rows[0].Cells["Fld_LateFeePay"].ReadOnly = true;
            dgvGSTR3B6.Rows[0].Cells["Fld_LateFeePay"].Style.BackColor = Color.Gray;
            dgvGSTR3B6.Rows[3].Cells["Fld_LateFeePay"].ReadOnly = true;
            dgvGSTR3B6.Rows[3].Cells["Fld_LateFeePay"].Style.BackColor = Color.Gray;

            dgvGSTR3B6.Rows[0].Cells["Fld_LateFeePayCash"].ReadOnly = true;
            dgvGSTR3B6.Rows[0].Cells["Fld_LateFeePayCash"].Style.BackColor = Color.Gray;
            dgvGSTR3B6.Rows[3].Cells["Fld_LateFeePayCash"].ReadOnly = true;
            dgvGSTR3B6.Rows[3].Cells["Fld_LateFeePayCash"].Style.BackColor = Color.Gray;

            this.dgvGSTR3B6.ClearSelection();
        }

        private void frmGSTR310_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName("");
        }

        #region SCROLL GRID
        private void dgvGSTR3B6_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                // SET TOTAL GRID OFFSET AS PAR MAIN GRID SCROLL
                this.dgvGSTR310Total.HorizontalScrollingOffset = this.dgvGSTR3B6.HorizontalScrollingOffset;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dgvGSTR310Total_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                // SET TOTAL GRID OFFSET AS PAR MAIN GRID SCROLL
                this.dgvGSTR3B6.HorizontalScrollingOffset = this.dgvGSTR310Total.HorizontalScrollingOffset;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}
