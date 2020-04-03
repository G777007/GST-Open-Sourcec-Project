//using DataLayer;
//using Newtonsoft.Json;
using Proactive.CustomTools.CustomDataGridView;
using Proactive.CustomTools;
//using SPEQTAGST.BAL;
//using SPEQTAGST.GstAuto;
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
using Newtonsoft.Json;

namespace SPEQTAGST.Report
{
    public partial class GSTR_9_Reports : Form
    {
        MainClass MC = new MainClass();
        string strQuery = "";
        DataSet dsgrid = new DataSet();
        private HttpWebResponse response;
        //AssesseeDetail assesseeModel;
        CookieContainer Cc = new CookieContainer();
        public GSTR_9_Reports()
        {
            InitializeComponent();
            lblYear.Text = "2017 - 2018";
            lblYear2.Text = "2017 - 2018";
            lblYear3.Text = "2017 - 2018";// CommonHelper.ReturnYear;
            CommonHelper.CompanyGSTN = "09AARFP2420N1ZR";
            CommonHelper.ReturnYear = "2017 - 2018";
            CommonHelper.SelectedMonth = "August";

            grdSummary.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            grdSummary.EnableHeadersVisualStyles = false;
            grdSummary.ColumnHeadersDefaultCellStyle.Font = new Font("Verdana", 8, FontStyle.Bold);
            grdSummary.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            this.WindowState = FormWindowState.Maximized;
            SetDefaultSettingForControl(tabGSTPortal);
         
        }

        private void GSTR_9_Reports_Load(object sender, EventArgs e)
        {
            // To initialize db Connection  Use this Method on Load
            MC.Connection();

            // Password=75T2@3P0W5R;";
            //datafile path = bin\Database\DBSPEQTAGST.db

            GetSetSoftwareData();
            GetSetGSTPortalData();
        }
        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tabcntrl = sender as TabControl;
            Graphics g = e.Graphics;
            Brush _textBrush;

            // Get the item from the collection.
            TabPage _tabPage = tabSoftware.TabPages[e.Index];

            // Get the real bounds for the tab rectangle.
            Rectangle _tabBounds = tabSoftware.GetTabRect(e.Index);

            if (e.State == DrawItemState.Selected)
            {
                // Draw a different background color, and don't paint a focus rectangle.


                //_textBrush = new SolidBrush(Color.Black);
                if (tabcntrl.Name == "tabGSTPortal") { _textBrush = new SolidBrush(Color.WhiteSmoke); g.FillRectangle(Brushes.Navy, e.Bounds); }
                else { _textBrush = new SolidBrush(Color.Black); g.FillRectangle(new SolidBrush(Color.FromArgb(23, 196, 187)), e.Bounds); }
                // Color.FromArgb(23,196,87)
            }
            else
            {
                //_textBrush = new SolidBrush(Color.WhiteSmoke); //e.ForeColor
                if (tabcntrl.Name == "tabGSTPortal") { _textBrush = new SolidBrush(Color.Black); g.FillRectangle(new SolidBrush(Color.FromArgb(23, 196, 187)), e.Bounds); }
                else { _textBrush = new SolidBrush(Color.WhiteSmoke); g.FillRectangle(Brushes.Navy, e.Bounds); }
                //e.DrawBackground();
            }

            // Use our own font.
            Font _tabFont = new Font("Verdana", (float)11.0, FontStyle.Bold, GraphicsUnit.Pixel);

            // Draw string. Center the text.
            StringFormat _stringFlags = new StringFormat();
            _stringFlags.Alignment = StringAlignment.Center;
            _stringFlags.LineAlignment = StringAlignment.Center;

            g.DrawString(_tabPage.Text, _tabFont, _textBrush, _tabBounds, new StringFormat(_stringFlags));
        }

        private void GetSetSoftwareData()
        {
            try
            {


                grdSummary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                Bind_GridSummary(grdSummary);
                grdSummary.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                grdSummary.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grdSummary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grdSummary.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                grd4_part2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                BindPart2_Grid4(grd4_part2);
                grd4_part2.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                grd4_part2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grd4_part2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grd4_part2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


                grd5_part2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                BindPart2_Grid5(grd5_part2);
                grd5_part2.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                grd5_part2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grd5_part2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grd5_part2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                grd6_part3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                BindPart3_Grid6(grd6_part3);
                grd6_part3.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                grd6_part3.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grd6_part3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grd6_part3.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                grd7_part3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                BindPart3_Grid7(grd7_part3);
                grd7_part3.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                grd7_part3.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grd7_part3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grd7_part3.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                grd8_part3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                BindPart3_Grid8(grd8_part3);
                grd8_part3.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                grd8_part3.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grd8_part3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grd8_part3.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                grd9_part4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                BindPart4_Grid9(grd9_part4);
                grd9_part4.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                grd9_part4.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grd9_part4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grd9_part4.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                grd10_part5.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                BindPart5_Grid10(grd10_part5);
                grd10_part5.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                grd10_part5.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grd10_part5.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grd10_part5.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                

                grd14_part5.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                BindPart5_Grid14(grd14_part5);
                grd14_part5.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                grd14_part5.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grd14_part5.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grd14_part5.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                grd15_part6.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                BindPart6_Grid15(grd15_part6);
                grd15_part6.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                grd15_part6.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grd15_part6.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grd15_part6.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                grd16_part6.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                BindPart6_Grid16(grd16_part6);
                grd16_part6.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                grd16_part6.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grd16_part6.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grd16_part6.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


                grdComputation.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                BindGrid_Computation(grdComputation);
                grdComputation.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                grdComputation.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grdComputation.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grdComputation.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                
            }
            catch (Exception ex)
            {
                //pbGSTR1.Visible = false;
                MessageBox.Show("Error : " + ex.Message);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTAGSTError.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
                
            }
        }

        private void GetSetGSTPortalData()
        {
            try
            {

                grdPortalSummary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                Bind_GridSummary(grdPortalSummary);
                grdPortalSummary.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                grdPortalSummary.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grdPortalSummary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grdPortalSummary.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                grdPortal4_part2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                BindPart2_Grid4(grdPortal4_part2);
                grdPortal4_part2.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                grdPortal4_part2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grdPortal4_part2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grdPortal4_part2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


                grdPortal5_part2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                BindPart2_Grid5(grdPortal5_part2);
                grdPortal5_part2.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                grdPortal5_part2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grdPortal5_part2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grdPortal5_part2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                grdPortal6_part3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                BindPart3_Grid6(grdPortal6_part3);
                grdPortal6_part3.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                grdPortal6_part3.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grdPortal6_part3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grdPortal6_part3.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                grdPortal7_part3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                BindPart3_Grid7(grdPortal7_part3);
                grdPortal7_part3.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                grdPortal7_part3.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grdPortal7_part3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grdPortal7_part3.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                grdPortal8_part3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                BindPart3_Grid8(grdPortal8_part3);
                grdPortal8_part3.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                grdPortal8_part3.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grdPortal8_part3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grdPortal8_part3.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                grdPortal9_part4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                BindPart4_Grid9(grdPortal9_part4);
                grdPortal9_part4.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                grdPortal9_part4.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grdPortal9_part4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grdPortal9_part4.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                grdPortal10_part5.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                BindPart5_Grid10(grdPortal10_part5);
                grdPortal10_part5.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                grdPortal10_part5.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grdPortal10_part5.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grdPortal10_part5.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


                grdPortal14_part5.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                BindPart5_Grid14(grdPortal14_part5);
                grdPortal14_part5.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                grdPortal14_part5.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grdPortal14_part5.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grdPortal14_part5.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                grdPortal15_part6.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                BindPart6_Grid15(grdPortal15_part6);
                grdPortal15_part6.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                grdPortal15_part6.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grdPortal15_part6.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grdPortal15_part6.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                grdPortal16_part6.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                BindPart6_Grid16(grdPortal16_part6);
                grdPortal16_part6.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                grdPortal16_part6.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grdPortal16_part6.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grdPortal16_part6.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


            }
            catch (Exception ex)
            {
                //pbGSTR1.Visible = false;
                MessageBox.Show("Error : " + ex.Message);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTAGSTError.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();

            }
        }


     

