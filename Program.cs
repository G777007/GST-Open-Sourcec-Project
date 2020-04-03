using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPEQTAGST_DESIGN
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
           Application.SetCompatibleTextRenderingDefault(false);
           // Application.Run(new SPEQTAGST.Report.GSTR_9_Reports());
           //Application.Run(new GSTR2A__Reports());
           //Application.Run(new GSTR_4A__Reports());
           //Application.Run(new GSTR_7_Reports());
           //Application.Run(new GSTR_4A_Reports());
           //Application.Run(new GSTR_3BREPORTS());
           //Application.Run(new GSTR_6A_Reports());
           //Application.Run(new GSTR6_Reports());
           //Application.Run(new GSTR1_Reports());
           //Application.Run(new formCMP_08());
           //Application.Run(new month_year());
           //Application.Run(new Verification());
           Application.Run(new gstrcertificate());
           //Application.Run(new GSTR_AddNotice());
           //Application.Run(new GST_DownloadSummary());
           //Application.Run(new GSTR_3BREPORTS());
           //Application.Run(new Form2());
           //Application.Run(new GSTR_3B_PDF());
           // Application.Run(new GSTR_1_PDF());
            //Application.Run(new PDF_Demo());
          

        }
    }
}
