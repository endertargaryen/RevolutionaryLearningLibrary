using DTOCollection;
using RevolutionaryLearningDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RevolutionaryLearningDataAccess.Controllers
{
    public class LookupController : ApiController
    {
		public DTOList<CategoryDTO> GetCategories()
		{
			DTOList<CategoryDTO> retValue = null;

			using (var context = new DataAccessContext())
			{
				var categories = (from n in context.Categories
								  select n).ToList();

				retValue = retValue.DTOConvert(categories);
			}

			return retValue;
		}

		public DTOList<SubjectDTO> GetSubjects()
		{
			DTOList<SubjectDTO> retValue = null;

			using (var context = new DataAccessContext())
			{
				var categories = (from n in context.Subjects
								  select n).ToList();

				retValue = retValue.DTOConvert(categories);
			}

			return retValue;
		}

		public DTOList<AgeGroupDTO> GetAgeGroups()
		{
			DTOList<AgeGroupDTO> retValue = null;

			using (var context = new DataAccessContext())
			{
				var categories = (from n in context.AgeGroups
								  select n).ToList();

				retValue = retValue.DTOConvert(categories);
			}

			return retValue;
		}

		public DTOList<LocationDTO> GetLocations()
		{
			DTOList<LocationDTO> retValue = null;

			using (var context = new DataAccessContext())
			{
				var categories = (from n in context.Locations
								  select n).ToList();

				retValue = retValue.DTOConvert(categories);
			}

			return retValue;
		}
	}
}
