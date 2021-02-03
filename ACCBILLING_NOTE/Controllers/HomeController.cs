using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;

using Newtonsoft.Json;
using System.Globalization;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Web.Helpers;
using BOI_QUO.Context;
using System.Text;
using BOI_QUO.Models;
using System.IO;



namespace BOI_QUO.Controllers
{
    public class HomeController : Controller
    {
        string months;

        ConnectdataBase con = new ConnectdataBase();

        OCTIIS_WEBAPPEntities1 dbObj = new OCTIIS_WEBAPPEntities1();
        Thomas_OguraEntities1 dbObjThomas = new Thomas_OguraEntities1();
       

        public ActionResult Index()
        {

           // ItemNoGet();
           // QuotationPurchaseView();
            return View();
        }



        public void SupplierGet()
        {
            string sqlq;
            Array SupplierArray;
            var SupllierM = new List<Supllier>();

            con.OpensThomas();

            sqlq = "SELECT BusinessPartnerCode , BusinessPartnerName FROM MasterBusinessPartner WHERE BusinessPartnerCode LIKE 'V%' ";

            DataTable dbq1 = new DataTable();
            SqlDataAdapter daq = new SqlDataAdapter(sqlq, con.consThos);
            daq.Fill(dbq1);
            var rowName = dbq1.Rows.Count;
            for (int i = 0; i < rowName; i++)
            {
                SupllierM.Add(new Supllier()
                {
                    BusinessPartnerCodeM = Convert.ToBase64String(Encoding.UTF8.GetBytes(Convert.ToBase64String(Encoding.UTF8.GetBytes(dbq1.Rows[i]["BusinessPartnerCode"].ToString())))),
                    BusinessPartnerNameM = dbq1.Rows[i]["BusinessPartnerName"].ToString()


                });
            }


            SupplierArray = SupllierM.ToArray();
            ViewBag.Suppllier = SupplierArray;
        }


        public void CustomerGet()
        {
            string sqlq;
            Array CustomerArray;
            var CustomerM = new List<Supllier>();

            con.OpensThomas();

            sqlq = "SELECT BusinessPartnerCode , BusinessPartnerName FROM MasterBusinessPartner WHERE BusinessPartnerCode LIKE 'C%' ";

            DataTable dbq1 = new DataTable();
            SqlDataAdapter daq = new SqlDataAdapter(sqlq, con.consThos);
            daq.Fill(dbq1);
            var rowName = dbq1.Rows.Count;
            for (int i = 0; i < rowName; i++)
            {
                CustomerM.Add(new Supllier()
                {
                    BusinessPartnerCodeM =Convert.ToBase64String(Encoding.UTF8.GetBytes(Convert.ToBase64String(Encoding.UTF8.GetBytes(dbq1.Rows[i]["BusinessPartnerCode"].ToString())))),
                    BusinessPartnerNameM = dbq1.Rows[i]["BusinessPartnerName"].ToString()


                });
            }


            CustomerArray = CustomerM.ToArray();
            ViewBag.Customer = CustomerArray;
        }


        public ActionResult AddQuotation()
        {

           
            CustomerGet();
            SupplierGet();
            return View();
        }

        [HttpPost]
        public ActionResult InsertQuotations(string ArData, string QuoNo, string EffectDate, string ExpireDate, string DateReceive, string PartnerCode,string Type)
        {

         
            string[] Arrd = ArData.Split(',');
            string[] DateEF = EffectDate.Split('-');
            string[] DateEX = ExpireDate.Split('-');
            string[] DateRE = DateReceive.Split('-');
           
            if (Request.Files.Count > 0)
            {
                try
                {

                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {


                        HttpPostedFileBase file = files[i];
                        string fname,fnameup;


                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                            fnameup = file.FileName;
                        }
                        else
                        {
                            fname = file.FileName;
                            fnameup = file.FileName;
                        }


                        fname = Path.Combine(Server.MapPath("~/File/PDF/"), fname);
                       file.SaveAs(fname);

                        for (var ii = 0; ii < Arrd.Count(); ii++)
                        {

                            var sql = new Boi_Quotations()
                            {
                                itemNo = Arrd[ii].ToString(),
                                supllierCode = Encoding.UTF8.GetString(Convert.FromBase64String(Encoding.UTF8.GetString(Convert.FromBase64String(PartnerCode.ToString())))),
                                quotationNo = QuoNo.ToString(),
                                quotationFile = fnameup,
                                dateEffective = DateEF[0] + DateEF[1] + DateEF[2],
                                dateExpriation = DateEX[0] + DateEX[1] + DateEX[2],
                                dateReceive = DateRE[0] + DateRE[1] + DateRE[2],
                                quotationType = Type,
                                dateModify  = DateTime.Now.ToString(),
                                userCode = Session["userCode"].ToString()

                            };
                            dbObj.Boi_Quotations.Add(sql);

                            dbObj.SaveChanges(); 
                        }
                      

                    }

                    return Json("File Uploaded Successfully!" );
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }


        }


