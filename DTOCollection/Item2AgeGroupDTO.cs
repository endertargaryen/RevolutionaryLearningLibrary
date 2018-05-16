namespace DTOCollection
{
	public class Item2AgeGroupDTO : DTOBase
	{
		public int ID { get; set; }

		public int ItemId { get; set; }

		public int AgeGroupId { get; set; }

		public AgeGroupDTO AgeGroup { get; set; }
	}
}