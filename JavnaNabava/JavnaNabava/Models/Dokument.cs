using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace JavnaNabava.Models
{
    /// <summary>
    /// Model za dokument generiran iz baze podataka
    /// Sadrži id dokumenta ,naslov , vrstu dokumenta , link na dokument , id ponude  te datum predaje
    /// </summary>
    public partial class Dokument
    {
        public Dokument()
        {
            PonDokumentis = new HashSet<PonDokumenti>();
        }
        [Display(Name = "Dokument id", Prompt = "Unesite id")]
        [Required(ErrorMessage = "DokumentId je obvezno polje!")]
        public int DokumentId { get; set; }
        [Display(Name = "Naslov dokumenta", Prompt = "Unesite naslov dokumenta")]
        [Required(ErrorMessage = "Naslov dokumenta je obvezno polje!")]
        [MaxLength(10, ErrorMessage = "Naslov dokumenta može sadržavati maksimalno 10 simbola")]
        public string Naslov { get; set; }
        [Display(Name = "Vrsta dokumenta", Prompt = "Unesite vrstu dokumenta")]
        [Required(ErrorMessage = "Vrsta dokumenta je obvezno polje!")]
        [MaxLength(10, ErrorMessage = "Vrsta dokumenta može sadržavati maksimalno 10 simbola")]
        public string Vrsta { get; set; }
        [Display(Name = "Link", Prompt = "Unesite link na file")]
        [Required(ErrorMessage = "Link je obvezno polje!")]
        public string Blob { get; set; }
        [Display(Name = "Naslov ponude", Prompt = "Odaberite ponudu")]
        [Required(ErrorMessage = "Naslov ponude je obvezno polje!")]
        public int PonudaId { get; set; }
        [Display(Name = "Datum predaje", Prompt = "Unesite datum predaje")]
        [Required(ErrorMessage = "Datum predaje je obvezno polje!")]
        public DateTime DatumPredaje { get; set; }
        


        public virtual ICollection<PonDokumenti> PonDokumentis { get; set; }

    }
}
