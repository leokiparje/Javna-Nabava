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

    /// <summary>
    ///Kontroler za dohvaćanje, uređivanje i brisanje vrsta kompetencija
    /// </summary>

    public class VrstaKompetencijeController : Controller
    {
        private readonly Random random = new Random();
        private readonly RPPP06Context context;
        private readonly AppSettings appSettings;
        private static int MAX = int.MaxValue;
        public VrstaKompetencijeController(RPPP06Context context, IOptionsSnapshot<AppSettings> os) {
            this.context = context;
            appSettings = os.Value;
            
        }

        [HttpGet]

        public IActionResult Create() {
            return View();
        
        }
        private int GenerateRandomNumber() {
            return random.Next(1000, MAX);
        }

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VrstaKompetencije kompetencija, int idKompetencije) {
            if (ModelState.IsValid)
            {
                try
                {
                    kompetencija.IdKompetencije = GenerateRandomNumber();

                    context.Add(kompetencija);
                    context.SaveChanges();
                    TempData[Constants.Message] = $"Kompetencija broj: {kompetencija.IdKompetencije} uspješno dodana!";
                    TempData[Constants.ErrorOccurred] = false;



                    return RedirectToAction(nameof(Index));

                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.CompleteExceptionMessage());
                    return View(kompetencija);

                }

            }
            else
            {
             return View(kompetencija);
            }

            
        }


        
        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pageSize = appSettings.PageSize;
            var query = context.VrstaKompetencijes.AsNoTracking();
               

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

            System.Linq.Expressions.Expression<Func<VrstaKompetencije, object>> orderSelector = null;
            switch (sort) { 
                case 1:
                        orderSelector = d => d.IdKompetencije;
                    break;
                case 2:
                    orderSelector = d => d.NazivKompetencije;
                    break;
            
               

            }

            if (orderSelector != null) {
                query = ascending ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);
            
            }

            var kompetencije = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
       

            var model = new VrstaKompetencijeViewModel
            {
                Kompetencije = kompetencije,
                PagingInfo = pagingInfo

            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete(int idKompetencije, int page = 1, int sort = 1, bool ascending = true) {
            var kompetencije = context.VrstaKompetencijes.Find(idKompetencije);
            if (kompetencije == null)
            {
                return NotFound();
            }
            else {
                string naziv = kompetencije.NazivKompetencije;
                try
                {
                    
                    context.Remove(kompetencije);
                    context.SaveChanges();
                    TempData[Constants.Message] = $"Kompetencija: {naziv} uspješno obrisana!";
                    TempData[Constants.ErrorOccurred] = false;



                    return RedirectToAction(nameof(Index), new { page, sort, ascending });
                }
                catch (Exception)
                {

                    TempData[Constants.Message] = $"Kompetencija: {naziv} nije obrisana, dogodila se pogreška.";
                    TempData[Constants.ErrorOccurred] = true;
                }
                return RedirectToAction(nameof(Index));

            }
               

        }


        [HttpGet]
        public IActionResult Edit(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            var kompetencije = context.VrstaKompetencijes
                .Where(d => d.IdKompetencije == id)
                .FirstOrDefault();
            if (kompetencije == null)
            {
                return NotFound($"Ne postoji kompetencija s oznakom {id}");
            }
            else
            {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                return View(kompetencije);
            }

        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            try
            {
                var kompetencije = await context.VrstaKompetencijes.FindAsync(id);
                if (kompetencije == null)
                {
                    return NotFound($"Ne postoji kompetencija čiji je id {id}");
                }


                bool ok = await TryUpdateModelAsync<VrstaKompetencije>(kompetencije, "", d => d.NazivKompetencije);
                if (ok)
                {
                    ViewBag.Page = page;
                    ViewBag.Sort = sort;
                    ViewBag.Ascending = ascending;
                    try
                    {
                        await context.SaveChangesAsync();
                        TempData[Constants.Message] = $"Kompetencija {kompetencije.NazivKompetencije} uspješno ažurirana.";
                        TempData[Constants.ErrorOccurred] = false;
                        return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        return View(kompetencije);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Podatke nije moguće povezati");
                    return View(kompetencije);
                }

            }
            catch (Exception exc)
            {
                TempData[Constants.Message] = exc.CompleteExceptionMessage();
                TempData[Constants.ErrorOccurred] = true;
                return RedirectToAction(nameof(Edit), id);
            }

        }



    }
}
