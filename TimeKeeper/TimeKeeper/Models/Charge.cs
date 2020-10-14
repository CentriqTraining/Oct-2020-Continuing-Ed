using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace TimeKeeper.Models
{
    public class Charge
    {
        public int ID { get; set; }
        public decimal Hours { get; set; }
        [DisplayName("Employee ID")]
        public int EmpID { get; set;  }
        public Department Department { get; set; }
        public Project Project { get; set;  }
        public DateTime DateEntered { get; set; }
    }
}
