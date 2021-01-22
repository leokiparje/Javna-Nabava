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
    public class KontaktPonuditeljController : Controller
    {
        private readonly RPPP06Context ctx;
        private readonly AppSettings appSettings;

        public KontaktPonuditeljController(RPPP06Context ctx, IOptionsSnapshot<AppSettings> optionSnapshot)
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
            var ponuditelji = ctx.Ponuditeljs.OrderBy(p => p.NazivPonuditelj).Select(p => new { p.NazivPonuditelj, p.OibPonuditelj }).ToList();
            ViewBag.Ponuditelji = new SelectList(ponuditelji, nameof(Ponuditelj.OibPonuditelj), nameof(Ponuditelj.NazivPonuditelj));

            var vrsteKontakta = ctx.VrstaKontakta.OrderBy(p => p.NazivVrstaKontakta).Select(p => new { p.NazivVrstaKontakta, p.IdVrstaKontakta }).ToList();
            ViewBag.VrsteKontakta = new SelectList(vrsteKontakta, nameof(VrstaKontaktum.IdVrstaKontakta), nameof(VrstaKontaktum.NazivVrstaKontakta));
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //medu podacima koje primamo mora biti i antiforgery token
        public IActionResult Create(KontaktPonuditelj kontaktPonuditelj) //podaci se prvo uzimaju iz forme, pa iz parametara rute i onda query stringa
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(kontaktPonuditelj);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Kontakt {kontaktPonuditelj.KontaktP} uspješno dodan.";
                    TempData[Constants.ErrorOccured] = false;

                    return RedirectToAction("Index");
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage()); //radimo proširenje za Exception
                    PrepareDropDownLists();
                    return View(kontaktPonuditelj); //ne vracamo praznu stranicu, nego postojece podatke
                }
            }
            else
            {
                PrepareDropDownLists();
                return View(kontaktPonuditelj);
            }

        }

        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pageSize = appSettings.PageSize;
            //koliko ima podataka
            var query = ctx.KontaktPonuditeljs.AsNoTracking();
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
            System.Linq.Expressions.Expression<Func<KontaktPonuditelj, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = p => p.KontaktP;
                    break;
                case 2:
                    orderSelector = p => p.OibPonuditeljNavigation.NazivPonuditelj;
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


            var kontaktiPonuditelj = query.Select(k => new KontaktPonuditeljViewModel
            {
                KontaktP = k.KontaktP,
                NazivPonuditelj = k.OibPonuditeljNavigation.NazivPonuditelj,
                NazivVrstaKontakta = k.IdVrsteKontaktaNavigation.NazivVrstaKontakta
            }).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var model = new KontaktiPonuditeljViewModel
            {
                KontaktiPonuditelja = kontaktiPonuditelj,
                PagingInfo = pagingInfo
            };
            return View(model);
        }

         [HttpPost]
         [ValidateAntiForgeryToken]
         public IActionResult Delete(string kontaktPon)
        {
            var kontakt = ctx.KontaktPonuditeljs.Where(p => p.KontaktP == kontaktPon).FirstOrDefault();
            //var kontakt = ctx.KontaktPonuditeljs.Find(oibPonuditelj, idVrsteKontakta);
            if(kontakt != null)
            {
                try
                {
                    //string naziv = kontakt.KontaktP;
                    ctx.Remove(kontakt);
                    ctx.SaveChanges();
                    var result = new
                    {
                        message = $"Kontakt {kontaktPon} obrisan",
                        successful = true
                    };
                    return Json(result);
                }
                catch(Exception exc)
                {
                    var result = new
                    {
                        message = $"Pogreška prilikom brisanja kontakta {exc.CompleteExceptionMessage()}",
                        successful = false
                    };
                    return Json(result);
                }
            }
            else
            {
                return NotFound($"Kontakt ne postoji");
            }
        }
        


    }
}
