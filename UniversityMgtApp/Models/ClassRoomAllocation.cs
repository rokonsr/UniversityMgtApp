using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniversityMgtApp.Models
{
    public class ClassRoomAllocation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public virtual ICollection<Department> Department { get; set; }

        [Required(ErrorMessage = "Course is required")]
        [Display(Name = "Course")]
        public int CourseId { get; set; }
        public virtual ICollection<Course> Course { get; set; }

        [Required(ErrorMessage = "Room No. required")]
        [Display(Name = "Room No.")]
        public int ClassRoomId { get; set; }

        [Required(ErrorMessage = "Day required")]
        public string Day { get; set; }

        [Required(ErrorMessage = "Time is required")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "Time is required")]
        public TimeSpan EndTime { get; set; }

        [Required(ErrorMessage = "Required")]
        public string StartAmPm { get; set; }

        [Required(ErrorMessage = "Required")]
        public string EndAmPm { get; set; }
    }
}