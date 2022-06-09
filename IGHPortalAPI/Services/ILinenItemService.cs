using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IGHportalAPI.ViewModels.LinenItemViewModels;

namespace IGHportalAPI.Services
{
    public interface ILinenItemService
    {
        Task AddLinenItem(LinenItemViewModel model);
        List<LinenItemViewModel> GetLinenItems(int hotelId);
        Task UpdateLinenItem(LinenItemViewModel model);
        
        Task DeleteLinenItem(int id);

        LinenItemViewModel GetLinenItem(int id);

        
        bool SystemNameAlreadyExist(LinenItemViewModel model);

       

    }
}
