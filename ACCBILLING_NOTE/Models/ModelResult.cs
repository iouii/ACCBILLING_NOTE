using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCBILLING_NOTE.Models
{
    public class ModelResult
    {
        public string date { get; set; }
        public string duedate { get; set; }
        public string Oninv { get; set; }
        public string Codecus { get; set; }
        public string CusName { get; set; }
        public string Amount { get; set; }
        public string Vat { get; set; }
        public string Total { get; set; }
    }
}