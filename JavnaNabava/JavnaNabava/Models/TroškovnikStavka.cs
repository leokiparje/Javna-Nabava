using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    /// <summary>
    /// Klasa Koja je veza između troškovnika is tavki
    /// Sadrži TroškovnikId, IdStavke, vezu na troškovnik i vezu na stavke
    /// </summary>
    public partial class TroškovnikStavka
    {
        public int TroškovnikId { get; set; }
        public int IdStavke { get; set; }

        public virtual StavkaUTroškovniku IdStavkeNavigation { get; set; }
        public virtual Troškovnik Troškovnik { get; set; }
    }
}
