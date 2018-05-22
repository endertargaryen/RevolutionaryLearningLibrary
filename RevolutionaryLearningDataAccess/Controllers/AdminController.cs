using DTOCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RevolutionaryLearningDataAccess;
using RevolutionaryLearningDataAccess.Models;

namespace RevolutionaryLearningDataAccess.Controllers
{
    public class AdminController : ApiController
    {
		public DTOList<ItemDTO> GetItemRequests()
		{
			DTOList<ItemDTO> retValue = null;

			using(var context = new DataAccessContext())
			{
				var list = (from n in context.Items
							where n.RequestDate != null
							select n).ToList();

				retValue = retValue.DTOConvert(list);
			}

			return retValue;
		}

		public DTOList<ItemDTO> GetItemCheckouts()
		{
			DTOList<ItemDTO> retValue = null;

			using (var context = new DataAccessContext())
			{
				var list = (from n in context.Items
							where n.CheckOutDate != null
							select n).ToList();

				retValue = retValue.DTOConvert(list);
			}

			return retValue;
		}

		public ResultDTO CheckItemIn(ItemDTO itemDto)
		{
			ResultDTO retValue = new ResultDTO();

			using (var context = new DataAccessContext())
			{
				Item item = (from n in context.Items
							 where n.ID == itemDto.ID
							 select n).First();

				item.AssociatedUserId = null;
				item.CheckOutDate = null;

				try
				{
					context.SaveChanges();
				}
				catch(Exception ex)
				{
					retValue.StatusCode = (int)HttpStatusCode.InternalServerError;
					retValue.StatusCodeSuccess = false;
					retValue.StatusMessage = ex.Message;
				}
			}

			return retValue;
		}

		public ResultDTO CheckItemOut(ItemDTO itemDto)
		{
			ResultDTO retValue = new ResultDTO();

			using (var context = new DataAccessContext())
			{
				Item item = (from n in context.Items
							 where n.ID == itemDto.ID
							 select n).First();

				item.CheckOutDate = DateTime.Now;
				item.RequestDate = null;

				try
				{
					context.SaveChanges();
				}
				catch (Exception ex)
				{
					retValue.StatusCode = (int)HttpStatusCode.InternalServerError;
					retValue.StatusCodeSuccess = false;
					retValue.StatusMessage = ex.Message;
				}
			}

			return retValue;
		}

		public ResultDTO SaveItem(ItemDTO item)
		{
			ResultDTO result = new ResultDTO();

			using (var context = new DataAccessContext())
			{
				Item newItem = new Item
				{
					Barcode = item.Barcode,
					CategoryId = item.CategoryId,
					Description = item.Description,
					ImageName = item.ImageName,
					LocationId = item.LocationId,
					Name = item.Name,
					SubCategoryId = item.SubCategoryId,
					SubLocationId = item.SubLocationId
				};
				
				context.Items.Add(newItem);

				context.SaveChanges();

				// add many to many values
				foreach (var ageGroup in item.Item2AgeGroup)
				{
					newItem.Item2AgeGroup.Add(new Item2AgeGroup
					{
						AgeGroupId = ageGroup.AgeGroupId,
						ItemId = newItem.ID
					});
				}

				foreach (var subject in item.Item2Subject)
				{
					newItem.Item2Subject.Add(new Item2Subject
					{
						SubjectId = subject.SubjectId,
						ItemId = newItem.ID
					});
				}

				context.SaveChanges();
			}

			return result;
		}
	}
}
