using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.Models
{
    /// <summary>
    /// Klasa ViewPonuda Predstavlja pogled za pristup text varijabli ponude preko njenog id-a.
    /// </summary>
    public class ViewPonuda
    {
        public int PonudaId { get; set; }
        public string Text { get; set; }
    }
}
