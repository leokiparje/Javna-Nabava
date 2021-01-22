using JavnaNabava.Models;
using JavnaNabava.ViewModels.JTable;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.Controllers.JTable
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LookupController : ControllerBase
    {
        private readonly RPPP06Context ctx;

        public LookupController(RPPP06Context ctx)
        {
            this.ctx = ctx;
        }

        [HttpGet]
        [HttpPost]
        public async Task<OptionsResult> Ponuditelji()
        {
            var options = await ctx.Ponuditeljs
                                   .OrderBy(d => d.NazivPonuditelj)
                                   .Select(d => new TextValue
                                   {
                                       DisplayText = d.NazivPonuditelj,
                                       Value = d.OibPonuditelj
                                   })
                                   .ToListAsync();
            return new OptionsResult(options);
        }

        [HttpGet]
        [HttpPost]
        public async Task<OptionsResult> Ovlaštenici()
        {
            var options = await ctx.Ovlašteniks
                                   .OrderBy(d => d.ImeOvlaštenik)
                                   .Select(d => new TextValue
                                   {
                                       DisplayText = d.ImeOvlaštenik,
                                       Value = ""+d.OibOvlaštenik
                                   })
                                   .ToListAsync();
            return new OptionsResult(options);
        }

        [HttpGet]
        [HttpPost]
        public async Task<OptionsResult> Naručitelji()
        {
            var options = await ctx.Naručiteljs
                                   .OrderBy(d => d.NazivNaručitelja)
                                   .Select(d => new TextValue
                                   {
                                       DisplayText = d.NazivNaručitelja,
                                       Value = d.OibNaručitelja
                                   })
                                   .ToListAsync();
            return new OptionsResult(options);
        }

        [HttpGet]
        [HttpPost]
        public async Task<OptionsResult> Kompetencije()
        {
            var options = await ctx.VrstaKompetencijes
                                   .OrderBy(d => d.NazivKompetencije)
                                   .Select(d => new TextValue
                                   {
                                       DisplayText = d.NazivKompetencije,
                                       Value = d.IdKompetencije.ToString()
                                   })
                                   .ToListAsync();
            return new OptionsResult(options);
        }
        [HttpGet]
        [HttpPost]
        public async Task<OptionsResult> VrsteStavki()
        {
            var options = await ctx.VrstaStavkes
                                   .OrderBy(d => d.NazivVrste)
                                   .Select(d => new TextValue
                                   {
                                       DisplayText = d.NazivVrste,
                                       Value = d.IdVrste.ToString()
                                   })
                                   .ToListAsync();
            return new OptionsResult(options);
        }
        
        
        [HttpGet]
        [HttpPost]
        public async Task<OptionsResult> Dokumenti()
        {
            var options = await ctx.Dokuments
                                   .OrderBy(d => d.Naslov)
                                   .Select(d => new TextValue
                                   {
                                       DisplayText = d.Naslov,
                                       Value = d.DokumentId.ToString()
                                   })
                                   .ToListAsync();
            return new OptionsResult(options);
        }
    }
}
