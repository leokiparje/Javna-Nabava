using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    /// <summary>
    /// Model za dohvaćanje vrste kompetencije zaposlenika sa svim potrebnim atributima
    /// </summary>
    public partial class VrstaKompetencije
    {
        public VrstaKompetencije()
        {
            Zaposleniks = new HashSet<Zaposlenik>();
        }

        public int IdKompetencije { get; set; }
        public string NazivKompetencije { get; set; }
        public string SjedišteObrazovneUstanove { get; set; }
        public DateTime PočetakObrazovanja { get; set; }
        public DateTime KrajObrazovanja { get; set; }


        public virtual ICollection<Zaposlenik> Zaposleniks { get; set; }
    }
}
