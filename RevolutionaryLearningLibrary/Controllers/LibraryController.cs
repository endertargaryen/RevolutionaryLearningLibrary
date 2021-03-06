﻿using DTOCollection;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RevolutionaryLearningLibrary.Controllers
{
	[RevAuthorize]
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

		public async Task<ActionResult> Index()
        {
			ClaimsIdentity identity = (ClaimsIdentity)HttpContext.User.Identity;

			// check if they are an admin
			Claim adminClaim = identity.FindFirst(RevConstants.IS_ADMIN);

			bool isAdmin = (adminClaim != null && bool.Parse(adminClaim.Value));

			var subjects = await DataService.CallDataServiceList<SubjectDTO>("Lookup", "GetSubjects");
			var ageGroups = await DataService.CallDataServiceList<AgeGroupDTO>("Lookup", "GetAgeGroups");
			var locations = await DataService.CallDataServiceList<LocationDTO>("Lookup", "GetLocations");
			var categories = await DataService.CallDataServiceList<CategoryDTO>("Lookup", "GetCategories");

			ViewBag.Subjects = JsonConvert.SerializeObject(subjects);
			ViewBag.AgeGroups = JsonConvert.SerializeObject(ageGroups);
			ViewBag.Locations = JsonConvert.SerializeObject(locations);
			ViewBag.Categories = JsonConvert.SerializeObject(categories);
			ViewBag.IsAdmin = (isAdmin ? 1 : 0);

            return View();
        }

		public async Task<ActionResult> GetItems(ItemFilterDTO filter)
		{
			var items = await DataService.CallDataServiceList<ItemDTO>("Item", "GetItems", filter);

			return new JsonResult { Data = items };
		}

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