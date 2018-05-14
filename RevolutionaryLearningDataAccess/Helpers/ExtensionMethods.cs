using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using DTOCollection;
using System.Security.Cryptography;
using System.Text;

namespace RevolutionaryLearningDataAccess
{
	public static class ExtensionMethods
	{
		public static T DTOConvert<T>(this T obj, object convertFrom) where T : DTOBase
		{
			IMapper mapper = Mapper.Configuration.CreateMapper();

			return mapper.Map<T>(convertFrom);
		}

		public static DTOList<T> Convert<T>(this DTOList<T> list, object convertFromList) where T : DTOBase
		{
			IMapper mapper = Mapper.Configuration.CreateMapper();

			return mapper.Map<DTOList<T>>(convertFromList);
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
	}
}