using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAuction.Models
{
    public class Bet
    {
        public int Id { get; set; }
        public string User { get; set; }
        public int betSize { get; set; }
    }
}
