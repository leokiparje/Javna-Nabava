using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class VwZapisnici
    {
        public int ZapisnikId { get; set; }
        public string NazivZapisnik { get; set; }
        public int IdPovjerenstva { get; set; }
        public int PonudaId { get; set; }
        public int? IdPrethZapisnika { get; set; }
        public string NazivPovjerenstva { get; set; }
        public string NazivPonude { get; set; }
    }
}
