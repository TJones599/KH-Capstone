using System.ComponentModel.DataAnnotations;

namespace KH_Capstone.Models
{
    public class EnemyPO
    {
        public int EnemyID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Description { get; set; }

        public string ImagePath { get; set; }

        public bool Validated { get; set; }
    }
}