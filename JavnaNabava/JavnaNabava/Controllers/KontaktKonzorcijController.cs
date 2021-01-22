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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JavnaNabava.Controllers
{
    public class KontaktKonzorcijController : Controller
    {
        private readonly RPPP06Context ctx;
        private readonly AppSettings appSettings;

        public KontaktKonzorcijController(RPPP06Context ctx, IOptionsSnapshot<AppSettings> optionSnapshot)
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

        private void PrepareDropDownLists()
        {
            var konzorciji = ctx.Konzorcijs.OrderBy(p => p.NazivKonzorcij).Select(p => new { p.NazivKonzorcij, p.IdKonzorcij }).ToList();
            ViewBag.Ponuditelji = new SelectList(konzorciji, nameof(Konzorcij.IdKonzorcij), nameof(Konzorcij.NazivKonzorcij));

            var vrsteKontakta = ctx.VrstaKontakta.OrderBy(p => p.NazivVrstaKontakta).Select(p => new { p.NazivVrstaKontakta, p.IdVrstaKontakta }).ToList();
            ViewBag.VrsteKontakta = new SelectList(vrsteKontakta, nameof(VrstaKontaktum.IdVrstaKontakta), nameof(VrstaKontaktum.NazivVrstaKontakta));
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //medu podacima koje primamo mora biti i antiforgery token
        public IActionResult Create(KontaktKonzorcij kontaktKonzorcij) //podaci se prvo uzimaju iz forme, pa iz parametara rute i onda query stringa
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(kontaktKonzorcij);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Kontakt {kontaktKonzorcij.KontaktK} uspješno dodan.";
                    TempData[Constants.ErrorOccured] = false;

                    return RedirectToAction("Index");
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage()); //radimo proširenje za Exception
                    PrepareDropDownLists();
                    return View(kontaktKonzorcij); //ne vracamo praznu stranicu, nego postojece podatke
                }
            }
            else
            {
                PrepareDropDownLists();
                return View(kontaktKonzorcij);
            }

        }

        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pageSize = appSettings.PageSize;
            //koliko ima podataka
            var query = ctx.KontaktKonzorcijs.AsNoTracking();
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
            System.Linq.Expressions.Expression<Func<KontaktKonzorcij, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = p => p.KontaktK;
                    break;
                case 2:
                    orderSelector = p => p.IdKonzorcijNavigation.NazivKonzorcij;
                    break;
                case 3:
                    orderSelector = p => p.IdVrsteKontaktaNavigation.NazivVrstaKontakta;
                    break;

            }

            //kad odaberemo neki od selectora, zelimo li sortirati silazno ili uzlazno 
            if (orderSelector != null)
            {
                //prosirujemo query
                query = ascending ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);
            }


            var kontaktiKonzorcij = query.Select(k => new KontaktKonzorcijViewModel
            {
                KontaktK = k.KontaktK,
                NazivKonzorcij = k.IdKonzorcijNavigation.NazivKonzorcij,
                NazivVrstaKontakta = k.IdVrsteKontaktaNavigation.NazivVrstaKontakta
            }).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var model = new KontaktiKonzorcijViewModel
            {
                KontaktiKonzorcija = kontaktiKonzorcij,
                PagingInfo = pagingInfo
            };
            return View(model);
        }


    }
}
