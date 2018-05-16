namespace DTOCollection
{
	public class LocationDTO : DTOBase
	{
		public int ID { get; set; }

		public string Name { get; set; }

		public int OwnerUserId { get; set; }

		public UserDTO OwnerUser { get; set; }
	}
}