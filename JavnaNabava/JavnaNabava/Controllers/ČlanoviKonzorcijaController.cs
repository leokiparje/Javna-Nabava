using JavnaNabava.Extensions;
using JavnaNabava.Models;
using JavnaNabava.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.Controllers
{
    public class ČlanoviKonzorcijaController : Controller
    {
        private readonly RPPP06Context ctx;
        private readonly AppSettings appSettings;

        public ČlanoviKonzorcijaController(RPPP06Context ctx, IOptionsSnapshot<AppSettings> optionSnapshot)
        {
            this.ctx = ctx;
            appSettings = optionSnapshot.Value;
        }

        [HttpGet]
        public IActionResult Create()
        {
            PrepareDropDownLists();
            return View();
        }

        private void PrepareDropDownLists() { 

            var clanovi = ctx.Ponuditeljs.OrderBy(p => p.NazivPonuditelj).Select(p => new { p.NazivPonuditelj, p.OibPonuditelj }).ToList();
            ViewBag.Clanovi = new SelectList(clanovi, nameof(Ponuditelj.OibPonuditelj), nameof(Ponuditelj.NazivPonuditelj));
        
            var konzorciji = ctx.Konzorcijs.OrderBy(p => p.NazivKonzorcij).Select(p => new { p.NazivKonzorcij, p.IdKonzorcij }).ToList();
            ViewBag.Ponuditelji = new SelectList(konzorciji, nameof(Konzorcij.IdKonzorcij), nameof(Konzorcij.NazivKonzorcij));

            
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //medu podacima koje primamo mora biti i antiforgery token
        public IActionResult Create(ČlanoviKonzorcija clanoviKonzorcij) //podaci se prvo uzimaju iz forme, pa iz parametara rute i onda query stringa
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(clanoviKonzorcij);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Ponuditelj {clanoviKonzorcij.OibPonuditeljNavigation.NazivPonuditelj} uspješno dodan kao član konzorcija {clanoviKonzorcij.IdKonzorcijNavigation.NazivKonzorcij}.";
                    TempData[Constants.ErrorOccured] = false;

                    return RedirectToAction("Index");
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage()); //radimo proširenje za Exception
                    PrepareDropDownLists();
                    return View(clanoviKonzorcij); //ne vracamo praznu stranicu, nego postojece podatke
                }
            }
            else
            {
                PrepareDropDownLists();
                return View(clanoviKonzorcij);
            }

        }

        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pageSize = appSettings.PageSize;
            //koliko ima podataka
            var query = ctx.ČlanoviKonzorcijas.AsNoTracking();
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
            System.Linq.Expressions.Expression<Func<ČlanoviKonzorcija, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = p => p.OibPonuditeljNavigation.NazivPonuditelj;
                    break;
                case 2:
                    orderSelector = p => p.IdKonzorcijNavigation.NazivKonzorcij;
                    break;
                

            }

            //kad odaberemo neki od selectora, zelimo li sortirati silazno ili uzlazno 
            if (orderSelector != null)
            {
                //prosirujemo query
                query = ascending ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);
            }


            var članoviKonzorcij = query.Select(k => new ČlanKonzorcijaViewModel
            {
                NazivPonuditelj = k.OibPonuditeljNavigation.NazivPonuditelj,
                NazivKonzorcij = k.IdKonzorcijNavigation.NazivKonzorcij,
                
            }).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var model = new ČlanoviKonzorcijaViewModel
            {
                ČlanoviKonzorcija = članoviKonzorcij,
                PagingInfo = pagingInfo
            };
            return View(model);
        }
    }
}
