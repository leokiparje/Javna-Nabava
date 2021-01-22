using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;

namespace JavnaNabava.ViewModels
{
    /// <summary>
    /// Pogled za dohvat svih ponuda
    /// </summary>
    public class PonudeViewModel
    {
        public IEnumerable<PonudaViewModel> Ponude { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
