using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    /// <summary>
    /// Model za dohvaćanje podataka o stručnoj spremi zaposlenika
    /// </summary>
    public partial class VrstaStručneSpreme
    {
        public VrstaStručneSpreme()
        {
            Zaposleniks = new HashSet<Zaposlenik>();
        }

        public int IdStručneSpreme { get; set; }
        public string NazivStručneSpreme { get; set; }

        public virtual ICollection<Zaposlenik> Zaposleniks { get; set; }
    }
}
