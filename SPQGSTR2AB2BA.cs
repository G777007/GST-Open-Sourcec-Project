using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using SPEQTAGST.BAL;
using SPEQTAGST.BAL.M125r2a;
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

namespace SPEQTAGST.cachsR2a
{
    public partial class SPQGSTR2AB2BA : Form
    {
        R2APublicclass objGSTR2A = new R2APublicclass();

        public SPQGSTR2AB2BA()
        {
            InitializeComponent();

            // SET GRID PROPERTY
            SetGridViewColor();

            //Bind Data
            GetData();

            // TOTAL CALCULATION
            string[] colNo = { "colOrgInvoiceNo", "colResInvoiceNo", "colInvoiceValue", "colTaxableVal", "colIntTax", "colCtrlTax", "colStateTax", "colCessTax" };
            GetTotal(colNo);
            BindFilter();
            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName(CommonHelper.ReturnName);

            dgvGSTR2A_B2BA.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dgvGSTR2A_B2BA.EnableHeadersVisualStyles = false;
            dgvGSTR2A_B2BA.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);
            dgvGSTR2A_B2BA.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvGSTR2A_B2BA.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dgvGSTR2A3Total.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        #region Filter

        private void BindFilter()
        {
            try
            {
                List<colList> lstColumns = new List<colList>();
                for (int i = 0; i < dgvGSTR2A_B2BA.Columns.Count; i++)
                {
                    if (i > 1)
                    {
                        string HeaderText = dgvGSTR2A_B2BA.Columns[i].HeaderText;
                        string Name = dgvGSTR2A_B2BA.Columns[i].Name;
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

        private void PrefillData()
        {
            try
            {
                #region JSON DATA STATIC
                string _json = "{ \"b2b\": [ { \"ctin\": \"01AABCE2207R1Z5\", \"cfs\": \"Y\", \"inv\": [ { \"chksum\": \"AflJufPlFStqKBZ\", \"inum\": \"S008400\", \"idt\": \"24-11-2016\", \"val\": 729248.16, \"pos\": \"06\", \"rchrg\": \"N\", \"itms\": [ { \"num\": 1, \"itm_det\": { \"ty\": \"S\", \"hsn_sc\": \"G1221\", \"txval\": 6210.99, \"irt\": 0, \"iamt\": 0, \"crt\": 37.4, \"camt\": 614.44, \"srt\": 33.41, \"samt\": 5.68, \"csrt\": 10, \"csamt\": 621.09 } }, { \"num\": 2, \"itm_det\": { \"ty\": \"S\", \"hsn_sc\": \"G1231\", \"txval\": 1000.05, \"irt\": 0, \"iamt\": 0, \"crt\": 37.45, \"camt\": 887.44, \"srt\": 33.41, \"samt\": 5.68, \"csrt\": 5.12, \"csamt\": 50.12 } } ] } ] } ] }";
                #endregion

                RootObject obj = JsonConvert.DeserializeObject<RootObject>(_json);

                #region ADD DATATABLE COLUMN
                DataTable dt = new DataTable();

                foreach (DataGridViewColumn col in dgvGSTR2A_B2BA.Columns)
                {
                    if (col.Name.ToLower() != "colchk")
                    {
                        dt.Columns.Add(col.Name.ToString());
                    }
                }

                #endregion

                #region ASSIGN GRIDVIEW ROWS IN DATATABLE
                for (int i = 0; i < obj.b2b.Count; i++)
                {
                    for (int j = 0; j < obj.b2b[i].inv.Count; j++)
                    {
                        for (int k = 0; k < obj.b2b[i].inv[j].itms.Count; k++)
                        {
                            dt.Rows.Add();
                            //ROOT START
                            dt.Rows[dt.Rows.Count - 1]["colGSTIN"] = Convert.ToString(obj.b2b[i].ctin);
                            //ROOT END

                            //INVOICE DATA START
                            dt.Rows[dt.Rows.Count - 1]["colInvoiceNo"] = Convert.ToString(obj.b2b[i].inv[j].inum);//INVOICE NO.
                            dt.Rows[dt.Rows.Count - 1]["colInvoiceDate"] = Convert.ToString(obj.b2b[i].inv[j].idt);//INVOICE DATE
                            dt.Rows[dt.Rows.Count - 1]["colPOS"] = Convert.ToString(obj.b2b[i].inv[j].pos);//POS
                            // dt.Rows[dt.Rows.Count - 1]["colIndSupAttac"] = Convert.ToString(obj.b2b[i].inv[j].rchrg);//INDICATE SUPPLY ATTACK
                            dt.Rows[dt.Rows.Count - 1]["colInvoiceValue"] = Convert.ToString(obj.b2b[i].inv[j].val);//SUPPLYER INVOICE VALUE
                            //INVOICE DATA END

                            //ITEM DATA START
                            dt.Rows[dt.Rows.Count - 1]["colInvoiceGoodsServi"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.ty);//GOODS AND SERVICE
                            dt.Rows[dt.Rows.Count - 1]["colInvoiceHSNSAC"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.hsn_sc);//HSN
                            dt.Rows[dt.Rows.Count - 1]["colInvoiceTaxableVal"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.txval);//TAXABLE VALUE
                            dt.Rows[dt.Rows.Count - 1]["colIGSTRate"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.irt);//IGST RATE
                            dt.Rows[dt.Rows.Count - 1]["colIntTax"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.iamt);//IGST AMOUNT
                            dt.Rows[dt.Rows.Count - 1]["colCGSTRate"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.crt);//CGST RATE
                            dt.Rows[dt.Rows.Count - 1]["colCtrlTax"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.camt);//CGST AMOUNT
                            dt.Rows[dt.Rows.Count - 1]["colSGSTRate"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.srt);//SGST RATE
                            dt.Rows[dt.Rows.Count - 1]["colStateTax"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.samt);//SGST AMOUNT
                            dt.Rows[dt.Rows.Count - 1]["colCessRate"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.csrt);//SGST RATE
                            dt.Rows[dt.Rows.Count - 1]["colCessTax"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.csamt);//SGST AMOUNT


                            #region New Parameter
                            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].inv[j].flag);
                            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].inv[j].updby);
                            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].inv[j].od_num);
                            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].inv[j].od_dt);
                            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].inv[j].etin);
                            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].cfs);

                            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.csrt);
                            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.csamt);
                            #endregion

                            //ITEM DATA END
                        }
                    }
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["colSequence"] = Convert.ToString(i + 1);
                }

                dt.AcceptChanges();
                dgvGSTR2A_B2BA.DataSource = dt;
                Application.DoEvents();
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("Prefill Data Error : " + ex.Message);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }

        #endregion

        private void GetData()
        {
            try
            {
                DataTable dt = new DataTable();
                string Query = "Select * from SPQR2AB2BAmend where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";//and Fld_ReverseCharge = 'False'
                Application.DoEvents();
                dt = objGSTR2A.GetDataGSTR2A(Query);

                if (dt != null && dt.Rows.Count > 0)
                {
                    pbGSTR1.Visible = true;
                    // ASSIGN FILE STATUS FILED VALUE
                    if (Convert.ToString(dt.Rows[0]["Fld_FileStatus"]).ToLower() == "draft")
                        ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(1);
                    else if (Convert.ToString(dt.Rows[0]["Fld_FileStatus"]).ToLower() == "completed")
                        ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(2);
                    else if (Convert.ToString(dt.Rows[0]["Fld_FileStatus"]).ToLower() == "not-completed")
                        ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(3);

                    // REMOVE LAST COLUMN (MONTH)
                    dt.Columns.Remove(dt.Columns["Fld_Month"]);
                    // REMOVE LAST COLUMN (FILE STATUS)
                    dt.Columns.Remove(dt.Columns["Fld_FileStatus"]);
                    // REMOVE FIRST COLUMN (FIELD ID)
                    dt.Columns.Remove(dt.Columns[0]);
                    // ADD COLUMN (CHEK BOX)
                    dt.Columns.Add(new DataColumn("colChk"));
                    // SET CHECK BOX COLUMN AT FIRST INDEX OF DATATABLE

                    dt.Columns["colChk"].SetOrdinal(0);
                    //dt.Columns["Fld_InvoiceType"].SetOrdinal(4);
                    // RENAME DATATABLE COLUMN NAME TO DATAGRIDVIEW COLUMN NAME
                    foreach (DataGridViewColumn col in dgvGSTR2A_B2BA.Columns)
                    {
                        dt.Columns[col.Index].ColumnName = col.Name.ToString();
                        col.DataPropertyName = col.Name;
                    }

                    dt.AcceptChanges();

                    // ASSIGN DATATABLE TO DATA GRID VIEW
                    dgvGSTR2A_B2BA.DataSource = dt;
                    Application.DoEvents();
                    pbGSTR1.Visible = false;
                }
                else
                {
                    ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(0);
                }
            }
            catch (Exception ex)
            {
                pbGSTR1.Visible = false;
                MessageBox.Show(ex.Message);
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
                if (dgvGSTR2A_B2BA.Rows.Count > 0)
                {
                    // IF MAIN GRID HAVING RECORDS

                    if (dgvGSTR2A3Total.Rows.Count == 0)
                    {
                        #region IF TOTAL GRID HAVING NO RECORD
                        // CREATE TEMPORARY DATATABLE TO STORE COLUMN CALCULATION
                        DataTable dtTotal = new DataTable();

                        // ADD COLUMN AS PAR DATAGRIDVIEW COLUMN
                        foreach (DataGridViewColumn col in dgvGSTR2A3Total.Columns)
                        {
                            dtTotal.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;
                        }

                        // CREATE DATAROW TO STORE GRID COLUMN CALCULATION
                        DataRow dr = dtTotal.NewRow();

                        #region Invoice no
                        DataTable dt = new DataTable();
                        #region ADD DATATABLE COLUMN

                        // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                        foreach (DataGridViewColumn col in dgvGSTR2A_B2BA.Columns)
                        {
                            dt.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;
                        }
                        #endregion
                        #region ASSIGN GRIDVIEW ROWS IN DATATABLE
                        object[] rowValue = new object[dt.Columns.Count];

                        foreach (DataGridViewRow drn in dgvGSTR2A_B2BA.Rows)
                        {
                            rowValue[0] = "False";
                            for (int i = 1; i < drn.Cells.Count; i++)
                            {
                                rowValue[i] = Convert.ToString(drn.Cells[i].Value);
                            }
                            dt.Rows.Add(rowValue);
                        }
                        dt.AcceptChanges();
                        #endregion

                        var result = (from row in dt.AsEnumerable()
                                      where row.Field<string>("colOrgInvoiceNo") != "" && row.Field<string>("colGSTIN") != ""
                                      group row by new { colGSTIN = row.Field<string>("colGSTIN"), colInvNo = row.Field<string>("colOrgInvoiceNo") } into grp
                                      select new
                                      {
                                          colGSTIN = grp.Key.colGSTIN,
                                          colInvNo = grp.Key.colInvNo,
                                      }).ToList();

                        if (result != null && result.Count > 0)
                            dr["colTOrgInvoiceNo"] = result.Count;
                        else
                            dr["colTOrgInvoiceNo"] = 0;

                        var result1 = (from row in dt.AsEnumerable()
                                       where row.Field<string>("colResInvoiceNo") != "" && row.Field<string>("colGSTIN") != ""
                                       group row by new { colGSTIN = row.Field<string>("colGSTIN"), colInvNo = row.Field<string>("colResInvoiceNo") } into grp
                                      select new
                                      {
                                          colGSTIN = grp.Key.colGSTIN,
                                          colInvNo = grp.Key.colInvNo,
                                      }).ToList();

                        if (result1 != null && result1.Count > 0)
                            dr["colTResInvoiceNo"] = result1.Count;
                        else
                            dr["colTResInvoiceNo"] = 0;
                        #endregion

                        //dr["colTOrgInvoiceNo"] = dgvGSTR2A_B2BA.Rows.Cast<DataGridViewRow>().Where(x => Convert.ToString(x.Cells["colOrgInvoiceNo"].Value).Trim() != "").GroupBy(x => x.Cells["colOrgInvoiceNo"].Value).Select(x => x.First()).Distinct().Count();
                        //dr["colTResInvoiceNo"] = dgvGSTR2A_B2BA.Rows.Cast<DataGridViewRow>().Where(x => Convert.ToString(x.Cells["colResInvoiceNo"].Value).Trim() != "").GroupBy(x => x.Cells["colResInvoiceNo"].Value).Select(x => x.First()).Distinct().Count();                        
                        dr["colTInvoiceValue"] = dgvGSTR2A_B2BA.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colInvoiceValue"].Value != null).Sum(x => x.Cells["colInvoiceValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colInvoiceValue"].Value)).ToString();
                        dr["colTTaxableVal"] = dgvGSTR2A_B2BA.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colTaxableVal"].Value != null).Sum(x => x.Cells["colTaxableVal"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colTaxableVal"].Value)).ToString();
                        dr["colTIntTax"] = dgvGSTR2A_B2BA.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colIntTax"].Value != null).Sum(x => x.Cells["colIntTax"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIntTax"].Value)).ToString();
                        dr["colTCtrlTax"] = dgvGSTR2A_B2BA.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCtrlTax"].Value != null).Sum(x => x.Cells["colCtrlTax"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCtrlTax"].Value)).ToString();
                        dr["colTStateTax"] = dgvGSTR2A_B2BA.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colStateTax"].Value != null).Sum(x => x.Cells["colStateTax"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colStateTax"].Value)).ToString();
                        dr["colTCessTax"] = dgvGSTR2A_B2BA.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCessTax"].Value != null).Sum(x => x.Cells["colCessTax"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCessTax"].Value)).ToString();

                        // ADD DATAROW TO DATATABLE
                        dtTotal.Rows.Add(dr);
                        dtTotal.AcceptChanges();

                        // ASSIGN DATATABLE TO GRID
                        dgvGSTR2A3Total.DataSource = dtTotal;

                        #endregion
                    }
                    else if (dgvGSTR2A3Total.Rows.Count == 1)
                    {
                        #region IF TOTAL GRID HAVING ONLY ONE RECORDS

                        // CALCULATE TOTAL ONLY SPECIFIC COLUMN
                        foreach (var item in colNo)
                        {
                            if (item == "colOrgInvoiceNo")
                            {
                                #region Invoice no
                                DataTable dt = new DataTable();
                                #region ADD DATATABLE COLUMN

                                // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                                foreach (DataGridViewColumn col in dgvGSTR2A_B2BA.Columns)
                                {
                                    dt.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }
                                #endregion

                                #region ASSIGN GRIDVIEW ROWS IN DATATABLE
                                object[] rowValue = new object[dt.Columns.Count];

                                foreach (DataGridViewRow drn in dgvGSTR2A_B2BA.Rows)
                                {
                                    rowValue[0] = "False";
                                    for (int i = 1; i < drn.Cells.Count; i++)
                                    {
                                        rowValue[i] = Convert.ToString(drn.Cells[i].Value);
                                    }
                                    dt.Rows.Add(rowValue);
                                }
                                dt.AcceptChanges();
                                #endregion

                                #region
                                var result = (from row in dt.AsEnumerable()
                                              where row.Field<string>("colOrgInvoiceNo") != "" && row.Field<string>("colGSTIN") != ""
                                              group row by new { colGSTIN = row.Field<string>("colGSTIN"), colInvNo = row.Field<string>("colOrgInvoiceNo") } into grp
                                              select new
                                              {
                                                  colGSTIN = grp.Key.colGSTIN,
                                                  colInvNo = grp.Key.colInvNo,
                                              }).ToList();

                                if (result != null && result.Count > 0)
                                    dgvGSTR2A3Total.Rows[0].Cells["colTOrgInvoiceNo"].Value = result.Count;
                                else
                                    dgvGSTR2A3Total.Rows[0].Cells["colTOrgInvoiceNo"].Value = 0;
                                
                                var result1 = (from row in dt.AsEnumerable()
                                              where row.Field<string>("colResInvoiceNo") != "" && row.Field<string>("colGSTIN") != ""
                                              group row by new { colGSTIN = row.Field<string>("colGSTIN"), colInvNo = row.Field<string>("colResInvoiceNo") } into grp
                                              select new
                                              {
                                                  colGSTIN = grp.Key.colGSTIN,
                                                  colInvNo = grp.Key.colInvNo,
                                              }).ToList();

                                if (result1 != null && result1.Count > 0)
                                    dgvGSTR2A3Total.Rows[0].Cells["colTResInvoiceNo"].Value = result1.Count;
                                else
                                    dgvGSTR2A3Total.Rows[0].Cells["colTResInvoiceNo"].Value = 0;
                                
                                #endregion

                                #endregion
                            }

                            dgvGSTR2A3Total.Rows[0].Cells["colTInvoiceValue"].Value = dgvGSTR2A_B2BA.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colInvoiceValue"].Value != null).Sum(x => x.Cells["colInvoiceValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colInvoiceValue"].Value)).ToString();
                            dgvGSTR2A3Total.Rows[0].Cells["colTTaxableVal"].Value = dgvGSTR2A_B2BA.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colTaxableVal"].Value != null).Sum(x => x.Cells["colTaxableVal"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colTaxableVal"].Value)).ToString();

                            dgvGSTR2A3Total.Rows[0].Cells["colTIntTax"].Value = dgvGSTR2A_B2BA.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colIntTax"].Value != null).Sum(x => x.Cells["colIntTax"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIntTax"].Value)).ToString();

                            dgvGSTR2A3Total.Rows[0].Cells["colTCtrlTax"].Value = dgvGSTR2A_B2BA.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCtrlTax"].Value != null).Sum(x => x.Cells["colCtrlTax"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCtrlTax"].Value)).ToString();

                            dgvGSTR2A3Total.Rows[0].Cells["colTStateTax"].Value = dgvGSTR2A_B2BA.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colStateTax"].Value != null).Sum(x => x.Cells["colStateTax"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colStateTax"].Value)).ToString();

                            dgvGSTR2A3Total.Rows[0].Cells["colTCessTax"].Value = dgvGSTR2A_B2BA.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCessTax"].Value != null).Sum(x => x.Cells["colCessTax"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCessTax"].Value)).ToString();

                            dgvGSTR2A3Total.Rows[0].Cells["colTPOS"].Value = "Completed";
                            dgvGSTR2A3Total.Rows[0].Cells["colTSupAttResCharge"].Value = "False";
                            dgvGSTR2A3Total.Rows[0].Cells[0].Value = "Total";

                        }
                        #endregion
                    }

                    // SET GRID ROW HEIGHT AND ASSIGN TOTAL HEADER
                    dgvGSTR2A3Total.Rows[0].Cells[0].Value = "TOTAL";
                }
                else
                {
                    // CHECK IF TOTAL GRID HAVING RECORD

                    if (dgvGSTR2A3Total.Rows.Count >= 0)
                    {
                        #region IF THERE ARE NO RECORDS IN MAIN GRID THEN ASSIGN BLANK DATATABLE TO TOTAL GRID
                        DataTable dtTotal = new DataTable();
                        foreach (DataGridViewColumn col in dgvGSTR2A3Total.Columns)
                        {
                            dtTotal.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;
                        }
                        dgvGSTR2A3Total.DataSource = dtTotal;
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

        private void dgvGSTR24A_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete)
                {
                    pbGSTR1.Visible = true;

                    #region DELETE SELECTED CELLS
                    try
                    {
                        // CHECK PRESENT RECORDS IN MAIN GRID
                        if (dgvGSTR2A_B2BA.Rows.Count > 0)
                        {
                            // DELETE SELECTED CELL IN GRID
                            foreach (DataGridViewCell oneCell in dgvGSTR2A_B2BA.SelectedCells)
                            {
                                // CHECK BOX COLUMN (0,17) DATA DO NOT DELETE
                                if (oneCell.Selected && oneCell.ColumnIndex != 0 && oneCell.ColumnIndex != 19 && !oneCell.ReadOnly)
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
                        MessageBox.Show("Error : " + ex.Message);
                        return;
                    }
                    #endregion

                    string[] colNo = { "colOrgInvoiceNo", "colResInvoiceNo", "colInvoiceValue", "colTaxableVal", "colIntTax", "colCtrlTax", "colStateTax", "colCessTax" };
                    GetTotal(colNo);
                    pbGSTR1.Visible = false;
                }
                if (e.KeyCode == Keys.V)
                {
                    pbGSTR1.Visible = true;

                    #region PAST FROM EXCELL SHEET

                    // GET COPIED DATA TO STRING
                    string s = Clipboard.GetText();
                    string[] lines = s.Split('\n');
                    int iRow = 0, iCol = 0;

                    #region PAST ON SELECTED CELLS ONLY
                    if (dgvGSTR2A_B2BA.RowCount > 0) // IF GRID IS NOT NULL AND PAST ONLY ON SELECTED CELLS
                    {
                        foreach (DataGridViewCell oneCell in dgvGSTR2A_B2BA.SelectedCells)
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
                            DisableControls(dgvGSTR2A_B2BA);

                            gRowNo = dgvGSTR2A_B2BA.Rows.Count - 1;
                            int no = tmp;

                            if (iRow > gRowNo - 1)
                            {
                                DataTable dtDGV = new DataTable();
                                //dtDGV = dgvGSTR15.DataSource as DataTable;

                                #region ADD DATATABLE COLUMN

                                // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                                foreach (DataGridViewColumn col in dgvGSTR2A_B2BA.Columns)
                                {
                                    dtDGV.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }
                                #endregion

                                #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                                // CREATE OBJECT ARRAY TO STORE ONE ROW DATA OF MAIN GRID
                                object[] rowValue = new object[dtDGV.Columns.Count];

                                foreach (DataGridViewRow dr in dgvGSTR2A_B2BA.Rows)
                                {
                                    if (dr.Index != dgvGSTR2A_B2BA.Rows.Count - 1) // DON'T ADD LAST ROW
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
                                // PASTE DATA TO EXISTING ROW IN GRID
                                if (line.Length > 0)
                                {
                                    #region ROW PASTE
                                    // SPLIT ONE COPIED ROW DATA TO ARRAY
                                    string[] sCells = line.Split('\t');

                                    for (int i = 0; i < sCells.GetLength(0); ++i)
                                    {
                                        // CHECK GRID COLUMN COUNT
                                        if (iCol + i < this.dgvGSTR2A_B2BA.ColumnCount && i < 14)
                                        {
                                            // SKIP CHECK BOX COLUMN AND SEQUANCE COLUMN TO PASTE DATA
                                            if (iCol == 0)
                                                oCell = dgvGSTR2A_B2BA[iCol + i + 2, iRow];
                                            else if (iCol == 1)
                                                oCell = dgvGSTR2A_B2BA[iCol + i + 1, iRow];
                                            else
                                                oCell = dgvGSTR2A_B2BA[iCol + i, iRow];

                                            sCells[i] = sCells[i].Trim().Replace(",", "");
                                            if (oCell.ColumnIndex != 0)
                                            {
                                                if (dgvGSTR2A_B2BA.Columns[oCell.ColumnIndex].Name != "colChk" && dgvGSTR2A_B2BA.Columns[oCell.ColumnIndex].Name != "colSequence")
                                                {
                                                    #region VALIDATION
                                                    if (sCells[i].ToString().Trim() == "") { dgvGSTR2A_B2BA.Rows[iRow].Cells[oCell.ColumnIndex].Value = DBNull.Value; }
                                                    else
                                                    {
                                                        if (oCell.ColumnIndex >= 2 && oCell.ColumnIndex <= 15)
                                                        {
                                                            if (chkCellValue(sCells[i].Trim(), dgvGSTR2A_B2BA.Columns[oCell.ColumnIndex].Name))
                                                                dgvGSTR2A_B2BA.Rows[iRow].Cells[oCell.ColumnIndex].Value = sCells[i].Trim();
                                                            else
                                                                dgvGSTR2A_B2BA.Rows[iRow].Cells[oCell.ColumnIndex].Value = DBNull.Value;
                                                        }
                                                        else { dgvGSTR2A_B2BA.Rows[iRow].Cells[oCell.ColumnIndex].Value = sCells[i].Trim(); }
                                                    }
                                                    #endregion
                                                }
                                            }
                                            else
                                            {
                                                if (iCol > i)
                                                {
                                                    for (int j = oCell.ColumnIndex; j < dgvGSTR2A_B2BA.Columns.Count; j++)
                                                    {
                                                        //dgvGSTR15.Rows[iRow].Cells[j].Value = sCells[i].Trim();

                                                        #region VALIDATION
                                                        if (sCells[i].ToString().Trim() == "") { dgvGSTR2A_B2BA.Rows[iRow].Cells[j].Value = DBNull.Value; }
                                                        else
                                                        {
                                                            if (j >= 2 && j <= 15)
                                                            {
                                                                if (chkCellValue(sCells[i].Trim(), dgvGSTR2A_B2BA.Columns[j].Name))
                                                                    dgvGSTR2A_B2BA.Rows[iRow].Cells[j].Value = sCells[i].Trim();
                                                                else
                                                                    dgvGSTR2A_B2BA.Rows[iRow].Cells[j].Value = DBNull.Value;
                                                            }
                                                            else { dgvGSTR2A_B2BA.Rows[iRow].Cells[j].Value = sCells[i].Trim(); }
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
                                                    for (int j = oCell.ColumnIndex; j < dgvGSTR2A_B2BA.Columns.Count; j++)
                                                    {
                                                        #region VALIDATION
                                                        if (sCells[i].ToString().Trim() == "") { dgvGSTR2A_B2BA.Rows[iRow].Cells[j].Value = DBNull.Value; }
                                                        else
                                                        {
                                                            if (j >= 2 && j <= 15)
                                                            {
                                                                if (chkCellValue(sCells[i].Trim(), dgvGSTR2A_B2BA.Columns[j].Name))
                                                                    dgvGSTR2A_B2BA.Rows[iRow].Cells[j].Value = sCells[i].Trim();
                                                                else
                                                                    dgvGSTR2A_B2BA.Rows[iRow].Cells[j].Value = DBNull.Value;
                                                            }
                                                            else { dgvGSTR2A_B2BA.Rows[iRow].Cells[j].Value = sCells[i].Trim(); }
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

                    for (int i = 0; i < dgvGSTR2A_B2BA.Rows.Count - 1; i++)
                    {
                        dgvGSTR2A_B2BA.Rows[i].Cells["colSequence"].Value = i + 1;
                    }
                    #endregion

                    // ENABLE CONTROL
                    EnableControls(dgvGSTR2A_B2BA);
                    pbGSTR1.Visible = false;
                }
                if ((e.Control && (e.KeyCode == Keys.A || e.KeyCode == Keys.Subtract)) || (e.KeyCode == Keys.Space && Control.ModifierKeys == Keys.Shift) || (e.Alt && e.KeyCode == Keys.F4))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                pbGSTR1.Visible = false;
                EnableControls(dgvGSTR2A_B2BA);
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
                pbGSTR1.Visible = true;
                DisableControls(dgvGSTR2A_B2BA);

                #region SET DATATABLE
                int cnt = 0, colNo = 0;

                // ASSIGN GRID DATA TO DATATABLE
                DataTable dt = dtDGV;

                if (dt == null)
                {
                    // IF NO RECORD IN GRID THEN CREATE NEW DATATABLE
                    dt = new DataTable();

                    // ADD COLUMN AS PAR MAIN GRID AND SET DATA ACCESS PROPERTY
                    foreach (DataGridViewColumn col in dgvGSTR2A_B2BA.Columns)
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
                                if (iCol + i < this.dgvGSTR2A_B2BA.ColumnCount && colNo < 15)
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
                                                if (colNo >= 2 && colNo <= 15)
                                                {
                                                    if (chkCellValue(sCells[i].Trim(), dgvGSTR2A_B2BA.Columns[colNo].Name))
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
                                            for (int j = colNo; j < dgvGSTR2A_B2BA.Columns.Count; j++)
                                            {
                                                #region VALIDATION
                                                if (sCells[i].ToString().Trim() == "") { dt.Rows[dt.Rows.Count - 1][j] = DBNull.Value; }
                                                else
                                                {
                                                    if (j >= 2 && j <= 15)
                                                    {
                                                        if (chkCellValue(sCells[i].Trim(), dgvGSTR2A_B2BA.Columns[j].Name))
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
                                            for (int j = colNo; j < dgvGSTR2A_B2BA.Columns.Count; j++)
                                            {
                                                #region VALIDATION
                                                if (sCells[i].ToString().Trim() == "") { dt.Rows[dt.Rows.Count - 1][j] = DBNull.Value; }
                                                else
                                                {
                                                    if (j >= 2 && j <= 15)
                                                    {
                                                        if (chkCellValue(sCells[i].Trim(), dgvGSTR2A_B2BA.Columns[j].Name))
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
                    }
                    cnt++;
                }

                #region EXPORT DATATABLE TO GRID

                // IF THERE ARE RECORDS IN DATA TABLE THEN ASSIGN IT TO GRID
                if (dt != null && dt.Rows.Count > 0)
                    dgvGSTR2A_B2BA.DataSource = dt;

                // TOTAL CALCULATION
                string[] colGroup = { "colOrgInvoiceNo", "colResInvoiceNo", "colInvoiceValue", "colTaxableVal", "colIntTax", "colCtrlTax", "colStateTax", "colCessTax" };
                GetTotal(colGroup);

                EnableControls(dgvGSTR2A_B2BA);

                #endregion

                pbGSTR1.Visible = false;
            }
            catch (Exception ex)
            {
                pbGSTR1.Visible = false;
                EnableControls(dgvGSTR2A_B2BA);
                MessageBox.Show("Error : " + ex.Message);
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
                    if (cNo == "colGSTIN") //GSTN
                    {
                        if (Utility.IsGSTN(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colInvoiceDate") // Date
                    {
                        if (Utility.IsDate(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colInvoiceValue" || cNo == "colInvoiceTaxableVal" || cNo == "colIntTax" || cNo == "colCtrlTax" || cNo == "colStateTax" || cNo == "colCessTax") // value
                    {
                        if (Utility.IsDecimalOrNumber(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colRate") // Rate
                    {
                        if (Utility.IsRate(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colInvoiceType") // Invoice Type
                    {
                        if (cellValue == "Regular" || cellValue == "SEZ Exports with payment" || cellValue == "SEZ exports without payment" || cellValue == "Deemed Exports")
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
                    else if (cNo == "colReverseCharge") // Reverse Charge
                    {
                        if (cellValue == "True" || cellValue == "Yes")
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
                MessageBox.Show("Error : " + ex.Message);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
                return false;
            }
        }

        private void dgvGSTR2A4_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string cNo = dgvGSTR2A_B2BA.Columns[e.ColumnIndex].Name;

                if (e.RowIndex >= 0)
                {
                    if (cNo == "colGSTIN" || cNo == "colName" || cNo == "colInvoiceDate" || cNo == "colRate" || cNo == "colPOS" || cNo == "colInvoiceType") // Other
                    {
                        if (!chkCellValue(Convert.ToString(dgvGSTR2A_B2BA.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                            dgvGSTR2A_B2BA.Rows[e.RowIndex].Cells[cNo].Value = "";
                    }
                    else if (cNo == "colInvoiceNo" || cNo == "colInvoiceValue" || cNo == "colInvoiceTaxableVal" || cNo == "colIntTax" || cNo == "colCtrlTax" || cNo == "colStateTax" || cNo == "colCessTax") // value
                    {
                        if (chkCellValue(Convert.ToString(dgvGSTR2A_B2BA.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                        {
                            string[] colNo = { dgvGSTR2A_B2BA.Columns[e.ColumnIndex].Name };
                            GetTotal(colNo);
                        }
                        else { dgvGSTR2A_B2BA.Rows[e.RowIndex].Cells[cNo].Value = ""; }
                    }
                    else if (cNo == "colReverseCharge")
                    {
                        if (chkCellValue(Convert.ToString(dgvGSTR2A_B2BA.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                            dgvGSTR2A_B2BA.Rows[e.RowIndex].Cells[cNo].Value = "True";
                        else
                            dgvGSTR2A_B2BA.Rows[e.RowIndex].Cells[cNo].Value = "False";
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

        public bool IsValidateData()
        {
            try
            {
                int _cnt = 0;
                string _str = "";
                pbGSTR1.Visible = true;
                dgvGSTR2A_B2BA.CurrentCell = dgvGSTR2A_B2BA.Rows[0].Cells[0];
                dgvGSTR2A_B2BA.AllowUserToAddRows = false;

                #region GSTIN Number
                List<DataGridViewRow> list = dgvGSTR2A_B2BA.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsGSTN(Convert.ToString(x.Cells["colGSTIN"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR2A_B2BA.Rows[list[i].Cells["colGSTIN"].RowIndex].Cells["colGSTIN"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper GSTIN of Supplier.\n";
                }
                list = dgvGSTR2A_B2BA.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsGSTN(Convert.ToString(x.Cells["colGSTIN"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR2A_B2BA.Rows[list[i].Cells["colGSTIN"].RowIndex].Cells["colGSTIN"].Style.BackColor = Color.White;
                }
                #endregion

                #region Invoice Date
                list = null;//dd-MM-yyyy
                list = dgvGSTR2A_B2BA.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsDate(Convert.ToString(x.Cells["colInvoiceDate"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR2A_B2BA.Rows[list[i].Cells["colInvoiceDate"].RowIndex].Cells["colInvoiceDate"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper invoice date.\n";
                }
                list = dgvGSTR2A_B2BA.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsDate(Convert.ToString(x.Cells["colInvoiceDate"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR2A_B2BA.Rows[list[i].Cells["colInvoiceDate"].RowIndex].Cells["colInvoiceDate"].Style.BackColor = Color.White;
                }
                #endregion

                #region Invoice Value
                list = null;
                list = dgvGSTR2A_B2BA.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsNumber(Convert.ToString(x.Cells["colInvoiceValue"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR2A_B2BA.Rows[list[i].Cells["colInvoiceValue"].RowIndex].Cells["colInvoiceValue"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper invoice value.\n";
                }
                list = dgvGSTR2A_B2BA.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsNumber(Convert.ToString(x.Cells["colInvoiceValue"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR2A_B2BA.Rows[list[i].Cells["colInvoiceValue"].RowIndex].Cells["colInvoiceValue"].Style.BackColor = Color.White;
                }
                #endregion

                #region Rate
                list = null;
                list = dgvGSTR2A_B2BA.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsNumber(Convert.ToString(x.Cells["colRate"].Value)) || Convert.ToDouble(x.Cells["colRate"].Value) > 100)
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR2A_B2BA.Rows[list[i].Cells["colRate"].RowIndex].Cells["colRate"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper IGST Rate.\n";
                }
                list = dgvGSTR2A_B2BA.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsNumber(Convert.ToString(x.Cells["colRate"].Value)) && Convert.ToDouble(x.Cells["colRate"].Value) <= 100)
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR2A_B2BA.Rows[list[i].Cells["colRate"].RowIndex].Cells["colRate"].Style.BackColor = Color.White;
                }
                #endregion

                #region Integrated tax Amount
                list = null;
                list = dgvGSTR2A_B2BA.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsNumber(Convert.ToString(x.Cells["colIntTax"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR2A_B2BA.Rows[list[i].Cells["colIntTax"].RowIndex].Cells["colIntTax"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper Integrated tax Amount.\n";
                }
                list = dgvGSTR2A_B2BA.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsNumber(Convert.ToString(x.Cells["colIntTax"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR2A_B2BA.Rows[list[i].Cells["colIntTax"].RowIndex].Cells["colIntTax"].Style.BackColor = Color.White;
                }
                #endregion

                #region Central tax Amount
                list = null;
                list = dgvGSTR2A_B2BA.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsNumber(Convert.ToString(x.Cells["colCtrlTax"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR2A_B2BA.Rows[list[i].Cells["colCtrlTax"].RowIndex].Cells["colCtrlTax"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper Central tax Amount.\n";
                }
                list = dgvGSTR2A_B2BA.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsNumber(Convert.ToString(x.Cells["colCtrlTax"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR2A_B2BA.Rows[list[i].Cells["colCtrlTax"].RowIndex].Cells["colCtrlTax"].Style.BackColor = Color.White;
                }
                #endregion

                #region State / UT tax Amount
                list = null;
                list = dgvGSTR2A_B2BA.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsNumber(Convert.ToString(x.Cells["colStateTax"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR2A_B2BA.Rows[list[i].Cells["colStateTax"].RowIndex].Cells["colStateTax"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper State/UT tax Amount.\n";
                }
                list = dgvGSTR2A_B2BA.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsNumber(Convert.ToString(x.Cells["colStateTax"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR2A_B2BA.Rows[list[i].Cells["colStateTax"].RowIndex].Cells["colStateTax"].Style.BackColor = Color.White;
                }
                #endregion

                #region CESS tax Amount
                list = null;
                list = dgvGSTR2A_B2BA.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsNumber(Convert.ToString(x.Cells["colCessTax"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR2A_B2BA.Rows[list[i].Cells["colCessTax"].RowIndex].Cells["colCessTax"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper CESS tax Amount.\n";
                }
                list = dgvGSTR2A_B2BA.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsNumber(Convert.ToString(x.Cells["colCessTax"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR2A_B2BA.Rows[list[i].Cells["colCessTax"].RowIndex].Cells["colCessTax"].Style.BackColor = Color.White;
                }
                #endregion

                dgvGSTR2A_B2BA.AllowUserToAddRows = true;
                pbGSTR1.Visible = false;
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
                pbGSTR1.Visible = false;
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

                pbGSTR1.Visible = true;

                //For text clear before save
                cmbFilter.SelectedIndex = 0;
                txtSearch.Text = "";

                #region ADD DATATABLE COLUMN
                DataTable dt = new DataTable();

                foreach (DataGridViewColumn col in dgvGSTR2A_B2BA.Columns)
                {
                    dt.Columns.Add(col.Name.ToString());
                }
                dt.Columns.Add("colFileStatus");
                #endregion

                #region ASSIGN GRIDVIEW ROWS IN DATATABLE
                object[] rowValue = new object[dt.Columns.Count];

                foreach (DataGridViewRow dr in dgvGSTR2A_B2BA.Rows)
                {
                    //if (dr.Index != dgvGSTR2A3.Rows.Count - 1)// DON'T ADD LAST ROW
                    //{
                    for (int i = 0; i < dr.Cells.Count; i++)
                    {
                        rowValue[i] = Convert.ToString(dr.Cells[i].Value);
                    }

                    rowValue[dr.Cells.Count] = Convert.ToString(CommonHelper.StatusText);

                    dt.Rows.Add(rowValue);
                    //}
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
                    Query = "Delete from SPQR2AB2BAmend where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'"; //and Fld_ReverseCharge ='False'";
                    _Result = objGSTR2A.IUDData(Query);
                    if (_Result != 1)
                    {
                        //FAIL
                        MessageBox.Show("System error.\nPlease try after sometime!");
                        return;
                    }
                    #endregion

                    
                    _Result = objGSTR2A.GSTR2A3B2BABulkEntry(dt, Convert.ToString(CommonHelper.StatusText));

                    if (_Result == 1)
                    {
                        
                        
                        // TOTAL CALCULATION
                        string[] colNo = { "colOrgInvoiceNo", "colResInvoiceNo", "colInvoiceValue", "colTaxableVal", "colIntTax", "colCtrlTax", "colStateTax", "colCessTax" };
                        GetTotal(colNo);

                        #region ADD DATATABLE COLUMN
                        dt = new DataTable();

                        foreach (DataGridViewColumn col in dgvGSTR2A_B2BA.Columns)
                        {
                            dt.Columns.Add(col.Name.ToString());
                        }
                        dt.Columns.Add("colFileStatus");
                        #endregion

                        #region ASSIGN GRIDVIEW ROWS IN DATATABLE
                        object[] rowVal = new object[dt.Columns.Count];

                        if (dgvGSTR2A3Total.Rows.Count == 1)
                        {
                            foreach (DataGridViewRow dr in dgvGSTR2A3Total.Rows)
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

                        _Result = objGSTR2A.GSTR2A3B2BABulkEntry(dt, "Total");
                        if (_Result == 1)
                        {
                            //DONE
                            MessageBox.Show("Data Saved Successfully …!!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // BIND DATA
                            GetData();
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
                        //FAIL
                        MessageBox.Show("System error.\nPlease try after sometime!");
                        return;
                    }
                }
                else
                {
                    #region DELETE RECORD
                    Query = "Delete from SPQR2AB2BAmend where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                    _Result = objGSTR2A.IUDData(Query);
                    if (_Result == 1)
                    {
                        //DONE
                        MessageBox.Show("Record Successfully Deleted!");
                        // MAKE FILE STATUS BLANK
                        ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(0);
                        string[] colNo = { "colOrgInvoiceNo", "colResInvoiceNo", "colInvoiceValue", "colTaxableVal", "colIntTax", "colCtrlTax", "colStateTax", "colCessTax" };
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

                pbGSTR1.Visible = false;
            }
            catch (Exception ex)
            {
                pbGSTR1.Visible = false;
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
                if (dgvGSTR2A_B2BA.CurrentCell.RowIndex == 0 && dgvGSTR2A_B2BA.CurrentCell.ColumnIndex == 0)
                {
                    dgvGSTR2A_B2BA.CurrentCell = dgvGSTR2A_B2BA.Rows[0].Cells[1];
                }
                else { dgvGSTR2A_B2BA.CurrentCell = dgvGSTR2A_B2BA.Rows[0].Cells[0]; }

                pbGSTR1.Visible = true;
                // CREATE FLAG FRO DELETE ROWS
                Boolean flgChk = false; Boolean flgSelect = false;

                // CREATE OBJECT OF SELECTED ROW TO DELETE
                List<DataGridViewRow> toDelete = new List<DataGridViewRow>();

                // CHECK THERE ARE RECORD PRESENT IN GRID
                if (dgvGSTR2A_B2BA.Rows.Count > 0)
                {
                    // FLAG TRUE IF CHECK ALL SELECTED
                    if (ckboxHeader.Checked)
                        flgChk = true;

                    #region ADD SELECTED ROW TO OBJECT FOR DELETE
                    for (int i = 0; i < dgvGSTR2A_B2BA.Rows.Count; i++)
                    {
                        if (dgvGSTR2A_B2BA[0, i].Value != null && dgvGSTR2A_B2BA[0, i].Value.ToString() != "")
                        {
                            if (Convert.ToBoolean(dgvGSTR2A_B2BA[0, i].Value) == true)
                            {
                                // ADD ROW TO OBJECT IF ROW IS SELECTED
                                flgSelect = true;
                                toDelete.Add(dgvGSTR2A_B2BA.Rows[i]);
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
                                foreach (DataGridViewColumn col in dgvGSTR2A_B2BA.Columns)
                                {
                                    dt.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }

                                // ASSIGN BLANK DATATABLE TO GRID
                                dgvGSTR2A_B2BA.DataSource = dt;
                            }
                            else if (flgSelect == true)
                            {
                                // DELETE SELECTED ROW
                                foreach (DataGridViewRow row in toDelete)
                                {
                                    dgvGSTR2A_B2BA.Rows.RemoveAt(row.Index);
                                    Application.DoEvents();
                                }
                            }

                            // SEQUANCING MAIN GRID RECORDS
                            for (int i = 0; i < dgvGSTR2A_B2BA.Rows.Count - 1; i++)
                            {
                                dgvGSTR2A_B2BA.Rows[i].Cells["colSequence"].Value = Convert.ToString(i + 1);
                            }

                            if (dgvGSTR2A_B2BA.Rows.Count == 1)
                            {
                                // IF THERE ARE NO RECORDS IN MAIN GRID THENE ASSIGN BLANK DATATABLE TO TOTAL GRID
                                DataTable dtTotal = new DataTable();
                                foreach (DataGridViewColumn col in dgvGSTR2A3Total.Columns)
                                {
                                    dtTotal.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }
                                dgvGSTR2A3Total.DataSource = dtTotal;
                            }

                            // SET CONTROL PROPERTY AFTER ROW DELETION
                            ckboxHeader.Checked = false;
                            dgvGSTR2A_B2BA.Columns[0].HeaderText = "Check All";
                            #endregion
                        }
                    }

                    // TOTAL CALCULATION
                    string[] colNo = { "colOrgInvoiceNo", "colResInvoiceNo", "colInvoiceValue", "colTaxableVal", "colIntTax", "colCtrlTax", "colStateTax", "colCessTax" };
                    GetTotal(colNo);
                }
                else
                {
                    pbGSTR1.Visible = false;
                    // IF THERE ARE NO RECORD TO DELETE
                    ckboxHeader.Checked = false;
                    dgvGSTR2A_B2BA.Columns[0].HeaderText = "Check All";
                    MessageBox.Show("There are no records to delete.");
                }
                pbGSTR1.Visible = false;
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
                    pbGSTR1.Visible = true;
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
                        dt = (DataTable)dgvGSTR2A_B2BA.DataSource;

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
                                DisableControls(dgvGSTR2A_B2BA);

                                #region IMPORT EXCEL DATATABLE TO GRID DATATABLE
                                if (dtExcel != null && dtExcel.Rows.Count > 0)
                                {
                                    int tmp = 1;
                                    foreach (DataRow row in dtExcel.Rows)
                                    {
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
                                foreach (DataGridViewColumn col in dgvGSTR2A_B2BA.Columns)
                                {
                                    dt.Columns[col.Index].ColumnName = col.Name.ToString();
                                    col.DataPropertyName = col.Name;
                                }
                                #endregion

                                //ASSIGN DATATABLE TO DATAGRID
                                dgvGSTR2A_B2BA.DataSource = dt;

                                // ENABLE MAIN GRID
                                EnableControls(dgvGSTR2A_B2BA);
                            }
                            else
                            {
                                // IF THERE ARE NO RECORDS IN MAIN GRID

                                if (dtExcel != null && dtExcel.Rows.Count > 0)
                                {
                                    // IF THERE ARE DATA IN IMPORTED EXCEL FILE

                                    // DISABLE MAIN GRID
                                    DisableControls(dgvGSTR2A_B2BA);

                                    #region RENAME DATATABLE COLUMN NAME AS PAR MAIN GRID
                                    foreach (DataGridViewColumn col in dgvGSTR2A_B2BA.Columns)
                                    {
                                        dtExcel.Columns[col.Index].ColumnName = col.Name.ToString();
                                        col.DataPropertyName = col.Name;
                                    }
                                    dtExcel.AcceptChanges();

                                    // ASSIGN DATATALE TO GRID
                                    dgvGSTR2A_B2BA.DataSource = dtExcel;

                                    // ENABLE MAIN GRID
                                    EnableControls(dgvGSTR2A_B2BA);
                                    #endregion
                                }
                                else
                                {
                                    // IF THERE ARE NO RECORDS IN IMPORTED EXCEL FILE
                                    MessageBox.Show("There are no records found in imported excel ...!!!!");
                                }
                            }

                            // TOTAL CALCULATION
                            string[] colNo = { "colOrgInvoiceNo", "colResInvoiceNo", "colInvoiceValue", "colTaxableVal", "colIntTax", "colCtrlTax", "colStateTax", "colCessTax" };
                            GetTotal(colNo);
                        }
                        else
                        {
                            pbGSTR1.Visible = false;
                            MessageBox.Show("Please import valid excel template...!!!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please choose .xls or .xlsx file only.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error); //CUSTOM MESSAGEBOX TO SHOW ERROR  
                    } pbGSTR1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                pbGSTR1.Visible = false;
                EnableControls(dgvGSTR2A_B2BA);
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
                        OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [B2BA$]", con);
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
                        for (int i = 1; i < dgvGSTR2A_B2BA.Columns.Count; i++)
                        {
                            Boolean flg = false;
                            for (int j = 0; j < dtexcel.Columns.Count; j++)
                            {
                                // CHECK GRID COLUMN IS PRESENT OR NOT IN IMPORTED EXCEL
                                if (dgvGSTR2A_B2BA.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim().Length >= 40)
                                {
                                    string piece = dgvGSTR2A_B2BA.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim().Substring(0, 40);
                                    string piece1 = string.Empty;

                                    if (dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim().Length >= 40)
                                        piece1 = dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim().Substring(0, 40);
                                    else
                                        piece1 = dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim();

                                    if (piece == piece1)
                                    {
                                        // if grid column present in excel then its index as par grid column index
                                        flg = true;
                                        dtexcel.Columns[j].SetOrdinal(dgvGSTR2A_B2BA.Columns[i].Index - 1);
                                        break;
                                    }
                                }
                                else if (dgvGSTR2A_B2BA.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim() == dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim())
                                {
                                    // IF GRID COLUMN PRESENT IN EXCEL THEN ITS INDEX AS PAR GRID COLUMN INDEX
                                    flg = true;
                                    dtexcel.Columns[j].SetOrdinal(dgvGSTR2A_B2BA.Columns[i].Index - 1);
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
                        if (dtexcel.Columns.Count >= dgvGSTR2A_B2BA.Columns.Count - 2)
                        {
                            for (int i = dtexcel.Columns.Count - 1; i > (dgvGSTR2A_B2BA.Columns.Count - 2); i--)
                            {
                                dtexcel.Columns.Remove(dtexcel.Columns[i]);
                            }
                        }
                        dtexcel.AcceptChanges();
                        #endregion

                        #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME
                        foreach (DataGridViewColumn col in dgvGSTR2A_B2BA.Columns)
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
                            dtexcel.Rows[i]["colChk"] = "False";
                            dtexcel.Rows[i]["colSequence"] = i + 1;
                        }
                        dtexcel.AcceptChanges();
                        #endregion
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

                // return datatable
                return dtexcel;
            }
        }

        public void ExportExcel()
        {
            try
            {
                if (dgvGSTR2A_B2BA.Rows.Count > 0)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID

                    pbGSTR1.Visible = true;

                    #region CREATE WORKBOOK AND ASSIGN COLUMNNAME
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook WB = excelApp.Workbooks.Add(Missing.Value);

                    Excel.Worksheet newWS = (Excel.Worksheet)excelApp.Worksheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    newWS.Name = "B2BA";

                    // DELETE UNUSED WORKSHEETS FROM WORKBOOK
                    foreach (Excel.Worksheet ws in WB.Worksheets)
                    {
                        if (ws.Name != "B2BA")
                            ((Excel.Worksheet)WB.Worksheets[ws.Name]).Delete();
                    }

                    // ASSIGN COLUMN HEADER AS PAR THE GRID HEADER
                    for (int i = 1; i < dgvGSTR2A_B2BA.Columns.Count; i++)
                    {
                        newWS.Cells[1, i] = dgvGSTR2A_B2BA.Columns[i].HeaderText.ToString();

                        // SET COLUMN WIDTH
                        if (i == 1)
                            ((Excel.Range)newWS.Cells[1, i]).ColumnWidth = 7;
                        else if (i == 2)
                            ((Excel.Range)newWS.Cells[1, i]).ColumnWidth = 20;
                        else
                            ((Excel.Range)newWS.Cells[1, i]).ColumnWidth = 15;
                    }

                    // GET RANGE AND SET DIFFRENT PROPERTIES
                    Excel.Range headerRange = (Excel.Range)newWS.get_Range((Excel.Range)newWS.Cells[1, 1], (Excel.Range)newWS.Cells[1, dgvGSTR2A_B2BA.Columns.Count - 1]);
                    headerRange.WrapText = true;
                    headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Calibri";
                    #endregion

                    #region COPY DATA FROM DATATABLE TO ARRAY

                    // CREATE ARRAY TO HOLD THE DATA OF DATATABLE
                    object[,] arr = new object[dgvGSTR2A_B2BA.Rows.Count, dgvGSTR2A_B2BA.Columns.Count];

                    // ASSIGN DATA TO ARRAY FROM DATATABLE
                    if (CommonHelper.IsLicence)
                    {
                        // FOR LICENECE ALLOWS TO EXPORT ALL RECORDS
                        for (int i = 0; i < dgvGSTR2A_B2BA.Rows.Count; i++)
                        {
                            for (int j = 1; j < dgvGSTR2A_B2BA.Columns.Count; j++)
                            {
                                //arr[i, j - 1] = Convert.ToString(dgvGSTR2A_B2BA.Rows[i].Cells[j].Value);

                                if (dgvGSTR2A_B2BA.Columns[j].Name == "colOrgInvoiceDate" || dgvGSTR2A_B2BA.Columns[j].Name == "colResInvoiceDate")
                                {
                                    try
                                    {
                                        DateTime ss = Convert.ToDateTime(dgvGSTR2A_B2BA.Rows[i].Cells[j].Value);
                                        //arr[i, j - 1] = ss.ToString("dd-MM-yyyy").Replace('/', '-');
                                        arr[i, j - 1] = ss;
                                    }
                                    catch (Exception)
                                    {
                                        arr[i, j - 1] = "";
                                    }
                                }
                                else
                                    arr[i, j - 1] = Convert.ToString(dgvGSTR2A_B2BA.Rows[i].Cells[j].Value);
                            }
                        }
                    }
                    else
                    {
                        // FOR DEMO ALLOW ONLY 100 RECORDS TO EXPORT
                        for (int i = 0; i < dgvGSTR2A_B2BA.Rows.Count; i++)
                        {
                            if (i < 100)
                            {
                                for (int j = 1; j < dgvGSTR2A_B2BA.Columns.Count; j++)
                                {
                                    //arr[i, j - 1] = Convert.ToString(dgvGSTR2A_B2BA.Rows[i].Cells[j].Value);
                                    if (dgvGSTR2A_B2BA.Columns[j].Name == "colOrgInvoiceDate" || dgvGSTR2A_B2BA.Columns[j].Name == "colResInvoiceDate")
                                    {
                                        try
                                        {
                                            DateTime ss = Convert.ToDateTime(dgvGSTR2A_B2BA.Rows[i].Cells[j].Value);
                                            //arr[i, j - 1] = ss.ToString("dd-MM-yyyy").Replace('/', '-');
                                            arr[i, j - 1] = ss;
                                        }
                                        catch (Exception)
                                        {
                                            arr[i, j - 1] = "";
                                        }
                                    }
                                    else
                                        arr[i, j - 1] = Convert.ToString(dgvGSTR2A_B2BA.Rows[i].Cells[j].Value);
                                }
                            }
                        }
                    }

                    //SET EXCEL RANGE TO PASTE THE DATA
                    Excel.Range top = (Excel.Range)newWS.Cells[2, 1];
                    Excel.Range bottom = (Excel.Range)newWS.Cells[dgvGSTR2A_B2BA.Rows.Count + 1, dgvGSTR2A_B2BA.Columns.Count];
                    Excel.Range sheetRange = newWS.Range[top, bottom];
                    sheetRange.WrapText = true;
                    sheetRange.Columns.AutoFit();
                    sheetRange.Rows.AutoFit();
                    //sheetRange.NumberFormat = "@";

                    Excel.Range rg = rg = (Excel.Range)sheetRange.Cells[1, 5];
                    rg.EntireColumn.NumberFormat = "dd-MM-yyyy";

                    rg = rg = (Excel.Range)sheetRange.Cells[1, 4];
                    rg.EntireColumn.NumberFormat = "@";

                    rg = rg = (Excel.Range)sheetRange.Cells[1, 7];
                    rg.EntireColumn.NumberFormat = "@";

                    rg = rg = (Excel.Range)sheetRange.Cells[1, 8];
                    rg.EntireColumn.NumberFormat = "dd-MM-yyyy";

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
                pbGSTR1.Visible = false;
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
                    pbGSTR1.Visible = true;
                    // CHCK EXTENTION OF SELECTED FILE
                    if (fileExt.CompareTo(".csv") == 0 || fileExt.CompareTo(".~csv") == 0)
                    {
                        // CREATE DATATABLE AND SAVE GRID DATA
                        DataTable dt = new DataTable();
                        dt = (DataTable)dgvGSTR2A_B2BA.DataSource;

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
                                DisableControls(dgvGSTR2A_B2BA);

                                #region COPY IMPORTED CSV DATATABLE DATA INTO GRID DATATABLE
                                if (dtCsv != null && dtCsv.Rows.Count > 0)
                                {
                                    int tmp = 1;
                                    foreach (DataRow row in dtCsv.Rows)
                                    {
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
                                foreach (DataGridViewColumn col in dgvGSTR2A_B2BA.Columns)
                                {
                                    dtCsv.Columns[col.Index].ColumnName = col.Name.ToString();
                                    col.DataPropertyName = col.Name;
                                }
                                dt.AcceptChanges();
                                #endregion

                                // ASSIGN DATATABLE TO GRID
                                dgvGSTR2A_B2BA.DataSource = dt;

                                // ENABLE MAIN GRID
                                EnableControls(dgvGSTR2A_B2BA);
                            }
                            else
                            {
                                // IF THERE ARE NO RECORDS IN MAIN GRID

                                if (dtCsv != null && dtCsv.Rows.Count > 0)
                                {
                                    // IF THERE ARE RECORD PRESENT IN IMPORT FILE

                                    // DISABLE MAIN GRID
                                    DisableControls(dgvGSTR2A_B2BA);

                                    #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME AND ASSIGN TO MAIN GRID
                                    foreach (DataGridViewColumn col in dgvGSTR2A_B2BA.Columns)
                                    {
                                        dtCsv.Columns[col.Index].ColumnName = col.Name.ToString();
                                        col.DataPropertyName = col.Name;
                                    }
                                    dtCsv.AcceptChanges();

                                    // ASSIGN DATATABLE TO GRID
                                    dgvGSTR2A_B2BA.DataSource = dtCsv;

                                    // ENABLE CONTROL
                                    EnableControls(dgvGSTR2A_B2BA);
                                    #endregion
                                }
                                else
                                {
                                    pbGSTR1.Visible = false;
                                    // IF THERE ARE NO RECORDS IN IMPORT FILE
                                    MessageBox.Show("There are no records in CSV file...!!!");
                                    return;
                                }
                            }

                            // TOTAL CALCULATION
                            string[] colNo = { "colOrgInvoiceNo", "colResInvoiceNo", "colInvoiceValue", "colTaxableVal", "colIntTax", "colCtrlTax", "colStateTax", "colCessTax" };
                            GetTotal(colNo);
                        }
                        else
                        {
                            MessageBox.Show("Please import valid csv template...!!!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please choose .csv or .~csv file only.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error); //CUSTOM MESSAGEBOX TO SHOW ERROR  
                    }
                    pbGSTR1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                pbGSTR1.Visible = false;
                EnableControls(dgvGSTR2A_B2BA);
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
                    for (int i = 1; i < dgvGSTR2A_B2BA.Columns.Count; i++)
                    {
                        Boolean flg = false;
                        for (int j = 0; j < csvData.Columns.Count; j++)
                        {
                            // CHECK GRID COLUMN IS PRESENT OR NOT IN IMPORTED EXCEL
                            if (dgvGSTR2A_B2BA.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim().Length >= 40)
                            {
                                string piece = dgvGSTR2A_B2BA.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim().Substring(0, 40);
                                string piece1 = string.Empty;

                                if (csvData.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim().Length >= 40)
                                    piece1 = csvData.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim().Substring(0, 40);
                                else
                                    piece1 = csvData.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim();

                                if (piece == piece1)
                                {
                                    // if grid column present in excel then its index as par grid column index
                                    flg = true;
                                    csvData.Columns[j].SetOrdinal(dgvGSTR2A_B2BA.Columns[i].Index - 1);
                                    break;
                                }
                            }
                            else if (dgvGSTR2A_B2BA.Columns[i].HeaderText.Replace(" ", "") == csvData.Columns[j].ColumnName.Replace(" ", "").Trim())
                            {
                                // IF GRID COLUMN PRESENT IN EXCEL THEN ITS INDEX AS PAR GRID COLUMN INDEX
                                flg = true;
                                csvData.Columns[j].SetOrdinal(dgvGSTR2A_B2BA.Columns[i].Index - 1);
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
                    if (csvData.Columns.Count >= dgvGSTR2A_B2BA.Columns.Count - 2)
                    {
                        for (int i = csvData.Columns.Count - 1; i > (dgvGSTR2A_B2BA.Columns.Count - 2); i--)
                        {
                            csvData.Columns.Remove(csvData.Columns[i]);
                        }
                    }
                    #endregion

                    #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME
                    foreach (DataGridViewColumn col in dgvGSTR2A_B2BA.Columns)
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
                        csvData.Rows[i]["colChk"] = "False";
                        csvData.Rows[i]["colSequence"] = i + 1;
                    }
                    csvData.AcceptChanges();
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
                if (dgvGSTR2A_B2BA.Rows.Count > 1)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID
                    pbGSTR1.Visible = true;
                    string csv = string.Empty;

                    // CREATE DATATABLE AND GET GRID DATA
                    DataTable dt = new DataTable();
                    dt = (DataTable)dgvGSTR2A_B2BA.DataSource;
                    dt.AcceptChanges();

                    #region ASSIGN COLUMN NAME TO CSV STRING
                    for (int i = 1; i < dgvGSTR2A_B2BA.Columns.Count; i++)
                    {
                        csv += dgvGSTR2A_B2BA.Columns[i].HeaderText + ',';
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

                    pbGSTR1.Visible = false;
                }
                else
                {
                    // IF THERE ARE NO RECORD IN MAIN  GRID
                    MessageBox.Show("ExportToCSV: There are no records to export...!!!");
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

        #endregion

        #region PDF TRANSACTIONS

        public void ExportPDF()
        {
            try
            {
                if (dgvGSTR2A_B2BA.Rows.Count > 1)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID
                    pbGSTR1.Visible = true;
                    #region CREATING ITEXTSHARP TABLE FROM THE DATATABLE DATA AND ASSIGNING TABLE HEADER
                    PdfPTable pdfTable = new PdfPTable(dgvGSTR2A_B2BA.ColumnCount - 1);
                    pdfTable.DefaultCell.Padding = 3;
                    pdfTable.WidthPercentage = 100;
                    pdfTable.DefaultCell.BorderWidth = 0;
                    iTextSharp.text.Font fontHeader = iTextSharp.text.FontFactory.GetFont("Calibri", 6);

                    // ADD HEADER TO PDF TABLE
                    string headerName = "4. Inward supplies received from Registered Taxable Persons";
                    pdfTable = AssignHeader(pdfTable, headerName);
                    #endregion

                    #region ADDING HEADER ROW
                    int i = 0;
                    foreach (DataGridViewColumn column in dgvGSTR2A_B2BA.Columns)
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
                        foreach (DataGridViewRow row in dgvGSTR2A_B2BA.Rows)
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
                    else
                    {
                        // FOR DEMO ALLOW ONLY 100 RECORDS TO EXPORT
                        foreach (DataGridViewRow row in dgvGSTR2A_B2BA.Rows)
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
                    pbGSTR1.Visible = false;
                }
                else
                {
                    // IF THERE ARE NO RECORD IN MAIN  GRID
                    MessageBox.Show("ExportToPDF: There are no records to export...!!!");
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

        public PdfPTable AssignHeader(PdfPTable pdfTable, string HeaderName)
        {
            try
            {
                // ADD HEADER TO PDF TABLE
                iTextSharp.text.Font fontHeader = iTextSharp.text.FontFactory.GetFont("Calibri", 10);
                PdfPCell ce1 = new PdfPCell(new Phrase(HeaderName, fontHeader));
                ce1.Colspan = dgvGSTR2A_B2BA.Columns.Count;
                ce1.VerticalAlignment = Element.ALIGN_CENTER;
                ce1.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(ce1);

                PdfPCell ce2 = new PdfPCell(new Phrase(" "));
                ce2.Colspan = dgvGSTR2A_B2BA.Columns.Count;
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
                // do not allow to auto generate columns
                dgvGSTR2A_B2BA.AutoGenerateColumns = false;
                dgvGSTR2A_B2BA.AllowUserToAddRows = false;

                // set height width of form
                this.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.97));
                this.Height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.77));

                // set width of header, main and total grid
                this.pnlHeader.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.96));
                this.dgvGSTR2A_B2BA.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.96));
                this.dgvGSTR2A3Total.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.96));

                // set height of main grid
                this.dgvGSTR2A_B2BA.Height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.65));

                // set location of header,loading pic, checkbox and main and total grid
                //this.pnlHeader.Location = new System.Drawing.Point(12, 0);
                //this.dgvGSTR2A_B2BA.Location = new System.Drawing.Point(12, Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.05)));
                //this.dgvGSTR2A3Total.Location = new System.Drawing.Point(12, Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.71)));
                //this.ckboxHeader.Location = new System.Drawing.Point(32, Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.135)));
                //this.pbGSTR1.Location = new System.Drawing.Point(Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.45)), Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.30)));

                dgvGSTR2A_B2BA.EnableHeadersVisualStyles = false;
                dgvGSTR2A_B2BA.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(23, 196, 187);
                dgvGSTR2A_B2BA.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

                this.dgvGSTR2A_B2BA.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                this.dgvGSTR2A_B2BA.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                this.dgvGSTR2A_B2BA.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

                foreach (DataGridViewColumn column in dgvGSTR2A_B2BA.Columns)
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

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)dgvGSTR2A_B2BA.DataSource;
                if (dt == null || dt.Rows.Count == 0)
                {
                    if (txtSearch.Text.ToString().Trim() != "")
                        MessageBox.Show("Kindly save record(s) before search!!!", "Alert", MessageBoxButtons.OK);
                    return;
                }
                if (cmbFilter.SelectedValue.ToString() == "")
                {
                    ((DataTable)dgvGSTR2A_B2BA.DataSource).DefaultView.RowFilter = string.Format("colSequence like '%{0}%' or colOrgInvoiceNo like '%{0}%' or colOrgInvoiceDate like '%{0}%' or colInvoiceType like '%{0}%' or colResInvoiceNo like '%{0}%' or colResInvoiceDate like '%{0}%' or colPOS like '%{0}%' or colSupAttResCharge like '%{0}%' or colApplicablePer like '%{0}%' or colInvoiceValue like '%{0}%' or colRate like '%{0}%' or colTaxableVal like '%{0}%' or colIntTax like '%{0}%' or colCtrlTax like '%{0}%' or colStateTax like '%{0}%' or colCessTax like '%{0}%'", txtSearch.Text.Trim().Replace("'", "''"));
                }
                else
                {
                    ((DataTable)dgvGSTR2A_B2BA.DataSource).DefaultView.RowFilter = string.Format("" + cmbFilter.SelectedValue + " like '%{0}%'", txtSearch.Text.Trim().Replace("'", "''"));
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

        private void dgvGSTR2A4_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                // SET INDEX OF USER ADDED ROW IN MAIN GRID
                dgvGSTR2A_B2BA.Rows[e.Row.Index - 1].Cells[1].Value = e.Row.Index;
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

        private void dgvGSTR2A4_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                // SET SEQUNCING AFTER USER DELETING ROW IN GRID

                for (int i = e.Row.Index; i < dgvGSTR2A_B2BA.Rows.Count - 1; i++)
                {
                    dgvGSTR2A_B2BA.Rows[i].Cells["colSequence"].Value = i;
                }

                // TOTAL CALCULATION
                string[] colNo = { "colOrgInvoiceNo", "colResInvoiceNo", "colInvoiceValue", "colTaxableVal", "colIntTax", "colCtrlTax", "colStateTax", "colCessTax" };
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
                if (c.Name != "frmGSTR2A3" && c.Name != "dgvGSTR2A3Total")
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

        #region SCROLL GRID

        private void dgvGSTR2A4_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                // SET TOTAL GRID OFFSET AS PAR MAIN GRID SCROLL
                this.dgvGSTR2A3Total.HorizontalScrollingOffset = this.dgvGSTR2A_B2BA.HorizontalScrollingOffset;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
            }
        }

        private void dgvGSTR2A4Total_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                // SET TOTAL GRID OFFSET AS PAR MAIN GRID SCROLL
                this.dgvGSTR2A_B2BA.HorizontalScrollingOffset = this.dgvGSTR2A3Total.HorizontalScrollingOffset;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
            }
        }

        #endregion

        #region CHECK ALL AND UNCHECK ALL

        private void dgvGSTR2A4_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                // CHECK FIRST COLUMN HEADER PRESSED AND MAIN GRID HAVING RECORDS
                if (e.ColumnIndex == 0 && dgvGSTR2A_B2BA.Rows.Count > 1)
                {
                    // CHECK AND UNCHECK CHECK BOX OF HEADER FOR SELECTING AND UNSELECTING ALL RECORDS
                    if (dgvGSTR2A_B2BA.Columns[e.ColumnIndex].HeaderText == "Check All")
                        ckboxHeader.Checked = true;
                    else if (dgvGSTR2A_B2BA.Columns[e.ColumnIndex].HeaderText == "Uncheck All")
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
                if (dgvGSTR2A_B2BA.Rows.Count > 0)
                {
                    if (ckboxHeader.Checked)
                    {
                        // IF CHECK BOX IS CHECKED
                        pbGSTR1.Visible = true;
                        // SET CHECK BOX COLUMN VALUE AS TRUE
                        for (int i = 0; i < dgvGSTR2A_B2BA.Rows.Count; i++)
                        {
                            dgvGSTR2A_B2BA.Rows[i].Cells[0].Value = "True";
                            Application.DoEvents();
                        }

                        // CHANGE HEADER TEXT
                        dgvGSTR2A_B2BA.Columns[0].HeaderText = "Uncheck All";
                        pbGSTR1.Visible = false;
                    }
                    else if (ckboxHeader.Checked == false)
                    {
                        // IF CHECK BOX IS UNCHECKED
                        pbGSTR1.Visible = true;
                        // SET CHECK BOX COLUMN VALUE AS FALSE
                        for (int i = 0; i < dgvGSTR2A_B2BA.Rows.Count; i++)
                        {
                            dgvGSTR2A_B2BA.Rows[i].Cells[0].Value = "False";
                            Application.DoEvents();
                        }
                        // CHANGE HEADER TEXT
                        dgvGSTR2A_B2BA.Columns[0].HeaderText = "Check All";
                        pbGSTR1.Visible = false;
                    }
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

        #endregion

        public void JSONCreator()
        {
            try
            {
                RootObject ObjJson = new RootObject();

                #region Invoice Number Group By
                List<string> list = dgvGSTR2A_B2BA.Rows
                       .OfType<DataGridViewRow>()
                       .Select(x => Convert.ToString(x.Cells[3].Value))
                       .Distinct().ToList();
                #endregion

                if (list != null && list.Count > 0)
                {
                    pbGSTR1.Visible = true;
                    List<B2b> obj = new List<B2b>();

                    B2b b2bObj = new B2b();
                    b2bObj.ctin = Convert.ToString(dgvGSTR2A_B2BA.Rows[0].Cells[2].Value);
                    obj.Add(b2bObj);

                    ObjJson.b2b = obj;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i] != "")
                        {
                            #region Invoice Number
                            List<DataGridViewRow> Invoicelist = dgvGSTR2A_B2BA.Rows
                                   .OfType<DataGridViewRow>()
                                   .Where(x => list[i] == Convert.ToString(x.Cells[3].Value))
                                   .Select(x => x)
                                   .ToList();
                            #endregion

                            if (Invoicelist != null && Invoicelist.Count > 0)
                            {
                                List<Inv> objInv = new List<Inv>();
                                List<Itm> objItm = new List<Itm>();
                                List<ItmDet> objItemDetails = new List<ItmDet>();

                                for (int j = 0; j < Invoicelist.Count; j++)
                                {
                                    if (j == 0)
                                    {
                                        #region Invoice Details
                                        Inv clsInv = new Inv();
                                        clsInv.pos = Convert.ToString(Invoicelist[j].Cells["colPOS"].Value);//POS
                                        clsInv.inum = Convert.ToString(Invoicelist[j].Cells["colInvoiceNo"].Value);
                                        clsInv.idt = Convert.ToString(Invoicelist[j].Cells["colInvoiceDate"].Value);
                                        //clsInv.rchrg = Convert.ToString(Invoicelist[j].Cells[16].Value);//RCharge

                                        if (ObjJson.b2b[0].inv == null)
                                        {
                                            objInv.Add(clsInv); ObjJson.b2b[0].inv = objInv;
                                        }
                                        else { objInv.Add(clsInv); ObjJson.b2b[0].inv.AddRange(objInv); }
                                        #endregion

                                        #region New parameter
                                        //clsInv.flag = Convert.ToString(Invoicelist[j].Cells[3].Value);
                                        //clsInv.updby = Convert.ToString(Invoicelist[j].Cells[3].Value);
                                        //clsInv.chksum = Convert.ToString(Invoicelist[j].Cells[3].Value);
                                        //clsInv.etin = Convert.ToString(Invoicelist[j].Cells[3].Value);
                                        #endregion
                                    }

                                    Itm clsItems = new Itm();
                                    clsItems.num = j + 1;

                                    #region Invoice Item Details
                                    ItmDet clsItmDet = new ItmDet();
                                    if (Convert.ToString(Invoicelist[j].Cells["colInvoiceGoodsServi"].Value).ToLower() == "goods" || Convert.ToString(Invoicelist[j].Cells["colInvoiceGoodsServi"].Value).ToLower() == "g")
                                    {
                                        clsItmDet.ty = Convert.ToString("G");
                                    }
                                    else if (Convert.ToString(Invoicelist[j].Cells["colInvoiceGoodsServi"].Value).ToLower() == "services" || Convert.ToString(Invoicelist[j].Cells["colInvoiceGoodsServi"].Value).ToLower() == "s")
                                    {
                                        clsItmDet.ty = Convert.ToString("S");
                                    }
                                    else { clsItmDet.ty = Convert.ToString(""); }

                                    clsItmDet.hsn_sc = Convert.ToString(Invoicelist[j].Cells["colInvoiceHSNSAC"].Value);

                                    if (Convert.ToString(Invoicelist[j].Cells["colInvoiceTaxableVal"].Value).Trim() != "")//Taxable Value
                                    {
                                        clsItmDet.txval = Convert.ToDouble(Invoicelist[j].Cells["colInvoiceTaxableVal"].Value);
                                    }
                                    else { clsItmDet.txval = 0.0; }
                                    if (Convert.ToString(Invoicelist[j].Cells["colIGSTRate"].Value).Trim() != "")//IGST Rate
                                    {
                                        clsItmDet.irt = Convert.ToInt32(Invoicelist[j].Cells["colIGSTRate"].Value);
                                    }
                                    else { clsItmDet.irt = 0; }
                                    if (Convert.ToString(Invoicelist[j].Cells["colIntTax"].Value).Trim() != "")//IGST Amount
                                    {
                                        clsItmDet.iamt = Convert.ToInt32(Invoicelist[j].Cells["colIntTax"].Value);
                                    }
                                    else { clsItmDet.iamt = 0; }
                                    if (Convert.ToString(Invoicelist[j].Cells["colCGSTRate"].Value).Trim() != "")//CGST Rate
                                    {
                                        clsItmDet.crt = Convert.ToDouble(Invoicelist[j].Cells["colCGSTRate"].Value);
                                    }
                                    else { clsItmDet.crt = 0.0; }
                                    if (Convert.ToString(Invoicelist[j].Cells["colCtrlTax"].Value).Trim() != "")//CGST Amount
                                    {
                                        clsItmDet.camt = Convert.ToDouble(Invoicelist[j].Cells["colCtrlTax"].Value);
                                    }
                                    else { clsItmDet.camt = 0.0; }
                                    if (Convert.ToString(Invoicelist[j].Cells["colSGSTRate"].Value).Trim() != "")//SGST Rate
                                    {
                                        clsItmDet.srt = Convert.ToDouble(Invoicelist[j].Cells["colSGSTRate"].Value);
                                    }
                                    else { clsItmDet.srt = 0.0; }
                                    if (Convert.ToString(Invoicelist[j].Cells["colStateTax"].Value).Trim() != "")//SGST Amount
                                    {
                                        clsItmDet.samt = Convert.ToDouble(Invoicelist[j].Cells["colStateTax"].Value);
                                    }
                                    else { clsItmDet.samt = 0.0; }
                                    if (Convert.ToString(Invoicelist[j].Cells["colCessRate"].Value).Trim() != "")//Cess Rate
                                    {
                                        clsItmDet.csrt = Convert.ToDouble(Invoicelist[j].Cells["colCessRate"].Value);
                                    }
                                    else { clsItmDet.csrt = 0.0; }
                                    if (Convert.ToString(Invoicelist[j].Cells["colCessTax"].Value).Trim() != "")//Cess Amount
                                    {
                                        clsItmDet.csamt = Convert.ToDouble(Invoicelist[j].Cells["colCessTax"].Value);
                                    }
                                    else { clsItmDet.csamt = 0.0; }

                                    #region New Paramter
                                    //if (Convert.ToString(Invoicelist[j].Cells[13].Value).Trim() != "")//SGST Rate
                                    //{
                                    //    clsItmDet.csrt = Convert.ToDouble(Invoicelist[j].Cells[13].Value);
                                    //}
                                    //else { clsItmDet.csrt = 0.0; }
                                    //if (Convert.ToString(Invoicelist[j].Cells[14].Value).Trim() != "")//SGST Amount
                                    //{
                                    //    clsItmDet.csamt = Convert.ToDouble(Invoicelist[j].Cells[14].Value);
                                    //}
                                    //else { clsItmDet.csamt = 0.0; }
                                    #endregion

                                    clsItems.itm_det = clsItmDet;
                                    objItm.Add(clsItems);
                                    ObjJson.b2b[0].inv[i].itms = objItm;
                                    #endregion
                                }
                            }
                        }
                    }
                    #region File Save
                    JavaScriptSerializer objScript = new JavaScriptSerializer();
                    objScript.MaxJsonLength = 2147483647;
                    string FinalJson = objScript.Serialize(ObjJson);
                    SaveFileDialog save = new SaveFileDialog();
                    save.FileName = "B2B.json";
                    save.Filter = "Json File | *.json";
                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        StreamWriter writer = new StreamWriter(save.OpenFile());
                        writer.WriteLine(FinalJson);
                        writer.Dispose();
                        writer.Close();
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

        #region JSON CLass
        public class ItmDet
        {
            public string ty { get; set; }
            public string hsn_sc { get; set; }
            public double txval { get; set; }
            public int irt { get; set; }
            public int iamt { get; set; }
            public double crt { get; set; }
            public double camt { get; set; }
            public double srt { get; set; }
            public double samt { get; set; }
            public double csrt { get; set; }
            public double csamt { get; set; }
        }

        public class Itm
        {
            public int num { get; set; }
            public ItmDet itm_det { get; set; }
        }

        public class Inv
        {
            public string chksum { get; set; }
            public string inum { get; set; }
            public string idt { get; set; }
            public double val { get; set; }
            public string pos { get; set; }
            public string rchrg { get; set; }
            public List<Itm> itms { get; set; }
        }

        public class B2b
        {
            public string ctin { get; set; }
            public string cfs { get; set; }
            public List<Inv> inv { get; set; }
        }

        public class RootObject
        {
            public List<B2b> b2b { get; set; }
        }
        #endregion

        private void dgvGSTR2A4Total_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                if (dgvGSTR2A3Total.Rows.Count > 0)
                {
                    DataGridViewRow row = this.dgvGSTR2A3Total.RowTemplate;
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

        private void frmGSTR2A4_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName("");
        }

        public void ValidataAndGetGSTIN()
        {
            try
            {
                if (dgvGSTR2A_B2BA.Rows.Count > 0)
                {
                    pbGSTR1.Visible = true;
                    new PrefillHelper().GetNameByGSTIN(dgvGSTR2A_B2BA, "colGSTIN", "colName");
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
    }
}