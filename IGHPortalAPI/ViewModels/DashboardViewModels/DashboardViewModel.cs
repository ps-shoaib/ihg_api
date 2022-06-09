using System;
using System.Collections.Generic;

namespace IGHportalAPI.ViewModels.DashboardViewModels
{
    public class DashboardViewModel
    {
        public DashboardViewModel()
        {
            FourMonthsWeeksArray = new List<DateTime>();
            LastSevenWeeksTotalPayrollsList = new List<double>();
            LastSevenWeeksMPORList = new List<double>();
        }
        public int TotalRoomsCleaned { get; set; }
        public int TotalRoomsSold { get; set; }
        public int NumberOfEmployees { get; set; }

        public int TotalSundriesShopProductsSales { get; set; }

        public int ExpiredSundriesShopProducts { get; set; }
        public int TotalLinenInventoriesToBOrdered { get; set; }

        public List<DateTime> FourMonthsWeeksArray { get; set; }


        public List<double> LastSevenWeeksTotalPayrollsList { get; set; }
        
        public List<double> LastSevenWeeksMPORList { get; set; }

    }
}

