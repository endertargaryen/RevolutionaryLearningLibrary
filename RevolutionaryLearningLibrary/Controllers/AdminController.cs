using DTOCollection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

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

		public async Task<ActionResult> SaveItem(ItemDTO item)
		{
			ResultDTO result = await DataService.CallDataService<ResultDTO>("Admin", "SaveItem", item);

			return new JsonResult
			{
				Data = result
			};
		}

		[HttpPost]
		public ActionResult UploadImage(HttpPostedFileBase httpFilePost)
		{
			if (Request.Files.Count > 0)
			{
				var file = Request.Files[0];

				if (file != null && file.ContentLength > 0)
				{
					var fileName = Path.GetFileName(file.FileName);
					var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
					file.SaveAs(path);

					Helpers.HelperMethods.ResizeImage(path);
				}
			}

			var routeValues = new RouteValueDictionary(new
			{
				action = "Index",
				Controller = "Admin",
				tab = 3 // send them back to add inventory
			});

			return new RedirectToRouteResult(routeValues);
		}
	}
}