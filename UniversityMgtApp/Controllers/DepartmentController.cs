using System;
using System.Linq;
using System.Web.Mvc;
using UniversityMgtApp.BLL;
using UniversityMgtApp.Models;

namespace UniversityMgtApp.Controllers
{
    public class DepartmentController : Controller
    {
        public ActionResult Index()
        {
            DepartmentManager departmentManager = new DepartmentManager();
            var department = departmentManager.GetAllDepartments();

            return View(department.ToList());
        }

        public ActionResult Save()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Save(Department department)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DepartmentManager departmentManager = new DepartmentManager();

                    if (departmentManager.SaveDepartment(department))
                    {
                        ViewBag.Success = "Save Successfully";
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
	}
}