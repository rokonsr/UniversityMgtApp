using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using UniversityMgtApp.Models;

namespace UniversityMgtApp.Gateway
{
    public class CourseGateway
    {
        ServerConnection connection = new ServerConnection();
        public List<Semester> GetAllSemesters()
        {
            List<Semester> courses = new List<Semester>();

            string query = "SELECT * FROM Semester";

            SqlCommand command = new SqlCommand(query, connection.Connection());
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while(reader.Read())
                {
                    Semester course = new Semester
                    {
                        SemesterId = Convert.ToInt32(reader["SemesterId"]),
                        SemesterName = reader["SemesterName"].ToString()
                    };
                    courses.Add(course);
                }
            }
            return courses;
        }

        public int SaveCourse(Course course)
        {
            SqlCommand command = new SqlCommand("uspSaveCourse", connection.Connection());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("Code", course.CourseCode);
            command.Parameters.AddWithValue("Name", course.CourseName);
            command.Parameters.AddWithValue("Credit", course.Credit);
            command.Parameters.AddWithValue("Description", course.Description);
            command.Parameters.AddWithValue("DepartmentId", course.DepartmentId);
            command.Parameters.AddWithValue("SemesterId", course.SemesterId);

            int affectedRow = 0;

            affectedRow = command.ExecuteNonQuery();
            return affectedRow;
        }

        public bool IsExistCourseCode(string code)
        {
            string query = "SELECT * FROM Course WHERE CourseCode = '" + code + "'";

            SqlCommand command = new SqlCommand(query, connection.Connection());
            SqlDataReader reader = command.ExecuteReader();

            bool IsExist = reader.HasRows;

            return IsExist;
        }

        public bool IsExistCourseName(string name)
        {
            string query = "SELECT * FROM Course WHERE CourseName = '" + name + "'";

            SqlCommand command = new SqlCommand(query, connection.Connection());
            SqlDataReader reader = command.ExecuteReader();

            bool IsExist = reader.HasRows;

            return IsExist;
        }

        //public List<Course> GetAllCourse(int? departmentId)
        //{
        //    List<Course> courses = new List<Course>();

        //    string query = "SELECT * FROM Course WHERE DepartmentId = '" + departmentId + "'";

        //    SqlCommand command = new SqlCommand(query, server.Connection());
        //    SqlDataReader reader = command.ExecuteReader();

        //    if (reader.HasRows)
        //    {
        //        while (reader.Read())
        //        {
        //            Course course = new Course()
        //            {
        //                CourseId = Convert.ToInt32(reader["CourseId"]),
        //                CourseName = reader["CourseName"].ToString()
        //            };
        //            courses.Add(course);
        //        }
        //    }
        //    return courses;
        //}

        public List<Course> GetCourseDetail(int? studentId)
        {
            List<Course> courses = new List<Course>();
            SqlCommand command = new SqlCommand("uspGetCourseDetail", connection.Connection());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("StudentId", studentId);

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Course course = new Course()
                    {
                        CourseId = Convert.ToInt32(reader["CourseId"]),
                        CourseName = reader["CourseName"].ToString()
                    };
                    courses.Add(course);
                }
            }
            return courses;
        }

        public List<Course> GetEnrollCourseDetail(int? studentId)
        {
            List<Course> courses = new List<Course>();
            SqlCommand command = new SqlCommand("uspGetEnrollCourseDetail", connection.Connection());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("StudentId", studentId);

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Course course = new Course()
                    {
                        CourseId = Convert.ToInt32(reader["CourseId"]),
                        CourseName = reader["CourseName"].ToString()
                    };
                    courses.Add(course);
                }
            }
            return courses;
        }
    }
}