        public ActionResult QuotationPurchaseView()
        {
            Array QuotationArray;
            List<QuotationShow> QuotationShowModel = new List<QuotationShow>();


            var quotationM = dbObj.Boi_Quotations.ToList();
            var masterPartner = dbObjThomas.MasterBusinessPartners.ToList();
            var masterIt = dbObjThomas.MasterItems.ToList();

        
            var quotationQuery = quotationM
                .Join(masterPartner, qm => qm.supllierCode, mp => mp.BusinessPartnerCode, (qm, mp) => new { qm, mp })
                .Join(masterIt, qm => qm.qm.itemNo, mi => mi.ItemNo, (qm, mi) => new { qm, mi })
                .Where(qmw => qmw.qm.qm.quotationType == "Purchase")
                .ToList();
            foreach (var gr in quotationQuery)
            {
                string dateRe = gr.qm.qm.dateReceive;
                string dateree = dateRe[0] + "" + dateRe[1] + "" + dateRe[2] + "" + dateRe[3] + "-" + dateRe[4] + "" + dateRe[5] + "-" + dateRe[6] + "" + dateRe[7];
                string dateEf = gr.qm.qm.dateEffective;
                string dateefR = dateEf[0]+""+ dateEf[1] +""+ dateEf[2]+""+ dateEf[3] + "-" + dateEf[4]+""+ dateEf[5] + "-" + dateEf[6]+"" + dateEf[7];
                string dateEx = gr.qm.qm.dateExpriation;
                string dateexR = dateEx[0] + "" + dateEx[1] + "" + dateEx[2] + "" + dateEx[3] + "-" + dateEx[4] + "" + dateEx[5] + "-" + dateEx[6] + "" + dateEx[7];

                QuotationShowModel.Add(new QuotationShow
                {
                    Ida = gr.qm.qm.id,
                    itemNo = gr.mi.ItemNo,
                    ModelNoa = gr.mi.ItemName,
                    ItemShortNamea  =gr.mi.ItemShortName,
                    BusinessPartnerNameM = gr.qm.mp.BusinessPartnerName,
                    dateEffective = dateefR,
                    dateExpriation = dateexR,
                    quotationFile = gr.qm.qm.quotationFile,
                    quotationRemark = gr.qm.qm.quotationRemark,
                    dateModify = gr.qm.qm.dateModify,
                    quotationNo = gr.qm.qm.quotationNo,
                    dateReceive = dateree

                });
            }


            QuotationArray = QuotationShowModel.ToArray();

            ViewBag.Quotations = QuotationArray;


            return View();
        
       
        }

