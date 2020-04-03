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
    public partial class GSTR_7_Reports : Form
    {
        public GSTR_7_Reports()
        {
            InitializeComponent();
        }

        private void GSTR_7_Reports_Load(object sender, EventArgs e)
        {
            GetSetSoftwareData();
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
                Bind_Gridcheck(grd_check);
                //grd_check.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
               // grd_check.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
               // grd_check.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                //grd_check.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                grid_year.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                Bind_Gridyear(grid_year);
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
        private void Bind_GridSummary(DataGridView grdSummary)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Types");
            dt.Columns.Add("Status");
            dt.Columns.Add("No Of Records");
            dt.Columns.Add("Total Taxable Value");
            dt.Columns.Add("Total intergrated Tax");
            dt.Columns.Add("Total Intergrated tax");
            dt.Columns.Add("Total State/UT Tax");
            dt.Columns.Add("Total tax");

            DataRow dr = dt.NewRow();
            dr[0] = "Details of Tax Deducted  at Source";
            dr[1] = "-";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Amendment in Details of Tax Deducted at Source";
            dr[1] = "-";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dt.Rows.Add(dr);
            grdSummary.DataSource = dt;
            //grdSummary.Rows.Add(2);

            //grdSummary.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ////grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;


            //grdSummary[0, 0].Value = "Details of Tax Deducted  at Source ";
            //grdSummary[1, 0].Value = "-";
            //grdSummary[2, 0].Value = "0";
            //grdSummary[3, 0].Value = "0";"
            //grdSummary[4, 0].Value = "0";
            //grdSummary[5, 0].Value = "0";
            //grdSummary[6, 0].Value = "0";
            //grdSummary[7, 0].Value = "0";



            //grdSummary[0, 1].Value = "Amendment in Details of Tax Deducted at Source ";
            //grdSummary[1, 1].Value = "-";
            //grdSummary[2, 1].Value = "0";
            //grdSummary[3, 1].Value = "0";
            //grdSummary[4, 1].Value = "0";
            //grdSummary[5, 1].Value = "0";
            //grdSummary[6, 1].Value = "0 ";
            //grdSummary[7, 1].Value = "0";
           

            //foreach (DataGridViewColumn col in grdSummary.Columns)
            //{
            //    col.SortMode = DataGridViewColumnSortMode.NotSortable;
            //}
        }
        private void Bind_Gridcheck(DataGridView grd_check)
        {
            grd_check.Rows.Add(2);

            grdSummary.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;


            grd_check[0, 0].Value = "0";
            grd_check[1, 0].Value = "0";
            grd_check[2, 0].Value = "0";
            grd_check[3, 0].Value = "0";
            grd_check[4, 0].Value = "0";
            grd_check[5, 0].Value = "0";
            grd_check[6, 0].Value = "0";
            grd_check[7, 0].Value = "0";



            


            foreach (DataGridViewColumn col in grdSummary.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        private void Bind_Gridyear(DataGridView grid_year)
        {
            grid_year.Rows.Add(1);

            grdSummary.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;


            grid_year[0, 0].Value = "";
            grid_year[1, 0].Value = "";
            grid_year[2, 0].Value = "";
            grid_year[3, 0].Value = "";
            grid_year[4, 0].Value = "";
            grid_year[5, 0].Value = "";
            grid_year[6, 0].Value = "";
            grid_year[7, 0].Value = "";
            grid_year[8, 0].Value = "";
            grid_year[9, 0].Value = "";
            grid_year[10, 0].Value = "";
            grid_year[11, 0].Value = "";
            grid_year[12, 0].Value = "";






            foreach (DataGridViewColumn col in grdSummary.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void grid_year_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void grdSummary_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grdSummary_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == 0)
            {
                tabSoftware.SelectTab(1);
            }
            if (e.RowIndex == 1)
            {
                tabSoftware.SelectTab(2);
            }
        }
        
    }
     
}
