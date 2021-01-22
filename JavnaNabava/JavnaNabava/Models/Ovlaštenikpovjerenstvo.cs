using System;
using System.Collections.Generic;

#nullable disable

namespace JavnaNabava.Models
{
    public partial class Ovlaštenikpovjerenstvo
    {
        public int IdPovjerenstva { get; set; }
        public int OibOvlaštenik { get; set; }

        public virtual Povjerenstvo IdPovjerenstvaNavigation { get; set; }
        public virtual Ovlaštenik OibOvlaštenikNavigation { get; set; }
    }
}
