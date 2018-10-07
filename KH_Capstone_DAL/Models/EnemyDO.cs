using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KH_Capstone_DAL.Models
{
    public class EnemyDO
    {
        public int EnemyID { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public bool Validated { get; set; }
    }
}
