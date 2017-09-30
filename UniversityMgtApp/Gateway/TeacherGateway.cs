using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using UniversityMgtApp.Models;

namespace UniversityMgtApp.Gateway
{
    public class TeacherGateway
    {
        ServerConnection server = new ServerConnection();
        public List<Designation> GetAllDesignations()
        {
            List<Designation> designations = new List<Designation>();

            string query = "SELECT * FROM Designation";
            SqlCommand command = new SqlCommand(query, server.Connection());
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Designation designation = new Designation
                    {
                        DesignationId = Convert.ToInt32(reader["DesignationId"]),
                        DesignationName = reader["DesignationName"].ToString()
                    };
                    designations.Add(designation);
                }
            }
            return designations;
        }

        public int SaveTeacher(Teacher teacher)
        {
            SqlCommand command = new SqlCommand("uspSaveTeacher", server.Connection());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("Name", teacher.TeacherName);
            command.Parameters.AddWithValue("Address", teacher.Address);
            command.Parameters.AddWithValue("Email", teacher.Email);
            command.Parameters.AddWithValue("ContactNumber", teacher.ContactNumber);
            command.Parameters.AddWithValue("DesignationId", teacher.DesignationId);
            command.Parameters.AddWithValue("DepartmentId", teacher.DepartmentId);
            command.Parameters.AddWithValue("Credit", teacher.CreditToBeTaken);

            int affectedRow = 0;

            affectedRow = command.ExecuteNonQuery();

            return affectedRow;
        }

        public bool IsExistTeacherEmail(string email)
        {
            string query = "SELECT * FROM Teacher WHERE Email = '" + email + "'";

            SqlCommand command = new SqlCommand(query, server.Connection());

            SqlDataReader reader = command.ExecuteReader();

            bool IsExist = reader.HasRows;
            return IsExist;
        }

        public List<Teacher> GetAllTeachers(int? departmentId)
        {
            List<Teacher> teachers = new List<Teacher>();

            string query = "SELECT * FROM Teacher WHERE DepartmentId = '" + departmentId + "'";

            SqlCommand command = new SqlCommand(query, server.Connection());
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Teacher teacher = new Teacher()
                    {
                        TeacherId = Convert.ToInt32(reader["TeacherId"]),
                        TeacherName = reader["TeacherName"].ToString()
                    };
                    teachers.Add(teacher);
                }
            }
            return teachers;
        }

        internal Teacher GetTeacherDetails(int? teacherId)
        {
            Teacher teacher = new Teacher();
            string query = "SELECT * FROM Teacher WHERE TeacherId = '" + teacherId + "'";

            SqlCommand command = new SqlCommand(query, server.Connection());
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    teacher = new Teacher()
                    {
                        CreditToBeTaken = Convert.ToDecimal(reader["CreditToBeTaken"])
                    };
                }
            }
            return teacher;
        }

        public decimal GetTeacherAssignCredit(int? teacherId)
        {
            SqlCommand command = new SqlCommand("uspGetTeacherAssignCredit", server.Connection());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("TeacherId", teacherId);

            SqlDataReader reader = command.ExecuteReader();

            decimal assignCredit = 0;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    assignCredit = Convert.ToDecimal(reader["AssignCredit"]);
                }
            }
            return assignCredit;
        }
    }
}