using JavnaNabava.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.ViewModels;
using JavnaNabava.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JavnaNabava.Controllers
{
  public class ZaposlenikController : Controller
  {
    private readonly RPPP06Context context;
    private readonly AppSettings appSettings;
    public ZaposlenikController(RPPP06Context context, IOptionsSnapshot<AppSettings> os)
    {
      this.context = context;
      appSettings = os.Value;
    }


    private void PrepareDropdownLists()
    {
      var kompetencije = context.VrstaKompetencijes
          .Select(s => new { s.NazivKompetencije, s.IdKompetencije })
          .ToList();
      ViewBag.Kompetencije = new SelectList(kompetencije, nameof(VrstaKompetencije.IdKompetencije), nameof(VrstaKompetencije.NazivKompetencije));

      var spreme = context.VrstaStručneSpremes
          .Select(s => new { s.NazivStručneSpreme, s.IdStručneSpreme })
          .ToList();
      ViewBag.StručneSpreme = new SelectList(spreme, nameof(VrstaStručneSpreme.IdStručneSpreme), nameof(VrstaStručneSpreme.NazivStručneSpreme));

      var ponuditelji = context.Ponuditeljs
          .Select(s => new { s.NazivPonuditelj, s.OibPonuditelj })
          .ToList();
      ViewBag.Ponuditelji = new SelectList(ponuditelji, nameof(Ponuditelj.OibPonuditelj), nameof(Ponuditelj.NazivPonuditelj));


    }


    [HttpGet]

    public IActionResult Create()
    {
      PrepareDropdownLists();
      return View();

    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Zaposlenik zaposlenik)
    {
      if (ModelState.IsValid)
      {
        try
        {
          context.Add(zaposlenik);
          context.SaveChanges();
          TempData[Constants.Message] = $"Zaposlenik broj: {zaposlenik.OibZaposlenik} uspješno dodan!";
          TempData[Constants.ErrorOccurred] = false;

          return RedirectToAction(nameof(Index));

        }
        catch (Exception e)
        {
          ModelState.AddModelError(string.Empty, e.CompleteExceptionMessage());
          PrepareDropdownLists();
          return View(zaposlenik);
        }
      }
      else
      {
        PrepareDropdownLists();
        return View(zaposlenik);
      }
    }


    public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
    {
      int pageSize = appSettings.PageSize;
      var query = context.Zaposleniks.AsNoTracking();

      int count = query.Count();

      var pagingInfo = new PagingInfo
      {
        CurrentPage = page,
        Sort = sort,
        Ascending = ascending,
        ItemsPerPage = pageSize,
        TotalItems = count

      };

      if (page > pagingInfo.TotalPages)
      {
        return RedirectToAction(nameof(Index), new
        {
          page = pagingInfo.TotalPages,
          sort,
          ascending
        });
      }

      System.Linq.Expressions.Expression<Func<Zaposlenik, object>> orderSelector = null;
      switch (sort)
      {
        case 1:
          orderSelector = d => d.OibZaposlenik;
          break;
        case 2:
          orderSelector = d => d.ImeZaposlenik;
          break;

        case 3:
          orderSelector = d => d.PrezimeZaposlenik;
          break;
        case 4:
          orderSelector = d => d.DatumRođenja;
          break;
        case 5:
          orderSelector = d => d.MjestoPrebivališta;
          break;
        case 6:
          orderSelector = d => d.IdKompetencijeNavigation.NazivKompetencije;
          break;
        case 7:
          orderSelector = d => d.IdStručneSpremeNavigation.NazivStručneSpreme;
          break;
        case 8:
          orderSelector = d => d.OibPonuditeljNavigation.NazivPonuditelj;
          break;


      }

      if (orderSelector != null)
      {
        query = ascending ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);

      }

      var zaposlenici = query
          .Skip((page - 1) * pageSize)
          .Take(pageSize)
          .Include(z => z.IdKompetencijeNavigation)
          .Include(z => z.IdStručneSpremeNavigation)
          .Include(z => z.OibPonuditeljNavigation)
          .Take(pageSize)
          .ToList();

      var model = new ZaposleniciViewModel
      {
        Zaposlenici = zaposlenici,
        PagingInfo = pagingInfo

      };
      return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]

    public IActionResult Delete(string oibZaposlenik, int page = 1, int sort = 1, bool ascending = true)
    {
      var zaposlenik = context.Zaposleniks.Find(oibZaposlenik);
      if (zaposlenik == null)
      {
        return NotFound();
      }
      else
      {
        string naziv = zaposlenik.OibZaposlenik;
        try
        {

          context.Remove(zaposlenik);
          context.SaveChanges();
          TempData[Constants.Message] = $"Zaposlenik: {naziv} uspješno obrisan!";
          TempData[Constants.ErrorOccurred] = false;



          return RedirectToAction(nameof(Index), new { page, sort, ascending });
        }
        catch (Exception)
        {

          TempData[Constants.Message] = $"Zaposlenik: {naziv} nije obrisan, možete brisati samo zaposlenike bez priloga!.";
          TempData[Constants.ErrorOccurred] = true;
        }

        return RedirectToAction(nameof(Index));
      }
    }


    public async Task<IActionResult> Show(string id, int page = 1, int sort = 1, bool ascending = true, string viewName = nameof(Show))
    {
      var zaposlenik = await context.Zaposleniks
                              .Where(z => z.OibZaposlenik == id)
                              .Select(z => new ZaposlenikViewModel
                              {
                                oibZaposlenik = z.OibZaposlenik,
                                ImeZaposlenik = z.ImeZaposlenik,
                                PrezimeZaposlenik = z.PrezimeZaposlenik,
                                IdKompetencije = z.IdKompetencije,
                                IdStručneSpreme = z.IdStručneSpreme,
                                DatumRođenja = z.DatumRođenja,
                                MjestoPrebivališta = z.MjestoPrebivališta
                              })
                              .FirstOrDefaultAsync();
      if (zaposlenik == null)
      {
        return NotFound($"zaposlenik {id} ne postoji");
      }
      else
      {

        //učitavanje stavki
        var prilozi = await context.ZaposlenikPrilogs
                              .Where(s => s.OibZaposlenik == zaposlenik.oibZaposlenik)
                              .OrderBy(s => s.OibZaposlenik)
                              .Select(s => new ZaposlenikPrilogViewModel
                              {
                                OibZaposlenika = s.OibZaposlenik,
                                IdPriloga = s.IdPrilog,
                                nazivPriloga = s.IdPrilogNavigation.NazivPrilog

                              })
                              .ToListAsync();
        zaposlenik.Prilozi = prilozi;

        zaposlenik.SpremaName = (await context.VrstaStručneSpremes.FindAsync(zaposlenik.IdStručneSpreme)).NazivStručneSpreme;
        zaposlenik.KompetencijaName = (await context.VrstaKompetencijes.FindAsync(zaposlenik.IdKompetencije)).NazivKompetencije;

        return View(viewName, zaposlenik);
      }
    }
    private async Task SetPreviousAndNext(int position, int sort, bool ascending)
    {
      var query = context.Zaposleniks.AsQueryable();


      if (position > 0)
      {
        ViewBag.Previous = await query.Skip(position - 1).Select(z => z.OibZaposlenik).FirstAsync();
      }
      if (position < await query.CountAsync() - 1)
      {
        ViewBag.Next = await query.Skip(position + 1).Select(z => z.OibZaposlenik).FirstAsync();
      }
    }

    [HttpGet]
    public IActionResult Edit(string id, int page = 1, int sort = 1, bool ascending = true)
    {
      var z = context.Zaposleniks
          .Where(d => d.OibZaposlenik == id)
          .FirstOrDefault();

      if (z == null)
      {
        return NotFound($"Ne postoji zaposlenik s ovim oibom {id}");
      }
      else
      {
        ViewBag.Page = page;
        ViewBag.Sort = sort;
        ViewBag.Ascending = ascending;
        PrepareDropdownLists();
        return View(new ZaposlenikViewModel
        {
          oibZaposlenik = z.OibZaposlenik,
          ImeZaposlenik = z.ImeZaposlenik,
          PrezimeZaposlenik = z.PrezimeZaposlenik,
          DatumRođenja = z.DatumRođenja,
          IdKompetencije = z.IdKompetencije,
          IdStručneSpreme = z.IdStručneSpreme,
          MjestoPrebivališta = z.MjestoPrebivališta,
          OibPonuditelj = z.OibPonuditelj,
        }
        );
      }
    }

    [HttpPost, ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(string id, int page = 1, int sort = 1, bool ascending = true)
    {
      try
      {
        var z = context.Zaposleniks
          .Where(d => d.OibZaposlenik == id)
          .FirstOrDefault();

        if (z == null)
        {
          return NotFound($"Ne postoji zaposlenik s ovim oibom {id}");
        }

        bool potvrda = await TryUpdateModelAsync<Zaposlenik>(z, "", d => d.OibZaposlenik, d => d.ImeZaposlenik, d => d.PrezimeZaposlenik, d => d.DatumRođenja, d => d.MjestoPrebivališta, d => d.IdKompetencije, d => d.IdStručneSpreme);
        if (potvrda)
        {
          ViewBag.Page = page;
          ViewBag.Sort = sort;
          ViewBag.Ascending = ascending;
          try
          {
            await context.SaveChangesAsync();
            TempData[Constants.Message] = $"Zaposlenik {z.OibZaposlenik} uspješno ažurirano.";
            TempData[Constants.ErrorOccurred] = false;
            return RedirectToAction(nameof(Index), new { page, sort, ascending });
          }
          catch (Exception exc)
          {
            ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
            PrepareDropdownLists();
            return View(z);
          }
        }
        else
        {
          ModelState.AddModelError(string.Empty, "Podatke nije moguće povezati");
          PrepareDropdownLists();
          return View(z);
        }

      }
      catch (Exception exc)
      {
        TempData[Constants.Message] = exc.CompleteExceptionMessage();
        TempData[Constants.ErrorOccurred] = true;
        return RedirectToAction(nameof(Edit), new { id, page, sort, ascending });
      }

    }
  }
}

