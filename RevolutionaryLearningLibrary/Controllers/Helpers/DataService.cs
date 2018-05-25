using DTOCollection;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace RevolutionaryLearningLibrary.Controllers
{
	public class DataService
	{
		private const string DS_PATH = "http://localhost:456/api/{0}/{1}";

		private  HttpClient _Client;

		private HttpClient Client
		{
			get
			{
				if(_Client == null)
				{
					_Client = new HttpClient();
					_Client.BaseAddress = new Uri("http://localhost:456/");
					_Client.DefaultRequestHeaders.Accept.Clear();
					_Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				}

				return _Client;
			}
		}

		public async Task<T> CallDataService<T>(string controller, string action, object postData = null) where T : DTOBase, new()
		{
			T retObj = default(T);
			PropertyInfo[] properties = typeof(T).GetProperties();
			string path = String.Format(DS_PATH, controller, action);
			HttpResponseMessage response;

			if (postData == null)
			{
				response = await Client.GetAsync(path);
			}
			else
			{
				// only support a GET by the ID
				if (!(postData is DTOBase || postData is IList))
				{
					path += "?id=" + postData;

					response = await Client.GetAsync(path);
				}
				else
				{
					response = await Client.PostAsJsonAsync(path, postData);
				}
			}

			if(response.IsSuccessStatusCode)
			{
				retObj = await response.Content.ReadAsAsync<T>();

				if (retObj == null)
				{
					retObj = new T();
					retObj.StatusCodeSuccess = true;
					retObj.StatusCode = (int)System.Net.HttpStatusCode.NoContent;
				}
				else
				{
					retObj.StatusCode = (int)response.StatusCode;
					retObj.StatusCodeSuccess = true;

					List<PropertyInfo> dates = (from n in properties
												where n.PropertyType == typeof(DateTime) ||
												n.PropertyType == typeof(DateTime?)
												select n).ToList();

					dates.ForEach(n =>
					{
						typeof(T).GetProperty(n.Name + "JS").SetValue(retObj,
							(n.GetValue(retObj) == null ? "" : n.GetValue(retObj).ToString()));
					});
				}
			}
			else
			{
				retObj = new T();
				retObj.StatusCode = (int)response.StatusCode;
				retObj.StatusCodeSuccess = false;
			}

			Debug.WriteLine(JsonConvert.SerializeObject(postData));
			Debug.WriteLine(JsonConvert.SerializeObject(retObj));

			return retObj;
		}

		public async Task<DTOList<T>> CallDataServiceList<T>(string controller, string action, object postData = null) where T : DTOBase, new()
		{
			PropertyInfo[] properties = typeof(T).GetProperties();
			DTOList<T> retList = default(DTOList<T>);
			string path = String.Format(DS_PATH, controller, action);
			HttpResponseMessage response;

			if (postData == null)
			{
				response = await Client.GetAsync(path);
			}
			else
			{
				if(!(postData is DTOBase) && !(postData is IDtoList))
				{
					throw new Exception("Have to pass in a DTOList<T> or DTOBase for the postData parameter");
				}

				response = await Client.PostAsJsonAsync(path, postData);
			}

			if (response.IsSuccessStatusCode)
			{
				retList = await response.Content.ReadAsAsync<DTOList<T>>();
				retList.StatusCode = (int)response.StatusCode;
				retList.StatusCodeSuccess = true;

				foreach (var obj in retList)
				{
					obj.StatusCode = (int)response.StatusCode;
					obj.StatusCodeSuccess = true;

					List<PropertyInfo> dates = (from n in properties
											where n.PropertyType == typeof(DateTime) ||
											n.PropertyType == typeof(DateTime?)
											select n).ToList();

					dates.ForEach(n =>
					{
						typeof(T).GetProperty(n.Name + "JS").SetValue(obj,
							(n.GetValue(obj) == null ? "" : n.GetValue(obj).ToString()));
					});
				}
			}
			else
			{
				retList = new DTOList<T>();
				
				retList.StatusCode = (int)response.StatusCode;
				retList.StatusCodeSuccess = false;
			}

			Debug.WriteLine(JsonConvert.SerializeObject(postData));
			Debug.WriteLine(JsonConvert.SerializeObject(retList));

			return retList;
		}

		public void SendEmail(string subject, string body, bool isError = false, string recipient = null)
		{
			MailMessage mail = new MailMessage("messages@revolutionarylearning.org", 
				(isError ? "endersshadow20@gmail.com" : recipient));
			SmtpClient client = new SmtpClient();
			client.Port = 25;
			client.DeliveryMethod = SmtpDeliveryMethod.Network;
			client.UseDefaultCredentials = false;
			client.Host = "smtp.gmail.com";
			mail.Subject = subject;
			mail.Body = body;
			client.Send(mail);
		}
	}
}