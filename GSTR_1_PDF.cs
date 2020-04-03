using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json.Linq;
using System;
using Proactive.CustomTools;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;





namespace SPEQTAGST_DESIGN
{
    public partial class GSTR_1_PDF : Form
    {
         CookieContainer Cc = new CookieContainer();
        HttpWebResponse response;
        public GSTR_1_PDF()
        {
            InitializeComponent();
            
        }



      

        public void DownloadLiveData(ref string returnvalue)
        {
            
            //clsPubPro _clsPubPro;
            try
            {
                
                string reply = "";
                
                // string strQuery = "";
                //string companyGSTN = CommonHelper.CompanyGSTN;
                //string TdstcsYear= CommonHelper.ReturnYear;
                //string month = CommonHelper.GetMonth(CommonHelper.SelectedMonth);
                //string reqParam = "";
                //if (Convert.ToInt32(month) > 3 && Convert.ToInt32(month) <= 12) reqParam = string.Concat(CommonHelper.GetMonth(CommonHelper.SelectedMonth), CommonHelper.ReturnYear.Split('-')[0].Trim());
                //else reqParam = string.Concat(CommonHelper.GetMonth(CommonHelper.SelectedMonth), CommonHelper.ReturnYear.Split('-')[1].Trim());
                //string _Param = "092019";

                var obj = clsPro.Cooki != null ? clsPro.Cooki.FirstOrDefault(x => x.ckname == (string.Concat("loginCookies_", "1"))) : null;

                if (obj != null && obj.CC1 != null)
                {
                    this.Cc = obj.CC1;

                    //Request URL: https://return.gst.gov.in/returns2/auth/gstr9/dashboard
                    //Referer: https://services.gst.gov.in/services/auth/certs
                    //Request:https://return.gst.gov.in/returns/auth/api/offline/upload/summary?rtn_prd=032018&rtn_typ=GSTR9C
                    //Referer:https://return.gst.gov.in/returns2/auth/gstr9c/offlineupload

                    //https://return.gst.gov.in/returns2/auth/api/gstr9/gstr1summ?gstin=09AAHFC3214F1ZT&ret_period=032018

                    HttpWebRequest httpWebRequest = this.PrepareGetRequestTdsTcs(new Uri(string.Format("https://return.gst.gov.in/returns2/auth/api/gstr9/gstr1summ?gstin=09AAHFC3214F1ZT&ret_period=032018")), "https://return.gst.gov.in/returns2/auth/gstr9/dashboard");
                    this.response = (HttpWebResponse)httpWebRequest.GetResponse();
                    Stream responseStream = this.response.GetResponseStream();
                    reply = (new StreamReader(responseStream, Encoding.UTF8)).ReadToEnd();
                    returnvalue = reply;

                }
                else
                {
                    SPQGstLogin frm = new SPQGstLogin();
                    frm.strBulk = "BULK";
                    frm.Visible = false;
                    var result = frm.ShowDialog();
                    if (result != DialogResult.OK)
                    {
                        //DownloadLiveData(returnvalue);
                    }
                    else
                    {
                        DownloadLiveData(ref returnvalue);
                    }
                }

            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                if (!exception.Message.Contains("403"))
                {
                    MessageBox.Show(string.Concat("Error : ", exception.Message), "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    object[] message = new object[] { exception.Message, Environment.NewLine, exception.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine };
                    string str = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", message);
                    StreamWriter streamWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                    streamWriter.Write(str);
                    streamWriter.Close();
                    
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
                        DownloadLiveData(ref returnvalue);
                    }

                  
                  
                }
            }
            finally
            {
               
            }
            // return true;
        }

        protected HttpWebRequest PrepareGetRequestTdsTcs(Uri uri, string referer)
        {
            HttpWebRequest httpWebRequest;
            try
            {
                HttpWebRequest cc = (HttpWebRequest)WebRequest.Create(uri);
                cc.CookieContainer = this.Cc;
                cc.KeepAlive = true;
                cc.Method = "GET";


                //if (uri.ToString().Contains("https://services.gst.gov.in/document/" + docid + "/" + applnId + ""))
                //{
                //    cc.Host = "enroll.gst.gov.in";
                //}
                if (uri.ToString().Contains("registration/auth/"))
                {
                    cc.Host = "enroll.gst.gov.in";
                }

                else if (uri.ToString().Contains("payment.gst.gov.in/"))
                {
                    cc.Host = "payment.gst.gov.in";
                }
                else if (uri.ToString().Contains("return.gst.gov.in/"))
                {
                    cc.Host = "return.gst.gov.in";
                }
                else if (uri.ToString().Contains("files.gst.gov.in"))
                {
                    cc.Host = "files.gst.gov.in";
                }

                else
                {
                    cc.Host = "services.gst.gov.in";
                }
                if (referer != null)
                {
                    cc.Referer = referer;

                }

                else if (referer == null)
                {
                    cc.Headers.Add("Upgrade-Insecure-Requests", "1");
                }
                if (uri.ToString().Contains("files.gst.gov.in"))
                {
                    cc.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                    cc.Headers.Add("Accept-Encoding", "gzip, deflate, br");

                }
                else
                {
                    cc.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                }
                cc.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
                cc.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                httpWebRequest = cc;
            }
            catch (Exception exception )
            {
                httpWebRequest = null;
                exception.ToString();
            }
            return httpWebRequest;
        }













        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                string reply="";
                DownloadLiveData(ref reply);
                string strjson = reply;

