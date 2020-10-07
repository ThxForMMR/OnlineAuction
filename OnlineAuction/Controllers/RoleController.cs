using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineAuction.Models;
using Microsoft.AspNetCore.Authorization;

namespace OnlineAuction.Controllers
{
    [Authorize(Roles = "admin")]
    public class RoleController : Controller
    {
        AuctionContext db;
        public RoleController(AuctionContext context)
        {
            db = context;
        }
        public async Task<IActionResult> Index(string searchString)
        {
            IQueryable<User> items = db.Users;

            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.Email.Contains(searchString));
            }

            return View(items);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                {
                    db.Users.Remove(user);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
        public async Task<IActionResult> ChangeRole(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                {
                    if (user.RoleId == 2)
                    {
                        user.RoleId = 1;
                        Role userRole = await db.Roles.FirstOrDefaultAsync(r => r.Name == "admin");
                        if (userRole != null)
                            user.Role = userRole;
                    }
                    else
                    {
                        user.RoleId = 2;
                        Role userRole = await db.Roles.FirstOrDefaultAsync(r => r.Name == "user");
                        if (userRole != null)
                            user.Role = userRole;
                    }
                    db.Users.Update(user);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}
