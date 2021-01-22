using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.ViewModels
{
    public class PonuditeljaViewModel
    {
        public string oibPonditelj { get; set; }
        public string nazivPonuditelj { get; set; }
        public string adresaPonuditelj { get; set; }
        public string sjedistePonuditelj { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
