using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DTOCollection
{
	public class DTOList<T> : List<T> where T : DTOBase
	{
		public int StatusCode { get; set; }
		public bool StatusCodeSuccess { get; set; }
	}
}
