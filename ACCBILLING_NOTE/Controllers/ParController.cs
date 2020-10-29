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

        public static int  IntE()
        {
            int iy = 50;
            
            
            
            return iy;


        }
       
    }
}