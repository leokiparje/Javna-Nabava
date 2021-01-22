using System;
using System.Collections.Generic;
using JavnaNabava.Models;
using System.ComponentModel.DataAnnotations;

namespace JavnaNabava.ViewModels
{    /// <summary>
    /// Jednostavniji pogled na model za dohvat jedne ponude
    /// </summary>
    public class PonudaViewModel
    {
        public int PonudaId { get; set; }
        public string NazivNatječaja { get; set; }
        public string Text { get; set; }
        public string Naslov { get; set; }
        public string NazivPonuditelj { get; set; }
        public string OibPonuditelj { get; set; }
        public int EvidBrojNatječaj { get; set; }


        public IEnumerable<DokumentViewModel> doks { get; set; } 
        public PonudaViewModel()
        {
            this.doks = new List<DokumentViewModel>();
        }

    }
}
