using System;
using System.ComponentModel.DataAnnotations;

namespace UniversityMgtApp.Models
{
    public class CourseEnrollment
    {
        public int EnrollmentId { get; set; }

        [Required(ErrorMessage = "Registration number is required")]
        [Display(Name = "Student Reg. No.")]
        public string StudentRegistrationNumber { get; set; }

        public int StudentId { get; set; }

        [Display(Name = "Name")]
        public string StudentName { get; set; }
        public string Email { get; set; }

        [Display(Name = "Department")]
        public string DepartmentName { get; set; }

        [Required(ErrorMessage = "Course is required")]
        [Display(Name = "Select Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime EnrollDate { get; set; }
    }
}