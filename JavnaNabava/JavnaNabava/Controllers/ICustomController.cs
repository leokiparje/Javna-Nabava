using JavnaNabava.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnaNabava.Controllers
{
    public interface ICustomController<TKey, TModel>
    {
        public Task<int> Count([FromQuery] string filter);
        public Task<List<TModel>> GetAll([FromQuery] LoadParams loadParams);
        public Task<ActionResult<TModel>> Get(TKey oib);
        public Task<IActionResult> Create(TModel model);
        public Task<IActionResult> Update(TKey oib, TModel model);
        public Task<IActionResult> Delete(TKey oib);
    }
}
