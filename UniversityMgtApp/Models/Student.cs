using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniversityMgtApp.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        public string RegNumber { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        public string StudentName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please Input Valid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contact number is required")]
        [Display(Name = "Contact No.")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime RegistrationDate { get; set; }

        [Required]
        public string Address { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [Display(Name = "Department")]
        public string DepartmentCode { get; set; }

        public string DepartmentName { get; set; }
        public virtual ICollection<Department> Department { get; set; }
    }
}