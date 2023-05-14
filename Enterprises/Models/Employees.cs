
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Enterprises;
namespace Enterprises
{
    public partial class Employees
    {
        public Employees()
        {
            Production = new HashSet<Production>();
            PurchaseOfrawmaterials = new HashSet<PurchaseOfrawmaterials>();
            Saleofproducts = new HashSet<Saleofproducts>();
            Salaries = new HashSet<SalaryEmp>();
        }
        [Key]
        public int Id { get; set; }
        public string Fullname { get; set; }
        public short? Position { get; set; }
        public decimal? Salary { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }

		[ForeignKey("Position")]
		public virtual Positions PositionNavigation { get; set; }
        public virtual ICollection<Production> Production { get; set; }

        public virtual ICollection<SalaryEmp> Salaries { get; set; }

        public virtual ICollection<PurchaseOfrawmaterials> PurchaseOfrawmaterials { get; set; }
        public virtual ICollection<Saleofproducts> Saleofproducts { get; set; }
    }
}
