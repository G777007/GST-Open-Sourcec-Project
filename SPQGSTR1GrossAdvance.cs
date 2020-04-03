using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.VisualBasic.FileIO;
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
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace SPEQTAGST.xasjbr1
{
    public partial class SPQGSTR1GrossAdvance : Form
    {
        r1Publicclass objGSTR1A5 = new r1Publicclass();

        public SPQGSTR1GrossAdvance()
        {
            InitializeComponent();
            // SET GRID PROPERTY
            SetGridViewColor();
            //Bind Data
            GetData();
            // TOTAL CALCULATION
            string[] colNo = { "colInvoiceNo", "colInvoiceValue", "colInvoiceTaxableVal", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmount" };
            GetTotal(colNo);
            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName(CommonHelper.ReturnName);
            BindFilter();

            dgvGSTR1A5.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dgvGSTR1A5.EnableHeadersVisualStyles = false;
            dgvGSTR1A5.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);
            dgvGSTR1A5.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvGSTR1A5.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dgvGSTR1A5Total.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            dgvGSTR1A5.Columns[2].Visible = false;
            dgvGSTR1A5.Columns[3].Visible = false;
            dgvGSTR1A5.Columns[4].Visible = false;
            dgvGSTR1A5.Columns[5].Visible = false;

            dgvGSTR1A5Total.Columns[2].Visible = false;
            dgvGSTR1A5Total.Columns[3].Visible = false;
            dgvGSTR1A5Total.Columns[4].Visible = false;
            dgvGSTR1A5Total.Columns[5].Visible = false;


        }

        #region Filter
        private void BindFilter()
        {
            try
            {
                List<colList> lstColumns = new List<colList>();
                for (int i = 0; i < dgvGSTR1A5.Columns.Count; i++)
                {
                    if (i > 1)
                    {
                        string HeaderText = dgvGSTR1A5.Columns[i].HeaderText;
                        string Name = dgvGSTR1A5.Columns[i].Name;
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
            //try
            //{
            //    #region JSON DATA STATIC
            //    string _json = "{ \"b2b\": [ { \"ctin\": \"01AABCE2207R1Z5\", \"cfs\": \"Y\", \"inv\": [ { \"flag\": \"M\", \"updby\": \"R\", \"chksum\": \"AflJufPlFStqKBZ\", \"inum\": \"S008400\", \"idt\": \"24-11-2016\", \"val\": 729248.16, \"pos\": \"06\", \"rchrg\": \"N\", \"prs\": \"Y\", \"od_num\": \"S008400\", \"od_dt\": \"03-02-2016\", \"etin\": \"01AABCE5507R1Z4\", \"itms\": [ { \"num\": 1, \"itm_det\": { \"ty\": \"G\", \"hsn_sc\": \"G1221\", \"txval\": 10000, \"irt\": 3, \"iamt\": 833.33, \"crt\": 4, \"camt\": 500, \"srt\": 5, \"samt\": 900, \"csrt\": 2, \"csamt\": 500 } }, { \"num\": 2, \"itm_det\": { \"ty\": \"S\", \"hsn_sc\": \"S1231\", \"txval\": 10000, \"irt\": 4, \"iamt\": 625.33, \"crt\": 6, \"camt\": 333.33, \"srt\": 5, \"samt\": 900, \"csrt\": 3, \"csamt\": 333.33 } } ] } ] } ] }";
            //    #endregion

            //    RootObject obj = JsonConvert.DeserializeObject<RootObject>(_json);

            //    #region ADD DATATABLE COLUMN
            //    DataTable dt = new DataTable();

            //    foreach (DataGridViewColumn col in dgvGSTR1A5.Columns)
            //    {
            //        if (col.Name.ToLower() != "colchk")
            //        {
            //            dt.Columns.Add(col.Name.ToString());
            //        }
            //    }

            //    #endregion

            //    #region ASSIGN GRIDVIEW ROWS IN DATATABLE
            //    for (int i = 0; i < obj.b2b.Count; i++)
            //    {
            //        for (int j = 0; j < obj.b2b[i].inv.Count; j++)
            //        {
            //            for (int k = 0; k < obj.b2b[i].inv[j].itms.Count; k++)
            //            {
            //                dt.Rows.Add();
            //                //ROOT START
            //                dt.Rows[dt.Rows.Count - 1]["colGSTIN"] = Convert.ToString(obj.b2b[i].ctin);
            //                //ROOT END

            //                //INVOICE DATA START
            //                dt.Rows[dt.Rows.Count - 1]["colInvoiceNo"] = Convert.ToString(obj.b2b[i].inv[j].inum);//INVOICE NO.
            //                dt.Rows[dt.Rows.Count - 1]["colInvoiceDate"] = Convert.ToString(obj.b2b[i].inv[j].idt);//INVOICE DATE
            //                dt.Rows[dt.Rows.Count - 1]["colPOS"] = Convert.ToString(obj.b2b[i].inv[j].pos);//POS
            //                dt.Rows[dt.Rows.Count - 1]["colTax"] = (Convert.ToString(obj.b2b[i].inv[j].prs) == "Y" ? "true" : "false");//TAX
            //                dt.Rows[dt.Rows.Count - 1]["colIndSupAttac"] = Convert.ToString(obj.b2b[i].inv[j].rchrg);//INDICATE SUPPLY ATTACK
            //                dt.Rows[dt.Rows.Count - 1]["colInvoiceValue"] = Convert.ToString(obj.b2b[i].inv[j].val);//SUPPLYER INVOICE VALUE
            //                //INVOICE DATA END

            //                //ITEM DATA START
            //                dt.Rows[dt.Rows.Count - 1]["colInvoiceGoodsServi"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.ty);//GOODS AND SERVICE
            //                dt.Rows[dt.Rows.Count - 1]["colInvoiceHSNSAC"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.hsn_sc);//HSN
            //                dt.Rows[dt.Rows.Count - 1]["colInvoiceTaxableVal"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.txval);//TAXABLE VALUE
            //                dt.Rows[dt.Rows.Count - 1]["colIGSTRate"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.irt);//IGST RATE
            //                dt.Rows[dt.Rows.Count - 1]["colIGSTAmnt"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.iamt);//IGST AMOUNT
            //                dt.Rows[dt.Rows.Count - 1]["colCGSTRate"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.crt);//CGST RATE
            //                dt.Rows[dt.Rows.Count - 1]["colCGSTAmnt"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.camt);//CGST AMOUNT
            //                dt.Rows[dt.Rows.Count - 1]["colSGSTRate"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.srt);//SGST RATE
            //                dt.Rows[dt.Rows.Count - 1]["colSGSTAmnt"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.samt);//SGST AMOUNT

            //                #region New Parameter
            //                //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].inv[j].flag);
            //                //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].inv[j].updby);
            //                //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].inv[j].od_num);
            //                //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].inv[j].od_dt);
            //                //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].inv[j].etin);
            //                //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].cfs);

            //                //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.csrt);
            //                //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.csamt);
            //                #endregion

            //                //ITEM DATA END
            //            }
            //        }
            //    }
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        dt.Rows[i]["colSequence"] = Convert.ToString(i + 1);
            //    }
            //    dt.AcceptChanges();
            //    dgvGSTR1A5.DataSource = dt;
            //    #endregion
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Prefill Data Error : " + ex.Message);
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
                DataTable dt = new DataTable();
                string Query = "Select * from SPQR1GrossAdvance where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
                Application.DoEvents();
                dt = objGSTR1A5.GetDataGSTR1(Query);

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
                    dt.Columns.Remove("Fld_Month");
                    // REMOVE LAST COLUMN (FILE STATUS)
                    dt.Columns.Remove("Fld_FileStatus");
                    // REMOVE FIRST COLUMN (FIELD ID)
                    dt.Columns.Remove("Fld_ID");

                    // ADD COLUMN (CHEK BOX)
                    dt.Columns.Add(new DataColumn("colChk"));
                    // SET CHECK BOX COLUMN AT FIRST INDEX OF DATATABLE
                    dt.Columns["colChk"].SetOrdinal(0);
                    //dt.Columns.Add(new DataColumn("colError"));

                    // RENAME DATATABLE COLUMN NAME TO DATAGRIDVIEW COLUMN NAME
                    foreach (DataGridViewColumn col in dgvGSTR1A5.Columns)
                    {
                        dt.Columns[col.Index].ColumnName = col.Name.ToString();
                        col.DataPropertyName = col.Name;
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string ColName = dt.Columns[j].ColumnName;
                            if (ColName == "colInvoiceValue" || ColName == "colInvoiceTaxableVal" || ColName == "colIGSTAmnt" || ColName == "colCGSTAmnt" || ColName == "colSGSTAmnt" || ColName == "colCessAmount")
                                dt.Rows[i][j] = Utility.DisplayIndianCurrency(Convert.ToString(dt.Rows[i][j]));
                        }
                    }

                    dt.AcceptChanges();

                    // ASSIGN DATATABLE TO DATA GRID VIEW
                    dgvGSTR1A5.DataSource = dt;
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
                if (dgvGSTR1A5.Rows.Count > 1)
                {
                    // IF MAIN GRID HAVING RECORDS

                    if (dgvGSTR1A5Total.Rows.Count == 0)
                    {
                        #region IF TOTAL GRID HAVING NO RECORD
                        // CREATE TEMPORARY DATATABLE TO STORE COLUMN CALCULATION
                        DataTable dtTotal = new DataTable();

                        // ADD COLUMN AS PAR DATAGRIDVIEW COLUMN
                        foreach (DataGridViewColumn col in dgvGSTR1A5Total.Columns)
                        {
                            dtTotal.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;
                        }

                        #region ADD DATATABLE COLUMN
                        DataTable dt = new DataTable();
                        // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                        foreach (DataGridViewColumn col in dgvGSTR1A5.Columns)
                        {
                            dt.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;

                            if (col.Name == "colPOS")
                                dt.Columns["colPOS"].DataType = typeof(System.String);
                        }
                        #endregion

                        #region ASSIGN GRIDVIEW ROWS IN DATATABLE
                        object[] rowValue = new object[dt.Columns.Count];

                        foreach (DataGridViewRow drn in dgvGSTR1A5.Rows)
                        {
                            if (drn.Index != dgvGSTR1A5.Rows.Count - 1)
                            {
                                rowValue[0] = "False";
                                for (int i = 1; i < drn.Cells.Count; i++)
                                {
                                    rowValue[i] = Convert.ToString(drn.Cells[i].Value);
                                }
                                dt.Rows.Add(rowValue);
                            }
                        }
                        dt.AcceptChanges();
                        #endregion

                        // CREATE DATAROW TO STORE GRID COLUMN CALCULATION
                        DataRow dr = dtTotal.NewRow();

                        #region POS Count
                        var result2 = (from row in dt.AsEnumerable()
                                       where row.Field<string>("colPOS") != ""
                                       group row by new { colPOS = row.Field<string>("colPOS") } into grp
                                       select new
                                       {
                                           colPOS = grp.Key.colPOS,
                                       }).ToList();

                        if (result2.Count != null && result2.Count > 0)
                            dr["colTPOS"] = result2.Count;
                        else
                            dr["colTPOS"] = 0;
                        #endregion

                        dr["colTInvoiceNo"] = dgvGSTR1A5.Rows.Cast<DataGridViewRow>().Where(x => Convert.ToString(x.Cells["colInvoiceNo"].Value).Trim() != "").GroupBy(x => x.Cells["colInvoiceNo"].Value).Select(x => x.First()).Distinct().Count();
                        dr["colTInvoiceValue"] = dgvGSTR1A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colInvoiceValue"].Value != null).Sum(x => x.Cells["colInvoiceValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colInvoiceValue"].Value)).ToString();
                        // dr["colTInvoiceTaxableVal"] = dgvGSTR1A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colInvoiceTaxableVal"].Value != null).Sum(x => x.Cells["colInvoiceTaxableVal"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colInvoiceTaxableVal"].Value)).ToString();
                        dr["colTIGSTAmnt"] = dgvGSTR1A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colIGSTAmnt"].Value != null).Sum(x => x.Cells["colIGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIGSTAmnt"].Value)).ToString();
                        dr["colTCGSTAmnt"] = dgvGSTR1A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCGSTAmnt"].Value != null).Sum(x => x.Cells["colCGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCGSTAmnt"].Value)).ToString();
                        dr["colTSGSTAmnt"] = dgvGSTR1A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colSGSTAmnt"].Value != null).Sum(x => x.Cells["colSGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colSGSTAmnt"].Value)).ToString();
                        dr["colTCessAmount"] = dgvGSTR1A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCessAmount"].Value != null).Sum(x => x.Cells["colCessAmount"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCessAmount"].Value)).ToString();

                        // ADD DATAROW TO DATATABLE
                        dtTotal.Rows.Add(dr);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                string ColName = dt.Columns[j].ColumnName;
                                if (ColName == "colTInvoiceValue" || ColName == "colTIGSTAmnt" || ColName == "colTCGSTAmnt" || ColName == "colTSGSTAmnt" || ColName == "colTCessAmount")
                                    dt.Rows[i][j] = Utility.DisplayIndianCurrency(Convert.ToString(dt.Rows[i][j]));
                            }
                        }

                        dtTotal.AcceptChanges();

                        // ASSIGN DATATABLE TO GRID
                        dgvGSTR1A5Total.DataSource = dtTotal;

                        // SET GRID ROW HEIGHT AND ASSIGN TOTAL HEADER
                        dgvGSTR1A5Total.Rows[0].Height = 30;
                        dgvGSTR1A5Total.Rows[0].Cells[0].Value = "TOTAL";
                        #endregion
                    }
                    else if (dgvGSTR1A5Total.Rows.Count == 1)
                    {
                        #region IF TOTAL GRID HAVING ONLY ONE RECORDS
                        // SET GRID ROW HEIGHT AND ASSIGN TOTAL HEADER
                        dgvGSTR1A5Total.Rows[0].Height = 30;
                        dgvGSTR1A5Total.Rows[0].Cells[0].Value = "TOTAL";

                        // CALCULATE TOTAL ONLY SPECIFIC COLUMN
                        foreach (var item in colNo)
                        {
                            if (item == "colPOS")
                            {
                                #region ADD DATATABLE COLUMN
                                DataTable dt = new DataTable();
                                // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                                foreach (DataGridViewColumn col in dgvGSTR1A5.Columns)
                                {
                                    dt.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;

                                    if (col.Name == "colPOS")
                                        dt.Columns["colPOS"].DataType = typeof(System.String);
                                }
                                #endregion

                                #region ASSIGN GRIDVIEW ROWS IN DATATABLE
                                object[] rowValue = new object[dt.Columns.Count];

                                foreach (DataGridViewRow drn in dgvGSTR1A5.Rows)
                                {
                                    if (drn.Index != dgvGSTR1A5.Rows.Count - 1)
                                    {
                                        rowValue[0] = "False";
                                        for (int i = 1; i < drn.Cells.Count; i++)
                                        {
                                            rowValue[i] = Convert.ToString(drn.Cells[i].Value);
                                        }
                                        dt.Rows.Add(rowValue);
                                    }
                                }
                                dt.AcceptChanges();
                                #endregion

                                #region POS Count
                                var result2 = (from row in dt.AsEnumerable()
                                               where row.Field<string>("colPOS") != ""
                                               group row by new { colPOS = row.Field<string>("colPOS") } into grp
                                               select new
                                               {
                                                   colPOS = grp.Key.colPOS,
                                               }).ToList();

                                if (result2.Count != null && result2.Count > 0)
                                    dgvGSTR1A5Total.Rows[0].Cells["colTPOS"].Value = result2.Count;
                                else
                                    dgvGSTR1A5Total.Rows[0].Cells["colTPOS"].Value = "";
                                #endregion
                            }

                            if (item == "colInvoiceNo")
                                dgvGSTR1A5Total.Rows[0].Cells["colTInvoiceNo"].Value = dgvGSTR1A5.Rows.Cast<DataGridViewRow>().Where(x => Convert.ToString(x.Cells["colInvoiceNo"].Value).Trim() != "").GroupBy(x => x.Cells["colInvoiceNo"].Value).Select(x => x.First()).Distinct().Count();
                            else if (item == "colInvoiceValue")
                                dgvGSTR1A5Total.Rows[0].Cells["colTInvoiceValue"].Value = Utility.DisplayIndianCurrency(dgvGSTR1A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colInvoiceValue"].Value != null).Sum(x => x.Cells["colInvoiceValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colInvoiceValue"].Value)).ToString());
                            //else if (item == "colInvoiceTaxableVal")
                            //    dgvGSTR1A5Total.Rows[0].Cells["colTInvoiceTaxableVal"].Value = dgvGSTR1A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colInvoiceTaxableVal"].Value != null).Sum(x => x.Cells["colInvoiceTaxableVal"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colInvoiceTaxableVal"].Value)).ToString();
                            else if (item == "colIGSTAmnt")
                                dgvGSTR1A5Total.Rows[0].Cells["colTIGSTAmnt"].Value = Utility.DisplayIndianCurrency(dgvGSTR1A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colIGSTAmnt"].Value != null).Sum(x => x.Cells["colIGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIGSTAmnt"].Value)).ToString());
                            else if (item == "colCGSTAmnt")
                                dgvGSTR1A5Total.Rows[0].Cells["colTCGSTAmnt"].Value = Utility.DisplayIndianCurrency(dgvGSTR1A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCGSTAmnt"].Value != null).Sum(x => x.Cells["colCGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCGSTAmnt"].Value)).ToString());
                            else if (item == "colSGSTAmnt")
                                dgvGSTR1A5Total.Rows[0].Cells["colTSGSTAmnt"].Value = Utility.DisplayIndianCurrency(dgvGSTR1A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colSGSTAmnt"].Value != null).Sum(x => x.Cells["colSGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colSGSTAmnt"].Value)).ToString());
                            else if (item == "colCessAmount")
                                dgvGSTR1A5Total.Rows[0].Cells["colTCessAmount"].Value = Utility.DisplayIndianCurrency(dgvGSTR1A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCessAmount"].Value != null).Sum(x => x.Cells["colCessAmount"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCessAmount"].Value)).ToString());
                        }
                        #endregion
                    }

                    // SET TOTAL GRID HEIGHT ROW
                    dgvGSTR1A5Total.Rows[0].Height = 30;
                }
                else
                {
                    // CHECK IF TOTAL GRID HAVING RECORD

                    if (dgvGSTR1A5Total.Rows.Count >= 0)
                    {
                        #region IF THERE ARE NO RECORDS IN MAIN GRID THEN ASSIGN BLANK DATATABLE TO TOTAL GRID
                        DataTable dtTotal = new DataTable();
                        foreach (DataGridViewColumn col in dgvGSTR1A5Total.Columns)
                        {
                            dtTotal.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;
                        }
                        dgvGSTR1A5Total.DataSource = dtTotal;
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

        private void dgvGSTR1A5_KeyDown(object sender, KeyEventArgs e)
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
                        if (dgvGSTR1A5.Rows.Count > 0)
                        {
                            // DELETE SELECTED CELL IN GRID
                            foreach (DataGridViewCell oneCell in dgvGSTR1A5.SelectedCells)
                            {
                                // CHECK BOX COLUMN (0,17) DATA DO NOT DELETE
                                if (oneCell.Selected && oneCell.ColumnIndex != 0 && oneCell.ColumnIndex != 19)
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

                    string[] colNo = { "colInvoiceNo", "colInvoiceValue", "colInvoiceTaxableVal", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmount" };
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
                    if (dgvGSTR1A5.RowCount > 0) // IF GRID IS NOT NULL AND PAST ONLY ON SELECTED CELLS
                    {
                        foreach (DataGridViewCell oneCell in dgvGSTR1A5.SelectedCells)
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
                            DisableControls(dgvGSTR1A5);

                            gRowNo = dgvGSTR1A5.Rows.Count - 1;
                            int no = tmp;

                            if (iRow > gRowNo - 1)
                            {
                                DataTable dtDGV = new DataTable();
                                //dtDGV = dgvGSTR1A5.DataSource as DataTable;

                                #region ADD DATATABLE COLUMN

                                // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                                foreach (DataGridViewColumn col in dgvGSTR1A5.Columns)
                                {
                                    dtDGV.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }
                                #endregion

                                #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                                // CREATE OBJECT ARRAY TO STORE ONE ROW DATA OF MAIN GRID
                                object[] rowValue = new object[dtDGV.Columns.Count];

                                foreach (DataGridViewRow dr in dgvGSTR1A5.Rows)
                                {
                                    if (dr.Index != dgvGSTR1A5.Rows.Count - 1) // DON'T ADD LAST ROW
                                    {
                                        // SET CHECK BOX VALUE
                                        rowValue[0] = "False";
                                        for (int i = 1; i < dr.Cells.Count; i++)
                                        {
                                            rowValue[i] = Convert.ToString(dr.Cells[i].Value);
                                        }

                                        //if (Convert.ToString(dr.Cells[dr.Cells.Count - 2].Value).ToLower() == "y" || Convert.ToString(dr.Cells[dr.Cells.Count - 2].Value).ToLower() == "yes")
                                        //    rowValue[dr.Cells.Count - 2] = "Yes";
                                        //else
                                        //    rowValue[dr.Cells.Count - 2] = "No";

                                        //if (Convert.ToString(dr.Cells[dr.Cells.Count - 1].Value).ToLower() == "1" || Convert.ToString(dr.Cells[dr.Cells.Count - 1].Value).ToLower() == "yes" || Convert.ToString(dr.Cells[dr.Cells.Count - 1].Value).ToLower() == "true")
                                        //    rowValue[dr.Cells.Count - 1] = "True";
                                        //else
                                        //    rowValue[dr.Cells.Count - 1] = "False";

                                        //rowValue[dr.Cells.Count] = Convert.ToString(dr.Cells[iCol].Value);

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
                                        if (iCol + i < this.dgvGSTR1A5.ColumnCount && i < 11)
                                        {
                                            // SKIP CHECK BOX COLUMN AND SEQUANCE COLUMN TO PASTE DATA
                                            if (iCol == 0)
                                                oCell = dgvGSTR1A5[iCol + i + 2, iRow];
                                            else if (iCol == 1)
                                                oCell = dgvGSTR1A5[iCol + i + 1, iRow];
                                            else
                                                oCell = dgvGSTR1A5[iCol + i, iRow];

                                            sCells[i] = sCells[i].Trim().Replace(",", "");
                                            if (oCell.ColumnIndex != 0)
                                            {
                                                if (dgvGSTR1A5.Columns[oCell.ColumnIndex].Name != "colChk" && dgvGSTR1A5.Columns[oCell.ColumnIndex].Name != "colSequence")
                                                {
                                                    #region VALIDATION
                                                    if (sCells[i].ToString().Trim() == "") { dgvGSTR1A5.Rows[iRow].Cells[oCell.ColumnIndex].Value = ""; }
                                                    else
                                                    {
                                                        if (oCell.ColumnIndex >= 2 && oCell.ColumnIndex <= 12)
                                                        {
                                                            if (chkCellValue(sCells[i].Trim(), oCell.ColumnIndex))
                                                                dgvGSTR1A5.Rows[iRow].Cells[oCell.ColumnIndex].Value = sCells[i].Trim();
                                                            else
                                                                dgvGSTR1A5.Rows[iRow].Cells[oCell.ColumnIndex].Value = "";
                                                        }
                                                        else { dgvGSTR1A5.Rows[iRow].Cells[oCell.ColumnIndex].Value = sCells[i].Trim(); }
                                                    }
                                                    #endregion
                                                }
                                            }
                                            else
                                            {
                                                if (iCol > i)
                                                {
                                                    for (int j = oCell.ColumnIndex; j < dgvGSTR1A5.Columns.Count; j++)
                                                    {
                                                        #region VALIDATION
                                                        if (sCells[i].ToString().Trim() == "") { dgvGSTR1A5.Rows[iRow].Cells[j].Value = ""; }
                                                        else
                                                        {
                                                            if (j >= 2 && j <= 12)
                                                            {
                                                                if (chkCellValue(sCells[i].Trim(), j))
                                                                    dgvGSTR1A5.Rows[iRow].Cells[j].Value = sCells[i].Trim();
                                                                else
                                                                    dgvGSTR1A5.Rows[iRow].Cells[j].Value = "";
                                                            }
                                                            else { dgvGSTR1A5.Rows[iRow].Cells[j].Value = sCells[i].Trim(); }
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
                                                    for (int j = oCell.ColumnIndex; j < dgvGSTR1A5.Columns.Count; j++)
                                                    {
                                                        #region VALIDATION
                                                        if (sCells[i].ToString().Trim() == "") { dgvGSTR1A5.Rows[iRow].Cells[j].Value = ""; }
                                                        else
                                                        {
                                                            if (j >= 2 && j <= 12)
                                                            {
                                                                if (chkCellValue(sCells[i].Trim(), j))
                                                                    dgvGSTR1A5.Rows[iRow].Cells[j].Value = sCells[i].Trim();
                                                                else
                                                                    dgvGSTR1A5.Rows[iRow].Cells[j].Value = "";
                                                            }
                                                            else { dgvGSTR1A5.Rows[iRow].Cells[j].Value = sCells[i].Trim(); }
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

                    for (int i = 0; i < dgvGSTR1A5.Rows.Count - 1; i++)
                    {
                        dgvGSTR1A5.Rows[i].Cells["colSequence"].Value = i + 1;
                    }
                    #endregion

                    // ENABLE CONTROL
                    EnableControls(dgvGSTR1A5);
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
                EnableControls(dgvGSTR1A5);
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

                DisableControls(dgvGSTR1A5);

                #region SET DATATABLE
                int cnt = 0, colNo = 0;

                // ASSIGN GRID DATA TO DATATABLE
                DataTable dt = dtDGV;

                if (dt == null)
                {
                    // IF NO RECORD IN GRID THEN CREATE NEW DATATABLE
                    dt = new DataTable();

                    // ADD COLUMN AS PAR MAIN GRID AND SET DATA ACCESS PROPERTY
                    foreach (DataGridViewColumn col in dgvGSTR1A5.Columns)
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
                                if (iCol + i < this.dgvGSTR1A5.ColumnCount && colNo < 12)
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
                                                if (colNo >= 2 && colNo <= 12)
                                                {
                                                    if (chkCellValue(sCells[i].Trim(), colNo))
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
                                            for (int j = colNo; j < dgvGSTR1A5.Columns.Count; j++)
                                            {
                                                #region VALIDATION
                                                if (sCells[i].ToString().Trim() == "") { dt.Rows[dt.Rows.Count - 1][j] = DBNull.Value; }
                                                else
                                                {
                                                    if (j >= 2 && j <= 12)
                                                    {
                                                        if (chkCellValue(sCells[i].Trim(), j))
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
                                            for (int j = colNo; j < dgvGSTR1A5.Columns.Count; j++)
                                            {
                                                #region VALIDATION
                                                if (sCells[i].ToString().Trim() == "") { dt.Rows[dt.Rows.Count - 1][j] = DBNull.Value; }
                                                else
                                                {
                                                    if (j >= 2 && j <= 12)
                                                    {
                                                        if (chkCellValue(sCells[i].Trim(), j))
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
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string ColName = dt.Columns[j].ColumnName;
                            if (ColName == "colInvoiceValue" || ColName == "colInvoiceTaxableVal" || ColName == "colIGSTAmnt" || ColName == "colCGSTAmnt" || ColName == "colSGSTAmnt" || ColName == "colCessAmount")
                                dt.Rows[i][j] = Utility.DisplayIndianCurrency(Convert.ToString(dt.Rows[i][j]));
                        }
                    }

                    dgvGSTR1A5.DataSource = dt;
                }

                // TOTAL CALCULATION
                string[] colGroup = { "colInvoiceNo", "colInvoiceValue", "colInvoiceTaxableVal", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmount" }; ;
                GetTotal(colGroup);

                EnableControls(dgvGSTR1A5);

                #endregion

                pbGSTR1.Visible = false;
            }
            catch (Exception ex)
            {
                pbGSTR1.Visible = false;
                EnableControls(dgvGSTR1A5);
                MessageBox.Show("Error : " + ex.Message);
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
            string pgst;
            bool flag;
            try
            {
                int _cnt = 0;
                string _str = "";
                this.dgvGSTR1A5.CurrentCell = this.dgvGSTR1A5.Rows[0].Cells[0];
                this.dgvGSTR1A5.AllowUserToAddRows = false;
                this.pbGSTR1.Visible = true;
                List<DataGridViewRow> list = (
                    from x in this.dgvGSTR1A5.Rows.OfType<DataGridViewRow>()
                    where !Utility.IsBlankGSTN(Convert.ToString(x.Cells["colGSTIN"].Value))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR1A5.Rows[list[i].Cells["colGSTIN"].RowIndex].Cells["colGSTIN"].Style.BackColor = Color.Red;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Please enter proper GSTIN Number.\n");
                }
                list = (
                    from x in this.dgvGSTR1A5.Rows.OfType<DataGridViewRow>()
                    where Utility.IsBlankGSTN(Convert.ToString(x.Cells["colGSTIN"].Value))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    this.dgvGSTR1A5.Rows[list[i].Cells["colGSTIN"].RowIndex].Cells["colGSTIN"].Style.BackColor = Color.White;
                }
                list = null;
                list = (
                    from x in this.dgvGSTR1A5.Rows.OfType<DataGridViewRow>()
                    where !Utility.IsBlankDate(Convert.ToString(x.Cells["colInvoiceDate"].Value))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR1A5.Rows[list[i].Cells["colInvoiceDate"].RowIndex].Cells["colInvoiceDate"].Style.BackColor = Color.Red;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Please enter proper invoice date.\n");
                }
                list = (
                    from x in this.dgvGSTR1A5.Rows.OfType<DataGridViewRow>()
                    where Utility.IsBlankDate(Convert.ToString(x.Cells["colInvoiceDate"].Value))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    this.dgvGSTR1A5.Rows[list[i].Cells["colInvoiceDate"].RowIndex].Cells["colInvoiceDate"].Style.BackColor = Color.White;
                }
                list = null;
                list = (
                    from x in this.dgvGSTR1A5.Rows.OfType<DataGridViewRow>()
                    where !Utility.IsTaxableValue(Convert.ToString(x.Cells["colInvoiceValue"].Value))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR1A5.Rows[list[i].Cells["colInvoiceValue"].RowIndex].Cells["colInvoiceValue"].Style.BackColor = Color.Red;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Please enter proper Gross Advance Received.\n");
                }
                list = (
                    from x in this.dgvGSTR1A5.Rows.OfType<DataGridViewRow>()
                    where Utility.IsTaxableValue(Convert.ToString(x.Cells["colInvoiceValue"].Value))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    this.dgvGSTR1A5.Rows[list[i].Cells["colInvoiceValue"].RowIndex].Cells["colInvoiceValue"].Style.BackColor = Color.White;
                }
                list = null;
                list = (
                    from x in this.dgvGSTR1A5.Rows.OfType<DataGridViewRow>()
                    where !Utility.IsRate(Convert.ToString(x.Cells["colIGSTRate"].Value))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR1A5.Rows[list[i].Cells["colIGSTRate"].RowIndex].Cells["colIGSTRate"].Style.BackColor = Color.Red;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Please enter proper Rate.\n");
                }
                list = (
                    from x in this.dgvGSTR1A5.Rows.OfType<DataGridViewRow>()
                    where Utility.IsRate(Convert.ToString(x.Cells["colIGSTRate"].Value))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    this.dgvGSTR1A5.Rows[list[i].Cells["colIGSTRate"].RowIndex].Cells["colIGSTRate"].Style.BackColor = Color.White;
                }
                string gstin = CommonHelper.GetStateName(Convert.ToString(CommonHelper.CompanyGSTN).Substring(0, 2));
                string result = gstin;
                list = this.dgvGSTR1A5.Rows.OfType<DataGridViewRow>().ToList<DataGridViewRow>();
                for (j = 0; j < list.Count; j++)
                {
                    pgst = Convert.ToString(this.dgvGSTR1A5.Rows[list[j].Cells["colPOS"].RowIndex].Cells["colPOS"].Value);
                    if (result.ToLower() != pgst.ToLower())
                    {
                        if (Convert.ToString(this.dgvGSTR1A5.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value) == "")
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Please enter proper IGST Amount.\n");
                            this.dgvGSTR1A5.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.Red;
                        }
                        else if (Utility.IsICSC(Convert.ToString(this.dgvGSTR1A5.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value)))
                        {
                            this.dgvGSTR1A5.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.White;
                        }
                        else
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Please enter proper IGST Amount.\n");
                            this.dgvGSTR1A5.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.Red;
                        }
                    }
                    else if (Convert.ToString(this.dgvGSTR1A5.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value) != "")
                    {
                        _cnt++;
                        _str = string.Concat(_str, _cnt, ") Please enter proper Integrated tax Amount.\n");
                        this.dgvGSTR1A5.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.Red;
                    }
                    else if (Convert.ToString(this.dgvGSTR1A5.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value) == "")
                    {
                        this.dgvGSTR1A5.Rows[list[j].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.White;
                    }
                }
                for (j = 0; j < list.Count; j++)
                {
                    pgst = Convert.ToString(this.dgvGSTR1A5.Rows[list[j].Cells["colPOS"].RowIndex].Cells["colPOS"].Value);
                    if (result.ToLower() == pgst.ToLower())
                    {
                        if (Convert.ToString(this.dgvGSTR1A5.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value) == "")
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Please enter proper CGST Amount.\n");
                            this.dgvGSTR1A5.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.Red;
                        }
                        else if (Utility.IsICSC(Convert.ToString(this.dgvGSTR1A5.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value)))
                        {
                            this.dgvGSTR1A5.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.White;
                        }
                        else
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Please enter proper CGST Amount.\n");
                            this.dgvGSTR1A5.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.Red;
                        }
                    }
                    else if (Convert.ToString(this.dgvGSTR1A5.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value) != "")
                    {
                        _cnt++;
                        _str = string.Concat(_str, _cnt, ") Please enter proper Central tax Amount.\n");
                        this.dgvGSTR1A5.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.Red;
                    }
                    else if (Convert.ToString(this.dgvGSTR1A5.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Value) == "")
                    {
                        this.dgvGSTR1A5.Rows[list[j].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.White;
                    }
                }
                for (j = 0; j < list.Count; j++)
                {
                    pgst = Convert.ToString(this.dgvGSTR1A5.Rows[list[j].Cells["colPOS"].RowIndex].Cells["colPOS"].Value);
                    if (result.ToLower() == pgst.ToLower())
                    {
                        if (Convert.ToString(this.dgvGSTR1A5.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value) == "")
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Please enter proper SGST Amount.\n");
                            this.dgvGSTR1A5.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.Red;
                        }
                        else if (Utility.IsICSC(Convert.ToString(this.dgvGSTR1A5.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value)))
                        {
                            this.dgvGSTR1A5.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.White;
                        }
                        else
                        {
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Please enter proper SGST Amount.\n");
                            this.dgvGSTR1A5.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.Red;
                        }
                    }
                    else if (Convert.ToString(this.dgvGSTR1A5.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value) != "")
                    {
                        _cnt++;
                        _str = string.Concat(_str, _cnt, ") Please enter proper State/UT tax Amount.\n");
                        this.dgvGSTR1A5.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.Red;
                    }
                    else if (Convert.ToString(this.dgvGSTR1A5.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Value) == "")
                    {
                        this.dgvGSTR1A5.Rows[list[j].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.White;
                    }
                }
                for (j = 0; j < list.Count; j++)
                {
                    if (!(Convert.ToString(this.dgvGSTR1A5.Rows[list[j].Cells["colCessAmount"].RowIndex].Cells["colCessAmount"].Value) != ""))
                    {
                        this.dgvGSTR1A5.Rows[list[j].Cells["colCessAmount"].RowIndex].Cells["colCessAmount"].Style.BackColor = Color.White;
                    }
                    else if (Utility.IsICSC(Convert.ToString(this.dgvGSTR1A5.Rows[list[j].Cells["colCessAmount"].RowIndex].Cells["colCessAmount"].Value)))
                    {
                        this.dgvGSTR1A5.Rows[list[j].Cells["colCessAmount"].RowIndex].Cells["colCessAmount"].Style.BackColor = Color.White;
                    }
                    else
                    {
                        _cnt++;
                        _str = string.Concat(_str, _cnt, ") Please enter proper CESS Amount.\n");
                        this.dgvGSTR1A5.Rows[list[j].Cells["colCessAmount"].RowIndex].Cells["colCessAmount"].Style.BackColor = Color.Red;
                    }
                }
                list = null;
                list = (
                    from x in this.dgvGSTR1A5.Rows.OfType<DataGridViewRow>()
                    where (!(Convert.ToString(x.Cells["colCGSTAmnt"].Value) != Convert.ToString(x.Cells["colSGSTAmnt"].Value)) || !(Convert.ToString(x.Cells["colCGSTAmnt"].Value) != "") ? false : Convert.ToString(x.Cells["colSGSTAmnt"].Value) != "")
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR1A5.Rows[list[i].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.Red;
                        this.dgvGSTR1A5.Rows[list[i].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.Red;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Please enter proper CGST Amount and SGST Amount it can be no different value.\n");
                }
                list = (
                    from x in this.dgvGSTR1A5.Rows.OfType<DataGridViewRow>()
                    where (!(Convert.ToString(x.Cells["colCGSTAmnt"].Value) == Convert.ToString(x.Cells["colSGSTAmnt"].Value)) || !(Convert.ToString(x.Cells["colCGSTAmnt"].Value) != "") ? false : Convert.ToString(x.Cells["colSGSTAmnt"].Value) != "")
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    if (this.dgvGSTR1A5.Rows[list[i].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor != Color.Red)
                    {
                        this.dgvGSTR1A5.Rows[list[i].Cells["colCGSTAmnt"].RowIndex].Cells["colCGSTAmnt"].Style.BackColor = Color.White;
                    }
                    if (this.dgvGSTR1A5.Rows[list[i].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor != Color.Red)
                    {
                        this.dgvGSTR1A5.Rows[list[i].Cells["colSGSTAmnt"].RowIndex].Cells["colSGSTAmnt"].Style.BackColor = Color.White;
                    }
                }
                list = null;
                list = (
                    from x in this.dgvGSTR1A5.Rows.OfType<DataGridViewRow>()
                    where !Utility.IsValidStateName(Convert.ToString(x.Cells["colPOS"].Value))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR1A5.Rows[list[i].Cells["colPOS"].RowIndex].Cells["colPOS"].Style.BackColor = Color.Red;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Please enter proper place of supply.\n");
                }
                list = (
                    from x in this.dgvGSTR1A5.Rows.OfType<DataGridViewRow>()
                    where Utility.IsNumber(Convert.ToString(x.Cells["colPOS"].Value))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    this.dgvGSTR1A5.Rows[list[i].Cells["colPOS"].RowIndex].Cells["colPOS"].Style.BackColor = Color.White;
                }
                this.dgvGSTR1A5.AllowUserToAddRows = true;
                this.pbGSTR1.Visible = false;
                if (!(_str != ""))
                {
                    if (this.objGSTR1A5.InsertValidationFlg("GSTR1", "GROSS-ADVANCE", "true", CommonHelper.SelectedMonth) != 1)
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
                    if (this.objGSTR1A5.InsertValidationFlg("GSTR1", "GROSS-ADVANCE", "false", CommonHelper.SelectedMonth) != 1)
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
                this.dgvGSTR1A5.AllowUserToAddRows = true;
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

        private Boolean chkCellValue(string cellValue, int cNo)
        {
            try
            {
                if (cellValue.Trim() != "")// NOT EQUEL BLANK
                {
                    if (cNo == 2) //GSTN
                    {
                        if (Utility.IsGSTN(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == 5) // Date
                    {
                        if (Utility.IsDate(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == 6 || cNo == 8 || cNo == 9 || cNo == 10 || cNo == 11) // value
                    {
                        if (Utility.IsDecimalOrNumber(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == 7)  // Rate
                    {
                        if (Utility.IsRate(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (dgvGSTR1A5.Columns[cNo].Name == "colPOS")
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
                MessageBox.Show("Error : " + ex.Message);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
                return false;
            }
        }

        private void dgvGSTR1A5_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int cNo = e.ColumnIndex;

                if (e.RowIndex >= 0)
                {
                    if (cNo == 2 || cNo == 5 || cNo == 7 || dgvGSTR1A5.Columns[cNo].Name == "colPOS")
                    {
                        if (!chkCellValue(Convert.ToString(dgvGSTR1A5.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                            dgvGSTR1A5.Rows[e.RowIndex].Cells[cNo].Value = "";

                        if (dgvGSTR1A5.Columns[cNo].Name == "colPOS")
                        {
                            string[] colNo = { dgvGSTR1A5.Columns[e.ColumnIndex].Name };
                            GetTotal(colNo);
                        }

                        if (cNo == 7)
                        {
                            if (Convert.ToString(dgvGSTR1A5.Rows[e.RowIndex].Cells[cNo].Value).Trim() != "")
                            {
                                dgvGSTR1A5.CellValueChanged -= dgvGSTR1A5_CellValueChanged;
                                dgvGSTR1A5.Rows[e.RowIndex].Cells[cNo].Value = Math.Round(Convert.ToDecimal(dgvGSTR1A5.Rows[e.RowIndex].Cells[cNo].Value), 2, MidpointRounding.AwayFromZero);
                                dgvGSTR1A5.CellValueChanged += dgvGSTR1A5_CellValueChanged;
                            }
                        }
                    }
                    else if (cNo == 4 || cNo == 6 || cNo == 8 || cNo == 9 || cNo == 10 || cNo == 11) // value
                    {
                        if (chkCellValue(Convert.ToString(dgvGSTR1A5.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                        {
                            if (cNo != 4)
                            {
                                if (Convert.ToString(dgvGSTR1A5.Rows[e.RowIndex].Cells[cNo].Value).Trim() != "")
                                {
                                    dgvGSTR1A5.CellValueChanged -= dgvGSTR1A5_CellValueChanged;
                                    dgvGSTR1A5.Rows[e.RowIndex].Cells[cNo].Value = Utility.DisplayIndianCurrency(Convert.ToString(Math.Round(Convert.ToDecimal(dgvGSTR1A5.Rows[e.RowIndex].Cells[cNo].Value), 2, MidpointRounding.AwayFromZero)));
                                    dgvGSTR1A5.CellValueChanged += dgvGSTR1A5_CellValueChanged;
                                }
                            }

                            string[] colNo = { dgvGSTR1A5.Columns[e.ColumnIndex].Name };
                            GetTotal(colNo);
                        }
                        else { dgvGSTR1A5.Rows[e.RowIndex].Cells[cNo].Value = ""; }
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
                //if (CommonHelper.StatusIndex == 0)
                //{
                //    MessageBox.Show("Please Select File Status!");
                //    return;
                //}

                pbGSTR1.Visible = true;

                #region ADD DATATABLE COLUMN
                DataTable dt = new DataTable();

                foreach (DataGridViewColumn col in dgvGSTR1A5.Columns)
                {
                    dt.Columns.Add(col.Name.ToString());
                }
                dt.Columns.Add("colFileStatus");
                #endregion

                #region ASSIGN GRIDVIEW ROWS IN DATATABLE
                object[] rowValue = new object[dt.Columns.Count];

                foreach (DataGridViewRow dr in dgvGSTR1A5.Rows)
                {
                    if (dr.Index != dgvGSTR1A5.Rows.Count - 1)// DON'T ADD LAST ROW
                    {
                        for (int i = 0; i < dr.Cells.Count; i++)
                        {
                            rowValue[i] = Convert.ToString(dr.Cells[i].Value);
                        }

                        rowValue[dr.Cells.Count] = Convert.ToString(CommonHelper.StatusText);

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

                if (dt != null && dt.Rows.Count > 0)
                {
                    #region FIRST DELETE OLD DATA FROM DATABASE
                    Query = "Delete from SPQR1GrossAdvance where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                    _Result = objGSTR1A5.IUDData(Query);
                    if (_Result != 1)
                    {
                        //FAIL
                        MessageBox.Show("System error.\nPlease try after sometime!");
                        return;
                    }
                    #endregion

                    _Result = objGSTR1A5.GSTR1_GAdvance(dt, Convert.ToString(CommonHelper.StatusText));

                    if (_Result == 1)
                    {
                        // TOTAL CALCULATION
                        string[] colNo = { "colInvoiceNo", "colInvoiceValue", "colInvoiceTaxableVal", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmount" };
                        GetTotal(colNo);

                        #region ADD DATATABLE COLUMN
                        dt = new DataTable();

                        foreach (DataGridViewColumn col in dgvGSTR1A5.Columns)
                        {
                            dt.Columns.Add(col.Name.ToString());
                        }
                        dt.Columns.Add("colFileStatus");
                        #endregion

                        #region ASSIGN GRIDVIEW ROWS IN DATATABLE
                        object[] rowVal = new object[dt.Columns.Count];

                        if (dgvGSTR1A5Total.Rows.Count == 1)
                        {
                            foreach (DataGridViewRow dr in dgvGSTR1A5Total.Rows)
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

                        _Result = objGSTR1A5.GSTR1_GAdvance(dt, "Total");
                        if (_Result == 1)
                        {
                            //DONE
                            //MessageBox.Show("Data Saved Successfully …!!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        // IF ERRORS OCCURS WHILE INSERTING DATA TO DATABASE
                        MessageBox.Show("System error.\nPlease try after sometime!");
                        return;
                    }
                }
                else
                {
                    #region DELETE ALL OLD RECORD IF THERE ARE NO RECORDS PRESENT IN GRID
                    Query = "Delete from SPQR1GrossAdvance where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                    _Result = objGSTR1A5.IUDData(Query);
                    if (_Result == 1)
                    {
                        //DONE
                        MessageBox.Show("Record Successfully Deleted!");
                        // MAKE FILE STATUS BLANK
                        ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(0);
                        string[] colNo = { "colInvoiceNo", "colInvoiceValue", "colInvoiceTaxableVal", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmount" };
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
                if (dgvGSTR1A5.CurrentCell.RowIndex == 0 && dgvGSTR1A5.CurrentCell.ColumnIndex == 0)
                {
                    dgvGSTR1A5.CurrentCell = dgvGSTR1A5.Rows[0].Cells[1];
                }
                else { dgvGSTR1A5.CurrentCell = dgvGSTR1A5.Rows[0].Cells[0]; }

                pbGSTR1.Visible = true;

                // CREATE FLAG FRO DELETE ROWS
                Boolean flgChk = false; Boolean flgSelect = false;

                // CREATE OBJECT OF SELECTED ROW TO DELETE
                List<DataGridViewRow> toDelete = new List<DataGridViewRow>();

                // CHECK THERE ARE RECORD PRESENT IN GRID
                if (dgvGSTR1A5.Rows.Count > 1)
                {
                    // FLAG TRUE IF CHECK ALL SELECTED
                    if (ckboxHeader.Checked)
                        flgChk = true;

                    #region ADD SELECTED ROW TO OBJECT FOR DELETE
                    for (int i = 0; i < dgvGSTR1A5.Rows.Count - 1; i++)
                    {
                        if (dgvGSTR1A5[0, i].Value != null && dgvGSTR1A5[0, i].Value.ToString() != "")
                        {
                            if (Convert.ToBoolean(dgvGSTR1A5[0, i].Value) == true)
                            {
                                // ADD ROW TO OBJECT IF ROW IS SELECTED
                                flgSelect = true;
                                toDelete.Add(dgvGSTR1A5.Rows[i]);
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
                                foreach (DataGridViewColumn col in dgvGSTR1A5.Columns)
                                {
                                    dt.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }

                                // ASSIGN BLANK DATATABLE TO GRID
                                dgvGSTR1A5.DataSource = dt;
                            }
                            else if (flgSelect == true)
                            {
                                // DELETE SELECTED ROW
                                foreach (DataGridViewRow row in toDelete)
                                {
                                    dgvGSTR1A5.Rows.RemoveAt(row.Index);
                                    Application.DoEvents();
                                }
                            }

                            // SEQUANCING MAIN GRID RECORDS
                            for (int i = 0; i < dgvGSTR1A5.Rows.Count - 1; i++)
                            {
                                dgvGSTR1A5.Rows[i].Cells["colSequence"].Value = Convert.ToString(i + 1);
                            }

                            if (dgvGSTR1A5.Rows.Count == 1)
                            {
                                // IF THERE ARE NO RECORDS IN MAIN GRID THENE ASSIGN BLANK DATATABLE TO TOTAL GRID
                                DataTable dtTotal = new DataTable();
                                foreach (DataGridViewColumn col in dgvGSTR1A5Total.Columns)
                                {
                                    dtTotal.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }
                                dgvGSTR1A5Total.DataSource = dtTotal;
                            }

                            // SET CONTROL PROPERTY AFTER ROW DELETION
                            ckboxHeader.Checked = false;
                            dgvGSTR1A5.Columns[0].HeaderText = "Check All";
                            #endregion
                        }
                    }

                    // TOTAL CALCULATION
                    string[] colNo = { "colInvoiceNo", "colInvoiceValue", "colInvoiceTaxableVal", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmount" };
                    GetTotal(colNo);
                }
                else
                {
                    // IF THERE ARE NO RECORD TO DELETE
                    ckboxHeader.Checked = false;
                    dgvGSTR1A5.Columns[0].HeaderText = "Check All";
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
                    pbGSTR1.Visible = true;
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

                        DataTable dt = new DataTable();
                        #region ADD DATATABLE COLUMN

                        // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                        foreach (DataGridViewColumn col in dgvGSTR1A5.Columns)
                        {
                            dt.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;
                        }
                        #endregion

                        #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                        // CREATE OBJECT ARRAY TO STORE ONE ROW DATA OF MAIN GRID
                        object[] rowValue = new object[dt.Columns.Count];

                        foreach (DataGridViewRow dr in dgvGSTR1A5.Rows)
                        {
                            if (dr.Index != dgvGSTR1A5.Rows.Count - 1) // DON'T ADD LAST ROW
                            {
                                // SET CHECK BOX VALUE
                                rowValue[0] = "False";
                                for (int i = 1; i < dr.Cells.Count; i++)
                                {
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

                        // CREATE DATATABLE TO STORE MAIN GRID DATA

                        // CHECK IMPORTED TEMPLATE
                        if (dtExcel.Columns.Count != 1)
                        {
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                // COMBINE IMPORTED EXCEL DATA AND GRID DATA

                                // DISABLE MAIN GRID
                                DisableControls(dgvGSTR1A5);

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
                                foreach (DataGridViewColumn col in dgvGSTR1A5.Columns)
                                {
                                    dt.Columns[col.Index].ColumnName = col.Name.ToString();
                                    col.DataPropertyName = col.Name;
                                }
                                #endregion

                                //ASSIGN DATATABLE TO DATAGRID
                                dgvGSTR1A5.DataSource = dt;

                                // ENABLE MAIN GRID
                                EnableControls(dgvGSTR1A5);
                            }
                            else
                            {
                                // IF THERE ARE NO RECORDS IN MAIN GRID

                                if (dtExcel != null && dtExcel.Rows.Count > 0)
                                {
                                    // IF THERE ARE DATA IN IMPORTED EXCEL FILE

                                    // DISABLE MAIN GRID
                                    DisableControls(dgvGSTR1A5);

                                    #region RENAME DATATABLE COLUMN NAME AS PAR MAIN GRID
                                    foreach (DataGridViewColumn col in dgvGSTR1A5.Columns)
                                    {
                                        dtExcel.Columns[col.Index].ColumnName = col.Name.ToString();
                                        col.DataPropertyName = col.Name;
                                    }
                                    dtExcel.AcceptChanges();

                                    // ASSIGN DATATALE TO GRID
                                    dgvGSTR1A5.DataSource = dtExcel;

                                    // ENABLE MAIN GRID
                                    EnableControls(dgvGSTR1A5);
                                    #endregion
                                }
                                else
                                {
                                    // IF THERE ARE NO RECORDS IN IMPORTED EXCEL FILE
                                    MessageBox.Show("There are no records found in imported excel ...!!!!");
                                }
                            }

                            // TOTAL CALCULATION
                            string[] colNo = { "colInvoiceNo", "colInvoiceValue", "colInvoiceTaxableVal", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmount" };
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
                    }
                    pbGSTR1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                pbGSTR1.Visible = false;
                EnableControls(dgvGSTR1A5);
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
                        OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [advance_tax$]", con);
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
                        if (dtexcel.Columns.Count >= dgvGSTR1A5.Columns.Count - 2)
                        {
                            for (int k = dtexcel.Columns.Count - 1; k > (dgvGSTR1A5.Columns.Count - 4); k--)
                            {
                                dtexcel.Columns.Remove(dtexcel.Columns[k]);
                            }
                        }
                        dtexcel.AcceptChanges();
                        #endregion

                        #region VALIDATE TEMPLATE
                        for (int i = 2; i < dgvGSTR1A5.Columns.Count; i++)
                        {
                            flg = false;
                            for (int j = 0; j < dtexcel.Columns.Count; j++)
                            {
                                // CHECK GRID COLUMN IS PRESENT OR NOT IN IMPORTED EXCEL
                                if (dgvGSTR1A5.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim() == dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim())
                                {
                                    // IF GRID COLUMN PRESENT IN EXCEL THEN ITS INDEX AS PAR GRID COLUMN INDEX
                                    flg = true;
                                    //dtexcel.Columns[j].SetOrdinal(dgvGSTR1A5.Columns[i].Index - 2);
                                    dtexcel.Columns[j].SetOrdinal(dgvGSTR1A5.Columns[i - 2].Index);
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
                        dtexcel.Columns.Add("colError");

                        #region REMOVE UNUSED COLUMN FROM EXCEL
                        //if (dtexcel.Columns.Count >= dgvGSTR1A5.Columns.Count - 2)
                        //{
                        //    for (int i = dtexcel.Columns.Count; i > (dgvGSTR1A5.Columns.Count - 2); i--)
                        //    {
                        //        dtexcel.Columns.Remove(dtexcel.Columns[i - 1]);
                        //    }
                        //}
                        //dtexcel.AcceptChanges();
                        #endregion

                        #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME
                        foreach (DataGridViewColumn col in dgvGSTR1A5.Columns)
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

                        #region SET SEQUENCE NO
                        for (int i = 0; i < dtexcel.Rows.Count; i++)
                        {
                            dtexcel.Rows[i]["colChk"] = "False";
                            dtexcel.Rows[i]["colSequence"] = i + 1;

                            if (!Utility.IsValidStateName(Convert.ToString(dtexcel.Rows[i]["colPOS"]).Trim()))
                                dtexcel.Rows[i]["colPOS"] = "";

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
                if (dgvGSTR1A5.Rows.Count > 1)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID
                    pbGSTR1.Visible = true;

                    #region CREATE WORKBOOK AND ASSIGN COLUMNNAME
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook WB = excelApp.Workbooks.Add(Missing.Value);

                    Excel.Worksheet newWS = (Excel.Worksheet)excelApp.Worksheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    newWS.Name = "advance_tax";

                    // DELETE UNUSED WORKSHEETS FROM WORKBOOK
                    foreach (Excel.Worksheet ws in WB.Worksheets)
                    {
                        if (ws.Name != "advance_tax")
                            ((Excel.Worksheet)WB.Worksheets[ws.Name]).Delete();
                    }

                    // ASSIGN COLUMN HEADER AS PAR THE GRID HEADER
                    for (int i = 2; i < dgvGSTR1A5.Columns.Count; i++)
                    {
                        newWS.Cells[1, i - 1] = dgvGSTR1A5.Columns[i].HeaderText.ToString();

                        ((Excel.Range)newWS.Cells[1, i - 1]).ColumnWidth = 17;
                    }

                    // GET RANGE AND SET DIFFRENT PROPERTIES
                    Excel.Range headerRange = (Excel.Range)newWS.get_Range((Excel.Range)newWS.Cells[1, 1], (Excel.Range)newWS.Cells[1, dgvGSTR1A5.Columns.Count - 2]);
                    headerRange.WrapText = true;
                    headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Calibri";
                    #endregion

                    #region COPY DATA FROM DATATABLE TO ARRAY

                    // CREATE ARRAY TO HOLD THE DATA OF DATATABLE
                    object[,] arr = new object[dgvGSTR1A5.Rows.Count - 1, dgvGSTR1A5.Columns.Count];

                    // ASSIGN DATA TO ARRAY FROM DATATABLE
                    if (CommonHelper.IsLicence)
                    {
                        // FOR LICENECE ALLOWS TO EXPORT ALL RECORDS
                        for (int i = 0; i < dgvGSTR1A5.Rows.Count - 1; i++)
                        {
                            for (int j = 2; j < dgvGSTR1A5.Columns.Count; j++)
                            {
                                if (dgvGSTR1A5.Columns[j].Name == "colInvoiceDate")
                                {
                                    try
                                    {
                                        DateTime ss = Convert.ToDateTime(dgvGSTR1A5.Rows[i].Cells[j].Value);
                                        //arr[i, j - 2] = ss.ToString("dd-MM-yyyy").Replace('/', '-');
                                        arr[i, j - 2] = ss;
                                    }
                                    catch (Exception)
                                    {
                                        arr[i, j - 2] = "";
                                    }
                                }
                                else
                                    arr[i, j - 2] = Convert.ToString(dgvGSTR1A5.Rows[i].Cells[j].Value);
                            }
                        }
                    }
                    else
                    {
                        // FOR DEMO ALLOW ONLY 100 RECORDS TO EXPORT
                        for (int i = 0; i < dgvGSTR1A5.Rows.Count - 1; i++)
                        {
                            if (i < 100)
                            {
                                for (int j = 2; j < dgvGSTR1A5.Columns.Count; j++)
                                {
                                    if (dgvGSTR1A5.Columns[j].Name == "colInvoiceDate")
                                    {
                                        try
                                        {
                                            DateTime ss = Convert.ToDateTime(dgvGSTR1A5.Rows[i].Cells[j].Value);
                                            //arr[i, j - 2] = ss.ToString("dd-MM-yyyy").Replace('/', '-');
                                            arr[i, j - 2] = ss;
                                        }
                                        catch (Exception)
                                        {
                                            arr[i, j - 2] = "";
                                        }
                                    }
                                    else
                                        arr[i, j - 2] = Convert.ToString(dgvGSTR1A5.Rows[i].Cells[j].Value);
                                }
                            }
                        }
                    }

                    //SET EXCEL RANGE TO PASTE THE DATA
                    Excel.Range top = (Excel.Range)newWS.Cells[2, 1];
                    Excel.Range bottom = (Excel.Range)newWS.Cells[dgvGSTR1A5.Rows.Count, dgvGSTR1A5.Columns.Count];
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

                    pbGSTR1.Visible = false;
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

        public void ExportExcelForValidatation()
        {
            List<int> listValid = new List<int>();
            try
            {
                if (dgvGSTR1A5.Rows.Count > 1)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID

                    pbGSTR1.Visible = true;

                    #region CREATE WORKBOOK AND ASSIGN COLUMNNAME
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook WB = excelApp.Workbooks.Add(Missing.Value);

                    Excel.Worksheet newWS = (Excel.Worksheet)excelApp.Worksheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    newWS.Name = "advance_tax";

                    // DELETE UNUSED WORKSHEETS FROM WORKBOOK
                    foreach (Excel.Worksheet ws in WB.Worksheets)
                    {
                        if (ws.Name != "advance_tax")
                            ((Excel.Worksheet)WB.Worksheets[ws.Name]).Delete();
                    }

                    // ASSIGN COLUMN HEADER AS PAR THE GRID HEADER
                    int yy = 1;
                    for (int i = 2; i < dgvGSTR1A5.Columns.Count + 1; i++)
                    {
                        newWS.Cells[1, i - 1] = dgvGSTR1A5.Columns[yy].HeaderText.ToString();

                        ((Excel.Range)newWS.Cells[1, i - 1]).ColumnWidth = 17;
                        yy++;
                    }
                    ((Excel.Range)newWS.Cells[1, 13]).ColumnWidth = 45;
                    //Change as per Requirement


                    // GET RANGE AND SET DIFFRENT PROPERTIES
                    Excel.Range headerRange = (Excel.Range)newWS.get_Range((Excel.Range)newWS.Cells[1, 1], (Excel.Range)newWS.Cells[1, dgvGSTR1A5.Columns.Count]);
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
                    foreach (DataGridViewColumn column in dgvGSTR1A5.Columns)
                        dt.Columns.Add(column.Name, typeof(string));

                    for (int k = 0; k < dgvGSTR1A5.Rows.Count; k++)
                    {
                        for (int j = 0; j < dgvGSTR1A5.ColumnCount; j++)
                        {
                            if (dgvGSTR1A5.Rows[k].Cells[j].Style.BackColor == Color.Red)
                            {
                                ExcelValidFlag = true;
                                //sheetRange.Cells[k + 1, j - 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                            }
                        }
                        if (ExcelValidFlag == true)
                        {
                            dt.Rows.Add();
                            int count = dt.Rows.Count - 1;
                            for (int b = 0; b < dgvGSTR1A5.Columns.Count; b++)
                            {
                                dt.Rows[count][b] = dgvGSTR1A5.Rows[k].Cells[b].Value;
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

                    for (int k = 0; k < dgvGSTR1A5.Rows.Count; k++)
                    {
                        string str_error = "";
                        int cnt = 1;
                        for (int j = 0; j < dgvGSTR1A5.ColumnCount; j++)
                        {
                            if (dgvGSTR1A5.Rows[k].Cells[j].Style.BackColor == Color.Red)
                            {
                                ExcelValidFlag = true;
                                sheetRange.Cells[Ab + 1, j].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);

                                if (dgvGSTR1A5.Columns[j].Name == "colInvoiceNo")
                                {
                                    if (dgvGSTR1A5.Rows[k].Cells[j].Value == "")
                                        str_error += cnt + ") " + " Please enter " + dgvGSTR1A5.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") ARN number max length is 16 And invoice number can consist only(/) and (-). \n";
                                }
                                else if (dgvGSTR1A5.Columns[j].Name == "colInvoiceDate")
                                {
                                    if (dgvGSTR1A5.Rows[k].Cells[j].Value == "")
                                        str_error += cnt + ") " + " Please enter " + dgvGSTR1A5.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") " + " Please enter proper " + dgvGSTR1A5.Columns[j].HeaderText + " on this format(dd-MM-YYYY) OR Enter current month Date. \n";
                                }
                                else if (dgvGSTR1A5.Columns[j].Name == "colIGSTRate")
                                {
                                    if (dgvGSTR1A5.Rows[k].Cells[j].Value == "")
                                        str_error += cnt + ") " + " Please enter " + dgvGSTR1A5.Columns[j].HeaderText + "(Ex : (0),(2.5),(3),(5),(12),(18),(28)). \n";
                                    else
                                        str_error += cnt + ") " + " Please enter proper " + dgvGSTR1A5.Columns[j].HeaderText + "(Ex : (0),(2.5),(3),(5),(12),(18),(28)). \n";
                                }

                                else if (dgvGSTR1A5.Columns[j].Name == "colIGSTAmnt")
                                {
                                    if (dgvGSTR1A5.Rows[k].Cells[j].Value.ToString() == "")
                                        str_error += cnt + ") " + " Please enter proper " + dgvGSTR1A5.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") " + dgvGSTR1A5.Columns[j].HeaderText + " is not applicable for Intra State. Please enter exact match " + dgvGSTR1A5.Columns[j].HeaderText + " base on `Gross Advance Received` and `Rate` calculation. \n";
                                }
                                else if (dgvGSTR1A5.Columns[j].Name == "colCGSTAmnt")
                                {
                                    if (dgvGSTR1A5.Rows[k].Cells[j].Value.ToString() == "")
                                        str_error += cnt + ") " + " Please enter proper " + dgvGSTR1A5.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") " + dgvGSTR1A5.Columns[j].HeaderText + " is not applicable for Inter State. Please enter exact match " + dgvGSTR1A5.Columns[j].HeaderText + " base on `Gross Advance Received` and `Rate` calculation or CGST & SGST values must be same. \n";
                                }
                                else if (dgvGSTR1A5.Columns[j].Name == "colSGSTAmnt")
                                {
                                    if (dgvGSTR1A5.Rows[k].Cells[j].Value.ToString() == "")
                                        str_error += cnt + ") " + " Please enter proper " + dgvGSTR1A5.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") " + dgvGSTR1A5.Columns[j].HeaderText + " is not applicable for Inter State. Please enter exact match " + dgvGSTR1A5.Columns[j].HeaderText + " base on `Gross Advance Received` and `Rate` calculation or CGST & SGST values must be same. \n";
                                }
                                else if (dgvGSTR1A5.Columns[j].Name == "colCessAmnt")
                                {
                                    if (dgvGSTR1A5.Rows[k].Cells[j].Value.ToString() == "")
                                        str_error += cnt + ") " + " Please enter proper " + dgvGSTR1A5.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") " + dgvGSTR1A5.Columns[j].HeaderText + "is not required in SEZ exports without payment invoice .\n";
                                }
                                else
                                {
                                    str_error += cnt + ") " + " Please enter proper " + dgvGSTR1A5.Columns[j].HeaderText + ".\n";
                                }
                                cnt++;
                            }
                        }
                        if (ExcelValidFlag == true)
                        {
                            Ab++;
                            dt_new.Rows.Add();
                            int c = dt_new.Rows.Count;
                            for (int b = 0; b < dgvGSTR1A5.Columns.Count; b++)
                            {
                                if (dt_new.Columns.Count - 1 == b)
                                {
                                    dt_new.Rows[c - 1][b] = str_error;
                                }
                                else
                                {
                                    dt_new.Rows[c - 1][b] = Convert.ToString(dgvGSTR1A5.Rows[k].Cells[b].Value);
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
                    pbGSTR1.Visible = true;
                    // CHCK EXTENTION OF SELECTED FILE
                    if (fileExt.CompareTo(".csv") == 0 || fileExt.CompareTo(".~csv") == 0)
                    {
                        // CREATE DATATABLE AND SAVE GRID DATA
                        DataTable dt = new DataTable();
                        dt = (DataTable)dgvGSTR1A5.DataSource;

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
                                DisableControls(dgvGSTR1A5);

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
                                foreach (DataGridViewColumn col in dgvGSTR1A5.Columns)
                                {
                                    dtCsv.Columns[col.Index].ColumnName = col.Name.ToString();
                                    col.DataPropertyName = col.Name;
                                }
                                dt.AcceptChanges();
                                #endregion

                                // ASSIGN DATATABLE TO GRID
                                dgvGSTR1A5.DataSource = dt;

                                // ENABLE MAIN GRID
                                EnableControls(dgvGSTR1A5);
                            }
                            else
                            {
                                // IF THERE ARE NO RECORDS IN MAIN GRID

                                if (dtCsv != null && dtCsv.Rows.Count > 0)
                                {
                                    // IF THERE ARE RECORD PRESENT IN IMPORT FILE

                                    // DISABLE MAIN GRID
                                    DisableControls(dgvGSTR1A5);

                                    #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME AND ASSIGN TO MAIN GRID
                                    foreach (DataGridViewColumn col in dgvGSTR1A5.Columns)
                                    {
                                        dtCsv.Columns[col.Index].ColumnName = col.Name.ToString();
                                        col.DataPropertyName = col.Name;
                                    }
                                    dtCsv.AcceptChanges();

                                    // ASSIGN DATATABLE TO GRID
                                    dgvGSTR1A5.DataSource = dtCsv;

                                    // ENABLE CONTROL
                                    EnableControls(dgvGSTR1A5);
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
                            string[] colNo = { "colInvoiceNo", "colInvoiceValue", "colInvoiceTaxableVal", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmount" };
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
                EnableControls(dgvGSTR1A5);
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
                    for (int i = 1; i < dgvGSTR1A5.Columns.Count; i++)
                    {
                        Boolean flg = false;
                        for (int j = 0; j < csvData.Columns.Count; j++)
                        {
                            // CHECK GRID COLUMN IS PRESENT OR NOT IN IMPORTED EXCEL
                            if (dgvGSTR1A5.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim().Length >= 40)
                            {
                                string piece = dgvGSTR1A5.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim().Substring(0, 40);
                                string piece1 = string.Empty;

                                if (csvData.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim().Length >= 40)
                                    piece1 = csvData.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim().Substring(0, 40);
                                else
                                    piece1 = csvData.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim();

                                if (piece == piece1)
                                {
                                    // if grid column present in excel then its index as par grid column index
                                    flg = true;
                                    csvData.Columns[j].SetOrdinal(dgvGSTR1A5.Columns[i].Index - 1);
                                    break;
                                }
                            }
                            else if (dgvGSTR1A5.Columns[i].HeaderText.Replace(" ", "") == csvData.Columns[j].ColumnName.Replace(" ", "").Trim())
                            {
                                // IF GRID COLUMN PRESENT IN EXCEL THEN ITS INDEX AS PAR GRID COLUMN INDEX
                                flg = true;
                                csvData.Columns[j].SetOrdinal(dgvGSTR1A5.Columns[i].Index - 1);
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
                    if (csvData.Columns.Count >= dgvGSTR1A5.Columns.Count - 2)
                    {
                        for (int i = csvData.Columns.Count - 1; i > (dgvGSTR1A5.Columns.Count - 2); i--)
                        {
                            csvData.Columns.Remove(csvData.Columns[i]);
                        }
                    }
                    #endregion

                    #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME
                    foreach (DataGridViewColumn col in dgvGSTR1A5.Columns)
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
                if (dgvGSTR1A5.Rows.Count > 1)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID
                    pbGSTR1.Visible = true;
                    string csv = string.Empty;
                    // CREATE DATATABLE AND GET GRID DATA
                    DataTable dt = new DataTable();
                    dt = (DataTable)dgvGSTR1A5.DataSource;

                    #region ASSIGN COLUMN NAME TO CSV STRING
                    for (int i = 1; i < dgvGSTR1A5.Columns.Count; i++)
                    {
                        csv += dgvGSTR1A5.Columns[i].HeaderText + ',';
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
                if (dgvGSTR1A5.Rows.Count > 1)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID
                    pbGSTR1.Visible = true;

                    #region CREATING ITEXTSHARP TABLE FROM THE DATATABLE DATA AND ASSIGNING TABLE HEADER
                    PdfPTable pdfTable = new PdfPTable(dgvGSTR1A5.ColumnCount - 1);
                    pdfTable.DefaultCell.Padding = 3;
                    pdfTable.WidthPercentage = 100;
                    pdfTable.DefaultCell.BorderWidth = 0;
                    iTextSharp.text.Font fontHeader = iTextSharp.text.FontFactory.GetFont("Calibri", 6);

                    // ADD HEADER TO PDF TABLE
                    string headerName = "Gross Advance";
                    pdfTable = AssignHeader(pdfTable, headerName);
                    #endregion

                    #region ADDING HEADER ROW
                    int i = 0;
                    foreach (DataGridViewColumn column in dgvGSTR1A5.Columns)
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
                        foreach (DataGridViewRow row in dgvGSTR1A5.Rows)
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
                        foreach (DataGridViewRow row in dgvGSTR1A5.Rows)
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
                ce1.Colspan = dgvGSTR1A5.Columns.Count;
                ce1.VerticalAlignment = Element.ALIGN_CENTER;
                ce1.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(ce1);

                PdfPCell ce2 = new PdfPCell(new Phrase(" "));
                ce2.Colspan = dgvGSTR1A5.Columns.Count;
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

        #region JSON TRANSACTION

        #region JSON CLASS
        public class Itm
        {
            [DefaultValue("")]
            public double rt { get; set; }
            [DefaultValue("")]
            public double ad_amt { get; set; }
            public double iamt { get; set; }
            public double camt { get; set; }
            public double samt { get; set; }
            public double csamt { get; set; }
        }

        public class At
        {
            public string pos { get; set; }
            public string sply_ty { get; set; }
            public List<Itm> itms { get; set; }
        }

        public class RootObject
        {
            public string gstin { get; set; }
            public string fp { get; set; }
            public double gt { get; set; }
            public double cur_gt { get; set; }
            public List<At> at { get; set; }
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
                    pbGSTR1.Visible = true;
                    DataTable dt = new DataTable();

                    #region Bind Grid Data

                    #region ADD DATATABLE COLUMN

                    foreach (DataGridViewColumn col in dgvGSTR1A5.Columns)
                    {
                        dt.Columns.Add(col.Name.ToString());
                        col.DataPropertyName = col.Name;
                    }
                    #endregion

                    #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                    object[] rowValue = new object[dt.Columns.Count];

                    foreach (DataGridViewRow dr in dgvGSTR1A5.Rows)
                    {
                        if (dr.Index != dgvGSTR1A5.Rows.Count - 1)
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
                    dt.Columns.Remove("colName");
                    dt.Columns.Remove("colInvoiceNo");
                    dt.Columns.Remove("colInvoiceDate");
                    dt.AcceptChanges();
                    #endregion

                    #endregion

                    List<string> lstPOS = dt.Rows
                           .OfType<DataRow>()
                           .Select(x => Convert.ToString(x["colPOS"]).Trim())
                           .Distinct().ToList();

                    if (lstPOS != null && lstPOS.Count > 0)
                    {
                        ObjJson.gstin = CommonHelper.CompanyGSTN; // tax person GSTIN
                        ObjJson.fp = CommonHelper.GetReturnPeriod(); // current return period
                        ObjJson.gt = CommonHelper.TurnOver; // previous financial year turnover
                        ObjJson.cur_gt = Convert.ToDouble(CommonHelper.CurrentTurnOver); // current Finacial year turnover

                        List<At> objLstAT = new List<At>();

                        for (int i = 0; i < lstPOS.Count; i++)
                        {
                            if (lstPOS[i] != "")
                            {
                                string stCode = CommonHelper.GetStateCode(Convert.ToString(lstPOS[i]).Trim()).ToString(); //POS

                                At objAT = new At();
                                objAT.pos = stCode; //POS

                                if (Convert.ToString(CommonHelper.CompanyStateCode) == stCode)
                                    objAT.sply_ty = "INTRA"; // Supply Type
                                else
                                    objAT.sply_ty = "INTER"; // Supply Type

                                objLstAT.Add(objAT);
                                ObjJson.at = objLstAT;

                                List<string> lstRate = dt.Rows
                                               .OfType<DataRow>()
                                               .Where(x => lstPOS[i] == Convert.ToString(x["colPOS"]).Trim())
                                               .Select(x => Convert.ToString(x["colIGSTRate"]).Trim())
                                               .Distinct().ToList();

                                if (lstRate != null && lstRate.Count > 0)
                                {
                                    List<Itm> objItm = new List<Itm>();

                                    for (int k = 0; k < lstRate.Count; k++)
                                    {
                                        if (Convert.ToString(lstRate[k]).Trim() != "")
                                        {
                                            List<DataRow> lstDrRate = dt.Rows
                                                   .OfType<DataRow>()
                                                   .Where(x => lstPOS[i] == Convert.ToString(x["colPOS"]).Trim() && Convert.ToDecimal(lstRate[k]) == Convert.ToDecimal(x["colIGSTRate"]))
                                                   .Select(x => x)
                                                   .ToList();

                                            if (lstDrRate != null && lstDrRate.Count > 0)
                                            {
                                                Itm clsItm = new Itm();

                                                clsItm.rt = Convert.ToDouble(lstDrRate[0]["colIGSTRate"]); // Rate

                                                if (lstDrRate.Count == 1)
                                                {
                                                    if (!string.IsNullOrEmpty(Convert.ToString(lstDrRate[0]["colInvoiceValue"]).Trim()))
                                                        clsItm.ad_amt = Convert.ToDouble(Convert.ToString(lstDrRate[0]["colInvoiceValue"]).Trim()); // Advance received

                                                    if (!string.IsNullOrEmpty(Convert.ToString(lstDrRate[0]["colIGSTAmnt"]).Trim()))
                                                        clsItm.iamt = Convert.ToDouble(Convert.ToString(lstDrRate[0]["colIGSTAmnt"]).Trim()); // IGST Amount

                                                    if (!string.IsNullOrEmpty(Convert.ToString(lstDrRate[0]["colCGSTAmnt"]).Trim()))
                                                        clsItm.camt = Convert.ToDouble(Convert.ToString(lstDrRate[0]["colCGSTAmnt"]).Trim()); // CGST Amount

                                                    if (!string.IsNullOrEmpty(Convert.ToString(lstDrRate[0]["colSGSTAmnt"]).Trim()))
                                                        clsItm.samt = Convert.ToDouble(Convert.ToString(lstDrRate[0]["colSGSTAmnt"]).Trim()); // SGST Amount

                                                    if (!string.IsNullOrEmpty(Convert.ToString(lstDrRate[0]["colCessAmount"]).Trim()))
                                                        clsItm.csamt = Convert.ToDouble(Convert.ToString(lstDrRate[0]["colCessAmount"]).Trim()); // Cess Amount
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

                                                        if (Utility.IsDecimalOrNumber(Convert.ToString(lstDrRate[sr]["colCessAmount"]).Trim()))
                                                            cess = Convert.ToDouble(cess) + Convert.ToDouble(lstDrRate[sr]["colCessAmount"]);
                                                    }

                                                    clsItm.ad_amt = lstDrRate.Cast<DataRow>().Where(x => x["colInvoiceValue"] != null).Sum(x => Convert.ToString(x["colInvoiceValue"]).Trim() == "" ? 0 : Convert.ToDouble(x["colInvoiceValue"])); // Advance received

                                                    if (igst != null) { clsItm.iamt = Convert.ToDouble(igst); } // IGST value 
                                                    if (cgst != null) { clsItm.camt = Convert.ToDouble(cgst); } // CGST value
                                                    if (sgst != null) { clsItm.samt = Convert.ToDouble(sgst); } // SGST value
                                                    if (cess != null) { clsItm.csamt = Convert.ToDouble(cess); } // CESS value
                                                }

                                                objItm.Add(clsItm);
                                                ObjJson.at[i].itms = objItm;
                                            }
                                        }
                                    }
                                //}
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
                    save.FileName = "AT.json";
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

        #endregion

        public void SetGridViewColor()
        {
            try
            {
                // do not allow to auto generate columns
                dgvGSTR1A5.AutoGenerateColumns = false;

                // set height width of form
                this.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.90));
                this.Height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.77));

                // set width of header, main and total grid
                this.pnlHeader.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.885));
                this.dgvGSTR1A5.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.885));
                this.dgvGSTR1A5Total.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.885));

                // set height of main grid
                this.dgvGSTR1A5.Height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.65));
                this.dgvGSTR1A5Total.Height = 43;

                // set location of header,loading pic, checkbox and main and total grid
                //this.pnlHeader.Location = new System.Drawing.Point(12, 0);
                ////this.lnkback.Location = new System.Drawing.Point(Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.85)), 5);
                //this.dgvGSTR1A5.Location = new System.Drawing.Point(12, Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.05)));
                //this.dgvGSTR1A5Total.Location = new System.Drawing.Point(12, Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.71)));
                //this.ckboxHeader.Location = new System.Drawing.Point(32, Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.135)));
                //this.pbGSTR1.Location = new System.Drawing.Point(Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.45)), Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.30)));

                dgvGSTR1A5.EnableHeadersVisualStyles = false;
                dgvGSTR1A5.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(23, 196, 187);
                dgvGSTR1A5.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

                this.dgvGSTR1A5.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                this.dgvGSTR1A5.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                this.dgvGSTR1A5.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

                foreach (DataGridViewColumn column in dgvGSTR1A5.Columns)
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
                DataTable dt = (DataTable)dgvGSTR1A5.DataSource;
                if (dt == null)
                {
                    MessageBox.Show("Kindly save record(s) before search!!!", "Alert", MessageBoxButtons.OK);
                    return;
                }
                if (cmbFilter.SelectedValue.ToString() == "")
                    ((DataTable)dgvGSTR1A5.DataSource).DefaultView.RowFilter = string.Format("colSequence like '%{0}%' or colGSTIN like '%{0}%' or colName like '%{0}%' or colInvoiceNo like '%{0}%' or colInvoiceDate like '%{0}%' or colInvoiceValue like '%{0}%' or colIGSTRate like '%{0}%' or colIGSTAmnt like '%{0}%' or colCGSTAmnt like '%{0}%' or colSGSTAmnt like '%{0}%' or colCessAmount like '%{0}%' or colPOS like '%{0}%'", txtSearch.Text.Trim().Replace("'", "''"));
                else
                    ((DataTable)dgvGSTR1A5.DataSource).DefaultView.RowFilter = string.Format("" + cmbFilter.SelectedValue + " like '%{0}%'", txtSearch.Text.Trim().Replace("'", "''"));
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

        private void dgvGSTR1A5_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                // SET INDEX OF USER ADDED ROW IN MAIN GRID
                dgvGSTR1A5.Rows[e.Row.Index - 1].Cells[1].Value = e.Row.Index;
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

        private void dgvGSTR1A5_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                // SET SEQUNCING AFTER USER DELETING ROW IN GRID

                for (int i = e.Row.Index; i < dgvGSTR1A5.Rows.Count - 1; i++)
                {
                    dgvGSTR1A5.Rows[i].Cells["colSequence"].Value = i;
                }

                // TOTAL CALCULATION
                string[] colNo = { "colInvoiceNo", "colInvoiceValue", "colInvoiceTaxableVal", "colIGSTAmnt", "colCGSTAmnt", "colSGSTAmnt", "colCessAmount" };
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
                if (c.Name != "SPQGSTR1B2B" && c.Name != "dgvGSTR1A5Total")
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
        private void dgvGSTR1A5_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                // SET TOTAL GRID OFFSET AS PAR MAIN GRID SCROLL
                this.dgvGSTR1A5Total.HorizontalScrollingOffset = this.dgvGSTR1A5.HorizontalScrollingOffset;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
            }
        }

        private void dgvGSTR1A5Total_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                // SET TOTAL GRID OFFSET AS PAR MAIN GRID SCROLL
                this.dgvGSTR1A5.HorizontalScrollingOffset = this.dgvGSTR1A5Total.HorizontalScrollingOffset;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
            }
        }
        #endregion

        #region CHECK ALL AND UNCHECK ALL

        private void dgvGSTR1A5_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                // CHECK FIRST COLUMN HEADER PRESSED AND MAIN GRID HAVING RECORDS
                if (e.ColumnIndex == 0 && dgvGSTR1A5.Rows.Count > 1)
                {
                    // CHECK AND UNCHECK CHECK BOX OF HEADER FOR SELECTING AND UNSELECTING ALL RECORDS
                    if (dgvGSTR1A5.Columns[e.ColumnIndex].HeaderText == "Check All")
                        ckboxHeader.Checked = true;
                    else if (dgvGSTR1A5.Columns[e.ColumnIndex].HeaderText == "Uncheck All")
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
                if (dgvGSTR1A5.Rows.Count > 1)
                {
                    if (ckboxHeader.Checked)
                    {
                        // IF CHECK BOX IS CHECKED
                        pbGSTR1.Visible = true;
                        // SET CHECK BOX COLUMN VALUE AS TRUE
                        for (int i = 0; i < dgvGSTR1A5.Rows.Count - 1; i++)
                        {
                            dgvGSTR1A5.Rows[i].Cells[0].Value = "True";
                            Application.DoEvents();
                        }

                        // CHANGE HEADER TEXT
                        dgvGSTR1A5.Columns[0].HeaderText = "Uncheck All";
                        pbGSTR1.Visible = false;
                    }
                    else if (ckboxHeader.Checked == false)
                    {
                        // IF CHECK BOX IS UNCHECKED
                        pbGSTR1.Visible = true;
                        // SET CHECK BOX COLUMN VALUE AS FALSE
                        for (int i = 0; i < dgvGSTR1A5.Rows.Count - 1; i++)
                        {
                            dgvGSTR1A5.Rows[i].Cells[0].Value = "False";
                            Application.DoEvents();
                        }

                        // CHANGE HEADER TEXT
                        dgvGSTR1A5.Columns[0].HeaderText = "Check All";
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

        private void dgvGSTR1A5Total_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                if (dgvGSTR1A5Total.Rows.Count > 0)
                {
                    DataGridViewRow row = this.dgvGSTR1A5Total.RowTemplate;
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

        private void lnkback_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SPQGSTR1AdvanceReceived obj = new SPQGSTR1AdvanceReceived();
            obj.MdiParent = this.MdiParent;
            Utility.CloseAllOpenForm();
            obj.Dock = DockStyle.Fill;
            obj.Show();


            ((SPQMDI)Application.OpenForms["SPQMDI"]).HideExtraToolsMenu();
            ((SPQMDI)Application.OpenForms["SPQMDI"]).HideCompanyDetailMenu();
        }

        private void frmGSTR1A5_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName("");
        }

        private void lnkClose_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SPQGSTR1AdvanceReceived obj = new SPQGSTR1AdvanceReceived();
            obj.MdiParent = this.MdiParent;
            Utility.CloseAllOpenForm();
            obj.Dock = DockStyle.Fill;
            obj.Show();


            ((SPQMDI)Application.OpenForms["SPQMDI"]).HideExtraToolsMenu();
            ((SPQMDI)Application.OpenForms["SPQMDI"]).HideCompanyDetailMenu();
        }

        public void ValidataAndGetGSTIN()
        {
            try
            {
                if (dgvGSTR1A5.Rows.Count > 1)
                {
                    pbGSTR1.Visible = true;
                    new PrefillHelper().GetNameByGSTIN(dgvGSTR1A5, "colGSTIN", "colName");

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

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}