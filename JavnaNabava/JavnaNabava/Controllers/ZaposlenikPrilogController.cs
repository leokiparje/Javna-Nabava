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
    public class ZaposlenikPrilogController : Controller
    {
        private readonly RPPP06Context context;
        private readonly AppSettings appSettings;
        public ZaposlenikPrilogController(RPPP06Context context, IOptionsSnapshot<AppSettings> os) {
            this.context = context;
            appSettings = os.Value;
        }


        private void PrepareDropdownLists()
        {
            var nazivi = context.Prilogs
                .Select(p => new { p.NazivPrilog, p.IdPrilog })
                .ToList();
            ViewBag.Nazivi = new SelectList(nazivi, nameof(Prilog.IdPrilog), nameof(Prilog.NazivPrilog));

            var oibovi = context.Zaposleniks
                 .Select(s => new { s.OibZaposlenik, s.PrezimeZaposlenik })
                 .ToList();
            ViewBag.OibZaposlenik = new SelectList(oibovi, nameof(Zaposlenik.OibZaposlenik), nameof(Zaposlenik.OibZaposlenik));


        }

        [HttpGet]

        public IActionResult Create() {
            PrepareDropdownLists();
            return View();
        
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ZaposlenikPrilog zaposlenikPrilog) {
            if (ModelState.IsValid)
            {
                try
                {
                    context.Add(zaposlenikPrilog);
                    context.SaveChanges();
                    TempData[Constants.Message] = $"Prilog broj: {zaposlenikPrilog.IdPrilog} uspješno dodan zaposleniku !";
                    TempData[Constants.ErrorOccurred] = false;



                    return RedirectToAction(nameof(Index));

                }
                catch (Exception e)
                {
                    
                    ModelState.AddModelError(string.Empty, e.CompleteExceptionMessage());
                    PrepareDropdownLists();
                    return View(zaposlenikPrilog);

                }

            }
            else
            {
                PrepareDropdownLists();
                return View(zaposlenikPrilog);
            }

            
        }


        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pageSize = appSettings.PageSize;
            var query = context.ZaposlenikPrilogs.AsNoTracking();

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

            System.Linq.Expressions.Expression<Func<ZaposlenikPrilog, object>> orderSelector = null;
            switch (sort) { 
                case 1:
                        orderSelector = z => z.IdPrilogNavigation.IdPrilog;
                    break;
                case 2:
                    orderSelector = z => z.IdPrilogNavigation.NazivPrilog;
                    break;
                case 3:
                    orderSelector = z => z.OibZaposlenik;
                    break;

                case 4:
                    orderSelector = z => z.OibZaposlenikNavigation.ImeZaposlenik;
                    break;
                case 5:
                    orderSelector = d => d.OibZaposlenikNavigation.PrezimeZaposlenik;
                    break;
                case 6:
                    orderSelector = z => z.VrijediOd;
                    break;
                case 7:
                    orderSelector = z => z.VrijediDo;
                    break;
                case 8:
                    orderSelector = z => z.LinkNaPrilog;
                    break;



            }

            if (orderSelector != null) {
                query = ascending ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);
            
            }

            var zaposleniciPrilozi = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(z=>z.OibZaposlenikNavigation)
                .Include(z=>z.IdPrilogNavigation)
                .Take(pageSize)
                .ToList();

            var model = new ZaposlenikPrilogViewModel
            {
                ZaposleniciPrilozi = zaposleniciPrilozi,
                PagingInfo = pagingInfo

            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete(ZaposlenikPrilog zp, int page = 1, int sort = 1, bool ascending = true) {
            Console.WriteLine(zp.OibZaposlenik);
            Console.WriteLine(zp.IdPrilog);

            var zaposlenikPrilog = context.ZaposlenikPrilogs.Find(zp.OibZaposlenik, zp.IdPrilog);
            if (zaposlenikPrilog == null)
            {
                TempData[Constants.Message] = $"Prilog nije pronađen.";
                TempData[Constants.ErrorOccurred] = true;
            
            return RedirectToAction(nameof(Index));
        }
            else {
                
           
                try
                {
                    
                    context.Remove(zaposlenikPrilog);
                    context.SaveChanges();
                    TempData[Constants.Message] = $"Prilog uspješno obrisan iz popisa priloga zaposlenika";
                    TempData[Constants.ErrorOccurred] = false;



                    return RedirectToAction(nameof(Index), new { page, sort, ascending });
                }
                catch (Exception e)
                {

                    TempData[Constants.Message] = $"Prilog nije obrisan, dogodila se pogreška.";
                    TempData[Constants.ErrorOccurred] = true;
                }
                return RedirectToAction(nameof(Index));

            }
               

        }


        }


    }

