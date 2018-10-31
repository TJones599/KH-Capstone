using System.ComponentModel.DataAnnotations;

namespace KH_Capstone.Models
{
    public class UserPO
    {
        public int UserID { get; set; }

        [Required]
        public string UserName { get; set; }
        
        public byte[] Password { get; set; }

        public string Salt { get; set; }

        public int Role { get; set; }

        public string RoleName { get; set; }

        [Display (Name = "First Name")]
        public string FirstName { get; set; }

        [Display (Name = "Last Name")]
        public string LastName { get; set; }

        public bool Banned { get; set; }

        public bool Inactive { get; set; }
    }
}