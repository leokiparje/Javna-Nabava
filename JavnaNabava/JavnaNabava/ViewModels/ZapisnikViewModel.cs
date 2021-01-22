using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;

namespace JavnaNabava.ViewModels
{
    /// <summary>
    /// Pogled ZapisnikViewModel predstavlja pogled jednog zapisnika s relevantnim atributima.
    /// </summary>
    public class ZapisnikViewModel
    {
        public ZapisnikViewModel()
        {
            this.StavkeZapisnika = new List<StavkaZapisnikViewModel>();
        }
        [Display(Name = "Naziv zapisnika")]
        public string NazivZapisnik { get; set; }
        [Display(Name = "Zapisnik")]
        public int ZapisnikId { get; set; }
        [Display(Name = "Povjerenstvo")]
        public int IdPovjerenstva { get; set; }
        [Display(Name = "Naziv povjerenstva")]
        public string NazivPovjerenstva { get; set; }
        [Display(Name = "Ponuda")]
        public int IdPonude { get; set; }
        [Display(Name = "Naziv ponude")]
        public string TextPonude { get; set; }
        [Display(Name ="Valjanost zapisnika")]
        public string ispravnostZapisnik { get; set; }
        [Display(Name = "Prethodni zapisnik")]
        public int? IdPrethZapisnika { get; set; }
        public string NazPrethodnogZapisnika { get; set; }
        public IEnumerable<StavkaZapisnikViewModel> StavkeZapisnika { get; set; }
     
    }
}
