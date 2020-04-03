using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SPEQTAGST.BAL;

namespace SPEQTAGST.xasjbr1
{
    public partial class SPQGSTR1ImportDialoge : Form
    {
        public SPQGSTR1ImportDialoge()
        {
            InitializeComponent();
            lblYear.Text = "F.Y. " + CommonHelper.ReturnYear;

        }
        public SPQGSTR1ImportDialoge(string strReco)
        {
            InitializeComponent();
            lblYear.Text = "F.Y. " + CommonHelper.ReturnYear;
            lblMainHeader.Text = "Import Purchase Books Data ";
            btnImportGstin.Visible = false;
            btnImportJson.Visible = false;


        }

        public string Gstr1ImportFilePath { get; set; }
        public string Gstr1ImportType { get; set; }
        private void SPQGSTR1ImportDialoge_Load(object sender, EventArgs e)
        {
            
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            Button btnimport = sender as Button;
            this.AcceptButton = btnimport;
            if (btnimport.Name == "btnImportSoftExcel") //rdbSoftExcel.Checked == true &&
            {
                Gstr1ImportType = "SOFT";
            }
            //else
            //{
            //    MessageBox.Show("Please Select Import Type Software excel");
            //    return;
            //}

                //rdbTallyExcel.Checked == true &&
            else if ( btnimport.Name == "btnImportTallyExcel") { Gstr1ImportType = "TALLY"; }
            //else
            //{
            //    MessageBox.Show("Please Select Import Type Tally excel");
            //    return;
            //}
            //rdbGstin.Checked == true &&
            else if ( btnimport.Name == "btnImportGstin") { Gstr1ImportType = "GSTIN"; }
            //else
            //{
            //    MessageBox.Show("Please Select Import Type GSTIN ");
            //    return;
            //}
            //rdbJson.Checked == true &&
            else if ( btnimport.Name == "btnImportJson") { Gstr1ImportType = "JSON"; }
            else
            {
                MessageBox.Show("Please Select Correct Import Type");
                return;
            }

            // OpenFileDialog file = new OpenFileDialog();
            // file.InitialDirectory = Environment.SpecialFolder.Desktop;
            //file.Multiselect= false;
            
            //if (rdbSoftExcel.Checked)
            //{
            //    //file.DefaultExt = "Excel Files|*.xls;*.xlsx;*.xlsm";
            //    Gstr1ImportType = "SOFT";
            //}
            //if (rdbTallyExcel.Checked)
            //{
            //    //file.DefaultExt = "Excel Files|*.xls;*.xlsx;*.xlsm";
            //    Gstr1ImportType = "TALLY";
            //}
            //if (rdbGstin.Checked)
            //{
            //    //file.DefaultExt = "Json files (*.json)|*.json|Text files (*.txt)|*.txt";
            //    Gstr1ImportType = "GSTIN";
            //}
            //if (rdbJson.Checked)
            //{
            //   // file.DefaultExt = "Json files (*.json)|*.json|Text files (*.txt)|*.txt";
            //    Gstr1ImportType = "JSON";
            //}

           // var result = file.ShowDialog();
            //string filepath = file.FileName;
            this.Gstr1ImportFilePath =  ""; //file.FileName;
            //if (result == System.Windows.Forms.DialogResult.OK)
            //{
            //    //MessageBox.Show(this.Gstr1ImportFilePath);
            //    this.Close();
            //}
            //else
            //{ 
                
            //}
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pnlContent_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}
