using FluentValidation;
using JavnaNabava.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.ModelsValidation
{
    public class PonuditeljViewModelValidator : AbstractValidator<PonuditeljViewModel>
    {
        public PonuditeljViewModelValidator()
        {
            RuleFor(p => p.OibPonuditelj).NotEmpty().WithMessage("Potrebno je unijeti oib ponuditelja").MinimumLength(11).MaximumLength(11);
            RuleFor(p => p.NazivPonuditelj).NotEmpty().WithMessage("Potrebno je unijeti naziv ponuditelja");
            RuleFor(p => p.AdresaPonuditelj).NotEmpty().WithMessage("Potrebno je unijeti adresu ponuditelja");
            RuleFor(p => p.SjedistePonuditelj).NotEmpty().WithMessage("Potrebno je unijeti sjedište ponuditelja");
        }
    }
}
