using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KH_Capstone.Models
{
    public class UserUpdateVM
    {
        public UserPO User { get; set; }

        public List<RolePO> Roles { get; set; }
    }
}