using System;
using System.Collections.Generic;
using JavnaNabava.Models;
using JavnaNabava.ViewModels;

namespace JavnaNabava.ViewModels {

    /// <summary>
    /// Pogled na model koji opisuje jednog zaposlenika
    /// </summary>
    public class ZaposlenikViewModel {
        public string KompetencijaName {get; set; }
        public string SpremaName {get; set; }

        public string oibZaposlenik { get; set; }
        public string ImeZaposlenik { get; set; }
        public string PrezimeZaposlenik { get; set; }
        public DateTime DatumRođenja { get; set; }
        public string MjestoPrebivališta { get; set; }
        public int IdKompetencije { get; set; }
        public int IdStručneSpreme { get; set; }
        public string OibPonuditelj { get; set; }

        public string oibPrethodnog { get; set; }
        public string oibSljedećeg { get; set; }


        public List<ZaposlenikPrilogViewModel> Prilozi { get; internal set; }
        public PagingInfo PagingInfo { get; set; }

    }
    
}