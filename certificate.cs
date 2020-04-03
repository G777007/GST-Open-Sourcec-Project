using Newtonsoft.Json;
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
using Newtonsoft.Json.Linq;
using Proactive.CustomTools;
using System.Data.SQLite;

namespace SPEQTAGST_DESIGN
{
    public partial class gstrcertificate : Form
    {

        CookieContainer Cc = new CookieContainer();
        HttpWebResponse response;
        MainClass MC = new MainClass();

        public gstrcertificate()
        {
            InitializeComponent();
           // MC.Connection();

        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            //DownloadLiveData();
        }

        //private void DownloadLiveData()
        //{
        //    bool flag;
        //    //clsPubPro _clsPubPro;
        //    try
        //    {
        //        MC.Open();
        //        string reply = "";
        //        System.Xml.XmlDocument xmldoc;
        //        DataSet dsresult;
        //        // string strQuery = "";
        //        //string companyGSTN = CommonHelper.CompanyGSTN;
        //        //string TdstcsYear= CommonHelper.ReturnYear;
        //        //string month = CommonHelper.GetMonth(CommonHelper.SelectedMonth);
        //        //string reqParam = "";
        //        //if (Convert.ToInt32(month) > 3 && Convert.ToInt32(month) <= 12) reqParam = string.Concat(CommonHelper.GetMonth(CommonHelper.SelectedMonth), CommonHelper.ReturnYear.Split('-')[0].Trim());
        //        //else reqParam = string.Concat(CommonHelper.GetMonth(CommonHelper.SelectedMonth), CommonHelper.ReturnYear.Split('-')[1].Trim());
        //        //string _Param = "092019";

        //        var obj = clsPro.Cooki != null ? clsPro.Cooki.FirstOrDefault(x => x.ckname == (string.Concat("loginCookies_","1"))) : null;

        //        if (obj != null && obj.CC1 != null)
        //        {
        //            this.Cc = obj.CC1;

        //            //Request URL: https://services.gst.gov.in/services/auth/api/get/certs
        //            //Referer: https://services.gst.gov.in/services/auth/certs

        //            HttpWebRequest httpWebRequest = this.PrepareGetRequestTdsTcs(new Uri(string.Format("https://services.gst.gov.in/services/auth/api/get/certs")), "https://services.gst.gov.in/services/auth/certs");
        //            this.response = (HttpWebResponse)httpWebRequest.GetResponse();
        //            Stream responseStream = this.response.GetResponseStream();
        //            reply = (new StreamReader(responseStream, Encoding.UTF8)).ReadToEnd();
        //            bool flagstatus = false;

        //            JArray arr = JArray.Parse(reply);
                     

        //            for (int i = 0; i < arr.Count; i++)
        //            {
        //                string frmno = Convert.ToString(arr[i]["frmno"]);
        //                string frmdc = Convert.ToString(arr[i]["frmdc"]);
        //                string isdt = Convert.ToString(arr[i]["isdt"]);
        //                string docid = Convert.ToString(arr[i]["docid"]);
        //                string applnId = Convert.ToString(arr[i]["applnId"]);

        //                // DataSet ds = new DataSet();

        //                string sql = "";
        //                //sql = "Delete from SPQViewCertificate";
        //                sql = " insert into SPQViewCertificate ( frmno, frmdc, isdt, docid, appInId ) " +
        //                    " VALUES('" + frmno + "','" + frmdc + "','" + isdt + "', '" + docid + "','" + applnId + "')" ;
        //                // sql = sql + " Values ('" + frmno + "','" + frmdc + "','" + isdt + "', '" + doc id + "','" + applnId + "')";

        //                MC.sqlcmd = new SQLiteCommand(sql, MC.con);
        //                MC.sqlcmd.ExecuteNonQuery();
                        
        //                //MC.InitializeColumn(dgv_view, 3, "Downloads", 100, true, DataGridViewContentAlignment.MiddleCenter);

        //            }
        //            DataTable dt = new DataTable();
        //            dt = MC.GetValueindatatable("Select  frmno,frmdc,isdt From SPQViewCertificate");
        //            dgv_view.DataSource = dt;
        //            MC.InitializeColumn(dgv_view, 0, "From No", 100, true, DataGridViewContentAlignment.MiddleCenter);
        //            MC.InitializeColumn(dgv_view, 1, "Form Description", 300, true, DataGridViewContentAlignment.MiddleCenter);
        //            MC.InitializeColumn(dgv_view, 2, "Date Of Issue", 200, true, DataGridViewContentAlignment.MiddleCenter);

        //            //List<Certificate> listdata = JsonConvert.DeserializeObject<Certificate>(reply);
        //            //list = JsonConvert.DeserializeObject<Certificate>(reply);
        //            //string jsonString = "{ \"TdsCertificate\": {" + reply.Trim().TrimStart('{').TrimEnd('}') + @"} }";
        //            ////// Now it is secure that we have always a Json with one node as root 
        //            //xmldoc = JsonConvert.DeserializeXmlNode(jsonString);
        //            ////// DataSet is able to read from XML and return a proper DataSet
        //            //dsresult = new DataSet();
        //            //dsresult.ReadXml(new StringReader(xmldoc.InnerXml));

        //        }
        //        else
        //        {
        //            SPQGstLogin frm = new SPQGstLogin();
        //            frm.strBulk = "BULK";
        //            frm.Visible = false;
        //            var result = frm.ShowDialog();
        //            if (result != DialogResult.OK)
        //            {
        //                DownloadLiveData();
        //            }
        //            else
        //            {
        //                DownloadLiveData();
        //            }
        //        }

        //    }
        //    catch (Exception exception1)
        //    {
        //        Exception exception = exception1;
        //        if (!exception.Message.Contains("403"))
        //        {
        //            MessageBox.Show(string.Concat("Error : ", exception.Message), "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        //            object[] message = new object[] { exception.Message, Environment.NewLine, exception.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine };
        //            string str = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", message);
        //            StreamWriter streamWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
        //            streamWriter.Write(str);
        //            streamWriter.Close();
        //            flag = false;
        //        }
        //        else
        //        {
        //            SPQGstLogin frm = new SPQGstLogin();
        //            frm.Visible = false;
        //            var result = frm.ShowDialog();
        //            if (result != DialogResult.OK)
        //            {
        //                //GstLogin objLogin = new GstLogin();
        //                //objLogin.Show();
        //            }
        //            else
        //            {
        //                DownloadLiveData();
        //            }

        //            //frmGstLogin _frmGstLogin = new frmGstLogin()
        //            //{
        //            //    Visible = false
        //            //};
        //            //if (_frmGstLogin.ShowDialog() == DialogResult.OK)
        //            //{
        //            //    this.DownloadLiveGSTR9();
        //            //}
        //            flag = true;
        //        }
        //    }
        //    finally
        //    {
        //        MC.Close();
        //    }
        //    // return true;
        //}

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

        public class Data
        {
            public List<Certificate> list { get; set; }
        }
        public class Certificate
        {
            public string frmno { get; set; }
            public string frmdc { get; set; }
            public string isdt { get; set; }
            public string docid { get; set; }

            public string appInId { get; set; }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void gstrcertificate_Load(object sender, EventArgs e)
        {

        }

        //private void dgv_view_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    TDSCertificates certificate = new TDSCertificates();
        //    certificate.Text = this.dgv_view.Rows[1].Cells[1].Value.ToString();
           
        //    certificate.ShowDialog();
        //}
    }
}
