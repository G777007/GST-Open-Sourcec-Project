using DataLayer;
using Newtonsoft.Json;
using Proactive.CustomTools.CustomDataGridView;
using SPEQTAGST.BAL;
using SPQ.Automation;
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


namespace SPEQTAGST.xasjbr1
{
    public partial class SPQGSTR1GetSummaryNew : Form
    {
        DataTable dt = new DataTable();
        MainClass MC = new MainClass();
        string strQuery = "";

        private HttpWebResponse response;
        AssesseeDetail assesseeModel;
        CookieContainer Cc = new CookieContainer();
        public SPQGSTR1GetSummaryNew()
        {
            InitializeComponent();
            MC.Connection();
            lblYear.Text = CommonHelper.SelectedYear;

            SetDefaultSettingForControl(tabControl);
        }
 
        private void SPQGSTR1GetSummaryNew_Load(object sender, EventArgs e)
        {
            BindSummaryData();
            BindSummaryAmendedData();
            BindDifference();

            //DataBind_Grd_year(Grd_year);
            //DataBind_Grd_Asummary(Grd_Asummary);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //if (Convert.ToString(CommonHelper.IsMainFormType) == "1Sum") // GSTR-1 Main form
            //{
            //    SPQCompanyDashboard obj = new SPQCompanyDashboard();
            //    obj.MdiParent = this.MdiParent;
            //    Utility.CloseAllOpenForm();
            //    obj.Dock = DockStyle.Fill;
            //    obj.Show();
            //}
            //else
            //{
                SPQGSTR1Dashboard obj = new SPQGSTR1Dashboard();
                obj.MdiParent = this.MdiParent;
                Utility.CloseAllOpenForm();
                obj.Dock = DockStyle.Fill;
                obj.Show();
            //}
        }

        private void DataBind_Grd_year(DataGridView Grd_year)
        {
            Grd_year.Rows.Add(32);

            Grd_year.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;
            // Grd_year.Columns[0].DefaultCellStyle.BackColor = Color.Aquamarine;
            Grd_year.Rows[3].DefaultCellStyle.BackColor = Color.Navy;
            Grd_year.Rows[3].DefaultCellStyle.ForeColor = Color.White;
            Grd_year.Rows[3].DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 9, FontStyle.Bold);

            Grd_year.Rows[7].DefaultCellStyle.BackColor = Color.Navy;
            Grd_year.Rows[7].DefaultCellStyle.ForeColor = Color.White;
            Grd_year.Rows[7].DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 9, FontStyle.Bold);

