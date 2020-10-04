using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAuction.Models
{
    public class IndexViewModel
    {
        public IEnumerable<Item> Items { get; set; }
        public SortViewModel SortViewModel { get; set; }
    }
}
