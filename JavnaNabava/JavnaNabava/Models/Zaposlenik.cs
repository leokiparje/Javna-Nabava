using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    /// <summary>
    /// Model na opis zaposlenika
    /// </summary>
    public partial class Zaposlenik
    {
        public Zaposlenik()
        {
            ZaposlenikPrilogs = new HashSet<ZaposlenikPrilog>();
        }

        public string OibZaposlenik { get; set; }
        public string ImeZaposlenik { get; set; }
        public string PrezimeZaposlenik { get; set; }
        public DateTime DatumRođenja { get; set; }
        public string MjestoPrebivališta { get; set; }
        public int IdKompetencije { get; set; }
        public int IdStručneSpreme { get; set; }
        public string OibPonuditelj { get; set; }

        public virtual VrstaKompetencije IdKompetencijeNavigation { get; set; }
        public virtual VrstaStručneSpreme IdStručneSpremeNavigation { get; set; }
        public virtual Ponuditelj OibPonuditeljNavigation { get; set; }
        public virtual ICollection<ZaposlenikPrilog> ZaposlenikPrilogs { get; set; }
    }
}
