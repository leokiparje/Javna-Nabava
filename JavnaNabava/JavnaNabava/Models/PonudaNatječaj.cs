using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class PonudaNatječaj
    {
        public int EvidBrojNatječaj { get; set; }
        public int PonudaId { get; set; }

        public virtual Natječaj EvidBrojNatječajNavigation { get; set; }
        public virtual Ponudum Ponuda { get; set; }
    }
}
