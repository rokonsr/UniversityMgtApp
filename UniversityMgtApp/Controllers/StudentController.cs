using System;
using System.Web.Mvc;
using UniversityMgtApp.BLL;
using UniversityMgtApp.Models;

namespace UniversityMgtApp.Controllers
{
    public class StudentController : Controller
    {
        public ActionResult Save()
        {
            DepartmentManager departmentManager = new DepartmentManager();
            ViewBag.Department = departmentManager.GetAllDepartments();

            return View();
        }

        [HttpPost]
        public ActionResult Save(Student student)
        {
            DepartmentManager departmentManager = new DepartmentManager();
            ViewBag.Department = departmentManager.GetAllDepartments();

            StudentManager studentManager = new StudentManager();
            student.RegNumber = studentManager.GenerateRegNumber(student);

            if (ModelState.IsValid)
            {
                try
                {
                    if (studentManager.SaveStudent(student))
                    {
                        ViewBag.Success = "Name : " + student.StudentName + ", Reg No. " + student.RegNumber +
                                          ". Registered Successfully";
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

        public ActionResult Result()
        {
            StudentManager studentManager = new StudentManager();
            ViewBag.StudentList = studentManager.GetAllStudents();

            return View();
        }

        [HttpPost]
        public ActionResult Result(StudentResult saveResult)
        {
            StudentManager studentManager = new StudentManager();
            ViewBag.StudentList = studentManager.GetAllStudents();

            if (ModelState.IsValid)
            {
                try
                {
                    if (studentManager.SaveResult(saveResult))
                    {
                        ViewBag.Success = "Result saved succefully";
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

        public JsonResult GetStudentDetailForResult(int? studentId)
        {
            StudentManager studentManager = new StudentManager();
            var studentDetail = studentManager.GetStudentDetail(studentId);

            CourseManager courseManager = new CourseManager();
            var courseDetail = courseManager.GetEnrollCourseDetail(studentId);

            var studentAndCourseDetail = new { studentDetail = studentDetail, courseDetail = courseDetail };

            return Json(studentAndCourseDetail, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewResult(int? studentId)
        {
            StudentManager studentManager = new StudentManager();
            ViewBag.StudentList = studentManager.GetAllStudents();

            return View();
        }

        public JsonResult GetStudentResultDetail(int? studentId)
        {
            StudentManager studentManager = new StudentManager();
            var studentDetail = studentManager.GetStudentDetail(studentId);

            var studentResult = studentManager.GetStudentResult(studentId);

            var studentAndResultDetail = new { studentDetail = studentDetail, studentResult = studentResult };

            return Json(studentAndResultDetail, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenteratePdf(int? studentId)
        {
            return new Rotativa.ActionAsPdf("GeneratePdf", new { studentId = studentId });
        }

        public ActionResult GeneratePdf(int? studentId)
        {
            StudentManager studentManager = new StudentManager();
            //ViewBag.StudentDetail = studentManager.GetStudentDetail(studentId);
            ViewBag.ResultDetail = studentManager.GetStudentResult(studentId);

            return View(studentManager.GetStudentDetail(studentId));
        }
	}
}