        #region ***** Summary .... 
        private void Bind_GridSummary(DataGridView grd)
        {
            
            if (grd.Name == "grdPortalSummary")
            {

                strQuery = " select header.Fld_SrNo, header.Fld_Description,null as Validate "+
                    " ,cast(Fld_TaxableValue as decimal) TaxableValue,cast(Fld_CGST as decimal) CGST, cast(Fld_SGST as decimal) SGST, cast(Fld_IGST as decimal) as IGST "+
                    " ,cast(Fld_Cess as decimal) CESS,cast((Fld_CGST + Fld_SGST + Fld_IGST + Fld_Cess) as decimal) as TotalGST from tblGSTR9_AllTable_Description as header " +
                    "  left join  " +
                        "  ( " +
                        "    select * from tblGSTR9_Summary where Fld_GSTIN='" + CommonHelper.CompanyGSTN + "' and Fld_FinancialYear='" + CommonHelper.ReturnYear + "'" +
                        "  ) as dtl on header.Fld_SrNo= dtl.Fld_SrNo  " +
                     "  where header.Fld_HeaderGroup='Summary' order by cast(header.Fld_SrNo as integer)";
                grd.Columns[0].DataPropertyName = "Fld_SrNo";
                grd.Columns[1].DataPropertyName = "Fld_Description";
                grd.Columns[2].DataPropertyName = "Validate";
                grd.Columns[3].DataPropertyName = "TaxableValue";
                grd.Columns[4].DataPropertyName = "CGST";
                grd.Columns[5].DataPropertyName = "SGST";
                grd.Columns[6].DataPropertyName = "IGST";
                grd.Columns[7].DataPropertyName = "Cess";
                grd.Columns[8].DataPropertyName = "TotalGST";

                dsgrid = MC.GetValueInDataset(strQuery, "dtSummaryPortal");
                //DataRow[] dr = dsgrid.Tables["dtSummaryPortal"].Select("Fld_SrNo ='9' or Fld_SrNo = '14' or  Fld_SrNo ='15'");
                //DataRow[] dr1 = dsgrid.Tables["dtSummaryPortal"].Select("Fld_SrNo in ('9','14','15')");

                
                grd.DataSource = dsgrid.Tables["dtSummaryPortal"];

                
            }
            else
            {
             
                //strQuery = "select header.Fld_SrNo as a1, header.Fld_Description as a2 ,null as Validate1,null as Fld_TaxableValue1, null as Fld_CGST1,null as Fld_SGST1,null as Fld_IGST1,null as Fld_Cess1 , null as TotalGst1 " +
                //            " from tblGSTR9_AllTable_Description as header where header.Fld_HeaderGroup='Summary' order by cast(header.Fld_SrNo as integer)";
                strQuery = " select header.Fld_SrNo, header.Fld_Description,null as Validate " +
                  " ,cast(Fld_TaxableValue as decimal) TaxableValue,cast(Fld_CGST as decimal) CGST, cast(Fld_SGST as decimal) SGST, cast(Fld_IGST as decimal) as IGST " +
                  " ,cast(Fld_Cess as decimal) CESS,cast((Fld_CGST + Fld_SGST + Fld_IGST + Fld_Cess) as decimal) as TotalGST from tblGSTR9_AllTable_Description as header " +
                  "  left join  " +
                      "  ( " +
                      "    select * from tblGSTR9_Local_Summary where Fld_GSTIN='" + CommonHelper.CompanyGSTN + "' and Fld_FinancialYear='" + CommonHelper.ReturnYear + "'" +
                      "  ) as dtl on header.Fld_SrNo= dtl.Fld_SrNo  " +
                   "  where header.Fld_HeaderGroup='Summary' order by cast(header.Fld_SrNo as integer)";

                grd.Columns[0].DataPropertyName = "Fld_SrNo";
                grd.Columns[1].DataPropertyName = "Fld_Description";
                grd.Columns[2].DataPropertyName = "Validate";
                grd.Columns[3].DataPropertyName = "TaxableValue";
                grd.Columns[4].DataPropertyName = "CGST";
                grd.Columns[5].DataPropertyName = "SGST";
                grd.Columns[6].DataPropertyName = "IGST";
                grd.Columns[7].DataPropertyName = "Cess";
                grd.Columns[8].DataPropertyName = "TotalGST";
                
                dsgrid = MC.GetValueInDataset(strQuery,"dtSummaryLocal");
                grd.DataSource = dsgrid.Tables["dtSummaryLocal"];
            }




            //grdSummary.Rows.Add(16);
           //// grdSummary.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ////grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;
            //grdSummary[0, 0].Value = "4";
            //grdSummary[0, 1].Value = "5";
            //grdSummary[0, 2].Value = "6";
            //grdSummary[0, 3].Value = "7";
            //grdSummary[0, 4].Value = "8";
            //grdSummary[0, 5].Value = "9";
            //grdSummary[0, 6].Value = "10";
            //grdSummary[0, 7].Value = "11";
            //grdSummary[0, 8].Value = "12";
            //grdSummary[0, 9].Value = "13";
            //grdSummary[0, 10].Value = "14";
            //grdSummary[0, 11].Value = "15";
            //grdSummary[0, 12].Value = "16";
            //grdSummary[0, 13].Value = "17";
            //grdSummary[0, 14].Value = "18";
            //grdSummary[0, 15].Value = "19";

            //grdSummary.Rows[0].Cells[1].Value = "Outward Supply & Inward Supply with GST";
            //grdSummary[1, 1].Value = "Outward Supply  without Payment of  GST";
            //grdSummary[1, 2].Value = "ITC Availed During the F.Y.";
            //grdSummary[1, 3].Value = "ITC Reversal & Ineligible ITC for the F.Y.";
            //grdSummary[1, 4].Value = "Other ITC Related Information";
            //grdSummary[1, 5].Value = "Details of Tax Paid";
            //grdSummary[1, 6].Value = "Supply / Tax of Current Year  Declared in Next F.Y. ";
            //grdSummary[1, 7].Value = "Supply / Tax Reduced  in Next F.Y. ";
            //grdSummary[1, 8].Value = "Reversal of ITC of Current Year in Next F.Y.";
            //grdSummary[1, 9].Value = "ITC of Current Year Availed in Next F.Y.";
            //grdSummary[1, 10].Value = "Differential Tax ";
            //grdSummary[1, 11].Value = "Demand & Refund";
            //grdSummary[1, 12].Value = "Composition / Deemed Supply / Goosds on Aproval";
            //grdSummary[1, 13].Value = "HSN Outward";
            //grdSummary[1, 14].Value = "HSN Inward";
            //grdSummary[1, 15].Value = "Late Fees";

            foreach (DataGridViewColumn col in grd.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.NullValue = "-";
                if (col.Index == 0) col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (col.Index > 2)
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = String.Format("N2");
                }
            }
        }

        private void grdSummary_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grdSummary_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                grdSummary_CellDoubleClick(grdSummary, null);
            }
        } 
        #endregion

        #region ***** Part-2 Table 4 .... 
        private void BindPart2_Grid4(DataGridView grd)
        {

            grd.Columns[0].DataPropertyName = "Fld_SrNo";
            grd.Columns[1].DataPropertyName = "Fld_Description";
            grd.Columns[2].DataPropertyName = "Validate";
            grd.Columns[3].DataPropertyName = "TaxableValue";
            grd.Columns[4].DataPropertyName = "CGST";
            grd.Columns[5].DataPropertyName = "SGST";
            grd.Columns[6].DataPropertyName = "IGST";
            grd.Columns[7].DataPropertyName = "CESS";
            if (grd.Name == "grdPortal4_part2")
            {

                strQuery = " select header.Fld_SrNo, header.Fld_Description,null as Validate, " +
                           "  cast(Fld_TaxableValue as decimal) TaxableValue,cast(Fld_CGST as decimal) CGST, cast(Fld_SGST as decimal) SGST," +
                           " cast(Fld_IGST as decimal) as IGST ,cast(Fld_Cess as decimal) CESS  from tblGSTR9_AllTable_Description header " +
                        " Left join " +
                        " ( " +
                        "   select * from tblGSTR9_PtII_PtIII_PtV10_PtVI16 where Fld_HeaderGroup='PtII-4' and Fld_GSTIN='" + CommonHelper.CompanyGSTN + "'" +
                        "   and Fld_FinancialYear='" + CommonHelper.ReturnYear + "'" +
                        " ) as dtl on header.Fld_SrNo=dtl.Fld_SrNo   " +
                        " where header.Fld_HeaderGroup='PtII-4' order by header.Fld_SrNo ";
                dsgrid = MC.GetValueInDataset(strQuery, "dt4part2Portal");
                grd.DataSource = dsgrid.Tables["dt4part2Portal"];
            }
            else
            {
                //strQuery = "select header.Fld_SrNo, header.Fld_Description ,null as Validate,null as TaxableValue, null as CGST,null as SGST,null as IGST,null as CESS " +
                //              " from tblGSTR9_AllTable_Description as header where header.Fld_HeaderGroup='PtII-4' order by header.Fld_SrNo ";
                strQuery = " select header.Fld_SrNo, header.Fld_Description,null as Validate, " +
                          "  cast(Fld_TaxableValue as decimal) TaxableValue,cast(Fld_CGST as decimal) CGST, cast(Fld_SGST as decimal) SGST," +
                          " cast(Fld_IGST as decimal) as IGST ,cast(Fld_Cess as decimal) CESS  from tblGSTR9_AllTable_Description header " +
                       " Left join " +
                       " ( " +
                       "   select * from tblGSTR9_Local_PtII_PtIII_PtV10_PtVI16 where Fld_HeaderGroup='PtII-4' and Fld_GSTIN='" + CommonHelper.CompanyGSTN + "'" +
                       "   and Fld_FinancialYear='" + CommonHelper.ReturnYear + "'" +
                       " ) as dtl on header.Fld_SrNo=dtl.Fld_SrNo   " +
                       " where header.Fld_HeaderGroup='PtII-4' order by header.Fld_SrNo ";
                dsgrid = MC.GetValueInDataset(strQuery, "dt4part2Local");
                grd.DataSource = dsgrid.Tables["dt4part2Local"];
            }
               
            //grd.Rows.Add(14);

            //grd.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //grd.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;


            ////grd.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //////grd.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;

            ////grd.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;


            //grd[0, 0].Value = "A";
            //grd[0, 1].Value = "B";
            //grd[0, 2].Value = "C";
            //grd[0, 3].Value = "D";
            //grd[0, 4].Value = "E";
            //grd[0, 5].Value = "F";
            //grd[0, 6].Value = "G";
            //grd[0, 7].Value = "H";
            //grd[0, 8].Value = "I";
            //grd[0, 9].Value = "J";
            //grd[0, 10].Value = "K";
            //grd[0, 11].Value = "L";
            //grd[0, 12].Value = "M";
            //grd[0, 13].Value = "N";

            //grd.Rows[0].Cells[1].Value = "Supplies made to un-registered persons (B2C)";
            //grd[1, 1].Value = "Supplies made to registered persons (B2B)";
            //grd[1, 2].Value = "Zero rated supply (Export) on payment of tax (except supplies to SEZs)";
            //grd[1, 3].Value = "Supply to SEZs on payment of tax";
            //grd[1, 4].Value = "Deemed Exports";
            //grd[1, 5].Value = "Advances on which tax has been paid but invoice has not been issued (not covered under (A) to (E) above)";
            //grd[1, 6].Value = "Inward supplies on which tax is to be paid on reverse charge basis";
            //grd[1, 7].Value = "Sub-total (A to G above)";
            //grd[1, 8].Value = "Credit Notes issued in respect of transactions specified in (B) to (E) above (-)";
            //grd[1, 9].Value = "Debit Notes issued in respect of transactions specified in (B) to (E) above (+)";
            //grd[1, 10].Value = "Supplies / tax declared through Amendments (+)";
            //grd[1, 11].Value = "Supplies / tax reduced through Amendments (-)";
            //grd[1, 12].Value = "Sub-total (I to L above)";
            //grd[1, 13].Value = "Supplies and advances on which tax is to be paid (H + M) above";

            grd.Rows[7].ReadOnly = true;
            grd.Rows[12].ReadOnly = true;
            grd.Rows[13].ReadOnly = true;
            //grd.Columns[9].ReadOnly = true;
            //grd.Columns[10].ReadOnly = true;

            grd.Rows[7].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            grd.Rows[12].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            grd.Rows[13].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            //grd.Columns[9].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            //grd.Columns[10].DefaultCellStyle.BackColor = System.Drawing.Color.LightPink;

            foreach (DataGridViewColumn col in grd.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.NullValue = "-";
                if (col.Index == 0) col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (col.Index > 2)
                {  
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = String.Format("N2");
                }
            }

        }
        private void grd4_part2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            try
            {

                DataGridView gvr = sender as DataGridView;

                decimal val2 = 0; //Convert.ToDecimal(gvr[2, gvr.CurrentRow.Index].Value.ToString() == "" ? "0" : gvr[2, gvr.CurrentRow.Index].Value.ToString());
                decimal val3 = 0; // Convert.ToDecimal(gvr[3, gvr.CurrentRow.Index].Value.ToString() == "" ? "0" : gvr[3, gvr.CurrentRow.Index].Value.ToString());
                decimal val4 = 0;// Convert.ToDecimal(gvr[4, gvr.CurrentRow.Index].Value.ToString() == "" ? "0" : gvr[4, gvr.CurrentRow.Index].Value.ToString());
                decimal val5 = 0;// Convert.ToDecimal(gvr[5, gvr.CurrentRow.Index].Value.ToString() == "" ? "0" : gvr[5, gvr.CurrentRow.Index].Value.ToString());
                decimal val6 = 0;// Convert.ToDecimal(gvr[6, gvr.CurrentRow.Index].Value.ToString() == "" ? "0" : gvr[6, gvr.CurrentRow.Index].Value.ToString());
                int index = 0;
                foreach (DataGridViewRow gr in gvr.Rows)
                {

                    if (index <= 6)
                    {
                        val2 += Convert.ToDecimal(Convert.ToString(gr.Cells[2].Value) == "" ? "0" : gr.Cells[2].Value.ToString());
                        val3 += Convert.ToDecimal(Convert.ToString(gr.Cells[3].Value) == "" ? "0" : gr.Cells[3].Value.ToString());
                        val4 += Convert.ToDecimal(Convert.ToString(gr.Cells[4].Value) == "" ? "0" : gr.Cells[4].Value.ToString());
                        val5 += Convert.ToDecimal(Convert.ToString(gr.Cells[5].Value) == "" ? "0" : gr.Cells[5].Value.ToString());
                        val6 += Convert.ToDecimal(Convert.ToString(gr.Cells[6].Value) == "" ? "0" : gr.Cells[6].Value.ToString());
                    }

                    if (index == 7)
                    {
                        gr.Cells[2].Value = val2;
                        gr.Cells[3].Value = val3;
                        gr.Cells[4].Value = val4;
                        gr.Cells[5].Value = val5;
                        gr.Cells[6].Value = val6;

                        val2 = 0;
                        val3 = 0;
                        val4 = 0;
                        val5 = 0;
                        val6 = 0;
                    }
                    if (index == 8)
                    {
                        val2 = val2 - Convert.ToDecimal(Convert.ToString(gr.Cells[2].Value) == "" ? "0" : gr.Cells[2].Value.ToString());
                        val3 = val3 - Convert.ToDecimal(Convert.ToString(gr.Cells[3].Value) == "" ? "0" : gr.Cells[3].Value.ToString());
                        val4 = val4 - Convert.ToDecimal(Convert.ToString(gr.Cells[4].Value) == "" ? "0" : gr.Cells[4].Value.ToString());
                        val5 = val5 - Convert.ToDecimal(Convert.ToString(gr.Cells[5].Value) == "" ? "0" : gr.Cells[5].Value.ToString());
                        val6 = val6 - Convert.ToDecimal(Convert.ToString(gr.Cells[6].Value) == "" ? "0" : gr.Cells[6].Value.ToString());

                    }

                    if (index == 9)
                    {
                        val2 += Convert.ToDecimal(Convert.ToString(gr.Cells[2].Value) == "" ? "0" : gr.Cells[2].Value.ToString());
                        val3 += Convert.ToDecimal(Convert.ToString(gr.Cells[3].Value) == "" ? "0" : gr.Cells[3].Value.ToString());
                        val4 += Convert.ToDecimal(Convert.ToString(gr.Cells[4].Value) == "" ? "0" : gr.Cells[4].Value.ToString());
                        val5 += Convert.ToDecimal(Convert.ToString(gr.Cells[5].Value) == "" ? "0" : gr.Cells[5].Value.ToString());
                        val6 += Convert.ToDecimal(Convert.ToString(gr.Cells[6].Value) == "" ? "0" : gr.Cells[6].Value.ToString());

                    }
                    if (index == 10)
                    {
                        val2 += Convert.ToDecimal(Convert.ToString(gr.Cells[2].Value) == "" ? "0" : gr.Cells[2].Value.ToString());
                        val3 += Convert.ToDecimal(Convert.ToString(gr.Cells[3].Value) == "" ? "0" : gr.Cells[3].Value.ToString());
                        val4 += Convert.ToDecimal(Convert.ToString(gr.Cells[4].Value) == "" ? "0" : gr.Cells[4].Value.ToString());
                        val5 += Convert.ToDecimal(Convert.ToString(gr.Cells[5].Value) == "" ? "0" : gr.Cells[5].Value.ToString());
                        val6 += Convert.ToDecimal(Convert.ToString(gr.Cells[6].Value) == "" ? "0" : gr.Cells[6].Value.ToString());

                    }
                    if (index == 11)
                    {
                        val2 = val2 - Convert.ToDecimal(Convert.ToString(gr.Cells[2].Value) == "" ? "0" : gr.Cells[2].Value.ToString());
                        val3 = val3 - Convert.ToDecimal(Convert.ToString(gr.Cells[3].Value) == "" ? "0" : gr.Cells[3].Value.ToString());
                        val4 = val4 - Convert.ToDecimal(Convert.ToString(gr.Cells[4].Value) == "" ? "0" : gr.Cells[4].Value.ToString());
                        val5 = val5 - Convert.ToDecimal(Convert.ToString(gr.Cells[5].Value) == "" ? "0" : gr.Cells[5].Value.ToString());
                        val6 = val6 - Convert.ToDecimal(Convert.ToString(gr.Cells[6].Value) == "" ? "0" : gr.Cells[6].Value.ToString());

                    }
                    if (index == 12)
                    {
                        gr.Cells[2].Value = val2;
                        gr.Cells[3].Value = val3;
                        gr.Cells[4].Value = val4;
                        gr.Cells[5].Value = val5;
                        gr.Cells[6].Value = val6;

                    }
                    if (index == 13)
                    {
                        gr.Cells[2].Value = Convert.ToDecimal(Convert.ToString(gvr[2, 7].Value) == "" ? "0" : gvr[2, 7].Value.ToString()) + val2;
                        gr.Cells[3].Value = Convert.ToDecimal(Convert.ToString(gvr[3, 7].Value) == "" ? "0" : gvr[3, 7].Value.ToString()) + val3;
                        gr.Cells[4].Value = Convert.ToDecimal(Convert.ToString(gvr[4, 7].Value) == "" ? "0" : gvr[4, 7].Value.ToString()) + val4;
                        gr.Cells[5].Value = Convert.ToDecimal(Convert.ToString(gvr[5, 7].Value) == "" ? "0" : gvr[5, 7].Value.ToString()) + val5;
                        gr.Cells[6].Value = Convert.ToDecimal(Convert.ToString(gvr[6, 7].Value) == "" ? "0" : gvr[6, 7].Value.ToString()) + val6;

                    }

                    index++;
                }

                Update_LinkedTable_Value(1, 1, 1, 1, 1);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }

        }
        private void grd4_part2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView grd = new DataGridView();
            grd = grd4_part2;
            // e.Control.KeyPress -= new KeyPressEventHandler(Check);
            if (grd.CurrentCell.ColumnIndex > 1)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Check);
                }
            }
        }

        #endregion

        #region ***** Part-2 Table 5 ....

        private void BindPart2_Grid5(DataGridView grd)
        {
            
            grd.Columns[0].DataPropertyName = "Fld_SrNo";
            grd.Columns[1].DataPropertyName = "Fld_Description";
            grd.Columns[2].DataPropertyName = "Validate";
            grd.Columns[3].DataPropertyName = "TaxableValue";
            grd.Columns[4].DataPropertyName = "CGST";
            grd.Columns[5].DataPropertyName = "SGST";
            grd.Columns[6].DataPropertyName = "IGST";
            grd.Columns[7].DataPropertyName = "CESS";
            if (grd.Name == "grdPortal5_part2")
            {

                strQuery = " select header.Fld_SrNo, header.Fld_Description,null as Validate, " +
                           "  cast(Fld_TaxableValue as decimal) TaxableValue,cast(Fld_CGST as decimal) CGST, cast(Fld_SGST as decimal) SGST," +
                           " cast(Fld_IGST as decimal) as IGST ,cast(Fld_Cess as decimal) CESS  from tblGSTR9_AllTable_Description header " +
                        " Left join " +
                        " ( " +
                        "   select * from tblGSTR9_PtII_PtIII_PtV10_PtVI16 where Fld_HeaderGroup='PtII-5' and Fld_GSTIN='" + CommonHelper.CompanyGSTN + "'" +
                        "   and Fld_FinancialYear='" + CommonHelper.ReturnYear + "'" +
                        " ) as dtl on header.Fld_SrNo=dtl.Fld_SrNo   " +
                        " where header.Fld_HeaderGroup='PtII-5' order by header.Fld_SrNo ";
                dsgrid = MC.GetValueInDataset(strQuery, "dt5part2Portal");
                grd.DataSource = dsgrid.Tables["dt5part2Portal"];
            }
            else
            {
                //strQuery = "select header.Fld_SrNo, header.Fld_Description ,null as Validate,null as TaxableValue, null as CGST,null as SGST,null as IGST,null as CESS " +
                //              " from tblGSTR9_AllTable_Description as header where header.Fld_HeaderGroup='PtII-4' order by header.Fld_SrNo ";
                strQuery = " select header.Fld_SrNo, header.Fld_Description,null as Validate, " +
                          "  cast(Fld_TaxableValue as decimal) TaxableValue,cast(Fld_CGST as decimal) CGST, cast(Fld_SGST as decimal) SGST," +
                          " cast(Fld_IGST as decimal) as IGST ,cast(Fld_Cess as decimal) CESS  from tblGSTR9_AllTable_Description header " +
                       " Left join " +
                       " ( " +
                       "   select * from tblGSTR9_Local_PtII_PtIII_PtV10_PtVI16 where Fld_HeaderGroup='PtII-5' and Fld_GSTIN='" + CommonHelper.CompanyGSTN + "'" +
                       "   and Fld_FinancialYear='" + CommonHelper.ReturnYear + "'" +
                       " ) as dtl on header.Fld_SrNo=dtl.Fld_SrNo   " +
                       " where header.Fld_HeaderGroup='PtII-5' order by header.Fld_SrNo ";
                dsgrid = MC.GetValueInDataset(strQuery, "dt5part2Local");
                grd.DataSource = dsgrid.Tables["dt5part2Local"];
            }
            //grd.Rows.Add(14);
            ////grd.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //////grd.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;

            ////grd.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ////grd.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;

            //grd[0, 0].Value = "A";
            //grd[0, 1].Value = "B";
            //grd[0, 2].Value = "C";
            //grd[0, 3].Value = "D";
            //grd[0, 4].Value = "E";
            //grd[0, 5].Value = "F";
            //grd[0, 6].Value = "G";
            //grd[0, 7].Value = "H";
            //grd5_part2[0, 8].Value = "I";
            //grd[0, 9].Value = "J";
            //grd[0, 10].Value = "K";
            //grd[0, 11].Value = "L";
            //grd[0, 12].Value = "M";
            //grd[0, 13].Value = "N";

            //grd.Rows[0].Cells[1].Value = "Zero rated supply (Export) without payment of tax";
            //grd[1, 1].Value = "Supply to SEZs without payment of tax";
            //grd[1, 2].Value = "Supplies on which tax is to be paid by the recipient on reverse charge basis";
            //grd[1, 3].Value = "Exempted";
            //grd[1, 4].Value = "Nil Rated";
            //grd[1, 5].Value = "Non-GST supply";
            //grd[1, 6].Value = "Sub-total (A to F above)";
            //grd[1, 7].Value = "Credit Notes issued in respect of transactions specified in A to F above (-)";
            //grd[1, 8].Value = "Debit Notes issued in respect of transactions specified in A to F above (+)";
            //grd[1, 9].Value = "Supplies declared through Amendments (+)";
            //grd[1, 10].Value = "Supplies reduced through Amendments (-)";
            //grd[1, 11].Value = "Sub-Total (H to K above)";
            //grd[1, 12].Value = "Turnover on which tax is not to be paid (G + L above)";
            //grd[1, 13].Value = "Total Turnover (including advances)(4N + 5M - 4G above)";


            grd.Rows[6].ReadOnly = true;
            grd.Rows[11].ReadOnly = true;
            grd.Rows[12].ReadOnly = true;
            grd.Rows[13].ReadOnly = true;
            grd.Rows[6].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            grd.Rows[11].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            grd.Rows[12].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            grd.Rows[13].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;

            //grd5_part2.Columns[3].ReadOnly = true;
            //grd5_part2.Columns[4].ReadOnly = true;
            //grd5_part2.Columns[5].ReadOnly = true;
            //grd5_part2.Columns[6].ReadOnly = true;

            grd.Columns[3].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            grd.Columns[4].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            grd.Columns[5].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            grd.Columns[6].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;

            foreach (DataGridViewRow row in grd.Rows)
            {
                grd[3, row.Index].Style.BackColor = System.Drawing.Color.LightGray;
                grd[4, row.Index].Style.BackColor = System.Drawing.Color.LightGray;
                grd[5, row.Index].Style.BackColor = System.Drawing.Color.LightGray;
                grd[6, row.Index].Style.BackColor = System.Drawing.Color.LightGray;
            }

            foreach (DataGridViewColumn col in grd.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.NullValue = "-";
                if (col.Index == 0) col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (col.Index > 2)
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = String.Format("N2");
                }
            }
        }
        private void grd5_part2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                DataGridView gvr = sender as DataGridView;

                decimal val2 = 0; //Convert.ToDecimal(gvr[2, gvr.CurrentRow.Index].Value.ToString() == "" ? "0" : gvr[2, gvr.CurrentRow.Index].Value.ToString());
                decimal val3 = 0; // Convert.ToDecimal(gvr[3, gvr.CurrentRow.Index].Value.ToString() == "" ? "0" : gvr[3, gvr.CurrentRow.Index].Value.ToString());
                decimal val4 = 0;// Convert.ToDecimal(gvr[4, gvr.CurrentRow.Index].Value.ToString() == "" ? "0" : gvr[4, gvr.CurrentRow.Index].Value.ToString());
                decimal val5 = 0;// Convert.ToDecimal(gvr[5, gvr.CurrentRow.Index].Value.ToString() == "" ? "0" : gvr[5, gvr.CurrentRow.Index].Value.ToString());
                decimal val6 = 0;// Convert.ToDecimal(gvr[6, gvr.CurrentRow.Index].Value.ToString() == "" ? "0" : gvr[6, gvr.CurrentRow.Index].Value.ToString());
                int index = 0;
                foreach (DataGridViewRow gr in gvr.Rows)
                {

                    if (index <= 5)
                    {
                        val2 += Convert.ToDecimal(Convert.ToString(gr.Cells[2].Value) == "" ? "0" : gr.Cells[2].Value.ToString());

                    }

                    if (index == 6)
                    {
                        gr.Cells[2].Value = val2;
                        val2 = 0;

                    }
                    if (index == 7)
                    {
                        val2 = val2 - Convert.ToDecimal(Convert.ToString(gr.Cells[2].Value) == "" ? "0" : gr.Cells[2].Value.ToString());
                    }

                    if (index == 8)
                    {
                        val2 += Convert.ToDecimal(Convert.ToString(gr.Cells[2].Value) == "" ? "0" : gr.Cells[2].Value.ToString());
                    }
                    if (index == 9)
                    {
                        val2 += Convert.ToDecimal(Convert.ToString(gr.Cells[2].Value) == "" ? "0" : gr.Cells[2].Value.ToString());
                    }
                    if (index == 10)
                    {
                        val2 = val2 - Convert.ToDecimal(Convert.ToString(gr.Cells[2].Value) == "" ? "0" : gr.Cells[2].Value.ToString());
                    }
                    if (index == 11)
                    {
                        gr.Cells[2].Value = val2;

                    }
                    if (index == 12)
                    {
                        gr.Cells[2].Value = Convert.ToDecimal(Convert.ToString(gvr[2, 6].Value) == "" ? "0" : gvr[2, 6].Value.ToString()) + val2;

                    }

                    if (index == 13)
                    {

                        gr.Cells[2].Value = Convert.ToDecimal(Convert.ToString(grd4_part2[2, 13].Value) == "" ? "0" : grd4_part2[2, 13].Value.ToString())
                                          + Convert.ToDecimal(Convert.ToString(grd5_part2[2, 12].Value) == "" ? "0" : grd5_part2[2, 12].Value.ToString())
                                          - Convert.ToDecimal(Convert.ToString(grd4_part2[2, 6].Value) == "" ? "0" : grd4_part2[2, 6].Value.ToString());

                        gr.Cells[3].Value = Convert.ToDecimal(Convert.ToString(grd4_part2[3, 13].Value) == "" ? "0" : grd4_part2[3, 13].Value.ToString())
                                        + Convert.ToDecimal(Convert.ToString(grd5_part2[3, 12].Value) == "" ? "0" : grd5_part2[3, 12].Value.ToString())
                                        - Convert.ToDecimal(Convert.ToString(grd4_part2[3, 6].Value) == "" ? "0" : grd4_part2[3, 6].Value.ToString());

                        gr.Cells[4].Value = Convert.ToDecimal(Convert.ToString(grd4_part2[4, 13].Value) == "" ? "0" : grd4_part2[4, 13].Value.ToString())
                                        + Convert.ToDecimal(Convert.ToString(grd5_part2[4, 12].Value) == "" ? "0" : grd5_part2[4, 12].Value.ToString())
                                        - Convert.ToDecimal(Convert.ToString(grd4_part2[4, 6].Value) == "" ? "0" : grd4_part2[4, 6].Value.ToString());

                        gr.Cells[5].Value = Convert.ToDecimal(Convert.ToString(grd4_part2[5, 13].Value) == "" ? "0" : grd4_part2[5, 13].Value.ToString())
                                        + Convert.ToDecimal(Convert.ToString(grd5_part2[5, 12].Value) == "" ? "0" : grd5_part2[5, 12].Value.ToString())
                                        - Convert.ToDecimal(Convert.ToString(grd4_part2[5, 6].Value) == "" ? "0" : grd4_part2[5, 6].Value.ToString());

                        gr.Cells[6].Value = Convert.ToDecimal(Convert.ToString(grd4_part2[6, 13].Value) == "" ? "0" : grd4_part2[6, 13].Value.ToString())
                                        + Convert.ToDecimal(Convert.ToString(grd5_part2[6, 12].Value) == "" ? "0" : grd5_part2[6, 12].Value.ToString())
                                        - Convert.ToDecimal(Convert.ToString(grd4_part2[6, 6].Value) == "" ? "0" : grd4_part2[6, 6].Value.ToString());
                    }

                    index++;
                }

                Update_LinkedTable_Value(0, 1, 1, 1, 1);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }
        }
        private void grd5_part2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView grd = new DataGridView();
            grd = grd5_part2;
            // e.Control.KeyPress -= new KeyPressEventHandler(Check);
            if (grd.CurrentCell.ColumnIndex > 1)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Check);
                }
            }
        }

        #endregion

        #region ***** Part-3 Table 6 ....

        private void BindPart3_Grid6(DataGridView grd)
        {
            
            
            grd.Columns[0].DataPropertyName = "Fld_SrNo";
            grd.Columns[1].DataPropertyName = "Fld_Description";
            grd.Columns[2].DataPropertyName = "Type";
            grd.Columns[3].DataPropertyName = "CGST";
            grd.Columns[4].DataPropertyName = "SGST";
            grd.Columns[5].DataPropertyName = "IGST";
            grd.Columns[6].DataPropertyName = "CESS";
            if (grd.Name == "grdPortal6_part3")
            {

                strQuery = " select header.Fld_SrNo, header.Fld_Description,header.Fld_Type as Type, " +
                           " cast(Fld_CGST as decimal) CGST, cast(Fld_SGST as decimal) SGST," +
                           " cast(Fld_IGST as decimal) as IGST ,cast(Fld_Cess as decimal) CESS  from tblGSTR9_AllTable_Description header " +
                        " Left join " +
                        " ( " +
                        "   select * from tblGSTR9_PtII_PtIII_PtV10_PtVI16 where Fld_HeaderGroup='PtIII-6' and Fld_GSTIN='" + CommonHelper.CompanyGSTN + "'" +
                        "   and Fld_FinancialYear='" + CommonHelper.ReturnYear + "'" +
                        " ) as dtl on header.Fld_SrNo=dtl.Fld_SrNo  and header.Fld_Type=dtl.Fld_Type " +
                        " where header.Fld_HeaderGroup='PtIII-6' order by header.Fld_SrNo ";
                dsgrid = MC.GetValueInDataset(strQuery, "dt6part3Portal");
                grd.DataSource = dsgrid.Tables["dt6part3Portal"];
            }
            else
            {
                //strQuery = "select header.Fld_SrNo, header.Fld_Description ,null as Validate,null as TaxableValue, null as CGST,null as SGST,null as IGST,null as CESS " +
                //              " from tblGSTR9_AllTable_Description as header where header.Fld_HeaderGroup='PtII-4' order by header.Fld_SrNo ";
                strQuery = " select header.Fld_SrNo, header.Fld_Description,header.Fld_Type as Type, " +
                          "  cast(Fld_CGST as decimal) CGST, cast(Fld_SGST as decimal) SGST," +
                          " cast(Fld_IGST as decimal) as IGST ,cast(Fld_Cess as decimal) CESS  from tblGSTR9_AllTable_Description header " +
                       " Left join " +
                       " ( " +
                       "   select * from tblGSTR9_Local_PtII_PtIII_PtV10_PtVI16 where Fld_HeaderGroup='PtIII-6' and Fld_GSTIN='" + CommonHelper.CompanyGSTN + "'" +
                       "   and Fld_FinancialYear='" + CommonHelper.ReturnYear + "'" +
                       " ) as dtl on header.Fld_SrNo=dtl.Fld_SrNo   " +
                       " where header.Fld_HeaderGroup='PtIII-6' order by header.Fld_SrNo ";
                dsgrid = MC.GetValueInDataset(strQuery, "dt6part3Local");
                grd.DataSource = dsgrid.Tables["dt6part3Local"];
            }
            //grd.Rows.Add(22);
            ////grd.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //////grd.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;

            ////grd.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //////grd_4.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;

            //grd[0, 0].Value = "A";
            //grd[0, 1].Value = "B";
            //grd[0, 2].Value = "B-2";
            //grd[0, 3].Value = "B-3";
            //grd[0, 4].Value = "C";
            //grd[0, 5].Value = "C-2";
            //grd[0, 6].Value = "C-3";
            //grd[0, 7].Value = "D";
            //grd[0, 8].Value = "D-2";
            //grd[0, 9].Value = "D-3";
            //grd[0, 10].Value = "E";
            //grd[0, 11].Value = "E-2";
            //grd[0, 12].Value = "F";
            //grd[0, 13].Value = "G";
            //grd[0, 14].Value = "H";
            //grd[0, 15].Value = "I";
            //grd[0, 16].Value = "J";
            //grd[0, 17].Value = "K";
            //grd[0, 18].Value = "L";
            //grd[0, 19].Value = "M";
            //grd[0, 20].Value = "N";
            //grd[0, 21].Value = "O";

            //grd.Rows[0].Cells[1].Value = "Total amount of input tax credit availed through FORM GSTR-3B (Sum total of table 4A of FORM GSTR-3B)";

            //grd[1, 1].Value = "Inward supplies (other than imports and inward supplies liable to reverse charge but includes services received from SEZs)";
            //grd[1, 2].Value = "Inward supplies (other than imports and inward supplies liable to reverse charge but includes services received from SEZs)";
            //grd[1, 3].Value = "Inward supplies (other than imports and inward supplies liable to reverse charge but includes services received from SEZs)";

            //grd[1, 4].Value = "Inward supplies received from unregistered persons liable to reverse charge  (other than B above) on which tax is paid & ITC availed";
            //grd[1, 5].Value = "Inward supplies received from unregistered persons liable to reverse charge  (other than B above) on which tax is paid & ITC availed";
            //grd[1, 6].Value = "Inward supplies received from unregistered persons liable to reverse charge  (other than B above) on which tax is paid & ITC availed";

            //grd[1, 7].Value = "Inward supplies received from registered persons liable to reverse charge (other than B above) on which tax is paid and ITC availed";
            //grd[1, 8].Value = "Inward supplies received from registered persons liable to reverse charge (other than B above) on which tax is paid and ITC availed";
            //grd[1, 9].Value = "Inward supplies received from registered persons liable to reverse charge (other than B above) on which tax is paid and ITC availed";

            //grd[1, 10].Value = "Import of goods (including supplies from SEZ)";
            //grd[1, 11].Value = "Import of goods (including supplies from SEZ)";

            //grd[1, 12].Value = "Import of services (excluding inward supplies from SEZs)";
            //grd[1, 13].Value = "Input Tax  credit received from ISD";

            //grd[1, 14].Value = "Amount of ITC reclaimed (other than B above) under the provisions of the Act";
            //grd[1, 15].Value = "Sub-total (B to H above)";
            //grd[1, 16].Value = "Difference (I - A) above";
            //grd[1, 17].Value = "Transition Credit through TRAN-1 (including revisions if any)";
            //grd[1, 18].Value = "Transition Credit through TRAN-2";
            //grd[1, 19].Value = "Any other ITC availed but not specified above";
            //grd[1, 20].Value = "Sub-total (K to M above)";
            //grd[1, 21].Value = "Total ITC availed (I + N) above";


            //grd[2, 0].Value = "";
            //grd[2, 1].Value = "Inputs";
            //grd[2, 2].Value = "Capital Goods";
            //grd[2, 3].Value = "Input Services";
            //grd[2, 4].Value = "Inputs";
            //grd[2, 5].Value = "Capital Goods";
            //grd[2, 6].Value = "Input Services";
            //grd[2, 7].Value = "Inputs";
            //grd[2, 8].Value = "Capital Goods";
            //grd[2, 9].Value = "Input Services";
            //grd[2, 10].Value = "Inputs";
            //grd[2, 11].Value = "Capital Goods";
            //grd[2, 12].Value = "";
            //grd[2, 13].Value = "";
            //grd[2, 14].Value = "";
            //grd[2, 15].Value = "";
            //grd[2, 16].Value = "";
            //grd[2, 17].Value = "";
            //grd[2, 18].Value = "";
            //grd[2, 19].Value = "";
            //grd[2, 20].Value = "";
            //grd[2, 21].Value = "";

            //int index = 0;
            //foreach (DataGridViewRow gvr in grd.Rows)
            //{
            //    if (index > 3)
            //    {
            //        gvr.Cells[3].ReadOnly = true;
            //        gvr.Cells[3].Style.BackColor = System.Drawing.Color.LightGray;
            //    }
            //    index++;   
            //}


            //grd.Rows[0].ReadOnly = true;
            //grd.Rows[11].ReadOnly = true;
            //grd.Rows[12].ReadOnly = true;
            //grd.Rows[13].ReadOnly = true;
            //grd.Rows[0].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            //grd.Rows[11].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            //grd.Rows[12].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            //grd.Rows[13].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;

            //grd5_part2.Columns[3].ReadOnly = true;
            //grd5_part2.Columns[4].ReadOnly = true;
            //grd5_part2.Columns[5].ReadOnly = true;
            //grd5_part2.Columns[6].ReadOnly = true;

            //grd[2, 2].Style.BackColor = System.Drawing.Color.LightGray;
            //grd[2, 3].Style.BackColor = System.Drawing.Color.LightGray;
            //grd.Columns[3].DefaultCellStyle .BackColor = System.Drawing.Color.LightGray;
            //grd.Columns[4].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            //grd.Columns[5].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            //grd.Columns[6].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;

           // grd.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            //grd.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //grd.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;


            grd[3, 0].ReadOnly = true;
            grd[4, 0].ReadOnly = true;
            grd[5, 0].ReadOnly = true;
            grd[6, 0].ReadOnly = true;
            grd[3, 10].ReadOnly = true;
            grd[3, 11].ReadOnly = true;
            grd[3, 12].ReadOnly = true;
            grd[4, 10].ReadOnly = true;
            grd[4, 11].ReadOnly = true;
            grd[4, 12].ReadOnly = true;
            grd.Rows[15].ReadOnly = true;
            grd.Rows[16].ReadOnly = true;

            grd[5, 17].ReadOnly = true;
            grd[5, 18].ReadOnly = true;
            grd[6, 17].ReadOnly = true;
            grd[6, 18].ReadOnly = true;

            grd.Rows[20].ReadOnly = true;
            grd.Rows[21].ReadOnly = true;

            grd[3, 0].Style.BackColor = System.Drawing.Color.LightGray;
            grd[4, 0].Style.BackColor = System.Drawing.Color.LightGray;
            grd[5, 0].Style.BackColor = System.Drawing.Color.LightGray;
            grd[6, 0].Style.BackColor = System.Drawing.Color.LightGray;

            grd[3, 10].Style.BackColor = System.Drawing.Color.LightGray;
            grd[3, 11].Style.BackColor = System.Drawing.Color.LightGray;
            grd[3, 12].Style.BackColor = System.Drawing.Color.LightGray;

            grd[4, 10].Style.BackColor = System.Drawing.Color.LightGray;
            grd[4, 11].Style.BackColor = System.Drawing.Color.LightGray;
            grd[4, 12].Style.BackColor = System.Drawing.Color.LightGray;

            grd.Rows[15].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            grd.Rows[16].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;

            grd[5, 17].Style.BackColor = System.Drawing.Color.LightGray;
            grd[5, 18].Style.BackColor = System.Drawing.Color.LightGray;

            grd[6, 17].Style.BackColor = System.Drawing.Color.LightGray;
            grd[6, 18].Style.BackColor = System.Drawing.Color.LightGray;

            grd.Rows[20].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            grd.Rows[21].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;


            foreach (DataGridViewColumn col in grd.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.NullValue = "-";
                if (col.Index == 0) col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (col.Index > 2)
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = String.Format("N2");
                }
            }

            //var cell = (DataGridViewTextBoxCellPro)grd[0, 0];
            //cell.ColumnSpan = 3;
            //cell.RowSpan = 2;
            //grd.Rows[1].DividerHeight = 2;
            //string abc=grd.Columns .CellType.ToString();

            DataGridViewTextBoxColumnPro colpro1 = new DataGridViewTextBoxColumnPro();
            colpro1.HeaderText = grd.Columns[0].HeaderText;
            colpro1.Name = grd.Columns[0].Name;
            colpro1.DataPropertyName = grd.Columns[0].DataPropertyName;
            grd.Columns.RemoveAt(0);
            grd.Columns.Insert(0, colpro1);
            ((DataGridViewTextBoxCellPro)grd.Rows[1].Cells[0]).RowSpan = 3;

            //DataGridViewTextBoxColumnPro colpro2 = new DataGridViewTextBoxColumnPro();
            //colpro2.HeaderText = grd.Columns[1].HeaderText;
            //colpro2.Name = grd.Columns[1].Name;
            //colpro2.DataPropertyName = grd.Columns[1].DataPropertyName;
            //grd.Columns.RemoveAt(1);
            //grd.Columns.Insert(1, colpro2);
            //((DataGridViewTextBoxCellPro)grd[1, 4]).RowSpan = 3;
            //((DataGridViewTextBoxCellPro)grd[0, 7]).RowSpan = 3;
            //((DataGridViewTextBoxCellPro)grd[0, 10]).RowSpan = 2;

            //((DataGridViewTextBoxCellPro)grd[1, 1]).RowSpan = 3;
            //((DataGridViewTextBoxCellPro)grd[1, 4]).RowSpan = 3;
            //((DataGridViewTextBoxCellPro)grd[1, 7]).RowSpan = 3;
            //((DataGridViewTextBoxCellPro)grd[1, 10]).RowSpan = 2;

        }

        private void grd6_part3_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            try
            {

                DataGridView gvr = sender as DataGridView;


                decimal val3 = 0; // Convert.ToDecimal(gvr[3, gvr.CurrentRow.Index].Value.ToString() == "" ? "0" : gvr[3, gvr.CurrentRow.Index].Value.ToString());
                decimal val4 = 0;// Convert.ToDecimal(gvr[4, gvr.CurrentRow.Index].Value.ToString() == "" ? "0" : gvr[4, gvr.CurrentRow.Index].Value.ToString());
                decimal val5 = 0;// Convert.ToDecimal(gvr[5, gvr.CurrentRow.Index].Value.ToString() == "" ? "0" : gvr[5, gvr.CurrentRow.Index].Value.ToString());
                decimal val6 = 0;// Convert.ToDecimal(gvr[6, gvr.CurrentRow.Index].Value.ToString() == "" ? "0" : gvr[6, gvr.CurrentRow.Index].Value.ToString());
                int index = 0;
                foreach (DataGridViewRow gr in gvr.Rows)
                {

                    if (index > 0 && index <= 14)
                    {

                        val3 += Convert.ToDecimal(Convert.ToString(gr.Cells[3].Value) == "" ? "0" : gr.Cells[3].Value.ToString());
                        val4 += Convert.ToDecimal(Convert.ToString(gr.Cells[4].Value) == "" ? "0" : gr.Cells[4].Value.ToString());
                        val5 += Convert.ToDecimal(Convert.ToString(gr.Cells[5].Value) == "" ? "0" : gr.Cells[5].Value.ToString());
                        val6 += Convert.ToDecimal(Convert.ToString(gr.Cells[6].Value) == "" ? "0" : gr.Cells[6].Value.ToString());
                    }

                    if (index == 15)
                    {

                        gr.Cells[3].Value = val3;
                        gr.Cells[4].Value = val4;
                        gr.Cells[5].Value = val5;
                        gr.Cells[6].Value = val6;


                    }

                    if (index == 16)
                    {

                        gr.Cells[3].Value = val3 - Convert.ToDecimal(Convert.ToString(gvr[3, 0].Value) == "" ? "0" : gvr[3, 0].Value.ToString());
                        gr.Cells[4].Value = val4 - Convert.ToDecimal(Convert.ToString(gvr[4, 0].Value) == "" ? "0" : gvr[4, 0].Value.ToString());
                        gr.Cells[5].Value = val5 - Convert.ToDecimal(Convert.ToString(gvr[5, 0].Value) == "" ? "0" : gvr[5, 0].Value.ToString());
                        gr.Cells[6].Value = val6 - Convert.ToDecimal(Convert.ToString(gvr[6, 0].Value) == "" ? "0" : gvr[6, 0].Value.ToString());


                        val3 = 0;
                        val4 = 0;
                        val5 = 0;
                        val6 = 0;
                    }

                    if (index >= 17 && index <= 19)
                    {

                        val3 = val3 + Convert.ToDecimal(Convert.ToString(gr.Cells[3].Value) == "" ? "0" : gr.Cells[3].Value.ToString());
                        val4 = val4 + Convert.ToDecimal(Convert.ToString(gr.Cells[4].Value) == "" ? "0" : gr.Cells[4].Value.ToString());
                        val5 = val5 + Convert.ToDecimal(Convert.ToString(gr.Cells[5].Value) == "" ? "0" : gr.Cells[5].Value.ToString());
                        val6 = val6 + Convert.ToDecimal(Convert.ToString(gr.Cells[6].Value) == "" ? "0" : gr.Cells[6].Value.ToString());

                    }

                    if (index == 20)
                    {
                        //gr.Cells[2].Value = val2;
                        gr.Cells[3].Value = val3;
                        gr.Cells[4].Value = val4;
                        gr.Cells[5].Value = val5;
                        gr.Cells[6].Value = val6;

                    }
                    if (index == 21)
                    {

                        gr.Cells[3].Value = Convert.ToDecimal(Convert.ToString(gvr[3, 15].Value) == "" ? "0" : gvr[3, 15].Value.ToString()) + val3;
                        gr.Cells[4].Value = Convert.ToDecimal(Convert.ToString(gvr[4, 15].Value) == "" ? "0" : gvr[4, 15].Value.ToString()) + val4;
                        gr.Cells[5].Value = Convert.ToDecimal(Convert.ToString(gvr[5, 15].Value) == "" ? "0" : gvr[5, 15].Value.ToString()) + val5;
                        gr.Cells[6].Value = Convert.ToDecimal(Convert.ToString(gvr[6, 15].Value) == "" ? "0" : gvr[6, 15].Value.ToString()) + val6;

                    }


                    index++;

                }
                Update_LinkedTable_Value(0, 0, 1, 1, 1);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }

        }

        private void grd6_part3_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView grd = new DataGridView();
            grd = grd6_part3;
            // e.Control.KeyPress -= new KeyPressEventHandler(Check);
            if (grd.CurrentCell.ColumnIndex > 1)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Check);
                }
            }
        }

        private void grd6_part3_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //if (e.RowIndex == 0 && e.ColumnIndex == 0)
            //{
            //    e.CellStyle.BackColor = Color.Azure;
            //    e.CellStyle.ForeColor = Color.Red;
            //}
        }

        private void grd6_part3_CurrentCellChanged(object sender, EventArgs e)
        {
            // var cell = grd6_part3.CurrentCell as DataGridViewTextBoxCellPro;

        }

        #endregion

        #region ***** Part-3 Table 7 ....

        private void BindPart3_Grid7(DataGridView grd)
        {
            
            grd.Columns[0].DataPropertyName = "Fld_SrNo";
            grd.Columns[1].DataPropertyName = "Fld_Description";
            grd.Columns[2].DataPropertyName = "CGST";
            grd.Columns[3].DataPropertyName = "SGST";
            grd.Columns[4].DataPropertyName = "IGST";
            grd.Columns[5].DataPropertyName = "CESS";
            if (grd.Name == "grdPortal7_part3")
            {

                strQuery = " select header.Fld_SrNo, header.Fld_Description, " +
                           " cast(Fld_CGST as decimal) CGST, cast(Fld_SGST as decimal) SGST," +
                           " cast(Fld_IGST as decimal) as IGST ,cast(Fld_Cess as decimal) CESS  from tblGSTR9_AllTable_Description header " +
                        " Left join " +
                        " ( " +
                        "   select * from tblGSTR9_PtII_PtIII_PtV10_PtVI16 where Fld_HeaderGroup='PtIII-7' and Fld_GSTIN='" + CommonHelper.CompanyGSTN + "'" +
                        "   and Fld_FinancialYear='" + CommonHelper.ReturnYear + "'" +
                        " ) as dtl on header.Fld_SrNo=dtl.Fld_SrNo   " +
                        " where header.Fld_HeaderGroup='PtIII-7' order by header.Fld_SrNo ";
                dsgrid = MC.GetValueInDataset(strQuery, "dt7part3Portal");
                grd.DataSource = dsgrid.Tables["dt7part3Portal"];
            }
            else
            {
                //strQuery = "select header.Fld_SrNo, header.Fld_Description ,null as Validate,null as TaxableValue, null as CGST,null as SGST,null as IGST,null as CESS " +
                //              " from tblGSTR9_AllTable_Description as header where header.Fld_HeaderGroup='PtII-4' order by header.Fld_SrNo ";
                strQuery = " select header.Fld_SrNo, header.Fld_Description, " +
                          "  cast(Fld_CGST as decimal) CGST, cast(Fld_SGST as decimal) SGST," +
                          " cast(Fld_IGST as decimal) as IGST ,cast(Fld_Cess as decimal) CESS  from tblGSTR9_AllTable_Description header " +
                       " Left join " +
                       " ( " +
                       "   select * from tblGSTR9_Local_PtII_PtIII_PtV10_PtVI16 where Fld_HeaderGroup='PtIII-7' and Fld_GSTIN='" + CommonHelper.CompanyGSTN + "'" +
                       "   and Fld_FinancialYear='" + CommonHelper.ReturnYear + "'" +
                       " ) as dtl on header.Fld_SrNo=dtl.Fld_SrNo   " +
                       " where header.Fld_HeaderGroup='PtIII-7' order by header.Fld_SrNo ";
                dsgrid = MC.GetValueInDataset(strQuery, "dt7part3Local");
                grd.DataSource = dsgrid.Tables["dt7part3Local"];
            }
           // grd.Rows.Add(10);
           //// grd.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
           // //grd.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
           // //grd.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
           // //grd.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;

           

           // grd[0, 0].Value = "A";
           // grd[0, 1].Value = "B";
           // grd[0, 2].Value = "C";
           // grd[0, 3].Value = "D";
           // grd[0, 4].Value = "E";
           // grd[0, 5].Value = "F";
           // grd[0, 6].Value = "G";
           // grd[0, 7].Value = "H";
           // grd[0, 8].Value = "I";
           // grd[0, 9].Value = "J";
           // //grd[0, 10].Value = "K";
           // //grd[0, 11].Value = "L";
           // //grd[0, 12].Value = "M";
           // //grd[0, 13].Value = "N";

           // grd[1, 0].Value = "As per Rule 37";
           // grd[1, 1].Value = "As per Rule 39";
           // grd[1, 2].Value = "As per Rule 42";
           // grd[1, 3].Value = "As per Rule 43";
           // grd[1, 4].Value = "As per section 17(5)";
           // grd[1, 5].Value = "Reversal of TRAN-I credit";
           // grd[1, 6].Value = "Reversal of TRAN-II credit";
           // grd[1, 7].Value = "Other reversals(specify)";
           // grd[1, 8].Value = "Total ITC Reversed (Sum of A to H above)";
           // grd[1, 9].Value = "Net ITC Available for Utilization (6O - 7I)";
           // //grd[1, 10].Value = "Supplies reduced through Amendments (-)";
           // //grd[1, 11].Value = "Sub-Total (H to K above)";
           // //grd[1, 12].Value = "Turnover on which tax is not to be paid (G + L above)";
           // //grd[1, 13].Value = "Total Turnover (including advances)(4N + 5M - 4G above)";

            grd.Rows[8].ReadOnly = true;
            grd.Rows[9].ReadOnly = true;
            grd[5, 5].ReadOnly = true;
            grd[5, 6].ReadOnly = true;
            grd[4, 5].ReadOnly = true;
            grd[4, 6].ReadOnly = true;

            grd.Rows[8].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            grd.Rows[9].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;

            grd[5, 5].Style.BackColor = System.Drawing.Color.LightGray;
            grd[5, 6].Style.BackColor = System.Drawing.Color.LightGray;
            grd[4, 5].Style.BackColor = System.Drawing.Color.LightGray;
            grd[4, 6].Style.BackColor = System.Drawing.Color.LightGray;

            foreach (DataGridViewColumn col in grd.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.NullValue = "-";
                if (col.Index == 0) col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (col.Index > 1)
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = String.Format("N2");
                }
            }


        }
        private void grd7_part3_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                DataGridView gvr = sender as DataGridView;

                decimal val2 = 0;
                decimal val3 = 0; // Convert.ToDecimal(gvr[3, gvr.CurrentRow.Index].Value.ToString() == "" ? "0" : gvr[3, gvr.CurrentRow.Index].Value.ToString());
                decimal val4 = 0;// Convert.ToDecimal(gvr[4, gvr.CurrentRow.Index].Value.ToString() == "" ? "0" : gvr[4, gvr.CurrentRow.Index].Value.ToString());
                decimal val5 = 0;// Convert.ToDecimal(gvr[5, gvr.CurrentRow.Index].Value.ToString() == "" ? "0" : gvr[5, gvr.CurrentRow.Index].Value.ToString());
                //decimal val6 = 0;// Convert.ToDecimal(gvr[6, gvr.CurrentRow.Index].Value.ToString() == "" ? "0" : gvr[6, gvr.CurrentRow.Index].Value.ToString());
                int index = 0;
                foreach (DataGridViewRow gr in gvr.Rows)
                {

                    if (index <= 7)
                    {

                        val2 += Convert.ToDecimal(Convert.ToString(gr.Cells[2].Value) == "" ? "0" : gr.Cells[2].Value.ToString());
                        val3 += Convert.ToDecimal(Convert.ToString(gr.Cells[3].Value) == "" ? "0" : gr.Cells[3].Value.ToString());
                        val4 += Convert.ToDecimal(Convert.ToString(gr.Cells[4].Value) == "" ? "0" : gr.Cells[4].Value.ToString());
                        val5 += Convert.ToDecimal(Convert.ToString(gr.Cells[5].Value) == "" ? "0" : gr.Cells[5].Value.ToString());

                    }

                    if (index == 8)
                    {

                        gr.Cells[2].Value = val2;
                        gr.Cells[3].Value = val3;
                        gr.Cells[4].Value = val4;
                        gr.Cells[5].Value = val5;
                    }

                    if (index == 9)
                    {

                        gr.Cells[2].Value = Convert.ToDecimal(Convert.ToString(grd6_part3[3, 21].Value) == "" ? "0" : grd6_part3[3, 21].Value.ToString()) + val2;
                        gr.Cells[3].Value = Convert.ToDecimal(Convert.ToString(grd6_part3[4, 21].Value) == "" ? "0" : grd6_part3[4, 21].Value.ToString()) + val3;
                        gr.Cells[4].Value = Convert.ToDecimal(Convert.ToString(grd6_part3[5, 21].Value) == "" ? "0" : grd6_part3[5, 21].Value.ToString()) + val4;
                        gr.Cells[5].Value = Convert.ToDecimal(Convert.ToString(grd6_part3[6, 21].Value) == "" ? "0" : grd6_part3[6, 21].Value.ToString()) + val5;

                    }

                    index++;

                }
                Update_LinkedTable_Value(0, 0, 0, 1, 1);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }
        }

        private void grd7_part3_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView grd = new DataGridView();
            grd = grd7_part3;
            // e.Control.KeyPress -= new KeyPressEventHandler(Check);
            if (grd.CurrentCell.ColumnIndex > 1)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Check);
                }
            }
        }

        #endregion

        #region ***** Part-3 Table 8 ....

        private void BindPart3_Grid8(DataGridView grd)
        {
            
            grd.Columns[0].DataPropertyName = "Fld_SrNo";
            grd.Columns[1].DataPropertyName = "Fld_Description";
            grd.Columns[2].DataPropertyName = "CGST";
            grd.Columns[3].DataPropertyName = "SGST";
            grd.Columns[4].DataPropertyName = "IGST";
            grd.Columns[5].DataPropertyName = "CESS";
            if (grd.Name == "grdPortal8_part3")
            {

                strQuery = " select header.Fld_SrNo, header.Fld_Description, " +
                           " cast(Fld_CGST as decimal) CGST, cast(Fld_SGST as decimal) SGST," +
                           " cast(Fld_IGST as decimal) as IGST ,cast(Fld_Cess as decimal) CESS  from tblGSTR9_AllTable_Description header " +
                        " Left join " +
                        " ( " +
                        "   select * from tblGSTR9_PtII_PtIII_PtV10_PtVI16 where Fld_HeaderGroup='PtIII-8' and Fld_GSTIN='" + CommonHelper.CompanyGSTN + "'" +
                        "   and Fld_FinancialYear='" + CommonHelper.ReturnYear + "'" +
                        " ) as dtl on header.Fld_SrNo=dtl.Fld_SrNo   " +
                        " where header.Fld_HeaderGroup='PtIII-8' order by header.Fld_SrNo ";
                dsgrid = MC.GetValueInDataset(strQuery, "dt8part3Portal");
                grd.DataSource = dsgrid.Tables["dt8part3Portal"];
            }
            else
            {
                //strQuery = "select header.Fld_SrNo, header.Fld_Description ,null as Validate,null as TaxableValue, null as CGST,null as SGST,null as IGST,null as CESS " +
                //              " from tblGSTR9_AllTable_Description as header where header.Fld_HeaderGroup='PtII-4' order by header.Fld_SrNo ";
                strQuery = " select header.Fld_SrNo, header.Fld_Description, " +
                          "  cast(Fld_CGST as decimal) CGST, cast(Fld_SGST as decimal) SGST," +
                          " cast(Fld_IGST as decimal) as IGST ,cast(Fld_Cess as decimal) CESS  from tblGSTR9_AllTable_Description header " +
                       " Left join " +
                       " ( " +
                       "   select * from tblGSTR9_Local_PtII_PtIII_PtV10_PtVI16 where Fld_HeaderGroup='PtIII-8' and Fld_GSTIN='" + CommonHelper.CompanyGSTN + "'" +
                       "   and Fld_FinancialYear='" + CommonHelper.ReturnYear + "'" +
                       " ) as dtl on header.Fld_SrNo=dtl.Fld_SrNo   " +
                       " where header.Fld_HeaderGroup='PtIII-8' order by header.Fld_SrNo ";
                dsgrid = MC.GetValueInDataset(strQuery, "dt8part3Local");
                grd.DataSource = dsgrid.Tables["dt8part3Local"];
            }
            //grd.Rows.Add(11);
            ////grd.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ////grd.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;
            //grd.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //grd.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
           

            //grd[0, 0].Value = "A";
            //grd[0, 1].Value = "B";
            //grd[0, 2].Value = "C";
            //grd[0, 3].Value = "D";
            //grd[0, 4].Value = "E";
            //grd[0, 5].Value = "F";
            //grd[0, 6].Value = "G";
            //grd[0, 7].Value = "H";
            //grd[0, 8].Value = "I";
            //grd[0, 9].Value = "J";
            //grd[0, 10].Value = "K";
            ////grd[0, 11].Value = "L";
            ////grd[0, 12].Value = "M";
            ////grd[0, 13].Value = "N";

            //grd.Rows[0].Cells[1].Value = "ITC as per GSTR-2A (Table 3 & 5 thereof)";
            //grd[1, 1].Value = "ITC as per sum total 6(B) and 6(H)  above";
            //grd[1, 2].Value = "ITC on inward supplies (other than imports and inward supplies liable to reverse charge but includes services received from SEZs) received during 2017-18 but availed during April, 2018 to March, 2019";
            //grd[1, 3].Value = "Difference [A-(B+C)]";
            //grd[1, 4].Value = "ITC available but not availed";
            //grd[1, 5].Value = "ITC available but ineligible";
            //grd[1, 6].Value = "IGST paid  on import of goods (including supplies from SEZ)";
            //grd[1, 7].Value = "IGST credit availed on import of goods (as per 6(E) above)";
            //grd[1, 8].Value = "Difference (G-H)";
            //grd[1, 9].Value = "ITC available but not availed on import of goods (Equal to I)";
            //grd[1, 10].Value = "Total ITC to be lapsed in current financial year (E + F + J)";
            ////grd[1, 11].Value = "Sub-Total (H to K above)";
            ////grd[1, 12].Value = "Turnover on which tax is not to be paid (G + L above)";
            ////grd[1, 13].Value = "Total Turnover (including advances)(4N + 5M - 4G above)";

            grd.Rows[0].ReadOnly = true;
            grd.Rows[1].ReadOnly = true;
            grd.Rows[3].ReadOnly = true;
            grd.Rows[7].ReadOnly = true;
            grd.Rows[8].ReadOnly = true;
            grd.Rows[9].ReadOnly = true;
            grd.Rows[10].ReadOnly = true;

            grd.Rows[0].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            grd.Rows[1].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            grd.Rows[3].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            grd.Rows[7].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            grd.Rows[8].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            grd.Rows[9].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            grd.Rows[10].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;

            foreach (DataGridViewColumn col in grd.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.NullValue = "-";
                if (col.Index == 0) col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (col.Index > 1)
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = String.Format("N2");
                }
            }


        }
        private void grd8_part3_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            try
            {

                DataGridView gvr = sender as DataGridView;

                decimal val2 = 0;
                decimal val3 = 0; // Convert.ToDecimal(gvr[3, gvr.CurrentRow.Index].Value.ToString() == "" ? "0" : gvr[3, gvr.CurrentRow.Index].Value.ToString());
                decimal val4 = 0;// Convert.ToDecimal(gvr[4, gvr.CurrentRow.Index].Value.ToString() == "" ? "0" : gvr[4, gvr.CurrentRow.Index].Value.ToString());
                decimal val5 = 0;// Convert.ToDecimal(gvr[5, gvr.CurrentRow.Index].Value.ToString() == "" ? "0" : gvr[5, gvr.CurrentRow.Index].Value.ToString());
                //decimal val6 = 0;// Convert.ToDecimal(gvr[6, gvr.CurrentRow.Index].Value.ToString() == "" ? "0" : gvr[6, gvr.CurrentRow.Index].Value.ToString());
                int index = 0;
                foreach (DataGridViewRow gr in gvr.Rows)
                {
                    if (index == 1)
                    {

                        gr.Cells[2].Value = Convert.ToDecimal(Convert.ToString(grd6_part3[3, 1].Value) == "" ? "0" : grd6_part3[3, 1].Value.ToString())
                                          + Convert.ToDecimal(Convert.ToString(grd6_part3[3, 2].Value) == "" ? "0" : grd6_part3[3, 2].Value.ToString())
                                          + Convert.ToDecimal(Convert.ToString(grd6_part3[3, 3].Value) == "" ? "0" : grd6_part3[3, 3].Value.ToString())
                                          + Convert.ToDecimal(Convert.ToString(grd6_part3[3, 14].Value) == "" ? "0" : grd6_part3[3, 14].Value.ToString());

                        gr.Cells[3].Value = Convert.ToDecimal(Convert.ToString(grd6_part3[4, 1].Value) == "" ? "0" : grd6_part3[4, 1].Value.ToString())
                                          + Convert.ToDecimal(Convert.ToString(grd6_part3[4, 3].Value) == "" ? "0" : grd6_part3[4, 2].Value.ToString())
                                          + Convert.ToDecimal(Convert.ToString(grd6_part3[4, 3].Value) == "" ? "0" : grd6_part3[4, 3].Value.ToString())
                                          + Convert.ToDecimal(Convert.ToString(grd6_part3[4, 14].Value) == "" ? "0" : grd6_part3[4, 14].Value.ToString());

                        gr.Cells[4].Value = Convert.ToDecimal(Convert.ToString(grd6_part3[5, 1].Value) == "" ? "0" : grd6_part3[5, 1].Value.ToString())
                                          + Convert.ToDecimal(Convert.ToString(grd6_part3[5, 3].Value) == "" ? "0" : grd6_part3[5, 2].Value.ToString())
                                          + Convert.ToDecimal(Convert.ToString(grd6_part3[5, 3].Value) == "" ? "0" : grd6_part3[5, 3].Value.ToString())
                                          + Convert.ToDecimal(Convert.ToString(grd6_part3[5, 14].Value) == "" ? "0" : grd6_part3[5, 14].Value.ToString());

                        gr.Cells[5].Value = Convert.ToDecimal(Convert.ToString(grd6_part3[6, 1].Value) == "" ? "0" : grd6_part3[6, 1].Value.ToString())
                                          + Convert.ToDecimal(Convert.ToString(grd6_part3[6, 3].Value) == "" ? "0" : grd6_part3[6, 2].Value.ToString())
                                          + Convert.ToDecimal(Convert.ToString(grd6_part3[6, 3].Value) == "" ? "0" : grd6_part3[6, 3].Value.ToString())
                                          + Convert.ToDecimal(Convert.ToString(grd6_part3[6, 14].Value) == "" ? "0" : grd6_part3[6, 14].Value.ToString());


                    }

                    if (index == 2)
                    {

                        val2 += Convert.ToDecimal(Convert.ToString(gr.Cells[2].Value) == "" ? "0" : gr.Cells[2].Value.ToString());
                        val3 += Convert.ToDecimal(Convert.ToString(gr.Cells[3].Value) == "" ? "0" : gr.Cells[3].Value.ToString());
                        val4 += Convert.ToDecimal(Convert.ToString(gr.Cells[4].Value) == "" ? "0" : gr.Cells[4].Value.ToString());
                        val5 += Convert.ToDecimal(Convert.ToString(gr.Cells[5].Value) == "" ? "0" : gr.Cells[5].Value.ToString());

                        //  val2 = 0; val3 = 0; val4 = 0; val5 = 0;


                    }

                    if (index == 3)
                    {

                        gr.Cells[2].Value = Convert.ToDecimal(Convert.ToString(grd8_part3[2, 0].Value) == "" ? "0" : grd8_part3[2, 0].Value.ToString())
                                  - (val2 + Convert.ToDecimal(Convert.ToString(grd8_part3[2, 1].Value) == "" ? "0" : grd8_part3[2, 1].Value.ToString()));

                        gr.Cells[3].Value = Convert.ToDecimal(Convert.ToString(grd8_part3[3, 0].Value) == "" ? "0" : grd8_part3[3, 0].Value.ToString())
                                  - (val3 + Convert.ToDecimal(Convert.ToString(grd8_part3[3, 1].Value) == "" ? "0" : grd8_part3[3, 1].Value.ToString()));

                        gr.Cells[4].Value = Convert.ToDecimal(Convert.ToString(grd8_part3[4, 0].Value) == "" ? "0" : grd8_part3[4, 0].Value.ToString())
                                  - (val4 + Convert.ToDecimal(Convert.ToString(grd8_part3[4, 1].Value) == "" ? "0" : grd8_part3[4, 1].Value.ToString()));

                        gr.Cells[5].Value = Convert.ToDecimal(Convert.ToString(grd8_part3[5, 0].Value) == "" ? "0" : grd8_part3[5, 0].Value.ToString())
                                  - (val5 + Convert.ToDecimal(Convert.ToString(grd8_part3[5, 1].Value) == "" ? "0" : grd8_part3[5, 1].Value.ToString()));

                        val2 = 0; val3 = 0; val4 = 0; val5 = 0;
                    }

                    if (index >= 4 && index <= 6)
                    {

                        val2 += Convert.ToDecimal(Convert.ToString(gr.Cells[2].Value) == "" ? "0" : gr.Cells[2].Value.ToString());
                        val3 += Convert.ToDecimal(Convert.ToString(gr.Cells[3].Value) == "" ? "0" : gr.Cells[3].Value.ToString());
                        val4 += Convert.ToDecimal(Convert.ToString(gr.Cells[4].Value) == "" ? "0" : gr.Cells[4].Value.ToString());
                        val5 += Convert.ToDecimal(Convert.ToString(gr.Cells[5].Value) == "" ? "0" : gr.Cells[5].Value.ToString());

                    }

                    if (index == 7)
                    {

                        gr.Cells[2].Value = Convert.ToDecimal(Convert.ToString(grd6_part3[3, 10].Value) == "" ? "0" : grd6_part3[3, 10].Value.ToString())
                                          + Convert.ToDecimal(Convert.ToString(grd6_part3[3, 11].Value) == "" ? "0" : grd6_part3[3, 11].Value.ToString());

                        gr.Cells[3].Value = Convert.ToDecimal(Convert.ToString(grd6_part3[4, 10].Value) == "" ? "0" : grd6_part3[4, 10].Value.ToString())
                                          + Convert.ToDecimal(Convert.ToString(grd6_part3[4, 11].Value) == "" ? "0" : grd6_part3[4, 11].Value.ToString());

                        gr.Cells[4].Value = Convert.ToDecimal(Convert.ToString(grd6_part3[5, 10].Value) == "" ? "0" : grd6_part3[5, 10].Value.ToString())
                                          + Convert.ToDecimal(Convert.ToString(grd6_part3[5, 11].Value) == "" ? "0" : grd6_part3[5, 11].Value.ToString());

                        gr.Cells[5].Value = Convert.ToDecimal(Convert.ToString(grd6_part3[6, 10].Value) == "" ? "0" : grd6_part3[6, 10].Value.ToString())
                                          + Convert.ToDecimal(Convert.ToString(grd6_part3[6, 11].Value) == "" ? "0" : grd6_part3[6, 11].Value.ToString());
                    }

                    if (index == 8)
                    {

                        gr.Cells[2].Value = Convert.ToDecimal(Convert.ToString(grd8_part3[2, 6].Value) == "" ? "0" : grd8_part3[2, 6].Value.ToString())
                                        - (Convert.ToDecimal(Convert.ToString(grd8_part3[2, 7].Value) == "" ? "0" : grd8_part3[2, 7].Value.ToString()));

                        gr.Cells[3].Value = Convert.ToDecimal(Convert.ToString(grd8_part3[3, 6].Value) == "" ? "0" : grd8_part3[3, 6].Value.ToString())
                                         - (Convert.ToDecimal(Convert.ToString(grd8_part3[3, 7].Value) == "" ? "0" : grd8_part3[3, 7].Value.ToString()));

                        gr.Cells[4].Value = Convert.ToDecimal(Convert.ToString(grd8_part3[4, 6].Value) == "" ? "0" : grd8_part3[4, 6].Value.ToString())
                                        - (Convert.ToDecimal(Convert.ToString(grd8_part3[4, 7].Value) == "" ? "0" : grd8_part3[4, 7].Value.ToString()));

                        gr.Cells[5].Value = Convert.ToDecimal(Convert.ToString(grd8_part3[5, 6].Value) == "" ? "0" : grd8_part3[5, 6].Value.ToString())
                                        - (Convert.ToDecimal(Convert.ToString(grd8_part3[5, 7].Value) == "" ? "0" : grd8_part3[5, 7].Value.ToString()));

                    }

                    if (index == 9)
                    {

                        gr.Cells[2].Value = grd8_part3[2, 8].Value;
                        gr.Cells[3].Value = grd8_part3[3, 8].Value;
                        gr.Cells[4].Value = grd8_part3[4, 8].Value;
                        gr.Cells[5].Value = grd8_part3[5, 8].Value;

                    }

                    if (index == 10)
                    {

                        gr.Cells[2].Value = val2;
                        gr.Cells[3].Value = val3;
                        gr.Cells[4].Value = val4;
                        gr.Cells[5].Value = val5;

                    }

                    index++;

                }
                Update_LinkedTable_Value(0, 0, 0, 0, 1);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }

        }
        private void grd8_part3_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView grd = new DataGridView();
            grd = grd8_part3;
            // e.Control.KeyPress -= new KeyPressEventHandler(Check);
            if (grd.CurrentCell.ColumnIndex > 1)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Check);
                }
            }
        }

        #endregion

        #region ***** Part-4 Table 9 ....

        private void BindPart4_Grid9(DataGridView grd9_part4)
        {
            grd9_part4.Rows.Add(8);
            grd9_part4.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //grd9_part4.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;

            grd9_part4.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //grd_4.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;

            grd9_part4[0, 0].Value = "A";
            grd9_part4[0, 1].Value = "B";
            grd9_part4[0, 2].Value = "C";
            grd9_part4[0, 3].Value = "D";
            grd9_part4[0, 4].Value = "E";
            grd9_part4[0, 5].Value = "F";
            grd9_part4[0, 6].Value = "G";
            grd9_part4[0, 7].Value = "H";

            grd9_part4.Rows[0].Cells[1].Value = "Integrated Tax";
            grd9_part4[1, 1].Value = "Central Tax";
            grd9_part4[1, 2].Value = "State / UT Tax";
            grd9_part4[1, 3].Value = "Cess";
            grd9_part4[1, 4].Value = "Interest";
            grd9_part4[1, 5].Value = "Late Fee";
            grd9_part4[1, 6].Value = "Penalty";
            grd9_part4[1, 7].Value = "Other";

            grd9_part4.Rows[0].ReadOnly = true;
            grd9_part4.Rows[1].ReadOnly = true;
            grd9_part4.Rows[2].ReadOnly = true;
            grd9_part4.Rows[3].ReadOnly = true;
            grd9_part4.Rows[4].ReadOnly = true;
            grd9_part4.Rows[5].ReadOnly = true;
            grd9_part4.Rows[6].ReadOnly = true;
            grd9_part4.Rows[7].ReadOnly = true;

            grd9_part4.Rows[0].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            grd9_part4.Rows[1].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            grd9_part4.Rows[2].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            grd9_part4.Rows[3].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            grd9_part4.Rows[4].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            grd9_part4.Rows[5].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            grd9_part4.Rows[6].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            grd9_part4.Rows[7].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;

            grd9_part4[2, 0].ReadOnly = false; //grd9_part4[8, 0].ReadOnly = false;
            grd9_part4[2, 0].Style.BackColor = System.Drawing.Color.White; //grd9_part4[8, 0].Style.BackColor = System.Drawing.Color.White;

            grd9_part4[2, 1].ReadOnly = false; //grd9_part4[8, 1].ReadOnly = false;
            grd9_part4[2, 1].Style.BackColor = System.Drawing.Color.White;// grd9_part4[8, 1].Style.BackColor = System.Drawing.Color.White;


            grd9_part4[2, 2].ReadOnly = false;// grd9_part4[8, 2].ReadOnly = false;
            grd9_part4[2, 2].Style.BackColor = System.Drawing.Color.White; //grd9_part4[8, 2].Style.BackColor = System.Drawing.Color.White;


            grd9_part4[2, 3].ReadOnly = false;// grd9_part4[8, 3].ReadOnly = false;
            grd9_part4[2, 3].Style.BackColor = System.Drawing.Color.White;// grd9_part4[8, 3].Style.BackColor = System.Drawing.Color.White;


            grd9_part4[2, 4].ReadOnly = false; //grd9_part4[8, 4].ReadOnly = false;
            grd9_part4[2, 4].Style.BackColor = System.Drawing.Color.White; //grd9_part4[8, 4].Style.BackColor = System.Drawing.Color.White;

            grd9_part4[2, 5].ReadOnly = false; //grd9_part4[8, 5].ReadOnly = false;
            grd9_part4[2, 5].Style.BackColor = System.Drawing.Color.White; //grd9_part4[8, 5].Style.BackColor = System.Drawing.Color.White;

            grd9_part4[2, 6].ReadOnly = false; //grd9_part4[8, 6].ReadOnly = false;
            grd9_part4[2, 6].Style.BackColor = System.Drawing.Color.White; //grd9_part4[8, 6].Style.BackColor = System.Drawing.Color.White;

            grd9_part4[2, 7].ReadOnly = false; //grd9_part4[8, 7].ReadOnly = false;
            grd9_part4[2, 7].Style.BackColor = System.Drawing.Color.White; //grd9_part4[8, 7].Style.BackColor = System.Drawing.Color.White;




        }

        private void grd9_part4_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView grd = new DataGridView();
            grd = grd9_part4;
            // e.Control.KeyPress -= new KeyPressEventHandler(Check);
            if (grd.CurrentCell.ColumnIndex > 1)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Check);
                }
            }
        }

        #endregion

        #region ***** Part-5 Table 10 ....

        private void BindPart5_Grid10(DataGridView grd10_part5)
        {
            grd10_part5.Rows.Add(5);
            grd10_part5.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //grd10_part5.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;

            grd10_part5.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //grd_4.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;

            grd10_part5[0, 0].Value = "10";
            grd10_part5[0, 1].Value = "11";
            grd10_part5[0, 2].Value = "12";
            grd10_part5[0, 3].Value = "13";
            grd10_part5[0, 4].Value = "";




            grd10_part5.Rows[0].Cells[1].Value = " Supplies / tax declared through Amendments (+) (net of debit notes)";
            grd10_part5[1, 1].Value = "Supplies / tax reduced through Amendments (-) (net of credit notes)";
            grd10_part5[1, 2].Value = "Reversal of ITC availed during previous financial year";
            grd10_part5[1, 3].Value = "ITC availed for the previous financial year";
            grd10_part5[1, 4].Value = "Total turnover (5N +10-11)";

            grd10_part5[2, 2].ReadOnly = true;
            grd10_part5[2, 3].ReadOnly = true;
            grd10_part5.Rows[4].ReadOnly = true;

            grd10_part5[2, 2].Style.BackColor = System.Drawing.Color.LightGray;
            grd10_part5[2, 3].Style.BackColor = System.Drawing.Color.LightGray;
            grd10_part5.Rows[4].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;



            foreach (DataGridViewColumn col in grd10_part5.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        private void grd10_part5_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                DataGridView gvr = sender as DataGridView;
                int index = 0;
                foreach (DataGridViewRow gr in gvr.Rows)
                {

                    if (index == 4)
                    {

                        gr.Cells[2].Value = Convert.ToDecimal(Convert.ToString(grd5_part2[2, 13].Value) == "" ? "0" : grd5_part2[2, 13].Value.ToString())
                                          + Convert.ToDecimal(Convert.ToString(grd10_part5[2, 0].Value) == "" ? "0" : grd10_part5[2, 0].Value.ToString())
                                          - Convert.ToDecimal(Convert.ToString(grd10_part5[2, 1].Value) == "" ? "0" : grd10_part5[2, 1].Value.ToString());

                        gr.Cells[3].Value = Convert.ToDecimal(Convert.ToString(grd5_part2[3, 13].Value) == "" ? "0" : grd5_part2[3, 13].Value.ToString())
                                          + Convert.ToDecimal(Convert.ToString(grd10_part5[3, 0].Value) == "" ? "0" : grd10_part5[3, 0].Value.ToString())
                                          - Convert.ToDecimal(Convert.ToString(grd10_part5[3, 1].Value) == "" ? "0" : grd10_part5[3, 1].Value.ToString());

                        gr.Cells[4].Value = Convert.ToDecimal(Convert.ToString(grd5_part2[4, 13].Value) == "" ? "0" : grd5_part2[4, 13].Value.ToString())
                                          + Convert.ToDecimal(Convert.ToString(grd10_part5[4, 0].Value) == "" ? "0" : grd10_part5[4, 0].Value.ToString())
                                          - Convert.ToDecimal(Convert.ToString(grd10_part5[4, 1].Value) == "" ? "0" : grd10_part5[4, 1].Value.ToString());

                        gr.Cells[5].Value = Convert.ToDecimal(Convert.ToString(grd5_part2[5, 13].Value) == "" ? "0" : grd5_part2[5, 13].Value.ToString())
                                          + Convert.ToDecimal(Convert.ToString(grd10_part5[5, 0].Value) == "" ? "0" : grd10_part5[5, 0].Value.ToString())
                                          - Convert.ToDecimal(Convert.ToString(grd10_part5[5, 1].Value) == "" ? "0" : grd10_part5[5, 1].Value.ToString());

                        gr.Cells[6].Value = Convert.ToDecimal(Convert.ToString(grd5_part2[6, 13].Value) == "" ? "0" : grd5_part2[6, 13].Value.ToString())
                                          + Convert.ToDecimal(Convert.ToString(grd10_part5[6, 0].Value) == "" ? "0" : grd10_part5[6, 0].Value.ToString())
                                          - Convert.ToDecimal(Convert.ToString(grd10_part5[6, 1].Value) == "" ? "0" : grd10_part5[6, 1].Value.ToString());

                    }

                    index++;

                }
                // Update_LinkedTable_Value(0, 0, 0, 0, 0);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }
        }

        private void grd10_part5_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView grd = new DataGridView();
            grd = grd10_part5;
            // e.Control.KeyPress -= new KeyPressEventHandler(Check);
            if (grd.CurrentCell.ColumnIndex > 1)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Check);
                }
            }
        }

        #endregion

        #region ***** Part-5 Table 14 ....

        private void BindPart5_Grid14(DataGridView grd14_part5)
        {
            grd14_part5.Rows.Add(5);
            grd14_part5.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //grd14_part5.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;

            grd14_part5.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //grd_4.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;

            grd14_part5[0, 0].Value = "A";
            grd14_part5[0, 1].Value = "B";
            grd14_part5[0, 2].Value = "C";
            grd14_part5[0, 3].Value = "D";
            grd14_part5[0, 4].Value = "E";



            grd14_part5.Rows[0].Cells[1].Value = "Integrated Tax";
            grd14_part5[1, 1].Value = "Central Tax";
            grd14_part5[1, 2].Value = "State / UT Tax";
            grd14_part5[1, 3].Value = "Cess";
            grd14_part5[1, 4].Value = "Interest";

            foreach (DataGridViewColumn col in grd14_part5.Columns)
            {

                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            }

        }
        private void grd14_part5_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView grd = new DataGridView();
            grd = grd14_part5;
            // e.Control.KeyPress -= new KeyPressEventHandler(Check);
            if (grd.CurrentCell.ColumnIndex > 1)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Check);
                }
            }
        }

        #endregion

        #region ***** Part-6 Table 15 ....

        private void BindPart6_Grid15(DataGridView grd15_part6)
        {

            grd15_part6.Rows.Add(7);
           // grd15_part6.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //grd15_part6.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;

           // grd15_part6.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //grd_4.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;

            grd15_part6[0, 0].Value = "A";
            grd15_part6[0, 1].Value = "B";
            grd15_part6[0, 2].Value = "C";
            grd15_part6[0, 3].Value = "D";
            grd15_part6[0, 4].Value = "E";
            grd15_part6[0, 5].Value = "F";
            grd15_part6[0, 6].Value = "G";


            grd15_part6.Rows[0].Cells[1].Value = "Total Refund claimed";
            grd15_part6[1, 1].Value = "Total Refund sanctioned";
            grd15_part6[1, 2].Value = "Total Refund Rejected";
            grd15_part6[1, 3].Value = "Total Refund Pending";
            grd15_part6[1, 4].Value = "Total demand of taxes ";
            grd15_part6[1, 5].Value = "Total taxes paid in respect of E above";
            grd15_part6[1, 6].Value = "Total demands pending out of E above";


            grd15_part6[6, 0].ReadOnly = true;
            grd15_part6[6, 1].ReadOnly = true;
            grd15_part6[6, 2].ReadOnly = true;
            grd15_part6[6, 3].ReadOnly = true;

            grd15_part6[7, 0].ReadOnly = true;
            grd15_part6[7, 1].ReadOnly = true;
            grd15_part6[7, 2].ReadOnly = true;
            grd15_part6[7, 3].ReadOnly = true;

            grd15_part6[8, 0].ReadOnly = true;
            grd15_part6[8, 1].ReadOnly = true;
            grd15_part6[8, 2].ReadOnly = true;
            grd15_part6[8, 3].ReadOnly = true;


            grd15_part6[6, 0].Style.BackColor = System.Drawing.Color.LightGray;
            grd15_part6[6, 1].Style.BackColor = System.Drawing.Color.LightGray;
            grd15_part6[6, 2].Style.BackColor = System.Drawing.Color.LightGray;
            grd15_part6[6, 3].Style.BackColor = System.Drawing.Color.LightGray;

            grd15_part6[7, 0].Style.BackColor = System.Drawing.Color.LightGray;
            grd15_part6[7, 1].Style.BackColor = System.Drawing.Color.LightGray;
            grd15_part6[7, 2].Style.BackColor = System.Drawing.Color.LightGray;
            grd15_part6[7, 3].Style.BackColor = System.Drawing.Color.LightGray;

            grd15_part6[8, 0].Style.BackColor = System.Drawing.Color.LightGray;
            grd15_part6[8, 1].Style.BackColor = System.Drawing.Color.LightGray;
            grd15_part6[8, 2].Style.BackColor = System.Drawing.Color.LightGray;
            grd15_part6[8, 3].Style.BackColor = System.Drawing.Color.LightGray;


            foreach (DataGridViewColumn col in grd15_part6.Columns)
            {

                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            }

        }

        private void grd15_part6_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView grd = new DataGridView();
            grd = grd15_part6;
            // e.Control.KeyPress -= new KeyPressEventHandler(Check);
            if (grd.CurrentCell.ColumnIndex > 1)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Check);
                }
            }
        }

        #endregion

        #region ***** Part-6 Table 16 ....

        private void BindPart6_Grid16(DataGridView grd16_part6)
        {
            grd16_part6.Rows.Add(3);

            grd16_part6[0, 0].Value = "A";
            grd16_part6[0, 1].Value = "B";
            grd16_part6[0, 2].Value = "C";

            grd16_part6.Rows[0].Cells[1].Value = "Supplies received from Composition taxpayers";
            grd16_part6[1, 1].Value = "Deemed supply under section 143";
            grd16_part6[1, 2].Value = "Goods sent on approval basis but not returned";


            grd16_part6[3, 0].ReadOnly = true;
            grd16_part6[4, 0].ReadOnly = true;
            grd16_part6[5, 0].ReadOnly = true;
            grd16_part6[6, 0].ReadOnly = true;
            grd16_part6[3, 0].Style.BackColor = System.Drawing.Color.LightGray;
            grd16_part6[4, 0].Style.BackColor = System.Drawing.Color.LightGray;
            grd16_part6[5, 0].Style.BackColor = System.Drawing.Color.LightGray;
            grd16_part6[6, 0].Style.BackColor = System.Drawing.Color.LightGray;


            foreach (DataGridViewColumn col in grd16_part6.Columns)
            {

                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            }

        }

        private void grd16_part6_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView grd = new DataGridView();
            grd = grd16_part6;
            // e.Control.KeyPress -= new KeyPressEventHandler(Check);
            if (grd.CurrentCell.ColumnIndex > 1)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Check);
                }
            }
        }

        #endregion

        #region ***** GSTR-9 Computation .....

        private void BindGrid_Computation(DataGridView grdComputation)
        {
            grdComputation.Rows.Add(41);


            grdComputation.Rows[0].Cells[0].Value = "";
            grdComputation.Rows[0].Cells[1].Value = "Outward Supply Details";
            ((DataGridViewTextBoxCellPro)grdComputation[1, 0]).ColumnSpan = grdComputation.ColumnCount-1;
            grdComputation.Rows[0].DefaultCellStyle.BackColor = System.Drawing.Color.LightSkyBlue;
            grdComputation.Rows[0].DefaultCellStyle.Font = new Font("Verdana", 8, FontStyle.Bold);
            grdComputation.Rows[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            grdComputation[0, 1].Value = "1";
            grdComputation[1, 1].Value = "Supply & Tax Reported in Current F.Y.";
            grdComputation[2, 1].Value = "Table No. 5N";

            grdComputation[0, 2].Value = "2";
            grdComputation[1, 2].Value = "Suppy & Tax Short Reported in Current F.Y. Increased in Next F.Y.";
            grdComputation[2, 2].Value = "Table No. 10";

            grdComputation[0, 3].Value = "3";
            grdComputation[1, 3].Value = "Supplies & Tax Excess Reported Current F.Y. Reduced in Next F.Y.";
            grdComputation[2, 3].Value = "Table No. 11";

            grdComputation[0, 4].Value = "";
            grdComputation[1, 4].Value = "Total Outward Supply & Tax Payable [A] = [ 1 + 2 - 3]";
            grdComputation[2, 4].Value = "";
            grdComputation.Rows[4].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(65, 65, 65);
            grdComputation.Rows[4].DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            grdComputation.Rows[4].DefaultCellStyle.Font = new Font("Verdana", 8, FontStyle.Bold);
            
            // row-5  is blanck row

            grdComputation.Rows[6].Cells[0].Value = "";
            grdComputation.Rows[6].Cells[1].Value = "Input Tax Credit Details";
            ((DataGridViewTextBoxCellPro)grdComputation[1, 6]).ColumnSpan = grdComputation.ColumnCount-1;
            grdComputation.Rows[6].DefaultCellStyle.BackColor = System.Drawing.Color.LightSkyBlue;
            grdComputation.Rows[6].DefaultCellStyle.Font = new Font("Verdana", 8, FontStyle.Bold);
            grdComputation.Rows[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            grdComputation[0, 7].Value = "4";
            grdComputation[1, 7].Value = "ITC Claimed Reported in Current F.Y.";
            grdComputation[2, 7].Value = "Table No. 6O";

            grdComputation[0, 8].Value = "5";
            grdComputation[1, 8].Value = "ITC Short Reported / Claimed in Curent F.Y. Increased / Availed  in Next F.Y.";
            grdComputation[2, 8].Value = "Table No. 13";

            grdComputation[0, 9].Value = "";
            grdComputation[1, 9].Value = "Total ITC Reversed [C]= [6+7]";
            grdComputation[2, 9].Value = "";
            grdComputation.Rows[9].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(65, 65, 65);
            grdComputation.Rows[9].DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            grdComputation.Rows[9].DefaultCellStyle.Font = new Font("Verdana", 8, FontStyle.Bold);

            grdComputation[0, 10].Value = "6";
            grdComputation[1, 10].Value = "ITC Reversal in Current F.Y. ";
            grdComputation[2, 10].Value = "Table No. 7I";

            grdComputation[0, 11].Value = "7";
            grdComputation[1, 11].Value = "ITC Excess Reported / Claimed in Curent F.Y. Reduced / Reversed  in Next F.Y. ";
            grdComputation[2, 11].Value = "Table No. 12";

            grdComputation[0, 12].Value = "";
            grdComputation[1, 12].Value = "Total ITC Reversed [C]= [6+7]";
            grdComputation[2, 12].Value = "";
            grdComputation.Rows[12].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(65, 65, 65);
            grdComputation.Rows[12].DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            grdComputation.Rows[12].DefaultCellStyle.Font = new Font("Verdana", 8, FontStyle.Bold);

            // row-13 is Empty
            grdComputation[0, 14].Value = "";
            grdComputation[1, 14].Value = "Net ITC Claimed [D] = [B-C]";
            grdComputation[2, 14].Value = "";
            grdComputation.Rows[14].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(65, 65, 65);
            grdComputation.Rows[14].DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            grdComputation.Rows[14].DefaultCellStyle.Font = new Font("Verdana", 8, FontStyle.Bold);
            //// row-15 is Empty
            grdComputation.Rows[16].Cells[0].Value = "";
            grdComputation.Rows[16].Cells[1].Value = "Tax Payable On RCM";
            ((DataGridViewTextBoxCellPro)grdComputation[1, 16]).ColumnSpan = grdComputation.ColumnCount - 1;
            grdComputation.Rows[16].DefaultCellStyle.BackColor = System.Drawing.Color.LightSkyBlue;
            grdComputation.Rows[16].DefaultCellStyle.Font = new Font("Verdana", 8, FontStyle.Bold);
            grdComputation.Rows[16].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            grdComputation[0, 17].Value = "";
            grdComputation[1, 17].Value = "Tax On Inward Supplies Liable to Reverse Charge   [E]";
            grdComputation[2, 17].Value = "Table No. 4G";
            // row-18 is Empty
            grdComputation.Rows[19].Cells[0].Value = "";
            grdComputation.Rows[19].Cells[1].Value = "Computation of Tax Payable & Tax Paid";
            ((DataGridViewTextBoxCellPro)grdComputation[1, 19]).ColumnSpan = grdComputation.ColumnCount - 1;
            grdComputation.Rows[19].DefaultCellStyle.BackColor = System.Drawing.Color.LightSkyBlue;
            grdComputation.Rows[19].DefaultCellStyle.Font = new Font("Verdana", 8, FontStyle.Bold);
            grdComputation.Rows[19].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            grdComputation[0, 20].Value = "8";
            grdComputation[1, 20].Value = "Tax Payable [ A+ E]";
            grdComputation[2, 20].Value = "Table No. 9";

            grdComputation[0, 21].Value = "9";
            grdComputation[1, 21].Value = "Interest Payable ";
            grdComputation[2, 21].Value = "Table No. 9";

            grdComputation[0, 22].Value = "10";
            grdComputation[1, 22].Value = "Late Fees Payable";
            grdComputation[2, 22].Value = "Table No. 9";

            grdComputation[0, 23].Value = "11";
            grdComputation[1, 23].Value = "Penalty";
            grdComputation[2, 23].Value = "Table no. 9";

            grdComputation[0, 24].Value = "12";
            grdComputation[1, 24].Value = "Total Amount Payable [F] = [8+9+10+11+12]";
            grdComputation[2, 24].Value = "";
            grdComputation.Rows[24].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(65, 65, 65);
            grdComputation.Rows[24].DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            grdComputation.Rows[24].DefaultCellStyle.Font = new Font("Verdana", 8, FontStyle.Bold);

            grdComputation[0, 25].Value = "13(a)";
            grdComputation[1, 25].Value = " Tax Already Paid through IGST";
            grdComputation[2, 25].Value = "Table No. 9";

            grdComputation[0, 26].Value = "13(b)";
            grdComputation[1, 26].Value = " Tax Already Paid through CGST ";
            grdComputation[2, 26].Value = "Table No. 9";

            grdComputation[0, 27].Value = "13(c)";
            grdComputation[1, 27].Value = " Tax Already Paid through SGST";
            grdComputation[2, 27].Value = "Table No. 9";

            grdComputation[0, 28].Value = "13(d)";
            grdComputation[1, 28].Value = " Tax Already Paid through CESS";
            grdComputation[2, 28].Value = "Table no. 9";


            grdComputation[0, 29].Value = "13";
            grdComputation[1, 29].Value = "Total Tax Paid through Input Tax Credit";
            grdComputation[2, 29].Value = "";
            grdComputation.Rows[29].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(65, 65, 65);
            grdComputation.Rows[29].DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            grdComputation.Rows[29].DefaultCellStyle.Font = new Font("Verdana", 8, FontStyle.Bold);


            grdComputation[0, 30].Value = "14(a)";
            grdComputation[1, 30].Value = "Tax Already Paid through Cash";
            grdComputation[2, 30].Value = "Table No. 9";

            grdComputation[0, 31].Value = "14(b)";
            grdComputation[1, 31].Value = "Late Fees Paid through Cash ";
            grdComputation[2, 31].Value = "Table No. 9";

            grdComputation[0, 32].Value = "14(c)";
            grdComputation[1, 32].Value = " Interest Paid in Cash";
            grdComputation[2, 32].Value = "Table No. 9";

            grdComputation[0, 33].Value = "14(d)";
            grdComputation[1, 33].Value = "Penalty Paid in Cash";
            grdComputation[2, 33].Value = "Table no. 9";

            grdComputation[0, 34].Value = "14";
            grdComputation[1, 34].Value = "Total Paid through Cash Ledger";
            grdComputation[2, 34].Value = "";
            grdComputation.Rows[34].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(65, 65, 65);
            grdComputation.Rows[34].DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            grdComputation.Rows[34].DefaultCellStyle.Font = new Font("Verdana", 8, FontStyle.Bold);

            grdComputation[0, 35].Value = "15";
            grdComputation[1, 35].Value = "Total Amount Already Paid [G] = [13 + 14]";
            grdComputation[2, 35].Value = "";
            grdComputation.Rows[35].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            grdComputation.Rows[35].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            grdComputation.Rows[35].DefaultCellStyle.Font = new Font("Verdana", 8, FontStyle.Bold);

            grdComputation[0, 36].Value = "16";
            grdComputation[1, 36].Value = "Balance Amount Payable  [H] = [F-G]";
            //grdComputation[2, 36].Value = "";
            ((DataGridViewTextBoxCellPro)grdComputation[1, 36]).ColumnSpan = 3;
            grdComputation.Rows[36].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 192, 0);
            grdComputation.Rows[36].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            grdComputation.Rows[36].DefaultCellStyle.Font = new Font("Verdana", 8, FontStyle.Bold);

            grdComputation[0, 37].Value = "17";
            grdComputation[1, 37].Value = "Excess Tax Paid [ I ] = [G-F]";
            //grdComputation[2, 37].Value = "";
            ((DataGridViewTextBoxCellPro)grdComputation[1, 37]).ColumnSpan = 3;
            grdComputation.Rows[37].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(146, 208, 80);
            grdComputation.Rows[37].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            grdComputation.Rows[37].DefaultCellStyle.Font = new Font("Verdana", 8, FontStyle.Bold);

            grdComputation[0, 38].Value = "18";
            grdComputation[1, 38].Value = "ITC to Be Carried forward to Next Tax Period [J]= [D-13]";
            //grdComputation[2, 38].Value = "";
            ((DataGridViewTextBoxCellPro)grdComputation[1, 38]).ColumnSpan = 3;
            grdComputation.Rows[38].DefaultCellStyle.BackColor = System.Drawing.Color.SkyBlue;
            grdComputation.Rows[38].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            grdComputation.Rows[38].DefaultCellStyle.Font = new Font("Verdana", 8, FontStyle.Bold);

            grdComputation[0, 39].Value = "19";
            grdComputation[1, 39].Value = "ITC Balance as per Credit Ledger after Filing March-18 Return";
            //grdComputation[2, 39].Value = "";
            ((DataGridViewTextBoxCellPro)grdComputation[1, 39]).ColumnSpan = 3;
            grdComputation.Rows[39].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(252, 213, 180);
            grdComputation.Rows[39].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            grdComputation.Rows[39].DefaultCellStyle.Font = new Font("Verdana", 8, FontStyle.Bold);


              grdComputation[0, 40].Value = "20";
            grdComputation[1, 40].Value = "Disclaimer: This Sheet is Developed by Team Khamesra Bhatia & Mehrotra, Chartered Accountants, Kanpur on the Basis of their understanding on GSTR-9 and GST Law. All the efforts have been made to make this Computation error free." +
                                           " However, If you find any issue or error in this computation please infomr so that corrections may be done. Please cross Check the Data and do your own calculation before filing of GSTR-9 on GST Portal on the basis of this working.";
            ((DataGridViewTextBoxCellPro)grdComputation[1, 40]).ColumnSpan = grdComputation.ColumnCount - 1;
            grdComputation.Rows[40].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            grdComputation.Rows[40].DefaultCellStyle.Font = new Font("Verdana", 8, FontStyle.Bold);
            
            //grdComputation.Rows[40].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(252, 213, 180);
            //grdComputation.Rows[40].DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            //grdComputation.Rows[40].DefaultCellStyle.Font = new Font("Verdana", 8, FontStyle.Bold);

            foreach (DataGridViewColumn col in grd16_part6.Columns)
            {

                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            }

        }

        #endregion

        private void Update_LinkedTable_Value(int check_grd5, int check_grd6, int check_grd7, int check_grd8, int check_grd10)
        {
            if (check_grd5 == 1) grd5_part2_CellEndEdit(grd5_part2, null);
            if (check_grd6 == 1) grd6_part3_CellEndEdit(grd6_part3, null);
            if (check_grd7 == 1) grd7_part3_CellEndEdit(grd7_part3, null);
            if (check_grd8 == 1) grd8_part3_CellEndEdit(grd8_part3, null);
            if (check_grd10 == 1) grd10_part5_CellEndEdit(grd10_part5, null);

        }
        private void Check(object sender, KeyPressEventArgs e)
        {
            MC.Decimalsvalues(e);
            //MC.ToBlockChar(e);
        }
        private void tp_Summary_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {

            //CompanyDashboard obj = new CompanyDashboard();
            //obj.MdiParent = this.MdiParent;
            //Utility.CloseAllOpenForm();
            this.Close();
            //obj.Dock = DockStyle.Fill;
            //obj.Show();

            
        }

        private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (tabMain.SelectedIndex == 0) { btnImportExcel.Visible = true; btnDownloadData.Visible = false; btnDifference.Visible = false; }
            //if (tabMain.SelectedIndex == 1) { btnImportExcel.Visible = false; btnDownloadData.Visible = true; btnDifference.Visible = false; }
            //if (tabMain.SelectedIndex == 2) { btnImportExcel.Visible = false; btnDownloadData.Visible = false; btnDifference.Visible = true; }
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            //OpenFileDialog openfile = new OpenFileDialog();
            //openfile.DefaultExt = ".xlsx";
            //openfile.Filter = "Excel Files (*.xls, *.xlsx)|*.xls;*.xlsx|CSV Files (*.csv)|*.csv";

            //var result = openfile.ShowDialog();
            //if (DialogResult.OK == result)
            //{
            //    MessageBox.Show("Work is under process.......!!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }

    

        private void btnDownloadData_Click(object sender, EventArgs e) 
        {

            MessageBox.Show("Work is Under Process");

        }
      
        public bool GetDataTableSummary(string jsonString)
        {
            bool flag;
            try
            {
                if (jsonString != string.Empty)
                {
                    RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(jsonString);
                    if ((rootObject == null ? false : rootObject.data != null))
                    {
                        if ((rootObject.data.msg == null ? true : !(rootObject.data.msg != "")))
                        {
                            MC.Open();
                            string strQuery = "";
                            strQuery = "Delete from tblGSTR9_Summary where Fld_GSTIN='" + CommonHelper.CompanyGSTN + "' and Fld_FinancialYear='" + CommonHelper.ReturnYear + "'";
                            MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                            MC.sqlcmd.ExecuteNonQuery();

                            if (rootObject.data.table4 != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_Summary " +
                                            "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                            " Fld_Description,Fld_TaxableValue,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_HeaderGroup) " +
                                            " Values('" +
                                            Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                            Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grdSummary.Rows[0].Cells[0].Value) + "','" +
                                            Convert.ToString(grdSummary.Rows[0].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.table4.txval) + "','" +
                                            Convert.ToString(rootObject.data.table4.camt) + "','" + Convert.ToString(rootObject.data.table4.samt) + "','" +
                                            Convert.ToString(rootObject.data.table4.iamt) + "','" + Convert.ToString(rootObject.data.table4.csamt) + "','" +
                                            Convert.ToString("Table4") +
                                            "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                            }
                            if (rootObject.data.table5 != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_Summary " +
                                            "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                            " Fld_Description,Fld_TaxableValue,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_HeaderGroup) " +
                                            " Values('" +
                                            Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                            Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grdSummary.Rows[1].Cells[0].Value) + "','" +
                                            Convert.ToString(grdSummary.Rows[1].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.table5.txval) + "','" +
                                            Convert.ToString(rootObject.data.table5.camt) + "','" + Convert.ToString(rootObject.data.table5.samt) + "','" +
                                            Convert.ToString(rootObject.data.table5.iamt) + "','" + Convert.ToString(rootObject.data.table5.csamt) + "','" +
                                            Convert.ToString("Table5") +
                                            "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                            }
                            if (rootObject.data.table6 != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_Summary " +
                                            "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                            " Fld_Description,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_HeaderGroup) " + //Fld_TaxableValue,
                                            " Values('" +
                                            Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                            Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grdSummary.Rows[2].Cells[0].Value) + "','" +
                                            Convert.ToString(grdSummary.Rows[2].Cells[1].Value) + "','" + //Convert.ToString(rootObject.data.table6.txval) + "','" +
                                            Convert.ToString(rootObject.data.table6.camt) + "','" + Convert.ToString(rootObject.data.table6.samt) + "','" +
                                            Convert.ToString(rootObject.data.table6.iamt) + "','" + Convert.ToString(rootObject.data.table6.csamt) + "','" +
                                            Convert.ToString("Table6") +
                                            "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                            }
                            if (rootObject.data.table7 != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_Summary " +
                                            "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                            " Fld_Description,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_HeaderGroup) " + //Fld_TaxableValue,
                                            " Values('" +
                                            Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                            Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grdSummary.Rows[3].Cells[0].Value) + "','" +
                                            Convert.ToString(grdSummary.Rows[3].Cells[1].Value) + "','" + //Convert.ToString(rootObject.data.table7.txval) + "','" +
                                            Convert.ToString(rootObject.data.table7.camt) + "','" + Convert.ToString(rootObject.data.table7.samt) + "','" +
                                            Convert.ToString(rootObject.data.table7.iamt) + "','" + Convert.ToString(rootObject.data.table7.csamt) + "','" +
                                            Convert.ToString("Table7") +
                                            "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                            }
                            if (rootObject.data.table8 != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_Summary " +
                                            "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                            " Fld_Description,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_HeaderGroup) " + //Fld_TaxableValue,
                                            " Values('" +
                                            Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                            Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grdSummary.Rows[4].Cells[0].Value) + "','" +
                                            Convert.ToString(grdSummary.Rows[4].Cells[1].Value) + "','" + //Convert.ToString(rootObject.data.table8.txval) + "','" +
                                            Convert.ToString(rootObject.data.table8.camt) + "','" + Convert.ToString(rootObject.data.table8.samt) + "','" +
                                            Convert.ToString(rootObject.data.table8.iamt) + "','" + Convert.ToString(rootObject.data.table8.csamt) + "','" +
                                           Convert.ToString("Table8") +
                                            "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                            }
                            if (rootObject.data.table9 != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_Summary " +
                                            "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                            " Fld_Description,Fld_TaxableValue,Fld_Cash,Fld_Itc,Fld_HeaderGroup) " + //Fld_TaxableValue,
                                            " Values('" +
                                            Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                            Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grdSummary.Rows[5].Cells[0].Value) + "','" +
                                            Convert.ToString(grdSummary.Rows[5].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.table9.txpyble) + "','" +
                                           // Convert.ToString(rootObject.data.table8.camt) + "','" + Convert.ToString(rootObject.data.table8.samt) + "','" +
                                            Convert.ToString(rootObject.data.table9.txpaid_cash) + "','" + Convert.ToString(rootObject.data.table9.txpaid_itc) + "','" +
                                           Convert.ToString("Table9") +
                                            "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Data older that 24 hours Request is being sent to regenerate the latest data. Please refresh the screen after 20 seconds");
                            flag = false;
                            return flag;
                        }
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                MessageBox.Show(string.Concat("Error : ", exception.Message), "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                object[] message = new object[] { exception.Message, Environment.NewLine, exception.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine };
                string str = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", message);
                StreamWriter streamWriter = new StreamWriter("PowerGSTError.txt", true);
                streamWriter.Write(str);
                streamWriter.Close();
                flag = false;
                return flag;
            }
            finally
            {
                MC.Close();
            }
            flag = true;
            return flag;
        }

        public bool GetDataTable4(string jsonString)
        {
            bool flag;
            try
            {
                if (jsonString != string.Empty)
                {
                    RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(jsonString);
                    if ((rootObject == null ? false : rootObject.data != null))
                    {
                        if ((rootObject.data.msg == null ? true : !(rootObject.data.msg != "")))
                        {
                            MC.Open();
                            string strQuery = "";
                            strQuery = "Delete from tblGSTR9_PtII_PtIII_PtV10_PtVI16 where Fld_HeaderGroup='PtII-4' and Fld_GSTIN='" + 
                                        CommonHelper.CompanyGSTN + "' and Fld_FinancialYear='" + CommonHelper.ReturnYear + "'";
                            MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                            MC.sqlcmd.ExecuteNonQuery();

                            if (rootObject.data.procRecord4.b2c != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                            "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                            " Fld_Description,Fld_TaxableValue,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " +
                                            " Values('" +
                                            Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                            Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd4_part2.Rows[0].Cells[0].Value) + "','" +
                                            Convert.ToString(grd4_part2.Rows[0].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord4.b2c.txval) + "','" +
                                            Convert.ToString(rootObject.data.procRecord4.b2c.camt) + "','" + Convert.ToString(rootObject.data.procRecord4.b2c.samt) + "','" +
                                            Convert.ToString(rootObject.data.procRecord4.b2c.iamt) + "','" + Convert.ToString(rootObject.data.procRecord4.b2c.csamt) + "','" +
                                            Convert.ToString(" ") + "','" + Convert.ToString("PtII-4") +
                                            "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                                //        this.dgv1Gov.Rows[0].Cells["Taxable Value"].Value = rootObject.data.procRecord4.b2c.txval;
                                //        this.dgv1Gov.Rows[0].Cells["Central Tax"].Value = rootObject.data.procRecord4.b2c.camt;
                                //        this.dgv1Gov.Rows[0].Cells["State Tax/UT Tax"].Value = rootObject.data.procRecord4.b2c.samt;
                                //        this.dgv1Gov.Rows[0].Cells["Integrated Tax"].Value = rootObject.data.procRecord4.b2c.iamt;
                                //        this.dgv1Gov.Rows[0].Cells["Cess"].Value = rootObject.data.procRecord4.b2c.csamt;

                            }
                                if (rootObject.data.procRecord4.b2b != null)
                                {
                                    strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                          "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                          " Fld_Description,Fld_TaxableValue,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " +
                                          " Values('" +
                                          Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                          Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd4_part2.Rows[1].Cells[0].Value) + "','" +
                                          Convert.ToString(grd4_part2.Rows[1].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord4.b2b.txval) + "','" +
                                          Convert.ToString(rootObject.data.procRecord4.b2b.camt) + "','" + Convert.ToString(rootObject.data.procRecord4.b2b.samt) + "','" +
                                          Convert.ToString(rootObject.data.procRecord4.b2b.iamt) + "','" + Convert.ToString(rootObject.data.procRecord4.b2b.csamt) + "','" +
                                          Convert.ToString(" ") + "','" + Convert.ToString("PtII-4") +
                                          "')";
                                    MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                    MC.sqlcmd.ExecuteNonQuery();
                            //        this.dgv1Gov.Rows[1].Cells["Taxable Value"].Value = rootObject.data.procRecord4.b2b.txval;
                            //        this.dgv1Gov.Rows[1].Cells["Central Tax"].Value = rootObject.data.procRecord4.b2b.camt;
                            //        this.dgv1Gov.Rows[1].Cells["State Tax/UT Tax"].Value = rootObject.data.procRecord4.b2b.samt;
                            //        this.dgv1Gov.Rows[1].Cells["Integrated Tax"].Value = rootObject.data.procRecord4.b2b.iamt;
                            //        this.dgv1Gov.Rows[1].Cells["Cess"].Value = rootObject.data.procRecord4.b2b.csamt;
                                }
                                if (rootObject.data.procRecord4.exp != null)
                                {
                                    strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                         "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                         " Fld_Description,Fld_TaxableValue, " +  // + "Fld_CGST,Fld_SGST,"
                                         " Fld_IGST, Fld_Cess,Fld_Type,Fld_HeaderGroup)  Values('" +
                                         Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                         Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd4_part2.Rows[2].Cells[0].Value) + "','" +
                                         Convert.ToString(grd4_part2.Rows[2].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord4.exp.txval) + "','" +
                                         //Convert.ToString(rootObject.data.procRecord4.exp.camt) + "','" + Convert.ToString(rootObject.data.procRecord4.exp.samt) + "','" +
                                         Convert.ToString(rootObject.data.procRecord4.exp.iamt) + "','" + Convert.ToString(rootObject.data.procRecord4.exp.csamt) + "','" +
                                         Convert.ToString(" ") + "','" + Convert.ToString("PtII-4") +
                                         "')";
                                    MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                    MC.sqlcmd.ExecuteNonQuery();
                            //        this.dgv1Gov.Rows[2].Cells["Taxable Value"].Value = rootObject.data.procRecord4.exp.txval;
                            //        this.dgv1Gov.Rows[2].Cells["Integrated Tax"].Value = rootObject.data.procRecord4.exp.iamt;
                            //        this.dgv1Gov.Rows[2].Cells["Cess"].Value = rootObject.data.procRecord4.exp.csamt;
                                }
                                if (rootObject.data.procRecord4.sez != null)
                                {
                                    strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                        "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                        " Fld_Description,Fld_TaxableValue, " +  // + "Fld_CGST,Fld_SGST,"
                                        " Fld_IGST, Fld_Cess,Fld_Type,Fld_HeaderGroup)  Values('" +
                                        Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                        Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd4_part2.Rows[3].Cells[0].Value) + "','" +
                                        Convert.ToString(grd4_part2.Rows[3].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord4.sez.txval) + "','" +
                                        //Convert.ToString(rootObject.data.procRecord4.sez.camt) + "','" + Convert.ToString(rootObject.data.procRecord4.sez.samt) + "','" +
                                        Convert.ToString(rootObject.data.procRecord4.sez.iamt) + "','" + Convert.ToString(rootObject.data.procRecord4.sez.csamt) + "','" +
                                        Convert.ToString(" ") + "','" + Convert.ToString("PtII-4") +
                                        "')";
                                    MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                    MC.sqlcmd.ExecuteNonQuery();

                                    //this.dgv1Gov.Rows[3].Cells["Taxable Value"].Value = rootObject.data.procRecord4.sez.txval;
                                    //this.dgv1Gov.Rows[3].Cells["Integrated Tax"].Value = rootObject.data.procRecord4.sez.iamt;
                                    //this.dgv1Gov.Rows[3].Cells["Cess"].Value = rootObject.data.procRecord4.sez.csamt;
                                }
                                if (rootObject.data.procRecord4.deemed != null)
                                {
                                    strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                         "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                         " Fld_Description,Fld_TaxableValue,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " +
                                         " Values('" +
                                         Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                         Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd4_part2.Rows[4].Cells[0].Value) + "','" +
                                         Convert.ToString(grd4_part2.Rows[4].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord4.deemed.txval) + "','" +
                                         Convert.ToString(rootObject.data.procRecord4.deemed.camt) + "','" + Convert.ToString(rootObject.data.procRecord4.deemed.samt) + "','" +
                                         Convert.ToString(rootObject.data.procRecord4.deemed.iamt) + "','" + Convert.ToString(rootObject.data.procRecord4.deemed.csamt) + "','" +
                                         Convert.ToString(" ") + "','" + Convert.ToString("PtII-4") +
                                         "')";
                                    MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                    MC.sqlcmd.ExecuteNonQuery();
                                    //this.dgv1Gov.Rows[4].Cells["Taxable Value"].Value = rootObject.data.procRecord4.deemed.txval;
                                    //this.dgv1Gov.Rows[4].Cells["Central Tax"].Value = rootObject.data.procRecord4.deemed.camt;
                                    //this.dgv1Gov.Rows[4].Cells["State Tax/UT Tax"].Value = rootObject.data.procRecord4.deemed.samt;
                                    //this.dgv1Gov.Rows[4].Cells["Integrated Tax"].Value = rootObject.data.procRecord4.deemed.iamt;
                                    //this.dgv1Gov.Rows[4].Cells["Cess"].Value = rootObject.data.procRecord4.deemed.csamt;
                                }
                                if (rootObject.data.procRecord4.at != null)
                                {
                                    strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                        "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                        " Fld_Description,Fld_TaxableValue,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " +
                                        " Values('" +
                                        Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                        Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd4_part2.Rows[5].Cells[0].Value) + "','" +
                                        Convert.ToString(grd4_part2.Rows[5].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord4.at.txval) + "','" +
                                        Convert.ToString(rootObject.data.procRecord4.at.camt) + "','" + Convert.ToString(rootObject.data.procRecord4.at.samt) + "','" +
                                        Convert.ToString(rootObject.data.procRecord4.at.iamt) + "','" + Convert.ToString(rootObject.data.procRecord4.at.csamt) + "','" +
                                        Convert.ToString(" ") + "','" + Convert.ToString("PtII-4") +
                                        "')";
                                    MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                    MC.sqlcmd.ExecuteNonQuery();
                                    
                                    //this.dgv1Gov.Rows[5].Cells["Taxable Value"].Value = rootObject.data.procRecord4.at.txval;
                                    //this.dgv1Gov.Rows[5].Cells["Central Tax"].Value = rootObject.data.procRecord4.at.camt;
                                    //this.dgv1Gov.Rows[5].Cells["State Tax/UT Tax"].Value = rootObject.data.procRecord4.at.samt;
                                    //this.dgv1Gov.Rows[5].Cells["Integrated Tax"].Value = rootObject.data.procRecord4.at.iamt;
                                    //this.dgv1Gov.Rows[5].Cells["Cess"].Value = rootObject.data.procRecord4.at.csamt;
                                }
                                if (rootObject.data.procRecord4.rchrg != null)
                                {
                                    strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                       "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                       " Fld_Description,Fld_TaxableValue,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " +
                                       " Values('" +
                                       Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                       Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd4_part2.Rows[6].Cells[0].Value) + "','" +
                                       Convert.ToString(grd4_part2.Rows[6].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord4.rchrg.txval) + "','" +
                                       Convert.ToString(rootObject.data.procRecord4.rchrg.camt) + "','" + Convert.ToString(rootObject.data.procRecord4.rchrg.samt) + "','" +
                                       Convert.ToString(rootObject.data.procRecord4.rchrg.iamt) + "','" + Convert.ToString(rootObject.data.procRecord4.rchrg.csamt) + "','" +
                                       Convert.ToString(" ") + "','" + Convert.ToString("PtII-4") +
                                       "')";
                                    MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                    MC.sqlcmd.ExecuteNonQuery();

                                    //this.dgv1Gov.Rows[6].Cells["Taxable Value"].Value = rootObject.data.procRecord4.rchrg.txval;
                                    //this.dgv1Gov.Rows[6].Cells["Central Tax"].Value = rootObject.data.procRecord4.rchrg.camt;
                                    //this.dgv1Gov.Rows[6].Cells["State Tax/UT Tax"].Value = rootObject.data.procRecord4.rchrg.samt;
                                    //this.dgv1Gov.Rows[6].Cells["Integrated Tax"].Value = rootObject.data.procRecord4.rchrg.iamt;
                                    //this.dgv1Gov.Rows[6].Cells["Cess"].Value = rootObject.data.procRecord4.rchrg.csamt;
                                }

                                if (rootObject.data.procRecord4.sub_totalAG != null)
                                {
                                    strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                      "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                      " Fld_Description,Fld_TaxableValue,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " +
                                      " Values('" +
                                      Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                      Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd4_part2.Rows[7].Cells[0].Value) + "','" +
                                      Convert.ToString(grd4_part2.Rows[7].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord4.sub_totalAG.txval) + "','" +
                                      Convert.ToString(rootObject.data.procRecord4.sub_totalAG.camt) + "','" + Convert.ToString(rootObject.data.procRecord4.sub_totalAG.samt) + "','" +
                                      Convert.ToString(rootObject.data.procRecord4.sub_totalAG.iamt) + "','" + Convert.ToString(rootObject.data.procRecord4.sub_totalAG.csamt) + "','" +
                                      Convert.ToString(" ") + "','" + Convert.ToString("PtII-4") +
                                      "')";
                                    MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                    MC.sqlcmd.ExecuteNonQuery();
                                    //this.dgv1Gov.Rows[7].Cells["Taxable Value"].Value = rootObject.data.procRecord4.sub_totalAG.txval;
                                    //this.dgv1Gov.Rows[7].Cells["Central Tax"].Value = rootObject.data.procRecord4.sub_totalAG.camt;
                                    //this.dgv1Gov.Rows[7].Cells["State Tax/UT Tax"].Value = rootObject.data.procRecord4.sub_totalAG.samt;
                                    //this.dgv1Gov.Rows[7].Cells["Integrated Tax"].Value = rootObject.data.procRecord4.sub_totalAG.iamt;
                                    //this.dgv1Gov.Rows[7].Cells["Cess"].Value = rootObject.data.procRecord4.sub_totalAG.csamt;
                                }

                                if (rootObject.data.procRecord4.cr_nt != null)
                                {
                                    strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                      "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                      " Fld_Description,Fld_TaxableValue,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " +
                                      " Values('" +
                                      Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                      Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd4_part2.Rows[8].Cells[0].Value) + "','" +
                                      Convert.ToString(grd4_part2.Rows[8].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord4.cr_nt.txval) + "','" +
                                      Convert.ToString(rootObject.data.procRecord4.cr_nt.camt) + "','" + Convert.ToString(rootObject.data.procRecord4.cr_nt.samt) + "','" +
                                      Convert.ToString(rootObject.data.procRecord4.cr_nt.iamt) + "','" + Convert.ToString(rootObject.data.procRecord4.cr_nt.csamt) + "','" +
                                      Convert.ToString(" ") + "','" + Convert.ToString("PtII-4") +
                                      "')";
                                    MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                    MC.sqlcmd.ExecuteNonQuery();
                                    //this.dgv1Gov.Rows[8].Cells["Taxable Value"].Value = rootObject.data.procRecord4.cr_nt.txval;
                                    //this.dgv1Gov.Rows[8].Cells["Central Tax"].Value = rootObject.data.procRecord4.cr_nt.camt;
                                    //this.dgv1Gov.Rows[8].Cells["State Tax/UT Tax"].Value = rootObject.data.procRecord4.cr_nt.samt;
                                    //this.dgv1Gov.Rows[8].Cells["Integrated Tax"].Value = rootObject.data.procRecord4.cr_nt.iamt;
                                    //this.dgv1Gov.Rows[8].Cells["Cess"].Value = rootObject.data.procRecord4.cr_nt.csamt;
                                }
                                if (rootObject.data.procRecord4.dr_nt != null)
                                {
                                    strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                     "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                     " Fld_Description,Fld_TaxableValue,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " +
                                     " Values('" +
                                     Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                     Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd4_part2.Rows[9].Cells[0].Value) + "','" +
                                     Convert.ToString(grd4_part2.Rows[9].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord4.sub_totalAG.txval) + "','" +
                                     Convert.ToString(rootObject.data.procRecord4.dr_nt.camt) + "','" + Convert.ToString(rootObject.data.procRecord4.dr_nt.samt) + "','" +
                                     Convert.ToString(rootObject.data.procRecord4.dr_nt.iamt) + "','" + Convert.ToString(rootObject.data.procRecord4.dr_nt.csamt) + "','" +
                                     Convert.ToString(" ") + "','" + Convert.ToString("PtII-4") +
                                     "')";
                                    MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                    MC.sqlcmd.ExecuteNonQuery();
                                    //this.dgv1Gov.Rows[9].Cells["Taxable Value"].Value = rootObject.data.procRecord4.dr_nt.txval;
                                    //this.dgv1Gov.Rows[9].Cells["Central Tax"].Value = rootObject.data.procRecord4.dr_nt.camt;
                                    //this.dgv1Gov.Rows[9].Cells["State Tax/UT Tax"].Value = rootObject.data.procRecord4.dr_nt.samt;
                                    //this.dgv1Gov.Rows[9].Cells["Integrated Tax"].Value = rootObject.data.procRecord4.dr_nt.iamt;
                                    //this.dgv1Gov.Rows[9].Cells["Cess"].Value = rootObject.data.procRecord4.dr_nt.csamt;
                                }
                                if (rootObject.data.procRecord4.amd_pos != null)
                                {
                                    strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                    "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                    " Fld_Description,Fld_TaxableValue,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " +
                                    " Values('" +
                                    Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                    Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd4_part2.Rows[10].Cells[0].Value) + "','" +
                                    Convert.ToString(grd4_part2.Rows[10].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord4.amd_pos.txval) + "','" +
                                    Convert.ToString(rootObject.data.procRecord4.amd_pos.camt) + "','" + Convert.ToString(rootObject.data.procRecord4.amd_pos.samt) + "','" +
                                    Convert.ToString(rootObject.data.procRecord4.amd_pos.iamt) + "','" + Convert.ToString(rootObject.data.procRecord4.amd_pos.csamt) + "','" +
                                    Convert.ToString(" ") + "','" + Convert.ToString("PtII-4") +
                                    "')";
                                    MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                    MC.sqlcmd.ExecuteNonQuery();

                                    //this.dgv1Gov.Rows[10].Cells["Taxable Value"].Value = rootObject.data.procRecord4.amd_pos.txval;
                                    //this.dgv1Gov.Rows[10].Cells["Central Tax"].Value = rootObject.data.procRecord4.amd_pos.camt;
                                    //this.dgv1Gov.Rows[10].Cells["State Tax/UT Tax"].Value = rootObject.data.procRecord4.amd_pos.samt;
                                    //this.dgv1Gov.Rows[10].Cells["Integrated Tax"].Value = rootObject.data.procRecord4.amd_pos.iamt;
                                    //this.dgv1Gov.Rows[10].Cells["Cess"].Value = rootObject.data.procRecord4.amd_pos.csamt;
                                }
                                if (rootObject.data.procRecord4.amd_neg != null)
                                {
                                    strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                   "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                   " Fld_Description,Fld_TaxableValue,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " +
                                   " Values('" +
                                   Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                   Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd4_part2.Rows[11].Cells[0].Value) + "','" +
                                   Convert.ToString(grd4_part2.Rows[11].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord4.amd_neg.txval) + "','" +
                                   Convert.ToString(rootObject.data.procRecord4.amd_neg.camt) + "','" + Convert.ToString(rootObject.data.procRecord4.amd_neg.samt) + "','" +
                                   Convert.ToString(rootObject.data.procRecord4.amd_neg.iamt) + "','" + Convert.ToString(rootObject.data.procRecord4.amd_neg.csamt) + "','" +
                                   Convert.ToString(" ") + "','" + Convert.ToString("PtII-4") +
                                   "')";
                                    MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                    MC.sqlcmd.ExecuteNonQuery();

                                    //this.dgv1Gov.Rows[11].Cells["Taxable Value"].Value = rootObject.data.procRecord4.amd_neg.txval;
                                    //this.dgv1Gov.Rows[11].Cells["Central Tax"].Value = rootObject.data.procRecord4.amd_neg.camt;
                                    //this.dgv1Gov.Rows[11].Cells["State Tax/UT Tax"].Value = rootObject.data.procRecord4.amd_neg.samt;
                                    //this.dgv1Gov.Rows[11].Cells["Integrated Tax"].Value = rootObject.data.procRecord4.amd_neg.iamt;
                                    //this.dgv1Gov.Rows[11].Cells["Cess"].Value = rootObject.data.procRecord4.amd_neg.csamt;
                                }

                                if (rootObject.data.procRecord4.sub_totalIL != null)
                                {
                                    strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                  "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                  " Fld_Description,Fld_TaxableValue,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " +
                                  " Values('" +
                                  Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                  Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd4_part2.Rows[12].Cells[0].Value) + "','" +
                                  Convert.ToString(grd4_part2.Rows[12].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord4.sub_totalIL.txval) + "','" +
                                  Convert.ToString(rootObject.data.procRecord4.sub_totalIL.camt) + "','" + Convert.ToString(rootObject.data.procRecord4.sub_totalIL.samt) + "','" +
                                  Convert.ToString(rootObject.data.procRecord4.sub_totalIL.iamt) + "','" + Convert.ToString(rootObject.data.procRecord4.sub_totalIL.csamt) + "','" +
                                  Convert.ToString(" ") + "','" + Convert.ToString("PtII-4") +
                                  "')";
                                    MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                    MC.sqlcmd.ExecuteNonQuery();

                                    //this.dgv1Gov.Rows[12].Cells["Taxable Value"].Value = rootObject.data.procRecord4.sub_totalIL.txval;
                                    //this.dgv1Gov.Rows[12].Cells["Central Tax"].Value = rootObject.data.procRecord4.sub_totalIL.camt;
                                    //this.dgv1Gov.Rows[12].Cells["State Tax/UT Tax"].Value = rootObject.data.procRecord4.sub_totalIL.samt;
                                    //this.dgv1Gov.Rows[12].Cells["Integrated Tax"].Value = rootObject.data.procRecord4.sub_totalIL.iamt;
                                    //this.dgv1Gov.Rows[12].Cells["Cess"].Value = rootObject.data.procRecord4.sub_totalIL.csamt;
                                }
                                if (rootObject.data.procRecord4.sup_adv != null)
                                {
                                    strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                " Fld_Description,Fld_TaxableValue,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " +
                                " Values('" +
                                Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd4_part2.Rows[13].Cells[0].Value) + "','" +
                                Convert.ToString(grd4_part2.Rows[13].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord4.sup_adv.txval) + "','" +
                                Convert.ToString(rootObject.data.procRecord4.sup_adv.camt) + "','" + Convert.ToString(rootObject.data.procRecord4.sup_adv.samt) + "','" +
                                Convert.ToString(rootObject.data.procRecord4.sup_adv.iamt) + "','" + Convert.ToString(rootObject.data.procRecord4.sup_adv.csamt) + "','" +
                                Convert.ToString(" ") + "','" + Convert.ToString("PtII-4") +
                                "')";
                                    MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                    MC.sqlcmd.ExecuteNonQuery();
                                    //this.dgv1Gov.Rows[13].Cells["Taxable Value"].Value = rootObject.data.procRecord4.sup_adv.txval;
                                    //this.dgv1Gov.Rows[13].Cells["Central Tax"].Value = rootObject.data.procRecord4.sup_adv.camt;
                                    //this.dgv1Gov.Rows[13].Cells["State Tax/UT Tax"].Value = rootObject.data.procRecord4.sup_adv.samt;
                                    //this.dgv1Gov.Rows[13].Cells["Integrated Tax"].Value = rootObject.data.procRecord4.sup_adv.iamt;
                                    //this.dgv1Gov.Rows[13].Cells["Cess"].Value = rootObject.data.procRecord4.sup_adv.csamt;
                                }
                        }
                        else
                        {
                            MessageBox.Show("Data older that 24 hours Request is being sent to regenerate the latest data. Please refresh the screen after 20 seconds");
                            flag = false;
                            return flag;
                        }
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                MessageBox.Show(string.Concat("Error : ", exception.Message), "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                object[] message = new object[] { exception.Message, Environment.NewLine, exception.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine };
                string str = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", message);
                StreamWriter streamWriter = new StreamWriter("PowerGSTError.txt", true);
                streamWriter.Write(str);
                streamWriter.Close();
                flag = false;
                return flag;
            }
            finally
            {
                MC.Close();
            }
            flag = true;
            return flag;
        }

        public bool GetDataTable5(string jsonString)
        {
            bool flag;
            string strQuery = "";
            try
            {
                if (jsonString != string.Empty)
                {
                    RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(jsonString);
                    if ((rootObject == null ? false : rootObject.data != null))
                    {
                        if ((rootObject.data.msg == null ? true : !(rootObject.data.msg != "")))
                        {
                            MC.Open();
                            strQuery = "Delete from tblGSTR9_PtII_PtIII_PtV10_PtVI16 where Fld_HeaderGroup='PtII-5' and Fld_GSTIN='" + 
                                        CommonHelper.CompanyGSTN + "' and Fld_FinancialYear='" + CommonHelper.ReturnYear + "'";
                            MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                            MC.sqlcmd.ExecuteNonQuery();
                            strQuery = " ";

                            if (rootObject.data.procRecord5.zero_rtd != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                 "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                 " Fld_Description,Fld_TaxableValue,Fld_Type,Fld_HeaderGroup) " + //Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,
                                 " Values('" +
                                 Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                 Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd5_part2.Rows[0].Cells[0].Value) + "','" +
                                 Convert.ToString(grd5_part2.Rows[0].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord5.zero_rtd.txval) + "','" +  //Convert.ToString(rootObject.data.procRecord5.zero_rtd.camt) + "','" + Convert.ToString(rootObject.data.procRecord5.zero_rtd.samt) + "','" +//Convert.ToString(rootObject.data.procRecord5.zero_rtd.iamt) + "','" + Convert.ToString(rootObject.data.procRecord5.zero_rtd.csamt) + "','" +
                                 Convert.ToString(" ") + "','" + Convert.ToString("PtII-5") +
                                 "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                                // this.dgv2Gov.Rows[0].Cells["Taxable Value"].Value = rootObject.data.procRecord5.zero_rtd.txval;
                            }
                            if (rootObject.data.procRecord5.sez != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                 "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                 " Fld_Description,Fld_TaxableValue,Fld_Type,Fld_HeaderGroup) " + //Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,
                                 " Values('" +
                                 Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                 Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd5_part2.Rows[1].Cells[0].Value) + "','" +
                                 Convert.ToString(grd5_part2.Rows[1].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord5.sez.txval) + "','" +  //Convert.ToString(rootObject.data.procRecord5.zero_rtd.camt) + "','" + Convert.ToString(rootObject.data.procRecord5.zero_rtd.samt) + "','" +//Convert.ToString(rootObject.data.procRecord5.zero_rtd.iamt) + "','" + Convert.ToString(rootObject.data.procRecord5.zero_rtd.csamt) + "','" +
                                 Convert.ToString(" ") + "','" + Convert.ToString("PtII-5") +
                                 "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                                //this.dgv2Gov.Rows[1].Cells["Taxable Value"].Value = rootObject.data.procRecord5.sez.txval;
                            }
                            if (rootObject.data.procRecord5.rchrg != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                 "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                 " Fld_Description,Fld_TaxableValue,Fld_Type,Fld_HeaderGroup) " + //Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,
                                 " Values('" +
                                 Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                 Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd5_part2.Rows[2].Cells[0].Value) + "','" +
                                 Convert.ToString(grd5_part2.Rows[2].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord5.rchrg.txval) + "','" +  //Convert.ToString(rootObject.data.procRecord5.zero_rtd.camt) + "','" + Convert.ToString(rootObject.data.procRecord5.zero_rtd.samt) + "','" +//Convert.ToString(rootObject.data.procRecord5.zero_rtd.iamt) + "','" + Convert.ToString(rootObject.data.procRecord5.zero_rtd.csamt) + "','" +
                                 Convert.ToString(" ") + "','" + Convert.ToString("PtII-5") +
                                 "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                                // this.dgv2Gov.Rows[2].Cells["Taxable Value"].Value = rootObject.data.procRecord5.rchrg.txval;
                            }
                            if (rootObject.data.procRecord5.exmt != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                 "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                 " Fld_Description,Fld_TaxableValue,Fld_Type,Fld_HeaderGroup) " + //Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,
                                 " Values('" +
                                 Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                 Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd5_part2.Rows[3].Cells[0].Value) + "','" +
                                 Convert.ToString(grd5_part2.Rows[3].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord5.exmt.txval) + "','" +  //Convert.ToString(rootObject.data.procRecord5.zero_rtd.camt) + "','" + Convert.ToString(rootObject.data.procRecord5.zero_rtd.samt) + "','" +//Convert.ToString(rootObject.data.procRecord5.zero_rtd.iamt) + "','" + Convert.ToString(rootObject.data.procRecord5.zero_rtd.csamt) + "','" +
                                 Convert.ToString(" ") + "','" + Convert.ToString("PtII-5") +
                                 "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                                //this.dgv2Gov.Rows[3].Cells["Taxable Value"].Value = rootObject.data.procRecord5.exmt.txval;
                            }
                            if (rootObject.data.procRecord5.nil != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                " Fld_Description,Fld_TaxableValue,Fld_Type,Fld_HeaderGroup) " + //Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,
                                " Values('" +
                                Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd5_part2.Rows[4].Cells[0].Value) + "','" +
                                Convert.ToString(grd5_part2.Rows[4].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord5.nil.txval) + "','" +  //Convert.ToString(rootObject.data.procRecord5.zero_rtd.camt) + "','" + Convert.ToString(rootObject.data.procRecord5.zero_rtd.samt) + "','" +//Convert.ToString(rootObject.data.procRecord5.zero_rtd.iamt) + "','" + Convert.ToString(rootObject.data.procRecord5.zero_rtd.csamt) + "','" +
                                Convert.ToString(" ") + "','" + Convert.ToString("PtII-5") +
                                "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                                //this.dgv2Gov.Rows[4].Cells["Taxable Value"].Value = rootObject.data.procRecord5.nil.txval;
                            }
                            if (rootObject.data.procRecord5.non_gst != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                              "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                              " Fld_Description,Fld_TaxableValue,Fld_Type,Fld_HeaderGroup) " + //Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,
                              " Values('" +
                              Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                              Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd5_part2.Rows[5].Cells[0].Value) + "','" +
                              Convert.ToString(grd5_part2.Rows[5].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord5.non_gst.txval) + "','" +  //Convert.ToString(rootObject.data.procRecord5.zero_rtd.camt) + "','" + Convert.ToString(rootObject.data.procRecord5.zero_rtd.samt) + "','" +//Convert.ToString(rootObject.data.procRecord5.zero_rtd.iamt) + "','" + Convert.ToString(rootObject.data.procRecord5.zero_rtd.csamt) + "','" +
                              Convert.ToString(" ") + "','" + Convert.ToString("PtII-5") +
                              "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                                //this.dgv2Gov.Rows[5].Cells["Taxable Value"].Value = rootObject.data.procRecord5.non_gst.txval;
                            }
                            if (rootObject.data.procRecord5.sub_totalAF != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                             "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                             " Fld_Description,Fld_TaxableValue,Fld_Type,Fld_HeaderGroup) " + //Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,
                             " Values('" +
                             Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                             Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd5_part2.Rows[6].Cells[0].Value) + "','" +
                             Convert.ToString(grd5_part2.Rows[6].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord5.sub_totalAF.txval) + "','" +  //Convert.ToString(rootObject.data.procRecord5.zero_rtd.camt) + "','" + Convert.ToString(rootObject.data.procRecord5.zero_rtd.samt) + "','" +//Convert.ToString(rootObject.data.procRecord5.zero_rtd.iamt) + "','" + Convert.ToString(rootObject.data.procRecord5.zero_rtd.csamt) + "','" +
                             Convert.ToString(" ") + "','" + Convert.ToString("PtII-5") +
                             "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                                //  this.dgv2Gov.Rows[6].Cells["Taxable Value"].Value = rootObject.data.procRecord5.sub_totalAF.txval;
                            }
                            if (rootObject.data.procRecord5.cr_nt != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                               "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                               " Fld_Description,Fld_TaxableValue,Fld_Type,Fld_HeaderGroup) " + //Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,
                               " Values('" +
                               Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                               Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd5_part2.Rows[7].Cells[0].Value) + "','" +
                               Convert.ToString(grd5_part2.Rows[7].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord5.cr_nt.txval) + "','" +  //Convert.ToString(rootObject.data.procRecord5.zero_rtd.camt) + "','" + Convert.ToString(rootObject.data.procRecord5.zero_rtd.samt) + "','" +//Convert.ToString(rootObject.data.procRecord5.zero_rtd.iamt) + "','" + Convert.ToString(rootObject.data.procRecord5.zero_rtd.csamt) + "','" +
                               Convert.ToString(" ") + "','" + Convert.ToString("PtII-5") +
                               "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                                //this.dgv2Gov.Rows[7].Cells["Taxable Value"].Value = rootObject.data.procRecord5.cr_nt.txval;
                            }
                            if (rootObject.data.procRecord5.dr_nt != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                             "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                             " Fld_Description,Fld_TaxableValue,Fld_Type,Fld_HeaderGroup) " + //Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,
                             " Values('" +
                             Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                             Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd5_part2.Rows[8].Cells[0].Value) + "','" +
                             Convert.ToString(grd5_part2.Rows[8].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord5.dr_nt.txval) + "','" +  //Convert.ToString(rootObject.data.procRecord5.zero_rtd.camt) + "','" + Convert.ToString(rootObject.data.procRecord5.zero_rtd.samt) + "','" +//Convert.ToString(rootObject.data.procRecord5.zero_rtd.iamt) + "','" + Convert.ToString(rootObject.data.procRecord5.zero_rtd.csamt) + "','" +
                             Convert.ToString(" ") + "','" + Convert.ToString("PtII-5") +
                             "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                                //this.dgv2Gov.Rows[8].Cells["Taxable Value"].Value = rootObject.data.procRecord5.dr_nt.txval;
                            }
                            if (rootObject.data.procRecord5.amd_pos != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                            "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                            " Fld_Description,Fld_TaxableValue,Fld_Type,Fld_HeaderGroup) " + //Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,
                            " Values('" +
                            Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                            Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd5_part2.Rows[9].Cells[0].Value) + "','" +
                            Convert.ToString(grd5_part2.Rows[9].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord5.amd_pos.txval) + "','" +  //Convert.ToString(rootObject.data.procRecord5.zero_rtd.camt) + "','" + Convert.ToString(rootObject.data.procRecord5.zero_rtd.samt) + "','" +//Convert.ToString(rootObject.data.procRecord5.zero_rtd.iamt) + "','" + Convert.ToString(rootObject.data.procRecord5.zero_rtd.csamt) + "','" +
                            Convert.ToString(" ") + "','" + Convert.ToString("PtII-5") +
                            "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                                //  this.dgv2Gov.Rows[9].Cells["Taxable Value"].Value = rootObject.data.procRecord5.amd_pos.txval;
                            }
                            if (rootObject.data.procRecord5.amd_neg != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                           "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                           " Fld_Description,Fld_TaxableValue,Fld_Type,Fld_HeaderGroup) " + //Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,
                           " Values('" +
                           Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                           Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd5_part2.Rows[10].Cells[0].Value) + "','" +
                           Convert.ToString(grd5_part2.Rows[10].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord5.amd_neg.txval) + "','" +  //Convert.ToString(rootObject.data.procRecord5.zero_rtd.camt) + "','" + Convert.ToString(rootObject.data.procRecord5.zero_rtd.samt) + "','" +//Convert.ToString(rootObject.data.procRecord5.zero_rtd.iamt) + "','" + Convert.ToString(rootObject.data.procRecord5.zero_rtd.csamt) + "','" +
                           Convert.ToString(" ") + "','" + Convert.ToString("PtII-5") +
                           "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                                // this.dgv2Gov.Rows[10].Cells["Taxable Value"].Value = rootObject.data.procRecord5.amd_neg.txval;
                            }
                            if (rootObject.data.procRecord5.sub_totalHK != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                          "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                          " Fld_Description,Fld_TaxableValue,Fld_Type,Fld_HeaderGroup) " + //Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,
                          " Values('" +
                          Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                          Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd5_part2.Rows[11].Cells[0].Value) + "','" +
                          Convert.ToString(grd5_part2.Rows[11].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord5.sub_totalHK.txval) + "','" +  //Convert.ToString(rootObject.data.procRecord5.zero_rtd.camt) + "','" + Convert.ToString(rootObject.data.procRecord5.zero_rtd.samt) + "','" +//Convert.ToString(rootObject.data.procRecord5.zero_rtd.iamt) + "','" + Convert.ToString(rootObject.data.procRecord5.zero_rtd.csamt) + "','" +
                          Convert.ToString(" ") + "','" + Convert.ToString("PtII-5") +
                          "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                                // this.dgv2Gov.Rows[11].Cells["Taxable Value"].Value = rootObject.data.procRecord5.sub_totalHK.txval;
                            }
                            if (rootObject.data.procRecord5.tover_tax_np != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                         "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                         " Fld_Description,Fld_TaxableValue,Fld_Type,Fld_HeaderGroup) " + //Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,
                         " Values('" +
                         Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                         Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd5_part2.Rows[12].Cells[0].Value) + "','" +
                         Convert.ToString(grd5_part2.Rows[12].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord5.tover_tax_np.txval) + "','" +  //Convert.ToString(rootObject.data.procRecord5.zero_rtd.camt) + "','" + Convert.ToString(rootObject.data.procRecord5.zero_rtd.samt) + "','" +//Convert.ToString(rootObject.data.procRecord5.zero_rtd.iamt) + "','" + Convert.ToString(rootObject.data.procRecord5.zero_rtd.csamt) + "','" +
                         Convert.ToString(" ") + "','" + Convert.ToString("PtII-5") +
                         "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                                // this.dgv2Gov.Rows[12].Cells["Taxable Value"].Value = rootObject.data.procRecord5.tover_tax_np.txval;
                            }
                            if (rootObject.data.procRecord5.total_tover != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                         "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                         " Fld_Description,Fld_TaxableValue,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " + //Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,
                         " Values('" +
                         Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                         Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd5_part2.Rows[13].Cells[0].Value) + "','" +
                         Convert.ToString(grd5_part2.Rows[13].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord5.total_tover.txval) + "','" +
                         Convert.ToString(rootObject.data.procRecord5.total_tover.camt) + "','" + Convert.ToString(rootObject.data.procRecord5.total_tover.samt) + "','" +
                         Convert.ToString(rootObject.data.procRecord5.total_tover.iamt) + "','" + Convert.ToString(rootObject.data.procRecord5.total_tover.csamt) + "','" +
                         Convert.ToString(" ") + "','" + Convert.ToString("PtII-5") +
                         "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                                //this.dgv2Gov.Rows[13].Cells["Taxable Value"].Value = rootObject.data.procRecord5.total_tover.txval;
                                //this.dgv2Gov.Rows[13].Cells["Central Tax"].Value = rootObject.data.procRecord5.total_tover.camt;
                                //this.dgv2Gov.Rows[13].Cells["State Tax/UT Tax"].Value = rootObject.data.procRecord5.total_tover.samt;
                                //this.dgv2Gov.Rows[13].Cells["Integrated Tax"].Value = rootObject.data.procRecord5.total_tover.iamt;
                                //this.dgv2Gov.Rows[13].Cells["Cess"].Value = rootObject.data.procRecord5.total_tover.csamt;
                            }

                        }
                        else
                        {
                            MessageBox.Show("Data older that 24 hours Request is being sent to regenerate the latest data. Please refresh the screen after 20 seconds");
                            flag = false;
                            return flag;
                        }
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                MessageBox.Show(string.Concat("Error : ", exception.Message), "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                object[] message = new object[] { exception.Message, Environment.NewLine, exception.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine };
                string str = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", message);
                StreamWriter streamWriter = new StreamWriter("PowerGSTError.txt", true);
                streamWriter.Write(str);
                streamWriter.Close();
                flag = false;
                return flag;
            }
            finally
            {
                MC.Close();
            }
            flag = true;
            return flag;
        }

        public bool GetDataTable6(string jsonString)
        {
            bool flag;
            string strQuery = "";
            try
            {
                if (jsonString != string.Empty)
                {
                    RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(jsonString);
                    if ((rootObject == null ? false : rootObject.data != null))
                    {
                        if ((rootObject.data.msg == null ? true : !(rootObject.data.msg != "")))
                        {
                            MC.Open();
                            strQuery = "Delete from tblGSTR9_PtII_PtIII_PtV10_PtVI16 where Fld_HeaderGroup='PtII-6' and Fld_GSTIN='" + CommonHelper.CompanyGSTN + "' and Fld_FinancialYear='" + CommonHelper.ReturnYear + "'";
                            MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                            MC.sqlcmd.ExecuteNonQuery();
                            strQuery = " ";

                            if (rootObject.data.procRecord6.itc_3b != null)  // supp_non_rchrg
                            {
                               
                              strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                               "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                               " Fld_Description,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,
                               " Values('" +
                               Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                               Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd6_part3.Rows[0].Cells[0].Value) + "','" +
                               Convert.ToString(grd6_part3.Rows[0].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord6.itc_3b.txval) + "','" +
                               Convert.ToString(rootObject.data.procRecord6.itc_3b.camt) + "','" + Convert.ToString(rootObject.data.procRecord6.itc_3b.samt) + "','" +
                               Convert.ToString(rootObject.data.procRecord6.itc_3b.iamt) + "','" + Convert.ToString(rootObject.data.procRecord6.itc_3b.csamt) + "','" +
                               Convert.ToString(grd6_part3.Rows[0].Cells[2].Value=="") + "','" + Convert.ToString("PtIII-6") +
                               "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                                //this.dgv3Gov.Rows[0].Cells["Central Tax"].Value = rootObject.data.procRecord6.itc_3b.camt;
                                //this.dgv3Gov.Rows[0].Cells["State Tax/UT Tax"].Value = rootObject.data.procRecord6.itc_3b.samt;
                                //this.dgv3Gov.Rows[0].Cells["Integrated Tax"].Value = rootObject.data.procRecord6.itc_3b.iamt;
                                //this.dgv3Gov.Rows[0].Cells["Cess"].Value = rootObject.data.procRecord6.itc_3b.csamt;
                            }
                            if (rootObject.data.procRecord6.supp_non_rchrg != null)
                            {
                                for (int i = 0; i < rootObject.data.procRecord6.supp_non_rchrg.Count; i++)
                                {
                                    string strtype = Convert.ToString(rootObject.data.procRecord6.supp_non_rchrg[i].itc_typ) == "cg" ? "Capital Goods" : (Convert.ToString(rootObject.data.procRecord6.supp_non_rchrg[i].itc_typ) == "is" ? "Input Services" : "Inputs");

                                    strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                  "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                  " Fld_Description,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,
                                  " Values('" +
                                  Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                  Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd6_part3.Rows[1].Cells[0].Value) + "','" +
                                  Convert.ToString(grd6_part3.Rows[1].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord6.itc_3b.txval) + "','" +
                                  Convert.ToString(rootObject.data.procRecord6.supp_non_rchrg[i].camt) + "','" + Convert.ToString(rootObject.data.procRecord6.supp_non_rchrg[i].samt) + "','" +
                                  Convert.ToString(rootObject.data.procRecord6.supp_non_rchrg[i].iamt) + "','" + Convert.ToString(rootObject.data.procRecord6.supp_non_rchrg[i].csamt) + "','" +
                                  Convert.ToString(strtype) + "','" + Convert.ToString("PtIII-6") +
                                  "')";
                                    MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                    MC.sqlcmd.ExecuteNonQuery();
                                }
                            }

                            if (rootObject.data.procRecord6.supp_rchrg_unreg != null)
                            {
                                for (int i = 0; i < rootObject.data.procRecord6.supp_rchrg_unreg.Count; i++)
                                {
                                    string strtype = Convert.ToString(rootObject.data.procRecord6.supp_non_rchrg[i].itc_typ) == "cg" ? "Capital Goods" : (Convert.ToString(rootObject.data.procRecord6.supp_non_rchrg[i].itc_typ) == "is" ? "Input Services" : "Inputs");
                                    strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                    "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                    " Fld_Description,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,
                                    " Values('" +
                                    Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                    Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd6_part3.Rows[4].Cells[0].Value) + "','" +
                                    Convert.ToString(grd6_part3.Rows[4].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord6.itc_3b.txval) + "','" +
                                    Convert.ToString(rootObject.data.procRecord6.supp_rchrg_unreg[i].camt) + "','" + Convert.ToString(rootObject.data.procRecord6.supp_rchrg_unreg[i].samt) + "','" +
                                    Convert.ToString(rootObject.data.procRecord6.supp_rchrg_unreg[i].iamt) + "','" + Convert.ToString(rootObject.data.procRecord6.supp_rchrg_unreg[i].csamt) + "','" +
                                    Convert.ToString(strtype) + "','" + Convert.ToString("PtIII-6") +
                                    "')";
                                    MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                    MC.sqlcmd.ExecuteNonQuery();
                                }
                            }

                            if (rootObject.data.procRecord6.supp_rchrg_reg != null)
                            {
                                for (int i = 0; i < rootObject.data.procRecord6.supp_rchrg_reg.Count; i++)
                                {
                                    string strtype = Convert.ToString(rootObject.data.procRecord6.supp_non_rchrg[i].itc_typ) == "cg" ? "Capital Goods" : (Convert.ToString(rootObject.data.procRecord6.supp_non_rchrg[i].itc_typ) == "is" ? "Input Services" : "Inputs");
                                    strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                    "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                    " Fld_Description,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,
                                    " Values('" +
                                    Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                    Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd6_part3.Rows[7].Cells[0].Value) + "','" +
                                    Convert.ToString(grd6_part3.Rows[7].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord6.itc_3b.txval) + "','" +
                                    Convert.ToString(rootObject.data.procRecord6.supp_rchrg_reg[i].camt) + "','" + Convert.ToString(rootObject.data.procRecord6.supp_rchrg_reg[i].samt) + "','" +
                                    Convert.ToString(rootObject.data.procRecord6.supp_rchrg_reg[i].iamt) + "','" + Convert.ToString(rootObject.data.procRecord6.supp_rchrg_reg[i].csamt) + "','" +
                                    Convert.ToString(strtype) + "','" + Convert.ToString("PtIII-6") +
                                    "')";
                                    MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                    MC.sqlcmd.ExecuteNonQuery();
                                }
                            }

                            if (rootObject.data.procRecord6.iog != null)
                            {
                                for (int i = 0; i < rootObject.data.procRecord6.iog.Count; i++)
                                {
                                    string strtype = Convert.ToString(rootObject.data.procRecord6.supp_non_rchrg[i].itc_typ) == "cg" ? "Capital Goods" : (Convert.ToString(rootObject.data.procRecord6.supp_non_rchrg[i].itc_typ) == "is" ? "Input Services" : "Inputs");
                                    strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                    "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                    " Fld_Description,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue, Fld_CGST,Fld_SGST,
                                    " Values('" +
                                    Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                    Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd6_part3.Rows[10].Cells[0].Value) + "','" +
                                    Convert.ToString(grd6_part3.Rows[10].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord6.itc_3b.txval) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord6.iog[i].camt) + "','" + Convert.ToString(rootObject.data.procRecord6.iog[i].samt) + "','" +
                                    Convert.ToString(rootObject.data.procRecord6.iog[i].iamt) + "','" + Convert.ToString(rootObject.data.procRecord6.iog[i].csamt) + "','" +
                                    Convert.ToString(strtype) + "','" + Convert.ToString("PtIII-6") +
                                    "')";
                                    MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                    MC.sqlcmd.ExecuteNonQuery();
                                }
                            }
                            if (rootObject.data.procRecord6.ios != null)
                            {

                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                " Fld_Description,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue, Fld_CGST,Fld_SGST,
                                " Values('" +
                                Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd6_part3.Rows[12].Cells[0].Value) + "','" +
                                Convert.ToString(grd6_part3.Rows[12].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord6.itc_3b.txval) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord6.iog[i].camt) + "','" + Convert.ToString(rootObject.data.procRecord6.iog[i].samt) + "','" +
                                Convert.ToString(rootObject.data.procRecord6.ios.iamt) + "','" + Convert.ToString(rootObject.data.procRecord6.ios.csamt) + "','" +
                                Convert.ToString("") + "','" + Convert.ToString("PtIII-6") +
                                "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();

                            }

                            if (rootObject.data.procRecord6.isd != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                   "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                   " Fld_Description,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,
                                   " Values('" +
                                   Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                   Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd6_part3.Rows[13].Cells[0].Value) + "','" +
                                   Convert.ToString(grd6_part3.Rows[13].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord6.itc_3b.txval) + "','" +
                                   Convert.ToString(rootObject.data.procRecord6.isd.camt) + "','" + Convert.ToString(rootObject.data.procRecord6.isd.samt) + "','" +
                                   Convert.ToString(rootObject.data.procRecord6.isd.iamt) + "','" + Convert.ToString(rootObject.data.procRecord6.isd.csamt) + "','" +
                                   Convert.ToString("") + "','" + Convert.ToString("PtIII-6") +
                                   "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                                //this.dgv3Gov.Rows[13].Cells["Central Tax"].Value = rootObject.data.procRecord6.isd.camt;
                                //this.dgv3Gov.Rows[13].Cells["State Tax/UT Tax"].Value = rootObject.data.procRecord6.isd.samt;
                                //this.dgv3Gov.Rows[13].Cells["Integrated Tax"].Value = rootObject.data.procRecord6.isd.iamt;
                                //this.dgv3Gov.Rows[13].Cells["Cess"].Value = rootObject.data.procRecord6.isd.csamt;
                            }

                            if (rootObject.data.procRecord6.itc_clmd != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                   "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                   " Fld_Description,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,
                                   " Values('" +
                                   Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                   Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd6_part3.Rows[14].Cells[0].Value) + "','" +
                                   Convert.ToString(grd6_part3.Rows[14].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord6.itc_3b.txval) + "','" +
                                   Convert.ToString(rootObject.data.procRecord6.itc_clmd.camt) + "','" + Convert.ToString(rootObject.data.procRecord6.itc_clmd.samt) + "','" +
                                   Convert.ToString(rootObject.data.procRecord6.itc_clmd.iamt) + "','" + Convert.ToString(rootObject.data.procRecord6.itc_clmd.csamt) + "','" +
                                   Convert.ToString("") + "','" + Convert.ToString("PtIII-6") +
                                   "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                                //this.dgv3Gov.Rows[14].Cells["Central Tax"].Value = rootObject.data.procRecord6.itc_clmd.camt;
                                //this.dgv3Gov.Rows[14].Cells["State Tax/UT Tax"].Value = rootObject.data.procRecord6.itc_clmd.samt;
                                //this.dgv3Gov.Rows[14].Cells["Integrated Tax"].Value = rootObject.data.procRecord6.itc_clmd.iamt;
                                //this.dgv3Gov.Rows[14].Cells["Cess"].Value = rootObject.data.procRecord6.itc_clmd.csamt;
                            }

                            if (rootObject.data.procRecord6.sub_totalBH != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                  "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                  " Fld_Description,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,
                                  " Values('" +
                                  Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                  Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd6_part3.Rows[15].Cells[0].Value) + "','" +
                                  Convert.ToString(grd6_part3.Rows[15].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord6.sub_totalBH.txval) + "','" +
                                  Convert.ToString(rootObject.data.procRecord6.sub_totalBH.camt) + "','" + Convert.ToString(rootObject.data.procRecord6.sub_totalBH.samt) + "','" +
                                  Convert.ToString(rootObject.data.procRecord6.sub_totalBH.iamt) + "','" + Convert.ToString(rootObject.data.procRecord6.sub_totalBH.csamt) + "','" +
                                  Convert.ToString("") + "','" + Convert.ToString("PtIII-6") +
                                  "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();

                                //this.dgv3Gov.Rows[15].Cells["Central Tax"].Value = rootObject.data.procRecord6.sub_totalBH.camt;
                                //this.dgv3Gov.Rows[15].Cells["State Tax/UT Tax"].Value = rootObject.data.procRecord6.sub_totalBH.samt;
                                //this.dgv3Gov.Rows[15].Cells["Integrated Tax"].Value = rootObject.data.procRecord6.sub_totalBH.iamt;
                                //this.dgv3Gov.Rows[15].Cells["Cess"].Value = rootObject.data.procRecord6.sub_totalBH.csamt;
                            }
                            if (rootObject.data.procRecord6.difference != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                 "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                 " Fld_Description,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,
                                 " Values('" +
                                 Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                 Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd6_part3.Rows[16].Cells[0].Value) + "','" +
                                 Convert.ToString(grd6_part3.Rows[16].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord6.difference.txval) + "','" +
                                 Convert.ToString(rootObject.data.procRecord6.difference.camt) + "','" + Convert.ToString(rootObject.data.procRecord6.difference.samt) + "','" +
                                 Convert.ToString(rootObject.data.procRecord6.difference.iamt) + "','" + Convert.ToString(rootObject.data.procRecord6.difference.csamt) + "','" +
                                 Convert.ToString("") + "','" + Convert.ToString("PtIII-6") +
                                 "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();

                                //this.dgv3Gov.Rows[16].Cells["Central Tax"].Value = rootObject.data.procRecord6.difference.camt;
                                //this.dgv3Gov.Rows[16].Cells["State Tax/UT Tax"].Value = rootObject.data.procRecord6.difference.samt;
                                //this.dgv3Gov.Rows[16].Cells["Integrated Tax"].Value = rootObject.data.procRecord6.difference.iamt;
                                //this.dgv3Gov.Rows[16].Cells["Cess"].Value = rootObject.data.procRecord6.difference.csamt;
                            }
                            if (rootObject.data.procRecord6.tran1 != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                " Fld_Description,Fld_CGST,Fld_SGST,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,Fld_IGST,Fld_Cess,
                                " Values('" +
                                Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd6_part3.Rows[17].Cells[0].Value) + "','" +
                                Convert.ToString(grd6_part3.Rows[17].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord6.difference.txval) + "','" +
                                Convert.ToString(rootObject.data.procRecord6.tran1.camt) + "','" + Convert.ToString(rootObject.data.procRecord6.tran1.samt) + "','" +
                               // Convert.ToString(rootObject.data.procRecord6.tran1.iamt) + "','" + Convert.ToString(rootObject.data.procRecord6.tran1.csamt) + "','" +
                                Convert.ToString("") + "','" + Convert.ToString("PtIII-6") +
                                "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();

                                //this.dgv3Gov.Rows[17].Cells["Central Tax"].Value = rootObject.data.procRecord6.tran1.camt;
                                //this.dgv3Gov.Rows[17].Cells["State Tax/UT Tax"].Value = rootObject.data.procRecord6.tran1.samt;
                            }
                            if (rootObject.data.procRecord6.tran2 != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                              "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                              " Fld_Description,Fld_CGST,Fld_SGST,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,Fld_IGST,Fld_Cess,
                              " Values('" +
                              Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                              Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd6_part3.Rows[18].Cells[0].Value) + "','" +
                              Convert.ToString(grd6_part3.Rows[18].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord6.tran2.txval) + "','" +
                              Convert.ToString(rootObject.data.procRecord6.tran2.camt) + "','" + Convert.ToString(rootObject.data.procRecord6.tran2.samt) + "','" +
                                    // Convert.ToString(rootObject.data.procRecord6.tran2.iamt) + "','" + Convert.ToString(rootObject.data.procRecord6.tran2.csamt) + "','" +
                              Convert.ToString("") + "','" + Convert.ToString("PtIII-6") +
                              "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                                //this.dgv3Gov.Rows[18].Cells["Central Tax"].Value = rootObject.data.procRecord6.tran2.camt;
                                //this.dgv3Gov.Rows[18].Cells["State Tax/UT Tax"].Value = rootObject.data.procRecord6.tran2.samt;
                            }
                            if (rootObject.data.procRecord6.other != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                " Fld_Description,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,
                                " Values('" +
                                Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd6_part3.Rows[19].Cells[0].Value) + "','" +
                                Convert.ToString(grd6_part3.Rows[19].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord6.other.txval) + "','" +
                                Convert.ToString(rootObject.data.procRecord6.other.camt) + "','" + Convert.ToString(rootObject.data.procRecord6.other.samt) + "','" +
                                Convert.ToString(rootObject.data.procRecord6.other.iamt) + "','" + Convert.ToString(rootObject.data.procRecord6.other.csamt) + "','" +
                                Convert.ToString("") + "','" + Convert.ToString("PtIII-6") +
                                "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();


                                //this.dgv3Gov.Rows[19].Cells["Central Tax"].Value = rootObject.data.procRecord6.sub_totalKM.camt;
                                //this.dgv3Gov.Rows[19].Cells["State Tax/UT Tax"].Value = rootObject.data.procRecord6.sub_totalKM.samt;
                                //this.dgv3Gov.Rows[19].Cells["Integrated Tax"].Value = rootObject.data.procRecord6.sub_totalKM.iamt;
                                //this.dgv3Gov.Rows[19].Cells["Cess"].Value = rootObject.data.procRecord6.sub_totalKM.csamt;
                            }

                            if (rootObject.data.procRecord6.sub_totalKM != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                                "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                                " Fld_Description,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,
                                " Values('" +
                                Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd6_part3.Rows[20].Cells[0].Value) + "','" +
                                Convert.ToString(grd6_part3.Rows[20].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord6.difference.txval) + "','" +
                                Convert.ToString(rootObject.data.procRecord6.sub_totalKM.camt) + "','" + Convert.ToString(rootObject.data.procRecord6.sub_totalKM.samt) + "','" +
                                Convert.ToString(rootObject.data.procRecord6.sub_totalKM.iamt) + "','" + Convert.ToString(rootObject.data.procRecord6.sub_totalKM.csamt) + "','" +
                                Convert.ToString("") + "','" + Convert.ToString("PtIII-6") +
                                "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();


                                //this.dgv3Gov.Rows[20].Cells["Central Tax"].Value = rootObject.data.procRecord6.sub_totalKM.camt;
                                //this.dgv3Gov.Rows[20].Cells["State Tax/UT Tax"].Value = rootObject.data.procRecord6.sub_totalKM.samt;
                                //this.dgv3Gov.Rows[20].Cells["Integrated Tax"].Value = rootObject.data.procRecord6.sub_totalKM.iamt;
                                //this.dgv3Gov.Rows[20].Cells["Cess"].Value = rootObject.data.procRecord6.sub_totalKM.csamt;
                            }
                            if (rootObject.data.procRecord6.total_itc_availed != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                              "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                              " Fld_Description,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,
                              " Values('" +
                              Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                              Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd6_part3.Rows[21].Cells[0].Value) + "','" +
                              Convert.ToString(grd6_part3.Rows[21].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord6.total_itc_availed.txval) + "','" +
                              Convert.ToString(rootObject.data.procRecord6.total_itc_availed.camt) + "','" + Convert.ToString(rootObject.data.procRecord6.total_itc_availed.samt) + "','" +
                              Convert.ToString(rootObject.data.procRecord6.total_itc_availed.iamt) + "','" + Convert.ToString(rootObject.data.procRecord6.total_itc_availed.csamt) + "','" +
                              Convert.ToString("") + "','" + Convert.ToString("PtIII-6") +
                              "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();

                                //this.dgv3Gov.Rows[21].Cells["Central Tax"].Value = rootObject.data.procRecord6.total_itc_availed.camt;
                                //this.dgv3Gov.Rows[21].Cells["State Tax/UT Tax"].Value = rootObject.data.procRecord6.total_itc_availed.samt;
                                //this.dgv3Gov.Rows[21].Cells["Integrated Tax"].Value = rootObject.data.procRecord6.total_itc_availed.iamt;
                                //this.dgv3Gov.Rows[21].Cells["Cess"].Value = rootObject.data.procRecord6.total_itc_availed.csamt;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Data older that 24 hours Request is being sent to regenerate the latest data. Please refresh the screen after 20 seconds");
                            flag = false;
                            return flag;
                        }
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                MessageBox.Show(string.Concat("Error : ", exception.Message), "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                object[] message = new object[] { exception.Message, Environment.NewLine, exception.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine };
                string str = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", message);
                StreamWriter streamWriter = new StreamWriter("PowerGSTError.txt", true);
                streamWriter.Write(str);
                streamWriter.Close();
                flag = false;
                return flag;
            }
            flag = true;
            return flag;
        }

        public bool GetDataTable7(string jsonString)
        {
            bool flag;

            try
            {
                if (jsonString != string.Empty)
                {
                    RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(jsonString);
                    if ((rootObject == null ? false : rootObject.data != null))
                    {
                        if ((rootObject.data.msg == null ? true : !(rootObject.data.msg != "")))
                        {
                            MC.Open();
                            strQuery = "Delete from tblGSTR9_PtII_PtIII_PtV10_PtVI16 where Fld_HeaderGroup='PtII-7' and Fld_GSTIN='" + CommonHelper.CompanyGSTN + "' and Fld_FinancialYear='" + CommonHelper.ReturnYear + "'";
                            MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                            MC.sqlcmd.ExecuteNonQuery();
                            strQuery = " ";

                            if (rootObject.data.procRecord7.rule37 != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                               "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                               " Fld_Description,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,
                               " Values('" +
                               Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                               Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd7_part3.Rows[0].Cells[0].Value) + "','" +
                               Convert.ToString(grd7_part3.Rows[0].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord7.rule37.txval) + "','" +
                               Convert.ToString(rootObject.data.procRecord7.rule37.camt) + "','" + Convert.ToString(rootObject.data.procRecord7.rule37.samt) + "','" +
                               Convert.ToString(rootObject.data.procRecord7.rule37.iamt) + "','" + Convert.ToString(rootObject.data.procRecord7.rule37.csamt) + "','" +
                               Convert.ToString("") + "','" + Convert.ToString("PtIII-7") +
                               "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                                
                            }
                            if (rootObject.data.procRecord7.rule39 != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                               "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                               " Fld_Description,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,
                               " Values('" +
                               Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                               Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd7_part3.Rows[1].Cells[0].Value) + "','" +
                               Convert.ToString(grd7_part3.Rows[1].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord7.rule39.txval) + "','" +
                               Convert.ToString(rootObject.data.procRecord7.rule39.camt) + "','" + Convert.ToString(rootObject.data.procRecord7.rule39.samt) + "','" +
                               Convert.ToString(rootObject.data.procRecord7.rule39.iamt) + "','" + Convert.ToString(rootObject.data.procRecord7.rule39.csamt) + "','" +
                               Convert.ToString("") + "','" + Convert.ToString("PtIII-7") +
                               "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();

                            }
                            if (rootObject.data.procRecord7.rule42 != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                               "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                               " Fld_Description,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,
                               " Values('" +
                               Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                               Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd7_part3.Rows[2].Cells[0].Value) + "','" +
                               Convert.ToString(grd7_part3.Rows[2].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord7.rule42.txval) + "','" +
                               Convert.ToString(rootObject.data.procRecord7.rule42.camt) + "','" + Convert.ToString(rootObject.data.procRecord7.rule42.samt) + "','" +
                               Convert.ToString(rootObject.data.procRecord7.rule42.iamt) + "','" + Convert.ToString(rootObject.data.procRecord7.rule42.csamt) + "','" +
                               Convert.ToString("") + "','" + Convert.ToString("PtIII-7") +
                               "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();

                            }
                            if (rootObject.data.procRecord7.rule43 != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                               "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                               " Fld_Description,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,
                               " Values('" +
                               Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                               Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd7_part3.Rows[3].Cells[0].Value) + "','" +
                               Convert.ToString(grd7_part3.Rows[3].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord7.rule43.txval) + "','" +
                               Convert.ToString(rootObject.data.procRecord7.rule43.camt) + "','" + Convert.ToString(rootObject.data.procRecord7.rule43.samt) + "','" +
                               Convert.ToString(rootObject.data.procRecord7.rule43.iamt) + "','" + Convert.ToString(rootObject.data.procRecord7.rule43.csamt) + "','" +
                               Convert.ToString("") + "','" + Convert.ToString("PtIII-7") +
                               "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();

                            }
                            if (rootObject.data.procRecord7.sec17 != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                               "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                               " Fld_Description,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,
                               " Values('" +
                               Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                               Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd7_part3.Rows[4].Cells[0].Value) + "','" +
                               Convert.ToString(grd7_part3.Rows[4].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord7.sec17.txval) + "','" +
                               Convert.ToString(rootObject.data.procRecord7.sec17.camt) + "','" + Convert.ToString(rootObject.data.procRecord7.sec17.samt) + "','" +
                               Convert.ToString(rootObject.data.procRecord7.sec17.iamt) + "','" + Convert.ToString(rootObject.data.procRecord7.sec17.csamt) + "','" +
                               Convert.ToString("") + "','" + Convert.ToString("PtIII-7") +
                               "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();


                            }
                            if (rootObject.data.procRecord7.revsl_tran1 != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                               "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                               " Fld_Description,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,
                               " Values('" +
                               Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                               Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd7_part3.Rows[5].Cells[0].Value) + "','" +
                               Convert.ToString(grd7_part3.Rows[5].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord7.revsl_tran1.txval) + "','" +
                               Convert.ToString(rootObject.data.procRecord7.revsl_tran1.camt) + "','" + Convert.ToString(rootObject.data.procRecord7.revsl_tran1.samt) + "','" +
                               Convert.ToString(rootObject.data.procRecord7.revsl_tran1.iamt) + "','" + Convert.ToString(rootObject.data.procRecord7.revsl_tran1.csamt) + "','" +
                               Convert.ToString("") + "','" + Convert.ToString("PtIII-7") +
                               "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                            }

                            if (rootObject.data.procRecord7.revsl_tran2 != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                               "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                               " Fld_Description,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,
                               " Values('" +
                               Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                               Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd7_part3.Rows[6].Cells[0].Value) + "','" +
                               Convert.ToString(grd7_part3.Rows[6].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord7.revsl_tran2.txval) + "','" +
                               Convert.ToString(rootObject.data.procRecord7.revsl_tran2.camt) + "','" + Convert.ToString(rootObject.data.procRecord7.revsl_tran2.samt) + "','" +
                               Convert.ToString(rootObject.data.procRecord7.revsl_tran2.iamt) + "','" + Convert.ToString(rootObject.data.procRecord7.revsl_tran2.csamt) + "','" +
                               Convert.ToString("") + "','" + Convert.ToString("PtIII-7") +
                               "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();

                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                               "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                               " Fld_Description,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,
                               " Values('" +
                               Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                               Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd7_part3.Rows[7].Cells[0].Value) + "','" +
                               Convert.ToString(grd7_part3.Rows[7].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord7.tot_itc_revd.txval) + "','" +
                               //Convert.ToString(rootObject.data.procRecord7.tot_itc_revd.camt) + "','" + Convert.ToString(rootObject.data.procRecord7.tot_itc_revd.samt) + "','" +
                               //Convert.ToString(rootObject.data.procRecord7.tot_itc_revd.iamt) + "','" + Convert.ToString(rootObject.data.procRecord7.tot_itc_revd.csamt) + "','" +
                               Convert.ToString("") + "','" + Convert.ToString("PtIII-7") +
                               "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                            }

                            if (rootObject.data.procRecord7.tot_itc_revd != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                               "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                               " Fld_Description,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,
                               " Values('" +
                               Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                               Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd7_part3.Rows[8].Cells[0].Value) + "','" +
                               Convert.ToString(grd7_part3.Rows[8].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord7.tot_itc_revd.txval) + "','" +
                               Convert.ToString(rootObject.data.procRecord7.tot_itc_revd.camt) + "','" + Convert.ToString(rootObject.data.procRecord7.tot_itc_revd.samt) + "','" +
                               Convert.ToString(rootObject.data.procRecord7.tot_itc_revd.iamt) + "','" + Convert.ToString(rootObject.data.procRecord7.tot_itc_revd.csamt) + "','" +
                               Convert.ToString("") + "','" + Convert.ToString("PtIII-7") +
                               "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                            }
                            if (rootObject.data.procRecord7.net_itc_aval != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                               "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                               " Fld_Description,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,
                               " Values('" +
                               Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                               Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd7_part3.Rows[9].Cells[0].Value) + "','" +
                               Convert.ToString(grd7_part3.Rows[9].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord7.net_itc_aval.txval) + "','" +
                               Convert.ToString(rootObject.data.procRecord7.net_itc_aval.camt) + "','" + Convert.ToString(rootObject.data.procRecord7.net_itc_aval.samt) + "','" +
                               Convert.ToString(rootObject.data.procRecord7.net_itc_aval.iamt) + "','" + Convert.ToString(rootObject.data.procRecord7.net_itc_aval.csamt) + "','" +
                               Convert.ToString("") + "','" + Convert.ToString("PtIII-7") +
                               "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                                //this.dgv4Gov.Rows[9].Cells["Central Tax"].Value = rootObject.data.procRecord7.tab6o.camt;
                                //this.dgv4Gov.Rows[9].Cells["State Tax/UT Tax"].Value = rootObject.data.procRecord7.tab6o.samt;
                                //this.dgv4Gov.Rows[9].Cells["Integrated Tax"].Value = rootObject.data.procRecord7.tab6o.iamt;
                                //this.dgv4Gov.Rows[9].Cells["Cess"].Value = rootObject.data.procRecord7.tab6o.csamt;
                            }

                        }
                        else
                        {
                            MessageBox.Show("Data older that 24 hours Request is being sent to regenerate the latest data. Please refresh the screen after 20 seconds");
                            flag = false;
                            return flag;
                        }
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                MessageBox.Show(string.Concat("Error : ", exception.Message), "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                object[] message = new object[] { exception.Message, Environment.NewLine, exception.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine };
                string str = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", message);
                StreamWriter streamWriter = new StreamWriter("PowerGSTError.txt", true);
                streamWriter.Write(str);
                streamWriter.Close();
                flag = false;
                return flag;
            }
            finally
            {

                MC.Close();
            }
            flag = true;
            return flag;
        }

        public bool GetDataTable8(string jsonString)
        {
            bool flag;
            try
            {
                if (jsonString != string.Empty)
                {
                    RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(jsonString);
                    if ((rootObject == null ? false : rootObject.data != null))
                    {
                        if ((rootObject.data.msg == null ? true : !(rootObject.data.msg != "")))
                        { 
                            MC.Open();
                            strQuery = "Delete from tblGSTR9_PtII_PtIII_PtV10_PtVI16 where Fld_HeaderGroup='PtII-8' and Fld_GSTIN='" + CommonHelper.CompanyGSTN + "' and Fld_FinancialYear='" + CommonHelper.ReturnYear + "'";
                            MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                            MC.sqlcmd.ExecuteNonQuery();
                            strQuery = " ";

                            if (rootObject.data.procRecord8.itc_2a != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                               "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                               " Fld_Description,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,
                               " Values('" +
                               Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                               Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd8_part3.Rows[0].Cells[0].Value) + "','" +
                               Convert.ToString(grd8_part3.Rows[0].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord8.itc_2a.txval) + "','" +
                               Convert.ToString(rootObject.data.procRecord8.itc_2a.camt) + "','" + Convert.ToString(rootObject.data.procRecord8.itc_2a.samt) + "','" +
                               Convert.ToString(rootObject.data.procRecord8.itc_2a.iamt) + "','" + Convert.ToString(rootObject.data.procRecord8.itc_2a.csamt) + "','" +
                               Convert.ToString("") + "','" + Convert.ToString("PtIII-8") +
                               "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                                //    this.dgv5Gov.Rows[0].Cells["Central Tax"].Value = rootObject.data.procRecord8.itc_2a.camt;
                                //    this.dgv5Gov.Rows[0].Cells["State Tax/UT Tax"].Value = rootObject.data.procRecord8.itc_2a.samt;
                                //    this.dgv5Gov.Rows[0].Cells["Integrated Tax"].Value = rootObject.data.procRecord8.itc_2a.iamt;
                                //    this.dgv5Gov.Rows[0].Cells["Cess"].Value = rootObject.data.procRecord8.itc_2a.csamt;

                            }
                            if (rootObject.data.procRecord8.itc_tot != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                             "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                             " Fld_Description,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,
                             " Values('" +
                             Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                             Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd8_part3.Rows[1].Cells[0].Value) + "','" +
                             Convert.ToString(grd8_part3.Rows[1].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord8.itc_tot.txval) + "','" +
                             Convert.ToString(rootObject.data.procRecord8.itc_tot.camt) + "','" + Convert.ToString(rootObject.data.procRecord8.itc_tot.samt) + "','" +
                             Convert.ToString(rootObject.data.procRecord8.itc_tot.iamt) + "','" + Convert.ToString(rootObject.data.procRecord8.itc_tot.csamt) + "','" +
                             Convert.ToString("") + "','" + Convert.ToString("PtIII-8") +
                             "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                                //this.dgv5Gov.Rows[1].Cells["Central Tax"].Value = rootObject.data.procRecord8.itc_tot.camt;
                                //this.dgv5Gov.Rows[1].Cells["State Tax/UT Tax"].Value = rootObject.data.procRecord8.itc_tot.samt;
                                //this.dgv5Gov.Rows[1].Cells["Integrated Tax"].Value = rootObject.data.procRecord8.itc_tot.iamt;
                                //this.dgv5Gov.Rows[1].Cells["Cess"].Value = rootObject.data.procRecord8.itc_tot.csamt;
                          
                                
                                //}

                            //if (rootObject.data.procRecord8.itc_tot != null)
                            //{
                                // Row 2

                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                             "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                             " Fld_Description,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,
                             " Values('" +
                             Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                             Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd8_part3.Rows[2].Cells[0].Value) + "','" +
                             Convert.ToString(grd8_part3.Rows[2].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord8.itc_tot.txval) + "','" +
                             //Convert.ToString(rootObject.data.procRecord8.itc_tot.camt) + "','" + Convert.ToString(rootObject.data.procRecord8.itc_tot.samt) + "','" +
                             //Convert.ToString(rootObject.data.procRecord8.itc_tot.iamt) + "','" + Convert.ToString(rootObject.data.procRecord8.itc_tot.csamt) + "','" +
                             Convert.ToString("") + "','" + Convert.ToString("PtIII-8") +
                             "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                            
                            }
                            if (rootObject.data.procRecord8.differenceABC != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                            "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                            " Fld_Description,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,
                            " Values('" +
                            Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                            Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd8_part3.Rows[3].Cells[0].Value) + "','" +
                            Convert.ToString(grd8_part3.Rows[3].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord8.differenceABC.txval) + "','" +
                            Convert.ToString(rootObject.data.procRecord8.differenceABC.camt) + "','" + Convert.ToString(rootObject.data.procRecord8.differenceABC.samt) + "','" +
                            Convert.ToString(rootObject.data.procRecord8.differenceABC.iamt) + "','" + Convert.ToString(rootObject.data.procRecord8.differenceABC.csamt) + "','" +
                            Convert.ToString("") + "','" + Convert.ToString("PtIII-8") +
                            "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                                //this.dgv5Gov.Rows[3].Cells["Central Tax"].Value = rootObject.data.procRecord8.differenceABC.camt;
                                //this.dgv5Gov.Rows[3].Cells["State Tax/UT Tax"].Value = rootObject.data.procRecord8.differenceABC.samt;
                                //this.dgv5Gov.Rows[3].Cells["Integrated Tax"].Value = rootObject.data.procRecord8.differenceABC.iamt;
                                //this.dgv5Gov.Rows[3].Cells["Cess"].Value = rootObject.data.procRecord8.differenceABC.csamt;


                                // Row 4

                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                             "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                             " Fld_Description,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,
                             " Values('" +
                             Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                             Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd8_part3.Rows[4].Cells[0].Value) + "','" +
                             Convert.ToString(grd8_part3.Rows[4].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord8.itc_tot.txval) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord8.itc_tot.camt) + "','" + Convert.ToString(rootObject.data.procRecord8.itc_tot.samt) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord8.itc_tot.iamt) + "','" + Convert.ToString(rootObject.data.procRecord8.itc_tot.csamt) + "','" +
                             Convert.ToString("") + "','" + Convert.ToString("PtIII-8") +
                             "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();


                                // Row 5

                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                             "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                             " Fld_Description,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,
                             " Values('" +
                             Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                             Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd8_part3.Rows[5].Cells[0].Value) + "','" +
                             Convert.ToString(grd8_part3.Rows[5].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord8.itc_tot.txval) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord8.itc_tot.camt) + "','" + Convert.ToString(rootObject.data.procRecord8.itc_tot.samt) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord8.itc_tot.iamt) + "','" + Convert.ToString(rootObject.data.procRecord8.itc_tot.csamt) + "','" +
                             Convert.ToString("") + "','" + Convert.ToString("PtIII-8") +
                             "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();

                                // Row 6

                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                             "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                             " Fld_Description,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,
                             " Values('" +
                             Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                             Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd8_part3.Rows[6].Cells[0].Value) + "','" +
                             Convert.ToString(grd8_part3.Rows[6].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord8.itc_tot.txval) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord8.itc_tot.camt) + "','" + Convert.ToString(rootObject.data.procRecord8.itc_tot.samt) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord8.itc_tot.iamt) + "','" + Convert.ToString(rootObject.data.procRecord8.itc_tot.csamt) + "','" +
                             Convert.ToString("") + "','" + Convert.ToString("PtIII-8") +
                             "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();

                                // Row 7

                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                             "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                             " Fld_Description,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,
                             " Values('" +
                             Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                             Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd8_part3.Rows[7].Cells[0].Value) + "','" +
                             Convert.ToString(grd8_part3.Rows[7].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord8.itc_tot.txval) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord8.itc_tot.camt) + "','" + Convert.ToString(rootObject.data.procRecord8.itc_tot.samt) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord8.itc_tot.iamt) + "','" + Convert.ToString(rootObject.data.procRecord8.itc_tot.csamt) + "','" +
                             Convert.ToString("") + "','" + Convert.ToString("PtIII-8") +
                             "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                            
                            }
                            if (rootObject.data.procRecord8.differenceGH != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                               "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                               " Fld_Description,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,
                               " Values('" +
                               Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                               Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd8_part3.Rows[8].Cells[0].Value) + "','" +
                               Convert.ToString(grd8_part3.Rows[8].Cells[1].Value) + "','" + // Convert.ToString(rootObject.data.procRecord8.differenceGH.txval) + "','" +
                               Convert.ToString(rootObject.data.procRecord8.differenceGH.camt) + "','" + Convert.ToString(rootObject.data.procRecord8.differenceGH.samt) + "','" +
                               Convert.ToString(rootObject.data.procRecord8.differenceGH.iamt) + "','" + Convert.ToString(rootObject.data.procRecord8.differenceGH.csamt) + "','" +
                               Convert.ToString("") + "','" + Convert.ToString("PtIII-8") +
                               "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();

                                //this.dgv5Gov.Rows[8].Cells["Central Tax"].Value = rootObject.data.procRecord8.differenceGH.camt;
                                //this.dgv5Gov.Rows[8].Cells["State Tax/UT Tax"].Value = rootObject.data.procRecord8.differenceGH.samt;
                                //this.dgv5Gov.Rows[8].Cells["Integrated Tax"].Value = rootObject.data.procRecord8.differenceGH.iamt;
                                //this.dgv5Gov.Rows[8].Cells["Cess"].Value = rootObject.data.procRecord8.differenceGH.csamt;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Data older that 24 hours Request is being sent to regenerate the latest data. Please refresh the screen after 20 seconds");
                            flag = false;
                            return flag;
                        }
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                MessageBox.Show(string.Concat("Error : ", exception.Message), "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                object[] message = new object[] { exception.Message, Environment.NewLine, exception.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine };
                string str = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", message);
                StreamWriter streamWriter = new StreamWriter("PowerGSTError.txt", true);
                streamWriter.Write(str);
                streamWriter.Close();
                flag = false;
                return flag;
            }
            flag = true;
            return flag;
        }

        public bool GetDataTable9(string jsonString)
        {
            bool flag;
            try
            {
                if (jsonString != string.Empty)
                {
                    RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(jsonString);
                    if ((rootObject == null ? false : rootObject.data != null))
                    {
                        if ((rootObject.data.msg == null ? true : !(rootObject.data.msg != "")))
                        {
                             MC.Open();
                             strQuery = "Delete from tblGSTR9_PtIV9 where Fld_HeaderGroup='PtIV-9' and Fld_GSTIN='" + 
                                        CommonHelper.CompanyGSTN + "' and Fld_FinancialYear='" + CommonHelper.ReturnYear + "'";
                            MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                            MC.sqlcmd.ExecuteNonQuery();
                            strQuery = " ";

                            if (rootObject.data.procRecord9.iamt != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtIV9 " +
                                    "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo,Fld_Description, "+
                                    " Fld_TaxPayable,Fld_Paidthroughcash,Fld_CGST,Fld_SGST,Fld_IGST,Fld_HeaderGroup)" + //Fld_Cess,
                                " Values('"+
                                Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                                Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd9_part4.Rows[0].Cells[0].Value) + "','" +
                                Convert.ToString(grd9_part4.Rows[0].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord9.iamt.txpyble) + "','" +
                                Convert.ToString(rootObject.data.procRecord9.iamt.txpaid_cash) + "','" + Convert.ToString(rootObject.data.procRecord9.iamt.tax_paid_itc_camt) + "','" +
                                Convert.ToString(rootObject.data.procRecord9.iamt.tax_paid_itc_samt) + "','" + Convert.ToString(rootObject.data.procRecord9.iamt.tax_paid_itc_iamt) + "','" +
                                //Convert.ToString(rootObject.data.procRecord9.iamt.tax_paid_itc_csamt) + 
                                Convert.ToString("PtIV-9") + "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                           
                            //    this.dgv6Gov.Rows[0].Cells["Tax Payable"].Value = rootObject.data.procRecord9.iamt.txpyble;
                            //    this.dgv6Gov.Rows[0].Cells["Paid through cash"].Value = rootObject.data.procRecord9.iamt.txpaid_cash;
                            //    this.dgv6Gov.Rows[0].Cells["Central Tax (Paid through ITC)"].Value = rootObject.data.procRecord9.iamt.tax_paid_itc_camt;
                            //    this.dgv6Gov.Rows[0].Cells["State Tax/UT Tax (Paid through ITC)"].Value = rootObject.data.procRecord9.iamt.tax_paid_itc_samt;
                            //    this.dgv6Gov.Rows[0].Cells["Integrated Tax (Paid through ITC)"].Value = rootObject.data.procRecord9.iamt.tax_paid_itc_iamt;
                            }
                            if (rootObject.data.procRecord9.camt != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtIV9 " +
                                   "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo,Fld_Description, " +
                                   " Fld_TaxPayable,Fld_Paidthroughcash,Fld_CGST,Fld_IGST,Fld_HeaderGroup)" + //Fld_SGST,Fld_Cess,
                               " Values('" +
                               Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                               Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd9_part4.Rows[1].Cells[0].Value) + "','" +
                               Convert.ToString(grd9_part4.Rows[1].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord9.camt.txpyble) + "','" +
                               Convert.ToString(rootObject.data.procRecord9.camt.txpaid_cash) + "','" + Convert.ToString(rootObject.data.procRecord9.camt.tax_paid_itc_camt) + "','" +
                               //Convert.ToString(rootObject.data.procRecord9.camt.tax_paid_itc_samt) + 
                               Convert.ToString(rootObject.data.procRecord9.camt.tax_paid_itc_iamt) + "','" +
                               //Convert.ToString(rootObject.data.procRecord9.iamt.tax_paid_itc_csamt) + 
                                Convert.ToString("PtIV-9") + "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();

                                //this.dgv6Gov.Rows[1].Cells["Tax Payable"].Value = rootObject.data.procRecord9.camt.txpyble;
                                //this.dgv6Gov.Rows[1].Cells["Paid through cash"].Value = rootObject.data.procRecord9.camt.txpaid_cash;
                                //this.dgv6Gov.Rows[1].Cells["Central Tax (Paid through ITC)"].Value = rootObject.data.procRecord9.camt.tax_paid_itc_camt;
                                //this.dgv6Gov.Rows[1].Cells["Integrated Tax (Paid through ITC)"].Value = rootObject.data.procRecord9.camt.tax_paid_itc_iamt;
                            }
                            if (rootObject.data.procRecord9.samt != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtIV9 " +
                                  "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo,Fld_Description, " +
                                  " Fld_TaxPayable,Fld_Paidthroughcash,Fld_SGST,Fld_IGST,Fld_HeaderGroup)" + // Fld_CGST,Fld_Cess,
                              " Values('" +
                              Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                              Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd9_part4.Rows[2].Cells[0].Value) + "','" +
                              Convert.ToString(grd9_part4.Rows[2].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord9.samt.txpyble) + "','" +
                              Convert.ToString(rootObject.data.procRecord9.samt.txpaid_cash) + "','" + 
                              //Convert.ToString(rootObject.data.procRecord9.samt.tax_paid_itc_camt) + "','" +
                              Convert.ToString(rootObject.data.procRecord9.samt.tax_paid_itc_samt) + "','" + Convert.ToString(rootObject.data.procRecord9.samt.tax_paid_itc_iamt) + "','" +
                               //Convert.ToString(rootObject.data.procRecord9.samt.tax_paid_itc_csamt) + 
                             Convert.ToString("PtIV-9") + "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();

                                //this.dgv6Gov.Rows[2].Cells["Tax Payable"].Value = rootObject.data.procRecord9.samt.txpyble;
                                //this.dgv6Gov.Rows[2].Cells["Paid through cash"].Value = rootObject.data.procRecord9.samt.txpaid_cash;
                                //this.dgv6Gov.Rows[2].Cells["State Tax/UT Tax (Paid through ITC)"].Value = rootObject.data.procRecord9.samt.tax_paid_itc_samt;
                                //this.dgv6Gov.Rows[2].Cells["Integrated Tax (Paid through ITC)"].Value = rootObject.data.procRecord9.samt.tax_paid_itc_iamt;
                            }
                            if (rootObject.data.procRecord9.csamt != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtIV9 " +
                                 "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo,Fld_Description, " +
                                 " Fld_TaxPayable,Fld_Paidthroughcash,Fld_HeaderGroup)" + // Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,
                             " Values('" +
                             Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                             Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd9_part4.Rows[3].Cells[0].Value) + "','" +
                             Convert.ToString(grd9_part4.Rows[3].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord9.csamt.txpyble) + "','" +
                             Convert.ToString(rootObject.data.procRecord9.csamt.txpaid_cash) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord9.csamt.tax_paid_itc_camt) + "','" +
                                    // Convert.ToString(rootObject.data.procRecord9.csamt.tax_paid_itc_samt) + "','" + Convert.ToString(rootObject.data.procRecord9.csamt.tax_paid_itc_iamt) + "','" +
                             Convert.ToString(rootObject.data.procRecord9.csamt.tax_paid_itc_csamt) + 
                             Convert.ToString("PtIV-9") + "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();

                                //this.dgv6Gov.Rows[3].Cells["Tax Payable"].Value = rootObject.data.procRecord9.csamt.txpyble;
                                //this.dgv6Gov.Rows[3].Cells["Paid through cash"].Value = rootObject.data.procRecord9.csamt.txpaid_cash;
                                //this.dgv6Gov.Rows[3].Cells["Cess (Paid through ITC)"].Value = rootObject.data.procRecord9.csamt.tax_paid_itc_csamt;
                            }
                            if (rootObject.data.procRecord9.intr != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtIV9 " +
                                "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo,Fld_Description, " +
                                " Fld_TaxPayable,Fld_Paidthroughcash,Fld_HeaderGroup)" + // Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,
                            " Values('" +
                            Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                            Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd9_part4.Rows[4].Cells[0].Value) + "','" +
                            Convert.ToString(grd9_part4.Rows[4].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord9.intr.txpyble) + "','" +
                            Convert.ToString(rootObject.data.procRecord9.intr.txpaid_cash) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord9.intr.tax_paid_itc_camt) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord9.intr.tax_paid_itc_samt) + "','" + 
                                    //Convert.ToString(rootObject.data.procRecord9.intr.tax_paid_itc_iamt) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord9.intr.tax_paid_itc_csamt) +"','" + 
                            Convert.ToString("PtIV-9") + "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                                //this.dgv6Gov.Rows[4].Cells["Tax Payable"].Value = rootObject.data.procRecord9.intr.txpyble;
                                //this.dgv6Gov.Rows[4].Cells["Paid through cash"].Value = rootObject.data.procRecord9.intr.txpaid_cash;
                            }
                            if (rootObject.data.procRecord9.fee != null)
                            {
                                strQuery = " INSERT INTO tblGSTR9_PtIV9 " +
                               "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo,Fld_Description, " +
                               " Fld_TaxPayable,Fld_Paidthroughcash,Fld_HeaderGroup)" + // Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,
                           " Values('" +
                           Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                           Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd9_part4.Rows[5].Cells[0].Value) + "','" +
                           Convert.ToString(grd9_part4.Rows[5].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord9.fee.txpyble) + "','" +
                           Convert.ToString(rootObject.data.procRecord9.fee.txpaid_cash) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord9.fee.tax_paid_itc_camt) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord9.fee.tax_paid_itc_samt) + "','" + 
                                    //Convert.ToString(rootObject.data.procRecord9.fee.tax_paid_itc_iamt) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord9.fee.tax_paid_itc_csamt) +"','" + 
                           Convert.ToString("PtIV-9") + "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                                //this.dgv6Gov.Rows[5].Cells["Tax Payable"].Value = rootObject.data.procRecord9.fee.txpyble;
                                //this.dgv6Gov.Rows[5].Cells["Paid through cash"].Value = rootObject.data.procRecord9.fee.txpaid_cash;

                                //  Penalty -Row 6
                                strQuery = " INSERT INTO tblGSTR9_PtIV9 " +
                             "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo,Fld_Description, " + " Fld_HeaderGroup)" + //Fld_TaxPayable,Fld_Paidthroughcash, Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,
                             " Values('" +
                         Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                         Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd9_part4.Rows[6].Cells[0].Value) + "','" +
                         Convert.ToString(grd9_part4.Rows[6].Cells[1].Value) + "','" + 
                                     //Convert.ToString(rootObject.data.procRecord9.fee.txpyble) + "','" +
                                     //Convert.ToString(rootObject.data.procRecord9.fee.txpaid_cash) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord9.fee.tax_paid_itc_camt) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord9.fee.tax_paid_itc_samt) + "','" + 
                                    //Convert.ToString(rootObject.data.procRecord9.fee.tax_paid_itc_iamt) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord9.fee.tax_paid_itc_csamt) +"','" + 
                         Convert.ToString("PtIV-9") + "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();

                                //  Ohter -Row 7
                                strQuery = " INSERT INTO tblGSTR9_PtIV9 " +
                             "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo,Fld_Description, " + " Fld_HeaderGroup)" + //Fld_TaxPayable,Fld_Paidthroughcash, Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,
                             " Values('" +
                         Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                         Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd9_part4.Rows[7].Cells[0].Value) + "','" +
                         Convert.ToString(grd9_part4.Rows[7].Cells[1].Value) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord9.fee.txpyble) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord9.fee.txpaid_cash) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord9.fee.tax_paid_itc_camt) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord9.fee.tax_paid_itc_samt) + "','" + 
                                    //Convert.ToString(rootObject.data.procRecord9.fee.tax_paid_itc_iamt) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord9.fee.tax_paid_itc_csamt) +"','" + 
                         Convert.ToString("PtIV-9") + "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Data older that 24 hours Request is being sent to regenerate the latest data. Please refresh the screen after 20 seconds");
                            flag = false;
                            return flag;
                        }
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                MessageBox.Show(string.Concat("Error : ", exception.Message), "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                object[] message = new object[] { exception.Message, Environment.NewLine, exception.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine };
                string str = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", message);
                StreamWriter streamWriter = new StreamWriter("PowerGSTError.txt", true);
                streamWriter.Write(str);
                streamWriter.Close();
                flag = false;
                return flag;
            }
            flag = true;
            return flag;
        }

        public bool GetDataTable10_11_12_13(string jsonString)
        {
            bool flag;
            try
            {
                if (jsonString != string.Empty)
                {
                    RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(jsonString);
                    if ((rootObject == null ? false : rootObject.data != null))
                    {
                        if ((rootObject.data.msg == null ? true : !(rootObject.data.msg != "")))
                        {

                            MC.Open();
                            strQuery = "Delete from tblGSTR9_PtII_PtIII_PtV10_PtVI16 where Fld_HeaderGroup='PtV-10-13' and Fld_GSTIN='" + CommonHelper.CompanyGSTN + "' and Fld_FinancialYear='" + CommonHelper.ReturnYear + "'";
                            MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                            MC.sqlcmd.ExecuteNonQuery();
                            strQuery = " ";

                            if (rootObject.data.procRecord10.total_turnover != null)
                            { 
                                //row=0
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                             "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                             " Fld_Description,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,
                             " Values('" +
                             Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                             Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd10_part5.Rows[0].Cells[0].Value) + "','" +
                             Convert.ToString(grd10_part5.Rows[0].Cells[1].Value) + "','" + //Convert.ToString(rootObject.data.procRecord10.total_turnover.txval) + "','" +
                             //Convert.ToString(rootObject.data.procRecord10.total_turnover.camt) + "','" + Convert.ToString(rootObject.data.procRecord10.total_turnover.samt) + "','" +
                             //Convert.ToString(rootObject.data.procRecord10.total_turnover.iamt) + "','" + Convert.ToString(rootObject.data.procRecord10.total_turnover.csamt) + "','" +
                             Convert.ToString("") + "','" + Convert.ToString("PtV-10-13") +
                             "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();

                                //row=1
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                             "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                             " Fld_Description,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,
                             " Values('" +
                             Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                             Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd10_part5.Rows[1].Cells[0].Value) + "','" +
                             Convert.ToString(grd10_part5.Rows[1].Cells[1].Value) + "','" + //Convert.ToString(rootObject.data.procRecord10.total_turnover.txval) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord10.total_turnover.camt) + "','" + Convert.ToString(rootObject.data.procRecord10.total_turnover.samt) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord10.total_turnover.iamt) + "','" + Convert.ToString(rootObject.data.procRecord10.total_turnover.csamt) + "','" +
                             Convert.ToString("") + "','" + Convert.ToString("PtV-10-13") +
                             "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();

                                //row=2
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                             "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                             " Fld_Description,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,
                             " Values('" +
                             Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                             Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd10_part5.Rows[2].Cells[0].Value) + "','" +
                             Convert.ToString(grd10_part5.Rows[2].Cells[1].Value) + "','" + //Convert.ToString(rootObject.data.procRecord10.total_turnover.txval) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord10.total_turnover.camt) + "','" + Convert.ToString(rootObject.data.procRecord10.total_turnover.samt) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord10.total_turnover.iamt) + "','" + Convert.ToString(rootObject.data.procRecord10.total_turnover.csamt) + "','" +
                             Convert.ToString("") + "','" + Convert.ToString("PtV-10-13") +
                             "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                                
                                //row=3
                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                             "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                             " Fld_Description,Fld_Type,Fld_HeaderGroup) " + //Fld_TaxableValue,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,
                             " Values('" +
                             Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                             Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd10_part5.Rows[3].Cells[0].Value) + "','" +
                             Convert.ToString(grd10_part5.Rows[3].Cells[1].Value) + "','" + //Convert.ToString(rootObject.data.procRecord10.total_turnover.txval) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord10.total_turnover.camt) + "','" + Convert.ToString(rootObject.data.procRecord10.total_turnover.samt) + "','" +
                                    //Convert.ToString(rootObject.data.procRecord10.total_turnover.iamt) + "','" + Convert.ToString(rootObject.data.procRecord10.total_turnover.csamt) + "','" +
                             Convert.ToString("") + "','" + Convert.ToString("PtV-10-13") +
                             "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();


                                strQuery = " INSERT INTO tblGSTR9_PtII_PtIII_PtV10_PtVI16 " +
                               "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                               " Fld_Description,Fld_TaxableValue,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup) " + //
                               " Values('" +
                               Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                               Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grd10_part5.Rows[4].Cells[0].Value) + "','" +
                               Convert.ToString(grd10_part5.Rows[4].Cells[1].Value) + "','" + Convert.ToString(rootObject.data.procRecord10.total_turnover.txval) + "','" +
                               Convert.ToString(rootObject.data.procRecord10.total_turnover.camt) + "','" + Convert.ToString(rootObject.data.procRecord10.total_turnover.samt) + "','" +
                               Convert.ToString(rootObject.data.procRecord10.total_turnover.iamt) + "','" + Convert.ToString(rootObject.data.procRecord10.total_turnover.csamt) + "','" +
                               Convert.ToString("") + "','" + Convert.ToString("PtV-10-13") +
                               "')";
                                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                                MC.sqlcmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            //"INSERT INTO tblGSTR9_PtII_PtIII_PtV_16(Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo,Fld_Description,Fld_TaxableValue,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_Type,Fld_HeaderGroup)Values('", Convert.ToString(dt.Rows[i]["Fld_GSTIN"]), "','", Convert.ToString(dt.Rows[i]["Fld_FinancialYear"]), "','", Convert.ToString(dt.Rows[i]["Fld_Month"]), "','", Convert.ToString(dt.Rows[i]["Fld_SrNo"]), "','", Convert.ToString(dt.Rows[i]["Fld_Description"]), "','", Convert.ToString(dt.Rows[i]["Fld_TaxableValue"]), "','", Convert.ToString(dt.Rows[i]["Fld_CGST"]), "','", Convert.ToString(dt.Rows[i]["Fld_SGST"]), "','", Convert.ToString(dt.Rows[i]["Fld_IGST"]), "','", Convert.ToString(dt.Rows[i]["Fld_Cess"]), "','", Convert.ToString(dt.Rows[i]["Fld_Type"]), "','", Convert.ToString(dt.Rows[i]["Fld_HeaderGroup"]), "')" };
                            MessageBox.Show("Data older that 24 hours Request is being sent to regenerate the latest data. Please refresh the screen after 20 seconds");
                            flag = false;
                            return flag;
                        }
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                MessageBox.Show(string.Concat("Error : ", exception.Message), "Technical Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                object[] message = new object[] { exception.Message, Environment.NewLine, exception.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine };
                string str = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", message);
                StreamWriter streamWriter = new StreamWriter("PowerGSTError.txt", true);
                streamWriter.Write(str);
                streamWriter.Close();
                flag = false;
                return flag;
            }
            flag = true;
            return flag;
        }
        protected HttpWebRequest PrepareRequest_AnnualReturn(Uri uri, string referer)
        {
            HttpWebRequest httpWebRequest;
            try
            {
                HttpWebRequest httpWebRequest1 = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest1.CookieContainer = this.Cc;
                httpWebRequest1.KeepAlive = true;
                httpWebRequest1.Method = "GET";
                //httpWebRequest1.Method = "POST";

                if (uri.ToString().Contains("registration/auth/"))
                {
                    httpWebRequest1.Host = "enroll.gst.gov.in";
                }
                else if (uri.ToString().Contains("payment.gst.gov.in/"))
                {
                    httpWebRequest1.Host = "payment.gst.gov.in";
                }
                else if (uri.ToString().Contains("return.gst.gov.in/"))
                {
                    httpWebRequest1.Host = "return.gst.gov.in";
                }
                else if (!uri.ToString().Contains("files.gst.gov.in"))
                {
                    httpWebRequest1.Host = "services.gst.gov.in";
                }
                else
                {
                    httpWebRequest1.Host = "files.gst.gov.in";
                }

                if (referer != null)
                {
                    httpWebRequest1.Referer = referer;
                }
                httpWebRequest1.Accept = "application/json, text/plain, */*";
                httpWebRequest1.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
                httpWebRequest1.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                //httpWebRequest1.Headers.Add("Origin", "https://services.gst.gov.in");
                httpWebRequest1.Headers.Add("Origin", "https://return.gst.gov.in");
                // httpWebRequest1.Headers.Add("Origin", "https://payment.gst.gov.in");
                httpWebRequest1.KeepAlive = true;
                //httpWebRequest1.Host = "services.gst.gov.in";
                httpWebRequest1.Host = "return.gst.gov.in";
                httpWebRequest1.ContentType = "application/json;charset=UTF-8";
                //using (StreamWriter streamWriter = new StreamWriter(httpWebRequest1.GetRequestStream()))
                //{
                //    streamWriter.Write(string.Concat(new string[] { "{\"form_type\":\"" + formtype + "\" ,\"fy\":\"" + formtype + "\"}" }));
                //    //streamWriter.Write(string.Concat(new string[] { "{\"ctin\":\"" + gstin + "\"}" }));
                //}
                httpWebRequest = httpWebRequest1;
            }
            catch (Exception exception)
            {
                // this.getError = string.Concat("Error in requesting to server", exception.Message);
                httpWebRequest = null;
            }
            return httpWebRequest;
        }
        private void btnDifference_Click(object sender, EventArgs e)
        {

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
                    grd.ReadOnly = true;
                    grd.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
                    grd.EnableHeadersVisualStyles = false;
                    grd.ColumnHeadersDefaultCellStyle.Font = new Font("Verdana", 8, FontStyle.Bold);
                    grd.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
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

        private void btnPdf_Click(object sender, EventArgs e)
        {

           
        }

        private void btnPrepareGSTR9_Click(object sender, EventArgs e)
        {
         
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Wotk is Under Process");
            return;

            //**********************   Demo Explain for CRUD Operation """"""""""""""""""""""""""""""""""

            //*********** To Get/Fetch data from Table  use below Function
            
            DataTable dt= MC.GetValueindatatable("select *  from tblGSTR9_Summary");
            DataSet ds= MC.GetValueInDataset("select *  from tblGSTR9_Summary", "TableName");

            //********************** For Insert delete Update  Operation
            try
            {
                MC.Open();

                //for delete
                string strQuery = "";
                strQuery = "Delete from tblGSTR9_Summary where Fld_GSTIN='" + CommonHelper.CompanyGSTN + "' and Fld_FinancialYear='" + CommonHelper.ReturnYear + "'";
                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                MC.sqlcmd.ExecuteNonQuery();

                //for insert
                strQuery = " INSERT INTO tblGSTR9_Summary " +
                            "( Fld_GSTIN,Fld_FinancialYear,Fld_Month,Fld_SrNo, " +
                            " Fld_Description,Fld_TaxableValue,Fld_CGST,Fld_SGST,Fld_IGST,Fld_Cess,Fld_HeaderGroup) " +
                            " Values('" +
                            Convert.ToString(CommonHelper.CompanyGSTN) + "','" + Convert.ToString(CommonHelper.ReturnYear) + "','" +
                            Convert.ToString(CommonHelper.SelectedMonth) + "','" + Convert.ToString(grdSummary.Rows[0].Cells[0].Value) + "','" +
                            Convert.ToString(grdSummary.Rows[0].Cells[1].Value) + "','" + Convert.ToString("") + "','" +
                            Convert.ToString("") + "','" + Convert.ToString("") + "','" +
                            Convert.ToString("") + "','" + Convert.ToString("") + "','" +
                            Convert.ToString("Table4") +
                            "')";
                MC.sqlcmd = new SQLiteCommand(strQuery, MC.con);
                MC.sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                MC.Close();
            }
        }

        private void grdSummary_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grdSummary_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        
    }

    public static class CommonHelper
    {
        public static string CompanyGSTN { set; get; }
        public static string ReturnYear { set; get; }
        public static string SelectedMonth { set; get; }

      
    }

    #region Properties

         public class AmdNeg
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public AmdNeg()
            {
            }
        }

        public class AmdPos
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public AmdPos()
            {
            }
        }

        public class At
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public At()
            {
            }
        }

        public class B2b
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public B2b()
            {
            }
        }

        public class B2c
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public B2c()
            {
            }
        }

        public class Camt
        {
            public double tax_paid_itc_camt
            {
                get;
                set;
            }

            public double tax_paid_itc_iamt
            {
                get;
                set;
            }

            public double txpaid_cash
            {
                get;
                set;
            }

            public double txpyble
            {
                get;
                set;
            }

            public Camt()
            {
            }
        }

        public class CrNt
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public CrNt()
            {
            }
        }

        public class Csamt
        {
            public double tax_paid_itc_csamt
            {
                get;
                set;
            }

            public double txpaid_cash
            {
                get;
                set;
            }

            public double txpyble
            {
                get;
                set;
            }

            public Csamt()
            {
            }
        }

        public class Data
        {
            public string msg
            {
                get;
                set;
            }

            public ProcRecord10 procRecord10
            {
                get;
                set;
            }

            public ProcRecord4 procRecord4
            {
                get;
                set;
            }

            public ProcRecord5 procRecord5
            {
                get;
                set;
            }

            public  ProcRecord6 procRecord6
            {
                get;
                set;
            }

            public  ProcRecord7 procRecord7
            {
                get;
                set;
            }

            public  ProcRecord8 procRecord8
            {
                get;
                set;
            }

            public  ProcRecord9 procRecord9
            {
                get;
                set;
            }

            string gstin { get; set; }
            string fp { get; set; }
            public Table4 table4
            {

                get;
                set;
            }
            public Table5 table5
            {

                get;
                set;
            }
            public Table6 table6
            {

                get;
                set;
            }
            public Table7 table7
            {

                get;
                set;
            }
            public Table8 table8
            {

                get;
                set;
            }
            public Table9 table9
            {

                get;
                set;
            }

            public Data()
            {
            }
        }

        public class Deemed
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public Deemed()
            {
            }
        }

        public class Difference
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public Difference()
            {
            }
        }

        public class DifferenceABC
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public DifferenceABC()
            {
            }
        }

        public class DifferenceGH
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public DifferenceGH()
            {
            }
        }

        public class DrNt
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public DrNt()
            {
            }
        }

        public class Exmt
        {
            public double txval
            {
                get;
                set;
            }

            public Exmt()
            {
            }
        }

        public class Exp
        {
            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public Exp()
            {
            }
        }

        public class Fee
        {
            public double txpaid_cash
            {
                get;
                set;
            }

            public double txpyble
            {
                get;
                set;
            }

            public Fee()
            {
            }
        }

        public class Iamt
        {
            public double tax_paid_itc_camt
            {
                get;
                set;
            }

            public double tax_paid_itc_iamt
            {
                get;
                set;
            }

            public double tax_paid_itc_samt
            {
                get;
                set;
            }

            public double txpaid_cash
            {
                get;
                set;
            }

            public double txpyble
            {
                get;
                set;
            }

            public Iamt()
            {
            }
        }

        public class Intr
        {
            public double txpaid_cash
            {
                get;
                set;
            }

            public double txpyble
            {
                get;
                set;
            }

            public Intr()
            {
            }
        }

        public class IogItcAvaild
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public IogItcAvaild()
            {

            }
        }

        public class Other
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public Other()
            {
            }
        }
       public class Itcclmd
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public Itcclmd()
            {
            }
        }
        public class Isd
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public Isd()
            {
            }
        }

        public class Itc2a
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public Itc2a()
            {
            }
        }

        public class Itc3b
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public Itc3b()
            {
            }
        }

        public class ItcTot
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public ItcTot()
            {
            }
        }

        public class NetItcAval
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public NetItcAval()
            {
            }
        }

        public class Rule37
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public Rule37()
            {
            }
        }
        public class Rule39
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public Rule39()
            {
            }
        }
        public class Rule42
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public Rule42()
            {
            }
        }

        public class Rule43
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public Rule43()
            {
            }
        }

        public class Sec17
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public Sec17()
            {
            }
        }

        public class RevslTran1
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public RevslTran1()
            {
            }
        }

        public class RevslTran2
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public RevslTran2()
            {
            }
        }

        public class TotItcRevd
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public TotItcRevd()
            {
            }
        }

        public class Nil
        {
            public double txval
            {
                get;
                set;
            }

            public Nil()
            {
            }
        }

        public class NonGst
        {
            public double txval
            {
                get;
                set;
            }

            public NonGst()
            {
            }
        }
        public class Supp_Non_Rchrg
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public string itc_typ
            {
                get;
                set;
            }

            public Supp_Non_Rchrg()
            {
            }
        }

        public class Supp_Rchrg_Unreg
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public string itc_typ
            {
                get;
                set;
            }

            public Supp_Rchrg_Unreg()
            {
            }
        }

        public class Supp_Rchrg_Reg
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public string itc_typ
            {
                get;
                set;
            }

            public Supp_Rchrg_Reg()
            {
            }
        }

        
        public class Iog
        {

            public double iamt
            {
                get;
                set;
            }
            public double csamt
            {
                get;
                set;
            }

            public string itc_typ
            {
                get;
                set;
            }
            public Iog()
            {
            }
        }

        public class Ios
        { 
         public double iamt
            {
                get;
                set;
            }
            public double csamt
            {
                get;
                set;
            }

            public Ios()
            {
            }
        }

        public class ProcRecord10
        {
            public string editable
            {
                get;
                set;
            }

            public string status
            {
                get;
                set;
            }

            public  Tab4g tab4g
            {
                get;
                set;
            }

            public  Tab4n tab4n
            {
                get;
                set;
            }

            public  Tab5m tab5m
            {
                get;
                set;
            }

            public  TotalTurnover total_turnover
            {
                get;
                set;
            }

            public ProcRecord10()
            {
            }
        }

        public class ProcRecord4
        {
            public  AmdNeg amd_neg
            {
                get;
                set;
            }

            public  AmdPos amd_pos
            {
                get;
                set;
            }

            public  At at
            {
                get;
                set;
            }

            public  B2b b2b
            {
                get;
                set;
            }

            public  B2c b2c
            {
                get;
                set;
            }

            public string chksum
            {
                get;
                set;
            }

            public  CrNt cr_nt
            {
                get;
                set;
            }

            public  Deemed deemed
            {
                get;
                set;
            }

            public  DrNt dr_nt
            {
                get;
                set;
            }

            public string editable
            {
                get;
                set;
            }

            public  Exp exp
            {
                get;
                set;
            }

            public  Rchrg rchrg
            {
                get;
                set;
            }

            public  Sez sez
            {
                get;
                set;
            }

            public  SubTotalAG sub_totalAG
            {
                get;
                set;
            }

            public  SubTotalIL sub_totalIL
            {
                get;
                set;
            }

            public  SupAdv sup_adv
            {
                get;
                set;
            }

            public ProcRecord4()
            {
            }
        }

        public class ProcRecord5
        {
            public  AmdNeg amd_neg
            {
                get;
                set;
            }

            public  AmdPos amd_pos
            {
                get;
                set;
            }

            public string chksum
            {
                get;
                set;
            }

            public  CrNt cr_nt
            {
                get;
                set;
            }

            public  DrNt dr_nt
            {
                get;
                set;
            }

            public string editable
            {
                get;
                set;
            }

            public  Exmt exmt
            {
                get;
                set;
            }

            public  Nil nil
            {
                get;
                set;
            }

            public  NonGst non_gst
            {
                get;
                set;
            }

            public  Rchrg rchrg
            {
                get;
                set;
            }

            public  Sez sez
            {
                get;
                set;
            }

            public string status
            {
                get;
                set;
            }

            public  SubTotalAF sub_totalAF
            {
                get;
                set;
            }

            public  SubTotalHK sub_totalHK
            {
                get;
                set;
            }

            public  Tab4g tab4g
            {
                get;
                set;
            }

            public  Tab4n tab4n
            {
                get;
                set;
            }

            public  TotalTover total_tover
            {
                get;
                set;
            }

            public  ToverTaxNp tover_tax_np
            {
                get;
                set;
            }

            public  ZeroRtd zero_rtd
            {
                get;
                set;
            }

            public ProcRecord5()
            {
            }
        }


        public class ProcRecord6
        {
            public string chksum
            {
                get;
                set;
            }

            public  Difference difference
            {
                get;
                set;
            }

            public List<Supp_Non_Rchrg> supp_non_rchrg {
                get;
                set;
            }

            public List<Supp_Rchrg_Reg> supp_rchrg_reg
            {
                get;
                set;
            }

            public List<Supp_Rchrg_Unreg> supp_rchrg_unreg
            {
                get;
                set;
            }

            public List<Iog> iog
            {

                get;
                set;
            }
            public Ios ios
            {

                get;
                set;
            }

            public string editable
            {
                get;
                set;
            }

            public  Isd isd
            {
                get;
                set;
            }

            public Itcclmd itc_clmd
            {
                get;
                set;
            }
            public  Itc3b itc_3b
            {
                get;
                set;
            }

            public Other other
            {

                get;
                set;
            }
            public string status
            {
                get;
                set;
            }

            public  SubTotalBH sub_totalBH
            {
                get;
                set;
            }

            public  SubTotalKM sub_totalKM
            {
                get;
                set;
            }

            public  TotalItcAvailed total_itc_availed
            {
                get;
                set;
            }

            public  Tran1 tran1
            {
                get;
                set;
            }

            public  Tran2 tran2
            {
                get;
                set;
            }

            public ProcRecord6()
            {
            }
        }

        public class ProcRecord7
        {
            public Rule37 rule37
            {

                get;
                set;
            }
            public Rule39 rule39
            {

                get;
                set;
            }
            public Rule42 rule42
            {

                get;
                set;
            }

            public Rule43 rule43
            {

                get;
                set;
            }

            public Sec17 sec17
            {

                get;
                set;
            }

            public RevslTran1 revsl_tran1
            {

                get;
                set;
            }

            public RevslTran2 revsl_tran2
            {

                get;
                set;
            }

            public TotItcRevd tot_itc_revd
            {
                get;
                set;
            }

            public  NetItcAval net_itc_aval
            {
                get;
                set;
            }
            public Tab6o tab6o
            {
                get;
                set;
            }

            public string status
            {
                get;
                set;
            }

            public string editable
            {
                get;
                set;
            }
         

            public ProcRecord7()
            {

            }
        }

        public class ProcRecord8
        {
            public string chksum
            {
                get;
                set;
            }

            public  DifferenceABC differenceABC
            {
                get;
                set;
            }

            public  DifferenceGH differenceGH
            {
                get;
                set;
            }

            public string editable
            {
                get;
                set;
            }

            public  IogItcAvaild iog_itc_availd
            {
                get;
                set;
            }

            public  Itc2a itc_2a
            {
                get;
                set;
            }

            public  ItcTot itc_tot
            {
                get;
                set;
            }

            public ProcRecord8()
            {
            }
        }

        public class ProcRecord9
        {
            public  Camt camt
            {
                get;
                set;
            }

            public string chksum
            {
                get;
                set;
            }

            public  Csamt csamt
            {
                get;
                set;
            }

            public string editable
            {
                get;
                set;
            }

            public  Fee fee
            {
                get;
                set;
            }

            public  Iamt iamt
            {
                get;
                set;
            }

            public  Intr intr
            {
                get;
                set;
            }

            public  Samt samt
            {
                get;
                set;
            }

            public string status
            {
                get;
                set;
            }

            public ProcRecord9()
            {
            }
        }

      

        public class Table4
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public Table4()
            {
            }
        }
        public class Table5
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public Table5()
            {
            }
        }
        public class Table6
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public Table6()
            {
            }
        }
        public class Table7
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public Table7()
            {
            }
        }
        public class Table8
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public Table8()
            {
            }
        }
        public class Table9
        {
            public double txpaid_itc
            {
                get;
                set;
            }

            public double txpaid_cash
            {
                get;
                set;
            }

            public double txpyble
            {
                get;
                set;
            }

            public Table9()
            {
            }
        }


        public class Rchrg
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public Rchrg()
            {
            }
        }

        public class RootObject
        {
            public  Data data
            {
                get;
                set;
            }

            public int status
            {
                get;
                set;
            }

            public RootObject()
            {
            }
        }

        public class Samt
        {
            public double tax_paid_itc_iamt
            {
                get;
                set;
            }

            public double tax_paid_itc_samt
            {
                get;
                set;
            }

            public double txpaid_cash
            {
                get;
                set;
            }

            public double txpyble
            {
                get;
                set;
            }

            public Samt()
            {
            }
        }

        public class Sez
        {
            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public Sez()
            {
            }
        }

        public class SubTotalAF
        {
            public double txval
            {
                get;
                set;
            }

            public SubTotalAF()
            {
            }
        }

        public class SubTotalAG
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public SubTotalAG()
            {
            }
        }

        public class SubTotalBH
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public SubTotalBH()
            {
            }
        }

        public class SubTotalHK
        {
            public double txval
            {
                get;
                set;
            }

            public SubTotalHK()
            {
            }
        }

        public class SubTotalIL
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public SubTotalIL()
            {
            }
        }

        public class SubTotalKM
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public SubTotalKM()
            {
            }
        }

        public class SupAdv
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public SupAdv()
            {
            }
        }

        public class Tab4g
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public Tab4g()
            {
            }
        }

        public class Tab4n
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public Tab4n()
            {
            }
        }

        public class Tab5m
        {
            public double txval
            {
                get;
                set;
            }

            public Tab5m()
            {
            }
        }

        public class Tab6o
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public Tab6o()
            {
            }
        }

        public class TotalItcAvailed
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public TotalItcAvailed()
            {
            }
        }

        public class TotalTover
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public TotalTover()
            {
            }
        }

        public class TotalTurnover
        {
            public double camt
            {
                get;
                set;
            }

            public double csamt
            {
                get;
                set;
            }

            public double iamt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public double txval
            {
                get;
                set;
            }

            public TotalTurnover()
            {
            }
        }

        public class ToverTaxNp
        {
            public double txval
            {
                get;
                set;
            }

            public ToverTaxNp()
            {
            }
        }

        public class Tran1
        {
            public double camt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public Tran1()
            {
            }
        }

        public class Tran2
        {
            public double camt
            {
                get;
                set;
            }

            public double samt
            {
                get;
                set;
            }

            public Tran2()
            {
            }
        }

        public class ZeroRtd
        {
            public double txval
            {
                get;
                set;
            }

            public ZeroRtd()
            {
            }
        }
   

        #endregion
}
