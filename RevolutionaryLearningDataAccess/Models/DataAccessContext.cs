namespace RevolutionaryLearningDataAccess.Models
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public partial class DataAccessContext : DbContext
	{
		public DataAccessContext()
			: base("name=DataAccessContext")
		{
		}

		public virtual DbSet<AgeGroup> AgeGroups { get; set; }
		public virtual DbSet<Category> Categories { get; set; }
		public virtual DbSet<Item> Items { get; set; }
		public virtual DbSet<Item2AgeGroup> Item2AgeGroup { get; set; }
		public virtual DbSet<Item2Subject> Item2Subject { get; set; }
		public virtual DbSet<Location> Locations { get; set; }
		public virtual DbSet<SubCategory> SubCategories { get; set; }
		public virtual DbSet<Subject> Subjects { get; set; }
		public virtual DbSet<SubLocation> SubLocations { get; set; }
		public virtual DbSet<User> Users { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<AgeGroup>()
				.HasMany(e => e.Item2AgeGroup)
				.WithRequired(e => e.AgeGroup)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Category>()
				.HasMany(e => e.Items)
				.WithRequired(e => e.Category)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Category>()
				.HasMany(e => e.SubCategories)
				.WithRequired(e => e.Category)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Item>()
				.HasMany(e => e.Item2AgeGroup)
				.WithRequired(e => e.Item)
				.HasForeignKey(e => e.ItemId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Item>()
				.HasMany(e => e.Item2Subject)
				.WithRequired(e => e.Item)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Location>()
				.HasMany(e => e.Items)
				.WithRequired(e => e.Location)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Location>()
				.HasMany(e => e.SubLocations)
				.WithRequired(e => e.Location)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Subject>()
				.HasMany(e => e.Item2Subject)
				.WithRequired(e => e.Subject)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<User>()
				.HasMany(e => e.Items)
				.WithOptional(e => e.User)
				.HasForeignKey(e => e.AssociatedUserId);

			modelBuilder.Entity<User>()
				.HasMany(e => e.Locations)
				.WithRequired(e => e.User)
				.HasForeignKey(e => e.OwnerUserId)
				.WillCascadeOnDelete(false);
		}
	}
}
