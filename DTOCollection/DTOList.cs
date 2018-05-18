using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DTOCollection
{
	public interface IDtoList
	{
		int StatusCode { get; set; }
		bool StatusCodeSuccess { get; set; }
	}

	public class DTOList<T> : List<T>, IDtoList where T : DTOBase
	{
		public int StatusCode { get; set; }
		public bool StatusCodeSuccess { get; set; }
	}
}
