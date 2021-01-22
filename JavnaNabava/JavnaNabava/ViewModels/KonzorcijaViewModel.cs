using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.ViewModels
{
    public class KonzorcijaViewModel
    {
        public int idKonzorcij { get; set; }
        public List<PonuditeljaViewModel> Clanovi { get; internal set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
