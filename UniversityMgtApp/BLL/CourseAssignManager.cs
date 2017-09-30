using System;
using System.Collections.Generic;
using UniversityMgtApp.Gateway;
using UniversityMgtApp.Models;
using UniversityMgtApp.Models.ViewModel;

namespace UniversityMgtApp.BLL
{
    public class CourseAssignManager
    {
        CourseAssignGateway courseAssignGateway = new CourseAssignGateway();
        public CourseAssign GetCourseDetails(int? courseId)
        {
            return courseAssignGateway.GetCourseDetails(courseId);
        }

        public bool SaveAssignCourse(CourseAssign courseAssign)
        {
            if (courseAssignGateway.IsAssignedCourse(courseAssign.CourseId))
            {
                throw new Exception("Course already assigned");
            }

            return courseAssignGateway.SaveAssignCourse(courseAssign) > 0;
        }

        public List<ViewCourseStatics> GetAllCourseDetails(int? departmentId)
        {
            return courseAssignGateway.GetAllCourseDetails(departmentId);
        }

        public bool SaveCourseEnroll(CourseEnrollment courseEnrollment)
        {
            if (courseAssignGateway.IsEnrolled(courseEnrollment))
            {
                throw new Exception("Selected course already enrolled");
            }
            return courseAssignGateway.SaveCourseEnroll(courseEnrollment) > 0;
        }

        public bool UnassignAllCourses()
        {
            return courseAssignGateway.UnassignAllCourses() > 0;
        }
    }
}