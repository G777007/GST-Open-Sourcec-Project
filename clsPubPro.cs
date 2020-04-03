using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SPEQTAGST_DESIGN
{
    public class clsPubPro
    {

        public CookieContainer CC1 { get; set; }
        public string ckname { get; set; }
    }
    public class clsPro
    {
        public static CookieContainer CC { get; set; }
        public static List<clsPubPro> Cooki { get; set; }
    }
}
