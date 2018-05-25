using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using DTOCollection;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;
using System.Collections;
using RevolutionaryLearningDataAccess.Models;
using System.Net;

namespace RevolutionaryLearningDataAccess
{
	public static class ExtensionMethods
	{
		public static T DTOConvert<T>(this T obj, object convertFrom) where T : DTOBase
		{
			IMapper mapper = Mapper.Configuration.CreateMapper();

			return mapper.Map<T>(convertFrom);
		}

		public static DTOList<T> DTOConvert<T>(this DTOList<T> list, object convertFromList) where T : DTOBase
		{
			IMapper mapper = Mapper.Configuration.CreateMapper();

			return mapper.Map<DTOList<T>>(convertFromList);
		}

		public static T DTOConvertNonRecursive<T>(this T obj, object convertFrom) where T : DTOBase, new()
		{
			if(obj == null)
			{
				obj = new T();
			}

			PropertyInfo[] toProperties = obj.GetType().GetProperties();

			foreach (var fromProp in convertFrom.GetType().GetProperties())
			{
				var toProp = (from n in toProperties
							  where n.Name == fromProp.Name
							  select n).FirstOrDefault();

				if(toProp != null && toProp.PropertyType.GetInterface("IEnumerable") == null)
				{
					if (toProp.PropertyType.IsClass)
					{
						if(toProp.PropertyType == typeof(string) ||
							toProp.PropertyType == typeof(Nullable))
						{
							toProp.SetValue(obj, fromProp.GetValue(convertFrom));
						}
					}
					else
					{
						toProp.SetValue(obj, fromProp.GetValue(convertFrom));
					}
				}
			}

			return obj;
		}

		public static DTOList<T> DTOConvertNonRecursive<T>(this DTOList<T> list, object convertFrom) where T : DTOBase, new()
		{
			if (list == null)
			{
				list = new DTOList<T>();
			}

			foreach(var fromObj in ((IList)convertFrom))
			{
				T toObj = new T();

				toObj = toObj.DTOConvertNonRecursive<T>(fromObj);

				list.Add(toObj);
			}

			return list;
		}

		public static string MD5Encrypt(this string value)
		{
			StringBuilder sBuilder = new StringBuilder();

			using (var md5Hash = MD5.Create())
			{
				byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(value));

				for (int i = 0; i < data.Length; i++)
				{
					sBuilder.Append(data[i].ToString("x2"));
				}
			}

			return sBuilder.ToString();
		}

		public static ResultDTO Authenticate(this DTOBase obj, DataAccessContext context)
		{
			var result = new ResultDTO();

			var isAuthenticated = (from n in context.Users
								   where n.ID == obj.UserIdForAuth
								   select n).First().IsAdmin;

			if (!isAuthenticated)
			{
				result.StatusCode = (int)HttpStatusCode.Forbidden;
				result.StatusCodeSuccess = false;
				result.StatusMessage = "This user does not have access to endpoint";
			}

			return result;
		}
	}
}