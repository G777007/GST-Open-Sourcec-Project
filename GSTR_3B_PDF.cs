using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Proactive.CustomTools;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;




namespace SPEQTAGST_DESIGN
{
    public partial class GSTR_3B_PDF : Form
    {
        CookieContainer Cc = new CookieContainer();
        HttpWebResponse response;
        public GSTR_3B_PDF()
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

                    //https://return.gst.gov.in/returns2/auth/api/gstr9/details/calc?gstin=09AAHFC3214F1ZT&ret_period=032018

                    HttpWebRequest httpWebRequest = this.PrepareGetRequestTdsTcs(new Uri(string.Format("https://return.gst.gov.in/returns2/auth/api/gstr9/gstr3bsumm?gstin=09AAHFC3214F1ZT&ret_period=032018")), "https://return.gst.gov.in/returns2/auth/gstr9/dashboard");
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
            catch (Exception exception)
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


                String reply = "";
                DownloadLiveData(ref reply);
                String strjson = reply;
              
                JObject obj = JObject.Parse(strjson);
               
                //--------DateTime
                String fy = obj["data"]["fy"].ToString();

                //---Outward supplies and inward supplies
                String osup_det_txval = obj["data"]["sup_details"]["osup_det"]["txval"].ToString();
                String osup_det_iamt = obj["data"]["sup_details"]["osup_det"]["iamt"].ToString();
                String osup_det_camt = obj["data"]["sup_details"]["osup_det"]["camt"].ToString();
                String osup_det_samt = obj["data"]["sup_details"]["osup_det"]["samt"].ToString();
                String osup_det_csamt = obj["data"]["sup_details"]["osup_det"]["csamt"].ToString();

                String osup_zero_txval = obj["data"]["sup_details"]["osup_zero"]["txval"].ToString();
                String osup_zero_iamt = obj["data"]["sup_details"]["osup_zero"]["iamt"].ToString();
                String osup_zero_camt = obj["data"]["sup_details"]["osup_zero"]["camt"].ToString();
                String osup_zero_samt = obj["data"]["sup_details"]["osup_zero"]["samt"].ToString();
                String osup_zero_csamt = obj["data"]["sup_details"]["osup_zero"]["csamt"].ToString();

                String osup_nil_exmp_txval = obj["data"]["sup_details"]["osup_nil_exmp"]["txval"].ToString();
                String osup_nil_exmp_iamt = obj["data"]["sup_details"]["osup_nil_exmp"]["iamt"].ToString();
                String osup_nil_exmp_camt = obj["data"]["sup_details"]["osup_nil_exmp"]["camt"].ToString();
                String osup_nil_exmp_samt = obj["data"]["sup_details"]["osup_nil_exmp"]["samt"].ToString();
                String osup_nil_exmp_csamt = obj["data"]["sup_details"]["osup_nil_exmp"]["csamt"].ToString();

                String isup_rev_txval = obj["data"]["sup_details"]["isup_rev"]["txval"].ToString();
                String isup_rev_iamt = obj["data"]["sup_details"]["isup_rev"]["iamt"].ToString();
                String isup_rev_camt = obj["data"]["sup_details"]["isup_rev"]["camt"].ToString();
                String isup_rev_samt = obj["data"]["sup_details"]["isup_rev"]["samt"].ToString();
                String isup_rev_csamt = obj["data"]["sup_details"]["isup_rev"]["csamt"].ToString();

                String osup_nongst_txval = obj["data"]["sup_details"]["osup_nongst"]["txval"].ToString();
                String osup_nongst_iamt = obj["data"]["sup_details"]["osup_nongst"]["iamt"].ToString();
                String osup_nongst_camt = obj["data"]["sup_details"]["osup_nongst"]["camt"].ToString();
                String osup_nongst_samt = obj["data"]["sup_details"]["osup_nongst"]["samt"].ToString();
                String osup_nongst_csamt = obj["data"]["sup_details"]["osup_nongst"]["csamt"].ToString();

                //---3.2 Supplies made to Unregistered Persons
                //String unreg_details_txval = obj["data"]["inter_sup"]["unreg_details"][0]["txval"].ToString();
                //String unreg_details_iamt = obj["data"]["inter_sup"]["unreg_details"][0]["iamt"].ToString();

                // String comp_details_txval = obj["data"]["inter_sup"]["comp_details"][0]["txval"].ToString();
                //String comp_details_iamt = obj["data"]["inter_sup"]["comp_details"][0]["iamt"].ToString();

                //String uin_details_txval = obj["data"]["inter_sup"]["uin_details"][0]["txval"].ToString();
                //String uin_details_iamt = obj["data"]["inter_sup"]["uin_details"][0]["iamt"].ToString();


                //-----------------------------------------------------------------------------------------------------
                String unreg_details_txval = obj["data"]["inter_sup"]["unreg_details"].ToString();
                JArray arrunreg = JArray.Parse(unreg_details_txval);
                //JArray arrunreg1 = (JArray)obj["data"]["inter_sup"]["unreg_details"];
                decimal tot_unreg_details_txval = 0;
                for (int i = 0; i < arrunreg.Count; i++)
                {

                    string txval = arrunreg[i]["txval"].ToString();
                    tot_unreg_details_txval = tot_unreg_details_txval + Convert.ToDecimal(txval);

                }

                String unreg_details_iamt = obj["data"]["inter_sup"]["unreg_details"].ToString();
                JArray arrunregiamt = JArray.Parse(unreg_details_iamt);
                decimal tot_unreg_details_iamt = 0;
                for (int i = 0; i < arrunregiamt.Count; i++)
                {

                    string iamt = arrunregiamt[i]["iamt"].ToString();
                    tot_unreg_details_iamt = tot_unreg_details_iamt + Convert.ToDecimal(iamt);

                }

                //-------------------------------------------------------------------------------------------------------
                String comp_details_txval = obj["data"]["inter_sup"]["comp_details"].ToString();
                JArray arrunregcomptx = JArray.Parse(comp_details_txval);
                decimal tot_comp_details_txval = 0;
                for (int i = 0; i < arrunregcomptx.Count; i++)
                {

                    string txval = arrunregcomptx[i]["txval"].ToString();
                    tot_comp_details_txval = tot_comp_details_txval + Convert.ToDecimal(txval);

                }

