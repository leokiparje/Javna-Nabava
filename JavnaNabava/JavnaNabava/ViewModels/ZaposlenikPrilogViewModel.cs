using System.Collections.Generic;
using JavnaNabava.Models;
using JavnaNabava.ViewModels;

namespace JavnaNabava.ViewModels {

    /// <summary>
    /// Pogled na model koji opisuje vezu između zaposlenika i priloga tog zaposlenika
    /// </summary>
    public class ZaposlenikPrilogViewModel {

        public string OibZaposlenika { get; set; }

        public int IdPriloga { get; set; }
        public string nazivPriloga { get; set; }

        public IEnumerable<ZaposlenikPrilog> ZaposleniciPrilozi { get; set; }
        public PagingInfo PagingInfo { get; set; }

 

    }
    
}