            Grd_year.Rows[11].DefaultCellStyle.BackColor = Color.Navy;
            Grd_year.Rows[11].DefaultCellStyle.ForeColor = Color.White;
            Grd_year.Rows[11].DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 9, FontStyle.Bold);

            Grd_year.Rows[15].DefaultCellStyle.BackColor = Color.Navy;
            Grd_year.Rows[15].DefaultCellStyle.ForeColor = Color.White;
            Grd_year.Rows[15].DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 9, FontStyle.Bold);

            Grd_year.Rows[19].DefaultCellStyle.BackColor = Color.Navy;
            Grd_year.Rows[19].DefaultCellStyle.ForeColor = Color.White;
            Grd_year.Rows[19].DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 9, FontStyle.Bold);


            Grd_year.Rows[23].DefaultCellStyle.BackColor = Color.Navy;
            Grd_year.Rows[23].DefaultCellStyle.ForeColor = Color.White;
            Grd_year.Rows[23].DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 9, FontStyle.Bold);

            Grd_year.Rows[27].DefaultCellStyle.BackColor = Color.Navy;
            Grd_year.Rows[27].DefaultCellStyle.ForeColor = Color.White;
            Grd_year.Rows[27].DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 9, FontStyle.Bold);


            Grd_year.Rows[31].DefaultCellStyle.BackColor = Color.Navy;
            Grd_year.Rows[31].DefaultCellStyle.ForeColor = Color.White;
            Grd_year.Rows[31].DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 9, FontStyle.Bold);

            Grd_year.Rows[8].Cells[9].Style.BackColor = Color.Silver;
            Grd_year.Rows[9].Cells[9].Style.BackColor = Color.Silver;
            Grd_year.Rows[10].Cells[9].Style.BackColor = Color.Silver;
            Grd_year.Rows[16].Cells[9].Style.BackColor = Color.Silver;
            Grd_year.Rows[17].Cells[9].Style.BackColor = Color.Silver;
            Grd_year.Rows[18].Cells[9].Style.BackColor = Color.Silver;
            Grd_year.Rows[20].Cells[9].Style.BackColor = Color.Silver;
            Grd_year.Rows[21].Cells[9].Style.BackColor = Color.Silver;
            Grd_year.Rows[22].Cells[9].Style.BackColor = Color.Silver;
            Grd_year.Rows[24].Cells[9].Style.BackColor = Color.Silver;
            Grd_year.Rows[25].Cells[9].Style.BackColor = Color.Silver;
            Grd_year.Rows[26].Cells[9].Style.BackColor = Color.Silver;
            Grd_year.Rows[28].Cells[9].Style.BackColor = Color.Silver;
            Grd_year.Rows[29].Cells[9].Style.BackColor = Color.Silver;
            Grd_year.Rows[30].Cells[9].Style.BackColor = Color.Silver;

            Grd_year[0, 0].Value = "";
            Grd_year[1, 0].Value = "GST Portal Summary";
            Grd_year[2, 0].Value = "";

            Grd_year[0, 1].Value = "";
            Grd_year[1, 1].Value = "Software Summary";
            Grd_year[2, 1].Value = "";

            Grd_year[0, 2].Value = "";
            Grd_year[1, 2].Value = "Difference";
            Grd_year[2, 2].Value = "";

            Grd_year[0, 3].Value = "2";
            Grd_year[1, 3].Value = "B2B Large Invoices";
            Grd_year[2, 3].Value = "No of Invoices";
            Grd_year[3, 3].Value = "Taxable Value";
            Grd_year[4, 3].Value = "B2B Large Invoices";
            Grd_year[5, 3].Value = "IGST";
            Grd_year[6, 3].Value = "CGST";
            Grd_year[7, 3].Value = "SGST";
            Grd_year[8, 3].Value = "CESS";
            Grd_year[9, 3].Value = "Total Value";

            Grd_year[0, 4].Value = "";
            Grd_year[1, 4].Value = "GST Portal Summary";
            Grd_year[2, 4].Value = "";

            Grd_year[0, 5].Value = "";
            Grd_year[1, 5].Value = "Software Summary";
            Grd_year[2, 5].Value = "";

            Grd_year[0, 6].Value = "";
            Grd_year[1, 6].Value = "Difference";
            Grd_year[2, 6].Value = "";

            Grd_year[0, 7].Value = "3";
            Grd_year[1, 7].Value = "B2B Small";
            Grd_year[2, 7].Value = "No of Records";
            Grd_year[3, 7].Value = "Taxable Value";
            Grd_year[4, 7].Value = "B2B Large Invoices";
            Grd_year[5, 7].Value = "IGST";
            Grd_year[6, 7].Value = "CGST";
            Grd_year[7, 7].Value = "SGST";
            Grd_year[8, 7].Value = "CESS";
            Grd_year[9, 7].Value = "Total Value";

            Grd_year[0, 8].Value = "";
            Grd_year[1, 8].Value = "GST Portal Summary";
            Grd_year[2, 8].Value = "";

            Grd_year[0, 9].Value = "";
            Grd_year[1, 9].Value = "Software Summary";
            Grd_year[2, 9].Value = "";

            Grd_year[0, 10].Value = "";
            Grd_year[1, 10].Value = "Difference";
            Grd_year[2, 10].Value = "";


            Grd_year[0, 11].Value = "4";
            Grd_year[1, 11].Value = "Export Invoices";
            Grd_year[2, 11].Value = "No of Invoices";
            Grd_year[3, 11].Value = "Taxable Value";
            Grd_year[4, 11].Value = "B2B Large Invoices";
            Grd_year[5, 11].Value = "IGST";
            Grd_year[6, 11].Value = "CGST";
            Grd_year[7, 11].Value = "SGST";
            Grd_year[8, 11].Value = "CESS";
            Grd_year[9, 11].Value = "Total Value";

            Grd_year[0, 12].Value = "";
            Grd_year[1, 12].Value = "GST Portal Summary";
            Grd_year[2, 12].Value = "";

            Grd_year[0, 13].Value = "";
            Grd_year[1, 13].Value = "Software Summary";
            Grd_year[2, 13].Value = "";

            Grd_year[0, 14].Value = "";
            Grd_year[1, 14].Value = "Difference";
            Grd_year[2, 14].Value = "";

            Grd_year[0, 15].Value = "5";
            Grd_year[1, 15].Value = "Cr. / Dr. Note (Reg)";
            Grd_year[2, 15].Value = "No of Notes";
            Grd_year[3, 15].Value = "Taxable Value";
            Grd_year[4, 15].Value = "B2B Large Invoices";
            Grd_year[5, 15].Value = "IGST";
            Grd_year[6, 15].Value = "CGST";
            Grd_year[7, 15].Value = "SGST";
            Grd_year[8, 15].Value = "CESS";



            Grd_year[0, 16].Value = "";
            Grd_year[1, 16].Value = "GST Portal Summary";
            Grd_year[2, 16].Value = "";

            Grd_year[0, 17].Value = "";
            Grd_year[1, 17].Value = "Software Summary";
            Grd_year[2, 17].Value = "";

            Grd_year[0, 18].Value = "";
            Grd_year[1, 18].Value = "Difference";
            Grd_year[2, 18].Value = "";

            Grd_year[0, 19].Value = "6";
            Grd_year[1, 19].Value = "Cr. / Dr. Note (URD)";
            Grd_year[2, 19].Value = "No of Invoices";
            Grd_year[3, 19].Value = "Taxable Value";
            Grd_year[4, 19].Value = "B2B Large Invoices";
            Grd_year[5, 19].Value = "IGST";
            Grd_year[6, 19].Value = "CGST";
            Grd_year[7, 19].Value = "SGST";
            Grd_year[8, 19].Value = "CESS";

            Grd_year[0, 20].Value = "";
            Grd_year[1, 20].Value = "GST Portal Summary";
            Grd_year[2, 20].Value = "";

            Grd_year[0, 21].Value = "";
            Grd_year[1, 21].Value = "Software Summary";
            Grd_year[2, 21].Value = "";

            Grd_year[0, 22].Value = "";
            Grd_year[1, 22].Value = "Difference";
            Grd_year[2, 22].Value = "";

            Grd_year[0, 23].Value = "7";
            Grd_year[1, 23].Value = "Nil / Exempted / Non GST	";
            Grd_year[2, 23].Value = "No of Records";
            Grd_year[3, 23].Value = "Taxable Value";
            Grd_year[4, 23].Value = "B2B Large Invoices";
            Grd_year[5, 23].Value = "IGST";
            Grd_year[6, 23].Value = "CGST";
            Grd_year[7, 23].Value = "SGST";
            Grd_year[8, 23].Value = "CESS";

            Grd_year[0, 24].Value = "";
            Grd_year[1, 24].Value = "GST Portal Summary";
            Grd_year[2, 24].Value = "";

            Grd_year[0, 25].Value = "";
            Grd_year[1, 25].Value = "Software Summary";
            Grd_year[2, 25].Value = "";

            Grd_year[0, 26].Value = "";
            Grd_year[1, 26].Value = "Difference";
            Grd_year[2, 26].Value = "";

            Grd_year[0, 27].Value = "8";
            Grd_year[1, 27].Value = "Advance Received";
            Grd_year[2, 27].Value = "No of Records";
            Grd_year[3, 27].Value = "Taxable Value";
            Grd_year[4, 27].Value = "B2B Large Invoices";
            Grd_year[5, 27].Value = "IGST";
            Grd_year[6, 27].Value = "CGST";
            Grd_year[7, 27].Value = "SGST";
            Grd_year[8, 27].Value = "CESS";

            Grd_year[0, 28].Value = "";
            Grd_year[1, 28].Value = "GST Portal Summary";
            Grd_year[2, 28].Value = "";

            Grd_year[0, 29].Value = "";
            Grd_year[1, 29].Value = "Software Summary";
            Grd_year[2, 29].Value = "";

            Grd_year[0, 30].Value = "";
            Grd_year[1, 30].Value = "Difference";
            Grd_year[2, 30].Value = "";

            Grd_year[0, 31].Value = "9";
            Grd_year[1, 31].Value = "Advance Adjusted";
            Grd_year[2, 31].Value = "No of Records";
            Grd_year[3, 31].Value = "Taxable Value";
            Grd_year[4, 31].Value = "B2B Large Invoices";
            Grd_year[5, 31].Value = "IGST";
            Grd_year[6, 31].Value = "CGST";
            Grd_year[7, 31].Value = "SGST";
            Grd_year[8, 31].Value = "CESS";




            foreach (DataGridViewColumn col in Grd_year.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void DataBind_Grd_Asummary(DataGridView Grd_Asummary)
        {

            Grd_Asummary.Rows.Add(32);

            Grd_Asummary.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;
            // Grd_year.Columns[0].DefaultCellStyle.BackColor = Color.Aquamarine;
            Grd_Asummary.Rows[3].DefaultCellStyle.BackColor = Color.Navy;
            Grd_Asummary.Rows[3].DefaultCellStyle.ForeColor = Color.White;
            Grd_Asummary.Rows[3].DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 9, FontStyle.Bold);

            Grd_Asummary.Rows[7].DefaultCellStyle.BackColor = Color.Navy;
            Grd_Asummary.Rows[7].DefaultCellStyle.ForeColor = Color.White;
            Grd_Asummary.Rows[7].DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 9, FontStyle.Bold);

            Grd_Asummary.Rows[11].DefaultCellStyle.BackColor = Color.Navy;
            Grd_Asummary.Rows[11].DefaultCellStyle.ForeColor = Color.White;
            Grd_Asummary.Rows[11].DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 9, FontStyle.Bold);

            Grd_Asummary.Rows[15].DefaultCellStyle.BackColor = Color.Navy;
            Grd_Asummary.Rows[15].DefaultCellStyle.ForeColor = Color.White;
            Grd_Asummary.Rows[15].DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 9, FontStyle.Bold);

            Grd_Asummary.Rows[19].DefaultCellStyle.BackColor = Color.Navy;
            Grd_Asummary.Rows[19].DefaultCellStyle.ForeColor = Color.White;
            Grd_Asummary.Rows[19].DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 9, FontStyle.Bold);


            Grd_Asummary.Rows[23].DefaultCellStyle.BackColor = Color.Navy;
            Grd_Asummary.Rows[23].DefaultCellStyle.ForeColor = Color.White;
            Grd_Asummary.Rows[23].DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 9, FontStyle.Bold);

            Grd_Asummary.Rows[27].DefaultCellStyle.BackColor = Color.Navy;
            Grd_Asummary.Rows[27].DefaultCellStyle.ForeColor = Color.White;
            Grd_Asummary.Rows[27].DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 9, FontStyle.Bold);


            Grd_Asummary.Rows[31].DefaultCellStyle.BackColor = Color.Navy;
            Grd_Asummary.Rows[31].DefaultCellStyle.ForeColor = Color.White;
            Grd_Asummary.Rows[31].DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 9, FontStyle.Bold);

            Grd_Asummary.Rows[8].Cells[9].Style.BackColor = Color.Silver;
            Grd_Asummary.Rows[9].Cells[9].Style.BackColor = Color.Silver;
            Grd_Asummary.Rows[10].Cells[9].Style.BackColor = Color.Silver;
            Grd_Asummary.Rows[16].Cells[9].Style.BackColor = Color.Silver;
            Grd_Asummary.Rows[17].Cells[9].Style.BackColor = Color.Silver;
            Grd_Asummary.Rows[18].Cells[9].Style.BackColor = Color.Silver;
            Grd_Asummary.Rows[20].Cells[9].Style.BackColor = Color.Silver;
            Grd_Asummary.Rows[21].Cells[9].Style.BackColor = Color.Silver;
            Grd_Asummary.Rows[22].Cells[9].Style.BackColor = Color.Silver;
            Grd_Asummary.Rows[24].Cells[9].Style.BackColor = Color.Silver;
            Grd_Asummary.Rows[25].Cells[9].Style.BackColor = Color.Silver;
            Grd_Asummary.Rows[26].Cells[9].Style.BackColor = Color.Silver;
            Grd_Asummary.Rows[28].Cells[9].Style.BackColor = Color.Silver;
            Grd_Asummary.Rows[29].Cells[9].Style.BackColor = Color.Silver;
            Grd_Asummary.Rows[30].Cells[9].Style.BackColor = Color.Silver;

            Grd_Asummary[0, 0].Value = "";
            Grd_Asummary[1, 0].Value = "GST Portal Summary";
            Grd_Asummary[2, 0].Value = "";

            Grd_Asummary[0, 1].Value = "";
            Grd_Asummary[1, 1].Value = "Software Summary";
            Grd_Asummary[2, 1].Value = "";

            Grd_Asummary[0, 2].Value = "";
            Grd_Asummary[1, 2].Value = "Difference";
            Grd_Asummary[2, 2].Value = "";

            Grd_Asummary[0, 3].Value = "2";
            Grd_Asummary[1, 3].Value = "B2B Amendment";
            Grd_Asummary[2, 3].Value = "No of Invoices";
            Grd_Asummary[3, 3].Value = "Taxable Value";
            Grd_Asummary[4, 3].Value = "B2B Large Invoices";
            Grd_Asummary[5, 3].Value = "IGST";
            Grd_Asummary[6, 3].Value = "CGST";
            Grd_Asummary[7, 3].Value = "SGST";
            Grd_Asummary[8, 3].Value = "CESS";
            Grd_Asummary[9, 3].Value = "Total Value";

            Grd_Asummary[0, 4].Value = "";
            Grd_Asummary[1, 4].Value = "GST Portal Summary";
            Grd_Asummary[2, 4].Value = "";

            Grd_Asummary[0, 5].Value = "";
            Grd_Asummary[1, 5].Value = "Software Summary";
            Grd_Asummary[2, 5].Value = "";

            Grd_Asummary[0, 6].Value = "";
            Grd_Asummary[1, 6].Value = "Difference";
            Grd_Asummary[2, 6].Value = "";

            Grd_Asummary[0, 7].Value = "3";
            Grd_Asummary[1, 7].Value = "B2C Large Amendment";
            Grd_Asummary[2, 7].Value = "No of Records";
            Grd_Asummary[3, 7].Value = "Taxable Value";
            Grd_Asummary[4, 7].Value = "B2B Large Invoices";
            Grd_Asummary[5, 7].Value = "IGST";
            Grd_Asummary[6, 7].Value = "CGST";
            Grd_Asummary[7, 7].Value = "SGST";
            Grd_Asummary[8, 7].Value = "CESS";
            Grd_Asummary[9, 7].Value = "Total Value";

            Grd_Asummary[0, 8].Value = "";
            Grd_Asummary[1, 8].Value = "GST Portal Summary";
            Grd_Asummary[2, 8].Value = "";

            Grd_Asummary[0, 9].Value = "";
            Grd_Asummary[1, 9].Value = "Software Summary";
            Grd_Asummary[2, 9].Value = "";

            Grd_Asummary[0, 10].Value = "";
            Grd_Asummary[1, 10].Value = "Difference";
            Grd_Asummary[2, 10].Value = "";


            Grd_Asummary[0, 11].Value = "4";
            Grd_Asummary[1, 11].Value = "Export Invoices Amendment";
            Grd_Asummary[2, 11].Value = "No of Invoices";
            Grd_Asummary[3, 11].Value = "Taxable Value";
            Grd_Asummary[4, 11].Value = "B2B Large Invoices";
            Grd_Asummary[5, 11].Value = "IGST";
            Grd_Asummary[6, 11].Value = "CGST";
            Grd_Asummary[7, 11].Value = "SGST";
            Grd_Asummary[8, 11].Value = "CESS";
            Grd_Asummary[9, 11].Value = "Total Value";

            Grd_Asummary[0, 12].Value = "";
            Grd_Asummary[1, 12].Value = "GST Portal Summary";
            Grd_Asummary[2, 12].Value = "";

            Grd_Asummary[0, 13].Value = "";
            Grd_Asummary[1, 13].Value = "Software Summary";
            Grd_Asummary[2, 13].Value = "";

            Grd_Asummary[0, 14].Value = "";
            Grd_Asummary[1, 14].Value = "Difference";
            Grd_Asummary[2, 14].Value = "";

            Grd_Asummary[0, 15].Value = "5";
            Grd_Asummary[1, 15].Value = "Cr. / Dr. Note (Reg) Amend";
            Grd_Asummary[2, 15].Value = "No of Notes";
            Grd_Asummary[3, 15].Value = "Taxable Value";
            Grd_Asummary[4, 15].Value = "B2B Large Invoices";
            Grd_Asummary[5, 15].Value = "IGST";
            Grd_Asummary[6, 15].Value = "CGST";
            Grd_Asummary[7, 15].Value = "SGST";
            Grd_Asummary[8, 15].Value = "CESS";



            Grd_Asummary[0, 16].Value = "";
            Grd_Asummary[1, 16].Value = "GST Portal Summary";
            Grd_Asummary[2, 16].Value = "";

            Grd_Asummary[0, 17].Value = "";
            Grd_Asummary[1, 17].Value = "Software Summary";
            Grd_Asummary[2, 17].Value = "";

            Grd_Asummary[0, 18].Value = "";
            Grd_Asummary[1, 18].Value = "Difference";
            Grd_Asummary[2, 18].Value = "";

            Grd_Asummary[0, 19].Value = "6";
            Grd_Asummary[1, 19].Value = "Cr. / Dr. Note (URD) Amend";
            Grd_Asummary[2, 19].Value = "No of Invoices";
            Grd_Asummary[3, 19].Value = "Taxable Value";
            Grd_Asummary[4, 19].Value = "B2B Large Invoices";
            Grd_Asummary[5, 19].Value = "IGST";
            Grd_Asummary[6, 19].Value = "CGST";
            Grd_Asummary[7, 19].Value = "SGST";
            Grd_Asummary[8, 19].Value = "CESS";

            Grd_Asummary[0, 20].Value = "";
            Grd_Asummary[1, 20].Value = "GST Portal Summary";
            Grd_Asummary[2, 20].Value = "";

            Grd_Asummary[0, 21].Value = "";
            Grd_Asummary[1, 21].Value = "Software Summary";
            Grd_Asummary[2, 21].Value = "";

            Grd_Asummary[0, 22].Value = "";
            Grd_Asummary[1, 22].Value = "Difference";
            Grd_Asummary[2, 22].Value = "";

            Grd_Asummary[0, 23].Value = "7";
            Grd_Asummary[1, 23].Value = "Advance Received Amend";
            Grd_Asummary[2, 23].Value = "No of Records";
            Grd_Asummary[3, 23].Value = "Taxable Value";
            Grd_Asummary[4, 23].Value = "B2B Large Invoices";
            Grd_Asummary[5, 23].Value = "IGST";
            Grd_Asummary[6, 23].Value = "CGST";
            Grd_Asummary[7, 23].Value = "SGST";
            Grd_Asummary[8, 23].Value = "CESS";

            Grd_Asummary[0, 24].Value = "";
            Grd_Asummary[1, 24].Value = "GST Portal Summary";
            Grd_Asummary[2, 24].Value = "";

            Grd_Asummary[0, 25].Value = "";
            Grd_Asummary[1, 25].Value = "Software Summary";
            Grd_Asummary[2, 25].Value = "";

            Grd_Asummary[0, 26].Value = "";
            Grd_Asummary[1, 26].Value = "Difference";
            Grd_Asummary[2, 26].Value = "";

            Grd_Asummary[0, 27].Value = "8";
            Grd_Asummary[1, 27].Value = "Advance Received Amend	";
            Grd_Asummary[2, 27].Value = "No of Records";
            Grd_Asummary[3, 27].Value = "Taxable Value";
            Grd_Asummary[4, 27].Value = "B2B Large Invoices";
            Grd_Asummary[5, 27].Value = "IGST";
            Grd_Asummary[6, 27].Value = "CGST";
            Grd_Asummary[7, 27].Value = "SGST";
            Grd_Asummary[8, 27].Value = "CESS";

            Grd_Asummary[0, 28].Value = "";
            Grd_Asummary[1, 28].Value = "GST Portal Summary";
            Grd_Asummary[2, 28].Value = "";

            Grd_Asummary[0, 29].Value = "";
            Grd_Asummary[1, 29].Value = "Software Summary";
            Grd_Asummary[2, 29].Value = "";

            Grd_Asummary[0, 30].Value = "";
            Grd_Asummary[1, 30].Value = "Difference";
            Grd_Asummary[2, 30].Value = "";

            Grd_Asummary[0, 31].Value = "9";
            Grd_Asummary[1, 31].Value = "Advance Adjusted Amend";
            Grd_Asummary[2, 31].Value = "No of Records";
            Grd_Asummary[3, 31].Value = "Taxable Value";
            Grd_Asummary[4, 31].Value = "B2B Large Invoices";
            Grd_Asummary[5, 31].Value = "IGST";
            Grd_Asummary[6, 31].Value = "CGST";
            Grd_Asummary[7, 31].Value = "SGST";
            Grd_Asummary[8, 31].Value = "CESS";




            foreach (DataGridViewColumn col in Grd_Asummary.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        public void SetDefaultSettingForControl(Control frm)
        {
            foreach (Control ctr in frm.Controls)
            {
                string x = ctr.GetType().ToString();

                if (ctr.GetType().ToString() == "System.Windows.Forms.Panel" || (ctr.GetType().ToString() == "System.Windows.Forms.GroupBox") || (ctr.GetType().ToString() == "System.Windows.Forms.TabControl") || ctr.GetType().ToString() == "System.Windows.Forms.TabPage")
                {
                    SetDefaultSettingForControl(ctr);
                }

                if (ctr.GetType().ToString() == "Proactive.CustomTools.CustomDataGridView.CustomDataGridViews")
                {

                    CustomDataGridViews grd = (CustomDataGridViews)ctr;
                    //grd.ReadOnly = true;
                    grd.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
                    grd.EnableHeadersVisualStyles = false;
                    grd.ColumnHeadersDefaultCellStyle.Font = new Font("Verdana", 8, FontStyle.Bold);
                    grd.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    grd.DefaultCellStyle.Font = new Font("Verdana", 8, FontStyle.Regular);
                    grd.RowsDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(255, 226, 171);
                    grd.RowsDefaultCellStyle.SelectionForeColor = Color.Black;
                    //grd.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    grd.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    //grd.CellClick += DataGrdView_CellClick;

                    //TextBox TextBox;
                    //TextBox = (TextBox)ctr;
                    //TextBox.Text = "";
                }

                if (ctr.GetType().ToString() == "System.Windows.Forms.DataGridView")
                {

                    DataGridView grd = (DataGridView)ctr;
                    //grd.ReadOnly = true;
                    grd.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
                    grd.EnableHeadersVisualStyles = false;
                    grd.ColumnHeadersDefaultCellStyle.Font = new Font("Verdana", 8, FontStyle.Bold);
                    grd.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    grd.DefaultCellStyle.Font = new Font("Verdana", 8, FontStyle.Regular);
                    grd.RowsDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(255, 226, 171);
                    grd.RowsDefaultCellStyle.SelectionForeColor = Color.Black;
                    //grd.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    grd.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    //grd.RowsDefaultCellStyle.SelectionBackColor = System.Drawing.Color.LemonChiffon;
                    //255, 226, 171
                    //grd.CellClick += DataGrdView_CellClick;

                    //TextBox TextBox;
                    //TextBox = (TextBox)ctr;
                    //TextBox.Text = "";
                }

                //if (ctr.GetType().ToString() == "System.Windows.Forms.TextBox")
                //{
                //    TextBox TextBox;
                //    TextBox = (TextBox)ctr;
                //    TextBox.Text = "";
                //}
                //if (ctr.GetType().ToString() == "System.Windows.Forms.ComboBox")
                //{
                //    ComboBox combo;
                //    combo = (ComboBox)ctr;
                //    if (combo.Items.Count > 0)
                //    {
                //        combo.SelectedIndex = 0;
                //    }

                //}
                //if (ctr.GetType().ToString() == "System.Windows.Forms.MaskedTextBox")
                //{
                //    MaskedTextBox txtMask;
                //    txtMask = (MaskedTextBox)ctr;
                //    txtMask.Text = "";
                //}

                //if (ctr.GetType().ToString() == "System.Windows.Forms.RadioButton")
                //{
                //    RadioButton rad;
                //    rad = (RadioButton)ctr;
                //    rad.Checked = false;
                //}
                //if (ctr.GetType().ToString() == "System.Windows.Forms.ListBox")
                //{
                //    ListBox lstbox;
                //    lstbox = (ListBox)ctr;
                //    if (lstbox.Items.Count > 0)
                //    {
                //        lstbox.Items.Clear();
                //    }
                //}
                //if (ctr.GetType().ToString() == "System.Windows.Forms.CheckBox")
                //{
                //    CheckBox chkbox;
                //    chkbox = (CheckBox)ctr;
                //    if (chkbox.Checked)
                //    {
                //        chkbox.Checked = false;
                //    }
                //}
                //if (ctr.GetType().ToString() == "System.Windows.Forms.DateTimePicker")
                //{
                //    DateTimePicker DTPicker;
                //    DTPicker = (DateTimePicker)ctr;
                //    if (DTPicker.Value.ToString() != DateTime.Now.Date.ToString())
                //    {
                //        DTPicker.Value = DateTime.Now.Date;
                //    }
                //}
                //if (ctr.GetType().ToString() == "System.Windows.Forms.PictureBox")
                //{
                //    PictureBox PIC;
                //    PIC = (PictureBox)ctr;
                //    if (PIC.ImageLocation != "")
                //    {
                //        PIC.Image = Payroll.Properties.Resources.NoPhoto;
                //    }
                //}
            }
        }

        private void BindSummaryData()
        {
         

            // B2B Sumamry
            #region B2B Summary
            strQuery = " Select  'B2B' Type , 'Portal Summary' Description,  ifnull(Fld_InvoiceNo,0) TotalInvoice, " +
                       " ifnull(sum(replace(Fld_TaxValue,',','')),0.00) TaxableValue, " +
                       " ifnull(sum(replace(Fld_IGST,',','')),0.00) IGST, " +
                       " ifnull(sum(replace(Fld_CGST,',','')),0.00)  CGST, " +
                       " ifnull(sum(replace(Fld_SGST,',','')),0.00) SGST, " +
                       " ifnull( sum(replace(Fld_CESS,',','')),0.00)  Cess, " +
                       " (ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                       " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00)) TotalGST, " +
                       " (ifnull(sum(replace(Fld_TaxValue,',','')),0.00) + ( ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                       " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00))) as InvoiceValue " +
                       " from SPQGSTNSummary where Fld_SectionName='B2B' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' " +
                      " union all " +
                      " select 'B2B' Type ,'Software Summary' Description, count(*) TotalInvoice, " +
                          "  ifnull(sum(replace(Fld_InvoiceTaxableVal,',','')),0.00) TaxableValue, " +
                          "  ifnull(sum(replace(Fld_IGSTAmnt,',','')),0.00) IGST, " +
                          "  ifnull(sum(replace(Fld_CGSTAmnt,',','')),0.00)  CGST, " +
                          "  ifnull(sum(replace(Fld_SGSTAmnt,',','')),0.00) SGST, " +
                          "  ifnull( sum(replace(Fld_CessAmount,',','')),0.00)  Cess, " +
                          "  (ifnull(sum(replace(Fld_IGSTAmnt,',','')),0.00) + ifnull(sum(replace(Fld_CGSTAmnt,',','')),0.00) + " +
                          "  ifnull(sum(replace(Fld_SGSTAmnt,',','')),0.00) +  ifnull( sum(replace(Fld_CessAmount,',','')),0.00)) TotalGST, " +
                        " ifnull(sum(replace(Fld_InvoiceValue,',','')),0.00) InvoiceValue " +
                       " from SPQR1B2B where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
            strQuery += " union all select 'B2B' ,'Difference',0 ,0 ,0,0,0,0,0,0 ";
          
            dt = MC.GetValueindatatable(strQuery);
            foreach (DataGridViewColumn col in grdB2b.Columns)
            {
                col.DataPropertyName = dt.Columns[col.Index].ColumnName;
                if (col.Index > 1)
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = string.Format("N2");
                    col.DefaultCellStyle.NullValue = "-";
                }

            }
            grdB2b.DataSource = dt;
            #endregion


            // B2CL Sumamry
            #region B2CL Sumamry
            strQuery = " Select 'B2CL' Type ,  'Portal Summary' Description,  ifnull(Fld_InvoiceNo,0) TotalInvoice, " +
                    " ifnull(sum(replace(Fld_TaxValue,',','')),0.00) TaxableValue, " +
                    " ifnull(sum(replace(Fld_IGST,',','')),0.00) IGST, " +
                    " ifnull(sum(replace(Fld_CGST,',','')),0.00)  CGST, " +
                    " ifnull(sum(replace(Fld_SGST,',','')),0.00) SGST, " +
                    " ifnull( sum(replace(Fld_CESS,',','')),0.00)  Cess, " +
                    " (ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                    " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00)) TotalGST, " +
                    " (ifnull(sum(replace(Fld_TaxValue,',','')),0.00) + ( ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                    " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00))) as InvoiceValue " +
                    " from SPQGSTNSummary where Fld_SectionName='B2CL' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' " +
                   " union all " +
                   " select 'B2CL' Type , 'Software Summary' Description, count(Fld_InvoiceNo) TotalInvoice, " +
                       "  ifnull(sum(replace(Fld_TaxableValue,',','')),0.00) TaxableValue, " +
                       "  ifnull(sum(replace(Fld_IGST,',','')),0.00) IGSTt, " +
                       "  0.00  CGST, " +
                       "  0.00 SGST, " +
                       "  ifnull( sum(replace(Fld_Cess,',','')),0.00)  Cess, " +
                       "  (ifnull(sum(replace(Fld_IGST,',','')),0.00) +  ifnull( sum(replace(Fld_Cess,',','')),0.00)) TotalGST, " +
                     " ifnull(sum(replace(Fld_InvoiceValue,',','')),0.00) InvoiceValue " +
                    " from SPQR1B2CL where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
            strQuery += " union all select 'B2CL' ,'Difference',0 ,0 ,0,0,0,0,0,0 ";
           
             dt = MC.GetValueindatatable(strQuery);
             foreach (DataGridViewColumn col in grdB2cl.Columns)
             {
                 col.DataPropertyName = dt.Columns[col.Index].ColumnName;
                 if (col.Index > 1)
                 {
                     col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                     col.DefaultCellStyle.Format = string.Format("N2");
                     col.DefaultCellStyle.NullValue = "-";
                 }

             }
            grdB2cl.DataSource = dt;
            #endregion
          
            // B2CS
            #region B2CS Query
            strQuery = " Select 'B2CS' Type ,  'Portal Summary' Description,  ifnull(Fld_InvoiceNo,0) TotalInvoice, " +
                    " ifnull(sum(replace(Fld_TaxValue,',','')),0.00) TaxableValue, " +
                    " ifnull(sum(replace(Fld_IGST,',','')),0.00) IGST, " +
                    " ifnull(sum(replace(Fld_CGST,',','')),0.00)  CGST, " +
                    " ifnull(sum(replace(Fld_SGST,',','')),0.00) SGST, " +
                    " ifnull( sum(replace(Fld_CESS,',','')),0.00)  Cess, " +
                    " (ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                    " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00)) TotalGST, " +
                    " (ifnull(sum(replace(Fld_TaxValue,',','')),0.00) + ( ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                    " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00))) as InvoiceValue " +
                    " from SPQGSTNSummary where Fld_SectionName='B2CS' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' " +
                   " union all " +
                   " select 'B2CS' Type , 'Software Summary' Description, count(*) TotalInvoice, " +
                       "  ifnull(sum(replace(Fld_TaxableValue,',','')),0.00) TaxableValue, " +
                       "  ifnull(sum(replace(Fld_IGST,',','')),0.00) IGST, " +
                       " ifnull(sum(replace(Fld_CGST,',','')),0.00) CGST, " +
                       "  ifnull(sum(replace(Fld_SGST,',','')),0.00) SGST, " +
                       "  ifnull( sum(replace(Fld_Cess,',','')),0.00)  Cess, " +
                       "  (ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                       " ifnull(sum(replace(Fld_SGST,',','')),0.00) + ifnull( sum(replace(Fld_Cess,',','')),0.00)) TotalGST, " +
                      " ( ifnull(sum(replace(Fld_TaxableValue,',','')),0.00)+ ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                       " ifnull(sum(replace(Fld_SGST,',','')),0.00) + ifnull( sum(replace(Fld_Cess,',','')),0.00)) InvoiceValue " +
                    " from SPQR1B2CS where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
            strQuery += " union all select 'B2CS' ,'Difference',0 ,0 ,0,0,0,0,0,0 ";
         
            dt = MC.GetValueindatatable(strQuery);
            foreach (DataGridViewColumn col in grdB2cs.Columns)
            {
                col.DataPropertyName = dt.Columns[col.Index].ColumnName;
                if (col.Index > 1)
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = string.Format("N2");
                    col.DefaultCellStyle.NullValue = "-";
                }

            }
            grdB2cs.DataSource = dt;
            #endregion

            // Export Invoices
            #region Export Invoices (SPQR1ZeroRated)

            strQuery = " Select 'EXP' Type ,  'Portal Summary' Description,  ifnull(Fld_InvoiceNo,0) TotalInvoice, " +
                  " ifnull(sum(replace(Fld_TaxValue,',','')),0.00) TaxableValue, " +
                  " ifnull(sum(replace(Fld_IGST,',','')),0.00) IGST, " +
                  " ifnull(sum(replace(Fld_CGST,',','')),0.00)  CGST, " +
                  " ifnull(sum(replace(Fld_SGST,',','')),0.00) SGST, " +
                  " ifnull( sum(replace(Fld_CESS,',','')),0.00)  Cess, " +
                  " (ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                  " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00)) TotalGST, " +
                  " (ifnull(sum(replace(Fld_TaxValue,',','')),0.00) + ( ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                  " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00))) as InvoiceValue " +
                  " from SPQGSTNSummary where Fld_SectionName='EXP' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' " +
                 " union all " +
                 " select 'EXP' Type , 'Software Summary' Description, count(Fld_InvoiceNo) TotalInvoice, " +
                     "  ifnull(sum(replace(Fld_IGSTInvoiceTaxableVal,',','')),0.00) TaxableValue, " +
                     "  ifnull(sum(replace(Fld_IGSTAmnt,',','')),0.00) IGSTt, " +
                     "  0.00  CGST, " +
                     "  0.00 SGST, " +
                     "  ifnull( sum(replace(Fld_CESS,',','')),0.00)  Cess, " +
                     "  (ifnull(sum(replace(Fld_IGSTAmnt,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00)) TotalGST, " +
                   " ifnull(sum(replace(Fld_InvoiceValue,',','')),0.00) InvoiceValue " +
                  " from SPQR1ZeroRated where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
            strQuery += " union all select 'EXP' ,'Difference',0 ,0 ,0,0,0,0,0,0 ";

           
            dt = MC.GetValueindatatable(strQuery);
            foreach (DataGridViewColumn col in grdExport.Columns)
            {
                col.DataPropertyName = dt.Columns[col.Index].ColumnName;
                if (col.Index > 1)
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = string.Format("N2");
                    col.DefaultCellStyle.NullValue = "-";
                }

            }
            grdExport.DataSource = dt;

            #endregion

            // Cr./ Dr. Note Regd 
            #region Cr.Dr. Note Regd.

            strQuery = " Select  'CDN' Type , 'Portal Summary' Description,  ifnull(Fld_InvoiceNo,0) TotalInvoice, " +
                   " ifnull(sum(replace(Fld_TaxValue,',','')),0.00) TaxableValue, " +
                   " ifnull(sum(replace(Fld_IGST,',','')),0.00) IGST, " +
                   " ifnull(sum(replace(Fld_CGST,',','')),0.00)  CGST, " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) SGST, " +
                   " ifnull( sum(replace(Fld_CESS,',','')),0.00)  Cess, " +
                   " (ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00)) TotalGST, " +
                   " (ifnull(sum(replace(Fld_TaxValue,',','')),0.00) + ( ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00))) as InvoiceValue " +
                   " from SPQGSTNSummary where Fld_SectionName='CDN' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' " +
                  " union all " +
                  " select 'CDN' Type ,'Software Summary' Description, (select count(*) from SPQR1CDN where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total') as TotalInvoice, " +
                      "  ifnull(sum(replace(Fld_Taxable,',','')),0.00) TaxableValue, " +
                      "  ifnull(sum(replace(Fld_IGSTAmnt,',','')),0.00) IGST, " +
                      "  ifnull(sum(replace(Fld_CGSTAmnt,',','')),0.00)  CGST, " +
                      "  ifnull(sum(replace(Fld_SGSTAmnt,',','')),0.00) SGST, " +
                      "  ifnull( sum(replace(Fld_CessAmnt,',','')),0.00)  Cess, " +
                      "  (ifnull(sum(replace(Fld_IGSTAmnt,',','')),0.00) + ifnull(sum(replace(Fld_CGSTAmnt,',','')),0.00) + " +
                      "  ifnull(sum(replace(Fld_SGSTAmnt,',','')),0.00) +  ifnull( sum(replace(Fld_CessAmnt,',','')),0.00)) TotalGST, " +
                    " ifnull(sum(replace(Fld_OrgInvoiceValue,',','')),0.00) InvoiceValue " +
                   " from SPQR1CDN where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus = 'Total'";
            strQuery += " union all select 'CDN' ,'Difference',0 ,0 ,0,0,0,0,0,0 ";

        

            dt = MC.GetValueindatatable(strQuery);
            foreach (DataGridViewColumn col in grdCrDrNote.Columns)
            {
                col.DataPropertyName = dt.Columns[col.Index].ColumnName;
                if (col.Index > 1)
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = string.Format("N2");
                    col.DefaultCellStyle.NullValue = "-";
                }

            }
            grdCrDrNote.DataSource = dt;
            #endregion

            // Cr./ Dr. Note Un-Regd 
            #region Cr.Dr. Note Un-Regd.

            strQuery = " Select  'CDNUR' Type , 'Portal Summary' Description,  ifnull(Fld_InvoiceNo,0) TotalInvoice, " +
                   " ifnull(sum(replace(Fld_TaxValue,',','')),0.00) TaxableValue, " +
                   " ifnull(sum(replace(Fld_IGST,',','')),0.00) IGST, " +
                   " ifnull(sum(replace(Fld_CGST,',','')),0.00)  CGST, " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) SGST, " +
                   " ifnull( sum(replace(Fld_CESS,',','')),0.00)  Cess, " +
                   " (ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00)) TotalGST, " +
                   " (ifnull(sum(replace(Fld_TaxValue,',','')),0.00) + ( ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00))) as InvoiceValue " +
                   " from SPQGSTNSummary where Fld_SectionName='CDNUR' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' " +
                  " union all " +
                  " select 'CDNUR' Type ,'Software Summary' Description, count(*) TotalInvoice, " +
                      "  ifnull(sum(replace(Fld_Taxable,',','')),0.00) TaxableValue, " +
                      "  ifnull(sum(replace(Fld_IGSTAmnt,',','')),0.00) IGST, " +
                      "  ifnull(sum(replace(Fld_CGSTAmnt,',','')),0.00)  CGST, " +
                      "  ifnull(sum(replace(Fld_SGSTAmnt,',','')),0.00) SGST, " +
                      "  ifnull( sum(replace(Fld_CessAmnt,',','')),0.00)  Cess, " +
                      "  (ifnull(sum(replace(Fld_IGSTAmnt,',','')),0.00) + ifnull(sum(replace(Fld_CGSTAmnt,',','')),0.00) + " +
                      "  ifnull(sum(replace(Fld_SGSTAmnt,',','')),0.00) +  ifnull( sum(replace(Fld_CessAmnt,',','')),0.00)) TotalGST, " +
                    " ifnull(sum(replace(Fld_OrgInvoiceValue,',','')),0.00) InvoiceValue " +
                   " from SPQR1CDNUR where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
            strQuery += " union all select 'CDNUR' ,'Difference',0 ,0 ,0,0,0,0,0,0 ";

           
            dt = MC.GetValueindatatable(strQuery);
            foreach (DataGridViewColumn col in grdCrDrNoteUnreg.Columns)
            {
                col.DataPropertyName = dt.Columns[col.Index].ColumnName;
                if (col.Index > 1)
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = string.Format("N2");
                    col.DefaultCellStyle.NullValue = "-";
                }

            }
            grdCrDrNoteUnreg.DataSource = dt;

            #endregion

            // Nil Rated 
            #region Nil Rated

            strQuery = " Select  'NIL' Type , 'Portal Summary' Description,  ifnull(Fld_InvoiceNo,0) TotalInvoice, " +
                 " ifnull(sum(replace(Fld_TaxValue,',','')),0.00) TaxableValue, " +
                 " ifnull(sum(replace(Fld_IGST,',','')),0.00) IGST, " +
                 " ifnull(sum(replace(Fld_CGST,',','')),0.00)  CGST, " +
                 " ifnull(sum(replace(Fld_SGST,',','')),0.00) SGST, " +
                 " ifnull( sum(replace(Fld_CESS,',','')),0.00)  Cess, " +
                 " (ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                 " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00)) TotalGST, " +
                 " (ifnull(sum(replace(Fld_TaxValue,',','')),0.00) + ( ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                 " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00))) as InvoiceValue " +
                 " from SPQGSTNSummary where Fld_SectionName='NIL' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' " +
                " union all " +
                " Select  'NIL' Type , 'Software Summary' Description, " +
                  "   (Select case when (ifnull(sum(replace(Fld_NilRatedSupply,',','')),0.00) +  ifnull(sum(replace(Fld_Exempted,',','')),0.00) + ifnull(sum(replace(Fld_NonGSTSupplies,',','')),0.00)) > 0 then 1 else 0 end as TotalInvoice " +
                     " from SPQR1NilRated where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "') ,0 ,0,0,0,0,0,0 ";
            strQuery += " union all select 'NIL' ,'Difference',0 ,0 ,0,0,0,0,0,0 ";
          
           
            dt = MC.GetValueindatatable(strQuery);
            foreach (DataGridViewColumn col in grdNilExemptNonGst.Columns)
            {
                col.DataPropertyName = dt.Columns[col.Index].ColumnName;
                if (col.Index > 1)
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = string.Format("N2");
                    col.DefaultCellStyle.NullValue = "-";
                }

            }
            grdNilExemptNonGst.DataSource = dt;
            #endregion

            //  Advance Received
            #region Advance Received

            strQuery = " Select  'AR' Type , 'Portal Summary' Description,  ifnull(Fld_InvoiceNo,0) TotalInvoice, " +
                   " ifnull(sum(replace(Fld_TaxValue,',','')),0.00) TaxableValue, " +
                   " ifnull(sum(replace(Fld_IGST,',','')),0.00) IGST, " +
                   " ifnull(sum(replace(Fld_CGST,',','')),0.00)  CGST, " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) SGST, " +
                   " ifnull( sum(replace(Fld_CESS,',','')),0.00)  Cess, " +
                   " (ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00)) TotalGST, " +
                   " (ifnull(sum(replace(Fld_TaxValue,',','')),0.00) + ( ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00))) as InvoiceValue " +
                   " from SPQGSTNSummary where Fld_SectionName='AR' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' " +
                  " union all " +
                  " select 'AR' Type ,'Software Summary' Description, count(*) TotalInvoice, " +
                      "  ifnull(sum(replace(Fld_GrossAdvRcv,',','')),0.00) TaxableValue, " +
                      "  ifnull(sum(replace(Fld_IGSTAmnt,',','')),0.00) IGST, " +
                      "  ifnull(sum(replace(Fld_CGSTAmnt,',','')),0.00)  CGST, " +
                      "  ifnull(sum(replace(Fld_SGSTAmnt,',','')),0.00) SGST, " +
                      "  ifnull( sum(replace(Fld_CessAmount,',','')),0.00)  Cess, " +
                      "  (ifnull(sum(replace(Fld_IGSTAmnt,',','')),0.00) + ifnull(sum(replace(Fld_CGSTAmnt,',','')),0.00) + " +
                      "  ifnull(sum(replace(Fld_SGSTAmnt,',','')),0.00) +  ifnull( sum(replace(Fld_CessAmount,',','')),0.00)) TotalGST, " +
                      "  0.00 as InvoiceValue " +
                   " from SPQR1GrossAdvance where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
            strQuery += " union all select 'AR' ,'Difference',0 ,0 ,0,0,0,0,0,0 ";

            dt = MC.GetValueindatatable(strQuery);
            foreach (DataGridViewColumn col in grdAdvcRcvd.Columns)
            {
                col.DataPropertyName = dt.Columns[col.Index].ColumnName;
                if (col.Index > 1)
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = string.Format("N2");
                    col.DefaultCellStyle.NullValue = "-";
                }

            }
            grdAdvcRcvd.DataSource = dt;

            #endregion

            //  Advance Adjusted
            #region Advance Adjusted

            strQuery = " Select  'AA' Type , 'Portal Summary' Description,  ifnull(Fld_InvoiceNo,0) TotalInvoice, " +
                   " ifnull(sum(replace(Fld_TaxValue,',','')),0.00) TaxableValue, " +
                   " ifnull(sum(replace(Fld_IGST,',','')),0.00) IGST, " +
                   " ifnull(sum(replace(Fld_CGST,',','')),0.00)  CGST, " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) SGST, " +
                   " ifnull( sum(replace(Fld_CESS,',','')),0.00)  Cess, " +
                   " (ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00)) TotalGST, " +
                   " (ifnull(sum(replace(Fld_TaxValue,',','')),0.00) + ( ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00))) as InvoiceValue " +
                   " from SPQGSTNSummary where Fld_SectionName='AA' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' " +
                  " union all " +
                  " select 'AA' Type ,'Software Summary' Description, count(*) TotalInvoice, " +
                      "  ifnull(sum(replace(Fld_Advadj,',','')),0.00) TaxableValue, " +
                      "  ifnull(sum(replace(Fld_IGSTAmnt,',','')),0.00) IGST, " +
                      "  ifnull(sum(replace(Fld_CGSTAmnt,',','')),0.00)  CGST, " +
                      "  ifnull(sum(replace(Fld_SGSTAmnt,',','')),0.00) SGST, " +
                      "  ifnull( sum(replace(Fld_CessAmount,',','')),0.00)  Cess, " +
                      "  (ifnull(sum(replace(Fld_IGSTAmnt,',','')),0.00) + ifnull(sum(replace(Fld_CGSTAmnt,',','')),0.00) + " +
                      "  ifnull(sum(replace(Fld_SGSTAmnt,',','')),0.00) +  ifnull( sum(replace(Fld_CessAmount,',','')),0.00)) TotalGST, " +
                      "  0.00 as InvoiceValue " +
                   " from SPQR1NetAdvance where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
            strQuery += " union all select 'AA' ,'Difference',0 ,0 ,0,0,0,0,0,0 ";
          
            dt = MC.GetValueindatatable(strQuery);

            foreach (DataGridViewColumn col in grdAdvAdjst.Columns)
            {
                col.DataPropertyName = dt.Columns[col.Index].ColumnName;
                if (col.Index > 1)
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = string.Format("N2");
                    col.DefaultCellStyle.NullValue = "-";
                }

            }
            grdAdvAdjst.DataSource = dt;

            #endregion
        }

        private void BindSummaryAmendedData()
        {


            // B2B Amend Sumamry
            #region B2B Amend Summary
            strQuery = " Select  'B2BA' Type , 'Portal Summary' Description,  ifnull(Fld_InvoiceNo,0) TotalInvoice, " +
                       " ifnull(sum(replace(Fld_TaxValue,',','')),0.00) TaxableValue, " +
                       " ifnull(sum(replace(Fld_IGST,',','')),0.00) IGST, " +
                       " ifnull(sum(replace(Fld_CGST,',','')),0.00)  CGST, " +
                       " ifnull(sum(replace(Fld_SGST,',','')),0.00) SGST, " +
                       " ifnull( sum(replace(Fld_CESS,',','')),0.00)  Cess, " +
                       " (ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                       " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00)) TotalGST, " +
                       " (ifnull(sum(replace(Fld_TaxValue,',','')),0.00) + ( ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                       " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00))) as InvoiceValue " +
                       " from SPQGSTNSummary where Fld_SectionName='B2BA' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' " +
                      " union all " +
                      " select 'B2BA' Type ,'Software Summary' Description, count(*) TotalInvoice, " +
                          "  ifnull(sum(replace(Fld_TaxVal,',','')),0.00) TaxableValue, " +
                          "  ifnull(sum(replace(Fld_IGSTAmt,',','')),0.00) IGST, " +
                          "  ifnull(sum(replace(Fld_CGSTAmt,',','')),0.00)  CGST, " +
                          "  ifnull(sum(replace(Fld_SGSTAmt,',','')),0.00) SGST, " +
                          "  ifnull( sum(replace(Fld_CESSAmt,',','')),0.00)  Cess, " +
                          "  (ifnull(sum(replace(Fld_IGSTAmt,',','')),0.00) + ifnull(sum(replace(Fld_CGSTAmt,',','')),0.00) + " +
                          "  ifnull(sum(replace(Fld_SGSTAmt,',','')),0.00) +  ifnull( sum(replace(Fld_CESSAmt,',','')),0.00)) TotalGST, " +
                        " ifnull(sum(replace(Fld_InvoiceVal,',','')),0.00) InvoiceValue " +
                       " from SPQR1AmendB2B where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
            strQuery += " union all select 'B2BA' ,'Difference',0 ,0 ,0,0,0,0,0,0 ";
          
            dt = MC.GetValueindatatable(strQuery);
            foreach (DataGridViewColumn col in grdB2bAmend.Columns)
            {
                col.DataPropertyName = dt.Columns[col.Index].ColumnName;
                if (col.Index > 1)
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = string.Format("N2");
                    col.DefaultCellStyle.NullValue = "-";
                }

            }
            grdB2bAmend.DataSource = dt;
            #endregion

            // B2CL Amend Sumamry
            #region B2CL Amend Sumamry
            strQuery = " Select 'B2CLA' Type ,  'Portal Summary' Description,  ifnull(Fld_InvoiceNo,0) TotalInvoice, " +
                    " ifnull(sum(replace(Fld_TaxValue,',','')),0.00) TaxableValue, " +
                    " ifnull(sum(replace(Fld_IGST,',','')),0.00) IGST, " +
                    " ifnull(sum(replace(Fld_CGST,',','')),0.00)  CGST, " +
                    " ifnull(sum(replace(Fld_SGST,',','')),0.00) SGST, " +
                    " ifnull( sum(replace(Fld_CESS,',','')),0.00)  Cess, " +
                    " (ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                    " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00)) TotalGST, " +
                    " (ifnull(sum(replace(Fld_TaxValue,',','')),0.00) + ( ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                    " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00))) as InvoiceValue " +
                    " from SPQGSTNSummary where Fld_SectionName='B2CLA' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' " +
                   " union all " +
                   " select 'B2CLA' Type , 'Software Summary' Description, count(*) TotalInvoice, " +
                       "  ifnull(sum(replace(Fld_TaxVal,',','')),0.00) TaxableValue, " +
                       "  ifnull(sum(replace(Fld_IGSTAmt,',','')),0.00) IGSTt, " +
                       "  0.00  CGST, " +
                       "  0.00 SGST, " +
                       "  ifnull( sum(replace(Fld_CESSAmt,',','')),0.00)  Cess, " +
                       "  (ifnull(sum(replace(Fld_IGSTAmt,',','')),0.00) +  ifnull( sum(replace(Fld_CESSAmt,',','')),0.00)) TotalGST, " +
                     " ifnull(sum(replace(Fld_SupInvoiceVal,',','')),0.00) InvoiceValue " +
                    " from SPQR1AmendB2CL where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
            strQuery += " union all select 'B2CLA' ,'Difference',0 ,0 ,0,0,0,0,0,0 ";
          
            dt = MC.GetValueindatatable(strQuery);
            foreach (DataGridViewColumn col in grdB2CLAmend.Columns)
            {
                col.DataPropertyName = dt.Columns[col.Index].ColumnName;
                if (col.Index > 1)
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = string.Format("N2");
                    col.DefaultCellStyle.NullValue = "-";
                }

            }
            grdB2CLAmend.DataSource = dt;

            #endregion
         

            // B2CS Amend Sumamry
            #region B2CS Amend  Sumamry
            strQuery = " Select 'B2CSA' Type ,  'Portal Summary' Description,  ifnull(Fld_InvoiceNo,0) TotalInvoice, " +
                    " ifnull(sum(replace(Fld_TaxValue,',','')),0.00) TaxableValue, " +
                    " ifnull(sum(replace(Fld_IGST,',','')),0.00) IGST, " +
                    " ifnull(sum(replace(Fld_CGST,',','')),0.00)  CGST, " +
                    " ifnull(sum(replace(Fld_SGST,',','')),0.00) SGST, " +
                    " ifnull( sum(replace(Fld_CESS,',','')),0.00)  Cess, " +
                    " (ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                    " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00)) TotalGST, " +
                    " (ifnull(sum(replace(Fld_TaxValue,',','')),0.00) + ( ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                    " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00))) as InvoiceValue " +
                    " from SPQGSTNSummary where Fld_SectionName='B2CSA' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' " +
                   " union all " +
                   " select 'B2CSA' Type , 'Software Summary' Description, count(*) TotalInvoice, " +
                       "  ifnull(sum(replace(Fld_TaxVal,',','')),0.00) TaxableValue, " +
                       "  ifnull(sum(replace(Fld_IGSTAmt,',','')),0.00) IGST, " +
                       " ifnull(sum(replace(Fld_CGSTAmt,',','')),0.00) CGST, " +
                       "  ifnull(sum(replace(Fld_SGSTAmt,',','')),0.00) SGST, " +
                       "  ifnull( sum(replace(Fld_CESSAmt,',','')),0.00)  Cess, " +
                       "  (ifnull(sum(replace(Fld_IGSTAmt,',','')),0.00) + ifnull(sum(replace(Fld_CGSTAmt,',','')),0.00) + " +
                       " ifnull(sum(replace(Fld_SGSTAmt,',','')),0.00) + ifnull( sum(replace(Fld_CESSAmt,',','')),0.00)) TotalGST, " +
                      " ( ifnull(sum(replace(Fld_TaxVal,',','')),0.00)+ ifnull(sum(replace(Fld_IGSTAmt,',','')),0.00) + ifnull(sum(replace(Fld_CGSTAmt,',','')),0.00) + " +
                       " ifnull(sum(replace(Fld_SGSTAmt,',','')),0.00) + ifnull( sum(replace(Fld_CESSAmt,',','')),0.00)) InvoiceValue " +
                    " from SPQR1AmendB2CS where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
            strQuery += " union all select 'B2CSA' ,'Difference',0 ,0 ,0,0,0,0,0,0 ";
           
            dt = MC.GetValueindatatable(strQuery);
            foreach (DataGridViewColumn col in grdB2CSAmend.Columns)
            {
                col.DataPropertyName = dt.Columns[col.Index].ColumnName;
                if (col.Index > 1)
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = string.Format("N2");
                    col.DefaultCellStyle.NullValue = "-";
                }

            }
            grdB2CSAmend.DataSource = dt;

            #endregion

         
            // Export Amend Invoices
            #region Export Amend Invoices (SPQR1ZeroRated)

            strQuery = " Select 'EXPA' Type ,  'Portal Summary' Description,  ifnull(Fld_InvoiceNo,0) TotalInvoice, " +
                  " ifnull(sum(replace(Fld_TaxValue,',','')),0.00) TaxableValue, " +
                  " ifnull(sum(replace(Fld_IGST,',','')),0.00) IGST, " +
                  " ifnull(sum(replace(Fld_CGST,',','')),0.00)  CGST, " +
                  " ifnull(sum(replace(Fld_SGST,',','')),0.00) SGST, " +
                  " ifnull( sum(replace(Fld_CESS,',','')),0.00)  Cess, " +
                  " (ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                  " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00)) TotalGST, " +
                  " (ifnull(sum(replace(Fld_TaxValue,',','')),0.00) + ( ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                  " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00))) as InvoiceValue " +
                  " from SPQGSTNSummary where Fld_SectionName='EXPA' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' " +
                 " union all " +
                 " select 'EXPA' Type , 'Software Summary' Description, count(*) TotalInvoice, " +
                     "  ifnull(sum(replace(Fld_TaxableValue,',','')),0.00) TaxableValue, " +
                     "  ifnull(sum(replace(Fld_IGSTAmt,',','')),0.00) IGST, " +
                     "  0.00  CGST, " +
                     "  0.00 SGST, " +
                     "  ifnull( sum(replace(Fld_CESSAmt,',','')),0.00)  Cess, " +
                     "  (ifnull(sum(replace(Fld_IGSTAmt,',','')),0.00) +  ifnull( sum(replace(Fld_CESSAmt,',','')),0.00)) TotalGST, " +
                   " ifnull(sum(replace(Fld_SupInvoiceVal,',','')),0.00) InvoiceValue " +
                  " from SPQR1AmendEXPORT where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
            strQuery += " union all select 'EXPA' ,'Difference',0 ,0 ,0,0,0,0,0,0 ";



            dt = MC.GetValueindatatable(strQuery);
            foreach (DataGridViewColumn col in grdExportAmend.Columns)
            {
                col.DataPropertyName = dt.Columns[col.Index].ColumnName;
                if (col.Index > 1)
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = string.Format("N2");
                    col.DefaultCellStyle.NullValue = "-";
                }

            }
            grdExportAmend.DataSource = dt;

            #endregion


            // Cr./ Dr. Note Regd Amend 
            #region Cr.Dr. Note Regd. Amend

            strQuery = " Select  'CDNRA' Type , 'Portal Summary' Description,  ifnull(Fld_InvoiceNo,0) TotalInvoice, " +
                   " ifnull(sum(replace(Fld_TaxValue,',','')),0.00) TaxableValue, " +
                   " ifnull(sum(replace(Fld_IGST,',','')),0.00) IGST, " +
                   " ifnull(sum(replace(Fld_CGST,',','')),0.00)  CGST, " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) SGST, " +
                   " ifnull( sum(replace(Fld_CESS,',','')),0.00)  Cess, " +
                   " (ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00)) TotalGST, " +
                   " (ifnull(sum(replace(Fld_TaxValue,',','')),0.00) + ( ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00))) as InvoiceValue " +
                   " from SPQGSTNSummary where Fld_SectionName='CDNRA' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' " +
                  " union all " +
                  " select 'CDNRA' Type ,'Software Summary' Description, count(*) TotalInvoice, " +
                      "  ifnull(sum(replace(Fld_TaxableValue,',','')),0.00) TaxableValue, " +
                      "  ifnull(sum(replace(Fld_IGSTAmt,',','')),0.00) IGST, " +
                      "  ifnull(sum(replace(Fld_CGSTAmt,',','')),0.00)  CGST, " +
                      "  ifnull(sum(replace(Fld_SGSTAmt,',','')),0.00) SGST, " +
                      "  ifnull( sum(replace(Fld_CESSAmt,',','')),0.00)  Cess, " +
                      "  (ifnull(sum(replace(Fld_IGSTAmt,',','')),0.00) + ifnull(sum(replace(Fld_CGSTAmt,',','')),0.00) + " +
                      "  ifnull(sum(replace(Fld_SGSTAmt,',','')),0.00) +  ifnull( sum(replace(Fld_CESSAmt,',','')),0.00)) TotalGST, " +
                    " ifnull(sum(replace(Fld_TotalNoteValue,',','')),0.00) InvoiceValue " +
                   " from SPQR1AmendCDNR where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
            strQuery += " union all select 'CDNRA' ,'Difference',0 ,0 ,0,0,0,0,0,0 ";

            dt = MC.GetValueindatatable(strQuery);
            foreach (DataGridViewColumn col in grdCDNRAmend.Columns)
            {
                col.DataPropertyName = dt.Columns[col.Index].ColumnName;
                if (col.Index > 1)
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = string.Format("N2");
                    col.DefaultCellStyle.NullValue = "-";
                }

            }
            grdCDNRAmend.DataSource = dt;

            #endregion

         
            // Cr./ Dr. Note Un-Regd Amend 
            #region Cr.Dr. Note Un-Regd.

            strQuery = " Select  'CDNURA' Type , 'Portal Summary' Description,  ifnull(Fld_InvoiceNo,0) TotalInvoice, " +
                   " ifnull(sum(replace(Fld_TaxValue,',','')),0.00) TaxableValue, " +
                   " ifnull(sum(replace(Fld_IGST,',','')),0.00) IGST, " +
                   " ifnull(sum(replace(Fld_CGST,',','')),0.00)  CGST, " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) SGST, " +
                   " ifnull( sum(replace(Fld_CESS,',','')),0.00)  Cess, " +
                   " (ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00)) TotalGST, " +
                   " (ifnull(sum(replace(Fld_TaxValue,',','')),0.00) + ( ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00))) as InvoiceValue " +
                   " from SPQGSTNSummary where Fld_SectionName='CDNURA' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' " +
                  " union all " +
                  " select 'CDNURA' Type ,'Software Summary' Description, count(*) TotalInvoice, " +
                      "  ifnull(sum(replace(Fld_TaxableValue,',','')),0.00) TaxableValue, " +
                      "  ifnull(sum(replace(Fld_IGSTAmt,',','')),0.00) IGST, " +
                      "  ifnull(sum(replace(Fld_CGSTAmt,',','')),0.00)  CGST, " +
                      "  ifnull(sum(replace(Fld_SGSTAmt,',','')),0.00) SGST, " +
                      "  ifnull( sum(replace(Fld_CESSAmt,',','')),0.00)  Cess, " +
                      "  (ifnull(sum(replace(Fld_IGSTAmt,',','')),0.00) + ifnull(sum(replace(Fld_CGSTAmt,',','')),0.00) + " +
                      "  ifnull(sum(replace(Fld_SGSTAmt,',','')),0.00) +  ifnull( sum(replace(Fld_CESSAmt,',','')),0.00)) TotalGST, " +
                    " ifnull(sum(replace(Fld_TotalNoteValue,',','')),0.00) InvoiceValue " +
                   " from SPQR1AmendCDNUR where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
            strQuery += " union all select 'CDNURA' ,'Difference',0 ,0 ,0,0,0,0,0,0 ";

          
            dt = MC.GetValueindatatable(strQuery);
            foreach (DataGridViewColumn col in grdCDNURAmend.Columns)
            {
                col.DataPropertyName = dt.Columns[col.Index].ColumnName;
                if (col.Index > 1)
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = string.Format("N2");
                    col.DefaultCellStyle.NullValue = "-";
                }

            }
            grdCDNURAmend.DataSource = dt;

            #endregion
           

            // Nil Rated Amend  NOT NEEDED
            #region Nil Rated Amend

            //strQuery = " Select  'NIL' Type , 'Portal Summary' Description,  ifnull(Fld_InvoiceNo,0) TotalInvoice, " +
            //     " ifnull(sum(replace(Fld_TaxValue,',','')),0.00) TaxableValue, " +
            //     " ifnull(sum(replace(Fld_IGST,',','')),0.00) IGST, " +
            //     " ifnull(sum(replace(Fld_CGST,',','')),0.00)  CGST, " +
            //     " ifnull(sum(replace(Fld_SGST,',','')),0.00) SGST, " +
            //     " ifnull( sum(replace(Fld_CESS,',','')),0.00)  Cess, " +
            //     " (ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
            //     " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00)) TotalGST, " +
            //     " (ifnull(sum(replace(Fld_TaxValue,',','')),0.00) + ( ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
            //     " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00))) as InvoiceValue " +
            //     " from SPQGSTNSummary where Fld_SectionName='NIL' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' " +
            //    " union all " +
            //    " Select  'NIL' Type , 'Software Summary' Description, " +
            //      "   (Select case when (ifnull(sum(replace(Fld_NilRatedSupply,',','')),0.00) +  ifnull(sum(replace(Fld_Exempted,',','')),0.00) + ifnull(sum(replace(Fld_NonGSTSupplies,',','')),0.00)) > 0 then 1 else 0 end as TotalInvoice " +
            //         " from SPQR1NilRated where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "') ,0 ,0,0,0,0,0,0 ";
            //strQuery += " union all select 'NIL' ,'Difference',0 ,0 ,0,0,0,0,0,0 ";
         
            //dt = MC.GetValueindatatable(strQuery);
            //dt = MC.GetValueindatatable(strQuery);
            //foreach (DataGridViewColumn col in grdNilExemptNonGst.Columns)
            //{
            //    col.DataPropertyName = dt.Columns[col.Index].ColumnName;
            //    if (col.Index > 1)
            //    {
            //        col.DefaultCellStyle.Format = string.Format("N2");
            //        col.DefaultCellStyle.NullValue = "-";
            //    }

            //}
            //grdNilExemptNonGst.DataSource = dt;

            #endregion
           

            //  Advance Received Amend
            #region Advance Received

            strQuery = " Select  'ATA' Type , 'Portal Summary' Description,  ifnull(Fld_InvoiceNo,0) TotalInvoice, " +
                   " ifnull(sum(replace(Fld_TaxValue,',','')),0.00) TaxableValue, " +
                   " ifnull(sum(replace(Fld_IGST,',','')),0.00) IGST, " +
                   " ifnull(sum(replace(Fld_CGST,',','')),0.00)  CGST, " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) SGST, " +
                   " ifnull( sum(replace(Fld_CESS,',','')),0.00)  Cess, " +
                   " (ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00)) TotalGST, " +
                   " (ifnull(sum(replace(Fld_TaxValue,',','')),0.00) + ( ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00))) as InvoiceValue " +
                   " from SPQGSTNSummary where Fld_SectionName='ATA' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' " +
                  " union all " +
                  " select 'ATA' Type ,'Software Summary' Description, count(*) TotalInvoice, " +
                      "  ifnull(sum(replace(Fld_AdvReceived,',','')),0.00) TaxableValue, " +
                      "  ifnull(sum(replace(Fld_IGSTRate,',','')),0.00) IGST, " +
                      "  ifnull(sum(replace(Fld_CGSTRate,',','')),0.00)  CGST, " +
                      "  ifnull(sum(replace(Fld_SGSTRate,',','')),0.00) SGST, " +
                      "  ifnull( sum(replace(Fld_CESSRate,',','')),0.00)  Cess, " +
                      "  (ifnull(sum(replace(Fld_IGSTRate,',','')),0.00) + ifnull(sum(replace(Fld_CGSTRate,',','')),0.00) + " +
                      "  ifnull(sum(replace(Fld_SGSTRate,',','')),0.00) +  ifnull( sum(replace(Fld_CESSRate,',','')),0.00)) TotalGST, " +
                      "  0.00 as InvoiceValue " +
                   " from SPQR1AmendAT where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
            strQuery += " union all select 'ATA' ,'Difference',0 ,0 ,0,0,0,0,0,0 ";
           
            dt = MC.GetValueindatatable(strQuery);
            foreach (DataGridViewColumn col in grdAdvRcvdAmend.Columns)
            {
                col.DataPropertyName = dt.Columns[col.Index].ColumnName;
                if (col.Index > 1)
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = string.Format("N2");
                    col.DefaultCellStyle.NullValue = "-";
                }

            }
            grdAdvRcvdAmend.DataSource = dt;

            #endregion

          
            //  Advance Adjusted Amend
            #region Advance Adjusted

            strQuery = " Select  'AA' Type , 'Portal Summary' Description,  ifnull(Fld_InvoiceNo,0) TotalInvoice, " +
                   " ifnull(sum(replace(Fld_TaxValue,',','')),0.00) TaxableValue, " +
                   " ifnull(sum(replace(Fld_IGST,',','')),0.00) IGST, " +
                   " ifnull(sum(replace(Fld_CGST,',','')),0.00)  CGST, " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) SGST, " +
                   " ifnull( sum(replace(Fld_CESS,',','')),0.00)  Cess, " +
                   " (ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00)) TotalGST, " +
                   " (ifnull(sum(replace(Fld_TaxValue,',','')),0.00) + ( ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00))) as InvoiceValue " +
                   " from SPQGSTNSummary where Fld_SectionName='AA' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' " +
                  " union all " +
                  " select 'AA' Type ,'Software Summary' Description, count(*) TotalInvoice, " +
                      "  ifnull(sum(replace(Fld_AdvToAdjusted,',','')),0.00) TaxableValue, " +
                      "  ifnull(sum(replace(Fld_IGSTAmt,',','')),0.00) IGST, " +
                      "  ifnull(sum(replace(Fld_CGSTAmt,',','')),0.00)  CGST, " +
                      "  ifnull(sum(replace(Fld_SGSTAmt,',','')),0.00) SGST, " +
                      "  ifnull( sum(replace(Fld_CessAmt,',','')),0.00)  Cess, " +
                      "  (ifnull(sum(replace(Fld_IGSTAmt,',','')),0.00) + ifnull(sum(replace(Fld_CGSTAmt,',','')),0.00) + " +
                      "  ifnull(sum(replace(Fld_SGSTAmt,',','')),0.00) +  ifnull( sum(replace(Fld_CessAmt,',','')),0.00)) TotalGST, " +
                      "  0.00 as InvoiceValue " +
                   " from SPQR1AmendTXP where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
            strQuery += " union all select 'AA' ,'Difference',0 ,0 ,0,0,0,0,0,0 ";


        


            dt = MC.GetValueindatatable(strQuery);

            foreach (DataGridViewColumn col in grdAdvAdjstAmend.Columns)
            {
                col.DataPropertyName = dt.Columns[col.Index].ColumnName;
                if (col.Index > 1)
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = string.Format("N2");
                    col.DefaultCellStyle.NullValue = "-";
                }

            }
            grdAdvAdjstAmend.DataSource = dt;

            #endregion

        }

        private void BindDifference()
        {

            #region Original 
            
            #region B2B Difference

            grdB2b.Rows[2].Cells[2].Value = Convert.ToDecimal(grdB2b.Rows[0].Cells[2].Value) - Convert.ToDecimal(grdB2b.Rows[1].Cells[2].Value);
            grdB2b.Rows[2].Cells[3].Value = Convert.ToDecimal(grdB2b.Rows[0].Cells[3].Value) - Convert.ToDecimal(grdB2b.Rows[1].Cells[3].Value);
            grdB2b.Rows[2].Cells[4].Value = Convert.ToDecimal(grdB2b.Rows[0].Cells[4].Value) - Convert.ToDecimal(grdB2b.Rows[1].Cells[4].Value);
            grdB2b.Rows[2].Cells[5].Value = Convert.ToDecimal(grdB2b.Rows[0].Cells[5].Value) - Convert.ToDecimal(grdB2b.Rows[1].Cells[5].Value);
            grdB2b.Rows[2].Cells[6].Value = Convert.ToDecimal(grdB2b.Rows[0].Cells[6].Value) - Convert.ToDecimal(grdB2b.Rows[1].Cells[6].Value);
            grdB2b.Rows[2].Cells[7].Value = Convert.ToDecimal(grdB2b.Rows[0].Cells[7].Value) - Convert.ToDecimal(grdB2b.Rows[1].Cells[7].Value);
            grdB2b.Rows[2].Cells[8].Value = Convert.ToDecimal(grdB2b.Rows[0].Cells[8].Value) - Convert.ToDecimal(grdB2b.Rows[1].Cells[8].Value);
            grdB2b.Rows[2].Cells[9].Value = Convert.ToDecimal(grdB2b.Rows[0].Cells[9].Value) - Convert.ToDecimal(grdB2b.Rows[1].Cells[9].Value);

            #endregion

            #region B2CL Difference

            grdB2cl.Rows[2].Cells[2].Value = Convert.ToDecimal(grdB2cl.Rows[0].Cells[2].Value) - Convert.ToDecimal(grdB2cl.Rows[1].Cells[2].Value);
            grdB2cl.Rows[2].Cells[3].Value = Convert.ToDecimal(grdB2cl.Rows[0].Cells[3].Value) - Convert.ToDecimal(grdB2cl.Rows[1].Cells[3].Value);
            grdB2cl.Rows[2].Cells[4].Value = Convert.ToDecimal(grdB2cl.Rows[0].Cells[4].Value) - Convert.ToDecimal(grdB2cl.Rows[1].Cells[4].Value);
            grdB2cl.Rows[2].Cells[5].Value = Convert.ToDecimal(grdB2cl.Rows[0].Cells[5].Value) - Convert.ToDecimal(grdB2cl.Rows[1].Cells[5].Value);
            grdB2cl.Rows[2].Cells[6].Value = Convert.ToDecimal(grdB2cl.Rows[0].Cells[6].Value) - Convert.ToDecimal(grdB2cl.Rows[1].Cells[6].Value);
            grdB2cl.Rows[2].Cells[7].Value = Convert.ToDecimal(grdB2cl.Rows[0].Cells[7].Value) - Convert.ToDecimal(grdB2cl.Rows[1].Cells[7].Value);
            grdB2cl.Rows[2].Cells[8].Value = Convert.ToDecimal(grdB2cl.Rows[0].Cells[8].Value) - Convert.ToDecimal(grdB2cl.Rows[1].Cells[8].Value);
            grdB2cl.Rows[2].Cells[9].Value = Convert.ToDecimal(grdB2cl.Rows[0].Cells[9].Value) - Convert.ToDecimal(grdB2cl.Rows[1].Cells[9].Value);

            #endregion

            #region B2CS Difference

            grdB2cs.Rows[2].Cells[2].Value = Convert.ToDecimal(grdB2cs.Rows[0].Cells[2].Value) - Convert.ToDecimal(grdB2cs.Rows[1].Cells[2].Value);
            grdB2cs.Rows[2].Cells[3].Value = Convert.ToDecimal(grdB2cs.Rows[0].Cells[3].Value) - Convert.ToDecimal(grdB2cs.Rows[1].Cells[3].Value);
            grdB2cs.Rows[2].Cells[4].Value = Convert.ToDecimal(grdB2cs.Rows[0].Cells[4].Value) - Convert.ToDecimal(grdB2cs.Rows[1].Cells[4].Value);
            grdB2cs.Rows[2].Cells[5].Value = Convert.ToDecimal(grdB2cs.Rows[0].Cells[5].Value) - Convert.ToDecimal(grdB2cs.Rows[1].Cells[5].Value);
            grdB2cs.Rows[2].Cells[6].Value = Convert.ToDecimal(grdB2cs.Rows[0].Cells[6].Value) - Convert.ToDecimal(grdB2cs.Rows[1].Cells[6].Value);
            grdB2cs.Rows[2].Cells[7].Value = Convert.ToDecimal(grdB2cs.Rows[0].Cells[7].Value) - Convert.ToDecimal(grdB2cs.Rows[1].Cells[7].Value);
            grdB2cs.Rows[2].Cells[8].Value = Convert.ToDecimal(grdB2cs.Rows[0].Cells[8].Value) - Convert.ToDecimal(grdB2cs.Rows[1].Cells[8].Value);
            grdB2cs.Rows[2].Cells[9].Value = Convert.ToDecimal(grdB2cs.Rows[0].Cells[9].Value) - Convert.ToDecimal(grdB2cs.Rows[1].Cells[9].Value);

            #endregion

            #region Export Difference

            grdExport.Rows[2].Cells[2].Value = Convert.ToDecimal(grdExport.Rows[0].Cells[2].Value) - Convert.ToDecimal(grdExport.Rows[1].Cells[2].Value);
            grdExport.Rows[2].Cells[3].Value = Convert.ToDecimal(grdExport.Rows[0].Cells[3].Value) - Convert.ToDecimal(grdExport.Rows[1].Cells[3].Value);
            grdExport.Rows[2].Cells[4].Value = Convert.ToDecimal(grdExport.Rows[0].Cells[4].Value) - Convert.ToDecimal(grdExport.Rows[1].Cells[4].Value);
            grdExport.Rows[2].Cells[5].Value = Convert.ToDecimal(grdExport.Rows[0].Cells[5].Value) - Convert.ToDecimal(grdExport.Rows[1].Cells[5].Value);
            grdExport.Rows[2].Cells[6].Value = Convert.ToDecimal(grdExport.Rows[0].Cells[6].Value) - Convert.ToDecimal(grdExport.Rows[1].Cells[6].Value);
            grdExport.Rows[2].Cells[7].Value = Convert.ToDecimal(grdExport.Rows[0].Cells[7].Value) - Convert.ToDecimal(grdExport.Rows[1].Cells[7].Value);
            grdExport.Rows[2].Cells[8].Value = Convert.ToDecimal(grdExport.Rows[0].Cells[8].Value) - Convert.ToDecimal(grdExport.Rows[1].Cells[8].Value);
            grdExport.Rows[2].Cells[9].Value = Convert.ToDecimal(grdExport.Rows[0].Cells[9].Value) - Convert.ToDecimal(grdExport.Rows[1].Cells[9].Value);
 
	        #endregion

            #region CDNR Difference

            grdCrDrNote.Rows[2].Cells[2].Value = Convert.ToDecimal(grdCrDrNote.Rows[0].Cells[2].Value) - Convert.ToDecimal(grdCrDrNote.Rows[1].Cells[2].Value);
            grdCrDrNote.Rows[2].Cells[3].Value = Convert.ToDecimal(grdCrDrNote.Rows[0].Cells[3].Value) - Convert.ToDecimal(grdCrDrNote.Rows[1].Cells[3].Value);
            grdCrDrNote.Rows[2].Cells[4].Value = Convert.ToDecimal(grdCrDrNote.Rows[0].Cells[4].Value) - Convert.ToDecimal(grdCrDrNote.Rows[1].Cells[4].Value);
            grdCrDrNote.Rows[2].Cells[5].Value = Convert.ToDecimal(grdCrDrNote.Rows[0].Cells[5].Value) - Convert.ToDecimal(grdCrDrNote.Rows[1].Cells[5].Value);
            grdCrDrNote.Rows[2].Cells[6].Value = Convert.ToDecimal(grdCrDrNote.Rows[0].Cells[6].Value) - Convert.ToDecimal(grdCrDrNote.Rows[1].Cells[6].Value);
            grdCrDrNote.Rows[2].Cells[7].Value = Convert.ToDecimal(grdCrDrNote.Rows[0].Cells[7].Value) - Convert.ToDecimal(grdCrDrNote.Rows[1].Cells[7].Value);
            grdCrDrNote.Rows[2].Cells[8].Value = Convert.ToDecimal(grdCrDrNote.Rows[0].Cells[8].Value) - Convert.ToDecimal(grdCrDrNote.Rows[1].Cells[8].Value);
            grdCrDrNote.Rows[2].Cells[9].Value = Convert.ToDecimal(grdCrDrNote.Rows[0].Cells[9].Value) - Convert.ToDecimal(grdCrDrNote.Rows[1].Cells[9].Value);

            #endregion

            #region CDNUR Difference

            grdCrDrNoteUnreg.Rows[2].Cells[2].Value = Convert.ToDecimal(grdCrDrNoteUnreg.Rows[0].Cells[2].Value) - Convert.ToDecimal(grdCrDrNoteUnreg.Rows[1].Cells[2].Value);
            grdCrDrNoteUnreg.Rows[2].Cells[3].Value = Convert.ToDecimal(grdCrDrNoteUnreg.Rows[0].Cells[3].Value) - Convert.ToDecimal(grdCrDrNoteUnreg.Rows[1].Cells[3].Value);
            grdCrDrNoteUnreg.Rows[2].Cells[4].Value = Convert.ToDecimal(grdCrDrNoteUnreg.Rows[0].Cells[4].Value) - Convert.ToDecimal(grdCrDrNoteUnreg.Rows[1].Cells[4].Value);
            grdCrDrNoteUnreg.Rows[2].Cells[5].Value = Convert.ToDecimal(grdCrDrNoteUnreg.Rows[0].Cells[5].Value) - Convert.ToDecimal(grdCrDrNoteUnreg.Rows[1].Cells[5].Value);
            grdCrDrNoteUnreg.Rows[2].Cells[6].Value = Convert.ToDecimal(grdCrDrNoteUnreg.Rows[0].Cells[6].Value) - Convert.ToDecimal(grdCrDrNoteUnreg.Rows[1].Cells[6].Value);
            grdCrDrNoteUnreg.Rows[2].Cells[7].Value = Convert.ToDecimal(grdCrDrNoteUnreg.Rows[0].Cells[7].Value) - Convert.ToDecimal(grdCrDrNoteUnreg.Rows[1].Cells[7].Value);
            grdCrDrNoteUnreg.Rows[2].Cells[8].Value = Convert.ToDecimal(grdCrDrNoteUnreg.Rows[0].Cells[8].Value) - Convert.ToDecimal(grdCrDrNoteUnreg.Rows[1].Cells[8].Value);
            grdCrDrNoteUnreg.Rows[2].Cells[9].Value = Convert.ToDecimal(grdCrDrNoteUnreg.Rows[0].Cells[9].Value) - Convert.ToDecimal(grdCrDrNoteUnreg.Rows[1].Cells[9].Value);

            #endregion

            #region Nil Rated Difference

            grdNilExemptNonGst.Rows[2].Cells[2].Value = Convert.ToDecimal(grdNilExemptNonGst.Rows[0].Cells[2].Value) - Convert.ToDecimal(grdNilExemptNonGst.Rows[1].Cells[2].Value);
            grdNilExemptNonGst.Rows[2].Cells[3].Value = Convert.ToDecimal(grdNilExemptNonGst.Rows[0].Cells[3].Value) - Convert.ToDecimal(grdNilExemptNonGst.Rows[1].Cells[3].Value);
            grdNilExemptNonGst.Rows[2].Cells[4].Value = Convert.ToDecimal(grdNilExemptNonGst.Rows[0].Cells[4].Value) - Convert.ToDecimal(grdNilExemptNonGst.Rows[1].Cells[4].Value);
            grdNilExemptNonGst.Rows[2].Cells[5].Value = Convert.ToDecimal(grdNilExemptNonGst.Rows[0].Cells[5].Value) - Convert.ToDecimal(grdNilExemptNonGst.Rows[1].Cells[5].Value);
            grdNilExemptNonGst.Rows[2].Cells[6].Value = Convert.ToDecimal(grdNilExemptNonGst.Rows[0].Cells[6].Value) - Convert.ToDecimal(grdNilExemptNonGst.Rows[1].Cells[6].Value);
            grdNilExemptNonGst.Rows[2].Cells[7].Value = Convert.ToDecimal(grdNilExemptNonGst.Rows[0].Cells[7].Value) - Convert.ToDecimal(grdNilExemptNonGst.Rows[1].Cells[7].Value);
            grdNilExemptNonGst.Rows[2].Cells[8].Value = Convert.ToDecimal(grdNilExemptNonGst.Rows[0].Cells[8].Value) - Convert.ToDecimal(grdNilExemptNonGst.Rows[1].Cells[8].Value);
            grdNilExemptNonGst.Rows[2].Cells[9].Value = Convert.ToDecimal(grdNilExemptNonGst.Rows[0].Cells[9].Value) - Convert.ToDecimal(grdNilExemptNonGst.Rows[1].Cells[9].Value);

            #endregion

            #region Advance Received Difference

            grdAdvcRcvd.Rows[2].Cells[2].Value = Convert.ToDecimal(grdAdvcRcvd.Rows[0].Cells[2].Value) - Convert.ToDecimal(grdAdvcRcvd.Rows[1].Cells[2].Value);
            grdAdvcRcvd.Rows[2].Cells[3].Value = Convert.ToDecimal(grdAdvcRcvd.Rows[0].Cells[3].Value) - Convert.ToDecimal(grdAdvcRcvd.Rows[1].Cells[3].Value);
            grdAdvcRcvd.Rows[2].Cells[4].Value = Convert.ToDecimal(grdAdvcRcvd.Rows[0].Cells[4].Value) - Convert.ToDecimal(grdAdvcRcvd.Rows[1].Cells[4].Value);
            grdAdvcRcvd.Rows[2].Cells[5].Value = Convert.ToDecimal(grdAdvcRcvd.Rows[0].Cells[5].Value) - Convert.ToDecimal(grdAdvcRcvd.Rows[1].Cells[5].Value);
            grdAdvcRcvd.Rows[2].Cells[6].Value = Convert.ToDecimal(grdAdvcRcvd.Rows[0].Cells[6].Value) - Convert.ToDecimal(grdAdvcRcvd.Rows[1].Cells[6].Value);
            grdAdvcRcvd.Rows[2].Cells[7].Value = Convert.ToDecimal(grdAdvcRcvd.Rows[0].Cells[7].Value) - Convert.ToDecimal(grdAdvcRcvd.Rows[1].Cells[7].Value);
            grdAdvcRcvd.Rows[2].Cells[8].Value = Convert.ToDecimal(grdAdvcRcvd.Rows[0].Cells[8].Value) - Convert.ToDecimal(grdAdvcRcvd.Rows[1].Cells[8].Value);
            grdAdvcRcvd.Rows[2].Cells[9].Value = Convert.ToDecimal(grdAdvcRcvd.Rows[0].Cells[9].Value) - Convert.ToDecimal(grdAdvcRcvd.Rows[1].Cells[9].Value);

            #endregion

            #region Advance Adjust Difference

            grdAdvAdjst.Rows[2].Cells[2].Value = Convert.ToDecimal(grdAdvAdjst.Rows[0].Cells[2].Value) - Convert.ToDecimal(grdAdvAdjst.Rows[1].Cells[2].Value);
            grdAdvAdjst.Rows[2].Cells[3].Value = Convert.ToDecimal(grdAdvAdjst.Rows[0].Cells[3].Value) - Convert.ToDecimal(grdAdvAdjst.Rows[1].Cells[3].Value);
            grdAdvAdjst.Rows[2].Cells[4].Value = Convert.ToDecimal(grdAdvAdjst.Rows[0].Cells[4].Value) - Convert.ToDecimal(grdAdvAdjst.Rows[1].Cells[4].Value);
            grdAdvAdjst.Rows[2].Cells[5].Value = Convert.ToDecimal(grdAdvAdjst.Rows[0].Cells[5].Value) - Convert.ToDecimal(grdAdvAdjst.Rows[1].Cells[5].Value);
            grdAdvAdjst.Rows[2].Cells[6].Value = Convert.ToDecimal(grdAdvAdjst.Rows[0].Cells[6].Value) - Convert.ToDecimal(grdAdvAdjst.Rows[1].Cells[6].Value);
            grdAdvAdjst.Rows[2].Cells[7].Value = Convert.ToDecimal(grdAdvAdjst.Rows[0].Cells[7].Value) - Convert.ToDecimal(grdAdvAdjst.Rows[1].Cells[7].Value);
            grdAdvAdjst.Rows[2].Cells[8].Value = Convert.ToDecimal(grdAdvAdjst.Rows[0].Cells[8].Value) - Convert.ToDecimal(grdAdvAdjst.Rows[1].Cells[8].Value);
            grdAdvAdjst.Rows[2].Cells[9].Value = Convert.ToDecimal(grdAdvAdjst.Rows[0].Cells[9].Value) - Convert.ToDecimal(grdAdvAdjst.Rows[1].Cells[9].Value);

            #endregion



            #endregion

            #region Amendement

            #region B2B Amendment Difference

            grdB2bAmend.Rows[2].Cells[2].Value = Convert.ToDecimal(grdB2bAmend.Rows[0].Cells[2].Value) - Convert.ToDecimal(grdB2bAmend.Rows[1].Cells[2].Value);
            grdB2bAmend.Rows[2].Cells[3].Value = Convert.ToDecimal(grdB2bAmend.Rows[0].Cells[3].Value) - Convert.ToDecimal(grdB2bAmend.Rows[1].Cells[3].Value);
            grdB2bAmend.Rows[2].Cells[4].Value = Convert.ToDecimal(grdB2bAmend.Rows[0].Cells[4].Value) - Convert.ToDecimal(grdB2bAmend.Rows[1].Cells[4].Value);
            grdB2bAmend.Rows[2].Cells[5].Value = Convert.ToDecimal(grdB2bAmend.Rows[0].Cells[5].Value) - Convert.ToDecimal(grdB2bAmend.Rows[1].Cells[5].Value);
            grdB2bAmend.Rows[2].Cells[6].Value = Convert.ToDecimal(grdB2bAmend.Rows[0].Cells[6].Value) - Convert.ToDecimal(grdB2bAmend.Rows[1].Cells[6].Value);
            grdB2bAmend.Rows[2].Cells[7].Value = Convert.ToDecimal(grdB2bAmend.Rows[0].Cells[7].Value) - Convert.ToDecimal(grdB2bAmend.Rows[1].Cells[7].Value);
            grdB2bAmend.Rows[2].Cells[8].Value = Convert.ToDecimal(grdB2bAmend.Rows[0].Cells[8].Value) - Convert.ToDecimal(grdB2bAmend.Rows[1].Cells[8].Value);
            grdB2bAmend.Rows[2].Cells[9].Value = Convert.ToDecimal(grdB2bAmend.Rows[0].Cells[9].Value) - Convert.ToDecimal(grdB2bAmend.Rows[1].Cells[9].Value);

            #endregion

            #region B2CL Amendment Difference

            grdB2CLAmend.Rows[2].Cells[2].Value = Convert.ToDecimal(grdB2CLAmend.Rows[0].Cells[2].Value) - Convert.ToDecimal(grdB2CLAmend.Rows[1].Cells[2].Value);
            grdB2CLAmend.Rows[2].Cells[3].Value = Convert.ToDecimal(grdB2CLAmend.Rows[0].Cells[3].Value) - Convert.ToDecimal(grdB2CLAmend.Rows[1].Cells[3].Value);
            grdB2CLAmend.Rows[2].Cells[4].Value = Convert.ToDecimal(grdB2CLAmend.Rows[0].Cells[4].Value) - Convert.ToDecimal(grdB2CLAmend.Rows[1].Cells[4].Value);
            grdB2CLAmend.Rows[2].Cells[5].Value = Convert.ToDecimal(grdB2CLAmend.Rows[0].Cells[5].Value) - Convert.ToDecimal(grdB2CLAmend.Rows[1].Cells[5].Value);
            grdB2CLAmend.Rows[2].Cells[6].Value = Convert.ToDecimal(grdB2CLAmend.Rows[0].Cells[6].Value) - Convert.ToDecimal(grdB2CLAmend.Rows[1].Cells[6].Value);
            grdB2CLAmend.Rows[2].Cells[7].Value = Convert.ToDecimal(grdB2CLAmend.Rows[0].Cells[7].Value) - Convert.ToDecimal(grdB2CLAmend.Rows[1].Cells[7].Value);
            grdB2CLAmend.Rows[2].Cells[8].Value = Convert.ToDecimal(grdB2CLAmend.Rows[0].Cells[8].Value) - Convert.ToDecimal(grdB2CLAmend.Rows[1].Cells[8].Value);
            grdB2CLAmend.Rows[2].Cells[9].Value = Convert.ToDecimal(grdB2CLAmend.Rows[0].Cells[9].Value) - Convert.ToDecimal(grdB2CLAmend.Rows[1].Cells[9].Value);

            #endregion

            #region B2CS Amendment Difference

            grdB2CSAmend.Rows[2].Cells[2].Value = Convert.ToDecimal(grdB2CSAmend.Rows[0].Cells[2].Value) - Convert.ToDecimal(grdB2CSAmend.Rows[1].Cells[2].Value);
            grdB2CSAmend.Rows[2].Cells[3].Value = Convert.ToDecimal(grdB2CSAmend.Rows[0].Cells[3].Value) - Convert.ToDecimal(grdB2CSAmend.Rows[1].Cells[3].Value);
            grdB2CSAmend.Rows[2].Cells[4].Value = Convert.ToDecimal(grdB2CSAmend.Rows[0].Cells[4].Value) - Convert.ToDecimal(grdB2CSAmend.Rows[1].Cells[4].Value);
            grdB2CSAmend.Rows[2].Cells[5].Value = Convert.ToDecimal(grdB2CSAmend.Rows[0].Cells[5].Value) - Convert.ToDecimal(grdB2CSAmend.Rows[1].Cells[5].Value);
            grdB2CSAmend.Rows[2].Cells[6].Value = Convert.ToDecimal(grdB2CSAmend.Rows[0].Cells[6].Value) - Convert.ToDecimal(grdB2CSAmend.Rows[1].Cells[6].Value);
            grdB2CSAmend.Rows[2].Cells[7].Value = Convert.ToDecimal(grdB2CSAmend.Rows[0].Cells[7].Value) - Convert.ToDecimal(grdB2CSAmend.Rows[1].Cells[7].Value);
            grdB2CSAmend.Rows[2].Cells[8].Value = Convert.ToDecimal(grdB2CSAmend.Rows[0].Cells[8].Value) - Convert.ToDecimal(grdB2CSAmend.Rows[1].Cells[8].Value);
            grdB2CSAmend.Rows[2].Cells[9].Value = Convert.ToDecimal(grdB2CSAmend.Rows[0].Cells[9].Value) - Convert.ToDecimal(grdB2CSAmend.Rows[1].Cells[9].Value);

            #endregion

            #region Export Amendment Difference

            grdExportAmend.Rows[2].Cells[2].Value = Convert.ToDecimal(grdExportAmend.Rows[0].Cells[2].Value) - Convert.ToDecimal(grdExportAmend.Rows[1].Cells[2].Value);
            grdExportAmend.Rows[2].Cells[3].Value = Convert.ToDecimal(grdExportAmend.Rows[0].Cells[3].Value) - Convert.ToDecimal(grdExportAmend.Rows[1].Cells[3].Value);
            grdExportAmend.Rows[2].Cells[4].Value = Convert.ToDecimal(grdExportAmend.Rows[0].Cells[4].Value) - Convert.ToDecimal(grdExportAmend.Rows[1].Cells[4].Value);
            grdExportAmend.Rows[2].Cells[5].Value = Convert.ToDecimal(grdExportAmend.Rows[0].Cells[5].Value) - Convert.ToDecimal(grdExportAmend.Rows[1].Cells[5].Value);
            grdExportAmend.Rows[2].Cells[6].Value = Convert.ToDecimal(grdExportAmend.Rows[0].Cells[6].Value) - Convert.ToDecimal(grdExportAmend.Rows[1].Cells[6].Value);
            grdExportAmend.Rows[2].Cells[7].Value = Convert.ToDecimal(grdExportAmend.Rows[0].Cells[7].Value) - Convert.ToDecimal(grdExportAmend.Rows[1].Cells[7].Value);
            grdExportAmend.Rows[2].Cells[8].Value = Convert.ToDecimal(grdExportAmend.Rows[0].Cells[8].Value) - Convert.ToDecimal(grdExportAmend.Rows[1].Cells[8].Value);
            grdExportAmend.Rows[2].Cells[9].Value = Convert.ToDecimal(grdExportAmend.Rows[0].Cells[9].Value) - Convert.ToDecimal(grdExportAmend.Rows[1].Cells[9].Value);

            #endregion

            #region CDNR Amendment Difference

            grdCDNRAmend.Rows[2].Cells[2].Value = Convert.ToDecimal(grdCDNRAmend.Rows[0].Cells[2].Value) - Convert.ToDecimal(grdCDNRAmend.Rows[1].Cells[2].Value);
            grdCDNRAmend.Rows[2].Cells[3].Value = Convert.ToDecimal(grdCDNRAmend.Rows[0].Cells[3].Value) - Convert.ToDecimal(grdCDNRAmend.Rows[1].Cells[3].Value);
            grdCDNRAmend.Rows[2].Cells[4].Value = Convert.ToDecimal(grdCDNRAmend.Rows[0].Cells[4].Value) - Convert.ToDecimal(grdCDNRAmend.Rows[1].Cells[4].Value);
            grdCDNRAmend.Rows[2].Cells[5].Value = Convert.ToDecimal(grdCDNRAmend.Rows[0].Cells[5].Value) - Convert.ToDecimal(grdCDNRAmend.Rows[1].Cells[5].Value);
            grdCDNRAmend.Rows[2].Cells[6].Value = Convert.ToDecimal(grdCDNRAmend.Rows[0].Cells[6].Value) - Convert.ToDecimal(grdCDNRAmend.Rows[1].Cells[6].Value);
            grdCDNRAmend.Rows[2].Cells[7].Value = Convert.ToDecimal(grdCDNRAmend.Rows[0].Cells[7].Value) - Convert.ToDecimal(grdCDNRAmend.Rows[1].Cells[7].Value);
            grdCDNRAmend.Rows[2].Cells[8].Value = Convert.ToDecimal(grdCDNRAmend.Rows[0].Cells[8].Value) - Convert.ToDecimal(grdCDNRAmend.Rows[1].Cells[8].Value);
            grdCDNRAmend.Rows[2].Cells[9].Value = Convert.ToDecimal(grdCDNRAmend.Rows[0].Cells[9].Value) - Convert.ToDecimal(grdCDNRAmend.Rows[1].Cells[9].Value);

            #endregion

            #region CDNUR Amendment Difference

            grdCDNURAmend.Rows[2].Cells[2].Value = Convert.ToDecimal(grdCDNURAmend.Rows[0].Cells[2].Value) - Convert.ToDecimal(grdCDNURAmend.Rows[1].Cells[2].Value);
            grdCDNURAmend.Rows[2].Cells[3].Value = Convert.ToDecimal(grdCDNURAmend.Rows[0].Cells[3].Value) - Convert.ToDecimal(grdCDNURAmend.Rows[1].Cells[3].Value);
            grdCDNURAmend.Rows[2].Cells[4].Value = Convert.ToDecimal(grdCDNURAmend.Rows[0].Cells[4].Value) - Convert.ToDecimal(grdCDNURAmend.Rows[1].Cells[4].Value);
            grdCDNURAmend.Rows[2].Cells[5].Value = Convert.ToDecimal(grdCDNURAmend.Rows[0].Cells[5].Value) - Convert.ToDecimal(grdCDNURAmend.Rows[1].Cells[5].Value);
            grdCDNURAmend.Rows[2].Cells[6].Value = Convert.ToDecimal(grdCDNURAmend.Rows[0].Cells[6].Value) - Convert.ToDecimal(grdCDNURAmend.Rows[1].Cells[6].Value);
            grdCDNURAmend.Rows[2].Cells[7].Value = Convert.ToDecimal(grdCDNURAmend.Rows[0].Cells[7].Value) - Convert.ToDecimal(grdCDNURAmend.Rows[1].Cells[7].Value);
            grdCDNURAmend.Rows[2].Cells[8].Value = Convert.ToDecimal(grdCDNURAmend.Rows[0].Cells[8].Value) - Convert.ToDecimal(grdCDNURAmend.Rows[1].Cells[8].Value);
            grdCDNURAmend.Rows[2].Cells[9].Value = Convert.ToDecimal(grdCDNURAmend.Rows[0].Cells[9].Value) - Convert.ToDecimal(grdCDNURAmend.Rows[1].Cells[9].Value);

            #endregion

            #region Nil Rated Difference  Not Needed

            //grdNilExemptNonGst.Rows[2].Cells[2].Value = Convert.ToDecimal(grdNilExemptNonGst.Rows[0].Cells[2].Value) - Convert.ToDecimal(grdNilExemptNonGst.Rows[1].Cells[2].Value);
            //grdNilExemptNonGst.Rows[2].Cells[3].Value = Convert.ToDecimal(grdNilExemptNonGst.Rows[0].Cells[3].Value) - Convert.ToDecimal(grdNilExemptNonGst.Rows[1].Cells[3].Value);
            //grdNilExemptNonGst.Rows[2].Cells[4].Value = Convert.ToDecimal(grdNilExemptNonGst.Rows[0].Cells[4].Value) - Convert.ToDecimal(grdNilExemptNonGst.Rows[1].Cells[4].Value);
            //grdNilExemptNonGst.Rows[2].Cells[5].Value = Convert.ToDecimal(grdNilExemptNonGst.Rows[0].Cells[5].Value) - Convert.ToDecimal(grdNilExemptNonGst.Rows[1].Cells[5].Value);
            //grdNilExemptNonGst.Rows[2].Cells[6].Value = Convert.ToDecimal(grdNilExemptNonGst.Rows[0].Cells[6].Value) - Convert.ToDecimal(grdNilExemptNonGst.Rows[1].Cells[6].Value);
            //grdNilExemptNonGst.Rows[2].Cells[7].Value = Convert.ToDecimal(grdNilExemptNonGst.Rows[0].Cells[7].Value) - Convert.ToDecimal(grdNilExemptNonGst.Rows[1].Cells[7].Value);
            //grdNilExemptNonGst.Rows[2].Cells[8].Value = Convert.ToDecimal(grdNilExemptNonGst.Rows[0].Cells[8].Value) - Convert.ToDecimal(grdNilExemptNonGst.Rows[1].Cells[8].Value);
            //grdNilExemptNonGst.Rows[2].Cells[9].Value = Convert.ToDecimal(grdNilExemptNonGst.Rows[0].Cells[9].Value) - Convert.ToDecimal(grdNilExemptNonGst.Rows[1].Cells[9].Value);

            #endregion

            #region Advance Received Difference

            grdAdvRcvdAmend.Rows[2].Cells[2].Value = Convert.ToDecimal(grdAdvRcvdAmend.Rows[0].Cells[2].Value) - Convert.ToDecimal(grdAdvRcvdAmend.Rows[1].Cells[2].Value);
            grdAdvRcvdAmend.Rows[2].Cells[3].Value = Convert.ToDecimal(grdAdvRcvdAmend.Rows[0].Cells[3].Value) - Convert.ToDecimal(grdAdvRcvdAmend.Rows[1].Cells[3].Value);
            grdAdvRcvdAmend.Rows[2].Cells[4].Value = Convert.ToDecimal(grdAdvRcvdAmend.Rows[0].Cells[4].Value) - Convert.ToDecimal(grdAdvRcvdAmend.Rows[1].Cells[4].Value);
            grdAdvRcvdAmend.Rows[2].Cells[5].Value = Convert.ToDecimal(grdAdvRcvdAmend.Rows[0].Cells[5].Value) - Convert.ToDecimal(grdAdvRcvdAmend.Rows[1].Cells[5].Value);
            grdAdvRcvdAmend.Rows[2].Cells[6].Value = Convert.ToDecimal(grdAdvRcvdAmend.Rows[0].Cells[6].Value) - Convert.ToDecimal(grdAdvRcvdAmend.Rows[1].Cells[6].Value);
            grdAdvRcvdAmend.Rows[2].Cells[7].Value = Convert.ToDecimal(grdAdvRcvdAmend.Rows[0].Cells[7].Value) - Convert.ToDecimal(grdAdvRcvdAmend.Rows[1].Cells[7].Value);
            grdAdvRcvdAmend.Rows[2].Cells[8].Value = Convert.ToDecimal(grdAdvRcvdAmend.Rows[0].Cells[8].Value) - Convert.ToDecimal(grdAdvRcvdAmend.Rows[1].Cells[8].Value);
            grdAdvRcvdAmend.Rows[2].Cells[9].Value = Convert.ToDecimal(grdAdvRcvdAmend.Rows[0].Cells[9].Value) - Convert.ToDecimal(grdAdvRcvdAmend.Rows[1].Cells[9].Value);

            #endregion

            #region Advance Adjust Amendment Difference

            grdAdvAdjstAmend.Rows[2].Cells[2].Value = Convert.ToDecimal(grdAdvAdjstAmend.Rows[0].Cells[2].Value) - Convert.ToDecimal(grdAdvAdjstAmend.Rows[1].Cells[2].Value);
            grdAdvAdjstAmend.Rows[2].Cells[3].Value = Convert.ToDecimal(grdAdvAdjstAmend.Rows[0].Cells[3].Value) - Convert.ToDecimal(grdAdvAdjstAmend.Rows[1].Cells[3].Value);
            grdAdvAdjstAmend.Rows[2].Cells[4].Value = Convert.ToDecimal(grdAdvAdjstAmend.Rows[0].Cells[4].Value) - Convert.ToDecimal(grdAdvAdjstAmend.Rows[1].Cells[4].Value);
            grdAdvAdjstAmend.Rows[2].Cells[5].Value = Convert.ToDecimal(grdAdvAdjstAmend.Rows[0].Cells[5].Value) - Convert.ToDecimal(grdAdvAdjstAmend.Rows[1].Cells[5].Value);
            grdAdvAdjstAmend.Rows[2].Cells[6].Value = Convert.ToDecimal(grdAdvAdjstAmend.Rows[0].Cells[6].Value) - Convert.ToDecimal(grdAdvAdjstAmend.Rows[1].Cells[6].Value);
            grdAdvAdjstAmend.Rows[2].Cells[7].Value = Convert.ToDecimal(grdAdvAdjstAmend.Rows[0].Cells[7].Value) - Convert.ToDecimal(grdAdvAdjstAmend.Rows[1].Cells[7].Value);
            grdAdvAdjstAmend.Rows[2].Cells[8].Value = Convert.ToDecimal(grdAdvAdjstAmend.Rows[0].Cells[8].Value) - Convert.ToDecimal(grdAdvAdjstAmend.Rows[1].Cells[8].Value);
            grdAdvAdjstAmend.Rows[2].Cells[9].Value = Convert.ToDecimal(grdAdvAdjstAmend.Rows[0].Cells[9].Value) - Convert.ToDecimal(grdAdvAdjstAmend.Rows[1].Cells[9].Value);

            #endregion


            #endregion
        
        }


        private void btnDownloadGSTNData_Click(object sender, EventArgs e)
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
                            btnDownloadGSTNData_Click(sender, e);
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
        public void GetJSONData(string jsonString)
        {
            try
            {
                int _Result = 0;
                MC.Open();
                MC.BeginTransaction();
                #region first delete old data from database
                string Query = "Delete from SPQGSTNSummary where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                //_Result = objGSTR5.IUDData(Query);
                MC.sqlcmd = new SQLiteCommand(Query, MC.con, MC.Transaction);
                _Result = Convert.ToInt32(MC.sqlcmd.ExecuteNonQuery());

                //if (_Result != 1)
                //{
                //    MessageBox.Show("System error.\nPlease try after sometime - SPQGSTNSummary!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
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

                    //_Result = objGSTR5.GSTNSummaryBulkEntryJson(dt);
                    foreach (DataRow dr in dt.Rows)
                    {
                        strQuery = "insert into SPQGSTNSummary(Fld_SectionName,Fld_InvoiceNo,Fld_TaxValue,Fld_IGST,Fld_CGST,Fld_SGST,Fld_CESS,Fld_Month,Fld_FinancialYear)Values('" + Convert.ToString(dr["Fld_SectionName"]) + "','" + Convert.ToString(dr["Fld_InvoiceNo"]) + "','" + Utility.Round(Convert.ToString(dr["Fld_TaxValue"])) + "','" + Utility.Round(Convert.ToString(dr["Fld_IGST"])) + "','" + Utility.Round(Convert.ToString(dr["Fld_CGST"])) + "','" + Utility.Round(Convert.ToString(dr["Fld_SGST"])) + "','" + Utility.Round(Convert.ToString(dr["Fld_CESS"])) + "','" + CommonHelper.SelectedMonth + "','" + CommonHelper.ReturnYear + "');";
                        MC.sqlcmd = new SQLiteCommand(strQuery, MC.con, MC.Transaction);
                        _Result = Convert.ToInt32(MC.sqlcmd.ExecuteNonQuery());
                        Application.DoEvents();
                    }

                    //if (_Result != 1)
                    //    MessageBox.Show("System error.\nPlease try after sometime - SPQGSTNSummary Save!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //else
                        MessageBox.Show("Data saved successfully..", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                MC.CommitTransaction();
                BindSummaryData();
                BindSummaryAmendedData();
                BindDifference();
            }
            catch (Exception ex)
            {
                pbGSTR1.Visible = false;
                MC.RollbackTransaction();
                MessageBox.Show("Error : " + ex.Message, "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
            finally
            {
                pbGSTR1.Visible = false;
                MC.Close();
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

        private void BindSummaryDataOLD()
        {
            DataTable dtSumamry = new DataTable();

            // B2B Sumamry
            #region B2B Summary
            strQuery = " Select  'B2B' Type , 'Portal Summary' Description,  ifnull(Fld_InvoiceNo,0) TotalInvoice, " +
                       " ifnull(sum(replace(Fld_TaxValue,',','')),0.00) TaxableValue, " +
                       " ifnull(sum(replace(Fld_IGST,',','')),0.00) IGST, " +
                       " ifnull(sum(replace(Fld_CGST,',','')),0.00)  CGST, " +
                       " ifnull(sum(replace(Fld_SGST,',','')),0.00) SGST, " +
                       " ifnull( sum(replace(Fld_CESS,',','')),0.00)  Cess, " +
                       " (ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                       " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00)) TotalGST, " +
                       " (ifnull(sum(replace(Fld_TaxValue,',','')),0.00) + ( ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                       " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00))) as InvoiceValue " +
                       " from SPQGSTNSummary where Fld_SectionName='B2B' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' " +
                      " union all " +
                      " select 'B2B' Type ,'Software Summary' Description, count(*) TotalInvoice, " +
                          "  ifnull(sum(replace(Fld_InvoiceTaxableVal,',','')),0.00) TaxableValue, " +
                          "  ifnull(sum(replace(Fld_IGSTAmnt,',','')),0.00) IGST, " +
                          "  ifnull(sum(replace(Fld_CGSTAmnt,',','')),0.00)  CGST, " +
                          "  ifnull(sum(replace(Fld_SGSTAmnt,',','')),0.00) SGST, " +
                          "  ifnull( sum(replace(Fld_CessAmount,',','')),0.00)  Cess, " +
                          "  (ifnull(sum(replace(Fld_IGSTAmnt,',','')),0.00) + ifnull(sum(replace(Fld_CGSTAmnt,',','')),0.00) + " +
                          "  ifnull(sum(replace(Fld_SGSTAmnt,',','')),0.00) +  ifnull( sum(replace(Fld_CessAmount,',','')),0.00)) TotalGST, " +
                        " ifnull(sum(replace(Fld_InvoiceValue,',','')),0.00) InvoiceValue " +
                       " from SPQR1B2B where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
            strQuery += " union all select 'B2B' ,'Difference',0 ,0 ,0,0,0,0,0,0 ";
            #endregion


            strQuery += " union all select 'B2CL', 'B2C Large Invoice', '0','0','0', '0', '0','0' , '0', '0' ";
            strQuery += " union all ";

            // B2CL Sumamry
            #region B2CL Sumamry
            strQuery += " Select 'B2CL' Type ,  'Portal Summary' Description,  ifnull(Fld_InvoiceNo,0) TotalInvoice, " +
                    " ifnull(sum(replace(Fld_TaxValue,',','')),0.00) TaxableValue, " +
                    " ifnull(sum(replace(Fld_IGST,',','')),0.00) IGST, " +
                    " ifnull(sum(replace(Fld_CGST,',','')),0.00)  CGST, " +
                    " ifnull(sum(replace(Fld_SGST,',','')),0.00) SGST, " +
                    " ifnull( sum(replace(Fld_CESS,',','')),0.00)  Cess, " +
                    " (ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                    " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00)) TotalGST, " +
                    " (ifnull(sum(replace(Fld_TaxValue,',','')),0.00) + ( ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                    " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00))) as InvoiceValue " +
                    " from SPQGSTNSummary where Fld_SectionName='B2CL' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' " +
                   " union all " +
                   " select 'B2CL' Type , 'Software Summary' Description, count(Fld_InvoiceNo) TotalInvoice, " +
                       "  ifnull(sum(replace(Fld_TaxableValue,',','')),0.00) TaxableValue, " +
                       "  ifnull(sum(replace(Fld_IGST,',','')),0.00) IGSTt, " +
                       "  0.00  CGST, " +
                       "  0.00 SGST, " +
                       "  ifnull( sum(replace(Fld_Cess,',','')),0.00)  Cess, " +
                       "  (ifnull(sum(replace(Fld_IGST,',','')),0.00) +  ifnull( sum(replace(Fld_Cess,',','')),0.00)) TotalGST, " +
                     " ifnull(sum(replace(Fld_InvoiceValue,',','')),0.00) InvoiceValue " +
                    " from SPQR1B2CL where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
            strQuery += " union all select 'B2CL' ,'Difference',0 ,0 ,0,0,0,0,0,0 ";
            #endregion


            strQuery += " union all select 'B2CS', 'B2C Small Invoice', '0','0','0', '0', '0','0' , '0', '0' ";
            strQuery += " union all ";

            // B2CS
            #region B2CS Query
            strQuery += " Select 'B2CS' Type ,  'Portal Summary' Description,  ifnull(Fld_InvoiceNo,0) TotalInvoice, " +
                    " ifnull(sum(replace(Fld_TaxValue,',','')),0.00) TaxableValue, " +
                    " ifnull(sum(replace(Fld_IGST,',','')),0.00) IGST, " +
                    " ifnull(sum(replace(Fld_CGST,',','')),0.00)  CGST, " +
                    " ifnull(sum(replace(Fld_SGST,',','')),0.00) SGST, " +
                    " ifnull( sum(replace(Fld_CESS,',','')),0.00)  Cess, " +
                    " (ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                    " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00)) TotalGST, " +
                    " (ifnull(sum(replace(Fld_TaxValue,',','')),0.00) + ( ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                    " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00))) as InvoiceValue " +
                    " from SPQGSTNSummary where Fld_SectionName='B2CS' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' " +
                   " union all " +
                   " select 'B2CS' Type , 'Software Summary' Description, count(*) TotalInvoice, " +
                       "  ifnull(sum(replace(Fld_TaxableValue,',','')),0.00) TaxableValue, " +
                       "  ifnull(sum(replace(Fld_IGST,',','')),0.00) IGST, " +
                       " ifnull(sum(replace(Fld_CGST,',','')),0.00) CGST, " +
                       "  ifnull(sum(replace(Fld_SGST,',','')),0.00) SGST, " +
                       "  ifnull( sum(replace(Fld_Cess,',','')),0.00)  Cess, " +
                       "  (ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                       " ifnull(sum(replace(Fld_SGST,',','')),0.00) + ifnull( sum(replace(Fld_Cess,',','')),0.00)) TotalGST, " +
                      " ( ifnull(sum(replace(Fld_TaxableValue,',','')),0.00)+ ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                       " ifnull(sum(replace(Fld_SGST,',','')),0.00) + ifnull( sum(replace(Fld_Cess,',','')),0.00)) InvoiceValue " +
                    " from SPQR1B2CS where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
            strQuery += " union all select 'B2CS' ,'Difference',0 ,0 ,0,0,0,0,0,0 ";
            #endregion


            strQuery += " union all select 'EXP', 'Export Invoice', '0','0','0', '0', '0','0' , '0', '0' ";
            strQuery += " union all ";

            // Export Invoices
            #region Export Invoices (SPQR1ZeroRated)

            strQuery += " Select 'EXP' Type ,  'Portal Summary' Description,  ifnull(Fld_InvoiceNo,0) TotalInvoice, " +
                  " ifnull(sum(replace(Fld_TaxValue,',','')),0.00) TaxableValue, " +
                  " ifnull(sum(replace(Fld_IGST,',','')),0.00) IGST, " +
                  " ifnull(sum(replace(Fld_CGST,',','')),0.00)  CGST, " +
                  " ifnull(sum(replace(Fld_SGST,',','')),0.00) SGST, " +
                  " ifnull( sum(replace(Fld_CESS,',','')),0.00)  Cess, " +
                  " (ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                  " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00)) TotalGST, " +
                  " (ifnull(sum(replace(Fld_TaxValue,',','')),0.00) + ( ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                  " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00))) as InvoiceValue " +
                  " from SPQGSTNSummary where Fld_SectionName='EXP' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' " +
                 " union all " +
                 " select 'EXP' Type , 'Software Summary' Description, count(Fld_InvoiceNo) TotalInvoice, " +
                     "  ifnull(sum(replace(Fld_IGSTInvoiceTaxableVal,',','')),0.00) TaxableValue, " +
                     "  ifnull(sum(replace(Fld_IGSTAmnt,',','')),0.00) IGSTt, " +
                     "  0.00  CGST, " +
                     "  0.00 SGST, " +
                     "  ifnull( sum(replace(Fld_CESS,',','')),0.00)  Cess, " +
                     "  (ifnull(sum(replace(Fld_IGSTAmnt,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00)) TotalGST, " +
                   " ifnull(sum(replace(Fld_InvoiceValue,',','')),0.00) InvoiceValue " +
                  " from SPQR1ZeroRated where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
            strQuery += " union all select 'EXP' ,'Difference',0 ,0 ,0,0,0,0,0,0 ";

            #endregion


            strQuery += " union all select 'CDN', 'Cr./Dr. Note (Reg.)', '0','0','0', '0', '0','0' , '0', '0' ";
            strQuery += " union all ";

            // Cr./ Dr. Note Regd 
            #region Cr.Dr. Note Regd.

            strQuery += " Select  'CDN' Type , 'Portal Summary' Description,  ifnull(Fld_InvoiceNo,0) TotalInvoice, " +
                   " ifnull(sum(replace(Fld_TaxValue,',','')),0.00) TaxableValue, " +
                   " ifnull(sum(replace(Fld_IGST,',','')),0.00) IGST, " +
                   " ifnull(sum(replace(Fld_CGST,',','')),0.00)  CGST, " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) SGST, " +
                   " ifnull( sum(replace(Fld_CESS,',','')),0.00)  Cess, " +
                   " (ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00)) TotalGST, " +
                   " (ifnull(sum(replace(Fld_TaxValue,',','')),0.00) + ( ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00))) as InvoiceValue " +
                   " from SPQGSTNSummary where Fld_SectionName='CDN' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' " +
                  " union all " +
                  " select 'CDN' Type ,'Software Summary' Description, count(*) TotalInvoice, " +
                      "  ifnull(sum(replace(Fld_Taxable,',','')),0.00) TaxableValue, " +
                      "  ifnull(sum(replace(Fld_IGSTAmnt,',','')),0.00) IGST, " +
                      "  ifnull(sum(replace(Fld_CGSTAmnt,',','')),0.00)  CGST, " +
                      "  ifnull(sum(replace(Fld_SGSTAmnt,',','')),0.00) SGST, " +
                      "  ifnull( sum(replace(Fld_CessAmnt,',','')),0.00)  Cess, " +
                      "  (ifnull(sum(replace(Fld_IGSTAmnt,',','')),0.00) + ifnull(sum(replace(Fld_CGSTAmnt,',','')),0.00) + " +
                      "  ifnull(sum(replace(Fld_SGSTAmnt,',','')),0.00) +  ifnull( sum(replace(Fld_CessAmnt,',','')),0.00)) TotalGST, " +
                    " ifnull(sum(replace(Fld_OrgInvoiceValue,',','')),0.00) InvoiceValue " +
                   " from SPQR1CDN where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
            strQuery += " union all select 'CDN' ,'Difference',0 ,0 ,0,0,0,0,0,0 ";

            #endregion


            strQuery += " union all select 'CDNUR', 'Cr./Dr. Note (UnRegd)', '0','0','0', '0', '0','0' , '0', '0' ";
            strQuery += " union all ";

            // Cr./ Dr. Note Un-Regd 
            #region Cr.Dr. Note Un-Regd.

            strQuery += " Select  'CDNUR' Type , 'Portal Summary' Description,  ifnull(Fld_InvoiceNo,0) TotalInvoice, " +
                   " ifnull(sum(replace(Fld_TaxValue,',','')),0.00) TaxableValue, " +
                   " ifnull(sum(replace(Fld_IGST,',','')),0.00) IGST, " +
                   " ifnull(sum(replace(Fld_CGST,',','')),0.00)  CGST, " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) SGST, " +
                   " ifnull( sum(replace(Fld_CESS,',','')),0.00)  Cess, " +
                   " (ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00)) TotalGST, " +
                   " (ifnull(sum(replace(Fld_TaxValue,',','')),0.00) + ( ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00))) as InvoiceValue " +
                   " from SPQGSTNSummary where Fld_SectionName='CDNUR' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' " +
                  " union all " +
                  " select 'CDNUR' Type ,'Software Summary' Description, count(*) TotalInvoice, " +
                      "  ifnull(sum(replace(Fld_Taxable,',','')),0.00) TaxableValue, " +
                      "  ifnull(sum(replace(Fld_IGSTAmnt,',','')),0.00) IGST, " +
                      "  ifnull(sum(replace(Fld_CGSTAmnt,',','')),0.00)  CGST, " +
                      "  ifnull(sum(replace(Fld_SGSTAmnt,',','')),0.00) SGST, " +
                      "  ifnull( sum(replace(Fld_CessAmnt,',','')),0.00)  Cess, " +
                      "  (ifnull(sum(replace(Fld_IGSTAmnt,',','')),0.00) + ifnull(sum(replace(Fld_CGSTAmnt,',','')),0.00) + " +
                      "  ifnull(sum(replace(Fld_SGSTAmnt,',','')),0.00) +  ifnull( sum(replace(Fld_CessAmnt,',','')),0.00)) TotalGST, " +
                    " ifnull(sum(replace(Fld_OrgInvoiceValue,',','')),0.00) InvoiceValue " +
                   " from SPQR1CDNUR where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
            strQuery += " union all select 'CDNUR' ,'Difference',0 ,0 ,0,0,0,0,0,0 ";

            #endregion


            strQuery += " union all select 'NIL', 'NIL/Exempted/Non-GST', '0','0','0', '0', '0','0' , '0', '0' ";
            strQuery += " union all ";

            // Nil Rated 
            #region Nil Rated

            strQuery += " Select  'NIL' Type , 'Portal Summary' Description,  ifnull(Fld_InvoiceNo,0) TotalInvoice, " +
                 " ifnull(sum(replace(Fld_TaxValue,',','')),0.00) TaxableValue, " +
                 " ifnull(sum(replace(Fld_IGST,',','')),0.00) IGST, " +
                 " ifnull(sum(replace(Fld_CGST,',','')),0.00)  CGST, " +
                 " ifnull(sum(replace(Fld_SGST,',','')),0.00) SGST, " +
                 " ifnull( sum(replace(Fld_CESS,',','')),0.00)  Cess, " +
                 " (ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                 " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00)) TotalGST, " +
                 " (ifnull(sum(replace(Fld_TaxValue,',','')),0.00) + ( ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                 " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00))) as InvoiceValue " +
                 " from SPQGSTNSummary where Fld_SectionName='NIL' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' " +
                " union all " +
                " Select  'NIL' Type , 'Software Summary' Description, " +
                  "   (Select case when (ifnull(sum(replace(Fld_NilRatedSupply,',','')),0.00) +  ifnull(sum(replace(Fld_Exempted,',','')),0.00) + ifnull(sum(replace(Fld_NonGSTSupplies,',','')),0.00)) > 0 then 1 else 0 end as TotalInvoice " +
                     " from SPQR1NilRated where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "') ,0 ,0,0,0,0,0,0 ";
            strQuery += " union all select 'NIL' ,'Difference',0 ,0 ,0,0,0,0,0,0 ";
            #endregion


            strQuery += " union all select 'AR', 'Advance Received', '0','0','0', '0', '0','0' , '0', '0' ";
            strQuery += " union all ";

            //  Advance Received
            #region Advance Received

            strQuery += " Select  'AR' Type , 'Portal Summary' Description,  ifnull(Fld_InvoiceNo,0) TotalInvoice, " +
                   " ifnull(sum(replace(Fld_TaxValue,',','')),0.00) TaxableValue, " +
                   " ifnull(sum(replace(Fld_IGST,',','')),0.00) IGST, " +
                   " ifnull(sum(replace(Fld_CGST,',','')),0.00)  CGST, " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) SGST, " +
                   " ifnull( sum(replace(Fld_CESS,',','')),0.00)  Cess, " +
                   " (ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00)) TotalGST, " +
                   " (ifnull(sum(replace(Fld_TaxValue,',','')),0.00) + ( ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00))) as InvoiceValue " +
                   " from SPQGSTNSummary where Fld_SectionName='AR' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' " +
                  " union all " +
                  " select 'AR' Type ,'Software Summary' Description, count(*) TotalInvoice, " +
                      "  ifnull(sum(replace(Fld_GrossAdvRcv,',','')),0.00) TaxableValue, " +
                      "  ifnull(sum(replace(Fld_IGSTAmnt,',','')),0.00) IGST, " +
                      "  ifnull(sum(replace(Fld_CGSTAmnt,',','')),0.00)  CGST, " +
                      "  ifnull(sum(replace(Fld_SGSTAmnt,',','')),0.00) SGST, " +
                      "  ifnull( sum(replace(Fld_CessAmount,',','')),0.00)  Cess, " +
                      "  (ifnull(sum(replace(Fld_IGSTAmnt,',','')),0.00) + ifnull(sum(replace(Fld_CGSTAmnt,',','')),0.00) + " +
                      "  ifnull(sum(replace(Fld_SGSTAmnt,',','')),0.00) +  ifnull( sum(replace(Fld_CessAmount,',','')),0.00)) TotalGST, " +
                      "  0.00 as InvoiceValue " +
                   " from SPQR1GrossAdvance where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
            strQuery += " union all select 'AR' ,'Difference',0 ,0 ,0,0,0,0,0,0 ";


            #endregion


            strQuery += " union all select 'AA', 'Advance Adjusted', '0','0','0', '0', '0','0' , '0', '0' ";
            strQuery += " union all ";

            //  Advance Adjusted
            #region Advance Adjusted

            strQuery += " Select  'AA' Type , 'Portal Summary' Description,  ifnull(Fld_InvoiceNo,0) TotalInvoice, " +
                   " ifnull(sum(replace(Fld_TaxValue,',','')),0.00) TaxableValue, " +
                   " ifnull(sum(replace(Fld_IGST,',','')),0.00) IGST, " +
                   " ifnull(sum(replace(Fld_CGST,',','')),0.00)  CGST, " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) SGST, " +
                   " ifnull( sum(replace(Fld_CESS,',','')),0.00)  Cess, " +
                   " (ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00)) TotalGST, " +
                   " (ifnull(sum(replace(Fld_TaxValue,',','')),0.00) + ( ifnull(sum(replace(Fld_IGST,',','')),0.00) + ifnull(sum(replace(Fld_CGST,',','')),0.00) + " +
                   " ifnull(sum(replace(Fld_SGST,',','')),0.00) +  ifnull( sum(replace(Fld_CESS,',','')),0.00))) as InvoiceValue " +
                   " from SPQGSTNSummary where Fld_SectionName='AA' and Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' " +
                  " union all " +
                  " select 'AA' Type ,'Software Summary' Description, count(*) TotalInvoice, " +
                      "  ifnull(sum(replace(Fld_Advadj,',','')),0.00) TaxableValue, " +
                      "  ifnull(sum(replace(Fld_IGSTAmnt,',','')),0.00) IGST, " +
                      "  ifnull(sum(replace(Fld_CGSTAmnt,',','')),0.00)  CGST, " +
                      "  ifnull(sum(replace(Fld_SGSTAmnt,',','')),0.00) SGST, " +
                      "  ifnull( sum(replace(Fld_CessAmount,',','')),0.00)  Cess, " +
                      "  (ifnull(sum(replace(Fld_IGSTAmnt,',','')),0.00) + ifnull(sum(replace(Fld_CGSTAmnt,',','')),0.00) + " +
                      "  ifnull(sum(replace(Fld_SGSTAmnt,',','')),0.00) +  ifnull( sum(replace(Fld_CessAmount,',','')),0.00)) TotalGST, " +
                      "  0.00 as InvoiceValue " +
                   " from SPQR1NetAdvance where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' and Fld_FileStatus != 'Total'";
            strQuery += " union all select 'AA' ,'Difference',0 ,0 ,0,0,0,0,0,0 ";


            #endregion


            dt = MC.GetValueindatatable(strQuery);

            //DataRow dr = dt.NewRow();

            string[] arraycolheader = new string[] { "", "Advance Adjusted", "No of Invoices", "Taxable Value", "IGST", "CGST", "SGST", "CESS", "Total GST", "Invoice Value" };

            foreach (DataGridViewColumn col in grdB2b.Columns)
            {
                col.DataPropertyName = dt.Columns[col.Index].ColumnName;
                if (col.Index > 1)
                {
                    col.DefaultCellStyle.Format = string.Format("N2");
                    col.DefaultCellStyle.NullValue = "-";
                }

            }

            grdB2b.DataSource = dt;

            int rowindex = 1;
            foreach (DataGridViewRow dr in grdB2b.Rows)
            {

                if (dr.Index > 2)
                {
                    if (dr.Index == (4 * rowindex - 1))
                    {
                        dr.Cells[0].Value = rowindex + 1;
                        //dr.Cells[2].Value = "";//"no of invoices";
                        //dr.Cells[3].Value = "";//"axable value";
                        //dr.Cells[4].Value = "";//"igst";
                        //dr.Cells[5].Value = "";//"cgst";
                        //dr.Cells[6].Value = "";//"sgst";
                        //dr.Cells[7].Value = "";//"cess";
                        //dr.Cells[8].Value = "";//"Total gst";
                        //dr.Cells[9].Value = "";//"invoice value";
                        rowindex++;
                    }
                    else dr.Cells[0].Value = "";

                }
                else dr.Cells[0].Value = "";
                //if (dr.Index == 3) dr.Cells[1].Value = "";

            }

            //int rowindex=0;
            //for (int i = 0; i < grdGstr1Summary.Rows.Count; i++ )
            //{
            //    if (i == 3) grdGstr1Summary.Rows.Insert(3, "B2CL", "Advance Adjusted", "No of Invoices", "Taxable Value", "IGST", "CGST", "SGST", "CESS", "Total GST", "Invoice Value");
            //    if (i == 7) grdGstr1Summary.Rows.Insert(7, "B2CS", "Advance Adjusted", "No of Invoices", "Taxable Value", "IGST", "CGST", "SGST", "CESS", "Total GST", "Invoice Value");
            //    if (i == 11) grdGstr1Summary.Rows.Insert(11, "Zero Rated", "Advance Adjusted", "No of Invoices", "Taxable Value", "IGST", "CGST", "SGST", "CESS", "Total GST", "Invoice Value");
            //}

        }
        private void msSave_Click(object sender, EventArgs e)
        {
            #region ADD DATATABLE COLUMN

            // CREATE DATATABLE TO STORE MAIN GRID DATA
            DataTable dt = new DataTable();

            // ADD DATATBLE COLLUMN AS PAR MAIN  GRID COLUMN
            //foreach (DataGridViewColumn col in dgvB2B.Columns)
            //{
            //    dt.Columns.Add(col.Name.ToString());
            //}
            #endregion

            #region GET DATA
            //if (dgvB2B.Rows.Count == 3)
            //{
            //    DataRow dr = dt.NewRow();
            //    dr["colSectionName"] = "B2B";
            //    dr["colInvoices"] = Convert.ToString(dgvB2B.Rows[1].Cells["colInvoices"].Value).Trim();
            //    dr["colTaxValue"] = Convert.ToString(dgvB2B.Rows[1].Cells["colTaxValue"].Value).Trim();
            //    dr["colIGST"] = Convert.ToString(dgvB2B.Rows[1].Cells["colIGST"].Value).Trim();
            //    dr["colCGST"] = Convert.ToString(dgvB2B.Rows[1].Cells["colCGST"].Value).Trim();
            //    dr["colSGST"] = Convert.ToString(dgvB2B.Rows[1].Cells["colSGST"].Value).Trim();
            //    dr["colCess"] = Convert.ToString(dgvB2B.Rows[1].Cells["colCess"].Value).Trim();
            //    dt.Rows.Add(dr);
            //}
            //if (dgvB2CL.Rows.Count == 3)
            //{
            //    DataRow dr = dt.NewRow();
            //    dr["colSectionName"] = "B2CL";
            //    dr["colInvoices"] = Convert.ToString(dgvB2CL.Rows[1].Cells["colInvoicesB2CL"].Value).Trim();
            //    dr["colTaxValue"] = Convert.ToString(dgvB2CL.Rows[1].Cells["colTaxValueB2CL"].Value).Trim();
            //    dr["colIGST"] = Convert.ToString(dgvB2CL.Rows[1].Cells["colIGSTB2CL"].Value).Trim();
            //    dr["colCGST"] = Convert.ToString(dgvB2CL.Rows[1].Cells["colCGSTB2CL"].Value).Trim();
            //    dr["colSGST"] = Convert.ToString(dgvB2CL.Rows[1].Cells["colCGSTB2CL"].Value).Trim();
            //    dr["colCess"] = Convert.ToString(dgvB2CL.Rows[1].Cells["colCessB2CL"].Value).Trim();
            //    dt.Rows.Add(dr);
            //}
            //if (dgvB2CS.Rows.Count == 3)
            //{
            //    DataRow dr = dt.NewRow();
            //    dr["colSectionName"] = "B2CS";
            //    dr["colInvoices"] = Convert.ToString(dgvB2CS.Rows[1].Cells["colInvoicesB2CS"].Value).Trim();
            //    dr["colTaxValue"] = Convert.ToString(dgvB2CS.Rows[1].Cells["colTaxValueB2CS"].Value).Trim();
            //    dr["colIGST"] = Convert.ToString(dgvB2CS.Rows[1].Cells["colIGSTB2CS"].Value).Trim();
            //    dr["colCGST"] = Convert.ToString(dgvB2CS.Rows[1].Cells["colCGSTB2CS"].Value).Trim();
            //    dr["colSGST"] = Convert.ToString(dgvB2CS.Rows[1].Cells["colCGSTB2CS"].Value).Trim();
            //    dr["colCess"] = Convert.ToString(dgvB2CS.Rows[1].Cells["colCessB2CS"].Value).Trim();
            //    dt.Rows.Add(dr);
            //}
            //if (dgvZeroRated.Rows.Count == 3)
            //{
            //    DataRow dr = dt.NewRow();
            //    dr["colSectionName"] = "EXP";
            //    dr["colInvoices"] = Convert.ToString(dgvZeroRated.Rows[1].Cells["colInvoicesZ"].Value).Trim();
            //    dr["colTaxValue"] = Convert.ToString(dgvZeroRated.Rows[1].Cells["colTaxValueZ"].Value).Trim();
            //    dr["colIGST"] = Convert.ToString(dgvZeroRated.Rows[1].Cells["colIGSTZ"].Value).Trim();
            //    dr["colCGST"] = Convert.ToString(dgvZeroRated.Rows[1].Cells["colCGSTZ"].Value).Trim();
            //    dr["colSGST"] = Convert.ToString(dgvZeroRated.Rows[1].Cells["colSGSTZ"].Value).Trim();
            //    dr["colCess"] = Convert.ToString(dgvZeroRated.Rows[1].Cells["colCessZ"].Value).Trim();
            //    dt.Rows.Add(dr);
            //}
            //if (dgvCDN.Rows.Count == 3)
            //{
            //    DataRow dr = dt.NewRow();
            //    dr["colSectionName"] = "CDN";
            //    dr["colInvoices"] = Convert.ToString(dgvCDN.Rows[1].Cells["colInvoicesCD"].Value).Trim();
            //    dr["colTaxValue"] = Convert.ToString(dgvCDN.Rows[1].Cells["colTaxValueCD"].Value).Trim();
            //    dr["colIGST"] = Convert.ToString(dgvCDN.Rows[1].Cells["colIGSTCD"].Value).Trim();
            //    dr["colCGST"] = Convert.ToString(dgvCDN.Rows[1].Cells["colCGSTCD"].Value).Trim();
            //    dr["colSGST"] = Convert.ToString(dgvCDN.Rows[1].Cells["colSGSTCD"].Value).Trim();
            //    dr["colCess"] = Convert.ToString(dgvCDN.Rows[1].Cells["colCessCD"].Value).Trim();
            //    dt.Rows.Add(dr);
            //}
            //if (CDnUR.Rows.Count == 3)
            //{
            //    DataRow dr = dt.NewRow();
            //    dr["colSectionName"] = "CDNUR";
            //    dr["colInvoices"] = Convert.ToString(CDnUR.Rows[1].Cells["colInvoicesCDNUR"].Value).Trim();
            //    dr["colTaxValue"] = Convert.ToString(CDnUR.Rows[1].Cells["colTaxValueCDNUR"].Value).Trim();
            //    dr["colIGST"] = Convert.ToString(CDnUR.Rows[1].Cells["colIGSTCDNUR"].Value).Trim();
            //    dr["colCGST"] = Convert.ToString(CDnUR.Rows[1].Cells["colCGSTCDNUR"].Value).Trim();
            //    dr["colSGST"] = Convert.ToString(CDnUR.Rows[1].Cells["colSGSTCDNUR"].Value).Trim();
            //    dr["colCess"] = Convert.ToString(CDnUR.Rows[1].Cells["colCessCDNUR"].Value).Trim();
            //    dt.Rows.Add(dr);
            //}
            //if (dgvNill.Rows.Count == 3)
            //{
            //    DataRow dr = dt.NewRow();
            //    dr["colSectionName"] = "NIL";
            //    dr["colInvoices"] = Convert.ToString(dgvNill.Rows[1].Cells["colInvoicesNIL"].Value).Trim();
            //    dr["colTaxValue"] = Convert.ToString(dgvNill.Rows[1].Cells["colTaxValueNIL"].Value).Trim();
            //    dr["colIGST"] = Convert.ToString(dgvNill.Rows[1].Cells["colIGSTNIL"].Value).Trim();
            //    dr["colCGST"] = Convert.ToString(dgvNill.Rows[1].Cells["colCGSTNIL"].Value).Trim();
            //    dr["colSGST"] = Convert.ToString(dgvNill.Rows[1].Cells["colSGSTNIL"].Value).Trim();
            //    dr["colCess"] = Convert.ToString(dgvNill.Rows[1].Cells["colCessNIL"].Value).Trim();
            //    dt.Rows.Add(dr);
            //}
            //if (dgvAR.Rows.Count == 3)
            //{
            //    DataRow dr = dt.NewRow();
            //    dr["colSectionName"] = "AR";
            //    dr["colInvoices"] = Convert.ToString(dgvAR.Rows[1].Cells["colInvoicesAR"].Value).Trim();
            //    dr["colTaxValue"] = Convert.ToString(dgvAR.Rows[1].Cells["colTaxValueAR"].Value).Trim();
            //    dr["colIGST"] = Convert.ToString(dgvAR.Rows[1].Cells["colIGSTAR"].Value).Trim();
            //    dr["colCGST"] = Convert.ToString(dgvAR.Rows[1].Cells["colCGSTAR"].Value).Trim();
            //    dr["colSGST"] = Convert.ToString(dgvAR.Rows[1].Cells["colSGSTAR"].Value).Trim();
            //    dr["colCess"] = Convert.ToString(dgvAR.Rows[1].Cells["colCessAR"].Value).Trim();
            //    dt.Rows.Add(dr);
            //}
            //if (dgvAA.Rows.Count == 3)
            //{
            //    DataRow dr = dt.NewRow();
            //    dr["colSectionName"] = "AA";
            //    dr["colInvoices"] = Convert.ToString(dgvAA.Rows[1].Cells["colInvoicesAA"].Value).Trim();
            //    dr["colTaxValue"] = Convert.ToString(dgvAA.Rows[1].Cells["colTaxValueAA"].Value).Trim();
            //    dr["colIGST"] = Convert.ToString(dgvAA.Rows[1].Cells["colIGSTAA"].Value).Trim();
            //    dr["colCGST"] = Convert.ToString(dgvAA.Rows[1].Cells["colCGSTAA"].Value).Trim();
            //    dr["colSGST"] = Convert.ToString(dgvAA.Rows[1].Cells["colSGSTAA"].Value).Trim();
            //    dr["colCess"] = Convert.ToString(dgvAA.Rows[1].Cells["colCessAA"].Value).Trim();
            //    dt.Rows.Add(dr);
            //}
            //if (dgvHSN.Rows.Count == 3)
            //{
            //    DataRow dr = dt.NewRow();
            //    dr["colSectionName"] = "HSN";
            //    dr["colInvoices"] = Convert.ToString(dgvHSN.Rows[1].Cells["colInvoicesHSN"].Value).Trim();
            //    dr["colTaxValue"] = Convert.ToString(dgvHSN.Rows[1].Cells["colTaxValueHSN"].Value).Trim();
            //    dr["colIGST"] = Convert.ToString(dgvHSN.Rows[1].Cells["colIGSTHSN"].Value).Trim();
            //    dr["colCGST"] = Convert.ToString(dgvHSN.Rows[1].Cells["colCGSTHSN"].Value).Trim();
            //    dr["colSGST"] = Convert.ToString(dgvHSN.Rows[1].Cells["colSGSTHSN"].Value).Trim();
            //    dr["colCess"] = Convert.ToString(dgvHSN.Rows[1].Cells["colCessHSN"].Value).Trim();
            //    dt.Rows.Add(dr);
            //}
            //if (dgvDoc.Rows.Count == 3)
            //{
            //    DataRow dr = dt.NewRow();
            //    dr["colSectionName"] = "DOC";
            //    dr["colInvoices"] = Convert.ToString(dgvDoc.Rows[1].Cells["colInvoicesDOC"].Value).Trim();
            //    dr["colTaxValue"] = Convert.ToString(dgvDoc.Rows[1].Cells["colTaxValDOC"].Value).Trim();
            //    dr["colIGST"] = Convert.ToString(dgvDoc.Rows[1].Cells["colIGSTDOC"].Value).Trim();
            //    dr["colCGST"] = Convert.ToString(dgvDoc.Rows[1].Cells["colCGSTDOC"].Value).Trim();
            //    dr["colSGST"] = Convert.ToString(dgvDoc.Rows[1].Cells["colSGSTDOC"].Value).Trim();
            //    dr["colCess"] = Convert.ToString(dgvDoc.Rows[1].Cells["colCessDOC"].Value).Trim();
            //    dt.Rows.Add(dr);
            //}
            #endregion

            if (dt != null && dt.Rows.Count > 0)
            {
                #region RECORD SAVE
                //string Query = "";
                //int _Result = 0;

                //// CHECK THERE ARE RECORDS IN GRID
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    #region FIRST DELETE OLD DATA FROM DATABASE
                //    Query = "Delete from SPQGSTNSummary where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";
                //    _Result = objGSTR5.IUDData(Query);
                //    if (_Result != 1)
                //    {
                //        // ERROR OCCURS WHILE DELETING DATA
                //        pbGSTR1.Visible = false;
                //        MessageBox.Show("System error.\nPlease try after sometime!", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        return;
                //    }
                //    #endregion

                //    // QUERY FIRE TO SAVE RECORDS TO DATABASE
                //    _Result = objGSTR5.GSTNSummaryBulkEntry(dt);

                //    if (_Result == 1)
                //    {
                //        pbGSTR1.Visible = false;

                //        //DONE
                //        MessageBox.Show("Data Saved Successfully …!!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //        // BIND DATA
                //        //GetData();
                //    }
                //    else
                //    {
                //        // IF ERRORS OCCURS WHILE INSERTING DATA TO DATABASE
                //        pbGSTR1.Visible = false;
                //        MessageBox.Show("System error.\nPlease try after sometime!", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        return;
                //    }
                //}
                //else
                //{
                //    #region DELETE ALL OLD RECORD IF THERE ARE NO RECORDS PRESENT IN GRID
                //    Query = "Delete from SPQGSTNSummary where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "'";

                //    // FIRE QUEARY TO DELETE RECORDS
                //    _Result = objGSTR5.IUDData(Query);

                //    if (_Result == 1)
                //    {
                //        // IF RECORDS DELETED FROM DATABASE  
                //        pbGSTR1.Visible = false;
                //        MessageBox.Show("Record Successfully Deleted!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    }
                //    else
                //    {
                //        // IF ERRORS OCCURS WHILE DELETING RECORD FROM THE DATABASE
                //        pbGSTR1.Visible = false;
                //        MessageBox.Show("System error.\nPlease try after sometime!", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        return;
                //    }
                //    #endregion
                //}
                #endregion
            }
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
