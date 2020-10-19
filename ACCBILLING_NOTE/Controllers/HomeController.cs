using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using ACCBILLING_NOTE.Models;
using Newtonsoft.Json;
using System.Globalization;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Web.Helpers;


namespace ACCBILLING_NOTE.Controllers
{
    public class HomeController : Controller
    {
      
        ConnectdataBase con = new ConnectdataBase();

        
        public ActionResult Index()
        {

            ViewBag.cus = TempData["Cus"];
            ViewBag.data = TempData["invoi"];
            ViewBag.datacount = TempData["count"];
            ViewBag.calAm1 = TempData["calAm1"];
            ViewBag.calAm2 = TempData["calAm2"];
            ViewBag.calV = TempData["calV"];
            ViewBag.TextCal = TempData["textCal"];     
                     

            return View();
        }

        public ActionResult Export()
        {

            ViewBag.cus = TempData["Cus"];
            ViewBag.data = TempData["invoi"];
            ViewBag.datacount = TempData["count"];
            ViewBag.calAm1 = TempData["calAm1"];
            ViewBag.calAm2 = TempData["calAm2"];
            ViewBag.calV = TempData["calV"];
            ViewBag.TextCal = TempData["textCal"];


            return View();
        }
        public ActionResult GetOver()
        {
            return View();
        }

