using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCBILLING_NOTE.Models
{
    public class ModelBank
    {
        public int Id { get; set; }
        public string bankName_En { get; set; }
        public string address_En { get; set; }
        public string bankName_Thi { get; set; }
        public string address_Thi { get; set; }
        public string accOn { get; set; }
        public string branch { get; set; }
        public string swiftcode { get; set; }
    }
}