        public ActionResult QuotationSalesView()
        {
            Array QuotationArray;
            List<QuotationShow> QuotationShowModel = new List<QuotationShow>();


            var quotationM = dbObj.Boi_Quotations.ToList();
            var masterPartner = dbObjThomas.MasterBusinessPartners.ToList();
            var masterIt = dbObjThomas.MasterItems.ToList();


            var quotationQuery = quotationM
                .Join(masterPartner, qm => qm.supllierCode, mp => mp.BusinessPartnerCode, (qm, mp) => new { qm, mp })
                .Join(masterIt, qm => qm.qm.itemNo, mi => mi.ItemNo, (qm, mi) => new { qm, mi })
                .Where(qmw => qmw.qm.qm.quotationType == "Sales")
                .ToList();
            foreach (var gr in quotationQuery)
            {
                string dateRe = gr.qm.qm.dateReceive;
                string dateree = dateRe[0] + "" + dateRe[1] + "" + dateRe[2] + "" + dateRe[3] + "-" + dateRe[4] + "" + dateRe[5] + "-" + dateRe[6] + "" + dateRe[7];

                string dateEf = gr.qm.qm.dateEffective;
                string dateefR = dateEf[0] + "" + dateEf[1] + "" + dateEf[2] + "" + dateEf[3] + "-" + dateEf[4] + "" + dateEf[5] + "-" + dateEf[6] + "" + dateEf[7];
                string dateEx = gr.qm.qm.dateExpriation;
                string dateexR = dateEx[0] + "" + dateEx[1] + "" + dateEx[2] + "" + dateEx[3] + "-" + dateEx[4] + "" + dateEx[5] + "-" + dateEx[6] + "" + dateEx[7];
                QuotationShowModel.Add(new QuotationShow
                {
                    Ida = gr.qm.qm.id,
                    itemNo = gr.mi.ItemNo,
                    ModelNoa = gr.mi.ItemName,
                    ItemShortNamea = gr.mi.ItemShortName,
                    BusinessPartnerNameM = gr.qm.mp.BusinessPartnerName,
                    dateEffective = dateefR,//gr.qm.qm.dateEffective,
                    dateExpriation = dateexR,//gr.qm.qm.dateExpriation,
                    quotationFile = gr.qm.qm.quotationFile,
                    quotationRemark = gr.qm.qm.quotationRemark,
                    dateModify = gr.qm.qm.dateModify,
                    quotationNo = gr.qm.qm.quotationNo,
                    dateReceive = dateree


                });
            }


            QuotationArray = QuotationShowModel.ToArray();

            ViewBag.Quotations = QuotationArray;


            return View();
        }

