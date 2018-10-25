using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KH_Capstone.Models
{
    public class UserPO
    {
        public int UserID { get; set; }

        [Required]
        public string UserName { get; set; }
        
        public byte[] Password { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string PasswordConfirmation { get; set; }

        public string Salt { get; set; }

        public int Role { get; set; }

        public string RoleName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool Banned { get; set; }

        public bool Inactive { get; set; }
    }
}