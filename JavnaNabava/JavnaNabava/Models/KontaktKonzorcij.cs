using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class KontaktKonzorcij
    {
        public string KontaktK { get; set; }
        public int IdKonzorcij { get; set; }
        public int IdVrsteKontakta { get; set; }

        public virtual Konzorcij IdKonzorcijNavigation { get; set; }
        public virtual VrstaKontaktum IdVrsteKontaktaNavigation { get; set; }
    }
}
