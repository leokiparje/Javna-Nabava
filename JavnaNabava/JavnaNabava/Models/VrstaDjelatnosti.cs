using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    /// <summary>
    ///Model za dohvaćanje vrste djelatnosti vezane za plan nabave
    /// </summary>
    public partial class VrstaDjelatnosti
    {
        public VrstaDjelatnosti()
        {
            PlanNabaves = new HashSet<PlanNabave>();
        }

        public int IdDjelatnosti { get; set; }
        public string NazivDjelatnosti { get; set; }

        public virtual ICollection<PlanNabave> PlanNabaves { get; set; }
    }
}
