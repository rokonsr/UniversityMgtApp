using System.Web.Mvc;
using UniversityMgtApp.BLL;

namespace UniversityMgtApp.Controllers
{
    public class ClassScheduleController : Controller
    {
        public ActionResult Index()
        {
            DepartmentManager departmentManager = new DepartmentManager();
            ViewBag.Department = departmentManager.GetAllDepartments();

            return View();
        }

        [HttpPost]
        public JsonResult ViewClassSchedule(int? departmentId)
        {
            AllocationClassRoomManager allocationClassRoomManager = new AllocationClassRoomManager();
            var classScheduleStatus = allocationClassRoomManager.GetClassScheduleStatus(departmentId);
            
            return Json(classScheduleStatus, JsonRequestBehavior.AllowGet);
        }
	}
}