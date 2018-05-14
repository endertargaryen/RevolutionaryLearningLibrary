using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOCollection
{
	public class ChangePasswordDTO : DTOBase
	{
		public string Email { get; set; }

		public string CurrentPassword { get; set; }

		public string NewPassword { get; set; }

		public string ConfirmPassword { get; set; }
	}
}
