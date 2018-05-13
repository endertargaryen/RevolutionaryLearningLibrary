namespace RevolutionaryLearningDataAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Item2AgeGroup
    {
        public int ID { get; set; }

        public int ItemId { get; set; }

        public int AgeGroupId { get; set; }

        public virtual AgeGroup AgeGroup { get; set; }

        public virtual Item Item { get; set; }
    }
}
