using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniversityMgtApp.Models
{
    public class Course
    {
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Course code is required!")]
        [Display(Name="Code")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Code must be atleast 5 characters long")]
        public string CourseCode { get; set; }

        [Required(ErrorMessage = "Course name is required!")]
        [Display(Name = "Name")]
        public string CourseName { get; set; }

        [Required(ErrorMessage = "Credit is required!")]
        [Range(0.5, 5.0, ErrorMessage = "Credit must be 0.5 to 5.0")]
        public decimal Credit { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage="Department is required!")]
        [Display(Name="Department")]
        public int DepartmentId { get; set; }
        public virtual ICollection<Department> Department { get; set; }

        [Required(ErrorMessage="Semester is required!")]
        [Display(Name="Semester")]
        public int SemesterId { get; set; }
        public bool IsAssign { get; set; }
    }
}