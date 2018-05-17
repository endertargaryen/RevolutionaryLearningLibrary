using DTOCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace RevolutionaryLearningLibrary
{
	public class DataService
	{
		private const string DS_PATH = "http://localhost:456/api/{0}/{1}/{2}";
		//private const string DS_PATH = "http://localhost:21515/api/{0}/{1}/{2}";

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

		public T CallDataServiceSync<T>(string controller, string action, int id = 0, DTOBase postData = null) where T : DTOBase, new()
		{
			var task = CallDataService<T>(controller, action, id, postData);

			task.Wait();

			return task.Result;
		}

		public DTOList<T> CallDataServiceListSync<T>(string controller, string action, int id = 0, DTOBase postData = null) where T : DTOBase, new()
		{
			var task = CallDataServiceList<T>(controller, action, id, postData);

			task.Wait();

			return task.Result;
		}

		public async Task<T> CallDataService<T>(string controller, string action, int id = 0, DTOBase postData = null) where T : DTOBase, new()
		{
			T retObj = default(T);
			string path = String.Format(DS_PATH, controller, action, (id > 0 ? id.ToString() : String.Empty));
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

			return retObj;
		}

		public async Task<DTOList<T>> CallDataServiceList<T>(string controller, string action, int id = 0, DTOBase postData = null) where T : DTOBase, new()
		{
			DTOList<T> retObj = default(DTOList<T>);
			string path = String.Format(DS_PATH, controller, action, (id > 0 ? id.ToString() : String.Empty));
			HttpResponseMessage response;

			if (postData == null)
			{
				response = await Client.GetAsync(path);
			}
			else
			{
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

			return retObj;
		}
	}
}