        [HttpPost]
        public ActionResult QuotationPurchaseView(string itemNo, string modelNo, string partNo, string supllierNo, string quotationNo, string efNo,string exNo)
        {
            var query = QuotationSearch(itemNo,modelNo,partNo,supllierNo,quotationNo,efNo,exNo,"Purchase");

            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult QuotationSalesView(string itemNo, string modelNo, string partNo, string supllierNo, string quotationNo, string efNo, string exNo)
        {
            var query = QuotationSearch(itemNo, modelNo, partNo, supllierNo, quotationNo, efNo, exNo,"Sales");

            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public List<QuotationShow> QuotationSearch(string itemNo, string modelNo, string partNo, string supllierNo, string quotationNo, string efNo, string exNo, string type)
        {
           
            var QuotationShowModel = new List<QuotationShow>();

            var quotationM = dbObj.Boi_Quotations.ToList();
            var masterPartner = dbObjThomas.MasterBusinessPartners.ToList();
            var masterIt = dbObjThomas.MasterItems.ToList();
            string[] DateEF;
            string[] DateEX;
            
            if (efNo =="")
            {

            var quotationQuery = quotationM
                .Join(masterPartner, qm => qm.supllierCode, mp => mp.BusinessPartnerCode, (qm, mp) => new { qm, mp })
                .Join(masterIt, qm => qm.qm.itemNo, mi => mi.ItemNo, (qm, mi) => new { qm, mi })
                .Where(qmw => qmw.qm.qm.quotationType == type
                    && qmw.qm.qm.quotationNo.ToLower().Contains(quotationNo.ToLower())
                    && qmw.qm.qm.itemNo.ToLower().Contains(itemNo.ToLower())
                    && qmw.mi.ModelNo.ToLower().Contains(modelNo.ToLower())
                    && qmw.mi.ItemShortName.ToLower().Contains(partNo.ToLower())
                    && qmw.qm.mp.BusinessPartnerName.ToLower().Contains(supllierNo.ToLower())
                    ).ToList();


            foreach (var gr in quotationQuery)
            {
                string dateEf = gr.qm.qm.dateEffective;
                string dateefR = dateEf[0] + "" + dateEf[1] + "" + dateEf[2] + "" + dateEf[3] + "-" + dateEf[4] + "" + dateEf[5] + "-" + dateEf[6] + "" + dateEf[7];
                string dateEx = gr.qm.qm.dateExpriation;
                string dateexR = dateEx[0] + "" + dateEx[1] + "" + dateEx[2] + "" + dateEx[3] + "-" + dateEx[4] + "" + dateEx[5] + "-" + dateEx[6] + "" + dateEx[7];


                QuotationShowModel.Add(new QuotationShow
                {
                    Ida = gr.qm.qm.id,
                    itemNo = gr.mi.ItemNo,
                    ModelNoa = gr.mi.ItemName,
                    ItemShortNamea = gr.mi.ItemShortName,
                    BusinessPartnerNameM = gr.qm.mp.BusinessPartnerName,
                    dateEffective = dateefR,
                    dateExpriation = dateexR,
                    quotationFile = gr.qm.qm.quotationFile,
                    quotationRemark = gr.qm.qm.quotationRemark,
                    dateModify = gr.qm.qm.dateModify,
                    quotationNo = gr.qm.qm.quotationNo,
                    dateReceive = gr.qm.qm.dateReceive

                });
            }
            return QuotationShowModel;
                

           }else{

            if (exNo =="")
            {
                exNo = "9999-99-99";
                
                }

            DateEF = efNo.Split('-');
            DateEX = exNo.Split('-');
            var ef = DateEF[0] + DateEF[1] + DateEF[2];
            var ex = DateEX[0] + DateEX[1] + DateEX[2];
           

           var  quotationQuery = quotationM
                .Join(masterPartner, qm => qm.supllierCode, mp => mp.BusinessPartnerCode, (qm, mp) => new { qm, mp })
                .Join(masterIt, qm => qm.qm.itemNo, mi => mi.ItemNo, (qm, mi) => new { qm, mi })
                .Where(qmw => qmw.qm.qm.quotationType == type
                    && qmw.qm.qm.quotationNo.ToLower().Contains(quotationNo.ToLower())
                    && qmw.qm.qm.itemNo.ToLower().Contains(itemNo.ToLower())
                    && Convert.ToInt32(qmw.qm.qm.dateEffective) >= Convert.ToInt32(ef)
                    && Convert.ToInt32(qmw.qm.qm.dateExpriation) <= Convert.ToInt32(ex)
                    && qmw.mi.ModelNo.ToLower().Contains(modelNo.ToLower())
                    && qmw.mi.ItemShortName.ToLower().Contains(partNo.ToLower())
                    && qmw.qm.mp.BusinessPartnerName.ToLower().Contains(supllierNo.ToLower())
                    )          
                .ToList();


           foreach (var gr in quotationQuery)
           {
               string dateEf = gr.qm.qm.dateEffective;
               string dateefR = dateEf[0] + "" + dateEf[1] + "" + dateEf[2] + "" + dateEf[3] + "-" + dateEf[4] + "" + dateEf[5] + "-" + dateEf[6] + "" + dateEf[7];
               string dateEx = gr.qm.qm.dateExpriation;
               string dateexR = dateEx[0] + "" + dateEx[1] + "" + dateEx[2] + "" + dateEx[3] + "-" + dateEx[4] + "" + dateEx[5] + "-" + dateEx[6] + "" + dateEx[7];


               QuotationShowModel.Add(new QuotationShow
               {
                   itemNo = gr.mi.ItemNo,
                   ModelNoa = gr.mi.ItemName,
                   ItemShortNamea = gr.mi.ItemShortName,
                   BusinessPartnerNameM = gr.qm.mp.BusinessPartnerName,
                   dateEffective =dateefR ,
                   dateExpriation = dateexR,
                   quotationFile = gr.qm.qm.quotationFile,
                   quotationRemark = gr.qm.qm.quotationRemark,
                   dateModify = gr.qm.qm.dateModify,
                   quotationNo = gr.qm.qm.quotationNo,
                   dateReceive = gr.qm.qm.dateReceive

               });
           }
           return QuotationShowModel;

           }

            
        }
        public ActionResult QuotationPurchaseDelete()
        {
            Array QuotationArray;
            List<QuotationShow> QuotationShowModel = new List<QuotationShow>();


            var quotationM = dbObj.Boi_Quotations.ToList();
            var masterPartner = dbObjThomas.MasterBusinessPartners.ToList();
            var masterIt = dbObjThomas.MasterItems.ToList();


            var quotationQuery = quotationM
                .Join(masterPartner, qm => qm.supllierCode, mp => mp.BusinessPartnerCode, (qm, mp) => new { qm, mp })
                .Join(masterIt, qm => qm.qm.itemNo, mi => mi.ItemNo, (qm, mi) => new { qm, mi })
                .Where(qmw => qmw.qm.qm.quotationType == "Purchase")
                .ToList();
            foreach (var gr in quotationQuery)
            {
                string dateEf = gr.qm.qm.dateEffective;
                string dateefR = dateEf[0] + "" + dateEf[1] + "" + dateEf[2] + "" + dateEf[3] + "-" + dateEf[4] + "" + dateEf[5] + "-" + dateEf[6] + "" + dateEf[7];
                string dateEx = gr.qm.qm.dateExpriation;
                string dateexR = dateEx[0] + "" + dateEx[1] + "" + dateEx[2] + "" + dateEx[3] + "-" + dateEx[4] + "" + dateEx[5] + "-" + dateEx[6] + "" + dateEx[7];

                QuotationShowModel.Add(new QuotationShow
                {
                    Ida = gr.qm.qm.id,
                    itemNo = gr.mi.ItemNo,
                    ModelNoa = gr.mi.ItemName,
                    ItemShortNamea = gr.mi.ItemShortName,
                    BusinessPartnerNameM = gr.qm.mp.BusinessPartnerName,
                    dateEffective = dateefR,
                    dateExpriation = dateexR,
                    quotationFile = gr.qm.qm.quotationFile,
                    quotationRemark = gr.qm.qm.quotationRemark,
                    dateModify = gr.qm.qm.dateModify,
                    quotationNo = gr.qm.qm.quotationNo,
                    dateReceive = gr.qm.qm.dateReceive

                });
            }


            QuotationArray = QuotationShowModel.ToArray();

            ViewBag.Quotations = QuotationArray;


            return View();

        }

        public ActionResult QuotationSalesDelete()
        {
            Array QuotationArray;
            List<QuotationShow> QuotationShowModel = new List<QuotationShow>();


            var quotationM = dbObj.Boi_Quotations.ToList();
            var masterPartner = dbObjThomas.MasterBusinessPartners.ToList();
            var masterIt = dbObjThomas.MasterItems.ToList();


            var quotationQuery = quotationM
                .Join(masterPartner, qm => qm.supllierCode, mp => mp.BusinessPartnerCode, (qm, mp) => new { qm, mp })
                .Join(masterIt, qm => qm.qm.itemNo, mi => mi.ItemNo, (qm, mi) => new { qm, mi })
                .Where(qmw => qmw.qm.qm.quotationType == "Sales")
                .ToList();
            foreach (var gr in quotationQuery)
            {
                string dateEf = gr.qm.qm.dateEffective;
                string dateefR = dateEf[0] + "" + dateEf[1] + "" + dateEf[2] + "" + dateEf[3] + "-" + dateEf[4] + "" + dateEf[5] + "-" + dateEf[6] + "" + dateEf[7];
                string dateEx = gr.qm.qm.dateExpriation;
                string dateexR = dateEx[0] + "" + dateEx[1] + "" + dateEx[2] + "" + dateEx[3] + "-" + dateEx[4] + "" + dateEx[5] + "-" + dateEx[6] + "" + dateEx[7];

                QuotationShowModel.Add(new QuotationShow
                {
                    Ida = gr.qm.qm.id,
                    itemNo = gr.mi.ItemNo,
                    ModelNoa = gr.mi.ItemName,
                    ItemShortNamea = gr.mi.ItemShortName,
                    BusinessPartnerNameM = gr.qm.mp.BusinessPartnerName,
                    dateEffective = dateefR,
                    dateExpriation = dateexR,
                    quotationFile = gr.qm.qm.quotationFile,
                    quotationRemark = gr.qm.qm.quotationRemark,
                    dateModify = gr.qm.qm.dateModify,
                    quotationNo = gr.qm.qm.quotationNo,
                    dateReceive = gr.qm.qm.dateReceive

                });
            }


            QuotationArray = QuotationShowModel.ToArray();

            ViewBag.Quotations = QuotationArray;


            return View();

        }
        [HttpPost]
        public ActionResult QuotationPurchaseDeletez(string id)
        {
            QuotationDelete(id);

            return View();
        }

        [HttpPost]
        public ActionResult QuotationSalesDeletez(string id)
        {

            QuotationDelete(id);

            return View();
        }


        public void QuotationDelete(string id)
        {

            var sql = dbObj.Boi_Quotations.Single(c => c.id.ToString() == id);

            dbObj.Boi_Quotations.Remove(sql);
            dbObj.SaveChanges();
        }


        public ActionResult SupplierPerformance()
        {
          var type = "Supllier" ;
          PartnerPerformance(type);
          return View();
        }
    


        public ActionResult CustomerEvaluation()
        {
            var type = "Customer";
            PartnerPerformance(type);
            return View();
        }

        [HttpPost]
        public ActionResult SupplierPerformance(string YearVal, string supllierNo, string permission)
        {
            var type = "Supllier";
            var query = PartnerPerformanceSearch(type, YearVal, supllierNo, permission);
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CustomerEvaluation(string YearVal, string supllierNo, string permission)
        {
            var type = "Customer";
            var query = PartnerPerformanceSearch(type, YearVal, supllierNo, permission);
          return Json(query, JsonRequestBehavior.AllowGet);
        }

        public List<Performance> PartnerPerformanceSearch(string type, string YearVal, string supllierNo, string permission)
        {
          
            List<Performance> Perfomance = new List<Performance>();
            var bpart = dbObj.BusinessPartner_Performance.ToList();
            var masterPartner = dbObjThomas.MasterBusinessPartners.ToList();

            List<string> listToFill = new List<string>();

            string[] ct = permission.Split(',');
            for (var i = 0; i < ct.Count(); i++)
            {
                listToFill.Add(ct[i].ToString());

            }

            if (permission == "")
            {

                var sql = bpart
                    .Join(masterPartner, per => per.partnerId, tb => tb.BusinessPartnerCode, (per, tb) => new { per, tb })
                    .Where(c => c.per.remark == type
                     && c.per.year.Contains(YearVal)
                        //&& listToFill.Contains() c.per.month.Contains(permission)
                     //&& listToFill.Contains(c.per.month.ToString())
                     && c.tb.BusinessPartnerName.ToLower().Contains(supllierNo)

                    )
                    .ToList();
                foreach (var data in sql)
                {


                    int montr = Convert.ToInt32(data.per.month);
                    switch (montr)
                    {

                        case 1:
                            months = "January";
                            break;
                        case 2:
                            months = "February";
                            break;
                        case 3:
                            months = "March";
                            break;
                        case 4:
                            months = "April";
                            break;
                        case 5:
                            months = "May";
                            break;
                        case 6:
                            months = "June";
                            break;
                        case 7:
                            months = "July";
                            break;
                        case 8:
                            months = "August";
                            break;
                        case 9:
                            months = "September";
                            break;
                        case 10:
                            months = "October";
                            break;
                        case 11:
                            months = "November";
                            break;
                        case 12:
                            months = "December";
                            break;
                    }

                    Perfomance.Add(new Performance
                    {

                        id = data.per.id,
                        partnerCode = data.per.partnerId,
                        year = data.per.year,
                        month = months,
                        partnerName = data.tb.BusinessPartnerName,
                        dateCreate = data.per.dateCreate,
                        dateUpdate = data.per.dateUpdate,
                        remark = data.per.remark,
                        remark1 = data.per.remark1,
                        userCode = data.per.userCode,
                        usercodeUpdate = data.per.userCodeUpdate,
                        version = data.per.version


                    });
                }
            }
            else
            {


                var sql = bpart
                    .Join(masterPartner, per => per.partnerId, tb => tb.BusinessPartnerCode, (per, tb) => new { per, tb })
                    .Where(c => c.per.remark == type
                     && c.per.year.Contains(YearVal)
                        //&& listToFill.Contains() c.per.month.Contains(permission)
                     && listToFill.Contains(c.per.month.ToString())
                     && c.tb.BusinessPartnerName.ToLower().Contains(supllierNo)

                    )
                    .ToList();
                foreach (var data in sql)
                {


                    int montr = Convert.ToInt32(data.per.month);
                    switch (montr)
                    {

                        case 1:
                            months = "January";
                            break;
                        case 2:
                            months = "February";
                            break;
                        case 3:
                            months = "March";
                            break;
                        case 4:
                            months = "April";
                            break;
                        case 5:
                            months = "May";
                            break;
                        case 6:
                            months = "June";
                            break;
                        case 7:
                            months = "July";
                            break;
                        case 8:
                            months = "August";
                            break;
                        case 9:
                            months = "September";
                            break;
                        case 10:
                            months = "October";
                            break;
                        case 11:
                            months = "November";
                            break;
                        case 12:
                            months = "December";
                            break;
                    }

                    Perfomance.Add(new Performance
                    {

                        id = data.per.id,
                        partnerCode = data.per.partnerId,
                        year = data.per.year,
                        month = months,
                        partnerName = data.tb.BusinessPartnerName,
                        dateCreate = data.per.dateCreate,
                        dateUpdate = data.per.dateUpdate,
                        remark = data.per.remark,
                        remark1 = data.per.remark1,
                        userCode = data.per.userCode,
                        usercodeUpdate = data.per.userCodeUpdate,
                        version = data.per.version


                    });
                }
            }


            return Perfomance;


        }
        public void PartnerPerformance(string type)
        {
            Array PartnerPerformanceArray;
            List<Performance> Perfomance = new List<Performance>();
            var bpart = dbObj.BusinessPartner_Performance.ToList();
            var masterPartner = dbObjThomas.MasterBusinessPartners.ToList();
            



            var sql = bpart
                .Join(masterPartner, per => per.partnerId, tb => tb.BusinessPartnerCode, (per, tb) => new { per, tb })
                .Where(c => c.per.remark == type)
                .ToList();

            foreach (var data in sql)
            {

              
                int montr =Convert.ToInt32(data.per.month);
                switch (montr){

                     case 1:
                        months = "January";
                        break;
                     case 2:
                        months = "February";
                        break;
                     case 3:
                        months = "March";
                        break;
                     case 4:
                        months = "April";
                        break;
                     case 5:
                        months = "May";
                        break;
                     case 6:
                        months = "June";
                        break;
                     case 7:
                        months = "July";
                        break;
                     case 8:
                        months = "August";
                        break;
                     case 9:
                        months = "September";
                        break;
                     case 10:
                        months = "October";
                        break;
                     case 11:
                        months = "November";
                        break;
                     case 12:
                        months = "December";
                        break;
                }
                   


                Perfomance.Add(new Performance
                {

                    id = data.per.id,
                    partnerCode = data.per.partnerId,
                    year = data.per.year,
                    month = months,
                    partnerName = data.tb.BusinessPartnerName,
                    dateCreate = data.per.dateCreate,
                    dateUpdate = data.per.dateUpdate,
                    remark = data.per.remark,
                    remark1 = data.per.remark1,
                    userCode = data.per.userCode,
                    usercodeUpdate = data.per.userCodeUpdate,
                    version = data.per.version


                });
            }

            PartnerPerformanceArray = Perfomance.ToArray();
            ViewBag.Performance = PartnerPerformanceArray;

           
        }

        public ActionResult InsertPerformance()
        {

            CustomerGet();
            SupplierGet();

            return View();
        }

        [HttpPost]
        public ActionResult InsertPerformanceQue(string month, string PartnerCode,string Type)
        {
            if (Request.Files.Count > 0)
            {
                try
                {

                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {


                        HttpPostedFileBase file = files[i];
                        string fname, fnameup;


                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                            fnameup = file.FileName;
                        }
                        else
                        {
                            fname = file.FileName;
                            fnameup = file.FileName;
                        }


                        fname = Path.Combine(Server.MapPath("~/File/Performance/"), fname);
                        file.SaveAs(fname);

                        var monthz = month.Split('-');
                        
                            var sql = new BusinessPartner_Performance()
                            {
                                partnerId = Encoding.UTF8.GetString(Convert.FromBase64String(Encoding.UTF8.GetString(Convert.FromBase64String(PartnerCode.ToString())))),
                                year = monthz[0],
                                month = monthz[1],
                                dateCreate = DateTime.Now.ToString(),
                                dateUpdate = DateTime.Now.ToString(),
                                userCode = Session["userCode"].ToString(),
                                userCodeUpdate = Session["userCode"].ToString(),
                                remark = Type,
                                remark1 = fnameup

                            };
                            dbObj.BusinessPartner_Performance.Add(sql);

                            dbObj.SaveChanges();
                        }


                    

                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }



        }
    }

}