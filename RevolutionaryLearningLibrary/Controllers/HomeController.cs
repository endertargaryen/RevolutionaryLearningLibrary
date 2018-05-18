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
using Microsoft.Owin.Security;

namespace RevolutionaryLearningLibrary.Controllers
{
	public class HomeController : Controller
	{
		#region Fields & Properties

		private DataService _dataService;
		private ApplicationSignInManager _signInManager;
		private ApplicationUserManager _userManager;

		private IAuthenticationManager AuthenticationManager
		{
			get
			{
				return HttpContext.GetOwinContext().Authentication;
			}
		}

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
		[ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{
			AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
			return RedirectToAction("Index", "Home");
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<ActionResult> LoginUser(LoginDTO login)
		{
			UserDTO user = await DataService.CallDataService<UserDTO>("user", "VerifyUser",
									postData: login);

			if (user.StatusCodeSuccess && user.StatusCode == (int)HttpStatusCode.NoContent)
			{
				user.StatusMessage = "Invalid username or password";

				return new JsonResult { Data = user };
			}
			else
			{
				user.StatusMessage = $"Internal Server Error - error code: {(HttpStatusCode)user.StatusCode}";
			}

			var appUser = new ApplicationUser
			{
				Email = user.Email,
				UserName = user.Email,
				PasswordHash = user.Password,
				Id = user.ID.ToString()
			};

			var exists = await UserManager.FindByEmailAsync(user.Email);

			// exists in the DB, but not the local UserManager, create the user
			if (exists == null)
			{
				var createResult = await UserManager.CreateAsync(appUser, login.Password);
			}
			else
			{
				// otherwise set the appUser to the one found in the manager
				appUser = exists;
			}

			var signInResult = await SignInManager.PasswordSignInAsync(login.Email, login.Password,
				true, false);

			// SignInManager is out of sync with the data service, update the manager
			if (signInResult != SignInStatus.Success)
			{
				var updateResult = await UserManager.UpdateAsync(appUser);

				var signInResult2 = await SignInManager.PasswordSignInAsync(login.Email, login.Password,
				true, false);

				if(signInResult2 != SignInStatus.Success)
				{
					user.StatusCodeSuccess = false;
					user.StatusCode = (int)HttpStatusCode.InternalServerError;
					user.StatusMessage = "Problem signing in.  Please try again later";
				}
			}

			return new JsonResult { Data = user };
		}

		#endregion
	}
}