                String comp_details_iamt = obj["data"]["inter_sup"]["comp_details"].ToString();
                JArray arrunregcompiamt = JArray.Parse(comp_details_iamt);
                decimal tot_comp_details_iamt = 0;
                for (int i = 0; i < arrunregcompiamt.Count; i++)
                {

                    string iamt = arrunregcompiamt[i]["iamt"].ToString();
                    tot_comp_details_iamt = tot_comp_details_iamt + Convert.ToDecimal(iamt);

                }



                //--------------------------------------------------------------------------------------------------

                String uin_details_txval = obj["data"]["inter_sup"]["uin_details"].ToString();
                JArray arrunreguintx = JArray.Parse(uin_details_txval);
                decimal tot_uin_details_txval = 0;
                for (int i = 0; i < arrunreguintx.Count; i++)
                {

                    string txval = arrunreguintx[i]["txval"].ToString();
                    tot_uin_details_txval = tot_uin_details_txval + Convert.ToDecimal(txval);

                }

                String uin_details_iamt = obj["data"]["inter_sup"]["uin_details"].ToString();
                JArray arrunreguiniamt = JArray.Parse(uin_details_iamt);
                decimal tot_uin_details_iamt = 0;
                for (int i = 0; i < arrunreguiniamt.Count; i++)
                {

                    string iamt = arrunreguiniamt[i]["iamt"].ToString();
                    tot_uin_details_iamt = tot_uin_details_iamt + Convert.ToDecimal(iamt);

                }

                //MessageBox.Show(tot_comp_details_txval.ToString("N2"));

                String tot_unreg_details_txval_1 = tot_unreg_details_txval.ToString();
                String tot_unreg_details_iamt_1 = tot_unreg_details_iamt.ToString();

                String tot_comp_details_txval_1 = tot_comp_details_txval.ToString();
                String tot_comp_details_iamt_1 = tot_comp_details_iamt.ToString();

                String tot_uin_details_txval_1 = tot_uin_details_txval.ToString();
                String tot_uin_details_iamt_1 = tot_uin_details_iamt.ToString();







                //---Eligible ITC
                String itc_avl_iamt = obj["data"]["itc_elg"]["itc_avl"][0]["iamt"].ToString();
                String itc_avl_camt = obj["data"]["itc_elg"]["itc_avl"][0]["camt"].ToString();
                String itc_avl_samt = obj["data"]["itc_elg"]["itc_avl"][0]["samt"].ToString();
                String itc_avl_csamt = obj["data"]["itc_elg"]["itc_avl"][0]["csamt"].ToString();

                String itc_avl1_iamt = obj["data"]["itc_elg"]["itc_avl"][1]["iamt"].ToString();
                String itc_avl1_camt = obj["data"]["itc_elg"]["itc_avl"][1]["camt"].ToString();
                String itc_avl1_samt = obj["data"]["itc_elg"]["itc_avl"][1]["samt"].ToString();
                String itc_avl1_csamt = obj["data"]["itc_elg"]["itc_avl"][1]["csamt"].ToString();

                String itc_avl2_iamt = obj["data"]["itc_elg"]["itc_avl"][2]["iamt"].ToString();
                String itc_avl2_camt = obj["data"]["itc_elg"]["itc_avl"][2]["camt"].ToString();
                String itc_avl2_samt = obj["data"]["itc_elg"]["itc_avl"][2]["samt"].ToString();
                String itc_avl2_csamt = obj["data"]["itc_elg"]["itc_avl"][2]["csamt"].ToString();

                String itc_avl3_iamt = obj["data"]["itc_elg"]["itc_avl"][3]["iamt"].ToString();
                String itc_avl3_camt = obj["data"]["itc_elg"]["itc_avl"][3]["camt"].ToString();
                String itc_avl3_samt = obj["data"]["itc_elg"]["itc_avl"][3]["samt"].ToString();
                String itc_avl3_csamt = obj["data"]["itc_elg"]["itc_avl"][3]["csamt"].ToString();

                String itc_avl4_iamt = obj["data"]["itc_elg"]["itc_avl"][4]["iamt"].ToString();
                String itc_avl4_camt = obj["data"]["itc_elg"]["itc_avl"][4]["camt"].ToString();
                String itc_avl4_samt = obj["data"]["itc_elg"]["itc_avl"][4]["samt"].ToString();
                String itc_avl4_csamt = obj["data"]["itc_elg"]["itc_avl"][4]["csamt"].ToString();


                int x1 = Convert.ToInt32(itc_avl_iamt);
                int x2 = Convert.ToInt32(itc_avl1_iamt);
                int x3 = Convert.ToInt32(itc_avl2_iamt);
                int x4 = Convert.ToInt32(itc_avl3_iamt);
                int x5 = Convert.ToInt32(itc_avl4_iamt);
                int total1 = x1 + x2 + x3 + x4 + x5;
                String ftotal1 = total1.ToString();

                int x6 = Convert.ToInt32(itc_avl_camt);
                int x7 = Convert.ToInt32(itc_avl1_camt);
                int x8 = Convert.ToInt32(itc_avl2_camt);
                int x9 = Convert.ToInt32(itc_avl3_camt);
                int x10 = Convert.ToInt32(itc_avl4_camt);
                int total2 = x6 + x7 + x8 + x9 + x10;
                String ftotal2 = total2.ToString();

                int x11 = Convert.ToInt32(itc_avl_samt);
                int x12 = Convert.ToInt32(itc_avl1_samt);
                int x13 = Convert.ToInt32(itc_avl2_samt);
                int x14 = Convert.ToInt32(itc_avl3_samt);
                int x15 = Convert.ToInt32(itc_avl4_samt);
                int total3 = x11 + x12 + x13 + x14 + x15;
                String ftotal3 = total3.ToString();

                int x16 = Convert.ToInt32(itc_avl_samt);
                int x17 = Convert.ToInt32(itc_avl1_samt);
                int x18 = Convert.ToInt32(itc_avl2_samt);
                int x19 = Convert.ToInt32(itc_avl3_samt);
                int x20 = Convert.ToInt32(itc_avl4_samt);
                int total4 = x16 + x17 + x18 + x19 + x20;
                String ftotal4 = total4.ToString();

