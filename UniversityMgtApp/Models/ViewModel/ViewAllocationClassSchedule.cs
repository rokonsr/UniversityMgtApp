using System.ComponentModel.DataAnnotations;

namespace UniversityMgtApp.Models.ViewModel
{
    public class ViewAllocationClassSchedule
    {
        [Display(Name = "Course Code")]
        public string CourseCode { get; set; }

        [Display(Name = "Name")]
        public string CourseName { get; set; }

        [Display(Name = "Schedule Info")]
        public string ScheduleInfo { get; set; }
    }
}