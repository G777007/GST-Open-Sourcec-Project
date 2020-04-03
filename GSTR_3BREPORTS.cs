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
    public partial class GSTR_3BREPORTS : Form
    {
        public GSTR_3BREPORTS()
        {
            InitializeComponent();
        }
        private void Bind_GridSummary(DataGridView grdSummary)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Description");
            dt.Columns.Add("Validation Status ");
            dt.Columns.Add("Taxable Value");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("Cess");
            dt.Columns.Add("Total");

            DataRow dr = dt.NewRow();
            dr[0] = "Tax on Outward Supply & Tax Under RCM on Inward Supply Table [3.1]";
            dr[1] = "";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";

            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Interstate Supply Table [3.2]";
            dr[1] = "";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "";
            dr[5] = "";
            dr[6] = "0";
            dr[7] = "0";

            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Eligible ITC [Table 4]";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";

            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Exempt, Nil and Non-GST Inward Supplies[Table 5]";
            dr[1] = "";
            dr[2] = "0";
            dr[3] = "";
            dr[4] = "";
            dr[5] = "";
            dr[6] = "";
            dr[7] = "";

            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Interest[Table 5.1]";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "0";
            dr[4] = "";
            dr[5] = "";
            dr[6] = "";
            dr[7] = "";

            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Late Fees [Auto calculated by GST Portal 5.1]";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "";
            dr[7] = "0";

            dt.Rows.Add(dr);
            grdSummary.DataSource = dt;

            //grdSummary.Rows.Add(5);

            //grdSummary.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ////grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;


            //grdSummary[0, 0].Value = "1 ";
            //grdSummary[1, 0].Value = "Detail Of Outward Supplies and inward supplies liable to reverse Charge";
            //grdSummary[2, 0].Value = "0";
            //grdSummary[3, 0].Value = "0";
            //grdSummary[4, 0].Value = "0";
            //grdSummary[5, 0].Value = "0";
            //grdSummary[6, 0].Value = "0";
            //grdSummary[7, 0].Value = "0";

            //grdSummary[0, 1].Value = "2 ";
            //grdSummary[1, 1].Value = "Details of Inter-State Supplies  made to Unregistered Person, composition  taxable Person and UIN Holders";
            //grdSummary[2, 1].Value = "0";
            //grdSummary[3, 1].Value = "0";
            //grdSummary[4, 1].Value = "0";
            //grdSummary[5, 1].Value = "0";
            //grdSummary[6, 1].Value = "0 ";
            //grdSummary[7, 1].Value = "0";

            //grdSummary[0, 2].Value = "3 ";
            //grdSummary[1, 2].Value = "Eligibile ITC ";
            //grdSummary[2, 2].Value = "0";
            //grdSummary[3, 2].Value = "0";
            //grdSummary[4, 2].Value = "0";
            //grdSummary[5, 2].Value = "0";
            //grdSummary[6, 2].Value = "0 ";
            //grdSummary[7, 2].Value = "0";

            //grdSummary[0, 3].Value = "4 ";
            //grdSummary[1, 3].Value = "Value of exempt, nil-rated   and non-GST Inward Supplies";
            //grdSummary[2, 3].Value = "0";
            //grdSummary[3, 3].Value = "0";
            //grdSummary[4, 3].Value = "0";
            //grdSummary[5, 3].Value = "0";
            //grdSummary[6, 3].Value = "0 ";
            //grdSummary[7, 3].Value = "0";

            //grdSummary[0, 4].Value = "5 ";
            //grdSummary[1, 4].Value = "Payment Tax";
            //grdSummary[2, 4].Value = "0";
            //grdSummary[3, 4].Value = "0";
            //grdSummary[4, 4].Value = "0";
            //grdSummary[5, 4].Value = "0";
            //grdSummary[6, 4].Value = "0 ";
            //grdSummary[7, 4].Value = "0";

            foreach (DataGridViewColumn col in grdSummary.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        private void Bind_grid_inter(DataGridView grid_inter)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
            dt.Columns.Add("Supply Type");
            dt.Columns.Add("place of supply");
            dt.Columns.Add("Taxable Value");
            dt.Columns.Add("Amount of Intergrated Tax");

            grid_inter.DataSource = dt;

            //grid_inter.Rows[0].DefaultCellStyle.BackColor = Color.Navy;
            //grid_inter.Rows.Add(1);

            //grid_inter.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ////grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;


            //grid_inter[0, 0].Value = "";
            //grid_inter[1, 0].Value = "";
            //grid_inter[2, 0].Value = "";
            //grid_inter[3, 0].Value = "";
            //grid_inter[4, 0].Value = "";
            //grid_inter[5, 0].Value = "";


            foreach (DataGridViewColumn col in grid_inter.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void Bind_grid_value(DataGridView grid_value)
        {
            grid_value.Rows.Add(2);

            grid_value.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;


            grid_value[0, 0].Value = "From a supplier under composition scheme, Exempt and Nil rated Supply";
            grid_value[1, 0].Value = "";
            grid_value[2, 0].Value = "";

            grid_value[0, 1].Value = "Non GST Supply";
            grid_value[1, 1].Value = "";
            grid_value[2, 1].Value = "";



            foreach (DataGridViewColumn col in grid_value.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        private void Bind_grid_payment(DataGridView grid_Payment)
        {
            grid_Payment.Rows.Add(4);

            //grid_Payment.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;


            grid_Payment[0, 0].Value = "Integrated Tax";
            grid_Payment[1, 0].Value = "";
            grid_Payment[2, 0].Value = "";
            grid_Payment[3, 0].Value = "";
            grid_Payment[4, 0].Value = "";
            grid_Payment[5, 0].Value = "";
            grid_Payment[6, 0].Value = "";
            grid_Payment[7, 0].Value = "";
            grid_Payment[8, 0].Value = "";
            grid_Payment[9, 0].Value = "";
            grid_Payment[10, 0].Value = "";
            grid_Payment[11, 0].Value = "";
            grid_Payment[12, 0].Value = "";
            grid_Payment[13, 0].Value = "";
            grid_Payment[14, 0].Value = "";


            grid_Payment[0, 1].Value = "Central Tax";
            grid_Payment[1, 1].Value = "";
            grid_Payment[2, 1].Value = "";
            grid_Payment[3, 1].Value = "";
            grid_Payment[4, 1].Value = "";
            grid_Payment[5, 1].Value = "";
            grid_Payment[6, 1].Value = "";
            grid_Payment[7, 1].Value = "";
            grid_Payment[8, 1].Value = "";
            grid_Payment[9, 1].Value = "";
            grid_Payment[10, 1].Value = "";
            grid_Payment[11, 1].Value = "";
            grid_Payment[12, 1].Value = "";
            grid_Payment[13, 1].Value = "";
            grid_Payment[14, 1].Value = "";


            grid_Payment[0, 2].Value = "State/UT Tax";
            grid_Payment[1, 2].Value = "";
            grid_Payment[2, 2].Value = "";
            grid_Payment[3, 2].Value = "";
            grid_Payment[4, 2].Value = "";
            grid_Payment[5, 2].Value = "";
            grid_Payment[6, 2].Value = "";
            grid_Payment[7, 2].Value = "";
            grid_Payment[8, 2].Value = "";
            grid_Payment[9, 2].Value = "";
            grid_Payment[10, 2].Value = "";
            grid_Payment[11, 2].Value = "";
            grid_Payment[12, 2].Value = "";
            grid_Payment[13, 2].Value = "";
            grid_Payment[14, 2].Value = "";


            grid_Payment[0, 3].Value = "Cess";
            grid_Payment[1, 3].Value = "";
            grid_Payment[2, 3].Value = "";
            grid_Payment[3, 3].Value = "";
            grid_Payment[4, 3].Value = "";
            grid_Payment[5, 3].Value = "";
            grid_Payment[6, 3].Value = "";
            grid_Payment[7, 3].Value = "";
            grid_Payment[8, 3].Value = "";
            grid_Payment[9, 3].Value = "";
            grid_Payment[10, 3].Value = "";
            grid_Payment[11, 3].Value = "";
            grid_Payment[12, 3].Value = "";
            grid_Payment[13, 3].Value = "";
            grid_Payment[14, 3].Value = "";




            foreach (DataGridViewColumn col in grid_Payment.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        private void GetSetSoftwareData()
        {
            try
            {


                grdSummary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;  // OK
                Bind_GridSummary(grdSummary);


                grid_inter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                Bind_grid_inter(grid_inter);


                grid_eligible.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                Bind_grid_eligible(grid_eligible);
                //grid_eligible.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                //grid_eligible.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                grid_detail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                Bind_grid_detail(grid_detail);
                //grid_detail.autosizecolumnsmode = datagridviewautosizecolumnsmode.allcells;
                //grid_detail.columns[0].autosizemode = datagridviewautosizecolumnmode.fill;
              

                grid_value.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                Bind_grid_value(grid_value);


                Bind_grd_payment(grd_payment);
                Bind_dataGridView1(dataGridView1);

                Bind_datagridview2(dataGridView2);

                //Bind_grdnewsummary(grd_newsummary);
              //  Bind_grid_ptax(Grd_paTax);

                DataBind_Grd_ptax(Grd_ptax);
                DataBind_Grd_paTax(Grd_paTax);



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

        private void GSTR_3BREPORTS_Load(object sender, EventArgs e)
        {
            GetSetSoftwareData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void grdSummary_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {


        }

        private void grid_eligible_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void Bind_grid_eligible(DataGridView grid_eligible)
        {
            // grid_eligible.Rows.Add(13);

            //grid_eligible.Rows[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //grid_eligible.Rows[0].DefaultCellStyle.BackColor = Color.SkyBlue;
            //grid_eligible.Rows[6].DefaultCellStyle.BackColor = Color.Silver;
            //grid_eligible.Rows[9].DefaultCellStyle.BackColor = Color.Silver;
            //grid_eligible.Rows[10].DefaultCellStyle.BackColor = Color.Silver;
            //grid_eligible.Rows[6].DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);
            //grid_eligible.Rows[9].DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);
            //grid_eligible.Rows[10].DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);
            //grid_eligible.Rows[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //grid_eligible.Rows[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //grid_eligible.Rows[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //grid_eligible.Rows[11].DefaultCellStyle.BackColor = Color.SkyBlue;


            DataTable dt = new DataTable();
            dt.Columns.Add("Description");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("CESS");
            dt.Columns.Add("Total");

            DataRow dr = dt.NewRow();

            dr[0] = "(A) ITC Available (Whether in full or part)";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";
            dr[5] = "";

            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "   (1) Import of goods";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";

            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "   (2) Import of services";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";

            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "   (3) Inward Supplies liable to reverse charge(other than 1 & 2 above)";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";

            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "   (4) Inward supplies from ISD";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";

            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "   (5) All other ITC";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";

            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "(A) Total ITC Available";
            dr[1] = "0.0";
            dr[2] = "0.0";
            dr[3] = "0.0";
            dr[4] = "0.0";

            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "(B) ITC Reversed";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";

            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "   (1) As per rules 42 & 43 IGST Rules";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";

            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "   (2) Others";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";

            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "(B) Total ITC Reversed";
            dr[1] = "0.0";
            dr[2] = "0.0";
            dr[3] = "0.0";
            dr[4] = "0.0";

            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";

            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "Net ITC Available  [C = A - B ]";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";

            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "(D) Ineligible ITC";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";

            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "   (1) As per section 17(5)";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";

            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "   (2) Others";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";

            dt.Rows.Add(dr);


            grid_eligible.DataSource = dt;

            //grid_eligible.DefaultCellStyle.BackColor = Color.Salmon;

            foreach (DataGridViewColumn col in grid_eligible.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }


        }




        //grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;


        //grid_eligible[0, 0].Value = "ITC Available(Whether in full or part) ";
        //grid_eligible[1, 0].Value = "";
        //grid_eligible[2, 0].Value = "";
        //grid_eligible[3, 0].Value = "";
        //grid_eligible[4, 0].Value = "";

        //grid_eligible[0, 1].Value = "(1)Imports of goods ";
        //grid_eligible[1, 1].Value = "";
        //grid_eligible[2, 1].Value = "";
        //grid_eligible[3, 1].Value = "";
        //grid_eligible[4, 1].Value = "";

        //grid_eligible[0, 2].Value = "   (2) Import of Services";
        //grid_eligible[1, 2].Value = "";
        //grid_eligible[2, 2].Value = "";
        //grid_eligible[3, 2].Value = "";
        //grid_eligible[4, 2].Value = "";

        //grid_eligible[0, 3].Value = "   (3) Inward Supplies liable to reverse charge(other than 1 & 2 above) ";
        //grid_eligible[1, 3].Value = "";
        //grid_eligible[2, 3].Value = "";
        //grid_eligible[3, 3].Value = "";
        //grid_eligible[4, 3].Value = "";

        //grid_eligible[0, 4].Value = "   (4) Inward supplies from ISD";
        //grid_eligible[1, 4].Value = "";
        //grid_eligible[2, 4].Value = "";
        //grid_eligible[3, 4].Value = "";
        //grid_eligible[4, 4].Value = "";

        //grid_eligible[0, 5].Value = "      (5) All other ITC";
        //grid_eligible[1, 5].Value = "";
        //grid_eligible[2, 5].Value = "";
        //grid_eligible[3, 5].Value = "";
        //grid_eligible[4, 5].Value = "";

        //grid_eligible[0, 6].Value = "Total ITC Available (A)";
        //grid_eligible[1, 6].Value = "0.0";
        //grid_eligible[2, 6].Value = "0.0";
        //grid_eligible[3, 6].Value = "0.0";
        //grid_eligible[4, 6].Value = "0.0";

        //grid_eligible[0, 7].Value = "(B) ITC Reversed";
        //grid_eligible[1, 7].Value = "";
        //grid_eligible[2,7].Value = "";
        //grid_eligible[3, 7].Value = "";
        //grid_eligible[4, 7].Value = "";

        //grid_eligible[0, 8].Value = "      (2) Others ";
        //grid_eligible[1, 8].Value = "";
        //grid_eligible[2, 8].Value = "";
        //grid_eligible[3, 8].Value = "";
        //grid_eligible[4, 8].Value = "";

        //grid_eligible[0, 9].Value = "Total ITC Reversed (B) ";
        //grid_eligible[1, 9].Value = "0.0";
        //grid_eligible[2, 9].Value = "0.0";
        //grid_eligible[3, 9].Value = "0.0";
        //grid_eligible[4, 9].Value = "0.0";

        //grid_eligible[0, 10].Value = "(C) Net ITC Available (A) – (B) ";
        //grid_eligible[1, 10].Value = "0.0";
        //grid_eligible[2, 10].Value = "0.0";
        //grid_eligible[3, 10].Value = "0.0";
        //grid_eligible[4, 10].Value = "0.0";

        //grid_eligible[0, 11].Value = "(D) Ineligible ITC ";
        //grid_eligible[1, 11].Value = "";
        //grid_eligible[2, 11].Value = "";
        //grid_eligible[3, 11].Value = "";
        //grid_eligible[4,11].Value = "";

        //grid_eligible[0, 12].Value = "   (1) As per section 17(5)";
        //grid_eligible[1, 12].Value = "";
        //grid_eligible[2, 12].Value = "";
        //grid_eligible[3, 12].Value = "";
        //grid_eligible[4, 12].Value = "";





        //foreach (DataGridViewColumn col in grid_eligible.Columns)
        //{
        //    col.SortMode = DataGridViewColumnSortMode.NotSortable;
        //}

        private void Bind_grid_detail(DataGridView grid_detail)
        {

            DataTable dt = new DataTable();

            dt.Columns.Add("Nature Of Supplies ");
            dt.Columns.Add("Total Taxable Value");
            dt.Columns.Add("Integrated tax");
            dt.Columns.Add("Central tax");
            dt.Columns.Add("State/UT Tax");
            dt.Columns.Add("CESS");
            dt.Columns.Add("Total");

            DataRow dr = dt.NewRow();
            dr[0] = "(a) Outward Taxable Supplies (other than Zero rated,nil rated and exemted)";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";
            dr[5] = "";

            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "(b) Outward Taxable Supplies (Zero rated)";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";
            dr[5] = "";

            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "(c) Other outward Supplies (Nil rated, exemted)";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";
            dr[5] = "";

            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "(d) Inward Supplies (liable to reverse charge)";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";
            dr[5] = "";

            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "(e) Non-GST outward supplies";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";
            dr[5] = "";

            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Total";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";
            dr[5] = "";

            dt.Rows.Add(dr);

            grid_detail.DataSource = dt;
       
            //grid_detail.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //grid_detail.Columns[0].Width = 500;


            //grid_detail.Rows.Add(5);

            //grid_detail.Rows[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ////grid_eligible.Rows[0].DefaultCellStyle.BackColor = Color.MediumSpringGreen;
            ////grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;


            //grid_detail[0, 0].Value = "(a) Outward Taxable Supplies (other than Zero rated,nil rated and exemted) ";
            //grid_detail[1, 0].Value = "";
            //grid_detail[2, 0].Value = "";
            //grid_detail[3, 0].Value = "";
            //grid_detail[4, 0].Value = "";
            //grid_detail[5, 0].Value = "";

            //grid_detail[0, 1].Value = "(b) Outward Taxable Supplies (Zero rated) ";
            //grid_detail[1, 1].Value = "";
            //grid_detail[2, 1].Value = "";
            //grid_detail[3, 1].Value = "";
            //grid_detail[4, 1].Value = "";
            //grid_detail[5, 1].Value = "";

            //grid_detail[0, 2].Value = "(c) Other outward Supplies(Nil rated, exemted)";
            //grid_detail[1, 2].Value = "";
            //grid_detail[2, 2].Value = "";
            //grid_detail[3, 2].Value = "";
            //grid_detail[4, 2].Value = "";
            //grid_detail[5, 2].Value = "";

            //grid_detail[0, 3].Value = "(d) Inward Supplies(liable to reverse charge) ";
            //grid_detail[1, 3].Value = "";
            //grid_detail[2, 3].Value = "";
            //grid_detail[3, 3].Value = "";
            //grid_detail[4, 3].Value = "";
            //grid_detail[5, 3].Value = "";

            //grid_detail[0, 4].Value = "(e) Non-GST outward supplies";
            //grid_detail[1, 4].Value = "";
            //grid_detail[2, 4].Value = "";
            //grid_detail[3, 4].Value = "";
            //grid_detail[4, 4].Value = "";
            //grid_detail[5, 4].Value = "";





            //foreach (DataGridViewColumn col in grid_detail.Columns)
            //{
            //    col.SortMode = DataGridViewColumnSortMode.NotSortable;
            //}
           

        }

        private void Bind_dataGridView1(DataGridView dataGridView1)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Return Type");
            dt.Columns.Add("Original Value");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("Cess");
            dt.Columns.Add("Total GST");
            dt.Columns.Add("Zero Value");
            dt.Columns.Add("Zero IGST");
            dt.Columns.Add("NValue");

            DataRow dr = dt.NewRow();
            dr[0] = "GSTR-3B";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";
            dr[5] = "";
            dr[6] = "";
            dr[7] = "";
            dr[8] = "";
            dr[9] = "";

            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "GSTR-1";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";
            dr[5] = "";
            dr[6] = "";
            dr[7] = "";
            dr[8] = "";
            dr[9] = "";

            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Difference";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";
            dr[5] = "";
            dr[6] = "";
            dr[7] = "";
            dr[8] = "";
            dr[9] = "";

            dt.Rows.Add(dr);


            dataGridView1.DataSource = dt;
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        }

        private void DataBind_Grd_paTax(DataGridView Grd_paTax)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Description");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("Cess");
            dt.Columns.Add("Total");

            DataRow dr = dt.NewRow();
            dr[0] = "Tax Paid Through Cash [Table 6.1]";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            

            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Tax Paid Through Credit [Table 6.1]";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            
            dt.Rows.Add(dr);

            Grd_paTax.DataSource = dt;

            Grd_paTax.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        }
        private void Bind_datagridview2(DataGridView datagridview2)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Description");
            dt.Columns.Add("Current Taxable Value");
            dt.Columns.Add("Current CGST");
            dt.Columns.Add("Current SGST");
            dt.Columns.Add("Current IGST");
            dt.Columns.Add("Cess");
            dt.Columns.Add("Previous Taxable Value");
            dt.Columns.Add("Previous CGST");
            dt.Columns.Add("Previous SGST");
            dt.Columns.Add("Previous IGST");
            dt.Columns.Add("PreviousCess");

            DataRow dr = dt.NewRow();
            dr[0] = "Difference in Taxable Supply";
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

            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "Difference in Zero Rated  Supply";
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
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "Difference in Nill Rated  Supply";
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
            dt.Rows.Add(dr);
            dataGridView2.DataSource = dt;
            datagridview2.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
     
       
        private void Bind_grd_payment(DataGridView grd_payment)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Description ");
            dt.Columns.Add("Other than reverse reverse charge  ");
            dt.Columns.Add("Intergrated Tax ");
            dt.Columns.Add("Central tax ");
            dt.Columns.Add("State/UT Tax ");
            dt.Columns.Add("Cess");
            dt.Columns.Add("Total Paid Through ITC");
            dt.Columns.Add("Balance Payable in Cash");

            DataRow dr = dt.NewRow();
            dr[0] = "Intergrated Tax";
            dr[1] = "0.0";
            dr[2] = "0.0";
            dr[3] = "0.0";
            dr[4] = "0.0";
            dr[5] = "0.0";
            dr[6] = "0.0";
            dr[7] = "0.0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Central  Tax";
            dr[1] = "0.0";
            dr[2] = "0.0";
            dr[3] = "0.0";
            dr[4] = "0.0";
            dr[5] = "0.0";
            dr[6] = "0.0";
            dr[7] = "0.0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "State/UTTax";
            dr[1] = "0.0";
            dr[2] = "0.0";
            dr[3] = "0.0";
            dr[4] = "0.0";
            dr[5] = "0.0";
            dr[6] = "0.0";
            dr[7] = "0.0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Cess";
            dr[1] = "0.0";
            dr[2] = "0.0";
            dr[3] = "0.0";
            dr[4] = "0.0";
            dr[5] = "0.0";
            dr[6] = "0.0";
            dr[7] = "0.0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Total";
            dr[1] = "0.0";
            dr[2] = "0.0";
            dr[3] = "0.0";
            dr[4] = "0.0";
            dr[5] = "0.0";
            dr[6] = "0.0";
            dr[7] = "0.0";
            dt.Rows.Add(dr);
            grd_payment.DataSource = dt;
            foreach (DataGridViewColumn col in grd_payment.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }


        }

        private void DataBind_Grd_ptax(DataGridView Grd_ptax)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Description");
            dt.Columns.Add("Balance Payable in Cash ");
            dt.Columns.Add("RCM Taxable Payable ");
            dt.Columns.Add("RCM  tax to be paid In Cash ");
            dt.Columns.Add("Interest  Payable ");
            dt.Columns.Add("Interset to be Paid in cash  ");
            dt.Columns.Add("Late Fee Payble  ");
            dt.Columns.Add("Late Fee to be  paid in cash  ");

            DataRow dr = dt.NewRow();
            dr[0] = "Intergrated Tax ";
            dr[1] = "0.0 ";
            dr[2] = "0.0";
            dr[3] = " 0.0";
            dr[4] = "0.0";
            dr[5] = " 0.0";
            dr[6] = "0.0 ";
            dr[7] = "0.0";

            dt.Rows.Add(dr);
            dr = dt.NewRow();

            dr[0] = "Central  Tax ";
            dr[1] = "0.0 ";
            dr[2] = "0.0";
            dr[3] = " 0.0";
            dr[4] = "0.0";
            dr[5] = " 0.0";
            dr[6] = "0.0 ";
            dr[7] = "0.0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "State/UT  Tax ";
            dr[1] = "0.0 ";
            dr[2] = "0.0";
            dr[3] = " 0.0";
            dr[4] = "0.0";
            dr[5] = " 0.0";
            dr[6] = "0.0 ";
            dr[7] = "0.0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Cess Tax ";
            dr[1] = "0.0 ";
            dr[2] = "0.0";
            dr[3] = " 0.0";
            dr[4] = "0.0";
            dr[5] = " 0.0";
            dr[6] = "0.0 ";
            dr[7] = "0.0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();



            dr[0] = "Total";
            dr[1] = "0.0 ";
            dr[2] = "0.0";
            dr[3] = " 0.0";
            dr[4] = "0.0";
            dr[5] = " 0.0";
            dr[6] = "0.0 ";
            dr[7] = "0.0";
            dt.Rows.Add(dr);

            Grd_ptax.DataSource = dt;


        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void grd_newsummary_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void Grd_paTax_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabSoftware_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabSoftware.SelectedIndex == 0)
            {
                grdSummary.RowHeadersDefaultCellStyle.BackColor = Color.SkyBlue;

                grdSummary.Rows[1].Cells[4].Style.BackColor = Color.Silver;
                grdSummary.Rows[1].Cells[5].Style.BackColor = Color.Silver;
                grdSummary.Rows[2].Cells[2].Style.BackColor = Color.Silver;
                grdSummary.Rows[3].Cells[4].Style.BackColor = Color.Silver;
                grdSummary.Rows[3].Cells[5].Style.BackColor = Color.Silver;
                grdSummary.Rows[3].Cells[6].Style.BackColor = Color.Silver;
                grdSummary.Rows[3].Cells[7].Style.BackColor = Color.Silver;

                grdSummary.Rows[4].Cells[2].Style.BackColor = Color.Silver;
                grdSummary.Rows[5].Cells[2].Style.BackColor = Color.Silver;
                grdSummary.Rows[5].Cells[3].Style.BackColor = Color.Silver;
                grdSummary.Rows[5].Cells[6].Style.BackColor = Color.Silver;
                grdSummary.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

              

                if(tabSoftware.SelectedIndex == 5)
                {
                    grd_payment.Rows[1].Cells[4].Style.BackColor = Color.Silver;
                    grd_payment.Rows[1].Cells[5].Style.BackColor = Color.Silver;
                    grd_payment.Rows[2].Cells[1].Style.BackColor = Color.Silver;
                    grd_payment.Rows[3].Cells[1].Style.BackColor = Color.Silver;
                    grd_payment.Rows[4].Cells[1].Style.BackColor = Color.Silver;
                    grd_payment.Rows[3].Cells[2].Style.BackColor = Color.Silver;
                    grd_payment.Rows[3].Cells[3].Style.BackColor = Color.Silver;
                    grd_payment.Rows[3].Cells[4].Style.BackColor = Color.Silver;

                    grd_payment.Rows[1].Cells[1].Style.BackColor = Color.Silver;
                    grd_payment.Rows[2].Cells[1].Style.BackColor = Color.Silver;
                    grd_payment.Rows[0].Cells[1].Style.BackColor = Color.Silver;
                    grd_payment.Rows[0].Cells[5].Style.BackColor = Color.Silver;
                    grd_payment.Rows[2].Cells[3].Style.BackColor = Color.Silver;
                    grd_payment.Rows[1].Cells[5].Style.BackColor = Color.Silver;
                    grd_payment.Rows[2].Cells[5].Style.BackColor = Color.Silver;
                }
                //grd_payment.Rows[1].Cells[4].Style.BackColor = Color.Silver;
                //grd_payment.Rows[1].Cells[5].Style.BackColor = Color.Silver;
                //grd_payment.Rows[2].Cells[1].Style.BackColor = Color.Silver;
                //grd_payment.Rows[3].Cells[1].Style.BackColor = Color.Silver;
                //grd_payment.Rows[4].Cells[1].Style.BackColor = Color.Silver;
                //grd_payment.Rows[3].Cells[2].Style.BackColor = Color.Silver;
                //grd_payment.Rows[3].Cells[3].Style.BackColor = Color.Silver;
                //grd_payment.Rows[3].Cells[4].Style.BackColor = Color.Silver;

                //grd_payment.Rows[1].Cells[1].Style.BackColor = Color.Silver;
                //grd_payment.Rows[2].Cells[1].Style.BackColor = Color.Silver;
                //grd_payment.Rows[0].Cells[1].Style.BackColor = Color.Silver;
                //grd_payment.Rows[0].Cells[5].Style.BackColor = Color.Silver;
                //grd_payment.Rows[2].Cells[3].Style.BackColor = Color.Silver;
                //grd_payment.Rows[1].Cells[5].Style.BackColor = Color.Silver;
                //grd_payment.Rows[2].Cells[5].Style.BackColor = Color.Silver;
                //grd_payment.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                //grd_payment.Rows[0].Cells[1].Style.BackColor = Color.Silver;

                
                //Grd_ptax.Rows[0].Cells[1].Style.BackColor = Color.Silver;
                //Grd_ptax.Rows[1].Cells[1].Style.BackColor = Color.Silver;
                //Grd_ptax.Rows[2].Cells[1].Style.BackColor = Color.Silver;
                //Grd_ptax.Rows[3].Cells[1].Style.BackColor = Color.Silver;
                //Grd_ptax.Rows[4].Cells[1].Style.BackColor = Color.Silver;
                //Grd_ptax.Rows[0].Cells[2].Style.BackColor = Color.Silver;
                //Grd_ptax.Rows[1].Cells[2].Style.BackColor = Color.Silver;
                //Grd_ptax.Rows[2].Cells[2].Style.BackColor = Color.Silver;
                //Grd_ptax.Rows[3].Cells[2].Style.BackColor = Color.Silver;
                //Grd_ptax.Rows[4].Cells[2].Style.BackColor = Color.Silver;

                //   grd_payment.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            if (tabSoftware.SelectedIndex == 3)
            {
                grid_eligible.Rows[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grid_eligible.Rows[0].DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);
                grid_eligible.Rows[0].DefaultCellStyle.BackColor = Color.DeepSkyBlue;
                grid_eligible.Rows[0].DefaultCellStyle.ForeColor = Color.White;
                grid_eligible.Rows[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grid_eligible.Rows[7].DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);
                grid_eligible.Rows[7].DefaultCellStyle.BackColor = Color.DeepSkyBlue;
                grid_eligible.Rows[7].DefaultCellStyle.ForeColor = Color.White;
                grid_eligible.Rows[13].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grid_eligible.Rows[13].DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);
                grid_eligible.Rows[13].DefaultCellStyle.BackColor = Color.DeepSkyBlue;
                grid_eligible.Rows[13].DefaultCellStyle.ForeColor = Color.White;
                grid_eligible.Rows[10].DefaultCellStyle.BackColor = Color.Silver;
                grid_eligible.Rows[6].DefaultCellStyle.BackColor = Color.Silver;
                
                grid_eligible.Rows[12].DefaultCellStyle.BackColor = Color.Silver;
                grid_eligible.Rows[6].DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);
                grid_eligible.Rows[10].DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);               
                grid_eligible.Rows[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                grid_eligible.Rows[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                grid_eligible.Rows[12].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                grid_eligible.Rows[12].DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);
                grid_eligible.Rows[1].Cells[2].Style.BackColor = Color.Silver;
                grid_eligible.Rows[1].Cells[3].Style.BackColor = Color.Silver;
                grid_eligible.Rows[2].Cells[2].Style.BackColor = Color.Silver;
                grid_eligible.Rows[2].Cells[3].Style.BackColor = Color.Silver;
                grid_eligible.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grid_eligible.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            }
            if(tabSoftware.SelectedIndex == 1)
            {
                grid_detail.Rows[1].Cells[3].Style.BackColor = Color.Silver;
                grid_detail.Rows[1].Cells[4].Style.BackColor = Color.Silver;
                grid_detail.Rows[2].Cells[2].Style.BackColor = Color.Silver;
                grid_detail.Rows[2].Cells[3].Style.BackColor = Color.Silver;
                grid_detail.Rows[2].Cells[4].Style.BackColor = Color.Silver;
                grid_detail.Rows[2].Cells[5].Style.BackColor = Color.Silver;

                grid_detail.Rows[4].Cells[2].Style.BackColor = Color.Silver;
                grid_detail.Rows[4].Cells[3].Style.BackColor = Color.Silver;
                grid_detail.Rows[4].Cells[4].Style.BackColor = Color.Silver;
                grid_detail.Rows[4].Cells[5].Style.BackColor = Color.Silver;
                grid_detail.Rows[2].Cells[2].Style.BackColor = Color.Silver;

                grid_detail.Rows[4].Cells[2].Style.BackColor = Color.Silver;
                grid_detail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grid_detail.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
              


            }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        


        }
            

        }



    


    

       
    

