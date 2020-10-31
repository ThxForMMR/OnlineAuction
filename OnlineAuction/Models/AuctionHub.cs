using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace OnlineAuction.Models
{
    public class AuctionHub : Hub
    {
        public async Task Send(string message, string userName)
        {
            await Clients.All.SendAsync("Receive", message, userName);
        }

        public async Task Update()
        {
            await Clients.All.SendAsync("DbUpdate");
        }
    }
}
