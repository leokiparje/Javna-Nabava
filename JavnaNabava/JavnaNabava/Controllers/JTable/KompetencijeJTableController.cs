using JavnaNabava.ViewModels;
using JavnaNabava.ViewModels.JTable;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices.Util.ExceptionFilters;

namespace JavnaNabava.Controllers.JTable
{
    [Route("jtable/kompetencija/[action]")]
    [TypeFilter(typeof(ErrorStatusTo200WithErrorMessage))]
    public class KompetencijeJTableController : JTableController<ApiKompetencijeController, int, KompetencijaViewModel>
    {
        public KompetencijeJTableController(ApiKompetencijeController controller) : base(controller)
        {

        }

        [HttpPost]
        public async Task<JTableAjaxResult> Update([FromForm] KompetencijaViewModel model)
        {
            return await base.UpdateItem(model.IdKompetencije, model);
        }

        [HttpPost]
        public async Task<JTableAjaxResult> Delete([FromForm] int IdKompetencije)
        {
            return await base.DeleteItem(IdKompetencije);
        }
    }
}
