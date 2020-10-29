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
using ACCBILLING_NOTE.Context;


namespace ACCBILLING_NOTE.Controllers
{
    public class HomeController : Controller
    {
       

        ConnectdataBase con = new ConnectdataBase();

        OCTIIS_WEBAPPEntities sql = new OCTIIS_WEBAPPEntities();

        public ActionResult Index()
        {

            ViewBag.cus = TempData["Cus"];
           // ViewBag.data = TempData["invoi"];
            ViewBag.datacount = TempData["count"];
            ViewBag.calAm1 = TempData["calAm1"];
            ViewBag.calAm2 = TempData["calAm2"];
            ViewBag.calV = TempData["calV"];
            ViewBag.TextCal = TempData["textCal"];
            ViewBag.data    =  TempData["data"]   ;
            ViewBag.data1    =  TempData["data1"]  ;
            ViewBag.data2    =  TempData["data2"]  ;
            ViewBag.data3    = TempData["data3"]  ;
            ViewBag.data4    = TempData["data4"]  ;
            ViewBag.calco = TempData["calcount"];


            ViewBag.countDt = TempData["dataCu"];
            ViewBag.countDt1 = TempData["dataCu1"];
            ViewBag.countDt2 = TempData["dataCu2"];
            ViewBag.countDt3 = TempData["dataCu13"];

            ViewBag.bankTr = bankQuery();

            return View();
        }

   
        public ActionResult Export()
        {

            ViewBag.cus = TempData["Cus"];
            // ViewBag.data = TempData["invoi"];
            ViewBag.datacount = TempData["count"];
            ViewBag.calAm1 = TempData["calAm1"];
            ViewBag.calAm2 = TempData["calAm2"];
            ViewBag.calV = TempData["calV"];
            ViewBag.TextCal = TempData["textCal"];
            ViewBag.data = TempData["data"];
            ViewBag.data1 = TempData["data1"];
            ViewBag.data2 = TempData["data2"];
            ViewBag.data3 = TempData["data3"];
            ViewBag.data4 = TempData["data4"];
            ViewBag.calco = TempData["calcount"];


            ViewBag.countDt = TempData["dataCu"];
            ViewBag.countDt1 = TempData["dataCu1"];
            ViewBag.countDt2 = TempData["dataCu2"];
            ViewBag.countDt3 = TempData["dataCu13"];

            ViewBag.bankTr = bankQuery();

            return View();
        }
        public ActionResult GetOver()
        {
            return View();
        }

        public ActionResult Contact()
        {



            ViewBag.getIn = ParController.IntE();

            return View();
        }

