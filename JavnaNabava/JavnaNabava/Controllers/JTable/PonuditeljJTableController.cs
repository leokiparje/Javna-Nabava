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
    [Route("jtable/ponuditelj/[action]")]
    [TypeFilter(typeof(ErrorStatusTo200WithErrorMessage))]
    public class PonuditeljJTableController : JTableController<ApiPonuditeljController, string, PonuditeljViewModel>
    {
        public PonuditeljJTableController(ApiPonuditeljController controller) : base(controller)
        {

        }

        [HttpPost]
        public async Task<JTableAjaxResult> Update([FromForm] PonuditeljViewModel model)
        {
            return await base.UpdateItem(model.OibPonuditelj, model);
        }

        [HttpPost]
        public async Task<JTableAjaxResult> Delete([FromForm] string oibPonuditelj)
        {
            return await base.DeleteItem(oibPonuditelj);
        }
    }
}
