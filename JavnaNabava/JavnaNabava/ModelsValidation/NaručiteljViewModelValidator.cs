using FluentValidation;
using JavnaNabava.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.ModelsValidation
{
    public class NaručiteljViewModelValidator : AbstractValidator<NaručiteljaViewModel>
    {
        public NaručiteljViewModelValidator()
        {
            RuleFor(p => p.oibNaručitelj).NotEmpty().WithMessage("Potrebno je unijeti oib naručitelja").MinimumLength(11).MaximumLength(11);
            RuleFor(p => p.nazivNaručitelj).NotEmpty().WithMessage("Potrebno je unijeti naziv naručitelja");
            RuleFor(p => p.adresaNaručitelj).NotEmpty().WithMessage("Potrebno je unijeti adresu naručitelja");
            RuleFor(p => p.poštanskiBrojNaručitelj).NotEmpty().WithMessage("Potrebno je unijeti poštanski broj naručitelja");
        }
    }
}
