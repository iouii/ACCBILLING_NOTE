using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using ACCBILLING_NOTE.Models;
using ACCBILLING_NOTE.Context;
using System.Globalization;

namespace ACCBILLING_NOTE.Controllers
{
    public class ResultController : Controller
    {
        string textShow;
        string cusCode,sum1,sum2,sum3,dateCu,bankI;
        // GET: Result
        ConnectdataBase con = new ConnectdataBase();
        OCTIIS_WEBAPPEntities db = new OCTIIS_WEBAPPEntities();
        Thomas_OguraEntities db2 = new Thomas_OguraEntities();


        public ActionResult Index()
        {

            ViewBag.cusname = cus();



            var ae = DateTime.Now.Date.ToString("dd/MM/yyyy").ToString();

            ViewBag.datew = ae;
            return View();
        }

        public Array cus()
        {
            string sqlq;
            Array cusname;


            con.OpensThomas();
            sqlq = "SELECT  CustomerCode AS IDCUST,InvoiceName AS NAMECUST FROM MasterCustomer MC " +
            "INNER JOIN MasterInvoice MI ON MC.MasterInvoiceId = MI.Id ";

            DataTable dbq = new DataTable();
            SqlDataAdapter daq = new SqlDataAdapter(sqlq, con.consThos);

            daq.Fill(dbq);

            con.ClosesThomas();

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
        [HttpPost]
        public ActionResult SearchData(string jsonData)
        {

            string sql = "";
            JObject jsonObj = new JObject();
            jsonObj = JObject.Parse(jsonData);
            //var Customer = "THAI MICRO CO.,LTD.";
            // jsonObj
            // "customerName": customerName, "dateStart": dateStart, "dateEnd": dateEnd

            sql = "SELECT " +
                   "AD.Inv_headerDate," +
                   "AD.Inv_duedate," +
                   "AD.OnInvoiceHeader," +
                   "AD.CodeCustomer," +
                   "AD.CustomerName," +
                   "SUM(CONVERT(float,AD.Amount)) AS Amount," +
                   "SUM(CONVERT(float,AD.Vat)) AS Vat," +
                   "SUM(CONVERT(float,AD.Total)) AS  Total ";
            sql += "FROM Acc_BillingDetail AD " +
                   "WHERE  AD.CustomerName LIKE '" + jsonObj["customerName"].ToString().Trim() + "%' AND  CONVERT(date,AD.Inv_headerDate,105) BETWEEN CONVERT(date,'" + jsonObj["dateStart"].ToString() + "',105) AND CONVERT(date,'" + jsonObj["dateEnd"].ToString() + "',105) ";

            sql += "GROUP BY AD.Inv_headerDate,AD.OnInvoiceHeader,AD.CodeCustomer,AD.CustomerName,AD.Inv_duedate ";

            con.Opens();
            SqlCommand cmd = new SqlCommand(sql, con.cons);
            SqlDataReader reader = cmd.ExecuteReader();

            List<ModelResult> modelResult = new List<ModelResult>();
            while (reader.Read())
            {
               
                modelResult.Add(new ModelResult()
                {
                    date = reader["Inv_headerDate"].ToString(),
                    duedate = reader["Inv_duedate"].ToString(),
                    Oninv = reader["OnInvoiceHeader"].ToString(),
                    Codecus = reader["CodeCustomer"].ToString(),
                    CusName = reader["CustomerName"].ToString(),
                    Amount = reader["Amount"].ToString(),
                    Vat = reader["Vat"].ToString(),
                    Total = reader["Total"].ToString()
                });

            }
            con.Closes();

            return Json(modelResult);
        }

        public ActionResult DeleteData(string InvoiceH)
        {
            string sql = "";
            con.Opens();
            sql = "DELETE FROM Acc_BillingDetail WHERE OnInvoiceHeader ='" + InvoiceH + "'";
            SqlCommand cmd = new SqlCommand(sql, con.cons);
            cmd.ExecuteNonQuery();
            con.Closes();

            return Json(String.Format("Delete data Success"));
        }



        public ActionResult Reprint(string InvoiceH)
        {
           

            var sql = db.Acc_BillingDetail.Where(abd => abd.OnInvoiceHeader == InvoiceH).Select(cae => cae.OnInvoiceDetail).FirstOrDefault();
            var af = sql.Substring(0, 5);
            if(af == "OCTDO"){

                textShow = "ReDomestic";
                TempData["data"] = Rese(InvoiceH);
               
            }else if(af  == "OCTEX"){

                TempData["data"] = Rese(InvoiceH);
            
                textShow = "ReOversea";

            }

            return Json(String.Format(textShow));
        }

        public Array Rese(string InvoiceH)
        {
           
            Array ag,ada,ag2,ag3;
            List<Invoice> inVoice = new List<Invoice>();
            List<InvoiceCus> inVoiceCus = new List<InvoiceCus>();

            var sql = db.Acc_BillingDetail.Join(db.Acc_Bank, ab => ab.AccBankId, abd => abd.Id.ToString(), (ab, abd) => new { ab, abd })
                .Where(abd => abd.ab.OnInvoiceHeader == InvoiceH).ToList();


            foreach (var que in sql)
            {
                cusCode = que.ab.Codecustomer;
                dateCu = que.ab.Inv_headerDate;
                bankI = que.ab.AccBankId;
                inVoice.Add(new Invoice
                {
                    invoiceNo = que.ab.OnInvoiceDetail,
                    invoiceDate = que.ab.Inv_date,
                    invoiceDue = que.ab.Inv_duedate,
                    invoiceAm1Cal = Convert.ToDouble(que.ab.Amount).ToString("N2",CultureInfo.InvariantCulture),
                    invoiceVatCal = Convert.ToDouble(que.ab.Vat).ToString("N2", CultureInfo.InvariantCulture),
                    invoiceAm2Cal = Convert.ToDouble(que.ab.Total).ToString("N2", CultureInfo.InvariantCulture)
                    
                });

            }
            var jsonSr  = new List<Invoice>();
            var jsonSr1 = new List<Invoice>();
            var jsonSr2 = new List<Invoice>();
            var jsonSr3 = new List<Invoice>();
            var jsonSr4 = new List<Invoice>();
            var bnk = new List<bankTranfer>();
            for (var i = 0; i < inVoice.Count(); i++)
            {

                if (i < 15)
                {

                    jsonSr.Add(new Invoice
                    {
                        invoiceNo     = inVoice[i].invoiceNo,
                        invoiceDate   = inVoice[i].invoiceDate,
                        invoiceDue    = inVoice[i].invoiceDue,
                        invoiceAm1Cal = inVoice[i].invoiceAm1Cal,
                        invoiceVatCal = inVoice[i].invoiceVatCal,
                        invoiceAm2Cal = inVoice[i].invoiceAm2Cal
                    

                    });


                }
                else if (i > 14 && i < 30)
                {


                    jsonSr1.Add(new Invoice
                    {
                        invoiceNo     = inVoice[i].invoiceNo,
                        invoiceDate   = inVoice[i].invoiceDate,
                        invoiceDue    = inVoice[i].invoiceDue,
                        invoiceAm1Cal = inVoice[i].invoiceAm1Cal,
                        invoiceVatCal = inVoice[i].invoiceVatCal,
                        invoiceAm2Cal = inVoice[i].invoiceAm2Cal
                    
                    });

                }
            }



            sum1 = inVoice.Sum(c => Convert.ToDouble(c.invoiceAm1Cal)).ToString("N2", CultureInfo.InvariantCulture);
            sum2 = inVoice.Sum(c => Convert.ToDouble(c.invoiceVatCal)).ToString("N2", CultureInfo.InvariantCulture);
            sum3 = inVoice.Sum(c => Convert.ToDouble(c.invoiceAm2Cal)).ToString("N2", CultureInfo.InvariantCulture);

            var cusQue = db2.MasterCustomers.Join(db2.MasterInvoices, mc => mc.MasterInvoiceId, mi => mi.Id, (mc, mi) => new { mc, mi })
                .Where(mc => mc.mc.CustomerCode == cusCode).FirstOrDefault();

           

                inVoiceCus.Add(new InvoiceCus
                {
                    idCust = cusQue.mc.CustomerCode,
                    custName = cusQue.mi.InvoiceName,
                    custAdress = cusQue.mi.Address1 + "   " + cusQue.mi.Address2 + "   " + cusQue.mi.PostalCode,
                    custAdressi = cusQue.mi.Attn,
                    custPhone = cusQue.mi.TelephoneNo1,
                    custFax = cusQue.mi.FaxNo,
                    custTex =cusQue.mi.TaxId,
                    custAdressii  = InvoiceH



                });


                var bank = db.Acc_Bank.Where(ba => ba.Id.ToString() == bankI).FirstOrDefault();

                bnk.Add(new bankTranfer
                {

                    bId = bank.Id.ToString(),
                    bNameEn = bank.BankName_EN,
                    bAddressEn = bank.Address_EN,
                    bAccNo = bank.AccOn,
                    bBranch = bank.Branch,
                    bSwiftCode = bank.SwiftCode

                });


                    ag  = jsonSr.ToArray();
                    ada = inVoiceCus.ToArray();
                    ag2 = jsonSr1.ToArray();
                    ag3 = bnk.ToArray();
                    var ade = sum3.ToString();

                    TempData["sumcal"] = ThaiBahtText(ade) ;
                    TempData["date"]   = dateCu;
                    TempData["rt"]     = ada;
                    TempData["sum1"]   = sum1;
                    TempData["sum2"]   = sum2;
                    TempData["sum3"]   = sum3;
                    TempData["count"]  = inVoice.Count();
                    TempData["counts"] = jsonSr.Count();
                    TempData["countt"] = jsonSr1.Count();
                    TempData["data2"]  = ag2;
                    TempData["bank"]   = ag3;
                    //  TempData["sum2"] = ada;

                    ViewBag.result = ag;


                    return ag;
           
        }

        public ActionResult ReDomestic()
        {
            ViewBag.dater      = TempData["date"];
            ViewBag.cus        = TempData["rt"];
            ViewBag.sum1       = TempData["sum1"];
            ViewBag.sum2       = TempData["sum2"];
            ViewBag.sum3       = TempData["sum3"];
            ViewBag.TextCal    = TempData["sumcal"];
            ViewBag.count      = TempData["count"];
            ViewBag.countt     = TempData["countt"];
            ViewBag.counts     = TempData["counts"];
            ViewBag.data2      = TempData["data2"];
            ViewBag.bank       = TempData["bank"];
            ViewBag.resulii    = TempData["data"];
            return View();
        }


        public ActionResult ReOversea()
        {
            ViewBag.count      = TempData["count"];
            ViewBag.countt     = TempData["countt"];
            ViewBag.dater      = TempData["date"];
            ViewBag.cus        = TempData["rt"];
            ViewBag.sum1       = TempData["sum1"];
            ViewBag.sum2       = TempData["sum2"];
            ViewBag.sum3       = TempData["sum3"];
            ViewBag.resulii    = TempData["data"];
            ViewBag.TextCal    = TempData["sumcal"];
            ViewBag.data2      = TempData["data2"];
            ViewBag.counts     = TempData["counts"];
            ViewBag.bank       = TempData["bank"];
            return View();
        }


        public string queryReprint(string reInvoice)
        {
            con.OpensThomas();
            string asd = "211";


            return asd;
        }


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



        public Array getExortExcel(){


            var dqer = db.Acc_BillingDetail.ToList();


            List<Invoice> liose = new List<Invoice>();

            foreach(var adrty in dqer){

                
                liose.Add(new Invoice { 
            
            
                invoiceId = adrty.AccBankId,

            
            
            
                });

            }
            



            return getExortExcel();
        }
    }
}