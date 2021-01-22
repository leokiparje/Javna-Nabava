using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class VrstaKontaktum
    {
        public VrstaKontaktum()
        {
            KontaktKonzorcijs = new HashSet<KontaktKonzorcij>();
            KontaktPonuditeljs = new HashSet<KontaktPonuditelj>();
        }

        public int IdVrstaKontakta { get; set; }
        public string NazivVrstaKontakta { get; set; }

        public virtual ICollection<KontaktKonzorcij> KontaktKonzorcijs { get; set; }
        public virtual ICollection<KontaktPonuditelj> KontaktPonuditeljs { get; set; }
    }
}
