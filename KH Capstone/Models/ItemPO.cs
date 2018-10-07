using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KH_Capstone.Models
{
    public class ItemPO
    {
        public int ItemID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public bool Purchasable { get; set; }

        public bool Validated { get; set; }
    }
}