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
				var authResult = itemDto.Authenticate(context);

				if(!authResult.StatusCodeSuccess)
				{
					return authResult;
				}

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
				var authResult = itemDto.Authenticate(context);

				if (!authResult.StatusCodeSuccess)
				{
					return authResult;
				}

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
	}
}
