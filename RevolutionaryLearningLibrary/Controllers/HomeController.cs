using DTOCollection;
using Microsoft.AspNet.Identity;
using RevolutionaryLearningLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;

namespace RevolutionaryLearningLibrary.Controllers
{
	public class HomeController : Controller
	{
		#region Fields & Properties

		private DataService _dataService;
		private ApplicationSignInManager _signInManager;
		private ApplicationUserManager _userManager;

		public ApplicationSignInManager SignInManager
		{
			get
			{
				return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
			}
			private set
			{
				_signInManager = value;
			}
		}

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
				if(_dataService == null)
				{
					_dataService = new DataService();
				}

				return _dataService;
			}
		}

		#endregion

		#region Post Backs

		public ActionResult Index(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			ViewBag.LoginUrl = "/Home/LoginUser";
			ViewBag.RedirectUrl = "/Library/Index";

			return View();
		}

		public ActionResult Contact()
		{
			return View();
		}

		#endregion

		#region Async Methods

		[HttpPost]
		[AllowAnonymous]
		public async Task<ActionResult> LoginUser(LoginDTO login)
		{
			UserDTO user = await DataService.CallDataService<UserDTO>("user", "VerifyUser",
									postData: login);

			// if a user was found
			if (user.StatusCodeSuccess && user.StatusCode != (int)HttpStatusCode.NoContent)
			{
				var appUser = new ApplicationUser
				{
					Email = user.Email,
					UserName = user.Email,
					PasswordHash = user.Password,
					Id = user.Email
				};

				var exists = await UserManager.FindByEmailAsync(user.Email);

				// create the user
				if (exists == null)
				{
					var createResult = await UserManager.CreateAsync(appUser, login.Password);
				}

				var signInResult = await SignInManager.PasswordSignInAsync(login.Email, login.Password,
					true, false);

				// SignInManager is out of sync with the data service
				if (signInResult != SignInStatus.Success)
				{
					var deleteResult = await UserManager.DeleteAsync(appUser);

					if (deleteResult.Succeeded)
					{
						var createResult = await UserManager.CreateAsync(appUser, login.Password);
					}
				}
			}

			return new JsonResult { Data = user };
		}

		#endregion
	}
}