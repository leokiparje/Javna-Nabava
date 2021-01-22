using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;
using Microsoft.Extensions.Options;
using JavnaNabava.ViewModels;
using JavnaNabava.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JavnaNabava.Controllers
{
    public class StavkaUTroškovnikuController : Controller
    {
        private readonly RPPP06Context ctx;

        private readonly AppSettings appSettings;
        public StavkaUTroškovnikuController(RPPP06Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot)
        {
            this.ctx = ctx;
            appSettings = optionsSnapshot.Value;
        }
        /// <summary>
        /// priprema listu vrsti 
        /// </summary>
        /// <returns>vraća view()</returns>
        [HttpGet]
        public IActionResult Create()
        {
            PrepareDropDownLists();
            return View();
        }


        /// <summary>
        /// Stvara listu svih vrsti stavki i sprema u ViewBag.Vrste
        /// </summary>
        private void PrepareDropDownLists()
        {
            var vrste = ctx.VrstaStavkes.OrderBy(d => d.NazivVrste)
                                        .Select(d => new { d.NazivVrste, d.IdVrste })
                                        .ToList();
            ViewBag.Vrste = new SelectList(vrste, nameof(VrstaStavke.IdVrste), nameof(VrstaStavke.NazivVrste));
        }
        /// <summary>
        /// Stvara novu stavku u troškovniku i sprema ju u bazu
        /// </summary>
        /// <param name="stavkaUTroškovniku"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StavkaUTroškovniku stavkaUTroškovniku)
        {
            if (ModelState.IsValid)
                try
                {
                    //traži slijedeći key
                    var query = ctx;
                    int zadnji = query.StavkaUTroškovnikus.AsNoTracking().Count();
                    query = ctx;
                    var query2 = query.StavkaUTroškovnikus.AsQueryable();
                    int Id = query2.OrderBy(d => d.IdStavke).Skip(zadnji - 1).Select(d => d.IdStavke).First() + 1;
                    stavkaUTroškovniku.IdStavke = Id;

                    ctx.Add(stavkaUTroškovniku);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Stavku pod nazivom {stavkaUTroškovniku.NazivStavke} uspješno dodana";
                    TempData[Constants.ErrorOccurred] = false;

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    PrepareDropDownLists();
                    return View(stavkaUTroškovniku);
                }
            else
            {
                PrepareDropDownLists();
                return View(stavkaUTroškovniku);
            }
                


        }
        /// <summary>
        /// Vraća stranicu za editanje stavke
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(int Id, int page = 1, int sort = 1, bool ascending = true)
        {
            var stavkaUTroškovniku = ctx.StavkaUTroškovnikus.AsNoTracking().Where(d => d.IdStavke == Id).SingleOrDefault();
            if (stavkaUTroškovniku == null)
            {
                return NotFound($"Ne postoji stavka s oznakom: {Id}");
            }
            else
            {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                PrepareDropDownLists();
                return View(stavkaUTroškovniku);
            }

        }
        /// <summary>
        /// Preko poslanog Id-a trazi stavku u bazi i onda joj izmjenjuje podatke na one upisane u formi na stranici edit
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int Id, int page = 1, int sort = 1, bool ascending = true)
        {
            try
            {
                StavkaUTroškovniku stavkaUTroškovniku = await ctx.StavkaUTroškovnikus
                          .Where(d => d.IdStavke == Id)
                          .FirstOrDefaultAsync();
                if (stavkaUTroškovniku == null)
                {
                    return NotFound($"Ne postoji stavka s oznakom {Id}");
                }

                if (await TryUpdateModelAsync<StavkaUTroškovniku>(stavkaUTroškovniku, "",
                    d => d.NazivStavke , d => d.TraženaKoličina, d => d.IdVrste, d => d.DodatneInformacije
                 ))
                {
                    ViewBag.Page = page;
                    ViewBag.Sort = sort;
                    ViewBag.Ascending = ascending;
                    try
                    {


                        await ctx.SaveChangesAsync();
                        TempData[Constants.Message] = $"Stavka pod nazivom {stavkaUTroškovniku.NazivStavke} uspješno ažurirana";
                        TempData[Constants.ErrorOccurred] = false;
                        return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        PrepareDropDownLists();
                        return View(stavkaUTroškovniku);
                    }
                }
                else
                {

                    ModelState.AddModelError(string.Empty, "Podatke o stavci nije moguće povezati s forme");
                    PrepareDropDownLists();
                    return View(stavkaUTroškovniku);
                }
            }
            catch (Exception exc)
            {
                TempData[Constants.Message] = exc.CompleteExceptionMessage();
                TempData[Constants.ErrorOccurred] = true;
                PrepareDropDownLists();
                return RedirectToAction(nameof(Edit), new { Id });
            }
        }

        /// <summary>
        /// Brise stavku u bazi
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int Id, int page = 1, int sort = 1, bool ascending = true)
        {
            var stavkaUTroškovniku = ctx.StavkaUTroškovnikus.Find(Id);
            if (stavkaUTroškovniku == null)
            {
                TempData[Constants.Message] = "Ne postoji stavka s oznakom: " + Id;
                TempData[Constants.ErrorOccurred] = true;
            }
            else
            {
                try
                {
                    int ID = stavkaUTroškovniku.IdStavke;
                    ctx.Remove(stavkaUTroškovniku);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Stavka {ID} uspješno obrisana";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Pogreška prilikom brisanja stavke: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }

            }
            return RedirectToAction(nameof(Index), new { page, sort, ascending });

        }
        /// <summary>
        /// Dohvaća sve stavke u troškovniku i daje njihov prikaz
        /// /// Postavlja sortove
        /// </summary>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="ascending"></param>
        /// <returns>View(model)</returns>
        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pageSize = appSettings.PageSize;
            var query = ctx.StavkaUTroškovnikus.AsNoTracking();

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
                return RedirectToAction(nameof(Index), new { page = pagingInfo.TotalPages, sort, ascending });
            }

            System.Linq.Expressions.Expression<Func<StavkaUTroškovniku, object>> orderSelector = null;
            switch (sort)
            {
               
                case 1:
                    orderSelector = d => d.NazivStavke;
                    break;
                case 2:
                    orderSelector = d => d.TraženaKoličina;
                    break;
                case 3:
                    orderSelector = d => d.IdVrsteNavigation.NazivVrste;
                    break;
                case 4:
                    orderSelector = d => d.DodatneInformacije;
                    break;
            }
            if (orderSelector != null)
            {
                query = ascending ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);
            }
            var stavkaUTroškovniku = query
                                .Select(m => new StavkaUTroškovnikuViewModel
                                {
                                    IdStavke = m.IdStavke,
                                    NazivStavke = m.NazivStavke,
                                    TraženaKoličina = m.TraženaKoličina,
                                    NazivVrste = m.IdVrsteNavigation.NazivVrste,
                                    DodatneInformacije = m.DodatneInformacije,
                                        TroškovnikId =  m.TroškovnikStavkas.First().TroškovnikId > 0 ?  m.TroškovnikStavkas.First().TroškovnikId : 0


                                })
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();
            var model = new StavkeUTroškovnikuViewModel
            {
                StavkeUTroškovniku = stavkaUTroškovniku,
                PagingInfo = pagingInfo
            };
            return View(model);
        }
    }
}
