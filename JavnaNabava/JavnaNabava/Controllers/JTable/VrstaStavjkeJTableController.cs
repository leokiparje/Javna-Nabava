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
    [Route("jtable/vrstaStavke/[action]")]
    [TypeFilter(typeof(ErrorStatusTo200WithErrorMessage))]
    public class VrstaStavkeJTableController : JTableController<ApiVrstaStavkiController, int, VrstaStavkeViewModel>
    {
        public VrstaStavkeJTableController(ApiVrstaStavkiController controller) : base(controller)
        {

        }

        [HttpPost]
        public async Task<JTableAjaxResult> Update([FromForm] VrstaStavkeViewModel model)
        {
            return await base.UpdateItem(model.IdVrste, model);
        }

        [HttpPost]
        public async Task<JTableAjaxResult> Delete([FromForm] int IdVrste)
        {
            return await base.DeleteItem(IdVrste);
        }
    }
}