       [HttpPost]
        public ActionResult GetOver(string name,string month,string year,string invoice)
        {

            string sqlPirce = "", sqlHeader;
            Array asas, cuss;

            sqlPirce = "WITH INVOICE AS ( " +
                  "SELECT  CNTBTCH,IDCUST,IDINVC,FISCYR,FISCPER," +
                  "TERMCODE,BASETAX1,AMTTAX1,AMTPYMSCHD,EXCHRATEHC,CODECURN," +
                  "SHPTOSTE2,SHPTOSTE3,DATEINVC,DATEDUE,SHPTOPHON,SHPTOFAX FROM ARIBH WHERE FISCYR = '" + year + "' AND FISCPER = '" + month + "' " +
            ") " +
            "SELECT DISTINCT " +
            "INV.IDINVC,INV.DATEINVC,INV.DATEDUE,INV.BASETAX1,INV.AMTTAX1,INV.AMTPYMSCHD " +
            "FROM INVOICE INV INNER JOIN ARSTCUS AST " +
            "ON INV.IDCUST = AST.IDCUST  WHERE INV.IDINVC IN (" + invoice + ")";

            sqlHeader = "WITH INVOICE AS ( " +
            "SELECT CNTBTCH,IDCUST,IDINVC,FISCYR,FISCPER, " +
            "TERMCODE,BASETAX1,AMTTAX1,AMTPYMSCHD,EXCHRATEHC,CODECURN, " +
            "SHPTOSTE2,SHPTOSTE3,DATEINVC,DATEDUE,SHPTOPHON,SHPTOFAX,SHPTOSTTE,SHPTOPOST,SHPTOCTRY  " +
            "FROM ARIBH WHERE FISCYR = '" + year + "' AND FISCPER = '" + month + "' " +
            ") " +
            "SELECT TOP 1 INV.IDCUST,ART.NAMECUST,ART.IDTAXREGI1," +
            "REPLACE(INV.SHPTOSTE2,' ','')+SPACE(2)+ " +
            "REPLACE(INV.SHPTOSTTE,' ','')+SPACE(2)+ " +
            "REPLACE(INV.SHPTOPOST,' ','')+SPACE(2)+ " +
            "REPLACE(INV.SHPTOCTRY,' ','') AS ADRRESS," +
            "INV.TERMCODE,INV.SHPTOPHON,INV.SHPTOFAX , ART.NAMECTAC " +
            "FROM INVOICE INV INNER JOIN ARSTCUS ART " +
            "ON INV.IDCUST = ART.IDCUST WHERE ART.NAMECUST = '" + name + "' ";






            con.OpenConnectionSql();

            DataTable dbp = new DataTable();
            SqlDataAdapter daprice = new SqlDataAdapter(sqlPirce, con.con);
            daprice.Fill(dbp);


            DataTable dbh = new DataTable();
            SqlDataAdapter daheader = new SqlDataAdapter(sqlHeader, con.con);
            daheader.Fill(dbh);




            var json = new List<Invoice>();
            var row = dbp.Rows.Count;

            for (int i = 0; i < row; i++)
            {

                var yearT = dbp.Rows[i]["DATEINVC"].ToString().Substring(0, 4);
                var monthT = dbp.Rows[i]["DATEINVC"].ToString().Substring(4, 2);
                var day = dbp.Rows[i]["DATEINVC"].ToString().Substring(6, 2);
                var yearD = dbp.Rows[i]["DATEDUE"].ToString().Substring(0, 4);
                var monthD = dbp.Rows[i]["DATEDUE"].ToString().Substring(4, 2);
                var dayD = dbp.Rows[i]["DATEDUE"].ToString().Substring(6, 2);
                json.Add(new Invoice()
                {

                    invoiceNo = dbp.Rows[i]["IDINVC"].ToString(),
                    invoiceDate = day + "/" + monthT + "/" + yearT,
                    invoiceDue = dayD + "/" + monthD + "/" + yearD,
                    invoiceAm1 = Convert.ToDouble(dbp.Rows[i]["BASETAX1"]),
                    invoiceVat = Convert.ToDouble(dbp.Rows[i]["AMTTAX1"]),
                    invoiceAm2 = Convert.ToDouble(dbp.Rows[i]["AMTPYMSCHD"]),
                    invoiceAm1Cal = Convert.ToDouble(dbp.Rows[i]["BASETAX1"]).ToString("N2", CultureInfo.InvariantCulture),
                    invoiceVatCal = Convert.ToDouble(dbp.Rows[i]["AMTTAX1"]).ToString("N2", CultureInfo.InvariantCulture),
                    invoiceAm2Cal = Convert.ToDouble(dbp.Rows[i]["AMTPYMSCHD"]).ToString("N2", CultureInfo.InvariantCulture),

                });




            }


            var jsonAd = new List<InvoiceCus>();
            var rowCus = dbh.Rows.Count;
            byte textD;
            for (int i = 0; i < rowCus; i++)
            {
                var text =dbh.Rows[i]["TERMCODE"].ToString();
                if (text == "DTABLE")
                {

                 textD = 150;

                }
                else
                {

                 textD = Convert.ToByte(dbh.Rows[i]["TERMCODE"].ToString().Substring(0, 3));
                }
                
                jsonAd.Add(new InvoiceCus()
                {

                    idCust = dbh.Rows[i]["IDCUST"].ToString(),
                    custName = dbh.Rows[i]["NAMECUST"].ToString(),
                    custTex = dbh.Rows[i]["IDTAXREGI1"].ToString(),
                    custAdress = dbh.Rows[i]["ADRRESS"].ToString(),
                    custTerm = textD.ToString(),
                    //custTerm = dbh.Rows[i]["TERMCODE"].ToString(),
                    custPhone = dbh.Rows[i]["SHPTOPHON"].ToString(),
                    custFax = dbh.Rows[i]["SHPTOFAX"].ToString(),



                });


            }


            cuss = jsonAd.ToArray();
            ViewBag.cus = cuss;

            // var model = JsonConvert.SerializeObject(json);

            var cal = json.Sum(c => c.invoiceAm1);
            var calV = json.Sum(c => c.invoiceVat);
            var calAm = json.Sum(c => c.invoiceAm2);
            var calText = calAm.ToString();

            asas = json.ToArray();
            ViewBag.data = asas;
            ViewBag.datacount = json.Count();
            ViewBag.calAm1 = cal.ToString("N2", CultureInfo.InvariantCulture);
            ViewBag.calAm2 = calAm.ToString("N2", CultureInfo.InvariantCulture);
            ViewBag.calV = calV.ToString("N2", CultureInfo.InvariantCulture);

            ViewBag.TextCal = ThaiBahtText(calText);

            TempData["invoi"] = asas;

            TempData["calAm1"] = cal.ToString("N2", CultureInfo.InvariantCulture);
            TempData["calAm2"] = calAm.ToString("N2", CultureInfo.InvariantCulture);
            TempData["calV"] =   calV.ToString("N2", CultureInfo.InvariantCulture);
            TempData["textCal"] = ViewBag.TextCal;
            TempData["Cus"]     = cuss;
            TempData["count"] = ViewBag.datacount;

            return View("Index");
        }


