using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using UniversityMgtApp.Models;
using UniversityMgtApp.Models.ViewModel;

namespace UniversityMgtApp.Gateway
{
    public class CourseAssignGateway
    {
        ServerConnection connection = new ServerConnection();
        public CourseAssign GetCourseDetails(int? courseId)
        {
            CourseAssign courseAssign = new CourseAssign();
            string query = "SELECT * FROM Course WHERE CourseId = '" + courseId + "'";

            SqlCommand command = new SqlCommand(query, connection.Connection());
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    courseAssign.CourseName = reader["CourseName"].ToString();
                    courseAssign.CourseCredit = Convert.ToDecimal(reader["Credit"]);
                }
            }
            return courseAssign;
        }

        public int SaveAssignCourse(CourseAssign courseAssign)
        {
            SqlCommand command = new SqlCommand("uspSaveAssignCourse", connection.Connection());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("DepartmentId", courseAssign.DepartmentId);
            command.Parameters.AddWithValue("TeacherId", courseAssign.TeacherId);
            command.Parameters.AddWithValue("CourseId", courseAssign.CourseId);
            command.Parameters.AddWithValue("CourseCredit", courseAssign.CourseCredit);

            int noOfAffectedRow = 0;

            noOfAffectedRow = command.ExecuteNonQuery();

            return noOfAffectedRow;
        }

        public bool IsAssignedCourse(int courseId)
        {
            string query = "SELECT * FROM CourseAssign WHERE CourseId = '" + courseId + "' AND IsAssign = '" + 1 + "'";

            SqlCommand command = new SqlCommand(query, connection.Connection());
            SqlDataReader reader = command.ExecuteReader();

            bool isExist = reader.HasRows;

            return isExist;
        }

        public List<ViewCourseStatics> GetAllCourseDetails(int? departmentId)
        {
            List<ViewCourseStatics> viewCourseStaticses = new List<ViewCourseStatics>();

            SqlCommand command = new SqlCommand("uspGetAllCourseDetails", connection.Connection());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("DepartmentId", departmentId);

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ViewCourseStatics viewCourseStatics = new ViewCourseStatics()
                    {
                        CourseId = Convert.ToInt32(reader["CourseId"]),
                        CourseCode = reader["CourseCode"].ToString(),
                        CourseName = reader["CourseName"].ToString(),
                        Semester = reader["SemesterName"].ToString(),
                        TeacherName = reader["TeacherName"].ToString()
                    };
                    viewCourseStaticses.Add(viewCourseStatics);
                }
            }
            return viewCourseStaticses;
        }

        public int SaveCourseEnroll(CourseEnrollment courseEnrollment)
        {
            SqlCommand command = new SqlCommand("uspSaveCourseEnroll", connection.Connection());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("StudentId", courseEnrollment.StudentId);
            command.Parameters.AddWithValue("CourseId", courseEnrollment.CourseId);
            command.Parameters.AddWithValue("EnrollDate", courseEnrollment.EnrollDate);

            int noOfAffectedRow = 0;

            noOfAffectedRow = command.ExecuteNonQuery();

            return noOfAffectedRow;
        }

        public bool IsEnrolled(CourseEnrollment courseEnrollment)
        {
            bool IsEnrolled = false;

            string query = "SELECT * FROM CourseEnrollment WHERE StudentId = '" + courseEnrollment.StudentId +
                           "' AND CourseId = '" + courseEnrollment.CourseId + "'";

            SqlCommand command = new SqlCommand(query, connection.Connection());
            SqlDataReader reader = command.ExecuteReader();

            return IsEnrolled = reader.HasRows;
        }

        public int UnassignAllCourses()
        {
            int noOfAffectedRow = 0;

            string query = "UPDATE CourseAssign SET IsAssign = '" + 0 + "'";

            SqlCommand command = new SqlCommand(query, connection.Connection());
            noOfAffectedRow = command.ExecuteNonQuery();
            return noOfAffectedRow;
        }
    }
}