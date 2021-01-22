using JavnaNabava.Models;
using JavnaNabava.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace JavnaNabava.Controllers
{
    /// <summary>
    /// Kontroler za ponuditelje 
    /// </summary>
    public class PonuditeljController : Controller
    {
        private readonly RPPP06Context ctx;
        private readonly AppSettings appSettings;

        public PonuditeljController(RPPP06Context ctx, IOptionsSnapshot<AppSettings> optionSnapshot)
        {
            this.ctx = ctx;
            appSettings = optionSnapshot.Value;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Metoda za dodavanje novog ponuditelja
        /// </summary>
        /// <param name="ponuditelj">Ponuditelj koji se dodaje</param>
        /// <returns>Vraća pogled</returns>
        [HttpPost]
        [ValidateAntiForgeryToken] //medu podacima koje primamo mora biti i antiforgery token
        public IActionResult Create(Ponuditelj ponuditelj) //podaci se prvo uzimaju iz forme, pa iz parametara rute i onda query stringa
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(ponuditelj);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Ponuditelj {ponuditelj.NazivPonuditelj} uspješno dodan.";
                    TempData[Constants.ErrorOccured] = false;

                    return RedirectToAction("Index");
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage()); //radimo proširenje za Exception
                    return View(ponuditelj); //ne vracamo praznu stranicu, nego postojece podatke
                }
            }
            else
            {
                return View(ponuditelj);
            }

        }

        /// <summary>
        /// Metoda za brisanje ponuditelja
        /// </summary>
        /// <param name="OibPonuditelj"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="ascending"></param>
        /// <returns>Vraća pogled na index stranicu</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        //parametar kako smo ga nazvali u <form>/name u Index.cshtml
        public IActionResult Delete(string OibPonuditelj, int page = 1, int sort = 1, bool ascending = true)
        {
            var ponuditelj = ctx.Ponuditeljs.Find(OibPonuditelj);
            if (ponuditelj == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    string naziv = ponuditelj.NazivPonuditelj;
                    ctx.Remove(ponuditelj); //oznacimo pon. za brisanje
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Ponuditelj {naziv} uspješno obrisan.";
                    TempData[Constants.ErrorOccured] = false;

                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Pogreška prilikom brisanja ponuditelja: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccured] = true;
                }
                return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
            }

        }

        

        [HttpGet]
        public IActionResult Edit(string id, int page = 1, int sort = 1, bool ascending = true)
        {
            var ponuditelj = ctx.Ponuditeljs.Where(p => p.OibPonuditelj == id).FirstOrDefault();
            if (ponuditelj == null)
            {
                return NotFound($"Ne postoji ponuditelj čiji je OIB {id}");
            }
            else
            {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                return View(ponuditelj);
            }

        }

        /// <summary>
        /// Metoda za uređivanje ponuditelja
        /// </summary>
        /// <param name="id">Identifikator ponuditelja koji se uređuje (oib)</param>
        /// <param name="page">Broj stranice</param>
        /// <param name="sort">Atribut po kojemu se sortira</param>
        /// <param name="ascending">Oznaka je li sortiranje uzlazno ili silazno</param>
        /// <returns>Vraća pogled</returns>
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, int page = 1, int sort = 1, bool ascending = true)
        {
            try
            {
                Ponuditelj ponuditelj = await ctx.Ponuditeljs.FindAsync(id);
                if (ponuditelj == null)
                {
                    return NotFound($"Ne postoji ponuditelj čiji je OIB {id}");
                }


                bool ok = await TryUpdateModelAsync<Ponuditelj>(ponuditelj, "", p => p.NazivPonuditelj, p => p.AdresaPonuditelj, p => p.SjedištePonuditelj);
                if (ok)
                {
                    ViewBag.Page = page;
                    ViewBag.Sort = sort;
                    ViewBag.Ascending = ascending;
                    try
                    {
                        await ctx.SaveChangesAsync();
                        TempData[Constants.Message] = $"Ponuditelj {ponuditelj.NazivPonuditelj} uspješno ažuriran.";
                        TempData[Constants.ErrorOccured] = false;
                        return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        return View(ponuditelj);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Podatke nije moguće povezati");
                    return View(ponuditelj);
                }

            }
            catch (Exception exc)
            {
                TempData[Constants.Message] = exc.CompleteExceptionMessage();
                TempData[Constants.ErrorOccured] = true;
                return RedirectToAction(nameof(Edit), id);
            }

        }


        /// <summary>
        /// Prikazuje početnu stranicu
        /// </summary>
        /// <param name="page">Broj stranice</param>
        /// <param name="sort">Atribut po kojemu se sortira</param>
        /// <param name="ascending">Oznaka je li sortiranje uzlazno ili silazno</param>
        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pageSize = appSettings.PageSize;
            //koliko ima podataka
            var query = ctx.Ponuditeljs.AsNoTracking();
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
                //redirect
                return RedirectToAction(nameof(Index), new { page = pagingInfo.TotalPages, sort, ascending });
            }

            //za orderBy
            System.Linq.Expressions.Expression<Func<Ponuditelj, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = p => p.OibPonuditelj;
                    break;
                case 2:
                    orderSelector = p => p.NazivPonuditelj;
                    break;
                case 3:
                    orderSelector = p => p.AdresaPonuditelj;
                    break;
                case 4:
                    orderSelector = p => p.SjedištePonuditelj;
                    break;
            }

            //kad odaberemo neki od selectora, zelimo li sortirati silazno ili uzlazno 
            if (orderSelector != null)
            {
                //prosirujemo query
                query = ascending ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);
            }


            var ponuditelji = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var model = new PonuditeljiViewModel
            {
                Ponuditelji = ponuditelji,
                PagingInfo = pagingInfo
            };
            return View(model);
        }
    }
}