        public ActionResult Test()
        {

            ViewBag.cusname = cus();

            return View();
        }



        public ActionResult GetExport()
        {
            return View();
        }


        [HttpPost]
        public ActionResult GetExport(string name, string month, string year, string invoice)
        {

            string sqlPirce = "", sqlHeader;
            Array asas, cuss;

            sqlPirce = "WITH INVOICE AS ( " +
                  "SELECT  CNTBTCH,IDCUST,IDINVC,FISCYR,FISCPER," +
                  "TERMCODE,BASETAX1,AMTTAX1,AMTPYMSCHD,EXCHRATEHC,CODECURN," +
                  "SHPTOSTE2,SHPTOSTE3,DATEINVC,DATEDUE,SHPTOPHON,SHPTOFAX FROM ARIBH WHERE FISCYR = '" + year + "' AND FISCPER = '" + month + "' " +
            ") " +
            "SELECT DISTINCT " +
            "INV.IDINVC,INV.DATEINVC,INV.DATEDUE,INV.BASETAX1,INV.AMTTAX1,INV.AMTPYMSCHD " +
            "FROM INVOICE INV INNER JOIN ARSTCUS AST " +
            "ON INV.IDCUST = AST.IDCUST  WHERE INV.IDINVC IN (" + invoice + ")";

            sqlHeader = "WITH INVOICE AS ( " +
            "SELECT CNTBTCH,IDCUST,IDINVC,FISCYR,FISCPER, " +
            "TERMCODE,BASETAX1,AMTTAX1,AMTPYMSCHD,EXCHRATEHC,CODECURN, " +
            "SHPTOSTE2,SHPTOSTE3,DATEINVC,DATEDUE,SHPTOPHON,SHPTOFAX,SHPTOSTTE,SHPTOPOST,SHPTOCTRY  " +
            "FROM ARIBH WHERE FISCYR = '" + year + "' AND FISCPER = '" + month + "' " +
            ") " +
            "SELECT TOP 1 INV.IDCUST,ART.NAMECUST,ART.IDTAXREGI1," +
            "REPLACE(INV.SHPTOSTE2,' ','')+SPACE(2)+ " +
            "REPLACE(INV.SHPTOSTTE,' ','')+SPACE(2)+ " +
            "REPLACE(INV.SHPTOPOST,' ','')+SPACE(2)+ " +
            "REPLACE(INV.SHPTOCTRY,' ','') AS ADRRESS," +
            "INV.TERMCODE,INV.SHPTOPHON,INV.SHPTOFAX , ART.NAMECTAC " +
            "FROM INVOICE INV INNER JOIN ARSTCUS ART " +
            "ON INV.IDCUST = ART.IDCUST WHERE ART.NAMECUST = '" + name + "' ";






            con.OpenConnectionSql();

            DataTable dbp = new DataTable();
            SqlDataAdapter daprice = new SqlDataAdapter(sqlPirce, con.con);
            daprice.Fill(dbp);


            DataTable dbh = new DataTable();
            SqlDataAdapter daheader = new SqlDataAdapter(sqlHeader, con.con);
            daheader.Fill(dbh);




            var json = new List<Invoice>();
            var row = dbp.Rows.Count;

            for (int i = 0; i < row; i++)
            {

                var yearT = dbp.Rows[i]["DATEINVC"].ToString().Substring(0, 4);
                var monthT = dbp.Rows[i]["DATEINVC"].ToString().Substring(4, 2);
                var day = dbp.Rows[i]["DATEINVC"].ToString().Substring(6, 2);
                var yearD = dbp.Rows[i]["DATEDUE"].ToString().Substring(0, 4);
                var monthD = dbp.Rows[i]["DATEDUE"].ToString().Substring(4, 2);
                var dayD = dbp.Rows[i]["DATEDUE"].ToString().Substring(6, 2);
                json.Add(new Invoice()
                {

                    invoiceNo = dbp.Rows[i]["IDINVC"].ToString(),
                    invoiceDate = day + "-" + monthT + "-" + yearT,
                    invoiceDue = dayD + "-" + monthD + "-" + yearD,
                    invoiceAm1 = Convert.ToDouble(dbp.Rows[i]["BASETAX1"]),
                    invoiceVat = Convert.ToDouble(dbp.Rows[i]["AMTTAX1"]),
                    invoiceAm2 = Convert.ToDouble(dbp.Rows[i]["AMTPYMSCHD"]),
                    invoiceAm1Cal = Convert.ToDouble(dbp.Rows[i]["BASETAX1"]).ToString("N2", CultureInfo.InvariantCulture),
                    invoiceVatCal = Convert.ToDouble(dbp.Rows[i]["AMTTAX1"]).ToString("N2", CultureInfo.InvariantCulture),
                    invoiceAm2Cal = Convert.ToDouble(dbp.Rows[i]["AMTPYMSCHD"]).ToString("N2", CultureInfo.InvariantCulture),

                });




            }


            var jsonAd = new List<InvoiceCus>();
            var rowCus = dbh.Rows.Count;

            byte textD;

            for (int i = 0; i < rowCus; i++)
            {



                var text = dbh.Rows[i]["TERMCODE"].ToString();
                if (text == "DTABLE")
                {

                    textD = 150;

                }
                else
                {

                    textD = Convert.ToByte(dbh.Rows[i]["TERMCODE"].ToString().Substring(0, 3));
                }

                jsonAd.Add(new InvoiceCus()
                {

                    idCust = dbh.Rows[i]["IDCUST"].ToString(),
                    custName = dbh.Rows[i]["NAMECUST"].ToString(),
                    custTex = dbh.Rows[i]["IDTAXREGI1"].ToString(),
                    custAdress = dbh.Rows[i]["ADRRESS"].ToString(),
                    custTerm = dbh.Rows[i]["TERMCODE"].ToString(),
                    custPhone = dbh.Rows[i]["SHPTOPHON"].ToString(),
                    custFax = dbh.Rows[i]["SHPTOFAX"].ToString(),



                });


            }


            cuss = jsonAd.ToArray();
            ViewBag.cus = cuss;

            // var model = JsonConvert.SerializeObject(json);

            var cal = json.Sum(c => c.invoiceAm1);
            var calV = json.Sum(c => c.invoiceVat);
            var calAm = json.Sum(c => c.invoiceAm2);
            var calText = calAm.ToString();

            asas = json.ToArray();
            ViewBag.data = asas;
            ViewBag.datacount = json.Count();
            ViewBag.calAm1 = cal.ToString("N2", CultureInfo.InvariantCulture);
            ViewBag.calAm2 = calAm.ToString("N2", CultureInfo.InvariantCulture);
            ViewBag.calV = calV.ToString("N2", CultureInfo.InvariantCulture);

            ViewBag.TextCal = ThaiBahtText(calText);

            TempData["invoi"] = asas;

            TempData["calAm1"] = cal.ToString("N2", CultureInfo.InvariantCulture);
            TempData["calAm2"] = calAm.ToString("N2", CultureInfo.InvariantCulture);
            TempData["calV"] = calV.ToString("N2", CultureInfo.InvariantCulture);
            TempData["textCal"] = ViewBag.TextCal;
            TempData["Cus"] = cuss;
            TempData["count"] = ViewBag.datacount;

            return View("Export");
        }


