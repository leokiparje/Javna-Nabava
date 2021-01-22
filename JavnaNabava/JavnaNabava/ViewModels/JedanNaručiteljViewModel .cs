using System.Collections.Generic;
using JavnaNabava.Models;
using JavnaNabava.ViewModels;

namespace JavnaNabava.ViewModels {
    /// <summary>
    /// Jednostavniji pogled na model za dohvat jednog naručitelja
    /// </summary>
    public class JedanNaručiteljViewModel {

        public string oibNaručitelj { get; set; }
        public string nazivNaručitelj { get; set; }
        public string adresaNaručitelj { get; set; }
        public int poštanskiBrojNaručitelj { get; set; }


    }
    
}