using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAuction.Models
{
    public class SortViewModel
    {
        public SortState NameSort { get; set; } // значение для сортировки по имени
        public SortState CostSort { get; set; }    // значение для сортировки по цене
        public SortState CompanySort { get; set; }   // значение для сортировки по компании
        public SortState Current { get; set; }     // значение свойства, выбранного для сортировки
        public bool Up { get; set; }  // Сортировка по возрастанию или убыванию

        public SortViewModel(SortState sortOrder)
        {
            // значения по умолчанию
            NameSort = SortState.NameAsc;
            CostSort = SortState.CostAsc;
            CompanySort = SortState.CompanyAsc;
            Up = true;

            if (sortOrder == SortState.CostDesc || sortOrder == SortState.NameDesc
                || sortOrder == SortState.CompanyDesc)
            {
                Up = false;
            }

            switch (sortOrder)
            {
                case SortState.NameDesc:
                    Current = NameSort = SortState.NameAsc;
                    break;
                case SortState.CostAsc:
                    Current = CostSort = SortState.CostDesc;
                    break;
                case SortState.CostDesc:
                    Current = CostSort = SortState.CostAsc;
                    break;
                case SortState.CompanyAsc:
                    Current = CompanySort = SortState.CompanyDesc;
                    break;
                case SortState.CompanyDesc:
                    Current = CompanySort = SortState.CompanyAsc;
                    break;
                default:
                    Current = NameSort = SortState.NameDesc;
                    break;
            }
        }
    }
}
