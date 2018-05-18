using DTOCollection;
using RevolutionaryLearningDataAccess.Models;
using System;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace RevolutionaryLearningDataAccess.Controllers
{
    public class ItemController : ApiController
    {
		public DTOList<ItemDTO> GetItems()
		{
			DTOList<ItemDTO> retValue = null;

			using (var context = new DataAccessContext())
			{
				var items = (from i in context.Items
							 select i).ToList();

				retValue = retValue.DTOConvert(items);
			}

			return retValue;
		}

		[HttpPost]
		public DTOList<ItemDTO> GetItems(ItemFilterDTO filter)
		{
			DTOList<ItemDTO> retValue = null;

			using (var context = new DataAccessContext())
			{
				var items = (from n in context.Items
							 where n.CategoryId == filter.CategoryId &&
							 n.AssociatedUserId == null // no one has it checked out or requested
							 select n).ToList();

				if(filter.AgeGroupId > 0)
				{
					items = (from i in items
							 where i.Item2AgeGroup.Count(n => n.AgeGroupId == filter.AgeGroupId) > 0
							 select i).ToList();
				}

				if(filter.SubjectId > 0)
				{
					items = (from i in items
							 where i.Item2Subject.Count(n => n.SubjectId == filter.SubjectId) > 0
							 select i).ToList();
				}

				if(filter.LocationId > 0)
				{
					items = (from i in items
							 where i.LocationId == filter.LocationId
							 select i).ToList();
				}

				retValue = retValue.DTOConvert(items);
			}

			return retValue;
		}

		[HttpPost]
		public ResultDTO RequestItems(DTOList<ItemDTO> dtoItems)
		{
			ResultDTO result = null;

			try
			{
				using (var context = new DataAccessContext())
				{
					var idList = dtoItems.Select(n => n.ID);

					var items = (from n in context.Items
								 where idList.Contains(n.ID)
								 select n).ToList();

					items.ForEach(item =>
					{
						item.AssociatedUserId = dtoItems.Where(dto => dto.ID == item.ID).First().AssociatedUserId;
						item.RequestDate = DateTime.Now;
					});

					context.SaveChanges();

					result = new ResultDTO
					{
						StatusCode = (int)HttpStatusCode.OK,
						StatusCodeSuccess = true
					};
				}
			}
			catch (Exception ex)
			{
				result = new ResultDTO
				{
					StatusCode = (int)HttpStatusCode.InternalServerError,
					StatusCodeSuccess = false,
					StatusMessage = ex.ToString()
				};
			}

			return result;
		}

		public ItemDTO Get(int id)
		{
			ItemDTO retValue = null;

			using (var context = new DataAccessContext())
			{
				var item = (from n in context.Items
							where n.ID == id
							select n).FirstOrDefault();

				retValue = retValue.DTOConvert(item);
			}

			return retValue;
		}
    }
}
