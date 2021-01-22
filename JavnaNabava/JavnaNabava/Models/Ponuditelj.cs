using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace JavnaNabava.Models
{
    /// <summary>
    /// Model za ponuditelja generiran iz baze podataka
    /// Sadrži OIB ponuditelja, naziv, adresu te sjedište s dodanim validacijskim atributima
    /// </summary>
    public partial class Ponuditelj
    {
        public Ponuditelj()
        {
            KontaktPonuditeljs = new HashSet<KontaktPonuditelj>();
            PonudaPonuditeljs = new HashSet<PonudaPonuditelj>();
            Zaposleniks = new HashSet<Zaposlenik>();
            ČlanoviKonzorcijas = new HashSet<ČlanoviKonzorcija>();
        }

        [System.ComponentModel.DataAnnotations.Display(Name = "OIB ponuditelja", Prompt = "Unesite OIB")]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "OIB ponuditelja je obvezno polje!")]
        [System.ComponentModel.DataAnnotations.RegularExpression("^[0-9]*$", ErrorMessage = "OIB ponuditelja smije sadržavati samo znamenke!")]
        [System.ComponentModel.DataAnnotations.MinLength(11, ErrorMessage = "OIB ponuditelja mora biti duljine 11 znakova!")]
        [MaxLength(11)]
        public string OibPonuditelj { get; set; }
        public string NazivPonuditelj { get; set; }
        public string AdresaPonuditelj { get; set; }
        public string SjedištePonuditelj { get; set; }

        public virtual ICollection<KontaktPonuditelj> KontaktPonuditeljs { get; set; }
        public virtual ICollection<PonudaPonuditelj> PonudaPonuditeljs { get; set; }
        public virtual ICollection<Zaposlenik> Zaposleniks { get; set; }
        public virtual ICollection<ČlanoviKonzorcija> ČlanoviKonzorcijas { get; set; }
    }
}
