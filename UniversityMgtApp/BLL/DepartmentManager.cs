using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityMgtApp.Gateway;
using UniversityMgtApp.Models;

namespace UniversityMgtApp.BLL
{
    public class DepartmentManager
    {
        DepartmentGateway departmentGateway = new DepartmentGateway();

        public bool SaveDepartment(Department department)
        {
            if (departmentGateway.IsExistDepartmentCode(department.DepartmentCode))
            {
                throw new Exception("Code already exist!");
            }

            if (departmentGateway.IsExistDepartmentName(department.DepartmentName))
            {
                throw new Exception("Name already exist!");
            }

            return departmentGateway.SaveDepartment(department) > 0;
        }

        public List<Department> GetAllDepartments()
        {
            return departmentGateway.GelAllDepartments();
        }
    }
}