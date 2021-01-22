using FluentValidation;
using JavnaNabava.ViewModels;

namespace JavnaNabava.ModelsValidation
{
  public class ZaposlenikViewModelValidator : AbstractValidator<ZaposlenikViewModel>
  {
    public ZaposlenikViewModelValidator()
    {
      RuleFor(p => p.oibZaposlenik).NotEmpty().WithMessage("Potrebno je unijeti oib").MinimumLength(11).MaximumLength(11);
      RuleFor(p => p.ImeZaposlenik).NotEmpty().WithMessage("Potrebno je unijeti ime");
      RuleFor(p => p.PrezimeZaposlenik).NotEmpty().WithMessage("Potrebno je unijeti prezime");
      RuleFor(p => p.MjestoPrebivališta).NotEmpty().WithMessage("Potrebno je unijeti adresu");
      RuleFor(p => p.OibPonuditelj).NotEmpty().WithMessage("Potrebno je odabrati ponuditelja");
    }
  }
}
