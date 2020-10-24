using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using OnlineAuction.Models;
using Microsoft.EntityFrameworkCore;

namespace OnlineAuction.Models
{
    public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        private AuctionContext db;

        public CustomCookieAuthenticationEvents(AuctionContext context)
        {
            // Get the database from registered DI services.
            db = context;
        }

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var userPrincipal = context.Principal;

            int lastRole = 2;
            var lRole = (from c in userPrincipal.Claims
                               where c.Type == ClaimsIdentity.DefaultRoleClaimType
                               select c.Value).FirstOrDefault();
            if (lRole == "admin") lastRole = 1;

            var email = (from c in userPrincipal.Claims where c.Type == ClaimsIdentity.DefaultNameClaimType select c.Value).FirstOrDefault();

            User user = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user.RoleId != lastRole)
            {
                context.RejectPrincipal();

                await context.HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }
    }
}
