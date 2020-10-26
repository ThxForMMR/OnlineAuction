using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAuction.Models
{
    public class AuctionIndexViewModel
    {
        public IEnumerable<Bet> Bets { get; set; }
        public Lot ActualLot { get; set; }
    }
}
