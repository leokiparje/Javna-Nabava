using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
/// <summary>
/// Model za povezivanje zaposlenika s njegovim prilozima
/// </summary>
{
    public partial class ZaposlenikPrilog
    {
        public string OibZaposlenik { get; set; }
        public int IdPrilog { get; set; }
        public DateTime? VrijediOd { get; set; }
        public DateTime? VrijediDo { get; set; }
        public string LinkNaPrilog { get; set; }

        public virtual Prilog IdPrilogNavigation { get; set; }
        public virtual Zaposlenik OibZaposlenikNavigation { get; set; }
    }
}
