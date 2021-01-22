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
    public class ApiOvlaštenikController : ControllerBase, ICustomController<int, OvlViewModel>
    {
        private readonly RPPP06Context ctx;
        private static Dictionary<string, Expression<Func<Ovlaštenik, object>>> orderSelectors = new Dictionary<string, Expression<Func<Ovlaštenik, object>>>
        {
            [nameof(OvlViewModel.OibOvlaštenik).ToLower()] = p => p.OibOvlaštenik,
            [nameof(OvlViewModel.ImeOvlaštenik).ToLower()] = p => p.ImeOvlaštenik,
            [nameof(OvlViewModel.PrezimeOvlaštenik).ToLower()] = p => p.PrezimeOvlaštenik,
            [nameof(OvlViewModel.IdPovjerenstva).ToLower()] = p => p.IdPovjerenstva,
            [nameof(OvlViewModel.OibNaručitelj).ToLower()] = p => p.OibNaručitelja
        };

        public ApiOvlaštenikController(RPPP06Context ctx)
        {
            this.ctx = ctx;
        }

        [HttpGet("count", Name = "BrojOvlaštenika")]
        public async Task<int> Count([FromQuery] string filter)
        {
            var query = ctx.Ovlašteniks.AsQueryable();
            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(p => p.ImeOvlaštenik.Contains(filter));
            }
            int count = await query.CountAsync();
            return count;
        }

        [HttpGet(Name = "Dohvati ovlaštenike")]
        public async Task<List<OvlViewModel>> GetAll([FromQuery] LoadParams loadParams)
        {
            var query = ctx.Ovlašteniks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(loadParams.Filter))
            {
                query = query.Where(m => m.ImeOvlaštenik.Contains(loadParams.Filter));
            }

            if (loadParams.SortColumn != null)
            {
                if (orderSelectors.TryGetValue(loadParams.SortColumn.ToLower(), out var expr))
                {
                    query = loadParams.Descending ? query.OrderByDescending(expr) : query.OrderBy(expr);
                }
            }

            var list = await query.Select(p => new OvlViewModel
            {
                OibOvlaštenik = p.OibOvlaštenik,
                ImeOvlaštenik = p.ImeOvlaštenik,
                PrezimeOvlaštenik = p.PrezimeOvlaštenik,
                IdPovjerenstva = p.IdPovjerenstva,
                OibNaručitelj = p.OibNaručitelja

            }).Skip(loadParams.StartIndex).Take(loadParams.Rows).ToListAsync();

            return list;
        }

        [HttpGet("{oib}", Name = "DohvatiOvlaštenika")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OvlViewModel>> Get(int oib)
        {
            var ovlaštenik = await ctx.Ovlašteniks
                                            .Where(p => p.OibOvlaštenik.Equals(oib))
                                            .Select(p => new OvlViewModel
                                            {
                                                OibOvlaštenik = p.OibOvlaštenik,
                                                ImeOvlaštenik = p.ImeOvlaštenik,
                                                PrezimeOvlaštenik = p.PrezimeOvlaštenik,
                                                IdPovjerenstva = p.IdPovjerenstva,
                                                OibNaručitelj = p.OibNaručitelja
                                            }).FirstOrDefaultAsync();

            if (ovlaštenik == null)
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Nema podataka za oib = {oib}");
            }
            else
            {
                return ovlaštenik;
            }
        }

        [HttpDelete("{oib}", Name = "ObrisiOvlaštenika")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int oib)
        {
            var ovlaštenik = await ctx.Ovlašteniks.FindAsync(oib);
            if (ovlaštenik == null)
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Neispravni oib = {oib}");
            }
            else
            {
                ctx.Remove(ovlaštenik);
                await ctx.SaveChangesAsync();
                return NoContent();
            }
        }

        [HttpPut("{oib}", Name = "AzurirajOvlaštenika")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int oib, OvlViewModel model)
        {
            if (model.OibOvlaštenik != oib)
            {
                return Problem(statusCode: StatusCodes.Status400BadRequest, detail: $"Different ids {oib} vs {model.OibOvlaštenik}");
            }
            else
            {
                var ovlaštenik = await ctx.Ovlašteniks.FindAsync(oib);
                if (ovlaštenik == null)
                {
                    return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Neispravni oib = {oib}");
                }

                ovlaštenik.OibOvlaštenik = model.OibOvlaštenik;
                ovlaštenik.ImeOvlaštenik = model.ImeOvlaštenik;
                ovlaštenik.PrezimeOvlaštenik = model.PrezimeOvlaštenik;
                ovlaštenik.IdPovjerenstva = model.IdPovjerenstva;
                ovlaštenik.OibNaručitelja = model.OibNaručitelj;

                await ctx.SaveChangesAsync();
                return NoContent();

            }
        }

        [HttpPost(Name = "DodajOvlaštenika")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(OvlViewModel model)
        {
            Ovlaštenik ovlaštenik = new Ovlaštenik
            {
                OibOvlaštenik = model.OibOvlaštenik,
                ImeOvlaštenik = model.ImeOvlaštenik,
                PrezimeOvlaštenik = model.PrezimeOvlaštenik,
                IdPovjerenstva = model.IdPovjerenstva,
                OibNaručitelja = model.OibNaručitelj
            };

            ctx.Add(ovlaštenik);
            await ctx.SaveChangesAsync();

            var addedItem = await Get(ovlaštenik.OibOvlaštenik);

            return CreatedAtAction(nameof(Get), new { oib = ovlaštenik.OibOvlaštenik }, addedItem.Value);
        }
    }
}