                //---B. ITC Reversed
                String itc_rev1_iamt = obj["data"]["itc_elg"]["itc_rev"][1]["iamt"].ToString();
                String itc_rev1_camt = obj["data"]["itc_elg"]["itc_rev"][1]["camt"].ToString();
                String itc_rev1_samt = obj["data"]["itc_elg"]["itc_rev"][1]["samt"].ToString();
                String itc_rev1_csamt = obj["data"]["itc_elg"]["itc_rev"][1]["csamt"].ToString();

                String itc_rev_iamt = obj["data"]["itc_elg"]["itc_rev"][0]["iamt"].ToString();
                String itc_rev_camt = obj["data"]["itc_elg"]["itc_rev"][0]["camt"].ToString();
                String itc_rev_samt = obj["data"]["itc_elg"]["itc_rev"][0]["samt"].ToString();
                String itc_rev_csamt = obj["data"]["itc_elg"]["itc_rev"][0]["csamt"].ToString();

                int x21 = Convert.ToInt32(itc_rev_iamt);
                int x22 = Convert.ToInt32(itc_rev1_iamt);
                int total5 = x21 + x22;
                String ftotal5 = total5.ToString();

                int x23 = Convert.ToInt32(itc_rev_camt);
                int x24 = Convert.ToInt32(itc_rev1_camt);
                int total6 = x23 + x24;
                String ftotal6 = total6.ToString();

                int x25 = Convert.ToInt32(itc_rev_samt);
                int x26 = Convert.ToInt32(itc_rev1_samt);
                int total7 = x25 + x26;
                String ftotal7 = total7.ToString();

                int x27 = Convert.ToInt32(itc_rev_csamt);
                int x28 = Convert.ToInt32(itc_rev1_csamt);
                int total8 = x27 + x28;
                String ftotal8 = total8.ToString();

                //---Net ITC Available (A–B)
                String itc_net_iamt = obj["data"]["itc_elg"]["itc_net"]["iamt"].ToString();
                String itc_net_camt = obj["data"]["itc_elg"]["itc_net"]["camt"].ToString();
                String itc_net_samt = obj["data"]["itc_elg"]["itc_net"]["samt"].ToString();
                String itc_net_csamt = obj["data"]["itc_elg"]["itc_net"]["csamt"].ToString();

                //-----D. Ineligible ITC
                String itc_inelg_iamt = obj["data"]["itc_elg"]["itc_inelg"][0]["iamt"].ToString();
                String itc_inelg_camt = obj["data"]["itc_elg"]["itc_inelg"][0]["camt"].ToString();
                String itc_inelg_samt = obj["data"]["itc_elg"]["itc_inelg"][0]["samt"].ToString();
                String itc_inelg_csamt = obj["data"]["itc_elg"]["itc_inelg"][0]["csamt"].ToString();

                String itc_inelg1_iamt = obj["data"]["itc_elg"]["itc_inelg"][1]["iamt"].ToString();
                String itc_inelg1_camt = obj["data"]["itc_elg"]["itc_inelg"][1]["camt"].ToString();
                String itc_inelg1_samt = obj["data"]["itc_elg"]["itc_inelg"][1]["samt"].ToString();
                String itc_inelg1_csamt = obj["data"]["itc_elg"]["itc_inelg"][1]["csamt"].ToString();

                int x29 = Convert.ToInt32(itc_inelg_iamt);
                int x30 = Convert.ToInt32(itc_inelg1_iamt);
                int total9 = x29 + x30;
                String ftotal9 = total9.ToString();
                int x31 = Convert.ToInt32(itc_inelg_camt);
                int x32 = Convert.ToInt32(itc_inelg1_camt);
                int total10 = x31 + x32;
                String ftotal10 = total10.ToString();
                int x33 = Convert.ToInt32(itc_inelg_samt);
                int x34 = Convert.ToInt32(itc_inelg1_samt);
                int total11 = x33 + x34;
                String ftotal11 = total11.ToString();
                int x35 = Convert.ToInt32(itc_inelg_csamt);
                int x36 = Convert.ToInt32(itc_inelg1_csamt);
                int total12 = x35 + x36;
                String ftotal12 = total12.ToString();

                //---5 Values of Exempt, Nil-Rated and Non-GST Inward Supplies
                String inward_sup_inter = obj["data"]["inward_sup"]["isup_details"][0]["inter"].ToString();
                String inward_sup_intra = obj["data"]["inward_sup"]["isup_details"][0]["intra"].ToString();

                String inward_sup1_inter = obj["data"]["inward_sup"]["isup_details"][1]["inter"].ToString();
                String inward_sup1_intra = obj["data"]["inward_sup"]["isup_details"][1]["intra"].ToString();

                //--5.1 Interest and Late fee
                String intr_details_iamt = obj["data"]["intr_ltfee"]["intr_details"]["iamt"].ToString();
                String intr_details_camt = obj["data"]["intr_ltfee"]["intr_details"]["camt"].ToString();
                String intr_details_samt = obj["data"]["intr_ltfee"]["intr_details"]["samt"].ToString();
                String intr_details_csamt = obj["data"]["intr_ltfee"]["intr_details"]["csamt"].ToString();

                String ltfee_details_iamt = obj["data"]["intr_ltfee"]["ltfee_details"]["iamt"].ToString();
                String ltfee_details_camt = obj["data"]["intr_ltfee"]["ltfee_details"]["camt"].ToString();
                String ltfee_details_samt = obj["data"]["intr_ltfee"]["ltfee_details"]["samt"].ToString();
                String ltfee_details_csamt = obj["data"]["intr_ltfee"]["ltfee_details"]["csamt"].ToString();

