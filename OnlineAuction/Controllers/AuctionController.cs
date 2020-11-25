using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineAuction.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace OnlineAuction.Controllers
{
    public class AuctionController : Controller
    {
        AuctionContext db;
        public AuctionController(AuctionContext context)
        {
            db = context;
        }
        public async Task<IActionResult> Index()
        {
            IQueryable <Bet> bets = db.Bets;
            AuctionIndexViewModel viewModel = new AuctionIndexViewModel
            {
                Bets = await bets.AsNoTracking().ToListAsync(),
                ActualLot = db.Lots.FirstOrDefault()
            };

            return View(viewModel);
        }
        public async Task<IActionResult> Up()
        {
            if (db.Lots.Count() == 0 || db.Lots.FirstOrDefault().Status == false) return RedirectToAction("Index");

            Bet newBet = new Bet();
            newBet.User = User.Identity.Name;

            if (db.Bets.Count() != 0) newBet.betSize = db.Bets.Max(p=>p.betSize) + db.Lots.FirstOrDefault().RateBet;
            else newBet.betSize = db.Lots.FirstOrDefault().StartBet;

            db.Lots.FirstOrDefault().ActualCost = newBet.betSize;
            db.Bets.Add(newBet);

            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Lot lot)
        {
            foreach (var element in db.Bets) db.Bets.Remove(element);
            foreach (var element in db.Lots) db.Lots.Remove(element);
            lot.Status = true;
            db.Lots.Add(lot);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EndAuction()
        {
            if (db.Lots.Count() == 0 || db.Lots.FirstOrDefault().Status == false) return RedirectToAction("Index");
            db.Lots.FirstOrDefault().Status = false;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
