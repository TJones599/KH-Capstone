using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KH_Capstone.Models
{
    public class CreateEnemyVM
    {
        public CreateEnemyVM()
        {
            this.Item1 = 0;
            this.Item2 = 0;
            this.Enemy = new EnemyPO();
            this.Enemy.ImagePath = "";
        }

        public EnemyPO Enemy { get; set; }
        public List<ItemPO> ItemList { get; set; }
        public int Item1 { get; set; }
        public int Item2 { get; set; }

        public HttpPostedFileBase File { get; set; }
    }
}