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
    ///Kontroler za dohvaćanje, uređivanje i brisanje Vrste stavki preko swaggera
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [TypeFilter(typeof(ProblemDetailsForSqlException))]
    public class ApiVrstaStavkiController : ControllerBase, ICustomController<int, VrstaStavkeViewModel>
    {
        private readonly RPPP06Context ctx;
        private static Dictionary<string, Expression<Func<VrstaStavke, object>>> orderSelectors = new Dictionary<string, Expression<Func<VrstaStavke, object>>>
        {
            [nameof(VrstaStavkeViewModel.IdVrste).ToLower()] = p => p.IdVrste,
            [nameof(VrstaStavkeViewModel.NazivVrste).ToLower()] = p => p.NazivVrste
        };

        public ApiVrstaStavkiController(RPPP06Context ctx)
        {
            this.ctx = ctx;
        }

        [HttpGet("count", Name = "BrojVrsti")]
        public async Task<int> Count([FromQuery] string filter)
        {
            var query = ctx.VrstaStavkes.AsQueryable();
            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(p => p.NazivVrste.Contains(filter));
            }
            int count = await query.CountAsync();
            return count;
        }

        [HttpGet(Name = "Dohvati VrsteStavki")]
        public async Task<List<VrstaStavkeViewModel>> GetAll([FromQuery] LoadParams loadParams)
        {
            var query = ctx.VrstaStavkes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(loadParams.Filter))
            {
                query = query.Where(m => m.NazivVrste.Contains(loadParams.Filter));
            }

            if (loadParams.SortColumn != null)
            {
                if (orderSelectors.TryGetValue(loadParams.SortColumn.ToLower(), out var expr))
                {
                    query = loadParams.Descending ? query.OrderByDescending(expr) : query.OrderBy(expr);
                }
            }

            var list = await query.Select(p => new VrstaStavkeViewModel
            {
                IdVrste = p.IdVrste,
                NazivVrste = p.NazivVrste

            }).Skip(loadParams.StartIndex).Take(loadParams.Rows).ToListAsync();

            return list;
        }

        [HttpGet("{id}", Name = "Dohvati VrstuStavke")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VrstaStavkeViewModel>> Get(int id)
        {
            var vrstaStavke = await ctx.VrstaStavkes
                                            .Where(p => p.IdVrste.Equals(id))
                                            .Select(p => new VrstaStavkeViewModel
                                            {
                                                IdVrste = p.IdVrste,
                                                NazivVrste = p.NazivVrste
                                            }).FirstOrDefaultAsync();

            if (vrstaStavke == null)
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Nema podataka za idVrste = {id}");
            }
            else
            {
                return vrstaStavke;
            }
        }

        [HttpDelete("{id}", Name = "ObrisiVrstuStavke")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var vrstaStavke = await ctx.VrstaStavkes.FindAsync(id);
            if (vrstaStavke == null)
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Neispravni id = {id}");
            }
            else
            {
                ctx.Remove(vrstaStavke);
                await ctx.SaveChangesAsync();
                return NoContent();
            }
        }

        [HttpPut("{id}", Name = "AzurirajVrstuStavke")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, VrstaStavkeViewModel model)
        {
            if (model.IdVrste != id)
            {
                return Problem(statusCode: StatusCodes.Status400BadRequest, detail: $"Different ids {id} vs {model.IdVrste}");
            }
            else
            {
                var vrstaStavke = await ctx.VrstaStavkes.FindAsync(id);
                if (vrstaStavke == null)
                {
                    return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Neispravni id = {id}");
                }

                vrstaStavke.IdVrste = model.IdVrste;
                vrstaStavke.NazivVrste = model.NazivVrste;

                await ctx.SaveChangesAsync();
                return NoContent();

            }
        }

        [HttpPost(Name = "DodajVrstuStavke")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(VrstaStavkeViewModel model)
        {
            VrstaStavke vrstaStavke = new VrstaStavke
            {
                IdVrste = model.IdVrste,
                NazivVrste = model.NazivVrste
            };

            ctx.Add(vrstaStavke);
            await ctx.SaveChangesAsync();

            var addedItem = await Get(vrstaStavke.IdVrste);

            return CreatedAtAction(nameof(Get), new { id = vrstaStavke.IdVrste }, addedItem.Value);
        }
    }
}
