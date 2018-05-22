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

namespace RevolutionaryLearningDataAccess
{
	//public static class HelperMethods
	//{
	//	public static void DTOConvert<T>(this T obj, object convertFrom) where T : DTOBase
	//	{
	//		DTOConvert(obj, convertFrom);
	//	}

	//	private static object DTOConvert(Type convertToType, object convertFrom)
	//	{
	//		var toProperties = convertToType.GetProperties().ToList();
	//		var fromProperties = convertFrom.GetType().GetProperties().ToList();

	//		DTOBase convertTo = (DTOBase)convertToType.GetConstructor(Type.EmptyTypes).Invoke(null);

	//		// loop through each from property
	//		foreach(var fromProp in fromProperties)
	//		{
	//			// find the corresponding property name
	//			var toProp = (from p in toProperties
	//							where p.Name == fromProp.Name
	//							select p).FirstOrDefault();

	//			if (toProp != null)
	//			{
	//				// if it's another DTO, recursive call
	//				if (toProp.PropertyType.BaseType == typeof(DTOBase))
	//				{
	//					DTOConvert(toProp.PropertyType, fromProp.GetValue(convertFrom));
	//				}
	//				else if (toProp.PropertyType == typeof(IList) &&
	//						toProp.PropertyType.GetGenericArguments()[0].BaseType == typeof(DTOBase))
	//				{
	//					DTOConvertList(toProp.PropertyType, (IList)fromProp.GetValue(convertFrom));
	//				}
	//				else // otherwise set value
	//				{
	//					toProp.SetValue(convertTo, fromProp.GetValue(convertFrom));
	//				}
	//			}
	//		}

	//		return convertTo;
	//	}

	//	public static void DTOConvert<T>(this DTOList<T> list, IList convertFromList) where T : DTOBase
	//	{
	//		DTOConvertList(list.GetType(), convertFromList);	
	//	}

	//	private static IList DTOConvertList(Type convertListToType, IList convertFromList)
	//	{
	//		IList convertToList = (IList)convertListToType.GetConstructor(Type.EmptyTypes).Invoke(null);

	//		var toType = convertListToType.GetGenericArguments()[0];
	//		var fromType = convertFromList.GetType().GetGenericArguments()[0];

	//		var toProperties = toType.GetProperties().ToList();
	//		var fromProperties = fromType.GetProperties().ToList();
			

	//		// loop through all properties converting from
	//		foreach (object convertFromObj in convertFromList)
	//		{
	//			// create a new ConvertTo
	//			object convertToObj = toType.GetConstructor(Type.EmptyTypes).Invoke(null);

	//			// loop through each property in convertFrom
	//			foreach(var fromProp in fromProperties)
	//			{
	//				// find the corresponding convert to property
	//				var toProp = (from p in toProperties
	//								where p.Name == fromProp.Name
	//								select p).FirstOrDefault();

	//				// found a match
	//				if (toProp != null)
	//				{
	//					// if the type is another DTO
	//					if (toProp.PropertyType.BaseType == typeof(DTOBase))
	//					{
	//						convertToObj = (DTOBase)DTOConvert(toProp.PropertyType, fromProp.GetValue(convertFromObj));
	//					}
	//					// if the type is another list of DTOs
	//					else if (fromProp.PropertyType == typeof(IList) &&
	//						fromProp.PropertyType.GetGenericArguments()[0].BaseType == typeof(DTOBase))
	//					{
	//						DTOConvertList(convertToObj.GetType(), (IList)fromProp.GetValue(convertFromObj));
	//					}
	//					else // otherwise set value
	//					{
	//						toProp.SetValue(convertToObj, fromProp.GetValue(convertFromObj));
	//					}
	//				}
	//			}

	//			convertToList.Add(convertToObj);
	//		};

	//		return convertToList;
	//	}

	//	public static string MD5Encrypt(this string value)
	//	{
	//		StringBuilder sBuilder = new StringBuilder();

	//		using (var md5Hash = MD5.Create())
	//		{
	//			byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(value));

	//			for (int i = 0; i < data.Length; i++)
	//			{
	//				sBuilder.Append(data[i].ToString("x2"));
	//			}
	//		}

	//		return sBuilder.ToString();
	//	}
	//}
}