using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JavnaNabava.Models;

namespace JavnaNabava.ViewModels
{
    public class KontaktiPonuditeljViewModel
    {
        public IEnumerable<KontaktPonuditeljViewModel> KontaktiPonuditelja { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
