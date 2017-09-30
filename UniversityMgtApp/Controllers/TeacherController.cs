using System;
using System.Web.Mvc;
using UniversityMgtApp.BLL;
using UniversityMgtApp.Models;

namespace UniversityMgtApp.Controllers
{
    public class TeacherController : Controller
    {
        public ActionResult Save()
        {
            DepartmentManager departmentManager = new DepartmentManager();
            ViewBag.Department = departmentManager.GetAllDepartments();

            TeacherManager teacherManager = new TeacherManager();
            ViewBag.Designation = teacherManager.GetAllDesignations();

            return View();
        }

        [HttpPost]
        public ActionResult Save(Teacher teacher)
        {
            DepartmentManager departmentManager = new DepartmentManager();
            ViewBag.Department = departmentManager.GetAllDepartments();

            TeacherManager teacherManager = new TeacherManager();
            ViewBag.Designation = teacherManager.GetAllDesignations();

            try
            {
                if (ModelState.IsValid)
                {
                    teacherManager.SaveTeacher(teacher);

                    ViewBag.Success = "Save Succesfully";
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