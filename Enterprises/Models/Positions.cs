using System;
using System.Collections.Generic;


namespace Enterprises
{
    public partial class Positions
    {
        public Positions()
        {
            Employees = new HashSet<Employees>();
        }

        public short Id { get; set; }
        public string Position { get; set; }

        public virtual ICollection<Employees> Employees { get; set; }

       
    }
}
