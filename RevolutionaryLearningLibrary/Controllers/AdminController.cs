using DTOCollection;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
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
		private ApplicationSignInManager _signInManager;

		private ApplicationSignInManager SignInManager
		{
			get
			{
				if(_signInManager == null)
				{
					_signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
				}

				return _signInManager;
			}
		}

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

		public async Task<ActionResult> Index()
        {
			var subjects = await DataService.CallDataServiceList<SubjectDTO>("Lookup", "GetSubjects");
			var ageGroups = await DataService.CallDataServiceList<AgeGroupDTO>("Lookup", "GetAgeGroups");
			var locations = await DataService.CallDataServiceList<LocationDTO>("Lookup", "GetLocations");
			var categories = await DataService.CallDataServiceList<CategoryDTO>("Lookup", "GetCategories");
			var subLocations = await DataService.CallDataServiceList<SubLocationDTO>("Lookup", "GetSubLocations");

			ViewBag.Subjects = JsonConvert.SerializeObject(subjects);
			ViewBag.AgeGroups = JsonConvert.SerializeObject(ageGroups);
			ViewBag.Locations = JsonConvert.SerializeObject(locations);
			ViewBag.SubLocations = JsonConvert.SerializeObject(subLocations);
			ViewBag.Categories = JsonConvert.SerializeObject(categories);

			return View();
        }

		[HttpPost]
		public async Task<ActionResult> GetInitialData(IdDTO idDto)
		{
			var requests = await DataService.CallDataServiceList<ItemDTO>("Admin", "GetItemRequests");
			var checkouts = await DataService.CallDataServiceList<ItemDTO>("Admin", "GetItemCheckouts");
			var item = new ItemDTO();

			if(idDto.ID > 0)
			{
				item = await DataService.CallDataService<ItemDTO>("Item", "Get", idDto.ID);
			}

			AdminDataDTO dto = new AdminDataDTO
			{
				Checkouts = checkouts,
				Requests = requests,
				Item = item
			};

			return new JsonResult
			{
				JsonRequestBehavior = JsonRequestBehavior.AllowGet,
				Data = dto
			};
		}
		
		public async Task<ActionResult> CheckItemIn(ItemDTO item)
		{
			int authId = HelperMethods.GetAuthenticationId(User);

			item.UserIdForAuth = authId;

			var retValue = await DataService.CallDataService<ResultDTO>("Admin", "CheckItemIn", item);

			return new JsonResult
			{
				Data = retValue
			};
		}

		public async Task<ActionResult> CheckItemOut(ItemDTO item)
		{
			int authId = HelperMethods.GetAuthenticationId(User);

			item.UserIdForAuth = authId;

			var retValue = await DataService.CallDataService<ResultDTO>("Admin", "CheckItemOut", item);

			return new JsonResult
			{
				Data = retValue
			};
		}

		public async Task<ActionResult> SaveItem(ItemDTO item)
		{
			int authId = HelperMethods.GetAuthenticationId(User);

			item.UserIdForAuth = authId;

			ResultDTO result = await DataService.CallDataService<ResultDTO>("Item", "SaveItem", item);

			return new JsonResult
			{
				Data = result
			};
		}

		public async Task<ActionResult> DeleteItem(ItemDTO item)
		{
			int authId = HelperMethods.GetAuthenticationId(User);

			ResultDTO result = await DataService.CallDataService<ResultDTO>("Item", "DeleteItem",
				new IdDTO
				{
					ID = item.ID,
					UserIdForAuth = authId
				});

			return new JsonResult { Data = result };
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

					HelperMethods.ResizeImage(path);
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