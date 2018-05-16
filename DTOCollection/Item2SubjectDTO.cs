namespace DTOCollection
{
	public class Item2SubjectDTO : DTOBase
	{
		public int ID { get; set; }

		public int ItemId { get; set; }

		public int SubjectId { get; set; }

		public SubjectDTO Subject { get; set; }
	}
}