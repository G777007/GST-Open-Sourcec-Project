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
    public partial class GSTR_4A_Reports : Form
    {
        public GSTR_4A_Reports()
        {
            InitializeComponent();
        }

        private void Bind_GridSummary(DataGridView grdSummary)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("Table No");
            dt.Columns.Add("Type Of Invoice ");
            dt.Columns.Add("Status");
            dt.Columns.Add("Discount Count");
            dt.Columns.Add("Taxable Value");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");


            DataRow dr = dt.NewRow();

            dr[0] = "Table 3A,3B";
            dr[1] = "B2B Invoices";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Table 4";
            dr[1] = "Credit/Debit Notes";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Table 4";
            dr[1] = "Amendments to B2B Invoices";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Table 4";
            dr[1] = "Amendments to Credit/Debit Notes";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dt.Rows.Add(dr);

            grdSummary.DataSource = dt;

            //grdSummary.Rows.Add(4);

            //grdSummary.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ////grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;


            //grdSummary[0, 0].Value = "1 ";
            //grdSummary[1, 0].Value = "Inward Supplies received from a registered Person";
            //grdSummary[2, 0].Value = "0";
            //grdSummary[3, 0].Value = "0";
            //grdSummary[4, 0].Value = "0";
            //grdSummary[5, 0].Value = "0";
            //grdSummary[6, 0].Value = "0";
            //grdSummary[7, 0].Value = "0";
            //grdSummary[8, 0].Value = "0";



            //grdSummary[0, 1].Value = "2";
            //grdSummary[1, 1].Value = "Details Of Credit /Debit Notes";
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


            //grdSummary[0, 3].Value = "4";
            //grdSummary[1, 3].Value = "Total";
            //grdSummary[2, 3].Value = "0";
            //grdSummary[3, 3].Value = "0";
            //grdSummary[4, 3].Value = "0";
            //grdSummary[5, 3].Value = "0";
            //grdSummary[6, 3].Value = "0 ";
            //grdSummary[7, 3].Value = "0";
            //grdSummary[8, 3].Value = "0";


            //foreach (DataGridViewColumn col in grdSummary.Columns)
            //{
            //    col.SortMode = DataGridViewColumnSortMode.NotSortable;
            //}
        }

        private void Bind_Grd_invoices(DataGridView Grd_invoices )
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
            dt.Columns.Add("Supplier GSTIN");
            dt.Columns.Add("Supplier Name");
            dt.Columns.Add("Invoice Number ");
           
            dt.Columns.Add("Invoice Date");
            dt.Columns.Add("Total Invoice Value");
            dt.Columns.Add("Taxable Value");
         
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("CESS");
            dt.Columns.Add("Place of Supply");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Invoice Type");
            dt.Columns.Add("Reverse Charge");
            dt.Columns.Add("Return Filed ");

            Grd_invoices.DataSource = dt;
        }

        private void Bind_grid_credit(DataGridView grid_credit)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
            dt.Columns.Add("Supplier GSTIN/UID");
            dt.Columns.Add("Supplier Name");
            dt.Columns.Add("Invoice Number ");
            dt.Columns.Add("Note Type");
            dt.Columns.Add("Credit/Debit Note No");
            dt.Columns.Add("Credit/Debit Note Date");
            dt.Columns.Add("Original Invoice Date");
            dt.Columns.Add("Original Invoices No");
            dt.Columns.Add("Pre GST Regime ");
            dt.Columns.Add("Note Value");
            dt.Columns.Add("Taxable Value");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("CESS");
            dt.Columns.Add("Rate");          
            dt.Columns.Add("Return Filed ");

            grid_credit.DataSource = dt;
        }

        private void Bind_Grd_AB2BInvoices(DataGridView Grd_AB2BInvoices)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
            dt.Columns.Add("Supplier GSTIN");
            dt.Columns.Add("Supplier Name");
            dt.Columns.Add("Original Invoice Number ");
            dt.Columns.Add("Original Invoice Date");
            dt.Columns.Add("Revised Invoice Number ");
            dt.Columns.Add("Revised Invoice Date");
            dt.Columns.Add("Total Invoice Value");
            dt.Columns.Add("Taxable Value");

            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("CESS");
            dt.Columns.Add("Place of Supply");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Invoice Type");
            dt.Columns.Add("Reverse Charge");
            dt.Columns.Add("Return Filed ");

            Grd_AB2BInvoices.DataSource = dt;
        }

        private void DataBind_Grd_ACDN(DataGridView Grd_ACDN)
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

            DataRow dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";
            dr[5] = "";
            dr[6] = "";
            dr[7] = "";
            dr[8] = "";
            dr[9] = "";
            dr[10] = "";
            dr[11] = "";
            dr[12] = "";
            dr[13] = "";
            dr[14] = "";
            dr[15] = "";
            dr[16] = "";
            dr[17] = "";

            Grd_ACDN.DataSource = dt;
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

                Grd_invoices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                Bind_Grd_invoices(Grd_invoices);
                //grd_check.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                // grd_check.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                // grd_check.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                //grd_check.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                //grid_year.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                Bind_grid_credit(grid_credit);
                // grid_year.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                //  grid_year.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                //  grid_year.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                //  grid_year.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                Bind_Grd_AB2BInvoices(Grd_AB2BInvoices);

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

        private void grdSummary_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void GSTR_4A_Reports_Load(object sender, EventArgs e)
        {
            GetSetSoftwareData();
        }

        private void tabControl2_DrawItem(object sender, DrawItemEventArgs e)
        {

            TabControl tabcntrl = sender as TabControl;
            Graphics g = e.Graphics;
            Brush _textBrush;

            // Get the item from the collection.
            TabPage _tabPage = tabsoftware.TabPages[e.Index];

            // Get the real bounds for the tab rectangle.
            Rectangle _tabBounds = tabsoftware.GetTabRect(e.Index);

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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void grdSummary_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grdSummary_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex==0)
            {
                tabsoftware.SelectTab(1);
            }
            if (e.RowIndex == 1)
            {
                tabsoftware.SelectTab(2);
            }
            if (e.RowIndex == 2)
            {
                tabsoftware.SelectTab(3);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
