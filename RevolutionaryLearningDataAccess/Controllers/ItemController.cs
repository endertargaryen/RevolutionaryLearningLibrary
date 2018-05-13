using Newtonsoft.Json;
using RevolutionaryLearningDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RevolutionaryLearningDataAccess.Controllers
{
    public class ItemController : ApiController
    {
		public string Get()
		{
			string retValue = String.Empty;

			using (var context = new DataAccessContext())
			{
				var items = (from n in context.Items
							 select n).ToList();

				retValue = JsonConvert.SerializeObject(items);
			}

			return retValue;
		}

		public string Get(int id)
		{
			string retValue = String.Empty;

			using (var context = new DataAccessContext())
			{
				var item = (from n in context.Items
							where n.ID == id
							select n).FirstOrDefault();

				retValue = JsonConvert.SerializeObject(item);
			}

			return retValue;
		}
    }
}
