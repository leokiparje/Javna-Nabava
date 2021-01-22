using FluentValidation;
using JavnaNabava.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.ModelsValidation
{
    public class KompetencijeViewModelValidator : AbstractValidator<KompetencijaViewModel>
    {
        public KompetencijeViewModelValidator()
        {
            RuleFor(p => p.IdKompetencije).NotEmpty().WithMessage("Potrebno je unijeti id kompetencije");
            RuleFor(p => p.NazivKompetencije).NotEmpty().WithMessage("Potrebno je unijeti naziv kompetencije");
           
        }
    }
}
