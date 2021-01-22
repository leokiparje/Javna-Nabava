using JavnaNabava.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.ViewModels
{
    public class KonzorcijiViewModel
    {
        public IEnumerable<Konzorcij> Konzorciji { get; set; }

        public List<Ponuditelj> Clanovi { get; internal set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
