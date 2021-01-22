using System.Collections.Generic;
using JavnaNabava.Models;
using JavnaNabava.ViewModels;

namespace JavnaNabava.ViewModels {
    /// <summary>
    /// Pogled na model za dohvaćanje svih natječaja
    /// </summary>
    public class NatječajViewModel {

        public IEnumerable<Natječaj> Natječaji { get; set; }
        public PagingInfo PagingInfo { get; set; }

 

    }
    
}