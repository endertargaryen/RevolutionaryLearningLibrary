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

		public T CallSync<T>(string controller, int id = 0, DTOBase postData = null)
		{
			var task = Call<T>(controller, id, postData);

			task.Wait();

			return task.Result;
		}

		public async Task<T> Call<T>(string controller, int id = 0, DTOBase postData = null)
		{
			T retObj = default(T);
			string path = String.Format(DS_PATH, controller, (id > 0 ? id.ToString() : String.Empty));
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
			}

			return retObj;
		}
	}
}