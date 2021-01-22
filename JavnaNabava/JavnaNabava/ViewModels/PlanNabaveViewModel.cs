using System.Collections.Generic;
using JavnaNabava.Models;
using JavnaNabava.ViewModels;

namespace JavnaNabava.ViewModels {
    /// <summary>
    /// Pogled na model koji dohvaća sve planove nabave
    /// </summary>
    public class PlanNabaveViewModel {

        public IEnumerable<PlanNabave> PlanoviNabave { get; set; }
        public PagingInfo PagingInfo { get; set; }

 

    }
    
}