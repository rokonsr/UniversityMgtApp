using System;
using System.Web.Mvc;
using UniversityMgtApp.BLL;

namespace UniversityMgtApp.Controllers
{
    public class ResetController : Controller
    {
        public ActionResult UnassignCourses()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UnassignCourses(int? departmentId)
        {
            CourseAssignManager courseAssignManager = new CourseAssignManager();

            try
            {
                if (courseAssignManager.UnassignAllCourses())
                {
                    ViewBag.Success = "Unassign all courses successfully";
                }
            }
            catch (Exception exception)
            {
                ViewBag.Exist = exception.Message;
            }
            return View();
        }

        public ActionResult UnallocateClassroom()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UnallocateClassroom(int? departmentId)
        {
            AllocationClassRoomManager allocationClassRoomManager = new AllocationClassRoomManager();

            try
            {
                if (allocationClassRoomManager.UnallocationAllClassroom())
                {
                    ViewBag.Success = "Unallocate all classroom";
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