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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JavnaNabava.Controllers
{
    public class TroškovnikController : Controller
    {
        private readonly RPPP06Context ctx;

        private readonly AppSettings appSettings;
        public TroškovnikController(RPPP06Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot)
        {
            this.ctx = ctx;
            appSettings = optionsSnapshot.Value;
        }
        /// <summary>
        /// vraća prikaz za stvaranje novog troškovnika i priprema listu natječaja za odabir
        /// </summary>
        /// <returns>View()</returns>
        [HttpGet]
        public IActionResult Create()
        {
            PrepareDropDownLists();
            return View();
        }

        /// <summary>
        /// Stvara novi troškovnik s podacima dobivenog modela i sprema ga u bazu
        /// </summary>
        /// <param name="troškovnik"></param>
        /// <returns>View(troškovnik)</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Troškovnik troškovnik)
        {   if (ModelState.IsValid)
                try
                {
                    //traži slijedeći key
                    var query = ctx;
                    int zadnji = query.Troškovniks.AsNoTracking().Count();
                    query = ctx;
                    var query2 = query.Troškovniks.AsQueryable();
                    int Id = query2.OrderBy(d => d.TroškovnikId).Skip(zadnji - 1).Select(d => d.TroškovnikId).First() + 1;
                    troškovnik.TroškovnikId = Id;

                    ctx.Add(troškovnik);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Troškovnik {troškovnik.TroškovnikId} uspješno dodan";
                    TempData[Constants.ErrorOccurred] = false;

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    PrepareDropDownLists();
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    return View(troškovnik);
                }
            else
            {
                PrepareDropDownLists();
                return View(troškovnik);
            }


        }
        /// <summary>
        /// šalje nas na show akciju koja će nam vratiti drugaciji view
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="ascending"></param>
        /// <returns>await Show(Id, page, sort, ascending, "Edit")</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int Id, int page = 1, int sort = 1, bool ascending = true)
        {
            PrepareDropDownLists();
            return await Show(Id, page, sort, ascending, "Edit");
            
        }
        /// <summary>
        /// Master-detail edit
        /// Mijena sve stavke troškovnika preko poslanog modela
        /// Mijenja evid broj natječaja troškovnika
        /// Brise stavke koje više nisu prisutne u modelu
        /// </summary>
        /// <param name="model"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="ascending"></param>
        /// <returns>return View(model)</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TroškovnikViewModel model, int page = 1, int sort = 1, bool ascending = true)
        {
            
            ViewBag.Page = page;
            ViewBag.Sort = sort;
            ViewBag.Ascending = ascending;
            
            if (ModelState.IsValid)
            {
                var troškovnik = await ctx.Troškovniks
                                        .Include(d => d.TroškovnikStavkas)
                                        .Where(d => d.TroškovnikId == model.TroškovnikId)
                                        .FirstOrDefaultAsync();
                if (troškovnik == null)
                {
                    return NotFound("Ne postoji troškovnik s id-om: " + model.TroškovnikId);
                }

                
                await SetPreviousAndNext(model.TroškovnikId);
                Console.WriteLine("troškovnik id :" + model.TroškovnikId + " naziv natzječaja:" + model.IdNatječaja + "stavke" );
               

                troškovnik.EvidBrojNatječaj = model.IdNatječaja;
                
                List<int> idStavki = model.Stavke
                                          .Where(s => s.IdStavke > 0)
                                          .Select(s => s.IdStavke)
                                          .ToList();
                foreach(var item in idStavki)
                {
                    Console.WriteLine("id stavki :" + idStavki);
                }
                //izbaci sve koje su nisu više u modelu
                throw new Exception("Kod ne radi, bacam grešku da ne pobriše podatke");
                //ctx.RemoveRange(troškovnik.TroškovnikStavkas.Where(s => !idStavki.Contains(s.IdStavke)));

                foreach (var stavka in model.Stavke)
                {
                    //ažuriraj postojeće i dodaj nove
                    StavkaUTroškovniku novaStavka; // potpuno nova ili dohvaćena ona koju treba izmijeniti
                    if (stavka.IdStavke > 0)
                    {
                        novaStavka = troškovnik.TroškovnikStavkas.Where(a=> a.IdStavke == stavka.IdStavke).Select(s => new StavkaUTroškovniku
                        {
                            IdStavke = s.IdStavkeNavigation.IdStavke,
                            NazivStavke = s.IdStavkeNavigation.NazivStavke,
                            TraženaKoličina = s.IdStavkeNavigation.TraženaKoličina,
                            IdVrste = s.IdStavkeNavigation.IdVrste,
                            DodatneInformacije = s.IdStavkeNavigation.DodatneInformacije
                        }).FirstOrDefault();
                    }
                    else
                    {
                        novaStavka = new StavkaUTroškovniku();
                        
                    }
                    //traži slijedeći key
                    if(novaStavka.IdStavke > 0)
                    {
                        int Id = novaStavka.IdStavke; 


                    }
                    else
                    {
                        var query = ctx;
                        int zadnji = query.StavkaUTroškovnikus.AsNoTracking().Count();
                        query = ctx;
                        var query2 = query.StavkaUTroškovnikus.AsQueryable();
                        int Id = query2.OrderBy(d => d.IdStavke).Skip(zadnji - 1).Select(d => d.IdStavke).First() + 1;
                        novaStavka.IdStavke = Id;
                        //dodaj vezu
                        var vezaTroškovnikStavka = new TroškovnikStavka
                        {
                            IdStavke = novaStavka.IdStavke,
                            TroškovnikId = troškovnik.TroškovnikId
                        };
                        ctx.Add(vezaTroškovnikStavka);
                    }
                    //trazim id vrste jer sam dobio naziv iz modela
                    var query3 = ctx;
                    int IdVr = query3.VrstaStavkes.AsNoTracking().Where(a => a.NazivVrste == stavka.NazivVrste).First().IdVrste;

                    //updataj ili napravi novu stavku
                    novaStavka.NazivStavke = stavka.NazivStavke;
                    novaStavka.TraženaKoličina = stavka.TraženaKoličina;
                    novaStavka.IdVrste = IdVr;
                    novaStavka.DodatneInformacije = stavka.DodatneInformacije;
                    
                    ctx.Add(novaStavka);

                }

                //eventualno umanji iznos za dodatni popust za kupca i sl... nešto što bi bilo poslovno pravilo
                try
                {

                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"troškovnik {troškovnik.TroškovnikId} uspješno ažuriran.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Edit), new
                    {
                        id = troškovnik.TroškovnikId,
                        page,
                        sort,
                        ascending
                    });

                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        /// <summary>
        /// Brise troškovnik i baze
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="ascending"></param>
        /// <returns>RedirectToAction(nameof(Index), new { page, sort, ascending })</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int Id, int page = 1, int sort = 1, bool ascending = true)
        {
            var troškovnik = ctx.Troškovniks.Find(Id);
            if(troškovnik == null)
            {
                TempData[Constants.Message] = "Ne postoji Troškovnik s oznakom: " + Id;
                TempData[Constants.ErrorOccurred] = true;
            }
            else
            {
                try
                {
                    int ID = troškovnik.TroškovnikId;
                    ctx.Remove(troškovnik);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Troškovnik {ID} uspješno obrisan";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Pogreška prilikom brisanja troškovnika: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }
                
            }
            return RedirectToAction(nameof(Index), new { page, sort, ascending });

        }
        /// <summary>
        /// Daje Master-detail prikaz troškovnika i njegovih stavki
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="ascending"></param>
        /// <param name="viewName"></param>
        /// <returns>View(viewName, troškovnik)</returns>
        public async Task<IActionResult> Show(int id, int page = 1, int sort = 1, bool ascending = true, string viewName = nameof(Show))
        {
            var troškovnik = await ctx.Troškovniks
                                    .Where(d => d.TroškovnikId == id)
                                    .Select(d => new TroškovnikViewModel
                                    {
                                        TroškovnikId = d.TroškovnikId,
                                        NazivNatječaja = d.EvidBrojNatječajNavigation.NazivNatječaja
                                    })
                                    .FirstOrDefaultAsync();
            if (troškovnik == null)
            {
                return NotFound($"Troškovnik {id} ne postoji");
            }
            else
            {
                //dohvat stavki za složeni prikaz
                var stavke = await ctx.TroškovnikStavkas
                                      .Where(s => s.TroškovnikId == troškovnik.TroškovnikId)
                                      .OrderBy(s => s.IdStavke)
                                      .Select(s => new StavkaUTroškovnikuViewModel
                                      {
                                          IdStavke = s.IdStavkeNavigation.IdStavke,
                                          NazivStavke = s.IdStavkeNavigation.NazivStavke,
                                          TraženaKoličina = s.IdStavkeNavigation.TraženaKoličina,
                                          NazivVrste = s.IdStavkeNavigation.IdVrsteNavigation.NazivVrste,
                                          IdVrste = s.IdStavkeNavigation.IdVrste,
                                          DodatneInformacije = s.IdStavkeNavigation.DodatneInformacije
                                      })
                                      .ToListAsync();
                troškovnik.Stavke = stavke;
                //povratak na pravu stranicu
                var query2 = ctx.Troškovniks;
                int broj = query2.Where(d => d.TroškovnikId < id).Count();
                page = 1 + broj / appSettings.PageSize;
                await SetPreviousAndNext(id);

                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                var queryZaId = ctx.Natječajs.Where(a => a.NazivNatječaja == troškovnik.NazivNatječaja).First().EvidBrojNatječaj;
                troškovnik.IdNatječaja = queryZaId;

                return View(viewName, troškovnik);
            }
        }
       /// <summary>
       /// Priprema Id prethodnoh i slijedećeg troškovnika od trenutnog preko Id-a
       /// </summary>
       /// <param name="position"></param>
       /// <returns></returns>
        private async Task SetPreviousAndNext(int position)
        {
            var query = ctx.Troškovniks.AsQueryable();
            //jako loša efikasnost
            var prev =  query.Where(d => d.TroškovnikId < position).Count();
            var next = query.Where(d => d.TroškovnikId > position).Count();
            Console.Write(next);
            if (prev > 0)
            {
                
                ViewBag.Previous = await query.OrderBy(d => d.TroškovnikId).Skip(prev - 1).Select(d => d.TroškovnikId).FirstAsync();
            }
            if (next > 0)
            {
                ViewBag.Next = await query.OrderBy(d => d.TroškovnikId).Skip(prev + 1).Select(d => d.TroškovnikId).FirstAsync();
            }
        }


        //funkcija za padajuću listu
        private void PrepareDropDownLists()
        {
            var natječaji = ctx.Natječajs.OrderBy(d => d.NazivNatječaja)
                                        .Select(d => new { d.NazivNatječaja, d.EvidBrojNatječaj })
                                        .ToList();
            ViewBag.natječaji = new SelectList(natječaji, nameof(Natječaj.EvidBrojNatječaj), nameof(Natječaj.NazivNatječaja));

            var vrste = ctx.VrstaStavkes.OrderBy(d => d.NazivVrste)
                                        .Select(d => new { d.NazivVrste, d.IdVrste })
                                        .ToList();
            ViewBag.Vrste = new SelectList(vrste, nameof(VrstaStavke.IdVrste), nameof(VrstaStavke.NazivVrste));
        }
        /// <summary>
        /// Dohvaća sve troškovnike i daje njihov prikaz
        /// Postavlja sortove
        /// </summary>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="ascending"></param>
        /// <returns>View(model)</returns>
        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pageSize = appSettings.PageSize;
            var query = ctx.Troškovniks.AsNoTracking();

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

            System.Linq.Expressions.Expression<Func<Troškovnik, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.TroškovnikId;
                    break;
                case 2:
                    orderSelector = d => d.EvidBrojNatječajNavigation.NazivNatječaja;
                    break;
            }
            if (orderSelector!=null)
            {
                query = ascending ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);
            }
            var troškovnik = query.Select(m => new TroškovnikViewModel
            {
                TroškovnikId = m.TroškovnikId,
                NazivNatječaja = m.EvidBrojNatječajNavigation.NazivNatječaja,
                Stavke = m.TroškovnikStavkas.Select(a => new StavkaUTroškovnikuViewModel
                {
                    IdStavke = a.IdStavke,
                    NazivStavke = a.IdStavkeNavigation.NazivStavke,
                    TraženaKoličina = a.IdStavkeNavigation.TraženaKoličina,
                    NazivVrste = a.IdStavkeNavigation.IdVrsteNavigation.NazivVrste,
                    DodatneInformacije = a.IdStavkeNavigation.DodatneInformacije,
                    TroškovnikId = a.IdStavkeNavigation.TroškovnikStavkas.First().TroškovnikId
                }).ToList()
            })
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();
            var model = new TroškovniciViewModel
            {
                Troškovnici = troškovnik,
            PagingInfo = pagingInfo
             
            };
            return View(model);
        }
    }
}
