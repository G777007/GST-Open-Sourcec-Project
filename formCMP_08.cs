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
    public partial class formCMP_08 : Form
    {
        public formCMP_08()
        {
            InitializeComponent();
        }

        private void DataBind_grd_cmp_08(DataGridView Grd_Cmp08 )
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("S No.");
            dt.Columns.Add("Description");
            dt.Columns.Add("Value");
            dt.Columns.Add("Intergrated Tax");
            dt.Columns.Add("Central Tax");
            dt.Columns.Add("State/UT Tax");
            dt.Columns.Add("Cess");
            dt.Columns.Add("Total");
            
            DataRow dr = dt.NewRow();
            dr[0] = "1";
            dr[1] = "Outward Supplies (including exempt Supplies) ";
            dr[2] = "0.0";
            dr[3]  = "";
            dr[4] = "0.0";
            dr[5] = "0.0";
            dr[6] = "";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "2";
            dr[1] = "Inward Supplies  attracting reverse charge including import of services ";
            dr[2] = "0.0";
            dr[3] = "0.0";
            dr[4] = "0.0";
            dr[5] = "0.0";
            dr[6] = "0.0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "3";
            dr[1] = "Tax Payable (1+2)  ";
            dr[2] = "0.0";
            dr[3] = "0.0";
            dr[4] = "0.0";
            dr[5] = "0.0";
            dr[6] = "0.0";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "4";
            dr[1] = "Interset Payable , if any  ";
            dr[2] = "";
            dr[3] = "0.0";
            dr[4] = "0.0";
            dr[5] = "0.0";
            dr[6] = "0.0";
            dt.Rows.Add(dr);

            Grd_Cmp08.DataSource = dt;
            
            Grd_Cmp08.Rows[0].Cells[3].Style.BackColor = Color.Silver;

            Grd_Cmp08.Rows[0].Cells[6].Style.BackColor = Color.Silver;
            Grd_Cmp08.Rows[3].Cells[2].Style.BackColor = Color.Silver;
            Grd_Cmp08.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Grd_Cmp08.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            Grd_Cmp08.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


            foreach (DataGridViewColumn col in Grd_Cmp08.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

        }

        private void formCMP_08_Load(object sender, EventArgs e)
        {
            DataBind_grd_cmp_08(Grd_Cmp08);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
