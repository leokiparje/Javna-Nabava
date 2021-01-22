using JavnaNabava.Models;
using JavnaNabava.Util.ExceptionFilters;
using JavnaNabava.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JavnaNabava.Controllers
{
    /// <summary>
    ///Kontroler za dohvaćanje, uređivanje i brisanje vrsta kompetencija preko swaggera
    /// </summary>

    [ApiController]
    [Route("api/[controller]")]
    [TypeFilter(typeof(ProblemDetailsForSqlException))]
    public class ApiKompetencijeController : ControllerBase, ICustomController<int, KompetencijaViewModel>
    {
        private readonly RPPP06Context ctx;
        private static Dictionary<string, Expression<Func<VrstaKompetencije, object>>> orderSelectors = new Dictionary<string, Expression<Func<VrstaKompetencije, object>>>
        {
            [nameof(KompetencijaViewModel.IdKompetencije).ToLower()] = p => p.IdKompetencije,
            [nameof(KompetencijaViewModel.NazivKompetencije).ToLower()] = p => p.NazivKompetencije,
            [nameof(KompetencijaViewModel.SjedišteObrazovneUstanove).ToLower()] = p => p.SjedišteObrazovneUstanove,
            [nameof(KompetencijaViewModel.PočetakObrazovanja).ToLower()] = p => p.PočetakObrazovanja,
            [nameof(KompetencijaViewModel.KrajObrazovanja).ToLower()] = p => p.KrajObrazovanja,

        };

        public ApiKompetencijeController(RPPP06Context ctx)
        {
            this.ctx = ctx;
        }

        [HttpGet("count", Name = "BrojKompetencija")]
        public async Task<int> Count([FromQuery] string filter)
        {
            var query = ctx.VrstaKompetencijes.AsQueryable();
            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(p => p.NazivKompetencije.Contains(filter));
            }
            int count = await query.CountAsync();
            return count;
        }

        [HttpGet(Name = "Dohvati VrsteKompetencija")]
        public async Task<List<KompetencijaViewModel>> GetAll([FromQuery] LoadParams loadParams)
        {
            var query = ctx.VrstaKompetencijes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(loadParams.Filter))
            {
                query = query.Where(m => m.NazivKompetencije.Contains(loadParams.Filter));
            }

            if (loadParams.SortColumn != null)
            {
                if (orderSelectors.TryGetValue(loadParams.SortColumn.ToLower(), out var expr))
                {
                    query = loadParams.Descending ? query.OrderByDescending(expr) : query.OrderBy(expr);
                }
            }

            var list = await query.Select(p => new KompetencijaViewModel
            {
                IdKompetencije = p.IdKompetencije,
                NazivKompetencije = p.NazivKompetencije,
                SjedišteObrazovneUstanove = p.SjedišteObrazovneUstanove,
                PočetakObrazovanja = p.PočetakObrazovanja,
                KrajObrazovanja = p.KrajObrazovanja

            }).Skip(loadParams.StartIndex).Take(loadParams.Rows).ToListAsync();

            return list;
        }

        [HttpGet("{id}", Name = "Dohvati kompetenciju")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<KompetencijaViewModel>> Get(int id)
        {
            var kompetencija = await ctx.VrstaKompetencijes
                                            .Where(p => p.IdKompetencije.Equals(id))
                                            .Select(p => new KompetencijaViewModel
                                            {
                                                IdKompetencije = p.IdKompetencije,
                                                NazivKompetencije = p.NazivKompetencije,
                                                SjedišteObrazovneUstanove = p.SjedišteObrazovneUstanove,
                                                PočetakObrazovanja = p.PočetakObrazovanja,
                                                KrajObrazovanja = p.KrajObrazovanja
                                            }).FirstOrDefaultAsync();

            if (kompetencija == null)
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Nema podataka za id = {id}");
            }
            else
            {
                return kompetencija;
            }
        }

        [HttpDelete("{id}", Name = "ObrisiKompetenciju")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var kompetencija = await ctx.VrstaKompetencijes.FindAsync(id);
            if (kompetencija == null)
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Neispravni id = {id}");
            }
            else
            {
                ctx.Remove(kompetencija);
                await ctx.SaveChangesAsync();
                return NoContent();
            }
        }

        [HttpPut("{id}", Name = "AzurirajKompetenciju")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, KompetencijaViewModel model)
        {
            if (model.IdKompetencije != id)
            {
                return Problem(statusCode: StatusCodes.Status400BadRequest, detail: $"Different ids {id} vs {model.IdKompetencije}");
            }
            else
            {
                var kompetencije = await ctx.VrstaKompetencijes.FindAsync(id);
                if (kompetencije == null)
                {
                    return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Neispravni id = {id}");
                }

                kompetencije.IdKompetencije = model.IdKompetencije;
                kompetencije.NazivKompetencije = model.NazivKompetencije;
                kompetencije.SjedišteObrazovneUstanove = model.SjedišteObrazovneUstanove;
                kompetencije.PočetakObrazovanja = model.PočetakObrazovanja;
                kompetencije.KrajObrazovanja = model.KrajObrazovanja;

                await ctx.SaveChangesAsync();
                return NoContent();

            }
        }

        [HttpPost(Name = "DodajKompetenciju")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(KompetencijaViewModel model)
        {
            VrstaKompetencije kompetencija = new VrstaKompetencije
            {
                IdKompetencije = model.IdKompetencije,
            NazivKompetencije = model.NazivKompetencije,
           SjedišteObrazovneUstanove = model.SjedišteObrazovneUstanove,
           PočetakObrazovanja = model.PočetakObrazovanja,
            KrajObrazovanja = model.KrajObrazovanja
        };

            ctx.Add(kompetencija);
            await ctx.SaveChangesAsync();

            var addedItem = await Get(kompetencija.IdKompetencije);

            return CreatedAtAction(nameof(Get), new { id = kompetencija.IdKompetencije }, addedItem.Value);
        }
    }
}
