using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KH_Capstone.Models
{
    public class EnemyUpdateVM
    {
        public EnemyPO Enemy { get; set; }

        //public int EnemyID { get; set; }

        //[Required]
        //public string Name { get; set; }

        //[Required]
        //public string Location { get; set; }

        //[Required]
        //public string Description { get; set; }

        //public string ImagePath { get; set; }

        //public bool Validated { get; set; }

        public List<ItemPO> itemList = new List<ItemPO>();

        public int Item1 { get; set; }
        public int Item2 { get; set; }
    }
}