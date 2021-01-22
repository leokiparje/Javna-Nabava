using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class Zapisnik
    {
        /// <summary>
        /// Model Zapisnik predstavlja jedan zapisnik
        /// Zapisnik ima atribute : id ponude, id zapisnika, id povjerenstva, naziv zapisnika, ispravnost zapisnika i id prethodnog zapisnika.
        /// </summary>
        public Zapisnik()
        {
            StavkaZapisniks = new HashSet<StavkaZapisnik>();
        }

        public int PonudaId { get; set; }
        public int ZapisnikId { get; set; }
        public int IdPovjerenstva { get; set; }
        public string NazivZapisnik { get; set; }
        public int? IdPrethZapisnika { get; set; }
        public string IspravnostZapisnik { get; set; }

        public virtual Povjerenstvo IdPovjerenstvaNavigation { get; set; }
        public virtual Ponudum Ponuda { get; set; }
        public virtual ICollection<StavkaZapisnik> StavkaZapisniks { get; set; }
    }
}
