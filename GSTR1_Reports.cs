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
    public partial class GSTR1_Reports : Form
    {
        public GSTR1_Reports()
        {
            InitializeComponent();
        }
        #region ***** Part-Grd Summary ....
        private void DataBind_Grd_Summary(DataGridView Grd_Summary)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Table No.");
            dt.Columns.Add("Types Of Invoices");
            dt.Columns.Add("Status");
            dt.Columns.Add("Document Count");
            dt.Columns.Add("Taxable Value");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("Cess");
            dt.Columns.Add("Total");
            dt.Columns.Add("Total Invoice ");

            DataRow dr = dt.NewRow();
            dr[0] = "Table 4A,4B,4C,6B,6C";
            dr[1] = "B2B Invoices";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dr[8] = "0";
            dr[9] = "0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            
            dr[0] = "Table 5A,5B";
            dr[1] = "B2C (Large) Invoices";
            dr[2] = "_";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dr[8] = "0";
            dr[9] = "0";
            dt.Rows.Add(dr);
            dr = dt.NewRow();

            dr[0] = "Table 7";
            dr[1] = "B2C (Others)";
            dr[2] = "_";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dr[8] = "0";
            dr[9] = "0";
            dt.Rows.Add(dr);


            dr = dt.NewRow();
            dr[0] = "Table 6A";
            dr[1] = "Export Invoices";
            dr[2] = "_";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dr[8] = "0";
            dr[9] = "0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Table 9B";
            dr[1] = "Credit/Debit Notes (Registered)";
            dr[2] = "_";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dr[8] = "0";
            dr[9] = "0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Table 9B";
            dr[1] = "Credit Note/Debit Notes (Uregistered) ";
            dr[2] = "_";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dr[8] = "0";
            dr[9] = "0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Table 8A,8B,8C,8D";
            dr[1] = "Nil rated supplies";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dr[8] = "0";
            dr[9] = "0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Table 11A(1),11(2)";
            dr[1] = "Advance Received";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dr[8] = "0";
            dr[9] = "0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Table 11B(1),11B(2)";
            dr[1] = "Adjustment of Advances";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dr[8] = "0";
            dr[9] = "0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "Total [B2B+B2C(Large)+B2C(Others)+Export Invoices+Nil Rated + Advances Received-CDN(Registered)-CDN(Unregistered)-Adjustment of Advances]";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dr[8] = "0";
            dr[9] = "0";
            dt.Rows.Add(dr);
         
            grdSummary.DataSource = dt;
            dr = dt.NewRow();
            dr[0] = "Table 12";
            dr[1] = "HSN-wise summary of outward supplies";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dr[8] = "0";
            dr[9] = "0";
            dt.Rows.Add(dr);

            grdSummary.DataSource = dt;

            dr = dt.NewRow();
            dr[0] = "Table 13";
            dr[1] = "Documents Issued";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dr[8] = "0";
            dr[9] = "0";
            dt.Rows.Add(dr);

            grdSummary.DataSource = dt;
            grdSummary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;


        }
        #endregion

        #region ***** Part-Grd AmendementSummary ....
        private void DataBind_Grd_ASummary(DataGridView Grd_Asummary)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Table No");
            dt.Columns.Add("Supply Type");
            dt.Columns.Add("Status");
            dt.Columns.Add("Numnber of Records");
            dt.Columns.Add("Taxable Value");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("Cess");
            dt.Columns.Add("Total Tax");

            DataRow dr = dt.NewRow();

           


            
            dr = dt.NewRow();
            dr[0] = "Table 9A";
            dr[1] = "Amended B2B Invoices";
            dr[2] = "_";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dr[8] = "0";
            dr[9] = "0";
            
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Table 9A";
            dr[1] = "Amended B2C Large Invoices";
            dr[2] = "_";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dr[8] = "0";
            dr[9] = "0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Table 10";
            dr[1] = "Amended B2C Others";
            dr[2] = "_";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dr[8] = "0";
            dr[9] = "0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Table 9A";
            dr[1] = "Amended Export Invoices";
            dr[2] = "_";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dr[8] = "0";
            dr[9] = "0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Table 9C";
            dr[1] = "Amended Credit/Debit Notes (Registered)";
            dr[2] = "_";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dr[8] = "0";
            dr[9] = "0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Table 9C";
            dr[1] = "Amended Credit/Debit Notes (Unregistered)";
            dr[2] = "_";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dr[8] = "0";
            dr[9] = "0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Table 11A";
            dr[1] = "Amended Advances Received";
            dr[2] = "_";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dr[8] = "0";
            dr[9] = "0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Table 11B";
            dr[1] = "Amended Adjustment Of  Advances";
            dr[2] = "_";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            dr[8] = "0";
            dr[9] = "0";
            dt.Rows.Add(dr);

            Grd_Asummary.DataSource = dt;
           // Grd_ASummary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;


        }
        #endregion

        #region *****part-1Grd Taxable------
        private void DataBind_Grd_Taxable(DataGridView grid_taxable)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
            dt.Columns.Add("GSTIN/UIN");
            dt.Columns.Add("Receiver Name");
            dt.Columns.Add("Invoice No");
            dt.Columns.Add("Invoice Date");
            dt.Columns.Add(" Total Invoice Value");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Taxable value");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("CESS");
            dt.Columns.Add("Place Of Supply ");
            dt.Columns.Add("Reverse Charge");
            dt.Columns.Add("Invoice Type");
            dt.Columns.Add("E-Commerce GSTIN");

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

            dt.Rows.Add(dr);
            grid_taxable.DataSource = dt;
            


        }
        #endregion

        #region *****part-1Grd_B2BA ------
        private void DataBind_Grd_B2BA(DataGridView Grd_B2BA)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
            dt.Columns.Add("GSTIN/UIN");
            dt.Columns.Add("Receiver Name");
            dt.Columns.Add("Original Invoice No.");
            dt.Columns.Add("Original Invoice Date ");
            dt.Columns.Add("Revisied Invoice No.");
            dt.Columns.Add("Revisied Invoice Date");
            dt.Columns.Add("Invoice Type");
            dt.Columns.Add("Place of Supply ");
            dt.Columns.Add("Reverse charge");
            dt.Columns.Add("Differntial % Applicable");
            dt.Columns.Add("Total Invoice Value ");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Taxable Value ");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("CESS");
            dt.Columns.Add("E-Commerce GSTIN");

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
            dr[9] = "dropdown";
            dr[10] = "dropdown";
            dr[11] = "dropdown";
            dr[12] = "";
            dr[13] = "";
            dr[14] = "";
            dr[15] = "";
            dr[16] = "";
            dr[17] = "";
            dr[18] = "";
            dr[19] = "";
        

            dt.Rows.Add(dr);
            Grd_B2BA.DataSource = dt;



        }
        #endregion

        #region********** part 2 - Grd_inter*************


        private void DataBind_Grd_inter( DataGridView Grd_inter)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
          
            dt.Columns.Add("Place Of Supply");
            dt.Columns.Add("Invoice No");
            dt.Columns.Add("Invoice Date");
            dt.Columns.Add("Total Invoice Value");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Taxable value");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CESS");
            dt.Columns.Add("E-Commerce GSTIN");

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
          
          

            dt.Rows.Add(dr);
            Grd_inter.DataSource = dt;
        }

        #endregion

        #region********** part 2 - Grd_B2BCLA*************


        private void DataBind_Grd_B2BLA(DataGridView Grd_B2BLA)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
            //dt.Columns.Add("Place Of Supply");
            dt.Columns.Add("Original Invoice No.");
            dt.Columns.Add(" Original Invoice Date");
            dt.Columns.Add("Revised Invoice No.");
            dt.Columns.Add("Revised Invoice Date ");
            dt.Columns.Add("Total Invoice Value");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Differential Percentage ");
            dt.Columns.Add("Taxable value");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CESS");
            dt.Columns.Add("E-Commerce GSTIN");

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
          
           



            dt.Rows.Add(dr);
            Grd_B2BLA.DataSource = dt;
        }

        #endregion

        #region Part  -3 Grd_Intrastate***********

        private void DataBind_Grd_Intrastate(DataGridView Grd_intrastate)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
            dt.Columns.Add("Supply Type");
            dt.Columns.Add("Place Of Supply");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Taxable  Value");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("CESS");
            dt.Columns.Add("E-Commerce GSTIN");

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
           
           

            dt.Rows.Add(dr);
            Grd_intrastate.DataSource = dt;
        }
        #endregion

            #region Part  -3 Grd_AIntrastate***********
        private void DataBind_Grd_AIntrastate(DataGridView Grd_AIntrastate)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
            dt.Columns.Add("Year");
            dt.Columns.Add("Month");
            dt.Columns.Add("Place Of Supply");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Differential % Applicable");
            dt.Columns.Add("Taxable value");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("CESS");
            dt.Columns.Add("GSTIN-E-Commerce Operator ");

            DataRow dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "Dropdown";
            dr[2] = "dropdown";
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
            
          
                

            dt.Rows.Add(dr);
            Grd_AIntrastate.DataSource = dt;
        }
        #endregion

        #region Part-4 Grd_Exports********
        private void DataBind_Grd_Exports (DataGridView Grd_Export)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
            dt.Columns.Add("Payment of GST");
            dt.Columns.Add("Invoice No");
            dt.Columns.Add("Invoice Date");
            dt.Columns.Add("Total Invoice Value");
            dt.Columns.Add("Port Code" );
            dt.Columns.Add("Shipping Bill/Bill of Export No.");
            dt.Columns.Add("Shipping Bill/Bill of Export Date");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Taxable value");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CESS");

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
            


           
          

            dt.Rows.Add(dr);
            Grd_Export.DataSource = dt;
            foreach (DataGridViewColumn col in Grd_Export.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        #endregion

        #region Part-4 Grd_AExports********
        private void DataBind_Grd_AExports(DataGridView Grd_AExports)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
            dt.Columns.Add("Exports Type");
            dt.Columns.Add("Port Code");
            dt.Columns.Add("Original Invoice No");
            dt.Columns.Add("Original Invoice Date");
            dt.Columns.Add("Revised Invoice No ");
            dt.Columns.Add("Revised Invoice Date ");
            dt.Columns.Add("Shipping Bill No.");
            dt.Columns.Add("Shipping Bill Date");         
            dt.Columns.Add("Rate");
            dt.Columns.Add("Differential % Applicable ");
            dt.Columns.Add("Total Invoice Value  ");
            dt.Columns.Add("Taxable value");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CESS");

            DataRow dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "";
            dr[2] = "Dropdown";
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



            dt.Rows.Add(dr);
            Grd_AExports.DataSource = dt;
            foreach (DataGridViewColumn col in Grd_AExports.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        #endregion

        #region Part-5Grd_debit********
        private void DataBind_Grd_debit(DataGridView Grd_debit)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
            dt.Columns.Add("Receiver GSTIN/UIN");
            dt.Columns.Add("Receiver Name");
            dt.Columns.Add("Note Type");
            dt.Columns.Add("Pre GST Regime");
            dt.Columns.Add("Debit/Credit Note No.");
            dt.Columns.Add("Debit/Credit Note Date");
            dt.Columns.Add("Original Invoice No.");
            dt.Columns.Add("Original Invoice Date");
            dt.Columns.Add("Note Value");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Differential % Applicable ");
            dt.Columns.Add("Taxable value");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("CESS");
            dt.Columns.Add("Place Of Supply");


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
            dr[18] = "";
        


            dt.Rows.Add(dr);
            Grd_debit.DataSource = dt;

            foreach (DataGridViewColumn col in Grd_credit.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
          
        }
        #endregion

        #region Part-6Grd_Credit********
        private void DataBind_Grd_credit(DataGridView Grd_credit)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");       
            dt.Columns.Add("Note Type");
            dt.Columns.Add("Pre GST Regime");
            dt.Columns.Add("Debit/Credit Note No.");
            dt.Columns.Add("Debit/Credit Note Date");
            dt.Columns.Add("Original Invoice No.");
            dt.Columns.Add("Original Invoice Date");
            dt.Columns.Add("Note Value");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Differential % Applicable ");
            dt.Columns.Add("Taxable value");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("CESS");
            dt.Columns.Add("Place Of Supply");
            dt.Columns.Add("Supply Type");

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



            dt.Rows.Add(dr);
            Grd_credit.DataSource = dt;
            foreach (DataGridViewColumn col in Grd_credit.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        #endregion

        #region Part-6Grd_CDNRA********
        private void DataBind_Grd_CDNRA(DataGridView Grd_CDNRA)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S.No.");
            dt.Columns.Add("Receiver GSTIN/UIN");
            dt.Columns.Add("Receiver Name ");
            dt.Columns.Add("Note type");
            dt.Columns.Add("Pre GST Regime");
            dt.Columns.Add("Original Credit/Debit Note No.");
            dt.Columns.Add("Original Credit/Debit Note Date");
            dt.Columns.Add("Revised Credit/Debit Note  No.");
            dt.Columns.Add("Revised Credit/Debit Note  Date");
            dt.Columns.Add("Original Invoice No.");
            dt.Columns.Add("Original Invoice Data");
            dt.Columns.Add("Differntial Percentage ");
            dt.Columns.Add("Note value ");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Differential % Applicable");
            dt.Columns.Add("Taxable Value");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("CESS");


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
            dr[18] = "";
            dr[19] = "";
            dr[20] = "";
           
          



            dt.Rows.Add(dr);
            Grd_CDNRA.DataSource = dt;
            foreach (DataGridViewColumn col in Grd_CDNRA.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        #endregion

        #region Part-7Grd_Nill********
        private void DataBind_Grd_Nill(DataGridView Grd_Nill)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("S.No");
            dt.Columns.Add("Description");
            dt.Columns.Add("Nil Rated Supplies");
            dt.Columns.Add("Exempted (Other than Nil rated /non-GST supply)");
            dt.Columns.Add("Non-GST Supplies");
            

            DataRow dr = dt.NewRow();
            dr[0] = "1";
            dr[1] = "Intra-state supplies to registered persons";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";
            



            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "2";
            dr[1] = "Intra-state supplies to unregistered persons";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";




            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "3";
            dr[1] = "Inter-state supplies to registered persons";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";




            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "4";
            dr[1] = "Inter-state supplies to unregistered persons";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";




            dt.Rows.Add(dr);
            Grd_Nill.DataSource = dt;
            //Grd_Nill.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            Grd_Nill.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Grd_Nill.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Grd_Nill.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Grd_Nill.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            foreach (DataGridViewColumn col in Grd_Nill.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

        }
        #endregion

        #region Part-8Grd_Advanced********
        private void DataBind_Grd_Advanced(DataGridView Grd_advanced)
        {
            DataTable dt = new DataTable();
         
            dt.Columns.Add("S.No.");
            dt.Columns.Add("Placce Of Supply");
            dt.Columns.Add("Supply Type ");
            dt.Columns.Add(" Advance Received");
            dt.Columns.Add("GST % ");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("CESS");
            dt.Columns.Add("Total GST");
            

            DataRow dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "State List";
            dr[2] = "Intra State ";
            dr[2] = "Inter State ";
            dr[3] = "";
            dr[4] = "";
            dr[5] = "";
            dr[6] = "";
            dr[7] = "";
            dr[8] = "";
            dr[9] = "";
           
      



            dt.Rows.Add(dr);
            Grd_advanced.DataSource = dt;
            foreach (DataGridViewColumn col in Grd_advanced.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        #endregion

        #region Part-9Grd_Adjusted********

        private void DataBind_Grd_Adjusted (DataGridView Grd_Adjusted)
        {

             DataTable dt = new DataTable();

            dt.Columns.Add("S.No.");
            dt.Columns.Add("Placce Of Supply");
            dt.Columns.Add("Supply Type ");
            dt.Columns.Add("Advance Adjusted");
            dt.Columns.Add("GST % ");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("CESS");
            dt.Columns.Add("Total GST");


            DataRow dr = dt.NewRow();
            dr[0] = "";
            dr[1] = " dropdown";
            dr[2] = "Intra State ";
            dr[2] = "Inter State ";
            dr[3] = "";
            dr[4] = "";
            dr[5] = "";
            dr[6] = "";
            dr[7] = "";
            dr[8] = "";
            dr[9] = "";





            dt.Rows.Add(dr);
            Grd_Adjusted.DataSource = dt;
            foreach (DataGridViewColumn col in Grd_Adjusted.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        #endregion

      
     

        #region Part-10Grd_Document********
        private void DataBind_Grd_Document(DataGridView Grd_Document)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
            dt.Columns.Add("Document Type");
            dt.Columns.Add("S No.Form");
            dt.Columns.Add("S No.To");
            dt.Columns.Add("Total No.");
            dt.Columns.Add("Cancelled");
            dt.Columns.Add("Net Issued");
           


            DataRow dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "";
            dr[2] = "";
            dr[3] = "";
            dr[4] = "";
            dr[5] = "";
            dr[6] = "";
            dr[7] = "";
            dt.Rows.Add(dr);
            Grd_Document.DataSource = dt;
            Grd_Document.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            foreach (DataGridViewColumn col in Grd_Document.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        #endregion

        #region Part-11Grd_HSN********
        private void DataBind_Grd_Wise(DataGridView Grd_Wise)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
            dt.Columns.Add("HSN");
            dt.Columns.Add("Description");
            dt.Columns.Add("UQC");
            dt.Columns.Add("Total Quantity");
            dt.Columns.Add("Total Value ");
            dt.Columns.Add("Total Taxable Value ");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("CESS");
          



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
            
          
            dt.Rows.Add(dr);
            Grd_Wise.DataSource = dt;
            Grd_Wise.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            foreach (DataGridViewColumn col in Grd_Wise.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        #endregion

        #region Part-12Grd_CDNURA********
        private void DataBind_Grd_CDNURA(DataGridView Grd_CDNURA)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
            dt.Columns.Add("Supply Type ");
            dt.Columns.Add("Note Type");
            dt.Columns.Add("Pre GST Regime");
           // dt.Columns.Add("Place Of Supply");        
            dt.Columns.Add("Original Debit/Credit Note No.");
            dt.Columns.Add("Original Debit/Credit Note Date");
            dt.Columns.Add("Revised Debit/Credit Note No.");
            dt.Columns.Add("Revised Debit/Credit Note Date");
            dt.Columns.Add("Original Invoice No");
            dt.Columns.Add("Original Invoice Data");
            dt.Columns.Add("Note value ");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Differntial % Applicable ");
            dt.Columns.Add("Taxable Value");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("CESS");




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
            dr[18] = "";
      
           
          

            dt.Rows.Add(dr);
            Grd_CDNURA.DataSource = dt;
            Grd_CDNURA.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            foreach (DataGridViewColumn col in Grd_CDNURA.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        #endregion
        #region Part-13Grd_ATA********
        private void DataBind_Grd_ATA(DataGridView Grd_ATA)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
            dt.Columns.Add("Year");
            dt.Columns.Add("Month");
            dt.Columns.Add("Original Place Of Supply");      
            dt.Columns.Add("Advanced Received");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Differential % Applicable");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("CESS");
          



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
            

            dt.Rows.Add(dr);
            Grd_ATA.DataSource = dt;
            Grd_ATA.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            foreach (DataGridViewColumn col in Grd_ATA.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        #endregion

          private void DataBind_Grd_Adjustment(DataGridView Grd_Adjustment)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
            dt.Columns.Add("Year");
            dt.Columns.Add("Month");
            dt.Columns.Add("Original Place Of Supply");          
            dt.Columns.Add("Advanced to be  Adjusted ");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Differential % Applicable");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("CESS");
          



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
            

            dt.Rows.Add(dr);
            Grd_Adjustment.DataSource = dt;
            Grd_Adjustment.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            foreach (DataGridViewColumn col in Grd_Adjustment.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
       
        


        private void GSTR1_Reports_Load(object sender, EventArgs e)
        {
            GetSetSoftwareData();
        }  
             private void GetSetSoftwareData()
             {
             try
            {


                grdSummary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;  // OK
                DataBind_Grd_Summary(grdSummary);

                Grd_Asummary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                DataBind_Grd_ASummary(Grd_Asummary);

                grid_taxable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                DataBind_Grd_Taxable(grid_taxable);

                //Grd_B2BA.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                DataBind_Grd_B2BA(Grd_B2BA);


                Grd_inter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                DataBind_Grd_inter(Grd_inter);
                DataBind_Grd_B2BLA(Grd_B2BLA);

                Grd_AIntrastate.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                DataBind_Grd_AIntrastate(Grd_AIntrastate);

                Grd_intrastate.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                DataBind_Grd_Intrastate(Grd_intrastate);

                Grd_Export.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                DataBind_Grd_Exports(Grd_Export);

                DataBind_Grd_AExports(Grd_AExports);

                DataBind_Grd_debit(Grd_debit);
                DataBind_Grd_credit(Grd_credit);

                DataBind_Grd_CDNRA(Grd_CDNRA);
                DataBind_Grd_CDNURA(Grd_CDNURA);

                DataBind_Grd_Nill(Grd_Nill);

                DataBind_Grd_Wise(Grd_Wise);

                DataBind_Grd_Document(Grd_Document);

                DataBind_Grd_Advanced(Grd_advanced);
                DataBind_Grd_Adjusted(Grd_Adjusted);
                DataBind_Grd_ATA(Grd_ATA);

                DataBind_Grd_Adjustment(Grd_Adjustment);


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

             private void btnClose_Click(object sender, EventArgs e)
             {
                 this.Close();
             }

           

             private void btnClose_Click_1(object sender, EventArgs e)
             {
                 this.Close();
             }

             private void gb10_part5_Enter(object sender, EventArgs e)
             {

             }

             private void tabSoftware_SelectedIndexChanged(object sender, EventArgs e)
             {
                 if (tabSoftware.SelectedIndex == 0)
                 {
                     grdSummary.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                 }
             }

             private void tabSoftware_DrawItem_1(object sender, DrawItemEventArgs e)
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

             private void tabGSTPortal_DrawItem(object sender, DrawItemEventArgs e)
             {

             }

             private void btnClose2_Click(object sender, EventArgs e)
             {
                 this.Close();
             }

             private void tabportal_DrawItem(object sender, DrawItemEventArgs e)
             {
                 TabControl tabcntrl = sender as TabControl;
                 Graphics g = e.Graphics;
                 Brush _textBrush;
                
                 // Get the item from the collection.
                 TabPage _tabPage = tabcntrl.TabPages[e.Index];

                 // Get the real bounds for the tab rectangle.
                 Rectangle _tabBounds = tabcntrl.GetTabRect(e.Index);

                 if (e.State == DrawItemState.Selected)
                 {
                     // Draw a different background color, and don't paint a focus rectangle.


                     //_textBrush = new SolidBrush(Color.Black);
                     if (tabcntrl.Name == "tabportal") { _textBrush = new SolidBrush(Color.WhiteSmoke); g.FillRectangle(Brushes.Navy, e.Bounds); }
                     else { _textBrush = new SolidBrush(Color.Black); g.FillRectangle(new SolidBrush(Color.FromArgb(23, 196, 187)), e.Bounds); }
                     // Color.FromArgb(23,196,87)
                 }
                 else
                 {
                     //_textBrush = new SolidBrush(Color.WhiteSmoke); //e.ForeColor
                     if (tabcntrl.Name == "tabportal") { _textBrush = new SolidBrush(Color.Black); g.FillRectangle(new SolidBrush(Color.FromArgb(23, 196, 187)), e.Bounds); }
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

             private void Grd_Adjustment_CellContentClick(object sender, DataGridViewCellEventArgs e)
             {

             }

             //private void tabSoftware_SelectedIndexChanged(object sender, EventArgs e)
             //{
             //    //if (tabSoftware.SelectedIndex == 0)
             //    //{
             //    //    grdSummary.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
             //    //}
             //}
        }
        }
