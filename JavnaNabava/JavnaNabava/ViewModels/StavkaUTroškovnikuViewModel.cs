using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;

namespace JavnaNabava.ViewModels
{
    /// <summary>
    /// View model stavke koji dodatno sadrži naziv vrste i id svog troškovnika
    /// </summary>
    public class StavkaUTroškovnikuViewModel
    {

        public int IdStavke { get; set; }

        
        public string NazivStavke { get; set; }

        
        public int TraženaKoličina { get; set; }

        public string NazivVrste { get; set; }

        public string DodatneInformacije { get; set; }

        public int TroškovnikId { get; set; }

        public int IdVrste { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
