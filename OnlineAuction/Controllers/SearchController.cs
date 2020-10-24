using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineAuction.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        string seachString;
        public SearchController()
        {
            seachString = "";
        }
        [HttpGet]
        public string Get()
        {
            return seachString;
        }
        [HttpPost]
        public void Post(string SearchString)
        {
            seachString = SearchString;
        }
    }
}
