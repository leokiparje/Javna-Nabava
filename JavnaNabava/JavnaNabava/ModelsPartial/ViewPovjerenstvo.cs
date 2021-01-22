using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.Models
{
    /// <summary>
    /// Klasa ViewPovjerenstvo predstavlja pogled za pristup varijabli naziv povjerenstva preko id-a povjerenstva.
    /// </summary>
    public class ViewPovjerenstvo
    {
        public int IdPovjerenstva { get; set; }
        public string NazivPovjerenstva { get; set; }
    }
}
