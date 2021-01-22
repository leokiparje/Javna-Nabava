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
    ///Kontroler za dohvaćanje, uređivanje i brisanje dokumenata preko swaggera
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [TypeFilter(typeof(ProblemDetailsForSqlException))]
    public class ApiDokumentController : ControllerBase, ICustomController<int, DokumentaViewModel>
    {
        private readonly RPPP06Context ctx;
        private static Dictionary<string, Expression<Func<Dokument, object>>> orderSelectors = new Dictionary<string, Expression<Func<Dokument, object>>>
        {
            [nameof(DokumentaViewModel.DokumentId).ToLower()] = p => p.DokumentId,
            [nameof(DokumentaViewModel.Naslov).ToLower()] = p => p.Naslov,
            [nameof(DokumentaViewModel.Vrsta).ToLower()] = p => p.Vrsta,
            [nameof(DokumentaViewModel.Blob).ToLower()] = p => p.Blob,
            [nameof(DokumentaViewModel.PonudaId).ToLower()] = p => p.PonudaId,
            [nameof(DokumentaViewModel.DatumPredaje).ToLower()] = p => p.DatumPredaje
        };

        public ApiDokumentController(RPPP06Context ctx)
        {
            this.ctx = ctx;
        }

        [HttpGet("count", Name = "BrojDokumenta")]
        public async Task<int> Count([FromQuery] string filter)
        {
            var query = ctx.Dokuments.AsQueryable();
            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(p => p.Naslov.Contains(filter));
            }
            int count = await query.CountAsync();
            return count;
        }

        [HttpGet(Name = "DohvatiDokumente")]
        public async Task<List<DokumentaViewModel>> GetAll([FromQuery] LoadParams loadParams)
        {
            var query = ctx.Dokuments.AsQueryable();

            if (!string.IsNullOrWhiteSpace(loadParams.Filter))
            {
                query = query.Where(m => m.Naslov.Contains(loadParams.Filter));
            }

            if (loadParams.SortColumn != null)
            {
                if (orderSelectors.TryGetValue(loadParams.SortColumn.ToLower(), out var expr))
                {
                    query = loadParams.Descending ? query.OrderByDescending(expr) : query.OrderBy(expr);
                }
            }

            var list = await query.Select(p => new DokumentaViewModel
            {
                DokumentId = p.DokumentId,
                Naslov = p.Naslov,
                Vrsta = p.Vrsta,
                Blob = p.Blob,
                PonudaId = p.PonudaId,
                DatumPredaje = p.DatumPredaje

            }).Skip(loadParams.StartIndex).Take(loadParams.Rows).ToListAsync();

            return list;
        }

        [HttpGet("{id}", Name = "DohvatiDokument")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DokumentaViewModel>> Get(int id)
        {
            var dokument = await ctx.Dokuments
                                            .Where(p => p.DokumentId.Equals(id))
                                            .Select(p => new DokumentaViewModel
                                            {
                                                DokumentId = p.DokumentId,
                                                Naslov = p.Naslov,
                                                Vrsta = p.Vrsta,
                                                Blob = p.Blob,
                                                PonudaId = p.PonudaId,
                                                DatumPredaje = p.DatumPredaje
                                            }).FirstOrDefaultAsync();

            if (dokument == null)
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Nema podataka za id Dokumenta = {id}");
            }
            else
            {
                return dokument;
            }
        }

        [HttpDelete("{id}", Name = "ObrisiDokument")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var dokument = await ctx.Dokuments.FindAsync(id);
            if (dokument == null)
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Neispravni id = {id}");
            }
            else
            {
                ctx.Remove(dokument);
                await ctx.SaveChangesAsync();
                return NoContent();
            }
        }

        [HttpPut("{id}", Name = "AzurirajDokument")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, DokumentaViewModel model)
        {
            if (model.DokumentId != id)
            {
                return Problem(statusCode: StatusCodes.Status400BadRequest, detail: $"Different ids {id} vs {model.DokumentId}");
            }
            else
            {
                var dokument = await ctx.Dokuments.FindAsync(id);
                if (dokument == null)
                {
                    return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Neispravni id = {id}");
                }

                dokument.DokumentId = model.DokumentId;
                dokument.Naslov = model.Naslov;
                dokument.Vrsta = model.Vrsta;
                dokument.Blob = model.Blob;
                dokument.PonudaId = model.PonudaId;
                dokument.DatumPredaje = model.DatumPredaje;

                await ctx.SaveChangesAsync();
                return NoContent();

            }
        }

        [HttpPost(Name = "DodajDokument")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(DokumentaViewModel model)
        {
            Dokument dokument = new Dokument
            {
                DokumentId = model.DokumentId,
                Naslov = model.Naslov,
                Vrsta = model.Vrsta,
                Blob = model.Blob,
                PonudaId = model.PonudaId,
                DatumPredaje = model.DatumPredaje
            };

            ctx.Add(dokument);
            await ctx.SaveChangesAsync();

            var addedItem = await Get(dokument.DokumentId);

            return CreatedAtAction(nameof(Get), new { id = dokument.DokumentId }, addedItem.Value);
        }
    }
}
