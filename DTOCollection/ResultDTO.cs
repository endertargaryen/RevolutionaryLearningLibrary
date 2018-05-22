using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DTOCollection
{
	public class ResultDTO : DTOBase
	{
		public ResultDTO()
		{
			this.StatusCode = (int)HttpStatusCode.OK;
			this.StatusCodeSuccess = true;
			this.StatusMessage = String.Empty;
		}
	}
}