                JObject obj = JObject.Parse(strjson);
                //--------DateTime
                String fy = obj["data"]["fy"].ToString();
                //B2B Invoices
                String b2b_ttl_rec = obj["data"]["sec_sum"][0]["ttl_rec"].ToString();
                String b2b_ttl_val = obj["data"]["sec_sum"][0]["ttl_val"].ToString();
                String b2b_ttl_tax = obj["data"]["sec_sum"][0]["ttl_tax"].ToString();
                String b2b_ttl_igst = obj["data"]["sec_sum"][0]["ttl_igst"].ToString();
                String b2b_ttl_cgst = obj["data"]["sec_sum"][0]["ttl_cgst"].ToString();
                String b2b_ttl_sgst = obj["data"]["sec_sum"][0]["ttl_sgst"].ToString();
                String b2b_ttl_cess = obj["data"]["sec_sum"][0]["ttl_cess"].ToString();
                //(Large) Invoices
                String b2cl_ttl_rec = obj["data"]["sec_sum"][2]["ttl_rec"].ToString();
                String b2cl_ttl_val = obj["data"]["sec_sum"][2]["ttl_val"].ToString();
                String b2cl_ttl_tax = obj["data"]["sec_sum"][2]["ttl_tax"].ToString();
                String b2cl_ttl_igst = obj["data"]["sec_sum"][2]["ttl_igst"].ToString();
                String b2cl_ttl_cess = obj["data"]["sec_sum"][2]["ttl_cess"].ToString();
                //----Credit/DebitNotesregistered
                String cndr_ttl_rec = obj["data"]["sec_sum"][6]["ttl_rec"].ToString();
                String cndr_ttl_val = obj["data"]["sec_sum"][6]["ttl_val"].ToString();
                String cndr_ttl_tax = obj["data"]["sec_sum"][6]["ttl_tax"].ToString();
                String cndr_ttl_igst = obj["data"]["sec_sum"][6]["ttl_igst"].ToString();
                String cndr_ttl_cgst = obj["data"]["sec_sum"][6]["ttl_cgst"].ToString();
                String cndr_ttl_sgst = obj["data"]["sec_sum"][6]["ttl_sgst"].ToString();
                String cndr_ttl_cess = obj["data"]["sec_sum"][6]["ttl_cess"].ToString();
                //--UnRegistered
                String cndur_ttl_rec = obj["data"]["sec_sum"][8]["ttl_rec"].ToString();
                String cndur_ttl_val = obj["data"]["sec_sum"][8]["ttl_val"].ToString();
                String cndur_ttl_tax = obj["data"]["sec_sum"][8]["ttl_tax"].ToString();
                String cndur_ttl_igst = obj["data"]["sec_sum"][8]["ttl_igst"].ToString();
                String cndur_ttl_cess = obj["data"]["sec_sum"][8]["ttl_cess"].ToString();
                //--6A-Exports Invoices
                String exp_ttl_rec = obj["data"]["sec_sum"][10]["ttl_rec"].ToString();
                String exp_ttl_val = obj["data"]["sec_sum"][10]["ttl_val"].ToString();
                String exp_ttl_tax = obj["data"]["sec_sum"][10]["ttl_tax"].ToString();
                String exp_ttl_igst = obj["data"]["sec_sum"][10]["ttl_igst"].ToString();
                //---7-B2C Others
                String b2cs_ttl_rec = obj["data"]["sec_sum"][4]["ttl_rec"].ToString();
                String b2cs_ttl_val = obj["data"]["sec_sum"][4]["ttl_val"].ToString();
                String b2cs_ttl_tax = obj["data"]["sec_sum"][4]["ttl_tax"].ToString();
                String b2cs_ttl_igst = obj["data"]["sec_sum"][4]["ttl_igst"].ToString();
                String b2cs_ttl_cgst = obj["data"]["sec_sum"][4]["ttl_cgst"].ToString();
                String b2cs_ttl_sgst = obj["data"]["sec_sum"][4]["ttl_sgst"].ToString();
                String b2cs_ttl_cess = obj["data"]["sec_sum"][4]["ttl_cess"].ToString();
                //----Nill Rated
                String nil_ttl_rec = obj["data"]["sec_sum"][16]["ttl_rec"].ToString();
                String nil_ttl_nilsup_amt = obj["data"]["sec_sum"][16]["ttl_nilsup_amt"].ToString();
                String nil_ttl_expt_amt = obj["data"]["sec_sum"][16]["ttl_expt_amt"].ToString();
                String nil_ttl_ngsup_amt = obj["data"]["sec_sum"][16]["ttl_ngsup_amt"].ToString();
                //---Tax Laibility
                String txpd_ttl_rec = obj["data"]["sec_sum"][14]["ttl_rec"].ToString();
                String txpd_ttl_val = obj["data"]["sec_sum"][14]["ttl_val"].ToString();
                String txpd_ttl_tax = obj["data"]["sec_sum"][14]["ttl_tax"].ToString();
                String txpd_ttl_igst = obj["data"]["sec_sum"][14]["ttl_igst"].ToString();
                String txpd_ttl_cgst = obj["data"]["sec_sum"][14]["ttl_cgst"].ToString();
                String txpd_ttl_sgst = obj["data"]["sec_sum"][14]["ttl_sgst"].ToString();
                String txpd_ttl_cess = obj["data"]["sec_sum"][14]["ttl_cess"].ToString();
                //Adjustment of Advances
                String at_ttl_rec = obj["data"]["sec_sum"][12]["ttl_rec"].ToString();
                String at_ttl_val = obj["data"]["sec_sum"][12]["ttl_val"].ToString();
                String at_ttl_tax = obj["data"]["sec_sum"][12]["ttl_tax"].ToString();
                String at_ttl_igst = obj["data"]["sec_sum"][12]["ttl_igst"].ToString();
                String at_ttl_cgst = obj["data"]["sec_sum"][12]["ttl_cgst"].ToString();
                String at_ttl_sgst = obj["data"]["sec_sum"][12]["ttl_sgst"].ToString();
                String at_ttl_cess = obj["data"]["sec_sum"][12]["ttl_cess"].ToString();

