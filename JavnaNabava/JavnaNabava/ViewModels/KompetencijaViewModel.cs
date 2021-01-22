using System;
using System.Collections.Generic;
using JavnaNabava.Models;
using JavnaNabava.ViewModels;

namespace JavnaNabava.ViewModels {
    public class KompetencijaViewModel {
        public int IdKompetencije { get; set; }
        public string NazivKompetencije { get; set; }
        public string SjedišteObrazovneUstanove { get; set; }
        public DateTime PočetakObrazovanja { get; set; }
        public DateTime KrajObrazovanja { get; set; }


    }
    
}