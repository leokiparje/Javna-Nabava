using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;

namespace JavnaNabava.ViewModels
{   /// <summary>
    /// Jednostavniji pogled na model za dohvat jedne PonudaStavke
    /// </summary>
    public class PonudaStavkeViewModel
    {
        public int PonudaId { get; set; }
        public string NazivStavke { get; set; }
        public decimal CijenaStavke { get; set; }
        public int KoličinaStavke { get; set; }
    }
}
