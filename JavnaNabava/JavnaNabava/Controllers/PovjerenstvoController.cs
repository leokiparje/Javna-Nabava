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

    public class PovjerenstvoController : Controller
    {
        private readonly RPPP06Context ctx;
        private readonly AppSettings appSettings;
        private readonly ILogger<PovjerenstvoController> logger;

        public PovjerenstvoController(RPPP06Context ctx, IOptionsSnapshot<AppSettings> optionSnapshot, ILogger<PovjerenstvoController> logger)
        {
            this.ctx = ctx;
            appSettings = optionSnapshot.Value;
            this.logger = logger;

        }

        [HttpGet]
        public IActionResult Create()
        {
            PrepareDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Povjerenstvo povjerenstvo)
        {
            logger.LogTrace(JsonSerializer.Serialize(povjerenstvo));
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(povjerenstvo);
                    ctx.SaveChanges();
                    logger.LogInformation(new EventId(1000), $"Povjerenstvo {povjerenstvo.NazivPovjerenstva} dodano.");

                    TempData[Constants.Message] = $"Povjerenstvo {povjerenstvo.NazivPovjerenstva} dodano.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    PrepareDropDownList();
                    logger.LogError("Pogreška prilikom dodavanja novog povjerenstva: {0}", exc.CompleteExceptionMessage());
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage()); //radimo proširenje za Exception
                    return View(povjerenstvo); //ne vracamo praznu stranicu, nego postojece podatke
                }
            }
            else
            {
                PrepareDropDownList();
                return View(povjerenstvo);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            var povjerenstvo = ctx.Povjerenstvos.Where(p => p.IdPovjerenstva == id).SingleOrDefault();
            if (povjerenstvo == null)
            {
                return NotFound($"Id povjerenstva {id} ne pripada niti jednom povjerenstvu.");
            }
            else
            {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                PrepareDropDownList2();
                return View(povjerenstvo);
            }
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            try
            {
                Povjerenstvo povjerenstvo = await ctx.Povjerenstvos.Where(p => p.IdPovjerenstva == id).SingleOrDefaultAsync();
                if (povjerenstvo == null)
                {
                    return NotFound($"Povjerenstvo s id-om {id} ne postoji.");
                }


                bool potvrda = await TryUpdateModelAsync<Povjerenstvo>(povjerenstvo, "", p => p.NazivPovjerenstva, p => p.EvidBrojNatječaj);
                if (potvrda)
                {
                    ViewBag.Page = page;
                    ViewBag.Sort = sort;
                    ViewBag.Ascending = ascending;
                    try
                    {
                        await ctx.SaveChangesAsync();
                        TempData[Constants.Message] = $"Povjerenstvo {povjerenstvo.NazivPovjerenstva} uspješno ažurirano.";
                        TempData[Constants.ErrorOccurred] = false;
                        return RedirectToAction(nameof(Index), new { page, sort, ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        PrepareDropDownList2();
                        return View(povjerenstvo);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Podatke nije moguće povezati");
                    PrepareDropDownList2();
                    return View(povjerenstvo);
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
        public IActionResult Delete(int idPovjerenstva, int page = 1, int sort = 1, bool ascending = true)
        {
            var povjerenstvo = ctx.Povjerenstvos.Find(idPovjerenstva);
            if (povjerenstvo == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    string naziv = povjerenstvo.NazivPovjerenstva;
                    ctx.Remove(povjerenstvo);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Povjerenstvo {povjerenstvo.NazivPovjerenstva} je uspješno obrisano.";
                    TempData[Constants.ErrorOccurred] = false;

                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Pogreška prilikom brisanja povjerenstva: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = false;
                }
                return RedirectToAction(nameof(Index), new { page, sort, ascending });
            }
        }

        public async Task<IActionResult> Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appSettings.PageSize;

            var query = ctx.Povjerenstvos.AsNoTracking();

            int count = query.Count();

            if (count == 0)
            {
                logger.LogInformation("Ne postoji niti jedno povjerenstvo.");
                TempData[Constants.Message] = "Ne postoji niti jedno povjerenstvo.";
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

            var povjerenstva = await query
                .Select(p => new PovjerenstvoViewModel
                {
                    idPovjerenstva = p.IdPovjerenstva,
                    nazivPovjerenstva = p.NazivPovjerenstva,
                    nazivNatječaja = p.EvidBrojNatječajNavigation.NazivNatječaja
                })
                .Skip((page - 1) * pagesize).Take(pagesize).ToListAsync();

            var model = new PovjerenstvaViewModel
            {
                Povjerenstva = povjerenstva,
                PagingInfo = pagingInfo
            };
            return View(model);
        }

        private void PrepareDropDownList()
        {
            var natječaji = ctx.Natječajs.OrderBy(p => p.NazivNatječaja)
                .Select(p => new
                {
                    p.NazivNatječaja,
                    p.EvidBrojNatječaj
                }).ToList();
            ViewBag.natječaji = new SelectList(natječaji, nameof(Natječaj.EvidBrojNatječaj), nameof(Natječaj.NazivNatječaja));
        }

        private void PrepareDropDownList2()
        {
            var natječaji = ctx.Natječajs.OrderBy(p => p.EvidBrojNatječaj)
                .Select(p => new
                {
                    p.EvidBrojNatječaj
                }).ToList();
            ViewBag.natječaji = new SelectList(natječaji, nameof(Natječaj.EvidBrojNatječaj), nameof(Natječaj.EvidBrojNatječaj));
        }

    }
}
