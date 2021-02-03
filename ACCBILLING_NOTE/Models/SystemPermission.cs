using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BOI_QUO.Models
{
    public class SystemPermission
    {
        public int id { get; set; }
        public string typePermission { get; set; }
        public string remarkPermission { get; set; }
        public string controller { get; set; }
        public string status { get; set; }
    }
}