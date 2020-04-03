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

namespace SPEQTAGST.xasjbr1
{
    public partial class SPQGSTR1CDNUR : Form
    {
        r2Publicclass objGSTR2 = new r2Publicclass();
        r1Publicclass objGSTR7A = new r1Publicclass();

        public SPQGSTR1CDNUR()
        {
            InitializeComponent();
            SetGridViewColor();

            GetData();

            string[] colNo = { "colDbtCrdtNoteNo", "colOrgInvoiceNo", "colOrginvoiceValue", "colTaxable", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };
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
            //    string[] colNo = { "colDbtCrdtNoteNo", "colOrgInvoiceNo", "colOrginvoiceValue", "colTaxable", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };
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
                //string Query = "Select * from SPQR1CDNUR where Fld_Month='" + CommonHelper.SelectedMonth + "' and Fld_FileStatus != 'Total'";
                string Query = "Select Fld_Sequence,Fld_PartyName,Fld_SupplyType,Fld_TypeOfNote,Fld_PreGST,Fld_DbtCrdtNoteNo,Fld_DbtCrdtNoteDate,Fld_OrgInvoiceNo,Fld_OrginvoiceDate,Fld_OrgInvoiceValue,Fld_Rate,Fld_Taxable,Fld_IGSTAmnt,Fld_CGSTAmnt,Fld_SGSTAmnt,Fld_CessAmnt,Fld_PlaceOfSupply,Fld_FileStatus,Fld_Month from SPQR1CDNUR where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
                Application.DoEvents();

                // GET DATA FROM DATABASE
                dt = objGSTR2.GetDataGSTR2(Query);

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

                    dt.Columns.Add(new DataColumn("colChk"));
                    dt.Columns["colChk"].SetOrdinal(0);
                    dt.Columns.Add(new DataColumn("colError"));
                    // RENAME DATATABLE COLUMN NAME TO DATAGRIDVIEW COLUMN NAME
                    foreach (DataGridViewColumn col in dgvGSTR27Other.Columns)
                    {
                        dt.Columns[col.Index].ColumnName = col.Name.ToString();
                        col.DataPropertyName = col.Name;
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string ColName = dt.Columns[j].ColumnName;
                            if (ColName == "colOrginvoiceValue" || ColName == "colTaxable" || ColName == "colIGSTAmnt" || ColName == "colCGSTAmnt" || ColName == "colSGSTAmnt" || ColName == "colCessAmnt")
                                dt.Rows[i][j] = Utility.DisplayIndianCurrency(Convert.ToString(dt.Rows[i][j]));
                        }
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
                if (dgvGSTR27Other.Rows.Count > 1)
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

                        DataRow dr = dtTotal.NewRow();
                        dr["colTDbtCrdtNoteNo"] = dgvGSTR27Other.Rows.Cast<DataGridViewRow>().Where(x => Convert.ToString(x.Cells["colDbtCrdtNoteNo"].Value).Trim() != "").GroupBy(x => x.Cells["colDbtCrdtNoteNo"].Value).Select(x => x.First()).Distinct().Count();
                        dr["colTOrgInvoiceNo"] = dgvGSTR27Other.Rows.Cast<DataGridViewRow>().Where(x => Convert.ToString(x.Cells["colOrgInvoiceNo"].Value).Trim() != "").GroupBy(x => x.Cells["colOrgInvoiceNo"].Value).Select(x => x.First()).Distinct().Count();

                        decimal invval = 0, tax = 0, igst = 0, cgst = 0, sgst = 0, cess = 0;

                        // CREATE DATAROW TO STORE GRID COLUMN CALCULATION
                        List<DataGridViewRow> list = dgvGSTR27Other.Rows
                        .OfType<DataGridViewRow>()
                        .Where(x => Convert.ToString(x.Cells["colTypeOfNote"].Value).Trim() == "Debit Note")
                        .Select(x => x)
                        .ToList();
                        if (list != null && list.Count > 0)
                        {
                            invval = list.Cast<DataGridViewRow>().Where(x => x.Cells["colOrginvoiceValue"].Value != null).Sum(x => x.Cells["colOrginvoiceValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colOrginvoiceValue"].Value));
                            tax = list.Cast<DataGridViewRow>().Where(x => x.Cells["colTaxable"].Value != null).Sum(x => x.Cells["colTaxable"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colTaxable"].Value));
                            igst = list.Cast<DataGridViewRow>().Where(x => x.Cells["colIGSTAmnt"].Value != null).Sum(x => x.Cells["colIGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIGSTAmnt"].Value));
                            cgst = list.Cast<DataGridViewRow>().Where(x => x.Cells["colCGSTAmnt"].Value != null).Sum(x => x.Cells["colCGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCGSTAmnt"].Value));
                            sgst = list.Cast<DataGridViewRow>().Where(x => x.Cells["colSGSTAmnt"].Value != null).Sum(x => x.Cells["colSGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colSGSTAmnt"].Value));
                            cess = list.Cast<DataGridViewRow>().Where(x => x.Cells["colCessAmnt"].Value != null).Sum(x => x.Cells["colCessAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCessAmnt"].Value));
                        }


                        list = null;
                        list = dgvGSTR27Other.Rows
                        .OfType<DataGridViewRow>()
                        .Where(x => Convert.ToString(x.Cells["colTypeOfNote"].Value).Trim() == "Credit Note")
                        .Select(x => x)
                        .ToList();
                        if (list != null && list.Count > 0)
                        {
                            invval = invval - list.Cast<DataGridViewRow>().Where(x => x.Cells["colOrginvoiceValue"].Value != null).Sum(x => x.Cells["colOrginvoiceValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colOrginvoiceValue"].Value));
                            tax = tax - list.Cast<DataGridViewRow>().Where(x => x.Cells["colTaxable"].Value != null).Sum(x => x.Cells["colTaxable"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colTaxable"].Value));
                            igst = igst - list.Cast<DataGridViewRow>().Where(x => x.Cells["colIGSTAmnt"].Value != null).Sum(x => x.Cells["colIGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIGSTAmnt"].Value));
                            cgst = cgst - list.Cast<DataGridViewRow>().Where(x => x.Cells["colCGSTAmnt"].Value != null).Sum(x => x.Cells["colCGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCGSTAmnt"].Value));
                            sgst = sgst - list.Cast<DataGridViewRow>().Where(x => x.Cells["colSGSTAmnt"].Value != null).Sum(x => x.Cells["colSGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colSGSTAmnt"].Value));
                            cess = cess - list.Cast<DataGridViewRow>().Where(x => x.Cells["colCessAmnt"].Value != null).Sum(x => x.Cells["colCessAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCessAmnt"].Value));
                        }

                        list = null;
                        list = dgvGSTR27Other.Rows
                        .OfType<DataGridViewRow>()
                        .Where(x => Convert.ToString(x.Cells["colTypeOfNote"].Value).Trim() == "Refund Voucher")
                        .Select(x => x)
                        .ToList();
                        if (list != null && list.Count > 0)
                        {
                            invval = invval - dgvGSTR27Other.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colOrginvoiceValue"].Value != null).Sum(x => x.Cells["colOrginvoiceValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colOrginvoiceValue"].Value));
                            tax = tax - dgvGSTR27Other.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colTaxable"].Value != null).Sum(x => x.Cells["colTaxable"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colTaxable"].Value));
                            igst = igst - dgvGSTR27Other.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colIGSTAmnt"].Value != null).Sum(x => x.Cells["colIGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIGSTAmnt"].Value));
                            cgst = cgst - dgvGSTR27Other.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCGSTAmnt"].Value != null).Sum(x => x.Cells["colCGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCGSTAmnt"].Value));
                            sgst = sgst - dgvGSTR27Other.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colSGSTAmnt"].Value != null).Sum(x => x.Cells["colSGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colSGSTAmnt"].Value));
                            cess = cess - dgvGSTR27Other.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCessAmnt"].Value != null).Sum(x => x.Cells["colCessAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCessAmnt"].Value));
                        }

                        dr["colTOrginvoiceValue"] = invval;
                        dr["colTTaxable"] = tax;
                        dr["colTIGSTAmnt"] = igst;
                        dr["colTCGSTAmnt"] = cgst;
                        dr["colTSGSTAmnt"] = sgst;
                        dr["colTCESSAmnt"] = cess;

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

                        decimal invval = 0, tax = 0, igst = 0, cgst = 0, sgst = 0, cess = 0;

                        // CREATE DATAROW TO STORE GRID COLUMN CALCULATION
                        List<DataGridViewRow> list = dgvGSTR27Other.Rows
                        .OfType<DataGridViewRow>()
                        .Where(x => Convert.ToString(x.Cells["colTypeOfNote"].Value).Trim() == "Debit Note")
                        .Select(x => x)
                        .ToList();
                        if (list != null && list.Count > 0)
                        {
                            invval = list.Cast<DataGridViewRow>().Where(x => x.Cells["colOrginvoiceValue"].Value != null).Sum(x => x.Cells["colOrginvoiceValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colOrginvoiceValue"].Value));
                            tax = list.Cast<DataGridViewRow>().Where(x => x.Cells["colTaxable"].Value != null).Sum(x => x.Cells["colTaxable"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colTaxable"].Value));
                            igst = list.Cast<DataGridViewRow>().Where(x => x.Cells["colIGSTAmnt"].Value != null).Sum(x => x.Cells["colIGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIGSTAmnt"].Value));
                            cgst = list.Cast<DataGridViewRow>().Where(x => x.Cells["colCGSTAmnt"].Value != null).Sum(x => x.Cells["colCGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCGSTAmnt"].Value));
                            sgst = list.Cast<DataGridViewRow>().Where(x => x.Cells["colSGSTAmnt"].Value != null).Sum(x => x.Cells["colSGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colSGSTAmnt"].Value));
                            cess = list.Cast<DataGridViewRow>().Where(x => x.Cells["colCessAmnt"].Value != null).Sum(x => x.Cells["colCessAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCessAmnt"].Value));
                        }

                        list = null;
                        list = dgvGSTR27Other.Rows
                        .OfType<DataGridViewRow>()
                        .Where(x => Convert.ToString(x.Cells["colTypeOfNote"].Value).Trim() == "Credit Note")
                        .Select(x => x)
                        .ToList();
                        if (list != null && list.Count > 0)
                        {
                            invval = invval - list.Cast<DataGridViewRow>().Where(x => x.Cells["colOrginvoiceValue"].Value != null).Sum(x => x.Cells["colOrginvoiceValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colOrginvoiceValue"].Value));
                            tax = tax - list.Cast<DataGridViewRow>().Where(x => x.Cells["colTaxable"].Value != null).Sum(x => x.Cells["colTaxable"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colTaxable"].Value));
                            igst = igst - list.Cast<DataGridViewRow>().Where(x => x.Cells["colIGSTAmnt"].Value != null).Sum(x => x.Cells["colIGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIGSTAmnt"].Value));
                            cgst = cgst - list.Cast<DataGridViewRow>().Where(x => x.Cells["colCGSTAmnt"].Value != null).Sum(x => x.Cells["colCGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCGSTAmnt"].Value));
                            sgst = sgst - list.Cast<DataGridViewRow>().Where(x => x.Cells["colSGSTAmnt"].Value != null).Sum(x => x.Cells["colSGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colSGSTAmnt"].Value));
                            cess = cess - list.Cast<DataGridViewRow>().Where(x => x.Cells["colCessAmnt"].Value != null).Sum(x => x.Cells["colCessAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCessAmnt"].Value));
                        }


                        list = null;
                        list = dgvGSTR27Other.Rows
                        .OfType<DataGridViewRow>()
                        .Where(x => Convert.ToString(x.Cells["colTypeOfNote"].Value).Trim() == "Refund Voucher")
                        .Select(x => x)
                        .ToList();
                        if (list != null && list.Count > 0)
                        {
                            invval = invval - list.Cast<DataGridViewRow>().Where(x => x.Cells["colOrginvoiceValue"].Value != null).Sum(x => x.Cells["colOrginvoiceValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colOrginvoiceValue"].Value));
                            tax = tax - list.Cast<DataGridViewRow>().Where(x => x.Cells["colTaxable"].Value != null).Sum(x => x.Cells["colTaxable"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colTaxable"].Value));
                            igst = igst - list.Cast<DataGridViewRow>().Where(x => x.Cells["colIGSTAmnt"].Value != null).Sum(x => x.Cells["colIGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIGSTAmnt"].Value));
                            cgst = cgst - list.Cast<DataGridViewRow>().Where(x => x.Cells["colCGSTAmnt"].Value != null).Sum(x => x.Cells["colCGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCGSTAmnt"].Value));
                            sgst = sgst - list.Cast<DataGridViewRow>().Where(x => x.Cells["colSGSTAmnt"].Value != null).Sum(x => x.Cells["colSGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colSGSTAmnt"].Value));
                            cess = cess - list.Cast<DataGridViewRow>().Where(x => x.Cells["colCessAmnt"].Value != null).Sum(x => x.Cells["colCessAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCessAmnt"].Value));
                        }

                        // CALCULATE TOTAL ONLY SPECIFIC COLUMN
                        foreach (var item in colNo)
                        {
                            if (item == "colDbtCrdtNoteNo")
                                dgvGSTR27OtherTotal.Rows[0].Cells["colTDbtCrdtNoteNo"].Value = dgvGSTR27Other.Rows.Cast<DataGridViewRow>().Where(x => Convert.ToString(x.Cells["colDbtCrdtNoteNo"].Value).Trim() != "").GroupBy(x => x.Cells["colDbtCrdtNoteNo"].Value).Select(x => x.First()).Distinct().Count();
                            else if (item == "colOrgInvoiceNo")
                                dgvGSTR27OtherTotal.Rows[0].Cells["colTOrgInvoiceNo"].Value = dgvGSTR27Other.Rows.Cast<DataGridViewRow>().Where(x => Convert.ToString(x.Cells["colOrgInvoiceNo"].Value).Trim() != "").GroupBy(x => x.Cells["colOrgInvoiceNo"].Value).Select(x => x.First()).Distinct().Count();
                            else if (item == "colOrginvoiceValue")
                                dgvGSTR27OtherTotal.Rows[0].Cells["colTOrginvoiceValue"].Value = invval;
                            else if (item == "colTaxable")
                                dgvGSTR27OtherTotal.Rows[0].Cells["colTTaxable"].Value = tax;
                            else if (item == "colIGSTAmnt")
                                dgvGSTR27OtherTotal.Rows[0].Cells["colTIGSTAmnt"].Value = igst;
                            else if (item == "colCGSTAmnt")
                                dgvGSTR27OtherTotal.Rows[0].Cells["colTCGSTAmnt"].Value = cgst;
                            else if (item == "colSGSTAmnt")
                                dgvGSTR27OtherTotal.Rows[0].Cells["colTSGSTAmnt"].Value = sgst;
                            else if (item == "colCessAmnt")
                                dgvGSTR27OtherTotal.Rows[0].Cells["colTCESSAmnt"].Value = cess;
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

                    string[] colNo = { "colDbtCrdtNoteNo", "colOrgInvoiceNo", "colOrginvoiceValue", "colTaxable", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };
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
                                            if (dtDGV.Columns[i].ColumnName == "colSupplyType")
                                            {
                                                if (Convert.ToString(dr.Cells[i].Value).Trim() == "Export with payment of GST" || Convert.ToString(dr.Cells[i].Value).Trim() == "Export without payment of GST" || Convert.ToString(dr.Cells[i].Value).Trim() == "B2C Large")
                                                    rowValue[i] = Convert.ToString(dr.Cells[i].Value);
                                                else
                                                    rowValue[i] = "";
                                            }
                                            else if (dtDGV.Columns[i].ColumnName == "colTypeOfNote")
                                            {
                                                if (Convert.ToString(dr.Cells[i].Value).Trim() == "Credit Note" || Convert.ToString(dr.Cells[i].Value).Trim() == "Debit Note" || Convert.ToString(dr.Cells[i].Value).Trim() == "Refund Voucher")
                                                    rowValue[i] = Convert.ToString(dr.Cells[i].Value);
                                                else
                                                    rowValue[i] = "";
                                            }
                                            else if (dtDGV.Columns[i].ColumnName == "colPreGST")
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
                                        if (iCol + i < this.dgvGSTR27Other.ColumnCount && i < 16)
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
                                                        if (oCell.ColumnIndex >= 2 && oCell.ColumnIndex <= 17)
                                                        {
                                                            if (chkCellValue(sCells[i].Trim(), dgvGSTR27Other.Columns[oCell.ColumnIndex].Name))
                                                            {
                                                                if (dgvGSTR27Other.Columns[oCell.ColumnIndex].Name == "colTypeOfNote")
                                                                    dgvGSTR27Other.Rows[iRow].Cells[oCell.ColumnIndex].Value = Utility.StrCDNUR1TypesofNote(sCells[i]);
                                                                //else if (dgvGSTR27Other.Columns[oCell.ColumnIndex].Name == "colReason")
                                                                //    dgvGSTR27Other.Rows[iRow].Cells[oCell.ColumnIndex].Value = Utility.StrCDNUR1Reasonissuing(sCells[i]);
                                                                else if (dgvGSTR27Other.Columns[oCell.ColumnIndex].Name == "colPreGST")
                                                                    dgvGSTR27Other.Rows[iRow].Cells[oCell.ColumnIndex].Value = Utility.StrCDNUR1PreGSTRegime(sCells[i]);
                                                                else if (dgvGSTR27Other.Columns[oCell.ColumnIndex].Name == "colSupplyType")
                                                                    dgvGSTR27Other.Rows[iRow].Cells[oCell.ColumnIndex].Value = Utility.StrCDNUR1TypeofExport(sCells[i]);
                                                                else
                                                                    dgvGSTR27Other.Rows[iRow].Cells[oCell.ColumnIndex].Value = sCells[i].Trim();
                                                            }
                                                            else
                                                            {
                                                                if (dgvGSTR27Other.Columns[oCell.ColumnIndex].Name == "colPreGST")
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
                                                            if (j >= 2 && j <= 17)
                                                            {
                                                                if (chkCellValue(sCells[i].Trim(), dgvGSTR27Other.Columns[j].Name))
                                                                {
                                                                    if (dgvGSTR27Other.Columns[j].Name == "colTypeOfNote")
                                                                        dgvGSTR27Other.Rows[iRow].Cells[j].Value = Utility.StrCDNUR1TypesofNote(sCells[i]);
                                                                    //else if (dgvGSTR27Other.Columns[j].Name == "colReason")
                                                                    //    dgvGSTR27Other.Rows[iRow].Cells[j].Value = Utility.StrCDNUR1Reasonissuing(sCells[i]);
                                                                    else if (dgvGSTR27Other.Columns[j].Name == "colPreGST")
                                                                        dgvGSTR27Other.Rows[iRow].Cells[j].Value = Utility.StrCDNUR1PreGSTRegime(sCells[i]);
                                                                    else if (dgvGSTR27Other.Columns[j].Name == "colSupplyType")
                                                                        dgvGSTR27Other.Rows[iRow].Cells[j].Value = Utility.StrCDNUR1TypeofExport(sCells[i]);
                                                                    else
                                                                        dgvGSTR27Other.Rows[iRow].Cells[j].Value = sCells[i].Trim();
                                                                }
                                                                else
                                                                {
                                                                    if (dgvGSTR27Other.Columns[j].Name == "colPreGST")
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
                                                            if (j >= 2 && j <= 17)
                                                            {
                                                                if (chkCellValue(sCells[i].Trim(), dgvGSTR27Other.Columns[j].Name))
                                                                {
                                                                    if (dgvGSTR27Other.Columns[j].Name == "colTypeOfNote")
                                                                        dgvGSTR27Other.Rows[iRow].Cells[j].Value = Utility.StrCDNUR1TypesofNote(sCells[i]);
                                                                    //else if (dgvGSTR27Other.Columns[j].Name == "colReason")
                                                                    //    dgvGSTR27Other.Rows[iRow].Cells[j].Value = Utility.StrCDNUR1Reasonissuing(sCells[i]);
                                                                    else if (dgvGSTR27Other.Columns[j].Name == "colPreGST")
                                                                        dgvGSTR27Other.Rows[iRow].Cells[j].Value = Utility.StrCDNUR1PreGSTRegime(sCells[i]);
                                                                    else if (dgvGSTR27Other.Columns[j].Name == "colSupplyType")
                                                                        dgvGSTR27Other.Rows[iRow].Cells[j].Value = Utility.StrCDNUR1TypeofExport(sCells[i]);
                                                                    else
                                                                        dgvGSTR27Other.Rows[iRow].Cells[j].Value = sCells[i].Trim();
                                                                }
                                                                else
                                                                {
                                                                    if (dgvGSTR27Other.Columns[j].Name == "colPreGST")
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
                                if (iCol + i < this.dgvGSTR27Other.ColumnCount && colNo < 17)
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
                                                if (colNo >= 2 && colNo <= 17)
                                                {
                                                    if (chkCellValue(sCells[i].Trim(), dt.Columns[colNo].ColumnName))
                                                    {
                                                        if (dgvGSTR27Other.Columns[colNo].Name == "colTypeOfNote")
                                                            dt.Rows[dt.Rows.Count - 1][colNo] = Utility.StrCDNUR1TypesofNote(sCells[i]);
                                                        //else if (dgvGSTR27Other.Columns[colNo].Name == "colReason")
                                                        //    dt.Rows[dt.Rows.Count - 1][colNo] = Utility.StrCDNUR1Reasonissuing(sCells[i]);
                                                        else if (dgvGSTR27Other.Columns[colNo].Name == "colPreGST")
                                                            dt.Rows[dt.Rows.Count - 1][colNo] = Utility.StrCDNUR1PreGSTRegime(sCells[i]);
                                                        else if (dgvGSTR27Other.Columns[colNo].Name == "colSupplyType")
                                                            dt.Rows[dt.Rows.Count - 1][colNo] = Utility.StrCDNUR1TypeofExport(sCells[i]);
                                                        else
                                                            dt.Rows[dt.Rows.Count - 1][colNo] = sCells[i].Trim();
                                                    }
                                                    else
                                                    {
                                                        if (dgvGSTR27Other.Columns[colNo].Name == "colPreGST")
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
                                                    if (j >= 2 && j <= 17)
                                                    {
                                                        if (chkCellValue(sCells[i].Trim(), dt.Columns[j].ColumnName))
                                                        {
                                                            if (dgvGSTR27Other.Columns[j].Name == "colTypeOfNote")
                                                                dt.Rows[dt.Rows.Count - 1][j] = Utility.StrCDNUR1TypesofNote(sCells[i]);
                                                            //else if (dgvGSTR27Other.Columns[j].Name == "colReason")
                                                            //    dt.Rows[dt.Rows.Count - 1][j] = Utility.StrCDNUR1Reasonissuing(sCells[i]);
                                                            else if (dgvGSTR27Other.Columns[j].Name == "colPreGST")
                                                                dt.Rows[dt.Rows.Count - 1][j] = Utility.StrCDNUR1PreGSTRegime(sCells[i]);
                                                            else if (dgvGSTR27Other.Columns[j].Name == "colSupplyType")
                                                                dt.Rows[dt.Rows.Count - 1][j] = Utility.StrCDNUR1TypeofExport(sCells[i]);
                                                            else
                                                                dt.Rows[dt.Rows.Count - 1][j] = sCells[i].Trim();
                                                        }
                                                        else
                                                        {
                                                            if (dgvGSTR27Other.Columns[j].Name == "colPreGST")
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
                                                    if (j >= 2 && j <= 17)
                                                    {
                                                        if (chkCellValue(sCells[i].Trim(), dt.Columns[j].ColumnName))
                                                        {
                                                            if (dgvGSTR27Other.Columns[j].Name == "colTypeOfNote")
                                                                dt.Rows[dt.Rows.Count - 1][j] = Utility.StrCDNUR1TypesofNote(sCells[i]);
                                                            //else if (dgvGSTR27Other.Columns[j].Name == "colReason")
                                                            //    dt.Rows[dt.Rows.Count - 1][j] = Utility.StrCDNUR1Reasonissuing(sCells[i]);
                                                            else if (dgvGSTR27Other.Columns[j].Name == "colPreGST")
                                                                dt.Rows[dt.Rows.Count - 1][j] = Utility.StrCDNUR1PreGSTRegime(sCells[i]);
                                                            else if (dgvGSTR27Other.Columns[j].Name == "colSupplyType")
                                                                dt.Rows[dt.Rows.Count - 1][j] = Utility.StrCDNUR1TypeofExport(sCells[i]);
                                                            else
                                                                dt.Rows[dt.Rows.Count - 1][j] = sCells[i].Trim();
                                                        }
                                                        else
                                                        {
                                                            if (dgvGSTR27Other.Columns[j].Name == "colPreGST")
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
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string ColName = dt.Columns[j].ColumnName;
                            if (ColName == "colOrginvoiceValue" || ColName == "colTaxable" || ColName == "colIGSTAmnt" || ColName == "colCGSTAmnt" || ColName == "colSGSTAmnt" || ColName == "colCessAmnt")
                                dt.Rows[i][j] = Utility.DisplayIndianCurrency(Convert.ToString(dt.Rows[i][j]));
                        }
                    }

                    dgvGSTR27Other.DataSource = dt;
                }

                // TOTAL CALCULATION
                string[] colGroup = { "colDbtCrdtNoteNo", "colOrgInvoiceNo", "colOrginvoiceValue", "colTaxable", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };
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
            int i;
            int j;
            bool flag;
            bool num;
            try
            {
                int _cnt = 0;
                string _str = "";
                string red = "";
                this.dgvGSTR27Other.CurrentCell = this.dgvGSTR27Other.Rows[0].Cells[0];
                this.dgvGSTR27Other.AllowUserToAddRows = false;
                this.pbGSTR1.Visible = true;
                DataTable dt = (DataTable)this.dgvGSTR27Other.DataSource;
                List<DataGridViewRow> list = (
                    from x in this.dgvGSTR27Other.Rows.OfType<DataGridViewRow>()
                    where !Utility.CDNUR1TypeofExport(Convert.ToString(x.Cells["colSupplyType"].Value))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR27Other.Rows[list[i].Cells["colSupplyType"].RowIndex].Cells["colSupplyType"].Style.BackColor = Color.Red;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Please enter proper Supply type.\n");
                }
                list = (
                    from x in this.dgvGSTR27Other.Rows.OfType<DataGridViewRow>()
                    where Utility.CDNUR1TypeofExport(Convert.ToString(x.Cells["colSupplyType"].Value))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    this.dgvGSTR27Other.Rows[list[i].Cells["colSupplyType"].RowIndex].Cells["colSupplyType"].Style.BackColor = Color.White;
                }
                list = null;
                list = (
                    from x in this.dgvGSTR27Other.Rows.OfType<DataGridViewRow>()
                    where !Utility.CDNURTypesofNote(Convert.ToString(x.Cells["colTypeOfNote"].Value))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR27Other.Rows[list[i].Cells["colTypeOfNote"].RowIndex].Cells["colTypeOfNote"].Style.BackColor = Color.Red;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Please enter proper type of notes.\n");
                }
                list = (
                    from x in this.dgvGSTR27Other.Rows.OfType<DataGridViewRow>()
                    where Utility.CDNURTypesofNote(Convert.ToString(x.Cells["colTypeOfNote"].Value))
                    select x into x
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    this.dgvGSTR27Other.Rows[list[i].Cells["colTypeOfNote"].RowIndex].Cells["colTypeOfNote"].Style.BackColor = Color.White;
                }
                red = "colPreGST";
                list = null;
                list = (
                    from x in this.dgvGSTR27Other.Rows.OfType<DataGridViewRow>()
                    where ("Yes" == Convert.ToString(x.Cells[red].Value) ? false : "No" != Convert.ToString(x.Cells[red].Value))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR27Other.Rows[list[i].Cells[red].RowIndex].Cells[red].Style.BackColor = Color.Red;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Please enter proper Pre GST Regime Dr./Cr. Notes.\n");
                }
                list = (
                    from x in this.dgvGSTR27Other.Rows.OfType<DataGridViewRow>()
                    where ("Yes" == Convert.ToString(x.Cells[red].Value) ? true : "No" == Convert.ToString(x.Cells[red].Value))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    this.dgvGSTR27Other.Rows[list[i].Cells[red].RowIndex].Cells[red].Style.BackColor = Color.White;
                }
                red = "colDbtCrdtNoteNo";
                list = null;
                list = (
                    from x in this.dgvGSTR27Other.Rows.OfType<DataGridViewRow>()
                    where !Utility.IsInvoiceNumber(Convert.ToString(x.Cells[red].Value))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR27Other.Rows[list[i].Cells[red].RowIndex].Cells[red].Style.BackColor = Color.Red;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Please enter proper Debit Note/ credit note/ Refund voucher No.\n");
                }
                list = (
                    from x in this.dgvGSTR27Other.Rows.OfType<DataGridViewRow>()
                    where Utility.IsInvoiceNumber(Convert.ToString(x.Cells[red].Value))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    this.dgvGSTR27Other.Rows[list[i].Cells[red].RowIndex].Cells[red].Style.BackColor = Color.White;
                }
                if (!CommonHelper.IsQuarter)
                {
                    red = "colDbtCrdtNoteDate";
                    list = null;
                    list = (
                        from x in this.dgvGSTR27Other.Rows.OfType<DataGridViewRow>()
                        where !Utility.IsInvoiceDate(Convert.ToString(x.Cells[red].Value))
                        select x).ToList<DataGridViewRow>();
                    if (list.Count > 0)
                    {
                        for (i = 0; i < list.Count; i++)
                        {
                            this.dgvGSTR27Other.Rows[list[i].Cells[red].RowIndex].Cells[red].Style.BackColor = Color.Red;
                        }
                        _cnt++;
                        _str = string.Concat(_str, _cnt, ") Please enter proper Debit Note/ credit note/ Refund voucher Date.\n");
                    }
                    list = (
                        from x in this.dgvGSTR27Other.Rows.OfType<DataGridViewRow>()
                        where Utility.IsInvoiceDate(Convert.ToString(x.Cells[red].Value))
                        select x).ToList<DataGridViewRow>();
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR27Other.Rows[list[i].Cells[red].RowIndex].Cells[red].Style.BackColor = Color.White;
                    }
                }
                else
                {
                    red = "colDbtCrdtNoteDate";
                    list = null;
                    list = (
                        from x in this.dgvGSTR27Other.Rows.OfType<DataGridViewRow>()
                        where !Utility.IsQuarterlyFilingDate(Convert.ToString(x.Cells[red].Value))
                        select x).ToList<DataGridViewRow>();
                    if (list.Count > 0)
                    {
                        for (i = 0; i < list.Count; i++)
                        {
                            this.dgvGSTR27Other.Rows[list[i].Cells[red].RowIndex].Cells[red].Style.BackColor = Color.Red;
                        }
                        _cnt++;
                        _str = string.Concat(_str, _cnt, ") Please enter proper Debit Note/ credit note/ Refund voucher Date.\n");
                    }
                    list = (
                        from x in this.dgvGSTR27Other.Rows.OfType<DataGridViewRow>()
                        where Utility.IsQuarterlyFilingDate(Convert.ToString(x.Cells[red].Value))
                        select x).ToList<DataGridViewRow>();
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR27Other.Rows[list[i].Cells[red].RowIndex].Cells[red].Style.BackColor = Color.White;
                    }
                }
                red = "colOrgInvoiceNo";
                list = null;
                list = (
                    from x in this.dgvGSTR27Other.Rows.OfType<DataGridViewRow>()
                    where !Utility.IsInvoiceNumber(Convert.ToString(x.Cells[red].Value))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR27Other.Rows[list[i].Cells[red].RowIndex].Cells[red].Style.BackColor = Color.Red;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Original invoice no can not be more than 16 digit.\n");
                }
                list = (
                    from x in this.dgvGSTR27Other.Rows.OfType<DataGridViewRow>()
                    where Utility.IsInvoiceNumber(Convert.ToString(x.Cells[red].Value))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    this.dgvGSTR27Other.Rows[list[i].Cells[red].RowIndex].Cells[red].Style.BackColor = Color.White;
                }
                list = null;
                list = (
                    from x in this.dgvGSTR27Other.Rows.OfType<DataGridViewRow>()
                    where !Utility.IsDate(Convert.ToString(x.Cells["colOrginvoiceDate"].Value))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceDate"].RowIndex].Cells["colOrginvoiceDate"].Style.BackColor = Color.Red;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Please enter proper original invoice date.\n");
                }
                list = (
                    from x in this.dgvGSTR27Other.Rows.OfType<DataGridViewRow>()
                    where Utility.IsDate(Convert.ToString(x.Cells["colOrginvoiceDate"].Value))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    this.dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceDate"].RowIndex].Cells["colOrginvoiceDate"].Style.BackColor = Color.White;
                }
                list = null;
                list = (
                    from x in this.dgvGSTR27Other.Rows.OfType<DataGridViewRow>()
                    where (!Utility.IsDate(Convert.ToString(x.Cells["colDbtCrdtNoteDate"].Value)) ? false : Utility.IsDate(Convert.ToString(x.Cells["colOrginvoiceDate"].Value)))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        if (Convert.ToString(this.dgvGSTR27Other.Rows[list[i].Cells["colPreGST"].RowIndex].Cells["colPreGST"].Value).ToLower() == "yes")
                        {
                            if (!(Convert.ToDateTime(Convert.ToString(this.dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceDate"].RowIndex].Cells["colOrginvoiceDate"].Value).Trim()) < Convert.ToDateTime(Convert.ToString(this.dgvGSTR27Other.Rows[list[i].Cells["colDbtCrdtNoteDate"].RowIndex].Cells["colDbtCrdtNoteDate"].Value).Trim())))
                            {
                                _cnt++;
                                _str = string.Concat(_str, _cnt, ") Please enter proper Original invoice date.\n");
                                this.dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceDate"].RowIndex].Cells["colOrginvoiceDate"].Style.BackColor = Color.Red;
                            }
                            else
                            {
                                if (Convert.ToInt32(Convert.ToDateTime(this.dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceDate"].RowIndex].Cells["colOrginvoiceDate"].Value).ToString("MM").Replace("-", "")) < Convert.ToInt32(7))
                                {
                                    num = false;
                                }
                                else
                                {
                                    DateTime dateTime = Convert.ToDateTime(this.dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceDate"].RowIndex].Cells["colOrginvoiceDate"].Value);
                                    int num1 = Convert.ToInt32(dateTime.ToString("yyyy").Replace("-", ""));
                                    dateTime = Convert.ToDateTime(this.dgvGSTR27Other.Rows[list[i].Cells["colDbtCrdtNoteDate"].RowIndex].Cells["colDbtCrdtNoteDate"].Value);
                                    num = num1 >= Convert.ToInt32(dateTime.ToString("yyyy").Replace("-", ""));
                                }
                                if (num)
                                {
                                    _cnt++;
                                    _str = string.Concat(_str, _cnt, ") Please enter proper Original invoice date.\n");
                                    this.dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceDate"].RowIndex].Cells["colOrginvoiceDate"].Style.BackColor = Color.Red;
                                }
                                else
                                {
                                    this.dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceDate"].RowIndex].Cells["colOrginvoiceDate"].Style.BackColor = Color.White;
                                }
                            }
                        }
                        else if (Convert.ToString(this.dgvGSTR27Other.Rows[list[i].Cells["colPreGST"].RowIndex].Cells["colPreGST"].Value).ToLower() == "no")
                        {
                            if ((Convert.ToDateTime(Convert.ToString(this.dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceDate"].RowIndex].Cells["colOrginvoiceDate"].Value).Trim()) < Convert.ToDateTime("01-07-2017") ? true : !(Convert.ToDateTime(Convert.ToString(this.dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceDate"].RowIndex].Cells["colOrginvoiceDate"].Value).Trim()) <= Convert.ToDateTime(Convert.ToString(this.dgvGSTR27Other.Rows[list[i].Cells["colDbtCrdtNoteDate"].RowIndex].Cells["colDbtCrdtNoteDate"].Value).Trim()))))
                            {
                                _cnt++;
                                _str = string.Concat(_str, _cnt, ") Please enter proper Original invoice date.\n");
                                this.dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceDate"].RowIndex].Cells["colOrginvoiceDate"].Style.BackColor = Color.Red;
                            }
                            else
                            {
                                this.dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceDate"].RowIndex].Cells["colOrginvoiceDate"].Style.BackColor = Color.White;
                            }
                        }
                    }
                }
                red = "colOrginvoiceValue";
                list = null;
                list = (
                    from x in this.dgvGSTR27Other.Rows.OfType<DataGridViewRow>()
                    where !Utility.IsInvoiceValue(Convert.ToString(x.Cells[red].Value))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR27Other.Rows[list[i].Cells[red].RowIndex].Cells[red].Style.BackColor = Color.Red;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Please enter proper Note/Refund Voucher Value.\n");
                }
                list = (
                    from x in this.dgvGSTR27Other.Rows.OfType<DataGridViewRow>()
                    where Utility.IsInvoiceValue(Convert.ToString(x.Cells[red].Value))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    this.dgvGSTR27Other.Rows[list[i].Cells[red].RowIndex].Cells[red].Style.BackColor = Color.White;
                }
                red = "colRate";
                list = null;
                list = (
                    from x in this.dgvGSTR27Other.Rows.OfType<DataGridViewRow>()
                    where !Utility.IsRate(Convert.ToString(x.Cells[red].Value))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR27Other.Rows[list[i].Cells[red].RowIndex].Cells[red].Style.BackColor = Color.Red;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Please enter proper Rate.\n");
                }
                list = (
                    from x in this.dgvGSTR27Other.Rows.OfType<DataGridViewRow>()
                    where Utility.IsRate(Convert.ToString(x.Cells[red].Value))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    this.dgvGSTR27Other.Rows[list[i].Cells[red].RowIndex].Cells[red].Style.BackColor = Color.White;
                }
                list = null;
                list = (
                    from x in this.dgvGSTR27Other.Rows.OfType<DataGridViewRow>()
                    where !Utility.IsTaxableValue(Convert.ToString(x.Cells["colTaxable"].Value))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR27Other.Rows[list[i].Cells["colTaxable"].RowIndex].Cells["colTaxable"].Style.BackColor = Color.Red;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Please enter proper Taxable Value.\n");
                }
                list = (
                    from x in this.dgvGSTR27Other.Rows.OfType<DataGridViewRow>()
                    where Utility.IsTaxableValue(Convert.ToString(x.Cells["colTaxable"].Value))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    decimal s1 = new decimal(0);
                    decimal s2 = new decimal(0);
                    s1 = (Convert.ToString(this.dgvGSTR27Other.Rows[list[i].Cells["colTaxable"].RowIndex].Cells["colTaxable"].Value).Trim() == "" ? Convert.ToDecimal(0) : Convert.ToDecimal(this.dgvGSTR27Other.Rows[list[i].Cells["colTaxable"].RowIndex].Cells["colTaxable"].Value));
                    if (!(s1 > (Convert.ToString(this.dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceValue"].RowIndex].Cells["colOrginvoiceValue"].Value).Trim() == "" ? Convert.ToDecimal(0) : Convert.ToDecimal(this.dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceValue"].RowIndex].Cells["colOrginvoiceValue"].Value))))
                    {
                        this.dgvGSTR27Other.Rows[list[i].Cells["colTaxable"].RowIndex].Cells["colTaxable"].Style.BackColor = Color.White;
                    }
                    else
                    {
                        this.dgvGSTR27Other.Rows[list[i].Cells["colTaxable"].RowIndex].Cells["colTaxable"].Style.BackColor = Color.Red;
                        _cnt++;
                        _str = string.Concat(_str, _cnt, ") Taxable values can not be more than Invoice value.\n");
                    }
                }
                string result = CommonHelper.StateName;
                list = null;
                list = (
                    from x in this.dgvGSTR27Other.Rows.OfType<DataGridViewRow>()
                    where (Convert.ToString(x.Cells["colSupplyType"].Value) == "Export without payment of GST" ? false : Convert.ToString(x.Cells["colSupplyType"].Value) != "Export with payment of GST")
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (j = 0; j < list.Count; j++)
                    {
                        string pgst = Convert.ToString(this.dgvGSTR27Other.Rows[list[j].Cells["colPlaceOfSupply"].RowIndex].Cells["colPlaceOfSupply"].Value);
                        string result1 = pgst;
                        if (!(result.ToLower() != result1.ToLower()))
                        {
                            if (Convert.ToString(this.dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value) != "")
                            {
                                if (!(Convert.ToDecimal(this.dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value) > new decimal(0)))
                                {
                                    this.dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.White;
                                }
                                else
                                {
                                    _cnt++;
                                    _str = string.Concat(_str, _cnt, ") IGST amount is not required in inter state invoice.\n");
                                    this.dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.Red;
                                }
                            }
                            else if (Convert.ToString(this.dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value) != "")
                            {
                                if (Utility.IsICSC(Convert.ToString(this.dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value)))
                                {
                                    this.dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.White;
                                }
                                else if (!(Convert.ToDecimal(this.dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value) > new decimal(0)))
                                {
                                    this.dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.White;
                                }
                                else
                                {
                                    _cnt++;
                                    _str = string.Concat(_str, _cnt, ") Please enter proper IGST Amount.\n");
                                    this.dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.Red;
                                }
                            }
                        }
                        else if (Convert.ToString(this.dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value) == "")
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Please enter proper IGST Amount.\n");
                            this.dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.Red;
                        }
                        else if (Utility.IsICSC(Convert.ToString(this.dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value)))
                        {
                            this.dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.White;
                        }
                        else if (!(Convert.ToDecimal(this.dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value) > new decimal(0)))
                        {
                            this.dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.White;
                        }
                        else
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Please enter proper IGST Amount.\n");
                            this.dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.Red;
                        }
                        if (!(result.ToLower() == result1.ToLower()))
                        {
                            if (!(Convert.ToString(this.dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value) != ""))
                            {
                                this.dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.White;
                            }
                            else if (!(Convert.ToDecimal(this.dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value) > new decimal(0)))
                            {
                                this.dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.White;
                            }
                            else
                            {
                                _cnt++;
                                _str = string.Concat(_str, _cnt, ") CGST amount is not required in intra state invoice.\n");
                                this.dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.Red;
                            }
                        }
                        else if (Convert.ToString(this.dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value) == "")
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Please enter proper Central tax Amount.\n");
                            this.dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.Red;
                        }
                        else if (Utility.IsICSC(Convert.ToString(this.dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value)))
                        {
                            this.dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.White;
                        }
                        else if (!(Convert.ToDecimal(this.dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value) > new decimal(0)))
                        {
                            this.dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.White;
                        }
                        else
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Please enter proper CGST Amount.\n");
                            this.dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.Red;
                        }
                        if (!(result.ToLower() == result1.ToLower()))
                        {
                            if (!(Convert.ToString(this.dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value) != ""))
                            {
                                this.dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.White;
                            }
                            else if (!(Convert.ToDecimal(this.dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value) > new decimal(0)))
                            {
                                this.dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.White;
                            }
                            else
                            {
                                _cnt++;
                                _str = string.Concat(_str, _cnt, ") SGST amount is not required in intra state invoice.\n");
                                this.dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.Red;
                            }
                        }
                        else if (Convert.ToString(this.dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value) == "")
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Please enter proper State/UT tax Amount.\n");
                            this.dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.Red;
                        }
                        else if (Utility.IsICSC(Convert.ToString(this.dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value)))
                        {
                            this.dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.White;
                        }
                        else if (!(Convert.ToDecimal(this.dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value) > new decimal(0)))
                        {
                            this.dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.White;
                        }
                        else
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Please enter proper SGST Amount.\n");
                            this.dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.Red;
                        }
                        if (!(Convert.ToString(this.dgvGSTR27Other.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Value) != ""))
                        {
                            this.dgvGSTR27Other.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.White;
                        }
                        else if (Utility.IsICSC(Convert.ToString(this.dgvGSTR27Other.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Value)))
                        {
                            this.dgvGSTR27Other.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.White;
                        }
                        else
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Please enter proper CESS Amount.\n");
                            this.dgvGSTR27Other.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.Red;
                        }
                    }
                }
                list = null;
                list = (
                    from x in this.dgvGSTR27Other.Rows.OfType<DataGridViewRow>()
                    where Convert.ToString(x.Cells["colSupplyType"].Value) == "Export without payment of GST"
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (j = 0; j < list.Count; j++)
                    {
                        if (!(Convert.ToString(this.dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value) != ""))
                        {
                            this.dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.White;
                        }
                        else if (!(Convert.ToDecimal(this.dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value) > new decimal(0)))
                        {
                            this.dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.White;
                        }
                        else
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") IGST amount is not required in exports without payment.\n");
                            this.dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.Red;
                        }
                        if (!(Convert.ToString(this.dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value) != ""))
                        {
                            this.dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.White;
                        }
                        else if (!(Convert.ToDecimal(this.dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value) > new decimal(0)))
                        {
                            this.dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.White;
                        }
                        else
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") CGST amount is not required in exports without payment.\n");
                            this.dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.Red;
                        }
                        if (!(Convert.ToString(this.dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value) != ""))
                        {
                            this.dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.White;
                        }
                        else if (!(Convert.ToDecimal(this.dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value) > new decimal(0)))
                        {
                            this.dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.White;
                        }
                        else
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") CGST amount is not required in exports without payment.\n");
                            this.dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.Red;
                        }
                        if (!(Convert.ToString(this.dgvGSTR27Other.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Value) != ""))
                        {
                            this.dgvGSTR27Other.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.White;
                        }
                        else if (!(Convert.ToDecimal(this.dgvGSTR27Other.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Value) > new decimal(0)))
                        {
                            this.dgvGSTR27Other.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.White;
                        }
                        else
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") CESS amount is not required in exports without payment.\n");
                            this.dgvGSTR27Other.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.Red;
                        }
                    }
                }
                list = null;
                list = (
                    from x in this.dgvGSTR27Other.Rows.OfType<DataGridViewRow>()
                    where Convert.ToString(x.Cells["colSupplyType"].Value) == "Export with payment of GST"
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (j = 0; j < list.Count; j++)
                    {
                        if (Convert.ToString(this.dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value) == "")
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Please enter proper IGST Amount.\n");
                            this.dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.Red;
                        }
                        else if (Utility.IsICSC(Convert.ToString(this.dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value)))
                        {
                            this.dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.White;
                        }
                        else if (!(Convert.ToDecimal(this.dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value) > new decimal(0)))
                        {
                            this.dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.White;
                        }
                        else
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Please enter proper IGST Amount.\n");
                            this.dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.Red;
                        }
                        if (!(Convert.ToString(this.dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value) != ""))
                        {
                            this.dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.White;
                        }
                        else if (!(Convert.ToDecimal(this.dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value) > new decimal(0)))
                        {
                            this.dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.White;
                        }
                        else
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") CGST amount is not required in SEZ exports with payment and Deemed Export invoice.\n");
                            this.dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.Red;
                        }
                        if (!(Convert.ToString(this.dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value) != ""))
                        {
                            this.dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.White;
                        }
                        else if (!(Convert.ToDecimal(this.dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value) > new decimal(0)))
                        {
                            this.dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.White;
                        }
                        else
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") CGST amount is not required in SEZ exports with payment and Deemed Export invoice.\n");
                            this.dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.Red;
                        }
                        if (!(Convert.ToString(this.dgvGSTR27Other.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Value) != ""))
                        {
                            this.dgvGSTR27Other.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.White;
                        }
                        else if (!(Convert.ToDecimal(this.dgvGSTR27Other.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Value) > new decimal(0)))
                        {
                            this.dgvGSTR27Other.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.White;
                        }
                        else
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") CESS amount is not required in SEZ exports with payment and Deemed Export invoice.\n");
                            this.dgvGSTR27Other.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.Red;
                        }
                    }
                }
                list = null;
                list = (
                    from x in this.dgvGSTR27Other.Rows.OfType<DataGridViewRow>()
                    where Convert.ToString(x.Cells["colSupplyType"].Value) == "B2C Large"
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (j = 0; j < list.Count; j++)
                    {
                        if (Utility.IsValidStateName(Convert.ToString(this.dgvGSTR27Other.Rows[list[j].Cells["colPlaceOfSupply"].RowIndex].Cells["colPlaceOfSupply"].Value)))
                        {
                            this.dgvGSTR27Other.Rows[list[j].Cells["colPlaceOfSupply"].RowIndex].Cells["colPlaceOfSupply"].Style.BackColor = Color.White;
                        }
                        else
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Please select Place of supply.\n");
                            this.dgvGSTR27Other.Rows[list[j].Cells["colPlaceOfSupply"].RowIndex].Cells["colPlaceOfSupply"].Style.BackColor = Color.Red;
                        }
                    }
                }
                list = null;
                list = (
                    from x in this.dgvGSTR27Other.Rows.OfType<DataGridViewRow>()
                    where Convert.ToString(x.Cells["colSupplyType"].Value) != "B2C Large"
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (j = 0; j < list.Count; j++)
                    {
                        if ((Convert.ToString(this.dgvGSTR27Other.Rows[list[j].Cells["colPlaceOfSupply"].RowIndex].Cells["colPlaceOfSupply"].Value) == "" ? false : !Utility.IsValidStateName(Convert.ToString(this.dgvGSTR27Other.Rows[list[j].Cells["colPlaceOfSupply"].RowIndex].Cells["colPlaceOfSupply"].Value))))
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Please select Place of supply.\n");
                            this.dgvGSTR27Other.Rows[list[j].Cells["colPlaceOfSupply"].RowIndex].Cells["colPlaceOfSupply"].Style.BackColor = Color.Red;
                        }
                        else
                        {
                            this.dgvGSTR27Other.Rows[list[j].Cells["colPlaceOfSupply"].RowIndex].Cells["colPlaceOfSupply"].Style.BackColor = Color.White;
                        }
                    }
                }
                DataTable dt9 = (DataTable)this.dgvGSTR27Other.DataSource;
                if (dt9 != null)
                {
                    var result9 = (
                        from row in dt9.AsEnumerable()
                        group row by new { colDbtCrdtNoteNo = row.Field<string>("colDbtCrdtNoteNo"), colPlaceOfSupply = row.Field<string>("colPlaceOfSupply") } into grp
                        select new { colDbtCrdtNoteNo = grp.Key.colDbtCrdtNoteNo, colPlaceOfSupply = grp.Key.colPlaceOfSupply }).ToList();
                    if ((result9 == null ? false : result9.Count > 0))
                    {
                        foreach (var variable in result9)
                        {
                            list = (
                                from x in this.dgvGSTR27Other.Rows.OfType<DataGridViewRow>()
                                where (Convert.ToString(x.Cells["colDbtCrdtNoteNo"].Value) != Convert.ToString(variable.colDbtCrdtNoteNo) ? false : Convert.ToString(x.Cells["colPlaceOfSupply"].Value) != Convert.ToString(variable.colPlaceOfSupply))
                                select x into p
                                select p).ToList<DataGridViewRow>();
                            if ((list == null ? false : list.Count > 0))
                            {
                                for (i = 0; i < list.Count; i++)
                                {
                                    this.dgvGSTR27Other.Rows[list[i].Cells["colPlaceOfSupply"].RowIndex].Cells["colPlaceOfSupply"].Style.BackColor = Color.Red;
                                }
                                _cnt++;
                                _str = string.Concat(_str, _cnt, ") Same invoice no for different POS is not possible.\n");
                            }
                        }
                    }
                }
                this.dgvGSTR27Other.AllowUserToAddRows = true;
                this.pbGSTR1.Visible = false;
                if (!(_str != ""))
                {
                    if (this.objGSTR7A.InsertValidationFlg("GSTR1", "CDNUR", "true", CommonHelper.SelectedMonth) != 1)
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
                    if (this.objGSTR7A.InsertValidationFlg("GSTR1", "CDNUR", "false", CommonHelper.SelectedMonth) != 1)
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
                this.dgvGSTR27Other.AllowUserToAddRows = true;
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
                string _str = "", sj = "";

                dgvGSTR27Other.CurrentCell = dgvGSTR27Other.Rows[0].Cells[0];
                dgvGSTR27Other.AllowUserToAddRows = false;
                pbGSTR1.Visible = true;

                DataTable dt = (DataTable)dgvGSTR27Other.DataSource;

                #region Supply Type
                List<DataGridViewRow> list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.CDNUR1TypeofExport(Convert.ToString(x.Cells["colSupplyType"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR27Other.Rows[list[i].Cells["colSupplyType"].RowIndex].Cells["colSupplyType"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper Supply type.\n";
                }
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.CDNUR1TypeofExport(Convert.ToString(x.Cells["colSupplyType"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR27Other.Rows[list[i].Cells["colSupplyType"].RowIndex].Cells["colSupplyType"].Style.BackColor = Color.White;
                }
                #endregion

                #region Credit And Debit
                list = null;
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.CDNURTypesofNote(Convert.ToString(x.Cells["colTypeOfNote"].Value)))
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
                       .Where(x => true == Utility.CDNURTypesofNote(Convert.ToString(x.Cells["colTypeOfNote"].Value)))
                       .Select(x => x)
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR27Other.Rows[list[i].Cells["colTypeOfNote"].RowIndex].Cells["colTypeOfNote"].Style.BackColor = Color.White;
                }
                #endregion

                #region Reason for issuing note Dr./ Cr. Notes
                //list = null;
                //list = dgvGSTR27Other.Rows
                //       .OfType<DataGridViewRow>()
                //       .Where(x => true != chkVal(Convert.ToString(x.Cells["colReason"].Value)))
                //       .Select(x => x)
                //       .ToList();
                //if (list.Count > 0)
                //{
                //    for (int i = 0; i < list.Count; i++)
                //    {
                //        dgvGSTR27Other.Rows[list[i].Cells["colReason"].RowIndex].Cells["colReason"].Style.BackColor = Color.Red;
                //    }
                //    _cnt += 1;
                //    _str += _cnt + ") Please enter proper Reason for issuing note Dr./ Cr. Notes.\n";
                //}
                //list = dgvGSTR27Other.Rows
                //       .OfType<DataGridViewRow>()
                //       .Where(x => true == chkVal(Convert.ToString(x.Cells["colReason"].Value)))
                //       .Select(x => x)
                //       .ToList();
                //for (int i = 0; i < list.Count; i++)
                //{
                //    dgvGSTR27Other.Rows[list[i].Cells["colReason"].RowIndex].Cells["colReason"].Style.BackColor = Color.White;
                //}
                #endregion

                #region Pre GST Regime Dr./ Cr. Notes
                sj = "colPreGST";
                list = null;
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => "Yes" != Convert.ToString(x.Cells[sj].Value) && "No" != Convert.ToString(x.Cells[sj].Value))
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
                       .Where(x => "Yes" == Convert.ToString(x.Cells[sj].Value) || "No" == Convert.ToString(x.Cells[sj].Value))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR27Other.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.White;
                }
                #endregion

                #region PreGST Inv no and Inv Date
                /*
                #region PreGST = No && InvNo
                sj = "colPreGST";
                list = null;
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Convert.ToString(x.Cells[sj].Value) == "No" && Utility.IsInvoiceNumber(Convert.ToString(x.Cells["colOrgInvoiceNo"].Value)) == false)
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR27Other.Rows[list[i].Cells["colOrgInvoiceNo"].RowIndex].Cells["colOrgInvoiceNo"].Style.BackColor = Color.Red;
                        _cnt += 1;
                        _str += _cnt + ") Original invoice no is mandatory and can not be more than 16 digit.\n";
                    }
                }
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Convert.ToString(x.Cells[sj].Value) == "No" && Utility.IsInvoiceNumber(Convert.ToString(x.Cells["colOrgInvoiceNo"].Value)) == true)
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR27Other.Rows[list[i].Cells["colOrgInvoiceNo"].RowIndex].Cells["colOrgInvoiceNo"].Style.BackColor = Color.White;
                }
                #endregion

                #region PreGST = Yes && InvNo
                list = null;
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Convert.ToString(x.Cells[sj].Value) == "Yes" && Utility.IsBlankInvoiceNumber(Convert.ToString(x.Cells["colOrgInvoiceNo"].Value)) == false)
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR27Other.Rows[list[i].Cells["colOrgInvoiceNo"].RowIndex].Cells["colOrgInvoiceNo"].Style.BackColor = Color.Red;
                        _cnt += 1;
                        _str += _cnt + ") Original invoice no can not be more than 16 digit.\n";
                    }
                }
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Convert.ToString(x.Cells[sj].Value) == "Yes" && Utility.IsBlankInvoiceNumber(Convert.ToString(x.Cells["colOrgInvoiceNo"].Value)) == true)
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR27Other.Rows[list[i].Cells["colOrgInvoiceNo"].RowIndex].Cells["colOrgInvoiceNo"].Style.BackColor = Color.White;
                }
                #endregion

                #region PreGST = No && InvDate
                list = null;
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Convert.ToString(x.Cells[sj].Value) == "No" && Utility.IsInvoiceDate(Convert.ToString(x.Cells["colOrginvoiceDate"].Value)) == false)
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceDate"].RowIndex].Cells["colOrginvoiceDate"].Style.BackColor = Color.Red;
                        _cnt += 1;
                        _str += _cnt + ") Please enter proper Original invoice date.\n";
                    }
                }
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Convert.ToString(x.Cells[sj].Value) == "No" && Utility.IsInvoiceDate(Convert.ToString(x.Cells["colOrginvoiceDate"].Value)) == true)
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceDate"].RowIndex].Cells["colOrginvoiceDate"].Style.BackColor = Color.White;
                }
                #endregion

                #region PreGST = Yes && InvDate
                list = null;
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Convert.ToString(x.Cells[sj].Value) == "Yes" && Utility.IsBlankInvoiceDate(Convert.ToString(x.Cells["colOrginvoiceDate"].Value)) == false)
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceDate"].RowIndex].Cells["colOrginvoiceDate"].Style.BackColor = Color.Red;
                        _cnt += 1;
                        _str += _cnt + ") Please enter proper Original invoice date.\n";
                    }
                }
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Convert.ToString(x.Cells[sj].Value) == "Yes" && Utility.IsBlankInvoiceDate(Convert.ToString(x.Cells["colOrginvoiceDate"].Value)) == true)
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceDate"].RowIndex].Cells["colOrginvoiceDate"].Style.BackColor = Color.White;
                }
                #endregion
                */
                #endregion

                #region Debit Note/ credit note/ Refund voucher No
                sj = "colDbtCrdtNoteNo";
                list = null;
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsInvoiceNumber(Convert.ToString(x.Cells[sj].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR27Other.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper Debit Note/ credit note/ Refund voucher No.\n";
                }
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsInvoiceNumber(Convert.ToString(x.Cells[sj].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR27Other.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.White;
                }
                #endregion

                if (CommonHelper.IsQuarter)
                {
                    #region Debit Note/ credit note/ Refund voucher Date
                    sj = "colDbtCrdtNoteDate";
                    list = null;
                    list = dgvGSTR27Other.Rows
                           .OfType<DataGridViewRow>()
                           .Where(x => true != Utility.IsQuarterlyFilingDate(Convert.ToString(x.Cells[sj].Value)))
                           .Select(x => x)
                           .ToList();
                    if (list.Count > 0)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            dgvGSTR27Other.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.Red;
                        }
                        _cnt += 1;
                        _str += _cnt + ") Please enter proper Debit Note/ credit note/ Refund voucher Date.\n";
                    }
                    list = dgvGSTR27Other.Rows
                           .OfType<DataGridViewRow>()
                           .Where(x => true == Utility.IsQuarterlyFilingDate(Convert.ToString(x.Cells[sj].Value)))
                           .Select(x => x)
                           .ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR27Other.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.White;
                    }
                    #endregion
                }
                else
                {
                    #region Debit Note/ credit note/ Refund voucher Date
                    sj = "colDbtCrdtNoteDate";
                    list = null;
                    list = dgvGSTR27Other.Rows
                           .OfType<DataGridViewRow>()
                           .Where(x => true != Utility.IsInvoiceDate(Convert.ToString(x.Cells[sj].Value)))
                           .Select(x => x)
                           .ToList();
                    if (list.Count > 0)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            dgvGSTR27Other.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.Red;
                        }
                        _cnt += 1;
                        _str += _cnt + ") Please enter proper Debit Note/ credit note/ Refund voucher Date.\n";
                    }
                    list = dgvGSTR27Other.Rows
                           .OfType<DataGridViewRow>()
                           .Where(x => true == Utility.IsInvoiceDate(Convert.ToString(x.Cells[sj].Value)))
                           .Select(x => x)
                           .ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR27Other.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.White;
                    }
                    #endregion
                }

                #region Org Inv No
                sj = "colOrgInvoiceNo";
                list = null;
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsInvoiceNumber(Convert.ToString(x.Cells[sj].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR27Other.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Original invoice no can not be more than 16 digit.\n";
                }
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsInvoiceNumber(Convert.ToString(x.Cells[sj].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR27Other.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.White;
                }
                #endregion

                //if (CommonHelper.IsQuarter)
                //{
                //    #region Org Invoice Date
                //    list = null; //dd-MM-yyyy
                //    list = dgvGSTR27Other.Rows
                //           .OfType<DataGridViewRow>()
                //           .Where(x => true != Utility.IsQuarterlyFilingDate(Convert.ToString(x.Cells["colOrginvoiceDate"].Value)))
                //           .Select(x => x)
                //           .ToList();
                //    if (list.Count > 0)
                //    {
                //        for (int i = 0; i < list.Count; i++)
                //        {
                //            dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceDate"].RowIndex].Cells["colOrginvoiceDate"].Style.BackColor = Color.Red;
                //        }
                //        _cnt += 1;
                //        _str += _cnt + ") Please enter proper original invoice date.\n";
                //    }
                //    list = dgvGSTR27Other.Rows
                //           .OfType<DataGridViewRow>()
                //           .Where(x => true == Utility.IsQuarterlyFilingDate(Convert.ToString(x.Cells["colOrginvoiceDate"].Value)))
                //           .Select(x => x)
                //           .ToList();
                //    for (int i = 0; i < list.Count; i++)
                //    {
                //        dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceDate"].RowIndex].Cells["colOrginvoiceDate"].Style.BackColor = Color.White;
                //    }
                //    #endregion
                //}
                //else
                //{
                #region Org Invoice Date
                list = null; //dd-MM-yyyy
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsDate(Convert.ToString(x.Cells["colOrginvoiceDate"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceDate"].RowIndex].Cells["colOrginvoiceDate"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper original invoice date.\n";
                }
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsDate(Convert.ToString(x.Cells["colOrginvoiceDate"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceDate"].RowIndex].Cells["colOrginvoiceDate"].Style.BackColor = Color.White;
                }
                #endregion
                //}

                #region Debit Note && Org Invoice Date
                list = null;//dd-MM-yyyy
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsDate(Convert.ToString(x.Cells["colDbtCrdtNoteDate"].Value)) && true == Utility.IsDate(Convert.ToString(x.Cells["colOrginvoiceDate"].Value)))
                       .Select(x => x)
                       .ToList();

                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (Convert.ToString(dgvGSTR27Other.Rows[list[i].Cells["colPreGST"].RowIndex].Cells["colPreGST"].Value).ToLower() == "yes")
                        {
                            if (Convert.ToDateTime(Convert.ToString(dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceDate"].RowIndex].Cells["colOrginvoiceDate"].Value).Trim()) < Convert.ToDateTime(Convert.ToString(dgvGSTR27Other.Rows[list[i].Cells["colDbtCrdtNoteDate"].RowIndex].Cells["colDbtCrdtNoteDate"].Value).Trim()))
                            {
                                if (Convert.ToInt32(Convert.ToDateTime(dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceDate"].RowIndex].Cells["colOrginvoiceDate"].Value).ToString("MM").Replace("-", "")) < Convert.ToInt32(07) || Convert.ToInt32(Convert.ToDateTime(dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceDate"].RowIndex].Cells["colOrginvoiceDate"].Value).ToString("yyyy").Replace("-", "")) < Convert.ToInt32(Convert.ToDateTime(dgvGSTR27Other.Rows[list[i].Cells["colDbtCrdtNoteDate"].RowIndex].Cells["colDbtCrdtNoteDate"].Value).ToString("yyyy").Replace("-", "")))
                                    dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceDate"].RowIndex].Cells["colOrginvoiceDate"].Style.BackColor = Color.White;
                                else
                                {
                                    _cnt += 1;
                                    _str += _cnt + ") Please enter proper Original invoice date.\n";
                                    dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceDate"].RowIndex].Cells["colOrginvoiceDate"].Style.BackColor = Color.Red;
                                }
                            }
                            else
                            {
                                _cnt += 1;
                                _str += _cnt + ") Please enter proper Original invoice date.\n";
                                dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceDate"].RowIndex].Cells["colOrginvoiceDate"].Style.BackColor = Color.Red;
                            }
                        }
                        else if (Convert.ToString(dgvGSTR27Other.Rows[list[i].Cells["colPreGST"].RowIndex].Cells["colPreGST"].Value).ToLower() == "no")
                        {
                            if (Convert.ToDateTime(Convert.ToString(dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceDate"].RowIndex].Cells["colOrginvoiceDate"].Value).Trim()) >= Convert.ToDateTime("01-07-2017") && Convert.ToDateTime(Convert.ToString(dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceDate"].RowIndex].Cells["colOrginvoiceDate"].Value).Trim()) <= Convert.ToDateTime(Convert.ToString(dgvGSTR27Other.Rows[list[i].Cells["colDbtCrdtNoteDate"].RowIndex].Cells["colDbtCrdtNoteDate"].Value).Trim()))
                                dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceDate"].RowIndex].Cells["colOrginvoiceDate"].Style.BackColor = Color.White;
                            else
                            {
                                _cnt += 1;
                                _str += _cnt + ") Please enter proper Original invoice date.\n";
                                dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceDate"].RowIndex].Cells["colOrginvoiceDate"].Style.BackColor = Color.Red;
                            }
                        }
                    }
                }
                #endregion

                #region Note/Refund Voucher Value
                sj = "colOrginvoiceValue";
                list = null;
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsInvoiceValue(Convert.ToString(x.Cells[sj].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR27Other.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper Note/Refund Voucher Value.\n";
                }
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsInvoiceValue(Convert.ToString(x.Cells[sj].Value)))
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
                       .Where(x => true != Utility.IsRate(Convert.ToString(x.Cells[sj].Value)))
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
                       .Where(x => true == Utility.IsRate(Convert.ToString(x.Cells[sj].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR27Other.Rows[list[i].Cells[sj].RowIndex].Cells[sj].Style.BackColor = Color.White;
                }
                #endregion

                #region Taxable Value
                list = null;
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsTaxableValue(Convert.ToString(x.Cells["colTaxable"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR27Other.Rows[list[i].Cells["colTaxable"].RowIndex].Cells["colTaxable"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper Taxable Value.\n";
                }
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsTaxableValue(Convert.ToString(x.Cells["colTaxable"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    //dgvGSTR27Other.Rows[list[i].Cells["colTaxable"].RowIndex].Cells["colTaxable"].Style.BackColor = Color.White;
                    decimal s1 = 0, s2 = 0;

                    s1 = Convert.ToString(dgvGSTR27Other.Rows[list[i].Cells["colTaxable"].RowIndex].Cells["colTaxable"].Value).Trim() == "" ? Convert.ToDecimal(0) : Convert.ToDecimal(dgvGSTR27Other.Rows[list[i].Cells["colTaxable"].RowIndex].Cells["colTaxable"].Value);
                    s2 = Convert.ToString(dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceValue"].RowIndex].Cells["colOrginvoiceValue"].Value).Trim() == "" ? Convert.ToDecimal(0) : Convert.ToDecimal(dgvGSTR27Other.Rows[list[i].Cells["colOrginvoiceValue"].RowIndex].Cells["colOrginvoiceValue"].Value);

                    if (s1 > s2)
                    {
                        dgvGSTR27Other.Rows[list[i].Cells["colTaxable"].RowIndex].Cells["colTaxable"].Style.BackColor = Color.Red;
                        _cnt += 1;
                        _str += _cnt + ") Taxable values can not be more than Invoice value.\n";
                    }
                    else
                    {
                        dgvGSTR27Other.Rows[list[i].Cells["colTaxable"].RowIndex].Cells["colTaxable"].Style.BackColor = Color.White;
                    }
                }
                #endregion

                string gstin = CommonHelper.StateName;
                string result = gstin;

                #region Regular B2cl IGST CGST SGST CESS Validation
                list = null;
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Convert.ToString(x.Cells["colSupplyType"].Value) != "Export without payment of GST" && Convert.ToString(x.Cells["colSupplyType"].Value) != "Export with payment of GST")
                       .Select(x => x)
                       .ToList();

                if (list.Count > 0)
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        string pgst = Convert.ToString(dgvGSTR27Other.Rows[list[j].Cells["colPlaceOfSupply"].RowIndex].Cells["colPlaceOfSupply"].Value);
                        string result1 = pgst;

                        #region IGST
                        if (result.ToLower() != result1.ToLower())
                        {
                            if (Convert.ToString(dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value) == "")
                            {
                                _cnt += 1;
                                _str += _cnt + ") Please enter proper IGST Amount.\n";
                                dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.Red;
                            }
                            else
                            {
                                if (!Utility.IsICSC(Convert.ToString(dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value)))
                                {
                                    _cnt += 1;
                                    _str += _cnt + ") Please enter proper IGST Amount.\n";
                                    dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.Red;
                                }
                                else
                                { dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.White; }
                            }
                        }
                        else if (Convert.ToString(dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value) != "")
                        {
                            _cnt += 1;
                            _str += _cnt + ") IGST amount is not required in inter state invoice.\n";
                            dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.Red;
                        }
                        else if (Convert.ToString(dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value) != "")
                        {
                            if (!Utility.IsICSC(Convert.ToString(dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value)))
                            {
                                _cnt += 1;
                                _str += _cnt + ") Please enter proper IGST Amount.\n";
                                dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.Red;
                            }
                            else
                            { dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.White; }
                        }
                        #endregion

                        #region CGST
                        if (result.ToLower() == result1.ToLower())
                        {
                            if (Convert.ToString(dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value) == "")
                            {
                                _cnt += 1;
                                _str += _cnt + ") Please enter proper Central tax Amount.\n";
                                dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.Red;
                            }
                            else
                            {
                                if (!Utility.IsICSC(Convert.ToString(dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value)))
                                {
                                    _cnt += 1;
                                    _str += _cnt + ") Please enter proper CGST Amount.\n";
                                    dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.Red;
                                }
                                else
                                { dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.White; }
                            }
                        }
                        else if (Convert.ToString(dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value) != "")
                        {
                            _cnt += 1;
                            _str += _cnt + ") CGST amount is not required in intra state invoice.\n";
                            dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.Red;
                        }
                        else
                        { dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.White; }
                        //else if (Convert.ToString(dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value) == "")
                        //{
                        //    if (!Utility.IsICSC(Convert.ToString(dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value)))
                        //    {
                        //        _cnt += 1;
                        //        _str += _cnt + ") Please enter proper CGST Amount.\n";
                        //        dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.Red;
                        //    }
                        //    else
                        //    { dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.White; }
                        //}
                        #endregion

                        #region SGST
                        if (result.ToLower() == result1.ToLower())
                        {
                            if (Convert.ToString(dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value) == "")
                            {
                                _cnt += 1;
                                _str += _cnt + ") Please enter proper State/UT tax Amount.\n";
                                dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.Red;
                            }
                            else
                            {
                                if (!Utility.IsICSC(Convert.ToString(dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value)))
                                {
                                    _cnt += 1;
                                    _str += _cnt + ") Please enter proper SGST Amount.\n";
                                    dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.Red;
                                }
                                else
                                { dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.White; }
                            }
                        }
                        else if (Convert.ToString(dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value) != "")
                        {
                            _cnt += 1;
                            _str += _cnt + ") SGST amount is not required in intra state invoice.\n";
                            dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.Red;
                        }
                        else
                        { dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.White; }
                        //else if (Convert.ToString(dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value) == "")
                        //{
                        //    if (!Utility.IsICSC(Convert.ToString(dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value)))
                        //    {
                        //        _cnt += 1;
                        //        _str += _cnt + ") Please enter proper SGST Amount.\n";
                        //        dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.Red;
                        //    }
                        //    else
                        //    { dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.White; }
                        //}
                        #endregion

                        #region CESS Amount
                        if (Convert.ToString(dgvGSTR27Other.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Value) != "")
                        {
                            if (!Utility.IsICSC(Convert.ToString(dgvGSTR27Other.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Value)))
                            {
                                _cnt += 1;
                                _str += _cnt + ") Please enter proper CESS Amount.\n";
                                dgvGSTR27Other.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.Red;
                            }
                            else
                            { dgvGSTR27Other.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.White; }
                        }
                        else
                        { dgvGSTR27Other.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.White; }
                        #endregion
                    }
                }
                #endregion

                #region Actual value and Cumputer value different validation
                /*
                list = null;
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Convert.ToString(x.Cells["colCGSTAmnt"].Value) != "" && Convert.ToString(x.Cells["colSGSTAmnt"].Value) != "" && Convert.ToString(x.Cells["colRate"].Value) != "" && Convert.ToString(x.Cells["colTaxable"].Value) != "")
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        decimal CGST = Convert.ToDecimal(dgvGSTR27Other.Rows[list[i].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value);
                        decimal SGST = Convert.ToDecimal(dgvGSTR27Other.Rows[list[i].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value);
                        decimal Rate = Convert.ToDecimal(dgvGSTR27Other.Rows[list[i].Cells["colRate"].RowIndex].Cells["colRate"].Value);
                        decimal Tax = Convert.ToDecimal(dgvGSTR27Other.Rows[list[i].Cells["colTaxable"].RowIndex].Cells["colTaxable"].Value);

                        decimal ComValue = Tax * Rate / 200;
                        ComValue = Math.Round(ComValue, 2, MidpointRounding.AwayFromZero);
                        decimal ResultCGST = ComValue - CGST;
                        decimal ResultSGST = ComValue - SGST;

                        //if (ResultCGST >= -1 && ResultCGST < 1 && ResultSGST >= -1 && ResultSGST < 1)
                        if (Convert.ToDecimal(dgvGSTR27Other.Rows[list[i].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value) == ComValue && Convert.ToDecimal(dgvGSTR27Other.Rows[list[i].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value) == ComValue)
                        {
                            if (dgvGSTR27Other.Rows[list[i].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor != Color.Red)
                                dgvGSTR27Other.Rows[list[i].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.White;
                            if (dgvGSTR27Other.Rows[list[i].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor != Color.Red)
                                dgvGSTR27Other.Rows[list[i].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.White;
                        }
                        else
                        {
                            dgvGSTR27Other.Rows[list[i].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.Red;
                            dgvGSTR27Other.Rows[list[i].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.Red;
                            _cnt += 1;
                            _str += _cnt + ") Please enter proper CGST Amount and SGST Amount it can be no different value.\n";
                        }
                    }
                }

                list = null;
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Convert.ToString(x.Cells["colIGSTAmnt"].Value) != "" && Convert.ToString(x.Cells["colRate"].Value) != "" && Convert.ToString(x.Cells["colTaxable"].Value) != "")
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        decimal IGST = Convert.ToDecimal(dgvGSTR27Other.Rows[list[i].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value);
                        decimal Rate = Convert.ToDecimal(dgvGSTR27Other.Rows[list[i].Cells["colRate"].RowIndex].Cells["colRate"].Value);
                        decimal Tax = Convert.ToDecimal(dgvGSTR27Other.Rows[list[i].Cells["colTaxable"].RowIndex].Cells["colTaxable"].Value);

                        decimal ComValue = Tax * Rate / 100;
                        ComValue = Math.Round(ComValue, 2, MidpointRounding.AwayFromZero);
                        decimal ResultIGST = ComValue - IGST;

                        //if (ResultIGST >= -1 && ResultIGST < 1)
                        if (Convert.ToDecimal(dgvGSTR27Other.Rows[list[i].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value) == ComValue)
                        {
                            if (dgvGSTR27Other.Rows[list[i].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor != Color.Red)
                                dgvGSTR27Other.Rows[list[i].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.White;
                        }
                        else
                        {
                            dgvGSTR27Other.Rows[list[i].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.Red;
                            _cnt += 1;
                            _str += _cnt + ") Please enter proper IGST Amount it can be no different value.\n";
                        }
                    }
                }
                */
                #endregion

                #region Export Without payment Validation
                list = null;
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Convert.ToString(x.Cells["colSupplyType"].Value) == "Export without payment of GST")
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (Convert.ToString(dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value) != "")
                        {
                            _cnt += 1;
                            _str += _cnt + ") IGST amount is not required in exports without payment.\n";
                            dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.Red;
                        }
                        else { dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.White; }

                        if (Convert.ToString(dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value) != "")
                        {
                            _cnt += 1;
                            _str += _cnt + ") CGST amount is not required in exports without payment.\n";
                            dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.Red;
                        }
                        else { dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.White; }

                        if (Convert.ToString(dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value) != "")
                        {
                            _cnt += 1;
                            _str += _cnt + ") CGST amount is not required in exports without payment.\n";
                            dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.Red;
                        }
                        else { dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.White; }

                        if (Convert.ToString(dgvGSTR27Other.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Value) != "")
                        {
                            _cnt += 1;
                            _str += _cnt + ") CESS amount is not required in exports without payment.\n";
                            dgvGSTR27Other.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.Red;
                        }
                        else { dgvGSTR27Other.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.White; }
                    }
                }
                #endregion

                #region Export with payment Validation
                list = null;
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Convert.ToString(x.Cells["colSupplyType"].Value) == "Export with payment of GST")
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (Convert.ToString(dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value) == "")
                        {
                            _cnt += 1;
                            _str += _cnt + ") Please enter proper IGST Amount.\n";
                            dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.Red;
                        }
                        else
                        {
                            if (!Utility.IsICSC(Convert.ToString(dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value)))
                            {
                                _cnt += 1;
                                _str += _cnt + ") Please enter proper IGST Amount.\n";
                                dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.Red;
                            }
                            else
                            { dgvGSTR27Other.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.White; }
                        }

                        if (Convert.ToString(dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value) != "")
                        {
                            _cnt += 1;
                            _str += _cnt + ") CGST amount is not required in SEZ exports with payment and Deemed Export invoice.\n";
                            dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.Red;
                        }
                        else { dgvGSTR27Other.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.White; }

                        if (Convert.ToString(dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value) != "")
                        {
                            _cnt += 1;
                            _str += _cnt + ") CGST amount is not required in SEZ exports with payment and Deemed Export invoice.\n";
                            dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.Red;
                        }
                        else { dgvGSTR27Other.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.White; }

                        if (Convert.ToString(dgvGSTR27Other.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Value) != "")
                        {
                            _cnt += 1;
                            _str += _cnt + ") CESS amount is not required in SEZ exports with payment and Deemed Export invoice.\n";
                            dgvGSTR27Other.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.Red;
                        }
                        else { dgvGSTR27Other.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.White; }
                    }
                }
                #endregion

                #region POS
                list = null;
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Convert.ToString(x.Cells["colSupplyType"].Value) == "B2C Large")
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (Utility.IsValidStateName(Convert.ToString(dgvGSTR27Other.Rows[list[j].Cells["colPlaceOfSupply"].RowIndex].Cells["colPlaceOfSupply"].Value)) == false)
                        {
                            _cnt += 1;
                            _str += _cnt + ") Please select Place of supply.\n";
                            dgvGSTR27Other.Rows[list[j].Cells["colPlaceOfSupply"].RowIndex].Cells["colPlaceOfSupply"].Style.BackColor = Color.Red;
                        }
                        else { dgvGSTR27Other.Rows[list[j].Cells["colPlaceOfSupply"].RowIndex].Cells["colPlaceOfSupply"].Style.BackColor = Color.White; }
                    }
                }

                list = null;
                list = dgvGSTR27Other.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Convert.ToString(x.Cells["colSupplyType"].Value) != "B2C Large")
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (Convert.ToString(dgvGSTR27Other.Rows[list[j].Cells["colPlaceOfSupply"].RowIndex].Cells["colPlaceOfSupply"].Value) == "" || Utility.IsValidStateName(Convert.ToString(dgvGSTR27Other.Rows[list[j].Cells["colPlaceOfSupply"].RowIndex].Cells["colPlaceOfSupply"].Value)) == true)
                        { dgvGSTR27Other.Rows[list[j].Cells["colPlaceOfSupply"].RowIndex].Cells["colPlaceOfSupply"].Style.BackColor = Color.White; }
                        else
                        {
                            _cnt += 1;
                            _str += _cnt + ") Please select Place of supply.\n";
                            dgvGSTR27Other.Rows[list[j].Cells["colPlaceOfSupply"].RowIndex].Cells["colPlaceOfSupply"].Style.BackColor = Color.Red;
                        }
                    }
                }
                #endregion

                #region same Invoice number for Same POS is required

                DataTable dt9 = (DataTable)dgvGSTR27Other.DataSource;
                if (dt9 != null)
                {
                    var result9 = (from row in dt9.AsEnumerable()
                                   group row by new { colDbtCrdtNoteNo = row.Field<string>("colDbtCrdtNoteNo"), colPlaceOfSupply = row.Field<string>("colPlaceOfSupply") } into grp
                                   select new
                                   {
                                       colDbtCrdtNoteNo = grp.Key.colDbtCrdtNoteNo,
                                       colPlaceOfSupply = grp.Key.colPlaceOfSupply,
                                   }).ToList();

                    if (result9 != null && result9.Count > 0)
                    {
                        foreach (var item in result9)
                        {
                            #region Same Invoice no Same pos
                            list = dgvGSTR27Other.Rows
                                    .OfType<DataGridViewRow>()
                                    .Where(x => Convert.ToString(x.Cells["colDbtCrdtNoteNo"].Value) == Convert.ToString(item.colDbtCrdtNoteNo) && Convert.ToString(x.Cells["colPlaceOfSupply"].Value) != Convert.ToString(item.colPlaceOfSupply))
                                    .Select(p => p)
                                    .ToList();

                            if (list != null && list.Count > 0)
                            {
                                for (int i = 0; i < list.Count; i++)
                                {
                                    dgvGSTR27Other.Rows[list[i].Cells["colPlaceOfSupply"].RowIndex].Cells["colPlaceOfSupply"].Style.BackColor = Color.Red;
                                }
                                _cnt += 1;
                                _str += _cnt + ") Same invoice no for different POS is not possible.\n";
                            }
                            #endregion
                        }
                    }
                }
                #endregion

                dgvGSTR27Other.AllowUserToAddRows = true;
                pbGSTR1.Visible = false;

                if (_str != "")
                {
                    CommonHelper.StatusText = "Draft";
                    int _Result = objGSTR7A.InsertValidationFlg("GSTR1", "CDNUR", "false", CommonHelper.SelectedMonth);
                    if (_Result != 1)
                        MessageBox.Show("System error.\nPlease try after sometime! SPQValidation Error", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    DialogResult dialogResult = MessageBox.Show("File Not Validated. Do you want error description in excel?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    if (dialogResult == DialogResult.Yes)
                        ExportExcelForValidatation();

                    return false;
                }
                else
                {
                    int _Result = objGSTR7A.InsertValidationFlg("GSTR1", "CDNUR", "true", CommonHelper.SelectedMonth);
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
                dgvGSTR27Other.AllowUserToAddRows = true;
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
                    if (cNo == "colSupplyType") // Supply type
                    {
                        if (Utility.CDNUR1TypeofExport(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colTypeOfNote") //Debit and Credit
                    {
                        if (Utility.CDNUR1TypesofNote(cellValue))
                            return true;
                        else
                            return false;
                    }
                    //else if (cNo == "colReason")
                    //{
                    //    if (Utility.CDNUR1Reasonissuing(cellValue))
                    //        return true;
                    //    else
                    //        return false;
                    //}
                    else if (cNo == "colPreGST")
                    {
                        if (Utility.CDNUR1PreGSTRegime(cellValue))
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
                    if (cNo == "colRate" || cNo == "colPlaceOfSupply" || cNo == "colElgbForITC" || cNo == "colDbtCrdtNoteDate" || cNo == "colOrginvoiceDate")
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
                    else if (cNo == "colTypeOfNote")
                    {
                        if (!chkCellValue(Convert.ToString(dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                            dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value = Utility.StrCDNUR1TypesofNote(Convert.ToString(dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value));

                        string[] colNo = { "colDbtCrdtNoteNo", "colOrgInvoiceNo", "colOrginvoiceValue", "colTaxable", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };
                        GetTotal(colNo);
                    }
                    //else if (cNo == "colReason")
                    //{
                    //    if (!chkCellValue(Convert.ToString(dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                    //        dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value = Utility.StrCDNUR1Reasonissuing(Convert.ToString(dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value));
                    //}
                    else if (cNo == "colPreGST")
                    {
                        if (!chkCellValue(Convert.ToString(dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                            dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value = Utility.StrCDNUR1PreGSTRegime(Convert.ToString(dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value));
                    }
                    else if (cNo == "colSupplyType")
                    {
                        if (!chkCellValue(Convert.ToString(dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                            dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value = Utility.StrCDNUR1TypeofExport(Convert.ToString(dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value));
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
                                    dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value = Utility.DisplayIndianCurrency(Convert.ToString(Math.Round(Convert.ToDecimal(dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value), 2, MidpointRounding.AwayFromZero)));
                                    dgvGSTR27Other.CellValueChanged += dgvGSTR27Other_CellValueChanged;
                                }
                            }

                            string[] colNo = { cNo };
                            GetTotal(colNo);
                        }
                        else { dgvGSTR27Other.Rows[e.RowIndex].Cells[cNo].Value = ""; }
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
                    if (dr.Index != dgvGSTR27Other.Rows.Count - 1) // DON'T ADD LAST ROW
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
                    Query = "Delete from SPQR1CDNUR where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
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
                    _Result = objGSTR7A.GSTR1DCNURBulkEntry(dt, Convert.ToString(CommonHelper.StatusText));

                    if (_Result == 1)
                    {
                        // TOTAL CALCULATION
                        string[] colGroup = { "colDbtCrdtNoteNo", "colOrgInvoiceNo", "colOrginvoiceValue", "colTaxable", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };
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

                        _Result = objGSTR7A.GSTR1DCNURBulkEntry(dt, "Total");

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
                    Query = "Delete from SPQR1CDNUR where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";

                    // FIRE QUEARY TO DELETE RECORDS
                    _Result = objGSTR2.IUDData(Query);

                    if (_Result == 1)
                    {
                        // IF RECORDS DELETED FROM DATABASE
                        MessageBox.Show("Record Successfully Deleted!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // MAKE FILE STATUS BLANK
                        ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(0);

                        // TOTAL CALCULATION
                        string[] colNo = { "colDbtCrdtNoteNo", "colOrgInvoiceNo", "colOrginvoiceValue", "colTaxable", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };
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
                    string[] colNo = { "colDbtCrdtNoteNo", "colOrgInvoiceNo", "colOrginvoiceValue", "colTaxable", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };
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
                                    if (dt.Columns[i].ColumnName == "colSupplyType")
                                    {
                                        if (Convert.ToString(dr.Cells[i].Value).Trim() == "Export with payment of GST" || Convert.ToString(dr.Cells[i].Value).Trim() == "Export without payment of GST" || Convert.ToString(dr.Cells[i].Value).Trim() == "B2C Large")
                                            rowValue[i] = Convert.ToString(dr.Cells[i].Value);
                                        else
                                            rowValue[i] = "";
                                    }
                                    else if (dt.Columns[i].ColumnName == "colTypeOfNote")
                                    {
                                        if (Convert.ToString(dr.Cells[i].Value).Trim() == "Credit Note" || Convert.ToString(dr.Cells[i].Value).Trim() == "Debit Note" || Convert.ToString(dr.Cells[i].Value).Trim() == "Refund Voucher")
                                            rowValue[i] = Convert.ToString(dr.Cells[i].Value);
                                        else
                                            rowValue[i] = "";
                                    }
                                    else if (dt.Columns[i].ColumnName == "colPreGST")
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
                            string[] colNo = { "colDbtCrdtNoteNo", "colOrgInvoiceNo", "colOrginvoiceValue", "colTaxable", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };
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
                        OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [cdnur$]", con);
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
                        if (dtexcel.Columns.Count >= dgvGSTR27Other.Columns.Count - 2)
                        {
                            for (int k = dtexcel.Columns.Count - 1; k > (dgvGSTR27Other.Columns.Count - 4); k--)
                            {
                                dtexcel.Columns.Remove(dtexcel.Columns[k]);
                            }
                        }
                        dtexcel.AcceptChanges();
                        #endregion

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
                                        //dtexcel.Columns[j].SetOrdinal(dgvGSTR27Other.Columns[i].Index);
                                        dtexcel.Columns[j].SetOrdinal(dgvGSTR27Other.Columns[i - 2].Index);
                                        break;
                                    }
                                }
                                else if (dgvGSTR27Other.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim() == dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim())
                                {
                                    // IF GRID COLUMN PRESENT IN EXCEL THEN ITS INDEX AS PAR GRID COLUMN INDEX
                                    flg = true;
                                    //dtexcel.Columns[j].SetOrdinal(dgvGSTR27Other.Columns[i].Index - 2);
                                    dtexcel.Columns[j].SetOrdinal(dgvGSTR27Other.Columns[i - 2].Index);
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

                        //#region REMOVE UNUSED COLUMN FROM EXCEL
                        //if (dtexcel.Columns.Count >= dgvGSTR27Other.Columns.Count - 2)
                        //{
                        //    for (int i = dtexcel.Columns.Count; i > (dgvGSTR27Other.Columns.Count - 2); i--)
                        //    {
                        //        dtexcel.Columns.Remove(dtexcel.Columns[i - 1]);
                        //    }
                        //}
                        //dtexcel.AcceptChanges();
                        //#endregion

                        dtexcel.Columns.Add("colSequence");
                        dtexcel.Columns[dtexcel.Columns.Count - 1].SetOrdinal(0);
                        dtexcel.Columns.Add("colError");

                        #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME
                        foreach (DataGridViewColumn col in dgvGSTR27Other.Columns)
                        {
                            if (col.Index != 0 && col.Index != 1)
                                dtexcel.Columns[col.Index - 1].ColumnName = col.Name.ToString();
                        }
                        #endregion

                        // ADD CHECK BOX COLUMN TO DATATBLE AND MAKE IT FIRST TABLE COLUMN
                        //dtexcel.Columns.Add(new DataColumn("colSequence"));
                        //dtexcel.Columns["colSequence"].SetOrdinal(0);
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

                            //if (Utility.CDNUR1Reasonissuing(Convert.ToString(dtexcel.Rows[i]["colReason"]).Trim()))
                            //    dtexcel.Rows[i]["colReason"] = Utility.StrCDNUR1Reasonissuing(Convert.ToString(dtexcel.Rows[i]["colReason"]).Trim());
                            //else
                            //    dtexcel.Rows[i]["colReason"] = "";

                            if (Utility.CDNUR1TypesofNote(Convert.ToString(dtexcel.Rows[i]["colTypeOfNote"]).Trim()))
                                dtexcel.Rows[i]["colTypeOfNote"] = Utility.StrCDNUR1TypesofNote(Convert.ToString(dtexcel.Rows[i]["colTypeOfNote"]).Trim());
                            else
                                dtexcel.Rows[i]["colTypeOfNote"] = "";

                            if (Utility.CDNUR1TypeofExport(Convert.ToString(dtexcel.Rows[i]["colSupplyType"]).Trim()))
                                dtexcel.Rows[i]["colSupplyType"] = Utility.StrCDNUR1TypeofExport(Convert.ToString(dtexcel.Rows[i]["colSupplyType"]).Trim());
                            else
                                dtexcel.Rows[i]["colSupplyType"] = "";

                            if (Utility.CDNUR1PreGSTRegime(Convert.ToString(dtexcel.Rows[i]["colPreGST"]).Trim()))
                                dtexcel.Rows[i]["colPreGST"] = Utility.StrCDNUR1PreGSTRegime(Convert.ToString(dtexcel.Rows[i]["colPreGST"]).Trim());
                            else
                                dtexcel.Rows[i]["colPreGST"] = "No";

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
                if (dgvGSTR27Other.Rows.Count > 1)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID

                    pbGSTR1.Visible = true;

                    #region CREATE WORKBOOK AND ASSIGN COLUMNNAME
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook WB = excelApp.Workbooks.Add(Missing.Value);

                    Excel.Worksheet newWS = (Excel.Worksheet)excelApp.Worksheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    newWS.Name = "cdnur";

                    // DELETE UNUSED WORKSHEETS FROM WORKBOOK
                    foreach (Excel.Worksheet ws in WB.Worksheets)
                    {
                        if (ws.Name != "cdnur")
                            ((Excel.Worksheet)WB.Worksheets[ws.Name]).Delete();
                    }

                    // ASSIGN COLUMN HEADER AS PAR THE GRID HEADER
                    for (int i = 2; i < dgvGSTR27Other.Columns.Count; i++)
                    {
                        newWS.Cells[1, i - 1] = dgvGSTR27Other.Columns[i].HeaderText.ToString();

                        ((Excel.Range)newWS.Cells[1, i - 1]).ColumnWidth = 17;
                    }

                    // GET RANGE AND SET DIFFRENT PROPERTIES
                    Excel.Range headerRange = (Excel.Range)newWS.get_Range((Excel.Range)newWS.Cells[1, 1], (Excel.Range)newWS.Cells[1, dgvGSTR27Other.Columns.Count - 2]);
                    headerRange.WrapText = true;
                    headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Calibri";
                    #endregion

                    #region COPY DATA FROM DATATABLE TO ARRAY

                    // CREATE ARRAY TO HOLD THE DATA OF DATATABLE
                    object[,] arr = new object[dgvGSTR27Other.Rows.Count - 1, dgvGSTR27Other.Columns.Count];

                    // ASSIGN DATA TO ARRAY FROM DATATABLE
                    if (CommonHelper.IsLicence)
                    {
                        // FOR LICENECE ALLOWS TO EXPORT ALL RECORDS
                        for (int i = 0; i < dgvGSTR27Other.Rows.Count - 1; i++)
                        {
                            for (int j = 2; j < dgvGSTR27Other.Columns.Count; j++)
                            {
                                if (dgvGSTR27Other.Columns[j].Name == "colDbtCrdtNoteDate" || dgvGSTR27Other.Columns[j].Name == "colOrginvoiceDate")
                                {
                                    try
                                    {
                                        DateTime ss = Convert.ToDateTime(dgvGSTR27Other.Rows[i].Cells[j].Value);
                                        //arr[i, j - 2] = ss.ToString("dd-MM-yyyy").Replace('/', '-');
                                        arr[i, j - 2] = ss;
                                    }
                                    catch (Exception)
                                    {
                                        arr[i, j - 2] = "";
                                    }
                                }
                                else
                                    arr[i, j - 2] = Convert.ToString(dgvGSTR27Other.Rows[i].Cells[j].Value);
                            }
                        }
                    }
                    else
                    {
                        // FOR DEMO ALLOW ONLY 100 RECORDS TO EXPORT
                        for (int i = 0; i < dgvGSTR27Other.Rows.Count - 1; i++)
                        {
                            if (i < 100)
                            {
                                for (int j = 2; j < dgvGSTR27Other.Columns.Count; j++)
                                {
                                    if (dgvGSTR27Other.Columns[j].Name == "colDbtCrdtNoteDate" || dgvGSTR27Other.Columns[j].Name == "colOrginvoiceDate")
                                    {
                                        try
                                        {
                                            DateTime ss = Convert.ToDateTime(dgvGSTR27Other.Rows[i].Cells[j].Value);
                                            //arr[i, j - 2] = ss.ToString("dd-MM-yyyy").Replace('/', '-');
                                            arr[i, j - 2] = ss;
                                        }
                                        catch (Exception)
                                        {
                                            arr[i, j - 2] = "";
                                        }
                                    }
                                    else
                                        arr[i, j - 2] = Convert.ToString(dgvGSTR27Other.Rows[i].Cells[j].Value);
                                }
                            }
                        }
                    }

                    //SET EXCEL RANGE TO PASTE THE DATA
                    Excel.Range top = (Excel.Range)newWS.Cells[2, 1];
                    Excel.Range bottom = (Excel.Range)newWS.Cells[dgvGSTR27Other.Rows.Count, dgvGSTR27Other.Columns.Count];
                    Excel.Range sheetRange = newWS.Range[top, bottom];
                    sheetRange.WrapText = true;
                    sheetRange.Columns.AutoFit();
                    sheetRange.Rows.AutoFit();
                    //sheetRange.NumberFormat = "@";

                    Excel.Range rg = (Excel.Range)sheetRange.Cells[1, 6];
                    rg.EntireColumn.NumberFormat = "dd-MM-yyyy";

                    rg = (Excel.Range)sheetRange.Cells[1, 5];
                    rg.EntireColumn.NumberFormat = "@";

                    rg = (Excel.Range)sheetRange.Cells[1, 7];
                    rg.EntireColumn.NumberFormat = "@";

                    rg = (Excel.Range)sheetRange.Cells[1, 8];
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

        public void ExportExcelForValidatation()
        {
            List<int> listValid = new List<int>();
            try
            {
                if (dgvGSTR27Other.Rows.Count > 1)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID

                    pbGSTR1.Visible = true;

                    #region CREATE WORKBOOK AND ASSIGN COLUMNNAME
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook WB = excelApp.Workbooks.Add(Missing.Value);

                    Excel.Worksheet newWS = (Excel.Worksheet)excelApp.Worksheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    newWS.Name = "cdnur";

                    // DELETE UNUSED WORKSHEETS FROM WORKBOOK
                    foreach (Excel.Worksheet ws in WB.Worksheets)
                    {
                        if (ws.Name != "cdnur")
                            ((Excel.Worksheet)WB.Worksheets[ws.Name]).Delete();
                    }

                    // ASSIGN COLUMN HEADER AS PAR THE GRID HEADER
                    int yy = 1;
                    for (int i = 2; i < dgvGSTR27Other.Columns.Count + 1; i++)
                    {
                        newWS.Cells[1, i - 1] = dgvGSTR27Other.Columns[yy].HeaderText.ToString();

                        ((Excel.Range)newWS.Cells[1, i - 1]).ColumnWidth = 17;
                        yy++;
                    }

                    //Change as per Requirement

                    ((Excel.Range)newWS.Cells[1, 19]).ColumnWidth = 45;
                    // GET RANGE AND SET DIFFRENT PROPERTIES
                    Excel.Range headerRange = (Excel.Range)newWS.get_Range((Excel.Range)newWS.Cells[1, 1], (Excel.Range)newWS.Cells[1, dgvGSTR27Other.Columns.Count]);
                    headerRange.WrapText = false;
                    headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Calibri";
                    #endregion

                    #region COPY DATA FROM DATATABLE TO ARRAY

                    //SET EXCEL RANGE TO PASTE THE DATA
                    bool ExcelValidFlag = false;
                    DataTable dt = new DataTable();
                    foreach (DataGridViewColumn column in dgvGSTR27Other.Columns)
                        dt.Columns.Add(column.Name, typeof(string));

                    for (int k = 0; k < dgvGSTR27Other.Rows.Count; k++)
                    {
                        for (int j = 0; j < dgvGSTR27Other.ColumnCount; j++)
                        {
                            if (dgvGSTR27Other.Rows[k].Cells[j].Style.BackColor == Color.Red)
                            {
                                ExcelValidFlag = true;
                                //sheetRange.Cells[k + 1, j - 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                            }
                        }
                        if (ExcelValidFlag == true)
                        {
                            dt.Rows.Add();
                            int count = dt.Rows.Count - 1;
                            for (int b = 0; b < dgvGSTR27Other.Columns.Count; b++)
                            {
                                dt.Rows[count][b] = dgvGSTR27Other.Rows[k].Cells[b].Value;
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

                    for (int k = 0; k < dgvGSTR27Other.Rows.Count; k++)
                    {
                        string str_error = "";
                        int cnt = 1;
                        for (int j = 0; j < dgvGSTR27Other.ColumnCount; j++)
                        {
                            if (dgvGSTR27Other.Rows[k].Cells[j].Style.BackColor == Color.Red)
                            {
                                ExcelValidFlag = true;
                                sheetRange.Cells[Ab + 1, j].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);

                                if (dgvGSTR27Other.Columns[j].Name == "colOrgInvoiceNo")
                                {
                                    if (dgvGSTR27Other.Rows[k].Cells[j].Value == "")
                                        str_error += cnt + ") " + " Please enter " + dgvGSTR27Other.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") Invoice number max length is 16 And invoice number can consist only(/) and (-). \n";
                                }
                                else if (dgvGSTR27Other.Columns[j].Name == "colOrginvoiceDate")
                                {
                                    if (dgvGSTR27Other.Rows[k].Cells[j].Value == "")
                                        str_error += cnt + ") " + " Please enter " + dgvGSTR27Other.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") " + " Please enter proper " + dgvGSTR27Other.Columns[j].HeaderText + " on this format(dd-MM-YYYY) OR Enter current month Date. \n";
                                }
                                else if (dgvGSTR27Other.Columns[j].Name == "colRate")
                                {
                                    if (dgvGSTR27Other.Rows[k].Cells[j].Value == "")
                                        str_error += cnt + ") " + " Please enter " + dgvGSTR27Other.Columns[j].HeaderText + "(Ex : (0),(2.5),(3),(5),(12),(18),(28)). \n";
                                    else
                                        str_error += cnt + ") " + " Please enter proper " + dgvGSTR27Other.Columns[j].HeaderText + "(Ex : (0),(2.5),(3),(5),(12),(18),(28)). \n";
                                }

                                else if (dgvGSTR27Other.Columns[j].Name == "colIGSTAmnt")
                                {
                                    if (dgvGSTR27Other.Rows[k].Cells[j].Value.ToString() == "")
                                        str_error += cnt + ") " + " Please enter proper " + dgvGSTR27Other.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") " + dgvGSTR27Other.Columns[j].HeaderText + " is not applicable for Intra State. Please enter exact match " + dgvGSTR27Other.Columns[j].HeaderText + " base on `Note/Refund Voucher Value` and `Rate` calculation. \n";
                                }
                                else if (dgvGSTR27Other.Columns[j].Name == "colCGSTAmnt")
                                {
                                    if (dgvGSTR27Other.Rows[k].Cells[j].Value.ToString() == "")
                                        str_error += cnt + ") " + " Please enter proper " + dgvGSTR27Other.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") " + dgvGSTR27Other.Columns[j].HeaderText + " is not applicable for Inter State. Please enter exact match " + dgvGSTR27Other.Columns[j].HeaderText + " base on `Note/Refund Voucher Value` and `Rate` calculation or CGST & SGST values must be same. \n";
                                }
                                else if (dgvGSTR27Other.Columns[j].Name == "colSGSTAmnt")
                                {
                                    if (dgvGSTR27Other.Rows[k].Cells[j].Value.ToString() == "")
                                        str_error += cnt + ") " + " Please enter proper " + dgvGSTR27Other.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") " + dgvGSTR27Other.Columns[j].HeaderText + " is not applicable for Inter State. Please enter exact match " + dgvGSTR27Other.Columns[j].HeaderText + " base on `Note/Refund Voucher Value` and `Rate` calculation or CGST & SGST values must be same. \n";
                                }
                                else if (dgvGSTR27Other.Columns[j].Name == "colCessAmnt")
                                {
                                    if (dgvGSTR27Other.Rows[k].Cells[j].Value.ToString() == "")
                                        str_error += cnt + ") " + " Please enter proper " + dgvGSTR27Other.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") " + dgvGSTR27Other.Columns[j].HeaderText + "is not required in SEZ exports without payment invoice .\n";
                                }
                                else
                                {
                                    str_error += cnt + ") " + " Please enter proper " + dgvGSTR27Other.Columns[j].HeaderText + ".\n";
                                }
                                cnt++;
                            }
                        }
                        if (ExcelValidFlag == true)
                        {
                            Ab++;
                            dt_new.Rows.Add();
                            int c = dt_new.Rows.Count;
                            for (int b = 0; b < dgvGSTR27Other.Columns.Count; b++)
                            {
                                if (dt_new.Columns.Count - 1 == b)
                                {
                                    dt_new.Rows[c - 1][b] = str_error;
                                }
                                else
                                {
                                    dt_new.Rows[c - 1][b] = Convert.ToString(dgvGSTR27Other.Rows[k].Cells[b].Value);
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

                    Excel.Range rg = (Excel.Range)sheetRange.Cells[1, 7];
                    rg.EntireColumn.NumberFormat = "dd-MM-yyyy";

                    rg = (Excel.Range)sheetRange.Cells[1, 6];
                    rg.EntireColumn.NumberFormat = "@";

                    rg = (Excel.Range)sheetRange.Cells[1, 8];
                    rg.EntireColumn.NumberFormat = "@";

                    rg = (Excel.Range)sheetRange.Cells[1, 9];
                    rg.EntireColumn.NumberFormat = "dd-MM-yyyy";

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

                            string[] colNo = { "colDbtCrdtNoteNo", "colOrgInvoiceNo", "colOrginvoiceValue", "colTaxable", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };
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

                if (CommonHelper.CurrentTurnOver != null && Convert.ToString(CommonHelper.CurrentTurnOver).Trim() != "")
                {
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

                                                            //if (chkVal(Convert.ToString(lstDrRate[0]["colReason"]).Trim()))
                                                            //    clsNt.rsn = Convert.ToString(lstDrRate[0]["colReason"]).Trim(); // CDR Reason

                                                            if (!string.IsNullOrEmpty(Convert.ToString(lstDrRate[0]["colOrginvoiceDate"]).Trim()))
                                                                clsNt.idt = Convert.ToString(Convert.ToDateTime(lstDrRate[0]["colOrginvoiceDate"]).ToString("dd-MM-yyyy")); // invoice Date

                                                            //int val = Convert.ToInt32(lstDrRate.Cast<DataRow>().Where(x => Convert.ToString(x["colOrginvoiceValue"]).Trim() != null).Sum(x => Convert.ToString(x["colOrginvoiceValue"]).ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["colOrginvoiceValue"])));
                                                            clsNt.val = Convert.ToDouble(Convert.ToString(lstDrRate[0]["colTaxable"]).Trim()); // val; // Invoice Value

                                                            clsNt.p_gst = GetJsonVal(Convert.ToString(lstDrRate[0]["colPreGST"]).Trim(), "PRE"); // CDR Pre GST

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
                }

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

                                                //if (!string.IsNullOrEmpty(Convert.ToString(Invoicelist[j].Cells["colReason"].Value)))
                                                //    clsNt.rsn = Convert.ToString(Invoicelist[j].Cells["colReason"].Value); // CDR Reason

                                                if (!string.IsNullOrEmpty(Convert.ToString(Invoicelist[j].Cells["colOrginvoiceDate"].Value)))
                                                    clsNt.idt = Convert.ToString(Convert.ToDateTime(Invoicelist[j].Cells["colOrginvoiceDate"].Value).ToString("dd-MM-yyyy")); // invoice Date

                                                int val = Convert.ToInt32(Invoicelist.Cast<DataGridViewRow>().Where(x => x.Cells["colOrginvoiceValue"].Value != null).Sum(x => x.Cells["colOrginvoiceValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colOrginvoiceValue"].Value)));
                                                clsNt.val = val; // Invoice Value

                                                clsNt.p_gst = GetJsonVal(Convert.ToString(Invoicelist[j].Cells["colPreGST"].Value), "PRE"); // CDR Pre GST

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
                    builder.SaveJsonToGSTN(FinalJson, "GSTR1");
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
                ((DataTable)dgvGSTR27Other.DataSource).DefaultView.RowFilter = string.Format("colSequence like '%{0}%' or colSupplyType like '%{0}%' or colParty like '%{0}%' or colTypeOfNote like '%{0}%' or colPreGST like '%{0}%' or colDbtCrdtNoteNo like '%{0}%' or colDbtCrdtNoteDate like '%{0}%' or colOrgInvoiceNo like '%{0}%' or colOrginvoiceDate like '%{0}%' or colRate like '%{0}%' or colTaxable like '%{0}%' or colIGSTAmnt like '%{0}%' or colCGSTAmnt like '%{0}%' or colSGSTAmnt like '%{0}%' or colCessAmnt like '%{0}%' or colPlaceOfSupply like '%{0}%'", txtSearch.Text.Trim().Replace("'", "''"));
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
                string[] colNo = { "colDbtCrdtNoteNo", "colOrgInvoiceNo", "colOrginvoiceValue", "colTaxable", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };
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

        public bool ValidateData(string colName, string colValue)
        {
            bool flg = false;
            colValue = colValue.Trim();
            try
            {
                if (colName == "colTypeOfNote")
                {
                    if (colValue == "Credit Note" || colValue == "Debit Note" || colValue == "Refund Voucher")
                        flg = true;
                }
                else if (colName == "colSupplyType")
                {
                    if (colValue == "Export with payment of GST" || colValue == "Export without payment of GST" || colValue == "B2C Large")
                        flg = true;
                }
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
           // ValidataAndGetGSTIN();
        }

    }
}
