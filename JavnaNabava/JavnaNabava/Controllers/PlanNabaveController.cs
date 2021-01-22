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
    public class PlanNabaveController : Controller
    {
        private readonly RPPP06Context context;
        private readonly AppSettings appSettings;
        private Random random;

        public PlanNabaveController(RPPP06Context context, IOptionsSnapshot<AppSettings> os) {
            this.context = context;
            appSettings = os.Value;
            random = new Random();
        }

        [HttpGet]

        public IActionResult Create() {
            PrepareDropdownLists();
            return View();
        
        }

        private int GenerateRandomNumber()
        {
            return random.Next(10000, 1000000000);
        }

        private void PrepareDropdownLists()
        {
            var CPVovi = context.Cpvs
                .OrderBy(s => s.NazivCpv)
                .Select(s => new { s.NazivCpv, s.ŠifraCpv })
                .ToList();
            ViewBag.CPV = new SelectList(CPVovi, nameof(Cpv.ŠifraCpv), nameof(Cpv.NazivCpv));

            var naručitelji = context.Naručiteljs
                .OrderBy(s=> s.NazivNaručitelja)
                .Select(s => new { s.NazivNaručitelja, s.OibNaručitelja })
                .ToList();
            ViewBag.Naručitelji = new SelectList(naručitelji, nameof(Naručitelj.OibNaručitelja), nameof(Naručitelj.NazivNaručitelja));

            var djelatnosti = context.VrstaDjelatnostis
                .OrderBy(s=> s.NazivDjelatnosti)
                .Select(s => new { s.NazivDjelatnosti, s.IdDjelatnosti })
                .ToList();
            ViewBag.Djelatnosti = new SelectList(djelatnosti, nameof(VrstaDjelatnosti.IdDjelatnosti), nameof(VrstaDjelatnosti.NazivDjelatnosti));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PlanNabave plan) {
            if (ModelState.IsValid)
            {
                try
                {
                    plan.EvidBrojPlan = GenerateRandomNumber();
                    context.Add(plan);
                    context.SaveChanges();
                    TempData[Constants.Message] = $"Plan nabave broj: {plan.EvidBrojPlan} uspješno dodan!";
                    TempData[Constants.ErrorOccurred] = false;



                    return RedirectToAction(nameof(Index));

                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.CompleteExceptionMessage());
                    PrepareDropdownLists();
                    return View(plan);

                }

            }
            else
            {
                PrepareDropdownLists();
                return View(plan);
            }

            
        }


        
        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pageSize = appSettings.PageSize;
            var query = context.PlanNabaves.AsNoTracking();
               

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

            System.Linq.Expressions.Expression<Func<PlanNabave, object>> orderSelector = null;
            switch (sort) { 
                case 1:
                        orderSelector = d => d.EvidBrojPlan;
                    break;
                case 2:
                    orderSelector = d => d.TrajanjeNabave;
                    break;
            
                case 3:
                    orderSelector = d => d.VrijednostNabave;
                    break;
                case 4:
                    orderSelector = d => d.ŠifraCpvNavigation.NazivCpv;
                    break;
                case 5:
                    orderSelector = d => d.OibNaručiteljaNavigation.NazivNaručitelja;
                    break;
                case 6:
                    orderSelector = d => d.IdDjelatnostiNavigation.NazivDjelatnosti;
                    break;
             


            }

            if (orderSelector != null) {
                query = ascending ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);
            
            }

            var planovi = query
                .Skip((page-1)*pageSize)
                .Take(pageSize)
                .Include(n => n.ŠifraCpvNavigation)
                .Include(n => n.OibNaručiteljaNavigation)
                .Include(n => n.IdDjelatnostiNavigation)
                .ToList();

       

            var model = new PlanNabaveViewModel
            {
                PlanoviNabave = planovi,
                PagingInfo = pagingInfo

            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete(int evidBrojPlan, int page = 1, int sort = 1, bool ascending = true)
        {
            var plan = context.PlanNabaves.Find(evidBrojPlan);
            if (plan == null)
            {
                return NotFound();
            }
            else
            {
                int broj = plan.EvidBrojPlan;
                try
                {

                    context.Remove(plan);
                    context.SaveChanges();
                    TempData[Constants.Message] = $"Plan nabave broj: {broj} uspješno obrisan!";
                    TempData[Constants.ErrorOccurred] = false;



                    return RedirectToAction(nameof(Index), new { page, sort, ascending });
                }
                catch (Exception)
                {

                    TempData[Constants.Message] = $"Plan nabave: {broj} nije obrisan, ne možemo obrisati plan koji je dodijeljen nekom natječaju.";
                    TempData[Constants.ErrorOccurred] = true;
                }
                return RedirectToAction(nameof(Index));

            }


        }
    }



    }

