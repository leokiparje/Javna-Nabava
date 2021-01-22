using JavnaNabava.Extensions;
using JavnaNabava.Models;
using JavnaNabava.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.Controllers
{

    /// <summary>
    /// Kontroler za konzorcije
    /// </summary>
    public class KonzorcijController : Controller
    {
        private readonly Random random = new Random();
        private readonly RPPP06Context ctx;
        private readonly AppSettings appSettings;
        private int MAX = int.MaxValue;

        public KonzorcijController(RPPP06Context ctx, IOptionsSnapshot<AppSettings> optionSnapshot)
        {
            this.ctx = ctx;
            appSettings = optionSnapshot.Value;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        private int GenerateRandomNumber()
        {
            return random.Next(10, MAX);
        }

        /// <summary>
        /// Metoda za dodavanje novog konzorcija
        /// </summary>
        /// <param name="konzorcij">Konzorcij koji se dodaje</param>
        /// <returns>Vraća pogled</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Konzorcij konzorcij)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    konzorcij.IdKonzorcij = GenerateRandomNumber(); 
                    ctx.Add(konzorcij);
                    await ctx.SaveChangesAsync();
                    TempData[Constants.Message] = $"Ponuditelj {konzorcij.NazivKonzorcij} uspješno dodan.";
                    TempData[Constants.ErrorOccured] = false;

                    return RedirectToAction("Index");
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage()); //radimo proširenje za Exception
                    return View(konzorcij); //ne vracamo praznu stranicu, nego postojece podatke
                }
            }
            else
            {
                return View(konzorcij);
            }
        }

        /// <summary>
        /// Metoda za brisanje konzorcija
        /// </summary>
        /// <param name="IdKonzorcij"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="ascending"></param>
        /// <returns>Vraća pogled na index stranicu</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int IdKonzorcij, int page = 1, int sort = 1, bool ascending = true)
        {
            var konzorcij = ctx.Konzorcijs.Find(IdKonzorcij);
            if (konzorcij == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    string naziv = konzorcij.NazivKonzorcij;
                    ctx.Remove(konzorcij); //oznacimo pon. za brisanje
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Konzorcij {naziv} uspješno obrisan.";
                    TempData[Constants.ErrorOccured] = false;

                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Pogreška prilikom brisanja konzorcija: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccured] = false;
                }
                return RedirectToAction(nameof(Index), new { page, sort, ascending });
            }
        }

        [HttpGet]
        public IActionResult Edit(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            var konzorcij = ctx.Konzorcijs.Where(p => p.IdKonzorcij == id).FirstOrDefault();
            if (konzorcij == null)
            {
                return NotFound($"Ne postoji konzorcij čiji je ID {id}");
            }
            else
            {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                return View(konzorcij);
            }

        }
        
        /// <summary>
        /// Metoda za uređivanje konzorcija
        /// </summary>
        /// <param name="id">Identifikator konzorcija koji se uređuje (id)</param>
        /// <param name="page">Broj stranice</param>
        /// <param name="sort">Atribut po kojemu se sortira</param>
        /// <param name="ascending">Oznaka je li sortiranje uzlazno ili silazno</param>
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            try
            {
                Konzorcij konzorcij = await ctx.Konzorcijs.FindAsync(id);
                if (konzorcij == null)
                {
                    return NotFound($"Ne postoji konzorcij čiji je ID {id}");
                }


                bool ok = await TryUpdateModelAsync<Konzorcij>(konzorcij, "", p => p.NazivKonzorcij);
                if (ok)
                {
                    ViewBag.Page = page;
                    ViewBag.Sort = sort;
                    ViewBag.Ascending = ascending;
                    try
                    {
                        await ctx.SaveChangesAsync();
                        TempData[Constants.Message] = $"PKonzorcij {konzorcij.NazivKonzorcij} uspješno ažuriran.";
                        TempData[Constants.ErrorOccured] = false;
                        return RedirectToAction(nameof(Index), new { page, sort, ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        return View(konzorcij);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Podatke nije moguće povezati");
                    return View(konzorcij);
                }

            }
            catch (Exception exc)
            {
                TempData[Constants.Message] = exc.CompleteExceptionMessage();
                TempData[Constants.ErrorOccured] = true;
                return RedirectToAction(nameof(Edit), new { id, page, sort, ascending });
            }

        }

   
        

        /// <summary>
        /// Prikazuje početnu stranicu
        /// </summary>
        /// <param name="page">Broj stranice</param>
        /// <param name="sort">Atribut po kojemu se sortira</param>
        /// <param name="ascending">Oznaka je li sortiranje uzlazno ili silazno</param>
        /// <returns></returns>
        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pageSize = appSettings.PageSize;
            //koliko ima podataka
            var query = ctx.Konzorcijs.AsNoTracking();
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
            System.Linq.Expressions.Expression<Func<Konzorcij, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = k => k.IdKonzorcij;
                    break;
                case 2:
                    orderSelector = k => k.NazivKonzorcij;
                    break;

            }

            //kad odaberemo neki od selectora, zelimo li sortirati silazno ili uzlazno 
            if (orderSelector != null)
            {
                //prosirujemo query
                query = ascending ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);
            }


            var konzorciji = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var model = new KonzorcijiViewModel
            {
                Konzorciji = konzorciji,
                PagingInfo = pagingInfo
            };

            return View(model);
        }
        
        public async Task<IActionResult> Show(int id, int page = 1, int sort = 1, bool ascending = true)
        {

            var clanovi = await ctx.ČlanoviKonzorcijas.Where(s => s.IdKonzorcij == id)
                .Select(s => new PonuditeljaViewModel { oibPonditelj = s.OibPonuditelj,
                    nazivPonuditelj = s.OibPonuditeljNavigation.NazivPonuditelj, adresaPonuditelj = s.OibPonuditeljNavigation.AdresaPonuditelj,
                    sjedistePonuditelj = s.OibPonuditeljNavigation.SjedištePonuditelj }).ToListAsync();


            var model = new KonzorcijaViewModel
            {
                idKonzorcij = id,
                Clanovi = clanovi
            };

            ViewBag.Page = page;
            ViewBag.Sort = sort;
            ViewBag.Ascending = ascending;

            return View(model);
        }
        
        
        
    }
}
