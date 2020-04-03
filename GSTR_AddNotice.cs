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
    public partial class GSTR_AddNotice : Form
    {
        CookieContainer Cc = new CookieContainer();
        HttpWebResponse response;
        MainClass MC = new MainClass();
        public GSTR_AddNotice()
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


                    HttpWebRequest httpWebRequest = this.PrepareGetRequestTdsTcs(new Uri(string.Format("https://return.gst.gov.in/returns/auth/api/offline/upload/summary?rtn_prd=032018&rtn_typ=GSTR9C")), "https://return.gst.gov.in/returns2/auth/gstr9c/offlineupload");
                    this.response = (HttpWebResponse)httpWebRequest.GetResponse();
                    Stream responseStream = this.response.GetResponseStream();
                    reply = (new StreamReader(responseStream, Encoding.UTF8)).ReadToEnd();
                    bool flagstatus = false;

                    JObject jobj = JObject.Parse(reply);
                    JObject jdata = (JObject)jobj["data"];
                    //JObject jupload = (JObject)jdata["upload"];
                    JArray jupld = (JArray)jdata["upload"];

                   // JArray arr = JArray.Parse(reply);
                    string sql = "";
                    //sql = "Delete from SPQViewCertificate";
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

                        // DataSet ds = new DataSet();

                        //string sql = "";
                        sql = "Delete from SPQ_UploadSummary";
                        MC.sqlcmd = new SQLiteCommand(sql, MC.con);
                        MC.sqlcmd.ExecuteNonQuery();
                        //sql = "Delete from SPQViewCertificate";
                        sql = " insert into SPQ_UploadSummary (Fld_Num, Fld_Date,Fld_Time, Fld_ref_id, Fld_status,Fld_er_token,Fld_er_status) " +
                            " VALUES('" + num + "','" + date + "','" + time + "', '" + ref_id + "','" + status + "','"+er_token+"','"+er_status+"')";
                        // sql = sql + " Values ('" + frmno + "','" + frmdc + "','" + isdt + "', '" + doc id + "','" + applnId + "')";

                        MC.sqlcmd = new SQLiteCommand(sql, MC.con);
                        MC.sqlcmd.ExecuteNonQuery();

                        //MC.InitializeColumn(dgv_view, 3, "Downloads", 100, true, DataGridViewContentAlignment.MiddleCenter);

                    }
                    //DataTable dt = new DataTable();
                    //dt = MC.GetValueindatatable("Select    Fld_Date, Fld_Time ,Fld_ref_id,Fld_status From SPQViewCertificate");
                    //dgv_view.DataSource = dt;
                    //MC.InitializeColumn(dgv_view, 0, "From No", 100, true, DataGridViewContentAlignment.MiddleCenter);
                    //MC.InitializeColumn(dgv_view, 1, "Form Description", 300, true, DataGridViewContentAlignment.MiddleCenter);
                    //MC.InitializeColumn(dgv_view, 2, "Date Of Issue", 200, true, DataGridViewContentAlignment.MiddleCenter);
                    //MC.InitializeColumn(dgv_view, 3, "Downloads", 200, false, DataGridViewContentAlignment.MiddleCenter);


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
    }
}
