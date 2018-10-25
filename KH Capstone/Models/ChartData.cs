using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KH_Capstone.Models
{
    public class ChartData<T, T2>
    {
        public List<T> Labels { get; set; }
        public List<T2> Values { get; set; }
    }
}