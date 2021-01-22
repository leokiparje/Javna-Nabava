using System.Collections.Generic;
using JavnaNabava.Models;
using JavnaNabava.ViewModels;
using MVC.ViewModels;

namespace JavnaNabava.ViewModels {
    /// <summary>
    /// Pogled na model koji dohvaća sve zaposlenike
    /// </summary>
    public class ZaposleniciViewModel {

        public IEnumerable<Zaposlenik> Zaposlenici { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public ZaposlenikFilter Filter { get; set; }



    }
    
}