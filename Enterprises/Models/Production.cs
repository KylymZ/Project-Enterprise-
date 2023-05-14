using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Enterprises
{
    public partial class Production
    {
        public short Id { get; set; }
        public short? Product { get; set; }
        public float? CountProduction { get; set; }
        public DateTime? Date { get; set; }
        public int? Employee { get; set; }

		[ForeignKey("Employee")]
		public virtual Employees EmployeeNavigation { get; set; }
		[ForeignKey("Product")]
		public virtual Finproducts ProductNavigation { get; set; }
    }
}