                //---Payment of Tax
                String tax_pay_igst_tx = obj["data"]["tx_pay_dtls"]["returnsDbCdredList"]["tax_pay"][0]["igst"]["tx"].ToString();
                String tax_pay_cgst_tx = obj["data"]["tx_pay_dtls"]["returnsDbCdredList"]["tax_pay"][0]["cgst"]["tx"].ToString();
                String tax_pay_sgst_tx = obj["data"]["tx_pay_dtls"]["returnsDbCdredList"]["tax_pay"][0]["sgst"]["tx"].ToString();
                String tax_pay_cess_tx = obj["data"]["tx_pay_dtls"]["returnsDbCdredList"]["tax_pay"][0]["cess"]["tx"].ToString();

                String tax_pay_igst_tx1 = obj["data"]["tx_pay_dtls"]["returnsDbCdredList"]["tax_pay"][1]["igst"]["tx"].ToString();
                String tax_pay_cgst_tx1 = obj["data"]["tx_pay_dtls"]["returnsDbCdredList"]["tax_pay"][1]["cgst"]["tx"].ToString();
                String tax_pay_sgst_tx1 = obj["data"]["tx_pay_dtls"]["returnsDbCdredList"]["tax_pay"][1]["sgst"]["tx"].ToString();
                String tax_pay_cess_tx1 = obj["data"]["tx_pay_dtls"]["returnsDbCdredList"]["tax_pay"][1]["cess"]["tx"].ToString();

                String tax_paid_igst_tx1 = obj["data"]["tx_pay_dtls"]["returnsDbCdredList"]["tax_paid"]["pd_by_cash"][1]["igst"]["tx"].ToString();
                String tax_paid_cgst_tx1 = obj["data"]["tx_pay_dtls"]["returnsDbCdredList"]["tax_paid"]["pd_by_cash"][1]["cgst"]["tx"].ToString();
                String tax_paid_sgst_tx1 = obj["data"]["tx_pay_dtls"]["returnsDbCdredList"]["tax_paid"]["pd_by_cash"][1]["sgst"]["tx"].ToString();
                String tax_paid_cess_tx1 = obj["data"]["tx_pay_dtls"]["returnsDbCdredList"]["tax_paid"]["pd_by_cash"][1]["cess"]["tx"].ToString();

                String pd_by_itc_igst_igst_amt = obj["data"]["tx_pay_dtls"]["returnsDbCdredList"]["tax_paid"]["pd_by_itc"][1]["igst_igst_amt"].ToString();
                String pd_by_itc_igst_cgst_amt = obj["data"]["tx_pay_dtls"]["returnsDbCdredList"]["tax_paid"]["pd_by_itc"][1]["igst_cgst_amt"].ToString();
                String pd_by_itc_igst_sgst_amt = obj["data"]["tx_pay_dtls"]["returnsDbCdredList"]["tax_paid"]["pd_by_itc"][1]["igst_sgst_amt"].ToString();
                String pd_by_itc_sgst_sgst_amt = obj["data"]["tx_pay_dtls"]["returnsDbCdredList"]["tax_paid"]["pd_by_itc"][1]["sgst_sgst_amt"].ToString();
                String pd_by_itc_sgst_igst_amt = obj["data"]["tx_pay_dtls"]["returnsDbCdredList"]["tax_paid"]["pd_by_itc"][1]["sgst_igst_amt"].ToString();
                String pd_by_itc_cgst_cgst_amt = obj["data"]["tx_pay_dtls"]["returnsDbCdredList"]["tax_paid"]["pd_by_itc"][1]["cgst_cgst_amt"].ToString();
                String pd_by_itc_cgst_igst_amt = obj["data"]["tx_pay_dtls"]["returnsDbCdredList"]["tax_paid"]["pd_by_itc"][1]["cgst_igst_amt"].ToString();
                String pd_by_itc_cess_cess_amt = obj["data"]["tx_pay_dtls"]["returnsDbCdredList"]["tax_paid"]["pd_by_itc"][1]["cess_cess_amt"].ToString();

                String tax_paid_igst_intr1 = obj["data"]["tx_pay_dtls"]["returnsDbCdredList"]["tax_paid"]["pd_by_cash"][1]["igst"]["intr"].ToString();
                String tax_paid_igst_fee1 = obj["data"]["tx_pay_dtls"]["returnsDbCdredList"]["tax_paid"]["pd_by_cash"][1]["igst"]["fee"].ToString();
                String tax_paid_cgst_intr1 = obj["data"]["tx_pay_dtls"]["returnsDbCdredList"]["tax_paid"]["pd_by_cash"][1]["cgst"]["intr"].ToString();
                String tax_paid_cgst_fee1 = obj["data"]["tx_pay_dtls"]["returnsDbCdredList"]["tax_paid"]["pd_by_cash"][1]["cgst"]["fee"].ToString();
                String tax_paid_sgst_intr1 = obj["data"]["tx_pay_dtls"]["returnsDbCdredList"]["tax_paid"]["pd_by_cash"][1]["sgst"]["intr"].ToString();
                String tax_paid_sgst_fee1 = obj["data"]["tx_pay_dtls"]["returnsDbCdredList"]["tax_paid"]["pd_by_cash"][1]["sgst"]["fee"].ToString();
                String tax_paid_cess_intr1 = obj["data"]["tx_pay_dtls"]["returnsDbCdredList"]["tax_paid"]["pd_by_cash"][1]["cess"]["intr"].ToString();
                String tax_paid_cess_fee1 = obj["data"]["tx_pay_dtls"]["returnsDbCdredList"]["tax_paid"]["pd_by_cash"][1]["cess"]["fee"].ToString();








                //--------DateTime
                #region
                //---
                PdfPTable datetime = new PdfPTable(2);

                PdfPCell celldate = new PdfPCell(new Phrase("Row 1 , Col 1, Col 2 and col 3"));

                datetime.AddCell("Financial Year");
                datetime.AddCell(fy);

                datetime.WidthPercentage = 25;
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

                GSTIN.AddCell("2(a). Legal Name of the Registered Person");
                GSTIN.AddCell("");

                GSTIN.AddCell("2(b). Trade name, if any");
                GSTIN.AddCell("");

                GSTIN.WidthPercentage = 100;
                GSTIN.HorizontalAlignment = Element.ALIGN_LEFT;

                #endregion

                //---Outward supplies and inward supplies
                #region
                //---Outward supplies and inward supplies
                PdfPTable OutInwardSupplies = new PdfPTable(6);

