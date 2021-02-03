using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace BOI_QUO
{
    public class ConnectdataBase
    {
        public SqlConnection con;
        public SqlConnection cons;
        public SqlConnection consThos;
        public void OpenConnectionSql()
        { 
            string constring = ConfigurationManager.ConnectionStrings["ConnectSql"].ConnectionString;
            con = new SqlConnection(constring);

            con.Open();
        }
        public void CloseConnectionSql() {

            con.Close();
        }
         public void Opens()
        { 
            string constring = ConfigurationManager.ConnectionStrings["ConnectSqls"].ConnectionString;
            cons = new SqlConnection(constring);

            cons.Open();
        }
        public void Closes() {

            cons.Close();
        }
           public void OpensThomas()
        {
            string constring = ConfigurationManager.ConnectionStrings["ConnectSqlThomas"].ConnectionString;
            consThos = new SqlConnection(constring);

            consThos.Open();
        }
        public void ClosesThomas() {

            consThos.Close();
        }
        
    }
 }
