using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPEQTAGST_DESIGN
{
    public partial class GSTR2A__Reports : Form
    {
        public GSTR2A__Reports()
        {
            InitializeComponent();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void GSTR2A__Reports_Load(object sender, EventArgs e)
        {
            GetSetSoftwareData();
        }
        private void Bind_GridSummary(DataGridView grdSummary)
        {
            DataTable dt = new DataTable();
            
            dt.Columns.Add("Type Of Invoices ");
            dt.Columns.Add("Status");
            dt.Columns.Add("Document Type");
            dt.Columns.Add("Taxable Value");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("CESS");

            DataRow dr = dt.NewRow();
            dr[0] = "B2B Invoices";
            dr[1] = "-";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";

            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Credit/Debit Notes";
            dr[1] = "-";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
          
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Amendements to B2B Invoices ";
            dr[1] = "-";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Amendements to Credit/Debit Notes";
            dr[1] = "-";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Total";
            dr[1] = "-";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dt.Rows.Add(dr);
            grdSummary.DataSource = dt;

            //grdSummary.Rows.Add(6);

            //grdSummary.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ////grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;


            //grdSummary[0, 0].Value = "1";
            //grdSummary[1, 0].Value = "Inward of supplies recevied from a registrated  person";
            //grdSummary[2, 0].Value = "0";
            //grdSummary[3, 0].Value = "0";
            //grdSummary[4, 0].Value = "0";
            //grdSummary[5, 0].Value = "0";
            //grdSummary[6, 0].Value = "0";
            //grdSummary[7, 0].Value = "0";
            //grdSummary[8, 0].Value = "0";



            //grdSummary[0, 1].Value = "2 ";
            //grdSummary[1, 1].Value = "Details of Credit/ Debit Notes";
            //grdSummary[2, 1].Value = "0";

            //grdSummary[3, 1].Value = "0";
            //grdSummary[4, 1].Value = "0";
            //grdSummary[5, 1].Value = "0";
            //grdSummary[6, 1].Value = "0 ";
            //grdSummary[7, 1].Value = "0";
            //grdSummary[8, 1].Value = "0";

            

            //grdSummary[0, 2].Value = "3";
            //grdSummary[1, 2].Value = "Amendments to B2B Invoices ";
            //grdSummary[2, 2].Value = "0";

            //grdSummary[3, 2].Value = "0";
            //grdSummary[4, 2].Value = "0";
            //grdSummary[5, 2].Value = "0";
            //grdSummary[6, 2].Value = "0 ";
            //grdSummary[7, 2].Value = "0";
            //grdSummary[8, 2].Value = "0";

            //grdSummary[0, 3].Value = "4 ";
            //grdSummary[1, 3].Value = "Amendements to credit/Debit Notes";
            //grdSummary[2, 3].Value = "0";

            //grdSummary[3, 3].Value = "0";
            //grdSummary[4, 3].Value = "0";
            //grdSummary[5, 3].Value = "0";
            //grdSummary[6, 3].Value = "0 ";
            //grdSummary[7, 3].Value = "0";
            //grdSummary[8, 3].Value = "0";

            //grdSummary[0, 4].Value = "5 ";
            //grdSummary[1, 4].Value = "Total";
            //grdSummary[2, 4].Value = "0";

            //grdSummary[3, 4].Value = "0";
            //grdSummary[4, 4].Value = "0";
            //grdSummary[5, 4].Value = "0";
            //grdSummary[6, 4].Value = "0 ";
            //grdSummary[7, 4].Value = "0";
            //grdSummary[8, 4].Value = "0";


            //foreach (DataGridViewColumn col in grdSummary.Columns)
            //{
            //    col.SortMode = DataGridViewColumnSortMode.NotSortable;
            //}
        }

        private void Bind_Grid_check(DataGridView grdSummary)
        {

        }
        private void Bind_Grid_Invoice(DataGridView amendGrid)
        {

        }
             private void GetSetSoftwareData()
        {
            try
            {


                grdSummary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                Bind_GridSummary(grdSummary);
                //grdSummary.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                //grdSummary.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
               // grdSummary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
               // grdSummary.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                grd_check.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                Bind_Grid_check(grd_check);
                //grd_check.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
               // grd_check.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
               // grd_check.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                //grd_check.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                amendGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                Bind_Grid_Invoice(amendGrid);
               // grid_year.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
              //  grid_year.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
              //  grid_year.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
              //  grid_year.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


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

             
     

        private void tabcontrol_DrawItem(object sender, DrawItemEventArgs e)
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void grdSummary_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == 0)
            {
                tabSoftware.SelectTab(1);
            }
            if (e.RowIndex == 1)
            {
                tabSoftware.SelectTab(2);
            }
            if(e.RowIndex ==2)
            {
                tabSoftware.SelectTab(3);
            }
            if (e.RowIndex == 3)
            {
                tabSoftware.SelectTab(4);
            }
            if (e.RowIndex == 4)
            {
                tabSoftware.SelectTab(5);
            }
        }

        private void grdSummary_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
             }
        }
    

