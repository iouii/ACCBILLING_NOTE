using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using ACCBILLING_NOTE.Models;
using Newtonsoft.Json.Linq;

namespace ACCBILLING_NOTE.Controllers
{
    public class SetupController : Controller
    {
        // GET: Setup
        ConnectdataBase con = new ConnectdataBase();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddData()
        {
            return View();
        }
        [HttpPost]
        public JsonResult SearchData(string name)
        {
            string sql = "SELECT Id,BankName_EN,Address_EN,BankName_Thi,Address_Thi,AccOn,Branch,SwiftCode FROM Acc_Bank ";
            con.Opens();
            SqlCommand com = new SqlCommand(sql, con.cons);
            SqlDataReader read = com.ExecuteReader();


            List<ModelBank> modelbank = new List<ModelBank>();
            while (read.Read())
            {
                modelbank.Add(new ModelBank()
                {
                    Id = Convert.ToInt32(read["Id"].ToString()),
                    bankName_En = read["BankName_EN"].ToString(),
                    address_En = read["Address_EN"].ToString(),
                    bankName_Thi = read["BankName_Thi"].ToString(),
                    address_Thi = read["Address_Thi"].ToString(),
                    accOn = read["AccOn"].ToString(),
                    branch = read["Branch"].ToString(),
                    swiftcode = read["SwiftCode"].ToString()

                });
            }
            con.Closes();
            return Json(modelbank, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult AddData(string idtxtBankEn, string idtxtBankTh, string idtxtAen, string idtxtAeth, string idtxtAcc, string idtxtBranch, string idtxtSwit)
        {
            string sql = "";

            sql = "INSERT INTO Acc_Bank VALUES ('" + idtxtBankEn + "','" + idtxtAen + "','" + idtxtBankTh + "','" + idtxtAeth + "','" + idtxtAcc + "','" + idtxtBranch + "','" + idtxtSwit + "')";

            con.Opens();
            SqlCommand com = new SqlCommand(sql, con.cons);
            com.ExecuteNonQuery();
            con.Closes();

            return Json(String.Format("Complete"));
        }

        [HttpPost]
        public ActionResult UpdateData(string jsonstr)
        {
            string sql = "", updateData = "", straleart = "";
            int i = 0, chkif = 0;
            JObject jsonobj = new JObject();
            jsonobj = JObject.Parse(jsonstr);
            var ssd = jsonobj["arrGlobel"][2].ToString().Length;
            //loop check data for into data is json : arrGlobel arrStrData arrNameCol
            for (i = 1; i < 6; i++)
            {

                if (jsonobj["arrGlobel"][i].ToString() != jsonobj["arrStrData"][i].ToString())
                {
                    if (chkif == 0)
                    {
                        updateData = jsonobj["arrNameCol"][i].ToString() + "='" + jsonobj["arrStrData"][i].ToString() + "'";
                    }
                    else
                    {

                        updateData += "," + jsonobj["arrNameCol"][i].ToString() + "='" + jsonobj["arrStrData"][i].ToString() + "'";
                    }
                    chkif += 1;
                }
            }
            if (updateData != "")
            {
                sql = "UPDATE Acc_Bank SET " + updateData + " WHERE Id = " + jsonobj["arrStrData"][0] + " ";
                con.Opens();
                SqlCommand cmd = new SqlCommand(sql, con.cons);
                cmd.ExecuteNonQuery();
                con.Closes();
                straleart = "Update Data Complete";
            }
            else
            {
                straleart = "Data not have change. Please change data...";
            }
            return Json(String.Format(straleart));
        }

        [HttpPost]
        public ActionResult DeleteData(string Id)
        {
            string sql = "";

            sql = "DELETE FROM Acc_Bank WHERE Id =" + Id + "";
            con.Opens();
            SqlCommand cmd = new SqlCommand(sql, con.cons);
            cmd.ExecuteNonQuery();
            con.Closes();

            return Json(String.Format("Delete data complete"));
        }
    }
}