using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System.Web.Mvc;

namespace ACCBILLING_NOTE.NumberAuto
{
    public class AutoNumberDoc
    {

        public static string _Autonumber()
        {

            ConnectdataBase con = new ConnectdataBase();
            string sql = "", getDatainv = "", Month = "", year = "", DocOn = "", docData = "", getNumberDoc = "";
            sql = "SELECT TOP 1 OnInvoiceHeader FROM Acc_BillingDetail WHERE OnInvoiceHeader != '' ORDER BY Id DESC ";
            con.Opens();
            SqlCommand cmd = new SqlCommand(sql, con.cons);
            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                getDatainv = reader["OnInvoiceHeader"].ToString();
            }
            con.Closes();
            //getDatetime 
            DateTime date = DateTime.Now;
            Month = date.Month.ToString("00");
            year = date.Year.ToString("00");
            if (getDatainv.Length > 0)
            {
                docData = getDatainv.Substring(0, 9);
                getNumberDoc = getDatainv.Substring(10);
            }
            DocOn = "OCTB" + year.Substring(2) + "-" + Month;

            if (DocOn == docData)
            {
                DocOn = DocOn + "-" + (Convert.ToInt32(getNumberDoc) + 1).ToString("000");
            }
            else
            {
                DocOn = DocOn + "-" + (1).ToString("000");
            }


            return DocOn;
        }
    }
}