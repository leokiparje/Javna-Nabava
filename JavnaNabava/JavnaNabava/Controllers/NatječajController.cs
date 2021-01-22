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
    public class NatječajController : Controller
    {
        private readonly RPPP06Context context;
        private readonly AppSettings appSettings;
        Random random;
        public NatječajController(RPPP06Context context, IOptionsSnapshot<AppSettings> os) {
            this.context = context;
            appSettings = os.Value;
            random = new Random();
        }

        [HttpGet]

        public IActionResult Create() {
            PrepareDropdownLists();
            return View();
        
        }

        private int GenerateRandomNumber() {
            return random.Next(10000, 1000000000);
                }

        private void PrepareDropdownLists()
        {
            var statusi = context.StatusNatječajas
                .Select(s => new { s.NazivStatusaNatječaja, s.IdStatusaNatječaja})
                .ToList();
            ViewBag.Statusi = new SelectList(statusi, nameof(StatusNatječaja.IdStatusaNatječaja), nameof(StatusNatječaja.NazivStatusaNatječaja));

            var planoviNabave = context.PlanNabaves
                .Select(s => new { s.EvidBrojPlan,s.VrijednostNabave})
                .ToList();
            ViewBag.PlanoviNabave = new SelectList(planoviNabave, nameof(PlanNabave.EvidBrojPlan), nameof(PlanNabave.EvidBrojPlan));

            var kriteriji = context.KriterijZaVrednovanjes
                .Select(s => new { s.NazivKriterija, s.IdKriterija })
                .ToList();
            ViewBag.Kriteriji = new SelectList(kriteriji, nameof(KriterijZaVrednovanje.IdKriterija), nameof(KriterijZaVrednovanje.NazivKriterija));

            var postupci = context.VrstaPostupkas
                .Select(s => new { s.NazivVrste, s.IdVrstePostupka })
                .ToList();
            ViewBag.VrstePostupaka = new SelectList(postupci, nameof(VrstaPostupka.IdVrstePostupka), nameof(VrstaPostupka.NazivVrste));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Natječaj natječaj) {
            if (ModelState.IsValid)
            {
                try
                {
                    natječaj.EvidBrojNatječaj = GenerateRandomNumber();
                    context.Add(natječaj);
                    context.SaveChanges();
                    TempData[Constants.Message] = $"Natječaj broj: {natječaj.EvidBrojNatječaj} uspješno dodan!";
                    TempData[Constants.ErrorOccurred] = false;



                    return RedirectToAction(nameof(Index));

                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.CompleteExceptionMessage());
                    PrepareDropdownLists();
                    return View(natječaj);

                }

            }
            else
            {
                PrepareDropdownLists();
                return View(natječaj);
            }

            
        }


        
        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pageSize = appSettings.PageSize;
            var query = context.Natječajs.AsNoTracking();
               

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

            System.Linq.Expressions.Expression<Func<Natječaj, object>> orderSelector = null;
            switch (sort) { 
                case 1:
                        orderSelector = d => d.EvidBrojNatječaj;
                    break;
                case 2:
                    orderSelector = d => d.NazivNatječaja;
                    break;
            
                case 3:
                    orderSelector = d => d.LimitNatječaja;
                    break;
                case 4:
                    orderSelector = d => d.RokZaDostavu;
                    break;
                case 5:
                    orderSelector = d => d.TrajanjeUgovora;
                    break;
                case 6:
                    orderSelector = d => d.IdStatusaNatječajaNavigation.NazivStatusaNatječaja;
                    break;
                case 7:
                    orderSelector = d => d.IdKriterijaNavigation.NazivKriterija;
                    break;
                case 8:
                    orderSelector = d => d.IdVrstePostupkaNavigation.NazivVrste;
                    break;


            }

            if (orderSelector != null) {
                query = ascending ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);
            
            }

            var natječaji = query
                .Skip((page-1)*pageSize)
                .Take(pageSize)
                .Include(n => n.IdStatusaNatječajaNavigation)
                .Include(n => n.IdKriterijaNavigation)
                .Include(n => n.IdVrstePostupkaNavigation)
                .ToList();

       

            var model = new NatječajViewModel
            {
                Natječaji = natječaji,
                PagingInfo = pagingInfo

            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            var natječaj = context.Natječajs.Find(id);
            if (natječaj == null)
            {
                return NotFound();
            }
            else
            {
                string naziv = natječaj.NazivNatječaja;
                try
                {

                    context.Remove(natječaj);
                    context.SaveChanges();
                    TempData[Constants.Message] = $"Natječaj: {naziv} uspješno obrisan!";
                    TempData[Constants.ErrorOccurred] = false;



                    return RedirectToAction(nameof(Index), new { page, sort, ascending });
                }
                catch (Exception e)
                {

                    TempData[Constants.Message] = $"Natječaj: {naziv} nije obrisan, ne možete obrisati natječaj o kojemu ovise druge tablice (troškovnici i sl.)";
                    TempData[Constants.ErrorOccurred] = true;
                }
                return RedirectToAction(nameof(Index));

            }


        }



    }
}
