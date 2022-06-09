using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IGHportalAPI.ViewModels.LinenInventoryViewModels
{
    public class LinenInventoryViewModel
    {
        public LinenInventoryViewModel()
        {
            LinenInventoryDetails = new HashSet<LinenInventoryDetailsViewModel>();
        }

        public int Id { get; set; }


        public int HotelId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ReportMonthYear { get; set; }


      



        public virtual ICollection<LinenInventoryDetailsViewModel> LinenInventoryDetails { get; set; }

    }
}