        [HttpPost]
        public ActionResult Test(string name,string month,string year)
        {
           
          var sh  = nameQ(name,month,year);
         
         return Json(sh,JsonRequestBehavior.AllowGet);
        }

 

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public Array cus()
        {
            string sqlq;
            Array cusname;


            con.OpenConnectionSql();
            sqlq = " SELECT DISTINCT IDCUST,NAMECUST FROM  ARSTCUS  ";

            DataTable dbq = new DataTable();
            SqlDataAdapter daq = new SqlDataAdapter(sqlq, con.con);

            daq.Fill(dbq);



            con.CloseConnectionSql();

            var jsonCusname = new List<InvoiceCus>();
            var rowName = dbq.Rows.Count;
            for (int i = 0; i < rowName; i++)
            {
                jsonCusname.Add(new InvoiceCus()
                {
                    idCust = dbq.Rows[i]["IDCUST"].ToString(),
                    custName = dbq.Rows[i]["NAMECUST"].ToString(),


                });
            }

            cusname = jsonCusname.ToArray();


            return cusname;
        }

        public List<Invoice> nameQ(string name,string month,string year)
        {

            string sqlPirce;

           
            sqlPirce = "WITH INVOICE AS ( " +
         "SELECT CNTBTCH,IDCUST,IDINVC,FISCYR,FISCPER," +
         "TERMCODE,BASETAX1,AMTTAX1,AMTPYMSCHD,EXCHRATEHC,CODECURN," +
         "SHPTOSTE2,SHPTOSTE3,DATEINVC,DATEDUE,SHPTOPHON,SHPTOFAX FROM ARIBH WHERE FISCYR = '"+year+"' AND FISCPER = '"+month+"' " +
           ") " +
           "SELECT " +
           "INV.IDINVC,INV.DATEINVC,INV.DATEDUE,INV.BASETAX1,INV.AMTTAX1,INV.AMTPYMSCHD " +
           "FROM INVOICE INV INNER JOIN ARSTCUS AST " +
           "ON INV.IDCUST = AST.IDCUST  WHERE AST.NAMECUST = '"+ name +"' ";


            con.OpenConnectionSql();

            DataTable dbp = new DataTable();
            SqlDataAdapter daprice = new SqlDataAdapter(sqlPirce, con.con);
            daprice.Fill(dbp);


            var json = new List<Invoice>();
            var row = dbp.Rows.Count;
            

            for (int i = 0; i < row; i++)
            {

                var yearT = dbp.Rows[i]["DATEINVC"].ToString().Substring(0, 4);
                var monthT= dbp.Rows[i]["DATEINVC"].ToString().Substring(4, 2);
                var day = dbp.Rows[i]["DATEINVC"].ToString().Substring(6, 2);
                var yearD = dbp.Rows[i]["DATEDUE"].ToString().Substring(0, 4);
                var monthD = dbp.Rows[i]["DATEDUE"].ToString().Substring(4, 2);
                var dayD = dbp.Rows[i]["DATEDUE"].ToString().Substring(6, 2);
                json.Add(new Invoice()
                {

                    invoiceNo = dbp.Rows[i]["IDINVC"].ToString(),
                    invoiceDate = day + "/" + monthT + "/" + yearT,
                    invoiceDue = dayD + "/" + monthD + "/" + yearD,
                    invoiceAm1 = Convert.ToDouble(dbp.Rows[i]["BASETAX1"]),
                    invoiceVat = Convert.ToDouble(dbp.Rows[i]["AMTTAX1"]),
                    invoiceAm2 = Convert.ToDouble(dbp.Rows[i]["AMTPYMSCHD"]),
                    invoiceAm1Cal = Convert.ToDouble(dbp.Rows[i]["BASETAX1"]).ToString("N2", CultureInfo.InvariantCulture),
                    invoiceVatCal = Convert.ToDouble(dbp.Rows[i]["AMTTAX1"]).ToString("N2", CultureInfo.InvariantCulture),
                    invoiceAm2Cal = Convert.ToDouble(dbp.Rows[i]["AMTPYMSCHD"]).ToString("N2", CultureInfo.InvariantCulture),
                    
                
                });

              
            }

        

         
            return  json;
            
        }

