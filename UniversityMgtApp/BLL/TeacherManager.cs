using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using UniversityMgtApp.Gateway;
using UniversityMgtApp.Models;

namespace UniversityMgtApp.BLL
{
    public class TeacherManager
    {
        TeacherGateway teacherGateway = new TeacherGateway();
        public List<Designation> GetAllDesignations()
        {
            return teacherGateway.GetAllDesignations();
        }

        public bool SaveTeacher(Teacher teacher)
        {
            if (teacherGateway.IsExistTeacherEmail(teacher.Email))
            {
                throw new Exception("Email address already exist");
            }
            return teacherGateway.SaveTeacher(teacher) > 0;
        }

        public List<Teacher> GetAllTeachers(int? departmentId)
        {
            return teacherGateway.GetAllTeachers(departmentId);
        }

        public Teacher GetTeacherDetails(int? teacherId)
        {
            return teacherGateway.GetTeacherDetails(teacherId);
        }

        public decimal GetTeacherAssignCredit(int? teacherId)
        {
            return teacherGateway.GetTeacherAssignCredit(teacherId);
        }
    }
}