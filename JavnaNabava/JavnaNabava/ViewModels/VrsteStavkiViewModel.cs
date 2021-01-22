using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;

namespace JavnaNabava.ViewModels
{
    /// <summary>
    /// View model koji služi za stvaranje liste stavki
    /// </summary>
    public class VrsteStavkiViewModel
    {
        public IEnumerable<VrstaStavke> VrsteStavki { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