        //public List<InvoiceCus> cusHead(string name,string month,string year)
        //{

        //    string sqlHeader;

        //    sqlHeader = "WITH INVOICE AS ( " +
        //   "SELECT CNTBTCH,IDCUST,IDINVC,FISCYR,FISCPER, " +
        //   "TERMCODE,BASETAX1,AMTTAX1,AMTPYMSCHD,EXCHRATEHC,CODECURN, " +
        //   "SHPTOSTE2,SHPTOSTE3,DATEINVC,DATEDUE,SHPTOPHON,SHPTOFAX,SHPTOSTTE,SHPTOPOST,SHPTOCTRY  " +
        //   "FROM ARIBH WHERE FISCYR = '"+year+"' AND FISCPER = '"+month+"' " +
        //   ") " +
        //   "SELECT TOP 1 INV.IDCUST,ART.NAMECUST,ART.IDTAXREGI1," +
        //   "REPLACE(INV.SHPTOSTE2,' ','')+SPACE(2)+ " +
        //   "REPLACE(INV.SHPTOSTTE,' ','')+SPACE(2)+ " +
        //   "REPLACE(INV.SHPTOPOST,' ','')+SPACE(2)+ " +
        //   "REPLACE(INV.SHPTOCTRY,' ','') AS ADRRESS," +
        //   "INV.TERMCODE,INV.SHPTOPHON,INV.SHPTOFAX " +
        //   "FROM INVOICE INV INNER JOIN ARSTCUS ART " +
        //   "ON INV.IDCUST = ART.IDCUST WHERE ART.NAMECUST = '" + name + "' ";

