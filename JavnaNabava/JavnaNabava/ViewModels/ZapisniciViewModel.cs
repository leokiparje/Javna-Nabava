using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;

namespace JavnaNabava.ViewModels
{
    /// <summary>
    /// Klasa ZapisniciViewModel predstavlja pogled na zapisnike
    /// </summary>
    public class ZapisniciViewModel
    {
        public IEnumerable<ViewZapisnikInfo> Zapisnici { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
