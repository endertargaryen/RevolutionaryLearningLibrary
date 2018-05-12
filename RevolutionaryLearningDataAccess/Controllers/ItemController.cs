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
		public Item Get(int id)
		{
			Item item = null;

			using (var context = new DataAccessContext())
			{
				item = (from n in context.Items
						where n.ID == id
						select n).FirstOrDefault();
			}

			return item;
		}
    }
}
