using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KH_Capstone.Models
{
    public class ItemPO
    {
        public int ItemID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        
        //[Required]
        public string ImagePath { get; set; }
        
        public bool Purchasable { get; set; }

        public bool Validated { get; set; }
    }
}