                //---HSN summary
                String hsn_ttl_rec = obj["data"]["sec_sum"][17]["ttl_rec"].ToString();
                String hsn_ttl_val = obj["data"]["sec_sum"][17]["ttl_val"].ToString();
                String hsn_ttl_tax = obj["data"]["sec_sum"][17]["ttl_tax"].ToString();
                String hsn_ttl_igst = obj["data"]["sec_sum"][17]["ttl_igst"].ToString();
                String hsn_ttl_cgst = obj["data"]["sec_sum"][17]["ttl_cgst"].ToString();
                String hsn_ttl_sgst = obj["data"]["sec_sum"][17]["ttl_sgst"].ToString();
                String hsn_ttl_cess = obj["data"]["sec_sum"][17]["ttl_cess"].ToString();
                //---Document Issued
                String doc_issue_ttl_rec = obj["data"]["sec_sum"][18]["ttl_rec"].ToString();
                String doc_issue_ttl_doc_issued = obj["data"]["sec_sum"][18]["ttl_doc_issued"].ToString();
                String doc_issue_ttl_doc_cancelled = obj["data"]["sec_sum"][18]["ttl_doc_cancelled"].ToString();
                String doc_issue_net_doc_issued = obj["data"]["sec_sum"][18]["net_doc_issued"].ToString();
                //---AmendementB2BInvoices
                String b2ba_ttl_rec = obj["data"]["sec_sum"][1]["ttl_rec"].ToString();
                String b2ba_ttl_val = obj["data"]["sec_sum"][1]["ttl_val"].ToString();
                String b2ba_ttl_tax = obj["data"]["sec_sum"][1]["ttl_tax"].ToString();
                String b2ba_ttl_igst = obj["data"]["sec_sum"][1]["ttl_igst"].ToString();
                String b2ba_ttl_cgst = obj["data"]["sec_sum"][1]["ttl_cgst"].ToString();
                String b2ba_ttl_sgst = obj["data"]["sec_sum"][1]["ttl_sgst"].ToString();
                String b2ba_ttl_cess = obj["data"]["sec_sum"][1]["ttl_cess"].ToString();
                //---AmendementB2C(Large)Invoices
                String b2cla_ttl_rec = obj["data"]["sec_sum"][3]["ttl_rec"].ToString();
                String b2cla_ttl_val = obj["data"]["sec_sum"][3]["ttl_val"].ToString();
                String b2cla_ttl_tax = obj["data"]["sec_sum"][3]["ttl_tax"].ToString();
                String b2cla_ttl_igst = obj["data"]["sec_sum"][3]["ttl_igst"].ToString();
                String b2cla_ttl_cess = obj["data"]["sec_sum"][3]["ttl_cess"].ToString();
                //---AmendementCdt(Regesitered)Invoices
                String cdnra_ttl_rec = obj["data"]["sec_sum"][7]["ttl_rec"].ToString();
                String cdnra_ttl_val = obj["data"]["sec_sum"][7]["ttl_val"].ToString();
                String cdnra_ttl_tax = obj["data"]["sec_sum"][7]["ttl_tax"].ToString();
                String cdnra_ttl_igst = obj["data"]["sec_sum"][7]["ttl_igst"].ToString();
                String cdnra_ttl_cgst = obj["data"]["sec_sum"][7]["ttl_cgst"].ToString();
                String cdnra_ttl_sgst = obj["data"]["sec_sum"][7]["ttl_sgst"].ToString();
                String cdnra_ttl_cess = obj["data"]["sec_sum"][7]["ttl_cess"].ToString();
                //---AmendementCdt(UnRegesitered)Invoices
                String cdnura_ttl_rec = obj["data"]["sec_sum"][9]["ttl_rec"].ToString();
                String cdnura_ttl_val = obj["data"]["sec_sum"][9]["ttl_val"].ToString();
                String cdnura_ttl_tax = obj["data"]["sec_sum"][9]["ttl_tax"].ToString();
                String cdnura_ttl_igst = obj["data"]["sec_sum"][9]["ttl_igst"].ToString();
                String cdnura_ttl_cess = obj["data"]["sec_sum"][9]["ttl_cess"].ToString();
                //---AmendementCdt(Export)Invoices
                String expa_ttl_rec = obj["data"]["sec_sum"][11]["ttl_rec"].ToString();
                String expa_ttl_val = obj["data"]["sec_sum"][11]["ttl_val"].ToString();
                String expa_ttl_tax = obj["data"]["sec_sum"][11]["ttl_tax"].ToString();
                String expa_ttl_igst = obj["data"]["sec_sum"][11]["ttl_igst"].ToString();
                //---AmendementCdtB2COthers
                String b2csa_ttl_rec = obj["data"]["sec_sum"][5]["ttl_rec"].ToString();
                String b2csa_ttl_val = obj["data"]["sec_sum"][5]["ttl_val"].ToString();
                String b2csa_ttl_tax = obj["data"]["sec_sum"][5]["ttl_tax"].ToString();
                String b2csa_ttl_igst = obj["data"]["sec_sum"][5]["ttl_igst"].ToString();
                String b2csa_ttl_cgst = obj["data"]["sec_sum"][5]["ttl_cgst"].ToString();
                String b2csa_ttl_sgst = obj["data"]["sec_sum"][5]["ttl_sgst"].ToString();
                String b2csa_ttl_cess = obj["data"]["sec_sum"][5]["ttl_cess"].ToString();
                //---AmendementTaxLibility
                String txpda_ttl_rec = obj["data"]["sec_sum"][15]["ttl_rec"].ToString();
                String txpda_ttl_val = obj["data"]["sec_sum"][15]["ttl_val"].ToString();
                String txpda_ttl_tax = obj["data"]["sec_sum"][15]["ttl_tax"].ToString();
                String txpda_ttl_igst = obj["data"]["sec_sum"][15]["ttl_igst"].ToString();
                String txpda_ttl_cgst = obj["data"]["sec_sum"][15]["ttl_cgst"].ToString();
                String txpda_ttl_sgst = obj["data"]["sec_sum"][15]["ttl_sgst"].ToString();
                String txpda_ttl_cess = obj["data"]["sec_sum"][15]["ttl_cess"].ToString();
                //---AmendementTaxadjustment

                String ata_ttl_rec = obj["data"]["sec_sum"][12]["ttl_rec"].ToString();
                String ata_ttl_val = obj["data"]["sec_sum"][12]["ttl_val"].ToString();
                String ata_ttl_tax = obj["data"]["sec_sum"][12]["ttl_tax"].ToString();
                String ata_ttl_igst = obj["data"]["sec_sum"][12]["ttl_igst"].ToString();
                String ata_ttl_cgst = obj["data"]["sec_sum"][12]["ttl_cgst"].ToString();
                String ata_ttl_sgst = obj["data"]["sec_sum"][12]["ttl_sgst"].ToString();
                String ata_ttl_cess = obj["data"]["sec_sum"][12]["ttl_cess"].ToString();


                //--------DateTime
                #region
                //---
                PdfPTable datetime = new PdfPTable(2);

                PdfPCell celldate = new PdfPCell(new Phrase("Row 1 , Col 1, Col 2 and col 3"));
                celldate.Colspan = 1;
                celldate.HorizontalAlignment = Element.ALIGN_RIGHT;
                //datetime.AddCell(celldate);

                datetime.AddCell("Financial Year");
                datetime.AddCell(fy);


                datetime.WidthPercentage = 30;
                datetime.HorizontalAlignment = Element.ALIGN_RIGHT;

                #endregion

                //--------DateTime
                #region
                //---
                PdfPTable GSTIN = new PdfPTable(2);

                PdfPCell cellGSTIN = new PdfPCell(new Phrase("Row 1 , Col 1, Col 2 and col 3"));
                GSTIN.TotalWidth = 950f;
                // Inward.LockedWidth = true;
                float[] widthGST = new float[] { 700f, 250f };
                GSTIN.SetWidths(widthGST);
                cellGSTIN.Colspan = 1;
                cellGSTIN.HorizontalAlignment = Element.ALIGN_RIGHT;
                //datetime.AddCell(celldate);

                GSTIN.AddCell("1.GSTIN");
                GSTIN.AddCell("");


                GSTIN.AddCell("2(a).Legal Name of the registered Person");
                GSTIN.AddCell("");

                GSTIN.AddCell("2(b).Trade Name ,If any");
                GSTIN.AddCell("");


                GSTIN.AddCell("3(a).Aggregate Turnover in the preceding Financial Year");
                GSTIN.AddCell("");

                GSTIN.AddCell("3(b).Aggregate Turnover- April to June,2017");
                GSTIN.AddCell("");

                GSTIN.WidthPercentage = 100;
                GSTIN.HorizontalAlignment = Element.ALIGN_LEFT;

                #endregion

                //---B2BINVOICES
                #region
                //---B2BINVOICES
                PdfPTable B2BInvoices = new PdfPTable(7);

                PdfPCell cellB2B = new PdfPCell(new Phrase("Row 1 , Col 1, Col 2 and col 3"));
                B2BInvoices.TotalWidth = 1750f;
                // Inward.LockedWidth = true;
                float[] widthB2B = new float[] { 250f, 250f, 250f, 250f, 250f, 250f, 250f };
                B2BInvoices.SetWidths(widthB2B);
                cellGSTIN.Colspan = 1;
                cellB2B.HorizontalAlignment = 1;
                

                B2BInvoices.AddCell("No. of Records");
                B2BInvoices.AddCell("Total Invoice Value");
                B2BInvoices.AddCell("Total Taxable Value");
                B2BInvoices.AddCell("Total Integrated  Tax");
                B2BInvoices.AddCell("Total Central  Tax");
                B2BInvoices.AddCell("Total State/UT  Tax");
                B2BInvoices.AddCell("Total Cess");


                
                B2BInvoices.AddCell(b2b_ttl_rec);
                B2BInvoices.AddCell(b2b_ttl_val);
                B2BInvoices.AddCell(b2b_ttl_tax);
                B2BInvoices.AddCell(b2b_ttl_igst);
                B2BInvoices.AddCell(b2b_ttl_cgst);
                B2BInvoices.AddCell(b2b_ttl_sgst);
                B2BInvoices.AddCell(b2b_ttl_cess);


