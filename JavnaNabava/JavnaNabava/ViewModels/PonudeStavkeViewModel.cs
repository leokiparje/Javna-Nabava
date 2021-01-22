using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;

namespace JavnaNabava.ViewModels
{
    /// <summary>
    /// Model za dohvacanje svih PonudaStavki
    /// </summary>
    public class PonudeStavkeViewModel
    {
        public IEnumerable<PonudaStavkeViewModel> PonudeStavke { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
