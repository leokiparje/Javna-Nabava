using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using JavnaNabava.Extensions;
using JavnaNabava.Extensions.Selectors;
using JavnaNabava.Models;
using JavnaNabava.ViewModels;
using JavnaNabava;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.Controllers
{
    /// <summary>
    /// Kontroler za PonudaStavke 
    /// </summary>
  public class ponudaStavkeController : Controller
  {
    private readonly RPPP06Context ctx;
    private readonly AppSettings appData;
   
    public ponudaStavkeController(RPPP06Context ctx, IOptionsSnapshot<AppSettings> options)
    {
      this.ctx = ctx;
      appData = options.Value;
    }

    public async Task<IActionResult> Index(int page = 1, int sort = 1, bool ascending = true)
    {
      int pagesize = appData.PageSize;

      var query = ctx.PonudaStavkes.AsNoTracking();
      int count = await query.CountAsync();

      var pagingInfo = new PagingInfo
      {
        CurrentPage = page,
        Sort = sort,
        Ascending = ascending,
        ItemsPerPage = pagesize,
        TotalItems = count
      };
      if (page < 1 || page > pagingInfo.TotalPages)
      {
        return RedirectToAction(nameof(Index), new { page = 1, sort = sort, ascending = ascending });
      }

      query = query.ApplySort(sort, ascending);
     
      var dok = await query
                          .Select(m => new PonudaStavkeViewModel
                          {
                            PonudaId = m.Ponuda.PonudaId,
                            NazivStavke = m.IdStavkeNavigation.NazivStavke,
                            CijenaStavke = m.CijenaStavke,
                            KoličinaStavke = m.KoličinaStavke   
                          })
                          .Skip((page - 1) * pagesize)
                          .Take(pagesize)
                          .ToListAsync();
      var model = new PonudeStavkeViewModel
      {
        PonudeStavke = dok,
        PagingInfo = pagingInfo
      };

      return View(model);
    }   

    [HttpGet]
    public async Task<IActionResult> Create()
    {
      await PrepareDropDownLists();
      return View();
    }
        /// <summary>
        /// Metoda za dodavanje nove ponudastavke
        /// </summary>
        /// <param name="ponudastavke">ponudastavke koji se dodaje</param>
        /// <returns>Vraća novu ponudastavke</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PonudaStavke ponudastavke)
    {
      if (ModelState.IsValid)
      {
        try
        {
          ctx.Add(ponudastavke);
          await ctx.SaveChangesAsync();

          TempData[Constants.Message] = $"dokument je dodan. Id mjesta = {ponudastavke.PonudaId} Id stavke = {ponudastavke.IdStavke}";
          TempData[Constants.ErrorOccurred] = false;
          return RedirectToAction(nameof(Index));

        }
        catch (Exception exc)
        {
          ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
          await PrepareDropDownLists();
          return View(ponudastavke);
        }
      }
      else
      {
        await PrepareDropDownLists();
        return View(ponudastavke);
      }
    }
        /// <summary>
        /// Metoda za brisanje PonudeStavke
        /// </summary>
        /// <param name="id">Identifikator ponude koji se uređuje (id)</param>
        /// <param name="idS">Identifikator stavke koji se uređuje (id)</param>
        /// <param name="page">Broj stranice</param>
        /// <param name="sort">Atribut po kojemu se sortira</param>
        /// <param name="ascending">Oznaka je li sortiranje uzlazno ili silazno</param>
        /// <returns>Vraća pogled na index stranicu</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int idS,int page = 1, int sort = 1, bool ascending = true)
    {
      var ponudastavke = await ctx.PonudaStavkes.FindAsync(id);                       
      if (ponudastavke != null)
      {
        try
        {
          
          ctx.Remove(ponudastavke);          
          await ctx.SaveChangesAsync();
          TempData[Constants.Message] = $"PonudaStavka sa šifrom {id} je obrisana.";
          TempData[Constants.ErrorOccurred] = false;        
        }
        catch (Exception exc)
        {
          TempData[Constants.Message] = "Pogreška prilikom brisanja PonudeStavke: " + exc.CompleteExceptionMessage();
          TempData[Constants.ErrorOccurred] = true;         
        }
      }
      else
      {
        TempData[Constants.Message] = $"Ne postoji PonudaStavka sa šifrom: {id}";
        TempData[Constants.ErrorOccurred] = true;
      }
      return RedirectToAction(nameof(Index), new { page, sort, ascending });
    }

    private async Task PrepareDropDownLists()
    {

      var stavkautroškovniku = await ctx.StavkaUTroškovnikus                      
                            .OrderBy(d => d.NazivStavke)
                            .Select(d => new { d.NazivStavke, d.IdStavke })
                            .ToListAsync();
        
      ViewBag.StavkaUTroškovniku = new SelectList(stavkautroškovniku, nameof(StavkaUTroškovniku.IdStavke), nameof(StavkaUTroškovniku.NazivStavke));

      var ponuda = await ctx.Ponuda                      
                            .OrderBy(d => d.Naslov)
                            .Select(d => new { d.Naslov, d.PonudaId })
                            .ToListAsync();
        
      ViewBag.Ponuda = new SelectList(ponuda, nameof(Ponudum.PonudaId), nameof(Ponudum.Naslov));
    }

        /// <summary>
        /// Metoda za uređivanje PonudeStavke
        /// </summary>
        /// <param name="id">Identifikator ponude koji se uređuje</param>
        /// <param name="page">Broj stranice</param>
        /// <param name="sort">Atribut po kojemu se sortira</param>
        /// <param name="ascending">Oznaka je li sortiranje uzlazno ili silazno</param>
    [HttpGet]
    public async Task<IActionResult> Edit(int id, int page = 1, int sort = 1, bool ascending = true)
    {
      var ponudastavke = await ctx.PonudaStavkes
                            .AsNoTracking()
                            .Where(m => m.PonudaId == id)
                            .SingleOrDefaultAsync();
      if (ponudastavke != null)
      {
        ViewBag.Page = page;
        ViewBag.Sort = sort;
        ViewBag.Ascending = ascending;
        await PrepareDropDownLists();
        return View(ponudastavke);
      }
      else
      {
        return NotFound($"Neispravan id ponude: {id}");
      }
    }

    [HttpPost ,ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(PonudaStavke ponudastavke, int page = 1, int sort = 1, bool ascending = true)
    {
      if (ponudastavke == null)
      {
        return NotFound("Nema poslanih ponuda");
      }
      bool checkId = await ctx.PonudaStavkes.AnyAsync(m => m.PonudaId == ponudastavke.PonudaId);
      if (!checkId)
      {
        return NotFound($"Neispravan id: {ponudastavke?.PonudaId}");
      }
      
      if (ModelState.IsValid)
      {
        try
        {
          ctx.Update(ponudastavke);
          await ctx.SaveChangesAsync();
          TempData[Constants.Message] = "PonudaStavke ažurirana.";
          TempData[Constants.ErrorOccurred] = false;
          return RedirectToAction(nameof(Index), new { page, sort, ascending });
        }
        catch (Exception exc)
        {
          ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
          await PrepareDropDownLists();
          return View(ponudastavke);
        }
      }
      else
      {
        await PrepareDropDownLists();
        return View(ponudastavke);
      }
    }
  }
}
