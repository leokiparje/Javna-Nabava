using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    /// <summary>
    /// Klasa Stavke u troškovniku
    /// Sadrži IdStavke, NazivStavke, TraženaKoličina, IdVrste, DodatneInformacije, vezu na vrstu stavke , vezu na troškovnike i vezu na ponude
    /// </summary>
    public partial class StavkaUTroškovniku
    {
        public StavkaUTroškovniku()
        {
            PonudaStavkes = new HashSet<PonudaStavke>();
            TroškovnikStavkas = new HashSet<TroškovnikStavka>();
        }

        public int IdStavke { get; set; }
        public string NazivStavke { get; set; }
        public int TraženaKoličina { get; set; }
        public int IdVrste { get; set; }
        public string DodatneInformacije { get; set; }

        public virtual VrstaStavke IdVrsteNavigation { get; set; }
        public virtual ICollection<PonudaStavke> PonudaStavkes { get; set; }
        public virtual ICollection<TroškovnikStavka> TroškovnikStavkas { get; set; }
    }
}
