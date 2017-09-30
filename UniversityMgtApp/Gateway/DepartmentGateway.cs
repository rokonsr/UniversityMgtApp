using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using UniversityMgtApp.Models;

namespace UniversityMgtApp.Gateway
{
    public class DepartmentGateway
    {
        ServerConnection Server = new ServerConnection();
        public int SaveDepartment(Department department)
        {
            int count = 0;

            SqlCommand command = new SqlCommand("uspSaveDepartment", Server.Connection());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("Code", department.DepartmentCode);
            command.Parameters.AddWithValue("Name", department.DepartmentName);
            
            count = command.ExecuteNonQuery();
            Server.Connection().Close();

            return count;
        }

        public bool IsExistDepartmentCode(string code)
        {
            string query = "SELECT * FROM Department WHERE DepartmentCode = '" + code + "'";

            SqlCommand command = new SqlCommand(query, Server.Connection());
            SqlDataReader reader = command.ExecuteReader();

            bool IsExist = reader.HasRows;

            return IsExist;
        }

        public bool IsExistDepartmentName(string name)
        {
            string query = "SELECT * FROM Department WHERE DepartmentName = '" + name + "'";

            SqlCommand command = new SqlCommand(query, Server.Connection());
            SqlDataReader reader = command.ExecuteReader();

            bool IsExist = reader.HasRows;

            return IsExist;
        }

        public List<Department> GelAllDepartments()
        {
            List<Department> departments = new List<Department>();
            string query = "SELECT * FROM Department";

            SqlCommand command = new SqlCommand(query, Server.Connection());
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while(reader.Read())
                {
                    Department department = new Department();
                    department.DepartmentId = Convert.ToInt32(reader["DepartmentId"]);
                    department.DepartmentCode = reader["DepartmentCode"].ToString();
                    department.DepartmentName = reader["DepartmentName"].ToString();
                    departments.Add(department);
                }
            }
            return departments;
        }
    }
}