using DTOCollection;
using RevolutionaryLearningDataAccess.Models;
using RevolutionaryLearningLibrary.Controllers;
using System;
using System.Collections.Generic;
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

				retValue.StatusMessage = $"Found {items.Count} items";
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

				retValue.StatusMessage = $"Found {items.Count} items";
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
						StatusCodeSuccess = true,
						StatusMessage = $"Updated {items.Count} items"
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
				retValue.StatusMessage = $"Found Item";
			}

			return retValue;
		}

		[HttpPost]
		public ResultDTO DeleteItem(IdDTO idDTO)
		{
			ResultDTO result = new ResultDTO();

			using (var context = new DataAccessContext())
			{
				var authResult = idDTO.Authenticate(context);

				if(!authResult.StatusCodeSuccess)
				{
					return authResult;
				}

				// remove foreign keys
				List<Item2AgeGroup> item2AgeGroups = (from n in context.Item2AgeGroup
													  where n.ItemId == idDTO.ID
													  select n).ToList();

				item2AgeGroups.ForEach(n => context.Item2AgeGroup.Remove(n));

				List<Item2Subject> item2Subjects = (from n in context.Item2Subject
													where n.ItemId == idDTO.ID
													select n).ToList();

				item2Subjects.ForEach(n => context.Item2Subject.Remove(n));

				// delete item
				Item curItem = (from n in context.Items
								where n.ID == idDTO.ID
								select n).First();

				result.StatusMessage = "Deleted 1 item";

				context.Items.Remove(curItem);

				context.SaveChanges();
			}

			return result;
		}

		public ResultDTO SaveItem(ItemDTO item)
		{
			ResultDTO result = new ResultDTO();
			Item itemToProcess = null;

			try
			{
				using (var context = new DataAccessContext())
				{
					var authResult = item.Authenticate(context);

					if (!authResult.StatusCodeSuccess)
					{
						return authResult;
					}

					if (item.ID > 0)
					{
						itemToProcess = (from n in context.Items
										where n.ID == item.ID
										select n).First();

						itemToProcess.Barcode = item.Barcode;
						itemToProcess.CategoryId = item.CategoryId;
						itemToProcess.Description = item.Description;
						itemToProcess.ImageName = item.ImageName;
						itemToProcess.LocationId = item.LocationId;
						itemToProcess.Name = item.Name;
						itemToProcess.SubCategoryId = item.SubCategoryId;
						itemToProcess.SubLocationId = item.SubLocationId;

						result.StatusMessage = "Found 1 item to edit";

						// remove existing many to many values
						var ageGroups = (from n in context.Item2AgeGroup
										 where n.ItemId == itemToProcess.ID
										 select n).ToList();

						ageGroups.ForEach(n => context.Item2AgeGroup.Remove(n));

						var subjects = (from n in context.Item2Subject
										where n.ItemId == itemToProcess.ID
										select n).ToList();

						subjects.ForEach(n => context.Item2Subject.Remove(n));
					}
					else
					{
						itemToProcess = new Item
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

						context.Items.Add(itemToProcess);

						result.StatusMessage = "Inserted 1 item";

						context.SaveChanges();
					}

					// add many to many values
					foreach (var ageGroup in item.Item2AgeGroup)
					{
						itemToProcess.Item2AgeGroup.Add(new Item2AgeGroup
						{
							AgeGroupId = ageGroup.AgeGroupId,
							ItemId = itemToProcess.ID
						});
					}

					foreach (var subject in item.Item2Subject)
					{
						itemToProcess.Item2Subject.Add(new Item2Subject
						{
							SubjectId = subject.SubjectId,
							ItemId = itemToProcess.ID
						});
					}

					context.SaveChanges();
				}
			}
			catch(Exception ex)
			{
				result.StatusCode = (int)HttpStatusCode.InternalServerError;
				result.StatusCodeSuccess = false;
				result.StatusMessage = ex.ToString();
			}

			return result;
		}
	}
}
