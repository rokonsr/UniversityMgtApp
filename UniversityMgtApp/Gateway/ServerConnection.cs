using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace UniversityMgtApp.Gateway
{
    public class ServerConnection
    {
        public SqlConnection Connection()
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            return con;
        }
    }
}