                B2BInvoices.WidthPercentage = 100;
                cellB2B.HorizontalAlignment = Element.ALIGN_CENTER;

                #endregion
                //---Large Invoices
                #region
                //---Credit/Debit(Registered)
                //var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                var normalFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 6);
                PdfPTable LargeInvoices = new PdfPTable(5);

                PdfPCell cellLarge = new PdfPCell(new Phrase("Row 1 , Col 1, Col 2 and col 3"));
                LargeInvoices.TotalWidth = 1750f;
                // Inward.LockedWidth = true;
                float[] widthlarge = new float[] { 250f, 250f, 250f, 250f, 250f };
                LargeInvoices.SetWidths(widthlarge);
                cellLarge.Colspan = 1;
                cellLarge.HorizontalAlignment = Element.ALIGN_RIGHT;
                //datetime.AddCell(celldate);

                LargeInvoices.AddCell("No. of Records");
                LargeInvoices.AddCell("Total Invoice Value");
                LargeInvoices.AddCell("Total Taxable Value");
                LargeInvoices.AddCell("Total Integrated  Tax");
                LargeInvoices.AddCell("Total Cess");


                LargeInvoices.AddCell(b2cl_ttl_rec);
                LargeInvoices.AddCell(b2cl_ttl_val);
                LargeInvoices.AddCell(b2cl_ttl_tax);
                LargeInvoices.AddCell(b2cl_ttl_igst);
                LargeInvoices.AddCell(b2cl_ttl_cess);


                LargeInvoices.WidthPercentage = 70;
                LargeInvoices.HorizontalAlignment = Element.ALIGN_LEFT;

                #endregion
                //----Credit/DebitNotesregistered
                #region

                PdfPTable CreditReg = new PdfPTable(7);

                PdfPCell cellCredit = new PdfPCell(new Phrase("Row 1 , Col 1, Col 2 and col 3"));
                CreditReg.TotalWidth = 1750f;
                // Inward.LockedWidth = true;
                float[] widthCredit = new float[] { 250f, 250f, 250f, 250f, 250f, 250f, 250f };
                CreditReg.SetWidths(widthCredit);
                cellCredit.Colspan = 1;
                cellCredit.HorizontalAlignment = Element.ALIGN_RIGHT;
                //datetime.AddCell(celldate);

                CreditReg.AddCell("No. of Records");
                CreditReg.AddCell("Total Invoice Value");
                CreditReg.AddCell("Total Taxable Value");
                CreditReg.AddCell("Total Integrated  Tax");
                CreditReg.AddCell("Total Central  Tax");
                CreditReg.AddCell("Total State/UT  Tax");
                CreditReg.AddCell("Total Cess");

                CreditReg.AddCell(cndr_ttl_rec);
                CreditReg.AddCell(cndr_ttl_val);
                CreditReg.AddCell(cndr_ttl_tax);
                CreditReg.AddCell(cndr_ttl_igst);
                CreditReg.AddCell(cndr_ttl_cgst);
                CreditReg.AddCell(cndr_ttl_sgst);
                CreditReg.AddCell(cndr_ttl_cess);


                CreditReg.WidthPercentage = 100;
                CreditReg.HorizontalAlignment = Element.ALIGN_LEFT;

                #endregion
                //--UnRegistered
                #region

                PdfPTable CreditUnReg = new PdfPTable(5);

                PdfPCell cellUNCredit = new PdfPCell(new Phrase("Row 1 , Col 1, Col 2 and col 3"));
                CreditUnReg.TotalWidth = 1750f;
                // Inward.LockedWidth = true;
                float[] widthUNCredit = new float[] { 250f, 250f, 250f, 250f, 250f };
                CreditUnReg.SetWidths(widthUNCredit);
                cellUNCredit.Colspan = 1;
                cellUNCredit.HorizontalAlignment = Element.ALIGN_RIGHT;
                //datetime.AddCell(celldate);

                CreditUnReg.AddCell("No. of Records");
                CreditUnReg.AddCell("Total Invoice Value");
                CreditUnReg.AddCell("Total Taxable Value");
                CreditUnReg.AddCell("Total Integrated  Tax");
                CreditUnReg.AddCell("Total Cess");


                CreditUnReg.AddCell(cndur_ttl_rec);
                CreditUnReg.AddCell(cndur_ttl_val);
                CreditUnReg.AddCell(cndur_ttl_tax);
                CreditUnReg.AddCell(cndur_ttl_igst);
                CreditUnReg.AddCell(cndur_ttl_cess);



                CreditUnReg.WidthPercentage = 70;
                CreditUnReg.HorizontalAlignment = Element.ALIGN_LEFT;

                #endregion
                //--6A-Exports Invoices
                #region

                PdfPTable ExportInvoices = new PdfPTable(4);

                PdfPCell cellExport = new PdfPCell(new Phrase("Row 1 , Col 1, Col 2 and col 3"));
                ExportInvoices.TotalWidth = 1000f;
                // Inward.LockedWidth = true;
                float[] widthExport = new float[] { 250f, 250f, 250f, 250f };
                ExportInvoices.SetWidths(widthExport);
                cellExport.Colspan = 1;
                cellExport.HorizontalAlignment = Element.ALIGN_RIGHT;
                //datetime.AddCell(celldate);

                ExportInvoices.AddCell("No. of Records");
                ExportInvoices.AddCell("Total Invoice Value");
                ExportInvoices.AddCell("Total Taxable Value");
                ExportInvoices.AddCell("Total Integrated  Tax");

                ExportInvoices.AddCell(exp_ttl_rec);
                ExportInvoices.AddCell(exp_ttl_val);
                ExportInvoices.AddCell(exp_ttl_tax);
                ExportInvoices.AddCell(exp_ttl_igst);



                ExportInvoices.WidthPercentage = 60;
                ExportInvoices.HorizontalAlignment = Element.ALIGN_LEFT;

                #endregion
                //---7-B2C Others
                #region

                PdfPTable B2COTHERS = new PdfPTable(7);

                PdfPCell cellB2C = new PdfPCell(new Phrase("Row 1 , Col 1, Col 2 and col 3"));
                B2COTHERS.TotalWidth = 1750f;
                // Inward.LockedWidth = true;
                float[] widthB2C = new float[] { 250f, 250f, 250f, 250f, 250f, 250f, 250f };
                ExportInvoices.SetWidths(widthExport);
                cellB2C.Colspan = 1;
                cellB2C.HorizontalAlignment = Element.ALIGN_RIGHT;
                //datetime.AddCell(celldate);

                B2COTHERS.AddCell("No. of Records");
                B2COTHERS.AddCell("Total Invoice Value");
                B2COTHERS.AddCell("Total Taxable Value");
                B2COTHERS.AddCell("Total Integrated  Tax");
                B2COTHERS.AddCell("Total Central Tax");
                B2COTHERS.AddCell("Total State/UT Tax");
                B2COTHERS.AddCell("Total Cess");


                B2COTHERS.AddCell(b2cs_ttl_rec);
                B2COTHERS.AddCell(b2cs_ttl_rec);
                B2COTHERS.AddCell(b2cs_ttl_rec);
                B2COTHERS.AddCell(b2cs_ttl_rec);
                B2COTHERS.AddCell(b2cs_ttl_rec);
                B2COTHERS.AddCell(b2cs_ttl_rec);
                B2COTHERS.AddCell(b2cs_ttl_rec);


