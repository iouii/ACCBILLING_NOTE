using ACCBILLING_NOTE.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCBILLING_NOTE.Controllers
{
    public class ParController : Controller
    {
        // GET: Par
        ConnectdataBase con = new ConnectdataBase();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _PopupPartial()
        {

            string sqlPirce;

            Array asas;
            sqlPirce = "WITH INVOICE AS ( " +
         "SELECT CNTBTCH,IDCUST,IDINVC,FISCYR,FISCPER," +
         "TERMCODE,BASETAX1,AMTTAX1,AMTPYMSCHD,EXCHRATEHC,CODECURN," +
         "SHPTOSTE2,SHPTOSTE3,DATEINVC,DATEDUE,SHPTOPHON,SHPTOFAX FROM ARIBH WHERE FISCYR = '2020' AND FISCPER = '08' " +
   ") " +
   "SELECT " +
   "INV.IDINVC,INV.DATEINVC,INV.DATEDUE,INV.BASETAX1,INV.AMTTAX1,INV.AMTPYMSCHD " +
   "FROM INVOICE INV INNER JOIN ARSTCUS AST " +
   "ON INV.IDCUST = AST.IDCUST  WHERE AST.NAMECUST = 'MARELLI (THAILAND) CO.,LTD.'";


            con.OpenConnectionSql();

            DataTable dbp = new DataTable();
            SqlDataAdapter daprice = new SqlDataAdapter(sqlPirce, con.con);
            daprice.Fill(dbp);


            var json = new List<Invoice>();
            var row = dbp.Rows.Count;

            for (int i = 0; i < row; i++)
            {

                var year = dbp.Rows[i]["DATEINVC"].ToString().Substring(0, 4);
                var month = dbp.Rows[i]["DATEINVC"].ToString().Substring(4, 2);
                var day = dbp.Rows[i]["DATEINVC"].ToString().Substring(6, 2);
                var yearD = dbp.Rows[i]["DATEDUE"].ToString().Substring(0, 4);
                var monthD = dbp.Rows[i]["DATEDUE"].ToString().Substring(4, 2);
                var dayD = dbp.Rows[i]["DATEDUE"].ToString().Substring(6, 2);
                json.Add(new Invoice()
                {

                    invoiceNo = dbp.Rows[i]["IDINVC"].ToString(),
                    invoiceDate = day + "/" + month + "/" + year,
                    invoiceDue = dayD + "/" + monthD + "/" + yearD,
                    invoiceAm1 = Convert.ToDouble(dbp.Rows[i]["BASETAX1"]),
                    invoiceVat = Convert.ToDouble(dbp.Rows[i]["AMTTAX1"]),
                    invoiceAm2 = Convert.ToDouble(dbp.Rows[i]["AMTPYMSCHD"]),
                    invoiceAm1Cal = Convert.ToDouble(dbp.Rows[i]["BASETAX1"]).ToString("N2", CultureInfo.InvariantCulture),
                    invoiceVatCal = Convert.ToDouble(dbp.Rows[i]["AMTTAX1"]).ToString("N2", CultureInfo.InvariantCulture),
                    invoiceAm2Cal = Convert.ToDouble(dbp.Rows[i]["AMTPYMSCHD"]).ToString("N2", CultureInfo.InvariantCulture),

                });

            }

            asas = json.ToArray();

            return PartialView();
        }
    }
}