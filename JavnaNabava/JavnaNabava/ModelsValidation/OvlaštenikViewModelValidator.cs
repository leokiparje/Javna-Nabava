using FluentValidation;
using JavnaNabava.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.ModelsValidation
{
    /// <summary>
    /// Klasa OvlaštenikViewModelValidator u svom konstruktoru ima definirana pravila validacije s odgovarajućom porukom u slučaju neispravnosti.
    /// </summary>
    public class OvlaštenikViewModelValidator : AbstractValidator<OvlViewModel>
    {
        public OvlaštenikViewModelValidator()
        {
            RuleFor(o => o.OibOvlaštenik).NotEmpty().WithMessage("Potrebno je unijeti oib ovlaštenika");
            RuleFor(o => o.ImeOvlaštenik).NotEmpty().WithMessage("Potrebno je unijeti ime ovlaštenika");
            RuleFor(o => o.PrezimeOvlaštenik).NotEmpty().WithMessage("Potrebno je unijeti prezime ovlaštenika");
            RuleFor(o => o.OibNaručitelj).NotEmpty().WithMessage("Potrebno je unijeti oib naručitelja");
            RuleFor(o => o.IdPovjerenstva).NotEmpty().WithMessage("Potrebno je unijeti id povjerenstva");
        }
    }
}