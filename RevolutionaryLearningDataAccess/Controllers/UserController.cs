using AutoMapper;
using DTOCollection;
using RevolutionaryLearningDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;

namespace RevolutionaryLearningDataAccess.Controllers
{
    public class UserController : ApiController
    {
		public UserDTO Get(int id)
		{
			UserDTO retValue = null;

			using (var context = new DataAccessContext())
			{
				User user = (from n in context.Users
							where n.ID == id
							select n).FirstOrDefault();

				retValue = retValue.Convert(user);
			}

			return retValue;
		}

		[HttpPost]
		public UserDTO VerifyUser(LoginDTO user)
		{
			UserDTO retValue = null;
			string hashedPassword = user.Password.MD5Encrypt();

			using (var context = new DataAccessContext())
			{
				User verifiedUser = null;

				verifiedUser = (from n in context.Users
								where n.Email == user.Email &&
								n.Password == hashedPassword
								select n).FirstOrDefault();

				if(verifiedUser != null)
				{
					retValue = retValue.Convert<UserDTO>(verifiedUser);
				}
			}

			return retValue;
		}

		public DTOList<UserDTO> GetUsers()
		{
			DTOList<UserDTO> retValue = null;
			

			using (var context = new DataAccessContext())
			{
				var list = (from n in context.Users
							select n).ToList();

				retValue = retValue.Convert<UserDTO>(list);
			}

			return retValue;
		}

	}
}
