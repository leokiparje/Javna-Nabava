using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class Povjerenstvo
    {
        /// <summary>
        /// Model Povjerenstvo predstavlja jedno povjerenstvo.
        /// Povjerenstvo ima atribute : id povjerenstva, naziv povjerenstva i evidencijski broj natječaja.
        /// </summary>
        public Povjerenstvo()
        {
            Ovlaštenikpovjerenstvos = new HashSet<Ovlaštenikpovjerenstvo>();
            Zapisniks = new HashSet<Zapisnik>();
        }

        public int IdPovjerenstva { get; set; }
        public string NazivPovjerenstva { get; set; }
        public int EvidBrojNatječaj { get; set; }

        public virtual Natječaj EvidBrojNatječajNavigation { get; set; }
        public virtual ICollection<Ovlaštenikpovjerenstvo> Ovlaštenikpovjerenstvos { get; set; }
        public virtual ICollection<Zapisnik> Zapisniks { get; set; }
    }
}
