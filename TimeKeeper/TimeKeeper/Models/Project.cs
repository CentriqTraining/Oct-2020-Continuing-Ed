using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace TimeKeeper.Models
{
    public class Project
    {
        public int ID { get; set; }
        [DisplayName("Project Name")]
        public string ProjectName { get; set; }
        public ICollection<Charge>  Charges { get; set; }
    }
}