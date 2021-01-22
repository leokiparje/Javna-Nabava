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
using RPPP06Context = JavnaNabava.Models.RPPP06Context;

namespace JavnaNabava.Controllers
{
    /// <summary>
    ///Kontroler za dohvaćanje, uređivanje i brisanje naručitelja
    /// </summary>
    public class NaručiteljController : Controller
    {
        private readonly RPPP06Context context;
        private readonly AppSettings appSettings;
        public NaručiteljController(RPPP06Context context, IOptionsSnapshot<AppSettings> os) {
            this.context = context;
            appSettings = os.Value;
        }

        [HttpGet]

        public IActionResult Create() {
            return View();
        
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Naručitelj naručitelj) {
            if (ModelState.IsValid)
            {
                try
                {
                    context.Add(naručitelj);
                    context.SaveChanges();
                    TempData[Constants.Message] = $"Naručitelj broj: {naručitelj.OibNaručitelja} uspješno dodan!";
                    TempData[Constants.ErrorOccurred] = false;



                    return RedirectToAction(nameof(Index));

                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.CompleteExceptionMessage());
                    return View(naručitelj);

                }

            }
            else
            {
                return View(naručitelj);
            }

            
        }


        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pageSize = appSettings.PageSize;
            var query = context.Naručiteljs.AsNoTracking();

            int count = query.Count();

            var pagingInfo = new PagingInfo { 
                CurrentPage = page,
                Sort = sort,
                Ascending = ascending,
                ItemsPerPage = pageSize,
                TotalItems = count
           
            };

            if (page > pagingInfo.TotalPages) {
                return RedirectToAction(nameof(Index),new { 
                    page = pagingInfo.TotalPages, sort, ascending
                } );
            }

            System.Linq.Expressions.Expression<Func<Naručitelj, object>> orderSelector = null;
            switch (sort) { 
                case 1:
                        orderSelector = d => d.OibNaručitelja;
                    break;
                case 2:
                    orderSelector = d => d.NazivNaručitelja;
                    break;
            
                case 3:
                    orderSelector = d => d.AdresaNaručitelja;
                    break;
                case 4:
                    orderSelector = d => d.PoštanskiBrojNaručitelja;
                    break;
            


            }

            if (orderSelector != null) {
                query = ascending ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);
            
            }

            var naručitelji = query
                .Skip((page-1)*pageSize)
                .Take(pageSize)
                .ToList();

            var model = new NaručiteljViewModel
            {
                Naručitelji = (IEnumerable<Naručitelj>)naručitelji,
                PagingInfo = pagingInfo

            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete(string OibNaručitelja, int page = 1, int sort = 1, bool ascending = true) {
            var naručitelj = context.Naručiteljs.Find(OibNaručitelja);
            if (naručitelj == null)
            {
                return NotFound();
            }
            else {
                string naziv = naručitelj.NazivNaručitelja;
                try
                {
                    
                    context.Remove(naručitelj);
                    context.SaveChanges();
                    TempData[Constants.Message] = $"Naručitelj: {naziv} uspješno obrisan!";
                    TempData[Constants.ErrorOccurred] = false;



                    return RedirectToAction(nameof(Index), new { page, sort, ascending });
                }
                catch (Exception e)
                {

                    TempData[Constants.Message] = $"Naručitelj: {naziv} nije obrisan, dogodila se pogreška.";
                    TempData[Constants.ErrorOccurred] = true;
                }
                return RedirectToAction(nameof(Index));

            }
               

        }

        [HttpGet]
        public IActionResult Edit(string id, int page = 1, int sort = 1, bool ascending = true)
        {
            var naručitelj = context.Naručiteljs
                .Where(d => d.OibNaručitelja == id)
                .FirstOrDefault();
            if (naručitelj == null)
            {
                return NotFound($"Ne postoji naručitelj s oznakom {id}");
            }
            else
            {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                return View(naručitelj);
            }

        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, int page = 1, int sort = 1, bool ascending = true)
        {
            try
            {
                Naručitelj naručitelj= await context.Naručiteljs.FindAsync(id);
                if (naručitelj == null)
                {
                    return NotFound($"Ne postoji naručitelj čiji je id {id}");
                }


                bool ok = await TryUpdateModelAsync<Naručitelj>(naručitelj, "", d => d.NazivNaručitelja, d => d.AdresaNaručitelja, d => d.PoštanskiBrojNaručitelja);
                if (ok)
                {
                    ViewBag.Page = page;
                    ViewBag.Sort = sort;
                    ViewBag.Ascending = ascending;
                    try
                    {
                        await context.SaveChangesAsync();
                        TempData[Constants.Message] = $"Naručitelj {naručitelj.NazivNaručitelja} uspješno ažuriran.";
                        TempData[Constants.ErrorOccurred] = false;
                        return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        return View(naručitelj);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Podatke nije moguće povezati");
                    return View(naručitelj);
                }

            }
            catch (Exception exc)
            {
                TempData[Constants.Message] = exc.CompleteExceptionMessage();
                TempData[Constants.ErrorOccurred] = true;
                return RedirectToAction(nameof(Edit), id);
            }

        }

        public async Task<IActionResult> Show(string id, int page = 1, int sort = 1, bool ascending = true)
        {               
                var planovi = await context.PlanNabaves
                                      .Where(s => s.OibNaručitelja == id)
                                      .Select(s => new PlanNabaveaViewModel
                                      {
                                           evidBrojPlan = s.EvidBrojPlan,
                                           vrijednost = s.VrijednostNabave,
                                           CPV = s.ŠifraCpvNavigation.NazivCpv

                                        
                                      })
                                      .ToListAsync();



            var model = new NaručiteljaViewModel
            {
                oibNaručitelj = id,
                Planovi = planovi
               

            };

                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;


                return View(model);
            }
        }


    }

