using IGHportalAPI.ViewModels.DashboardViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace IGHportalAPI.Services
{
    public interface IDashboardService
    {

        DashboardViewModel GetDashboardData(int hotelId);
       

    }
}
