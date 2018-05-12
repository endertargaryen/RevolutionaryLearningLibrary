namespace RevolutionaryLearningDataAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Item2Subject
    {
        public int ID { get; set; }

        public int ItemId { get; set; }

        public int SubjectId { get; set; }

        public virtual Item Item { get; set; }

        public virtual Subject Subject { get; set; }
    }
}
