using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniversityMgtApp.Models
{
    public class StudentResult
    {
        public int StudentResultId { get; set; }

        [Required(ErrorMessage = "Registration number is required")]
        [Display(Name = "Student Reg. No.")]
        public string StudentRegistrationNumber { get; set; }

        public int StudentId { get; set; }

        [Display(Name = "Name")]
        public string StudentName { get; set; }

        public string Email { get; set; }

        [Display(Name = "Department")]
        public string DepartmentName { get; set; }

        [Display(Name = "Select Course")]
        public int CourseId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public virtual ICollection<Course> Course { get; set; }

        [Required(ErrorMessage = "Grade letter is required")]
        [Display(Name = "Select Grade Letter")]
        public string GradeLetter { get; set; }
    }
}