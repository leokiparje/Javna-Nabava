using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace JavnaNabava.Models
{
    /// <summary>
    /// Model Ovlaštenik predstavlja jednog ovlaštenika.
    /// Ovlaštenik ima atribute : ime, prezime, oib, oib naručitelja i id povjerenstva.
    /// </summary>
    public partial class Ovlaštenik
    {
        public Ovlaštenik()
        {
            Naručiteljovlašteniks = new HashSet<Naručiteljovlaštenik>();
            Ovlaštenikpovjerenstvos = new HashSet<Ovlaštenikpovjerenstvo>();
        }
        [Display(Name = "Ime ovlaštenika")]
        public string ImeOvlaštenik { get; set; }
        [Display(Name = "Prezime ovlaštenika")]
        public string PrezimeOvlaštenik { get; set; }
        [Display(Name = "Oib ovlaštenika")]
        public int OibOvlaštenik { get; set; }
        [Display(Name = "Oib naručitelja")]
        public string OibNaručitelja { get; set; }
        [Display(Name = "Id povjerenstva")]
        public int IdPovjerenstva { get; set; }

        public virtual ICollection<Naručiteljovlaštenik> Naručiteljovlašteniks { get; set; }
        public virtual ICollection<Ovlaštenikpovjerenstvo> Ovlaštenikpovjerenstvos { get; set; }
    }
}
