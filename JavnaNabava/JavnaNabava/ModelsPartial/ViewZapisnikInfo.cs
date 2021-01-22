using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.Models
{
    /// <summary>
    /// Klasa ViewZapisnikInfo koristi se kao pogled u bazi podataka u kojem se nalaze relevantne varijable korištene u prikazu zapisnika.
    /// </summary>
    public class ViewZapisnikInfo
    {
        public int ZapisnikId { get; set; }
        public int? IdPrethZapisnika { get; set; }
        public string NazivZapisnik { get; set; }
        public int IdPovjerenstva { get; set; }
        public string NazivPovjerenstva { get; set; }
        public int PonudaId { get; set; }
        public string TekstPonude { get; set; }

        [NotMapped]
        public int Position { get; set; }
    }
}