        //    con.OpenConnectionSql();

        //    DataTable dbh = new DataTable();
        //    SqlDataAdapter daheader = new SqlDataAdapter(sqlHeader, con.con);
        //    daheader.Fill(dbh);
        //    var jsonAd = new List<InvoiceCus>();
        //    var rowCus = dbh.Rows.Count;

        //    for (int i = 0; i < rowCus; i++)
        //    {
        //        jsonAd.Add(new InvoiceCus()
        //        {

        //            idCust = dbh.Rows[i]["IDCUST"].ToString(),
        //            custName = dbh.Rows[i]["NAMECUST"].ToString(),
        //            custTex = dbh.Rows[i]["IDTAXREGI1"].ToString(),
        //            custAdress = dbh.Rows[i]["ADRRESS"].ToString(),
        //            custTerm = dbh.Rows[i]["TERMCODE"].ToString(),
        //            custPhone = dbh.Rows[i]["SHPTOPHON"].ToString(),
        //            custFax = dbh.Rows[i]["SHPTOFAX"].ToString(),



        //        });




        //    }



        //    return jsonAd;
        //}



        //public void call(string name,string month,string year,string invoice)
        //{

        //    string sqlPirce = "", sqlHeader;
        //    Array asas, cuss;

        //    sqlPirce = "WITH INVOICE AS ( " +
        //          "SELECT CNTBTCH,IDCUST,IDINVC,FISCYR,FISCPER," +
        //          "TERMCODE,BASETAX1,AMTTAX1,AMTPYMSCHD,EXCHRATEHC,CODECURN," +
        //          "SHPTOSTE2,SHPTOSTE3,DATEINVC,DATEDUE,SHPTOPHON,SHPTOFAX FROM ARIBH WHERE FISCYR = '"+year+"' AND FISCPER = '"+month+"' " +
        //    ") " +
        //    "SELECT " +
        //    "INV.IDINVC,INV.DATEINVC,INV.DATEDUE,INV.BASETAX1,INV.AMTTAX1,INV.AMTPYMSCHD " +
        //    "FROM INVOICE INV INNER JOIN ARSTCUS AST " +
        //    "ON INV.IDCUST = AST.IDCUST  WHERE INV.IDINVC IN ("+invoice+")";