                B2COTHERS.WidthPercentage = 100;
                B2COTHERS.HorizontalAlignment = Element.ALIGN_LEFT;

                #endregion
                //----Nill Rated
                #region

                PdfPTable NillRated = new PdfPTable(4);

                PdfPCell cellNill = new PdfPCell(new Phrase("Row 1 , Col 1, Col 2 and col 3"));
                NillRated.TotalWidth = 1000f;
                // Inward.LockedWidth = true;
                float[] widthNill = new float[] { 150f, 150f, 150f, 150f };
                NillRated.SetWidths(widthNill);
                cellB2C.Colspan = 1;
                cellB2C.HorizontalAlignment = Element.ALIGN_RIGHT;
                //datetime.AddCell(celldate);

                NillRated.AddCell("No. of Records");
                NillRated.AddCell("Total Nill Amount");
                NillRated.AddCell("Total Exampted Amount");
                NillRated.AddCell("Total Non-GST Amount");


                NillRated.AddCell(nil_ttl_rec);
                NillRated.AddCell(nil_ttl_nilsup_amt);
                NillRated.AddCell(nil_ttl_expt_amt);
                NillRated.AddCell(nil_ttl_ngsup_amt);




                NillRated.WidthPercentage = 60;
                NillRated.HorizontalAlignment = Element.ALIGN_LEFT;

                #endregion
                //---Tax Laibility
                #region

                PdfPTable TaxLibility = new PdfPTable(7);

                PdfPCell cellTax = new PdfPCell(new Phrase("Row 1 , Col 1, Col 2 and col 3"));
                TaxLibility.TotalWidth = 1750f;
                // Inward.LockedWidth = true;
                float[] widthTax = new float[] { 250f, 250f, 250f, 250f, 250f, 250f, 250f };
                TaxLibility.SetWidths(widthTax);
                cellTax.Colspan = 1;
                cellTax.HorizontalAlignment = Element.ALIGN_RIGHT;
                //datetime.AddCell(celldate);

                TaxLibility.AddCell("No. of Records");
                TaxLibility.AddCell("Total Invoice  Value ");
                TaxLibility.AddCell("Total Taxable  value");
                TaxLibility.AddCell("Total Integrated tax");
                TaxLibility.AddCell("Total Central tax");
                TaxLibility.AddCell("Total State/UT Tax");
                TaxLibility.AddCell("Total Cess");


                TaxLibility.AddCell(txpd_ttl_rec);
                TaxLibility.AddCell(txpd_ttl_val);
                TaxLibility.AddCell(txpd_ttl_tax);
                TaxLibility.AddCell(txpd_ttl_igst);
                TaxLibility.AddCell(txpd_ttl_cgst);
                TaxLibility.AddCell(txpd_ttl_sgst);
                TaxLibility.AddCell(txpd_ttl_cess);


                TaxLibility.WidthPercentage = 100;
                TaxLibility.HorizontalAlignment = Element.ALIGN_LEFT;

                #endregion
                //---Adjustment of advances
                #region

                PdfPTable adjustment = new PdfPTable(7);

                PdfPCell celladjustment = new PdfPCell(new Phrase("Row 1 , Col 1, Col 2 and col 3"));
                adjustment.TotalWidth = 1750f;
                // Inward.LockedWidth = true;
                float[] widthadjustment = new float[] { 250f, 250f, 250f, 250f, 250f, 250f, 250f };
                adjustment.SetWidths(widthadjustment);
                celladjustment.Colspan = 1;
                celladjustment.HorizontalAlignment = Element.ALIGN_RIGHT;
                //datetime.AddCell(celldate);

                adjustment.AddCell("No. of Records");
                adjustment.AddCell("Total Invoice  Value ");
                adjustment.AddCell("Total Taxable  value");
                adjustment.AddCell("Total Integrated tax");
                adjustment.AddCell("Total Central tax");
                adjustment.AddCell("Total State/UT Tax");
                adjustment.AddCell("Total Cess");


                adjustment.AddCell(at_ttl_rec);
                adjustment.AddCell(at_ttl_val);
                adjustment.AddCell(at_ttl_tax);
                adjustment.AddCell(at_ttl_igst);
                adjustment.AddCell(at_ttl_cgst);
                adjustment.AddCell(at_ttl_sgst);
                adjustment.AddCell(at_ttl_cess);


                adjustment.WidthPercentage = 100;
                adjustment.HorizontalAlignment = Element.ALIGN_LEFT;

                #endregion
                //---HSN summary
                #region

                PdfPTable HSNSummary = new PdfPTable(7);

                PdfPCell cellHSN = new PdfPCell(new Phrase("Row 1 , Col 1, Col 2 and col 3"));
                adjustment.TotalWidth = 1750f;
                // Inward.LockedWidth = true;
                float[] widthHSN = new float[] { 250f, 250f, 250f, 250f, 250f, 250f, 250f };
                adjustment.SetWidths(widthHSN);
                cellHSN.Colspan = 1;
                cellHSN.HorizontalAlignment = Element.ALIGN_RIGHT;
                //datetime.AddCell(celldate);

                HSNSummary.AddCell("No. of Records");
                HSNSummary.AddCell("Total Invoice  Value ");
                HSNSummary.AddCell("Total Taxable  value");
                HSNSummary.AddCell("Total Integrated tax");
                HSNSummary.AddCell("Total Central tax");
                HSNSummary.AddCell("Total State/UT Tax");
                HSNSummary.AddCell("Total Cess");


                HSNSummary.AddCell(hsn_ttl_rec);
                HSNSummary.AddCell(hsn_ttl_val);
                HSNSummary.AddCell(hsn_ttl_tax);
                HSNSummary.AddCell(hsn_ttl_igst);
                HSNSummary.AddCell(hsn_ttl_cgst);
                HSNSummary.AddCell(hsn_ttl_sgst);
                HSNSummary.AddCell(hsn_ttl_cess);


                HSNSummary.WidthPercentage = 100;
                HSNSummary.HorizontalAlignment = Element.ALIGN_LEFT;

                #endregion
                //---Document Issued
                #region

                PdfPTable DocumentIssued = new PdfPTable(4);

                PdfPCell cellissued = new PdfPCell(new Phrase("Row 1 , Col 1, Col 2 and col 3"));
                DocumentIssued.TotalWidth = 1750f;
                // Inward.LockedWidth = true;
                float[] widthissued = new float[] { 250f, 250f, 250f, 250f };
                DocumentIssued.SetWidths(widthissued);
                cellissued.Colspan = 1;
                cellissued.HorizontalAlignment = Element.ALIGN_RIGHT;
                //datetime.AddCell(celldate);

                DocumentIssued.AddCell("No. of Records");
                DocumentIssued.AddCell("Document Issued ");
                DocumentIssued.AddCell("Document Cancelled");
                DocumentIssued.AddCell("Net Issued Documents");


                DocumentIssued.AddCell(doc_issue_ttl_rec);
                DocumentIssued.AddCell(doc_issue_ttl_doc_issued);
                DocumentIssued.AddCell(doc_issue_ttl_doc_cancelled);
                DocumentIssued.AddCell(doc_issue_net_doc_issued);


