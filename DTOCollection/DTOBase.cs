using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace DTOCollection
{
	public abstract class DTOBase
	{
		public int StatusCode { get; set; }
		public bool StatusCodeSuccess { get; set; }

		public string StatusMessage { get; set; }
	}
}