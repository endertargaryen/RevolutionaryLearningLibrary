using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using RevolutionaryLearningLibrary.Models;
using DTOCollection;
using System.Net;

namespace RevolutionaryLearningLibrary.Controllers
{
	[Authorize]
	public class ManageController : Controller
	{
		private ApplicationUserManager _userManager;
		private DataService _dataService;

		public ApplicationUserManager UserManager
		{
			get
			{
				if (_userManager == null)
				{
					_userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
				}

				return _userManager;
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

		public ActionResult Index()
		{
			return View();
		}

		[Authorize]
		public ActionResult ChangePassword()
		{
			ViewBag.Title = "Change Password";
			

			return View();
		}

		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ChangeUserPassword(ChangePasswordDTO data)
		{
			string email = User.Identity.GetUserId();
			JsonResult retValue = new JsonResult();

			var result = await UserManager.ChangePasswordAsync(email,
				data.CurrentPassword, data.NewPassword);

			if (result.Succeeded)
			{
				data.Email = email;

				var dsResult = await DataService.CallDataService<ResultDTO>("user", "ChangePassword",
					postData: data);

				retValue.Data = dsResult;
			}
			else
			{
				ResultDTO retData = new ResultDTO
				{
					StatusCode = (int)HttpStatusCode.BadRequest,
					StatusCodeSuccess = false,
					StatusMessage = result.Errors.Count() > 0 ? result.Errors.First() : "Error while changing password"
				};

				retValue.Data = retData;
			}
			return retValue;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && _userManager != null)
			{
				_userManager.Dispose();
				_userManager = null;
			}

			base.Dispose(disposing);
		}
	}
}