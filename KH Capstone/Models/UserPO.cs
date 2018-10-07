using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KH_Capstone.Models
{
    public class UserPO
    {
        public int UserID { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public int Role { get; set; }
    }
}