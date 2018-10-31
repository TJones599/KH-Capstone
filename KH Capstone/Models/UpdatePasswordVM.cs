using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KH_Capstone.Models
{
    public class UpdatePasswordVM
    {
        [Required]
        [StringLength(22)]
        [Display (Name = "Old Password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(22)]
        [Display (Name = "New Password")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).{5,22}$", ErrorMessage = "Password must follow the requirements shown!")]
        public string NewPassword { get; set; }

        [Required]
        [StringLength(22)]
        [Display (Name = "Password Confirmation")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).{5,22}$", ErrorMessage = "Password must follow the requirements shown!")]
        public string PasswordConfirmation { get; set; }
    }
}