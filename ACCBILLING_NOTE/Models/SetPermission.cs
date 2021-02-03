using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BOI_QUO.Models
{
    public class SetPermission
    {
        public int id { get; set; }
        public string userCode { get; set; }
        public string permissionId { get; set; }
        public string permissionName { get; set; }
        public string dateCreate { get; set; }
        public string remarkName { get; set; }
        public string controllerPermissin { get; set; }
      

    }
}