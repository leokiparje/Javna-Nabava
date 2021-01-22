using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;

namespace JavnaNabava.ViewModels
{
    /// <summary>
    /// Pogled za dohvat svih dokumenata
    /// </summary>
    public class DokumentiViewModel
    {
        public IEnumerable<DokumentViewModel> Dokumenti { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}