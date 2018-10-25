using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KH_Capstone.Models
{
    public class EnemyDropsVM
    {
        public int enemyID { get; set; }
        public List<ItemPO> Items = new List<ItemPO>();
    }
}