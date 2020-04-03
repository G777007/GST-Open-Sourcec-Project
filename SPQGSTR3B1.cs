using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SPEQTAGST.BAL;
using SPEQTAGST.BAL.M796r3b;
using System.Reflection;
using System.Data.OleDb;
using Newtonsoft.Json;
using SPEQTAGST.BAL.M264r1;

namespace SPEQTAGST.rintlcs3b
{
    public partial class SPQGSTR3B1 : Form
    {
        r1Publicclass objGSTR5 = new r1Publicclass();
        r3bPublicclass objGSTR3B = new r3bPublicclass();

        public SPQGSTR3B1()
        {
            InitializeComponent();
            GetData();
            BindData();

            // total calculation
            string[] colNo = { "colTotalTaxableValue", "colIGST", "colCGST", "colSGST", "colCESS" };
            GetTotal(colNo);

            if (CommonHelper.IsQuarter == true)
            {
                btnGSTR1.Visible = false;
            }

            SetGridViewColor();
            ((SPQMDI)Application.OpenForms["SPQMDI"]).ShowExtraToolsMenu();
            ((SPQMDI)Application.OpenForms["SPQMDI"]).ShowCompanyDetailMenu();
            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName(CommonHelper.ReturnName);

            dgvGSTR3B1.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dgvGSTR3B1.EnableHeadersVisualStyles = false;
            dgvGSTR3B1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);
            dgvGSTR3B1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvGSTR3B1.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            TotaldgvGSTR13.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void GetData()
        {
            try
            {
                // create datatable to store database data
                DataTable dt = new DataTable();
                string Query = "Select * from SPQR3BOutwardSupplies where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
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

                    //RENAME DATATABLE COLUMN NAME TO DATAGRIDVIEW COLUMN NAME
                    foreach (DataGridViewColumn col in dgvGSTR3B1.Columns)
                    {
                        dt.Columns[col.Index].ColumnName = col.Name.ToString();
                        col.DataPropertyName = col.Name;
                    }
                    dt.AcceptChanges();
                    dgvGSTR3B1.DataSource = dt;
                }
                else
                {
                    dgvGSTR3B1.DataSource = null;
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

        public DataTable GetGSTR1data()
        {
            DataTable dtGSTR1 = new DataTable();
            try
            {
                DataTable dt = new DataTable();

                dtGSTR1.Columns.Add("Type of Invoices", typeof(string));
                dtGSTR1.Columns.Add("Taxable Value", typeof(string));
                dtGSTR1.Columns.Add("IGST", typeof(string));
                dtGSTR1.Columns.Add("CGST", typeof(string));
                dtGSTR1.Columns.Add("SGST", typeof(string));
                dtGSTR1.Columns.Add("Cess", typeof(string));

                #region  B2B
                #region For Only Regular
                string Query = "Select * from SPQR1B2B where Fld_InvType = 'Regular' AND Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id;";// DESC LIMIT 2;";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count > 0)
                {
                    decimal? Fld_InvoiceTaxableVal = 0;
                    decimal? Fld_IGSTAmnt = 0;
                    decimal? Fld_CGSTAmnt = 0;
                    decimal? Fld_SGSTAmnt = 0;
                    decimal? Fld_CessAmount = 0;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["Fld_InvoiceTaxableVal"] != null && Convert.ToString(dt.Rows[i]["Fld_InvoiceTaxableVal"]) != "")
                        {
                            Fld_InvoiceTaxableVal = Fld_InvoiceTaxableVal + Convert.ToDecimal(dt.Rows[i]["Fld_InvoiceTaxableVal"]);
                        }
                        if (dt.Rows[i]["Fld_IGSTAmnt"] != null && Convert.ToString(dt.Rows[i]["Fld_IGSTAmnt"]) != "")
                        {
                            Fld_IGSTAmnt = Fld_IGSTAmnt + Convert.ToDecimal(dt.Rows[i]["Fld_IGSTAmnt"]);
                        }
                        if (dt.Rows[i]["Fld_CGSTAmnt"] != null && Convert.ToString(dt.Rows[i]["Fld_CGSTAmnt"]) != "")
                        {
                            Fld_CGSTAmnt = Fld_CGSTAmnt + Convert.ToDecimal(dt.Rows[i]["Fld_CGSTAmnt"]);
                        }
                        if (dt.Rows[i]["Fld_SGSTAmnt"] != null && Convert.ToString(dt.Rows[i]["Fld_SGSTAmnt"]) != "")
                        {
                            Fld_SGSTAmnt = Fld_SGSTAmnt + Convert.ToDecimal(dt.Rows[i]["Fld_SGSTAmnt"]);
                        }
                        if (dt.Rows[i]["Fld_CessAmount"] != null && Convert.ToString(dt.Rows[i]["Fld_CessAmount"]) != "")
                        {
                            Fld_CessAmount = Fld_CessAmount + Convert.ToDecimal(dt.Rows[i]["Fld_CessAmount"]);
                        }
                    }

                    dtGSTR1.Rows.Add("B2B", Fld_InvoiceTaxableVal, Fld_IGSTAmnt, Fld_CGSTAmnt, Fld_SGSTAmnt, Fld_CessAmount);
                }
                else
                {
                    dtGSTR1.Rows.Add("B2B", "0", "0", "0", "0", "0");
                }
                #endregion

                #region For Only not Regular
                Query = "Select * from SPQR1B2B where Fld_InvType != 'Regular' AND Fld_InvType != '' AND Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id;";// DESC LIMIT 2;";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count > 0)
                {
                    decimal? Fld_InvoiceTaxableVal = 0;
                    decimal? Fld_IGSTAmnt = 0;
                    decimal? Fld_CGSTAmnt = 0;
                    decimal? Fld_SGSTAmnt = 0;
                    decimal? Fld_CessAmount = 0;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["Fld_InvoiceTaxableVal"] != null && Convert.ToString(dt.Rows[i]["Fld_InvoiceTaxableVal"]) != "")
                        {
                            Fld_InvoiceTaxableVal = Fld_InvoiceTaxableVal + Convert.ToDecimal(dt.Rows[i]["Fld_InvoiceTaxableVal"]);
                        }
                        if (dt.Rows[i]["Fld_IGSTAmnt"] != null && Convert.ToString(dt.Rows[i]["Fld_IGSTAmnt"]) != "")
                        {
                            Fld_IGSTAmnt = Fld_IGSTAmnt + Convert.ToDecimal(dt.Rows[i]["Fld_IGSTAmnt"]);
                        }
                        if (dt.Rows[i]["Fld_CGSTAmnt"] != null && Convert.ToString(dt.Rows[i]["Fld_CGSTAmnt"]) != "")
                        {
                            Fld_CGSTAmnt = Fld_CGSTAmnt + Convert.ToDecimal(dt.Rows[i]["Fld_CGSTAmnt"]);
                        }
                        if (dt.Rows[i]["Fld_SGSTAmnt"] != null && Convert.ToString(dt.Rows[i]["Fld_SGSTAmnt"]) != "")
                        {
                            Fld_SGSTAmnt = Fld_SGSTAmnt + Convert.ToDecimal(dt.Rows[i]["Fld_SGSTAmnt"]);
                        }
                        if (dt.Rows[i]["Fld_CessAmount"] != null && Convert.ToString(dt.Rows[i]["Fld_CessAmount"]) != "")
                        {
                            Fld_CessAmount = Fld_CessAmount + Convert.ToDecimal(dt.Rows[i]["Fld_CessAmount"]);
                        }
                    }

                    dtGSTR1.Rows.Add("B2BNOTREG", Fld_InvoiceTaxableVal, Fld_IGSTAmnt, Fld_CGSTAmnt, Fld_SGSTAmnt, Fld_CessAmount);
                }
                else
                {
                    dtGSTR1.Rows.Add("B2BNOTREG", "0", "0", "0", "0", "0");
                }
                #endregion

                #endregion

                #region B2CL
                Query = "Select * from SPQR1B2CL where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count > 1)
                {
                    dtGSTR1.Rows.Add("B2CL", dt.Rows[0]["Fld_TaxableValue"].ToString(), dt.Rows[0]["Fld_IGST"].ToString(), "0", "0", dt.Rows[0]["Fld_CESS"].ToString());
                }
                else
                {
                    dtGSTR1.Rows.Add("B2CL", "0", "0", "0", "0", "0");
                }
                #endregion

                #region B2CS
                Query = "Select * from SPQR1B2CS where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count > 1)
                {
                    dtGSTR1.Rows.Add("B2CS", dt.Rows[0]["Fld_TaxableValue"].ToString(), dt.Rows[0]["Fld_IGST"].ToString(), dt.Rows[0]["Fld_CGST"].ToString(), dt.Rows[0]["Fld_SGST"].ToString(), dt.Rows[0]["Fld_CESS"].ToString());
                }
                else
                {
                    dtGSTR1.Rows.Add("B2CS", "0", "0", "0", "0", "0");
                }
                #endregion

                #region Zero rated supplies
                Query = "Select * from SPQR1ZeroRated where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count > 0)
                {
                    dtGSTR1.Rows.Add("ZRS", dt.Rows[0]["Fld_IGSTInvoiceTaxableVal"].ToString(), dt.Rows[0]["Fld_IGSTAmnt"].ToString(), "", "", dt.Rows[0]["Fld_CESS"].ToString());
                }
                else
                {
                    dtGSTR1.Rows.Add("ZRS", "0", "0", "0", "0", "0");
                }
                #endregion

                #region Credit/Debit Note

                #region For Only Debit Note
                Query = "Select * from SPQR1CDN where Fld_TypeOfNote ='Debit Note' AND Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id;";// DESC LIMIT 2;";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);

