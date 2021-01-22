using System;
using System.Collections.Generic;

#nullable disable
using System.ComponentModel.DataAnnotations;
namespace JavnaNabava.Models
{
    /// <summary>
    /// Model za PonudaStavke generiran iz baze podataka
    /// Sadrži id stavke, id ponude , cijenu stavke te kolicinu 
    /// </summary>
    public partial class PonudaStavke
    {
        [Display(Name = "Id ponude", Prompt = "Unesite id ponude")]
        [Required(ErrorMessage = "Id ponude je obvezno polje!")]
        public int PonudaId { get; set; }
        [Display(Name = "Id stavke u troskovniku", Prompt = "Unesite Id stavke u troskovniku")]
        [Required(ErrorMessage = "Id stavke u troskovniku je obvezno polje!")]
        public int IdStavke { get; set; }
        [Display(Name = "Cijena stavke", Prompt = "Unesite cijenu stavke")]
        [Required(ErrorMessage = "Cijena stavke je obvezno polje!")]
        public decimal CijenaStavke { get; set; }
        [Display(Name = "Kolicina", Prompt = "Unesite kolicinu stavke")]
        [Required(ErrorMessage = "Kolicina stavke je obvezno polje!")]
        public int KoličinaStavke { get; set; }

        public virtual StavkaUTroškovniku IdStavkeNavigation { get; set; }
        public virtual Ponudum Ponuda { get; set; }
    }
}