                DocumentIssued.WidthPercentage = 60;
                DocumentIssued.HorizontalAlignment = Element.ALIGN_LEFT;

                #endregion
                //---AmendementB2BInvoices
                #region

                PdfPTable B2BAmendement = new PdfPTable(7);

                PdfPCell cellamendemnet = new PdfPCell(new Phrase("Row 1 , Col 1, Col 2 and col 3"));
                B2BAmendement.TotalWidth = 1750f;
                // Inward.LockedWidth = true;
                float[] widthb2bamdmt = new float[] { 250f, 250f, 250f, 250f, 250f, 250f, 250f };
                B2BAmendement.SetWidths(widthb2bamdmt);
                cellamendemnet.Colspan = 1;
                cellamendemnet.HorizontalAlignment = Element.ALIGN_RIGHT;
                //datetime.AddCell(celldate);

                B2BAmendement.AddCell("No. of Records");
                B2BAmendement.AddCell("Total Invoice Value");
                B2BAmendement.AddCell("Total taxable Value");
                B2BAmendement.AddCell("Total Integrated Tax");
                B2BAmendement.AddCell("Total Central Tax");
                B2BAmendement.AddCell("Total State/UT Tax");
                B2BAmendement.AddCell("Total Cess");


                B2BAmendement.AddCell(b2ba_ttl_rec);
                B2BAmendement.AddCell(b2ba_ttl_val);
                B2BAmendement.AddCell(b2ba_ttl_tax);
                B2BAmendement.AddCell(b2ba_ttl_igst);
                B2BAmendement.AddCell(b2ba_ttl_cgst);
                B2BAmendement.AddCell(b2ba_ttl_sgst);
                B2BAmendement.AddCell(b2ba_ttl_cess);


                B2BAmendement.WidthPercentage = 100;
                B2BAmendement.HorizontalAlignment = Element.ALIGN_LEFT;

                #endregion
                //---AmendementB2C(Large)Invoices
                #region

                PdfPTable B2BLarge = new PdfPTable(5);

                PdfPCell cellB2BLarge = new PdfPCell(new Phrase("Row 1, Col 1, Col 2 and col 3"));
                B2BLarge.TotalWidth = 1750f;
                // Inward.LockedWidth = true;
                float[] widthB2BLarg = new float[] { 250f, 250f, 250f, 250f, 250f };
                B2BLarge.SetWidths(widthB2BLarg);
                cellB2BLarge.Colspan = 1;
                cellB2BLarge.HorizontalAlignment = Element.ALIGN_RIGHT;
                //datetime.AddCell(celldate);

                B2BLarge.AddCell("No. of Records");
                B2BLarge.AddCell("Total Invoice Value");
                B2BLarge.AddCell("Total taxable Value");
                B2BLarge.AddCell("Total Integrated Tax");
                B2BLarge.AddCell("Total Cess");


                B2BLarge.AddCell(b2cla_ttl_rec);
                B2BLarge.AddCell(b2cla_ttl_val);
                B2BLarge.AddCell(b2cla_ttl_tax);
                B2BLarge.AddCell(b2cla_ttl_igst);
                B2BLarge.AddCell(b2cla_ttl_cess);


                B2BLarge.WidthPercentage = 70;
                B2BLarge.HorizontalAlignment = Element.ALIGN_LEFT;

                #endregion
                //---AmendementCdt(Regesitered)Invoices
                #region

                PdfPTable Amendedcdtdbtregt = new PdfPTable(7);

                PdfPCell cellAmendedcdtdbtregt = new PdfPCell(new Phrase("Row 1 , Col 1, Col 2 and col 3"));
                Amendedcdtdbtregt.TotalWidth = 1750f;
                // Inward.LockedWidth = true;
                float[] widthAmendedcdtdbtregt = new float[] { 250f, 250f, 250f, 250f, 250f, 250f, 250f };
                Amendedcdtdbtregt.SetWidths(widthAmendedcdtdbtregt);
                cellAmendedcdtdbtregt.Colspan = 1;
                cellAmendedcdtdbtregt.HorizontalAlignment = Element.ALIGN_RIGHT;
                //datetime.AddCell(celldate);

                Amendedcdtdbtregt.AddCell("No. of Records");
                Amendedcdtdbtregt.AddCell("Total Invoice Value");
                Amendedcdtdbtregt.AddCell("Total taxable Value");
                Amendedcdtdbtregt.AddCell("Total Integrated Tax");
                Amendedcdtdbtregt.AddCell("Total Central Tax");
                Amendedcdtdbtregt.AddCell("Total State/UT Tax");
                Amendedcdtdbtregt.AddCell("Total Cess");


                Amendedcdtdbtregt.AddCell(cdnra_ttl_rec);
                Amendedcdtdbtregt.AddCell(cdnra_ttl_val);
                Amendedcdtdbtregt.AddCell(cdnra_ttl_tax);
                Amendedcdtdbtregt.AddCell(cdnra_ttl_igst);
                Amendedcdtdbtregt.AddCell(cdnra_ttl_cgst);
                Amendedcdtdbtregt.AddCell(cdnra_ttl_sgst);
                Amendedcdtdbtregt.AddCell(cdnra_ttl_cess);



                Amendedcdtdbtregt.WidthPercentage = 100;
                Amendedcdtdbtregt.HorizontalAlignment = Element.ALIGN_LEFT;

                #endregion
                //---AmendementCdt(UnRegesitered)Invoices
                #region

                PdfPTable AmendedcdtdbtUnregt = new PdfPTable(5);

                PdfPCell cellAmendedcdtdbUntregt = new PdfPCell(new Phrase("Row 1 , Col 1, Col 2 and col 3"));
                AmendedcdtdbtUnregt.TotalWidth = 1750f;
                // Inward.LockedWidth = true;
                float[] widthAmendedcdtdbtUnregt = new float[] { 250f, 250f, 250f, 250f, 250f };
                AmendedcdtdbtUnregt.SetWidths(widthAmendedcdtdbtUnregt);
                cellAmendedcdtdbUntregt.Colspan = 1;
                cellAmendedcdtdbUntregt.HorizontalAlignment = Element.ALIGN_RIGHT;
                //datetime.AddCell(celldate);

                AmendedcdtdbtUnregt.AddCell("No. of Records");
                AmendedcdtdbtUnregt.AddCell("Total Invoice Value");
                AmendedcdtdbtUnregt.AddCell("Total taxable Value");
                AmendedcdtdbtUnregt.AddCell("Total Integrated Tax");
                AmendedcdtdbtUnregt.AddCell("Total Cess");


                AmendedcdtdbtUnregt.AddCell(cdnura_ttl_rec);
                AmendedcdtdbtUnregt.AddCell(cdnura_ttl_val);
                AmendedcdtdbtUnregt.AddCell(cdnura_ttl_tax);
                AmendedcdtdbtUnregt.AddCell(cdnura_ttl_igst);
                AmendedcdtdbtUnregt.AddCell(cdnura_ttl_cess);


                AmendedcdtdbtUnregt.WidthPercentage = 70;
                AmendedcdtdbtUnregt.HorizontalAlignment = Element.ALIGN_LEFT;

