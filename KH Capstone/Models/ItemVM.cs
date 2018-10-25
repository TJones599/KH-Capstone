using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KH_Capstone.Models
{
    public class ItemVM
    {
        public ItemPO Item { get; set; }
        public HttpPostedFileBase File { get; set; }
    }
}