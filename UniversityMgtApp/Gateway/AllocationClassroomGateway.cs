using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using UniversityMgtApp.Models;
using UniversityMgtApp.Models.ViewModel;

namespace UniversityMgtApp.Gateway
{
    public class AllocationClassroomGateway
    {
        ServerConnection connection = new ServerConnection();
        public List<Classroom> GetAllClassrooms()
        {
            List<Classroom> classrooms = new List<Classroom>();

            string query = "SELECT * FROM ClassRoom ";
            SqlCommand command = new SqlCommand(query, connection.Connection());
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
	        {
                while (reader.Read())
	            {
                    Classroom classroom = new Classroom()
                    {
                        ClassroomId = Convert.ToInt32(reader["ClassroomId"]),
                        RoomNumber = reader["RoomNumber"].ToString()
                    };
                    classrooms.Add(classroom);
	            }
	        }
            return classrooms;
        }

        public int AllocationClassRoom(ClassRoomAllocation classRoomAllocation)
        {
            SqlCommand command = new SqlCommand("uspAllocationClassRoom", connection.Connection());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("DepartmentId", classRoomAllocation.DepartmentId);
            command.Parameters.AddWithValue("CourseId", classRoomAllocation.CourseId);
            command.Parameters.AddWithValue("RoomId", classRoomAllocation.ClassRoomId);
            command.Parameters.AddWithValue("Day", classRoomAllocation.Day);
            command.Parameters.AddWithValue("StartTime",
                classRoomAllocation.StartTime + " " + classRoomAllocation.StartAmPm);
            command.Parameters.AddWithValue("EndTime", classRoomAllocation.EndTime + " " + classRoomAllocation.EndAmPm);

            int noOfAffectedRow = 0;

            noOfAffectedRow = command.ExecuteNonQuery();
            return noOfAffectedRow;
        }

        public bool IsExist(ClassRoomAllocation classRoomAllocation)
        {
            SqlCommand command = new SqlCommand("uspIsClassRoomAllocated", connection.Connection());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("ScheduleDate", classRoomAllocation.Day);
            command.Parameters.AddWithValue("StartTime",
                classRoomAllocation.StartTime + " " + classRoomAllocation.StartAmPm);
            command.Parameters.AddWithValue("EndTime", classRoomAllocation.EndTime + " " + classRoomAllocation.EndAmPm);
            command.Parameters.AddWithValue("Day", classRoomAllocation.Day);
            command.Parameters.AddWithValue("RoomNumber", classRoomAllocation.ClassRoomId);

            SqlDataReader reader = command.ExecuteReader();

            bool isExist = reader.HasRows;

            return isExist;
        }

        public List<ViewAllocationClassSchedule> GetClassScheduleStatus(int? departmentId)
        {
            List<ViewAllocationClassSchedule> viewAllocationClassSchedules = new List<ViewAllocationClassSchedule>();

            SqlCommand command = new SqlCommand("uspClassScheduleInfo", connection.Connection());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("DepartmentId", departmentId);

            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ViewAllocationClassSchedule viewAllocationClassSchedule = new ViewAllocationClassSchedule()
                    {
                        CourseCode = reader["CourseCode"].ToString(),
                        CourseName = reader["CourseName"].ToString(),
                        ScheduleInfo = reader["ScheduleInfo"].ToString()
                    };
                    viewAllocationClassSchedules.Add(viewAllocationClassSchedule);
                }
            }
            return viewAllocationClassSchedules;
        }

        public int UnallocationAllClassroom()
        {
            int noOfAffectedRow = 0;
            string query = "UPDATE ClassRoomAllocation SET IsAllocate = '" + 0 + "'";
            SqlCommand command = new SqlCommand(query, connection.Connection());

            noOfAffectedRow = command.ExecuteNonQuery();

            return noOfAffectedRow;
        }
    }
}