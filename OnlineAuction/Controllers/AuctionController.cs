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
        private readonly IEmailSender _emailSender;
        public AuctionController(AuctionContext context, IEmailSender emailSender)
        {
            db = context;
            _emailSender = emailSender;
        }
        public async Task<IActionResult> Index()
        {
            IQueryable <Bet> bets = db.Bets;
            if (db.Lots.Count() != 0 && db.Lots.FirstOrDefault().Status == true && db.Lots.FirstOrDefault().endDate < DateTimeOffset.UtcNow)
                return RedirectToAction("EndAuction");

            AuctionIndexViewModel viewModel = new AuctionIndexViewModel
            {
                Bets = await bets.AsNoTracking().ToListAsync(),
                ActualLot = db.Lots.FirstOrDefault()
            };
            viewModel.ActualLot.endDate = viewModel.ActualLot.endDate.ToLocalTime();

            return View(viewModel);
        }
        public async Task<IActionResult> Up()
        {
            if (db.Lots.Count() == 0 || db.Lots.FirstOrDefault().Status == false) return RedirectToAction("Index");
            if (db.Lots.FirstOrDefault().endDate < DateTimeOffset.UtcNow) return RedirectToAction("EndAuction");

            Bet newBet = new Bet();
            newBet.User = User.Identity.Name;

            if (db.Bets.Count() != 0) newBet.betSize = db.Bets.Max(p=>p.betSize) + db.Lots.FirstOrDefault().RateBet;
            else newBet.betSize = db.Lots.FirstOrDefault().StartBet;

            db.Lots.FirstOrDefault().ActualCost = newBet.betSize;
            db.Lots.FirstOrDefault().Owner = User.Identity.Name;
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
        public async Task<IActionResult> Create(Lot lot, int Time)
        {
            if (db.Lots.Count() != 0 && db.Lots.FirstOrDefault().Status == true)
            {
                if (db.Bets.Count() != 0) SendMsg();
            }
            foreach (var element in db.Bets) db.Bets.Remove(element);
            foreach (var element in db.Lots) db.Lots.Remove(element);
            lot.Status = true;
            lot.endDate = DateTimeOffset.UtcNow;
            lot.endDate = lot.endDate.AddMinutes(Time);
            db.Lots.Add(lot);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EndAuction()
        {
            if (db.Lots.Count() == 0 || db.Lots.FirstOrDefault().Status == false) return RedirectToAction("Index");
            db.Lots.FirstOrDefault().Status = false;
            if (db.Bets.Count() != 0) SendMsg();
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private async void SendMsg()
        {
            string msg = "Товар \"" + db.Lots.FirstOrDefault().Name + "\" успешно куплен на аукционе за " + db.Lots.FirstOrDefault().ActualCost;
            await _emailSender.SendEmailAsync(db.Keys.FirstOrDefault().Value, new List<string> { db.Lots.FirstOrDefault().Owner }, "Аукцион", msg);
        }
    }
}
