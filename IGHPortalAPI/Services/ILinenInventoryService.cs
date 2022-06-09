using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IGHportalAPI.ViewModels.LinenInventoryViewModels;

namespace IGHportalAPI.Services
{
    public interface ILinenInventoryService
    {
        Task AddLinenInventory(LinenInventoryViewModel model);
        List<LinenInventoryViewModel> GetLinenInventories(int hotelId, string Month, string Year);
        Task UpdateLinenInventory(LinenInventoryViewModel model);
        

        Task DeleteLinenInventory(int id);

        LinenInventoryViewModel GetLinenInventory(int id, int hotelId);

         bool ReportAlreadyExistOfCurrentMonth(LinenInventoryViewModel model);




    }
}
