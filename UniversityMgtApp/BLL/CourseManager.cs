using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;
using UniversityMgtApp.Gateway;
using UniversityMgtApp.Models;

namespace UniversityMgtApp.BLL
{
    public class CourseManager
    {
        CourseGateway courseGateway = new CourseGateway();
        public List<Semester> GetAllSemesters()
        {
            return courseGateway.GetAllSemesters();
        }

        public bool SaveCourse(Course course)
        {
            if (courseGateway.IsExistCourseCode(course.CourseCode))
            {
                throw new Exception("Code is exist!");
            }
            if (courseGateway.IsExistCourseName(course.CourseName))
            {
                throw new Exception("Name is exist");
            }
            return courseGateway.SaveCourse(course) > 0;
        }

        //public List<Course> GetAllCourse(int? departmentId)
        //{
        //    return courseGateway.GetAllCourse(departmentId);
        //}

        public List<Course> GetCourseDetail(int? studentId)
        {
            return courseGateway.GetCourseDetail(studentId);
        }

        public List<Course> GetEnrollCourseDetail(int? studentId)
        {
            return courseGateway.GetEnrollCourseDetail(studentId);
        }
    }
}