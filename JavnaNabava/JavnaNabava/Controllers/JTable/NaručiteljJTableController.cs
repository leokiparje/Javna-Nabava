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
    [Route("jtable/naručitelj/[action]")]
    [TypeFilter(typeof(ErrorStatusTo200WithErrorMessage))]
    public class NaručiteljJTableController : JTableController<ApiNaručiteljController, string, JedanNaručiteljViewModel>
    {
        public NaručiteljJTableController(ApiNaručiteljController controller) : base(controller)
        {

        }

        [HttpPost]
        public async Task<JTableAjaxResult> Update([FromForm] JedanNaručiteljViewModel model)
        {
            return await base.UpdateItem(model.oibNaručitelj, model);
        }

        [HttpPost]
        public async Task<JTableAjaxResult> Delete([FromForm] string oibNaručitelja)
        {
            return await base.DeleteItem(oibNaručitelja);
        }
    }
}
