using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace JavnaNabava.Models
{
    /// <summary>
    ///Model koji opisuje naručitelja sa svim potrebnim atributima
    /// </summary>
    public partial class Naručitelj
    {
        public Naručitelj()
        {
            Naručiteljovlašteniks = new HashSet<Naručiteljovlaštenik>();
            PlanNabaves = new HashSet<PlanNabave>();
        }
        [System.ComponentModel.DataAnnotations.Display(Name = "OIB naručitelja", Prompt = "Unesite OIB")]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "OIB naručitelja je obvezno polje!")]
        [System.ComponentModel.DataAnnotations.RegularExpression("^[0-9]*$", ErrorMessage = "OIB naručitelja smije sadržavati samo znamenke!")]
        [System.ComponentModel.DataAnnotations.MinLength(11, ErrorMessage = "OIB naručitelja mora biti duljine 11 znakova!")]
        [MaxLength(11)]
        public string OibNaručitelja { get; set; }
        [Display(Name = "Naziv", Prompt = "Unesite naziv naručitelja")]
        [Required(ErrorMessage = "Naziv naručitelja je obvezno polje!")]
        public string NazivNaručitelja { get; set; }

        [Display(Name = "Adresa", Prompt = "Unesite adresu naručitelja")]
        [Required(ErrorMessage = "Adresa naručitelja je obvezno polje!")]
        public string AdresaNaručitelja { get; set; }

        [Display(Name = "Poštanski broj", Prompt = "Unesite poštanski broj naručitelja")]
        [Required(ErrorMessage = "Poštanski broj naručitelja je obvezno polje!")]
        public int PoštanskiBrojNaručitelja { get; set; }

        public virtual ICollection<Naručiteljovlaštenik> Naručiteljovlašteniks { get; set; }
        public virtual ICollection<PlanNabave> PlanNabaves { get; set; }
    }
}