                PdfPCell cellOutIn = new PdfPCell(new Phrase("Row 1 , Col 1, Col 2 and col 3"));
                OutInwardSupplies.TotalWidth = 1750f;
                // Inward.LockedWidth = true;
                float[] widthOutIn = new float[] { 500f, 200f, 200f, 200f, 200f, 200f };
                OutInwardSupplies.SetWidths(widthOutIn);
                cellOutIn.Colspan = 1;
                cellOutIn.HorizontalAlignment = Element.ALIGN_RIGHT;
                //datetime.AddCell(celldate);

                OutInwardSupplies.AddCell("Nature of Supplies");
                OutInwardSupplies.AddCell("Total Taxable Value(₹)");
                OutInwardSupplies.AddCell("Integrated Tax(₹)");
                OutInwardSupplies.AddCell("Central Tax (₹)");
                OutInwardSupplies.AddCell("State/UT Tax(₹)");
                OutInwardSupplies.AddCell("Cess(₹)");

                OutInwardSupplies.AddCell("(a) Outward Taxable Supplies (Other Than Zero Rated, Nil Rated and Exempted)");
                OutInwardSupplies.AddCell(osup_det_txval);
                OutInwardSupplies.AddCell(osup_det_iamt);
                OutInwardSupplies.AddCell(osup_det_camt);
                OutInwardSupplies.AddCell(osup_det_samt);
                OutInwardSupplies.AddCell(osup_det_csamt);

                OutInwardSupplies.AddCell("(b) Outward Taxable Supplies (Zero Rated)");
                OutInwardSupplies.AddCell(osup_zero_txval);
                OutInwardSupplies.AddCell(osup_zero_iamt);
                OutInwardSupplies.AddCell(osup_zero_camt);
                OutInwardSupplies.AddCell(osup_zero_samt);
                OutInwardSupplies.AddCell(osup_zero_csamt);

                OutInwardSupplies.AddCell("(c) Other Outward Supplies (Nil Rated, Exempted)");
                OutInwardSupplies.AddCell(osup_nil_exmp_txval);
                OutInwardSupplies.AddCell(osup_nil_exmp_iamt);
                OutInwardSupplies.AddCell(osup_nil_exmp_camt);
                OutInwardSupplies.AddCell(osup_nil_exmp_samt);
                OutInwardSupplies.AddCell(osup_nil_exmp_csamt);

                OutInwardSupplies.AddCell("(d) Inward Supplies (Liable to Reverse Charge)");
                OutInwardSupplies.AddCell(isup_rev_txval);
                OutInwardSupplies.AddCell(isup_rev_iamt);
                OutInwardSupplies.AddCell(isup_rev_camt);
                OutInwardSupplies.AddCell(isup_rev_samt);
                OutInwardSupplies.AddCell(isup_rev_csamt);

                OutInwardSupplies.AddCell("Non-GST Outward Supplies");
                OutInwardSupplies.AddCell(osup_nongst_txval);
                OutInwardSupplies.AddCell(osup_nongst_iamt);
                OutInwardSupplies.AddCell(osup_nongst_camt);
                OutInwardSupplies.AddCell(osup_nongst_samt);
                OutInwardSupplies.AddCell(osup_nongst_csamt);

                OutInwardSupplies.WidthPercentage = 100;
                OutInwardSupplies.HorizontalAlignment = Element.ALIGN_LEFT;

                #endregion
                //---3.2 Supplies made to Unregistered Persons
                #region
                //---Supplies made to Unregistered Persons
                //var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                var normalFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 6);
                PdfPTable SuppliesMadeUnreg = new PdfPTable(3);

                PdfPCell cellMadeUnreg = new PdfPCell(new Phrase("Row 1 , Col 1, Col 2 and col 3"));
                SuppliesMadeUnreg.TotalWidth = 1750f;
                // Inward.LockedWidth = true;
                float[] widthMadeUnreg = new float[] { 600f, 250f, 250f };
                SuppliesMadeUnreg.SetWidths(widthMadeUnreg);
                cellMadeUnreg.Colspan = 1;
                cellMadeUnreg.HorizontalAlignment = Element.ALIGN_RIGHT;
                //datetime.AddCell(celldate);

                SuppliesMadeUnreg.AddCell("Nature of Supplies");
                SuppliesMadeUnreg.AddCell("Total Taxable Value(₹)");
                SuppliesMadeUnreg.AddCell("Integrated Tax(₹)");

                SuppliesMadeUnreg.AddCell("Supplies Made to Unregistered Persons");
                SuppliesMadeUnreg.AddCell(tot_unreg_details_txval_1);
                SuppliesMadeUnreg.AddCell(tot_unreg_details_iamt_1);

                SuppliesMadeUnreg.AddCell("Supplies Made to Composition Taxable Persons");
                SuppliesMadeUnreg.AddCell(tot_comp_details_txval_1);
                SuppliesMadeUnreg.AddCell(tot_comp_details_iamt_1);

                SuppliesMadeUnreg.AddCell("Supplies Made to UIN holders");
                SuppliesMadeUnreg.AddCell(tot_uin_details_txval_1);
                SuppliesMadeUnreg.AddCell(tot_uin_details_iamt_1);



                SuppliesMadeUnreg.WidthPercentage = 70;
                SuppliesMadeUnreg.HorizontalAlignment = Element.ALIGN_LEFT;

                #endregion
                //----Eligible ITC
                #region
                //----Eligible ITC
                PdfPTable EligibleITC = new PdfPTable(5);

                PdfPCell cellITC = new PdfPCell(new Phrase("Row 1 , Col 1, Col 2 and col 3"));
                EligibleITC.TotalWidth = 1750f;
                // Inward.LockedWidth = true;
                float[] widthITC = new float[] { 500f, 250f, 250f, 250f, 250f };
                EligibleITC.SetWidths(widthITC);
                cellITC.Colspan = 1;
                cellITC.HorizontalAlignment = Element.ALIGN_RIGHT;

                //datetime.AddCell(celldate); 


