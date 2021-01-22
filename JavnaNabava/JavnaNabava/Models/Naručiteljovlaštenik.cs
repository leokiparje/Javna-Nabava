using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.Models
{
    public class Naručiteljovlaštenik
    {
        public string OibNaručitelja { get; set; }
        public int OibOvlaštenik { get; set; }

        public virtual Naručitelj OibNaručiteljaNavigation { get; set; }
        public virtual Ovlaštenik OibOvlaštenikNavigation { get; set; }

    }
}
