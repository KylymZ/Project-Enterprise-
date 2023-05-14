using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Enterprises
{
    public partial class PurchaseOfrawmaterials
    {
        public short Id { get; set; }
        public short? RawMaterials { get; set; }
        public float? CountPur { get; set; }
        public decimal? Sum { get; set; }
        public DateTime? Date { get; set; }
        public int? Employee { get; set; }

		[ForeignKey("Employee")]
		public virtual Employees EmployeeNavigation { get; set; } // Создаем метод для работы с первычными ключами
        
        [ForeignKey("RawMaterials")]
		public virtual Rawmaterials RawMaterialsNavigation { get; set; }
    }
}