                EligibleITC.AddCell("Details");
                EligibleITC.AddCell("Integrated Tax(₹)");
                EligibleITC.AddCell("Central Tax (₹)");
                EligibleITC.AddCell("State/UT Tax(₹)");
                EligibleITC.AddCell("Cess(₹)");

                EligibleITC.AddCell("A. ITC Available(Whether in Full or Part)");
                EligibleITC.AddCell(ftotal1);
                EligibleITC.AddCell(ftotal2);
                EligibleITC.AddCell(ftotal3);
                EligibleITC.AddCell(ftotal4);

                EligibleITC.AddCell("(1) Import of goods");
                EligibleITC.AddCell(itc_avl2_iamt);
                EligibleITC.AddCell(itc_avl2_camt);
                EligibleITC.AddCell(itc_avl2_samt);
                EligibleITC.AddCell(itc_avl2_csamt);

                EligibleITC.AddCell("(2) Import of services");
                EligibleITC.AddCell(itc_avl1_iamt);
                EligibleITC.AddCell(itc_avl1_camt);
                EligibleITC.AddCell(itc_avl1_samt);
                EligibleITC.AddCell(itc_avl1_csamt);

                EligibleITC.AddCell("(3) Inward supplies liable to reverse charge (other than 1 & 2 above)");
                EligibleITC.AddCell(itc_avl3_iamt);
                EligibleITC.AddCell(itc_avl3_camt);
                EligibleITC.AddCell(itc_avl3_samt);
                EligibleITC.AddCell(itc_avl3_csamt);

                EligibleITC.AddCell("(4) Inward supplies from ISD");
                EligibleITC.AddCell(itc_avl4_iamt);
                EligibleITC.AddCell(itc_avl4_camt);
                EligibleITC.AddCell(itc_avl4_samt);
                EligibleITC.AddCell(itc_avl4_csamt);

                EligibleITC.AddCell("(5) All other ITC");
                EligibleITC.AddCell(itc_avl_iamt);
                EligibleITC.AddCell(itc_avl_camt);
                EligibleITC.AddCell(itc_avl_samt);
                EligibleITC.AddCell(itc_avl_csamt);

                EligibleITC.AddCell("B. ITC Reversed");
                EligibleITC.AddCell(ftotal5);
                EligibleITC.AddCell(ftotal6);
                EligibleITC.AddCell(ftotal7);
                EligibleITC.AddCell(ftotal8);

                EligibleITC.AddCell("(1) As per rules 42 & 43 of CGST Rules");
                EligibleITC.AddCell(itc_rev1_iamt);
                EligibleITC.AddCell(itc_rev1_camt);
                EligibleITC.AddCell(itc_rev1_samt);
                EligibleITC.AddCell(itc_rev1_csamt);

                EligibleITC.AddCell("(2) Others");
                EligibleITC.AddCell(itc_rev_iamt);
                EligibleITC.AddCell(itc_rev_camt);
                EligibleITC.AddCell(itc_rev_samt);
                EligibleITC.AddCell(itc_rev_csamt);

                EligibleITC.AddCell("C. Net ITC Available (A–B)");
                EligibleITC.AddCell(itc_net_iamt);
                EligibleITC.AddCell(itc_net_camt);
                EligibleITC.AddCell(itc_net_samt);
                EligibleITC.AddCell(itc_net_csamt);

                EligibleITC.AddCell("D. Ineligible ITC");
                EligibleITC.AddCell(ftotal9);
                EligibleITC.AddCell(ftotal10);
                EligibleITC.AddCell(ftotal11);
                EligibleITC.AddCell(ftotal12);

                EligibleITC.AddCell("(1) As per section 17(5)");
                EligibleITC.AddCell(itc_inelg1_iamt);
                EligibleITC.AddCell(itc_inelg1_camt);
                EligibleITC.AddCell(itc_inelg1_samt);
                EligibleITC.AddCell(itc_inelg1_csamt);

                EligibleITC.AddCell("(2) Others");
                EligibleITC.AddCell(itc_inelg_iamt);
                EligibleITC.AddCell(itc_inelg_iamt);
                EligibleITC.AddCell(itc_inelg_iamt);
                EligibleITC.AddCell(itc_inelg_iamt);



                EligibleITC.WidthPercentage = 90;
                EligibleITC.HorizontalAlignment = Element.ALIGN_LEFT;

                #endregion
                //---5 Values of Exempt, Nil-Rated and Non-GST Inward Supplies
                #region
                //---5 Values of Exempt, Nil-Rated and Non-GST Inward Supplies

                PdfPTable ExemptNilNonGST = new PdfPTable(3);
                PdfPCell cellNilNonGST = new PdfPCell(new Phrase("Row 1 , Col 1, Col 2 and col 3"));
                ExemptNilNonGST.TotalWidth = 1750f;
                // Inward.LockedWidth = true;
                float[] widthNilNonGST = new float[] { 500f, 250f, 250f };
                ExemptNilNonGST.SetWidths(widthNilNonGST);
                cellNilNonGST.Colspan = 1;
                cellNilNonGST.HorizontalAlignment = Element.ALIGN_RIGHT;
                //datetime.AddCell(celldate);

                ExemptNilNonGST.AddCell("Nature of Supplies");
                ExemptNilNonGST.AddCell("Inter-State Supplies(₹)");
                ExemptNilNonGST.AddCell("Intra-State Supplies(₹)");

                ExemptNilNonGST.AddCell("From a Supplier under Composition Scheme, Exempt and Nil Rated Supply");
                ExemptNilNonGST.AddCell(inward_sup_inter);
                ExemptNilNonGST.AddCell(inward_sup_intra);

                ExemptNilNonGST.AddCell("Non GST Supply");
                ExemptNilNonGST.AddCell(inward_sup1_inter);
                ExemptNilNonGST.AddCell(inward_sup1_intra);



                ExemptNilNonGST.WidthPercentage = 70;
                ExemptNilNonGST.HorizontalAlignment = Element.ALIGN_LEFT;

                #endregion
                //--5.1 Interest and Late fee
                #region
                //--5.1 Interest and Late fee   

                PdfPTable InterestLateFee = new PdfPTable(5);

