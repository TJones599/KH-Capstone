using System.ComponentModel.DataAnnotations;

namespace KH_Capstone.Models
{
    public class RegisterVM
    {
        [Display (Name ="First Name")]
        public string FirstName { get; set; }

        [Display (Name ="Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [Display (Name ="Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "New Password is required")]
        [StringLength(22, MinimumLength =5)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).{5,22}$", ErrorMessage ="Password must follow the requirements shown!")]
        [Display (Name ="New Password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Password confirmation is required")]
        [StringLength(22, MinimumLength = 5)]
        [Compare("NewPassword")]
        [Display (Name ="Password Confirmation")]
        public string PasswordConfirmation { get; set; }

    }
}