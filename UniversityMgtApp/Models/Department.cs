using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniversityMgtApp.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }

        [Required(ErrorMessage="Code is required!")]
        [StringLength(7, MinimumLength = 2, ErrorMessage = "Code must be 2 to 7 characters")]
        [Display(Name="Code")]
        public string DepartmentCode { get; set; }

        [Required(ErrorMessage="Name is required!")]
        [StringLength(45)]
        [Display(Name="Name")]
        public string DepartmentName { get; set; }
    }
}