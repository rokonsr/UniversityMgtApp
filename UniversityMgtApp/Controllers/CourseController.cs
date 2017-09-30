using System;
using System.Web.Mvc;
using UniversityMgtApp.BLL;
using UniversityMgtApp.Models;

namespace UniversityMgtApp.Controllers
{
    public class CourseController : Controller
    {
        public ActionResult Save()
        {
            DepartmentManager departmentManager = new DepartmentManager();
            ViewBag.Department = departmentManager.GetAllDepartments();

            CourseManager courseManager = new CourseManager();
            ViewBag.Semester = courseManager.GetAllSemesters();

            return View();
        }

        [HttpPost]
        public ActionResult Save(Course course)
        {
            DepartmentManager departmentManager = new DepartmentManager();
            ViewBag.Department = departmentManager.GetAllDepartments();

            CourseManager courseManager = new CourseManager();
            ViewBag.Semester = courseManager.GetAllSemesters();
            
            try
            {
                if (ModelState.IsValid)
                {
                    courseManager.SaveCourse(course);
                    ViewBag.Success = "Save Successfully";
                    ModelState.Clear();
                }
            }
            catch (Exception exception)
            {
                ViewBag.Exist = exception.Message;
            }
            return View();
        }
	}
}