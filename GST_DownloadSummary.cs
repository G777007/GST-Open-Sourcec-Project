using Newtonsoft.Json.Linq;
using Proactive.CustomTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPEQTAGST_DESIGN
{
    public partial class GST_DownloadSummary : Form
    {
        CookieContainer Cc = new CookieContainer();
        HttpWebResponse response;
        MainClass MC = new MainClass();
        public GST_DownloadSummary()
        {
            InitializeComponent();
            MC.Connection();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            DownloadLiveData();
        }

        public void DownloadLiveData()
        {
            bool flag;
            //clsPubPro _clsPubPro;
            try
            {
                MC.Open();
                string reply = "";
                System.Xml.XmlDocument xmldoc;
                DataSet dsresult;
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

                    //Request URL: https://services.gst.gov.in/services/auth/api/get/certs
                    //Referer: https://services.gst.gov.in/services/auth/certs
                    //Request:https://return.gst.gov.in/returns/auth/api/offline/upload/summary?rtn_prd=032018&rtn_typ=GSTR9C
                    //Referer:https://return.gst.gov.in/returns2/auth/gstr9c/offlineupload

                    //https://return.gst.gov.in/returns/auth/api/offline/upload/error/report/url?token=66ebc40febc6467eac57d0ceb0e87600affb2&rtn_prd=032018&rtn_typ=GSTR9C

                     HttpWebRequest httpWebRequest = this.PrepareGetRequestTdsTcs(new Uri(string.Format("https://return.gst.gov.in/returns/auth/api/offline/upload/error/generate?ref_id=a8cad1ed-1061-43ac-993a-51ce2017fc37&rtn_prd=032018&rtn_typ=GSTR9C")), " https://return.gst.gov.in/returns/auth/api/offline/upload/error/report/url?token=66ebc40febc6467eac57d0ceb0e876002&rtn_prd=032018&rtn_typ=GSTR9C");
                        this.response = (HttpWebResponse)httpWebRequest.GetResponse();
                        Stream responseStream = this.response.GetResponseStream();
                       reply = (new StreamReader(responseStream, Encoding.UTF8)).ReadToEnd();
                        bool flagstatus = false;
                        

                   

                    //HttpWebRequest httpWebRequest = this.PrepareGetRequestTdsTcs(new Uri(string.Format("https://return.gst.gov.in/returns/auth/api/offline/upload/summary?rtn_prd=032018&rtn_typ=GSTR9C")), "https://return.gst.gov.in/returns2/auth/gstr9c/offlineupload");
                    //this.response = (HttpWebResponse)httpWebRequest.GetResponse();
                    //Stream responseStream = this.response.GetResponseStream();
                    //reply = (new StreamReader(responseStream, Encoding.UTF8)).ReadToEnd();
                    //bool flagstatus1 = false;

                    JObject jobj = JObject.Parse(reply);
                    JObject jdata = (JObject)jobj["data"];
                    //JObject jupload = (JObject)jdata["upload"];
                    JArray jupld = (JArray)jdata["upload"];

                    // JArray arr = JArray.Parse(reply);
                    string sql = "";
                  
                    //sql = "Delete from SPQ_UploadSummary";
                    //MC.sqlcmd = new SQLiteCommand(sql, MC.con);
                    //MC.sqlcmd.ExecuteNonQuery();
                    for (int i = 0; i < jupld.Count; i++)
                    {

                        string num = Convert.ToString(jupld[i]["num"]);
                        string date = Convert.ToString(jupld[i]["date"]);
                        string time = Convert.ToString(jupld[i]["time"]);
                        string ref_id = Convert.ToString(jupld[i]["ref_id"]);
                        string status = Convert.ToString(jupld[i]["status"]);
                        string er_token = Convert.ToString(jupld[i]["er_token"]);
                        string er_status = Convert.ToString(jupld[i]["er_status"]);

                    }
                    DataTable dt = new DataTable();
                    //dt = MC.GetValueindatatable("Select  Fld_Date, Fld_Time ,Fld_ref_id,Fld_status,Fld_Downloads" +
                    //"CASE WHEN Fld_status = 'P' THEN 'Processed' " +
                    //"WHEN Fld_status = 'PE' THEN 'Processed With error'" +
                    //"END AS QuantityText FROM SPQ_UploadSummary");
                    dt = MC.GetValueindatatable("SELECT  Fld_Date, Fld_Time,Fld_ref_id,CASE WHEN Fld_status = 'P' THEN 'Processed'WHEN Fld_status = 'PE' THEN 'Processed with error' END as 'Status' , Fld_Downloads FROM SPQ_UploadSummary");
                    Grid_certification.DataSource = dt;
                    MC.InitializeColumn(Grid_certification, 0, "Date", 200, true, DataGridViewContentAlignment.MiddleCenter);
                    MC.InitializeColumn(Grid_certification, 1, "Time", 200, true, DataGridViewContentAlignment.MiddleCenter);
                    MC.InitializeColumn(Grid_certification, 2, "Reference Id", 300, true, DataGridViewContentAlignment.MiddleCenter);
                    MC.InitializeColumn(Grid_certification, 3, "Status", 200, true, DataGridViewContentAlignment.MiddleCenter);
                    MC.InitializeColumn(Grid_certification, 4, "Error Reports", 200, true, DataGridViewContentAlignment.MiddleCenter);

                   
                  
                    //DataGridViewLinkColumn link = new DataGridViewLinkColumn();
                    //link.HeaderText = "Action";
                    //link.Name = "pdf";
                    //link.UseColumnTextForLinkValue = true;
                    //link.Text = "View PDF";
                    //link.Width = 60;
                    //dgv_view.Columns.Add(link);
                    //+ System.Diagnostics.Process.Start(Convert.ToString(dgv_view.Rows[0].Cells[3].Value));

                }
                else
                {
                    SPQGstLogin frm = new SPQGstLogin();
                    frm.strBulk = "BULK";
                    frm.Visible = false;
                    var result = frm.ShowDialog();
                    if (result != DialogResult.OK)
                    {
                        DownloadLiveData();
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
                MC.Close();
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
            }
            return httpWebRequest;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Grid_certification_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Grid_certification_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 5)
            {
                HttpWebRequest httpWebRequest = this.PrepareGetRequestTdsTcs(new Uri(string.Format("https://return.gst.gov.in/returns/auth/api/offline/upload/error/generate?ref_id=a8cad1ed-1061-43ac-993a-51ce2017fc37&rtn_prd=032018&rtn_typ=GSTR9C")), " https://return.gst.gov.in/returns/auth/api/offline/upload/error/report/url?token=66ebc40febc6467eac57d0ceb0e876002&rtn_prd=032018&rtn_typ=GSTR9C");
                this.response = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = this.response.GetResponseStream();
                string reply = (new StreamReader(responseStream, Encoding.UTF8)).ReadToEnd();
                bool flagstatus = false;
                

            }
           
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void GST_DownloadSummary_Load(object sender, EventArgs e)
        {

        }
    }
}
