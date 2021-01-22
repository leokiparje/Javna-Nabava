using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;

namespace JavnaNabava.ViewModels
{
    /// <summary>
    /// View model troškovnika koji dodatno sadrži naziv natječaja i sve svoje stavke
    /// </summary>
    public class TroškovnikViewModel
    {

        public int TroškovnikId { get; set; }

        public string NazivNatječaja { get; set; }

        public int IdNatječaja { get; set; }

        public PagingInfo PagingInfo { get; set; }
        public IEnumerable<StavkaUTroškovnikuViewModel> Stavke { get;  set; }

        public TroškovnikViewModel()
        {
            this.Stavke = new List<StavkaUTroškovnikuViewModel>();
        }
    }
}
