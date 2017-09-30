using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityMgtApp.Models;
using UniversityMgtApp.Gateway;
using UniversityMgtApp.Models.ViewModel;

namespace UniversityMgtApp.BLL
{
    public class AllocationClassRoomManager
    {
        AllocationClassroomGateway allocationClassRoomGateway = new AllocationClassroomGateway();
        public List<Classroom> GetAllClassrooms()
        {
            return allocationClassRoomGateway.GetAllClassrooms();
        }

        public bool AllocationClassRoom(ClassRoomAllocation classRoomAllocation)
        {
            if (allocationClassRoomGateway.IsExist(classRoomAllocation))
            {
                throw new Exception("Mentioned time already assigned");
            }

            return allocationClassRoomGateway.AllocationClassRoom(classRoomAllocation) > 0;
        }

        public List<ViewAllocationClassSchedule> GetClassScheduleStatus(int? departmentId)
        {
            return allocationClassRoomGateway.GetClassScheduleStatus(departmentId);
        }

        public bool UnallocationAllClassroom()
        {
            return allocationClassRoomGateway.UnallocationAllClassroom() > 0;
        }
    }
}