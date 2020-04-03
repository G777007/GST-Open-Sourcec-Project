using Newtonsoft.Json;
using SPEQTAGST.Report;
using System;
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
    public partial class TDSCertificates : Form
    {

        CookieContainer Cc = new CookieContainer();
        HttpWebResponse response;
        public TDSCertificates()
        {
            InitializeComponent();
        }

        private void TDSCertificates_Load(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            DownloadLiveData();
        }

        private void DownloadLiveData()
        {
            bool flag;
            //clsPubPro _clsPubPro;
            try
            {
                string reply = "";
                System.Xml.XmlDocument xmldoc;
                DataSet dsresult;
                // string strQuery = "";
                string companyGSTN = CommonHelper.CompanyGSTN;
                //string TdstcsYear= CommonHelper.ReturnYear;
                //string month = CommonHelper.GetMonth(CommonHelper.SelectedMonth);
                //string reqParam = "";
                //if (Convert.ToInt32(month) > 3 && Convert.ToInt32(month) <= 12) reqParam = string.Concat(CommonHelper.GetMonth(CommonHelper.SelectedMonth), CommonHelper.ReturnYear.Split('-')[0].Trim());
                //else reqParam = string.Concat(CommonHelper.GetMonth(CommonHelper.SelectedMonth), CommonHelper.ReturnYear.Split('-')[1].Trim());
                string _Param = "092019";

                var obj = clsPro.Cooki != null ? clsPro.Cooki.FirstOrDefault(x => x.ckname == (string.Concat("loginCookies_", "1"))) : null;

                if (obj != null && obj.CC1 != null)
                {
                    this.Cc = obj.CC1;

                    //Request URL: https://return.gst.gov.in/returns2/auth/api/gstr7a/getcertificate?req_typ=SRCH&rtn_prd=092019
                    //Referer: https://return.gst.gov.in/returns2/auth/gstr7a/search

                    HttpWebRequest httpWebRequest = this.PrepareGetRequestTdsTcs(new Uri(string.Format("https://return.gst.gov.in/returns2/auth/api/gstr7a/getcertificate?req_typ=SRCH&rtn_prd={0}", _Param)), "https://return.gst.gov.in/returns2/auth/gstr7a/search");
                    this.response = (HttpWebResponse)httpWebRequest.GetResponse();
                    Stream responseStream = this.response.GetResponseStream();
                    reply = (new StreamReader(responseStream, Encoding.UTF8)).ReadToEnd();
                    bool flagstatus = false;
                    string jsonString = "{ \"TdsCertificate\": {" + reply.Trim().TrimStart('{').TrimEnd('}') + @"} }";
                    //// Now it is secure that we have always a Json with one node as root 
                    xmldoc = JsonConvert.DeserializeXmlNode(jsonString);
                    //// DataSet is able to read from XML and return a proper DataSet
                    dsresult = new DataSet();
                    dsresult.ReadXml(new StringReader(xmldoc.InnerXml));
                   
                }
                else
                {
                    SPQGstLogin frm = new SPQGstLogin();
                    frm.Visible = false;
                    var result = frm.ShowDialog();
                    if (result != DialogResult.OK)
                    {

                    }
                    else
                    {
                        DownloadLiveData();
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
                    flag = false;
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
                        DownloadLiveData();
                    }

                    //frmGstLogin _frmGstLogin = new frmGstLogin()
                    //{
                    //    Visible = false
                    //};
                    //if (_frmGstLogin.ShowDialog() == DialogResult.OK)
                    //{
                    //    this.DownloadLiveGSTR9();
                    //}
                    flag = true;
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
            }
            return httpWebRequest;
        }

        private void btn_search_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
