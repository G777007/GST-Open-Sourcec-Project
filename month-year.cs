using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SPEQTAGST_DESIGN
{
    public partial class month_year : Form
    {
        public month_year()
        {
            InitializeComponent();
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
        private void month_year_Load(object sender, EventArgs e)
        {
            DataBind_Grd_year(Grd_year);
            DataBind_Grd_Asummary(Grd_Asummary);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Grd_year_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

    }
}
