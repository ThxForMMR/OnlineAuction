﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineAuction.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace OnlineAuction.Controllers
{
    public class HomeController : Controller
    {        
        AuctionContext db;
        private readonly IEmailSender _emailSender;

        public HomeController(AuctionContext context, IEmailSender emailSender)
        {
            db = context;
            _emailSender = emailSender;
        }
        public async Task<IActionResult> Index(string searchString, SortState sortOrder = SortState.NameAsc)
        {
            IQueryable<Item> items = db.Items;

            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.Name.Contains(searchString));
            }            

            items = sortOrder switch
            {
                SortState.NameDesc => items.OrderByDescending(s => s.Name),
                SortState.CostAsc => items.OrderBy(s => s.Cost),
                SortState.CostDesc => items.OrderByDescending(s => s.Cost),
                SortState.CompanyAsc => items.OrderBy(s => s.Company),
                SortState.CompanyDesc => items.OrderByDescending(s => s.Company),
                _ => items.OrderBy(s => s.Name),
            };

            IndexViewModel viewModel = new IndexViewModel
            {
                Items = await items.AsNoTracking().ToListAsync(),
                SortViewModel = new SortViewModel(sortOrder)
            };
            
            return View(viewModel);
        }    
     
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Item item)
        {
            db.Items.Add(item);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        public IActionResult Key()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Key(ConnectionKey connectionKey)
        {
            if (db.Keys.Count() != 0) foreach (var element in db.Keys) db.Keys.Remove(element);
            db.Keys.Add(connectionKey);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                Item item = await db.Items.FirstOrDefaultAsync(p => p.Id == id);
                if (item != null)
                    return View(item);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Item item)
        {
            db.Items.Update(item);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Delete")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Item item = await db.Items.FirstOrDefaultAsync(p => p.Id == id);
                if (item != null)
                    return View(item);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Item item = await db.Items.FirstOrDefaultAsync(p => p.Id == id);
                if (item != null)
                {
                    db.Items.Remove(item);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }

        [HttpGet]
        [ActionName("Buy")]
        [Authorize(Roles = "admin,user")]
        public async Task<IActionResult> ConfirmBuy(int? id)
        {
            if (id != null)
            {
                Item item = await db.Items.FirstOrDefaultAsync(p => p.Id == id);
                if (item != null)
                    return View(item);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Buy(int? id, int? card)
        {
            if (id != null && card != null)
            {
                Item item = await db.Items.FirstOrDefaultAsync(p => p.Id == id);
                if (item != null)
                {
                    string msg = "Товар \"" + item.Name + "\" успешно куплен, оплачено " + card.ToString() + " con str: " + db.Keys.FirstOrDefault().Value;
                    ViewBag.Message = msg;
                    await _emailSender.SendEmailAsync(db.Keys.FirstOrDefault().Value, new List<string> { User.Identity.Name }, "Покупка", msg);
                    return View("SuccessfulBuy");
                    //return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true }
            ); ;

            return LocalRedirect(returnUrl);
        }
        public IActionResult Chat()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