                #endregion
                //---AmendementCdt(Export)Invoices
                #region

                PdfPTable Amendedexptinvoices = new PdfPTable(4);

                PdfPCell cellAmendedexptinvoices = new PdfPCell(new Phrase("Row 1 , Col 1, Col 2 and col 3"));
                Amendedexptinvoices.TotalWidth = 1750f;
                // Inward.LockedWidth = true;
                float[] widthAmendedexptinvoices = new float[] { 250f, 250f, 250f, 250f };
                Amendedexptinvoices.SetWidths(widthAmendedexptinvoices);
                cellAmendedexptinvoices.Colspan = 1;
                cellAmendedexptinvoices.HorizontalAlignment = Element.ALIGN_RIGHT;
                //datetime.AddCell(celldate);

                Amendedexptinvoices.AddCell("No. of Records");
                Amendedexptinvoices.AddCell("Total Invoice Value");
                Amendedexptinvoices.AddCell("Total taxable Value");
                Amendedexptinvoices.AddCell("Total Integrated Tax");


                Amendedexptinvoices.AddCell(expa_ttl_rec);
                Amendedexptinvoices.AddCell(expa_ttl_val);
                Amendedexptinvoices.AddCell(expa_ttl_tax);
                Amendedexptinvoices.AddCell(expa_ttl_igst);



                Amendedexptinvoices.WidthPercentage = 60;
                Amendedexptinvoices.HorizontalAlignment = Element.ALIGN_LEFT;

                #endregion
                //---AmendementCdtB2COthers
                #region

                PdfPTable AmendedB2COthers = new PdfPTable(7);

                PdfPCell cellAmendedB2COthers = new PdfPCell(new Phrase("Row 1 , Col 1, Col 2 and col 3"));
                AmendedB2COthers.TotalWidth = 1750f;
                // Inward.LockedWidth = true;
                float[] widthAmendedB2COthers = new float[] { 250f, 250f, 250f, 250f, 250f, 250f, 250f };
                AmendedB2COthers.SetWidths(widthAmendedB2COthers);
                cellAmendedB2COthers.Colspan = 1;
                cellAmendedB2COthers.HorizontalAlignment = Element.ALIGN_RIGHT;
                //datetime.AddCell(celldate);

                AmendedB2COthers.AddCell("No. of Records");
                AmendedB2COthers.AddCell("Total Invoice Value");
                AmendedB2COthers.AddCell("Total taxable Value");
                AmendedB2COthers.AddCell("Total Integrated Tax");
                AmendedB2COthers.AddCell("Total Central Tax");
                AmendedB2COthers.AddCell("Total State/UT Tax");
                AmendedB2COthers.AddCell("Total Cess");


                AmendedB2COthers.AddCell(b2csa_ttl_rec);
                AmendedB2COthers.AddCell(b2csa_ttl_val);
                AmendedB2COthers.AddCell(b2csa_ttl_tax);
                AmendedB2COthers.AddCell(b2csa_ttl_igst);
                AmendedB2COthers.AddCell(b2csa_ttl_cgst);
                AmendedB2COthers.AddCell(b2csa_ttl_sgst);
                AmendedB2COthers.AddCell(b2csa_ttl_cess);


                AmendedB2COthers.WidthPercentage = 100;
                AmendedB2COthers.HorizontalAlignment = Element.ALIGN_LEFT;

                #endregion
                //---AmendementTaxLibility
                #region

                PdfPTable AmendedTaxliblty = new PdfPTable(7);

                PdfPCell cellAmendedTaxliblty = new PdfPCell(new Phrase("Row 1 , Col 1, Col 2 and col 3"));
                AmendedTaxliblty.TotalWidth = 1750f;
                // Inward.LockedWidth = true;
                float[] widthAmendedTaxliblty = new float[] { 250f, 250f, 250f, 250f, 250f, 250f, 250f };
                AmendedTaxliblty.SetWidths(widthAmendedTaxliblty);
                cellAmendedTaxliblty.Colspan = 1;
                cellAmendedTaxliblty.HorizontalAlignment = Element.ALIGN_RIGHT;
                //datetime.AddCell(celldate);

                AmendedTaxliblty.AddCell("No. of Records");
                AmendedTaxliblty.AddCell("Total Invoice Value");
                AmendedTaxliblty.AddCell("Total taxable Value");
                AmendedTaxliblty.AddCell("Total Integrated Tax");
                AmendedTaxliblty.AddCell("Total Central Tax");
                AmendedTaxliblty.AddCell("Total State/UT Tax");
                AmendedTaxliblty.AddCell("Total Cess");


                AmendedTaxliblty.AddCell(txpda_ttl_rec);
                AmendedTaxliblty.AddCell(txpda_ttl_val);
                AmendedTaxliblty.AddCell(txpda_ttl_tax);
                AmendedTaxliblty.AddCell(txpda_ttl_igst);
                AmendedTaxliblty.AddCell(txpda_ttl_cgst);
                AmendedTaxliblty.AddCell(txpda_ttl_sgst);
                AmendedTaxliblty.AddCell(txpda_ttl_cess);


                AmendedTaxliblty.WidthPercentage = 100;
                AmendedTaxliblty.HorizontalAlignment = Element.ALIGN_LEFT;

                #endregion
                //---AmendementTaxadjustment
                #region

                PdfPTable Amendedadjustment = new PdfPTable(7);

                PdfPCell cellAmendedadjustment = new PdfPCell(new Phrase("Row 1 , Col 1, Col 2 and col 3"));
                Amendedadjustment.TotalWidth = 1750f;
                // Inward.LockedWidth = true;
                float[] widthAmendedadjustment = new float[] { 250f, 250f, 250f, 250f, 250f, 250f, 250f };
                Amendedadjustment.SetWidths(widthAmendedadjustment);
                cellAmendedTaxliblty.Colspan = 1;
                cellAmendedTaxliblty.HorizontalAlignment = Element.ALIGN_RIGHT;
                //datetime.AddCell(celldate);

                Amendedadjustment.AddCell("No. of Records");
                Amendedadjustment.AddCell("Total Invoice Value");
                Amendedadjustment.AddCell("Total taxable Value");
                Amendedadjustment.AddCell("Total Integrated Tax");
                Amendedadjustment.AddCell("Total Central Tax");
                Amendedadjustment.AddCell("Total State/UT Tax");
                Amendedadjustment.AddCell("Total Cess");


                Amendedadjustment.AddCell(ata_ttl_rec);
                Amendedadjustment.AddCell(ata_ttl_val);
                Amendedadjustment.AddCell(ata_ttl_tax);
                Amendedadjustment.AddCell(ata_ttl_igst);
                Amendedadjustment.AddCell(ata_ttl_cgst);
                Amendedadjustment.AddCell(ata_ttl_sgst);
                Amendedadjustment.AddCell(ata_ttl_cess);


                Amendedadjustment.WidthPercentage = 100;
                Amendedadjustment.HorizontalAlignment = Element.ALIGN_LEFT;

                #endregion



                var savefiledialog = new SaveFileDialog();

