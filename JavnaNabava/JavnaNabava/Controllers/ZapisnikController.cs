using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;
using JavnaNabava.ViewModels;
using JavnaNabava.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using JavnaNabava.Extensions.Selectors;
using System.Text.Json;
using JavnaNabava.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JavnaNabava.Controllers
{
    public class ZapisnikController : Controller
    {
        private readonly RPPP06Context ctx;
        private readonly AppSettings appSettings;
        private readonly ILogger<ZapisnikController> logger;

        public ZapisnikController(RPPP06Context ctx, IOptionsSnapshot<AppSettings> optionSnapshot, ILogger<ZapisnikController> logger)
        {
            this.ctx = ctx;
            appSettings = optionSnapshot.Value;
            this.logger = logger;

        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var zapisnik = new ZapisnikViewModel
            {
                
            };
            return View(zapisnik);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ZapisnikViewModel model)
        {
            if (ModelState.IsValid)
            {
                Zapisnik z = new Zapisnik();
                z.ZapisnikId = model.ZapisnikId;
                z.IdPovjerenstva = model.IdPovjerenstva;
                z.PonudaId = model.IdPonude;
                z.NazivZapisnik = model.NazivZapisnik;
                z.IdPrethZapisnika = model.IdPrethZapisnika;
                z.IspravnostZapisnik = model.ispravnostZapisnik;

                foreach (var stavka in model.StavkeZapisnika)
                {
                    StavkaZapisnik novaStavka = new StavkaZapisnik();
                    novaStavka.idOdredba = stavka.idOdredba;
                    novaStavka.cijenaKršenja = stavka.cijenaKršenjaOdluke;
                    novaStavka.ispravnostStavka = stavka.ispravnostStavka;
                    z.StavkaZapisniks.Add(novaStavka);
                }

                try
                {
                    ctx.Add(z);
                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"Zapisnik uspješno dodan. Id={z.ZapisnikId}";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Edit), new { id = z.ZapisnikId });

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

        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Zapisnik zapisnik)
        {
            logger.LogTrace(JsonSerializer.Serialize(zapisnik));
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(zapisnik);
                    ctx.SaveChanges();
                    logger.LogInformation(new EventId(1000), $"Zapisnik {zapisnik.NazivZapisnik} dodan.");

                    TempData[Constants.Message] = $"Zapisnik {zapisnik.NazivZapisnik} dodan.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    PrepareDropDownList();
                    PrepareDropDownList2();
                    logger.LogError("Pogreška prilikom dodavanja novog zapisnika: {0}", exc.Message);
                    ModelState.AddModelError(string.Empty, exc.Message); 
                    return View(zapisnik); 
                }
            }
            else
            {
                PrepareDropDownList();
                PrepareDropDownList2();
                return View(zapisnik);
            }
        }
        
        public async Task<IActionResult> Show(int id, int? position, int page = 1, int sort = 1, bool ascending = true, string viewName = "Show")
        {
            var zapisnik = await ctx.Zapisniks
                                    .Where(z => z.ZapisnikId == id)
                                    .Select(z => new ZapisnikViewModel
                                    {
                                        ZapisnikId = z.ZapisnikId,
                                        NazivZapisnik = z.NazivZapisnik,
                                        IdPovjerenstva = z.IdPovjerenstva,
                                        IdPonude = z.PonudaId,
                                        IdPrethZapisnika = z.IdPrethZapisnika
                                    })
                                    .FirstOrDefaultAsync();
                                  
            if (zapisnik == null)
            {
                return NotFound($"Zapisnik {id} ne postoji");
            }
            else
            {
                zapisnik.NazivPovjerenstva = await ctx.vw_Povjerenstvo
                                                .Where(p => p.IdPovjerenstva == zapisnik.IdPovjerenstva)
                                                .Select(p => p.NazivPovjerenstva)
                                                .FirstOrDefaultAsync();

                zapisnik.TextPonude = await ctx.vw_Ponuda
                                                .Where(p => p.PonudaId == zapisnik.IdPonude)
                                                .Select(p => p.Text)
                                                .FirstOrDefaultAsync();

                if (zapisnik.IdPrethZapisnika.HasValue)
                {
                    zapisnik.NazPrethodnogZapisnika = await ctx.vw_Zapisnici
                                                               .Where(z => z.ZapisnikId == zapisnik.IdPrethZapisnika)
                                                               .Select(z => z.ZapisnikId + " " + z.NazivPovjerenstva)
                                                               .FirstOrDefaultAsync();
                }
                //učitavanje stavki zapisnika
                var stavke = await ctx.StavkaZapisniks
                                      .Where(s => s.zapisnikID == zapisnik.ZapisnikId)
                                      .OrderBy(s => s.)
                                      .Select(s => new StavkaZapisnikViewModel
                                      {
                                          redniBrojStavke = s.RedniBrojStavke,
                                          naslovStavke = s.NaslovStavke,
                                          odredbaStavke = s.OdredbaStavke,
                                          uvjetStavke = s.UvjetStavke
                                      })
                                      .ToListAsync();
                zapisnik.StavkeZapisnika = stavke;

                if (position.HasValue)
                {
                    page = 1 + position.Value / appSettings.PageSize;
                    await SetPreviousAndNext(position.Value, sort, ascending);
                }

                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                ViewBag.Position = position;

                return View(viewName, zapisnik);
            
            }
        }
        */

        public async Task<IActionResult> Show(int id, int? position, int page = 1, int sort = 1, bool ascending = true, string viewName = nameof(Show))
        {
            var zapisnik = await ctx.Zapisniks
                                    .Where(z => z.ZapisnikId == id)
                                    .Select(z => new ZapisnikViewModel
                                    {
                                        ZapisnikId = z.ZapisnikId,
                                        IdPovjerenstva = z.IdPovjerenstva,
                                        IdPonude = z.PonudaId,
                                        NazivZapisnik = z.NazivZapisnik,
                                        ispravnostZapisnik = z.IspravnostZapisnik,
                                        IdPrethZapisnika = z.IdPrethZapisnika
                                    })
                                    .FirstOrDefaultAsync();
            if (zapisnik == null)
            {
                return NotFound($"Zapisnik {id} ne postoji");
            }
            else
            {
                zapisnik.NazivPovjerenstva = await ctx.vw_Povjerenstvo
                                                .Where(p => p.IdPovjerenstva == zapisnik.IdPovjerenstva)
                                                .Select(p => p.NazivPovjerenstva)
                                                .FirstOrDefaultAsync();

                zapisnik.TextPonude = await ctx.vw_Ponuda
                                                .Where(p => p.PonudaId == zapisnik.IdPonude)
                                                .Select(p => p.Text)
                                                .FirstOrDefaultAsync();

                if (zapisnik.IdPrethZapisnika.HasValue)
                {
                    zapisnik.NazPrethodnogZapisnika = await ctx.vw_Zapisnici
                                                               .Where(z => z.ZapisnikId == zapisnik.IdPrethZapisnika)
                                                               .Select(z => z.ZapisnikId + " " + z.NazivPovjerenstva)
                                                               .FirstOrDefaultAsync();
                }
                //učitavanje stavki
                var stavke = await ctx.StavkaZapisniks
                                      .Where(s => s.zapisnikID == zapisnik.ZapisnikId)
                                      .OrderBy(s => s.idStavke)
                                      .Select(s => new StavkaZapisnikViewModel
                                      {
                                          idStavka = s.idStavke,
                                          ZapisnikId = s.zapisnikID,
                                          idOdredba = s.idOdredba,
                                          nazivOdredba = s.OdredbaZapisnik.nazivOdredba,
                                          ispravnostStavka = s.ispravnostStavka,
                                          cijenaKršenjaOdluke = s.cijenaKršenja
                                      })
                                      .ToListAsync();
                zapisnik.StavkeZapisnika = stavke;

                if (position.HasValue)
                {
                    page = 1 + position.Value / appSettings.PageSize;
                    await SetPreviousAndNext(position.Value, sort, ascending);
                }

                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                ViewBag.Position = position;

                return View(viewName, zapisnik);
            }
        }

        private async Task SetPreviousAndNext(int position, int sort, bool ascending)
        {
            var query = ctx.vw_Zapisnici.AsQueryable();

            query = query.ApplySort(sort, ascending);

            if (position > 0)
            {
                ViewBag.Previous = await query.Skip(position - 1).Select(z => z.ZapisnikId).FirstAsync();
            }
            if (position < await query.CountAsync() - 1)
            {
                ViewBag.Next = await query.Skip(position + 1).Select(z => z.ZapisnikId).FirstAsync();
            }
        }

        [HttpGet]
        public Task<IActionResult> Edit(int id,int? position, int page = 1, int sort = 1, bool ascending = true)
        {
            return Show(id, position, page, sort, ascending, viewName: nameof(Edit));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ZapisnikViewModel model, int? position, int page = 1, int sort = 1, bool ascending = true)
        {
            ViewBag.Page = page;
            ViewBag.Sort = sort;
            ViewBag.Ascending = ascending;
            ViewBag.Position = position;
            if (ModelState.IsValid)
            {
                var zapisnik = await ctx.Zapisniks
                                        .Include(d => d.StavkaZapisniks)
                                        .Where(d => d.ZapisnikId == model.ZapisnikId)
                                        .FirstOrDefaultAsync();
                if (zapisnik == null)
                {
                    return NotFound("Ne postoji zapisnik s id-om: " + model.ZapisnikId);
                }

                if (position.HasValue)
                {
                    page = 1 + position.Value / appSettings.PageSize;
                    await SetPreviousAndNext(position.Value, sort, ascending);
                }

                zapisnik.ZapisnikId = model.ZapisnikId;
                zapisnik.PonudaId = model.IdPonude;
                zapisnik.IdPovjerenstva = model.IdPovjerenstva;
                zapisnik.NazivZapisnik = model.NazivZapisnik;
                zapisnik.IdPrethZapisnika = model.IdPrethZapisnika;
                zapisnik.IspravnostZapisnik = model.ispravnostZapisnik;

                List<int> idStavki = model.StavkeZapisnika
                                          .Where(s => s.idStavka > 0)
                                          .Select(s => s.idStavka)
                                          .ToList();
                //izbaci sve koje su nisu više u modelu
                ctx.RemoveRange(zapisnik.StavkaZapisniks.Where(s => !idStavki.Contains(s.idStavke)));

                foreach (var stavka in model.StavkeZapisnika)
                {
                    //ažuriraj postojeće i dodaj nove
                    StavkaZapisnik novaStavka; // potpuno nova ili dohvaćena ona koju treba izmijeniti
                    if (stavka.idStavka > 0)
                    {
                        novaStavka = zapisnik.StavkaZapisniks.First(s => s.idStavke == stavka.idStavka);
                    }
                    else
                    {
                        novaStavka = new StavkaZapisnik();
                        zapisnik.StavkaZapisniks.Add(novaStavka);
                    }
                    // stavka.paramteri su prazni??
                    novaStavka.idStavke = stavka.idStavka;
                    novaStavka.zapisnikID = stavka.ZapisnikId;
                    novaStavka.idOdredba = stavka.idOdredba;
                    novaStavka.ispravnostStavka = stavka.ispravnostStavka;
                    novaStavka.cijenaKršenja = stavka.cijenaKršenjaOdluke;
                }

                try
                {

                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"Zapisnik {zapisnik.ZapisnikId} uspješno ažuriran.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Edit), new
                    {
                        id = zapisnik.ZapisnikId,
                        position,
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

        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ZapisnikViewModel model, int? position, int page = 1, int sort = 1, bool ascending = true)
        {
            ViewBag.Page = page;
            ViewBag.Sort = sort;
            ViewBag.Ascending = ascending;;
            ViewBag.Position = position;
            if (ModelState.IsValid)
            {
                var zapisnik = await ctx.Zapisniks
                                        .Include(z => z.StavkaZapisniks)
                                        .Where(s => s.ZapisnikId == model.ZapisnikId)
                                        .FirstOrDefaultAsync();
                if (zapisnik == null)
                {
                    return NotFound("Ne postoji zapisnik s id-om: " + model.ZapisnikId);
                }

                if (position.HasValue)
                {
                    page = 1 + position.Value / appSettings.PageSize;
                    await SetPreviousAndNext(position.Value, sort, ascending);
                }

                zapisnik.ZapisnikId = model.ZapisnikId;
                zapisnik.PonudaId = model.IdPonude;
                zapisnik.IdPovjerenstva = model.IdPovjerenstva;
                zapisnik.NazivZapisnik = model.NazivZapisnik;
                zapisnik.IdPrethZapisnika = model.IdPrethZapisnika;

                List<int> idStavki = model.StavkeZapisnika
                                          .Where(s => s.redniBrojStavke > 0)
                                          .Select(s => s.redniBrojStavke)
                                          .ToList();
                //izbaci sve koje su nisu više u modelu
                ctx.RemoveRange(zapisnik.StavkaZapisniks.Where(s => !idStavki.Contains(s.RedniBrojStavke)));

                foreach (var stavka in model.StavkeZapisnika)
                {
                    //ažuriraj postojeće i dodaj nove
                    StavkaZapisnik novaStavka; // potpuno nova ili dohvaćena ona koju treba izmijeniti

                    if (stavka.redniBrojStavke > 0)
                    {
                        novaStavka = zapisnik.StavkaZapisniks.First(s => s.RedniBrojStavke == stavka.redniBrojStavke);
                    }
                    else
                    {
                        novaStavka = new StavkaZapisnik();
                        zapisnik.StavkaZapisniks.Add(novaStavka);
                    }

                    novaStavka.RedniBrojStavke = stavka.redniBrojStavke;
                    novaStavka.NaslovStavke = stavka.naslovStavke;
                    novaStavka.UvjetStavke = stavka.uvjetStavke;
                    novaStavka.OdredbaStavke = stavka.odredbaStavke;
                }

                //eventualno umanji iznos za dodatni popust za kupca i sl... nešto što bi bilo poslovno pravilo
                try
                {

                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"Zapisnik {zapisnik.ZapisnikId} uspješno ažuriran.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Edit), new
                    {
                        id = zapisnik.ZapisnikId,
                        position,
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
        */

        /*
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            try
            {
                Zapisnik zapisnik = await ctx.Zapisniks.Where(z => z.ZapisnikId == id).SingleOrDefaultAsync();
                if (zapisnik == null)
                {
                    return NotFound($"Zapisnik {id} ne postoji");
                }


                bool potvrda = await TryUpdateModelAsync<Zapisnik>(zapisnik, "", z => z.NazivZapisnik, z => z.IdPovjerenstva, z => z.PonudaId);
                if (potvrda)
                {
                    ViewBag.Page = page;
                    ViewBag.Sort = sort;
                    ViewBag.Ascending = ascending;
                    try
                    {
                        await ctx.SaveChangesAsync();
                        TempData[Constants.Message] = $"Zapisnik {zapisnik.NazivZapisnik} uspješno ažuriran.";
                        TempData[Constants.ErrorOccurred] = false;
                        return RedirectToAction(nameof(Index), new { page, sort, ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        PrepareDropDownList();
                        PrepareDropDownList2();
                        return View(zapisnik);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Podatke nije moguće povezati");
                    PrepareDropDownList();
                    PrepareDropDownList2();
                    return View(zapisnik);
                }

            }
            catch (Exception exc)
            {
                TempData[Constants.Message] = exc.CompleteExceptionMessage();
                TempData[Constants.ErrorOccurred] = true;
                return RedirectToAction(nameof(Edit), new { id, page, sort, ascending });
            }

        }
        */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int zapisnikID, int page = 1, int sort = 1, bool ascending = true)
        {
            var zapisnik = ctx.Zapisniks.Find(zapisnikID);
            if (zapisnik == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    string naziv = zapisnik.NazivZapisnik;
                    ctx.Remove(zapisnik);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Zapisnik {naziv} uspješno obrisan.";
                    TempData[Constants.ErrorOccurred] = false;

                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Pogreška prilikom brisanja zapisnika: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = false;
                }
                return RedirectToAction(nameof(Index), new { page, sort, ascending });
            }
        }

        public async Task<IActionResult> Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appSettings.PageSize;
            var query = ctx.vw_Zapisnici.AsQueryable();

            int count = await query.CountAsync();

            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                Sort = sort,
                Ascending = ascending,
                ItemsPerPage = pagesize,
                TotalItems = count
            };

            if (count > 0 && (page < 1 || page > pagingInfo.TotalPages))
            {
                return RedirectToAction(nameof(Index), new { page = 1, sort, ascending });
            }

            query = query.ApplySort(sort, ascending);

            var zapisnici = await query
                                  .Skip((page - 1) * pagesize)
                                  .Take(pagesize)
                                  .ToListAsync();

            for (int i = 0; i < zapisnici.Count; i++)
            {
                zapisnici[i].Position = (page - 1) * pagesize + i;
            }
            var model = new ZapisniciViewModel
            {
                Zapisnici = zapisnici,
                PagingInfo = pagingInfo
            };

            return View(model);
        }

        /*
        public async Task<IActionResult> Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appSettings.PageSize;

            var query = ctx.Zapisniks.AsNoTracking();

            int count = query.Count();

            if (count == 0)
            {
                logger.LogInformation("Ne postoji nijedan zapisnik.");
                TempData[Constants.Message] = "Ne postoji niti jedan zapisnik.";
                TempData[Constants.ErrorOccurred] = false;
                return RedirectToAction(nameof(Create));
            }

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
                return RedirectToAction(nameof(Index), new { page = pagingInfo.TotalPages, sort, ascending });
            }

            query = query.ApplySort(sort, ascending);

            var zapisnici = await query
            .Select(z => new ZapisnikViewModel
            {

                NazivZapisnik = z.NazivZapisnik,
                ZapisnikId = z.ZapisnikId,
                NazivPovjerenstva = z.IdPovjerenstvaNavigation.NazivPovjerenstva,
                TextPonude = z.IdPonudeNavigation.Text

            })
            .Skip((page - 1) * pagesize).Take(pagesize).ToListAsync();
            

            var model = new ZapisniciViewModel
            {
                Zapisnici = zapisnici,
                PagingInfo = pagingInfo
            };
            return View(model);
        }*/

        private void PrepareDropDownList()
        {
            var povjerenstva = ctx.Povjerenstvos.OrderBy(p => p.IdPovjerenstva)
                .Select(p => new
                {
                    p.IdPovjerenstva
                }).ToList();
            ViewBag.povjerenstva = new SelectList(povjerenstva, nameof(Povjerenstvo.IdPovjerenstva), nameof(Povjerenstvo.IdPovjerenstva));
        }

        private void PrepareDropDownList2()
        {
            var ponude = ctx.Ponuda.OrderBy(p => p.PonudaId)
                .Select(p => new
                {
                    p.PonudaId
                }).ToList();
            ViewBag.ponude = new SelectList(ponude, nameof(Ponudum.PonudaId), nameof(Ponudum.PonudaId));
        }
    }
}
