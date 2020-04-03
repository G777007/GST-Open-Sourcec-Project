using SPEQTAGST.BAL;
using SPEQTAGST.BAL.M363r1a;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Web.Script.Serialization;
using SPEQTAGST.BAL.M796r3b;
using System.Data.OleDb;
using SPEQTAGST.BAL.M112t;
using SPEQTAGST.Usermain;
using SPQ.Helper;
using SPEQTAGST.BAL.V019js;
using System.Net;
using SPEQTAGST.xasjbr1;
using SPQ.Automation;
using System.Threading;
namespace SPEQTAGST.rintlcs3b
{
    public partial class SPQGSTR3BMain : Form
    {
        r1aPublicclass objGSTR1A = new r1aPublicclass();
        r3bPublicclass objGSTR3B = new r3bPublicclass();

        private CookieContainer Cc = new CookieContainer();
        private HttpWebResponse response;
        private Stream responseStream;
        private StreamReader responseStreamReader;

        private string DashboardPage
        {
            get
            {
                return "https://services.gst.gov.in/services/auth/fowelcome";
            }
            set
            {
            }
        }
        public string getError
        {
            get;
            set;
        }
        private string GstLoginPage
        {
            get
            {
                return "https://services.gst.gov.in/services/login";
            }
            set
            {
            }
        }
        public string saveTransIdVal
        {
            get;
            set;
        }

