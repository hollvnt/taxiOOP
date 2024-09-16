using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taxi.Models
{
    public class Driver
    {
        public int DriverID { get; set; }
        public string DriverName { get; set; }
        public string DriverLastName { get; set; }
        public string Password { get; set; }
        public string Contact { get; set; }
        public string Mail { get; set; }
        public byte[] Avatar { get; set; }
      
    }
}
