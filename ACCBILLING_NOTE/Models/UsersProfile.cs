using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BOI_QUO.Models
{
    public class UsersProfile
    {
        public int userId { get; set; }
        public string userFname { get; set; }
        public string userLname { get; set; }
        public string userCode { get; set; }
        public string DepartId { get; set; }
        public string position { get; set; }


    }
}