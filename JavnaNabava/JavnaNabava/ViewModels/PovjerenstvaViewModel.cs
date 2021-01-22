using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;

namespace JavnaNabava.ViewModels
{
    /// <summary>
    /// Pogled PovjerenstvaViewModel na model za dohvaćanje svih povjerenstava.
    /// </summary>
    public class PovjerenstvaViewModel
    {
        public IEnumerable<PovjerenstvoViewModel> Povjerenstva { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
