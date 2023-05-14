using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Enterprises
{
    public partial class Ingredients
    {
        public short Id { get; set; }
        public short? Product { get; set; }
        public short? RawMaterials { get; set; }
        public float Countingred { get; set; }

		[ForeignKey("Product")]
		public virtual Finproducts ProductNavigation { get; set; }
		[ForeignKey("RawMaterials")]
		public virtual Rawmaterials RawMaterialsNavigation { get; set; }
        
    }
}
