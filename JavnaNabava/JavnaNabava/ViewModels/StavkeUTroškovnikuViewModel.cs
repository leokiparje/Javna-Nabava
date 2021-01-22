using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;

namespace JavnaNabava.ViewModels
{
    /// <summary>
    /// View model koji služi za stvaranje klase koja ce imati Naziv vrste
    /// </summary>
    public class StavkeUTroškovnikuViewModel
    {
        public IEnumerable<StavkaUTroškovnikuViewModel> StavkeUTroškovniku { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
