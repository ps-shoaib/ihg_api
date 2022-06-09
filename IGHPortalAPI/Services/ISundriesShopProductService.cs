using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IGHportalAPI.ViewModels.SundriesShopProductViewModels;


namespace IGHportalAPI.Services
{
    public interface ISundriesShopProductService
    {
        Task AddSundriesShopProduct(SundriesShopProductViewModel model);
        List<SundriesShopProductViewModel> GetSundriesShopProducts(int hotelId);
        Task UpdateSundriesShopProduct(SundriesShopProductViewModel model);
        
        Task DeleteSundriesShopProduct(int id);

        SundriesShopProductViewModel GetSundriesShopProduct(int id);

        
        bool SystemNameAlreadyExist(SundriesShopProductViewModel model);

       

    }
}
