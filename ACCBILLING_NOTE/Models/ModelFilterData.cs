using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCBILLING_NOTE.Models
{
    public class ModelFilterData
    {
        public string OnInvDetail { get; set; }
        public string CodeCustomer { get; set; }
        public string CustomerName { get; set; }
        public string Inv_date { get; set; }
        public string Amount { get; set; }
        public string Vat { get; set; }
        public string Total { get; set; }
    }
}