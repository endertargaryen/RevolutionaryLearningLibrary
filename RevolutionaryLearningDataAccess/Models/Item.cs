namespace RevolutionaryLearningDataAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Item")]
    public partial class Item
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Item()
        {
            Item2AgeGroup = new HashSet<Item2AgeGroup>();
            Item2Subject = new HashSet<Item2Subject>();
        }

        public int ID { get; set; }

        public int LocationId { get; set; }

        public int? SubLocationId { get; set; }

        public int CategoryId { get; set; }

        public int? SubCategoryId { get; set; }

        public int? AssociatedUserId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CheckOutDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? RequestDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Barcode { get; set; }

		[StringLength(100)]
		public string ImageUrl { get; set; }

		public virtual Category Category { get; set; }

        public virtual Location Location { get; set; }

        public virtual SubCategory SubCategory { get; set; }

        public virtual SubLocation SubLocation { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Item2AgeGroup> Item2AgeGroup { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Item2Subject> Item2Subject { get; set; }
    }
}
