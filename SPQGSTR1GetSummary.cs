using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using SPEQTAGST.BAL;
using SPEQTAGST.BAL.M264r1;
using System.Net;
using SPEQTAGST.cachsR2a;
using SPQ.Automation;
using SPEQTAGST.xasjbr1;
using System.ComponentModel;

namespace SPEQTAGST
{
    public partial class SPQGSTR1GetSummary : Form
    {
        r1Publicclass objGSTR5 = new r1Publicclass();
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();

        private HttpWebResponse response;
        AssesseeDetail assesseeModel;
        CookieContainer Cc = new CookieContainer();


        public SPQGSTR1GetSummary()
        {
            InitializeComponent();
            GetData();

            this.dgvB2B.ClearSelection();
            this.dgvB2CL.ClearSelection();
            this.dgvCDN.ClearSelection();
            this.CDnUR.ClearSelection();
            this.dgvNill.ClearSelection();
            this.dgvZeroRated.ClearSelection();
            this.dgvAA.ClearSelection();
            this.dgvAR.ClearSelection();
            this.dgvHSN.ClearSelection();
            this.dgvDoc.ClearSelection();

            this.dgvB2BA.ClearSelection();
            this.dgvB2CLA.ClearSelection();
            this.dgvB2CSA.ClearSelection();
            this.dgvEXPA.ClearSelection();
            this.dgvCDNRA.ClearSelection();
            this.dgvCDNURA.ClearSelection();
            this.dgvATA.ClearSelection();
            this.dgvTXPDA.ClearSelection();
        }

