using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;

namespace JavnaNabava.ViewModels
{
    public class PrilogViewModel
    {

        public int idPrilog { get; set; }

        
        public string NazivPrilog { get; set; }

        public string LinkNaPrilog { get; set; }


        public PagingInfo PagingInfo { get; set; }
    }
}
