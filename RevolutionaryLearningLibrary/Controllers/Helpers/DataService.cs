using DTOCollection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace RevolutionaryLearningLibrary
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
			string path = String.Format(DS_PATH, controller, action);
			HttpResponseMessage response;

			if (postData == null)
			{
				response = await Client.GetAsync(path);
			}
			else
			{
				response = await Client.PostAsJsonAsync(path, postData);
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
			DTOList<T> retObj = default(DTOList<T>);
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
				retObj = await response.Content.ReadAsAsync<DTOList<T>>();
				retObj.StatusCode = (int)response.StatusCode;
				retObj.StatusCodeSuccess = true;

				foreach (var obj in retObj)
				{
					obj.StatusCode = (int)response.StatusCode;
					obj.StatusCodeSuccess = true;
				}
			}
			else
			{
				retObj = new DTOList<T>();
				
				retObj.StatusCode = (int)response.StatusCode;
				retObj.StatusCodeSuccess = false;
			}

			Debug.WriteLine(JsonConvert.SerializeObject(postData));
			Debug.WriteLine(JsonConvert.SerializeObject(retObj));

			return retObj;
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