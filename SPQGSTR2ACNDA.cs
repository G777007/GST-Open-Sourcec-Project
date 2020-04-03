using SPEQTAGST.BAL;
using SPEQTAGST.BAL.M125r2a;
using SPEQTAGST.BAL.M956r2;
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
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.VisualBasic.FileIO;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using SPEQTAGST.BAL.M264r1;
using SPEQTAGST.Usermain;

namespace SPEQTAGST.cachsR2a
{
    public partial class SPQGSTR2ACNDA : Form
    {
        R2APublicclass objGSTR2 = new R2APublicclass();

        public SPQGSTR2ACNDA()
        {
            InitializeComponent();
            SetGridViewColor();

            GetData();

            string[] colNo = { "colDbtCrdtNoteNo", "colOrgInvoiceNo", "colInvoiceNumber", "colOrginvoiceValue", "colTaxable", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };
            GetTotal(colNo);

            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName(CommonHelper.ReturnName);

            pbGSTR1.Visible = false;

            dgvGSTR27Other.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dgvGSTR27Other.EnableHeadersVisualStyles = false;
            dgvGSTR27Other.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);
            dgvGSTR27Other.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvGSTR27Other.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dgvGSTR27OtherTotal.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        #region Filter
        private void PrefillData()
        {
            //try
            //{
            //    #region JSON DATA STATIC
            //    string _json = "{ \"cdn\": [ { \"ctin\": \"01AAAAP1208Q1ZS\", \"cfs\": \"Y/N\", \"nt\": [ { \"flag\": \"A\", \"chksum\": \"AflJufPlFStqKBZ\", \"ntty\": \"C\", \"nt_num\": \"533515\", \"nt_dt\": \"23-09-2016\", \"rsn\": \"Not mentioned\", \"inum\": \"915914\", \"idt\": \"23-09-2016\", \"val\": 5225.28, \"irt\": 48.76, \"iamt\": 845.22, \"crt\": 95.79, \"camt\": 37661.29, \"srt\": 6.45, \"samt\": 42.13, \"csrt\": 10, \"csamt\": 789.52, \"updby\": \"R/S\", \"elg\": \"ip\", \"rchrg\": \"N\", \"itc\": { \"tx_i\": 147.2, \"tx_s\": 159.3, \"tx_c\": 159.3, \"tx_cs\": 0, \"tc_c\": 0, \"tc_i\": 7896.3, \"tc_s\": 4563.2, \"tc_cs\": 0 } } ] } ] }";
            //    #endregion

            //    RootObject obj = JsonConvert.DeserializeObject<RootObject>(_json);

            //    #region ADD DATATABLE COLUMN
            //    DataTable dt = new DataTable();

            //    foreach (DataGridViewColumn col in dgvGSTR27Other.Columns)
            //    {
            //        if (col.Name.ToLower() != "colchk")
            //        {
            //            dt.Columns.Add(col.Name.ToString());
            //            col.DataPropertyName = col.Name;
            //        }
            //    }

            //    #endregion

            //    #region ASSIGN GRIDVIEW ROWS IN DATATABLE
            //    for (int i = 0; i < obj.cdn.Count; i++)
            //    {
            //        for (int j = 0; j < obj.cdn[i].nt.Count; j++)
            //        {
            //            dt.Rows.Add();
            //            //ROOT START
            //            dt.Rows[dt.Rows.Count - 1]["colGSTIN"] = Convert.ToString(obj.cdn[i].ctin);
            //            //ROOT END

            //            //Item DATA START
            //            dt.Rows[dt.Rows.Count - 1]["colTypeOfNote"] = Convert.ToString(obj.cdn[i].nt[j].ntty);
            //            dt.Rows[dt.Rows.Count - 1]["colDbtCrdtNoteNo"] = Convert.ToString(obj.cdn[i].nt[j].nt_num);
            //            dt.Rows[dt.Rows.Count - 1]["colDbtCrdtNoteDate"] = Convert.ToString(obj.cdn[i].nt[j].nt_dt);
            //            dt.Rows[dt.Rows.Count - 1]["colOrgInvoiceNo"] = Convert.ToString(obj.cdn[i].nt[j].inum);
            //            dt.Rows[dt.Rows.Count - 1]["colOrginvoiceDate"] = Convert.ToString(obj.cdn[i].nt[j].idt);
            //            dt.Rows[dt.Rows.Count - 1]["colDiffValue"] = Convert.ToString(obj.cdn[i].nt[j].val);
            //            dt.Rows[dt.Rows.Count - 1]["colRate"] = Convert.ToString(obj.cdn[i].nt[j].irt);
            //            dt.Rows[dt.Rows.Count - 1]["colIGSTAmnt"] = Convert.ToString(obj.cdn[i].nt[j].iamt);
            //            dt.Rows[dt.Rows.Count - 1]["colCGSTRate"] = Convert.ToString(obj.cdn[i].nt[j].crt);
            //            dt.Rows[dt.Rows.Count - 1]["colCGSTAmnt"] = Convert.ToString(obj.cdn[i].nt[j].camt);
            //            dt.Rows[dt.Rows.Count - 1]["colSGSTRate"] = Convert.ToString(obj.cdn[i].nt[j].srt);
            //            dt.Rows[dt.Rows.Count - 1]["colSGSTAmnt"] = Convert.ToString(obj.cdn[i].nt[j].samt);
            //            dt.Rows[dt.Rows.Count - 1]["colElgbForITC"] = Convert.ToString(obj.cdn[i].nt[j].elg);
            //            //Item DATA End

            //            //SubItem DATA START
            //            dt.Rows[dt.Rows.Count - 1]["colTotalTaxCGST"] = Convert.ToString(obj.cdn[i].nt[j].itc.tx_c);
            //            dt.Rows[dt.Rows.Count - 1]["colTotalTaxIGST"] = Convert.ToString(obj.cdn[i].nt[j].itc.tx_i);
            //            dt.Rows[dt.Rows.Count - 1]["colTotalTaxSGST"] = Convert.ToString(obj.cdn[i].nt[j].itc.tx_s);
            //            dt.Rows[dt.Rows.Count - 1]["colITCCGST"] = Convert.ToString(obj.cdn[i].nt[j].itc.tc_c);
            //            dt.Rows[dt.Rows.Count - 1]["colITCIGST"] = Convert.ToString(obj.cdn[i].nt[j].itc.tc_i);
            //            dt.Rows[dt.Rows.Count - 1]["colITCSGST"] = Convert.ToString(obj.cdn[i].nt[j].itc.tc_s);
            //            //SubItem DATA End

            //            #region New 22-04-2017
            //            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.cdn[i].cfs);
            //            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.cdn[i].nt[j].flag);
            //            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.cdn[i].nt[j].chksum);
            //            dt.Rows[dt.Rows.Count - 1]["colCessRate"] = Convert.ToString(obj.cdn[i].nt[j].crt);
            //            dt.Rows[dt.Rows.Count - 1]["colCessAmnt"] = Convert.ToString(obj.cdn[i].nt[j].csamt);
            //            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.cdn[i].nt[j].updby);
            //            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.cdn[i].nt[j].rchrg);
            //            dt.Rows[dt.Rows.Count - 1]["colTotalTaxCESS"] = Convert.ToString(obj.cdn[i].nt[j].itc.tx_cs);
            //            dt.Rows[dt.Rows.Count - 1]["colITCCESS"] = Convert.ToString(obj.cdn[i].nt[j].itc.tc_cs);
            //            #endregion
            //        }
            //    }
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        dt.Rows[i]["colSequence"] = Convert.ToString(i + 1);
            //    }

            //    dt.AcceptChanges();
            //    dgvGSTR27Other.DataSource = dt;
            //    string[] colNo = { "colDbtCrdtNoteNo", "colOrgInvoiceNo", "colInvoiceNumber", "colOrginvoiceValue", "colTaxable", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };
            //    GetTotal(colNo);
            //    #endregion
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Prefill Data Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
            //    StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
            //    errorWriter.Write(errorMessage);
            //    errorWriter.Close();
            //}
        }

        #endregion

