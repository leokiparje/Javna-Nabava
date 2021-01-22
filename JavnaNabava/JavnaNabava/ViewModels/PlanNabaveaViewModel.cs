using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;

namespace JavnaNabava.ViewModels

{
    /// <summary>
    /// Pogled na model koji dohvaća jedan plan nabave
    /// </summary>
    public class PlanNabaveaViewModel
    {

        public int evidBrojPlan { get; set; }

        
        public decimal vrijednost { get; set; }

        
        public string CPV { get; set; }

        

        public PagingInfo PagingInfo { get; set; }
    }
}
