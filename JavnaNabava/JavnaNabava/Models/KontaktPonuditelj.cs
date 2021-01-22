using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class KontaktPonuditelj
    {
        public string KontaktP { get; set; }
        public string OibPonuditelj { get; set; }
        public int IdVrsteKontakta { get; set; }

        public virtual VrstaKontaktum IdVrsteKontaktaNavigation { get; set; }
        public virtual Ponuditelj OibPonuditeljNavigation { get; set; }
    }
}
