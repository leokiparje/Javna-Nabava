using JavnaNabava.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.ViewModels
{
    public class PonuditeljiViewModel
    {
        public IEnumerable<Ponuditelj> Ponuditelji { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
