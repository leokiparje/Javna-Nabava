using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.ViewModels.JTable
{
    public class CreateResult : JTableAjaxResult
    {
        public CreateResult(object record) : base()
        {
            Record = record;
        }
        public object Record { get; set; }
    }
}
