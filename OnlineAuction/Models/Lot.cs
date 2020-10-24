using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAuction.Models
{
    public class Lot
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public int StartBet { get; set; }
        public int RateBet { get; set; }
        public int ActualCost { get; set; }
        public User Owner { get; set; }
        public bool Status { get; set; }
    }
}
