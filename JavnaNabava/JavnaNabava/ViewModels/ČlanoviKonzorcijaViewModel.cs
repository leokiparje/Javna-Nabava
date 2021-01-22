using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.ViewModels
{
    public class ČlanoviKonzorcijaViewModel
    {
        public IEnumerable<ČlanKonzorcijaViewModel> ČlanoviKonzorcija { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
