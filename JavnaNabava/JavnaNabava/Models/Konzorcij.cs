using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class Konzorcij
    {
        public Konzorcij()
        {
            KontaktKonzorcijs = new HashSet<KontaktKonzorcij>();
            ČlanoviKonzorcijas = new HashSet<ČlanoviKonzorcija>();
        }

        [Display(Name = "ID konzorcija", Prompt = "Unesite ID")]
        public int IdKonzorcij { get; set; }
        public string NazivKonzorcij { get; set; }

        public virtual ICollection<KontaktKonzorcij> KontaktKonzorcijs { get; set; }
        public virtual ICollection<ČlanoviKonzorcija> ČlanoviKonzorcijas { get; set; }
    }
}
