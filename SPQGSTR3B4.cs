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
using iTextSharp.text;
using iTextSharp.text.pdf;
using SPEQTAGST.BAL;
using SPEQTAGST.BAL.M796r3b;


namespace SPEQTAGST.rintlcs3b
{
    public partial class SPQGSTR3B4 : Form
    {
        r3bPublicclass objGSTR3B = new r3bPublicclass();

        public SPQGSTR3B4()
        {
            InitializeComponent();
            GetData();
            BindData();

            // total calculation
            string[] colNo = { "colIGST", "colCGST", "colSGST", "colCESS" };
            GetTotal(colNo);
            GetTotalNew();

            SetGridViewColor(); 
            ((SPQMDI)Application.OpenForms["SPQMDI"]).ShowExtraToolsMenu();
            ((SPQMDI)Application.OpenForms["SPQMDI"]).ShowCompanyDetailMenu();
            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName(CommonHelper.ReturnName);

            dgvGSTR3B4.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dgvGSTR3B4.EnableHeadersVisualStyles = false;
            dgvGSTR3B4.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);
            dgvGSTR3B4.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvGSTR3B4.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            TotaldgvGSTR13.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void GetData()
        {
            try
            {
                // create datatable to store database data
                DataTable dt = new DataTable();
                string Query = "Select * from SPQR3BEligibleITC where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
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
                    foreach (DataGridViewColumn col in dgvGSTR3B4.Columns)
                    {
                        dt.Columns[col.Index].ColumnName = col.Name.ToString();
                        col.DataPropertyName = col.Name;
                    }
                    dt.AcceptChanges();
                    dgvGSTR3B4.DataSource = dt;

                    #endregion
                }
                else
                {
                    dgvGSTR3B4.DataSource = null;                    
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
                if (dgvGSTR3B4.Rows.Count <= 1)
                {
                    DataTable dt = new DataTable();

                    // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                    foreach (DataGridViewColumn col in dgvGSTR3B4.Columns)
                    {
                        dt.Columns.Add(col.Name.ToString());
                        col.DataPropertyName = col.Name;
                    }
                    dt.AcceptChanges();


                    DataRow dr = dt.NewRow();
                    dr["colSrNo"] = "1";
                    dr["colDetails"] = "(A) ITC Available (Whether in full or part)";
                    dr["colIGST"] = "";
                    dr["colCGST"] = "";
                    dr["colSGST"] = "";
                    dr["colCESS"] = "";
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr["colSrNo"] = "2";
                    dr["colDetails"] = "   (1) Import of goods";
                    dr["colIGST"] = "";
                    dr["colCGST"] = "";
                    dr["colSGST"] = "";
                    dr["colCESS"] = "";
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr["colSrNo"] = "3";
                    dr["colDetails"] = "   (2) Import of Services";
                    dr["colIGST"] = "";
                    dr["colCGST"] = "";
                    dr["colSGST"] = "";
                    dr["colCESS"] = "";
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr["colSrNo"] = "4";
                    dr["colDetails"] = "   (3) Inward Supplies liable to reverse charge(other than 1 & 2 above)";
                    dr["colIGST"] = "";
                    dr["colCGST"] = "";
                    dr["colSGST"] = "";
                    dr["colCESS"] = "";
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr["colSrNo"] = "5";
                    dr["colDetails"] = "   (4) Inward supplies from ISD";
                    dr["colIGST"] = "";
                    dr["colCGST"] = "";
                    dr["colSGST"] = "";
                    dr["colCESS"] = "";
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr["colSrNo"] = "6";
                    dr["colDetails"] = "   (5) All other ITC";
                    dr["colIGST"] = "";
                    dr["colCGST"] = "";
                    dr["colSGST"] = "";
                    dr["colCESS"] = "";
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr["colSrNo"] = "7";
                    dr["colDetails"] = "Total ITC Available (A)";
                    dr["colIGST"] = "0";
                    dr["colCGST"] = "0";
                    dr["colSGST"] = "0";
                    dr["colCESS"] = "0";
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr["colSrNo"] = "8";
                    dr["colDetails"] = "(B) ITC Reversed";
                    dr["colIGST"] = "";
                    dr["colCGST"] = "";
                    dr["colSGST"] = "";
                    dr["colCESS"] = "";
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr["colSrNo"] = "9";
                    dr["colDetails"] = "   (1) As per rules 42 & 43 IGST Rules";
                    dr["colIGST"] = "";
                    dr["colCGST"] = "";
                    dr["colSGST"] = "";
                    dr["colCESS"] = "";
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr["colSrNo"] = "10";
                    dr["colDetails"] = "   (2) Others";
                    dr["colIGST"] = "";
                    dr["colCGST"] = "";
                    dr["colSGST"] = "";
                    dr["colCESS"] = "";
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr["colSrNo"] = "11";
                    dr["colDetails"] = "Total ITC Reversed (B)";
                    dr["colIGST"] = "0";
                    dr["colCGST"] = "0";
                    dr["colSGST"] = "0";
                    dr["colCESS"] = "0";
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr["colSrNo"] = "12";
                    dr["colDetails"] = "(C) Net ITC Available (A) – (B)";
                    dr["colIGST"] = "0";
                    dr["colCGST"] = "0";
                    dr["colSGST"] = "0";
                    dr["colCESS"] = "0";
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr["colSrNo"] = "13";
                    dr["colDetails"] = "(D) Ineligible ITC";
                    dr["colIGST"] = "";
                    dr["colCGST"] = "";
                    dr["colSGST"] = "";
                    dr["colCESS"] = "";
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr["colSrNo"] = "14";
                    dr["colDetails"] = "   (1) As per section 17(5)";
                    dr["colIGST"] = "";
                    dr["colCGST"] = "";
                    dr["colSGST"] = "";
                    dr["colCESS"] = "";
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr["colSrNo"] = "15";
                    dr["colDetails"] = "   (2) Others";
                    dr["colIGST"] = "";
                    dr["colCGST"] = "";
                    dr["colSGST"] = "";
                    dr["colCESS"] = "";
                    dt.Rows.Add(dr);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i != 0 && i != 7 && i != 12)
                        {
                            for (int j = 3; j < dt.Columns.Count; j++)
                            {
                                if (dt.Rows[i][j].ToString() == "-" || dt.Rows[i][j].ToString() == "" || dt.Rows[i][j] == null)
                                {
                                    dt.Rows[i][j] = "";
                                }
                            }
                        }
                    }

                    dgvGSTR3B4.DataSource = dt;

                    dgvGSTR3B4.Columns["colDetails"].ReadOnly = true;
                    DataGridViewRow row = this.dgvGSTR3B4.RowTemplate;
                    row.MinimumHeight = 30;
                }
                else
                {
                    dgvGSTR3B4.Rows[0].Cells[1].Value = "(A) ITC Available (Whether in full or part)";
                    dgvGSTR3B4.Rows[1].Cells[1].Value = "   (1) Import of goods";
                    dgvGSTR3B4.Rows[2].Cells[1].Value = "   (2) Import of Services";
                    dgvGSTR3B4.Rows[3].Cells[1].Value = "   (3) Inward Supplies liable to reverse charge(other than 1 & 2 above)";
                    dgvGSTR3B4.Rows[4].Cells[1].Value = "   (4) Inward supplies from ISD";
                    dgvGSTR3B4.Rows[5].Cells[1].Value = "   (5) All other ITC";
                    dgvGSTR3B4.Rows[6].Cells[1].Value = "Total ITC Available (A)";
                    dgvGSTR3B4.Rows[7].Cells[1].Value = "(B) ITC Reversed";
                    dgvGSTR3B4.Rows[8].Cells[1].Value = "   (1) As per rules 42 & 43 IGST Rules";
                    dgvGSTR3B4.Rows[9].Cells[1].Value = "   (2) Others";
                    dgvGSTR3B4.Rows[10].Cells[1].Value = "Total ITC Reversed (B)";
                    dgvGSTR3B4.Rows[11].Cells[1].Value = "(C) Net ITC Available (A) – (B)";
                    dgvGSTR3B4.Rows[12].Cells[1].Value = "(D) Ineligible ITC";
                    dgvGSTR3B4.Rows[13].Cells[1].Value = "   (1) As per section 17(5)";
                    dgvGSTR3B4.Rows[14].Cells[1].Value = "   (2) Others";
                    DataGridViewRow row = this.dgvGSTR3B4.RowTemplate;
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

        public void GetTotal(string[] colNo)
        {

            try
            {
                if (dgvGSTR3B4.Rows.Count == 13)
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
                        dr["colTIGST"] = dgvGSTR3B4.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colIGST"].Value != null).Sum(x => x.Cells["colIGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIGST"].Value)).ToString();
                        dr["colTCGST"] = dgvGSTR3B4.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCGST"].Value != null).Sum(x => x.Cells["colCGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCGST"].Value)).ToString();
                        dr["colTSGST"] = dgvGSTR3B4.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colSGST"].Value != null).Sum(x => x.Cells["colSGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colSGST"].Value)).ToString();
                        dr["colTCESS"] = dgvGSTR3B4.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCESS"].Value != null).Sum(x => x.Cells["colCESS"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCESS"].Value)).ToString();

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
                            if (item == "colIGST")
                                TotaldgvGSTR13.Rows[0].Cells["colTIGST"].Value = dgvGSTR3B4.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colIGST"].Value != null).Sum(x => x.Cells["colIGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIGST"].Value)).ToString();
                            else if (item == "colCGST")
                                TotaldgvGSTR13.Rows[0].Cells["colTCGST"].Value = dgvGSTR3B4.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCGST"].Value != null).Sum(x => x.Cells["colCGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCGST"].Value)).ToString();
                            else if (item == "colSGST")
                                TotaldgvGSTR13.Rows[0].Cells["colTSGST"].Value = dgvGSTR3B4.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colSGST"].Value != null).Sum(x => x.Cells["colSGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colSGST"].Value)).ToString();
                            else if (item == "colCESS")
                                TotaldgvGSTR13.Rows[0].Cells["colTCESS"].Value = dgvGSTR3B4.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCESS"].Value != null).Sum(x => x.Cells["colCESS"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCESS"].Value)).ToString();
                        }
                        #endregion
                    }
                    TotaldgvGSTR13.Rows[0].Cells[1].Value = "TOTAL";
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

        public void GetTotalNew()
        {
            try
            {
                decimal IGSTTotalA = 0;

                if (IGSTTotalA == 0)
                {
                    if (dgvGSTR3B4.Rows[1].Cells["colIGST"].Value != "")
                        IGSTTotalA = IGSTTotalA + Convert.ToDecimal(dgvGSTR3B4.Rows[1].Cells["colIGST"].Value);
                    if (dgvGSTR3B4.Rows[2].Cells["colIGST"].Value != "")
                        IGSTTotalA = IGSTTotalA + Convert.ToDecimal(dgvGSTR3B4.Rows[2].Cells["colIGST"].Value);
                    if (dgvGSTR3B4.Rows[3].Cells["colIGST"].Value != "")
                        IGSTTotalA = IGSTTotalA + Convert.ToDecimal(dgvGSTR3B4.Rows[3].Cells["colIGST"].Value);
                    if (dgvGSTR3B4.Rows[4].Cells["colIGST"].Value != "")
                        IGSTTotalA = IGSTTotalA + Convert.ToDecimal(dgvGSTR3B4.Rows[4].Cells["colIGST"].Value);
                    if (dgvGSTR3B4.Rows[5].Cells["colIGST"].Value != "")
                        IGSTTotalA = IGSTTotalA + Convert.ToDecimal(dgvGSTR3B4.Rows[5].Cells["colIGST"].Value);

                    if (IGSTTotalA > 0)
                    {
                        dgvGSTR3B4.Rows[6].Cells["colIGST"].Value = Convert.ToString(IGSTTotalA);
                    }
                    else
                    {
                        dgvGSTR3B4.Rows[6].Cells["colIGST"].Value = "0";
                    }
                }

                decimal CGSTTotalA = 0;
                if (CGSTTotalA == 0)
                {
                    if (dgvGSTR3B4.Rows[3].Cells["colCGST"].Value != "")
                        CGSTTotalA = CGSTTotalA + Convert.ToDecimal(dgvGSTR3B4.Rows[3].Cells["colCGST"].Value);
                    if (dgvGSTR3B4.Rows[4].Cells["colCGST"].Value != "")
                        CGSTTotalA = CGSTTotalA + Convert.ToDecimal(dgvGSTR3B4.Rows[4].Cells["colCGST"].Value);
                    if (dgvGSTR3B4.Rows[5].Cells["colCGST"].Value != "")
                        CGSTTotalA = CGSTTotalA + Convert.ToDecimal(dgvGSTR3B4.Rows[5].Cells["colCGST"].Value);

                    if (CGSTTotalA > 0)
                    {
                        dgvGSTR3B4.Rows[6].Cells["colCGST"].Value = Convert.ToString(CGSTTotalA);
                    }
                    else
                    {
                        dgvGSTR3B4.Rows[6].Cells["colCGST"].Value = "0";
                    }
                }

                decimal SGSTTotalA = 0;
                if (SGSTTotalA == 0)
                {
                    if (dgvGSTR3B4.Rows[3].Cells["colSGST"].Value != "")
                        SGSTTotalA = SGSTTotalA + Convert.ToDecimal(dgvGSTR3B4.Rows[3].Cells["colSGST"].Value);
                    if (dgvGSTR3B4.Rows[4].Cells["colSGST"].Value != "")
                        SGSTTotalA = SGSTTotalA + Convert.ToDecimal(dgvGSTR3B4.Rows[4].Cells["colSGST"].Value);
                    if (dgvGSTR3B4.Rows[5].Cells["colSGST"].Value != "")
                        SGSTTotalA = SGSTTotalA + Convert.ToDecimal(dgvGSTR3B4.Rows[5].Cells["colSGST"].Value);

                    if (SGSTTotalA > 0)
                    {
                        dgvGSTR3B4.Rows[6].Cells["colSGST"].Value = Convert.ToString(SGSTTotalA);
                    }
                    else
                    {
                        dgvGSTR3B4.Rows[6].Cells["colSGST"].Value = "0";
                    }
                }
                decimal CESSTotalA = 0;

                if (dgvGSTR3B4.Rows[1].Cells["colCESS"].Value != "")
                    CESSTotalA = CESSTotalA + Convert.ToDecimal(dgvGSTR3B4.Rows[1].Cells["colCESS"].Value);
                if (dgvGSTR3B4.Rows[2].Cells["colCESS"].Value != "")
                    CESSTotalA = CESSTotalA + Convert.ToDecimal(dgvGSTR3B4.Rows[2].Cells["colCESS"].Value);
                if (dgvGSTR3B4.Rows[3].Cells["colCESS"].Value != "")
                    CESSTotalA = CESSTotalA + Convert.ToDecimal(dgvGSTR3B4.Rows[3].Cells["colCESS"].Value);
                if (dgvGSTR3B4.Rows[4].Cells["colCESS"].Value != "")
                    CESSTotalA = CESSTotalA + Convert.ToDecimal(dgvGSTR3B4.Rows[4].Cells["colCESS"].Value);
                if (dgvGSTR3B4.Rows[5].Cells["colCESS"].Value != "")
                    CESSTotalA = CESSTotalA + Convert.ToDecimal(dgvGSTR3B4.Rows[5].Cells["colCESS"].Value);

                if (CESSTotalA > 0)
                {
                    dgvGSTR3B4.Rows[6].Cells["colCESS"].Value = Convert.ToString(CESSTotalA);
                }
                else
                {
                    dgvGSTR3B4.Rows[6].Cells["colCESS"].Value = "0";
                }
                //Part B
                decimal IGSTTotalB = 0;

                if (dgvGSTR3B4.Rows[8].Cells["colIGST"].Value != "")
                    IGSTTotalB = IGSTTotalB + Convert.ToDecimal(dgvGSTR3B4.Rows[8].Cells["colIGST"].Value);
                if (dgvGSTR3B4.Rows[9].Cells["colIGST"].Value != "")
                    IGSTTotalB = IGSTTotalB + Convert.ToDecimal(dgvGSTR3B4.Rows[9].Cells["colIGST"].Value);

                if (IGSTTotalB > 0)
                {
                    dgvGSTR3B4.Rows[10].Cells["colIGST"].Value = Convert.ToString(IGSTTotalB);
                }
                else
                {
                    dgvGSTR3B4.Rows[10].Cells["colIGST"].Value = "0";
                }

                decimal CGSTTotalB = 0;

                if (dgvGSTR3B4.Rows[8].Cells["colCGST"].Value != "")
                    CGSTTotalB = CGSTTotalB + Convert.ToDecimal(dgvGSTR3B4.Rows[8].Cells["colCGST"].Value);
                if (dgvGSTR3B4.Rows[9].Cells["colCGST"].Value != "")
                    CGSTTotalB = CGSTTotalB + Convert.ToDecimal(dgvGSTR3B4.Rows[9].Cells["colCGST"].Value);

                if (CGSTTotalB > 0)
                {
                    dgvGSTR3B4.Rows[10].Cells["colCGST"].Value = Convert.ToString(CGSTTotalB);
                }
                else
                {
                    dgvGSTR3B4.Rows[10].Cells["colCGST"].Value = "0";
                }
                decimal SGSTTotalB = 0;

                if (dgvGSTR3B4.Rows[8].Cells["colSGST"].Value != "")
                    SGSTTotalB = SGSTTotalB + Convert.ToDecimal(dgvGSTR3B4.Rows[8].Cells["colSGST"].Value);
                if (dgvGSTR3B4.Rows[9].Cells["colSGST"].Value != "")
                    SGSTTotalB = SGSTTotalB + Convert.ToDecimal(dgvGSTR3B4.Rows[9].Cells["colSGST"].Value);

                if (SGSTTotalB > 0)
                {
                    dgvGSTR3B4.Rows[10].Cells["colSGST"].Value = Convert.ToString(SGSTTotalB);
                }
                else
                {
                    dgvGSTR3B4.Rows[10].Cells["colSGST"].Value = "0";
                }
                decimal CESSTotalB = 0;

                if (dgvGSTR3B4.Rows[8].Cells["colCESS"].Value != "")
                    CESSTotalB = CESSTotalB + Convert.ToDecimal(dgvGSTR3B4.Rows[8].Cells["colCESS"].Value);
                if (dgvGSTR3B4.Rows[9].Cells["colCESS"].Value != "")
                    CESSTotalB = CESSTotalB + Convert.ToDecimal(dgvGSTR3B4.Rows[9].Cells["colCESS"].Value);

                if (CESSTotalB > 0)
                {
                    dgvGSTR3B4.Rows[10].Cells["colCESS"].Value = Convert.ToString(CESSTotalB);
                }
                else
                {
                    dgvGSTR3B4.Rows[10].Cells["colCESS"].Value = "0";
                }


                #region Part A -B
                decimal IGSTTotalAB = 0;
                if (dgvGSTR3B4.Rows[6].Cells["colIGST"].Value.ToString() != "")
                    IGSTTotalAB = IGSTTotalAB + Convert.ToDecimal(dgvGSTR3B4.Rows[6].Cells["colIGST"].Value);
                if (dgvGSTR3B4.Rows[10].Cells["colIGST"].Value.ToString() != "")
                    IGSTTotalAB = IGSTTotalAB - Convert.ToDecimal(dgvGSTR3B4.Rows[10].Cells["colIGST"].Value);

                dgvGSTR3B4.Rows[11].Cells["colIGST"].Value = Convert.ToString(IGSTTotalAB);
                //decimal IGSTTotalAB = (Convert.ToDecimal(dgvGSTR3B4.Rows[6].Cells["colIGST"].Value) - Convert.ToDecimal(dgvGSTR3B4.Rows[10].Cells["colIGST"].Value));
                //if (IGSTTotalAB > 0)
                //{
                //    dgvGSTR3B4.Rows[11].Cells["colIGST"].Value = Convert.ToString(IGSTTotalAB);
                //}
                //else
                //{
                //    dgvGSTR3B4.Rows[11].Cells["colIGST"].Value = "0";
                //}


                decimal CGSTTotalAB = 0;
                if (dgvGSTR3B4.Rows[6].Cells["colCGST"].Value.ToString() != "")
                    CGSTTotalAB = CGSTTotalAB + Convert.ToDecimal(dgvGSTR3B4.Rows[6].Cells["colCGST"].Value);
                if (dgvGSTR3B4.Rows[10].Cells["colCGST"].Value.ToString() != "")
                    CGSTTotalAB = CGSTTotalAB - Convert.ToDecimal(dgvGSTR3B4.Rows[10].Cells["colCGST"].Value);

                dgvGSTR3B4.Rows[11].Cells["colCGST"].Value = Convert.ToString(CGSTTotalAB);
                //decimal CGSTTotalAB = (Convert.ToDecimal(dgvGSTR3B4.Rows[6].Cells["colCGST"].Value) - Convert.ToDecimal(dgvGSTR3B4.Rows[10].Cells["colCGST"].Value));
                //if (CGSTTotalAB > 0)
                //{
                //    dgvGSTR3B4.Rows[11].Cells["colCGST"].Value = Convert.ToString(CGSTTotalAB);
                //}
                //else
                //{
                //    dgvGSTR3B4.Rows[11].Cells["colCGST"].Value = "0";
                //}

                decimal SGSTTotalAB = 0;
                if (dgvGSTR3B4.Rows[6].Cells["colSGST"].Value.ToString() != "")
                    SGSTTotalAB = SGSTTotalAB + Convert.ToDecimal(dgvGSTR3B4.Rows[6].Cells["colSGST"].Value);
                if (dgvGSTR3B4.Rows[10].Cells["colSGST"].Value.ToString() != "")
                    SGSTTotalAB = SGSTTotalAB - Convert.ToDecimal(dgvGSTR3B4.Rows[10].Cells["colSGST"].Value);

                dgvGSTR3B4.Rows[11].Cells["colSGST"].Value = Convert.ToString(SGSTTotalAB);
                //decimal SGSTTotalAB = (Convert.ToDecimal(dgvGSTR3B4.Rows[6].Cells["colSGST"].Value) - Convert.ToDecimal(dgvGSTR3B4.Rows[10].Cells["colSGST"].Value));
                //if (SGSTTotalAB > 0)
                //{
                //    dgvGSTR3B4.Rows[11].Cells["colSGST"].Value = Convert.ToString(SGSTTotalAB);
                //}
                //else
                //{
                //    dgvGSTR3B4.Rows[11].Cells["colSGST"].Value = "0";
                //}

                decimal CESSTotalAB = 0;
                if (dgvGSTR3B4.Rows[6].Cells["colCESS"].Value.ToString() != "")
                    CESSTotalAB = CESSTotalAB + Convert.ToDecimal(dgvGSTR3B4.Rows[6].Cells["colCESS"].Value);
                if (dgvGSTR3B4.Rows[10].Cells["colCESS"].Value.ToString() != "")
                    CESSTotalAB = CESSTotalAB - Convert.ToDecimal(dgvGSTR3B4.Rows[10].Cells["colCESS"].Value);

                dgvGSTR3B4.Rows[11].Cells["colCESS"].Value = Convert.ToString(CESSTotalAB);
                //decimal CESSTotalAB = (Convert.ToDecimal(dgvGSTR3B4.Rows[6].Cells["colCESS"].Value) - Convert.ToDecimal(dgvGSTR3B4.Rows[10].Cells["colCESS"].Value));
                //if (CESSTotalAB > 0)
                //{
                //    dgvGSTR3B4.Rows[11].Cells["colCESS"].Value = Convert.ToString(CESSTotalAB);
                //}
                //else
                //{
                //    dgvGSTR3B4.Rows[11].Cells["colCESS"].Value = "0";
                //}
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

        public void SetGridViewColor()
        {
            try
            {
                // set main grid property
                this.dgvGSTR3B4.AllowUserToAddRows = false;
                this.dgvGSTR3B4.AutoGenerateColumns = false;

                dgvGSTR3B4.EnableHeadersVisualStyles = false;
                dgvGSTR3B4.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(23, 196, 187);
                dgvGSTR3B4.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

                this.dgvGSTR3B4.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                this.dgvGSTR3B4.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                this.dgvGSTR3B4.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

                dgvGSTR3B4.Columns[0].ReadOnly = true;
                dgvGSTR3B4.Columns[0].DefaultCellStyle.BackColor = Color.FromArgb(23, 196, 187);

                foreach (DataGridViewColumn column in dgvGSTR3B4.Columns)
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
                #region CHECK SGST & CGST VALIDATION
                int[] arrOfIndex = new int[7] { 3, 4, 5, 8, 9, 13, 14 };

                 bool isValid = true;
                 if (dgvGSTR3B4.Rows.Count > 0)
                 {
                     int IndexCount = 0;
                     foreach (DataGridViewRow dr in dgvGSTR3B4.Rows)
                     {
                         if (arrOfIndex.Contains(IndexCount))
                         {
                             decimal cGST = 0;
                             decimal sGST = 0;

                             if (dr.Cells["colCGST"].Value != null && Convert.ToString(dr.Cells["colCGST"].Value).Trim() != "")
                             {
                                 cGST = Convert.ToDecimal(dr.Cells["colCGST"].Value);
                             }
                             if (dr.Cells["colSGST"].Value != null && Convert.ToString(dr.Cells["colSGST"].Value).Trim() != "")
                             {
                                 sGST = Convert.ToDecimal(dr.Cells["colSGST"].Value);
                             }

                             if (cGST != sGST)
                             {
                                 dr.Cells["colCGST"].Style.BackColor = Color.Red;
                                 dr.Cells["colSGST"].Style.BackColor = Color.Red;
                                 isValid = false;
                             }
                             else
                             {
                                 dr.Cells["colCGST"].Style.BackColor = Color.White;
                                 dr.Cells["colSGST"].Style.BackColor = Color.White;
                             }
                         }

                         IndexCount = IndexCount + 1;
                     }
                 }
                 if (isValid == false)
                 {
                     MessageBox.Show("CGST and SGST is mismatch!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                     return;
                 }
                #endregion

                #region ADD DATATABLE COLUMN

                // create datatable to store main grid data
                DataTable dt = new DataTable();

                // add datatble collumn as par main  grid column
                foreach (DataGridViewColumn col in dgvGSTR3B4.Columns)
                {
                    dt.Columns.Add(col.Name.ToString());
                }

                // add datatable column to store file status
                dt.Columns.Add("colFileStatus");

                #endregion

                #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                // create object array to store one row data of main grid
                object[] rowValue = new object[dt.Columns.Count];

                foreach (DataGridViewRow dr in dgvGSTR3B4.Rows)
                {
                    if (dr.Index != dgvGSTR3B4.Rows.Count) // DON'T ADD LAST ROW
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

                    Query = "Delete from SPQR3BEligibleITC where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                    _Result = objGSTR3B.IUDData(Query);
                    if (_Result != 1)
                    {
                        // error occurs while deleting data
                        MessageBox.Show("System error.\nPlease try after sometime!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    #endregion

                    // query fire to save records to database
                    _Result = objGSTR3B.GSTR3B4BulkEntry(dt, Convert.ToString(CommonHelper.StatusText));

                    if (_Result == 1)
                    {

                        string[] colNo = { "colIGST", "colCGST", "colSGST", "colCESS" };
                        GetTotal(colNo);
                        GetTotalNew();

                        #region ADD DATATABLE COLUMN

                        // CREATE DATATABLE TO STORE MAIN GRID DATA
                        dt = new DataTable();

                        // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                        foreach (DataGridViewColumn col in dgvGSTR3B4.Columns)
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

                        _Result = objGSTR3B.GSTR3B4BulkEntry(dt, "Total");

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

                    Query = "Delete from SPQR3BEligibleITC where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";

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
                    string Query = "Delete from SPQR3BEligibleITC where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
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
                        if (dgvGSTR3B4.Rows.Count > 0)
                        {
                            foreach (DataGridViewCell oneCell in dgvGSTR3B4.SelectedCells)
                            {
                                if (oneCell.ColumnIndex != 1 && (oneCell.ColumnIndex != 3 || oneCell.RowIndex != 1) && (oneCell.ColumnIndex != 4 || oneCell.RowIndex != 1) && (oneCell.ColumnIndex != 3 || oneCell.RowIndex != 2) && (oneCell.ColumnIndex != 4 || oneCell.RowIndex != 2))
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
                    string[] colNo = { "colIGST", "colCGST", "colSGST", "colCESS" };
                    GetTotal(colNo);
                    GetTotalNew();
                }
                if (e.KeyCode == Keys.V)
                {
                    #region PAST FROM EXCELL SHEET

                    string s = Clipboard.GetText();
                    string[] lines = s.Split('\n');
                    int iRow = 0, iCol = 0;

                    #region PAST ON SELECTED CELLS ONLY
                    if (dgvGSTR3B4.RowCount > 0) // IF GRID IS NOT NULL AND PAST ONLY ON SELECTED CELLS
                    {
                        foreach (DataGridViewCell oneCell in dgvGSTR3B4.SelectedCells)
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
                            if (iRow < dgvGSTR3B4.RowCount && line.Length > 0 && iRow < 15)
                            {
                                string[] sCells = line.Split('\t');

                                for (int i = 0; i < sCells.GetLength(0); ++i)
                                {
                                    if (iCol + i < this.dgvGSTR3B4.ColumnCount && i < 7)
                                    {
                                        if (iCol == 0)
                                            oCell = dgvGSTR3B4[iCol + i + 2, iRow];
                                        else if (iCol == 1)
                                            oCell = dgvGSTR3B4[iCol + i + 1, iRow];
                                        else
                                            oCell = dgvGSTR3B4[iCol + i, iRow];

                                        sCells[i] = sCells[i].Trim().Replace(",", "");
                                        if (oCell.ColumnIndex != 0)
                                        {
                                            if (!dgvGSTR3B4.Rows[iRow].Cells[oCell.ColumnIndex].ReadOnly)
                                            {
                                                if (dgvGSTR3B4.Columns[oCell.ColumnIndex].Name != "colChk" && dgvGSTR3B4.Columns[oCell.ColumnIndex].Name != "colSequence")
                                                {
                                                    #region VALIDATION
                                                    if (sCells[i].ToString().Trim() == "") { dgvGSTR3B4.Rows[iRow].Cells[oCell.ColumnIndex].Value = DBNull.Value; }
                                                    else
                                                    {
                                                        if (oCell.ColumnIndex >= 1 && oCell.ColumnIndex <= 8)
                                                        {
                                                            dgvGSTR3B4.Rows[iRow].Cells[oCell.ColumnIndex].Value = sCells[i].Trim();
                                                        }
                                                        else { dgvGSTR3B4.Rows[iRow].Cells[oCell.ColumnIndex].Value = sCells[i].Trim(); }
                                                    }
                                                    #endregion
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (iCol > i)
                                            {
                                                for (int j = oCell.ColumnIndex; j < dgvGSTR3B4.Columns.Count; j++)
                                                {
                                                    #region VALIDATION
                                                    if (sCells[i].ToString().Trim() == "") { dgvGSTR3B4.Rows[iRow].Cells[j].Value = DBNull.Value; }
                                                    else
                                                    {
                                                        if (j >= 1 && j <= 8)
                                                        {
                                                            dgvGSTR3B4.Rows[iRow].Cells[j].Value = sCells[i].Trim();
                                                        }
                                                        else { dgvGSTR3B4.Rows[iRow].Cells[j].Value = sCells[i].Trim(); }
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
                                                for (int j = oCell.ColumnIndex; j < dgvGSTR3B4.Columns.Count; j++)
                                                {
                                                    #region VALIDATION
                                                    if (sCells[i].ToString().Trim() == "") { dgvGSTR3B4.Rows[iRow].Cells[j].Value = DBNull.Value; }
                                                    else
                                                    {
                                                        if (j >= 1 && j <= 8)
                                                        {
                                                            dgvGSTR3B4.Rows[iRow].Cells[j].Value = sCells[i].Trim();
                                                        }
                                                        else { dgvGSTR3B4.Rows[iRow].Cells[j].Value = sCells[i].Trim(); }
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

        private void dgvGSTR13_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string cNo = dgvGSTR3B4.Columns[e.ColumnIndex].Name;

                if (e.RowIndex >= 0)
                {
                    if (cNo == "colIGST" || cNo == "colCGST" || cNo == "colSGST" || cNo == "colCESS")
                    {
                        if (!chkCellValue(Convert.ToString(dgvGSTR3B4.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                            dgvGSTR3B4.Rows[e.RowIndex].Cells[cNo].Value = "";

                        if (chkCellValue(Convert.ToString(dgvGSTR3B4.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                        {
                            if (dgvGSTR3B4.Rows[e.RowIndex].Cells[cNo].Value != Utility.DisplayIndianCurrency(Convert.ToString(dgvGSTR3B4.Rows[e.RowIndex].Cells[cNo].Value)))
                            {

                                dgvGSTR3B4.CellValueChanged -= dgvGSTR13_CellValueChanged;
                                dgvGSTR3B4.Rows[e.RowIndex].Cells[cNo].Value = Utility.DisplayIndianCurrency(Convert.ToString(Math.Round(Convert.ToDecimal(dgvGSTR3B4.Rows[e.RowIndex].Cells[cNo].Value), 2, MidpointRounding.AwayFromZero)));
                                dgvGSTR3B4.CellValueChanged += dgvGSTR13_CellValueChanged;

                                string[] colNo = { dgvGSTR3B4.Columns[e.ColumnIndex].Name };
                                GetTotal(colNo);
                                //GetTotalNew();
                            }
                        }
                        else
                        {
                            dgvGSTR3B4.Rows[e.RowIndex].Cells[cNo].Value = "";
                        }

                        
                        #region CHECK SGST & CGST VALIDATION
                        int[] arrOfIndex = new int[5] { 3, 4, 5, 13, 14 };
                        
                        if (arrOfIndex.Contains(e.RowIndex))
                        {
                            GetTotalNew();
                            decimal cGST = 0;
                            decimal sGST = 0;

                            if (dgvGSTR3B4.Rows[e.RowIndex].Cells["colCGST"].Value != null && Convert.ToString(dgvGSTR3B4.Rows[e.RowIndex].Cells["colCGST"].Value).Trim() != "")
                            {
                                cGST = Convert.ToDecimal(dgvGSTR3B4.Rows[e.RowIndex].Cells["colCGST"].Value);
                            }
                            if (dgvGSTR3B4.Rows[e.RowIndex].Cells["colSGST"].Value != null && Convert.ToString(dgvGSTR3B4.Rows[e.RowIndex].Cells["colSGST"].Value).Trim() != "")
                            {
                                sGST = Convert.ToDecimal(dgvGSTR3B4.Rows[e.RowIndex].Cells["colSGST"].Value);
                            }

                            if (cGST != sGST)
                            {
                                dgvGSTR3B4.Rows[e.RowIndex].Cells["colCGST"].Style.BackColor = Color.Red;
                                dgvGSTR3B4.Rows[e.RowIndex].Cells["colSGST"].Style.BackColor = Color.Red;
                            }
                            else
                            {
                                dgvGSTR3B4.Rows[e.RowIndex].Cells["colCGST"].Style.BackColor = Color.White;
                                dgvGSTR3B4.Rows[e.RowIndex].Cells["colSGST"].Style.BackColor = Color.White;
                            }
                        }
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

        private Boolean chkCellValue(string cellValue, string cNo)
        {
            try
            {
                if (cellValue.Trim() != "")// NOT EQUEL BLANK
                {
                    cellValue = cellValue.Replace("-", "");
                    if (cNo == "colIGST" || cNo == "colCGST" || cNo == "colSGST" || cNo == "colCESS")
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
                        //pbGSTR1.Visible = true;

                        #region IF IMPOTED FILE IS OPEN THEN CLOSE OPEN FILE
                        foreach (Process proc in System.Diagnostics.Process.GetProcessesByName("EXCEL"))
                        {
                            if (proc.MainWindowTitle == "Microsoft Excel - " + file.SafeFileName)
                                proc.Kill();
                        }
                        #endregion

                        // CREATE DATATABLE TO STORE MAIN GRID DATA
                        DataTable dt = new DataTable();
                        dt = (DataTable)dgvGSTR3B4.DataSource;

                        // CREATE DATATABLE TO STORE IMPOTED FILE DATA
                        DataTable dtExcel = new DataTable();
                        dtExcel = ReadExcel(filePath, fileExt, dt);

                        // CHECK IMPORTED TEMPLATE
                        if (dtExcel.Columns.Count != 1)
                        {
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                dt.Columns.Remove("colSrNo");
                                dt.Columns.Remove("colDetails");
                                dtExcel.Columns.Remove("colDetails");
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
                                        dgvGSTR3B4.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                                        dgvGSTR3B4.RowHeadersVisible = false;

                                        // assign datatale to grid
                                        dgvGSTR3B4.DataSource = dtExcel;
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
                                    dgvGSTR3B4.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                                    dgvGSTR3B4.RowHeadersVisible = false;

                                    // assign datatale to grid
                                    dgvGSTR3B4.DataSource = dtExcel;
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
                            string[] colNo = { "colIGST", "colCGST", "colSGST", "colCESS" };
                            GetTotal(colNo);
                            GetTotalNew();
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
                EnableControls(dgvGSTR3B4);
                // pbGSTR1.Visible = false;
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
                        OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [3B4$]", con);
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
                        for (int i = 1; i < dgvGSTR3B4.Columns.Count - 1; i++)
                        {
                            flg = false;
                            for (int j = 0; j < dtexcel.Columns.Count; j++)
                            {
                                // CHECK GRID COLUMN IS PRESENT OR NOT IN IMPORTED EXCEL
                                if (dgvGSTR3B4.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim() == dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim())
                                {
                                    // IF GRID COLUMN PRESENT IN EXCEL THEN ITS INDEX AS PAR GRID COLUMN INDEX
                                    flg = true;
                                    dtexcel.Columns[j].SetOrdinal(dgvGSTR3B4.Columns[i].Index - 1);
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
                        if (dtexcel.Columns.Count >= dgvGSTR3B4.Columns.Count - 2)
                        {
                            for (int i = dtexcel.Columns.Count - 1; i > (dgvGSTR3B4.Columns.Count - 2); i--)
                            {
                                dtexcel.Columns.Remove(dtexcel.Columns[i]);
                            }
                        }
                        dtexcel.AcceptChanges();
                        #endregion

                        #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME
                        foreach (DataGridViewColumn col in dgvGSTR3B4.Columns)
                        {
                            if (col.Index != 0)
                                dtexcel.Columns[col.Index - 1].ColumnName = col.Name.ToString();
                        }
                        #endregion

                        dtexcel.AcceptChanges();

                    }
                }
                catch (Exception ex)
                {
                    //pbGSTR1.Visible = false;
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
                if (dgvGSTR3B4.Rows.Count > 1)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID

                    #region CREATE WORKBOOK AND ASSIGN COLUMNNAME
                    Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook WB = excelApp.Workbooks.Add(Missing.Value);

                    Microsoft.Office.Interop.Excel.Worksheet newWS = (Microsoft.Office.Interop.Excel.Worksheet)excelApp.Worksheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    newWS.Name = "3B4";

                    // DELETE UNUSED WORKSHEETS FROM WORKBOOK
                    foreach (Microsoft.Office.Interop.Excel.Worksheet ws in WB.Worksheets)
                    {
                        if (ws.Name != "3B4")
                            ((Microsoft.Office.Interop.Excel.Worksheet)WB.Worksheets[ws.Name]).Delete();
                    }

                    // ASSIGN COLUMN HEADER AS PAR THE GRID HEADER
                    for (int i = 1; i < dgvGSTR3B4.Columns.Count; i++)
                    {
                        newWS.Cells[1, i] = dgvGSTR3B4.Columns[i].HeaderText.ToString();

                        // SET COLUMN WIDTH
                        if (i == 1)
                            ((Microsoft.Office.Interop.Excel.Range)newWS.Cells[1, i]).ColumnWidth = 30;
                        else if (i >= 2 && i <= 12)
                            ((Microsoft.Office.Interop.Excel.Range)newWS.Cells[1, i]).ColumnWidth = 10;
                        else
                            ((Microsoft.Office.Interop.Excel.Range)newWS.Cells[1, i]).ColumnWidth = 15;
                    }

                    // GET RANGE AND SET DIFFRENT PROPERTIES
                    Microsoft.Office.Interop.Excel.Range headerRange = (Microsoft.Office.Interop.Excel.Range)newWS.get_Range((Microsoft.Office.Interop.Excel.Range)newWS.Cells[1, 1], (Microsoft.Office.Interop.Excel.Range)newWS.Cells[1, dgvGSTR3B4.Columns.Count - 1]);
                    headerRange.WrapText = true;
                    headerRange.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                    headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Calibri";
                    #endregion

                    #region COPY DATA FROM DATATABLE TO ARRAY

                    // CREATE ARRAY TO HOLD THE DATA OF DATATABLE
                    object[,] arr = new object[dgvGSTR3B4.Rows.Count, dgvGSTR3B4.Columns.Count];

                    // ASSIGN DATA TO ARRAY FROM DATATABLE
                    if (CommonHelper.IsLicence)
                    {
                        // FOR LICENECE ALLOWS TO EXPORT ALL RECORDS
                        for (int i = 0; i < dgvGSTR3B4.Rows.Count; i++)
                        {
                            for (int j = 1; j < dgvGSTR3B4.Columns.Count; j++)
                            {
                                arr[i, j - 1] = Convert.ToString(dgvGSTR3B4.Rows[i].Cells[j].Value);
                            }
                        }
                    }
                    else
                    {
                        // FOR DEMO ALLOW ONLY 100 RECORDS TO EXPORT
                        for (int i = 0; i < dgvGSTR3B4.Rows.Count; i++)
                        {
                            if (i < 100)
                            {
                                for (int j = 1; j < dgvGSTR3B4.Columns.Count; j++)
                                {
                                    arr[i, j - 1] = Convert.ToString(dgvGSTR3B4.Rows[i].Cells[j].Value);
                                }
                            }
                        }
                    }

                    for (int i = 0; i < dgvGSTR3B4.Rows.Count; i++)
                    {
                        if (dgvGSTR3B4.Rows[i].Cells[1].Value.ToString() == "Total ITC Available (A)")
                        {
                            headerRange = (Microsoft.Office.Interop.Excel.Range)newWS.get_Range((Microsoft.Office.Interop.Excel.Range)newWS.Cells[i + 2, 1], (Microsoft.Office.Interop.Excel.Range)newWS.Cells[i + 2, dgvGSTR3B4.Columns.Count]);
                            headerRange.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                            headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                            headerRange.Font.Bold = true;
                        }
                        else if (dgvGSTR3B4.Rows[i].Cells[1].Value.ToString() == "Total ITC Reversed (B)")
                        {
                            headerRange = (Microsoft.Office.Interop.Excel.Range)newWS.get_Range((Microsoft.Office.Interop.Excel.Range)newWS.Cells[i + 2, 1], (Microsoft.Office.Interop.Excel.Range)newWS.Cells[i + 2, dgvGSTR3B4.Columns.Count]);
                            headerRange.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                            headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                            headerRange.Font.Bold = true;
                        }
                    }

                    //SET EXCEL RANGE TO PASTE THE DATA
                    Microsoft.Office.Interop.Excel.Range top = (Microsoft.Office.Interop.Excel.Range)newWS.Cells[2, 1];
                    Microsoft.Office.Interop.Excel.Range bottom = (Microsoft.Office.Interop.Excel.Range)newWS.Cells[dgvGSTR3B4.Rows.Count + 1, dgvGSTR3B4.Columns.Count];
                    Microsoft.Office.Interop.Excel.Range sheetRange = newWS.Range[top, bottom];

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
                //pbGSTR1.Visible = false;
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
                PdfPTable pdfTable = new PdfPTable(dgvGSTR3B4.ColumnCount - 1);
                pdfTable.DefaultCell.Padding = 3;
                pdfTable.WidthPercentage = 100;
                pdfTable.DefaultCell.BorderWidth = 0;
                iTextSharp.text.Font fontHeader = iTextSharp.text.FontFactory.GetFont("Calibri", 6);

                // ADD HEADER TO PDF TABLE
                pdfTable = AssignHeader(pdfTable, "Details of Outward Supplies and inward Supplies liable to Reverse Charge");
                #endregion

                #region ADDING HEADER ROW
                int i = 0;

                #region HEADER1
                PdfPCell celHeader1 = new PdfPCell();

                celHeader1 = new PdfPCell(new Phrase("Details", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Integrated Tax", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Central Tax", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("State/UT Tax", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Cess", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                pdfTable.CompleteRow();
                #endregion

                Application.DoEvents();
                #endregion

                #region ADDING COLUMN NUMBER
                i = 0;
                foreach (DataGridViewColumn column in dgvGSTR3B4.Columns)
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
                    foreach (DataGridViewRow row in dgvGSTR3B4.Rows)
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
                    foreach (DataGridViewRow row in dgvGSTR3B4.Rows)
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
                ce1.Colspan = dgvGSTR3B4.Columns.Count - 1;
                ce1.VerticalAlignment = Element.ALIGN_CENTER;
                ce1.HorizontalAlignment = Element.ALIGN_LEFT;
                ce1.BackgroundColor = new iTextSharp.text.BaseColor(197, 223, 197);
                ce1.BorderWidth = 0;
                pdfTable.AddCell(ce1);

                iTextSharp.text.Font FigToRs = FontFactory.GetFont("Calibri", 10, iTextSharp.text.Font.BOLD);
                PdfPCell ceHeader2 = new PdfPCell(new Phrase("(figures in Rs)", FigToRs));
                ceHeader2.Colspan = dgvGSTR3B4.Columns.Count - 1;
                ceHeader2.VerticalAlignment = Element.ALIGN_CENTER;
                ceHeader2.HorizontalAlignment = Element.ALIGN_RIGHT;
                ceHeader2.BorderWidth = 0;
                pdfTable.AddCell(ceHeader2);

                PdfPCell ce2 = new PdfPCell(new Phrase(" "));
                ce2.Colspan = dgvGSTR3B4.Columns.Count - 1;
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

        #region json
        public class ItcAvl
        {
            public string ty { get; set; }
            public double iamt { get; set; }
            public int camt { get; set; }
            public double samt { get; set; }
            public int csamt { get; set; }
        }

        public class ItcRev
        {
            public string ty { get; set; }
            public double iamt { get; set; }
            public int camt { get; set; }
            public double samt { get; set; }
            public int csamt { get; set; }
        }

        public class ItcNet
        {
            public double iamt { get; set; }
            public int camt { get; set; }
            public double samt { get; set; }
            public int csamt { get; set; }
        }

        public class ItcInelg
        {
            public string ty { get; set; }
            public double iamt { get; set; }
            public int camt { get; set; }
            public double samt { get; set; }
            public int csamt { get; set; }
        }

        public class ItcElg
        {
            public List<ItcAvl> itc_avl { get; set; }
            public List<ItcRev> itc_rev { get; set; }
            public ItcNet itc_net { get; set; }
            public List<ItcInelg> itc_inelg { get; set; }
        }

        public class RootObject
        {
            public ItcElg itc_elg { get; set; }
        }
        #endregion

        public void JSONCreator()
        {
            RootObject ObjJson = new RootObject();

            List<DataGridViewRow> Invoicelist = dgvGSTR3B4.Rows
                      .OfType<DataGridViewRow>()
                      .ToList();
            ItcElg objItcElg = new ItcElg();
            List<ItcAvl> objitc_avl = new List<ItcAvl>();
            List<ItcRev> objitc_rev = new List<ItcRev>();
            List<ItcInelg> obj__invalid_name = new List<ItcInelg>();
            for (int i = 0; i < Invoicelist.Count; i++)
            {
                ItcAvl Itcavvv = new ItcAvl();
                ItcRev ItcRevvv = new ItcRev();
                if (i == 1 || i == 2 || i == 3 || i == 4 || i == 5)
                {
                    if (i == 1)
                        Itcavvv.ty = "IMPG";
                    if (i == 2)
                        Itcavvv.ty = "IMPS";
                    if (i == 3)
                        Itcavvv.ty = "ISRC";
                    if (i == 4)
                        Itcavvv.ty = "ISD";
                    if (i == 5)
                        Itcavvv.ty = "OTH";
                    Itcavvv.iamt = Convert.ToDouble(Invoicelist[i].Cells["colIGST"].Value.ToString());
                    Itcavvv.camt = Convert.ToInt32(Invoicelist[i].Cells["colCGST"].Value.ToString());
                    Itcavvv.samt = Convert.ToDouble(Invoicelist[i].Cells["colSGST"].Value.ToString());
                    Itcavvv.csamt = Convert.ToInt32(Invoicelist[i].Cells["colCESS"].Value.ToString());
                    objitc_avl.Add(Itcavvv);
                    objItcElg.itc_avl = objitc_avl;
                }
                if (i == 7 || i == 8)
                {
                    if (i == 7)
                        ItcRevvv.ty = "RUL";
                    else
                        ItcRevvv.ty = "OTH";
                    ItcRevvv.iamt = Convert.ToDouble(Invoicelist[i].Cells["colIGST"].Value.ToString());
                    ItcRevvv.camt = Convert.ToInt32(Invoicelist[i].Cells["colCGST"].Value.ToString());
                    ItcRevvv.samt = Convert.ToDouble(Invoicelist[i].Cells["colSGST"].Value.ToString());
                    ItcRevvv.csamt = Convert.ToInt32(Invoicelist[i].Cells["colCESS"].Value.ToString());
                    objitc_rev.Add(ItcRevvv);
                    objItcElg.itc_rev = objitc_rev;
                }
                if (i == 9)
                {
                    ItcNet ObjItcNet = new ItcNet();
                    ObjItcNet.iamt = Convert.ToDouble(Invoicelist[i].Cells["colIGST"].Value.ToString());
                    ObjItcNet.camt = Convert.ToInt32(Invoicelist[i].Cells["colCGST"].Value.ToString());
                    ObjItcNet.samt = Convert.ToDouble(Invoicelist[i].Cells["colSGST"].Value.ToString());
                    ObjItcNet.csamt = Convert.ToInt32(Invoicelist[i].Cells["colCESS"].Value.ToString());
                    objItcElg.itc_net = ObjItcNet;
                }
                if (i == 11 || i == 12)
                {
                    ItcInelg ObjItcInelg = new ItcInelg();
                    if (i == 11)
                        ObjItcInelg.ty = "RUL";
                    else
                        ObjItcInelg.ty = "OTH";
                    ObjItcInelg.iamt = Convert.ToDouble(Invoicelist[i].Cells["colIGST"].Value.ToString());
                    ObjItcInelg.camt = Convert.ToInt32(Invoicelist[i].Cells["colCGST"].Value.ToString());
                    ObjItcInelg.samt = Convert.ToDouble(Invoicelist[i].Cells["colSGST"].Value.ToString());
                    ObjItcInelg.csamt = Convert.ToInt32(Invoicelist[i].Cells["colCESS"].Value.ToString());
                    obj__invalid_name.Add(ObjItcInelg);
                    objItcElg.itc_inelg = obj__invalid_name;
                }
                ObjJson.itc_elg = objItcElg;

            }
            #region File Save
            JavaScriptSerializer objScript = new JavaScriptSerializer();
            objScript.MaxJsonLength = 2147483647;
            string FinalJson = objScript.Serialize(ObjJson);
            SaveFileDialog save = new SaveFileDialog();
            save.FileName = "3B4.json";
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

        #region DISABLE/ENABLE CONTROLS

        private void DisableControls(Control con)
        {
            foreach (Control c in con.Controls)
            {
                if (c.Name != "frmGSTR13" && c.Name != "TotaldgvGSTR13")
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

        

        private void frmGSTR3B4_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName("");
        }

        private void dgvGSTR3B4_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            #region
            System.Windows.Forms.DataGridViewCellStyle boldStyle = new System.Windows.Forms.DataGridViewCellStyle();
            boldStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);

            dgvGSTR3B4.Rows[0].ReadOnly = true;
            dgvGSTR3B4.Rows[7].ReadOnly = true;
            dgvGSTR3B4.Rows[12].ReadOnly = true;

            //dgvGSTR3B4.Columns["colDetails"].ReadOnly = true;
            dgvGSTR3B4.Rows[6].ReadOnly = true;
            dgvGSTR3B4.Rows[6].DefaultCellStyle = boldStyle;
            dgvGSTR3B4.Rows[6].DefaultCellStyle.BackColor = Color.LightGray;
            dgvGSTR3B4.Rows[6].Cells["colDetails"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvGSTR3B4.Rows[10].ReadOnly = true;
            dgvGSTR3B4.Rows[10].DefaultCellStyle = boldStyle;
            dgvGSTR3B4.Rows[10].DefaultCellStyle.BackColor = Color.LightGray;
            dgvGSTR3B4.Rows[10].Cells["colDetails"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvGSTR3B4.Rows[11].ReadOnly = true;
            dgvGSTR3B4.Rows[11].DefaultCellStyle = boldStyle;
            dgvGSTR3B4.Rows[11].DefaultCellStyle.BackColor = Color.LightGray;
            dgvGSTR3B4.Rows[11].Cells["colDetails"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvGSTR3B4.Rows[0].DefaultCellStyle.BackColor = Color.FromArgb(23, 196, 187);
            dgvGSTR3B4.Rows[7].DefaultCellStyle.BackColor = Color.FromArgb(23, 196, 187);
            dgvGSTR3B4.Rows[12].DefaultCellStyle.BackColor = Color.FromArgb(23, 196, 187);

            dgvGSTR3B4.Rows[1].Cells["colCGST"].ReadOnly = true;
            dgvGSTR3B4.Rows[1].Cells["colCGST"].Style.BackColor = Color.Gray;
            //dgvGSTR3B4.Rows[1].Cells["colCGST"].Value = "";
            dgvGSTR3B4.Rows[2].Cells["colCGST"].ReadOnly = true;
            dgvGSTR3B4.Rows[2].Cells["colCGST"].Style.BackColor = Color.Gray;
            //dgvGSTR3B4.Rows[2].Cells["colCGST"].Value = "";

            dgvGSTR3B4.Rows[1].Cells["colSGST"].ReadOnly = true;
            dgvGSTR3B4.Rows[1].Cells["colSGST"].Style.BackColor = Color.Gray;
            //dgvGSTR3B4.Rows[1].Cells["colSGST"].Value = "";
            dgvGSTR3B4.Rows[2].Cells["colSGST"].ReadOnly = true;
            dgvGSTR3B4.Rows[2].Cells["colSGST"].Style.BackColor = Color.Gray;
            //dgvGSTR3B4.Rows[2].Cells["colSGST"].Value = "";

            this.dgvGSTR3B4.ClearSelection();
            this.TotaldgvGSTR13.ClearSelection();
            #endregion
        }
    }
}
