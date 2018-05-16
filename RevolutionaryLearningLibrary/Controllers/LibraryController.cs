using DTOCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RevolutionaryLearningLibrary.Controllers
{
    public class LibraryController : Controller
    {
		#region Fields & Properties

		private DataService _dataService;

		public DataService DataService
		{
			get
			{
				if (_dataService == null)
				{
					_dataService = new DataService();
				}

				return _dataService;
			}
		}

		#endregion

		[Authorize]
		public ActionResult Index()
        {
            return View();
        }

		public async Task<ActionResult> GetItems(ItemFilterDTO filter)
		{
			var items = await DataService.CallDataServiceList<ItemDTO>("Item", "GetItems",
				postData: filter);

			return new JsonResult { Data = items };
		}
    }
}