                PdfPCell cellInterestLateFee = new PdfPCell(new Phrase("Row 1 , Col 1, Col 2 and col 3"));
                InterestLateFee.TotalWidth = 1750f;
                // Inward.LockedWidth = true;
                float[] widthInterestLateFee = new float[] { 500f, 250f, 250f, 250f, 250f };
                InterestLateFee.SetWidths(widthInterestLateFee);
                cellInterestLateFee.Colspan = 1;
                cellInterestLateFee.HorizontalAlignment = Element.ALIGN_RIGHT;
                //datetime.AddCell(celldate);

                InterestLateFee.AddCell("Details");
                InterestLateFee.AddCell("Integrated Tax(₹)");
                InterestLateFee.AddCell("Central Tax(₹)");
                InterestLateFee.AddCell("State/UT Tax(₹)");
                InterestLateFee.AddCell("Cess(₹)");

                InterestLateFee.AddCell("Interest");
                InterestLateFee.AddCell(intr_details_iamt);
                InterestLateFee.AddCell(intr_details_camt);
                InterestLateFee.AddCell(intr_details_samt);
                InterestLateFee.AddCell(intr_details_csamt);

                InterestLateFee.AddCell("Late Fee");
                InterestLateFee.AddCell(ltfee_details_iamt);
                InterestLateFee.AddCell(ltfee_details_camt);
                InterestLateFee.AddCell(ltfee_details_samt);
                InterestLateFee.AddCell(ltfee_details_csamt);




                InterestLateFee.WidthPercentage = 60;
                InterestLateFee.HorizontalAlignment = Element.ALIGN_LEFT;

                #endregion
                //---Payment of Tax
                #region
                //---Payment of Tax
                PdfPTable PaymentOfTax = new PdfPTable(9);

                PdfPCell cell = new PdfPCell(new Phrase("Description"));
                cell.Rowspan = 2;
                PaymentOfTax.AddCell(cell);
                cell = new PdfPCell(new Phrase("Total Tax Payable(₹)"));
                cell.Rowspan = 2;
                PaymentOfTax.AddCell(cell);


                PdfPCell cellTax = new PdfPCell(new Phrase("Tax Paid Through ITC(₹)"));
                PaymentOfTax.TotalWidth = 1800f;
                // Inward.LockedWidth = true;
                float[] widthTax = new float[] { 200f, 200f, 200f, 200f, 200f, 200f, 200f, 200f, 200f };
                PaymentOfTax.SetWidths(widthTax);
                cellTax.Colspan = 4;
                cellTax.HorizontalAlignment = Element.ALIGN_RIGHT;
                PaymentOfTax.AddCell(cellTax);

                cell = new PdfPCell(new Phrase("Tax/Cess Paid in Cash(₹)"));
                cell.Rowspan = 2;
                PaymentOfTax.AddCell(cell);
                cell = new PdfPCell(new Phrase("Interest Paid in Cash(₹)"));
                cell.Rowspan = 2;
                PaymentOfTax.AddCell(cell);
                cell = new PdfPCell(new Phrase("Late Fee Paid in Integrated Cash(₹)"));
                cell.Rowspan = 2;
                PaymentOfTax.AddCell(cell);

                PaymentOfTax.AddCell("Integrated Tax");
                PaymentOfTax.AddCell("Central Tax");
                PaymentOfTax.AddCell("State/UT Tax");
                PaymentOfTax.AddCell("Cess");

                cell = new PdfPCell(new Phrase("(A) Other than Reverse Charge"));
                cell.Colspan = 9;
                PaymentOfTax.AddCell(cell);


                PaymentOfTax.AddCell("Integrated Tax");
                PaymentOfTax.AddCell(tax_pay_igst_tx);
                PaymentOfTax.AddCell(pd_by_itc_igst_igst_amt);
                PaymentOfTax.AddCell(pd_by_itc_igst_cgst_amt);
                PaymentOfTax.AddCell(pd_by_itc_igst_sgst_amt);
                PaymentOfTax.AddCell("0");
                PaymentOfTax.AddCell(tax_paid_igst_tx1);
                PaymentOfTax.AddCell(tax_paid_igst_intr1);
                PaymentOfTax.AddCell(tax_paid_igst_fee1);

                PaymentOfTax.AddCell("Central Tax");
                PaymentOfTax.AddCell(tax_pay_cgst_tx);
                PaymentOfTax.AddCell(pd_by_itc_cgst_igst_amt);
                PaymentOfTax.AddCell(pd_by_itc_sgst_sgst_amt);
                PaymentOfTax.AddCell("0");
                PaymentOfTax.AddCell("0");
                PaymentOfTax.AddCell(tax_paid_cgst_tx1);
                PaymentOfTax.AddCell(tax_paid_cgst_intr1);
                PaymentOfTax.AddCell(tax_paid_cgst_fee1);

                PaymentOfTax.AddCell("State/UT Tax");
                PaymentOfTax.AddCell(tax_pay_sgst_tx);
                PaymentOfTax.AddCell(pd_by_itc_sgst_igst_amt);
                PaymentOfTax.AddCell("0");
                PaymentOfTax.AddCell(pd_by_itc_cgst_cgst_amt);
                PaymentOfTax.AddCell("0");
                PaymentOfTax.AddCell(tax_paid_sgst_tx1);
                PaymentOfTax.AddCell(tax_paid_sgst_intr1);
                PaymentOfTax.AddCell(tax_paid_sgst_fee1);

                PaymentOfTax.AddCell("Cess");
                PaymentOfTax.AddCell(tax_pay_cess_tx);
                PaymentOfTax.AddCell("0");
                PaymentOfTax.AddCell("0");
                PaymentOfTax.AddCell("0");
                PaymentOfTax.AddCell(pd_by_itc_cess_cess_amt);
                PaymentOfTax.AddCell(tax_paid_cess_tx1);
                PaymentOfTax.AddCell(tax_paid_cess_intr1);
                PaymentOfTax.AddCell(tax_paid_cess_fee1);

                cell = new PdfPCell(new Phrase("(B) Reverse Charge"));
                cell.Colspan = 9;
                PaymentOfTax.AddCell(cell);

