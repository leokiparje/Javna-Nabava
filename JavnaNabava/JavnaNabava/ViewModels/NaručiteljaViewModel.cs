using System.Collections.Generic;
using JavnaNabava.Models;
using JavnaNabava.ViewModels;

namespace JavnaNabava.ViewModels {

    /// <summary>
    /// Pogled na model koji dohvaća jednog naručitelja sa svim potrebnim atributima
    /// </summary>
    public class NaručiteljaViewModel {

        public string oibNaručitelj { get; set; }
        public string nazivNaručitelj { get; set; }
        public string adresaNaručitelj { get; set; }
        public int poštanskiBrojNaručitelj { get; set; }
        public List<PlanNabaveaViewModel> Planovi { get; internal set; }
        public PagingInfo PagingInfo { get; set; }

    }
    
}