using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimeKeeper.Models
{
    public class ChargeViewModel
    {
        public int ID { get; set; }
        [Required]
        [Range(minimum: 1, maximum: 40)]
        public decimal Hours { get; set; }

        [Required]
        [DisplayName("Employee ID")]
        public int EmpID { get; set; }


        [Required]
        public string Department { get; set; }
        [Required]
        public string Project { get; set; }
        public List<SelectListItem> Departments { get; set; }
        public List<SelectListItem> Projects { get; set; }
    }
}