                if (dt != null && dt.Rows.Count > 0)
                {
                    // dtGSTR1.Rows.Add("CDN", dt.Rows[0]["Fld_Taxable"].ToString(), dt.Rows[0]["Fld_IGSTAmnt"].ToString(), dt.Rows[0]["Fld_CGSTAmnt"].ToString(), dt.Rows[0]["Fld_SGSTAmnt"].ToString(), dt.Rows[0]["Fld_CessAmnt"].ToString());

                    decimal? Fld_InvoiceTaxableVal = 0;
                    decimal? Fld_IGSTAmnt = 0;
                    decimal? Fld_CGSTAmnt = 0;
                    decimal? Fld_SGSTAmnt = 0;
                    decimal? Fld_CessAmount = 0;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["Fld_Taxable"] != null && Convert.ToString(dt.Rows[i]["Fld_Taxable"]) != "")
                        {
                            Fld_InvoiceTaxableVal = Fld_InvoiceTaxableVal + Convert.ToDecimal(dt.Rows[i]["Fld_Taxable"]);
                        }
                        if (dt.Rows[i]["Fld_IGSTAmnt"] != null && Convert.ToString(dt.Rows[i]["Fld_IGSTAmnt"]) != "")
                        {
                            Fld_IGSTAmnt = Fld_IGSTAmnt + Convert.ToDecimal(dt.Rows[i]["Fld_IGSTAmnt"]);
                        }
                        if (dt.Rows[i]["Fld_CGSTAmnt"] != null && Convert.ToString(dt.Rows[i]["Fld_CGSTAmnt"]) != "")
                        {
                            Fld_CGSTAmnt = Fld_CGSTAmnt + Convert.ToDecimal(dt.Rows[i]["Fld_CGSTAmnt"]);
                        }
                        if (dt.Rows[i]["Fld_SGSTAmnt"] != null && Convert.ToString(dt.Rows[i]["Fld_SGSTAmnt"]) != "")
                        {
                            Fld_SGSTAmnt = Fld_SGSTAmnt + Convert.ToDecimal(dt.Rows[i]["Fld_SGSTAmnt"]);
                        }
                        if (dt.Rows[i]["Fld_CessAmnt"] != null && Convert.ToString(dt.Rows[i]["Fld_CessAmnt"]) != "")
                        {
                            Fld_CessAmount = Fld_CessAmount + Convert.ToDecimal(dt.Rows[i]["Fld_CessAmnt"]);
                        }
                    }

