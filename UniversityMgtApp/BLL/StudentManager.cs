using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityMgtApp.Gateway;
using UniversityMgtApp.Models;

namespace UniversityMgtApp.BLL
{
    public class StudentManager
    {
        StudentGateway studentGateway = new StudentGateway();
        public string GenerateRegNumber(Student student)
        {
            return studentGateway.GenerateRegNumber(student);
        }

        public bool SaveStudent(Student student)
        {
            if (studentGateway.IsExistEmailAddress(student.Email))
            {
                throw new Exception("Email address exist");
            }
            return studentGateway.SaveStudent(student) > 0;
        }

        public List<Student> GetAllStudents()
        {
            return studentGateway.GetAllStudents();
        }

        public Student GetStudentDetail(int? studentId)
        {
            return studentGateway.GetStudentDetail(studentId);
        }

        public bool SaveResult(StudentResult saveResult)
        {
            if (studentGateway.IsSavedStudentResult(saveResult))
            {
                throw new Exception("Selected course result already saved!");
            }
            return studentGateway.SaveResult(saveResult) > 0;
        }

        public List<StudentResult> GetStudentResult(int? studentId)
        {
            return studentGateway.GetStudentResult(studentId);
        }
    }
}