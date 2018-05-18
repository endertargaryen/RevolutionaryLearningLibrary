using DTOCollection;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
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
		private ApplicationUserManager _userManager;

		public ApplicationUserManager UserManager
		{
			get
			{
				return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			private set
			{
				_userManager = value;
			}
		}

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
		public async Task<ActionResult> Index()
        {
			var subjects = await DataService.CallDataServiceList<SubjectDTO>("Lookup", "GetSubjects");
			var ageGroups = await DataService.CallDataServiceList<AgeGroupDTO>("Lookup", "GetAgeGroups");
			var locations = await DataService.CallDataServiceList<LocationDTO>("Lookup", "GetLocations");
			var categories = await DataService.CallDataServiceList<CategoryDTO>("Lookup", "GetCategories");

			ViewBag.Subjects = JsonConvert.SerializeObject(subjects);
			ViewBag.AgeGroups = JsonConvert.SerializeObject(ageGroups);
			ViewBag.Locations = JsonConvert.SerializeObject(locations);
			ViewBag.Categories = JsonConvert.SerializeObject(categories);

            return View();
        }

		public async Task<ActionResult> GetItems(ItemFilterDTO filter)
		{
			var items = await DataService.CallDataServiceList<ItemDTO>("Item", "GetItems", filter);

			return new JsonResult { Data = items };
		}

		[Authorize]
		public async Task<ActionResult> SubmitRequestList(DTOList<ItemDTO> items)
		{
			string identityUserId = User.Identity.GetUserId();

			int userId = Convert.ToInt32(identityUserId);

			items.ForEach(n =>
			{
				n.AssociatedUserId = userId;
			});

			ResultDTO result = null;

			try
			{
				result = await DataService.CallDataService<ResultDTO>("Item", "RequestItems", items);

				if (result.StatusCodeSuccess)
				{
					result.StatusMessage = "Your requests have been submitted to the admin.  Thank you!";
				}
			}
			catch(Exception ex)
			{
				DataService.SendEmail(ex.Message, ex.InnerException.ToString(), isError: true);
			}

			return new JsonResult { Data = result };
		}

	}
}