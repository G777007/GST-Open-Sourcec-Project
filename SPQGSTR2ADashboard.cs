using SPEQTAGST.BAL;
using SPEQTAGST.BAL.M125r2a;
using SPEQTAGST.BAL.M112t;
using SPQ.Helper;
using SPEQTAGST.Usermain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Web.Script.Serialization;
using SPEQTAGST.Softmodel;
using SPQ.Automation;
using loadtogstin;
using SPEQTAGST.BAL.H179rp;


namespace SPEQTAGST.cachsR2a
{
    public partial class SPQGSTR2ADashboard : Form
    {
        R2APublicclass objGSTR2A = new R2APublicclass();
        private HttpWebResponse response;
        AssesseeDetail assesseeModel;
        CookieContainer Cc = new CookieContainer();

        public SPQGSTR2ADashboard()
        {
            InitializeComponent();
            //ToolTip.Show("Tooltip text goes here", msRequest);            
            //ToolTip toolTip1 = new ToolTip();
            //toolTip1.SetToolTip(this.msRequest, "My button1");
            //ToolStripMenuItem msRequest = new System.Windows.Forms.ToolStripMenuItem();
            //ToolTip toolTip1 = new System.Windows.Forms.ToolTip();
            //toolTip1.SetToolTip(msRequest, "test");
            //msRequest.ToolTipText = "Json Download Ahiya";

            Getdata();
            GetMessage();
            ((SPQMDI)Application.OpenForms["SPQMDI"]).SetReturnName(CommonHelper.ReturnName);

            DgvMain.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            DgvMain.EnableHeadersVisualStyles = false;
            DgvMain.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);
            DgvMain.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DgvMain.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            

        }

        public void Getdata()
        {
            try
            {
                DataTable dt1 = new DataTable();
                DataTable dt = new DataTable();

                dt1.Columns.Add("Type of Invoices", typeof(string));
                //dt1.Columns.Add("Validation Status", typeof(string));
                dt1.Columns.Add("Status", typeof(string));
                dt1.Columns.Add("NOofInv", typeof(string));
                //dt1.Columns.Add("InvValue", typeof(string));
                dt1.Columns.Add("InvTaxVal", typeof(string));
                dt1.Columns.Add("IGST", typeof(string));
                dt1.Columns.Add("CGST", typeof(string));
                dt1.Columns.Add("SGST", typeof(string));
                dt1.Columns.Add("Cess", typeof(string));

                #region Form 3
                string Query = "Select * from SPQR2AInwardSupplies where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_ReverseCharge == 'False' and Fld_FileStatus == 'Total'";
                dt = new DataTable();
                dt = objGSTR2A.GetDataGSTR2A(Query);

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (Convert.ToString(dt.Rows[0]["Fld_POS"]) == "")
                        dt.Rows[0]["Fld_POS"] = "0";
                    if (Convert.ToString(dt.Rows[0]["Fld_InvoiceNo"]) == "")
                        dt.Rows[0]["Fld_InvoiceNo"] = "0";
                    if (Convert.ToString(dt.Rows[0]["Fld_InvoiceTaxableVal"]) == "")
                        dt.Rows[0]["Fld_InvoiceTaxableVal"] = "0";
                    if (Convert.ToString(dt.Rows[0]["Fld_IntTax"]) == "")
                        dt.Rows[0]["Fld_IntTax"] = "0";
                    if (Convert.ToString(dt.Rows[0]["Fld_CtrlTax"]) == "")
                        dt.Rows[0]["Fld_CtrlTax"] = "0";
                    if (Convert.ToString(dt.Rows[0]["Fld_StateTax"]) == "")
                        dt.Rows[0]["Fld_StateTax"] = "0";
                    if (Convert.ToString(dt.Rows[0]["Fld_CessTax"]) == "")
                        dt.Rows[0]["Fld_CessTax"] = "0";

                    //Inward supplies received from a registered person other than the supplies attracting reverse charge
                    dt1.Rows.Add("Inward supplies received from a registered person", dt.Rows[0]["Fld_POS"].ToString(), dt.Rows[0]["Fld_InvoiceNo"].ToString(), dt.Rows[0]["Fld_InvoiceTaxableVal"].ToString(), dt.Rows[0]["Fld_IntTax"].ToString(), dt.Rows[0]["Fld_CtrlTax"].ToString(), dt.Rows[0]["Fld_StateTax"].ToString(), dt.Rows[0]["Fld_CessTax"].ToString());
                }
                else
                {
                    dt1.Rows.Add("Inward supplies received from a registered person", "-", "0", "0", "0", "0", "0", "0");
                }
                #endregion

                #region Form 4
                //Query = "Select * from SPQR2AInwardSupplies where Fld_Month='" + CommonHelper.SelectedMonth + "' and Fld_ReverseCharge='True' and Fld_FileStatus='Total'";
                //dt = new DataTable();
                //dt = objGSTR2A.GetDataGSTR2A(Query);

                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    dt1.Rows.Add("Inward supplies received from Registered Taxable Persons", dt.Rows[0]["Fld_POS"].ToString(), dt.Rows[0]["Fld_InvoiceNo"].ToString(), dt.Rows[0]["Fld_InvoiceTaxableVal"].ToString(), dt.Rows[0]["Fld_IntTax"].ToString(), dt.Rows[0]["Fld_CtrlTax"].ToString(), dt.Rows[0]["Fld_StateTax"].ToString(), dt.Rows[0]["Fld_CessTax"].ToString());
                //}
                //else
                //{
                //    dt1.Rows.Add("Inward supplies received from Registered Taxable Persons", "-", "0", "0", "0", "0", "0", "0");
                //}
                #endregion

                #region Form 4A
                //Query = "Select * from GSTR2AForm4A where Fld_Month='" + CommonHelper.SelectedMonth + "' order by Fld_Id DESC LIMIT 2";
                //dt = new DataTable();
                //dt = objGSTR2A.GetDataGSTR2A(Query);

                //if (dt != null && dt.Rows.Count == 2)
                //{
                //    dt1.Rows.Add("Amendments to details of inward supplies received in earlier tax periods", dt.Rows[1]["Fld_FileStatus"].ToString(), dt.Rows[0]["Fld_OInvoiceNo"].ToString(), dt.Rows[0]["Fld_RTaxable"].ToString(), dt.Rows[0]["Fld_IGSTAmnt"].ToString(), dt.Rows[0]["Fld_CGSTAmnt"].ToString(), dt.Rows[0]["Fld_SGSTAmnt"].ToString(), "0");
                //}
                //else
                //{
                //    dt1.Rows.Add("Amendments to details of inward supplies received in earlier tax periods", "-", "0", "0", "0", "0", "0", "0");
                //}
                #endregion

                #region Form 5
                Query = "Select * from SPQR2ACND where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2";
                dt = new DataTable();
                dt = objGSTR2A.GetDataGSTR2A(Query);

                if (dt != null && dt.Rows.Count == 2)
                {
                    if (Convert.ToString(dt.Rows[0]["Fld_DbtCrdtNoteNo"]) == "")
                        dt.Rows[0]["Fld_DbtCrdtNoteNo"] = "0";
                    if (Convert.ToString(dt.Rows[0]["Fld_Taxable"]) == "")
                        dt.Rows[0]["Fld_Taxable"] = "0";
                    if (Convert.ToString(dt.Rows[0]["Fld_IGSTAmnt"]) == "")
                        dt.Rows[0]["Fld_IGSTAmnt"] = "0";
                    if (Convert.ToString(dt.Rows[0]["Fld_CGSTAmnt"]) == "")
                        dt.Rows[0]["Fld_CGSTAmnt"] = "0";
                    if (Convert.ToString(dt.Rows[0]["Fld_SGSTAmnt"]) == "")
                        dt.Rows[0]["Fld_SGSTAmnt"] = "0";
                    if (Convert.ToString(dt.Rows[0]["Fld_CessAmnt"]) == "")
                        dt.Rows[0]["Fld_CessAmnt"] = "0";

                    dt1.Rows.Add("Details of Credit/Debit Notes", dt.Rows[1]["Fld_FileStatus"].ToString(), dt.Rows[0]["Fld_DbtCrdtNoteNo"].ToString(), dt.Rows[0]["Fld_Taxable"].ToString(), dt.Rows[0]["Fld_IGSTAmnt"].ToString(), dt.Rows[0]["Fld_CGSTAmnt"].ToString(), dt.Rows[0]["Fld_SGSTAmnt"].ToString(), dt.Rows[0]["Fld_CessAmnt"].ToString());
                }
                else
                {
                    dt1.Rows.Add("Details of Credit/Debit Notes", "-", "0", "0", "0", "0", "0", "0");
                }
                #endregion

                #region B2BA
                Query = "Select * from SPQR2AB2BAmend where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2";
                dt = new DataTable();
                dt = objGSTR2A.GetDataGSTR2A(Query);

                if (dt != null && dt.Rows.Count == 2)
                {

                    if (Convert.ToString(dt.Rows[0]["Fld_OrgInvoiceNo"]) == "")
                        dt.Rows[0]["Fld_OrgInvoiceNo"] = "0";
                    if (Convert.ToString(dt.Rows[0]["Fld_TaxableVal"]) == "")
                        dt.Rows[0]["Fld_TaxableVal"] = "0";
                    if (Convert.ToString(dt.Rows[0]["Fld_IntTax"]) == "")
                        dt.Rows[0]["Fld_IntTax"] = "0";
                    if (Convert.ToString(dt.Rows[0]["Fld_CtrlTax"]) == "")
                        dt.Rows[0]["Fld_CtrlTax"] = "0";
                    if (Convert.ToString(dt.Rows[0]["Fld_StateTax"]) == "")
                        dt.Rows[0]["Fld_StateTax"] = "0";
                    if (Convert.ToString(dt.Rows[0]["Fld_CessTax"]) == "")
                        dt.Rows[0]["Fld_CessTax"] = "0";

                    dt1.Rows.Add("Amendments to B2B Invoices", dt.Rows[1]["Fld_FileStatus"].ToString(), dt.Rows[0]["Fld_OrgInvoiceNo"].ToString(), dt.Rows[0]["Fld_TaxableVal"].ToString(), dt.Rows[0]["Fld_IntTax"].ToString(), dt.Rows[0]["Fld_CtrlTax"].ToString(), dt.Rows[0]["Fld_StateTax"].ToString(), dt.Rows[0]["Fld_CessTax"].ToString());
                }
                else
                {
                    dt1.Rows.Add("Amendments to B2B Invoices", "-", "0", "0", "0", "0", "0", "0");
                }
                #endregion

                #region CDNA
                Query = "Select * from SPQR2ACNDAmend where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2";
                dt = new DataTable();
                dt = objGSTR2A.GetDataGSTR2A(Query);

                if (dt != null && dt.Rows.Count == 2)
                {

                    if (Convert.ToString(dt.Rows[0]["Fld_DbtCrdtNoteNo"]) == "")
                        dt.Rows[0]["Fld_DbtCrdtNoteNo"] = "0";
                    if (Convert.ToString(dt.Rows[0]["Fld_Taxable"]) == "")
                        dt.Rows[0]["Fld_Taxable"] = "0";
                    if (Convert.ToString(dt.Rows[0]["Fld_IGSTAmnt"]) == "")
                        dt.Rows[0]["Fld_IGSTAmnt"] = "0";
                    if (Convert.ToString(dt.Rows[0]["Fld_CGSTAmnt"]) == "")
                        dt.Rows[0]["Fld_CGSTAmnt"] = "0";
                    if (Convert.ToString(dt.Rows[0]["Fld_SGSTAmnt"]) == "")
                        dt.Rows[0]["Fld_SGSTAmnt"] = "0";
                    if (Convert.ToString(dt.Rows[0]["Fld_CessAmnt"]) == "")
                        dt.Rows[0]["Fld_CessAmnt"] = "0";

                    dt1.Rows.Add("Details of Credit/Debit Notes", dt.Rows[1]["Fld_FileStatus"].ToString(), dt.Rows[0]["Fld_DbtCrdtNoteNo"].ToString(), dt.Rows[0]["Fld_Taxable"].ToString(), dt.Rows[0]["Fld_IGSTAmnt"].ToString(), dt.Rows[0]["Fld_CGSTAmnt"].ToString(), dt.Rows[0]["Fld_SGSTAmnt"].ToString(), dt.Rows[0]["Fld_CessAmnt"].ToString());
                }
                else
                {
                    dt1.Rows.Add("Amendments to Credit/Debit Notes", "-", "0", "0", "0", "0", "0", "0");
                }
                #endregion

                #region Form 5A
                //Query = "Select * from GSTR2AForm5A where Fld_Month='" + CommonHelper.SelectedMonth + "' order by Fld_Id DESC LIMIT 2";
                //dt = new DataTable();
                //dt = objGSTR2A.GetDataGSTR2A(Query);

                //if (dt != null && dt.Rows.Count == 2)
                //{
                //    dt1.Rows.Add("Amendment to Details of Credit/Debit Notes of earlier tax periods", dt.Rows[1]["Fld_FileStatus"].ToString(), dt.Rows[0]["Fld_ORNo"].ToString(), "0", dt.Rows[0]["Fld_IGSTAmnt"].ToString(), dt.Rows[0]["Fld_CGSTAmnt"].ToString(), dt.Rows[1]["Fld_SGSTAmnt"].ToString(), "0");
                //}
                //else
                //{
                //    dt1.Rows.Add("Amendment to Details of Credit/Debit Notes of earlier tax periods", "-", "0", "0", "0", "0", "0", "0");
                //}
                #endregion

                #region Form 6
                //Query = "Select * from GSTR2AForm6 where Fld_Month='" + CommonHelper.SelectedMonth + "' order by Fld_Id DESC LIMIT 2";
                //dt = new DataTable();
                //dt = objGSTR2A.GetDataGSTR2A(Query);

                //if (dt != null && dt.Rows.Count == 2)
                //{
                //    dt1.Rows.Add("ISD credit received", dt.Rows[1]["Fld_FileStatus"].ToString(), dt.Rows[0]["Fld_InvoiceNo"].ToString(), "0", dt.Rows[0]["Fld_IGST"].ToString(), dt.Rows[0]["Fld_CGST"].ToString(), dt.Rows[0]["Fld_SGST"].ToString(), "0");
                //}
                //else
                //{
                //    dt1.Rows.Add("ISD credit received", "-", "0", "0", "0", "0", "0", "0");
                //}

                #endregion

                #region Form 71
                //Query = "Select * from GSTR2AForm71 where Fld_Month='" + CommonHelper.SelectedMonth + "' order by Fld_Id DESC LIMIT 2";
                //dt = new DataTable();
                //dt = objGSTR2A.GetDataGSTR2A(Query);

                //if (dt != null && dt.Rows.Count == 2)
                //{
                //    dt1.Rows.Add("TDS Credit received", dt.Rows[1]["Fld_FileStatus"].ToString(), dt.Rows[0]["Fld_InvoiceNo"].ToString(), dt.Rows[0]["Fld_ValueOn"], dt.Rows[0]["Fld_IGSTAmount"].ToString(), dt.Rows[0]["Fld_CGSTAmount"].ToString(), dt.Rows[0]["Fld_SGSTAmount"].ToString(), "0");
                //}
                //else
                //{
                //    dt1.Rows.Add("TDS Credit received", "-", "0", "0", "0", "0", "0", "0");
                //}
                #endregion

                #region Form 72
                //Query = "Select * from GSTR2AForm72 where Fld_Month='" + CommonHelper.SelectedMonth + "' order by Fld_Id DESC LIMIT 2";
                //dt = new DataTable();
                //dt = objGSTR2A.GetDataGSTR2A(Query);

                //if (dt != null && dt.Rows.Count == 2)
                //{
                //    dt1.Rows.Add("TDS Credit received", dt.Rows[1]["Fld_FileStatus"].ToString(), "0", dt.Rows[0]["Fld_Taxable"], dt.Rows[0]["Fld_IGSTAmount"].ToString(), dt.Rows[0]["Fld_SGSTAmount"].ToString(), dt.Rows[0]["Fld_SGSTAmount"].ToString(), "0");
                //}
                //else
                //{
                //    dt1.Rows.Add("TDS Credit received", "-", "0", "0", "0", "0", "0", "0");
                //}
                #endregion

                dt1.Rows.Add("", "-", "0", "0", "0", "0", "0", "0");
                dt1.Columns["Type of Invoices"].ColumnName = "Type of Invoices";
                // dt1.Columns["Validation Status"].ColumnName = "Validation Status";
                dt1.Columns["Status"].ColumnName = "Status";
                dt1.Columns["NOofInv"].ColumnName = "Document Count";
                // dt1.Columns["InvValue"].ColumnName = "Invoice Value";
                dt1.Columns["InvTaxVal"].ColumnName = "Taxable Value";
                dt1.Columns["IGST"].ColumnName = "IGST";
                dt1.Columns["CGST"].ColumnName = "CGST";
                dt1.Columns["SGST"].ColumnName = "SGST";
                dt1.Columns["Cess"].ColumnName = "Cess";
                DgvMain.DataSource = dt1;
                DgvMain.Columns["Type of Invoices"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                DgvMain.ColumnHeadersHeight = 50;
                DataGridViewRow row = this.DgvMain.RowTemplate;
                row.MinimumHeight = 25;
                // DgvMain.Columns["Status"].Width = 120;
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
                        if (DgvMain.Rows[i].Cells[j].Value.ToString() == "-" || DgvMain.Rows[i].Cells[j].Value.ToString() == "" || DgvMain.Rows[i].Cells[j].Value == null)
                        {
                            DgvMain.Rows[i].Cells[j].Value = "0";
                        }
                    }
                }


                #region Add Total and Account and Diffrent
                decimal NOofInv = DgvMain.Rows.Cast<DataGridViewRow>()
                    .Sum(t => Convert.ToDecimal(t.Cells["Document Count"].Value));
                //  int InvValue = DgvMain.Rows.Cast<DataGridViewRow>()
                //.Sum(t => Convert.ToInt32(t.Cells["Invoice Value"].Value));
                decimal InvTaxVal = DgvMain.Rows.Cast<DataGridViewRow>()
                    .Sum(t => Convert.ToDecimal(t.Cells["Taxable Value"].Value));
                decimal IGST = DgvMain.Rows.Cast<DataGridViewRow>()
                    .Sum(t => Convert.ToDecimal(t.Cells["IGST"].Value));
                decimal CGST = DgvMain.Rows.Cast<DataGridViewRow>()
                    .Sum(t => Convert.ToDecimal(t.Cells["CGST"].Value));
                decimal SGST = DgvMain.Rows.Cast<DataGridViewRow>()
                    .Sum(t => Convert.ToDecimal(t.Cells["SGST"].Value));
                decimal Cess = DgvMain.Rows.Cast<DataGridViewRow>()
                    .Sum(t => Convert.ToDecimal(t.Cells["Cess"].Value));

                DgvMain.Rows[DgvMain.Rows.Count - 1].Cells["Document Count"].Value = NOofInv.ToString();
                DgvMain.Rows[DgvMain.Rows.Count - 1].Cells["Taxable Value"].Value = InvTaxVal.ToString();
                //DgvMain.Rows[DgvMain.Rows.Count - 1].Cells["Invoice Value"].Value = InvValue.ToString();
                DgvMain.Rows[DgvMain.Rows.Count - 1].Cells["IGST"].Value = IGST.ToString();
                DgvMain.Rows[DgvMain.Rows.Count - 1].Cells["CGST"].Value = CGST.ToString();
                DgvMain.Rows[DgvMain.Rows.Count - 1].Cells["SGST"].Value = SGST.ToString();
                DgvMain.Rows[DgvMain.Rows.Count - 1].Cells["Cess"].Value = Cess.ToString();
                DgvMain.Rows[DgvMain.Rows.Count - 1].Cells["Type of Invoices"].Value = "                                                                           Total";