       [HttpPost]
        public ActionResult GetOver(string name,string month,string year,string invoice)
        {

            string sqlPirce = "", sqlHeader;
            Array asas, cuss,data,data1,data2,data3,data4;
          

            sqlPirce = "WITH INVOICE AS ( " +
                  "SELECT  CNTBTCH,IDCUST,IDINVC,FISCYR,FISCPER," +
                  "TERMCODE,BASETAX1,AMTTAX1,AMTPYMSCHD,EXCHRATEHC,CODECURN," +
                  "SHPTOSTE2,SHPTOSTE3,DATEINVC,DATEDUE,SHPTOPHON,SHPTOFAX FROM ARIBH WHERE DATEINVC BETWEEN  '" + Convert.ToInt32(month) + "' AND  '" + Convert.ToInt32(year) + "' " +
            ") " +
            "SELECT DISTINCT " +
            "INV.IDINVC,INV.DATEINVC,INV.DATEDUE,INV.BASETAX1,INV.AMTTAX1,INV.AMTPYMSCHD " +
            "FROM INVOICE INV INNER JOIN ARSTCUS AST " +
            "ON INV.IDCUST = AST.IDCUST  WHERE INV.IDINVC IN (" + invoice + ")";

            sqlHeader = "WITH INVOICE AS ( " +
            "SELECT CNTBTCH,IDCUST,IDINVC,FISCYR,FISCPER, " +
            "TERMCODE,BASETAX1,AMTTAX1,AMTPYMSCHD,EXCHRATEHC,CODECURN, " +
            "SHPTOSTE2,SHPTOSTE3,DATEINVC,DATEDUE,SHPTOPHON,SHPTOFAX,SHPTOSTTE,SHPTOPOST,SHPTOCTRY  " +
            "FROM ARIBH WHERE DATEINVC BETWEEN  '" + Convert.ToInt32(month) + "' AND  '" + Convert.ToInt32(year) + "' " +
            ") " +
            "SELECT TOP 1 INV.IDCUST,ART.NAMECUST,ART.IDTAXREGI1," +
            "REPLACE(INV.SHPTOSTE2,' ',' ')+SPACE(2)+ " +
            "REPLACE(INV.SHPTOSTE3,' ',' ')+SPACE(2)+ " +
            "REPLACE(INV.SHPTOSTTE,' ',' ')+SPACE(2)+ " +
            "REPLACE(INV.SHPTOPOST,' ',' ')+SPACE(2)+ " +
            "REPLACE(INV.SHPTOCTRY,' ',' ') AS ADRRESS," +
            "INV.TERMCODE,INV.SHPTOPHON,INV.SHPTOFAX , ART.NAMECTAC " +
            "FROM INVOICE INV INNER JOIN ARSTCUS ART " +
            "ON INV.IDCUST = ART.IDCUST WHERE  ART.IDCUST = '" + name + "'ORDER BY ART.NAMECUST DESC";






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

                    invoiceNo       = dbp.Rows[i]["IDINVC"].ToString(),
                    invoiceDate         = day + "/" + monthT + "/" + yearT,
                    invoiceDue      = dayD + "/" + monthD + "/" + yearD,
                    invoiceAm1      = Convert.ToDouble(dbp.Rows[i]["BASETAX1"]),
                    invoiceVat      = Convert.ToDouble(dbp.Rows[i]["AMTTAX1"]),
                    invoiceAm2      = Convert.ToDouble(dbp.Rows[i]["AMTPYMSCHD"]),
                    invoiceAm1Cal       = Convert.ToDouble(dbp.Rows[i]["BASETAX1"]).ToString("N2", CultureInfo.InvariantCulture),
                    invoiceVatCal       = Convert.ToDouble(dbp.Rows[i]["AMTTAX1"]).ToString("N2", CultureInfo.InvariantCulture),
                    invoiceAm2Cal       = Convert.ToDouble(dbp.Rows[i]["AMTPYMSCHD"]).ToString("N2", CultureInfo.InvariantCulture),


                });




            }


            var jsonAd = new List<InvoiceCus>();
            var rowCus = dbh.Rows.Count;
            byte textD;
            string adi;

          
           // string Add;
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


                 adi =  dbh.Rows[i]["ADRRESS"].ToString();


               //  Add = dbh.Rows[i]["ADRRESS"].ToString().Split(", ");
             
                
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
                    custAdressi = dbh.Rows[i]["NAMECTAC"].ToString()



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


            var jsonSr = new List<Invoice>();
            var jsonSr1 = new List<Invoice>();
            var jsonSr2 = new List<Invoice>();
            var jsonSr3 = new List<Invoice>();
            var jsonSr4 = new List<Invoice>();

            for (var i = 0; i < json.Count();i++ )
            {

                if(i < 15){

                    jsonSr.Add(new Invoice { 
                    
                   invoiceNo     = json[i].invoiceNo,
                   invoiceDate   = json[i].invoiceDate,
                   invoiceDue    = json[i].invoiceDue,
                   invoiceAm1    = json[i].invoiceAm1, 
                   invoiceVat    = json[i].invoiceVat, 
                   invoiceAm2    = json[i].invoiceAm2,
                   invoiceAm1Cal = json[i].invoiceAm1Cal,
                   invoiceVatCal = json[i].invoiceVatCal,
                   invoiceAm2Cal = json[i].invoiceAm2Cal

                    });
                   

                }else if(i > 14 && i < 30  ){


                    jsonSr1.Add(new Invoice
                    {

                        invoiceNo = json[i].invoiceNo,
                        invoiceDate = json[i].invoiceDate,
                        invoiceDue = json[i].invoiceDue,
                        invoiceAm1 = json[i].invoiceAm1,
                        invoiceVat = json[i].invoiceVat,
                        invoiceAm2 = json[i].invoiceAm2,
                        invoiceAm1Cal = json[i].invoiceAm1Cal,
                        invoiceVatCal = json[i].invoiceVatCal,
                        invoiceAm2Cal = json[i].invoiceAm2Cal

                    });
                  
                }
                else if (i > 29 && i < 45)
                {


                    jsonSr2.Add(new Invoice
                    {

                        invoiceNo = json[i].invoiceNo,
                        invoiceDate = json[i].invoiceDate,
                        invoiceDue = json[i].invoiceDue,
                        invoiceAm1 = json[i].invoiceAm1,
                        invoiceVat = json[i].invoiceVat,
                        invoiceAm2 = json[i].invoiceAm2,
                        invoiceAm1Cal = json[i].invoiceAm1Cal,
                        invoiceVatCal = json[i].invoiceVatCal,
                        invoiceAm2Cal = json[i].invoiceAm2Cal


                    });

                }
                else if (i > 44 && i < 60)
                {


                    jsonSr3.Add(new Invoice
                    {

                        invoiceNo = json[i].invoiceNo,
                        invoiceDate = json[i].invoiceDate,
                        invoiceDue = json[i].invoiceDue,
                        invoiceAm1 = json[i].invoiceAm1,
                        invoiceVat = json[i].invoiceVat,
                        invoiceAm2 = json[i].invoiceAm2,
                        invoiceAm1Cal = json[i].invoiceAm1Cal,
                        invoiceVatCal = json[i].invoiceVatCal,
                        invoiceAm2Cal = json[i].invoiceAm2Cal

                    });

                }
                else if (i > 60 && i < 75)
                {


                    jsonSr4.Add(new Invoice
                    {

                        invoiceNo = json[i].invoiceNo,
                        invoiceDate = json[i].invoiceDate,
                        invoiceDue = json[i].invoiceDue,
                        invoiceAm1 = json[i].invoiceAm1,
                        invoiceVat = json[i].invoiceVat,
                        invoiceAm2 = json[i].invoiceAm2,
                        invoiceAm1Cal = json[i].invoiceAm1Cal,
                        invoiceVatCal = json[i].invoiceVatCal,
                        invoiceAm2Cal = json[i].invoiceAm2Cal






                    });

                }



            }


           data = jsonSr.ToArray();
           data1 = jsonSr1.ToArray();
           data2 = jsonSr2.ToArray();
           data3 = jsonSr3.ToArray();
           data4 = jsonSr4.ToArray();



         //  List<Invoice> ArrayData = data.Tolist()


           TempData["data"] = data;
           TempData["data1"] = data1;
           TempData["data2"] = data2;
           TempData["data3"] = data3;
           TempData["data4"] = data4;

           TempData["dataCu"] = jsonSr.Count();
           TempData["dataCu1"] = jsonSr1.Count();
           TempData["dataCu2"] = jsonSr2.Count();
           TempData["dataCu3"] = jsonSr3.Count();

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


           var adde = (json.Count / 15);

           TempData["calcount"] = adde;

            return View("Index");

        }


        public ActionResult Test()
        {

            ViewBag.cusname = cus();

           

            var ae = DateTime.Now.Date.ToString("dd/MM/yyyy").ToString();

            ViewBag.datew = ae;

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
            Array asas, cuss, data, data1, data2, data3, data4;

            sqlPirce = "WITH INVOICE AS ( " +
                  "SELECT  CNTBTCH,IDCUST,IDINVC,FISCYR,FISCPER," +
                  "TERMCODE,BASETAX1,AMTTAX1,AMTPYMSCHD,EXCHRATEHC,CODECURN," +
                  "SHPTOSTE2,SHPTOSTE3,DATEINVC,DATEDUE,SHPTOPHON,SHPTOFAX FROM ARIBH WHERE DATEINVC BETWEEN   '" + Convert.ToInt32(month) + "' AND  '" + Convert.ToInt32(year) + "' " +
            ") " +
            "SELECT DISTINCT " +
            "INV.IDINVC,INV.DATEINVC,INV.DATEDUE,INV.BASETAX1,INV.AMTTAX1,INV.AMTPYMSCHD " +
            "FROM INVOICE INV INNER JOIN ARSTCUS AST " +
            "ON INV.IDCUST = AST.IDCUST  WHERE INV.IDINVC IN (" + invoice + ")";

            sqlHeader = "WITH INVOICE AS ( " +
            "SELECT CNTBTCH,IDCUST,IDINVC,FISCYR,FISCPER, " +
            "TERMCODE,BASETAX1,AMTTAX1,AMTPYMSCHD,EXCHRATEHC,CODECURN, " +
            "SHPTOSTE2,SHPTOSTE3,DATEINVC,DATEDUE,SHPTOPHON,SHPTOFAX,SHPTOSTTE,SHPTOPOST,SHPTOCTRY  " +
            "FROM ARIBH WHERE DATEINVC BETWEEN   '" + Convert.ToInt32(month) + "' AND  '" + Convert.ToInt32(year) + "' " +
            ") " +
            "SELECT TOP 1 INV.IDCUST,ART.NAMECUST,ART.IDTAXREGI1," +
            "REPLACE(INV.SHPTOSTE2,' ',' ')+SPACE(2)+ " +
            "REPLACE(INV.SHPTOSTE3,' ',' ')+SPACE(2)+ " +
            "REPLACE(INV.SHPTOSTTE,' ',' ')+SPACE(2)+ " +
            "REPLACE(INV.SHPTOPOST,' ',' ')+SPACE(2)+ " +
            "REPLACE(INV.SHPTOCTRY,' ',' ') AS ADRRESS," +
            "INV.TERMCODE,INV.SHPTOPHON,INV.SHPTOFAX , ART.NAMECTAC " +
            "FROM INVOICE INV INNER JOIN ARSTCUS ART " +
            "ON INV.IDCUST = ART.IDCUST WHERE   ART.IDCUST = '" + name + "'  ORDER BY ART.NAMECUST DESC";






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
                else if (text == " ")
                {
                    textD = 0;
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
                    custAdressi = dbh.Rows[i]["NAMECTAC"].ToString()


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



            var jsonSr = new List<Invoice>();
            var jsonSr1 = new List<Invoice>();
            var jsonSr2 = new List<Invoice>();
            var jsonSr3 = new List<Invoice>();
            var jsonSr4 = new List<Invoice>();

            for (var i = 0; i < json.Count(); i++)
            {

                if (i < 15)
                {

                    jsonSr.Add(new Invoice
                    {

                        invoiceNo = json[i].invoiceNo,
                        invoiceDate = json[i].invoiceDate,
                        invoiceDue = json[i].invoiceDue,
                        invoiceAm1 = json[i].invoiceAm1,
                        invoiceVat = json[i].invoiceVat,
                        invoiceAm2 = json[i].invoiceAm2,
                        invoiceAm1Cal = json[i].invoiceAm1Cal,
                        invoiceVatCal = json[i].invoiceVatCal,
                        invoiceAm2Cal = json[i].invoiceAm2Cal

                    });


                }
                else if (i > 14 && i < 30)
                {


                    jsonSr1.Add(new Invoice
                    {

                        invoiceNo = json[i].invoiceNo,
                        invoiceDate = json[i].invoiceDate,
                        invoiceDue = json[i].invoiceDue,
                        invoiceAm1 = json[i].invoiceAm1,
                        invoiceVat = json[i].invoiceVat,
                        invoiceAm2 = json[i].invoiceAm2,
                        invoiceAm1Cal = json[i].invoiceAm1Cal,
                        invoiceVatCal = json[i].invoiceVatCal,
                        invoiceAm2Cal = json[i].invoiceAm2Cal

                    });

                }
                else if (i > 29 && i < 45)
                {


                    jsonSr2.Add(new Invoice
                    {

                        invoiceNo = json[i].invoiceNo,
                        invoiceDate = json[i].invoiceDate,
                        invoiceDue = json[i].invoiceDue,
                        invoiceAm1 = json[i].invoiceAm1,
                        invoiceVat = json[i].invoiceVat,
                        invoiceAm2 = json[i].invoiceAm2,
                        invoiceAm1Cal = json[i].invoiceAm1Cal,
                        invoiceVatCal = json[i].invoiceVatCal,
                        invoiceAm2Cal = json[i].invoiceAm2Cal


                    });

                }
                else if (i > 44 && i < 60)
                {


                    jsonSr3.Add(new Invoice
                    {

                        invoiceNo = json[i].invoiceNo,
                        invoiceDate = json[i].invoiceDate,
                        invoiceDue = json[i].invoiceDue,
                        invoiceAm1 = json[i].invoiceAm1,
                        invoiceVat = json[i].invoiceVat,
                        invoiceAm2 = json[i].invoiceAm2,
                        invoiceAm1Cal = json[i].invoiceAm1Cal,
                        invoiceVatCal = json[i].invoiceVatCal,
                        invoiceAm2Cal = json[i].invoiceAm2Cal

                    });

                }
                else if (i > 60 && i < 75)
                {


                    jsonSr4.Add(new Invoice
                    {

                        invoiceNo = json[i].invoiceNo,
                        invoiceDate = json[i].invoiceDate,
                        invoiceDue = json[i].invoiceDue,
                        invoiceAm1 = json[i].invoiceAm1,
                        invoiceVat = json[i].invoiceVat,
                        invoiceAm2 = json[i].invoiceAm2,
                        invoiceAm1Cal = json[i].invoiceAm1Cal,
                        invoiceVatCal = json[i].invoiceVatCal,
                        invoiceAm2Cal = json[i].invoiceAm2Cal






                    });

                }



            }


            data = jsonSr.ToArray();
            data1 = jsonSr1.ToArray();
            data2 = jsonSr2.ToArray();
            data3 = jsonSr3.ToArray();
            data4 = jsonSr4.ToArray();



            //  List<Invoice> ArrayData = data.Tolist()


            TempData["data"] = data;
            TempData["data1"] = data1;
            TempData["data2"] = data2;
            TempData["data3"] = data3;
            TempData["data4"] = data4;

            TempData["dataCu"] = jsonSr.Count();
            TempData["dataCu1"] = jsonSr1.Count();
            TempData["dataCu2"] = jsonSr2.Count();
            TempData["dataCu3"] = jsonSr3.Count();


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
            var adde = (json.Count / 15);

            TempData["calcount"] = adde;


            return View("Export");
        }


        [HttpPost]
        public ActionResult Test(string name,string month,string year)
        {
           
          var sh  = nameQ(name,month,year);
         
         return Json(sh,JsonRequestBehavior.AllowGet);
        }

 

       

        public Array cus()
        {
            string sqlq;
            Array cusname;


            con.OpenConnectionSql();
            sqlq = " SELECT DISTINCT IDCUST,NAMECUST FROM  ARSTCUS WHERE  NOT  NAMECUST = 'CALSONIC KANSEI (THAILAND) CO.,LTD.' ";

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
                    idCust = dbq.Rows[i]["IDCUST"].ToString().Trim(),
                    custName = dbq.Rows[i]["NAMECUST"].ToString(),


                });
            }

            cusname = jsonCusname.ToArray();


            return cusname;
        }


        public Array bankQuery()
        {
            List<bankTranfer> bank = new List<bankTranfer>();
            Array bankAr;

            var querySql = sql.Acc_Bank.ToList();

            foreach (var bn in querySql)
            {

                bank.Add(new bankTranfer
                {
                    bId = bn.Id.ToString(),
                    bAddressEn  =bn.Address_EN,
                    bAddressThai = bn.Address_Thi,
                    bNameEn = bn.BankName_EN,
                    bNameThai = bn.BankName_Thi,
                    bAccNo = bn.AccOn,
                    bBranch = bn.Branch,
                    bSwiftCode = bn.SwiftCode


                });
            }


            bankAr = bank.ToArray();

            return bankAr;


        }

        [HttpPost]
        public ActionResult Index(string id)
        {
            var bnf = cBankTranfer(id);

            return Json(bnf, JsonRequestBehavior.AllowGet);


          
        }


        [HttpPost]
        public ActionResult Export(string id)
        {
            var bnf = cBankTranfer(id);


            return Json(bnf, JsonRequestBehavior.AllowGet);
        }
        
        public List<bankTranfer> cBankTranfer(string id)
        {
            var json = new List<bankTranfer>();

            var bankQuery = sql.Acc_Bank.Where(c => c.Id.ToString() == id).ToList();


            foreach (var bn in bankQuery)
            {

                json.Add(new bankTranfer {

                    bId = bn.Id.ToString(),
                    bAddressEn = bn.Address_EN,
                    bAddressThai = bn.Address_Thi,
                    bNameEn = bn.BankName_EN,
                    bNameThai = bn.BankName_Thi,
                    bAccNo = bn.AccOn,
                    bBranch = bn.Branch,
                    bSwiftCode = bn.SwiftCode
                
                
                });

            }





            return json;
        }

        public List<Invoice> nameQ(string name,string month,string year)
        {

            string sqlPirce;

            sqlPirce = "WITH INVOICE AS ( " +
         "SELECT CNTBTCH,IDCUST,IDINVC,FISCYR,FISCPER," +
         "TERMCODE,BASETAX1,AMTTAX1,AMTPYMSCHD,EXCHRATEHC,CODECURN," +
         "SHPTOSTE2,SHPTOSTE3,DATEINVC,DATEDUE,SHPTOPHON,SHPTOFAX FROM ARIBH WHERE DATEINVC BETWEEN   '" + Convert.ToInt32(month) + "' AND  '" + Convert.ToInt32(year) + "' " +
           ") " +
           "SELECT DISTINCT " +
           "INV.IDINVC,INV.DATEINVC,INV.DATEDUE,INV.BASETAX1,INV.AMTTAX1,INV.AMTPYMSCHD " +
           "FROM INVOICE INV INNER JOIN ARSTCUS AST " +
           "ON INV.IDCUST = AST.IDCUST  WHERE AST.IDCUST LIKE  '" + name + "%' ";


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