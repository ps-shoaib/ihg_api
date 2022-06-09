using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IGHportalAPI.ViewModels.HotelViewModels;
using Microsoft.AspNetCore.Http;

namespace IGHportalAPI.Services
{
    public interface IHotelService
    {
        Task AddHotel(HotelViewModel model);
        Task<List<HotelViewModel>> GetHotelsByUserId(string userId, HttpRequest httpRequest);


        Task<List<HotelViewModel>> GetHotels(HttpRequest httpRequest);
        
        Task UpdateHotel(HotelViewModel model);        
        Task DeleteHotel(int id);
        HotelViewModel GetHotel(int id, HttpRequest httpRequest);
        bool SystemNameAlreadyExist(HotelViewModel model);

        List<HotelNamesViewModel> GetHotelsNames();


    }
}
