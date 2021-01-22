using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace JavnaNabava.Models
{
    public partial class Ponudum
    {
     /// <summary>
    /// Model za Ponudu generiran iz baze podataka
    /// Sadrži Id, Naslov, tekst ponude (o cemu je ponuda ), Id natječaja te OIB ponuditelja
    /// </summary>
        public Ponudum()
        {

            PonDokumentis = new HashSet<PonDokumenti>();
            PonudaNatječajs = new HashSet<PonudaNatječaj>();
            Zapisniks = new HashSet<Zapisnik>();
        }
        [Display(Name = "Ponuda id", Prompt = "Unesite id ponude")]
        [Required(ErrorMessage = "Id ponude je obvezno polje!")]
        public int PonudaId { get; set; }
        [Display(Name = "Natječaj", Prompt = "Odaberite natječaj")]
        [Required(ErrorMessage = "Natječaj je obvezno polje!")]
        public int EvidBrojNatječaj { get; set; }
        [Display(Name = "Tekst ponude", Prompt = "Unesite Tekst ponude")]
        [Required(ErrorMessage = "Tekst ponude je obvezno polje!")]
        [MaxLength(500, ErrorMessage = "Naslov dokumenta može sadržavati maksimalno 50 simbola")] 
        public string Text { get; set; }
        [Display(Name = "Naslov ponude", Prompt = "Unesite naslov ponude")]
        [Required(ErrorMessage = "Naslov ponude je obvezno polje!")]
        [MaxLength(50, ErrorMessage = "Naslov dokumenta može sadržavati maksimalno 50 simbola")]        
        public string Naslov { get; set; }

        [Display(Name = "Ponuditelj", Prompt = "Oznacite ponuditelja")]
        [Required(ErrorMessage = "Ponuditelj je obvezno polje!")]
        [MaxLength(11, ErrorMessage = "Oib mora sadržavati 11 znakova")]
        public string OibPonuditelj { get; set; }

        public virtual PonudaPonuditelj PonudaPonuditelj { get; set; }
        public virtual PonudaStavke PonudaStavke { get; set; }
        public virtual ICollection<PonDokumenti> PonDokumentis { get; set; }
        public virtual ICollection<PonudaNatječaj> PonudaNatječajs { get; set; }
        public virtual ICollection<Zapisnik> Zapisniks { get; set; }
    }
}