                PaymentOfTax.AddCell("Integrated Tax");
                PaymentOfTax.AddCell(tax_pay_igst_tx1);
                PaymentOfTax.AddCell("0");
                PaymentOfTax.AddCell("0");
                PaymentOfTax.AddCell("0");
                PaymentOfTax.AddCell("0");
                PaymentOfTax.AddCell(tax_paid_igst_tx1);
                PaymentOfTax.AddCell(tax_paid_igst_intr1);
                PaymentOfTax.AddCell(tax_paid_igst_fee1);

                PaymentOfTax.AddCell("Central Tax");
                PaymentOfTax.AddCell(tax_pay_cgst_tx1);
                PaymentOfTax.AddCell("0");
                PaymentOfTax.AddCell("0");
                PaymentOfTax.AddCell("0");
                PaymentOfTax.AddCell("0");
                PaymentOfTax.AddCell(tax_paid_cgst_tx1);
                PaymentOfTax.AddCell(tax_paid_cgst_intr1);
                PaymentOfTax.AddCell(tax_paid_cgst_fee1);

                PaymentOfTax.AddCell("State/UT Tax");
                PaymentOfTax.AddCell(tax_pay_sgst_tx1);
                PaymentOfTax.AddCell("0");
                PaymentOfTax.AddCell("0");
                PaymentOfTax.AddCell("0");
                PaymentOfTax.AddCell("0");
                PaymentOfTax.AddCell(tax_paid_sgst_tx1);
                PaymentOfTax.AddCell(tax_paid_sgst_intr1);
                PaymentOfTax.AddCell(tax_paid_sgst_fee1);

                PaymentOfTax.AddCell("Cess");
                PaymentOfTax.AddCell(tax_pay_cess_tx1);
                PaymentOfTax.AddCell("0");
                PaymentOfTax.AddCell("0");
                PaymentOfTax.AddCell("0");
                PaymentOfTax.AddCell("0");
                PaymentOfTax.AddCell(tax_paid_cess_tx1);
                PaymentOfTax.AddCell(tax_paid_cess_intr1);
                PaymentOfTax.AddCell(tax_paid_cess_fee1);



                PaymentOfTax.WidthPercentage = 90;
                PaymentOfTax.HorizontalAlignment = Element.ALIGN_LEFT;

                #endregion

                var savefiledialog = new SaveFileDialog();

                if (savefiledialog.ShowDialog() == DialogResult.OK)
                {

                    Document document = new Document(PageSize.A4.Rotate(), 50, 50, 15, 15);


                    var output = new FileStream(savefiledialog.FileName, FileMode.Create);

                    PdfWriter writer = PdfWriter.GetInstance(document, output);

                    // Open the Document for writing

                    document.Open();


                    Paragraph welcomeParagraph = new Paragraph(" Form GSTR-3B ");
                    Paragraph report = new Paragraph("[See Rule 61(5)]");
                    Paragraph date = new Paragraph("System Generated Summary");
                    Paragraph sysgen = new Paragraph("(For Reference only)");
                    Paragraph OutInwardSuppliesHadding = new Paragraph("3.1 Details of Outward supplies and inward supplies liable to reverse charge", normalFont);
                    Paragraph SuppliesMadeUnregHadding = new Paragraph("3.2 Out of Supplies made in 3.1 (a) above, Details of Inter-State Supplies made to Unregistered Persons, Composition Taxable Persons and UIN Holders", normalFont);
                    Paragraph EligibleITCHadding = new Paragraph("4. Eligible ITC", normalFont);
                    Paragraph ExemptNilNonGSTHadding = new Paragraph("5 Values of Exempt, Nil-Rated and Non-GST Inward Supplies", normalFont);
                    Paragraph InterestLateFeeHadding = new Paragraph("5.1 Interest and Late fee", normalFont);
                    Paragraph PaymentOfTaxHadding = new Paragraph("6.1 Payment of Tax", normalFont);



                    report.Alignment = Element.ALIGN_CENTER;
                    date.Alignment = Element.ALIGN_CENTER;
                    sysgen.Alignment = Element.ALIGN_CENTER;
                    welcomeParagraph.Alignment = Element.ALIGN_CENTER;


                    welcomeParagraph.Font.SetColor(242, 132, 0);
                    welcomeParagraph.Font.Size = 20;
                    report.Font.Size = 10;
                    OutInwardSuppliesHadding.Font.Size = 15;
                    SuppliesMadeUnregHadding.Font.Size = 15;


                    document.Add(welcomeParagraph);

                    document.Add(new Paragraph("\n"));
                    document.Add(report);
                    document.Add(new Paragraph("\n"));
                    document.Add(date);
                    document.Add(new Paragraph("\n"));
                    document.Add(sysgen);
                    document.Add(new Paragraph("\n"));
                    document.Add(datetime);
                    document.Add(new Paragraph("\n"));
                    document.Add(GSTIN);
                    document.Add(new Paragraph("\n"));
                    document.Add(OutInwardSuppliesHadding);
                    document.Add(new Paragraph("\n"));
                    document.Add(OutInwardSupplies);
                    document.Add(new Paragraph("\n"));
                    document.Add(SuppliesMadeUnregHadding);
                    document.Add(new Paragraph("\n"));
                    document.Add(SuppliesMadeUnreg);
                    document.Add(new Paragraph("\n"));
                    document.Add(EligibleITCHadding);
                    document.Add(new Paragraph("\n"));
                    document.Add(EligibleITC);
                    document.Add(new Paragraph("\n"));
                    document.Add(ExemptNilNonGSTHadding);
                    document.Add(new Paragraph("\n"));
                    document.Add(ExemptNilNonGST);
                    document.Add(new Paragraph("\n"));
                    document.Add(InterestLateFeeHadding);
                    document.Add(new Paragraph("\n"));
                    document.Add(InterestLateFee);
                    document.Add(new Paragraph("\n"));
                    document.Add(PaymentOfTaxHadding);
                    document.Add(new Paragraph("\n"));
                    document.Add(PaymentOfTax);
                    document.Add(new Paragraph("\n"));


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

        private void GSTR_3B_PDF_Load(object sender, EventArgs e)
        {

        }
    }
}
