using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class ČlanoviKonzorcija
    {
        public string OibPonuditelj { get; set; }
        public int IdKonzorcij { get; set; }
        public bool JeLiVoditelj { get; set; }

        public virtual Konzorcij IdKonzorcijNavigation { get; set; }
        public virtual Ponuditelj OibPonuditeljNavigation { get; set; }
    }
}
