using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    /// <summary>
    /// Model za dohvaćanje vrste postupka jednog natječaja
    /// </summary>
    public partial class VrstaPostupka
    {
        public VrstaPostupka()
        {
            Natječajs = new HashSet<Natječaj>();
        }

        public int IdVrstePostupka { get; set; }
        public string NazivVrste { get; set; }

        public virtual ICollection<Natječaj> Natječajs { get; set; }
    }
}
