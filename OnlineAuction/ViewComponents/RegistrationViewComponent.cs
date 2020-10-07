using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OnlineAuction.ViewComponents
{
    public class RegistrationViewComponent : ViewComponent
    { 
        public IViewComponentResult Invoke()
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return View("UnregTopBar");
            }
            if (User.IsInRole("admin") == true)
            {
                return View("AdminTopBar");
            }
            return View("UserTopBar");
        }
    }
}
