using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BOI_QUO.Models
{
    public class Performance
    {
        public int id { get; set; }
        public string partnerCode {get; set;}
        public string partnerName { get; set; }
        public string year { get; set; }
        public string month { get; set; }
        public string dateCreate { get; set; }
        public string dateUpdate { get; set; }
        public string version { get; set; }
        public string remark { get; set; }
        public string remark1 { get; set; }
        public string userCode { get; set; }
        public string usercodeUpdate { get; set; }
       
    }
}