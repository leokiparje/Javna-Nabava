using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.ViewModels
{
    /// <summary>
    /// Pogled OvlViewModel na model za dohvaćanje svih ovlaštenika.
    /// </summary>
    public class OvlViewModel
    {
        public int OibOvlaštenik { get; set; }
        public string ImeOvlaštenik { get; set; }
        public string PrezimeOvlaštenik { get; set; }
        public string OibNaručitelj { get; set; }
        public int IdPovjerenstva { get; set; }
    }
}
