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
    /// Kontroler za Dokumente 
    /// </summary>
  public class dokumentController : Controller
  {
    private readonly RPPP06Context ctx;
    private readonly AppSettings appData;
   
    public dokumentController(RPPP06Context ctx, IOptionsSnapshot<AppSettings> options)
    {
      this.ctx = ctx;
      appData = options.Value;
    }
        /// <summary>
        /// Prikazuje početnu stranicu
        /// </summary>
        /// <param name="page">Broj stranice</param>
        /// <param name="sort">Atribut po kojemu se sortira</param>
        /// <param name="ascending">Oznaka je li sortiranje uzlazno ili silazno</param>
        /// <returns></returns>
    public async Task<IActionResult> Index(int page = 1, int sort = 1, bool ascending = true)
    {
      int pagesize = appData.PageSize;

      var query = ctx.Dokuments.AsNoTracking();
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
                          
                          .Select(m => new DokumentViewModel
                          
                          {
                            DokumentId = m.DokumentId,
                            Naslov = m.Naslov,
                            Vrsta = m.Vrsta,
                            Blob = m.Blob,
                            NazivPonude = m.PonDokumentis.Where(k => k.DokumentId == m.DokumentId).First().Ponuda.Naslov,
                            DatumPredaje= m.DatumPredaje
                          })
                          .Skip((page - 1) * pagesize)
                          .Take(pagesize)
                          .ToListAsync();
      var model = new DokumentiViewModel
      {
        Dokumenti = dok,
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
        /// Metoda za stvaranje novog dokumenta 
        /// </summary>
   
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Dokument dokument)
    { 
      var query = ctx;
      int zadnji = query.Dokuments.OrderBy(d =>d.DokumentId).Last().DokumentId;
      dokument.DokumentId = zadnji+1;
      if (ModelState.IsValid)
      {var idemo = new PonDokumenti();
            idemo.PonudaId = dokument.PonudaId;
            idemo.DokumentId = dokument.DokumentId;
        try
        { ctx.Add(idemo);
          ctx.Add(dokument);
          await ctx.SaveChangesAsync();

          TempData[Constants.Message] = $"dokument {dokument.Naslov} dodano. Id mjesta = {dokument.DokumentId}";
          TempData[Constants.ErrorOccurred] = false;
          return RedirectToAction(nameof(Index));

        }
        catch (Exception exc)
        {
          ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
          await PrepareDropDownLists();
          return View(dokument);
        }
      }
      else
      {
        await PrepareDropDownLists();
        return View(dokument);
      }
    }
        /// <summary>
        /// Metoda za brisanje dokumenata 
        /// </summary>
        /// <param name="id">Identifikator dokumenta koji brise (id)</param>
        /// <param name="page">Broj stranice</param>
        /// <param name="sort">Atribut po kojemu se sortira</param>
        /// <param name="ascending">Oznaka je li sortiranje uzlazno ili silazno</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int page = 1, int sort = 1, bool ascending = true)
    {
      var dokument = await ctx.Dokuments.FindAsync(id);                       
      if (dokument != null)
      {var idemo = new PonDokumenti();
            idemo.PonudaId = dokument.PonudaId;
            idemo.DokumentId = dokument.DokumentId;
        try
        {
          string naziv = dokument.Naslov;
          ctx.Remove(idemo);
          ctx.Remove(dokument);          
          await ctx.SaveChangesAsync();
          TempData[Constants.Message] = $"dokument {naziv} sa šifrom {id} obrisano.";
          TempData[Constants.ErrorOccurred] = false;        
        }
        catch (Exception exc)
        {
          TempData[Constants.Message] = "Pogreška prilikom brisanja dokumenta: " + exc.CompleteExceptionMessage();
          TempData[Constants.ErrorOccurred] = true;         
        }
      }
      else
      {
        TempData[Constants.Message] = $"Ne postoji dokument sa šifrom: {id}";
        TempData[Constants.ErrorOccurred] = true;
      }
      return RedirectToAction(nameof(Index), new { page, sort, ascending });
    }

    private async Task PrepareDropDownLists()
    {

      var ponude = await ctx.Ponuda                      
                            .OrderBy(d => d.Naslov)
                            .Select(d => new { d.Naslov, d.PonudaId })
                            .ToListAsync();
        
      ViewBag.Ponude = new SelectList(ponude, nameof(Ponudum.PonudaId), nameof(Ponudum.Naslov));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, int page = 1, int sort = 1, bool ascending = true)
    {
      var dokument = await ctx.Dokuments
                            .AsNoTracking()
                            .Where(m => m.DokumentId == id)
                            .SingleOrDefaultAsync();
      if (dokument != null)
      {
        ViewBag.Page = page;
        ViewBag.Sort = sort;
        ViewBag.Ascending = ascending;
        await PrepareDropDownLists();
        return View(dokument);
      }
      else
      {
        return NotFound($"Neispravan id mjesta: {id}");
      }
    }

        /// <summary>
        /// Metoda za uređivanje dokumenta
        /// </summary>
        /// <param name="dokument">Identifikator dokumenta koji se uređuje (id)</param>
        /// <param name="page">Broj stranice</param>
        /// <param name="sort">Atribut po kojemu se sortira</param>
        /// <param name="ascending">Oznaka je li sortiranje uzlazno ili silazno</param>
    [HttpPost ,ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Dokument dokument, int page = 1, int sort = 1, bool ascending = true)
    {
      if (dokument == null)
      {
        return NotFound("Nema poslanih podataka");
      }
      bool checkId = await ctx.Dokuments.AnyAsync(m => m.DokumentId == dokument.DokumentId);
      if (!checkId)
      {
        return NotFound($"Neispravan id mjesta: {dokument?.DokumentId}");
      }
      
      if (ModelState.IsValid)
      {
      var idemo = new PonDokumenti();
            idemo.PonudaId = dokument.PonudaId;
            idemo.DokumentId = dokument.DokumentId;
  
        try
        {
          ctx.Update(idemo);
          ctx.Update(dokument);
          await ctx.SaveChangesAsync();
          TempData[Constants.Message] = "dokument ažurirano.";
          TempData[Constants.ErrorOccurred] = false;
          return RedirectToAction(nameof(Index), new { page, sort, ascending });
        }
        catch (Exception exc)
        {
          ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
          await PrepareDropDownLists();
          return View(dokument);
        }
      }
      else
      {
        await PrepareDropDownLists();
        return View(dokument);
      }
    }

  }
}
