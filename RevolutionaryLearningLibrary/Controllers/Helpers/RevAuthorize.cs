using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace RevolutionaryLearningLibrary.Controllers
{
	public class RevAuthorizeAttribute : AuthorizeAttribute
	{
		private bool _requiresAdminAccess;

		public RevAuthorizeAttribute(bool requiresAdminAccess = false)
		{
			_requiresAdminAccess = requiresAdminAccess;
		}

		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			bool authorize = base.AuthorizeCore(httpContext);

			if (_requiresAdminAccess)
			{
				ClaimsIdentity identity = (ClaimsIdentity)httpContext.User.Identity;

				// check if they are an admin
				Claim adminClaim = identity.FindFirst(RevConstants.IS_ADMIN);

				authorize = (adminClaim != null && bool.Parse(adminClaim.Value));
			}

			return authorize;
		}
	}
}