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
    [Route("jtable/dokument/[action]")]
    [TypeFilter(typeof(ErrorStatusTo200WithErrorMessage))]
    public class DokumentJTableControler : JTableController<ApiDokumentController, int, DokumentaViewModel>
    {
        public DokumentJTableControler(ApiDokumentController controller) : base(controller)
        {

        }

        [HttpPost]
        public async Task<JTableAjaxResult> Update([FromForm] DokumentaViewModel model)
        {
            return await base.UpdateItem(model.DokumentId, model);
        }

        [HttpPost]
        public async Task<JTableAjaxResult> Delete([FromForm] int DokumentId)
        {
            return await base.DeleteItem(DokumentId);
        }
    }
}
