using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class PonDokumenti
    {
        public int PonudaId { get; set; }
        public int DokumentId { get; set; }

        public virtual Dokument Dokument { get; set; }
        public virtual Ponudum Ponuda { get; set; }
    }
}
