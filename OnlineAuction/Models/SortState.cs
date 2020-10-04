using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAuction.Models
{
    public enum SortState
    {
        NameAsc,    // по имени по возрастанию
        NameDesc,   // по имени по убыванию
        CompanyAsc, // по компании по возрастанию
        CompanyDesc, // по компании по убыванию
        CostAsc, // по цене по возрастанию
        CostDesc    // по цене по убыванию        
    }
}
