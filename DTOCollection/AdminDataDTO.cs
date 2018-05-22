using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOCollection
{
	public class AdminDataDTO : DTOBase
	{
		public DTOList<ItemDTO> Requests { get; set; }

		public DTOList<ItemDTO> Checkouts { get; set; }
	}
}
