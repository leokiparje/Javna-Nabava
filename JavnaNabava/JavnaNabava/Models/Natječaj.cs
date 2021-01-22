using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    /// <summary>
    ///Model koji opisuje natječaj sa svim potrebnim atributima i "navigation" funkcijama
    /// </summary>
    public partial class Natječaj
    {
        public Natječaj()
        {
            PonudaNatječajs = new HashSet<PonudaNatječaj>();
            Povjerenstvos = new HashSet<Povjerenstvo>();
        }

        public int EvidBrojNatječaj { get; set; }
        public string NazivNatječaja { get; set; }
        public decimal LimitNatječaja { get; set; }
        public DateTime? RokZaDostavu { get; set; }
        public DateTime? TrajanjeUgovora { get; set; }
        public int IdStatusaNatječaja { get; set; }
        public int EvidBrojPlan { get; set; }
        public int IdKriterija { get; set; }
        public int IdVrstePostupka { get; set; }

        public virtual PlanNabave EvidBrojPlanNavigation { get; set; }
        public virtual KriterijZaVrednovanje IdKriterijaNavigation { get; set; }
        public virtual StatusNatječaja IdStatusaNatječajaNavigation { get; set; }
        public virtual VrstaPostupka IdVrstePostupkaNavigation { get; set; }
        public virtual Troškovnik Troškovnik { get; set; }
        public virtual ICollection<PonudaNatječaj> PonudaNatječajs { get; set; }
        public virtual ICollection<Povjerenstvo> Povjerenstvos { get; set; }
    }
}
