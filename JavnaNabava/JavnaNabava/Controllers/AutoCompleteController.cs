using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using JavnaNabava.Models;
using JavnaNabava.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.Controllers
{
    public class AutoCompleteController : Controller
    {
        private readonly RPPP06Context ctx;
        private readonly AppSettings appData;

        public AutoCompleteController(RPPP06Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            appData = options.Value;
        }

        public async Task<IEnumerable<IdLabel>> Zapisnik(string term)
        {
            var query = ctx.Zapisniks
                            .Select(z => new IdLabel
                            {
                                Id = z.ZapisnikId,
                                Label = z.NazivZapisnik
                            })
                            .Where(l => l.Label.Contains(term));

            var list = await query.OrderBy(l => l.Label)
                                  .ThenBy(l => l.Id)
                                  .Take(appData.AutoCompleteCount)
                                  .ToListAsync();
            return list;
        }
        public async Task<IEnumerable<IdLabel>> Povjerenstvo(string term)
        {
            var query = ctx.Povjerenstvos
                            .Select(p => new IdLabel
                            {
                                Id = p.IdPovjerenstva,
                                Label = p.NazivPovjerenstva
                            })
                            .Where(l => l.Label.Contains(term));

            var list = await query.OrderBy(l => l.Label)
                                  .ThenBy(l => l.Id)
                                  .Take(appData.AutoCompleteCount)
                                  .ToListAsync();
            return list;
        }
        public async Task<IEnumerable<IdLabel>> Ponuda(string term)
        {
            var query = ctx.Ponuda
                            .Select(p => new IdLabel
                            {
                                Id = p.PonudaId,
                                Label = p.Text
                            })
                            .Where(l => l.Label.Contains(term));

            var list = await query.OrderBy(l => l.Label)
                                  .ThenBy(l => l.Id)
                                  .Take(appData.AutoCompleteCount)
                                  .ToListAsync();
            return list;
        }
        public async Task<IEnumerable<IdLabel>> Odredba(string term)
        {
            var query = ctx.OdredbaZapisniks
                           .Where(a => a.nazivOdredba.Contains(term))
                           .OrderBy(a => a.nazivOdredba)
                           .Select(a => new IdLabel
                           {
                               Id = a.idOdredba,
                               Label = a.nazivOdredba,
                           });

            var list = await query.OrderBy(l => l.Label)
                                  .ThenBy(l => l.Id)
                                  .Take(appData.AutoCompleteCount)
                                  .ToListAsync();
            return list;
        }
    }
}
