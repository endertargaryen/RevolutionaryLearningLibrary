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
using System.Security.Claims;
using Microsoft.AspNet.Identity.EntityFramework;

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

			if (!user.StatusCodeSuccess)
			{
				if (user.StatusCode == (int)HttpStatusCode.NoContent)
				{
					user.StatusMessage = "Invalid username or password";
				}
				else
				{
					user.StatusMessage = $"error: {user.StatusCode}";
				}

				return new JsonResult { Data = user };
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

			// add admin role
			appUser.Claims.Add(new IdentityUserClaim
			{
				UserId = appUser.Id,
				ClaimType = RevConstants.IS_ADMIN,
				ClaimValue = user.IsAdmin.ToString()
			});

			try
			{
				var validUserName = UserManager.FindByName(login.Email);

				var valid = SignInManager.UserManager.CheckPassword(appUser, login.Password);

				if(valid)
				{
					SignInManager.PasswordSignIn(login.Email, login.Password, true, false);
				}
			}
			catch(Exception ex)
			{
				var updateResult = await UserManager.UpdateAsync(appUser);
				SignInManager.SignIn(appUser, true, true);
			}

			return new JsonResult { Data = user };
		}

		#endregion
	}
}