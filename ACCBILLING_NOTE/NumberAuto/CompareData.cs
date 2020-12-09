using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace ACCBILLING_NOTE.NumberAuto
{
    public class CompareData
    {
       
        public static string _getCompareData(string year, string month, string CodeCustName)
        {

            ConnectdataBase con = new ConnectdataBase();
            DataTable dt = new DataTable();
            int countRow = 0, i = 0;
            string resultData = "";
            string dateStart, dateEnd;
            dateStart = Convert.ToDateTime(month).ToString("dd-MM-yyyy");//Substring(month);
            dateEnd = Convert.ToDateTime(year).ToString("dd-MM-yyyy");//Substring(year);

            string sql = "SELECT OnInvoiceDetail FROM Acc_BillingDetail WHERE CONVERT(date,Inv_date,105) BETWEEN CONVERT(date,'" + dateStart + "',105) AND  CONVERT(date,'" + dateEnd + "',105) AND Codecustomer ='" + CodeCustName.Trim() + "' ";
            con.Opens();
            SqlDataAdapter da = new SqlDataAdapter(sql, con.cons);
            da.Fill(dt);
            countRow = dt.Rows.Count;
            con.Closes();

            for (i = 0; i < countRow; i++)
            {
                if (i == 0)
                {
                    resultData = "'" + dt.Rows[i]["OnInvoiceDetail"].ToString() + "'";
                }
                else
                {
                    resultData += ",'" + dt.Rows[i]["OnInvoiceDetail"] + "'";

                }
            }
            if (resultData!= "")
            {
                resultData = resultData.Replace("\"", "");
            }else{
                resultData = "''";
            }
            return resultData;
        }

        //not use 
        public static string Substring(string dateTime)
        {
            string dateSub = "";

            dateSub = dateTime.Substring(6) + "-" + dateTime.Substring(4, 2) + "-" + dateTime.Substring(0, 4);

            return dateSub;
        }
    }
}