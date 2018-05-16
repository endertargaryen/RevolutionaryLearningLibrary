using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOCollection
{
	public class ItemFilterDTO : DTOBase
	{
		public int SubjectId { get; set; }
		public int AgeGroupId { get; set; }
		public int CategoryId { get; set; }
		public int LocationId { get; set; }
	}
}
