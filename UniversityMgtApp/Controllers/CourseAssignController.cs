using System;
using System.Web.Mvc;
using UniversityMgtApp.BLL;
using UniversityMgtApp.Models;

namespace UniversityMgtApp.Controllers
{
    public class CourseAssignController : Controller
    {
        public ActionResult Save()
        {
            DepartmentManager departmentManager = new DepartmentManager();
            ViewBag.Department = departmentManager.GetAllDepartments();

            return View();
        }

        [HttpPost]
        public ActionResult Save(CourseAssign courseAssign)
        {
            DepartmentManager departmentManager = new DepartmentManager();
            ViewBag.Department = departmentManager.GetAllDepartments();

            if (ModelState.IsValid)
            {
                try
                {
                    CourseAssignManager courseAssignManager = new CourseAssignManager();

                    if (courseAssignManager.SaveAssignCourse(courseAssign))
                    {
                        ViewBag.Success = "Assign Successfully";
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

        [HttpPost]
        public JsonResult GetTeacher(int? departmentId)
        {
            TeacherManager teacherManager = new TeacherManager();
            CourseAssignManager courseAssignManager = new CourseAssignManager();

            var teachers = teacherManager.GetAllTeachers(departmentId);
            var courses = courseAssignManager.GetAllCourseDetails(departmentId);

            var teacherCourses = new { courses = courses, teachers = teachers };
            return Json(teacherCourses, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTeacherDetail(int? teacherId)
        {
            TeacherManager teacherManager = new TeacherManager();
            var teacherDetails = teacherManager.GetTeacherDetails(teacherId);
            var teacherAssignCredit = teacherManager.GetTeacherAssignCredit(teacherId);

            var remainingCredit = teacherDetails.CreditToBeTaken - teacherAssignCredit;

            var creditToBeTakenRemainingCredit =
                new {remainingCredit = remainingCredit, teacherDetails = teacherDetails};

            return Json(creditToBeTakenRemainingCredit, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCourseDetail(int? courseId)
        {
            CourseAssignManager courseAssignManager = new CourseAssignManager();
            var courseDetails = courseAssignManager.GetCourseDetails(courseId);

            return Json(courseDetails, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewCourse()
        {
            DepartmentManager departmentManager = new DepartmentManager();
            ViewBag.Department = departmentManager.GetAllDepartments();

            return View();
        }

        public JsonResult CourseStatic(int? departmentId)
        {
            CourseAssignManager courseAssignManager = new CourseAssignManager();

            var courses = courseAssignManager.GetAllCourseDetails(departmentId);

            return Json(courses, JsonRequestBehavior.AllowGet);
        }
	}
}