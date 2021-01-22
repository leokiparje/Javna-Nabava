using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;

namespace JavnaNabava.ViewModels
{/// <summary>
/// View model koji služi za stvaranje klase koja ce imati Naziv natječaja
/// </summary>
    public class TroškovniciViewModel
    {
        public IEnumerable<TroškovnikViewModel> Troškovnici { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
