using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IGHportalAPI.ViewModels.HotelViewModels
{
    public class HotelViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }

        public string ImageURL { get; set; }

        public IFormFile file { get; set; }


        //public string UserId { get; set; }


    }
}
