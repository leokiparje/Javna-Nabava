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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.Controllers
{
  public class ponudaController : Controller
  {
    /// <summary>
    /// Kontroler za Ponudu 
    /// </summary>
    private readonly RPPP06Context ctx;
    private readonly AppSettings appData;
   
    public ponudaController(RPPP06Context ctx, IOptionsSnapshot<AppSettings> options)
    {
      this.ctx = ctx;
      appData = options.Value;
    }

        /// <summary>
        /// Ucitavanje index stranice 
        /// </summary>
        /// <param name="page">Broj stranice</param>
        /// <param name="sort">Atribut po kojemu se sortira</param>
        /// <param name="ascending">Oznaka je li sortiranje uzlazno ili silazno</param>
    public async Task<IActionResult> Index(int page = 1, int sort = 1, bool ascending = true)
    {
      int pagesize = appData.PageSize;

      var query = ctx.Ponuda.AsNoTracking();
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
                          .Select(m => new PonudaViewModel
                          {
                            PonudaId = m.PonudaId,
                            NazivNatječaja = m.PonudaNatječajs.Where(k => k.PonudaId == m.PonudaId).First().EvidBrojNatječajNavigation.NazivNatječaja,
                            Text = m.Text,
                            Naslov = m.Naslov,
                            NazivPonuditelj = m.PonudaPonuditelj.OibPonuditeljNavigation.NazivPonuditelj
                            
                          })
                          .Skip((page - 1) * pagesize)
                          .Take(pagesize)
                          .ToListAsync();
      var model = new PonudeViewModel
      {
        Ponude = dok,
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Ponudum ponuda)
    { var query = ctx;
      int zadnji = query.Ponuda.OrderBy(d =>d.PonudaId).Last().PonudaId;
      ponuda.PonudaId = zadnji+1;
      if (ModelState.IsValid)
      {var idemo = new PonudaNatječaj();
            idemo.PonudaId=ponuda.PonudaId;
            idemo.EvidBrojNatječaj = ponuda.EvidBrojNatječaj;
            var idemo2 = new PonudaPonuditelj();
            idemo2.PonudaId=ponuda.PonudaId;
            idemo2.OibPonuditelj = ponuda.OibPonuditelj;
        try
        { ctx.Add(idemo);
        ctx.Add(idemo2);
          ctx.Add(ponuda);
          await ctx.SaveChangesAsync();

          TempData[Constants.Message] = $"dokument {ponuda.Naslov} dodano. Id mjesta = {ponuda.PonudaId}";
          TempData[Constants.ErrorOccurred] = false;
          return RedirectToAction(nameof(Index));

        }
        catch (Exception exc)
        {
          ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
          await PrepareDropDownLists();
          return View(ponuda);
        }
      }
      else
      {
        await PrepareDropDownLists();
        return View(ponuda);
      }
    }


        /// <summary>
        /// Metoda za uređivanje brisanje ponude
        /// </summary>
        /// <param name="id">Identifikator ponude koja se brise</param>
        /// <param name="page">Broj stranice</param>
        /// <param name="sort">Atribut po kojemu se sortira</param>
        /// <param name="ascending">Oznaka je li sortiranje uzlazno ili silazno</param>
  
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int page = 1, int sort = 1, bool ascending = true)
    {
      var ponuda = await ctx.Ponuda.FindAsync(id);                       
      if (ponuda != null)
      {var idemo = new PonudaNatječaj();
            idemo.PonudaId=ponuda.PonudaId;
            idemo.EvidBrojNatječaj = ponuda.EvidBrojNatječaj;
      var idemo2 = new PonudaPonuditelj();
            idemo2.PonudaId=ponuda.PonudaId;
            idemo2.OibPonuditelj = ponuda.OibPonuditelj;
        try
        {
          string naziv = ponuda.Naslov;
          ctx.Remove(idemo); 
          ctx.Remove(idemo2); 
          ctx.Remove(ponuda);          
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

      var natječaji = await ctx.Natječajs                      
                            .OrderBy(d => d.NazivNatječaja)
                            .Select(d => new { d.NazivNatječaja, d.EvidBrojNatječaj })
                            .ToListAsync();
        
      ViewBag.Natječaj = new SelectList(natječaji, nameof(Natječaj.EvidBrojNatječaj), nameof(Natječaj.NazivNatječaja));

      var ponuditelji = await ctx.Ponuditeljs                      
                            .OrderBy(d => d.NazivPonuditelj)
                            .Select(d => new { d.NazivPonuditelj, d.OibPonuditelj })
                            .ToListAsync();
        
      ViewBag.Ponuditelj = new SelectList(ponuditelji, nameof(Ponuditelj.OibPonuditelj), nameof(Ponuditelj.NazivPonuditelj));
    }

/*     [HttpGet]
    public async Task<IActionResult> Edit(int id, int page = 1, int sort = 1, bool ascending = true)
    {
      var ponuda = await ctx.Ponuda
                            .AsNoTracking()
                            .Where(m => m.PonudaId == id)
                            .SingleOrDefaultAsync();
      if (ponuda != null)
      {
        ViewBag.Page = page;
        ViewBag.Sort = sort;
        ViewBag.Ascending = ascending;
        await PrepareDropDownLists();
        return View(ponuda);
      }
      else
      {
        return NotFound($"Neispravan id ponude: {id}");
      }
    } */

 
    /* public async Task<IActionResult> Edit(Ponudum ponuda, int page = 1, int sort = 1, bool ascending = true)
    { 
             
      if (ponuda == null)
      {
        return NotFound("Nema poslanih ponuda");
      }
      bool checkId = await ctx.Ponuda.AnyAsync(m => m.PonudaId == ponuda.PonudaId);
      if (!checkId)
      {
        return NotFound($"Neispravan id mjesta: {ponuda?.PonudaId}");
      }
      
      if (ModelState.IsValid)
      { var idemo = new PonudaNatječaj();
            idemo.PonudaId=ponuda.PonudaId;
            idemo.EvidBrojNatječaj = ponuda.EvidBrojNatječaj;
        var idemo2 = new PonudaPonuditelj();
            idemo2.PonudaId=ponuda.PonudaId;
            idemo2.OibPonuditelj = ponuda.OibPonuditelj;
        try
        { ctx.Update(idemo);
        ctx.Update(idemo2);
          ctx.Update(ponuda);
          await ctx.SaveChangesAsync();
          TempData[Constants.Message] = "ponuda ažurirana.";
          TempData[Constants.ErrorOccurred] = false;
          return RedirectToAction(nameof(Index), new { page, sort, ascending });
        }
        catch (Exception exc)
        {
          ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
          await PrepareDropDownLists();
          return View(ponuda);
        }
      }
      else
      {
        await PrepareDropDownLists();
        return View(ponuda);
      }
    } */

/*     [HttpPost]
    public IActionResult Filter(PonudaFilter filter)
    {
      return RedirectToAction(nameof(Index), new { filter = filter.ToString() });
    } */


    [HttpGet]
    public Task<IActionResult> Edit(int id, int page = 1, int sort = 1, bool ascending = true)
    {
      return Show(id, page, sort, ascending, viewName: nameof(Edit));
    }

        /// <summary>
        /// Metoda za uređivanje Ponude
        /// </summary>
        /// <param name="model">Identifikator konzorcija koji se uređuje (id)</param>
        /// <param name="page">Broj stranice</param>
        /// <param name="sort">Atribut po kojemu se sortira</param>
        /// <param name="ascending">Oznaka je li sortiranje uzlazno ili silazno</param>

    [HttpPost ,ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(PonudaViewModel model, int page = 1, int sort = 1, bool ascending = true)
    {
      ViewBag.Page = page;
      ViewBag.Sort = sort;
      ViewBag.Ascending = ascending;
      if (ModelState.IsValid)
      {
        var ponuda = await ctx.Ponuda
                                .Include(d => d.PonDokumentis)
                                .Where(d => d.PonudaId == model.PonudaId)
                                .FirstOrDefaultAsync();
        if (ponuda == null)
        {
          return NotFound("Ne postoji ponuda s id-om: " + model.PonudaId);
        }
 /*        if (position.HasValue)
        {
          page = 1 + position.Value / appData.PageSize;
          await SetPreviousAndNext(position.Value, filter, sort, ascending);
        } */

       

        ponuda.PonudaId = model.PonudaId;
        ponuda.EvidBrojNatječaj = model.EvidBrojNatječaj;
        ponuda.Text = model.Text;
        ponuda.Naslov = model.Naslov;
        ponuda.OibPonuditelj = model.OibPonuditelj;

        List<int> DoksId = model.doks
                                  .Where(s => s.DokumentId > 0)
                                  .Select(s => s.DokumentId)
                                  .ToList();
        //izbaci sve koje su nisu više u modelu
        ctx.RemoveRange(ponuda.PonDokumentis.Where(s => !DoksId.Contains(s.DokumentId)));

        ctx.RemoveRange(ctx.Dokuments.Where(s => !DoksId.Contains(s.DokumentId)));

        foreach (var dokument1 in model.doks)
        {
          //ažuriraj postojeće i dodaj nove
          Dokument noviDokkument; // potpuno nova ili dohvaćena ona koju treba izmijeniti
          if (dokument1.DokumentId > 0)
          {
            noviDokkument = ponuda.PonDokumentis.First(s => s.DokumentId == dokument1.DokumentId).Dokument;
          }
          else
          {
            noviDokkument = new Dokument();
            
            ctx.Add(noviDokkument);
          }
          noviDokkument.DokumentId = dokument1.DokumentId;
          noviDokkument.Naslov = dokument1.Naslov;
          noviDokkument.Vrsta = dokument1.Vrsta;
          noviDokkument.Blob = dokument1.Blob;
          noviDokkument.PonudaId = dokument1.PonudaId;
          noviDokkument.DatumPredaje = dokument1.DatumPredaje;

        }

        try
        {

          await ctx.SaveChangesAsync();

          TempData[Constants.Message] = $"Ponuda {ponuda.PonudaId} uspješno ažuriran.";
          TempData[Constants.ErrorOccurred] = false;
          return RedirectToAction(nameof(Edit), new
          {
            id = ponuda.PonudaId,
            page,
            sort,
            ascending
          });

        }
        catch (Exception exc)
        {
          ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
          return View(model);
        }
      }
      else
      {
        return View(model);
      }
    }

        /// <summary>
        /// Metoda za prikazivanje Ponude 
        /// </summary>
        /// <param name="id">Identifikator ponude koja se pokazuje (PonudaId)</param>
        /// <param name="page">Broj stranice</param>
        /// <param name="sort">Atribut po kojemu se sortira</param>
        /// <param name="ascending">Oznaka je li sortiranje uzlazno ili silazno</param>
        /// <param name="viewName">Ime ponude koja se prikazuje</param>
    
    public async Task<IActionResult> Show(int id, int page = 1, int sort = 1, bool ascending = true, string viewName = nameof(Show))
    {      
      var ponuda = await ctx.Ponuda
                              .Where(d => d.PonudaId == id)
                              .Select(d => new PonudaViewModel
                              {
                                PonudaId = d.PonudaId,
                                NazivNatječaja =  d.PonudaNatječajs.Where(k => k.PonudaId == d.PonudaId).First().EvidBrojNatječajNavigation.NazivNatječaja,
                                Text = d.Text,
                                Naslov = d.Naslov,
                                NazivPonuditelj = d.PonudaPonuditelj.OibPonuditeljNavigation.NazivPonuditelj,
                                OibPonuditelj=d.OibPonuditelj,
                                EvidBrojNatječaj=d.EvidBrojNatječaj
                              })
                              .FirstOrDefaultAsync();
      if (ponuda == null)
      {
        return NotFound($"Ponuda {id} ne postoji");
      }

        //učitavanje dokumenata
        var dokumenti = await ctx.Dokuments
                              .Where(s => s.PonudaId == ponuda.PonudaId)
                              .OrderBy(s => s.DokumentId)
                              .Select(s => new DokumentViewModel
                              {
                                DokumentId = s.DokumentId,
                                Naslov = s.Naslov,
                                Vrsta = s.Vrsta,
                                Blob = s.Blob,
                                PonudaId = s.PonudaId,
                                NazivPonude = s.Naslov,
                                DatumPredaje = s.DatumPredaje
                              })
                              .ToListAsync();
      ponuda.doks = dokumenti;

        ViewBag.Page = page;
        ViewBag.Sort = sort;
        ViewBag.Ascending = ascending;

        return View(viewName, ponuda);
      }
    }   

    
  }

