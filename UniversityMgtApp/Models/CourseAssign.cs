using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniversityMgtApp.Models
{
    public class CourseAssign
    {
        public int CourseAssignId { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public virtual ICollection<Department> Department { get; set; }

        [Required(ErrorMessage = "Teacher name is required")]
        [Display(Name = "Teacher")]
        public int TeacherId { get; set; }
        public virtual ICollection<Teacher> Teacher { get; set; }

        [Display(Name = "Credit To Be Taken")]
        public double CreditToBeTaken { get; set; }

        [Display(Name = "Remaining Credit")]
        public double RemainingCredit { get; set; }

        [Required(ErrorMessage = "Course code is required")]
        [Display(Name = "Course Code")]
        public int CourseId { get; set; }
        public virtual ICollection<Course> Course { get; set; }

        [Display(Name = "Name")]
        public string CourseName { get; set; }

        [Display(Name = "Credit")]
        public decimal CourseCredit { get; set; }
    }
}