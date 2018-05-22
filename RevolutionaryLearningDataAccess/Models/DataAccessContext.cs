namespace RevolutionaryLearningDataAccess.Models
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using AutoMapper;
	using System.Reflection;
	using System.Collections.Generic;
	using DTOCollection;

	public partial class DataAccessContext : DbContext
	{
		private static bool _MapperInitialized = false;

		public DataAccessContext()
			: base("name=DataAccessContext")
		{
			// setup maps for all EF objects to DTOs
			if (!_MapperInitialized)
			{
				Mapper.Initialize(cfg =>
				{
					cfg.CreateMissingTypeMaps = true;
					cfg.ForAllMaps((map, exp) => exp.ForMember("StatusCode", n => n.Ignore()).
						ForMember("StatusCodeSuccess", n => n.Ignore()).
						ForMember("StatusMessage", n => n.Ignore()));

					var list = AppDomain.CurrentDomain.GetAssemblies()
						   .SelectMany(t => t.GetTypes())
						   .Where(t => t.IsClass && t.Namespace == "RevolutionaryLearningDataAccess.Models" &&
								t.Name != "Item").ToList();

					foreach (Type dtoType in AppDomain.CurrentDomain.GetAssemblies()
						   .SelectMany(t => t.GetTypes())
						   .Where(t => t.IsClass && t.Namespace == "RevolutionaryLearningDataAccess.DTOs"))
					{
						var entityType = (from n in list
										  where n.Name == dtoType.Name.Replace("DTO", String.Empty)
										  select n).FirstOrDefault();

						cfg.CreateMap(entityType, dtoType);
					}

					cfg.CreateMap(typeof(Item), typeof(ItemDTO)).ForMember("StatusCode", n => n.Ignore()).
						ForMember("StatusCodeSuccess", n => n.Ignore()).
						ForMember("StatusMessage", n => n.Ignore()).
						ForMember("RequestDateJS", n => n.Ignore()).
						ForMember("CheckOutDateJS", n => n.Ignore());
			});

				_MapperInitialized = true;
			}
		}

		public virtual DbSet<AgeGroup> AgeGroups { get; set; }
		public virtual DbSet<Category> Categories { get; set; }
		public virtual DbSet<Item> Items { get; set; }
		public virtual DbSet<Item2AgeGroup> Item2AgeGroup { get; set; }
		public virtual DbSet<Item2Subject> Item2Subject { get; set; }
		public virtual DbSet<Location> Locations { get; set; }
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
		}
	}
}
