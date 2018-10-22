using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KH_Capstone.Models
{
    public class ItemDropsVM
    {
        public List<EnemyPO> enemies = new List<EnemyPO>();

        public ItemPO item { get; set; }
    }
}