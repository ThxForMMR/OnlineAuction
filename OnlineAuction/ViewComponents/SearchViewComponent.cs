using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace OnlineAuction.ViewComponents
{
    public class SearchViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = new HttpClient();
            var responce = client.GetAsync("https://localhost:44381/api/get");
            var searchString = await responce.Result.Content.ReadAsStringAsync();
            var test = new HubConnectionBuilder();
            return Content(searchString);
            
        }
    }
}
