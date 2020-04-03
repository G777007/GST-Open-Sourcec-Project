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
    public partial class SPQGSTR1B2CL : Form
    {
        r1Publicclass objGSTR6 = new r1Publicclass();

        public SPQGSTR1B2CL()
        {
            InitializeComponent();
            // set grid property
            SetGridViewColor();
            // Bind data
            GetData();
            // total calculation
            string[] colNo = { "colInvoiceNo", "colInvoiceValue", "colTaxableVal", "colIGSTAmnt", "colCessAmnt" };
            GetTotal(colNo);
            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName(CommonHelper.ReturnName);

            pbGSTR1.Visible = false;
            BindFilter();

            dgvGSTR16.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dgvGSTR16.EnableHeadersVisualStyles = false;
            dgvGSTR16.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);
            dgvGSTR16.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvGSTR16.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dgvGSTR16Total.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        #region Filter

        private void PrefillData()
        {
            try
            {
                #region JSON DATA STATIC
                string _json = "{ \"b2cl\": [ { \"state_cd\": 28, \"inv\": [ { \"cname\": \"R_Glasswork Enterprise\", \"inum\": \"B129840\", \"idt\": \"14-04-2016\", \"val\": 1000, \"pos\": \"0\", \"pro_ass\": \"Y\", \"itms\": [ { \"num\": 1, \"itm_det\": { \"ty\": \"G\", \"hsn_sc\": \"82011000\", \"txval\": 100, \"irt\": 10, \"iamt\": 1000, \"crt\": 0, \"camt\": 0, \"srt\": 0, \"samt\": 0 } }, { \"num\": 2, \"itm_det\": { \"ty\": \"G\", \"hsn_sc\": \"82011000\", \"txval\": 100, \"irt\": 10, \"iamt\": 1000, \"crt\": 0, \"camt\": 0, \"srt\": 0, \"samt\": 0 } }, { \"num\": 3, \"itm_det\": { \"ty\": \"G\", \"hsn_sc\": \"82011000\", \"txval\": 100, \"irt\": 10, \"iamt\": 1000, \"crt\": 0, \"camt\": 0, \"srt\": 0, \"samt\": 0 } } ] } ] }, { \"state_cd\": 28, \"inv\": [ { \"chksum\": \"nJGrLNmyfgpjQMP\", \"cname\": \"R_Glasswork Enterprise\", \"inum\": \"B129840\", \"idt\": \"10-03-2016\", \"val\": 1000, \"pos\": \"0\", \"pro_ass\": \"Y\", \"itms\": [ { \"num\": 1, \"itm_det\": { \"ty\": \"S\", \"hsn_sc\": \"82011000\", \"txval\": 100, \"irt\": 10, \"iamt\": 1000, \"crt\": 0, \"camt\": 0, \"srt\": 0, \"samt\": 0 } }, { \"num\": 2, \"itm_det\": { \"ty\": \"S\", \"hsn_sc\": \"82011000\", \"txval\": 100, \"irt\": 10, \"iamt\": 1000, \"crt\": 0, \"camt\": 0, \"srt\": 0, \"samt\": 0 } }, { \"num\": 3, \"itm_det\": { \"ty\": \"S\", \"hsn_sc\": \"82011000\", \"txval\": 100, \"irt\": 10, \"iamt\": 1000, \"crt\": 0, \"camt\": 0, \"srt\": 0, \"samt\": 0 } } ] } ] } ] }";
                #endregion

                RootObject obj = JsonConvert.DeserializeObject<RootObject>(_json);

                #region ADD DATATABLE COLUMN
                DataTable dt = new DataTable();

                foreach (DataGridViewColumn col in dgvGSTR16.Columns)
                {
                    if (col.Name.ToLower() != "colchk")
                    {
                        dt.Columns.Add(col.Name.ToString());
                    }
                }

                #endregion

                #region ASSIGN GRIDVIEW ROWS IN DATATABLE
                for (int i = 0; i < obj.b2cl.Count; i++)
                {
                    //dt.Rows.Add();
                    ////ROOT START
                    //dt.Rows[dt.Rows.Count - 1]["colSequence"] = Convert.ToString(i + 1);
                    //dt.Rows[dt.Rows.Count - 1]["colShareHolderName"] = Convert.ToString(obj.b2b[i].ctin);
                    ////ROOT END

                    for (int j = 0; j < obj.b2cl[i].inv.Count; j++)
                    {
                        //dt.Rows[dt.Rows.Count - 1]["colInvoiceNo"] = Convert.ToString(obj.b2b[i].inv[j].inum);//INVOICE NO.
                        //dt.Rows[dt.Rows.Count - 1]["colInvoiceDate"] = Convert.ToString(obj.b2b[i].inv[j].idt);//INVOICE DATE
                        //dt.Rows[dt.Rows.Count - 1]["colPOS"] = Convert.ToString(obj.b2b[i].inv[j].pos);//POS
                        //dt.Rows[dt.Rows.Count - 1]["colTax"] = (Convert.ToString(obj.b2b[i].inv[j].pro_ass) == "Y"? "true" : "false");//TAX
                        //dt.Rows[dt.Rows.Count - 1]["colIndSupAttac"] = Convert.ToString(obj.b2b[i].inv[j].rchrg);//INDICATE SUPPLY ATTACK
                        //dt.Rows[dt.Rows.Count - 1]["colInvoiceValue"] = Convert.ToString(obj.b2b[i].inv[j].val);//SUPPLYER INVOICE VALUE

                        for (int k = 0; k < obj.b2cl[i].inv[j].itms.Count; k++)
                        {
                            dt.Rows.Add();
                            //ROOT START
                            dt.Rows[dt.Rows.Count - 1]["colreciptStateCode"] = Convert.ToString(obj.b2cl[i].state_cd);
                            //ROOT END

                            //INVOICE DATA START
                            dt.Rows[dt.Rows.Count - 1]["colNameOfRec"] = Convert.ToString(obj.b2cl[i].inv[j].cname);//INVOICE NO.
                            dt.Rows[dt.Rows.Count - 1]["colInvoiceNo"] = Convert.ToString(obj.b2cl[i].inv[j].inum);//INVOICE NO.
                            dt.Rows[dt.Rows.Count - 1]["colInvoiceDate"] = Convert.ToString(obj.b2cl[i].inv[j].idt);//INVOICE DATE
                            dt.Rows[dt.Rows.Count - 1]["colPOS"] = Convert.ToString(obj.b2cl[i].inv[j].pos);//POS
                            dt.Rows[dt.Rows.Count - 1]["colTax"] = (Convert.ToString(obj.b2cl[i].inv[j].prs) == "Y" ? "true" : "false");//TAX
                            dt.Rows[dt.Rows.Count - 1]["colInvoiceValue"] = Convert.ToString(obj.b2cl[i].inv[j].val);//SUPPLYER INVOICE VALUE

                            //INVOICE DATA END

                            //ITEM DATA START
                            dt.Rows[dt.Rows.Count - 1]["colInvoiceGoodsServi"] = Convert.ToString(obj.b2cl[i].inv[j].itms[k].itm_det.ty);//GOODS AND SERVICE
                            dt.Rows[dt.Rows.Count - 1]["colInvoiceHSNSAC"] = Convert.ToString(obj.b2cl[i].inv[j].itms[k].itm_det.hsn_sc);//HSN
                            dt.Rows[dt.Rows.Count - 1]["colTaxableVal"] = Convert.ToString(obj.b2cl[i].inv[j].itms[k].itm_det.txval);//TAXABLE VALUE
                            dt.Rows[dt.Rows.Count - 1]["colRate"] = Convert.ToString(obj.b2cl[i].inv[j].itms[k].itm_det.irt);//IGST RATE
                            dt.Rows[dt.Rows.Count - 1]["colIGSTAmnt"] = Convert.ToString(obj.b2cl[i].inv[j].itms[k].itm_det.iamt);//IGST AMOUNT
                            dt.Rows[dt.Rows.Count - 1]["colCessRate"] = Convert.ToString(obj.b2cl[i].inv[j].itms[k].itm_det.csrt);//Cess RATE
                            dt.Rows[dt.Rows.Count - 1]["colCessAmnt"] = Convert.ToString(obj.b2cl[i].inv[j].itms[k].itm_det.csamt);//Cess AMOUNT
                            //ITEM DATA END

                            #region New parameter
                            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2cl[i].inv[j].od_num);
                            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2cl[i].inv[j].od_dt;
                            //dt.Rows[dt.Rows.Count - 1][""] = Convert.ToString(obj.b2cl[i].inv[j].etin);
                            #endregion
                        }
                    }
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["colSequence"] = Convert.ToString(i + 1);
                }
                dt.AcceptChanges();
                dgvGSTR16.DataSource = dt;
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("Prefill Data Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindFilter()
        {
            try
            {
                List<colList> lstColumns = new List<colList>();
                for (int i = 0; i < dgvGSTR16.Columns.Count; i++)
                {
                    if (i > 1)
                    {
                        string HeaderText = dgvGSTR16.Columns[i].HeaderText;
                        string Name = dgvGSTR16.Columns[i].Name;
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

        private void GetData()
        {
            try
            {
                DataTable dt = new DataTable();
                string Query = "Select  "+
                    " Fld_Id,Fld_Sequence,Fld_Party,Fld_POS,Fld_InvoiceNo,Fld_InvoiceDate,Fld_TaxableValue,Fld_Rate,Fld_IGST,Fld_Cess,Fld_InvoiceValue,Fld_GSTINEComm, "+
                    " Fld_FileStatus, Fld_Month, Fld_FinancialYear "+
                    " from SPQR1B2CL where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
                Application.DoEvents();
                dt = objGSTR6.GetDataGSTR1(Query);

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
                    dt.Columns.Remove("Fld_FileStatus");
                    dt.Columns.Remove("Fld_ID");

                    dt.Columns.Add(new DataColumn("colChk"));
                    dt.Columns["colChk"].SetOrdinal(0);
                    dt.Columns.Add(new DataColumn("colError"));

                    foreach (DataGridViewColumn col in dgvGSTR16.Columns)
                    {
                        dt.Columns[col.Index].ColumnName = col.Name.ToString();
                        col.DataPropertyName = col.Name;
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string ColName = dt.Columns[j].ColumnName;
                            if (ColName == "colInvoiceValue" || ColName == "colTaxableVal" || ColName == "colIGSTAmnt" || ColName == "colCessAmnt")
                                dt.Rows[i][j] = Utility.DisplayIndianCurrency(Convert.ToString(dt.Rows[i][j]));
                        }
                    }

                    dt.AcceptChanges();
                    dgvGSTR16.DataSource = dt;
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
                if (dgvGSTR16.Rows.Count >= 1)
                {
                    if (dgvGSTR16Total.Rows.Count == 0)
                    {
                        #region IF TOTAL GRID HAVING NO RECORD
                        DataTable dtTotal = new DataTable();
                        foreach (DataGridViewColumn col in dgvGSTR16Total.Columns)
                        {
                            dtTotal.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;
                        }


                        #region ADD DATATABLE COLUMN
                        DataTable dt = new DataTable();
                        // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                        foreach (DataGridViewColumn col in dgvGSTR16.Columns)
                        {
                            dt.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;

                            if (col.Name == "colInvoiceValue")
                                dt.Columns["colInvoiceValue"].DataType = typeof(System.Double);
                        }
                        #endregion

                        #region ASSIGN GRIDVIEW ROWS IN DATATABLE
                        object[] rowValue = new object[dt.Columns.Count];

                        foreach (DataGridViewRow drn in dgvGSTR16.Rows)
                        {
                            if (drn.Index != dgvGSTR16.Rows.Count - 1)
                            {
                                rowValue[0] = "False";
                                for (int i = 1; i < drn.Cells.Count; i++)
                                {
                                    if (i != 6)
                                    {
                                        if (i == 10) rowValue[i] = Convert.ToString(drn.Cells[i].Value) == "" ? 0 : Convert.ToDecimal(Convert.ToString(drn.Cells[i].Value));
                                        else rowValue[i] = Convert.ToString(drn.Cells[i].Value);
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

                        DataRow dr = dtTotal.NewRow();

                        #region Invoice No
                        var result = (from row in dt.AsEnumerable()
                                      where row.Field<string>("colInvoiceNo") != ""
                                      group row by new { colInvNo = row.Field<string>("colInvoiceNo") } into grp
                                      select new
                                      {
                                          //colGSTIN = grp.Key.colGSTIN,
                                          colInvNo = grp.Key.colInvNo,
                                      }).ToList();

                        if (result != null && result.Count > 0)
                            dr["colTInvoiceNo"] = result.Count;
                        else
                            dr["colTInvoiceNo"] = 0;
                        #endregion

                        #region Invoice Value
                        var result2 = (from row in dt.AsEnumerable()
                                       where row.Field<double>("colInvoiceValue") != null && row.Field<string>("colInvoiceNo") != ""
                                       group row by new { colInvoiceValue = row.Field<double>("colInvoiceValue"), colInvNo = row.Field<string>("colInvoiceNo") } into grp
                                       select new
                                       {
                                           //colGSTIN = grp.Key.colGSTIN,
                                           colInvoiceValue = grp.Key.colInvoiceValue,
                                           colInvNo = grp.Key.colInvNo
                                       }).ToList();

                        DataTable InvoiceSum = new DataTable();
                        InvoiceSum = LINQResultToDataTable(result2);

                        double sum = InvoiceSum.AsEnumerable().Sum(row => Convert.ToDouble(row.Field<double>("colInvoiceValue")));

                        if (sum != null && sum > 0)
                            dr["colTInvoiceValue"] = sum;
                        else
                            dr["colTInvoiceValue"] = 0;
                        #endregion


                        dr["colTInvoiceNo"] = dgvGSTR16.Rows.Cast<DataGridViewRow>().Where(x => Convert.ToString(x.Cells["colInvoiceNo"].Value).Trim() != "").GroupBy(x => x.Cells["colInvoiceNo"].Value).Select(x => x.First()).Distinct().Count();

                        //dr["colTInvoiceValue"] = dgvGSTR16.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colInvoiceValue"].Value != null).Sum(x => x.Cells["colInvoiceValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colInvoiceValue"].Value)).ToString();

                        dr["colTInvoiceTaxableVal"] = dgvGSTR16.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colTaxableVal"].Value != null).Sum(x => x.Cells["colTaxableVal"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colTaxableVal"].Value)).ToString();

                        dr["colTIGSTAmnt"] = dgvGSTR16.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colIGSTAmnt"].Value != null).Sum(x => x.Cells["colIGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIGSTAmnt"].Value)).ToString();

                        dr["colTCessAmount"] = dgvGSTR16.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCessAmnt"].Value != null).Sum(x => x.Cells["colCessAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCessAmnt"].Value)).ToString();

                        dtTotal.Rows.Add(dr);

                        for (int i = 0; i < dtTotal.Rows.Count; i++)
                        {
                            for (int j = 0; j < dtTotal.Columns.Count; j++)
                            {
                                string ColName = dtTotal.Columns[j].ColumnName;
                                if (ColName == "colTInvoiceValue" || ColName == "colTInvoiceTaxableVal" || ColName == "colTIGSTAmnt" || ColName == "colTCessAmount")
                                    dtTotal.Rows[i][j] = Utility.DisplayIndianCurrency(Convert.ToString(dtTotal.Rows[i][j]));
                            }
                        }

                        dtTotal.AcceptChanges();

                        dgvGSTR16Total.DataSource = dtTotal;

                        #endregion
                    }
                    else if (dgvGSTR16Total.Rows.Count == 1)
                    {
                        #region IF TOTAL GRID HAVING ONLY ONE RECORDS
                        foreach (var item in colNo)
                        {
                            if (item == "colInvoiceNo" || item == "colInvoiceValue")
                            {
                                DataTable dt = new DataTable();
                                #region ADD DATATABLE COLUMN

                                // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                                foreach (DataGridViewColumn col in dgvGSTR16.Columns)
                                {
                                    dt.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                    if (col.Name == "colInvoiceValue")
                                        dt.Columns["colInvoiceValue"].DataType = typeof(System.Double);
                                }
                                #endregion

                                #region ASSIGN GRIDVIEW ROWS IN DATATABLE
                                object[] rowValue = new object[dt.Columns.Count];

                                foreach (DataGridViewRow drn in dgvGSTR16.Rows)
                                {
                                    if (drn.Index != dgvGSTR16.Rows.Count - 1)
                                    {
                                        rowValue[0] = "False";
                                        for (int i = 1; i < drn.Cells.Count; i++)
                                        {
                                            if (i != 6)
                                            {
                                                if (i == 10) rowValue[i]=(Convert.ToString(drn.Cells[i].Value) == "" ? 0 : Convert.ToDecimal(drn.Cells[i].Value));
                                                else rowValue[i] = (Convert.ToString(drn.Cells[i].Value));
                                            }
                                            else
                                            {
                                                if (drn.Cells[i].Value != null && Convert.ToString(drn.Cells[i].Value) != "")
                                                {
                                                    rowValue[i] = Convert.ToDouble(drn.Cells[i].Value);
                                                }
                                                else
                                                {
                                                    rowValue[i] = 0;
                                                }
                                            }
                                        }
                                        //string abc = "";
                                        //foreach (DataColumn dc in dt.Columns)
                                        //{
                                        //    abc += ", " + dc.DataType.ToString();
                                        //} 
                                        dt.Rows.Add(rowValue);
                                    }
                                }
                                dt.AcceptChanges();
                                #endregion

                                #region InvoiceNo Count
                                var result = (from row in dt.AsEnumerable()
                                              where row.Field<string>("colInvoiceNo") != ""
                                              group row by new { colInvNo = row.Field<string>("colInvoiceNo") } into grp
                                              select new
                                              {
                                                  //colGSTIN = grp.Key.colGSTIN,
                                                  colInvNo = grp.Key.colInvNo,
                                              }).ToList();

                                if (result != null && result.Count > 0)
                                    dgvGSTR16Total.Rows[0].Cells["colTInvoiceNo"].Value = result.Count;
                                else
                                    dgvGSTR16Total.Rows[0].Cells["colTInvoiceNo"].Value = 0;
                                #endregion

                                #region Invoice Value
                                var result2 = (from row in dt.AsEnumerable()
                                               where row.Field<double>("colInvoiceValue") != null && row.Field<string>("colInvoiceNo") != ""
                                               group row by new { colInvoiceValue = row.Field<double>("colInvoiceValue"), colInvNo = row.Field<string>("colInvoiceNo") } into grp
                                               select new
                                               {
                                                   //colGSTIN = grp.Key.colGSTIN,
                                                   colInvoiceValue = grp.Key.colInvoiceValue,
                                                   colInvNo = grp.Key.colInvNo
                                               }).ToList();

                                DataTable InvoiceSum = new DataTable();
                                InvoiceSum = LINQResultToDataTable(result2);

                                double sum = InvoiceSum.AsEnumerable().Sum(row => Convert.ToDouble(row.Field<double>("colInvoiceValue")));

                                if (sum != null && sum > 0)
                                    dgvGSTR16Total.Rows[0].Cells["colTInvoiceValue"].Value = Utility.DisplayIndianCurrency(Convert.ToString(sum));
                                else
                                    dgvGSTR16Total.Rows[0].Cells["colTInvoiceValue"].Value = 0;
                                #endregion
                            }

                            if (item == "colInvoiceNo")
                                dgvGSTR16Total.Rows[0].Cells["colTInvoiceNo"].Value = dgvGSTR16.Rows.Cast<DataGridViewRow>().Where(x => Convert.ToString(x.Cells["colInvoiceNo"].Value).Trim() != "").GroupBy(x => x.Cells["colInvoiceNo"].Value).Select(x => x.First()).Distinct().Count();

                            //else if (item == "colInvoiceValue")
                            //    dgvGSTR16Total.Rows[0].Cells["colTInvoiceValue"].Value = dgvGSTR16.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colInvoiceValue"].Value != null).Sum(x => x.Cells["colInvoiceValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colInvoiceValue"].Value)).ToString();

                            else if (item == "colTaxableVal")
                                dgvGSTR16Total.Rows[0].Cells["colTInvoiceTaxableVal"].Value = Utility.DisplayIndianCurrency(dgvGSTR16.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colTaxableVal"].Value != null).Sum(x => x.Cells["colTaxableVal"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colTaxableVal"].Value)).ToString());

                            else if (item == "colIGSTAmnt")
                                dgvGSTR16Total.Rows[0].Cells["colTIGSTAmnt"].Value = Utility.DisplayIndianCurrency(dgvGSTR16.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colIGSTAmnt"].Value != null).Sum(x => x.Cells["colIGSTAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIGSTAmnt"].Value)).ToString());


                            else if (item == "colCessAmnt")
                                dgvGSTR16Total.Rows[0].Cells["colTCessAmount"].Value = Utility.DisplayIndianCurrency(dgvGSTR16.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCessAmnt"].Value != null).Sum(x => x.Cells["colCessAmnt"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCessAmnt"].Value)).ToString());
                        }
                        #endregion
                    }

                    // set total grid height row
                    dgvGSTR16Total.Rows[0].Height = 30;
                    dgvGSTR16Total.Rows[0].Cells[0].Value = "TOTAL";
                }
                else
                {
                    // check if total grid having record
                    if (dgvGSTR16Total.Rows.Count >= 1)
                    {
                        // if there are no records in main grid thene assign blank datatable to total grid
                        DataTable dtTotal = new DataTable();
                        foreach (DataGridViewColumn col in dgvGSTR16Total.Columns)
                        {
                            dtTotal.Columns.Add(col.Name.ToString());
                            col.DataPropertyName = col.Name;
                        }
                        dgvGSTR16Total.DataSource = dtTotal;
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
            }
        }

        private void dgvGSTR16_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                pbGSTR1.Visible = true;

                if (e.KeyCode == Keys.Delete)
                {
                    #region DELETE SELECTED CELLS
                    try
                    {
                        if (dgvGSTR16.Rows.Count > 0)
                        {
                            foreach (DataGridViewCell oneCell in dgvGSTR16.SelectedCells)
                            {
                                if (oneCell.Selected && oneCell.ColumnIndex != 0 && dgvGSTR16.Columns[oneCell.ColumnIndex].Name != "colTax")
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

                    string[] colNo = { "colInvoiceNo", "colInvoiceValue", "colTaxableVal", "colIGSTAmnt", "colCessAmnt" };
                    GetTotal(colNo);
                }
                if (e.KeyCode == Keys.V)
                {
                    #region PAST FROM EXCELL SHEET

                    string s = Clipboard.GetText();
                    string[] lines = s.Split('\n');
                    int iRow = 0, iCol = 0;

                    #region PAST ON SELECTED CELLS ONLY
                    if (dgvGSTR16.RowCount > 0) // IF GRID IS NOT NULL AND PAST ONLY ON SELECTED CELLS
                    {
                        foreach (DataGridViewCell oneCell in dgvGSTR16.SelectedCells)
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
                            DisableControls(dgvGSTR16);

                            gRowNo = dgvGSTR16.Rows.Count - 1;
                            int no = tmp;

                            if (iRow > gRowNo - 1)
                            {
                                DataTable dtDGV = new DataTable();
                                //dtDGV = dgvGSTR16.DataSource as DataTable;

                                #region ADD DATATABLE COLUMN

                                // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                                foreach (DataGridViewColumn col in dgvGSTR16.Columns)
                                {
                                    dtDGV.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }
                                #endregion

                                #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                                // CREATE OBJECT ARRAY TO STORE ONE ROW DATA OF MAIN GRID
                                object[] rowValue = new object[dtDGV.Columns.Count];

                                foreach (DataGridViewRow dr in dgvGSTR16.Rows)
                                {
                                    if (dr.Index != dgvGSTR16.Rows.Count - 1) // DON'T ADD LAST ROW
                                    {
                                        // SET CHECK BOX VALUE
                                        rowValue[0] = "False";
                                        for (int i = 1; i < dr.Cells.Count; i++)
                                        {
                                            rowValue[i] = Convert.ToString(dr.Cells[i].Value);
                                        }

                                        //if (Convert.ToString(dr.Cells[dr.Cells.Count - 3].Value).ToLower() == "1" || Convert.ToString(dr.Cells[dr.Cells.Count - 3].Value).ToLower() == "yes" || Convert.ToString(dr.Cells[dr.Cells.Count - 3].Value).ToLower() == "true")
                                        //    rowValue[dr.Cells.Count - 3] = "True";
                                        //else
                                        //    rowValue[dr.Cells.Count - 3] = "False";

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
                                if (line.Length > 0)
                                {
                                    #region Row Paste
                                    string[] sCells = line.Split('\t');

                                    for (int i = 0; i < sCells.GetLength(0); ++i)
                                    {
                                        if (iCol + i < this.dgvGSTR16.ColumnCount && i < this.dgvGSTR16.ColumnCount - 1)
                                        {
                                            if (iCol == 0)
                                                oCell = dgvGSTR16[iCol + i + 2, iRow];
                                            else if (iCol == 1)
                                                oCell = dgvGSTR16[iCol + i + 1, iRow];
                                            else
                                                oCell = dgvGSTR16[iCol + i, iRow];

                                            sCells[i] = sCells[i].Trim().Replace(",", "");
                                            if (oCell.ColumnIndex != 0)
                                            {
                                                if (dgvGSTR16.Columns[oCell.ColumnIndex].Name != "colChk" && dgvGSTR16.Columns[oCell.ColumnIndex].Name != "colSequence")
                                                {
                                                    #region VALIDATION
                                                    if (sCells[i].ToString().Trim() == "") { dgvGSTR16.Rows[iRow].Cells[oCell.ColumnIndex].Value = ""; }
                                                    else
                                                    {
                                                        if (oCell.ColumnIndex >= 2 && oCell.ColumnIndex <= dgvGSTR16.ColumnCount)
                                                            dgvGSTR16.Rows[iRow].Cells[oCell.ColumnIndex].Value = sCells[i].Replace("₹", "").Trim();
                                                    }
                                                    #endregion
                                                }
                                            }
                                            else
                                            {
                                                if (iCol > i)
                                                {
                                                    for (int j = oCell.ColumnIndex; j < dgvGSTR16.Columns.Count; j++)
                                                    {
                                                        #region VALIDATION
                                                        if (sCells[i].ToString().Trim() == "") { dgvGSTR16.Rows[iRow].Cells[j].Value = ""; }
                                                        else
                                                        {
                                                            if (j >= 2 && j <= dgvGSTR16.ColumnCount)
                                                                dgvGSTR16.Rows[iRow].Cells[j].Value = sCells[i].Replace("₹", "").Trim();
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
                                                    for (int j = oCell.ColumnIndex; j < dgvGSTR16.Columns.Count; j++)
                                                    {
                                                        #region VALIDATION
                                                        if (sCells[i].ToString().Trim() == "") { dgvGSTR16.Rows[iRow].Cells[j].Value = ""; }
                                                        else
                                                        {
                                                            if (j >= 2 && j <= dgvGSTR16.ColumnCount)
                                                                dgvGSTR16.Rows[iRow].Cells[j].Value = sCells[i].Replace("₹", "").Trim();
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

                    // enable main grid
                    EnableControls(dgvGSTR16);
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
                EnableControls(dgvGSTR16);
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
                DisableControls(dgvGSTR16);

                #region Set datatable
                int cnt = 0, colNo = 0;
                DataTable dt = dtDGV;
                if (dt == null)
                {
                    dt = new DataTable();
                    foreach (DataGridViewColumn col in dgvGSTR16.Columns)
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
                            DataRow dtRow = dt.NewRow();
                            dt.Rows.Add(dtRow);

                            #region Row Paste
                            string[] sCells = line.Split('\t');

                            for (int i = 0; i < sCells.GetLength(0); ++i)
                            {
                                if (iCol + i < this.dgvGSTR16.ColumnCount && colNo < dgvGSTR16.ColumnCount - 1)
                                {
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
                                                if (colNo >= 2 && colNo <= dgvGSTR16.Columns.Count)
                                                {
                                                    if (chkCellValue(sCells[i].Trim(), dgvGSTR16.Columns[colNo].Name))
                                                    {
                                                        if (dgvGSTR16.Columns[colNo].Name == "colPOS")
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
                                            for (int j = colNo; j < dgvGSTR16.Columns.Count; j++)
                                            {
                                                #region VALIDATION
                                                if (sCells[i].ToString().Trim() == "") { dt.Rows[dt.Rows.Count - 1][j] = DBNull.Value; }
                                                else
                                                {
                                                    if (j >= 2 && j <= dgvGSTR16.Columns.Count)
                                                    {
                                                        if (dgvGSTR16.Columns[j].Name == "colPOS")
                                                            dt.Rows[dt.Rows.Count - 1][j] = Utility.strValidStateName(sCells[i]);
                                                        else
                                                            dt.Rows[dt.Rows.Count - 1][j] = sCells[i].Trim();
                                                    }
                                                    else
                                                        dt.Rows[dt.Rows.Count - 1][j] = "";
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
                                            for (int j = colNo; j < dgvGSTR16.Columns.Count; j++)
                                            {
                                                #region VALIDATION
                                                if (sCells[i].ToString().Trim() == "") { dt.Rows[dt.Rows.Count - 1][j] = DBNull.Value; }
                                                else
                                                {
                                                    if (j >= 2 && j <= dgvGSTR16.Columns.Count)
                                                    {
                                                        if (dgvGSTR16.Columns[j].Name == "colPOS")
                                                            dt.Rows[dt.Rows.Count - 1][j] = Utility.strValidStateName(sCells[i]);
                                                        else
                                                            dt.Rows[dt.Rows.Count - 1][j] = sCells[i].Trim();
                                                    }
                                                    else
                                                        dt.Rows[dt.Rows.Count - 1][j] = "";
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

                if (dt != null && dt.Rows.Count > 0)
                    dgvGSTR16.DataSource = dt;

                string[] colGroup = { "colInvoiceNo", "colInvoiceValue", "colTaxableVal", "colIGSTAmnt", "colCessAmnt" };
                GetTotal(colGroup);

                pbGSTR1.Visible = false;

                EnableControls(dgvGSTR16);

                #endregion
            }
            catch (Exception ex)
            {
                pbGSTR1.Visible = false;
                EnableControls(dgvGSTR16);
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
            bool flag;
            object[] value;
            try
            {
                int _cnt = 0;
                string _str = "";
                this.pbGSTR1.Visible = true;
                this.dgvGSTR16.CurrentCell = this.dgvGSTR16.Rows[0].Cells["colChk"];
                this.dgvGSTR16.AllowUserToAddRows = false;
                List<DataGridViewRow> list = (
                    from x in this.dgvGSTR16.Rows.OfType<DataGridViewRow>()
                    where !Utility.IsInvoiceNumber(Convert.ToString(x.Cells["colInvoiceNo"].Value))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR16.Rows[list[i].Cells["colInvoiceNo"].RowIndex].Cells["colInvoiceNo"].Style.BackColor = Color.LightPink;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Invoice no can not be more than 16 digit.\n");
                }
                list = (
                    from x in this.dgvGSTR16.Rows.OfType<DataGridViewRow>()
                    where Utility.IsInvoiceNumber(Convert.ToString(x.Cells["colInvoiceNo"].Value))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    this.dgvGSTR16.Rows[list[i].Cells["colInvoiceNo"].RowIndex].Cells["colInvoiceNo"].Style.BackColor = Color.LightGreen;
                }
                DataTable dataTable = new DataTable();
                list = null;
                list = (
                    from x in this.dgvGSTR16.Rows.OfType<DataGridViewRow>()
                    where Convert.ToString(x.Cells["colInvoiceNo"].Value) != ""
                    select x).ToList<DataGridViewRow>();
                List<DataGridViewRow> Listdt = list;
                DataTable Ldt = new DataTable();
                Ldt.Columns.Add("Fld_InvoiceNo");
                DataRow row1 = null;
                foreach (DataGridViewRow rowObj in Listdt)
                {
                    row1 = Ldt.NewRow();
                    DataRowCollection rows = Ldt.Rows;
                    value = new object[] { rowObj.Cells[4].Value };
                    rows.Add(value);
                }
                string Lists = "";
                for (i = 0; i < Ldt.Rows.Count; i++)
                {
                    Lists = (Ldt.Rows.Count == i + 1 ? string.Concat(Lists, Ldt.Rows[i][0].ToString()) : string.Concat(Lists, Ldt.Rows[i][0].ToString(), ","));
                }
                string[] selectedMonth = new string[] { "Select distinct Fld_InvoiceNo from SPQR1B2B where Fld_InvoiceNo in ('", Lists, "') and Fld_Month='", CommonHelper.SelectedMonth, "' AND Fld_FinancialYear = '", CommonHelper.ReturnYear, "' and Fld_FileStatus != 'Total'" };
                string Query = string.Concat(selectedMonth);
                Application.DoEvents();
                dataTable = this.objGSTR6.GetDataGSTR1(Query);
                if (dataTable != null)
                {
                    for (int j = 0; j < dataTable.Rows.Count; j++)
                    {
                        list = null;
                        list = (
                            from x in this.dgvGSTR16.Rows.OfType<DataGridViewRow>()
                            where Convert.ToString(x.Cells["colInvoiceNo"].Value) == dataTable.Rows[j]["Fld_InvoiceNo"].ToString()
                            select x).ToList<DataGridViewRow>();
                        if (list.Count > 0)
                        {
                            for (i = 0; i < list.Count; i++)
                            {
                                this.dgvGSTR16.Rows[list[i].Cells["colInvoiceNo"].RowIndex].Cells["colInvoiceNo"].Style.BackColor = Color.LightPink;
                            }
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Please enter proper invoice no.\n");
                        }
                        list = (
                            from x in this.dgvGSTR16.Rows.OfType<DataGridViewRow>()
                            where Convert.ToString(x.Cells["colInvoiceNo"].Value) != dataTable.Rows[j]["Fld_InvoiceNo"].ToString()
                            select x).ToList<DataGridViewRow>();
                        for (i = 0; i < list.Count; i++)
                        {
                            if (Convert.ToString(this.dgvGSTR16.Rows[i].Cells["colInvoiceNo"].Value) != "")
                            {
                                this.dgvGSTR16.Rows[list[i].Cells["colInvoiceNo"].RowIndex].Cells["colInvoiceNo"].Style.BackColor = Color.LightGreen;
                            }
                        }
                    }
                }
                if (!CommonHelper.IsQuarter)
                {
                    list = null;
                    list = (
                        from x in this.dgvGSTR16.Rows.OfType<DataGridViewRow>()
                        where !Utility.IsInvoiceDate(Convert.ToString(x.Cells["colInvoiceDate"].Value))
                        select x).ToList<DataGridViewRow>();
                    if (list.Count > 0)
                    {
                        for (i = 0; i < list.Count; i++)
                        {
                            this.dgvGSTR16.Rows[list[i].Cells["colInvoiceDate"].RowIndex].Cells["colInvoiceDate"].Style.BackColor = Color.LightPink;
                        }
                        _cnt++;
                        _str = string.Concat(_str, _cnt, ") Please enter proper invoice date.\n");
                    }
                    list = (
                        from x in this.dgvGSTR16.Rows.OfType<DataGridViewRow>()
                        where Utility.IsInvoiceDate(Convert.ToString(x.Cells["colInvoiceDate"].Value))
                        select x).ToList<DataGridViewRow>();
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR16.Rows[list[i].Cells["colInvoiceDate"].RowIndex].Cells["colInvoiceDate"].Style.BackColor = Color.LightGreen;
                    }
                }
                else
                {
                    list = null;
                    list = (
                        from x in this.dgvGSTR16.Rows.OfType<DataGridViewRow>()
                        where !Utility.IsQuarterlyFilingDate(Convert.ToString(x.Cells["colInvoiceDate"].Value))
                        select x).ToList<DataGridViewRow>();
                    if (list.Count > 0)
                    {
                        for (i = 0; i < list.Count; i++)
                        {
                            this.dgvGSTR16.Rows[list[i].Cells["colInvoiceDate"].RowIndex].Cells["colInvoiceDate"].Style.BackColor = Color.LightPink;
                        }
                        _cnt++;
                        _str = string.Concat(_str, _cnt, ") Please enter proper invoice date.\n");
                    }
                    list = (
                        from x in this.dgvGSTR16.Rows.OfType<DataGridViewRow>()
                        where Utility.IsQuarterlyFilingDate(Convert.ToString(x.Cells["colInvoiceDate"].Value))
                        select x).ToList<DataGridViewRow>();
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR16.Rows[list[i].Cells["colInvoiceDate"].RowIndex].Cells["colInvoiceDate"].Style.BackColor = Color.LightGreen;
                    }
                }
                var result2 = (
                    from row in ((DataTable)this.dgvGSTR16.DataSource).AsEnumerable()
                    group row by new { colInvoiceNo = row.Field<string>("colInvoiceNo"), colInvoiceDate = row.Field<string>("colInvoiceDate") } into grp
                    select new { colInvoiceNo = grp.Key.colInvoiceNo, colInvoiceDate = grp.Key.colInvoiceDate }).ToList();
                if ((result2 == null ? false : result2.Count > 0))
                {
                    foreach (var variable in result2)
                    {
                        list = (
                            from x in this.dgvGSTR16.Rows.OfType<DataGridViewRow>()
                            where (Convert.ToString(x.Cells["colInvoiceNo"].Value) != Convert.ToString(variable.colInvoiceNo) ? false : Convert.ToString(x.Cells["colInvoiceDate"].Value) != Convert.ToString(variable.colInvoiceDate))
                            select x into p
                            select p).ToList<DataGridViewRow>();
                        if ((list == null ? false : list.Count > 0))
                        {
                            for (i = 0; i < list.Count; i++)
                            {
                                this.dgvGSTR16.Rows[list[i].Cells["colInvoiceDate"].RowIndex].Cells["colInvoiceDate"].Style.BackColor = Color.LightPink;
                            }
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Same invoice no for different Invoice Date is not possible.\n");
                        }
                    }
                }
                list = null;
                list = (
                    from x in this.dgvGSTR16.Rows.OfType<DataGridViewRow>()
                    where (!Utility.IsDecimalOrNumber(Convert.ToString(x.Cells["colInvoiceValue"].Value)) ? true : Convert.ToDecimal(x.Cells["colInvoiceValue"].Value) <= new decimal(250000))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR16.Rows[list[i].Cells["colInvoiceValue"].RowIndex].Cells["colInvoiceValue"].Style.BackColor = Color.LightPink;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Please enter proper invoice value.\n");
                }
                list = (
                    from x in this.dgvGSTR16.Rows.OfType<DataGridViewRow>()
                    where (!Utility.IsDecimalOrNumber(Convert.ToString(x.Cells["colInvoiceValue"].Value)) ? false : Convert.ToDecimal(x.Cells["colInvoiceValue"].Value) > new decimal(250000))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    this.dgvGSTR16.Rows[list[i].Cells["colInvoiceValue"].RowIndex].Cells["colInvoiceValue"].Style.BackColor = Color.LightGreen;
                }
                var result3 = (
                    from row in ((DataTable)this.dgvGSTR16.DataSource).AsEnumerable()
                    group row by new { colInvoiceNo = row.Field<string>("colInvoiceNo"), colInvoiceValue = row.Field<string>("colInvoiceValue") } into grp
                    select new { colInvoiceNo = grp.Key.colInvoiceNo, colInvoiceValue = grp.Key.colInvoiceValue }).ToList();
                if ((result3 == null ? false : result3.Count > 0))
                {
                    foreach (var variable1 in result3)
                    {
                        list = (
                            from x in this.dgvGSTR16.Rows.OfType<DataGridViewRow>()
                            where (Convert.ToString(x.Cells["colInvoiceNo"].Value) != Convert.ToString(variable1.colInvoiceNo) ? false : Convert.ToString(x.Cells["colInvoiceValue"].Value) != Convert.ToString(variable1.colInvoiceValue))
                            select x into p
                            select p).ToList<DataGridViewRow>();
                        if ((list == null ? false : list.Count > 0))
                        {
                            for (i = 0; i < list.Count; i++)
                            {
                                this.dgvGSTR16.Rows[list[i].Cells["colInvoiceValue"].RowIndex].Cells["colInvoiceValue"].Style.BackColor = Color.LightPink;
                            }
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Same invoice no for different Invoice Value no is not possible.\n");
                        }
                    }
                }
                list = null;
                list = (
                    from x in this.dgvGSTR16.Rows.OfType<DataGridViewRow>()
                    where !Utility.IsRate(Convert.ToString(x.Cells["colRate"].Value))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR16.Rows[list[i].Cells["colRate"].RowIndex].Cells["colRate"].Style.BackColor = Color.LightPink;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Please enter proper Rate.\n");
                }
                list = (
                    from x in this.dgvGSTR16.Rows.OfType<DataGridViewRow>()
                    where Utility.IsRate(Convert.ToString(x.Cells["colRate"].Value))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    this.dgvGSTR16.Rows[list[i].Cells[10].RowIndex].Cells["colRate"].Style.BackColor = Color.LightGreen;
                }
                var result4 = (
                    from row in ((DataTable)this.dgvGSTR16.DataSource).AsEnumerable()
                    group row by new { colInvoiceNo = row.Field<string>("colInvoiceNo"), colInvoiceValue = row.Field<string>("colInvoiceValue"), colRate = row.Field<string>("colRate") } into grp
                    select new { colRate = grp.Key.colRate, colInvoiceNo = grp.Key.colInvoiceNo, colInvoiceValue = grp.Key.colInvoiceValue }).ToList();
                if ((result4 == null ? false : result4.Count > 0))
                {
                    foreach (var variable2 in result4)
                    {
                        list = (
                            from x in this.dgvGSTR16.Rows.OfType<DataGridViewRow>()
                            where (!(Convert.ToString(x.Cells["colRate"].Value) == Convert.ToString(variable2.colRate)) || !(Convert.ToString(x.Cells["colInvoiceNo"].Value) == Convert.ToString(variable2.colInvoiceNo)) ? false : Convert.ToString(x.Cells["colInvoiceValue"].Value) == Convert.ToString(variable2.colInvoiceValue))
                            select x into p
                            select p).ToList<DataGridViewRow>();
                        if ((list == null ? true : list.Count <= 1))
                        {
                            for (i = 0; i < list.Count; i++)
                            {
                                if (this.dgvGSTR16.Rows[list[i].Cells["colRate"].RowIndex].Cells["colRate"].Style.BackColor != Color.LightPink)
                                {
                                    this.dgvGSTR16.Rows[list[i].Cells["colRate"].RowIndex].Cells["colRate"].Style.BackColor = Color.LightGreen;
                                }
                            }
                        }
                        else
                        {
                            for (i = 0; i < list.Count; i++)
                            {
                                this.dgvGSTR16.Rows[list[i].Cells["colRate"].RowIndex].Cells["colRate"].Style.BackColor = Color.LightPink;
                            }
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Same invoice no for different rate is not possible.\n");
                        }
                    }
                }
                list = null;
                list = (
                    from x in this.dgvGSTR16.Rows.OfType<DataGridViewRow>()
                    where !Utility.IsDecimalOrNumber(Convert.ToString(x.Cells["colTaxableVal"].Value))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR16.Rows[list[i].Cells["colTaxableVal"].RowIndex].Cells["colTaxableVal"].Style.BackColor = Color.LightPink;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Please enter proper taxable value.\n");
                }
                list = (
                    from x in this.dgvGSTR16.Rows.OfType<DataGridViewRow>()
                    where Utility.IsDecimalOrNumber(Convert.ToString(x.Cells["colTaxableVal"].Value))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    if (!Utility.IsDecimalOrNumber(this.dgvGSTR16.Rows[i].Cells["colInvoiceValue"].Value.ToString()))
                    {
                        this.dgvGSTR16.Rows[list[i].Cells["colTaxableVal"].RowIndex].Cells["colTaxableVal"].Style.BackColor = Color.LightGreen;
                    }
                    else if (!(Convert.ToDecimal(this.dgvGSTR16.Rows[list[i].Cells["colTaxableVal"].RowIndex].Cells["colTaxableVal"].Value) > Convert.ToDecimal(this.dgvGSTR16.Rows[list[i].Cells["colInvoiceValue"].RowIndex].Cells["colInvoiceValue"].Value)))
                    {
                        this.dgvGSTR16.Rows[list[i].Cells["colTaxableVal"].RowIndex].Cells["colTaxableVal"].Style.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        this.dgvGSTR16.Rows[list[i].Cells["colTaxableVal"].RowIndex].Cells["colTaxableVal"].Style.BackColor = Color.LightPink;
                        _cnt++;
                        _str = string.Concat(_str, _cnt, ") Taxable values can not be more than Invoice value.\n");
                    }
                }
                string stateName = CommonHelper.StateName;
                list = null;
                list = (
                    from x in this.dgvGSTR16.Rows.OfType<DataGridViewRow>()
                    where (!Utility.IsValidStateName(Convert.ToString(x.Cells["colPOS"].Value)) ? true : Convert.ToString(x.Cells["colPOS"].Value) == stateName)
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR16.Rows[list[i].Cells["colPOS"].RowIndex].Cells["colPOS"].Style.BackColor = Color.LightPink;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Please enter proper place of supply.\n");
                }
                list = (
                    from x in this.dgvGSTR16.Rows.OfType<DataGridViewRow>()
                    where (!Utility.IsValidStateName(Convert.ToString(x.Cells["colPOS"].Value)) ? false : Convert.ToString(x.Cells["colPOS"].Value) != stateName)
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    this.dgvGSTR16.Rows[list[i].Cells["colPOS"].RowIndex].Cells["colPOS"].Style.BackColor = Color.LightGreen;
                }
                var result9 = (
                    from row in ((DataTable)this.dgvGSTR16.DataSource).AsEnumerable()
                    group row by new { colInvoiceNo = row.Field<string>("colInvoiceNo"), colPOS = row.Field<string>("colPOS") } into grp
                    select new { colInvoiceNo = grp.Key.colInvoiceNo, colPOS = grp.Key.colPOS }).ToList();
                if ((result9 == null ? false : result9.Count > 0))
                {
                    foreach (var variable3 in result9)
                    {
                        list = (
                            from x in this.dgvGSTR16.Rows.OfType<DataGridViewRow>()
                            where (Convert.ToString(x.Cells["colInvoiceNo"].Value) != Convert.ToString(variable3.colInvoiceNo) ? false : Convert.ToString(x.Cells["colPOS"].Value) != Convert.ToString(variable3.colPOS))
                            select x into p
                            select p).ToList<DataGridViewRow>();
                        if ((list == null ? false : list.Count > 0))
                        {
                            for (i = 0; i < list.Count; i++)
                            {
                                this.dgvGSTR16.Rows[list[i].Cells["colPOS"].RowIndex].Cells["colPOS"].Style.BackColor = Color.LightPink;
                            }
                            _cnt++;
                            _str = string.Concat(_str, _cnt, ") Same invoice no for different POS is not possible.\n");
                        }
                    }
                }
                list = null;
                list = (
                    from x in this.dgvGSTR16.Rows.OfType<DataGridViewRow>()
                    where !Utility.IsICSC(Convert.ToString(x.Cells["colIGSTAmnt"].Value))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR16.Rows[list[i].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.LightPink;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Please enter proper IGST Amount.\n");
                }
                list = (
                    from x in this.dgvGSTR16.Rows.OfType<DataGridViewRow>()
                    where Utility.IsICSC(Convert.ToString(x.Cells["colIGSTAmnt"].Value))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    this.dgvGSTR16.Rows[list[i].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.LightGreen;
                }
                list = null;
                list = (
                    from x in this.dgvGSTR16.Rows.OfType<DataGridViewRow>()
                    where !Utility.IsBlankICSC(Convert.ToString(x.Cells["colCessAmnt"].Value))
                    select x).ToList<DataGridViewRow>();
                if (list.Count > 0)
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR16.Rows[list[i].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.LightPink;
                    }
                    _cnt++;
                    _str = string.Concat(_str, _cnt, ") Please enter proper Cess Amount.\n");
                }
                list = (
                    from x in this.dgvGSTR16.Rows.OfType<DataGridViewRow>()
                    where Utility.IsBlankICSC(Convert.ToString(x.Cells["colCessAmnt"].Value))
                    select x).ToList<DataGridViewRow>();
                for (i = 0; i < list.Count; i++)
                {
                    this.dgvGSTR16.Rows[list[i].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.LightGreen;
                }
                list = null;
                list = (
                    from x in this.dgvGSTR16.Rows.OfType<DataGridViewRow>()
                    where Convert.ToString(x.Cells["colEComm"].Value).Trim() != ""
                    select x).ToList<DataGridViewRow>();
                if ((list == null ? false : list.Count > 0))
                {
                    List<DataGridViewRow> list1 = null;
                    list1 = (
                        from x in list.OfType<DataGridViewRow>()
                        where !Utility.IsBlankGSTN(Convert.ToString(x.Cells["colEComm"].Value).Trim())
                        select x).ToList<DataGridViewRow>();
                    if ((list1 == null ? true : list1.Count <= 0))
                    {
                        for (i = 0; i < list1.Count; i++)
                        {
                            this.dgvGSTR16.Rows[list[i].Cells["colEComm"].RowIndex].Cells["colEComm"].Style.BackColor = Color.LightGreen;
                        }
                    }
                    else
                    {
                        for (i = 0; i < list1.Count; i++)
                        {
                            this.dgvGSTR16.Rows[list[i].Cells["colEComm"].RowIndex].Cells["colEComm"].Style.BackColor = Color.LightPink;
                        }
                        _cnt++;
                        _str = string.Concat(_str, _cnt, ") Please enter proper GSTIN of E-Commerce.\n");
                    }
                }
                list = null;
                list = (
                    from x in this.dgvGSTR16.Rows.OfType<DataGridViewRow>()
                    where Utility.IsBlankGSTN(Convert.ToString(x.Cells["colEComm"].Value).Trim())
                    select x).ToList<DataGridViewRow>();
                if ((list == null ? false : list.Count > 0))
                {
                    for (i = 0; i < list.Count; i++)
                    {
                        this.dgvGSTR16.Rows[list[i].Cells["colEComm"].RowIndex].Cells["colEComm"].Style.BackColor = Color.LightGreen;
                    }
                }
                DataTable dsOldinvoice = new DataTable();
                selectedMonth = new string[] { "Select DISTINCT Fld_InvoiceNo from SPQR1B2CL where Fld_Month <> '", CommonHelper.SelectedMonth, "' AND Fld_FinancialYear = '", CommonHelper.ReturnYear, "'" };
                Query = string.Concat(selectedMonth);
                dsOldinvoice = this.objGSTR6.GetDataGSTR1(Query);
                if ((dsOldinvoice == null ? false : dsOldinvoice.Rows.Count > 0))
                {
                    for (int k = 0; k < dsOldinvoice.Rows.Count; k++)
                    {
                        for (i = 0; i < this.dgvGSTR16.RowCount; i++)
                        {
                            if (dsOldinvoice.Rows[k]["Fld_InvoiceNo"].ToString().Trim() == this.dgvGSTR16.Rows[i].Cells["colInvoiceNo"].Value.ToString().Trim())
                            {
                                this.dgvGSTR16.Rows[i].Cells["colInvoiceNo"].Style.BackColor = Color.LightPink;
                            }
                        }
                    }
                }
                this.dgvGSTR16.AllowUserToAddRows = true;
                this.pbGSTR1.Visible = false;
                if (!(_str != ""))
                {
                    if (this.objGSTR6.InsertValidationFlg("GSTR1", "B2CL", "true", CommonHelper.SelectedMonth) != 1)
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
                    if (this.objGSTR6.InsertValidationFlg("GSTR1", "B2CL", "false", CommonHelper.SelectedMonth) != 1)
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
                this.dgvGSTR16.AllowUserToAddRows = true;
                MessageBox.Show(string.Concat("Error : ", ex.Message), "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                value = new object[] { ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine };
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", value);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
                flag = false;
            }
            return flag;
        }

        public bool IsValidateData_Old()
        {
            try
            {
                int _cnt = 0;
                string _str = "";

                pbGSTR1.Visible = true;
                dgvGSTR16.CurrentCell = dgvGSTR16.Rows[0].Cells["colChk"];
                dgvGSTR16.AllowUserToAddRows = false;

                #region Invoice No
                List<DataGridViewRow> list = dgvGSTR16.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsInvoiceNumber(Convert.ToString(x.Cells["colInvoiceNo"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR16.Rows[list[i].Cells["colInvoiceNo"].RowIndex].Cells["colInvoiceNo"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Invoice no can not be more than 16 digit.\n";
                }
                list = dgvGSTR16.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsInvoiceNumber(Convert.ToString(x.Cells["colInvoiceNo"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR16.Rows[list[i].Cells["colInvoiceNo"].RowIndex].Cells["colInvoiceNo"].Style.BackColor = Color.White;
                }
                #endregion

                #region Invoice No check in B2B Invoice No
                DataTable dt = new DataTable();
                list = null;
                list = dgvGSTR16.Rows
                           .OfType<DataGridViewRow>()
                           .Where(x => Convert.ToString(x.Cells["colInvoiceNo"].Value) != "")
                           .Select(x => x)
                           .ToList();
                var Listdt = list;

                DataTable Ldt = new DataTable();
                Ldt.Columns.Add("Fld_InvoiceNo");
                DataRow row1 = null;
                foreach (var rowObj in Listdt)
                {
                    row1 = Ldt.NewRow();
                    Ldt.Rows.Add(rowObj.Cells[4].Value);
                }
                string Lists = "";
                for (int i = 0; i < Ldt.Rows.Count; i++)
                {
                    if (Ldt.Rows.Count != i + 1)
                        Lists = Lists + "" + Ldt.Rows[i][0].ToString() + ",";
                    else
                        Lists = Lists + "" + Ldt.Rows[i][0].ToString() + "";
                }
                string Query = "Select distinct Fld_InvoiceNo from SPQR1B2B where Fld_InvoiceNo in ('" + Lists + "') and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
                Application.DoEvents();
                // GET DATA FROM DATABASE
                dt = objGSTR6.GetDataGSTR1(Query);

                if (dt != null)
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        list = null;
                        list = dgvGSTR16.Rows
                               .OfType<DataGridViewRow>()
                               .Where(x => Convert.ToString(x.Cells["colInvoiceNo"].Value) == dt.Rows[j]["Fld_InvoiceNo"].ToString())
                               .Select(x => x)
                               .ToList();
                        if (list.Count > 0)
                        {
                            for (int i = 0; i < list.Count; i++)
                            {
                                dgvGSTR16.Rows[list[i].Cells["colInvoiceNo"].RowIndex].Cells["colInvoiceNo"].Style.BackColor = Color.Red;
                            }
                            _cnt += 1;
                            _str += _cnt + ") Please enter proper invoice no.\n";
                        }
                        list = dgvGSTR16.Rows
                               .OfType<DataGridViewRow>()
                               .Where(x => Convert.ToString(x.Cells["colInvoiceNo"].Value) != dt.Rows[j]["Fld_InvoiceNo"].ToString())
                               .Select(x => x)
                               .ToList();
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (Convert.ToString(dgvGSTR16.Rows[i].Cells["colInvoiceNo"].Value) != "")
                                dgvGSTR16.Rows[list[i].Cells["colInvoiceNo"].RowIndex].Cells["colInvoiceNo"].Style.BackColor = Color.White;
                        }
                    }
                }
                #endregion

                if (CommonHelper.IsQuarter)
                {
                    #region Invoice Date
                    list = null;
                    list = dgvGSTR16.Rows
                           .OfType<DataGridViewRow>()
                           .Where(x => true != Utility.IsQuarterlyFilingDate(Convert.ToString(x.Cells["colInvoiceDate"].Value)))
                           .Select(x => x)
                           .ToList();
                    if (list.Count > 0)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            dgvGSTR16.Rows[list[i].Cells["colInvoiceDate"].RowIndex].Cells["colInvoiceDate"].Style.BackColor = Color.Red;
                        }
                        _cnt += 1;
                        _str += _cnt + ") Please enter proper invoice date.\n";
                    }
                    list = dgvGSTR16.Rows
                           .OfType<DataGridViewRow>()
                           .Where(x => true == Utility.IsQuarterlyFilingDate(Convert.ToString(x.Cells["colInvoiceDate"].Value)))
                           .Select(x => x)
                           .ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR16.Rows[list[i].Cells["colInvoiceDate"].RowIndex].Cells["colInvoiceDate"].Style.BackColor = Color.White;
                    }
                    #endregion
                }
                else
                {
                    #region Invoice Date
                    list = null;
                    list = dgvGSTR16.Rows
                           .OfType<DataGridViewRow>()
                           .Where(x => true != Utility.IsInvoiceDate(Convert.ToString(x.Cells["colInvoiceDate"].Value)))
                           .Select(x => x)
                           .ToList();
                    if (list.Count > 0)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            dgvGSTR16.Rows[list[i].Cells["colInvoiceDate"].RowIndex].Cells["colInvoiceDate"].Style.BackColor = Color.Red;
                        }
                        _cnt += 1;
                        _str += _cnt + ") Please enter proper invoice date.\n";
                    }
                    list = dgvGSTR16.Rows
                           .OfType<DataGridViewRow>()
                           .Where(x => true == Utility.IsInvoiceDate(Convert.ToString(x.Cells["colInvoiceDate"].Value)))
                           .Select(x => x)
                           .ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR16.Rows[list[i].Cells["colInvoiceDate"].RowIndex].Cells["colInvoiceDate"].Style.BackColor = Color.White;
                    }
                    #endregion
                }

                #region same Invoice number for Same Invoice Date is required
                DataTable dt1 = (DataTable)dgvGSTR16.DataSource;

                var result2 = (from row in dt1.AsEnumerable()
                               group row by new { colInvoiceNo = row.Field<string>("colInvoiceNo"), colInvoiceDate = row.Field<string>("colInvoiceDate") } into grp
                               select new
                               {
                                   colInvoiceNo = grp.Key.colInvoiceNo,
                                   colInvoiceDate = grp.Key.colInvoiceDate,
                               }).ToList();

                if (result2 != null && result2.Count > 0)
                {
                    foreach (var item in result2)
                    {
                        #region Same Invoice no Same GSTIN
                        list = dgvGSTR16.Rows
                                .OfType<DataGridViewRow>()
                                .Where(x => Convert.ToString(x.Cells["colInvoiceNo"].Value) == Convert.ToString(item.colInvoiceNo) && Convert.ToString(x.Cells["colInvoiceDate"].Value) != Convert.ToString(item.colInvoiceDate))
                                .Select(p => p)
                                .ToList();

                        if (list != null && list.Count > 0)
                        {
                            for (int i = 0; i < list.Count; i++)
                            {
                                dgvGSTR16.Rows[list[i].Cells["colInvoiceDate"].RowIndex].Cells["colInvoiceDate"].Style.BackColor = Color.Red;
                            }
                            _cnt += 1;
                            _str += _cnt + ") Same invoice no for different Invoice Date is not possible.\n";
                        }
                        #endregion
                    }
                }
                #endregion

                #region Invoice Value
                list = null;
                list = dgvGSTR16.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsDecimalOrNumber(Convert.ToString(x.Cells["colInvoiceValue"].Value)) || Convert.ToDecimal(x.Cells["colInvoiceValue"].Value) <= 250000)
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR16.Rows[list[i].Cells["colInvoiceValue"].RowIndex].Cells["colInvoiceValue"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper invoice value.\n";
                }
                list = dgvGSTR16.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsDecimalOrNumber(Convert.ToString(x.Cells["colInvoiceValue"].Value)) && Convert.ToDecimal(x.Cells["colInvoiceValue"].Value) > 250000)
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR16.Rows[list[i].Cells["colInvoiceValue"].RowIndex].Cells["colInvoiceValue"].Style.BackColor = Color.White;
                }
                #endregion

                #region Same Invoice no for diffrent Invoice Value
                DataTable dt2 = (DataTable)dgvGSTR16.DataSource;

                var result3 = (from row in dt2.AsEnumerable()
                               group row by new { colInvoiceNo = row.Field<string>("colInvoiceNo"), colInvoiceValue = row.Field<string>("colInvoiceValue") } into grp
                               select new
                               {
                                   colInvoiceNo = grp.Key.colInvoiceNo,
                                   colInvoiceValue = grp.Key.colInvoiceValue,
                               }).ToList();

                if (result3 != null && result3.Count > 0)
                {
                    foreach (var item in result3)
                    {
                        #region Same Invoice no Same Invoice Value
                        list = dgvGSTR16.Rows
                                .OfType<DataGridViewRow>()
                                .Where(x => Convert.ToString(x.Cells["colInvoiceNo"].Value) == Convert.ToString(item.colInvoiceNo) && Convert.ToString(x.Cells["colInvoiceValue"].Value) != Convert.ToString(item.colInvoiceValue))
                                .Select(p => p)
                                .ToList();

                        if (list != null && list.Count > 0)
                        {
                            for (int i = 0; i < list.Count; i++)
                            {
                                dgvGSTR16.Rows[list[i].Cells["colInvoiceValue"].RowIndex].Cells["colInvoiceValue"].Style.BackColor = Color.Red;
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
                list = dgvGSTR16.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsRate(Convert.ToString(x.Cells["colRate"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR16.Rows[list[i].Cells["colRate"].RowIndex].Cells["colRate"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper Rate.\n";
                }
                list = dgvGSTR16.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsRate(Convert.ToString(x.Cells["colRate"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR16.Rows[list[i].Cells[10].RowIndex].Cells["colRate"].Style.BackColor = Color.White;
                }
                #endregion

                #region Same Invoice no for diffrent Rate
                DataTable dt3 = (DataTable)dgvGSTR16.DataSource;

                var result4 = (from row in dt3.AsEnumerable()
                               group row by new { colInvoiceNo = row.Field<string>("colInvoiceNo"), colInvoiceValue = row.Field<string>("colInvoiceValue"), colRate = row.Field<string>("colRate") } into grp
                               select new
                               {
                                   colRate = grp.Key.colRate,
                                   colInvoiceNo = grp.Key.colInvoiceNo,
                                   colInvoiceValue = grp.Key.colInvoiceValue,
                               }).ToList();

                if (result4 != null && result4.Count > 0)
                {
                    foreach (var item in result4)
                    {
                        #region Same Invoice no Same Rate
                        list = dgvGSTR16.Rows
                                .OfType<DataGridViewRow>()
                                .Where(x => Convert.ToString(x.Cells["colRate"].Value) == Convert.ToString(item.colRate) && Convert.ToString(x.Cells["colInvoiceNo"].Value) == Convert.ToString(item.colInvoiceNo) && Convert.ToString(x.Cells["colInvoiceValue"].Value) == Convert.ToString(item.colInvoiceValue))
                                .Select(p => p)
                                .ToList();

                        if (list != null && list.Count > 1)
                        {
                            for (int i = 0; i < list.Count; i++)
                            {
                                dgvGSTR16.Rows[list[i].Cells["colRate"].RowIndex].Cells["colRate"].Style.BackColor = Color.Red;
                            }
                            _cnt += 1;
                            _str += _cnt + ") Same invoice no for different rate is not possible.\n";
                        }
                        else
                        {
                            for (int i = 0; i < list.Count; i++)
                            {
                                if (dgvGSTR16.Rows[list[i].Cells["colRate"].RowIndex].Cells["colRate"].Style.BackColor != Color.Red)
                                    dgvGSTR16.Rows[list[i].Cells["colRate"].RowIndex].Cells["colRate"].Style.BackColor = Color.White;
                            }
                        }
                        #endregion
                    }
                }
                #endregion

                #region Taxable Value
                list = null;
                list = dgvGSTR16.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsDecimalOrNumber(Convert.ToString(x.Cells["colTaxableVal"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR16.Rows[list[i].Cells["colTaxableVal"].RowIndex].Cells["colTaxableVal"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper taxable value.\n";
                }
                list = dgvGSTR16.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsDecimalOrNumber(Convert.ToString(x.Cells["colTaxableVal"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    //dgvGSTR16.Rows[list[i].Cells["colTaxableVal"].RowIndex].Cells["colTaxableVal"].Style.BackColor = Color.White;
                    if (Utility.IsDecimalOrNumber(dgvGSTR16.Rows[i].Cells["colInvoiceValue"].Value.ToString()))
                    {
                        if (Convert.ToDecimal(dgvGSTR16.Rows[list[i].Cells["colTaxableVal"].RowIndex].Cells["colTaxableVal"].Value) > Convert.ToDecimal(dgvGSTR16.Rows[list[i].Cells["colInvoiceValue"].RowIndex].Cells["colInvoiceValue"].Value))
                        {
                            dgvGSTR16.Rows[list[i].Cells["colTaxableVal"].RowIndex].Cells["colTaxableVal"].Style.BackColor = Color.Red;
                            _cnt += 1;
                            _str += _cnt + ") Taxable values can not be more than Invoice value.\n";
                        }
                        else
                        {
                            dgvGSTR16.Rows[list[i].Cells["colTaxableVal"].RowIndex].Cells["colTaxableVal"].Style.BackColor = Color.White;
                        }
                    }
                    else
                    {
                        dgvGSTR16.Rows[list[i].Cells["colTaxableVal"].RowIndex].Cells["colTaxableVal"].Style.BackColor = Color.White;
                    }
                }
                #endregion

                string gstin = CommonHelper.StateName;
                string result = gstin;

                #region POS
                list = null;
                list = dgvGSTR16.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsValidStateName(Convert.ToString(x.Cells["colPOS"].Value)) || Convert.ToString(x.Cells["colPOS"].Value) == result)
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR16.Rows[list[i].Cells["colPOS"].RowIndex].Cells["colPOS"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper place of supply.\n";
                }
                list = dgvGSTR16.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsValidStateName(Convert.ToString(x.Cells["colPOS"].Value)) && Convert.ToString(x.Cells["colPOS"].Value) != result)
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR16.Rows[list[i].Cells["colPOS"].RowIndex].Cells["colPOS"].Style.BackColor = Color.White;
                }
                #endregion

                #region same Invoice number for Same POS is required

                DataTable dt9 = (DataTable)dgvGSTR16.DataSource;
                var result9 = (from row in dt9.AsEnumerable()
                               group row by new { colInvoiceNo = row.Field<string>("colInvoiceNo"), colPOS = row.Field<string>("colPOS") } into grp
                               select new
                               {
                                   colInvoiceNo = grp.Key.colInvoiceNo,
                                   colPOS = grp.Key.colPOS,
                               }).ToList();

                if (result9 != null && result9.Count > 0)
                {
                    foreach (var item in result9)
                    {
                        #region Same Invoice no Same pos
                        list = dgvGSTR16.Rows
                                .OfType<DataGridViewRow>()
                                .Where(x => Convert.ToString(x.Cells["colInvoiceNo"].Value) == Convert.ToString(item.colInvoiceNo) && Convert.ToString(x.Cells["colPOS"].Value) != Convert.ToString(item.colPOS))
                                .Select(p => p)
                                .ToList();

                        if (list != null && list.Count > 0)
                        {
                            for (int i = 0; i < list.Count; i++)
                            {
                                dgvGSTR16.Rows[list[i].Cells["colPOS"].RowIndex].Cells["colPOS"].Style.BackColor = Color.Red;
                            }
                            _cnt += 1;
                            _str += _cnt + ") Same invoice no for different POS is not possible.\n";
                        }
                        #endregion
                    }
                }
                #endregion

                #region IGST Amount
                list = null;
                list = dgvGSTR16.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsICSC(Convert.ToString(x.Cells["colIGSTAmnt"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR16.Rows[list[i].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper IGST Amount.\n";
                }
                list = dgvGSTR16.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsICSC(Convert.ToString(x.Cells["colIGSTAmnt"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR16.Rows[list[i].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.White;
                }
                #endregion

                #region Actual value and Cumputer value different validation
                /*
                list = null;
                list = dgvGSTR16.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Convert.ToString(x.Cells["colIGSTAmnt"].Value) != "" && Convert.ToString(x.Cells["colRate"].Value) != "" && Convert.ToString(x.Cells["colTaxableVal"].Value) != "")
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        decimal IGST = Convert.ToDecimal(dgvGSTR16.Rows[list[i].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value);
                        decimal Rate = Convert.ToDecimal(dgvGSTR16.Rows[list[i].Cells["colRate"].RowIndex].Cells["colRate"].Value);
                        decimal Tax = Convert.ToDecimal(dgvGSTR16.Rows[list[i].Cells["colTaxableVal"].RowIndex].Cells["colTaxableVal"].Value);

                        decimal ComValue = Tax * Rate / 100;
                        ComValue = Math.Round(ComValue, 2, MidpointRounding.AwayFromZero);
                        decimal ResultIGST = ComValue - IGST;

                        //if (ResultIGST >= -1 && ResultIGST < 1)
                        if (Convert.ToDecimal(dgvGSTR16.Rows[list[i].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Value) == ComValue)
                        {
                            if (dgvGSTR16.Rows[list[i].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor != Color.Red)
                                dgvGSTR16.Rows[list[i].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.White;
                        }
                        else
                        {
                            dgvGSTR16.Rows[list[i].Cells["colIGSTAmnt"].RowIndex].Cells["colIGSTAmnt"].Style.BackColor = Color.Red;
                            _cnt += 1;
                            _str += _cnt + ") Please enter proper IGST Amount it can be no different value.\n";
                        }
                    }
                }
                */
                #endregion

                #region Cess Amount
                list = null;
                list = dgvGSTR16.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true != Utility.IsBlankICSC(Convert.ToString(x.Cells["colCessAmnt"].Value)))
                       .Select(x => x)
                       .ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR16.Rows[list[i].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.Red;
                    }
                    _cnt += 1;
                    _str += _cnt + ") Please enter proper Cess Amount.\n";
                }
                list = dgvGSTR16.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => true == Utility.IsBlankICSC(Convert.ToString(x.Cells["colCessAmnt"].Value)))
                       .Select(x => x)
                       .ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    dgvGSTR16.Rows[list[i].Cells["colCessAmnt"].RowIndex].Cells["colCessAmnt"].Style.BackColor = Color.White;
                }
                #endregion

                #region E-Com GSTIN
                list = null;
                list = dgvGSTR16.Rows
                       .OfType<DataGridViewRow>()
                       .Where(x => Convert.ToString(x.Cells["colEComm"].Value).Trim() != "")
                       .Select(x => x)
                       .ToList();
                if (list != null && list.Count > 0)
                {
                    List<DataGridViewRow> list1 = null;
                    list1 = list
                           .OfType<DataGridViewRow>()
                           .Where(x => true != Utility.IsBlankGSTN(Convert.ToString(x.Cells["colEComm"].Value).Trim()))
                           .Select(x => x)
                           .ToList();
                    if (list1 != null && list1.Count > 0)
                    {
                        for (int i = 0; i < list1.Count; i++)
                        {
                            dgvGSTR16.Rows[list[i].Cells["colEComm"].RowIndex].Cells["colEComm"].Style.BackColor = Color.Red;
                        }
                        _cnt += 1;
                        _str += _cnt + ") Please enter proper GSTIN of E-Commerce.\n";
                    }
                    else
                    {
                        for (int i = 0; i < list1.Count; i++)
                        {
                            dgvGSTR16.Rows[list[i].Cells["colEComm"].RowIndex].Cells["colEComm"].Style.BackColor = Color.White;
                        }
                    }
                }
                list = null;
                list = dgvGSTR16.Rows
                   .OfType<DataGridViewRow>()
                   .Where(x => true == Utility.IsBlankGSTN(Convert.ToString(x.Cells["colEComm"].Value).Trim()))
                   .Select(x => x)
                   .ToList();
                if (list != null && list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvGSTR16.Rows[list[i].Cells["colEComm"].RowIndex].Cells["colEComm"].Style.BackColor = Color.White;
                    }
                }
                #endregion

                DataTable dsOldinvoice = new DataTable();
                Query = "Select DISTINCT Fld_InvoiceNo from SPQR1B2CL where Fld_Month <> '" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                //Application.DoEvents();

                // GET DATA FROM DATABASE
                dsOldinvoice = objGSTR6.GetDataGSTR1(Query);
                if (dsOldinvoice != null && dsOldinvoice.Rows.Count > 0)
                {
                    for (int k = 0; k < dsOldinvoice.Rows.Count; k++)
                    {
                        for (int i = 0; i < dgvGSTR16.RowCount; i++)
                        {
                            if (dsOldinvoice.Rows[k]["Fld_InvoiceNo"].ToString().Trim() == dgvGSTR16.Rows[i].Cells["colInvoiceNo"].Value.ToString().Trim())
                            {
                                dgvGSTR16.Rows[i].Cells["colInvoiceNo"].Style.BackColor = Color.Red;
                            }
                        }
                    }
                }

                dgvGSTR16.AllowUserToAddRows = true;
                pbGSTR1.Visible = false;

                if (_str != "")
                {
                    CommonHelper.StatusText = "Draft";
                    int _Result = objGSTR6.InsertValidationFlg("GSTR1", "B2CL", "false", CommonHelper.SelectedMonth);
                    if (_Result != 1)
                        MessageBox.Show("System error.\nPlease try after sometime! SPQValidation Error", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    DialogResult dialogResult = MessageBox.Show("File Not Validated. Do you want error description in excel?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    if (dialogResult == DialogResult.Yes)
                        ExportExcelForValidatation();

                    return false;
                }
                else
                {
                    int _Result = objGSTR6.InsertValidationFlg("GSTR1", "B2CL", "true", CommonHelper.SelectedMonth);
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
                dgvGSTR16.AllowUserToAddRows = true;
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
                    if (cNo == "colPOS")
                    {
                        //if (Utility.IsValidStateName(cellValue))
                        return true;
                        //else
                        //    return false;
                    }
                    else if (cNo == "colInvoiceNo")
                    {
                        if (Utility.IsInvoiceNumber(cellValue))
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
                    else if (cNo == "colInvoiceValue") // value
                    {
                        if (Utility.IsInvoiceValue(cellValue))
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
                    else if (cNo == "colTaxableVal")
                    {
                        if (Utility.IsTaxableValue(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colIGSTAmnt" || cNo == "colCessAmnt")
                    {
                        if (Utility.IsICSC(cellValue))
                            return true;
                        else
                            return false;
                    }
                    else if (cNo == "colEComm") // GSTIN
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

        private void dgvGSTR16_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string cNo = dgvGSTR16.Columns[e.ColumnIndex].Name;

                if (e.RowIndex >= 0)
                {
                    if (cNo == "colPOS")
                    {
                        if (chkCellValue(Convert.ToString(dgvGSTR16.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                            dgvGSTR16.Rows[e.RowIndex].Cells[cNo].Value = Utility.strValidStateName(Convert.ToString(dgvGSTR16.Rows[e.RowIndex].Cells[cNo].Value));
                    }
                    else if (cNo == "colInvoiceDate" || cNo == "colRate" || cNo == "colEComm") // Rest column
                    {
                        if (!chkCellValue(Convert.ToString(dgvGSTR16.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                            dgvGSTR16.Rows[e.RowIndex].Cells[cNo].Value = "";

                        if (cNo == "colRate")
                        {
                            if (Convert.ToString(dgvGSTR16.Rows[e.RowIndex].Cells[cNo].Value).Trim() != "")
                            {
                                dgvGSTR16.CellValueChanged -= dgvGSTR16_CellValueChanged;
                                dgvGSTR16.Rows[e.RowIndex].Cells[cNo].Value = Math.Round(Convert.ToDecimal(dgvGSTR16.Rows[e.RowIndex].Cells[cNo].Value), 2, MidpointRounding.AwayFromZero);
                                dgvGSTR16.CellValueChanged += dgvGSTR16_CellValueChanged;
                            }
                        }
                    }
                    else if (cNo == "colInvoiceNo" || cNo == "colInvoiceValue" || cNo == "colTaxableVal" || cNo == "colIGSTAmnt" || cNo == "colCessAmnt") // value
                    {
                        if (chkCellValue(Convert.ToString(dgvGSTR16.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                        {
                            if (cNo != "colInvoiceNo")
                            {
                                if (Convert.ToString(dgvGSTR16.Rows[e.RowIndex].Cells[cNo].Value).Trim() != "")
                                {
                                    dgvGSTR16.CellValueChanged -= dgvGSTR16_CellValueChanged;
                                    dgvGSTR16.Rows[e.RowIndex].Cells[cNo].Value = Utility.DisplayIndianCurrency(Convert.ToString(Math.Round(Convert.ToDecimal(dgvGSTR16.Rows[e.RowIndex].Cells[cNo].Value), 2, MidpointRounding.AwayFromZero)));
                                    dgvGSTR16.CellValueChanged += dgvGSTR16_CellValueChanged;
                                }
                            }
                        }
                        else { dgvGSTR16.Rows[e.RowIndex].Cells[cNo].Value = ""; }

                        string[] colNo = { dgvGSTR16.Columns[e.ColumnIndex].Name };
                        GetTotal(colNo);
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
                //    MessageBox.Show("Please Select File Status!");
                //    return;
                //}

                #region ADD DATATABLE COLUMN
                DataTable dt = new DataTable();

                foreach (DataGridViewColumn col in dgvGSTR16.Columns)
                {
                    dt.Columns.Add(col.Name.ToString());
                }
                dt.Columns.Add("colFileStatus");
                #endregion

                #region ASSIGN GRIDVIEW ROWS IN DATATABLE
                object[] rowValue = new object[dt.Columns.Count];
                foreach (DataGridViewRow dr in dgvGSTR16.Rows)
                {
                    if (dr.Index != dgvGSTR16.Rows.Count - 1) // DON'T ADD LAST ROW
                    {
                        for (int i = 0; i < dr.Cells.Count; i++)
                        {
                            rowValue[i] = Convert.ToString(dr.Cells[i].Value);
                        }
                        rowValue[dr.Cells.Count] = Convert.ToString(CommonHelper.StatusText);
                        dt.Rows.Add(rowValue);
                    }
                }
                dt.Columns.Remove(dt.Columns["colChk"]);
                dt.AcceptChanges();
                #endregion

                #region RECORD SAVE
                string Query = "";
                int _Result = 0;

                if (dt != null && dt.Rows.Count > 0)
                {
                    #region DELETE RECORD
                    Query = "Delete from SPQR1B2CL where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                    _Result = objGSTR6.IUDData(Query);
                    if (_Result != 1)
                    {
                        // error occurs while deleting data
                        pbGSTR1.Visible = false;
                        MessageBox.Show("System error.\nPlease try after sometime!", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    #endregion

                    _Result = objGSTR6.GSTR1B2CLBulkEntry(dt, Convert.ToString(CommonHelper.StatusText));

                    if (_Result == 1)
                    {
                        // TOTAL CALCULATION
                        string[] colNo = { "colInvoiceNo", "colInvoiceValue", "colTaxableVal", "colIGSTAmnt", "colCessAmnt" };
                        GetTotal(colNo);

                        #region ADD DATATABLE COLUMN
                        dt = new DataTable();

                        foreach (DataGridViewColumn col in dgvGSTR16.Columns)
                        {
                            dt.Columns.Add(col.Name.ToString());
                        }
                        dt.Columns.Add("colFileStatus");
                        #endregion

                        #region ASSIGN GRIDVIEW ROWS IN DATATABLE
                        object[] rowVal = new object[dt.Columns.Count];

                        if (dgvGSTR16Total.Rows.Count == 1)
                        {
                            foreach (DataGridViewRow dr in dgvGSTR16Total.Rows)
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

                        _Result = objGSTR6.GSTR1B2CLBulkEntry(dt, Convert.ToString("Total"));
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
                        //FAIL
                        pbGSTR1.Visible = false;
                        MessageBox.Show("System error.\nPlease try after sometime!", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    #region DELETE RECORD
                    Query = "Delete from SPQR1B2CL where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                    _Result = objGSTR6.IUDData(Query);
                    if (_Result == 1)
                    {
                        //DONE
                        pbGSTR1.Visible = false;
                        MessageBox.Show("Record Successfully Deleted!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ((SPQMDI)Application.OpenForms["SPQMDI"]).SetFileStatus(0);
                        string[] colNo = { "colInvoiceNo", "colInvoiceValue", "colTaxableVal", "colIGSTAmnt", "colCessAmnt" };
                        GetTotal(colNo);
                    }
                    else
                    {
                        //FAIL
                        pbGSTR1.Visible = false;
                        MessageBox.Show("System error.\nPlease try after sometime!", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (dgvGSTR16.CurrentCell.RowIndex == 0 && dgvGSTR16.CurrentCell.ColumnIndex == 0)
                {
                    dgvGSTR16.CurrentCell = dgvGSTR16.Rows[0].Cells[1];
                }
                else { dgvGSTR16.CurrentCell = dgvGSTR16.Rows[0].Cells[0]; }


                // CREATE FLAG FRO DELETE ROWS
                Boolean flgChk = false; Boolean flgSelect = false;

                // CREATE OBJECT OF SELECTED ROW TO DELETE
                List<DataGridViewRow> toDelete = new List<DataGridViewRow>();

                // CHECK THERE ARE RECORD PRESENT IN GRID
                if (dgvGSTR16.Rows.Count > 1)
                {
                    // FLAG TRUE IF CHECK ALL SELECTED
                    if (ckboxHeader.Checked)
                        flgChk = true;

                    #region ADD SELECTED ROW TO OBJECT FOR DELETE
                    for (int i = 0; i < dgvGSTR16.Rows.Count - 1; i++)
                    {
                        if (dgvGSTR16[0, i].Value != null && dgvGSTR16[0, i].Value.ToString() != "")
                        {
                            if (Convert.ToBoolean(dgvGSTR16[0, i].Value) == true)
                            {
                                // ADD ROW TO OBJECT IF ROW IS SELECTED
                                flgSelect = true;
                                toDelete.Add(dgvGSTR16.Rows[i]);
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
                                foreach (DataGridViewColumn col in dgvGSTR16.Columns)
                                {
                                    dt.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }

                                // ASSIGN BLANK DATATABLE TO GRID
                                dgvGSTR16.DataSource = dt;
                            }
                            else if (flgSelect == true)
                            {
                                // DELETE SELECTED ROW
                                foreach (DataGridViewRow row in toDelete)
                                {
                                    dgvGSTR16.Rows.RemoveAt(row.Index);
                                    Application.DoEvents();
                                }
                            }

                            // SEQUANCING MAIN GRID RECORDS
                            for (int i = 0; i < dgvGSTR16.Rows.Count - 1; i++)
                            {
                                dgvGSTR16.Rows[i].Cells["colSequence"].Value = Convert.ToString(i + 1);
                            }

                            if (dgvGSTR16.Rows.Count == 1)
                            {
                                // IF THERE ARE NO RECORDS IN MAIN GRID THENE ASSIGN BLANK DATATABLE TO TOTAL GRID
                                DataTable dtTotal = new DataTable();
                                foreach (DataGridViewColumn col in dgvGSTR16Total.Columns)
                                {
                                    dtTotal.Columns.Add(col.Name.ToString());
                                    col.DataPropertyName = col.Name;
                                }
                                dgvGSTR16Total.DataSource = dtTotal;
                            }

                            // SET CONTROL PROPERTY AFTER ROW DELETION
                            ckboxHeader.Checked = false;
                            dgvGSTR16.Columns[0].HeaderText = "Check All";
                            #endregion
                        }
                    }

                    pbGSTR1.Visible = false;

                    // TOTAL CALCULATION
                    string[] colNo = { "colInvoiceNo", "colInvoiceValue", "colTaxableVal", "colIGSTAmnt", "colCessAmnt" };
                    GetTotal(colNo);
                }
                else
                {
                    // IF THERE ARE NO RECORD TO DELETE
                    ckboxHeader.Checked = false;
                    dgvGSTR16.Columns[0].HeaderText = "Check All";
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

        #region Excel transactions

        public void ImportExcel()
        {
            try
            {
                string filePath = string.Empty; string fileExt = string.Empty;

                OpenFileDialog file = new OpenFileDialog(); //open dialog to choose file  
                if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    filePath = file.FileName;
                    fileExt = Path.GetExtension(filePath);
                    if (fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0 || fileExt.CompareTo(".xlsm") == 0)
                    {
                        pbGSTR1.Visible = true;

                        #region if impoted file is open then close open file
                        foreach (Process proc in System.Diagnostics.Process.GetProcessesByName("EXCEL"))
                        {
                            if (proc.MainWindowTitle == "Microsoft Excel - " + file.SafeFileName)
                                proc.Kill();
                        }
                        #endregion

                        DataTable dt = new DataTable();
                        dt = (DataTable)dgvGSTR16.DataSource;

                        DataTable dtExcel = new DataTable();
                        dtExcel = ReadExcel(filePath, fileExt, dt);

                        // check imported template
                        if (dtExcel.Columns.Count != 1)
                        {
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                // combine imported excel data and grid data

                                // disable main grid
                                DisableControls(dgvGSTR16);

                                #region import excel datatable to grid datatable
                                if (dtExcel != null && dtExcel.Rows.Count > 0)
                                {
                                    foreach (DataRow row in dtExcel.Rows)
                                    {
                                        // copy each row of imported datatable row to grid datatale
                                        DataRow newRow = dt.NewRow();
                                        newRow.ItemArray = row.ItemArray;
                                        dt.Rows.Add(newRow);
                                        dt.Rows[dt.Rows.Count - 1]["colSequence"] = dt.Rows.Count;
                                        Application.DoEvents();
                                    }
                                }
                                dt.AcceptChanges();
                                #endregion

                                #region rename datatable column name as par main grid
                                foreach (DataGridViewColumn col in dgvGSTR16.Columns)
                                {
                                    dt.Columns[col.Index].ColumnName = col.Name.ToString();
                                    col.DataPropertyName = col.Name;
                                }
                                #endregion

                                //Assign datatable to datagrid                                
                                dgvGSTR16.DataSource = dt;

                                // enable main grid
                                EnableControls(dgvGSTR16);
                            }
                            else
                            {
                                // if there are no records in main grid

                                if (dtExcel != null && dtExcel.Rows.Count > 0)
                                {
                                    // if there are data in imported excel file

                                    // disable main grid
                                    DisableControls(dgvGSTR16);

                                    #region rename datatable column name as par main grid
                                    foreach (DataGridViewColumn col in dgvGSTR16.Columns)
                                    {
                                        dtExcel.Columns[col.Index].ColumnName = col.Name.ToString();
                                        col.DataPropertyName = col.Name;
                                    }
                                    dtExcel.AcceptChanges();

                                    // assign datatale to grid
                                    dgvGSTR16.DataSource = dtExcel;

                                    // enable main grid
                                    EnableControls(dgvGSTR16);
                                    #endregion
                                }
                                else
                                {
                                    pbGSTR1.Visible = false;
                                    MessageBox.Show("There are no records found in imported excel ...!!!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }

                            string[] colNo = { "colInvoiceNo", "colInvoiceValue", "colTaxableVal", "colIGSTAmnt", "colCessAmnt" };
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
                        MessageBox.Show("Please choose .xls or .xlsx file only.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //custom messageBox to show error  
                    }
                }
            }
            catch (Exception ex)
            {
                EnableControls(dgvGSTR16);
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
            bool flg = false;
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
                        OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [b2cl$]", con);
                        oleAdpt.Fill(dtexcel); //fill excel data into dataTable
                    }
                    catch
                    {
                        // call when imported template sheet name is differ from predefine template
                        DataTable dt = new DataTable();
                        dt.Columns.Add("colError");
                        return dt;
                    }
                    dtexcel = Utility.RemoveEmptyRowsFromDataTable(dtexcel);
                    if (dtexcel != null && dtexcel.Rows.Count > 0)
                    {
                        #region REMOVE UNUSED COLUMN FROM EXCEL
                        if (dtexcel.Columns.Count >= dgvGSTR16.Columns.Count)
                        {
                            for (int k = dtexcel.Columns.Count - 1; k > (dgvGSTR16.Columns.Count - 3); k--)
                            {
                                dtexcel.Columns.Remove(dtexcel.Columns[k]);
                            }
                        }
                        dtexcel.AcceptChanges();
                        #endregion

                        #region validate template
                        for (int i = 1; i < dgvGSTR16.Columns.Count; i++)
                        {
                            flg = false;
                            for (int j = 0; j < dtexcel.Columns.Count; j++)
                            {
                                // check grid column is present or not in imported excel
                                if (dgvGSTR16.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim() == dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim())
                                {
                                    // if grid column present in excel then its index as par grid column index
                                    flg = true;
                                    dtexcel.Columns[j].SetOrdinal(dgvGSTR16.Columns[i].Index - 1);
                                    break;
                                }
                            }
                            //if (flg == false)
                            //{
                            //    // if grid column not present in excel then return datatable with error
                            //    DataTable dt = new DataTable();
                            //    dt.Columns.Add("colError");
                            //    return dt;
                            //}
                            dtexcel.AcceptChanges();
                        }
                        #endregion

                        #region Remove unused column from excel
                        if (dtexcel.Columns.Count >= dgvGSTR16.Columns.Count - 2)
                        {
                            for (int i = dtexcel.Columns.Count - 1; i > (dgvGSTR16.Columns.Count - 2); i--)
                            {
                                dtexcel.Columns.Remove(dtexcel.Columns[i]);
                            }
                        }
                        #endregion

                        dtexcel.Columns.Add("colSequence");
                        dtexcel.Columns[dtexcel.Columns.Count - 1].SetOrdinal(0);
                        dtexcel.Columns["Party Name"].SetOrdinal(1);
                        dtexcel.Columns["Place Of Supply"].SetOrdinal(2);
                        dtexcel.Columns["Invoice No"].SetOrdinal(3);
                        dtexcel.Columns["Invoice Date"].SetOrdinal(4);
                        dtexcel.Columns["Invoice Value"].SetOrdinal(5);
                        dtexcel.Columns["Rate"].SetOrdinal(6);
                        dtexcel.Columns["Taxable Value"].SetOrdinal(7);
                        dtexcel.Columns["IGST Amount"].SetOrdinal(8);
                        dtexcel.Columns["CESS Amount"].SetOrdinal(9);
                        dtexcel.Columns["E-Commerce GSTIN"].SetOrdinal(10);

                        #region Assign column name to datatable
                        foreach (DataGridViewColumn col in dgvGSTR16.Columns)
                        {
                            if (col.Index != 0)
                                dtexcel.Columns[col.Index - 1].ColumnName = col.Name.ToString();
                        }
                        #endregion

                        // ADD CHECK BOX COLUMN TO DATATBLE AND MAKE IT FIRST TABLE COLUMN
                        dtexcel.Columns.Add(new DataColumn("colChk"));
                        dtexcel.Columns["colChk"].SetOrdinal(0);
                        dtexcel.Columns.Add("colError");
                        dtexcel.AcceptChanges();

                        #region SET COLTAX VALUE AS TRUE/FALSE
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
                    pbGSTR1.Visible = false;
                    MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                    StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                    errorWriter.Write(errorMessage);
                    errorWriter.Close();
                }

                return dtexcel;
            }
        }

        public void ExportExcel()
        {
            try
            {
                if (dgvGSTR16.Rows.Count > 1)
                {
                    // if records are present in main grid

                    pbGSTR1.Visible = true;

                    #region Create Workbook and assign columnName
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook WB = excelApp.Workbooks.Add(Missing.Value);

                    Excel.Worksheet newWS = (Excel.Worksheet)excelApp.Worksheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    newWS.Name = "B2CL";

                    // Delete unused worksheets from workbook
                    foreach (Excel.Worksheet ws in WB.Worksheets)
                    {
                        if (ws.Name != "B2CL")
                            ((Excel.Worksheet)WB.Worksheets[ws.Name]).Delete();
                    }

                    // assign column header as par the grid header
                    for (int i = 1; i < dgvGSTR16.Columns.Count; i++)
                    {
                        newWS.Cells[1, i] = dgvGSTR16.Columns[i].HeaderText.ToString();

                        // set column width
                        if (i == 1)
                            ((Excel.Range)newWS.Cells[1, i]).ColumnWidth = 7;
                        else if (i >= 2 && i <= 11)
                            ((Excel.Range)newWS.Cells[1, i]).ColumnWidth = 10;
                        else
                            ((Excel.Range)newWS.Cells[1, i]).ColumnWidth = 15;
                    }

                    // get range and set range properties
                    Excel.Range headerRange = (Excel.Range)newWS.get_Range((Excel.Range)newWS.Cells[1, 1], (Excel.Range)newWS.Cells[1, dgvGSTR16.Columns.Count - 1]);
                    headerRange.WrapText = true;
                    headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Calibri";

                    #endregion

                    #region Copy Data from DataTable to Array

                    if (dgvGSTR16.Rows.Count <= 0)
                        throw new Exception("ExportToExcel: There are no records in grid...!!!\n");

                    // Create Array to hold the data of DataTable                
                    object[,] arr = new object[dgvGSTR16.Rows.Count - 1, dgvGSTR16.Columns.Count];

                    // Assign data to Array from DataTable
                    if (CommonHelper.IsLicence)
                    {
                        // for licenece allows to export all records
                        for (int i = 0; i < dgvGSTR16.Rows.Count - 1; i++)
                        {
                            for (int j = 1; j < dgvGSTR16.Columns.Count; j++)
                            {
                                arr[i, j - 1] = Convert.ToString(dgvGSTR16.Rows[i].Cells[j].Value);
                            }
                        }
                    }
                    else
                    {
                        // for demo allow only 100 records to export
                        for (int i = 0; i < dgvGSTR16.Rows.Count - 1; i++)
                        {
                            if (i < 100)
                            {
                                for (int j = 1; j < dgvGSTR16.Columns.Count; j++)
                                {
                                    arr[i, j - 1] = Convert.ToString(dgvGSTR16.Rows[i].Cells[j].Value);
                                }
                            }
                        }
                    }

                    //Set Excel Range to Paste the Data
                    Excel.Range top = (Excel.Range)newWS.Cells[2, 1];
                    Excel.Range bottom = (Excel.Range)newWS.Cells[dgvGSTR16.Rows.Count, dgvGSTR16.Columns.Count];
                    Excel.Range sheetRange = newWS.Range[top, bottom];

                    Excel.Range rg = (Excel.Range)sheetRange.Cells[1, 4];
                    rg.EntireColumn.NumberFormat = "@";

                    rg = (Excel.Range)sheetRange.Cells[1, 5];
                    rg.EntireColumn.NumberFormat = "dd-MM-yyyy";



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

        public void ExportExcelForValidatation()
        {
            List<int> listValid = new List<int>();
            try
            {
                if (dgvGSTR16.Rows.Count > 1)
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
                    for (int i = 2; i < dgvGSTR16.Columns.Count + 1; i++)
                    {
                        newWS.Cells[1, i - 1] = dgvGSTR16.Columns[yy].HeaderText.ToString();

                        ((Excel.Range)newWS.Cells[1, i - 1]).ColumnWidth = 17;
                        yy++;
                    }

                    ((Excel.Range)newWS.Cells[1, 13]).ColumnWidth = 45;
                    // GET RANGE AND SET DIFFRENT PROPERTIES
                    Excel.Range headerRange = (Excel.Range)newWS.get_Range((Excel.Range)newWS.Cells[1, 1], (Excel.Range)newWS.Cells[1, dgvGSTR16.Columns.Count]);
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
                    foreach (DataGridViewColumn column in dgvGSTR16.Columns)
                        dt.Columns.Add(column.Name, typeof(string));

                    for (int k = 0; k < dgvGSTR16.Rows.Count; k++)
                    {
                        for (int j = 0; j < dgvGSTR16.ColumnCount; j++)
                        {
                            if (dgvGSTR16.Rows[k].Cells[j].Style.BackColor == Color.Red)
                            {
                                ExcelValidFlag = true;
                                //sheetRange.Cells[k + 1, j - 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                            }
                        }
                        if (ExcelValidFlag == true)
                        {
                            dt.Rows.Add();
                            int count = dt.Rows.Count - 1;
                            for (int b = 0; b < dgvGSTR16.Columns.Count; b++)
                            {
                                dt.Rows[count][b] = dgvGSTR16.Rows[k].Cells[b].Value;
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

                    for (int k = 0; k < dgvGSTR16.Rows.Count; k++)
                    {
                        string str_error = "";
                        int cnt = 1;
                        for (int j = 0; j < dgvGSTR16.ColumnCount; j++)
                        {
                            if (dgvGSTR16.Rows[k].Cells[j].Style.BackColor == Color.Red)
                            {
                                ExcelValidFlag = true;
                                sheetRange.Cells[Ab + 1, j].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                if (dgvGSTR16.Columns[j].Name == "colInvoiceNo")
                                {
                                    if (Convert.ToString(dgvGSTR16.Rows[k].Cells[j].Value).Trim() == "")
                                        str_error += cnt + ") " + " Please enter " + dgvGSTR16.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") Invoice number max length is 16 And invoice number can consist only(/) and (-) Or Same invoice number can not be possible on B2B and B2CL. Invoice number already exists. \n";
                                }
                                else if (dgvGSTR16.Columns[j].Name == "colInvoiceValue")
                                {
                                    if (dgvGSTR16.Rows[k].Cells[j].Value == "")
                                        str_error += cnt + ") " + " Please enter " + dgvGSTR16.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") Different " + dgvGSTR16.Columns[j].HeaderText + " can not be possible for same invoice no Or Invoice Value Should be more than Rs.250000. \n";
                                }
                                else if (dgvGSTR16.Columns[j].Name == "colInvoiceDate")
                                {
                                    if (Convert.ToString(dgvGSTR16.Rows[k].Cells[j].Value).Trim() == "")
                                        str_error += cnt + ") " + " Please enter " + dgvGSTR16.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") " + " Please enter proper " + dgvGSTR16.Columns[j].HeaderText + " on this format(dd/MM/YYYY) OR Enter current month Date OR Same invoice no same invoice date is required. \n";
                                }
                                else if (dgvGSTR16.Columns[j].Name == "colPOS")
                                {
                                    if (Convert.ToString(dgvGSTR16.Rows[k].Cells[j].Value).Trim() == "")
                                        str_error += cnt + ") " + " Please select " + dgvGSTR16.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") " + " Please enter proper " + dgvGSTR16.Columns[j].HeaderText + " Or Same invoice no for different POS is not possible Or Same state can not be possible on " + dgvGSTR16.Columns[j].HeaderText + ". \n";
                                }
                                else if (dgvGSTR16.Columns[j].Name == "colRate")
                                {
                                    if (Convert.ToString(dgvGSTR16.Rows[k].Cells[j].Value).Trim() == "")
                                        str_error += cnt + ") " + " Please enter " + dgvGSTR16.Columns[j].HeaderText + "(Ex : (0),(2.5),(3),(5),(12),(18),(28)). \n";
                                    else
                                        str_error += cnt + ") " + " Please enter proper " + dgvGSTR16.Columns[j].HeaderText + "(Ex : (0),(2.5),(3),(5),(12),(18),(28)).\n";
                                }
                                else if (dgvGSTR16.Columns[j].Name == "colTaxableVal")
                                {
                                    if (dgvGSTR16.Rows[k].Cells[j].Value == "")
                                        str_error += cnt + ") " + " Please enter " + dgvGSTR16.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") Taxable values must be max 11 digit and can not be more than Invoice value. \n";
                                }
                                else if (dgvGSTR16.Columns[j].Name == "colIGSTAmnt")
                                {
                                    if (Convert.ToString(dgvGSTR16.Rows[k].Cells[j].Value).Trim() == "")
                                        str_error += cnt + ") " + " Please enter Must be " + dgvGSTR16.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") " + " Please enter exact match " + dgvGSTR16.Columns[j].HeaderText + " base on `Taxable values` and `Rate` calculation. \n";
                                }
                                else if (dgvGSTR16.Columns[j].Name == "colCessAmnt")
                                {
                                    if (Convert.ToString(dgvGSTR16.Rows[k].Cells[j].Value).Trim() == "")
                                        str_error += cnt + ") " + " Please enter Must be " + dgvGSTR16.Columns[j].HeaderText + ".\n";
                                    else
                                        str_error += cnt + ") " + " Please enter proper " + dgvGSTR16.Columns[j].HeaderText + ".\n";
                                }
                                else
                                {
                                    str_error += cnt + ") " + " Please enter proper " + dgvGSTR16.Columns[j].HeaderText + ".\n";
                                }
                                cnt++;
                            }
                        }
                        if (ExcelValidFlag == true)
                        {
                            Ab++;
                            dt_new.Rows.Add();
                            int c = dt_new.Rows.Count;
                            for (int b = 0; b < dgvGSTR16.Columns.Count; b++)
                            {
                                if (dt_new.Columns.Count - 1 == b)
                                {
                                    dt_new.Rows[c - 1][b] = str_error;
                                }
                                else
                                {
                                    dt_new.Rows[c - 1][b] = Convert.ToString(dgvGSTR16.Rows[k].Cells[b].Value);
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

        #region CSV transactions

        public void ImportCSV()
        {
            try
            {
                string filePath = string.Empty, fileExt = string.Empty;
                OpenFileDialog file = new OpenFileDialog(); //open dialog to choose file  

                if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    filePath = file.FileName;
                    fileExt = Path.GetExtension(filePath);
                    if (fileExt.CompareTo(".csv") == 0 || fileExt.CompareTo(".~csv") == 0)
                    {
                        pbGSTR1.Visible = true;

                        DataTable dt = new DataTable();
                        dt = (DataTable)dgvGSTR16.DataSource;

                        DataTable dtCsv = new DataTable();
                        dtCsv = GetDataTabletFromCSVFile(filePath, dt);

                        // check imported template
                        if (dtCsv.Columns.Count != 1)
                        {
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                // combine imported csv data and grid data

                                // disable main grid
                                DisableControls(dgvGSTR16);

                                #region copy imported csv datatable data into grid datatable
                                if (dtCsv != null && dtCsv.Rows.Count > 0)
                                {
                                    foreach (DataRow row in dtCsv.Rows)
                                    {
                                        // copy each row of imported datatable row to grid datatale
                                        DataRow newRow = dt.NewRow();
                                        newRow.ItemArray = row.ItemArray;
                                        dt.Rows.Add(newRow);
                                        dt.Rows[dt.Rows.Count - 1]["colSequence"] = dt.Rows.Count;
                                        Application.DoEvents();
                                    }
                                }
                                #endregion

                                #region rename column name as par grid column name
                                foreach (DataGridViewColumn col in dgvGSTR16.Columns)
                                {
                                    dtCsv.Columns[col.Index].ColumnName = col.Name.ToString();
                                    col.DataPropertyName = col.Name;
                                }
                                dt.AcceptChanges();
                                #endregion

                                // assign datatable to grid
                                dgvGSTR16.DataSource = dt;

                                // enable main grid
                                EnableControls(dgvGSTR16);
                            }
                            else
                            {
                                // if there are no records in main grid

                                if (dtCsv != null && dtCsv.Rows.Count > 0)
                                {
                                    // if there are record present in import file

                                    // disable main grid
                                    DisableControls(dgvGSTR16);

                                    #region RENAME COLUMN NAME AS PAR GRID COLUMN NAME AND ASSIGN TO MAIN GRID
                                    foreach (DataGridViewColumn col in dgvGSTR16.Columns)
                                    {
                                        dtCsv.Columns[col.Index].ColumnName = col.Name.ToString();
                                        col.DataPropertyName = col.Name;
                                    }
                                    dtCsv.AcceptChanges();

                                    // ASSIGN DATATABLE TO GRID
                                    dgvGSTR16.DataSource = dtCsv;

                                    // ENABLE CONTROL
                                    EnableControls(dgvGSTR16);
                                    #endregion
                                }
                                else
                                {
                                    pbGSTR1.Visible = false;
                                    MessageBox.Show("There are no records in CSV file...!!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }

                            string[] colNo = { "colInvoiceNo", "colInvoiceValue", "colTaxableVal", "colIGSTAmnt", "colCessAmnt" };
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
                        MessageBox.Show("Please choose .csv or .~csv file only.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //custom messageBox to show error  
                    }
                }
            }
            catch (Exception ex)
            {
                EnableControls(dgvGSTR16);
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
            DataTable csvData = new DataTable();

            using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
            {
                try
                {
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
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData);
                    }
                    #endregion

                    #region validate template
                    for (int i = 1; i < dgvGSTR16.Columns.Count; i++)
                    {
                        Boolean flg = false;
                        for (int j = 0; j < csvData.Columns.Count; j++)
                        {
                            // check grid column is present or not in imported excel
                            if (dgvGSTR16.Columns[i].HeaderText.Replace(" ", "") == csvData.Columns[j].ColumnName.Replace(" ", "").Trim())
                            {
                                // if grid column present in excel then its index as par grid column index
                                flg = true;
                                csvData.Columns[j].SetOrdinal(dgvGSTR16.Columns[i].Index - 1);
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
                    if (csvData.Columns.Count >= dgvGSTR16.Columns.Count - 2)
                    {
                        for (int i = csvData.Columns.Count - 1; i > (dgvGSTR16.Columns.Count - 2); i--)
                        {
                            csvData.Columns.Remove(csvData.Columns[i]);
                        }
                    }
                    #endregion

                    #region rename column name as par grid column name
                    foreach (DataGridViewColumn col in dgvGSTR16.Columns)
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
                if (dgvGSTR16.Rows.Count > 1)
                {
                    // if records are present in main grid

                    pbGSTR1.Visible = true;

                    string csv = string.Empty;
                    DataTable dt = new DataTable();
                    dt = (DataTable)dgvGSTR16.DataSource;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        #region Assign column name to csv string
                        for (int i = 1; i < dgvGSTR16.Columns.Count; i++)
                        {
                            csv += dgvGSTR16.Columns[i].HeaderText + ',';
                        }

                        //Add new line.
                        csv += "\r\n";
                        #endregion

                        #region Assign dreid row to csv string
                        StringBuilder sb = new StringBuilder();
                        sb.Append(csv);

                        // seprate each record and append as seprated strinf
                        int sj = 0;
                        if (CommonHelper.IsLicence)
                        {
                            // for licenece allows to export all records
                            foreach (DataRow row in dt.Rows)
                            {
                                var fields = row.ItemArray.Select(field => "\"" + field.ToString().Replace("\"", "\"\"") + "\"").Skip(1).ToArray();
                                sb.AppendLine(string.Join(",", fields));
                                sj++;
                            }
                        }
                        else
                        {
                            // for demo allow only 100 records to export
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

                        #region Exporting to CSV
                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Filter = "CSV files (*.csv)|*.csv";
                        if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            try
                            {
                                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(sfd.FileName, false))
                                {
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
                    // if there are no record in main  grid
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
                PdfPTable pdfTable = new PdfPTable(dgvGSTR16.ColumnCount - 2);
                pdfTable.DefaultCell.Padding = 3;
                pdfTable.WidthPercentage = 100;
                pdfTable.DefaultCell.BorderWidth = 0;
                iTextSharp.text.Font fontHeader = iTextSharp.text.FontFactory.GetFont("Calibri", 6);

                // ADD HEADER TO PDF TABLE
                pdfTable = AssignHeader(pdfTable, "5. Taxable outward inter-State supplies to un-registered persons where the invoice value is more than Rs 2.5 lakh");
                #endregion

                #region ADDING HEADER ROW
                int i = 0;

                #region HEADER1
                PdfPCell celHeader1 = new PdfPCell();

                celHeader1 = new PdfPCell(new Phrase("Place of Supply (State)", fontHeader));
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
                celHeader1.Colspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("GSTIN of Ecommerce", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                pdfTable.CompleteRow();
                #endregion

                #region HEADER2
                PdfPCell celHeader2 = new PdfPCell();

                celHeader2 = new PdfPCell(new Phrase("No", fontHeader));
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

                celHeader2 = new PdfPCell(new Phrase("CESS", fontHeader));
                celHeader2 = SetAllignMent(celHeader2, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(255, 255, 204));
                pdfTable.AddCell(celHeader2);

                pdfTable.CompleteRow();
                #endregion

                Application.DoEvents();
                #endregion

                #region ADDING COLUMN NUMBER
                i = 0;
                foreach (DataGridViewColumn column in dgvGSTR16.Columns)
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
                    foreach (DataGridViewRow row in dgvGSTR16.Rows)
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
                    foreach (DataGridViewRow row in dgvGSTR16.Rows)
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
                ce1.Colspan = dgvGSTR16.Columns.Count - 2;
                ce1.VerticalAlignment = Element.ALIGN_CENTER;
                ce1.HorizontalAlignment = Element.ALIGN_LEFT;
                ce1.BackgroundColor = new iTextSharp.text.BaseColor(197, 223, 197);
                ce1.BorderWidth = 0;
                pdfTable.AddCell(ce1);

                iTextSharp.text.Font FigToRs = FontFactory.GetFont("Calibri", 10, iTextSharp.text.Font.BOLD);
                PdfPCell ceHeader2 = new PdfPCell(new Phrase("(figures in Rs)", FigToRs));
                ceHeader2.Colspan = dgvGSTR16.Columns.Count - 2;
                ceHeader2.VerticalAlignment = Element.ALIGN_CENTER;
                ceHeader2.HorizontalAlignment = Element.ALIGN_RIGHT;
                ceHeader2.BorderWidth = 0;
                pdfTable.AddCell(ceHeader2);

                PdfPCell ce2 = new PdfPCell(new Phrase(" "));
                ce2.Colspan = dgvGSTR16.Columns.Count - 2;
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

        #region JSON CLASS
        public class ItmDet
        {
            public string ty { get; set; }
            public string hsn_sc { get; set; }
            public double txval { get; set; }
            public double irt { get; set; }
            public double iamt { get; set; }
            public double csrt { get; set; }
            public double csamt { get; set; }

            //Delete In V.2
            public double srt { get; set; }
            public double samt { get; set; }
        }

        public class Itm
        {
            public int num { get; set; }
            public ItmDet itm_det { get; set; }
        }

        public class Inv
        {
            public string cname { get; set; }
            public string inum { get; set; }
            public string idt { get; set; }
            public double val { get; set; }
            public string pos { get; set; }
            public string prs { get; set; }
            public List<Itm> itms { get; set; }
            public string chksum { get; set; }

            //Add new PAramter

            public int od_num { get; set; }
            public DateTime od_dt { get; set; }
            public string etin { get; set; }

            //end
        }

        public class B2cl
        {
            public int state_cd { get; set; }
            public List<Inv> inv { get; set; }
        }

        public class RootObject
        {
            public List<B2cl> b2cl { get; set; }
        }
        #endregion

        public void JSONCreator()
        {
            try
            {
                RootObject ObjJson = new RootObject();

                #region State Code Group By
                List<string> StateCodelist = dgvGSTR16.Rows
                       .OfType<DataGridViewRow>()
                       .Select(x => Convert.ToString(x.Cells["colreciptStateCode"].Value))
                       .Distinct().ToList();
                #endregion

                if (StateCodelist != null && StateCodelist.Count > 0)
                {
                    pbGSTR1.Visible = true;

                    for (int k = 0; k < StateCodelist.Count; k++)
                    {
                        if (Convert.ToString(StateCodelist[k]) != "")
                        {
                            #region Invoice Number Group By
                            List<string> list = dgvGSTR16.Rows
                                   .OfType<DataGridViewRow>()
                                   .Where(x => Convert.ToString(x.Cells["colreciptStateCode"].Value) == Convert.ToString(StateCodelist[k]))
                                   .Select(x => Convert.ToString(x.Cells["colInvoiceNo"].Value))
                                   .Distinct().ToList();
                            #endregion

                            if (list != null && list.Count > 0)
                            {
                                List<B2cl> obj = new List<B2cl>();

                                B2cl b2bObj = new B2cl();
                                b2bObj.state_cd = Convert.ToInt32(dgvGSTR16.Rows[k].Cells["colreciptStateCode"].Value);//State Code

                                if (ObjJson.b2cl == null)
                                {
                                    obj.Add(b2bObj);
                                    ObjJson.b2cl = obj;
                                }
                                else
                                {
                                    obj.Add(b2bObj);
                                    ObjJson.b2cl.AddRange(obj);
                                }

                                for (int i = 0; i < list.Count; i++)
                                {
                                    if (Convert.ToString(list[i]) != "")
                                    {
                                        #region Invoice Number
                                        List<DataGridViewRow> Invoicelist = dgvGSTR16.Rows
                                               .OfType<DataGridViewRow>()
                                               .Where(x => Convert.ToString(list[i]) == Convert.ToString(x.Cells["colInvoiceNo"].Value))//Invoice Number
                                               .Select(x => x)
                                               .Distinct().ToList();
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

                                                    clsInv.cname = Convert.ToString(Invoicelist[j].Cells["colNameOfRec"].Value);//Name of the
                                                    clsInv.inum = Convert.ToString(Invoicelist[j].Cells["colInvoiceNo"].Value);//Invoice Number
                                                    clsInv.idt = Convert.ToString(Invoicelist[j].Cells["colInvoiceDate"].Value);// Invoice Date
                                                    int val = Convert.ToInt32(Invoicelist.Cast<DataGridViewRow>().Where(x => x.Cells["colInvoiceValue"].Value != null).Sum(x => x.Cells["colInvoiceValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colInvoiceValue"].Value)));
                                                    clsInv.val = val;
                                                    //if (Convert.ToString(Invoicelist[j].Cells[6].Value).Trim() != "")//Invoice Value
                                                    //{
                                                    //    clsInv.val = Convert.ToDouble(Invoicelist[j].Cells[6].Value);
                                                    //}
                                                    //else { clsInv.val = 0.0; }

                                                    clsInv.pos = Convert.ToString(Invoicelist[j].Cells["colCessRate"].Value);//POS

                                                    if (Convert.ToString(Invoicelist[j].Cells["colTax"].Value).ToLower() == "true")//Tax on this Invoice is paid under provisional assessment
                                                    {
                                                        clsInv.prs = Convert.ToString("Y");
                                                    }
                                                    else { clsInv.prs = Convert.ToString("N"); }

                                                    if (ObjJson.b2cl[k].inv == null)
                                                    {
                                                        objInv.Add(clsInv); ObjJson.b2cl[k].inv = objInv;
                                                    }
                                                    else { objInv.Add(clsInv); ObjJson.b2cl[k].inv.AddRange(objInv); }

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

                                                if (Convert.ToString(Invoicelist[j].Cells["colTaxableVal"].Value).Trim() != "")//Taxable Value
                                                {
                                                    clsItmDet.txval = Convert.ToDouble(Invoicelist[j].Cells["colTaxableVal"].Value);
                                                }
                                                else { clsItmDet.txval = 0.0; }
                                                if (Convert.ToString(Invoicelist[j].Cells["colRate"].Value).Trim() != "")//IGST Rate
                                                {
                                                    clsItmDet.irt = Convert.ToDouble(Invoicelist[j].Cells["colRate"].Value);
                                                }
                                                else { clsItmDet.irt = 0.0; }
                                                if (Convert.ToString(Invoicelist[j].Cells["colIGSTAmnt"].Value).Trim() != "")//IGST Amount
                                                {
                                                    clsItmDet.iamt = Convert.ToDouble(Invoicelist[j].Cells["colIGSTAmnt"].Value);
                                                }
                                                else { clsItmDet.iamt = 0.0; }

                                                if (Convert.ToString(Invoicelist[j].Cells["colCessRate"].Value).Trim() != "")//Cess Rate
                                                {
                                                    clsItmDet.csrt = Convert.ToDouble(Invoicelist[j].Cells["colCessRate"].Value);
                                                }
                                                else { clsItmDet.csrt = 0.0; }
                                                if (Convert.ToString(Invoicelist[j].Cells["colCessAmnt"].Value).Trim() != "")//Cess Amount
                                                {
                                                    clsItmDet.csamt = Convert.ToDouble(Invoicelist[j].Cells["colCessAmnt"].Value);
                                                }
                                                else { clsItmDet.csamt = 0.0; }

                                                clsItems.itm_det = clsItmDet;
                                                objItm.Add(clsItems);
                                                ObjJson.b2cl[k].inv[i].itms = objItm;
                                                #endregion
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    pbGSTR1.Visible = false;

                    #region File Save
                    JavaScriptSerializer objScript = new JavaScriptSerializer();
                    objScript.MaxJsonLength = 2147483647;
                    string FinalJson = objScript.Serialize(ObjJson);
                    SaveFileDialog save = new SaveFileDialog();
                    save.FileName = "B2CL.json";
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

                #region State Code Group By
                List<string> StateCodelist = dgvGSTR16.Rows
                       .OfType<DataGridViewRow>()
                       .Select(x => Convert.ToString(x.Cells[2].Value))
                       .Distinct().ToList();
                #endregion

                if (StateCodelist != null && StateCodelist.Count > 1)
                {
                    for (int k = 0; k < StateCodelist.Count; k++)
                    {
                        if (Convert.ToString(StateCodelist[k]) != "")
                        {
                            #region Invoice Number Group By
                            List<string> list = dgvGSTR16.Rows
                                   .OfType<DataGridViewRow>()
                                   .Where(x => Convert.ToString(x.Cells["colreciptStateCode"].Value) == Convert.ToString(StateCodelist[k]))
                                   .Select(x => Convert.ToString(x.Cells["colInvoiceNo"].Value))
                                   .Distinct().ToList();
                            #endregion

                            if (list != null && list.Count > 0)
                            {
                                List<B2cl> obj = new List<B2cl>();

                                B2cl b2bObj = new B2cl();
                                b2bObj.state_cd = Convert.ToInt32(dgvGSTR16.Rows[k].Cells["colreciptStateCode"].Value);//State Code


                                if (ObjJson.b2cl == null)
                                {
                                    obj.Add(b2bObj);
                                    ObjJson.b2cl = obj;
                                }
                                else
                                {
                                    obj.Add(b2bObj);
                                    ObjJson.b2cl.AddRange(obj);
                                }

                                for (int i = 0; i < list.Count; i++)
                                {
                                    if (Convert.ToString(list[i]) != "")
                                    {
                                        #region Invoice Number
                                        List<DataGridViewRow> Invoicelist = dgvGSTR16.Rows
                                               .OfType<DataGridViewRow>()
                                               .Where(x => Convert.ToString(list[i]) == Convert.ToString(x.Cells["colInvoiceNo"].Value))//Invoice Number
                                               .Select(x => x)
                                               .Distinct().ToList();
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

                                                    clsInv.cname = Convert.ToString(Invoicelist[j].Cells["colNameOfRec"].Value);//Name of the
                                                    clsInv.inum = Convert.ToString(Invoicelist[j].Cells["colInvoiceNo"].Value);//Invoice Number
                                                    clsInv.idt = Convert.ToString(Invoicelist[j].Cells["colInvoiceDate"].Value);// Invoice Date
                                                    int val = Convert.ToInt32(Invoicelist.Cast<DataGridViewRow>().Where(x => x.Cells["colInvoiceValue"].Value != null).Sum(x => x.Cells["colInvoiceValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colInvoiceValue"].Value)));
                                                    clsInv.val = val;

                                                    clsInv.pos = Convert.ToString(Invoicelist[j].Cells["colCessRate"].Value);//POS

                                                    if (Convert.ToString(Invoicelist[j].Cells["colTax"].Value).ToLower() == "true")//Tax on this Invoice is paid under provisional assessment
                                                    {
                                                        clsInv.prs = Convert.ToString("Y");
                                                    }
                                                    else { clsInv.prs = Convert.ToString("N"); }

                                                    if (ObjJson.b2cl[k].inv == null)
                                                    {
                                                        objInv.Add(clsInv); ObjJson.b2cl[k].inv = objInv;
                                                    }
                                                    else { objInv.Add(clsInv); ObjJson.b2cl[k].inv.AddRange(objInv); }

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

                                                if (Convert.ToString(Invoicelist[j].Cells["colTaxableVal"].Value).Trim() != "")//Taxable Value
                                                {
                                                    clsItmDet.txval = Convert.ToDouble(Invoicelist[j].Cells["colTaxableVal"].Value);
                                                }
                                                else { clsItmDet.txval = 0.0; }
                                                if (Convert.ToString(Invoicelist[j].Cells["colRate"].Value).Trim() != "")//IGST Rate
                                                {
                                                    clsItmDet.irt = Convert.ToDouble(Invoicelist[j].Cells["colRate"].Value);
                                                }
                                                else { clsItmDet.irt = 0.0; }
                                                if (Convert.ToString(Invoicelist[j].Cells["colIGSTAmnt"].Value).Trim() != "")//IGST Amount
                                                {
                                                    clsItmDet.iamt = Convert.ToDouble(Invoicelist[j].Cells["colIGSTAmnt"].Value);
                                                }
                                                else { clsItmDet.iamt = 0.0; }
                                                clsItems.itm_det = clsItmDet;
                                                objItm.Add(clsItems);
                                                ObjJson.b2cl[k].inv[i].itms = objItm;
                                                #endregion
                                            }
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
                else
                {
                    MessageBox.Show("No Data Avilable.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        public void SetGridViewColor()
        {
            try
            {
                // do not allow to auto generate columns
                dgvGSTR16.AutoGenerateColumns = false;

                // set height width of form
                this.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.89));
                this.Height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.77));

                // set width of header, main and total grid
                this.pnlHeader.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.875));
                this.dgvGSTR16.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.875));
                this.dgvGSTR16Total.Width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * (0.875));

                // set height of main grid
                this.dgvGSTR16.Height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.612));
                this.dgvGSTR16Total.Height = 45;

                // set location of header,loading pic, checkbox and main and total grid
                this.pnlHeader.Location = new System.Drawing.Point(12, 0);
                this.pbClose.Location = new System.Drawing.Point(pbClose.Location.X - 20, pbClose.Location.Y);
                this.dgvGSTR16.Location = new System.Drawing.Point(12, Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.085)));
                this.dgvGSTR16Total.Location = new System.Drawing.Point(12, Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height * (0.705)));

                dgvGSTR16.EnableHeadersVisualStyles = false;
                dgvGSTR16.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(23, 196, 187);
                dgvGSTR16.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

                this.dgvGSTR16.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                this.dgvGSTR16.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                this.dgvGSTR16.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

                foreach (DataGridViewColumn column in dgvGSTR16.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.Automatic;
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

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)dgvGSTR16.DataSource;
                if (dt == null)
                {
                    MessageBox.Show("Kindly save record(s) before search!!!", "Alert", MessageBoxButtons.OK);
                    return;
                }
                if (cmbFilter.SelectedValue.ToString() == "")
                {
                    ((DataTable)dgvGSTR16.DataSource).DefaultView.RowFilter = string.Format("colParty like '%{0}%' or colPOS like '%{0}%' or colInvoiceNo like '%{0}%' or colInvoiceDate like '%{0}%' or colInvoiceValue like '%{0}%' or colRate like '%{0}%' or colTaxableVal like '%{0}%' or colIGSTAmnt like '%{0}%' or colCessAmnt like '%{0}%' or colEComm like '%{0}%'", txtSearch.Text.Trim().Replace("'", "''"));
                }
                else
                {
                    ((DataTable)dgvGSTR16.DataSource).DefaultView.RowFilter = string.Format("" + cmbFilter.SelectedValue + " like '%{0}%'", txtSearch.Text.Trim().Replace("'", "''"));
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

        private void dgvGSTR16_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                dgvGSTR16.Rows[e.Row.Index - 1].Cells["colSequence"].Value = e.Row.Index;
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

        private void dgvGSTR16_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                for (int i = e.Row.Index; i < dgvGSTR16.Rows.Count - 1; i++)
                {
                    dgvGSTR16.Rows[i].Cells["colSequence"].Value = i;
                }

                string[] colNo = { "colInvoiceNo", "colInvoiceValue", "colTaxableVal", "colIGSTAmnt", "colCessAmnt" };
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

        #region disable/enable controls

        private void DisableControls(Control con)
        {
            foreach (Control c in con.Controls)
            {
                if (c.Name != "SPQGSTR1B2CL" && c.Name != "dgvGSTR16Total")
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

        private void dgvGSTR16_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                this.dgvGSTR16Total.HorizontalScrollingOffset = this.dgvGSTR16.HorizontalScrollingOffset;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvGSTR16Total_Scroll(object sender, ScrollEventArgs e)
        {

            try
            {
                this.dgvGSTR16.HorizontalScrollingOffset = this.dgvGSTR16Total.HorizontalScrollingOffset;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void dgvGSTR16Total_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //this.dgvGSTR16Total.ClearSelection();
            this.dgvGSTR16.ClearSelection();
            //dgvGSTR16Total.Rows[0].Height = 30;
            if (dgvGSTR16Total.Rows.Count > 0)
            {
                DataGridViewRow row = this.dgvGSTR16Total.RowTemplate;
                row.MinimumHeight = 30;
            }
        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            //frmGSTR1B2CSummary obj = new frmGSTR1B2CSummary();
            //obj.MdiParent = this.MdiParent;
            //Utility.CloseAllOpenForm();
            //obj.Dock = DockStyle.Fill;
            //obj.Show();


            //((SPQMDI)Application.OpenForms["SPQMDI"]).HideExtraToolsMenu();
            //((SPQMDI)Application.OpenForms["SPQMDI"]).HideCompanyDetailMenu();
        }

        private void frmGSTR16_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName("");
        }

        private void dgvGSTR16_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                // CHECK FIRST COLUMN HEADER PRESSED AND MAIN GRID HAVING RECORDS
                if (e.ColumnIndex == 0 && dgvGSTR16.Rows.Count > 1)
                {
                    // CHECK AND UNCHECK CHECK BOX OF HEADER FOR SELECTING AND UNSELECTING ALL RECORDS
                    if (dgvGSTR16.Columns[e.ColumnIndex].HeaderText == "Check All")
                        ckboxHeader.Checked = true;
                    else if (dgvGSTR16.Columns[e.ColumnIndex].HeaderText == "Uncheck All")
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
                if (dgvGSTR16.Rows.Count > 1)
                {
                    pbGSTR1.Visible = true;
                    if (ckboxHeader.Checked)
                    {
                        // IF CHECK BOX IS CHECKED

                        // SET CHECK BOX COLUMN VALUE AS TRUE
                        for (int i = 0; i < dgvGSTR16.Rows.Count - 1; i++)
                        {
                            dgvGSTR16.Rows[i].Cells[0].Value = "True";
                            Application.DoEvents();
                        }

                        // CHANGE HEADER TEXT AND WIDTH OF COLUMN AND POSITION OF CHECK BOX OF CHECK ALL COLUMN
                        //dgvGSTR27Other.Columns[0].DefaultCellStyle.NullValue = true;
                        dgvGSTR16.Columns[0].HeaderText = "Uncheck All";
                    }
                    else if (ckboxHeader.Checked == false)
                    {
                        // IF CHECK BOX IS UNCHECKED

                        // SET CHECK BOX COLUMN VALUE AS FALSE
                        for (int i = 0; i < dgvGSTR16.Rows.Count - 1; i++)
                        {
                            dgvGSTR16.Rows[i].Cells[0].Value = "False";
                            Application.DoEvents();
                        }

                        // CHANGE HEADER TEXT AND WIDTH OF COLUMN AND POSITION OF CHECK BOX OF CHECK ALL COLUMN
                        //dgvGSTR27Other.Columns[0].DefaultCellStyle.NullValue = false;
                        dgvGSTR16.Columns[0].HeaderText = "Check All";
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


        private void btnClose_Click(object sender, EventArgs e)
        {
            //(new SPQMDI()).Save_Close();
            ((SPQMDI)Application.OpenForms["SPQMDI"]).Save_Close();

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
            IsValidateData();
           // Validate();
        }

        private void btnVerifyGSTIN_Click(object sender, EventArgs e)
        {
           // ValidataAndGetGSTIN();
            
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}