        public SPQGSTR3BMain()
        {
            InitializeComponent();
            Getdata();
            GetDataFileOption();
            GetReportData();
            GetFilingStatusMsg();
            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName(CommonHelper.ReturnName);

            DgvMain.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            DgvMain.EnableHeadersVisualStyles = false;
            DgvMain.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);
            DgvMain.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DgvMain.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            dgvReport.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dgvReport.EnableHeadersVisualStyles = false;
            dgvReport.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);
            dgvReport.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvReport.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

        }

        public void Getdata()
        {
            try
            {
                DataTable dt1 = new DataTable();
                DataTable dt = new DataTable();

                dt1.Columns.Add("SectionName", typeof(string));
                // dt1.Columns.Add("Validation Status", typeof(string));
                dt1.Columns.Add("Status", typeof(string));
                dt1.Columns.Add("NOofInv", typeof(string));
                // dt1.Columns.Add("InvValue", typeof(string));
                dt1.Columns.Add("InvTaxVal", typeof(string));
                dt1.Columns.Add("IGST", typeof(string));
                dt1.Columns.Add("CGST", typeof(string));
                dt1.Columns.Add("SGST", typeof(string));
                dt1.Columns.Add("Cess", typeof(string));

                #region Form 1
                string Query = "Select * from SPQR3BOutwardSupplies where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2";
                dt = new DataTable();
                dt = objGSTR1A.GetDataGSTR1A(Query);

                if (dt != null && dt.Rows.Count == 2)
                {
                    //dt1.Rows.Add("Details of Outward Supplies and inward Supplies liable to Reverse Charge", dt.Rows[1]["Fld_FileStatus"].ToString().Trim(), "0", dt.Rows[0]["Fld_TotalTaxableValue"].ToString().Trim(), dt.Rows[0]["Fld_IGST"].ToString().Trim(), dt.Rows[0]["Fld_CGST"].ToString().Trim(), dt.Rows[0]["Fld_SGST"].ToString().Trim(), dt.Rows[0]["Fld_CESS"].ToString().Trim());
                    dt1.Rows.Add("Details of Outward Supplies and inward Supplies liable to Reverse Charge", "Completed", "0", dt.Rows[0]["Fld_TotalTaxableValue"].ToString().Trim(), dt.Rows[0]["Fld_IGST"].ToString().Trim(), dt.Rows[0]["Fld_CGST"].ToString().Trim(), dt.Rows[0]["Fld_SGST"].ToString().Trim(), dt.Rows[0]["Fld_CESS"].ToString().Trim());
                }
                else
                {
                    dt1.Rows.Add("Details of Outward Supplies and inward Supplies liable to Reverse Charge", "-", "0", "0", "0", "0", "0", "0");
                }
                #endregion

                #region Form 2
                Query = "Select * from SPQR3BInterStateSupplies where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2";
                dt = new DataTable();
                dt = objGSTR1A.GetDataGSTR1A(Query);

                if (dt != null && dt.Rows.Count == 2)
                {
                    //dt1.Rows.Add("Details of Inter-State Supplies made to unregistered persons, composition taxable persons and UIN holders", dt.Rows[1]["Fld_FileStatus"].ToString().Trim(), "0", dt.Rows[0]["Fld_Taxable"].ToString().Trim(), dt.Rows[0]["Fld_IGST"].ToString().Trim(), "0", "0", "0");
                    dt1.Rows.Add("Details of Inter-State Supplies made to unregistered persons, composition taxable persons and UIN holders", "Completed", "0", dt.Rows[0]["Fld_Taxable"].ToString().Trim(), dt.Rows[0]["Fld_IGST"].ToString().Trim(), "0", "0", "0");
                }
                else
                {
                    dt1.Rows.Add("Details of Inter-State Supplies made to unregistered persons, composition taxable persons and UIN holders", "-", "0", "0", "0", "0", "0", "0");
                }
                #endregion

                #region Form 4
                Query = "Select * from SPQR3BEligibleITC where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                dt = new DataTable();
                dt = objGSTR1A.GetDataGSTR1A(Query);

                if (dt != null && dt.Rows.Count >= 0)
                {
                    //dt1.Rows.Add("Eligible ITC", dt.Rows[0]["Fld_FileStatus"].ToString().Trim(), "0", "0", dt.Rows[11]["Fld_IGST"].ToString().Trim(), dt.Rows[11]["Fld_CGST"].ToString().Trim(), dt.Rows[11]["Fld_SGST"].ToString().Trim(), dt.Rows[11]["Fld_CESS"].ToString().Trim());
                    dt1.Rows.Add("Eligible ITC", "Completed", "0", "0", dt.Rows[11]["Fld_IGST"].ToString().Trim(), dt.Rows[11]["Fld_CGST"].ToString().Trim(), dt.Rows[11]["Fld_SGST"].ToString().Trim(), dt.Rows[11]["Fld_CESS"].ToString().Trim());
                }
                else
                {
                    dt1.Rows.Add("Eligible ITC", "-", "0", "0", "0", "0", "0", "0");
                }
                #endregion

                #region Form 5
                Query = "Select * from SPQR3BExemptSupply where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                dt = new DataTable();
                dt = objGSTR1A.GetDataGSTR1A(Query);

                if (dt != null && dt.Rows.Count >= 0)
                {
                    //dt1.Rows.Add("Values of exempt, nil-rated and non-GST inward supplies", dt.Rows[0]["Fld_FileStatus"].ToString().Trim(), "0", "0", "0", "0", "0", "0");
                    dt1.Rows.Add("Values of exempt, nil-rated and non-GST inward supplies", "Completed", "0", "0", "0", "0", "0", "0");
                }
                else
                {
                    dt1.Rows.Add("Values of exempt, nil-rated and non-GST inward supplies", "-", "0", "0", "0", "0", "0", "0");
                }
                #endregion

                #region Form 6
                Query = "Select * from SPQR3BTaxPayment where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus = 'Total'";
                dt = new DataTable();
                dt = objGSTR1A.GetDataGSTR1A(Query);

                if (dt != null && dt.Rows.Count == 1)
                {
                    //dt1.Rows.Add("Payment of Tax", dt.Rows[1]["Fld_FileStatus"].ToString().Trim(), "0", "0", dt.Rows[1]["Fld_IntegratedTax"].ToString().Trim(), dt.Rows[0]["Fld_CentarlTax"].ToString().Trim(), dt.Rows[0]["Fld_StateUTTax"].ToString().Trim(), dt.Rows[0]["Fld_Cess"].ToString().Trim());
                    dt1.Rows.Add("Payment of Tax", "Completed", "0", dt.Rows[0]["Fld_OtRcTaxPay"].ToString().Trim(), dt.Rows[0]["Fld_IGST"].ToString().Trim(), dt.Rows[0]["Fld_CGST"].ToString().Trim(), dt.Rows[0]["Fld_SGST"].ToString().Trim(), dt.Rows[0]["Fld_CESS"].ToString().Trim());
                }
                else
                {
                    dt1.Rows.Add("Payment of Tax", "-", "0", "0", "0", "0", "0", "0");
                }
                #endregion

                dt1.Columns["SectionName"].ColumnName = "SectionName";
                dt1.Columns["Status"].ColumnName = "Status";
                dt1.Columns["NOofInv"].ColumnName = "Document Count";
                dt1.Columns["InvTaxVal"].ColumnName = "Taxable Value";
                dt1.Columns["IGST"].ColumnName = "IGST";
                dt1.Columns["CGST"].ColumnName = "CGST";
                dt1.Columns["SGST"].ColumnName = "SGST";
                dt1.Columns["Cess"].ColumnName = "Cess";
                DgvMain.DataSource = dt1;
                DgvMain.Columns["SectionName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                DgvMain.Columns["Status"].Width = 140;
                //DgvMain.Columns["Status"].Visible = false;
                DataGridViewRow row = this.DgvMain.RowTemplate;
                row.MinimumHeight = 25;

                foreach (DataGridViewColumn column in DgvMain.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                DgvMain.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(232, 241, 252);
                DgvMain.EnableHeadersVisualStyles = false;
                for (int i = 0; i < DgvMain.Rows.Count; i++)
                {
                    for (int j = 3; j < DgvMain.ColumnCount; j++)
                    {
                        if (DgvMain.Rows[i].Cells[j].Value == "-" || DgvMain.Rows[i].Cells[j].Value == "" || DgvMain.Rows[i].Cells[j].Value == null)
                        {
                            DgvMain.Rows[i].Cells[j].Value = "0";
                        }
                    }
                }

                #region Add Total and Account and Diffrent
                string NOofInv = dt1.Rows.Cast<DataRow>().Where(x => x["Document Count"] != null).Sum(x => x["Document Count"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Document Count"])).ToString().Trim();
                string InvTaxVal = dt1.Rows.Cast<DataRow>().Where(x => x["Taxable Value"] != null).Sum(x => x["Taxable Value"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Taxable Value"])).ToString().Trim();
                string IGST = dt1.Rows.Cast<DataRow>().Where(x => Convert.ToString(x["IGST"]).Trim() != "").Sum(x => x["IGST"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["IGST"])).ToString().Trim();
                string CGST = dt1.Rows.Cast<DataRow>().Where(x => Convert.ToString(x["CGST"]).Trim() != "").Sum(x => x["CGST"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["CGST"])).ToString().Trim();
                string SGST = dt1.Rows.Cast<DataRow>().Where(x => Convert.ToString(x["SGST"]).Trim() != "").Sum(x => x["SGST"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["SGST"])).ToString().Trim();
                string Cess = dt1.Rows.Cast<DataRow>().Where(x => Convert.ToString(x["Cess"]).Trim() != "").Sum(x => x["Cess"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Cess"])).ToString().Trim();

                if (dgvtotal.Columns.Count == 0)
                {
                    foreach (DataGridViewColumn dgvc in DgvMain.Columns)
                    {
                        dgvtotal.Columns.Add(dgvc.Clone() as DataGridViewColumn);
                    }
                }
                dgvtotal.Rows.Add("Total", "", "", "", "", "", "", "");
                dgvtotal.Rows[0].Cells["Document Count"].Value = NOofInv.ToString().Trim();
                dgvtotal.Rows[0].Cells["Taxable Value"].Value = InvTaxVal.ToString().Trim();
                dgvtotal.Rows[0].Cells["IGST"].Value = IGST.ToString().Trim();
                dgvtotal.Rows[0].Cells["CGST"].Value = CGST.ToString().Trim();
                dgvtotal.Rows[0].Cells["SGST"].Value = SGST.ToString().Trim();
                dgvtotal.Rows[0].Cells["Cess"].Value = Cess.ToString().Trim();
                dgvtotal.Rows[0].Cells["SectionName"].Value = "Total";

                DgvMain.Refresh();
                Application.DoEvents();

                DataTable dtAcc = new DataTable();
                if (dgvaccount.Columns.Count == 0)
                {
                    foreach (DataGridViewColumn dgvc in DgvMain.Columns)
                    {
                        dgvaccount.Columns.Add(dgvc.Clone() as DataGridViewColumn);
                    }
                }
                dtAcc = dt1.Clone();
                dtAcc.Rows.Add("As per Account", "", "", "", "", "", "", "");
                dgvaccount.DataSource = dtAcc;
                dgvaccount.Columns[0].ReadOnly = true;
                dgvaccount.Columns[1].ReadOnly = true;

                if (dgvdiff.Columns.Count == 0)
                {
                    foreach (DataGridViewColumn dgvc in DgvMain.Columns)
                    {
                        dgvdiff.Columns.Add(dgvc.Clone() as DataGridViewColumn);
                    }
                }
                DataTable dtDiff = new DataTable();
                dtDiff = dt1.Clone();
                dtDiff.Rows.Add("Difference", "", "", "", "", "", "", "");
                dgvdiff.DataSource = dtDiff;
                dgvdiff.ReadOnly = true;
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

        public void GetReportData()
        {
            try
            {
                decimal Fld_ExportTurn_3B = 0, Fld_IGSTTurn_3B = 0, Fld_TaxTurn_3B = 0, Fld_CESS_3B = 0, Fld_IGST_3B = 0, Fld_CGST_3B = 0, Fld_SGST_3B = 0;
                decimal Fld_ExportTurn_1 = 0, Fld_IGSTTurn_1 = 0, Fld_TaxTurn_1 = 0, Fld_CESS_1 = 0, Fld_IGST_1 = 0, Fld_CGST_1 = 0, Fld_SGST_1 = 0;
                string Query = "";
                DataTable dt = new DataTable();

                foreach (DataGridViewColumn column in dgvReport.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                dgvReport.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(232, 241, 252);
                dgvReport.EnableHeadersVisualStyles = false;

                #region IsQuarter = True
                if (CommonHelper.IsQuarter == true)
                {
                    if ("June" == CommonHelper.SelectedMonth || "September" == CommonHelper.SelectedMonth || "December" == CommonHelper.SelectedMonth || "March" == CommonHelper.SelectedMonth)
                    {
                        lblReportTitle.Visible = true;
                        dgvReport.Visible = true;

                        #region GSTR-3B
                        if (CommonHelper.IsQuarter == true)
                        {
                            string Month = SetQuarterlyMonth(CommonHelper.SelectedMonth);
                            string[] Monthstr = Month.Split(',');

                            for (int i = 0; i < Monthstr.Length; i++)
                            {
                                Query = "Select * from SPQR3BOutwardSupplies where Fld_Month='" + Monthstr[i].Trim() + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                                dt = new DataTable();
                                dt = objGSTR1A.GetDataGSTR1A(Query);
                                if (dt != null)
                                {
                                    if (dt.Rows.Count > 0)
                                    {
                                        if (dt.Rows[1][3].ToString().Trim() != "")
                                            Fld_ExportTurn_3B = Convert.ToDecimal(dt.Rows[1][3]);
                                        if (dt.Rows[1][4].ToString().Trim() != "")
                                            Fld_IGSTTurn_3B = Convert.ToDecimal(dt.Rows[1][4]);

                                        if (dt.Rows[0][3].ToString().Trim() != "")
                                            Fld_TaxTurn_3B = Fld_TaxTurn_3B + Convert.ToDecimal(dt.Rows[0][3]);
                                        if (dt.Rows[2][3].ToString().Trim() != "")
                                            Fld_TaxTurn_3B = Fld_TaxTurn_3B + Convert.ToDecimal(dt.Rows[2][3]);
                                        if (dt.Rows[4][3].ToString().Trim() != "")
                                            Fld_TaxTurn_3B = Fld_TaxTurn_3B + Convert.ToDecimal(dt.Rows[4][3]);

                                        if (dt.Rows[0][7].ToString().Trim() != "")
                                            Fld_CESS_3B = Fld_CESS_3B + Convert.ToDecimal(dt.Rows[0][7]);
                                        if (dt.Rows[1][7].ToString().Trim() != "")
                                            Fld_CESS_3B = Fld_CESS_3B + Convert.ToDecimal(dt.Rows[1][7]);
                                        //if (dt.Rows[3][7].ToString().Trim() != "")
                                        //    Fld_CESS_3B = Fld_CESS_3B + Convert.ToDecimal(dt.Rows[3][7]);

                                        if (dt.Rows[0][4].ToString().Trim() != "")
                                            Fld_IGST_3B = Fld_IGST_3B + Convert.ToDecimal(dt.Rows[0][4]);
                                        //if (dt.Rows[3][4].ToString().Trim() != "")
                                        //    Fld_IGST_3B = Fld_IGST_3B + Convert.ToDecimal(dt.Rows[3][4]);

                                        if (dt.Rows[0][5].ToString().Trim() != "")
                                            Fld_CGST_3B = Fld_CGST_3B + Convert.ToDecimal(dt.Rows[0][5]);
                                        //if (dt.Rows[3][5].ToString().Trim() != "")
                                        //    Fld_CGST_3B = Fld_CGST_3B + Convert.ToDecimal(dt.Rows[3][5]);

                                        if (dt.Rows[0][6].ToString().Trim() != "")
                                            Fld_SGST_3B = Fld_SGST_3B + Convert.ToDecimal(dt.Rows[0][6]);
                                        //if (dt.Rows[3][6].ToString().Trim() != "")
                                        //    Fld_SGST_3B = Fld_SGST_3B + Convert.ToDecimal(dt.Rows[3][6]);
                                    }
                                }
                            }
                        }

                        dgvReport.Rows.Add("GSTR-3B", Fld_TaxTurn_3B, Fld_IGST_3B, Fld_CGST_3B, Fld_SGST_3B, Fld_CESS_3B, Fld_ExportTurn_3B, Fld_IGSTTurn_3B);
                        #endregion

                        #region GSTR-1
                        Query = "Select * from SPQR1B2B where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                        dt = new DataTable();
                        dt = objGSTR1A.GetDataGSTR1A(Query);

                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0]["Fld_InvoiceTaxableVal"].ToString().Trim() != "")
                                    Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[0]["Fld_InvoiceTaxableVal"]);

                                if (dt.Rows[0]["Fld_IGSTAmnt"].ToString().Trim() != "")
                                    Fld_IGST_1 = Fld_IGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_IGSTAmnt"]);
                                if (dt.Rows[0]["Fld_CGSTAmnt"].ToString().Trim() != "")
                                    Fld_CGST_1 = Fld_CGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_CGSTAmnt"]);
                                if (dt.Rows[0]["Fld_SGSTAmnt"].ToString().Trim() != "")
                                    Fld_SGST_1 = Fld_SGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_SGSTAmnt"]);
                                if (dt.Rows[0]["Fld_CessAmount"].ToString().Trim() != "")
                                    Fld_CESS_1 = Fld_CESS_1 + Convert.ToDecimal(dt.Rows[0]["Fld_CessAmount"]);
                            }
                        }

                        Query = "Select * from SPQR1B2CL where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                        dt = new DataTable();
                        dt = objGSTR1A.GetDataGSTR1A(Query);

                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0]["Fld_TaxableValue"].ToString().Trim() != "")
                                    Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[0]["Fld_TaxableValue"]);

                                if (dt.Rows[0]["Fld_IGST"].ToString().Trim() != "")
                                    Fld_IGST_1 = Fld_IGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_IGST"]);
                                //if (dt.Rows[0]["Fld_CGST"].ToString().Trim() != "")
                                //    Fld_CGST_1 = Fld_CGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_CGST"]);
                                //if (dt.Rows[0]["Fld_SGST"].ToString().Trim() != "")
                                //    Fld_SGST_1 = Fld_SGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_SGST"]);
                                if (dt.Rows[0]["Fld_CESS"].ToString().Trim() != "")
                                    Fld_CESS_1 = Fld_CESS_1 + Convert.ToDecimal(dt.Rows[0]["Fld_CESS"]);
                            }
                        }

                        Query = "Select * from SPQR1B2CS where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                        dt = new DataTable();
                        dt = objGSTR1A.GetDataGSTR1A(Query);

                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0]["Fld_TaxableValue"].ToString().Trim() != "")
                                    Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[0]["Fld_TaxableValue"]);

                                if (dt.Rows[0]["Fld_IGST"].ToString().Trim() != "")
                                    Fld_IGST_1 = Fld_IGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_IGST"]);
                                if (dt.Rows[0]["Fld_CGST"].ToString().Trim() != "")
                                    Fld_CGST_1 = Fld_CGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_CGST"]);
                                if (dt.Rows[0]["Fld_SGST"].ToString().Trim() != "")
                                    Fld_SGST_1 = Fld_SGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_SGST"]);
                                if (dt.Rows[0]["Fld_CESS"].ToString().Trim() != "")
                                    Fld_CESS_1 = Fld_CESS_1 + Convert.ToDecimal(dt.Rows[0]["Fld_CESS"]);
                            }
                        }

                        Query = "Select * from SPQR1CDN where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                        dt = new DataTable();
                        dt = objGSTR1A.GetDataGSTR1A(Query);

                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0]["Fld_Taxable"].ToString().Trim() != "")
                                    Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[0]["Fld_Taxable"]);

                                if (dt.Rows[0]["Fld_IGSTAmnt"].ToString().Trim() != "")
                                    Fld_IGST_1 = Fld_IGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_IGSTAmnt"]);
                                if (dt.Rows[0]["Fld_CGSTAmnt"].ToString().Trim() != "")
                                    Fld_CGST_1 = Fld_CGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_CGSTAmnt"]);
                                if (dt.Rows[0]["Fld_SGSTAmnt"].ToString().Trim() != "")
                                    Fld_SGST_1 = Fld_SGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_SGSTAmnt"]);
                                if (dt.Rows[0]["Fld_CessAmnt"].ToString().Trim() != "")
                                    Fld_CESS_1 = Fld_CESS_1 + Convert.ToDecimal(dt.Rows[0]["Fld_CessAmnt"]);
                            }
                        }

                        Query = "Select * from SPQR1CDNUR where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                        dt = new DataTable();
                        dt = objGSTR1A.GetDataGSTR1A(Query);

                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0]["Fld_Taxable"].ToString().Trim() != "")
                                    Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[0]["Fld_Taxable"]);

                                if (dt.Rows[0]["Fld_IGSTAmnt"].ToString().Trim() != "")
                                    Fld_IGST_1 = Fld_IGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_IGSTAmnt"]);
                                if (dt.Rows[0]["Fld_CGSTAmnt"].ToString().Trim() != "")
                                    Fld_CGST_1 = Fld_CGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_CGSTAmnt"]);
                                if (dt.Rows[0]["Fld_SGSTAmnt"].ToString().Trim() != "")
                                    Fld_SGST_1 = Fld_SGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_SGSTAmnt"]);
                                if (dt.Rows[0]["Fld_CessAmnt"].ToString().Trim() != "")
                                    Fld_CESS_1 = Fld_CESS_1 + Convert.ToDecimal(dt.Rows[0]["Fld_CessAmnt"]);
                            }
                        }

                        Query = "Select * from SPQR1GrossAdvance where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                        dt = new DataTable();
                        dt = objGSTR1A.GetDataGSTR1A(Query);

                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0]["Fld_GrossAdvRcv"].ToString().Trim() != "")
                                    Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[0]["Fld_GrossAdvRcv"]);

                                if (dt.Rows[0]["Fld_IGSTAmnt"].ToString().Trim() != "")
                                    Fld_IGST_1 = Fld_IGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_IGSTAmnt"]);
                                if (dt.Rows[0]["Fld_CGSTAmnt"].ToString().Trim() != "")
                                    Fld_CGST_1 = Fld_CGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_CGSTAmnt"]);
                                if (dt.Rows[0]["Fld_SGSTAmnt"].ToString().Trim() != "")
                                    Fld_SGST_1 = Fld_SGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_SGSTAmnt"]);
                                if (dt.Rows[0]["Fld_CessAmount"].ToString().Trim() != "")
                                    Fld_CESS_1 = Fld_CESS_1 + Convert.ToDecimal(dt.Rows[0]["Fld_CessAmount"]);
                            }
                        }

                        Query = "Select * from SPQR1NetAdvance where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                        dt = new DataTable();
                        dt = objGSTR1A.GetDataGSTR1A(Query);

                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0]["Fld_Advadj"].ToString().Trim() != "")
                                    Fld_TaxTurn_1 = Fld_TaxTurn_1 - Convert.ToDecimal(dt.Rows[0]["Fld_Advadj"]);

                                if (dt.Rows[0]["Fld_IGSTAmnt"].ToString().Trim() != "")
                                    Fld_IGST_1 = Fld_IGST_1 - Convert.ToDecimal(dt.Rows[0]["Fld_IGSTAmnt"]);
                                if (dt.Rows[0]["Fld_CGSTAmnt"].ToString().Trim() != "")
                                    Fld_CGST_1 = Fld_CGST_1 - Convert.ToDecimal(dt.Rows[0]["Fld_CGSTAmnt"]);
                                if (dt.Rows[0]["Fld_SGSTAmnt"].ToString().Trim() != "")
                                    Fld_SGST_1 = Fld_SGST_1 - Convert.ToDecimal(dt.Rows[0]["Fld_SGSTAmnt"]);
                                if (dt.Rows[0]["Fld_CessAmount"].ToString().Trim() != "")
                                    Fld_CESS_1 = Fld_CESS_1 - Convert.ToDecimal(dt.Rows[0]["Fld_CessAmount"]);
                            }
                        }

                        Query = "Select * from SPQR1ZeroRated where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                        dt = new DataTable();
                        dt = objGSTR1A.GetDataGSTR1A(Query);

                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0]["Fld_IGSTInvoiceTaxableVal"].ToString().Trim() != "")
                                    Fld_ExportTurn_1 = Fld_ExportTurn_1 + Convert.ToDecimal(dt.Rows[0]["Fld_IGSTInvoiceTaxableVal"]);

                                if (dt.Rows[0]["Fld_IGSTAmnt"].ToString().Trim() != "")
                                    Fld_IGSTTurn_1 = Fld_IGSTTurn_1 + Convert.ToDecimal(dt.Rows[0]["Fld_IGSTAmnt"]);

                            }
                        }

                        Query = "Select * from SPQR1NilRated where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                        dt = new DataTable();
                        dt = objGSTR1A.GetDataGSTR1A(Query);

                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0]["Fld_NilRatedSupply"].ToString().Trim() != "")
                                    Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[0]["Fld_NilRatedSupply"]);
                                if (dt.Rows[0]["Fld_Exempted"].ToString().Trim() != "")
                                    Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[0]["Fld_Exempted"]);
                                if (dt.Rows[0]["Fld_NonGSTSupplies"].ToString().Trim() != "")
                                    Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[0]["Fld_NonGSTSupplies"]);

                                if (dt.Rows[1]["Fld_NilRatedSupply"].ToString().Trim() != "")
                                    Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[1]["Fld_NilRatedSupply"]);
                                if (dt.Rows[1]["Fld_Exempted"].ToString().Trim() != "")
                                    Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[1]["Fld_Exempted"]);
                                if (dt.Rows[1]["Fld_NonGSTSupplies"].ToString().Trim() != "")
                                    Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[1]["Fld_NonGSTSupplies"]);

                                if (dt.Rows[2]["Fld_NilRatedSupply"].ToString().Trim() != "")
                                    Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[2]["Fld_NilRatedSupply"]);
                                if (dt.Rows[2]["Fld_Exempted"].ToString().Trim() != "")
                                    Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[2]["Fld_Exempted"]);
                                if (dt.Rows[2]["Fld_NonGSTSupplies"].ToString().Trim() != "")
                                    Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[2]["Fld_NonGSTSupplies"]);

                                if (dt.Rows[3]["Fld_NilRatedSupply"].ToString().Trim() != "")
                                    Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[3]["Fld_NilRatedSupply"]);
                                if (dt.Rows[3]["Fld_Exempted"].ToString().Trim() != "")
                                    Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[3]["Fld_Exempted"]);
                                if (dt.Rows[3]["Fld_NonGSTSupplies"].ToString().Trim() != "")
                                    Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[3]["Fld_NonGSTSupplies"]);

                            }
                        }

                        dgvReport.Rows.Add("GSTR-1", Fld_TaxTurn_1, Fld_IGST_1, Fld_CGST_1, Fld_SGST_1, Fld_CESS_1, Fld_ExportTurn_1, Fld_IGSTTurn_1);
                        #endregion

                    }
                    else
                    {
                        lblReportTitle.Visible = false;
                        dgvReport.Visible = false;
                    }
                }
                #endregion

                #region IsQuarter = False
                if (CommonHelper.IsQuarter == false)
                {
                    lblReportTitle.Visible = true;
                    dgvReport.Visible = true;

                    #region GSTR-3B

                    Query = "Select * from SPQR3BOutwardSupplies where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                    dt = new DataTable();
                    dt = objGSTR1A.GetDataGSTR1A(Query);
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[1][3].ToString().Trim() != "")
                                Fld_ExportTurn_3B = Convert.ToDecimal(dt.Rows[1][3]);
                            if (dt.Rows[1][4].ToString().Trim() != "")
                                Fld_IGSTTurn_3B = Convert.ToDecimal(dt.Rows[1][4]);

                            if (dt.Rows[0][3].ToString().Trim() != "")
                                Fld_TaxTurn_3B = Fld_TaxTurn_3B + Convert.ToDecimal(dt.Rows[0][3]);
                            if (dt.Rows[2][3].ToString().Trim() != "")
                                Fld_TaxTurn_3B = Fld_TaxTurn_3B + Convert.ToDecimal(dt.Rows[2][3]);
                            if (dt.Rows[4][3].ToString().Trim() != "")
                                Fld_TaxTurn_3B = Fld_TaxTurn_3B + Convert.ToDecimal(dt.Rows[4][3]);

                            if (dt.Rows[0][7].ToString().Trim() != "")
                                Fld_CESS_3B = Fld_CESS_3B + Convert.ToDecimal(dt.Rows[0][7]);
                            if (dt.Rows[1][7].ToString().Trim() != "")
                                Fld_CESS_3B = Fld_CESS_3B + Convert.ToDecimal(dt.Rows[1][7]);
                            //if (dt.Rows[3][7].ToString().Trim() != "")
                            //    Fld_CESS_3B = Fld_CESS_3B + Convert.ToDecimal(dt.Rows[3][7]);

                            if (dt.Rows[0][4].ToString().Trim() != "")
                                Fld_IGST_3B = Fld_IGST_3B + Convert.ToDecimal(dt.Rows[0][4]);
                            //if (dt.Rows[3][4].ToString().Trim() != "")
                            //    Fld_IGST_3B = Fld_IGST_3B + Convert.ToDecimal(dt.Rows[3][4]);

                            if (dt.Rows[0][5].ToString().Trim() != "")
                                Fld_CGST_3B = Fld_CGST_3B + Convert.ToDecimal(dt.Rows[0][5]);
                            //if (dt.Rows[3][5].ToString().Trim() != "")
                            //    Fld_CGST_3B = Fld_CGST_3B + Convert.ToDecimal(dt.Rows[3][5]);

                            if (dt.Rows[0][6].ToString().Trim() != "")
                                Fld_SGST_3B = Fld_SGST_3B + Convert.ToDecimal(dt.Rows[0][6]);
                            //if (dt.Rows[3][6].ToString().Trim() != "")
                            //    Fld_SGST_3B = Fld_SGST_3B + Convert.ToDecimal(dt.Rows[3][6]);
                        }
                    }

                    dgvReport.Rows.Add("GSTR-3B", Fld_TaxTurn_3B, Fld_IGST_3B, Fld_CGST_3B, Fld_SGST_3B, Fld_CESS_3B, Fld_ExportTurn_3B, Fld_IGSTTurn_3B);
                    #endregion

                    #region GSTR-1
                    Query = "Select * from SPQR1B2B where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                    dt = new DataTable();
                    dt = objGSTR1A.GetDataGSTR1A(Query);

                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["Fld_InvoiceTaxableVal"].ToString().Trim() != "")
                                Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[0]["Fld_InvoiceTaxableVal"]);

                            if (dt.Rows[0]["Fld_IGSTAmnt"].ToString().Trim() != "")
                                Fld_IGST_1 = Fld_IGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_IGSTAmnt"]);
                            if (dt.Rows[0]["Fld_CGSTAmnt"].ToString().Trim() != "")
                                Fld_CGST_1 = Fld_CGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_CGSTAmnt"]);
                            if (dt.Rows[0]["Fld_SGSTAmnt"].ToString().Trim() != "")
                                Fld_SGST_1 = Fld_SGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_SGSTAmnt"]);
                            if (dt.Rows[0]["Fld_CessAmount"].ToString().Trim() != "")
                                Fld_CESS_1 = Fld_CESS_1 + Convert.ToDecimal(dt.Rows[0]["Fld_CessAmount"]);
                        }
                    }

                    Query = "Select * from SPQR1B2CL where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                    dt = new DataTable();
                    dt = objGSTR1A.GetDataGSTR1A(Query);

                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["Fld_TaxableValue"].ToString().Trim() != "")
                                Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[0]["Fld_TaxableValue"]);

                            if (dt.Rows[0]["Fld_IGST"].ToString().Trim() != "")
                                Fld_IGST_1 = Fld_IGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_IGST"]);
                            //if (dt.Rows[0]["Fld_CGST"].ToString().Trim() != "")
                            //    Fld_CGST_1 = Fld_CGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_CGST"]);
                            //if (dt.Rows[0]["Fld_SGST"].ToString().Trim() != "")
                            //    Fld_SGST_1 = Fld_SGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_SGST"]);
                            if (dt.Rows[0]["Fld_CESS"].ToString().Trim() != "")
                                Fld_CESS_1 = Fld_CESS_1 + Convert.ToDecimal(dt.Rows[0]["Fld_CESS"]);
                        }
                    }

                    Query = "Select * from SPQR1B2CS where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                    dt = new DataTable();
                    dt = objGSTR1A.GetDataGSTR1A(Query);

                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["Fld_TaxableValue"].ToString().Trim() != "")
                                Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[0]["Fld_TaxableValue"]);

                            if (dt.Rows[0]["Fld_IGST"].ToString().Trim() != "")
                                Fld_IGST_1 = Fld_IGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_IGST"]);
                            if (dt.Rows[0]["Fld_CGST"].ToString().Trim() != "")
                                Fld_CGST_1 = Fld_CGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_CGST"]);
                            if (dt.Rows[0]["Fld_SGST"].ToString().Trim() != "")
                                Fld_SGST_1 = Fld_SGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_SGST"]);
                            if (dt.Rows[0]["Fld_CESS"].ToString().Trim() != "")
                                Fld_CESS_1 = Fld_CESS_1 + Convert.ToDecimal(dt.Rows[0]["Fld_CESS"]);
                        }
                    }

                    Query = "Select * from SPQR1CDN where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                    dt = new DataTable();
                    dt = objGSTR1A.GetDataGSTR1A(Query);

                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["Fld_Taxable"].ToString().Trim() != "")
                                Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[0]["Fld_Taxable"]);

                            if (dt.Rows[0]["Fld_IGSTAmnt"].ToString().Trim() != "")
                                Fld_IGST_1 = Fld_IGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_IGSTAmnt"]);
                            if (dt.Rows[0]["Fld_CGSTAmnt"].ToString().Trim() != "")
                                Fld_CGST_1 = Fld_CGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_CGSTAmnt"]);
                            if (dt.Rows[0]["Fld_SGSTAmnt"].ToString().Trim() != "")
                                Fld_SGST_1 = Fld_SGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_SGSTAmnt"]);
                            if (dt.Rows[0]["Fld_CessAmnt"].ToString().Trim() != "")
                                Fld_CESS_1 = Fld_CESS_1 + Convert.ToDecimal(dt.Rows[0]["Fld_CessAmnt"]);
                        }
                    }

                    Query = "Select * from SPQR1CDNUR where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                    dt = new DataTable();
                    dt = objGSTR1A.GetDataGSTR1A(Query);

                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["Fld_Taxable"].ToString().Trim() != "")
                                Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[0]["Fld_Taxable"]);

                            if (dt.Rows[0]["Fld_IGSTAmnt"].ToString().Trim() != "")
                                Fld_IGST_1 = Fld_IGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_IGSTAmnt"]);
                            if (dt.Rows[0]["Fld_CGSTAmnt"].ToString().Trim() != "")
                                Fld_CGST_1 = Fld_CGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_CGSTAmnt"]);
                            if (dt.Rows[0]["Fld_SGSTAmnt"].ToString().Trim() != "")
                                Fld_SGST_1 = Fld_SGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_SGSTAmnt"]);
                            if (dt.Rows[0]["Fld_CessAmnt"].ToString().Trim() != "")
                                Fld_CESS_1 = Fld_CESS_1 + Convert.ToDecimal(dt.Rows[0]["Fld_CessAmnt"]);
                        }
                    }

                    Query = "Select * from SPQR1GrossAdvance where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                    dt = new DataTable();
                    dt = objGSTR1A.GetDataGSTR1A(Query);

                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["Fld_GrossAdvRcv"].ToString().Trim() != "")
                                Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[0]["Fld_GrossAdvRcv"]);

                            if (dt.Rows[0]["Fld_IGSTAmnt"].ToString().Trim() != "")
                                Fld_IGST_1 = Fld_IGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_IGSTAmnt"]);
                            if (dt.Rows[0]["Fld_CGSTAmnt"].ToString().Trim() != "")
                                Fld_CGST_1 = Fld_CGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_CGSTAmnt"]);
                            if (dt.Rows[0]["Fld_SGSTAmnt"].ToString().Trim() != "")
                                Fld_SGST_1 = Fld_SGST_1 + Convert.ToDecimal(dt.Rows[0]["Fld_SGSTAmnt"]);
                            if (dt.Rows[0]["Fld_CessAmount"].ToString().Trim() != "")
                                Fld_CESS_1 = Fld_CESS_1 + Convert.ToDecimal(dt.Rows[0]["Fld_CessAmount"]);
                        }
                    }

                    Query = "Select * from SPQR1NetAdvance where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                    dt = new DataTable();
                    dt = objGSTR1A.GetDataGSTR1A(Query);

                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["Fld_Advadj"].ToString().Trim() != "")
                                Fld_TaxTurn_1 = Fld_TaxTurn_1 - Convert.ToDecimal(dt.Rows[0]["Fld_Advadj"]);

                            if (dt.Rows[0]["Fld_IGSTAmnt"].ToString().Trim() != "")
                                Fld_IGST_1 = Fld_IGST_1 - Convert.ToDecimal(dt.Rows[0]["Fld_IGSTAmnt"]);
                            if (dt.Rows[0]["Fld_CGSTAmnt"].ToString().Trim() != "")
                                Fld_CGST_1 = Fld_CGST_1 - Convert.ToDecimal(dt.Rows[0]["Fld_CGSTAmnt"]);
                            if (dt.Rows[0]["Fld_SGSTAmnt"].ToString().Trim() != "")
                                Fld_SGST_1 = Fld_SGST_1 - Convert.ToDecimal(dt.Rows[0]["Fld_SGSTAmnt"]);
                            if (dt.Rows[0]["Fld_CessAmount"].ToString().Trim() != "")
                                Fld_CESS_1 = Fld_CESS_1 - Convert.ToDecimal(dt.Rows[0]["Fld_CessAmount"]);
                        }
                    }

                    Query = "Select * from SPQR1ZeroRated where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                    dt = new DataTable();
                    dt = objGSTR1A.GetDataGSTR1A(Query);

                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["Fld_IGSTInvoiceTaxableVal"].ToString().Trim() != "")
                                Fld_ExportTurn_1 = Fld_ExportTurn_1 + Convert.ToDecimal(dt.Rows[0]["Fld_IGSTInvoiceTaxableVal"]);

                            if (dt.Rows[0]["Fld_IGSTAmnt"].ToString().Trim() != "")
                                Fld_IGSTTurn_1 = Fld_IGSTTurn_1 + Convert.ToDecimal(dt.Rows[0]["Fld_IGSTAmnt"]);

                        }
                    }

                    Query = "Select * from SPQR1NilRated where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                    dt = new DataTable();
                    dt = objGSTR1A.GetDataGSTR1A(Query);

                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["Fld_NilRatedSupply"].ToString().Trim() != "")
                                Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[0]["Fld_NilRatedSupply"]);
                            if (dt.Rows[0]["Fld_Exempted"].ToString().Trim() != "")
                                Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[0]["Fld_Exempted"]);
                            if (dt.Rows[0]["Fld_NonGSTSupplies"].ToString().Trim() != "")
                                Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[0]["Fld_NonGSTSupplies"]);

                            if (dt.Rows[1]["Fld_NilRatedSupply"].ToString().Trim() != "")
                                Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[1]["Fld_NilRatedSupply"]);
                            if (dt.Rows[1]["Fld_Exempted"].ToString().Trim() != "")
                                Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[1]["Fld_Exempted"]);
                            if (dt.Rows[1]["Fld_NonGSTSupplies"].ToString().Trim() != "")
                                Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[1]["Fld_NonGSTSupplies"]);

                            if (dt.Rows[2]["Fld_NilRatedSupply"].ToString().Trim() != "")
                                Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[2]["Fld_NilRatedSupply"]);
                            if (dt.Rows[2]["Fld_Exempted"].ToString().Trim() != "")
                                Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[2]["Fld_Exempted"]);
                            if (dt.Rows[2]["Fld_NonGSTSupplies"].ToString().Trim() != "")
                                Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[2]["Fld_NonGSTSupplies"]);

                            if (dt.Rows[3]["Fld_NilRatedSupply"].ToString().Trim() != "")
                                Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[3]["Fld_NilRatedSupply"]);
                            if (dt.Rows[3]["Fld_Exempted"].ToString().Trim() != "")
                                Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[3]["Fld_Exempted"]);
                            if (dt.Rows[3]["Fld_NonGSTSupplies"].ToString().Trim() != "")
                                Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[3]["Fld_NonGSTSupplies"]);

                        }
                    }

                    dgvReport.Rows.Add("GSTR-1", Fld_TaxTurn_1, Fld_IGST_1, Fld_CGST_1, Fld_SGST_1, Fld_CESS_1, Fld_ExportTurn_1, Fld_IGSTTurn_1);
                    #endregion

                }
                #endregion

                if (dgvReport.Rows.Count == 2)
                {
                    dgvReport.Rows.Add("Difference", Fld_TaxTurn_3B - Fld_TaxTurn_1, Fld_IGST_3B - Fld_IGST_1, Fld_CGST_3B - Fld_CGST_1, Fld_SGST_3B - Fld_SGST_1, Fld_CESS_3B - Fld_CESS_1, Fld_ExportTurn_3B - Fld_ExportTurn_1, Fld_IGSTTurn_3B - Fld_IGSTTurn_1);
                }

                if (dgvReport.RowCount > 0)
                {
                    dgvReport.Rows[1].Cells[0].Style.ForeColor = Color.Blue;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void DgvMain_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CommonHelper.IsMainFormType = "3B";
            if (e.RowIndex == 0 && e.ColumnIndex == 0)
            {
                SPQGSTR3B1 obj = new SPQGSTR3B1();
                obj.MdiParent = this.MdiParent;
                Utility.CloseAllOpenForm();
                obj.Dock = DockStyle.Fill;
                obj.Show();

                ((SPQMDI)Application.OpenForms["SPQMDI"]).ShowExtraToolsMenu();
                ((SPQMDI)Application.OpenForms["SPQMDI"]).ShowCompanyDetailMenu();
            }
            else if (e.RowIndex == 1 && e.ColumnIndex == 0)
            {
                SPQGSTR3B2 obj = new SPQGSTR3B2();
                obj.MdiParent = this.MdiParent;
                Utility.CloseAllOpenForm();
                obj.Dock = DockStyle.Fill;
                obj.Show();

                ((SPQMDI)Application.OpenForms["SPQMDI"]).ShowExtraToolsMenu();
                ((SPQMDI)Application.OpenForms["SPQMDI"]).ShowCompanyDetailMenu();
            }
            else if (e.RowIndex == 2 && e.ColumnIndex == 0)
            {
                SPQGSTR3B4 obj = new SPQGSTR3B4();
                obj.MdiParent = this.MdiParent;
                Utility.CloseAllOpenForm();
                obj.Dock = DockStyle.Fill;
                obj.Show();

                ((SPQMDI)Application.OpenForms["SPQMDI"]).ShowExtraToolsMenu();
                ((SPQMDI)Application.OpenForms["SPQMDI"]).ShowCompanyDetailMenu();
            }
            else if (e.RowIndex == 3 && e.ColumnIndex == 0)
            {
                SPQGSTR3B5 obj = new SPQGSTR3B5();
                obj.MdiParent = this.MdiParent;
                Utility.CloseAllOpenForm();
                obj.Dock = DockStyle.Fill;
                 
                obj.Show();

                ((SPQMDI)Application.OpenForms["SPQMDI"]).ShowExtraToolsMenu();
                ((SPQMDI)Application.OpenForms["SPQMDI"]).ShowCompanyDetailMenu();
            }
            else if (e.RowIndex == 4 && e.ColumnIndex == 0)
            {
                SPQGSTR3B6 obj = new SPQGSTR3B6();
                obj.MdiParent = this.MdiParent;
                Utility.CloseAllOpenForm();
                obj.Dock = DockStyle.Fill;
                //obj.Dock = DockStyle.Fill;
                // 
                obj.Show();

                ((SPQMDI)Application.OpenForms["SPQMDI"]).ShowExtraToolsMenu();
                ((SPQMDI)Application.OpenForms["SPQMDI"]).ShowCompanyDetailMenu();
            }
            else
            {
            }
        }

        public string SetQuarterlyMonth(string Month)
        {
            try
            {
                string FinalString = "";

                if (Month == "April" || Month == "May" || Month == "June")
                    FinalString = "April,May,June";
                else if (Month == "July" || Month == "August" || Month == "September")
                    FinalString = "July,August,September";
                else if (Month == "October" || Month == "November" || Month == "December")
                    FinalString = "October,November,December";
                else if (Month == "January" || Month == "February" || Month == "March")
                    FinalString = "January,February,March";

                return FinalString;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        private void msClearAll_Click(object sender, EventArgs e)
        {
            try
            {
                 DialogResult result = MessageBox.Show("Are You Sure to Delete all the Records...?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    #region first delete old data from database
                    int _Result = 0;
                    string Query = "", _str = "";

                    _Result = 0;
                    Query = "Delete from SPQR3BOutwardSupplies where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                    _Result = objGSTR3B.IUDData(Query);
                    if (_Result != 1)
                    { _str += "Data delete error - Form1..!\n"; }

                    _Result = 0;
                    Query = "Delete from SPQR3BInterStateSupplies where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                    _Result = objGSTR3B.IUDData(Query);
                    if (_Result != 1)
                    { _str += "Data delete error - Form2..!\n"; }

                    _Result = 0;
                    Query = "Delete from SPQR3BEligibleITC where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                    _Result = objGSTR3B.IUDData(Query);
                    if (_Result != 1)
                    { _str += "Data delete error - Form4..!\n"; }

                    _Result = 0;
                    Query = "Delete from SPQR3BExemptSupply where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                    _Result = objGSTR3B.IUDData(Query);
                    if (_Result != 1)
                    { _str += "Data delete error - Form5..!\n"; }

                    Query = "Delete from SPQR3BTaxPayment where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                    _Result = objGSTR3B.IUDData(Query);
                    if (_Result != 1)
                    { _str += "Data delete error - Form6..!\n"; }

                    dgvtotal.Rows[0].Cells["Document Count"].Value = 0;
                    dgvtotal.Rows[0].Cells["Taxable Value"].Value = 0;
                    dgvtotal.Rows[0].Cells["IGST"].Value = 0;
                    dgvtotal.Rows[0].Cells["CGST"].Value = 0;
                    dgvtotal.Rows[0].Cells["SGST"].Value = 0;
                    dgvtotal.Rows[0].Cells["Cess"].Value = 0;
                    dgvtotal.Refresh();

                    #endregion

                    if (_str != "")
                    {
                        CommonHelper.ErrorList = Convert.ToString(_str);
                        SPQErrorList obj = new SPQErrorList();
                        obj.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Data cleared successfully...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Getdata();
                    }

                    dgvtotal.DataSource = null;
                    Getdata();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error:{0}{1}Source:{2}{3}Error Time:{4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("Update_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }

        #region Excel import
        private void msImpExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = string.Empty; string fileExt = string.Empty;
                string conn = string.Empty, _str = string.Empty;
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
                        pbGSTR1.Visible = true;

                        #region if impoted file is open then close open file
                        foreach (Process proc in System.Diagnostics.Process.GetProcessesByName("EXCEL"))
                        {
                            if (proc.MainWindowTitle == "Microsoft Excel - " + file.SafeFileName)
                                proc.Kill();
                        }
                        #endregion

                        #region connection string
                        if (fileExt.CompareTo(".xls") == 0)
                            conn = @"provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //for below excel 2007  
                        else
                            conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 12.0 Xml;HDR=Yes;IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text'"; //for above excel 2007  
                        #endregion

                        using (OleDbConnection con = new OleDbConnection(conn))
                        {
                            #region read Data
                            DataSet ds31 = new DataSet();
                            DataSet ds32 = new DataSet();
                            DataSet ds4 = new DataSet();
                            DataSet ds5 = new DataSet();
                            DataSet ds6 = new DataSet();

                            OleDbDataAdapter oleda = new OleDbDataAdapter("SELECT * FROM [3.1-Outward-Supply$]", conn);
                            oleda.Fill(ds31, "Customer");
                            OleDbDataAdapter oleda1 = new OleDbDataAdapter("SELECT * FROM [3.2-URDComposition$]", conn);
                            oleda1.Fill(ds32, "Customer");
                            OleDbDataAdapter oleda4 = new OleDbDataAdapter("SELECT * FROM [4-Eligible-ITC$]", conn);
                            oleda4.Fill(ds4, "Customer");
                            OleDbDataAdapter oleda5 = new OleDbDataAdapter("SELECT * FROM [5.Nilrated-NonGST$]", conn);
                            oleda5.Fill(ds5, "Customer");
                            OleDbDataAdapter oleda6 = new OleDbDataAdapter("SELECT * FROM [6.PaymentOfTax$]", conn);
                            oleda6.Fill(ds6, "Customer");

                            DataTable dt31 = new DataTable();
                            dt31 = ds31.Tables["Customer"];
                            DataTable dt32 = new DataTable();
                            dt32 = ds32.Tables["Customer"];
                            DataTable dt4 = new DataTable();
                            dt4 = ds4.Tables["Customer"];
                            DataTable dt5 = new DataTable();
                            dt5 = ds5.Tables["Customer"];
                            DataTable dt6 = new DataTable();
                            dt6 = ds6.Tables["Customer"];

                            dt31 = Utility.RemoveEmptyRowsFromDataTable(dt31);
                            dt32 = Utility.RemoveEmptyRowsFromDataTable(dt32);
                            dt4 = Utility.RemoveEmptyRowsFromDataTable(dt4);
                            dt5 = Utility.RemoveEmptyRowsFromDataTable(dt5);
                            dt6 = Utility.RemoveEmptyRowsFromDataTable(dt6);

                            #endregion

                            #region first delete old data from database

                            int _Result = 0;
                            string Query = "Delete from SPQR3BOutwardSupplies where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                            _Result = objGSTR3B.IUDData(Query);
                            if (_Result != 1)
                                MessageBox.Show("System error.\nPlease try after sometime!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            _Result = 0;
                            Query = "Delete from SPQR3BInterStateSupplies where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                            _Result = objGSTR3B.IUDData(Query);
                            if (_Result != 1)
                                MessageBox.Show("System error.\nPlease try after sometime!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            _Result = 0;
                            Query = "Delete from SPQR3BEligibleITC where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                            _Result = objGSTR3B.IUDData(Query);
                            if (_Result != 1)
                                MessageBox.Show("System error.\nPlease try after sometime!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            _Result = 0;
                            Query = "Delete from SPQR3BExemptSupply where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                            _Result = objGSTR3B.IUDData(Query);
                            if (_Result != 1)
                                MessageBox.Show("System error.\nPlease try after sometime!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            Query = "Delete from SPQR3BForm6 where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                            _Result = objGSTR3B.IUDData(Query);
                            if (_Result != 1)
                                MessageBox.Show("System error.\nPlease try after sometime!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            #endregion

                            #region  Validation

                            string s31Nature = "Nature of Supplies", s31TaxVal = "Total Taxable value", s31I = "Integrated Tax", s31C = "Central Tax", s31S = "State/UT Tax", s31CE = "Cess";
                            string s32Type = "Type of supply", s32POS = "Place of Supply (State/UT)", s32TaxVal = "Total Taxable value", s32IntTax = "Amount of Integrated Tax";
                            string s34Details = "Nature of Supplies", s34I = "Integrated Tax", s34C = "Central Tax", s34S = "State/UT Tax", s34CE = "Cess";
                            string s36Desc = "Description", s36TaxPay = "Tax payable", s36PaidI = "Paid through ITC Integrated Tax", s36PaidC = "Paid through ITC Central Tax", s36PaidS = "Paid through ITC State/UT Tax", s36PaidCe = "Paid through ITC Cess", s36TaxPaid = "Tax paid TDS./TCS", s36CessPaid = "Tax/Cess paid in cash", s36Int = "Interest", s36LateFee = "Late Fee";

                            #region 3.1-Outward-Supply
                            dt31.Columns.Add("Status");
                            dt31 = Utility.ChangeColumnDataType(dt31, s31TaxVal, typeof(string));
                            dt31 = Utility.ChangeColumnDataType(dt31, s31I, typeof(string));
                            dt31 = Utility.ChangeColumnDataType(dt31, s31C, typeof(string));
                            dt31 = Utility.ChangeColumnDataType(dt31, s31S, typeof(string));
                            dt31 = Utility.ChangeColumnDataType(dt31, s31CE, typeof(string));
                            for (int i = 0; i < dt31.Rows.Count; i++)
                            {
                                dt31.Rows[i]["Status"] = "Completed";
                                if (i == 1)
                                {
                                    dt31.Rows[i][s31C] = "";
                                    dt31.Rows[i][s31S] = "";
                                }
                                else if (i == 2 || i == 4)
                                {
                                    dt31.Rows[i][s31I] = "";
                                    dt31.Rows[i][s31C] = "";
                                    dt31.Rows[i][s31S] = "";
                                    dt31.Rows[i][s31CE] = "";
                                }
                                else if (i > 4)
                                    dt31.Rows.RemoveAt(i);
                            }
                            #endregion

                            #region 3.2-URDComposition
                            dt32.Columns.Add("Status");
                            for (int i = 0; i < dt32.Rows.Count; i++)
                            {
                                dt32.Rows[i]["Status"] = "Completed";

                                if (chkCellValue(Convert.ToString(dt32.Rows[i][s32Type]).Trim(), "B31Nature"))
                                    dt32.Rows[i][s32Type] = "";

                                if (!CommonHelper.ValidateStateName(Convert.ToString(dt32.Rows[i][s32POS]).Trim()))
                                    dt32.Rows[i][s32POS] = "";
                            }
                            #endregion

                            #region 4-Eligible-ITC
                            dt4.Columns.Add("Status");
                            for (int i = 0; i < dt4.Rows.Count; i++)
                            {
                                dt4.Rows[i]["Status"] = "Completed";

                                if (i == 1 || i == 2)
                                {
                                    dt4.Rows[i][s34C] = DBNull.Value;
                                    dt4.Rows[i][s34S] = DBNull.Value;
                                }
                                else if (i > 14)
                                    dt4.Rows.RemoveAt(i);
                            }
                            #endregion

                            #region 5.Nilrated-NonGST
                            dt5.Columns.Add("Status");
                            for (int i = 0; i < dt5.Rows.Count; i++)
                            {
                                dt5.Rows[i]["Status"] = "Completed";

                                if (i > 1)
                                    dt5.Rows.RemoveAt(i);
                            }
                            #endregion

                            #region 6.PaymentOfTax
                            dt6.Columns.Add("Status");
                            dt6 = Utility.ChangeColumnDataType(dt6, s36PaidI, typeof(string));
                            dt6 = Utility.ChangeColumnDataType(dt6, s36PaidC, typeof(string));
                            dt6 = Utility.ChangeColumnDataType(dt6, s36PaidS, typeof(string));
                            for (int i = 0; i < dt6.Rows.Count; i++)
                            {
                                dt6.Rows[i]["Status"] = "Completed";
                                if (i == 1)
                                {
                                    dt6.Rows[i][s36PaidS] = "";
                                }
                                else if (i == 2)
                                {
                                    dt6.Rows[i][s36PaidC] = "";
                                }
                                else if (i == 3)
                                {
                                    dt6.Rows[i][s36PaidI] = "";
                                    dt6.Rows[i][s36PaidC] = "";
                                    dt6.Rows[i][s36PaidS] = "";
                                }
                                else if (i > 3)
                                    dt6.Rows.RemoveAt(i);
                            }

                            #endregion

                            #endregion

                            #region Save

                            #region 3B1
                            DataRow dr = dt31.NewRow();
                            dr[s31TaxVal] = dt31.Rows.Cast<DataRow>().Where(x => x[s31TaxVal] != null).Sum(x => x[s31TaxVal].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x[s31TaxVal])).ToString().Trim();
                            dr[s31I] = dt31.Rows.Cast<DataRow>().Where(x => x[s31I] != null).Sum(x => x[s31I].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x[s31I])).ToString().Trim();
                            dr[s31C] = dt31.Rows.Cast<DataRow>().Where(x => x[s31C] != null).Sum(x => x[s31C].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x[s31C])).ToString().Trim();
                            dr[s31S] = dt31.Rows.Cast<DataRow>().Where(x => x[s31S] != null).Sum(x => x[s31S].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x[s31S])).ToString().Trim();
                            dr[s31CE] = dt31.Rows.Cast<DataRow>().Where(x => x[s31CE] != null).Sum(x => x[s31CE].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x[s31CE])).ToString().Trim();
                            dr["Status"] = "Total";
                            dt31.Rows.Add(dr);

                            int _Result31 = 0;
                            _Result31 = objGSTR3B.GSTR3B1ExcelBulkEntry(dt31, Convert.ToString(CommonHelper.StatusText));
                            if (_Result31 != 1)
                            { _str += "Outward-Supply data entry error..!\n"; }
                            #endregion

                            #region 3B2
                            DataRow dr32 = dt32.NewRow();
                            dr32[s32TaxVal] = dt32.Rows.Cast<DataRow>().Where(x => x[s32TaxVal] != null).Sum(x => x[s32TaxVal].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x[s32TaxVal])).ToString().Trim();
                            dr32[s32IntTax] = dt32.Rows.Cast<DataRow>().Where(x => x[s32IntTax] != null).Sum(x => x[s32IntTax].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x[s32IntTax])).ToString().Trim();
                            dr32["Status"] = "Total";
                            dt32.Rows.Add(dr32);

                            int _Result32 = 0;
                            _Result32 = objGSTR3B.GSTR3B2ExcelBulkEntry(dt32, Convert.ToString(CommonHelper.StatusText));
                            if (_Result32 != 1)
                            { _str += "URD Composition data entry error..!\n"; }
                            #endregion

                            #region 3B4
                            int _Result4 = 0;
                            _Result4 = objGSTR3B.GSTR3B4ExcelBulkEntry(dt4, Convert.ToString(CommonHelper.StatusText));
                            if (_Result4 != 1)
                            { _str += "Eligible-ITC data entry error..!\n"; }
                            #endregion

                            #region 3B5
                            int _Result5 = 0;
                            _Result5 = objGSTR3B.GSTR3B5ExcelBulkEntry(dt5, Convert.ToString(CommonHelper.StatusText));
                            if (_Result5 != 1)
                            { _str += "Nilrated-NonGST data entry error..!\n"; }
                            #endregion

                            #region 3B6
                            int _Result6 = 0;
                            _Result6 = 1;//objGSTR3B.GSTR3B6ExcelBulkEntry(dt6, Convert.ToString(CommonHelper.StatusText));
                            if (_Result6 != 1)
                            { _str += "PaymentOfTax data entry error..!\n"; }
                            #endregion

                            #endregion

                            pbGSTR1.Visible = false;

                            if (_str != "")
                            {
                                CommonHelper.ErrorList = Convert.ToString(_str);
                                SPQErrorList obj = new SPQErrorList();
                                obj.ShowDialog();
                            }
                            else
                            {
                                MessageBox.Show("Data imported successfully...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Getdata();
                            }
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
                pbGSTR1.Visible = false;
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }
        private void msImpGSTNutility_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = string.Empty; string fileExt = string.Empty;
                string conn = string.Empty, _str = string.Empty;
                //open dialog to choose file
                OpenFileDialog file = new OpenFileDialog();
                if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // get file name and extention of selected file
                    filePath = file.FileName;
                    fileExt = Path.GetExtension(filePath);

                    // check selected file extention
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

                        #region connection string
                        if (fileExt.CompareTo(".xls") == 0)
                            conn = @"provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //for below excel 2007  
                        else
                            conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 12.0 Xml;HDR=Yes;IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text'"; //for above excel 2007  
                        #endregion

                        using (OleDbConnection con = new OleDbConnection(conn))
                        {

                            #region read Data
                            DataSet ds31 = new DataSet();

                            OleDbDataAdapter oleda = new OleDbDataAdapter("SELECT * FROM [GSTR-3B$]", conn);
                            oleda.Fill(ds31, "Customer");

                            DataTable dt31 = new DataTable();
                            dt31 = ds31.Tables["Customer"];

                            dt31 = Utility.RemoveEmptyRowsFromDataTable(dt31);

                            #region SPQR3BOutwardSupplies
                            DataTable dt111 = new DataTable();
                            dt111.Columns.Add("Fld_Sequence");
                            dt111.Columns.Add("Fld_NatureofSupply");
                            dt111.Columns.Add("Fld_TotalTaxableValue");
                            dt111.Columns.Add("Fld_IGST");
                            dt111.Columns.Add("Fld_CGST");
                            dt111.Columns.Add("Fld_SGST");
                            dt111.Columns.Add("Fld_CESS");
                            dt111.Columns.Add("Fld_Month");
                            dt111.Columns.Add("Fld_FileStatus");
                            dt111.AcceptChanges();
                            if (dt31 != null)
                            {
                                dt31.Rows.RemoveAt(0);
                                dt31.Rows.RemoveAt(0);
                                dt31.Rows.RemoveAt(0);
                                dt31.Rows.RemoveAt(0);
                                dt31.Rows.RemoveAt(0);
                                dt31.Rows.RemoveAt(0);
                                dt31.Columns.RemoveAt(0);
                                dt31.AcceptChanges();

                                for (int i = 0; i <= dt31.Rows.Count; i++)
                                {
                                    DataRow row = dt111.NewRow();
                                    if (i >= 0 && i <= 5)
                                    {
                                        row["Fld_Sequence"] = i + 1;
                                        row["Fld_FileStatus"] = "Completed";
                                        if (i == 0)
                                        {
                                            row["Fld_NatureofSupply"] = "(a) Outward Taxable Supplies (other than Zero rated,nil rated and exemted)";
                                            row["Fld_TotalTaxableValue"] = dt31.Rows[i]["F3"].ToString().Trim();
                                            row["Fld_IGST"] = dt31.Rows[i]["F4"].ToString().Trim();
                                            row["Fld_CGST"] = dt31.Rows[i]["F5"].ToString().Trim();
                                            row["Fld_SGST"] = dt31.Rows[i]["F6"].ToString().Trim();
                                            row["Fld_CESS"] = dt31.Rows[i]["F7"].ToString().Trim();
                                        }
                                        if (i == 1)
                                        {
                                            row["Fld_NatureofSupply"] = "(b) Outward Taxable Supplies (Zero rated)";
                                            row["Fld_TotalTaxableValue"] = dt31.Rows[i]["F3"].ToString().Trim();
                                            row["Fld_IGST"] = dt31.Rows[i]["F4"].ToString().Trim();
                                            row["Fld_CGST"] = dt31.Rows[i]["F5"].ToString().Trim();
                                            row["Fld_SGST"] = dt31.Rows[i]["F6"].ToString().Trim();
                                            row["Fld_CESS"] = dt31.Rows[i]["F7"].ToString().Trim();
                                        }
                                        if (i == 2)
                                        {
                                            row["Fld_NatureofSupply"] = "(c) Other outward Supplies(Nil rated, exemted)";
                                            row["Fld_TotalTaxableValue"] = dt31.Rows[i]["F3"].ToString().Trim();
                                            row["Fld_IGST"] = dt31.Rows[i]["F4"].ToString().Trim();
                                            row["Fld_CGST"] = dt31.Rows[i]["F5"].ToString().Trim();
                                            row["Fld_SGST"] = dt31.Rows[i]["F6"].ToString().Trim();
                                            row["Fld_CESS"] = dt31.Rows[i]["F7"].ToString().Trim();
                                        }
                                        if (i == 3)
                                        {
                                            row["Fld_NatureofSupply"] = "(d) Inward Supplies(liable to reverse charge)";
                                            row["Fld_TotalTaxableValue"] = dt31.Rows[i]["F3"].ToString().Trim();
                                            row["Fld_IGST"] = dt31.Rows[i]["F4"].ToString().Trim();
                                            row["Fld_CGST"] = dt31.Rows[i]["F5"].ToString().Trim();
                                            row["Fld_SGST"] = dt31.Rows[i]["F6"].ToString().Trim();
                                            row["Fld_CESS"] = dt31.Rows[i]["F7"].ToString().Trim();
                                        }
                                        if (i == 4)
                                        {
                                            row["Fld_NatureofSupply"] = "(e) Non-GST outward supplies";
                                            row["Fld_TotalTaxableValue"] = dt31.Rows[i]["F3"].ToString().Trim();
                                            row["Fld_IGST"] = dt31.Rows[i]["F4"].ToString().Trim();
                                            row["Fld_CGST"] = dt31.Rows[i]["F5"].ToString().Trim();
                                            row["Fld_SGST"] = dt31.Rows[i]["F6"].ToString().Trim();
                                            row["Fld_CESS"] = dt31.Rows[i]["F7"].ToString().Trim();
                                        }
                                        if (i == 5)
                                        {
                                            row["Fld_NatureofSupply"] = "Total";
                                            row["Fld_TotalTaxableValue"] = dt31.Rows[i]["F3"].ToString().Trim();
                                            row["Fld_IGST"] = dt31.Rows[i]["F4"].ToString().Trim();
                                            row["Fld_CGST"] = dt31.Rows[i]["F5"].ToString().Trim();
                                            row["Fld_SGST"] = dt31.Rows[i]["F6"].ToString().Trim();
                                            row["Fld_CESS"] = dt31.Rows[i]["F7"].ToString().Trim();
                                            row["Fld_FileStatus"] = "Total";
                                        }
                                        dt111.Rows.Add(row);
                                        if (i == 5)
                                        {
                                            dt111.AcceptChanges();
                                            break;
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region SPQR3BEligibleITC
                            DataTable dt4_new = new DataTable();
                            dt4_new.Columns.Add("Fld_Sequence");
                            dt4_new.Columns.Add("Fld_Details");
                            dt4_new.Columns.Add("Fld_IGST");
                            dt4_new.Columns.Add("Fld_CGST");
                            dt4_new.Columns.Add("Fld_SGST");
                            dt4_new.Columns.Add("Fld_CESS");
                            dt4_new.Columns.Add("Fld_Month");
                            dt4_new.Columns.Add("Fld_FileStatus");
                            dt4_new.AcceptChanges();
                            if (dt31 != null)
                            {
                                for (int j = 0; j <= 8; j++)
                                {
                                    dt31.Rows.RemoveAt(0);
                                }
                                dt31.AcceptChanges();
                                for (int i = 0; i <= dt31.Rows.Count; i++)
                                {
                                    DataRow row = dt4_new.NewRow();
                                    row["Fld_Sequence"] = i + 1;
                                    row["Fld_FileStatus"] = "Completed";
                                    if (i >= 0 && i <= 14)
                                    {
                                        if (i == 0)
                                        {
                                            row["Fld_Details"] = "(A) ITC Available (Whether in full or part)";
                                            row["Fld_IGST"] = dt31.Rows[i]["F3"].ToString().Trim();
                                            row["Fld_CGST"] = dt31.Rows[i]["F4"].ToString().Trim();
                                            row["Fld_SGST"] = dt31.Rows[i]["F5"].ToString().Trim();
                                            row["Fld_CESS"] = dt31.Rows[i]["F6"].ToString().Trim();
                                            dt4_new.Rows.Add(row);
                                        }
                                        if (i == 1)
                                        {
                                            row["Fld_Details"] = "  (1) Import of goods";
                                            row["Fld_IGST"] = dt31.Rows[i]["F3"].ToString().Trim();
                                            row["Fld_CGST"] = dt31.Rows[i]["F4"].ToString().Trim();
                                            row["Fld_SGST"] = dt31.Rows[i]["F5"].ToString().Trim();
                                            row["Fld_CESS"] = dt31.Rows[i]["F6"].ToString().Trim();
                                            dt4_new.Rows.Add(row);
                                        }
                                        if (i == 2)
                                        {
                                            row["Fld_Details"] = "  (2) Import of Services";
                                            row["Fld_IGST"] = dt31.Rows[i]["F3"].ToString().Trim();
                                            row["Fld_CGST"] = dt31.Rows[i]["F4"].ToString().Trim();
                                            row["Fld_SGST"] = dt31.Rows[i]["F5"].ToString().Trim();
                                            row["Fld_CESS"] = dt31.Rows[i]["F6"].ToString().Trim();
                                            dt4_new.Rows.Add(row);
                                        }
                                        if (i == 3)
                                        {
                                            row["Fld_Details"] = "  (3) Inward Supplies liable to reverse charge(other than 1 & 2 above)";
                                            row["Fld_IGST"] = dt31.Rows[i]["F3"].ToString().Trim();
                                            row["Fld_CGST"] = dt31.Rows[i]["F4"].ToString().Trim();
                                            row["Fld_SGST"] = dt31.Rows[i]["F5"].ToString().Trim();
                                            row["Fld_CESS"] = dt31.Rows[i]["F6"].ToString().Trim();
                                            dt4_new.Rows.Add(row);
                                        }
                                        if (i == 4)
                                        {
                                            row["Fld_Details"] = "  (4) Inward supplies from ISD";
                                            row["Fld_IGST"] = dt31.Rows[i]["F3"].ToString().Trim();
                                            row["Fld_CGST"] = dt31.Rows[i]["F4"].ToString().Trim();
                                            row["Fld_SGST"] = dt31.Rows[i]["F5"].ToString().Trim();
                                            row["Fld_CESS"] = dt31.Rows[i]["F6"].ToString().Trim();
                                            dt4_new.Rows.Add(row);
                                        }
                                        if (i == 5)
                                        {
                                            row["Fld_Details"] = "  (5) All other ITC";
                                            row["Fld_IGST"] = dt31.Rows[i]["F3"].ToString().Trim();
                                            row["Fld_CGST"] = dt31.Rows[i]["F4"].ToString().Trim();
                                            row["Fld_SGST"] = dt31.Rows[i]["F5"].ToString().Trim();
                                            row["Fld_CESS"] = dt31.Rows[i]["F6"].ToString().Trim();
                                            dt4_new.Rows.Add(row);
                                        }

                                        if (i == 6)
                                        {
                                            row = dt4_new.NewRow();
                                            row["Fld_Sequence"] = i + 1;
                                            row["Fld_Details"] = "Total ITC Available (A)";
                                            row["Fld_IGST"] = "";
                                            row["Fld_CGST"] = "";
                                            row["Fld_SGST"] = "";
                                            row["Fld_CESS"] = "";
                                            row["Fld_FileStatus"] = "Completed";
                                            dt4_new.Rows.Add(row);

                                            row = dt4_new.NewRow();
                                            row["Fld_Sequence"] = i + 2;
                                            row["Fld_Details"] = "(B) ITC Reversed";
                                            row["Fld_IGST"] = dt31.Rows[i]["F3"].ToString().Trim();
                                            row["Fld_CGST"] = dt31.Rows[i]["F4"].ToString().Trim();
                                            row["Fld_SGST"] = dt31.Rows[i]["F5"].ToString().Trim();
                                            row["Fld_CESS"] = dt31.Rows[i]["F6"].ToString().Trim();
                                            row["Fld_FileStatus"] = "Completed";
                                            dt4_new.Rows.Add(row);
                                        }
                                        if (i == 7)
                                        {
                                            row["Fld_Sequence"] = i + 2;
                                            row["Fld_Details"] = "  (1) As per rules 42 & 43 IGST Rules";
                                            row["Fld_IGST"] = dt31.Rows[i]["F3"].ToString().Trim();
                                            row["Fld_CGST"] = dt31.Rows[i]["F4"].ToString().Trim();
                                            row["Fld_SGST"] = dt31.Rows[i]["F5"].ToString().Trim();
                                            row["Fld_CESS"] = dt31.Rows[i]["F6"].ToString().Trim();
                                            dt4_new.Rows.Add(row);
                                        }
                                        if (i == 8)
                                        {
                                            row["Fld_Sequence"] = i + 2;
                                            row["Fld_Details"] = "  (2) Others";
                                            row["Fld_IGST"] = dt31.Rows[i]["F3"].ToString().Trim();
                                            row["Fld_CGST"] = dt31.Rows[i]["F4"].ToString().Trim();
                                            row["Fld_SGST"] = dt31.Rows[i]["F5"].ToString().Trim();
                                            row["Fld_CESS"] = dt31.Rows[i]["F6"].ToString().Trim();
                                            dt4_new.Rows.Add(row);
                                        }
                                        if (i == 9)
                                        {
                                            row = dt4_new.NewRow();
                                            row["Fld_Sequence"] = i + 2;
                                            row["Fld_Details"] = "Total ITC Reversed (B)";
                                            row["Fld_IGST"] = "";
                                            row["Fld_CGST"] = "";
                                            row["Fld_SGST"] = "";
                                            row["Fld_CESS"] = "";
                                            row["Fld_FileStatus"] = "Completed";
                                            dt4_new.Rows.Add(row);

                                            row = dt4_new.NewRow();
                                            row["Fld_Sequence"] = i + 3;
                                            row["Fld_Details"] = "(C) Net ITC Available (A) – (B)";
                                            row["Fld_IGST"] = dt31.Rows[i]["F3"].ToString().Trim();
                                            row["Fld_CGST"] = dt31.Rows[i]["F4"].ToString().Trim();
                                            row["Fld_SGST"] = dt31.Rows[i]["F5"].ToString().Trim();
                                            row["Fld_CESS"] = dt31.Rows[i]["F6"].ToString().Trim();
                                            row["Fld_FileStatus"] = "Completed";
                                            dt4_new.Rows.Add(row);

                                        }
                                        if (i == 10)
                                        {
                                            row["Fld_Sequence"] = i + 3;
                                            row["Fld_Details"] = "(D) Ineligible ITC";
                                            row["Fld_IGST"] = dt31.Rows[i]["F3"].ToString().Trim();
                                            row["Fld_CGST"] = dt31.Rows[i]["F4"].ToString().Trim();
                                            row["Fld_SGST"] = dt31.Rows[i]["F5"].ToString().Trim();
                                            row["Fld_CESS"] = dt31.Rows[i]["F6"].ToString().Trim();
                                            dt4_new.Rows.Add(row);
                                        }
                                        if (i == 11)
                                        {
                                            row["Fld_Sequence"] = i + 3;
                                            row["Fld_Details"] = "  (1) As per section 17(5)";
                                            row["Fld_IGST"] = dt31.Rows[i]["F3"].ToString().Trim();
                                            row["Fld_CGST"] = dt31.Rows[i]["F4"].ToString().Trim();
                                            row["Fld_SGST"] = dt31.Rows[i]["F5"].ToString().Trim();
                                            row["Fld_CESS"] = dt31.Rows[i]["F6"].ToString().Trim();
                                            dt4_new.Rows.Add(row);
                                        }
                                        if (i == 12)
                                        {
                                            row["Fld_Sequence"] = i + 3;
                                            row["Fld_Details"] = "  (2) Others";
                                            row["Fld_IGST"] = dt31.Rows[i]["F3"].ToString().Trim();
                                            row["Fld_CGST"] = dt31.Rows[i]["F4"].ToString().Trim();
                                            row["Fld_SGST"] = dt31.Rows[i]["F5"].ToString().Trim();
                                            row["Fld_CESS"] = dt31.Rows[i]["F6"].ToString().Trim();
                                            dt4_new.Rows.Add(row);
                                        }
                                        if (i == 12)
                                        {
                                            dt4_new.AcceptChanges();
                                            break;
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region SPQR3BExemptSupply
                            DataTable dt5_new = new DataTable();
                            dt5_new.Columns.Add("Fld_Sequence");
                            dt5_new.Columns.Add("Fld_NatureofSupply");
                            dt5_new.Columns.Add("Fld_InterStateSupplies");
                            dt5_new.Columns.Add("Fld_IntraStateSupplies");
                            dt5_new.Columns.Add("Fld_Month");
                            dt5_new.Columns.Add("Fld_FileStatus");
                            dt5_new.AcceptChanges();
                            if (dt31 != null)
                            {
                                for (int j = 0; j <= 15; j++)
                                {
                                    dt31.Rows.RemoveAt(0);
                                }
                                dt31.AcceptChanges();
                                for (int i = 0; i <= dt31.Rows.Count; i++)
                                {
                                    DataRow row = dt5_new.NewRow();
                                    row["Fld_Sequence"] = i + 1;
                                    row["Fld_FileStatus"] = "Completed";
                                    if (i >= 0 && i <= 2)
                                    {
                                        if (i == 0)
                                        {
                                            row["Fld_NatureofSupply"] = "From a supplier under composition scheme, Exempt and Nil rated Supply";
                                            row["Fld_InterStateSupplies"] = dt31.Rows[i]["F4"].ToString().Trim();
                                            row["Fld_IntraStateSupplies"] = dt31.Rows[i]["F5"].ToString().Trim();
                                        }
                                        if (i == 1)
                                        {
                                            row["Fld_NatureofSupply"] = "Non GST Supply";
                                            row["Fld_InterStateSupplies"] = dt31.Rows[i]["F4"].ToString().Trim();
                                            row["Fld_IntraStateSupplies"] = dt31.Rows[i]["F5"].ToString().Trim();
                                        }
                                        if (i == 2)
                                        {
                                            row["Fld_NatureofSupply"] = "Total";
                                            row["Fld_InterStateSupplies"] = dt31.Rows[i]["F4"].ToString().Trim();
                                            row["Fld_IntraStateSupplies"] = dt31.Rows[i]["F5"].ToString().Trim();
                                            row["Fld_FileStatus"] = "Total";
                                        }
                                    }
                                    dt5_new.Rows.Add(row);
                                    if (i == 2)
                                    {
                                        dt5_new.AcceptChanges();
                                        break;
                                    }
                                }
                            }
                            #endregion

                            #region SPQR3BInterStateSupplies

                            DataTable dt32_new = new DataTable();
                            dt32_new.Columns.Add("Fld_Sequence");
                            dt32_new.Columns.Add("Fld_Details");
                            dt32_new.Columns.Add("Fld_POS");
                            dt32_new.Columns.Add("Fld_Taxable");
                            dt32_new.Columns.Add("Fld_IGST");
                            dt32_new.Columns.Add("Fld_Month");
                            dt32_new.Columns.Add("Fld_FileStatus");
                            dt32_new.AcceptChanges();
                            if (dt31 != null)
                            {
                                for (int j = 0; j <= 33; j++)
                                {
                                    dt31.Rows.RemoveAt(0);
                                }
                                dt31.AcceptChanges();
                                for (int i = 0; i <= dt31.Rows.Count; i++)
                                {
                                    if (i >= 0 && i < dt31.Rows.Count - 1)
                                    {
                                        if (dt31.Rows[i]["F3"].ToString().Trim() != "" && dt31.Rows[i]["F4"].ToString().Trim() != "")
                                        {
                                            DataRow row = dt32_new.NewRow();
                                            row["Fld_Sequence"] = "";
                                            row["Fld_FileStatus"] = "Completed";
                                            row["Fld_Details"] = "Supplies made to Unregistered Persons";
                                            row["Fld_POS"] = Utility.strValidStateName(dt31.Rows[i]["F2"].ToString().Trim());
                                            row["Fld_Taxable"] = dt31.Rows[i]["F3"].ToString().Trim();
                                            row["Fld_IGST"] = dt31.Rows[i]["F4"].ToString().Trim();
                                            dt32_new.Rows.Add(row);
                                        }
                                    }
                                    if (i >= 0 && i < dt31.Rows.Count - 1)
                                    {
                                        if (dt31.Rows[i]["F5"].ToString().Trim() != "" && dt31.Rows[i]["F6"].ToString().Trim() != "")
                                        {
                                            DataRow row = dt32_new.NewRow();
                                            row["Fld_Sequence"] = "";
                                            row["Fld_FileStatus"] = "Completed";
                                            row["Fld_Details"] = "Supplies made to Composition Taxable Persons";
                                            row["Fld_POS"] = Utility.strValidStateName(dt31.Rows[i]["F2"].ToString().Trim());
                                            row["Fld_Taxable"] = dt31.Rows[i]["F5"].ToString().Trim();
                                            row["Fld_IGST"] = dt31.Rows[i]["F6"].ToString().Trim();
                                            dt32_new.Rows.Add(row);
                                        }
                                    }
                                    if (i >= 0 && i < dt31.Rows.Count - 1)
                                    {
                                        if (dt31.Rows[i]["F7"].ToString().Trim() != "" && dt31.Rows[i]["F8"].ToString().Trim() != "")
                                        {
                                            DataRow row = dt32_new.NewRow();
                                            row["Fld_Sequence"] = "";
                                            row["Fld_FileStatus"] = "Completed";
                                            row["Fld_Details"] = "Supplies made to UIN holders";
                                            row["Fld_POS"] = Utility.strValidStateName(dt31.Rows[i]["F2"].ToString());
                                            row["Fld_Taxable"] = dt31.Rows[i]["F7"].ToString();
                                            row["Fld_IGST"] = dt31.Rows[i]["F8"].ToString();
                                            dt32_new.Rows.Add(row);
                                        }
                                    }
                                    if (i == dt31.Rows.Count - 1)
                                    {
                                        DataRow row = dt32_new.NewRow();
                                        row["Fld_Sequence"] = "";
                                        row["Fld_FileStatus"] = "Total";
                                        row["Fld_Details"] = "Total";
                                        row["Fld_POS"] = "";
                                        double x = Convert.ToDouble(dt31.Rows[i]["F3"].ToString()) + Convert.ToDouble(dt31.Rows[i]["F5"].ToString()) + Convert.ToDouble(dt31.Rows[i]["F7"].ToString());
                                        row["Fld_Taxable"] = x;
                                        double y = Convert.ToDouble(dt31.Rows[i]["F4"].ToString()) + Convert.ToDouble(dt31.Rows[i]["F6"].ToString()) + Convert.ToDouble(dt31.Rows[i]["F8"].ToString());
                                        row["Fld_IGST"] = y;
                                        dt32_new.Rows.Add(row);
                                    }
                                }
                            }
                            dt32_new.AcceptChanges();

                            #endregion

                            #endregion

                            #region first delete old data from database

                            int _Result = 0;
                            string Query = "Delete from SPQR3BOutwardSupplies where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                            _Result = objGSTR3B.IUDData(Query);
                            if (_Result != 1)
                                MessageBox.Show("System error.\nPlease try after sometime!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            _Result = 0;
                            Query = "Delete from SPQR3BInterStateSupplies where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                            _Result = objGSTR3B.IUDData(Query);
                            if (_Result != 1)
                                MessageBox.Show("System error.\nPlease try after sometime!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            _Result = 0;
                            Query = "Delete from SPQR3BEligibleITC where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                            _Result = objGSTR3B.IUDData(Query);
                            if (_Result != 1)
                                MessageBox.Show("System error.\nPlease try after sometime!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            _Result = 0;
                            Query = "Delete from SPQR3BExemptSupply where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                            _Result = objGSTR3B.IUDData(Query);
                            if (_Result != 1)
                                MessageBox.Show("System error.\nPlease try after sometime!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            #endregion

                            #region Save

                            #region 3B1

                            int _Result31 = 0;
                            _Result31 = objGSTR3B.GSTR3B1_Gov_Utility_ExcelBulkEntry(dt111, Convert.ToString(CommonHelper.StatusText));
                            if (_Result31 != 1)
                            { _str += "outward-supply data entry error..!\n"; }
                            #endregion

                            #region 3B2

                            int _Result32 = 0;
                            _Result32 = objGSTR3B.GSTR3B2_Gov_Utility_ExcelBulkEntry(dt32_new, Convert.ToString(CommonHelper.StatusText));
                            if (_Result32 != 1)
                            { _str += "URD Composition data entry error..!\n"; }

                            #endregion

                            #region 3B4
                            int _Result4 = 0;
                            _Result4 = objGSTR3B.GSTR3B4_Gov_Utility_ExcelBulkEntry(dt4_new, Convert.ToString(CommonHelper.StatusText));
                            if (_Result4 != 1)
                            { _str += "Eligible-ITC data entry error..!\n"; }
                            #endregion

                            #region 3B5
                            int _Result5 = 0;
                            _Result5 = objGSTR3B.GSTR3B5_Gov_Utility_ExcelBulkEntry(dt5_new, Convert.ToString(CommonHelper.StatusText));
                            if (_Result5 != 1)
                            { _str += "Nilrated-NonGST data entry error..!\n"; }
                            #endregion

                            #endregion

                            pbGSTR1.Visible = false;

                            if (_str != "")
                            {
                                CommonHelper.ErrorList = Convert.ToString(_str);
                                SPQErrorList obj = new SPQErrorList();
                                obj.ShowDialog();
                            }
                            else
                            {
                                MessageBox.Show("Data imported successfully...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Getdata();
                            }
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
                pbGSTR1.Visible = false;
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }
        private void msImpTally_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = string.Empty; string fileExt = string.Empty;
                string conn = string.Empty, _str = string.Empty;
                //open dialog to choose file
                OpenFileDialog file = new OpenFileDialog();
                if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // get file name and extention of selected file
                    filePath = file.FileName;
                    fileExt = Path.GetExtension(filePath);

                    // check selected file extention
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

                        #region connection string
                        if (fileExt.CompareTo(".xls") == 0)
                            conn = @"provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //for below excel 2007  
                        else
                            conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 12.0 Xml;HDR=Yes;IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text'"; //for above excel 2007  
                        #endregion

                        using (OleDbConnection con = new OleDbConnection(conn))
                        {

                            #region read Data
                            DataSet ds31 = new DataSet();

                            OleDbDataAdapter oleda = new OleDbDataAdapter("SELECT * FROM [GSTR-3B$]", conn);
                            oleda.Fill(ds31, "Customer");

                            DataTable dt31 = new DataTable();
                            dt31 = ds31.Tables["Customer"];

                            dt31 = Utility.RemoveEmptyRowsFromDataTable(dt31);

                            #region SPQR3BOutwardSupplies
                            DataTable dt111 = new DataTable();
                            dt111.Columns.Add("Fld_Sequence");
                            dt111.Columns.Add("Fld_NatureofSupply");
                            dt111.Columns.Add("Fld_TotalTaxableValue");
                            dt111.Columns.Add("Fld_IGST");
                            dt111.Columns.Add("Fld_CGST");
                            dt111.Columns.Add("Fld_SGST");
                            dt111.Columns.Add("Fld_CESS");
                            dt111.Columns.Add("Fld_Month");
                            dt111.Columns.Add("Fld_FileStatus");
                            dt111.AcceptChanges();
                            if (dt31 != null)
                            {
                                dt31.Rows.RemoveAt(0);
                                dt31.Rows.RemoveAt(0);
                                dt31.Rows.RemoveAt(0);
                                dt31.Rows.RemoveAt(0);
                                dt31.Rows.RemoveAt(0);
                                dt31.Rows.RemoveAt(0);
                                dt31.Columns.RemoveAt(0);
                                dt31.AcceptChanges();

                                for (int i = 0; i <= dt31.Rows.Count; i++)
                                {
                                    DataRow row = dt111.NewRow();
                                    if (i >= 0 && i <= 5)
                                    {
                                        row["Fld_Sequence"] = i + 1;
                                        row["Fld_FileStatus"] = "Completed";
                                        if (i == 0)
                                        {
                                            row["Fld_NatureofSupply"] = "(a) Outward Taxable Supplies (other than Zero rated,nil rated and exemted)";
                                            row["Fld_TotalTaxableValue"] = dt31.Rows[i]["F3"].ToString();
                                            row["Fld_IGST"] = dt31.Rows[i]["F4"].ToString();
                                            row["Fld_CGST"] = dt31.Rows[i]["F5"].ToString();
                                            row["Fld_SGST"] = dt31.Rows[i]["F6"].ToString();
                                            row["Fld_CESS"] = dt31.Rows[i]["F7"].ToString();
                                        }
                                        if (i == 1)
                                        {
                                            row["Fld_NatureofSupply"] = "(b) Outward Taxable Supplies (Zero rated)";
                                            row["Fld_TotalTaxableValue"] = dt31.Rows[i]["F3"].ToString();
                                            row["Fld_IGST"] = dt31.Rows[i]["F4"].ToString();
                                            row["Fld_CGST"] = dt31.Rows[i]["F5"].ToString();
                                            row["Fld_SGST"] = dt31.Rows[i]["F6"].ToString();
                                            row["Fld_CESS"] = dt31.Rows[i]["F7"].ToString();
                                        }
                                        if (i == 2)
                                        {
                                            row["Fld_NatureofSupply"] = "(c) Other outward Supplies(Nil rated, exemted)";
                                            row["Fld_TotalTaxableValue"] = dt31.Rows[i]["F3"].ToString();
                                            row["Fld_IGST"] = dt31.Rows[i]["F4"].ToString();
                                            row["Fld_CGST"] = dt31.Rows[i]["F5"].ToString();
                                            row["Fld_SGST"] = dt31.Rows[i]["F6"].ToString();
                                            row["Fld_CESS"] = dt31.Rows[i]["F7"].ToString();
                                        }
                                        if (i == 3)
                                        {
                                            row["Fld_NatureofSupply"] = "(d) Inward Supplies(liable to reverse charge)";
                                            row["Fld_TotalTaxableValue"] = dt31.Rows[i]["F3"].ToString();
                                            row["Fld_IGST"] = dt31.Rows[i]["F4"].ToString();
                                            row["Fld_CGST"] = dt31.Rows[i]["F5"].ToString();
                                            row["Fld_SGST"] = dt31.Rows[i]["F6"].ToString();
                                            row["Fld_CESS"] = dt31.Rows[i]["F7"].ToString();
                                        }
                                        if (i == 4)
                                        {
                                            row["Fld_NatureofSupply"] = "(e) Non-GST outward supplies";
                                            row["Fld_TotalTaxableValue"] = dt31.Rows[i]["F3"].ToString();
                                            row["Fld_IGST"] = dt31.Rows[i]["F4"].ToString();
                                            row["Fld_CGST"] = dt31.Rows[i]["F5"].ToString();
                                            row["Fld_SGST"] = dt31.Rows[i]["F6"].ToString();
                                            row["Fld_CESS"] = dt31.Rows[i]["F7"].ToString();
                                        }
                                        if (i == 5)
                                        {
                                            row["Fld_NatureofSupply"] = "Total";
                                            row["Fld_TotalTaxableValue"] = dt31.Rows[i]["F3"].ToString();
                                            row["Fld_IGST"] = dt31.Rows[i]["F4"].ToString();
                                            row["Fld_CGST"] = dt31.Rows[i]["F5"].ToString();
                                            row["Fld_SGST"] = dt31.Rows[i]["F6"].ToString();
                                            row["Fld_CESS"] = dt31.Rows[i]["F7"].ToString();
                                            row["Fld_FileStatus"] = "Total";
                                        }
                                        dt111.Rows.Add(row);
                                        if (i == 5)
                                        {
                                            dt111.AcceptChanges();
                                            break;
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region SPQR3BEligibleITC
                            DataTable dt4_new = new DataTable();
                            dt4_new.Columns.Add("Fld_Sequence");
                            dt4_new.Columns.Add("Fld_Details");
                            dt4_new.Columns.Add("Fld_IGST");
                            dt4_new.Columns.Add("Fld_CGST");
                            dt4_new.Columns.Add("Fld_SGST");
                            dt4_new.Columns.Add("Fld_CESS");
                            dt4_new.Columns.Add("Fld_Month");
                            dt4_new.Columns.Add("Fld_FileStatus");
                            dt4_new.AcceptChanges();
                            if (dt31 != null)
                            {
                                for (int j = 0; j <= 8; j++)
                                {
                                    dt31.Rows.RemoveAt(0);
                                }
                                dt31.AcceptChanges();
                                for (int i = 0; i <= dt31.Rows.Count; i++)
                                {
                                    DataRow row = dt4_new.NewRow();
                                    row["Fld_Sequence"] = i + 1;
                                    row["Fld_FileStatus"] = "Completed";
                                    if (i >= 0 && i <= 14)
                                    {
                                        if (i == 0)
                                        {
                                            row["Fld_Details"] = "(A) ITC Available (Whether in full or part)";
                                            row["Fld_IGST"] = dt31.Rows[i]["F3"].ToString();
                                            row["Fld_CGST"] = dt31.Rows[i]["F4"].ToString();
                                            row["Fld_SGST"] = dt31.Rows[i]["F5"].ToString();
                                            row["Fld_CESS"] = dt31.Rows[i]["F6"].ToString();
                                            dt4_new.Rows.Add(row);
                                        }
                                        if (i == 1)
                                        {
                                            row["Fld_Details"] = "  (1) Import of goods";
                                            row["Fld_IGST"] = dt31.Rows[i]["F3"].ToString();
                                            row["Fld_CGST"] = dt31.Rows[i]["F4"].ToString();
                                            row["Fld_SGST"] = dt31.Rows[i]["F5"].ToString();
                                            row["Fld_CESS"] = dt31.Rows[i]["F6"].ToString();
                                            dt4_new.Rows.Add(row);
                                        }
                                        if (i == 2)
                                        {
                                            row["Fld_Details"] = "  (2) Import of Services";
                                            row["Fld_IGST"] = dt31.Rows[i]["F3"].ToString();
                                            row["Fld_CGST"] = dt31.Rows[i]["F4"].ToString();
                                            row["Fld_SGST"] = dt31.Rows[i]["F5"].ToString();
                                            row["Fld_CESS"] = dt31.Rows[i]["F6"].ToString();
                                            dt4_new.Rows.Add(row);
                                        }
                                        if (i == 3)
                                        {
                                            row["Fld_Details"] = "  (3) Inward Supplies liable to reverse charge(other than 1 & 2 above)";
                                            row["Fld_IGST"] = dt31.Rows[i]["F3"].ToString();
                                            row["Fld_CGST"] = dt31.Rows[i]["F4"].ToString();
                                            row["Fld_SGST"] = dt31.Rows[i]["F5"].ToString();
                                            row["Fld_CESS"] = dt31.Rows[i]["F6"].ToString();
                                            dt4_new.Rows.Add(row);
                                        }
                                        if (i == 4)
                                        {
                                            row["Fld_Details"] = "  (4) Inward supplies from ISD";
                                            row["Fld_IGST"] = dt31.Rows[i]["F3"].ToString();
                                            row["Fld_CGST"] = dt31.Rows[i]["F4"].ToString();
                                            row["Fld_SGST"] = dt31.Rows[i]["F5"].ToString();
                                            row["Fld_CESS"] = dt31.Rows[i]["F6"].ToString();
                                            dt4_new.Rows.Add(row);
                                        }
                                        if (i == 5)
                                        {
                                            row["Fld_Details"] = "  (5) All other ITC";
                                            row["Fld_IGST"] = dt31.Rows[i]["F3"].ToString();
                                            row["Fld_CGST"] = dt31.Rows[i]["F4"].ToString();
                                            row["Fld_SGST"] = dt31.Rows[i]["F5"].ToString();
                                            row["Fld_CESS"] = dt31.Rows[i]["F6"].ToString();
                                            dt4_new.Rows.Add(row);
                                        }

                                        if (i == 6)
                                        {
                                            row = dt4_new.NewRow();
                                            row["Fld_Sequence"] = i + 1;
                                            row["Fld_Details"] = "Total ITC Available (A)";
                                            row["Fld_IGST"] = "";
                                            row["Fld_CGST"] = "";
                                            row["Fld_SGST"] = "";
                                            row["Fld_CESS"] = "";
                                            row["Fld_FileStatus"] = "Completed";
                                            dt4_new.Rows.Add(row);

                                            row = dt4_new.NewRow();
                                            row["Fld_Sequence"] = i + 2;
                                            row["Fld_Details"] = "(B) ITC Reversed";
                                            row["Fld_IGST"] = dt31.Rows[i]["F3"].ToString();
                                            row["Fld_CGST"] = dt31.Rows[i]["F4"].ToString();
                                            row["Fld_SGST"] = dt31.Rows[i]["F5"].ToString();
                                            row["Fld_CESS"] = dt31.Rows[i]["F6"].ToString();
                                            row["Fld_FileStatus"] = "Completed";
                                            dt4_new.Rows.Add(row);
                                        }
                                        if (i == 7)
                                        {
                                            row["Fld_Sequence"] = i + 2;
                                            row["Fld_Details"] = "  (1) As per rules 42 & 43 IGST Rules";
                                            row["Fld_IGST"] = dt31.Rows[i]["F3"].ToString();
                                            row["Fld_CGST"] = dt31.Rows[i]["F4"].ToString();
                                            row["Fld_SGST"] = dt31.Rows[i]["F5"].ToString();
                                            row["Fld_CESS"] = dt31.Rows[i]["F6"].ToString();
                                            dt4_new.Rows.Add(row);
                                        }
                                        if (i == 8)
                                        {
                                            row["Fld_Sequence"] = i + 2;
                                            row["Fld_Details"] = "  (2) Others";
                                            row["Fld_IGST"] = dt31.Rows[i]["F3"].ToString();
                                            row["Fld_CGST"] = dt31.Rows[i]["F4"].ToString();
                                            row["Fld_SGST"] = dt31.Rows[i]["F5"].ToString();
                                            row["Fld_CESS"] = dt31.Rows[i]["F6"].ToString();
                                            dt4_new.Rows.Add(row);
                                        }
                                        if (i == 9)
                                        {
                                            row = dt4_new.NewRow();
                                            row["Fld_Sequence"] = i + 2;
                                            row["Fld_Details"] = "Total ITC Reversed (B)";
                                            row["Fld_IGST"] = "";
                                            row["Fld_CGST"] = "";
                                            row["Fld_SGST"] = "";
                                            row["Fld_CESS"] = "";
                                            row["Fld_FileStatus"] = "Completed";
                                            dt4_new.Rows.Add(row);

                                            row = dt4_new.NewRow();
                                            row["Fld_Sequence"] = i + 3;
                                            row["Fld_Details"] = "(C) Net ITC Available (A) – (B)";
                                            row["Fld_IGST"] = dt31.Rows[i]["F3"].ToString();
                                            row["Fld_CGST"] = dt31.Rows[i]["F4"].ToString();
                                            row["Fld_SGST"] = dt31.Rows[i]["F5"].ToString();
                                            row["Fld_CESS"] = dt31.Rows[i]["F6"].ToString();
                                            row["Fld_FileStatus"] = "Completed";
                                            dt4_new.Rows.Add(row);

                                        }
                                        if (i == 10)
                                        {
                                            row["Fld_Sequence"] = i + 3;
                                            row["Fld_Details"] = "(D) Ineligible ITC";
                                            row["Fld_IGST"] = dt31.Rows[i]["F3"].ToString();
                                            row["Fld_CGST"] = dt31.Rows[i]["F4"].ToString();
                                            row["Fld_SGST"] = dt31.Rows[i]["F5"].ToString();
                                            row["Fld_CESS"] = dt31.Rows[i]["F6"].ToString();
                                            dt4_new.Rows.Add(row);
                                        }
                                        if (i == 11)
                                        {
                                            row["Fld_Sequence"] = i + 3;
                                            row["Fld_Details"] = "  (1) As per section 17(5)";
                                            row["Fld_IGST"] = dt31.Rows[i]["F3"].ToString();
                                            row["Fld_CGST"] = dt31.Rows[i]["F4"].ToString();
                                            row["Fld_SGST"] = dt31.Rows[i]["F5"].ToString();
                                            row["Fld_CESS"] = dt31.Rows[i]["F6"].ToString();
                                            dt4_new.Rows.Add(row);
                                        }
                                        if (i == 12)
                                        {
                                            row["Fld_Sequence"] = i + 3;
                                            row["Fld_Details"] = "  (2) Others";
                                            row["Fld_IGST"] = dt31.Rows[i]["F3"].ToString();
                                            row["Fld_CGST"] = dt31.Rows[i]["F4"].ToString();
                                            row["Fld_SGST"] = dt31.Rows[i]["F5"].ToString();
                                            row["Fld_CESS"] = dt31.Rows[i]["F6"].ToString();
                                            dt4_new.Rows.Add(row);
                                        }
                                        if (i == 12)
                                        {
                                            dt4_new.AcceptChanges();
                                            break;
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region SPQR3BExemptSupply
                            DataTable dt5_new = new DataTable();
                            dt5_new.Columns.Add("Fld_Sequence");
                            dt5_new.Columns.Add("Fld_NatureofSupply");
                            dt5_new.Columns.Add("Fld_InterStateSupplies");
                            dt5_new.Columns.Add("Fld_IntraStateSupplies");
                            dt5_new.Columns.Add("Fld_Month");
                            dt5_new.Columns.Add("Fld_FileStatus");
                            dt5_new.AcceptChanges();
                            if (dt31 != null)
                            {
                                for (int j = 0; j <= 15; j++)
                                {
                                    dt31.Rows.RemoveAt(0);
                                }
                                dt31.AcceptChanges();
                                for (int i = 0; i <= dt31.Rows.Count; i++)
                                {
                                    DataRow row = dt5_new.NewRow();
                                    row["Fld_Sequence"] = i + 1;
                                    row["Fld_FileStatus"] = "Completed";
                                    if (i >= 0 && i <= 2)
                                    {
                                        if (i == 0)
                                        {
                                            row["Fld_NatureofSupply"] = "From a supplier under composition scheme, Exempt and Nil rated Supply";
                                            row["Fld_InterStateSupplies"] = dt31.Rows[i]["F4"].ToString();
                                            row["Fld_IntraStateSupplies"] = dt31.Rows[i]["F5"].ToString();
                                        }
                                        if (i == 1)
                                        {
                                            row["Fld_NatureofSupply"] = "Non GST Supply";
                                            row["Fld_InterStateSupplies"] = dt31.Rows[i]["F4"].ToString();
                                            row["Fld_IntraStateSupplies"] = dt31.Rows[i]["F5"].ToString();
                                        }
                                        if (i == 2)
                                        {
                                            row["Fld_NatureofSupply"] = "Total";
                                            row["Fld_InterStateSupplies"] = dt31.Rows[i]["F4"].ToString();
                                            row["Fld_IntraStateSupplies"] = dt31.Rows[i]["F5"].ToString();
                                            row["Fld_FileStatus"] = "Total";
                                        }
                                    }
                                    dt5_new.Rows.Add(row);
                                    if (i == 2)
                                    {
                                        dt5_new.AcceptChanges();
                                        break;
                                    }
                                }
                            }
                            #endregion

                            #region SPQR3BInterStateSupplies

                            DataTable dt32_new = new DataTable();
                            dt32_new.Columns.Add("Fld_Sequence");
                            dt32_new.Columns.Add("Fld_Details");
                            dt32_new.Columns.Add("Fld_POS");
                            dt32_new.Columns.Add("Fld_Taxable");
                            dt32_new.Columns.Add("Fld_IGST");
                            dt32_new.Columns.Add("Fld_Month");
                            dt32_new.Columns.Add("Fld_FileStatus");
                            dt32_new.AcceptChanges();
                            if (dt31 != null)
                            {
                                for (int j = 0; j <= 33; j++)
                                {
                                    dt31.Rows.RemoveAt(0);
                                }
                                dt31.AcceptChanges();
                                for (int i = 0; i <= dt31.Rows.Count; i++)
                                {
                                    if (i >= 0 && i < dt31.Rows.Count - 1)
                                    {
                                        if (dt31.Rows[i]["F3"].ToString().Trim() != "" && dt31.Rows[i]["F4"].ToString().Trim() != "")
                                        {
                                            DataRow row = dt32_new.NewRow();
                                            row["Fld_Sequence"] = "";
                                            row["Fld_FileStatus"] = "Completed";
                                            row["Fld_Details"] = "Supplies made to Unregistered Persons";
                                            row["Fld_POS"] = Utility.strValidStateName(dt31.Rows[i]["F2"].ToString());
                                            row["Fld_Taxable"] = dt31.Rows[i]["F3"].ToString();
                                            row["Fld_IGST"] = dt31.Rows[i]["F4"].ToString();
                                            dt32_new.Rows.Add(row);
                                        }
                                    }
                                    if (i >= 0 && i < dt31.Rows.Count - 1)
                                    {
                                        if (dt31.Rows[i]["F5"].ToString().Trim() != "" && dt31.Rows[i]["F6"].ToString().Trim() != "")
                                        {
                                            DataRow row = dt32_new.NewRow();
                                            row["Fld_Sequence"] = "";
                                            row["Fld_FileStatus"] = "Completed";
                                            row["Fld_Details"] = "Supplies made to Composition Taxable Persons";
                                            row["Fld_POS"] = Utility.strValidStateName(dt31.Rows[i]["F2"].ToString());
                                            row["Fld_Taxable"] = dt31.Rows[i]["F5"].ToString();
                                            row["Fld_IGST"] = dt31.Rows[i]["F6"].ToString();
                                            dt32_new.Rows.Add(row);
                                        }
                                    }
                                    if (i >= 0 && i < dt31.Rows.Count - 1)
                                    {
                                        if (dt31.Rows[i]["F7"].ToString().Trim() != "" && dt31.Rows[i]["F8"].ToString().Trim() != "")
                                        {
                                            DataRow row = dt32_new.NewRow();
                                            row["Fld_Sequence"] = "";
                                            row["Fld_FileStatus"] = "Completed";
                                            row["Fld_Details"] = "Supplies made to UIN holders";
                                            row["Fld_POS"] = Utility.strValidStateName(dt31.Rows[i]["F2"].ToString());
                                            row["Fld_Taxable"] = dt31.Rows[i]["F7"].ToString();
                                            row["Fld_IGST"] = dt31.Rows[i]["F8"].ToString();
                                            dt32_new.Rows.Add(row);
                                        }
                                    }
                                    if (i == dt31.Rows.Count - 1)
                                    {
                                        DataRow row = dt32_new.NewRow();
                                        row["Fld_Sequence"] = "";
                                        row["Fld_FileStatus"] = "Total";
                                        row["Fld_Details"] = "Total";
                                        row["Fld_POS"] = "";
                                        double x = Convert.ToDouble(dt31.Rows[i]["F3"].ToString()) + Convert.ToDouble(dt31.Rows[i]["F5"].ToString()) + Convert.ToDouble(dt31.Rows[i]["F7"].ToString());
                                        row["Fld_Taxable"] = x;
                                        double y = Convert.ToDouble(dt31.Rows[i]["F4"].ToString()) + Convert.ToDouble(dt31.Rows[i]["F6"].ToString()) + Convert.ToDouble(dt31.Rows[i]["F8"].ToString());
                                        row["Fld_IGST"] = y;
                                        dt32_new.Rows.Add(row);
                                    }
                                }
                            }
                            dt32_new.AcceptChanges();

                            #endregion

                            #endregion

                            #region first delete old data from database

                            int _Result = 0;
                            string Query = "Delete from SPQR3BOutwardSupplies where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                            _Result = objGSTR3B.IUDData(Query);
                            if (_Result != 1)
                                MessageBox.Show("System error.\nPlease try after sometime!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            _Result = 0;
                            Query = "Delete from SPQR3BInterStateSupplies where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                            _Result = objGSTR3B.IUDData(Query);
                            if (_Result != 1)
                                MessageBox.Show("System error.\nPlease try after sometime!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            _Result = 0;
                            Query = "Delete from SPQR3BEligibleITC where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                            _Result = objGSTR3B.IUDData(Query);
                            if (_Result != 1)
                                MessageBox.Show("System error.\nPlease try after sometime!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            _Result = 0;
                            Query = "Delete from SPQR3BExemptSupply where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                            _Result = objGSTR3B.IUDData(Query);
                            if (_Result != 1)
                                MessageBox.Show("System error.\nPlease try after sometime!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            #endregion

                            #region Save

                            #region 3B1

                            int _Result31 = 0;
                            _Result31 = objGSTR3B.GSTR3B1_Gov_Utility_ExcelBulkEntry(dt111, Convert.ToString(CommonHelper.StatusText));
                            if (_Result31 != 1)
                            { _str += "outward-supply data entry error..!\n"; }
                            #endregion

                            #region 3B2

                            int _Result32 = 0;
                            _Result32 = objGSTR3B.GSTR3B2_Gov_Utility_ExcelBulkEntry(dt32_new, Convert.ToString(CommonHelper.StatusText));
                            if (_Result32 != 1)
                            { _str += "URD Composition data entry error..!\n"; }

                            #endregion

                            #region 3B4
                            int _Result4 = 0;
                            _Result4 = objGSTR3B.GSTR3B4_Gov_Utility_ExcelBulkEntry(dt4_new, Convert.ToString(CommonHelper.StatusText));
                            if (_Result4 != 1)
                            { _str += "Eligible-ITC data entry error..!\n"; }
                            #endregion

                            #region 3B5
                            int _Result5 = 0;
                            _Result5 = objGSTR3B.GSTR3B5_Gov_Utility_ExcelBulkEntry(dt5_new, Convert.ToString(CommonHelper.StatusText));
                            if (_Result5 != 1)
                            { _str += "Nilrated-NonGST data entry error..!\n"; }
                            #endregion

                            #endregion

                            pbGSTR1.Visible = false;

                            if (_str != "")
                            {
                                CommonHelper.ErrorList = Convert.ToString(_str);
                                SPQErrorList obj = new SPQErrorList();
                                obj.ShowDialog();
                            }
                            else
                            {
                                MessageBox.Show("Data imported successfully...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Getdata();
                            }
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
                pbGSTR1.Visible = false;
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }
        private void msExpExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (DgvMain.Rows.Count > 1)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID

                    pbGSTR1.Visible = true;

                    #region CREATE WORKBOOK AND ASSIGN COLUMNNAME
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook WB = excelApp.Workbooks.Add(Missing.Value);

                    Excel.Worksheet newWS = (Excel.Worksheet)excelApp.Worksheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    newWS.Name = "GSTR1A";

                    // DELETE UNUSED WORKSHEETS FROM WORKBOOK
                    foreach (Excel.Worksheet ws in WB.Worksheets)
                    {
                        if (ws.Name != "GSTR1A")
                            ((Excel.Worksheet)WB.Worksheets[ws.Name]).Delete();
                    }
                    int temp = 0;
                    // ASSIGN COLUMN HEADER AS PAR THE GRID HEADER
                    for (int i = 1; i < DgvMain.Columns.Count + 1; i++)
                    {
                        if (DgvMain.Columns[i - 1].Visible)
                        {
                            newWS.Cells[1, temp + 1] = DgvMain.Columns[i - 1].HeaderText.ToString();
                            // SET COLUMN WIDTH
                            if (i == 1 || i == 0)
                                ((Excel.Range)newWS.Cells[1, i]).ColumnWidth = 35;
                            else if (i >= 2 && i <= 14)
                                ((Excel.Range)newWS.Cells[1, i]).ColumnWidth = 10;
                            else
                                ((Excel.Range)newWS.Cells[1, i]).ColumnWidth = 15;
                            temp++;
                        }
                    }

                    // GET RANGE AND SET DIFFRENT PROPERTIES
                    Excel.Range headerRange = (Excel.Range)newWS.get_Range((Excel.Range)newWS.Cells[1, 1], (Excel.Range)newWS.Cells[1, DgvMain.Columns.Count]);
                    headerRange.WrapText = true;
                    headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Calibri";
                    #endregion

                    #region COPY DATA FROM DATATABLE TO ARRAY

                    // CREATE ARRAY TO HOLD THE DATA OF DATATABLE
                    object[,] arr = new object[DgvMain.Rows.Count + 7, DgvMain.Columns.Count];

                    // ASSIGN DATA TO ARRAY FROM DATATABLE
                    // FOR LICENECE ALLOWS TO EXPORT ALL RECORDS
                    for (int i = 0; i < DgvMain.Rows.Count; i++)
                    {
                        temp = 0;
                        for (int j = 0; j < DgvMain.Columns.Count; j++)
                        {
                            if (DgvMain.Columns[j].Visible)
                            {
                                arr[i, temp] = Convert.ToString(DgvMain.Rows[i].Cells[j].Value);
                                temp++;
                            }
                        }
                    }
                    //for (int i = DgvMain.Rows.Count; i < DgvMain.Rows.Count + 1; i++)
                    //{
                    //    for (int j = 0; j < dgvtotal.Columns.Count; j++)
                    //    {
                    //        arr[DgvMain.Rows.Count + 1, j] = Convert.ToString(dgvtotal.Rows[0].Cells[j].Value);
                    //    }
                    //}

                    //SET EXCEL RANGE TO PASTE THE DATA
                    Excel.Range top = (Excel.Range)newWS.Cells[2, 1];
                    Excel.Range bottom = (Excel.Range)newWS.Cells[DgvMain.Rows.Count + 7, DgvMain.Columns.Count];
                    Excel.Range sheetRange = newWS.Range[top, bottom];

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
        #endregion

        private void msClose_Click(object sender, EventArgs e)
        {
            SPQCompanyDashboard obj = new SPQCompanyDashboard();
            obj.MdiParent = this.MdiParent;
            Utility.CloseAllOpenForm();
            obj.Dock = DockStyle.Fill;
            obj.Show();
        }

        #region Functional Method

        public bool SetGSPSetting()
        {
            bool flg = false;
            try
            {
                AppCompany objcompany = new AppCompany();

                //SPQUploadPopUp frm = new SPQUploadPopUp();
                //frm.Visible = false;
                //var result = frm.ShowDialog();
                //if (result == DialogResult.OK)
                {
                    DataTable dt = new DataTable();
                    dt = objcompany.GetAPIDetail("select * from SPQGSPApi order by id desc limit 1");

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        Constants.AspClintSecret = Convert.ToString(dt.Rows[0]["AspClintSecret"]);
                        Constants.AspClintID = Convert.ToString(dt.Rows[0]["AspClintID"]);
                        Constants.GspClintSecret = Convert.ToString(dt.Rows[0]["GspClintSecret"]);
                        Constants.GspClintID = Convert.ToString(dt.Rows[0]["GspClintID"]);

                        flg = true;
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

            return flg;
        }

        private Boolean chkCellValue(string cellValue, string sht)
        {
            bool flg = false;
            try
            {
                if (sht == "B31Nature")
                {
                    if (cellValue != "Supplies made to Unregistered Persons" && cellValue != "Supplies made to Composition Taxable Persons" && cellValue != "Supplies made to UIN holders")
                        flg = true;
                }
            }
            catch
            {
                flg = false;
            }

            return flg;
        }

        #endregion

        #region GST Method
        public bool saveGSTR3BOption(string jsonOption)
        {
            bool flag;
            try
            {
                flag = false;

                #region get summary
                this.Cc = clsPro.Cooki != null ? ((clsPro.Cooki.FirstOrDefault(x => x.ckname == "loginCookies_" + CommonHelper.CompanyID) != null ? clsPro.Cooki.FirstOrDefault(x => x.ckname == "loginCookies_" + CommonHelper.CompanyID).CC1 : null)) : null; //(CookieContainer)HttpContext.Current.Session["loginCookies_0"];
                string[] strArrays = clssummary.ReturnDate();
                string str = strArrays[0];
                string str1 = strArrays[1];
                string assesseeDetails = clssummary.GetAssesseeDetails()[0];
                CommonHelper.ReturnMonthYearStr = string.Concat(str1, str);
                HttpWebRequest httpWebRequest = this.PrepareGetRequest(new Uri(this.DashboardPage), this.GstLoginPage, this.Cc);
                this.response = (HttpWebResponse)httpWebRequest.GetResponse();
                httpWebRequest = this.PrepareGetRequest(new Uri(string.Concat("https://return.gst.gov.in/returns/auth/api/gstr3b/summary?rtn_prd=" + CommonHelper.ReturnMonthYearStr + "") ?? ""), "https://return.gst.gov.in/returns/auth/gstr3b");
                this.response = (HttpWebResponse)httpWebRequest.GetResponse();
                this.responseStream = this.response.GetResponseStream();
                this.responseStreamReader = new StreamReader(this.responseStream, Encoding.UTF8);
                string summjsonStr = this.responseStreamReader.ReadToEnd();
                RootObjectJson outwordSummary = JsonConvert.DeserializeObject<RootObjectJson>(summjsonStr);
                #endregion

                if (outwordSummary.status == 1)
                {
                    if (outwordSummary.data != null && outwordSummary.data.qn == null)
                    {
                        SPEQTAGST.BAL.V019js.GSTR3BJson.OptionRootObject objOpt = JsonConvert.DeserializeObject<SPEQTAGST.BAL.V019js.GSTR3BJson.OptionRootObject>(jsonOption);

                        if (objOpt.qn != null)
                        {
                            Qn objqn = new Qn();
                            objqn.q1 = Convert.ToString(objOpt.qn.q1);
                            objqn.q2 = Convert.ToString(objOpt.qn.q2);
                            objqn.q3 = Convert.ToString(objOpt.qn.q3);
                            objqn.q4 = Convert.ToString(objOpt.qn.q4);
                            objqn.q5 = Convert.ToString(objOpt.qn.q5);
                            objqn.q6 = Convert.ToString(objOpt.qn.q6);
                            objqn.q7 = Convert.ToString(objOpt.qn.q7);
                            outwordSummary.data.qn = objqn;
                        }
                    }
                    if (outwordSummary.data != null && outwordSummary.data.intr_ltfee != null && outwordSummary.data.intr_ltfee.intr_details == null)
                    {
                        IntrDetails objIntrDetails = new IntrDetails();
                        objIntrDetails.iamt = 0;
                        objIntrDetails.camt = 0;
                        objIntrDetails.samt = 0;
                        objIntrDetails.csamt = 0;

                        outwordSummary.data.intr_ltfee.intr_details = objIntrDetails;
                    }

                    summjsonStr = JsonConvert.SerializeObject(outwordSummary.data);

                    httpWebRequest = this.GSTR3BPostRequest(new Uri("https://return.gst.gov.in/returns/auth/api/gstr3b/save"), "https://return.gst.gov.in/returns/auth/gstr3b", this.Cc, CommonHelper.ReturnMonthYearStr, summjsonStr);
                    // httpWebRequest = this.GSTR3BPostRequest(new Uri("https://return.gst.gov.in/returns/auth/api/gstr3b/save"), "https://return.gst.gov.in/returns/auth/gstr3b", this.Cc, CommonHelper.ReturnMonthYearStr, jsonOption);
                    this.response = (HttpWebResponse)httpWebRequest.GetResponse();
                    this.responseStream = this.response.GetResponseStream();
                    this.responseStreamReader = new StreamReader(this.responseStream, Encoding.UTF8);
                    string jsonStr = this.responseStreamReader.ReadToEnd();
                    StatusOfGSTR3BModel statusOfGSTR3BModel = JsonConvert.DeserializeObject<StatusOfGSTR3BModel>(jsonStr);


                    if (statusOfGSTR3BModel.message == "REC" || statusOfGSTR3BModel.message == "IP")
                    {
                        Thread.Sleep(5000);

                        httpWebRequest = this.PrepareGetRequest(new Uri(string.Concat("https://return.gst.gov.in/returns/auth/api/gstr3b/getTxnStatus?gstin=" + CommonHelper.CompanyGSTN + "&retPeriod=" + CommonHelper.ReturnMonthYearStr + "") ?? ""), "https://return.gst.gov.in/returns/auth/gstr3b");
                        this.response = (HttpWebResponse)httpWebRequest.GetResponse();
                        this.responseStream = this.response.GetResponseStream();
                        this.responseStreamReader = new StreamReader(this.responseStream, Encoding.UTF8);
                        string TxnjsonStr = this.responseStreamReader.ReadToEnd();
                        StatusOfGSTR3BModel TxnstatusOfGSTR3BModel = JsonConvert.DeserializeObject<StatusOfGSTR3BModel>(TxnjsonStr);
                    }
                    else if (statusOfGSTR3BModel.message == "P")
                    {
                        flag = true;
                        this.saveTransIdVal = statusOfGSTR3BModel.transId;
                    }
                    else if (statusOfGSTR3BModel.message == "INIT")
                    {
                        flag = false;
                        this.getError = "Error Code: INIT \n Message: You already have Submitted/Filed For Current Return Period...!";
                    }
                    else if (statusOfGSTR3BModel.message != "ER")
                    {
                        flag = false;
                        this.getError = "Some error occured please check ur Connection/Data and try again";
                    }
                    else
                    {
                        flag = false;
                        this.getError = string.Concat("Error Saving Data to GSTN transaction Id = ", statusOfGSTR3BModel.transId, ". Please review your data and try again.");
                    }
                }
                else
                {
                    flag = false;
                    this.getError = "Something seems to have gone wrong while processing your request. Please try again";
                }

            }
            catch (Exception exception1)
            {
                if (exception1.Message.Contains("403"))
                {
                    SPQGstLogin frm = new SPQGstLogin();
                    frm.Visible = false;
                    var result = frm.ShowDialog();
                    if (result != DialogResult.OK)
                    {
                        flag = false;
                    }
                    else
                    {
                        saveGSTR3BOption(jsonOption);
                        flag = true;
                    }
                }
                else
                {
                    this.getError = string.Concat("Some error occured please check ur Connection/Data and try again", exception1.Message);
                    flag = false;
                }
            }
            return flag;
        }

        public bool saveGSTR3B(string strjson)
        {
            bool flag;
            try
            {
                this.Cc = clsPro.Cooki != null ? ((clsPro.Cooki.FirstOrDefault(x => x.ckname == "loginCookies_" + CommonHelper.CompanyID) != null ? clsPro.Cooki.FirstOrDefault(x => x.ckname == "loginCookies_" + CommonHelper.CompanyID).CC1 : null)) : null; //(CookieContainer)HttpContext.Current.Session["loginCookies_0"];
                string[] strArrays = clssummary.ReturnDate();
                string str = strArrays[0];
                string str1 = strArrays[1];
                string assesseeDetails = clssummary.GetAssesseeDetails()[0];
                CommonHelper.ReturnMonthYearStr = string.Concat(str1, str);
                HttpWebRequest httpWebRequest = this.PrepareGetRequest(new Uri(this.DashboardPage), this.GstLoginPage, this.Cc);
                this.response = (HttpWebResponse)httpWebRequest.GetResponse();
                httpWebRequest = this.GSTR3BPostRequest(new Uri("https://return.gst.gov.in/returns/auth/gstr3b"), "https://return.gst.gov.in/returns/auth/dashboard", this.Cc, string.Concat(str1, str), strjson);
                this.response = (HttpWebResponse)httpWebRequest.GetResponse();
                httpWebRequest = this.GSTR3BPostRequest(new Uri("https://return.gst.gov.in/returns/auth/api/gstr3b/save"), "https://return.gst.gov.in/returns/auth/gstr3b", this.Cc, string.Concat(str1, str), strjson);
                this.response = (HttpWebResponse)httpWebRequest.GetResponse();
                httpWebRequest = this.PrepareGetRequest(new Uri(string.Format("https://return.gst.gov.in/returns/auth/api/gstr3b/getTxnStatus?gstin={0}&retPeriod={1}", assesseeDetails, string.Concat(str1, str))), "https://return.gst.gov.in/returns/auth/gstr3b", this.Cc);
                this.response = (HttpWebResponse)httpWebRequest.GetResponse();
                this.responseStream = this.response.GetResponseStream();
                this.responseStreamReader = new StreamReader(this.responseStream, Encoding.UTF8);
                string jsonStr = this.responseStreamReader.ReadToEnd();
                StatusOfGSTR3BModel statusOfGSTR3BModel = JsonConvert.DeserializeObject<StatusOfGSTR3BModel>(jsonStr);
                if (statusOfGSTR3BModel.message == "P")
                {
                    flag = true;
                    this.saveTransIdVal = statusOfGSTR3BModel.transId;
                    #region FilingLog
                    string StatusCode = "";
                    string Status = "0";
                    string ErrorMsg = "";
                    string RefrenceID = "";

                    if (statusOfGSTR3BModel.message.ToString().Trim() == null)
                        StatusCode = "";
                    else
                        StatusCode = statusOfGSTR3BModel.message.ToString();

                    //if (statusOfGSTR3BModel.message == null)
                    //    Status = "";
                    //else
                    //    Status = statusOfGSTR3BModel.message;

                    if (statusOfGSTR3BModel.status_cd == null)
                        ErrorMsg = "";
                    else
                        ErrorMsg = statusOfGSTR3BModel.status_cd;

                    if (statusOfGSTR3BModel.transId == null)
                        RefrenceID = "";
                    else
                        RefrenceID = statusOfGSTR3BModel.transId;

                    Utility.FileingLogs("", "", CommonHelper.CompanyID, CommonHelper.CompanyGSTN, CommonHelper.SelectedMonth, string.Concat(str1, str), "GSTR-3B", "Save To GSTIN", System.DateTime.Now.ToString(), RefrenceID, StatusCode, Status, this.getError, ErrorMsg);
                    #endregion
                }
                else if (statusOfGSTR3BModel.message == "INIT")
                {
                    flag = false;
                    this.getError = "Error Code: INIT \n Message: You already have Submitted/Filed For Current Return Period...!";

                    #region FilingLog
                    string StatusCode = "";
                    string Status = "0";
                    string ErrorMsg = "";
                    string RefrenceID = "";

                    if (statusOfGSTR3BModel.message.ToString().Trim() == null)
                        StatusCode = "";
                    else
                        StatusCode = statusOfGSTR3BModel.message.ToString();

                    //if (statusOfGSTR3BModel.message == null)
                    //    Status = "";
                    //else
                    //    Status = statusOfGSTR3BModel.message;

                    if (statusOfGSTR3BModel.status_cd == null)
                        ErrorMsg = "";
                    else
                        ErrorMsg = statusOfGSTR3BModel.status_cd;

                    if (statusOfGSTR3BModel.transId == null)
                        RefrenceID = "";
                    else
                        RefrenceID = statusOfGSTR3BModel.transId;

                    Utility.FileingLogs("", "", CommonHelper.CompanyID, CommonHelper.CompanyGSTN, CommonHelper.SelectedMonth, string.Concat(str1, str), "GSTR-3B", "Save To GSTIN", System.DateTime.Now.ToString(), RefrenceID, StatusCode, Status, this.getError, ErrorMsg);
                    #endregion
                }
                else if (statusOfGSTR3BModel.message != "ER")
                {
                    this.getError = "Some error occured please check ur Connection/Data and try again";
                    flag = false;

                    #region FilingLog
                    string StatusCode = "";
                    string Status = "0";
                    string ErrorMsg = "";
                    string RefrenceID = "";

                    if (statusOfGSTR3BModel.message.ToString().Trim() == null)
                        StatusCode = "";
                    else
                        StatusCode = statusOfGSTR3BModel.message.ToString();

                    //if (statusOfGSTR3BModel.message == null)
                    //    Status = "";
                    //else
                    //    Status = statusOfGSTR3BModel.message;

                    if (statusOfGSTR3BModel.status_cd == null)
                        ErrorMsg = "";
                    else
                        ErrorMsg = statusOfGSTR3BModel.status_cd;

                    if (statusOfGSTR3BModel.transId == null)
                        RefrenceID = "";
                    else
                        RefrenceID = statusOfGSTR3BModel.transId;

                    Utility.FileingLogs("", "", CommonHelper.CompanyID, CommonHelper.CompanyGSTN, CommonHelper.SelectedMonth, string.Concat(str1, str), "GSTR-3B", "Save To GSTIN", System.DateTime.Now.ToString(), RefrenceID, StatusCode, Status, this.getError, ErrorMsg);
                    #endregion
                }
                else
                {
                    this.getError = string.Concat("Error Saving Data to GSTN transaction Id = ", statusOfGSTR3BModel.transId, ". Please review your data and try again.");
                    flag = false;

                    #region FilingLog
                    string StatusCode = "";
                    string Status = "0";
                    string ErrorMsg = "";
                    string RefrenceID = "";

                    if (statusOfGSTR3BModel.message.ToString().Trim() == null)
                        StatusCode = "";
                    else
                        StatusCode = statusOfGSTR3BModel.message.ToString();

                    //if (statusOfGSTR3BModel.message == null)
                    //    Status = "";
                    //else
                    //    Status = statusOfGSTR3BModel.message;

                    if (statusOfGSTR3BModel.status_cd == null)
                        ErrorMsg = "";
                    else
                        ErrorMsg = statusOfGSTR3BModel.status_cd;

                    if (statusOfGSTR3BModel.transId == null)
                        RefrenceID = "";
                    else
                        RefrenceID = statusOfGSTR3BModel.transId;

                    Utility.FileingLogs("", "", CommonHelper.CompanyID, CommonHelper.CompanyGSTN, CommonHelper.SelectedMonth, string.Concat(str1, str), "GSTR-3B", "Save To GSTIN", System.DateTime.Now.ToString(), RefrenceID, StatusCode, Status, this.getError, ErrorMsg);
                    #endregion
                }
            }
            catch (Exception exception1)
            {
                if (exception1.Message.Contains("403"))
                {
                    SPQGstLogin frm = new SPQGstLogin();
                    frm.Visible = false;
                    var result = frm.ShowDialog();
                    if (result != DialogResult.OK)
                    {
                        flag = false;
                    }
                    else
                    {
                        saveGSTR3B(strjson);
                        flag = true;
                    }
                }
                else
                {
                    this.getError = string.Concat("Some error occured please check ur Connection/Data and try again", exception1.Message);
                    flag = false;
                }
            }
            return flag;
        }

        protected HttpWebRequest GSTR3BPostRequest(Uri uri, string referer, CookieContainer ccc, string ReturnPeriod, string strjson)
        {
            HttpWebRequest httpWebRequest;
            try
            {
                HttpWebRequest cc = (HttpWebRequest)WebRequest.Create(uri);
                cc.CookieContainer = ccc;
                cc.Method = "POST";
                if (referer != null)
                {
                    cc.Referer = referer;
                }
                cc.Accept = "application/json, text/plain, */*";
                cc.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
                cc.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                cc.Headers.Add("Origin", "https://return.gst.gov.in");
                cc.KeepAlive = true;
                cc.Host = "return.gst.gov.in";
                cc.ContentType = "application/json;charset=UTF-8";
                using (StreamWriter streamWriter = new StreamWriter(cc.GetRequestStream()))
                {
                    string end = "";
                    if (uri.ToString().Contains("returns/auth/gstr3b"))
                    {
                        end = string.Concat("{\"RTN_PRD\"=\"", ReturnPeriod, "\"}");
                    }
                    else if (uri.ToString().Contains("returns/auth/api/gstr3b/save"))
                    {
                        end = strjson;
                        //try
                        //{
                        //    StreamReader streamReader = new StreamReader(string.Concat(new string[] { this.jsonPath, AssesseeId, "_", ReturnPeriod, ".json" }));
                        //    end = streamReader.ReadToEnd();
                        //    streamReader.Close();
                        //}
                        //catch (Exception exception)
                        //{
                        //    this.getError = string.Concat("Error Reading Json File", exception.Message);
                        //}
                        if (end == null)
                        {
                            this.getError = "Json Payload is Empty Error";
                            httpWebRequest = null;
                            return httpWebRequest;
                        }
                    }
                    streamWriter.Write(end);
                }
                httpWebRequest = cc;
            }
            catch (Exception exception1)
            {
                this.getError = string.Concat("Error in requesting to server", exception1.Message);
                httpWebRequest = null;
            }
            return httpWebRequest;
        }
        protected HttpWebRequest PrepareGetRequest(Uri uri, string referer, CookieContainer ccc)
        {
            HttpWebRequest httpWebRequest;
            try
            {
                HttpWebRequest cc = (HttpWebRequest)WebRequest.Create(uri);
                cc.CookieContainer = ccc;
                cc.KeepAlive = true;
                cc.Method = "GET";
                if (uri.ToString().Contains("registration/auth/"))
                {
                    cc.Host = "enroll.gst.gov.in";
                }
                else if (uri.ToString().Contains("payment.gst.gov.in/"))
                {
                    cc.Host = "payment.gst.gov.in";
                }
                else if (!uri.ToString().Contains("return.gst.gov.in/"))
                {
                    cc.Host = "services.gst.gov.in";
                }
                else
                {
                    cc.Host = "return.gst.gov.in";
                }
                if (referer != null)
                {
                    cc.Referer = referer;
                }
                else if (referer == null)
                {
                    cc.Headers.Add("Upgrade-Insecure-Requests", "1");
                }
                cc.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                cc.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
                cc.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                httpWebRequest = cc;
            }
            catch (Exception exception)
            {
                this.getError = "Error in requesting to server";
                httpWebRequest = null;
            }
            return httpWebRequest;
        }

        #endregion

        private void btnSaveToGSTN_Click(object sender, EventArgs e)
        {
            panSelectOption.Visible = true;
            panSelectOption.BringToFront();
        }

        #region Extra Events

        private void DgvMain_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridViewCell cell = DgvMain.Rows[e.RowIndex].Cells[e.ColumnIndex];
            //if (cell.Value.ToString().Trim()== "Completed" || cell.Value.ToString().Trim()== "Not-Completed" || cell.Value.ToString().Trim()== "Draft")
            //{
            //   // cell.Value = "Completed";
            //    e.CellStyle.ForeColor = Color.Green;
            //}
            if (cell.Value.ToString().Trim() == "Completed")
            {
                e.CellStyle.ForeColor = Color.Green;
            }
            else if (cell.Value.ToString().Trim() == "Not-Completed")
            {
                e.CellStyle.ForeColor = Color.Red;
            }
            else if (cell.Value.ToString().Trim() == "Draft")
            {
                e.CellStyle.ForeColor = Color.Blue;
            }
        }

        private void dgvtotal_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.DgvMain.ClearSelection();
            this.dgvtotal.ClearSelection();
        }

        private void DgvMain_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DgvMain.Columns["Document Count"].Visible = false;
            this.DgvMain.ClearSelection();
            this.dgvtotal.ClearSelection();
        }

        #endregion

        #region Mouse Event
        private void btnSave_MouseEnter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.BackColor = Color.FromArgb(21, 66, 139);
            btn.ForeColor = Color.White;
        }

        private void btnSave_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.BackColor = Color.FromArgb(23, 196, 187);
            btn.ForeColor = Color.FromArgb(21, 66, 139);
        }
        #endregion

        #region GSP Class
        public class RootObject
        {
            public string RecordId { get; set; }
            public string ipuser { get; set; }
            public string statecd { get; set; }
            public string txn { get; set; }
            public string AppKey { get; set; }
            public string AuthToken { get; set; }
            public string gstin { get; set; }
            public string GSTINUserName { get; set; }
            public string Datetime { get; set; }
            public string Decipher { get; set; }
            public string SEK { get; set; }
            public string InsertDate { get; set; }
            public string UpdateDate { get; set; }
            public string DeleteDate { get; set; }
            public string IsDeleted { get; set; }
        }

        public class GSPResClass
        {
            public string Message { get; set; }
            public string Result { get; set; }
            public string Status { get; set; }
        }
        #endregion

        private void msImpFromGSP_Click(object sender, EventArgs e)
        {
            pbGSTR1.Visible = true;
            getGSTR3BSummary();
            pbGSTR1.Visible = false;


        }

        protected bool getGSTR3BSummary()
        {
            string gstr1summary = "";
            bool flag = true;
            string assesseeIdCookies = null;
            try
            {
                if (Utility.CheckNet())
                {
                    var objcc = clsPro.Cooki != null ? clsPro.Cooki.FirstOrDefault(x => x.ckname == (string.Concat("loginCookies_", CommonHelper.CompanyID))) : null;

                    if (objcc != null && objcc.CC1 != null)
                    {
                        this.Cc = objcc.CC1;
                        string[] strArrays = clssummary.ReturnDate();
                        string str = strArrays[0];
                        string str1 = string.Concat(strArrays[1], str);
                        //string assesseeDetails = clssummary.GetAssesseeDetails(assesseeIdCookies)[0];
                        HttpWebRequest httpWebRequest = this.PrepareGetRequest(new Uri(string.Concat("https://return.gst.gov.in/returns/auth/api/gstr3b/summary?rtn_prd=", str1)), "https://return.gst.gov.in/returns/auth/gstr3b");
                        this.response = (HttpWebResponse)httpWebRequest.GetResponse();
                        Stream responseStream = this.response.GetResponseStream();
                        StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
                        gstr1summary = streamReader.ReadToEnd();
                        RootObjectJson outwordSummary = JsonConvert.DeserializeObject<RootObjectJson>(gstr1summary);

                        if (outwordSummary.status == 1)
                        {

                            JsonImportMethod(gstr1summary);

                            httpWebRequest = this.PrepareGetRequest(new Uri(string.Concat("https://return.gst.gov.in/returns/auth/api/gstr3b/taxpayble?rtn_prd=", str1)), "https://return.gst.gov.in/returns/auth/gstr3b/payment");
                            this.response = (HttpWebResponse)httpWebRequest.GetResponse();
                            responseStream = this.response.GetResponseStream();
                            streamReader = new StreamReader(responseStream, Encoding.UTF8);
                            string reply = streamReader.ReadToEnd();
                            RootObjectReport taxpay = JsonConvert.DeserializeObject<RootObjectReport>(reply);
                            if (taxpay.status == 1)
                            {
                                JsonImportMethodForPaymentofTax(reply);
                                return true;
                            }
                            else
                                return false;

                            flag = true;
                            return flag;
                        }
                        else
                        {
                            MessageBox.Show("System error.\nSomething went wrong with return type!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            flag = false;
                        }
                    }
                    else
                    {
                        SPQGstLogin frm = new SPQGstLogin();
                        frm.Visible = false;
                        var result = frm.ShowDialog();
                        if (result != DialogResult.OK)
                        {
                            //GstLogin objLogin = new GstLogin();
                            //objLogin.Show();
                        }
                        else
                        {
                            getGSTR3BSummary();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("It Seems Your Internet Conection is Not Available, Please Connect Internet…!!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                string str2 = "";
                if (gstr1summary.Contains("Server Busy"))
                {
                    str2 = "due to GST Server Busy";
                }
                else if (exception.Message.Contains("403"))
                {
                    SPQGstLogin frm = new SPQGstLogin();
                    frm.Visible = false;
                    var result = frm.ShowDialog();
                    if (result != DialogResult.OK)
                    {
                    }
                    else
                    {
                        getGSTR3BSummary();
                    }
                }
                else if (!exception.Message.Contains("403"))
                {
                    //this.getError = string.Concat("Some error occured please check Your Data Connection and try again", str2);
                    flag = false;
                }
            }
            return flag;
        }
        protected HttpWebRequest PrepareGetRequest(Uri uri, string referer)
        {
            HttpWebRequest httpWebRequest;
            try
            {
                HttpWebRequest cc = (HttpWebRequest)WebRequest.Create(uri);
                cc.CookieContainer = this.Cc;
                cc.KeepAlive = true;
                cc.Method = "GET";
                if (uri.ToString().Contains("registration/auth/"))
                {
                    cc.Host = "enroll.gst.gov.in";
                }
                else if (uri.ToString().Contains("payment.gst.gov.in/"))
                {
                    cc.Host = "payment.gst.gov.in";
                }
                else if (!uri.ToString().Contains("return.gst.gov.in/"))
                {
                    cc.Host = "services.gst.gov.in";
                }
                else
                {
                    cc.Host = "return.gst.gov.in";
                }
                if (referer != null)
                {
                    cc.Referer = referer;
                }
                else if (referer == null)
                {
                    cc.Headers.Add("Upgrade-Insecure-Requests", "1");
                }
                cc.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                cc.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
                cc.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                httpWebRequest = cc;
            }
            catch (Exception exception)
            {
                //this.getError = "Error in requesting to server";
                httpWebRequest = null;
            }
            return httpWebRequest;
        }

        public void JsonImportMethod(string jsonString)
        {
            string _str = string.Empty;
            int _Result = 0;
            DataTable dt = new DataTable();

            #region first delete old data from database
            string Query = "Delete from SPQR3BOutwardSupplies where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
            _Result = objGSTR3B.IUDData(Query);
            if (_Result != 1)
            {
                MessageBox.Show("System error.\nPlease try after sometime - SPQR3BOutwardSupplies!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Query = "Delete from SPQR3BInterStateSupplies where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
            _Result = objGSTR3B.IUDData(Query);
            if (_Result != 1)
            {
                MessageBox.Show("System error.\nPlease try after sometime - SPQR3BInterStateSupplies!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Query = "Delete from SPQR3BEligibleITC where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
            _Result = objGSTR3B.IUDData(Query);
            if (_Result != 1)
            {
                MessageBox.Show("System error.\nPlease try after sometime - SPQR3BEligibleITC!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Query = "Delete from SPQR3BExemptSupply where Fld_Month='" + CommonHelper.SelectedMonth + "'";
            _Result = objGSTR3B.IUDData(Query);
            if (_Result != 1)
            {
                MessageBox.Show("System error.\nPlease try after sometime - SPQR3BExemptSupply!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            #endregion

            if (Convert.ToString(jsonString).Trim() != "")
            {
                bool flg = false;
                #region Form1
                flg = Form1Entry(jsonString);
                if (flg == false)
                    _str += "SPQR3BOutwardSupplies data entry error...\n";
                #endregion

                #region Form2
                if (flg)
                {
                    flg = Form2Entry(jsonString);
                    if (flg == false)
                        _str += "GSTR3BForm2 data entry error...\n";
                }
                #endregion

                #region Form4
                if (flg)
                {
                    flg = Form4Entry(jsonString);
                    if (flg == false)
                        _str += "GSTR3BForm4 data entry error...\n";
                }
                #endregion

                #region Form5
                if (flg)
                {
                    flg = Form5Entry(jsonString);
                    if (flg == false)
                        _str += "GSTR3BForm5 data entry error...\n";
                }
                #endregion
            }

            if (_str != "")
            {
                Getdata();
                CommonHelper.ErrorList = Convert.ToString(_str);
                SPQErrorList obje = new SPQErrorList();
                obje.ShowDialog();
            }
            //else
            //{
            //    Getdata();
            //    MessageBox.Show("Data saved successfully..", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }
        public bool JsonImportMethodForPaymentofTax(string JsonString)
        {
            try
            {
                string _str = string.Empty;
                int _Result = 0;
                DataTable dt = new DataTable();
                RootObjectReport obj = JsonConvert.DeserializeObject<RootObjectReport>(JsonString);

                #region first delete old data from database
                string Query = "Delete from SPQR3BTaxPayment where Fld_Month='" + CommonHelper.SelectedMonth + "' and Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                _Result = objGSTR3B.IUDData(Query);
                if (_Result != 1)
                {
                    MessageBox.Show("System error.\nPlease try after sometime - Payment of Tax!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                #endregion

                #region ASSIGN GRIDVIEW ROWS IN DATATABLE For SPQR3BOutwardSupplies

                #region Add Rows & Columns
                dt = new DataTable();
                dt.Columns.Add("Fld_Sequence");
                dt.Columns.Add("Fld_Description");
                dt.Columns.Add("Fld_OtRcTaxPay");
                dt.Columns.Add("Fld_IGST");
                dt.Columns.Add("Fld_CGST");
                dt.Columns.Add("Fld_SGST");
                dt.Columns.Add("Fld_CESS");
                dt.Columns.Add("Fld_OtRcTaxPayCash");
                dt.Columns.Add("Fld_RcTaxPay");
                dt.Columns.Add("Fld_RcTaxPayCash");
                dt.Columns.Add("Fld_InterestPay");
                dt.Columns.Add("Fld_InterestPayCash");
                dt.Columns.Add("Fld_LateFeePay");
                dt.Columns.Add("Fld_LateFeePayCash");
                dt.Columns.Add("Fld_UtilizableCash");
                dt.Columns.Add("Fld_AdditionalCash");
                dt.Columns.Add("Fld_FileStatus");
                dt.Columns.Add("Fld_FinancialYear");
                dt.Columns.Add("Fld_Month");

                dt.AcceptChanges();
                DataRow dr = dt.NewRow();
                dr[1] = "Integrated Tax";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[1] = "Central Tax";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[1] = "State/UT Tax";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[1] = "Cess";
                dt.Rows.Add(dr);
                #endregion

                if (obj != null)
                {
                    if (obj.data != null)
                    {
                        if (obj.data.returnsDbCdredList != null)
                        {
                            #region Tax Payable
                            if (obj.data.returnsDbCdredList.tax_pay != null)
                            {
                                #region Tax Payable IGST
                                if (obj.data.returnsDbCdredList.tax_pay.Count > 0)
                                {
                                    if (obj.data.returnsDbCdredList.tax_pay[0].igst != null)
                                    {
                                        if (obj.data.returnsDbCdredList.tax_pay[0].igst.tx != null)
                                        {
                                            dt.Rows[0]["Fld_OtRcTaxPay"] = obj.data.returnsDbCdredList.tax_pay[0].igst.tx;
                                        }
                                        if (obj.data.returnsDbCdredList.tax_pay[0].igst.intr != null)
                                        {
                                            dt.Rows[0]["Fld_InterestPay"] = obj.data.returnsDbCdredList.tax_pay[0].igst.intr;
                                        }
                                        //if (obj.data.returnsDbCdredList.tax_pay[0].igst.fee != null)
                                        //{
                                        //    dt.Rows[0]["Fld_LateFeePay"] = obj.data.returnsDbCdredList.tax_pay[0].igst.fee;
                                        //}
                                    }
                                }
                                else
                                {
                                    dt.Rows[0]["Fld_OtRcTaxPay"] = "0";
                                    dt.Rows[0]["Fld_InterestPay"] = "0";
                                }
                                if (obj.data.returnsDbCdredList.tax_pay.Count > 1)
                                {
                                    if (obj.data.returnsDbCdredList.tax_pay[1].igst != null)
                                    {
                                        if (obj.data.returnsDbCdredList.tax_pay[1].igst.tx != null)
                                        {
                                            dt.Rows[0]["Fld_RcTaxPay"] = obj.data.returnsDbCdredList.tax_pay[1].igst.tx;
                                        }
                                    }
                                }
                                else
                                {
                                    dt.Rows[0]["Fld_RcTaxPay"] = "0";
                                }
                                #endregion

                                #region Tax Payable CGST
                                if (obj.data.returnsDbCdredList.tax_pay.Count > 0)
                                {
                                    if (obj.data.returnsDbCdredList.tax_pay[0].cgst != null)
                                    {
                                        if (obj.data.returnsDbCdredList.tax_pay[0].cgst.tx != null)
                                        {
                                            dt.Rows[1]["Fld_OtRcTaxPay"] = obj.data.returnsDbCdredList.tax_pay[0].cgst.tx;
                                        }
                                        if (obj.data.returnsDbCdredList.tax_pay[0].cgst.intr != null)
                                        {
                                            dt.Rows[1]["Fld_InterestPay"] = obj.data.returnsDbCdredList.tax_pay[0].cgst.intr;
                                        }
                                        if (obj.data.returnsDbCdredList.tax_pay[0].cgst.fee != null)
                                        {
                                            dt.Rows[1]["Fld_LateFeePay"] = obj.data.returnsDbCdredList.tax_pay[0].cgst.fee;
                                        }
                                    }
                                }
                                else
                                {
                                    dt.Rows[1]["Fld_OtRcTaxPay"] = "0";
                                    dt.Rows[1]["Fld_InterestPay"] = "0";
                                    dt.Rows[1]["Fld_LateFeePay"] = "0";
                                }
                                if (obj.data.returnsDbCdredList.tax_pay.Count > 1)
                                {
                                    if (obj.data.returnsDbCdredList.tax_pay[1].cgst != null)
                                    {
                                        if (obj.data.returnsDbCdredList.tax_pay[1].cgst.tx != null)
                                        {
                                            dt.Rows[1]["Fld_RcTaxPay"] = obj.data.returnsDbCdredList.tax_pay[1].cgst.tx;
                                        }
                                    }
                                }
                                else
                                {
                                    dt.Rows[1]["Fld_RcTaxPay"] = "0";
                                }
                                #endregion

                                #region Tax Payable SGST
                                if (obj.data.returnsDbCdredList.tax_pay.Count > 0)
                                {
                                    if (obj.data.returnsDbCdredList.tax_pay[0].sgst != null)
                                    {
                                        if (obj.data.returnsDbCdredList.tax_pay[0].sgst.tx != null)
                                        {
                                            dt.Rows[2]["Fld_OtRcTaxPay"] = obj.data.returnsDbCdredList.tax_pay[0].sgst.tx;
                                        }
                                        if (obj.data.returnsDbCdredList.tax_pay[0].sgst.intr != null)
                                        {
                                            dt.Rows[2]["Fld_InterestPay"] = obj.data.returnsDbCdredList.tax_pay[0].sgst.intr;
                                        }
                                        if (obj.data.returnsDbCdredList.tax_pay[0].sgst.fee != null)
                                        {
                                            dt.Rows[2]["Fld_LateFeePay"] = obj.data.returnsDbCdredList.tax_pay[0].sgst.fee;
                                        }
                                    }
                                }
                                else
                                {
                                    dt.Rows[2]["Fld_OtRcTaxPay"] = "0";
                                    dt.Rows[2]["Fld_InterestPay"] = "0";
                                    dt.Rows[2]["Fld_LateFeePay"] = "0";
                                }
                                if (obj.data.returnsDbCdredList.tax_pay.Count > 1)
                                {
                                    if (obj.data.returnsDbCdredList.tax_pay[1].sgst != null)
                                    {
                                        if (obj.data.returnsDbCdredList.tax_pay[1].sgst.tx != null)
                                        {
                                            dt.Rows[2]["Fld_RcTaxPay"] = obj.data.returnsDbCdredList.tax_pay[1].sgst.tx;
                                        }
                                    }
                                }
                                else
                                {
                                    dt.Rows[2]["Fld_RcTaxPay"] = "0";
                                }
                                #endregion

                                #region Tax Payable CESS
                                if (obj.data.returnsDbCdredList.tax_pay.Count > 0)
                                {
                                    if (obj.data.returnsDbCdredList.tax_pay[0].cess != null)
                                    {
                                        if (obj.data.returnsDbCdredList.tax_pay[0].cess.tx != null)
                                        {
                                            dt.Rows[3]["Fld_OtRcTaxPay"] = obj.data.returnsDbCdredList.tax_pay[0].cess.tx;
                                        }
                                        if (obj.data.returnsDbCdredList.tax_pay[0].cess.intr != null)
                                        {
                                            dt.Rows[3]["Fld_InterestPay"] = obj.data.returnsDbCdredList.tax_pay[0].cess.intr;
                                        }
                                        //if (obj.data.returnsDbCdredList.tax_pay[0].cess.fee != null)
                                        //{
                                        //    dt.Rows[3]["Fld_LateFeePay"] = obj.data.returnsDbCdredList.tax_pay[0].cess.fee;
                                        //}
                                    }
                                }
                                else
                                {
                                    dt.Rows[3]["Fld_OtRcTaxPay"] = "0";
                                    dt.Rows[3]["Fld_InterestPay"] = "0";
                                }
                                if (obj.data.returnsDbCdredList.tax_pay.Count > 1)
                                {
                                    if (obj.data.returnsDbCdredList.tax_pay[1].cess != null)
                                    {
                                        if (obj.data.returnsDbCdredList.tax_pay[1].cess.tx != null)
                                        {
                                            dt.Rows[3]["Fld_RcTaxPay"] = obj.data.returnsDbCdredList.tax_pay[1].cess.tx;
                                        }
                                    }
                                }
                                else
                                {
                                    dt.Rows[3]["Fld_RcTaxPay"] = "0";
                                }
                                #endregion
                            }
                            #endregion

                            #region Tax Paid
                            if (obj.data.returnsDbCdredList.tax_paid != null)
                            {
                                if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash != null)
                                {
                                    #region Tax Paid IGST
                                    if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash.Count > 0)
                                    {
                                        if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash[0].igst != null)
                                        {
                                            if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash[0].igst.tx != null)
                                            {
                                                dt.Rows[0]["Fld_OtRcTaxPayCash"] = obj.data.returnsDbCdredList.tax_paid.pd_by_cash[0].igst.tx;
                                            }
                                            if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash[0].igst.intr != null)
                                            {
                                                dt.Rows[0]["Fld_InterestPayCash"] = obj.data.returnsDbCdredList.tax_paid.pd_by_cash[0].igst.intr;
                                            }
                                            //if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash[0].igst.fee != null)
                                            //{
                                            //    dt.Rows[0]["Fld_LateFeePayCash"] = obj.data.returnsDbCdredList.tax_paid.pd_by_cash[0].igst.fee;
                                            //}                                        
                                        }
                                    }
                                    else
                                    {
                                        dt.Rows[0]["Fld_OtRcTaxPayCash"] = "0";
                                        dt.Rows[0]["Fld_InterestPayCash"] = "0";
                                    }
                                    if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash.Count > 1)
                                    {
                                        if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash[1].igst != null)
                                        {
                                            if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash[1].igst.tx != null)
                                            {
                                                dt.Rows[0]["Fld_RcTaxPayCash"] = obj.data.returnsDbCdredList.tax_paid.pd_by_cash[1].igst.tx;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dt.Rows[0]["Fld_RcTaxPayCash"] = "0";
                                    }
                                    #endregion

                                    #region Tax Paid CGST
                                    if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash.Count > 0)
                                    {
                                        if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash[0].cgst != null)
                                        {
                                            if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash[0].cgst.tx != null)
                                            {
                                                dt.Rows[1]["Fld_OtRcTaxPayCash"] = obj.data.returnsDbCdredList.tax_paid.pd_by_cash[0].cgst.tx;
                                            }
                                            if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash[0].cgst.intr != null)
                                            {
                                                dt.Rows[1]["Fld_InterestPayCash"] = obj.data.returnsDbCdredList.tax_paid.pd_by_cash[0].cgst.intr;
                                            }
                                            if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash[0].cgst.fee != null)
                                            {
                                                dt.Rows[1]["Fld_LateFeePayCash"] = obj.data.returnsDbCdredList.tax_paid.pd_by_cash[0].cgst.fee;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dt.Rows[1]["Fld_OtRcTaxPayCash"] = "0";
                                        dt.Rows[1]["Fld_InterestPayCash"] = "0";
                                        dt.Rows[1]["Fld_LateFeePayCash"] = "0";

                                    }
                                    if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash.Count > 1)
                                    {
                                        if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash[1].cgst != null)
                                        {
                                            if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash[1].cgst.tx != null)
                                            {
                                                dt.Rows[1]["Fld_RcTaxPayCash"] = obj.data.returnsDbCdredList.tax_paid.pd_by_cash[1].cgst.tx;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dt.Rows[1]["Fld_RcTaxPayCash"] = "0";
                                    }
                                    #endregion

                                    #region Tax Paid SGST
                                    if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash.Count > 0)
                                    {
                                        if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash[0].sgst != null)
                                        {
                                            if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash[0].sgst.tx != null)
                                            {
                                                dt.Rows[2]["Fld_OtRcTaxPayCash"] = obj.data.returnsDbCdredList.tax_paid.pd_by_cash[0].sgst.tx;
                                            }
                                            if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash[0].sgst.intr != null)
                                            {
                                                dt.Rows[2]["Fld_InterestPayCash"] = obj.data.returnsDbCdredList.tax_paid.pd_by_cash[0].sgst.intr;
                                            }
                                            if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash[0].sgst.fee != null)
                                            {
                                                dt.Rows[2]["Fld_LateFeePayCash"] = obj.data.returnsDbCdredList.tax_paid.pd_by_cash[0].sgst.fee;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dt.Rows[2]["Fld_OtRcTaxPayCash"] = "0";
                                        dt.Rows[2]["Fld_InterestPayCash"] = "0";
                                        dt.Rows[2]["Fld_LateFeePayCash"] = "0";
                                    }
                                    if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash.Count > 1)
                                    {
                                        if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash[1].sgst != null)
                                        {
                                            if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash[1].sgst.tx != null)
                                            {
                                                dt.Rows[2]["Fld_RcTaxPayCash"] = obj.data.returnsDbCdredList.tax_paid.pd_by_cash[1].sgst.tx;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dt.Rows[2]["Fld_RcTaxPayCash"] = "0";
                                    }
                                    #endregion

                                    #region Tax Paid CESS
                                    if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash.Count > 0)
                                    {
                                        if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash[0].cess != null)
                                        {
                                            if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash[0].cess.tx != null)
                                            {
                                                dt.Rows[3]["Fld_OtRcTaxPayCash"] = obj.data.returnsDbCdredList.tax_paid.pd_by_cash[0].cess.tx;
                                            }
                                            if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash[0].cess.intr != null)
                                            {
                                                dt.Rows[3]["Fld_InterestPayCash"] = obj.data.returnsDbCdredList.tax_paid.pd_by_cash[0].cess.intr;
                                            }
                                            //if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash[0].cess.fee != null)
                                            //{
                                            //    dt.Rows[3]["Fld_LateFeePayCash"] = obj.data.returnsDbCdredList.tax_paid.pd_by_cash[0].cess.fee;
                                            //}
                                        }
                                    }
                                    else
                                    {
                                        dt.Rows[3]["Fld_OtRcTaxPayCash"] = "0";
                                        dt.Rows[3]["Fld_InterestPayCash"] = "0";
                                    }
                                    if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash.Count > 1)
                                    {
                                        if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash[1].cess.tx != null)
                                        {
                                            if (obj.data.returnsDbCdredList.tax_paid.pd_by_cash[1].cess.tx != null)
                                            {
                                                dt.Rows[3]["Fld_RcTaxPayCash"] = obj.data.returnsDbCdredList.tax_paid.pd_by_cash[1].cess.tx;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dt.Rows[3]["Fld_RcTaxPayCash"] = "0";
                                    }
                                    #endregion
                                }
                            }
                            #endregion

                            #region Paid Through ITC
                            if (obj.data.returnsDbCdredList.tax_paid != null)
                            {
                                if (obj.data.returnsDbCdredList.tax_paid.pd_by_itc.Count > 0)
                                {
                                    if (obj.data.returnsDbCdredList.tax_paid.pd_by_itc != null)
                                    {
                                        #region Tax Paid IGST
                                        if (obj.data.returnsDbCdredList.tax_paid.pd_by_itc[0].igst_igst_amt != null)
                                        {
                                            dt.Rows[0]["Fld_IGST"] = obj.data.returnsDbCdredList.tax_paid.pd_by_itc[0].igst_igst_amt;
                                        }
                                        if (obj.data.returnsDbCdredList.tax_paid.pd_by_itc[0].cgst_igst_amt != null)
                                        {
                                            dt.Rows[1]["Fld_IGST"] = obj.data.returnsDbCdredList.tax_paid.pd_by_itc[0].cgst_igst_amt;
                                        }
                                        if (obj.data.returnsDbCdredList.tax_paid.pd_by_itc[0].sgst_igst_amt != null)
                                        {
                                            dt.Rows[2]["Fld_IGST"] = obj.data.returnsDbCdredList.tax_paid.pd_by_itc[0].sgst_igst_amt;
                                        }
                                        #endregion

                                        #region Tax Paid CGST
                                        if (obj.data.returnsDbCdredList.tax_paid.pd_by_itc[0].igst_cgst_amt != null)
                                        {
                                            dt.Rows[0]["Fld_CGST"] = obj.data.returnsDbCdredList.tax_paid.pd_by_itc[0].igst_cgst_amt;
                                        }
                                        if (obj.data.returnsDbCdredList.tax_paid.pd_by_itc[0].cgst_cgst_amt != null)
                                        {
                                            dt.Rows[1]["Fld_CGST"] = obj.data.returnsDbCdredList.tax_paid.pd_by_itc[0].cgst_cgst_amt;
                                        }
                                        #endregion

                                        #region Tax Paid SGST
                                        if (obj.data.returnsDbCdredList.tax_paid.pd_by_itc[0].igst_sgst_amt != null)
                                        {
                                            dt.Rows[0]["Fld_SGST"] = obj.data.returnsDbCdredList.tax_paid.pd_by_itc[0].igst_sgst_amt;
                                        }
                                        if (obj.data.returnsDbCdredList.tax_paid.pd_by_itc[0].sgst_sgst_amt != null)
                                        {
                                            dt.Rows[2]["Fld_SGST"] = obj.data.returnsDbCdredList.tax_paid.pd_by_itc[0].sgst_sgst_amt;
                                        }
                                        #endregion

                                        #region Tax Paid CESS
                                        if (obj.data.returnsDbCdredList.tax_paid.pd_by_itc[0].cess_cess_amt != null)
                                        {
                                            dt.Rows[3]["Fld_CESS"] = obj.data.returnsDbCdredList.tax_paid.pd_by_itc[0].cess_cess_amt;
                                        }
                                        #endregion
                                    }
                                }
                                else
                                {
                                    dt.Rows[0]["Fld_IGST"] = "0";
                                    dt.Rows[1]["Fld_IGST"] = "0";
                                    dt.Rows[2]["Fld_IGST"] = "0";
                                    dt.Rows[0]["Fld_CGST"] = "0";
                                    dt.Rows[1]["Fld_CGST"] = "0";
                                    dt.Rows[0]["Fld_SGST"] = "0";
                                    dt.Rows[2]["Fld_SGST"] = "0";
                                    dt.Rows[3]["Fld_CESS"] = "0";
                                }
                            }
                            #endregion
                        }
                    }
                }

                #region Save Get Data
                #region FIRST DELETE OLD DATA FROM DATABASE
                Query = "Delete from SPQR3BTaxPayment where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                _Result = objGSTR3B.IUDData(Query);
                if (_Result != 1)
                {
                    // ERROR OCCURS WHILE DELETING DATA
                    MessageBox.Show("System error.\nPlease try after sometime!");
                }
                else
                {
                    _Result = objGSTR3B.GSTR3B6BulkEntry(dt, Convert.ToString(CommonHelper.StatusText));

                    #region Get Total
                    DataTable dtT = new DataTable();
                    dtT.Columns.Add("Fld_Sequence");
                    dtT.Columns.Add("Fld_Description");
                    dtT.Columns.Add("Fld_OtRcTaxPay");
                    dtT.Columns.Add("Fld_IGST");
                    dtT.Columns.Add("Fld_CGST");
                    dtT.Columns.Add("Fld_SGST");
                    dtT.Columns.Add("Fld_CESS");
                    dtT.Columns.Add("Fld_OtRcTaxPayCash");
                    dtT.Columns.Add("Fld_RcTaxPay");
                    dtT.Columns.Add("Fld_RcTaxPayCash");
                    dtT.Columns.Add("Fld_InterestPay");
                    dtT.Columns.Add("Fld_InterestPayCash");
                    dtT.Columns.Add("Fld_LateFeePay");
                    dtT.Columns.Add("Fld_LateFeePayCash");
                    dtT.Columns.Add("Fld_UtilizableCash");
                    dtT.Columns.Add("Fld_AdditionalCash");
                    dtT.Columns.Add("Fld_FileStatus");
                    dtT.Columns.Add("Fld_FinancialYear");
                    dtT.Columns.Add("Fld_Month");

                    DataRow drn = dtT.NewRow();
                    drn["Fld_Description"] = "Total";
                    drn["Fld_OtRcTaxPay"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_OtRcTaxPay"] != null).Sum(x => x["Fld_OtRcTaxPay"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_OtRcTaxPay"])).ToString();
                    drn["Fld_IGST"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_IGST"] != null).Sum(x => x["Fld_IGST"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_IGST"])).ToString();
                    drn["Fld_CGST"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_CGST"] != null).Sum(x => x["Fld_CGST"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_CGST"])).ToString();
                    drn["Fld_SGST"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_SGST"] != null).Sum(x => x["Fld_SGST"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_SGST"])).ToString();
                    drn["Fld_CESS"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_CESS"] != null).Sum(x => x["Fld_CESS"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_CESS"])).ToString();
                    drn["Fld_OtRcTaxPayCash"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_OtRcTaxPayCash"] != null).Sum(x => x["Fld_OtRcTaxPayCash"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_OtRcTaxPayCash"])).ToString();
                    drn["Fld_RcTaxPay"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_RcTaxPay"] != null).Sum(x => x["Fld_RcTaxPay"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_RcTaxPay"])).ToString();
                    drn["Fld_RcTaxPayCash"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_RcTaxPayCash"] != null).Sum(x => x["Fld_RcTaxPayCash"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_RcTaxPayCash"])).ToString();
                    drn["Fld_InterestPay"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_InterestPay"] != null).Sum(x => x["Fld_InterestPay"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_InterestPay"])).ToString();
                    drn["Fld_InterestPayCash"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_InterestPayCash"] != null).Sum(x => x["Fld_InterestPayCash"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_InterestPayCash"])).ToString();
                    drn["Fld_LateFeePay"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_LateFeePay"] != null).Sum(x => x["Fld_LateFeePay"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_LateFeePay"])).ToString();
                    drn["Fld_LateFeePayCash"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_LateFeePayCash"] != null).Sum(x => x["Fld_LateFeePayCash"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_LateFeePayCash"])).ToString();
                    drn["Fld_UtilizableCash"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_UtilizableCash"] != null).Sum(x => x["Fld_UtilizableCash"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_UtilizableCash"])).ToString();
                    drn["Fld_AdditionalCash"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_AdditionalCash"] != null).Sum(x => x["Fld_AdditionalCash"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_AdditionalCash"])).ToString();

                    // ADD DATAROW TO DATATABLE
                    dtT.Rows.Add(drn);

                    dtT.AcceptChanges();
                    #endregion

                    _Result = objGSTR3B.GSTR3B6BulkEntry(dtT, "Total");
                }

                if (_Result == 1)
                {
                    Getdata();
                    MessageBox.Show("Data saved successfully..", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion
                #endregion
                #endregion

                return true;
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
        public bool Form1Entry(string jsonData)
        {
            bool flg = false;
            try
            {
                DataTable dt = new DataTable();
                RootObjectJson obj = JsonConvert.DeserializeObject<RootObjectJson>(jsonData);

                #region ASSIGN GRIDVIEW ROWS IN DATATABLE For B2B

                dt = new DataTable();
                dt.Columns.Add("Fld_Sequence");
                dt.Columns.Add("Fld_NatureofSupply");
                dt.Columns.Add("Fld_TotalTaxableValue");
                dt.Columns.Add("Fld_IGST");
                dt.Columns.Add("Fld_CGST");
                dt.Columns.Add("Fld_SGST");
                dt.Columns.Add("Fld_CESS");
                dt.Columns.Add("Fld_FileStatus");

                if (obj.data != null && obj.data.sup_details != null)
                {
                    dt.Rows.Add("", "(a) Outward Taxable Supplies (other than Zero rated,nil rated and exemted)", "", "", "", "", "", "");
                    dt.Rows.Add("", "(b) Outward Taxable Supplies (Zero rated)", "", "", "", "", "", "");
                    dt.Rows.Add("", "(c) Other outward Supplies(Nil rated, exemted)", "", "", "", "", "", "");
                    dt.Rows.Add("", "(d) Inward Supplies(liable to reverse charge)", "", "", "", "", "", "");
                    dt.Rows.Add("", "(e) Non-GST outward supplies", "", "", "", "", "", "");

                    if (obj.data.sup_details.osup_det != null)
                    {
                        dt.Rows[0]["Fld_TotalTaxableValue"] = obj.data.sup_details.osup_det.txval;
                        dt.Rows[0]["Fld_IGST"] = obj.data.sup_details.osup_det.iamt;
                        dt.Rows[0]["Fld_CGST"] = obj.data.sup_details.osup_det.camt;
                        dt.Rows[0]["Fld_SGST"] = obj.data.sup_details.osup_det.samt;
                        dt.Rows[0]["Fld_CESS"] = obj.data.sup_details.osup_det.csamt;
                    }

                    if (obj.data.sup_details.osup_zero != null)
                    {
                        dt.Rows[1]["Fld_TotalTaxableValue"] = obj.data.sup_details.osup_zero.txval;
                        dt.Rows[1]["Fld_IGST"] = obj.data.sup_details.osup_zero.iamt;
                        dt.Rows[1]["Fld_CESS"] = obj.data.sup_details.osup_zero.csamt;
                    }

                    if (obj.data.sup_details.osup_nil_exmp != null)
                    {
                        dt.Rows[2]["Fld_TotalTaxableValue"] = obj.data.sup_details.osup_nil_exmp.txval;
                    }

                    if (obj.data.sup_details.isup_rev != null)
                    {
                        dt.Rows[3]["Fld_TotalTaxableValue"] = obj.data.sup_details.isup_rev.txval;
                        dt.Rows[3]["Fld_IGST"] = obj.data.sup_details.isup_rev.iamt;
                        dt.Rows[3]["Fld_CGST"] = obj.data.sup_details.isup_rev.camt;
                        dt.Rows[3]["Fld_SGST"] = obj.data.sup_details.isup_rev.samt;
                        dt.Rows[3]["Fld_CESS"] = obj.data.sup_details.isup_rev.csamt;
                    }

                    if (obj.data.sup_details.osup_nongst != null)
                    {
                        dt.Rows[4]["Fld_TotalTaxableValue"] = obj.data.sup_details.osup_nongst.txval;
                    }
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["Fld_Sequence"] = Convert.ToString(i + 1);
                        dt.Rows[i]["Fld_FileStatus"] = "Completed";
                    }
                    dt.AcceptChanges();
                }
                else
                {
                    return true;
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.NewRow();
                    //dr["Fld_InvoiceNo"] = dt.Rows.Cast<DataRow>().Where(x => Convert.ToString(x["Fld_InvoiceNo"]).Trim() != "").GroupBy(x => x["Fld_InvoiceNo"]).Select(x => x.First()).Distinct().Count();

                    dr["Fld_TotalTaxableValue"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_TotalTaxableValue"] != null).Sum(x => x["Fld_TotalTaxableValue"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_TotalTaxableValue"])).ToString();
                    dr["Fld_IGST"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_IGST"] != null).Sum(x => x["Fld_IGST"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_IGST"])).ToString();
                    dr["Fld_CGST"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_CGST"] != null).Sum(x => x["Fld_CGST"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_CGST"])).ToString();
                    dr["Fld_SGST"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_SGST"] != null).Sum(x => x["Fld_SGST"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_SGST"])).ToString();
                    dr["Fld_CESS"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_CESS"] != null).Sum(x => x["Fld_CESS"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_CESS"])).ToString();

                    dr["Fld_NatureofSupply"] = "Total";
                    dr["Fld_FileStatus"] = "Total";
                    dt.Rows.Add(dr);

                    int _Result = objGSTR3B.GSTR3BForm1BulkEntryJson(dt);
                    if (_Result == 1)
                        flg = true;
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

            return flg;
        }
        public bool Form2Entry(string jsonData)
        {
            bool flg = false;
            try
            {
                DataTable dt = new DataTable();
                RootObjectJson obj = JsonConvert.DeserializeObject<RootObjectJson>(jsonData);

                #region ASSIGN GRIDVIEW ROWS IN DATATABLE For B2B

                dt = new DataTable();
                dt.Columns.Add("Fld_Sequence");
                dt.Columns.Add("Fld_Details");
                dt.Columns.Add("Fld_POS");
                dt.Columns.Add("Fld_Taxable");
                dt.Columns.Add("Fld_IGST");
                dt.Columns.Add("Fld_FileStatus");

                if (obj.data != null && obj.data.inter_sup != null)
                {
                    if (obj.data.inter_sup.unreg_details != null && obj.data.inter_sup.unreg_details.Count > 0)
                    {
                        for (int i = 0; i < obj.data.inter_sup.unreg_details.Count; i++)
                        {
                            dt.Rows.Add();

                            dt.Rows[dt.Rows.Count - 1]["Fld_Details"] = "Supplies made to Unregistered Persons";
                            if (Convert.ToString(obj.data.inter_sup.unreg_details[i].pos) != null)
                                dt.Rows[dt.Rows.Count - 1]["Fld_POS"] = CommonHelper.GetStateName(Convert.ToString(obj.data.inter_sup.unreg_details[i].pos));
                            dt.Rows[dt.Rows.Count - 1]["Fld_Taxable"] = Convert.ToString(obj.data.inter_sup.unreg_details[i].txval);
                            dt.Rows[dt.Rows.Count - 1]["Fld_IGST"] = Convert.ToString(obj.data.inter_sup.unreg_details[i].iamt);
                        }
                    }
                    if (obj.data.inter_sup.comp_details != null && obj.data.inter_sup.comp_details.Count > 0)
                    {
                        for (int i = 0; i < obj.data.inter_sup.comp_details.Count; i++)
                        {
                            dt.Rows.Add();

                            dt.Rows[dt.Rows.Count - 1]["Fld_Details"] = "Supplies made to Composition Taxable Persons";
                            if (Convert.ToString(obj.data.inter_sup.comp_details[i].pos) != null)
                                dt.Rows[dt.Rows.Count - 1]["Fld_POS"] = CommonHelper.GetStateName(Convert.ToString(obj.data.inter_sup.comp_details[i].pos));
                            dt.Rows[dt.Rows.Count - 1]["Fld_Taxable"] = Convert.ToString(obj.data.inter_sup.comp_details[i].txval);
                            dt.Rows[dt.Rows.Count - 1]["Fld_IGST"] = Convert.ToString(obj.data.inter_sup.comp_details[i].iamt);
                        }
                    }
                    if (obj.data.inter_sup.uin_details != null && obj.data.inter_sup.uin_details.Count > 0)
                    {
                        for (int i = 0; i < obj.data.inter_sup.uin_details.Count; i++)
                        {
                            dt.Rows.Add();

                            dt.Rows[dt.Rows.Count - 1]["Fld_Details"] = "Supplies made to UIN holders";
                            if (Convert.ToString(obj.data.inter_sup.uin_details[i].pos) != null)
                                dt.Rows[dt.Rows.Count - 1]["Fld_POS"] = CommonHelper.GetStateName(Convert.ToString(obj.data.inter_sup.uin_details[i].pos));
                            dt.Rows[dt.Rows.Count - 1]["Fld_Taxable"] = Convert.ToString(obj.data.inter_sup.uin_details[i].txval);
                            dt.Rows[dt.Rows.Count - 1]["Fld_IGST"] = Convert.ToString(obj.data.inter_sup.uin_details[i].iamt);
                        }
                    }

                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["Fld_Sequence"] = Convert.ToString(i + 1);
                        dt.Rows[i]["Fld_FileStatus"] = "Completed";
                    }
                    dt.AcceptChanges();
                }
                else
                {
                    return true;
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.NewRow();
                    //dr["Fld_InvoiceNo"] = dt.Rows.Cast<DataRow>().Where(x => Convert.ToString(x["Fld_InvoiceNo"]).Trim() != "").GroupBy(x => x["Fld_InvoiceNo"]).Select(x => x.First()).Distinct().Count();

                    dr["Fld_Taxable"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_Taxable"] != null).Sum(x => x["Fld_Taxable"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_Taxable"])).ToString();
                    dr["Fld_IGST"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_IGST"] != null).Sum(x => x["Fld_IGST"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_IGST"])).ToString();

                    dr["Fld_FileStatus"] = "Total";
                    dt.Rows.Add(dr);

                    int _Result = objGSTR3B.GSTR3BForm2BulkEntryJson(dt);
                    if (_Result == 1)
                        flg = true;
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

            return flg;
        }
        public bool Form4Entry(string jsonData)
        {
            bool flg = false;
            try
            {
                DataTable dt = new DataTable();
                RootObjectJson obj = JsonConvert.DeserializeObject<RootObjectJson>(jsonData);

                #region ASSIGN GRIDVIEW ROWS IN DATATABLE For B2B

                dt = new DataTable();
                dt.Columns.Add("Fld_Sequence");
                dt.Columns.Add("Fld_Details");
                dt.Columns.Add("Fld_IGST");
                dt.Columns.Add("Fld_CGST");
                dt.Columns.Add("Fld_SGST");
                dt.Columns.Add("Fld_CESS");
                dt.Columns.Add("Fld_FileStatus");

                if (obj.data != null && obj.data.itc_elg != null)
                {
                    int r = 0;
                    dt.Rows.Add("", "(A) ITC Available (Whether in full or part)", "", "", "", "", "");
                    dt.Rows.Add("", "   (1) Import of goods", "", "", "", "", "");
                    dt.Rows.Add("", "   (2) Import of Services", "", "", "", "", "");
                    dt.Rows.Add("", "   (3) Inward Supplies liable to reverse charge(other than 1 & 2 above)", "", "", "", "", "");
                    dt.Rows.Add("", "   (4) Inward supplies from ISD", "", "", "", "", "");
                    dt.Rows.Add("", "   (5) All other ITC", "", "", "", "", "");
                    dt.Rows.Add("", "Total ITC Available (A)", "", "", "", "", "");
                    dt.Rows.Add("", "(B) ITC Reversed", "", "", "", "", "");
                    dt.Rows.Add("", "   (1) As per rules 42 & 43 IGST Rules", "", "", "", "", "");
                    dt.Rows.Add("", "   (2) Others", "", "", "", "", "");
                    dt.Rows.Add("", "Total ITC Reversed (B)", "", "", "", "", "");
                    dt.Rows.Add("", "(C) Net ITC Available (A) – (B)", "", "", "", "", "");
                    dt.Rows.Add("", "(D) Ineligible ITC", "", "", "", "", "");
                    dt.Rows.Add("", "   (1) As per section 17(5)", "", "", "", "", "");
                    dt.Rows.Add("", "   (2) Others", "", "", "", "", "");

                    if (obj.data.itc_elg.itc_avl != null && obj.data.itc_elg.itc_avl.Count > 0)
                    {
                        for (int i = 0; i < obj.data.itc_elg.itc_avl.Count; i++)
                        {
                            if (Convert.ToString(obj.data.itc_elg.itc_avl[i].ty) == "IMPG")
                                r = 1;
                            else if (Convert.ToString(obj.data.itc_elg.itc_avl[i].ty) == "IMPS")
                                r = 2;
                            else if (Convert.ToString(obj.data.itc_elg.itc_avl[i].ty) == "ISRC")
                                r = 3;
                            else if (Convert.ToString(obj.data.itc_elg.itc_avl[i].ty) == "ISD")
                                r = 4;
                            else if (Convert.ToString(obj.data.itc_elg.itc_avl[i].ty) == "OTH")
                                r = 5;

                            dt.Rows[r]["Fld_IGST"] = Convert.ToString(obj.data.itc_elg.itc_avl[i].iamt);
                            if (r != 1 && r != 2)
                            {
                                dt.Rows[r]["Fld_CGST"] = Convert.ToString(obj.data.itc_elg.itc_avl[i].camt);
                                dt.Rows[r]["Fld_SGST"] = Convert.ToString(obj.data.itc_elg.itc_avl[i].samt);
                            }
                            dt.Rows[r]["Fld_CESS"] = Convert.ToString(obj.data.itc_elg.itc_avl[i].csamt);
                        }
                    }

                    if (obj.data.itc_elg.itc_rev != null && obj.data.itc_elg.itc_rev.Count > 0)
                    {
                        for (int i = 0; i < obj.data.itc_elg.itc_rev.Count; i++)
                        {
                            if (Convert.ToString(obj.data.itc_elg.itc_rev[i].ty) == "RUL")
                                r = 8;
                            else if (Convert.ToString(obj.data.itc_elg.itc_rev[i].ty) == "OTH")
                                r = 9;

                            dt.Rows[r]["Fld_IGST"] = Convert.ToString(obj.data.itc_elg.itc_rev[i].iamt);
                            dt.Rows[r]["Fld_CGST"] = Convert.ToString(obj.data.itc_elg.itc_rev[i].camt);
                            dt.Rows[r]["Fld_SGST"] = Convert.ToString(obj.data.itc_elg.itc_rev[i].samt);
                            dt.Rows[r]["Fld_CESS"] = Convert.ToString(obj.data.itc_elg.itc_rev[i].csamt);
                        }
                    }

                    if (obj.data.itc_elg.itc_net != null)
                    {
                        r = 11;
                        dt.Rows[r]["Fld_IGST"] = Convert.ToString(obj.data.itc_elg.itc_net.iamt);
                        dt.Rows[r]["Fld_CGST"] = Convert.ToString(obj.data.itc_elg.itc_net.camt);
                        dt.Rows[r]["Fld_SGST"] = Convert.ToString(obj.data.itc_elg.itc_net.samt);
                        dt.Rows[r]["Fld_CESS"] = Convert.ToString(obj.data.itc_elg.itc_net.csamt);

                    }

                    if (obj.data.itc_elg.itc_inelg != null && obj.data.itc_elg.itc_inelg.Count > 0)
                    {
                        for (int i = 0; i < obj.data.itc_elg.itc_inelg.Count; i++)
                        {
                            if (Convert.ToString(obj.data.itc_elg.itc_inelg[i].ty) == "RUL")
                                r = 13;
                            else if (Convert.ToString(obj.data.itc_elg.itc_inelg[i].ty) == "OTH")
                                r = 14;

                            dt.Rows[r]["Fld_IGST"] = Convert.ToString(obj.data.itc_elg.itc_inelg[i].iamt);
                            dt.Rows[r]["Fld_CGST"] = Convert.ToString(obj.data.itc_elg.itc_inelg[i].camt);
                            dt.Rows[r]["Fld_SGST"] = Convert.ToString(obj.data.itc_elg.itc_inelg[i].samt);
                            dt.Rows[r]["Fld_CESS"] = Convert.ToString(obj.data.itc_elg.itc_inelg[i].csamt);
                        }
                    }

                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["Fld_Sequence"] = Convert.ToString(i + 1);
                        dt.Rows[i]["Fld_FileStatus"] = "Completed";
                    }
                    dt.AcceptChanges();
                }
                else
                {
                    return true;
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    //DataRow dr = dt.NewRow();
                    //dr["Fld_TotalTaxableValue"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_TotalTaxableValue"] != null).Sum(x => x["Fld_TotalTaxableValue"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_TotalTaxableValue"])).ToString();
                    //dr["Fld_IGST"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_IGST"] != null).Sum(x => x["Fld_IGST"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_IGST"])).ToString();
                    //dr["Fld_CGST"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_CGST"] != null).Sum(x => x["Fld_CGST"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_CGST"])).ToString();
                    //dr["Fld_SGST"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_SGST"] != null).Sum(x => x["Fld_SGST"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_SGST"])).ToString();
                    //dr["Fld_CESS"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_CESS"] != null).Sum(x => x["Fld_CESS"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_CESS"])).ToString();

                    //dr["Fld_NatureofSupply"] = "Total";
                    //dr["Fld_FileStatus"] = "Total";
                    //dt.Rows.Add(dr);

                    int _Result = objGSTR3B.GSTR3BForm4BulkEntryJson(dt);
                    if (_Result == 1)
                        flg = true;
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

            return flg;
        }
        public bool Form5Entry(string jsonData)
        {
            bool flg = false;
            try
            {
                DataTable dt = new DataTable();
                RootObjectJson obj = JsonConvert.DeserializeObject<RootObjectJson>(jsonData);

                #region ASSIGN GRIDVIEW ROWS IN DATATABLE For B2B

                dt = new DataTable();
                dt.Columns.Add("Fld_Sequence");
                dt.Columns.Add("Fld_NatureofSupply");
                dt.Columns.Add("Fld_InterStateSupplies");
                dt.Columns.Add("Fld_IntraStateSupplies");
                dt.Columns.Add("Fld_FileStatus");

                dt.Rows.Add("", "From a supplier under composition scheme, Exempt and Nil rated Supply", "", "", "");
                dt.Rows.Add("", "Non GST Supply", "", "", "");

                if (obj.data != null && obj.data.inward_sup != null)
                {
                    if (obj.data.inward_sup.isup_details != null && obj.data.inward_sup.isup_details.Count > 0)
                    {
                        for (int i = 0; i < obj.data.inward_sup.isup_details.Count; i++)
                        {
                            if (Convert.ToString(obj.data.inward_sup.isup_details[i].ty) == "GST")
                            {
                                dt.Rows[0]["Fld_InterStateSupplies"] = Convert.ToString(obj.data.inward_sup.isup_details[i].inter);
                                dt.Rows[0]["Fld_IntraStateSupplies"] = Convert.ToString(obj.data.inward_sup.isup_details[i].intra);
                            }
                            else if (Convert.ToString(obj.data.inward_sup.isup_details[i].ty) == "NONGST")
                            {
                                dt.Rows[1]["Fld_InterStateSupplies"] = Convert.ToString(obj.data.inward_sup.isup_details[i].inter);
                                dt.Rows[1]["Fld_IntraStateSupplies"] = Convert.ToString(obj.data.inward_sup.isup_details[i].intra);
                            }
                        }
                    }

                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["Fld_Sequence"] = Convert.ToString(i + 1);
                        dt.Rows[i]["Fld_FileStatus"] = "Completed";
                    }
                    dt.AcceptChanges();
                }
                else
                {
                    return true;
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    int _Result = objGSTR3B.GSTR3BForm5BulkEntryJson(dt);
                    if (_Result == 1)
                        flg = true;
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

            return flg;
        }

        #region Json Class
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
            public double camt { get; set; }
            public double samt { get; set; }
            public double csamt { get; set; }
        }

        public class OsupNilExmp
        {
            public double txval { get; set; }
            public double iamt { get; set; }
            public double camt { get; set; }
            public double samt { get; set; }
            public double csamt { get; set; }
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
            public double iamt { get; set; }
            public double camt { get; set; }
            public double samt { get; set; }
            public double csamt { get; set; }
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
            public List<ItcInelg> itc_inelg { get; set; }
        }

        public class IsupDetail
        {
            public string ty { get; set; }
            public double inter { get; set; }
            public double intra { get; set; }
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

        public class LtfeeDetails
        {
            public double iamt { get; set; }
            public double camt { get; set; }
            public double samt { get; set; }
            public double csamt { get; set; }
        }

        public class IntrLtfee
        {
            public IntrDetails intr_details { get; set; }
            public LtfeeDetails ltfee_details { get; set; }
        }

        public class TtVal
        {
            public double tt_pay { get; set; }
            public double tt_csh_pd { get; set; }
            public double tt_itc_pd { get; set; }
        }

        public class Qn
        {
            public string q1 { get; set; }
            public string q2 { get; set; }
            public string q3 { get; set; }
            public string q4 { get; set; }
            public string q5 { get; set; }
            public string q6 { get; set; }
            public string q7 { get; set; }
        }

        public class Data
        {
            public string gstin { get; set; }
            public string ret_period { get; set; }
            public SupDetails sup_details { get; set; }
            public InterSup inter_sup { get; set; }
            public ItcElg itc_elg { get; set; }
            public InwardSup inward_sup { get; set; }
            public IntrLtfee intr_ltfee { get; set; }
            public TtVal tt_val { get; set; }
            public Qn qn { get; set; }
        }

        public class RootObjectJson
        {
            public int status { get; set; }
            public Data data { get; set; }
        }

        #region Report Json
        public class Igst
        {
            public double tx { get; set; }
            public double intr { get; set; }
            public double pen { get; set; }
            public double fee { get; set; }
            public double oth { get; set; }
        }

        public class Cgst
        {
            public double tx { get; set; }
            public double intr { get; set; }
            public double pen { get; set; }
            public double fee { get; set; }
            public double oth { get; set; }
        }

        public class Sgst
        {
            public double tx { get; set; }
            public double intr { get; set; }
            public double pen { get; set; }
            public double fee { get; set; }
            public double oth { get; set; }
        }

        public class Cess
        {
            public double tx { get; set; }
            public double intr { get; set; }
            public double pen { get; set; }
            public double fee { get; set; }
            public double oth { get; set; }
        }

        public class CashBal
        {
            public Igst igst { get; set; }
            public Cgst cgst { get; set; }
            public Sgst sgst { get; set; }
            public Cess cess { get; set; }
            public double igst_tot_bal { get; set; }
            public double cgst_tot_bal { get; set; }
            public double sgst_tot_bal { get; set; }
            public double cess_tot_bal { get; set; }
        }

        public class ItcBal
        {
            public double igst_bal { get; set; }
            public double cgst_bal { get; set; }
            public double sgst_bal { get; set; }
            public double cess_bal { get; set; }
        }

        public class Bal
        {
            public string gstin { get; set; }
            public CashBal cash_bal { get; set; }
            public ItcBal itc_bal { get; set; }
        }

        public class Igst2
        {
            public double tx { get; set; }
            public double intr { get; set; }
            public double pen { get; set; }
            public double fee { get; set; }
            public double oth { get; set; }
            public double tot { get; set; }
        }

        public class Sgst2
        {
            public double tx { get; set; }
            public double intr { get; set; }
            public double pen { get; set; }
            public double fee { get; set; }
            public double oth { get; set; }
            public double tot { get; set; }
        }

        public class Cgst2
        {
            public double tx { get; set; }
            public double intr { get; set; }
            public double pen { get; set; }
            public double fee { get; set; }
            public double oth { get; set; }
            public double tot { get; set; }
        }

        public class Cess2
        {
            public double tx { get; set; }
            public double intr { get; set; }
            public double pen { get; set; }
            public double fee { get; set; }
            public double oth { get; set; }
            public double tot { get; set; }
        }

        public class TaxPay
        {
            public Igst2 igst { get; set; }
            public Sgst2 sgst { get; set; }
            public Cgst2 cgst { get; set; }
            public Cess2 cess { get; set; }
            public int liab_id { get; set; }
            public int trancd { get; set; }
            public string trandate { get; set; }
        }

        public class Igst3
        {
            public double tx { get; set; }
            public double intr { get; set; }
            public double pen { get; set; }
            public double fee { get; set; }
            public double oth { get; set; }
            public double tot { get; set; }
        }

        public class Sgst3
        {
            public double tx { get; set; }
            public double intr { get; set; }
            public double pen { get; set; }
            public double fee { get; set; }
            public double oth { get; set; }
            public double tot { get; set; }
        }

        public class Cgst3
        {
            public double tx { get; set; }
            public double intr { get; set; }
            public double pen { get; set; }
            public double fee { get; set; }
            public double oth { get; set; }
            public double tot { get; set; }
        }

        public class Cess3
        {
            public double tx { get; set; }
            public double intr { get; set; }
            public double pen { get; set; }
            public double fee { get; set; }
            public double oth { get; set; }
            public double tot { get; set; }
        }

        public class PdByCash
        {
            public Igst3 igst { get; set; }
            public Sgst3 sgst { get; set; }
            public Cgst3 cgst { get; set; }
            public Cess3 cess { get; set; }
            public int liab_id { get; set; }
            public string debit_id { get; set; }
            public int trancd { get; set; }
            public string trandate { get; set; }
        }

        public class PdByItc
        {
            public string debit_id { get; set; }
            public int liab_id { get; set; }
            public double igst_igst_amt { get; set; }
            public double igst_cgst_amt { get; set; }
            public double igst_sgst_amt { get; set; }
            public double sgst_sgst_amt { get; set; }
            public double sgst_igst_amt { get; set; }
            public double cgst_cgst_amt { get; set; }
            public double cgst_igst_amt { get; set; }
            public double cess_cess_amt { get; set; }
            public int trancd { get; set; }
            public string trandate { get; set; }
        }
        public class TaxPaid
        {
            public List<PdByCash> pd_by_cash { get; set; }
            public List<PdByItc> pd_by_itc { get; set; }
        }
        public class ReturnsDbCdredList
        {
            public List<TaxPay> tax_pay { get; set; }
            public TaxPaid tax_paid { get; set; }
        }
        public class DataR
        {
            public Bal bal { get; set; }
            public int status { get; set; }
            public ReturnsDbCdredList returnsDbCdredList { get; set; }
        }
        public class RootObjectReport
        {
            public int status { get; set; }
            public DataR data { get; set; }
        }
        #endregion
        #endregion

        private void msDownload_Click(object sender, EventArgs e)
        {
            pbGSTR1.Visible = true;
            getGSTR3BSummary();
            pbGSTR1.Visible = false;
        }

        private void btbSubmitReturn_Click(object sender, EventArgs e)
        {
            bool isExtensionInstalled = Utility.chromeExtensionCheck();

            if (true)
            {
                string GetMonth = CommonHelper.GetMonth(CommonHelper.SelectedMonth);
                string Year = CommonHelper.ReturnYear.Replace(" ", "");

                string[] Years = Year.Split('-');
                Year = Years[0] + "-" + Years[1].Substring(2, 2);

                var encodedString = Utility.encoding(Convert.ToString(Constants.UserName) + ',' + Convert.ToString(CommonHelper.CompanyPassword) + ',' + "submitgst3b" + ',' + GetMonth + ',' + Year);
                Process.Start("chrome.exe", "https://services.gst.gov.in/services/login?submitgst3b," + encodedString);
            }
        }

        private void btnFileGSTR_Click(object sender, EventArgs e)
        {
            bool isExtensionInstalled = Utility.chromeExtensionCheck();

            if (true)
            {
                string GetMonth = CommonHelper.GetMonth(CommonHelper.SelectedMonth);
                string Year = CommonHelper.ReturnYear.Replace(" ", "");

                string[] Years = Year.Split('-');
                Year = Years[0] + "-" + Years[1].Substring(2, 2);

                var encodedString = Utility.encoding(Convert.ToString(Constants.UserName) + ',' + Convert.ToString(CommonHelper.CompanyPassword) + ',' + "submitgst3b" + ',' + GetMonth + ',' + Year);
                Process.Start("chrome.exe", "https://services.gst.gov.in/services/login?submitgst3b," + encodedString);
            }
        }

        private void btnGetSummary_Click(object sender, EventArgs e)
        {

        }

        public void GetFilingStatusMsg()
        {
            try
            {
                DataTable dt = new DataTable();
                string Query = "Select * from SPQReturnStatus where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_ReturnType='GSTR-3B' order by Fld_Id DESC LIMIT 1;";
                dt = objGSTR3B.GetDataGSTR3B(Query);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string Message = "";
                    string Status = "";
                    if (dt.Rows[0]["Fld_Status"].ToString().Trim() == "0")
                        Status = "Not File";
                    else
                        Status = "File";

                    Message = "Last Action " + dt.Rows[0]["Fld_Action"].ToString().Trim() + ", " + dt.Rows[0]["Fld_ActionDate"].ToString().Trim() + ", Status " + Status + ".";
                    if (Message != "")
                    {
                        label1.Text = Message;
                    }
                }
                else
                {
                    label1.Text = "";
                }
            }
            catch { }
        }

        private void dgvReport_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == 1)
            {
                CommonHelper.IsMainFormType = "1";
                CommonHelper.ReturnName = "GSTR 1";
                SPQGSTR1Dashboard obj = new SPQGSTR1Dashboard();
                obj.MdiParent = this.MdiParent;
                Utility.CloseAllOpenForm();
                //obj.Dock = DockStyle.Fill;
                // 
                obj.Dock = DockStyle.Fill;
                obj.Show();
                ((SPQMDI)Application.OpenForms["SPQMDI"]).HideExtraToolsMenu();
                ((SPQMDI)Application.OpenForms["SPQMDI"]).HideCompanyDetailMenu();
            }
        }

        private void btnPaymentofTax_Click(object sender, EventArgs e)
        {
            //SPQ3Bcalculate obj = new SPQ3Bcalculate();
            //obj.MdiParent = this.MdiParent;
            //Utility.CloseAllOpenForm();
            //obj.Dock = DockStyle.Fill;
             
            //obj.Show();
            ((SPQMDI)Application.OpenForms["SPQMDI"]).HideCompanyDetailMenu();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            panSelectOption.Visible = false;
            //panSelectOption.BringToFront();
        }

        private void btnNaxt_Click(object sender, EventArgs e)
        {
            try
            {
                #region Check Option
                DataTable dtOption = new DataTable();
                dtOption.Columns.Add("Fld_Sequence");
                dtOption.Columns.Add("Fld_OptionName");
                dtOption.Columns.Add("Fld_OptionType");
                dtOption.Columns.Add("Fld_FinancialYear");
                dtOption.Columns.Add("Fld_Month");

                dtOption.Rows.Add("1", "A", "", CommonHelper.ReturnYear, CommonHelper.SelectedMonth);
                dtOption.Rows.Add("2", "B", "", CommonHelper.ReturnYear, CommonHelper.SelectedMonth);
                dtOption.Rows.Add("3", "C", "", CommonHelper.ReturnYear, CommonHelper.SelectedMonth);
                dtOption.Rows.Add("4", "D", "", CommonHelper.ReturnYear, CommonHelper.SelectedMonth);
                dtOption.Rows.Add("5", "E", "", CommonHelper.ReturnYear, CommonHelper.SelectedMonth);
                dtOption.Rows.Add("6", "F", "", CommonHelper.ReturnYear, CommonHelper.SelectedMonth);
                dtOption.Rows.Add("7", "G", "", CommonHelper.ReturnYear, CommonHelper.SelectedMonth);

                bool Flg = true;

                if (rbtAYes.Checked == true || rbtANo.Checked == true)
                {
                    if (rbtAYes.Checked == true)
                        dtOption.Rows[0]["Fld_OptionType"] = "Y";
                    else if (rbtANo.Checked == true)
                        dtOption.Rows[0]["Fld_OptionType"] = "N";
                }
                else
                    Flg = false;

                if (rbtBYes.Checked == true || rbtBNo.Checked == true)
                {
                    if (rbtBYes.Checked == true)
                        dtOption.Rows[1]["Fld_OptionType"] = "Y";
                    else if (rbtBNo.Checked == true)
                        dtOption.Rows[1]["Fld_OptionType"] = "N";
                }
                else
                    Flg = false;

                if (rbtCYes.Checked == true || rbtCNo.Checked == true)
                {
                    if (rbtCYes.Checked == true)
                        dtOption.Rows[2]["Fld_OptionType"] = "Y";
                    else if (rbtCNo.Checked == true)
                        dtOption.Rows[2]["Fld_OptionType"] = "N";
                }
                else
                    Flg = false;

                if (rbtDYes.Checked == true || rbtDNo.Checked == true)
                {
                    if (rbtDYes.Checked == true)
                        dtOption.Rows[3]["Fld_OptionType"] = "Y";
                    else if (rbtDNo.Checked == true)
                        dtOption.Rows[3]["Fld_OptionType"] = "N";
                }
                else
                    Flg = false;

                if (rbtEYes.Checked == true || rbtENo.Checked == true)
                {
                    if (rbtEYes.Checked == true)
                        dtOption.Rows[4]["Fld_OptionType"] = "Y";
                    else if (rbtENo.Checked == true)
                        dtOption.Rows[4]["Fld_OptionType"] = "N";
                }
                else
                    Flg = false;

                if (rbtFYes.Checked == true || rbtFNo.Checked == true)
                {
                    if (rbtFYes.Checked == true)
                        dtOption.Rows[5]["Fld_OptionType"] = "Y";
                    else if (rbtFNo.Checked == true)
                        dtOption.Rows[5]["Fld_OptionType"] = "N";
                }
                else
                    Flg = false;

                if (rbtGYes.Checked == true || rbtGNo.Checked == true)
                {
                    if (rbtGYes.Checked == true)
                        dtOption.Rows[6]["Fld_OptionType"] = "Y";
                    else if (rbtGNo.Checked == true)
                        dtOption.Rows[6]["Fld_OptionType"] = "N";
                }
                else
                    Flg = false;
                #endregion

                if (Flg == true)
                {
                    string Query = "Delete from SPQR3BFileOption where Fld_Month='" + CommonHelper.SelectedMonth + "' and Fld_FinancialYear='" + CommonHelper.ReturnYear + "'";
                    int _Result = objGSTR3B.IUDData(Query);
                    if (_Result == 1)
                    {
                        _Result = objGSTR3B.GSTR3BOption(dtOption);
                    }

                    if (_Result == 1)
                    {
                        panSelectOption.Visible = false;
                        #region sHTMl code


                        if (Utility.CheckNet())
                        {
                            var obj = clsPro.Cooki != null ? clsPro.Cooki.FirstOrDefault(x => x.ckname == (string.Concat("loginCookies_", CommonHelper.CompanyID))) : null;

                            if (obj != null && obj.CC1 != null)
                            {
                                pbGSTR1.Visible = true;
                                Application.DoEvents();

                                GSTR3BJson objJson = new GSTR3BJson();
                                if (objJson.generateJSON("GST").Trim() != "")
                                {
                                    string jsonFile = File.ReadAllText(CommonHelper.jsonFilePath);

                                    if (!this.saveGSTR3B(jsonFile))
                                    {
                                        MessageBox.Show(this.getError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    else
                                    {
                                        if (this.saveTransIdVal != string.Empty)
                                        {
                                            Thread.Sleep(3000);
                                            string jsonOption = "";
                                            objJson = new GSTR3BJson();
                                            if (objJson.generateOptionJSON("GST").Trim() != "")
                                            {
                                                jsonOption = File.ReadAllText(CommonHelper.jsonFilePath);
                                            }
                                            if (!this.saveGSTR3BOption(jsonOption))
                                            {
                                                //MessageBox.Show(this.getError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }

                                            string sj = string.Concat("Successfully saved  to GSTN. Transaction Id=", this.saveTransIdVal, ".Please cross check it from the GSTN server");
                                            MessageBox.Show(sj, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                            #region FilingLog
                                            string StatusCode = "";
                                            string Status = "1";
                                            string RefrenceID = "";

                                            Utility.FileingLogs("", "", CommonHelper.CompanyID, CommonHelper.CompanyGSTN, CommonHelper.SelectedMonth, CommonHelper.ReturnMonthYearStr, "GSTR-3B", "Save To GSTIN", System.DateTime.Now.ToString(), RefrenceID, Status, StatusCode, this.getError, sj);
                                            #endregion
                                        }

                                    }
                                }

                                CommonHelper.ReturnMonthYearStr = "";
                            }
                            else
                            {
                                SPQGstLogin frm = new SPQGstLogin();
                                frm.Visible = false;
                                var result = frm.ShowDialog();
                                if (result != DialogResult.OK)
                                {
                                    //GstLogin objLogin = new GstLogin();
                                    //objLogin.Show();
                                }
                                else
                                {
                                    btnNaxt_Click(sender, e);
                                }
                            }

                        }
                        else
                        {
                            MessageBox.Show("It Seems Your Internet Conection is Not Available, Please Connect Internet…!!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }


                        #endregion

                        pbGSTR1.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("Please try after sometime!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Please select mandatory fields!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                #region Gst Upload Code
                //if (SetGSPSetting())
                //{
                //    GSTR3BJson objJson = new GSTR3BJson();
                //    if (objJson.generateJSON("GST").Trim() != "")
                //    {
                //        frmGSTR1Upload obj = new frmGSTR1Upload();
                //        obj.MdiParent = this.MdiParent;
                //        Utility.CloseAllOpenForm();
                //        obj.Dock = DockStyle.Fill;
                //         
                //        obj.Show();
                //    }
                //}
                #endregion

                #region Old Code
                //if (SetGSPSetting())
                //{
                //    pbGSTR1.Visible = true;
                //    pbUploadInv.Enabled = false;

                //    GSPApisetting builder = new GSPApisetting();
                //    AppCompany objcompany = new AppCompany();
                //    DataTable data = new DataTable();
                //    double mi = 0;

                //    var request = (HttpWebRequest)WebRequest.Create("http://13.126.181.225:8000/SPEQTAGSTOffLineUtility/GetOffGstnApi?GSTIN=" + CommonHelper.CompanyGSTN + "");
                //    request.Headers.Add("Token", "MVPLGSPTKN221232");
                //    string webData = Utility.GetApi(request);
                //    RootObject b2bs = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(webData);

                //    if (Convert.ToString(b2bs.RecordId).Trim() != "")
                //    {
                //        GSPSetting.RecordId = b2bs.RecordId;
                //        GSPSetting.ipuser = b2bs.ipuser;
                //        GSPSetting.statecd = b2bs.statecd;
                //        GSPSetting.txn = b2bs.txn;
                //        GSPSetting.AppKey = b2bs.AppKey;
                //        GSPSetting.AuthToken = b2bs.AuthToken;
                //        GSPSetting.gstin = b2bs.gstin;
                //        GSPSetting.GSTINUserName = b2bs.GSTINUserName;
                //        GSPSetting.Datetime = b2bs.Datetime;
                //        GSPSetting.Decipher = b2bs.Decipher;
                //        GSPSetting.SEK = b2bs.SEK;
                //        GSPSetting.InsertDate = b2bs.InsertDate;
                //        GSPSetting.UpdateDate = b2bs.UpdateDate;
                //        GSPSetting.DeleteDate = b2bs.DeleteDate;
                //        GSPSetting.IsDeleted = b2bs.IsDeleted;

                //        if (Convert.ToString(GSPSetting.Datetime).Trim() != "")
                //        {
                //            DateTime time = Convert.ToDateTime(GSPSetting.Datetime);
                //            mi = DateTime.Now.Subtract(time).TotalMinutes;
                //        }

                //        if (mi >= 720 || mi == 0 || Convert.ToString(GSPSetting.AuthToken).Trim() == "")
                //        {
                //            string s1 = builder.OTPRequest(); // if otp is needed
                //            if (Convert.ToString(s1) != "")
                //            {
                //                builder.AuthenticationRequest(s1);

                //                #region gstr-3B

                //                GSTR3BJson objJson = new GSTR3BJson();
                //                if (objJson.generateJSON("GSP").Trim() != "")
                //                {
                //                    pbGetSummary.Enabled = true;
                //                    pbUploadInv.Enabled = false;
                //                    pbSubmitReturn.Enabled = false;
                //                    pbFileGSTR.Enabled = false;
                //                    //pbUploadInv.BackgroundImage = (System.Drawing.Image)Properties.Resources.Btn_1_Dark;
                //                    //pbGetSummary.BackgroundImage = (System.Drawing.Image)Properties.Resources.Btn_2_Light;
                //                }
                //                else
                //                { pbUploadInv.Enabled = true; }
                //                #endregion
                //            }
                //        }
                //        else // if otp no need
                //        {
                //            GSTR3BJson objJson = new GSTR3BJson();
                //            if (objJson.generateJSON("GSP").Trim() != "")
                //            {
                //                pbGetSummary.Enabled = true;
                //                pbUploadInv.Enabled = false;
                //                pbSubmitReturn.Enabled = false;
                //                pbFileGSTR.Enabled = false;
                //                //pbUploadInv.BackgroundImage = (System.Drawing.Image)Properties.Resources.Btn_1_Dark;
                //                //pbGetSummary.BackgroundImage = (System.Drawing.Image)Properties.Resources.Btn_2_Light;
                //            }
                //            else
                //            { pbUploadInv.Enabled = true; }
                //        }
                //        pbGSTR1.Visible = false;
                //        //pbUploadInv.Image = (System.Drawing.Image)Properties.Resources.Btn_1_Light;
                //    }
                //    else
                //    {
                //        //AppCompany objCompany = new AppCompany();

                //        //string query = "insert into tb_GstnApi(statecd,gstin, Datetime)Values ('" + CommonHelper.CompanyStateCode + "','" + CommonHelper.CompanyGSTN + "','" + DateTime.Now + "')";
                //        //objCompany.IUDData(query);

                //        RootObject obj = new RootObject();
                //        obj.statecd = CommonHelper.CompanyStateCode;
                //        obj.gstin = CommonHelper.CompanyGSTN;
                //        obj.GSTINUserName = Constants.UserName;
                //        obj.Datetime = DateTime.Now.ToString("yyyy-MM-dd");
                //        string json = JsonConvert.SerializeObject(obj);

                //        Application.DoEvents();
                //        WebRequest request1 = WebRequest.Create("http://13.126.181.225:8000/SPEQTAGSTOffLineUtility/InsertOffGstnApi");
                //        string responseFromServer = Utility.PostApiGSP(request1, json);

                //        GSPResClass objAppKey = JsonConvert.DeserializeObject<GSPResClass>(responseFromServer);

                //        if (objAppKey.Status != "1")
                //        {
                //            MessageBox.Show("Error in Insert Data...");
                //            return;
                //        }
                //        Application.DoEvents();

                //        string s1 = builder.OTPRequest();
                //        if (Convert.ToString(s1) != "")
                //        {
                //            builder.AuthenticationRequest(s1);

                //            #region gstr-3B

                //            GSTR3BJson objJson = new GSTR3BJson();
                //            if (objJson.generateJSON("GSP").Trim() != "")
                //            {
                //                pbGetSummary.Enabled = true;
                //                pbUploadInv.Enabled = false;
                //                pbSubmitReturn.Enabled = false;
                //                pbFileGSTR.Enabled = false;
                //                //pbUploadInv.BackgroundImage = (System.Drawing.Image)Properties.Resources._1;
                //                //pbGetSummary.BackgroundImage = (System.Drawing.Image)Properties.Resources.Btn_2;
                //            }
                //            else
                //            { pbUploadInv.Enabled = true; }
                //            #endregion

                //            pbGSTR1.Visible = false;
                //        }
                //    }
                //}
                #endregion
            }
            catch (Exception ex)
            {
                pbGSTR1.Visible = false;
                if (ex.Message.Contains("403"))
                {
                    SPQGstLogin frm = new SPQGstLogin();
                    frm.Visible = false;
                    var result = frm.ShowDialog();
                    if (result != DialogResult.OK)
                    {
                        //GstLogin objLogin = new GstLogin();
                        //objLogin.Show();
                    }
                    else
                    {
                        btnNaxt_Click(sender, e);
                    }
                }
                else if (!ex.Message.Contains("403"))
                {
                    pbGSTR1.Visible = false;
                    string errorMessage = string.Format("Error:{0}{1}Source:{2}{3}Error Time:{4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                    StreamWriter errorWriter = new StreamWriter("Update_Error_File.txt", true);
                    errorWriter.Write(errorMessage);
                    errorWriter.Close();
                }
            }
            //pbGetSummary.Image = (System.Drawing.Image)Properties.Resources.Btn_2_Dark;
            //MessageBox.Show("Successfully Uploaded Invoices to GSTN", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //btnCompare.Visible = false;
            //Getdata();
        }

        public void GetDataFileOption()
        {
            try
            {
                string Query = "Select * from SPQR3BFileOption where Fld_Month='" + CommonHelper.SelectedMonth + "' and Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                DataTable dt = new DataTable();
                dt = objGSTR1A.GetDataGSTR1A(Query);

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        if (Convert.ToString(dt.Rows[i]["Fld_OptionName"]) == "A")
                        {
                            if (Convert.ToString(dt.Rows[i]["Fld_OptionType"]) == "Y")
                                rbtAYes.Checked = true;
                            else if (Convert.ToString(dt.Rows[i]["Fld_OptionType"]) == "N")
                                rbtANo.Checked = true;
                        }
                        if (Convert.ToString(dt.Rows[i]["Fld_OptionName"]) == "B")
                        {
                            if (Convert.ToString(dt.Rows[i]["Fld_OptionType"]) == "Y")
                                rbtBYes.Checked = true;
                            else if (Convert.ToString(dt.Rows[i]["Fld_OptionType"]) == "N")
                                rbtBNo.Checked = true;
                        }
                        if (Convert.ToString(dt.Rows[i]["Fld_OptionName"]) == "C")
                        {
                            if (Convert.ToString(dt.Rows[i]["Fld_OptionType"]) == "Y")
                                rbtCYes.Checked = true;
                            else if (Convert.ToString(dt.Rows[i]["Fld_OptionType"]) == "N")
                                rbtCNo.Checked = true;
                        }
                        if (Convert.ToString(dt.Rows[i]["Fld_OptionName"]) == "D")
                        {
                            if (Convert.ToString(dt.Rows[i]["Fld_OptionType"]) == "Y")
                                rbtDYes.Checked = true;
                            else if (Convert.ToString(dt.Rows[i]["Fld_OptionType"]) == "N")
                                rbtDNo.Checked = true;
                        }
                        if (Convert.ToString(dt.Rows[i]["Fld_OptionName"]) == "E")
                        {
                            if (Convert.ToString(dt.Rows[i]["Fld_OptionType"]) == "Y")
                                rbtEYes.Checked = true;
                            else if (Convert.ToString(dt.Rows[i]["Fld_OptionType"]) == "N")
                                rbtENo.Checked = true;
                        }
                        if (Convert.ToString(dt.Rows[i]["Fld_OptionName"]) == "F")
                        {
                            if (Convert.ToString(dt.Rows[i]["Fld_OptionType"]) == "Y")
                                rbtFYes.Checked = true;
                            else if (Convert.ToString(dt.Rows[i]["Fld_OptionType"]) == "N")
                                rbtFNo.Checked = true;
                        }
                        if (Convert.ToString(dt.Rows[i]["Fld_OptionName"]) == "G")
                        {
                            if (Convert.ToString(dt.Rows[i]["Fld_OptionType"]) == "Y")
                                rbtGYes.Checked = true;
                            else if (Convert.ToString(dt.Rows[i]["Fld_OptionType"]) == "N")
                                rbtGNo.Checked = true;
                        }
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

        private void btnDownloadPDF_Click(object sender, EventArgs e)
        {
            bool isExtensionInstalled = Utility.chromeExtensionCheck();

            if (true)
            {
                string GetMonth = CommonHelper.GetMonth(CommonHelper.SelectedMonth);
                string Year = CommonHelper.ReturnYear.Replace(" ", "");

                string[] Years = Year.Split('-');
                Year = Years[0] + "-" + Years[1].Substring(2, 2);

                var encodedString = Utility.encoding(Convert.ToString(Constants.UserName) + ',' + Convert.ToString(CommonHelper.CompanyPassword) + ',' + "downloadgst3b" + ',' + GetMonth + ',' + Year);
                Process.Start("chrome.exe", "https://services.gst.gov.in/services/login?downloadgst3b," + encodedString);
            }
        }

        private void rbtAYes_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbtAYes.Checked == true)
                {
                    rbtBYes.Enabled = false;
                    rbtBNo.Enabled = false;
                    rbtBNo.Checked = true;

                    rbtCYes.Enabled = false;
                    rbtCNo.Enabled = false;
                    rbtCNo.Checked = true;


                    rbtDYes.Enabled = false;
                    rbtDNo.Enabled = false;
                    rbtDNo.Checked = true;

                    rbtEYes.Enabled = false;
                    rbtENo.Enabled = false;
                    rbtENo.Checked = true;

                    rbtFYes.Enabled = false;
                    rbtFNo.Enabled = false;
                    rbtFNo.Checked = true;
                }
                else
                {
                    rbtBYes.Enabled = true;
                    rbtBNo.Enabled = true;
                    rbtBNo.Checked = false;

                    rbtCYes.Enabled = true;
                    rbtCNo.Enabled = true;
                    rbtCNo.Checked = false;

                    rbtDYes.Enabled = true;
                    rbtDNo.Enabled = true;
                    rbtDNo.Checked = false;

                    rbtEYes.Enabled = true;
                    rbtENo.Enabled = true;
                    rbtENo.Checked = false;

                    rbtFYes.Enabled = true;
                    rbtFNo.Enabled = true;
                    rbtFNo.Checked = false;
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

    }
}
