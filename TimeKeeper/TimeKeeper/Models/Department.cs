using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace TimeKeeper.Models
{
    public class Department
    {
        public int ID { get; set; }
        [DisplayName("Department Name")]
        public string Name { get; set; }
        public ICollection<Charge> Charges { get; set; }
    }
}
