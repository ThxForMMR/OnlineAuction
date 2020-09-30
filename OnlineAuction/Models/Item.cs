using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAuction.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public int Cost { get; set; }
    }
}
