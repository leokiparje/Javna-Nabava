using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;
using JavnaNabava.ViewModels;
using JavnaNabava.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using JavnaNabava.Extensions.Selectors;
using System.Text.Json;
using JavnaNabava.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JavnaNabava.Controllers
{
    /// <summary>
    /// Controller za ovlaštenike
    /// </summary>
    public class OvlaštenikController : Controller
    {

        private readonly RPPP06Context ctx;
        private readonly AppSettings appSettings;
        private readonly ILogger<OvlaštenikController> logger;

        public OvlaštenikController(RPPP06Context ctx, IOptionsSnapshot<AppSettings> optionSnapshot, ILogger<OvlaštenikController> logger)
        {
            this.ctx = ctx;
            appSettings = optionSnapshot.Value;
            this.logger = logger;

        }
        
        [HttpGet]
        public IActionResult Create()
        {
            PrepareDropDownList();
            PrepareDropDownList2();
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Ovlaštenik ovlaštenik)
        {
            logger.LogTrace(JsonSerializer.Serialize(ovlaštenik));
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(ovlaštenik);
                    ctx.SaveChanges();
                    logger.LogInformation(new EventId(1000), $"Ovlaštenik {ovlaštenik.ImeOvlaštenik} dodan.");

                    TempData[Constants.Message] = $"Ovlaštenik {ovlaštenik.ImeOvlaštenik} dodan.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    PrepareDropDownList();
                    PrepareDropDownList2();
                    logger.LogError("Pogreška prilikom dodavanja novog ovlaštenika: {0}", exc.Message);
                    ModelState.AddModelError(string.Empty, exc.Message); //radimo proširenje za Exception
                    return View(ovlaštenik); //ne vracamo praznu stranicu, nego postojece podatke
                }
            }
            else
            {
                PrepareDropDownList();
                PrepareDropDownList2();
                return View(ovlaštenik);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            var ovlaštenik = ctx.Ovlašteniks.Where(o => o.OibOvlaštenik == id).SingleOrDefault();
            if (ovlaštenik == null)
            {
                return NotFound($"Oib {id} ne pripada niti jednom ovlašteniku");
            }
            else
            {
                PrepareDropDownList();
                PrepareDropDownList2();
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                return View(ovlaštenik);
            }
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            try
            {
                Ovlaštenik ovlaštenik = await ctx.Ovlašteniks.Where(o => o.OibOvlaštenik == id).SingleOrDefaultAsync();
                if (ovlaštenik == null)
                {
                    return NotFound($"Ovlaštenik s oibom {id} ne postoji");
                }


                bool potvrda = await TryUpdateModelAsync<Ovlaštenik>(ovlaštenik, "", o => o.ImeOvlaštenik, o => o.PrezimeOvlaštenik, o => o.IdPovjerenstva, o => o.OibNaručitelja);
                if (potvrda)
                {
                    ViewBag.Page = page;
                    ViewBag.Sort = sort;
                    ViewBag.Ascending = ascending;
                    try
                    {
                        await ctx.SaveChangesAsync();
                        TempData[Constants.Message] = $"Ovlaštenik {ovlaštenik.ImeOvlaštenik} uspješno ažuriran.";
                        TempData[Constants.ErrorOccurred] = false;
                        return RedirectToAction(nameof(Index), new { page, sort, ascending });
                    }
                    catch (Exception exc)
                    {
                        PrepareDropDownList();
                        PrepareDropDownList2();
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        return View(ovlaštenik);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Podatke nije moguće povezati");
                    PrepareDropDownList();
                    PrepareDropDownList2();
                    return View(ovlaštenik);
                }

            }
            catch (Exception exc)
            {
                TempData[Constants.Message] = exc.CompleteExceptionMessage();
                TempData[Constants.ErrorOccurred] = true;
                return RedirectToAction(nameof(Edit), new { id, page, sort, ascending });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int OibOvlaštenik, int page = 1, int sort = 1, bool ascending = true)
        {
            var ovlaštenik = ctx.Ovlašteniks.Find(OibOvlaštenik);
            if (ovlaštenik == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    string naziv = ovlaštenik.ImeOvlaštenik;
                    ctx.Remove(ovlaštenik);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Ovlaštenik {naziv} uspješno obrisan.";
                    TempData[Constants.ErrorOccurred] = false;

                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Pogreška prilikom brisanja ovlaštenika: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = false;
                }
                return RedirectToAction(nameof(Index), new { page, sort, ascending });
            }
        }

        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appSettings.PageSize;

            var query = ctx.Ovlašteniks.AsNoTracking();

            int count = query.Count();
            
            if (count == 0)
            {
                logger.LogInformation("Ne postoji nijedan ovlaštenik.");
                TempData[Constants.Message] = "Ne postoji niti jedan ovlaštenik.";
                TempData[Constants.ErrorOccurred] = false;
                return RedirectToAction(nameof(Create));
            }
            
            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                Sort = sort,
                Ascending = ascending,
                ItemsPerPage = pagesize,
                TotalItems = count
            };

            if (page < 1 || page > pagingInfo.TotalPages)
            {
                return RedirectToAction(nameof(Index), new { page = pagingInfo.TotalPages, sort, ascending });
            }

            query = query.ApplySort(sort, ascending);

            var ovlaštenici = query.Skip((page - 1) * pagesize).Take(pagesize).ToList();

            var model = new OvlaštenikViewModel
            {
                Ovlaštenici = ovlaštenici,
                PagingInfo = pagingInfo
            };

            return View(model);

        }
        private void PrepareDropDownList()
        {
            var povjerenstva = ctx.Povjerenstvos.OrderBy(p => p.IdPovjerenstva)
                .Select(p => new
                {
                    p.IdPovjerenstva
                }).ToList();
            ViewBag.povjerenstva = new SelectList(povjerenstva, nameof(Povjerenstvo.IdPovjerenstva), nameof(Povjerenstvo.IdPovjerenstva));
        }

        private void PrepareDropDownList2()
        {
            var naručitelji = ctx.Naručiteljs.OrderBy(p => p.OibNaručitelja)
                .Select(p => new
                {
                    p.OibNaručitelja
                }).ToList();
            ViewBag.naručitelji = new SelectList(naručitelji, nameof(Naručitelj.OibNaručitelja), nameof(Naručitelj.OibNaručitelja));
        }
    }
}
