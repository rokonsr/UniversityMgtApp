using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniversityMgtApp.Models.ViewModel
{
    public class ViewCourseStatics
    {
        public int CourseId { get; set; }

        [Display(Name = "Code")]
        public string CourseCode { get; set; }

        [Display(Name = "Name/Title")]
        public string CourseName { get; set; }

        public string Semester { get; set; }

        [Display(Name = "Assigned To")]
        public string TeacherName { get; set; }
    }
}