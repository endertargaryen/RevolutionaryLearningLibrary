using System.Collections.Generic;

namespace DTOCollection
{
	public class LocationDTO : DTOBase
	{
		public int ID { get; set; }

		public string Name { get; set; }

		public int OwnerUserId { get; set; }

		public UserDTO OwnerUser { get; set; }

		public ICollection<SubLocationDTO> SubLocations { get; set; }
	}
}