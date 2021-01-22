using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class StavkaZapisnik
    {
        public int idStavke { get; set; }
        public int zapisnikID { get; set; }
        public int idOdredba { get; set; }
        public string ispravnostStavka { get; set; }
        public int cijenaKršenja { get; set; }

        public virtual OdredbaZapisnik OdredbaZapisnik { get; set; }
        public virtual Zapisnik Zapisnik { get; set; }
    }
}
