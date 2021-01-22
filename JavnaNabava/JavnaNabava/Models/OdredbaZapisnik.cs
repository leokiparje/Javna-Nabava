using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.Models
{
    public class OdredbaZapisnik
    {
        public OdredbaZapisnik()
        {
            StavkaZapisniks = new HashSet<StavkaZapisnik>();
        }

        public int idOdredba { get; set; }
        public string nazivOdredba { get; set; }
        public string uvjetOdredba { get; set; }
        public string tekstOdredba { get; set; }

        public virtual ICollection<StavkaZapisnik> StavkaZapisniks { get; set; }
    }
}
