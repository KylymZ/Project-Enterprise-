using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Enterprises
{

    public partial class SalaryEmp
    {
		[Key]
        public int Id { get; set; }
        public int? Year { get; set; }
        public byte Month { get; set; }
        public int Employee { get; set; }
        public byte? ParticipationPurchase { get; set; }
        public byte? ParticipationSale { get; set; }
        public byte? ParticipationProduction { get; set; }
        public byte? CountParticipation { get; set; }
        public decimal? SalaryEmployee { get; set; }
        public float? TotalAmount { get; set; }
        public bool Issued { get; set; }
        public float? Bonus { get; set; }

		[ForeignKey("Employee")]
		public virtual Employees EmployeeNavigation { get; set; }
		[ForeignKey("Year")]
		public virtual Years YearNavigation { get; set; }
		[ForeignKey("Month")]
		public virtual Months MonthNavigation { get; set; }


    }
}
