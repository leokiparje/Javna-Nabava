using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{/// <summary>
/// Klasa troškovnika
/// Sadrži Id troškovnika, evidencijski broj natječaja i poveznice na taj natječaj, uz to sadrži kolekciju koja spava troškovnik s stavkama
/// </summary>
    public partial class Troškovnik
    {
        public Troškovnik()
        {
            TroškovnikStavkas = new HashSet<TroškovnikStavka>();
        }

        public int TroškovnikId { get; set; }
        public int EvidBrojNatječaj { get; set; }

        public virtual Natječaj EvidBrojNatječajNavigation { get; set; }
        public virtual ICollection<TroškovnikStavka> TroškovnikStavkas { get; set; }
    }
}
