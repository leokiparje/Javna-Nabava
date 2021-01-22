using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.ViewModels
{
    public class DokumentaViewModel
    {
        public int DokumentId { get; set; }
        public string Naslov { get; set; }
        public string Vrsta { get; set; }
        public string Blob { get; set; }
        public int PonudaId { get; set; }
        public DateTime DatumPredaje { get; set; }

    }
}