        //    sqlHeader = "WITH INVOICE AS ( " +
        //    "SELECT CNTBTCH,IDCUST,IDINVC,FISCYR,FISCPER, " +
        //    "TERMCODE,BASETAX1,AMTTAX1,AMTPYMSCHD,EXCHRATEHC,CODECURN, " +
        //    "SHPTOSTE2,SHPTOSTE3,DATEINVC,DATEDUE,SHPTOPHON,SHPTOFAX,SHPTOSTTE,SHPTOPOST,SHPTOCTRY  " +
        //    "FROM ARIBH WHERE FISCYR = '" + year + "' AND FISCPER = '"+month+"' " +
        //    ") " +
        //    "SELECT TOP 1 INV.IDCUST,ART.NAMECUST,ART.IDTAXREGI1," +
        //    "REPLACE(INV.SHPTOSTE2,' ','')+SPACE(2)+ " +
        //    "REPLACE(INV.SHPTOSTTE,' ','')+SPACE(2)+ " +
        //    "REPLACE(INV.SHPTOPOST,' ','')+SPACE(2)+ " +
        //    "REPLACE(INV.SHPTOCTRY,' ','') AS ADRRESS," +
        //    "INV.TERMCODE,INV.SHPTOPHON,INV.SHPTOFAX " +
        //    "FROM INVOICE INV INNER JOIN ARSTCUS ART " +
        //    "ON INV.IDCUST = ART.IDCUST WHERE ART.NAMECUST = '"+name+"' ";






        //    con.OpenConnectionSql();

        //    DataTable dbp = new DataTable();
        //    SqlDataAdapter daprice = new SqlDataAdapter(sqlPirce, con.con);
        //    daprice.Fill(dbp);


        //    DataTable dbh = new DataTable();
        //    SqlDataAdapter daheader = new SqlDataAdapter(sqlHeader, con.con);
        //    daheader.Fill(dbh);




        //    var json = new List<Invoice>();
        //    var row = dbp.Rows.Count;

        //    for (int i = 0; i < row; i++)
        //    {

        //        var yearT = dbp.Rows[i]["DATEINVC"].ToString().Substring(0, 4);
        //        var monthT = dbp.Rows[i]["DATEINVC"].ToString().Substring(4, 2);
        //        var day = dbp.Rows[i]["DATEINVC"].ToString().Substring(6, 2);
        //        var yearD = dbp.Rows[i]["DATEDUE"].ToString().Substring(0, 4);
        //        var monthD = dbp.Rows[i]["DATEDUE"].ToString().Substring(4, 2);
        //        var dayD = dbp.Rows[i]["DATEDUE"].ToString().Substring(6, 2);
        //        json.Add(new Invoice()
        //        {

        //            invoiceNo = dbp.Rows[i]["IDINVC"].ToString(),
        //            invoiceDate = day + "/" + monthT + "/" + yearT,
        //            invoiceDue = dayD + "/" + monthD + "/" + yearD,
        //            invoiceAm1 = Convert.ToDouble(dbp.Rows[i]["BASETAX1"]),
        //            invoiceVat = Convert.ToDouble(dbp.Rows[i]["AMTTAX1"]),
        //            invoiceAm2 = Convert.ToDouble(dbp.Rows[i]["AMTPYMSCHD"]),
        //            invoiceAm1Cal = Convert.ToDouble(dbp.Rows[i]["BASETAX1"]).ToString("N2", CultureInfo.InvariantCulture),
        //            invoiceVatCal = Convert.ToDouble(dbp.Rows[i]["AMTTAX1"]).ToString("N2", CultureInfo.InvariantCulture),
        //            invoiceAm2Cal = Convert.ToDouble(dbp.Rows[i]["AMTPYMSCHD"]).ToString("N2", CultureInfo.InvariantCulture),

        //        });




        //    }


        //    var jsonAd = new List<InvoiceCus>();
        //    var rowCus = dbh.Rows.Count;

        //    for (int i = 0; i < rowCus; i++)
        //    {
        //        jsonAd.Add(new InvoiceCus()
        //        {

        //            idCust = dbh.Rows[i]["IDCUST"].ToString(),
        //            custName = dbh.Rows[i]["NAMECUST"].ToString(),
        //            custTex = dbh.Rows[i]["IDTAXREGI1"].ToString(),
        //            custAdress = dbh.Rows[i]["ADRRESS"].ToString(),
        //            custTerm = dbh.Rows[i]["TERMCODE"].ToString(),
        //            custPhone = dbh.Rows[i]["SHPTOPHON"].ToString(),
        //            custFax = dbh.Rows[i]["SHPTOFAX"].ToString(),



