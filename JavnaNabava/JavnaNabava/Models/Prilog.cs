using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    /// <summary>
    /// Model koji opisuje prilog jednog zaposlenika
    /// </summary>
    public partial class Prilog
    {
        public Prilog()
        {
            ZaposlenikPrilogs = new HashSet<ZaposlenikPrilog>();
        }

        public int IdPrilog { get; set; }
        public string NazivPrilog { get; set; }

        public virtual ICollection<ZaposlenikPrilog> ZaposlenikPrilogs { get; set; }
    }
}
