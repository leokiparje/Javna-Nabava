using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;

namespace JavnaNabava.ViewModels
{
    public class KontaktiKonzorcijViewModel
    {
        public IEnumerable<KontaktKonzorcijViewModel> KontaktiKonzorcija { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
