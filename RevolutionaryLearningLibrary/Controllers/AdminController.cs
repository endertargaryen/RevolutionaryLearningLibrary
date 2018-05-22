using DTOCollection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RevolutionaryLearningLibrary.Controllers
{
	[RevAuthorize(true)]
    public class AdminController : Controller
    {
		#region Fields & Properties

		private DataService _dataService;

		private DataService DataService
		{
			get
			{
				if(_dataService == null)
				{
					_dataService = new DataService();
				}

				return _dataService;
			}
		}

		#endregion

		public ActionResult Index()
        {
			return View();
        }

		public async Task<ActionResult> GetInitialData()
		{
			var requests = await DataService.CallDataServiceList<ItemDTO>("Admin", "GetItemRequests");
			var checkouts = await DataService.CallDataServiceList<ItemDTO>("Admin", "GetItemCheckouts");

			AdminDataDTO dto = new AdminDataDTO
			{
				Checkouts = checkouts,
				Requests = requests
			};

			return new JsonResult
			{
				JsonRequestBehavior = JsonRequestBehavior.AllowGet,
				Data = dto
			};
		}

		public async Task<ActionResult> CheckItemIn(ItemDTO item)
		{
			var retValue = await DataService.CallDataService<ResultDTO>("Admin", "CheckItemIn", item);

			return new JsonResult
			{
				Data = retValue
			};
		}

		public async Task<ActionResult> CheckItemOut(ItemDTO item)
		{
			var retValue = await DataService.CallDataService<ResultDTO>("Admin", "CheckItemOut", item);

			return new JsonResult
			{
				Data = retValue
			};
		}
	}
}