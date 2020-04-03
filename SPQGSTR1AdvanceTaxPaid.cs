using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using SPEQTAGST.BAL;
using SPEQTAGST.BAL.M264r1;
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
    public partial class SPQGSTR1AdvanceTaxPaid : Form
   {
        r1Publicclass objGSTR1A5 = new r1Publicclass();

        public SPQGSTR1AdvanceTaxPaid()
        {
            InitializeComponent();
            // SET GRID PROPERTY
            SetGridViewColor();
            //Bind Data
            GetData();
            // TOTAL CALCULATION
            string[] colNo = {"colGrossAdvRcv", "colIGST", "colCGST", "colSGST", "colCess" };
            GetTotal(colNo);
            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName(CommonHelper.ReturnName);
            BindFilter();

            dgvGSTR1A5.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dgvGSTR1A5.EnableHeadersVisualStyles = false;
            dgvGSTR1A5.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);
            dgvGSTR1A5.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvGSTR1A5.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dgvGSTR1A5Total.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        #region Filter
        class colList
        {
            public string colHeaderText { get; set; }

            public string colName { get; set; }
        }

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

        private void PrefillData()
        {
            try
            {
                #region JSON DATA STATIC
                string _json = "{ \"b2b\": [ { \"ctin\": \"01AABCE2207R1Z5\", \"cfs\": \"Y\", \"inv\": [ { \"flag\": \"M\", \"updby\": \"R\", \"chksum\": \"AflJufPlFStqKBZ\", \"inum\": \"S008400\", \"idt\": \"24-11-2016\", \"val\": 729248.16, \"pos\": \"06\", \"rchrg\": \"N\", \"prs\": \"Y\", \"od_num\": \"S008400\", \"od_dt\": \"03-02-2016\", \"etin\": \"01AABCE5507R1Z4\", \"itms\": [ { \"num\": 1, \"itm_det\": { \"ty\": \"G\", \"hsn_sc\": \"G1221\", \"txval\": 10000, \"irt\": 3, \"iamt\": 833.33, \"crt\": 4, \"camt\": 500, \"srt\": 5, \"samt\": 900, \"csrt\": 2, \"csamt\": 500 } }, { \"num\": 2, \"itm_det\": { \"ty\": \"S\", \"hsn_sc\": \"S1231\", \"txval\": 10000, \"irt\": 4, \"iamt\": 625.33, \"crt\": 6, \"camt\": 333.33, \"srt\": 5, \"samt\": 900, \"csrt\": 3, \"csamt\": 333.33 } } ] } ] } ] }";
                #endregion

                RootObject obj = JsonConvert.DeserializeObject<RootObject>(_json);

                #region ADD DATATABLE COLUMN
                DataTable dt = new DataTable();

                foreach (DataGridViewColumn col in dgvGSTR1A5.Columns)
                {
                    if (col.Name.ToLower() != "colchk")
                    {
                        dt.Columns.Add(col.Name.ToString());
                    }
                }

                #endregion

                #region ASSIGN GRIDVIEW ROWS IN DATATABLE
                //for (int i = 0; i < obj.b2b.Count; i++)
                //{
                //    for (int j = 0; j < obj.b2b[i].inv.Count; j++)
                //    {
                //        for (int k = 0; k < obj.b2b[i].inv[j].itms.Count; k++)
                //        {
                //            dt.Rows.Add();
                //            //ROOT START
                //            dt.Rows[dt.Rows.Count - 1]["colGSTIN"] = Convert.ToString(obj.b2b[i].ctin);
                //            //ROOT END

                //            //INVOICE DATA START
                //            dt.Rows[dt.Rows.Count - 1]["colInvoiceNo"] = Convert.ToString(obj.b2b[i].inv[j].inum);//INVOICE NO.
                //            dt.Rows[dt.Rows.Count - 1]["colInvoiceDate"] = Convert.ToString(obj.b2b[i].inv[j].idt);//INVOICE DATE
                //            dt.Rows[dt.Rows.Count - 1]["colPOS"] = Convert.ToString(obj.b2b[i].inv[j].pos);//POS
                //            dt.Rows[dt.Rows.Count - 1]["colTax"] = (Convert.ToString(obj.b2b[i].inv[j].prs) == "Y" ? "true" : "false");//TAX
                //            dt.Rows[dt.Rows.Count - 1]["colIndSupAttac"] = Convert.ToString(obj.b2b[i].inv[j].rchrg);//INDICATE SUPPLY ATTACK
                //            dt.Rows[dt.Rows.Count - 1]["colInvoiceValue"] = Convert.ToString(obj.b2b[i].inv[j].val);//SUPPLYER INVOICE VALUE
                //            //INVOICE DATA END

                //            //ITEM DATA START
                //            dt.Rows[dt.Rows.Count - 1]["colInvoiceGoodsServi"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.ty);//GOODS AND SERVICE
                //            dt.Rows[dt.Rows.Count - 1]["colInvoiceHSNSAC"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.hsn_sc);//HSN
                //            dt.Rows[dt.Rows.Count - 1]["colInvoiceTaxableVal"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.txval);//TAXABLE VALUE
                //            dt.Rows[dt.Rows.Count - 1]["colIGSTRate"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.irt);//IGST RATE
                //            dt.Rows[dt.Rows.Count - 1]["colIGST"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.iamt);//IGST AMOUNT
                //            dt.Rows[dt.Rows.Count - 1]["colCGSTRate"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.crt);//CGST RATE
                //            dt.Rows[dt.Rows.Count - 1]["colCGST"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.camt);//CGST AMOUNT
                //            dt.Rows[dt.Rows.Count - 1]["colSGSTRate"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.srt);//SGST RATE
                //            dt.Rows[dt.Rows.Count - 1]["colSGST"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.samt);//SGST AMOUNT

                //            #region New Parameter
                //            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].inv[j].flag);
                //            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].inv[j].updby);
                //            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].inv[j].od_num);
                //            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].inv[j].od_dt);
                //            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].inv[j].etin);
                //            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].cfs);

                //            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.csrt);
                //            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.csamt);
                //            #endregion

                //            //ITEM DATA END
                //        }
                //    }
                //}
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    dt.Rows[i]["colSequence"] = Convert.ToString(i + 1);
                //}
                //dt.AcceptChanges();
                //dgvGSTR1A5.DataSource = dt;
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
                string Query = "Select * from SPQR1GrossAdvance where Fld_Month='" + CommonHelper.SelectedMonth + "' and Fld_FileStatus != 'Total'";
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


                    var result = (from r in dt.AsEnumerable()
                                  group r by new
                                  {
                                      colRate = r["Fld_Rate"],
                                      colPOS = r["Fld_POS"],
                                  } into g

                                  select new
                                  {
                                      colRate = g.Key.colRate,
                                      colPOS = g.Key.colPOS,
                                      colGrossAdvRcv = g.Sum(x => Convert.ToString(x["Fld_GrossAdvRcv"]).Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_GrossAdvRcv"])),
                                      colIGST = g.Sum(x => Convert.ToString(x["Fld_IGSTAmnt"]).Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_IGSTAmnt"])),
                                      colCGST = g.Sum(x => Convert.ToString(x["Fld_CGSTAmnt"]).Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_CGSTAmnt"])),
                                      colSGST = g.Sum(x => Convert.ToString(x["Fld_SGSTAmnt"]).Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_SGSTAmnt"])),
                                      colCess = g.Sum(x => Convert.ToString(x["Fld_CessAmount"]).Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_CessAmount"])),
                                  });
                    int i = 1;
                    foreach (var item in result)
                    {
                        dgvGSTR1A5.Rows.Add("", i, item.colRate, item.colPOS, Utility.DisplayIndianCurrency(Convert.ToString(item.colGrossAdvRcv)), Utility.DisplayIndianCurrency(Convert.ToString(item.colIGST)), Utility.DisplayIndianCurrency(Convert.ToString(item.colCGST)), Utility.DisplayIndianCurrency(Convert.ToString(item.colSGST)), Utility.DisplayIndianCurrency(Convert.ToString(item.colCess)));
                        i++;
                    }
                    Application.DoEvents();
                    pbGSTR1.Visible = false;
                }
                else
                    ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(0);
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
                if (dgvGSTR1A5.Rows.Count > 0)
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

                        // CREATE DATAROW TO STORE GRID COLUMN CALCULATION
                        DataRow dr = dtTotal.NewRow();
                        dr["colTGrossAdvRcv"] = dgvGSTR1A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colGrossAdvRcv"].Value != null).Sum(x => x.Cells["colGrossAdvRcv"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colGrossAdvRcv"].Value)).ToString();
                        dr["colTIGST"] = dgvGSTR1A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colIGST"].Value != null).Sum(x => x.Cells["colIGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIGST"].Value)).ToString();
                        dr["colTCGST"] = dgvGSTR1A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCGST"].Value != null).Sum(x => x.Cells["colCGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCGST"].Value)).ToString();
                        dr["colTSGST"] = dgvGSTR1A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colSGST"].Value != null).Sum(x => x.Cells["colSGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colSGST"].Value)).ToString();
                        dr["colTCess"] = dgvGSTR1A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCess"].Value != null).Sum(x => x.Cells["colCess"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCess"].Value)).ToString();

                        // ADD DATAROW TO DATATABLE
                        dtTotal.Rows.Add(dr);

                        for (int i = 0; i < dtTotal.Rows.Count; i++)
                        {
                            for (int j = 0; j < dtTotal.Columns.Count; j++)
                            {
                                string ColName = dtTotal.Columns[j].ColumnName;
                                if (ColName == "colTGrossAdvRcv" || ColName == "colTIGST" || ColName == "colTCGST" || ColName == "colTSGST" || ColName == "colTCess")
                                    dtTotal.Rows[i][j] = Utility.DisplayIndianCurrency(Convert.ToString(dtTotal.Rows[i][j]));
                            }
                        }

                        dtTotal.AcceptChanges();

                        // ASSIGN DATATABLE TO GRID
                        dgvGSTR1A5Total.DataSource = dtTotal;

                        // SET GRID ROW HEIGHT AND ASSIGN TOTAL HEADER
                        dgvGSTR1A5Total.Rows[0].Height = 30;
                        dgvGSTR1A5Total.Rows[0].Cells[1].Value = "TOTAL";
                        #endregion
                    }
                    else if (dgvGSTR1A5Total.Rows.Count == 1)
                    {
                        #region IF TOTAL GRID HAVING ONLY ONE RECORDS
                        // SET GRID ROW HEIGHT AND ASSIGN TOTAL HEADER
                        dgvGSTR1A5Total.Rows[0].Height = 30;
                        dgvGSTR1A5Total.Rows[0].Cells[1].Value = "TOTAL";

                        // CALCULATE TOTAL ONLY SPECIFIC COLUMN
                        foreach (var item in colNo)
                        {
                            if (item == "colGrossAdvRcv")
                                dgvGSTR1A5Total.Rows[0].Cells["colTGrossAdvRcv"].Value = Utility.DisplayIndianCurrency(dgvGSTR1A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colGrossAdvRcv"].Value != null).Sum(x => x.Cells["colGrossAdvRcv"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colGrossAdvRcv"].Value)).ToString());
                            else if (item == "colIGST")
                                dgvGSTR1A5Total.Rows[0].Cells["colTIGST"].Value = Utility.DisplayIndianCurrency(dgvGSTR1A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colIGST"].Value != null).Sum(x => x.Cells["colIGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIGST"].Value)).ToString());
                            else if (item == "colCGST")
                                dgvGSTR1A5Total.Rows[0].Cells["colTCGST"].Value = Utility.DisplayIndianCurrency(dgvGSTR1A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCGST"].Value != null).Sum(x => x.Cells["colCGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCGST"].Value)).ToString());
                            else if (item == "colSGST")
                                dgvGSTR1A5Total.Rows[0].Cells["colSGST"].Value = Utility.DisplayIndianCurrency(dgvGSTR1A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colSGST"].Value != null).Sum(x => x.Cells["colSGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colSGST"].Value)).ToString());
                            else if (item == "colCess")
                                dgvGSTR1A5Total.Rows[0].Cells["colTCess"].Value = Utility.DisplayIndianCurrency(dgvGSTR1A5.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCess"].Value != null).Sum(x => x.Cells["colCess"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCess"].Value)).ToString());
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
                        dt = (DataTable)dgvGSTR1A5.DataSource;

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
                            string[] colNo = { "colInvoiceNo", "colInvoiceValue", "colInvoiceTaxableVal", "colIGST", "colCGST", "colSGST", "colCess" };
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
                        OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [B2BA_1A5$]", con);
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
                        for (int i = 1; i < dgvGSTR1A5.Columns.Count; i++)
                        {
                            Boolean flg = false;
                            for (int j = 0; j < dtexcel.Columns.Count; j++)
                            {
                                // CHECK GRID COLUMN IS PRESENT OR NOT IN IMPORTED EXCEL
                                if (dgvGSTR1A5.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim().Length >= 40)
                                {
                                    string piece = dgvGSTR1A5.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim().Substring(0, 40);
                                    string piece1 = string.Empty;

                                    if (dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim().Length >= 40)
                                        piece1 = dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim().Substring(0, 40);
                                    else
                                        piece1 = dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim();

                                    if (piece == piece1)
                                    {
                                        // if grid column present in excel then its index as par grid column index
                                        flg = true;
                                        dtexcel.Columns[j].SetOrdinal(dgvGSTR1A5.Columns[i].Index - 1);
                                        break;
                                    }
                                }
                                else if (dgvGSTR1A5.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim() == dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim())
                                {
                                    // IF GRID COLUMN PRESENT IN EXCEL THEN ITS INDEX AS PAR GRID COLUMN INDEX
                                    flg = true;
                                    dtexcel.Columns[j].SetOrdinal(dgvGSTR1A5.Columns[i].Index - 1);
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
                        if (dtexcel.Columns.Count >= dgvGSTR1A5.Columns.Count - 2)
                        {
                            for (int i = dtexcel.Columns.Count - 1; i > (dgvGSTR1A5.Columns.Count - 2); i--)
                            {
                                dtexcel.Columns.Remove(dtexcel.Columns[i]);
                            }
                        }
                        dtexcel.AcceptChanges();
                        #endregion

                        #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME
                        foreach (DataGridViewColumn col in dgvGSTR1A5.Columns)
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
                if (dgvGSTR1A5.Rows.Count > 1)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID
                    pbGSTR1.Visible = true;

                    #region CREATE WORKBOOK AND ASSIGN COLUMNNAME
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook WB = excelApp.Workbooks.Add(Missing.Value);

                    Excel.Worksheet newWS = (Excel.Worksheet)excelApp.Worksheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    newWS.Name = "B2BA_1A5";

                    // DELETE UNUSED WORKSHEETS FROM WORKBOOK
                    foreach (Excel.Worksheet ws in WB.Worksheets)
                    {
                        if (ws.Name != "B2BA_1A5")
                            ((Excel.Worksheet)WB.Worksheets[ws.Name]).Delete();
                    }

                    // ASSIGN COLUMN HEADER AS PAR THE GRID HEADER
                    for (int i = 1; i < dgvGSTR1A5.Columns.Count; i++)
                    {
                        newWS.Cells[1, i] = dgvGSTR1A5.Columns[i].HeaderText.ToString();

                        // SET COLUMN WIDTH
                        if (i == 1)
                            ((Excel.Range)newWS.Cells[1, i]).ColumnWidth = 7;
                        else if (i == 2)
                            ((Excel.Range)newWS.Cells[1, i]).ColumnWidth = 20;
                        else
                            ((Excel.Range)newWS.Cells[1, i]).ColumnWidth = 15;
                    }

                    // GET RANGE AND SET DIFFRENT PROPERTIES
                    Excel.Range headerRange = (Excel.Range)newWS.get_Range((Excel.Range)newWS.Cells[1, 1], (Excel.Range)newWS.Cells[1, dgvGSTR1A5.Columns.Count - 1]);
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
                            for (int j = 1; j < dgvGSTR1A5.Columns.Count; j++)
                            {
                                arr[i, j - 1] = dgvGSTR1A5.Rows[i].Cells[j].Value.ToString();
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
                                for (int j = 1; j < dgvGSTR1A5.Columns.Count; j++)
                                {
                                    arr[i, j - 1] = dgvGSTR1A5.Rows[i].Cells[j].Value.ToString();
                                }
                            }
                        }
                    }

                    //SET EXCEL RANGE TO PASTE THE DATA
                    Excel.Range top = (Excel.Range)newWS.Cells[2, 1];
                    Excel.Range bottom = (Excel.Range)newWS.Cells[dgvGSTR1A5.Rows.Count, dgvGSTR1A5.Columns.Count];
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
                            string[] colNo = { "colInvoiceNo", "colInvoiceValue", "colInvoiceTaxableVal", "colIGST", "colCGST", "colSGST", "colCess" };
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

        public void SetGridViewColor()
        {
            try
            {
                // do not allow to auto generate columns
                //dgvGSTR1A5.AutoGenerateColumns = false;

                //// set height width of form
                //this.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.70));
                //this.Height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.90));

                //// set width of header, main and total grid
                //this.panel1.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.754));
                //this.dgvGSTR1A5.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.754));
                //this.dgvGSTR1A5Total.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.754));

                //// set height of main grid
                //this.dgvGSTR1A5.Height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.90));

                //// set location of header,loading pic, checkbox and main and total grid
                //this.panel1.Location = new System.Drawing.Point(12, 0);
                //this.lnkClose.Location = new System.Drawing.Point(Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.53)), 1);
                //this.dgvGSTR1A5.Location = new System.Drawing.Point(12, Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.05)));
                //this.dgvGSTR1A5Total.Location = new System.Drawing.Point(12, Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.71)));              
                ////this.pbGSTR1.Location = new System.Drawing.Point(Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.45)), Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.30)));

                dgvGSTR1A5.EnableHeadersVisualStyles = false;
                dgvGSTR1A5.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(23, 196, 187);
                dgvGSTR1A5.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

                this.dgvGSTR1A5.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                this.dgvGSTR1A5.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                this.dgvGSTR1A5.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

                foreach (DataGridViewColumn column in dgvGSTR1A5.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.Automatic;
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
                //DataTable dt = (DataTable)dgvGSTR1A5.DataSource;
                //int dt=dgvGSTR1A5.RowCount;
                //if (dt == 0)
                //{
                //    MessageBox.Show("Kindly save record(s) before search!!!", "Alert", MessageBoxButtons.OK);
                //    return;
                //}
                //if (cmbFilter.SelectedValue.ToString() == "")
                //{
                //    ((DataTable)dgvGSTR1A5.DataSource).DefaultView.RowFilter = string.Format("colSequence like '%{0}%' or colRate like '%{0}%' or colIGST like '%{0}%' or colCGST like '%{0}%' or colSGST like '%{0}%' or colCess like '%{0}%' or colPOS like '%{0}%'", txtSearch.Text.Trim().Replace("'", "''"));
                //}
                //else
                //{
                //    ((DataTable)dgvGSTR1A5.DataSource).DefaultView.RowFilter = string.Format("" + cmbFilter.SelectedValue + " like '%{0}%'", txtSearch.Text.Trim().Replace("'", "''"));
                //}
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

        private void dgvGSTR1A5Total_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                this.dgvGSTR1A5.ClearSelection();
                this.dgvGSTR1A5Total.ClearSelection();
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

        private void lnkClose_Click(object sender, EventArgs e)
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
    }
}