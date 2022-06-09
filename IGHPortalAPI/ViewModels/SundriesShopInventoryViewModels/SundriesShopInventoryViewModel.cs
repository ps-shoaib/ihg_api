using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IGHportalAPI.ViewModels.SundriesShopInventoryViewModels
{
    public class SundriesShopInventoryViewModel
    {
        public SundriesShopInventoryViewModel()
        {
            SundriesShopInventoryDetails = new HashSet<SundriesShopInventoryDetailsViewModel>();
        }
        public int Id { get; set; }
        public int HotelId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ReportMonthYear { get; set; }


        public virtual ICollection<SundriesShopInventoryDetailsViewModel> SundriesShopInventoryDetails { get; set; }

    }
}
