using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace KH_Capstone.Models
{
    public class EnemyUpdateVM
    {
        public EnemyPO Enemy { get; set; }

        public List<ItemPO> itemList = new List<ItemPO>();

        public int Item1 { get; set; }

        public int Item2 { get; set; }

        public HttpPostedFileBase File { get; set; }
    }
}