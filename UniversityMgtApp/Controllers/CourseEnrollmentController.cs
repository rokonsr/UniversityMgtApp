using System;
using System.Web.Mvc;
using UniversityMgtApp.BLL;
using UniversityMgtApp.Models;

namespace UniversityMgtApp.Controllers
{
    public class CourseEnrollmentController : Controller
    {
        public ActionResult Save()
        {
            StudentManager studentManager = new StudentManager();
            ViewBag.StudentList = studentManager.GetAllStudents();
            return View();
        }

        [HttpPost]
        public ActionResult Save(CourseEnrollment courseEnrollment)
        {
            StudentManager studentManager = new StudentManager();
            ViewBag.StudentList = studentManager.GetAllStudents();

            if (ModelState.IsValid)
            {
                try
                {
                    CourseAssignManager courseAssignManager = new CourseAssignManager();
                    if (courseAssignManager.SaveCourseEnroll(courseEnrollment))
                    {
                        ViewBag.Success = "Course enroll successfully";
                        ModelState.Clear();
                    }
                }
                catch (Exception exception)
                {
                    ViewBag.Exist = exception.Message;
                }
            }
            return View();
        }

        public JsonResult GetStudentDetail(int? studentId)
        {
            StudentManager studentManager = new StudentManager();
            var studentDetail = studentManager.GetStudentDetail(studentId);

            CourseManager courseManager = new CourseManager();
            var courseDetail = courseManager.GetCourseDetail(studentId);

            var studentAndCourseDetail = new {studentDetail = studentDetail, courseDetail = courseDetail};

            return Json(studentAndCourseDetail, JsonRequestBehavior.AllowGet);
        }
	}
}