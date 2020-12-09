using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCBILLING_NOTE.Models
{
    public class Invoice
    {

       
            public string invoiceId { get; set; }
            public string invoiceNo { get; set; }
            public string invoiceDate { get; set; }
            public string invoiceDue { get; set; }
            public double invoiceAm1 { get; set; }
            public double invoiceVat { get; set; }
            public double invoiceAm2 { get; set; }
            public string invoiceAm1Cal { get; set; }
            public string invoiceAm2Cal { get; set; }
            public string invoiceVatCal { get; set; }
            public string invoicebldate { get; set; }
            public string invoicenocom { get; set; }
            public SumData datas { get; set; }

    }
    public class SumData {

        public double data { get; set; }

    }
}