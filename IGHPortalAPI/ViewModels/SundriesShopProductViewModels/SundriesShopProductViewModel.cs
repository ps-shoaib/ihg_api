using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IGHportalAPI.ViewModels.SundriesShopProductViewModels
{
    public class SundriesShopProductViewModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }


        public int HotelId { get; set; }
    }
}
