using SPEQTAGST;
using SPEQTAGST.BAL;
using SPEQTAGST.BAL.GSTR1;
using SPEQTAGST.BAL.M264r1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.VisualBasic.FileIO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Web.Script.Serialization;
using System.ComponentModel;
using SPEQTAGST.Usermain;
using System.Globalization;
using ClosedXML.Excel;
using Microsoft.Office.Interop;
using SPEQTAGST.pachsR2;
using SPEQTAGST.cachsR2a;
using SPEQTAGST.rintlcs3b;
//using SPEQTAGST.rpubclasr4;
using SPEQTAGST.rintlclass4a;
//using SPEQTAGST.closexcs;
using SPEQTAGST.sdnfksd;
//using SPEQTAGST.sdclose;
//using SPEQTAGST.abcclose;
using SPEQTAGST.old9close;
namespace SPEQTAGST.xasjbr1
{
    public partial class SPQGSTR1B2B : Form
    {
        r1Publicclass objGSTR5 = new r1Publicclass();

        public SPQGSTR1B2B()
        {
            InitializeComponent();
            // SET GRID PROPERTY
            SetGridViewColor();

            // BIND DATA
            GetData();

            // TOTAL CALCULATION
            string[] colNo = { "colInvoiceNo", "colInvoiceValue", "colTaxableVal", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };
            GetTotal(colNo);
            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName(CommonHelper.ReturnName);

            pbGSTR1.Visible = false;
            BindFilter();

            dgvGSTR15.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dgvGSTR15.EnableHeadersVisualStyles = false;
            dgvGSTR15.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);
            dgvGSTR15.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvGSTR15.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dgvGSTR15Total.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        #region Filter
        private void BindFilter()
        {
            try
            {
                List<colList> lstColumns = new List<colList>();
                for (int i = 0; i < dgvGSTR15.Columns.Count; i++)
                {
                    if (i > 1)
                    {
                        string HeaderText = dgvGSTR15.Columns[i].HeaderText;
                        string Name = dgvGSTR15.Columns[i].Name;
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
                MessageBox.Show("Error : " + ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                string _json = "{\"b2b\":[{ \"ctin\": \"01AABCE2207R1Z5\", \"cfs\": \"Y\", \"inv\": [ { \"flag\": \"A\", \"updby\": \"R\", \"chksum\": \"AflJufPlFStqKBZ\", \"inum\": \"S008400\", \"idt\": \"24-11-2016\", \"val\": 729248.16, \"pos\": \"04\", \"rchrg\": \"N\", \"prs\": \"Y\", \"od_num\": \"DR008400\", \"od_dt\": \"20-11-2016\", \"etin\": \"01AABCE5507R1Z4\", \"itms\": [ { \"num\": 1, \"itm_det\": { \"ty\": \"G\", \"hsn_sc\": \"G1221\", \"txval\": 10000, \"irt\": 3, \"iamt\": 833.33, \"crt\": 4, \"camt\": 500, \"srt\": 5, \"samt\": 900, \"csrt\": 2, \"csamt\": 500 } }, { \"num\": 2, \"itm_det\": { \"ty\": \"S\", \"hsn_sc\": \"S1231\", \"txval\": 10000, \"irt\": 4, \"iamt\": 625.33, \"crt\": 6, \"camt\": 333.33, \"srt\": 5, \"samt\": 900, \"csrt\": 3, \"csamt\": 333.33 } } ] } ] } ] }";
                #endregion

                RootObject obj = JsonConvert.DeserializeObject<RootObject>(_json);

                #region ADD DATATABLE COLUMN
                DataTable dt = new DataTable();

                foreach (DataGridViewColumn col in dgvGSTR15.Columns)
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
                    //dt.Rows.Add();
                    ////ROOT START
                    //dt.Rows[dt.Rows.Count - 1]["colSequence"] = Convert.ToString(i + 1);
                    //dt.Rows[dt.Rows.Count - 1]["colGSTIN"] = Convert.ToString(obj.b2b[i].ctin);
                    ////ROOT END

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
                            //dt.Rows[dt.Rows.Count - 1]["colTax"] = (Convert.ToString(obj.b2b[i].inv[j].pro_ass) == "Y" ? "true" : "false");//TAX
                            dt.Rows[dt.Rows.Count - 1]["colIndSupAttac"] = Convert.ToString(obj.b2b[i].inv[j].rchrg);//INDICATE SUPPLY ATTACK
                            dt.Rows[dt.Rows.Count - 1]["colInvoiceValue"] = Convert.ToString(obj.b2b[i].inv[j].val);//SUPPLYER INVOICE VALUE
                            //INVOICE DATA END

                            ////ITEM DATA START
                            //dt.Rows[dt.Rows.Count - 1]["colInvoiceGoodsServi"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.ty);//GOODS AND SERVICE
                            //dt.Rows[dt.Rows.Count - 1]["colInvoiceHSNSAC"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.hsn_sc);//HSN
                            //dt.Rows[dt.Rows.Count - 1]["colTaxableVal"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.txval);//TAXABLE VALUE
                            //dt.Rows[dt.Rows.Count - 1]["colRate"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.irt);//IGST RATE
                            //dt.Rows[dt.Rows.Count - 1]["colIGSTAmnt"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.iamt);//IGST AMOUNT
                            ////dt.Rows[dt.Rows.Count - 1]["colCGSTRate"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.crt);//CGST RATE
                            ////dt.Rows[dt.Rows.Count - 1]["colCGSTAmnt"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.camt);//CGST AMOUNT
                            ////dt.Rows[dt.Rows.Count - 1]["colSGSTRate"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.srt);//SGST RATE
                            ////dt.Rows[dt.Rows.Count - 1]["colSGSTAmnt"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.samt);//SGST AMOUNT

                            //ITEM DATA END

                            #region New Parameter
                            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].cfs);

                            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].inv[j].flag);
                            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].inv[j].updby);
                            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].inv[j].chksum);

                            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].inv[j].prs);
                            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].inv[j].od_num);
                            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].inv[j].od_dt);
                            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].inv[j].etin);

                            //dt.Rows[dt.Rows.Count - 1]["colCessRate"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.csrt);
                            //dt.Rows[dt.Rows.Count - 1]["colCessAmnt"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.csamt);
                            #endregion
                        }
                    }
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["colSequence"] = Convert.ToString(i + 1);
                }
                dt.AcceptChanges();
                dgvGSTR15.DataSource = dt;
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("Prefill Data Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void GetData()
        {
            try
            {
                // CREATE DATATABLE TO STORE DATABASE DATA //SPQR1B2B
                DataTable dt = new DataTable();
                //string Query = "Select * from SPQR1B2B where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";

                string Query = "Select "+
                    " Fld_Id,Fld_Sequence,Fld_CustomerName,Fld_PartyName,Fld_InvoiceNo,Fld_InvoiceDate,Fld_InvoiceTaxableVal,Fld_IGSTRate,Fld_IGSTAmnt,Fld_CGSTAmnt,Fld_SGSTAmnt," +
                    " Fld_CessAmount,Fld_InvoiceValue,Fld_POS,Fld_IndSupAttac,Fld_InvType,Fld_GSTINofEcom,Fld_FileStatus,Fld_Month,Fld_FinancialYear "+
                    " from SPQR1B2B where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
                Application.DoEvents();

                // GET DATA FROM DATABASE
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count > 0)
                {
                    // ASSIGN FILE STATUS FILED VALUE
                    if (Convert.ToString(dt.Rows[0]["Fld_FileStatus"]).ToLower() == "draft")
                        ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(1);
                    else if (Convert.ToString(dt.Rows[0]["Fld_FileStatus"]).ToLower() == "completed")
                        ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(2);
                    else if (Convert.ToString(dt.Rows[0]["Fld_FileStatus"]).ToLower() == "not-completed")
                        ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(3);

                    dt.Columns.Remove(dt.Columns["Fld_Month"]);
                    dt.Columns.Remove(dt.Columns["Fld_FileStatus"]);
                    dt.Columns.Remove(dt.Columns["Fld_FinancialYear"]);
                    dt.Columns.Remove(dt.Columns[0]);

                    dt.Columns.Add(new DataColumn("colChk"));
                    dt.Columns["colChk"].SetOrdinal(0);
                    dt.Columns.Add(new DataColumn("colError"));

                    // RENAME DATATABLE COLUMN NAME TO DATAGRIDVIEW COLUMN NAME
                    foreach (DataGridViewColumn col in dgvGSTR15.Columns)
                    {
                        dt.Columns[col.Index].ColumnName = col.Name.ToString();
                        col.DataPropertyName = col.Name;
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string ColName = dt.Columns[j].ColumnName;
                            if (ColName == "colInvoiceValue" || ColName == "colTaxableVal" || ColName == "colIGSTAmnt" || ColName == "colCGSTAmnt" || ColName == "colSGSTAmnt" || ColName == "colCessAmnt")
                            dt.Rows[i][j] = Utility.DisplayIndianCurrency(Convert.ToString(dt.Rows[i][j]));
                        }
                    }

                    dt.AcceptChanges();

                    // ASSIGN DATATABLE TO DATA GRID VIEW
                    dgvGSTR15.DataSource = dt;
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
                if (dgvGSTR15.Rows.Count > 1)
                {
                    // IF MAIN GRID HAVING RECORDS

                    if (dgvGSTR15Total.Rows.Count == 0)
                    {
                        #region IF TOTAL GRID HAVING NO RECORD
                        // CREATE TEMPORARY DATATABLE TO STORE COLUMN CALCULATION
                        DataTable dtTotal = new DataTable();

                        // ADD COLUMN AS PAR DATAGRIDVIEW COLUMN
                        foreach (DataGridViewColumn col in dgvGSTR15Total.Columns)
                        {
                            dtTotal.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;
                        }

                        // CREATE DATAROW TO STORE GRID COLUMN CALCULATION
                        DataRow dr = dtTotal.NewRow();
                        dr["colTInvoiceNo"] = dgvGSTR15.Rows.Cast<DataGridViewRow>().Where(x => Convert.ToString(x.Cells["colInvoiceNo"].Value).Trim() != "").GroupBy(x => x.Cells["colInvoiceNo"].Value).Select(x => x.First()).Distinct().Count();

                        //dr["colTInvoiceValue"] = dgvGSTR15.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colInvoiceValue"].Value != null).Sum(x => x.Cells["colInvoiceValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colInvoiceValue"].Value)).ToString();
                        #region Total Invoice Value (Unique by invoice no)
                        DataTable dt = new DataTable();
                        #region ADD DATATABLE COLUMN

                        // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                        foreach (DataGridViewColumn col in dgvGSTR15.Columns)
                        {
                            dt.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;
                        }
                        #endregion
                        #region ASSIGN GRIDVIEW ROWS IN DATATABLE
                        object[] rowValue = new object[dt.Columns.Count];

                        foreach (DataGridViewRow drn in dgvGSTR15.Rows)
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

                        dt = dt.DefaultView.ToTable(true, "colInvoiceNo", "colInvoiceValue");
                        dr["colTInvoiceValue"] = dt.AsEnumerable()
                        .Sum(r => r.Field<string>("colInvoiceValue") == "" ? 0 : Convert.ToDecimal(r.Field<string>("colInvoiceValue")))
                        .ToString();
                        #endregion

                        dr["colTTaxableValue"] = dgvGSTR15.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colTaxableVal"].Value != null).Sum(x => x.Cells["colTaxableVal"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colTaxableVal"].Value)).ToString();

                        dr["colTIGSTAmnt"] = dgvGSTR15.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colIGSTAmnt"].Value != null).Sum(x => x.Cells["colIGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIGSTAmnt"].Value)).ToString();

                        dr["colTCGSTAmnt"] = dgvGSTR15.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCGSTAmnt"].Value != null).Sum(x => x.Cells["colCGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCGSTAmnt"].Value)).ToString();

                        dr["colTSGSTAmnt"] = dgvGSTR15.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colSGSTAmnt"].Value != null).Sum(x => x.Cells["colSGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colSGSTAmnt"].Value)).ToString();

                        dr["colTCessAmnt"] = dgvGSTR15.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCessAmnt"].Value != null).Sum(x => x.Cells["colCessAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCessAmnt"].Value)).ToString();

                        // ADD DATAROW TO DATATABLE
                        dtTotal.Rows.Add(dr);

                        for (int i = 0; i < dtTotal.Rows.Count; i++)
                        {
                            for (int j = 0; j < dtTotal.Columns.Count; j++)
                            {
                                string ColName = dtTotal.Columns[j].ColumnName;
                                if (ColName == "colTInvoiceValue" || ColName == "colTTaxableVal" || ColName == "colTIGSTAmnt" || ColName == "colTCGSTAmnt" || ColName == "colTSGSTAmnt" || ColName == "colTCessAmnt")
                                    dtTotal.Rows[i][j] = Utility.DisplayIndianCurrency(Convert.ToString(dtTotal.Rows[i][j]));
                            }
                        }

                        dtTotal.AcceptChanges();

                        // ASSIGN DATATABLE TO GRID
                        dgvGSTR15Total.DataSource = dtTotal;

                        #endregion
                    }
                    else if (dgvGSTR15Total.Rows.Count == 1)
                    {
                        #region IF TOTAL GRID HAVING ONLY ONE RECORDS

                        // CALCULATE TOTAL ONLY SPECIFIC COLUMN
                        foreach (var item in colNo)
                        {
                            if (item == "colInvoiceNo")
                                dgvGSTR15Total.Rows[0].Cells["colTInvoiceNo"].Value = dgvGSTR15.Rows.Cast<DataGridViewRow>().Where(x => Convert.ToString(x.Cells["colInvoiceNo"].Value).Trim() != "").GroupBy(x => x.Cells["colInvoiceNo"].Value).Select(x => x.First()).Distinct().Count();
                            else if (item == "colInvoiceValue")
                            {
                                //dgvGSTR15Total.Rows[0].Cells["colTInvoiceValue"].Value = dgvGSTR15.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colInvoiceValue"].Value != null).Sum(x => x.Cells["colInvoiceValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colInvoiceValue"].Value)).ToString();
                                #region Total Invoice Value (Unique by invoice no)
                                DataTable dt = new DataTable();
                                #region ADD DATATABLE COLUMN

                                // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                                foreach (DataGridViewColumn col in dgvGSTR15.Columns)
                                {
                                    dt.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }
                                #endregion
                                #region ASSIGN GRIDVIEW ROWS IN DATATABLE
                                object[] rowValue = new object[dt.Columns.Count];

                                foreach (DataGridViewRow drn in dgvGSTR15.Rows)
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

                                dt = dt.DefaultView.ToTable(true, "colInvoiceNo", "colInvoiceValue");
                                dgvGSTR15Total.Rows[0].Cells["colTInvoiceValue"].Value = Utility.DisplayIndianCurrency(dt.AsEnumerable()
                                .Sum(r => r.Field<string>("colInvoiceValue") == "" ? 0 : Convert.ToDecimal(r.Field<string>("colInvoiceValue"))).ToString());
                                #endregion
                            }
                            else if (item == "colTaxableVal")
                                dgvGSTR15Total.Rows[0].Cells["colTTaxableValue"].Value = Utility.DisplayIndianCurrency(dgvGSTR15.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colTaxableVal"].Value != null).Sum(x => x.Cells["colTaxableVal"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colTaxableVal"].Value)).ToString());
                            else if (item == "colIGSTAmnt")
                                dgvGSTR15Total.Rows[0].Cells["colTIGSTAmnt"].Value = Utility.DisplayIndianCurrency(dgvGSTR15.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colIGSTAmnt"].Value != null).Sum(x => x.Cells["colIGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIGSTAmnt"].Value)).ToString());
                            else if (item == "colCGSTAmnt")
                                dgvGSTR15Total.Rows[0].Cells["colTCGSTAmnt"].Value = Utility.DisplayIndianCurrency(dgvGSTR15.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCGSTAmnt"].Value != null).Sum(x => x.Cells["colCGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCGSTAmnt"].Value)).ToString());
                            else if (item == "colSGSTAmnt")
                                dgvGSTR15Total.Rows[0].Cells["colTSGSTAmnt"].Value = Utility.DisplayIndianCurrency(dgvGSTR15.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colSGSTAmnt"].Value != null).Sum(x => x.Cells["colSGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colSGSTAmnt"].Value)).ToString());
                            else if (item == "colCessAmnt")
                                dgvGSTR15Total.Rows[0].Cells["colTCessAmnt"].Value = Utility.DisplayIndianCurrency(dgvGSTR15.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCessAmnt"].Value != null).Sum(x => x.Cells["colCessAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCessAmnt"].Value)).ToString());
                        }
                        #endregion
                    }

                    // SET TOTAL GRID HEIGHT ROW
                    dgvGSTR15Total.Rows[0].Height = 26;
                    dgvGSTR15Total.Rows[0].Cells[0].Value = "TOTAL";
                }
                else
                {
                    // CHECK IF TOTAL GRID HAVING RECORD

                    if (dgvGSTR15Total.Rows.Count >= 0)
                    {
                        #region IF THERE ARE NO RECORDS IN MAIN GRID THEN ASSIGN BLANK DATATABLE TO TOTAL GRID
                        DataTable dtTotal = new DataTable();
                        foreach (DataGridViewColumn col in dgvGSTR15Total.Columns)
                        {
                            dtTotal.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;
                        }
                        dgvGSTR15Total.DataSource = dtTotal;
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

        private void dgvGSTR15_KeyDown(object sender, KeyEventArgs e)
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
                        if (dgvGSTR15.Rows.Count > 0)
                        {
                            // DELETE SELECTED CELL IN GRID
                            foreach (DataGridViewCell oneCell in dgvGSTR15.SelectedCells)
                            {
                                //oneCell.ValueType = typeof(string);
                                // CHECK BOX COLUMN (0,17) DATA DO NOT DELETE
                                if (oneCell.Selected && oneCell.ColumnIndex != 0 && oneCell.ColumnIndex != 19)
                                {
                                    // dgvGSTR15.Columns[oneCell.ColumnIndex].ValueType = typeof(string);
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
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    #endregion

                    // CALCULATE TOTAL
                    string[] colNo = { "colInvoiceNo", "colInvoiceValue", "colTaxableVal", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };
                    GetTotal(colNo);
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
                    if (dgvGSTR15.RowCount > 0) // IF GRID IS NOT NULL AND PAST ONLY ON SELECTED CELLS
                    {
                        foreach (DataGridViewCell oneCell in dgvGSTR15.SelectedCells)
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
                        pbGSTR1.Visible = true;

                        if (line != "")
                        {
                            // disable main grid
                            DisableControls(dgvGSTR15);

                            gRowNo = dgvGSTR15.Rows.Count - 1;
                            int no = tmp;

                            if (iRow > gRowNo - 1)
                            {
                                DataTable dtDGV = new DataTable();
                                //dtDGV = dgvGSTR15.DataSource as DataTable;

                                #region ADD DATATABLE COLUMN

                                // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                                foreach (DataGridViewColumn col in dgvGSTR15.Columns)
                                {
                                    dtDGV.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }
                                #endregion

                                #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                                // CREATE OBJECT ARRAY TO STORE ONE ROW DATA OF MAIN GRID
                                object[] rowValue = new object[dtDGV.Columns.Count];

                                foreach (DataGridViewRow dr in dgvGSTR15.Rows)
                                {
                                    if (dr.Index != dgvGSTR15.Rows.Count - 1) // DON'T ADD LAST ROW
                                    {
                                        // SET CHECK BOX VALUE
                                        rowValue[0] = "False";
                                        for (int i = 1; i < dr.Cells.Count; i++)
                                        {
                                            rowValue[i] = Convert.ToString(dr.Cells[i].Value);
                                        }

                                        //if (Convert.ToString(dr.Cells["colIndSupAttac"].Value).ToLower() == "y" || Convert.ToString(dr.Cells["colIndSupAttac"].Value).ToLower() == "yes")
                                        //    rowValue[dr.Cells.Count - 7] = "Yes";
                                        //else
                                        //    rowValue[dr.Cells.Count - 7] = "No";

                                        //if (Convert.ToString(dr.Cells["colTax"].Value).ToLower() == "1" || Convert.ToString(dr.Cells["colTax"].Value).ToLower() == "yes" || Convert.ToString(dr.Cells["colTax"].Value).ToLower() == "true")
                                        //    rowValue[dr.Cells.Count - 6] = "True";
                                        //else
                                        //    rowValue[dr.Cells.Count - 6] = "False";

                                        rowValue[dr.Cells.Count - 1] = Convert.ToString(dr.Cells[dr.Cells.Count - 1].Value);

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
                                        if (iCol + i < this.dgvGSTR15.ColumnCount && i < 15)
                                        {
                                            // SKIP CHECK BOX COLUMN AND SEQUANCE COLUMN TO PASTE DATA
                                            if (iCol == 0)
                                                oCell = dgvGSTR15[iCol + i + 2, iRow];
                                            else if (iCol == 1)
                                                oCell = dgvGSTR15[iCol + i + 1, iRow];
                                            else
                                                oCell = dgvGSTR15[iCol + i, iRow];

                                            sCells[i] = sCells[i].Trim().Replace(",", "");
                                            if (oCell.ColumnIndex != 0)
                                            {
                                                if (dgvGSTR15.Columns[oCell.ColumnIndex].Name != "colChk" && dgvGSTR15.Columns[oCell.ColumnIndex].Name != "colSequence" && dgvGSTR15.Columns[oCell.ColumnIndex].Name != "colStateList")
                                                {
                                                    #region VALIDATION
                                                    if (sCells[i].ToString().Trim() == "") { dgvGSTR15.Rows[iRow].Cells[oCell.ColumnIndex].Value = DBNull.Value; }
                                                    else
                                                    {
                                                        if ((oCell.ColumnIndex >= 2 && oCell.ColumnIndex <= 13) || oCell.ColumnIndex == 15 || oCell.ColumnIndex == 16)
                                                        {
                                                            if (chkCellValue(sCells[i].Trim(), dgvGSTR15.Columns[oCell.ColumnIndex].Name))
                                                            {
                                                                if (dgvGSTR15.Columns[oCell.ColumnIndex].Name == "colIndSupAttac")
                                                                    dgvGSTR15.Rows[iRow].Cells[oCell.ColumnIndex].Value = Utility.StrreverseCharge(sCells[i]);
                                                                else if (dgvGSTR15.Columns[oCell.ColumnIndex].Name == "colInvType")
                                                                    dgvGSTR15.Rows[iRow].Cells[oCell.ColumnIndex].Value = Utility.Strb2bInvType(sCells[i]);
                                                                else
                                                                    dgvGSTR15.Rows[iRow].Cells[oCell.ColumnIndex].Value = sCells[i].Trim();
                                                            }
                                                            else
                                                            {
                                                                if (dgvGSTR15.Columns[oCell.ColumnIndex].Name == "colPOS")
                                                                    dgvGSTR15.Rows[iRow].Cells[oCell.ColumnIndex].Value = "";
                                                                else
                                                                    dgvGSTR15.Rows[iRow].Cells[oCell.ColumnIndex].Value = DBNull.Value;
                                                            }
                                                        }
                                                        else if (oCell.ColumnIndex == 14)
                                                        {
                                                            if (Convert.ToString(sCells[i].Trim()).ToLower() == "y" || Convert.ToString(sCells[i].Trim()).ToLower() == "yes")
                                                                dgvGSTR15.Rows[iRow].Cells[oCell.ColumnIndex].Value = "Yes";
                                                            else
                                                                dgvGSTR15.Rows[iRow].Cells[oCell.ColumnIndex].Value = "No";
                                                        }
                                                        else { dgvGSTR15.Rows[iRow].Cells[oCell.ColumnIndex].Value = sCells[i].Trim(); }
                                                    }
                                                    #endregion
                                                }
                                            }
                                            else
                                            {
                                                if (iCol > i)
                                                {
                                                    for (int j = oCell.ColumnIndex; j < dgvGSTR15.Columns.Count; j++)
                                                    {
                                                        #region VALIDATION
                                                        if (sCells[i].ToString().Trim() == "") { dgvGSTR15.Rows[iRow].Cells[j].Value = DBNull.Value; }
                                                        else
                                                        {
                                                            if ((j >= 2 && j <= 13) || j == 15 || j == 16)
                                                            {
                                                                if (dgvGSTR15.Columns[j].Name == "colIndSupAttac")
                                                                    dgvGSTR15.Rows[iRow].Cells[j].Value = Utility.StrreverseCharge(sCells[i]);
                                                                else if (dgvGSTR15.Columns[j].Name == "colInvType")
                                                                    dgvGSTR15.Rows[iRow].Cells[j].Value = Utility.Strb2bInvType(sCells[i]);
                                                                else
                                                                    dgvGSTR15.Rows[iRow].Cells[j].Value = sCells[i].Trim();
                                                            }
                                                            else if (j == 14)
                                                            {
                                                                if (Convert.ToString(sCells[i].Trim()).ToLower() == "y" || Convert.ToString(sCells[i].Trim()).ToLower() == "yes")
                                                                    dgvGSTR15.Rows[iRow].Cells[j].Value = "Yes";
                                                                else
                                                                    dgvGSTR15.Rows[iRow].Cells[j].Value = "No";
                                                            }
                                                            else { dgvGSTR15.Rows[iRow].Cells[j].Value = sCells[i].Trim(); }
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
                                                    for (int j = oCell.ColumnIndex; j < dgvGSTR15.Columns.Count; j++)
                                                    {
                                                        #region VALIDATION
                                                        if (sCells[i].ToString().Trim() == "") { dgvGSTR15.Rows[iRow].Cells[j].Value = DBNull.Value; }
                                                        else
                                                        {
                                                            if ((j >= 2 && j <= 13) || j == 15 || j == 16)
                                                            {
                                                                if (dgvGSTR15.Columns[j].Name == "colIndSupAttac")
                                                                    dgvGSTR15.Rows[iRow].Cells[j].Value = Utility.StrreverseCharge(sCells[i]);
                                                                else if (dgvGSTR15.Columns[j].Name == "colInvType")
                                                                    dgvGSTR15.Rows[iRow].Cells[j].Value = Utility.Strb2bInvType(sCells[i]);
                                                                else
                                                                    dgvGSTR15.Rows[iRow].Cells[j].Value = sCells[i].Trim();
                                                            }
                                                            else if (j == 14)
                                                            {
                                                                if (Convert.ToString(sCells[i].Trim()).ToLower() == "y" || Convert.ToString(sCells[i].Trim()).ToLower() == "yes")
                                                                    dgvGSTR15.Rows[iRow].Cells[j].Value = "Yes";
                                                                else
                                                                    dgvGSTR15.Rows[iRow].Cells[j].Value = "No";
                                                            }
                                                            else { dgvGSTR15.Rows[iRow].Cells[j].Value = sCells[i].Trim(); }
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
                    for (int i = 0; i < dgvGSTR15.Rows.Count - 1; i++)
                    {
                        dgvGSTR15.Rows[i].Cells["colSequence"].Value = i + 1;
                    }
                    #endregion

                    // enable control
                    EnableControls(dgvGSTR15);
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
                EnableControls(dgvGSTR15);
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
                DisableControls(dgvGSTR15);

                #region SET DATATABLE
                int cnt = 0, colNo = 0;

                // ASSIGN GRID DATA TO DATATABLE
                DataTable dt = dtDGV;

                if (dt == null)
                {
                    // IF NO RECORD IN GRID THEN CREATE NEW DATATABLE
                    dt = new DataTable();

                    // ADD COLUMN AS PAR MAIN GRID AND SET DATA ACCESS PROPERTY
                    foreach (DataGridViewColumn col in dgvGSTR15.Columns)
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
                                if (iCol + i < this.dgvGSTR15.ColumnCount && colNo < 16)
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
                                        if (dt.Columns[colNo].ColumnName != "colChk" && dt.Columns[colNo].ColumnName != "colStateList")
                                        {
                                            #region VALIDATION
                                            if (sCells[i].ToString().Trim() == "") { dt.Rows[dt.Rows.Count - 1][colNo] = DBNull.Value; }
                                            else
                                            {
                                                if ((colNo >= 2 && colNo <= 13) || colNo == 15 || colNo == 16)
                                                {
                                                    if (chkCellValue(sCells[i].Trim(), dgvGSTR15.Columns[colNo].Name))
                                                    {
                                                        if (dgvGSTR15.Columns[colNo].Name == "colIndSupAttac")
                                                            dt.Rows[dt.Rows.Count - 1][colNo] = Utility.StrreverseCharge(sCells[i]);
                                                        else if (dgvGSTR15.Columns[colNo].Name == "colInvType")
                                                            dt.Rows[dt.Rows.Count - 1][colNo] = Utility.Strb2bInvType(sCells[i]);
                                                        else
                                                            dt.Rows[dt.Rows.Count - 1][colNo] = sCells[i].Trim();
                                                    }
                                                    else
                                                    {
                                                        if (dgvGSTR15.Columns[colNo].Name == "colPOS")
                                                            dt.Rows[dt.Rows.Count - 1][colNo] = "";
                                                        else
                                                            dt.Rows[dt.Rows.Count - 1][colNo] = DBNull.Value;
                                                    }
                                                }
                                                else if (colNo == 14) // reverse charge
                                                {
                                                    if (Convert.ToString(sCells[i].Trim()).ToLower() == "y" || Convert.ToString(sCells[i].Trim()).ToLower() == "yes")
                                                        dt.Rows[dt.Rows.Count - 1][colNo] = "Yes";
                                                    else
                                                        dt.Rows[dt.Rows.Count - 1][colNo] = "No";
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
                                            for (int j = colNo; j < dgvGSTR15.Columns.Count; j++)
                                            {
                                                #region VALIDATION
                                                if (sCells[i].ToString().Trim() == "") { dt.Rows[dt.Rows.Count - 1][j] = DBNull.Value; }
                                                else
                                                {
                                                    if ((j >= 2 && j <= 13) || j == 15 || j == 16)
                                                    {
                                                        if (chkCellValue(sCells[i].Trim(), dgvGSTR15.Columns[j].Name))
                                                        {
                                                            if (dgvGSTR15.Columns[j].Name == "colIndSupAttac")
                                                                dt.Rows[dt.Rows.Count - 1][j] = Utility.StrreverseCharge(sCells[i]);
                                                            else if (dgvGSTR15.Columns[j].Name == "colInvType")
                                                                dt.Rows[dt.Rows.Count - 1][j] = Utility.Strb2bInvType(sCells[i]);
                                                            else
                                                                dt.Rows[dt.Rows.Count - 1][j] = sCells[i].Trim();
                                                        }
                                                        else
                                                        {
                                                            if (dgvGSTR15.Columns[colNo].Name == "colPOS")
                                                                dt.Rows[dt.Rows.Count - 1][j] = "";
                                                            else
                                                                dt.Rows[dt.Rows.Count - 1][j] = DBNull.Value;
                                                        }
                                                    }
                                                    else if (j == 14) // reverse charge
                                                    {
                                                        if (Convert.ToString(sCells[i].Trim()).ToLower() == "y" || Convert.ToString(sCells[i].Trim()).ToLower() == "yes")
                                                            dt.Rows[dt.Rows.Count - 1][j] = "Yes";
                                                        else
                                                            dt.Rows[dt.Rows.Count - 1][j] = "No";
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
                                            for (int j = colNo; j < dgvGSTR15.Columns.Count; j++)
                                            {
                                                #region VALIDATION
                                                if (sCells[i].ToString().Trim() == "") { dt.Rows[dt.Rows.Count - 1][j] = DBNull.Value; }
                                                else
                                                {
                                                    if ((j >= 2 && j <= 13) || j == 15 || j == 16)
                                                    {
                                                        if (chkCellValue(sCells[i].Trim(), dgvGSTR15.Columns[j].Name))
                                                        {
                                                            if (dgvGSTR15.Columns[j].Name == "colIndSupAttac")
                                                                dt.Rows[dt.Rows.Count - 1][j] = Utility.StrreverseCharge(sCells[i]);
                                                            else if (dgvGSTR15.Columns[j].Name == "colInvType")
                                                                dt.Rows[dt.Rows.Count - 1][j] = Utility.Strb2bInvType(sCells[i]);
                                                            else
                                                                dt.Rows[dt.Rows.Count - 1][j] = sCells[i].Trim();
                                                        }
                                                        else
                                                        {
                                                            if (dgvGSTR15.Columns[colNo].Name == "colPOS")
                                                                dt.Rows[dt.Rows.Count - 1][j] = "";
                                                            else
                                                                dt.Rows[dt.Rows.Count - 1][j] = DBNull.Value;
                                                        }
                                                    }
                                                    else if (j == 14) // reverse charge
                                                    {
                                                        if (Convert.ToString(sCells[i].Trim()).ToLower() == "y" || Convert.ToString(sCells[i].Trim()).ToLower() == "yes")
                                                            dt.Rows[dt.Rows.Count - 1][j] = "Yes";
                                                        else
                                                            dt.Rows[dt.Rows.Count - 1][j] = "No";
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
                            dt.Rows[dt.Rows.Count - 1]["colChk"] = "False";
                            dt.Rows[dt.Rows.Count - 1]["colSequence"] = dt.Rows.Count;
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
                            if (ColName == "colInvoiceValue" || ColName == "colTaxableVal" || ColName == "colIGSTAmnt" || ColName == "colCGSTAmnt" || ColName == "colSGSTAmnt" || ColName == "colCessAmnt")
                                dt.Rows[i][j] = Utility.DisplayIndianCurrency(Convert.ToString(dt.Rows[i][j]));
                        }
                    }

                    dgvGSTR15.DataSource = dt;

                }
                // TOTAL CALCULATION
                string[] colGroup = { "colInvoiceNo", "colInvoiceValue", "colTaxableVal", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };
                GetTotal(colGroup);

                pbGSTR1.Visible = false;
                EnableControls(dgvGSTR15);

                #endregion
            }
            catch (Exception ex)
            {
                EnableControls(dgvGSTR15);
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
            try
            {
                int _cnt = 0;
                string _str = "";
                this.pbGSTR1.Visible = true;
                this.dgvGSTR15.CurrentCell = this.dgvGSTR15.Rows[0].Cells[0];
                this.dgvGSTR15.AllowUserToAddRows = false;
                List<DataGridViewRow> list = (
                    from x in this.dgvGSTR15.Rows.OfType<DataGridViewRow>()
                    where !Utility.IsValidGSTN(Convert.ToString(x.Cells["colGSTIN"].Value))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR15.Rows[list[i].Cells["colGSTIN"].RowIndex].Cells["colGSTIN"].Style.BackColor = Color.LightPink;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Please Enter Valid GSTIN .\n");
                }
                list = (
                    from x in this.dgvGSTR15.Rows.OfType<DataGridViewRow>()
                    where Utility.IsValidGSTN(Convert.ToString(x.Cells["colGSTIN"].Value))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    //this.dgvGSTR15.Rows[list[i].Cells["colGSTIN"].RowIndex].Cells["colGSTIN"].Style.BackColor = Color.White;
                    this.dgvGSTR15.Rows[list[i].Cells["colGSTIN"].RowIndex].Cells["colGSTIN"].Style.BackColor = Color.LightGreen;
                }
                list = null;
                list = (
                    from x in this.dgvGSTR15.Rows.OfType<DataGridViewRow>()
                    where !Utility.IsInvoiceNumber(Convert.ToString(x.Cells["colInvoiceNo"].Value))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR15.Rows[list[i].Cells["colInvoiceNo"].RowIndex].Cells["colInvoiceNo"].Style.BackColor = Color.LightPink;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Maximum Lenght of Invoice No can not be more than 16 digit.\n");
                }
                list = (
                    from x in this.dgvGSTR15.Rows.OfType<DataGridViewRow>()
                    where Utility.IsInvoiceNumber(Convert.ToString(x.Cells["colInvoiceNo"].Value))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    this.dgvGSTR15.Rows[list[i].Cells["colInvoiceNo"].RowIndex].Cells["colInvoiceNo"].Style.BackColor = Color.LightGreen;
                }
                var result2 = (
                    from row in ((DataTable)this.dgvGSTR15.DataSource).AsEnumerable()
                    group row by new { colInvoiceNo = row.Field<string>("colInvoiceNo"), colGSTIN = row.Field<string>("colGSTIN") } into grp
                    select new { colInvoiceNo = grp.Key.colInvoiceNo, colGSTIN = grp.Key.colGSTIN }).ToList();
                if ((result2 == null ? false : result2.Count > 0))
                {
                    foreach (var variable in result2)
                    {
                        list = (
                            from x in this.dgvGSTR15.Rows.OfType<DataGridViewRow>()
                            where (Convert.ToString(x.Cells["colInvoiceNo"].Value) != Convert.ToString(variable.colInvoiceNo) ? false : Convert.ToString(x.Cells["colGSTIN"].Value) != Convert.ToString(variable.colGSTIN))
                            select x into p
                            select p).ToList<DataGridViewRow>();
                        if ((list == null ? false : list.Count > 0))
                        {
                            for (i = 0; i < list.Count; i++)
                            {
                                this.dgvGSTR15.Rows[list[i].Cells["colInvoiceNo"].RowIndex].Cells["colInvoiceNo"].Style.BackColor = Color.LightPink;
                            }
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Same invoice Cannot be Issued to Different GSTIN.\n");
                        }
                    }
                }
                if (!CommonHelper.IsQuarter)
                {
                    list = null;
                    list = (
                        from x in this.dgvGSTR15.Rows.OfType<DataGridViewRow>()
                        where !Utility.IsInvoiceDate(Convert.ToString(x.Cells["colInvoiceDate"].Value))
                        select x).ToList<DataGridViewRow>();
                    if (list.Count > 0)
                    {
                        for (i = 0; i < list.Count; i++)
                        {
                            this.dgvGSTR15.Rows[list[i].Cells["colInvoiceDate"].RowIndex].Cells["colInvoiceDate"].Style.BackColor = Color.LightPink;
                        }
                        _cnt++;
                        _str = string.Concat(_str, _cnt, ") Enter Invoice Date in dd-MM-yyyy or and Invoice Date Cannot be beyond the Select Month Last Date.\n");
                    }
                    list = (
                        from x in this.dgvGSTR15.Rows.OfType<DataGridViewRow>()
                        where Utility.IsInvoiceDate(Convert.ToString(x.Cells["colInvoiceDate"].Value))
                        select x).ToList<DataGridViewRow>();
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR15.Rows[list[i].Cells["colInvoiceDate"].RowIndex].Cells["colInvoiceDate"].Style.BackColor = Color.LightGreen;
                    }
                }
                else
                {
                    list = null;
                    list = (
                        from x in this.dgvGSTR15.Rows.OfType<DataGridViewRow>()
                        where !Utility.IsQuarterlyFilingDate(Convert.ToString(x.Cells["colInvoiceDate"].Value))
                        select x).ToList<DataGridViewRow>();
                    if (list.Count > 0)
                    {
                        for (i = 0; i < list.Count; i++)
                        {
                            this.dgvGSTR15.Rows[list[i].Cells["colInvoiceDate"].RowIndex].Cells["colInvoiceDate"].Style.BackColor = Color.LightPink;
                        }
                        _cnt++;
                        _str = string.Concat(_str, _cnt, ") Please enter proper invoice date.\n");
                    }
                    list = (
                        from x in this.dgvGSTR15.Rows.OfType<DataGridViewRow>()
                        where Utility.IsQuarterlyFilingDate(Convert.ToString(x.Cells["colInvoiceDate"].Value))
                        select x).ToList<DataGridViewRow>();
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR15.Rows[list[i].Cells["colInvoiceDate"].RowIndex].Cells["colInvoiceDate"].Style.BackColor = Color.LightGreen;
                    }
                }
                var result12 = (
                    from row in ((DataTable)this.dgvGSTR15.DataSource).AsEnumerable()
                    group row by new { colInvoiceNo = row.Field<string>("colInvoiceNo"), colGSTIN = row.Field<string>("colGSTIN"), colInvoiceDate = row.Field<string>("colInvoiceDate") } into grp
                    select new { colInvoiceNo = grp.Key.colInvoiceNo, colGSTIN = grp.Key.colGSTIN, colInvoiceDate = grp.Key.colInvoiceDate }).ToList();
                if ((result12 == null ? false : result12.Count > 0))
                {
                    foreach (var variable1 in result12)
                    {
                        list = (
                            from x in this.dgvGSTR15.Rows.OfType<DataGridViewRow>()
                            where (!(Convert.ToString(x.Cells["colInvoiceNo"].Value) == Convert.ToString(variable1.colInvoiceNo)) || !(Convert.ToString(x.Cells["colGSTIN"].Value) == Convert.ToString(variable1.colGSTIN)) ? false : Convert.ToString(x.Cells["colInvoiceDate"].Value) != Convert.ToString(variable1.colInvoiceDate))
                            select x into p
                            select p).ToList<DataGridViewRow>();
                        if ((list == null ? false : list.Count > 0))
                        {
                            for (i = 0; i < list.Count; i++)
                            {
                                this.dgvGSTR15.Rows[list[i].Cells["colInvoiceDate"].RowIndex].Cells["colInvoiceDate"].Style.BackColor = Color.LightPink;
                            }
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Same Invoice No. cannot have different Date.\n");
                        }
                    }
                }
                list = null;
                list = (
                    from x in this.dgvGSTR15.Rows.OfType<DataGridViewRow>()
                    where !Utility.IsInvoiceValue(Convert.ToString(x.Cells["colInvoiceValue"].Value))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR15.Rows[list[i].Cells["colInvoiceValue"].RowIndex].Cells["colInvoiceValue"].Style.BackColor = Color.LightPink;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Invoice value must be in Numeric.\n");
                }
                list = (
                    from x in this.dgvGSTR15.Rows.OfType<DataGridViewRow>()
                    where Utility.IsInvoiceValue(Convert.ToString(x.Cells["colInvoiceValue"].Value))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    this.dgvGSTR15.Rows[list[i].Cells["colInvoiceValue"].RowIndex].Cells["colInvoiceValue"].Style.BackColor = Color.LightGreen;
                }
                var result3 = (
                    from row in ((DataTable)this.dgvGSTR15.DataSource).AsEnumerable()
                    group row by new { colInvoiceNo = row.Field<string>("colInvoiceNo"), colInvoiceValue = row.Field<string>("colInvoiceValue"), colGSTIN = row.Field<string>("colGSTIN") } into grp
                    select new { colGSTIN = grp.Key.colGSTIN, colInvoiceNo = grp.Key.colInvoiceNo, colInvoiceValue = grp.Key.colInvoiceValue }).ToList();
                if ((result3 == null ? false : result3.Count > 0))
                {
                    foreach (var variable2 in result3)
                    {
                        list = (
                            from x in this.dgvGSTR15.Rows.OfType<DataGridViewRow>()
                            where (!(Convert.ToString(x.Cells["colGSTIN"].Value) == Convert.ToString(variable2.colGSTIN)) || !(Convert.ToString(x.Cells["colInvoiceNo"].Value) == Convert.ToString(variable2.colInvoiceNo)) ? false : Convert.ToString(x.Cells["colInvoiceValue"].Value) != Convert.ToString(variable2.colInvoiceValue))
                            select x into p
                            select p).ToList<DataGridViewRow>();
                        if ((list == null ? false : list.Count > 0))
                        {
                            for (i = 0; i < list.Count; i++)
                            {
                                this.dgvGSTR15.Rows[list[i].Cells["colInvoiceValue"].RowIndex].Cells["colInvoiceValue"].Style.BackColor = Color.LightPink;
                            }
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Same Invoice No. cannot have different Invoice Value.\n");
                        }
                    }
                }
                list = null;
                list = (
                    from x in this.dgvGSTR15.Rows.OfType<DataGridViewRow>()
                    where !Utility.IsRate(Convert.ToString(x.Cells["colRate"].Value))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR15.Rows[list[i].Cells["colRate"].RowIndex].Cells["colRate"].Style.BackColor = Color.LightPink;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Entered Rate of GST is not Correct.\n");
                }
                list = (
                    from x in this.dgvGSTR15.Rows.OfType<DataGridViewRow>()
                    where Utility.IsRate(Convert.ToString(x.Cells["colRate"].Value))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    this.dgvGSTR15.Rows[list[i].Cells["colRate"].RowIndex].Cells["colRate"].Style.BackColor = Color.LightGreen;
                }
                var result4 = (
                    from row in ((DataTable)this.dgvGSTR15.DataSource).AsEnumerable()
                    group row by new { colInvoiceNo = row.Field<string>("colInvoiceNo"), colInvoiceValue = row.Field<string>("colInvoiceValue"), colGSTIN = row.Field<string>("colGSTIN"), colRate = row.Field<string>("colRate") } into grp
                    select new { colGSTIN = grp.Key.colGSTIN, colRate = grp.Key.colRate, colInvoiceNo = grp.Key.colInvoiceNo, colInvoiceValue = grp.Key.colInvoiceValue }).ToList();
                if ((result4 == null ? false : result4.Count > 0))
                {
                    foreach (var variable3 in result4)
                    {
                        list = (
                            from x in this.dgvGSTR15.Rows.OfType<DataGridViewRow>()
                            where (!(Convert.ToString(x.Cells["colGSTIN"].Value) == Convert.ToString(variable3.colGSTIN)) || !(Convert.ToString(x.Cells["colRate"].Value) == Convert.ToString(variable3.colRate)) || !(Convert.ToString(x.Cells["colInvoiceNo"].Value) == Convert.ToString(variable3.colInvoiceNo)) || !(Convert.ToString(x.Cells["colInvoiceValue"].Value) == Convert.ToString(variable3.colInvoiceValue)) ? false : Convert.ToString(x.Cells["colRate"].Value) != "")
                            select x into p
                            select p).ToList<DataGridViewRow>();
                        if ((list == null ? false : list.Count > 1))
                        {
                            for (i = 0; i < list.Count; i++)
                            {
                                this.dgvGSTR15.Rows[list[i].Cells["colRate"].RowIndex].Cells["colRate"].Style.BackColor = Color.LightPink;
                            }
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Same invoice No. Cannot have different GST %.\n");
                        }
                    }
                }
                list = null;
                list = (
                    from x in this.dgvGSTR15.Rows.OfType<DataGridViewRow>()
                    where !Utility.IsTaxableValue(Convert.ToString(x.Cells["colTaxableVal"].Value))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR15.Rows[list[i].Cells["colTaxableVal"].RowIndex].Cells["colTaxableVal"].Style.BackColor = Color.LightPink;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Please Enter Taxable Value in Numeric.\n");
                }
                list = (
                    from x in this.dgvGSTR15.Rows.OfType<DataGridViewRow>()
                    where Utility.IsTaxableValue(Convert.ToString(x.Cells["colTaxableVal"].Value))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    if (!(Convert.ToDecimal(this.dgvGSTR15.Rows[list[i].Cells["colTaxableVal"].RowIndex].Cells["colTaxableVal"].Value) > Convert.ToDecimal(this.dgvGSTR15.Rows[list[i].Cells["colInvoiceValue"].RowIndex].Cells["colInvoiceValue"].Value)))
                    {
                        this.dgvGSTR15.Rows[list[i].Cells["colTaxableVal"].RowIndex].Cells["colTaxableVal"].Style.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        this.dgvGSTR15.Rows[list[i].Cells["colTaxableVal"].RowIndex].Cells["colTaxableVal"].Style.BackColor = Color.LightPink;
                        _cnt++;
                        _str = string.Concat(_str, _cnt, ") Taxable values can not be more than Invoice value.\n");
                    }
                }
                list = null;
                list = (
                    from x in this.dgvGSTR15.Rows.OfType<DataGridViewRow>()
                    where Convert.ToString(x.Cells["colInvType"].Value).Trim() == "SEZ exports without payment"
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (j = 0; j < list.Count; j++)
                    {
                        if (!(Convert.ToString(this.dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value) != ""))
                        {
                            this.dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") In SEZ Exports without payment of IGST, IGST Amount Not Required.\n");
                            this.dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.LightPink;
                        }
                        if (!(Convert.ToString(this.dgvGSTR15.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value) != ""))
                        {
                            this.dgvGSTR15.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") In SEZ Exports without payment of IGST, CGST Amount Not Required.\n");
                            this.dgvGSTR15.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.LightPink;
                        }
                        if (!(Convert.ToString(this.dgvGSTR15.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value) != ""))
                        {
                            this.dgvGSTR15.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") In SEZ Exports without payment of IGST, SGST Amount Not Required.\n");
                            this.dgvGSTR15.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.LightPink;
                        }
                        if (!(Convert.ToString(this.dgvGSTR15.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Value) != ""))
                        {
                            this.dgvGSTR15.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") In SEZ Exports without payment of IGST, Cess Amount Not Required\n");
                            this.dgvGSTR15.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.LightPink;
                        }
                    }
                }
                list = null;
                list = (
                    from x in this.dgvGSTR15.Rows.OfType<DataGridViewRow>()
                    where Convert.ToString(x.Cells["colInvType"].Value).Trim() == "SEZ Exports with payment"
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (j = 0; j < list.Count; j++)
                    {
                        if (Convert.ToString(this.dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value) == "")
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") In SEZ Export with IGST, Please enter IGST Amount.\n");
                            this.dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.LightPink;
                        }
                        else if (Utility.IsICSC(Convert.ToString(this.dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value).Trim()))
                        {
                            this.dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Please Numeric IGST Amount.\n");
                            this.dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.LightPink;
                        }
                        if (!(Convert.ToString(this.dgvGSTR15.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value) != ""))
                        {
                            this.dgvGSTR15.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") CGST Value is not required in SEZ exports with payment and Deemed Export invoice.\n");
                            this.dgvGSTR15.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.LightPink;
                        }
                        if (!(Convert.ToString(this.dgvGSTR15.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value) != ""))
                        {
                            this.dgvGSTR15.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") SGST Value is not required in SEZ exports with payment and Deemed Export invoice.\n");
                            this.dgvGSTR15.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.LightPink;
                        }
                        if (!(Convert.ToString(this.dgvGSTR15.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Value) != ""))
                        {
                            this.dgvGSTR15.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") CESS Value is not required in SEZ exports with payment and Deemed Export invoice.\n");
                            this.dgvGSTR15.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.LightPink;
                        }
                    }
                }
                string result = CommonHelper.StateName;
                list = null;
                list = (
                    from x in this.dgvGSTR15.Rows.OfType<DataGridViewRow>()
                    where (Convert.ToString(x.Cells["colInvType"].Value) == "SEZ exports without payment" ? false : Convert.ToString(x.Cells["colInvType"].Value) != "SEZ Exports with payment")
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (j = 0; j < list.Count; j++)
                    {
                        string pgst = Convert.ToString(this.dgvGSTR15.Rows[list[j].Cells["colPOS"].RowIndex].Cells["colPOS"].Value);
                        string result1 = pgst;
                        if (result1 == "Delhi")
                        {
                        }
                        if (!(result.ToLower() != result1.ToLower()))
                        {
                            string check_igst = Convert.ToString(this.dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value);

                            if (check_igst == "" || Convert.ToDecimal(check_igst == "" ? "0" : check_igst) == 0)
                            {
                                this.dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.LightGreen;
                            }
                            //if (!(Convert.ToString(this.dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value) != ""))
                            //{
                            //    this.dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.LightGreen;
                            //}
                            else
                            {
                                _cnt++;
                                _str = string.Concat(_str, _cnt, ") For Inter-State Transaction, Enter IGST only.\n");
                                this.dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.LightPink;
                            }
                        }
                        else if (Convert.ToString(this.dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value) == "")
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Please Enter IGST Amount.\n");
                            this.dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.LightPink;
                        }
                        else if (Utility.IsICSC(Convert.ToString(this.dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value)))
                        {
                            this.dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Please Enter Numeric IGST Amount.\n");
                            this.dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.LightPink;
                        }
                        if (!(result.ToLower() == result1.ToLower()))
                        {

                            string check_cgst = Convert.ToString(this.dgvGSTR15.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value);
                            if (check_cgst == "" || Convert.ToDecimal(check_cgst == "" ? "0" : check_cgst) == 0)
                            {
                                this.dgvGSTR15.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.LightGreen;
                            }

                            //if (!(Convert.ToString(this.dgvGSTR15.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value) != ""))
                            //{
                            //    this.dgvGSTR15.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.LightGreen;
                            //}
                            else
                            {
                                _cnt++;
                                _str = string.Concat(_str, _cnt, ") For Inter-State Transaction, Enter IGST Only.\n");
                                this.dgvGSTR15.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.LightPink;
                            }
                        }
                        else if (Convert.ToString(this.dgvGSTR15.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value) == "")
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Please enter CGST Amount.\n");
                            this.dgvGSTR15.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.LightPink;
                        }
                        else if (Utility.IsICSC(Convert.ToString(this.dgvGSTR15.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value)))
                        {
                            this.dgvGSTR15.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Please enter Numeric CGST Amount.\n");
                            this.dgvGSTR15.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.LightPink;
                        }
                        if (!(result.ToLower() == result1.ToLower()))
                        {
                            string check_sgst = Convert.ToString(this.dgvGSTR15.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value);

                            if (check_sgst == "" || Convert.ToDecimal(check_sgst == "" ? "0" : check_sgst) == 0)
                            {
                                this.dgvGSTR15.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.LightGreen;
                            }

                            //if (!(Convert.ToString(this.dgvGSTR15.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value) != ""))
                            //{
                            //    this.dgvGSTR15.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.LightGreen;
                            //}
                            else
                            {
                                _cnt++;
                                _str = string.Concat(_str, _cnt, ") For Intra-State Transaction, Enter CGST & SGST.\n");
                                this.dgvGSTR15.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.LightPink;
                            }
                        }
                        else if (Convert.ToString(this.dgvGSTR15.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value) == "")
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Please enter proper State/UT GST Amount.\n");
                            this.dgvGSTR15.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.LightPink;
                        }
                        else if (Utility.IsICSC(Convert.ToString(this.dgvGSTR15.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value)))
                        {
                            this.dgvGSTR15.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Please enter proper SGST Amount.\n");
                            this.dgvGSTR15.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.LightPink;
                        }
                        if (!(Convert.ToString(this.dgvGSTR15.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Value) != ""))
                        {
                            this.dgvGSTR15.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.LightGreen;
                        }
                        else if (Utility.IsICSC(Convert.ToString(this.dgvGSTR15.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Value)))
                        {
                            this.dgvGSTR15.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Please enter CESS Amount.\n");
                            this.dgvGSTR15.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.LightPink;
                        }
                    }
                }
                list = null;
                list = (
                    from x in this.dgvGSTR15.Rows.OfType<DataGridViewRow>()
                    where (!(Convert.ToString(x.Cells["colCGSTAmnt"].Value) != Convert.ToString(x.Cells["colSGSTAmnt"].Value)) || !(Convert.ToString(x.Cells["colCGSTAmnt"].Value) != "") ? false : Convert.ToString(x.Cells["colSGSTAmnt"].Value) != "")
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR15.Rows[list[i].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.LightPink;
                        this.dgvGSTR15.Rows[list[i].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.LightPink;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") For Intra-State Transaction CGST and SGST Amount Must be Same.\n");
                }
                list = (
                    from x in this.dgvGSTR15.Rows.OfType<DataGridViewRow>()
                    where (!(Convert.ToString(x.Cells["colCGSTAmnt"].Value) == Convert.ToString(x.Cells["colSGSTAmnt"].Value)) || !(Convert.ToString(x.Cells["colCGSTAmnt"].Value) != "") ? false : Convert.ToString(x.Cells["colSGSTAmnt"].Value) != "")
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    if (this.dgvGSTR15.Rows[list[i].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor != Color.LightPink)
                    {
                        this.dgvGSTR15.Rows[list[i].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.LightGreen;
                    }
                    if (this.dgvGSTR15.Rows[list[i].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor != Color.LightPink)
                    {
                        this.dgvGSTR15.Rows[list[i].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.LightGreen;
                    }
                }
                list = null;
                list = (
                    from x in this.dgvGSTR15.Rows.OfType<DataGridViewRow>()
                    where !Utility.IsValidStateName(Convert.ToString(x.Cells["colPOS"].Value))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR15.Rows[list[i].Cells["colPOS"].RowIndex].Cells["colPOS"].Style.BackColor = Color.LightPink;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Please Choose Correct place of supply.\n");
                }
                list = (
                    from x in this.dgvGSTR15.Rows.OfType<DataGridViewRow>()
                    where Utility.IsValidStateName(Convert.ToString(x.Cells["colPOS"].Value))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    this.dgvGSTR15.Rows[list[i].Cells["colPOS"].RowIndex].Cells["colPOS"].Style.BackColor = Color.LightGreen;
                }
                var result6 = (
                    from row in ((DataTable)this.dgvGSTR15.DataSource).AsEnumerable()
                    group row by new { colInvoiceNo = row.Field<string>("colInvoiceNo"), colGSTIN = row.Field<string>("colGSTIN"), colPOS = row.Field<string>("colPOS") } into grp
                    select new { colInvoiceNo = grp.Key.colInvoiceNo, colGSTIN = grp.Key.colGSTIN, colPOS = grp.Key.colPOS }).ToList();
                if ((result6 == null ? false : result6.Count > 0))
                {
                    foreach (var variable4 in result6)
                    {
                        list = (
                            from x in this.dgvGSTR15.Rows.OfType<DataGridViewRow>()
                            where (!(Convert.ToString(x.Cells["colInvoiceNo"].Value) == Convert.ToString(variable4.colInvoiceNo)) || !(Convert.ToString(x.Cells["colGSTIN"].Value) == Convert.ToString(variable4.colGSTIN)) ? false : Convert.ToString(x.Cells["colPOS"].Value) != Convert.ToString(variable4.colPOS))
                            select x into p
                            select p).ToList<DataGridViewRow>();
                        if ((list == null ? false : list.Count > 0))
                        {
                            for (i = 0; i < list.Count; i++)
                            {
                                this.dgvGSTR15.Rows[list[i].Cells["colPOS"].RowIndex].Cells["colPOS"].Style.BackColor = Color.LightPink;
                            }
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Same GSTIN or Same invoice No. Not allowed for different POS.\n");
                        }
                    }
                }
                list = null;
                list = (
                    from x in this.dgvGSTR15.Rows.OfType<DataGridViewRow>()
                    where !Utility.reverseCharge(Convert.ToString(x.Cells["colIndSupAttac"].Value))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR15.Rows[list[i].Cells["colIndSupAttac"].RowIndex].Cells["colIndSupAttac"].Style.BackColor = Color.LightPink;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Please Choose Invoice Type RCM, Yes / No .\n");
                }
                list = (
                    from x in this.dgvGSTR15.Rows.OfType<DataGridViewRow>()
                    where Utility.reverseCharge(Convert.ToString(x.Cells["colIndSupAttac"].Value))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    this.dgvGSTR15.Rows[list[i].Cells["colIndSupAttac"].RowIndex].Cells["colIndSupAttac"].Style.BackColor = Color.LightGreen;
                }
                list = null;
                list = (
                    from x in this.dgvGSTR15.Rows.OfType<DataGridViewRow>()
                    where !Utility.IsBlankGSTN(Convert.ToString(x.Cells["colGSTINofEcom"].Value))
                    select x).ToList<DataGridViewRow>();
                if ((list == null ? false : list.Count > 0))
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR15.Rows[list[i].Cells["colGSTINofEcom"].RowIndex].Cells["colGSTINofEcom"].Style.BackColor = Color.LightPink;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Please enter GSTIN of E-Commerce.\n");
                }
                list = (
                    from x in this.dgvGSTR15.Rows.OfType<DataGridViewRow>()
                    where Utility.IsBlankGSTN(Convert.ToString(x.Cells["colGSTINofEcom"].Value))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    this.dgvGSTR15.Rows[list[i].Cells["colGSTINofEcom"].RowIndex].Cells["colGSTINofEcom"].Style.BackColor = Color.LightGreen;
                }
                DataTable dsOldinvoice = new DataTable();
                string[] selectedMonth = new string[] { "Select DISTINCT Fld_InvoiceNo from SPQR1B2B where Fld_Month <> '", CommonHelper.SelectedMonth, "' AND Fld_FinancialYear = '", CommonHelper.ReturnYear, "'" };
                string Query = string.Concat(selectedMonth);
                dsOldinvoice = this.objGSTR5.GetDataGSTR1(Query);
                if ((dsOldinvoice == null ? false : dsOldinvoice.Rows.Count > 0))
                {
                    for (int k = 0; k < dsOldinvoice.Rows.Count; k++)
                    {
                        for (i = 0; i < this.dgvGSTR15.RowCount; i++)
                        {
                            if (dsOldinvoice.Rows[k]["Fld_InvoiceNo"].ToString().Trim() == this.dgvGSTR15.Rows[i].Cells["colInvoiceNo"].Value.ToString().Trim())
                            {
                                this.dgvGSTR15.Rows[i].Cells["colInvoiceNo"].Style.BackColor = Color.LightPink;
                            }
                        }
                    }
                }
                this.dgvGSTR15.AllowUserToAddRows = true;
                this.pbGSTR1.Visible = false;
                if (!(_str != ""))
                {
                    if (this.objGSTR5.InsertValidationFlg("GSTR1", "B2B", "true", CommonHelper.SelectedMonth) != 1)
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
                    if (this.objGSTR5.InsertValidationFlg("GSTR1", "B2B", "false", CommonHelper.SelectedMonth) != 1)
                    {
                        MessageBox.Show("System error.\nPlease try after sometime! SPQValidation Error", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    if (MessageBox.Show(" Validation Error in Some Records, Export Error File in excel...!!!?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.Yes)
                    {
                        this.ExportExcelForValidatation();
                    }
                    CommonHelper.StatusText = "Draft";
                    flag = false;
                }
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                this.pbGSTR1.Visible = false;
                this.dgvGSTR15.AllowUserToAddRows = true;
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
                dgvGSTR15.CurrentCell = dgvGSTR15.Rows[0].Cells[0];
                dgvGSTR15.AllowUserToAddRows = false;

                #region GSTN Number
                List<DataGridViewRow> list = dgvGSTR15.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsValidGSTN(Convert.ToString(x.Cells["colGSTIN"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR15.Rows[list[i].Cells["colGSTIN"].RowIndex].Cells["colGSTIN"].Style.BackColor = Color.LightPink;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper GSTIN Number.\n";
                }
                list = dgvGSTR15.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsValidGSTN(Convert.ToString(x.Cells["colGSTIN"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR15.Rows[list[i].Cells["colGSTIN"].RowIndex].Cells["colGSTIN"].Style.BackColor = Color.LightGreen;
                }
                #endregion

                #region Same Invoice no for diffrent Invoice Value
                /*
                DataTable dt4 = (DataTable)dgvGSTR15.DataSource;

                var result5 = (from row in dt4.AsEnumerable()
                               group row by new { colInvoiceNo = row.Field<string>("colInvoiceNo"), colInvoiceValue = row.Field<string>("colInvoiceValue"), colGSTIN = row.Field<string>("colGSTIN") } into grp
                               select new
                               {
                                   colGSTIN = grp.Key.colGSTIN,
                                   colInvoiceNo = grp.Key.colInvoiceNo,
                                   colInvoiceValue = grp.Key.colInvoiceValue,
                               }).ToList();

                if (result5 != null && result5.Count > 0)
                {
                    foreach (var item in result5)
                    {
                        #region Same Invoice no Same Invoice Value
                        list = dgvGSTR15.Rows
                                .OfType<DataGridViewRow>()
                                .Where(x => Convert.ToString(x.Cells["colGSTIN"].Value) != Convert.ToString(item.colGSTIN) && Convert.ToString(x.Cells["colInvoiceNo"].Value) == Convert.ToString(item.colInvoiceNo) && Convert.ToString(x.Cells["colInvoiceValue"].Value) == Convert.ToString(item.colInvoiceValue))
                                .Select(p => p)
                                .ToList();

                        if (list != null && list.Count > 0)
                        {
                            for (int i = 0; i < list.Count; i++)
                            {
                                dgvGSTR15.Rows[list[i].Cells["colGSTIN"].RowIndex].Cells["colGSTIN"].Style.BackColor = Color.LightPink;
                            }
                            _cnt += 1;
                            _str += _cnt + ") Same invoice no for different GSTIN Number is not possible.\n";
                        }
                        #endregion
                    }
                }
                */
                #endregion

                #region Invoice No
                list = null;
                list = dgvGSTR15.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsInvoiceNumber(Convert.ToString(x.Cells["colInvoiceNo"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR15.Rows[list[i].Cells["colInvoiceNo"].RowIndex].Cells["colInvoiceNo"].Style.BackColor = Color.LightPink;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Invoice no can not be more than 16 digit.\n";
                }
                list = dgvGSTR15.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsInvoiceNumber(Convert.ToString(x.Cells["colInvoiceNo"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR15.Rows[list[i].Cells["colInvoiceNo"].RowIndex].Cells["colInvoiceNo"].Style.BackColor = Color.LightGreen;
                }
                #endregion

                DataTable dt = (DataTable)dgvGSTR15.DataSource;

                #region Same Invoice no for diffrent GSTIN

                var result2 = (from row in dt.AsEnumerable()
                               group row by new { colInvoiceNo = row.Field<string>("colInvoiceNo"), colGSTIN = row.Field<string>("colGSTIN") } into grp
                               select new
                               {
                                   colInvoiceNo = grp.Key.colInvoiceNo,
                                   colGSTIN = grp.Key.colGSTIN,
                               }).ToList();

                if (result2 != null && result2.Count > 0)
                {
                    foreach (var item in result2)
                    {
                        #region Same Invoice no Same GSTIN
                        list = dgvGSTR15.Rows
                                .OfType<DataGridViewRow>()
                                .Where(x => Convert.ToString(x.Cells["colInvoiceNo"].Value) == Convert.ToString(item.colInvoiceNo) && Convert.ToString(x.Cells["colGSTIN"].Value) != Convert.ToString(item.colGSTIN))
                                .Select(p => p)
                                .ToList();

                        if (list != null && list.Count > 0)
                        {
                            for (int i = 0; i < list.Count; i++)
                            {
                                dgvGSTR15.Rows[list[i].Cells["colInvoiceNo"].RowIndex].Cells["colInvoiceNo"].Style.BackColor = Color.LightPink;
                            }
                            _cnt += 1;
                            _str += _cnt + ") Same invoice no for different GSTIN no is not possible.\n";
                        }
                        #endregion
                    }
                }

                #endregion

                if (CommonHelper.IsQuarter)
                {
                    #region Invoice Date
                    list = null;
                    list = dgvGSTR15.Rows
                           .OfType<DataGridViewRow>()
                           .Where(x => true != Utility.IsQuarterlyFilingDate(Convert.ToString(x.Cells["colInvoiceDate"].Value)))
                           .Select(x => x)
                           .ToList();
                    if (list.Count > 0)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            dgvGSTR15.Rows[list[i].Cells["colInvoiceDate"].RowIndex].Cells["colInvoiceDate"].Style.BackColor = Color.LightPink;
                        }
                        _cnt += 1;
                        _str += _cnt + ") Please enter proper invoice date.\n";
                    }
                    list = dgvGSTR15.Rows
                           .OfType<DataGridViewRow>()
                           .Where(x => true == Utility.IsQuarterlyFilingDate(Convert.ToString(x.Cells["colInvoiceDate"].Value)))
                           .Select(x => x)
                           .ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR15.Rows[list[i].Cells["colInvoiceDate"].RowIndex].Cells["colInvoiceDate"].Style.BackColor = Color.LightGreen;
                    }
                    #endregion
                }
                else
                {
                    #region Invoice Date
                    list = null;
                    list = dgvGSTR15.Rows
                           .OfType<DataGridViewRow>()
                           .Where(x => true != Utility.IsInvoiceDate(Convert.ToString(x.Cells["colInvoiceDate"].Value)))
                           .Select(x => x)
                           .ToList();
                    if (list.Count > 0)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            dgvGSTR15.Rows[list[i].Cells["colInvoiceDate"].RowIndex].Cells["colInvoiceDate"].Style.BackColor = Color.LightPink;
                        }
                        _cnt += 1;
                        _str += _cnt + ") Please enter proper invoice date.\n";
                    }
                    list = dgvGSTR15.Rows
                           .OfType<DataGridViewRow>()
                           .Where(x => true == Utility.IsInvoiceDate(Convert.ToString(x.Cells["colInvoiceDate"].Value)))
                           .Select(x => x)
                           .ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR15.Rows[list[i].Cells["colInvoiceDate"].RowIndex].Cells["colInvoiceDate"].Style.BackColor = Color.LightGreen;
                    }
                    #endregion
                }

                #region same Invoice number for Same Invoice Date is required
                DataTable dt1 = (DataTable)dgvGSTR15.DataSource;

                var result12 = (from row in dt1.AsEnumerable()
                                group row by new { colInvoiceNo = row.Field<string>("colInvoiceNo"), colGSTIN = row.Field<string>("colGSTIN"), colInvoiceDate = row.Field<string>("colInvoiceDate") } into grp
                                select new
                                {
                                    colInvoiceNo = grp.Key.colInvoiceNo,
                                    colGSTIN = grp.Key.colGSTIN,
                                    colInvoiceDate = grp.Key.colInvoiceDate,
                                }).ToList();

                if (result12 != null && result12.Count > 0)
                {
                    foreach (var item in result12)
                    {
                        #region Same Invoice no Same GSTIN
                        list = dgvGSTR15.Rows
                                .OfType<DataGridViewRow>()
                                .Where(x => Convert.ToString(x.Cells["colInvoiceNo"].Value) == Convert.ToString(item.colInvoiceNo) && Convert.ToString(x.Cells["colGSTIN"].Value) == Convert.ToString(item.colGSTIN) && Convert.ToString(x.Cells["colInvoiceDate"].Value) != Convert.ToString(item.colInvoiceDate))
                                .Select(p => p)
                                .ToList();

                        if (list != null && list.Count > 0)
                        {
                            for (int i = 0; i < list.Count; i++)
                            {
                                dgvGSTR15.Rows[list[i].Cells["colInvoiceDate"].RowIndex].Cells["colInvoiceDate"].Style.BackColor = Color.LightPink;
                            }
                            _cnt += 1;
                            _str += _cnt + ") Same GSTIN no and invoice no for different Invoice Date is not possible.\n";
                        }
                        #endregion
                    }
                }
                #endregion

                #region Invoice Value
                list = null;
                list = dgvGSTR15.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsInvoiceValue(Convert.ToString(x.Cells["colInvoiceValue"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR15.Rows[list[i].Cells["colInvoiceValue"].RowIndex].Cells["colInvoiceValue"].Style.BackColor = Color.LightPink;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper invoice value.\n";
                }
                list = dgvGSTR15.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsInvoiceValue(Convert.ToString(x.Cells["colInvoiceValue"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR15.Rows[list[i].Cells["colInvoiceValue"].RowIndex].Cells["colInvoiceValue"].Style.BackColor = Color.LightGreen;
                }
                #endregion

                #region Same Invoice no for diffrent Invoice Value
                DataTable dt2 = (DataTable)dgvGSTR15.DataSource;

                var result3 = (from row in dt2.AsEnumerable()
                               group row by new { colInvoiceNo = row.Field<string>("colInvoiceNo"), colInvoiceValue = row.Field<string>("colInvoiceValue"), colGSTIN = row.Field<string>("colGSTIN") } into grp
                               select new
                               {
                                   colGSTIN = grp.Key.colGSTIN,
                                   colInvoiceNo = grp.Key.colInvoiceNo,
                                   colInvoiceValue = grp.Key.colInvoiceValue,
                               }).ToList();

                if (result3 != null && result3.Count > 0)
                {
                    foreach (var item in result3)
                    {
                        #region Same Invoice no Same Invoice Value
                        list = dgvGSTR15.Rows
                                .OfType<DataGridViewRow>()
                                .Where(x => Convert.ToString(x.Cells["colGSTIN"].Value) == Convert.ToString(item.colGSTIN) && Convert.ToString(x.Cells["colInvoiceNo"].Value) == Convert.ToString(item.colInvoiceNo) && Convert.ToString(x.Cells["colInvoiceValue"].Value) != Convert.ToString(item.colInvoiceValue))
                                .Select(p => p)
                                .ToList();

                        if (list != null && list.Count > 0)
                        {
                            for (int i = 0; i < list.Count; i++)
                            {
                                dgvGSTR15.Rows[list[i].Cells["colInvoiceValue"].RowIndex].Cells["colInvoiceValue"].Style.BackColor = Color.LightPink;
                            }
                            _cnt += 1;
                            _str += _cnt + ") Same invoice no for different Invoice Value no is not possible.\n";
                        }
                        #endregion
                    }
                }
                #endregion

                #region Rate
                list = null;
                list = dgvGSTR15.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsRate(Convert.ToString(x.Cells["colRate"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR15.Rows[list[i].Cells["colRate"].RowIndex].Cells["colRate"].Style.BackColor = Color.LightPink;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper Rate.\n";
                }
                list = dgvGSTR15.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsRate(Convert.ToString(x.Cells["colRate"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR15.Rows[list[i].Cells["colRate"].RowIndex].Cells["colRate"].Style.BackColor = Color.LightGreen;
                }
                #endregion

                #region Same Invoice no for diffrent Rate
                DataTable dt3 = (DataTable)dgvGSTR15.DataSource;

                var result4 = (from row in dt3.AsEnumerable()
                               group row by new { colInvoiceNo = row.Field<string>("colInvoiceNo"), colInvoiceValue = row.Field<string>("colInvoiceValue"), colGSTIN = row.Field<string>("colGSTIN"), colRate = row.Field<string>("colRate") } into grp
                               select new
                               {
                                   colGSTIN = grp.Key.colGSTIN,
                                   colRate = grp.Key.colRate,
                                   colInvoiceNo = grp.Key.colInvoiceNo,
                                   colInvoiceValue = grp.Key.colInvoiceValue,
                               }).ToList();

                if (result4 != null && result4.Count > 0)
                {
                    foreach (var item in result4)
                    {
                        #region Same Invoice no Same Rate
                        list = dgvGSTR15.Rows
                                .OfType<DataGridViewRow>()
                                .Where(x => Convert.ToString(x.Cells["colGSTIN"].Value) == Convert.ToString(item.colGSTIN) && Convert.ToString(x.Cells["colRate"].Value) == Convert.ToString(item.colRate) && Convert.ToString(x.Cells["colInvoiceNo"].Value) == Convert.ToString(item.colInvoiceNo) && Convert.ToString(x.Cells["colInvoiceValue"].Value) == Convert.ToString(item.colInvoiceValue) && Convert.ToString(x.Cells["colRate"].Value) != "")
                                .Select(p => p)
                                .ToList();

                        if (list != null && list.Count > 1)
                        {
                            for (int i = 0; i < list.Count; i++)
                            {
                                dgvGSTR15.Rows[list[i].Cells["colRate"].RowIndex].Cells["colRate"].Style.BackColor = Color.LightPink;
                            }
                            _cnt += 1;
                            _str += _cnt + ") Same invoice no for different rate is not possible.\n";
                        }
                        else
                        {
                            //for (int i = 0; i < list.Count; i++)
                            //{
                            //    dgvGSTR15.Rows[list[i].Cells["colRate"].RowIndex].Cells["colRate"].Style.BackColor = Color.LightGreen;
                            //}
                        }
                        #endregion
                    }
                }
                #endregion

                #region Taxable Value
                list = null;
                list = dgvGSTR15.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsTaxableValue(Convert.ToString(x.Cells["colTaxableVal"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR15.Rows[list[i].Cells["colTaxableVal"].RowIndex].Cells["colTaxableVal"].Style.BackColor = Color.LightPink;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper Taxable Value.\n";
                }
                list = dgvGSTR15.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsTaxableValue(Convert.ToString(x.Cells["colTaxableVal"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    //dgvGSTR15.Rows[list[i].Cells["colTaxableVal"].RowIndex].Cells["colTaxableVal"].Style.BackColor = Color.LightGreen;
                    if (Convert.ToDecimal(dgvGSTR15.Rows[list[i].Cells["colTaxableVal"].RowIndex].Cells["colTaxableVal"].Value) > Convert.ToDecimal(dgvGSTR15.Rows[list[i].Cells["colInvoiceValue"].RowIndex].Cells["colInvoiceValue"].Value))
                    {
                        dgvGSTR15.Rows[list[i].Cells["colTaxableVal"].RowIndex].Cells["colTaxableVal"].Style.BackColor = Color.LightPink;
                        _cnt += 1;
                        _str += _cnt + ") Taxable values can not be more than Invoice value.\n";
                    }
                    else
                    {
                        dgvGSTR15.Rows[list[i].Cells["colTaxableVal"].RowIndex].Cells["colTaxableVal"].Style.BackColor = Color.LightGreen;
                    }
                }
                #endregion

                #region SEZ Without Validation
                list = null;
                list = dgvGSTR15.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Convert.ToString(x.Cells["colInvType"].Value).Trim() == "SEZ exports without payment")
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (Convert.ToString(dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value) != "")
                        {
                            _cnt += 1;
                            _str += _cnt + ") IGST amount is not required in SEZ exports without payment invoice.\n";
                            dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.LightPink;
                        }
                        else { dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.LightGreen; }

                        if (Convert.ToString(dgvGSTR15.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value) != "")
                        {
                            _cnt += 1;
                            _str += _cnt + ") CGST amount is not required in SEZ exports without payment invoice.\n";
                            dgvGSTR15.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.LightPink;
                        }
                        else { dgvGSTR15.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.LightGreen; }

                        if (Convert.ToString(dgvGSTR15.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value) != "")
                        {
                            _cnt += 1;
                            _str += _cnt + ") CGST amount is not required in SEZ exports without payment invoice.\n";
                            dgvGSTR15.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.LightPink;
                        }
                        else { dgvGSTR15.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.LightGreen; }

                        if (Convert.ToString(dgvGSTR15.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Value) != "")
                        {
                            _cnt += 1;
                            _str += _cnt + ") CESS amount is not required in SEZ exports without payment invoice.\n";
                            dgvGSTR15.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.LightPink;
                        }
                        else { dgvGSTR15.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.LightGreen; }
                    }
                }
                #endregion

                #region SEZ with & Deemed Export Validation
                list = null;
                list = dgvGSTR15.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Convert.ToString(x.Cells["colInvType"].Value).Trim() == "SEZ Exports with payment") // || Convert.ToString(x.Cells["colInvType"].Value).Trim() == "Deemed Exports")
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (Convert.ToString(dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value) == "")
                        {
                            _cnt += 1;
                            _str += _cnt + ") Please enter proper IGST Amount.\n";
                            dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.LightPink;
                        }
                        else
                        {
                            if (!Utility.IsICSC(Convert.ToString(dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value).Trim()))
                            {
                                _cnt += 1;
                                _str += _cnt + ") Please enter proper IGST Amount.\n";
                                dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.LightPink;
                            }
                            else
                            { dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.LightGreen; }
                        }

                        if (Convert.ToString(dgvGSTR15.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value) != "")
                        {
                            _cnt += 1;
                            _str += _cnt + ") CGST amount is not required in SEZ exports with payment and Deemed Export invoice.\n";
                            dgvGSTR15.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.LightPink;
                        }
                        else { dgvGSTR15.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.LightGreen; }

                        if (Convert.ToString(dgvGSTR15.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value) != "")
                        {
                            _cnt += 1;
                            _str += _cnt + ") SGST amount is not required in SEZ exports with payment and Deemed Export invoice.\n";
                            dgvGSTR15.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.LightPink;
                        }
                        else { dgvGSTR15.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.LightGreen; }

                        if (Convert.ToString(dgvGSTR15.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Value) != "")
                        {
                            _cnt += 1;
                            _str += _cnt + ") CESS amount is not required in SEZ exports with payment and Deemed Export invoice.\n";
                            dgvGSTR15.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.LightPink;
                        }
                        else { dgvGSTR15.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.LightGreen; }
                    }
                }
                #endregion

                string gstin = CommonHelper.StateName;
                string result = gstin;

                #region Regular B2b IGST CGST SGST CESS Validation

                list = null;
                list = dgvGSTR15.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Convert.ToString(x.Cells["colInvType"].Value) != "SEZ exports without payment" && Convert.ToString(x.Cells["colInvType"].Value) != "SEZ Exports with payment")// && Convert.ToString(x.Cells["colInvType"].Value) != "Deemed Exports")
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        string pgst = Convert.ToString(dgvGSTR15.Rows[list[j].Cells["colPOS"].RowIndex].Cells["colPOS"].Value);
                        string result1 = pgst;

                        if (result1 == "Delhi")
                        {

                        }

                        #region IGST
                        if (result.ToLower() != result1.ToLower())
                        {
                            if (Convert.ToString(dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value) == "")
                            {
                                _cnt += 1;
                                _str += _cnt + ") Please enter proper IGST Amount.\n";
                                dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.LightPink;
                            }
                            else
                            {
                                if (!Utility.IsICSC(Convert.ToString(dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value)))
                                {
                                    _cnt += 1;
                                    _str += _cnt + ") Please enter proper IGST Amount.\n";
                                    dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.LightPink;
                                }
                                else
                                { dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.LightGreen; }
                            }
                        }
                        else if (Convert.ToString(dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value) != "")
                        {
                            _cnt += 1;
                            _str += _cnt + ") IGST amount is not required in inter state invoice.\n";
                            dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.LightPink;
                        }
                        else
                        { dgvGSTR15.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.LightGreen; }
                        #endregion

                        #region CGST
                        if (result.ToLower() == result1.ToLower())
                        {
                            if (Convert.ToString(dgvGSTR15.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value) == "")
                            {
                                _cnt += 1;
                                _str += _cnt + ") Please enter proper CGST Amount.\n";
                                dgvGSTR15.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.LightPink;
                            }
                            else
                            {
                                if (!Utility.IsICSC(Convert.ToString(dgvGSTR15.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value)))
                                {
                                    _cnt += 1;
                                    _str += _cnt + ") Please enter proper CGST Amount.\n";
                                    dgvGSTR15.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.LightPink;
                                }
                                else
                                { dgvGSTR15.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.LightGreen; }
                            }
                        }
                        else if (Convert.ToString(dgvGSTR15.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value) != "")
                        {
                            _cnt += 1;
                            _str += _cnt + ") CGST amount is not required in intra state invoice.\n";
                            dgvGSTR15.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.LightPink;
                        }
                        else
                        { dgvGSTR15.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.LightGreen; }
                        #endregion

                        #region SGST
                        if (result.ToLower() == result1.ToLower())
                        {
                            if (Convert.ToString(dgvGSTR15.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value) == "")
                            {
                                _cnt += 1;
                                _str += _cnt + ") Please enter proper State/UT tax Amount.\n";
                                dgvGSTR15.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.LightPink;
                            }
                            else
                            {
                                if (!Utility.IsICSC(Convert.ToString(dgvGSTR15.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value)))
                                {
                                    _cnt += 1;
                                    _str += _cnt + ") Please enter proper SGST Amount.\n";
                                    dgvGSTR15.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.LightPink;
                                }
                                else
                                { dgvGSTR15.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.LightGreen; }
                            }
                        }
                        else if (Convert.ToString(dgvGSTR15.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value) != "")
                        {
                            _cnt += 1;
                            _str += _cnt + ") SGST amount is not required in intra state invoice.\n";
                            dgvGSTR15.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.LightPink;
                        }
                        else
                        { dgvGSTR15.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.LightGreen; }
                        #endregion

                        #region CESS Amount
                        if (Convert.ToString(dgvGSTR15.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Value) != "")
                        {
                            if (!Utility.IsICSC(Convert.ToString(dgvGSTR15.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Value)))
                            {
                                _cnt += 1;
                                _str += _cnt + ") Please enter proper CESS Amount.\n";
                                dgvGSTR15.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.LightPink;
                            }
                            else
                            { dgvGSTR15.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.LightGreen; }
                        }
                        else
                        { dgvGSTR15.Rows[list[j].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.LightGreen; }
                        #endregion
                    }
                }
                #endregion

                #region CGST & SGST Amount different validation
                list = null;
                list = dgvGSTR15.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Convert.ToString(x.Cells["colCGSTAmnt"].Value) != Convert.ToString(x.Cells["colSGSTAmnt"].Value) && Convert.ToString(x.Cells["colCGSTAmnt"].Value) != "" && Convert.ToString(x.Cells["colSGSTAmnt"].Value) != "")
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR15.Rows[list[i].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.LightPink;
                        dgvGSTR15.Rows[list[i].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.LightPink;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper CGST Amount and SGST Amount it can be no different value.\n";
                }
                list = dgvGSTR15.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Convert.ToString(x.Cells["colCGSTAmnt"].Value) == Convert.ToString(x.Cells["colSGSTAmnt"].Value) && Convert.ToString(x.Cells["colCGSTAmnt"].Value) != "" && Convert.ToString(x.Cells["colSGSTAmnt"].Value) != "")
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    if (dgvGSTR15.Rows[list[i].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor != Color.LightPink)
                        dgvGSTR15.Rows[list[i].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.LightGreen;
                    if (dgvGSTR15.Rows[list[i].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor != Color.LightPink)
                        dgvGSTR15.Rows[list[i].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.LightGreen;
                }
                #endregion

                #region Actual value and Cumputer value different validation
                /*
                list = null;
                list = dgvGSTR15.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Convert.ToString(x.Cells["colCGSTAmnt"].Value) != "" && Convert.ToString(x.Cells["colSGSTAmnt"].Value) != "" && Convert.ToString(x.Cells["colRate"].Value) != "" && Convert.ToString(x.Cells["colTaxableVal"].Value) != "" && x.Cells["colCGSTAmnt"].Style.BackColor == Color.LightGreen && x.Cells["colSGSTAmnt"].Style.BackColor == Color.LightGreen)
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        decimal CGST = Convert.ToDecimal(dgvGSTR15.Rows[list[i].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value);
                        decimal SGST = Convert.ToDecimal(dgvGSTR15.Rows[list[i].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value);
                        decimal Rate = Convert.ToDecimal(dgvGSTR15.Rows[list[i].Cells["colRate"].RowIndex].Cells["colRate"].Value);
                        decimal Tax = Convert.ToDecimal(dgvGSTR15.Rows[list[i].Cells["colTaxableVal"].RowIndex].Cells["colTaxableVal"].Value);

                        decimal ComValue = Tax * Rate / 200;
                        ComValue = Math.Round(ComValue, 2, MidpointRounding.AwayFromZero);
                        decimal ResultCGST = ComValue - CGST;
                        decimal ResultSGST = ComValue - SGST;

                        //if (ResultCGST >= -1 && ResultCGST < 1 && ResultSGST >= -1 && ResultSGST < 1)
                        if (Convert.ToDecimal(dgvGSTR15.Rows[list[i].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value) == ComValue && Convert.ToDecimal(dgvGSTR15.Rows[list[i].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value) == ComValue)
                        {
                            if (dgvGSTR15.Rows[list[i].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor != Color.LightPink)
                                dgvGSTR15.Rows[list[i].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.LightGreen;
                            if (dgvGSTR15.Rows[list[i].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor != Color.LightPink)
                                dgvGSTR15.Rows[list[i].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            dgvGSTR15.Rows[list[i].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.LightPink;
                            dgvGSTR15.Rows[list[i].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.LightPink;
                            _cnt += 1;
                            _str += _cnt + ") Please enter proper CGST Amount and SGST Amount it can be no different value.\n";
                        }
                    }
                }

                list = null;
                list = dgvGSTR15.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Convert.ToString(x.Cells["colIGSTAmnt"].Value) != "" && Convert.ToString(x.Cells["colRate"].Value) != "" && Convert.ToString(x.Cells["colTaxableVal"].Value) != "" && x.Cells["colIGSTAmnt"].Style.BackColor == Color.LightGreen)
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        decimal IGST = Convert.ToDecimal(dgvGSTR15.Rows[list[i].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value);
                        decimal Rate = Convert.ToDecimal(dgvGSTR15.Rows[list[i].Cells["colRate"].RowIndex].Cells["colRate"].Value);
                        decimal Tax = Convert.ToDecimal(dgvGSTR15.Rows[list[i].Cells["colTaxableVal"].RowIndex].Cells["colTaxableVal"].Value);

                        decimal ComValue = Tax * Rate / 100;
                        ComValue = Math.Round(ComValue, 2, MidpointRounding.AwayFromZero);
                        decimal ResultIGST = ComValue - IGST;

                        //if (ResultIGST >= -1 && ResultIGST < 1)
                        if (Convert.ToDecimal(dgvGSTR15.Rows[list[i].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value) == ComValue)
                        {
                            if (dgvGSTR15.Rows[list[i].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor != Color.LightPink)
                                dgvGSTR15.Rows[list[i].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            dgvGSTR15.Rows[list[i].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.LightPink;
                            _cnt += 1;
                            _str += _cnt + ") Please enter proper IGST Amount it can be no different value.\n";
                        }
                    }
                }
                */
                #endregion

                #region POS
                list = null;
                list = dgvGSTR15.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsValidStateName(Convert.ToString(x.Cells["colPOS"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR15.Rows[list[i].Cells["colPOS"].RowIndex].Cells["colPOS"].Style.BackColor = Color.LightPink;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper place of supply.\n";
                }
                list = dgvGSTR15.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsValidStateName(Convert.ToString(x.Cells["colPOS"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR15.Rows[list[i].Cells["colPOS"].RowIndex].Cells["colPOS"].Style.BackColor = Color.LightGreen;
                }
                #endregion

                #region same Invoice number for Same POS is required

                DataTable dt6 = (DataTable)dgvGSTR15.DataSource;
                var result6 = (from row in dt6.AsEnumerable()
                               group row by new { colInvoiceNo = row.Field<string>("colInvoiceNo"), colGSTIN = row.Field<string>("colGSTIN"), colPOS = row.Field<string>("colPOS") } into grp
                               select new
                               {
                                   colInvoiceNo = grp.Key.colInvoiceNo,
                                   colGSTIN = grp.Key.colGSTIN,
                                   colPOS = grp.Key.colPOS,
                               }).ToList();

                if (result6 != null && result6.Count > 0)
                {
                    foreach (var item in result6)
                    {
                        #region Same Invoice no Same pos
                        list = dgvGSTR15.Rows
                                .OfType<DataGridViewRow>()
                                .Where(x => Convert.ToString(x.Cells["colInvoiceNo"].Value) == Convert.ToString(item.colInvoiceNo) && Convert.ToString(x.Cells["colGSTIN"].Value) == Convert.ToString(item.colGSTIN) && Convert.ToString(x.Cells["colPOS"].Value) != Convert.ToString(item.colPOS))
                                .Select(p => p)
                                .ToList();

                        if (list != null && list.Count > 0)
                        {
                            for (int i = 0; i < list.Count; i++)
                            {
                                dgvGSTR15.Rows[list[i].Cells["colPOS"].RowIndex].Cells["colPOS"].Style.BackColor = Color.LightPink;
                            }
                            _cnt += 1;
                            _str += _cnt + ") Same GSTIN no and invoice no for different POS is not possible.\n";
                        }
                        #endregion
                    }
                }
                #endregion

                #region Reverse Charge
                list = null;
                list = dgvGSTR15.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.reverseCharge(Convert.ToString(x.Cells["colIndSupAttac"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR15.Rows[list[i].Cells["colIndSupAttac"].RowIndex].Cells["colIndSupAttac"].Style.BackColor = Color.LightPink;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper reverse charge.\n";
                }
                list = dgvGSTR15.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.reverseCharge(Convert.ToString(x.Cells["colIndSupAttac"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR15.Rows[list[i].Cells["colIndSupAttac"].RowIndex].Cells["colIndSupAttac"].Style.BackColor = Color.LightGreen;
                }
                #endregion

                #region E-Com GSTIN
                list = null;
                list = dgvGSTR15.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsBlankGSTN(Convert.ToString(x.Cells["colGSTINofEcom"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list != null && list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR15.Rows[list[i].Cells["colGSTINofEcom"].RowIndex].Cells["colGSTINofEcom"].Style.BackColor = Color.LightPink;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper GSTIN of E-Commerce.\n";
                }
                list = dgvGSTR15.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsBlankGSTN(Convert.ToString(x.Cells["colGSTINofEcom"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR15.Rows[list[i].Cells["colGSTINofEcom"].RowIndex].Cells["colGSTINofEcom"].Style.BackColor = Color.LightGreen;
                }
                #endregion

                DataTable dsOldinvoice = new DataTable();
                string Query = "Select DISTINCT Fld_InvoiceNo from SPQR1B2B where Fld_Month <> '" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                //Application.DoEvents();

                // GET DATA FROM DATABASE
                dsOldinvoice = objGSTR5.GetDataGSTR1(Query);
                if (dsOldinvoice != null && dsOldinvoice.Rows.Count > 0)
                {
                    for (int k = 0; k < dsOldinvoice.Rows.Count; k++)
                    {
                        for (int i = 0; i < dgvGSTR15.RowCount; i++)
                        {
                            if (dsOldinvoice.Rows[k]["Fld_InvoiceNo"].ToString().Trim() == dgvGSTR15.Rows[i].Cells["colInvoiceNo"].Value.ToString().Trim())
                            {
                                dgvGSTR15.Rows[i].Cells["colInvoiceNo"].Style.BackColor = Color.LightPink;
                            }
                        }
                    }
                }

                dgvGSTR15.AllowUserToAddRows = true;
                pbGSTR1.Visible = false;

                if (_str != "")
                {
                    CommonHelper.StatusText = "Draft";
                    int _Result = objGSTR5.InsertValidationFlg("GSTR1", "B2B", "false", CommonHelper.SelectedMonth);
                    if (_Result != 1)
                        MessageBox.Show("System error.\nPlease try after sometime! SPQValidation Error", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                    DialogResult dialogResult = MessageBox.Show("File Not Validated. Do you want error description in excel?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    if (dialogResult == DialogResult.Yes)
                        ExportExcelForValidatation();

                    CommonHelper.StatusText = "Draft";
                    return false;
                }
                else
                {
                    int _Result = objGSTR5.InsertValidationFlg("GSTR1", "B2B", "true", CommonHelper.SelectedMonth);
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
                dgvGSTR15.AllowUserToAddRows = true;
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
                    if (cNo == "colGSTIN" || cNo == "colGSTINofEcom") //GSTIN
                    {
                        if (Utility.IsGSTN(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colInvoiceDate")// Date
                    {
                        if (Utility.IsDate(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colInvoiceValue")
                    {
                        if (Utility.IsDecimalOrNumber(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colTaxableVal")// taxable value
                    {
                        if (Utility.IsDecimalOrNumber(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colIGSTAmnt" || cNo == "colCGSTAmnt" || cNo == "colSGSTAmnt" || cNo == "colCessAmnt")
                    {
                        if (Utility.IsDecimalOrNumber(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colRate")  // Rate
                    {
                        if (Utility.IsRate(cellValue))
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
                    else if (cNo == "colInvType")
                    {
                        if (Utility.b2bInvType(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colIndSupAttac") //Reverse Charge
                    {
                        if (Utility.reverseCharge(cellValue))
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

        private void dgvGSTR15_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string cNo = dgvGSTR15.Columns[e.ColumnIndex].Name;

                if (e.RowIndex >= 0)
                {
                    if (cNo == "colGSTIN" || cNo == "colInvoiceDate" || cNo == "colGSTINofEcom" || cNo == "colRate" || cNo == "colPOS")
                    {
                        if (!chkCellValue(Convert.ToString(dgvGSTR15.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                            dgvGSTR15.Rows[e.RowIndex].Cells[cNo].Value = "";

                        if (cNo == "colRate")
                        {
                            if (Convert.ToString(dgvGSTR15.Rows[e.RowIndex].Cells[cNo].Value).Trim() != "")
                            {
                                dgvGSTR15.CellValueChanged -= dgvGSTR15_CellValueChanged;
                                dgvGSTR15.Rows[e.RowIndex].Cells[cNo].Value = Math.Round(Convert.ToDecimal(dgvGSTR15.Rows[e.RowIndex].Cells[cNo].Value), 2, MidpointRounding.AwayFromZero);
                                dgvGSTR15.CellValueChanged += dgvGSTR15_CellValueChanged;
                            }
                        }
                    }
                    else if (cNo == "colIndSupAttac")
                    {
                        if (!chkCellValue(Convert.ToString(dgvGSTR15.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                            dgvGSTR15.Rows[e.RowIndex].Cells[cNo].Value = Utility.StrreverseCharge(Convert.ToString(dgvGSTR15.Rows[e.RowIndex].Cells[cNo].Value));
                    }
                    else if (cNo == "colInvType")
                    {
                        if (!chkCellValue(Convert.ToString(dgvGSTR15.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                            dgvGSTR15.Rows[e.RowIndex].Cells[cNo].Value = Utility.Strb2bInvType(Convert.ToString(dgvGSTR15.Rows[e.RowIndex].Cells[cNo].Value));
                    }
                    else if (cNo == "colInvoiceNo" || cNo == "colInvoiceValue" || cNo == "colTaxableVal" || cNo == "colIGSTAmnt" || cNo == "colCGSTAmnt" || cNo == "colSGSTAmnt" || cNo == "colCessAmnt") // value
                    {
                        if (chkCellValue(Convert.ToString(dgvGSTR15.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                        {
                            if (cNo != "colInvoiceNo")
                            {
                                if (Convert.ToString(dgvGSTR15.Rows[e.RowIndex].Cells[cNo].Value).Trim() != "")
                                {
                                    dgvGSTR15.CellValueChanged -= dgvGSTR15_CellValueChanged;
                                    dgvGSTR15.Rows[e.RowIndex].Cells[cNo].Value = Utility.DisplayIndianCurrency(Convert.ToString(Math.Round(Convert.ToDecimal(dgvGSTR15.Rows[e.RowIndex].Cells[cNo].Value), 2, MidpointRounding.AwayFromZero)));
                                    dgvGSTR15.CellValueChanged += dgvGSTR15_CellValueChanged;
                                }
                            }

                            string[] colNo = { cNo };
                            GetTotal(colNo);
                        }
                        else { dgvGSTR15.Rows[e.RowIndex].Cells[cNo].Value = ""; }
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

                //For text clear before save
                cmbFilter.SelectedIndex = 0;
                txtSearch.Text = "";

                //if (CommonHelper.StatusIndex == 0)
                //{
                //    pbGSTR1.Visible = false;
                //    MessageBox.Show("Please Select File Status!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}

                #region ADD DATATABLE COLUMN

                // CREATE DATATABLE TO STORE MAIN GRID DATA
                DataTable dt = new DataTable();

                // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                foreach (DataGridViewColumn col in dgvGSTR15.Columns)
                {
                    dt.Columns.Add(col.Name.ToString());
                }

                // ADD DATATABLE COLUMN TO STORE FILE STATUS
                dt.Columns.Add("colFileStatus");

                #endregion

                #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                // CREATE OBJECT ARRAY TO STORE ONE ROW DATA OF MAIN GRID
                object[] rowValue = new object[dt.Columns.Count];

                foreach (DataGridViewRow dr in dgvGSTR15.Rows)
                {
                    if (dr.Index != dgvGSTR15.Rows.Count - 1) // DON'T ADD LAST ROW
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
                dt.Columns.Remove(dt.Columns["colChk"]);
                dt.AcceptChanges();
                #endregion

                #region RECORD SAVE
                string Query = "";
                int _Result = 0;

                // CHECK THERE ARE RECORDS IN GRID
                if (dt != null && dt.Rows.Count > 0)
                {
                    #region FIRST DELETE OLD DATA FROM DATABASE
                    Query = "Delete from SPQR1B2B where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                    _Result = objGSTR5.IUDData(Query);
                    if (_Result != 1)
                    {
                        // ERROR OCCURS WHILE DELETING DATA
                        pbGSTR1.Visible = false;
                        MessageBox.Show("System error.\nPlease try after sometime!", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    #endregion

                    // QUERY FIRE TO SAVE RECORDS TO DATABASE
                    _Result = objGSTR5.GSTR15BulkEntry(dt, Convert.ToString(CommonHelper.StatusText));

                    if (_Result == 1)
                    {
                        // TOTAL CALCULATION

                        string[] colNo = { "colInvoiceNo", "colInvoiceValue", "colTaxableVal", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };
                        GetTotal(colNo);

                        #region ADD DATATABLE COLUMN
                        dt = new DataTable();

                        foreach (DataGridViewColumn col in dgvGSTR15.Columns)
                        {
                            dt.Columns.Add(col.Name.ToString());
                        }
                        dt.Columns.Add("colFileStatus");
                        #endregion

                        #region ASSIGN GRIDVIEW ROWS IN DATATABLE
                        object[] rowVal = new object[dt.Columns.Count];

                        if (dgvGSTR15Total.Rows.Count == 1)
                        {
                            foreach (DataGridViewRow dr in dgvGSTR15Total.Rows)
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

                        _Result = objGSTR5.GSTR15BulkEntry(dt, "Total");
                        if (_Result == 1)
                        {
                            pbGSTR1.Visible = false;

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
                    Query = "Delete from SPQR1B2B where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";

                    // FIRE QUEARY TO DELETE RECORDS SPQR1B2B
                    _Result = objGSTR5.IUDData(Query);

                    if (_Result == 1)
                    {
                        // IF RECORDS DELETED FROM DATABASE  
                        pbGSTR1.Visible = false;
                        MessageBox.Show("Record Successfully Deleted!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // MAKE FILE STATUS BLANK
                        ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(0);

                        // TOTAL CALCULATION
                        string[] colNo = { "colInvoiceNo", "colInvoiceValue", "colTaxableVal", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };
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
                if (dgvGSTR15.Rows.Count == 1)
                {
                    ckboxHeader.Checked = false;
                    return;
                }
                if (dgvGSTR15.CurrentCell.RowIndex == 0 && dgvGSTR15.CurrentCell.ColumnIndex == 0)
                {
                    dgvGSTR15.CurrentCell = dgvGSTR15.Rows[0].Cells["colSequence"];
                }
                else { dgvGSTR15.CurrentCell = dgvGSTR15.Rows[0].Cells["colChk"]; }

                // CREATE FLAG FRO DELETE ROWS
                Boolean flgChk = false; Boolean flgSelect = false;

                // CREATE OBJECT OF SELECTED ROW TO DELETE
                List<DataGridViewRow> toDelete = new List<DataGridViewRow>();

                // CHECK THERE ARE RECORD PRESENT IN GRID
                if (dgvGSTR15.Rows.Count > 1)
                {
                    // FLAG TRUE IF CHECK ALL SELECTED
                    if (ckboxHeader.Checked)
                        flgChk = true;

                    #region ADD SELECTED ROW TO OBJECT FOR DELETE
                    for (int i = 0; i < dgvGSTR15.Rows.Count - 1; i++)
                    {
                        if (dgvGSTR15[0, i].Value != null && dgvGSTR15[0, i].Value.ToString() != "")
                        {
                            if (Convert.ToBoolean(dgvGSTR15[0, i].Value) == true)
                            {
                                // ADD ROW TO OBJECT IF ROW IS SELECTED
                                flgSelect = true;
                                toDelete.Add(dgvGSTR15.Rows[i]);
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
                                foreach (DataGridViewColumn col in dgvGSTR15.Columns)
                                {
                                    dt.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }

                                // ASSIGN BLANK DATATABLE TO GRID
                                dgvGSTR15.DataSource = dt;
                            }
                            else if (flgSelect == true)
                            {
                                // DELETE SELECTED ROW
                                foreach (DataGridViewRow row in toDelete)
                                {
                                    dgvGSTR15.Rows.RemoveAt(row.Index);
                                    Application.DoEvents();
                                }
                            }

                            // SEQUANCING MAIN GRID RECORDS
                            for (int i = 0; i < dgvGSTR15.Rows.Count - 1; i++)
                            {
                                dgvGSTR15.Rows[i].Cells["colSequence"].Value = Convert.ToString(i + 1);
                            }

                            if (dgvGSTR15.Rows.Count == 1)
                            {
                                // IF THERE ARE NO RECORDS IN MAIN GRID THENE ASSIGN BLANK DATATABLE TO TOTAL GRID
                                DataTable dtTotal = new DataTable();
                                foreach (DataGridViewColumn col in dgvGSTR15Total.Columns)
                                {
                                    dtTotal.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }
                                dgvGSTR15Total.DataSource = dtTotal;
                            }

                            // SET CONTROL PROPERTY AFTER ROW DELETION
                            ckboxHeader.Checked = false;
                            dgvGSTR15.Columns[0].HeaderText = "Check All";
                            #endregion
                        }
                    }
                    pbGSTR1.Visible = false;

                    // TOTAL CALCULATION
                    string[] colNo = { "colInvoiceNo", "colInvoiceValue", "colTaxableVal", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };

                    GetTotal(colNo);
                }
                else
                {
                    // IF THERE ARE NO RECORD TO DELETE
                    ckboxHeader.Checked = false;
                    dgvGSTR15.Columns[0].HeaderText = "Check All";
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
                        foreach (DataGridViewColumn col in dgvGSTR15.Columns)
                        {
                            dt.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;
                        }
                        #endregion

                        #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                        // CREATE OBJECT ARRAY TO STORE ONE ROW DATA OF MAIN GRID
                        object[] rowValue = new object[dt.Columns.Count];

                        foreach (DataGridViewRow dr in dgvGSTR15.Rows)
                        {
                            if (dr.Index != dgvGSTR15.Rows.Count - 1) // DON'T ADD LAST ROW
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
                                DisableControls(dgvGSTR15);

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
                                foreach (DataGridViewColumn col in dgvGSTR15.Columns)
                                {
                                    dt.Columns[col.Index].ColumnName = col.Name.ToString();
                                    col.DataPropertyName = col.Name;
                                }
                                #endregion

                                //ASSIGN DATATABLE TO DATAGRID
                                dgvGSTR15.DataSource = dt;

                                // ENABLE MAIN GRID
                                EnableControls(dgvGSTR15);
                            }
                            else
                            {
                                // IF THERE ARE NO RECORDS IN MAIN GRID

                                if (dtExcel != null && dtExcel.Rows.Count > 0)
                                {
                                    // IF THERE ARE DATA IN IMPORTED EXCEL FILE

                                    // DISABLE MAIN GRID
                                    DisableControls(dgvGSTR15);

                                    #region RENAME DATATABLE COLUMN NAME AS PAR MAIN GRID
                                    foreach (DataGridViewColumn col in dgvGSTR15.Columns)
                                    {
                                        dtExcel.Columns[col.Index].ColumnName = col.Name.ToString();
                                        col.DataPropertyName = col.Name;
                                    }
                                    dtExcel.AcceptChanges();

                                    // ASSIGN DATATALE TO GRID
                                    dgvGSTR15.DataSource = dtExcel;

                                    // ENABLE MAIN GRID
                                    EnableControls(dgvGSTR15);
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
                            string[] colNo = { "colInvoiceNo", "colInvoiceValue", "colTaxableVal", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };
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
                EnableControls(dgvGSTR15);
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
                        OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [b2b$]", con);
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
                        if (dtexcel.Columns.Count >= dgvGSTR15.Columns.Count - 2)
                        {
                            for (int k = dtexcel.Columns.Count - 1; k > (dgvGSTR15.Columns.Count - 4); k--)
                            {
                                dtexcel.Columns.Remove(dtexcel.Columns[k]);
                            }
                        }
                        dtexcel.AcceptChanges();
                        #endregion

                        flg = false;

                        #region VALIDATE TEMPLATE
                        for (int i = 2; i < dgvGSTR15.Columns.Count; i++)
                        {
                            for (int j = 0; j < dtexcel.Columns.Count; j++)
                            {
                                // CHECK GRID COLUMN IS PRESENT OR NOT IN IMPORTED EXCEL
                                if (dgvGSTR15.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim() == dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim())
                                {
                                    // IF GRID COLUMN PRESENT IN EXCEL THEN ITS INDEX AS PAR GRID COLUMN INDEX
                                    flg = true;
                                    //dtexcel.Columns[j].SetOrdinal(dgvGSTR15.Columns[i].Index - 2);
                                    dtexcel.Columns[j].SetOrdinal(dgvGSTR15.Columns[i - 2].Index);
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
                        //if (dtexcel.Columns.Count >= dgvGSTR15.Columns.Count - 2)
                        //{
                        //    for (int i = dtexcel.Columns.Count; i > (dgvGSTR15.Columns.Count - 2); i--)
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
                        foreach (DataGridViewColumn col in dgvGSTR15.Columns)
                        {
                            if (col.Index != 0)
                                dtexcel.Columns[col.Index - 1].ColumnName = col.Name.ToString();
                        }
                        #endregion

                        // ADD CHECK BOX COLUMN TO DATATBLE AND MAKE IT FIRST TABLE COLUMN
                        //dtexcel.Columns.Add(new DataColumn("colSequence"));
                        //dtexcel.Columns["colSequence"].SetOrdinal(0);
                        dtexcel.Columns.Add(new DataColumn("colChk"));
                        dtexcel.Columns["colChk"].SetOrdinal(0);
                        dtexcel.AcceptChanges();

                        #region SET COLTAX VALUE AS TRUE/FALSE
                        for (int i = 0; i < dtexcel.Rows.Count; i++)
                        {
                            dtexcel.Rows[i]["colChk"] = "False";
                            dtexcel.Rows[i]["colSequence"] = i + 1;

                            if (!Utility.IsValidStateName(Convert.ToString(dtexcel.Rows[i]["colPOS"]).Trim()))
                                dtexcel.Rows[i]["colPOS"] = "";

                            if (Utility.b2bInvType(Convert.ToString(dtexcel.Rows[i]["colInvType"]).Trim()))
                                dtexcel.Rows[i]["colInvType"] = Utility.Strb2bInvType(Convert.ToString(dtexcel.Rows[i]["colInvType"]).Trim());
                            else
                                dtexcel.Rows[i]["colInvType"] = "";

                            if (Utility.reverseCharge(Convert.ToString(dtexcel.Rows[i]["colIndSupAttac"]).Trim()))
                                dtexcel.Rows[i]["colIndSupAttac"] = Utility.StrreverseCharge(Convert.ToString(dtexcel.Rows[i]["colIndSupAttac"]).Trim());
                            else
                                dtexcel.Rows[i]["colIndSupAttac"] = "";

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

        public void ExportExcel()
        {
            try
            {
                if (dgvGSTR15.Rows.Count > 1)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID

                    pbGSTR1.Visible = true;

                    #region CREATE WORKBOOK AND ASSIGN COLUMNNAME
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook WB = excelApp.Workbooks.Add(Missing.Value);

                    Excel.Worksheet newWS = (Excel.Worksheet)excelApp.Worksheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    newWS.Name = "b2b";

                    // DELETE UNUSED WORKSHEETS FROM WORKBOOK
                    foreach (Excel.Worksheet ws in WB.Worksheets)
                    {
                        if (ws.Name != "b2b")
                            ((Excel.Worksheet)WB.Worksheets[ws.Name]).Delete();
                    }

                    // ASSIGN COLUMN HEADER AS PAR THE GRID HEADER
                    for (int i = 2; i < dgvGSTR15.Columns.Count; i++)
                    {
                        newWS.Cells[1, i - 1] = dgvGSTR15.Columns[i].HeaderText.ToString();
                        ((Excel.Range)newWS.Cells[1, i - 1]).ColumnWidth = 17;
                    }

                    // GET RANGE AND SET DIFFRENT PROPERTIES
                    Excel.Range headerRange = (Excel.Range)newWS.get_Range((Excel.Range)newWS.Cells[1, 1], (Excel.Range)newWS.Cells[1, dgvGSTR15.Columns.Count - 2]);
                    headerRange.WrapText = true;
                    headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Calibri";
                    #endregion

                    #region COPY DATA FROM DATATABLE TO ARRAY

                    // CREATE ARRAY TO HOLD THE DATA OF DATATABLE
                    object[,] arr = new object[dgvGSTR15.Rows.Count - 1, dgvGSTR15.Columns.Count];

                    // ASSIGN DATA TO ARRAY FROM DATATABLE
                    if (CommonHelper.IsLicence)
                    {
                        // FOR LICENECE ALLOWS TO EXPORT ALL RECORDS
                        for (int i = 0; i < dgvGSTR15.Rows.Count - 1; i++)
                        {
                            for (int j = 2; j < dgvGSTR15.Columns.Count; j++)
                            {
                                if (dgvGSTR15.Columns[j].Name == "colInvoiceDate")
                                {
                                    try
                                    {
                                        DateTime ss = Convert.ToDateTime(dgvGSTR15.Rows[i].Cells[j].Value);
                                        //arr[i, j - 2] = ss.ToString("dd-MM-yyyy").Replace('/', '-');
                                        arr[i, j - 2] = ss;
                                    }
                                    catch (Exception)
                                    {
                                        arr[i, j - 2] = "";
                                    }
                                }
                                else
                                    arr[i, j - 2] = Convert.ToString(dgvGSTR15.Rows[i].Cells[j].Value);
                            }
                        }
                    }
                    else
                    {
                        // FOR DEMO ALLOW ONLY 100 RECORDS TO EXPORT
                        for (int i = 0; i < dgvGSTR15.Rows.Count - 1; i++)
                        {
                            if (i < 100)
                            {
                                for (int j = 2; j < dgvGSTR15.Columns.Count; j++)
                                {
                                    if (dgvGSTR15.Columns[j].Name == "colInvoiceDate")
                                    {
                                        try
                                        {
                                            DateTime ss = Convert.ToDateTime(dgvGSTR15.Rows[i].Cells[j].Value);
                                            //arr[i, j - 2] = ss.ToString("dd-MM-yyyy").Replace('/', '-');
                                            arr[i, j - 2] = ss;
                                        }
                                        catch (Exception)
                                        {
                                            arr[i, j - 2] = "";
                                        }
                                    }
                                    else
                                        arr[i, j - 2] = Convert.ToString(dgvGSTR15.Rows[i].Cells[j].Value);
                                }
                            }
                        }
                    }

                    //SET EXCEL RANGE TO PASTE THE DATA
                    Excel.Range top = (Excel.Range)newWS.Cells[2, 1];
                    Excel.Range bottom = (Excel.Range)newWS.Cells[dgvGSTR15.Rows.Count, dgvGSTR15.Columns.Count];
                    Excel.Range sheetRange = newWS.Range[top, bottom];
                    sheetRange.WrapText = true;
                    sheetRange.Columns.AutoFit();
                    sheetRange.Rows.AutoFit();
                    //sheetRange.NumberFormat = "@";

                    Excel.Range rg = (Excel.Range)sheetRange.Cells[1, 4];
                    rg.EntireColumn.NumberFormat = "dd-MM-yyyy";

                    rg = (Excel.Range)sheetRange.Cells[1, 3];
                    rg.EntireColumn.NumberFormat = "@";

                    //FILL ARRAY IN EXCEL
                    sheetRange.Value2 = arr;
                    bool ValidFlag = false;
                    for (int i = 0; i < dgvGSTR15.Rows.Count; i++)
                    {
                        for (int j = 0; j < dgvGSTR15.ColumnCount; j++)
                        {
                            if (dgvGSTR15.Rows[i].Cells[j].Style.BackColor == Color.LightPink)
                            {
                                ValidFlag = true;
                                sheetRange.Cells[i + 1, j - 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightPink);
                            }
                        }
                        if (ValidFlag == false)
                        {

                        }
                    }

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
                if (dgvGSTR15.Rows.Count > 1)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID

                    pbGSTR1.Visible = true;

                    #region CREATE WORKBOOK AND ASSIGN COLUMNNAME
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook WB = excelApp.Workbooks.Add(Missing.Value);

                    Excel.Worksheet newWS = (Excel.Worksheet)excelApp.Worksheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    newWS.Name = "b2b";

                    // DELETE UNUSED WORKSHEETS FROM WORKBOOK
                    foreach (Excel.Worksheet ws in WB.Worksheets)
                    {
                        if (ws.Name != "b2b")
                            ((Excel.Worksheet)WB.Worksheets[ws.Name]).Delete();
                    }

                    // ASSIGN COLUMN HEADER AS PAR THE GRID HEADER
                    int yy = 1;
                    for (int i = 2; i < dgvGSTR15.Columns.Count + 1; i++)
                    {
                        newWS.Cells[1, i - 1] = dgvGSTR15.Columns[yy].HeaderText.ToString();

                        ((Excel.Range)newWS.Cells[1, i - 1]).ColumnWidth = 17;
                        yy++;
                    }
                    ((Excel.Range)newWS.Cells[1, 17]).ColumnWidth = 45;
                    //Change as per Requirement


                    // GET RANGE AND SET DIFFRENT PROPERTIES
                    Excel.Range headerRange = (Excel.Range)newWS.get_Range((Excel.Range)newWS.Cells[1, 1], (Excel.Range)newWS.Cells[1, dgvGSTR15.Columns.Count]);
                    headerRange.WrapText = true;
                    headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Calibri";
                    #endregion

                    #region COPY DATA FROM DATATABLE TO ARRAY

                    #region
                    //SET EXCEL RANGE TO PASTE THE DATA
                    bool ExcelValidFlag = false;
                    DataTable dt = new DataTable();
                    foreach (DataGridViewColumn column in dgvGSTR15.Columns)
                        dt.Columns.Add(column.Name, typeof(string));

                    for (int k = 0; k < dgvGSTR15.Rows.Count; k++)
                    {
                        for (int j = 0; j < dgvGSTR15.ColumnCount; j++)
                        {
                            if (dgvGSTR15.Rows[k].Cells[j].Style.BackColor == Color.LightPink)
                            {
                                ExcelValidFlag = true;
                                //sheetRange.Cells[k + 1, j - 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightPink);
                            }
                        }
                        if (ExcelValidFlag == true)
                        {
                            dt.Rows.Add();
                            int count = dt.Rows.Count - 1;
                            for (int b = 0; b < dgvGSTR15.Columns.Count; b++)
                            {
                                dt.Rows[count][b] = dgvGSTR15.Rows[k].Cells[b].Value;
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
                    #endregion

                    for (int k = 0; k < dgvGSTR15.Rows.Count; k++)
                    {
                        string str_error = "";
                        int cnt = 1;
                        for (int j = 0; j < dgvGSTR15.ColumnCount; j++)
                        {
                            if (dgvGSTR15.Rows[k].Cells[j].Style.BackColor == Color.LightPink)
                            {
                                ExcelValidFlag = true;
                                sheetRange.Cells[Ab + 1, j].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightPink);

                                if (dgvGSTR15.Columns[j].Name == "colGSTIN")
                                {
                                    if (dgvGSTR15.Rows[k].Cells[j].Value == "")
                                        str_error += cnt + ") " + " Please enter " + dgvGSTR15.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") Enter 15 Digit GSTN Number OR Enter in proper format. \n";
                                }
                                else if (dgvGSTR15.Columns[j].Name == "colInvoiceNo")
                                {
                                    if (dgvGSTR15.Rows[k].Cells[j].Value == "")
                                        str_error += cnt + ") " + " Please enter " + dgvGSTR15.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") Invoice number max length is 16 And invoice number can consist only(/) and (-). Same invoice number can not possible on different GSTIN. Invoice number already exists. \n";
                                }
                                else if (dgvGSTR15.Columns[j].Name == "colInvoiceDate")
                                {
                                    if (dgvGSTR15.Rows[k].Cells[j].Value == "")
                                        str_error += cnt + ") " + " Please enter " + dgvGSTR15.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") " + " Please enter proper " + dgvGSTR15.Columns[j].HeaderText + " on this format(dd-MM-yyyy) OR Enter current month Date OR Same invoice no same invoice date is required. \n";
                                }
                                else if (dgvGSTR15.Columns[j].Name == "colInvoiceValue")
                                {
                                    if (dgvGSTR15.Rows[k].Cells[j].Value == "")
                                        str_error += cnt + ") " + " Please enter " + dgvGSTR15.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") Different " + dgvGSTR15.Columns[j].HeaderText + " can not be possible for same invoice no. \n";
                                }
                                else if (dgvGSTR15.Columns[j].Name == "colRate")
                                {
                                    if (dgvGSTR15.Rows[k].Cells[j].Value == "")
                                        str_error += cnt + ") " + " Please enter " + dgvGSTR15.Columns[j].HeaderText + "(Ex : (0),(2.5),(3),(5),(12),(18),(28)). \n";
                                    else
                                        str_error += cnt + ") " + " Please enter proper " + dgvGSTR15.Columns[j].HeaderText + "(Ex : (0),(2.5),(3),(5),(12),(18),(28)) or Same rate can not be possible more than one time on same invoice number.\n";
                                }
                                else if (dgvGSTR15.Columns[j].Name == "colTaxableVal")
                                {
                                    if (dgvGSTR15.Rows[k].Cells[j].Value == "")
                                        str_error += cnt + ") " + " Please enter " + dgvGSTR15.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") Taxable values must be max 11 digit and can not be more than Invoice value. \n";
                                }
                                else if (dgvGSTR15.Columns[j].Name == "colPOS")
                                {
                                    if (dgvGSTR15.Rows[k].Cells[j].Value == "")
                                        str_error += cnt + ") " + " Please select proper " + dgvGSTR15.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") Different " + dgvGSTR15.Columns[j].HeaderText + " can not be possible for same invoice no. \n";
                                }

                                else if (dgvGSTR15.Columns[j].Name == "colIGSTAmnt")
                                {
                                    if (dgvGSTR15.Rows[k].Cells[j].Value == "")
                                        str_error += cnt + ") " + " Please enter Must be " + dgvGSTR15.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") " + dgvGSTR15.Columns[j].HeaderText + " is not applicable for Intra State. Please enter exact match " + dgvGSTR15.Columns[j].HeaderText + " base on `Total Taxable Value` and `Rate` calculation. \n";
                                }
                                else if (dgvGSTR15.Columns[j].Name == "colCGSTAmnt")
                                {
                                    if (dgvGSTR15.Rows[k].Cells[j].Value == "")
                                        str_error += cnt + ") " + " Please enter Must be " + dgvGSTR15.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") " + dgvGSTR15.Columns[j].HeaderText + " is not applicable for Inter State. Please enter exact match " + dgvGSTR15.Columns[j].HeaderText + " base on `Total Taxable Value` and `Rate` calculation or CGST & SGST values must be same. \n";
                                }
                                else if (dgvGSTR15.Columns[j].Name == "colSGSTAmnt")
                                {
                                    if (dgvGSTR15.Rows[k].Cells[j].Value == "")
                                        str_error += cnt + ") " + " Please enter Must be " + dgvGSTR15.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") " + dgvGSTR15.Columns[j].HeaderText + " is not applicable for Inter State. Please enter exact match " + dgvGSTR15.Columns[j].HeaderText + " base on `Total Taxable Value` and `Rate` calculation or CGST & SGST values must be same. \n";
                                }
                                else if (dgvGSTR15.Columns[j].Name == "colCessAmnt")
                                {
                                    if (dgvGSTR15.Rows[k].Cells[j].Value == "")
                                        str_error += cnt + ") " + " Please enter Must be " + dgvGSTR15.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") " + dgvGSTR15.Columns[j].HeaderText + " is not required.\n";
                                }
                                else
                                {
                                    str_error += cnt + ") " + " Please enter proper " + dgvGSTR15.Columns[j].HeaderText + ".\n";
                                }
                                cnt++;
                            }
                        }
                        #region
                        if (ExcelValidFlag == true)
                        {
                            Ab++;
                            dt_new.Rows.Add();
                            int c = dt_new.Rows.Count;
                            for (int b = 0; b < dgvGSTR15.Columns.Count; b++)
                            {
                                if (dt_new.Columns.Count - 1 == b)
                                {
                                    dt_new.Rows[c - 1][b] = str_error;
                                }
                                else
                                {
                                    dt_new.Rows[c - 1][b] = Convert.ToString(dgvGSTR15.Rows[k].Cells[b].Value);
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

                    Excel.Range rg = (Excel.Range)sheetRange.Cells[1, 5];
                    rg.EntireColumn.NumberFormat = "dd-MM-yyyy";

                    rg = (Excel.Range)sheetRange.Cells[1, 4];
                    rg.EntireColumn.NumberFormat = "@";

                    sheetRange.Value2 = MyExcelarr;
                        #endregion
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
                        dt = (DataTable)dgvGSTR15.DataSource;

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
                                DisableControls(dgvGSTR15);

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
                                foreach (DataGridViewColumn col in dgvGSTR15.Columns)
                                {
                                    dtCsv.Columns[col.Index].ColumnName = col.Name.ToString();
                                    col.DataPropertyName = col.Name;
                                }
                                dt.AcceptChanges();
                                #endregion

                                // ASSIGN DATATABLE TO GRID
                                dgvGSTR15.DataSource = dt;

                                // ENABLE MAIN GRID
                                EnableControls(dgvGSTR15);
                            }
                            else
                            {
                                // IF THERE ARE NO RECORDS IN MAIN GRID

                                if (dtCsv != null && dtCsv.Rows.Count > 0)
                                {
                                    // IF THERE ARE RECORD PRESENT IN IMPORT FILE

                                    // DISABLE MAIN GRID
                                    DisableControls(dgvGSTR15);

                                    #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME AND ASSIGN TO MAIN GRID
                                    foreach (DataGridViewColumn col in dgvGSTR15.Columns)
                                    {
                                        dtCsv.Columns[col.Index].ColumnName = col.Name.ToString();
                                        col.DataPropertyName = col.Name;
                                    }
                                    dtCsv.AcceptChanges();

                                    // ASSIGN DATATABLE TO GRID
                                    dgvGSTR15.DataSource = dtCsv;

                                    // ENABLE CONTROL
                                    EnableControls(dgvGSTR15);
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
                            string[] colNo = { "colInvoiceNo", "colInvoiceValue", "colTaxableVal", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };
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
                EnableControls(dgvGSTR15);
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
                    for (int i = 1; i < dgvGSTR15.Columns.Count; i++)
                    {
                        Boolean flg = false;
                        for (int j = 0; j < csvData.Columns.Count; j++)
                        {
                            // CHECK GRID COLUMN IS PRESENT OR NOT IN IMPORTED EXCEL
                            if (dgvGSTR15.Columns[i].HeaderText.Replace(" ", "") == csvData.Columns[j].ColumnName.Replace(" ", "").Trim())
                            {
                                // IF GRID COLUMN PRESENT IN EXCEL THEN ITS INDEX AS PAR GRID COLUMN INDEX
                                flg = true;
                                csvData.Columns[j].SetOrdinal(dgvGSTR15.Columns[i].Index - 1);
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
                    if (csvData.Columns.Count >= dgvGSTR15.Columns.Count - 2)
                    {
                        for (int i = csvData.Columns.Count - 1; i > (dgvGSTR15.Columns.Count - 2); i--)
                        {
                            csvData.Columns.Remove(csvData.Columns[i]);
                        }
                    }
                    #endregion

                    #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME
                    foreach (DataGridViewColumn col in dgvGSTR15.Columns)
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
                if (dgvGSTR15.Rows.Count > 1)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID
                    pbGSTR1.Visible = true;
                    string csv = string.Empty;
                    // CREATE DATATABLE AND GET GRID DATA
                    DataTable dt = new DataTable();
                    dt = (DataTable)dgvGSTR15.DataSource;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        #region ASSIGN COLUMN NAME TO CSV STRING
                        for (int i = 1; i < dgvGSTR15.Columns.Count; i++)
                        {
                            csv += dgvGSTR15.Columns[i].HeaderText + ',';
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
                PdfPTable pdfTable = new PdfPTable(dgvGSTR15.ColumnCount - 2);
                pdfTable.DefaultCell.Padding = 3;
                pdfTable.WidthPercentage = 100;
                pdfTable.DefaultCell.BorderWidth = 0;
                iTextSharp.text.Font fontHeader = iTextSharp.text.FontFactory.GetFont("Calibri", 6);

                // ADD HEADER TO PDF TABLE
                pdfTable = AssignHeader(pdfTable, "4. Taxable outward supplies made to registered persons (including UIN-holders) other than supplies covered by Table 6");
                #endregion

                #region ADDING HEADER ROW
                int i = 0;

                #region HEADER1
                PdfPCell celHeader1 = new PdfPCell();

                celHeader1 = new PdfPCell(new Phrase("GSTIN/UIN", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Name Of Party", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Invoice details", fontHeader));
                celHeader1.Colspan = 3;
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

                celHeader1 = new PdfPCell(new Phrase("Indicate if supply attracts reverse charge", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("GSTIN of e-commerce operator(if applicable)", fontHeader));
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

                celHeader2 = new PdfPCell(new Phrase("Value", fontHeader));
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
                foreach (DataGridViewColumn column in dgvGSTR15.Columns)
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
                    foreach (DataGridViewRow row in dgvGSTR15.Rows)
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
                    foreach (DataGridViewRow row in dgvGSTR15.Rows)
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
                ce1.Colspan = dgvGSTR15.Columns.Count - 2;
                ce1.VerticalAlignment = Element.ALIGN_CENTER;
                ce1.HorizontalAlignment = Element.ALIGN_LEFT;
                ce1.BackgroundColor = new iTextSharp.text.BaseColor(197, 223, 197);
                ce1.BorderWidth = 0;
                pdfTable.AddCell(ce1);

                iTextSharp.text.Font FigToRs = FontFactory.GetFont("Calibri", 10, iTextSharp.text.Font.BOLD);
                PdfPCell ceHeader2 = new PdfPCell(new Phrase("(figures in Rs)", FigToRs));
                ceHeader2.Colspan = dgvGSTR15.Columns.Count - 2;
                ceHeader2.VerticalAlignment = Element.ALIGN_CENTER;
                ceHeader2.HorizontalAlignment = Element.ALIGN_RIGHT;
                ceHeader2.BorderWidth = 0;
                pdfTable.AddCell(ceHeader2);

                PdfPCell ce2 = new PdfPCell(new Phrase(" "));
                ce2.Colspan = dgvGSTR15.Columns.Count - 2;
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

        #region JSON CLASS
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

        public class Inv
        {
            public string inum { get; set; }
            [DefaultValue("")]
            public string idt { get; set; }
            [DefaultValue("")]
            public double val { get; set; }
            [DefaultValue("")]
            public string pos { get; set; }
            public string rchrg { get; set; }

            [DefaultValue(null)]
            public string etin { get; set; }
            public string inv_typ { get; set; }
            public List<Itm> itms { get; set; }
        }

        public class B2b
        {
            public string ctin { get; set; }
            public List<Inv> inv { get; set; }
        }

        public class RootObject
        {
            public string gstin { get; set; }
            public string fp { get; set; }
            public double gt { get; set; }
            public double cur_gt { get; set; }
            public List<B2b> b2b { get; set; }
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
                    #region Datatable JSON

                    DataTable dt = new DataTable();

                    #region Bid Data

                    #region Bind Grid Data

                    #region ADD DATATABLE COLUMN

                    foreach (DataGridViewColumn col in dgvGSTR15.Columns)
                    {
                        dt.Columns.Add(col.Name.ToString());
                        col.DataPropertyName = col.Name;
                    }
                    dt.Columns.Add("colType");
                    #endregion

                    #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                    object[] rowValue = new object[dt.Columns.Count];

                    foreach (DataGridViewRow dr in dgvGSTR15.Rows)
                    {
                        if (dr.Index != dgvGSTR15.Rows.Count - 1)
                        {
                            rowValue[0] = "False";
                            for (int i = 1; i < dr.Cells.Count; i++)
                            {
                                rowValue[i] = Convert.ToString(dr.Cells[i].Value);
                            }

                            rowValue[dt.Columns.Count - 1] = "Regular";

                            dt.Rows.Add(rowValue);
                        }
                    }
                    dt.Columns.Remove("colChk");
                    dt.Columns.Remove("colSequence");
                    dt.Columns.Remove("colNameofParty");
                    dt.AcceptChanges();

                    #endregion

                    #endregion

                    #region Export Data

                    DataTable dtExp = new DataTable();
                    string Query = "Select Fld_Type,Fld_GSTIN,Fld_party,Fld_InvoiceNo,Fld_InvoiceDate,Fld_InvoiceValue,Fld_PortCode,Fld_Shipingbill,Fld_Billdate,Fld_IGSTRate,Fld_IGSTInvoiceTaxableVal,Fld_IGSTAmnt,Fld_FileStatus from SPQR1ZeroRated where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
                    Application.DoEvents();
                    dtExp = objGSTR5.GetDataGSTR1(Query);

                    if (dtExp != null && dtExp.Rows.Count > 0)
                    {
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtExp.Rows)
                            {
                                if (Convert.ToString(dr["Fld_Type"]).Trim().ToLower() == "sez exports with payment" || Convert.ToString(dr["Fld_Type"]).Trim().ToLower() == "sez exports without payment" || Convert.ToString(dr["Fld_Type"]).Trim().ToLower() == "deemed exports")
                                {
                                    dt.Rows.Add();
                                    dt.Rows[dt.Rows.Count - 1]["colGSTIN"] = Convert.ToString(dr["Fld_GSTIN"]).Trim();
                                    dt.Rows[dt.Rows.Count - 1]["colInvoiceNo"] = Convert.ToString(dr["Fld_InvoiceNo"]).Trim();
                                    dt.Rows[dt.Rows.Count - 1]["colInvoiceDate"] = Convert.ToString(dr["Fld_InvoiceDate"]).Trim();
                                    dt.Rows[dt.Rows.Count - 1]["colInvoiceValue"] = Convert.ToString(dr["Fld_InvoiceValue"]).Trim();
                                    dt.Rows[dt.Rows.Count - 1]["colRate"] = Convert.ToString(dr["Fld_IGSTRate"]).Trim();
                                    dt.Rows[dt.Rows.Count - 1]["colTaxableVal"] = Convert.ToString(dr["Fld_IGSTInvoiceTaxableVal"]).Trim();
                                    dt.Rows[dt.Rows.Count - 1]["colIGSTAmnt"] = Convert.ToString(dr["Fld_IGSTAmnt"]).Trim();
                                    dt.Rows[dt.Rows.Count - 1]["colType"] = Convert.ToString(dr["Fld_Type"]).Trim();
                                    dt.AcceptChanges();
                                }
                            }
                        }
                        else
                        {
                            foreach (DataRow dr in dtExp.Rows)
                            {
                                if (Convert.ToString(dr["Fld_Type"]).Trim().ToLower() == "sez exports with payment" || Convert.ToString(dr["Fld_Type"]).Trim().ToLower() == "sez exports without payment" || Convert.ToString(dr["Fld_Type"]).Trim().ToLower() == "deemed exports")
                                {
                                    dt.Rows.Add();
                                    dt.Rows[dt.Rows.Count - 1]["colGSTIN"] = Convert.ToString(dr["Fld_GSTIN"]).Trim();
                                    dt.Rows[dt.Rows.Count - 1]["colInvoiceNo"] = Convert.ToString(dr["Fld_InvoiceNo"]).Trim();
                                    dt.Rows[dt.Rows.Count - 1]["colInvoiceDate"] = Convert.ToString(dr["Fld_InvoiceDate"]).Trim();
                                    dt.Rows[dt.Rows.Count - 1]["colInvoiceValue"] = Convert.ToString(dr["Fld_InvoiceValue"]).Trim();
                                    dt.Rows[dt.Rows.Count - 1]["colRate"] = Convert.ToString(dr["Fld_IGSTRate"]).Trim();
                                    dt.Rows[dt.Rows.Count - 1]["colTaxableVal"] = Convert.ToString(dr["Fld_IGSTInvoiceTaxableVal"]).Trim();
                                    dt.Rows[dt.Rows.Count - 1]["colIGSTAmnt"] = Convert.ToString(dr["Fld_IGSTAmnt"]).Trim();
                                    dt.Rows[dt.Rows.Count - 1]["colType"] = Convert.ToString(dr["Fld_Type"]).Trim();
                                    dt.AcceptChanges();
                                }
                            }
                        }
                    }

                    #endregion
                    #endregion

                    if (dt != null && dt.Rows.Count > 0)
                    {
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

                            List<B2b> objB2b = new List<B2b>();

                            for (int i = 0; i < list.Count; i++)
                            {
                                if (list[i] != "")
                                {
                                    B2b objTB2b = new B2b();
                                    objTB2b.ctin = Convert.ToString(list[i]);
                                    objB2b.Add(objTB2b);

                                    ObjJson.b2b = objB2b;

                                    #region Group By Invoice no
                                    List<string> lstInvNo = dt.Rows
                                           .OfType<DataRow>()
                                           .Where(x => list[i] == Convert.ToString(x["colGSTIN"]).Trim())
                                           .Select(x => Convert.ToString(x["colInvoiceNo"]).Trim())
                                           .Distinct().ToList();
                                    #endregion

                                    for (int sj = 0; sj < lstInvNo.Count; sj++)
                                    {
                                        if (lstInvNo[sj] != "")
                                        {
                                            List<string> lstRate = dt.Rows
                                                   .OfType<DataRow>()
                                                   .Where(x => list[i] == Convert.ToString(x["colGSTIN"]).Trim() && Convert.ToString(lstInvNo[sj]).Trim() == Convert.ToString(x["colInvoiceNo"]).Trim())
                                                   .Select(x => Convert.ToString(x["colRate"]).Trim())
                                                   .Distinct().ToList();

                                            if (lstRate != null && lstRate.Count > 0)
                                            {
                                                List<Inv> objInv = new List<Inv>();
                                                List<Itm> objItm = new List<Itm>();
                                                List<ItmDet> objItemDetails = new List<ItmDet>();

                                                for (int k = 0; k < lstRate.Count; k++)
                                                {
                                                    if (Convert.ToString(lstRate[k]).Trim() != "")
                                                    {
                                                        #region Rate wise Data Row
                                                        List<DataRow> lstDrRate = dt.Rows
                                                               .OfType<DataRow>()
                                                               .Where(x => list[i] == Convert.ToString(x["colGSTIN"]).Trim() && Convert.ToString(lstInvNo[sj]).Trim() == Convert.ToString(x["colInvoiceNo"]).Trim() && Convert.ToDecimal(lstRate[k]) == Convert.ToDecimal(x["colRate"]))
                                                               .Select(x => x)
                                                               .ToList();
                                                        #endregion

                                                        if (lstDrRate != null && lstDrRate.Count > 0)
                                                        {
                                                            if (k == 0)
                                                            {
                                                                Inv clsInv = new Inv();

                                                                #region Invoice Details

                                                                clsInv.inum = Convert.ToString(lstDrRate[0]["colInvoiceNo"]).Trim();//Invoice Number

                                                                if (!string.IsNullOrEmpty(Convert.ToString(lstDrRate[0]["colInvoiceDate"]).Trim()))
                                                                    clsInv.idt = Convert.ToString(Convert.ToDateTime(lstDrRate[0]["colInvoiceDate"]).ToString("dd-MM-yyyy"));//Invoice Date

                                                                clsInv.val = Convert.ToDouble(lstDrRate[0]["colInvoiceValue"]); // val; // Invoice Value

                                                                clsInv.pos = CommonHelper.GetStateCode(Convert.ToString(lstDrRate[0]["colPOS"]).Trim()).ToString(); //POS

                                                                clsInv.rchrg = GetInvoiceType(Convert.ToString(lstDrRate[0]["colIndSupAttac"]).Trim(), "REV"); //Reverse Charge

                                                                if (!string.IsNullOrEmpty(Convert.ToString(lstDrRate[0]["colGSTINofEcom"]).Trim())) // E-Com GSTIN
                                                                    clsInv.etin = Convert.ToString(lstDrRate[0]["colGSTINofEcom"]).Trim();

                                                                clsInv.inv_typ = GetInvoiceType(Convert.ToString(lstDrRate[0]["colType"]).Trim(), "TPE"); // Invoice type

                                                                #endregion

                                                                objInv.Add(clsInv);

                                                                if (ObjJson.b2b[i].inv == null)
                                                                    ObjJson.b2b[i].inv = objInv;
                                                                else
                                                                    ObjJson.b2b[i].inv.AddRange(objInv);
                                                            }

                                                            Itm clsItems = new Itm();
                                                            clsItems.num = k + 1;

                                                            #region Invoice Item Details

                                                            ItmDet clsItmDet = new ItmDet();

                                                            if (!string.IsNullOrEmpty(Convert.ToString(lstDrRate[0]["colRate"]).Trim())) // Rate
                                                                clsItmDet.rt = Convert.ToInt32(Convert.ToString(lstDrRate[0]["colRate"]).Trim());

                                                            if (lstDrRate.Count == 1)
                                                            {
                                                                if (!string.IsNullOrEmpty(Convert.ToString(lstDrRate[0]["colTaxableVal"]).Trim())) // Taxable value
                                                                    clsItmDet.txval = Convert.ToDouble(Convert.ToString(lstDrRate[0]["colTaxableVal"]).Trim());

                                                                if (!string.IsNullOrEmpty(Convert.ToString(lstDrRate[0]["colIGSTAmnt"]).Trim())) // IGST amount
                                                                    clsItmDet.iamt = Convert.ToDouble(Convert.ToString(lstDrRate[0]["colIGSTAmnt"]).Trim());

                                                                if (!string.IsNullOrEmpty(Convert.ToString(lstDrRate[0]["colCGSTAmnt"]).Trim())) // CGST amount
                                                                    clsItmDet.camt = Convert.ToDouble(Convert.ToString(lstDrRate[0]["colCGSTAmnt"]).Trim());

                                                                if (!string.IsNullOrEmpty(Convert.ToString(lstDrRate[0]["colSGSTAmnt"]).Trim())) // SGST amount
                                                                    clsItmDet.samt = Convert.ToDouble(Convert.ToString(lstDrRate[0]["colSGSTAmnt"]).Trim());

                                                                if (!string.IsNullOrEmpty(Convert.ToString(lstDrRate[0]["colCessAmnt"]).Trim())) // CESS amount
                                                                    clsItmDet.csamt = Convert.ToInt32(Convert.ToString(lstDrRate[0]["colCessAmnt"]).Trim());
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

                                                                clsItmDet.txval = lstDrRate.Cast<DataRow>().Where(x => x["colTaxableVal"] != null).Sum(x => Convert.ToString(x["colTaxableVal"]).Trim() == "" ? 0 : Convert.ToDouble(x["colTaxableVal"])); // Taxble value

                                                                if (igst != null) { clsItmDet.iamt = Convert.ToDouble(igst); } // IGST value 
                                                                if (cgst != null) { clsItmDet.camt = Convert.ToDouble(cgst); } // CGST value
                                                                if (sgst != null) { clsItmDet.samt = Convert.ToDouble(sgst); } // SGST value
                                                                if (cess != null) { clsItmDet.csamt = Convert.ToDouble(cess); } // CESS value
                                                            }
                                                            #endregion

                                                            clsItems.itm_det = clsItmDet;
                                                            objItm.Add(clsItems);
                                                            ObjJson.b2b[i].inv[sj].itms = objItm;
                                                        }
                                                    }
                                                }
                                            }
                                        }
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
                        save.FileName = "B2B.json";
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
        }

        public bool SaveJson()
        {
            try
            {
                RootObject ObjJson = new RootObject();

                List<string> list = dgvGSTR15.Rows
                       .OfType<DataGridViewRow>()
                       .Select(x => Convert.ToString(x.Cells["colGSTIN"].Value))
                       .Distinct().ToList();

                if (list != null && list.Count > 0)
                {
                    ObjJson.gstin = CommonHelper.CompanyGSTN; // tax person GSTIN
                    ObjJson.fp = CommonHelper.GetReturnPeriod(); // current return period
                    ObjJson.gt = CommonHelper.TurnOver; // previous financial year turnover
                    ObjJson.cur_gt = 99.99; // current Finacial year turnover

                    List<B2b> objB2b = new List<B2b>();

                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i] != "")
                        {
                            B2b objTB2b = new B2b();
                            objTB2b.ctin = Convert.ToString(list[i]);
                            objB2b.Add(objTB2b);

                            ObjJson.b2b = objB2b;

                            #region Group By Invoice no
                            List<string> lstInvNo = dgvGSTR15.Rows
                                   .OfType<DataGridViewRow>()
                                   .Where(x => list[i] == Convert.ToString(x.Cells["colGSTIN"].Value))
                                   .Select(x => Convert.ToString(x.Cells["colInvoiceNo"].Value))
                                   .Distinct().ToList();
                            #endregion

                            for (int sj = 0; sj < lstInvNo.Count; sj++)
                            {
                                if (lstInvNo[sj] != "")
                                {
                                    #region Invoice Number
                                    List<DataGridViewRow> Invoicelist = dgvGSTR15.Rows
                                           .OfType<DataGridViewRow>()
                                           .Where(x => lstInvNo[sj] == Convert.ToString(x.Cells["colInvoiceNo"].Value))
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
                                                Inv clsInv = new Inv();

                                                #region Invoice Details

                                                clsInv.inum = Convert.ToString(Invoicelist[j].Cells["colInvoiceNo"].Value);//Invoice Number

                                                if (!string.IsNullOrEmpty(Convert.ToString(Invoicelist[j].Cells["colInvoiceDate"].Value)))
                                                    clsInv.idt = Convert.ToString(Convert.ToDateTime(Invoicelist[j].Cells["colInvoiceDate"].Value).ToString("dd-MM-yyyy"));//Invoice Date

                                                int val = Convert.ToInt32(Invoicelist.Cast<DataGridViewRow>().Where(x => x.Cells["colInvoiceValue"].Value != null).Sum(x => x.Cells["colInvoiceValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colInvoiceValue"].Value)));
                                                clsInv.val = val; // Invoice Value

                                                clsInv.pos = CommonHelper.GetStateCode(Convert.ToString(Invoicelist[j].Cells["colPOS"].Value)).ToString(); //POS

                                                clsInv.rchrg = "N"; //Reverse Charge

                                                if (!string.IsNullOrEmpty(Convert.ToString(Invoicelist[j].Cells["colGSTINofEcom"].Value))) // E-Com GSTIN
                                                    clsInv.etin = Convert.ToString(Invoicelist[j].Cells["colGSTINofEcom"].Value);

                                                clsInv.inv_typ = "R"; // Invoice type

                                                #endregion

                                                objInv.Add(clsInv);

                                                if (ObjJson.b2b[i].inv == null)
                                                    ObjJson.b2b[i].inv = objInv;
                                                else
                                                    ObjJson.b2b[i].inv.AddRange(objInv);
                                            }

                                            Itm clsItems = new Itm();
                                            clsItems.num = j + 1;

                                            #region Invoice Item Details

                                            ItmDet clsItmDet = new ItmDet();

                                            if (!string.IsNullOrEmpty(Convert.ToString(Invoicelist[j].Cells["colRate"].Value).Trim())) // Rate
                                                clsItmDet.rt = Convert.ToInt32(Invoicelist[j].Cells["colRate"].Value);

                                            if (!string.IsNullOrEmpty(Convert.ToString(Invoicelist[j].Cells["colTaxableVal"].Value).Trim())) // Taxable value
                                                clsItmDet.txval = Convert.ToInt32(Invoicelist[j].Cells["colTaxableVal"].Value);

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
                                            ObjJson.b2b[i].inv[sj].itms = objItm;
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
                    return builder.SaveJsonToGSTN(FinalJson, "GSTR1");
                    #endregion
                }
                else
                {
                    MessageBox.Show("No Data Avilable.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        public string GetInvoiceType(string inv, string colName)
        {
            string invType = "";
            inv = inv.Trim().ToLower();
            try
            {
                if (colName == "TPE")
                {
                    if (inv != "")
                    {
                        if (inv == "sez exports with payment")
                            invType = "SEWP";
                        else if (inv == "sez exports without payment")
                            invType = "SEWOP";
                        else if (inv == "deemed exports")
                            invType = "DE";
                        else
                            invType = "R";
                    }
                }
                else if (colName == "REV")
                {
                    if (inv == "yes")
                        invType = "Y";
                    else
                        invType = "N";
                }
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }

            return invType;
        }

        #endregion

        public void SetGridViewColor()
        {
            try
            {
                // DO NOT ALLOW TO AUTO GENERATE COLUMNS
                dgvGSTR15.AutoGenerateColumns = false;

                // SET HEIGHT WIDTH OF FORM
                this.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.97));
                this.Height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.77));

                // SET WIDTH OF HEADER, MAIN AND TOTAL GRID
                this.pnlHeader.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.96));
                this.dgvGSTR15.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.96));
                this.dgvGSTR15Total.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.96));

                // SET HEIGHT OF MAIN GRID
                this.dgvGSTR15.Height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.65));
                this.dgvGSTR15Total.Height = 42;

                // SET LOCATION OF HEADER,LOADING PIC, CHECKBOX AND MAIN AND TOTAL GRID
                //this.pnlHeader.Location = new System.Drawing.Point(12, 0);
                //this.dgvGSTR15.Location = new System.Drawing.Point(12, Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.05)));
                //this.dgvGSTR15Total.Location = new System.Drawing.Point(12, Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.705)));
                //this.ckboxHeader.Location = new System.Drawing.Point(32, Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.135)));

                // SET MAIN GRID PROPERTY
                dgvGSTR15.EnableHeadersVisualStyles = false;
                dgvGSTR15.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(23, 196, 187);
                dgvGSTR15.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

                this.dgvGSTR15.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                this.dgvGSTR15.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                this.dgvGSTR15.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                foreach (DataGridViewColumn column in dgvGSTR15.Columns)
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

        public bool chkB2bInvType(string val)
        {
            bool flg = false;
            val = val.ToString();
            try
            {
                if (val == "Regular" || val == "SEZ Exports with payment" || val == "SEZ exports without payment" || val == "Deemed Exports" || val == "")
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)dgvGSTR15.DataSource;
                if (dt == null || dt.Rows.Count == 0)
                {
                    if (txtSearch.Text.ToString().Trim() != "")
                        MessageBox.Show("Kindly save record(s) before search!!!", "Alert", MessageBoxButtons.OK);
                    return;
                }
                if (cmbFilter.SelectedValue.ToString() == "")
                {
                    ((DataTable)dgvGSTR15.DataSource).DefaultView.RowFilter = string.Format("colSequence like '%{0}%' or colGSTIN like '%{0}%' or colNameofParty like '%{0}%' or colInvoiceNo like '%{0}%' or colInvoiceDate like '%{0}%' or colInvoiceValue like '%{0}%' or colRate like '%{0}%' or colTaxableVal like '%{0}%' or colIGSTAmnt like '%{0}%' or colCGSTAmnt like '%{0}%' or colSGSTAmnt like '%{0}%' or colCessAmnt like '%{0}%' or colPOS like '%{0}%' or colIndSupAttac like '%{0}%' or colGSTINofEcom like '%{0}%'", txtSearch.Text.Trim().Replace("'", "''"));
                }
                else
                {
                    ((DataTable)dgvGSTR15.DataSource).DefaultView.RowFilter = string.Format("" + cmbFilter.SelectedValue + " like '%{0}%'", txtSearch.Text.Trim().Replace("'", "''"));
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

        private void dgvGSTR15_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                // SET INDEX OF USER ADDED ROW IN MAIN GRID
                dgvGSTR15.Rows[e.Row.Index - 1].Cells["colSequence"].Value = e.Row.Index;
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

        private void dgvGSTR15_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                // SET SEQUNCING AFTER USER DELETING ROW IN GRID
                for (int i = e.Row.Index; i < dgvGSTR15.Rows.Count - 1; i++)
                {
                    dgvGSTR15.Rows[i].Cells["colSequence"].Value = i;
                }

                // TOTAL CALCULATION
                string[] colNo = { "colInvoiceNo", "colInvoiceValue", "colTaxableVal", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmnt" };
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

        #region DISABLE/ENABLE CONTROLS

        private void DisableControls(Control con)
        {
            foreach (Control c in con.Controls)
            {
                if (c.Name != "SPQGSTR1B2B" && c.Name != "dgvGSTR15Total")
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

        private void dgvGSTR15_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                // SET TOTAL GRID OFFSET AS PAR MAIN GRID SCROLL
                this.dgvGSTR15Total.HorizontalScrollingOffset = this.dgvGSTR15.HorizontalScrollingOffset;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvGSTR15Total_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                // SET MAIN GRID OFFSET AS PAR TOTAL GRID SCROLL
                this.dgvGSTR15.HorizontalScrollingOffset = this.dgvGSTR15Total.HorizontalScrollingOffset;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region CHECK ALL AND UNCHECK ALL

        private void dgvGSTR15_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                // CHECK FIRST COLUMN HEADER PRESSED AND MAIN GRID HAVING RECORDS
                if (e.ColumnIndex == 0 && dgvGSTR15.Rows.Count > 1)
                {
                    // CHECK AND UNCHECK CHECK BOX OF HEADER FOR SELECTING AND UNSELECTING ALL RECORDS
                    if (dgvGSTR15.Columns[e.ColumnIndex].HeaderText == "Check All")
                        ckboxHeader.Checked = true;
                    else if (dgvGSTR15.Columns[e.ColumnIndex].HeaderText == "Uncheck All")
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
                if (dgvGSTR15.Rows.Count > 1)
                {
                    pbGSTR1.Visible = true;
                    if (ckboxHeader.Checked)
                    {
                        // IF CHECK BOX IS CHECKED

                        // SET CHECK BOX COLUMN VALUE AS TRUE
                        for (int i = 0; i < dgvGSTR15.Rows.Count - 1; i++)
                        {
                            dgvGSTR15.Rows[i].Cells[0].Value = "True";
                            Application.DoEvents();
                        }

                        // CHANGE HEADER TEXT AND WIDTH OF COLUMN AND POSITION OF CHECK BOX OF CHECK ALL COLUMN
                        //dgvGSTR15.Columns[0].DefaultCellStyle.NullValue = true;
                        dgvGSTR15.Columns[0].HeaderText = "Uncheck All";
                    }
                    else if (ckboxHeader.Checked == false)
                    {
                        // IF CHECK BOX IS UNCHECKED

                        // SET CHECK BOX COLUMN VALUE AS FALSE
                        for (int i = 0; i < dgvGSTR15.Rows.Count - 1; i++)
                        {
                            dgvGSTR15.Rows[i].Cells[0].Value = "False";
                            Application.DoEvents();
                        }

                        // CHANGE HEADER TEXT AND WIDTH OF COLUMN AND POSITION OF CHECK BOX OF CHECK ALL COLUMN
                        //dgvGSTR15.Columns[0].DefaultCellStyle.NullValue = false;
                        dgvGSTR15.Columns[0].HeaderText = "Check All";
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

        private void GSTR1B2B_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName("");
        }

        public void ValidataAndGetGSTIN()
        {
            try
            {
                if (dgvGSTR15.Rows.Count > 1)
                {
                    pbGSTR1.Visible = true;
                    new PrefillHelper().GetNameByGSTIN(dgvGSTR15, "colGSTIN", "colNameofParty");

                    if (CommonHelper.IsGetGSINError != null)
                    {
                        if ((bool)CommonHelper.IsGetGSINError)
                            MessageBox.Show("Some GSTIN could not be Validated...!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            ValidataAndGetGSTIN();
        }

        private void dgvGSTR15_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pnlMdl_Paint(object sender, PaintEventArgs e)
        {

        }



    }
}
