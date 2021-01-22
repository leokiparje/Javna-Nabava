using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    /// <summary>
    /// Model koji opisuje plan nabave sa svim potrebnim atributima
    /// </summary>
    public partial class PlanNabave
    {
        public PlanNabave()
        {
            Natječajs = new HashSet<Natječaj>();
        }

        public int EvidBrojPlan { get; set; }
        public DateTime TrajanjeNabave { get; set; }
        public string ŠifraCpv { get; set; }
        public string OibNaručitelja { get; set; }
        public int IdDjelatnosti { get; set; }
        public decimal VrijednostNabave { get; set; }

        public virtual VrstaDjelatnosti IdDjelatnostiNavigation { get; set; }
        public virtual Naručitelj OibNaručiteljaNavigation { get; set; }
        public virtual Cpv ŠifraCpvNavigation { get; set; }
        public virtual ICollection<Natječaj> Natječajs { get; set; }
    }
}
