using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class PonudaPonuditelj
    {
        public int PonudaId { get; set; }
        public string OibPonuditelj { get; set; }

        public virtual Ponuditelj OibPonuditeljNavigation { get; set; }
        public virtual Ponudum Ponuda { get; set; }
    }
}
