using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOCollection
{
	public class ItemDTO : DTOBase
	{
		public int ID { get; set; }

		public int LocationId { get; set; }

		public int? SubLocationId { get; set; }

		public int CategoryId { get; set; }

		public int? SubCategoryId { get; set; }

		public int? AssociatedUserId { get; set; }

		public DateTime? CheckOutDate { get; set; }

		public DateTime? RequestDate { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public string Barcode { get; set; }

		public string ImageName { get; set; }

		public CategoryDTO Category { get; set; }

		public LocationDTO Location { get; set; }

		public SubCategoryDTO SubCategory { get; set; }

		public SubLocationDTO SubLocation { get; set; }

		public UserDTO AssociatedUser { get; set; }

		public ICollection<Item2AgeGroupDTO> Item2AgeGroup { get; set; }

		public ICollection<Item2SubjectDTO> Item2Subject { get; set; }
	}
}
