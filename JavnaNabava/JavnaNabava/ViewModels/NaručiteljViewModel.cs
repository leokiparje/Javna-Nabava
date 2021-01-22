using System.Collections.Generic;
using JavnaNabava.Models;
using JavnaNabava.ViewModels;

namespace JavnaNabava.ViewModels {

    /// <summary>
    /// Pogled na model za dohvaćanje svih naručitelja
    /// </summary>
    public class NaručiteljViewModel {

        public IEnumerable<Naručitelj> Naručitelji { get; set; }
        public PagingInfo PagingInfo { get; set; }

    }
    
}