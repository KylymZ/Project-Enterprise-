using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Enterprises
{
    public partial class Budgets
    {
        public byte Id { get; set; }
        public decimal Budgetamount { get; set; }
        public float SalePercentage { get; set; }
        public float Bonus { get; set; }
    }
}
