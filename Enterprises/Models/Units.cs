using Enterprises;
using System;
using System.Collections.Generic;



namespace Enterprises
{
    public partial class Units
    {
        public Units()
        {
            Finproducts = new HashSet<Finproducts>();
            Rawmaterials = new HashSet<Rawmaterials>();
        }

        public short Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Finproducts> Finproducts { get; set; }
        public virtual ICollection<Rawmaterials> Rawmaterials { get; set; }
    }
}
