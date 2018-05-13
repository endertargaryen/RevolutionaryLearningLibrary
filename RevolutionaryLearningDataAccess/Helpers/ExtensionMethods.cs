using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using DTOCollection;

namespace RevolutionaryLearningDataAccess
{
	public static class ExtensionMethods
	{
		public static T Convert<T>(this T obj, object convertFrom) where T : DTOBase
		{
			IMapper mapper = Mapper.Configuration.CreateMapper();

			return mapper.Map<T>(convertFrom);
		}

		public static List<T> Convert<T>(this List<T> list, object convertFromList)
		{
			IMapper mapper = Mapper.Configuration.CreateMapper();

			return mapper.Map<List<T>>(convertFromList);
		}
	}
}