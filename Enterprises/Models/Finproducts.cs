using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Enterprises
{
    public partial class Finproducts
    {
        public Finproducts()
        {
            Ingredients = new HashSet<Ingredients>();
            Production = new HashSet<Production>();
            Saleofproducts = new HashSet<Saleofproducts>();
        }

        [Key]
        public short Id { get; set; }
        public string Name { get; set; }
        public short? Unit { get; set; }
        public decimal? Sum { get; set; }
        public float? Countproducts { get; set; }

		[ForeignKey("Unit")]
		public virtual Units UnitNavigation { get; set; }
        public virtual ICollection<Ingredients> Ingredients { get; set; }
        public virtual ICollection<Production> Production { get; set; }
        public virtual ICollection<Saleofproducts> Saleofproducts { get; set; }
    }
}
