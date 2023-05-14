
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Enterprises
{
    public partial class Months
    {
        public Months() {
            SalaryEmp = new HashSet<SalaryEmp>();
        }
        [Key]
        public byte Id { get; set; }
        public string MonthName { get; set; }
        public virtual ICollection<SalaryEmp> SalaryEmp { get; set; }
    }
}
