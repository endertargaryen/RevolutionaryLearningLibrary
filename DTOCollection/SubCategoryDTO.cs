namespace DTOCollection
{
	public class SubCategoryDTO : DTOBase
	{
		public int ID { get; set; }

		public int CategoryId { get; set; }

		public string Name { get; set; }

		public CategoryDTO Category { get; set; }
	}
}