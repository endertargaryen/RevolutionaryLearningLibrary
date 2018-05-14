using DTOCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RevolutionaryLearningLibrary.Controllers
{
	public class HomeController : Controller
	{
		public async Task<ActionResult> Index()
		{
			var users = await (new DataService()).CallList<UserDTO>("user");

			ViewBag.Users = users;

			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}