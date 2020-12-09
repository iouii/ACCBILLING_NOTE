using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using ACCBILLING_NOTE.Models;
using Newtonsoft.Json;

namespace ACCBILLING_NOTE.NumberAuto
{
    public class FilterSaveData
    {
        public static string _getData(DataTable dtH, DataTable dtD)
        {
            string jsondata = "";
            int countRowD = 0, i = 0;

            countRowD = dtD.Rows.Count;

            List<ModelFilterData> modelFiter = new List<ModelFilterData>();
            for (i = 0; i < countRowD; i++)
            {
                modelFiter.Add(new ModelFilterData()
                {

                    OnInvDetail = dtD.Rows[i]["IDINVC"].ToString().Trim(),
                    CodeCustomer = dtH.Rows[0]["IDCUST"].ToString().Trim(),
                    CustomerName = dtH.Rows[0]["NAMECUST"].ToString().Trim(),
                    Inv_date = dtD.Rows[i]["DATEINVC"].ToString(),//.Substring(6).Trim() + "-" + dtD.Rows[i]["DATEINVC"].ToString().Substring(4, 2).Trim() + "-" + dtD.Rows[i]["DATEINVC"].ToString().Substring(0, 4).Trim(),
                    Amount = dtD.Rows[i]["BASETAX1"].ToString().Trim(),
                    Vat = dtD.Rows[i]["AMTTAX1"].ToString().Trim(),
                    Total = dtD.Rows[i]["AMTPYMSCHD"].ToString().Trim()

                });
            }

            jsondata = JsonConvert.SerializeObject(modelFiter);

            return jsondata;
        }
    }
}