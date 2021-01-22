using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class StatusNatječaja
    {
        public StatusNatječaja()
        {
            Natječajs = new HashSet<Natječaj>();
        }

        public int IdStatusaNatječaja { get; set; }
        public string NazivStatusaNatječaja { get; set; }

        public virtual ICollection<Natječaj> Natječajs { get; set; }
    }
}
