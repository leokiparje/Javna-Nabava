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
    ///Kontroler za dohvaćanje, uređivanje i brisanje naručitelja preko swaggera
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [TypeFilter(typeof(ProblemDetailsForSqlException))]
    public class ApiNaručiteljController : ControllerBase, ICustomController<string, JedanNaručiteljViewModel>
    {
        private readonly RPPP06Context ctx;
        private static Dictionary<string, Expression<Func<Naručitelj, object>>> orderSelectors = new Dictionary<string, Expression<Func<Naručitelj, object>>>
        {
            [nameof(JedanNaručiteljViewModel.oibNaručitelj).ToLower()] = p => p.OibNaručitelja,
            [nameof(JedanNaručiteljViewModel.nazivNaručitelj).ToLower()] = p => p.NazivNaručitelja,
            [nameof(JedanNaručiteljViewModel.adresaNaručitelj).ToLower()] = p => p.AdresaNaručitelja,
            [nameof(JedanNaručiteljViewModel.poštanskiBrojNaručitelj).ToLower()] = p => p.PoštanskiBrojNaručitelja
        };

        public ApiNaručiteljController(RPPP06Context ctx)
        {
            this.ctx = ctx;
        }

        [HttpGet("count", Name = "BrojNaručitelja")]
        public async Task<int> Count([FromQuery] string filter)
        {
            var query = ctx.Naručiteljs.AsQueryable();
            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(p => p.NazivNaručitelja.Contains(filter));
            }
            int count = await query.CountAsync();
            return count;
        }

        [HttpGet(Name = "Dohvati naručitelje")]
        public async Task<List<JedanNaručiteljViewModel>> GetAll([FromQuery] LoadParams loadParams)
        {
            var query = ctx.Naručiteljs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(loadParams.Filter))
            {
                query = query.Where(m => m.NazivNaručitelja.Contains(loadParams.Filter));
            }

            if (loadParams.SortColumn != null)
            {
                if (orderSelectors.TryGetValue(loadParams.SortColumn.ToLower(), out var expr))
                {
                    query = loadParams.Descending ? query.OrderByDescending(expr) : query.OrderBy(expr);
                }
            }

            var list = await query.Select(p => new JedanNaručiteljViewModel
            {
                oibNaručitelj = p.OibNaručitelja,
                nazivNaručitelj = p.NazivNaručitelja,
                adresaNaručitelj = p.AdresaNaručitelja,
                poštanskiBrojNaručitelj = p.PoštanskiBrojNaručitelja

            }).Skip(loadParams.StartIndex).Take(loadParams.Rows).ToListAsync();

            return list;
        }

        [HttpGet("{oib}", Name = "Dohvati naručitelja")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<JedanNaručiteljViewModel>> Get(string oib)
        {
            var naručitelj = await ctx.Naručiteljs
                                            .Where(p => p.OibNaručitelja.Equals(oib))
                                            .Select(p => new JedanNaručiteljViewModel
                                            {
                                                oibNaručitelj = p.OibNaručitelja,
                                                nazivNaručitelj = p.NazivNaručitelja,
                                                adresaNaručitelj = p.AdresaNaručitelja,
                                                poštanskiBrojNaručitelj = p.PoštanskiBrojNaručitelja
                                            }).FirstOrDefaultAsync();

            if (naručitelj == null)
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Nema podataka za oib = {oib}");
            }
            else
            {
                return naručitelj;
            }
        }

        [HttpDelete("{oib}", Name = "ObrisiNarucitelja")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string oib)
        {
            var narucitelj = await ctx.Naručiteljs.FindAsync(oib);
            if (narucitelj == null)
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Neispravni oib = {oib}");
            }
            else
            {
                ctx.Remove(narucitelj);
                await ctx.SaveChangesAsync();
                return NoContent();
            }
        }

        [HttpPut("{oib}", Name = "AzurirajNarucitelja")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(string oib, JedanNaručiteljViewModel model)
        {
            if (model.oibNaručitelj != oib)
            {
                return Problem(statusCode: StatusCodes.Status400BadRequest, detail: $"Different ids {oib} vs {model.oibNaručitelj}");
            }
            else
            {
                var narucitelj = await ctx.Naručiteljs.FindAsync(oib);
                if (narucitelj == null)
                {
                    return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Neispravni oib = {oib}");
                }

                narucitelj.OibNaručitelja = model.oibNaručitelj;
                narucitelj.NazivNaručitelja = model.nazivNaručitelj;
                narucitelj.AdresaNaručitelja = model.adresaNaručitelj;
                narucitelj.PoštanskiBrojNaručitelja = model.poštanskiBrojNaručitelj;

                await ctx.SaveChangesAsync();
                return NoContent();

            }
        }

        [HttpPost(Name = "DodajNaručitelja")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(JedanNaručiteljViewModel model)
        {
            Naručitelj narucitelj = new Naručitelj
            {
                OibNaručitelja = model.oibNaručitelj,
                NazivNaručitelja = model.nazivNaručitelj,
                AdresaNaručitelja = model.adresaNaručitelj,
                PoštanskiBrojNaručitelja = model.poštanskiBrojNaručitelj
            };

            ctx.Add(narucitelj);
            await ctx.SaveChangesAsync();

            var addedItem = await Get(narucitelj.OibNaručitelja);

            return CreatedAtAction(nameof(Get), new { oib = narucitelj.OibNaručitelja }, addedItem.Value);
        }
    }
}
