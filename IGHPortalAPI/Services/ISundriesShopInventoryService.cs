using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IGHportalAPI.ViewModels.SundriesShopInventoryViewModels;

namespace IGHportalAPI.Services
{
    public interface ISundriesShopInventoryService
    {
        Task AddSundriesShopInventory(SundriesShopInventoryViewModel model);
        List<SundriesShopInventoryViewModel> GetSundriesShopInventories(int hotelId, string Month, string Year);
        Task UpdateSundriesShopInventory(SundriesShopInventoryViewModel model);
        

        Task DeleteSundriesShopInventory(int id);

        SundriesShopInventoryViewModel GetSundriesShopInventory(int id, int hotelId);

        bool ReportAlreadyExistOfCurrentMonth(SundriesShopInventoryViewModel model);





    }
}
