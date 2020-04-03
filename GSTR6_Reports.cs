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
    public partial class GSTR6_Reports : Form
    {
        public GSTR6_Reports()
        {
            InitializeComponent();
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void Bind_GridSummary(DataGridView grdSummary)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Table No");
            dt.Columns.Add("Types");
            dt.Columns.Add("Status");
            dt.Columns.Add("Total taxable Value");
            dt.Columns.Add("Total Intergrated Tax");
            dt.Columns.Add("Total Central Tax");
            dt.Columns.Add("Total State/UT Tax");
            dt.Columns.Add("Total CESS");

            DataRow dr = dt.NewRow();
            dr[0] = "Table 3";
            dr[1] = "Input Tax Credit received for distribution";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Table 6B ";
            dr[1] = "Credit/Debit Notes received";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
           
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Table 6A.";
            dr[1] = "Amendment of Input Tax Credit received";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
            
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Table 6C";
            dr[1] = "Amendment of Debit/Credit Notes received ";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
         
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Table 5,8";
            dr[1] = "Distribution of input Tax Credit";
            dr[2] = "0";
            dr[3] = "0";
            dr[4] = "0";
            dr[5] = "0";
            dr[6] = "0";
            dr[7] = "0";
          
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Table 9";
            dr[1] = "Amendement of Distribution of ITC";
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
            //grdSummary[1, 0].Value = "3.B2B";
            //grdSummary[2, 0].Value = "0";
            //grdSummary[3, 0].Value = "0";
            //grdSummary[4, 0].Value = "0";
            //grdSummary[5, 0].Value = "0";
            //grdSummary[6, 0].Value = "0";
            //grdSummary[7, 0].Value = "0";
           



            //grdSummary[0, 1].Value = "2 ";
            //grdSummary[1, 1].Value = "6B CDN";
            //grdSummary[2, 1].Value = "0";

            //grdSummary[3, 1].Value = "0";
            //grdSummary[4, 1].Value = "0";
            //grdSummary[5, 1].Value = "0";
            //grdSummary[6, 1].Value = "0 ";
            //grdSummary[7, 1].Value = "0";
            



            //grdSummary[0, 2].Value = "3";
            //grdSummary[1, 2].Value = "6A  .ITC received (B2BA) ";
            //grdSummary[2, 2].Value = "0";

            //grdSummary[3, 2].Value = "0";
            //grdSummary[4, 2].Value = "0";
            //grdSummary[5, 2].Value = "0";
            //grdSummary[6, 2].Value = "0 ";
            //grdSummary[7, 2].Value = "0";
           

            //grdSummary[0, 3].Value = "4 ";
            //grdSummary[1, 3].Value = "6C,CDNA";
            //grdSummary[2, 3].Value = "0";

            //grdSummary[3, 3].Value = "0";
            //grdSummary[4, 3].Value = "0";
            //grdSummary[5, 3].Value = "0";
            //grdSummary[6, 3].Value = "0 ";
            //grdSummary[7, 3].Value = "0";
            

            //grdSummary[0, 4].Value = "5 ";
            //grdSummary[1, 4].Value = "5,8 Distribution of ITC";
            //grdSummary[2, 4].Value = "0";

            //grdSummary[3, 4].Value = "0";
            //grdSummary[4, 4].Value = "0";
            //grdSummary[5, 4].Value = "0";
            //grdSummary[6, 4].Value = "0 ";
            //grdSummary[7, 4].Value = "0";

            //grdSummary[0, 5].Value = "6 ";
            //grdSummary[1, 5].Value = "9. Amend of Distribution of ITC";
            //grdSummary[2, 5].Value = "0";

            //grdSummary[3, 5].Value = "0";
            //grdSummary[4, 5].Value = "0";
            //grdSummary[5, 5].Value = "0";
            //grdSummary[6, 5].Value = "0 ";
            //grdSummary[7, 5].Value = "0";
           


            //foreach (DataGridViewColumn col in grdSummary.Columns)
            //{
            //    col.SortMode = DataGridViewColumnSortMode.NotSortable;
            //}
        }
        private void Bind_GridTax(DataGridView grid_tax)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
            dt.Columns.Add("Supplier Name");
            dt.Columns.Add("Supply Type");
            dt.Columns.Add("Invoice Date");
            dt.Columns.Add("Invoice Number");
            dt.Columns.Add("Total Invoice Value");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Taxable Value");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("Cess");
            dt.Columns.Add("Place Of Supply");
            dt.Columns.Add("Total");

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
           
            dt.Rows.Add(dr);
            grid_tax.DataSource = dt;
            foreach (DataGridViewColumn col in grid_tax.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            //grid_tax.Rows.Add(1);dt.c

            //grdSummary.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ////grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;


            //grid_tax[0, 0].Value = "";
            //grid_tax[1, 0].Value = "";
            //grid_tax[2, 0].Value = "";
            //grid_tax[3, 0].Value = "";
            //grid_tax[4, 0].Value = "";
            //grid_tax[5, 0].Value = "";
            //grid_tax[6, 0].Value = "";
            //grid_tax[7, 0].Value = "";
            //grid_tax[8, 0].Value = "0";

        }
        private void gridview_Datadebit(DataGridView grid_debitreceived)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
            dt.Columns.Add("Supplier Name");
            dt.Columns.Add("Note type");
            dt.Columns.Add("Credit/Debit Note Date");
            dt.Columns.Add("Credit/Debit Note No.");
            dt.Columns.Add("Original Invoice No");
            dt.Columns.Add("Original Invoice Date");
            dt.Columns.Add("Pre GST Regime");
            dt.Columns.Add("Note Value");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Taxable Value");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("CESS");
            dt.Columns.Add("Total");
            
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

            dt.Rows.Add(dr);

            grid_debitreceived.DataSource = dt;

            //gridview_debit.Rows.Add(1);

            //grdSummary.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ////grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;


            //gridview_debit[0, 0].Value = "";
            //gridview_debit[1, 0].Value = "";
            //gridview_debit[2, 0].Value = "";
            //gridview_debit[3, 0].Value = "";
            //gridview_debit[4, 0].Value = "";
            //gridview_debit[5, 0].Value = "";
            //gridview_debit[6, 0].Value = "";
            //gridview_debit[7, 0].Value = "";
            //gridview_debit[8, 0].Value = "0";







        }

        private void gridview_Datainformation(DataGridView grid_information)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
            dt.Columns.Add("Year");
            dt.Columns.Add("Supplier GSTIN");
            dt.Columns.Add("Original Invoice Number");
            dt.Columns.Add("Original Invoice Date");
            dt.Columns.Add("Revised Supplier GSTIN");
            dt.Columns.Add("Revised Supplier Number");
            dt.Columns.Add("Revisied Invoice Date");
            dt.Columns.Add("Total Invoice Value");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Taxable Value");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("CESS");
            dt.Columns.Add("SEZ Supplier");
            dt.Columns.Add("Place of Supply");

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
            grid_information.DataSource = dt;


            //grid_information.Rows.Add(1);

            ////grdSummary.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ////grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;


            //grid_information[0, 0].Value = "";
            //grid_information[1, 0].Value = "";
            //grid_information[2, 0].Value = "";
            //grid_information[3, 0].Value = "";
            //grid_information[4, 0].Value = "";
            //grid_information[5, 0].Value = "";
            //grid_information[6, 0].Value = "";
            //grid_information[7, 0].Value = "";
            //grid_information[8, 0].Value = "0";

        }

        private void gridview_datadebit(DataGridView grid_debit)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
            dt.Columns.Add("Year");
            dt.Columns.Add("Supplier GSTIN");
            dt.Columns.Add("Supplier Name");
            dt.Columns.Add("Original Credit/Debit Note Date");
            dt.Columns.Add("Original Credit/Debit Note No.");
            dt.Columns.Add("Original Invoice  No.");
            dt.Columns.Add("Revised Credit/Debit Note No.");
            dt.Columns.Add("Revised Credit/Debit Note Date.");
            dt.Columns.Add("Note Value");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Taxable Value");
            dt.Columns.Add("IGST");
            dt.Columns.Add("CGST");
            dt.Columns.Add("SGST");
            dt.Columns.Add("CESS");
            dt.Columns.Add("Place of Supply");
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
            //grid_debit[8, 0].Value = "0";

        }

        private void  gridview_datadistrub(DataGridView grid_distrubution)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
            dt.Columns.Add("Eligilibilty of ITC");
            dt.Columns.Add("Unit Type");
            dt.Columns.Add("GSTIN of Recipient");
            dt.Columns.Add("ISD Document Type");
            dt.Columns.Add("ISD Invoice No.");
            dt.Columns.Add("ISD Invoice Date");
            dt.Columns.Add("ISD Credit Note No.");
            dt.Columns.Add("ISD Credit Note Date");
            dt.Columns.Add("Original Invoice Date");
            dt.Columns.Add("ISD(only for Invoices) as Intergrated Tax IGST ");
            dt.Columns.Add("ISD(only for Invoices) as Intergrated Tax CGST ");
            dt.Columns.Add("ISD(only for Invoices) as Intergrated Tax SGST ");
            dt.Columns.Add("ISD(only for Invoices) as Total Tax  ");
            dt.Columns.Add("ISD(only for Invoices) as Central Tax IGST ");
            dt.Columns.Add("ISD(only for Invoices) as Central Tax CGST ");
            dt.Columns.Add("ISD(only for Invoices) as Central Tax Total ");
            dt.Columns.Add("ISD(only for Invoices) as State/UT IGST ");
            dt.Columns.Add("ISD(only for Invoices) as  State/UT Tax Total ");
            dt.Columns.Add("ISD(only for Invoices CESS)  ");
            dt.Columns.Add("Only for credit Notes IGST ");
            dt.Columns.Add("Only for credit Notes CGST ");
            dt.Columns.Add("Only for credit Notes SGST ");
            dt.Columns.Add("Only for credit Notes CESS ");
            dt.Columns.Add("SEZ Supplier");

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
            dr[21] = "";
            dr[22] = "";
            dr[23] = "";

            dt.Rows.Add(dr);
            grid_distrubution.DataSource = dt;
            //grid_distrub.Rows.Add(1);

            ////grdSummary.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ////grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;


            //grid_distrub[0, 0].Value = "";0
            //grid_distrub[1, 0].Value = "";
            //grid_distrub[2, 0].Value = "";
            //grid_distrub[3, 0].Value = "";
            //grid_distrub[4, 0].Value = "";
            //grid_distrub[5, 0].Value = "";
            //grid_distrub[6, 0].Value = "";
            //grid_distrub[7, 0].Value = "";
            //grid_distrub[8, 0].Value = "0";
            //grid_distrub[9, 0].Value = "";
            //grid_distrub[10, 0].Value = "";
            //grid_distrub[11, 0].Value = "";
            //grid_distrub[12, 0].Value = "";
            //grid_distrub[13, 0].Value = "";
            //grid_distrub[14, 0].Value = "";
            //grid_distrub[15, 0].Value = "";
            //grid_distrub[16, 0].Value = "";
            //grid_distrub[17, 0].Value = "";
            //grid_distrub[18, 0].Value = "0";

        }
        private void gridview_dataITC(DataGridView grid_itc)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Select All");
            dt.Columns.Add("S No.");
            dt.Columns.Add("Original Unit Type");
            dt.Columns.Add("Original GSTIN of Recipient");
            dt.Columns.Add("Original Document No.");
            dt.Columns.Add("Original Document Date");
            dt.Columns.Add("Eligibilty Of ITC");
            dt.Columns.Add("Unit Type");
            dt.Columns.Add("Revised GSTIN of Recipient");
            dt.Columns.Add("ISD Document Type");
            dt.Columns.Add("ISD Invoice Type");
            dt.Columns.Add("ISD Invoice No.");
            dt.Columns.Add("ISD Invoice Date");
            dt.Columns.Add("Revisied ISD Invoice No.");
            dt.Columns.Add("Revisied ISD Invoice Date");
            dt.Columns.Add("Revisied ISD Credit Note No.");
            dt.Columns.Add("Revisied ISD Credit Note Date");
            dt.Columns.Add("Original Invoice No.");
            dt.Columns.Add("Original Invoice Date");
            dt.Columns.Add("ISD(only for Invoices) As Intergrated Tax IGST");
            dt.Columns.Add("ISD(only for Invoices) As Intergrated Tax CGST");
            dt.Columns.Add("ISD(only for Invoices) As Intergrated Tax SGST");
            dt.Columns.Add("ISD(only for Invoices) As Total Tax ");
            dt.Columns.Add("ISD(only for Invoices) As Central  Tax IGST");
            dt.Columns.Add("ISD(only for Invoices) As Central  Tax CGST");
            dt.Columns.Add("ISD(only for Invoices) As Central  Tax Total");
            dt.Columns.Add("ISD(only for Invoices) As State/UT  Tax IGST");
            dt.Columns.Add("ISD(only for Invoices) As State/UT  Tax SGST");
            dt.Columns.Add("ISD(only for Invoices) As State/UT  Tax Total");
            dt.Columns.Add("ISD(only for Invoices CESS) ");
            dt.Columns.Add("only for credit Notes IGST");
            dt.Columns.Add("only for credit Notes CGST");
            dt.Columns.Add("only for credit Notes SGST");
            dt.Columns.Add("only for credit Notes CESS");
            dt.Columns.Add("SEZ Supplier");

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
            dr[21] = "";
            dr[22] = "";
            dr[23] = "";
            dr[24] = "";
            dr[25] = "";
            dr[26] = "";
            dr[27] = "";
            dr[28] = "";
            dr[29] = "";
            dr[30] = "";
            dr[31] = "";
            dr[32] = "";
            dr[33] = "";

            dt.Rows.Add(dr);
            grid_itc.DataSource = dt;

            //grid_itc.Rows.Add(1);

            ////grdSummary.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ////grdSummary.Rows[0].Cells[0].Style.WrapMode = DataGridViewTriState.True;


            //grid_itc[0, 0].Value = "";
            //grid_itc[1, 0].Value = "";
            //grid_itc[2, 0].Value = "";
            //grid_itc[3, 0].Value = "";
            //grid_itc[4, 0].Value = "";
            //grid_itc[5, 0].Value = "";
            //grid_itc[6, 0].Value = "";
            //grid_itc[7, 0].Value = "";
            //grid_itc[8, 0].Value = "";
            //grid_itc[9, 0].Value = "";
            //grid_itc[10, 0].Value = "";
            //grid_itc[11, 0].Value = "";
            //grid_itc[12, 0].Value = "";
            //grid_itc[13, 0].Value = "";
            //grid_itc[14, 0].Value = "";
            //grid_itc[15, 0].Value = "";
            //grid_itc[16, 0].Value = "";
            //grid_itc[17, 0].Value = "";
            //grid_itc[18, 0].Value = "0";

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

                //grd_check.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                Bind_GridTax(grid_tax);
                //grd_check.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                // grd_check.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                // grd_check.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                //grd_check.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                //amendGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                gridview_Datadebit(grid_debitreceived);
                // grid_year.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                //  grid_year.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                //  grid_year.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                //  grid_year.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                gridview_Datainformation(grid_information);

                gridview_datadebit(grid_debit);

                gridview_datadistrub(grid_distrubution);

                gridview_dataITC(grid_itc);
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

        private void GSTR6_Reports_Load(object sender, EventArgs e)
        {
            GetSetSoftwareData();
            //label6.Text = "5. Distribution  of Input Tax  Credit  received in Table 4 " + Environment.NewLine + "8.  Distributed  of input Tax  credit  reported in Table NO 6 B";
        }

        private void tabsoftware_DrawItem(object sender, DrawItemEventArgs e)
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void customDataGridViews2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void grid_debit_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grdSummary_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            


        }

        private void grid_distrub_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grdSummary_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grdSummary_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == 0)
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
            if (e.RowIndex == 3)
            {
                tabsoftware.SelectTab(4);
            }
            if (e.RowIndex == 4)
            {
                tabsoftware.SelectTab(5);
            }
            if (e.RowIndex == 5)
            {
                tabsoftware.SelectTab(6);
            }

        }
    }
}