                    dtGSTR1.Rows.Add("CDN", Fld_InvoiceTaxableVal, Fld_IGSTAmnt, Fld_CGSTAmnt, Fld_SGSTAmnt, Fld_CessAmount);
                }
                else
                {
                    dtGSTR1.Rows.Add("CDN", "0", "0", "0", "0", "0");
                }
                #endregion

                #region For Only Credit Note AND Refund Voucher
                Query = "Select * from SPQR1CDN where Fld_TypeOfNote !='Debit Note' AND Fld_TypeOfNote !='' AND Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC;";// LIMIT 2;";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);

                if (dt != null && dt.Rows.Count > 0)
                {
                    // dtGSTR1.Rows.Add("CDNCREREF", dt.Rows[0]["Fld_Taxable"].ToString(), dt.Rows[0]["Fld_IGSTAmnt"].ToString(), dt.Rows[0]["Fld_CGSTAmnt"].ToString(), dt.Rows[0]["Fld_SGSTAmnt"].ToString(), dt.Rows[0]["Fld_CessAmnt"].ToString());

                    decimal? Fld_InvoiceTaxableVal = 0;
                    decimal? Fld_IGSTAmnt = 0;
                    decimal? Fld_CGSTAmnt = 0;
                    decimal? Fld_SGSTAmnt = 0;
                    decimal? Fld_CessAmount = 0;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["Fld_Taxable"] != null && Convert.ToString(dt.Rows[i]["Fld_Taxable"]) != "")
                        {
                            Fld_InvoiceTaxableVal = Fld_InvoiceTaxableVal + Convert.ToDecimal(dt.Rows[i]["Fld_Taxable"]);
                        }
                        if (dt.Rows[i]["Fld_IGSTAmnt"] != null && Convert.ToString(dt.Rows[i]["Fld_IGSTAmnt"]) != "")
                        {
                            Fld_IGSTAmnt = Fld_IGSTAmnt + Convert.ToDecimal(dt.Rows[i]["Fld_IGSTAmnt"]);
                        }
                        if (dt.Rows[i]["Fld_CGSTAmnt"] != null && Convert.ToString(dt.Rows[i]["Fld_CGSTAmnt"]) != "")
                        {
                            Fld_CGSTAmnt = Fld_CGSTAmnt + Convert.ToDecimal(dt.Rows[i]["Fld_CGSTAmnt"]);
                        }
                        if (dt.Rows[i]["Fld_SGSTAmnt"] != null && Convert.ToString(dt.Rows[i]["Fld_SGSTAmnt"]) != "")
                        {
                            Fld_SGSTAmnt = Fld_SGSTAmnt + Convert.ToDecimal(dt.Rows[i]["Fld_SGSTAmnt"]);
                        }
                        if (dt.Rows[i]["Fld_CessAmnt"] != null && Convert.ToString(dt.Rows[i]["Fld_CessAmnt"]) != "")
                        {
                            Fld_CessAmount = Fld_CessAmount + Convert.ToDecimal(dt.Rows[i]["Fld_CessAmnt"]);
                        }
                    }

                    dtGSTR1.Rows.Add("CDNCREREF", Fld_InvoiceTaxableVal, Fld_IGSTAmnt, Fld_CGSTAmnt, Fld_SGSTAmnt, Fld_CessAmount);
                }
                else
                {
                    dtGSTR1.Rows.Add("CDNCREREF", "0", "0", "0", "0", "0");
                }
                #endregion

                #endregion

                #region CDN UR

                #region For Only Debit Note
                Query = "Select * from SPQR1CDNUR where Fld_TypeOfNote ='Debit Note' AND Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC;";// LIMIT 2;";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count > 0)
                {
                    // dtGSTR1.Rows.Add("CDNUR", dt.Rows[0]["Fld_Taxable"].ToString(), dt.Rows[0]["Fld_IGSTAmnt"].ToString(), dt.Rows[0]["Fld_CGSTAmnt"].ToString(), dt.Rows[0]["Fld_SGSTAmnt"].ToString(), dt.Rows[0]["Fld_CessAmnt"].ToString());

                    decimal? Fld_InvoiceTaxableVal = 0;
                    decimal? Fld_IGSTAmnt = 0;
                    decimal? Fld_CGSTAmnt = 0;
                    decimal? Fld_SGSTAmnt = 0;
                    decimal? Fld_CessAmount = 0;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["Fld_Taxable"] != null && Convert.ToString(dt.Rows[i]["Fld_Taxable"]) != "")
                        {
                            Fld_InvoiceTaxableVal = Fld_InvoiceTaxableVal + Convert.ToDecimal(dt.Rows[i]["Fld_Taxable"]);
                        }
                        if (dt.Rows[i]["Fld_IGSTAmnt"] != null && Convert.ToString(dt.Rows[i]["Fld_IGSTAmnt"]) != "")
                        {
                            Fld_IGSTAmnt = Fld_IGSTAmnt + Convert.ToDecimal(dt.Rows[i]["Fld_IGSTAmnt"]);
                        }
                        if (dt.Rows[i]["Fld_CGSTAmnt"] != null && Convert.ToString(dt.Rows[i]["Fld_CGSTAmnt"]) != "")
                        {
                            Fld_CGSTAmnt = Fld_CGSTAmnt + Convert.ToDecimal(dt.Rows[i]["Fld_CGSTAmnt"]);
                        }
                        if (dt.Rows[i]["Fld_SGSTAmnt"] != null && Convert.ToString(dt.Rows[i]["Fld_SGSTAmnt"]) != "")
                        {
                            Fld_SGSTAmnt = Fld_SGSTAmnt + Convert.ToDecimal(dt.Rows[i]["Fld_SGSTAmnt"]);
                        }
                        if (dt.Rows[i]["Fld_CessAmnt"] != null && Convert.ToString(dt.Rows[i]["Fld_CessAmnt"]) != "")
                        {
                            Fld_CessAmount = Fld_CessAmount + Convert.ToDecimal(dt.Rows[i]["Fld_CessAmnt"]);
                        }
                    }

                    dtGSTR1.Rows.Add("CDNUR", Fld_InvoiceTaxableVal, Fld_IGSTAmnt, Fld_CGSTAmnt, Fld_SGSTAmnt, Fld_CessAmount);
                }
                else
                {
                    dtGSTR1.Rows.Add("CDNUR", "0", "0", "0", "0", "0");
                }
                #endregion

                #region For Only Credit Note AND Refund Voucher
                Query = "Select * from SPQR1CDNUR where Fld_TypeOfNote !='Debit Note' AND Fld_TypeOfNote !='' AND Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id;";// DESC LIMIT 2;";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count > 0)
                {
                    // dtGSTR1.Rows.Add("CDNURCREREF", dt.Rows[0]["Fld_Taxable"].ToString(), dt.Rows[0]["Fld_IGSTAmnt"].ToString(), dt.Rows[0]["Fld_CGSTAmnt"].ToString(), dt.Rows[0]["Fld_SGSTAmnt"].ToString(), dt.Rows[0]["Fld_CessAmnt"].ToString());

                    decimal? Fld_InvoiceTaxableVal = 0;
                    decimal? Fld_IGSTAmnt = 0;
                    decimal? Fld_CGSTAmnt = 0;
                    decimal? Fld_SGSTAmnt = 0;
                    decimal? Fld_CessAmount = 0;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["Fld_Taxable"] != null && Convert.ToString(dt.Rows[i]["Fld_Taxable"]) != "")
                        {
                            Fld_InvoiceTaxableVal = Fld_InvoiceTaxableVal + Convert.ToDecimal(dt.Rows[i]["Fld_Taxable"]);
                        }
                        if (dt.Rows[i]["Fld_IGSTAmnt"] != null && Convert.ToString(dt.Rows[i]["Fld_IGSTAmnt"]) != "")
                        {
                            Fld_IGSTAmnt = Fld_IGSTAmnt + Convert.ToDecimal(dt.Rows[i]["Fld_IGSTAmnt"]);
                        }
                        if (dt.Rows[i]["Fld_CGSTAmnt"] != null && Convert.ToString(dt.Rows[i]["Fld_CGSTAmnt"]) != "")
                        {
                            Fld_CGSTAmnt = Fld_CGSTAmnt + Convert.ToDecimal(dt.Rows[i]["Fld_CGSTAmnt"]);
                        }
                        if (dt.Rows[i]["Fld_SGSTAmnt"] != null && Convert.ToString(dt.Rows[i]["Fld_SGSTAmnt"]) != "")
                        {
                            Fld_SGSTAmnt = Fld_SGSTAmnt + Convert.ToDecimal(dt.Rows[i]["Fld_SGSTAmnt"]);
                        }
                        if (dt.Rows[i]["Fld_CessAmnt"] != null && Convert.ToString(dt.Rows[i]["Fld_CessAmnt"]) != "")
                        {
                            Fld_CessAmount = Fld_CessAmount + Convert.ToDecimal(dt.Rows[i]["Fld_CessAmnt"]);
                        }
                    }

                    dtGSTR1.Rows.Add("CDNURCREREF", Fld_InvoiceTaxableVal, Fld_IGSTAmnt, Fld_CGSTAmnt, Fld_SGSTAmnt, Fld_CessAmount);
                }
                else
                {
                    dtGSTR1.Rows.Add("CDNURCREREF", "0", "0", "0", "0", "0");
                }
                #endregion

                #endregion

                #region Nil Rated
                /*
                Query = "Select * from SPQR1NilRatedMulti where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count > 1)
                {
                    dtGSTR1.Rows.Add("NILRATED", dt.Rows[0]["Fld_InvoiceValue"].ToString(), "0", "0", "0", "0");
                }
                else
                {
                    dtGSTR1.Rows.Add("NILRATED", "0", "0", "0", "0", "0");
                }
                */

                Query = "Select * from SPQR1NilRated where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                decimal Fld_TaxTurn_1 = 0;
                decimal Fld_TaxTurn_1NonGST = 0;
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["Fld_NilRatedSupply"].ToString() != "")
                            Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[i]["Fld_NilRatedSupply"]);
                        if (dt.Rows[i]["Fld_Exempted"].ToString() != "")
                            Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[i]["Fld_Exempted"]);
                        if (dt.Rows[i]["Fld_NonGSTSupplies"].ToString() != "")
                            Fld_TaxTurn_1NonGST = Fld_TaxTurn_1NonGST + Convert.ToDecimal(dt.Rows[i]["Fld_NonGSTSupplies"]);
                    }
                }
                if (Fld_TaxTurn_1 != 0)
                {
                    dtGSTR1.Rows.Add("NILRATED", Fld_TaxTurn_1, "0", "0", "0", "0");
                }
                else
                {
                    dtGSTR1.Rows.Add("NILRATED", "0", "0", "0", "0", "0");
                }
                if (Fld_TaxTurn_1NonGST != 0)
                {
                    dtGSTR1.Rows.Add("NILRATEDNONGST", Fld_TaxTurn_1NonGST, "0", "0", "0", "0");
                }
                else
                {
                    dtGSTR1.Rows.Add("NILRATEDNONGST", "0", "0", "0", "0", "0");
                }
                #endregion

                #region Form Gross Advance (Advance Received)
                Query = "Select * from SPQR1GrossAdvance where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count > 1)
                {
                    dtGSTR1.Rows.Add("AR", dt.Rows[0]["Fld_GrossAdvRcv"].ToString(), dt.Rows[0]["Fld_IGSTAmnt"].ToString(), dt.Rows[0]["Fld_CGSTAmnt"].ToString(), dt.Rows[0]["Fld_SGSTAmnt"].ToString(), dt.Rows[0]["Fld_CessAmount"].ToString());
                }
                else
                {
                    dtGSTR1.Rows.Add("AR", "0", "0", "0", "0", "0");
                }
                #endregion

                #region Form Net Advance (Advance Adjusted)
                Query = "Select * from SPQR1NetAdvance where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count > 1)
                {
                    dtGSTR1.Rows.Add("AA", dt.Rows[0]["Fld_Advadj"].ToString(), dt.Rows[0]["Fld_IGSTAmnt"].ToString(), dt.Rows[0]["Fld_CGSTAmnt"].ToString(), dt.Rows[0]["Fld_SGSTAmnt"].ToString(), dt.Rows[0]["Fld_CessAmount"].ToString());
                }
                else
                {
                    dtGSTR1.Rows.Add("AA", "0", "0", "0", "0", "0");
                }
                #endregion

                for (int i = 0; i < dtGSTR1.Rows.Count; i++)
                {
                    for (int j = 0; j < dtGSTR1.Columns.Count; j++)
                    {
                        string ColName = dtGSTR1.Columns[j].ColumnName;
                        if (ColName == "Taxable Value" || ColName == "IGST" || ColName == "CGST" || ColName == "SGST" || ColName == "Cess")
                            dtGSTR1.Rows[i][j] = Utility.DisplayIndianCurrency(Convert.ToString(dtGSTR1.Rows[i][j]));
                    }
                }

                return dtGSTR1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();

                return dtGSTR1;
            }
        }

        private void BindData()
        {
            try
            {
                if (dgvGSTR3B1.Rows.Count <= 1)
                {
                    DataTable dt = new DataTable();

                    // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                    foreach (DataGridViewColumn col in dgvGSTR3B1.Columns)
                    {
                        dt.Columns.Add(col.Name.ToString());
                        col.DataPropertyName = col.Name;
                    }
                    dt.AcceptChanges();

                    DataRow dr = dt.NewRow();
                    dr["colSrNo"] = "1";
                    dr["colNatureofSupply"] = "(a) Outward Taxable Supplies (other than Zero rated,nil rated and exemted)";
                    dr["colTotalTaxableValue"] = "";
                    dr["colIGST"] = "";
                    dr["colCGST"] = "";
                    dr["colSGST"] = "";
                    dr["colCESS"] = "";
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr["colSrNo"] = "2";
                    dr["colNatureofSupply"] = "(b) Outward Taxable Supplies (Zero rated)";
                    dr["colTotalTaxableValue"] = "";
                    dr["colIGST"] = "";
                    dr["colCGST"] = "";
                    dr["colSGST"] = "";
                    dr["colCESS"] = "";
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr["colSrNo"] = "3";
                    dr["colNatureofSupply"] = "(c) Other outward Supplies(Nil rated, exemted)";
                    dr["colTotalTaxableValue"] = "";
                    dr["colIGST"] = "";
                    dr["colCGST"] = "";
                    dr["colSGST"] = "";
                    dr["colCESS"] = "";
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr["colSrNo"] = "4";
                    dr["colNatureofSupply"] = "(d) Inward Supplies(liable to reverse charge)";
                    dr["colTotalTaxableValue"] = "";
                    dr["colIGST"] = "";
                    dr["colCGST"] = "";
                    dr["colSGST"] = "";
                    dr["colCESS"] = "";
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr["colSrNo"] = "5";
                    dr["colNatureofSupply"] = "(e) Non-GST outward supplies";
                    dr["colTotalTaxableValue"] = "";
                    dr["colIGST"] = "";
                    dr["colCGST"] = "";
                    dr["colSGST"] = "";
                    dr["colCESS"] = "";
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    // assign datatable to main grid
                    dgvGSTR3B1.DataSource = dt;

                    dgvGSTR3B1.Columns["colNatureofSupply"].ReadOnly = true;
                    //dgvGSTR13.Columns["colReduce"].ReadOnly = true;
                    DataGridViewRow row = this.dgvGSTR3B1.RowTemplate;
                    row.MinimumHeight = 30;
                    for (int i = 0; i < dgvGSTR3B1.Rows.Count; i++)
                    {
                        for (int j = 3; j < dgvGSTR3B1.ColumnCount; j++)
                        {
                            if (dgvGSTR3B1.Rows[i].Cells[j].Value.ToString() == "-" || dgvGSTR3B1.Rows[i].Cells[j].Value.ToString() == "" || dgvGSTR3B1.Rows[i].Cells[j].Value == null)
                            {
                                dgvGSTR3B1.Rows[i].Cells[j].Value = "";
                            }
                        }
                    }
                }
                else
                {
                    dgvGSTR3B1.Rows[0].Cells[1].Value = "(a) Outward Taxable Supplies (other than Zero rated,nil rated and exemted)";
                    dgvGSTR3B1.Rows[1].Cells[1].Value = "(b) Outward Taxable Supplies (Zero rated)";
                    dgvGSTR3B1.Rows[2].Cells[1].Value = "(c) Other outward Supplies(Nil rated, exemted)";
                    dgvGSTR3B1.Rows[3].Cells[1].Value = "(d) Inward Supplies(liable to reverse charge)";
                    dgvGSTR3B1.Rows[4].Cells[1].Value = "(e) Non-GST outward supplies";
                    DataGridViewRow row = this.dgvGSTR3B1.RowTemplate;
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
                if (dgvGSTR3B1.Rows.Count == 5)
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
                        dr["colTTotalTaxableValue"] = dgvGSTR3B1.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colTotalTaxableValue"].Value != null).Sum(x => x.Cells["colTotalTaxableValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colTotalTaxableValue"].Value)).ToString();
                        dr["colTIGST"] = dgvGSTR3B1.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colIGST"].Value != null).Sum(x => x.Cells["colIGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIGST"].Value)).ToString();
                        dr["colTCGST"] = dgvGSTR3B1.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCGST"].Value != null).Sum(x => x.Cells["colCGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCGST"].Value)).ToString();
                        dr["colTSGST"] = dgvGSTR3B1.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colSGST"].Value != null).Sum(x => x.Cells["colSGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colSGST"].Value)).ToString();
                        dr["colTCESS"] = dgvGSTR3B1.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCESS"].Value != null).Sum(x => x.Cells["colCESS"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCESS"].Value)).ToString();

                        // add datarow to datatable
                        dtTotal.Rows.Add(dr);

                        for (int i = 0; i < dtTotal.Rows.Count; i++)
                        {
                            for (int j = 0; j < dtTotal.Columns.Count; j++)
                            {
                                string ColName = dtTotal.Columns[j].ColumnName;
                                if (ColName == "colTTotalTaxableValue" || ColName == "colTIGST" || ColName == "colTCGST" || ColName == "colTSGST" || ColName == "colTCESS")
                                    dtTotal.Rows[i][j] = Utility.DisplayIndianCurrency(Convert.ToString(dtTotal.Rows[i][j]));
                            }
                        }

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
                            if (item == "colTotalTaxableValue")
                                TotaldgvGSTR13.Rows[0].Cells["colTTotalTaxableValue"].Value = Utility.DisplayIndianCurrency(dgvGSTR3B1.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colTotalTaxableValue"].Value != null).Sum(x => x.Cells["colTotalTaxableValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colTotalTaxableValue"].Value)).ToString());
                            if (item == "colIGST")
                                TotaldgvGSTR13.Rows[0].Cells["colTIGST"].Value = Utility.DisplayIndianCurrency(dgvGSTR3B1.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colIGST"].Value != null).Sum(x => x.Cells["colIGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIGST"].Value)).ToString());
                            else if (item == "colCGST")
                                TotaldgvGSTR13.Rows[0].Cells["colTCGST"].Value = Utility.DisplayIndianCurrency(dgvGSTR3B1.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCGST"].Value != null).Sum(x => x.Cells["colCGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCGST"].Value)).ToString());
                            else if (item == "colSGST")
                                TotaldgvGSTR13.Rows[0].Cells["colTSGST"].Value = Utility.DisplayIndianCurrency(dgvGSTR3B1.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colSGST"].Value != null).Sum(x => x.Cells["colSGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colSGST"].Value)).ToString());

                            else if (item == "colCESS")
                                TotaldgvGSTR13.Rows[0].Cells["colTCESS"].Value = Utility.DisplayIndianCurrency(dgvGSTR3B1.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCESS"].Value != null).Sum(x => x.Cells["colCESS"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCESS"].Value)).ToString());
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

        private void dgvGSTR13_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete)
                {
                    #region DELETE SELECTED CELLS
                    try
                    {
                        if (dgvGSTR3B1.Rows.Count > 0)
                        {
                            foreach (DataGridViewCell oneCell in dgvGSTR3B1.SelectedCells)
                            {
                                if (oneCell.ColumnIndex != 1 && (oneCell.ColumnIndex != 4 || oneCell.RowIndex != 1) && (oneCell.ColumnIndex != 5 || oneCell.RowIndex != 1) && (oneCell.ColumnIndex != 3 || oneCell.RowIndex != 2) && (oneCell.ColumnIndex != 4 || oneCell.RowIndex != 2) && (oneCell.ColumnIndex != 5 || oneCell.RowIndex != 2) && (oneCell.ColumnIndex != 6 || oneCell.RowIndex != 2) && (oneCell.ColumnIndex != 3 || oneCell.RowIndex != 4) && (oneCell.ColumnIndex != 4 || oneCell.RowIndex != 4) && (oneCell.ColumnIndex != 5 || oneCell.RowIndex != 4) && (oneCell.ColumnIndex != 6 || oneCell.RowIndex != 4))
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

                    string[] colNo = { "colTotalTaxableValue", "colIGST", "colCGST", "colSGST", "colCESS" };
                    GetTotal(colNo);
                }
                if (e.KeyCode == Keys.V)
                {
                    #region PAST FROM EXCELL SHEET

                    string s = Clipboard.GetText();
                    string[] lines = s.Split('\n');
                    int iRow = 0, iCol = 0;

                    #region PAST ON SELECTED CELLS ONLY
                    if (dgvGSTR3B1.RowCount > 0) // IF GRID IS NOT NULL AND PAST ONLY ON SELECTED CELLS
                    {
                        foreach (DataGridViewCell oneCell in dgvGSTR3B1.SelectedCells)
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
                            if (iRow < dgvGSTR3B1.RowCount && line.Length > 0 && iRow < 12)
                            {
                                string[] sCells = line.Split('\t');

                                for (int i = 0; i < sCells.GetLength(0); ++i)
                                {
                                    if (iCol + i < this.dgvGSTR3B1.ColumnCount && i < 7)
                                    {
                                        if (iCol == 0)
                                            oCell = dgvGSTR3B1[iCol + i + 2, iRow];
                                        else if (iCol == 1)
                                            oCell = dgvGSTR3B1[iCol + i + 1, iRow];
                                        else
                                            oCell = dgvGSTR3B1[iCol + i, iRow];

                                        sCells[i] = sCells[i].Trim().Replace(",", "");
                                        if (oCell.ColumnIndex != 0)
                                        {
                                            if (dgvGSTR3B1.Columns[oCell.ColumnIndex].Name != "colChk" && dgvGSTR3B1.Columns[oCell.ColumnIndex].Name != "colSequence")
                                            {
                                                #region VALIDATION
                                                if (sCells[i].ToString().Trim() == "") { dgvGSTR3B1.Rows[iRow].Cells[oCell.ColumnIndex].Value = DBNull.Value; }
                                                else
                                                {
                                                    if (oCell.ColumnIndex >= 1 && oCell.ColumnIndex <= 8)
                                                    {
                                                        dgvGSTR3B1.Rows[iRow].Cells[oCell.ColumnIndex].Value = sCells[i].Trim();
                                                        dgvGSTR3B1.Rows[1].Cells[4].Value = "";
                                                        dgvGSTR3B1.Rows[1].Cells[5].Value = "";
                                                        dgvGSTR3B1.Rows[2].Cells[3].Value = "";
                                                        dgvGSTR3B1.Rows[4].Cells[3].Value = "";
                                                        dgvGSTR3B1.Rows[2].Cells[4].Value = "";
                                                        dgvGSTR3B1.Rows[4].Cells[4].Value = "";
                                                        dgvGSTR3B1.Rows[4].Cells[5].Value = "";
                                                        dgvGSTR3B1.Rows[2].Cells[6].Value = "";
                                                        dgvGSTR3B1.Rows[2].Cells[5].Value = "";
                                                        dgvGSTR3B1.Rows[4].Cells[5].Value = "";
                                                        dgvGSTR3B1.Rows[4].Cells[6].Value = "";
                                                    }
                                                    else { dgvGSTR3B1.Rows[iRow].Cells[oCell.ColumnIndex].Value = sCells[i].Trim(); }
                                                }
                                                #endregion
                                            }
                                        }
                                        else
                                        {
                                            if (iCol > i)
                                            {
                                                for (int j = oCell.ColumnIndex; j < dgvGSTR3B1.Columns.Count; j++)
                                                {
                                                    #region VALIDATION
                                                    if (sCells[i].ToString().Trim() == "") { dgvGSTR3B1.Rows[iRow].Cells[j].Value = DBNull.Value; }
                                                    else
                                                    {
                                                        if (j >= 1 && j <= 8)
                                                        {
                                                            //if (chkCellValue(sCells[i].Trim(), j))
                                                            dgvGSTR3B1.Rows[iRow].Cells[j].Value = sCells[i].Trim();
                                                            dgvGSTR3B1.Rows[1].Cells[4].Value = "";
                                                            dgvGSTR3B1.Rows[1].Cells[5].Value = "";
                                                            dgvGSTR3B1.Rows[2].Cells[3].Value = "";
                                                            dgvGSTR3B1.Rows[4].Cells[3].Value = "";
                                                            dgvGSTR3B1.Rows[2].Cells[4].Value = "";
                                                            dgvGSTR3B1.Rows[4].Cells[4].Value = "";
                                                            dgvGSTR3B1.Rows[4].Cells[5].Value = "";
                                                            dgvGSTR3B1.Rows[2].Cells[6].Value = "";
                                                            dgvGSTR3B1.Rows[2].Cells[5].Value = "";
                                                            dgvGSTR3B1.Rows[4].Cells[5].Value = "";
                                                            dgvGSTR3B1.Rows[4].Cells[6].Value = "";
                                                            // else
                                                            //   dgvGSTR13.Rows[iRow].Cells[j].Value = DBNull.Value;
                                                        }
                                                        else { dgvGSTR3B1.Rows[iRow].Cells[j].Value = sCells[i].Trim(); }
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
                                                for (int j = oCell.ColumnIndex; j < dgvGSTR3B1.Columns.Count; j++)
                                                {
                                                    #region VALIDATION
                                                    if (sCells[i].ToString().Trim() == "") { dgvGSTR3B1.Rows[iRow].Cells[j].Value = DBNull.Value; }
                                                    else
                                                    {
                                                        if (j >= 1 && j <= 8)
                                                        {
                                                            //if (chkCellValue(sCells[i].Trim(), j))
                                                            dgvGSTR3B1.Rows[iRow].Cells[j].Value = sCells[i].Trim();
                                                            dgvGSTR3B1.Rows[1].Cells[4].Value = "";
                                                            dgvGSTR3B1.Rows[1].Cells[5].Value = "";
                                                            dgvGSTR3B1.Rows[2].Cells[3].Value = "";
                                                            dgvGSTR3B1.Rows[4].Cells[3].Value = "";
                                                            dgvGSTR3B1.Rows[2].Cells[4].Value = "";
                                                            dgvGSTR3B1.Rows[4].Cells[4].Value = "";
                                                            dgvGSTR3B1.Rows[4].Cells[5].Value = "";
                                                            dgvGSTR3B1.Rows[2].Cells[6].Value = "";
                                                            dgvGSTR3B1.Rows[2].Cells[5].Value = "";
                                                            dgvGSTR3B1.Rows[4].Cells[5].Value = "";
                                                            dgvGSTR3B1.Rows[4].Cells[6].Value = "";
                                                            //    else
                                                            //       dgvGSTR13.Rows[iRow].Cells[j].Value = DBNull.Value;
                                                        }
                                                        else { dgvGSTR3B1.Rows[iRow].Cells[j].Value = sCells[i].Trim(); }
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

        public void Save()
        {
            try
            {
                #region CHECK SGST & CGST VALIDATION
                bool isValid = true;
                if (dgvGSTR3B1.Rows.Count > 0)
                {
                    foreach (DataGridViewRow dr in dgvGSTR3B1.Rows)
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
                foreach (DataGridViewColumn col in dgvGSTR3B1.Columns)
                    dt.Columns.Add(col.Name.ToString());

                // add datatable column to store file status
                dt.Columns.Add("colFileStatus");

                #endregion

                #region ASSIGN GRIDVIEW ROWS IN DATATABLE

                // create object array to store one row data of main grid
                object[] rowValue = new object[dt.Columns.Count];

                foreach (DataGridViewRow dr in dgvGSTR3B1.Rows)
                {
                    if (dr.Index != dgvGSTR3B1.Rows.Count) // DON'T ADD LAST ROW
                    {
                        for (int i = 0; i < dr.Cells.Count; i++)
                            rowValue[i] = Convert.ToString(dr.Cells[i].Value);

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
                    Query = "Delete from SPQR3BOutwardSupplies where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                    _Result = objGSTR3B.IUDData(Query);
                    if (_Result != 1)
                    {
                        // error occurs while deleting data
                        MessageBox.Show("System error.\nPlease try after sometime!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    #endregion

                    // query fire to save records to database
                    _Result = objGSTR3B.GSTR3B1BulkEntry(dt, Convert.ToString(CommonHelper.StatusText));

                    if (_Result == 1)
                    {
                        string[] colNo = { "colIntegretdtax", "colCentralTax", "colState", "colCESS" };
                        GetTotal(colNo);

                        #region ADD DATATABLE COLUMN

                        // CREATE DATATABLE TO STORE MAIN GRID DATA
                        dt = new DataTable();

                        // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                        foreach (DataGridViewColumn col in dgvGSTR3B1.Columns)
                            dt.Columns.Add(col.Name.ToString());

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
                                    rowVal[i] = Convert.ToString(dr.Cells[i].Value);

                                // ASSIGN FILE STATUS VALUE WITH EACH GRID ROW
                                rowVal[dr.Cells.Count] = "Total";

                                // ADD ARRAY OF GRID ROW VALUE TO DATATABLE AS ROW
                                dt.Rows.Add(rowVal);
                            }
                        }
                        dt.AcceptChanges();
                        #endregion

                        _Result = objGSTR3B.GSTR3B1BulkEntry(dt, "Total");

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
                    Query = "Delete from SPQR3BOutwardSupplies where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";

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
                    string Query = "Delete from SPQR3BOutwardSupplies where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
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

                        string[] colNo = { "colTotalTaxableValue", "colIGST", "colCGST", "colSGST", "colCESS" };
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

        private void dgvGSTR13_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string cNo = dgvGSTR3B1.Columns[e.ColumnIndex].Name;

                if (e.RowIndex >= 0)
                {
                    if (cNo == "colTotalTaxableValue" || cNo == "colIGST" || cNo == "colCGST" || cNo == "colSGST" || cNo == "colCESS")
                    {
                        if (chkCellValue(Convert.ToString(dgvGSTR3B1.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                        {
                            dgvGSTR3B1.CellValueChanged -= dgvGSTR13_CellValueChanged;
                            dgvGSTR3B1.Rows[e.RowIndex].Cells[cNo].Value = Utility.DisplayIndianCurrency(Convert.ToString(Math.Round(Convert.ToDecimal(dgvGSTR3B1.Rows[e.RowIndex].Cells[cNo].Value), 2, MidpointRounding.AwayFromZero)));
                            dgvGSTR3B1.CellValueChanged += dgvGSTR13_CellValueChanged;

                            string[] colNo = { dgvGSTR3B1.Columns[e.ColumnIndex].Name };
                            GetTotal(colNo);
                        }
                        else
                        {
                            //if (!chkCellValue(Convert.ToString(dgvGSTR13.Rows[e.RowIndex].Cells[cNo].Value).Trim(), cNo))
                            dgvGSTR3B1.Rows[e.RowIndex].Cells[cNo].Value = "";
                        }

                        #region CHECK SGST & CGST VALIDATION
                        decimal cGST = 0;
                        decimal sGST = 0;

                        if (dgvGSTR3B1.Rows[e.RowIndex].Cells["colCGST"].Value != null && Convert.ToString(dgvGSTR3B1.Rows[e.RowIndex].Cells["colCGST"].Value).Trim() != "")
                        {
                            cGST = Convert.ToDecimal(dgvGSTR3B1.Rows[e.RowIndex].Cells["colCGST"].Value);
                        }
                        if (dgvGSTR3B1.Rows[e.RowIndex].Cells["colSGST"].Value != null && Convert.ToString(dgvGSTR3B1.Rows[e.RowIndex].Cells["colSGST"].Value).Trim() != "")
                        {
                            sGST = Convert.ToDecimal(dgvGSTR3B1.Rows[e.RowIndex].Cells["colSGST"].Value);
                        }

                        if (cGST != sGST)
                        {
                            dgvGSTR3B1.Rows[e.RowIndex].Cells["colCGST"].Style.BackColor = Color.Red;
                            dgvGSTR3B1.Rows[e.RowIndex].Cells["colSGST"].Style.BackColor = Color.Red;
                        }
                        else
                        {
                            dgvGSTR3B1.Rows[e.RowIndex].Cells["colCGST"].Style.BackColor = Color.White;
                            dgvGSTR3B1.Rows[e.RowIndex].Cells["colSGST"].Style.BackColor = Color.White;
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
                    if (cNo == "colTotalTaxableValue" || cNo == "colIGST" || cNo == "colCGST" || cNo == "colSGST" || cNo == "colCESS")
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

        public void SetGridViewColor()
        {
            try
            {
                // set main grid property
                this.dgvGSTR3B1.AllowUserToAddRows = false;
                this.dgvGSTR3B1.AutoGenerateColumns = false;

                dgvGSTR3B1.EnableHeadersVisualStyles = false;
                dgvGSTR3B1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(23, 196, 187);
                dgvGSTR3B1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

                this.dgvGSTR3B1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                this.dgvGSTR3B1.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                this.dgvGSTR3B1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

                dgvGSTR3B1.Columns[0].ReadOnly = true;
                dgvGSTR3B1.Columns[0].DefaultCellStyle.BackColor = Color.FromArgb(23, 196, 187);

                foreach (DataGridViewColumn column in dgvGSTR3B1.Columns)
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
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
                        dt = (DataTable)dgvGSTR3B1.DataSource;


                        // check imported template
                        if (dtExcel.Columns.Count != 1)
                        {
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                dt.Columns.Remove("colSrNo");
                                dt.Columns.Remove("colNatureofSupply");
                                dtExcel.Columns.Remove("colNatureofSupply");
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
                                        dgvGSTR3B1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                                        dgvGSTR3B1.RowHeadersVisible = false;

                                        // assign datatale to grid
                                        dgvGSTR3B1.DataSource = dtExcel;
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
                                    dgvGSTR3B1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                                    dgvGSTR3B1.RowHeadersVisible = false;

                                    // assign datatale to grid
                                    dgvGSTR3B1.DataSource = dtExcel;
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
                            string[] colNo = { "colTotalTaxableValue", "colIGST", "colCGST", "colSGST", "colCESS" };
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
                        OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [3B1$]", con);
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
                        for (int i = 0; i < dgvGSTR3B1.Columns.Count; i++)
                        {
                            if (i != 0)
                            {
                                Boolean flg = false;
                                for (int j = 0; j < dtexcel.Columns.Count; j++)
                                {
                                    if (dgvGSTR3B1.Columns[i].HeaderText.Replace(".", "#").Replace(" ", "").ToLower().Trim() == dtexcel.Columns[j].ColumnName.Replace(" ", "").ToLower().Trim())
                                    {
                                        // IF GRID COLUMN PRESENT IN EXCEL THEN ITS INDEX AS PAR GRID COLUMN INDEX
                                        flg = true;
                                        dtexcel.Columns[j].SetOrdinal(dgvGSTR3B1.Columns[i].Index - 1);
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
                        }
                        #endregion

                        #region Remove unused rows from excel
                        //if (dtexcel.Rows.Count > 4)
                        //{
                        //    for (int i = dtexcel.Rows.Count; i > 4; i--)
                        //    {
                        //        dtexcel.Rows[i - 1].Delete();
                        //    }
                        //}
                        dtexcel.AcceptChanges();
                        #endregion

                        #region Remove unused column from excel
                        if (dtexcel.Columns.Count > dgvGSTR3B1.Columns.Count)
                        {
                            for (int i = dtexcel.Columns.Count; i > (dgvGSTR3B1.Columns.Count); i--)
                            {
                                dtexcel.Columns.Remove(dtexcel.Columns[i - 1]);
                            }
                        }
                        dtexcel.AcceptChanges();
                        #endregion

                        #region rename column name as par grid column name
                        foreach (DataGridViewColumn col in dgvGSTR3B1.Columns)
                        {
                            if (col.Index != 0)
                            {
                                dtexcel.Columns[col.Index - 1].ColumnName = col.Name.ToString();
                                col.DataPropertyName = col.Name;
                            }
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
                if (dgvGSTR3B1.Rows.Count > 1)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID

                    // pbGSTR1.Visible = true;

                    #region CREATE WORKBOOK AND ASSIGN COLUMNNAME
                    Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook WB = excelApp.Workbooks.Add(Missing.Value);

                    Microsoft.Office.Interop.Excel.Worksheet newWS = (Microsoft.Office.Interop.Excel.Worksheet)excelApp.Worksheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    newWS.Name = "3B1";

                    // DELETE UNUSED WORKSHEETS FROM WORKBOOK
                    foreach (Microsoft.Office.Interop.Excel.Worksheet ws in WB.Worksheets)
                    {
                        if (ws.Name != "3B1")
                            ((Microsoft.Office.Interop.Excel.Worksheet)WB.Worksheets[ws.Name]).Delete();
                    }

                    // ASSIGN COLUMN HEADER AS PAR THE GRID HEADER
                    for (int i = 1; i < dgvGSTR3B1.Columns.Count; i++)
                    {
                        newWS.Cells[1, i] = dgvGSTR3B1.Columns[i].HeaderText.ToString();

                        // SET COLUMN WIDTH
                        if (i == 1)
                            ((Microsoft.Office.Interop.Excel.Range)newWS.Cells[1, i]).ColumnWidth = 15;
                        else if (i >= 2 && i <= 12)
                            ((Microsoft.Office.Interop.Excel.Range)newWS.Cells[1, i]).ColumnWidth = 10;
                    }

                    // GET RANGE AND SET DIFFRENT PROPERTIES
                    Microsoft.Office.Interop.Excel.Range headerRange = (Microsoft.Office.Interop.Excel.Range)newWS.get_Range((Microsoft.Office.Interop.Excel.Range)newWS.Cells[1, 1], (Microsoft.Office.Interop.Excel.Range)newWS.Cells[1, dgvGSTR3B1.Columns.Count - 1]);
                    headerRange.WrapText = true;
                    headerRange.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                    headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Calibri";
                    #endregion

                    #region COPY DATA FROM DATATABLE TO ARRAY

                    // CREATE ARRAY TO HOLD THE DATA OF DATATABLE
                    object[,] arr = new object[dgvGSTR3B1.Rows.Count, dgvGSTR3B1.Columns.Count];

                    // ASSIGN DATA TO ARRAY FROM DATATABLE
                    if (CommonHelper.IsLicence)
                    {
                        // FOR LICENECE ALLOWS TO EXPORT ALL RECORDS
                        for (int i = 0; i < dgvGSTR3B1.Rows.Count; i++)
                        {
                            for (int j = 1; j < dgvGSTR3B1.Columns.Count; j++)
                            {
                                arr[i, j - 1] = Convert.ToString(dgvGSTR3B1.Rows[i].Cells[j].Value);
                            }
                        }
                    }
                    else
                    {
                        // FOR DEMO ALLOW ONLY 100 RECORDS TO EXPORT
                        for (int i = 0; i < dgvGSTR3B1.Rows.Count; i++)
                        {
                            if (i < 100)
                            {
                                for (int j = 1; j < dgvGSTR3B1.Columns.Count; j++)
                                {
                                    arr[i, j - 1] = Convert.ToString(dgvGSTR3B1.Rows[i].Cells[j].Value);
                                }
                            }
                        }
                    }

                    //SET EXCEL RANGE TO PASTE THE DATA
                    Microsoft.Office.Interop.Excel.Range top = (Microsoft.Office.Interop.Excel.Range)newWS.Cells[2, 1];
                    Microsoft.Office.Interop.Excel.Range bottom = (Microsoft.Office.Interop.Excel.Range)newWS.Cells[dgvGSTR3B1.Rows.Count + 1, dgvGSTR3B1.Columns.Count];
                    Microsoft.Office.Interop.Excel.Range sheetRange = newWS.Range[top, bottom];

                    //FILL ARRAY IN EXCEL
                    sheetRange.Value2 = arr;

                    #endregion

                    //pbGSTR1.Visible = false;

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
                PdfPTable pdfTable = new PdfPTable(dgvGSTR3B1.ColumnCount - 1);
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

                celHeader1 = new PdfPCell(new Phrase("Nature of Supplies", fontHeader));
                celHeader1.Rowspan = 2;
                celHeader1 = SetAllignMent(celHeader1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, new iTextSharp.text.BaseColor(217, 217, 217));
                pdfTable.AddCell(celHeader1);

                celHeader1 = new PdfPCell(new Phrase("Total Taxable Value", fontHeader));
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
                foreach (DataGridViewColumn column in dgvGSTR3B1.Columns)
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
                    foreach (DataGridViewRow row in dgvGSTR3B1.Rows)
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
                    foreach (DataGridViewRow row in dgvGSTR3B1.Rows)
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
                ce1.Colspan = dgvGSTR3B1.Columns.Count - 1;
                ce1.VerticalAlignment = Element.ALIGN_CENTER;
                ce1.HorizontalAlignment = Element.ALIGN_LEFT;
                ce1.BackgroundColor = new iTextSharp.text.BaseColor(197, 223, 197);
                ce1.BorderWidth = 0;
                pdfTable.AddCell(ce1);

                iTextSharp.text.Font FigToRs = FontFactory.GetFont("Calibri", 10, iTextSharp.text.Font.BOLD);
                PdfPCell ceHeader2 = new PdfPCell(new Phrase("(figures in Rs)", FigToRs));
                ceHeader2.Colspan = dgvGSTR3B1.Columns.Count - 1;
                ceHeader2.VerticalAlignment = Element.ALIGN_CENTER;
                ceHeader2.HorizontalAlignment = Element.ALIGN_RIGHT;
                ceHeader2.BorderWidth = 0;
                pdfTable.AddCell(ceHeader2);

                PdfPCell ce2 = new PdfPCell(new Phrase(" "));
                ce2.Colspan = dgvGSTR3B1.Columns.Count - 1;
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

        #region JsonClass

        public class OsupDet
        {
            public double txval { get; set; }
            public double iamt { get; set; }
            public double camt { get; set; }
            public double samt { get; set; }
            public double csamt { get; set; }
        }

        public class OsupZero
        {
            public double txval { get; set; }
            public double iamt { get; set; }
            public double csamt { get; set; }
        }

        public class OsupNilExmp
        {
            public double txval { get; set; }
        }

        public class IsupRev
        {
            public double txval { get; set; }
            public double iamt { get; set; }
            public double camt { get; set; }
            public double samt { get; set; }
            public double csamt { get; set; }
        }

        public class OsupNongst
        {
            public double txval { get; set; }
        }

        public class SupDetails
        {
            public OsupDet osup_det { get; set; }
            public OsupZero osup_zero { get; set; }
            public OsupNilExmp osup_nil_exmp { get; set; }
            public IsupRev isup_rev { get; set; }
            public OsupNongst osup_nongst { get; set; }
        }

        public class UnregDetail
        {
            public string pos { get; set; }
            public double txval { get; set; }
            public double iamt { get; set; }
        }

        public class CompDetail
        {
            public string pos { get; set; }
            public double txval { get; set; }
            public double iamt { get; set; }
        }

        public class UinDetail
        {
            public string pos { get; set; }
            public double txval { get; set; }
            public double iamt { get; set; }
        }

        public class InterSup
        {
            public List<UnregDetail> unreg_details { get; set; }
            public List<CompDetail> comp_details { get; set; }
            public List<UinDetail> uin_details { get; set; }
        }

        public class ItcAvl
        {
            public string ty { get; set; }
            public double iamt { get; set; }
            public double camt { get; set; }
            public double samt { get; set; }
            public double csamt { get; set; }
        }

        public class ItcRev
        {
            public string ty { get; set; }
            public double iamt { get; set; }
            public double camt { get; set; }
            public double samt { get; set; }
            public double csamt { get; set; }
        }

        public class ItcNet
        {
            public double iamt { get; set; }
            public double camt { get; set; }
            public double samt { get; set; }
            public double csamt { get; set; }
        }

        public class ItcInelg
        {
            public string ty { get; set; }
            public double iamt { get; set; }
            public double camt { get; set; }
            public double samt { get; set; }
            public double csamt { get; set; }
        }

        public class ItcElg
        {
            public List<ItcAvl> itc_avl { get; set; }
            public List<ItcRev> itc_rev { get; set; }
            public ItcNet itc_net { get; set; }
            public List<ItcInelg> __invalid_name__itc_inelg { get; set; }
        }

        public class IsupDetail
        {
            public string ty { get; set; }
            public int inter { get; set; }
            public int intra { get; set; }
        }

        public class InwardSup
        {
            public List<IsupDetail> isup_details { get; set; }
        }

        public class IntrDetails
        {
            public double iamt { get; set; }
            public double camt { get; set; }
            public double samt { get; set; }
            public double csamt { get; set; }
        }

        public class IntrLtfee
        {
            public IntrDetails intr_details { get; set; }
        }

        public class RootObject
        {
            public string gstin { get; set; }
            public string ret_period { get; set; }
            public SupDetails sup_details { get; set; }
            public InterSup inter_sup { get; set; }
            public ItcElg itc_elg { get; set; }
            public InwardSup inward_sup { get; set; }
            public IntrLtfee intr_ltfee { get; set; }
        }

        #endregion

        public void JSONCreator()
        {

            #region First Technique With StringBuilder
            //var JSONString = new StringBuilder();
            //DataTable dt = new DataTable();
            //dt = (DataTable)dgvGSTR13.DataSource;

            //dt.Columns[2].ColumnName = "txval";
            //dt.Columns[3].ColumnName = "iamt";
            //dt.Columns[4].ColumnName = "camt";
            //dt.Columns[5].ColumnName = "samt";
            //dt.Columns[6].ColumnName = "csamt";

            //if (dt.Rows.Count > 0)
            //{
            //    //JSONString.Append("[");
            //    JSONString.Append("{");
            //    JSONString.Append("\"gstin\":" + "\"" + CommonHelper.CompanyGSTN + "\",");
            //    JSONString.Append("\"ret_period\":" + "\"" + CommonHelper.SelectedYear + "\",");

            //    JSONString.Append("\"sup_details" + "\": {");
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {

            //        if (i == 0)
            //        {
            //            JSONString.Append("\"osup_det" + "\" : {");
            //        }
            //        if (i == 1)
            //        {
            //            JSONString.Append("\"osup_zero" + "\" : {");
            //        }
            //        if (i == 2)
            //        {
            //            JSONString.Append("\"osup_nil_exmp" + "\" : {");
            //        }
            //        if (i == 3)
            //        {
            //            JSONString.Append("\"isup_rev" + "\" : {");
            //        }
            //        if (i == 4)
            //        {
            //            JSONString.Append("\"osup_nongst" + "\" : {");
            //        }

            //        for (int j = 2; j < dt.Columns.Count; j++)
            //        {

            //            if (j != dt.Columns.Count - 1)
            //                JSONString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\",");
            //            else
            //                JSONString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\"");
            //        }
            //        if (i != dt.Rows.Count - 1)
            //            JSONString.Append("},");
            //        else
            //            JSONString.Append("}");
            //    }
            //    JSONString.Append("}}");
            //    // JSONString.Append("]");

            //    #region File Save
            //    JavaScriptSerializer objScript = new JavaScriptSerializer();
            //    objScript.MaxJsonLength = 2147483647;
            //    string FinalJson = objScript.Serialize(JSONString);
            //    SaveFileDialog save = new SaveFileDialog();
            //    save.FileName = "3B1.json";
            //    save.Filter = "Json File | *.json";
            //    if (save.ShowDialog() == DialogResult.OK)
            //    {
            //        StreamWriter writer = new StreamWriter(save.OpenFile());
            //        writer.WriteLine(JSONString);
            //        writer.Dispose();
            //        writer.Close();
            //        MessageBox.Show("Save Json.");
            //    }
            //    GetData();
            //    #endregion
            //}
            #endregion

            #region Second With Object GSTR3B1
            RootObject ObjJson = new RootObject();
            ObjJson.gstin = CommonHelper.CompanyGSTN;
            ObjJson.ret_period = CommonHelper.GetReturnPeriod();

            List<DataGridViewRow> Invoicelist = dgvGSTR3B1.Rows
                      .OfType<DataGridViewRow>()
                      .ToList();

            //List<SupDetails> sup_details = new List<SupDetails>();
            SupDetails objSupdetais = new SupDetails();

            try
            {
                for (int i = 0; i < Invoicelist.Count; i++)
                {
                    if (i == 0)
                    {
                        OsupDet objosupDet = new OsupDet();
                        objosupDet.txval = Convert.ToDouble(Invoicelist[i].Cells["colTotalTaxableValue"].Value);
                        objosupDet.iamt = Convert.ToDouble(Invoicelist[i].Cells["colIGST"].Value);
                        objosupDet.camt = Convert.ToDouble(Invoicelist[i].Cells["colCGST"].Value);
                        objosupDet.samt = Convert.ToDouble(Invoicelist[i].Cells["colSGST"].Value);
                        objosupDet.csamt = Convert.ToDouble(Invoicelist[i].Cells["colCESS"].Value);
                        objSupdetais.osup_det = objosupDet;
                    }
                    if (i == 1)
                    {
                        OsupZero objosupDet = new OsupZero();
                        objosupDet.txval = Convert.ToDouble(Invoicelist[i].Cells["colTotalTaxableValue"].Value.ToString());
                        objosupDet.iamt = Convert.ToDouble(Invoicelist[i].Cells["colIGST"].Value);
                        objosupDet.csamt = Convert.ToDouble(Invoicelist[i].Cells["colCESS"].Value);
                        objSupdetais.osup_zero = objosupDet;
                    }
                    if (i == 2)
                    {
                        OsupNilExmp objosupDet = new OsupNilExmp();
                        objosupDet.txval = Convert.ToDouble(Invoicelist[i].Cells["colTotalTaxableValue"].Value.ToString());

                        objSupdetais.osup_nil_exmp = objosupDet;
                    }
                    if (i == 3)
                    {
                        IsupRev objosupDet = new IsupRev();
                        objosupDet.txval = Convert.ToDouble(Invoicelist[i].Cells["colTotalTaxableValue"].Value.ToString());
                        objosupDet.iamt = Convert.ToDouble(Invoicelist[i].Cells["colIGST"].Value);
                        objosupDet.camt = Convert.ToDouble(Invoicelist[i].Cells["colCGST"].Value);
                        objosupDet.samt = Convert.ToDouble(Invoicelist[i].Cells["colSGST"].Value);
                        objosupDet.csamt = Convert.ToDouble(Invoicelist[i].Cells["colCESS"].Value);
                        objSupdetais.isup_rev = objosupDet;
                    }
                    if (i == 4)
                    {
                        OsupNongst objosupDet = new OsupNongst();
                        objosupDet.txval = Convert.ToDouble(Invoicelist[i].Cells["colTotalTaxableValue"].Value.ToString());

                        objSupdetais.osup_nongst = objosupDet;
                    }
                }

                ObjJson.sup_details = objSupdetais;

                #region File Save
                JavaScriptSerializer objScript = new JavaScriptSerializer();

                var settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.DefaultValueHandling = DefaultValueHandling.Ignore;

                objScript.MaxJsonLength = 2147483647;

                string FinalJson = JsonConvert.SerializeObject(ObjJson, settings);

                SaveFileDialog save = new SaveFileDialog();
                save.FileName = "3B1.json";
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
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
            #endregion
        }

        #endregion

        #region disable/enable controls

        private void DisableControls(Control con)
        {
            foreach (Control c in con.Controls)
            {
                if (c.Name != "dgvGSTR13" && c.Name != "TotaldgvGSTR13")
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

        private void dgvGSTR3B1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                if (dgvGSTR3B1.Rows.Count > 0)
                {
                    dgvGSTR3B1.Rows[1].Cells["colCGST"].ReadOnly = true;
                    dgvGSTR3B1.Rows[1].Cells["colCGST"].Style.BackColor = Color.Gray;

                    dgvGSTR3B1.Rows[1].Cells["colSGST"].ReadOnly = true;
                    dgvGSTR3B1.Rows[1].Cells["colSGST"].Style.BackColor = Color.Gray;

                    dgvGSTR3B1.Rows[2].Cells["colIGST"].ReadOnly = true;
                    dgvGSTR3B1.Rows[2].Cells["colIGST"].Style.BackColor = Color.Gray;

                    dgvGSTR3B1.Rows[2].Cells["colCESS"].ReadOnly = true;
                    dgvGSTR3B1.Rows[2].Cells["colCESS"].Style.BackColor = Color.Gray;

                    dgvGSTR3B1.Rows[2].Cells["colCGST"].ReadOnly = true;
                    dgvGSTR3B1.Rows[2].Cells["colCGST"].Style.BackColor = Color.Gray;

                    dgvGSTR3B1.Rows[2].Cells["colSGST"].ReadOnly = true;
                    dgvGSTR3B1.Rows[2].Cells["colSGST"].Style.BackColor = Color.Gray;

                    dgvGSTR3B1.Rows[4].Cells["colIGST"].ReadOnly = true;
                    dgvGSTR3B1.Rows[4].Cells["colIGST"].Style.BackColor = Color.Gray;

                    dgvGSTR3B1.Rows[4].Cells["colCESS"].ReadOnly = true;
                    dgvGSTR3B1.Rows[4].Cells["colCESS"].Style.BackColor = Color.Gray;

                    dgvGSTR3B1.Rows[4].Cells["colCGST"].ReadOnly = true;
                    dgvGSTR3B1.Rows[4].Cells["colCGST"].Style.BackColor = Color.Gray;

                    dgvGSTR3B1.Rows[4].Cells["colSGST"].ReadOnly = true;
                    dgvGSTR3B1.Rows[4].Cells["colSGST"].Style.BackColor = Color.Gray;

                    this.dgvGSTR3B1.ClearSelection();
                    this.TotaldgvGSTR13.ClearSelection();
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

        private void TotaldgvGSTR13_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.dgvGSTR3B1.ClearSelection();
            this.TotaldgvGSTR13.ClearSelection();
        }

        private void frmGSTR3B1_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName("");
        }

        private void btnGSTR1_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();

                // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
                foreach (DataGridViewColumn col in dgvGSTR3B1.Columns)
                {
                    dt.Columns.Add(col.Name.ToString());
                    col.DataPropertyName = col.Name;
                }
                dt.AcceptChanges();

                DataRow dr = dt.NewRow();
                dr["colSrNo"] = "1";
                dr["colNatureofSupply"] = "(a) Outward Taxable Supplies (other than Zero rated,nil rated and exemted)";
                dr["colTotalTaxableValue"] = "";
                dr["colIGST"] = "";
                dr["colCGST"] = "";
                dr["colSGST"] = "";
                dr["colCESS"] = "";
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["colSrNo"] = "2";
                dr["colNatureofSupply"] = "(b) Outward Taxable Supplies (Zero rated)";
                dr["colTotalTaxableValue"] = "";
                dr["colIGST"] = "";
                dr["colCGST"] = "";
                dr["colSGST"] = "";
                dr["colCESS"] = "";
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["colSrNo"] = "3";
                dr["colNatureofSupply"] = "(c) Other outward Supplies(Nil rated, exemted)";
                dr["colTotalTaxableValue"] = "";
                dr["colIGST"] = "";
                dr["colCGST"] = "";
                dr["colSGST"] = "";
                dr["colCESS"] = "";
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["colSrNo"] = "4";
                dr["colNatureofSupply"] = "(d) Inward Supplies(liable to reverse charge)";
                dr["colTotalTaxableValue"] = "";
                dr["colIGST"] = "";
                dr["colCGST"] = "";
                dr["colSGST"] = "";
                dr["colCESS"] = "";
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["colSrNo"] = "5";
                dr["colNatureofSupply"] = "(e) Non-GST outward supplies";
                dr["colTotalTaxableValue"] = "";
                dr["colIGST"] = "";
                dr["colCGST"] = "";
                dr["colSGST"] = "";
                dr["colCESS"] = "";
                dt.Rows.Add(dr);

                #region FETCH DATA FROM GSTR1
                DataTable dtGSTR1 = new DataTable();
                dtGSTR1 = GetGSTR1data();

                if (dtGSTR1 != null && dtGSTR1.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        decimal? TaxVal = 0;
                        decimal? IGST = 0;
                        decimal? CGST = 0;
                        decimal? SGST = 0;
                        decimal? CESS = 0;

                        if (Convert.ToString(dt.Rows[i]["colSrNo"]) == "1")
                        {
                            #region Outward taxable Supplies(Other Than Zero Rated, Nil Rated & Exempted
                            for (int j = 0; j < dtGSTR1.Rows.Count; j++)
                            {
                                string Type = Convert.ToString(dtGSTR1.Rows[j]["Type of Invoices"]);
                                if (Type != string.Empty)
                                    Type = Type.ToUpper();

                                if (Type == "B2B" || Type == "B2CL" || Type == "B2CS" || Type == "AR" || Type == "CDN" || Type == "CDNUR")
                                {
                                    if (dtGSTR1.Rows[j]["Taxable Value"] != null && Convert.ToString(dtGSTR1.Rows[j]["Taxable Value"]) != "")
                                        TaxVal = TaxVal + Convert.ToDecimal(dtGSTR1.Rows[j]["Taxable Value"]);
                                    if (dtGSTR1.Rows[j]["IGST"] != null && Convert.ToString(dtGSTR1.Rows[j]["IGST"]) != "")
                                        IGST = IGST + Convert.ToDecimal(dtGSTR1.Rows[j]["IGST"]);
                                    if (dtGSTR1.Rows[j]["CGST"] != null && Convert.ToString(dtGSTR1.Rows[j]["CGST"]) != "")
                                        CGST = CGST + Convert.ToDecimal(dtGSTR1.Rows[j]["CGST"]);
                                    if (dtGSTR1.Rows[j]["SGST"] != null && Convert.ToString(dtGSTR1.Rows[j]["SGST"]) != "")
                                        SGST = SGST + Convert.ToDecimal(dtGSTR1.Rows[j]["SGST"]);
                                    if (dtGSTR1.Rows[j]["Cess"] != null && Convert.ToString(dtGSTR1.Rows[j]["Cess"]) != "")
                                        CESS = CESS + Convert.ToDecimal(dtGSTR1.Rows[j]["Cess"]);
                                }
                                else if (Type == "AA" || Type == "CDNCREREF" || Type == "CDNURCREREF")
                                {
                                    if (dtGSTR1.Rows[j]["Taxable Value"] != null && Convert.ToString(dtGSTR1.Rows[j]["Taxable Value"]) != "")
                                        TaxVal = TaxVal - Convert.ToDecimal(dtGSTR1.Rows[j]["Taxable Value"]);
                                    if (dtGSTR1.Rows[j]["IGST"] != null && Convert.ToString(dtGSTR1.Rows[j]["IGST"]) != "")
                                        IGST = IGST - Convert.ToDecimal(dtGSTR1.Rows[j]["IGST"]);
                                    if (dtGSTR1.Rows[j]["CGST"] != null && Convert.ToString(dtGSTR1.Rows[j]["CGST"]) != "")
                                        CGST = CGST - Convert.ToDecimal(dtGSTR1.Rows[j]["CGST"]);
                                    if (dtGSTR1.Rows[j]["SGST"] != null && Convert.ToString(dtGSTR1.Rows[j]["SGST"]) != "")
                                        SGST = SGST - Convert.ToDecimal(dtGSTR1.Rows[j]["SGST"]);
                                    if (dtGSTR1.Rows[j]["Cess"] != null && Convert.ToString(dtGSTR1.Rows[j]["Cess"]) != "")
                                        CESS = CESS - Convert.ToDecimal(dtGSTR1.Rows[j]["Cess"]);
                                }
                            }

                            dt.Rows[i]["colTotalTaxableValue"] = TaxVal;
                            dt.Rows[i]["colIGST"] = IGST;
                            dt.Rows[i]["colCGST"] = CGST;
                            dt.Rows[i]["colSGST"] = SGST;
                            dt.Rows[i]["colCESS"] = CESS;
                            #endregion
                        }
                        else if (Convert.ToString(dt.Rows[i]["colSrNo"]) == "2")
                        {
                            #region Outward taxable Supplies(Zero Rated)
                            for (int j = 0; j < dtGSTR1.Rows.Count; j++)
                            {
                                string Type = Convert.ToString(dtGSTR1.Rows[j]["Type of Invoices"]);
                                if (Type != string.Empty)
                                    Type = Type.ToUpper();

                                if (Type == "ZRS" || Type == "B2BNOTREG")
                                {
                                    if (dtGSTR1.Rows[j]["Taxable Value"] != null && Convert.ToString(dtGSTR1.Rows[j]["Taxable Value"]) != "")
                                        TaxVal = TaxVal + Convert.ToDecimal(dtGSTR1.Rows[j]["Taxable Value"]);
                                    if (dtGSTR1.Rows[j]["IGST"] != null && Convert.ToString(dtGSTR1.Rows[j]["IGST"]) != "")
                                        IGST = IGST + Convert.ToDecimal(dtGSTR1.Rows[j]["IGST"]);
                                    //if (dtGSTR1.Rows[j]["CGST"] != null && Convert.ToString(dtGSTR1.Rows[j]["CGST"]) != "")
                                    //    CGST = CGST + Convert.ToDecimal(dtGSTR1.Rows[j]["CGST"]);
                                    //if (dtGSTR1.Rows[j]["SGST"] != null && Convert.ToString(dtGSTR1.Rows[j]["SGST"]) != "")
                                    //    SGST = SGST + Convert.ToDecimal(dtGSTR1.Rows[j]["SGST"]);
                                    if (dtGSTR1.Rows[j]["Cess"] != null && Convert.ToString(dtGSTR1.Rows[j]["Cess"]) != "")
                                        CESS = CESS + Convert.ToDecimal(dtGSTR1.Rows[j]["Cess"]);
                                }
                            }

                            dt.Rows[i]["colTotalTaxableValue"] = TaxVal;
                            dt.Rows[i]["colIGST"] = IGST;
                            //dt.Rows[i]["colCGST"] = CGST;
                            //dt.Rows[i]["colSGST"] = SGST;
                            dt.Rows[i]["colCESS"] = CESS;
                            #endregion
                        }
                        else if (Convert.ToString(dt.Rows[i]["colSrNo"]) == "3")
                        {
                            #region Other Outward Supplied(Nil Rated, Exempted)
                            for (int j = 0; j < dtGSTR1.Rows.Count; j++)
                            {
                                string Type = Convert.ToString(dtGSTR1.Rows[j]["Type of Invoices"]);
                                if (Type != string.Empty)
                                    Type = Type.ToUpper();

                                if (Type == "NILRATED")
                                {
                                    if (dtGSTR1.Rows[j]["Taxable Value"] != null && Convert.ToString(dtGSTR1.Rows[j]["Taxable Value"]) != "")
                                        TaxVal = TaxVal + Convert.ToDecimal(dtGSTR1.Rows[j]["Taxable Value"]);
                                    //if (dtGSTR1.Rows[j]["IGST"] != null && Convert.ToString(dtGSTR1.Rows[j]["IGST"]) != "")
                                    //    IGST = IGST + Convert.ToDecimal(dtGSTR1.Rows[j]["IGST"]);
                                    //if (dtGSTR1.Rows[j]["CGST"] != null && Convert.ToString(dtGSTR1.Rows[j]["CGST"]) != "")
                                    //    CGST = CGST + Convert.ToDecimal(dtGSTR1.Rows[j]["CGST"]);
                                    //if (dtGSTR1.Rows[j]["SGST"] != null && Convert.ToString(dtGSTR1.Rows[j]["SGST"]) != "")
                                    //    SGST = SGST + Convert.ToDecimal(dtGSTR1.Rows[j]["SGST"]);
                                    //if (dtGSTR1.Rows[j]["Cess"] != null && Convert.ToString(dtGSTR1.Rows[j]["Cess"]) != "")
                                    //    CESS = CESS + Convert.ToDecimal(dtGSTR1.Rows[j]["Cess"]);
                                }
                            }

                            dt.Rows[i]["colTotalTaxableValue"] = TaxVal;
                            //dt.Rows[i]["colIGST"] = IGST;
                            //dt.Rows[i]["colCGST"] = CGST;
                            //dt.Rows[i]["colSGST"] = SGST;
                            //dt.Rows[i]["colCESS"] = CESS;
                            #endregion
                        }
                        else if (Convert.ToString(dt.Rows[i]["colSrNo"]) == "5")
                        {
                            #region Other Outward Supplied(Nil Rated, Exempted)
                            for (int j = 0; j < dtGSTR1.Rows.Count; j++)
                            {
                                string Type = Convert.ToString(dtGSTR1.Rows[j]["Type of Invoices"]);
                                if (Type != string.Empty)
                                    Type = Type.ToUpper();

                                if (Type == "NILRATEDNONGST")
                                {
                                    if (dtGSTR1.Rows[j]["Taxable Value"] != null && Convert.ToString(dtGSTR1.Rows[j]["Taxable Value"]) != "")
                                        TaxVal = TaxVal + Convert.ToDecimal(dtGSTR1.Rows[j]["Taxable Value"]);
                                }
                            }

                            dt.Rows[i]["colTotalTaxableValue"] = TaxVal;
                            #endregion
                        }
                    }
                }
                #endregion

                // assign datatable to main grid
                this.dgvGSTR3B1.ClearSelection();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        string ColName = dt.Columns[j].ColumnName;
                        if (ColName == "colTotalTaxableValue" || ColName == "colIGST" || ColName == "colCGST" || ColName == "colSGST" || ColName == "colCess")
                            dt.Rows[i][j] = Utility.DisplayIndianCurrency(Convert.ToString(dt.Rows[i][j]));
                    }
                }

                dgvGSTR3B1.DataSource = dt;

                dgvGSTR3B1.Columns["colNatureofSupply"].ReadOnly = true;
                DataGridViewRow row = this.dgvGSTR3B1.RowTemplate;
                row.MinimumHeight = 30;
                for (int i = 0; i < dgvGSTR3B1.Rows.Count; i++)
                {
                    for (int j = 3; j < dgvGSTR3B1.ColumnCount; j++)
                    {
                        if (dgvGSTR3B1.Rows[i].Cells[j].Value.ToString() == "-" || dgvGSTR3B1.Rows[i].Cells[j].Value.ToString() == "" || dgvGSTR3B1.Rows[i].Cells[j].Value == null)
                        {
                            dgvGSTR3B1.Rows[i].Cells[j].Value = "";
                        }
                    }
                }

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
                DataRow dr_Total = dtTotal.NewRow();
                dr_Total["colTTotalTaxableValue"] = dgvGSTR3B1.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colTotalTaxableValue"].Value != null).Sum(x => x.Cells["colTotalTaxableValue"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colTotalTaxableValue"].Value)).ToString();
                dr_Total["colTIGST"] = dgvGSTR3B1.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colIGST"].Value != null).Sum(x => x.Cells["colIGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colIGST"].Value)).ToString();
                dr_Total["colTCGST"] = dgvGSTR3B1.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCGST"].Value != null).Sum(x => x.Cells["colCGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCGST"].Value)).ToString();
                dr_Total["colTSGST"] = dgvGSTR3B1.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colSGST"].Value != null).Sum(x => x.Cells["colSGST"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colSGST"].Value)).ToString();
                dr_Total["colTCESS"] = dgvGSTR3B1.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["colCESS"].Value != null).Sum(x => x.Cells["colCESS"].Value.ToString().Trim() == "" ? 0 : Convert.ToDecimal(x.Cells["colCESS"].Value)).ToString();

                // add datarow to datatable
                dtTotal.Rows.Add(dr_Total);

                for (int i = 0; i < dtTotal.Rows.Count; i++)
                {
                    for (int j = 0; j < dtTotal.Columns.Count; j++)
                    {
                        string ColName = dtTotal.Columns[j].ColumnName;
                        if (ColName == "colTTotalTaxableValue" || ColName == "colTIGST" || ColName == "colTCGST" || ColName == "colTSGST" || ColName == "colTCESS")
                            dtTotal.Rows[i][j] = Utility.DisplayIndianCurrency(Convert.ToString(dtTotal.Rows[i][j]));
                    }
                }

                dtTotal.AcceptChanges();

                // assign datatable to grid
                TotaldgvGSTR13.DataSource = dtTotal;
                #endregion

                MessageBox.Show("Get Data From GSTR1 Succesfully!");
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
    }
}
