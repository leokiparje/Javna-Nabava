using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class KriterijZaVrednovanje
    {
        public KriterijZaVrednovanje()
        {
            Natječajs = new HashSet<Natječaj>();
        }

        public int IdKriterija { get; set; }
        public string NazivKriterija { get; set; }

        public virtual ICollection<Natječaj> Natječajs { get; set; }
    }
}
