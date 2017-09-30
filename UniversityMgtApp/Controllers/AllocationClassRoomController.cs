using System;
using System.Web.Mvc;
using UniversityMgtApp.BLL;
using UniversityMgtApp.Models;

namespace UniversityMgtApp.Controllers
{
    public class AllocationClassRoomController : Controller
    {
        public ActionResult Save(int? departmentId)
        {
            DepartmentManager departmentManager = new DepartmentManager();
            ViewBag.Department = departmentManager.GetAllDepartments();

            //CourseAssignManager courseAssignManager = new CourseAssignManager();
            //ViewBag.Course = courseAssignManager.GetAllCourseDetails(departmentId);

            AllocationClassRoomManager allocationClassRoomManager = new AllocationClassRoomManager();
            ViewBag.Classroom = allocationClassRoomManager.GetAllClassrooms();

            return View();
        }

        [HttpPost]
        public ActionResult Save(ClassRoomAllocation classRoomAllocation)
        {
            DepartmentManager departmentManager = new DepartmentManager();
            ViewBag.Department = departmentManager.GetAllDepartments();

            //CourseAssignManager courseAssignManager = new CourseAssignManager();
            //ViewBag.Course = courseAssignManager.GetAllCourseDetails(classRoomAllocation.DepartmentId);

            AllocationClassRoomManager allocationClassRoomManager = new AllocationClassRoomManager();
            ViewBag.Classroom = allocationClassRoomManager.GetAllClassrooms();

            if (ModelState.IsValid)
            {
                try
                {
                    if (allocationClassRoomManager.AllocationClassRoom(classRoomAllocation))
                    {
                        ViewBag.Success = "Save successfully";
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