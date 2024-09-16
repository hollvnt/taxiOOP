using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taxi.Models
{
    public class JoinResult
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public DateTime DateOrder { get; set; }
        public int? DriverID { get; set; }
        public string Status { get; set; }
        public int? UserID { get; set; }
        public string Contact { get; set; }
        public decimal Price { get; set; }
        //public JoinResult(string Address1, string Address2, DateTime DateOrder, int? DriverID, string Status,int? UserID,decimal Contact)
        //{
        //    this.Address1 = Address1;this.Address2 = Address2;this.DateOrder = DateOrder; this.DriverID = DriverID;this.Status = Status;this.UserID = UserID;this.Contact = Contact;
        //}
    }
}
