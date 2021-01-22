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
    [ApiController]
    [Route("api/[controller]")]
    [TypeFilter(typeof(ProblemDetailsForSqlException))]
    public class ApiPonuditeljController : ControllerBase, ICustomController<string, PonuditeljViewModel>
    {
        private readonly RPPP06Context ctx;
        private static Dictionary<string, Expression<Func<Ponuditelj, object>>> orderSelectors = new Dictionary<string, Expression<Func<Ponuditelj, object>>>
        {
            [nameof(PonuditeljViewModel.OibPonuditelj).ToLower()] = p => p.OibPonuditelj,
            [nameof(PonuditeljViewModel.NazivPonuditelj).ToLower()] = p => p.NazivPonuditelj,
            [nameof(PonuditeljViewModel.AdresaPonuditelj).ToLower()] = p => p.AdresaPonuditelj,
            [nameof(PonuditeljViewModel.SjedistePonuditelj).ToLower()] = p => p.SjedištePonuditelj
        };

        public ApiPonuditeljController(RPPP06Context ctx)
        {
            this.ctx = ctx;
        }

        [HttpGet("count", Name = "BrojPonuditelja")]
        public async Task<int> Count([FromQuery] string filter)
        {
            var query = ctx.Ponuditeljs.AsQueryable();
            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(p => p.NazivPonuditelj.Contains(filter));
            }
            int count = await query.CountAsync();
            return count;
        }

        [HttpGet(Name = "Dohvati ponuditelje")]
        public async Task<List<PonuditeljViewModel>> GetAll([FromQuery] LoadParams loadParams)
        {
            var query = ctx.Ponuditeljs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(loadParams.Filter))
            {
                query = query.Where(m => m.NazivPonuditelj.Contains(loadParams.Filter));
            }

            if (loadParams.SortColumn != null)
            {
                if (orderSelectors.TryGetValue(loadParams.SortColumn.ToLower(), out var expr))
                {
                    query = loadParams.Descending ? query.OrderByDescending(expr) : query.OrderBy(expr);
                }
            }

            var list = await query.Select(p => new PonuditeljViewModel
            {
                OibPonuditelj = p.OibPonuditelj,
                NazivPonuditelj = p.NazivPonuditelj,
                AdresaPonuditelj = p.AdresaPonuditelj,
                SjedistePonuditelj = p.SjedištePonuditelj

            }).Skip(loadParams.StartIndex).Take(loadParams.Rows).ToListAsync();

            return list;
        }

        [HttpGet("{oib}", Name = "DohvatiPonuditelja")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PonuditeljViewModel>> Get(string oib)
        {
            var ponuditelj = await ctx.Ponuditeljs
                                            .Where(p => p.OibPonuditelj.Equals(oib))
                                            .Select(p => new PonuditeljViewModel
                                            {
                                                OibPonuditelj = p.OibPonuditelj,
                                                NazivPonuditelj = p.NazivPonuditelj,
                                                AdresaPonuditelj = p.AdresaPonuditelj,
                                                SjedistePonuditelj = p.SjedištePonuditelj
                                            }).FirstOrDefaultAsync();

            if (ponuditelj == null)
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Nema podataka za oib = {oib}");
            }
            else
            {
                return ponuditelj;
            }
        }

        [HttpDelete("{oib}", Name = "ObrisiPonuditelja")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string oib)
        {
            var ponuditelj = await ctx.Ponuditeljs.FindAsync(oib);
            if(ponuditelj == null)
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Neispravni oib = {oib}");
            }
            else
            {
                ctx.Remove(ponuditelj);
                await ctx.SaveChangesAsync();
                return NoContent();
            }
        }

        [HttpPut("{oib}", Name = "AzurirajPonuditelja")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(string oib, PonuditeljViewModel model)
        {
            if(model.OibPonuditelj != oib)
            {
                return Problem(statusCode: StatusCodes.Status400BadRequest, detail: $"Different ids {oib} vs {model.OibPonuditelj}");
            }
            else
            {
                var ponuditelj = await ctx.Ponuditeljs.FindAsync(oib);
                if(ponuditelj == null)
                {
                    return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Neispravni oib = {oib}");
                }

                ponuditelj.OibPonuditelj = model.OibPonuditelj;
                ponuditelj.NazivPonuditelj = model.NazivPonuditelj;
                ponuditelj.AdresaPonuditelj = model.AdresaPonuditelj;
                ponuditelj.SjedištePonuditelj = model.SjedistePonuditelj;

                await ctx.SaveChangesAsync();
                return NoContent();
               
            }
        }

        [HttpPost(Name = "DodajPonuditelja")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(PonuditeljViewModel model)
        {
            Ponuditelj ponuditelj = new Ponuditelj
            {
                OibPonuditelj = model.OibPonuditelj,
                NazivPonuditelj = model.NazivPonuditelj,
                AdresaPonuditelj = model.AdresaPonuditelj,
                SjedištePonuditelj = model.SjedistePonuditelj
            };

            ctx.Add(ponuditelj);
            await ctx.SaveChangesAsync();

            var addedItem = await Get(ponuditelj.OibPonuditelj);

            return CreatedAtAction(nameof(Get), new { oib = ponuditelj.OibPonuditelj }, addedItem.Value);
        }
    }
}