        public void GetData()
        {
            try
            {
                #region First Clear dataGrid rows
                dgvB2B.Rows.Clear();
                dgvB2CL.Rows.Clear();
                dgvB2CS.Rows.Clear();
                dgvZeroRated.Rows.Clear();
                dgvCDN.Rows.Clear();
                CDnUR.Rows.Clear();
                dgvNill.Rows.Clear();
                dgvAR.Rows.Clear();
                dgvAA.Rows.Clear();
                dgvHSN.Rows.Clear();
                dgvDoc.Rows.Clear();

                dgvB2BA.Rows.Clear();
                dgvB2CLA.Rows.Clear();
                dgvB2CSA.Rows.Clear();
                dgvEXPA.Rows.Clear();
                dgvCDNURA.Rows.Clear();
                dgvCDNRA.Rows.Clear();
                dgvATA.Rows.Clear();
                dgvTXPDA.Rows.Clear();
                #endregion

                #region Data From Database

                #region Regular
                #region  B2B
                string Query = "Select * from SPQR1B2B where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count > 1)
                    dgvB2B.Rows.Add("System Summary", dt.Rows[0]["Fld_InvoiceNo"].ToString().Replace("-", "0"), dt.Rows[0]["Fld_InvoiceTaxableVal"].ToString(), dt.Rows[0]["Fld_IGSTAmnt"].ToString(), dt.Rows[0]["Fld_CGSTAmnt"].ToString(), dt.Rows[0]["Fld_SGSTAmnt"].ToString(), dt.Rows[0]["Fld_CessAmount"].ToString());
                else
                    dgvB2B.Rows.Add("System Summary", "0", "0", "0", "0", "0", "0");

                Query = "Select * from SPQGSTNSummary where Fld_SectionName='B2B' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "';";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count == 1)
                    dgvB2B.Rows.Add("GSTN Summary", dt.Rows[0]["Fld_InvoiceNo"].ToString().Trim() == "" ? "0" : dt.Rows[0]["Fld_InvoiceNo"].ToString().Trim(), dt.Rows[0]["Fld_TaxValue"].ToString(), dt.Rows[0]["Fld_IGST"].ToString(), dt.Rows[0]["Fld_CGST"].ToString(), dt.Rows[0]["Fld_SGST"].ToString(), dt.Rows[0]["Fld_CESS"].ToString());
                else
                    dgvB2B.Rows.Add("GSTN Summary", "0", "0", "0", "0", "0", "0");
                #endregion

                #region B2CL
                Query = "Select * from SPQR1B2CL where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count > 1)
                    dgvB2CL.Rows.Add("System Summary", dt.Rows[0]["Fld_InvoiceNo"].ToString().Replace("-", "0"), dt.Rows[0]["Fld_TaxableValue"].ToString(), dt.Rows[0]["Fld_IGST"].ToString(), "0", "0", dt.Rows[0]["Fld_Cess"].ToString());
                else
                    dgvB2CL.Rows.Add("System Summary", "0", "0", "0", "0", "0", "0");

                Query = "Select * from SPQGSTNSummary where Fld_SectionName='B2CL' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "';";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count == 1)
                    dgvB2CL.Rows.Add("GSTN Summary", dt.Rows[0]["Fld_InvoiceNo"].ToString().Trim() == "" ? "0" : dt.Rows[0]["Fld_InvoiceNo"].ToString().Trim(), dt.Rows[0]["Fld_TaxValue"].ToString(), dt.Rows[0]["Fld_IGST"].ToString(), dt.Rows[0]["Fld_CGST"].ToString(), dt.Rows[0]["Fld_SGST"].ToString(), dt.Rows[0]["Fld_CESS"].ToString());
                else
                    dgvB2CL.Rows.Add("GSTN Summary", "0", "0", "0", "0", "0", "0");
                #endregion

                #region B2CS
                Query = "Select * from SPQR1B2CS where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);               

                if (dt != null && dt.Rows.Count > 1)
                {
                    Int32 _CountN = 0;
                    Query = "Select * from SPQR1B2CS where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id;";
                    DataTable dtCount = new DataTable();
                    dtCount = objGSTR5.GetDataGSTR1(Query);
                    if (dtCount != null && dtCount.Rows.Count > 0)
                    {
                        _CountN = dtCount.Rows.Count - 1;
                    }

                    dgvB2CS.Rows.Add("System Summary", _CountN, dt.Rows[0]["Fld_TaxableValue"].ToString(), dt.Rows[0]["Fld_IGST"].ToString(), dt.Rows[0]["Fld_CGST"].ToString(), dt.Rows[0]["Fld_SGST"].ToString(), dt.Rows[0]["Fld_Cess"].ToString());
                    //dgvB2CS.Rows.Add("System Summary", dt.Rows[1]["Fld_Sequence"].ToString(), dt.Rows[0]["Fld_TaxableValue"].ToString(), dt.Rows[0]["Fld_IGST"].ToString(), dt.Rows[0]["Fld_CGST"].ToString(), dt.Rows[0]["Fld_SGST"].ToString(), dt.Rows[0]["Fld_Cess"].ToString());
                }
                else
                    dgvB2CS.Rows.Add("System Summary", "0", "0", "0", "0", "0", "0");

                Query = "Select * from SPQGSTNSummary where Fld_SectionName='B2CS' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "';";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count == 1)
                    dgvB2CS.Rows.Add("GSTN Summary", dt.Rows[0]["Fld_InvoiceNo"].ToString().Trim() == "" ? "0" : dt.Rows[0]["Fld_InvoiceNo"].ToString().Trim(), dt.Rows[0]["Fld_TaxValue"].ToString(), dt.Rows[0]["Fld_IGST"].ToString(), dt.Rows[0]["Fld_CGST"].ToString(), dt.Rows[0]["Fld_SGST"].ToString(), dt.Rows[0]["Fld_CESS"].ToString());
                else
                    dgvB2CS.Rows.Add("GSTN Summary", "0", "0", "0", "0", "0", "0");
                #endregion

                #region Zero rated supplies
                Query = "Select * from SPQR1ZeroRated where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'  order by Fld_Id DESC LIMIT 2;";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count > 0)
                    dgvZeroRated.Rows.Add("System Summary", dt.Rows[0]["Fld_InvoiceNo"].ToString().Replace("-", "0"), dt.Rows[0]["Fld_IGSTInvoiceTaxableVal"].ToString(), dt.Rows[0]["Fld_IGSTAmnt"].ToString(), "0", "0", "0");
                else
                    dgvZeroRated.Rows.Add("System Summary", "0", "0", "0", "0", "0", "0");

                Query = "Select * from SPQGSTNSummary where Fld_SectionName='EXP' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "';";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count == 1)
                    dgvZeroRated.Rows.Add("GSTN Summary", dt.Rows[0]["Fld_InvoiceNo"].ToString().Trim() == "" ? "0" : dt.Rows[0]["Fld_InvoiceNo"].ToString(), dt.Rows[0]["Fld_TaxValue"].ToString(), dt.Rows[0]["Fld_IGST"].ToString(), dt.Rows[0]["Fld_CGST"].ToString(), dt.Rows[0]["Fld_SGST"].ToString(), dt.Rows[0]["Fld_CESS"].ToString());
                else
                    dgvZeroRated.Rows.Add("GSTN Summary", "0", "0", "0", "0", "0", "0");
                #endregion

                #region Credit/Debit Note
                Query = "Select * from SPQR1CDN where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);

                if (dt != null && dt.Rows.Count > 1)
                    dgvCDN.Rows.Add("System Summary", dt.Rows[0]["Fld_DbtCrdtNoteNo"].ToString().Trim().Replace("-", "0") == "" ? "0" : dt.Rows[0]["Fld_DbtCrdtNoteNo"].ToString().Trim().Replace("-", "0"), dt.Rows[0]["Fld_Taxable"].ToString(), dt.Rows[0]["Fld_IGSTAmnt"].ToString(), dt.Rows[0]["Fld_CGSTAmnt"].ToString(), dt.Rows[0]["Fld_SGSTAmnt"].ToString(), dt.Rows[0]["Fld_CessAmnt"].ToString());
                else
                    dgvCDN.Rows.Add("System Summary", "0", "0", "0", "0", "0", "0");

                Query = "Select * from SPQGSTNSummary where Fld_SectionName='CDN' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "';";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count == 1)
                    dgvCDN.Rows.Add("GSTN Summary", dt.Rows[0]["Fld_InvoiceNo"].ToString().Trim() == "" ? "0" : dt.Rows[0]["Fld_InvoiceNo"].ToString(), dt.Rows[0]["Fld_TaxValue"].ToString(), dt.Rows[0]["Fld_IGST"].ToString(), dt.Rows[0]["Fld_CGST"].ToString(), dt.Rows[0]["Fld_SGST"].ToString(), dt.Rows[0]["Fld_CESS"].ToString());
                else
                    dgvCDN.Rows.Add("GSTN Summary", "0", "0", "0", "0", "0", "0");
                #endregion

                #region CDN UR
                Query = "Select * from SPQR1CDNUR where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count > 1)
                    CDnUR.Rows.Add("System Summary", dt.Rows[0]["Fld_OrgInvoiceNo"].ToString().Replace("-", "0"), dt.Rows[0]["Fld_Taxable"].ToString(), dt.Rows[0]["Fld_IGSTAmnt"].ToString(), dt.Rows[0]["Fld_CGSTAmnt"].ToString(), dt.Rows[0]["Fld_SGSTAmnt"].ToString(), dt.Rows[0]["Fld_CessAmnt"].ToString());
                else
                    CDnUR.Rows.Add("System Summary", "0", "0", "0", "0", "0", "0");

                Query = "Select * from SPQGSTNSummary where Fld_SectionName='CDNUR' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "';";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count == 1)
                    CDnUR.Rows.Add("GSTN Summary", dt.Rows[0]["Fld_InvoiceNo"].ToString().Trim() == "" ? "0" : dt.Rows[0]["Fld_InvoiceNo"].ToString().Trim(), dt.Rows[0]["Fld_TaxValue"].ToString(), dt.Rows[0]["Fld_IGST"].ToString(), dt.Rows[0]["Fld_CGST"].ToString(), dt.Rows[0]["Fld_SGST"].ToString(), dt.Rows[0]["Fld_CESS"].ToString());
                else
                    CDnUR.Rows.Add("GSTN Summary", "0", "0", "0", "0", "0", "0");
                #endregion

                #region Nil Rated
                //Query = "Select * from SPQR1NilRatedMulti where Fld_Month='" + CommonHelper.SelectedMonth + "' order by Fld_Id DESC LIMIT 2";
                //dt = new DataTable();
                //dt = objGSTR5.GetDataGSTR1(Query);
                //if (dt != null && dt.Rows.Count > 1)
                //    dgvNill.Rows.Add("System Summary", dt.Rows[0]["Fld_InvoiceNo"].ToString().Replace("-", "0"), dt.Rows[0]["Fld_InvoiceValue"].ToString(), "0", "0", "0", "0");
                //else
                //    dgvNill.Rows.Add("System Summary", "0", "0", "0", "0", "0", "0");

                Query = "Select * from SPQR1NilRated where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                decimal Fld_TaxTurn_1 = 0;
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["Fld_NilRatedSupply"].ToString() != "")
                            Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[i]["Fld_NilRatedSupply"]);
                        if (dt.Rows[i]["Fld_Exempted"].ToString() != "")
                            Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[i]["Fld_Exempted"]);
                        if (dt.Rows[i]["Fld_NonGSTSupplies"].ToString() != "")
                            Fld_TaxTurn_1 = Fld_TaxTurn_1 + Convert.ToDecimal(dt.Rows[i]["Fld_NonGSTSupplies"]);
                    }
                }

                if (Fld_TaxTurn_1 != 0)
                {
                    dgvNill.Rows.Add("System Summary", "1", "0", "0", "0", "0", "0");
                }
                else
                {
                    dgvNill.Rows.Add("System Summary", "0", "0", "0", "0", "0", "0");
                }

                Query = "Select * from SPQGSTNSummary where Fld_SectionName='NIL' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "';";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count == 1)
                    dgvNill.Rows.Add("GSTN Summary", dt.Rows[0]["Fld_InvoiceNo"].ToString().Trim() == "" ? "0" : dt.Rows[0]["Fld_InvoiceNo"].ToString().Trim(), dt.Rows[0]["Fld_TaxValue"].ToString(), dt.Rows[0]["Fld_IGST"].ToString(), dt.Rows[0]["Fld_CGST"].ToString(), dt.Rows[0]["Fld_SGST"].ToString(), dt.Rows[0]["Fld_CESS"].ToString());
                else
                    dgvNill.Rows.Add("GSTN Summary", "0", "0", "0", "0", "0", "0");
                #endregion

                #region Form Gross Advance
                Query = "Select * from SPQR1GrossAdvance where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count > 1)
                    dgvAR.Rows.Add("System Summary", "0", dt.Rows[0]["Fld_GrossAdvRcv"].ToString(), dt.Rows[0]["Fld_IGSTAmnt"].ToString(), dt.Rows[0]["Fld_CGSTAmnt"].ToString(), dt.Rows[0]["Fld_SGSTAmnt"].ToString(), dt.Rows[0]["Fld_CessAmount"].ToString());
                //dgvAR.Rows.Add("System Summary", dt.Rows[1]["Fld_Sequence"].ToString(), dt.Rows[0]["Fld_GrossAdvRcv"].ToString(), dt.Rows[0]["Fld_IGSTAmnt"].ToString(), dt.Rows[0]["Fld_CGSTAmnt"].ToString(), dt.Rows[0]["Fld_SGSTAmnt"].ToString(), dt.Rows[0]["Fld_CessAmount"].ToString());

                else
                    dgvAR.Rows.Add("System Summary", "0", "0", "0", "0", "0", "0");

                Query = "Select * from SPQGSTNSummary where Fld_SectionName='AR' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "';";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count == 1)
                    dgvAR.Rows.Add("GSTN Summary", dt.Rows[0]["Fld_InvoiceNo"].ToString().Trim() == "" ? "0" : dt.Rows[0]["Fld_InvoiceNo"].ToString().Trim(), dt.Rows[0]["Fld_TaxValue"].ToString(), dt.Rows[0]["Fld_IGST"].ToString(), dt.Rows[0]["Fld_CGST"].ToString(), dt.Rows[0]["Fld_SGST"].ToString(), dt.Rows[0]["Fld_CESS"].ToString());
                else
                    dgvAR.Rows.Add("GSTN Summary", "0", "0", "0", "0", "0", "0");
                #endregion

                #region Form Net Advance
                Query = "Select * from SPQR1NetAdvance where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count > 1)
                    dgvAA.Rows.Add("System Summary", dt.Rows[0]["Fld_InvoiceNo"].ToString().Replace("-", "0"), dt.Rows[0]["Fld_Advadj"].ToString(), dt.Rows[0]["Fld_IGSTAmnt"].ToString(), dt.Rows[0]["Fld_CGSTAmnt"].ToString(), dt.Rows[0]["Fld_SGSTAmnt"].ToString(), dt.Rows[0]["Fld_CessAmount"].ToString());
                //dgvAA.Rows.Add("System Summary", dt.Rows[1]["Fld_Sequence"].ToString(), dt.Rows[0]["Fld_Advadj"].ToString(), dt.Rows[0]["Fld_IGSTAmnt"].ToString(), dt.Rows[0]["Fld_CGSTAmnt"].ToString(), dt.Rows[0]["Fld_SGSTAmnt"].ToString(), dt.Rows[0]["Fld_CessAmount"].ToString());

                else
                    dgvAA.Rows.Add("System Summary", "0", "0", "0", "0", "0", "0", "0");

                Query = "Select * from SPQGSTNSummary where Fld_SectionName='AA' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "';";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count == 1)
                    dgvAA.Rows.Add("GSTN Summary", dt.Rows[0]["Fld_InvoiceNo"].ToString().Trim() == "" ? "0" : dt.Rows[0]["Fld_InvoiceNo"].ToString().Trim(), dt.Rows[0]["Fld_TaxValue"].ToString(), dt.Rows[0]["Fld_IGST"].ToString(), dt.Rows[0]["Fld_CGST"].ToString(), dt.Rows[0]["Fld_SGST"].ToString(), dt.Rows[0]["Fld_CESS"].ToString());
                else
                    dgvAA.Rows.Add("GSTN Summary", "0", "0", "0", "0", "0", "0");
                #endregion

                #region Form HSN Summary
                Query = "Select * from SPQR1HSN where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count > 1)
                {
                    Int32 _Count = 0;
                    string _Query = "Select * from SPQR1HSN where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id;";
                    DataTable dtCount = new DataTable();
                    dtCount = objGSTR5.GetDataGSTR1(_Query);
                    if (dtCount != null && dtCount.Rows.Count > 0)
                    {
                        _Count = dtCount.Rows.Count - 1;
                    }


                    dgvHSN.Rows.Add("System Summary",_Count, dt.Rows[0]["Fld_TotalTaxableValue"].ToString(), dt.Rows[0]["Fld_IGST"].ToString(), dt.Rows[0]["Fld_CGST"].ToString(), dt.Rows[0]["Fld_SGST"].ToString(), dt.Rows[0]["Fld_Cess"].ToString());
                    //dgvHSN.Rows.Add("System Summary", dt.Rows[1]["Fld_Sequence"].ToString(), dt.Rows[0]["Fld_TotalTaxableValue"].ToString(), dt.Rows[0]["Fld_IGST"].ToString(), dt.Rows[0]["Fld_CGST"].ToString(), dt.Rows[0]["Fld_SGST"].ToString(), dt.Rows[0]["Fld_Cess"].ToString());
                }
                else
                    dgvHSN.Rows.Add("System Summary", "0", "0", "0", "0", "0", "0");

                Query = "Select * from SPQGSTNSummary where Fld_SectionName='HSN' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "';";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count == 1)
                    dgvHSN.Rows.Add("GSTN Summary", dt.Rows[0]["Fld_InvoiceNo"].ToString().Trim() == "" ? "0" : dt.Rows[0]["Fld_InvoiceNo"].ToString().Trim(), dt.Rows[0]["Fld_TaxValue"].ToString(), dt.Rows[0]["Fld_IGST"].ToString(), dt.Rows[0]["Fld_CGST"].ToString(), dt.Rows[0]["Fld_SGST"].ToString(), dt.Rows[0]["Fld_CESS"].ToString());
                else
                    dgvHSN.Rows.Add("GSTN Summary", "0", "0", "0", "0", "0", "0");
                #endregion

                #region Form 13 Document
                Query = "Select * from SPQR1Document where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count > 1)
                {
                    Int32 _Count = 0;
                    string _Query = "Select * from SPQR1Document where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id;";
                    DataTable dtCount = new DataTable();
                    dtCount = objGSTR5.GetDataGSTR1(_Query);
                    if (dtCount != null && dtCount.Rows.Count > 0)
                    {
                        _Count = dtCount.Rows.Count - 1;
                    }
                    dgvDoc.Rows.Add("System Summary", _Count, "0", "0", "0", "0", "0");
                }
                else
                    dgvDoc.Rows.Add("System Summary", "0", "0", "0", "0", "0", "0");

                Query = "Select * from SPQGSTNSummary where Fld_SectionName='DOC' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "';";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count == 1)
                    dgvDoc.Rows.Add("GSTN Summary", dt.Rows[0]["Fld_InvoiceNo"].ToString().Trim() == "" ? "0" : dt.Rows[0]["Fld_InvoiceNo"].ToString().Trim(), dt.Rows[0]["Fld_TaxValue"].ToString(), dt.Rows[0]["Fld_IGST"].ToString(), dt.Rows[0]["Fld_CGST"].ToString(), dt.Rows[0]["Fld_SGST"].ToString(), dt.Rows[0]["Fld_CESS"].ToString());
                else
                    dgvDoc.Rows.Add("GSTN Summary", "0", "0", "0", "0", "0", "0");
                #endregion

                #endregion

                #region Amendment
                #region  B2BA
                Query = "Select * from SPQR1AmendB2B where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count > 1)
                    dgvB2BA.Rows.Add("System Summary", dt.Rows[0]["Fld_OrgInvoiceNo"].ToString().Replace("-", "0"), dt.Rows[0]["Fld_TaxVal"].ToString(), dt.Rows[0]["Fld_IGSTAmt"].ToString(), dt.Rows[0]["Fld_CGSTAmt"].ToString(), dt.Rows[0]["Fld_SGSTAmt"].ToString(), dt.Rows[0]["Fld_CESSAmt"].ToString());
                else
                    dgvB2BA.Rows.Add("System Summary", "0", "0", "0", "0", "0", "0");

                Query = "Select * from SPQGSTNSummary where Fld_SectionName='B2BA' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "';";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count == 1)
                    dgvB2BA.Rows.Add("GSTN Summary", dt.Rows[0]["Fld_InvoiceNo"].ToString(), dt.Rows[0]["Fld_TaxValue"].ToString(), dt.Rows[0]["Fld_IGST"].ToString(), dt.Rows[0]["Fld_CGST"].ToString(), dt.Rows[0]["Fld_SGST"].ToString(), dt.Rows[0]["Fld_CESS"].ToString());
                else
                    dgvB2BA.Rows.Add("GSTN Summary", "0", "0", "0", "0", "0", "0");
                #endregion

                #region  B2CLA
                Query = "Select * from SPQR1AmendB2CL where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count > 1)
                    dgvB2CLA.Rows.Add("System Summary", dt.Rows[0]["Fld_OrgInvoiceNo"].ToString().Replace("-", "0"), dt.Rows[0]["Fld_TaxVal"].ToString(), dt.Rows[0]["Fld_IGSTAmt"].ToString(), 0, 0, dt.Rows[0]["Fld_CESSAmt"].ToString());
                else
                    dgvB2CLA.Rows.Add("System Summary", "0", "0", "0", "0", "0", "0");

                Query = "Select * from SPQGSTNSummary where Fld_SectionName='B2CLA' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "';";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count == 1)
                    dgvB2CLA.Rows.Add("GSTN Summary", dt.Rows[0]["Fld_InvoiceNo"].ToString(), dt.Rows[0]["Fld_TaxValue"].ToString(), dt.Rows[0]["Fld_IGST"].ToString(), dt.Rows[0]["Fld_CGST"].ToString(), dt.Rows[0]["Fld_SGST"].ToString(), dt.Rows[0]["Fld_CESS"].ToString());
                else
                    dgvB2CLA.Rows.Add("GSTN Summary", "0", "0", "0", "0", "0", "0");
                #endregion

                #region  B2CSA
                Query = "Select * from SPQR1AmendB2CS where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count > 1)
                    dgvB2CSA.Rows.Add("System Summary", 0, dt.Rows[0]["Fld_TaxVal"].ToString(), dt.Rows[0]["Fld_IGSTAmt"].ToString(), dt.Rows[0]["Fld_CGSTAmt"].ToString(), dt.Rows[0]["Fld_SGSTAmt"].ToString(), dt.Rows[0]["Fld_CESSAmt"].ToString());
                else
                    dgvB2CSA.Rows.Add("System Summary", "0", "0", "0", "0", "0", "0");

                Query = "Select * from SPQGSTNSummary where Fld_SectionName='B2CSA' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "';";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count == 1)
                    dgvB2CSA.Rows.Add("GSTN Summary", dt.Rows[0]["Fld_InvoiceNo"].ToString(), dt.Rows[0]["Fld_TaxValue"].ToString(), dt.Rows[0]["Fld_IGST"].ToString(), dt.Rows[0]["Fld_CGST"].ToString(), dt.Rows[0]["Fld_SGST"].ToString(), dt.Rows[0]["Fld_CESS"].ToString());
                else
                    dgvB2CSA.Rows.Add("GSTN Summary", "0", "0", "0", "0", "0", "0");
                #endregion

                #region  EXPA
                Query = "Select * from SPQR1AmendEXPORT where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count > 1)
                    dgvEXPA.Rows.Add("System Summary", dt.Rows[0]["Fld_OrgSupInvoiceNo"].ToString(), dt.Rows[0]["Fld_TaxableValue"].ToString(), dt.Rows[0]["Fld_IGSTAmt"].ToString(), 0, 0, dt.Rows[0]["Fld_CESSAmt"].ToString());
                else
                    dgvEXPA.Rows.Add("System Summary", "0", "0", "0", "0", "0", "0");

                Query = "Select * from SPQGSTNSummary where Fld_SectionName='EXPA' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "';";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count == 1)
                    dgvEXPA.Rows.Add("GSTN Summary", dt.Rows[0]["Fld_InvoiceNo"].ToString(), dt.Rows[0]["Fld_TaxValue"].ToString(), dt.Rows[0]["Fld_IGST"].ToString(), dt.Rows[0]["Fld_CGST"].ToString(), dt.Rows[0]["Fld_SGST"].ToString(), dt.Rows[0]["Fld_CESS"].ToString());
                else
                    dgvEXPA.Rows.Add("GSTN Summary", "0", "0", "0", "0", "0", "0");
                #endregion

                #region  CDNRA
                Query = "Select * from SPQR1AmendCDNR where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count > 1)
                    dgvCDNRA.Rows.Add("System Summary", dt.Rows[0]["Fld_OrgCDNNo"].ToString().Replace("-", "0"), dt.Rows[0]["Fld_TaxableValue"].ToString(), dt.Rows[0]["Fld_IGSTAmt"].ToString(), dt.Rows[0]["Fld_CGSTAmt"].ToString(), dt.Rows[0]["Fld_SGSTAmt"].ToString(), dt.Rows[0]["Fld_CESSAmt"].ToString());
                else
                    dgvCDNRA.Rows.Add("System Summary", "0", "0", "0", "0", "0", "0");

                Query = "Select * from SPQGSTNSummary where Fld_SectionName='CDNRA' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "';";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count == 1)
                    dgvCDNRA.Rows.Add("GSTN Summary", dt.Rows[0]["Fld_InvoiceNo"].ToString(), dt.Rows[0]["Fld_TaxValue"].ToString(), dt.Rows[0]["Fld_IGST"].ToString(), dt.Rows[0]["Fld_CGST"].ToString(), dt.Rows[0]["Fld_SGST"].ToString(), dt.Rows[0]["Fld_CESS"].ToString());
                else
                    dgvCDNRA.Rows.Add("GSTN Summary", "0", "0", "0", "0", "0", "0");
                #endregion

                #region  CDNURA
                Query = "Select * from SPQR1AmendCDNUR where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count > 1)
                    dgvCDNURA.Rows.Add("System Summary", dt.Rows[0]["Fld_OrgCDNRefVouNo"].ToString().Replace("-", "0"), dt.Rows[0]["Fld_TaxableValue"].ToString(), dt.Rows[0]["Fld_IGSTAmt"].ToString(), 0, 0, dt.Rows[0]["Fld_CESSAmt"].ToString());
                else
                    dgvCDNURA.Rows.Add("System Summary", "0", "0", "0", "0", "0", "0");

                Query = "Select * from SPQGSTNSummary where Fld_SectionName='CDNURA' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "';";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count == 1)
                    dgvCDNURA.Rows.Add("GSTN Summary", dt.Rows[0]["Fld_InvoiceNo"].ToString(), dt.Rows[0]["Fld_TaxValue"].ToString(), dt.Rows[0]["Fld_IGST"].ToString(), dt.Rows[0]["Fld_CGST"].ToString(), dt.Rows[0]["Fld_SGST"].ToString(), dt.Rows[0]["Fld_CESS"].ToString());
                else
                    dgvCDNURA.Rows.Add("GSTN Summary", "0", "0", "0", "0", "0", "0");
                #endregion

                #region  ATA
                Query = "Select * from SPQR1AmendAT where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count > 1)
                    dgvATA.Rows.Add("System Summary", 0, dt.Rows[0]["Fld_AdvReceived"].ToString(), dt.Rows[0]["Fld_IGSTRate"].ToString(), dt.Rows[0]["Fld_CGSTRate"].ToString(), dt.Rows[0]["Fld_SGSTRate"].ToString(), dt.Rows[0]["Fld_CESSRate"].ToString());
                else
                    dgvATA.Rows.Add("System Summary", "0", "0", "0", "0", "0", "0");

                Query = "Select * from SPQGSTNSummary where Fld_SectionName='ATA' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "';";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count == 1)
                    dgvATA.Rows.Add("GSTN Summary", dt.Rows[0]["Fld_InvoiceNo"].ToString(), dt.Rows[0]["Fld_TaxValue"].ToString(), dt.Rows[0]["Fld_IGST"].ToString(), dt.Rows[0]["Fld_CGST"].ToString(), dt.Rows[0]["Fld_SGST"].ToString(), dt.Rows[0]["Fld_CESS"].ToString());
                else
                    dgvATA.Rows.Add("GSTN Summary", "0", "0", "0", "0", "0", "0");
                #endregion

                #region  TXPA
                Query = "Select * from SPQR1AmendTXP where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' order by Fld_Id DESC LIMIT 2;";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count > 1)
                    dgvTXPDA.Rows.Add("System Summary", 0, dt.Rows[0]["Fld_AdvToAdjusted"].ToString(), dt.Rows[0]["Fld_IGSTAmt"].ToString(), dt.Rows[0]["Fld_CGSTAmt"].ToString(), dt.Rows[0]["Fld_SGSTAmt"].ToString(), dt.Rows[0]["Fld_CESSAmt"].ToString());
                else
                    dgvTXPDA.Rows.Add("System Summary", "0", "0", "0", "0", "0", "0");

                Query = "Select * from SPQGSTNSummary where Fld_SectionName='TXPA' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "';";
                dt = new DataTable();
                dt = objGSTR5.GetDataGSTR1(Query);
                if (dt != null && dt.Rows.Count == 1)
                    dgvTXPDA.Rows.Add("GSTN Summary", dt.Rows[0]["Fld_InvoiceNo"].ToString(), dt.Rows[0]["Fld_TaxValue"].ToString(), dt.Rows[0]["Fld_IGST"].ToString(), dt.Rows[0]["Fld_CGST"].ToString(), dt.Rows[0]["Fld_SGST"].ToString(), dt.Rows[0]["Fld_CESS"].ToString());
                else
                    dgvTXPDA.Rows.Add("GSTN Summary", "0", "0", "0", "0", "0", "0");
                #endregion
                #endregion

                #endregion

                #region Different

                #region Regular
                //B2B
                dgvB2B.Rows.Add("Difference", (Convert.ToDouble(dgvB2B.Rows[0].Cells["colInvoices"].Value.ToString()) - Convert.ToDouble(dgvB2B.Rows[1].Cells["colInvoices"].Value.ToString())),
                  Convert.ToDouble(dgvB2B.Rows[0].Cells["colTaxValue"].Value.ToString()) - Convert.ToDouble(dgvB2B.Rows[1].Cells["colTaxValue"].Value.ToString()), Convert.ToDouble(dgvB2B.Rows[0].Cells["colIGST"].Value.ToString()) - Convert.ToDouble(dgvB2B.Rows[1].Cells["colIGST"].Value.ToString()), Convert.ToDouble(dgvB2B.Rows[0].Cells["colSGST"].Value.ToString()) - Convert.ToDouble(dgvB2B.Rows[1].Cells["colSGST"].Value.ToString()), Convert.ToDouble(dgvB2B.Rows[0].Cells["colCGST"].Value.ToString()) - Convert.ToDouble(dgvB2B.Rows[1].Cells["colCGST"].Value.ToString()), Convert.ToDouble(dgvB2B.Rows[0].Cells["colCess"].Value.ToString()) - Convert.ToDouble(dgvB2B.Rows[1].Cells["colCess"].Value.ToString()));

                //B2CL
                dgvB2CL.Rows.Add("Difference", (Convert.ToDouble(dgvB2CL.Rows[0].Cells["colInvoicesB2CL"].Value.ToString()) - Convert.ToDouble(dgvB2CL.Rows[1].Cells["colInvoicesB2CL"].Value.ToString())),
                 Convert.ToDouble(dgvB2CL.Rows[0].Cells["colTaxValueB2CL"].Value.ToString()) - Convert.ToDouble(dgvB2CL.Rows[1].Cells["colTaxValueB2CL"].Value.ToString()), Convert.ToDouble(dgvB2CL.Rows[0].Cells["colIGSTB2CL"].Value.ToString()) - Convert.ToDouble(dgvB2CL.Rows[1].Cells["colIGSTB2CL"].Value.ToString()), Convert.ToDouble(dgvB2CL.Rows[0].Cells["colSGSTB2CL"].Value.ToString()) - Convert.ToDouble(dgvB2CL.Rows[1].Cells["colSGSTB2CL"].Value.ToString()), Convert.ToDouble(dgvB2CL.Rows[0].Cells["colCGSTB2CL"].Value.ToString()) - Convert.ToDouble(dgvB2CL.Rows[1].Cells["colCGSTB2CL"].Value.ToString()), Convert.ToDouble(dgvB2CL.Rows[0].Cells["colCessB2CL"].Value.ToString()) - Convert.ToDouble(dgvB2CL.Rows[1].Cells["colCessB2CL"].Value.ToString()));

                //B2CS
                dgvB2CS.Rows.Add("Difference", (Convert.ToDouble(dgvB2CS.Rows[0].Cells["colInvoicesB2CS"].Value.ToString()) - Convert.ToDouble(dgvB2CS.Rows[1].Cells["colInvoicesB2CS"].Value.ToString())),
                 Convert.ToDouble(dgvB2CS.Rows[0].Cells["colTaxValueB2CS"].Value.ToString()) - Convert.ToDouble(dgvB2CS.Rows[1].Cells["colTaxValueB2CS"].Value.ToString()), Convert.ToDouble(dgvB2CS.Rows[0].Cells["colIGSTB2CS"].Value.ToString()) - Convert.ToDouble(dgvB2CS.Rows[1].Cells["colIGSTB2CS"].Value.ToString()), Convert.ToDouble(dgvB2CS.Rows[0].Cells["colSGSTB2CS"].Value.ToString()) - Convert.ToDouble(dgvB2CS.Rows[1].Cells["colSGSTB2CS"].Value.ToString()), Convert.ToDouble(dgvB2CS.Rows[0].Cells["colCGSTB2CS"].Value.ToString()) - Convert.ToDouble(dgvB2CS.Rows[1].Cells["colCGSTB2CS"].Value.ToString()), Convert.ToDouble(dgvB2CS.Rows[0].Cells["colCessB2CS"].Value.ToString()) - Convert.ToDouble(dgvB2CS.Rows[1].Cells["colCessB2CS"].Value.ToString()));

                //ZERO RATED
                dgvZeroRated.Rows.Add("Difference", (Convert.ToDouble(dgvZeroRated.Rows[0].Cells["colInvoicesZ"].Value.ToString()) - Convert.ToDouble(dgvZeroRated.Rows[1].Cells["colInvoicesZ"].Value.ToString())),
               Convert.ToDouble(dgvZeroRated.Rows[0].Cells["colTaxValueZ"].Value.ToString()) - Convert.ToDouble(dgvZeroRated.Rows[1].Cells["colTaxValueZ"].Value.ToString()), Convert.ToDouble(dgvZeroRated.Rows[0].Cells["colIGSTZ"].Value.ToString()) - Convert.ToDouble(dgvZeroRated.Rows[1].Cells["colIGSTZ"].Value.ToString()),
               Convert.ToDouble(dgvZeroRated.Rows[0].Cells["colCGSTZ"].Value.ToString()) - Convert.ToDouble(dgvZeroRated.Rows[1].Cells["colCGSTZ"].Value.ToString()),
               Convert.ToDouble(dgvZeroRated.Rows[0].Cells["colSGSTZ"].Value.ToString()) - Convert.ToDouble(dgvZeroRated.Rows[1].Cells["colSGSTZ"].Value.ToString()), Convert.ToDouble(dgvZeroRated.Rows[0].Cells["colCessZ"].Value.ToString()) - Convert.ToDouble(dgvZeroRated.Rows[1].Cells["colCessZ"].Value.ToString()));

                //CDN
                dgvCDN.Rows.Add("Difference", (Convert.ToDouble(dgvCDN.Rows[0].Cells["colInvoicesCD"].Value.ToString()) - Convert.ToDouble(dgvCDN.Rows[1].Cells["colInvoicesCD"].Value.ToString())),
                 Convert.ToDouble(dgvCDN.Rows[0].Cells["colTaxValueCD"].Value.ToString()) - Convert.ToDouble(dgvCDN.Rows[1].Cells["colTaxValueCD"].Value.ToString()), Convert.ToDouble(dgvCDN.Rows[0].Cells["colIGSTCD"].Value.ToString()) - Convert.ToDouble(dgvCDN.Rows[1].Cells["colIGSTCD"].Value.ToString()), Convert.ToDouble(dgvCDN.Rows[0].Cells["colSGSTCD"].Value.ToString()) - Convert.ToDouble(dgvCDN.Rows[1].Cells["colSGSTCD"].Value.ToString()), Convert.ToDouble(dgvCDN.Rows[0].Cells["colCGSTCD"].Value.ToString()) - Convert.ToDouble(dgvCDN.Rows[1].Cells["colCGSTCD"].Value.ToString()), Convert.ToDouble(dgvCDN.Rows[0].Cells["colCessCD"].Value.ToString()) - Convert.ToDouble(dgvCDN.Rows[1].Cells["colCessCD"].Value.ToString()));


                // CDNUR
                CDnUR.Rows.Add("Difference", (Convert.ToDouble(CDnUR.Rows[0].Cells["colInvoicesCDNUR"].Value.ToString()) - Convert.ToDouble(CDnUR.Rows[1].Cells["colInvoicesCDNUR"].Value.ToString())),
                Convert.ToDouble(CDnUR.Rows[0].Cells["colTaxValueCDNUR"].Value.ToString()) - Convert.ToDouble(CDnUR.Rows[1].Cells["colTaxValueCDNUR"].Value.ToString()), Convert.ToDouble(CDnUR.Rows[0].Cells["colIGSTCDNUR"].Value.ToString()) - Convert.ToDouble(CDnUR.Rows[1].Cells["colIGSTCDNUR"].Value.ToString()), Convert.ToDouble(CDnUR.Rows[0].Cells["colSGSTCDNUR"].Value.ToString()) - Convert.ToDouble(CDnUR.Rows[1].Cells["colSGSTCDNUR"].Value.ToString()), Convert.ToDouble(CDnUR.Rows[0].Cells["colCGSTCDNUR"].Value.ToString()) - Convert.ToDouble(CDnUR.Rows[1].Cells["colCGSTCDNUR"].Value.ToString()), Convert.ToDouble(CDnUR.Rows[0].Cells["colCessCDNUR"].Value.ToString()) - Convert.ToDouble(CDnUR.Rows[1].Cells["colCessCDNUR"].Value.ToString()));

                //NILL
                dgvNill.Rows.Add("Difference", (Convert.ToDouble(dgvNill.Rows[0].Cells["colInvoicesNIL"].Value.ToString()) - Convert.ToDouble(dgvNill.Rows[1].Cells["colInvoicesNIL"].Value.ToString())),
                Convert.ToDouble(dgvNill.Rows[0].Cells["colTaxValueNIL"].Value.ToString()) - Convert.ToDouble(dgvNill.Rows[1].Cells["colTaxValueNIL"].Value.ToString()), Convert.ToDouble(dgvNill.Rows[0].Cells["colIGSTNIL"].Value.ToString()) - Convert.ToDouble(dgvNill.Rows[1].Cells["colIGSTNIL"].Value.ToString()), Convert.ToDouble(dgvNill.Rows[0].Cells["colSGSTNIL"].Value.ToString()) - Convert.ToDouble(dgvNill.Rows[1].Cells["colSGSTNIL"].Value.ToString()), Convert.ToDouble(dgvNill.Rows[0].Cells["colCGSTNIL"].Value.ToString()) - Convert.ToDouble(dgvNill.Rows[1].Cells["colCGSTNIL"].Value.ToString()), Convert.ToDouble(dgvNill.Rows[0].Cells["colCessNIL"].Value.ToString()) - Convert.ToDouble(dgvNill.Rows[1].Cells["colCessNIL"].Value.ToString()));

                //AR
                dgvAR.Rows.Add("Difference", (Convert.ToString(dgvAR.Rows[0].Cells["colInvoicesAR"].Value).Trim() == "" ? 0 : Convert.ToDouble(dgvAR.Rows[0].Cells["colInvoicesAR"].Value) - Convert.ToDouble(dgvAR.Rows[1].Cells["colInvoicesAR"].Value.ToString())),
                    Convert.ToString(dgvAR.Rows[0].Cells["colTaxValueAR"].Value).Trim() == "" ? 0 : Convert.ToDouble(dgvAR.Rows[0].Cells["colTaxValueAR"].Value) - Convert.ToDouble(dgvAR.Rows[1].Cells["colTaxValueAR"].Value.ToString()), Convert.ToDouble(dgvAR.Rows[0].Cells["colIGSTAR"].Value.ToString()) - Convert.ToDouble(dgvAR.Rows[1].Cells["colIGSTAR"].Value.ToString()), Convert.ToDouble(dgvAR.Rows[0].Cells["colSGSTAR"].Value.ToString()) - Convert.ToDouble(dgvAR.Rows[1].Cells["colSGSTAR"].Value.ToString()), Convert.ToDouble(dgvAR.Rows[0].Cells["colCGSTAR"].Value.ToString()) - Convert.ToDouble(dgvAR.Rows[1].Cells["colCGSTAR"].Value.ToString()), Convert.ToDouble(dgvAR.Rows[0].Cells["colCessAR"].Value.ToString()) - Convert.ToDouble(dgvAR.Rows[1].Cells["colCessAR"].Value.ToString()));

                //AA
                dgvAA.Rows.Add("Difference", (Convert.ToString(dgvAA.Rows[0].Cells["colInvoicesAA"].Value).Trim() == "" ? 0 : Convert.ToDouble(Convert.ToString(dgvAA.Rows[0].Cells["colInvoicesAA"].Value).Trim()) - Convert.ToDouble(dgvAA.Rows[1].Cells["colInvoicesAA"].Value.ToString())),
                    Convert.ToString(dgvAA.Rows[0].Cells["colTaxValueAA"].Value).Trim() == "" ? 0 : Convert.ToDouble(dgvAA.Rows[0].Cells["colTaxValueAA"].Value) - Convert.ToDouble(dgvAA.Rows[1].Cells["colTaxValueAA"].Value.ToString()), Convert.ToDouble(dgvAA.Rows[0].Cells["colIGSTAA"].Value.ToString()) - Convert.ToDouble(dgvAA.Rows[1].Cells["colIGSTAA"].Value.ToString()), Convert.ToDouble(dgvAA.Rows[0].Cells["colSGSTAA"].Value.ToString()) - Convert.ToDouble(dgvAA.Rows[1].Cells["colSGSTAA"].Value.ToString()), Convert.ToDouble(dgvAA.Rows[0].Cells["colCGSTAA"].Value.ToString()) - Convert.ToDouble(dgvAA.Rows[1].Cells["colCGSTAA"].Value.ToString()), Convert.ToDouble(dgvAA.Rows[0].Cells["colCessAA"].Value.ToString()) - Convert.ToDouble(dgvAA.Rows[1].Cells["colCessAA"].Value.ToString()));

                //HSN
                dgvHSN.Rows.Add("Difference", (Convert.ToDouble(dgvHSN.Rows[0].Cells["colInvoicesHSN"].Value.ToString()) - Convert.ToDouble(dgvHSN.Rows[1].Cells["colInvoicesHSN"].Value.ToString())),
                  Convert.ToDouble(dgvHSN.Rows[0].Cells["colTaxValueHSN"].Value.ToString()) - Convert.ToDouble(dgvHSN.Rows[1].Cells["colTaxValueHSN"].Value.ToString()), Convert.ToDouble(dgvHSN.Rows[0].Cells["colIGSTHSN"].Value.ToString()) - Convert.ToDouble(dgvHSN.Rows[1].Cells["colIGSTHSN"].Value.ToString()), Convert.ToDouble(dgvHSN.Rows[0].Cells["colSGSTHSN"].Value.ToString()) - Convert.ToDouble(dgvHSN.Rows[1].Cells["colSGSTHSN"].Value.ToString()), Convert.ToDouble(dgvHSN.Rows[0].Cells["colCGSTHSN"].Value.ToString()) - Convert.ToDouble(dgvHSN.Rows[1].Cells["colCGSTHSN"].Value.ToString()), Convert.ToDouble(dgvHSN.Rows[0].Cells["colCessHSN"].Value.ToString()) - Convert.ToDouble(dgvHSN.Rows[1].Cells["colCessHSN"].Value.ToString()));


                //DOC
                dgvDoc.Rows.Add("Difference", (Convert.ToDouble(dgvDoc.Rows[0].Cells["colInvoicesDOC"].Value.ToString()) - Convert.ToDouble(dgvDoc.Rows[1].Cells["colInvoicesDOC"].Value.ToString())), "0", "0", "0", "0", "0");

                #endregion

                #region Amendment

                //B2BA
                dgvB2BA.Rows.Add("Difference", (Convert.ToDouble(dgvB2BA.Rows[0].Cells["colInvoicesB2BA"].Value.ToString()) - Convert.ToDouble(dgvB2BA.Rows[1].Cells["colInvoicesB2BA"].Value.ToString())),
                  Convert.ToDouble(dgvB2BA.Rows[0].Cells["colTaxValueB2BA"].Value.ToString()) - Convert.ToDouble(dgvB2BA.Rows[1].Cells["colTaxValueB2BA"].Value.ToString()), Convert.ToDouble(dgvB2BA.Rows[0].Cells["colIGSTB2BA"].Value.ToString()) - Convert.ToDouble(dgvB2BA.Rows[1].Cells["colIGSTB2BA"].Value.ToString()), Convert.ToDouble(dgvB2BA.Rows[0].Cells["colSGSTB2BA"].Value.ToString()) - Convert.ToDouble(dgvB2BA.Rows[1].Cells["colSGSTB2BA"].Value.ToString()), Convert.ToDouble(dgvB2BA.Rows[0].Cells["colCGSTB2BA"].Value.ToString()) - Convert.ToDouble(dgvB2BA.Rows[1].Cells["colCGSTB2BA"].Value.ToString()), Convert.ToDouble(dgvB2BA.Rows[0].Cells["colCessB2BA"].Value.ToString()) - Convert.ToDouble(dgvB2BA.Rows[1].Cells["colCessB2BA"].Value.ToString()));

                //B2CLA
                dgvB2CLA.Rows.Add("Difference", (Convert.ToDouble(dgvB2CLA.Rows[0].Cells["colInvoicesB2CLA"].Value.ToString()) - Convert.ToDouble(dgvB2CLA.Rows[1].Cells["colInvoicesB2CLA"].Value.ToString())),
                 Convert.ToDouble(dgvB2CLA.Rows[0].Cells["colTaxValueB2CLA"].Value.ToString()) - Convert.ToDouble(dgvB2CLA.Rows[1].Cells["colTaxValueB2CLA"].Value.ToString()), Convert.ToDouble(dgvB2CLA.Rows[0].Cells["colIGSTB2CLA"].Value.ToString()) - Convert.ToDouble(dgvB2CLA.Rows[1].Cells["colIGSTB2CLA"].Value.ToString()), Convert.ToDouble(dgvB2CLA.Rows[0].Cells["colSGSTB2CLA"].Value.ToString()) - Convert.ToDouble(dgvB2CLA.Rows[1].Cells["colSGSTB2CLA"].Value.ToString()), Convert.ToDouble(dgvB2CLA.Rows[0].Cells["colCGSTB2CLA"].Value.ToString()) - Convert.ToDouble(dgvB2CLA.Rows[1].Cells["colCGSTB2CLA"].Value.ToString()), Convert.ToDouble(dgvB2CLA.Rows[0].Cells["colCessB2CLA"].Value.ToString()) - Convert.ToDouble(dgvB2CLA.Rows[1].Cells["colCessB2CLA"].Value.ToString()));

                //B2CSA
                dgvB2CSA.Rows.Add("Difference", (Convert.ToDouble(dgvB2CSA.Rows[0].Cells["colInvoicesB2CSA"].Value.ToString()) - Convert.ToDouble(dgvB2CSA.Rows[1].Cells["colInvoicesB2CSA"].Value.ToString())),
                 Convert.ToDouble(dgvB2CSA.Rows[0].Cells["colTaxValueB2CSA"].Value.ToString()) - Convert.ToDouble(dgvB2CSA.Rows[1].Cells["colTaxValueB2CSA"].Value.ToString()), Convert.ToDouble(dgvB2CSA.Rows[0].Cells["colIGSTB2CSA"].Value.ToString()) - Convert.ToDouble(dgvB2CSA.Rows[1].Cells["colIGSTB2CSA"].Value.ToString()), Convert.ToDouble(dgvB2CSA.Rows[0].Cells["colSGSTB2CSA"].Value.ToString()) - Convert.ToDouble(dgvB2CSA.Rows[1].Cells["colSGSTB2CSA"].Value.ToString()), Convert.ToDouble(dgvB2CSA.Rows[0].Cells["colCGSTB2CSA"].Value.ToString()) - Convert.ToDouble(dgvB2CSA.Rows[1].Cells["colCGSTB2CSA"].Value.ToString()), Convert.ToDouble(dgvB2CSA.Rows[0].Cells["colCessB2CSA"].Value.ToString()) - Convert.ToDouble(dgvB2CSA.Rows[1].Cells["colCessB2CSA"].Value.ToString()));

                //EXPA
                dgvEXPA.Rows.Add("Difference", (Convert.ToDouble(dgvEXPA.Rows[0].Cells["colInvoicesEXPA"].Value.ToString()) - Convert.ToDouble(dgvEXPA.Rows[1].Cells["colInvoicesEXPA"].Value.ToString())),
               Convert.ToDouble(dgvEXPA.Rows[0].Cells["colTaxValueEXPA"].Value.ToString()) - Convert.ToDouble(dgvEXPA.Rows[1].Cells["colTaxValueEXPA"].Value.ToString()), Convert.ToDouble(dgvEXPA.Rows[0].Cells["colIGSTEXPA"].Value.ToString()) - Convert.ToDouble(dgvEXPA.Rows[1].Cells["colIGSTEXPA"].Value.ToString()),
               Convert.ToDouble(dgvEXPA.Rows[0].Cells["colSGSTEXPA"].Value.ToString()) - Convert.ToDouble(dgvEXPA.Rows[1].Cells["colSGSTEXPA"].Value.ToString()),
               Convert.ToDouble(dgvEXPA.Rows[0].Cells["colCGSTEXPA"].Value.ToString()) - Convert.ToDouble(dgvEXPA.Rows[1].Cells["colCGSTEXPA"].Value.ToString()), Convert.ToDouble(dgvEXPA.Rows[0].Cells["colCessEXPA"].Value.ToString()) - Convert.ToDouble(dgvEXPA.Rows[1].Cells["colCessEXPA"].Value.ToString()));

                //CDNA
                dgvCDNRA.Rows.Add("Difference", (Convert.ToDouble(dgvCDNRA.Rows[0].Cells["colInvoicesCDNRA"].Value.ToString()) - Convert.ToDouble(dgvCDNRA.Rows[1].Cells["colInvoicesCDNRA"].Value.ToString())),
                 Convert.ToDouble(dgvCDNRA.Rows[0].Cells["colTaxValueCDNRA"].Value.ToString()) - Convert.ToDouble(dgvCDNRA.Rows[1].Cells["colTaxValueCDNRA"].Value.ToString()), Convert.ToDouble(dgvCDNRA.Rows[0].Cells["colIGSTCDNRA"].Value.ToString()) - Convert.ToDouble(dgvCDNRA.Rows[1].Cells["colIGSTCDNRA"].Value.ToString()), Convert.ToDouble(dgvCDNRA.Rows[0].Cells["colSGSTCDNRA"].Value.ToString()) - Convert.ToDouble(dgvCDNRA.Rows[1].Cells["colSGSTCDNRA"].Value.ToString()), Convert.ToDouble(dgvCDNRA.Rows[0].Cells["colCGSTCDNRA"].Value.ToString()) - Convert.ToDouble(dgvCDNRA.Rows[1].Cells["colCGSTCDNRA"].Value.ToString()), Convert.ToDouble(dgvCDNRA.Rows[0].Cells["colCessCDNRA"].Value.ToString()) - Convert.ToDouble(dgvCDNRA.Rows[1].Cells["colCessCDNRA"].Value.ToString()));


                // CDNURA
                dgvCDNURA.Rows.Add("Difference", (Convert.ToDouble(dgvCDNURA.Rows[0].Cells["colInvoicesCDNURA"].Value.ToString()) - Convert.ToDouble(dgvCDNURA.Rows[1].Cells["colInvoicesCDNURA"].Value.ToString())),
                Convert.ToDouble(dgvCDNURA.Rows[0].Cells["colTaxValueCDNURA"].Value.ToString()) - Convert.ToDouble(dgvCDNURA.Rows[1].Cells["colTaxValueCDNURA"].Value.ToString()), Convert.ToDouble(dgvCDNURA.Rows[0].Cells["colIGSTCDNURA"].Value.ToString()) - Convert.ToDouble(dgvCDNURA.Rows[1].Cells["colIGSTCDNURA"].Value.ToString()), Convert.ToDouble(dgvCDNURA.Rows[0].Cells["colSGSTCDNURA"].Value.ToString()) - Convert.ToDouble(dgvCDNURA.Rows[1].Cells["colSGSTCDNURA"].Value.ToString()), Convert.ToDouble(dgvCDNURA.Rows[0].Cells["colCGSTCDNURA"].Value.ToString()) - Convert.ToDouble(dgvCDNURA.Rows[1].Cells["colCGSTCDNURA"].Value.ToString()), Convert.ToDouble(dgvCDNURA.Rows[0].Cells["colCessCDNURA"].Value.ToString()) - Convert.ToDouble(dgvCDNURA.Rows[1].Cells["colCessCDNURA"].Value.ToString()));

                //ATA
                dgvATA.Rows.Add("Difference", (Convert.ToString(dgvATA.Rows[0].Cells["colInvoicesATA"].Value).Trim() == "" ? 0 : Convert.ToDouble(dgvATA.Rows[0].Cells["colInvoicesATA"].Value) - Convert.ToDouble(dgvATA.Rows[1].Cells["colInvoicesATA"].Value.ToString())),
                    Convert.ToString(dgvATA.Rows[0].Cells["colTaxValueATA"].Value).Trim() == "" ? 0 : Convert.ToDouble(dgvATA.Rows[0].Cells["colTaxValueATA"].Value) - Convert.ToDouble(dgvATA.Rows[1].Cells["colTaxValueATA"].Value.ToString()), Convert.ToDouble(dgvATA.Rows[0].Cells["colIGSTATA"].Value.ToString()) - Convert.ToDouble(dgvATA.Rows[1].Cells["colIGSTATA"].Value.ToString()), Convert.ToDouble(dgvATA.Rows[0].Cells["colSGSTATA"].Value.ToString()) - Convert.ToDouble(dgvATA.Rows[1].Cells["colSGSTATA"].Value.ToString()), Convert.ToDouble(dgvATA.Rows[0].Cells["colCGSTATA"].Value.ToString()) - Convert.ToDouble(dgvATA.Rows[1].Cells["colCGSTATA"].Value.ToString()), Convert.ToDouble(dgvATA.Rows[0].Cells["colCessATA"].Value.ToString()) - Convert.ToDouble(dgvATA.Rows[1].Cells["colCessATA"].Value.ToString()));

                //TXPDA
                dgvTXPDA.Rows.Add("Difference", (Convert.ToString(dgvTXPDA.Rows[0].Cells["colInvoicesTXPDA"].Value).Trim() == "" ? 0 : Convert.ToDouble(Convert.ToString(dgvTXPDA.Rows[0].Cells["colInvoicesTXPDA"].Value).Trim()) - Convert.ToDouble(dgvTXPDA.Rows[1].Cells["colInvoicesTXPDA"].Value.ToString())),
                    Convert.ToString(dgvTXPDA.Rows[0].Cells["colTaxValueTXPDA"].Value).Trim() == "" ? 0 : Convert.ToDouble(dgvTXPDA.Rows[0].Cells["colTaxValueTXPDA"].Value) - Convert.ToDouble(dgvTXPDA.Rows[1].Cells["colTaxValueTXPDA"].Value.ToString()), Convert.ToDouble(dgvTXPDA.Rows[0].Cells["colIGSTTXPDA"].Value.ToString()) - Convert.ToDouble(dgvTXPDA.Rows[1].Cells["colIGSTTXPDA"].Value.ToString()), Convert.ToDouble(dgvTXPDA.Rows[0].Cells["colSGSTTXPDA"].Value.ToString()) - Convert.ToDouble(dgvTXPDA.Rows[1].Cells["colSGSTTXPDA"].Value.ToString()), Convert.ToDouble(dgvTXPDA.Rows[0].Cells["colCGSTTXPDA"].Value.ToString()) - Convert.ToDouble(dgvTXPDA.Rows[1].Cells["colCGSTTXPDA"].Value.ToString()), Convert.ToDouble(dgvTXPDA.Rows[0].Cells["colCessTXPDA"].Value.ToString()) - Convert.ToDouble(dgvTXPDA.Rows[1].Cells["colCessTXPDA"].Value.ToString()));
                #endregion

                #endregion

                #region Clear Selection
                dgvB2B.ClearSelection();
                dgvB2CL.ClearSelection();
                dgvB2CS.ClearSelection();
                dgvZeroRated.ClearSelection();
                dgvCDN.ClearSelection();
                CDnUR.ClearSelection();
                dgvNill.ClearSelection();
                dgvAR.ClearSelection();
                dgvAA.ClearSelection();
                dgvHSN.ClearSelection();
                dgvDoc.ClearSelection();

                dgvB2BA.ClearSelection();
                dgvB2CLA.ClearSelection();
                dgvB2CSA.ClearSelection();
                dgvEXPA.ClearSelection();
                dgvCDNRA.ClearSelection();
                dgvCDNURA.ClearSelection();
                dgvATA.ClearSelection();
                dgvTXPDA.ClearSelection();
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
        public void GetJSONDataOld(string jsonString)
        {
            try
            {
                #region Datatable
                //DataTable dt = new DataTable();
                //foreach (DataColumn col in dgvB2B.Columns)
                //{
                //    dt.Columns.Add(col.ColumnName);
                //}
                #endregion


                #region Download Json Data
                RootObjectSummary obj = JsonConvert.DeserializeObject<RootObjectSummary>(jsonString);
                if (obj != null)
                {
                    for (int i = 0; i < obj.data.sec_sum.Count; i++)
                    {
                        if (obj.data.sec_sum[i].sec_nm == "B2B")
                        {
                            dgvB2B.Rows[1].Cells["colIGST"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_igst);
                            dgvB2B.Rows[1].Cells["colCGST"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_cgst);
                            dgvB2B.Rows[1].Cells["colCess"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_cess);
                            dgvB2B.Rows[1].Cells["colTaxValue"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_val);
                            dgvB2B.Rows[1].Cells["colInvoices"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_rec);
                            dgvB2B.Rows[1].Cells["colSGST"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_sgst);
                        }
                        else if (obj.data.sec_sum[i].sec_nm == "B2CL")
                        {
                            dgvB2CL.Rows[1].Cells["colIGSTB2CL"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_igst);
                            dgvB2CL.Rows[1].Cells["colCGSTB2CL"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_cgst);
                            dgvB2CL.Rows[1].Cells["colCessB2CL"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_cess);
                            dgvB2CL.Rows[1].Cells["colTaxValueB2CL"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_val);
                            dgvB2CL.Rows[1].Cells["colInvoicesB2CL"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_rec);
                            dgvB2CL.Rows[1].Cells["colSGSTB2CL"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_sgst);
                        }
                        else if (obj.data.sec_sum[i].sec_nm == "EXP")
                        {
                            dgvZeroRated.Rows[1].Cells["colIGSTZ"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_igst);
                            // dgvZeroRated.Rows[1].Cells["colCGSTZ"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_cgst);
                            dgvZeroRated.Rows[1].Cells["colCessZ"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_cess);
                            dgvZeroRated.Rows[1].Cells["colTaxValueZ"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_val);
                            dgvZeroRated.Rows[1].Cells["colInvoicesZ"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_rec);
                            // dgvZeroRated.Rows[1].Cells["colSGSTZ"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_sgst);
                        }
                        else if (obj.data.sec_sum[i].sec_nm == "CDNR")
                        {
                            dgvCDN.Rows[1].Cells["colIGSTCD"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_igst);
                            dgvCDN.Rows[1].Cells["colCGSTCD"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_cgst);
                            dgvCDN.Rows[1].Cells["colCessCD"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_cess);
                            dgvCDN.Rows[1].Cells["colTaxValueCD"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_val);
                            dgvCDN.Rows[1].Cells["colInvoicesCD"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_rec);
                            dgvCDN.Rows[1].Cells["colSGSTCD"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_sgst);
                        }
                        else if (obj.data.sec_sum[i].sec_nm == "CDNUR")
                        {
                            CDnUR.Rows[1].Cells["colIGSTCDNUR"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_igst);
                            CDnUR.Rows[1].Cells["colCGSTCDNUR"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_cgst);
                            CDnUR.Rows[1].Cells["colCessCDNUR"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_cess);
                            CDnUR.Rows[1].Cells["colTaxValueCDNUR"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_val);
                            CDnUR.Rows[1].Cells["colInvoicesCDNUR"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_rec);
                            CDnUR.Rows[1].Cells["colSGSTCDNUR"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_sgst);
                        }
                        else if (obj.data.sec_sum[i].sec_nm == "NIL")
                        {
                            dgvNill.Rows[1].Cells["colIGSTNIL"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_igst);
                            dgvNill.Rows[1].Cells["colCGSTNIL"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_cgst);
                            dgvNill.Rows[1].Cells["colCessNIL"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_cess);
                            dgvNill.Rows[1].Cells["colTaxValueNIL"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_val);
                            dgvNill.Rows[1].Cells["colInvoicesNIL"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_rec);
                            dgvNill.Rows[1].Cells["colSGSTNIL"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_sgst);
                        }
                        else if (obj.data.sec_sum[i].sec_nm == "AT")
                        {
                            dgvAR.Rows[1].Cells["colIGSTAR"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_igst);
                            dgvAR.Rows[1].Cells["colCGSTAR"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_cgst);
                            dgvAR.Rows[1].Cells["colCessAR"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_cess);
                            dgvAR.Rows[1].Cells["colTaxValueAR"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_val);
                            dgvAR.Rows[1].Cells["colInvoicesAR"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_rec);
                            dgvAR.Rows[1].Cells["colSGSTAR"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_sgst);
                        }
                        else if (obj.data.sec_sum[i].sec_nm == "TXPD")
                        {
                            dgvAA.Rows[1].Cells["colIGSTAA"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_igst);
                            dgvAA.Rows[1].Cells["colCGSTAA"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_cgst);
                            dgvAA.Rows[1].Cells["colCessAA"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_cess);
                            dgvAA.Rows[1].Cells["colTaxValueAA"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_val);
                            dgvAA.Rows[1].Cells["colInvoicesAA"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_rec);
                            dgvAA.Rows[1].Cells["colSGSTAA"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_sgst);
                        }
                        else if (obj.data.sec_sum[i].sec_nm == "HSN")
                        {
                            dgvHSN.Rows[1].Cells["colIGSTHSN"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_igst);
                            dgvHSN.Rows[1].Cells["colCGSTHSN"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_cgst);
                            dgvHSN.Rows[1].Cells["colCessHSN"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_cess);
                            dgvHSN.Rows[1].Cells["colTaxValueHSN"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_val);
                            dgvHSN.Rows[1].Cells["colInvoicesHSN"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_rec);
                            dgvHSN.Rows[1].Cells["colSGSTHSN"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_sgst);
                        }
                        else if (obj.data.sec_sum[i].sec_nm == "DOC_ISSUE")
                        {
                            dgvDoc.Rows[1].Cells["colInvoicesDOC"].Value = Convert.ToString(obj.data.sec_sum[i].ttl_rec);
                        }
                    }
                }
                #endregion

                #region Different

                //B2B
                dgvB2B.Rows.RemoveAt(dgvB2B.Rows.Count - 1);
                dgvB2CL.Rows.RemoveAt(dgvB2CL.Rows.Count - 1);
                dgvZeroRated.Rows.RemoveAt(dgvZeroRated.Rows.Count - 1);
                dgvCDN.Rows.RemoveAt(dgvCDN.Rows.Count - 1);
                CDnUR.Rows.RemoveAt(CDnUR.Rows.Count - 1);
                dgvNill.Rows.RemoveAt(dgvNill.Rows.Count - 1);
                dgvAR.Rows.RemoveAt(dgvAR.Rows.Count - 1);
                dgvAA.Rows.RemoveAt(dgvAA.Rows.Count - 1);
                dgvHSN.Rows.RemoveAt(dgvHSN.Rows.Count - 1);
                dgvDoc.Rows.RemoveAt(dgvDoc.Rows.Count - 1);

                dgvB2B.Rows.Add("Difference", (Convert.ToDouble(dgvB2B.Rows[0].Cells["colInvoices"].Value.ToString()) - Convert.ToDouble(dgvB2B.Rows[1].Cells["colInvoices"].Value.ToString())),
                   Convert.ToDouble(dgvB2B.Rows[0].Cells["colTaxValue"].Value.ToString()) - Convert.ToDouble(dgvB2B.Rows[1].Cells["colTaxValue"].Value.ToString()), Convert.ToDouble(dgvB2B.Rows[0].Cells["colIGST"].Value.ToString()) - Convert.ToDouble(dgvB2B.Rows[1].Cells["colIGST"].Value.ToString()), Convert.ToDouble(dgvB2B.Rows[0].Cells["colSGST"].Value.ToString()) - Convert.ToDouble(dgvB2B.Rows[1].Cells["colSGST"].Value.ToString()), Convert.ToDouble(dgvB2B.Rows[0].Cells["colCGST"].Value.ToString()) - Convert.ToDouble(dgvB2B.Rows[1].Cells["colCGST"].Value.ToString()), Convert.ToDouble(dgvB2B.Rows[0].Cells["colCess"].Value.ToString()) - Convert.ToDouble(dgvB2B.Rows[1].Cells["colCess"].Value.ToString()));

                //B2CL
                dgvB2CL.Rows.Add("Difference", (Convert.ToDouble(dgvB2CL.Rows[0].Cells["colInvoicesB2CL"].Value.ToString()) - Convert.ToDouble(dgvB2CL.Rows[1].Cells["colInvoicesB2CL"].Value.ToString())),
                 Convert.ToDouble(dgvB2CL.Rows[0].Cells["colTaxValueB2CL"].Value.ToString()) - Convert.ToDouble(dgvB2CL.Rows[1].Cells["colTaxValueB2CL"].Value.ToString()), Convert.ToDouble(dgvB2CL.Rows[0].Cells["colIGSTB2CL"].Value.ToString()) - Convert.ToDouble(dgvB2CL.Rows[1].Cells["colIGSTB2CL"].Value.ToString()), Convert.ToDouble(dgvB2CL.Rows[0].Cells["colSGSTB2CL"].Value.ToString()) - Convert.ToDouble(dgvB2CL.Rows[1].Cells["colSGSTB2CL"].Value.ToString()), Convert.ToDouble(dgvB2CL.Rows[0].Cells["colCGSTB2CL"].Value.ToString()) - Convert.ToDouble(dgvB2CL.Rows[1].Cells["colCGSTB2CL"].Value.ToString()), Convert.ToDouble(dgvB2CL.Rows[0].Cells["colCessB2CL"].Value.ToString()) - Convert.ToDouble(dgvB2CL.Rows[1].Cells["colCessB2CL"].Value.ToString()));

                //ZERO RATED
                dgvZeroRated.Rows.Add("Difference", (Convert.ToDouble(dgvZeroRated.Rows[0].Cells["colInvoicesZ"].Value.ToString()) - Convert.ToDouble(dgvZeroRated.Rows[1].Cells["colInvoicesZ"].Value.ToString())),
               Convert.ToDouble(dgvZeroRated.Rows[0].Cells["colTaxValueZ"].Value.ToString()) - Convert.ToDouble(dgvZeroRated.Rows[1].Cells["colTaxValueZ"].Value.ToString()), Convert.ToDouble(dgvZeroRated.Rows[0].Cells["colIGSTZ"].Value.ToString()) - Convert.ToDouble(dgvZeroRated.Rows[1].Cells["colIGSTZ"].Value.ToString()), Convert.ToDouble(dgvZeroRated.Rows[0].Cells["colCessZ"].Value.ToString()) - Convert.ToDouble(dgvZeroRated.Rows[1].Cells["colCessZ"].Value.ToString()));

                //CDN
                dgvCDN.Rows.Add("Difference", (Convert.ToDouble(dgvCDN.Rows[0].Cells["colInvoicesCD"].Value.ToString()) - Convert.ToDouble(dgvCDN.Rows[1].Cells["colInvoicesCD"].Value.ToString())),
                 Convert.ToDouble(dgvCDN.Rows[0].Cells["colTaxValueCD"].Value.ToString()) - Convert.ToDouble(dgvCDN.Rows[1].Cells["colTaxValueCD"].Value.ToString()), Convert.ToDouble(dgvCDN.Rows[0].Cells["colIGSTCD"].Value.ToString()) - Convert.ToDouble(dgvCDN.Rows[1].Cells["colIGSTCD"].Value.ToString()), Convert.ToDouble(dgvCDN.Rows[0].Cells["colSGSTCD"].Value.ToString()) - Convert.ToDouble(dgvCDN.Rows[1].Cells["colSGSTCD"].Value.ToString()), Convert.ToDouble(dgvCDN.Rows[0].Cells["colCGSTCD"].Value.ToString()) - Convert.ToDouble(dgvCDN.Rows[1].Cells["colCGSTCD"].Value.ToString()), Convert.ToDouble(dgvCDN.Rows[0].Cells["colCessCD"].Value.ToString()) - Convert.ToDouble(dgvCDN.Rows[1].Cells["colCessCD"].Value.ToString()));


                // CDNUR
                CDnUR.Rows.Add("Difference", (Convert.ToDouble(CDnUR.Rows[0].Cells["colInvoicesCDNUR"].Value.ToString()) - Convert.ToDouble(CDnUR.Rows[1].Cells["colInvoicesCDNUR"].Value.ToString())),
                Convert.ToDouble(CDnUR.Rows[0].Cells["colTaxValueCDNUR"].Value.ToString()) - Convert.ToDouble(CDnUR.Rows[1].Cells["colTaxValueCDNUR"].Value.ToString()), Convert.ToDouble(CDnUR.Rows[0].Cells["colIGSTCDNUR"].Value.ToString()) - Convert.ToDouble(CDnUR.Rows[1].Cells["colIGSTCDNUR"].Value.ToString()), Convert.ToDouble(CDnUR.Rows[0].Cells["colSGSTCDNUR"].Value.ToString()) - Convert.ToDouble(CDnUR.Rows[1].Cells["colSGSTCDNUR"].Value.ToString()), Convert.ToDouble(CDnUR.Rows[0].Cells["colCGSTCDNUR"].Value.ToString()) - Convert.ToDouble(CDnUR.Rows[1].Cells["colCGSTCDNUR"].Value.ToString()), Convert.ToDouble(CDnUR.Rows[0].Cells["colCessCDNUR"].Value.ToString()) - Convert.ToDouble(CDnUR.Rows[1].Cells["colCessCDNUR"].Value.ToString()));

                //NILL
                dgvNill.Rows.Add("Difference", (Convert.ToDouble(dgvNill.Rows[0].Cells["colInvoicesNIL"].Value.ToString()) - Convert.ToDouble(dgvNill.Rows[1].Cells["colInvoicesNIL"].Value.ToString())),
                Convert.ToDouble(dgvNill.Rows[0].Cells["colTaxValueNIL"].Value.ToString()) - Convert.ToDouble(dgvNill.Rows[1].Cells["colTaxValueNIL"].Value.ToString()), Convert.ToDouble(dgvNill.Rows[0].Cells["colIGSTNIL"].Value.ToString()) - Convert.ToDouble(dgvNill.Rows[1].Cells["colIGSTNIL"].Value.ToString()), Convert.ToDouble(dgvNill.Rows[0].Cells["colSGSTNIL"].Value.ToString()) - Convert.ToDouble(dgvNill.Rows[1].Cells["colSGSTNIL"].Value.ToString()), Convert.ToDouble(dgvNill.Rows[0].Cells["colCGSTNIL"].Value.ToString()) - Convert.ToDouble(dgvNill.Rows[1].Cells["colCGSTNIL"].Value.ToString()), Convert.ToDouble(dgvNill.Rows[0].Cells["colCessNIL"].Value.ToString()) - Convert.ToDouble(dgvNill.Rows[1].Cells["colCessNIL"].Value.ToString()));

                //AR
                dgvAR.Rows.Add("Difference", (Convert.ToString(dgvAR.Rows[0].Cells["colInvoicesAR"].Value).Trim() == "" ? 0 : Convert.ToDouble(dgvAR.Rows[0].Cells["colInvoicesAR"].Value) - Convert.ToDouble(dgvAR.Rows[1].Cells["colInvoicesAR"].Value.ToString())),
                    Convert.ToString(dgvAR.Rows[0].Cells["colTaxValueAR"].Value).Trim() == "" ? 0 : Convert.ToDouble(dgvAR.Rows[0].Cells["colTaxValueAR"].Value) - Convert.ToDouble(dgvAR.Rows[1].Cells["colTaxValueAR"].Value.ToString()), Convert.ToDouble(dgvAR.Rows[0].Cells["colIGSTAR"].Value.ToString()) - Convert.ToDouble(dgvAR.Rows[1].Cells["colIGSTAR"].Value.ToString()), Convert.ToDouble(dgvAR.Rows[0].Cells["colSGSTAR"].Value.ToString()) - Convert.ToDouble(dgvAR.Rows[1].Cells["colSGSTAR"].Value.ToString()), Convert.ToDouble(dgvAR.Rows[0].Cells["colCGSTAR"].Value.ToString()) - Convert.ToDouble(dgvAR.Rows[1].Cells["colCGSTAR"].Value.ToString()), Convert.ToDouble(dgvAR.Rows[0].Cells["colCessAR"].Value.ToString()) - Convert.ToDouble(dgvAR.Rows[1].Cells["colCessAR"].Value.ToString()));

                //AA
                dgvAA.Rows.Add("Difference", (Convert.ToString(dgvAA.Rows[0].Cells["colInvoicesAA"].Value).Trim() == "" ? 0 : Convert.ToDouble(Convert.ToString(dgvAA.Rows[0].Cells["colInvoicesAA"].Value).Trim()) - Convert.ToDouble(dgvAA.Rows[1].Cells["colInvoicesAA"].Value.ToString())),
                    Convert.ToString(dgvAA.Rows[0].Cells["colTaxValueAA"].Value).Trim() == "" ? 0 : Convert.ToDouble(dgvAA.Rows[0].Cells["colTaxValueAA"].Value) - Convert.ToDouble(dgvAA.Rows[1].Cells["colTaxValueAA"].Value.ToString()), Convert.ToDouble(dgvAA.Rows[0].Cells["colIGSTAA"].Value.ToString()) - Convert.ToDouble(dgvAA.Rows[1].Cells["colIGSTAA"].Value.ToString()), Convert.ToDouble(dgvAA.Rows[0].Cells["colSGSTAA"].Value.ToString()) - Convert.ToDouble(dgvAA.Rows[1].Cells["colSGSTAA"].Value.ToString()), Convert.ToDouble(dgvAA.Rows[0].Cells["colCGSTAA"].Value.ToString()) - Convert.ToDouble(dgvAA.Rows[1].Cells["colCGSTAA"].Value.ToString()), Convert.ToDouble(dgvAA.Rows[0].Cells["colCessAA"].Value.ToString()) - Convert.ToDouble(dgvAA.Rows[1].Cells["colCessAA"].Value.ToString()));

                //HSN
                dgvHSN.Rows.Add("Difference", (Convert.ToDouble(dgvHSN.Rows[0].Cells["colInvoicesHSN"].Value.ToString()) - Convert.ToDouble(dgvHSN.Rows[1].Cells["colInvoicesHSN"].Value.ToString())),
                  Convert.ToDouble(dgvHSN.Rows[0].Cells["colTaxValueHSN"].Value.ToString()) - Convert.ToDouble(dgvHSN.Rows[1].Cells["colTaxValueHSN"].Value.ToString()), Convert.ToDouble(dgvHSN.Rows[0].Cells["colIGSTHSN"].Value.ToString()) - Convert.ToDouble(dgvHSN.Rows[1].Cells["colIGSTHSN"].Value.ToString()), Convert.ToDouble(dgvHSN.Rows[0].Cells["colSGSTHSN"].Value.ToString()) - Convert.ToDouble(dgvHSN.Rows[1].Cells["colSGSTHSN"].Value.ToString()), Convert.ToDouble(dgvHSN.Rows[0].Cells["colCGSTHSN"].Value.ToString()) - Convert.ToDouble(dgvHSN.Rows[1].Cells["colCGSTHSN"].Value.ToString()), Convert.ToDouble(dgvHSN.Rows[0].Cells["colCessHSN"].Value.ToString()) - Convert.ToDouble(dgvHSN.Rows[1].Cells["colCessHSN"].Value.ToString()));


                //B2B
                dgvDoc.Rows.Add("Difference", (Convert.ToDouble(dgvDoc.Rows[0].Cells["colInvoicesDOC"].Value.ToString()) - Convert.ToDouble(dgvDoc.Rows[1].Cells["colInvoicesDOC"].Value.ToString())), "0", "0", "0", "0", "0");

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

        public void GetJSONData(string jsonString)
        {
            try
            {
                int _Result = 0;

                #region first delete old data from database
                string Query = "Delete from SPQGSTNSummary where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                _Result = objGSTR5.IUDData(Query);
                if (_Result != 1)
                {
                    MessageBox.Show("System error.\nPlease try after sometime - SPQGSTNSummary!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion

                if (jsonString != "")
                {

                    #region Create DataTable
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Fld_SectionName");
                    dt.Columns.Add("Fld_InvoiceNo");
                    dt.Columns.Add("Fld_TaxValue");
                    dt.Columns.Add("Fld_IGST");
                    dt.Columns.Add("Fld_CGST");
                    dt.Columns.Add("Fld_SGST");
                    dt.Columns.Add("Fld_CESS");
                    dt.Rows.Add("B2B", "0", "0", "0", "0", "0", "0");
                    dt.Rows.Add("B2CL", "0", "0", "0", "0", "0", "0");
                    dt.Rows.Add("B2CS", "0", "0", "0", "0", "0", "0");
                    dt.Rows.Add("EXP", "0", "0", "0", "0", "0", "0");
                    dt.Rows.Add("CDN", "0", "0", "0", "0", "0", "0");
                    dt.Rows.Add("CDNUR", "0", "0", "0", "0", "0", "0");
                    dt.Rows.Add("NIL", "0", "0", "0", "0", "0", "0");
                    dt.Rows.Add("AR", "0", "0", "0", "0", "0", "0");
                    dt.Rows.Add("AA", "0", "0", "0", "0", "0", "0");
                    dt.Rows.Add("HSN", "0", "0", "0", "0", "0", "0");
                    dt.Rows.Add("DOC", "0", "0", "0", "0", "0", "0");

                    dt.Rows.Add("B2BA", "0", "0", "0", "0", "0", "0");
                    dt.Rows.Add("B2CLA", "0", "0", "0", "0", "0", "0");
                    dt.Rows.Add("B2CSA", "0", "0", "0", "0", "0", "0");
                    dt.Rows.Add("EXPA", "0", "0", "0", "0", "0", "0");
                    dt.Rows.Add("CDNRA", "0", "0", "0", "0", "0", "0");
                    dt.Rows.Add("CDNURA", "0", "0", "0", "0", "0", "0");
                    dt.Rows.Add("ATA", "0", "0", "0", "0", "0", "0");
                    dt.Rows.Add("TXPDA", "0", "0", "0", "0", "0", "0");
                    #endregion

                    #region Download Json Data
                    RootObjectSummary obj = JsonConvert.DeserializeObject<RootObjectSummary>(jsonString);
                    if (obj != null)
                    {
                        int rn = 0;
                        for (int i = 0; i < obj.data.sec_sum.Count; i++)
                        {
                            if (obj.data.sec_sum[i].sec_nm == "B2B")
                            {
                                rn = 0;
                                dt.Rows[rn]["Fld_InvoiceNo"] = Convert.ToString(obj.data.sec_sum[i].ttl_rec);
                                dt.Rows[rn]["Fld_TaxValue"] = Convert.ToString(obj.data.sec_sum[i].ttl_tax);
                                dt.Rows[rn]["Fld_IGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_igst);
                                dt.Rows[rn]["Fld_CGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_cgst);
                                dt.Rows[rn]["Fld_SGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_sgst);
                                dt.Rows[rn]["Fld_CESS"] = Convert.ToString(obj.data.sec_sum[i].ttl_cess);
                            }
                            else if (obj.data.sec_sum[i].sec_nm == "B2CL")
                            {
                                rn = 1;
                                dt.Rows[rn]["Fld_InvoiceNo"] = Convert.ToString(obj.data.sec_sum[i].ttl_rec);
                                dt.Rows[rn]["Fld_TaxValue"] = Convert.ToString(obj.data.sec_sum[i].ttl_tax);
                                dt.Rows[rn]["Fld_IGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_igst);
                                dt.Rows[rn]["Fld_CGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_cgst);
                                dt.Rows[rn]["Fld_SGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_sgst);
                                dt.Rows[rn]["Fld_CESS"] = Convert.ToString(obj.data.sec_sum[i].ttl_cess);
                            }
                            else if (obj.data.sec_sum[i].sec_nm == "B2CS")
                            {
                                rn = 2;
                                dt.Rows[rn]["Fld_InvoiceNo"] = Convert.ToString(obj.data.sec_sum[i].ttl_rec);
                                dt.Rows[rn]["Fld_TaxValue"] = Convert.ToString(obj.data.sec_sum[i].ttl_tax);
                                dt.Rows[rn]["Fld_IGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_igst);
                                dt.Rows[rn]["Fld_CGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_cgst);
                                dt.Rows[rn]["Fld_SGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_sgst);
                                dt.Rows[rn]["Fld_CESS"] = Convert.ToString(obj.data.sec_sum[i].ttl_cess);
                            }
                            else if (obj.data.sec_sum[i].sec_nm == "EXP")
                            {
                                rn = 3;
                                dt.Rows[rn]["Fld_InvoiceNo"] = Convert.ToString(obj.data.sec_sum[i].ttl_rec);
                                dt.Rows[rn]["Fld_TaxValue"] = Convert.ToString(obj.data.sec_sum[i].ttl_tax);
                                dt.Rows[rn]["Fld_IGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_igst);
                                dt.Rows[rn]["Fld_CGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_cgst);
                                dt.Rows[rn]["Fld_SGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_sgst);
                                dt.Rows[rn]["Fld_CESS"] = Convert.ToString(obj.data.sec_sum[i].ttl_cess);
                            }
                            else if (obj.data.sec_sum[i].sec_nm == "CDNR")
                            {
                                rn = 4;
                                dt.Rows[rn]["Fld_InvoiceNo"] = Convert.ToString(obj.data.sec_sum[i].ttl_rec);
                                dt.Rows[rn]["Fld_TaxValue"] = Convert.ToString(obj.data.sec_sum[i].ttl_tax);
                                dt.Rows[rn]["Fld_IGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_igst);
                                dt.Rows[rn]["Fld_CGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_cgst);
                                dt.Rows[rn]["Fld_SGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_sgst);
                                dt.Rows[rn]["Fld_CESS"] = Convert.ToString(obj.data.sec_sum[i].ttl_cess);
                            }
                            else if (obj.data.sec_sum[i].sec_nm == "CDNUR")
                            {
                                rn = 5;
                                dt.Rows[rn]["Fld_InvoiceNo"] = Convert.ToString(obj.data.sec_sum[i].ttl_rec);
                                dt.Rows[rn]["Fld_TaxValue"] = Convert.ToString(obj.data.sec_sum[i].ttl_tax);
                                dt.Rows[rn]["Fld_IGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_igst);
                                dt.Rows[rn]["Fld_CGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_cgst);
                                dt.Rows[rn]["Fld_SGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_sgst);
                                dt.Rows[rn]["Fld_CESS"] = Convert.ToString(obj.data.sec_sum[i].ttl_cess);
                            }
                            else if (obj.data.sec_sum[i].sec_nm == "NIL")
                            {
                                rn = 6;
                                dt.Rows[rn]["Fld_InvoiceNo"] = Convert.ToString(obj.data.sec_sum[i].ttl_rec);
                                dt.Rows[rn]["Fld_TaxValue"] = Convert.ToString(obj.data.sec_sum[i].ttl_tax);
                                dt.Rows[rn]["Fld_IGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_igst);
                                dt.Rows[rn]["Fld_CGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_cgst);
                                dt.Rows[rn]["Fld_SGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_sgst);
                                dt.Rows[rn]["Fld_CESS"] = Convert.ToString(obj.data.sec_sum[i].ttl_cess);
                            }
                            else if (obj.data.sec_sum[i].sec_nm == "AT")
                            {
                                rn = 7;
                                dt.Rows[rn]["Fld_InvoiceNo"] = Convert.ToString(obj.data.sec_sum[i].ttl_rec);
                                dt.Rows[rn]["Fld_TaxValue"] = Convert.ToString(obj.data.sec_sum[i].ttl_tax);
                                dt.Rows[rn]["Fld_IGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_igst);
                                dt.Rows[rn]["Fld_CGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_cgst);
                                dt.Rows[rn]["Fld_SGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_sgst);
                                dt.Rows[rn]["Fld_CESS"] = Convert.ToString(obj.data.sec_sum[i].ttl_cess);
                            }
                            else if (obj.data.sec_sum[i].sec_nm == "TXPD")
                            {
                                rn = 8;
                                dt.Rows[rn]["Fld_InvoiceNo"] = Convert.ToString(obj.data.sec_sum[i].ttl_rec);
                                dt.Rows[rn]["Fld_TaxValue"] = Convert.ToString(obj.data.sec_sum[i].ttl_tax);
                                dt.Rows[rn]["Fld_IGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_igst);
                                dt.Rows[rn]["Fld_CGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_cgst);
                                dt.Rows[rn]["Fld_SGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_sgst);
                                dt.Rows[rn]["Fld_CESS"] = Convert.ToString(obj.data.sec_sum[i].ttl_cess);
                            }
                            else if (obj.data.sec_sum[i].sec_nm == "HSN")
                            {
                                rn = 9;
                                dt.Rows[rn]["Fld_InvoiceNo"] = Convert.ToString(obj.data.sec_sum[i].ttl_rec);
                                dt.Rows[rn]["Fld_TaxValue"] = Convert.ToString(obj.data.sec_sum[i].ttl_tax);
                                dt.Rows[rn]["Fld_IGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_igst);
                                dt.Rows[rn]["Fld_CGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_cgst);
                                dt.Rows[rn]["Fld_SGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_sgst);
                                dt.Rows[rn]["Fld_CESS"] = Convert.ToString(obj.data.sec_sum[i].ttl_cess);
                            }
                            else if (obj.data.sec_sum[i].sec_nm == "DOC_ISSUE")
                            {
                                rn = 10;
                                dt.Rows[rn]["Fld_InvoiceNo"] = Convert.ToString(obj.data.sec_sum[i].ttl_rec);
                                dt.Rows[rn]["Fld_TaxValue"] = Convert.ToString(obj.data.sec_sum[i].ttl_tax);
                                dt.Rows[rn]["Fld_IGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_igst);
                                dt.Rows[rn]["Fld_CGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_cgst);
                                dt.Rows[rn]["Fld_SGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_sgst);
                                dt.Rows[rn]["Fld_CESS"] = Convert.ToString(obj.data.sec_sum[i].ttl_cess);
                            }
                            else if (obj.data.sec_sum[i].sec_nm == "B2BA")
                            {
                                rn = 11;
                                dt.Rows[rn]["Fld_InvoiceNo"] = Convert.ToString(obj.data.sec_sum[i].ttl_rec);
                                dt.Rows[rn]["Fld_TaxValue"] = Convert.ToString(obj.data.sec_sum[i].ttl_tax);
                                dt.Rows[rn]["Fld_IGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_igst);
                                dt.Rows[rn]["Fld_CGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_cgst);
                                dt.Rows[rn]["Fld_SGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_sgst);
                                dt.Rows[rn]["Fld_CESS"] = Convert.ToString(obj.data.sec_sum[i].ttl_cess);
                            }
                            else if (obj.data.sec_sum[i].sec_nm == "B2CLA")
                            {
                                rn = 12;
                                dt.Rows[rn]["Fld_InvoiceNo"] = Convert.ToString(obj.data.sec_sum[i].ttl_rec);
                                dt.Rows[rn]["Fld_TaxValue"] = Convert.ToString(obj.data.sec_sum[i].ttl_tax);
                                dt.Rows[rn]["Fld_IGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_igst);
                                dt.Rows[rn]["Fld_CGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_cgst);
                                dt.Rows[rn]["Fld_SGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_sgst);
                                dt.Rows[rn]["Fld_CESS"] = Convert.ToString(obj.data.sec_sum[i].ttl_cess);
                            }
                            else if (obj.data.sec_sum[i].sec_nm == "B2CSA")
                            {
                                rn = 13;
                                dt.Rows[rn]["Fld_InvoiceNo"] = Convert.ToString(obj.data.sec_sum[i].ttl_rec);
                                dt.Rows[rn]["Fld_TaxValue"] = Convert.ToString(obj.data.sec_sum[i].ttl_tax);
                                dt.Rows[rn]["Fld_IGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_igst);
                                dt.Rows[rn]["Fld_CGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_cgst);
                                dt.Rows[rn]["Fld_SGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_sgst);
                                dt.Rows[rn]["Fld_CESS"] = Convert.ToString(obj.data.sec_sum[i].ttl_cess);
                            }
                            else if (obj.data.sec_sum[i].sec_nm == "EXPA")
                            {
                                rn = 14;
                                dt.Rows[rn]["Fld_InvoiceNo"] = Convert.ToString(obj.data.sec_sum[i].ttl_rec);
                                dt.Rows[rn]["Fld_TaxValue"] = Convert.ToString(obj.data.sec_sum[i].ttl_tax);
                                dt.Rows[rn]["Fld_IGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_igst);
                                dt.Rows[rn]["Fld_CGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_cgst);
                                dt.Rows[rn]["Fld_SGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_sgst);
                                dt.Rows[rn]["Fld_CESS"] = Convert.ToString(obj.data.sec_sum[i].ttl_cess);
                            }
                            else if (obj.data.sec_sum[i].sec_nm == "CDNRA")
                            {
                                rn = 15;
                                dt.Rows[rn]["Fld_InvoiceNo"] = Convert.ToString(obj.data.sec_sum[i].ttl_rec);
                                dt.Rows[rn]["Fld_TaxValue"] = Convert.ToString(obj.data.sec_sum[i].ttl_tax);
                                dt.Rows[rn]["Fld_IGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_igst);
                                dt.Rows[rn]["Fld_CGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_cgst);
                                dt.Rows[rn]["Fld_SGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_sgst);
                                dt.Rows[rn]["Fld_CESS"] = Convert.ToString(obj.data.sec_sum[i].ttl_cess);
                            }
                            else if (obj.data.sec_sum[i].sec_nm == "CDNURA")
                            {
                                rn = 16;
                                dt.Rows[rn]["Fld_InvoiceNo"] = Convert.ToString(obj.data.sec_sum[i].ttl_rec);
                                dt.Rows[rn]["Fld_TaxValue"] = Convert.ToString(obj.data.sec_sum[i].ttl_tax);
                                dt.Rows[rn]["Fld_IGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_igst);
                                dt.Rows[rn]["Fld_CGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_cgst);
                                dt.Rows[rn]["Fld_SGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_sgst);
                                dt.Rows[rn]["Fld_CESS"] = Convert.ToString(obj.data.sec_sum[i].ttl_cess);
                            }
                            else if (obj.data.sec_sum[i].sec_nm == "ATA")
                            {
                                rn = 17;
                                dt.Rows[rn]["Fld_InvoiceNo"] = Convert.ToString(obj.data.sec_sum[i].ttl_rec);
                                dt.Rows[rn]["Fld_TaxValue"] = Convert.ToString(obj.data.sec_sum[i].ttl_tax);
                                dt.Rows[rn]["Fld_IGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_igst);
                                dt.Rows[rn]["Fld_CGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_cgst);
                                dt.Rows[rn]["Fld_SGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_sgst);
                                dt.Rows[rn]["Fld_CESS"] = Convert.ToString(obj.data.sec_sum[i].ttl_cess);
                            }
                            else if (obj.data.sec_sum[i].sec_nm == "TXPDA")
                            {
                                rn = 18;
                                dt.Rows[rn]["Fld_InvoiceNo"] = Convert.ToString(obj.data.sec_sum[i].ttl_rec);
                                dt.Rows[rn]["Fld_TaxValue"] = Convert.ToString(obj.data.sec_sum[i].ttl_tax);
                                dt.Rows[rn]["Fld_IGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_igst);
                                dt.Rows[rn]["Fld_CGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_cgst);
                                dt.Rows[rn]["Fld_SGST"] = Convert.ToString(obj.data.sec_sum[i].ttl_sgst);
                                dt.Rows[rn]["Fld_CESS"] = Convert.ToString(obj.data.sec_sum[i].ttl_cess);
                            }


                        }
                    }
                    #endregion

                    _Result = objGSTR5.GSTNSummaryBulkEntryJson(dt);
                    if (_Result != 1)
                        MessageBox.Show("System error.\nPlease try after sometime - SPQGSTNSummary Save!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show("Data saved successfully..", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                GetData();
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

        #region GST Methods
        protected string getSummary()
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
                HttpWebRequest httpWebRequest = this.PrepareGetRequest(new Uri(string.Concat("https://return.gst.gov.in/returns/auth/api/gstr1/summary?rtn_prd=", retDate)), "https://return.gst.gov.in/returns/auth/gstr1");
                this.response = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = this.response.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
                string str2 = streamReader.ReadToEnd();
                objRes.msg = this.ErrorCheck(str2);
                if (objRes.msg.ToLower() != "success")
                {
                    MessageBox.Show(objRes.msg, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return "";
                }
                else
                    return str2;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("403"))
                {
                    string str2 = "";
                    SPQGstLogin frm = new SPQGstLogin();
                    frm.Visible = false;
                    var result = frm.ShowDialog();
                    if (result != DialogResult.OK)
                    {
                        
                    }
                    else
                    {
                       str2 = getSummary();
                    }
                    return str2;
                }
                else
                {
                    pbGSTR1.Visible = false;
                    objRes.msg = string.Concat("Some error occured please check ur Connection/Data and try again", ex.Message);
                    return "";
                }
            }
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
        public string ErrorCheck(string reply)
        {
            string str;
            if (reply.Contains("Session Expired"))
            {
                str = "Session Expired";
            }
            else if (reply.Contains("You are not allowed to access Return for selected return period"))
            {
                str = "You are not allowed to access Return for selected return period.";
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
                        pbGSTR1.Visible = true;
                        Application.DoEvents();
                        string _str = "";

                        GetJSONData(getSummary());

                        pbGSTR1.Visible = false;
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

        private void msSave_Click(object sender, EventArgs e)
        {
            #region ADD DATATABLE COLUMN

            // CREATE DATATABLE TO STORE MAIN GRID DATA
            DataTable dt = new DataTable();

            // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
            foreach (DataGridViewColumn col in dgvB2B.Columns)
            {
                dt.Columns.Add(col.Name.ToString());
            }
            #endregion

            #region GET DATA
            if (dgvB2B.Rows.Count == 3)
            {
                DataRow dr = dt.NewRow();
                dr["colSectionName"] = "B2B";
                dr["colInvoices"] = Convert.ToString(dgvB2B.Rows[1].Cells["colInvoices"].Value).Trim();
                dr["colTaxValue"] = Convert.ToString(dgvB2B.Rows[1].Cells["colTaxValue"].Value).Trim();
                dr["colIGST"] = Convert.ToString(dgvB2B.Rows[1].Cells["colIGST"].Value).Trim();
                dr["colCGST"] = Convert.ToString(dgvB2B.Rows[1].Cells["colCGST"].Value).Trim();
                dr["colSGST"] = Convert.ToString(dgvB2B.Rows[1].Cells["colSGST"].Value).Trim();
                dr["colCess"] = Convert.ToString(dgvB2B.Rows[1].Cells["colCess"].Value).Trim();
                dt.Rows.Add(dr);
            }
            if (dgvB2CL.Rows.Count == 3)
            {
                DataRow dr = dt.NewRow();
                dr["colSectionName"] = "B2CL";
                dr["colInvoices"] = Convert.ToString(dgvB2CL.Rows[1].Cells["colInvoicesB2CL"].Value).Trim();
                dr["colTaxValue"] = Convert.ToString(dgvB2CL.Rows[1].Cells["colTaxValueB2CL"].Value).Trim();
                dr["colIGST"] = Convert.ToString(dgvB2CL.Rows[1].Cells["colIGSTB2CL"].Value).Trim();
                dr["colCGST"] = Convert.ToString(dgvB2CL.Rows[1].Cells["colCGSTB2CL"].Value).Trim();
                dr["colSGST"] = Convert.ToString(dgvB2CL.Rows[1].Cells["colCGSTB2CL"].Value).Trim();
                dr["colCess"] = Convert.ToString(dgvB2CL.Rows[1].Cells["colCessB2CL"].Value).Trim();
                dt.Rows.Add(dr);
            }
            if (dgvB2CS.Rows.Count == 3)
            {
                DataRow dr = dt.NewRow();
                dr["colSectionName"] = "B2CS";
                dr["colInvoices"] = Convert.ToString(dgvB2CS.Rows[1].Cells["colInvoicesB2CS"].Value).Trim();
                dr["colTaxValue"] = Convert.ToString(dgvB2CS.Rows[1].Cells["colTaxValueB2CS"].Value).Trim();
                dr["colIGST"] = Convert.ToString(dgvB2CS.Rows[1].Cells["colIGSTB2CS"].Value).Trim();
                dr["colCGST"] = Convert.ToString(dgvB2CS.Rows[1].Cells["colCGSTB2CS"].Value).Trim();
                dr["colSGST"] = Convert.ToString(dgvB2CS.Rows[1].Cells["colCGSTB2CS"].Value).Trim();
                dr["colCess"] = Convert.ToString(dgvB2CS.Rows[1].Cells["colCessB2CS"].Value).Trim();
                dt.Rows.Add(dr);
            }
            if (dgvZeroRated.Rows.Count == 3)
            {
                DataRow dr = dt.NewRow();
                dr["colSectionName"] = "EXP";
                dr["colInvoices"] = Convert.ToString(dgvZeroRated.Rows[1].Cells["colInvoicesZ"].Value).Trim();
                dr["colTaxValue"] = Convert.ToString(dgvZeroRated.Rows[1].Cells["colTaxValueZ"].Value).Trim();
                dr["colIGST"] = Convert.ToString(dgvZeroRated.Rows[1].Cells["colIGSTZ"].Value).Trim();
                dr["colCGST"] = Convert.ToString(dgvZeroRated.Rows[1].Cells["colCGSTZ"].Value).Trim();
                dr["colSGST"] = Convert.ToString(dgvZeroRated.Rows[1].Cells["colSGSTZ"].Value).Trim();
                dr["colCess"] = Convert.ToString(dgvZeroRated.Rows[1].Cells["colCessZ"].Value).Trim();
                dt.Rows.Add(dr);
            }
            if (dgvCDN.Rows.Count == 3)
            {
                DataRow dr = dt.NewRow();
                dr["colSectionName"] = "CDN";
                dr["colInvoices"] = Convert.ToString(dgvCDN.Rows[1].Cells["colInvoicesCD"].Value).Trim();
                dr["colTaxValue"] = Convert.ToString(dgvCDN.Rows[1].Cells["colTaxValueCD"].Value).Trim();
                dr["colIGST"] = Convert.ToString(dgvCDN.Rows[1].Cells["colIGSTCD"].Value).Trim();
                dr["colCGST"] = Convert.ToString(dgvCDN.Rows[1].Cells["colCGSTCD"].Value).Trim();
                dr["colSGST"] = Convert.ToString(dgvCDN.Rows[1].Cells["colSGSTCD"].Value).Trim();
                dr["colCess"] = Convert.ToString(dgvCDN.Rows[1].Cells["colCessCD"].Value).Trim();
                dt.Rows.Add(dr);
            }
            if (CDnUR.Rows.Count == 3)
            {
                DataRow dr = dt.NewRow();
                dr["colSectionName"] = "CDNUR";
                dr["colInvoices"] = Convert.ToString(CDnUR.Rows[1].Cells["colInvoicesCDNUR"].Value).Trim();
                dr["colTaxValue"] = Convert.ToString(CDnUR.Rows[1].Cells["colTaxValueCDNUR"].Value).Trim();
                dr["colIGST"] = Convert.ToString(CDnUR.Rows[1].Cells["colIGSTCDNUR"].Value).Trim();
                dr["colCGST"] = Convert.ToString(CDnUR.Rows[1].Cells["colCGSTCDNUR"].Value).Trim();
                dr["colSGST"] = Convert.ToString(CDnUR.Rows[1].Cells["colSGSTCDNUR"].Value).Trim();
                dr["colCess"] = Convert.ToString(CDnUR.Rows[1].Cells["colCessCDNUR"].Value).Trim();
                dt.Rows.Add(dr);
            }
            if (dgvNill.Rows.Count == 3)
            {
                DataRow dr = dt.NewRow();
                dr["colSectionName"] = "NIL";
                dr["colInvoices"] = Convert.ToString(dgvNill.Rows[1].Cells["colInvoicesNIL"].Value).Trim();
                dr["colTaxValue"] = Convert.ToString(dgvNill.Rows[1].Cells["colTaxValueNIL"].Value).Trim();
                dr["colIGST"] = Convert.ToString(dgvNill.Rows[1].Cells["colIGSTNIL"].Value).Trim();
                dr["colCGST"] = Convert.ToString(dgvNill.Rows[1].Cells["colCGSTNIL"].Value).Trim();
                dr["colSGST"] = Convert.ToString(dgvNill.Rows[1].Cells["colSGSTNIL"].Value).Trim();
                dr["colCess"] = Convert.ToString(dgvNill.Rows[1].Cells["colCessNIL"].Value).Trim();
                dt.Rows.Add(dr);
            }
            if (dgvAR.Rows.Count == 3)
            {
                DataRow dr = dt.NewRow();
                dr["colSectionName"] = "AR";
                dr["colInvoices"] = Convert.ToString(dgvAR.Rows[1].Cells["colInvoicesAR"].Value).Trim();
                dr["colTaxValue"] = Convert.ToString(dgvAR.Rows[1].Cells["colTaxValueAR"].Value).Trim();
                dr["colIGST"] = Convert.ToString(dgvAR.Rows[1].Cells["colIGSTAR"].Value).Trim();
                dr["colCGST"] = Convert.ToString(dgvAR.Rows[1].Cells["colCGSTAR"].Value).Trim();
                dr["colSGST"] = Convert.ToString(dgvAR.Rows[1].Cells["colSGSTAR"].Value).Trim();
                dr["colCess"] = Convert.ToString(dgvAR.Rows[1].Cells["colCessAR"].Value).Trim();
                dt.Rows.Add(dr);
            }
            if (dgvAA.Rows.Count == 3)
            {
                DataRow dr = dt.NewRow();
                dr["colSectionName"] = "AA";
                dr["colInvoices"] = Convert.ToString(dgvAA.Rows[1].Cells["colInvoicesAA"].Value).Trim();
                dr["colTaxValue"] = Convert.ToString(dgvAA.Rows[1].Cells["colTaxValueAA"].Value).Trim();
                dr["colIGST"] = Convert.ToString(dgvAA.Rows[1].Cells["colIGSTAA"].Value).Trim();
                dr["colCGST"] = Convert.ToString(dgvAA.Rows[1].Cells["colCGSTAA"].Value).Trim();
                dr["colSGST"] = Convert.ToString(dgvAA.Rows[1].Cells["colSGSTAA"].Value).Trim();
                dr["colCess"] = Convert.ToString(dgvAA.Rows[1].Cells["colCessAA"].Value).Trim();
                dt.Rows.Add(dr);
            }
            if (dgvHSN.Rows.Count == 3)
            {
                DataRow dr = dt.NewRow();
                dr["colSectionName"] = "HSN";
                dr["colInvoices"] = Convert.ToString(dgvHSN.Rows[1].Cells["colInvoicesHSN"].Value).Trim();
                dr["colTaxValue"] = Convert.ToString(dgvHSN.Rows[1].Cells["colTaxValueHSN"].Value).Trim();
                dr["colIGST"] = Convert.ToString(dgvHSN.Rows[1].Cells["colIGSTHSN"].Value).Trim();
                dr["colCGST"] = Convert.ToString(dgvHSN.Rows[1].Cells["colCGSTHSN"].Value).Trim();
                dr["colSGST"] = Convert.ToString(dgvHSN.Rows[1].Cells["colSGSTHSN"].Value).Trim();
                dr["colCess"] = Convert.ToString(dgvHSN.Rows[1].Cells["colCessHSN"].Value).Trim();
                dt.Rows.Add(dr);
            }
            if (dgvDoc.Rows.Count == 3)
            {
                DataRow dr = dt.NewRow();
                dr["colSectionName"] = "DOC";
                dr["colInvoices"] = Convert.ToString(dgvDoc.Rows[1].Cells["colInvoicesDOC"].Value).Trim();
                dr["colTaxValue"] = Convert.ToString(dgvDoc.Rows[1].Cells["colTaxValDOC"].Value).Trim();
                dr["colIGST"] = Convert.ToString(dgvDoc.Rows[1].Cells["colIGSTDOC"].Value).Trim();
                dr["colCGST"] = Convert.ToString(dgvDoc.Rows[1].Cells["colCGSTDOC"].Value).Trim();
                dr["colSGST"] = Convert.ToString(dgvDoc.Rows[1].Cells["colSGSTDOC"].Value).Trim();
                dr["colCess"] = Convert.ToString(dgvDoc.Rows[1].Cells["colCessDOC"].Value).Trim();
                dt.Rows.Add(dr);
            }
            #endregion

            if (dt != null && dt.Rows.Count > 0)
            {
                #region RECORD SAVE
                string Query = "";
                int _Result = 0;

                // CHECK THERE ARE RECORDS IN GRID
                if (dt != null && dt.Rows.Count > 0)
                {
                    #region FIRST DELETE OLD DATA FROM DATABASE
                    Query = "Delete from SPQGSTNSummary where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
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
                    _Result = objGSTR5.GSTNSummaryBulkEntry(dt);

                    if (_Result == 1)
                    {
                        pbGSTR1.Visible = false;

                        //DONE
                        MessageBox.Show("Data Saved Successfully …!!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // BIND DATA
                        //GetData();
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
                    Query = "Delete from SPQGSTNSummary where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";

                    // FIRE QUEARY TO DELETE RECORDS
                    _Result = objGSTR5.IUDData(Query);

                    if (_Result == 1)
                    {
                        // IF RECORDS DELETED FROM DATABASE  
                        pbGSTR1.Visible = false;
                        MessageBox.Show("Record Successfully Deleted!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            }
        }

        private void msClose_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(CommonHelper.IsMainFormType) == "1Sum") // GSTR-1 Main form
            {
                SPQCompanyDashboard obj = new SPQCompanyDashboard();
                obj.MdiParent = this.MdiParent;
                Utility.CloseAllOpenForm();
                obj.Dock = DockStyle.Fill;
                obj.Show();
            }
            else
            {
                SPQGSTR1Dashboard obj = new SPQGSTR1Dashboard();
                obj.MdiParent = this.MdiParent;
                Utility.CloseAllOpenForm();
                obj.Dock = DockStyle.Fill;
                obj.Show();
            }
        }


        private void dgvGetSummary_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.dgvB2B.ClearSelection();
        }
    }

    #region JSON Class
    public class CptySum
    {
        public string state_cd { get; set; }
        public string chksum { get; set; }
        public int ttl_rec { get; set; }
        public double ttl_val { get; set; }
        public double ttl_tax { get; set; }
        public double ttl_igst { get; set; }
        public double ttl_cess { get; set; }
        public string ctin { get; set; }
        public double? ttl_sgst { get; set; }
        public double? ttl_cgst { get; set; }
    }

    public class SecSum
    {
        public string sec_nm { get; set; }
        public string chksum { get; set; }

        [DefaultValue("0")]
        public int ttl_rec { get; set; }
        public double ttl_val { get; set; }
        public double ttl_tax { get; set; }
        public double ttl_igst { get; set; }
        public double ttl_sgst { get; set; }
        public double ttl_cgst { get; set; }
        public double ttl_cess { get; set; }
        public int? ttl_doc_issued { get; set; }
        public int? ttl_doc_cancelled { get; set; }
        public int? net_doc_issued { get; set; }
        public double? ttl_expt_amt { get; set; }
        public double? ttl_ngsup_amt { get; set; }
        public double? ttl_nilsup_amt { get; set; }
        public List<CptySum> cpty_sum { get; set; }
    }

    public class Data
    {
        public string gstin { get; set; }
        public string ret_period { get; set; }
        public string chksum { get; set; }
        public string time { get; set; }
        public List<SecSum> sec_sum { get; set; }
    }

    public class RootObjectSummary
    {
        public int status { get; set; }
        public Data data { get; set; }
    }
    #endregion

    #region utility class
    public class response2A
    {
        public string msg { get; set; }
        public string strJson { get; set; }
        public bool flg { get; set; }
    }
    #endregion

}