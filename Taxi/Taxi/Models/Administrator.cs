using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taxi.Models
{
    public class Administrator
    {
        [Key]
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Messages { get; set; }

    }
}
