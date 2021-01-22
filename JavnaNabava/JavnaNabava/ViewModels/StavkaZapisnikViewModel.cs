using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.ViewModels
{
    public class StavkaZapisnikViewModel
    {
        public int ZapisnikId { get; set; }
        public int idOdredba { get; set; }
        public int idStavka { get; set; }
        public string nazivOdredba { get; set; }
        public string ispravnostStavka { get; set; }
        public int cijenaKršenjaOdluke { get; set; }
        /*
        public string nazivStavke //autocomplete
        {
            get
            {
                return redniBrojStavke + " : " + naslovStavke;
            }
        }
        */
    }
}
