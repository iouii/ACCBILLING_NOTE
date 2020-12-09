using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;

namespace ACCBILLING_NOTE.Controllers
{
    public class SaveDataController : Controller
    {
        // GET: SaveData
        ConnectdataBase con = new ConnectdataBase();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SaveData(string jsonData)
        {
            JArray json = new JArray();
            int couRow = 0, i = 0;
            string sql = "";

            json = JArray.Parse(jsonData);
            couRow = json.Count;

            con.Opens();
            for (i = 0; i < couRow; i++)
            {
                sql = "INSERT INTO Acc_BillingDetail VALUES('" + json[i]["dateprints"].ToString().Trim() + "','" + json[i]["ondoc"].ToString().Trim() + "','" + json[i]["OnInvDetail"].ToString().Trim() + "','" + json[i]["CodeCustomer"].ToString().Trim() + "','" + json[i]["CustomerName"].ToString().Trim() + "','" + Convert.ToDateTime(json[i]["Inv_date"]).ToString("dd-MM-yyyy").Trim() + "','" + json[i]["duedate"].ToString().Trim() + "','" + json[i]["Amount"].ToString().Trim() + "','" + json[i]["Vat"].ToString().Trim() + "','" + json[i]["Total"].ToString().Trim() + "','" + json[i]["idBank"].ToString().Trim() + "')";
                SqlCommand cmd = new SqlCommand(sql, con.cons);
                cmd.ExecuteNonQuery();
            }

            con.Closes();
            return Json(String.Format("Save data success"));
        }
    }
}