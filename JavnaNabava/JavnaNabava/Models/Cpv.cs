using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    /// <summary>
    ///Model koji opisuje CPV sa svim potrebnim atributima
    /// </summary>
    public partial class Cpv
    {
        public Cpv()
        {
            PlanNabaves = new HashSet<PlanNabave>();
        }

        public string ŠifraCpv { get; set; }
        public string NazivCpv { get; set; }

        public virtual ICollection<PlanNabave> PlanNabaves { get; set; }
    }
}