        private void GetData()
        {
            try
            {
                // CREATE DATATABLE TO STORE DATABASE DATA
                DataTable dt = new DataTable();
                string Query = "Select * from SPQR2ACNDAmend where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
                Application.DoEvents();

                // GET DATA FROM DATABASE
                dt = objGSTR2.GetDataGSTR2A(Query);

                if (dt != null && dt.Rows.Count > 0)
                {
                    // ASSIGN FILE STATUS FILED VALUE
                    if (Convert.ToString(dt.Rows[0]["Fld_FileStatus"]).ToLower() == "draft")
                        ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(1);
                    else if (Convert.ToString(dt.Rows[0]["Fld_FileStatus"]).ToLower() == "completed")
                        ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(2);
                    else if (Convert.ToString(dt.Rows[0]["Fld_FileStatus"]).ToLower() == "not-completed")
                        ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(3);

                    dt.Columns.Remove("Fld_Month");
                    dt.Columns.Remove("Fld_FileStatus");
                    dt.Columns.Remove("Fld_ID");

                    dt.Columns.Add(new DataColumn("colChk"));
                    dt.Columns["colChk"].SetOrdinal(0);

                    // RENAME DATATABLE COLUMN NAME TO DATAGRIDVIEW COLUMN NAME
                    foreach (DataGridViewColumn col in dgvGSTR27Other.Columns)
                    {
                        dt.Columns[col.Index].ColumnName = col.Name.ToString();
                        col.DataPropertyName = col.Name;
                    }
                    dt.AcceptChanges();

                    // ASSIGN DATATABLE TO DATA GRID VIEW
                    dgvGSTR27Other.DataSource = dt;
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
                if (dgvGSTR27Other.Rows.Count > 0)
                {
                    // IF MAIN GRID HAVING RECORDS

                    if (dgvGSTR27OtherTotal.Rows.Count == 0)
                    {
                        #region IF TOTAL GRID HAVING NO RECORD
                        // CREATE TEMPORARY DATATABLE TO STORE COLUMN CALCULATION
                        DataTable dtTotal = new DataTable();

                        // ADD COLUMN AS PAR DATAGRIDVIEW COLUMN
                        foreach (DataGridViewColumn col in dgvGSTR27OtherTotal.Columns)
                        {
                            dtTotal.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;
                        }

                        // CREATE DATAROW TO STORE GRID COLUMN CALCULATION
                        DataRow dr = dtTotal.NewRow();

                        #region colTDbtCrdtNoteNo
                        DataTable dt = new DataTable();
                        #region ADD DATATABLE COLUMN

                        // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                        foreach (DataGridViewColumn col in dgvGSTR27Other.Columns)
                        {
                            dt.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;
                        }
                        #endregion
                        #region ASSIGN GRIDVIEW ROWS IN DATATABLE
                        object[] rowValue = new object[dt.Columns.Count];

                        foreach (DataGridViewRow drn in dgvGSTR27Other.Rows)
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
                                      where row.Field<string>("colDbtCrdtNoteNo") != "" && row.Field<string>("colGSTIN") != ""
                                      group row by new { colGSTIN = row.Field<string>("colGSTIN"), colInvNo = row.Field<string>("colDbtCrdtNoteNo") } into grp
                                      select new
                                      {
                                          colGSTIN = grp.Key.colGSTIN,
                                          colInvNo = grp.Key.colInvNo,
                                      }).ToList();

                        if (result != null && result.Count > 0)
                            dr["colTDbtCrdtNoteNo"] = result.Count;
                        else
                            dr["colTDbtCrdtNoteNo"] = 0;

                        var result1 = (from row in dt.AsEnumerable()
                                       where row.Field<string>("colOrgInvoiceNo") != "" && row.Field<string>("colGSTIN") != ""
                                       group row by new { colGSTIN = row.Field<string>("colGSTIN"), colInvNo = row.Field<string>("colOrgInvoiceNo") } into grp
                                      select new
                                      {
                                          colGSTIN = grp.Key.colGSTIN,
                                          colInvNo = grp.Key.colInvNo,
                                      }).ToList();

                        if (result1 != null && result1.Count > 0)
                            dr["colTOrgInvoiceNo"] = result1.Count;
                        else
                            dr["colTOrgInvoiceNo"] = 0;

                        var result2 = (from row in dt.AsEnumerable()
                                       where row.Field<string>("colInvoiceNumber") != "" && row.Field<string>("colGSTIN") != ""
                                       group row by new { colGSTIN = row.Field<string>("colGSTIN"), colInvNo = row.Field<string>("colInvoiceNumber") } into grp
                                       select new
                                       {
                                           colGSTIN = grp.Key.colGSTIN,
                                           colInvNo = grp.Key.colInvNo,
                                       }).ToList();

                        if (result2 != null && result2.Count > 0)
                            dr["colTInvoiceNumber"] = result2.Count;
                        else
                            dr["colTInvoiceNumber"] = 0;
                        #endregion
                        
                        dr["colTOrginvoiceValue"] = dgvGSTR27Other.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colOrginvoiceValue"].Value != null).Sum(x => x.Cells["colOrginvoiceValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colOrginvoiceValue"].Value)).ToString();
                        dr["colTTaxable"] = dgvGSTR27Other.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colTaxable"].Value != null).Sum(x => x.Cells["colTaxable"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colTaxable"].Value)).ToString();
                        dr["colTIGSTAmnt"] = dgvGSTR27Other.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colIGSTAmnt"].Value != null).Sum(x => x.Cells["colIGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIGSTAmnt"].Value)).ToString();
                        dr["colTCGSTAmnt"] = dgvGSTR27Other.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCGSTAmnt"].Value != null).Sum(x => x.Cells["colCGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCGSTAmnt"].Value)).ToString();
                        dr["colTSGSTAmnt"] = dgvGSTR27Other.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colSGSTAmnt"].Value != null).Sum(x => x.Cells["colSGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colSGSTAmnt"].Value)).ToString();
                        dr["colTCESSAmnt"] = dgvGSTR27Other.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCessAmnt"].Value != null).Sum(x => x.Cells["colCessAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCessAmnt"].Value)).ToString();

                        // ADD DATAROW TO DATATABLE
                        dtTotal.Rows.Add(dr);
                        dtTotal.AcceptChanges();

                        // ASSIGN DATATABLE TO GRID
                        dgvGSTR27OtherTotal.DataSource = dtTotal;
                        #endregion
                    }
                    else if (dgvGSTR27OtherTotal.Rows.Count == 1)
                    {
                        #region IF TOTAL GRID HAVING ONLY ONE RECORDS

                        // CALCULATE TOTAL ONLY SPECIFIC COLUMN
                        foreach (var item in colNo)
                        {
                            if (item == "colDbtCrdtNoteNo")
                            {
                                //dgvGSTR27OtherTotal.Rows[0].Cells["colTDbtCrdtNoteNo"].Value = dgvGSTR27Other.Rows.Cast<DataGridViewRow>().Where(x => Convert.ToString(x.Cells["colDbtCrdtNoteNo"].Value).Trim() != "").GroupBy(x => x.Cells["colDbtCrdtNoteNo"].Value).Select(x => x.First()).Distinct().Count();

                                #region colTDbtCrdtNoteNo
                                DataTable dt = new DataTable();
                                #region ADD DATATABLE COLUMN

                                // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                                foreach (DataGridViewColumn col in dgvGSTR27Other.Columns)
                                {
                                    dt.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }
                                #endregion
                                #region ASSIGN GRIDVIEW ROWS IN DATATABLE
                                object[] rowValue = new object[dt.Columns.Count];

                                foreach (DataGridViewRow drn in dgvGSTR27Other.Rows)
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
                                              where row.Field<string>("colDbtCrdtNoteNo") != "" && row.Field<string>("colGSTIN") != ""
                                              group row by new { colGSTIN = row.Field<string>("colGSTIN"), colInvNo = row.Field<string>("colDbtCrdtNoteNo") } into grp
                                              select new
                                              {
                                                  colGSTIN = grp.Key.colGSTIN,
                                                  colInvNo = grp.Key.colInvNo,
                                              }).ToList();

                                if (result != null && result.Count > 0)
                                    dgvGSTR27OtherTotal.Rows[0].Cells["colTDbtCrdtNoteNo"].Value = result.Count;
                                else
                                    dgvGSTR27OtherTotal.Rows[0].Cells["colTDbtCrdtNoteNo"].Value = 0;

                                var result1 = (from row in dt.AsEnumerable()
                                               where row.Field<string>("colOrgInvoiceNo") != "" && row.Field<string>("colGSTIN") != ""
                                               group row by new { colGSTIN = row.Field<string>("colGSTIN"), colInvNo = row.Field<string>("colOrgInvoiceNo") } into grp
                                              select new
                                              {
                                                  colGSTIN = grp.Key.colGSTIN,
                                                  colInvNo = grp.Key.colInvNo,
                                              }).ToList();

                                if (result1 != null && result1.Count > 0)
                                    dgvGSTR27OtherTotal.Rows[0].Cells["colTOrgInvoiceNo"].Value = result1.Count;
                                else
                                    dgvGSTR27OtherTotal.Rows[0].Cells["colTOrgInvoiceNo"].Value = 0;

                                var result2 = (from row in dt.AsEnumerable()
                                               where row.Field<string>("colInvoiceNumber") != "" && row.Field<string>("colGSTIN") != ""
                                               group row by new { colGSTIN = row.Field<string>("colGSTIN"), colInvNo = row.Field<string>("colInvoiceNumber") } into grp
                                               select new
                                               {
                                                   colGSTIN = grp.Key.colGSTIN,
                                                   colInvNo = grp.Key.colInvNo,
                                               }).ToList();

                                if (result2 != null && result2.Count > 0)
                                    dgvGSTR27OtherTotal.Rows[0].Cells["colTInvoiceNumber"].Value = result2.Count;
                                else
                                    dgvGSTR27OtherTotal.Rows[0].Cells["colTInvoiceNumber"].Value = 0;
                                #endregion
                            }                            
                            else if (item == "colOrginvoiceValue")
                                dgvGSTR27OtherTotal.Rows[0].Cells["colTOrginvoiceValue"].Value = dgvGSTR27Other.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colOrginvoiceValue"].Value != null).Sum(x => x.Cells["colOrginvoiceValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colOrginvoiceValue"].Value)).ToString();
                            else if (item == "colTaxable")
                                dgvGSTR27OtherTotal.Rows[0].Cells["colTTaxable"].Value = dgvGSTR27Other.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colTaxable"].Value != null).Sum(x => x.Cells["colTaxable"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colTaxable"].Value)).ToString();
                            else if (item == "colIGSTAmnt")
                                dgvGSTR27OtherTotal.Rows[0].Cells["colTIGSTAmnt"].Value = dgvGSTR27Other.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colIGSTAmnt"].Value != null).Sum(x => x.Cells["colIGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIGSTAmnt"].Value)).ToString();
                            else if (item == "colCGSTAmnt")
                                dgvGSTR27OtherTotal.Rows[0].Cells["colTCGSTAmnt"].Value = dgvGSTR27Other.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCGSTAmnt"].Value != null).Sum(x => x.Cells["colCGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCGSTAmnt"].Value)).ToString();
                            else if (item == "colSGSTAmnt")
                                dgvGSTR27OtherTotal.Rows[0].Cells["colTSGSTAmnt"].Value = dgvGSTR27Other.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colSGSTAmnt"].Value != null).Sum(x => x.Cells["colSGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colSGSTAmnt"].Value)).ToString();
                            else if (item == "colCessAmnt")
                                dgvGSTR27OtherTotal.Rows[0].Cells["colTCESSAmnt"].Value = dgvGSTR27Other.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCessAmnt"].Value != null).Sum(x => x.Cells["colCessAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCessAmnt"].Value)).ToString();
                        }
                        #endregion
                    }

                    // SET TOTAL GRID HEIGHT ROW
                    dgvGSTR27OtherTotal.Rows[0].Height = 30;
                    dgvGSTR27OtherTotal.Rows[0].Cells[0].Value = "TOTAL";
                }
                else
                {
                    // CHECK IF TOTAL GRID HAVING RECORD

                    if (dgvGSTR27OtherTotal.Rows.Count >= 0)
                    {
                        #region IF THERE ARE NO RECORDS IN MAIN GRID THEN ASSIGN BLANK DATATABLE TO TOTAL GRID
                        DataTable dtTotal = new DataTable();
                        foreach (DataGridViewColumn col in dgvGSTR27OtherTotal.Columns)
                        {
                            dtTotal.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;
                        }
                        dgvGSTR27OtherTotal.DataSource = dtTotal;
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

        private void dgvGSTR27_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                pbGSTR1.Visible = true;

                if (e.KeyCode == Keys.Delete)
                {
                    #region DELETE SELECTED CELLS
                    try
                    {
                        if (dgvGSTR27Other.Rows.Count > 0)
                        {
                            foreach (DataGridViewCell oneCell in dgvGSTR27Other.SelectedCells)
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
                        pbGSTR1.Visible = false;
                        MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                        StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                        errorWriter.Write(errorMessage);
                        errorWriter.Close();
                        return;
                    }
                    #endregion

                    string[] colNo = { "colDbtCrdtNoteNo", "colOrgInvoiceNo", "colInvoiceNumber", "colOrginvoiceValue", "colTaxable", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };
                    GetTotal(colNo);
                }
                if (e.KeyCode == Keys.V)
                {
                    #region PAST FROM EXCELL SHEET

                    string s = Clipboard.GetText();
                    string[] lines = s.Split('\n');
                    int iRow = 0, iCol = 0;

                    #region PAST ON SELECTED CELLS ONLY
                    if (dgvGSTR27Other.RowCount > 0) // IF GRID IS NOT NULL AND PAST ONLY ON SELECTED CELLS
                    {
                        foreach (DataGridViewCell oneCell in dgvGSTR27Other.SelectedCells)
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
                            DisableControls(dgvGSTR27Other);

                            gRowNo = dgvGSTR27Other.Rows.Count - 1;
                            int no = tmp;

                            if (iRow > gRowNo - 1)
                            {
                                DataTable dtDGV = new DataTable();
                                //dtDGV = dgvGSTR27Other.DataSource as DataTable;

                                #region ADD DATATABLE COLUMN

                                // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                                foreach (DataGridViewColumn col in dgvGSTR27Other.Columns)
                                {
                                    dtDGV.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }
                                #endregion

                                #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                                // CREATE OBJECT ARRAY TO STORE ONE ROW DATA OF MAIN GRID
                                object[] rowValue = new object[dtDGV.Columns.Count];

                                foreach (DataGridViewRow dr in dgvGSTR27Other.Rows)
                                {
                                    if (dr.Index != dgvGSTR27Other.Rows.Count - 1) // DON'T ADD LAST ROW
                                    {
                                        // SET CHECK BOX VALUE
                                        rowValue[0] = "False";
                                        for (int i = 1; i < dr.Cells.Count; i++)
                                        {
                                            if (dtDGV.Columns[i].ColumnName == "colTypeOfNote")
                                            {
                                                if (Convert.ToString(dr.Cells[i].Value).Trim() == "Credit note" || Convert.ToString(dr.Cells[i].Value).Trim() == "Debit note" || Convert.ToString(dr.Cells[i].Value).Trim() == "Refund Voucher")
                                                    rowValue[i] = Convert.ToString(dr.Cells[i].Value);
                                                else
                                                    rowValue[i] = "";
                                            }
                                            else if (dtDGV.Columns[i].ColumnName == "colRegime")
                                            {
                                                if (Convert.ToString(dr.Cells[i].Value).Trim().ToLower() == "yes" || Convert.ToString(dr.Cells[i].Value).Trim().ToLower() == "no")
                                                    rowValue[i] = Convert.ToString(dr.Cells[i].Value);
                                                else
                                                    rowValue[i] = "";
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
                                // PASTE DATA TO EXISTING ROW IN GRID
                                if (line.Length > 0)
                                {
                                    #region ROW PASTE
                                    // SPLIT ONE COPIED ROW DATA TO ARRAY
                                    string[] sCells = line.Split('\t');

                                    for (int i = 0; i < sCells.GetLength(0); ++i)
                                    {
                                        // CHECK GRID COLUMN COUNT
                                        if (iCol + i < this.dgvGSTR27Other.ColumnCount && i < 17)
                                        {
                                            // SKIP CHECK BOX COLUMN AND SEQUANCE COLUMN TO PASTE DATA
                                            if (iCol == 0)
                                                oCell = dgvGSTR27Other[iCol + i + 2, iRow];
                                            else if (iCol == 1)
                                                oCell = dgvGSTR27Other[iCol + i + 1, iRow];
                                            else
                                                oCell = dgvGSTR27Other[iCol + i, iRow];

                                            sCells[i] = sCells[i].Trim().Replace(",", "");
                                            if (oCell.ColumnIndex != 0)
                                            {
                                                if (dgvGSTR27Other.Columns[oCell.ColumnIndex].Name != "colChk" && dgvGSTR27Other.Columns[oCell.ColumnIndex].Name != "colSequence")
                                                {
                                                    #region VALIDATION
                                                    if (sCells[i].ToString().Trim() == "") { dgvGSTR27Other.Rows[iRow].Cells[oCell.ColumnIndex].Value = DBNull.Value; }
                                                    else
                                                    {
                                                        if (oCell.ColumnIndex >= 2 && oCell.ColumnIndex <= 18)
                                                        {
                                                            if (chkCellValue(sCells[i].Trim(), dgvGSTR27Other.Columns[oCell.ColumnIndex].Name))
                                                                dgvGSTR27Other.Rows[iRow].Cells[oCell.ColumnIndex].Value = sCells[i].Trim();
                                                            else
                                                            {
                                                                if (dgvGSTR27Other.Columns[oCell.ColumnIndex].Name == "colRegime")
                                                                    dgvGSTR27Other.Rows[iRow].Cells[oCell.ColumnIndex].Value = "No";
                                                                else
                                                                    dgvGSTR27Other.Rows[iRow].Cells[oCell.ColumnIndex].Value = "";
                                                            }
                                                        }
                                                        else { dgvGSTR27Other.Rows[iRow].Cells[oCell.ColumnIndex].Value = sCells[i].Trim(); }
                                                    }
                                                    #endregion
                                                }
                                            }
                                            else
                                            {
                                                if (iCol > i)
                                                {
                                                    for (int j = oCell.ColumnIndex; j < dgvGSTR27Other.Columns.Count; j++)
                                                    {
                                                        #region VALIDATION
                                                        if (sCells[i].ToString().Trim() == "") { dgvGSTR27Other.Rows[iRow].Cells[j].Value = DBNull.Value; }
                                                        else
                                                        {
                                                            if (j >= 2 && j <= 18)
                                                            {
                                                                if (chkCellValue(sCells[i].Trim(), dgvGSTR27Other.Columns[j].Name))
                                                                    dgvGSTR27Other.Rows[iRow].Cells[j].Value = sCells[i].Trim();
                                                                else
                                                                {
                                                                    if (dgvGSTR27Other.Columns[j].Name == "colRegime")
                                                                        dgvGSTR27Other.Rows[iRow].Cells[j].Value = "No";
                                                                    else
                                                                        dgvGSTR27Other.Rows[iRow].Cells[j].Value = "";
                                                                }
                                                            }
                                                            else { dgvGSTR27Other.Rows[iRow].Cells[j].Value = sCells[i].Trim(); }
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
                                                    for (int j = oCell.ColumnIndex; j < dgvGSTR27Other.Columns.Count; j++)
                                                    {
                                                        #region VALIDATION
                                                        if (sCells[i].ToString().Trim() == "") { dgvGSTR27Other.Rows[iRow].Cells[j].Value = DBNull.Value; }
                                                        else
                                                        {
                                                            if (j >= 2 && j <= 18)
                                                            {
                                                                if (chkCellValue(sCells[i].Trim(), dgvGSTR27Other.Columns[j].Name))
                                                                    dgvGSTR27Other.Rows[iRow].Cells[j].Value = sCells[i].Trim();
                                                                else
                                                                {
                                                                    if (dgvGSTR27Other.Columns[j].Name == "colRegime")
                                                                        dgvGSTR27Other.Rows[iRow].Cells[j].Value = "No";
                                                                    else
                                                                        dgvGSTR27Other.Rows[iRow].Cells[j].Value = "";
                                                                }
                                                            }
                                                            else { dgvGSTR27Other.Rows[iRow].Cells[j].Value = sCells[i].Trim(); }
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

                    // SEQUNCING GRID RECORDS
                    for (int i = 0; i < dgvGSTR27Other.Rows.Count - 1; i++)
                    {
                        dgvGSTR27Other.Rows[i].Cells["colSequence"].Value = i + 1;
                    }
                    #endregion

                    EnableControls(dgvGSTR27Other);
                }
                if ((e.Control && (e.KeyCode == Keys.A || e.KeyCode == Keys.Subtract)) || (e.KeyCode == Keys.Space && Control.ModifierKeys == Keys.Shift) || (e.Alt && e.KeyCode == Keys.F4))
                {
                    e.Handled = true;
                }

                pbGSTR1.Visible = false;
            }
            catch (Exception ex)
            {
                EnableControls(dgvGSTR27Other);
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
                DisableControls(dgvGSTR27Other);

                #region SET DATATABLE
                int cnt = 0, colNo = 0;

                // ASSIGN GRID DATA TO DATATABLE
                DataTable dt = dtDGV;

                if (dt == null)
                {
                    // IF NO RECORD IN GRID THEN CREATE NEW DATATABLE
                    dt = new DataTable();

                    // ADD COLUMN AS PAR MAIN GRID AND SET DATA ACCESS PROPERTY
                    foreach (DataGridViewColumn col in dgvGSTR27Other.Columns)
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
                                if (iCol + i < this.dgvGSTR27Other.ColumnCount && colNo < 18)
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
                                                if (colNo >= 2 && colNo <= 18)
                                                {
                                                    if (chkCellValue(sCells[i].Trim(), dt.Columns[colNo].ColumnName))
                                                        dt.Rows[dt.Rows.Count - 1][colNo] = sCells[i].Trim();
                                                    else
                                                    {
                                                        if (dgvGSTR27Other.Columns[colNo].Name == "colRegime")
                                                            dt.Rows[dt.Rows.Count - 1][colNo] = "No";
                                                        else
                                                            dt.Rows[dt.Rows.Count - 1][colNo] = "";
                                                    }
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
                                            for (int j = colNo; j < dgvGSTR27Other.Columns.Count; j++)
                                            {
                                                #region VALIDATION
                                                if (sCells[i].ToString().Trim() == "") { dt.Rows[dt.Rows.Count - 1][j] = DBNull.Value; }
                                                else
                                                {
                                                    if (j >= 2 && j <= 18)
                                                    {
                                                        if (chkCellValue(sCells[i].Trim(), dt.Columns[j].ColumnName))
                                                            dt.Rows[dt.Rows.Count - 1][j] = sCells[i].Trim();
                                                        else
                                                        {
                                                            if (dgvGSTR27Other.Columns[j].Name == "colRegime")
                                                                dt.Rows[dt.Rows.Count - 1][j] = "No";
                                                            else
                                                                dt.Rows[dt.Rows.Count - 1][j] = "";
                                                        }
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
                                            for (int j = colNo; j < dgvGSTR27Other.Columns.Count; j++)
                                            {
                                                #region VALIDATION
                                                if (sCells[i].ToString().Trim() == "") { dt.Rows[dt.Rows.Count - 1][j] = DBNull.Value; }
                                                else
                                                {
                                                    if (j >= 2 && j <= 18)
                                                    {
                                                        if (chkCellValue(sCells[i].Trim(), dt.Columns[j].ColumnName))
                                                            dt.Rows[dt.Rows.Count - 1][j] = sCells[i].Trim();
                                                        else
                                                        {
                                                            if (dgvGSTR27Other.Columns[j].Name == "colRegime")
                                                                dt.Rows[dt.Rows.Count - 1][j] = "No";
                                                            else
                                                                dt.Rows[dt.Rows.Count - 1][j] = "";
                                                        }
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
                    dgvGSTR27Other.DataSource = dt;

                // TOTAL CALCULATION
                string[] colGroup = { "colDbtCrdtNoteNo", "colOrgInvoiceNo", "colInvoiceNumber", "colOrginvoiceValue", "colTaxable", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };
                GetTotal(colGroup);

                pbGSTR1.Visible = false;

                EnableControls(dgvGSTR27Other);

                #endregion
            }
            catch (Exception ex)
            {
                EnableControls(dgvGSTR27Other);
                pbGSTR1.Visible = false;
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
                string _str = "", sj = "";

                dgvGSTR27Other.CurrentCell = dgvGSTR27Other.Rows[0].Cells[0];
                dgvGSTR27Other.AllowUserToAddRows = false;
                pbGSTR1.Visible = true;

                #region GSTN Number
                List<DataGridViewRow> list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsGSTN(Convert.ToString(x.Cells["colGSTIN"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR27Other.Rows[list[i].Cells["colGSTIN"].RowIndex].Cells["colGSTIN"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper GSTN Number.\n";
                }
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsGSTN(Convert.ToString(x.Cells["colGSTIN"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR27Other.Rows[list[i].Cells["colGSTIN"].RowIndex].Cells["colGSTIN"].Style.BackColor = Color.White;
                }
                #endregion

                #region Credit And Debit
                list = null;
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => "c" != Convert.ToString(x.Cells["colTypeOfNote"].Value).Trim() && "Credit note" != Convert.ToString(x.Cells["colTypeOfNote"].Value).Trim() && "Debit note" != Convert.ToString(x.Cells["colTypeOfNote"].Value).Trim() && "Refund Voucher" != Convert.ToString(x.Cells["colTypeOfNote"].Value))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR27Other.Rows[list[i].Cells["colTypeOfNote"].RowIndex].Cells["colTypeOfNote"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper type of notes.\n";
                }
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => "c" == Convert.ToString(x.Cells["colTypeOfNote"].Value).ToLower() || "credit" == Convert.ToString(x.Cells["colTypeOfNote"].Value).ToLower() || "d" == Convert.ToString(x.Cells["colTypeOfNote"].Value).ToLower() || "debit" == Convert.ToString(x.Cells["colTypeOfNote"].Value).ToLower())
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR27Other.Rows[list[i].Cells["colTypeOfNote"].RowIndex].Cells["colTypeOfNote"].Style.BackColor = Color.White;
                }
                #endregion

                #region Pre GST Regime Dr./ Cr. Notes
                sj = "colRegime";
                list = null;
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => "yes" != Convert.ToString(x.Cells[sj].Value).Trim().ToLower() && "no" != Convert.ToString(x.Cells[sj].Value).Trim().ToLower())
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR27Other.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper Pre GST Regime Dr./Cr. Notes.\n";
                }
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => "yes" == Convert.ToString(x.Cells[sj].Value).ToLower() || "no" == Convert.ToString(x.Cells[sj].Value).ToLower())
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR27Other.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.White;
                }
                #endregion

                #region Debit Note/ Credit Note Date
                sj = "colDbtCrdtNoteDate";
                list = null;//dd-MM-yyyy
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsDate(Convert.ToString(x.Cells[sj].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR27Other.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper Debit Note/ Credit Note Date.\n";
                }
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsDate(Convert.ToString(x.Cells[sj].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR27Other.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.White;
                }
                #endregion

                #region Invoice Date
                sj = "colOrginvoiceDate";
                list = null;//dd-MM-yyyy
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsDate(Convert.ToString(x.Cells[sj].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR27Other.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper Invoice Date.\n";
                }
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsDate(Convert.ToString(x.Cells[sj].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR27Other.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.White;
                }
                #endregion

                #region Invoice Value
                sj = "colOrginvoiceValue";
                list = null;
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsNumber(Convert.ToString(x.Cells[sj].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR27Other.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper Original invoice value.\n";
                }
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsNumber(Convert.ToString(x.Cells[sj].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR27Other.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.White;
                }
                #endregion

                #region Rate
                sj = "colRate";
                list = null;
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsNumber(Convert.ToString(x.Cells[sj].Value)) || Convert.ToDouble(x.Cells[sj].Value) > 100)
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR27Other.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper Rate.\n";
                }
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsNumber(Convert.ToString(x.Cells[sj].Value)) && Convert.ToDouble(x.Cells[sj].Value) <= 100)
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR27Other.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.White;
                }
                #endregion

                #region IGST Amount
                //sj = "colIGSTAmnt";
                //list = null;
                //list = dgvGSTR27Other.Rows
                //       .OfType<DataGridViewRow>()
                //       .Where(x => true != Utility.IsNumber(Convert.ToString(x.Cells[sj].Value)))
                //       .Select(x => x)
                //       .ToList();
                //if (list.Count > 0)
                //{
                //    for (int i = 0; i < list.Count; i++)
                //    {
                //        dgvGSTR27Other.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.Red;
                //    }
                //    _cnt += 1;
                //    _str += _cnt + ") Please enter proper IGST Amount.\n";
                //}
                //list = dgvGSTR27Other.Rows
                //       .OfType<DataGridViewRow>()
                //       .Where(x => true == Utility.IsNumber(Convert.ToString(x.Cells[sj].Value)))
                //       .Select(x => x)
                //       .ToList();
                //for (int i = 0; i < list.Count; i++)
                //{
                //    dgvGSTR27Other.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.White;
                //}
                #endregion

                #region Integrated tax Amount

                string gstin = CommonHelper.StateName;
                string result = gstin;
                list = dgvGSTR27Other.Rows
                                    .OfType<DataGridViewRow>()
                                    .ToList();
                for (int j = 0; j < list.Count; j++)
                {
                    string pgst = dgvGSTR27Other.Rows[list[j].Cells["colPlaceOfSupply"].RowIndex].Cells["colPlaceOfSupply"].Value.ToString();
                    string result1 = pgst;
                    if (result != result1)
                    {
                        if (dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value.ToString().Trim() == "")
                        {
                            _cnt += 1;
                            _str += _cnt + ") Please enter proper Integrated tax Amount.\n";
                            dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.Red;
                        }
                        else
                        { dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.White; }
                    }
                    else if (dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value.ToString().Trim() != "")
                    {
                        _cnt += 1;
                        _str += _cnt + ") Please enter proper Integrated tax Amount.\n";
                        dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.Red;
                    }
                    else if (dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value.ToString().Trim() == "")
                    {
                        dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.White;
                    }
                }
                #endregion

                #region Central tax Amount



                for (int j = 0; j < list.Count; j++)
                {
                    string pgst = dgvGSTR27Other.Rows[list[j].Cells["colPlaceOfSupply"].RowIndex].Cells["colPlaceOfSupply"].Value.ToString();
                    string result1 = pgst;

                    if (result == result1)
                    {
                        if (dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value.ToString().Trim() == "")
                        {
                            _cnt += 1;
                            _str += _cnt + ") Please enter proper Central tax Amount.\n";
                            dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.Red;
                        }
                        else
                        {
                            dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.White;
                        }
                    }
                    else if (dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value.ToString().Trim() != "")
                    {
                        _cnt += 1;
                        _str += _cnt + ") Please enter proper Central tax Amount.\n";
                        dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.Red;
                    }
                    else if (dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value.ToString().Trim() == "")
                    {
                        dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.White;
                    }
                }
                #endregion

                #region State / UT tax Amount

                for (int j = 0; j < list.Count; j++)
                {
                    string pgst = dgvGSTR27Other.Rows[list[j].Cells["colPlaceOfSupply"].RowIndex].Cells["colPlaceOfSupply"].Value.ToString();
                    string result1 = pgst;

                    if (result == result1)
                    {
                        if (dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value.ToString().Trim() == "")
                        {
                            _cnt += 1;
                            _str += _cnt + ") Please enter proper State/UT tax Amount.\n";
                            dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.Red;
                        }
                        else
                        { dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.White; }
                    }
                    else if (dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value.ToString().Trim() != "")
                    {
                        _cnt += 1;
                        _str += _cnt + ") Please enter proper State/UT tax Amount.\n";
                        dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.Red;
                    }
                    else if (dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value.ToString().Trim() == "")
                    {
                        dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.White;
                    }
                }
                #endregion

                #region CESS Amount
                //sj = "colCessAmnt";
                //list = null;

                //if (list.Count > 0)
                //{
                //    for (int i = 0; i < list.Count; i++)
                //    {
                //        dgvGSTR27Other.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.Red;
                //    }
                //    _cnt += 1;
                //    _str += _cnt + ") Please enter proper SGST Amount.\n";
                //}
                //list = dgvGSTR27Other.Rows
                //       .OfType<DataGridViewRow>()
                //       .Where(x => true == Utility.IsNumber(Convert.ToString(x.Cells[sj].Value)))
                //       .Select(x => x)
                //       .ToList();
                //for (int i = 0; i < list.Count; i++)
                //{
                //    dgvGSTR27Other.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.White;
                //}
                #endregion

                #region POS
                list = null;
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsValidStateName(Convert.ToString(x.Cells["colPlaceOfSupply"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR27Other.Rows[list[i].Cells["colPlaceOfSupply"].RowIndex].Cells["colPlaceOfSupply"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper place of supply.\n";
                }
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsNumber(Convert.ToString(x.Cells["colPlaceOfSupply"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR27Other.Rows[list[i].Cells["colPlaceOfSupply"].RowIndex].Cells["colPlaceOfSupply"].Style.BackColor = Color.White;
                }
                #endregion

                dgvGSTR27Other.AllowUserToAddRows = true;
                pbGSTR1.Visible = false;

                if (_str != "")
                {
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
                    if (cNo == "colGSTIN") //GSTN
                    {
                        if (Utility.IsGSTN(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colTypeOfNote") //Debit and Credit
                    {
                        if (Utility.CDNTypesofNote(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colRegime")
                    {
                        if (cellValue == "Yes" || cellValue == "No")
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colDbtCrdtNoteDate" || cNo == "colOrginvoiceDate") // Date
                    {
                        if (Utility.IsDate(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colIssue")
                    {
                        if (chkVal(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colOrginvoiceValue" || cNo == "colTaxable" || cNo == "colIGSTAmnt" || cNo == "colCGSTAmnt" || cNo == "colSGSTAmnt" || cNo == "colCessAmnt") // value
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
                    else if (cNo == "colPlaceOfSupply")
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
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
                return false;
            }
        }

        private void dgvGSTR27Other_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string cNo = dgvGSTR27Other.Columns[e.ColumnIndex].Name;

                if (e.RowIndex >= 0)
                {
                    if (cNo == "colRate" || cNo == "colDbtCrdtNoteDate" || cNo == "colOrginvoiceDate" || cNo == "colTypeOfNote" || cNo == "colIssue")
                    {
                        if (!chkCellValue(Convert.ToString(dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                            dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value = "";

                        if (cNo == "colRate")
                        {
                            if (Convert.ToString(dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value).Trim() != "")
                            {
                                dgvGSTR27Other.CellValueChanged -= dgvGSTR27Other_CellValueChanged;
                                dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value = Math.Round(Convert.ToDecimal(dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value), 2, MidpointRounding.AwayFromZero);
                                dgvGSTR27Other.CellValueChanged += dgvGSTR27Other_CellValueChanged;
                            }
                        }
                    }
                    else if (cNo == "colDbtCrdtNoteNo" || cNo == "colOrgInvoiceNo" || cNo == "colOrginvoiceValue" || cNo == "colTaxable" || cNo == "colIGSTAmnt" || cNo == "colCGSTAmnt" || cNo == "colSGSTAmnt" || cNo == "colCessAmnt") // value
                    {
                        if (chkCellValue(Convert.ToString(dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                        {
                            if (cNo != "colDbtCrdtNoteNo" && cNo != "colOrgInvoiceNo")
                            {
                                if (Convert.ToString(dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value).Trim() != "")
                                {
                                    dgvGSTR27Other.CellValueChanged -= dgvGSTR27Other_CellValueChanged;
                                    dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value = Math.Round(Convert.ToDecimal(dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value), 2, MidpointRounding.AwayFromZero);
                                    dgvGSTR27Other.CellValueChanged += dgvGSTR27Other_CellValueChanged;
                                }
                            }

                            string[] colNo = { cNo };
                            GetTotal(colNo);
                        }
                        else { dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value = ""; }
                    }
                    else if (cNo == "colTypeOfNote")
                    {
                        if (!chkCellValue(Convert.ToString(dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                            dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value = Utility.StrCDNTypesofNote(Convert.ToString(dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value));

                        string[] colNo = { "colDbtCrdtNoteNo", "colOrgInvoiceNo", "colInvoiceNumber", "colOrginvoiceValue", "colTaxable", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };
                        GetTotal(colNo);
                    }
                    else if (cNo == "colRegime")
                    {
                        if (!chkCellValue(Convert.ToString(dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                            dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value = Utility.StrCDNReasonissuing(Convert.ToString(dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value));
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
                //if (CommonHelper.StatusIndex == 0)
                //{
                //    MessageBox.Show("Please Select File Status!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}

                pbGSTR1.Visible = true;

                //For text clear before save
                txtSearch.Text = "";

                #region ADD DATATABLE COLUMN

                // CREATE DATATABLE TO STORE MAIN GRID DATA
                DataTable dt = new DataTable();

                // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                foreach (DataGridViewColumn col in dgvGSTR27Other.Columns)
                {
                    dt.Columns.Add(col.Name.ToString());
                }

                // ADD DATATABLE COLUMN TO STORE FILE STATUS
                dt.Columns.Add("colFileStatus");

                #endregion

                #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                // CREATE OBJECT ARRAY TO STORE ONE ROW DATA OF MAIN GRID
                object[] rowValue = new object[dt.Columns.Count];

                foreach (DataGridViewRow dr in dgvGSTR27Other.Rows)
                {
                    //if (dr.Index != dgvGSTR27Other.Rows.Count - 1) // DON'T ADD LAST ROW
                    //{
                    for (int i = 0; i < dr.Cells.Count; i++)
                    {
                        rowValue[i] = Convert.ToString(dr.Cells[i].Value);
                    }

                    // ASSIGN FILE STATUS VALUE WITH EACH GRID ROW
                    rowValue[dr.Cells.Count] = Convert.ToString(CommonHelper.StatusText);

                    // ADD ARRAY OF GRID ROW VALUE TO DATATABLE AS ROW
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

                // CHECK THERE ARE RECORDS IN GRID
                if (dt != null && dt.Rows.Count > 0)
                {
                    #region FIRST DELETE OLD DATA FROM DATABASE
                    Query = "Delete from SPQR2ACND where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                    _Result = objGSTR2.IUDData(Query);
                    if (_Result != 1)
                    {
                        // ERROR OCCURS WHILE DELETING DATA
                        pbGSTR1.Visible = false;
                        MessageBox.Show("System error.\nPlease try after sometime!", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    #endregion

                    // QUERY FIRE TO SAVE RECORDS TO DATABASE
                    _Result = objGSTR2.GSTR2A_CNDBulkEntry(dt, Convert.ToString(CommonHelper.StatusText));

                    if (_Result == 1)
                    {
                        // TOTAL CALCULATION
                        string[] colGroup = { "colDbtCrdtNoteNo", "colOrgInvoiceNo", "colInvoiceNumber", "colOrginvoiceValue", "colTaxable", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };
                        GetTotal(colGroup);

                        #region ADD DATATABLE COLUMN

                        // CREATE DATATABLE TO STORE MAIN GRID DATA
                        dt = new DataTable();

                        // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                        foreach (DataGridViewColumn col in dgvGSTR27Other.Columns)
                        {
                            dt.Columns.Add(col.Name.ToString());
                        }

                        // ADD DATATABLE COLUMN TO STORE FILE STATUS
                        dt.Columns.Add("colFileStatus");

                        #endregion

                        #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                        // CREATE OBJECT ARRAY TO STORE ONE ROW DATA OF MAIN GRID
                        object[] rowVal = new object[dt.Columns.Count];

                        if (dgvGSTR27OtherTotal.Rows.Count == 1)
                        {
                            foreach (DataGridViewRow dr in dgvGSTR27OtherTotal.Rows)
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

                        // REMOVE FIRST COLUMM (FIELD ID)
                        dt.Columns.Remove(dt.Columns[0]);
                        dt.AcceptChanges();
                        #endregion

                        _Result = objGSTR2.GSTR2A_CNDBulkEntry(dt, "Total");

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
                            pbGSTR1.Visible = false;
                            MessageBox.Show("System error.\nPlease try after sometime!", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        // IF ERRORS OCCURS WHILE INSERTING DATA TO DATABASE
                        pbGSTR1.Visible = false;
                        MessageBox.Show("System error.\nPlease try after sometime!", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    #region DELETE ALL OLD RECORD IF THERE ARE NO RECORDS PRESENT IN GRID
                    Query = "Delete from SPQR2ACND where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";

                    // FIRE QUEARY TO DELETE RECORDS
                    _Result = objGSTR2.IUDData(Query);

                    if (_Result == 1)
                    {
                        // IF RECORDS DELETED FROM DATABASE
                        MessageBox.Show("Record Successfully Deleted!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // MAKE FILE STATUS BLANK
                        ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(0);

                        // TOTAL CALCULATION
                        string[] colNo = { "colDbtCrdtNoteNo", "colOrgInvoiceNo", "colInvoiceNumber", "colOrginvoiceValue", "colTaxable", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };
                        GetTotal(colNo);
                    }
                    else
                    {
                        // IF ERRORS OCCURS WHILE DELETING RECORD FROM THE DATABASE
                        pbGSTR1.Visible = false;
                        MessageBox.Show("System error.\nPlease try after sometime!", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (dgvGSTR27Other.CurrentCell.RowIndex == 0 && dgvGSTR27Other.CurrentCell.ColumnIndex == 0)
                {
                    dgvGSTR27Other.CurrentCell = dgvGSTR27Other.Rows[0].Cells[1];
                }
                else { dgvGSTR27Other.CurrentCell = dgvGSTR27Other.Rows[0].Cells[0]; }


                // CREATE FLAG FRO DELETE ROWS
                Boolean flgChk = false; Boolean flgSelect = false;

                // CREATE OBJECT OF SELECTED ROW TO DELETE
                List<DataGridViewRow> toDelete = new List<DataGridViewRow>();

                // CHECK THERE ARE RECORD PRESENT IN GRID
                if (dgvGSTR27Other.Rows.Count > 1)
                {
                    // FLAG TRUE IF CHECK ALL SELECTED
                    if (ckboxHeader.Checked)
                        flgChk = true;

                    #region ADD SELECTED ROW TO OBJECT FOR DELETE
                    for (int i = 0; i < dgvGSTR27Other.Rows.Count - 1; i++)
                    {
                        if (dgvGSTR27Other[0, i].Value != null && dgvGSTR27Other[0, i].Value.ToString() != "")
                        {
                            if (Convert.ToBoolean(dgvGSTR27Other[0, i].Value) == true)
                            {
                                // ADD ROW TO OBJECT IF ROW IS SELECTED
                                flgSelect = true;
                                toDelete.Add(dgvGSTR27Other.Rows[i]);
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
                                foreach (DataGridViewColumn col in dgvGSTR27Other.Columns)
                                {
                                    dt.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }

                                // ASSIGN BLANK DATATABLE TO GRID
                                dgvGSTR27Other.DataSource = dt;
                            }
                            else if (flgSelect == true)
                            {
                                // DELETE SELECTED ROW
                                foreach (DataGridViewRow row in toDelete)
                                {
                                    dgvGSTR27Other.Rows.RemoveAt(row.Index);
                                    Application.DoEvents();
                                }
                            }

                            // SEQUANCING MAIN GRID RECORDS
                            for (int i = 0; i < dgvGSTR27Other.Rows.Count - 1; i++)
                            {
                                dgvGSTR27Other.Rows[i].Cells["colSequence"].Value = Convert.ToString(i + 1);
                            }

                            if (dgvGSTR27Other.Rows.Count == 1)
                            {
                                // IF THERE ARE NO RECORDS IN MAIN GRID THENE ASSIGN BLANK DATATABLE TO TOTAL GRID
                                DataTable dtTotal = new DataTable();
                                foreach (DataGridViewColumn col in dgvGSTR27OtherTotal.Columns)
                                {
                                    dtTotal.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }
                                dgvGSTR27OtherTotal.DataSource = dtTotal;
                            }

                            // SET CONTROL PROPERTY AFTER ROW DELETION
                            ckboxHeader.Checked = false;
                            dgvGSTR27Other.Columns[0].HeaderText = "Check All";
                            #endregion
                        }
                    }

                    pbGSTR1.Visible = false;

                    // TOTAL CALCULATION
                    string[] colNo = { "colDbtCrdtNoteNo", "colOrgInvoiceNo", "colInvoiceNumber", "colOrginvoiceValue", "colTaxable", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };
                    GetTotal(colNo);
                }
                else
                {
                    // IF THERE ARE NO RECORD TO DELETE
                    ckboxHeader.Checked = false;
                    dgvGSTR27Other.Columns[0].HeaderText = "Check All";
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
                        foreach (DataGridViewColumn col in dgvGSTR27Other.Columns)
                        {
                            dt.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;
                        }
                        #endregion

                        #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                        // CREATE OBJECT ARRAY TO STORE ONE ROW DATA OF MAIN GRID
                        object[] rowValue = new object[dt.Columns.Count];

                        foreach (DataGridViewRow dr in dgvGSTR27Other.Rows)
                        {
                            if (dr.Index != dgvGSTR27Other.Rows.Count - 1) // DON'T ADD LAST ROW
                            {
                                // SET CHECK BOX VALUE
                                rowValue[0] = "False";
                                for (int i = 1; i < dr.Cells.Count; i++)
                                {
                                    if (dt.Columns[i].ColumnName == "colTypeOfNote")
                                    {
                                        if (Convert.ToString(dr.Cells[i].Value).Trim() == "Credit note" || Convert.ToString(dr.Cells[i].Value).Trim() == "Debit note" || Convert.ToString(dr.Cells[i].Value).Trim() == "Refund Voucher")
                                            rowValue[i] = Convert.ToString(dr.Cells[i].Value);
                                        else
                                            rowValue[i] = "";
                                    }
                                    else if (dt.Columns[i].ColumnName == "colRegime")
                                    {
                                        if (Convert.ToString(dr.Cells[i].Value).Trim().ToLower() == "yes" || Convert.ToString(dr.Cells[i].Value).Trim().ToLower() == "no")
                                            rowValue[i] = Convert.ToString(dr.Cells[i].Value);
                                        else
                                            rowValue[i] = "";
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
                                DisableControls(dgvGSTR27Other);

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
                                foreach (DataGridViewColumn col in dgvGSTR27Other.Columns)
                                {
                                    dt.Columns[col.Index].ColumnName = col.Name.ToString();
                                    col.DataPropertyName = col.Name;
                                }
                                #endregion

                                //ASSIGN DATATABLE TO DATAGRID
                                dgvGSTR27Other.DataSource = dt;

                                // ENABLE MAIN GRID
                                EnableControls(dgvGSTR27Other);
                            }
                            else
                            {
                                // IF THERE ARE NO RECORDS IN MAIN GRID

                                if (dtExcel != null && dtExcel.Rows.Count > 0)
                                {
                                    // IF THERE ARE DATA IN IMPORTED EXCEL FILE

                                    // DISABLE MAIN GRID
                                    DisableControls(dgvGSTR27Other);

                                    #region RENAME DATATABLE COLUMN NAME AS PAR MAIN GRID
                                    foreach (DataGridViewColumn col in dgvGSTR27Other.Columns)
                                    {
                                        dtExcel.Columns[col.Index].ColumnName = col.Name.ToString();
                                        col.DataPropertyName = col.Name;
                                    }
                                    dtExcel.AcceptChanges();

                                    // ASSIGN DATATALE TO GRID
                                    dgvGSTR27Other.DataSource = dtExcel;

                                    // ENABLE MAIN GRID
                                    EnableControls(dgvGSTR27Other);
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
                            string[] colNo = { "colDbtCrdtNoteNo", "colOrgInvoiceNo", "colInvoiceNumber", "colOrginvoiceValue", "colTaxable", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };
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
                EnableControls(dgvGSTR27Other);
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
                        OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [cdn$]", con);
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
                        for (int i = 2; i < dgvGSTR27Other.Columns.Count; i++)
                        {
                            Boolean flg = false;
                            for (int j = 0; j < dtexcel.Columns.Count; j++)
                            {
                                // CHECK GRID COLUMN IS PRESENT OR NOT IN IMPORTED EXCEL
                                if (dgvGSTR27Other.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim().Length >= 40)
                                {
                                    string piece = dgvGSTR27Other.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim().Substring(0, 40);
                                    string piece1 = string.Empty;

                                    if (dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim().Length >= 40)
                                        piece1 = dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim().Substring(0, 40);
                                    else
                                        piece1 = dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim();

                                    if (piece == piece1)
                                    {
                                        // if grid column present in excel then its index as par grid column index
                                        flg = true;
                                        dtexcel.Columns[j].SetOrdinal(dgvGSTR27Other.Columns[i].Index);
                                        break;
                                    }
                                }
                                else if (dgvGSTR27Other.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim() == dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim())
                                {
                                    // IF GRID COLUMN PRESENT IN EXCEL THEN ITS INDEX AS PAR GRID COLUMN INDEX
                                    flg = true;
                                    dtexcel.Columns[j].SetOrdinal(dgvGSTR27Other.Columns[i].Index - 2);
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
                        if (dtexcel.Columns.Count >= dgvGSTR27Other.Columns.Count - 2)
                        {
                            for (int i = dtexcel.Columns.Count; i > (dgvGSTR27Other.Columns.Count - 2); i--)
                            {
                                dtexcel.Columns.Remove(dtexcel.Columns[i - 1]);
                            }
                        }
                        dtexcel.AcceptChanges();
                        #endregion

                        #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME
                        foreach (DataGridViewColumn col in dgvGSTR27Other.Columns)
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

                        #region SET SEQUENCE NO
                        for (int i = 0; i < dtexcel.Rows.Count; i++)
                        {
                            dtexcel.Rows[i]["colChk"] = "False";
                            dtexcel.Rows[i]["colSequence"] = i + 1;

                            if (!Utility.IsValidStateName(Convert.ToString(dtexcel.Rows[i]["colPlaceOfSupply"]).Trim()))
                                dtexcel.Rows[i]["colPlaceOfSupply"] = "";

                            if (!ValidateData(Convert.ToString(dtexcel.Rows[i]["colTypeOfNote"]).Trim()))
                                dtexcel.Rows[i]["colTypeOfNote"] = "";

                            if (!chkVal(Convert.ToString(dtexcel.Rows[i]["colIssue"]).Trim()))
                                dtexcel.Rows[i]["colIssue"] = "";

                            if (Convert.ToString(dtexcel.Rows[i]["colRegime"]).Trim().ToLower() == "yes" || Convert.ToString(dtexcel.Rows[i]["colRegime"]).Trim().ToLower() == "y")
                                dtexcel.Rows[i]["colRegime"] = "Yes";
                            else
                                dtexcel.Rows[i]["colRegime"] = "No";

                            int sj = dtexcel.Columns["colDbtCrdtNoteDate"].Ordinal;
                            dtexcel = Utility.ChangeColumnDataType(dtexcel, dtexcel.Columns["colDbtCrdtNoteDate"].ColumnName, typeof(string));
                            dtexcel.Columns["colDbtCrdtNoteDate"].SetOrdinal(sj);

                            sj = dtexcel.Columns["colOrginvoiceDate"].Ordinal;
                            dtexcel = Utility.ChangeColumnDataType(dtexcel, dtexcel.Columns["colOrginvoiceDate"].ColumnName, typeof(string));
                            dtexcel.Columns["colOrginvoiceDate"].SetOrdinal(sj);

                            try
                            {
                                DateTime ss = Convert.ToDateTime(dtexcel.Rows[i]["colDbtCrdtNoteDate"]);
                                dtexcel.Rows[i]["colDbtCrdtNoteDate"] = Convert.ToString(ss.ToString("dd-MM-yyyy").Replace('/', '-'));
                            }
                            catch (Exception)
                            {
                                dtexcel.Rows[i]["colDbtCrdtNoteDate"] = "";
                            }

                            try
                            {
                                DateTime ss = Convert.ToDateTime(dtexcel.Rows[i]["colOrginvoiceDate"]);
                                dtexcel.Rows[i]["colOrginvoiceDate"] = Convert.ToString(ss.ToString("dd-MM-yyyy").Replace('/', '-'));
                            }
                            catch (Exception)
                            {
                                dtexcel.Rows[i]["colOrginvoiceDate"] = "";
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

                // return datatable
                return dtexcel;
            }
        }

        public void ExportExcel()
        {
            try
            {
                if (dgvGSTR27Other.Rows.Count > 0)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID

                    pbGSTR1.Visible = true;

                    #region CREATE WORKBOOK AND ASSIGN COLUMNNAME
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook WB = excelApp.Workbooks.Add(Missing.Value);

                    Excel.Worksheet newWS = (Excel.Worksheet)excelApp.Worksheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    newWS.Name = "cdna";

                    // DELETE UNUSED WORKSHEETS FROM WORKBOOK
                    foreach (Excel.Worksheet ws in WB.Worksheets)
                    {
                        if (ws.Name != "cdna")
                            ((Excel.Worksheet)WB.Worksheets[ws.Name]).Delete();
                    }

                    // ASSIGN COLUMN HEADER AS PAR THE GRID HEADER
                    for (int i = 1; i < dgvGSTR27Other.Columns.Count; i++)
                    {
                        newWS.Cells[1, i] = dgvGSTR27Other.Columns[i].HeaderText.ToString();

                        ((Excel.Range)newWS.Cells[1, i]).ColumnWidth = 17;
                    }

                    // GET RANGE AND SET DIFFRENT PROPERTIES
                    Excel.Range headerRange = (Excel.Range)newWS.get_Range((Excel.Range)newWS.Cells[1, 1], (Excel.Range)newWS.Cells[1, dgvGSTR27Other.Columns.Count - 1]);
                    headerRange.WrapText = true;
                    headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Calibri";
                    #endregion

                    #region COPY DATA FROM DATATABLE TO ARRAY

                    // CREATE ARRAY TO HOLD THE DATA OF DATATABLE
                    object[,] arr = new object[dgvGSTR27Other.Rows.Count, dgvGSTR27Other.Columns.Count];

                    // ASSIGN DATA TO ARRAY FROM DATATABLE
                    if (CommonHelper.IsLicence)
                    {
                        // FOR LICENECE ALLOWS TO EXPORT ALL RECORDS
                        for (int i = 0; i < dgvGSTR27Other.Rows.Count; i++)
                        {
                            for (int j = 1; j < dgvGSTR27Other.Columns.Count; j++)
                            {
                                if (dgvGSTR27Other.Columns[j].Name == "colDbtCrdtNoteDate" || dgvGSTR27Other.Columns[j].Name == "colOrginvoiceDate")
                                {
                                    try
                                    {
                                        DateTime ss = Convert.ToDateTime(dgvGSTR27Other.Rows[i].Cells[j].Value);
                                        arr[i, j - 1] = ss.ToString("dd-MM-yyyy").Replace('/', '-');
                                    }
                                    catch (Exception)
                                    {
                                        arr[i, j - 1] = "";
                                    }
                                }
                                else
                                    arr[i, j - 1] = Convert.ToString(dgvGSTR27Other.Rows[i].Cells[j].Value);
                            }
                        }
                    }
                    else
                    {
                        // FOR DEMO ALLOW ONLY 100 RECORDS TO EXPORT
                        for (int i = 0; i < dgvGSTR27Other.Rows.Count; i++)
                        {
                            if (i < 100)
                            {
                                for (int j = 1; j < dgvGSTR27Other.Columns.Count; j++)
                                {
                                    if (dgvGSTR27Other.Columns[j].Name == "colDbtCrdtNoteDate" || dgvGSTR27Other.Columns[j].Name == "colOrginvoiceDate")
                                    {
                                        try
                                        {
                                            DateTime ss = Convert.ToDateTime(dgvGSTR27Other.Rows[i].Cells[j].Value);
                                            arr[i, j - 1] = ss.ToString("dd-MM-yyyy").Replace('/', '-');
                                        }
                                        catch (Exception)
                                        {
                                            arr[i, j - 1] = "";
                                        }
                                    }
                                    else
                                        arr[i, j - 1] = Convert.ToString(dgvGSTR27Other.Rows[i].Cells[j].Value);
                                }
                            }
                        }
                    }

                    //SET EXCEL RANGE TO PASTE THE DATA
                    Excel.Range top = (Excel.Range)newWS.Cells[2, 1];
                    Excel.Range bottom = (Excel.Range)newWS.Cells[dgvGSTR27Other.Rows.Count + 1, dgvGSTR27Other.Columns.Count];
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
                        //    MessageBox.Show("Please close opened related excel file..");
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
                        dt = (DataTable)dgvGSTR27Other.DataSource;

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
                                DisableControls(dgvGSTR27Other);

                                #region COPY IMPORTED CSV DATATABLE DATA INTO GRID DATATABLE
                                if (dtCsv != null && dtCsv.Rows.Count > 0)
                                {
                                    foreach (DataRow row in dtCsv.Rows)
                                    {
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
                                foreach (DataGridViewColumn col in dgvGSTR27Other.Columns)
                                {
                                    dtCsv.Columns[col.Index].ColumnName = col.Name.ToString();
                                    col.DataPropertyName = col.Name;
                                }
                                dt.AcceptChanges();
                                #endregion

                                // ASSIGN DATATABLE TO GRID
                                dgvGSTR27Other.DataSource = dt;

                                // ENABLE MAIN GRID
                                EnableControls(dgvGSTR27Other);
                            }
                            else
                            {
                                // IF THERE ARE NO RECORDS IN MAIN GRID

                                if (dtCsv != null && dtCsv.Rows.Count > 0)
                                {
                                    // IF THERE ARE RECORD PRESENT IN IMPORT FILE

                                    // DISABLE MAIN GRID
                                    DisableControls(dgvGSTR27Other);

                                    #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME
                                    foreach (DataGridViewColumn col in dgvGSTR27Other.Columns)
                                    {
                                        dtCsv.Columns[col.Index].ColumnName = col.Name.ToString();
                                        col.DataPropertyName = col.Name;
                                    }
                                    dtCsv.AcceptChanges();

                                    // ASSIGN DATATABLE TO GRID
                                    dgvGSTR27Other.DataSource = dtCsv;

                                    // ENABLE CONTROL
                                    EnableControls(dgvGSTR27Other);
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

                            string[] colNo = { "colDbtCrdtNoteNo", "colOrgInvoiceNo", "colInvoiceNumber", "colOrginvoiceValue", "colTaxable", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };
                            GetTotal(colNo);
                        }
                        else
                        {
                            MessageBox.Show("Please import valid csv template...!!!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please choose .csv or .~csv file only.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //CUSTOM MESSAGEBOX TO SHOW ERROR 
                    }
                }

                pbGSTR1.Visible = false;
            }
            catch (Exception ex)
            {
                EnableControls(dgvGSTR27Other);
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
                    for (int i = 1; i < dgvGSTR27Other.Columns.Count; i++)
                    {
                        Boolean flg = false;
                        for (int j = 0; j < csvData.Columns.Count; j++)
                        {
                            // CHECK GRID COLUMN IS PRESENT OR NOT IN IMPORTED EXCEL
                            if (dgvGSTR27Other.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim().Length >= 40)
                            {
                                string piece = dgvGSTR27Other.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim().Substring(0, 40);
                                string piece1 = string.Empty;

                                if (csvData.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim().Length >= 40)
                                    piece1 = csvData.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim().Substring(0, 40);
                                else
                                    piece1 = csvData.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim();

                                if (piece == piece1)
                                {
                                    // if grid column present in excel then its index as par grid column index
                                    flg = true;
                                    csvData.Columns[j].SetOrdinal(dgvGSTR27Other.Columns[i].Index);
                                    break;
                                }
                            }
                            else if (dgvGSTR27Other.Columns[i].HeaderText.Replace(" ", "") == csvData.Columns[j].ColumnName.Replace(" ", "").Trim())
                            {
                                // IF GRID COLUMN PRESENT IN EXCEL THEN ITS INDEX AS PAR GRID COLUMN INDEX
                                flg = true;
                                csvData.Columns[j].SetOrdinal(dgvGSTR27Other.Columns[i].Index - 1);
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
                    if (csvData.Columns.Count >= dgvGSTR27Other.Columns.Count - 2)
                    {
                        for (int i = csvData.Columns.Count - 1; i > (dgvGSTR27Other.Columns.Count - 2); i--)
                        {
                            csvData.Columns.Remove(csvData.Columns[i]);
                        }
                    }
                    #endregion

                    #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME
                    foreach (DataGridViewColumn col in dgvGSTR27Other.Columns)
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
                if (dgvGSTR27Other.Rows.Count > 1)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID

                    pbGSTR1.Visible = true;

                    string csv = string.Empty;
                    // CREATE DATATABLE AND GET GRID DATA
                    DataTable dt = new DataTable();
                    dt = (DataTable)dgvGSTR27Other.DataSource;

                    #region ASSIGN COLUMN NAME TO CSV STRING
                    for (int i = 1; i < dgvGSTR27Other.Columns.Count; i++)
                    {
                        csv += dgvGSTR27Other.Columns[i].HeaderText + ',';
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
                            MessageBox.Show("Please close opened related csv file..", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    #endregion
                }
                else
                {
                    // IF THERE ARE NO RECORD IN MAIN  GRID
                    MessageBox.Show("ExportToCSV: There are no records to export...!!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        #endregion

        #region PDF TRANSACTIONS

        public void ExportPDF()
        {
            try
            {
                pbGSTR1.Visible = true;

                #region CREATING ITEXTSHARP TABLE FROM THE DATATABLE DATA AND ASSIGNING TABLE HEADER
                PdfPTable pdfTable = new PdfPTable(dgvGSTR27Other.ColumnCount - 2);
                pdfTable.DefaultCell.Padding = 3;
                pdfTable.WidthPercentage = 100;
                pdfTable.DefaultCell.BorderWidth = 0;
                iTextSharp.text.Font fontHeader = iTextSharp.text.FontFactory.GetFont("Calibri", 6);

                // ADD HEADER TO PDF TABLE
                pdfTable = AssignHeader(pdfTable, "7. Details of Credit/Debit Notes (Other than reverse charge)");
                #endregion

                #region ADDING HEADER ROW
                int i = 0;

                #region HEADER1
                PdfPCell celHeader1 = new PdfPCell();

                celHeader1 = new PdfPCell(new Phrase("GSTIN", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Name Of Party", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Type of note (Debit/ Credit)", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Reason for issuing Dr./ Cr. Notes", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Pre GST Regime Dr./ Cr. Notes", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Debit Note/ Credit Note/ Refund Voucher", fontHeader));
                celHeader1.Colspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Original Invoice", fontHeader));
                celHeader1.Colspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Rate", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Taxable Value", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Amount", fontHeader));
                celHeader1.Colspan = 4;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Place of Supply (Name of State)", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                pdfTable.CompleteRow();
                #endregion

                #region HEADER2
                PdfPCell celHeader2 = new PdfPCell();

                celHeader2 = new PdfPCell(new Phrase("No.", fontHeader));
                celHeader2 = SetAllignMent(celHeader2, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(255, 255, 204));
                pdfTable.AddCell(celHeader2);

                celHeader2 = new PdfPCell(new Phrase("Date", fontHeader));
                celHeader2 = SetAllignMent(celHeader2, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(255, 255, 204));
                pdfTable.AddCell(celHeader2);

                celHeader2 = new PdfPCell(new Phrase("No.", fontHeader));
                celHeader2 = SetAllignMent(celHeader2, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(255, 255, 204));
                pdfTable.AddCell(celHeader2);

                celHeader2 = new PdfPCell(new Phrase("Date", fontHeader));
                celHeader2 = SetAllignMent(celHeader2, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(255, 255, 204));
                pdfTable.AddCell(celHeader2);

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
                foreach (DataGridViewColumn column in dgvGSTR27Other.Columns)
                {
                    if (i != 0 && i != 1)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase("(" + (i - 1).ToString() + ")", fontHeader));
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
                    foreach (DataGridViewRow row in dgvGSTR27Other.Rows)
                    {
                        i = 0;

                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            if (cell.Value != null && i != 0 && i != 1) // && i != 1)
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
                    foreach (DataGridViewRow row in dgvGSTR27Other.Rows)
                    {
                        if (sj < 100)
                        {
                            i = 0;
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                if (cell.Value != null && i != 0 && i != 1) // && i != 1)
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
                ce1.Colspan = dgvGSTR27Other.Columns.Count - 2;
                ce1.VerticalAlignment = Element.ALIGN_CENTER;
                ce1.HorizontalAlignment = Element.ALIGN_LEFT;
                ce1.BackgroundColor = new iTextSharp.text.BaseColor(197, 223, 197);
                ce1.BorderWidth = 0;
                pdfTable.AddCell(ce1);

                iTextSharp.text.Font FigToRs = FontFactory.GetFont("Calibri", 10, iTextSharp.text.Font.BOLD);
                PdfPCell ceHeader2 = new PdfPCell(new Phrase("(figures in Rs)", FigToRs));
                ceHeader2.Colspan = dgvGSTR27Other.Columns.Count - 2;
                ceHeader2.VerticalAlignment = Element.ALIGN_CENTER;
                ceHeader2.HorizontalAlignment = Element.ALIGN_RIGHT;
                ceHeader2.BorderWidth = 0;
                pdfTable.AddCell(ceHeader2);

                PdfPCell ce2 = new PdfPCell(new Phrase(" "));
                ce2.Colspan = dgvGSTR27Other.Columns.Count - 2;
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

        #region Json TRANSACTIONS

        #region json class

        public class ItmDet
        {
            [DefaultValue("")]
            public double rt { get; set; }
            [DefaultValue("")]
            public double txval { get; set; }
            public double iamt { get; set; }
            public double camt { get; set; }
            public double samt { get; set; }
            public double csamt { get; set; }
        }

        public class Itm
        {
            public int num { get; set; }
            public ItmDet itm_det { get; set; }
        }

        public class Nt
        {
            public string ntty { get; set; }
            public string nt_num { get; set; }
            public string nt_dt { get; set; }
            public string p_gst { get; set; }
            public string rsn { get; set; }
            public string inum { get; set; }
            public string idt { get; set; }
            [DefaultValue("")]
            public double val { get; set; }
            public List<Itm> itms { get; set; }
        }

        public class Cdnr
        {
            public string ctin { get; set; }
            public List<Nt> nt { get; set; }
        }

        public class RootObject
        {
            public string gstin { get; set; }
            public string fp { get; set; }
            public double gt { get; set; }
            public double cur_gt { get; set; }
            public List<Cdnr> cdnr { get; set; }
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
                    #region Datatable Json

                    DataTable dt = new DataTable();

                    #region Bind Grid Data

                    #region ADD DATATABLE COLUMN

                    foreach (DataGridViewColumn col in dgvGSTR27Other.Columns)
                    {
                        dt.Columns.Add(col.Name.ToString());
                        col.DataPropertyName = col.Name;
                    }
                    #endregion

                    #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                    object[] rowValue = new object[dt.Columns.Count];

                    foreach (DataGridViewRow dr in dgvGSTR27Other.Rows)
                    {
                        if (dr.Index != dgvGSTR27Other.Rows.Count - 1)
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
                    dt.Columns.Remove("colParty");
                    dt.AcceptChanges();

                    #endregion

                    #endregion

                    List<string> list = dt.Rows
                           .OfType<DataRow>()
                           .Select(x => Convert.ToString(x["colGSTIN"]).Trim())
                           .Distinct().ToList();

                    if (list != null && list.Count > 0)
                    {
                        ObjJson.gstin = CommonHelper.CompanyGSTN; // tax person GSTIN
                        ObjJson.fp = CommonHelper.GetReturnPeriod(); // current return period
                        ObjJson.gt = CommonHelper.TurnOver; // previous financial year turnover
                        ObjJson.cur_gt = Convert.ToDouble(CommonHelper.CurrentTurnOver); // current Finacial year turnover

                        List<Cdnr> objCdnr = new List<Cdnr>();

                        for (int i = 0; i < list.Count; i++)
                        {
                            if (list[i] != "")
                            {
                                Cdnr objTCdnr = new Cdnr();
                                objTCdnr.ctin = Convert.ToString(list[i]);
                                objCdnr.Add(objTCdnr);

                                ObjJson.cdnr = objCdnr;

                                #region Group By Invoice no
                                List<string> lstInvNo = dt.Rows
                                       .OfType<DataRow>()
                                       .Where(x => list[i] == Convert.ToString(x["colGSTIN"]).Trim())
                                       .Select(x => Convert.ToString(x["colOrgInvoiceNo"]).Trim())
                                       .Distinct().ToList();
                                #endregion

                                for (int sj = 0; sj < lstInvNo.Count; sj++)
                                {
                                    if (lstInvNo[sj] != "")
                                    {
                                        List<string> lstRate = dt.Rows
                                               .OfType<DataRow>()
                                               .Where(x => list[i] == Convert.ToString(x["colGSTIN"]).Trim() && Convert.ToString(lstInvNo[sj]).Trim() == Convert.ToString(x["colOrgInvoiceNo"]).Trim())
                                               .Select(x => Convert.ToString(x["colRate"]).Trim())
                                               .Distinct().ToList();

                                        if (lstRate != null && lstRate.Count > 0)
                                        {
                                            List<Nt> objInv = new List<Nt>();
                                            List<Itm> objItm = new List<Itm>();
                                            List<ItmDet> objItemDetails = new List<ItmDet>();

                                            for (int k = 0; k < lstRate.Count; k++)
                                            {
                                                if (Convert.ToString(lstRate[k]).Trim() != "")
                                                {
                                                    List<DataRow> lstDrRate = dt.Rows
                                                           .OfType<DataRow>()
                                                           .Where(x => list[i] == Convert.ToString(x["colGSTIN"]).Trim() && Convert.ToString(lstInvNo[sj]).Trim() == Convert.ToString(x["colOrgInvoiceNo"]).Trim() && Convert.ToDecimal(lstRate[k]) == Convert.ToDecimal(x["colRate"]))
                                                           .Select(x => x)
                                                           .ToList();

                                                    if (lstDrRate != null && lstDrRate.Count > 0)
                                                    {
                                                        if (k == 0)
                                                        {
                                                            Nt clsNt = new Nt();

                                                            #region Invoice Details

                                                            if (!string.IsNullOrEmpty(Convert.ToString(lstDrRate[0]["colDbtCrdtNoteNo"]).Trim()))
                                                                clsNt.nt_num = Convert.ToString(lstDrRate[0]["colDbtCrdtNoteNo"]).Trim(); //CDR No

                                                            if (!string.IsNullOrEmpty(Convert.ToString(lstDrRate[0]["colDbtCrdtNoteDate"]).Trim()))
                                                                clsNt.nt_dt = Convert.ToString(Convert.ToDateTime(lstDrRate[0]["colDbtCrdtNoteDate"]).ToString("dd-MM-yyyy")); // CDR Date

                                                            if (!string.IsNullOrEmpty(Convert.ToString(lstDrRate[0]["colOrgInvoiceNo"]).Trim()))
                                                                clsNt.inum = Convert.ToString(lstDrRate[0]["colOrgInvoiceNo"]).Trim(); // invoice No

                                                            clsNt.ntty = GetJsonVal(Convert.ToString(lstDrRate[0]["colTypeOfNote"]).Trim(), "TYPE"); // CDR type

                                                            if (chkVal(Convert.ToString(lstDrRate[0]["colIssue"]).Trim()))
                                                                clsNt.rsn = Convert.ToString(lstDrRate[0]["colIssue"]).Trim(); // CDR Reason

                                                            if (!string.IsNullOrEmpty(Convert.ToString(lstDrRate[0]["colOrginvoiceDate"]).Trim()))
                                                                clsNt.idt = Convert.ToString(Convert.ToDateTime(lstDrRate[0]["colOrginvoiceDate"]).ToString("dd-MM-yyyy")); // invoice Date

                                                            //int val = Convert.ToInt32(lstDrRate.Cast<DataRow>().Where(x => Convert.ToString(x["colOrginvoiceValue"]).Trim() != null).Sum(x => Convert.ToString(x["colOrginvoiceValue"]).ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["colOrginvoiceValue"])));
                                                            clsNt.val = Convert.ToDouble(Convert.ToString(lstDrRate[0]["colTaxable"]).Trim()); // val; // Invoice Value

                                                            clsNt.p_gst = GetJsonVal(Convert.ToString(lstDrRate[0]["colRegime"]).Trim(), "PRE"); // CDR Pre GST

                                                            #endregion

                                                            objInv.Add(clsNt);

                                                            if (ObjJson.cdnr[i].nt == null)
                                                                ObjJson.cdnr[i].nt = objInv;
                                                            else
                                                                ObjJson.cdnr[i].nt.AddRange(objInv);
                                                        }

                                                        Itm clsItems = new Itm();
                                                        clsItems.num = k + 1;

                                                        #region Invoice Item Details

                                                        ItmDet clsItmDet = new ItmDet();

                                                        if (!string.IsNullOrEmpty(Convert.ToString(lstDrRate[0]["colRate"]).Trim())) // Rate
                                                            clsItmDet.rt = Convert.ToInt32(lstDrRate[0]["colRate"]);

                                                        if (lstDrRate.Count == 1)
                                                        {
                                                            if (!string.IsNullOrEmpty(Convert.ToString(lstDrRate[0]["colTaxable"]).Trim())) // Taxable value
                                                                clsItmDet.txval = Convert.ToDouble(lstDrRate[0]["colTaxable"]);

                                                            if (!string.IsNullOrEmpty(Convert.ToString(lstDrRate[0]["colIGSTAmnt"]).Trim())) // IGST amount
                                                                clsItmDet.iamt = Convert.ToDouble(lstDrRate[0]["colIGSTAmnt"]);

                                                            if (!string.IsNullOrEmpty(Convert.ToString(lstDrRate[0]["colCGSTAmnt"]).Trim())) // CGST amount
                                                                clsItmDet.camt = Convert.ToDouble(lstDrRate[0]["colCGSTAmnt"]);

                                                            if (!string.IsNullOrEmpty(Convert.ToString(lstDrRate[0]["colSGSTAmnt"]).Trim())) // SGST amount
                                                                clsItmDet.samt = Convert.ToDouble(lstDrRate[0]["colSGSTAmnt"]);

                                                            if (!string.IsNullOrEmpty(Convert.ToString(lstDrRate[0]["colCessAmnt"]).Trim())) // CESS amount
                                                                clsItmDet.csamt = Convert.ToDouble(lstDrRate[0]["colCessAmnt"]);
                                                        }
                                                        else
                                                        {
                                                            double? igst = null, cgst = null, sgst = null, cess = null;
                                                            for (int sr = 0; sr < lstDrRate.Count; sr++)
                                                            {
                                                                if (Utility.IsDecimalOrNumber(Convert.ToString(lstDrRate[sr]["colIGSTAmnt"]).Trim()))
                                                                    igst = Convert.ToDouble(igst) + Convert.ToDouble(lstDrRate[sr]["colIGSTAmnt"]);

                                                                if (Utility.IsDecimalOrNumber(Convert.ToString(lstDrRate[sr]["colCGSTAmnt"]).Trim()))
                                                                    cgst = Convert.ToDouble(cgst) + Convert.ToDouble(lstDrRate[sr]["colCGSTAmnt"]);

                                                                if (Utility.IsDecimalOrNumber(Convert.ToString(lstDrRate[sr]["colSGSTAmnt"]).Trim()))
                                                                    sgst = Convert.ToDouble(sgst) + Convert.ToDouble(lstDrRate[sr]["colSGSTAmnt"]);

                                                                if (Utility.IsDecimalOrNumber(Convert.ToString(lstDrRate[sr]["colCessAmnt"]).Trim()))
                                                                    cess = Convert.ToDouble(cess) + Convert.ToDouble(lstDrRate[sr]["colCessAmnt"]);
                                                            }

                                                            clsItmDet.txval = lstDrRate.Cast<DataRow>().Where(x => x["colTaxable"] != null).Sum(x => Convert.ToString(x["colTaxable"]).Trim() == "" ? 0 : Convert.ToDouble(x["colTaxable"]));

                                                            if (igst != null) { clsItmDet.iamt = Convert.ToDouble(igst); } // IGST value 
                                                            if (cgst != null) { clsItmDet.camt = Convert.ToDouble(cgst); } // CGST value
                                                            if (sgst != null) { clsItmDet.samt = Convert.ToDouble(sgst); } // SGST value
                                                            if (cess != null) { clsItmDet.csamt = Convert.ToDouble(cess); } // CESS value
                                                        }
                                                        #endregion

                                                        clsItems.itm_det = clsItmDet;
                                                        objItm.Add(clsItems);
                                                        ObjJson.cdnr[i].nt[sj].itms = objItm;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // Cdnur
                            }
                        }
                    }
                    #endregion

                    #region File Save
                    JavaScriptSerializer objScript = new JavaScriptSerializer();

                    var settings = new JsonSerializerSettings();
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    settings.DefaultValueHandling = DefaultValueHandling.Ignore;

                    objScript.MaxJsonLength = 2147483647;

                    string FinalJson = JsonConvert.SerializeObject(ObjJson, settings);

                    SaveFileDialog save = new SaveFileDialog();
                    save.FileName = "CDN.json";
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
               // }

                #region Grid Json

                //List<string> list = dgvGSTR27Other.Rows
                //       .OfType<DataGridViewRow>()
                //       .Select(x => Convert.ToString(x.Cells["colGSTIN"].Value))
                //       .Distinct().ToList();

                //if (list != null && list.Count > 0)
                //{
                //    ObjJson.gstin = CommonHelper.CompanyGSTN; // tax person GSTIN
                //    ObjJson.fp = CommonHelper.GetReturnPeriod(); // current return period
                //    ObjJson.gt = CommonHelper.TurnOver; // previous financial year turnover
                //    ObjJson.cur_gt = 99.99; // current Finacial year turnover

                //    List<Cdnr> objCdnr = new List<Cdnr>();

                //    for (int i = 0; i < list.Count; i++)
                //    {
                //        if (list[i] != "")
                //        {
                //            Cdnr objTCdnr = new Cdnr();
                //            objTCdnr.ctin = Convert.ToString(list[i]);
                //            objCdnr.Add(objTCdnr);

                //            ObjJson.cdnr = objCdnr;

                //            #region Group By Invoice no
                //            List<string> lstInvNo = dgvGSTR27Other.Rows
                //                   .OfType<DataGridViewRow>()
                //                   .Where(x => list[i] == Convert.ToString(x.Cells["colGSTIN"].Value))
                //                   .Select(x => Convert.ToString(x.Cells["colOrgInvoiceNo"].Value))
                //                   .Distinct().ToList();
                //            #endregion

                //            for (int sj = 0; sj < lstInvNo.Count; sj++)
                //            {
                //                if (lstInvNo[sj] != "")
                //                {
                //                    #region Invoice Number
                //                    List<DataGridViewRow> Invoicelist = dgvGSTR27Other.Rows
                //                           .OfType<DataGridViewRow>()
                //                           .Where(x => lstInvNo[sj] == Convert.ToString(x.Cells["colOrgInvoiceNo"].Value) && list[i] == Convert.ToString(x.Cells["colGSTIN"].Value).Trim())
                //                           .Select(x => x)
                //                           .ToList();
                //                    #endregion

                //                    if (Invoicelist != null && Invoicelist.Count > 0)
                //                    {
                //                        List<Nt> objInv = new List<Nt>();
                //                        List<Itm> objItm = new List<Itm>();
                //                        List<ItmDet> objItemDetails = new List<ItmDet>();

                //                        for (int j = 0; j < Invoicelist.Count; j++)
                //                        {
                //                            if (j == 0)
                //                            {
                //                                Nt clsNt = new Nt();

                //                                #region Invoice Details

                //                                if (!string.IsNullOrEmpty(Convert.ToString(Invoicelist[j].Cells["colDbtCrdtNoteNo"].Value)))
                //                                    clsNt.nt_num = Convert.ToString(Invoicelist[j].Cells["colDbtCrdtNoteNo"].Value); //CDR No

                //                                if (!string.IsNullOrEmpty(Convert.ToString(Invoicelist[j].Cells["colDbtCrdtNoteDate"].Value)))
                //                                    clsNt.nt_dt = Convert.ToString(Convert.ToDateTime(Invoicelist[j].Cells["colDbtCrdtNoteDate"].Value).ToString("dd-MM-yyyy")); // CDR Date

                //                                if (!string.IsNullOrEmpty(Convert.ToString(Invoicelist[j].Cells["colOrgInvoiceNo"].Value)))
                //                                    clsNt.inum = Convert.ToString(Invoicelist[j].Cells["colOrgInvoiceNo"].Value); // invoice No

                //                                clsNt.ntty = GetJsonVal(Convert.ToString(Invoicelist[j].Cells["colTypeOfNote"].Value), "CDR"); // CDR type

                //                                if (!string.IsNullOrEmpty(Convert.ToString(Invoicelist[j].Cells["colReason"].Value)))
                //                                    clsNt.rsn = Convert.ToString(Invoicelist[j].Cells["colReason"].Value); // CDR Reason

                //                                if (!string.IsNullOrEmpty(Convert.ToString(Invoicelist[j].Cells["colOrginvoiceDate"].Value)))
                //                                    clsNt.idt = Convert.ToString(Convert.ToDateTime(Invoicelist[j].Cells["colOrginvoiceDate"].Value).ToString("dd-MM-yyyy")); // invoice Date

                //                                int val = Convert.ToInt32(Invoicelist.Cast<DataGridViewRow>().Where(x => x.Cells["colOrginvoiceValue"].Value != null).Sum(x => x.Cells["colOrginvoiceValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colOrginvoiceValue"].Value)));
                //                                clsNt.val = val; // Invoice Value

                //                                clsNt.p_gst = GetJsonVal(Convert.ToString(Invoicelist[j].Cells["colPreGST"].Value), "PRE"); // CDR Pre GST

                //                                #endregion

                //                                objInv.Add(clsNt);

                //                                if (ObjJson.cdnr[i].nt == null)
                //                                    ObjJson.cdnr[i].nt = objInv;
                //                                else
                //                                    ObjJson.cdnr[i].nt.AddRange(objInv);
                //                            }

                //                            Itm clsItems = new Itm();
                //                            clsItems.num = j + 1;

                //                            #region Invoice Item Details

                //                            ItmDet clsItmDet = new ItmDet();

                //                            if (!string.IsNullOrEmpty(Convert.ToString(Invoicelist[j].Cells["colRate"].Value).Trim())) // Rate
                //                                clsItmDet.rt = Convert.ToInt32(Invoicelist[j].Cells["colRate"].Value);

                //                            if (!string.IsNullOrEmpty(Convert.ToString(Invoicelist[j].Cells["colTaxable"].Value).Trim())) // Taxable value
                //                                clsItmDet.txval = Convert.ToInt32(Invoicelist[j].Cells["colTaxable"].Value);

                //                            if (!string.IsNullOrEmpty(Convert.ToString(Invoicelist[j].Cells["colIGSTAmnt"].Value).Trim())) // IGST amount
                //                                clsItmDet.iamt = Convert.ToDouble(Invoicelist[j].Cells["colIGSTAmnt"].Value);

                //                            if (!string.IsNullOrEmpty(Convert.ToString(Invoicelist[j].Cells["colCGSTAmnt"].Value).Trim())) // CGST amount
                //                                clsItmDet.camt = Convert.ToDouble(Invoicelist[j].Cells["colCGSTAmnt"].Value);

                //                            if (!string.IsNullOrEmpty(Convert.ToString(Invoicelist[j].Cells["colSGSTAmnt"].Value).Trim())) // SGST amount
                //                                clsItmDet.samt = Convert.ToDouble(Invoicelist[j].Cells["colSGSTAmnt"].Value);

                //                            if (!string.IsNullOrEmpty(Convert.ToString(Invoicelist[j].Cells["colCessAmnt"].Value).Trim())) // CESS amount
                //                                clsItmDet.csamt = Convert.ToInt32(Invoicelist[j].Cells["colCessAmnt"].Value);
                //                            #endregion

                //                            clsItems.itm_det = clsItmDet;
                //                            objItm.Add(clsItems);
                //                            ObjJson.cdnr[i].nt[sj].itms = objItm;
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
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

        public void SaveJson()
        {
            try
            {
                RootObject ObjJson = new RootObject();

                List<string> list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Select(x => Convert.ToString(x.Cells["colGSTIN"].Value))
                       .Distinct().ToList();

                if (list != null && list.Count > 0)
                {
                    ObjJson.gstin = CommonHelper.CompanyGSTN; // tax person GSTIN
                    ObjJson.fp = CommonHelper.GetReturnPeriod(); // current return period
                    ObjJson.gt = CommonHelper.TurnOver; // previous financial year turnover
                    ObjJson.cur_gt = 99.99; // current Finacial year turnover

                    List<Cdnr> objCdnr = new List<Cdnr>();

                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i] != "")
                        {
                            Cdnr objTCdnr = new Cdnr();
                            objTCdnr.ctin = Convert.ToString(list[i]);
                            objCdnr.Add(objTCdnr);

                            ObjJson.cdnr = objCdnr;

                            #region Group By Invoice no
                            List<string> lstInvNo = dgvGSTR27Other.Rows
                                   .OfType<DataGridViewRow>()
                                   .Where(x => list[i] == Convert.ToString(x.Cells["colGSTIN"].Value))
                                   .Select(x => Convert.ToString(x.Cells["colOrgInvoiceNo"].Value))
                                   .Distinct().ToList();
                            #endregion

                            for (int sj = 0; sj < lstInvNo.Count; sj++)
                            {
                                if (lstInvNo[sj] != "")
                                {
                                    #region Invoice Number
                                    List<DataGridViewRow> Invoicelist = dgvGSTR27Other.Rows
                                           .OfType<DataGridViewRow>()
                                           .Where(x => lstInvNo[sj] == Convert.ToString(x.Cells["colOrgInvoiceNo"].Value))
                                           .Select(x => x)
                                           .ToList();
                                    #endregion

                                    if (Invoicelist != null && Invoicelist.Count > 0)
                                    {
                                        List<Nt> objInv = new List<Nt>();
                                        List<Itm> objItm = new List<Itm>();
                                        List<ItmDet> objItemDetails = new List<ItmDet>();

                                        for (int j = 0; j < Invoicelist.Count; j++)
                                        {
                                            if (j == 0)
                                            {
                                                Nt clsNt = new Nt();

                                                #region Invoice Details

                                                if (!string.IsNullOrEmpty(Convert.ToString(Invoicelist[j].Cells["colDbtCrdtNoteNo"].Value)))
                                                    clsNt.nt_num = Convert.ToString(Invoicelist[j].Cells["colDbtCrdtNoteNo"].Value); //CDR No

                                                if (!string.IsNullOrEmpty(Convert.ToString(Invoicelist[j].Cells["colDbtCrdtNoteDate"].Value)))
                                                    clsNt.nt_dt = Convert.ToString(Convert.ToDateTime(Invoicelist[j].Cells["colDbtCrdtNoteDate"].Value).ToString("dd-MM-yyyy")); // CDR Date

                                                if (!string.IsNullOrEmpty(Convert.ToString(Invoicelist[j].Cells["colOrgInvoiceNo"].Value)))
                                                    clsNt.inum = Convert.ToString(Invoicelist[j].Cells["colOrgInvoiceNo"].Value); // invoice No

                                                clsNt.ntty = GetJsonVal(Convert.ToString(Invoicelist[j].Cells["colTypeOfNote"].Value), "CDR"); // CDR type

                                                if (!string.IsNullOrEmpty(Convert.ToString(Invoicelist[j].Cells["colIssue"].Value)))
                                                    clsNt.rsn = Convert.ToString(Invoicelist[j].Cells["colIssue"].Value); // CDR Reason

                                                if (!string.IsNullOrEmpty(Convert.ToString(Invoicelist[j].Cells["colOrginvoiceDate"].Value)))
                                                    clsNt.idt = Convert.ToString(Convert.ToDateTime(Invoicelist[j].Cells["colOrginvoiceDate"].Value).ToString("dd-MM-yyyy")); // invoice Date

                                                int val = Convert.ToInt32(Invoicelist.Cast<DataGridViewRow>().Where(x => x.Cells["colOrginvoiceValue"].Value != null).Sum(x => x.Cells["colOrginvoiceValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colOrginvoiceValue"].Value)));
                                                clsNt.val = val; // Invoice Value

                                                clsNt.p_gst = GetJsonVal(Convert.ToString(Invoicelist[j].Cells["colRegime"].Value), "PRE"); // CDR Pre GST

                                                #endregion

                                                objInv.Add(clsNt);

                                                if (ObjJson.cdnr[i].nt == null)
                                                    ObjJson.cdnr[i].nt = objInv;
                                                else
                                                    ObjJson.cdnr[i].nt.AddRange(objInv);
                                            }

                                            Itm clsItems = new Itm();
                                            clsItems.num = j + 1;

                                            #region Invoice Item Details

                                            ItmDet clsItmDet = new ItmDet();

                                            if (!string.IsNullOrEmpty(Convert.ToString(Invoicelist[j].Cells["colRate"].Value).Trim())) // Rate
                                                clsItmDet.rt = Convert.ToInt32(Invoicelist[j].Cells["colRate"].Value);

                                            if (!string.IsNullOrEmpty(Convert.ToString(Invoicelist[j].Cells["colTaxable"].Value).Trim())) // Taxable value
                                                clsItmDet.txval = Convert.ToInt32(Invoicelist[j].Cells["colTaxable"].Value);

                                            if (!string.IsNullOrEmpty(Convert.ToString(Invoicelist[j].Cells["colIGSTAmnt"].Value).Trim())) // IGST amount
                                                clsItmDet.iamt = Convert.ToDouble(Invoicelist[j].Cells["colIGSTAmnt"].Value);
                                            else
                                                clsItmDet.iamt = 0.0;

                                            if (!string.IsNullOrEmpty(Convert.ToString(Invoicelist[j].Cells["colCessAmnt"].Value).Trim())) // CESS amount
                                                clsItmDet.csamt = Convert.ToInt32(Invoicelist[j].Cells["colCessAmnt"].Value);
                                            else
                                                clsItmDet.csamt = 0;
                                            #endregion

                                            clsItems.itm_det = clsItmDet;
                                            objItm.Add(clsItems);
                                            ObjJson.cdnr[i].nt[sj].itms = objItm;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #region File Save
                    JavaScriptSerializer objScript = new JavaScriptSerializer();
                    objScript.MaxJsonLength = 2147483647;
                    string FinalJson = objScript.Serialize(ObjJson);
                    GSPApisetting builder = new GSPApisetting();
                    builder.SaveJsonToGSTN(FinalJson, "");
                    #endregion
                }
                else { MessageBox.Show("Data Not Available.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information); }
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

        public string GetJsonVal(string cellValue, string cellCol)
        {
            string cdr = "";
            cellValue = cellValue.Trim().ToLower();

            try
            {
                if (cellCol == "TYPE")
                {
                    if (cellValue == "credit note")
                        cdr = "C";
                    else if (cellValue == "debit note")
                        cdr = "D";
                    else if (cellValue == "refund voucher")
                        cdr = "R";
                }
                else if (cellCol == "PRE")
                {
                    if (cellValue == "yes")
                        cdr = "Y";
                    else
                        cdr = "N";
                }
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }

            return cdr;
        }

        #endregion

        public void SetGridViewColor()
        {
            try
            {
                // DO NOT ALLOW TO AUTO GENERATE COLUMNS
                dgvGSTR27Other.AutoGenerateColumns = false;
                dgvGSTR27Other.AllowUserToAddRows = false;

                // SET HEIGHT WIDTH OF FORM
                this.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.97));
                this.Height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.77));

                // SET WIDTH OF HEADER, MAIN AND TOTAL GRID
                this.pnlHeader.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.96));
                this.dgvGSTR27Other.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.96));
                this.dgvGSTR27OtherTotal.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.96));

                // SET HEIGHT OF MAIN GRID
                this.dgvGSTR27Other.Height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.65));

                // SET LOCATION OF HEADER,LOADING PIC, CHECKBOX AND MAIN AND TOTAL GRID
                //this.pnlHeader.Location = new System.Drawing.Point(12, 0);
                //this.dgvGSTR27Other.Location = new System.Drawing.Point(12, Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.05)));
                //this.dgvGSTR27OtherTotal.Location = new System.Drawing.Point(12, Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.71)));
                //this.ckboxHeader.Location = new System.Drawing.Point(32, Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.135)));
                //this.pbGSTR1.Location = new System.Drawing.Point(Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.45)), Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.30)));

                // SET MAIN GRID PROPERTY
                dgvGSTR27Other.EnableHeadersVisualStyles = false;
                dgvGSTR27Other.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(23, 196, 187);
                dgvGSTR27Other.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

                this.dgvGSTR27Other.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                this.dgvGSTR27Other.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                this.dgvGSTR27Other.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

                foreach (DataGridViewColumn column in dgvGSTR27Other.Columns)
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

        public bool chkVal(string val)
        {
            bool flg = false;
            val = val.Trim();
            try
            {
                if (val == "01-Sales Return" || val == "02-Post Sale Discount" || val == "03-Deficiency in services" || val == "04-Correction in Invoice" || val == "05-Change in POS" || val == "06-Finalization of Provisional assessment" || val == "07-Others")
                    flg = true;
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

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)dgvGSTR27Other.DataSource;
                if (dt == null)
                {
                    MessageBox.Show("Kindly save record(s) before search!!!", "Alert", MessageBoxButtons.OK);
                    return;
                }
                ((DataTable)dgvGSTR27Other.DataSource).DefaultView.RowFilter = string.Format("colSequence like '%{0}%' or colGSTIN like '%{0}%' or colParty like '%{0}%' or colTypeOfNote like '%{0}%' or colDbtCrdtNoteNo like '%{0}%' or colDbtCrdtNoteDate like '%{0}%' or colOrgInvoiceNo like '%{0}%' or colRegime like '%{0}%' or colIssue like '%{0}%' or colOrginvoiceValue like '%{0}%' or colOrginvoiceDate like '%{0}%' or colRate like '%{0}%' or colTaxable like '%{0}%' or colIGSTAmnt like '%{0}%' or colCGSTAmnt like '%{0}%' or colSGSTAmnt like '%{0}%' or colCessAmnt like '%{0}%'", txtSearch.Text.Trim().Replace("'", "''"));
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

        private void dgvGSTR27Other_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                // SET INDEX OF USER ADDED ROW IN MAIN GRID
                dgvGSTR27Other.Rows[e.Row.Index - 1].Cells[1].Value = e.Row.Index;
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

        private void dgvGSTR27Other_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                // SET SEQUNCING AFTER USER DELETING ROW IN GRID
                for (int i = e.Row.Index; i < dgvGSTR27Other.Rows.Count - 1; i++)
                {
                    dgvGSTR27Other.Rows[i].Cells["colSequence"].Value = i;
                }

                // TOTAL CALCULATION
                string[] colNo = { "colDbtCrdtNoteNo", "colOrgInvoiceNo", "colInvoiceNumber", "colOrginvoiceValue", "colTaxable", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };
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

        public bool ValidateData(string inv)
        {
            bool flg = false;
            inv = inv.Trim();
            try
            {
                if (inv == "Credit note" || inv == "Debit note" || inv == "Refund Voucher")
                    flg = true;
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }

            return flg;
        }

        #region DISABLE/ENABLE CONTROLS

        private void DisableControls(Control con)
        {
            foreach (Control c in con.Controls)
            {
                if (c.Name != "SPQGSTR1B2B" && c.Name != "dgvGSTR27OtherTotal")
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

        #region CHECK ALL AND UNCHECK ALL

        private void dgvGSTR27Other_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                // CHECK FIRST COLUMN HEADER PRESSED AND MAIN GRID HAVING RECORDS
                if (e.ColumnIndex == 0 && dgvGSTR27Other.Rows.Count > 1)
                {
                    // CHECK AND UNCHECK CHECK BOX OF HEADER FOR SELECTING AND UNSELECTING ALL RECORDS
                    if (dgvGSTR27Other.Columns[e.ColumnIndex].HeaderText == "Check All")
                        ckboxHeader.Checked = true;
                    else if (dgvGSTR27Other.Columns[e.ColumnIndex].HeaderText == "Uncheck All")
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
                // IF THERE ARE RECORDS IN MAIN GRID
                if (dgvGSTR27Other.Rows.Count > 1)
                {
                    pbGSTR1.Visible = true;
                    if (ckboxHeader.Checked)
                    {
                        // IF CHECK BOX IS CHECKED

                        // SET CHECK BOX COLUMN VALUE AS TRUE
                        for (int i = 0; i < dgvGSTR27Other.Rows.Count - 1; i++)
                        {
                            dgvGSTR27Other.Rows[i].Cells[0].Value = "True";
                            Application.DoEvents();
                        }

                        // CHANGE HEADER TEXT AND WIDTH OF COLUMN AND POSITION OF CHECK BOX OF CHECK ALL COLUMN
                        //dgvGSTR27Other.Columns[0].DefaultCellStyle.NullValue = true;
                        dgvGSTR27Other.Columns[0].HeaderText = "Uncheck All";
                    }
                    else if (ckboxHeader.Checked == false)
                    {
                        // IF CHECK BOX IS UNCHECKED

                        // SET CHECK BOX COLUMN VALUE AS FALSE
                        for (int i = 0; i < dgvGSTR27Other.Rows.Count - 1; i++)
                        {
                            dgvGSTR27Other.Rows[i].Cells[0].Value = "False";
                            Application.DoEvents();
                        }

                        // CHANGE HEADER TEXT AND WIDTH OF COLUMN AND POSITION OF CHECK BOX OF CHECK ALL COLUMN
                        //dgvGSTR27Other.Columns[0].DefaultCellStyle.NullValue = false;
                        dgvGSTR27Other.Columns[0].HeaderText = "Check All";
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

        #region MOUSE ENETER AND LEAVE EVENT

        private void btnDelete_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                if (btn != null)
                {
                    // SET EFFECT WHEN ANY BUTTON IS PRESSED
                    btn.BackColor = Color.FromArgb(21, 66, 139);
                    btn.ForeColor = Color.White;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnDelete_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                if (btn != null)
                {
                    // SET EFFECT WHEN ANY BUTTON IS RELEASED
                    btn.BackColor = Color.FromArgb(23, 196, 187);
                    btn.ForeColor = Color.FromArgb(21, 66, 139);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region SCROLL GRID

        private void dgvGSTR27Other_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                // SET TOTAL GRID OFFSET AS PAR MAIN GRID SCROLL
                this.dgvGSTR27OtherTotal.HorizontalScrollingOffset = this.dgvGSTR27Other.HorizontalScrollingOffset;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvGSTR27OtherTotal_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                // SET MAIN GRID OFFSET AS PAR TOTAL GRID SCROLL
                this.dgvGSTR27Other.HorizontalScrollingOffset = this.dgvGSTR27OtherTotal.HorizontalScrollingOffset;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void dgvGSTR27OtherTotal_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                if (dgvGSTR27OtherTotal.Rows.Count > 0)
                {
                    DataGridViewRow row = this.dgvGSTR27OtherTotal.RowTemplate;
                    row.MinimumHeight = 30;
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

        private void frmGSTR27Other_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName("");
        }

        public void ValidataAndGetGSTIN()
        {
            try
            {
                if (dgvGSTR27Other.Rows.Count > 0)
                {
                    pbGSTR1.Visible = true;
                    new PrefillHelper().GetNameByGSTIN(dgvGSTR27Other, "colGSTIN", "colParty");
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
