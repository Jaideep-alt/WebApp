using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Employee
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage ="Required")]
        [Display(Name ="Name")]
        public string EmpName { get; set; }

        [Required(ErrorMessage = "Required")]
        public Int64 Contact { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required")] 
        public int DepID { get; set; }

        public string Description { get; set; }

        [NotMapped]
        public string Department { get; set; }
    }
}
