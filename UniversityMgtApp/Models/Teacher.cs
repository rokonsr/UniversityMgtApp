using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniversityMgtApp.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        public string TeacherName { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please Input Valid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contact No. is required")]
        [Display(Name = "Contact No.")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Designation is required")]
        [Display(Name = "Designation")]
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public virtual ICollection<Department> Department { get; set; }

        [Required(ErrorMessage = "Credit is required")]
        [Display(Name = "Credit To Be Taken")]
        [Range(0.0, 50000.0, ErrorMessage = "Credit must be possitive value")]
        public decimal CreditToBeTaken { get; set; }
    }
}