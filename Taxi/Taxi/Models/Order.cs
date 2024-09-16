using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taxi.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string Comment { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime DateOrder { get; set; }
        public int? UserID { get; set; }
        public virtual User User { get; set; }
        public int? DriverID { get; set; }
        public virtual Driver Driver { get; set; }
    }
}
