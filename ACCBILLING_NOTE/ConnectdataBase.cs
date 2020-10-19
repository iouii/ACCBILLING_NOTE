using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace ACCBILLING_NOTE
{
    public class ConnectdataBase
    {
        public SqlConnection con;
        public void OpenConnectionSql()
        { 
            string constring = ConfigurationManager.ConnectionStrings["ConnectSql"].ConnectionString;
            con = new SqlConnection(constring);

            con.Open();
        }
        public void CloseConnectionSql() {

            con.Close();
        }
    }
}