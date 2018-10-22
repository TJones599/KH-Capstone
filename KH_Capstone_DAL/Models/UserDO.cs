using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KH_Capstone_DAL.Models
{
    public class UserDO
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public int Role { get; set; }

        public string RoleName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool Banned { get; set; }

        public bool Inactive { get; set; }
    }
}
