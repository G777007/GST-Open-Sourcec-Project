using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SPEQTAGST.BAL;
using SPEQTAGST.BAL.M956r2;

namespace SPEQTAGST.xasjbr1
{
    public partial class SPQGSTR1AdvanceReceived : Form
    {
        public SPQGSTR1AdvanceReceived()
        {
            InitializeComponent();
            GetData();

            dgvMain.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dgvMain.EnableHeadersVisualStyles = false;
            dgvMain.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Verdana", 8, FontStyle.Bold);
            dgvMain.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvMain.Font = new System.Drawing.Font("Verdana", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void GetData()
        {
            try
            {
                DataTable dtMain = new DataTable();
                dtMain.Columns.Add("Name");
                dtMain.Columns.Add("Gross Advance Received");
                dtMain.Columns.Add("IGST Amount");
                dtMain.Columns.Add("CGST Amount");
                dtMain.Columns.Add("SGST Amount");
                dtMain.Columns.Add("CESS Amount");

                DataTable dt;
                #region Form Gross Advance
                string Query = "Select * from SPQR1GrossAdvance where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' And Fld_FileStatus='Total'";
                dt = new DataTable();
                dt = new r2Publicclass().GetDataGSTR2(Query);
                if (dt != null && dt.Rows.Count > 0)
                {
                    dtMain.Rows.Add("Advance Tax Paid", Convert.ToString(dt.Rows[0]["Fld_GrossAdvRcv"]), Convert.ToString(dt.Rows[0]["Fld_IGSTAmnt"]), Convert.ToString(dt.Rows[0]["Fld_CGSTAmnt"]), Convert.ToString(dt.Rows[0]["Fld_SGSTAmnt"]), Convert.ToString(dt.Rows[0]["Fld_CessAmount"]));
                }
                else
                {
                    dtMain.Rows.Add("Advance Tax Paid", "0", "0", "0", "0", "0");
                }
                #endregion

                #region Form Gross Advance Summary
                Query = "Select * from SPQR1GrossAdvance where Fld_Month='" + CommonHelper.SelectedMonth + "' AND Fld_FinancialYear = '" + CommonHelper.ReturnYear + "' And Fld_FileStatus='Total'";
                dt = new DataTable();
                dt = new r2Publicclass().GetDataGSTR2(Query);
                if (dt != null && dt.Rows.Count > 0)
                {
                    dtMain.Rows.Add("Advance Tax Paid (System Summary)", Convert.ToString(dt.Rows[0]["Fld_GrossAdvRcv"]), Convert.ToString(dt.Rows[0]["Fld_IGSTAmnt"]), Convert.ToString(dt.Rows[0]["Fld_CGSTAmnt"]), Convert.ToString(dt.Rows[0]["Fld_SGSTAmnt"]), Convert.ToString(dt.Rows[0]["Fld_CessAmount"]));
                }
                else
                {
                    dtMain.Rows.Add("Advance Tax Paid (System Summary)", "0", "0", "0", "0", "0");
                }
                #endregion

                dgvMain.DataSource = dtMain;
                dgvMain.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvMain.Columns["Gross Advance Received"].Width = 140;
                dgvMain.Columns["IGST Amount"].Width = 130;
                dgvMain.Columns["CGST Amount"].Width = 130;
                dgvMain.Columns["SGST Amount"].Width = 130;
                dgvMain.Columns["CESS Amount"].Width = 130;
                dgvMain.ColumnHeadersHeight = 50;
                DataGridViewRow row = this.dgvMain.RowTemplate;
                row.MinimumHeight = 25;


                dgvMain.Columns["Name"].DefaultCellStyle.Font = new Font(dgvMain.Font, FontStyle.Bold);
                dgvMain.Columns["Name"].DefaultCellStyle.ForeColor = Color.Blue;
                dgvMain.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
                string errorMessage = string.Format("Error: {0}{1}Source: {2}{3}Error Time: {4}{5}", ex.Message, Environment.NewLine, ex.StackTrace, Environment.NewLine, DateTime.Now, Environment.NewLine);
                StreamWriter errorWriter = new StreamWriter("SPEQTA_Error_File.txt", true);
                errorWriter.Write(errorMessage);
                errorWriter.Close();
            }
        }
        
        private void msClose_Click(object sender, EventArgs e)
        {
            SPQGSTR1Dashboard obj = new SPQGSTR1Dashboard();
            obj.MdiParent = this.MdiParent;
            Utility.CloseAllOpenForm();
            obj.Dock = DockStyle.Fill;
            obj.Show();


            ((SPQMDI)Application.OpenForms["SPQMDI"]).HideExtraToolsMenu();
            ((SPQMDI)Application.OpenForms["SPQMDI"]).HideCompanyDetailMenu();
        }

        private void dgvMain_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == 0 && e.ColumnIndex == 0)
            {
                SPQGSTR1GrossAdvance obj = new SPQGSTR1GrossAdvance();
                obj.MdiParent = this.MdiParent;
                Utility.CloseAllOpenForm();
                obj.Dock = DockStyle.Fill;

                obj.Show();

                ((SPQMDI)Application.OpenForms["SPQMDI"]).ShowExtraToolsMenu();
                ((SPQMDI)Application.OpenForms["SPQMDI"]).ShowCompanyDetailMenu();
            }
            else if (e.RowIndex == 1 && e.ColumnIndex == 0)
            {
                SPQGSTR1AdvanceTaxPaid obj = new SPQGSTR1AdvanceTaxPaid();
                obj.MdiParent = this.MdiParent;
                Utility.CloseAllOpenForm();
                obj.Dock = DockStyle.Fill;

                obj.Show();


                ((SPQMDI)Application.OpenForms["SPQMDI"]).HideExtraToolsMenu();
                ((SPQMDI)Application.OpenForms["SPQMDI"]).HideCompanyDetailMenu();
            }
        }
    }
}
