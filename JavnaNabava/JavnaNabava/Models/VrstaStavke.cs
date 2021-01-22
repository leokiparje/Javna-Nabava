using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    /// <summary>
    /// Klasa Vrste stavke
    /// Sadrži NazivVrste, IdVrste i vezu na stavku u troškovniku
    /// </summary>
    public partial class VrstaStavke
    {
        public VrstaStavke()
        {
            StavkaUTroškovnikus = new HashSet<StavkaUTroškovniku>();
        }

        public string NazivVrste { get; set; }
        public int IdVrste { get; set; }

        public virtual ICollection<StavkaUTroškovniku> StavkaUTroškovnikus { get; set; }
    }
}
