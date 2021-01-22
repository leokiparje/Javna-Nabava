using System;
using System.Collections.Generic;
using JavnaNabava.Models;
using JavnaNabava.ViewModels;

namespace JavnaNabava.ViewModels {
    /// <summary>
    /// Pogled na model koji opisuje vrstu kompetencije zaposlenika sa svim potrebnim atributima
    /// </summary>
    public class VrstaKompetencijeViewModel {
        public int IdKompetencije { get; set; }
        public string NazivKompetencije { get; set; }
        public string SjedišteObrazovneUstanove { get; set; }
        public DateTime PočetakObrazovanja { get; set; }
        public DateTime KrajObrazovanja { get; set; }

        public IEnumerable<VrstaKompetencije> Kompetencije { get; set; }
        public PagingInfo PagingInfo { get; set; }

    }
    
}