                DgvMain.Refresh();
                Application.DoEvents();

                //DataTable dtAcc = new DataTable();
                //if (dgvaccount.Columns.Count == 0)
                //{
                //    foreach (DataGridViewColumn dgvc in DgvMain.Columns)
                //    {
                //        dgvaccount.Columns.Add(dgvc.Clone() as DataGridViewColumn);
                //    }
                //}
                //dtAcc = dt1.Clone();
                //dtAcc.Rows.Add("As per Account ", "", "", "", "", "", "", "", "");
                //dgvaccount.DataSource = dtAcc;
                //dgvaccount.Columns[0].ReadOnly = true;
                //dgvaccount.Columns[1].ReadOnly = true;

                //if (dgvdiff.Columns.Count == 0)
                //{
                //    foreach (DataGridViewColumn dgvc in DgvMain.Columns)
                //    {
                //        dgvdiff.Columns.Add(dgvc.Clone() as DataGridViewColumn);
                //    }
                //}
                //DataTable dtDiff = new DataTable();
                //dtDiff = dt1.Clone();
                //dtDiff.Rows.Add("Difference ", "", "", "", "", "", "", "", "");
                //dgvdiff.DataSource = dtDiff;
                //dgvdiff.ReadOnly = true;
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

        public void GetMessage()
        {
            try
            {
                DataTable dt = new DataTable();
                string Query = "Select * from SPQJsonDownloadMsg where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_ReturnType='GSTR2A'";
                dt = objGSTR2A.GetDataGSTR2A(Query);
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (Convert.ToDateTime(dt.Rows[0]["Fld_DownloadTime"]) > DateTime.Now)
                    {
                        label2.Text = Convert.ToString(dt.Rows[0]["Fld_Msg"]) + "...";
                    }
                    else
                    {
                        label2.Text = "You can download file now...";
                    }
                }
                else
                {
                    label2.Text = "";
                }


            }
            catch
            {
            }
        }

        private void DgvMain_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CommonHelper.IsMainFormType = "2A";

            if (e.RowIndex == 0 && e.ColumnIndex == 0)
            {
                SPQGSTR2AB2B obj = new SPQGSTR2AB2B();
                obj.MdiParent = this.MdiParent;
                Utility.CloseAllOpenForm();
                obj.Dock = DockStyle.Fill;
                obj.Show();

                ((SPQMDI)Application.OpenForms["SPQMDI"]).ShowExtraToolsMenu();
                ((SPQMDI)Application.OpenForms["SPQMDI"]).ShowCompanyDetailMenu();
            }
            else if (e.RowIndex == 1 && e.ColumnIndex == 0)
            {
                //  btnGSTR16.Text = "Please wait...";
                SPQGSTR2ACND obj = new SPQGSTR2ACND();
                obj.MdiParent = this.MdiParent;
                Utility.CloseAllOpenForm();
                obj.Dock = DockStyle.Fill;
                obj.Show();
                ((SPQMDI)Application.OpenForms["SPQMDI"]).ShowExtraToolsMenu();
                ((SPQMDI)Application.OpenForms["SPQMDI"]).ShowCompanyDetailMenu();
            }
            else if (e.RowIndex == 2 && e.ColumnIndex == 0)
            {
                SPQGSTR2AB2BA obj = new SPQGSTR2AB2BA();
                obj.MdiParent = this.MdiParent;
                Utility.CloseAllOpenForm();
                obj.Dock = DockStyle.Fill;
                obj.Show();
                ((SPQMDI)Application.OpenForms["SPQMDI"]).ShowExtraToolsMenu();
                ((SPQMDI)Application.OpenForms["SPQMDI"]).ShowCompanyDetailMenu();
            }
            else if (e.RowIndex == 3 && e.ColumnIndex == 0)
            {
                //MessageBox.Show("We are working on this Feature and it will be released very soon. Thanks for your support and co-operation.");
                SPQGSTR2ACNDA obj = new SPQGSTR2ACNDA();
                obj.MdiParent = this.MdiParent;
                Utility.CloseAllOpenForm();
                obj.Dock = DockStyle.Fill;
                obj.Show();
                ((SPQMDI)Application.OpenForms["SPQMDI"]).ShowExtraToolsMenu();
                ((SPQMDI)Application.OpenForms["SPQMDI"]).ShowCompanyDetailMenu();
            }
            else
            {

            }
        }

        #region Functional Methods

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

        private Boolean chkCellValue(string cellValue)
        {
            try
            {
                if (Utility.IsNumber(cellValue))
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public bool b2bEntry(string jsonData, string strMonth, string ReturnYear)
        {
            bool flg = false;
            try
            {
                //DataTable dtComp = objGSTR2A.GetDataGSTR2A("Select * from ");


                DataTable dt = new DataTable();
                RootObject obj = JsonConvert.DeserializeObject<RootObject>(jsonData);

                //if (obj != null && obj.b2b != null && obj.b2b.Count > 0)
                //    obj.b2b = obj.b2b.Where(x => x.cfs == "Y").ToList();

                #region ASSIGN GRIDVIEW ROWS IN DATATABLE For B2B

                dt = new DataTable();
                dt.Columns.Add("Fld_Sequence");
                dt.Columns.Add("Fld_GSTIN");
                dt.Columns.Add("Fld_NameOfParty");
                dt.Columns.Add("Fld_InvoiceNo");
                dt.Columns.Add("Fld_InvoiceDate");
                dt.Columns.Add("Fld_InvoiceValue");
                dt.Columns.Add("Fld_Rate");
                dt.Columns.Add("Fld_InvoiceTaxableVal");
                dt.Columns.Add("Fld_IntTax");
                dt.Columns.Add("Fld_CtrlTax");
                dt.Columns.Add("Fld_StateTax");
                dt.Columns.Add("Fld_CessTax");
                dt.Columns.Add("Fld_POS");
                dt.Columns.Add("Fld_ReverseCharge");
                dt.Columns.Add("Fld_InvoiceType");
                dt.Columns.Add("Fld_Submitted");
                dt.Columns.Add("Fld_FileStatus");

                if (obj != null && obj.b2b != null)
                {
                    for (int i = 0; i < obj.b2b.Count; i++)
                    {
                        for (int j = 0; j < obj.b2b[i].inv.Count; j++)
                        {
                            for (int k = 0; k < obj.b2b[i].inv[j].itms.Count; k++)
                            {
                                dt.Rows.Add();

                                #region root element
                                dt.Rows[dt.Rows.Count - 1]["Fld_GSTIN"] = Convert.ToString(obj.b2b[i].ctin);
                                dt.Rows[dt.Rows.Count - 1]["Fld_NameOfParty"] = Convert.ToString(obj.b2b[i].cname).Replace("'", "");
                                dt.Rows[dt.Rows.Count - 1]["Fld_Submitted"] = (Convert.ToString(obj.b2b[i].cfs) == "Y" ? "Yes" : "No");
                                #endregion

                                #region invoice details
                                dt.Rows[dt.Rows.Count - 1]["Fld_InvoiceNo"] = Convert.ToString(obj.b2b[i].inv[j].inum);
                                dt.Rows[dt.Rows.Count - 1]["Fld_InvoiceDate"] = Convert.ToString(obj.b2b[i].inv[j].idt);
                                dt.Rows[dt.Rows.Count - 1]["Fld_InvoiceValue"] = Convert.ToString(obj.b2b[i].inv[j].val);
                                dt.Rows[dt.Rows.Count - 1]["Fld_InvoiceType"] = GetVal(Convert.ToString(obj.b2b[i].inv[j].inv_typ));
                                if (Convert.ToString(obj.b2b[i].inv[j].pos) != null)
                                    dt.Rows[dt.Rows.Count - 1]["Fld_POS"] = CommonHelper.GetStateName(Convert.ToString(obj.b2b[i].inv[j].pos));
                                dt.Rows[dt.Rows.Count - 1]["Fld_ReverseCharge"] = (Convert.ToString(obj.b2b[i].inv[j].rchrg) == "Y" ? "Yes" : "No");
                                #endregion

                                #region item details
                                dt.Rows[dt.Rows.Count - 1]["Fld_Rate"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.rt);
                                dt.Rows[dt.Rows.Count - 1]["Fld_InvoiceTaxableVal"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.txval);

                                //if (Convert.ToDouble(obj.b2b[i].inv[j].itms[k].itm_det.iamt) == 0.0)
                                //    dt.Rows[dt.Rows.Count - 1]["Fld_IntTax"] = "";
                                //else
                                //    dt.Rows[dt.Rows.Count - 1]["Fld_IntTax"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.iamt);

                                //if (Convert.ToDouble(obj.b2b[i].inv[j].itms[k].itm_det.camt) == 0.0)
                                //    dt.Rows[dt.Rows.Count - 1]["Fld_CtrlTax"] = "";
                                //else
                                //    dt.Rows[dt.Rows.Count - 1]["Fld_CtrlTax"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.camt);

                                //if (Convert.ToDouble(obj.b2b[i].inv[j].itms[k].itm_det.samt) == 0.0)
                                //    dt.Rows[dt.Rows.Count - 1]["Fld_StateTax"] = "";
                                //else
                                //    dt.Rows[dt.Rows.Count - 1]["Fld_StateTax"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.samt);

                                //if (Convert.ToDouble(obj.b2b[i].inv[j].itms[k].itm_det.csamt) == 0.0)
                                //    dt.Rows[dt.Rows.Count - 1]["Fld_CessTax"] = "";
                                //else
                                //    dt.Rows[dt.Rows.Count - 1]["Fld_CessTax"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.csamt);

                                dt.Rows[dt.Rows.Count - 1]["Fld_IntTax"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.iamt);
                                dt.Rows[dt.Rows.Count - 1]["Fld_CtrlTax"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.camt);
                                dt.Rows[dt.Rows.Count - 1]["Fld_StateTax"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.samt);
                                dt.Rows[dt.Rows.Count - 1]["Fld_CessTax"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.csamt);

                                //dt.Rows[dt.Rows.Count - 1]["Fld_IntTax"] = Convert.ToString(obj.b2b[i].inv[j].iamt);
                                //dt.Rows[dt.Rows.Count - 1]["Fld_CtrlTax"] = Convert.ToString(obj.b2b[i].inv[j].camt);
                                //dt.Rows[dt.Rows.Count - 1]["Fld_StateTax"] = Convert.ToString(obj.b2b[i].inv[j].samt);
                                //dt.Rows[dt.Rows.Count - 1]["Fld_CessTax"] = Convert.ToString(obj.b2b[i].inv[j].csamt);
                                #endregion

                                #region Calculate
                                //if (Convert.ToDecimal(obj.b2b[i].inv[j].itms[k].itm_det.rt) > 0 && Convert.ToDecimal(obj.b2b[i].inv[j].itms[k].itm_det.iamt) == 0)
                                //{
                                //    decimal Amount = Convert.ToDecimal(obj.b2b[i].inv[j].itms[k].itm_det.rt) * Convert.ToDecimal(obj.b2b[i].inv[j].itms[k].itm_det.txval) / 100;

                                //    decimal Amt = Amount / 2;
                                //    Amt = Math.Round(Amt, 2);
                                //    dt.Rows[dt.Rows.Count - 1]["Fld_CtrlTax"] = Amt.ToString();
                                //    dt.Rows[dt.Rows.Count - 1]["Fld_StateTax"] = Amt.ToString();
                                //}


                                //if (Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.rt) != "")
                                //{
                                //    int CompanyGSTNState = Convert.ToInt32(CommonHelper.CompanyGSTN.Substring(0, 2));
                                //    int StateId = Convert.ToString(obj.b2b[i].inv[j].pos).Trim() != "" ? Convert.ToInt32(obj.b2b[i].inv[j].pos) : 0;

                                //    if (StateId == CompanyGSTNState)
                                //    {
                                //        decimal Amount = Convert.ToDecimal(obj.b2b[i].inv[j].itms[k].itm_det.rt) * Convert.ToDecimal(obj.b2b[i].inv[j].itms[k].itm_det.txval) / 100;
                                //        decimal Amt = Amount / 2;
                                //        Amt = Math.Round(Amt, 2);
                                //        dt.Rows[dt.Rows.Count - 1]["Fld_CtrlTax"] = Amt.ToString();
                                //        dt.Rows[dt.Rows.Count - 1]["Fld_StateTax"] = Amt.ToString();
                                //    }
                                //    else
                                //    {
                                //        decimal IGSTAmt = Convert.ToDecimal(obj.b2b[i].inv[j].itms[k].itm_det.rt) * Convert.ToDecimal(obj.b2b[i].inv[j].itms[k].itm_det.txval) / 100;
                                //        IGSTAmt = Math.Round(IGSTAmt, 2);
                                //        dt.Rows[dt.Rows.Count - 1]["Fld_IntTax"] = IGSTAmt.ToString();
                                //    }
                                //}


                                //dt.Rows[dt.Rows.Count - 1]["Fld_IntTax"] = Convert.ToString(obj.b2b[i].inv[j].iamt);
                                //dt.Rows[dt.Rows.Count - 1]["Fld_CtrlTax"] = Convert.ToString(obj.b2b[i].inv[j].camt);
                                //dt.Rows[dt.Rows.Count - 1]["Fld_StateTax"] = Convert.ToString(obj.b2b[i].inv[j].samt);
                                //dt.Rows[dt.Rows.Count - 1]["Fld_CessTax"] = Convert.ToString(obj.b2b[i].inv[j].csamt);
                                #endregion
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
                    DataRow dr = dt.NewRow();
                    //dr["Fld_InvoiceNo"] = dt.Rows.Cast<DataRow>().Where(x => Convert.ToString(x["Fld_InvoiceNo"]).Trim() != "").GroupBy(x => x["Fld_InvoiceNo"]).Select(x => x.First()).Distinct().Count();

                    #region Invoice no
                    var result = (from row in dt.AsEnumerable()
                                  where row.Field<string>("Fld_InvoiceNo") != "" && row.Field<string>("Fld_GSTIN") != ""
                                  group row by new { colGSTIN = row.Field<string>("Fld_GSTIN"), colInvNo = row.Field<string>("Fld_InvoiceNo") } into grp
                                  select new
                                  {
                                      colGSTIN = grp.Key.colGSTIN,
                                      colInvNo = grp.Key.colInvNo,
                                  }).ToList();

                    if (result != null && result.Count > 0)
                        dr["Fld_InvoiceNo"] = result.Count;
                    else
                        dr["Fld_InvoiceNo"] = 0;
                    #endregion

                    dr["Fld_InvoiceValue"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_InvoiceValue"] != null).Sum(x => x["Fld_InvoiceValue"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_InvoiceValue"])).ToString();
                    dr["Fld_InvoiceTaxableVal"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_InvoiceTaxableVal"] != null).Sum(x => x["Fld_InvoiceTaxableVal"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_InvoiceTaxableVal"])).ToString();
                    dr["Fld_IntTax"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_IntTax"] != null).Sum(x => x["Fld_IntTax"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_IntTax"])).ToString();
                    dr["Fld_CtrlTax"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_CtrlTax"] != null).Sum(x => x["Fld_CtrlTax"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_CtrlTax"])).ToString();
                    dr["Fld_StateTax"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_StateTax"] != null).Sum(x => x["Fld_StateTax"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_StateTax"])).ToString();
                    dr["Fld_CessTax"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_CessTax"] != null).Sum(x => x["Fld_CessTax"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_CessTax"])).ToString();

                    dr["Fld_POS"] = "Completed";
                    dr["Fld_ReverseCharge"] = "False";
                    dr["Fld_FileStatus"] = "Total";
                    dt.Rows.Add(dr);

                    int _Result = objGSTR2A.GSTR2A3BulkEntryJson(dt, strMonth, ReturnYear);
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
        public bool cdnEntry(string jsonData, string strMonth, string ReturnYear)
        {
            bool flg = false;
            try
            {
                DataTable dt = new DataTable();
                RootObject obj = JsonConvert.DeserializeObject<RootObject>(jsonData);

                //if (obj != null && obj.cdn != null && obj.cdn.Count > 0)
                //    obj.cdn = obj.cdn.Where(x => x.cfs == "Y").ToList();

                #region ASSIGN GRIDVIEW ROWS IN DATATABLE For CDN
                dt = new DataTable();
                dt.Columns.Add("Fld_Sequence");
                dt.Columns.Add("Fld_GSTIN");
                dt.Columns.Add("Fld_PartyName");
                dt.Columns.Add("Fld_TypeOfNote");
                dt.Columns.Add("Fld_DbtCrdtNoteNo");
                dt.Columns.Add("Fld_DbtCrdtNoteDate");
                dt.Columns.Add("Fld_InvoiceNo");
                dt.Columns.Add("Fld_Regime");
                dt.Columns.Add("Fld_Issue");
                dt.Columns.Add("Fld_NoteValue");
                dt.Columns.Add("Fld_InvoiceDate");
                dt.Columns.Add("Fld_Rate");
                dt.Columns.Add("Fld_Taxable");
                dt.Columns.Add("Fld_IGSTAmnt");
                dt.Columns.Add("Fld_CGSTAmnt");
                dt.Columns.Add("Fld_SGSTAmnt");
                dt.Columns.Add("Fld_CessAmnt");
                dt.Columns.Add("Fld_Submitted");
                dt.Columns.Add("Fld_FileStatus");

                if (obj != null && obj.cdn != null)
                {
                    for (int i = 0; i < obj.cdn.Count; i++)
                    {
                        for (int j = 0; j < obj.cdn[i].nt.Count; j++)
                        {
                            for (int k = 0; k < obj.cdn[i].nt[j].itms.Count; k++)
                            {
                                dt.Rows.Add();

                                #region root element
                                dt.Rows[dt.Rows.Count - 1]["Fld_GSTIN"] = Convert.ToString(obj.cdn[i].ctin);
                                dt.Rows[dt.Rows.Count - 1]["Fld_PartyName"] = Convert.ToString(obj.cdn[i].cname).Replace("'", "");
                                dt.Rows[dt.Rows.Count - 1]["Fld_Submitted"] = (Convert.ToString(obj.cdn[i].cfs) == "Y" ? "Yes" : "No");
                                #endregion

                                #region invoice details
                                if (Convert.ToString(obj.cdn[i].nt[j].ntty) == "C")
                                    dt.Rows[dt.Rows.Count - 1]["Fld_TypeOfNote"] = "Credit Note";
                                else if (Convert.ToString(obj.cdn[i].nt[j].ntty) == "D")
                                    dt.Rows[dt.Rows.Count - 1]["Fld_TypeOfNote"] = "Debit Note";
                                else if (Convert.ToString(obj.cdn[i].nt[j].ntty) == "R")
                                    dt.Rows[dt.Rows.Count - 1]["Fld_TypeOfNote"] = "Refund Voucher";

                                dt.Rows[dt.Rows.Count - 1]["Fld_DbtCrdtNoteNo"] = Convert.ToString(obj.cdn[i].nt[j].nt_num);
                                dt.Rows[dt.Rows.Count - 1]["Fld_DbtCrdtNoteDate"] = Convert.ToString(obj.cdn[i].nt[j].nt_dt);
                                dt.Rows[dt.Rows.Count - 1]["Fld_InvoiceNo"] = Convert.ToString(obj.cdn[i].nt[j].inum);
                                dt.Rows[dt.Rows.Count - 1]["Fld_Regime"] = (Convert.ToString(obj.cdn[i].nt[j].p_gst) == "Y" ? "Yes" : "No");
                                dt.Rows[dt.Rows.Count - 1]["Fld_Issue"] = Convert.ToString(obj.cdn[i].nt[j].rsn);
                                dt.Rows[dt.Rows.Count - 1]["Fld_NoteValue"] = Convert.ToString(obj.cdn[i].nt[j].val);
                                dt.Rows[dt.Rows.Count - 1]["Fld_InvoiceDate"] = Convert.ToString(obj.cdn[i].nt[j].idt);
                                #endregion

                                #region item details
                                dt.Rows[dt.Rows.Count - 1]["Fld_Rate"] = Convert.ToString(obj.cdn[i].nt[j].itms[k].itm_det.rt);
                                dt.Rows[dt.Rows.Count - 1]["Fld_Taxable"] = Convert.ToString(obj.cdn[i].nt[j].itms[k].itm_det.txval);
                                dt.Rows[dt.Rows.Count - 1]["Fld_IGSTAmnt"] = Convert.ToString(obj.cdn[i].nt[j].itms[k].itm_det.iamt);
                                dt.Rows[dt.Rows.Count - 1]["Fld_CGSTAmnt"] = Convert.ToString(obj.cdn[i].nt[j].itms[k].itm_det.camt);
                                dt.Rows[dt.Rows.Count - 1]["Fld_SGSTAmnt"] = Convert.ToString(obj.cdn[i].nt[j].itms[k].itm_det.samt);
                                dt.Rows[dt.Rows.Count - 1]["Fld_CessAmnt"] = Convert.ToString(obj.cdn[i].nt[j].itms[k].itm_det.csamt);
                                //dt.Rows[dt.Rows.Count - 1]["Fld_IGSTAmnt"]= Convert.ToString(obj.b2b[i].inv[j].iamt);
                                //dt.Rows[dt.Rows.Count - 1]["Fld_CGSTAmnt"] = Convert.ToString(obj.b2b[i].inv[j].camt);
                                //dt.Rows[dt.Rows.Count - 1]["Fld_SGSTAmnt"] = Convert.ToString(obj.b2b[i].inv[j].samt);
                                //dt.Rows[dt.Rows.Count - 1]["Fld_CessAmnt"] = Convert.ToString(obj.b2b[i].inv[j].csamt);
                                #endregion
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
                    DataRow dr = dt.NewRow();
                    //dr["Fld_DbtCrdtNoteNo"] = dt.Rows.Cast<DataRow>().Where(x => Convert.ToString(x["Fld_DbtCrdtNoteNo"]).Trim() != "").GroupBy(x => x["Fld_DbtCrdtNoteNo"]).Select(x => x.First()).Distinct().Count();

                    #region CDN no
                    var result = (from row in dt.AsEnumerable()
                                  where row.Field<string>("Fld_DbtCrdtNoteNo") != "" && row.Field<string>("Fld_GSTIN") != ""
                                  group row by new { colGSTIN = row.Field<string>("Fld_GSTIN"), colInvNo = row.Field<string>("Fld_DbtCrdtNoteNo") } into grp
                                  select new
                                  {
                                      colGSTIN = grp.Key.colGSTIN,
                                      colInvNo = grp.Key.colInvNo,
                                  }).ToList();

                    if (result != null && result.Count > 0)
                        dr["Fld_DbtCrdtNoteNo"] = result.Count;
                    else
                        dr["Fld_DbtCrdtNoteNo"] = 0;
                    #endregion

                    #region New
                    DataTable dtNew = new DataTable();
                    dtNew = dt.AsEnumerable()
                        .GroupBy(row => new
                        {
                            Member1 = row.Field<string>("Fld_TypeOfNote")
                        })
                        .Select(g =>
                        {
                            var row = dt.NewRow();
                            row["Fld_TypeOfNote"] = g.Key.Member1;
                            row["Fld_NoteValue"] = g.Sum(r => r["Fld_NoteValue"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(r["Fld_NoteValue"]));
                            row["Fld_Taxable"] = g.Sum(r => r["Fld_Taxable"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(r["Fld_Taxable"]));
                            row["Fld_IGSTAmnt"] = g.Sum(r => r["Fld_IGSTAmnt"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(r["Fld_IGSTAmnt"]));
                            row["Fld_CGSTAmnt"] = g.Sum(r => r["Fld_CGSTAmnt"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(r["Fld_CGSTAmnt"]));
                            row["Fld_SGSTAmnt"] = g.Sum(r => r["Fld_SGSTAmnt"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(r["Fld_SGSTAmnt"]));
                            row["Fld_CessAmnt"] = g.Sum(r => r["Fld_CessAmnt"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(r["Fld_CessAmnt"]));
                            return row;
                        }).CopyToDataTable();

                    decimal invval = 0, tax = 0, igst = 0, cgst = 0, sgst = 0, cess = 0;
                    foreach (DataRow drNew in dtNew.Rows)
                    {
                        if (Convert.ToString(drNew["Fld_TypeOfNote"]) == "Credit Note")
                        {
                            invval = invval - (Convert.ToString(drNew["Fld_NoteValue"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_NoteValue"]));
                            tax = tax - (Convert.ToString(drNew["Fld_Taxable"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_Taxable"]));
                            igst = igst - (Convert.ToString(drNew["Fld_IGSTAmnt"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_IGSTAmnt"]));
                            cgst = cgst - (Convert.ToString(drNew["Fld_CGSTAmnt"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_CGSTAmnt"]));
                            sgst = sgst - (Convert.ToString(drNew["Fld_SGSTAmnt"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_SGSTAmnt"]));
                            cess = cess - (Convert.ToString(drNew["Fld_CessAmnt"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_CessAmnt"]));
                        }
                        else if (Convert.ToString(drNew["Fld_TypeOfNote"]) == "Refund Voucher")
                        {
                            invval = invval - (Convert.ToString(drNew["Fld_NoteValue"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_NoteValue"]));
                            tax = tax - (Convert.ToString(drNew["Fld_Taxable"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_Taxable"]));
                            igst = igst - (Convert.ToString(drNew["Fld_IGSTAmnt"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_IGSTAmnt"]));
                            cgst = cgst - (Convert.ToString(drNew["Fld_CGSTAmnt"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_CGSTAmnt"]));
                            sgst = sgst - (Convert.ToString(drNew["Fld_SGSTAmnt"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_SGSTAmnt"]));
                            cess = cess - (Convert.ToString(drNew["Fld_CessAmnt"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_CessAmnt"]));
                        }
                        else
                        {
                            invval = invval + (Convert.ToString(drNew["Fld_NoteValue"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_NoteValue"]));
                            tax = tax + (Convert.ToString(drNew["Fld_Taxable"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_Taxable"]));
                            igst = igst + (Convert.ToString(drNew["Fld_IGSTAmnt"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_IGSTAmnt"]));
                            cgst = cgst + (Convert.ToString(drNew["Fld_CGSTAmnt"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_CGSTAmnt"]));
                            sgst = sgst + (Convert.ToString(drNew["Fld_SGSTAmnt"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_SGSTAmnt"]));
                            cess = cess + (Convert.ToString(drNew["Fld_CessAmnt"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_CessAmnt"]));
                        }
                    }

                    dr["Fld_NoteValue"] = invval;
                    dr["Fld_Taxable"] = tax;
                    dr["Fld_IGSTAmnt"] = igst;
                    dr["Fld_CGSTAmnt"] = cgst;
                    dr["Fld_SGSTAmnt"] = sgst;
                    dr["Fld_CessAmnt"] = cess;
                    #endregion

                    //dr["Fld_POS"] = "Completed";
                    //dr["Fld_ReverseCharge"] = "False";
                    dr["Fld_FileStatus"] = "Total";
                    dt.Rows.Add(dr);

                    int _Result = objGSTR2A.GSTR2A_CNDBulkEntryJson(dt, strMonth, ReturnYear);
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
        public bool b2baEntry(string jsonData, string strMonth, string ReturnYear)
        {
            bool flg = false;
            try
            {
                DataTable dt = new DataTable();
                RootObject obj = JsonConvert.DeserializeObject<RootObject>(jsonData);

                //if (obj != null && obj.cdn != null && obj.cdn.Count > 0)
                //    obj.cdn = obj.cdn.Where(x => x.cfs == "Y").ToList();

                #region ASSIGN GRIDVIEW ROWS IN DATATABLE For CDN
                dt = new DataTable();
                dt.Columns.Add("Fld_Sequence");
                dt.Columns.Add("Fld_GSTIN");
                dt.Columns.Add("Fld_NameOfParty");
                dt.Columns.Add("Fld_OrgInvoiceNo");
                dt.Columns.Add("Fld_OrgInvoiceDate");
                dt.Columns.Add("Fld_InvoiceType");
                dt.Columns.Add("Fld_ResInvoiceNo");
                dt.Columns.Add("Fld_ResInvoiceDate");
                dt.Columns.Add("Fld_POS");
                dt.Columns.Add("Fld_SupAttResCharge");
                dt.Columns.Add("Fld_ApplicablePer");
                dt.Columns.Add("Fld_InvoiceValue");
                dt.Columns.Add("Fld_Rate");
                dt.Columns.Add("Fld_TaxableVal");
                dt.Columns.Add("Fld_IntTax");
                dt.Columns.Add("Fld_CtrlTax");
                dt.Columns.Add("Fld_StateTax");
                dt.Columns.Add("Fld_CessTax");
                dt.Columns.Add("Fld_Submitted");
                dt.Columns.Add("Fld_FileStatus");

                if (obj != null && obj.b2ba != null)
                {
                    for (int i = 0; i < obj.b2ba.Count; i++)
                    {
                        for (int j = 0; j < obj.b2ba[i].inv.Count; j++)
                        {
                            for (int k = 0; k < obj.b2ba[i].inv[j].itms.Count; k++)
                            {
                                dt.Rows.Add();
                                dt.Rows[dt.Rows.Count - 1]["Fld_GSTIN"] = Convert.ToString(obj.b2ba[i].ctin);
                                dt.Rows[dt.Rows.Count - 1]["Fld_NameOfParty"] = Convert.ToString(obj.b2ba[i].cname).Replace("'", "");
                                //dt.Rows[dt.Rows.Count - 1]["Fld_OrgInvoiceNo"] = Convert.ToString(obj.b2ba[i].inv[j].oinum);
                                //dt.Rows[dt.Rows.Count - 1]["Fld_OrgInvoiceDate"] = Convert.ToString(obj.b2ba[i].inv[j].oidt);
                                dt.Rows[dt.Rows.Count - 1]["Fld_OrgInvoiceNo"] = Convert.ToString(obj.b2ba[i].inv[j].inum);
                                dt.Rows[dt.Rows.Count - 1]["Fld_OrgInvoiceDate"] = Convert.ToString(obj.b2ba[i].inv[j].idt);

                                dt.Rows[dt.Rows.Count - 1]["Fld_InvoiceType"] = GetVal(Convert.ToString(obj.b2ba[i].inv[j].inv_typ));
                                dt.Rows[dt.Rows.Count - 1]["Fld_ResInvoiceNo"] = Convert.ToString(obj.b2ba[i].inv[j].inum);
                                dt.Rows[dt.Rows.Count - 1]["Fld_ResInvoiceDate"] = Convert.ToString(obj.b2ba[i].inv[j].idt);

                                if (Convert.ToString(obj.b2ba[i].inv[j].pos) != null)
                                    dt.Rows[dt.Rows.Count - 1]["Fld_POS"] = CommonHelper.GetStateName(Convert.ToString(obj.b2ba[i].inv[j].pos));

                                dt.Rows[dt.Rows.Count - 1]["Fld_SupAttResCharge"] = (Convert.ToString(obj.b2ba[i].inv[j].rchrg) == "Y" ? "Yes" : "No");
                                dt.Rows[dt.Rows.Count - 1]["Fld_ApplicablePer"] = Convert.ToString("-");

                                dt.Rows[dt.Rows.Count - 1]["Fld_InvoiceValue"] = Convert.ToString(obj.b2ba[i].inv[j].val);
                                dt.Rows[dt.Rows.Count - 1]["Fld_Rate"] = Convert.ToString(obj.b2ba[i].inv[j].itms[k].itm_det.rt);
                                dt.Rows[dt.Rows.Count - 1]["Fld_TaxableVal"] = Convert.ToString(obj.b2ba[i].inv[j].itms[k].itm_det.txval);
                                dt.Rows[dt.Rows.Count - 1]["Fld_IntTax"] = Convert.ToString(obj.b2ba[i].inv[j].itms[k].itm_det.iamt);
                                dt.Rows[dt.Rows.Count - 1]["Fld_CtrlTax"] = Convert.ToString(obj.b2ba[i].inv[j].itms[k].itm_det.camt);
                                dt.Rows[dt.Rows.Count - 1]["Fld_StateTax"] = Convert.ToString(obj.b2ba[i].inv[j].itms[k].itm_det.samt);
                                dt.Rows[dt.Rows.Count - 1]["Fld_CessTax"] = Convert.ToString(obj.b2ba[i].inv[j].itms[k].itm_det.csamt);

                                //dt.Rows[dt.Rows.Count - 1]["Fld_IntTax"] = Convert.ToString(obj.b2ba[i].inv[j].iamt);
                                //dt.Rows[dt.Rows.Count - 1]["Fld_CtrlTax"] = Convert.ToString(obj.b2ba[i].inv[j].camt);
                                //dt.Rows[dt.Rows.Count - 1]["Fld_StateTax"] = Convert.ToString(obj.b2ba[i].inv[j].samt);
                                //dt.Rows[dt.Rows.Count - 1]["Fld_CessTax"] = Convert.ToString(obj.b2ba[i].inv[j].csamt);
                                dt.Rows[dt.Rows.Count - 1]["Fld_Submitted"] = (Convert.ToString(obj.b2ba[i].cfs) == "Y" ? "Yes" : "No");
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
                    DataRow dr = dt.NewRow();

                    #region Org Invoice no
                    var result = (from row in dt.AsEnumerable()
                                  where row.Field<string>("Fld_OrgInvoiceNo") != "" && row.Field<string>("Fld_GSTIN") != ""
                                  group row by new { colGSTIN = row.Field<string>("Fld_GSTIN"), colInvNo = row.Field<string>("Fld_OrgInvoiceNo") } into grp
                                  select new
                                  {
                                      colGSTIN = grp.Key.colGSTIN,
                                      colInvNo = grp.Key.colInvNo,
                                  }).ToList();

                    if (result != null && result.Count > 0)
                        dr["Fld_OrgInvoiceNo"] = result.Count;
                    else
                        dr["Fld_OrgInvoiceNo"] = 0;
                    #endregion

                    #region Org Invoice no
                    var result1 = (from row in dt.AsEnumerable()
                                   where row.Field<string>("Fld_ResInvoiceNo") != "" && row.Field<string>("Fld_GSTIN") != ""
                                   group row by new { colGSTIN = row.Field<string>("Fld_GSTIN"), colInvNo = row.Field<string>("Fld_ResInvoiceNo") } into grp
                                   select new
                                   {
                                       colGSTIN = grp.Key.colGSTIN,
                                       colInvNo = grp.Key.colInvNo,
                                   }).ToList();

                    if (result1 != null && result1.Count > 0)
                        dr["Fld_ResInvoiceNo"] = result1.Count;
                    else
                        dr["Fld_ResInvoiceNo"] = 0;
                    #endregion

                    //dr["Fld_OrgInvoiceNo"] = dt.Rows.Cast<DataRow>().Where(x => Convert.ToString(x["Fld_OrgInvoiceNo"]).Trim() != "").GroupBy(x => x["Fld_OrgInvoiceNo"]).Select(x => x.First()).Distinct().Count();

                    //dr["Fld_ResInvoiceNo"] = dt.Rows.Cast<DataRow>().Where(x => Convert.ToString(x["Fld_ResInvoiceNo"]).Trim() != "").GroupBy(x => x["Fld_ResInvoiceNo"]).Select(x => x.First()).Distinct().Count();


                    dr["Fld_InvoiceValue"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_InvoiceValue"] != null).Sum(x => x["Fld_InvoiceValue"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_InvoiceValue"])).ToString();
                    dr["Fld_TaxableVal"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_TaxableVal"] != null).Sum(x => x["Fld_TaxableVal"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_TaxableVal"])).ToString();
                    dr["Fld_IntTax"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_IntTax"] != null).Sum(x => x["Fld_IntTax"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_IntTax"])).ToString();
                    dr["Fld_CtrlTax"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_CtrlTax"] != null).Sum(x => x["Fld_CtrlTax"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_CtrlTax"])).ToString();
                    dr["Fld_StateTax"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_StateTax"] != null).Sum(x => x["Fld_StateTax"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_StateTax"])).ToString();
                    dr["Fld_CessTax"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_CessTax"] != null).Sum(x => x["Fld_CessTax"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_CessTax"])).ToString();

                    dr["Fld_POS"] = "Completed";
                    dr["Fld_SupAttResCharge"] = "False";
                    dr["Fld_FileStatus"] = "Total";

                    dt.Rows.Add(dr);

                    int _Result = objGSTR2A.GSTR2A_B2baBulkEntryJson(dt, strMonth, ReturnYear);
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
        public bool cdnaEntry(string jsonData, string strMonth, string ReturnYear)
        {
            bool flg = false;
            try
            {
                DataTable dt = new DataTable();
                RootObject obj = JsonConvert.DeserializeObject<RootObject>(jsonData);

                #region ASSIGN GRIDVIEW ROWS IN DATATABLE For CDN
                dt = new DataTable();
                dt.Columns.Add("Fld_Sequence");
                dt.Columns.Add("Fld_GSTIN");
                dt.Columns.Add("Fld_PartyName");
                dt.Columns.Add("Fld_TypeOfNote");
                dt.Columns.Add("Fld_DbtCrdtNoteNo");
                dt.Columns.Add("Fld_DbtCrdtNoteDate");
                dt.Columns.Add("Fld_OrgInvoiceNo");
                dt.Columns.Add("Fld_OrginvoiceDate");
                dt.Columns.Add("Fld_InvoiceNo");
                dt.Columns.Add("Fld_InvoiceDate");
                dt.Columns.Add("Fld_Regime");
                dt.Columns.Add("Fld_DiffPer");
                dt.Columns.Add("Fld_NoteValue");
                dt.Columns.Add("Fld_Rate");
                dt.Columns.Add("Fld_Taxable");
                dt.Columns.Add("Fld_IGSTAmnt");
                dt.Columns.Add("Fld_CGSTAmnt");
                dt.Columns.Add("Fld_SGSTAmnt");
                dt.Columns.Add("Fld_CessAmnt");
                dt.Columns.Add("Fld_Submitted");
                dt.Columns.Add("Fld_FileStatus");

                if (obj != null && obj.cdna != null)
                {
                    for (int i = 0; i < obj.cdna.Count; i++)
                    {
                        for (int j = 0; j < obj.cdna[i].nt.Count; j++)
                        {
                            for (int k = 0; k < obj.cdna[i].nt[j].itms.Count; k++)
                            {
                                dt.Rows.Add();

                                #region root element
                                dt.Rows[dt.Rows.Count - 1]["Fld_GSTIN"] = Convert.ToString(obj.cdna[i].ctin);
                                dt.Rows[dt.Rows.Count - 1]["Fld_Submitted"] = (Convert.ToString(obj.cdna[i].cfs) == "Y" ? "Yes" : "No");
                                #endregion

                                #region invoice details
                                if (Convert.ToString(obj.cdna[i].nt[j].ntty) == "C")
                                    dt.Rows[dt.Rows.Count - 1]["Fld_TypeOfNote"] = "Credit Note";
                                else if (Convert.ToString(obj.cdna[i].nt[j].ntty) == "D")
                                    dt.Rows[dt.Rows.Count - 1]["Fld_TypeOfNote"] = "Debit Note";
                                else if (Convert.ToString(obj.cdna[i].nt[j].ntty) == "R")
                                    dt.Rows[dt.Rows.Count - 1]["Fld_TypeOfNote"] = "Refund Voucher";

                                dt.Rows[dt.Rows.Count - 1]["Fld_DbtCrdtNoteNo"] = Convert.ToString(obj.cdna[i].nt[j].nt_num);
                                dt.Rows[dt.Rows.Count - 1]["Fld_DbtCrdtNoteDate"] = Convert.ToString(obj.cdna[i].nt[j].nt_dt);
                                dt.Rows[dt.Rows.Count - 1]["Fld_OrgInvoiceNo"] = Convert.ToString(obj.cdna[i].nt[j].ont_num);
                                dt.Rows[dt.Rows.Count - 1]["Fld_OrgInvoiceDate"] = Convert.ToString(obj.cdna[i].nt[j].ont_dt);
                                dt.Rows[dt.Rows.Count - 1]["Fld_InvoiceNo"] = Convert.ToString(obj.cdna[i].nt[j].inum);
                                dt.Rows[dt.Rows.Count - 1]["Fld_InvoiceDate"] = Convert.ToString(obj.cdna[i].nt[j].idt);
                                dt.Rows[dt.Rows.Count - 1]["Fld_Regime"] = (Convert.ToString(obj.cdna[i].nt[j].p_gst) == "Y" ? "Yes" : "No");
                                dt.Rows[dt.Rows.Count - 1]["Fld_DiffPer"] = Convert.ToString(obj.cdna[i].nt[j].diff_percent);
                                dt.Rows[dt.Rows.Count - 1]["Fld_NoteValue"] = Convert.ToString(obj.cdna[i].nt[j].val);
                                #endregion

                                #region item details
                                dt.Rows[dt.Rows.Count - 1]["Fld_Rate"] = Convert.ToString(obj.cdna[i].nt[j].itms[k].itm_det.rt);
                                dt.Rows[dt.Rows.Count - 1]["Fld_Taxable"] = Convert.ToString(obj.cdna[i].nt[j].itms[k].itm_det.txval);
                                dt.Rows[dt.Rows.Count - 1]["Fld_IGSTAmnt"] = Convert.ToString(obj.cdna[i].nt[j].itms[k].itm_det.iamt);
                                dt.Rows[dt.Rows.Count - 1]["Fld_CGSTAmnt"] = Convert.ToString(obj.cdna[i].nt[j].itms[k].itm_det.camt);
                                dt.Rows[dt.Rows.Count - 1]["Fld_SGSTAmnt"] = Convert.ToString(obj.cdna[i].nt[j].itms[k].itm_det.samt);
                                dt.Rows[dt.Rows.Count - 1]["Fld_CessAmnt"] = Convert.ToString(obj.cdna[i].nt[j].itms[k].itm_det.csamt);
                                #endregion
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
                    DataRow dr = dt.NewRow();
                    //dr["Fld_DbtCrdtNoteNo"] = dt.Rows.Cast<DataRow>().Where(x => Convert.ToString(x["Fld_DbtCrdtNoteNo"]).Trim() != "").GroupBy(x => x["Fld_DbtCrdtNoteNo"]).Select(x => x.First()).Distinct().Count();

                    #region CDNA no
                    var result = (from row in dt.AsEnumerable()
                                  where row.Field<string>("Fld_DbtCrdtNoteNo") != "" && row.Field<string>("Fld_GSTIN") != ""
                                  group row by new { colGSTIN = row.Field<string>("Fld_GSTIN"), colInvNo = row.Field<string>("Fld_DbtCrdtNoteNo") } into grp
                                  select new
                                  {
                                      colGSTIN = grp.Key.colGSTIN,
                                      colInvNo = grp.Key.colInvNo,
                                  }).ToList();

                    if (result != null && result.Count > 0)
                        dr["Fld_DbtCrdtNoteNo"] = result.Count;
                    else
                        dr["Fld_DbtCrdtNoteNo"] = 0;

                    var result1 = (from row in dt.AsEnumerable()
                                   where row.Field<string>("Fld_OrgInvoiceNo") != "" && row.Field<string>("Fld_GSTIN") != ""
                                   group row by new { colGSTIN = row.Field<string>("Fld_GSTIN"), colInvNo = row.Field<string>("Fld_OrgInvoiceNo") } into grp
                                   select new
                                   {
                                       colGSTIN = grp.Key.colGSTIN,
                                       colInvNo = grp.Key.colInvNo,
                                   }).ToList();

                    if (result1 != null && result1.Count > 0)
                        dr["Fld_OrgInvoiceNo"] = result1.Count;
                    else
                        dr["Fld_OrgInvoiceNo"] = 0;

                    var result2 = (from row in dt.AsEnumerable()
                                   where row.Field<string>("Fld_InvoiceNo") != "" && row.Field<string>("Fld_GSTIN") != ""
                                   group row by new { colGSTIN = row.Field<string>("Fld_GSTIN"), colInvNo = row.Field<string>("Fld_InvoiceNo") } into grp
                                   select new
                                   {
                                       colGSTIN = grp.Key.colGSTIN,
                                       colInvNo = grp.Key.colInvNo,
                                   }).ToList();

                    if (result2 != null && result2.Count > 0)
                        dr["Fld_InvoiceNo"] = result2.Count;
                    else
                        dr["Fld_InvoiceNo"] = 0;
                    #endregion

                    #region New
                    DataTable dtNew = new DataTable();
                    dtNew = dt.AsEnumerable()
                        .GroupBy(row => new
                        {
                            Member1 = row.Field<string>("Fld_TypeOfNote")
                        })
                        .Select(g =>
                        {
                            var row = dt.NewRow();
                            row["Fld_TypeOfNote"] = g.Key.Member1;
                            row["Fld_NoteValue"] = g.Sum(r => r["Fld_NoteValue"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(r["Fld_NoteValue"]));
                            row["Fld_Taxable"] = g.Sum(r => r["Fld_Taxable"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(r["Fld_Taxable"]));
                            row["Fld_IGSTAmnt"] = g.Sum(r => r["Fld_IGSTAmnt"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(r["Fld_IGSTAmnt"]));
                            row["Fld_CGSTAmnt"] = g.Sum(r => r["Fld_CGSTAmnt"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(r["Fld_CGSTAmnt"]));
                            row["Fld_SGSTAmnt"] = g.Sum(r => r["Fld_SGSTAmnt"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(r["Fld_SGSTAmnt"]));
                            row["Fld_CessAmnt"] = g.Sum(r => r["Fld_CessAmnt"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(r["Fld_CessAmnt"]));
                            return row;
                        }).CopyToDataTable();

                    decimal invval = 0, tax = 0, igst = 0, cgst = 0, sgst = 0, cess = 0;
                    foreach (DataRow drNew in dtNew.Rows)
                    {
                        if (Convert.ToString(drNew["Fld_TypeOfNote"]) == "Credit Note")
                        {
                            invval = invval - (Convert.ToString(drNew["Fld_NoteValue"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_NoteValue"]));
                            tax = tax - (Convert.ToString(drNew["Fld_Taxable"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_Taxable"]));
                            igst = igst - (Convert.ToString(drNew["Fld_IGSTAmnt"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_IGSTAmnt"]));
                            cgst = cgst - (Convert.ToString(drNew["Fld_CGSTAmnt"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_CGSTAmnt"]));
                            sgst = sgst - (Convert.ToString(drNew["Fld_SGSTAmnt"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_SGSTAmnt"]));
                            cess = cess - (Convert.ToString(drNew["Fld_CessAmnt"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_CessAmnt"]));
                        }
                        else if (Convert.ToString(drNew["Fld_TypeOfNote"]) == "Refund Voucher")
                        {
                            invval = invval - (Convert.ToString(drNew["Fld_NoteValue"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_NoteValue"]));
                            tax = tax - (Convert.ToString(drNew["Fld_Taxable"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_Taxable"]));
                            igst = igst - (Convert.ToString(drNew["Fld_IGSTAmnt"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_IGSTAmnt"]));
                            cgst = cgst - (Convert.ToString(drNew["Fld_CGSTAmnt"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_CGSTAmnt"]));
                            sgst = sgst - (Convert.ToString(drNew["Fld_SGSTAmnt"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_SGSTAmnt"]));
                            cess = cess - (Convert.ToString(drNew["Fld_CessAmnt"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_CessAmnt"]));
                        }
                        else
                        {
                            invval = invval + (Convert.ToString(drNew["Fld_NoteValue"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_NoteValue"]));
                            tax = tax + (Convert.ToString(drNew["Fld_Taxable"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_Taxable"]));
                            igst = igst + (Convert.ToString(drNew["Fld_IGSTAmnt"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_IGSTAmnt"]));
                            cgst = cgst + (Convert.ToString(drNew["Fld_CGSTAmnt"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_CGSTAmnt"]));
                            sgst = sgst + (Convert.ToString(drNew["Fld_SGSTAmnt"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_SGSTAmnt"]));
                            cess = cess + (Convert.ToString(drNew["Fld_CessAmnt"]) == "" ? 0 : Convert.ToDecimal(drNew["Fld_CessAmnt"]));
                        }
                    }

                    dr["Fld_NoteValue"] = invval;
                    dr["Fld_Taxable"] = tax;
                    dr["Fld_IGSTAmnt"] = igst;
                    dr["Fld_CGSTAmnt"] = cgst;
                    dr["Fld_SGSTAmnt"] = sgst;
                    dr["Fld_CessAmnt"] = cess;
                    #endregion

                    //dr["Fld_POS"] = "Completed";
                    //dr["Fld_ReverseCharge"] = "False";
                    dr["Fld_FileStatus"] = "Total";
                    dt.Rows.Add(dr);

                    int _Result = objGSTR2A.GSTR2A_CNDABulkEntryJson(dt, strMonth, ReturnYear);
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
        public string GetVal(string val)
        {
            string retVal = "";
            if (val == "R")
                retVal = "Regular";
            else if (val == "DE")
                retVal = "Deemed Exports";
            else if (val == "SEWP")
                retVal = "SEZ Exports with payment";
            else if (val == "SEWOP")
                retVal = "SEZ exports without payment";

            return retVal;
        }

        #endregion

        #region GST Methods

        protected response2A getProcessedInvoiceB2B()
        {
            bool flag;
            response2A objRes = new response2A();

            try
            {
                this.Cc = clsPro.Cooki != null ? ((clsPro.Cooki.FirstOrDefault(x => x.ckname == "loginCookies_" + CommonHelper.CompanyID) != null ? clsPro.Cooki.FirstOrDefault(x => x.ckname == "loginCookies_" + CommonHelper.CompanyID).CC1 : null)) : null;

                string[] strArrays = clssummary.ReturnDate();
                string retDate = string.Concat(strArrays[1], strArrays[0]);
                string str = strArrays[0];

                string assesseeDetails = clssummary.GetAssesseeDetails()[0];
                List<string> processedInvoiceUrl = GetUrlForRequest.GetProcessedInvoiceUrl2a(strArrays[1], str, "B2B");
                HttpWebRequest httpWebRequest = this.PrepareGetRequest(new Uri(processedInvoiceUrl[0]), processedInvoiceUrl[1]);

                response = (HttpWebResponse)httpWebRequest.GetResponse();
                MemoryStream memoryStream = new MemoryStream();
                Stream responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    responseStream.CopyTo(memoryStream);
                }
                string str1 = Encoding.UTF8.GetString(memoryStream.ToArray());

                #region NEW CODE
                objRes.msg = this.ErrorCheck(str1);
                if (objRes.msg != "Success")
                {
                    objRes.flg = false;
                }
                else if (JsonConvert.DeserializeObject<clsResponse>(str1).message != "No Invoices found for the provided Inputs")
                {
                    StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
                    RootObjectCTIN b2bmodelCTIN = JsonConvert.DeserializeObject<RootObjectCTIN>(str1);

                    if (b2bmodelCTIN != null)
                    {
                        if (b2bmodelCTIN.cpty != null)
                        {
                            List<B2b> objb2b = new List<B2b>();
                            foreach (var row in b2bmodelCTIN.cpty)
                            {
                                if (row.rc <= 499)
                                {
                                    strArrays = clssummary.ReturnDate();
                                    str = strArrays[0];
                                    HttpWebResponse httpWebResponse = null;
                                    processedInvoiceUrl = GetUrlForRequest.GetProcessedInvoiceUrl2aInvoice(strArrays[1], str, row.stin, "B2B");
                                    httpWebRequest = this.PrepareGetRequest2(new Uri(processedInvoiceUrl[0]).ToString(), processedInvoiceUrl[1]);
                                    httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                                    MemoryStream memoryStream1 = new MemoryStream();
                                    Stream stream = httpWebResponse.GetResponseStream();
                                    if (stream != null)
                                    {
                                        stream.CopyTo(memoryStream1);
                                    }
                                    str = Encoding.UTF8.GetString(memoryStream1.ToArray());

                                    //objRes.strJson = str;
                                    //objRes.flg = true;

                                    RootObject b2bmodel = JsonConvert.DeserializeObject<RootObject>(str);
                                    if (b2bmodel.b2b != null && b2bmodel.b2b.Count > 0)
                                    {
                                        objb2b.Add(b2bmodel.b2b.FirstOrDefault());

                                        objb2b[objb2b.Count - 1].cname = row.cname.Trim();
                                    }
                                }
                            }

                            string json = JsonConvert.SerializeObject(objb2b);

                            objRes.strJson = json;
                            objRes.flg = true;
                        }
                    }
                }
                else
                {
                    objRes.msg = "No Invoices found for the provided Inputs";
                    objRes.flg = false;
                }
                #endregion

                #region OLD CODE
                /*
                objRes.msg = this.ErrorCheck(str1);
                if (objRes.msg != "Success")
                {
                    objRes.flg = false;
                }
                else if (JsonConvert.DeserializeObject<clsResponse>(str1).message != "No Invoices found for the provided Inputs")
                {
                    this.Cc = clsPro.Cooki != null ? ((clsPro.Cooki.FirstOrDefault(x => x.ckname == "loginCookies_" + CommonHelper.CompanyID) != null ? clsPro.Cooki.FirstOrDefault(x => x.ckname == "loginCookies_" + CommonHelper.CompanyID).CC1 : null)) : null;
                    HttpWebRequest httpWebRequest1 = this.PrepareGetRequest2(string.Concat("https://return.gst.gov.in/returns/auth/api/gstr2a/b2b?rtn_prd=", retDate) ?? "", "https://return.gst.gov.in/returns/auth/gstr2/preview/b2bcountersupplier");
                    HttpWebResponse httpWebResponse = null;
                    httpWebResponse = (HttpWebResponse)httpWebRequest1.GetResponse();
                    MemoryStream memoryStream1 = new MemoryStream();
                    Stream stream = httpWebResponse.GetResponseStream();
                    if (stream != null)
                    {
                        stream.CopyTo(memoryStream1);
                    }
                    str = Encoding.UTF8.GetString(memoryStream1.ToArray());
                    objRes.strJson = str;
                    objRes.flg = true;
                }
                else
                {
                    objRes.msg = "No Invoices found for the provided Inputs";
                    objRes.flg = false;
                }
                */
                #endregion
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
                    }
                    else
                    {
                        getProcessedInvoiceB2B();
                    }
                }
                else
                {
                    objRes.msg = string.Concat("Some error occured please check ur Connection/Data and try again", exception1.Message);
                    flag = false;
                }
            }
            return objRes;
        }
        protected response2A getProcessedInvoiceCDN()
        {
            bool flag;
            response2A objRes = new response2A();

            try
            {

                this.Cc = clsPro.Cooki != null ? ((clsPro.Cooki.FirstOrDefault(x => x.ckname == "loginCookies_" + CommonHelper.CompanyID) != null ? clsPro.Cooki.FirstOrDefault(x => x.ckname == "loginCookies_" + CommonHelper.CompanyID).CC1 : null)) : null; //(CookieContainer)HttpContext.Current.Session["loginCookies_0"];

                string[] strArrays = clssummary.ReturnDate();
                string retDate = string.Concat(strArrays[1], strArrays[0]);
                string str = strArrays[0];

                string assesseeDetails = clssummary.GetAssesseeDetails()[0];
                List<string> processedInvoiceUrl = GetUrlForRequest.GetProcessedInvoiceUrl2a(strArrays[1], str, "CDN");
                HttpWebRequest httpWebRequest = this.PrepareGetRequest2(string.Concat("https://return.gst.gov.in/returns/auth/api/gstr2a/ctin?rtn_prd=", retDate, "&section_name=CDN"), "https://return.gst.gov.in/returns/auth/gstr2/preview/cdncounterpreview");

                response = (HttpWebResponse)httpWebRequest.GetResponse();
                MemoryStream memoryStream = new MemoryStream();
                Stream responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    responseStream.CopyTo(memoryStream);
                }
                string str1 = Encoding.UTF8.GetString(memoryStream.ToArray());


                #region NEW CODE
                objRes.msg = this.ErrorCheck(str1);
                if (objRes.msg != "Success")
                {
                    objRes.flg = false;
                }
                else if (JsonConvert.DeserializeObject<clsResponse>(str1).message != "No Invoices found for the provided Inputs")
                {
                    StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
                    RootObjectCTIN b2bmodelCTIN = JsonConvert.DeserializeObject<RootObjectCTIN>(str1);

                    if (b2bmodelCTIN != null)
                    {
                        if (b2bmodelCTIN.cpty != null)
                        {
                            List<Cdn> objcdn = new List<Cdn>();
                            foreach (var row in b2bmodelCTIN.cpty)
                            {
                                if (row.rc <= 499)
                                {
                                    strArrays = clssummary.ReturnDate();
                                    str = strArrays[0];
                                    HttpWebResponse httpWebResponse = null;
                                    processedInvoiceUrl = GetUrlForRequest.GetProcessedInvoiceUrl2aInvoice(strArrays[1], str, row.stin, "CDN");
                                    httpWebRequest = this.PrepareGetRequest2(new Uri(processedInvoiceUrl[0]).ToString(), processedInvoiceUrl[1]);
                                    httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                                    MemoryStream memoryStream1 = new MemoryStream();
                                    Stream stream = httpWebResponse.GetResponseStream();
                                    if (stream != null)
                                    {
                                        stream.CopyTo(memoryStream1);
                                    }
                                    str = Encoding.UTF8.GetString(memoryStream1.ToArray());

                                    //objRes.strJson = str;
                                    //objRes.flg = true;

                                    RootObject b2bmodel = JsonConvert.DeserializeObject<RootObject>(str);
                                    if (b2bmodel.cdn != null && b2bmodel.cdn.Count > 0)
                                    {
                                        objcdn.Add(b2bmodel.cdn.FirstOrDefault());

                                        objcdn[objcdn.Count - 1].cname = row.cname.Trim();
                                    }
                                }
                            }

                            string json = JsonConvert.SerializeObject(objcdn);

                            objRes.strJson = json;
                            objRes.flg = true;
                        }
                    }
                }
                else
                {
                    objRes.msg = "No Invoices found for the provided Inputs";
                    objRes.flg = false;
                }
                #endregion

                #region OLD CODE
                /*
                objRes.msg = this.ErrorCheck(str1);
                if (objRes.msg != "Success")
                {
                    objRes.flg = false;
                }
                else if (JsonConvert.DeserializeObject<clsResponse>(str1).message != "No Invoices found for the provided Inputs")
                {
                    this.Cc = clsPro.Cooki != null ? ((clsPro.Cooki.FirstOrDefault(x => x.ckname == "loginCookies_" + CommonHelper.CompanyID) != null ? clsPro.Cooki.FirstOrDefault(x => x.ckname == "loginCookies_" + CommonHelper.CompanyID).CC1 : null)) : null;
                    HttpWebRequest httpWebRequest1 = this.PrepareGetRequest2(string.Concat("https://return.gst.gov.in/returns/auth/api/gstr2a/cdn?rtn_prd=", retDate) ?? "", "https://return.gst.gov.in/returns/auth/gstr2/preview/cdncountersupplier");
                    HttpWebResponse httpWebResponse = null;
                    httpWebResponse = (HttpWebResponse)httpWebRequest1.GetResponse();
                    MemoryStream memoryStream1 = new MemoryStream();
                    Stream stream = httpWebResponse.GetResponseStream();
                    if (stream != null)
                    {
                        stream.CopyTo(memoryStream1);
                    }
                    str = Encoding.UTF8.GetString(memoryStream1.ToArray());
                    objRes.strJson = str;
                    objRes.flg = true;
                }
                else
                {
                    objRes.msg = "No Invoices found for the provided Inputs";
                    objRes.flg = false;
                }
                */
                #endregion
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
                    }
                    else
                    {
                        getProcessedInvoiceCDN();
                    }
                }
                else
                {
                    objRes.msg = string.Concat("Some error occured please check ur Connection/Data and try again", exception1.Message);
                    flag = false;
                }
            }
            return objRes;
        }
        protected response2A getProcessedInvoiceB2BA()
        {
            bool flag;
            response2A objRes = new response2A();

            try
            {
                this.Cc = clsPro.Cooki != null ? ((clsPro.Cooki.FirstOrDefault(x => x.ckname == "loginCookies_" + CommonHelper.CompanyID) != null ? clsPro.Cooki.FirstOrDefault(x => x.ckname == "loginCookies_" + CommonHelper.CompanyID).CC1 : null)) : null;

                string[] strArrays = clssummary.ReturnDate();
                string retDate = string.Concat(strArrays[1], strArrays[0]);
                string str = strArrays[0];

                string assesseeDetails = clssummary.GetAssesseeDetails()[0];
                List<string> processedInvoiceUrl = GetUrlForRequest.GetProcessedInvoiceUrl2a(strArrays[1], str, "B2BA");
                HttpWebRequest httpWebRequest = this.PrepareGetRequest(new Uri(processedInvoiceUrl[0]), processedInvoiceUrl[1]);

                response = (HttpWebResponse)httpWebRequest.GetResponse();
                MemoryStream memoryStream = new MemoryStream();
                Stream responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    responseStream.CopyTo(memoryStream);
                }
                string str1 = Encoding.UTF8.GetString(memoryStream.ToArray());

                #region NEW CODE
                objRes.msg = this.ErrorCheck(str1);
                if (objRes.msg != "Success")
                {
                    objRes.flg = false;
                }
                else if (JsonConvert.DeserializeObject<clsResponse>(str1).message != "No Invoices found for the provided Inputs")
                {
                    StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
                    RootObjectCTIN b2bmodelCTIN = JsonConvert.DeserializeObject<RootObjectCTIN>(str1);

                    if (b2bmodelCTIN != null)
                    {
                        if (b2bmodelCTIN.cpty != null)
                        {
                            List<B2ba> objb2ba = new List<B2ba>();
                            foreach (var row in b2bmodelCTIN.cpty)
                            {
                                if (row.rc <= 499)
                                {
                                    strArrays = clssummary.ReturnDate();
                                    str = strArrays[0];
                                    HttpWebResponse httpWebResponse = null;
                                    processedInvoiceUrl = GetUrlForRequest.GetProcessedInvoiceUrl2aInvoice(strArrays[1], str, row.stin, "B2BA");
                                    httpWebRequest = this.PrepareGetRequest2(new Uri(processedInvoiceUrl[0]).ToString(), processedInvoiceUrl[1]);
                                    httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                                    MemoryStream memoryStream1 = new MemoryStream();
                                    Stream stream = httpWebResponse.GetResponseStream();
                                    if (stream != null)
                                    {
                                        stream.CopyTo(memoryStream1);
                                    }
                                    str = Encoding.UTF8.GetString(memoryStream1.ToArray());

                                    //objRes.strJson = str;
                                    //objRes.flg = true;

                                    RootObject b2bmodel = JsonConvert.DeserializeObject<RootObject>(str);
                                    if (b2bmodel.b2ba != null && b2bmodel.b2ba.Count > 0)
                                    {
                                        objb2ba.Add(b2bmodel.b2ba.FirstOrDefault());
                                        objb2ba[objb2ba.Count - 1].cname = row.cname.Trim();
                                    }
                                }
                            }

                            string json = JsonConvert.SerializeObject(objb2ba);

                            objRes.strJson = json;
                            objRes.flg = true;
                        }
                    }
                }
                else
                {
                    objRes.msg = "No Invoices found for the provided Inputs";
                    objRes.flg = false;
                }
                #endregion

                #region Old Code
                /*
                objRes.msg = this.ErrorCheck(str1);
                if (objRes.msg != "Success")
                {
                    objRes.flg = false;
                }
                else if (JsonConvert.DeserializeObject<clsResponse>(str1).message != "No Invoices found for the provided Inputs")
                {
                    this.Cc = clsPro.Cooki != null ? ((clsPro.Cooki.FirstOrDefault(x => x.ckname == "loginCookies_" + CommonHelper.CompanyID) != null ? clsPro.Cooki.FirstOrDefault(x => x.ckname == "loginCookies_" + CommonHelper.CompanyID).CC1 : null)) : null;
                    HttpWebRequest httpWebRequest1 = this.PrepareGetRequest2(string.Concat("https://return.gst.gov.in/returns/auth/api/gstr2a/b2ba?rtn_prd=", retDate) ?? "", "https://return.gst.gov.in/returns/auth/gstr2/preview/b2bacounterpreview");
                    HttpWebResponse httpWebResponse = null;
                    httpWebResponse = (HttpWebResponse)httpWebRequest1.GetResponse();
                    MemoryStream memoryStream1 = new MemoryStream();
                    Stream stream = httpWebResponse.GetResponseStream();
                    if (stream != null)
                    {
                        stream.CopyTo(memoryStream1);
                    }
                    str = Encoding.UTF8.GetString(memoryStream1.ToArray());
                    objRes.strJson = str;
                    objRes.flg = true;
                }
                else
                {
                    objRes.msg = "No Invoices found for the provided Inputs";
                    objRes.flg = false;
                }
                */
                #endregion
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
                    }
                    else
                    {
                        getProcessedInvoiceB2BA();
                    }
                }
                else
                {
                    objRes.msg = string.Concat("Some error occured please check ur Connection/Data and try again", exception1.Message);
                    flag = false;
                }
            }
            return objRes;
        }
        protected response2A getProcessedInvoiceCDNA()
        {
            bool flag;
            response2A objRes = new response2A();

            try
            {

                this.Cc = clsPro.Cooki != null ? ((clsPro.Cooki.FirstOrDefault(x => x.ckname == "loginCookies_" + CommonHelper.CompanyID) != null ? clsPro.Cooki.FirstOrDefault(x => x.ckname == "loginCookies_" + CommonHelper.CompanyID).CC1 : null)) : null; //(CookieContainer)HttpContext.Current.Session["loginCookies_0"];

                string[] strArrays = clssummary.ReturnDate();
                string retDate = string.Concat(strArrays[1], strArrays[0]);
                string str = strArrays[0];

                string assesseeDetails = clssummary.GetAssesseeDetails()[0];
                List<string> processedInvoiceUrl = GetUrlForRequest.GetProcessedInvoiceUrl2a(strArrays[1], str, "CDNA");
                HttpWebRequest httpWebRequest = this.PrepareGetRequest2(string.Concat("https://return.gst.gov.in/returns/auth/api/gstr2a/ctin?rtn_prd=", retDate, "&section_name=CDNA"), "https://return.gst.gov.in/returns/auth/gstr2/preview/cdnacounterpreview");

                response = (HttpWebResponse)httpWebRequest.GetResponse();
                MemoryStream memoryStream = new MemoryStream();
                Stream responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    responseStream.CopyTo(memoryStream);
                }
                string str1 = Encoding.UTF8.GetString(memoryStream.ToArray());


                #region NEW CODE
                objRes.msg = this.ErrorCheck(str1);
                if (objRes.msg != "Success")
                {
                    objRes.flg = false;
                }
                else if (JsonConvert.DeserializeObject<clsResponse>(str1).message != "No Invoices found for the provided Inputs")
                {
                    StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
                    RootObjectCTIN b2bmodelCTIN = JsonConvert.DeserializeObject<RootObjectCTIN>(str1);

                    if (b2bmodelCTIN != null)
                    {
                        if (b2bmodelCTIN.cpty != null)
                        {
                            List<Cdna> objcdna = new List<Cdna>();
                            foreach (var row in b2bmodelCTIN.cpty)
                            {
                                if (row.rc <= 499)
                                {
                                    strArrays = clssummary.ReturnDate();
                                    str = strArrays[0];
                                    HttpWebResponse httpWebResponse = null;
                                    processedInvoiceUrl = GetUrlForRequest.GetProcessedInvoiceUrl2aInvoice(strArrays[1], str, row.stin, "CDNA");
                                    httpWebRequest = this.PrepareGetRequest2(new Uri(processedInvoiceUrl[0]).ToString(), processedInvoiceUrl[1]);
                                    httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                                    MemoryStream memoryStream1 = new MemoryStream();
                                    Stream stream = httpWebResponse.GetResponseStream();
                                    if (stream != null)
                                    {
                                        stream.CopyTo(memoryStream1);
                                    }
                                    str = Encoding.UTF8.GetString(memoryStream1.ToArray());

                                    RootObject b2bmodel = JsonConvert.DeserializeObject<RootObject>(str);
                                    if (b2bmodel.cdna != null && b2bmodel.cdn.Count > 0)
                                    {
                                        objcdna.Add(b2bmodel.cdna.FirstOrDefault());
                                        objcdna[objcdna.Count - 1].cname = row.cname.Trim();
                                    }
                                }
                            }

                            string json = JsonConvert.SerializeObject(objcdna);

                            objRes.strJson = json;
                            objRes.flg = true;
                        }
                    }
                }
                else
                {
                    objRes.msg = "No Invoices found for the provided Inputs";
                    objRes.flg = false;
                }
                #endregion

                #region Old Code
                /*
                objRes.msg = this.ErrorCheck(str1);
                if (objRes.msg != "Success")
                {
                    objRes.flg = false;
                }
                else if (JsonConvert.DeserializeObject<clsResponse>(str1).message != "No Invoices found for the provided Inputs")
                {
                    this.Cc = clsPro.Cooki != null ? ((clsPro.Cooki.FirstOrDefault(x => x.ckname == "loginCookies_" + CommonHelper.CompanyID) != null ? clsPro.Cooki.FirstOrDefault(x => x.ckname == "loginCookies_" + CommonHelper.CompanyID).CC1 : null)) : null;
                    HttpWebRequest httpWebRequest1 = this.PrepareGetRequest2(string.Concat("https://return.gst.gov.in/returns/auth/api/gstr2a/cdna?rtn_prd=", retDate) ?? "", "https://return.gst.gov.in/returns/auth/gstr2/preview/cdnacountersupplier");
                    HttpWebResponse httpWebResponse = null;
                    httpWebResponse = (HttpWebResponse)httpWebRequest1.GetResponse();
                    MemoryStream memoryStream1 = new MemoryStream();
                    Stream stream = httpWebResponse.GetResponseStream();
                    if (stream != null)
                    {
                        stream.CopyTo(memoryStream1);
                    }
                    str = Encoding.UTF8.GetString(memoryStream1.ToArray());
                    objRes.strJson = str;
                    objRes.flg = true;
                }
                else
                {
                    objRes.msg = "No Invoices found for the provided Inputs";
                    objRes.flg = false;
                }
                */
                #endregion
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
                    }
                    else
                    {
                        getProcessedInvoiceCDNA();
                    }
                }
                else
                {
                    objRes.msg = string.Concat("Some error occured please check ur Connection/Data and try again", exception1.Message);
                    flag = false;
                }
            }
            return objRes;
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
                // this.getError = "Error in requesting to server";
                httpWebRequest = null;
            }
            return httpWebRequest;
        }
        protected HttpWebRequest PrepareGetRequest2(string uri, string referer)
        {
            HttpWebRequest httpWebRequest;
            try
            {
                HttpWebRequest item = (HttpWebRequest)WebRequest.Create(uri);
                item.CookieContainer = this.Cc;
                item.KeepAlive = true;
                item.Method = "GET";
                item.Host = "return.gst.gov.in";
                if (referer != null)
                {
                    item.Referer = referer;
                }
                else if (referer == null)
                {
                    item.Headers.Add("Upgrade-Insecure-Requests", "1");
                }
                item.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*;q=0.8";
                item.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
                item.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                httpWebRequest = item;
            }
            catch (Exception exception)
            {
                httpWebRequest = null;
            }
            return httpWebRequest;
        }
        public string ErrorCheck(string reply)
        {
            string str;
            if (reply.Contains("Session Expired"))
            {
                str = "Session Expired";
            }
            else if (!reply.Contains("No Invoices found for the provided Inputs"))
            {
                str = (!reply.Contains("Your session is expired or you don't have permission to access the requested page.") ? "Success" : "Your session is expired or you don't have permission to access the requested page.");
            }
            else
            {
                str = "No Invoices found for the provided Inputs";
            }
            return str;
        }

        #endregion


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

                    Query = "Delete from SPQR2AInwardSupplies where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                    _Result = objGSTR2A.IUDData(Query);
                    if (_Result != 1)
                    { _str += "Data delete error - B2b..!\n"; }

                    Query = "Delete from SPQR2ACND where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                    _Result = objGSTR2A.IUDData(Query);
                    if (_Result != 1)
                    { _str += "Data delete error - Cdn..!\n"; }

                    Query = "Delete from SPQR2AB2BAmend where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                    _Result = objGSTR2A.IUDData(Query);
                    if (_Result != 1)
                    { _str += "Data delete error - B2ba..!\n"; }

                    Query = "Delete from SPQR2ACNDAmend where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                    _Result = objGSTR2A.IUDData(Query);
                    if (_Result != 1)
                    { _str += "Data delete error - Cdna..!\n"; }
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

        private void msImpExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = string.Empty; string fileExt = string.Empty;
                OpenFileDialog file = new OpenFileDialog();
                if (file.ShowDialog() == DialogResult.OK)
                {
                    // Check if you really have a file name 
                    if (file.FileName.Trim() != string.Empty)
                    {
                        using (StreamReader r = new StreamReader(file.FileName))
                        {
                            string _json = r.ReadToEnd();

                            pbGSTR1.Visible = true;
                            string _str = string.Empty;
                            int _Result = 0;
                            DataTable dt = new DataTable();

                            #region first delete old data from database
                            string Query = "Delete from SPQR2AInwardSupplies where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                            _Result = objGSTR2A.IUDData(Query);
                            if (_Result != 1)
                            {

                                MessageBox.Show("System error.\nPlease try after sometime - b2b!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            Query = "Delete from SPQR2ACND where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                            _Result = objGSTR2A.IUDData(Query);
                            if (_Result != 1)
                            {

                                MessageBox.Show("System error.\nPlease try after sometime - CDN!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            #endregion

                            RootObject obj = JsonConvert.DeserializeObject<RootObject>(_json);

                            #region ASSIGN GRIDVIEW ROWS IN DATATABLE For B2B

                            dt = new DataTable();
                            dt.Columns.Add("Fld_Sequence");
                            dt.Columns.Add("Fld_GSTIN");
                            dt.Columns.Add("Fld_NameOfParty");
                            dt.Columns.Add("Fld_InvoiceNo");
                            dt.Columns.Add("Fld_InvoiceDate");
                            dt.Columns.Add("Fld_InvoiceValue");
                            dt.Columns.Add("Fld_Rate");
                            dt.Columns.Add("Fld_InvoiceTaxableVal");
                            dt.Columns.Add("Fld_IntTax");
                            dt.Columns.Add("Fld_CtrlTax");
                            dt.Columns.Add("Fld_StateTax");
                            dt.Columns.Add("Fld_CessTax");
                            dt.Columns.Add("Fld_POS");
                            dt.Columns.Add("Fld_ReverseCharge");
                            dt.Columns.Add("Fld_FileStatus");
                            dt.Columns.Add("Fld_InvoiceType");

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
                                        dt.Rows[dt.Rows.Count - 1]["Fld_GSTIN"] = Convert.ToString(obj.b2b[i].ctin);
                                        //ROOT END

                                        //INVOICE DATA START
                                        dt.Rows[dt.Rows.Count - 1]["Fld_InvoiceNo"] = Convert.ToString(obj.b2b[i].inv[j].inum);
                                        dt.Rows[dt.Rows.Count - 1]["Fld_InvoiceDate"] = Convert.ToString(obj.b2b[i].inv[j].idt);
                                        dt.Rows[dt.Rows.Count - 1]["Fld_InvoiceValue"] = Convert.ToString(obj.b2b[i].inv[j].val);
                                        if (Convert.ToString(obj.b2b[i].inv[j].pos) != null)
                                            dt.Rows[dt.Rows.Count - 1]["Fld_POS"] = CommonHelper.GetStateName(Convert.ToString(obj.b2b[i].inv[j].pos));
                                        //dt.Rows[dt.Rows.Count - 1]["Fld_ReverseCharge"] = Convert.ToString(obj.b2b[i].inv[j].rchrg);
                                        dt.Rows[dt.Rows.Count - 1]["Fld_ReverseCharge"] = (Convert.ToString(obj.b2b[i].inv[j].rchrg) == "Y" ? "True" : "False");
                                        dt.Rows[dt.Rows.Count - 1]["Fld_InvoiceType"] = Convert.ToString(obj.b2b[i].inv[j].inv_typ);
                                        //INVOICE DATA END

                                        ////ITEM DATA START
                                        dt.Rows[dt.Rows.Count - 1]["Fld_Rate"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.rt);
                                        dt.Rows[dt.Rows.Count - 1]["Fld_InvoiceTaxableVal"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.txval);
                                        dt.Rows[dt.Rows.Count - 1]["Fld_IntTax"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.iamt);
                                        dt.Rows[dt.Rows.Count - 1]["Fld_CtrlTax"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.camt);
                                        dt.Rows[dt.Rows.Count - 1]["Fld_StateTax"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.samt);
                                        dt.Rows[dt.Rows.Count - 1]["Fld_CessTax"] = Convert.ToString(obj.b2b[i].inv[j].itms[k].itm_det.csamt);

                                        //ITEM DATA END

                                    }
                                }
                            }
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                dt.Rows[i]["Fld_Sequence"] = Convert.ToString(i + 1);
                                dt.Rows[i]["Fld_FileStatus"] = "Completed";
                            }
                            dt.AcceptChanges();

                            if (dt != null && dt.Rows.Count > 0)
                            {
                                DataRow dr = dt.NewRow();
                                dr["Fld_InvoiceNo"] = dt.Rows.Cast<DataRow>().Where(x => Convert.ToString(x["Fld_InvoiceNo"]).Trim() != "").GroupBy(x => x["Fld_InvoiceNo"]).Select(x => x.First()).Distinct().Count();
                                dr["Fld_InvoiceValue"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_InvoiceValue"] != null).Sum(x => x["Fld_InvoiceValue"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_InvoiceValue"])).ToString();
                                dr["Fld_InvoiceTaxableVal"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_InvoiceTaxableVal"] != null).Sum(x => x["Fld_InvoiceTaxableVal"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_InvoiceTaxableVal"])).ToString();
                                dr["Fld_IntTax"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_IntTax"] != null).Sum(x => x["Fld_IntTax"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_IntTax"])).ToString();
                                dr["Fld_CtrlTax"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_CtrlTax"] != null).Sum(x => x["Fld_CtrlTax"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_CtrlTax"])).ToString();
                                dr["Fld_StateTax"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_StateTax"] != null).Sum(x => x["Fld_StateTax"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_StateTax"])).ToString();
                                dr["Fld_CessTax"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_CessTax"] != null).Sum(x => x["Fld_CessTax"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_CessTax"])).ToString();

                                dr["Fld_POS"] = "Completed";
                                dr["Fld_ReverseCharge"] = "False";
                                dr["Fld_FileStatus"] = "Total";
                                dt.Rows.Add(dr);

                                _Result = objGSTR2A.GSTR2A3BulkEntryJson(dt, CommonHelper.SelectedMonth, CommonHelper.ReturnYear);
                                if (_Result != 1)
                                    _str += "B2B data entry error..!\n";

                            }
                            #endregion

                            #region ASSIGN GRIDVIEW ROWS IN DATATABLE For CDN
                            dt = new DataTable();
                            dt.Columns.Add("Fld_Sequence");
                            dt.Columns.Add("Fld_GSTIN");
                            dt.Columns.Add("Fld_PartyName");
                            dt.Columns.Add("Fld_TypeOfNote");
                            dt.Columns.Add("Fld_DbtCrdtNoteNo");
                            dt.Columns.Add("Fld_DbtCrdtNoteDate");
                            dt.Columns.Add("Fld_InvoiceNo");
                            dt.Columns.Add("Fld_Regime");
                            dt.Columns.Add("Fld_Issue");
                            dt.Columns.Add("Fld_NoteValue");
                            dt.Columns.Add("Fld_InvoiceDate");
                            dt.Columns.Add("Fld_Rate");
                            dt.Columns.Add("Fld_Taxable");
                            dt.Columns.Add("Fld_IGSTAmnt");
                            dt.Columns.Add("Fld_CGSTAmnt");
                            dt.Columns.Add("Fld_SGSTAmnt");
                            dt.Columns.Add("Fld_CessAmnt");
                            dt.Columns.Add("Fld_FileStatus");

                            for (int i = 0; i < obj.cdn.Count; i++)
                            {
                                for (int j = 0; j < obj.cdn[i].nt.Count; j++)
                                {
                                    for (int k = 0; k < obj.cdn[i].nt[j].itms.Count; k++)
                                    {
                                        dt.Rows.Add();
                                        //ROOT START
                                        dt.Rows[dt.Rows.Count - 1]["Fld_GSTIN"] = Convert.ToString(obj.cdn[i].ctin);
                                        //ROOT END

                                        //INVOICE DATA START
                                        if (Convert.ToString(obj.cdn[i].nt[j].ntty) == "C")
                                            dt.Rows[dt.Rows.Count - 1]["Fld_TypeOfNote"] = "Credit note";
                                        else if (Convert.ToString(obj.cdn[i].nt[j].ntty) == "D")
                                            dt.Rows[dt.Rows.Count - 1]["Fld_TypeOfNote"] = "Debit note";

                                        dt.Rows[dt.Rows.Count - 1]["Fld_DbtCrdtNoteNo"] = Convert.ToString(obj.cdn[i].nt[j].nt_num);
                                        dt.Rows[dt.Rows.Count - 1]["Fld_DbtCrdtNoteDate"] = Convert.ToString(obj.cdn[i].nt[j].nt_dt);
                                        dt.Rows[dt.Rows.Count - 1]["Fld_InvoiceNo"] = Convert.ToString(obj.cdn[i].nt[j].inum);
                                        //dt.Rows[dt.Rows.Count - 1]["Fld_Regime"] = Convert.ToString(obj.cdn[i].nt[j].rsn);
                                        dt.Rows[dt.Rows.Count - 1]["Fld_Regime"] = (Convert.ToString(obj.cdn[i].nt[j].p_gst) == "Y" ? "Yes" : "No");
                                        dt.Rows[dt.Rows.Count - 1]["Fld_Issue"] = Convert.ToString(obj.cdn[i].nt[j].rsn);
                                        dt.Rows[dt.Rows.Count - 1]["Fld_NoteValue"] = Convert.ToString(obj.cdn[i].nt[j].val);
                                        dt.Rows[dt.Rows.Count - 1]["Fld_InvoiceDate"] = Convert.ToString(obj.cdn[i].nt[j].idt);
                                        //INVOICE DATA END

                                        ////ITEM DATA START
                                        dt.Rows[dt.Rows.Count - 1]["Fld_Rate"] = Convert.ToString(obj.cdn[i].nt[j].itms[k].itm_det.rt);
                                        dt.Rows[dt.Rows.Count - 1]["Fld_Taxable"] = Convert.ToString(obj.cdn[i].nt[j].itms[k].itm_det.txval);
                                        dt.Rows[dt.Rows.Count - 1]["Fld_IGSTAmnt"] = Convert.ToString(obj.cdn[i].nt[j].itms[k].itm_det.iamt);
                                        dt.Rows[dt.Rows.Count - 1]["Fld_CGSTAmnt"] = Convert.ToString(obj.cdn[i].nt[j].itms[k].itm_det.camt);
                                        dt.Rows[dt.Rows.Count - 1]["Fld_SGSTAmnt"] = Convert.ToString(obj.cdn[i].nt[j].itms[k].itm_det.samt);
                                        dt.Rows[dt.Rows.Count - 1]["Fld_CessAmnt"] = Convert.ToString(obj.cdn[i].nt[j].itms[k].itm_det.csamt);


                                        //ITEM DATA END

                                    }
                                }
                            }
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                dt.Rows[i]["Fld_Sequence"] = Convert.ToString(i + 1);
                                dt.Rows[i]["Fld_FileStatus"] = "Completed";
                            }
                            dt.AcceptChanges();

                            if (dt != null && dt.Rows.Count > 0)
                            {
                                DataRow dr = dt.NewRow();
                                dr["Fld_DbtCrdtNoteNo"] = dt.Rows.Cast<DataRow>().Where(x => Convert.ToString(x["Fld_DbtCrdtNoteNo"]).Trim() != "").GroupBy(x => x["Fld_DbtCrdtNoteNo"]).Select(x => x.First()).Distinct().Count();
                                dr["Fld_NoteValue"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_NoteValue"] != null).Sum(x => x["Fld_NoteValue"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_NoteValue"])).ToString();
                                dr["Fld_Taxable"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_Taxable"] != null).Sum(x => x["Fld_Taxable"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_Taxable"])).ToString();
                                dr["Fld_IGSTAmnt"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_IGSTAmnt"] != null).Sum(x => x["Fld_IGSTAmnt"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_IGSTAmnt"])).ToString();
                                dr["Fld_CGSTAmnt"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_CGSTAmnt"] != null).Sum(x => x["Fld_CGSTAmnt"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_CGSTAmnt"])).ToString();
                                dr["Fld_SGSTAmnt"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_SGSTAmnt"] != null).Sum(x => x["Fld_SGSTAmnt"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_SGSTAmnt"])).ToString();
                                dr["Fld_CessAmnt"] = dt.Rows.Cast<DataRow>().Where(x => x["Fld_CessAmnt"] != null).Sum(x => x["Fld_CessAmnt"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(x["Fld_CessAmnt"])).ToString();

                                //dr["Fld_POS"] = "Completed";
                                //dr["Fld_ReverseCharge"] = "False";
                                dr["Fld_FileStatus"] = "Total";
                                dt.Rows.Add(dr);

                                _Result = objGSTR2A.GSTR2A_CNDBulkEntryJson(dt, CommonHelper.SelectedMonth, CommonHelper.ReturnYear);
                                if (_Result != 1)
                                    _str += "CDN data entry error..!\n";

                            }
                            #endregion


                            pbGSTR1.Visible = false;

                            if (_str != "")
                            {
                                CommonHelper.ErrorList = Convert.ToString(_str);
                                SPQErrorList obje = new SPQErrorList();
                                obje.ShowDialog();
                            }
                            else
                            {
                                Getdata();
                                MessageBox.Show("Invoices Imported successfully..", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
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
            }
        }

        private void msImpFromGSP_Click(object sender, EventArgs e)
        {
            try
            {
                #region sHTMl code
                if (Utility.CheckNet())
                {
                    var obj = clsPro.Cooki != null ? clsPro.Cooki.FirstOrDefault(x => x.ckname == (string.Concat("loginCookies_", CommonHelper.CompanyID))) : null;

                    if (obj != null && obj.CC1 != null)
                    {
                        string[] retPeriod = clssummary.ReturnDate();
                        bool IsGSTR2A500Record = objGSTR2A.CheckGSTR2A500Record(retPeriod);
                        if (IsGSTR2A500Record) // Send requiest
                        {
                            pbGSTR1.Visible = true;
                            new PrefillHelper().genratenewrequest("GSTR2A");
                            pbGSTR1.Visible = false;
                            // MessageBox.Show("Please download after 20 minutes! There is greater than 500 records. We have sended request for download!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            pbGSTR1.Visible = true;
                            Application.DoEvents();
                            string _str = "";

                            RootObject objr = new RootObject();
                            response2A objResB2b = new response2A();
                            objResB2b = getProcessedInvoiceB2B();

                            if (objResB2b.flg)
                            {
                                //RootObject objB2b = JsonConvert.DeserializeObject<RootObject>(objResB2b.strJson);
                                //if (objB2b != null)
                                //    objr.b2b = objB2b.b2b;

                                List<B2b> objB2b = JsonConvert.DeserializeObject<List<B2b>>(objResB2b.strJson);
                                if (objB2b != null && objB2b.Count > 0)
                                {
                                    objr.b2b = objB2b;
                                }

                            }

                            response2A objResCdn = new response2A();
                            objResCdn = getProcessedInvoiceCDN();
                            if (objResCdn.flg)
                            {
                                //RootObject objCdn = JsonConvert.DeserializeObject<RootObject>(objResCdn.strJson);
                                //if (objCdn != null)
                                //    objr.cdn = objCdn.cdn;

                                List<Cdn> objCdn = JsonConvert.DeserializeObject<List<Cdn>>(objResCdn.strJson);
                                if (objCdn != null && objCdn.Count > 0)
                                {
                                    objr.cdn = objCdn;
                                }
                            }

                            response2A objResB2ba = new response2A();
                            objResB2ba = getProcessedInvoiceB2BA();
                            if (objResB2ba.flg)
                            {
                                //RootObject objB2ba = JsonConvert.DeserializeObject<RootObject>(objResB2ba.strJson);
                                //if (objB2ba != null)
                                //    objr.b2ba = objB2ba.b2ba;

                                List<B2ba> objB2ba = JsonConvert.DeserializeObject<List<B2ba>>(objResB2ba.strJson);
                                if (objB2ba != null && objB2ba.Count > 0)
                                {
                                    objr.b2ba = objB2ba;
                                }
                            }

                            response2A objResCdnA = new response2A();
                            objResCdnA = getProcessedInvoiceCDNA();
                            if (objResCdnA.flg)
                            {
                                //RootObject objCdnA = JsonConvert.DeserializeObject<RootObject>(objResCdnA.strJson);
                                //if (objCdnA != null)
                                //    objr.cdna = objCdnA.cdna;

                                List<Cdna> objCdnA = JsonConvert.DeserializeObject<List<Cdna>>(objResCdnA.strJson);
                                if (objCdnA != null && objCdnA.Count > 0)
                                {
                                    objr.cdna = objCdnA;
                                }
                            }

                            if (!objResB2b.flg && !objResCdn.flg && !objResB2ba.flg)
                            {
                                if (objResB2b.msg.Length > 0)
                                    MessageBox.Show(objResB2b.msg);
                                else if (objResCdn.msg.Length > 0)
                                    MessageBox.Show(objResCdn.msg);
                                else if (objResB2ba.msg.Length > 0)
                                    MessageBox.Show(objResB2ba.msg);
                                else if (objResCdnA.msg.Length > 0)
                                    MessageBox.Show(objResCdnA.msg);
                            }
                            else
                            {
                                objr.gstin = CommonHelper.CompanyGSTN;
                                objr.fp = CommonHelper.GetMonth(CommonHelper.SelectedYear.ToString().Split('-')[0].Trim()) + CommonHelper.SelectedYear.ToString().Split('-')[1].Trim();
                                objr.gt = CommonHelper.TurnOver;
                                objr.cur_gt = CommonHelper.CurrentTurnOver;

                                string jsonString = JsonConvert.SerializeObject(objr);

                                Application.DoEvents();
                                bool flg = false;

                                #region first delete old data from database
                                int _Result = 0;
                                string Query = "";

                                Query = "Delete from SPQR2AInwardSupplies where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                                _Result = objGSTR2A.IUDData(Query);

                                Query = "Delete from SPQR2ACND where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                                _Result = objGSTR2A.IUDData(Query);

                                Query = "Delete from SPQR2AB2BAmend where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                                _Result = objGSTR2A.IUDData(Query);

                                Query = "Delete from SPQR2ACNDAmend where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                                _Result = objGSTR2A.IUDData(Query);
                                #endregion

                                #region b2b
                                if (objResB2b.flg)
                                {
                                    flg = b2bEntry(jsonString, CommonHelper.SelectedMonth, CommonHelper.ReturnYear);
                                    if (flg == false)
                                        _str += "B2b data entry error...\n";
                                }
                                #endregion

                                #region cdn
                                if (objResCdn.flg)
                                {
                                    flg = cdnEntry(jsonString, CommonHelper.SelectedMonth, CommonHelper.ReturnYear);
                                    if (flg == false)
                                        _str += "Cdn data entry error...\n";
                                }
                                #endregion

                                #region b2ba
                                if (objResB2ba.flg)
                                {
                                    flg = b2baEntry(jsonString, CommonHelper.SelectedMonth, CommonHelper.ReturnYear);
                                    if (flg == false)
                                        _str += "B2ba data entry error...\n";
                                }
                                #endregion

                                #region cdna
                                if (objResCdnA.flg)
                                {
                                    flg = cdnaEntry(jsonString, CommonHelper.SelectedMonth, CommonHelper.ReturnYear);
                                    if (flg == false)
                                        _str += "Cdna data entry error...\n";
                                }
                                #endregion

                                pbGSTR1.Visible = false;

                                if (_str != "")
                                {
                                    CommonHelper.ErrorList = Convert.ToString(_str);
                                    SPQErrorList obje = new SPQErrorList();
                                    obje.ShowDialog();
                                }
                                else
                                {
                                    Getdata();
                                    MessageBox.Show("Data saved successfully..", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                        }
                    }
                    else
                    {
                        SPQGstLogin frm = new SPQGstLogin();
                        frm.Visible = false;
                        var result = frm.ShowDialog();
                        if (result != DialogResult.OK)
                        {
                            SPQGstLogin objLogin = new SPQGstLogin();
                            objLogin.Show();
                        }
                        else
                        {
                            msImpFromGSP_Click(sender, e);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("It Seems Your Internet Conection is Not Available, Please Connect Internet…!!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                pbGSTR1.Visible = false;
                #endregion

                #region From GSP

                //pbGSTR1.Visible = true;
                //string _str = string.Empty;
                //int _Result = 0;
                //string b2bData = "", cdnData = "";
                //DataTable dt = new DataTable();

                //#region first delete old data from database
                //string Query = "Delete from SPQR2AInwardSupplies where Fld_Month='" + CommonHelper.SelectedMonth + "'";
                //_Result = objGSTR2A.IUDData(Query);
                //if (_Result != 1)
                //{
                //    MessageBox.Show("System error.\nPlease try after sometime - b2b!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                //Query = "Delete from SPQR2ACND where Fld_Month='" + CommonHelper.SelectedMonth + "'";
                //_Result = objGSTR2A.IUDData(Query);
                //if (_Result != 1)
                //{
                //    MessageBox.Show("System error.\nPlease try after sometime - CDN!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                //#endregion

                //#region Download Json Data

                //if (SetGSPSetting())
                //{
                //    pbGSTR1.Visible = true;

                //    GSPApisetting builder = new GSPApisetting();
                //    DataTable data = new DataTable();
                //    double mi = 0;

                //    var request = (HttpWebRequest)WebRequest.Create("http://13.126.181.225:8000/SPEQTAGSTOffLineUtility/GetOffGstnApi?GSTIN=" + CommonHelper.CompanyGSTN + "");
                //    request.Headers.Add("Token", "MVPLGSPTKN221232");
                //    string webData = Utility.GetApi(request);
                //    RootObjectGSP b2bs = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObjectGSP>(webData);

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

                //                SPEQTAGST.Softmodel.GSTR2A obj2A = new SPEQTAGST.Softmodel.GSTR2A();
                //                bool flg = false;

                //                #region b2b
                //                b2bData = obj2A.Download2AB2B("B2B");
                //                if (b2bData != "")
                //                    flg = b2bEntry(b2bData);
                //                if (flg == false)
                //                    _str += "B2b data entry error...\n";
                //                #endregion

                //                #region cdn
                //                if (flg)
                //                {
                //                    cdnData = obj2A.Download2AB2B("CDN");
                //                    if (cdnData != "")
                //                        flg = cdnEntry(cdnData);
                //                    if (flg == false)
                //                        _str += "Cdn data entry error...\n";
                //                }
                //                #endregion
                //            }
                //        }
                //        else // if otp no need
                //        {
                //            SPEQTAGST.Softmodel.GSTR2A obj2A = new SPEQTAGST.Softmodel.GSTR2A();
                //            bool flg = false;

                //            #region b2b
                //            b2bData = obj2A.Download2AB2B("B2B");
                //            if (b2bData != "")
                //                flg = b2bEntry(b2bData);
                //            if (flg == false)
                //                _str += "B2b data entry error...\n";
                //            #endregion

                //            #region cdn
                //            if (flg)
                //            {
                //                cdnData = obj2A.Download2AB2B("CDN");
                //                if (cdnData != "")
                //                    flg = cdnEntry(cdnData);
                //                if (flg == false)
                //                    _str += "Cdn data entry error...\n";
                //            }
                //            #endregion
                //        }
                //    }
                //    else
                //    {
                //        RootObjectGSP obj = new RootObjectGSP();
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

                //            SPEQTAGST.Softmodel.GSTR2A obj2A = new SPEQTAGST.Softmodel.GSTR2A();
                //            bool flg = false;

                //            #region b2b
                //            b2bData = obj2A.Download2AB2B("B2B");
                //            if (b2bData != "")
                //                flg = b2bEntry(b2bData);
                //            if (flg == false)
                //                _str += "B2b data entry error...\n";
                //            #endregion

                //            #region cdn
                //            if (flg)
                //            {
                //                cdnData = obj2A.Download2AB2B("CDN");
                //                if (cdnData != "")
                //                    flg = cdnEntry(cdnData);
                //                if (flg == false)
                //                    _str += "Cdn data entry error...\n";
                //            }
                //            #endregion
                //        }
                //    }
                //}
                //#endregion

                //pbGSTR1.Visible = false;

                //if (_str != "")
                //{
                //    CommonHelper.ErrorList = Convert.ToString(_str);
                //    ErrorList obje = new ErrorList();
                //    obje.ShowDialog();
                //}
                //else
                //{
                //    Getdata();
                //    MessageBox.Show("Invoices download successfully..", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void msImpJson_Click(object sender, EventArgs e)
        {
            try
            {
                //Download Json Data
                OpenFileDialog ofdJson = new OpenFileDialog();
                ofdJson.Multiselect = true;
                ofdJson.Title = "Browse Json File";
                ofdJson.CheckFileExists = true;
                ofdJson.CheckPathExists = true;
                ofdJson.DefaultExt = "txt";
                ofdJson.Filter = "Json File|*.json";
                ofdJson.FilterIndex = 2;
                ofdJson.RestoreDirectory = true;
                ofdJson.ReadOnlyChecked = true;
                ofdJson.ShowReadOnly = true;

                if (ofdJson.ShowDialog() == DialogResult.OK)
                {
                    pbGSTR1.Visible = true;

                    List<string> jsonString = new List<string>();
                    // Read the files
                    foreach (String file in ofdJson.FileNames)
                    {
                        StreamReader sr = new StreamReader(file);
                        string _jsonString = sr.ReadToEnd();
                        if (_jsonString != null && _jsonString.Trim() != "")
                        {
                            jsonString.Add(_jsonString);
                        }
                    }
                    if (jsonString != null && jsonString.Count > 0)
                    {
                        JsonImportMethod(jsonString, CommonHelper.SelectedMonth, CommonHelper.ReturnYear);
                        MessageBox.Show("Data saved successfully..", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    //StreamReader sr = new StreamReader(ofdJson.FileName);
                    //string jsonString = sr.ReadToEnd();
                    //JsonImportMethod(jsonString);
                    pbGSTR1.Visible = false;
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

        //public void JsonImportMethod(string jsonString)
        public void JsonImportMethod(List<string> _jsonString, string strMonth, string ReturnYear)
        {
            if (strMonth == null || strMonth == "") strMonth = CommonHelper.SelectedMonth;
            if (ReturnYear == null || ReturnYear == "") ReturnYear = CommonHelper.ReturnYear;

            string _str = string.Empty;
            int _Result = 0;
            DataTable dt = new DataTable();

            #region first delete old data from database
            string Query = "Delete from SPQR2AInwardSupplies where Fld_Month='" + strMonth + "' AND Fld_FinancialYear = '" + ReturnYear + "'";
            _Result = objGSTR2A.IUDData(Query);
            if (_Result != 1)
            {
                MessageBox.Show("System error.\nPlease try after sometime - B2B!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Query = "Delete from SPQR2ACND where Fld_Month='" + strMonth + "' AND Fld_FinancialYear = '" + ReturnYear + "'";
            _Result = objGSTR2A.IUDData(Query);
            if (_Result != 1)
            {
                MessageBox.Show("System error.\nPlease try after sometime - CDN!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Query = "Delete from SPQR2AB2BAmend where Fld_Month='" + strMonth + "' AND Fld_FinancialYear = '" + ReturnYear + "'";
            _Result = objGSTR2A.IUDData(Query);
            if (_Result != 1)
            {
                MessageBox.Show("System error.\nPlease try after sometime - B2BA!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Query = "Delete from SPQR2ACNDAmend where Fld_Month='" + strMonth + "' AND Fld_FinancialYear = '" + ReturnYear + "'";
            _Result = objGSTR2A.IUDData(Query);
            if (_Result != 1)
            {
                MessageBox.Show("System error.\nPlease try after sometime - CDNA!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            #endregion

            if (_jsonString != null && _jsonString.Count > 0)
            {
                foreach (string item in _jsonString)
                {
                    string jsonString = item;
                    if (Convert.ToString(jsonString).Trim() != "")
                    {
                        bool flg = false;
                        #region b2b
                        flg = b2bEntry(jsonString, strMonth, ReturnYear);
                        if (flg == false)
                            _str += "B2b data entry error...\n";
                        #endregion

                        #region cdn
                        if (flg)
                        {
                            flg = cdnEntry(jsonString, strMonth, ReturnYear);
                            if (flg == false)
                                _str += "Cdn data entry error...\n";
                        }
                        #endregion

                        #region b2ba
                        flg = b2baEntry(jsonString, strMonth, ReturnYear);
                        if (flg == false)
                            _str += "B2ba data entry error...\n";
                        #endregion

                        #region cdna
                        flg = cdnaEntry(jsonString, strMonth, ReturnYear);
                        if (flg == false)
                            _str += "Cdna data entry error...\n";
                        #endregion
                    }
                }

                #region manage Total Row value

                #region FOR SPQR2AInwardSupplies (B2B)
                DataTable dtTotal = new DataTable();
                string query = "select sum(Fld_InvoiceNo) AS Fld_InvoiceNo, sum(Fld_InvoiceValue) AS Fld_InvoiceValue, sum(Fld_InvoiceTaxableVal) AS Fld_InvoiceTaxableVal, ";
                query = query + "sum(Fld_IntTax) AS Fld_IntTax, sum(Fld_CtrlTax) AS Fld_CtrlTax, sum(Fld_StateTax) AS Fld_StateTax, sum(Fld_CessTax) AS Fld_CessTax  from SPQR2AInwardSupplies where Fld_Month = '" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus = 'Total' ";
                
                dtTotal = objGSTR2A.GetDataGSTR2A(query);
                if (dtTotal != null && dtTotal.Rows.Count > 0)
                {
                    // Delete All Total Rows
                    Query = "Delete from SPQR2AInwardSupplies where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus = 'Total' ";
                    _Result = objGSTR2A.IUDData(Query);
                    if (_Result != 1)
                    {
                        MessageBox.Show("System error.\nPlease try after sometime - B2B!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //return;
                    }
                    else
                    {
                        // Insert Total Value
                        Query = "INSERT INTO SPQR2AInwardSupplies (Fld_InvoiceNo, Fld_InvoiceValue, Fld_InvoiceTaxableVal, Fld_IntTax, Fld_CtrlTax, Fld_StateTax, Fld_CessTax, Fld_POS, Fld_ReverseCharge, Fld_FileStatus, Fld_Month, Fld_FinancialYear) ";
                        Query = Query + "VALUES ('" + Convert.ToString(dtTotal.Rows[0]["Fld_InvoiceNo"]) + "','" + Convert.ToString(dtTotal.Rows[0]["Fld_InvoiceValue"]) + "','" + Convert.ToString(dtTotal.Rows[0]["Fld_InvoiceTaxableVal"]) + "','" + Convert.ToString(dtTotal.Rows[0]["Fld_IntTax"]) + "','" + Convert.ToString(dtTotal.Rows[0]["Fld_CtrlTax"]) + "','" + Convert.ToString(dtTotal.Rows[0]["Fld_StateTax"]) + "','" + Convert.ToString(dtTotal.Rows[0]["Fld_CessTax"]) + "','Completed','False','Total','" + CommonHelper.SelectedMonth + "','" + CommonHelper.ReturnYear + "')";
                        _Result = objGSTR2A.IUDData(Query);
                        //if (_Result != 1)
                        //{
                        //    MessageBox.Show("System error.\nPlease try after sometime - B2B!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //    return;
                        //}
                    }
                }
                #endregion

                #region FOR SPQR2ACND (CDN)
                dtTotal = new DataTable();
                query = "select sum(Fld_DbtCrdtNoteNo) AS Fld_DbtCrdtNoteNo, sum(Fld_NoteValue) AS Fld_NoteValue, sum(Fld_Taxable) AS Fld_Taxable, ";
                query = query + "sum(Fld_IGSTAmnt) AS Fld_IGSTAmnt, sum(Fld_CGSTAmnt) AS Fld_CGSTAmnt, sum(Fld_SGSTAmnt) AS Fld_SGSTAmnt, sum(Fld_CessAmnt) AS Fld_CessAmnt from SPQR2ACND where Fld_Month = '" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus = 'Total' ";
                
                dtTotal = objGSTR2A.GetDataGSTR2A(query);
                if (dtTotal != null && dtTotal.Rows.Count > 0)
                {
                    // Delete All Total Rows
                    Query = "Delete from SPQR2ACND where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus = 'Total' ";
                    _Result = objGSTR2A.IUDData(Query);
                    if (_Result != 1)
                    {
                        MessageBox.Show("System error.\nPlease try after sometime - B2B!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //return;
                    }
                    else
                    {
                        // Insert Total Value
                        Query = "INSERT INTO SPQR2ACND (Fld_DbtCrdtNoteNo,Fld_NoteValue,Fld_Taxable,Fld_IGSTAmnt,Fld_CGSTAmnt,Fld_SGSTAmnt,Fld_CessAmnt,Fld_FileStatus,Fld_Month,Fld_FinancialYear) ";
                        Query = Query + "VALUES ('" + Convert.ToString(dtTotal.Rows[0]["Fld_DbtCrdtNoteNo"]) + "','" + Convert.ToString(dtTotal.Rows[0]["Fld_NoteValue"]) + "','" + Convert.ToString(dtTotal.Rows[0]["Fld_Taxable"]) + "','" + Convert.ToString(dtTotal.Rows[0]["Fld_IGSTAmnt"]) + "','" + Convert.ToString(dtTotal.Rows[0]["Fld_CGSTAmnt"]) + "','" + Convert.ToString(dtTotal.Rows[0]["Fld_SGSTAmnt"]) + "','" + Convert.ToString(dtTotal.Rows[0]["Fld_CessAmnt"]) + "','Total','" + CommonHelper.SelectedMonth + "','" + CommonHelper.ReturnYear + "')";
                        _Result = objGSTR2A.IUDData(Query);
                        //if (_Result != 1)
                        //{
                        //    MessageBox.Show("System error.\nPlease try after sometime - CDN!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //    return;
                        //}
                    }
                }
                #endregion

                #region FOR SPQR2AB2BAmend (B2BA)
                dtTotal = new DataTable();
                query = "select sum(Fld_OrgInvoiceNo) AS Fld_OrgInvoiceNo, sum(Fld_ResInvoiceNo) AS Fld_ResInvoiceNo, sum(Fld_InvoiceValue) AS Fld_InvoiceValue, sum(Fld_TaxableVal) AS Fld_TaxableVal, sum(Fld_IntTax) AS Fld_IntTax, sum(Fld_CtrlTax) AS Fld_CtrlTax, ";
                query = query + "sum(Fld_StateTax) AS Fld_StateTax, sum(Fld_CessTax) AS Fld_CessTax from SPQR2AB2BAmend where Fld_Month = '" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus = 'Total' ";
                dtTotal = objGSTR2A.GetDataGSTR2A(query);

                if (dtTotal != null && dtTotal.Rows.Count > 0)
                {
                    // Delete All Total Rows
                    Query = "Delete from SPQR2AB2BAmend where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus = 'Total' ";
                    _Result = objGSTR2A.IUDData(Query);
                    if (_Result != 1)
                    {
                        MessageBox.Show("System error.\nPlease try after sometime - B2B!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        // return;
                    }
                    else
                    {
                        // Insert Total Value
                        Query = "INSERT INTO SPQR2AB2BAmend (Fld_OrgInvoiceNo, Fld_ResInvoiceNo, Fld_POS, Fld_SupAttResCharge, Fld_InvoiceValue, Fld_TaxableVal, Fld_IntTax, Fld_CtrlTax, Fld_StateTax, Fld_CessTax, Fld_FileStatus, Fld_Month, Fld_FinancialYear) ";
                        Query = Query + "VALUES ('" + Convert.ToString(dtTotal.Rows[0]["Fld_OrgInvoiceNo"]) + "','" + Convert.ToString(dtTotal.Rows[0]["Fld_ResInvoiceNo"]) + "','Completed','False','" + Convert.ToString(dtTotal.Rows[0]["Fld_InvoiceValue"]) + "','" + Convert.ToString(dtTotal.Rows[0]["Fld_TaxableVal"]) + "','" + Convert.ToString(dtTotal.Rows[0]["Fld_IntTax"]) + "','" + Convert.ToString(dtTotal.Rows[0]["Fld_CtrlTax"]) + "','" + Convert.ToString(dtTotal.Rows[0]["Fld_StateTax"]) + "','" + Convert.ToString(dtTotal.Rows[0]["Fld_CessTax"]) + "','Total','" + CommonHelper.SelectedMonth + "','" + CommonHelper.ReturnYear + "')";
                        _Result = objGSTR2A.IUDData(Query);
                        //if (_Result != 1)
                        //{
                        //    MessageBox.Show("System error.\nPlease try after sometime - B2BA!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //    return;
                        //}
                    }
                }
                #endregion

                #region FOR SPQR2ACNDAmend (CDNA)
                dtTotal = new DataTable();
                query = "select sum(Fld_DbtCrdtNoteNo) AS Fld_DbtCrdtNoteNo, sum(Fld_OrgInvoiceNo) AS Fld_OrgInvoiceNo, sum(Fld_InvoiceNo) AS Fld_InvoiceNo, sum(Fld_DiffPer) AS Fld_DiffPer, ";
                query = query + "sum(Fld_NoteValue) AS Fld_NoteValue, sum(Fld_Taxable) AS Fld_Taxable,sum(Fld_IGSTAmnt) AS Fld_IGSTAmnt, sum(Fld_CGSTAmnt) AS Fld_CGSTAmnt, sum(Fld_SGSTAmnt) AS Fld_SGSTAmnt, ";
                query = query + "sum(Fld_CessAmnt) AS Fld_CessAmnt from SPQR2ACNDAmend where Fld_Month = '" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus = 'Total' ";
                dtTotal = objGSTR2A.GetDataGSTR2A(query);

                if (dtTotal != null && dtTotal.Rows.Count > 0)
                {
                    // Delete All Total Rows
                    Query = "Delete from SPQR2ACNDAmend where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus = 'Total' ";
                    _Result = objGSTR2A.IUDData(Query);
                    if (_Result != 1)
                    {
                        MessageBox.Show("System error.\nPlease try after sometime - B2B!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                       // return;
                    }
                    else
                    {
                        // Insert Total Value
                        Query = "INSERT INTO SPQR2ACNDAmend (Fld_DbtCrdtNoteNo, Fld_OrgInvoiceNo, Fld_InvoiceNo, Fld_DiffPer, Fld_NoteValue, Fld_Taxable, Fld_IGSTAmnt, Fld_CGSTAmnt, Fld_SGSTAmnt, Fld_CessAmnt, Fld_FileStatus, Fld_Month, Fld_FinancialYear) ";
                        Query = Query + "VALUES ('" + Convert.ToString(dtTotal.Rows[0]["Fld_DbtCrdtNoteNo"]) + "','" + Convert.ToString(dtTotal.Rows[0]["Fld_OrgInvoiceNo"]) + "','" + Convert.ToString(dtTotal.Rows[0]["Fld_InvoiceNo"]) + "','" + Convert.ToString(dtTotal.Rows[0]["Fld_DiffPer"]) + "','" + Convert.ToString(dtTotal.Rows[0]["Fld_NoteValue"]) + "','" + Convert.ToString(dtTotal.Rows[0]["Fld_Taxable"]) + "','" + Convert.ToString(dtTotal.Rows[0]["Fld_IGSTAmnt"]) + "','" + Convert.ToString(dtTotal.Rows[0]["Fld_CGSTAmnt"]) + "','" + Convert.ToString(dtTotal.Rows[0]["Fld_SGSTAmnt"]) + "','" + Convert.ToString(dtTotal.Rows[0]["Fld_CessAmnt"]) + "','Total','" + CommonHelper.SelectedMonth + "','" + CommonHelper.ReturnYear + "')";
                        _Result = objGSTR2A.IUDData(Query);
                        //if (_Result != 1)
                        //{
                        //    MessageBox.Show("System error.\nPlease try after sometime - B2BA!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //    return;
                        //}
                    }
                }
                #endregion

                #endregion
            }
            if (_str != "")
            {
                Getdata();
                CommonHelper.ErrorList = Convert.ToString(_str);
                SPQErrorList obje = new SPQErrorList();
                obje.ShowDialog();
            }
            else
            {
                Getdata();
            }
        }

        private void msExpExcel_Click(object sender, EventArgs e)
        {
            DataTable dt;
            string query = "";

            try
            {
                #region Bind Data
                dt = new DataTable();
                query = "Select Fld_Sequence,Fld_GSTIN,Fld_NameOfParty,Fld_InvoiceNo,Fld_InvoiceDate,Fld_InvoiceValue,Fld_Rate,Fld_InvoiceTaxableVal,Fld_IntTax,Fld_CtrlTax,Fld_StateTax,Fld_CessTax,Fld_POS,Fld_ReverseCharge,Fld_InvoiceType from SPQR2AInwardSupplies where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
                dt = objGSTR2A.GetDataGSTR2A(query);
                #endregion

                if (DgvMain.Rows.Count > 1)
                {
                    // IF RECORDS ARE PRESENT IN MAIN GRID

                    pbGSTR1.Visible = true;

                    #region comp Details
                    DataTable dtComp = new DataTable();
                    dtComp.Columns.Add("CompName");
                    dtComp.Columns.Add("CompValue");
                    dtComp.Columns.Add("CompSite");
                    dtComp.Rows.Add("Company Name : ", CommonHelper.OrgCompanyName);
                    dtComp.Rows.Add("Return Period : ", CommonHelper.ReturnYear, Utility.CompanySite());
                    dtComp.Rows.Add("Report Name : ", "");
                    string strVisible = "http://www.speqtagst.com", strHover = "http://www.speqtagst.com", strURL = "http://www.speqtagst.com";
                    #endregion

                    #region CREATE WORKBOOK AND ASSIGN COLUMNNAME
                    Excel.Application excelApp = new Excel.Application();
                    Excel.Workbook WB = excelApp.Workbooks.Add(Missing.Value);

                    Excel.Worksheet newWS = (Excel.Worksheet)excelApp.Worksheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    newWS.Name = "GSTR2A";

                    // DELETE UNUSED WORKSHEETS FROM WORKBOOK
                    foreach (Excel.Worksheet ws in WB.Worksheets)
                    {
                        if (ws.Name != "GSTR2A")
                            ((Excel.Worksheet)WB.Worksheets[ws.Name]).Delete();
                    }

                    #region Assign Company Data to Array
                    dtComp.Rows[2]["CompValue"] = "GSTR2A";
                    // CREATE ARRAY TO HOLD THE DATA OF DATATABLE
                    object[,] arrComp = new object[dtComp.Rows.Count, dtComp.Columns.Count + 1];

                    // ASSIGN DATA TO ARRAY FROM DATATABLE
                    // FOR LICENECE ALLOWS TO EXPORT ALL RECORDS
                    for (int i = 0; i < dtComp.Rows.Count; i++)
                    {
                        for (int j = 0; j < dtComp.Columns.Count; j++)
                        {
                            arrComp[i, j + 1] = Convert.ToString(dtComp.Rows[i][j]);
                        }
                    }

                    Excel.Range rng_logo = (Excel.Range)newWS.get_Range((Excel.Range)newWS.Cells[1, 1], (Excel.Range)newWS.Cells[3, 1]);
                    rng_logo.Merge();
                    string _stLOGO = Application.StartupPath + @"\Export_Excel.jpg";
                    System.Drawing.Image oImage = System.Drawing.Image.FromFile(_stLOGO);
                    System.Windows.Forms.Clipboard.SetDataObject(oImage, true);
                    newWS.Paste(rng_logo, _stLOGO);

                    //SET EXCEL RANGE TO PASTE THE DATA
                    Excel.Range topComp = (Excel.Range)newWS.Cells[1, 1];
                    Excel.Range bottomComp = (Excel.Range)newWS.Cells[dtComp.Rows.Count, dtComp.Columns.Count + 1];
                    Excel.Range sheetRangeComp = newWS.Range[topComp, bottomComp];
                    //FILL ARRAY IN EXCEL
                    sheetRangeComp.Value2 = arrComp;

                    Excel.Range headerRangeComp = (Excel.Range)newWS.get_Range((Excel.Range)newWS.Cells[1, 1], (Excel.Range)newWS.Cells[3, 2]);
                    headerRangeComp.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    headerRangeComp.Font.Bold = true;

                    #endregion

                    int temp = 0;
                    // ASSIGN COLUMN HEADER AS PAR THE GRID HEADER
                    for (int i = 1; i < DgvMain.Columns.Count + 1; i++)
                    {
                        newWS.Cells[1 + 4, i] = DgvMain.Columns[temp].HeaderText.ToString();
                        // SET COLUMN WIDTH
                        if (i == 1 || i == 0)
                            ((Excel.Range)newWS.Cells[1 + 4, i]).ColumnWidth = 35;
                        else if (i >= 2 && i <= 14)
                            ((Excel.Range)newWS.Cells[1 + 4, i]).ColumnWidth = 10;
                        else
                            ((Excel.Range)newWS.Cells[1 + 4, i]).ColumnWidth = 15;
                        temp++;
                    }

                    // GET RANGE AND SET DIFFRENT PROPERTIES
                    Excel.Range headerRange = (Excel.Range)newWS.get_Range((Excel.Range)newWS.Cells[1 + 4, 1], (Excel.Range)newWS.Cells[1 + 4, DgvMain.Columns.Count]);
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
                        for (int j = 0; j < DgvMain.Columns.Count; j++)
                        {
                            arr[i, j] = Convert.ToString(DgvMain.Rows[i].Cells[j].Value);
                        }
                    }

                    //SET EXCEL RANGE TO PASTE THE DATA
                    Excel.Range top = (Excel.Range)newWS.Cells[2 + 4, 1];
                    Excel.Range bottom = (Excel.Range)newWS.Cells[DgvMain.Rows.Count + 7 + 4, DgvMain.Columns.Count];
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

        private void msClose_Click(object sender, EventArgs e)
        {
            SPQCompanyDashboard obj = new SPQCompanyDashboard();
            obj.MdiParent = this.MdiParent;
            this.Close();
            obj.Dock = DockStyle.Fill;
            obj.Show();
        }


        #region Json Class
        public class ItmDet
        {
            public double rt { get; set; }
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
            public double val { get; set; }
            public double samt { get; set; }
            public double txval { get; set; }
            public double camt { get; set; }
            public string inum { get; set; }
            public string oinum { get; set; }
            public double iamt { get; set; }
            public double csamt { get; set; }
            public string inv_typ { get; set; }
            public string pos { get; set; }
            public string idt { get; set; }
            public string oidt { get; set; }
            public string rchrg { get; set; }
            public string chksum { get; set; }
            public string updby { get; set; }
            public string cflag { get; set; }
            public List<Itm> itms { get; set; }
        }


        public class Nt
        {
            public string chksum { get; set; }
            public string ntty { get; set; }
            public string nt_num { get; set; }
            public string nt_dt { get; set; }
            public string rsn { get; set; }
            public string p_gst { get; set; }
            public string inum { get; set; }
            public string idt { get; set; }
            public double val { get; set; }
            public string ont_num { get; set; }
            public string ont_dt { get; set; }
            public double diff_percent { get; set; }
            public List<Itm> itms { get; set; }
        }

        public class Cdn
        {
            public string ctin { get; set; }
            public string cfs { get; set; }
            public object cname { get; set; }
            public List<Nt> nt { get; set; }
        }
        public class B2b
        {
            public string ctin { get; set; }
            public string cfs { get; set; }
            public object cname { get; set; }
            public List<Inv> inv { get; set; }
        }
        public class B2ba
        {
            public List<Inv> inv { get; set; }
            public string cfs { get; set; }
            public object cname { get; set; }
            public string ctin { get; set; }
        }
        public class Cdna
        {
            public string ctin { get; set; }
            public string cfs { get; set; }
            public object cname { get; set; }
            public List<Nt> nt { get; set; }
        }

        public class RootObject
        {
            public string gstin { get; set; }
            public string fp { get; set; }
            public double gt { get; set; }
            public double cur_gt { get; set; }
            public List<B2b> b2b { get; set; }
            public List<Cdn> cdn { get; set; }
            public List<B2ba> b2ba { get; set; }
            public List<Cdna> cdna { get; set; }
        }
        #endregion

        #region GSP Class
        public class RootObjectGSP
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

        #region CellFormating Scroll CellValueChanged KeyDown Events

        private void DgvMain_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridViewCell cell = DgvMain.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (cell.Value.ToString() == "Completed")
            {
                e.CellStyle.ForeColor = Color.Green;
            }
            else if (cell.Value.ToString() == "Not-Completed")
            {
                e.CellStyle.ForeColor = Color.Red;
            }
            else if (cell.Value.ToString() == "Draft")
            {
                e.CellStyle.ForeColor = Color.Blue;
            }
        }

        private void dgvaccount_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            dgvaccount.Refresh();
            if (chkCellValue(Convert.ToString(dgvaccount.Rows[e.RowIndex].Cells[e.ColumnIndex].Value).Trim()))
            {
                int Cvalue = Convert.ToInt32(dgvaccount.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                if (dgvaccount.Columns[e.ColumnIndex].Name == "Document Count")
                {
                    dgvdiff.Rows[0].Cells["Document Count"].Value = Convert.ToInt32(DgvMain.Rows[DgvMain.Rows.Count - 1].Cells["Document Count"].Value) - Cvalue;
                }
                //else if (dgvaccount.Columns[e.ColumnIndex].Name == "Invoice Value")
                //{
                //    dgvdiff.Rows[0].Cells["Invoice Value"].Value = Convert.ToInt32(DgvMain.Rows[DgvMain.Rows.Count - 1].Cells["Invoice Value"].Value) - Cvalue;
                //}
                else if (dgvaccount.Columns[e.ColumnIndex].Name == "Taxable Value")
                {
                    dgvdiff.Rows[0].Cells["Taxable Value"].Value = Convert.ToInt32(DgvMain.Rows[DgvMain.Rows.Count - 1].Cells["Taxable Value"].Value) - Cvalue;
                }
                else if (dgvaccount.Columns[e.ColumnIndex].Name == "IGST")
                {
                    dgvdiff.Rows[0].Cells["IGST"].Value = Convert.ToInt32(DgvMain.Rows[DgvMain.Rows.Count - 1].Cells["IGST"].Value) - Cvalue;
                }
                else if (dgvaccount.Columns[e.ColumnIndex].Name == "CGST")
                {
                    dgvdiff.Rows[0].Cells["CGST"].Value = Convert.ToInt32(DgvMain.Rows[DgvMain.Rows.Count - 1].Cells["CGST"].Value) - Cvalue;

                }
                else if (dgvaccount.Columns[e.ColumnIndex].Name == "SGST")
                {
                    dgvdiff.Rows[0].Cells["SGST"].Value = Convert.ToInt32(DgvMain.Rows[DgvMain.Rows.Count - 1].Cells["SGST"].Value) - Cvalue;

                }
                else if (dgvaccount.Columns[e.ColumnIndex].Name == "Cess")
                {
                    dgvdiff.Rows[0].Cells["Cess"].Value = Convert.ToInt32(DgvMain.Rows[DgvMain.Rows.Count - 1].Cells["Cess"].Value) - Cvalue;

                }
            }
            else { dgvaccount.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = ""; dgvdiff.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = ""; }
        }

        private void dgvaccount_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.dgvaccount.ClearSelection();
            this.dgvdiff.ClearSelection();
            this.DgvMain.ClearSelection();
        }

        private void dgvaccount_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            e.CellStyle.ForeColor = Color.Red;
        }

        private void dgvdiff_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            e.CellStyle.ForeColor = Color.Green;
        }

        private void dgvdiff_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                // SET TOTAL GRID OFFSET AS PAR MAIN GRID SCROLL
                this.DgvMain.HorizontalScrollingOffset = this.dgvdiff.HorizontalScrollingOffset;
                this.dgvaccount.HorizontalScrollingOffset = this.dgvdiff.HorizontalScrollingOffset;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
            }
        }

        private void dgvaccount_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                // SET TOTAL GRID OFFSET AS PAR MAIN GRID SCROLL
                this.DgvMain.HorizontalScrollingOffset = this.dgvaccount.HorizontalScrollingOffset;
                this.dgvdiff.HorizontalScrollingOffset = this.dgvaccount.HorizontalScrollingOffset;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
            }
        }

        private void DgvMain_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                // SET TOTAL GRID OFFSET AS PAR MAIN GRID SCROLL
                this.dgvaccount.HorizontalScrollingOffset = this.DgvMain.HorizontalScrollingOffset;
                this.dgvdiff.HorizontalScrollingOffset = this.DgvMain.HorizontalScrollingOffset;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
            }
        }

        #endregion

        private void btnRequest_Click(object sender, EventArgs e)
        {
            try
            {
                pbGSTR1.Visible = true;
                new PrefillHelper().genratenewrequest("GSTR2A");
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

        private void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                //DataTable dt = new DataTable();
                //string Query = "Select * from SPQJsonDownloadMsg where Fld_Month='" + CommonHelper.SelectedMonth + "'";
                //dt = objGSTR2A.GetDataGSTR2A(Query);
                //if (dt != null && dt.Rows.Count > 0 && Convert.ToDateTime(dt.Rows[0]["Fld_DownloadTime"]) > DateTime.Now)
                //{
                //    MessageBox.Show(Convert.ToString(dt.Rows[0]["Fld_Msg"]), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}
                //else
                //{
                pbGSTR1.Visible = true;
                new PrefillHelper().downloadgstrfile("GSTR2A", CommonHelper.SelectedMonth);
                pbGSTR1.Visible = false;
                //}

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

        private void msRequest_Click(object sender, EventArgs e)
        {
            try
            {
                pbGSTR1.Visible = true;
                new PrefillHelper().genratenewrequest("GSTR2A");
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

        private void msDownload_Click(object sender, EventArgs e)
        {
            //DataTable dt = new DataTable();
            //string Query = "Select * from SPQJsonDownloadMsg where Fld_Month='" + CommonHelper.SelectedMonth + "'";
            //dt = objGSTR2A.GetDataGSTR2A(Query);
            //if (dt != null && dt.Rows.Count > 0 && Convert.ToDateTime(dt.Rows[0]["Fld_DownloadTime"]) > DateTime.Now)
            //{
            //    MessageBox.Show(Convert.ToString(dt.Rows[0]["Fld_Msg"]), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
            //else
            //{
            pbGSTR1.Visible = true;
            new PrefillHelper().downloadgstrfile("GSTR2A", CommonHelper.SelectedMonth);
            pbGSTR1.Visible = false;
            //}

        }

        private void msExpExcelAll_Click(object sender, EventArgs e)
        {
            string query = "";
            try
            {
                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook WB = excelApp.Workbooks.Add(Missing.Value);


                #region CDN
                #region Bind Data of CDN
                DataTable dtCDN = new DataTable();
                query = "Select Fld_Sequence,Fld_GSTIN,Fld_PartyName,Fld_TypeOfNote,Fld_DbtCrdtNoteNo,Fld_DbtCrdtNoteDate,Fld_InvoiceNo,Fld_Regime,Fld_Issue,Fld_NoteValue,Fld_InvoiceDate,Fld_Rate,Fld_Taxable,Fld_IGSTAmnt,Fld_CGSTAmnt,Fld_SGSTAmnt,Fld_CessAmnt,Fld_Submitted,Fld_Month from SPQR2ACND where Fld_FileStatus != 'Total'  AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by case when Fld_Month='April' then 'a' when Fld_month='May' then 'b' when Fld_Month='June' then 'c' when Fld_month='July' then 'd' when Fld_Month='August' then 'e' when Fld_month='September' then 'f' when Fld_Month='October' then 'g' when Fld_month='November' then 'h' when Fld_Month='December' then 'i' when Fld_month='January' then 'j' when Fld_Month='February' then 'k' when Fld_month='March' then 'l' else Fld_month end";
                dtCDN = objGSTR2A.GetDataGSTR2A(query);
                #endregion

                if (dtCDN != null && dtCDN.Rows.Count > 0)
                {
                    #region CREATE WORKBOOK AND ASSIGN COLUMNNAME
                    //Excel.Application excelApp = new Excel.Application();
                    //Excel.Workbook WB = excelApp.Workbooks.Add(Missing.Value);

                    Excel.Worksheet newWSCDN = (Excel.Worksheet)excelApp.Worksheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    newWSCDN.Name = "GSTR2A_CDN";

                    // DELETE UNUSED WORKSHEETS FROM WORKBOOK
                    foreach (Excel.Worksheet ws in WB.Worksheets)
                    {
                        if (ws.Name != "GSTR2A_CDN")
                            ((Excel.Worksheet)WB.Worksheets[ws.Name]).Delete();
                    }
                    int temp = 0;
                    // ASSIGN COLUMN HEADER AS PAR THE GRID HEADER                
                    string[] strHeader = { "", "Sr. #", "GSTIN/UIN of Recipient", "Party Name", "Type of note (Debit/ Credit)", "Debit Note/ credit note/ Refund voucher No.", "Debit Note/ credit note/ Refund voucher Date", "Original Invoice No", "Pre GST Regime Dr./ Cr. Notes", "Reason for issuing note Dr./ Cr. Notes", "Note/Refund Voucher Value", "Original Invoice Date", "Rate", "Taxable Value", "IGST Amount", "CGST Amount", "SGST Amount", "CESS Amount", "Submitted", "Month" };

                    for (int i = 1; i < strHeader.Length; i++)
                    {
                        newWSCDN.Cells[1, i] = strHeader[i].ToString();
                        // SET COLUMN WIDTH
                        if (i == 1)
                            ((Excel.Range)newWSCDN.Cells[1, i]).ColumnWidth = 15;
                        else
                            ((Excel.Range)newWSCDN.Cells[1, i]).ColumnWidth = 20;
                        temp++;
                    }

                    // GET RANGE AND SET DIFFRENT PROPERTIES
                    Excel.Range headerRange = (Excel.Range)newWSCDN.get_Range((Excel.Range)newWSCDN.Cells[1, 1], (Excel.Range)newWSCDN.Cells[1, strHeader.Length]);
                    headerRange.WrapText = true;
                    headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Calibri";
                    #endregion

                    #region COPY DATA FROM DATATABLE TO ARRAY

                    // CREATE ARRAY TO HOLD THE DATA OF DATATABLE
                    object[,] arrCDN = new object[dtCDN.Rows.Count, strHeader.Length];

                    // ASSIGN DATA TO ARRAY FROM DATATABLE
                    // FOR LICENECE ALLOWS TO EXPORT ALL RECORDS
                    for (int i = 0; i < dtCDN.Rows.Count; i++)
                    {
                        for (int j = 0; j < dtCDN.Columns.Count; j++)
                        {
                            if (j == 0)
                                arrCDN[i, j] = i + 1;
                            else
                                arrCDN[i, j] = Convert.ToString(dtCDN.Rows[i][j]);
                        }
                    }

                    //SET EXCEL RANGE TO PASTE THE DATA
                    Excel.Range top = (Excel.Range)newWSCDN.Cells[2, 1];
                    Excel.Range bottom = (Excel.Range)newWSCDN.Cells[dtCDN.Rows.Count + 1, strHeader.Length];
                    Excel.Range sheetRange = newWSCDN.Range[top, bottom];
                    sheetRange.NumberFormat = "General";
                    //FILL ARRAY IN EXCEL
                    sheetRange.Value2 = arrCDN;

                    #endregion
                }
                #endregion

                #region B2B
                #region Bind Data of B2B
                DataTable dtB2B = new DataTable();
                query = "Select Fld_Sequence,Fld_GSTIN,Fld_NameOfParty,Fld_InvoiceNo,Fld_InvoiceDate,Fld_InvoiceValue,Fld_Rate,Fld_InvoiceTaxableVal,Fld_IntTax,Fld_CtrlTax,Fld_StateTax,Fld_CessTax,Fld_POS,Fld_ReverseCharge,Fld_InvoiceType,Fld_Submitted,Fld_Month from SPQR2AInwardSupplies where Fld_FileStatus != 'Total' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by case when Fld_Month='April' then 'a' when Fld_month='May' then 'b' when Fld_Month='June' then 'c' when Fld_month='July' then 'd' when Fld_Month='August' then 'e' when Fld_month='September' then 'f' when Fld_Month='October' then 'g' when Fld_month='November' then 'h' when Fld_Month='December' then 'i' when Fld_month='January' then 'j' when Fld_Month='February' then 'k' when Fld_month='March' then 'l' else Fld_month end";
                dtB2B = objGSTR2A.GetDataGSTR2A(query);
                #endregion

                if (dtB2B != null && dtB2B.Rows.Count > 0)
                {
                    #region CREATE WORKBOOK AND ASSIGN COLUMNNAME
                    //Excel.Application excelApp = new Excel.Application();
                    //Excel.Workbook WB = excelApp.Workbooks.Add(Missing.Value);

                    Excel.Worksheet newWSExtra = (Excel.Worksheet)excelApp.Worksheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    newWSExtra.Name = "GSTR2A_B2B";

                    // DELETE UNUSED WORKSHEETS FROM WORKBOOK
                    foreach (Excel.Worksheet ws in WB.Worksheets)
                    {
                        if (ws.Name != "GSTR2A_B2B")
                            ((Excel.Worksheet)WB.Worksheets[ws.Name]).Delete();
                    }
                    int temp = 0;
                    // ASSIGN COLUMN HEADER AS PAR THE GRID HEADER
                    string[] strHeader = new string[] { "", "Sr. #", "GSTIN of supplier", "Name of Party", "Invoice No.", "Invoice Date", "Invoice Value", "Rate", "Taxable Value", "Integrated Tax Amount", "Central Tax Amount", "State/ UT Tax Amount", "Cess Tax Amount", "Place of supply (Name of State)", "Reverse Charge", "Invoice Type", "Submitted", "Month" };

                    for (int i = 1; i < strHeader.Length; i++)
                    {
                        newWSExtra.Cells[1, i] = strHeader[i].ToString();
                        // SET COLUMN WIDTH
                        if (i == 1)
                            ((Excel.Range)newWSExtra.Cells[1, i]).ColumnWidth = 15;
                        else
                            ((Excel.Range)newWSExtra.Cells[1, i]).ColumnWidth = 20;
                        temp++;
                    }

                    // GET RANGE AND SET DIFFRENT PROPERTIES
                    Excel.Range headerRange = (Excel.Range)newWSExtra.get_Range((Excel.Range)newWSExtra.Cells[1, 1], (Excel.Range)newWSExtra.Cells[1, strHeader.Length]);
                    headerRange.WrapText = true;
                    headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Calibri";
                    #endregion

                    #region COPY DATA FROM DATATABLE TO ARRAY

                    // CREATE ARRAY TO HOLD THE DATA OF DATATABLE
                    object[,] arrExtra = new object[dtB2B.Rows.Count, strHeader.Length];

                    // ASSIGN DATA TO ARRAY FROM DATATABLE
                    // FOR LICENECE ALLOWS TO EXPORT ALL RECORDS
                    for (int i = 0; i < dtB2B.Rows.Count; i++)
                    {
                        for (int j = 0; j < dtB2B.Columns.Count; j++)
                        {
                            if (j == 0)
                                arrExtra[i, j] = i + 1;
                            else
                                arrExtra[i, j] = Convert.ToString(dtB2B.Rows[i][j]);
                        }
                    }

                    //SET EXCEL RANGE TO PASTE THE DATA
                    Excel.Range top = (Excel.Range)newWSExtra.Cells[2, 1];
                    Excel.Range bottom = (Excel.Range)newWSExtra.Cells[dtB2B.Rows.Count + 1, strHeader.Length];
                    Excel.Range sheetRange = newWSExtra.Range[top, bottom];
                    sheetRange.NumberFormat = "@";
                    //FILL ARRAY IN EXCEL
                    sheetRange.Value2 = arrExtra;

                    #endregion
                }
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
                    //    MessageBox.Show("Please close opened related excel file.");
                    //    return;
                    //}

                    // SAVE EXCEL FILE AND CLOSE CREATED APPLICATION
                    WB.SaveAs(saveExcel.FileName);
                    excelApp.Quit();
                    MessageBox.Show("Excel file saved!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }


    #region utility class
    public class response2A
    {
        public string msg { get; set; }
        public string strJson { get; set; }
        public bool flg { get; set; }
    }
    #endregion

}
