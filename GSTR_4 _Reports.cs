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
    public partial class GSTR_4A__Reports : Form
    {
        public GSTR_4A__Reports()
        {
            InitializeComponent();
        }
       
        private void Bind_GridSummary(DataGridView grdSummary)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("Table No.");
            dt.Columns.Add("No of Suppliers");
            dt.Columns.Add("Status");
            dt.Columns.Add("No of Notes/Vouchers");
            dt.Columns.Add("Discount Count");
            dt.Columns.Add("Invoice Value");
            dt.Columns.Add("Taxable Value");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("CESS");

            DataRow dr = dt.NewRow();
            dr[0] = "Table 4A,4B";
            dr[1] = "Inward Supplies (Registered)";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dr[8] = "0";
            dr[9] = "0";
            dr[10] = "0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Table 4C";
            dr[1] = "Inward Supplies (Unregistered)";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dr[8] = "0";
            dr[9] = "0";
            dr[10] = "0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Table 4D";
            dr[1] = "Import of Service";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dr[8] = "0";
            dr[9] = "0";
            dr[10] = "0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Table 5B";
            dr[1] = "Debit/Credit Notes (Registered)";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dr[8] = "0";
            dr[9] = "0";
            dr[10] = "0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Table 5B";
            dr[1] = "Debit/Credit Notes (Unregistered)";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dr[8] = "0";
            dr[9] = "0";
            dr[10] = "0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Table 6";
            dr[1] = "Tax on Outward Supplies";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dr[8] = "0";
            dr[9] = "0";
            dr[10] = "0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Table 8A";
            dr[1] = "Advance amount paid ";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dr[8] = "0";
            dr[9] = "0";
            dr[10] = "0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Table 8B";
            dr[1] = "Adjustment of Advances paid";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dr[8] = "0";
            dr[9] = "0";
            dr[10] = "0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "Total";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dr[8] = "0";
            dr[9] = "0";
            dr[10] = "0";
            dt.Rows.Add(dr);

            grdSummary.DataSource = dt;



           
            //grdSummary.Rows.Add(9);

            //grdSummary.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ////grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;


            //grdSummary[0, 0].Value = "1";
            //grdSummary[1, 0].Value = "4B(B2B)";
            //grdSummary[2, 0].Value = "0";
            //grdSummary[3, 0].Value = "0";
            //grdSummary[4, 0].Value = "0";
            //grdSummary[5, 0].Value = "0";
            //grdSummary[6, 0].Value = "0";
            //grdSummary[7, 0].Value = "0";
            //grdSummary[8, 0].Value = "0";
            //grdSummary[9, 0].Value = "0";
            //grdSummary[10, 0].Value = "0";



            //grdSummary[0, 1].Value = "2 ";
            //grdSummary[1, 1].Value = "4C(B2BUR)";
            //grdSummary[2, 1].Value = "0";
            //grdSummary[3, 1].Value = "0";
            //grdSummary[4, 1].Value = "0";
            //grdSummary[5, 1].Value = "0";
            //grdSummary[6, 1].Value = "0 ";
            //grdSummary[7, 1].Value = "0";
            //grdSummary[8, 1].Value = "0";
            //grdSummary[9, 1].Value = "0";
            //grdSummary[10, 1].Value = "0";



            //grdSummary[0, 2].Value = "3";
            //grdSummary[1, 2].Value = "4D(IMPS) ";
            //grdSummary[2, 2].Value = "0";
            //grdSummary[3, 2].Value = "0";
            //grdSummary[4, 2].Value = "0";
            //grdSummary[5, 2].Value = "0";
            //grdSummary[6, 2].Value = "0 ";
            //grdSummary[7, 2].Value = "0";
            //grdSummary[8, 2].Value = "0";
            //grdSummary[9, 2].Value = "0";
            //grdSummary[10, 2].Value = "0";

            //grdSummary[0, 3].Value = "4 ";
            //grdSummary[1, 3].Value = "5B(CDNR)";
            //grdSummary[2, 3].Value = "0";
            //grdSummary[3, 3].Value = "0";
            //grdSummary[4, 3].Value = "0";
            //grdSummary[5, 3].Value = "0";
            //grdSummary[6, 3].Value = "0 ";
            //grdSummary[7, 3].Value = "0";
            //grdSummary[8, 3].Value = "0";
            //grdSummary[9, 3].Value = "0";
            //grdSummary[10, 3].Value = "0";

            //grdSummary[0, 4].Value = "5 ";
            //grdSummary[1, 4].Value = "5B(CDNUR)";
            //grdSummary[2, 4].Value = "0";
            //grdSummary[3, 4].Value = "0";
            //grdSummary[4, 4].Value = "0";
            //grdSummary[5, 4].Value = "0";
            //grdSummary[6, 4].Value = "0 ";
            //grdSummary[7, 4].Value = "0";
            //grdSummary[8, 4].Value = "0";
            //grdSummary[9, 4].Value = "0";
            //grdSummary[10, 4].Value = "0";

            //grdSummary[0, 5].Value = "6 ";
            //grdSummary[1, 5].Value = "TXOS";
            //grdSummary[2, 5].Value = "0";
            //grdSummary[3, 5].Value = "0";
            //grdSummary[4, 5].Value = "0";
            //grdSummary[5, 5].Value = "0";
            //grdSummary[6, 5].Value = "0 ";
            //grdSummary[7, 5].Value = "0";
            //grdSummary[8, 5].Value = "0";
            //grdSummary[9, 5].Value = "0";
            //grdSummary[10, 5].Value = "0";

            //grdSummary[0, 6].Value = "7 ";
            //grdSummary[1, 6].Value = "8A(AT)";
            //grdSummary[2, 6].Value = "0";
            //grdSummary[3, 6].Value = "0";
            //grdSummary[4, 6].Value = "0";
            //grdSummary[5, 6].Value = "0";
            //grdSummary[6, 6].Value = "0 ";
            //grdSummary[7, 6].Value = "0";
            //grdSummary[8, 6].Value = "0";
            //grdSummary[9, 6].Value = "0";
            //grdSummary[10, 6].Value = "0";

            //grdSummary[0, 7].Value = "8 ";
            //grdSummary[1, 7].Value = "8T(ATADJ)";
            //grdSummary[2, 7].Value = "0";
            //grdSummary[3, 7].Value = "0";
            //grdSummary[4, 7].Value = "0";
            //grdSummary[5, 7].Value = "0";
            //grdSummary[6, 7].Value = "0 ";
            //grdSummary[7, 7].Value = "0";
            //grdSummary[8, 7].Value = "0";
            //grdSummary[9, 7].Value = "0";
            //grdSummary[10, 7].Value = "0";

            //grdSummary[0, 8].Value = "9 ";
            //grdSummary[1, 8].Value = "TOTAL";
            //grdSummary[2, 8].Value = "0";
            //grdSummary[3, 8].Value = "0";
            //grdSummary[4, 8].Value = "0";
            //grdSummary[5, 8].Value = "0";
            //grdSummary[6, 8].Value = "0 ";
            //grdSummary[7, 8].Value = "0";
            //grdSummary[8, 8].Value = "0";
            //grdSummary[9, 8].Value = "0";
            //grdSummary[10, 8].Value = "0";


            foreach (DataGridViewColumn col in grdSummary.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        private void bind_grid_inward(DataGridView grid_inward)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
            dt.Columns.Add("Supplier GSTIN");
            dt.Columns.Add("Invoice Number");
            dt.Columns.Add("Invoice Date");
            dt.Columns.Add("Total Invoice Value");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Taxable Value");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("CESS");
            dt.Columns.Add("Invoice Type");
            dt.Columns.Add("Place Of supply");
            dt.Columns.Add("Reverse Charge");

            grid_inward.DataSource = dt;
            //grid_inward.Rows.Add(1);

            ////grdSummary.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ////grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;


            //grid_inward[0, 0].Value = "";
            //grid_inward[1, 0].Value = "";
            //grid_inward[2, 0].Value = "";
            //grid_inward[3, 0].Value = "";
            //grid_inward[4, 0].Value = "";
            //grid_inward[5, 0].Value = "";
            //grid_inward[6, 0].Value = "";
            //grid_inward[7, 0].Value = "";
            //grid_inward[8, 0].Value = "";
            //grid_inward[9, 0].Value = "";
            //grid_inward[10, 0].Value = "";
            //grid_inward[11, 0].Value = "";
            //grid_inward[12, 0].Value = "";
            //grid_inward[13, 0].Value = "";
            //grid_inward[14, 0].Value = "";
           

            foreach (DataGridViewColumn col in grid_inward.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void Bind_Grid_debit(DataGridView grid_debit)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
            dt.Columns.Add("Supplier GSTIN/UID");
            dt.Columns.Add("Supplier Name");
            dt.Columns.Add("Note Type");
            dt.Columns.Add("Debit/Credit Note No.");
            dt.Columns.Add("Debit/Credit Note Date");
           
            dt.Columns.Add("Original Invoice No.");
            dt.Columns.Add("Original Invoice Date");
            dt.Columns.Add("Note Value");
            dt.Columns.Add("Rate");
            
            dt.Columns.Add("Taxable Value");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("CESS");
            dt.Columns.Add("Reverse Charge");
            dt.Columns.Add("Place Of supply");
            dt.Columns.Add("Supply Type");

            grid_debit.DataSource = dt;

           
            //grid_debit.Rows.Add(1);

            ////grdSummary.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ////grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;


            //grid_debit[0, 0].Value = "";
            //grid_debit[1, 0].Value = "";
            //grid_debit[2, 0].Value = "";
            //grid_debit[3, 0].Value = "";
            //grid_debit[4, 0].Value = "";
            //grid_debit[5, 0].Value = "";
            //grid_debit[6, 0].Value = "";
            //grid_debit[7, 0].Value = "";
            //grid_debit[8, 0].Value = "";
            //grid_debit[9, 0].Value = "";
            //grid_debit[10, 0].Value = "";
            //grid_debit[11, 0].Value = "";
            //grid_debit[12, 0].Value = "";
            //grid_debit[13, 0].Value = "";


            foreach (DataGridViewColumn col in grid_debit.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        private void bind_grid_import(DataGridView grid_import)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
            dt.Columns.Add("Invoice Number");
            dt.Columns.Add("Invoice Date");
            dt.Columns.Add("Invoice Value");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Taxable Value");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CESS");
            dt.Columns.Add("Place Of supply");
            grid_import.DataSource = dt;
           
            //grid_import.Rows.Add(1);

            ////grdSummary.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ////grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;


            //grid_import[0, 0].Value = "";
            //grid_import[1, 0].Value = "";
            //grid_import[2, 0].Value = "";
            //grid_import[3, 0].Value = "";
            //grid_import[4, 0].Value = "";
            //grid_import[5, 0].Value = "";
            //grid_import[6, 0].Value = "";
            //grid_import[7, 0].Value = "";
            //grid_import[8, 0].Value = "";
            //grid_import[9, 0].Value = "";
           

            foreach (DataGridViewColumn col in grid_import.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        private void bind_grid_supplies(DataGridView grid_supplies)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
            dt.Columns.Add("Invoice Number");
            dt.Columns.Add("Invoice Date");
            dt.Columns.Add("Total Invoice Value");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Taxable Value");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("CESS");
            dt.Columns.Add("Place Of Supply");
            dt.Columns.Add("Supply Type");

            grid_supplies.DataSource = dt;

            //grid_supplies.Rows.Add(1);

            ////grdSummary.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ////grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;


            //grid_supplies[0, 0].Value = "";
            //grid_supplies[1, 0].Value = "";
            //grid_supplies[2, 0].Value = "";
            //grid_supplies[3, 0].Value = "";
            //grid_supplies[4, 0].Value = "";
            //grid_supplies[5, 0].Value = "";
            //grid_supplies[6, 0].Value = "";
            //grid_supplies[7, 0].Value = "";
            //grid_supplies[8, 0].Value = "";
            //grid_supplies[9, 0].Value = "";
            //grid_supplies[10, 0].Value = "";
            //grid_supplies[11, 0].Value = "";
            //grid_supplies[12, 0].Value = "";
         

            foreach (DataGridViewColumn col in grid_inward.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        private void bind_grd_credit(DataGridView grd_credit)
        {

            DataTable dt = new DataTable();
            
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");          
            dt.Columns.Add("Note Type");
            dt.Columns.Add("Debit/Credit Note No.");
            dt.Columns.Add("Debit/Credit Note Date");
            dt.Columns.Add("Original Invoice No.");
            dt.Columns.Add("Original Invoice Date");
            dt.Columns.Add("Note Value");
            dt.Columns.Add("Rate");                    
            dt.Columns.Add("Taxable Value");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("CESS");
           
            dt.Columns.Add("Supply Type");
            grd_credit.DataSource = dt;
            //grd_credit.Rows.Add(1);

            ////grdSummary.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ////grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;


            //grd_credit[0, 0].Value = "";
            //grd_credit[1, 0].Value = "";
            //grd_credit[2, 0].Value = "";
            //grd_credit[3, 0].Value = "";
            //grd_credit[4, 0].Value = "";
            //grd_credit[5, 0].Value = "";
            //grd_credit[6, 0].Value = "";
            //grd_credit[7, 0].Value = "";
            //grd_credit[8, 0].Value = "";
            //grd_credit[9, 0].Value = "";
            //grd_credit[10, 0].Value = "";
            //grd_credit[11, 0].Value = "";
            //grd_credit[12, 0].Value = "";
            //grd_credit[13, 0].Value = "";
            //grd_credit[14, 0].Value = "";
           
           
           


            foreach (DataGridViewColumn col in grid_inward.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        private void bind_grd_advance(DataGridView grid_advance)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Gross Advance Paid ");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("CESS");
            dt.Columns.Add("Place of Supply");
            dt.Columns.Add("Supply Type");
            grid_advance.DataSource = dt;
            //grid_advance.Rows.Add(1);

            ////grdSummary.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ////grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;


            //grid_advance[0, 0].Value = "";
            //grid_advance[1, 0].Value = "";
            //grid_advance[2, 0].Value = "";
            //grid_advance[3, 0].Value = "";
            //grid_advance[4, 0].Value = "";
            //grid_advance[5, 0].Value = "";
            //grid_advance[6, 0].Value = "";
            //grid_advance[7, 0].Value = "";
            //grid_advance[8, 0].Value = "";
            //grid_advance[9, 0].Value = "";
            




            foreach (DataGridViewColumn col in grid_inward.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        private void bind_grid_amount(DataGridView grid_amount)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Gross Advance Paid ");          
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("CESS");
            dt.Columns.Add("Place of Supply");
            dt.Columns.Add("Supply Type");
            grid_amount.DataSource = dt;
            //grid_amount.Rows.Add(1);

            ////grdSummary.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ////grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;


            //grid_amount[0, 0].Value = "";
            //grid_amount[1, 0].Value = "";
            //grid_amount[2, 0].Value = "";
            //grid_amount[3, 0].Value = "";
            //grid_amount[4, 0].Value = "";
            //grid_amount[5, 0].Value = "";
            //grid_amount[6, 0].Value = "";
            //grid_amount[7, 0].Value = "";
            //grid_amount[8, 0].Value = "";
            //grid_amount[9, 0].Value = "";





            foreach (DataGridViewColumn col in grid_inward.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        private void bind_grd_tax(DataGridView grid_tax)
        {


            DataTable dt = new DataTable();

            dt.Columns.Add("S No.");
            dt.Columns.Add("Rate Of Tax");
            dt.Columns.Add("Turn Over");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");

            dt.Columns.Add("Supply Type");
            grid_tax.DataSource = dt;
            //grid_tax.Rows.Add(3);

            ////grdSummary.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ////grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;


            //grid_tax[0, 0].Value = "1";
            //grid_tax[1, 0].Value = "1";
            //grid_tax[2, 0].Value = "";
            //grid_tax[3, 0].Value = "";
            //grid_tax[4, 0].Value = "";

            //grid_tax[0, 1].Value = "2";
            //grid_tax[1, 1].Value = "2";
            //grid_tax[2, 1].Value = "";
            //grid_tax[3, 1].Value = "";
            //grid_tax[4, 1].Value = "";

            //grid_tax[0, 2].Value = "3";
            //grid_tax[1, 2].Value = "5";
            //grid_tax[2, 2].Value = "";
            //grid_tax[3, 2].Value = "";
            //grid_tax[4, 2].Value = "";



            foreach (DataGridViewColumn col in grid_tax.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
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

                grid_inward.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                bind_grid_inward(grid_inward);
                //grd_check.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                // grd_check.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                // grd_check.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                //grd_check.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                //amendGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                //Bind_Grid_Invoice(amendGrid);
                // grid_year.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                //  grid_year.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                //  grid_year.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                //  grid_year.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                //grid_supplies.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                bind_grid_supplies(grid_supplies);

                bind_grid_import(grid_import);

                Bind_Grid_debit(grid_debit);

                bind_grd_credit(grd_credit);

                //grid_tax.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                bind_grd_tax(grid_tax);

                bind_grd_advance(grid_amount);

                bind_grid_amount(grid_advance);

               




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

        private void GSTR_4A__Reports_Load(object sender, EventArgs e)
        {
        GetSetSoftwareData();

        //label9.Text = "8B.Advance Amount On Which  Tax was  Paid in earlier Period" + Environment.NewLine + " But Invoice  has been received in the current Period ";
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grid_import_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabPage7_Click(object sender, EventArgs e)
        {

        }

        private void customDataGridViews1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabPage10_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void grdSummary_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex==0)
            {
                tabSoftware.SelectTab(1);

            }
            if (e.RowIndex == 1)
            {
                tabSoftware.SelectTab(2);

            }
            if (e.RowIndex == 2)
            {
                tabSoftware.SelectTab(3);

            }

        }
        }

    }