                if (savefiledialog.ShowDialog() == DialogResult.OK)
                {

                    Document document = new Document(PageSize.A4.Rotate(), 50, 50, 15, 15);


                    var output = new FileStream(savefiledialog.FileName, FileMode.Create);

                    PdfWriter writer = PdfWriter.GetInstance(document, output);

                    // Open the Document for writing

                    document.Open();


                    Paragraph welcomeParagraph = new Paragraph(" Form GSTR-1 ");
                    Paragraph report = new Paragraph("  See rule59[1]");
                    Paragraph date = new Paragraph("Details of Outward supplies of goods or services");
                    Paragraph sysgen = new Paragraph("System generated summary (For reference)");
                    Paragraph note = new Paragraph(" Note: All amounts displayed in the tables are in INR. ");
                    Paragraph B2B = new Paragraph("4A,4B,4C,6B,6C-B2B Invoices", normalFont);
                    Paragraph B2C = new Paragraph("5A,5B- B2C(Large Invoices)", normalFont);
                    Paragraph Registered = new Paragraph("9B- Credit/Debit Notes(Registered)", normalFont);
                    Paragraph UnRegistered = new Paragraph("9B- Credit/Debit Notes(UnRegistered)", normalFont);
                    Paragraph Export = new Paragraph("6A- Exports Invoices", normalFont);
                    Paragraph B2COthers = new Paragraph("7-B2C (Others)", normalFont);
                    Paragraph Nill = new Paragraph("8-Nill Rated ,Exempted and Non GST Outward supplies", normalFont);
                    Paragraph TaxLibty = new Paragraph("11A(1),11A(2)-Tax Liability(Advances Received) ", normalFont);
                    Paragraph adjustmentadvnc = new Paragraph("11B(1),11B(2)-adjustment of Advances ", normalFont);
                    Paragraph HSNSummarySupplies = new Paragraph("12-HSN-wise Summary of outward Supplies ", normalFont);
                    Paragraph Documentisud = new Paragraph("13-Document Issued ", normalFont);
                    Paragraph B2Bamedtinvoice = new Paragraph("9A- Amendement B2B Invoices ", normalFont);
                    Paragraph B2ClargeInvoices = new Paragraph("9A- Amendement B2C(large) Invoices ", normalFont);
                    Paragraph amdtcredtrgst = new Paragraph("9C- Amendement Credit/Debit Notes Registered ", normalFont);
                    Paragraph amdtcredtrungst = new Paragraph("9C- Amendement Credit/Debit Notes UnRegistered ", normalFont);
                    Paragraph amdtexptinvoices = new Paragraph("9A- Amendement Export Invoices ", normalFont);
                    Paragraph amdtB2COthers = new Paragraph("10- Amendement B2C (Others)", normalFont);
                    Paragraph amdtaxlibilty = new Paragraph("11A-Amended Tax Liability(Advance Received) ", normalFont);
                    Paragraph amdtamendtadvances = new Paragraph("11B- Amendement of Adjustment Of Advances", normalFont);


                    report.Alignment = Element.ALIGN_CENTER;
                    date.Alignment = Element.ALIGN_CENTER;
                    sysgen.Alignment = Element.ALIGN_CENTER;
                    welcomeParagraph.Alignment = Element.ALIGN_CENTER;


                    welcomeParagraph.Font.SetColor(242, 132, 0);
                    welcomeParagraph.Font.Size = 20;
                    report.Font.Size = 10;
                    B2B.Font.Size = 15;
                    B2C.Font.Size = 15;

                    document.Add(welcomeParagraph);

                    document.Add(new Paragraph("\n\n"));
                    document.Add(report);
                    document.Add(new Paragraph("\n"));
                    document.Add(date);
                    document.Add(new Paragraph("\n"));
                    document.Add(sysgen);
                    document.Add(new Paragraph("\n\n\n"));
                    document.Add(datetime);
                    document.Add(new Paragraph("\n"));
                    document.Add(GSTIN);
                    document.Add(new Paragraph("\n\n"));
                    document.Add(note);
                    document.Add(new Paragraph("\n\n"));
                    document.Add(B2B);
                    document.Add(new Paragraph("\n\n"));
                    document.Add(B2BInvoices);
                    document.Add(new Paragraph("\n"));
                    document.Add(B2C);
                    document.Add(new Paragraph("\n"));
                    document.Add(LargeInvoices);
                    document.Add(new Paragraph("\n"));
                    document.Add(Registered);
                    document.Add(new Paragraph("\n"));
                    document.Add(CreditReg);
                    document.Add(new Paragraph("\n"));
                    document.Add(UnRegistered);
                    document.Add(new Paragraph("\n"));
                    document.Add(CreditUnReg);
                    document.Add(new Paragraph("\n"));
                    document.Add(Export);
                    document.Add(new Paragraph("\n"));
                    document.Add(ExportInvoices);
                    document.Add(new Paragraph("\n"));
                    document.Add(B2COthers);
                    document.Add(new Paragraph("\n"));
                    document.Add(B2COTHERS);
                    document.Add(new Paragraph("\n\n\n"));
                    document.Add(Nill);
                    document.Add(new Paragraph("\n"));
                    document.Add(NillRated);
                    document.Add(new Paragraph("\n"));
                    document.Add(TaxLibty);
                    document.Add(new Paragraph("\n"));
                    document.Add(TaxLibility);
                    document.Add(new Paragraph("\n"));
                    document.Add(adjustmentadvnc);
                    document.Add(new Paragraph("\n"));
                    document.Add(adjustment);
                    document.Add(new Paragraph("\n"));
                    document.Add(HSNSummarySupplies);
                    document.Add(new Paragraph("\n"));
                    document.Add(HSNSummary);
                    document.Add(new Paragraph("\n"));
                    document.Add(Documentisud);
                    document.Add(new Paragraph("\n"));
                    document.Add(DocumentIssued);
                    document.Add(new Paragraph("\n\n\n"));
                    document.Add(B2Bamedtinvoice);
                    document.Add(new Paragraph("\n"));
                    document.Add(B2BAmendement);
                    document.Add(new Paragraph("\n"));
                    document.Add(B2ClargeInvoices);
                    document.Add(new Paragraph("\n"));
                    document.Add(B2BLarge);
                    document.Add(new Paragraph("\n"));
                    document.Add(amdtcredtrgst);
                    document.Add(new Paragraph("\n"));
                    document.Add(Amendedcdtdbtregt);
                    document.Add(new Paragraph("\n"));
                    document.Add(amdtcredtrungst);
                    document.Add(new Paragraph("\n"));
                    document.Add(AmendedcdtdbtUnregt);
                    document.Add(new Paragraph("\n"));
                    document.Add(amdtexptinvoices);
                    document.Add(new Paragraph("\n"));
                    document.Add(Amendedexptinvoices);
                    document.Add(new Paragraph("\n\n\n"));
                    document.Add(amdtB2COthers);
                    document.Add(new Paragraph("\n"));
                    document.Add(AmendedB2COthers);
                    document.Add(new Paragraph("\n"));
                    document.Add(amdtaxlibilty);
                    document.Add(new Paragraph("\n"));
                    document.Add(AmendedTaxliblty);
                    document.Add(new Paragraph("\n"));
                    document.Add(amdtamendtadvances);
                    document.Add(new Paragraph("\n"));
                    document.Add(Amendedadjustment);

                    document.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

            finally
            {
            }


        }

        private void GSTR_1_PDF_Load(object sender, EventArgs e)
        {

        }
    }
}
