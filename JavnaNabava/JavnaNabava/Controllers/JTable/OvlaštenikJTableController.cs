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
    [Route("jtable/ovlaštenik/[action]")]
    [TypeFilter(typeof(ErrorStatusTo200WithErrorMessage))]
    public class OvlaštenikJTableController : JTableController<ApiOvlaštenikController, int, OvlViewModel>
    {
        public OvlaštenikJTableController(ApiOvlaštenikController controller) : base(controller)
        {

        }

        [HttpPost]
        public async Task<JTableAjaxResult> Update([FromForm] OvlViewModel model)
        {
            return await base.UpdateItem(model.OibOvlaštenik, model);
        }

        [HttpPost]
        public async Task<JTableAjaxResult> Delete([FromForm] int oibOvlaštenik)
        {
            return await base.DeleteItem(oibOvlaštenik);
        }
    }
}
