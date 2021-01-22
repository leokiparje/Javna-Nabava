using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;

namespace JavnaNabava.ViewModels
{
    /// <summary>
    /// Pogled OvlaštenikViewModel na model za dohvaćanje svih ovlaštenika
    /// </summary>
    public class OvlaštenikViewModel
    {
        public IEnumerable<Ovlaštenik> Ovlaštenici { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
