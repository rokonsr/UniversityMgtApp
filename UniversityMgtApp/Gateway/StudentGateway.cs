using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using UniversityMgtApp.Models;

namespace UniversityMgtApp.Gateway
{
    public class StudentGateway
    {
        ServerConnection connection = new ServerConnection();
        public string GenerateRegNumber(Student student)
        {
            string registrationNumber = "";
            SqlCommand command = new SqlCommand("uspGetRegistrationNumber", connection.Connection());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("DepartmentCode", student.DepartmentCode);

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                registrationNumber = reader["RegNumber"].ToString();
            }
            return registrationNumber;
        }

        public bool IsExistEmailAddress(string email)
        {
            bool isExist;

            string query = "SELECT * FROM Student WHERE Email = '" + email + "'";
            SqlCommand command = new SqlCommand(query, connection.Connection());
            SqlDataReader reader = command.ExecuteReader();

            isExist = reader.HasRows;
            return isExist;
        }

        public int SaveStudent(Student student)
        {
            int noOfAffectedRow = 0;

            SqlCommand command = new SqlCommand("uspSaveStudent", connection.Connection());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("RegNumber", student.RegNumber);
            command.Parameters.AddWithValue("StudentName", student.StudentName);
            command.Parameters.AddWithValue("Email", student.Email);
            command.Parameters.AddWithValue("ContactNumber", student.ContactNumber);
            command.Parameters.AddWithValue("Date", student.RegistrationDate);
            command.Parameters.AddWithValue("Address", student.Address);
            command.Parameters.AddWithValue("DepartmentCode", student.DepartmentCode);

            noOfAffectedRow = command.ExecuteNonQuery();

            return noOfAffectedRow;
        }

        public List<Student> GetAllStudents()
        {
            List<Student> students = new List<Student>();
            string query = "SELECT * FROM Student";
            SqlCommand command = new SqlCommand(query, connection.Connection());
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Student student = new Student()
                    {
                        StudentId = Convert.ToInt32(reader["StudentId"]),
                        RegNumber = reader["RegNumber"].ToString()
                    };
                    students.Add(student);
                }
            }
            return students;
        }

        public Student GetStudentDetail(int? studentId)
        {
            Student student = new Student();
            SqlCommand command = new SqlCommand("uspGetStudentDetail", connection.Connection());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("StudentId", studentId);

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    student = new Student()
                    {
                        StudentId = Convert.ToInt32(reader["StudentId"]),
                        RegNumber = reader["RegNumber"].ToString(),
                        StudentName = reader["StudentName"].ToString(),
                        Email = reader["Email"].ToString(),
                        ContactNumber = reader["ContactNumber"].ToString(),
                        RegistrationDate = Convert.ToDateTime(reader["RegistrationDate"]),
                        Address = reader["Address"].ToString(),
                        DepartmentName = reader["DepartmentName"].ToString()
                    };
                }
            }
            return student;
        }

        public int SaveResult(StudentResult saveResult)
        {
            SqlCommand command = new SqlCommand("uspSaveResult", connection.Connection());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("StudentId", saveResult.StudentId);
            command.Parameters.AddWithValue("CourseId", saveResult.CourseId);
            command.Parameters.AddWithValue("GradeLetter", saveResult.GradeLetter);

            int noOfAffectedRow = 0;

            noOfAffectedRow = command.ExecuteNonQuery();

            return noOfAffectedRow;
        }

        public bool IsSavedStudentResult(StudentResult saveResult)
        {
            bool isResultSaved = false;

            string query = "SELECT * FROM StudentResult WHERE StudentId = '" + saveResult.StudentId +
                           "' AND CourseId = '" + saveResult.CourseId + "'";

            SqlCommand command = new SqlCommand(query, connection.Connection());
            SqlDataReader reader = command.ExecuteReader();

            isResultSaved = reader.HasRows;
            return isResultSaved;
        }

        public List<StudentResult> GetStudentResult(int? studentId)
        {
            List<StudentResult> studentResults = new List<StudentResult>();

            SqlCommand command = new SqlCommand("uspGetStudentResult", connection.Connection());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("StudentId", studentId);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    StudentResult studentResult = new StudentResult()
                    {
                        CourseCode = reader["CourseCode"].ToString(),
                        CourseName = reader["CourseName"].ToString(),
                        GradeLetter = reader["GradeLetter"].ToString()
                    };
                    studentResults.Add(studentResult);
                }
            }
            return studentResults;
        }
    }
}