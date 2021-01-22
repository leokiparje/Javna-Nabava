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

namespace JavnaNabava.Controllers
{
    public class VrstaStavkeController : Controller
    {
        private readonly RPPP06Context ctx;

        private readonly AppSettings appSettings;
        public VrstaStavkeController(RPPP06Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot)
        {
            this.ctx = ctx;
            appSettings = optionsSnapshot.Value;
        }
        /// <summary>
        /// vraća prikaz za stvaranje nove vrste stavki
        /// </summary>
        /// <returns>View()</returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// Stvara novu vrstu stavke s podacima dobivenog modela i sprema ga u bazu
        /// </summary>
        /// <param name="vrstaStavke"></param>
        /// <returns>View(vrstaStavke)</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VrstaStavke vrstaStavke)
        {
            
            if (ModelState.IsValid)
                try
                {
                    //tražim slijedeći key
                    var query = ctx;
                    int zadnji = query.VrstaStavkes.AsNoTracking().Count();
                    query = ctx;
                    var query2 = query.VrstaStavkes.AsQueryable();
                    int Id = query2.OrderBy(d => d.IdVrste).Skip(zadnji - 1).Select(d => d.IdVrste).First() + 1;
                    vrstaStavke.IdVrste = Id;

                    ctx.Add(vrstaStavke);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Vrsta stavke {vrstaStavke.NazivVrste} uspješno dodana";
                    TempData[Constants.ErrorOccurred] = false;

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    return View(vrstaStavke);
                }
            else
                return View(vrstaStavke);


        }
        /// <summary>
        /// Vraća prikaz za ažuriranje vrste stavke
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="ascending"></param>
        /// <returns>View(vrstaStavke)</returns>
        [HttpGet]
        public IActionResult Edit(int Id, int page = 1, int sort = 1, bool ascending = true)
        {
            var vrstaStavke = ctx.VrstaStavkes.AsNoTracking().Where(d => d.IdVrste == Id).SingleOrDefault();
            if (vrstaStavke == null)
            {
                return NotFound($"Ne postoji vrsta stavke s Id: {Id}");
            }
            else
            {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                return View(vrstaStavke);
            }

        }
        /// <summary>
        /// ažurira vrste stavke s podatcima iz poslanog modela
        /// vraća nas na popis vrstu stavki
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="ascending"></param>
        /// <returns>return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending })</returns>
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int Id, int page = 1, int sort = 1, bool ascending = true)
        {
            try
            {
                VrstaStavke vrstaStavke = await ctx.VrstaStavkes
                          .Where(d => d.IdVrste == Id)
                          .FirstOrDefaultAsync();
                if (vrstaStavke == null)
                {
                    return NotFound($"Ne postoji vrsta stavke s Id: {Id}");
                }

                if (await TryUpdateModelAsync<VrstaStavke>(vrstaStavke, "",d => d.NazivVrste
                 ))
                {
                    ViewBag.Page = page;
                    ViewBag.Sort = sort;
                    ViewBag.Ascending = ascending;
                    try
                    {


                        await ctx.SaveChangesAsync();
                        TempData[Constants.Message] = $"Vrsta stavke {Id} uspješno ažurirana";
                        TempData[Constants.ErrorOccurred] = false;
                        return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        return View(vrstaStavke);
                    }
                }
                else
                {

                    ModelState.AddModelError(string.Empty, "Podatke o vrsti stavke nije moguće povezati s forme");
                    return View(vrstaStavke);
                }
            }
            catch (Exception exc)
            {
                TempData[Constants.Message] = exc.CompleteExceptionMessage();
                TempData[Constants.ErrorOccurred] = true;
                return RedirectToAction(nameof(Edit), new { Id });
            }
        }

        /// <summary>
        /// brise stavku iz baze
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="ascending"></param>
        /// <returns>return RedirectToAction(nameof(Index), new { page, sort, ascending })</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int Id, int page = 1, int sort = 1, bool ascending = true)
        {
            var vrstaStavke = ctx.VrstaStavkes.Find(Id);
            if (vrstaStavke == null)
            {
                TempData[Constants.Message] = "Ne postoji vrsta stavke s Id: " + Id;
                TempData[Constants.ErrorOccurred] = true;
            }
            else
            {
                try
                {
                    string NAZIVVrste = vrstaStavke.NazivVrste;
                    ctx.Remove(vrstaStavke);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Vrsta stavke {NAZIVVrste} uspješno obrisana";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Pogreška prilikom brisanja vrste stavke: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }

            }
            return RedirectToAction(nameof(Index), new { page, sort, ascending });

        }
        /// <summary>
        /// Dohvaća sve vrste stavki i daje njihov prikaz
        /// Postavlja sortove
        /// </summary>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="ascending"></param>
        /// <returns>View(model)</returns>
        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pageSize = appSettings.PageSize;
            var query = ctx.VrstaStavkes.AsNoTracking();

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

            System.Linq.Expressions.Expression<Func<VrstaStavke, object>> orderSelector = null;
            switch (sort)
            {   
                case 1:
                    orderSelector = d => d.IdVrste;
                    break;
                case 2:
                    orderSelector = d => d.NazivVrste;
                    break;
            }
            if (orderSelector != null)
            {
                query = ascending ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);
            }
            var vrstaStavke = query
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();
            var model = new VrsteStavkiViewModel
            {
                VrsteStavki = vrstaStavke,
                PagingInfo = pagingInfo
            };
            return View(model);
        }
    }
}