        //        });




        //    }

            
            
        //    cuss = jsonAd.ToArray();
        //    ViewBag.cus = cuss;

        //    // var model = JsonConvert.SerializeObject(json);

        //    var cal = json.Sum(c => c.invoiceAm1);
        //    var calV = json.Sum(c => c.invoiceVat);
        //    var calAm = json.Sum(c => c.invoiceAm2);
        //    var calText = calAm.ToString();

        //    asas = json.ToArray();
        //    ViewBag.data = asas;
        //    ViewBag.datacount = json.Count();
        //    ViewBag.calAm1 = cal.ToString("N2", CultureInfo.InvariantCulture);
        //    ViewBag.calAm2 = calAm.ToString("N2", CultureInfo.InvariantCulture);
        //    ViewBag.calV = calV.ToString("N2", CultureInfo.InvariantCulture);

        //    ViewBag.TextCal = ThaiBahtText(calText);

            
            
        //}


        public string ThaiBahtText(string strNumber, bool IsTrillion = false)
        {
            string BahtText = "";
            string strTrillion = "";
            string[] strThaiNumber = { "ศูนย์", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า", "สิบ" };
            string[] strThaiPos = { "", "สิบ", "ร้อย", "พัน", "หมื่น", "แสน", "ล้าน" };

            decimal decNumber = 0;
            decimal.TryParse(strNumber, out decNumber);

            if (decNumber == 0)
            {
                return "ศูนย์บาทถ้วน";
            }

            strNumber = decNumber.ToString("0.00");
            string strInteger = strNumber.Split('.')[0];
            string strSatang = strNumber.Split('.')[1];

            if (strInteger.Length > 13)
                throw new Exception("รองรับตัวเลขได้เพียง ล้านล้าน เท่านั้น!");

            bool _IsTrillion = strInteger.Length > 7;
            if (_IsTrillion)
            {
                strTrillion = strInteger.Substring(0, strInteger.Length - 6);
                BahtText = ThaiBahtText(strTrillion, _IsTrillion);
                strInteger = strInteger.Substring(strTrillion.Length);
            }

            int strLength = strInteger.Length;
            for (int i = 0; i < strInteger.Length; i++)
            {
                string number = strInteger.Substring(i, 1);
                if (number != "0")
                {
                    if (i == strLength - 1 && number == "1" && strLength != 1)
                    {
                        BahtText += "เอ็ด";
                    }
                    else if (i == strLength - 2 && number == "2" && strLength != 1)
                    {
                        BahtText += "ยี่";
                    }
                    else if (i != strLength - 2 || number != "1")
                    {
                        BahtText += strThaiNumber[int.Parse(number)];
                    }

                    BahtText += strThaiPos[(strLength - i) - 1];
                }
            }

            if (IsTrillion)
            {
                return BahtText + "ล้าน";
            }

            if (strInteger != "0")
            {
                BahtText += "บาท";
            }

            if (strSatang == "00")
            {
                BahtText += "ถ้วน";
            }
            else
            {
                strLength = strSatang.Length;
                for (int i = 0; i < strSatang.Length; i++)
                {
                    string number = strSatang.Substring(i, 1);
                    if (number != "0")
                    {
                        if (i == strLength - 1 && number == "1" && strSatang[0].ToString() != "0")
                        {
                            BahtText += "เอ็ด";
                        }
                        else if (i == strLength - 2 && number == "2" && strSatang[0].ToString() != "0")
                        {
                            BahtText += "ยี่";
                        }
                        else if (i != strLength - 2 || number != "1")
                        {
                            BahtText += strThaiNumber[int.Parse(number)];
                        }

                        BahtText += strThaiPos[(strLength - i) - 1];
                    }
                }

                BahtText += "สตางค์";
            }

            return BahtText;
        }



         
   
    }
}