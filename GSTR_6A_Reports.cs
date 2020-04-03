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
    public partial class GSTR_6A_Reports : Form
    {
        public GSTR_6A_Reports()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {

        }
        private void GetSetSoftwareData()
        {
            try
            {


                grd_summary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                Bind_GridSummary(grd_summary);
                //grdSummary.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                //grdSummary.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                // grdSummary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                // grdSummary.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

               // grd_input.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                Bind_Gridinput(grd_input);
                //grd_check.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                // grd_check.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                // grd_check.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                //grd_check.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

              //  grd_debit.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                Bind_grd_debit(grd_debit);
                // grid_year.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                //  grid_year.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                //  grid_year.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                //  grid_year.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                //gridview_Datainformation(grid_information);

                //Bind_GridVoice(grd_invoice);

                //gridview_datadistrub(grid_distrub);

                //gridview_dataITC(grd_summary);
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
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblYear_Click(object sender, EventArgs e)
        {
                    }

        private void Bind_GridVoice(DataGridView grd_invoice)
        {
            grd_invoice.Rows.Add(1);

            grd_invoice.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;


            grd_invoice[0, 0].Value = "";
            grd_invoice[1, 0].Value = "";
            grd_invoice[2, 0].Value = "";
            grd_invoice[3, 0].Value = "";
            grd_invoice[4, 0].Value = "";
            grd_invoice[5, 0].Value = "";
            grd_invoice[6, 0].Value = "";
            grd_invoice[7, 0].Value = "";
            grd_invoice[8, 0].Value = "";
            grd_invoice[9, 0].Value = "";
            grd_invoice[10, 0].Value = "";
            grd_invoice[11, 0].Value = "";
            grd_invoice[12, 0].Value = "";
            grd_invoice[13, 0].Value = "";

            foreach (DataGridViewColumn col in grd_summary.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        private void Bind_GridSummary(DataGridView grd_summary)
        {
            // Row databind in datatable
            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("Type");
            dt.Columns.Add("Status");
            dt.Columns.Add("No Of Records");
            dt.Columns.Add("Taxable Value");
            dt.Columns.Add("Total IGST");
            dt.Columns.Add("Total CGST");
            dt.Columns.Add("Total SGST");
            

            DataRow dr = dt.NewRow();
            dr[0] = "1";
            dr[1] = "Input Tax Credit Recieved For distribution";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "2";
            dr[1] = "Debit/Credit notes (including amendments thereof) received during current tax period ";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";

            dt.Rows.Add(dr);

          

            dr = dt.NewRow();
            dr[0] = "3";
            dr[1] = "Total";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dt.Rows.Add(dr);
            

            grd_summary.DataSource = dt;


           

            
            
           
           

            
            



            //grd_summary.Rows.Add(4);

            //grd_summary.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ////grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;


            //grd_summary[0, 0].Value = "1";
            //grd_summary[1, 0].Value = "Input Tax Credit Recieved  for distribution[B2B]";
            //grd_summary[2, 0].Value = "0";
            //grd_summary[3, 0].Value = "0";
            //grd_summary[4, 0].Value = "0";
            //grd_summary[5, 0].Value = "0";
            //grd_summary[6, 0].Value = "0";
            //grd_summary[7, 0].Value = "0";
            //grd_summary[8, 0].Value = "0";





            //grd_summary[0, 1].Value = "2 ";
            //grd_summary[1, 1].Value = "Debit Credit  notes(including  amendment there of ) recevied  during current tax period [CDN]";
            //grd_summary[2, 1].Value = "0";

            //grd_summary[3, 1].Value = "0";
            //grd_summary[4, 1].Value = "0";
            //grd_summary[5, 1].Value = "0";
            //grd_summary[6, 1].Value = "0 ";
            //grd_summary[7, 1].Value = "0";
            //grd_summary[8, 1].Value = "0";





            //grd_summary[0, 2].Value = "3";
            //grd_summary[1, 2].Value = "Amendments  to input tax  credit received  for distribution ";
            //grd_summary[2, 2].Value = "0";

            //grd_summary[3, 2].Value = "0";
            //grd_summary[4, 2].Value = "0";
            //grd_summary[5, 2].Value = "0";
            //grd_summary[6, 2].Value = "0 ";
            //grd_summary[7, 2].Value = "0";
            //grd_summary[8, 2].Value = "0";



            //grd_summary[0, 3].Value = "4 ";
            //grd_summary[1, 3].Value = "Total";
            //grd_summary[2, 3].Value = "0";

            //grd_summary[3, 3].Value = "0";
            //grd_summary[4, 3].Value = "0";
            //grd_summary[5, 3].Value = "0";
            //grd_summary[6, 3].Value = "0 ";
            //grd_summary[7, 3].Value = "0";
            //grd_summary[8, 3].Value = "0";








            foreach (DataGridViewColumn col in grd_summary.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        private void Bind_Gridinput(DataGridView grd_input)

        {

            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
            dt.Columns.Add("Supplier GSTIN/UID");
            dt.Columns.Add("Supplier Name");
            dt.Columns.Add("Note Type");
            dt.Columns.Add("Credit/Debit Note No.");
            dt.Columns.Add("Credit/Debit Note Date");
            dt.Columns.Add("Original Invoice No ");
            dt.Columns.Add("Original Invoice Date");
            dt.Columns.Add("Revised Credit/Debit Note No");
            dt.Columns.Add("Revised Credit/Debit Note Date");
            dt.Columns.Add("Revised Invoice Date");
            dt.Columns.Add("Taxable Value");

            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("CESS");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Return Filed ");

            grd_input.DataSource = dt;
        //    grd_input.Rows.Add(1);

           // grd_input.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        //    //grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;


        //    grd_input[0, 0].Value = "";
        //    grd_input[1, 0].Value = "";
        //    grd_input[2, 0].Value = "";
        //    grd_input[3, 0].Value = "";
        //    grd_input[4, 0].Value = "";
        //    grd_input[5, 0].Value = "";
        //    grd_input[6, 0].Value = "";
        //    grd_input[7, 0].Value = "";
            foreach (DataGridViewColumn col in grd_summary.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        private void Bind_grd_debit(DataGridView grd_debit)
        {


            DataTable dt = new DataTable();
        
            dt.Columns.Add("S No.");
            dt.Columns.Add("Supplier GSTIN");
            dt.Columns.Add("Supplier Name");
            dt.Columns.Add("Note Type");
            dt.Columns.Add("Credit/Debit Note No.");
            dt.Columns.Add("Credit/Debit Note Date");
            dt.Columns.Add("Original Invoice No ");
            dt.Columns.Add("Original Invoice Date");
            dt.Columns.Add("Note Value");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Taxable Value");

            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("CESS");
         
            dt.Columns.Add("Return Filed ");
            grd_debit.DataSource = dt;
            //grd_debit.Rows.Add(1);

            //grd_debit.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ////grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;


            //grd_debit[0, 0].Value = "";
            //grd_debit[1, 0].Value = "";
            //grd_debit[2, 0].Value = "";
            //grd_debit[3, 0].Value = "";
            //grd_debit[4, 0].Value = "";
            //grd_debit[5, 0].Value = "";
            //grd_debit[6, 0].Value = "";
            //grd_debit[7, 0].Value = "";
            foreach (DataGridViewColumn col in grd_summary.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void GSTR_6A_Reports_Load(object sender, EventArgs e)
        {
            GetSetSoftwareData();
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();

        }

        private void grd_summary_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabSoftware_DrawItem(object sender, DrawItemEventArgs e)
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

        private void grd_summary_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex == 0)
            {
                tabSoftware.SelectTab(1);

            }
            if (e.RowIndex == 1)
            {
                tabSoftware.SelectTab(2);

            }
            
            
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
